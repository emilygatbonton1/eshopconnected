' eShopCONNECT for Connected Business - Windows Service
' Module: ASPStoreFrontStatusUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 02 April 2014

Imports System.IO
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module ASPStoreFrontStatusUpdate

    Private XMLStatusUpdateFile As XDocument
    Private xmlStatusUpdateNode As XElement
    Public bASPStoreFrontStatusUpdatesToSend As Boolean
    Private iASPStoreFrontXMLMessageType As Integer

    Public Sub DoASPStoreFrontOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ByRef RowID As Integer, _
     ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 27/09/10 | TJS             | 2010.1.02 | Modified to pick up tracking number from Shipping Module
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Shipments also being done from Invoice
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to mark sales order not found records as complete
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strXML As String, strTrackingNumber As String, strInvoiceCode As String ' TJS 27/09/10 TJS 09/07/11
        Dim iCheckLoop As Integer, iEntryRowID As Integer, bAllItemsDelivered As Boolean
        Dim bSomeItemsDelivered As Boolean, dteActionTimestamp As Date

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
                        ' do we have an ASPDotNetStorefront Order Number (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then
                                ' yes, send status
                                ' reset IsNew flag
                                dteActionTimestamp = ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionTimestamp_DEV000221
                                strXML = "<Set Table=""Orders"" ID=""" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 & """><IsNew>0</IsNew></Set>"
                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = strXML
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 1 ' 1 is OrderDownloaded

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
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
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
                        ' do we have aa ASPDotNetStoreFront Order Number (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then
                                ' yes, send status - get invoice detail (all rows) as order may not have QuantityShipped set
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerInvoiceDetail.TableName, _
                                   "ReadCustomerInvoiceDetailImportExport_DEV000221", AT_SOURCE_INVOICE_CODE, SalesOrderCode}}, _
                                   Interprise.Framework.Base.Shared.ClearType.Specific)

                                ' have all items been delivered ?
                                bAllItemsDelivered = True
                                bSomeItemsDelivered = False
                                strInvoiceCode = "" ' TJS 09/07/11
                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1
                                    strInvoiceCode = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).InvoiceCode ' TJS 09/07/11
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0 Then
                                        bSomeItemsDelivered = True
                                    End If
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityOrdered <> ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped Then
                                        bAllItemsDelivered = False
                                    End If
                                Next

                                If bAllItemsDelivered Then
                                    ' all items delivered, send Order Shipped message
                                    dteActionTimestamp = ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionTimestamp_DEV000221
                                    strXML = "<Set Table=""Orders"" ID=""100000""><ShippedOn>" & Format(dteActionTimestamp, "MM/DD/YYYY hh:mm:ss tt") & "</ShippedOn>"
                                    If ImportExportStatusFacade.GetField("ColumnName", "DataDictionaryColumn", "TableName = 'Shipment' AND ColumnName = 'TrackingNumber'") <> "" Then ' TJS 27/09/10
                                        strTrackingNumber = ImportExportStatusFacade.GetField("TrackingNumber", "Shipment", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & strInvoiceCode & "') ORDER BY ShippingDate DESC") ' TJS 27/09/10 TJS 09/07/11
                                        If "" & strTrackingNumber <> "" Then ' TJS 27/09/10
                                            strXML = strXML & "<ShippedVia>??</ShippedVia><ShippingTrackingNumber>" & strTrackingNumber & "</ShippingTrackingNumber></Set>"
                                        Else
                                            strXML = strXML & "<ShippedVia>??</ShippedVia><ShippingTrackingNumber></ShippingTrackingNumber></Set>"
                                        End If
                                    Else
                                        strXML = strXML & "<ShippedVia>??</ShippedVia><ShippingTrackingNumber></ShippingTrackingNumber></Set>"
                                    End If
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 2 ' 2 is OrderShipped

                                ElseIf bSomeItemsDelivered Then
                                    ' partial delivery only, send Submit Order Shipment List 
                                    strXML = ""

                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is ?? 
                                Else
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                    strXML = "" '
                                End If
                                If bSomeItemsDelivered Then
                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = strXML
                                    ' set message type and mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
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
                    m_ImportExportConfigFacade.WriteLogProgressRecord(SalesOrderCode & " Order Status " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                       " - No XML created.")
                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord(SalesOrderCode & " Order Status " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                        " - XML created " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221)
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoASPStoreFrontOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 02/04/14

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoASPStoreFrontOrderStatusUpdate", ex)

        End Try

    End Sub

    Public Sub StartASPStoreFrontStatusFile(ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, _
        ByVal ASPStoreFrontMessageType As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlUpdateNode As XElement

        iASPStoreFrontXMLMessageType = ASPStoreFrontMessageType
        Try
            XMLStatusUpdateFile = New XDocument

            xmlStatusUpdateNode = New XElement("AspDotNetStorefrontImport")
            xmlStatusUpdateNode.SetAttributeValue("Verbose", "")

            xmlUpdateNode = New XElement("Transaction")
            xmlStatusUpdateNode.Add(xmlUpdateNode)

            XMLStatusUpdateFile.Add(xmlStatusUpdateNode)

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartASPStoreFrontStatusFile", ex)

        End Try

    End Sub

    Public Sub AddToASPStoreFrontStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ByRef RowID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStartingUpdateXML As String, iInsertPosn As Integer

        Try
            If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null Then
                ' get current update XML
                strStartingUpdateXML = XMLStatusUpdateFile.ToString
                ' get position for insert
                iInsertPosn = InStr(strStartingUpdateXML, "</Transaction>")
                ' insert XML from action record
                strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & Mid(strStartingUpdateXML, iInsertPosn)
                ' reload update XML
                XMLStatusUpdateFile = XDocument.Parse(strStartingUpdateXML)
            End If

            ' mark action record as XML Sent (change is committed to DB when status file successfully posted to ASPDotNetStoreFront in SendASPStoreFrontStatusFile)
            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            bASPStoreFrontStatusUpdatesToSend = True

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddToASPStoreFrontStatusFile", ex)

        End Try

    End Sub

    Public Sub SendASPStoreFrontStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to use ASPStorefront Connector
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to log successful update and update dataset
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ASPStorefrontConnection As Lerryn.Facade.ImportExport.ASPStorefrontConnector ' TJS 18/03/11
        Dim XMLResponse As XDocument
        Dim strReturn As String, sTemp As String ' TJS 18/03/11
        Dim bInhibitWebPosts As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")

            If Not bInhibitWebPosts Then
                ASPStorefrontConnection = New Lerryn.Facade.ImportExport.ASPStorefrontConnector ' TJS 18/03/11
                sTemp = "" ' TJS 18/03/11
                XMLResponse = ASPStorefrontConnection.SendXMLToASPStorefront(ActiveASPStoreFrontSettings.UseWSE3Authentication, _
                    ActiveASPStoreFrontSettings.APIURL, ActiveASPStoreFrontSettings.APIUser, ActiveASPStoreFrontSettings.APIPwd, XMLStatusUpdateFile.ToString, sTemp) ' TJS 18/03/11
                ' any errors ?
                If sTemp = "" Then ' TJS 18/03/11
                    ' no, 
                    bASPStoreFrontStatusUpdatesToSend = False ' TJS 24/02/12
                    m_ImportExportConfigFacade.WriteLogProgressRecord("ASPDotNetStorefront Send Status update File Successful") ' TJS 24/02/12

                    ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                        "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", "DeleteLerrynImportExportActionStatus_DEV000221"}}, _
                        Interprise.Framework.Base.Shared.TransactionType.None, "ASPDotNetStorefront Send Status update File", False) ' TJS 24/02/12

                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendASPStoreFrontStatusFile", sTemp) ' TJS 18/03/11
                End If
                ASPStorefrontConnection = Nothing ' TJS 18/03/11

            Else
                m_ImportExportConfigFacade.WriteLogProgressRecord("ASPDotNetStorefront Send Status File - Inhibited from sending Status update file - content " & XMLStatusUpdateFile.ToString)
                strReturn = "<?xml version=""1.0"" encoding=""UTF-8""?>"

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendASPStoreFrontStatusFile", ex)

        End Try

    End Sub

End Module
