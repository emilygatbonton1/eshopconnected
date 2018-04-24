' eShopCONNECT for Connected Business - Windows Service
' Module: ChannelAdvStatusUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 01 May 2014

Imports System.IO
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 18/03/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module ChannelAdvStatusUpdate

    Private XMLNSManCAOrders As System.Xml.XmlNamespaceManager ' TJS 02/04/14
    Private XMLNameTabCAOrders As System.Xml.NameTable ' TJS 02/04/14

    Private XMLStatusUpdateFile As XDocument
    Private xmlStatusUpdateNode As XElement
    Public bChannelAdvStatusUpdatesToSend As Boolean
    Private iChannelAdvXMLMessageType As Integer
    Private iChannelAdvXMLMessageCount As Integer ' TJS 02/04/14

    Public Sub DoChannelAdvOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings, ByRef RowID As Integer, _
        ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 31/12/09 | TJS             | 2010.0.00 | Function added
        ' 07/01/10 | TJS             | 2010.0.02 | Corrected SubmitOrderShipmentList XML
        ' 12/10/10 | FA/TJS          | 2010.0.03 | Modified to cater for deliveries with no items.  This
        '                                          can happen if invoice produced but not marked as shipped
        ' 15/06/10 | TJS             | 2010.0.07 | Modified to prevent error when logging messages if related rows are skipped
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to check if deilvery method translation is enabled and modified to ignore 
        '                                        | blank MerchantOrderIDs as well as null values
        ' 27/09/10 | TJS             | 2010.1.02 | Modified to pick up tracking number from Shipping Module
        ' 23/11/10 | FA              | 2010.1.09 | Optional milliseconds passed to xml date
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Shipments also being done from Invoice
        ' 14/08/12 | TJS             | 2012.1.13 | Modified to ignore shipments if no Source Purchase ID to prevent Channel ADvisor errors
        ' 28/07/13 | TJS             | 2013.1.31 | Tidied progress log message text
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to replace OrderShipped with SubmitOrderShipmentList
        '                                        | and mark sales order not found records as complete
        ' 01/04/15 | TJS             | 2014.0.02 | Modified removed code to update shipment cost as it can cause errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSourceCarrierCode As String, strSourceCarrierClass As String
        Dim strSourceCarrierClassCode As String, strXML As String, strTrackingNumber As String ' TJS 27/09/10
        Dim iCheckLoop As Integer, iEntryRowID As Integer, bAllItemsDelivered As Boolean ' TJS 15/06/10
        Dim bSomeItemsDelivered As Boolean, strInvoiceCode As String 'FA/TJS 12/01/10 TJS 09/07/11
        Dim bTranslationFound As Boolean

        Try
            iEntryRowID = RowID ' TJS 15/06/10
            ' for some reason, POCode field seems to cause constraint error if populated so disable constraints to allow sales order to load
            ImportExportStatusDataset.EnforceConstraints = False
            ' get order record
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrder.TableName, _
               "ReadCustomerSalesOrder", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.CustomerSalesOrder.Count > 0 Then
                Select Case ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221
                    Case "100" ' New Order
                        ' do we have a Channel Advisor Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status
                                strXML = "<int>" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 & "</int>"
                                strXML = strXML & "<string>" & ImportExportStatusDataset.CustomerSalesOrder(0).SalesOrderCode & "</string>"

                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = strXML
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 1 ' 1 is SetSellerOrderID
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If

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
                        ' do we have a Channel Advisor Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status - get invoice detail (all rows) as order may not have QuantityShipped set
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerInvoiceDetail.TableName, _
                                   "ReadCustomerInvoiceDetailImportExport_DEV000221", AT_SOURCE_INVOICE_CODE, SalesOrderCode}}, _
                                   Interprise.Framework.Base.Shared.ClearType.Specific)

                                ' have all items been delivered ?
                                bAllItemsDelivered = True
                                bSomeItemsDelivered = False 'FA/TJS 12/01/10
                                strInvoiceCode = "" ' TJS 09/07/11
                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1
                                    strInvoiceCode = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).InvoiceCode ' TJS 09/07/11
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0 And _
                                        Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then  'FA/TJS 12/01/10 TJS 14/08/12
                                        bSomeItemsDelivered = True 'FA/TJS 12/01/10
                                    End If
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityOrdered <> ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped And _
                                        Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then ' TJS 14/08/12
                                        bAllItemsDelivered = False
                                        'Exit For  'FA/TJS 12/01/10
                                    End If
                                Next

                                strSourceCarrierCode = ""
                                strSourceCarrierClass = ""
                                strSourceCarrierClassCode = ""
                                bTranslationFound = False
                                If ActiveSource.EnableDeliveryMethodTranslation Then ' TJS 19/08/10
                                    ImportExportStatusFacade.TranslateDeliveryMethodFromIS(ActiveSource.SourceCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodGroup, strSourceCarrierCode, strSourceCarrierClass, strSourceCarrierClassCode)
                                    If Not String.IsNullOrWhiteSpace(strSourceCarrierCode) And Not String.IsNullOrWhiteSpace(strSourceCarrierClassCode) Then
                                        bTranslationFound = True
                                    End If
                                Else
                                    strSourceCarrierCode = ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode ' TJS 19/08/10
                                End If

                                If Not bTranslationFound Then
                                    Dim message As String = "Sales Order " & SalesOrderCode _
                                           & " - No source delivery method and class will be sent to Channel Advisor because the sales order shipping method has no delivery method translation. " _
                                           & "To ensure accurate tracking information is recorded, kindly make it updated in Channel Advisor."
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoChannelAdvOrderStatusUpdate", message)
                                End If

                                If bAllItemsDelivered Then
                                    ' all items delivered, send Order Shipped message
                                    'FA 23/11/10 modified to make milliseconds optional
                                    ' start of code re-written to use Submit Order Shipment List TJS 02/04/14 
                                    strXML = "<OrderID>" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221
                                    strXML = strXML & "</OrderID><ClientOrderIdentifier>" & ImportExportStatusDataset.CustomerSalesOrder(0).POCode
                                    strXML = strXML & "</ClientOrderIdentifier><ShipmentType>Full</ShipmentType><FullShipment><DateShippedGMT>" & ChannelAdvXMLDate(DateTime.UtcNow, False)
                                    strXML = strXML & "</DateShippedGMT><CarrierCode>" & strSourceCarrierCode & "</CarrierCode><ClassCode>" & strSourceCarrierClassCode & "</ClassCode>"
                                    If ImportExportStatusFacade.GetField("ColumnName", "DataDictionaryColumn", "TableName = 'Shipment' AND ColumnName = 'TrackingNumber'") <> "" Then

                                        strTrackingNumber = GetShipmentInformation(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, SalesOrderCode, strInvoiceCode, bTranslationFound)

                                        If "" & strTrackingNumber <> "" Then
                                            strXML = strXML & "<TrackingNumber>" & strTrackingNumber & "</TrackingNumber>"
                                        Else
                                            strXML = strXML & "<TrackingNumber xsi:nil=""true""/>"
                                        End If
                                    Else
                                        strXML = strXML & "<TrackingNumber xsi:nil=""true""/>"
                                    End If
                                    ' removed code to update shipment cost as it can cause errors TJS 01/05/14
                                    'strXML = strXML & "<ShipmentCost>0</ShipmentCost><ShipmentTaxCost>0</ShipmentTaxCost>"
                                    'strXML = strXML & "<InsuranceCost>0</InsuranceCost></FullShipment>"
                                    strXML = strXML & "</FullShipment>"

                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is SubmitOrderShipmentList 
                                    ' end of code re-written to use Submit Order Shipment List TJS 02/04/14 
                                ElseIf bSomeItemsDelivered Then 'FA/TJS 12/01/10
                                    ' partial delivery only, send Submit Order Shipment List 
                                    strXML = "<OrderID>" & ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 ' TJS 02/04/14
                                    strXML = strXML & "</OrderID><ClientOrderIdentifier>" & ImportExportStatusDataset.CustomerSalesOrder(0).POCode ' TJS 07/01/10
                                    strXML = strXML & "</ClientOrderIdentifier><ShipmentType>Partial</ShipmentType><PartialShipment>" ' TJS 07/01/10
                                    For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1
                                        If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0 And _
                                            Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then
                                            If ActiveChannelAdvSettings.ISItemIDField = "ItemName" Then
                                                strXML = strXML & "<LineItemList><SKU>" & ImportExportStatusFacade.GetField("ItemName", "InventoryItem", "ItemCode = '" & ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).ItemCode & "'")
                                            Else
                                                strXML = strXML & "<LineItemList><SKU>" & ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).ItemCode
                                            End If
                                            strXML = strXML & "</SKU><Quantity>" & Format(ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped, "0")
                                            strXML = strXML & "</Quantity></LineItemList>"
                                        End If
                                    Next
                                    strXML = strXML & "<DateShippedGMT>" & ChannelAdvXMLDate(DateTime.UtcNow, False) 'FA 23/11/10
                                    strXML = strXML & "</DateShippedGMT><CarrierCode>" & strSourceCarrierCode & "</CarrierCode><ClassCode>"
                                    If ImportExportStatusFacade.GetField("ColumnName", "DataDictionaryColumn", "TableName = 'Shipment' AND ColumnName = 'TrackingNumber'") <> "" Then ' TJS 27/09/10
                                        strTrackingNumber = GetShipmentInformation(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, SalesOrderCode, strInvoiceCode, bTranslationFound)
                                        If "" & strTrackingNumber <> "" Then ' TJS 27/09/10
                                            strXML = strXML & strSourceCarrierClassCode & "</ClassCode><TrackingNumber>" & strTrackingNumber & "</TrackingNumber>" ' TJS 27/09/10
                                        Else
                                            strXML = strXML & strSourceCarrierClassCode & "</ClassCode><TrackingNumber xsi:nil=""true""/>" ' TJS 27/09/10
                                        End If
                                    Else
                                        strXML = strXML & strSourceCarrierClassCode & "</ClassCode><TrackingNumber xsi:nil=""true""/>"
                                    End If
                                    ' removed code to update shipment cost as it can cause errors TJS 01/05/14
                                    'strXML = strXML & "<ShipmentCost>0</ShipmentCost><ShipmentTaxCost>0</ShipmentTaxCost>" ' TJS 07/01/10
                                    'strXML = strXML & "<InsuranceCost>0</InsuranceCost></PartialShipment>" ' TJS 07/01/10 TJS 02/04/14
                                    strXML = strXML & "</PartialShipment>"

                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is SubmitOrderShipmentList 
                                Else
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True  'FA/TJS 12/01/10
                                    strXML = "" 'FA/TJS 12/01/10
                                End If
                                If bSomeItemsDelivered Then 'FA/TJS 12/01/10
                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = strXML
                                    ' set message type and mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                        End If


                    Case Else ' TJS 19/08/10
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10

                End Select

                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor Order Status " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                       " - No XML created.") ' TJS 28/07/13
                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor Order Status " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                        " - XML created " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221) ' TJS 28/07/13
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoChannelAdvOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 02/04/14

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoChannelAdvOrderStatusUpdate", ex)

        End Try
    End Sub

    Public Function GetShipmentInformation(ByVal ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByVal ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, SalesOrderCode As String, InvoiceCode As String, TranslationFound As Boolean) As String
        Dim shipmentInfo() As String = ImportExportStatusFacade.GetRow(New String() {"CarrierCode", "ServiceCode", "UserModified", "TrackingNumber"}, "Shipment", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & InvoiceCode & "') ORDER BY ShippingDate DESC")
        Dim shipmentCarrierCode As String = String.Empty
        Dim shipmentServiceType As String = String.Empty
        Dim userModified As String = String.Empty
        Dim trackingNumber As String = String.Empty

        If shipmentInfo IsNot Nothing AndAlso shipmentInfo.Length = 4 Then
            shipmentCarrierCode = shipmentInfo(0)
            shipmentServiceType = shipmentInfo(1)
            userModified = shipmentInfo(2)
            trackingNumber = shipmentInfo(3)
        End If

        If TranslationFound Then
            If ActiveSource.EnableDeliveryMethodTranslation And Not String.IsNullOrWhiteSpace(trackingNumber) Then
                Dim shippingMethodCarrierCode As String = String.Empty
                Dim shippingMethodServiceType As String = String.Empty
                Dim shippingMethodInfo() As String = ImportExportStatusFacade.GetRow(New String() {"CarrierCode", "ServiceType"}, "ShipmentShippingMethodDefault", "ShippingMethodCode ='" & ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode & "' AND WarehouseCode = '" & ImportExportStatusDataset.CustomerSalesOrder(0).WarehouseCode & "'")
                If shippingMethodInfo IsNot Nothing AndAlso shippingMethodInfo.Length = 2 Then
                    shippingMethodCarrierCode = shippingMethodInfo(0)
                    shippingMethodServiceType = shippingMethodInfo(1)
                End If

                If Not String.Equals(shippingMethodCarrierCode, shipmentCarrierCode, StringComparison.OrdinalIgnoreCase) _
                Or Not String.Equals(shippingMethodServiceType, shipmentServiceType, StringComparison.OrdinalIgnoreCase) Then
                    Dim message As String = "Sales Order " & SalesOrderCode _
                        & " - Source delivery method and class will be sent to Channel Advisor based on Delivery Method Translation. " _
                        & "But, the default carrier or service type was changed manually by " & userModified & " in the shipment transaction." _
                        & "To ensure accurate tracking information, kindly make the information updated in Channel Advisor."
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "GetShipmentInformation", message)
                End If
            End If
        End If

        Return trackingNumber
    End Function

    Public Sub StartChannelAdvStatusFile(ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings, _
        ByVal ChannelAdvMessageType As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 31/12/09 | TJS             | 2010.0.00 | Function added
        ' 07/01/10 | TJS             | 2010.0.02 | Corrected SubmitOrderShipmentList XML
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to initialise namespace manager references and 
        '                                        | and initialised message counter
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSubmit As String

        iChannelAdvXMLMessageType = ChannelAdvMessageType
        Try
            Select Case iChannelAdvXMLMessageType
                Case 1 ' SetSellerOrderID
                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope  xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                    strSubmit = strSubmit & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">"
                    If "" & ActiveChannelAdvSettings.OwnDeveloperKey <> "" Then
                        strSubmit = strSubmit & "<soapenv:Header><APICredentials xmlns=""http://api.channeladvisor.com/webservices/""><DeveloperKey>"
                        strSubmit = strSubmit & ActiveChannelAdvSettings.OwnDeveloperKey & "</DeveloperKey><Password>" & ActiveChannelAdvSettings.OwnDeveloperPwd
                    Else
                        strSubmit = strSubmit & "<soapenv:Header><APICredentials xmlns=""http://api.channeladvisor.com/webservices/""><DeveloperKey>"
                        strSubmit = strSubmit & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY & "</DeveloperKey><Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                    End If
                    strSubmit = strSubmit & "</Password></APICredentials></soapenv:Header><soapenv:Body><SetSellerOrderID xmlns=""http://api.channeladvisor.com/webservices/"">"
                    strSubmit = strSubmit & "<accountID>" & ActiveChannelAdvSettings.AccountID & "</accountID><orderIDList></orderIDList>"
                    strSubmit = strSubmit & "<sellerOrderIDList></sellerOrderIDList></SetSellerOrderID></soapenv:Body></soapenv:Envelope>"

                Case 2 ' OrderShipped
                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope  xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                    strSubmit = strSubmit & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">"
                    If "" & ActiveChannelAdvSettings.OwnDeveloperKey <> "" Then
                        strSubmit = strSubmit & "<soapenv:Header><APICredentials xmlns=""http://api.channeladvisor.com/webservices/""><DeveloperKey>"
                        strSubmit = strSubmit & ActiveChannelAdvSettings.OwnDeveloperKey & "</DeveloperKey><Password>" & ActiveChannelAdvSettings.OwnDeveloperPwd
                    Else
                        strSubmit = strSubmit & "<soapenv:Header><APICredentials xmlns=""http://api.channeladvisor.com/webservices/""><DeveloperKey>"
                        strSubmit = strSubmit & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY & "</DeveloperKey><Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                    End If
                    strSubmit = strSubmit & "</Password></APICredentials></soapenv:Header><soapenv:Body><OrderShipped xmlns=""http://api.channeladvisor.com/webservices/"">"
                    strSubmit = strSubmit & "<accountID>" & ActiveChannelAdvSettings.AccountID & "</accountID></OrderShipped></soapenv:Body></soapenv:Envelope>"

                Case 3 ' SubmitOrderShipmentList
                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope  xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                    strSubmit = strSubmit & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">"
                    If "" & ActiveChannelAdvSettings.OwnDeveloperKey <> "" Then
                        strSubmit = strSubmit & "<soapenv:Header><APICredentials xmlns=""http://api.channeladvisor.com/webservices/""><DeveloperKey>"
                        strSubmit = strSubmit & ActiveChannelAdvSettings.OwnDeveloperKey & "</DeveloperKey><Password>" & ActiveChannelAdvSettings.OwnDeveloperPwd
                    Else
                        strSubmit = strSubmit & "<soapenv:Header><APICredentials xmlns=""http://api.channeladvisor.com/webservices/""><DeveloperKey>"
                        strSubmit = strSubmit & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY & "</DeveloperKey><Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                    End If
                    strSubmit = strSubmit & "</Password></APICredentials></soapenv:Header><soapenv:Body><SubmitOrderShipmentList xmlns=""http://api.channeladvisor.com/webservices/"">"
                    strSubmit = strSubmit & "<accountID>" & ActiveChannelAdvSettings.AccountID & "</accountID><shipmentList>" ' TJS 07/01/10 TJS 02/04/14
                    strSubmit = strSubmit & "</shipmentList></SubmitOrderShipmentList></soapenv:Body></soapenv:Envelope>" ' TJS 07/01/10 TJS 02/04/14

                Case Else
                    strSubmit = ""
                    Return
            End Select
            XMLStatusUpdateFile = XDocument.Parse(strSubmit)
            XMLNameTabCAOrders = New System.Xml.NameTable ' TJS 02/04/14
            XMLNSManCAOrders = New System.Xml.XmlNamespaceManager(XMLNameTabCAOrders) ' TJS 02/04/14
            XMLNSManCAOrders.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/04/14
            XMLNSManCAOrders.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/04/14
            iChannelAdvXMLMessageCount = 0 ' TJS 02/04/14

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartChannelAdvStatusFile", ex)

        End Try

    End Sub

    Public Sub AddToChannelAdvStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings, ByRef RowID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 31/12/09 | TJS             | 2010.0.00 | Function added
        ' 07/01/10 | TJS             | 2010.0.02 | Corrected SubmitOrderShipmentList XML
        ' 02/04/14 | TJS             | 2014.0.01 | Modified for max message counter
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStartingUpdateXML As String, iSplitPosn As Integer, iInsertPosn As Integer
        Dim bMaxMessageCount As Boolean ' TJS 02/04/14

        Try
            If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null Then
                ' get current update XML
                strStartingUpdateXML = XMLStatusUpdateFile.ToString
                Select Case iChannelAdvXMLMessageType
                    Case 1 ' SetSellerOrderID
                        ' get position to split insert XML
                        iSplitPosn = InStr(ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221, "<string>")

                        ' get position for first insert 
                        iInsertPosn = InStr(strStartingUpdateXML, "</orderIDList>")
                        ' insert first part of XML from action record
                        strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221.Substring(0, iSplitPosn - 1) & Mid(strStartingUpdateXML, iInsertPosn)

                        ' get position for second insert 
                        iInsertPosn = InStr(strStartingUpdateXML, "</sellerOrderIDList>")
                        ' insert second part of XML from action record
                        strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221.Substring(iSplitPosn - 1) & Mid(strStartingUpdateXML, iInsertPosn)


                    Case 2 ' OrderShipped - no longer used by CHannel Advisor
                        ' get position for insert
                        iInsertPosn = InStr(strStartingUpdateXML, "</OrderShipped>")
                        ' insert XML from action record
                        strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & Mid(strStartingUpdateXML, iInsertPosn)


                    Case 3 ' SubmitOrderShipmentList
                        ' get position for insert
                        iInsertPosn = InStr(strStartingUpdateXML, "</shipmentList>") ' TJS 07/01/10 TJS 02/04/14
                        ' check multiple order
                        If InStr(strStartingUpdateXML, "<OrderID>") > 0 Then
                            ' insert XML from action record
                            strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & "</shipmentList><shipmentList>" & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & Mid(strStartingUpdateXML, iInsertPosn)
                        Else
                            ' insert XML from action record
                            strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & Mid(strStartingUpdateXML, iInsertPosn)
                        End If

                    Case Else
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "AddToChannelAdvStatusFile", "Unknown Channel Advisor XML Message Type " & iChannelAdvXMLMessageType, "")
                        Return

                End Select
                ' reload update XML
                XMLStatusUpdateFile = XDocument.Parse(strStartingUpdateXML)
            End If

            ' mark action record as XML Sent (change is committed to DB when status file successfully posted to Channel Advisor in SendChannelAdvStatusFile)
            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            ' start of code added TJS 02/04/14
            iChannelAdvXMLMessageCount += 1
            bMaxMessageCount = False
            Select iChannelAdvXMLMessageType
                Case 1 ' SetSellerOrderID
                    If iChannelAdvXMLMessageCount >= 500 Then
                        bMaxMessageCount = True
                    End If

                Case 3 ' SubmitOrderShipmentList
                    If iChannelAdvXMLMessageCount >= 50 Then
                        bMaxMessageCount = True
                    End If
            End Select
            If bMaxMessageCount Then
                SendChannelAdvStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings)
                StartChannelAdvStatusFile(ActiveSource, ActiveChannelAdvSettings, iChannelAdvXMLMessageType)
                bChannelAdvStatusUpdatesToSend = False
                ' end of code added TJS 02/04/14
            Else

            	bChannelAdvStatusUpdatesToSend = True
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddToChannelAdvStatusFile", ex)

        End Try

    End Sub

    Public Sub SendChannelAdvStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 31/12/09 | TJS             | 2010.0.00 | Function added
        ' 07/06/10 | TJS             | 2010.0.07 | Modified to mask Channel Advisor Dev Pwd in error messages
        ' 19/08/10 | TJS             | 2010.1.00 | Corrected InhibitWebPosts initialisation
        ' 04/10/10 | TJS             | 2010.1.05 | Added CA account details in log messages
        ' 13/12/10 | FA              | 2010.1.12 | Changed send error details to mask developer password
        ' 14/12/10 | FA              | 2010.1.13 | Fix to above
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 01/05/14 | TJS             | 2014.0.02 | Added missing namespace manager references
        '                                        | Modified to ensure shipments actions are marked as complete even if CA Order has been deleted
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument, XMLSubmitTemp As XDocument ' TJS 02/12/11 TJS 02/04/14
        Dim XMLTemp As XDocument, XMLISSalesOrders As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLCAOrderIDs As System.Collections.Generic.IEnumerable(Of XElement), XMLCAOrderID As XElement
        Dim XMLResults As System.Collections.Generic.IEnumerable(Of XElement), XMLResult As XElement
        Dim strReturn As String = "", strUpdateURL As String, strSOAPAction As String
        Dim strAccountID As String, strCAOrderID As String, strErrorText As String
        Dim iSourceLoop As Integer, bInhibitWebPosts As Boolean, bOrderIDExists As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") ' TJS 19/08/10

            ' had difficulty getting XPath to read XML with this name space present so remove it
            XMLSubmitTemp = XDocument.Parse(XMLStatusUpdateFile.ToString.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", "")) ' TJS 02/04/14

            Select Case iChannelAdvXMLMessageType
                Case 1 ' SetSellerOrderID
                    strUpdateURL = CHANNEL_ADVISOR_ORDER_SERVICE_URL
                    strSOAPAction = "http://api.channeladvisor.com/webservices/SetSellerOrderID"
                    strAccountID = ImportExportStatusFacade.GetXMLElementText(XMLSubmitTemp, CHANNEL_ADV_STATUS_SET_SELLER_ORDER_ID & "accountID", XMLNSManCAOrders) ' TJS 02/04/14

                Case 2 ' OrderShipped
                    strUpdateURL = CHANNEL_ADVISOR_SHIPPING_SERVICE_URL
                    strSOAPAction = "http://api.channeladvisor.com/webservices/OrderShipped"
                    strAccountID = ImportExportStatusFacade.GetXMLElementText(XMLSubmitTemp, CHANNEL_ADV_STATUS_ORDER_SHIPPED & "accountID", XMLNSManCAOrders) ' TJS 02/04/14

                Case 3 ' SubmitOrderShipmentList
                    strUpdateURL = CHANNEL_ADVISOR_SHIPPING_SERVICE_URL
                    strSOAPAction = "http://api.channeladvisor.com/webservices/SubmitOrderShipmentList"
                    strAccountID = ImportExportStatusFacade.GetXMLElementText(XMLSubmitTemp, CHANNEL_ADV_STATUS_SUBMIT_ORDER_SHIPMENT_LIST & "accountID", XMLNSManCAOrders) ' TJS 02/04/14

                Case Else
                    strUpdateURL = ""
                    strSOAPAction = ""
                    strAccountID = ""
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Unknown Channel Advisor XML Message Type " & iChannelAdvXMLMessageType, XMLStatusUpdateFile.ToString) ' TJS 02/04/14
                    Return

            End Select
            ' check submission Account ID is as expected
            If strAccountID = ActiveChannelAdvSettings.AccountID Then

                ' start of code replaced TJS 02/12/11
                Try
                    If Not bInhibitWebPosts Then
                        WebSubmit = System.Net.WebRequest.Create(strUpdateURL)
                        WebSubmit.Method = "POST"
                        WebSubmit.ContentType = "text/xml; charset=utf-8"
                        WebSubmit.ContentLength = XMLStatusUpdateFile.ToString.Length
                        WebSubmit.Headers.Add("SOAPAction", strSOAPAction)
                        WebSubmit.Timeout = 30000

                        byteData = UTF8Encoding.UTF8.GetBytes(XMLStatusUpdateFile.ToString)

                        ' send to LerrynSecure.com (or the URL defined in the Registry)
                        postStream = WebSubmit.GetRequestStream()
                        postStream.Write(byteData, 0, byteData.Length)

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        strReturn = reader.ReadToEnd()

                    Else
                        m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor Send Status File - Inhibited from sending Status update file - content " & XMLStatusUpdateFile.ToString)
                        Select Case iChannelAdvXMLMessageType
                            Case 1 ' SetSellerOrderID
                                strReturn = "<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                strReturn = strReturn & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                                strReturn = strReturn & "<soap:Body><SetSellerOrderIDResponse xmlns=""http://api.channeladvisor.com/webservices/"">"
                                strReturn = strReturn & "<SetSellerOrderIDResult><Status>Success</Status><MessageCode>0</MessageCode><ResultData>"
                                ' get original Channel Advisor Order IDs from update XML
                                XMLCAOrderIDs = XMLSubmitTemp.XPathSelectElements(CHANNEL_ADV_STATUS_SET_SELLER_ORDER_ID & "orderIDList/int", XMLNSManCAOrders) ' TJS 02/04/14
                                For Each XMLCAOrderID In XMLCAOrderIDs
                                    XMLTemp = XDocument.Parse(XMLCAOrderID.ToString)
                                    ' did XML load correctly
                                    If XMLTemp.ToString <> "" Then
                                        ' yes, get Channel Advisor Order ID from update message
                                        strReturn = strReturn & "<int>" & ImportExportStatusFacade.GetXMLElementText(XMLTemp, "int", XMLNSManCAOrders) & "</int>" ' TJS 02/04/14
                                    End If
                                Next
                                strReturn = strReturn & "</ResultData></SetSellerOrderIDResult></SetSellerOrderIDResponse></soap:Body></soap:Envelope>"

                            Case 2 ' OrderShipped
                                strReturn = "<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                strReturn = strReturn & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                                strReturn = strReturn & "<soap:Body><OrderShippedResponse xmlns=""http://api.channeladvisor.com/webservices/"">"
                                strReturn = strReturn & "<OrderShippedResult><Status>Success</Status><MessageCode>0</MessageCode><ResultData>true</ResultData>"
                                strReturn = strReturn & "</OrderShippedResult></OrderShippedResponse></soap:Body></soap:Envelope>"

                            Case 3 ' SubmitOrderShipmentList
                                strReturn = "<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                strReturn = strReturn & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                                strReturn = strReturn & "<soap:Body><SubmitOrderShipmentListResponse xmlns=""http://api.channeladvisor.com/webservices/"">"
                                strReturn = strReturn & "<SubmitOrderShipmentListResult><Status>Success</Status><MessageCode>0</MessageCode><ResultData>true</ResultData>"
                                strReturn = strReturn & "</SubmitOrderShipmentListResult></SubmitOrderShipmentListResponse></soap:Body></soap:Envelope>"

                        End Select
                    End If

                Catch ex As Exception
                    ' send error details but mask developer password
                    'FA 14/12/10 
                    If ActiveChannelAdvSettings.OwnDeveloperPwd.ToString <> "" Then
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Unable to post Channel Advisor XML to " & strUpdateURL & " - " & ex.Message & ", " & ex.StackTrace, XMLStatusUpdateFile.ToString.Replace(ActiveChannelAdvSettings.OwnDeveloperPwd, "********")) ' TJS 07/06/10
                    Else
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Unable to post Channel Advisor XML to " & strUpdateURL & " - " & ex.Message & ", " & ex.StackTrace, XMLStatusUpdateFile.ToString.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********")) ' TJS 07/06/10
                    End If
                    Return

                Finally
                    If Not postStream Is Nothing Then postStream.Close()
                    If Not WebResponse Is Nothing Then WebResponse.Close()

                End Try
                ' end of code replaced TJS 02/12/11

                If strReturn <> "" Then
                    If strReturn.Length > 38 Then
                        If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                            Try ' TJS 02/04/14
                                ' had difficulty getting XPath to read XML with this name space present so remove it
                                XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", "")) ' TJS 02/04/14

                            Catch ex As Exception ' TJS 02/04/14
                                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Unable to process response from Channel Advisor due to XML error - " & ex.Message.Replace(vbCrLf, ""), strReturn) ' TJS 02/04/14
                                Return ' TJS 02/04/14

                            End Try

                            Select Case iChannelAdvXMLMessageType
                                Case 1 ' SetSellerOrderID
                                    ' check response status
                                    If ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "Status", XMLNSManCAOrders).ToLower = "success" Then ' TJS 02/04/14
                                        ' response is success, get results list
                                        XMLResults = XMLResponse.XPathSelectElements(CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "ResultData/int", XMLNSManCAOrders) ' TJS 02/04/14
                                        ' get original Channel Advisor Order IDs from update XML
                                        XMLCAOrderIDs = XMLSubmitTemp.XPathSelectElements(CHANNEL_ADV_STATUS_SET_SELLER_ORDER_ID & "orderIDList/int", XMLNSManCAOrders) ' TJS 02/04/14
                                        ' get original IS Sales Order Codes from update XML
                                        XMLISSalesOrders = XMLSubmitTemp.XPathSelectElements(CHANNEL_ADV_STATUS_SET_SELLER_ORDER_ID & "sellerOrderIDList/string", XMLNSManCAOrders) ' TJS 02/04/14
                                        ' is number of orders updated the same as the number of orders in update message ?
                                        If m_ImportExportConfigFacade.GetXMLElementListCount(XMLResults) = m_ImportExportConfigFacade.GetXMLElementListCount(XMLCAOrderIDs) Then
                                            ' yes - status update successful, save 
                                            If Not bInhibitWebPosts Then ' TJS 19/08/10
                                                m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor SetSellerOrderID Update Successful for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & ")") ' TJS 04/10/10 TJS 02/04/14
                                            End If
                                            ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                                                "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", "DeleteLerrynImportExportActionStatus_DEV000221"}}, _
                                                Interprise.Framework.Base.Shared.TransactionType.None, "Channel Advisor SetSellerOrderID", False)
                                        Else
                                            ' at least one update failed, find which one(s)
                                            If XMLResults IsNot Nothing Then
                                                strErrorText = ""
                                                For Each XMLCAOrderID In XMLCAOrderIDs
                                                    XMLTemp = XDocument.Parse(XMLCAOrderID.ToString)
                                                    ' did XML load correctly
                                                    If XMLTemp.ToString <> "" Then
                                                        ' yes, get Channel Advisor Order ID from update message
                                                        strCAOrderID = ImportExportStatusFacade.GetXMLElementText(XMLTemp, "int", XMLNSManCAOrders) ' TJS 02/04/14
                                                        bOrderIDExists = False
                                                        ' does Channel Advisor Order ID exist in result XML ?
                                                        For Each XMLResult In XMLResults
                                                            XMLTemp = XDocument.Parse(XMLResult.ToString)
                                                            ' did XML load correctly
                                                            If XMLTemp.ToString <> "" Then
                                                                ' yes, do Channel Advisor Order IDs match
                                                                If strCAOrderID = ImportExportStatusFacade.GetXMLElementText(XMLTemp, "int", XMLNSManCAOrders) Then ' TJS 02/04/14
                                                                    ' yes
                                                                    bOrderIDExists = True
                                                                    Exit For
                                                                End If
                                                            End If
                                                        Next
                                                        If Not bOrderIDExists Then
                                                            ' get matching IS Sales Order code
                                                            XMLTemp = XDocument.Parse(XMLISSalesOrders(iSourceLoop).ToString)
                                                            ' did XML load correctly
                                                            If XMLTemp.ToString <> "" Then
                                                                ' yes, add order details to error text
                                                                If strErrorText <> "" Then
                                                                    strErrorText = strErrorText & ", "
                                                                End If
                                                                strErrorText = strErrorText & "Channel Advisor Order ID " & strCAOrderID
                                                                strErrorText = strErrorText & ", IS Sales Order Code " & ImportExportStatusFacade.GetXMLElementText(XMLTemp, "string", XMLNSManCAOrders) ' TJS 02/04/14
                                                            End If
                                                        End If
                                                    End If
                                                Next
                                                'ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Channel Advisor SetSellerOrderID failed to update " & strErrorText & " for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & ")", XMLStatusUpdateFile.ToString) ' TJS 04/10/10
                                                'FA 13/12/10 send error details but mask developer password FA 14/12/10 modified to check either their dev passwordd or ours if their pwd is blank
                                                If ActiveChannelAdvSettings.OwnDeveloperPwd.ToString <> "" Then
                                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Channel Advisor SetSellerOrderID failed to update " & strErrorText & " for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & ")", XMLStatusUpdateFile.ToString.Replace(ActiveChannelAdvSettings.OwnDeveloperPwd, "********"))
                                                Else
                                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Channel Advisor SetSellerOrderID failed to update " & strErrorText & " for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & ")", XMLStatusUpdateFile.ToString.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********"))
                                                End If

                                            Else
                                                ' no results data found
                                            End If
                                        End If
                                        bChannelAdvStatusUpdatesToSend = False
                                    Else
                                        'ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "Status", XMLNSManCAOrder) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "Message", XMLNSManCAOrder), XMLResponse.ToString) ' TJS 04/10/10
                                        'FA 13/12/10 send error details but mask developer password FA 14/12/10 modified to check either their dev passwordd or ours if their pwd is blank
                                        If ActiveChannelAdvSettings.OwnDeveloperPwd.ToString <> "" Then
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(ActiveChannelAdvSettings.OwnDeveloperPwd, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10 TJS 02/04/14
                                        Else
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_SELLER_ORDER_ID & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10 TJS 02/04/14
                                        End If

                                    End If


                                Case 2 ' OrderShipped
                                    ' check response status
                                    If ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status", XMLNSManCAOrders).ToLower = "success" Then ' TJS 02/04/14
                                        If ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "ResultData", XMLNSManCAOrders).ToLower = "true" Then ' TJS 02/04/14
                                            ' yes - status update successful, save 
                                            If Not bInhibitWebPosts Then ' TJS 19/08/10
                                                m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor OrderShipped Update Successful for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), response content " & XMLResponse.ToString) ' TJS 04/10/10
                                            End If
                                            ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                                                "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", "DeleteLerrynImportExportActionStatus_DEV000221"}}, _
                                                Interprise.Framework.Base.Shared.TransactionType.None, "Channel Advisor OrderShipped", False)

                                        Else
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Channel Advisor OrderShipped failed to update " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status", XMLNSManCAOrders) & " for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "ResultData", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10 TJS 02/04/14
                                        End If
                                        bChannelAdvStatusUpdatesToSend = False
                                    Else
                                        'ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10
                                        'FA 13/12/10 send error details but mask developer password FA 14/12/10 modified to check either their dev passwordd or ours if their pwd is blank
                                        'ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********").Replace(ActiveChannelAdvSettings.OwnDeveloperPwd, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10
                                        If ActiveChannelAdvSettings.OwnDeveloperPwd.ToString <> "" Then
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(ActiveChannelAdvSettings.OwnDeveloperPwd, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10 TJS 02/04/14
                                        Else
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_ORDER_SHIPPED & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10 TJs 02/04/14
                                        End If
                                    End If

                                Case 3 ' SubmitOrderShipmentList
                                    If ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Status", XMLNSManCAOrders).ToLower = "success" Then ' TJS 02/04/14
                                        If ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "ResultData", XMLNSManCAOrders).ToLower.Contains("false") = False Then ' TJS 02/04/14
                                            ' yes - status update successful, save 
                                            If Not bInhibitWebPosts Then ' TJS 19/08/10
                                                m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor SubmitOrderShipmentList Update Successful for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), response content " & XMLResponse.ToString) ' TJS 04/10/10
                                            End If

                                        Else
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Channel Advisor SubmitOrderShipmentList failed to update " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Status", XMLNSManCAOrders) & " for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "ResultData", XMLNSManCAOrders), XMLStatusUpdateFile.ToString & vbCrLf & vbCrLf & "Response was " & vbCrLf & XMLResponse.ToString) ' TJS 04/10/10 TJS 02/04/14

                                        End If
                                        ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                                            "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", "DeleteLerrynImportExportActionStatus_DEV000221"}}, _
                                            Interprise.Framework.Base.Shared.TransactionType.None, "Channel Advisor OrderShipped", False) 'TJS 01/05/14
                                        bChannelAdvStatusUpdatesToSend = False
                                    Else
                                        'FA 13/12/10 send error details but mask developer password FA 14/12/10 modified to check either their dev passwordd or ours if their pwd is blank
                                        If ActiveChannelAdvSettings.OwnDeveloperPwd.ToString <> "" Then
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(ActiveChannelAdvSettings.OwnDeveloperPwd, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10 TJS 02/04/14
                                        Else
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10 TJS 02/04/14
                                        End If
                                        'ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Update rejected (" & XMLStatusUpdateFile.ToString.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********").Replace(ActiveChannelAdvSettings.OwnDeveloperPwd, "********") & ") for Account " & ActiveChannelAdvSettings.AccountName & " (" & ActiveChannelAdvSettings.AccountID & "), Status Code " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Status", XMLNSManCAOrders) & ", message " & ImportExportStatusFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SUBMIT_ORDER_SHIPMENT_LIST & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/10/10
                                    End If

                            End Select

                        Else
                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                        End If

                    Else
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                    End If

                Else
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvStatusFile", "Unexpected Account ID " & strAccountID & " in submission file, expected " & ActiveChannelAdvSettings.AccountID, XMLStatusUpdateFile.ToString)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendChannelAdvStatusFile", ex)

        End Try

    End Sub

End Module
