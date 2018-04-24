' eShopCONNECT for Connected Business - Windows Service
' Module: AmazonStatusUpdate.vb
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

Module AmazonStatusUpdate

    Private XMLStatusUpdateFile As XDocument
    Private iAmazonXMLMessageType As Integer
    Private xmlStatusUpdateNode As XElement
    Private rowAmazonMessages As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonMessageSettings_DEV000221Row ' TJS 05/07/09
    Public bAmazonStatusUpdatesToSend As Boolean

    Public Sub DoAmazonOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer, ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/03/09 | TJS             | 2009.1.09 | Function added
        ' 17/03/09 | TJs             | 2009.1.10 | Correded XML build error
        ' 26/08/09 | TJS             | 2009.3.05 | Corrected error on processing of partial deliveries and 
        '                                        | modified to use Invoice Detail for Quantity Shipped
        ' 29/09/09 | TJS             | 2009.3.07 | Modified to create Message element here for consistency 
        '                                        | instead of in AddToAmazonStatusFile
        ' 27/11/09 | TJS             | 2009.3.09 | Corrected error when checking for status rows to skip
        ' 27/02/10 | TJS             | 2010.0.06 | Corrected MerchantOrderItemID value for consistency 
        ' 15/06/10 | TJS             | 2010.0.07 | Modified to prevent error when logging messages if related rows are skipped
        ' 19/08/10 | TJS             | 2010.1.00 | Added Sales Order Number to log message and modified to ignore 
        '                                        | blank MerchantOrderIDs as well as null values
        ' 27/09/10 | TJS             | 2010.1.02 | Modified to pick up tracking number from Shipping Module
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Shipments also being done from Invoice
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 21/07/13 | TJS             | 2013.1.30 | Modified to prevent errors if SourcePurchaseID_DEV000221 is null
        ' 28/07/13 | TJS             | 2013.1.31 | Tidied progress log message text
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to mark sales order not found records as complete
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSourceCarrierCode As String
        Dim strSourceCarrierDescription As String
        Dim strSourceCarrierClassCode As String
        Dim XMLUpdate As XElement, xmlUpdateNode As XElement, xmlItemNode As XElement, xmlFulfillmentNode As XElement ' TJS 29/09/09
        Dim strSalesOrderNumber As String, strTrackingNumber As String, iCheckLoop As Integer, iEntryRowID As Integer ' TJS 27/11/09 TJS 15/06/10 TJS 27/09/10
        Dim strInvoiceCode As String
        Dim bTranslationFound As Boolean

        Try
            iEntryRowID = RowID ' TJS 15/06/10
            ' for some reason, POCode field seems to cause constraint error if populated so disable constraints to allow sales order to load
            ImportExportStatusDataset.EnforceConstraints = False
            strSalesOrderNumber = "" ' TJS 27/11/09
            For iCheckLoop = 0 To SalesOrderCode.Length - 1 ' TJS 27/11/09
                If Asc(SalesOrderCode.Substring(iCheckLoop, 1)) >= Asc("0") And Asc(SalesOrderCode.Substring(iCheckLoop, 1)) <= Asc("9") Then ' TJS 27/11/09
                    strSalesOrderNumber = strSalesOrderNumber & SalesOrderCode.Substring(iCheckLoop, 1) ' TJS 27/11/09
                End If
            Next

            ' get order record
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrder.TableName, _
               "ReadCustomerSalesOrder", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.CustomerSalesOrder.Count > 0 Then
                Select Case ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221
                    Case "100" ' New Order

                        ' do we have a Amazon Order ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status
                                XMLUpdate = New XElement("Message") ' TJS 29/09/09
                                xmlUpdateNode = New XElement("OrderAcknowledgement")
                                xmlUpdateNode.Add(New XElement("AmazonOrderID", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221))
                                xmlUpdateNode.Add(New XElement("MerchantOrderID", SalesOrderCode))
                                xmlUpdateNode.Add(New XElement("StatusCode", "Success"))

                                ' get order detail (all rows)
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrderDetail.TableName, _
                                   "ReadCustomerSalesOrderDetailImportExport_DEV000221", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerSalesOrderDetail.Count - 1
                                    If Not ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then ' TJS 21/07/13
                                        xmlItemNode = New XElement("Item")
                                        xmlItemNode.Add(New XElement("AmazonOrderItemCode", ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).SourcePurchaseID_DEV000221))
                                        xmlItemNode.Add(New XElement("MerchantOrderItemID", strSalesOrderNumber & ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).LineNum)) ' TJS 27/02/10
                                        xmlUpdateNode.Add(xmlItemNode)
                                    End If
                                Next
                                XMLUpdate.Add(xmlUpdateNode) ' TJS 17/03/09 TJS 29/09/09

                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 1 ' 1 is Order Acknowledgement
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
                                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).OrderStatus_DEV000221 = "105" Then ' TJS 27/11/09
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
                        If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then ' TJS 27/11/09
                            ' yes, set row pointer return value
                            RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1 ' TJS 27/11/09
                        End If


                    Case "105" ' New Item
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "200" ' Back Order
                        If ImportExportStatusDataset.CustomerSalesOrder.Count > 0 Then
                            ' do we have a Amazon Order ID (held in MerchantOrderID_DEV000221 field) ?
                            If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                                If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                    ' yes, send status, get order detail (all rows)
                                    ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrderDetail.TableName, _
                                       "ReadCustomerSalesOrderDetailImportExport_DEV000221", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)




                                Else
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                            End If
                        End If
                    Case "205" ' New Back Order Item
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True


                    Case "305" ' Item Order Qty changed
                        ' do we have a Amazon Order ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, do we have details in XML field ?
                                If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                                    ' yes, send status
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Processing Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record with trigger XML " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 & _
                                        " for Sales Order " & SalesOrderCode & ", Order Line Number " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)
                                    ' get order detail (modifed row)
                                    ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrderDetail.TableName, _
                                       "ReadCustomerSalesOrderDetailImportExport_DEV000221", AT_SALES_ORDER_CODE, SalesOrderCode, AT_LINE_NUM, _
                                       CStr(ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)}}, _
                                       Interprise.Framework.Base.Shared.ClearType.Specific)

                                    If XMLUpdate IsNot Nothing Then
                                        ' save XML ready for sending
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString
                                        ' set message type and mark record as having XML to send
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is Order Adjustment
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                    End If
                                Else
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record found with blank trigger XML for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If


                    Case "310" ' Item Order Price changed

                        ' do we have a Amazon Order ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, do we have details in XML field ?
                                If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                                    ' yes, send status
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Processing Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record with trigger XML " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 & _
                                        " for Sales Order " & SalesOrderCode & ", Order Line Number " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)
                                    ' get order detail (modifed row)
                                    ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrderDetail.TableName, _
                                       "ReadCustomerSalesOrderDetailImportExport_DEV000221", AT_SALES_ORDER_CODE, SalesOrderCode, AT_LINE_NUM, _
                                       CStr(ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)}}, _
                                       Interprise.Framework.Base.Shared.ClearType.Specific)

                                    If XMLUpdate IsNot Nothing Then
                                        ' save XML ready for sending
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString
                                        ' set message type and mark record as having XML to send
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is Order Adjustment
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                    End If
                                Else
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record found with blank trigger XML for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If


                    Case "350" ' Item Deleted

                        ' do we have a Amazon Order ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, do we have details in XML field ?
                                If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                                    ' yes, send status
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Processing Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record with trigger XML " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 & _
                                        " for Sales Order " & SalesOrderCode & ", Order Line Number " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)

                                    If XMLUpdate IsNot Nothing Then
                                        '' save XML ready for sending
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString
                                        ' set message type and mark record as having XML to send
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is Order Adjustment
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                    End If
                                Else
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record found with blank trigger XML for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If


                    Case "500" ' Order Voided

                        ' do we have a Amazon Order ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, do we have details in XML field ?
                                If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                                    ' yes, send status
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Processing Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record with trigger XML " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 & _
                                        " for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)

                                    ' start of code added TJS 27/11/09
                                    XMLUpdate = New XElement("Message")
                                    xmlUpdateNode = New XElement("OrderAdjustment")
                                    xmlUpdateNode.Add(New XElement("AmazonOrderID", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221))

                                    ' get order detail (all rows)
                                    ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrderDetail.TableName, _
                                       "ReadCustomerSalesOrderDetailImportExport_DEV000221", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                    For iCheckLoop = 0 To ImportExportStatusDataset.CustomerSalesOrderDetail.Count - 1
                                        If Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then ' TJS 21/07/13
                                            xmlItemNode = New XElement("AdjustedItem")
                                            xmlItemNode.Add(New XElement("AmazonOrderItemCode", ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).SourcePurchaseID_DEV000221))
                                            xmlItemNode.Add(New XElement("MerchantAdjustmentItemID", strSalesOrderNumber & ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).LineNum))
                                            xmlItemNode.Add(New XElement("AdjustmentReason", "Abandoned"))
                                            xmlUpdateNode.Add(xmlItemNode)
                                        End If
                                    Next
                                    XMLUpdate.Add(xmlUpdateNode)

                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString
                                    ' set message type and mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 3 ' 3 is Order Adjustment
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                    ' end of code added TJS 27/11/09

                                Else
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record found with blank trigger XML for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If


                    Case "600" ' Order Shipped

                        ' do we have a Amazon Order ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status - get invoice detail (all rows) as order may not have QuantityShipped set
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerInvoiceDetail.TableName, _
                                   "ReadCustomerInvoiceDetailImportExport_DEV000221", AT_SOURCE_INVOICE_CODE, SalesOrderCode}}, _
                                   Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 26/08/09

                                strSourceCarrierCode = String.Empty
                                strSourceCarrierDescription = String.Empty
                                strSourceCarrierClassCode = String.Empty
                                bTranslationFound = False
                                If ActiveSource.EnableDeliveryMethodTranslation Then ' TJS 19/08/10
                                    strSourceCarrierCode = ImportExportStatusFacade.TranslateDeliveryMethodFromIS(ActiveSource.SourceCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodGroup)
                                    If Not String.IsNullOrWhiteSpace(strSourceCarrierCode) Then bTranslationFound = True
                                Else
                                    strSourceCarrierCode = ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode ' TJS 19/08/10
                                End If

                                If Not bTranslationFound Then
                                    Dim message As String = "Sales Order " & SalesOrderCode _
                                            & " - No source delivery method will be sent to Amazon because the sales order shipping method has no delivery method translation. " _
                                            & "To ensure accurate tracking information, kindly make the information updated in Amazon."
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoAmazonOrderStatusUpdate", message)
                                End If

                                XMLUpdate = New XElement("Message") ' TJS 29/09/09
                                xmlUpdateNode = New XElement("OrderFulfillment")
                                xmlUpdateNode.Add(New XElement("AmazonOrderID", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221))
                                xmlUpdateNode.Add(New XElement("MerchantFulfillmentID", strSalesOrderNumber)) ' TJS 17/03/09 TJS 27/11/09
                                xmlUpdateNode.Add(New XElement("FulfillmentDate", CreateAmazonXMLDate(Date.Now.ToUniversalTime)))
                                If ImportExportStatusFacade.GetField("ColumnName", "DataDictionaryColumn", "TableName = 'Shipment' AND ColumnName = 'TrackingNumber'") <> "" Then ' TJS 27/09/10

                                    strInvoiceCode = String.Empty
                                    If ImportExportStatusDataset.CustomerInvoiceDetail.Count > 0 Then ' TJS 09/07/11
                                        strInvoiceCode = ImportExportStatusDataset.CustomerInvoiceDetail(0).InvoiceCode
                                    End If

                                    strTrackingNumber = GetShipmentInformation(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, SalesOrderCode, strInvoiceCode, bTranslationFound, strSourceCarrierClassCode, strSourceCarrierDescription)

                                    If "" & strTrackingNumber <> "" Then ' TJS 27/09/10
                                        xmlFulfillmentNode = New XElement("FulfillmentData")
                                        If bTranslationFound Then
                                            xmlFulfillmentNode.Add(New XElement("CarrierCode", strSourceCarrierCode)) ' TJS 17/03/09 TJS 27/09/10
                                        Else
                                            xmlFulfillmentNode.Add(New XElement("CarrierName", strSourceCarrierDescription)) ' TJS 17/03/09 TJS 27/09/10
                                        End If

                                        xmlFulfillmentNode.Add(New XElement("ShippingMethod", strSourceCarrierClassCode)) ' TJS 17/03/09 TJS 27/09/10
                                        xmlFulfillmentNode.Add(New XElement("ShipperTrackingNumber", strTrackingNumber)) ' TJS 17/03/09 TJS 27/09/10
                                        xmlUpdateNode.Add(xmlFulfillmentNode)
                                    End If
                                End If

                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1 ' TJS 26/08/09
                                    If Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null _
                                        And (Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsQuantityShippedNull AndAlso ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0) Then ' TJS 21/07/13
                                        xmlItemNode = New XElement("Item")
                                        xmlItemNode.Add(New XElement("AmazonOrderItemCode", ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).SourcePurchaseID_DEV000221)) ' TJS 26/08/09
                                        xmlItemNode.Add(New XElement("MerchantFulfillmentItemID", strSalesOrderNumber & ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).LineNum)) ' TJS 26/08/09 TJS 27/11/09
                                        xmlItemNode.Add(New XElement("Quantity", Format(ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped, "0"))) ' TJS 26/08/09 TJS 27/11/09
                                        xmlUpdateNode.Add(xmlItemNode)
                                    End If
                                Next
                                XMLUpdate.Add(xmlUpdateNode) ' TJS 17/03/09 TJS 29/09/09

                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLMessageType_DEV000221 = 2 ' 2 is Order Fulfillment
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True
                        End If


                    Case Else
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                End Select

                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Amazon Order Status " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                       " - No XML created.") ' TJS 19/08/10 TJS 28/07/13
                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Amazon Order Status " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                        " - XML created " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221) ' TJS 19/08/10 TJS 28/07/13
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoAmazonOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 02/04/14

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoAmazonOrderStatusUpdate", ex)

        End Try

    End Sub

    Public Function GetShipmentInformation(ByVal ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByVal ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, SalesOrderCode As String, InvoiceCode As String, TranslationFound As Boolean, _
        ByRef ServiceCode As String, ByRef CarrierDescription As String) As String
        Dim shipmentInfo() As String = ImportExportStatusFacade.GetRow(New String() {"CarrierCode", "ServiceCode", "CarrierDescription", "UserModified", "TrackingNumber"}, "Shipment", "(SourceDocument = '" & SalesOrderCode & "' OR SourceDocument = '" & InvoiceCode & "') ORDER BY ShippingDate DESC")
        Dim shipmentCarrierCode As String = String.Empty
        Dim userModified As String = String.Empty
        Dim trackingNumber As String = String.Empty

        If shipmentInfo IsNot Nothing AndAlso shipmentInfo.Length = 5 Then
            shipmentCarrierCode = shipmentInfo(0)
            ServiceCode = shipmentInfo(1)
            CarrierDescription = shipmentInfo(2)
            userModified = shipmentInfo(3)
            trackingNumber = shipmentInfo(4)
        End If

        If TranslationFound Then
            If ActiveSource.EnableDeliveryMethodTranslation And Not String.IsNullOrWhiteSpace(trackingNumber) Then
                Dim shippingMethodCarrierCode As String = ImportExportStatusFacade.GetField("CarrierCode", "ShipmentShippingMethodDefault", "ShippingMethodCode ='" & ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode & "' AND WarehouseCode = '" & ImportExportStatusDataset.CustomerSalesOrder(0).WarehouseCode & "'")

                If Not String.Equals(shippingMethodCarrierCode, shipmentCarrierCode, StringComparison.OrdinalIgnoreCase) Then
                    Dim message As String = "Sales Order " & SalesOrderCode _
                        & " - Source delivery method will be sent to Amazon based on Delivery Method Translation. " _
                        & "But, the default carrier was changed manually by " & userModified & " in the shipment transaction." _
                        & "To ensure accurate tracking information is recorded, kindly make it updated in Amazon."
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "GetShipmentInformation", message)
                End If
            End If
        End If
        Return trackingNumber
    End Function

    Public Sub StartAmazonStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByVal AmazonXMLMessageType As Integer) ' TJS 05/07/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/03/09 | TJS             | 2009.1.09 | Function added
        ' 17/03/09 | TJS             | 2009.1.10 | Corrected xml build
        ' 05/07/09 | TJS             | 2009.3.00 | Modified to get Message ID from config table
        ' 27/11/09 | TJS             | 2009.3.09 | Completed code for Amazon SOAP Client
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Amazon document ID exceeding SQL int value range
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        '                                        | and corrected generation of Amazon Envelope namespaces
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
        Dim xmlUpdateNode As XElement, xmlDefaultNamespace As XNamespace, xmlXSINamespace As XNamespace ' TJS 22/03/13
        Dim strXMLMessageType As String, strFileType As String, sTemp As String, iLoop As Integer, bFileExists As Boolean ' TJS 27/11/09

        Try
            xmlDefaultNamespace = XNamespace.Get("") ' TJS 22/03/13
            xmlXSINamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") ' TJS 22/03/13
            XMLStatusUpdateFile = New XDocument

            xmlStatusUpdateNode = New XElement(xmlDefaultNamespace + "AmazonEnvelope") ' TJS 22/03/13
            xmlStatusUpdateNode.SetAttributeValue(XNamespace.Xmlns + "xsi", xmlXSINamespace.NamespaceName) ' TJS 17/03/09 TJS 22/03/13
            xmlStatusUpdateNode.SetAttributeValue(xmlXSINamespace + "noNamespaceSchemaLocation", "amzn-envelope.xsd") ' TJS 17/03/09 TJS 22/03/13
            xmlUpdateNode = New XElement("Header")
            xmlUpdateNode.Add(New XElement("DocumentVersion", "1.01"))
            xmlUpdateNode.Add(New XElement("MerchantIdentifier", ActiveAmazonSettings.MerchantToken)) ' TJS 22/03/13
            xmlStatusUpdateNode.Add(xmlUpdateNode)
            Select Case AmazonXMLMessageType
                Case 1
                    strXMLMessageType = "OrderAcknowledgement"
                    strFileType = "_POST_ORDER_ACKNOWLEDGEMENT_DATA_" ' TJS 27/11/09
                Case 2
                    strXMLMessageType = "OrderFulfillment" ' TJS 17/03/09
                    strFileType = "_POST_ORDER_FULFILLMENT_DATA_" ' TJS 27/11/09
                Case 3
                    strXMLMessageType = "OrderAdjustment" ' TJS 05/07/09
                    strFileType = "_POST_PAYMENT_ADJUSTMENT_DATA_" ' TJS 27/11/09
                Case Else
                    strXMLMessageType = ""
                    strFileType = "" ' TJS 27/11/09
            End Select
            xmlStatusUpdateNode.Add(New XElement("MessageType", strXMLMessageType)) ' TJS 17/03/09
            iAmazonXMLMessageType = AmazonXMLMessageType
            ' have the Amazon Message Settings been loaded ?
            If ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.Count = 0 Then ' TJS 05/07/09
                ' no, load them
                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.TableName, _
               "ReadLerrynImportExportAmazonMessageSettings_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 05/07/09
            End If
            ' find message settings for active Merchant ID
            rowAmazonMessages = ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.FindByMerchantID_DEV000221(ActiveAmazonSettings.MerchantToken) ' TJS 05/07/09 TJS 22/03/13
            ' did we find them? 
            If rowAmazonMessages Is Nothing Then ' TJS 05/07/09
                ' no, create settings for active Merchant ID starting at message id 1
                rowAmazonMessages = ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.NewLerrynImportExportAmazonMessageSettings_DEV000221Row ' TJS 05/07/09
                rowAmazonMessages.MerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken ' TJS 05/07/09 TJS 22/03/13
                rowAmazonMessages.NextMessageID_DEV000221 = 1 ' TJS 05/07/09
                ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.AddLerrynImportExportAmazonMessageSettings_DEV000221Row(rowAmazonMessages) ' TJS 05/07/09
            End If

            bFileExists = False ' TJS 27/11/09
            rowAmazonFile = Nothing ' TJS 09/07/11
            For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1 ' TJS 27/11/09
                If ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = "Current" Then ' TJS 27/11/09
                    rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop) ' TJS 27/11/09
                    bFileExists = True ' TJS 27/11/09
                    Exit For ' TJS 27/11/09
                End If
            Next
            If Not bFileExists Then ' TJS 27/11/09
                rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.NewLerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
                sTemp = m_ImportExportConfigFacade.GetField("SELECT ISNULL(MIN(CAST(AmazonDocumentID_DEV000221 AS bigint)), 0) FROM dbo.LerrynImportExportAmazonFiles_DEV000221 WHERE CAST(AmazonDocumentID_DEV000221 AS bigint) < 0", CommandType.Text, Nothing) ' TJS 27/11/09 TJS 09/07/11
                If "" & sTemp <> "" Then ' TJS 27/11/09
                    rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(CInt(sTemp) - 1) ' TJS 27/11/09
                Else
                    rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(-1) ' TJS 27/11/09
                End If
            End If
            rowAmazonFile.SiteCode_DEV000221 = ActiveAmazonSettings.AmazonSite ' TJS 27/11/09
            rowAmazonFile.MerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken ' TJS 27/11/09 TJS 22/03/13
            rowAmazonFile.FileName_DEV000221 = "Current" ' TJS 27/11/09
            rowAmazonFile.AmazonMessageType_DEV000221 = strFileType ' TJS 27/11/09
            rowAmazonFile.FileIsInputFromAmazon_DEV000221 = False ' TJS 27/11/09
            rowAmazonFile.FileSentToAmazon_DEV000221 = False ' TJS 27/11/09
            rowAmazonFile.ResponseReceived_DEV000221 = False ' TJS 27/11/09
            rowAmazonFile.Processed_DEV000221 = False ' TJS 27/11/09
            rowAmazonFile.FileContent_DEV000221 = XMLStatusUpdateFile.ToString ' TJS 27/11/09
            If Not bFileExists Then ' TJS 27/11/09
                ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.AddLerrynImportExportAmazonFiles_DEV000221Row(rowAmazonFile) ' TJS 27/11/09
            End If

            XMLStatusUpdateFile.Add(xmlStatusUpdateNode)

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartAmazonStatusFile", ex)

        End Try

    End Sub

    Public Sub AddToAmazonStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/03/09 | TJS             | 2009.1.09 | Function added
        ' 17/03/09 | TJS             | 2009.1.10 | Corrected XML build
        ' 05/07/09 | TJS             | 2009.3.00 | Modified to use Message ID from config table
        ' 29/09/09 | TJS             | 2009.3.07 | Modified to cater for Message element already being created for consistency 
        ' 27/11/09 | TJS             | 2009.3.09 | Completed code for Amazon SOAP Client
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStartingUpdateXML As String, strMessageXML As String, iInsertPosn As Integer ' TJS 29/09/09
        Dim iLoop As Integer ' TJS 27/11/09

        Try
            If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null Then
                ' get message XML
                strMessageXML = ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 ' TJS 29/09/09
                ' does mesage XML end with cr lf ?
                If Right(strMessageXML, 2) = vbCrLf Then ' TJS 27/11/09
                    ' yes, remove cr lf
                    strMessageXML = Left(strMessageXML, Len(strMessageXML) - 2) ' TJS 27/11/09
                End If
                ' Insert MessageID
                strMessageXML = Left(strMessageXML, 9) & "<MessageID>" & rowAmazonMessages.NextMessageID_DEV000221 & "</MessageID>" & Mid(strMessageXML, 10) ' TJS 29/09/09
                ' and save in DB 
                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).SendInMessageID_DEV000221 = CStr(rowAmazonMessages.NextMessageID_DEV000221) ' TJS 29/09/09
                For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1 ' TJS 27/11/09
                    If ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = "Current" Then ' TJS 27/11/09
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).SentInFileID_DEV000221 = _
                            ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).AmazonDocumentID_DEV000221 ' TJS 27/11/09
                        Exit For ' TJS 27/11/09
                    End If
                Next

                ' get current update XML
                strStartingUpdateXML = XMLStatusUpdateFile.ToString
                ' get position for inserting message
                iInsertPosn = InStr(strStartingUpdateXML, "</AmazonEnvelope>")
                ' insert mesage XML 
                strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & strMessageXML & Mid(strStartingUpdateXML, iInsertPosn) ' TJS 17/03/09 TJS 05/07/09 TJS 29/09/09
                ' update Amazon XML Message ID
                rowAmazonMessages.NextMessageID_DEV000221 = rowAmazonMessages.NextMessageID_DEV000221 + 1 ' TJS 05/07/09
                ' reload update XML
                XMLStatusUpdateFile = XDocument.Parse(strStartingUpdateXML)

                bAmazonStatusUpdatesToSend = True
            End If

            ' mark action record as XML Sent (change is committed to DB when status file successfully saved in SaveAmazonStatusFile)
            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddToAmazonStatusFile", ex)

        End Try

    End Sub

    Public Sub SaveAmazonStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/03/09 | TJS             | 2009.1.09 | Function added
        ' 17/03/09 | TJS             | 2009.1.10 | Corrected extraction of Merchant ID from XML
        ' 08/04/09 | TJS             | 2009.2.02 | Corrected Amazon file name
        ' 05/07/09 | TJS             | 2009.3.00 | Modified to save Message ID in config table
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to support direct connection to Amazon
        ' 27/11/09 | TJS             | 2009.3.09 | Completed code for Amazon SOAP Client
        ' 13/06/10 | TJs             | 2010.0.07 | Modified to check Inhibit Web Posts when loading Amazon files to send 
        ' 19/08/10 | TJS             | 2010.1.00 | Corrected InhibitWebPosts initialisation
        ' 06/04/11 | TJS             | 2011.0.08 | Disabled Amazon client to allow dll signing so Web SErvices will work
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Amazon document ID exceeding SQL int value range
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
        Dim writer As StreamWriter, sXMLFileName As String, strFileType As String, sTemp As String, strXMLcontent As String ' TJS 27/11/09
        Dim iLoop As Integer, bFileExists As Boolean, bInhibitWebPosts As Boolean ' TJS 27/11/09 TJs 13/06/10

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") ' TJs 13/06/10 TJS 19/08/10

            ' check submission Merchant ID is as expected
            If ImportExportStatusFacade.GetXMLElementText(XMLStatusUpdateFile, AMAZON_STATUS_UPDATE_HEADER & "/MerchantIdentifier") = ActiveAmazonSettings.MerchantToken Then ' TJS 17/03/09 TJS 22/03/13
                Select Case iAmazonXMLMessageType ' TJS 08/04/09
                    Case 1
                        sXMLFileName = "OrderAck-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 08/04/09 TJS 15/10/09
                        strFileType = "_POST_ORDER_ACKNOWLEDGEMENT_DATA_" ' TJS 27/11/09
                    Case 2
                        sXMLFileName = "OrderFulfill-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 08/04/09 TJS 15/10/09
                        strFileType = "_POST_ORDER_FULFILLMENT_DATA_" ' TJS 27/11/09
                    Case 3
                        sXMLFileName = "OrderAdjust-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 08/04/09 TJS 15/10/09
                        strFileType = "_POST_PAYMENT_ADJUSTMENT_DATA_" ' TJS 27/11/09
                    Case Else
                        sXMLFileName = "Unknown-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 08/04/09 TJS 15/10/09
                        strFileType = "" ' TJS 27/11/09
                End Select

                If strFileType <> "" Then ' TJS 27/11/09
                    strXMLcontent = Replace("<?xml version=""1.0"" encoding=""utf-8""?>" & XMLStatusUpdateFile.ToString, "><", ">" & vbCrLf & "<") ' TJS 27/11/09
                    ' update File name in array 
                    bFileExists = False ' TJS 27/11/09
                    rowAmazonFile = Nothing ' TJS 09/07/11
                    For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1 ' TJS 27/11/09
                        If ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = "Current" Or _
                                ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = sXMLFileName Then ' TJS 27/11/09
                            rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop) ' TJS 27/11/09
                            bFileExists = True ' TJS 27/11/09
                            Exit For ' TJS 27/11/09
                        End If
                    Next
                    If Not bFileExists Then ' TJS 27/11/09
                        rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.NewLerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
                        sTemp = m_ImportExportConfigFacade.GetField("SELECT ISNULL(MIN(CAST(AmazonDocumentID_DEV000221 AS bigint)), 0) FROM dbo.LerrynImportExportAmazonFiles_DEV000221 WHERE CAST(AmazonDocumentID_DEV000221 AS bigint) < 0", CommandType.Text, Nothing) ' TJS 27/11/09 TJS 09/07/11
                        If "" & sTemp <> "" Then ' TJS 27/11/09
                            rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(CInt(sTemp) - 1) ' TJS 27/11/09
                        Else
                            rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(-1) ' TJS 27/11/09
                        End If
                    End If
                    rowAmazonFile.SiteCode_DEV000221 = ActiveAmazonSettings.AmazonSite ' TJS 27/11/09
                    rowAmazonFile.MerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken ' TJS 27/11/09 TJS 22/03/13
                    rowAmazonFile.FileName_DEV000221 = sXMLFileName ' TJS 27/11/09
                    rowAmazonFile.AmazonMessageType_DEV000221 = strFileType ' TJS 27/11/09
                    rowAmazonFile.FileIsInputFromAmazon_DEV000221 = False ' TJS 27/11/09
                    rowAmazonFile.FileSentToAmazon_DEV000221 = False ' TJS 27/11/09
                    rowAmazonFile.ResponseReceived_DEV000221 = False ' TJS 27/11/09
                    rowAmazonFile.Processed_DEV000221 = False ' TJS 27/11/09
                    rowAmazonFile.FileContent_DEV000221 = strXMLcontent ' TJS 27/11/09
                    If Not bFileExists Then ' TJS 27/11/09
                        ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.AddLerrynImportExportAmazonFiles_DEV000221Row(rowAmazonFile) ' TJS 27/11/09
                    End If

                    m_ImportExportConfigFacade.WriteLogProgressRecord("Amazon Send Status File created, File " & sXMLFileName)
                    ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                        "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", _
                        "DeleteLerrynImportExportActionStatus_DEV000221"}, _
                        New String() {ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.TableName, _
                        "CreateLerrynImportExportAmazonMessageSettings_DEV000221", "UpdateLerrynImportExportAmazonMessageSettings_DEV000221", _
                        "DeleteLerrynImportExportAmazonMessageSettings_DEV000221"}, _
                        New String() {ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                        "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                        "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                        "Update Amazon Action Status", False) ' TJS 27/11/09
                    bAmazonStatusUpdatesToSend = False ' TJS 27/11/09
                Else
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SaveAmazonStatusFile", "Unknown Amazon XML Message type " & iAmazonXMLMessageType, "") ' TJS 27/11/09

                End If

                If ActiveSource.XMLImportFileSavePath <> "" Then ' TJS 27/11/09
                    'Start the logging to the file
                    writer = New StreamWriter(ActiveSource.XMLImportFileSavePath & sXMLFileName, True) ' TJS 27/11/09
                    ' write the XML to it
                    writer.WriteLine(XMLStatusUpdateFile.ToString)
                    ' and close it
                    writer.Close()
                End If

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SaveAmazonStatusFile", "Unexpected Merchant ID " & ImportExportStatusFacade.GetXMLElementText(XMLStatusUpdateFile, AMAZON_STATUS_UPDATE_HEADER & "/MerchantIdentifier") & " in submission file, expected " & ActiveAmazonSettings.MerchantToken, XMLStatusUpdateFile.ToString) ' TJS 17/03/09 TJS 22/03/13
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SaveAmazonStatusFile", ex)

        End Try

    End Sub

    Private Function CreateAmazonXMLDate(ByVal DateToUse As Date) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/06/10 | TJS             | 2010.0.07 | Modified to cater for day, month etc values less than 10
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strAmazonXMLDate As String

        strAmazonXMLDate = DateToUse.Year & "-" & Right("00" & DateToUse.Month, 2) & "-" & Right("00" & DateToUse.Day, 2) ' TJS 04/06/10
        strAmazonXMLDate = strAmazonXMLDate & "T" & Right("00" & DateToUse.Hour, 2) & ":" & Right("00" & DateToUse.Minute, 2) ' TJS 04/06/10
        strAmazonXMLDate = strAmazonXMLDate & ":" & Right("00" & DateToUse.Second, 2) & "-00:00" ' TJS 04/06/10
        Return strAmazonXMLDate

    End Function
End Module
