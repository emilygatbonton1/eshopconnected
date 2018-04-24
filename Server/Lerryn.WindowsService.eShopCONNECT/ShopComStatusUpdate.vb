' eShopCONNECT for Connected Business - Windows Service
' Module: ShopComStatusUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 02 August 2013

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports System.IO ' TJS 02/12/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 02/12/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Module ShopComStatusUpdate

    Private XMLStatusUpdateFile As XDocument
    Private xmlStatusUpdateNode As XElement
    Public bShopComStatusUpdatesToSend As Boolean ' TJS 11/02/09

    Public Sub DoShopComOrderStatusUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveShopComSettings As ShopComSettings, ByRef RowID As Integer, ByVal SalesOrderCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/09 | TJS             | 2009.1.05 | Function added
        ' 22/02/09 | TJS             | 2009.1.08 | changed read procedure for performance reasons and added new functions
        ' 09/06/09 | TJS             | 2009.2.11 | Corrected processing errors
        ' 26/08/09 | TJS             | 2009.3.05 | Corrected error on processing of partial deliveries and 
        '                                        | modified to use Invoice Detail for Quantity Shipped
        ' 22/11/09 | TJS             | 2009.3.09 | Corrected error when checking for status rows to skip
        ' 15/06/10 | TJS             | 2010.0.07 | Modified to prevent error when logging messages if related rows are skipped
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to ignore blank MerchantOrderIDs as well as null values
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Shipments also being done from Invoice
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 28/07/13 | TJS             | 2013.1.31 | Tidied progress log message text
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLActionDetails As XDocument, xmlUpdateNode As XElement, xmlStatusNode As XElement ' TJS 22/02/09
        Dim strTemp As String, strInvoiceCode As String, iCheckLoop As Integer, iEntryRowID As Integer ' TJS 15/06/10 TJS 09/07/11
        Dim dteTimeStamp As Date, decOrigQty As Decimal, decNewQty As Decimal, decOrigPrice As Decimal
        Dim decNewPrice As Decimal, decOrigTax As Decimal, decNewTax As Decimal, bAllItemsDelivered As Boolean

        Try
            iEntryRowID = RowID ' TJS 15/06/10
            ' for some reason, POCode field seems to cause constraint error if populated so disable constraints to allow sales order to load
            ImportExportStatusDataset.EnforceConstraints = False
            ' get order record
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrder.TableName, _
               "ReadCustomerSalesOrder", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 22/02/09
            If ImportExportStatusDataset.CustomerSalesOrder.Count > 0 Then
                Select Case ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221
                    Case "100" ' New Order

                        ' do we have a Shop.com Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then ' TJS 22/02/09
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status
                                xmlUpdateNode = New XElement("ADI_OS_STATUS")
                                xmlUpdateNode.SetAttributeValue("INVOICE_NUM", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221)
                                xmlUpdateNode.SetAttributeValue("MERCHANT_ORDER_NUM", SalesOrderCode)
                                xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "700")
                                xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "700")
                                xmlUpdateNode.Add(xmlStatusNode)

                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = xmlUpdateNode.ToString
                                ' mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                        End If

                        ' check for any new item records and mark them as complete
                        For iCheckLoop = iEntryRowID + 1 To ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.Count - 1
                            If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).SalesOrderCode_DEV000221 = SalesOrderCode Then
                                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).OrderStatus_DEV000221 = "105" Then ' TJS 22/11/09
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
                        If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then ' TJS 22/11/09
                            ' yes, set row pointer return value
                            RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1 ' TJS 22/11/09
                        End If


                    Case "105" ' New Item
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09


                    Case "200" ' Back Order

                        ' Start of code rewritten TJS 22/02/09
                        ' do we have a Shop.com Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then ' TJS 22/02/09
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status, get order detail (all rows)
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrderDetail.TableName, _
                                   "ReadCustomerSalesOrderDetailImportExport_DEV000221", AT_SALES_ORDER_CODE, SalesOrderCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)

                                xmlUpdateNode = New XElement("ADI_OS_STATUS")
                                xmlUpdateNode.SetAttributeValue("INVOICE_NUM", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221)
                                xmlUpdateNode.SetAttributeValue("MERCHANT_ORDER_NUM", SalesOrderCode)

                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerSalesOrderDetail.Count - 1
                                    ' has Source Purchase ID copied over from original order ?
                                    If ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then
                                        ' no, read from original order
                                        strTemp = "" & ImportExportStatusFacade.GetField("SourcePurchaseID_DEV000221", "CustomerSalesOrderDetail", _
                                            "SalesOrderCode = '" & ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).RootDocumentCode & _
                                            "' AND LineNum = " & ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).SourceLineNum)
                                        ' did original order have one (blank indicates an item added after order received from Shop.com)
                                        If strTemp <> "" Then
                                            ' yes, insert into back order
                                            ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).SourcePurchaseID_DEV000221 = strTemp
                                        End If
                                    Else
                                        strTemp = "" & ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).SourcePurchaseID_DEV000221
                                    End If
                                    If strTemp <> "" Then
                                        xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                        xmlStatusNode.SetAttributeValue("PURCHASE_ID", strTemp)
                                        xmlStatusNode.SetAttributeValue("QTY_CHANGE", ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).QuantityOrdered)
                                        xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "101")
                                        xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "101")
                                        xmlUpdateNode.Add(xmlStatusNode)
                                    End If
                                Next

                                ' check for any new item records and mark them as complete
                                For iCheckLoop = iEntryRowID + 1 To ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.Count - 1
                                    If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).SalesOrderCode_DEV000221 = SalesOrderCode Then
                                        If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).OrderStatus_DEV000221 = "205" Then ' TJS 22/11/09
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
                                If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then ' TJS 22/11/09
                                    ' yes, set row pointer return value
                                    RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1 ' TJS 22/11/09
                                End If
                                ' End of code rewritten TJS 22/02/09

                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = xmlUpdateNode.ToString
                                ' mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                ' save any details record we updated
                                ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerSalesOrderDetail.TableName, _
                                    "", "UpdateCustomerSalesOrderDetail", ""}}, Interprise.Framework.Base.Shared.TransactionType.None, "Update Lerryn Import/Export Back Order", False) ' TJS 22/02/09
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                        End If


                    Case "205" ' New Back Order Item
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09


                    Case "305" ' Item Order Qty changed
                        ' Start of code added TJS 22/02/09
                        ' do we have a Shop.com Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then ' TJS 22/02/09
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

                                    If Not ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then ' TJS 09/06/09
                                        XMLActionDetails = XDocument.Parse(ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221)
                                        decOrigQty = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemQtyChanged/OriginalQty"))
                                        decNewQty = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemQtyChanged/NewQty"))
                                        decOrigPrice = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemQtyChanged/OldItemTotal"))
                                        decNewPrice = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemQtyChanged/NewItemTotal"))
                                        decOrigTax = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemQtyChanged/OldSalesTax"))
                                        decNewTax = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemQtyChanged/NewSalesTax"))

                                        xmlUpdateNode = New XElement("ADI_OS_STATUS")
                                        xmlUpdateNode.SetAttributeValue("INVOICE_NUM", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221)
                                        xmlUpdateNode.SetAttributeValue("MERCHANT_ORDER_NUM", SalesOrderCode)

                                        xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                        xmlStatusNode.SetAttributeValue("PURCHASE_ID", ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).SourcePurchaseID_DEV000221)
                                        xmlStatusNode.SetAttributeValue("QTY_CHANGE", decNewQty - decOrigQty)
                                        xmlUpdateNode.SetAttributeValue("PRINCIPAL_ADJUSTMENT", FormatNumber(decNewPrice - decOrigPrice, 2, TriState.True, TriState.False, TriState.False))
                                        xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "108")
                                        xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "108")
                                        xmlUpdateNode.Add(xmlStatusNode)

                                        xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                        xmlStatusNode.SetAttributeValue("TAX_ADJUSTMENT", FormatNumber(decNewTax - decOrigTax, 2, TriState.True, TriState.False, TriState.False))
                                        xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "201")
                                        xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "201")
                                        xmlUpdateNode.Add(xmlStatusNode)

                                        ' save XML ready for sending
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = xmlUpdateNode.ToString
                                        ' mark record as having XML to send
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                    Else
                                        m_ImportExportConfigFacade.WriteLogProgressRecord("Order Status " & _
                                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                            " record found with blank SourcePurchaseID for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221) ' TJS 09/06/09
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 09/06/09
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
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                        End If
                        ' end of code added TJS 22/02/09


                    Case "310" ' Item Order Price changed

                        ' Start of code added TJS 22/02/09
                        ' do we have a Shop.com Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then ' TJS 22/02/09
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

                                    If Not ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then ' TJS 09/06/09
                                        XMLActionDetails = XDocument.Parse(ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221)
                                        decOrigPrice = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemPriceChanged/OriginalPrice")) ' TJS 09/06/09
                                        decNewPrice = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemPriceChanged/NewPrice")) ' TJS 09/06/09
                                        decOrigTax = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemPriceChanged/OldSalesTax"))
                                        decNewTax = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemPriceChanged/NewSalesTax"))

                                        xmlUpdateNode = New XElement("ADI_OS_STATUS")
                                        xmlUpdateNode.SetAttributeValue("INVOICE_NUM", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221)
                                        xmlUpdateNode.SetAttributeValue("MERCHANT_ORDER_NUM", SalesOrderCode)

                                        xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                        xmlStatusNode.SetAttributeValue("PURCHASE_ID", ImportExportStatusDataset.CustomerSalesOrderDetail(iCheckLoop).SourcePurchaseID_DEV000221)
                                        xmlUpdateNode.SetAttributeValue("PRINCIPAL_ADJUSTMENT", FormatNumber(decNewPrice - decOrigPrice, 2, TriState.True, TriState.False, TriState.False))
                                        xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "108")
                                        xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "108")
                                        xmlUpdateNode.Add(xmlStatusNode)

                                        xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                        xmlStatusNode.SetAttributeValue("TAX_ADJUSTMENT", FormatNumber(decNewTax - decOrigTax, 2, TriState.True, TriState.False, TriState.False))
                                        xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "201")
                                        xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "201")
                                        xmlUpdateNode.Add(xmlStatusNode)

                                        ' save XML ready for sending
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = xmlUpdateNode.ToString
                                        ' mark record as having XML to send
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                                    Else
                                        m_ImportExportConfigFacade.WriteLogProgressRecord("Order Status " & _
                                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                            " record found with blank SourcePurchaseID for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221) ' TJS 09/06/09
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 09/06/09
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
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                        End If
                        ' end of code added TJS 22/02/09


                    Case "350" ' Item Deleted
                        ' Start of code added TJS 22/02/09
                        ' do we have a Shop.com Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then ' TJS 22/02/09
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, do we have details in XML field ?
                                If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then
                                    ' yes, send status
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Processing Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record with trigger XML " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 & _
                                        " for Sales Order " & SalesOrderCode & ", Order Line Number " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221)

                                    XMLActionDetails = XDocument.Parse(ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221)
                                    decOrigQty = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemDeleted/OriginalQty"))
                                    decOrigPrice = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemDeleted/OriginalTotal")) ' TJS 09/06/09
                                    decOrigTax = CDec(ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemDeleted/OldSalesTax"))

                                    xmlUpdateNode = New XElement("ADI_OS_STATUS")
                                    xmlUpdateNode.SetAttributeValue("INVOICE_NUM", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221)
                                    xmlUpdateNode.SetAttributeValue("MERCHANT_ORDER_NUM", SalesOrderCode)

                                    xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                    xmlStatusNode.SetAttributeValue("PURCHASE_ID", ImportExportStatusFacade.GetXMLElementText(XMLActionDetails, "ItemDeleted/StorePurchaseID"))
                                    xmlStatusNode.SetAttributeValue("QTY_CHANGE", 0 - decOrigQty)
                                    xmlUpdateNode.SetAttributeValue("PRINCIPAL_ADJUSTMENT", FormatNumber(0 - decOrigPrice, 2, TriState.True, TriState.False, TriState.False))
                                    xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "118")
                                    xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "118")
                                    xmlUpdateNode.Add(xmlStatusNode)

                                    xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                    xmlStatusNode.SetAttributeValue("TAX_ADJUSTMENT", FormatNumber(0 - decOrigTax, 2, TriState.True, TriState.False, TriState.False))
                                    xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "201")
                                    xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "201")
                                    xmlUpdateNode.Add(xmlStatusNode)

                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = xmlUpdateNode.ToString
                                    ' mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
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
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                        End If
                        ' end of code added TJS 22/02/09


                    Case "500" ' Order Voided
                        ' do we have a Shop.com Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then ' TJS 22/02/09
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, do we have details in XML field ?
                                If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then ' TJS 22/02/09
                                    ' yes, send status
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Processing Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record with trigger XML " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 & _
                                        " for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221) ' TJS 22/02/09

                                    xmlUpdateNode = New XElement("ADI_OS_STATUS")
                                    xmlUpdateNode.SetAttributeValue("INVOICE_NUM", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221)
                                    xmlUpdateNode.SetAttributeValue("MERCHANT_ORDER_NUM", SalesOrderCode)

                                    xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                    ' was Credit CArd Authorised ?
                                    If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = "<CardAuthorised>Yes</CardAuthorised>" Then
                                        ' yes, must be cancallation at Customer Request
                                        xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "307")
                                        xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "307")
                                    Else
                                        ' no, must be cancallation for Credit Card failure
                                        xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "309")
                                        xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "309")
                                    End If
                                    xmlUpdateNode.Add(xmlStatusNode)

                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = xmlUpdateNode.ToString
                                    ' mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True

                                    ' check for any new item records and mark them as complete
                                    For iCheckLoop = iEntryRowID + 1 To ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.Count - 1
                                        If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).SalesOrderCode_DEV000221 = SalesOrderCode Then
                                            If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).OrderStatus_DEV000221 = "105" Then ' TJS 22/11/09
                                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True
                                            End If
                                        Else
                                            RowID = iCheckLoop - 1
                                            Exit For
                                        End If
                                    Next
                                    ' did check loop exit without finding a new row for a different item ?
                                    If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then ' TJS 22/11/09
                                        ' yes, set row pointer return value
                                        RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1 ' TJS 22/11/09
                                    End If
                                Else
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Order Status " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                                        " record found with blank trigger XML for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221) ' TJS 22/02/09
                                    ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                        End If


                    Case "600" ' Order Shipped
                        ' do we have a Shop.com Invoice ID (held in MerchantOrderID_DEV000221 field) ?
                        If Not ImportExportStatusDataset.CustomerSalesOrder(0).IsMerchantOrderID_DEV000221Null Then ' TJS 22/02/09
                            If ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221 <> "" Then ' TJS 19/08/10
                                ' yes, send status - get invoice detail (all rows) as order may not have QuantityShipped set
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.CustomerInvoiceDetail.TableName, _
                                   "ReadCustomerInvoiceDetailImportExport_DEV000221", AT_SOURCE_INVOICE_CODE, SalesOrderCode}}, _
                                   Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 26/08/09

                                ' have all items been delivered ?
                                bAllItemsDelivered = True
                                strInvoiceCode = "" ' TJS 09/07/11
                                For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1 ' TJS 26/08/09
                                    strInvoiceCode = ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).InvoiceCode ' TJS 09/07/11
                                    If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityOrdered <> ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped Then ' TJS 26/08/09
                                        bAllItemsDelivered = False
                                        Exit For
                                    End If
                                Next

                                xmlUpdateNode = New XElement("ADI_OS_STATUS")
                                xmlUpdateNode.SetAttributeValue("INVOICE_NUM", ImportExportStatusDataset.CustomerSalesOrder(0).MerchantOrderID_DEV000221)
                                xmlUpdateNode.SetAttributeValue("MERCHANT_ORDER_NUM", SalesOrderCode)

                                If bAllItemsDelivered Then
                                    ' all items delivered, send Invoice Shipped message
                                    xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                    xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "400")
                                    xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "400")
                                    xmlUpdateNode.Add(xmlStatusNode)
                                Else
                                    ' partial delivery only, send Item Shipped Message(s)
                                    dteTimeStamp = Date.Now
                                    For iCheckLoop = 0 To ImportExportStatusDataset.CustomerInvoiceDetail.Count - 1 ' TJS 26/08/09
                                        If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped > 0 And _
                                            Not ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).IsSourcePurchaseID_DEV000221Null Then ' TJS 09/06/09 TJS 26/08/09
                                            xmlStatusNode = New XElement("ADI_OS_ORDER_STATUS_DETAIL")
                                            xmlStatusNode.SetAttributeValue("PURCHASE_ID", ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).SourcePurchaseID_DEV000221)
                                            strTemp = ImportExportStatusFacade.TranslateDeliveryMethodFromIS(ActiveSource.SourceCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodCode, ImportExportStatusDataset.CustomerSalesOrder(0).ShippingMethodGroup)
                                            If strTemp <> "" Then
                                                xmlStatusNode.SetAttributeValue("SHIP_METHOD", strTemp)
                                            End If
                                            If ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityOrdered <> ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped Then
                                                xmlStatusNode.SetAttributeValue("QTY_CHANGE", ImportExportStatusDataset.CustomerInvoiceDetail(iCheckLoop).QuantityShipped)
                                            End If
                                            xmlStatusNode.SetAttributeValue("SHIP_DATE", dteTimeStamp.Month & "/" & dteTimeStamp.Day & "/" & dteTimeStamp.Year)
                                            xmlStatusNode.SetAttributeValue("INTERNAL_STATUS", "106")
                                            xmlStatusNode.SetAttributeValue("EXTERNAL_STATUS", "106")
                                            xmlUpdateNode.Add(xmlStatusNode)
                                        End If
                                    Next
                                End If

                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221 = xmlUpdateNode.ToString
                                ' mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).XMLToSend_DEV000221 = True
                            Else
                                ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 19/08/10
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True ' TJS 22/02/09
                        End If


                    Case Else
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionComplete_DEV000221 = True

                End Select

                If ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).IsActionXMLFile_DEV000221Null Then ' TJS 22/02/09
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Shop.com Order Status " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                       ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                       " - No XML created.") ' TJS 22/02/09 TJS 19/08/10 TJS 28/07/13
                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Shop.com Order Status " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).OrderStatus_DEV000221 & _
                        " record processed for Sales Order " & SalesOrderCode & ", Order Line Number " & _
                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).SalesOrderDetailLineNum_DEV000221 & _
                        " - XML created " & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(iEntryRowID).ActionXMLFile_DEV000221) ' TJS 22/02/09 TJS 19/08/10 TJS 28/07/13
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoShopComOrderStatusUpdate", "No Sales Order found for " & SalesOrderCode)

            End If

        Catch ex As Exception ' TJS 22/02/09
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoShopComOrderStatusUpdate", ex) ' TJS 22/02/09

        End Try

    End Sub

    Public Sub StartShopComStatusFile(ByVal ActiveSource As SourceSettings, ByVal ActiveShopComSettings As ShopComSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/09 | TJS             | 2009.1.05 | Function added
        ' 22/02/09 | TJs             | 2009.1.08 | Added error handler
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlUpdateNode As XElement
        Dim dteFileTimestamp As Date, strTimeStamp As String

        Try
            XMLStatusUpdateFile = New XDocument
            dteFileTimestamp = Date.Now

            xmlStatusUpdateNode = New XElement("ADI_OS_ORDER_STATUS_TRANSMISSION")
            xmlUpdateNode = New XElement("ADI_OS_HEADER")
            xmlUpdateNode.SetAttributeValue("CATALOGID", ActiveShopComSettings.CatalogID)
            strTimeStamp = dteFileTimestamp.Month & "/" & dteFileTimestamp.Day & "/" & dteFileTimestamp.Year & " "
            If dteFileTimestamp.Hour > 12 Then
                strTimeStamp = strTimeStamp & dteFileTimestamp.Hour - 12 & ":" & dteFileTimestamp.Minute & ":" & dteFileTimestamp.Second & " PM"
            Else
                strTimeStamp = strTimeStamp & dteFileTimestamp.Hour & ":" & dteFileTimestamp.Minute & ":" & dteFileTimestamp.Second & " AM"
            End If
            xmlUpdateNode.SetAttributeValue("DATE_TIME_STAMP", strTimeStamp)
            xmlStatusUpdateNode.Add(xmlUpdateNode)

            XMLStatusUpdateFile.Add(xmlStatusUpdateNode)

        Catch ex As Exception ' TJS 22/02/09
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartShopComStatusFile", ex) ' TJS 22/02/09

        End Try

    End Sub

    Public Sub AddToShopComStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveShopComSettings As ShopComSettings, ByRef RowID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/09 | TJS             | 2009.1.05 | Function added
        ' 11/02/09 | TJS             | 2009.1.07 | Modified to set flag to indicate status updates need sending
        ' 22/02/09 | TJS             | 2009.1.08 | Added check for null XML field and error handler
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStartingUpdateXML As String, iInsertPosn As Integer

        Try
            If Not ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null Then ' 22/02/09
                ' get current update XML
                strStartingUpdateXML = XMLStatusUpdateFile.ToString
                ' get position for insert
                iInsertPosn = InStr(strStartingUpdateXML, "</ADI_OS_ORDER_STATUS_TRANSMISSION>")
                ' insert XML from action record
                strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & Mid(strStartingUpdateXML, iInsertPosn)
                ' reload update XML
                XMLStatusUpdateFile = XDocument.Parse(strStartingUpdateXML)
            End If

            ' mark action record as XML Sent (change is committed to DB when status file successfully posted to Shop.com in SendShopComStatusFile)
            ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            bShopComStatusUpdatesToSend = True ' TJS 11/02/09

        Catch ex As Exception ' TJS 22/02/09
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddToShopComStatusFile", ex) ' TJS 22/02/09

        End Try

    End Sub

    Public Sub SendShopComStatusFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveShopComSettings As ShopComSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/09 | TJS             | 2009.1.05 | Function added
        ' 11/02/09 | TJS             | 2009.1.07 | Modified to clear status updates need sending flag
        ' 22/02/09 | TJS             | 2009.1.08 | Mofified to cater for renaming of WriteLogProgressRecord
        '                                        | and error handler
        ' 30/04/09 | TJS             | 2009.2.05 | Corrected error messages if response not as expected and corrected 
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use CheckRegistryValue
        ' 19/08/10 | TJS             | 2010.1.00 | Corrected InhibitWebPosts initialisation
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strUpdateURL As String = "", strReturn As String = "", bInhibitWebPosts As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") ' TJS 23/08/09 TJS 19/08/10

            ' check submission Merchant ID is as expected
            If ImportExportStatusFacade.GetXMLElementAttribute(XMLStatusUpdateFile, SHOPDOTCOM_STATUS_UPDATE_HEADER, "CATALOGID") = ActiveShopComSettings.CatalogID Then

                strUpdateURL = ActiveShopComSettings.StatusPostURL
                ' start of code replaced TJS 18/07/11
                Try
                    If Not bInhibitWebPosts Then
                        WebSubmit = System.Net.WebRequest.Create(strUpdateURL)
                        WebSubmit.Method = "GET"
                        WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                        WebSubmit.Timeout = 30000

                        byteData = UTF8Encoding.UTF8.GetBytes("order_status_data=" & XMLStatusUpdateFile.ToString)

                        ' send to LerrynSecure.com (or the URL defined in the Registry)
                        postStream = WebSubmit.GetRequestStream()
                        postStream.Write(byteData, 0, byteData.Length)

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        strReturn = reader.ReadToEnd()
                    Else
                        m_ImportExportConfigFacade.WriteLogProgressRecord("Shop.com Send Status File - Inhibited from sending Status update file - content " & XMLStatusUpdateFile.ToString) ' TJS 22/02/09
                        strReturn = "<?xml version=""1.0"" encoding=""UTF-8""?><CC_TRANSMISSION_RESPONSE><ORDER ALTURA_CATALOG_ID="""
                        strReturn = strReturn & ActiveShopComSettings.CatalogID & """ " & "ALTURA_INVOICE_NO="""
                        strReturn = strReturn & ImportExportStatusFacade.GetXMLElementAttribute(XMLStatusUpdateFile, SHOPDOTCOM_STATUS_UPDATE_ORDER_STATUS, "INVOICE_NUM")
                        strReturn = strReturn & """ ALTURA_PURCHASE_NO="""" /><STATUS STATUS_CODE=""0"" MESSAGE="""
                        strReturn = strReturn & "Order Status Post Success - Dummy Response"" /></CC_TRANSMISSION_RESPONSE>"

                    End If
                Catch ex As Exception
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Unable to post XML to " & strUpdateURL & " - " & ex.Message & ", " & ex.StackTrace, XMLStatusUpdateFile.ToString)

                Finally
                    If Not postStream Is Nothing Then postStream.Close()
                    If Not WebResponse Is Nothing Then WebResponse.Close()

                End Try
                ' end of code replaced TJS 02/12/11

                If strReturn <> "" Then
                    If strReturn.Length > 38 Then
                        If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then ' TJS 30/04/09
                            XMLResponse = XDocument.Parse(strReturn)
                            ' check Merchant ID matches
                            If ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_ORDER, "ALTURA_CATALOG_ID") = ActiveShopComSettings.CatalogID Then
                                ' yes, check Invoice ID matches
                                If ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_ORDER, "ALTURA_INVOICE_NO") = _
                                    ImportExportStatusFacade.GetXMLElementAttribute(XMLStatusUpdateFile, SHOPDOTCOM_STATUS_UPDATE_ORDER_STATUS, "INVOICE_NUM") Then
                                    ' yes, was update accepted ?
                                    If ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_STATUS, "STATUS_CODE") = "0" Then
                                        ' status update successful, save 
                                        m_ImportExportConfigFacade.WriteLogProgressRecord("Shop.com Send Status File - Update Successful, Status Code " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_STATUS, "STATUS_CODE") & ", message " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_STATUS, "MESSAGE") & ", response content " & XMLResponse.ToString) ' TJS 22/02/09
                                        ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                                            "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", "DeleteLerrynImportExportActionStatus_DEV000221"}}, _
                                            Interprise.Framework.Base.Shared.TransactionType.None, "Update Shop.com Action Status", False)
                                        bShopComStatusUpdatesToSend = False ' TJS 11/02/09

                                    Else
                                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Update rejected, Status Code " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_STATUS, "STATUS_CODE") & ", message " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_STATUS, "MESSAGE"), XMLResponse.ToString)
                                    End If

                                Else
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Unexpected Shop.com Invoice No. " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_ORDER, "ALTURA_INVOICE_NO") & ", expected " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_UPDATE_ORDER_STATUS, "INVOICE_NUM"), XMLResponse.ToString)
                                End If

                            Else
                                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Unexpected Catalog ID " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_ORDER, "ALTURA_CATALOG_ID") & " in response, expected " & ActiveShopComSettings.CatalogID, XMLResponse.ToString)
                            End If

                        Else
                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn) ' TJS 30/04/09
                        End If

                    Else
                        ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn) ' TJS 30/04/09
                    End If

                Else
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Response does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn) ' TJS 30/04/09
                End If
            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComStatusFile", "Unexpected Catalog ID " & ImportExportStatusFacade.GetXMLElementAttribute(XMLResponse, SHOPDOTCOM_STATUS_RESPONSE_ORDER, "ALTURA_CATALOG_ID") & " in submission file, expected " & ActiveShopComSettings.CatalogID, XMLStatusUpdateFile.ToString)
            End If

        Catch ex As Exception ' TJS 22/02/09
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendShopComStatusFile", ex) ' TJS 22/02/09

        End Try

    End Sub
End Module
