' eShopCONNECT for Connected Business
' Module: ShopComOrderCode.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Connected Business SDK and may incorporate certain intellectual 
' property of Interprise Solutions Inc. who's
' rights are hereby recognised.
'

'-------------------------------------------------------------------
'
' Updated 10 June 2012

Imports System
Imports System.Diagnostics ' TJS 11/02/09
Imports System.IO ' TJS 02/12/11
Imports System.Web
Imports Microsoft.VisualBasic
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Public Module ShopComOrderCode

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private dblItemShipping As Decimal
    Private Const csSourceName As String = "ShopComOrder"
    Private strISItemIDFIeld As String ' TJS 18/02/09
    Private strSourceItemIDField As String ' TJS 18/02/09
    Private strCustomSKUProcessing As String ' TJS 19/08/10
    Private bUKNotUSDateFormat As Boolean ' TJS 19/06/09
    Private bPricesIncludeTax As Boolean ' TJS 02/06/09

    Public Sub ProcessShopComRequest(ByVal context As HttpContext)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 31/01/09 | TJS             | 2009.1.03 | Corrected ReadCustomereShopCONNECTImportView_DEV000221 
        '                                        | to ReadCustomerImportExportView_DEV000221
        ' 06/02/09 | TJS             | 2009.1.05 | Modified to cater for multiple Shop.com merchant IDs
        ' 11/02/09 | TJS             | 2009.1.07 | Added error trapping to catch DB login errors etc
        ' 18/02/09 | TJS             | 2009.1.08 | Modified to cater for Config setting ItemIDField
        ' 10/03/09 | TJS             | 2009.1.09 | Modified to remove case sensivity in XML header, to handle 
        '                                        | ISCustomerCode element not in XML and corrected error handling
        ' 16/03/09 | TJS             | 2009.1.10 | Modified to check for unknown currency indicators and log sales order number
        ' 25/03/09 | TJS             | 2009.1.11 | Added logout for SP5x
        ' 06/05/09 | TJS             | 2009.2.05 | Modified to return structured XML response on major source errors,
        '                                        | shortened Event Log app name and extended error details
        ' 12/05/09 | TJS             | 2009.1.06 | Modified to cater for Shop.com confusion re GET or POST
        ' 02/06/09 | TJS             | 2009.2.10 | Modified to cater for SOURCE_CONFIG_XML_SHOPDOTCOM_PRICES_ARE_TAX_INCLUSIVE
        ' 19/06/09 | TJS             | 2009.3.00 | Modified to cater for SOURCE_CONFIG_XML_SHOPDOTCOM_XML_DATE_FORMAT
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use EventLog source created during install as IIS 
        '                                        | generally doesn't have permissions to create an event source
        ' 06/10/09 | TJS             | 2009.3.07 | Modified to cater for reprocessing records
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for QuoteToConvert parameter on XMLOrderImport
        '                                        | and CustomSKUProcessing
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 26/10/11 | TJS             | 2011.1.xx | Corrected setting of source tax values and codes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and to use InputStream
        '                                        |  instead of Form to get input XML without spaces being changed to +.  Also modified
        '                                        | to set LastWebPost_DEV000221 so config form can check if web service is installed and active
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLRequest As XDocument, XMLGenericOrder As XDocument, XMLTemp As XDocument, XMLItemTemp As XDocument
        Dim XMLShopComMerchantConfig As XDocument, XMLShopComMerchantList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 06/02/09
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLOrderNode As XElement, XMLItemNode As XElement, xmlOrderHeaderNode As XElement, XMLOrderSourceNode As XElement
        Dim XMLISOrderImportNode As XElement, XMLShopComOrderResponse As XElement, XMLShopComResponseNode As XElement
        Dim XMLImportResponseNode As XElement, XMLOrderResponseNode As XElement, XMLShopComMerchant As XElement ' TJS 06/02/09
        Dim strInputXML As String, strErrCode As String, strError As String, strErrorMessage As String ' TJS 11/02/09
        Dim sTemp As String, strTaxCodeForSourceTax As String, bMerchantIDMatched As Boolean, bValidationError As Boolean ' TJS 06/02/09 TJS 16/03/09 TJS 26/10/11
        Dim reader As StreamReader ' TJS 02/12/11 

        Try
            context.Response.ContentType = "text/xml"
            strErrCode = ""
            strErrorMessage = "" ' TJS 11/02/09
            strInputXML = "" ' TJS 12/05/09
            bValidationError = False ' TJS 16/03/09
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, New Lerryn.facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12 -EMILY 10.11.2018

            Try
                ' Read XML and convert to Generic format
                If context.Request.HttpMethod = "POST" Then ' TJS 12/05/09
                    reader = New StreamReader(context.Request.InputStream()) ' TJS 02/12/11 
                    strInputXML = eShopCONNECTFacade.ConvertXMLFromWeb(reader.ReadToEnd()) ' TJS 02/12/11 
                    strInputXML = eShopCONNECTFacade.ConvertXMLFromWeb(reader.ReadToEnd()).Replace("%20", " ") ' TJS 02/12/11 TJS 19/04/12
                ElseIf context.Request.HttpMethod = "GET" Then ' TJS 12/05/09
                    strInputXML = context.Request.QueryString.ToString.Replace("%20", " ") ' TJS 12/05/09 TJS 19/04/12
                Else
                    strInputXML = "" ' TJS 12/05/09
                End If
                'Check the file is at least 40 chars (This prevents errors when checking for the xml version and encoding)
                If strInputXML.Length > 40 Then ' TJS 12/05/09
                    ' check XML starts with correct headers
                    If Left(Trim(strInputXML.ToLower), 38) = "<?xml version=""1.0"" encoding=""utf-8""?>" Then ' TJS 10/03/09
                        ' yes, load into XML cocument
                        XMLRequest = XDocument.Parse(Trim(strInputXML))
                        XMLOrderList = XMLRequest.XPathSelectElements(SHOPDOTCOM_ORDER_LIST)
                        ' check module and connector are activated and valid
                        If eShopCONNECTFacade.ValidateSource("ShopComOrder.ashx", csSourceName, "", False) Then ' TJS 06/10/09
                            ' yes, check Shop.com Catalog ID is correct
                            XMLShopComMerchantConfig = XDocument.Parse(eShopCONNECTFacade.SourceConfig.ToString) ' TJS 06/02/09
                            XMLShopComMerchantList = XMLShopComMerchantConfig.XPathSelectElements(SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST) ' TJS 06/02/09 TJS 09/05/09
                            bMerchantIDMatched = False ' TJS 06/02/09
                            For Each XMLShopComMerchant In XMLShopComMerchantList ' TJS 06/02/09
                                XMLTemp = XDocument.Parse(XMLShopComMerchant.ToString) ' TJS 06/02/09
                                If eShopCONNECTFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_ID) = _
                                    eShopCONNECTFacade.GetXMLElementAttribute(XMLRequest, "CC_TRANSMISSION", "CATALOG_ID") Then ' TJS 06/02/09
                                    strISItemIDFIeld = eShopCONNECTFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_IS_ITEM_ID_FIELD) ' TJS 18/02/09
                                    strSourceItemIDField = eShopCONNECTFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_SOURCE_ITEM_ID_FIELD) ' TJS 18/02/09
                                    strCustomSKUProcessing = eShopCONNECTFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CUSTOM_SKU_PROCESSING) ' TJS 19/08/10
                                    bPricesIncludeTax = (eShopCONNECTFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_PRICES_ARE_TAX_INCLUSIVE).ToUpper = "YES") ' TJS 02/06/09
                                    bUKNotUSDateFormat = (eShopCONNECTFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_XML_DATE_FORMAT).ToUpper = "DD/MM/YYYY") ' TJS 19/06/09
                                    strTaxCodeForSourceTax = eShopCONNECTFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_TAX_CODE_FOR_SOURCE_TAX) ' TJS 26/10/11
                                    bMerchantIDMatched = True ' TJS 06/02/09
                                End If
                            Next

                            If bMerchantIDMatched Then ' TJS 06/02/09
                                ' yes, process orders
                                XMLShopComOrderResponse = New XElement("CC_TRANSMISSION_RESPONSE") ' TJS 16/03/09

                                For Each XMLOrderNode In XMLOrderList
                                    XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                                    ' create Generic XML document
                                    XMLGenericOrder = New XDocument
                                    XMLISOrderImportNode = New XElement("eShopCONNECT")
                                    XMLOrderSourceNode = New XElement("Source")
                                    XMLOrderSourceNode.Add(New XElement("SourceName", "Shop.com Order"))
                                    XMLOrderSourceNode.Add(New XElement("SourceCode", csSourceName))
                                    XMLISOrderImportNode.Add(XMLOrderSourceNode)

                                    ' ORDER HEADER
                                    xmlOrderHeaderNode = New XElement("Order")
                                    xmlOrderHeaderNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "ORDER_NO")))
                                    xmlOrderHeaderNode.Add(New XElement("SourceOrderRef", eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO")))
                                    sTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & SHOP_COM_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & eShopCONNECTFacade.GetXMLElementAttribute(XMLRequest, "CC_TRANSMISSION", "CATALOG_ID").Replace("'", "''") & "'") ' TJS 02/12/11
                                    If "" & sTemp <> "" Then ' TJS 25/04/11
                                        xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", sTemp)) ' TJS 02/12/11
                                    Else
                                        xmlOrderHeaderNode.Add(New XElement("SourceWebSiteRef", "Shop.com"))
                                    End If
                                    xmlOrderHeaderNode.Add(New XElement("SourceMerchantID", eShopCONNECTFacade.GetXMLElementAttribute(XMLRequest, "CC_TRANSMISSION", "CATALOG_ID"))) ' TJS 11/02/09
                                    If bPricesIncludeTax Then ' TJS 26/10/11
                                        xmlOrderHeaderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 26/10/11
                                        xmlOrderHeaderNode.Add(New XElement("TaxCodeForSourceTax", strTaxCodeForSourceTax)) ' TJS 26/10/11
                                    End If
                                    ' which date format is being used in Shop.com xml
                                    If bUKNotUSDateFormat Then ' TJS 19/06/09
                                        ' get Shop.com order date (UK Format) and covert
                                        sTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, SHOPDOTCOM_ORDER_TOTALS & "/TL_ORDER_DATE").Substring(6, 4) & "-" & _
                                            eShopCONNECTFacade.GetXMLElementText(XMLTemp, SHOPDOTCOM_ORDER_TOTALS & "/TL_ORDER_DATE").Substring(3, 2) & "-" & _
                                            eShopCONNECTFacade.GetXMLElementText(XMLTemp, SHOPDOTCOM_ORDER_TOTALS & "/TL_ORDER_DATE").Substring(0, 2) ' TJS 19/06/09
                                    Else
                                        ' get Shop.com order date (US Format) and covert
                                        sTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, SHOPDOTCOM_ORDER_TOTALS & "/TL_ORDER_DATE").Substring(6, 4) & "-" & _
                                            eShopCONNECTFacade.GetXMLElementText(XMLTemp, SHOPDOTCOM_ORDER_TOTALS & "/TL_ORDER_DATE").Substring(0, 2) & "-" & _
                                            eShopCONNECTFacade.GetXMLElementText(XMLTemp, SHOPDOTCOM_ORDER_TOTALS & "/TL_ORDER_DATE").Substring(3, 2)
                                    End If
                                    xmlOrderHeaderNode.Add(New XElement("OrderDate", sTemp))
                                    sTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, SHOPDOTCOM_ORDER_TOTALS & "/TL_TOTAL").Substring(0, 1) ' TJS 16/03/09
                                    If sTemp = "�" Then ' TJS 18/02/09 TJS 16/03/09
                                        xmlOrderHeaderNode.Add(New XElement("OrderCurrency", "GBP")) ' TJS 18/02/09
                                    ElseIf sTemp = "$" Then ' TJS 18/02/09 TJS 16/03/09
                                        xmlOrderHeaderNode.Add(New XElement("OrderCurrency", "USD"))
                                    Else ' TJS 16/03/09
                                        ' start of code added TJS 16/03/09
                                        bValidationError = True
                                        XMLShopComResponseNode = New XElement("ORDER")
                                        XMLShopComResponseNode.SetAttributeValue("ALTURA_INVOICE_NO", eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO"))
                                        XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                        XMLShopComResponseNode = New XElement("STATUS")
                                        XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                                        XMLShopComResponseNode.SetAttributeValue("MESSAGE", "Unknown currency " & sTemp)
                                        XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Shop.com Order Import Error", "Unknown currency " & sTemp, eShopCONNECTFacade.ConvertXMLFromWeb(strInputXML)) ' TJS 12/05/09
                                        ' end of code added TJS 16/03/09
                                    End If
                                    If bPricesIncludeTax Then ' TJS 02/06/09
                                        xmlOrderHeaderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 02/06/09
                                    End If

                                    If Not bValidationError Then ' TJS 16/03/09
                                        ' BILLING DETAILS
                                        xmlOrderHeaderNode.Add(BillingDetails(XMLTemp))

                                        ' SHIPPING DETAILS
                                        xmlOrderHeaderNode.Add(ShippingDetails(XMLTemp))

                                        ' PAYMENT DETAILS
                                        xmlOrderHeaderNode.Add(PaymentDetails(XMLTemp))

                                        ' ITEM DETAILS
                                        dblItemShipping = 0
                                        XMLItemList = XMLTemp.XPathSelectElements(SHOPDOTCOM_ORDER_ITEM_LIST)
                                        For Each XMLItemNode In XMLItemList
                                            XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                            xmlOrderHeaderNode.Add(ItemDetails(XMLItemTemp, strErrorMessage)) ' TJS 11/02/09
                                            If strErrorMessage <> "" Then ' TJS 11/02/09
                                                Exit For ' TJS 11/02/09
                                            End If
                                        Next
                                        ' did we get any errors ?
                                        If strErrorMessage = "" Then ' TJS 11/02/09
                                            ' no, 
                                            xmlOrderHeaderNode.Add(OrderTotals(XMLTemp))
                                            xmlOrderHeaderNode.Add(New XElement("OrderCount", "1"))

                                            XMLISOrderImportNode.Add(xmlOrderHeaderNode)
                                            XMLGenericOrder.Add(XMLISOrderImportNode)

                                            sTemp = eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH)
                                            If sTemp <> "" Then
                                                If sTemp.Substring(sTemp.Length - 1, 1) <> "\" Then
                                                    sTemp = sTemp & "\"
                                                End If
                                                XMLTemp.Save(sTemp & "ShopDotCom_Invoice_" & eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO") & ".xml")
                                                XMLGenericOrder.Save(sTemp & "GenericXML_for_ShopDotCom_Invoice_" & eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO") & ".xml")
                                            End If

                                            'Generic XML Built, now do call to create order and customer (where customer does not exist)
                                            XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "") ' TJS 19/08/10
                                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                                            If XMLOrderResponseNode IsNot Nothing Then
                                                If XMLOrderResponseNode.Value = "Success" Then
                                                    XMLShopComResponseNode = New XElement("ORDER")
                                                    XMLShopComResponseNode.SetAttributeValue("ALTURA_INVOICE_NO", eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO"))
                                                    XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                                    XMLShopComResponseNode = New XElement("STATUS")
                                                    XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "700")
                                                    XMLShopComResponseNode.SetAttributeValue("MESSAGE", "Invoice received successfully")
                                                    XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                                    eShopCONNECTFacade.WriteLogProgressRecord("Shop.com Invoice ID " & eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO") & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value) ' TJS 10/03/09 TJS 16/03/09
                                                Else
                                                    ' start of code added TJS 11/02/09
                                                    XMLShopComResponseNode = New XElement("ORDER")
                                                    XMLShopComResponseNode.SetAttributeValue("ALTURA_INVOICE_NO", eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO"))
                                                    XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                                    XMLShopComResponseNode = New XElement("STATUS")
                                                    XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                                                    XMLShopComResponseNode.SetAttributeValue("MESSAGE", XMLImportResponseNode.XPathSelectElement("/ImportResponse/ErrorMessage").Value)
                                                    XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                                    ' end of code added TJS 11/02/09
                                                    eShopCONNECTFacade.WriteLogProgressRecord("Shop.com Invoice ID " & eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO") & " failed to import") ' TJS 10/03/09
                                                End If
                                            Else
                                                ' start of code added TJS 11/02/09
                                                XMLShopComResponseNode = New XElement("ORDER")
                                                XMLShopComResponseNode.SetAttributeValue("ALTURA_INVOICE_NO", eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO"))
                                                XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                                XMLShopComResponseNode = New XElement("STATUS")
                                                XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                                                XMLShopComResponseNode.SetAttributeValue("MESSAGE", XMLImportResponseNode.ToString)
                                                XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                                ' end of code added TJS 11/02/09
                                                eShopCONNECTFacade.WriteLogProgressRecord("Shop.com Invoice ID " & eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO") & " failed to import") ' TJS 10/03/09
                                            End If
                                        Else
                                            ' start of code added TJS 11/02/09
                                            XMLShopComResponseNode = New XElement("ORDER")
                                            XMLShopComResponseNode.SetAttributeValue("ALTURA_INVOICE_NO", eShopCONNECTFacade.GetXMLElementAttribute(XMLTemp, "CC_ORDER", "INVOICE_NO"))
                                            XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                            XMLShopComResponseNode = New XElement("STATUS")
                                            XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                                            XMLShopComResponseNode.SetAttributeValue("MESSAGE", strErrorMessage)
                                            XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                                            eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Shop.com Order Import Error", strErrorMessage, eShopCONNECTFacade.ConvertXMLFromWeb(strInputXML)) ' TJS 12/05/09
                                            ' end of code added TJS 11/02/09
                                        End If
                                    End If
                                    XMLGenericOrder = Nothing
                                Next
                                context.Response.Write(XMLShopComOrderResponse.ToString)
                            Else
                                strErrCode = "003 - Incorrect Shop.com Catalog ID"
                            End If
                        Else
                            strErrCode = "002 - source activation or validation error"
                        End If
                    Else
                        strErrCode = "001 - missing or invalid XML order data"
                    End If
                Else
                    strErrCode = "001 - missing or invalid XML order data"
                End If

                If strErrCode <> "" Then
                    If Left(UCase(context.Request.ServerVariables("SERVER_NAME")), 5) <> "LERHQ" And Left(UCase(context.Request.ServerVariables("SERVER_NAME")), 6) <> "LERRYN" Then
                        strError = context.Request.ServerVariables("SERVER_NAME") & ", ShopComOrder.ashx - Error Code " & strErrCode
                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Shop.com Order Import Error", strError, eShopCONNECTFacade.ConvertXMLFromWeb(strInputXML)) ' TJS 12/05/09
                    End If
                    ' start of code added TJS 06/05/09
                    XMLShopComOrderResponse = New XElement("CC_TRANSMISSION_RESPONSE")
                    XMLShopComResponseNode = New XElement("STATUS")
                    XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                    XMLShopComResponseNode.SetAttributeValue("MESSAGE", strErrCode)
                    XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                    context.Response.Write(XMLShopComOrderResponse.ToString)
                    ' end of code added TJS 06/05/09
                End If

                Try ' TJS 02/12/11
                    eShopCONNECTFacade.ExecuteNonQuery(System.Data.CommandType.Text, "UPDATE LerrynImportExportServiceAction_DEV000221 SET LastWebPost_DEV000221 = getdate()", Nothing) ' TJS 02/12/11
                Catch ex As Exception
                    eShopCONNECTFacade.SendErrorEmail(eShopCONNECTFacade.SourceConfig, "RunRoutines", ex) ' TJS 02/12/11
                End Try

                Try ' TJS 25/03/09
                    ' now logout
                    Interprise.Facade.Base.SimpleFacade.Instance.SignOut() ' TJS 25/03/09

                Catch ex As Exception
                    ' ignore error on logout as function does not exist pre SP5
                End Try

                eShopCONNECTFacade.Dispose()
                eShopCONNECTDatasetGateway.Dispose()

            Catch ex As Exception ' TJS 11/02/09
                eShopCONNECTFacade.SendErrorEmail(eShopCONNECTFacade.SourceConfig, "Shop.com Order Import Error", ex, eShopCONNECTFacade.ConvertXMLFromWeb(strInputXML)) ' TJS 11/02/09 TJS 12/05/09
                ' start of code added TJS 06/05/09
                XMLShopComOrderResponse = New XElement("CC_TRANSMISSION_RESPONSE")
                XMLShopComResponseNode = New XElement("STATUS")
                XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                XMLShopComResponseNode.SetAttributeValue("MESSAGE", "Shop.com Order Import Error - " & ex.Message & ", " & ex.StackTrace)
                XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                context.Response.Write(XMLShopComOrderResponse.ToString)
                ' end of code added TJS 06/05/09

            End Try

        Catch ex As Exception ' TJS 11/02/09

            Try
                Dim objEventLog As New EventLog() ' TJS 11/02/09

                objEventLog.Source = "eShopCONNECT" ' TJS 11/02/09 TJS 23/08/09

                objEventLog.WriteEntry("ProcessShopComRequest - " & ex.Message & " , " & ex.StackTrace, EventLogEntryType.Error) ' TJS 11/02/09 TJS 06/05/09
                ' start of code added TJS 06/05/09
                XMLShopComOrderResponse = New XElement("CC_TRANSMISSION_RESPONSE")
                XMLShopComResponseNode = New XElement("STATUS")
                XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                XMLShopComResponseNode.SetAttributeValue("MESSAGE", "ProcessShopComRequest - " & ex.Message & ", " & ex.StackTrace)
                XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                context.Response.Write(XMLShopComOrderResponse.ToString)
                ' end of code added TJS 06/05/09

            Catch ex2 As Exception
                context.Response.Write("ProcessShopComRequest2 - " & ex2.Message) ' TJS 11/02/09 TJS 06/05/09
                ' start of code added TJS 06/05/09
                XMLShopComOrderResponse = New XElement("CC_TRANSMISSION_RESPONSE")
                XMLShopComResponseNode = New XElement("STATUS")
                XMLShopComResponseNode.SetAttributeValue("STATUS_CODE", "999")
                XMLShopComResponseNode.SetAttributeValue("MESSAGE", "Error writing event log - " & ex.Message & ", " & ex.StackTrace)
                XMLShopComOrderResponse.Add(XMLShopComResponseNode)
                context.Response.Write(XMLShopComOrderResponse.ToString)
                ' end of code added TJS 06/05/09

            End Try

        End Try

    End Sub

    Private Function BillingDetails(ByVal XMLShopComOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 18/02/09 | TJS             | 2009.1.08 | Modified to cater for UK address formats
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBil As XElement, XMLBillingAddress As XElement
        Dim XMLResult As XElement, strCountry As String

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBil = New XElement("Customer")
        XMLCustomerBil.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_SHOPPER_ID")))
        XMLCustomerBil.Add(New XElement("NamePrefix", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_NAME_PREFIX")))
        XMLCustomerBil.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_FIRST_NAME")))
        XMLCustomerBil.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_LAST_NAME")))
        XMLCustomerBil.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_COMPANY")))
        If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_COMPANY") <> "" Then
            XMLCustomerBil.Add(New XElement("WorkPhone", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_PHONE")))
        Else
            XMLCustomerBil.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_PHONE")))
        End If
        XMLCustomerBil.Add(New XElement("Email", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_CUSTOMER & "/CU_EMAIL")))

        ' BILLING ADDRESS
        strCountry = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_COUNTRY")
        XMLBillingAddress = New XElement("BillingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_FLAT") <> "" Then ' TJS 18/02/09
            If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS2") <> "" Then ' TJS 18/02/09
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_FLAT") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS1") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS2"))) ' TJS 18/02/09
            Else
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_FLAT") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS1"))) ' TJS 18/02/09
            End If
        Else
            If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS2") <> "" Then ' TJS 18/02/09
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS1") & _
                    vbCr & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS2"))) ' TJS 18/02/09
            Else
                XMLBillingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ADDRESS1")))
            End If
        End If
        XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_CITY")))
        If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_COUNTRY_CODE") = "GB" Then ' TJS 18/02/09
            If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_STATE") <> "UK Mainland" And _
                eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_PROVINCE") = "" Then ' TJS 18/02/09
                XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_STATE"))) ' TJS 18/02/09
            ElseIf eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_STATE") = "UK Mainland" And _
                eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_PROVINCE") <> "" Then ' TJS 18/02/09
                XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_PROVINCE"))) ' TJS 18/02/09
            End If
        Else
            XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_PROVINCE"))) ' TJS 18/02/09
            XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_STATE")))
        End If
        ' need to ensure Country matches those used in IS
        If strCountry = "United States" Then
            strCountry = "United States of America"
        End If
        XMLBillingAddress.Add(New XElement("Country", strCountry))
        XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_BILLING_LABEL_ADDRESS & "/AD_ZIP")))

        XMLResult.Add(XMLCustomerBil)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult

    End Function

    Private Function ShippingDetails(ByVal XMLShopComOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 18/02/09 | TJS             | 2009.1.08 | Modified to correctly extract shipping method when no 
        '                                        | translation enabled and to cater for UK address formats
        ' 15/12/09 | TJS             | 2009.3.09 | Modified to cater for SourceDeliveryClass on TranslateDeliveryMethodToIS
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strShippingMethodGroup As String


        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        XMLCustomerShip.Add(New XElement("NamePrefix", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_CUSTOMER & "/CU_NAME_PREFIX")))
        XMLCustomerShip.Add(New XElement("FirstName", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_CUSTOMER & "/CU_FIRST_NAME")))
        XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_CUSTOMER & "/CU_LAST_NAME")))
        XMLCustomerShip.Add(New XElement("Company", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_CUSTOMER & "/CU_COMPANY")))
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_CUSTOMER & "/CU_PHONE")))
        XMLCustomerShip.Add(New XElement("Email", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_CUSTOMER & "/CU_EMAIL")))

        ' SHIPPING ADDRESS
        strCountry = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_COUNTRY")
        XMLShippingAddress = New XElement("ShippingAddress")
        If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_FLAT") <> "" Then ' TJS 18/02/09
            If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS2") <> "" Then ' TJS 18/02/09
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_FLAT") & _
                    vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS1") & _
                    vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS2"))) ' TJS 18/02/09
            Else
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_FLAT") & _
                    vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS1")))
            End If
        Else
            If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS2") <> "" Then ' TJS 18/02/09
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS1") & _
                    vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS2"))) ' TJS 18/02/09
            Else
                XMLShippingAddress.Add(New XElement("Address", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ADDRESS1")))
            End If
        End If
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_CITY")))
        If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_COUNTRY_CODE") = "GB" Then ' TJS 18/02/09
            If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_STATE") <> "UK Mainland" And _
                eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_PROVINCE") = "" Then ' TJS 18/02/09
                XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_STATE"))) ' TJS 18/02/09
            ElseIf eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_STATE") = "UK Mainland" And _
                eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_PROVINCE") <> "" Then ' TJS 18/02/09
                XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_PROVINCE"))) ' TJS 18/02/09
            End If
        Else
            XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_PROVINCE"))) ' TJS 18/02/09
            XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_STATE")))
        End If
        ' need to ensure Country matches those used in IS
        If strCountry = "United States" Then
            strCountry = "United States of America"
        End If
        XMLShippingAddress.Add(New XElement("Country", strCountry))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL_ADDRESS & "/AD_ZIP")))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper = "YES" Then
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS("ShopDotComOrder", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL & "/SL_METHOD"), "", strShippingMethodGroup))) ' TJS 15/12/09
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL & "/SL_METHOD") = "" Then ' TJS 18/02/09
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD)))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_SHIPPING_LABEL & "/SL_METHOD")))
            XMLResult.Add(New XElement("ShippingMethodGroup", eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)))
        End If

        Return XMLResult

    End Function

    Private Function PaymentDetails(ByVal XMLShopComOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLPaymentDetails As XElement, XMLResult As XElement

        XMLResult = New XElement("PaymentDetails")
        If eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_NUMBER") <> "" Then
            XMLResult.Add(New XElement("PaymentMethod", "Credit Card"))
            'CREDIT CARD DETAILS
            XMLPaymentDetails = New XElement("CreditCardDetails")
            XMLPaymentDetails.Add(New XElement("NameOnCard", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_NAMEONCARD")))
            XMLPaymentDetails.Add(New XElement("CardType", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_TYPE")))
            XMLPaymentDetails.Add(New XElement("CardIssuer", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_ISSUING_BANK")))
            XMLPaymentDetails.Add(New XElement("CardNumber", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_NUMBER")))
            XMLPaymentDetails.Add(New XElement("CardExpiryDate", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_EXPIRATION")))
            XMLPaymentDetails.Add(New XElement("CardSecurityNumber", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_SECURITY_NUMBER")))
            XMLPaymentDetails.Add(New XElement("CardIssueNumber", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_PAYMENT_CARD_DETAILS & "/CC_ISSUE_NUMBER")))
            XMLResult.Add(XMLPaymentDetails)

        End If
        Return XMLResult
    End Function

    Private Function ItemDetails(ByVal XMLShopComOrderItem As XDocument, ByRef ErrorMessage As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 11/02/09 | TJS             | 2009.1.07 | Modified to add ErrorMessage parameter and check for an Item Source Code
        ' 18/02/09 | TJS             | 2009.1.08 | Modified to cater for Config setting ISItemIDField and SourceItemIDField
        ' 16/03/09 | TJS             | 2009.1.10 | Modified to remove leading � as well as $ on currency 
        '                                        | fields and set empty currency fields to 0.00
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for CustomSKUProcessing
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLSpecialInstructTemp As XDocument, XMLSourceSpecialInstructions As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLResult As XElement, XMLSourceSpecialInstruction As XElement
        Dim XMLGenericSpecialInstruct As XElement, strTemp As String

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        XMLResult.Add(New XElement("SourceItemPurchaseID", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_PURCHASE_ID")))
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/" & strSourceItemIDField) ' TJS 19/08/10
        ' is there an SKU for the item ?
        If strTemp <> "" Then ' TJS 11/02/09 TJS 18/02/09 TJS 19/08/10
            ' yes, has Custom SKU Processing been enabled ?
            If strCustomSKUProcessing <> "" Then ' TJS 19/08/10
                ' yes, convert SKU
                strTemp = ConvertSKU(strCustomSKUProcessing, strTemp) ' TJS 19/08/10
            End If
            XMLResult.Add(New XElement("IS" & strISItemIDFIeld, strTemp)) ' TJS 18/02/09 TJS 19/08/10
        Else
            ErrorMessage = "No Item Source Code found for Shop.com Purchase ID " & eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_PURCHASE_ID") ' TJS 11/02/09
            Return XMLResult ' TJS 11/02/09
        End If
        XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_DESCRIPTION")))
        XMLResult.Add(New XElement("ItemQuantity", eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_QUANTITY")))
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_UNIT_PRICE")
        ' check for currency symbol at start, or empty price
        strTemp = RemoveCurrencySymbol(strTemp) ' TJS 16/03/09
        XMLResult.Add(New XElement("ItemUnitPrice", strTemp))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_SUB_TOTAL")
        ' check for currency symbol at start, or empty sub-total
        strTemp = RemoveCurrencySymbol(strTemp) ' TJS 16/03/09
        XMLResult.Add(New XElement("ItemSubTotal", strTemp))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_TAX")
        ' check for currency symbol at start, or empty tax
        strTemp = RemoveCurrencySymbol(strTemp) ' TJS 16/03/09
        XMLResult.Add(New XElement("ItemTaxValue", strTemp))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrderItem, SHOPDOTCOM_ORDER_ITEM & "/IT_SHIPPING")
        ' check for currency symbol at start, or empty shipping charge
        dblItemShipping = dblItemShipping + CDbl(RemoveCurrencySymbol(strTemp)) ' TJS 16/03/09

        XMLSourceSpecialInstructions = XMLShopComOrderItem.XPathSelectElements(SHOPDOTCOM_ORDER_ITEM_SPECIAL_INTRUCT_LIST)
        For Each XMLSourceSpecialInstruction In XMLSourceSpecialInstructions
            XMLSpecialInstructTemp = XDocument.Parse(XMLSourceSpecialInstruction.ToString)
            XMLGenericSpecialInstruct = New XElement("SpecialInstructions")
            XMLGenericSpecialInstruct.Add(New XElement("SpecialInstructionType", eShopCONNECTFacade.GetXMLElementText(XMLSpecialInstructTemp, SHOPDOTCOM_ORDER_ITEM_SPECIAL_INTRUCT & "/SI_TYPE")))
            XMLGenericSpecialInstruct.Add(New XElement("SpecialInstructionDetail", eShopCONNECTFacade.GetXMLElementText(XMLSpecialInstructTemp, SHOPDOTCOM_ORDER_ITEM_SPECIAL_INTRUCT & "/SI_VALUE")))
            XMLResult.Add(XMLGenericSpecialInstruct)
        Next
        ItemDetails = XMLResult

    End Function

    Private Function OrderTotals(ByVal XMLShopComOrder As XDocument) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 16/03/09 | TJS             | 2009.1.10 | Modified to remove leading � as well as $ on currency 
        '                                        | fields and set empty currency fields to 0.00
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResult As XElement, strTemp As String

        XMLResult = New XElement("OrderTotals")
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_TOTALS & "/TL_SUBTOTAL")
        ' check for currency symbol at start, or empty shipping charge
        strTemp = RemoveCurrencySymbol(strTemp) ' TJS 16/03/09
        XMLResult.Add(New XElement("SubTotal", strTemp))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_TOTALS & "/TL_TAX")
        ' check for currency symbol at start, or empty shipping charge
        strTemp = RemoveCurrencySymbol(strTemp) ' TJS 16/03/09
        XMLResult.Add(New XElement("Tax", strTemp))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_TOTALS & "/TL_SHIPPING")
        ' check for currency symbol at start, or empty shipping charge
        strTemp = RemoveCurrencySymbol(strTemp) ' TJS 16/03/09
        If dblItemShipping > 0 Then
            strTemp = (dblItemShipping + CDec(strTemp)).ToString("0.00")
        End If
        XMLResult.Add(New XElement("Shipping", strTemp))

        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLShopComOrder, SHOPDOTCOM_ORDER_TOTALS & "/TL_TOTAL")
        ' check for currency symbol at start, or empty shipping charge
        strTemp = RemoveCurrencySymbol(strTemp) ' TJS 16/03/09
        XMLResult.Add(New XElement("Total", strTemp))

        OrderTotals = XMLResult

    End Function

    Private Function RemoveCurrencySymbol(ByVal InputValue As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -     Checks for known Currency symbol at start of input value and removes it.  
        '                      If input value is empty, then set value to 0.00
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/03/09 | TJS             | 2009.1.10 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' is input value empty ?
        If InputValue.Length > 0 Then
            ' no, does it start with known currency symbol ?
            If InputValue.Substring(0, 1) = "$" Or InputValue.Substring(0, 1) = "�" Then
                ' yes, remove it
                Return InputValue.Substring(1)
            Else
                Return InputValue
            End If
        Else
            ' input value is empty, set to 0.00
            Return "0.00"
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
End Module
