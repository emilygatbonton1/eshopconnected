' eShopCONNECT for Connected Business - Windows Service
' Module: eBayStatusUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 02 April 2014

Imports System.IO
Imports System.Xml.Linq
Imports System.Xml.XPath
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.SourceConfig

Module eBayStatusUpdate

    Private eBayConnection As Lerryn.Facade.ImportExport.eBayXMLConnector
    Public bEBayStatusUpdateConnected As Boolean

    Public Sub DoEBayOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveEBaySettings As eBaySettings, ByRef RowID As Integer, ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 28/07/13 | TJS             | 2013.1.31 | Tidied progress log message text
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to mark sales order not found records as complete
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim eBayShippingDetails() As Lerryn.Facade.ImportExport.eBayXMLConnector.eBayShipmentDetails
        Dim strShipmentDetails As String()(), strEBayUser As String, strEBayFeedbackType As String
        Dim strXML As String, strTemp As String, strInvoiceCode As String, strEBayFeedbackMessage As String
        Dim iEntryRowID As Integer, iCheckLoop As Integer, iCarrierLoop As Integer
        Dim bAllItemsDelivered As Boolean, bSomeItemsDelivered As Boolean, bCarrierFound As Boolean

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
                        ' nothing to update in eBay for new Order imported
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
                        ' do we have a eBay Order Increment ID (held in ImporterMerchantOrderRef field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then
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
                                        'Exit For  
                                    End If
                                Next

                                If bAllItemsDelivered Then
                                    ' all items delivered, , create eBay Shipment 
                                    OpenEBayStatusConnection(ActiveSource, ActiveEBaySettings)
                                    strTemp = ""
                                    ReDim eBayShippingDetails(0)
                                    ' get tracking numbers and carriers for shipment
                                    strShipmentDetails = ImportExportStatusFacade.GetRows(New String() {"TrackingNumber", "CarrierCode"}, "Shipment", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & strInvoiceCode & "') ORDER BY ShippingDate DESC") ' TJS 18/03/11 TJS 09/07/11
                                    For Each strShipment As String() In strShipmentDetails
                                        If "" & strShipment(0) <> "" And "" & strShipment(1) <> "" Then
                                            If eBayShippingDetails(eBayShippingDetails.Length - 1).TrackingNumber <> "" Then
                                                ReDim Preserve eBayShippingDetails(eBayShippingDetails.Length)
                                            End If
                                            eBayShippingDetails(eBayShippingDetails.Length - 1).TrackingNumber = strShipment(0)
                                            eBayShippingDetails(eBayShippingDetails.Length - 1).CarrierUsed = strShipment(1)
                                        End If
                                    Next
                                    If (Not ImportExportStatusDataset.CustomerSalesOrder(0).IsSourceFeedbackType_DEV000221Null AndAlso ImportExportStatusDataset.CustomerSalesOrder(0).SourceFeedbackType_DEV000221 <> "") Or _
                                        (Not ImportExportStatusDataset.CustomerSalesOrder(0).IsSourceFeedbackMessage_DEV000221Null AndAlso ImportExportStatusDataset.CustomerSalesOrder(0).SourceFeedbackMessage_DEV000221 <> "") Then
                                        strEBayUser = ImportExportStatusFacade.GetField("ImportSourceBuyerID_DEV000221", "Customer", "CustomerCode = '" & ImportExportStatusDataset.CustomerSalesOrder(0).BillToCode & "'")
                                    Else
                                        strEBayUser = ""
                                    End If
                                    If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsSourceFeedbackType_DEV000221Null Then
                                        strEBayFeedbackType = ImportExportStatusDataset.CustomerSalesOrder(0).SourceFeedbackType_DEV000221
                                    Else
                                        strEBayFeedbackType = ""
                                    End If
                                    If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsSourceFeedbackMessage_DEV000221Null Then
                                        strEBayFeedbackMessage = ImportExportStatusDataset.CustomerSalesOrder(0).SourceFeedbackMessage_DEV000221
                                    Else
                                        strEBayFeedbackMessage = ""
                                    End If
                                    If eBayConnection.CompleteSale(ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221, strEBayFeedbackType, _
                                        strEBayFeedbackMessage, strEBayUser, "", eBayShippingDetails, True) Then
                                        ' save XML as a record
                                        strTemp = "<eBayShipment>"
                                        For Each shipment As Lerryn.Facade.ImportExport.eBayXMLConnector.eBayShipmentDetails In eBayShippingDetails
                                            If shipment.TrackingNumber <> "" And shipment.CarrierUsed <> "" Then
                                                strTemp = strTemp & "<Shipment>"
                                                strTemp = strTemp & "<TrackingNumber>" & shipment.TrackingNumber & "</TrackingNumber>"
                                                strTemp = strTemp & "<Carrier>" & shipment.CarrierUsed & "</Carrier>"
                                                strTemp = strTemp & "</Shipment>"
                                            End If
                                        Next
                                        strTemp = strTemp & "</eBayShipment>"
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = strTemp

                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is full shipment
                                    Else
                                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoEBayOrderStatusUpdate", "Unable to complete Order " & SalesOrderCode & ", error details " & eBayConnection.LastError) ' TJS 13/11/13
                                    End If

                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                                ElseIf bSomeItemsDelivered Then
                                    ' partial delivery not supported
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                                Else
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                    strXML = ""
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
                    m_ImportExportConfigFacade.WriteLogProgressRecord("eBay Order Status " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                       " - No XML created.") ' TJS 28/07/13
                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("eBay Order Status " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                        " - XML created " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221) ' TJS 28/07/13
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoEBayOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 02/04/14

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoEBayOrderStatusUpdate", ex)

        End Try

    End Sub

    Public Sub OpenEBayStatusConnection(ByVal ActiveSource As SourceSettings, ByVal ActiveEBaySettings As eBaySettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bUseEBaySandbox As Boolean

        Try
            If eBayConnection Is Nothing Then
                bUseEBaySandbox = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "UseEBaySandbox", "NO").ToUpper = "YES")
                eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(m_ImportExportConfigFacade, ActiveEBaySettings, bUseEBaySandbox)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "OpenEBayStatusConnection", ex)

        End Try

    End Sub

    Public Sub CloseEBayStatusConnection(ByVal ActiveSource As SourceSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            eBayConnection = Nothing

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "CloseEBayStatusConnection", ex)

        End Try

    End Sub

End Module
