' eShopCONNECT for Connected Business - Windows Service
' Module: eBayImport.vb
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
' Last Updated - 22 May 2014

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Xml.Linq
Imports System.Xml.XPath

Module eBayImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decItemTotal As Decimal
    Private decItemTaxTotal As Decimal
    Private decItemShippingTotal As Decimal
    Private decItemShippingTaxTotal As Decimal ' TJS/FA 15/10/13
    Private strNotesAddn As String

    Private Structure PaymentDetails
        Public TransactionID As String
        Public Timestamp As String
    End Structure

    Public Function ProcessEBayOrder(ByVal SourceConfig As SourceSettings, ByVal EBayConfig As eBaySettings, _
        ByRef XMLOrder As XDocument, ByVal ImportAsType As String, ByRef AlreadyImported As Boolean, _
        ByRef RowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes orders received from eBay
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 26/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to cater for re-try of failed imports and added Error Notification object to simplify facade login/logout
        ' 23/05/13 | TJS             | 2013.1.16 | Modified to cater for Do Not Import payment options
        ' 30/07/13 | TJS             | 2013.1.32 | Modified to cater for PricesIncludeTax
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to use BuyerUserID as source customer ID
        ' 07/10/13 | FA              | 2013.3.05 | Removed duplicated/unused local variables to correct tax processing
        ' 15/10/13 | FA/TJS            2013.3.08 | Added decItemShippingTaxTotal to tax figure and totals
        ' 19/03/14 | TJS             | 2014.0.00 | Corrected error when checking Do Not Import payment flag
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLGenericOrder As XDocument, XMLItemTemp As XDocument, XMLPaymentTemp As XDocument
        Dim XMLISOrderImportNode As XElement, XMLOrderSourceNode As XElement, XMLOrderHeaderNode As XElement
        Dim XMLPaymentDetailsNode As XElement, XMLOrderTotalsNode As XElement, XMLBuyerUserIDNode As XElement
        Dim XMLItemNode As XElement, XMLImportResponseNode As XElement, XMLOrderResponseNode As XElement
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLPaymentList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strSourceOrderRef As String, strCustomerOrderRef As String, strCustomerEmail As String
        Dim strISCustomerCode As String, strTemp As String, strErrorMessage As String, strBuyerUserID As String
        ' Dim decItemTotal As Decimal, decItemShippingTotal As Decimal, decItemTaxTotal As Decimal FA 07/10/13
        ' Dim decShippingInsuranceTax As Decimal FA 07/10/13
        Dim iLocalTimeOffset As Integer, dteOrderDate As Date, bReturnValue As Boolean, bDuplicateTransaction As Boolean
        Dim PaymentList() As PaymentDetails, bDoNotImportPayment As Boolean ' TJS 23/05/13

        Try
            bReturnValue = True
            AlreadyImported = False ' TJS 10/06/12
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            ' check module and connector are activated and valid
            If eShopCONNECTFacade.ValidateSource("eBay eShopCONNECTOR", EBAY_SOURCE_CODE, "", False) Then
                ' yes, has order already been imported ?
                strSourceOrderRef = eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID")
                strCustomerOrderRef = eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID")

                If SourceConfig.IgnoreVoidedOrdersAndInvoices Then
                    eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                        "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, EBAY_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                        AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, EBayConfig.SiteID, AT_IS_VOIDED, "0", _
                        AT_TYPE, "Sales Order"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                Else
                    eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                        "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, EBAY_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                        AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, EBayConfig.SiteID, AT_TYPE, "Sales Order"}}, _
                        Interprise.Framework.Base.Shared.ClearType.Specific)
                End If
                If eShopCONNECTDatasetGateway.CustomerSalesOrder.Count = 0 Then
                    ' order not yet imported, has payment failed i.e. ImportAsType is Cancel ?
                    If ImportAsType <> "Cancel" Then
                        ' no, has eBay order been marked as shipped ?
                        If eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderStatus") <> "Shipped" Then
                            ' no, was order originally imported as Quote ?
                            If SourceConfig.IgnoreVoidedOrdersAndInvoices Then
                                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                                    "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, EBAY_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                                    AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, EBayConfig.SiteID, AT_IS_VOIDED, "0", _
                                    AT_TYPE, "Quote"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                            Else
                                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                                    "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, EBAY_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                                    AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, EBayConfig.SiteID, AT_TYPE, "Quote"}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific)
                            End If
                            ' did we find a quote in IS and import type is still Quote ?
                            If eShopCONNECTDatasetGateway.CustomerSalesOrder.Count > 0 And ImportAsType = "Quote" Then
                                ' yes, is payment now cleared ?
                                If eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_CHECKOUT_DETAILS & "Status") = "Complete" Then
                                    ' yes, build generic XML as an Order
                                    ImportAsType = "Order"
                                Else
                                    ' no, payment failed on existing quote - nothing to do except return
                                    bReturnValue = True
                                    eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " import from Account " & EBayConfig.SiteID & " skipped as order already imported as quote (" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "), but payment now failed")

                                    eShopCONNECTFacade.Dispose()

                                    Return bReturnValue
                                End If

                            End If

                            decItemTotal = 0
                            decItemShippingTotal = 0
                            decItemTaxTotal = 0
                            decItemShippingTaxTotal = 0 ' TJS/FA 15/10/13
                            strBuyerUserID = ""
                            strNotesAddn = ""
                            bDoNotImportPayment = False ' TJS 23/05/13

                            ' no, create Generic XML document
                            XMLGenericOrder = New XDocument()
                            XMLISOrderImportNode = New XElement("eShopCONNECT")

                            'SOURCE DETAILS
                            XMLOrderSourceNode = New XElement("Source")
                            XMLOrderSourceNode.Add(New XElement("SourceName", "eBay Order"))
                            XMLOrderSourceNode.Add(New XElement("SourceCode", EBAY_SOURCE_CODE))
                            XMLISOrderImportNode.Add(XMLOrderSourceNode)


                            ' check if customer exists in IS
                            strBuyerUserID = eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "BuyerUserID")
                            eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.TableName, _
                                "ReadCustomerImportExportView_DEV000221", AT_IMPORT_CUSTOMER_ID, strBuyerUserID, AT_IMPORT_SOURCE_ID, EBAY_SOURCE_CODE}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)
                            If eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.Count > 0 Then
                                strISCustomerCode = eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221(0).CustomerCode
                            Else
                                strISCustomerCode = ""
                            End If

                            ' ORDER DETAILS

                            XMLOrderHeaderNode = New XElement(ImportAsType)
                            XMLOrderHeaderNode.Add(New XElement("CustomerOrderRef", strCustomerOrderRef))
                            XMLOrderHeaderNode.Add(New XElement("Source" & ImportAsType & "Ref", eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID")))
                            strTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & EBAY_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & EBayConfig.SiteID.Replace("'", "''") & "'")
                            If "" & strTemp <> "" Then ' TJS 25/04/11
                                XMLOrderHeaderNode.Add(New XElement("SourceWebSiteRef", strTemp))
                            Else
                                XMLOrderHeaderNode.Add(New XElement("SourceWebSiteRef", "ChannelAdvisor"))
                            End If
                            XMLOrderHeaderNode.Add(New XElement("SourceMerchantID", EBayConfig.SiteID))

                            If EBayConfig.PricesAreTaxInclusive Then ' TJS 30/07/13
                                XMLOrderHeaderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 30/07/13
                                XMLOrderHeaderNode.Add(New XElement("TaxCodeForSourceTax", EBayConfig.TaxCodeForSourceTax)) ' TJS 30/07/13
                            End If

                            ' get eBay order date (Date + time) 
                            dteOrderDate = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "CreatedTime"))
                            ' get local time zone difference
                            iLocalTimeOffset = CInt(DateDiff(DateInterval.Hour, Date.UtcNow, Date.Now))
                            ' do we need to adjust order date ?
                            If iLocalTimeOffset <> 0 Then
                                ' yes, add offset hours
                                dteOrderDate = dteOrderDate.AddHours(iLocalTimeOffset)
                            End If
                            ' and strip out time
                            XMLOrderHeaderNode.Add(New XElement(ImportAsType & "Date", dteOrderDate.Year & "-" & Right("00" & dteOrderDate.Month, 2) & "-" & Right("00" & dteOrderDate.Day, 2)))

                            ' take currency from total
                            XMLOrderHeaderNode.Add(New XElement(ImportAsType & "Currency", eShopCONNECTFacade.GetXMLElementAttribute(XMLOrder, EBAY_ORDER_DETAILS & "Total", "currencyID")))

                            XMLItemList = XMLOrder.XPathSelectElements(EBAY_ORDER_ITEM_LIST)
                            If eShopCONNECTFacade.GetXMLElementListCount(XMLItemList) > 0 Then
                                XMLItemTemp = XDocument.Parse(XMLItemList(0).ToString)
                                strCustomerEmail = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, EBAY_ORDER_ITEM_DETAILS & "Buyer/Email")
                                'eBay only keeps email addresses for 15 days and after that it says Invalid Request which causes IS valifation failures
                                If strCustomerEmail = "Invalid Request" Then ' TJS 10/08/12
                                    strCustomerEmail = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, EBAY_ORDER_ITEM_DETAILS & "Buyer/StaticAlias") ' TJS 10/08/12 TJS 19/09/13
                                End If
                            Else
                                m_ImportExportConfigFacade.BuildXMLErrorResponseNodeAndEmail("Error", "013", "No Order Items found", m_ImportExportConfigFacade.SourceConfig, _
                                    "ProcessEBayOrder", XMLOrder.ToString) ' TJS 10/08/12
                                Return False
                            End If

                            ' BILLING DETAILS
                            XMLOrderHeaderNode.Add(BillingDetails(XMLOrder, XMLGenericOrder, strBuyerUserID, strCustomerEmail, strISCustomerCode, ImportAsType))

                            ' SHIPPING DETAILS
                            XMLOrderHeaderNode.Add(ShippingDetails(XMLOrder, XMLGenericOrder, SourceConfig, ImportAsType))

                            ' ITEM DETAILS
                            strErrorMessage = ""
                            For Each XMLItemNode In XMLItemList
                                XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                XMLOrderHeaderNode.Add(ItemDetails(XMLItemTemp, XMLGenericOrder, EBayConfig, strErrorMessage))
                                If strErrorMessage <> "" Then
                                    Exit For
                                End If

                            Next

                            If strErrorMessage = "" Then
                                ' PAYMENT DETAILS
                                If eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_CHECKOUT_DETAILS & "/Status") = "Complete" Then
                                    XMLPaymentList = XMLOrder.XPathSelectElements(EBAY_ORDER_PAYMENT_DETAILS_LIST)
                                    ReDim PaymentList(0)
                                    ' make sure initial timastamp is well in the past
                                    PaymentList(0).Timestamp = CreateEBayXMLDate(Date.Now.AddYears(-10))
                                    ' get list of payment transactions and remove duplicates
                                    For Each XMLPaymentDetailsNode In XMLPaymentList
                                        XMLPaymentTemp = XDocument.Parse(XMLPaymentDetailsNode.ToString)
                                        ' check if transaction id already in payment list
                                        bDuplicateTransaction = False
                                        For Each Transaction As PaymentDetails In PaymentList
                                            If Transaction.TransactionID = eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionID") Then
                                                ' duplicate transaction, is timestamp later than existing value ?
                                                If eShopCONNECTFacade.ConvertXMLDate(Transaction.Timestamp) < eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionTime")) Then
                                                    ' yes, update timestamp
                                                    Transaction.Timestamp = eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionTime")
                                                End If
                                                bDuplicateTransaction = True
                                            End If
                                        Next
                                        ' is transaction a duplicate ?
                                        If Not bDuplicateTransaction Then
                                            ' no add to array
                                            ReDim Preserve PaymentList(PaymentList.Length)
                                            PaymentList(PaymentList.Length - 1).TransactionID = eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionID")
                                            PaymentList(PaymentList.Length - 1).Timestamp = eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionTime")
                                        End If
                                    Next
                                    ' now create payment records for the relevant transaction(s)
                                    For Each XMLPaymentDetailsNode In XMLPaymentList
                                        XMLPaymentTemp = XDocument.Parse(XMLPaymentDetailsNode.ToString)
                                        For Each Transaction As PaymentDetails In PaymentList
                                            If Transaction.TransactionID = eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionID") And
                                                Transaction.Timestamp = eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionTime") Then
                                                XMLPaymentDetailsNode = New XElement("PaymentDetails")
                                                XMLPaymentDetailsNode.Add(New XElement("PaymentMethod", "Source"))
                                                If EBayConfig.EnablePaymentTypeTranslation Then
                                                    XMLPaymentDetailsNode.Add(New XElement("PaymentType", eShopCONNECTFacade.TranslatePaymentTypeToIS(EBAY_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_CHECKOUT_DETAILS & "/PaymentMethod"), EBayConfig.PaymentType, bDoNotImportPayment))) ' TJS 23/05/13
                                                Else
                                                    XMLPaymentDetailsNode.Add(New XElement("PaymentType", EBayConfig.PaymentType))
                                                End If
                                                XMLPaymentDetailsNode.Add(New XElement("PaymentValue", eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "PaymentOrRefundAmount")))
                                                XMLPaymentDetailsNode.Add(New XElement("SourcePaymentID", eShopCONNECTFacade.GetXMLElementText(XMLPaymentTemp, EBAY_ORDER_PAYMENT_DETAILS & "ExternalTransactionID")))
                                                If Not bDoNotImportPayment Then ' TJS 23/05/13 TJS 19/03/14
                                                    XMLOrderHeaderNode.Add(XMLPaymentDetailsNode)
                                                End If
                                                Exit For
                                            End If
                                        Next
                                    Next
                                End If
                                ' ORDER TOTALS
                                XMLOrderTotalsNode = New XElement(ImportAsType & "Totals")
                                XMLOrderTotalsNode.Add(New XElement("SubTotal", Format(decItemTotal, "0.00")))
                                XMLOrderTotalsNode.Add(New XElement("Shipping", Format(decItemShippingTotal, "0.00")))
                                XMLOrderTotalsNode.Add(New XElement("Tax", Format(decItemTaxTotal + decItemShippingTaxTotal, "0.00"))) ' FA 07/10/13 TJS/FA 15/10/13
                                XMLOrderTotalsNode.Add(New XElement("Total", Format(decItemTotal + decItemShippingTotal + decItemTaxTotal + decItemShippingTaxTotal, "0.00"))) 'FA 15/10/13
                                XMLOrderHeaderNode.Add(XMLOrderTotalsNode)

                                If strBuyerUserID <> "" Then
                                    XMLBuyerUserIDNode = New XElement("CustomField")
                                    XMLBuyerUserIDNode.SetAttributeValue("FieldName", "ImportSourceBuyerID_DEV000221")
                                    XMLBuyerUserIDNode.Value = strBuyerUserID
                                    XMLItemNode = XMLOrderHeaderNode.XPathSelectElement("BillingDetails/Customer")
                                    XMLItemNode.Add(XMLBuyerUserIDNode)
                                End If
                                If strNotesAddn <> "" Then
                                    XMLOrderHeaderNode.Add(New XElement("CustomerComments", strNotesAddn))
                                End If
                                XMLISOrderImportNode.Add(XMLOrderHeaderNode)

                                XMLISOrderImportNode.Add(New XElement(ImportAsType & "Count", "1"))
                                XMLGenericOrder.Add(XMLISOrderImportNode)

                                strTemp = SourceConfig.XMLImportFileSavePath
                                If strTemp <> "" Then
                                    If strTemp.Substring(strTemp.Length - 1, 1) <> "\" Then
                                        strTemp = strTemp & "\"
                                    End If
                                    If My.Computer.FileSystem.FileExists(strTemp & "eBay_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & ".xml") Or _
                                        My.Computer.FileSystem.FileExists(strTemp & "GenericXML_for_eBay_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & ".xml") Then
                                        XMLOrder.Save(strTemp & "eBay_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & "_" & LogFileTimestamp(Now) & ".xml")
                                        XMLGenericOrder.Save(strTemp & "GenericXML_for_eBay_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & "_" & LogFileTimestamp(Now) & ".xml")
                                    Else
                                        XMLOrder.Save(strTemp & "eBay_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & ".xml")
                                        XMLGenericOrder.Save(strTemp & "GenericXML_for_eBay_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & ".xml")
                                    End If
                                End If

                                ' Generic XML Built, was order originally imported as Quote ?
                                If eShopCONNECTDatasetGateway.CustomerSalesOrder.Count = 0 Then
                                    ' no, now do call to create order and customer (where customer does not exist)
                                    strTemp = ""
                                    If ImportAsType = "Order" Then
                                        XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "")
                                    Else
                                        XMLImportResponseNode = eShopCONNECTFacade.XMLQuoteImport(XMLGenericOrder)
                                    End If
                                Else
                                    ' yes, now do call to update quote, update customer if details were blank and import updated order
                                    strTemp = eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode
                                    XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode)
                                End If
                                XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                                If XMLOrderResponseNode IsNot Nothing Then
                                    If XMLOrderResponseNode.Value = "Success" Then
                                        ' don't send any response now, new order DB trigger will create response
                                        If ImportAsType = "Order" Then
                                            ' was order originally imported as Quote ?
                                            If strTemp = "" Then
                                                ' no
                                                eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " successfully imported from eBay Site " & EBayConfig.SiteID & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value)
                                            Else
                                                ' yes
                                                eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " successfully imported from eBay Site " & EBayConfig.SiteID & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value & " - Quote " & strTemp & " has been closed")
                                            End If
                                            ' is payment status set to Failed ?
                                            If eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "PaymentHoldStatus") <> "None" Then
                                                ' not shipped, warn that payment failed and put order on hold
                                                eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrderWorkflow SET Stage = 'Approve Credit' WHERE SalesOrderCode = '" & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value & "'", Nothing)
                                                strErrorMessage = "Payment Status for eBay Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & _
                                                    " (Interprise Sales Order " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value & ") from eBay Site " & EBayConfig.SiteID & " has changed to Failed.  Order not yet shipped and workflow status set to Approve Credit."
                                                eShopCONNECTFacade.SendPaymentErrorEmail(SourceConfig.XMLConfig, strErrorMessage)
                                            End If
                                        Else
                                            eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " successfully imported from eBay Site " & EBayConfig.SiteID & " as Quote " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/QuoteNumber").Value)
                                        End If
                                    Else
                                        bReturnValue = False
                                        eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " import from eBay Site " & EBayConfig.SiteID & " failed")
                                    End If
                                Else
                                    bReturnValue = False
                                    eShopCONNECTFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessEBayOrder", XMLImportResponseNode.ToString, "")
                                End If
                            Else
                                bReturnValue = False
                                m_ImportExportConfigFacade.BuildXMLErrorResponseNodeAndEmail("Error", "013", strErrorMessage, m_ImportExportConfigFacade.SourceConfig, _
                                    "ProcessEBayOrder", XMLGenericOrder.ToString)
                            End If

                        Else
                            ' order not yet imported, has been partially or fully shipped - ignore it
                            bReturnValue = False
                            eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " import from eBay Site " & EBayConfig.SiteID & " skipped as order already shipped")
                        End If
                    Else
                        ' order not yet imported, payment has failed - ignore it
                        bReturnValue = False
                        eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " import from eBay Site " & EBayConfig.SiteID & " skipped as payment failed before import.")

                    End If
                    ' start of code added TJS 10/06/12
                    If bReturnValue Then
                        If RowOrderToRetry IsNot Nothing Then
                            RowOrderToRetry.SuccessfullyImported_DEV000221 = True
                            RowOrderToRetry.RetryCount_DEV000221 = 0
                        End If
                    Else
                        If RowOrderToRetry IsNot Nothing Then
                            If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                                RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                            Else
                                RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                            End If
                            RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                        Else
                            RowOrderToRetry = m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.NewLerrynImportExportSourceOrdersToRetry_DEV000221Row()
                            RowOrderToRetry.SourceCode_DEV000221 = EBAY_SOURCE_CODE
                            RowOrderToRetry.StoreMerchantID_DEV000221 = EBayConfig.SiteID
                            RowOrderToRetry.MerchantOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID")
                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                            RowOrderToRetry.RetryCount_DEV000221 = 6
                        End If
                    End If
                    ' end of code added TJS 10/06/12
                Else
                    ' order already imported, is payment status now set to hold ?
                    AlreadyImported = True ' TJS 10/06/12
                    If eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "PaymentHoldStatus") <> "None" Then
                        ' yes, check if email already sent
                        strTemp = eShopCONNECTFacade.GetField("PaymentFailedEmailSent_DEV000221", "CustomerSalesOrder", "SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'")
                        If strTemp = "" OrElse Not CBool(strTemp) Then
                            ' no, has order been shipped ?
                            strTemp = eShopCONNECTFacade.GetField("Stage", "CustomerSalesOrderWorkflow", "SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'")
                            If strTemp = "Completed" Then
                                ' yes, has it been partially shipped i.e. a Back Order has been created
                                strTemp = eShopCONNECTFacade.GetField("SELECT Stage FROM CustomerSalesOrderWorkflow WHERE SalesOrderCode in (SELECT SalesOrderCode FROM CustomerSalesOrder WHERE Type = 'Back Order' AND RootDocumentCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "')", CommandType.Text, Nothing)
                                If strTemp = "" Then
                                    ' no, warn that order already shipped
                                    strErrorMessage = "Payment Status for eBay Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & _
                                        ") from eBay Site " & EBayConfig.SiteID & " has changed to Failed after Order has been shipped."
                                ElseIf strTemp = "Completed" Then
                                    ' yes and Back Order also shipped, warn that order already shipped
                                    strErrorMessage = "Payment Status for eBay Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & _
                                        ") from eBay Site " & EBayConfig.SiteID & " has changed to Failed after Order, and a related Back Order, have been shipped."
                                Else
                                    ' yes, warn that order is part shipped and put Back Order on hold
                                    eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrderWorkflow SET Stage = 'Approve Credit' WHERE Stage <> 'Completed' AND SalesOrderCode IN (SELECT SalesOrderCode FROM CustomerSalesOrder WHERE Type = 'Back Order' AND RootDocumentCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "')", Nothing)
                                    strErrorMessage = "Payment Status for eBay Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & _
                                        ") from eBay Site " & EBayConfig.SiteID & " has changed to Failed after Order has been part shipped - Back Order workflow status now set to Approve Credit."
                                End If
                            Else
                                ' not shipped, warn that payment failed and put order on hold
                                eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrderWorkflow SET Stage = 'Approve Credit' WHERE SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'", Nothing)
                                strErrorMessage = "Payment Status for eBay Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & _
                                    ") from eBay Site " & EBayConfig.SiteID & " has changed to Failed.  Order not yet shipped and workflow status now set to Approve Credit."

                            End If
                            eShopCONNECTFacade.SendPaymentErrorEmail(SourceConfig.XMLConfig, strErrorMessage)
                            eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrder SET PaymentFailedEmailSent_DEV000221 = 1 WHERE SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'", Nothing)

                        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "PaymentHoldStatus") = "None" Then
                            ' payment status is still Cleared and shipping status is Shipped or Partially Shipped so ignore as status change 
                            ' likely to be the shipping status being updated

                        Else
                            ' order updated but payment not marked as failed - log details for now
                            eShopCONNECTFacade.WriteLogProgressRecord("eBay Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, EBAY_ORDER_DETAILS & "OrderID") & " from eBay Site " & EBayConfig.SiteID & " already imported, status changed, but payment not failed")

                        End If
                    End If
                    ' start of code added TJS 10/06/12
                    If RowOrderToRetry IsNot Nothing Then
                        RowOrderToRetry.SuccessfullyImported_DEV000221 = True
                        RowOrderToRetry.RetryCount_DEV000221 = 0
                    End If
                    ' end of code added TJS 10/06/12
                End If

                ' start of code added TJS 10/06/12
            Else
                If RowOrderToRetry IsNot Nothing Then
                    If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                        RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                    Else
                        RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                    End If
                    RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                End If
                ' end of code added TJS 10/06/12   
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(SourceConfig.XMLConfig, "ProcessEBayOrder", ex, "")
            bReturnValue = False

        Finally
            eShopCONNECTFacade.Dispose()

        End Try
        Return bReturnValue

    End Function

    Private Function BillingDetails(ByRef XMLEBayOrder As XDocument, ByRef XMLGenericOrder As XDocument, ByVal BuyerUserID As String, _
        ByVal EBayBuyerEmail As String, ByVal ISCustomerCode As String, ByVal ImportAsType As String) As XElement ' TJS 19/09/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/08/12 | TJS             | 2012.1.12 | Modified to cater for eBay using different Country Names than IS
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to use BuyerUserID as source customer ID
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBill As XElement, XMLBillingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strTemp As String

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBill = New XElement("Customer")
        XMLCustomerBill.Add(New XElement("SourceCustomerID", BuyerUserID)) ' TJS 19/09/13
        XMLCustomerBill.Add(New XElement("ISCustomerCode", ISCustomerCode))
        ' use shipping name etc as no specific billing details in order
        If eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Name") <> "" Then
            XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Name")))
        ElseIf ImportAsType = "Quote" Then
            ' When importing as a Quote, may not have any customer details other than BuyerUserID
            ' if so, need to ensure record is not rejected so we use a full stop in the name
            XMLCustomerBill.Add(New XElement("LastName", "."))
        End If
        XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Phone")))
        XMLCustomerBill.Add(New XElement("Email", EBayBuyerEmail))

        ' BILLING ADDRESS
        XMLBillingAddress = New XElement("BillingAddress")
        ' use shipping address as no specific billing address in order
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Street1")
        If eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Street2") <> "" Then
            strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Street2")
        End If
        If ImportAsType = "Quote" And strTemp = "" Then
            ' When importing as a Quote, may not have any customer details other than BuyerUserID 
            ' if so, need to ensure record is not rejected so we use a full stop in the address
            strTemp = "."
        End If
        XMLBillingAddress.Add(New XElement("Address", strTemp))
        XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "CityName")))
        ' need to ensure Country matches those used in IS
        strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "CountryName")) ' TJS 10/08/12
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "StateOrProvince").Length <= 2 Then
            XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "StateOrProvince")))
        Else
            XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "StateOrProvince")))
        End If
        XMLBillingAddress.Add(New XElement("Country", strCountry))
        XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "PostalCode")))

        XMLResult.Add(XMLCustomerBill)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult
    End Function

    Private Function ShippingDetails(ByRef XMLEBayOrder As XDocument, ByRef XMLGenericOrder As XDocument, _
        ByVal SourceConfig As SourceSettings, ByVal ImportAsType As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/08/12 | TJS             | 2012.1.12 | Modified to cater for eBay using different Country Names than IS
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strTemp As String, strShippingMethodGroup As String


        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        If eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Name") <> "" Then
            XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Name")))
        ElseIf ImportAsType = "Quote" Then
            ' When importing as a Quote, may not have any customer details other than email address 
            ' if so, need to ensure record is not rejected so we use a full stop in the name
            XMLCustomerShip.Add(New XElement("LastName", "."))
        End If
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Phone")))

        ' SHIPPING ADDRESS
        XMLShippingAddress = New XElement("ShippingAddress")
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Street1")
        If eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Street2") <> "" Then
            strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "Street2")
        End If
        If ImportAsType = "Quote" And strTemp = "" Then
            ' When importing as a Quote, may not have any customer details other than email address 
            ' if so, need to ensure record is not rejected so we use a full stop in the address
            strTemp = "."
        End If
        XMLShippingAddress.Add(New XElement("Address", strTemp))
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "CityName")))
        ' need to ensure Country matches those used in IS
        strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "CountryName")) ' TJS 10/08/12
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "StateOrProvince").Length <= 2 Then
            XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "StateOrProvince")))
        Else
            XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "StateOrProvince")))
        End If
        XMLShippingAddress.Add(New XElement("Country", strCountry))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS & "PostalCode")))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If SourceConfig.EnableDeliveryMethodTranslation AndAlso eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS_METHOD & "ShippingService") <> "" Then
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS(EBAY_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS_METHOD & "ShippingService"), "", strShippingMethodGroup)))
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS_METHOD & "ShippingService") = "" Then
            XMLResult.Add(New XElement("ShippingMethod", SourceConfig.DefaultShippingMethod))
            XMLResult.Add(New XElement("ShippingMethodGroup", SourceConfig.DefaultShippingMethodGroup))

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS_METHOD & "ShippingService")))
            XMLResult.Add(New XElement("ShippingMethodGroup", SourceConfig.DefaultShippingMethodGroup))
        End If
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrder, EBAY_ORDER_SHIPPING_DETAILS_METHOD & "ShippingServiceCost")
        If strTemp <> "" Then
            decItemShippingTotal = decItemShippingTotal + CDec(strTemp)
        End If

        Return XMLResult
    End Function

    Private Function ItemDetails(ByRef XMLEBayOrderItem As XDocument, ByRef XMLGenericOrder As XDocument, _
        ByVal EBayConfig As eBaySettings, ByRef ErrorMessage As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to allow use of eBay Item Id if no SellerInventoryID (SKU)
        ' 10/08/12 | TJS             | 2012.1.12 | Modified to cater for eBay US passing an SKU item instead of a SellerInventoryID
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221 and corrected SKU Alias processong
        ' 14/10/13 | FA              | 2013.3.07 | Modified to pick up the tax value and pick up SKU from ebay variations.
        ' 22/05/14 | TJS             | 2014.0.01 | Modified for Unit of Measure on SKU Alias lookup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTaxTemp As XDocument, XMLTaxNode As XElement ' TJS/FA 14/10/13
        Dim XMLTaxList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS/FA 14/10/13

        Dim XMLResult As XElement, strTemp As String
        Dim decItemUnitPrice As Decimal, decItemQuantity As Decimal

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        XMLResult.Add(New XElement("SourceItemPurchaseID", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "OrderLineItemID")))
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "Item/SKU") ' TJS 10/08/12
        ' is there one ?
        If String.IsNullOrEmpty(strTemp) Then 'FA 14/10/13
            'no, try the Variation/SKU
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "Variation/SKU") 'FA 14/10/13
            'is there one?
            If String.IsNullOrEmpty(strTemp) Then ' TJS 10/08/12
                ' no, try SellerInventoryID
                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "Item/SellerInventoryID")
                ' is there one ?
                If String.IsNullOrEmpty(strTemp) Then
                    ' no, get eBay Item ID - NOTE this will change if item is re-listed
                    strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "Item/ItemID")
                End If
            End If
        End If

        ' is there an SKU for the item ?
        If strTemp <> "" Then
            ' yes, has Custom SKU Processing been enabled ?
            If EBayConfig.CustomSKUProcessing <> "" Then
                ' yes, convert SKU
                strTemp = ConvertSKU(EBayConfig.CustomSKUProcessing, strTemp)
            End If
            If EBayConfig.EnableSKUAliasLookup Then
                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", EBAY_SOURCE_CODE, "@SourceSKU", strTemp}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then ' TJS 03/07/13
                    strTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 ' TJS 03/07/13
                    XMLResult.Add(New XElement("ISItemCode", strTemp)) ' TJS 03/07/13
                    If Not eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).IsUnitMeasureCode_DEV000221Null Then ' TJS 22/05/14
                        XMLResult.Add(New XElement("ItemUnitOfMeasure", eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).UnitMeasureCode_DEV000221)) ' TJS 22/05/14
                    End If
                Else
                    XMLResult.Add(New XElement("IS" & EBayConfig.ISItemIDField, strTemp))
                End If

            Else
                XMLResult.Add(New XElement("IS" & EBayConfig.ISItemIDField, strTemp))

            End If
        Else
            ErrorMessage = "No Item SKU found for eBay Order Item ID " & eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "OrderLineItemID")
            Return XMLResult
        End If
        If eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "Item/Title") <> "" Then
            XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "Item/Title")))
        End If

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "QuantityPurchased")
        XMLResult.Add(New XElement("ItemQuantity", strTemp))
        decItemQuantity = CDec(strTemp)
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLEBayOrderItem, EBAY_ORDER_ITEM_DETAILS & "TransactionPrice")
        XMLResult.Add(New XElement("ItemUnitPrice", strTemp))
        decItemUnitPrice = CDec(strTemp)
        XMLResult.Add(New XElement("ItemSubTotal", Format(decItemUnitPrice * decItemQuantity, "0.00")))

        ' start of code added FA 14/10/13 added tax total
        XMLTaxList = XMLEBayOrderItem.XPathSelectElements(EBAY_ORDER_ITEM_DETAILS & "Taxes/TaxDetails")
        For Each XMLTaxNode In XMLTaxList
            XMLTaxTemp = XDocument.Parse(XMLTaxNode.ToString)
            If eShopCONNECTFacade.GetXMLElementText(XMLTaxTemp, "TaxDetails/Imposition") = "SalesTax" Then
                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTaxTemp, "TaxDetails/TaxOnSubtotalAmount")
                XMLResult.Add(New XElement("ItemTaxValue", strTemp))
                decItemTaxTotal = decItemTaxTotal + CDec(strTemp)
                ' start of code added TJS/FA 15/10/13
                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTaxTemp, "TaxDetails/TaxOnShippingAmount")
                decItemShippingTaxTotal = decItemShippingTaxTotal + CDec(strTemp)
                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTaxTemp, "TaxDetails/TaxOnHandlingAmount")
                decItemShippingTaxTotal = decItemShippingTaxTotal + CDec(strTemp)
                ' end of code added TJS/FA 15/10/13
                Exit For
            End If
        Next
        ' end of code added FA 14/10/13 

        decItemTotal = decItemTotal + (decItemUnitPrice * decItemQuantity)

        ItemDetails = XMLResult

    End Function

    Private Function ConvertCountryCode(ByVal strCountry As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/08/12 | TJS             | 2012.1.12 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case strCountry
            Case "United States"
                Return "United States of America"

            Case Else
                Return strCountry
        End Select


    End Function

    Private Function ConvertSKU(ByVal ProcessingMode As String, ByVal SourceSKU As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case ProcessingMode

            Case "Lerryn Test"
                Return "NONSTOCK"

            Case Else
                Return SourceSKU

        End Select

    End Function

    Private Function LogFileTimestamp(ByVal TimestampValue As Date) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        LogFileTimestamp = TimestampValue.Year & Right("00" & TimestampValue.Month, 2) & Right("00" & TimestampValue.Day, 2)
        LogFileTimestamp = LogFileTimestamp & "-" & Right("00" & TimestampValue.Hour, 2) & Right("00" & TimestampValue.Minute, 2)
        LogFileTimestamp = LogFileTimestamp & Right("00" & TimestampValue.Second, 2) & "-" & Right("000" & TimestampValue.Millisecond, 3)

    End Function

    Private Function CreateEBayXMLDate(ByVal DateToUse As Date) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Formats the Date into XML 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strXMLDate As String

        strXMLDate = DateToUse.Year & "-" & Right("00" & DateToUse.Month, 2) & "-" & Right("00" & DateToUse.Day, 2)
        strXMLDate = strXMLDate & "T" & Right("00" & DateToUse.Hour, 2) & ":" & Right("00" & DateToUse.Minute, 2)
        strXMLDate = strXMLDate & ":" & Right("00" & DateToUse.Second, 2)
        Return strXMLDate

    End Function

End Module
