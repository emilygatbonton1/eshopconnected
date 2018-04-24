' eShopCONNECT for Connected Business - Windows Service
' Module: MagentoImport.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 22 May 2014

Imports System.IO
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Interprise.Framework.Base.Shared.Const ' TJS 20/04/12
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports Lerryn.Facade.ImportExport

Module MagentoImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decItemTotal As Decimal
    Private decItemTaxTotal As Decimal
    Private decItemShippingTotal As Decimal

    Public Function ProcessMagentoXML(ByVal SourceConfig As SourceSettings, ByVal MagentoConfig As MagentoSettings, ByVal strInputXML As String, _
       ByRef OrderTimestamp As Date, ByRef AlreadyImported As Boolean, ByRef MagentoConnection As Lerryn.Facade.ImportExport.MagentoSOAPConnector, _
        ByRef RowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row) As Boolean ' TJS 18/03/11 TJS 27/03/ TJS 04/01/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes files received from Magento
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Copied from eShopCONNECT for Tradepoint
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version and for Magento Guest checkout
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to check for and use relevant website record
        ' 04/05/11 | TJs             | 2011.0.13 | Modified to prevent errors when no company entered in address etc
        ' 26/10/11 | TJS             | 2011.1.xx | Corrected setting of source tax values and codes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 27/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 20/04/12 | TJS             | 2012.1.02 | Modified to cater for SplitSKUSeparatorCharacters setting
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout and 
        '                                        | modified to handle PaymentTypeTranslation and non credit card payment
        ' 30/04/13 | TJS             | 2013.1.11 | Modified to cater for CreateCustomerForGuestCheckout
        ' 08/05/13 | TJS             | 2013.1.13 | Modified to cater for nested SplitSKUSeparatorCharacters values
        '                                        | and Magento IncludeChildItemsOnOrder setting
        ' 10/05/13 | TJS             | 2013.1.14 | Corrected setting of XMLCustBillSourceID if Magento increment_id 
        '                                        | occurs after customer_id
        ' 23/05/13 | TJS             | 2013.1.16 | Modified to cater for Do Not Import payment option
        ' 16/06/13 | TJS             | 2013.1.20 | Modified to include any customer comments from Status History as CustomerComment
        '                                        | Corrected above to check for XMLCustBillSourceID = Nothing as well as "0"
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221 and corrected SKU Alias processing
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to prevent error when SplitSKU item not found
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to pick up shipping email address and copy billing email and phone if no shipping values
        ' 04/01/14 | TJS             | 2013.4.03 | Modified to cater for single quote characters in the MAgento Instance ID and discount_invoiced changing sign from version 1.12.xx
        ' 14/01/14 | TJS             | 2013.4.05 | Modified to cater for no shipping address details on gift card orders etc
        ' 11/02/14 | TJS             | 2013.4.09 | Removed code line from discount_invoiced processing which doubled discout value
        ' 20/02/14 | TJS             | 2014.0.01 | Modified to cater for shipping address being found before the shipping method
        '                                        | and to use discount_amount instead of discount_invoiced when detecting coupons
        ' 01/05/14 | TJS             | 2014.0.02 | Modified to take MAgento discount as amendment to item price if coupons not enabled
        '                                        | Changed name of variable used for item level discounts to separate from order level discounts
        '                                        | Modified to truncate overlength customer comments and to handle ImportAllOrdersAsSingleCustomer and OverrideMagentoPricesWith settings
        ' 22/05/14 | TJS             | 2014.0.01 | Modified for Unit of Measure on SKU Alias lookup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLGenericOrder As XDocument, XMLRequest As XDocument, XMLMember As XDocument, XMLAuthCardMember As XDocument ' TJS 10/06/12
        Dim XMLOrderResponseNode As XElement, XMLMagentoOrderItem As XElement, XMLImportResponseNode As XElement
        Dim XMLMagentoMember As XElement, XMLMagentoStructureMember As XElement, XMLOrderSourceNode As XElement
        Dim XMLOrderNode As XElement, XMLSourceOrderRef As XElement, XMLTPOrderImportNode As XElement
        Dim XMLOrderDate As XElement, XMLOrderCurrency As XElement, XMLBillDetail As XElement
        Dim XMLBillingAddress As XElement, XMLCustBillFirstName As XElement, XMLCustomerBill As XElement
        Dim XMLCustBillLastName As XElement, XMLCustBillSourceID As XElement, XMLCustBillEmail As XElement
        Dim XMLCustBillCountry As XElement, XMLCustBillPostalCode As XElement, XMLCustBillCity As XElement
        Dim XMLCustBillCompany As XElement, XMLShipDetail As XElement, XMLCustomerShip As XElement
        Dim XMLCustShipFirstName As XElement, XMLCustShipLastName As XElement, XMLShippingAddress As XElement
        Dim XMLCustShipPhone As XElement, XMLCustShipCity As XElement, XMLCustShipCountry As XElement
        Dim XMLCustShipCompany As XElement, XMLSourceMerchantID As XElement, XMLCustShipPostalCode As XElement
        Dim XMLOrderItem() As XElement, XMLItemTPID() As XElement, XMLItemDesc() As XElement, XMLItemQty() As XElement
        Dim XMLItemUnitPrice() As XElement, XMLItemSubTotal() As XElement, XMLItemTaxValue() As XElement, XMLSourceItemPurchaseID() As XElement
        Dim XMLItemUnitOfMeasure() As XElement ' TJS 22/05/14
        Dim XMLPaymentDetails As XElement, XMLPaymentMethod As XElement, XMLCCDetails As XElement, XMLNameOnCard As XElement
        Dim XMLCardType As XElement, XMLCardNumber As XElement, XMLOrderTotals As XElement, XMLShippingMethod As XElement
        Dim XMLMagentoStatusHistory As XElement, XMLCustShipEmail As XElement ' TJS 16/06/13 TJS 13/11/13
        Dim XMLPaymentType As XElement, XMLSourcePaymentID As XElement, XMLDiscountCoupon As XElement ' TJS 10/06/12
        Dim XMLMagentoMembersList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLMagentoStructureList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLMagentoOrderItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLMagentoAuthCardMembersList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 10/06/12
        Dim XMLMagentoStatusHistList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 16/06/13
        Dim strCustBillAddress As String, strCustBillPhone As String = "", strCustBillRegion As String
        Dim strCustBillCountryCode As String, strCustBillCountryName As String, strCustShipAddress As String ' TJS 18/03/11
        Dim strCustShipRegion As String, strCustShipCountryCode As String, strCustShipCountryName As String ' TJS 18/03/11
        Dim strMagentoOrderID As String, strMagentoPaymentType As String, strCardStartMonth As String
        Dim strCardStartYear As String, strCardExpiryMonth As String, strCardExpiryYear As String, strDescription As String ' TJS 19/09/13
        Dim strShippingMethodGroup As String, strSQL As String, sTemp As String, strMagentoStoreID As String ' TJS 02/12/11
        Dim strItemSplitSKU As String, strDiscountDescription As String, strNotes As String ' TJS 20/04/12 TJS 10/06/12 TJS 16/06/13
        Dim iItemLoop As Integer, iItemCount As Integer, decOrderDiscountValue As Decimal, decShippingDiscountValue As Decimal ' TJS 10/06/12
        Dim bReturnValue As Boolean, bOmitItem As Boolean, bDoNotImportPayment As Boolean, bShippingNameAddressFound As Boolean ' TJS 08/05/13 TJS 23/05/13 14/01/14
        Dim decItemPrice As Decimal, decItemSubTotal As Decimal, decItemDiscountValue As Decimal ' TJS 17/04/14 TJS 30/04/14
        Dim iMaxLength As Integer ' TJS 01/05/14

        Try
            bReturnValue = True
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12

            ' load order into XML document
            XMLRequest = XDocument.Parse(Trim(strInputXML))
            ' check module and connector are activated and valid
            If eShopCONNECTFacade.ValidateSource("Magento eShopCONNECTOR", MAGENTO_SOURCE_CODE, "", False) Then
                ' yes, create Generic XML document
                XMLGenericOrder = New XDocument
                XMLTPOrderImportNode = New XElement("eShopCONNECT")

                decItemTotal = 0
                decItemTaxTotal = 0
                decItemShippingTotal = 0
                decOrderDiscountValue = 0 ' TJS 10/06/12
                decShippingDiscountValue = 0 ' TJS 10/06/12
                strDiscountDescription = "" ' TJS 10/06/12
                AlreadyImported = False
                strMagentoOrderID = "" ' TJS 04/05/11
                strMagentoStoreID = "" ' TJS 02/12/11
                strDiscountDescription = "" ' TJS 10/06/12
                XMLSourceOrderRef = Nothing ' TJS 04/05/11
                XMLSourceMerchantID = Nothing ' TJS 04/05/11
                XMLOrderDate = Nothing ' TJS 04/05/11
                XMLOrderCurrency = Nothing ' TJS 04/05/11
                XMLCustBillSourceID = Nothing ' TJS 04/05/11
                XMLCustBillEmail = Nothing ' TJS 04/05/11
                XMLBillDetail = Nothing ' TJS 04/05/11
                XMLShipDetail = Nothing ' TJS 04/05/11
                XMLPaymentDetails = Nothing ' TJS 04/05/11
                XMLPaymentType = Nothing ' TJS 10/06/12
                XMLShippingMethod = Nothing ' TJS 04/05/11 TJS 20/02/14
                ReDim XMLOrderItem(0) ' TJS 04/05/11
                bDoNotImportPayment = False ' TJS 23/05/13
                strNotes = "" ' TJS 16/06/13

                'SOURCE DETAILS
                XMLOrderSourceNode = New XElement("Source")
                XMLOrderSourceNode.Add(New XElement("SourceName", "Magento Order"))
                XMLOrderSourceNode.Add(New XElement("SourceCode", MAGENTO_SOURCE_CODE))
                XMLTPOrderImportNode.Add(XMLOrderSourceNode)

                XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                XMLMagentoMembersList = XMLRequest.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                ' process each member as required
                For Each XMLMagentoMember In XMLMagentoMembersList
                    XMLMember = XDocument.Parse(XMLMagentoMember.ToString)
                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/key").ToLower
                        Case "increment_id"
                            XMLSourceOrderRef = New XElement("SourceOrderRef")
                            strMagentoOrderID = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                            strSQL = "SELECT COUNT(*) FROM CustomerSalesOrder WHERE SourceCode = '" & MAGENTO_SOURCE_CODE
                            strSQL = strSQL & "' AND StoreMerchantID_DEV000221 = '" & MagentoConfig.InstanceID.Replace("'", "''") ' TJS 18/03/11 TJS 04/01/14
                            strSQL = strSQL & "' AND MerchantOrderID_DEV000221 = '" & strMagentoOrderID & "'"
                            If SourceConfig.IgnoreVoidedOrdersAndInvoices Then
                                strSQL = strSQL & " AND IsVoided = 0"
                            End If
                            sTemp = eShopCONNECTFacade.GetField(strSQL, System.Data.CommandType.Text, Nothing)
                            If sTemp = "1" Then
                                AlreadyImported = True
                                Exit For
                            End If
                            XMLSourceOrderRef.Value = strMagentoOrderID
                            If MagentoConfig.CreateCustomerForGuestCheckout Then 'FA 21/06/13
                                If IsNothing(XMLCustBillSourceID) Then
                                    XMLCustBillSourceID.Value = "O" & strMagentoOrderID ' FA 21/06/13
                                ElseIf XMLCustBillSourceID.Value = "O" Then ' TJS 10/05/13 'FA 21/06/13
                                    XMLCustBillSourceID.Value = "O" & strMagentoOrderID ' TJS 10/05/13
                                Else
                                    'do nothing
                                End If
                            End If

                        Case "store_id"
                            XMLSourceMerchantID = New XElement("SourceMerchantID")
                            XMLSourceMerchantID.Value = MagentoConfig.InstanceID ' TJS 18/03/11
                            strMagentoStoreID = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") ' TJS 02/12/11

                        Case "order_currency_code"
                            XMLOrderCurrency = New XElement("OrderCurrency")
                            XMLOrderCurrency.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                        Case "created_at"
                            XMLOrderDate = New XElement("OrderDate")
                            ' get Magento order date (Date + time) and strip out time
                            XMLOrderDate.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value").Substring(0, 10)
                            OrderTimestamp = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"))

                        Case "customer_id"
                            If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then ' TJS 01/05/14
                                XMLCustBillSourceID = New XElement("ISCustomerCode") ' TJS 01/05/14
                                XMLCustBillSourceID.Value = MagentoConfig.ImportAllOrdersAsSingleCustomer ' TJS 01/05/14
                            Else
                                If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then ' TJS 18/03/11
                                    XMLCustBillSourceID = New XElement("SourceCustomerID")
                                    XMLCustBillSourceID.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                ElseIf MagentoConfig.CreateCustomerForGuestCheckout Then ' TSJ 30/04/13
                                    XMLCustBillSourceID = New XElement("SourceCustomerID") ' TSJ 30/04/13
                                    XMLCustBillSourceID.Value = "O" & strMagentoOrderID ' TSJ 30/04/13
                                Else
                                    XMLCustBillSourceID = New XElement("ISCustomerCode") ' TJS 18/03/11
                                    XMLCustBillSourceID.Value = "DefaultECommerceShopper" ' TJS 18/03/11
                                End If
                            End If

                        Case "customer_email"
                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                XMLCustBillEmail = New XElement("Email")
                                XMLCustBillEmail.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                            End If

                        Case "shipping_amount"
                            decItemShippingTotal = CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"))

                        Case "shipping_method"
                            XMLShippingMethod = New XElement("ShippingMethod")
                            If SourceConfig.EnableDeliveryMethodTranslation And eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                XMLShippingMethod.Value = eShopCONNECTFacade.TranslateDeliveryMethodToIS(MAGENTO_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"), "", strShippingMethodGroup)

                            ElseIf eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") = "" Then
                                XMLShippingMethod.Value = SourceConfig.DefaultShippingMethod

                            Else
                                XMLShippingMethod.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                            End If

                        Case "billing_address"
                            strCustBillAddress = "" ' TJS 04/05/11
                            strCustBillRegion = "" ' TJS 04/05/11
                            strCustBillCountryName = "" ' TJS 04/05/11
                            strCustBillPhone = "" ' TJS 04/05/11

                            XMLCustBillFirstName = Nothing ' TJS 04/05/11
                            XMLCustBillLastName = Nothing ' TJS 04/05/11
                            XMLCustBillCompany = Nothing ' TJS 04/05/11
                            XMLCustBillCity = Nothing ' TJS 04/05/11
                            XMLCustBillPostalCode = Nothing ' TJS 04/05/11
                            XMLCustBillCountry = Nothing ' TJS 04/05/11

                            ' create billing elements 
                            XMLBillDetail = New XElement("BillingDetails")
                            XMLCustomerBill = New XElement("Customer")
                            XMLBillingAddress = New XElement("BillingAddress")
                            ' process each member of address as required
                            XMLMagentoStructureList = XMLMagentoMember.XPathSelectElements("value/item")
                            For Each XMLMagentoStructureMember In XMLMagentoStructureList
                                XMLMember = XDocument.Parse(XMLMagentoStructureMember.ToString)
                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/key").ToLower
                                    Case "firstname"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            XMLCustBillFirstName = New XElement("FirstName")
                                            sTemp = eShopCONNECTFacade.GetField("DefaultContact", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                            XMLCustBillFirstName.Value = eShopCONNECTFacade.GetField("ContactFirstName", "CRMContact", "ContactCode = '" & sTemp & "' and EntityCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                                XMLCustBillFirstName = New XElement("FirstName")
                                                XMLCustBillFirstName.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            End If
                                        End If

                                    Case "lastname"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            XMLCustBillFirstName = New XElement("LastName")
                                            sTemp = eShopCONNECTFacade.GetField("DefaultContact", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                            XMLCustBillFirstName.Value = eShopCONNECTFacade.GetField("ContactLastName", "CRMContact", "ContactCode = '" & sTemp & "' and EntityCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            XMLCustBillLastName = New XElement("LastName")
                                            XMLCustBillLastName.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "company"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            XMLCustBillFirstName = New XElement("Company")
                                            XMLCustBillFirstName.Value = eShopCONNECTFacade.GetField("CustomerName", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                                XMLCustBillCompany = New XElement("Company")
                                                XMLCustBillCompany.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            End If
                                        End If

                                    Case "telephone"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            sTemp = eShopCONNECTFacade.GetField("DefaultContact", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                            strCustBillPhone = eShopCONNECTFacade.GetField("BusinessPhone", "CRMContact", "ContactCode = '" & sTemp & "' and EntityCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            strCustBillPhone = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "street"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            strCustBillAddress = eShopCONNECTFacade.GetField("Address", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            strCustBillAddress = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "city"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            XMLCustBillCity = New XElement("Town_City")
                                            XMLCustBillCity.Value = eShopCONNECTFacade.GetField("City", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            XMLCustBillCity = New XElement("Town_City")
                                            XMLCustBillCity.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "region"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            strCustBillRegion = eShopCONNECTFacade.GetField("State", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            strCustBillRegion = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "country_id"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            XMLCustBillCountry = New XElement("Country")
                                            XMLCustBillCountry.Value = eShopCONNECTFacade.GetField("Country", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            XMLCustBillCountry = New XElement("Country")
                                            strCustBillCountryCode = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            strCustBillCountryName = ConvertCountryCode(strCustBillCountryCode) ' TJS 18/03/11
                                            XMLCustBillCountry.Value = strCustBillCountryName ' TJS 18/03/11
                                        End If

                                    Case "postcode"
                                        ' start of code added TJS 01/05/14
                                        If MagentoConfig.ImportAllOrdersAsSingleCustomer <> "" Then
                                            XMLCustBillPostalCode = New XElement("PostalCode")
                                            XMLCustBillPostalCode.Value = eShopCONNECTFacade.GetField("PostalCode", "Customer", "CustomerCode = '" & MagentoConfig.ImportAllOrdersAsSingleCustomer & "'")
                                        Else
                                            ' end of code added TJS 01/05/14
                                            XMLCustBillPostalCode = New XElement("PostalCode")
                                            XMLCustBillPostalCode.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                End Select
                            Next

                            XMLCustomerBill.Add(XMLCustBillSourceID)
                            If XMLCustBillFirstName IsNot Nothing Then
                                XMLCustomerBill.Add(XMLCustBillFirstName)
                            End If
                            XMLCustomerBill.Add(XMLCustBillLastName)
                            If XMLCustBillCompany IsNot Nothing Then
                                XMLCustomerBill.Add(XMLCustBillCompany)
                            End If
                            If XMLCustBillCompany IsNot Nothing AndAlso XMLCustBillCompany.Value <> "" And strCustBillPhone <> "" Then ' TJS 04/05/11
                                XMLCustomerBill.Add(New XElement("WorkPhone", strCustBillPhone))
                            ElseIf strCustBillPhone <> "" Then
                                XMLCustomerBill.Add(New XElement("HomePhone", strCustBillPhone))
                            End If
                            If XMLCustBillEmail IsNot Nothing Then ' TJS 04/05/11
                                XMLCustomerBill.Add(XMLCustBillEmail)
                            End If
                            XMLBillDetail.Add(XMLCustomerBill)

                            XMLBillingAddress.Add(New XElement("Address", strCustBillAddress))
                            If XMLCustBillCity IsNot Nothing Then
                                XMLBillingAddress.Add(XMLCustBillCity)
                            End If
                            XMLBillingAddress.Add(New XElement("State", ConvertState(strCustBillCountryName, strCustBillRegion))) 'TJS 18/03/11
                            XMLBillingAddress.Add(New XElement("County", strCustBillRegion))
                            If XMLCustBillCountry IsNot Nothing Then ' TJS 04/05/11
                                XMLBillingAddress.Add(XMLCustBillCountry)
                            End If
                            If XMLCustBillPostalCode IsNot Nothing Then ' TJS 04/05/11
                                XMLBillingAddress.Add(XMLCustBillPostalCode)
                            End If
                            XMLBillDetail.Add(XMLBillingAddress)

                        Case "shipping_address"
                            strCustShipAddress = "" ' TJS 04/05/11
                            strCustShipRegion = "" ' TJS 04/05/11
                            strCustShipCountryName = "" ' TJS 04/05/11

                            XMLCustShipFirstName = Nothing ' TJS 04/05/11
                            XMLCustShipLastName = Nothing ' TJS 04/05/11
                            XMLCustShipCompany = Nothing ' TJS 04/05/11
                            XMLCustShipPhone = Nothing ' TJS 04/05/11
                            XMLCustShipEmail = Nothing ' TJS 13/11/13
                            XMLCustShipCity = Nothing ' TJS 04/05/11
                            XMLCustShipPostalCode = Nothing ' TJS 04/05/11
                            XMLCustShipCountry = Nothing ' TJS 04/05/11

                            ' create shipping elements 
                            XMLShipDetail = New XElement("ShippingDetails")
                            XMLCustomerShip = New XElement("Customer")
                            XMLShippingAddress = New XElement("ShippingAddress")
                            bShippingNameAddressFound = False ' TJS 14/01/14
                            ' process each member of address as required
                            XMLMagentoStructureList = XMLMagentoMember.XPathSelectElements("value/item")
                            For Each XMLMagentoStructureMember In XMLMagentoStructureList
                                XMLMember = XDocument.Parse(XMLMagentoStructureMember.ToString)
                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/key").ToLower
                                    Case "firstname"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLCustShipFirstName = New XElement("FirstName")
                                            XMLCustShipFirstName.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            bShippingNameAddressFound = True ' TJS 14/01/14
                                        End If

                                    Case "lastname"
                                        XMLCustShipLastName = New XElement("LastName")
                                        XMLCustShipLastName.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        bShippingNameAddressFound = True ' TJS 14/01/14

                                    Case "company"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLCustShipCompany = New XElement("Company")
                                            XMLCustShipCompany.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            bShippingNameAddressFound = True ' TJS 14/01/14
                                        End If

                                    Case "telephone"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLCustShipPhone = New XElement("Telephone")
                                            XMLCustShipPhone.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            bShippingNameAddressFound = True ' TJS 14/01/14
                                        End If

                                    Case "street"
                                        strCustShipAddress = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        bShippingNameAddressFound = True ' TJS 14/01/14

                                    Case "city"
                                        XMLCustShipCity = New XElement("Town_City")
                                        XMLCustShipCity.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        bShippingNameAddressFound = True ' TJS 14/01/14

                                    Case "region"
                                        strCustShipRegion = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        bShippingNameAddressFound = True ' TJS 14/01/14

                                    Case "country_id"
                                        XMLCustShipCountry = New XElement("Country")
                                        strCustShipCountryCode = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        strCustShipCountryName = ConvertCountryCode(strCustShipCountryCode) ' TJS 18/03/11
                                        XMLCustShipCountry.Value = strCustShipCountryName ' TJS 18/03/11
                                        bShippingNameAddressFound = True ' TJS 14/01/14

                                    Case "postcode"
                                        XMLCustShipPostalCode = New XElement("PostalCode")
                                        XMLCustShipPostalCode.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        bShippingNameAddressFound = True ' TJS 14/01/14

                                        ' start of code added TJS 13/11/13
                                    Case "email"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLCustShipEmail = New XElement("Email")
                                            XMLCustShipEmail.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            bShippingNameAddressFound = True ' TJS 14/01/14
                                        End If
                                        ' end of code added TJS 13/11/13

                                End Select
                            Next

                            If bShippingNameAddressFound Then ' TJS 14/01/14
                                If XMLCustShipFirstName IsNot Nothing Then
                                    XMLCustomerShip.Add(XMLCustShipFirstName)
                                End If
                                XMLCustomerShip.Add(XMLCustShipLastName)
                                If XMLCustShipCompany IsNot Nothing Then
                                    XMLCustomerShip.Add(XMLCustShipCompany)
                                End If
                                If XMLCustShipPhone IsNot Nothing Then
                                    XMLCustomerShip.Add(XMLCustShipPhone)
                                    ' start of code added TJS 13/11/13
                                ElseIf strCustBillPhone <> "" Then
                                    XMLCustShipPhone = New XElement("Telephone")
                                    XMLCustShipPhone.Value = strCustBillPhone
                                    XMLCustomerShip.Add(XMLCustShipPhone)
                                End If
                                If XMLCustShipEmail IsNot Nothing Then
                                    XMLCustomerShip.Add(XMLCustShipEmail)
                                ElseIf XMLCustBillEmail IsNot Nothing Then
                                    XMLCustShipEmail = New XElement("Email")
                                    XMLCustShipEmail.Value = XMLCustBillEmail.Value
                                    XMLCustomerShip.Add(XMLCustShipEmail)
                                End If
                                ' end of code added TJS 13/11/13
                                XMLShipDetail.Add(XMLCustomerShip)

                                XMLShippingAddress.Add(New XElement("Address", strCustShipAddress))
                                If XMLCustShipCity IsNot Nothing Then
                                    XMLShippingAddress.Add(XMLCustShipCity)
                                End If
                                XMLShippingAddress.Add(New XElement("State", ConvertState(strCustShipCountryName, strCustShipRegion))) 'TJS 18/03/11
                                XMLShippingAddress.Add(New XElement("County", strCustShipRegion))

                                If XMLCustShipCountry IsNot Nothing Then ' TJS 04/05/11
                                    XMLShippingAddress.Add(XMLCustShipCountry)
                                End If
                                If XMLCustShipPostalCode IsNot Nothing Then ' TJS 04/05/11
                                    XMLShippingAddress.Add(XMLCustShipPostalCode)
                                End If
                                XMLShipDetail.Add(XMLShippingAddress)
                            End If

                            If XMLShippingMethod Is Nothing Then
                                XMLShipDetail.Add(New XElement("ShippingMethod", SourceConfig.DefaultShippingMethod))
                            Else
                                XMLShipDetail.Add(XMLShippingMethod)
                            End If

                        Case "items"
                            ' get list of items on order
                            XMLMagentoOrderItemList = XMLMagentoMember.XPathSelectElements("value/item")
                            ' create array of Order Item Nodes for Generic Order
                            iItemCount = eShopCONNECTFacade.GetXMLElementListCount(XMLMagentoOrderItemList)
                            ReDim XMLOrderItem(iItemCount - 1)
                            ReDim XMLItemTPID(iItemCount - 1)
                            ReDim XMLSourceItemPurchaseID(iItemCount - 1)
                            ReDim XMLItemDesc(iItemCount - 1)
                            ReDim XMLItemQty(iItemCount - 1)
                            ReDim XMLItemUnitPrice(iItemCount - 1)
                            ReDim XMLItemSubTotal(iItemCount - 1)
                            ReDim XMLItemTaxValue(iItemCount - 1)
                            ReDim XMLItemUnitOfMeasure(iItemCount - 1) ' TJS 22/05/14
                            iItemCount = 0
                            For Each XMLMagentoOrderItem In XMLMagentoOrderItemList
                                XMLOrderItem(iItemCount) = New XElement("Item")
                                ' for each item, process each member as required 
                                XMLMagentoStructureList = XMLMagentoOrderItem.XPathSelectElements("item")
                                strItemSplitSKU = "" ' TJS 20/04/12
                                bOmitItem = False ' TJS 08/05/13
                                decItemDiscountValue = 0 ' TJs 17/04/14 TJS 30/04/14
                                For Each XMLMagentoStructureMember In XMLMagentoStructureList
                                    XMLMember = XDocument.Parse(XMLMagentoStructureMember.ToString)
                                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/key").ToLower
                                        Case "sku"
                                            ' get Item SKU
                                            sTemp = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            GetSplitSKUPrefixItem(MagentoConfig, sTemp, strItemSplitSKU) ' TJS 08/05/13
                                            ' has Custom SKU Processing been enabled ?
                                            If MagentoConfig.CustomSKUProcessing <> "" Then
                                                ' yes, convert SKU
                                                sTemp = ConvertSKU(MagentoConfig.CustomSKUProcessing, sTemp)
                                            End If
                                            If MagentoConfig.EnableSKUAliasLookup Then
                                                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                                                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", MAGENTO_SOURCE_CODE, "@SourceSKU", sTemp}}, _
                                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                                                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then ' TJS 03/07/13
                                                    sTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 ' TJS 03/07/13
                                                    XMLItemTPID(iItemCount) = New XElement("ISItemCode")
                                                    XMLItemTPID(iItemCount).Value = sTemp
                                                    If Not eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).IsUnitMeasureCode_DEV000221Null Then ' TJS 22/05/14
                                                        XMLItemUnitOfMeasure(iItemCount) = New XElement("ItemUnitOfMeasure", eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).UnitMeasureCode_DEV000221) ' TJS 22/05/14
                                                    End If

                                                Else
                                                    XMLItemTPID(iItemCount) = New XElement("IS" & MagentoConfig.ISItemIDField)
                                                    XMLItemTPID(iItemCount).Value = sTemp
                                                End If

                                            Else
                                                XMLItemTPID(iItemCount) = New XElement("IS" & MagentoConfig.ISItemIDField)
                                                XMLItemTPID(iItemCount).Value = sTemp

                                            End If

                                        Case "item_id"
                                            XMLSourceItemPurchaseID(iItemCount) = New XElement("SourceItemPurchaseID")
                                            XMLSourceItemPurchaseID(iItemCount).Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                        Case "name"
                                            XMLItemDesc(iItemCount) = New XElement("ItemDescription")
                                            XMLItemDesc(iItemCount).Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                        Case "qty_ordered"
                                            XMLItemQty(iItemCount) = New XElement("ItemQuantity")
                                            XMLItemQty(iItemCount).Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                        Case "price"
                                            ' start of code added TJS 01/05/14
                                            If MagentoConfig.OverrideMagentoPricesWith = "Retail" Then
                                                If XMLItemTPID(iItemCount).Name = "ISItemName" Then
                                                    sTemp = eShopCONNECTFacade.GetField("ItemCode", "InventoryItem", "ItemName = '" & XMLItemTPID(iItemCount).Value.Replace("'", "''") & "'")
                                                Else
                                                    sTemp = XMLItemTPID(iItemCount).Value
                                                End If
                                                decItemPrice = CDec(eShopCONNECTFacade.GetField("RetailPriceRate", "InventoryItemPricingDetail", "ItemCode = '" & sTemp & "' and CurrencyCode = '" & XMLOrderCurrency.Value & "'"))

                                            ElseIf MagentoConfig.OverrideMagentoPricesWith = "Wholesale" Then ' TJS 01/05/14
                                                If XMLItemTPID(iItemCount).Name = "ISItemName" Then
                                                    sTemp = eShopCONNECTFacade.GetField("ItemCode", "InventoryItem", "ItemName = '" & XMLItemTPID(iItemCount).Value.Replace("'", "''") & "'")
                                                Else
                                                    sTemp = XMLItemTPID(iItemCount).Value
                                                End If
                                                decItemPrice = CDec(eShopCONNECTFacade.GetField("WholesalePriceRate", "InventoryItemPricingDetail", "ItemCode = '" & sTemp & "' and CurrencyCode = '" & XMLOrderCurrency.Value & "'"))

                                            Else
                                                ' end of code added TJS 01/05/14
                                                decItemPrice = CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) ' TJS 17/04/14
                                            End If

                                            ' start of code added TJS 17/04/14
                                        Case "discount_amount"
                                            If Not SourceConfig.EnableCoupons AndAlso "" & eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" AndAlso _
                                                CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) <> 0 And MagentoConfig.OverrideMagentoPricesWith = "" Then ' TJS 01/05/14
                                                decItemDiscountValue = CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) ' TJS 30/04/14
                                            End If
                                            ' end of code added TJS 17/04/14

                                        Case "row_total"
                                            decItemSubTotal = CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) ' TJS 17/04/14

                                        Case "tax_amount"
                                            If MagentoConfig.OverrideMagentoPricesWith = "" Then ' TJS 01/05/14
                                                XMLItemTaxValue(iItemCount) = New XElement("ItemTaxValue")
                                                XMLItemTaxValue(iItemCount).Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                                decItemTaxTotal = decItemTaxTotal + CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"))
                                            End If

                                        Case "parent_item_id" ' TJS 08/05/13
                                            If Not MagentoConfig.IncludeChildItemsOnOrder And eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then ' TJS 08/05/13
                                                bOmitItem = True ' TJS 08/05/13
                                            End If
                                    End Select
                                Next
                                If Not bOmitItem Then ' TJS 08/05/13
                                    XMLOrderItem(iItemCount).Add(XMLSourceItemPurchaseID(iItemCount))
                                    XMLOrderItem(iItemCount).Add(XMLItemTPID(iItemCount))
                                    If XMLItemUnitOfMeasure(iItemCount) IsNot Nothing Then ' TJS 22/05/14
                                        XMLOrderItem(iItemCount).Add(XMLItemUnitOfMeasure(iItemCount)) ' TJS 22/05/14
                                    End If
                                    XMLOrderItem(iItemCount).Add(XMLItemDesc(iItemCount))
                                    XMLOrderItem(iItemCount).Add(XMLItemQty(iItemCount))
                                    XMLItemUnitPrice(iItemCount) = New XElement("ItemUnitPrice") ' TJS 17/04/14
                                    XMLItemUnitPrice(iItemCount).Value = (decItemPrice - (decItemDiscountValue / CDec(XMLItemQty(iItemCount).Value))).ToString("0.00") ' TJS 17/04/14 TJS 30/04/14
                                    XMLOrderItem(iItemCount).Add(XMLItemUnitPrice(iItemCount))
                                    XMLItemSubTotal(iItemCount) = New XElement("ItemSubTotal") ' TJS 17/04/14
                                    If MagentoConfig.OverrideMagentoPricesWith = "" Then ' TJS 01/05/14
                                        XMLItemSubTotal(iItemCount).Value = (decItemSubTotal - decItemDiscountValue).ToString("0.00") ' TJS 17/04/14 TJS 30/04/14
                                    Else
                                        XMLItemSubTotal(iItemCount).Value = (decItemPrice * CDec(XMLItemQty(iItemCount).Value)).ToString("0.00") ' TJS 01/05/14
                                    End If
                                    XMLOrderItem(iItemCount).Add(XMLItemSubTotal(iItemCount))
                                    XMLOrderItem(iItemCount).Add(XMLItemTaxValue(iItemCount))
                                    ' start of code added TJS 20/04/12
                                    ' was a split SKU item found ?
                                    If strItemSplitSKU <> "" Then
                                        ' yes
                                        Do ' TJS 08/05/13
                                            sTemp = strItemSplitSKU ' TJS 08/05/13
                                            ' check for nested Split SKUs
                                            GetSplitSKUPrefixItem(MagentoConfig, sTemp, strItemSplitSKU) ' TJS 08/05/13
                                            ' has Custom SKU Processing been enabled ?
                                            If MagentoConfig.CustomSKUProcessing <> "" Then ' TJS 08/05/13
                                                ' yes, convert SKU
                                                sTemp = ConvertSKU(MagentoConfig.CustomSKUProcessing, strItemSplitSKU) ' TJS 08/05/13
                                            End If
                                            ' increase item array size
                                            ReDim Preserve XMLOrderItem(XMLOrderItem.Length)
                                            ReDim Preserve XMLItemTPID(XMLOrderItem.Length)
                                            ReDim Preserve XMLSourceItemPurchaseID(XMLOrderItem.Length)
                                            ReDim Preserve XMLItemDesc(XMLOrderItem.Length)
                                            ReDim Preserve XMLItemQty(XMLOrderItem.Length)
                                            ReDim Preserve XMLItemUnitPrice(XMLOrderItem.Length)
                                            ReDim Preserve XMLItemSubTotal(XMLOrderItem.Length)
                                            ReDim Preserve XMLItemTaxValue(XMLOrderItem.Length)
                                            ReDim Preserve XMLItemUnitOfMeasure(XMLOrderItem.Length) ' TJS 22/05/14

                                            ' create split SKU item elements
                                            iItemCount += 1
                                            XMLSourceItemPurchaseID(iItemCount) = New XElement("SourceItemPurchaseID")
                                            XMLSourceItemPurchaseID(iItemCount).Value = XMLSourceItemPurchaseID(iItemCount - 1).Value
                                            ' start of code added TJS 08/05/13
                                            If MagentoConfig.EnableSKUAliasLookup Then
                                                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                                                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", MAGENTO_SOURCE_CODE, "@SourceSKU", sTemp}}, _
                                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                                                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then ' TJS 03/07/13
                                                    sTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 ' TJS 03/07/13
                                                    XMLItemTPID(iItemCount) = New XElement("ISItemCode") ' TJS 03/07/13
                                                    XMLItemTPID(iItemCount).Value = sTemp
                                                    If Not eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).IsUnitMeasureCode_DEV000221Null Then ' TJS 22/05/14
                                                        XMLItemUnitOfMeasure(iItemCount) = New XElement("ItemUnitOfMeasure", eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).UnitMeasureCode_DEV000221) ' TJS 22/05/14
                                                    End If
                                                Else
                                                    XMLItemTPID(iItemCount) = New XElement("IS" & MagentoConfig.ISItemIDField)
                                                    XMLItemTPID(iItemCount).Value = sTemp
                                                End If

                                            Else
                                                ' end of code added TJS 08/05/13
                                                XMLItemTPID(iItemCount) = New XElement("IS" & MagentoConfig.ISItemIDField)
                                                XMLItemTPID(iItemCount).Value = sTemp ' TJS 08/05/13

                                            End If

                                            XMLItemDesc(iItemCount) = New XElement("ItemDescription")
                                            If MagentoConfig.ISItemIDField = "ItemCode" Then
                                                strDescription = eShopCONNECTFacade.GetField("ItemDescription", "InventoryItemDescription", "ItemCode = '" & sTemp.Replace("'", "''") & _
                                                    "' AND LanguageCode ='" & eShopCONNECTFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE) & "'") ' TJS 08/05/13 TJS 19/09/13
                                                If Not String.IsNullOrEmpty(strDescription) Then ' TJS 19/09/13
                                                    XMLItemDesc(iItemCount).Value = strDescription ' TJS 19/09/13
                                                End If
                                            Else
                                                strDescription = eShopCONNECTFacade.GetField("ItemDescription", "InventoryItemDescription", "ItemCode = (SELECT ItemCode FROM InventoryItem WHERE ItemName = '" & _
                                                    sTemp.Replace("'", "''") & "') AND LanguageCode ='" & eShopCONNECTFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE) & "'")  ' TJS 08/05/13 TJS 19/09/13
                                                If Not String.IsNullOrEmpty(strDescription) Then ' TJS 19/09/13
                                                    XMLItemDesc(iItemCount).Value = strDescription ' TJS 19/09/13
                                                End If
                                            End If
                                            XMLItemQty(iItemCount) = New XElement("ItemQuantity")
                                            XMLItemQty(iItemCount).Value = XMLItemQty(iItemCount - 1).Value
                                            XMLItemUnitPrice(iItemCount) = New XElement("ItemUnitPrice")
                                            XMLItemUnitPrice(iItemCount).Value = "0.00"
                                            XMLItemSubTotal(iItemCount) = New XElement("ItemSubTotal")
                                            XMLItemSubTotal(iItemCount).Value = "0.00"
                                            XMLItemTaxValue(iItemCount) = New XElement("ItemTaxValue")
                                            XMLItemTaxValue(iItemCount).Value = "0.00"

                                            ' create split SKU item XML
                                            XMLOrderItem(iItemCount) = New XElement("Item")
                                            XMLOrderItem(iItemCount).Add(XMLSourceItemPurchaseID(iItemCount))
                                            XMLOrderItem(iItemCount).Add(XMLItemTPID(iItemCount))
                                            If XMLItemUnitOfMeasure(iItemCount) IsNot Nothing Then ' TJS 22/05/14
                                                XMLOrderItem(iItemCount).Add(XMLItemUnitOfMeasure(iItemCount)) ' TJS 22/05/14
                                            End If
                                            XMLOrderItem(iItemCount).Add(XMLItemDesc(iItemCount))
                                            XMLOrderItem(iItemCount).Add(XMLItemQty(iItemCount))
                                            XMLOrderItem(iItemCount).Add(XMLItemUnitPrice(iItemCount))
                                            XMLOrderItem(iItemCount).Add(XMLItemSubTotal(iItemCount))
                                            XMLOrderItem(iItemCount).Add(XMLItemTaxValue(iItemCount))
                                        Loop While strItemSplitSKU <> "" ' TJS 08/05/13
                                    End If
                                    ' end of code added TJS 20/04/12
                                    iItemCount += 1
                                End If
                            Next

                        Case "payment"
                            strMagentoPaymentType = ""
                            strCardStartMonth = ""
                            strCardStartYear = ""
                            strCardExpiryMonth = ""
                            strCardExpiryYear = ""

                            XMLPaymentMethod = Nothing ' TJS 04/05/11
                            XMLNameOnCard = Nothing ' TJS 04/05/11
                            XMLCardType = Nothing ' TJS 04/05/11
                            XMLCardNumber = Nothing ' TJS 04/05/11
                            XMLPaymentType = Nothing ' TJS 10/06/12
                            XMLSourcePaymentID = Nothing ' TJS 10/06/12

                            ' create payment
                            XMLPaymentDetails = New XElement("PaymentDetails")
                            ' process each member of address as required
                            XMLMagentoStructureList = XMLMagentoMember.XPathSelectElements("value/item")
                            For Each XMLMagentoStructureMember In XMLMagentoStructureList
                                XMLMember = XDocument.Parse(XMLMagentoStructureMember.ToString)
                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/key").ToLower
                                    Case "method"
                                        strMagentoPaymentType = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        XMLPaymentMethod = New XElement("PaymentMethod")
                                        If strMagentoPaymentType = "" Then ' TJS 10/06/12
                                            XMLPaymentMethod.Value = "Credit Card"
                                        Else
                                            XMLPaymentMethod.Value = "Source" ' TJS 10/06/12
                                            XMLPaymentType = New XElement("PaymentType") ' TJS 10/06/12
                                            If MagentoConfig.EnablePaymentTypeTranslation Then ' TJS 10/06/12
                                                XMLPaymentType.Value = eShopCONNECTFacade.TranslatePaymentTypeToIS(MAGENTO_SOURCE_CODE, strMagentoPaymentType, strMagentoPaymentType, bDoNotImportPayment) ' TJS 10/06/12 TJS 23/05/13
                                            Else
                                                XMLPaymentType.Value = strMagentoPaymentType ' TJS 10/06/12
                                            End If
                                        End If

                                    Case "cc_owner"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLNameOnCard = New XElement("LastName")
                                            XMLNameOnCard.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "cc_type"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLCardType = New XElement("CardType")
                                            XMLCardType.Value = ConvertCardType(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) ' TJS 10/06/12
                                        End If

                                    Case "cc_last4"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLCardNumber = New XElement("CardType")
                                            XMLCardNumber.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "cc_ss_start_month"
                                        strCardStartMonth = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                    Case "cc_ss_start_year"
                                        strCardStartYear = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                    Case "cc_exp_month"
                                        strCardExpiryMonth = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                    Case "cc_exp_year"
                                        strCardExpiryYear = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")

                                        ' start of code added TJS 10/06/12
                                    Case "last_trans_id"
                                        If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                            XMLSourcePaymentID = New XElement("SourcePaymentID")
                                            XMLSourcePaymentID.Value = eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                        End If

                                    Case "additional_information"
                                        If XMLMember.XPathSelectElement("item/value/item/key") IsNot Nothing AndAlso XMLMember.XPathSelectElement("item/value/item/key").Value = "authorize_cards" Then
                                            XMLMagentoAuthCardMembersList = XMLMember.XPathSelectElements("item/value/item/value/item/value/item")
                                            For Each XMLMagentoAuthCardMember As XElement In XMLMagentoAuthCardMembersList
                                                XMLAuthCardMember = XDocument.Parse(XMLMagentoAuthCardMember.ToString)
                                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/key").ToLower

                                                    Case "cc_owner"
                                                        If eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value") <> "" And XMLNameOnCard Is Nothing Then
                                                            XMLNameOnCard = New XElement("LastName")
                                                            XMLNameOnCard.Value = eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value")
                                                        End If

                                                    Case "cc_type"
                                                        If eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value") <> "" And XMLCardType Is Nothing Then
                                                            XMLCardType = New XElement("CardType")
                                                            XMLCardType.Value = ConvertCardType(eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value"))
                                                        End If

                                                    Case "cc_last4"
                                                        If eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value") <> "" And XMLCardNumber Is Nothing Then
                                                            XMLCardNumber = New XElement("CardType")
                                                            XMLCardNumber.Value = eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value")
                                                        End If

                                                    Case "cc_ss_start_month"
                                                        If strCardStartMonth = "" Then
                                                            strCardStartMonth = eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value")
                                                        End If

                                                    Case "cc_ss_start_year"
                                                        If strCardStartYear = "" Then
                                                            strCardStartYear = eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value")
                                                        End If

                                                    Case "cc_exp_month"
                                                        If strCardExpiryMonth = "" Then
                                                            strCardExpiryMonth = eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value")
                                                        End If

                                                    Case "cc_exp_year"
                                                        If strCardExpiryYear = "" Then
                                                            strCardExpiryYear = eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value")
                                                        End If

                                                    Case "last_trans_id"
                                                        If eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value") <> "" And XMLSourcePaymentID Is Nothing Then
                                                            XMLSourcePaymentID = New XElement("SourcePaymentID")
                                                            XMLSourcePaymentID.Value = eShopCONNECTFacade.GetXMLElementText(XMLAuthCardMember, "item/value")
                                                        End If
                                                End Select
                                            Next
                                        End If
                                        ' end of code added TJS 10/06/12

                                End Select

                            Next
                            Select Case strMagentoPaymentType
                                ' start of code commented out as credit card details are not transferred to IS currently - TJS 10/06/12
                                'Case ""
                                '    If XMLPaymentMethod IsNot Nothing Then ' TJS 04/05/11
                                '        XMLPaymentDetails.Add(XMLPaymentMethod)
                                '    End If
                                '    XMLCCDetails = New XElement("CreditCardDetails")
                                '    If XMLNameOnCard IsNot Nothing Then ' TJS 04/05/11
                                '        XMLCCDetails.Add(XMLNameOnCard)
                                '    End If
                                '    If XMLCardType IsNot Nothing Then ' TJS 04/05/11
                                '        XMLCCDetails.Add(XMLCardType)
                                '    End If
                                '    If XMLCardNumber IsNot Nothing Then ' TJS 04/05/11
                                '        XMLCCDetails.Add(XMLCardNumber)
                                '    End If
                                '    If strCardStartMonth <> "" And strCardStartYear <> "" Then
                                '        XMLCCDetails.Add(New XElement("CardStartDate", Right("00" & strCardStartMonth, 2) & "/" & Right("00" & strCardStartYear, 2)))
                                '    End If
                                '    If strCardExpiryMonth <> "" And strCardExpiryYear <> "" Then
                                '        XMLCCDetails.Add(New XElement("CardExpiryDate", Right("00" & strCardExpiryMonth, 2) & "/" & Right("00" & strCardExpiryYear, 2)))
                                '    End If
                                '    XMLPaymentDetails.Add(XMLCCDetails)
                                ' end of code commented out as credit card details are not transferred to IS currently - TJS 10/06/12

                                ' start of code added TJS 10/06/12
                                Case Else
                                    If XMLPaymentMethod IsNot Nothing Then
                                        XMLPaymentDetails.Add(XMLPaymentMethod)
                                    End If
                                    If XMLPaymentType IsNot Nothing Then
                                        XMLPaymentDetails.Add(XMLPaymentType)
                                    End If
                                    If XMLSourcePaymentID IsNot Nothing Then
                                        XMLPaymentDetails.Add(XMLSourcePaymentID)
                                    End If
                                    ' end of code added TJS 10/06/12

                            End Select

                            ' start of code added TJS 10/06/12
                        Case "discount_amount" ' TJS 28/03/14
                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" AndAlso _
                                CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) <> 0 And SourceConfig.EnableCoupons Then ' TJS 17/04/14
                                ' removed check on Magento Version and Negative/positive as discount_amount always negative
                                decOrderDiscountValue = decOrderDiscountValue - CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) ' TJS 04/01/14
                                End If

                            ' start of code added TJS 28/03/14
                        Case "discount_canceled"
                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" AndAlso _
                              CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) <> 0 And SourceConfig.EnableCoupons Then ' TJS 17/04/14
                                decOrderDiscountValue = decOrderDiscountValue - CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"))
                            End If
                            ' end of code added TJS 28/03/14

                        Case "reward_currency_amount_invoiced"
                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" AndAlso _
                                CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) <> 0 Then
                                decOrderDiscountValue = decOrderDiscountValue + CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"))
                                If Not String.IsNullOrEmpty(strDiscountDescription) Then
                                    strDiscountDescription = strDiscountDescription & " plus "
                                End If
                                strDiscountDescription = strDiscountDescription & "Reward Points redeemed"
                            End If

                        Case "gift_cards_invoiced"
                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" AndAlso _
                                CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) <> 0 Then
                                decOrderDiscountValue = decOrderDiscountValue + CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"))
                                If Not String.IsNullOrEmpty(strDiscountDescription) Then
                                    strDiscountDescription = strDiscountDescription & " plus "
                                End If
                                strDiscountDescription = strDiscountDescription & "Gift Cards redeemed"
                            End If

                        Case "shipping_discount_amount"
                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" AndAlso _
                                CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")) <> 0 Then
                                decShippingDiscountValue = decShippingDiscountValue + CDec(eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value"))
                            End If

                        Case "discount_description"
                            If Not String.IsNullOrEmpty(strDiscountDescription) Then
                                strDiscountDescription = strDiscountDescription & " plus "
                            End If
                            strDiscountDescription = strDiscountDescription & eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                            ' end of code added TJS 10/06/12

                            ' start of code added TJS 16/06/13 
                        Case "status_history"
                            ' get list of status history items
                            XMLMagentoStatusHistList = XMLMagentoMember.XPathSelectElements("value/item")
                            For Each XMLMagentoStatusHistory In XMLMagentoStatusHistList
                                ' for each status history item, process each member as required 
                                XMLMagentoStructureList = XMLMagentoStatusHistory.XPathSelectElements("item")
                                For Each XMLMagentoStructureMember In XMLMagentoStructureList
                                    XMLMember = XDocument.Parse(XMLMagentoStructureMember.ToString)
                                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/key").ToLower
                                        Case "comment"
                                            If eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value") <> "" Then
                                                If strNotes <> "" Then
                                                    strNotes = strNotes & vbCrLf
                                                End If
                                                strNotes = strNotes & eShopCONNECTFacade.GetXMLElementText(XMLMember, "item/value")
                                            End If
                                    End Select
                                Next
                            Next
                            ' end of code added TJS 16/06/13 
                    End Select
                Next
                If Not AlreadyImported Then
                    XMLOrderNode = New XElement("Order")
                    XMLOrderNode.Add(XMLSourceOrderRef)
                    ' get website code for Magento instance and store ID
                    sTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & MAGENTO_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & MagentoConfig.InstanceID.Replace("'", "''") & ":" & strMagentoStoreID & "'") ' TJS 25/04/11 TJS 02/12/11
                    ' did we find the website ?
                    If "" & sTemp = "" Then ' TJS 02/12/11
                        ' no, get website code for Magento instance without store ID
                        sTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & MAGENTO_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & MagentoConfig.InstanceID.Replace("'", "''") & "'") ' TJS 02/12/11
                    End If
                    If "" & sTemp <> "" Then ' TJS 25/04/11
                        XMLOrderNode.Add(New XElement("SourceWebSiteRef", sTemp)) ' TJS 25/04/11
                    Else
                        XMLOrderNode.Add(New XElement("SourceWebSiteRef", "Magento"))
                    End If
                    XMLOrderNode.Add(XMLSourceMerchantID)
                    XMLOrderNode.Add(XMLOrderDate)
                    XMLOrderNode.Add(XMLOrderCurrency)
                    If MagentoConfig.PricesAreTaxInclusive Then ' TJS 26/10/11
                        XMLOrderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 26/10/11
                        XMLOrderNode.Add(New XElement("TaxCodeForSourceTax", MagentoConfig.TaxCodeForSourceTax)) ' TJS 26/10/11
                    End If
                    XMLOrderNode.Add(New XElement("Status", "Credit Cleared"))
                    XMLOrderNode.Add(XMLBillDetail)
                    XMLOrderNode.Add(XMLShipDetail)
                    For iItemLoop = 0 To iItemCount - 1
                        XMLOrderNode.Add(XMLOrderItem(iItemLoop))
                    Next

                    If XMLPaymentDetails IsNot Nothing AndAlso XMLPaymentType IsNot Nothing AndAlso XMLPaymentType.Value <> "checkmo" AndAlso _
                        Not bDoNotImportPayment AndAlso MagentoConfig.ImportAllOrdersAsSingleCustomer = "" Then ' TJS 10/06/12 TJS 23/05/13 TJS 01/05/14
                        XMLOrderNode.Add(XMLPaymentDetails)
                    End If

                    ' start of code added TJS 10/06/12
                    If decOrderDiscountValue <> 0 And decShippingDiscountValue <> 0 And strDiscountDescription <> "" Then
                        XMLDiscountCoupon = New XElement("DiscountCoupon")
                        XMLDiscountCoupon.Add(New XElement("DiscountDescription", strDiscountDescription & " plus Shipping Discount"))
                        XMLDiscountCoupon.Add(New XElement("DiscountValue", Format(decOrderDiscountValue + decShippingDiscountValue, "0.00")))
                        XMLOrderNode.Add(XMLDiscountCoupon)

                    ElseIf decOrderDiscountValue <> 0 And strDiscountDescription <> "" Then
                        XMLDiscountCoupon = New XElement("DiscountCoupon")
                        XMLDiscountCoupon.Add(New XElement("DiscountDescription", strDiscountDescription))
                        XMLDiscountCoupon.Add(New XElement("DiscountValue", Format(decOrderDiscountValue, "0.00")))
                        XMLOrderNode.Add(XMLDiscountCoupon)

                    ElseIf decShippingDiscountValue <> 0 Then
                        XMLDiscountCoupon = New XElement("DiscountCoupon")
                        XMLDiscountCoupon.Add(New XElement("DiscountDescription", "Shipping Discount"))
                        XMLDiscountCoupon.Add(New XElement("DiscountValue", Format(decShippingDiscountValue, "0.00")))
                        XMLOrderNode.Add(XMLDiscountCoupon)
                    End If
                    ' end of code added TJS 10/06/12

                    XMLOrderTotals = New XElement("OrderTotals")
                    XMLOrderTotals.Add(New XElement("SubTotal", Format(decItemTotal - decOrderDiscountValue, "0.00"))) ' TJS 10/06/12
                    XMLOrderTotals.Add(New XElement("Shipping", Format(decItemShippingTotal - decShippingDiscountValue, "0.00"))) ' TJS 10/06/12
                    XMLOrderTotals.Add(New XElement("Tax", Format(decItemTaxTotal, "0.00")))
                    XMLOrderTotals.Add(New XElement("Total", Format(decItemTotal - decOrderDiscountValue + decItemShippingTotal - decShippingDiscountValue + decItemTaxTotal, "0.00"))) ' TJS 10/06/12
                    XMLOrderNode.Add(XMLOrderTotals)

                    If strNotes <> "" Then '  TJS 16/06/13
                        iMaxLength = CInt(eShopCONNECTFacade.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'CustomerSalesOrder' AND ColumnName = 'Notes'")) ' TJS 01/05/14
                        If Len(strNotes) > iMaxLength Then ' TJS 01/05/14
                            XMLOrderNode.Add(New XElement("CustomerComments", Left(strNotes, iMaxLength))) ' TJS 01/05/14
                            m_ErrorNotification.WriteToLogFileOrEvent("Warning", "Magento Order Notes truncated as too long for CustomerSalesOrder Notes field - full notes were: " & strNotes) ' TJS 01/05/14
                        Else
                            XMLOrderNode.Add(New XElement("CustomerComments", strNotes)) '  TJS 16/06/13
                        End If
                    End If
                    XMLTPOrderImportNode.Add(XMLOrderNode)

                    XMLTPOrderImportNode.Add(New XElement("OrderCount", "1"))
                    XMLGenericOrder.Add(XMLTPOrderImportNode)

                    sTemp = SourceConfig.XMLImportFileSavePath
                    If sTemp <> "" Then
                        If sTemp.Substring(sTemp.Length - 1, 1) <> "\" Then
                            sTemp = sTemp & "\"
                        End If
                        XMLRequest.Save(sTemp & "Magento_Order_" & strMagentoOrderID & ".xml")
                        XMLGenericOrder.Save(sTemp & "GenericXML_for_Magento_Order_" & strMagentoOrderID & ".xml")
                    End If

                    'Generic XML Built, now do call to create order and customer (where customer does not exist)
                    XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "")
                    XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                    If XMLOrderResponseNode IsNot Nothing Then
                        If XMLOrderResponseNode.Value = "Success" Then
                            eShopCONNECTFacade.WriteLogProgressRecord("Magento Order ID " & strMagentoOrderID & " successfully imported from Instance " & MagentoConfig.InstanceID & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value) ' TJS 02/12/11
                        Else
                            eShopCONNECTFacade.WriteLogProgressRecord("Magento Order ID " & strMagentoOrderID & " import from Instance " & MagentoConfig.InstanceID & " failed") ' TJS 02/12/11
                            bReturnValue = False ' TJS 27/03/12
                        End If
                    Else
                        eShopCONNECTFacade.WriteLogProgressRecord("Magento Order ID " & strMagentoOrderID & " import from Instance " & MagentoConfig.InstanceID & " failed") ' TJS 02/12/11
                        bReturnValue = False
                    End If
                    ' start of code added TJS 27/03/12
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
                            RowOrderToRetry.SourceCode_DEV000221 = MAGENTO_SOURCE_CODE
                            RowOrderToRetry.StoreMerchantID_DEV000221 = MagentoConfig.InstanceID
                            RowOrderToRetry.MerchantOrderID_DEV000221 = strMagentoOrderID
                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                            RowOrderToRetry.RetryCount_DEV000221 = 6
                        End If
                    End If
                Else
                    If RowOrderToRetry IsNot Nothing Then
                        RowOrderToRetry.SuccessfullyImported_DEV000221 = True
                        RowOrderToRetry.RetryCount_DEV000221 = 0
                    End If
                    ' end of code added TJS 27/03/12
                End If
                ' start of code added TJS 27/03/12
            Else
                If RowOrderToRetry IsNot Nothing Then
                    If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                        RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                    Else
                        RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                    End If
                    RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                End If
                ' end of code added TJS 27/03/12   
            End If

        Catch ex As Exception
            eShopCONNECTFacade.SendErrorEmail(SourceConfig.XMLConfig, "ProcessMagentoXML", ex, eShopCONNECTFacade.ConvertXMLFromWeb(strInputXML.ToString))
            bReturnValue = False

        Finally
            eShopCONNECTFacade.Dispose()

        End Try
        Return bReturnValue

    End Function

    Private Function ConvertCountryCode(ByVal Country As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to detect if Magento Country GG (Guernsey) doesn't match
        '                                        | any CB Country records and convert if to GB (United Kingdom)GB()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' get Country record IS
        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.SystemCountry.TableName, _
            "ReadSystemCountryImportExport_DEV000221", AT_ISO_CODE, Country}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        If eShopCONNECTDatasetGateway.SystemCountry.Count > 0 Then
            ConvertCountryCode = eShopCONNECTDatasetGateway.SystemCountry(0).CountryCode
            ' Start of code added TJS 09/08/13
        ElseIf Country = "GG" Then
            eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.SystemCountry.TableName, _
                "ReadSystemCountryImportExport_DEV000221", AT_ISO_CODE, "GB"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If eShopCONNECTDatasetGateway.SystemCountry.Count > 0 Then
                ConvertCountryCode = eShopCONNECTDatasetGateway.SystemCountry(0).CountryCode
            Else
                ConvertCountryCode = Country
            End If
            ' end of code added TJS 09/08/13
        Else
            ConvertCountryCode = Country
        End If

    End Function

    Private Function ConvertState(ByVal SourceCountry As String, ByVal SourceStateOrCounty As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for strStateCode being nothing
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStateCode As String()

        strStateCode = eShopCONNECTFacade.GetRow(New String() {"StateCode"}, "SystemPostalCode", "State = '" & SourceStateOrCounty & "' AND CountryCode = '" & SourceCountry & "'", False)
        If strStateCode IsNot Nothing AndAlso strStateCode(0) <> "" Then ' TJS 02/12/11
            SourceStateOrCounty = ""
            Return strStateCode(0)
        Else
            Return ""
        End If

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

        Select Case ProcessingMode
            Case "Lerryn Test"
                Return "NONSTOCK"

            Case Else
                Return SourceSKU

        End Select

    End Function

    Private Sub GetSplitSKUPrefixItem(ByVal MagentoConfig As MagentoSettings, ByRef sTemp As String, ByRef strItemSplitSKU As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/03/13 | TJS             | 2013.1.00 | Function 
        ' 10/04/13 | TJS             | 2013.1.09 | Modified to remove trailing hyphen from product name if necessary
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iSplitPosn As Integer

        If MagentoConfig.SplitSKUSeparatorCharacters <> "" Then
            iSplitPosn = InStr(sTemp, MagentoConfig.SplitSKUSeparatorCharacters)
            If iSplitPosn > 0 Then
                strItemSplitSKU = Mid(sTemp, iSplitPosn + Len(MagentoConfig.SplitSKUSeparatorCharacters))
                sTemp = Left(sTemp, iSplitPosn - 1)
                If Right(sTemp, 1) = "-" Then ' TJS 10/04/13
                    If eShopCONNECTFacade.GetField("ItemCode", "InventoryItem", "ItemName = '" & sTemp.Replace("'", "''") & "'") Is Nothing Then ' TJS 10/04/13
                        If eShopCONNECTFacade.GetField("ItemCode", "InventoryItem", "ItemName = '" & sTemp.Substring(0, sTemp.Length - 1).Replace("'", "''") & "'") IsNot Nothing Then ' TJS 10/04/13
                            sTemp = sTemp.Substring(0, sTemp.Length - 1) ' TJS 10/04/13
                        End If
                    End If
                End If
            Else
                strItemSplitSKU = ""
            End If
        Else
            strItemSplitSKU = ""
        End If

    End Sub

    Private Function ConvertCardType(ByVal MagentoCardType As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 3/05/12 | TJS             | 2012.1.05 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case MagentoCardType
            Case "VI"
                Return "Visa"

            Case "MC"
                Return "MasterCard"

            Case "DI"
                Return "Discover"

            Case "AE"
                Return "AmEx"

            Case "JCB"
                Return "JCB"

            Case "SO"
                Return "Solo"

            Case "SM"
                Return "Maestro"

            Case Else
                Return MagentoCardType

        End Select

    End Function
End Module
