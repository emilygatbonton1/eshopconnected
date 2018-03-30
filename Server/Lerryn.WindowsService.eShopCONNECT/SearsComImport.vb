' eShopCONNECT for Connected Business - Windows Service
' Module: SearsComImport.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'
'       © 2012 - 2013  Lerryn Business Solutions Ltd
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
' Last Updated - 02 August 2013

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports System.Xml.Linq
Imports System.Xml.XPath

Module SearsComImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decSubTotal As Decimal
    Private decItemTaxTotal As Decimal
    Private decItemShippingTotal As Decimal

    Public Function ProcessSearsComXML(ByVal SourceConfig As SourceSettings, ByVal SearsDotComConfig As SearsComSettings, _
        ByVal XMLOrders As XDocument, ByVal XMLNSManSearsCom As System.Xml.XmlNamespaceManager) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes XML orders received from Sears.com
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ' 26/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 30/07/13 | TJS             | 2013.1.32 | Corrected PricesIncludeTax XML creation
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLGenericOrder As XDocument, XMLTemp As XDocument, XMLItemTemp As XDocument
        Dim XMLOrderNode As XElement, XMLOrderImportNode As XElement, XMLOrderSourceNode As XElement
        Dim xmlOrderHeaderNode As XElement, XMLImportResponseNode As XElement, XMLOrderResponseNode As XElement
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim sTemp As String, strErrorMessage As String
        Dim bReturnValue As Boolean

        Try
            bReturnValue = True
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            ' check module and connector are activated and valid
            If eShopCONNECTFacade.ValidateSource("Sears.com eShopCONNECTOR", SEARS_COM_SOURCE_CODE, "", False) Then
                ' yes, get order list
                XMLOrderList = XMLOrders.XPathSelectElements(SEARSDOTCOM_ORDER_LIST, XMLNSManSearsCom)
                For Each XMLOrderNode In XMLOrderList
                    XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                    ' has order already been imported
                    sTemp = eShopCONNECTFacade.GetField("SalesOrderCode", "CustomerSalesOrder", "StoreMerchantID_DEV000221 = '" & SearsDotComConfig.SiteID.Replace("'", "''") & _
                        "' AND MerchantOrderID_DEV000221 = '" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-number-with-date") & "'")
                    If "" & sTemp = "" Then
                        XMLGenericOrder = New XDocument
                        XMLOrderImportNode = New XElement("eShopCONNECT")

                        XMLOrderSourceNode = New XElement("Source")
                        XMLOrderSourceNode.Add(New XElement("SourceName", "Sears.com Order"))
                        XMLOrderSourceNode.Add(New XElement("SourceCode", SEARS_COM_SOURCE_CODE))
                        XMLOrderImportNode.Add(XMLOrderSourceNode)

                        ' ORDER HEADER
                        xmlOrderHeaderNode = New XElement("Order")
                        xmlOrderHeaderNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "customer-order-confirmation-number")))
                        xmlOrderHeaderNode.Add(New XElement("SourceOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-number-with-date")))
                        sTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & SEARS_COM_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & SearsDotComConfig.SiteID.Replace("'", "''") & "'")
                        If "" & sTemp <> "" Then
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", sTemp))
                        Else
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", "Sears.com"))
                        End If
                        xmlOrderHeaderNode.Add(New XElement("SourceMerchantID", SearsDotComConfig.SiteID))
                        If SearsDotComConfig.PricesAreTaxInclusive Then
                            xmlOrderHeaderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 30/07/13
                            xmlOrderHeaderNode.Add(New XElement("TaxCodeForSourceTax", SearsDotComConfig.TaxCodeForSourceTax))
                        End If

                        xmlOrderHeaderNode.Add(New XElement("OrderDate", eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-date")))

                        xmlOrderHeaderNode.Add(New XElement("OrderCurrency", SearsDotComConfig.Currency))

                        ' BILLING DETAILS
                        xmlOrderHeaderNode.Add(BillingDetails(XMLTemp))

                        ' SHIPPING DETAILS
                        xmlOrderHeaderNode.Add(ShippingDetails(XMLTemp, SearsDotComConfig))

                        ' PAYMENT DETAILS
                        xmlOrderHeaderNode.Add(PaymentDetails(XMLTemp, SearsDotComConfig))

                        ' ITEM DETAILS
                        decSubTotal = 0
                        decItemShippingTotal = 0
                        strErrorMessage = ""
                        XMLItemList = XMLTemp.XPathSelectElements(SEARSDOTCOM_ORDER_ITEM_LIST)
                        For Each XMLItemNode In XMLItemList
                            XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                            xmlOrderHeaderNode.Add(ItemDetails(XMLItemTemp, SearsDotComConfig, strErrorMessage))
                            If strErrorMessage <> "" Then
                                Exit For
                            End If
                        Next
                        ' did we get any errors ?
                        If strErrorMessage = "" Then
                            ' no 
                            xmlOrderHeaderNode.Add(OrderTotals(XMLTemp))

                            XMLOrderImportNode.Add(xmlOrderHeaderNode)

                            XMLOrderImportNode.Add(New XElement("OrderCount", "1"))

                            XMLGenericOrder.Add(XMLOrderImportNode)

                            sTemp = eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH)
                            If sTemp <> "" Then
                                If sTemp.Substring(sTemp.Length - 1, 1) <> "\" Then
                                    sTemp = sTemp & "\"
                                End If
                                XMLTemp.Save(sTemp & "SearsDotCom_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-number-with-date") & ".xml")
                                XMLGenericOrder.Save(sTemp & "GenericXML_for_SearsDotCom_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-number-with-date") & ".xml")
                            End If

                            'Generic XML Built, now do call to create order and customer (where customer does not exist)
                            XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "")
                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                            If XMLOrderResponseNode IsNot Nothing Then
                                If XMLOrderResponseNode.Value = "Success" Then
                                    eShopCONNECTFacade.WriteLogProgressRecord("Sears.com Date + PO  " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-number-with-date") & " successfully imported from Site ID " & SearsDotComConfig.SiteID & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value)
                                Else
                                    eShopCONNECTFacade.WriteLogProgressRecord("Sears.com Date + PO  " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-number-with-date") & " import from Site ID " & SearsDotComConfig.SiteID & " failed")
                                End If
                            Else
                                eShopCONNECTFacade.WriteLogProgressRecord("Sears.com Date + PO  " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, SEARSDOTCOM_ORDER_HEADER & "po-number-with-date") & " import from Site ID " & SearsDotComConfig.SiteID & " failed")
                                bReturnValue = False
                            End If

                        Else
                            eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Sears.com Order Import Error", strErrorMessage, XMLTemp.ToString)
                            bReturnValue = False
                        End If
                        XMLGenericOrder = Nothing

                    Else
                        ' Order already imported - ignore as Sears.com has no way to filter this
                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
            End If

        Catch Ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(SourceConfig.XMLConfig, "ProcessSearsComXML", Ex, XMLOrders.ToString)
            bReturnValue = False

        Finally
            eShopCONNECTFacade.Dispose()

        End Try
        Return bReturnValue

    End Function

    Private Function BillingDetails(ByVal XMLSearsDotComOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBill As XElement, XMLBillingAddress As XElement, XMLResult As XElement

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBill = New XElement("Customer")
        XMLCustomerBill.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_ORDER_HEADER & "customer-email")))
        XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_ORDER_HEADER & "customer-name")))
        XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "phone")))

        ' BILLING ADDRESS
        XMLBillingAddress = New XElement("BillingAddress")
        XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "address")))
        XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "city")))
        XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "state")))
        XMLBillingAddress.Add(New XElement("Country", "United States of America"))
        XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "zipcode")))

        XMLResult.Add(XMLCustomerBill)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult

    End Function

    Private Function ShippingDetails(ByVal XMLSearsDotComOrder As XDocument, ByVal SearsDotComConfig As SearsComSettings) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strShippingMethodGroup As String


        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "ship-to-name")))
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "phone")))

        ' SHIPPING ADDRESS
        XMLShippingAddress = New XElement("ShippingAddress")
        XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "address")))
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "city")))
        XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "state")))
        XMLShippingAddress.Add(New XElement("Country", "United States of America"))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "zipcode")))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper = "YES" Then
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS(SEARS_COM_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "shipping-method"), "", strShippingMethodGroup)))
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "shipping-method") = "" Then
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD)))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_SHIPPING_DETAIL & "shipping-method")))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))
        End If

        Return XMLResult
    End Function

    Private Function PaymentDetails(ByVal XMLSearsDotComOrder As XDocument, ByVal SearsDotComConfig As SearsComSettings) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement

        XMLResult = New XElement("PaymentDetails")
        XMLResult.Add(New XElement("PaymentMethod", "Source"))
        XMLResult.Add(New XElement("PaymentType", SearsDotComConfig.PaymentType))

        Return XMLResult

    End Function

    Private Function ItemDetails(ByVal XMLVolusionOrderItem As XDocument, ByVal SearsDotComConfig As SearsComSettings, ByRef ErrorMessage As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, strTemp As String, decItemTotal As Decimal

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        XMLResult.Add(New XElement("SourceItemPurchaseID", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "line-number")))
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "item-id")
        ' is there an SKU for the item ?
        If strTemp <> "" Then
            ' yes, has Custom SKU Processing been enabled ?
            If SearsDotComConfig.CustomSKUProcessing <> "" Then
                ' yes, convert SKU
                strTemp = ConvertSKU(SearsDotComConfig.CustomSKUProcessing, strTemp)
            End If
            XMLResult.Add(New XElement("IS" & SearsDotComConfig.ISItemIDField, strTemp))
        Else
            ErrorMessage = "No Item Source Code found for Sears.com Order Line Number " & eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "line-number")
            Return XMLResult
        End If
        XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "item-name")))
        XMLResult.Add(New XElement("ItemQuantity", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "order-quantity")))
        XMLResult.Add(New XElement("ItemUnitPrice", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "selling-price-each")))

        decItemTotal = CDec(eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "order-quantity")) * CDec(eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "selling-price-each"))
        XMLResult.Add(New XElement("ItemSubTotal", Format(decItemTotal, "0.00")))

        XMLResult.Add(New XElement("SourceItemCommission", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "commission")))

        decSubTotal = decSubTotal + decItemTotal

        decItemShippingTotal = decItemShippingTotal + CDec(eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, SEARSDOTCOM_ORDER_ITEM_HEADER & "shipping-and-handling"))
        ItemDetails = XMLResult

    End Function

    Private Function OrderTotals(ByVal XMLSearsDotComOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, decSalesTax As Decimal, decShipping As Decimal, decTotal As Decimal

        XMLResult = New XElement("OrderTotals")

        XMLResult.Add(New XElement("SubTotal", Format(decSubTotal, "0.00")))

        decSalesTax = CDec(eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_ORDER_HEADER & "sales-tax"))
        XMLResult.Add(New XElement("Tax", Format(decSalesTax, "0.00")))

        decShipping = CDec(eShopCONNECTFacade.GetXMLElementText(XMLSearsDotComOrder, SEARSDOTCOM_ORDER_HEADER & "total-shipping-handling"))
        XMLResult.Add(New XElement("Shipping", Format(decShipping, "0.00")))

        decTotal = decSubTotal + decSalesTax + decShipping
        XMLResult.Add(New XElement("Total", Format(decTotal, "0.00")))

        OrderTotals = XMLResult

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
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case ProcessingMode
            Case "Lerryn Test"
                Return "NONSTOCK"

            Case Else
                Return SourceSKU

        End Select

    End Function

End Module
