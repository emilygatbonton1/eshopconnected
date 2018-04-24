' eShopCONNECT for Connected Business - Windows Service
' Module: ThreeDCartImport.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 22 May 2014

Imports System.IO
Imports System.Xml.Linq
Imports System.Xml.XPath
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports Lerryn.Facade.ImportExport

Module ThreeDCartImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decSubTotal As Decimal
    Private strOrderNotes As String

    Public Function Process3DCartXML(ByVal SourceConfig As SourceSettings, ByVal ThreeDCartConfig As ThreeDCartSettings, ByVal XMLOrder As XDocument, ByRef AlreadyImported As Boolean, _
        ByRef RowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes XML Orders received from 3DCart
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLGenericOrder As XDocument, XMLShipmentTemp As XDocument, XMLItemTemp As XDocument
        Dim XMLISOrderImportNode As XElement, XMLOrderSourceNode As XElement, xmlOrderHeaderNode As XElement
        Dim XMLShipmentNode As XElement, XMLItemNode As XElement, XMLPaymentNode As XElement
        Dim XMLImportResponseNode As XElement, XMLOrderResponseNode As XElement
        Dim XMLShipmentList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim sTemp As String, strErrorMessage As String, strShipmentID As String, sDate As String()
        Dim dblDiscountTotal As Decimal
        Dim bReturnValue As Boolean, bDoNotImport As Boolean

        Try
            bReturnValue = True
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

            ' check module and connector are activated and valid
            If eShopCONNECTFacade.ValidateSource("3DCart eShopCONNECTOR", THREE_D_CART_SOURCE_CODE, "", False) Then
                XMLShipmentList = XMLOrder.XPathSelectElements(THREE_D_CART_ORDER_SHIPMENT_LIST)
                For Each XMLShipmentNode In XMLShipmentList
                    XMLShipmentTemp = XDocument.Parse(XMLShipmentNode.ToString)
                    strShipmentID = eShopCONNECTFacade.GetXMLElementText(XMLShipmentTemp, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "ShipmentID")
                    ' has order shipment been completed ?
                    If eShopCONNECTFacade.GetXMLElementText(XMLShipmentTemp, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "ShipmentDate") = "" Then
                        ' no, create Generic XML document
                        XMLGenericOrder = New XDocument
                        XMLISOrderImportNode = New XElement("eShopCONNECT")
                        XMLOrderSourceNode = New XElement("Source")
                        XMLOrderSourceNode.Add(New XElement("SourceName", "3DCart Order"))
                        XMLOrderSourceNode.Add(New XElement("SourceCode", THREE_D_CART_SOURCE_CODE))
                        XMLISOrderImportNode.Add(XMLOrderSourceNode)

                        ' ORDER HEADER
                        xmlOrderHeaderNode = New XElement("Order")
                        xmlOrderHeaderNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "OrderID")))
                        sTemp = eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "InvoiceNumber") & ":" & strShipmentID
                        xmlOrderHeaderNode.Add(New XElement("SourceOrderRef", sTemp))
                        sTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & THREE_D_CART_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & ThreeDCartConfig.StoreID.Replace("'", "''") & "'")
                        If "" & sTemp <> "" Then
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", sTemp))
                        Else
                            xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", "3DCart"))
                        End If
                        xmlOrderHeaderNode.Add(New XElement("SourceMerchantID", ThreeDCartConfig.StoreID))

                        ' get 3DCart order date (US Format) and covert
                        sDate = eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "Date").Split(CChar("/"))
                        sTemp = sDate(2).Substring(0, 4) & "-" & Right("00" & sDate(0), 2) & "-" & Right("00" & sDate(1), 2)
                        xmlOrderHeaderNode.Add(New XElement("OrderDate", sTemp))

                        xmlOrderHeaderNode.Add(New XElement("OrderCurrency", ThreeDCartConfig.Currency))

                        strOrderNotes = eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "Comments/OrderComment")
                        If eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "Comments/OrderExternalComment") <> "" Then
                            strOrderNotes += vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "Comments/OrderExternalComment")
                        End If

                        ' BIL9LING DETAILS
                        xmlOrderHeaderNode.Add(BillingDetails(XMLOrder))

                        ' SHIPPING DETAILS
                        xmlOrderHeaderNode.Add(ShippingDetails(ThreeDCartConfig, XMLOrder, XMLShipmentTemp))

                        ' PAYMENT DETAILS
                        XMLPaymentNode = PaymentDetails(ThreeDCartConfig, XMLOrder, bDoNotImport)
                        If Not bDoNotImport Then
                            xmlOrderHeaderNode.Add(XMLPaymentNode)
                        End If

                        ' ITEM DETAILS
                        decSubTotal = 0
                        dblDiscountTotal = 0
                        strErrorMessage = ""
                        XMLItemList = XMLOrder.XPathSelectElements(THREE_D_CART_ORDER_ITEM_LIST)
                        For Each XMLItemNode In XMLItemList
                            XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                            If eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, THREE_D_CART_ORDER_ITEM_DETAILS & "ShipmentID") = strShipmentID Then
                                xmlOrderHeaderNode.Add(ItemDetails(XMLItemTemp, ThreeDCartConfig, strErrorMessage))
                                If strErrorMessage <> "" Then
                                    Exit For
                                End If
                            End If
                        Next
                        ' did we get any errors ?
                        If strErrorMessage = "" Then
                            ' no 
                            xmlOrderHeaderNode.Add(OrderTotals(XMLOrder))

                            xmlOrderHeaderNode.Add(New XElement("CustomerComments", strOrderNotes))

                            XMLISOrderImportNode.Add(xmlOrderHeaderNode)

                            XMLISOrderImportNode.Add(New XElement("OrderCount", "1"))

                            XMLGenericOrder.Add(XMLISOrderImportNode)

                            If SourceConfig.XMLImportFileSavePath <> "" Then
                                If SourceConfig.XMLImportFileSavePath.Substring(sTemp.Length - 1, 1) <> "\" Then
                                    SourceConfig.XMLImportFileSavePath = SourceConfig.XMLImportFileSavePath & "\"
                                End If
                                XMLOrder.Save(SourceConfig.XMLImportFileSavePath & "3DCart_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "InvoiceNumber") & "_Shipment_" & strShipmentID & ".xml")
                                XMLGenericOrder.Save(SourceConfig.XMLImportFileSavePath & "GenericXML_for_3DCart_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "InvoiceNumber") & "_Shipment_" & strShipmentID & ".xml")
                            End If

                            'Generic XML Built, now do call to create order and customer (where customer does not exist)
                            XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "")
                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                            If XMLOrderResponseNode IsNot Nothing Then
                                If XMLOrderResponseNode.Value = "Success" Then
                                    eShopCONNECTFacade.WriteLogProgressRecord("3DCart Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "InvoiceNumber") & ", Shipment " & strShipmentID & " successfully imported from Store ID " & ThreeDCartConfig.StoreID & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value)
                                Else
                                    eShopCONNECTFacade.WriteLogProgressRecord("3DCart Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "InvoiceNumber") & ", Shipment " & strShipmentID & " import from Store ID " & ThreeDCartConfig.StoreID & " failed")
                                End If
                            Else
                                eShopCONNECTFacade.WriteLogProgressRecord("3DCart Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "InvoiceNumber") & ", Shipment " & strShipmentID & " import from Store ID " & ThreeDCartConfig.StoreID & " failed")
                                bReturnValue = False
                            End If

                        Else
                            eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "3DCart Order Import Error", strErrorMessage, XMLOrder.ToString)
                            bReturnValue = False
                        End If
                        XMLGenericOrder = Nothing

                    Else
                        eShopCONNECTFacade.WriteLogProgressRecord("3DCart Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLOrder, THREE_D_CART_ORDER_HEADER & "InvoiceNumber") & ", Shipment " & strShipmentID & " import from Store ID " & ThreeDCartConfig.StoreID & " skipped as order already shipped")
                    End If
                    If bShutDownInProgress Then
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception
            eShopCONNECTFacade.SendErrorEmail(SourceConfig.XMLConfig, "Process3DCartXML", ex, XMLOrder.ToString)
            bReturnValue = False

        Finally
            eShopCONNECTFacade.Dispose()

        End Try
        Return bReturnValue

    End Function


    Private Function BillingDetails(ByVal XMP3DCartOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBill As XElement, XMLBillingAddress As XElement
        Dim XMLResult As XElement, strCountry As String

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBill = New XElement("Customer")
        XMLCustomerBill.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_HEADER & "CustomerID")))
        XMLCustomerBill.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "FirstName")))
        XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "LastName")))
        XMLCustomerBill.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Company")))
        If eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Company") <> "" Then
            XMLCustomerBill.Add(New XElement("WorkPhone", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Phone")))
        Else
            XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Phone")))
        End If
        XMLCustomerBill.Add(New XElement("Email", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Email")))

        ' BILLING ADDRESS
        strCountry = eShopCONNECTFacade.GetField("CountryCode", "SystemCountry", "ISOCode = '" & eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "CountryCode") & "'")
        XMLBillingAddress = New XElement("BillingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address") <> "" Then
            If eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address2") <> "" Then
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address2")))
            Else
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address")))
            End If
        ElseIf eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address2") <> "" Then
            XMLBillingAddress.Add(eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address2"))
        Else
            XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "Address")))
        End If
        XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "City")))
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "StateCode").Length <= 2 Then
            XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "StateCode")))
        Else
            XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "StateCode")))
        End If
        XMLBillingAddress.Add(New XElement("Country", strCountry))
        XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "ZipCode")))

        XMLResult.Add(XMLCustomerBill)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult

    End Function

    Private Function ShippingDetails(ByVal ThreeDCartConfig As ThreeDCartSettings, ByVal XMP3DCartOrder As XDocument, ByVal XML3DCartShipment As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strShippingMethodGroup As String

        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        If ThreeDCartConfig.AllowShippingLastNameBlank Then
            If eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "FirstName") = "" And _
                eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "LastName") = "" Then
                XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "FirstName")))
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_BILLING_DETAILS & "LastName")))

            ElseIf eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "FirstName") <> "" And _
                eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "LastName") = "" Then
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "FirstName")))
            Else
                XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "FirstName")))
                XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "LastName")))
            End If
        Else
            XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "FirstName")))
            XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "LastName")))
        End If
        XMLCustomerShip.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Company")))
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Phone")))

        ' SHIPPING ADDRESS
        strCountry = eShopCONNECTFacade.GetField("CountryCode", "SystemCountry", "ISOCode = '" & eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "CountryCode") & "'")
        XMLShippingAddress = New XElement("ShippingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address") <> "" Then
            If eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address2") <> "" Then
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address2")))
            Else
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address")))
            End If
        ElseIf eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address2") <> "" Then
            XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address2")))
        Else
            XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Address")))
        End If
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "City")))
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "StateCode").Length <= 2 Then
            XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "StateCode")))
        Else
            XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "StateCode")))
        End If
        XMLShippingAddress.Add(New XElement("Country", strCountry))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "ZipCode")))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper = "YES" Then
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS(THREE_D_CART_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Method"), "", strShippingMethodGroup)))
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Method") = "" Then
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD)))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XML3DCartShipment, THREE_D_CART_ORDER_SHIPMENT_DETAILS & "Method")))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))
        End If

        Return XMLResult

    End Function

    Private Function PaymentDetails(ByVal ThreeDCartConfig As ThreeDCartSettings, ByVal XMP3DCartOrder As XDocument, ByRef DoNotImport As Boolean) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLPaymentType As XElement, XMLResult As XElement
        Dim str3DCartPaymentType As String

        DoNotImport = False
        str3DCartPaymentType = eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_HEADER & "PaymentMethod")
        XMLResult = New XElement("PaymentDetails")
        XMLResult.Add(New XElement("PaymentMethod", "Source"))
        XMLPaymentType = New XElement("PaymentType")
        If ThreeDCartConfig.EnablePaymentTypeTranslation Then
            XMLPaymentType.Value = eShopCONNECTFacade.TranslatePaymentTypeToIS(THREE_D_CART_SOURCE_CODE, str3DCartPaymentType, str3DCartPaymentType, DoNotImport)
        Else
            XMLPaymentType.Value = str3DCartPaymentType
        End If
        XMLResult.Add(XMLPaymentType)
        XMLResult.Add(New XElement("SourcePaymentID", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrder, THREE_D_CART_ORDER_HEADER & "Transaction/TransactionId")))

        Return XMLResult

    End Function

    Private Function ItemDetails(ByVal XMP3DCartOrderItem As XDocument, ByVal ThreeDCartConfig As ThreeDCartSettings, ByRef ErrorMessage As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ' 22/05/14 | TJS             | 2014.0.01 | Modified for Unit of Measure on SKU Alias lookup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, strTemp As String
        Dim decItemQuantity As Decimal, decItemUnitPrice As Decimal

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrderItem, THREE_D_CART_ORDER_ITEM_DETAILS & "ProductID")
        ' is there an SKU for the item ?
        If strTemp <> "" Then
            ' yes, has Custom SKU Processing been enabled ?
            If ThreeDCartConfig.CustomSKUProcessing <> "" Then
                ' yes, convert SKU
                strTemp = ConvertSKU(ThreeDCartConfig.CustomSKUProcessing, strTemp)
            End If
            If ThreeDCartConfig.EnableSKUAliasLookup Then
                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", THREE_D_CART_SOURCE_CODE, "@SourceSKU", strTemp}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then
                    strTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221
                    XMLResult.Add(New XElement("ISItemCode", strTemp))
                    If Not eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).IsUnitMeasureCode_DEV000221Null Then ' TJS 22/05/14
                        XMLResult.Add(New XElement("ItemUnitOfMeasure", eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).UnitMeasureCode_DEV000221)) ' TJS 22/05/14
                    End If
                Else
                    XMLResult.Add(New XElement("IS" & ThreeDCartConfig.ISItemIDField, strTemp))
                End If

            Else
                XMLResult.Add(New XElement("IS" & ThreeDCartConfig.ISItemIDField, strTemp))

            End If
        Else
            ErrorMessage = "No Product SKU found for 3DCart Order Item " & eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrderItem, THREE_D_CART_ORDER_ITEM_DETAILS & "ProductName")
            Return XMLResult
        End If
        XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrderItem, THREE_D_CART_ORDER_ITEM_DETAILS & "ProductName")))
        decItemQuantity = CDec(eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrderItem, THREE_D_CART_ORDER_ITEM_DETAILS & "Quantity"))
        XMLResult.Add(New XElement("ItemQuantity", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrderItem, THREE_D_CART_ORDER_ITEM_DETAILS & "Quantity")))
        decItemUnitPrice = CDec(eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrderItem, THREE_D_CART_ORDER_ITEM_DETAILS & "UnitPrice"))
        XMLResult.Add(New XElement("ItemUnitPrice", eShopCONNECTFacade.GetXMLElementText(XMP3DCartOrderItem, THREE_D_CART_ORDER_ITEM_DETAILS & "UnitPrice")))

        XMLResult.Add(New XElement("ItemSubTotal", Format(decItemQuantity * decItemUnitPrice, "0.00")))
        decSubTotal = decSubTotal + (decItemQuantity * decItemUnitPrice)

        ItemDetails = XMLResult

    End Function

    Private Function DiscountCoupon(ByVal XMP3DCartOrder As XDocument, ByVal DiscountDescription As String, ByVal DiscountTotal As Decimal) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, decSalesTax As Decimal, decShipping As Decimal
        Dim decTotal As Decimal, strTemp As String

        XMLResult = New XElement("OrderTotals")

        XMLResult.Add(New XElement("SubTotal", Format(decSubTotal, "0.00")))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, THREE_D_CART_ORDER_HEADER & "Tax")
        decSalesTax = decSalesTax + CDec(strTemp)
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, THREE_D_CART_ORDER_HEADER & "Tax2")
        decSalesTax = decSalesTax + CDec(strTemp)
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, THREE_D_CART_ORDER_HEADER & "Tax3")
        decSalesTax = decSalesTax + CDec(strTemp)

        XMLResult.Add(New XElement("Tax", Format(decSalesTax, "0.00")))

        decShipping = CDec(eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, THREE_D_CART_ORDER_HEADER & "Shipping"))
        XMLResult.Add(New XElement("Shipping", Format(decShipping, "0.00")))

        decTotal = CDec(eShopCONNECTFacade.GetXMLElementText(XMLStoreFrontOrder, THREE_D_CART_ORDER_HEADER & "Total"))
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case ProcessingMode
            Case "Lerryn Test"
                Return "NONSTOCK"

            Case Else
                Return SourceSKU

        End Select

    End Function

End Module
