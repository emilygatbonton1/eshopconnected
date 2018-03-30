' eShopCONNECT for Connected Business
' Module: GenericImportStatusCode.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
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

Public Module GenericImportStatusCode

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade

    Public Sub ProcessGenericImportStatusRequest(ByVal context As HttpContext)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 11/02/09 | TJS             | 2009.1.07 | Added error trapping to catch DB login errors etc
        '                                        | corrected Read procedure names and modified to use constants
        '                                        | and fixed other errors
        ' 06/03/09 | TJS             | 2009.1.09 | Modified to remove case sensivity in XML header
        ' 25/03/09 | TJS             | 2009.1.11 | Added logout for SP5x
        ' 29/05/09 | TJS             | 2009.2.09 | Added checks for XML load errors
        ' 14/08/09 | TJS             | 2009.3.03 | Removed ConvertXMLFromWeb as caused unwanted conversions
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use EventLog source created during install as IIS 
        '                                        | generally doesn't have permissions to create an event source
        ' 25/08/09 | TJS             | 2009.3.05 | Tidied error response message
        ' 06/10/09 | TJS             | 2009.3.07 | Modified to cater for reprocessing records
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and to use InputStream
        '                                        |  instead of Form to get input XML without spaces being changed to +.  Also modified
        '                                        | to set LastWebPost_DEV000221 so config form can check if web service is installed and active
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLRequest As XDocument, XMLTemp As XDocument, XMLImportResponseNode As XElement, XMLStatusRequestNode As XElement
        Dim XMLStatusRequestList As System.Collections.Generic.IEnumerable(Of XElement), reader As StreamReader ' TJS 02/12/11 
        Dim strInputXML As String, strErrCode As String, strSourceCode As String, strError As String
        Dim strTemp As String, Params() As String
        Dim ParamCount As Int16, bRecordTypeValid As Boolean, bParametersOK As Boolean, bRecordFound As Boolean ' TJS 12/02/09

        Try

            context.Response.ContentType = "text/xml"
            strErrCode = ""
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12

            Try

                'Check the file is at least 40 chars (This prevents errors when checking for the xml version and encoding)
                If context.Request.Form.ToString.Length > 40 Then
                    ' Read XML 
                    reader = New StreamReader(context.Request.InputStream()) ' TJS 02/12/11 
                    strInputXML = eShopCONNECTFacade.ConvertXMLFromWeb(reader.ReadToEnd()).Replace("%20", " ") ' TJS 02/12/11 TJS 19/04/12
                    ' check XML starts with correct headers
                    If Left(Trim(strInputXML.ToLower), 38) = "<?xml version=""1.0"" encoding=""utf-8""?>" Then ' TJS 06/03/09
                        ' yes, load into XML cocument
                        Try
                            XMLRequest = XDocument.Parse(Trim(strInputXML))
                            ' check module and connector are activated and valid
                            strSourceCode = eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_CODE)
                            If eShopCONNECTFacade.ValidateSource("GenericXMLImport.ashx", strSourceCode, _
                                eShopCONNECTFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_AUTHENTICATION), False) Then ' TJS 06/10/09

                                XMLStatusRequestList = XMLRequest.XPathSelectElements(GENERIC_XML_STATUS_LIST)

                                For Each XMLStatusRequestNode In XMLStatusRequestList
                                    Try
                                        XMLTemp = XDocument.Parse(XMLStatusRequestNode.ToString)
                                        'process this import record
                                        bRecordTypeValid = True
                                        bParametersOK = True
                                        bRecordFound = False ' TJS 11/02/09
                                        XMLImportResponseNode = New XElement("StatusResponse")
                                        XMLImportResponseNode.Add("RecordType", eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_RECORD_TYPE))
                                        Select Case eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_RECORD_TYPE).ToLower ' TJS 11/02/09
                                            Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                ParamCount = -1
                                                ReDim Params(0)
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_CUSTOMER_ORDER_REF)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_PO_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("CustomerOrderRef", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_ORDER_REF)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_MERCHANT_ORDER_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("SourceOrderRef", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_WEBSITE_REF)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_WEBSITE_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("SourceWebSiteRef", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_MERCHANT_ID) ' TJS 11/02/09
                                                If strTemp <> "" Then ' TJS 11/02/09
                                                    ParamCount = ParamCount + 2 ' TJS 11/02/09
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_STORE_MERCHANT_ID ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp ' TJS 11/02/09
                                                    XMLImportResponseNode.Add("SourceMerchantID", strTemp) ' TJS 11/02/09
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_CUSTOMER_ID)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 4
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 3) = AT_SOURCE_CUSTOMER_ID ' TJS 11/02/09
                                                    Params(ParamCount - 2) = strTemp
                                                    Params(ParamCount - 1) = AT_SOURCE_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strSourceCode
                                                    XMLImportResponseNode.Add("SourceCustomerID", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_IS_CUSTOMER_CODE)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_CUSTOMER_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("ISCustomerCode", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_IS_RECORD_CODE)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_SALES_ORDER_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("ISRecordCode", strTemp)
                                                End If
                                                Select Case ParamCount
                                                    Case 1
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1)}}, _
                                                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 3
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3)}}, _
                                                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 5
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 7
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 9
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 11
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9), Params(10), Params(11)}}, _
                                                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 13
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9), Params(10), Params(11), _
                                                            Params(12), Params(13)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 15 ' TJS 11/02/09
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerSalesOrderImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9), Params(10), Params(11), _
                                                            Params(12), Params(13), Params(14), Params(15)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case Else
                                                        XMLImportResponseNode.Add("Status", "Error")
                                                        XMLImportResponseNode.Add("ErrorCode", "121")
                                                        XMLImportResponseNode.Add("ErrorMessage", "Invalid Status parameter record count " & ParamCount)
                                                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import Status", XMLImportResponseNode.ToString, strInputXML) ' TJS 14/08/09
                                                        bParametersOK = False

                                                End Select
                                                If eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.Count > 0 And bParametersOK Then
                                                    XMLImportResponseNode.Add("Status", "Order at " & eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221(0).Stage & " stage")

                                                ElseIf eShopCONNECTDatasetGateway.CustomerSalesOrderImportExportStatusView_DEV000221.Count = 0 Then
                                                    XMLImportResponseNode.Add("Status", "Error")
                                                    XMLImportResponseNode.Add("ErrorCode", "122")
                                                    XMLImportResponseNode.Add("ErrorMessage", "Import record not found")

                                                End If

                                            Case GENERIC_XML_INVOICE_IMPORT_TYPE, GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                ParamCount = 1
                                                ReDim Params(ParamCount)
                                                If eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_RECORD_TYPE) = GENERIC_XML_INVOICE_IMPORT_TYPE Then
                                                    Params(ParamCount - 1) = AT_TYPE ' TJS 11/02/09
                                                    Params(ParamCount) = "Invoice"
                                                Else
                                                    Params(ParamCount - 1) = AT_TYPE ' TJS 11/02/09
                                                    Params(ParamCount) = "Credit Memo"
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_CUSTOMER_ORDER_REF)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_PO_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("CustomerOrderRef", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_ORDER_REF)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_MERCHANT_ORDER_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("SourceOrderRef", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_WEBSITE_REF)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_WEBSITE_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("SourceWebSiteRef", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_MERCHANT_ID) ' TJS 11/02/09
                                                If strTemp <> "" Then ' TJS 11/02/09
                                                    ParamCount = ParamCount + 2 ' TJS 11/02/09
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_STORE_MERCHANT_ID ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp ' TJS 11/02/09
                                                    XMLImportResponseNode.Add("SourceMerchantID", strTemp) ' TJS 11/02/09
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_SOURCE_CUSTOMER_ID)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 4
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 3) = AT_SOURCE_CUSTOMER_ID ' TJS 11/02/09
                                                    Params(ParamCount - 2) = strTemp
                                                    Params(ParamCount - 1) = AT_SOURCE_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strSourceCode
                                                    XMLImportResponseNode.Add("SourceCustomerID", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_IS_CUSTOMER_CODE)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_CUSTOMER_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("ISCustomerCode", strTemp)
                                                End If
                                                strTemp = eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_IS_RECORD_CODE)
                                                If strTemp <> "" Then
                                                    ParamCount = ParamCount + 2
                                                    ReDim Preserve Params(ParamCount) ' TJS 11/02/09
                                                    Params(ParamCount - 1) = AT_INVOICE_CODE ' TJS 11/02/09
                                                    Params(ParamCount) = strTemp
                                                    XMLImportResponseNode.Add("ISRecordCode", strTemp)
                                                End If
                                                Select Case ParamCount
                                                    Case 1
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1)}}, _
                                                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 3
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3)}}, _
                                                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 5
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 7
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 9
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 11
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9), Params(10), Params(11)}}, _
                                                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 13
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9), Params(10), Params(11), _
                                                            Params(12), Params(13)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 15
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9), Params(10), Params(11), _
                                                            Params(12), Params(13), Params(14), Params(15)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case 17 ' TJS 11/02/09
                                                        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.TableName, _
                                                            "ReadCustomerInvoiceImportExportStatusView_DEV000221", Params(0), Params(1), Params(2), Params(3), _
                                                            Params(4), Params(5), Params(6), Params(7), Params(8), Params(9), Params(10), Params(11), _
                                                            Params(12), Params(13), Params(14), Params(15), Params(16), Params(17)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/02/09

                                                    Case Else
                                                        XMLImportResponseNode.Add("Status", "Error")
                                                        XMLImportResponseNode.Add("ErrorCode", "121")
                                                        XMLImportResponseNode.Add("ErrorMessage", "Invalid Status parameter record count " & ParamCount)
                                                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import Status", XMLImportResponseNode.ToString, strInputXML) ' TJS 14/08/09
                                                        bParametersOK = False

                                                End Select
                                                If eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.Count > 0 And bParametersOK Then
                                                    If eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221(0).IsPosted Then
                                                        XMLImportResponseNode.Add("Status", "Posted")
                                                    Else
                                                        XMLImportResponseNode.Add("Status", "Imported")
                                                    End If

                                                ElseIf eShopCONNECTDatasetGateway.CustomerInvoiceImportExportStatusView_DEV000221.Count = 0 Then
                                                    XMLImportResponseNode.Add("Status", "Error")
                                                    XMLImportResponseNode.Add("ErrorCode", "122")
                                                    XMLImportResponseNode.Add("ErrorMessage", "Import record not found")

                                                End If

                                            Case Else
                                                XMLImportResponseNode = New XElement("StatusResponse")
                                                XMLImportResponseNode.Add("Status", "Error")
                                                XMLImportResponseNode.Add("ErrorCode", "120")
                                                XMLImportResponseNode.Add("ErrorMessage", "Unknown Status Record Type " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, GENERIC_XML_STATUS_RECORD_TYPE))
                                                eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import Status", XMLImportResponseNode.ToString, strInputXML) ' TJS 14/08/09

                                                bRecordTypeValid = False
                                        End Select

                                    Catch ex As Exception
                                        ' start of code added TJS 29/05/09
                                        XMLImportResponseNode = New XElement("StatusResponse")
                                        XMLImportResponseNode.Add("Status", "Error")
                                        XMLImportResponseNode.Add("ErrorCode", "005")
                                        XMLImportResponseNode.Add("ErrorMessage", "Invalid XML - " & ex.Message.Replace(vbCrLf, ""))
                                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import Status", XMLImportResponseNode.ToString, strInputXML) ' TJS 14/08/09
                                        ' end of code added TJS 29/05/09

                                    End Try
                                Next

                                context.Response.Write("<?xml version=""1.0"" encoding=""UTF-8""?>" & XMLImportResponseNode.ToString)
                            Else
                                strErrCode = "002 - source activation or validation error"
                            End If

                        Catch ex As Exception
                            strErrCode = "005 - Input processing failed due to XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & strInputXML ' TJS 29/05/09

                        End Try
                    Else
                        strErrCode = "001 - missing or invalid XML data"
                    End If
                Else
                    strErrCode = "001 - missing or invalid XML data"
                End If

                If strErrCode <> "" Then
                    If Left(UCase(context.Request.ServerVariables("SERVER_NAME")), 5) <> "LERHQ" And Left(UCase(context.Request.ServerVariables("SERVER_NAME")), 6) <> "LERRYN" And _
                        UCase(context.Request.ServerVariables("SERVER_NAME")) <> "LOCALHOST" Then
                        strError = context.Request.ServerVariables("SERVER_NAME") & ", GenericOrderStatus.ashx - Error Code " & strErrCode
                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "Generic XML Web Import Status", strError, context.Request.Form.ToString) ' TJS 14/08/09
                    Else
                        context.Response.Write(strErrCode & " - " & eShopCONNECTFacade.ConvertXMLFromWeb(context.Request.Form.ToString)) ' TJS 14/08/09 TJS 25/08/09
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
                eShopCONNECTFacade.SendErrorEmail(eShopCONNECTFacade.SourceConfig, "Shop.com Order Import Error", ex, context.Request.Form.ToString) ' TJS 11/02/09 TJS 14/08/09

            End Try

        Catch ex As Exception ' TJS 11/02/09

            Try
                Dim objEventLog As New EventLog() ' TJS 11/02/09

                objEventLog.Source = "eShopCONNECT" ' TJS 11/02/09 TJS 23/08/09

                objEventLog.WriteEntry("ProcessGenericImportStatusRequest - " & ex.Message, EventLogEntryType.Error) ' TJS 11/02/09 TJS 23/08/09
                ' start of code added TJS 23/08/09
                XMLImportResponseNode = New XElement("StatusResponse")
                XMLImportResponseNode.Add("Status", "Error")
                XMLImportResponseNode.Add("ErrorCode", "999")
                XMLImportResponseNode.Add("ErrorMessage", "ProcessGenericImportStatusRequest - " & ex.Message)
                context.Response.Write(XMLImportResponseNode.ToString)
                ' end of code added TJS 23/08/09

            Catch ex2 As Exception
                context.Response.Write(ex2.Message) ' TJS 11/02/09

            End Try

        End Try
    End Sub

End Module
