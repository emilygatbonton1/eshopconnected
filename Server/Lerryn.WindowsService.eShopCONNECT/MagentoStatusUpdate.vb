' eShopCONNECT for Connected Business - Windows Service
' Module: MagentoStatusUpdate.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'
'       © 2012 - 2014  Lerryn Business Solutions Ltd
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
' Last Updated - 01 May 2014

Imports System.IO
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.Enum
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports Lerryn.Facade.ImportExport

Module MagentoStatusUpdate

    Private MagentoConnection As Lerryn.Facade.ImportExport.MagentoSOAPConnector = Nothing ' TJS 18/03/11
    Private iMagentoXMLMessageType As Integer

    Public Sub DoMagentoOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveMagentoSettings As MagentoSettings, ByRef RowID As Integer, _
        ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Copied from eShopCONNECT for Tradepoint
        ' 27/09/10 | TJS             | 2010.1.02 | Modified to pick up tracking number from Shipping Module
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for multiple tracking numbers per shipment
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Shipments also being done from Invoice
        ' 10/08/12 | TJS             | 2012.1.12 | Added missing Namespace manager references for MAgento returned XML processins
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to prevent errors if SourcePurchaseID_DEV000221 is null
        ' 28/07/13 | TJS             | 2013.1.31 | Tidied progress log message text and added error messages if unable to create Magento Shipment or add tracking numbers
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to pass items shipped on partial shipments as a ShipmentInvoiceItems array and include 
        '                                        | zero quantities as Magento V2 CreateInvoice API needs this to prevent invoicing items not yet shipped
        ' 20/11/13 | TJS             | 2013.4.00 | Corrected reading of CarrierCode to CarrierDescription on the ShipmentView 
        '                                        | and checked for valid carriers when getting shipment details
        ' 04/01/14 | TJS             | 2013.4.03 | Corrected extracting of Shipment ID when API is WSI Compliant
        ' 06/02/14 | TJs             | 2013.4.08 | Modified to always pass details of Items shipped to Create Shipment and Create Invoice as MAgento V2 WSI API requires this
        ' 20/02/14 | TJS             | 2013.4.10 | Added log message if unable to extract Shipment id
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to mark sales order not found records as complete
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable ' TJS 10/08/12
        Dim strSourceCarrierCode As String, strSourceCarrierClass As String, strTrackingNumbers As String()() ' TJS 27/09/10 
        Dim strSourceCarrierClassCode As String, strXML As String, strTemp As String, strInvoiceCode As String
        Dim iEntryRowID As Integer, iCheckLoop As Integer, iShipmentPtr As Integer ' TJS 09/07/11
        Dim bAllItemsDelivered As Boolean, bSomeItemsDelivered As Boolean
        Dim dataItemsShipped As MagentoSOAPConnector.ShipmentInvoiceItems() ' TJS 13/11/13

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
                        ' nothing to update in Magento for new Order imported
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
                        ' do we have a Magento Order Increment ID (held in ImporterMerchantOrderRef field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status - get invoice detail (all rows) as order may not have QuantityShipped set
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerInvoiceDetail.TableName, _
                                   "ReadCustomerInvoiceDetailImportExport_DEV000221", AT_SOURCE_INVOICE_CODE, SalesOrderCode}}, _
                                   Interprise.Framework.Base.Shared.ClearType.Specific)

                                ' have all items been delivered ?
                                bAllItemsDelivered = True
                                bSomeItemsDelivered = False
                                strInvoiceCode = "" ' TJS 09/07/11
                                ReDim dataItemsShipped(-1) ' TJS 13/11/13 TJS 06/02/14
                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1
                                    strInvoiceCode = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).InvoiceCode ' TJS 09/07/11
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0 Then
                                        bSomeItemsDelivered = True
                                    End If
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityOrdered <> ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped Then
                                        bAllItemsDelivered = False
                                        'Exit For  
                                    End If
                                    ' start of code moved TJS 06/02/14
                                    If Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then ' TJS 09/03/13 TJS 13/11/13
                                        ' yes, add to items shipped
                                        ReDim Preserve dataItemsShipped(dataItemsShipped.Length) ' TJS 13/11/13
                                        dataItemsShipped(dataItemsShipped.Length - 1).OrderItemID = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).SourcePurchaseID_DEV000221 ' TJS 13/11/13
                                        dataItemsShipped(dataItemsShipped.Length - 1).QuantityShipped = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped ' TJS 13/11/13
                                    End If
                                    ' end of code moved TJS 06/02/14
                                Next

                                strSourceCarrierCode = ""
                                strSourceCarrierClass = ""
                                strSourceCarrierClassCode = ""
                                If ActiveSource.EnableDeliveryMethodTranslation Then
                                    ImportExportStatusFacade.TranslateDeliveryMethodFromIS(ActiveSource.SourceCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodGroup, strSourceCarrierCode, strSourceCarrierClass, strSourceCarrierClassCode)
                                Else
                                    strSourceCarrierCode = ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode
                                End If
                                If bAllItemsDelivered Then
                                    ' all items delivered, , create Magento Shipment 
                                    OpenMagentoStatusConnection(ActiveSource, ActiveMagentoSettings)
                                    If MagentoConnection.LoggedIn Then
                                        If MagentoConnection.CreateSalesOrderShipment(ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221, dataItemsShipped, "Order Shipped", False, False) Then ' TJS 13/11/13
                                            ' save XML as a record
                                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = MagentoConnection.PostedXML
                                            ' get Shipment ID for Add Tracking Number
                                            XMLNameTabMagento = New System.Xml.NameTable ' TJS 10/08/12
                                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 10/08/12
                                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 10/08/12
                                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 10/08/12
                                            If MagentoConnection.V2SoapAPIWSICompliant And MagentoConnection.V2SoapAPIResponse Then ' TJS 13/11/13
                                                strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:salesOrderShipmentCreateResponseParam/result", XMLNSManMagento) ' TJS 13/11/13 TJS 04/01/14
                                            ElseIf MagentoConnection.V2SoapAPIResponse Then ' TJS 13/11/13
                                                strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:salesOrderShipmentCreateResponse/shipmentIncrementId", XMLNSManMagento) ' TJS 13/11/13
                                            Else
                                                strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento) ' TJS 10/08/12
                                            End If
                                            If "" & strTemp = "" Then ' TJS 11/02/14
                                                m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "Unable to extract Magento Shipment ID from CreateSalesOrderShipment response - sent " & vbCrLf & vbCrLf & MagentoConnection.PostedXML & vbCrLf & vbCrLf & "response was " & MagentoConnection.ReturnedData) ' TJS 11/02/14
                                            End If
                                            ' get tracking numbers for shipment
                                            strTrackingNumbers = ImportExportStatusFacade.GetRows(New String() {"TrackingNumber", "CarrierDescription", "ServiceCode"}, "ShipmentView", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & strInvoiceCode & "') ORDER BY ShippingDate DESC") ' TJS 18/03/11 TJS 09/07/11 TJS 10/08/12 TJS 20/11/13
                                            For Each TrackingNumber As String() In strTrackingNumbers ' TJS 18/03/11
                                                If "" & TrackingNumber(0) <> "" Then ' TJS 27/09/10
                                                    ' check if carrier is recognised by Magento
                                                    If TrackingNumber(1).ToLower <> "dhl" And TrackingNumber(1).ToLower <> "dhlint" And TrackingNumber(1).ToLower <> "fedex" And _
                                                        TrackingNumber(1).ToLower <> "ups" And TrackingNumber(1).ToLower <> "usps" Then ' TJS 20/11/13
                                                        ' no, set as custom
                                                        TrackingNumber(1) = "custom" ' TJS 20/11/13
                                                    End If
                                                    If Not MagentoConnection.AddTrackingNoToShipment(strTemp, TrackingNumber(1).ToLower, TrackingNumber(2), TrackingNumber(0)) Then ' TJS 27/09/10 TJS 10/08/12
                                                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "Unable to add Tracking Number to Magento Shipment - XML sent to Magento was " & MagentoConnection.PostedXML & vbCrLf & vbCrLf & "Response was " & MagentoConnection.LastError) ' TJS 28/07/13
                                                    End If
                                                End If
                                            Next
                                            If Not ActiveMagentoSettings.CardAuthAndCaptureWithOrder Then ' TJS 01/05/14
                                                If Not MagentoConnection.CreateSalesOrderInvoice(ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221, dataItemsShipped, "", False, False) Then ' TJS 13/11/13 TJS 06/02/14
                                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "Unable to create Magento Invoice - XML sent to Magento was " & MagentoConnection.PostedXML & vbCrLf & vbCrLf & "Response was " & MagentoConnection.LastError) ' TJS 06/02/14
                                                End If
                                            End If
                                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is full shipment
                                        Else
                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "Unable to create Magento Sales Order Shipment - XML sent to Magento was " & MagentoConnection.PostedXML & vbCrLf & vbCrLf & "Response was " & MagentoConnection.LastError) ' TJS 28/07/13
                                        End If
                                    End If
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                                ElseIf bSomeItemsDelivered Then
                                    ' partial delivery only, does Magento API support Partial Shipments ?
                                    If ActiveMagentoSettings.APISupportsPartialShipments Then
                                        ' yes, create Magento Shipment 
                                        OpenMagentoStatusConnection(ActiveSource, ActiveMagentoSettings)
                                        If MagentoConnection.LoggedIn Then
                                            If MagentoConnection.CreateSalesOrderShipment(ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221, dataItemsShipped, "Part Shipment", False, False) Then ' TJS 13/11/13
                                                ' save XML as a record
                                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = MagentoConnection.PostedXML
                                                ' get Shipment ID for Add Tracking Number
                                                XMLNameTabMagento = New System.Xml.NameTable ' TJS 10/08/12
                                                XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 10/08/12
                                                XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 10/08/12
                                                XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 10/08/12
                                                If MagentoConnection.V2SoapAPIWSICompliant And MagentoConnection.V2SoapAPIResponse Then ' TJS 13/11/13
                                                    strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:salesOrderShipmentCreateResponseParam/result", XMLNSManMagento) ' TJS 13/11/13 TJS 04/01/14
                                                ElseIf MagentoConnection.V2SoapAPIResponse Then ' TJS 13/11/13
                                                    strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:salesOrderShipmentCreateResponse/shipmentIncrementId", XMLNSManMagento) ' TJS 13/11/13
                                                Else
                                                    strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento) ' TJS 10/08/12
                                                End If
                                                ' get tracking numbers for shipment
                                                strTrackingNumbers = ImportExportStatusFacade.GetRows(New String() {"TrackingNumber", "CarrierDescription", "ServiceCode"}, "ShipmentView", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & strInvoiceCode & "') ORDER BY ShippingDate DESC") ' TJS 18/03/11 TJS 09/07/11 TJS 10/08/12 TJS 20/11/13
                                                For Each TrackingNumber As String() In strTrackingNumbers ' TJS 18/03/11
                                                    If "" & TrackingNumber(0) <> "" Then ' TJS 27/09/10
                                                        ' check if carrier is recognised by Magento
                                                        If TrackingNumber(1).ToLower <> "dhl" And TrackingNumber(1).ToLower <> "dhlint" And TrackingNumber(1).ToLower <> "fedex" And _
                                                            TrackingNumber(1).ToLower <> "ups" And TrackingNumber(1).ToLower <> "usps" Then ' TJS 20/11/13
                                                            ' no, set as custom
                                                            TrackingNumber(1) = "custom" ' TJS 20/11/13
                                                        End If
                                                        If Not MagentoConnection.AddTrackingNoToShipment(strTemp, TrackingNumber(1).ToLower, TrackingNumber(2), TrackingNumber(0)) Then ' TJS 27/09/10 TJS 10/08/12
                                                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "Unable to add Tracking Number to Magento Shipment - XML sent to Magento was " & MagentoConnection.PostedXML & vbCrLf & vbCrLf & "Response was " & MagentoConnection.LastError) ' TJS 28/07/13
                                                        End If
                                                    End If
                                                Next
                                                If Not ActiveMagentoSettings.CardAuthAndCaptureWithOrder Then ' TJS 01/05/14
                                                    If Not MagentoConnection.CreateSalesOrderInvoice(ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221, dataItemsShipped, "Part Shipment", False, False) Then ' TJS 13/11/13 TJS 06/02/14
                                                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "Unable to create Magento Invoice - XML sent to Magento was " & MagentoConnection.PostedXML & vbCrLf & vbCrLf & "Response was " & MagentoConnection.LastError) ' TJS 06/02/14
                                                    End If
                                                End If
                                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 2 ' 2 is partial shipment
                                            Else
                                                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "Unable to create Magento Sales Order Shipment - XML sent to Magento was " & MagentoConnection.PostedXML & vbCrLf & vbCrLf & "Response was " & MagentoConnection.LastError) ' TJS 28/07/13
                                            End If
                                        End If
                                    End If
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                                Else
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                    strXML = ""
                                End If

                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If


                    Case Else ' TJS 19/08/10
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10

                End Select

                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Magento Order Status " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                       " - No XML created.") ' TJS 28/07/13
                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Magento Order Status " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                        " - XML created " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221) ' TJS 28/07/13
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 02/04/14

            End If

        Catch ex As Exception
            ImportExportStatusFacade.SendErrorEmail(ActiveSource.XMLConfig, "DoMagentoOrderStatusUpdate", ex)

        End Try
    End Sub

    Public Sub OpenMagentoStatusConnection(ByVal ActiveSource As SourceSettings, ByVal ActiveMagentoSettings As MagentoSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Copied from eShopCONNECT for Tradepoint
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version 
        ' 02/12/11 | TJS             | 2011.2.00 | Corrected function name in error messages and set bMagentoStatusUpdateConnected
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to cater for Magento login potentially containing XML entities
        ' 15/04/13 | TJS             | 2013.1.10 | Removed setting of bMagentoStatusUpdateConnected as replaced by a function
        ' 30/09/13 | TJS             | 2013.3.02 | Modified to decode Config XML Value such as password etc 
        ' 02/10/13 | TJS             | 2013.3.03 | Moved decoding of Congif XML values to ImportExportRule LoadXMLConfig
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2SoapAPIWSICompliant and to cater for multiple API connections
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            If MagentoConnection Is Nothing Then
                MagentoConnection = New Lerryn.Facade.ImportExport.MagentoSOAPConnector() ' TJS 18/03/11
            End If
            If Not MagentoConnection.LoggedIn OrElse MagentoConnection.APIURL <> ActiveMagentoSettings.APIURL Then ' TJS 13/11/13
                If MagentoConnection.LoggedIn Then ' TJS 13/11/13
                    MagentoConnection.Logout() ' TJS 13/11/13
                End If
                MagentoConnection.V2SoapAPIWSICompliant = ActiveMagentoSettings.V2SoapAPIWSICompliant ' TJS 13/11/13
                MagentoConnection.MagentoVersion = ActiveMagentoSettings.MagentoVersion ' TJS 13/11/13
                MagentoConnection.LerrynAPIVersion = ActiveMagentoSettings.LerrynAPIVersion ' TJS 13/11/13
                If MagentoConnection.Login(ActiveMagentoSettings.APIURL, m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveMagentoSettings.APIUser), _
                   m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveMagentoSettings.APIPwd)) Then ' TJS 18/03/11 TJS 03/04/13 TJS 30/09/13 TJS 02/10/13
                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "OpenMagentoStatusConnection", MagentoConnection.LastError, "") ' TJS 18/03/11 TJS 02/12/11
                    MagentoConnection = Nothing ' TJS 02/12/11
                End If
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "OpenMagentoStatusConnection", ex) ' TJS 02/12/11

        End Try

    End Sub

    Public Sub CloseMagentoStatusConnection(ByVal ActiveSource As SourceSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Copied from eShopCONNECT for Tradepoint
        ' 02/12/11 | TJS             | 2011.2.00 | Corrected function name in error messages
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to clear connected flag
        ' 15/04/13 | TJS             | 2013.1.10 | Removed setting of bMagentoStatusUpdateConnected as replaced by a function
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            MagentoConnection.Logout()
            MagentoConnection = Nothing

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "CloseMagentoStatusConnection", ex)

        End Try

    End Sub

    Friend Function MagentoStatusUpdateConnected() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/04/13 | TJS             | 2013.1.10 | Function added to replace bMagentoStatusUpdateConnected 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MagentoConnection IsNot Nothing Then
            Return MagentoConnection.LoggedIn
        Else
            Return False
        End If

    End Function

    Public Function MagentoXMLDate(ByVal DateToUse As Date) As String

        Dim strMagentoXMLDate As String

        strMagentoXMLDate = DateToUse.Year & "-" & Right("00" & DateToUse.Month, 2) & "-" & Right("00" & DateToUse.Day, 2)
        strMagentoXMLDate = strMagentoXMLDate & "T" & Right("00" & DateToUse.Hour, 2) & ":" & Right("00" & DateToUse.Minute, 2)
        strMagentoXMLDate = strMagentoXMLDate & ":" & Right("00" & DateToUse.Second, 2)
        Return strMagentoXMLDate

    End Function

End Module
