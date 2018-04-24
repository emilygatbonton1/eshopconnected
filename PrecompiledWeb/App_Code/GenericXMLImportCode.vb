' eShopCONNECT for Connected Business
' Module: GenericXMLImportCode.vb
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
' Last Updated - 10 June 2012

Imports System
Imports System.Diagnostics ' TJS 11/02/09
Imports System.IO ' TJS 02/12/11
Imports System.Web
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Microsoft.VisualBasic
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11

Public Module GenericXMLImportCode

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade

    Public Sub ProcessGenericXMLRequest(ByVal context As HttpContext)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 23/01/09 | TJS             | 2009.1.01 | Modifed to allow blank Source and IS Customer IDs if config allows
        ' 31/01/09 | TJS             | 2009.1.03 | Corrected ReadCustomereShopCONNECTImportView_DEV000221 
        '                                        | to ReadCustomerImportExportView_DEV000221
        ' 11/02/09 | TJS             | 2009.1.07 | Added error trapping to catch DB login errors etc
        ' 08/03/09 | TJS             | 2009.1.09 | Modified to remove case sensivity in XML header, to handle 
        '                                        | ISCustomerCode element not in XML and corrected error handling
        ' 19/03/09 | TJS             | 2009.1.10 | Added log entry after import
        ' 25/03/09 | TJS             | 2009.1.11 | Added logout for SP5x
        ' 29/05/09 | TJS             | 2009.2.09 | Added checks for XML load errors
        ' 24/07/09 | TJS             | 2009.3.03 | Added code to process Leads, Prospects and Customers if the 
        '                                        | Prospect Importer connector is activated
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use EventLog source created during install as IIS 
        '                                        | generally doesn't have permissions to create an event source
        ' 25/08/09 | TJS             | 2009.3.05 | Tidied error response message
        ' 06/10/09 | TJS             | 2009.3.07 | Modified to cater for reprocessing orders from Volusion etc sources 
        ' 30/12/09 | TJS             | 2010.0.00 | Modified to cater for quote import
        ' 05/06/10 | TJS             | 2010.0.07 | Modified to use correct stored procedure when checking for customer using ISCustomerCode
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for QuoteToConvert parameter on XMLOrderImport
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and to use InputStream
        '                                        |  instead of Form to get input XML without spaces being changed to +.  Also modified
        '                                        | to set LastWebPost_DEV000221 so config form can check if web service is installed and active
        ' 02/04/12 | TJS             | 2011.2.12 | Moved check for reprocessing after check on source validation to prevent errors if activation has expired
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLRequest As XDocument, XMLSingleImportRecord As XDocument, XMLTemp As XDocument
        Dim XMLImportRecordNode As XElement, XMLSourceNode As XElement, XMLFacadeImportResponse As XElement
        Dim XMLImportResponseNode As XElement, XMLISImportNode As XElement, XMLeShopCONNECTNode As XElement
        Dim XMLISCustomerIDNode As XElement, XMLCustomer As XElement, XMLOrderResponseNode As XElement ' TJS 08/03/09 TJS 19/03/09
        Dim XMLImportRecordList As System.Collections.Generic.IEnumerable(Of XElement), reader As StreamReader ' TJS 02/12/11 
        Dim strInputXML As String, strErrCode As String, strError As String, strSourceCustomerID As String
        Dim strISCustomerCode As String, strSourceCode As String, sTemp As String, bCustomerValid As Boolean
        Dim bImportTypeValid As Boolean, bReprocess As Boolean, bXMLError As Boolean ' TJS 06/10/09

        Try

            context.Response.ContentType = "text/xml"
            strErrCode = ""
            strInputXML = "" ' TJS 08/03/09
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, New Lerryn.Business.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12

            Try

                'Check the file is at least 40 chars (This prevents errors when checking for the xml version and encoding)
                If context.Request.Form.ToString.Length > 40 Then
                    ' Read XML 
                    reader = New StreamReader(context.Request.InputStream()) ' TJS 02/12/11 
                    strInputXML = eShopCONNECTFacade.ConvertXMLFromWeb(reader.ReadToEnd()).Replace("%20", " ") ' TJS 02/12/11 TJS 19/04/12
                    ' check XML starts with correct headers
                    If Left(Trim(strInputXML.ToLower), 38) = "<?xml version=""1.0"" encoding=""utf-8""?>" Then ' TJS 08/03/09
                        ' yes, load into XML cocument
                        bXMLError = False
                        Try
                            XMLRequest = XDocument.Parse(Trim(strInputXML))

                        Catch ex As Exception
                            strErrCode = "005 - Input processing failed due to XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & Trim(strInputXML) ' TJS 29/05/09
                            bXMLError = True
                        End Try
                        ' did XML load correctly ?
                        If Not bXMLError Then
                            ' yes, check module and connector are activated and valid
                            strSourceCode = eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_CODE)
                            If eShopCONNECTFacade.ValidateSource("GenericXMLImport.ashx", strSourceCode, _
                                eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_AUTHENTICATION), bReprocess) Then ' TJS 06/10/09

                                bReprocess = False ' TJS 06/10/09
                                If eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_REPROCESS).ToLower = "true" Then ' TJS 06/10/09
                                    bReprocess = True ' TJS 06/10/09
                                End If

                                XMLeShopCONNECTNode = New XElement("eShopCONNECT")

                                bImportTypeValid = False
                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                    Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                        XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_QUOTE_LIST) ' TJS 30/12/09
                                        bImportTypeValid = True ' TJS 30/12/09

                                    Case GENERIC_XML_ORDER_IMPORT_TYPE
                                        XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_ORDER_LIST)
                                        bImportTypeValid = True

                                    Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                        XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_INVOICE_LIST)
                                        bImportTypeValid = True

                                    Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                        XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_CREDITNOTE_LIST)
                                        bImportTypeValid = True

                                        ' start of code added TJS 24/07/09
                                    Case GENERIC_XML_LEAD_IMPORT_TYPE, GENERIC_XML_PROSPECT_IMPORT_TYPE, GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                        If eShopCONNECTFacade.ConfigFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                                            Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                Case GENERIC_XML_LEAD_IMPORT_TYPE
                                                    XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_LEAD_LIST)

                                                Case GENERIC_XML_PROSPECT_IMPORT_TYPE
                                                    XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_PROSPECT_LIST)

                                                Case GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                                    XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_CUSTOMER_LIST)

                                            End Select
                                            bImportTypeValid = True

                                        Else
                                            XMLImportRecordList = Nothing
                                            XMLImportResponseNode = New XElement("ImportResponse")
                                            XMLImportResponseNode.Add(New XElement("Status", "Error"))
                                            XMLImportResponseNode.Add(New XElement("ErrorCode", "001"))
                                            XMLImportResponseNode.Add(New XElement("ErrorMessage", "Invalid Import Type " & eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE)))
                                            eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import", XMLImportResponseNode.ToString, strInputXML)
                                            XMLeShopCONNECTNode.Add(XMLImportResponseNode)
                                        End If
                                        ' end of code added TJS 24/07/09

                                    Case Else
                                        XMLImportRecordList = Nothing
                                        XMLImportResponseNode = New XElement("ImportResponse")
                                        XMLImportResponseNode.Add(New XElement("Status", "Error"))
                                        XMLImportResponseNode.Add(New XElement("ErrorCode", "001"))
                                        XMLImportResponseNode.Add(New XElement("ErrorMessage", "Unknown Import Type " & eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE)))
                                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import", XMLImportResponseNode.ToString, strInputXML) ' TJS 08/03/09
                                        XMLeShopCONNECTNode.Add(XMLImportResponseNode)

                                End Select


                                ' is import type valid and there are some orders, invoices or credit notes ?
                                If bImportTypeValid And XMLImportRecordList IsNot Nothing Then ' TJS 08/03/09
                                    ' yes, process records

                                    For Each XMLImportRecordNode In XMLImportRecordList
                                        bXMLError = False
                                        Try
                                            XMLTemp = XDocument.Parse(XMLImportRecordNode.ToString)

                                        Catch ex As Exception
                                            ' start of code added TJS 29/05/09
                                            XMLImportResponseNode = New XElement("ImportResponse")
                                            XMLImportResponseNode.Add(New XElement("Status", "Error"))
                                            Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                                    XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_CUSTOMER_ORDER_REF))) ' TJS 30/12/09
                                                    XMLImportResponseNode.Add(New XElement("SourceQuoteRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF))) ' TJS 30/12/09

                                                Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                    XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_CUSTOMER_ORDER_REF))) ' TJS 30/12/09
                                                    XMLImportResponseNode.Add(New XElement("SourceOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF)))

                                                Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                    XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_CUSTOMER_ORDER_REF))) ' TJS 30/12/09
                                                    XMLImportResponseNode.Add(New XElement("SourceInvoiceRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF)))

                                                Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                    XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_CUSTOMER_ORDER_REF))) ' TJS 30/12/09
                                                    XMLImportResponseNode.Add(New XElement("SourceCreditNoteRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF)))

                                            End Select
                                            XMLImportResponseNode.Add(New XElement("ErrorCode", "005"))
                                            XMLImportResponseNode.Add(New XElement("ErrorMessage", "Invalid XML - " & ex.Message.Replace(vbCrLf, "")))
                                            eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import", "Import processing failed due to XML error , reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLImportRecordNode.ToString, strInputXML)

                                            XMLeShopCONNECTNode.Add(XMLImportResponseNode)
                                            ' end of code added TJS 29/05/09
                                            bXMLError = True

                                        End Try

                                        ' did XML load correctly
                                        If Not bXMLError Then ' TJS 29/05/09
                                            ' yes, create Generic XML document for this import record
                                            XMLSingleImportRecord = New XDocument
                                            XMLISImportNode = New XElement("eShopCONNECT")
                                            XMLSourceNode = New XElement("Source")
                                            XMLSourceNode.Add(New XElement("SourceName", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_SOURCE_NAME)))
                                            XMLSourceNode.Add(New XElement("SourceCode", strSourceCode))
                                            XMLISImportNode.Add(XMLSourceNode)

                                            ' what import type are we processing ?
                                            ' Quotes, Orders, Invoices, Credit Notes are processed differently from Leads, Prospects and Customers
                                            If eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_QUOTE_IMPORT_TYPE Or _
                                                eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_ORDER_IMPORT_TYPE Or _
                                                eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_INVOICE_IMPORT_TYPE Or _
                                                eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_CREDITNOTE_IMPORT_TYPE Then ' TJS 24/07/09 TJS 30/12/09
                                                ' Import is an Order, Invoice or Credit Note
                                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                    Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                                        ' check if IS CustomerCode provided
                                                        strISCustomerCode = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode") ' TJS 30/12/09
                                                        ' get Source Customer ID
                                                        strSourceCustomerID = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID") ' TJS 30/12/09

                                                    Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                        ' check if IS CustomerCode provided
                                                        strISCustomerCode = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                        ' get Source Customer ID
                                                        strSourceCustomerID = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID")

                                                    Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                        ' check if IS CustomerCode provided
                                                        strISCustomerCode = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                        ' get Source Customer ID
                                                        strSourceCustomerID = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID")

                                                    Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                        ' check if IS CustomerCode provided
                                                        strISCustomerCode = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                        ' get Source Customer ID
                                                        strSourceCustomerID = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID")

                                                    Case Else
                                                        ' these lines added to suppress warning message
                                                        strISCustomerCode = ""
                                                        strSourceCustomerID = ""

                                                End Select

                                                ' check if customer exists in IS
                                                bCustomerValid = True
                                                If strISCustomerCode <> "" Then
                                                    eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.TableName, _
                                                      "ReadCustomerImportExportCustCodeView_DEV000221", AT_CUSTOMER_CODE, strISCustomerCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 31/01/09 TJS 05/06/10
                                                    If eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.Count = 0 Then
                                                        bCustomerValid = False
                                                    End If

                                                ElseIf strSourceCustomerID <> "" Then
                                                    eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.TableName, _
                                                     "ReadCustomerImportExportView_DEV000221", AT_IMPORT_CUSTOMER_ID, strSourceCustomerID, AT_IMPORT_SOURCE_ID, strSourceCode}}, _
                                                     Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 31/01/09
                                                    If eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.Count > 0 Then
                                                        strISCustomerCode = eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221(0).CustomerCode
                                                    Else
                                                        strISCustomerCode = ""
                                                    End If

                                                ElseIf eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_REQUIRE_SOURCE_CUSTOMER_ID) = "No" And _
                                                    eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower <> GENERIC_XML_CREDITNOTE_IMPORT_TYPE Then ' TJS 23/01/09
                                                    ' Blank Source Customer ID allowed, will always create new customer except for Credit Notes

                                                Else
                                                    bCustomerValid = False
                                                End If

                                                If bCustomerValid Then
                                                    XMLISImportNode.Add(XMLImportRecordNode)
                                                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                        Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                                            If strISCustomerCode <> "" Then ' TJS 30/12/09
                                                                XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode") ' TJS 30/12/09
                                                                If XMLISCustomerIDNode Is Nothing Then ' TJS 30/12/09
                                                                    XMLCustomer = XMLISImportNode.XPathSelectElement(GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER) ' TJS 30/12/09
                                                                    XMLCustomer.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 30/12/09
                                                                Else
                                                                    XMLISCustomerIDNode.Value = strISCustomerCode ' TJS 30/12/09
                                                                End If
                                                            End If
                                                            XMLISImportNode.Add(New XElement("QuoteCount", "1")) ' TJS 30/12/09

                                                        Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                            If strISCustomerCode <> "" Then ' TJS 08/03/09
                                                                XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                                If XMLISCustomerIDNode Is Nothing Then ' TJS 08/03/09
                                                                    XMLCustomer = XMLISImportNode.XPathSelectElement(GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER) ' TJS 08/03/09
                                                                    XMLCustomer.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 08/03/09
                                                                Else
                                                                    XMLISCustomerIDNode.Value = strISCustomerCode
                                                                End If
                                                            End If
                                                            XMLISImportNode.Add(New XElement("OrderCount", "1"))

                                                        Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                            If strISCustomerCode <> "" Then ' TJS 08/03/09
                                                                XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                                If XMLISCustomerIDNode Is Nothing Then ' TJS 08/03/09
                                                                    XMLCustomer = XMLISImportNode.XPathSelectElement(GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER) ' TJS 08/03/09
                                                                    XMLCustomer.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 08/03/09
                                                                Else
                                                                    XMLISCustomerIDNode.Value = strISCustomerCode
                                                                End If
                                                            End If
                                                            XMLISImportNode.Add(New XElement("InvoiceCount", "1"))

                                                        Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                            If strISCustomerCode <> "" Then ' TJS 08/03/09
                                                                XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                                If XMLISCustomerIDNode Is Nothing Then ' TJS 08/03/09
                                                                    XMLCustomer = XMLISImportNode.XPathSelectElement(GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER) ' TJS 08/03/09
                                                                    XMLCustomer.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 08/03/09
                                                                Else
                                                                    XMLISCustomerIDNode.Value = strISCustomerCode
                                                                End If
                                                            End If
                                                            XMLISImportNode.Add(New XElement("CreditNoteCount", "1"))

                                                    End Select
                                                    XMLSingleImportRecord.Add(XMLISImportNode)

                                                    sTemp = eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH)
                                                    If sTemp <> "" Then
                                                        If sTemp.Substring(sTemp.Length - 1, 1) <> "\" Then
                                                            sTemp = sTemp & "\"
                                                        End If
                                                        Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                            Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                                                XMLSingleImportRecord.Save(sTemp & "GenericXML_for_" & strSourceCode & "_Quote_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, "SourceQuoteRef") & ".xml") ' TJS 30/12/09

                                                            Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                                XMLSingleImportRecord.Save(sTemp & "GenericXML_for_" & strSourceCode & "_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, "SourceOrderRef") & ".xml")

                                                            Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                                XMLSingleImportRecord.Save(sTemp & "GenericXML_for_" & strSourceCode & "_Invoice_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, "SourceInvoiceRef") & ".xml")

                                                            Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                                XMLSingleImportRecord.Save(sTemp & "GenericXML_for_" & strSourceCode & "_CreditNote_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, "SourceCreditNoteRef") & ".xml")

                                                        End Select
                                                    End If

                                                    ' Single Import XML Built now do call to import record (and create customer where customer does not exist except on Credit Notes)
                                                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                        Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                                            XMLFacadeImportResponse = eShopCONNECTFacade.XMLQuoteImport(XMLSingleImportRecord) ' TJS 30/12/09
                                                            XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse") ' TJS 30/12/09
                                                            XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_CUSTOMER_ORDER_REF))) ' TJS 30/12/09
                                                            XMLImportResponseNode.Add(New XElement("SourceQuoteRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF))) ' TJS 30/12/09
                                                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status") ' TJS 30/12/09
                                                            If XMLOrderResponseNode IsNot Nothing Then ' TJS 30/12/09
                                                                If XMLOrderResponseNode.Value = "Success" Then ' TJS 30/12/09
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Quote Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF) & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/QuoteNumber").Value) ' TJS 30/12/09
                                                                Else
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Quote Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF) & " failed to import") ' TJS 30/12/09
                                                                End If
                                                            Else
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Quote Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF) & " failed to import") ' TJS 30/12/09
                                                            End If

                                                        Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                            XMLFacadeImportResponse = eShopCONNECTFacade.XMLOrderImport(XMLSingleImportRecord, "")
                                                            XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                            XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_CUSTOMER_ORDER_REF)))
                                                            XMLImportResponseNode.Add(New XElement("SourceOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF)))
                                                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status") ' TJS 19/03/09
                                                            If XMLOrderResponseNode IsNot Nothing Then ' TJS 19/03/09
                                                                If XMLOrderResponseNode.Value = "Success" Then ' TJS 19/03/09
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF) & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value) ' TJS TJS 19/03/09
                                                                Else
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF) & " failed to import") ' TJS 19/03/09
                                                                End If
                                                            Else
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF) & " failed to import") ' TJS 19/03/09
                                                            End If

                                                        Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                            XMLFacadeImportResponse = eShopCONNECTFacade.XMLInvoiceImport(XMLSingleImportRecord)
                                                            XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                            XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_CUSTOMER_ORDER_REF)))
                                                            XMLImportResponseNode.Add(New XElement("SourceInvoiceRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF)))
                                                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status") ' TJS 19/03/09
                                                            If XMLOrderResponseNode IsNot Nothing Then ' TJS 19/03/09
                                                                If XMLOrderResponseNode.Value = "Success" Then ' TJS 19/03/09
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Invoice Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF) & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/InvoiceNumber").Value) ' TJS 19/03/09
                                                                Else
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Invoice Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF) & " failed to import") ' TJS 19/03/09
                                                                End If
                                                            Else
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Invoice Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF) & " failed to import") ' TJS 19/03/09
                                                            End If

                                                        Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                            XMLFacadeImportResponse = eShopCONNECTFacade.XMLCreditNoteImport(XMLSingleImportRecord)
                                                            XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                            XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_CUSTOMER_ORDER_REF)))
                                                            XMLImportResponseNode.Add(New XElement("SourceCreditNoteRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF)))
                                                            XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status") ' TJS 19/03/09
                                                            If XMLOrderResponseNode IsNot Nothing Then ' TJS 19/03/09
                                                                If XMLOrderResponseNode.Value = "Success" Then ' TJS 19/03/09
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Credit Note Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF) & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/CreditNoteNumber").Value) ' TJS 19/03/09
                                                                Else
                                                                    eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Credit Note Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF) & " failed to import") ' TJS 19/03/09
                                                                End If
                                                            Else
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Credit Note Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF) & " failed to import") ' TJS 19/03/09
                                                            End If

                                                        Case Else
                                                            ' this line added to suppress warning message
                                                            XMLImportResponseNode = Nothing

                                                    End Select

                                                    XMLeShopCONNECTNode.Add(XMLImportResponseNode)

                                                    XMLSingleImportRecord = Nothing
                                                Else
                                                    XMLImportResponseNode = New XElement("ImportResponse")
                                                    XMLImportResponseNode.Add(New XElement("Status", "Error"))
                                                    XMLImportResponseNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_CUSTOMER_ORDER_REF)))
                                                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                        Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                                            XMLImportResponseNode.Add(New XElement("SourceQuoteRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF))) ' TJS 30/12/09

                                                        Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                            XMLImportResponseNode.Add(New XElement("SourceOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF)))

                                                        Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                            XMLImportResponseNode.Add(New XElement("SourceInvoiceRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF)))

                                                        Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                            XMLImportResponseNode.Add(New XElement("SourceCreditNoteRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF)))

                                                    End Select
                                                    XMLImportResponseNode.Add(New XElement("ErrorCode", "002"))
                                                    XMLImportResponseNode.Add(New XElement("ErrorMessage", "Missing or invalid customer code/id"))
                                                    eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import", XMLImportResponseNode.ToString, eShopCONNECTFacade.ConvertXMLFromWeb(context.Request.QueryString("data").ToString))

                                                    XMLeShopCONNECTNode.Add(XMLImportResponseNode)
                                                End If

                                            Else
                                                ' start of code added TJS 24/07/09
                                                XMLISImportNode.Add(XMLImportRecordNode)
                                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                    Case GENERIC_XML_LEAD_IMPORT_TYPE
                                                        XMLISImportNode.Add(New XElement("LeadCount", "1"))

                                                    Case GENERIC_XML_PROSPECT_IMPORT_TYPE
                                                        XMLISImportNode.Add(New XElement("ProspectCount", "1"))

                                                    Case GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                                        XMLISImportNode.Add(New XElement("CustomerCount", "1"))

                                                End Select
                                                XMLSingleImportRecord.Add(XMLISImportNode)

                                                sTemp = eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH)
                                                If sTemp <> "" Then
                                                    If sTemp.Substring(sTemp.Length - 1, 1) <> "\" Then
                                                        sTemp = sTemp & "\"
                                                    End If
                                                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                        Case GENERIC_XML_LEAD_IMPORT_TYPE
                                                            XMLSingleImportRecord.Save(sTemp & "GenericXML_for_" & strSourceCode & "_Lead_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_LEAD_SOURCE_LEAD_ID) & ".xml")

                                                        Case GENERIC_XML_PROSPECT_IMPORT_TYPE
                                                            XMLSingleImportRecord.Save(sTemp & "GenericXML_for_" & strSourceCode & "_Prospect_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_PROSPECT_SOURCE_PROSPECT_ID) & ".xml")

                                                        Case GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                                            XMLSingleImportRecord.Save(sTemp & "GenericXML_for_" & strSourceCode & "_Customer_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CUSTOMER_SOURCE_CUSTOMER_ID) & ".xml")

                                                    End Select
                                                End If

                                                ' Single Import XML Built now do call to import record 
                                                Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                    Case GENERIC_XML_LEAD_IMPORT_TYPE
                                                        XMLFacadeImportResponse = eShopCONNECTFacade.XMLLeadImport(XMLSingleImportRecord)
                                                        XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                        XMLImportResponseNode.Add(New XElement("SourceLeadID", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_LEAD_SOURCE_LEAD_ID)))
                                                        XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                                                        If XMLOrderResponseNode IsNot Nothing Then
                                                            If XMLOrderResponseNode.Value = "Success" Then
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Lead ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_LEAD_SOURCE_LEAD_ID) & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/LeadCode").Value)
                                                            Else
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Lead ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_LEAD_SOURCE_LEAD_ID) & " failed to import")
                                                            End If
                                                        Else
                                                            eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Lead ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_LEAD_SOURCE_LEAD_ID) & " failed to import")
                                                        End If

                                                    Case GENERIC_XML_PROSPECT_IMPORT_TYPE
                                                        XMLFacadeImportResponse = eShopCONNECTFacade.XMLCustomerProspectImport(XMLSingleImportRecord, True)
                                                        XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                        XMLImportResponseNode.Add(New XElement("SourceProspectID", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_PROSPECT_SOURCE_PROSPECT_ID)))
                                                        XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                                                        If XMLOrderResponseNode IsNot Nothing Then
                                                            If XMLOrderResponseNode.Value = "Success" Then
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Prospect ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_PROSPECT_SOURCE_PROSPECT_ID) & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/ProspectCode").Value) ' TJS 24/07/09
                                                            Else
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Prospect ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_PROSPECT_SOURCE_PROSPECT_ID) & " failed to import")
                                                            End If
                                                        Else
                                                            eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Prospect ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_PROSPECT_SOURCE_PROSPECT_ID) & " failed to import")
                                                        End If

                                                    Case GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                                        XMLFacadeImportResponse = eShopCONNECTFacade.XMLCustomerProspectImport(XMLSingleImportRecord, False)
                                                        XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                        XMLImportResponseNode.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CUSTOMER_SOURCE_CUSTOMER_ID)))
                                                        XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status")
                                                        If XMLOrderResponseNode IsNot Nothing Then
                                                            If XMLOrderResponseNode.Value = "Success" Then
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Customer ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CUSTOMER_SOURCE_CUSTOMER_ID) & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/CustomerCode").Value)
                                                            Else
                                                                eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Customer ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CUSTOMER_SOURCE_CUSTOMER_ID) & " failed to import")
                                                            End If
                                                        Else
                                                            eShopCONNECTFacade.WriteLogProgressRecord("Generic XML Web Import Source Customer ID " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CUSTOMER_SOURCE_CUSTOMER_ID) & " failed to import")
                                                        End If

                                                    Case Else
                                                        ' this line added to suppress warning message
                                                        XMLImportResponseNode = Nothing

                                                End Select

                                                XMLeShopCONNECTNode.Add(XMLImportResponseNode)

                                                XMLSingleImportRecord = Nothing
                                                ' end of code added TJS 24/07/09

                                            End If
                                        End If
                                    Next

                                ElseIf bImportTypeValid Then ' TJS 08/03/09
                                    XMLImportResponseNode = New XElement("ImportResponse") ' TJS 08/03/09
                                    XMLImportResponseNode.Add(New XElement("Status", "Error")) ' TJS 08/03/09
                                    XMLImportResponseNode.Add(New XElement("ErrorCode", "003")) ' TJS 08/03/09
                                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                        Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 30/12/09
                                            XMLImportResponseNode.Add(New XElement("ErrorMessage", "Input XML does not contain any quotes")) ' TJS 30/12/09

                                        Case GENERIC_XML_ORDER_IMPORT_TYPE
                                            XMLImportResponseNode.Add(New XElement("ErrorMessage", "Input XML does not contain any orders")) ' TJS 08/03/09

                                        Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                            XMLImportResponseNode.Add(New XElement("ErrorMessage", "Input XML does not contain any invoices")) ' TJS 08/03/09

                                        Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                            XMLImportResponseNode.Add(New XElement("ErrorMessage", "Input XML does not contain any credit notes")) ' TJS 08/03/09

                                    End Select
                                    eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import", XMLImportResponseNode.ToString, strInputXML) ' TJS 08/03/09
                                    XMLeShopCONNECTNode.Add(XMLImportResponseNode) ' TJS 08/03/09

                                End If

                                context.Response.Write("<?xml version=""1.0"" encoding=""UTF-8""?>" & XMLeShopCONNECTNode.ToString)
                            Else
                                strErrCode = "002 - source activation or validation error"
                            End If
                        End If
                    Else
                        strErrCode = "001 - missing or invalid XML import data"
                    End If
                Else
                    strErrCode = "001 - missing or invalid XML import data"
                End If

                If strErrCode <> "" Then
                    If Left(UCase(context.Request.ServerVariables("SERVER_NAME")), 5) <> "LERHQ" And Left(UCase(context.Request.ServerVariables("SERVER_NAME")), 6) <> "LERRYN" And _
                        UCase(context.Request.ServerVariables("SERVER_NAME")) <> "LOCALHOST" Then
                        strError = context.Request.ServerVariables("SERVER_NAME") & ", GenericXMLImport.ashx - Error Code " & strErrCode
                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import", strError, eShopCONNECTFacade.ConvertXMLFromWeb(context.Request.Form.ToString))
                    Else
                        context.Response.Write(strErrCode & " - " & eShopCONNECTFacade.ConvertXMLFromWeb(context.Request.Form.ToString)) ' TJS 25/08/09
                    End If
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
                eShopCONNECTFacade.SendErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic Order Import Error", ex, strInputXML)
                context.Response.Write(ex.Message) ' TJS 08/03/09

            End Try

        Catch ex As Exception ' TJS 11/02/09

            Try
                Dim objEventLog As New EventLog() ' TJS 11/02/09

                objEventLog.Source = "eShopCONNECT" ' TJS 11/02/09 TJS 23/08/09

                objEventLog.WriteEntry("ProcessGenericXMLRequest - " & ex.Message, EventLogEntryType.Error) ' TJS 11/02/09 TJS 23/08/09
                ' start of code added TJS 23/08/09
                XMLImportResponseNode = New XElement("StatusResponse")
                XMLImportResponseNode.Add(New XElement("Status", "Error"))
                XMLImportResponseNode.Add(New XElement("ErrorCode", "999"))
                XMLImportResponseNode.Add(New XElement("ErrorMessage", "ProcessGenericXMLRequest - " & ex.Message))
                ' end of code added TJS 23/08/09
                context.Response.Write(XMLImportResponseNode.ToString) ' TJS 08/03/09 TJS 23/08/09

            Catch ex2 As Exception
                context.Response.Write(ex2.Message & vbCrLf & ex2.StackTrace) ' TJS 11/02/09 TJS 02/04/12

            End Try

        End Try
    End Sub

End Module
