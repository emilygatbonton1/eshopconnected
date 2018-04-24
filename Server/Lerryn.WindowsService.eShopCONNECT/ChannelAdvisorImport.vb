' eShopCONNECT for Connected Business - Windows Service
' Module: ChannelAdvisorImport.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 01 May 2014

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports System.IO ' TJS 25/04/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 18/03/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Module ChannelAdvisorImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decItemTotal As Decimal
    Private decItemTaxTotal As Decimal
    Private decItemShippingTotal As Decimal
    Private decShippingInsurance As Decimal ' TJS 18/12/09
    Private decShippingInsuranceTax As Decimal ' TJS 18/12/09
    Private strBuyerUserID As String ' TJS/FA 13/12/10
    Private strNotesAddn As String ' TJS/FA 13/12/10

    Public Function ProcessChannelAdvXML(ByVal SourceConfig As SourceSettings, ByVal ChannelAdvConfig As ChannelAdvisorSettings, _
        ByVal XMLOrder As XDocument, ByVal XMLNamespace As System.Xml.XmlNamespaceManager, ByVal ImportAsType As String, _
        ByRef RowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row) As Boolean ' TJS 19/08/10 TJS 04/10/10 TJS 18/03/11 TJS 04/04/11 TJS 02/12/11 TJS 01/05/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes orders received from Channel Advisor
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/12/09 | TJS             | 2009.3.09 | Function added
        ' 18/12/09 | TJS             | 2009.3.12 | Corrected handling of shipping costs and modified to ignore shipped orders
        ' 31/12/09 | TJS             | 2010.0.00 | Added payment details and modified to cater for direct SOAP connetion
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for eBay payments and related order statuses
        '                                        | plus SOurce Code constants
        ' 22/09/10 | TJs             | 2010.1.01 | Modified to cater for Payment Type translation and SourcePaymentID
        ' 29/09/10 | TJS             | 2010.1.03 | Modified to save all versions of an order if logging is enabled
        ' 01/10/10 | TJS             | 2010.1.04 | Modified to convert UTC Order Date to local time zone
        ' 04/10/10 | TJS             | 2010.1.05 | Corrected error return if no order items found and removed Order Timestamp parameter
        ' 22/10/10 | TJS/FA          | 2010.1.06 | Modified code to exit without updating quote, if the an failed payment order
        '                                          is processed twice
        ' 13/12/10 | TJS/FA          | 2010.1.12 | Modified to check either their dev passwordd or ours if their pwd is blank
        ' 14/12/10 | FA              | 2010.1.13 | Fix to above change
        ' 28/02/11 | FA              | 2010.1.23 | Modified to only update LastChannelAdvisorTimeStamp as quotes are not being 
        '                                          updated when payment is made
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version and for Channel Advisor timimng
        ' 29/03/11 | TJs             | 2011.0.04 | Modified to cater for separate payment failed CA Timestamp
        ' 04/04/11 | TJS             | 2011.0.07 | Modified to remove setting of CA timestamp 
        ' 05/04/11 | TJS             | 2011.0.08 | Corrected error handler to prevent secondary errors
        ' 08/04/11 | TJS             | 2011.0.09 | Corrected check for Payment Error email sent to cater for null
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to check for and use relevant website record
        ' 26/10/11 | TJS             | 2011.1.xx | Corrected setting of source tax values and codes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and for IS 6
        ' 26/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 23/05/13 | TJS             | 2013.1.16 | Modified to cater for Do Not Import payment options
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to set Payment date instead of relying on Order Date
        ' 01/05/14 | TJS             | 2014.0.02 | Added code to retry orders which failed to import 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLGenericOrder As XDocument, XMLChannelAdvResponse As XDocument
        Dim XMLItemTemp As XDocument, XMLItemComponentTemp As XDocument
        Dim XMLOrderHeaderNode As XElement, XMLItemNode As XElement
        Dim XMLOrderSourceNode As XElement, XMLISOrderImportNode As XElement
        Dim XMLItemComponentNode As XElement, XMLOrderTotalsNode As XElement
        Dim XMLImportResponseNode As XElement, XMLOrderResponseNode As XElement
        Dim XMLPaymentDetailsNode As XElement, XMLBuyerUserIDNode As XElement  ' TJS 31/12/09TJS/FA 13/12/10
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemComponentList As System.Collections.Generic.IEnumerable(Of XElement)

        Dim strCustomerEmail As String, strISCustomerCode As String, strErrorMessage As String
        Dim strTemp As String, strSourceOrderRef As String, strCustomerOrderRef As String ' TJS 19/08/10
        Dim bReturnValue As Boolean, bDoNotImportPayment As Boolean, iLocalTimeOffset As Integer ' TJS 23/05/13
        Dim dteOrderDate As Date, dtePaymentDate As Date ' TJS 01/10/10 TJS02/04/14

        Try
            bReturnValue = True
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12

            ' check module and connector are activated and valid
            If eShopCONNECTFacade.ValidateSource("Channel Advisor eShopCONNECTOR", CHANNEL_ADVISOR_SOURCE_CODE, "", False) Then ' TJS 19/08/10
                ' yes, has order already been imported ?
                strSourceOrderRef = eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) ' TJS 19/08/10
                strCustomerOrderRef = eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:ClientOrderIdentifier", XMLNamespace) '' TJS 19/08/10
                'FA 28/02/11 removed to only update the time stamp if a order has been amended
                'LastChannelAdvisorTimestamp = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:LastUpdateDate", XMLNamespace)) ' TJS 22/02/11

                If SourceConfig.IgnoreVoidedOrdersAndInvoices Then ' TJS 19/08/10
                    eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                        "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, CHANNEL_ADVISOR_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                        AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, ChannelAdvConfig.AccountID, AT_IS_VOIDED, "0", _
                        AT_TYPE, "Sales Order"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 19/08/10
                Else
                    eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                        "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, CHANNEL_ADVISOR_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                        AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, ChannelAdvConfig.AccountID, AT_TYPE, "Sales Order"}}, _
                        Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 19/08/10
                End If
                If eShopCONNECTDatasetGateway.CustomerSalesOrder.Count = 0 Then ' TJS 19/08/10
                    ' order not yet imported, has payment failed i.e. ImportAsType is Cancel ?
                    If ImportAsType <> "Cancel" Then ' TJS 19/08/10
                        ' no, has Channel Advisor order been marked as shipped ?
                        If eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNamespace).ToLower <> "shipped" And _
                            eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNamespace).ToLower <> "partiallyshipped" Then ' TJS 18/12/09 TJS 19/08/10
                            ' no, was order originally imported as Quote ?
                            If SourceConfig.IgnoreVoidedOrdersAndInvoices Then ' TJS 19/08/10 TJS 04/10/10
                                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                                    "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, CHANNEL_ADVISOR_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                                    AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, ChannelAdvConfig.AccountID, AT_IS_VOIDED, "0", _
                                    AT_TYPE, "Quote"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 19/08/10 TJS 04/10/10
                            Else
                                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrder.TableName, _
                                    "[ReadCustomerSalesOrderImportExport_DEV000221]", AT_SOURCE_CODE, CHANNEL_ADVISOR_SOURCE_CODE, AT_PO_CODE, strCustomerOrderRef, _
                                    AT_MERCHANT_ORDER_CODE, strSourceOrderRef, AT_STORE_MERCHANT_ID, ChannelAdvConfig.AccountID, AT_TYPE, "Quote"}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 19/08/10 TJS 04/10/10
                            End If
                            ' did we find a quote in IS and import type is still Quote ?
                            If eShopCONNECTDatasetGateway.CustomerSalesOrder.Count > 0 And ImportAsType = "Quote" Then ' TJS 04/10/10
                                ' yes, is payment now cleared ?
                                If eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNamespace).ToLower = "cleared" Then ' TJS/FA 22/10/10
                                    ' yes, build generic XML as an Order
                                    ImportAsType = "Order" ' TJS 04/10/10
                                Else
                                    ' no, payment failed on existing quote - nothing to do except return
                                    bReturnValue = True ' TJS/FA 22/10/10
                                    eShopCONNECTFacade.WriteLogProgressRecord("Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " import from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") skipped as order already imported as quote (" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "), but payment now failed") ' TJS/FA 22/10/10

                                    'FA 26/10/2010 exit if failed payment order is processed twice
                                    eShopCONNECTFacade.Dispose()

                                    Return bReturnValue
                                End If

                            End If
                            ' no, create Generic XML document
                            XMLChannelAdvResponse = New XDocument()
                            bDoNotImportPayment = False ' TJS 23/05/13

                            XMLGenericOrder = New XDocument()
                            XMLISOrderImportNode = New XElement("eShopCONNECT")

                            'SOURCE DETAILS
                            XMLOrderSourceNode = New XElement("Source")
                            XMLOrderSourceNode.Add(New XElement("SourceName", "Channel Advisor Order"))
                            XMLOrderSourceNode.Add(New XElement("SourceCode", CHANNEL_ADVISOR_SOURCE_CODE)) ' TJS 19/08/10
                            XMLISOrderImportNode.Add(XMLOrderSourceNode)

                            ' Channel Advisor doesn't have Customer IDs, they use the email address
                            strCustomerEmail = eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:BuyerEmailAddress", XMLNamespace)
                            ' check if customer exists in IS
                            eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.TableName, _
                                "ReadCustomerImportExportView_DEV000221", AT_IMPORT_CUSTOMER_ID, strCustomerEmail, AT_IMPORT_SOURCE_ID, CHANNEL_ADVISOR_SOURCE_CODE}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 19/08/10
                            If eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.Count > 0 Then
                                strISCustomerCode = eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221(0).CustomerCode
                            Else
                                strISCustomerCode = ""
                            End If

                            ' ORDER DETAILS
                            XMLItemList = XMLOrder.XPathSelectElements(CHANNEL_ADV_ORDER_ITEM_LIST, XMLNamespace)

                            XMLOrderHeaderNode = New XElement(ImportAsType) ' TJS 19/08/10
                            XMLOrderHeaderNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:ClientOrderIdentifier", XMLNamespace)))
                            XMLOrderHeaderNode.Add(New XElement("Source" & ImportAsType & "Ref", eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace))) ' TJS 19/08/10
                            strTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & CHANNEL_ADVISOR_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & ChannelAdvConfig.AccountID.Replace("'", "''") & "'") ' TJS 25/04/11 02/12/11
                            If "" & strTemp <> "" Then ' TJS 25/04/11
                                XMLOrderHeaderNode.Add(New XElement("SourceWebSiteRef", strTemp)) ' TJS 25/04/11
                            Else
                                XMLOrderHeaderNode.Add(New XElement("SourceWebSiteRef", "ChannelAdvisor"))
                            End If
                            XMLOrderHeaderNode.Add(New XElement("SourceMerchantID", ChannelAdvConfig.AccountID))
                            If ChannelAdvConfig.PricesAreTaxInclusive Then ' TJS 26/10/11
                                XMLOrderHeaderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 26/10/11
                                XMLOrderHeaderNode.Add(New XElement("TaxCodeForSourceTax", ChannelAdvConfig.TaxCodeForSourceTax)) ' TJS 26/10/11
                            End If
                            ' get Channel Advisor order date (Date + time)
                            dteOrderDate = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderTimeGMT", XMLNamespace)) ' TJS 01/10/10
                            ' get local time zone difference
                            iLocalTimeOffset = CInt(DateDiff(DateInterval.Hour, Date.UtcNow, Date.Now)) ' TJS 01/10/10
                            ' do we need to adjust order date ?
                            If iLocalTimeOffset <> 0 Then ' TJS 01/10/10
                                ' yes, add offset hours
                                dteOrderDate = dteOrderDate.AddHours(iLocalTimeOffset) ' TJS 01/10/10
                            End If
                            ' and strip out time
                            XMLOrderHeaderNode.Add(New XElement(ImportAsType & "Date", dteOrderDate.Year & "-" & Right("00" & dteOrderDate.Month, 2) & "-" & Right("00" & dteOrderDate.Day, 2))) ' TJS 19/08/10 TJS 01/10/10

                            ' take currency from first item
                            XMLOrderHeaderNode.Add(New XElement(ImportAsType & "Currency", ChannelAdvConfig.Currency)) ' TJS 19/08/10

                            ' BILLING DETAILS
                            XMLOrderHeaderNode.Add(BillingDetails(XMLOrder, XMLNamespace, XMLGenericOrder, strISCustomerCode, ImportAsType)) ' TJS 19/08/10

                            ' SHIPPING DETAILS
                            XMLOrderHeaderNode.Add(ShippingDetails(XMLOrder, XMLNamespace, XMLGenericOrder, SourceConfig, ImportAsType)) ' TJS 19/08/10

                            ' ITEM DETAILS
                            strErrorMessage = ""
                            decItemTotal = 0
                            decItemShippingTotal = 0
                            decItemTaxTotal = 0
                            strBuyerUserID = "" ' TJS/FA 13/12/10
                            strNotesAddn = "" ' TJS/FA 13/12/10
                            If XMLItemList IsNot Nothing Then
                                For Each XMLItemNode In XMLItemList
                                    XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                    XMLOrderHeaderNode.Add(ItemDetails(XMLItemTemp, XMLNamespace, XMLGenericOrder, ChannelAdvConfig, strErrorMessage))
                                    If strErrorMessage <> "" Then
                                        Exit For
                                    End If
                                Next

                                If strErrorMessage = "" Then ' TJS 02/05/10
                                    If eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNamespace).ToLower = "cleared" Then ' TJS 31/12/09
                                        XMLPaymentDetailsNode = New XElement("PaymentDetails") ' TJS 31/12/09
                                        XMLPaymentDetailsNode.Add(New XElement("PaymentMethod", "Source")) ' TJS 31/12/09
                                        If ChannelAdvConfig.EnablePaymentTypeTranslation Then ' TJS 22/09/10
                                            XMLPaymentDetailsNode.Add(New XElement("PaymentType", eShopCONNECTFacade.TranslatePaymentTypeToIS(CHANNEL_ADVISOR_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_PAYMENT_DETAILS & "q1:PaymentType", XMLNamespace), ChannelAdvConfig.PaymentType, bDoNotImportPayment))) ' TJS 22/09/10 TJS 23/05/13
                                        Else
                                            XMLPaymentDetailsNode.Add(New XElement("PaymentType", ChannelAdvConfig.PaymentType)) ' TJS 31/12/09
                                        End If
                                        ' start of code added TJS 02/04/14
                                        ' get Channel Advisor payment date (Date + time)
                                        dtePaymentDate = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentDateGMT", XMLNamespace))
                                        ' do we need to adjust order date ?
                                        If iLocalTimeOffset <> 0 Then
                                            ' yes, add offset hours
                                            dtePaymentDate = dtePaymentDate.AddHours(iLocalTimeOffset)
                                        End If
                                        ' and strip out time
                                        XMLPaymentDetailsNode.Add(New XElement("PaymentDate", dtePaymentDate.Year & "-" & Right("00" & dtePaymentDate.Month, 2) & "-" & Right("00" & dtePaymentDate.Day, 2)))
                                        ' end of code added TJS 02/04/14
                                        XMLPaymentDetailsNode.Add(New XElement("SourcePaymentID", eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_PAYMENT_DETAILS & "q1:PaymentTransactionID", XMLNamespace))) ' TJS 22/09/10
                                        If Not bDoNotImportPayment Then ' TJS 23/05/13
                                            XMLOrderHeaderNode.Add(XMLPaymentDetailsNode) ' TJS 31/12/09
                                        End If
                                    End If

                                    ' ORDER TOTALS
                                    XMLItemComponentList = XMLOrder.XPathSelectElements(CHANNEL_ADV_ORDER_ITEM_PRICE_LIST, XMLNamespace)
                                    For Each XMLItemComponentNode In XMLItemComponentList
                                        XMLItemComponentTemp = XDocument.Parse(XMLItemComponentNode.ToString)
                                        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLItemComponentTemp, CHANNEL_ADV_ORDER_ITEM_PRICE_DETAILS & "q1:UnitPrice", XMLNamespace)
                                        Select Case eShopCONNECTFacade.GetXMLElementText(XMLItemComponentTemp, CHANNEL_ADV_ORDER_ITEM_PRICE_DETAILS & "q1:LineItemType", XMLNamespace)
                                            Case "Shipping"
                                                decItemShippingTotal = CDec(strTemp) ' TJS 18/12/09

                                            Case "ShippingInsurance"
                                                decShippingInsurance = CDec(strTemp) ' TJS 18/12/09

                                            Case "SalesTax"
                                                decItemTaxTotal = CDec(strTemp) ' TJS 18/12/09

                                            Case "VATShipping"
                                                decShippingInsuranceTax = CDec(strTemp)

                                        End Select
                                    Next
                                    decItemShippingTotal = decItemShippingTotal + decShippingInsurance ' TJS 18/12/09
                                    decItemTaxTotal = decItemTaxTotal + decShippingInsuranceTax ' TJS 18/12/09

                                    XMLOrderTotalsNode = New XElement(ImportAsType & "Totals") ' TJS 19/08/10
                                    XMLOrderTotalsNode.Add(New XElement("SubTotal", Format(decItemTotal, "0.00")))
                                    XMLOrderTotalsNode.Add(New XElement("Shipping", Format(decItemShippingTotal, "0.00")))
                                    XMLOrderTotalsNode.Add(New XElement("Tax", Format(decItemTaxTotal + decShippingInsuranceTax, "0.00")))
                                    XMLOrderTotalsNode.Add(New XElement("Total", Format(decItemTotal + decItemShippingTotal + decItemTaxTotal, "0.00")))
                                    XMLOrderHeaderNode.Add(XMLOrderTotalsNode)


                                    'TJS/FA 13/12/10 start
                                    If strBuyerUserID <> "" Then
                                        XMLBuyerUserIDNode = New XElement("CustomField")
                                        XMLBuyerUserIDNode.SetAttributeValue("FieldName", "ImportSourceBuyerID_DEV000221")
                                        XMLBuyerUserIDNode.Value = strBuyerUserID
                                        XMLItemNode = XMLOrderHeaderNode.XPathSelectElement("BillingDetails/Customer")
                                        XMLItemNode.Add(XMLBuyerUserIDNode)
                                        If strNotesAddn <> "" Then
                                            XMLOrderHeaderNode.Add(New XElement("CustomerComments", strNotesAddn))
                                        End If
                                    End If
                                    'TJS/FA 13/12/10 end
                                    XMLISOrderImportNode.Add(XMLOrderHeaderNode)

                                    XMLISOrderImportNode.Add(New XElement(ImportAsType & "Count", "1")) ' TJS 19/08/10
                                    XMLGenericOrder.Add(XMLISOrderImportNode)

                                    strTemp = SourceConfig.XMLImportFileSavePath ' TJS 19/08/10
                                    If strTemp <> "" Then
                                        If strTemp.Substring(strTemp.Length - 1, 1) <> "\" Then
                                            strTemp = strTemp & "\"
                                        End If
                                        If My.Computer.FileSystem.FileExists(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".xml") Or _
                                            My.Computer.FileSystem.FileExists(strTemp & "GenericXML_for_ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".xml") Then ' TJS 29/09/10
                                            XMLOrder.Save(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & "_" & LogFileTimestamp(Now) & ".xml") ' TJS 29/09/10
                                            XMLGenericOrder.Save(strTemp & "GenericXML_for_ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & "_" & LogFileTimestamp(Now) & ".xml") ' TJS 29/09/10
                                        Else
                                            XMLOrder.Save(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".xml")
                                            XMLGenericOrder.Save(strTemp & "GenericXML_for_ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".xml")
                                        End If
                                    End If

                                    ' Generic XML Built, was order originally imported as Quote ?
                                    If eShopCONNECTDatasetGateway.CustomerSalesOrder.Count = 0 Then ' TJS 19/08/10
                                        ' no, now do call to create order and customer (where customer does not exist)
                                        strTemp = "" ' TJS 04/10/10
                                        If ImportAsType = "Order" Then ' TJS 19/08/10
                                            XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "") ' TJS 19/08/10
                                        Else
                                            XMLImportResponseNode = eShopCONNECTFacade.XMLQuoteImport(XMLGenericOrder) ' TJS 19/08/10
                                        End If
                                    Else
                                        ' yes, now do call to update quote, update customer if details were blank and import updated order
                                        strTemp = eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode ' TJS 04/10/10
                                        XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode) ' TJS 19/08/10
                                    End If
                                    XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                                    If XMLOrderResponseNode IsNot Nothing Then
                                        If XMLOrderResponseNode.Value = "Success" Then
                                            ' don't send any response now, new order DB trigger will create response
                                            If ImportAsType = "Order" Then ' TJS 19/08/10
                                                ' was order originally imported as Quote ?
                                                If strTemp = "" Then ' TJS 04/10/10
                                                    ' no
                                                    eShopCONNECTFacade.WriteLogProgressRecord("Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " successfully imported from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value) ' TJS 19/08/10
                                                Else
                                                    ' yes
                                                    eShopCONNECTFacade.WriteLogProgressRecord("Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " successfully imported from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value & " - Quote " & strTemp & " has been closed") ' TJS 19/08/10 TJS 04/10/10
                                                End If
                                                ' is payment status set to Failed ?
                                                If eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNamespace).ToLower = "failed" Then ' TJS 04/10/10
                                                    ' not shipped, warn that payment failed and put order on hold
                                                    eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrderWorkflow SET Stage = 'Approve Credit' WHERE SalesOrderCode = '" & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value & "'", Nothing) ' TJS 04/10/10
                                                    strErrorMessage = "Payment Status for Channel Advisor Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & _
                                                        " (Interprise Sales Order " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value & ") from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") has changed to Failed.  Order not yet shipped and workflow status set to Approve Credit." ' TJS 04/10/10
                                                    eShopCONNECTFacade.SendPaymentErrorEmail(SourceConfig.XMLConfig, strErrorMessage) ' TJS 04/10/10
                                                End If
                                            Else
                                                eShopCONNECTFacade.WriteLogProgressRecord("Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " successfully imported from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") as Quote " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/QuoteNumber").Value) ' TJS 19/08/10
                                            End If
                                            If RowOrderToRetry IsNot Nothing Then ' TJS 01/05/14
                                                RowOrderToRetry.SuccessfullyImported_DEV000221 = True ' TJS 01/05/14
                                                RowOrderToRetry.RetryCount_DEV000221 = 0 ' TJS 01/05/14
                                            End If
                                        Else
                                            bReturnValue = False
                                            eShopCONNECTFacade.WriteLogProgressRecord("Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " import from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") failed") ' TJS 19/08/10

                                            ' start of code added TJS 01/05/14
                                            If RowOrderToRetry IsNot Nothing Then
                                                If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                                                    RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                                Else
                                                    RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                                                End If
                                                RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                                            Else
                                                RowOrderToRetry = m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.NewLerrynImportExportSourceOrdersToRetry_DEV000221Row()
                                                RowOrderToRetry.SourceCode_DEV000221 = CHANNEL_ADVISOR_SOURCE_CODE
                                                RowOrderToRetry.StoreMerchantID_DEV000221 = ChannelAdvConfig.AccountID
                                                RowOrderToRetry.MerchantOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace)
                                                RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                                RowOrderToRetry.RetryCount_DEV000221 = 6
                                            End If
                                            ' end of code added TJS 01/05/14
                                        End If
                                    Else
                                        bReturnValue = False
                                        eShopCONNECTFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessChannelAdvXML", XMLImportResponseNode.ToString, "")

                                        ' start of code added TJS 01/05/14
                                        If RowOrderToRetry IsNot Nothing Then
                                            If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                                                RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                            Else
                                                RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                                        End If
                                        RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                                    Else
                                            RowOrderToRetry = m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.NewLerrynImportExportSourceOrdersToRetry_DEV000221Row()
                                            RowOrderToRetry.SourceCode_DEV000221 = CHANNEL_ADVISOR_SOURCE_CODE
                                            RowOrderToRetry.StoreMerchantID_DEV000221 = ChannelAdvConfig.AccountID
                                            RowOrderToRetry.MerchantOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace)
                                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                            RowOrderToRetry.RetryCount_DEV000221 = 6
                                        End If
                                        ' end of code added TJS 01/05/14
                                    End If
                                Else
                                    bReturnValue = False ' TJS 04/10/10
                                    m_ImportExportConfigFacade.BuildXMLErrorResponseNodeAndEmail("Error", "013", strErrorMessage, m_ImportExportConfigFacade.SourceConfig, _
                                        "ProcessChannelAdvXML", XMLGenericOrder.ToString) ' TJS 04/10/10 TJS 18/03/11

                                    ' start of code added TJS 01/05/14
                                    If RowOrderToRetry IsNot Nothing Then
                                        If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                        Else
                                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                                        End If
                                        RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                                    Else
                                        RowOrderToRetry = m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.NewLerrynImportExportSourceOrdersToRetry_DEV000221Row()
                                        RowOrderToRetry.SourceCode_DEV000221 = CHANNEL_ADVISOR_SOURCE_CODE
                                        RowOrderToRetry.StoreMerchantID_DEV000221 = ChannelAdvConfig.AccountID
                                        RowOrderToRetry.MerchantOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace)
                                        RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                        RowOrderToRetry.RetryCount_DEV000221 = 6
                                    End If
                                    ' end of code added TJS 01/05/14
                                End If
                            Else
                                bReturnValue = False ' TJS 04/10/10
                                m_ImportExportConfigFacade.BuildXMLErrorResponseNodeAndEmail("Error", "013", "No Order Items found", m_ImportExportConfigFacade.SourceConfig, _
                                    "ProcessChannelAdvXML", XMLGenericOrder.ToString) ' TJS 04/10/10 TJS 18/03/11

                                ' start of code added TJS 01/05/14
                                If RowOrderToRetry IsNot Nothing Then
                                    If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                                        RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                    Else
                                        RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)

                                    End If
                                    RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                                Else
                                    RowOrderToRetry = m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.NewLerrynImportExportSourceOrdersToRetry_DEV000221Row()
                                    RowOrderToRetry.SourceCode_DEV000221 = CHANNEL_ADVISOR_SOURCE_CODE
                                    RowOrderToRetry.StoreMerchantID_DEV000221 = ChannelAdvConfig.AccountID
                                    RowOrderToRetry.MerchantOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace)
                                    RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                    RowOrderToRetry.RetryCount_DEV000221 = 6
                                End If
                                ' end of code added TJS 01/05/14
                            End If

                        Else
                            ' order not yet imported, has been partially or fully shipped - ignore it
                            bReturnValue = False ' TJS 31/12/09
                            eShopCONNECTFacade.WriteLogProgressRecord("Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " import from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") skipped as order already shipped") ' TJS 31/12/09 TJS 19/08/10

                            ' start of code added TJS 01/05/14
                            If RowOrderToRetry IsNot Nothing Then
                                RowOrderToRetry.RetryCount_DEV000221 = 0
                            End If
                            ' end of code added TJS 01/05/14
                        End If
                    Else
                        ' order not yet imported, payment has failed - ignore it
                        bReturnValue = False ' TJS 19/08/10
                        eShopCONNECTFacade.WriteLogProgressRecord("Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " import from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") skipped as payment failed before import.") ' TJS 19/08/10

                        ' start of code added TJS 01/10/10
                        strTemp = SourceConfig.XMLImportFileSavePath
                        If strTemp <> "" Then
                            If strTemp.Substring(strTemp.Length - 1, 1) <> "\" Then
                                strTemp = strTemp & "\"
                            End If
                            If My.Computer.FileSystem.FileExists(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".ToString") Then
                                XMLOrder.Save(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & "_" & LogFileTimestamp(Now) & ".ToString")
                            Else
                                XMLOrder.Save(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".ToString")
                            End If
                        End If
                        ' end of code added TJS 01/10/10

                        ' start of code added TJS 01/05/14
                        If RowOrderToRetry IsNot Nothing Then
                            RowOrderToRetry.RetryCount_DEV000221 = 0
                        End If
                        ' end of code added TJS 01/05/14
                    End If
                Else
                    ' order already imported, is payment status now set to Failed ?
                    If eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNamespace).ToLower = "failed" Then ' TJS 09/06/10
                        ' yes, check if email already sent
                        strTemp = eShopCONNECTFacade.GetField("PaymentFailedEmailSent_DEV000221", "CustomerSalesOrder", "SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'") ' TJS 08/04/11
                        If strTemp = "" OrElse Not CBool(strTemp) Then ' TJS 18/03/11 TJS 08/04/11
                            ' no, has order been shipped ?
                            strTemp = eShopCONNECTFacade.GetField("Stage", "CustomerSalesOrderWorkflow", "SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'") ' TJS 19/08/10
                            If strTemp = "Completed" Then ' TJS 09/06/10
                                ' yes, has it been partially shipped i.e. a Back Order has been created
                                strTemp = eShopCONNECTFacade.GetField("SELECT Stage FROM CustomerSalesOrderWorkflow WHERE SalesOrderCode in (SELECT SalesOrderCode FROM CustomerSalesOrder WHERE Type = 'Back Order' AND RootDocumentCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "')", CommandType.Text, Nothing) ' TJS 19/08/10
                                If strTemp = "" Then ' TJS 19/08/10
                                    ' no, warn that order already shipped
                                    strErrorMessage = "Payment Status for Channel Advisor Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & _
                                        " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & ") from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") has changed to Failed after Order has been shipped." ' TJS 19/08/10
                                ElseIf strTemp = "Completed" Then ' TJS 19/08/10
                                    ' yes and Back Order also shipped, warn that order already shipped
                                    strErrorMessage = "Payment Status for Channel Advisor Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & _
                                        " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & ") from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") has changed to Failed after Order, and a related Back Order, have been shipped." ' TJS 19/08/10
                                Else
                                    ' yes, warn that order is part shipped and put Back Order on hold
                                    eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrderWorkflow SET Stage = 'Approve Credit' WHERE Stage <> 'Completed' AND SalesOrderCode IN (SELECT SalesOrderCode FROM CustomerSalesOrder WHERE Type = 'Back Order' AND RootDocumentCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "')", Nothing) ' TJS 19/08/10
                                    strErrorMessage = "Payment Status for Channel Advisor Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & _
                                        " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & ") from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") has changed to Failed after Order has been part shipped - Back Order workflow status now set to Approve Credit." ' TJS 19/08/10
                                End If
                            Else
                                ' not shipped, warn that payment failed and put order on hold
                                eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrderWorkflow SET Stage = 'Approve Credit' WHERE SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'", Nothing) ' TJS 19/08/10
                                strErrorMessage = "Payment Status for Channel Advisor Order " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & _
                                    " (Interprise Sales Order " & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & ") from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") has changed to Failed.  Order not yet shipped and workflow status now set to Approve Credit." ' TJS 19/08/10

                            End If
                            eShopCONNECTFacade.SendPaymentErrorEmail(SourceConfig.XMLConfig, strErrorMessage) ' TJS 09/06/10
                            eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, "UPDATE CustomerSalesOrder SET PaymentFailedEmailSent_DEV000221 = 1 WHERE SalesOrderCode = '" & eShopCONNECTDatasetGateway.CustomerSalesOrder(0).SalesOrderCode & "'", Nothing) ' TJS 22/02/11

                            ' start of code added TJS 29/09/10
                            strTemp = SourceConfig.XMLImportFileSavePath
                            If strTemp <> "" Then
                                If strTemp.Substring(strTemp.Length - 1, 1) <> "\" Then
                                    strTemp = strTemp & "\"
                                End If
                                If My.Computer.FileSystem.FileExists(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".ToString") Then
                                    XMLOrder.Save(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & "_" & LogFileTimestamp(Now) & ".ToString")
                                Else
                                    XMLOrder.Save(strTemp & "ChannelAdvisor_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & ".ToString")
                                End If
                            End If
                            ' end of code added TJS 29/09/10

                        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNamespace).ToLower = "cleared" And _
                            (eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNamespace).ToLower = "shipped" Or _
                            eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNamespace).ToLower = "partiallyshipped") Then
                            ' payment status is still Cleared and shipping status is Shipped or Partially Shipped so ignore as status change 
                            ' likely to be the shipping status being updated

                        Else
                            ' order updated but payment not marked as failed - log details for now
                            'eShopCONNECTFacade.WriteLogProgressRecord(PRODUCT_NAME, "Channel Advisor Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNamespace) & " from Account " & ChannelAdvConfig.AccountName & " (" & ChannelAdvConfig.AccountID & ") already imported, status changed, but payment not failed - " & XMLOrder.ToString) ' TJS 19/08/10 TJS 01/10/10

                        End If
                    End If
                    ' start of code added TJS 01/05/14
                    If RowOrderToRetry IsNot Nothing Then
                        RowOrderToRetry.RetryCount_DEV000221 = 0
                    End If
                    ' end of code added TJS 01/05/14
                End If
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(SourceConfig.XMLConfig, "ProcessChannelAdvXML", ex, XMLOrder.ToString) ' TJS 31/12/09 TJS 05/04/11
            bReturnValue = False

        Finally
            eShopCONNECTFacade.Dispose()

        End Try
        Return bReturnValue

    End Function

    Private Function BillingDetails(ByVal XMLChannelAdvOrder As XDocument, ByVal XMLNamespace As System.Xml.XmlNamespaceManager, _
        ByRef XMLGenericOrder As XDocument, ByVal ISCustomerCode As String, ByVal ImportAsType As String) As XElement ' TJS 19/08/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/12/09 | TJS             | 2009.3.09 | Function added
        ' 18/12/09 | TJS             | 2009.3.12 | Modified to cater for Last name not being mandatory in input data
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for eBay payments and related order statuses
        ' 01/10/10 | TJS             | 2010.1.04 | Corrected detection of blank billing name
        ' 18/10/10 | FA              | 2010.1.06 | Corrected detection of email address
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to cater for customers only giving a first or last name
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBill As XElement, XMLBillingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strTemp As String

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBill = New XElement("Customer")
        ' Channel Advisor doesn't have Customer IDs, they use the email address
        XMLCustomerBill.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:BuyerEmailAddress", XMLNamespace)))
        XMLCustomerBill.Add(New XElement("ISCustomerCode", ISCustomerCode))
        ' is there a billing name ?
        If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:FirstName", XMLNamespace) <> "" Or _
            eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:LastName", XMLNamespace) <> "" Then ' TJS 01/10/10
            ' yes, use it
            XMLCustomerBill.Add(New XElement("NamePrefix", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:Title", XMLNamespace)))
            If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:LastName", XMLNamespace) <> "" Then ' TJS 18/12/09
                If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:FirstName", XMLNamespace) <> "" Then ' TJS 02/04/14
                	XMLCustomerBill.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:FirstName", XMLNamespace)))
                Else
                    XMLCustomerBill.Add(New XElement("FirstName", ".")) ' TJS 02/04/14
                End If
                XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:LastName", XMLNamespace)))
            Else
                XMLCustomerBill.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:FirstName", XMLNamespace))) ' TJS 18/12/09 TJS 02/04/14
                XMLCustomerBill.Add(New XElement("LastName", ".")) ' TJS 02/04/14
            End If
            XMLCustomerBill.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:CompanyName", XMLNamespace)))
            XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:PhoneNumberEvening", XMLNamespace)))
            XMLCustomerBill.Add(New XElement("WorkPhone", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:PhoneNumberDay", XMLNamespace)))
            'FA 18/10/10 Modified to refer to orderdetail, not billing details
            XMLCustomerBill.Add(New XElement("Email", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:BuyerEmailAddress", XMLNamespace)))
        Else
            ' no, use shipping address
            XMLCustomerBill.Add(New XElement("NamePrefix", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Title", XMLNamespace)))
            If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:LastName", XMLNamespace) <> "" Then ' TJS 18/12/09
                If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace) <> "" Then ' TJS 02/04/14
                	XMLCustomerBill.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace)))
                Else
                    XMLCustomerBill.Add(New XElement("FirstName", ".")) ' TJS 02/04/14
                End If
                XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:LastName", XMLNamespace)))
            ElseIf ImportAsType = "Quote" And eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace) = "" Then ' TJS 19/08/10
                ' When importing as a Quote, may not have any customer details other than email address 
                ' if so, need to ensure record is not rejected so we use a full stop in the name
                XMLCustomerBill.Add(New XElement("FirstName", ".")) ' TJS 02/04/14
                XMLCustomerBill.Add(New XElement("LastName", ".")) ' TJS 19/08/10
            Else
                XMLCustomerBill.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace))) ' TJS 18/12/09 TJS 02/04/14
                XMLCustomerBill.Add(New XElement("LastName", ".")) ' TJS 02/04/14
            End If
            XMLCustomerBill.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:CompanyName", XMLNamespace)))
            XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:PhoneNumberEvening", XMLNamespace)))
            XMLCustomerBill.Add(New XElement("WorkPhone", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:PhoneNumberDay", XMLNamespace)))
            'FA 18/10/10 Modified to refer to orderdetail, not billing details
            XMLCustomerBill.Add(New XElement("Email", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_DETAILS & "q1:BuyerEmailAddress", XMLNamespace)))
        End If

        ' BILLING ADDRESS
        XMLBillingAddress = New XElement("BillingAddress")
        ' is there a billing address ?
        If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:AddressLine1", XMLNamespace) <> "" Then
            ' yes, use it
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:AddressLine1", XMLNamespace)
            If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:AddressLine2", XMLNamespace) <> "" Then
                strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:AddressLine2", XMLNamespace)
            End If
            XMLBillingAddress.Add(New XElement("Address", strTemp))
            XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:City", XMLNamespace)))
            ' need to ensure Country matches those used in IS
            strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:CountryCode", XMLNamespace))
            If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:Region", XMLNamespace).Length <= 2 Then
                XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:Region", XMLNamespace)))
            Else
                XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:Region", XMLNamespace)))
            End If
            XMLBillingAddress.Add(New XElement("Country", strCountry))
            XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_BILLING_DETAILS & "q1:PostalCode", XMLNamespace)))
        Else
            ' no, use shipping address
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:AddressLine1", XMLNamespace)
            If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:AddressLine2", XMLNamespace) <> "" Then
                strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:AddressLine2", XMLNamespace)
            End If
            If ImportAsType = "Quote" And strTemp = "" Then ' TJS 19/08/10
                ' When importing as a Quote, may not have any customer details other than email address 
                ' if so, need to ensure record is not rejected so we use a full stop in the address
                strTemp = "." ' TJS 19/08/10
            End If
            XMLBillingAddress.Add(New XElement("Address", strTemp))
            XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:City", XMLNamespace)))
            ' need to ensure Country matches those used in IS
            strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:CountryCode", XMLNamespace))
            If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Region", XMLNamespace).Length <= 2 Then
                XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Region", XMLNamespace)))
            Else
                XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Region", XMLNamespace)))
            End If
            XMLBillingAddress.Add(New XElement("Country", strCountry))
            XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:PostalCode", XMLNamespace)))
        End If

        XMLResult.Add(XMLCustomerBill)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult
    End Function

    Private Function ShippingDetails(ByVal XMLChannelAdvOrder As XDocument, ByVal XMLNamespace As System.Xml.XmlNamespaceManager, _
        ByRef XMLGenericOrder As XDocument, ByVal SourceConfig As SourceSettings, ByVal ImportAsType As String) As XElement ' TJS 19/08/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/12/09 | TJS             | 2009.3.09 | Function added
        ' 18/12/09 | TJS             | 2009.3.12 | Modified to cater for Last name not being mandatory in input data
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to directly use config seting values
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for eBay payments and related order statuses
        '                                        | plus Source Code constants
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 02/04/14 | TJS             | 2014.0.01 | Modified to cater for customers only giving a first or last name
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strTemp As String, strShippingMethodGroup As String


        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        XMLCustomerShip.Add(New XElement("NamePrefix", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Title", XMLNamespace)))
        If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:LastName", XMLNamespace) <> "" Then ' TJS 18/12/09
            If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace) <> "" Then ' TJS 02/04/14
            	XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace)))
            Else
                XMLCustomerShip.Add(New XElement("FirstName", ".")) ' TJS 02/04/14
            End If
            XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:LastName", XMLNamespace)))
        ElseIf ImportAsType = "Quote" And eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace) = "" Then ' TJS 19/08/10
            ' When importing as a Quote, may not have any customer details other than email address 
            ' if so, need to ensure record is not rejected so we use a full stop in the name
            XMLCustomerShip.Add(New XElement("FirstName", ".")) ' TJS 02/04/14
            XMLCustomerShip.Add(New XElement("LastName", ".")) ' TJS 19/08/10
        Else
            XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:FirstName", XMLNamespace))) ' TJS 18/12/09 TJS 02/04/14
            XMLCustomerShip.Add(New XElement("LastName", ".")) ' TJS 02/04/14
        End If
        XMLCustomerShip.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:CompanyName", XMLNamespace)))
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:PhoneNumberDay", XMLNamespace)))

        ' SHIPPING ADDRESS
        XMLShippingAddress = New XElement("ShippingAddress")
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:AddressLine1", XMLNamespace)
        If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:AddressLine2", XMLNamespace) <> "" Then
            strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:AddressLine2", XMLNamespace)
        End If
        If ImportAsType = "Quote" And strTemp = "" Then ' TJS 19/08/10
            ' When importing as a Quote, may not have any customer details other than email address 
            ' if so, need to ensure record is not rejected so we use a full stop in the address
            strTemp = "." ' TJS 19/08/10
        End If
        XMLShippingAddress.Add(New XElement("Address", strTemp))
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:City", XMLNamespace)))
        ' need to ensure Country matches those used in IS
        strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:CountryCode", XMLNamespace))
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Region", XMLNamespace).Length <= 2 Then
            XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Region", XMLNamespace)))
        Else
            XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:Region", XMLNamespace)))
        End If
        XMLShippingAddress.Add(New XElement("Country", strCountry))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS & "q1:PostalCode", XMLNamespace)))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If SourceConfig.EnableDeliveryMethodTranslation And _
            eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS_METHOD & "q1:ShippingCarrier", XMLNamespace) <> "" Then
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS(CHANNEL_ADVISOR_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS_METHOD & "q1:ShippingCarrier", XMLNamespace), eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS_METHOD & "q1:ShippingClass", XMLNamespace), strShippingMethodGroup))) ' TJS 12/081/0
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS_METHOD & "q1:ShippingCarrier", XMLNamespace) = "" Then
            XMLResult.Add(New XElement("ShippingMethod", SourceConfig.DefaultShippingMethod)) ' TJS 19/08/10
            XMLResult.Add(New XElement("ShippingMethodGroup", SourceConfig.DefaultShippingMethodGroup)) ' TJS 19/08/10

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS_METHOD & "q1:ShippingClass", XMLNamespace)))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrder, CHANNEL_ADV_ORDER_SHIPPING_DETAILS_METHOD & "q1:ShippingCarrier", XMLNamespace)))
        End If

        Return XMLResult
    End Function

    Private Function ItemDetails(ByVal XMLChannelAdvOrderItem As XDocument, ByVal XMLNamespace As System.Xml.XmlNamespaceManager, _
        ByRef XMLGenericOrder As XDocument, ByVal ChannelAdvConfig As ChannelAdvisorSettings, ByRef ErrorMessage As String) As XElement ' TJS 19/08/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/12/09 | TJS             | 2009.3.09 | Function added
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for CustomSKUProcessing and EnableSKUAliasLookup
        '                                        | plus Source COde constants
        ' 13/12/10 | TJS/FA          | 2010.1.12 | Modified to cater for Source Buyer ID
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221 and corrected SKU Alias processing
        ' 22/05/14 | TJS             | 2014.0.01 | Modified for Unit of Measure on SKU Alias lookup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, strTemp As String
        Dim decItemUnitPrice As Decimal, decItemQuantity As Decimal

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        XMLResult.Add(New XElement("SourceItemPurchaseID", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:SalesSourceID", XMLNamespace)))
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:SKU", XMLNamespace) ' TJS 19/08/10
        ' is there an SKU for the item ?
        If strTemp <> "" Then ' TJS 19/08/10
            ' yes, has Custom SKU Processing been enabled ?
            If ChannelAdvConfig.CustomSKUProcessing <> "" Then ' TJS 19/08/10
                ' yes, convert SKU
                strTemp = ConvertSKU(ChannelAdvConfig.CustomSKUProcessing, strTemp) ' TJS 19/08/10
            End If
            If ChannelAdvConfig.EnableSKUAliasLookup Then ' TJS 19/08/10
                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", CHANNEL_ADVISOR_SOURCE_CODE, "@SourceSKU", strTemp}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 19/08/10 TJS 03/07/13
                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then ' TJS 19/08/10 TJS 03/07/13
                    strTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 ' TJS 19/08/10 TJS 03/07/13
                    XMLResult.Add(New XElement("ISItemCode", strTemp)) ' TJS 19/08/10 TJS 03/07/13
                Else
                    XMLResult.Add(New XElement("IS" & ChannelAdvConfig.ISItemIDField, strTemp)) ' TJS 19/08/10
                End If

            Else
                XMLResult.Add(New XElement("IS" & ChannelAdvConfig.ISItemIDField, strTemp)) ' TJS 19/08/10

            End If
        Else
            ErrorMessage = "No Item SKU found for Channel Advisor Order Item ID " & eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:SalesSourceID", XMLNamespace)
            Return XMLResult
        End If
        If eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:Title", XMLNamespace) <> "" Then
            XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:Title", XMLNamespace)))
        End If

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:Quantity", XMLNamespace)
        XMLResult.Add(New XElement("ItemQuantity", strTemp))
        decItemQuantity = CDec(strTemp)
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:UnitPrice", XMLNamespace)
        XMLResult.Add(New XElement("ItemUnitPrice", strTemp))
        decItemUnitPrice = CDec(strTemp)
        XMLResult.Add(New XElement("ItemSubTotal", Format(decItemUnitPrice * decItemQuantity, "0.00")))
        decItemTotal = decItemTotal + (decItemUnitPrice * decItemQuantity)

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:ShippingCost", XMLNamespace)
        If strTemp <> "" Then
            decItemShippingTotal = decItemShippingTotal + CDec(strTemp)
        End If

        If Not ChannelAdvConfig.PricesAreTaxInclusive Then
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:TaxCost", XMLNamespace)
            If strTemp <> "" Then
                XMLResult.Add(New XElement("ItemTaxValue", strTemp))
                decItemTaxTotal = decItemTaxTotal + CDec(strTemp)
            End If
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:ShippingTaxCost", XMLNamespace)
            If strTemp <> "" Then
                decItemTaxTotal = decItemTaxTotal + CDec(strTemp)
            End If
        End If

        ' start of code added TJS/FA 13/12/10
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLChannelAdvOrderItem, CHANNEL_ADV_ORDER_ITEM_DETAILS & "q1:BuyerUserID", XMLNamespace)
        If strTemp <> "" Then
            If strBuyerUserID = "" Then
                strBuyerUserID = strTemp
            Else
                If strBuyerUserID <> strTemp Then
                    If strNotesAddn = "" Then
                        strNotesAddn = strBuyerUserID & ", " & strTemp
                    Else
                        strNotesAddn = strNotesAddn & ", " & strTemp
                    End If
                End If
            End If
        End If
        ' end of code added TJS/FA 13/12/10
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
        ' 15/12/09 | TJS             | 2009.3.09 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' get Country record IS
        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.SystemCountry.TableName, _
            "ReadSystemCountryImportExport_DEV000221", AT_ISO_CODE, strCountry}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        If eShopCONNECTDatasetGateway.SystemCountry.Count > 0 Then
            ConvertCountryCode = eShopCONNECTDatasetGateway.SystemCountry(0).CountryCode
        Else
            ConvertCountryCode = strCountry
        End If

    End Function

    Public Function ChannelAdvXMLDate(ByVal DateToUse As Date, ByVal IncludeMilliSeconds As Boolean) As String ' TJS 23/11/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 31/12/09 | TJS             | 2010.0.00 | Function added
        ' 19/08/10 | TJS             | 2010.1.00 | Added milliseconds to XML date
        ' 23/11/10 | TJS/FA          | 2010.1.09 | Modifed to make milliseconds optional
        '                                          We suspect this was causing a 'input string
        '                                          not in correct format' error when sending a status
        '                                          update to CA
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strXMLDate As String

        strXMLDate = Microsoft.VisualBasic.Right("0000" & DateToUse.Year.ToString, 4) & "-" & Microsoft.VisualBasic.Right("00" & DateToUse.Month.ToString, 2)
        strXMLDate = strXMLDate & "-" & Microsoft.VisualBasic.Right("00" & DateToUse.Day.ToString, 2) & "T" & Microsoft.VisualBasic.Right("00" & DateToUse.Hour.ToString, 2)
        strXMLDate = strXMLDate & ":" & Microsoft.VisualBasic.Right("00" & DateToUse.Minute.ToString, 2) & ":" & Microsoft.VisualBasic.Right("00" & DateToUse.Second.ToString, 2)
        If IncludeMilliSeconds Then ' TJS/FA 23/11/10
            strXMLDate = strXMLDate & "." & Microsoft.VisualBasic.Right("000" & DateToUse.Millisecond.ToString, 3) ' TJS 19/08/10
        End If
        Return strXMLDate

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
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iHyphenPosn As Integer, iLoop As Integer, strTemp As String, bIsNumeric As Boolean

        Select Case ProcessingMode
            Case "JL Concepts"
                ' need to remove -xx or -xxx from end of SKU
                ' does SKU contain hyphen (use InStrRev so that we get the last one) ?
                iHyphenPosn = InStrRev(SourceSKU, "-")
                If iHyphenPosn > 0 Then
                    ' yes, is remainder of SKU after hyphen char either 2 or 3 numeric chars ?
                    ' get remainder after hyphen
                    strTemp = Mid(SourceSKU, iHyphenPosn + 1)
                    If Len(strTemp) = 2 Or Len(strTemp) = 3 Then
                        bIsNumeric = True
                        For iLoop = 1 To Len(strTemp)
                            If Asc(Mid(strTemp, iLoop, 1)) > Asc("9") Or Asc(Mid(strTemp, iLoop, 1)) < Asc("0") Then
                                bIsNumeric = False
                            End If
                        Next
                        If bIsNumeric Then
                            Return Left(SourceSKU, iHyphenPosn - 1)
                        Else
                            Return SourceSKU
                        End If
                    Else
                        ' lenght of remainder after hyphen not 2 or 3
                        Return SourceSKU
                    End If
                Else
                    Return SourceSKU
                End If

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
        ' 01/10/10 | TJS             | 2010.1.04 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        LogFileTimestamp = TimestampValue.Year & Right("00" & TimestampValue.Month, 2) & Right("00" & TimestampValue.Day, 2)
        LogFileTimestamp = LogFileTimestamp & "-" & Right("00" & TimestampValue.Hour, 2) & Right("00" & TimestampValue.Minute, 2)
        LogFileTimestamp = LogFileTimestamp & Right("00" & TimestampValue.Second, 2) & "-" & Right("000" & TimestampValue.Millisecond, 3)

    End Function
End Module
