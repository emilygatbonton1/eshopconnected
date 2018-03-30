' eShopCONNECT for Connected Business - Windows Service
' Module: VolusionStatusUpdate.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'
'       © 2014         Lerryn Business Solutions Ltd
'                      2 East View
'                      Bessie Lane
'                      Bradwell
'                      Hope Valley
'                      S33 9HZ
'
'  Tel +44 (0)1433 621584
'  Email Support@lerryn.com
'
' Lerryn is a Trademark of Lerryn Business Solutions Ltd
' eShopCONNECT is a Trademark of Lerryn Business Solutions Ltd
'-------------------------------------------------------------------
'
' Last Updated - 02 April 2014

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports System.IO
Imports System.Net

Module VolusionStatusUpdate

    Public Sub DoVolusionOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal ActiveVolusionSettings As VolusionSettings, ByRef RowID As Integer, _
     ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | Function added
        ' 11/04/14 | TJS             | 2013.4.04 | Modified to get carrier details and handle multiple tracking numbers
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to mark sales order not found records as complete
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strInvoiceCode As String, strShippingMethodID As String, strTrackingNumbers As String()() ' TJS 11/01/14
        Dim iEntryRowID As Integer, iCheckLoop As Integer, iShipmentNo As Integer ' TJS 11/01/14
        Dim bAllItemsDelivered As Boolean, bSomeItemsDelivered As Boolean

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
                        ' do we have a Volusion Order Number (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null AndAlso _
                            ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then
                            ' yes, for now mark as action complete
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
                                If bShutDownInProgress Then
                                    Exit For
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
                        ' do we have a Volusion Order Number (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null AndAlso _
                            ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then
                            ' yes, send status - get invoice detail (all rows) as order may not have QuantityShipped set
                            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerInvoiceDetail.TableName, _
                               "ReadCustomerInvoiceDetailImportExport_DEV000221", AT_SOURCE_INVOICE_CODE, SalesOrderCode}}, _
                               Interprise.Framework.Base.Shared.ClearType.Specific)

                            ' have all items been delivered ?
                            bAllItemsDelivered = True
                            bSomeItemsDelivered = False
                            strInvoiceCode = ""
                            For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1
                                strInvoiceCode = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).InvoiceCode
                                If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0 Then
                                    bSomeItemsDelivered = True
                                End If
                                If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityOrdered <> ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped Then
                                    bAllItemsDelivered = False
                                End If
                            Next

                            ' start of code amended TJS 11/01/14
                            If bAllItemsDelivered Or bSomeItemsDelivered Then
                                ' get tracking numbers for shipment
                                strTrackingNumbers = ImportExportStatusFacade.GetRows(New String() {"TrackingNumber", "CarrierDescription", "ServiceCode"}, "ShipmentView", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & strInvoiceCode & "') ORDER BY ShippingDate DESC")
                                If strTrackingNumbers Is Nothing OrElse strTrackingNumbers.Length = 0 Then
                                    strTrackingNumbers = New String()() {New String() {"", "", ""}}
                                End If
                                If ActiveSource.EnableDeliveryMethodTranslation Then
                                    strShippingMethodID = ImportExportStatusFacade.GetField("SourceDeliveryMethod_DEV000221", "LerrynImportExportDeliveryMethods_DEV000221", "SourceCode_DEV000221 = '" & VOLUSION_SOURCE_CODE & "' AND SourceDeliveryMethod_DEV000221 = '" & ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode & "'")
                                    If String.IsNullOrEmpty(strShippingMethodID) Then
                                        strShippingMethodID = ActiveVolusionSettings.DefaultShippingMethodID.ToString
                                    End If
                                Else
                                    strShippingMethodID = ActiveVolusionSettings.DefaultShippingMethodID.ToString
                                End If
                                iShipmentNo = 1
                                For Each TrackingNumber As String() In strTrackingNumbers
                                    ' only send tracking details if there is a tracking number or all items are delivered and this is the first shipment record
                                    If String.IsNullOrEmpty(TrackingNumber(0)) Or (bAllItemsDelivered And iShipmentNo = 1) Then
                                        If String.IsNullOrEmpty(TrackingNumber(0)) Then
                                            TrackingNumber(0) = "None-" & strInvoiceCode & iShipmentNo
                                        End If
                                        If Not SendVolusionShipmentDetails(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveVolusionSettings, iEntryRowID, SalesOrderCode, strInvoiceCode, bAllItemsDelivered, TrackingNumber(1), strShippingMethodID, TrackingNumber(0)) Then
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoVolusionOrderStatusUpdate", "Unable to add Tracking Number to Volusion Shipment")
                                        End If
                                    End If
                                Next
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

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
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoVolusionOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 02/04/14

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoVolusionOrderStatusUpdate", ex)

        End Try

    End Sub

    Private Function SendVolusionShipmentDetails(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveVolusionSettings As VolusionSettings, ByRef RowID As Integer, _
        ByVal SalesOrderCode As String, ByVal InvoiceCode As String, ByVal SetOrderShipped As Boolean, _
        ByVal CarrierName As String, ByVal ShippingMethodID As String, ByVal TrackingNumber As String) As Boolean ' TJS 11/04/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | Function added
        ' 11/04/14 | TJS             | 2013.4.04 | Modified to get carrier details and handle multiple tracking numbers
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument
        Dim strVolusionUpdateURL As String, strSubmit As String, strReturn As String
        Dim iTextPosn As Integer, bXMLError As Boolean

        strSubmit = "<?xml version=""1.0"" encoding=""utf-8"" ?>"
        strSubmit = strSubmit & "<xmldata>"
        strSubmit = strSubmit & "  <TrackingNumbers>"
        strSubmit = strSubmit & "     <gateway>" & CarrierName & "</gateway>" ' TJS 11/04/14
        strSubmit = strSubmit & "    <MarkOrderShipped>true</MarkOrderShipped>"
        strSubmit = strSubmit & "    <OrderID>" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 & "</OrderID>"
        strSubmit = strSubmit & "    <SendShippedEmail>false</SendShippedEmail>"
        strSubmit = strSubmit & "    <ShippingMethodID>" & ShippingMethodID & "</ShippingMethodID>" ' TJS 11/04/14
        strSubmit = strSubmit & "    <TrackingNumber>" & TrackingNumber & "</TrackingNumber>" ' TJS 11/04/14
        strSubmit = strSubmit & "  </TrackingNumbers>"
        strSubmit = strSubmit & "</xmldata>"

        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 2 ' 2 is OrderShipped
        ' save XML ready for sending
        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = strSubmit

        ' find where login password starts
        iTextPosn = InStr(ActiveVolusionSettings.OrderPollURL, "&EncryptedPassword=")
        iTextPosn = InStr(iTextPosn + 19, ActiveVolusionSettings.OrderPollURL, "&")
        strVolusionUpdateURL = Left(ActiveVolusionSettings.OrderPollURL, iTextPosn - 1) & "&Import=Insert-Update"

        Try
            WebSubmit = System.Net.WebRequest.Create(strVolusionUpdateURL)
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "text/xml; charset=utf-8"
            WebSubmit.ContentLength = strSubmit.Length
            WebSubmit.Timeout = 30000

            byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

            ' send to Volusion
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            strReturn = reader.ReadToEnd()

            If strReturn <> "" Then
                If strReturn.Length > 38 Then
                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Or strReturn.Trim.Substring(0, 14).ToLower = "<soap:envelope" Then
                        bXMLError = False
                        Try
                            ' had difficulty getting XPath to read XML with this name space present so remove it
                            XMLResponse = XDocument.Parse(strReturn)

                        Catch ex As Exception
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendVolusionShipmentDetails", "UpdateShipment XML from " & strVolusionUpdateURL & " for " & ActiveVolusionSettings.SiteID.ToString & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))
                            bXMLError = True

                        End Try

                        ' did XML load correctly
                        If Not bXMLError Then
                            ' yes
                            ' did we get an error ?
                            If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "Volusion_API/TrackingNumbers/Success").ToLower = "true" Then
                                ' no, mark as completed
                                Return True

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendVolusionShipmentDetails", "Response from " & strVolusionUpdateURL & " for " & _
                                    ActiveVolusionSettings.SiteID & " was " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/getOrderResponse/getOrderResult/Error"), _
                                    strSubmit)
                            End If
                        End If
                    Else
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendVolusionShipmentDetails", "Response from " & strVolusionUpdateURL & " for " & ActiveVolusionSettings.SiteID.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                    End If

                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendVolusionShipmentDetails", "Response from " & strVolusionUpdateURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                End If

            Else
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendVolusionShipmentDetails", "Response from " & strVolusionUpdateURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "SendVolusionShipmentDetails", ex)

        End Try
        Return False

    End Function
End Module
