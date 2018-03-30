Imports System.IO
Imports System.Collections.Generic
Imports MarketplaceWebService
Imports MarketplaceWebService.Mock
Imports MarketplaceWebService.Model

Public Class Form1

    Private Const UKDeveloperID As String = "3794-7460-2011" ' Lerryn's Developer ID for  for Amazon.co.uk AWS
    Private Const UKAccessKeyID As String = "AKIAJGG4R57W4LFE5A7Q" ' eShopCONNECT AWS Access Key for Amazon.co.uk
    Private Const UKSecretAccessKey As String = "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" ' eShopCONNECT AWS Secret Key for Amazon.co.uk

    Private Const USDeveloperID As String = "2069-7890-7290" ' Lerryn's Developer ID for Amazon.com AWS
    Private Const USAccessKeyID As String = "AKIAJ4RRBW5VGLYGVW3Q" ' eShopCONNECT AWS Access Key for Amazon.com
    Private Const USSecretAccessKey As String = "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" ' eShopCONNECT AWS Secret Key for Amazon.com

    Private Const accessKeyId As String = "AKIAIEN4K2KM7UTONPEA" ' Option International AWS Access Key
    Private Const secretAccessKey As String = "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" ' Option International AWS Secret Key
    Private Const applicationName As String = "eShopCONNECT" ' eShopCONNECT Application Name
    Private applicationVersion As String = "12.2.00"

    Private AmazonURL As String = "https://mws.amazonservices.com"
    'Private merchantId As String = "A2A1GVUGQMPXDX" ' User's Merchant ID
    'Private marketplaceId As String = "A1F83G8C2ARO7P" ' User's Marketplace ID
    Private merchantId As String = "A19PO5VJQS3VWO" ' Option International Merchant ID
    Private marketplaceId As String = "ATVPDKIKX0DER" ' Option International Marketplace ID
    Private LogFilePath As String = "C:\eShopCONNECT\Logfiles\"

    Private Sub btnGetOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetOrders.Click

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonListRequest As MarketplaceWebService.Model.GetReportListRequest
        Dim amazonOrderRequest As MarketplaceWebService.Model.GetReportRequest
        Dim amazonListResponse As MarketplaceWebService.Model.GetReportListResponse
        Dim amazonOrderResponse As MarketplaceWebService.Model.GetReportResponse
        Dim amazonListResult As MarketplaceWebService.Model.GetReportListResult
        Dim amazonOrderResult As MarketplaceWebService.Model.GetReportResult
        Dim reportInfoList As List(Of ReportInfo)
        Dim amazonOrderResponseMetadata As ResponseMetadata
        Dim reportStream As MemoryStream, reportFileStream As FileStream
        Dim XMLFileName As String, strOrderMD5 As String, strOrderRequestID As String, strError As String

        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = AmazonURL
        amazonMWSConfig.SetUserAgentHeader(applicationName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, amazonMWSConfig)

        amazonListRequest = New MarketplaceWebService.Model.GetReportListRequest()

        amazonListRequest.Merchant = merchantId
        amazonListRequest.Marketplace = marketplaceId

        ' LJG - Setting this to false means "do not include reports that have been acknowledged"
        ' See here: (search for UpdateReportAcknowledgements )
        ' https://images-na.ssl-images-amazon.com/images/G/02/mwsportal/doc/bde/SOAP_to_Amazon_MWS_Migration_Guide._V180902225_.pdf
        ' More general object info here:
        ' http://rdoc.info/github/dmichael/amazon-mws/master/Amazon/MWS/Report  
        amazonListRequest.Acknowledged = False

        ' @TODO: set additional request parameters here
        '      GetReportListSample.InvokeGetReportList(service, request);
        Try
            amazonListResponse = amazonMWSService.GetReportList(amazonListRequest)
            If amazonListResponse.IsSetGetReportListResult Then
                amazonListResult = amazonListResponse.GetReportListResult
                ' following features of amazonSubmissionResponseResult not currently used
                'If amazonListResult.IsSetNextToken() Then

                'End If
                'If amazonListResult.IsSetHasNext() Then

                'End If
                reportInfoList = amazonListResult.ReportInfo
                For Each OrderReportInfo In reportInfoList
                    If OrderReportInfo.IsSetReportId() Then
                        amazonOrderRequest = New GetReportRequest()
                        amazonOrderRequest.Merchant = merchantId
                        amazonOrderRequest.Marketplace = marketplaceId
                        amazonOrderRequest.ReportId = OrderReportInfo.ReportId

                        ' Note that depending on the type of report being downloaded, a report can reach 
                        ' sizes greater than 1GB. For this reason we recommend that you _always_ program to
                        ' MWS in a streaming fashion. Otherwise, as your business grows you may silently reach
                        ' the in-memory size limit and have to re-work your solution.
                        ' NOTE: Due to Content-MD5 validation, the stream must be read/write.
                        XMLFileName = LogFilePath & "amazon_" & OrderReportInfo.ReportType & "_" & OrderReportInfo.ReportId
                        If File.Exists(XMLFileName & ".xml") Then
                            XMLFileName = XMLFileName & "_" & Format(Date.Now, "yyyy-MM-dd-hh-mm-ss")
                        End If
                        ' amazonOrderRequest.Report = System.IO.File.Open(XMLFileName & ".xml", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite)
                        reportStream = New MemoryStream
                        amazonOrderRequest.Report = reportStream
                        amazonOrderResponse = amazonMWSService.GetReport(amazonOrderRequest)
                        If amazonOrderResponse.IsSetGetReportResult() Then
                            amazonOrderResult = amazonOrderResponse.GetReportResult
                            If amazonOrderResult.IsSetContentMD5() Then
                                strOrderMD5 = amazonOrderResult.ContentMD5
                            End If
                        End If
                        If amazonOrderResponse.IsSetResponseMetadata() Then
                            amazonOrderResponseMetadata = amazonOrderResponse.ResponseMetadata
                            If amazonOrderResponseMetadata.IsSetRequestId() Then
                                strOrderRequestID = amazonOrderResponseMetadata.RequestId
                            End If
                        End If
                        reportFileStream = System.IO.File.Open(XMLFileName & ".xml", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite)
                        reportStream.WriteTo(reportFileStream)
                        amazonOrderRequest.Report.Close()
                        reportFileStream.Close()

                    End If
                Next
            End If
        Catch ex As Exception
            strError = ex.Message
        End Try

    End Sub

    Private Sub btnCheckStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckStatus.Click

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
        Dim reportStream As MemoryStream, reportFileStream As FileStream
        Dim strOrderMD5 As String, strOrderRequestID As String, strSubmissionResponseID As String
        Dim strError As String, XMLFileName As String

        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = AmazonURL
        amazonMWSConfig.SetUserAgentHeader(applicationName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, amazonMWSConfig)

        amazonSubmissionListRequest = New GetFeedSubmissionListRequest()

        amazonSubmissionListRequest.Merchant = merchantId
        amazonSubmissionListRequest.Marketplace = marketplaceId

        ' set additional request parameters here if necessary
        Try
            amazonSubmissionListResponse = amazonMWSService.GetFeedSubmissionList(amazonSubmissionListRequest)
            If amazonSubmissionListResponse.IsSetGetFeedSubmissionListResult Then
                amazonSubmissionListResult = amazonSubmissionListResponse.GetFeedSubmissionListResult
                ' following features of amazonSubmissionResponseResult not currently used
                'If amazonSubmissionResponseResult.IsSetNextToken Then

                'End If
                'If amazonSubmissionResponseResult.IsSetHasNext Then

                'End If
                SubmissionResponseInfoList = amazonSubmissionListResult.FeedSubmissionInfo
                For Each SubmissionResponseInfo In SubmissionResponseInfoList
                    If SubmissionResponseInfo.IsSetFeedSubmissionId Then
                        amazonSubmissionResultRequest = New GetFeedSubmissionResultRequest()
                        amazonSubmissionResultRequest.Merchant = merchantId
                        amazonSubmissionResultRequest.Marketplace = marketplaceId
                        amazonSubmissionResultRequest.FeedSubmissionId = SubmissionResponseInfo.FeedSubmissionId

                        ' Note that depending on the type of report being downloaded, a report can reach 
                        ' sizes greater than 1GB. For this reason we recommend that you _always_ program to
                        ' MWS in a streaming fashion. Otherwise, as your business grows you may silently reach
                        ' the in-memory size limit and have to re-work your solution.
                        ' NOTE: Due to Content-MD5 validation, the stream must be read/write.
                        XMLFileName = LogFilePath & "amazon_feedSubmissionResult_" & SubmissionResponseInfo.FeedSubmissionId
                        If File.Exists(XMLFileName & ".xml") Then
                            XMLFileName = XMLFileName & "_" & Format(Date.Now, "yyyy-MM-dd-hh-mm-ss")
                        End If
                        ' amazonSubmissionResultRequest.FeedSubmissionResult = File.Open("feedSubmissionResult.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        reportStream = New MemoryStream
                        amazonSubmissionResultRequest.FeedSubmissionResult = reportStream

                        amazonSubmissionResultResponse = amazonMWSService.GetFeedSubmissionResult(amazonSubmissionResultRequest)

                    End If
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
                Next
            End If
            If amazonSubmissionListResponse.IsSetResponseMetadata Then
                amazonSubmissionListMetadata = amazonSubmissionListResponse.ResponseMetadata
                If amazonSubmissionListMetadata.IsSetRequestId() Then
                    strSubmissionResponseID = amazonSubmissionListMetadata.RequestId
                End If
            End If

        Catch ex As Exception
            strError = ex.Message
        End Try

    End Sub

    Private Sub btnSendFeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendFeed.Click

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonSubmitFeedRequest As MarketplaceWebService.Model.SubmitFeedRequest
        Dim feedStream As MemoryStream, encoding As System.Text.UTF8Encoding
        Dim amazonSubmitFeedResponse As MarketplaceWebService.Model.SubmitFeedResponse
        Dim amazonSubmitFeedResult As MarketplaceWebService.Model.SubmitFeedResult
        Dim amazonFeedSubmissionInfo As MarketplaceWebService.Model.FeedSubmissionInfo
        Dim amazonFeedResponseMetadata As ResponseMetadata
        Dim byteFileContent As Byte()
        Dim strAmazonDocumemtID As String, strFeedRequestID As String, strError As String

        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = AmazonURL
        amazonMWSConfig.SetUserAgentHeader(applicationName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, amazonMWSConfig)

        amazonSubmitFeedRequest = New SubmitFeedRequest()

        amazonSubmitFeedRequest.Merchant = merchantId
        amazonSubmitFeedRequest.Marketplace = marketplaceId

        feedStream = New MemoryStream
        encoding = New System.Text.UTF8Encoding
        byteFileContent = encoding.GetBytes(Me.txtFeedContent.Text)
        feedStream.Write(byteFileContent, 0, byteFileContent.Length)
        amazonSubmitFeedRequest.FeedContent = feedStream
        amazonSubmitFeedRequest.FeedContent.Position = 0

        'amazonSubmitFeedRequest.FeedContent = System.IO.File.Open("feed.xml", System.IO.FileMode.Open, System.IO.FileAccess.Read)

        ' Calculating the MD5 hash value exhausts the stream, and therefore we must either reset the
        ' position, or create another stream for the calculation.
        amazonSubmitFeedRequest.ContentMD5 = MarketplaceWebServiceClient.CalculateContentMD5(amazonSubmitFeedRequest.FeedContent)
        amazonSubmitFeedRequest.FeedContent.Position = 0

        ' set additional request parameters here if necessary
        amazonSubmitFeedRequest.FeedType = "_POST_ORDER_ACKNOWLEDGEMENT_DATA_"

        Try
            amazonSubmitFeedResponse = amazonMWSService.SubmitFeed(amazonSubmitFeedRequest)
            If amazonSubmitFeedResponse.IsSetSubmitFeedResult Then
                amazonSubmitFeedResult = amazonSubmitFeedResponse.SubmitFeedResult
                If amazonSubmitFeedResult.IsSetFeedSubmissionInfo Then
                    amazonFeedSubmissionInfo = amazonSubmitFeedResult.FeedSubmissionInfo
                    If amazonFeedSubmissionInfo.IsSetFeedSubmissionId() Then
                        strAmazonDocumemtID = amazonFeedSubmissionInfo.FeedSubmissionId
                    End If
                End If
            End If
            If amazonSubmitFeedResponse.IsSetResponseMetadata Then
                amazonFeedResponseMetadata = amazonSubmitFeedResponse.ResponseMetadata
                If amazonFeedResponseMetadata.IsSetRequestId() Then
                    strFeedRequestID = amazonFeedResponseMetadata.RequestId
                End If
            End If

        Catch ex As Exception
            strError = ex.Message

        End Try


    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click

        Dim amazonMWSService As MarketplaceWebService.MarketplaceWebService
        Dim amazonMWSConfig As MarketplaceWebService.MarketplaceWebServiceConfig
        Dim amazonRequestReport As MarketplaceWebService.Model.RequestReportRequest
        Dim amazonRequestResponse As MarketplaceWebService.Model.RequestReportResponse
        Dim amazonRequestResult As MarketplaceWebService.Model.RequestReportResult
        Dim amazonRequestInfo As MarketplaceWebService.Model.ReportRequestInfo
        Dim amazonListRequest As MarketplaceWebService.Model.GetReportListRequest
        Dim amazonFileRequest As MarketplaceWebService.Model.GetReportRequest
        Dim amazonListResponse As MarketplaceWebService.Model.GetReportListResponse
        Dim amazonListResult As MarketplaceWebService.Model.GetReportListResult
        Dim amazonFileResponse As MarketplaceWebService.Model.GetReportResponse
        Dim amazonFileResult As MarketplaceWebService.Model.GetReportResult
        Dim reportInfoList As List(Of ReportInfo), FileReportInfo As ReportInfo
        Dim amazonFileResponseMetadata As ResponseMetadata
        Dim reportStream As MemoryStream, reportFileStream As FileStream, reader As StreamReader

        Dim strNextToken As String, strOrderMD5 As String, strOrderRequestID As String
        Dim strFileContent As String, strFileName As String, strFileLines As String()
        Dim strFieldNames As String(), strFieldValues As String()

        amazonMWSConfig = New MarketplaceWebService.MarketplaceWebServiceConfig

        amazonMWSConfig.ServiceURL = AmazonURL
        amazonMWSConfig.SetUserAgentHeader(applicationName, applicationVersion, "C#", "<Parameter 1>", "<Parameter 2>")

        amazonMWSService = New MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, amazonMWSConfig)

        amazonRequestReport = New MarketplaceWebService.Model.RequestReportRequest
        amazonRequestReport.Merchant = merchantId
        amazonRequestReport.Marketplace = marketplaceId
        amazonRequestReport.ReportType = "_GET_FLAT_FILE_OPEN_LISTINGS_DATA_" ' "_GET_MERCHANT_LISTINGS_DATA_"

        Try
            amazonRequestResponse = amazonMWSService.RequestReport(amazonRequestReport)
            If amazonRequestResponse.IsSetRequestReportResult Then
                amazonRequestResult = amazonRequestResponse.RequestReportResult
                amazonRequestInfo = amazonRequestResult.ReportRequestInfo
                If amazonRequestInfo.ReportProcessingStatus = "_SUBMITTED_" Then

                    amazonListRequest = New MarketplaceWebService.Model.GetReportListRequest()

                    amazonListRequest.Merchant = merchantId
                    amazonListRequest.Marketplace = marketplaceId

                    amazonListRequest.WithReportRequestIdList(New IdList().WithId(amazonRequestInfo.ReportRequestId))

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
                        For Each FileReportInfo In reportInfoList
                            If FileReportInfo.IsSetReportId Then
                                amazonFileRequest = New GetReportRequest()
                                amazonFileRequest.Merchant = merchantId
                                amazonFileRequest.Marketplace = marketplaceId
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

                                ' write file to manual preocessing directory
                                strFileName = "Amazon_" & FileReportInfo.ReportType & "_" & FileReportInfo.ReportId
                                If File.Exists("D:\Interprise\LerrynImportExport\Documentation\Amazon FBA\" & strFileName & ".txt") Then
                                    strFileName = strFileName & "_" & Format(Date.Now, "yyyy-MM-dd-hh-mm-ss")
                                End If
                                reportFileStream = System.IO.File.Open("D:\Interprise\LerrynImportExport\Documentation\Amazon FBA\" & strFileName & ".txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite)
                                reportStream.WriteTo(reportFileStream)
                                amazonFileRequest.Report.Close()
                                reportFileStream.Close()
                                reader.Close()
                                reader.Dispose()
                                strFileLines = strFileContent.Split(vbLf)
                                strFieldNames = strFileLines(0).Split(vbTab)
                                strFieldValues = strFileLines(1).Split(vbTab)
                            End If
                        Next
                    End If
                End If



            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
