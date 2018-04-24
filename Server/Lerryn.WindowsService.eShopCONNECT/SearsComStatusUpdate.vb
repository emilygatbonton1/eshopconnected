' eShopCONNECT for Connected Business - Windows Service
' Module: SearsComStatusUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 02 August 2013

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Xml.Linq
Imports System.Xml.XPath

Module SearsComStatusUpdate

    Private XMLStatusUpdateFile As XDocument
    Private xmlStatusUpdateNode As XElement
    Public bSearsComStatusUpdatesToSend As Boolean
    Private iSearsComXMLMessageType As Integer

    Public Sub DoSearsComOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveSearsComSettings As SearsComSettings, ByRef RowID As Integer, ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ' 14/02/12 | TJS             | 2011.2.05 | Completed shipment update
        ' 28/07/13 | TJS             | 2013.1.31 | Corrected shipment XML when ISItemIDField is ItemName
        ' 28/07/13 | TJS             | 2013.1.31 | Tidied progress log message text
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strInvoiceCode As String, strItemXML As String, strSQLCondition As String, strInvoiceNumber As String ' TJS 14/02/12
        Dim strSourceCarrierCode As String, strSourceCarrierClass As String, strSourceCarrierClassCode As String ' TJS 14/02/12
        Dim strSouceDocumentCodes As String()(), strTrackingNumbersAndQuantity As String()() ' TJS 14/02/12
        Dim iCheckLoop As Integer, iEntryRowID As Integer

        Try
            iEntryRowID = RowID
            ' for some reason, POCode field seems to cause constraint error if populated so disable constraints to allow sales order to load
            ImportExportStatusDataset.EnforceConstraints = False
            ' get order record
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrder.TableName, _
               "ReadCustomerSalesOrder", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.CustomerSalesOrder.Count > 0 Then
                Select Case ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221
                    Case "100" ' New Order
                        ' no action to take on new orders
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                        ' check for any new item records and mark them as complete
                        For iCheckLoop = iEntryRowID + 1 To ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.Count - 1
                            If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).SalesOrderCode_DEV000221 = SalesOrderCode Then
                                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).OrderStatus_DEV000221 = "105" Then
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True
                                End If
                            Else
                                RowID = iCheckLoop - 1
                                Exit For
                            End If
                            If bShutDownInProgress Then ' TJS 02/08/13
                                Exit For ' TJS 02/08/13
                            End If
                        Next
                        ' did check loop exit without finding a new row for a different item ?
                        If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then
                            ' yes, set row pointer return value
                            RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                        End If


                    Case "105" ' New Item
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "200" ' Back Order
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "205" ' New Back Order Item
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "305" ' Item Order Qty changed
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "310" ' Item Order Price changed
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "350" ' Item Deleted
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "500" ' Order Voided
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "600" ' Order Shipped
                        ' do we have a Sears.com purchase order number (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then
                                ' yes, send status - get invoice detail (all rows) as order may not have QuantityShipped set
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerInvoiceDetail.TableName, _
                                   "ReadCustomerInvoiceDetailImportExport_DEV000221", AT_SOURCE_INVOICE_CODE, SalesOrderCode}}, _
                                   Interprise.Framework.Base.Shared.ClearType.Specific)

                                ' start of code enabled/added/updated TJS 14/02/12
                                strItemXML = ""
                                strInvoiceCode = ""
                                strSourceCarrierCode = ""
                                strSourceCarrierClass = ""
                                strSourceCarrierClassCode = ""
                                If ActiveSource.EnableDeliveryMethodTranslation Then
                                    ImportExportStatusFacade.TranslateDeliveryMethodFromIS(ActiveSource.SourceCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodGroup, strSourceCarrierCode, strSourceCarrierClass, strSourceCarrierClassCode)
                                Else
                                    strSourceCarrierCode = ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode
                                End If
                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1
                                    strInvoiceCode = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).InvoiceCode
                                    ' have we fully shipped Order Item ?
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityOrdered = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped Then
                                        ' yes, get all source order/invoice codes for this item
                                        strSouceDocumentCodes = ImportExportStatusFacade.GetRows(New String() {"InvoiceCode", "RootDocumentCode", "SourceInvoiceCode"}, "CustomerInvoiceDetailImportExportSourceView_DEV000221", _
                                            "SourcePurchaseID_DEV000221 = '" & ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).SourcePurchaseID_DEV000221 & _
                                            "' AND StoreMerchantID_DEV000221 = '" & ActiveSearsComSettings.SiteID & "' and SourceCode = '" & ActiveSource.SourceCode & _
                                            "' AND MerchantOrderID_DEV000221 = '" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 & "'")
                                        strSQLCondition = ""
                                        For Each SouceDocumentCode As String() In strSouceDocumentCodes
                                            If strSQLCondition <> "" Then
                                                strSQLCondition = strSQLCondition & " OR "
                                            End If
                                            strSQLCondition = strSQLCondition & "SourceDocument = '" & SouceDocumentCode(0) & "'"
                                            If SouceDocumentCode(2) <> "" Then
                                                strSQLCondition = strSQLCondition & " OR SourceDocument = '" & SouceDocumentCode(2) & "'"
                                            End If
                                        Next
                                        ' get tracking numbers for shipment
                                        strTrackingNumbersAndQuantity = ImportExportStatusFacade.GetRows(New String() {"TrackingNumber", "ShipQuantity"}, "ShipmentDetail", "ItemCode = '" & _
                                            ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).ItemCode & "' AND LineNum = " & _
                                            ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).LineNum & " AND (" & strSQLCondition & ") ORDER BY DateCreated")
                                        For Each TrackingNumberAndQuantity As String() In strTrackingNumbersAndQuantity
                                            If "" & TrackingNumberAndQuantity(0) <> "" And ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0 And _
                                                Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then
                                                strItemXML = strItemXML & " <detail><tracking-number>" & TrackingNumberAndQuantity(0) & "</tracking-number>"
                                                strItemXML = strItemXML & "<ship-date>" & ImportExportStatusFacade.GetField("replace(convert(varchar, ShippingDate, 111), '/', '-')", _
                                                    "Shipment", "TrackingNumber = '" & TrackingNumberAndQuantity(0) & "'") & "</ship-date>"
                                                strItemXML = strItemXML & "<shipping-carrier>" & strSourceCarrierCode & "</shipping-carrier><shipping-method>" & strSourceCarrierClass & "</shipping-method>"
                                                If ActiveSearsComSettings.ISItemIDField = "ItemName" Then
                                                    strItemXML = strItemXML & "<package-detail><line-number>" & ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).SourcePurchaseID_DEV000221
                                                    strItemXML = strItemXML & "</line-number><item-id>" & ImportExportStatusFacade.GetField("ItemName", "InventoryItem", "ItemCode = '" & _
                                                        ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).ItemCode & "'") & "</item-id><quantity>" & TrackingNumberAndQuantity(1) & _
                                                        "</quantity></package-detail></detail>" ' TJS 28/07/13
                                                Else
                                                    strItemXML = strItemXML & "<package-detail><line-number>" & ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).SourcePurchaseID_DEV000221
                                                    strItemXML = strItemXML & "</line-number><item-id>" & ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).ItemCode
                                                    strItemXML = strItemXML & "</item-id><quantity>" & TrackingNumberAndQuantity(1) & "</quantity></package-detail></detail>"
                                                End If
                                            End If
                                        Next
                                    End If
                                Next
                                If strItemXML <> "" Then
                                    ' get numeric part of invoice code to use in Sears ASN
                                    strInvoiceNumber = ""
                                    For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail(0).InvoiceCode.Length - 1
                                        If Asc(ImportExportStatusDataset.CustomerInvoiceDetail(0).InvoiceCode.Substring(iCheckLoop, 1)) >= Asc("0") And _
                                            Asc(ImportExportStatusDataset.CustomerInvoiceDetail(0).InvoiceCode.Substring(iCheckLoop, 1)) <= Asc("9") Then
                                            strInvoiceNumber = strInvoiceNumber & ImportExportStatusDataset.CustomerInvoiceDetail(0).InvoiceCode.Substring(iCheckLoop, 1)
                                        End If
                                    Next
                                    strItemXML = "<shipment><header><asn-number>" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 & _
                                        strInvoiceNumber & "</asn-number><po-number>" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 & _
                                        "</po-number><po-date>" & Format(ImportExportStatusDataset.CustomerSalesOrder(0).SalesOrderDate, "yyyy-MM-dd") & "</po-date></header>" & _
                                        strItemXML & "</shipment>"
                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = strItemXML
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 2 ' OrderShipped
                                    ' set message type and mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True

                                Else
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                End If

                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If


                    Case Else
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                End Select

                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Sears.com Order Status " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                       " - No XML created.") ' TJS 28/07/13
                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Sears.com Order Status " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                        " - XML created " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221) ' TJS 28/07/13
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoSearsComOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoSearsComOrderStatusUpdate", ex)

        End Try

    End Sub

    Public Sub StartSearsComStatusFile(ByVal ActiveSource As SourceSettings, ByVal ActiveSearsComSettings As SearsComSettings, _
        ByVal SearsComMessageType As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ' 14/02/12 | TJS             | 2011.2.05 | Completed shipment update
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSubmit As String

        iSearsComXMLMessageType = SearsComMessageType
        Try
            Select Case iSearsComXMLMessageType
                Case 1 ' reserverd for sending IS Order ID
                    strSubmit = ""
                    Return

                Case 2 ' OrderShipped
                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><shipment-feed xmlns=""http://seller.marketplace.sears.com/oms/v3"" "
                    strSubmit = strSubmit & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                    strSubmit = strSubmit & "xsi:schemaLocation=""http://seller.marketplace.sears.com/SellerPortal/s/oms/asn-v3.xsd "">"
                    strSubmit = strSubmit & "<deprecated>2010-12-31</deprecated></shipment-feed>" ' TJS 14/02/12

                Case Else
                    strSubmit = ""
                    Return

            End Select
            XMLStatusUpdateFile = XDocument.Parse(strSubmit)

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartSearsComStatusFile", ex)

        End Try

    End Sub

    Public Sub AddToSearsComStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveSearsComSettings As SearsComSettings, ByRef RowID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStartingUpdateXML As String, iInsertPosn As Integer

        Try
            If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null Then
                ' get current update XML
                strStartingUpdateXML = XMLStatusUpdateFile.ToString
                Select Case iSearsComXMLMessageType
                    Case 1 ' reserverd for sending IS Order ID
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "AddToSearsComStatusFile", "Invalid Sears.com XML Message Type " & iSearsComXMLMessageType, "")
                        Return


                    Case 2 ' OrderShipped
                        ' get position for insert
                        iInsertPosn = InStr(strStartingUpdateXML, "</shipment-feed>")
                        ' insert XML from action record
                        strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & Mid(strStartingUpdateXML, iInsertPosn)


                    Case Else
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "AddToSearsComStatusFile", "Unknown Sears.com XML Message Type " & iSearsComXMLMessageType, "")
                        Return

                End Select
                ' reload update XML
                XMLStatusUpdateFile = XDocument.Parse(strStartingUpdateXML)
            End If

            ' mark action record as XML Sent (change is committed to DB when status file successfully posted to Sears.com in SendSearsComStatusFile)
            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            bSearsComStatusUpdatesToSend = True

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddToSearsComStatusFile", ex)

        End Try

    End Sub

    Public Sub SendSearsComStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveSearsComSettings As SearsComSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ' 14/02/12 | TJS             | 2011.2.05 | Completed shipment update
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument
        Dim strReturn As String = "", strUpdateURL As String, bInhibitWebPosts As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")

            Select Case iSearsComXMLMessageType
                Case 1 ' reserverd for sending IS Order ID
                    strUpdateURL = ""
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Invalid Sears.com XML Message Type " & iSearsComXMLMessageType, "")

                Case 2 ' OrderShipped
                    strUpdateURL = SEARSDOTCOM_SHIPPING_NOTIFICATION_URL & "?email=" & ActiveSearsComSettings.APIUser & "&password=" & ActiveSearsComSettings.APIPwd

                Case Else
                    strUpdateURL = ""
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Unknown Sears.com XML Message Type " & iSearsComXMLMessageType, "")
                    Return

            End Select

            Try
                If Not bInhibitWebPosts Then
                    WebSubmit = System.Net.WebRequest.Create(strUpdateURL)
                    WebSubmit.Method = "PUT"
                    WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                    WebSubmit.ContentLength = XMLStatusUpdateFile.ToString.Length
                    WebSubmit.Timeout = 30000

                    byteData = UTF8Encoding.UTF8.GetBytes(XMLStatusUpdateFile.ToString)

                    ' send to LerrynSecure.com (or the URL defined in the Registry)
                    postStream = WebSubmit.GetRequestStream()
                    postStream.Write(byteData, 0, byteData.Length)

                    WebResponse = WebSubmit.GetResponse
                    reader = New StreamReader(WebResponse.GetResponseStream())
                    strReturn = reader.ReadToEnd()

                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Sears.com Send Status File - Inhibited from sending Status update file - content " & XMLStatusUpdateFile.ToString)
                    Select Case iSearsComXMLMessageType
                        Case 1 ' reserverd for sending IS Order ID

                        Case 2 ' OrderShipped
                            strReturn = "<?xml version=""1.0"" encoding=""utf-8""?>"

                    End Select
                End If

            Catch ex As Exception
                ' send error details
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Unable to post Sears.com XML to " & strUpdateURL & " - " & ex.Message & ", " & ex.StackTrace, XMLStatusUpdateFile.ToString)
                Return

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try

            If strReturn <> "" Then
                If strReturn.Length > 38 Then
                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                        XMLResponse = XDocument.Parse(strReturn)
                        Select Case iSearsComXMLMessageType
                            Case 1 ' reserverd for sending IS Order ID

                            Case 2 ' OrderShipped
                                ' check response status
                                If ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status").ToLower = "success" Then
                                    If ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "ResultData").ToLower = "true" Then
                                        ' yes - status update successful, save 
                                        If Not bInhibitWebPosts Then
                                            m_ImportExportConfigFacade.WriteLogProgressRecord("Sears.com OrderShipped Update Successful for Site " & ActiveSearsComSettings.SiteID & ", response content " & XMLResponse.ToString)
                                        End If
                                        ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                                            "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", "DeleteLerrynImportExportActionStatus_DEV000221"}}, _
                                            Interprise.Framework.Base.Shared.TransactionType.None, "Sears.com OrderShipped", False)

                                    Else
                                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Sears.com OrderShipped failed to update " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status") & " for Site " & ActiveSearsComSettings.SiteID & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "ResultData"), XMLResponse.ToString)
                                    End If
                                    bSearsComStatusUpdatesToSend = False
                                Else
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString & ") for Site " & ActiveSearsComSettings.SiteID & ", Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status") & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Message"), XMLResponse.ToString)
                                End If

                        End Select

                    Else
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                    End If

                Else
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                End If

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendSearsComStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendSearsComStatusFile", ex)

        End Try

    End Sub

End Module
