Imports Lerryn.Framework.ImportExport.SourceConfig
Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Collections.Generic
Imports System.Text
Imports MarketplaceWebService
Imports MarketplaceWebService.Mock
Imports MarketplaceWebService.Model

Public Class AmazonMWSConnector

    Private Const UKDeveloperID As String = Interprise.Licensing.Base.Shared.Const.AMAZON_MWS_DEVELOPER_ID
    Private Const UKAccessKeyID As String = Interprise.Licensing.Base.Shared.Const.AMAZON_MWS_ACCESS_KEY_ID
    Private Const UKSecretAccessKey As String = Interprise.Licensing.Base.Shared.Const.AMAZON_MWS_SECRET_ACCESS_KEY
    ' Start of code added TJS 16/06/12 and completed TJS 02/08/12
    Private Const USDeveloperID As String = Interprise.Licensing.Base.Shared.Const.AMAZON_MWS_DEVELOPER_ID
    Private Const USAccessKeyID As String = Interprise.Licensing.Base.Shared.Const.AMAZON_MWS_ACCESS_KEY_ID
    Private Const USSecretAccessKey As String = Interprise.Licensing.Base.Shared.Const.AMAZON_MWS_SECRET_ACCESS_KEY
    ' End of code added TJS 16/06/12 and completed TJS 02/08/12

    Private applicationVersion As String
    Private m_AmazonThrottling As Lerryn.Facade.ImportExport.AmazonMWSThrottling ' TJS 03/07/13
    Private m_ShutDownInProgress As Boolean = False ' TJS 02/08/13

    Public WriteOnly Property ShutDownInProgress() As Boolean ' TJS 02/08/13
        Set(value As Boolean) ' TJS 02/08/13
            m_ShutDownInProgress = value ' TJS 02/08/13
        End Set
    End Property

    Public Sub PollAmazonForFiles(ByRef ImportExportConfigFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade, _
        ByRef ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, ByVal BaseProductName As String, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByVal InhibitWebPosts As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -     Polls amazon for files to download 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/07/11 | TJS             | 2011.1.00 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Corrected source config being passed to SendErrorEmail
        ' 16/06/12 | TJS             | 2012.1.07 | Modified to cater for different developerids in different countries
        ' 05/07/12 | TJS             | 2012.1.08 | Modified error handling on call to UpdateReportAcknowledgement
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        ' 05/07/13 | TJS             | 2013.1.25 | Modified to acknowledge multiple reports in one operaion
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonListRequest As MarketplaceWebService.Model.GetReportListRequest
        Dim amazonListNextTokenRequest As MarketplaceWebService.Model.GetReportListByNextTokenRequest
        Dim amazonFileRequest As MarketplaceWebService.Model.GetReportRequest
        Dim amazonListResponse As MarketplaceWebService.Model.GetReportListResponse
        Dim amazonListNextTokenResponse As MarketplaceWebService.Model.GetReportListByNextTokenResponse
        Dim amazonFileResponse As MarketplaceWebService.Model.GetReportResponse
        Dim amazonListResult As MarketplaceWebService.Model.GetReportListResult
        Dim amazonListNextTokenResult As MarketplaceWebService.Model.GetReportListByNextTokenResult
        Dim amazonFileResult As MarketplaceWebService.Model.GetReportResult
        Dim reportInfoList As List(Of ReportInfo), FileReportInfo As ReportInfo
        Dim amazonFileResponseMetadata As ResponseMetadata
        Dim rowAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row
        Dim reportStream As MemoryStream, reportFileStream As FileStream, reader As StreamReader
        Dim strFileName As String, strOrderMD5 As String, strOrderRequestID As String
        Dim strFileContent As String, strError As String, strThrottlingDetails As String ' TJS 03/07/13
        Dim strTemp As String, strNextToken As String, strReportsToAck() As String ' TJS 05/07/13
        Dim iRowLoop As Integer, iColumnLoop As Integer, bCreateFileRecord As Boolean

        ReDim strReportsToAck(0) ' TJS 05/07/13

        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = GetAmazonMWSURL(ActiveAmazonSettings.AmazonSite)
        amazonMWSConfig.SetUserAgentHeader(BaseProductName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        If Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnAccessKeyID) And Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnSecretAccessKey) Then ' TJS 16/06/12
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(ActiveAmazonSettings.OwnAccessKeyID, ActiveAmazonSettings.OwnSecretAccessKey, amazonMWSConfig) ' TJS 16/06/12

        ElseIf ActiveAmazonSettings.AmazonSite = ".com" And Not String.IsNullOrEmpty(USAccessKeyID) And Not String.IsNullOrEmpty(USSecretAccessKey) Then ' TJS 16/06/12
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(USAccessKeyID, USSecretAccessKey, amazonMWSConfig) ' TJS 16/06/12

        Else
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(UKAccessKeyID, UKSecretAccessKey, amazonMWSConfig)
        End If

        amazonListRequest = New MarketplaceWebService.Model.GetReportListRequest()

        amazonListRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
        amazonListRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID

        ' LJG - Setting this to false means "do not include reports that have been acknowledged"
        ' See here: (search for UpdateReportAcknowledgements )
        ' https://images-na.ssl-images-amazon.com/images/G/02/mwsportal/doc/bde/SOAP_to_Amazon_MWS_Migration_Guide._V180902225_.pdf
        ' More general object info here:
        ' http://rdoc.info/github/dmichael/amazon-mws/master/Amazon/MWS/Report  
        amazonListRequest.Acknowledged = False
        ' set additional request parameters here if necessary

        Try
            If m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "GetReport") Then ' TJS 03/07/13
                amazonListResponse = amazonMWSService.GetReportList(amazonListRequest)
                If amazonListResponse.IsSetGetReportListResult Then
                    amazonListResult = amazonListResponse.GetReportListResult
                    If amazonListResult.IsSetNextToken Then
                        strNextToken = amazonListResult.NextToken
                    Else
                        strNextToken = ""
                    End If
                    If amazonListResult.IsSetHasNext Then

                    End If
                    reportInfoList = amazonListResult.ReportInfo
                    Do
                        For Each FileReportInfo In reportInfoList
                            If FileReportInfo.IsSetReportId Then
                                If m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "GetReport") Then ' TJS 03/07/13
                                    amazonFileRequest = New GetReportRequest()
                                    amazonFileRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
                                    amazonFileRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID
                                    amazonFileRequest.ReportId = FileReportInfo.ReportId

                                    ' Note that depending on the type of report being downloaded, a report can reach 
                                    ' sizes greater than 1GB. For this reason we recommend that you _always_ program to
                                    ' MWS in a streaming fashion. Otherwise, as your business grows you may silently reach
                                    ' the in-memory size limit and have to re-work your solution.
                                    ' NOTE: Due to Content-MD5 validation, the stream must be read/write.
                                    reportStream = New MemoryStream
                                    amazonFileRequest.Report = reportStream
                                    amazonFileResponse = amazonMWSService.GetReport(amazonFileRequest)
                                    If amazonFileResponse.IsSetGetReportResult Then
                                        amazonFileResult = amazonFileResponse.GetReportResult
                                        If amazonFileResult.IsSetContentMD5 Then
                                            strOrderMD5 = amazonFileResult.ContentMD5
                                        End If
                                    End If
                                    If amazonFileResponse.IsSetResponseMetadata Then
                                        amazonFileResponseMetadata = amazonFileResponse.ResponseMetadata
                                        If amazonFileResponseMetadata.IsSetRequestId() Then
                                            strOrderRequestID = amazonFileResponseMetadata.RequestId
                                        End If
                                    End If
                                    reader = New StreamReader(reportStream)
                                    strFileContent = reader.ReadToEnd
                                    ' check file is XML
                                    If Left(Trim(strFileContent.ToLower), 38) = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                        ' has file already been downloaded ?
                                        If "" & ImportExportConfigFacade.GetField("FileName_DEV000221", "LerrynImportExportAmazonFiles_DEV000221", "SiteCode_DEV000221 = '" & _
                                            ActiveAmazonSettings.AmazonSite & "' AND AmazonDocumentID_DEV000221 = '" & FileReportInfo.ReportId & "' AND MerchantID_DEV000221 = '" & _
                                            ActiveAmazonSettings.MerchantToken & "' AND FileIsInputFromAmazon_DEV000221 = 1") = "" Then ' TJS 22/03/13
                                            ' no, is it a response file
                                            bCreateFileRecord = True
                                            If FileReportInfo.ReportType = "FeedSummaryReport" Then
                                                ' yes, can we find the submission file ?
                                                strTemp = "" & ImportExportConfigFacade.GetField("Processed_DEV000221", "LerrynImportExportAmazonFiles_DEV000221", "SiteCode_DEV000221 = '" & _
                                                    ActiveAmazonSettings.AmazonSite & "' AND AmazonDocumentID_DEV000221 = '" & FileReportInfo.ReportRequestId & "' AND MerchantID_DEV000221 = '" & _
                                                    ActiveAmazonSettings.MerchantToken & "' AND FileIsInputFromAmazon_DEV000221 = 0 AND FileSentToAmazon_DEV000221 = 1") ' TJS 22/03/13
                                                If strTemp <> "" Then
                                                    ' yes, don't create new file record 
                                                    bCreateFileRecord = False
                                                    ' has response been processed ?
                                                    If strTemp.ToUpper = "FALSE" Then
                                                        ' no, save response with original record
                                                        ImportExportConfigFacade.ExecuteNonQuery(CommandType.StoredProcedure, "SetLerrynImpExpAmazonFileResponse_DEV000221", New String()() {New String() {"@SiteCode", ActiveAmazonSettings.AmazonSite}, New String() {"@MerchantID", ActiveAmazonSettings.MerchantToken}, New String() {"@SubmissionDocID", FileReportInfo.ReportRequestId}, New String() {"@ReceivedDocumentID", FileReportInfo.ReportId}, New String() {"@ResponseContent", strFileContent}}) ' TJS 22/03/13
                                                    End If
                                                End If
                                            End If
                                            ' do we still nneed to create record ?
                                            If bCreateFileRecord Then
                                                ' yes
                                                rowAmazonFile = ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.NewLerrynImportExportAmazonFiles_DEV000221Row
                                                rowAmazonFile.AmazonDocumentID_DEV000221 = FileReportInfo.ReportId
                                                rowAmazonFile.SiteCode_DEV000221 = ActiveAmazonSettings.AmazonSite
                                                rowAmazonFile.MerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken ' TJS 22/03/13
                                                rowAmazonFile.FileName_DEV000221 = FileReportInfo.ReportId
                                                rowAmazonFile.AmazonMessageType_DEV000221 = FileReportInfo.ReportType
                                                rowAmazonFile.FileIsInputFromAmazon_DEV000221 = True
                                                rowAmazonFile.FileSentToAmazon_DEV000221 = False
                                                rowAmazonFile.ResponseReceived_DEV000221 = False
                                                rowAmazonFile.FileContent_DEV000221 = strFileContent
                                                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.AddLerrynImportExportAmazonFiles_DEV000221Row(rowAmazonFile)
                                                If Not ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                                                    "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                                                    "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                                                    "Save Amazon File received", False) Then
                                                    strError = ""
                                                    For iRowLoop = 0 To ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Rows.Count - 1
                                                        For iColumnLoop = 0 To ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Columns.Count - 1
                                                            If ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                strError = strError & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName & _
                                                                    "." & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                                    ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                            End If
                                                        Next
                                                    Next
                                                End If
                                            End If
                                        End If
                                    Else
                                        ' write file to manual preocessing directory
                                        strFileName = "Amazon_" & FileReportInfo.ReportType & "_" & FileReportInfo.ReportId
                                        If File.Exists(ActiveAmazonSettings.ManualProcessingPath & strFileName & ".txt") Then
                                            strFileName = strFileName & "_" & Format(Date.Now, "yyyy-MM-dd-hh-mm-ss")
                                        End If
                                        reportFileStream = System.IO.File.Open(ActiveAmazonSettings.ManualProcessingPath & strFileName & ".txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite)
                                        reportStream.WriteTo(reportFileStream)
                                        amazonFileRequest.Report.Close()
                                        reportFileStream.Close()
                                    End If
                                    reader.Close()
                                    reader.Dispose()

                                    ' have we inhibited acknowledging files from amazon ?
                                    If Not InhibitWebPosts Then
                                        ' no, acknowledge report to prevent it being downloaded again
                                        If Not String.IsNullOrEmpty(strReportsToAck(strReportsToAck.Length - 1)) Then ' TJS 05/07/13
                                            ReDim Preserve strReportsToAck(strReportsToAck.Length) ' TJS 05/07/13
                                        End If
                                        strReportsToAck(strReportsToAck.Length - 1) = FileReportInfo.ReportId ' TJS 05/07/13
                                    Else
                                        ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from acknowledging Amazon Document ID " & FileReportInfo.ReportId & " from amazon" & ActiveAmazonSettings.AmazonSite) ' TJS 05/07/13
                                    End If
                                End If
                            Else
                                ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "PollAmazonForFiles", "Amazon report in list, but no Report ID available to download it", "") ' TJS 02/12/11
                            End If
                            If m_ShutDownInProgress Then ' TJS 02/08/13
                                Exit For ' TJS 02/08/13
                            End If
                        Next
                        If strNextToken = "" Or m_ShutDownInProgress Then ' TJS 02/08/13
                            Exit Do
                        ElseIf m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "GetReport") Then ' TJS 03/07/13
                            amazonListNextTokenRequest = New MarketplaceWebService.Model.GetReportListByNextTokenRequest

                            amazonListNextTokenRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
                            amazonListNextTokenRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID
                            amazonListNextTokenRequest.NextToken = strNextToken

                            amazonListNextTokenResponse = amazonMWSService.GetReportListByNextToken(amazonListNextTokenRequest)
                            If amazonListNextTokenResponse.IsSetGetReportListByNextTokenResult Then
                                amazonListNextTokenResult = amazonListNextTokenResponse.GetReportListByNextTokenResult
                                If amazonListNextTokenResult.IsSetNextToken Then
                                    strNextToken = amazonListNextTokenResult.NextToken
                                Else
                                    strNextToken = ""
                                End If
                                If amazonListNextTokenResult.IsSetHasNext Then

                                End If
                                reportInfoList = amazonListNextTokenResult.ReportInfo
                            Else
                                Exit Do
                            End If
                        Else
                            ' throttling is active so exit loop
                            Exit Do ' TJS 05/07/13
                        End If
                        If m_ShutDownInProgress Then ' TJS 02/08/13
                            Exit Do ' TJS 02/08/13
                        End If
                    Loop
                    ' have we inhibited acknowledging files from amazon and also do we have any reports to acknowledge ?
                    If Not InhibitWebPosts AndAlso Not String.IsNullOrEmpty(strReportsToAck(strReportsToAck.Length - 1)) Then ' TJS 05/07/13
                        strError = "" ' TJS 05/07/13
                        UpdateReportAcknowledgement(amazonMWSService, BaseProductName, ActiveSource, ActiveAmazonSettings, strReportsToAck, strError) ' TJS 05/07/13
                        If Not String.IsNullOrEmpty(strError) Then ' TJS 05/07/13
                            If strError.StartsWith("Throttling occurred") Then ' TJS 03/07/13
                                m_AmazonThrottling.SetThrottledFlag(ActiveAmazonSettings.AmazonSite, ActiveAmazonSettings.MerchantToken, "UpdateReportAck", strThrottlingDetails) ' TJS 03/07/13 TJS 05/07/13
                                ImportExportConfigFacade.WriteLogProgressRecord(strThrottlingDetails) ' TJS 03/07/13
                            ElseIf strError.StartsWith("Access denied") Then ' TJS 05/07/13
                                ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, BaseProductName, "UpdateReportAcknowledgement", "Access denied for Merchant Account " & ActiveAmazonSettings.MerchantToken & " on Amazon" & ActiveAmazonSettings.AmazonSite & " - please check your Amazon config settings") ' TJS 05/07/13
                            Else
                                ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "UpdateReportAcknowledgement", strError)
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            If ex.Message = "Request is throttled" Then ' TJS 03/07/13
                m_AmazonThrottling.SetThrottledFlag(ActiveAmazonSettings.AmazonSite, ActiveAmazonSettings.MerchantToken, "GetReport", strThrottlingDetails) ' TJS 03/07/13 TJS 05/07/13
                ImportExportConfigFacade.WriteLogProgressRecord(strThrottlingDetails) ' TJS 03/07/13
            ElseIf ex.Message = "Access denied" Then ' TJS 05/07/13
                ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, BaseProductName, "PollAmazonForFiles", "Access denied for Merchant Account " & ActiveAmazonSettings.MerchantToken & " on Amazon" & ActiveAmazonSettings.AmazonSite & " - please check your Amazon config settings") ' TJS 05/07/13
            Else
                ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "PollAmazonForFiles", ex)
            End If

        Finally
            amazonMWSService = Nothing
            amazonMWSConfig = Nothing
        End Try


    End Sub

    Public Sub SendFilesToAmazon(ByRef ImportExportConfigFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade, _
        ByRef ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, ByVal BaseProductName As String, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByVal InhibitWebPosts As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/07/11 | TJS             | 2011.1.00 | Function added
        ' 16/06/12 | TJS             | 2012.1.07 | Modified to cater for different developerids in different countries
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonSubmitFeedRequest As MarketplaceWebService.Model.SubmitFeedRequest
        Dim amazonSubmitFeedResponse As MarketplaceWebService.Model.SubmitFeedResponse
        Dim amazonSubmitFeedResult As MarketplaceWebService.Model.SubmitFeedResult
        Dim amazonFeedSubmissionInfo As MarketplaceWebService.Model.FeedSubmissionInfo
        Dim amazonFeedResponseMetadata As ResponseMetadata
        Dim feedStream As MemoryStream, encoding As UTF8Encoding
        Dim strFeedRequestID As String, strAmazonDocumemtID As String, strTempDocumentID As String
        Dim strUpdateSQL As String, strThrottlingDetails As String = "", iFileLoop As Integer, byteFileContent As Byte() ' TJS 03/07/13

        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = GetAmazonMWSURL(ActiveAmazonSettings.AmazonSite)
        amazonMWSConfig.SetUserAgentHeader(BaseProductName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        If Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnAccessKeyID) And Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnSecretAccessKey) Then ' TJS 16/06/12
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(ActiveAmazonSettings.OwnAccessKeyID, ActiveAmazonSettings.OwnSecretAccessKey, amazonMWSConfig) ' TJS 16/06/12

        ElseIf ActiveAmazonSettings.AmazonSite = ".com" And Not String.IsNullOrEmpty(USAccessKeyID) And Not String.IsNullOrEmpty(USSecretAccessKey) Then ' TJS 16/06/12
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(USAccessKeyID, USSecretAccessKey, amazonMWSConfig) ' TJS 16/06/12

        Else
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(UKAccessKeyID, UKSecretAccessKey, amazonMWSConfig)
        End If

        ' get details of any files created but not sent or files received and not processed
        ImportExportConfigFacade.LoadDataSet(New String()() {New String() {ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
            "GetLerrynImportExportAmazonFilesForProcessing_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        For iFileLoop = 0 To ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1
            If Not ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileIsInputFromAmazon_DEV000221 AndAlso _
                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221 <> "" AndAlso _
                CLng(ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221) < 0 Then
                ' have we inhibited sending files to amazon ?
                If Not InhibitWebPosts Then
                    ' no, load ready for sending
                    amazonSubmitFeedRequest = New MarketplaceWebService.Model.SubmitFeedRequest()

                    amazonSubmitFeedRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
                    amazonSubmitFeedRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID

                    feedStream = New MemoryStream
                    encoding = New UTF8Encoding
                    byteFileContent = encoding.GetBytes(ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileContent_DEV000221)
                    feedStream.Write(byteFileContent, 0, byteFileContent.Length)
                    amazonSubmitFeedRequest.FeedContent = feedStream
                    amazonSubmitFeedRequest.FeedContent.Position = 0

                    ' Calculating the MD5 hash value exhausts the stream, and therefore we must either reset the
                    ' position, or create another stream for the calculation.
                    amazonSubmitFeedRequest.ContentMD5 = MarketplaceWebServiceClient.CalculateContentMD5(amazonSubmitFeedRequest.FeedContent)
                    amazonSubmitFeedRequest.FeedContent.Position = 0

                    ' LJG - FeedType values seen here: https://images-na.ssl-images-amazon.com/images/G/02/mwsportal/doc/bde/SOAP_to_Amazon_MWS_Migration_Guide._V180902225_.pdf

                    ' SOAP Document Type           Valid SOAP Message Type and Amazon MWS      Amazon MWS Feed Type
                    ' Description                  FeedType value                              description
                    '
                    ' Product data                 _POST_PRODUCT_DATA_                         Product Feed
                    ' Product inventory            _POST_INVENTORY_AVAILABILITY_DATA_          Inventory Feed
                    ' Product pricing              _POST_PRODUCT_PRICING_DATA_                 Pricing Feed
                    ' Product relationships        _POST_PRODUCT_RELATIONSHIP_DATA_            Relationships Feed
                    ' Product images               _POST_PRODUCT_IMAGE_DATA_                   Product Images Feed
                    ' Product shipping overrides   _POST_PRODUCT_OVERRIDES_DATA_               Shipping Override Feed
                    ' Order Acknowledgement        _POST_ORDER_ACKNOWLEDGEMENT_DATA_           Order Acknowledgement Feed
                    ' Order Fulfillment            _POST_ORDER_FULFILLMENT_DATA_               Order Fulfillment Feed
                    ' Order Adjustment             _POST_PAYMENT_ADJUSTMENT_DATA_              Order Adjustment Feed

                    amazonSubmitFeedRequest.FeedType = ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonMessageType_DEV000221

                    Try
                        If m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "SubmitFeed") Then ' TJS 03/07/13
                            amazonSubmitFeedResponse = amazonMWSService.SubmitFeed(amazonSubmitFeedRequest)
                            If amazonSubmitFeedResponse.IsSetSubmitFeedResult Then
                                amazonSubmitFeedResult = amazonSubmitFeedResponse.SubmitFeedResult
                                If amazonSubmitFeedResult.IsSetFeedSubmissionInfo Then
                                    amazonFeedSubmissionInfo = amazonSubmitFeedResult.FeedSubmissionInfo
                                    If amazonFeedSubmissionInfo.IsSetFeedSubmissionId() Then
                                        strAmazonDocumemtID = amazonFeedSubmissionInfo.FeedSubmissionId

                                        ' mark message as sent, get temp Amazon ID uses and save actual Amazon Document ID
                                        ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileSentToAmazon_DEV000221 = True
                                        strTempDocumentID = ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221
                                        ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221 = strAmazonDocumemtID
                                        ' remove UTC Offset value to ensure XML Date conversion uses hours etc
                                        ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).SubmissionTimestamp_DEV000221 = ImportExportConfigFacade.ConvertXMLDate(amazonFeedSubmissionInfo.SubmittedDate.Substring(0, 19))
                                        ' now update Amazon Ducument ID in Status tables
                                        Select Case ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonMessageType_DEV000221
                                            Case "_POST_ORDER_ACKNOWLEDGEMENT_DATA_", "_POST_ORDER_FULFILLMENT_DATA_", "_POST_PAYMENT_ADJUSTMENT_DATA_"

                                                strUpdateSQL = "UPDATE dbo.LerrynImportExportActionStatus_DEV000221 SET SentInFileID_DEV000221 = '"
                                                strUpdateSQL = strUpdateSQL & strAmazonDocumemtID & "' WHERE SentInFileID_DEV000221 = '" & strTempDocumentID
                                                strUpdateSQL = strUpdateSQL & "' AND XMLMessageType_DEV000221 = "
                                                Select Case ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonMessageType_DEV000221
                                                    Case "_POST_ORDER_ACKNOWLEDGEMENT_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "1"

                                                    Case "_POST_ORDER_FULFILLMENT_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "2"

                                                    Case "_POST_PAYMENT_ADJUSTMENT_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "3"

                                                End Select
                                                ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, strUpdateSQL, Nothing)

                                            Case "_POST_PRODUCT_DATA_", "_POST_PRODUCT_IMAGE_DATA_", "_POST_INVENTORY_AVAILABILITY_DATA_", _
                                                "_POST_PRODUCT_PRICING_DATA_", "_POST_PRODUCT_RELATIONSHIP_DATA_"

                                                strUpdateSQL = "UPDATE dbo.LerrynImportExportInventoryActionStatus_DEV000221 SET SentInFileID_DEV000221 = '"
                                                strUpdateSQL = strUpdateSQL & strAmazonDocumemtID & "' WHERE SentInFileID_DEV000221 = '" & strTempDocumentID
                                                strUpdateSQL = strUpdateSQL & "' AND XMLMessageType_DEV000221 = "
                                                Select Case ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonMessageType_DEV000221
                                                    Case "_POST_PRODUCT_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "11"

                                                    Case "_POST_PRODUCT_IMAGE_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "12"

                                                    Case "_POST_INVENTORY_AVAILABILITY_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "13"

                                                    Case "_POST_PRODUCT_PRICING_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "14"

                                                    Case "_POST_PRODUCT_RELATIONSHIP_DATA_"
                                                        strUpdateSQL = strUpdateSQL & "15"

                                                End Select
                                                ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, strUpdateSQL, Nothing)

                                        End Select
                                    End If
                                    ' following features of amazonFeedSubmissionInfo not currently used
                                    'If amazonFeedSubmissionInfo.IsSetFeedType() Then

                                    'End If
                                    'If amazonFeedSubmissionInfo.IsSetSubmittedDate() Then

                                    'End If
                                    'If amazonFeedSubmissionInfo.IsSetFeedProcessingStatus() Then

                                    'End If
                                    'If amazonFeedSubmissionInfo.IsSetStartedProcessingDate() Then

                                    'End If
                                    'If amazonFeedSubmissionInfo.IsSetCompletedProcessingDate() Then

                                    'End If

                                End If
                            End If
                            If amazonSubmitFeedResponse.IsSetResponseMetadata Then
                                amazonFeedResponseMetadata = amazonSubmitFeedResponse.ResponseMetadata
                                If amazonFeedResponseMetadata.IsSetRequestId() Then
                                    strFeedRequestID = amazonFeedResponseMetadata.RequestId
                                End If
                            End If
                        End If

                    Catch ex As Exception
                        If ex.Message = "Request is throttled" Then ' TJS 03/07/13
                            m_AmazonThrottling.SetThrottledFlag(ActiveAmazonSettings.AmazonSite, ActiveAmazonSettings.MerchantToken, "SubmitFeed", strThrottlingDetails) ' TJS 03/07/13 TJS 05/07/13
                            ImportExportConfigFacade.WriteLogProgressRecord(strThrottlingDetails) ' TJS 03/07/13
                        ElseIf ex.Message = "Access denied" Then ' TJS 05/07/13
                            ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, BaseProductName, "SendFilesToAmazon", "Access denied for Merchant Account " & ActiveAmazonSettings.MerchantToken & " on Amazon" & ActiveAmazonSettings.AmazonSite & " - please check your Amazon config settings") ' TJS 05/07/13
                        Else
                            ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "SendFilesToAmazon", ex)
                        End If
                        Exit For

                    Finally
                        ' update file records
                        ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                            "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                            "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                            "Update Amazon Files sent", False)

                    End Try
                Else
                    ImportExportConfigFacade.WriteLogProgressRecord("Amazon File " & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileName_DEV000221 & _
                        " inhibited from being sent to amazon" & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).SiteCode_DEV000221)
                End If
            End If
            If m_ShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next

    End Sub

    Public Function RequestAmazonReport(ByVal ReportType As String, ByVal BaseProductName As String, ByVal ActiveSource As SourceSettings, _
        ByVal ActiveAmazonSettings As AmazonSettings, ByRef SubmissionTimestamp As Date, ByRef ErrorDetails As String) As String ' TJS 02/08/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ' 02/08/12 | TJS             | 2012.1.11 | Modified to return SubmissionTimestamp
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonRequestReport As MarketplaceWebService.Model.RequestReportRequest
        Dim amazonRequestResponse As MarketplaceWebService.Model.RequestReportResponse
        Dim amazonRequestResult As MarketplaceWebService.Model.RequestReportResult
        Dim amazonRequestInfo As MarketplaceWebService.Model.ReportRequestInfo
        Dim strThrottlingDetails As String ' TJS 03/07/13

        Try
            amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

            amazonMWSConfig.ServiceURL = GetAmazonMWSURL(ActiveAmazonSettings.AmazonSite)
            amazonMWSConfig.SetUserAgentHeader(BaseProductName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

            If Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnAccessKeyID) And Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnSecretAccessKey) Then
                amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(ActiveAmazonSettings.OwnAccessKeyID, ActiveAmazonSettings.OwnSecretAccessKey, amazonMWSConfig)

            ElseIf ActiveAmazonSettings.AmazonSite = ".com" And Not String.IsNullOrEmpty(USAccessKeyID) And Not String.IsNullOrEmpty(USSecretAccessKey) Then
                amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(USAccessKeyID, USSecretAccessKey, amazonMWSConfig)

            Else
                amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(UKAccessKeyID, UKSecretAccessKey, amazonMWSConfig)
            End If

            amazonRequestReport = New MarketplaceWebService.Model.RequestReportRequest
            amazonRequestReport.Merchant = ActiveAmazonSettings.MWSMerchantID
            amazonRequestReport.Marketplace = ActiveAmazonSettings.MWSMarketplaceID
            amazonRequestReport.ReportType = ReportType

            If m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "RequestReport") Then ' TJS 03/07/13
                amazonRequestResponse = amazonMWSService.RequestReport(amazonRequestReport)
                If amazonRequestResponse.IsSetRequestReportResult Then
                    amazonRequestResult = amazonRequestResponse.RequestReportResult
                    amazonRequestInfo = amazonRequestResult.ReportRequestInfo
                    If amazonRequestInfo.ReportProcessingStatus = "_SUBMITTED_" Then
                        If amazonRequestInfo.IsSetReportRequestId Then
                            SubmissionTimestamp = amazonRequestInfo.SubmittedDate ' TJS 02/08/12
                            Return amazonRequestInfo.ReportRequestId
                        Else
                            ErrorDetails = "RequestAmazonReport did not return as ReportRequestId for report type " & ReportType
                            Return ""
                        End If

                    Else
                        ErrorDetails = "RequestAmazonReport status for report type " & ReportType & " incorrect, expected _SUBMITTED_, actual was " & amazonRequestInfo.ReportProcessingStatus
                        Return ""
                    End If
                Else
                    ErrorDetails = "RequestAmazonReport did not return a RequestReportResult for report type " & ReportType
                    Return ""
                End If
            End If

        Catch ex As Exception
            If ex.Message = "Request is throttled" Then ' TJS 03/07/13
                m_AmazonThrottling.SetThrottledFlag(ActiveAmazonSettings.AmazonSite, ActiveAmazonSettings.MerchantToken, "RequestReport", strThrottlingDetails) ' TJS 03/07/13 TJS 05/07/13
                ErrorDetails = "Cannot currently request  report type " & ReportType & " as Amazon is throttling RequestReport operations on amazon" & ActiveAmazonSettings.AmazonSite ' TJS 03/07/13
            ElseIf ex.Message = "Access denied" Then ' TJS 05/07/13
                ErrorDetails = "Access denied for Merchant Account " & ActiveAmazonSettings.MerchantToken & " on Amazon" & ActiveAmazonSettings.AmazonSite & " - please check your Amazon config settings" ' TJS 05/07/13
            Else
                ErrorDetails = "Error in RequestAmazonReport for report type " & ReportType & " - " & ex.Message & " at " & ex.StackTrace
            End If
            Return ""

        End Try

    End Function

    Public Sub PollAmazonForDocumentStatus(ByRef ImportExportConfigFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade, _
        ByRef ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, ByVal BaseProductName As String, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByVal InhibitWebPosts As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/07/11 | TJS             | 2011.1.00 | Function added
        ' 16/06/12 | TJS             | 2012.1.07 | Modified to cater for different developerids in different countries
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonSubmissionListRequest As MarketplaceWebService.Model.GetFeedSubmissionListRequest
        Dim amazonSubmissionResultRequest As MarketplaceWebService.Model.GetFeedSubmissionResultRequest
        Dim amazonSubmissionListResponse As MarketplaceWebService.Model.GetFeedSubmissionListResponse
        Dim amazonSubmissionResultResponse As MarketplaceWebService.Model.GetFeedSubmissionResultResponse
        Dim amazonSubmissionListResult As MarketplaceWebService.Model.GetFeedSubmissionListResult
        Dim amazonSubmissionResultResult As MarketplaceWebService.Model.GetFeedSubmissionResultResult
        Dim SubmissionResponseInfoList As List(Of FeedSubmissionInfo), SubmissionResponseInfo As FeedSubmissionInfo
        Dim amazonSubmissionListMetadata As ResponseMetadata, amazonSubmissionResultMetadata As ResponseMetadata
        Dim reportStream As MemoryStream, reportFileStream As FileStream, reader As StreamReader
        Dim strSubmissionResultMD5 As String, strSubmissionResultRequestID As String, strError As String
        Dim strSubmissionResultContent As String, strFileName As String, strSubmissionListRequestID As String
        Dim iRowLoop As Integer, iColumnLoop As Integer, iFileLoop As Integer, dteGetStatusFrom As Date
        Dim strThrottlingDetails As String ' TJS 03/07/13

        ImportExportConfigFacade.LoadDataSet(New String()() {New String() {ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
            "GetLerrynImportExportAmazonFilesForProcessing_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        ' by default, get status for last 5 minutes
        dteGetStatusFrom = Date.Now.AddMinutes(-5)
        ' check if any submission older than default
        For iFileLoop = 0 To ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1
            ' is file one that was sent to Amazon and is awaiting a response ?
            If Not ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileIsInputFromAmazon_DEV000221 And _
                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileSentToAmazon_DEV000221 And _
                Not ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 Then
                ' was it created earlier than current status time value ?
                If ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).DateCreated < dteGetStatusFrom Then
                    ' yes, use as default status date/time
                    dteGetStatusFrom = ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).SubmissionTimestamp_DEV000221
                End If
            End If
        Next
        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = GetAmazonMWSURL(ActiveAmazonSettings.AmazonSite)
        amazonMWSConfig.SetUserAgentHeader(BaseProductName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        If Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnAccessKeyID) And Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnSecretAccessKey) Then ' TJS 16/06/12
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(ActiveAmazonSettings.OwnAccessKeyID, ActiveAmazonSettings.OwnSecretAccessKey, amazonMWSConfig) ' TJS 16/06/12

        ElseIf ActiveAmazonSettings.AmazonSite = ".com" And Not String.IsNullOrEmpty(USAccessKeyID) And Not String.IsNullOrEmpty(USSecretAccessKey) Then ' TJS 16/06/12
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(USAccessKeyID, USSecretAccessKey, amazonMWSConfig) ' TJS 16/06/12

        Else
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(UKAccessKeyID, UKSecretAccessKey, amazonMWSConfig)
        End If

        amazonSubmissionListRequest = New GetFeedSubmissionListRequest()

        amazonSubmissionListRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
        amazonSubmissionListRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID
        ' subtract 1 minute from submission timestamp to avoid any rounding issues
        amazonSubmissionListRequest.SubmittedFromDate = dteGetStatusFrom.AddMinutes(-1)

        ' set additional request parameters here if necessary
        Try
            If m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "GetFeedSubmissionList") Then ' TJS 03/07/13
                amazonSubmissionListResponse = amazonMWSService.GetFeedSubmissionList(amazonSubmissionListRequest)
                If amazonSubmissionListResponse.IsSetGetFeedSubmissionListResult Then
                    amazonSubmissionListResult = amazonSubmissionListResponse.GetFeedSubmissionListResult
                    ' following features of amazonSubmissionResponseResult not currently used
                    'If amazonSubmissionResponseResult.IsSetNextToken Then

                    'End If
                    'If amazonSubmissionResponseResult.IsSetHasNext Then

                    'End If
                    SubmissionResponseInfoList = amazonSubmissionListResult.FeedSubmissionInfo

                    ' LerrynImportExportAmazonFiles_DEV000221 will contain details of all files sent to or received 
                    ' from Amazon which require some form of processing - start by checking for responses to files sent
                    For iFileLoop = 0 To ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1
                        ' is file one that was sent to Amazon and is awaiting a response ?
                        If Not ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileIsInputFromAmazon_DEV000221 And _
                            ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileSentToAmazon_DEV000221 And _
                            Not ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 Then
                            ' yes, is response in status list from Amazon ?
                            For Each SubmissionResponseInfo In SubmissionResponseInfoList
                                If SubmissionResponseInfo.IsSetFeedSubmissionId And SubmissionResponseInfo.IsSetFeedProcessingStatus AndAlso _
                                    SubmissionResponseInfo.FeedSubmissionId = ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221 AndAlso _
                                    SubmissionResponseInfo.FeedProcessingStatus = "_DONE_" And m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "GetFeedSubmissionResult") Then ' TJS 03/07/13
                                    ' yes, check Amazon for response
                                    Try
                                        amazonSubmissionResultRequest = New GetFeedSubmissionResultRequest()
                                        amazonSubmissionResultRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
                                        amazonSubmissionResultRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID
                                        amazonSubmissionResultRequest.FeedSubmissionId = ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221

                                        ' Note that depending on the type of report being downloaded, a report can reach 
                                        ' sizes greater than 1GB. For this reason we recommend that you _always_ program to
                                        ' MWS in a streaming fashion. Otherwise, as your business grows you may silently reach
                                        ' the in-memory size limit and have to re-work your solution.
                                        ' NOTE: Due to Content-MD5 validation, the stream must be read/write.
                                        reportStream = New MemoryStream
                                        amazonSubmissionResultRequest.FeedSubmissionResult = reportStream
                                        amazonSubmissionResultResponse = amazonMWSService.GetFeedSubmissionResult(amazonSubmissionResultRequest)
                                        If amazonSubmissionResultResponse.IsSetGetFeedSubmissionResultResult Then
                                            amazonSubmissionResultResult = amazonSubmissionResultResponse.GetFeedSubmissionResultResult
                                            If amazonSubmissionResultResult.IsSetContentMD5 Then
                                                strSubmissionResultMD5 = amazonSubmissionResultResult.ContentMD5
                                            End If
                                        End If
                                        If amazonSubmissionResultResponse.IsSetResponseMetadata Then
                                            amazonSubmissionResultMetadata = amazonSubmissionResultResponse.ResponseMetadata
                                            If amazonSubmissionResultMetadata.IsSetRequestId() Then
                                                strSubmissionResultRequestID = amazonSubmissionResultMetadata.RequestId
                                            End If
                                        End If
                                        reader = New StreamReader(reportStream)
                                        strSubmissionResultContent = reader.ReadToEnd
                                        ' check file is XML
                                        If Left(Trim(strSubmissionResultContent.ToLower), 38) = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                            ' has file already been downloaded ?
                                            If ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).IsResponseContent_DEV000221Null OrElse _
                                                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseContent_DEV000221 = "" Then
                                                ' no
                                                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 = True
                                                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ReceivedDocumentID_DEV000221 = ""
                                                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseContent_DEV000221 = strSubmissionResultContent
                                                If Not ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                                                    "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                                                    "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                                                    "Save Amazon Response", False) Then
                                                    strError = ""
                                                    For iRowLoop = 0 To ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Rows.Count - 1
                                                        For iColumnLoop = 0 To ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Columns.Count - 1
                                                            If ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                strError = strError & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName & _
                                                                    "." & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                                    ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                            End If
                                                        Next
                                                    Next
                                                Else
                                                    ImportExportConfigFacade.WriteLogProgressRecord("Amazon response File received for " & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileName_DEV000221)
                                                End If
                                            Else
                                                ' yes, has a response been received but not savered in db ?
                                                If Not ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 Then
                                                    ' no, mark as response received
                                                    ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 = True
                                                End If
                                                ' write file to manual preocessing directory
                                                strFileName = "Amazon_FeedSubmissionResult_for_" & amazonSubmissionResultRequest.FeedSubmissionId
                                                If File.Exists(ActiveAmazonSettings.ManualProcessingPath & strFileName & ".xml") Then
                                                    strFileName = strFileName & "_" & Format(Date.Now, "yyyy-MM-dd-hh-mm-ss")
                                                End If
                                                strFileName = ActiveAmazonSettings.ManualProcessingPath & strFileName & ".xml"
                                                reportFileStream = System.IO.File.Open(strFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite)
                                                reportStream.WriteTo(reportFileStream)
                                                amazonSubmissionResultRequest.FeedSubmissionResult.Close()
                                                reportFileStream.Close()
                                                ImportExportConfigFacade.WriteLogProgressRecord("Amazon response File received for " & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileName_DEV000221 & " and saved as " & strFileName)
                                            End If

                                        Else
                                            If Not ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 Then
                                                ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 = True
                                            End If
                                            ' write file to manual processing directory
                                            strFileName = "Amazon_FeedSubmissionResult_for_" & amazonSubmissionResultRequest.FeedSubmissionId
                                            If File.Exists(ActiveAmazonSettings.ManualProcessingPath & strFileName & ".txt") Then
                                                strFileName = strFileName & "_" & Format(Date.Now, "yyyy-MM-dd-hh-mm-ss")
                                            End If
                                            strFileName = ActiveAmazonSettings.ManualProcessingPath & strFileName & ".txt"
                                            reportFileStream = System.IO.File.Open(strFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite)
                                            reportStream.WriteTo(reportFileStream)
                                            amazonSubmissionResultRequest.FeedSubmissionResult.Close()
                                            reportFileStream.Close()
                                            ImportExportConfigFacade.WriteLogProgressRecord("Amazon response File received for " & ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileName_DEV000221 & " and saved as " & strFileName)

                                        End If
                                        reader.Close()
                                        reader.Dispose()

                                        ' following features of SubmissionResponseInfo not currently used
                                        'If SubmissionResponseInfo.IsSetFeedType Then

                                        'End If
                                        'If SubmissionResponseInfo.IsSetSubmittedDate Then

                                        'End If
                                        'If SubmissionResponseInfo.IsSetFeedSubmissionId Then

                                        'End If
                                        'If SubmissionResponseInfo.IsSetStartedProcessingDate Then

                                        'End If
                                        'If SubmissionResponseInfo.IsSetCompletedProcessingDate Then

                                        'End If

                                    Catch ex As Exception
                                        If ex.Message = "Request is throttled" Then ' TJS 05/07/13
                                            m_AmazonThrottling.SetThrottledFlag(ActiveAmazonSettings.AmazonSite, ActiveAmazonSettings.MerchantToken, "GetFeedSubmissionResult", strThrottlingDetails) ' TJS 05/07/13
                                            ImportExportConfigFacade.WriteLogProgressRecord(strThrottlingDetails) ' TJS 05/07/13
                                        ElseIf ex.Message = "Access denied" Then ' TJS 05/07/13
                                            ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, BaseProductName, "PollAmazonForDocumentStatus", "Access denied for Merchant Account " & ActiveAmazonSettings.MerchantToken & " on Amazon" & ActiveAmazonSettings.AmazonSite & " - please check your Amazon config settings") ' TJS 05/07/13
                                        Else
                                            ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "PollAmazonForDocumentStatus", ex)
                                        End If

                                    End Try

                                End If
                                If m_ShutDownInProgress Then ' TJS 02/08/13
                                    Exit For ' TJS 02/08/13
                                End If
                            Next
                        End If
                        If m_ShutDownInProgress Then ' TJS 02/08/13
                            Exit For ' TJS 02/08/13
                        End If
                    Next
                    ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                        "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                        "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                        "Update Amazon response Files received", False)
                End If
                If amazonSubmissionListResponse.IsSetResponseMetadata Then
                    amazonSubmissionListMetadata = amazonSubmissionListResponse.ResponseMetadata
                    If amazonSubmissionListMetadata.IsSetRequestId() Then
                        strSubmissionListRequestID = amazonSubmissionListMetadata.RequestId
                    End If
                End If
            End If

        Catch ex As Exception
            If ex.Message = "Request is throttled" Then ' TJS 03/07/13
                m_AmazonThrottling.SetThrottledFlag(ActiveAmazonSettings.AmazonSite, ActiveAmazonSettings.MerchantToken, "GetFeedSubmissionList", strThrottlingDetails) ' TJS 03/07/13 TJS 05/07/13
                ImportExportConfigFacade.WriteLogProgressRecord(strThrottlingDetails) ' TJS 03/07/13
            ElseIf ex.Message = "Access denied" Then ' TJS 05/07/13
                ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, BaseProductName, "PollAmazonForDocumentStatus", "Access denied for Merchant Account " & ActiveAmazonSettings.MerchantToken & " on Amazon" & ActiveAmazonSettings.AmazonSite & " - please check your Amazon config settings") ' TJS 05/07/13
            Else
                ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "PollAmazonForDocumentStatus", ex)
            End If

        End Try

    End Sub

    Public Function PollAmazonForReport(ByVal BaseProductName As String, ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, _
        ByVal AmazonReportRequestID As String, ByVal InhibitWebPosts As Boolean, ByRef AmazonReportID As String, ByRef ReportContent As MemoryStream, _
        ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -     Polls amazon for the result of a specific Report request
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ' 05/07/13 | TJS             | 2013.1.25 | Modified to acknowledge multiple reports in one operaion
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonReportListRequest As MarketplaceWebService.Model.GetReportListRequest
        Dim amazonReportRequest As MarketplaceWebService.Model.GetReportRequest
        Dim amazonReportListResponse As MarketplaceWebService.Model.GetReportListResponse
        Dim amazonReportResponse As MarketplaceWebService.Model.GetReportResponse
        Dim amazonReportListResult As MarketplaceWebService.Model.GetReportListResult
        Dim amazonFileResult As MarketplaceWebService.Model.GetReportResult
        Dim reportInfoList As List(Of ReportInfo), ReportResponseInfo As ReportInfo
        Dim amazonReportResponseMetadata As ResponseMetadata
        Dim strReportResultMD5 As String, strReportResultRequestID As String, strError As String
        Dim strReportsToAck() As String ' TJS 05/07/13

        ReDim strReportsToAck(0) ' TJS 05/07/13

        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = GetAmazonMWSURL(ActiveAmazonSettings.AmazonSite)
        amazonMWSConfig.SetUserAgentHeader(BaseProductName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        If Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnAccessKeyID) And Not String.IsNullOrEmpty(ActiveAmazonSettings.OwnSecretAccessKey) Then
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(ActiveAmazonSettings.OwnAccessKeyID, ActiveAmazonSettings.OwnSecretAccessKey, amazonMWSConfig)

        ElseIf ActiveAmazonSettings.AmazonSite = ".com" And Not String.IsNullOrEmpty(USAccessKeyID) And Not String.IsNullOrEmpty(USSecretAccessKey) Then
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(USAccessKeyID, USSecretAccessKey, amazonMWSConfig)

        Else
            amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(UKAccessKeyID, UKSecretAccessKey, amazonMWSConfig)
        End If

        amazonReportListRequest = New GetReportListRequest()

        amazonReportListRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
        amazonReportListRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID

        ' tell amazon we want the results for a specific report request
        amazonReportListRequest.WithReportRequestIdList(New IdList().WithId(AmazonReportRequestID))
        ' don't need any additional request parameters e.g. exclude acknowledged reports as we are polling for a specific request ID

        Try
            If m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "GetReport") Then ' TJS 05/07/13
                amazonReportListResponse = amazonMWSService.GetReportList(amazonReportListRequest)
                If amazonReportListResponse.IsSetGetReportListResult Then
                    amazonReportListResult = amazonReportListResponse.GetReportListResult
                    If amazonReportListResult.IsSetReportInfo Then
                        reportInfoList = amazonReportListResult.ReportInfo
                        For Each ReportResponseInfo In reportInfoList
                            If ReportResponseInfo.IsSetReportId Then
                                amazonReportRequest = New GetReportRequest()
                                amazonReportRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
                                amazonReportRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID
                                amazonReportRequest.ReportId = ReportResponseInfo.ReportId
                                AmazonReportID = ReportResponseInfo.ReportId

                                ' Note that depending on the type of report being downloaded, a report can reach 
                                ' sizes greater than 1GB. For this reason we recommend that you _always_ program to
                                ' MWS in a streaming fashion. Otherwise, as your business grows you may silently reach
                                ' the in-memory size limit and have to re-work your solution.
                                ' NOTE: Due to Content-MD5 validation, the stream must be read/write.
                                ReportContent = New MemoryStream
                                amazonReportRequest.Report = ReportContent
                                amazonReportResponse = amazonMWSService.GetReport(amazonReportRequest)
                                If amazonReportResponse.IsSetGetReportResult Then
                                    amazonFileResult = amazonReportResponse.GetReportResult
                                    If amazonFileResult.IsSetContentMD5 Then
                                        strReportResultMD5 = amazonFileResult.ContentMD5
                                    End If
                                End If
                                If amazonReportResponse.IsSetResponseMetadata Then
                                    amazonReportResponseMetadata = amazonReportResponse.ResponseMetadata
                                    If amazonReportResponseMetadata.IsSetRequestId() Then
                                        strReportResultRequestID = amazonReportResponseMetadata.RequestId
                                    End If
                                End If

                                ' have we inhibited acknowledging files from amazon ?
                                If Not InhibitWebPosts Then
                                    ' no, acknowledge report to prevent it being downloaded again
                                    If Not String.IsNullOrEmpty(strReportsToAck(strReportsToAck.Length - 1)) Then ' TJS 05/07/13
                                        ReDim Preserve strReportsToAck(strReportsToAck.Length) ' TJS 05/07/13
                                    End If
                                    strReportsToAck(strReportsToAck.Length - 1) = ReportResponseInfo.ReportId ' TJS 05/07/13
                                End If
                                If Not InhibitWebPosts Then
                                    strError = ""
                                    UpdateReportAcknowledgement(amazonMWSService, BaseProductName, ActiveSource, ActiveAmazonSettings, strReportsToAck, strError) ' TJS 05/07/13
                                    If Not String.IsNullOrEmpty(strError) Then
                                        ErrorDetails = strError
                                    End If
                                End If
                                Return True
                            End If
                            If m_ShutDownInProgress Then ' TJS 02/08/13
                                Exit For ' TJS 02/08/13
                            End If
                        Next
                    End If
                End If
            End If
            Return False

        Catch ex As Exception
            ErrorDetails = "Error in PollAmazonForReport - " & ex.Message & " at " & ex.StackTrace
            Return False
        End Try

    End Function

    Private Function UpdateReportAcknowledgement(ByRef MWSService As MarketplaceWebService.MarketplaceWebService, ByVal BaseProductName As String, ByVal ActiveSource As SourceSettings, _
        ByVal ActiveAmazonSettings As AmazonSettings, ByVal ReportID As String(), ByRef ErrorDetails As String) As Boolean ' TJS 05/07/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/07/11 | TJS             | 2011.1.00 | Function added
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        ' 05/07/13 | TJS             | 2013.1.25 | Modified to only record throttling in log file and cater 
        '                                        | for multiple reports being acknowledged in one operaion
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim amazonUpdateAckRequest As MarketplaceWebService.Model.UpdateReportAcknowledgementsRequest
        Dim amazonUpdateAckResponse As MarketplaceWebService.Model.UpdateReportAcknowledgementsResponse
        Dim amazonUpdateAckResult As MarketplaceWebService.Model.UpdateReportAcknowledgementsResult
        Dim reportInfoList As List(Of ReportInfo), UpdateAckReportInfo As ReportInfo
        Dim amazonUpdateAckResponseMetadata As ResponseMetadata
        Dim strUpdateAckRequestID As String, strThrottlingDetails As String = "", iUpdateAckResultCount As Decimal ' TJS 03/07/13

        UpdateReportAcknowledgement = False
        Try

            amazonUpdateAckRequest = New UpdateReportAcknowledgementsRequest()
            amazonUpdateAckRequest.Merchant = ActiveAmazonSettings.MWSMerchantID
            amazonUpdateAckRequest.Marketplace = ActiveAmazonSettings.MWSMarketplaceID
            amazonUpdateAckRequest.WithReportIdList(New IdList().WithId(ReportID))
            amazonUpdateAckRequest.Acknowledged = True
            ' set additional request parameters here if necessary
            If m_AmazonThrottling.CheckNotThrottleLimited(ActiveAmazonSettings.AmazonSite, "UpdateReportAck") Then ' TJS 03/07/13
                amazonUpdateAckResponse = MWSService.UpdateReportAcknowledgements(amazonUpdateAckRequest)
                If amazonUpdateAckResponse.IsSetUpdateReportAcknowledgementsResult() Then
                    amazonUpdateAckResult = amazonUpdateAckResponse.UpdateReportAcknowledgementsResult
                    If amazonUpdateAckResult.IsSetCount Then
                        iUpdateAckResultCount = amazonUpdateAckResult.Count
                    End If
                    reportInfoList = amazonUpdateAckResult.ReportInfo
                    For Each UpdateAckReportInfo In reportInfoList
                        ' following features of UpdateAckReportInfo not currently used
                        '    If UpdateAckReportInfo.IsSetReportId Then

                        '    End If
                        '    If UpdateAckReportInfo.IsSetReportType Then

                        '    End If
                        '    If UpdateAckReportInfo.IsSetReportType Then

                        '    End If
                        '    If UpdateAckReportInfo.IsSetReportRequestId Then

                        '    End If
                        '    If UpdateAckReportInfo.IsSetAvailableDate Then

                        '    End If
                        '    If UpdateAckReportInfo.IsSetAcknowledgedDate Then

                        '    End If
                        '    If UpdateAckReportInfo.IsSetAcknowledged Then

                        '    End If
                    Next
                End If
                If amazonUpdateAckResponse.IsSetResponseMetadata Then
                    amazonUpdateAckResponseMetadata = amazonUpdateAckResponse.ResponseMetadata
                    If amazonUpdateAckResponseMetadata.IsSetRequestId() Then
                        strUpdateAckRequestID = amazonUpdateAckResponseMetadata.RequestId
                    End If
                End If
                UpdateReportAcknowledgement = True

            End If

        Catch ex As Exception
            If ex.Message = "Request is throttled" Then ' TJS 03/07/13
                m_AmazonThrottling.SetThrottledFlag(ActiveAmazonSettings.AmazonSite, ActiveAmazonSettings.MerchantToken, "UpdateReportAck", strThrottlingDetails) ' TJS 03/07/13 TJS 05/07/13
                ErrorDetails = strThrottlingDetails ' TJS 03/07/13 TJS 05/07/13
            ElseIf ex.Message = "Access denied" Then ' TJS 05/07/13
                ErrorDetails = "Access denied for Merchant Account " & ActiveAmazonSettings.MerchantToken & " on Amazon" & ActiveAmazonSettings.AmazonSite & " - please check your Amazon config settings" ' TJS 05/07/13
            Else
                ErrorDetails = ex.Message & vbCrLf & ex.StackTrace ' TJS 05/07/13
            End If

        End Try

    End Function

    Private Function GetAmazonMWSURL(ByVal AmazonSite As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/07/11 | TJS             | 2011.1.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case AmazonSite
            Case ".com"
                GetAmazonMWSURL = "https://mws.amazonservices.com"

            Case ".co.uk"
                GetAmazonMWSURL = "https://mws.amazonservices.co.uk"

            Case ".ca"
                GetAmazonMWSURL = "https://mws.amazonservices.ca"

            Case ".de"
                GetAmazonMWSURL = "https://mws.amazonservices.de"

            Case ".fr"
                GetAmazonMWSURL = "https://mws.amazonservices.fr"

            Case ".jp"
                GetAmazonMWSURL = "https://mws.amazonservices.jp"

            Case ".com.cn"
                GetAmazonMWSURL = "https://mws.amazonservices.com.cn"

            Case Else
                GetAmazonMWSURL = ""
        End Select
    End Function

    Public Sub New(ByRef AmazonThrottling As Lerryn.Facade.ImportExport.AmazonMWSThrottling) ' TJS 03/07/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/07/11 | TJS             | 2011.1.00 | Function added
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        applicationVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion
        m_AmazonThrottling = AmazonThrottling ' TJS 03/07/13

    End Sub
End Class
