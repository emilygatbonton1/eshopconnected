' eShopCONNECT for Connected Business - Windows Service
' Module: ASPStoreFrontImport.vb
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

Imports System.IO
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports Lerryn.Facade.ImportExport

Module ASPStoreFrontImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decSubTotal As Decimal
    Private strOrderNotes As String

    Public Function ProcessASPStoreFrontXML(ByVal SourceConfig As SourceSettings, ByVal ASPStoreFrontConfig As ASPStoreFrontSettings, _
        ByVal XMLOrders As XDocument) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes files received from ASPStoreFront
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to check for and use relevant website record
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and for IS 6
        ' 14/02/12 | TJS             | 2011.2.05 | Corrected logging of ORder REf
        ' 26/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLGenericOrder As XDocument, XMLTemp As XDocument, XMLItemTemp As XDocument
        Dim XMLOrderNode As XElement, XMLOrderSourceNode As XElement, XMLISOrderImportNode As XElement, xmlOrderHeaderNode As XElement
        Dim XMLItemNode As XElement, XMLImportResponseNode As XElement, XMLOrderResponseNode As XElement
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim sDate As String(), sTemp As String, strErrorMessage As String, bReturnValue As Boolean
        Dim dblDiscountTotal As Decimal, strDiscountDescription As String

        Try
            bReturnValue = True
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12

            ' check module and connector are activated and valid
            If eShopCONNECTFacade.ValidateSource("ASPDotNetStoreFront eShopCONNECTOR", ASP_STORE_FRONT_SOURCE_CODE, "", False) Then
                XMLOrderList = XMLOrders.XPathSelectElements(ASPDOTNETSTOREFRONT_ORDER_LIST)
                For Each XMLOrderNode In XMLOrderList
                    XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                    If eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippedOn") = "" Then
                        ' yes, create Generic XML document
                        ' create Generic XML document
                        XMLGenericOrder = New XDocument
                        XMLISOrderImportNode = New XElement("eShopCONNECT")
                        XMLOrderSourceNode = New XElement("Source")
                        XMLOrderSourceNode.Add(New XElement("SourceName", "ASPDotNetStoreFront Order"))
                        XMLOrderSourceNode.Add(New XElement("SourceCode", ASP_STORE_FRONT_SOURCE_CODE))
                        XMLISOrderImportNode.Add(XMLOrderSourceNode)

                        ' ORDER HEADER
                        xmlOrderHeaderNode = New XElement("Order")
                        xmlOrderHeaderNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "PONumber")))
                        xmlOrderHeaderNode.Add(New XElement("SourceOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderNumber")))
                        sTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & ASP_STORE_FRONT_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & ASPStoreFrontConfig.SiteID.Replace("'", "''") & "'") ' TJS 25/04/11 TJS 02/12/11
                        If "" & sTemp <> "" Then ' TJS 25/04/11
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", sTemp)) ' TJS 25/04/11
                        Else
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", "ASPDotNetStoreFront"))
                        End If
                        xmlOrderHeaderNode.Add(New XElement("SourceMerchantID", ASPStoreFrontConfig.SiteID))

                        ' get ASPDotNetStorefront order date (US Format) and covert
                        sDate = eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderDate").Split(CChar("/"))
                        sTemp = sDate(2).Substring(0, 4) & "-" & Right("00" & sDate(0), 2) & "-" & Right("00" & sDate(1), 2)
                        xmlOrderHeaderNode.Add(New XElement("OrderDate", sTemp))

                        xmlOrderHeaderNode.Add(New XElement("OrderCurrency", ASPStoreFrontConfig.Currency))

                        strOrderNotes = eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "Notes")
                        If eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "CustomerServiceNotes") <> "" Then
                            strOrderNotes += vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLTemp, VOLUSION_ORDER_HEADER & "CustomerServiceNotes")
                        End If

                        ' BILLING DETAILS
                        xmlOrderHeaderNode.Add(BillingDetails(XMLTemp))

                        ' SHIPPING DETAILS
                        xmlOrderHeaderNode.Add(ShippingDetails(ASPStoreFrontConfig, XMLTemp))

                        ' PAYMENT DETAILS
                        xmlOrderHeaderNode.Add(PaymentDetails(XMLTemp))

                        ' ITEM DETAILS
                        decSubTotal = 0
                        dblDiscountTotal = 0
                        strErrorMessage = ""
                        XMLItemList = XMLTemp.XPathSelectElements(ASPDOTNETSTOREFRONT_ORDER_ITEM_LIST)
                        For Each XMLItemNode In XMLItemList
                            XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                            ' does item have a negative value ?
                            If CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "OrderedProductPrice")) < 0 Then
                                dblDiscountTotal += 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "OrderedProductPrice"))
                                strDiscountDescription = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "OrderedProductName")
                            Else
                                xmlOrderHeaderNode.Add(ItemDetails(XMLItemTemp, ASPStoreFrontConfig, strErrorMessage))
                                If strErrorMessage <> "" Then
                                    Exit For
                                End If
                            End If
                        Next
                        ' did we get any errors ?
                        If strErrorMessage = "" Then
                            ' no did we have a negative value item ?
                            If dblDiscountTotal > 0 Then
                                xmlOrderHeaderNode.Add(DiscountCoupon(XMLTemp, strDiscountDescription, dblDiscountTotal))
                            End If
                            xmlOrderHeaderNode.Add(OrderTotals(XMLTemp))

                            xmlOrderHeaderNode.Add(New XElement("CustomerComments", strOrderNotes))

                            XMLISOrderImportNode.Add(xmlOrderHeaderNode)

                            XMLISOrderImportNode.Add(New XElement("OrderCount", "1"))

                            XMLGenericOrder.Add(XMLISOrderImportNode)

                            If SourceConfig.XMLImportFileSavePath <> "" Then
                                If sTemp.Substring(sTemp.Length - 1, 1) <> "\" Then
                                    sTemp = sTemp & "\"
                                End If
                                XMLTemp.Save(sTemp & "ASPDotNetStorefront_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderNumber") & ".xml")
                                XMLGenericOrder.Save(sTemp & "GenericXML_for_ASPDotNetStorefront_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderNumber") & ".xml")
                            End If

                            'Generic XML Built, now do call to create order and customer (where customer does not exist)
                            XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "")
                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                            If XMLOrderResponseNode IsNot Nothing Then
                                If XMLOrderResponseNode.Value = "Success" Then
                                    eShopCONNECTFacade.WriteLogProgressRecord("ASPDotNetStorefront Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderNumber") & " successfully imported from Site ID " & ASPStoreFrontConfig.SiteID & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value) ' TJS 14/02/12
                                Else
                                    eShopCONNECTFacade.WriteLogProgressRecord("ASPDotNetStorefront Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderNumber") & " import from Site ID " & ASPStoreFrontConfig.SiteID & " failed") ' TJS 14/02/12
                                End If
                            Else
                                eShopCONNECTFacade.WriteLogProgressRecord("ASPDotNetStorefront Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderNumber") & " import from Site ID " & ASPStoreFrontConfig.SiteID & " failed") ' TJS 14/02/12
                                bReturnValue = False
                            End If

                        Else
                            eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "ASPDotNetStorefront Order Import Error", strErrorMessage, XMLTemp.ToString)
                            bReturnValue = False
                        End If
                        XMLGenericOrder = Nothing

                    Else
                        eShopCONNECTFacade.WriteLogProgressRecord("ASPDotNetStorefront Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderNumber") & " import from Site ID " & ASPStoreFrontConfig.SiteID & " skipped as order already shipped") ' TJS 14/02/12
                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
            End If

        Catch ex As Exception
            eShopCONNECTFacade.SendErrorEmail(SourceConfig.XMLConfig, "ProcessASPStoreFrontXML", ex, XMLTemp.ToString)
            bReturnValue = False

        Finally
            eShopCONNECTFacade.Dispose()

        End Try
        Return bReturnValue

    End Function

    Private Function BillingDetails(ByVal XMLStoreFrontOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBill As XElement, XMLBillingAddress As XElement
        Dim XMLResult As XElement, strCountry As String

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBill = New XElement("Customer")
        XMLCustomerBill.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "CustomerGUID")))
        XMLCustomerBill.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingFirstName")))
        XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingLastName")))
        XMLCustomerBill.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingCompany")))
        If eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingCompany") <> "" Then
            XMLCustomerBill.Add(New XElement("WorkPhone", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingPhone")))
        Else
            XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingPhone")))
        End If
        XMLCustomerBill.Add(New XElement("Email", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "Email")))

        ' BILLING ADDRESS
        strCountry = eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingCountry")
        ' need to ensure Country matches those used in IS
        If strCountry = "United States" Then
            strCountry = "United States of America"
        ElseIf strCountry = "Republic of Ireland" Then
            strCountry = "Ireland"
        ElseIf strCountry = "England" Then
            strCountry = "United Kingdom"
        ElseIf strCountry = "Taiwan" Then
            strCountry = "Taiwan, Republic of China"
        ElseIf strCountry = "Bosnia" Then
            strCountry = "Bosnia & Herzegovina"
        ElseIf strCountry = "Russia" Then
            strCountry = "Russian Federation"
        ElseIf strCountry = "" Then
            strCountry = eShopCONNECTFacade.GetField("Country", "SystemCompanyInformation")
        End If
        XMLBillingAddress = New XElement("BillingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingSuite") <> "" Then
            If eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress2") <> "" Then
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingSuite") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress1") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress2")))
            Else
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingSuite") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress1")))
            End If
        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress2") <> "" Then
            XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress1") & _
                vbCr & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress2")))
        Else
            XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingAddress1")))
        End If
        XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingCity")))
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingState").Length <= 2 Then
            XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingState")))
        Else
            XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingState")))
        End If
        XMLBillingAddress.Add(New XElement("Country", strCountry))
        XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingZip")))

        XMLResult.Add(XMLCustomerBill)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult

    End Function

    Private Function ShippingDetails(ByVal ASPStoreFrontConfig As ASPStoreFrontSettings, ByVal XMLStoreFrontOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strShippingMethodGroup As String


        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        If ASPStoreFrontConfig.AllowShippingLastNameBlank Then
            If eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingFirstName") = "" And _
                eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingLastName") = "" Then
                XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingFirstName")))
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "BillingLastName")))

            ElseIf eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingFirstName") <> "" And _
                eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingLastName") = "" Then
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingFirstName")))
            Else
                XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingFirstName")))
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingLastName")))
            End If
        Else
            XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingFirstName")))
            XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingLastName")))
        End If
        XMLCustomerShip.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingCompany")))
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingPhone")))

        ' SHIPPING ADDRESS
        strCountry = eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingCountry")
        ' need to ensure Country matches those used in IS
        If strCountry = "United States" Then
            strCountry = "United States of America"
        ElseIf strCountry = "Republic of Ireland" Then
            strCountry = "Ireland"
        ElseIf strCountry = "England" Then
            strCountry = "United Kingdom"
        ElseIf strCountry = "Taiwan" Then
            strCountry = "Taiwan, Republic of China"
        ElseIf strCountry = "Bosnia" Then
            strCountry = "Bosnia & Herzegovina"
        ElseIf strCountry = "Russia" Then
            strCountry = "Russian Federation"
        ElseIf strCountry = "" Then
            strCountry = eShopCONNECTFacade.GetField("Country", "SystemCompanyInformation")
        End If
        XMLShippingAddress = New XElement("ShippingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingSuite") <> "" Then
            If eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress2") <> "" Then
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingSuite") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress1") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress2")))
            Else
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingSuite") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress1")))
            End If
        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress2") <> "" Then
            XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress1") & _
                vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress2")))
        Else
            XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingAddress1")))
        End If
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingCity")))
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingState").Length <= 2 Then
            XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingState")))
        Else
            XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingState")))
        End If
        XMLShippingAddress.Add(New XElement("Country", strCountry))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingZip")))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper = "YES" Then
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS(ASP_STORE_FRONT_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingMethodID"), "", strShippingMethodGroup)))
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingMethodID") = "" Then
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD)))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "ShippingMethodID")))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))
        End If

        Return XMLResult

    End Function

    Private Function PaymentDetails(ByVal XMLStoreFrontOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCreditCard As XElement, XMLResult As XElement

        XMLResult = New XElement("PaymentDetails")
        XMLResult.Add(New XElement("PaymentMethod", "Credit Card"))
        XMLCreditCard = New XElement("CreditCardDetails")
        XMLCreditCard.Add(New XElement("NameOnCard", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "CardType")))
        XMLCreditCard.Add(New XElement("CardType", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "CardName")))
        XMLCreditCard.Add(New XElement("CardNumber", "************" & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "Last4")))
        XMLCreditCard.Add(New XElement("CardExpiryDate", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "CardExpirationMonth") & "/" & Right(eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "CardExpirationYear"), 2)))
        If eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "AuthorizationPNREF") <> "" Then
            XMLCreditCard.Add(New XElement("TransactionStatus", "PaymentReceived"))
        End If

        XMLResult.Add(XMLCreditCard)

        Return XMLResult

    End Function

    Private Function ItemDetails(ByVal XMLStoreFrontOrderItem As XDocument, ByVal ASPStoreFrontConfig As ASPStoreFrontSettings, ByRef ErrorMessage As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221 and corrected SKU Alias processing
        ' 22/05/14 | TJS             | 2014.0.01 | Modified for Unit of Measure on SKU Alias lookup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, strTemp As String
        Dim decItemQuantity As Decimal, decItemUnitPrice As Decimal

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        XMLResult.Add(New XElement("SourceItemPurchaseID", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "ShoppingCartRecID")))
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "OrderedProductSKU")
        ' is there an SKU for the item ?
        If strTemp <> "" Then
            ' yes, has Custom SKU Processing been enabled ?
            If ASPStoreFrontConfig.CustomSKUProcessing <> "" Then
                ' yes, convert SKU
                strTemp = ConvertSKU(ASPStoreFrontConfig.CustomSKUProcessing, strTemp)
            End If
            If ASPStoreFrontConfig.EnableSKUAliasLookup Then
                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", ASP_STORE_FRONT_SOURCE_CODE, "@SourceSKU", strTemp}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then ' TJS 03/07/13
                    strTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 ' TJS 03/07/13
                    XMLResult.Add(New XElement("ISItemCode", strTemp)) ' TJS 03/07/13
                    If Not eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).IsUnitMeasureCode_DEV000221Null Then ' TJS 22/05/14
                        XMLResult.Add(New XElement("ItemUnitOfMeasure", eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).UnitMeasureCode_DEV000221)) ' TJS 22/05/14
                    End If
                Else
                    XMLResult.Add(New XElement("IS" & ASPStoreFrontConfig.ISItemIDField, strTemp))
                End If

            Else
                XMLResult.Add(New XElement("IS" & ASPStoreFrontConfig.ISItemIDField, strTemp))

            End If
        Else
            ErrorMessage = "No Product SKU found for ASPDotNetStorefront Order Detail ID " & eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "ShoppingCartRecID")
            Return XMLResult
        End If
        XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "OrderedProductName")))
        decItemQuantity = CDec(eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "Quantity"))
        XMLResult.Add(New XElement("ItemQuantity", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "Quantity")))
        decItemUnitPrice = CDec(eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "OrderedProductPrice"))
        XMLResult.Add(New XElement("ItemUnitPrice", eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrderItem, ASPDOTNETSTOREFRONT_ORDER_ITEM_DETAILS & "OrderedProductPrice")))

        XMLResult.Add(New XElement("ItemSubTotal", Format(decItemQuantity * decItemUnitPrice, "0.00")))
        decSubTotal = decSubTotal + (decItemQuantity * decItemUnitPrice)

        ItemDetails = XMLResult

    End Function

    Private Function DiscountCoupon(ByVal XMLStoreFrontOrder As XDocument, ByVal DiscountDescription As String, ByVal DiscountTotal As Decimal) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement

        XMLResult = New XElement("DiscountCoupon")

        XMLResult.Add(New XElement("DiscountDescription", DiscountDescription))

        XMLResult.Add(New XElement("DiscountValue", Format(DiscountTotal, "0.00")))

        DiscountCoupon = XMLResult

    End Function

    Private Function OrderTotals(ByVal XMLStoreFrontOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, decSalesTax As Decimal, decShipping As Decimal
        Dim decTotal As Decimal, strTemp As String

        XMLResult = New XElement("OrderTotals")

        XMLResult.Add(New XElement("SubTotal", Format(decSubTotal, "0.00")))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderTax")
        decSalesTax = decSalesTax + CDec(strTemp)
        XMLResult.Add(New XElement("Tax", Format(decSalesTax, "0.00")))

        decShipping = CDec(eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderShippingCosts"))
        XMLResult.Add(New XElement("Shipping", Format(decShipping, "0.00")))

        decTotal = CDec(eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, ASPDOTNETSTOREFRONT_ORDER_HEADER & "OrderTotal"))
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
