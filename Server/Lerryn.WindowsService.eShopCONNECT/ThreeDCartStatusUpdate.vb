' eShopCONNECT for Connected Business - Windows Service
' Module: ThreeDCartStatusUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 02 April 2014

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports System.IO
Imports System.Net

Module ThreeDCartStatusUpdate

    Public Sub Do3DCartOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal Active3DCartSettings As ThreeDCartSettings, ByRef RowID As Integer, _
     ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to mark sales order not found records as complete
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strInvoiceCode As String
        Dim iEntryRowID As Integer, iCheckLoop As Integer
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
                        ' do we have a 3DCart Invoice and Shipment Number (held in MerchantOrderID_DEV000221 field) ?
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
                        ' do we have a 3DCart Invoice and Shipment Number (held in MerchantOrderID_DEV000221 field) ?
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

                            If bAllItemsDelivered Then
                                ' all items delivered, send Order Shipped message
                                If Send3DCartShipmentDetails(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, Active3DCartSettings, iEntryRowID, SalesOrderCode, strInvoiceCode) Then
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                End If

                            ElseIf bSomeItemsDelivered Then
                                ' partial delivery only, 3DCart has no option for this
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
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Do3DCartOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 02/04/14

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "Do3DCartOrderStatusUpdate", ex)

        End Try

    End Sub

    Private Function Send3DCartShipmentDetails(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal Active3DCartSettings As ThreeDCartSettings, ByRef RowID As Integer, _
        ByVal SalesOrderCode As String, ByVal InvoiceCode As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument
        Dim strSubmit As String, strTrackingNumber As String, strReturn As String, strInvoiceShipment As String()
        Dim bXMLError As Boolean, dteActionTimestamp As Date

        dteActionTimestamp = ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionTimestamp_DEV000221
        strInvoiceShipment = ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221.Split(CChar(":"))
        strTrackingNumber = ImportExportStatusFacade.GetField("MasterTrackingNumber", "Shipment", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & InvoiceCode & "') ORDER BY ShippingDate DESC")

        strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?>"
        strSubmit = strSubmit & "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSubmit = strSubmit & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">"
        strSubmit = strSubmit & "<soap:Body><updateOrderShipment xmlns=""http://3dcart.com/"">"
        strSubmit = strSubmit & "<storeUrl>" & Active3DCartSettings.StoreURL & "</storeUrl>"
        strSubmit = strSubmit & "<userKey>" & Active3DCartSettings.UserKey & "</userKey>"
        strSubmit = strSubmit & "<invoiceNum>" & strInvoiceShipment(0) & "</invoiceNum>"
        strSubmit = strSubmit & "<shipmentID>" & strInvoiceShipment(1) & "</shipmentID>"
        strSubmit = strSubmit & "<tracking>" & strTrackingNumber & "</tracking>"
        strSubmit = strSubmit & "<shipmentDate>" & dteActionTimestamp.ToString("MM/dd/yyyy") & "</shipmentDate>"
        strSubmit = strSubmit & "<callBackURL></callBackURL>"
        strSubmit = strSubmit & "</updateOrderShipment></soap:Body></soap:Envelope>"

        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 2 ' 2 is OrderShipped
        ' save XML ready for sending
        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = strSubmit

        Try
            WebSubmit = System.Net.WebRequest.Create(THREE_D_CART_WEB_SERVICES_URL)
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "text/xml; charset=utf-8"
            WebSubmit.ContentLength = strSubmit.Length
            WebSubmit.Timeout = 30000

            byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

            ' send to 3DCart
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
                            XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://3dcart.com/""", ""))

                        Catch ex As Exception
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartShipmentDetails", "UpdateShipment XML from " & THREE_D_CART_WEB_SERVICES_URL & " for " & Active3DCartSettings.StoreID.ToString & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))
                            bXMLError = True

                        End Try

                        ' did XML load correctly
                        If Not bXMLError Then
                            ' yes
                            ' did we get an error ?
                            If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "UpdateOrderShipmentResponse/result") = "OK" Then
                                ' no, mark as completed
                                Return True

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartShipmentDetails", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & _
                                    Active3DCartSettings.StoreID & " was " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/getOrderResponse/getOrderResult/Error"), _
                                    strSubmit.Replace(Active3DCartSettings.UserKey, "********"))
                            End If
                        End If
                    Else
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartShipmentDetails", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & Active3DCartSettings.StoreID.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                    End If

                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartShipmentDetails", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                End If

            Else
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartShipmentDetails", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "Send3DCartShipmentDetails", ex)

        End Try
        Return False

    End Function
End Module
