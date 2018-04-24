' eShopCONNECT for Connected Business - Windows Service
' Module: VolusionImport.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 22 May 2014

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Module VolusionImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decSubTotal As Decimal
    Private strOrderNotes As String

    Public Function ProcessVolusionXML(ByVal SourceConfig As SourceSettings, ByVal VolusionConfig As VolusionSettings, ByVal XMLOrders As XDocument, _
        ByRef RowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row) As Boolean ' TJS 04/01/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes XML orders received from Volusion
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/06/09 | TJS             | 2009.3.00 | Function added
        ' 14/07/09 | TJS             | 2009.3.01 | Payment details added
        ' 15/08/09 | TJS             | 2009.3.03 | Removed ConvertXMLFromWeb as caused unwanted conversions and 
        '                                        | corrected date format to ensure 2 digit month and day
        ' 14/09/09 | TJS             | 2009.3.06 | Modified to exclude cancelled orders e.g. failed credit card payment
        '                                        | and to handle order discount item lines
        ' 06/10/09 | TJS             | 2009.3.07 | Modified to cater for reprocessing records
        ' 30/12/09 | TJS             | 2010.0.00 | Added log message about kipped orders
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for QuoteToConvert parameter on XMLOrderImport
        '                                        | and Source Code constants
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to check for and use relevant website record
        ' 26/10/11 | TJS             | 2011.1.xx | Corrected setting of source tax values and codes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and for IS 6
        ' 26/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 04/01/14 | TJS             | 2013.4.03 | Added code to retry orders which failed to import and ignore orders with a status of shipped
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLGenericOrder As XDocument
        Dim XMLItemTemp As XDocument, XMLTemp As XDocument
        Dim XMLOrderNode As XElement, XMLOrderSourceNode As XElement, XMLISOrderImportNode As XElement
        Dim xmlOrderHeaderNode As XElement, XMLItemNode As XElement, XMLImportResponseNode As XElement
        Dim XMLOrderResponseNode As XElement
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim sDate As String(), sTemp As String, strErrorMessage As String, bReturnValue As Boolean, bOrderImportFailed As Boolean ' TJS 04/01/14
        Dim dblDiscountTotal As Decimal, strDiscountDescription As String, strVolusionOrderID As String ' TJS 14/09/09 TJS 04/01/14

        Try
            bReturnValue = True

            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12

            ' check module and connector are activated and valid
            If eShopCONNECTFacade.ValidateSource("Volusion eShopCONNECTOR", VOLUSION_SOURCE_CODE, "", False) Then ' TJS 06/10/09 TJS 19/08/10
                XMLOrderList = XMLOrders.XPathSelectElements(VOLUSION_ORDER_LIST)
                For Each XMLOrderNode In XMLOrderList
                    XMLTemp = XDocument.Parse(XMLOrderNode.ToString) ' TJS 30/12/09
                    bOrderImportFailed = False ' TJS 04/01/14
                    strVolusionOrderID = eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "OrderID") ' TJS 04/01/14
                    If eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "OrderStatus") <> "Cancelled" And eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "OrderStatus") <> "Shipped" Then ' TJS 14/09/09 TJS 04/01/14
                        ' create Generic XML document
                        XMLGenericOrder = New XDocument
                        XMLISOrderImportNode = New XElement("eShopCONNECT")
                        XMLOrderSourceNode = New XElement("Source")
                        XMLOrderSourceNode.Add(New XElement("SourceName", "Volusion Order"))
                        XMLOrderSourceNode.Add(New XElement("SourceCode", VOLUSION_SOURCE_CODE)) ' TJS 19/08/10
                        XMLISOrderImportNode.Add(XMLOrderSourceNode)

                        ' ORDER HEADER
                        xmlOrderHeaderNode = New XElement("Order")
                        xmlOrderHeaderNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "PONum")))
                        xmlOrderHeaderNode.Add(New XElement("SourceOrderRef", strVolusionOrderID)) ' TJS 04/01/14
                        sTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & VOLUSION_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & VolusionConfig.SiteID.Replace("'", "''") & "'") ' TJS 25/04/11 TJS 02/12/11
                        If "" & sTemp <> "" Then ' TJS 25/04/11
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", sTemp)) ' TJS 25/04/11
                        Else
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", "Volusion"))
                        End If
                        xmlOrderHeaderNode.Add(New XElement("SourceMerchantID", VolusionConfig.SiteID))
                        If VolusionConfig.PricesAreTaxInclusive Then ' TJS 26/10/11
                            XMLOrderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 26/10/11
                            xmlOrderHeaderNode.Add(New XElement("TaxCodeForSourceTax", VolusionConfig.TaxCodeForSourceTax)) ' TJS 26/10/11
                        End If

                        ' get Volusion order date (US Format) and covert
                        sDate = eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "OrderDate").Split(CChar("/"))
                        sTemp = sDate(2).Substring(0, 4) & "-" & Right("00" & sDate(0), 2) & "-" & Right("00" & sDate(1), 2) ' TJS 15/08/09
                        xmlOrderHeaderNode.Add(New XElement("OrderDate", sTemp))

                        xmlOrderHeaderNode.Add(New XElement("OrderCurrency", VolusionConfig.Currency))

                        strOrderNotes = eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "Order_Comments")
                        If eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "OrderNotes") <> "" Then
                            strOrderNotes += vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "OrderNotes")
                        End If

                        ' BILLING DETAILS
                        xmlOrderHeaderNode.Add(BillingDetails(XMLTemp))

                        ' SHIPPING DETAILS
                        xmlOrderHeaderNode.Add(ShippingDetails(XMLTemp, VolusionConfig)) ' TJS 15/08/09

                        ' PAYMENT DETAILS
                        xmlOrderHeaderNode.Add(PaymentDetails(XMLTemp, VolusionConfig)) ' TJS 14/07/09 TJS 11/02/14

                        ' ITEM DETAILS
                        decSubTotal = 0
                        dblDiscountTotal = 0 ' TJS 14/09/09
                        strErrorMessage = "" ' TJS 14/09/09
                        XMLItemList = XMLTemp.XPathSelectElements(VOLUSION_ORDER_ITEM_LIST)
                        For Each XMLItemNode In XMLItemList
                            XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                            ' does item have a negative value ?
                            If CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, VOLUSION_ORDER_ITEM_DETAILS & "ProductPrice")) < 0 Then ' TJS 14/09/09
                                dblDiscountTotal += 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, VOLUSION_ORDER_ITEM_DETAILS & "ProductPrice")) ' TJS 14/09/09
                                strDiscountDescription = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, VOLUSION_ORDER_ITEM_DETAILS & "ProductName") ' TJS 14/09/09
                            Else
                                xmlOrderHeaderNode.Add(ItemDetails(XMLItemTemp, VolusionConfig, strErrorMessage))
                                If strErrorMessage <> "" Then
                                    Exit For
                                End If
                            End If
                        Next
                        ' did we get any errors ?
                        If strErrorMessage = "" Then
                            ' no did we have a negative value item ?
                            If dblDiscountTotal > 0 Then ' TJS 14/09/09
                                xmlOrderHeaderNode.Add(DiscountCoupon(XMLTemp, strDiscountDescription, dblDiscountTotal)) ' TJS 14/09/09
                            End If
                            xmlOrderHeaderNode.Add(OrderTotals(XMLTemp))

                            xmlOrderHeaderNode.Add(New XElement("CustomerComments", strOrderNotes))

                            XMLISOrderImportNode.Add(xmlOrderHeaderNode)

                            XMLISOrderImportNode.Add(New XElement("OrderCount", "1"))

                            XMLGenericOrder.Add(XMLISOrderImportNode)

                            sTemp = eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH)
                            If sTemp <> "" Then
                                If sTemp.Substring(sTemp.Length - 1, 1) <> "\" Then
                                    sTemp = sTemp & "\"
                                End If
                                XMLTemp.Save(sTemp & "Volusion_Order_" & strVolusionOrderID & ".xml") ' TJS 04/01/14
                                XMLGenericOrder.Save(sTemp & "GenericXML_for_Volusion_Order_" & strVolusionOrderID & ".xml") ' TJS 04/01/14
                            End If

                            'Generic XML Built, now do call to create order and customer (where customer does not exist)
                            XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "") ' TJS 19/08/10
                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                            If XMLOrderResponseNode IsNot Nothing Then
                                If XMLOrderResponseNode.Value = "Success" Then
                                    eShopCONNECTFacade.WriteLogProgressRecord("Volusion Order ID " & strVolusionOrderID & " successfully imported from Site ID " & VolusionConfig.SiteID & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value) ' TJS 19/08/10 TJS 04/01/14
                                Else
                                    eShopCONNECTFacade.WriteLogProgressRecord("Volusion Order ID " & strVolusionOrderID & " import from Site ID " & VolusionConfig.SiteID & " failed") ' TJS 19/08/10 TJS 04/01/14
                                    bReturnValue = False ' TJS 04/01/14
                                    bOrderImportFailed = True ' TJS 04/01/14
                                End If
                            Else
                                eShopCONNECTFacade.WriteLogProgressRecord("Volusion Order ID " & strVolusionOrderID & " import from Site ID " & VolusionConfig.SiteID & " failed") ' TJS 19/08/10 TJS 04/01/14
                                bReturnValue = False
                                bOrderImportFailed = True ' TJS 04/01/14
                            End If
                            ' start of code added TJS 04/01/14
                            If Not bOrderImportFailed Then
                                If RowOrderToRetry IsNot Nothing Then
                                    RowOrderToRetry.SuccessfullyImported_DEV000221 = True
                                    RowOrderToRetry.RetryCount_DEV000221 = 0
                                    ' don't need to save here as must be on order retry and PollVolusion in WebPollIO.vb will do it
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
                                    RowOrderToRetry.SourceCode_DEV000221 = VOLUSION_SOURCE_CODE
                                    RowOrderToRetry.StoreMerchantID_DEV000221 = VolusionConfig.SiteID
                                    RowOrderToRetry.MerchantOrderID_DEV000221 = strVolusionOrderID
                                    RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                    RowOrderToRetry.RetryCount_DEV000221 = 6
                                End If
                                RecordOrderIDToRetry(RowOrderToRetry, VolusionConfig.SiteID, "Volusion")
                            End If
                            RowOrderToRetry = Nothing
                            ' end of code added TJS 04/01/14
                        Else
                            eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Volusion Order Import Error", strErrorMessage, XMLTemp.ToString) ' TJS 15/08/09
                            bReturnValue = False
                        End If
                        XMLGenericOrder = Nothing
                    Else
                        If eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "OrderStatus") = "Cancelled" Then ' TJS 04/01/14
                            eShopCONNECTFacade.WriteLogProgressRecord("Volusion Order Ref " & strVolusionOrderID & " import skipped as order cancelled") ' TJS 04/01/14
                        Else
                            eShopCONNECTFacade.WriteLogProgressRecord("Volusion Order Ref " & strVolusionOrderID & " import skipped as order already shipped") ' TJS 30/12/09 TJS 04/01/14
                        End If
                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
            End If

        Catch Ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(SourceConfig.XMLConfig, "ProcessVolusionXML", Ex, XMLOrders.ToString) ' TJS 15/08/09
            bReturnValue = False

        Finally
            eShopCONNECTFacade.Dispose()

        End Try
        Return bReturnValue

    End Function

    Private Function BillingDetails(ByVal XMLVolusionOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/06/09 | TJS             | 2009.3.00 | Function added
        ' 15/08/09 | TJS             | 2009.3.03 | Modified to cater for non-US addresses using State as County
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBill As XElement, XMLBillingAddress As XElement
        Dim XMLResult As XElement, strCountry As String

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBill = New XElement("Customer")
        XMLCustomerBill.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CustomerID")))
        XMLCustomerBill.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingFirstName")))
        XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingLastName")))
        XMLCustomerBill.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingCompanyName")))
        If eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingCompanyName") <> "" Then
            XMLCustomerBill.Add(New XElement("WorkPhone", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingPhoneNumber")))
        Else
            XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingPhoneNumber")))
        End If

        ' BILLING ADDRESS
        strCountry = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingCountry")
        ' need to ensure Country matches those used in IS
        If strCountry = "United States" Then
            strCountry = "United States of America"
        ElseIf strCountry = "Republic of Ireland" Then ' TJS 15/08/09
            strCountry = "Ireland" ' TJS 15/08/09
        ElseIf strCountry = "England" Then ' TJS 15/08/09
            strCountry = "United Kingdom" ' TJS 15/08/09
        ElseIf strCountry = "Taiwan" Then ' TJS 15/08/09
            strCountry = "Taiwan, Republic of China" ' TJS 15/08/09
        ElseIf strCountry = "Bosnia" Then ' TJS 15/08/09
            strCountry = "Bosnia & Herzegovina" ' TJS 15/08/09
        ElseIf strCountry = "Russia" Then ' TJS 15/08/09
            strCountry = "Russian Federation" ' TJS 15/08/09
        End If
        XMLBillingAddress = New XElement("BillingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingAddress2") <> "" Then
            XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingAddress1") & _
                vbCr & eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingAddress2")))
        Else
            XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingAddress1")))
        End If
        XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingCity")))
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingState").Length <= 2 Then ' TJS 15/08/09
            XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingState")))
        Else
            XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingState"))) ' TJS 15/08/09
        End If
        XMLBillingAddress.Add(New XElement("Country", strCountry))
        XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingPostalCode")))

        XMLResult.Add(XMLCustomerBill)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult

    End Function

    Private Function ShippingDetails(ByVal XMLVolusionOrder As XDocument, ByVal VolusionConfig As VolusionSettings) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/06/09 | TJS             | 2009.3.00 | Function added
        ' 15/08/09 | TJS             | 2009.3.03 | Modified to cater for non-US addresses using State as County
        ' 15/12/09 | TJS             | 2009.3.09 | Modified to cater for SourceDeliveryClass on TranslateDeliveryMethodToIS
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to use Source Code constants
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strShippingMethodGroup As String


        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        If VolusionConfig.AllowShippingLastNameBlank Then ' TJS 15/08/09
            If eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipFirstName") = "" And _
                eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipLastName") = "" Then ' TJS 15/08/09
                XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingFirstName"))) ' TJS 15/08/09
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "BillingLastName"))) ' TJS 15/08/09

            ElseIf eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipFirstName") <> "" And _
                eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipLastName") = "" Then ' TJS 15/08/09
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipFirstName"))) ' TJS 15/08/09
            Else
                XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipFirstName"))) ' TJS 15/08/09
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipLastName"))) ' TJS 15/08/09
            End If
        Else
            XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipFirstName")))
            XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipLastName")))
        End If
        XMLCustomerShip.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipCompanyName")))
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipPhoneNumber")))

        ' SHIPPING ADDRESS
        strCountry = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipCountry")
        ' need to ensure Country matches those used in IS
        If strCountry = "United States" Then
            strCountry = "United States of America"
        ElseIf strCountry = "Republic of Ireland" Then ' TJS 15/08/09
            strCountry = "Ireland" ' TJS 15/08/09
        ElseIf strCountry = "England" Then ' TJS 15/08/09
            strCountry = "United Kingdom" ' TJS 15/08/09
        ElseIf strCountry = "Taiwan" Then ' TJS 15/08/09
            strCountry = "Taiwan, Republic of China" ' TJS 15/08/09
        ElseIf strCountry = "Bosnia" Then ' TJS 15/08/09
            strCountry = "Bosnia & Herzegovina" ' TJS 15/08/09
        ElseIf strCountry = "Russia" Then ' TJS 15/08/09
            strCountry = "Russian Federation" ' TJS 15/08/09
        End If
        XMLShippingAddress = New XElement("ShippingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipAddress2") <> "" Then
            XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipAddress1") & _
                vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipAddress2")))
        Else
            XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipAddress1")))
        End If
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipCity")))
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipState").Length <= 2 Then ' TJS 15/08/09
            XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipState")))
        Else
            XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipState"))) ' TJS 15/08/09
        End If
        XMLShippingAddress.Add(New XElement("Country", strCountry))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShipPostalCode")))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper = "YES" Then
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS(VOLUSION_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShippingMethodID"), "", strShippingMethodGroup))) ' TJS 15/12/09 TJS 12/081/0
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShippingMethodID") = "" Then
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD)))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "ShippingMethodID")))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))
        End If

        Return XMLResult
    End Function

    Private Function PaymentDetails(ByVal XMLVolusionOrder As XDocument, ByVal VolusionConfig As VolusionSettings) As XElement ' TJS 11/02/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/07/09 | TJS             | 2009.3.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCreditCard As XElement, XMLResult As XElement, XMLPaymentType As XElement ' TJS 11/02/14
        Dim strPaymentMethodID As String ' TJS 11/02/14
        Dim bDoNotImportPayment As Boolean ' TJS 11/02/14

        XMLResult = New XElement("PaymentDetails")
        If eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CC_Last4") <> "" Then ' TJS 11/02/14
            XMLResult.Add(New XElement("PaymentMethod", "Credit Card"))
            XMLCreditCard = New XElement("CreditCardDetails")
            XMLCreditCard.Add(New XElement("NameOnCard", "See Volusion system"))
            XMLCreditCard.Add(New XElement("CardNumber", "************" & eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CC_Last4")))
            XMLCreditCard.Add(New XElement("AuthorisationNumber", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CreditCardAuthorizationNumber")))
            XMLCreditCard.Add(New XElement("GatewayTransactionID", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CreditCardTransactionID")))
            XMLCreditCard.Add(New XElement("GatewayResponseAVS", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "AVS")))
            XMLCreditCard.Add(New XElement("GatewayResponseCVV", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CVV2_Response")))
            If eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CreditCardAuthorizationNumber") <> "" Then
                XMLCreditCard.Add(New XElement("TransactionStatus", "PaymentReceived"))
            End If

            XMLResult.Add(XMLCreditCard)

            ' start of code added TJS 11/02/14
        Else
            strPaymentMethodID = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "PaymentMethodID")
            XMLResult.Add(New XElement("PaymentMethod", "Source"))
            XMLPaymentType = New XElement("PaymentType")
            If VolusionConfig.EnablePaymentTypeTranslation Then
                XMLPaymentType.Value = eShopCONNECTFacade.TranslatePaymentTypeToIS(VOLUSION_SOURCE_CODE, strPaymentMethodID, strPaymentMethodID, bDoNotImportPayment)
            Else
                XMLPaymentType.Value = strPaymentMethodID
            End If
            XMLResult.Add(XMLPaymentType)
            XMLResult.Add(New XElement("SourcePaymentID", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "CreditCardTransactionID")))
            ' end of code added TJS 11/02/14
        End If

        Return XMLResult

    End Function

    Private Function ItemDetails(ByVal XMLVolusionOrderItem As XDocument, ByVal VolusionConfig As VolusionSettings, ByRef ErrorMessage As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/06/09 | TJS             | 2009.3.00 | Function added
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for CustomSKUProcessing
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 15/04/14 | TJS             | 2013.4.05 | Modified for EnableSKUAliasLookup
        ' 22/05/14 | TJS             | 2014.0.01 | Modified for Unit of Measure on SKU Alias lookup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, strTemp As String ' TJS 19/08/10

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        XMLResult.Add(New XElement("SourceItemPurchaseID", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "OrderDetailID")))
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "ProductCode") ' TJS 19/08/10
        ' is there an SKU for the item ?
        If strTemp <> "" Then
            ' yes, has Custom SKU Processing been enabled ?
            If VolusionConfig.CustomSKUProcessing <> "" Then ' TJS 19/08/10
                ' yes, convert SKU
                strTemp = ConvertSKU(VolusionConfig.CustomSKUProcessing, strTemp) ' TJS 19/08/10
            End If
            ' start of code added TJS 15/01/14
            If VolusionConfig.EnableSKUAliasLookup Then
                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", VOLUSION_SOURCE_CODE, "@SourceSKU", strTemp}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then
                    strTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221
                    XMLResult.Add(New XElement("ISItemCode", strTemp))
                    If Not eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).IsUnitMeasureCode_DEV000221Null Then ' TJS 22/05/14
                        XMLResult.Add(New XElement("ItemUnitOfMeasure", eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).UnitMeasureCode_DEV000221)) ' TJS 22/05/14
                    End If
                Else
                    XMLResult.Add(New XElement("IS" & VolusionConfig.ISItemIDField, strTemp))
                End If
            Else
                ' end of code added TJS 15/01/14
                XMLResult.Add(New XElement("IS" & VolusionConfig.ISItemIDField, strTemp)) ' TJS 19/08/10
            End If

        Else
            ErrorMessage = "No Item Source Code found for Volusion Order Detail ID " & eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "OrderDetailID")
            Return XMLResult
        End If
        XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "ProductName")))
        XMLResult.Add(New XElement("ItemQuantity", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "Quantity")))
        XMLResult.Add(New XElement("ItemUnitPrice", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "ProductPrice")))

        XMLResult.Add(New XElement("ItemSubTotal", eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "TotalPrice")))
        decSubTotal = decSubTotal + CDec(eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "TotalPrice"))

        If eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "Options") <> "" Then
            strOrderNotes += vbCrLf & "Item " & eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "ProductCode") & " - " & eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrderItem, VOLUSION_ORDER_ITEM_DETAILS & "OrderNotes")
        End If

        ItemDetails = XMLResult

    End Function

    Private Function DiscountCoupon(ByVal XMLVolusionOrder As XDocument, ByVal DiscountDescription As String, ByVal DiscountTotal As Decimal) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/09/09 | TJS             | 2009.3.06 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement

        XMLResult = New XElement("DiscountCoupon")

        XMLResult.Add(New XElement("DiscountDescription", DiscountDescription))

        XMLResult.Add(New XElement("DiscountValue", Format(DiscountTotal, "0.00")))

        DiscountCoupon = XMLResult

    End Function

    Private Function OrderTotals(ByVal XMLVolusionOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/06/09 | TJS             | 2009.3.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, decSalesTax As Decimal, decShipping As Decimal
        Dim decTotal As Decimal, strTemp As String

        XMLResult = New XElement("OrderTotals")

        XMLResult.Add(New XElement("SubTotal", Format(decSubTotal, "0.00")))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "SalesTax1")
        decSalesTax = CDec(strTemp)
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "SalesTax2")
        decSalesTax = decSalesTax + CDec(strTemp)
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "SalesTax2")
        decSalesTax = decSalesTax + CDec(strTemp)
        XMLResult.Add(New XElement("Tax", Format(decSalesTax, "0.00")))

        decShipping = CDec(eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "TotalShippingCost"))
        XMLResult.Add(New XElement("Shipping", Format(decShipping, "0.00")))

        decTotal = CDec(eShopCONNECTFacade.GetXMLElementText(XMLVolusionOrder, VOLUSION_ORDER_HEADER & "PaymentAmount"))
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
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case ProcessingMode
            Case "Lerryn Test"
                Return "NONSTOCK"

            Case Else
                Return SourceSKU

        End Select

    End Function

End Module
