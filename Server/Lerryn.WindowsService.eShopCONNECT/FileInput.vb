' eShopCONNECT for Connected Business - Windows Service
' Module: FileInput.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 13 November 2013

Imports System
Imports System.Text
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module FileInput

#Region "File Input Routines"

    Private ImportExportProcessFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade ' TJS 27/11/09

    Public Function DoesRecivedFileAlreadyExist(ByVal sFileName As String, ByVal sFolderToCheck As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.   | Description
        '------------------------------------------------------------------------------------------
        ' 21/10/08 | Craig Griffin   | 1.0.0.0 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Return My.Computer.FileSystem.FileExists(sFolderToCheck & "/" & sFileName)

    End Function

    Public Function PollForFileInput() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/01/09 | TJS             | 2009.1.01 | Modified to check all sources
        ' 28/01/09 | TJS             | 2009.1.02 | Modified to pass File name to ProcessXML and handle import failures
        ' 30/01/09 | TJS             | 2009.1.03 | Modified to add copy(x) to file name when duplicate files encountered
        ' 22/02/09 | TJS             | 2009.1.08 | Modified to cater for Amazon connector
        ' 16/03/09 | TJS             | 2009.1.10 | Modified to check if amazon poll active and cater for FileImport
        ' 03/04/09 | TJS             | 2009.2.00 | Modified to correct copy file names on duplicate files
        ' 16/10/09 | TJS             | 2009.3.08 | Modified to check that input directory exists
        ' 27/11/09 | TJS             | 2009.3.09 | Modified to remove Amazon file input from AMTU as replaced by direct connection to Amazon
        '                                        | and removed redundant variables
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to check activation in case it expired while service running 
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for QuoteToConvert parameter on XMLOrderImport
        '                                        | and for Source Code constants
        ' 18/10/10 | TJS             | 2010.1.06 | Corrected File_Import and Prospect_Import constant name
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for Connector Source Code property
        ' 26/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Reader As StreamReader, DirectoryInfo As DirectoryInfo, ActiveSource As SourceSettings
        Dim file As FileInfo

        Dim Line As String, Filename As String, sXML As String, sCopyFileName As String, iCopyFileNum As Integer ' TJS 30/01/09
        Dim iDotPosn As Integer, bAllComplete As Boolean, bPollActive As Boolean ' TJS 30/01/09 TJS 22/02/09 TJS 27/11/09

        dtFileReadDueAt = dtFileReadDueAt.AddMinutes(5)
        bAllComplete = True ' TJS 28/01/09

        Try
            bPollActive = False
            ' are there any sources with active file input ?
            For Each ActiveSource In Setts.ActiveSources ' TJS 28/01/09
                ' is polling enabled for source ?
                If ActiveSource.EnablePollGenericImportPath Then ' TJS 28/01/09
                    ' yes, need file input poll
                    bPollActive = True
                    Exit For
                End If
            Next
            If bPollActive Then
                m_ImportExportConfigFacade.WriteLogProgressRecord("File Input routine starting.") ' TJS 28/01/09 TJS 22/02/09
                For Each ActiveSource In Setts.ActiveSources ' TJS 20/01/09
                    If (ActiveSource.SourceCode = GENERIC_IMPORT_SOURCE_CODE Or (ActiveSource.SourceCode = FILE_IMPORT_SOURCE_CODE And _
                        PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE) Or (ActiveSource.SourceInputHandler = "Windows Service" And _
                        PRODUCT_CODE = ORDERIMPORTER_BASE_PRODUCT_CODE)) And m_ImportExportConfigFacade.IsActivated Then ' TJS 22/02/09 TJS 17/03/09 TJS 13/04/10 TJS 19/08/10 TJS/FA 18/10/10 TJS 18/03/11
                        If ActiveSource.EnablePollGenericImportPath Then ' TJS 20/01/09
                            'Loop through file input folder looking for orders to process
                            DirectoryInfo = New DirectoryInfo(ActiveSource.GenericImportPath) ' TJS 20/01/09
                            If DirectoryInfo.Exists Then ' TJS 16/10/09
                                For Each file In DirectoryInfo.GetFiles
                                    'Files found that need to be input
                                    Filename = file.Name
                                    'First check if the file already exists in the processed folder
                                    If Not DoesRecivedFileAlreadyExist(Filename, ActiveSource.GenericImportProcessedPath) Then
                                        If Microsoft.VisualBasic.Right(Filename, 3) = "xml" Then
                                            ' Create an instance of StreamReader to read from a file.
                                            Reader = New StreamReader(ActiveSource.GenericImportPath & Filename) ' TJS 20/01/09
                                            If IsNothing(Reader) Then
                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "FileInput", "File " & Filename & " cannot be opened.") ' TJS 28/01/09 TJS 13/11/13
                                                Return False
                                            End If

                                            ' Read and display the lines from the file until the end 
                                            ' of the file is reached.
                                            Line = Reader.ReadToEnd

                                            ' did we get data ?
                                            If Not Line Is Nothing Then
                                                ' yes, process import record
                                                sXML = Line.ToString
                                                If ProcessGenericXML(ActiveSource.XMLConfig, sXML, Filename) Then ' TJS 28/01/09 TJS 13/0209
                                                    Reader.Close()
                                                    If DoesRecivedFileAlreadyExist(Filename, ActiveSource.GenericImportProcessedPath) Then ' TJS 20/01/09
                                                        My.Computer.FileSystem.DeleteFile(ActiveSource.GenericImportProcessedPath & Filename) ' TJS 20/01/09
                                                    End If
                                                    My.Computer.FileSystem.MoveFile(ActiveSource.GenericImportPath & Filename, ActiveSource.GenericImportProcessedPath & Filename) ' TJS 20/01/09
                                                Else
                                                    Reader.Close() ' TJS 28/01/09
                                                    If DoesRecivedFileAlreadyExist(Filename, ActiveSource.GenericImportErrorPath) Then ' TJS 28/01/09
                                                        My.Computer.FileSystem.DeleteFile(ActiveSource.GenericImportErrorPath & Filename) ' TJS 28/01/09
                                                    End If
                                                    My.Computer.FileSystem.MoveFile(ActiveSource.GenericImportPath & Filename, ActiveSource.GenericImportErrorPath & Filename) ' TJS 28/01/09
                                                    bAllComplete = False ' TJS 28/01/09
                                                End If
                                            Else
                                                Reader.Close()
                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "FileInput", "File " & file.Name & " contains no data.") ' TJS 28/01/09 TJS 13/11/13
                                                If DoesRecivedFileAlreadyExist(Filename, ActiveSource.GenericImportErrorPath) Then ' TJS 20/01/09
                                                    My.Computer.FileSystem.DeleteFile(ActiveSource.GenericImportErrorPath & Filename) ' TJS 20/01/09
                                                End If
                                                My.Computer.FileSystem.MoveFile(ActiveSource.GenericImportPath & Filename, ActiveSource.GenericImportErrorPath & Filename) ' TJS 20/01/09
                                                bAllComplete = False ' TJS 28/01/09
                                            End If
                                        Else
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "FileInput", "File " & Filename & " has been ignored by " & PRODUCT_NAME & " as it is not an xml file.") ' TJS 28/01/09 TJS 13/11/13
                                            If DoesRecivedFileAlreadyExist(Filename, ActiveSource.GenericImportErrorPath) Then ' TJS 20/01/09
                                                My.Computer.FileSystem.DeleteFile(ActiveSource.GenericImportErrorPath & Filename) ' TJS 20/01/09
                                            End If
                                            My.Computer.FileSystem.MoveFile(ActiveSource.GenericImportPath & Filename, ActiveSource.GenericImportErrorPath & Filename) ' TJS 20/01/09
                                            bAllComplete = False ' TJS 28/01/09
                                        End If

                                    Else
                                        'The file already exists, reject the file along with reason
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "FileInput", "Duplicate file (" & Filename & ") received, New File rejected.") ' TJS 28/01/09 TJS 13/11/13
                                        ' does a file with the same name already exist in the error directory ?
                                        If DoesRecivedFileAlreadyExist(Filename, ActiveSource.GenericImportErrorPath) Then ' TJS 30/01/09
                                            ' yes, rename it as Copy(x)
                                            iCopyFileNum = 1 ' TJS 30/01/09
                                            iDotPosn = InStrRev(Filename, ".") ' TJS 30/01/09
                                            If iDotPosn >= 0 Then ' TJS 30/01/09
                                                sCopyFileName = Left(Filename, iDotPosn - 1) & " Copy(" & iCopyFileNum & ")" & Right(Filename, Len(Filename) - (iDotPosn - 1)) ' TJS 30/01/09 TJS 03/04/09
                                            Else
                                                sCopyFileName = Filename & " Copy(" & iCopyFileNum & ")" ' TJS 30/01/09
                                            End If
                                            Do While DoesRecivedFileAlreadyExist(sCopyFileName, ActiveSource.GenericImportErrorPath) ' TJS 30/01/09
                                                iCopyFileNum = iCopyFileNum + 1 ' TJS 30/01/09
                                                If iDotPosn >= 0 Then ' TJS 30/01/09
                                                    sCopyFileName = Left(Filename, iDotPosn - 1) & " Copy(" & iCopyFileNum & ")" & Right(Filename, Len(Filename) - (iDotPosn - 1)) ' TJS 30/01/09 TJS 03/04/09
                                                Else
                                                    sCopyFileName = Filename & " Copy(" & iCopyFileNum & ")" ' TJS 30/01/09
                                                End If
                                            Loop ' TJS 30/01/09
                                            My.Computer.FileSystem.MoveFile(ActiveSource.GenericImportPath & Filename, ActiveSource.GenericImportErrorPath & sCopyFileName) ' TJS 30/01/09
                                        Else
                                            My.Computer.FileSystem.MoveFile(ActiveSource.GenericImportPath & Filename, ActiveSource.GenericImportErrorPath & Filename) ' TJS 20/01/09 
                                        End If
                                        bAllComplete = False ' TJS 28/01/09
                                    End If
                                    If bShutDownInProgress Then ' TJS 02/08/13
                                        Exit For ' TJS 02/08/13
                                    End If
                                Next
                            End If
                        End If

                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
            End If
            Return bAllComplete

        Catch Ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "PollForFileInput", Ex) ' TJS 28/01/09
            Return False
        End Try

    End Function

    Public Function ProcessGenericXML(ByVal SourceConfig As XDocument, ByVal strInputXML As String, ByVal strFileName As String) As Boolean ' TJS 28/01/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 28/01/09 | TJS             | 2009.1.02 | Modified to allow blank Source and IS Customer IDs if config allows
        '                                        | and corrected logging of errors etc
        ' 06/02/09 | TJS             | 2009.1.05 | Corrected calls to SendSourceErrorEmail
        ' 22/02/09 | TJS             | 2009.1.08 | Renamed to avoid confusion with Amazon import etc
        ' 03/04/09 | TJS             | 2009.1.11 | Added logout for SP5x
        ' 06/05/09 | TJS             | 2009.2.05 | Modified to cater for ISCustomerCode not being present
        ' 11/05/09 | TJS             | 2009.2.06 | Modified to log each import record in turn for multi record files
        ' 20/05/09 | TJS             | 2009.2.08 | Corrected source xml passed to error logging if no supplier id provided
        ' 29/05/09 | TJS             | 2009.2.09 | Moved from General.vb and added checks for XML load errors
        ' 14/08/09 | TJS             | 2009.3.03 | Removed ConvertXMLFromWeb as caused unwanted conversions
        ' 06/10/09 | TJS             | 2009.3.07 | Modified to cater for reprocessing records
        ' 05/01/10 | TJS             | 2010.0.01 | Modified to cater for lead, prospect and customer import
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLRequest As XDocument, XMLSingleImportRecord As XDocument, XMLTemp As XDocument
        Dim XMLImportRecordList As System.Collections.Generic.IEnumerable(Of XElement), XMLImportRecordNode As XElement
        Dim XMLSourceNode As XElement, XMLISImportNode As XElement, XMLFacadeImportResponse As XElement
        Dim XMLISCustomerIDNode As XElement, XMLImportResponseNode As XElement
        Dim strErrCode As String, strSourceCustomerID As String
        Dim strISCustomerCode As String, strSourceCode As String, sTemp As String = ""
        Dim bImportTypeValid As Boolean, bCustomerValid As Boolean, bReturnValue As Boolean ' TJS 28/01/09
        Dim strBaseSourceHandler As String

        Try
            strErrCode = ""
            bReturnValue = True ' TJS 28/01/09

            ImportExportProcessFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(m_ImportExportDataset, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            'Check the file is at least 40 chars (This prevents errors when checking for the xml version and encoding)
            If strInputXML.Length > 40 Then
                ' Read XML 
                ' check XML starts with correct headers
                If Strings.Left(strInputXML.ToString, 14) = "<?xml version=" Then
                    ' yes, load into XML cocument
                    Try
                        ' did input XML load correctly
                        XMLRequest = XDocument.Parse(Trim(strInputXML))
                        ' yes, check module and connector are activated and valid
                        strSourceCode = ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_CODE)
                        If PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJE 26/01/09 
                            strBaseSourceHandler = "GenericXMLImport.ashx"
                        Else
                            strBaseSourceHandler = "Windows Service" ' TJE 26/01/09
                        End If
                        If ImportExportProcessFacade.ValidateSource(strBaseSourceHandler, strSourceCode, _
                            ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_AUTHENTICATION), False) Then ' TJS 06/10/09

                            bImportTypeValid = False
                            Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 05/01/10
                                    XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_QUOTE_LIST) ' TJS 05/01/10
                                    bImportTypeValid = True ' TJS 05/01/10

                                Case GENERIC_XML_ORDER_IMPORT_TYPE
                                    XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_ORDER_LIST)
                                    bImportTypeValid = True

                                Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                    XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_INVOICE_LIST)
                                    bImportTypeValid = True

                                Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                    XMLImportRecordList = XMLRequest.XPathSelectElements(GENERIC_XML_CREDITNOTE_LIST)
                                    bImportTypeValid = True

                                    ' start of code added TJS 05/01/10
                                Case GENERIC_XML_LEAD_IMPORT_TYPE, GENERIC_XML_PROSPECT_IMPORT_TYPE, GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                    If ImportExportProcessFacade.ConfigFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                                        Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
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
                                        XMLImportResponseNode.Add(New XElement("ErrorMessage", "Invalid Import Type " & ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE)))
                                        ImportExportProcessFacade.SendSourceErrorEmail(SourceConfig, "Generic XML Web Import", XMLImportResponseNode.ToString, strInputXML)
                                    End If
                                    ' end of code added TJS 05/01/10

                                Case Else
                                    XMLImportRecordList = Nothing
                                    XMLImportResponseNode = New XElement("ImportResponse")
                                    XMLImportResponseNode.Add(New XElement("Status", "Error"))
                                    XMLImportResponseNode.Add(New XElement("ErrorCode", "001"))
                                    XMLImportResponseNode.Add(New XElement("ErrorMessage", "Unknown Import Type " & ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE)))
                                    ImportExportProcessFacade.SendSourceErrorEmail(SourceConfig, "Generic XML Import", XMLImportResponseNode.ToString, strInputXML) ' TJS 06/02/09 TJS 14/08/09

                                    bReturnValue = False ' TJS 28/01/09

                            End Select
                            ' is import type valid and there are some orders, invoices or credit notes ?
                            If bImportTypeValid And XMLImportRecordList IsNot Nothing Then ' TJS 05/01/10
                                ' yes, process each import record in file
                                For Each XMLImportRecordNode In XMLImportRecordList
                                    Try
                                        XMLTemp = XDocument.Parse(XMLImportRecordNode.ToString)
                                        ' create Generic XML document for this import record
                                        XMLSingleImportRecord = New XDocument
                                        XMLISImportNode = New XElement("eShopCONNECT")
                                        XMLSourceNode = New XElement("Source")
                                        XMLSourceNode.Add(New XElement("SourceName", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_SOURCE_NAME)))
                                        XMLSourceNode.Add(New XElement("SourceCode", strSourceCode))
                                        XMLISImportNode.Add(XMLSourceNode)

                                        ' what import type are we processing ?
                                        ' Quotes, Orders, Invoices, Credit Notes are processed differenct from Leads, Prospects and Customers
                                        If ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_QUOTE_IMPORT_TYPE Or _
                                            ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_ORDER_IMPORT_TYPE Or _
                                                ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_INVOICE_IMPORT_TYPE Or _
                                                ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower = GENERIC_XML_CREDITNOTE_IMPORT_TYPE Then ' TJS 05/01/10
                                            ' Import is an Order, Invoice or Credit Note
                                            Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 05/01/10
                                                    ' check if IS CustomerCode provided
                                                    strISCustomerCode = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode") ' TJS 05/01/10
                                                    ' get Source Customer ID
                                                    strSourceCustomerID = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID") ' TJS 05/01/10

                                                Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                    ' check if IS CustomerCode provided
                                                    strISCustomerCode = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                    ' get Source Customer ID
                                                    strSourceCustomerID = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID")

                                                Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                    ' check if IS CustomerCode provided
                                                    strISCustomerCode = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                    ' get Source Customer ID
                                                    strSourceCustomerID = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID")

                                                Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                    ' check if IS CustomerCode provided
                                                    strISCustomerCode = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                    ' get Source Customer ID
                                                    strSourceCustomerID = ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER & "/SourceCustomerID")

                                                Case Else
                                                    ' these lines added to suppress warning message
                                                    strISCustomerCode = ""
                                                    strSourceCustomerID = ""

                                            End Select

                                            ' check if customer exists in IS
                                            bCustomerValid = True
                                            If strISCustomerCode <> "" Then
                                                ImportExportProcessFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.CustomerImportExportView_DEV000221.TableName, _
                                                    "ReadCustomerImportExportCustCodeView_DEV000221", AT_CUSTOMER_CODE, strISCustomerCode}}, _
                                                    Interprise.Framework.Base.Shared.ClearType.Specific)
                                                If m_ImportExportDataset.CustomerImportExportView_DEV000221.Count = 0 Then
                                                    bCustomerValid = False
                                                End If

                                            ElseIf strSourceCustomerID <> "" Then
                                                ImportExportProcessFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.CustomerImportExportView_DEV000221.TableName, _
                                                    "ReadCustomerImportExportView_DEV000221", AT_IMPORT_CUSTOMER_ID, strSourceCustomerID, AT_IMPORT_SOURCE_ID, strSourceCode}}, _
                                                    Interprise.Framework.Base.Shared.ClearType.Specific)
                                                If m_ImportExportDataset.CustomerImportExportView_DEV000221.Count > 0 Then
                                                    strISCustomerCode = m_ImportExportDataset.CustomerImportExportView_DEV000221(0).CustomerCode
                                                Else
                                                    strISCustomerCode = ""
                                                End If

                                            ElseIf ImportExportProcessFacade.GetXMLElementText(ImportExportProcessFacade.SourceConfig, SOURCE_CONFIG_REQUIRE_SOURCE_CUSTOMER_ID) = "No" And _
                                                ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower <> GENERIC_XML_CREDITNOTE_IMPORT_TYPE Then ' TJS 28/01/09
                                                ' Blank Source Customer ID allowed, will always create new customer except for Credit Notes

                                            Else
                                                bCustomerValid = False
                                            End If

                                            If bCustomerValid Then
                                                XMLISImportNode.Add(XMLImportRecordNode)
                                                Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                    Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 05/01/10
                                                        XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode") ' TJS 05/01/10
                                                        If XMLISCustomerIDNode IsNot Nothing Then ' TJS 05/01/10
                                                            XMLISCustomerIDNode.Value = strISCustomerCode ' TJS 05/01/10
                                                        Else
                                                            XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_QUOTE_BILLING_DETAILS_CUSTOMER) ' TJS 05/01/10
                                                            XMLISCustomerIDNode.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 05/01/10
                                                        End If
                                                        XMLISImportNode.Add(New XElement("QuoteCount", "1")) ' TJS 05/01/10

                                                    Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                        XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                        If XMLISCustomerIDNode IsNot Nothing Then ' TJS 06/05/09
                                                            XMLISCustomerIDNode.Value = strISCustomerCode
                                                        Else
                                                            XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_ORDER_BILLING_DETAILS_CUSTOMER) ' TJS 06/05/09
                                                            XMLISCustomerIDNode.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 06/05/09
                                                        End If
                                                        XMLISImportNode.Add(New XElement("OrderCount", "1"))

                                                    Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                        XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                        If XMLISCustomerIDNode IsNot Nothing Then ' TJS 06/05/09
                                                            XMLISCustomerIDNode.Value = strISCustomerCode
                                                        Else
                                                            XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_INVOICE_BILLING_DETAILS_CUSTOMER) ' TJS 06/05/09
                                                            XMLISCustomerIDNode.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 06/05/09
                                                        End If
                                                        XMLISImportNode.Add(New XElement("InvoiceCount", "1"))

                                                    Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                        XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER & "/ISCustomerCode")
                                                        If XMLISCustomerIDNode IsNot Nothing Then ' TJS 06/05/09
                                                            XMLISCustomerIDNode.Value = strISCustomerCode
                                                        Else
                                                            XMLISCustomerIDNode = XMLISImportNode.XPathSelectElement(GENERIC_XML_CREDITNOTE_BILLING_DETAILS_CUSTOMER) ' TJS 06/05/09
                                                            XMLISCustomerIDNode.Add(New XElement("ISCustomerCode", strISCustomerCode)) ' TJS 06/05/09
                                                        End If
                                                        XMLISImportNode.Add(New XElement("CreditNoteCount", "1"))

                                                End Select
                                                XMLSingleImportRecord.Add(XMLISImportNode)

                                                ' Single Import XML Built now do call to import record (and create customer where customer does not exist except on Credit Notes)
                                                Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                    Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 05/01/10
                                                        XMLFacadeImportResponse = ImportExportProcessFacade.XMLQuoteImport(XMLSingleImportRecord) ' TJS 05/01/10
                                                        XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse") ' TJS 05/01/10
                                                        XMLImportResponseNode.Add(New XElement("CustomerOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_CUSTOMER_ORDER_REF))) ' TJS 05/01/10
                                                        XMLImportResponseNode.Add(New XElement("SourceQuoteRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF))) ' TJS 05/01/10

                                                    Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                        XMLFacadeImportResponse = ImportExportProcessFacade.XMLOrderImport(XMLSingleImportRecord, "")
                                                        XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                        XMLImportResponseNode.Add(New XElement("CustomerOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_CUSTOMER_ORDER_REF)))
                                                        XMLImportResponseNode.Add(New XElement("SourceOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF)))

                                                    Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                        XMLFacadeImportResponse = ImportExportProcessFacade.XMLInvoiceImport(XMLSingleImportRecord)
                                                        XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                        XMLImportResponseNode.Add(New XElement("CustomerOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_CUSTOMER_ORDER_REF)))
                                                        XMLImportResponseNode.Add(New XElement("SourceInvoiceRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF)))

                                                    Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                        XMLFacadeImportResponse = ImportExportProcessFacade.XMLCreditNoteImport(XMLSingleImportRecord)
                                                        XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                        XMLImportResponseNode.Add(New XElement("CustomerOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_CUSTOMER_ORDER_REF)))
                                                        XMLImportResponseNode.Add(New XElement("SourceCreditNoteRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF)))

                                                    Case Else
                                                        ' this line added to suppress warning message
                                                        XMLFacadeImportResponse = Nothing ' TJS 11/05/09
                                                        XMLImportResponseNode = Nothing

                                                End Select

                                                If XMLFacadeImportResponse IsNot Nothing Then ' TJS 11/05/09
                                                    XMLTemp = XDocument.Parse(XMLImportResponseNode.ToString) ' TJS 11/05/09
                                                    If ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/Status").ToLower = "success" Then ' TJS 28/01/09 TJS 11/05/09
                                                        Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower ' TJS 28/01/09
                                                            Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 05/01/10
                                                                sTemp = "Customer Order Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerOrderRef") ' TJS 05/01/10
                                                                sTemp = sTemp & ", Source Quote Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/SourceQuoteRef") ' TJS 05/01/10
                                                                sTemp = sTemp & " in " & strFileName & " processed successfully for IS Customer Code " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerCode") ' TJS 05/01/10
                                                                sTemp = sTemp & " as Order Number " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/QuoteNumber") ' TJS 05/01/10

                                                            Case GENERIC_XML_ORDER_IMPORT_TYPE ' TJS 28/01/09
                                                                sTemp = "Customer Order Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerOrderRef") ' TJS 28/01/09 TJS 11/05/09
                                                                sTemp = sTemp & ", Source Order Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/SourceOrderRef") ' TJS 28/01/09 TJS 11/05/09
                                                                sTemp = sTemp & " in " & strFileName & " processed successfully for IS Customer Code " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerCode") ' TJS 11/05/09
                                                                sTemp = sTemp & " as Order Number " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/OrderNumber") ' TJS 28/01/09 TJS 11/05/09

                                                            Case GENERIC_XML_INVOICE_IMPORT_TYPE ' TJS 28/01/09
                                                                sTemp = "Customer Order Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerOrderRef") ' TJS 28/01/09 TJS 11/05/09
                                                                sTemp = sTemp & ", Source Invoice Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/SourceInvoiceRef") ' TJS 28/01/09 TJS 11/05/09
                                                                sTemp = sTemp & " in " & strFileName & " processed successfully for IS Customer Code " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerCode") ' TJS 11/05/09
                                                                sTemp = sTemp & " as Invoice Number " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/InvoiceNumber") ' TJS 28/01/09 TJS 11/05/09

                                                            Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE ' TJS 28/01/09
                                                                sTemp = "Customer Order Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerOrderRef") ' TJS 28/01/09 TJS 11/05/09
                                                                sTemp = sTemp & ", Source Credit Note Ref " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/SourceCreditNoteRef") ' TJS 28/01/09 TJS 11/05/09
                                                                sTemp = sTemp & " in " & strFileName & " processed successfully for IS Customer Code " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CustomerCode") ' TJS 11/05/09
                                                                sTemp = sTemp & " as Credit Note Number " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/CreditNoteNumber") ' TJS 28/01/09 TJS 11/05/09

                                                            Case Else ' TJS 28/01/09
                                                                sTemp = strFileName & " processed successfully, but unknown Source Type " & ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE) ' TJS 11/05/09
                                                                sTemp = sTemp & ", Response was " & XMLFacadeImportResponse.ToString ' TJS 28/01/09

                                                        End Select
                                                        m_ImportExportConfigFacade.WriteLogProgressRecord(sTemp) ' TJS 28/01/09 TJS 22/02/09
                                                    Else
                                                        ' error will alreay have been logged so just return
                                                        bReturnValue = False ' TJS 28/01/09
                                                    End If
                                                End If

                                                XMLSingleImportRecord = Nothing
                                            Else
                                                XMLImportResponseNode = New XElement("ImportResponse")
                                                XMLImportResponseNode.Add(New XElement("Status", "Error"))
                                                XMLImportResponseNode.Add(New XElement("CustomerOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_CUSTOMER_ORDER_REF)))
                                                Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                    Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 05/01/10
                                                        XMLImportResponseNode.Add(New XElement("SourceQuoteRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF))) ' TJS 05/01/10

                                                    Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                        XMLImportResponseNode.Add(New XElement("SourceOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF)))

                                                    Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                        XMLImportResponseNode.Add(New XElement("SourceInvoiceRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF)))

                                                    Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                        XMLImportResponseNode.Add(New XElement("SourceCreditNoteRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF)))

                                                End Select
                                                XMLImportResponseNode.Add(New XElement("ErrorCode", "002"))
                                                XMLImportResponseNode.Add(New XElement("ErrorMessage", "Missing or invalid customer code/id"))
                                                ImportExportProcessFacade.SendSourceErrorEmail(SourceConfig, "ProcessGenericXML", XMLImportResponseNode.ToString, XMLTemp.ToString) ' TJS 06/02/09 TJS 20/05/09 TJS 14/08/09

                                                bReturnValue = False ' TJS 29/05/09
                                            End If
                                        Else
                                            ' start of code added TJS 05/01/10
                                            XMLISImportNode.Add(XMLImportRecordNode)
                                            Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                Case GENERIC_XML_LEAD_IMPORT_TYPE
                                                    XMLISImportNode.Add(New XElement("LeadCount", "1"))

                                                Case GENERIC_XML_PROSPECT_IMPORT_TYPE
                                                    XMLISImportNode.Add(New XElement("ProspectCount", "1"))

                                                Case GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                                    XMLISImportNode.Add(New XElement("CustomerCount", "1"))

                                            End Select
                                            XMLSingleImportRecord.Add(XMLISImportNode)

                                            ' Single Import XML Built now do call to import record 
                                            Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                Case GENERIC_XML_LEAD_IMPORT_TYPE
                                                    XMLFacadeImportResponse = ImportExportProcessFacade.XMLLeadImport(XMLSingleImportRecord)
                                                    XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                    XMLImportResponseNode.Add(New XElement("SourceLeadID", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_LEAD_SOURCE_LEAD_ID)))

                                                Case GENERIC_XML_PROSPECT_IMPORT_TYPE
                                                    XMLFacadeImportResponse = ImportExportProcessFacade.XMLCustomerProspectImport(XMLSingleImportRecord, True)
                                                    XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                    XMLImportResponseNode.Add(New XElement("SourceProspectID", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_PROSPECT_SOURCE_PROSPECT_ID)))

                                                Case GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                                    XMLFacadeImportResponse = ImportExportProcessFacade.XMLCustomerProspectImport(XMLSingleImportRecord, False)
                                                    XMLImportResponseNode = XMLFacadeImportResponse.XPathSelectElement("/ImportResponse")
                                                    XMLImportResponseNode.Add(New XElement("SourceCustomerID", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CUSTOMER_SOURCE_CUSTOMER_ID)))

                                                Case Else
                                                    ' this line added to suppress warning message
                                                    XMLFacadeImportResponse = Nothing
                                                    XMLImportResponseNode = Nothing

                                            End Select

                                            If XMLFacadeImportResponse IsNot Nothing Then
                                                XMLTemp = XDocument.Parse(XMLImportResponseNode.ToString)
                                                If ImportExportProcessFacade.GetXMLElementText(XMLTemp, "ImportResponse/Status").ToLower = "success" Then
                                                    Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                                        Case GENERIC_XML_LEAD_IMPORT_TYPE
                                                            sTemp = "Source Lead ID " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_LEAD_SOURCE_LEAD_ID)
                                                            sTemp = sTemp & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/LeadCode").Value

                                                        Case GENERIC_XML_PROSPECT_IMPORT_TYPE
                                                            sTemp = "Source Prospect ID " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_PROSPECT_SOURCE_PROSPECT_ID)
                                                            sTemp = sTemp & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/ProspectCode").Value

                                                        Case GENERIC_XML_CUSTOMER_IMPORT_TYPE
                                                            sTemp = "Source Customer ID " & ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CUSTOMER_SOURCE_CUSTOMER_ID)
                                                            sTemp = sTemp & " successfully imported as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/CustomerCode").Value

                                                        Case Else
                                                            sTemp = strFileName & " processed successfully, but unknown Source Type " & ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE)
                                                            sTemp = sTemp & ", Response was " & XMLFacadeImportResponse.ToString

                                                    End Select
                                                    m_ImportExportConfigFacade.WriteLogProgressRecord(sTemp)
                                                Else
                                                    ' error will alreay have been logged so just return
                                                    bReturnValue = False
                                                End If
                                            End If

                                            XMLSingleImportRecord = Nothing
                                            ' end of code added TJS 05/01/10
                                        End If

                                    Catch ex As Exception
                                        ' start of code added TJS 29/05/09
                                        XMLImportResponseNode = New XElement("ImportResponse")
                                        XMLImportResponseNode.Add(New XElement("Status", "Error"))
                                        XMLImportResponseNode.Add(New XElement("CustomerOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_CUSTOMER_ORDER_REF)))
                                        Select Case ImportExportProcessFacade.GetXMLElementText(XMLRequest, GENERIC_XML_SOURCE_IMPORT_TYPE).ToLower
                                            Case GENERIC_XML_QUOTE_IMPORT_TYPE ' TJS 05/01/10
                                                XMLImportResponseNode.Add(New XElement("SourceQuoteRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_QUOTE_SOURCE_QUOTE_REF))) ' TJS 05/01/10

                                            Case GENERIC_XML_ORDER_IMPORT_TYPE
                                                XMLImportResponseNode.Add(New XElement("SourceOrderRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_ORDER_SOURCE_ORDER_REF)))

                                            Case GENERIC_XML_INVOICE_IMPORT_TYPE
                                                XMLImportResponseNode.Add(New XElement("SourceInvoiceRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_INVOICE_SOURCE_INVOICE_REF)))

                                            Case GENERIC_XML_CREDITNOTE_IMPORT_TYPE
                                                XMLImportResponseNode.Add(New XElement("SourceCreditNoteRef", ImportExportProcessFacade.GetXMLElementText(XMLTemp, GENERIC_XML_CREDITNOTE_SOURCE_CREDITNOTE_REF)))

                                        End Select
                                        XMLImportResponseNode.Add(New XElement("ErrorCode", "005"))
                                        XMLImportResponseNode.Add(New XElement("ErrorMessage", "Invalid XML - " & ex.Message.Replace(vbCrLf, "")))
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig, "ProcessGenericXML", strFileName & " processing failed due to XML error , reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLImportRecordNode.ToString, strInputXML)

                                        bReturnValue = False
                                        ' end of code added TJS 29/05/09

                                    End Try
                                    If bShutDownInProgress Then ' TJS 02/08/13
                                        Exit For ' TJS 02/08/13
                                    End If
                                Next
                            End If

                        Else
                            strErrCode = "002 - source activation or validation error"
                            bReturnValue = False ' TJS 28/01/09
                        End If

                    Catch ex As Exception
                        m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig, "ProcessGenericXML", strFileName & " processing failed due to XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & strInputXML, "") ' TJS 29/05/09
                        bReturnValue = False ' TJS 29/05/09

                    End Try

                Else
                    strErrCode = "001 - missing or invalid XML import data"
                    bReturnValue = False ' TJS 28/01/09
                End If
            Else
                strErrCode = "001 - missing or invalid XML import data"
                bReturnValue = False ' TJS 28/01/09
            End If

            If strErrCode <> "" Then
                m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig, "ProcessGenericXML", strFileName & " processing failed - Error Code " & strErrCode, strInputXML) ' TJS 28/01/09 TJS 06/02/09 TJS 14/08/09
            End If

        Catch Ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(SourceConfig, "ProcessGenericXML", Ex, strInputXML) ' TJS 28/01/09 TJS 06/02/09 TJS 14/08/09
            bReturnValue = False ' TJS 03/04/09

        Finally
            ImportExportProcessFacade.Dispose() ' TJS 28/01/09

        End Try
        Return bReturnValue ' TJS 28/01/09

    End Function
#End Region

End Module
