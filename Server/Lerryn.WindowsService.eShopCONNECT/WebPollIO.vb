' eShopCONNECT for Connected Business - Windows Service
' Module: WebPollIO.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 01 May 2014

Imports Lerryn.Framework.ImportExport.Shared.Const ' TJS 28/11/09
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst ' TJS 31/12/09
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports System.IO ' TJS 15/10/09
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 02/12/11
Imports System.Xml.Serialization ' TJS 10/12/09
Imports System.Xml ' TJS 10/12/09
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Module WebPollIO

    Private bEbayAuthTokenExpiryEMailSent As Boolean = False
    Private dteEbayAuthTokenExpiryEMailSent As Date = Nothing
    Friend AmazonConnection As Lerryn.Facade.ImportExport.AmazonMWSConnector = Nothing ' TJS 02/08/13

    Public Function PollForWebIO() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Poll relevant sources for data, both upload and download
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/08/09 | TJS             | 2009.3.03 | Function added
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to support direct connection to Amazon
        ' 10/12/09 | TJS             | 2009.3.09 | Modified to check for responses to files submitted to amazon
        ' 17/12/09 | TJS             | 2009.3.10 | Modified to correct size of Channel Advisor sReceivedOrderIds array
        ' 31/12/09 | TJs             | 2010.0.00 | Modified to use direct SOAP connection to Channel Advisor
        ' 07/01/10 | TJS             | 2010.0.02 | Modified to pass Channel Advisor ClientOrderIdentifier 
        '                                        | instead of OrderID in SetOrdersExportStatus
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to check activation in case it expired while service running 
        ' 13/06/10 | TJS             | 2010.0.07 | Modified to mask Channel Advisor Dev Pwd in error messages
        '                                        | and to detect dupliate downlaod of files from Amazon
        ' 19/08/10 | TJS             | 2010.1.00 | Added Magento and ASLDotNetStoreFront code and corrected InhibitWebPosts initialisation
        '                                        | plus Siurce COde constants
        ' 22/09/10 | TJs             | 2010.1.01 | Modified to cater for Offline Channel Advisor checkout
        ' 04/10/10 | TJS             | 2010.1.05 | Modified setting of LastOrderStatusUpdate for Channel ADvisor, removed Import as Order 
        '                                        | option from Channel ADvisor ActionIfNoPayment config setting and added check
        '                                        | for orders not matching requested criteria from Channel Advisor
        ' 15/11/10 | FA              | 2010.1.08 | Removed CA unexpected order message as too many were being sent and CA said to ignore these
        ' 23/11/10 | FA              | 2010.1.09 | made additions of milliseconds to xmldate calculation optional - True for web input, false for status update
        ' 07/12/10 | FA              | 2010.1.11 | Time stamp moved to beginning of sequence rather than after cases 1, 2, 3.  
        '                                          Think this may be causing import files to be missed on status updates
        ' 21/01/11 | FA              | 2010.1.15 | Added logging of order timestamp update
        ' 04/02/11 | FA              | 2010.1.18 | Modified logging as this no of entries was making logfiles unmanageable
        ' 04/02/11 | FA              | 2010.1.19 | added logfile for xml submitted to CA
        ' 04/02/11 | FA              | 2010.1.20 | Above change commented out to keep logfile manageable
        ' 22/02/11 | TJS             | 2010.1.22 | Modified Channel Advisor timing
        ' 28/02/11 | FA              | 2010.1.23 | Initialise dteChannelAdvisorTimestamp to the last order update, otherwise never set
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to use Magento SOAP Connector, ASPStorefront Connector and modified Channel Advisor timing
        ' 29/03/11 | TJS             | 2011.0.04 | SPlit code to use separate poll routines for each connector
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Modified for Sears.com connector 
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ActiveSource As SourceSettings
        Dim bReturnValue As Boolean, bInhibitWebPosts As Boolean ' TJS 31/12/09 TJS 13/06/10

        Try
            bReturnValue = False ' TJE 10/12/09
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") ' TJs 13/06/10 TJS 19/08/10

            ' are there any sources with active web polling ?
            For Each ActiveSource In Setts.ActiveSources
                If ActiveSource.SourceCode = VOLUSION_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then ' TJS 13/04/10 TJS 19/08/10
                    bReturnValue = PollVolusion(ActiveSource) ' TJS 29/03/11

                    ' start of Code Added TJS 15/10/09
                ElseIf ActiveSource.SourceCode = AMAZON_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then ' TJS 13/04/10 TJS 19/08/10
                    bReturnValue = PollAmazon(ActiveSource, bInhibitWebPosts) ' TJS 29/03/11

                    ' start of Code Added TJS 10/12/09
                ElseIf ActiveSource.SourceCode = CHANNEL_ADVISOR_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then ' TJS 13/04/10 TJS 19/08/10
                    bReturnValue = PollChannelAdvisor(ActiveSource, bInhibitWebPosts) ' TJS 29/03/11

                    ' start of code added TJS 19/08/10
                ElseIf ActiveSource.SourceCode = MAGENTO_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                    bReturnValue = PollMagento(ActiveSource) ' TJS 29/03/11

                ElseIf ActiveSource.SourceCode = ASP_STORE_FRONT_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then ' TJS 19/08/10
                    bReturnValue = PollASPStorefront(ActiveSource) ' TJS 29/03/11
                    ' end of code added TJS 19/08/10

                ElseIf ActiveSource.SourceCode = EBAY_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                    bReturnValue = PollEBay(ActiveSource) ' TJS 02/12/11

                ElseIf ActiveSource.SourceCode = SEARS_COM_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                    bReturnValue = PollSearsCom(ActiveSource) ' TJS 16/01/12

                ElseIf ActiveSource.SourceCode = THREE_D_CART_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then ' TJS 20/11/13
                    bReturnValue = Poll3DCart(ActiveSource, bInhibitWebPosts) ' TJS 20/11/13

                End If
                If bShutDownInProgress Then ' TJS 02/08/13
                    Exit For ' TJS 02/08/13
                End If
            Next
            Return bReturnValue ' TJS 28/11/09

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "PollForWebIO", ex)
            Return False ' TJS 28/11/09
        End Try

    End Function

    Private Function PollAmazon(ByRef ActiveSource As SourceSettings, ByVal InhibitWebPosts As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls Amazon for files and sends files to Amazon
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created using code from PollForWebIO
        ' 06/04/11 | TJS             | 2011.0.08 | Disabled Amazon client to allow dll signing so Web SErvices will work
        ' 09/07/11 | TJS             | 2011.1.00 | Replaced with code for Amazon MWS connection
        ' 02/12/11 | TJS             | 2011.2.00 | Added check for Amazon site settings
        ' 02/08/12 | TJS             | 2012.1.11 | Modified to prevent errors if no response content from AMazon
        ' 26/06/13 | TJS             | 2013.1.23 | Added code to retry orders which failed to import 
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row ' TJS 26/06/13
        Dim rowAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row ' TJS 26/06/13
        Dim iMerchantLoop As Integer, iFileLoop As Integer, bReturnValue As Boolean

        bReturnValue = False
        ' Start of Code Replaced TJS 09/07/11
        ' check each Merchant ID in turn
        For iMerchantLoop = 0 To ActiveSource.AmazonSettingCount - 1
            ' is next connection due, are account settings present and account not disabled ?
            If ActiveSource.AmazonSettings(iMerchantLoop).NextConnectionTime <= Date.Now And ActiveSource.AmazonSettings(iMerchantLoop).AmazonSite <> "" And _
                ActiveSource.AmazonSettings(iMerchantLoop).MWSMerchantID <> "" And ActiveSource.AmazonSettings(iMerchantLoop).MWSMarketplaceID <> "" And _
                Not ActiveSource.AmazonSettings(iMerchantLoop).AccountDisabled Then ' TJS 02/12/11
                ' yes, have we inhibited sending files to amazon ? 
                If Not InhibitWebPosts Then
                    ' no, send any files ready for sending to amazon
                    AmazonConnection = New Lerryn.Facade.ImportExport.AmazonMWSConnector(m_AmazonThrottling) ' TJS 03/07/13
                    AmazonConnection.SendFilesToAmazon(m_ImportExportConfigFacade, m_ImportExportDataset, PRODUCT_NAME, ActiveSource, ActiveSource.AmazonSettings(iMerchantLoop), InhibitWebPosts)
                    AmazonConnection = Nothing

                Else
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending files to amazon" & ActiveSource.AmazonSettings(iMerchantLoop).AmazonSite)
                End If

                ' check Amazon for submission responses
                AmazonConnection = New Lerryn.Facade.ImportExport.AmazonMWSConnector(m_AmazonThrottling) ' TJS 03/07/13
                AmazonConnection.PollAmazonForDocumentStatus(m_ImportExportConfigFacade, m_ImportExportDataset, PRODUCT_NAME, ActiveSource, ActiveSource.AmazonSettings(iMerchantLoop), InhibitWebPosts)

                ' now poll amazon for any new files
                AmazonConnection.PollAmazonForFiles(m_ImportExportConfigFacade, m_ImportExportDataset, PRODUCT_NAME, ActiveSource, ActiveSource.AmazonSettings(iMerchantLoop), InhibitWebPosts)
                AmazonConnection = Nothing

                ' start of code added TJS 28/11/09
                ' set next Amazon poll time
                ActiveSource.AmazonSettings(iMerchantLoop).NextConnectionTime = Date.Now.AddMinutes(15)

                ' now process received files
                For iFileLoop = 0 To m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1
                    ' is file one that was sent to Amazon and we now have a response that has not been processed ?
                    If Not m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileIsInputFromAmazon_DEV000221 And _
                         m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseReceived_DEV000221 And _
                         Not m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).Processed_DEV000221 Then
                        ' yes, Is there any response content (_GET_MERCHANT_LISTINGS_DATA_ responses are not saved as they can be hugh > 50MB) ?
                        If Not m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).IsResponseContent_DEV000221Null Then ' TJS 02/08/12
                            ' yes, process response
                            ProcessAmazonXML(ActiveSource, ActiveSource.AmazonSettings(iMerchantLoop), m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).ResponseContent_DEV000221, m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221, m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop), Nothing) ' TJS 19/08/10 TJS 26/06/13
                            m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).Processed_DEV000221 = True

                        ElseIf m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).DateCreated < Date.Now.AddDays(-7) Then ' TJS 02/08/12
                            ' report was requested over a week ago, mark as processed to prevent it clogging up the system
                            m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).Processed_DEV000221 = True ' TJS 02/08/12

                        End If

                    ElseIf m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileIsInputFromAmazon_DEV000221 And _
                         Not m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).Processed_DEV000221 Then
                        ' no, it's a new file from amazon for processing
                        rowOrderToRetry = Nothing ' TJS 26/06/13
                        ProcessAmazonXML(ActiveSource, ActiveSource.AmazonSettings(iMerchantLoop), m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).FileContent_DEV000221, m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).AmazonDocumentID_DEV000221, Nothing, rowOrderToRetry) ' TJS 19/08/10 TJS 26/06/13
                        RecordOrderIDToRetry(rowOrderToRetry, ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken, "Amazon") ' TJS 26/06/13
                        m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221(iFileLoop).Processed_DEV000221 = True

                    End If
                    m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                        "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                        "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                        "Update Amazon File processed", False)
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
                bReturnValue = True
                ' end of code added TJS 28/11/09
            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        ' End of Code Replaced TJS 09/07/11

        ' start of code added TJS 26/06/13
        ' loop through the Amazon web site list
        For iMerchantLoop = 0 To ActiveSource.AmazonSettingCount - 1
            ' has an API URL been set and account not disabled ?
            If ActiveSource.AmazonSettings(iMerchantLoop).AmazonSite <> "" And ActiveSource.AmazonSettings(iMerchantLoop).MWSMerchantID <> "" And _
                ActiveSource.AmazonSettings(iMerchantLoop).MWSMarketplaceID <> "" And Not ActiveSource.AmazonSettings(iMerchantLoop).AccountDisabled Then
                ' are there any orders which failed to import that are due for a retry ?
                m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                    "ReadLerrynImportExportSourceOrdersToRetry_DEV000221", AT_SOURCE_CODE, AMAZON_SOURCE_CODE, AT_STORE_MERCHANT_ID, ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.Count > 0 Then
                    ' yes
                    For Each rowOrderToRetry In m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221
                        rowAmazonFile = m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.FindByAmazonDocumentID_DEV000221MerchantID_DEV000221SiteCode_DEV000221(rowOrderToRetry.SourceFileRecordID_DEV000221, ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken, ActiveSource.AmazonSettings(iMerchantLoop).AmazonSite)
                        If rowAmazonFile Is Nothing Then
                            m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                                "ReadLerrynImportExportAmazonFiles_DEV000221", AT_AMAZON_DOCUMENT_ID, rowOrderToRetry.SourceFileRecordID_DEV000221, _
                                AT_MERCHANT_ID, ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken, AT_SITE_ID, ActiveSource.AmazonSettings(iMerchantLoop).AmazonSite}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)
                            rowAmazonFile = m_ImportExportDataset.LerrynImportExportAmazonFiles_DEV000221.FindByAmazonDocumentID_DEV000221MerchantID_DEV000221SiteCode_DEV000221(rowOrderToRetry.SourceFileRecordID_DEV000221, ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken, ActiveSource.AmazonSettings(iMerchantLoop).AmazonSite)
                            If rowAmazonFile Is Nothing Then
                                m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "PollAmazon", "Failed to read Amazon file for Amazon Document ID " & rowOrderToRetry.SourceFileRecordID_DEV000221 & ", Merchant Token " & ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken & ", site " & ActiveSource.AmazonSettings(iMerchantLoop).AmazonSite, "")
                                Continue For
                            End If
                        End If
                        ProcessAmazonXML(ActiveSource, ActiveSource.AmazonSettings(iMerchantLoop), rowAmazonFile.FileContent_DEV000221, rowAmazonFile.AmazonDocumentID_DEV000221, Nothing, rowOrderToRetry)
                        If bShutDownInProgress Then ' TJS 02/08/13
                            Exit For ' TJS 02/08/13
                        End If
                    Next
                    m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                         "CreateLerrynImportExportSourceOrdersToRetry_DEV000221", "UpdateLerrynImportExportSourceOrdersToRetry_DEV000221", _
                         "DeleteLerrynImportExportSourceOrdersToRetry_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                         "Updated Amazon Orders to Retry", False)
                End If
            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        ' end of code added TJS 26/06/13

        Return bReturnValue

    End Function

    Private Function PollASPStorefront(ByRef ActiveSource As SourceSettings) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls ASPDotNetStorefront for orders and updates to Orders
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created using code from PollForWebIO
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to check API URL is set
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ASPStorefrontConnection As Lerryn.Facade.ImportExport.ASPStorefrontConnector ' TJS 18/03/11
        Dim XMLResponse As XDocument
        Dim strSubmit As String, strErrorDetails As String, iMerchantLoop As Integer

        ' loop through the ASPDotNetStoreFront web site list
        For iMerchantLoop = 0 To ActiveSource.ASPStoreFrontSettingCount - 1
            ' is next connection due and account not disabled and account details are entered ?
            If ActiveSource.ASPStoreFrontSettings(iMerchantLoop).NextOrderPollTime <= Date.Now And Not ActiveSource.ASPStoreFrontSettings(iMerchantLoop).AccountDisabled And _
                ActiveSource.ASPStoreFrontSettings(iMerchantLoop).SiteID <> "" And ActiveSource.ASPStoreFrontSettings(iMerchantLoop).APIURL <> "" Then ' TJS 25/04/11
                ' yes, poll for orders
                strSubmit = "<AspDotNetStorefrontImport Verbose=""false""><Get Table=""Orders"" Name=""NewOrders"">"
                strSubmit = strSubmit & "<XmlPackage>DumpOrder.xml.config</XmlPackage><OrderBy>OrderDate asc</OrderBy>"
                strSubmit = strSubmit & "<Criteria IsNew=""1""/></Get></AspDotNetStorefrontImport>"

                ASPStorefrontConnection = New Lerryn.Facade.ImportExport.ASPStorefrontConnector ' TJS 18/03/11
                strErrorDetails = "" ' TJS 18/03/11
                XMLResponse = ASPStorefrontConnection.SendXMLToASPStorefront(ActiveSource.ASPStoreFrontSettings(iMerchantLoop).UseWSE3Authentication, _
                    ActiveSource.ASPStoreFrontSettings(iMerchantLoop).APIURL, ActiveSource.ASPStoreFrontSettings(iMerchantLoop).APIUser, ActiveSource.ASPStoreFrontSettings(iMerchantLoop).APIPwd, strSubmit, strErrorDetails) ' TJS 18/03/11
                ' any errors ?
                If strErrorDetails = "" Then ' TJS 18/03/11
                    ' no, process orders
                    ProcessASPStoreFrontXML(ActiveSource, ActiveSource.ASPStoreFrontSettings(iMerchantLoop), XMLResponse)

                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollASPStorefront", strErrorDetails) ' TJS 18/03/11 TJS 13/11/13
                End If
                ASPStorefrontConnection = Nothing ' TJS 18/03/11

                ActiveSource.ASPStoreFrontSettings(iMerchantLoop).NextOrderPollTime = Date.Now.AddMinutes(ActiveSource.ASPStoreFrontSettings(iMerchantLoop).OrderPollIntervalMinutes)
            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        Return True

    End Function

    Private Function PollChannelAdvisor(ByRef ActiveSource As SourceSettings, ByVal InhibitWebPosts As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls Channel Advisor for orders and updates to Orders
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created using code from PollForWebIO and modified 
        '                                        | to use separate timestamp for checking Payment failure updates
        ' 04/04/11 | TJS             | 2011.0.07 | Removed timestamp for checking Payment failure updates
        '                                        | and modified Order status timestamp to always be 1 hour ago
        ' 08/06/11 | TJS             | 2011.0.15 | Modified to comment out log print of Last Order Status Update Time
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/08/12 | TJS             | 2012.1.13 | Modified to detect Soap errors and include account name in error messages
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        ' 04/01/14 | TJS             | 2013.4.03 | Added missing namespace manager reference
        ' 02/04/14 | TJS             | 2014.0.01 | Corrected content sent and checking of element count in SetOrdersExportStatus response
        ' 01/05/14 | TJS             | 2014.0.02 | Added code to retry orders which failed to import 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim rowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row ' TJS 01/05/14
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim XMLTemp As XDocument, XMLOrderNode As XElement
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLResults As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLCAOrderIDs As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLNSManCAOrders As System.Xml.XmlNamespaceManager, XMLNameTabCAOrders As System.Xml.NameTable
        Dim strReturn As String, sUpdateXML As String, strReturnDetail As String, strSubmit As String
        Dim sTemp As String, iResultsLoop As Integer, iMerchantLoop As Integer, iChanAdvStatusLoop As Integer
        Dim iPageFilter As Integer, iOrderPosn As Integer, bOrderMeetsFilterCriteria As Boolean
        Dim bReturnValue As Boolean, bXMLError As Boolean, bPollError As Boolean ' TJS 01/05/14

        'Const dateFormat As String = "dd/MM/yyyy hh:mm:ss.fffffff"

        bReturnValue = False
        ' check each Merchant ID in turn
        For iMerchantLoop = 0 To ActiveSource.ChannelAdvSettingCount - 1
            ' is next connection due and account not disabled and account details are entered ?
            If ActiveSource.ChannelAdvSettings(iMerchantLoop).NextConnectionTime <= Date.Now And Not ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountDisabled And _
                ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID <> "" Then ' TJS 19/08/10
                ' yes, initialise page filter for first order block
                ' start of changes TJS 31/12/09

                ' TJS 04/04/11 Removed setting of CA update timestamps as CA cache makes it unworkable

                Dim bLimitReached As Boolean = False

                For iChanAdvStatusLoop = 1 To 8 ' TJS 19/08/10 TJS 22/09/10
                    ' ignore orders without payments or with failed payments if ImportAsQuoteIfNoPayment not set
                    If (ActiveSource.ChannelAdvSettings(iMerchantLoop).ActionIfNoPayment <> "Ignore" And _
                        (iChanAdvStatusLoop = 4 Or iChanAdvStatusLoop = 5)) Or _
                        (iChanAdvStatusLoop <> 4 And iChanAdvStatusLoop <> 5) Then ' TJS 19/08/10 TJS 04/10/10
                        sUpdateXML = ""
                        strReturnDetail = "" ' TJS 19/08/10
                        iPageFilter = 1
                        ' loop until no more orders
                        Do
                            ' only get orders created in last 30 days with check-out completed, Cleared payments and not already shipped 
                            strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                            strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/"" xmlns:ord=""http://api.channeladvisor.com/datacontracts/orders"">"
                            If "" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperKey <> "" Then
                                strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperKey
                                strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd
                            Else
                                strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY
                                strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                            End If
                            strSubmit = strSubmit & "</web:Password></web:APICredentials></soapenv:Header><soapenv:Body><web:GetOrderList><web:accountID>"
                            strSubmit = strSubmit & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID & "</web:accountID><web:orderCriteria>"
                            strSubmit = strSubmit & "<ord:DetailLevel>High</ord:DetailLevel>"
                            If iChanAdvStatusLoop = 1 Or iChanAdvStatusLoop = 4 Or iChanAdvStatusLoop = 5 Then ' TJS 18/03/11
                                strSubmit = strSubmit & "<ord:OrderCreationFilterBeginTimeGMT>" & ChannelAdvXMLDate(Date.Today.AddDays(-30), True) ' TJS 04/10/10 FA 23/11/10 18/03/11
                                strSubmit = strSubmit & "</ord:OrderCreationFilterBeginTimeGMT>"

                            Else
                                strSubmit = strSubmit & "<ord:StatusUpdateFilterBeginTimeGMT>" & ChannelAdvXMLDate(ActiveSource.ChannelAdvSettings(iMerchantLoop).LastOrderStatusUpdate, True) ' TJS 19/08/10 FA 23/11/10
                                strSubmit = strSubmit & "</ord:StatusUpdateFilterBeginTimeGMT>"
                            End If
                            Select Case iChanAdvStatusLoop ' TJS 19/08/10
                                Case 1 ' TJS 19/08/10
                                    ' get active orders created in last 30 days with check-out completed, Cleared payments and not already shipped or exported
                                    strSubmit = strSubmit & "<ord:OrderStateFilter>Active</ord:OrderStateFilter><ord:PaymentStatusFilter>Cleared</ord:PaymentStatusFilter>" ' TJS 19/08/10
                                    strSubmit = strSubmit & "<ord:CheckoutStatusFilter>Completed</ord:CheckoutStatusFilter><ord:ShippingStatusFilter>Unshipped"
                                    strSubmit = strSubmit & "</ord:ShippingStatusFilter><ord:ExportState>NotExported</ord:ExportState>"

                                Case 2 ' TJS 19/08/10
                                    ' get any order with payment status of Cleared and not already shipped - CA doesn't have a filter to only return orders already exported 
                                    ' but since we only get changes since last poll (see Update Filter above), this means we should find orders that changed in the meantime
                                    strSubmit = strSubmit & "<ord:OrderStateFilter>Active</ord:OrderStateFilter><ord:PaymentStatusFilter>Cleared</ord:PaymentStatusFilter>" ' TJS 19/08/10
                                    strSubmit = strSubmit & "<ord:CheckoutStatusFilter>Completed</ord:CheckoutStatusFilter><ord:ShippingStatusFilter>Unshipped" ' TJS 19/08/10
                                    strSubmit = strSubmit & "</ord:ShippingStatusFilter>" ' TJS 19/08/10

                                Case 3 ' TJS 22/09/10
                                    ' get any order with payment status of Cleared and not already shipped - CA doesn't have a filter to only return orders already exported 
                                    ' but since we only get changes since last poll (see Update Filter above), this means we should find orders that changed in the meantime
                                    ' this is the same as 2 above but with Checkout completed offline
                                    strSubmit = strSubmit & "<ord:OrderStateFilter>Active</ord:OrderStateFilter><ord:PaymentStatusFilter>Cleared</ord:PaymentStatusFilter>" ' TJS 22/09/10
                                    strSubmit = strSubmit & "<ord:CheckoutStatusFilter>CompletedOffline</ord:CheckoutStatusFilter><ord:ShippingStatusFilter>Unshipped" ' TJS 22/09/10
                                    strSubmit = strSubmit & "</ord:ShippingStatusFilter>" ' TJS 22/09/10

                                Case 4 ' TJS 19/08/10 TJS 22/09/10
                                    ' get active orders created in last 30 days with payment not submitted and not already shipped or exported - checkout status ignored
                                    strSubmit = strSubmit & "<ord:OrderStateFilter>Active</ord:OrderStateFilter><ord:PaymentStatusFilter>NotSubmitted</ord:PaymentStatusFilter>" ' TJS 19/08/10
                                    strSubmit = strSubmit & "<ord:ShippingStatusFilter>Unshipped</ord:ShippingStatusFilter><ord:ExportState>NotExported</ord:ExportState>" ' TJS 19/08/10

                                Case 5 ' TJS 19/08/10 TJS 22/09/10
                                    ' get active orders created in last 30 days with payment failed and not already shipped or exported - checkout status ignored
                                    strSubmit = strSubmit & "<ord:OrderStateFilter>Active</ord:OrderStateFilter><ord:PaymentStatusFilter>Failed</ord:PaymentStatusFilter>" ' TJS 19/08/10
                                    strSubmit = strSubmit & "<ord:ShippingStatusFilter>Unshipped</ord:ShippingStatusFilter><ord:ExportState>NotExported</ord:ExportState>" ' TJS 19/08/10

                                Case 6 ' TJS 19/08/10 TJS 22/09/10
                                    ' get any order with payment status of Failed and not already shipped - only return orders already exported 
                                    ' but since we only get changes since last poll (see Update Filter above), this means we should find orders that changed in the meantime
                                    strSubmit = strSubmit & "<ord:PaymentStatusFilter>Failed</ord:PaymentStatusFilter><ord:ShippingStatusFilter>Unshipped</ord:ShippingStatusFilter>" ' TJS 19/08/10
                                    strSubmit = strSubmit & "<ord:ExportState>Exported</ord:ExportState>" ' TJS 18/03/11

                                Case 7 ' TJS 19/08/10 TJS 22/09/10
                                    ' get any order with payment status of Failed and has been shipped - only return orders already exported 
                                    ' but since we only get changes since last poll (see Update Filter above), this means we should find orders that changed in the meantime
                                    strSubmit = strSubmit & "<ord:PaymentStatusFilter>Failed</ord:PaymentStatusFilter><ord:ShippingStatusFilter>Shipped</ord:ShippingStatusFilter>" ' TJS 19/08/10
                                    strSubmit = strSubmit & "<ord:ExportState>Exported</ord:ExportState>" ' TJS 18/03/11

                                Case 8 ' TJS 19/08/10 TJS 22/09/10
                                    ' get any order with payment status of Failed and has been partially shipped - only return orders already exported 
                                    ' but since we only get changes since last poll (see Update Filter above), this means we should find orders that changed in the meantime
                                    strSubmit = strSubmit & "<ord:PaymentStatusFilter>Failed</ord:PaymentStatusFilter><ord:ShippingStatusFilter>PartiallyShipped</ord:ShippingStatusFilter>" ' TJS 19/08/10
                                    strSubmit = strSubmit & "<ord:ExportState>Exported</ord:ExportState>" ' TJS 18/03/11

                            End Select
                            strSubmit = strSubmit & "<ord:PageNumberFilter>" & iPageFilter & "</ord:PageNumberFilter><ord:PageSize>20</ord:PageSize>"
                            strSubmit = strSubmit & "</web:orderCriteria></web:GetOrderList></soapenv:Body></soapenv:Envelope>"

                            ' start of code replaced TJS 18/07/11
                            Try
                                bPollError = False ' TJS 01/05/14
                                WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_ORDER_SERVICE_URL)
                                WebSubmit.Method = "POST"
                                WebSubmit.ContentType = "text/xml; charset=utf-8"
                                WebSubmit.ContentLength = strSubmit.Length
                                WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/GetOrderList")
                                WebSubmit.Timeout = 30000

                                byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                                ' send to Channel Advisor
                                postStream = WebSubmit.GetRequestStream()
                                postStream.Write(byteData, 0, byteData.Length)

                                WebResponse = WebSubmit.GetResponse
                                reader = New StreamReader(WebResponse.GetResponseStream())
                                strReturn = reader.ReadToEnd()

                                'Remember when the first call was made and every after 1 hour interval. This will be used as reference when 1000 call per hour limit is reached.
                                If (Not ActiveSource.ChannelAdvSettings(iMerchantLoop).FirstGetOrderListCall.HasValue) _
                                OrElse DateTime.Now.Subtract(ActiveSource.ChannelAdvSettings(iMerchantLoop).FirstGetOrderListCall.Value).TotalHours > 1 Then
                                    ActiveSource.ChannelAdvSettings(iMerchantLoop).FirstGetOrderListCall = DateTime.Now
                                End If

                                If strReturn <> "" Then
                                    If strReturn.Length > 38 Then
                                        If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Or strReturn.Trim.Substring(0, 14).ToLower = "<soap:envelope" Then ' TJS 14/08/12
                                            bXMLError = False
                                            Try
                                                ' had difficulty getting XPath to read XML with this name space present so remove it
                                                XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))

                                            Catch ex As Exception
                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Order XML from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")) ' TJS 19/08/10 TJS 14/08/12 TJS 13/11/13
                                                bXMLError = True
                                                Exit Do ' TJS 19/08/10

                                            End Try

                                            ' did XML load correctly
                                            If Not bXMLError Then
                                                ' yes, was response a success ?
                                                XMLNameTabCAOrders = New System.Xml.NameTable ' TJS 02/12/11
                                                XMLNSManCAOrders = New System.Xml.XmlNamespaceManager(XMLNameTabCAOrders) ' TJS 02/12/11
                                                XMLNSManCAOrders.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                                                XMLNSManCAOrders.AddNamespace("q1", "http://api.channeladvisor.com/datacontracts/orders") ' TJS 02/12/11
                                                If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/GetOrderListResponse/GetOrderListResult/Status", XMLNSManCAOrders) = "Success" Then
                                                    ' yes, process orders

                                                    XMLOrderList = XMLResponse.XPathSelectElements(CHANNEL_ADV_ORDER_LIST, XMLNSManCAOrders) ' TJS 19/08/10
                                                    If m_ImportExportConfigFacade.GetXMLElementListCount(XMLOrderList) > 0 Then
                                                        iOrderPosn = 1
                                                        For Each XMLOrderNode In XMLOrderList
                                                            XMLTemp = XDocument.Parse(XMLOrderNode.ToString.Replace("/q" & iOrderPosn & ":", "/q1:").Replace("<q" & iOrderPosn & ":", "<q1:").Replace("xmlns:q" & iOrderPosn, "xmlns:q1"))
                                                            ' does order meet requested CA filter criteria (CA sometimes returns incorrect criteria e.g. checkout NotVisited when Cleared was requested)
                                                            ' start of code added TJS 04/10/10
                                                            bOrderMeetsFilterCriteria = True
                                                            Select Case iChanAdvStatusLoop
                                                                Case 1
                                                                    ' active orders created in last 30 days with check-out completed, Cleared payments and not already shipped or exported
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderState", XMLNSManCAOrders).ToLower <> "active" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:CheckoutStatus", XMLNSManCAOrders).ToLower <> "completed" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "cleared" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "unshipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                                Case 2
                                                                    ' active orders with payment status of Cleared and not already shipped - CA doesn't have a filter to only return orders already exported 
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderState", XMLNSManCAOrders).ToLower <> "active" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:CheckoutStatus", XMLNSManCAOrders).ToLower <> "completed" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "cleared" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "unshipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                                Case 3
                                                                    ' orders with payment status of Cleared and not already shipped - CA doesn't have a filter to only return orders already exported 
                                                                    ' this is the same as 2 above but with Checkout completed offline
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderState", XMLNSManCAOrders).ToLower <> "active" Or _
                                                                       m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:CheckoutStatus", XMLNSManCAOrders).ToLower <> "completedoffline" Or _
                                                                       m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "cleared" Or _
                                                                       m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "unshipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                                Case 4
                                                                    ' active orders created in last 30 days with payment not submitted and not already shipped or exported - checkout status ignored
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderState", XMLNSManCAOrders).ToLower <> "active" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "notsubmitted" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "unshipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                                Case 5
                                                                    ' active orders created in last 30 days with payment failed and not already shipped or exported - checkout status ignored
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderState", XMLNSManCAOrders).ToLower <> "active" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "failed" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "unshipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                                Case 6
                                                                    ' any order with payment status of Failed and not already shipped - CA doesn't have a filter to only return orders already exported 
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "failed" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "unshipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                                Case 7
                                                                    ' any order with payment status of Failed and has been shipped- CA doesn't have a filter to only return orders already exported 
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "failed" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "shipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                                Case 8
                                                                    ' any order with payment status of Failed and has been partially shipped - CA doesn't have a filter to only return orders already exported 
                                                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower <> "failed" Or _
                                                                        m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower <> "partiallyshipped" Then
                                                                        bOrderMeetsFilterCriteria = False
                                                                    End If

                                                            End Select
                                                            ' end of code added TJS 04/10/10
                                                            If bOrderMeetsFilterCriteria Then ' TJS 04/10/10
                                                                rowOrderToRetry = Nothing ' TJS 01/05/14
                                                                Select Case iChanAdvStatusLoop ' TJS 19/08/10
                                                                    Case 1, 2, 3 ' TJS 19/08/10 TJS 22/09/10
                                                                        ' active orders created in last 30 days with check-out completed, Cleared payments and not already shipped 
                                                                        ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Order", rowOrderToRetry) ' TJS 19/08/10 TJS 04/10/10 TJS 18/03/11 TJS 04/04/11


                                                                    Case 4, 5 ' TJS 19/08/10 TJS 22/09/10
                                                                        ' active orders created in last 30 days with payment failed and not already shipped or exported - checkout status ignored
                                                                        ' or active orders created in last 30 days with payment not submitted and not already shipped or exported - checkout status ignored
                                                                        If ActiveSource.ChannelAdvSettings(iMerchantLoop).ActionIfNoPayment = "Import as Quote" Then ' TJS 19/08/10
                                                                            ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Quote", rowOrderToRetry) ' TJS 19/08/10 TJS 04/10/10 TJS 18/03/11 TJS 04/04/11
                                                                        End If

                                                                    Case 6, 7, 8 ' TJS 19/08/10 TJS 22/09/10
                                                                        ' any order with payment status of Failed and any shipment status 
                                                                        If ActiveSource.ChannelAdvSettings(iMerchantLoop).ActionIfNoPayment = "Import as Quote" Then ' TJS 19/08/10
                                                                            ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Quote", rowOrderToRetry) ' TJS 19/08/10 TJS 04/10/10 TJS 18/03/11 TJS 04/04/11
                                                                        Else
                                                                            ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Cancel", rowOrderToRetry) ' TJS 19/08/10 TJS 04/10/10 TJS 18/03/11 TJS 04/04/11
                                                                        End If

                                                                End Select
                                                                RecordOrderIDToRetry(rowOrderToRetry, ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID, "Channel Advisor") ' TJS 01/05/14

                                                                sUpdateXML = sUpdateXML & "<web:int>" & m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderID", XMLNSManCAOrders) & "</web:int>" ' TJS 19/08/10 TJS 02/04/14
                                                                strReturnDetail = strReturnDetail & "<Success>true</Success>" ' TJS 19/08/10 TJS 01/05/14

                                                            Else
                                                                'FA 15/11/10 removed as was causing too many error messages to be sent
                                                                'If ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd <> "" Then ' TJS 04/10/10
                                                                '    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Channel Advisor poll of account " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName & " (" & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID & ") returned unexpected orders" & vbCrLf & vbCrLf & "Request XML - " & strSubmit.Replace(ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd, "********") & vbCrLf & vbCrLf & "Order XML - " & XMLTemp.xml) ' TJS 04/10/10
                                                                'Else
                                                                '    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Channel Advisor poll of account " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName & " (" & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID & ") returned unexpected orders" & vbCrLf & vbCrLf & "Request XML - " & strSubmit.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********") & vbCrLf & vbCrLf & "Order XML - " & XMLTemp.xml) ' TJS 04/10/10
                                                                'End If
                                                            End If
                                                            iOrderPosn += 1
                                                            If bShutDownInProgress Then ' TJS 02/08/13
                                                                Exit For ' TJS 02/08/13
                                                            End If
                                                        Next
                                                    Else
                                                        Exit Do
                                                    End If

                                                ElseIf XMLResponse.XPathSelectElement("soap:Envelope/soap:Body/soap:Fault/faultstring", XMLNSManCAOrders) IsNot Nothing Then ' TJS 14/08/12
                                                    If ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd <> "" Then ' TJS 14/08/12
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & _
                                                            " was " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/soap:Fault/faultcode", XMLNSManCAOrders) & ", " & _
                                                            m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/soap:Fault/faultstring", XMLNSManCAOrders), strSubmit.Replace(ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd, "********")) ' TJS 14/08/12
                                                    Else
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & _
                                                            " was " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/soap:Fault/faultcode", XMLNSManCAOrders) & ", " & _
                                                            m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/soap:Fault/faultstring", XMLNSManCAOrders), strSubmit.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********")) ' TJS 14/08/12
                                                    End If
                                                    Exit Do ' TJS 14/08/12

                                                Else
                                                    If ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd <> "" Then ' TJS 19/08/10
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & " not a success message - " & strReturn, strSubmit.Replace(ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd, "********")) ' TJS 19/08/10 TJS 14/08/12
                                                    Else
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & " not a success message - " & strReturn, strSubmit.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********")) ' TJS 19/08/10 TJS 14/08/12
                                                    End If
                                                    Exit Do ' TJS 19/08/10
                                                End If
                                            End If
                                        Else
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                            Exit Do
                                        End If

                                    Else
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                        Exit Do
                                    End If

                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                    Exit Do
                                End If

                            Catch ex As Exception
                                'Handle error due to call limit exceeded.
                                Dim strLimitMessage = String.Empty
                                If (ex.Message IsNot Nothing AndAlso ex.Message.Contains("(429) Too Many Requests.")) Then
                                    bLimitReached = True
                                    If ActiveSource.ChannelAdvSettings(iMerchantLoop).FirstGetOrderListCall.HasValue Then
                                        Dim orig As DateTime = ActiveSource.ChannelAdvSettings(iMerchantLoop).FirstGetOrderListCall.Value
                                        ActiveSource.ChannelAdvSettings(iMerchantLoop).NextConnectionTime = orig.AddHours(1)
                                    Else
                                        ActiveSource.ChannelAdvSettings(iMerchantLoop).NextConnectionTime = DateTime.Now.AddMinutes(15)
                                    End If
                                    strLimitMessage = ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName & " next GetOrderList call schedule is " & ActiveSource.ChannelAdvSettings(iMerchantLoop).NextConnectionTime.ToString()
                                End If

                                ' send error details but mask developer password
                                If ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd <> "" Then ' TJS 19/08/10
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Unable to poll Channel Advisor for orders at " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " - " & ex.Message & ", " & strLimitMessage & ", " & ex.StackTrace, strSubmit.Replace(ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd, "********")) ' TJS 13/06/10 TJS 19/08/10
                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Unable to poll Channel Advisor for orders at " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " - " & ex.Message & ", " & strLimitMessage & ", " & ex.StackTrace, strSubmit.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********")) ' TJS 13/06/10 TJS 19/08/10
                                End If
                                bPollError = True ' TJS 01/05/14

                            Finally
                                If Not postStream Is Nothing Then postStream.Close()
                                If Not WebResponse Is Nothing Then WebResponse.Close()

                            End Try
                            ' end of code replaced TJS 02/12/11

                            If bPollError Then ' TJS 01/05/14
                                Exit Do ' TJS 01/05/14
                            End If

                            iPageFilter = iPageFilter + 1
                            If iPageFilter > 1000 Then
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Order Service has returned more then 1000 pages of orders from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL, strReturn)
                                Exit Do

                            End If
                            If bShutDownInProgress Then ' TJS 02/08/13
                                Exit Do ' TJS 02/08/13
                            End If
                        Loop
                        If sUpdateXML <> "" Then
                            strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                            strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/"">"
                            If "" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperKey <> "" Then
                                strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperKey
                                strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd
                            Else
                                strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY
                                strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                            End If
                            strSubmit = strSubmit & "</web:Password></web:APICredentials></soapenv:Header><soapenv:Body><web:SetOrdersExportStatus><web:accountID>"
                            strSubmit = strSubmit & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID & "</web:accountID><web:orderIDList>"
                            strSubmit = strSubmit & sUpdateXML & "</web:orderIDList><web:markAsExported>true</web:markAsExported>"
                            strSubmit = strSubmit & "</web:SetOrdersExportStatus></soapenv:Body></soapenv:Envelope>"

                            ' start of code replaced TJS 18/07/11
                            Try
                                If Not InhibitWebPosts Then
                                    WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_ORDER_SERVICE_URL)
                                    WebSubmit.Method = "POST"
                                    WebSubmit.ContentType = "text/xml; charset=utf-8"
                                    WebSubmit.ContentLength = strSubmit.Length
                                    WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/SetOrdersExportStatus")
                                    WebSubmit.Timeout = 30000

                                    byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                                    ' send to Channel Advisor
                                    postStream = WebSubmit.GetRequestStream()
                                    postStream.Write(byteData, 0, byteData.Length)

                                    WebResponse = WebSubmit.GetResponse
                                    reader = New StreamReader(WebResponse.GetResponseStream())
                                    strReturn = reader.ReadToEnd()
                                    ' start of code added TJS 19/08/10
                                Else
                                    If ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd <> "" Then ' TJS 19/08/10
                                        m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor Set Orders Export Status - Inhibited from sending update - content " & strSubmit.Replace(ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd, "********")) ' TJS 19/08/10
                                    Else
                                        m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor Set Orders Export Status - Inhibited from sending update - content " & strSubmit.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********")) ' TJS 19/08/10
                                    End If
                                    strReturn = "<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                    strReturn = strReturn & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                                    strReturn = strReturn & "<soap:Body><SetOrdersExportStatusResponse xmlns=""http://api.channeladvisor.com/webservices/"">"
                                    strReturn = strReturn & "<SetOrdersExportStatusResult><Status>Success</Status><MessageCode>0</MessageCode><ResultData>" ' TJS 09/06/10
                                    strReturn = strReturn & "<SetExportStatusResponse>" & strReturnDetail & "</SetExportStatusResponse></ResultData>" ' TJS 01/05/15
                                    strReturn = strReturn & "</SetOrdersExportStatusResult></SetOrdersExportStatusResponse></soap:Body></soap:Envelope>"
                                    ' end of code added TJS 19/08/10
                                End If


                            Catch ex As Exception
                                ' send error details but mask developer password
                                If ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd <> "" Then ' TJS 19/08/10
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Unable to post Channel Advisor SetOrdersExportStatus XML to " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " - " & ex.Message & ", " & ex.StackTrace, strSubmit.Replace(ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd, "********")) ' TJS 13/06/10 TJS 19/08/10
                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Unable to post Channel Advisor SetOrdersExportStatus XML to " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " - " & ex.Message & ", " & ex.StackTrace, strSubmit.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********")) ' TJS 13/06/10 TJS 19/08/10
                                End If

                            Finally
                                If Not postStream Is Nothing Then postStream.Close()
                                If Not WebResponse Is Nothing Then WebResponse.Close()

                            End Try

                            If strReturn <> "" Then
                                If strReturn.Length > 38 Then
                                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                        ' had difficulty getting XPath to read XML with this name space present so remove it
                                        XMLTemp = XDocument.Parse(strSubmit.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", "")) ' TJS 02/04/14
                                        ' had difficulty getting XPath to read XML with this name space present so remove it
                                        XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))
                                        ' check response status
                                        XMLNSManCAOrders.RemoveNamespace("q1", "http://api.channeladvisor.com/datacontracts/orders") ' TJS 02/12/11
                                        XMLNSManCAOrders.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                                        XMLNSManCAOrders.AddNamespace("web", "http://api.channeladvisor.com/webservices/") ' TJS 02/12/11
                                        If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_ORDER_EXPORT_STATUS & "Status", XMLNSManCAOrders).ToLower = "success" Then
                                            ' response is success, get results list
                                            XMLResults = XMLResponse.XPathSelectElements(CHANNEL_ADV_STATUS_RESPONSE_SET_ORDER_EXPORT_STATUS & "ResultData/SetExportStatusResponse/Success", XMLNSManCAOrders) ' TJS 02/04/14 TJS 01/05/14
                                            ' get Original Channel Advisor Order IDs from update XML
                                            XMLCAOrderIDs = XMLTemp.XPathSelectElements(CHANNEL_ADV_STATUS_SET_ORDER_EXPORT_STATUS & "web:int", XMLNSManCAOrders) ' TJS 02/04/14
                                            ' is number of orders updated the same as the number of orders in update message ?
                                            If m_ImportExportConfigFacade.GetXMLElementListCount(XMLResults) = m_ImportExportConfigFacade.GetXMLElementListCount(XMLCAOrderIDs) Then
                                                ' yes - check if all orders updated
                                                sTemp = ""
                                                For iResultsLoop = 0 To m_ImportExportConfigFacade.GetXMLElementListCount(XMLResults) - 1
                                                    Try
                                                        XMLTemp = XDocument.Parse(XMLResults(iResultsLoop).ToString)
                                                        If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "SetExportStatusResponse/Success", XMLNSManCAOrders).ToLower = "false" Then ' TJS 04/01/14 TJS 02/04/14
                                                            If sTemp <> "" Then
                                                                sTemp = sTemp & ", "
                                                            End If
                                                            XMLTemp = XDocument.Parse(XMLCAOrderIDs(iResultsLoop).ToString)
                                                            sTemp = sTemp & "Channel Advisor Order ID " & m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "web:int", XMLNSManCAOrders) ' TJS 02/04/14
                                                        End If

                                                    Catch ex As Exception
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Channel Advisor SetOrdersExportStatus response could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))
                                                    End Try
                                                Next
                                                If sTemp <> "" Then
                                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Channel Advisor SetOrdersExportStatus failed to update " & sTemp, strSubmit)
                                                End If
                                            Else
                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Channel Advisor SetOrdersExportStatus response result count different from update count", strReturn)
                                            End If
                                        Else
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Update rejected, Status Code " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_ORDER_EXPORT_STATUS & "Status", XMLNSManCAOrders) & ", message " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, CHANNEL_ADV_STATUS_RESPONSE_SET_ORDER_EXPORT_STATUS & "Message", XMLNSManCAOrders), XMLResponse.ToString) ' TJS 04/01/14
                                        End If

                                    End If
                                End If
                            End If
                        End If
                        bReturnValue = True
                        ' end of changes TJS 31/12/09
                    End If
                    If bLimitReached Then
                        Exit For
                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
                ' end of Code Added TJS 10/12/09
                ActiveSource.ChannelAdvSettings(iMerchantLoop).LastOrderStatusUpdate = Date.Now.AddHours(-1) ' TJS 18/03/11 TJS 04/04/11
            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        ' save last status update time
        sTemp = "<eShopCONNECTConfig>" ' TJS 19/08/10
        For iMerchantLoop = 0 To ActiveSource.ChannelAdvSettingCount - 1 ' TJS 19/08/10
            sTemp = sTemp & "<ChannelAdvisor><AccountID>" & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID & "</AccountID>" ' TJS 19/08/10
            sTemp = sTemp & "<LastOrderStatusUpdate>" & CreateXMLDate(ActiveSource.ChannelAdvSettings(iMerchantLoop).LastOrderStatusUpdate)
            sTemp = sTemp & "</LastOrderStatusUpdate></ChannelAdvisor>" ' TJS 29/03/11 TJS 04/04/11
            'm_ImportExportConfigFacade.WriteLogProgressRecord(PRODUCT_NAME, "Last Order Status Update Time for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & " is now " & ActiveSource.ChannelAdvSettings(iMerchantLoop).LastOrderStatusUpdate.ToString(dateFormat))  ' FA 21/01/11 TJS 08/06/11
        Next
        sTemp = sTemp & "</eShopCONNECTConfig>" ' TJS 19/08/10
        m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE LerrynImportExportConfig_DEV000221 SET LastOrderStatusUpdate_DEV000221 = '" & _
            sTemp & "' WHERE SourceCode_DEV000221 = '" & ActiveSource.SourceCode & "'", Nothing) ' TJS 19/08/10

        ' start of code added TJS 01/05/14
        ' check each Merchant ID in turn
        For iMerchantLoop = 0 To ActiveSource.ChannelAdvSettingCount - 1
            ' account not disabled and account details are entered ?
            If Not ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountDisabled And ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID <> "" Then
                ' are there any orders which failed to import that are due for a retry ?
                m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                    "ReadLerrynImportExportSourceOrdersToRetry_DEV000221", AT_SOURCE_CODE, CHANNEL_ADVISOR_SOURCE_CODE, AT_STORE_MERCHANT_ID, ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.Count > 0 Then
                    ' yes, get orders
                    For Each rowOrderToRetry In m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221
                        strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                        strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/"" xmlns:ord=""http://api.channeladvisor.com/datacontracts/orders"">"
                        If "" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperKey <> "" Then
                            strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperKey
                            strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd
                        Else
                            strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY
                            strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                        End If
                        strSubmit = strSubmit & "</web:Password></web:APICredentials></soapenv:Header><soapenv:Body><web:GetOrderList><web:accountID>"
                        strSubmit = strSubmit & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID & "</web:accountID><web:orderCriteria>"
                        strSubmit = strSubmit & "<ord:DetailLevel>High</ord:DetailLevel><ord:OrderIDList><ord:int>" & rowOrderToRetry.MerchantOrderID_DEV000221
                        strSubmit = strSubmit & "</ord:int></ord:OrderIDList><ord:PageNumberFilter>" & iPageFilter & "</ord:PageNumberFilter>"
                        strSubmit = strSubmit & "<ord:PageSize>20</ord:PageSize></web:orderCriteria></web:GetOrderList></soapenv:Body></soapenv:Envelope>"
                        Try
                            WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_ORDER_SERVICE_URL)
                            WebSubmit.Method = "POST"
                            WebSubmit.ContentType = "text/xml; charset=utf-8"
                            WebSubmit.ContentLength = strSubmit.Length
                            WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/GetOrderList")
                            WebSubmit.Timeout = 30000

                            byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                            ' send to Channel Advisor
                            postStream = WebSubmit.GetRequestStream()
                            postStream.Write(byteData, 0, byteData.Length)

                            WebResponse = WebSubmit.GetResponse
                            reader = New StreamReader(WebResponse.GetResponseStream())
                            strReturn = reader.ReadToEnd()

                            If strReturn <> "" Then
                                If strReturn.Length > 38 Then
                                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Or strReturn.Trim.Substring(0, 14).ToLower = "<soap:envelope" Then
                                        bXMLError = False
                                        Try
                                            ' had difficulty getting XPath to read XML with this name space present so remove it
                                            XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))

                                        Catch ex As Exception
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Order XML from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))
                                            bXMLError = True
                                            Exit For

                                        End Try

                                        ' did XML load correctly
                                        If Not bXMLError Then
                                            ' yes, was response a success ?
                                            XMLNameTabCAOrders = New System.Xml.NameTable
                                            XMLNSManCAOrders = New System.Xml.XmlNamespaceManager(XMLNameTabCAOrders)
                                            XMLNSManCAOrders.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/")
                                            XMLNSManCAOrders.AddNamespace("q1", "http://api.channeladvisor.com/datacontracts/orders")
                                            If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/GetOrderListResponse/GetOrderListResult/Status", XMLNSManCAOrders) = "Success" Then
                                                ' yes, process orders
                                                ' need to check status 
                                                XMLOrderList = XMLResponse.XPathSelectElements(CHANNEL_ADV_ORDER_LIST, XMLNSManCAOrders)
                                                If m_ImportExportConfigFacade.GetXMLElementListCount(XMLOrderList) > 0 Then
                                                    iOrderPosn = 1
                                                    For Each XMLOrderNode In XMLOrderList
                                                        XMLTemp = XDocument.Parse(XMLOrderNode.ToString.Replace("/q" & iOrderPosn & ":", "/q1:").Replace("<q" & iOrderPosn & ":", "<q1:").Replace("xmlns:q" & iOrderPosn, "xmlns:q1"))
                                                        If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderState", XMLNSManCAOrders).ToLower = "active" And _
                                                            (m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:CheckoutStatus", XMLNSManCAOrders).ToLower = "completed" Or _
                                                            m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:CheckoutStatus", XMLNSManCAOrders).ToLower = "completedoffline") And _
                                                            m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower = "cleared" And _
                                                            m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower = "unshipped" Then
                                                            ' active order with check-out completed, Cleared payments and not already shipped
                                                            ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Order", rowOrderToRetry)

                                                        ElseIf m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_DETAILS & "q1:OrderState", XMLNSManCAOrders).ToLower = "active" And _
                                                            (m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower = "notsubmitted" Or _
                                                            m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower = "failed") And _
                                                            m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower = "unshipped" Then
                                                            ' active order with payment not submitted or failed and not already shipped or exported - checkout status ignored
                                                            If ActiveSource.ChannelAdvSettings(iMerchantLoop).ActionIfNoPayment = "Import as Quote" Then
                                                                ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Quote", rowOrderToRetry)
                                                            End If

                                                        ElseIf m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:PaymentStatus", XMLNSManCAOrders).ToLower = "failed" And _
                                                            (m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower = "unshipped" Or _
                                                             m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower = "shipped" Or _
                                                             m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, CHANNEL_ADV_ORDER_STATUS & "q1:ShippingStatus", XMLNSManCAOrders).ToLower = "partiallyshipped") Then
                                                            ' any order with payment status of Failed 
                                                            If ActiveSource.ChannelAdvSettings(iMerchantLoop).ActionIfNoPayment = "Import as Quote" Then
                                                                ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Quote", rowOrderToRetry)
                                                            Else
                                                                ProcessChannelAdvXML(ActiveSource, ActiveSource.ChannelAdvSettings(iMerchantLoop), XMLTemp, XMLNSManCAOrders, "Cancel", rowOrderToRetry)
                                                            End If
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        End If
                                    Else
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " for " & ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountName.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                        Exit For
                                    End If

                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                    Exit For
                                End If

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Response from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                Exit For
                            End If

                        Catch ex As Exception
                            ' send error details but mask developer password
                            If ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd <> "" Then
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Unable to poll Channel Advisor for order to retry at " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " - " & ex.Message & ", " & ex.StackTrace, strSubmit.Replace(ActiveSource.ChannelAdvSettings(iMerchantLoop).OwnDeveloperPwd, "********"))
                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollChannelAdvisor", "Unable to poll Channel Advisor for order to retry at " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " - " & ex.Message & ", " & ex.StackTrace, strSubmit.Replace(CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD, "********"))
                            End If

                        Finally
                            If Not postStream Is Nothing Then postStream.Close()
                            If Not WebResponse Is Nothing Then WebResponse.Close()

                        End Try
                    Next
                    m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                         "CreateLerrynImportExportSourceOrdersToRetry_DEV000221", "UpdateLerrynImportExportSourceOrdersToRetry_DEV000221", _
                         "DeleteLerrynImportExportSourceOrdersToRetry_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                         "Updated Magento Orders to Retry", False)

                End If
            End If
            If bShutDownInProgress Then
                Exit For
            End If
        Next
        ' end of code added TJS 01/05/14

        Return bReturnValue

    End Function

    Private Function PollMagento(ByRef ActiveSource As SourceSettings) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls Magento for orders and updates to Orders
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created using code from PollForWebIO
        ' 31/03/11 | TJS             | 2011.0.05 | Modified to trap login issues as source errors
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2#
        ' 26/03/12 | TJS             | 2011.2.11 | Added code to retry orders which failed to import 
        ' 10/06/12 | TJS             | 2012.1.05 | Moved recording of orders to retry to RecordOrderIDToRetry
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to cater for Magento login potentially containing XML entities
        ' 15/04/13 | TJS             | 2013.1.10 | Added code to detect session timeouts etc
        ' 28/05/13 | TJS             | 2013.1.17 | Modified to inhibit Last Order Status Time Update if 
        '                                        | unable to read one or more orders from Magento 
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to decode Config XML Value such as password etc 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2SoapAPIWSICompliant and 
        '                                        | corrected error message call to remove product name
        ' 04/01/14 | TJS             | 2013.4.03 | Modified to initialise the Magento V2SoapAPIWSICompliant 
        '                                        | and API Version settings when retrying failed orders
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim MagentoConnection As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim rowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row ' TJS 26/03/12
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLResponse As XDocument, XMLTemp As XDocument, XMLMember As XDocument
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLMagentoMembersList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLOrderNode As XElement, XMLMagentoMember As XElement
        Dim sTemp As String, iMerchantLoop As Integer, bLoggedIn As Boolean ' TJS 31/03/11
        Dim bLastOrderStatusTimeChanged As Boolean, bAlreadyImported As Boolean, dteOrderTimestamp As Date
        Dim bInhibitLastOrderStatusTimeUpdate As Boolean ' TJS 28/05/13

        bLastOrderStatusTimeChanged = False
        ' create Magento connector
        MagentoConnection = New Lerryn.Facade.ImportExport.MagentoSOAPConnector() ' TJS 18/03/11
        ' loop through the Magento web site list
        For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
            ' has an API URL been set and account not disabled ?
            If ActiveSource.MagentoSettings(iMerchantLoop).APIURL <> "" And Not ActiveSource.MagentoSettings(iMerchantLoop).AccountDisabled Then ' TJS 07/01/11
                ' yes, has next poll time been reached ?
                If ActiveSource.MagentoSettings(iMerchantLoop).NextOrderPollTime <= Date.Now Then
                    ' yes, login
                    MagentoConnection.V2SoapAPIWSICompliant = ActiveSource.MagentoSettings(iMerchantLoop).V2SoapAPIWSICompliant ' TJS 13/11/13
                    MagentoConnection.MagentoVersion = ActiveSource.MagentoSettings(iMerchantLoop).MagentoVersion ' TJS 13/11/13
                    MagentoConnection.LerrynAPIVersion = ActiveSource.MagentoSettings(iMerchantLoop).LerrynAPIVersion ' TJS 13/11/13
                    Try ' TJS 31/03/11
                        bLoggedIn = MagentoConnection.Login(m_ImportExportConfigFacade.DecodeConfigXMLValue(ActiveSource.MagentoSettings(iMerchantLoop).APIURL), _
                            m_ImportExportConfigFacade.ConvertEntitiesForXML(m_ImportExportConfigFacade.DecodeConfigXMLValue(ActiveSource.MagentoSettings(iMerchantLoop).APIUser)), _
                            m_ImportExportConfigFacade.ConvertEntitiesForXML(m_ImportExportConfigFacade.DecodeConfigXMLValue(ActiveSource.MagentoSettings(iMerchantLoop).APIPwd))) ' TJS 31/03/11 TJS 03/04/13 TJS 19/09/13

                    Catch ex As Exception
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollMagento", "Unable to log in to Magento - URL " & ActiveSource.MagentoSettings(iMerchantLoop).APIURL & ", user " & ActiveSource.MagentoSettings(iMerchantLoop).APIUser & ", password *********") ' TJS 31/03/11 TJS 13/11/13

                    End Try
                    If bLoggedIn Then ' TJS 31/03/11
                        ' get list of new orders
                        If MagentoConnection.GetSalesOrderList(ActiveSource.MagentoSettings(iMerchantLoop).LastOrderStatusUpdate) Then
                            dteOrderTimestamp = ActiveSource.MagentoSettings(iMerchantLoop).LastOrderStatusUpdate
                            bInhibitLastOrderStatusTimeUpdate = False ' TJS 28/05/13
                            XMLResponse = XDocument.Parse(MagentoConnection.ReturnedXML.ToString)
                            XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                            XMLOrderList = XMLResponse.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                            For Each XMLOrderNode In XMLOrderList
                                XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                                XMLMagentoMembersList = XMLTemp.XPathSelectElements("item/item")
                                ' find order ID
                                For Each XMLMagentoMember In XMLMagentoMembersList
                                    XMLMember = XDocument.Parse(XMLMagentoMember.ToString)
                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLMember, "item/key").ToLower = "increment_id" Then
                                        If MagentoConnection.GetSalesOrderDetail(m_ImportExportConfigFacade.GetXMLElementText(XMLMember, "item/value")) Then
                                            bAlreadyImported = False ' TJS 18/03/11
                                            rowOrderToRetry = Nothing ' TJS 26/03/12
                                            ProcessMagentoXML(ActiveSource, ActiveSource.MagentoSettings(iMerchantLoop), MagentoConnection.ReturnedXML.ToString, dteOrderTimestamp, bAlreadyImported, MagentoConnection, rowOrderToRetry) ' TJS 18/03/11 TJS 26/03/12 TJS 04/01/14
                                            RecordOrderIDToRetry(rowOrderToRetry, ActiveSource.MagentoSettings(iMerchantLoop).InstanceID, "Magento") ' TJS 10/06/12
                                            ' did we only get 1 order which has already been imported ?
                                            If m_ImportExportConfigFacade.GetXMLElementListCount(XMLOrderList) = 1 And bAlreadyImported Then ' TJS 18/03/11
                                                ' yes, add 2 seconds to timestamp so we don't get order again because 
                                                ' we now know there no other orders with timestamps similar to this order
                                                ActiveSource.MagentoSettings(iMerchantLoop).LastOrderStatusUpdate = ActiveSource.MagentoSettings(iMerchantLoop).LastOrderStatusUpdate.AddSeconds(2) ' TJS 18/03/11
                                                bLastOrderStatusTimeChanged = True

                                            ElseIf dteOrderTimestamp > ActiveSource.MagentoSettings(iMerchantLoop).LastOrderStatusUpdate Then
                                                ' no, must have some new orders with order timestamp later than previous Last Order Status Update time 
                                                ' use it but subtract 1 second to make sure we don't miss any orders due to rounding
                                                ' as Magento doesn't send milliseconds in timesstamps
                                                ActiveSource.MagentoSettings(iMerchantLoop).LastOrderStatusUpdate = dteOrderTimestamp.AddSeconds(-1)
                                                bLastOrderStatusTimeChanged = True
                                            End If
                                        Else
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollMagento", MagentoConnection.LastError, "") ' TJS 10/06/12
                                            ' stop Last Order Status Time being updated so missed orders get imported next time
                                            bInhibitLastOrderStatusTimeUpdate = True ' TJS 28/05/13
                                        End If
                                        Exit For ' TJS 18/03/11
                                    End If
                                    If bShutDownInProgress Then ' TJS 02/08/13
                                        Exit For ' TJS 02/08/13
                                    End If
                                Next
                                ' start of code added TJS 15/04/13
                                ' check we haven't been logged out by a previous error
                                If Not MagentoConnection.LoggedIn Then
                                    Try
                                        bLoggedIn = MagentoConnection.Login(ActiveSource.MagentoSettings(iMerchantLoop).APIURL, _
                                            m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveSource.MagentoSettings(iMerchantLoop).APIUser), _
                                            m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveSource.MagentoSettings(iMerchantLoop).APIPwd))
                                        If Not bLoggedIn Then
                                            Exit For
                                        End If
                                    Catch ex As Exception
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollMagento", "Unable to log in to Magento - URL " & ActiveSource.MagentoSettings(iMerchantLoop).APIURL & ", user " & ActiveSource.MagentoSettings(iMerchantLoop).APIUser & ", password *********") ' TJS 13/11/13
                                        Exit For
                                    End Try
                                End If
                                ' end of code added TJS 15/04/13
                                If bShutDownInProgress Then ' TJS 02/08/13
                                    Exit For ' TJS 02/08/13
                                End If
                            Next
                        Else
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollMagento", MagentoConnection.LastError, "") ' TJS 10/06/12
                        End If
                        MagentoConnection.Logout()
                    Else
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollMagento", MagentoConnection.LastError, "") ' TJS 18/03/11
                    End If
                    ActiveSource.MagentoSettings(iMerchantLoop).NextOrderPollTime = Date.Now.AddMinutes(ActiveSource.MagentoSettings(iMerchantLoop).OrderPollIntervalMinutes)
                End If
            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next

        ' have any Last Order Status Update times changed and updates are not inhibited due to problems reading order details ?
        If bLastOrderStatusTimeChanged And Not bInhibitLastOrderStatusTimeUpdate Then ' TJS 28/05/13
            ' yes, save them
            sTemp = "<eShopCONNECTConfig>"
            For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
                sTemp = sTemp & "<Magento><InstanceID>" & ActiveSource.MagentoSettings(iMerchantLoop).InstanceID & "</InstanceID>" ' TJS 02/12/11
                sTemp = sTemp & "<LastOrderStatusUpdate>" & CreateXMLDate(ActiveSource.MagentoSettings(iMerchantLoop).LastOrderStatusUpdate)
                sTemp = sTemp & "</LastOrderStatusUpdate></Magento>"
            Next
            sTemp = sTemp & "</eShopCONNECTConfig>"
            m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE LerrynImportExportConfig_DEV000221 SET LastOrderStatusUpdate_DEV000221 = '" & _
                sTemp & "' WHERE SourceCode_DEV000221 = '" & ActiveSource.SourceCode & "'", Nothing)
        End If

        ' start of code added TJS 26/03/12
        ' loop through the Magento web site list
        For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
            ' has an API URL been set and account not disabled ?
            If ActiveSource.MagentoSettings(iMerchantLoop).APIURL <> "" And Not ActiveSource.MagentoSettings(iMerchantLoop).AccountDisabled Then
                ' are there any orders which failed to import that are due for a retry ?
                m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                    "ReadLerrynImportExportSourceOrdersToRetry_DEV000221", AT_SOURCE_CODE, MAGENTO_SOURCE_CODE, AT_STORE_MERCHANT_ID, ActiveSource.MagentoSettings(iMerchantLoop).InstanceID}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.Count > 0 Then
                    ' yes, login
                    MagentoConnection.V2SoapAPIWSICompliant = ActiveSource.MagentoSettings(iMerchantLoop).V2SoapAPIWSICompliant ' TJS 04/01/14
                    MagentoConnection.MagentoVersion = ActiveSource.MagentoSettings(iMerchantLoop).MagentoVersion ' TJS 04/01/14
                    MagentoConnection.LerrynAPIVersion = ActiveSource.MagentoSettings(iMerchantLoop).LerrynAPIVersion ' TJS 04/01/14
                    Try
                        ' www.dynenttech.com davidonelson 5/4/2018
                        ' This call was missing the ConvertEntitiesForXML for the uid and pwd, and this was causing improper password to be passsed to SOAP xml
                        bLoggedIn = MagentoConnection.Login(ActiveSource.MagentoSettings(iMerchantLoop).APIURL, m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveSource.MagentoSettings(iMerchantLoop).APIUser), m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveSource.MagentoSettings(iMerchantLoop).APIPwd))

                    Catch ex As Exception
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollMagento", "Unable to log in to Magento - URL " & ActiveSource.MagentoSettings(iMerchantLoop).APIURL & ", user " & ActiveSource.MagentoSettings(iMerchantLoop).APIUser & ", password *********")

                    End Try
                    If bLoggedIn Then
                        For Each rowOrderToRetry In m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221
                            If MagentoConnection.GetSalesOrderDetail(rowOrderToRetry.MerchantOrderID_DEV000221) Then
                                bAlreadyImported = False
                                ProcessMagentoXML(ActiveSource, ActiveSource.MagentoSettings(iMerchantLoop), MagentoConnection.ReturnedXML.ToString, dteOrderTimestamp, bAlreadyImported, MagentoConnection, rowOrderToRetry) ' TJS 04/01/14
                            End If
                        Next
                        m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                             "CreateLerrynImportExportSourceOrdersToRetry_DEV000221", "UpdateLerrynImportExportSourceOrdersToRetry_DEV000221", _
                             "DeleteLerrynImportExportSourceOrdersToRetry_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                             "Updated Magento Orders to Retry", False)
                        MagentoConnection.Logout()
                    Else
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "OpenMagentoConnection", MagentoConnection.LastError, "")
                    End If
                End If
            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        MagentoConnection = Nothing
        ' end of code added TJS 26/03/12

        Return True

    End Function

    Private Function PollVolusion(ByRef ActiveSource As SourceSettings) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls Volusion for orders and updates to Orders
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created using code from PollForWebIO
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 04/01/14 | TJS             | 2013.4.03 | Added code to retry orders which failed to import and removed erroneous code to send POST data
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row ' TJS 04/01/14
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing ' TJS 02/12/11
        Dim reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strReturn As String, iMerchantLoop As Integer, bReturnValue As Boolean

        bReturnValue = False
        ' loop through the Volusion web site list
        For iMerchantLoop = 0 To ActiveSource.VolusionSettingCount - 1
            ' has a poll URL been set and account not disabled ?
            If ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL <> "" And Not ActiveSource.VolusionSettings(iMerchantLoop).AccountDisabled Then ' TJS 19/08/10
                ' yes, has next poll time been reached ?
                If ActiveSource.VolusionSettings(iMerchantLoop).NextOrderPollTime <= Date.Now Then
                    ' yes, poll for orders
                    ' start of code replaced TJS 18/07/11
                    Try
                        WebSubmit = System.Net.WebRequest.Create(ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL)
                        WebSubmit.Method = "GET"
                        WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                        WebSubmit.ContentLength = 0
                        WebSubmit.Timeout = 30000

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        strReturn = reader.ReadToEnd()
                        ' end of code replaced TJS 02/12/11

                        If strReturn <> "" Then
                            If strReturn.Length > 38 Then
                                If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                    Try
                                        XMLResponse = XDocument.Parse(strReturn)
                                        rowOrderToRetry = Nothing ' TJS 04/01/14
                                        ProcessVolusionXML(ActiveSource, ActiveSource.VolusionSettings(iMerchantLoop), XMLResponse, rowOrderToRetry) ' TJS 04/01/14

                                    Catch ex As Exception
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Order XML from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))

                                    End Try

                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Response from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                End If

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Response from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                            End If

                        Else
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Response from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                        End If

                    Catch ex As Exception
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Unable to Poll for orders on " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " - " & ex.Message & ", " & ex.StackTrace, "")

                    Finally
                        If Not WebResponse Is Nothing Then WebResponse.Close()
                        ActiveSource.VolusionSettings(iMerchantLoop).NextOrderPollTime = Date.Now.AddMinutes(ActiveSource.VolusionSettings(iMerchantLoop).OrderPollIntervalMinutes)
                        bReturnValue = True ' TJS 10/12/09

                    End Try

                End If

            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next

        ' start of code added TJS 04/01/14
        ' loop through the Volusion web site list
        For iMerchantLoop = 0 To ActiveSource.VolusionSettingCount - 1
            ' has a poll URL been set and account not disabled ?
            If ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL <> "" And Not ActiveSource.VolusionSettings(iMerchantLoop).AccountDisabled Then
                ' are there any orders which failed to import that are due for a retry ?
                m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                    "ReadLerrynImportExportSourceOrdersToRetry_DEV000221", AT_SOURCE_CODE, VOLUSION_SOURCE_CODE, AT_STORE_MERCHANT_ID, ActiveSource.VolusionSettings(iMerchantLoop).SiteID}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.Count > 0 Then
                    ' yes, 
                    For Each rowOrderToRetry In m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221
                        Try
                            WebSubmit = System.Net.WebRequest.Create(ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & "&WHERE_Column=o.OrderID&WHERE_Value=" & rowOrderToRetry.MerchantOrderID_DEV000221)
                            WebSubmit.Method = "GET"
                            WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                            WebSubmit.ContentLength = 0
                            WebSubmit.Timeout = 30000

                            WebResponse = WebSubmit.GetResponse
                            reader = New StreamReader(WebResponse.GetResponseStream())
                            strReturn = reader.ReadToEnd()

                            If strReturn <> "" Then
                                If strReturn.Length > 38 Then
                                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                        Try
                                            XMLResponse = XDocument.Parse(strReturn)
                                            ProcessVolusionXML(ActiveSource, ActiveSource.VolusionSettings(iMerchantLoop), XMLResponse, rowOrderToRetry)

                                        Catch ex As Exception
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Order XML from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))

                                        End Try

                                    Else
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Response from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                    End If

                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Response from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                End If

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Response from " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                            End If

                        Catch ex As Exception
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollVolusion", "Unable to Poll orders id " & rowOrderToRetry.MerchantOrderID_DEV000221 & " on " & ActiveSource.VolusionSettings(iMerchantLoop).OrderPollURL & " - " & ex.Message & ", " & ex.StackTrace, "")

                        Finally
                            If Not WebResponse Is Nothing Then WebResponse.Close()

                        End Try

                    Next
                    m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                         "CreateLerrynImportExportSourceOrdersToRetry_DEV000221", "UpdateLerrynImportExportSourceOrdersToRetry_DEV000221", _
                         "DeleteLerrynImportExportSourceOrdersToRetry_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                         "Updated Volusion Orders to Retry", False)
                End If
            End If
            If bShutDownInProgress Then
                Exit For
            End If
        Next
        ' end of code added TJS 04/01/14

        Return bReturnValue

    End Function

    Private Function PollEBay(ByRef ActiveSource As SourceSettings) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls eBay for orders and updates to Orders
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.0. | Function added
        ' 10/06/12 | TJS             | 2012.1.05 | Added code to retry orders which failed to import 
        ' 10/08/12 | TJS             | 2012.1.12 | Modified to log errors when reading Official eBay time and ignore shipped orders
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 18/08/13 | FA              | 2013.2.03 | Modified to log warning messages to log and send email
        ' 03/10/13 | FA              | 2013.3.04 | Modified to ignore shipped orders on OrdersToRetry
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected detection of more orders
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim eBayConnection As Lerryn.Facade.ImportExport.eBayXMLConnector
        Dim rowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row ' TJS 10/06/12
        Dim XMLTemp As XDocument, XMLOrderNode As XElement
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim CreateDateRangeFilter As Lerryn.Facade.ImportExport.eBayXMLConnector.TimeFilter
        Dim ModificationDateRangeFilter As Lerryn.Facade.ImportExport.eBayXMLConnector.TimeFilter
        Dim sTemp As String, dteEBayTime As Date
        Dim iMerchantLoop As Integer, iPageFilter As Integer, bAlreadyImported As Boolean ' TJS 10/06/12
        Dim bReturnValue As Boolean, bUseEBaySandbox As Boolean, bHasMorOrders As Boolean

        bReturnValue = False ' TJE 10/12/09
        ' check registry for Use eBay Sandbox setting (used during testing)
        bUseEBaySandbox = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "UseEBaySandbox", "NO").ToUpper = "YES")

        ' loop through the eBay web site list
        For iMerchantLoop = 0 To ActiveSource.eBaySettingCount - 1
            ' is next connection due and account not disabled and account details are entered and eBay Auth Token not expired (Note ignore last 24 hours of token to avoid any timezone or other time differences)
            If ActiveSource.eBaySettings(iMerchantLoop).NextOrderPollTime <= Date.Now And Not ActiveSource.eBaySettings(iMerchantLoop).AccountDisabled And _
                ActiveSource.eBaySettings(iMerchantLoop).eBayCountry >= 0 And ActiveSource.eBaySettings(iMerchantLoop).AuthToken <> "" And _
                ActiveSource.eBaySettings(iMerchantLoop).TokenExpires > Date.Now.AddDays(-1) Then
                ' yes, poll for orders
                eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(m_ImportExportConfigFacade, ActiveSource.eBaySettings(iMerchantLoop), bUseEBaySandbox)
                If eBayConnection.GetOfficialEBayTime(dteEBayTime) Then

                    iPageFilter = 1
                    bHasMorOrders = False
                    ' set creation date filter to return all orders created in last 30 days
                    CreateDateRangeFilter = New Lerryn.Facade.ImportExport.eBayXMLConnector.TimeFilter()
                    CreateDateRangeFilter.TimeFrom = ActiveSource.eBaySettings(iMerchantLoop).LastOrderStatusUpdate
                    If dteEBayTime > CreateDateRangeFilter.TimeFrom.AddDays(30) Then
                        dteEBayTime = CreateDateRangeFilter.TimeFrom.AddDays(30).AddMinutes(-1)
                    End If
                    CreateDateRangeFilter.TimeTo = dteEBayTime
                    ' loop until no more orders
                    Do
                        If Not eBayConnection.GetOrders(CreateDateRangeFilter, Nothing, "", "Completed", iPageFilter) Then
                            Exit Do
                            ' start of code added FA 18/08/13
                        Else
                            If eBayConnection.ReturnedXML.XPathSelectElement("GetOrdersResponse/Ack") IsNot Nothing AndAlso _
                                eBayConnection.ReturnedXML.XPathSelectElement("GetOrdersResponse/Ack").Value = "Warning" Then
                                m_ImportExportConfigFacade.WriteLogProgressRecord(eBayConnection.ReturnedXML.XPathSelectElement("GetOrdersResponse/Errors/LongMessage").Value)
                            End If
                            ' end of code added FA 18/08/13
                        End If
                        bHasMorOrders = CBool(eBayConnection.ReturnedXML.XPathSelectElement("GetOrdersResponse/HasMoreOrders").Value) ' TJS 13/11/13
                        XMLOrderList = eBayConnection.ReturnedXML.XPathSelectElements("GetOrdersResponse/OrderArray/Order")
                        For Each XMLOrderNode In XMLOrderList
                            bAlreadyImported = False ' TJS 10/06/12
                            rowOrderToRetry = Nothing ' TJS 10/06/12
                            XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                            If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "Order/OrderStatus") = "Completed" Then
                                If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "PaymentHoldStatus") = "None" Then
                                    If String.IsNullOrEmpty(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "ShippedTime")) Then ' TJS 10/08/12
                                        ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Order", bAlreadyImported, rowOrderToRetry) ' TJS 10/06/12
                                    End If

                                Else
                                    ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Cancel", bAlreadyImported, rowOrderToRetry) ' TJS 10/06/12

                                End If

                            ElseIf ActiveSource.eBaySettings(iMerchantLoop).ActionIfNoPayment = "Import as Quote" Then
                                If String.IsNullOrEmpty(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "ShippedTime")) Then ' TJS 10/08/12
                                    ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Quote", bAlreadyImported, rowOrderToRetry) ' TJS 10/06/12
                                End If

                            End If
                            RecordOrderIDToRetry(rowOrderToRetry, ActiveSource.eBaySettings(iMerchantLoop).SiteID, "eBay") ' TJS 10/06/12
                            If bShutDownInProgress Then ' TJS 02/08/13
                                Exit For ' TJS 02/08/13
                            End If
                        Next

                        iPageFilter = iPageFilter + 1
                        If iPageFilter > 1000 Then
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollEBay", "Order Service has returned more then 1000 pages of orders from " & ActiveSource.eBaySettings(iMerchantLoop).SiteID, "")
                            Exit Do

                        End If
                        If bShutDownInProgress Then ' TJS 02/08/13
                            Exit Do ' TJS 02/08/13
                        End If
                    Loop While bHasMorOrders

                    iPageFilter = 1
                    bHasMorOrders = False
                    ' set modification date filter to only return orders with status modified since last poll (this will include new orders)
                    ModificationDateRangeFilter = New Lerryn.Facade.ImportExport.eBayXMLConnector.TimeFilter()
                    ModificationDateRangeFilter.TimeFrom = ActiveSource.eBaySettings(iMerchantLoop).LastOrderStatusUpdate
                    ModificationDateRangeFilter.TimeTo = dteEBayTime
                    ' loop until no more orders
                    Do
                        If Not eBayConnection.GetOrders(Nothing, ModificationDateRangeFilter, "", "Completed", iPageFilter) Then
                            Exit Do
                        End If
                        bHasMorOrders = CBool(eBayConnection.ReturnedXML.XPathSelectElement("GetOrdersResponse/HasMoreOrders"))
                        XMLOrderList = eBayConnection.ReturnedXML.XPathSelectElements(EBAY_ORDER_LIST)
                        For Each XMLOrderNode In XMLOrderList
                            bAlreadyImported = False ' TJS 10/06/12
                            rowOrderToRetry = Nothing ' TJS 10/06/12
                            XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                            If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "Order/OrderStatus") = "Completed" Then
                                If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "PaymentHoldStatus") = "None" Then
                                    If String.IsNullOrEmpty(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "ShippedTime")) Then ' FA 03/10/13
                                        ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Order", bAlreadyImported, rowOrderToRetry) ' TJS 10/06/12
                                    End If

                                Else
                                    ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Cancel", bAlreadyImported, rowOrderToRetry) ' TJS 10/06/12

                                End If

                            ElseIf ActiveSource.eBaySettings(iMerchantLoop).ActionIfNoPayment = "Import as Quote" Then
                                If String.IsNullOrEmpty(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "ShippedTime")) Then ' FA 03/10/13
                                    ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Quote", bAlreadyImported, rowOrderToRetry) ' TJS 10/06/12

                                End If

                            End If
                            RecordOrderIDToRetry(rowOrderToRetry, ActiveSource.eBaySettings(iMerchantLoop).SiteID, "eBay") ' TJS 10/06/12
                            If bShutDownInProgress Then ' TJS 02/08/13
                                Exit For ' TJS 02/08/13
                            End If
                        Next

                        iPageFilter = iPageFilter + 1
                        If iPageFilter > 1000 Then
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollEBay", "Order Service has returned more then 1000 pages of orders from " & ActiveSource.eBaySettings(iMerchantLoop).SiteID, "")
                            Exit Do

                        End If
                        If bShutDownInProgress Then ' TJS 02/08/13
                            Exit Do ' TJS 02/08/13
                        End If
                    Loop While bHasMorOrders


                    ActiveSource.eBaySettings(iMerchantLoop).LastOrderStatusUpdate = dteEBayTime
                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollEBay", "GetOfficialEBayTime failed on " & ActiveSource.eBaySettings(iMerchantLoop).SiteID & " with error details - " & eBayConnection.LastError, "") ' TJS 10/08/12

                End If
                ActiveSource.eBaySettings(iMerchantLoop).NextOrderPollTime = Date.Now.AddMinutes(ActiveSource.eBaySettings(iMerchantLoop).OrderPollIntervalMinutes)

                eBayConnection = Nothing

                ' check for eBay Auth Token expire in next month
                If ActiveSource.eBaySettings(iMerchantLoop).TokenExpires <= Date.Now.AddMonths(1) Then
                    ' is this the first email (since the service started) or was last email more than 7 days ago ?
                    If Not bEbayAuthTokenExpiryEMailSent OrElse dteEbayAuthTokenExpiryEMailSent < Date.Now.AddDays(-7) Then
                        ' yes, send wanring email
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollEBay", "eBay Auth Token for " & ActiveSource.eBaySettings(iMerchantLoop).SiteID & " will expire on " & ActiveSource.eBaySettings(iMerchantLoop).TokenExpires.ToShortDateString & " - Please create a new Auth Token for eShopCONNECT otherwise eShopCONNECT will cease to be able to connect to eBay", "")
                        bEbayAuthTokenExpiryEMailSent = True
                        dteEbayAuthTokenExpiryEMailSent = Date.Now
                    End If
                End If

            ElseIf ActiveSource.eBaySettings(iMerchantLoop).AuthToken <> "" And ActiveSource.eBaySettings(iMerchantLoop).TokenExpires <= Date.Now.AddDays(-1) Then
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollEBay", "eBay Auth Token expired for " & ActiveSource.eBaySettings(iMerchantLoop).SiteID, "")

            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        ' save last status update time
        sTemp = "<eShopCONNECTConfig>"
        For iMerchantLoop = 0 To ActiveSource.eBaySettingCount - 1
            sTemp = sTemp & "<eBay><SiteID>" & ActiveSource.eBaySettings(iMerchantLoop).SiteID & "</SiteID>"
            sTemp = sTemp & "<LastOrderStatusUpdate>" & CreateXMLDate(ActiveSource.eBaySettings(iMerchantLoop).LastOrderStatusUpdate)
            sTemp = sTemp & "</LastOrderStatusUpdate></eBay>"
        Next
        sTemp = sTemp & "</eShopCONNECTConfig>"
        m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE LerrynImportExportConfig_DEV000221 SET LastOrderStatusUpdate_DEV000221 = '" & _
            sTemp & "' WHERE SourceCode_DEV000221 = '" & ActiveSource.SourceCode & "'", Nothing)

        ' start of code added TJS 10/06/12
        ' loop through the eBay web site list again
        For iMerchantLoop = 0 To ActiveSource.eBaySettingCount - 1
            ' are account details entered and eBay Auth Token not expired and account not disabled ? (Note ignore last 24 hours of token to avoid any timezone or other time differences)
            If Not ActiveSource.eBaySettings(iMerchantLoop).AccountDisabled And ActiveSource.eBaySettings(iMerchantLoop).eBayCountry >= 0 And _
                ActiveSource.eBaySettings(iMerchantLoop).AuthToken <> "" And ActiveSource.eBaySettings(iMerchantLoop).TokenExpires > Date.Now.AddDays(-1) Then
                ' are there any orders which failed to import that are due for a retry ?
                m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                    "ReadLerrynImportExportSourceOrdersToRetry_DEV000221", AT_SOURCE_CODE, EBAY_SOURCE_CODE, AT_STORE_MERCHANT_ID, ActiveSource.eBaySettings(iMerchantLoop).SiteID}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.Count > 0 Then
                    ' yes, get order
                    eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(m_ImportExportConfigFacade, ActiveSource.eBaySettings(iMerchantLoop), bUseEBaySandbox)
                    If eBayConnection.GetOfficialEBayTime(dteEBayTime) Then
                        For Each rowOrderToRetry In m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221
                            If eBayConnection.GetOrders(Nothing, Nothing, rowOrderToRetry.MerchantOrderID_DEV000221, "Completed", iPageFilter) Then
                                bAlreadyImported = False
                                XMLOrderList = eBayConnection.ReturnedXML.XPathSelectElements(EBAY_ORDER_LIST)
                                For Each XMLOrderNode In XMLOrderList
                                    XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "Order/OrderStatus") = "Completed" Then
                                        If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "PaymentHoldStatus") = "None" Then
                                            If String.IsNullOrEmpty(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "ShippedTime")) Then ' FA 03/10/13
                                                ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Order", bAlreadyImported, rowOrderToRetry)
                                            End If

                                        Else
                                            ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Cancel", bAlreadyImported, rowOrderToRetry)

                                        End If

                                    ElseIf ActiveSource.eBaySettings(iMerchantLoop).ActionIfNoPayment = "Import as Quote" Then
                                        If String.IsNullOrEmpty(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, EBAY_ORDER_DETAILS & "ShippedTime")) Then ' FA 03/10/13
                                            ProcessEBayOrder(ActiveSource, ActiveSource.eBaySettings(iMerchantLoop), XMLTemp, "Quote", bAlreadyImported, rowOrderToRetry)
                                        End If

                                    End If
                                    If bShutDownInProgress Then ' TJS 02/08/13
                                        Exit For ' TJS 02/08/13
                                    End If
                                Next
                            End If
                            If bShutDownInProgress Then ' TJS 02/08/13
                                Exit For ' TJS 02/08/13
                            End If
                        Next
                        m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                             "CreateLerrynImportExportSourceOrdersToRetry_DEV000221", "UpdateLerrynImportExportSourceOrdersToRetry_DEV000221", _
                             "DeleteLerrynImportExportSourceOrdersToRetry_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                             "Updated eBay Orders to Retry", False)
                    End If

                    eBayConnection = Nothing
                End If
            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        ' end of code added TJS 10/06/12

        Return True

    End Function

    Private Function PollSearsCom(ByRef ActiveSource As SourceSettings) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls Sears.com for orders and updates to Orders
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManSearsCom As System.Xml.XmlNamespaceManager, XMLNameTabSearsCom As System.Xml.NameTable
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument
        Dim strURLParams As String, strReturn As String, iMerchantLoop As Integer, bReturnValue As Boolean

        bReturnValue = False
        ' loop through the Sears.com web site list
        For iMerchantLoop = 0 To ActiveSource.SearsComSettingCount - 1
            ' has an API USer and API Pwd been set and account not disabled ?
            If ActiveSource.SearsComSettings(iMerchantLoop).APIUser <> "" And ActiveSource.SearsComSettings(iMerchantLoop).APIPwd <> "" And _
                Not ActiveSource.SearsComSettings(iMerchantLoop).AccountDisabled Then
                ' yes, has next poll time been reached ?
                If ActiveSource.SearsComSettings(iMerchantLoop).NextOrderPollTime <= Date.Now Then
                    ' yes, poll for orders
                    Try
                        strURLParams = SEARSDOTCOM_ORDER_POLL_URL & "?email=" & ActiveSource.SearsComSettings(iMerchantLoop).APIUser
                        strURLParams = strURLParams & "&password=" & ActiveSource.SearsComSettings(iMerchantLoop).APIPwd
                        If ActiveSource.SearsComSettings(iMerchantLoop).LastOrderStatusUpdate = DateSerial(1900, 1, 1) Then
                            strURLParams = strURLParams & "&status=New"
                        Else
                            strURLParams = strURLParams & "&fromdate=" & Format(ActiveSource.SearsComSettings(iMerchantLoop).LastOrderStatusUpdate, "yyyy-MM-dd")
                            strURLParams = strURLParams & "&status=New"
                        End If


                        WebSubmit = System.Net.WebRequest.Create(strURLParams)
                        WebSubmit.Method = "GET"
                        WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                        WebSubmit.ContentLength = 0
                        WebSubmit.Timeout = 30000

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        strReturn = reader.ReadToEnd()

                        If strReturn <> "" Then
                            If strReturn.Length > 38 Then
                                If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                    Try
                                        XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://seller.marketplace.sears.com/shared/v1""", "").Replace(" xmlns=""http://seller.marketplace.sears.com/oms/v3""", ""))

                                        XMLNameTabSearsCom = New System.Xml.NameTable
                                        XMLNSManSearsCom = New System.Xml.XmlNamespaceManager(XMLNameTabSearsCom)
                                        XMLNSManSearsCom.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance")
                                        XMLNSManSearsCom.AddNamespace("schemaLocation", "rest/oms/export/v3/purchase-order.xsd")

                                        If XMLResponse.XPathSelectElement("po-response/purchase-order", XMLNSManSearsCom) IsNot Nothing Then
                                            ProcessSearsComXML(ActiveSource, ActiveSource.SearsComSettings(iMerchantLoop), XMLResponse, XMLNSManSearsCom)

                                        ElseIf XMLResponse.XPathSelectElement("api-response/error-detail", XMLNSManSearsCom) IsNot Nothing Then
                                            If XMLResponse.XPathSelectElement("api-response/error-detail", XMLNSManSearsCom).Value <> "No POs found" Then
                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollSearsCom", "Get Orders response XML from " & SEARSDOTCOM_ORDER_POLL_URL & " for APIUSer " & ActiveSource.SearsComSettings(iMerchantLoop).APIUser & " returned error details " & XMLResponse.XPathSelectElement("api-response/error-detail", XMLNSManSearsCom).Value)
                                            End If

                                        Else
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollSearsCom", "Get Orders response XML from " & SEARSDOTCOM_ORDER_POLL_URL & " for APIUSer " & ActiveSource.SearsComSettings(iMerchantLoop).APIUser & " not recognised - " & XMLResponse.ToString)

                                        End If

                                    Catch ex As Exception
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollSearsCom", "Get Orders response XML from " & SEARSDOTCOM_ORDER_POLL_URL & " for APIUSer " & ActiveSource.SearsComSettings(iMerchantLoop).APIUser & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))

                                    End Try

                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollSearsCom", "Response from " & SEARSDOTCOM_ORDER_POLL_URL & " for APIUSer " & ActiveSource.SearsComSettings(iMerchantLoop).APIUser & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                End If

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollSearsCom", "Response from " & SEARSDOTCOM_ORDER_POLL_URL & " for APIUSer " & ActiveSource.SearsComSettings(iMerchantLoop).APIUser & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                            End If

                        Else
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollSearsCom", "Response from " & SEARSDOTCOM_ORDER_POLL_URL & " for APIUSer " & ActiveSource.SearsComSettings(iMerchantLoop).APIUser & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                        End If

                    Catch ex As Exception
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "PollSearsCom", "Unable to Poll for orders on " & SEARSDOTCOM_ORDER_POLL_URL & " for APIUSer " & ActiveSource.SearsComSettings(iMerchantLoop).APIUser & " - " & ex.Message & ", " & ex.StackTrace, "")

                    Finally
                        If Not postStream Is Nothing Then postStream.Close()
                        If Not WebResponse Is Nothing Then WebResponse.Close()
                        ActiveSource.SearsComSettings(iMerchantLoop).NextOrderPollTime = Date.Now.AddMinutes(ActiveSource.SearsComSettings(iMerchantLoop).OrderPollIntervalMinutes)
                        bReturnValue = True

                    End Try

                End If

            End If
            If bShutDownInProgress Then ' TJS 02/08/13
                Exit For ' TJS 02/08/13
            End If
        Next
        Return bReturnValue

    End Function

    Private Function Poll3DCart(ByRef ActiveSource As SourceSettings, ByVal InhibitWebPosts As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Polls ASPDotNetStorefront for orders and updates to Orders
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row
        Dim XMLNSMan3DCart As System.Xml.XmlNamespaceManager, XMLNameTab3DCart As System.Xml.NameTable
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument, XMLTemp As XDocument
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement), XMLOrderNode As XElement
        Dim strSubmit As String, strReturn As String, sTemp As String
        Dim strDateParts As String(), strTimeParts As String()
        Dim iMerchantLoop As Integer, iBatchSize As Integer, iNumOrders As Integer
        Dim dteOrderTimestamp As Date, bAlreadyImported As Boolean
        Dim bReturnValue As Boolean, bXMLError As Boolean, bLastOrderStatusTimeChanged As Boolean

        ' loop through the 3DCart store list
        For iMerchantLoop = 0 To ActiveSource.ThreeDCartSettingCount - 1
            ' is next connection due and account not disabled and account details are entered ?
            If ActiveSource.ThreeDCartSettings(iMerchantLoop).NextOrderPollTime <= Date.Now And Not ActiveSource.ThreeDCartSettings(iMerchantLoop).AccountDisabled And _
                ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID <> "" And ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreURL <> "" Then
                ' yes, poll for orders
                iBatchSize = 100
                iNumOrders = 0
                Do
                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?>"
                    strSubmit = strSubmit & "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                    strSubmit = strSubmit & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" "
                    strSubmit = strSubmit & "xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">"
                    strSubmit = strSubmit & "<soap:Body><getOrder xmlns=""http://3dcart.com/"">"
                    strSubmit = strSubmit & "<storeUrl>" & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreURL & "</storeUrl>"
                    strSubmit = strSubmit & "<userKey>" & ActiveSource.ThreeDCartSettings(iMerchantLoop).UserKey & "</userKey>"
                    strSubmit = strSubmit & "<batchSize>100</batchSize>"
                    strSubmit = strSubmit & "<startNum>1</startNum>"
                    strSubmit = strSubmit & "<startFrom>false</startFrom>"
                    strSubmit = strSubmit & "<invoiceNum></invoiceNum>"
                    strSubmit = strSubmit & "<status>New</status>"
                    strSubmit = strSubmit & "<dateFrom>" & ActiveSource.ThreeDCartSettings(iMerchantLoop).LastOrderStatusUpdate.ToString("MM/dd/yyyy") & "</dateFrom>"
                    strSubmit = strSubmit & "<dateTo></dateTo>"
                    strSubmit = strSubmit & "<callBackURL></callBackURL>"
                    strSubmit = strSubmit & "</getOrder></soap:Body></soap:Envelope>"
                    Try
                        WebSubmit = System.Net.WebRequest.Create(THREE_D_CART_WEB_SERVICES_URL)
                        WebSubmit.Method = "POST"
                        WebSubmit.ContentType = "text/xml; charset=utf-8"
                        WebSubmit.ContentLength = strSubmit.Length
                        WebSubmit.Timeout = 30000

                        byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                        ' send to 3DCart
                        postStream = WebSubmit.GetRequestStream()
                        postStream.Write(byteData, 0, byteData.Length)

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        strReturn = reader.ReadToEnd()

                        If strReturn <> "" Then
                            If strReturn.Length > 38 Then
                                If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Or strReturn.Trim.Substring(0, 14).ToLower = "<soap:envelope" Then
                                    bXMLError = False
                                    Try
                                        ' had difficulty getting XPath to read XML with this name space present so remove it
                                        XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://3dcart.com/""", ""))

                                    Catch ex As Exception
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Order XML from " & THREE_D_CART_WEB_SERVICES_URL & " for " & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID.ToString & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))
                                        bXMLError = True

                                    End Try

                                    ' did XML load correctly
                                    If Not bXMLError Then
                                        ' yes
                                        XMLNameTab3DCart = New System.Xml.NameTable
                                        XMLNSMan3DCart = New System.Xml.XmlNamespaceManager(XMLNameTab3DCart)
                                        XMLNSMan3DCart.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/")
                                        ' did we get an error ?
                                        If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/getOrderResponse/getOrderResult/Error", XMLNSMan3DCart) = "" Then
                                            ' no, process orders
                                            XMLOrderList = XMLResponse.XPathSelectElements(THREE_D_CART_ORDER_LIST, XMLNSMan3DCart)
                                            iNumOrders = m_ImportExportConfigFacade.GetXMLElementListCount(XMLOrderList)
                                            If iNumOrders > 0 Then
                                                For Each XMLOrderNode In XMLOrderList
                                                    XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                                                    sTemp = m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "Order/Date")
                                                    strDateParts = sTemp.Split(CChar("/"))
                                                    sTemp = m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "Order/Time")
                                                    If InStr(sTemp, " ") > 0 Then
                                                        sTemp = sTemp.Substring(0, InStr(sTemp, " ") - 1)
                                                    End If
                                                    strTimeParts = sTemp.Split(CChar(":"))
                                                    dteOrderTimestamp = DateSerial(CInt(strDateParts(2)), CInt(strDateParts(0)), CInt(strDateParts(1))).AddHours(CDbl(strTimeParts(0))).AddMinutes(CDbl(strTimeParts(1))).AddSeconds(CInt(strTimeParts(2)))
                                                    bAlreadyImported = False
                                                    rowOrderToRetry = Nothing
                                                    Process3DCartXML(ActiveSource, ActiveSource.ThreeDCartSettings(iMerchantLoop), XMLTemp, bAlreadyImported, rowOrderToRetry)
                                                    RecordOrderIDToRetry(rowOrderToRetry, ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID, "3DCart")
                                                    ' did we only get 1 order which has already been imported ?
                                                    If m_ImportExportConfigFacade.GetXMLElementListCount(XMLOrderList) = 1 And bAlreadyImported Then
                                                        ' yes, add 2 seconds to timestamp so we don't get order again because 
                                                        ' we now know there no other orders with timestamps similar to this order
                                                        ActiveSource.ThreeDCartSettings(iMerchantLoop).LastOrderStatusUpdate = ActiveSource.ThreeDCartSettings(iMerchantLoop).LastOrderStatusUpdate.AddSeconds(2)
                                                        bLastOrderStatusTimeChanged = True

                                                    ElseIf dteOrderTimestamp > ActiveSource.ThreeDCartSettings(iMerchantLoop).LastOrderStatusUpdate Then
                                                        ' no, must have some new orders with order timestamp later than previous Last Order Status Update time 
                                                        ' use it but subtract 1 second to make sure we don't miss any orders due to rounding
                                                        ' as 3DCart doesn't send milliseconds in timesstamps
                                                        ActiveSource.ThreeDCartSettings(iMerchantLoop).LastOrderStatusUpdate = dteOrderTimestamp.AddSeconds(-1)
                                                        bLastOrderStatusTimeChanged = True
                                                    End If
                                                    If Not InhibitWebPosts Then
                                                        strSubmit = "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                                                        strSubmit = strSubmit & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" "
                                                        strSubmit = strSubmit & "xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">"
                                                        strSubmit = strSubmit & "<soap:Body> <updateOrderStatus xmlns=""http://3dcart.com/"">"
                                                        strSubmit = strSubmit & "<storeUrl>" & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreURL & "</storeUrl>"
                                                        strSubmit = strSubmit & "<userKey>" & ActiveSource.ThreeDCartSettings(iMerchantLoop).UserKey & "</userKey>"
                                                        strSubmit = strSubmit & "<invoiceNum>" & m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "Order/InvoiceNumber") & "</invoiceNum>"
                                                        strSubmit = strSubmit & "<newStatus>Processing</newStatus>"
                                                        strSubmit = strSubmit & "<callBackURL></callBackURL>"
                                                        strSubmit = strSubmit & "</updateOrderStatus></soap:Body></soap:Envelope>"
                                                        Try
                                                            WebSubmit = System.Net.WebRequest.Create(THREE_D_CART_WEB_SERVICES_URL)
                                                            WebSubmit.Method = "POST"
                                                            WebSubmit.ContentType = "text/xml; charset=utf-8"
                                                            WebSubmit.ContentLength = strSubmit.Length
                                                            WebSubmit.Timeout = 30000

                                                            byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                                                            ' send to 3DCart
                                                            postStream = WebSubmit.GetRequestStream()
                                                            postStream.Write(byteData, 0, byteData.Length)

                                                            WebResponse = WebSubmit.GetResponse
                                                            reader = New StreamReader(WebResponse.GetResponseStream())
                                                            strReturn = reader.ReadToEnd()
                                                            If strReturn <> "" Then
                                                                If strReturn.Length > 38 Then
                                                                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Or strReturn.Trim.Substring(0, 14).ToLower = "<soap:envelope" Then
                                                                        bXMLError = False
                                                                        Try
                                                                            ' had difficulty getting XPath to read XML with this name space present so remove it
                                                                            XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://3dcart.com/""", ""))

                                                                        Catch ex As Exception
                                                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Order XML from " & THREE_D_CART_WEB_SERVICES_URL & " for " & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID.ToString & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))
                                                                            bXMLError = True

                                                                        End Try

                                                                        ' did XML load correctly
                                                                        If Not bXMLError Then
                                                                            ' yes, did we get an error ?
                                                                            If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/updateOrderStatusResponse/updateOrderStatusResult/UpdateOrderStatusResponse/NewStatus", XMLNSMan3DCart) = "Processing" Then
                                                                                ' no, status has been updated

                                                                            ElseIf m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/updateOrderStatusResponse/updateOrderStatusResult/Error", XMLNSMan3DCart) <> "" Then
                                                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Order Status Update Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & _
                                                                                    ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID & " was " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/updateOrderStatusResponse/updateOrderStatusResult/Error", XMLNSMan3DCart), _
                                                                                    strSubmit.Replace(ActiveSource.ThreeDCartSettings(iMerchantLoop).UserKey, "********"))

                                                                            Else
                                                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Order Status Update Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID.ToString & " not recognised", strReturn)
                                                                            End If
                                                                        End If
                                                                    Else
                                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                                                    End If

                                                                Else
                                                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                                                End If

                                                            Else
                                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                                            End If

                                                        Catch ex As Exception
                                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Unable to update order status for 3DCart " & THREE_D_CART_WEB_SERVICES_URL & " - " & ex.Message & ", " & ex.StackTrace, strSubmit.Replace(ActiveSource.ThreeDCartSettings(iMerchantLoop).UserKey, "********"))

                                                        End Try
                                                    End If
                                                Next
                                            End If

                                        Else
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & _
                                                ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID & " was " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/getOrderResponse/getOrderResult/Error", XMLNSMan3DCart), _
                                                strSubmit.Replace(ActiveSource.ThreeDCartSettings(iMerchantLoop).UserKey, "********"))
                                        End If
                                    End If
                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                                End If

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                            End If

                        Else
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                        End If

                    Catch ex As Exception
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Poll3DCart", "Unable to poll 3DCart for orders at " & THREE_D_CART_WEB_SERVICES_URL & " - " & ex.Message & ", " & ex.StackTrace, strSubmit.Replace(ActiveSource.ThreeDCartSettings(iMerchantLoop).UserKey, "********"))
                    End Try
                Loop While iNumOrders = iBatchSize
            End If
            If bShutDownInProgress Then
                Exit For
            End If
        Next
        ' save last status update time
        sTemp = "<eShopCONNECTConfig>"
        For iMerchantLoop = 0 To ActiveSource.ThreeDCartSettingCount - 1
            sTemp = sTemp & "<ThreeDCart><StoreID>" & ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID & "</StoreID>"
            sTemp = sTemp & "<LastOrderStatusUpdate>" & CreateXMLDate(ActiveSource.ThreeDCartSettings(iMerchantLoop).LastOrderStatusUpdate)
            sTemp = sTemp & "</LastOrderStatusUpdate></ThreeDCart>"
        Next
        sTemp = sTemp & "</eShopCONNECTConfig>"
        m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE LerrynImportExportConfig_DEV000221 SET LastOrderStatusUpdate_DEV000221 = '" & _
            sTemp & "' WHERE SourceCode_DEV000221 = '" & ActiveSource.SourceCode & "'", Nothing)
        Return bReturnValue

    End Function

    Friend Sub RecordOrderIDToRetry(ByRef rowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row, _
        ByVal SiteOrInstanceID As String, ByVal SourceName As String) ' TJS 16/07/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | Function added with code to cater for retry records already being present
        ' 16/07/13 | TJS             | 2013.1.29 | Changed from Private to Friend to cater for Amazon order retry
        ' 04/01/14 | TJS             | 2013.4.03 | Modified to cater for single quote characters in the Store Merchant ID
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String

        If rowOrderToRetry IsNot Nothing Then
            ' have we tried to process the same order before and failed ?
            sTemp = "SourceCode_DEV000221 = '" & rowOrderToRetry.SourceCode_DEV000221 & "' AND StoreMerchantID_DEV000221 = '" & _
                rowOrderToRetry.StoreMerchantID_DEV000221.Replace("'", "''") & "' AND MerchantOrderID_DEV000221 = '" & rowOrderToRetry.MerchantOrderID_DEV000221 & "'" ' TJS 04/01/14
            If "" & m_ImportExportConfigFacade.GetField("MerchantOrderID_DEV000221", "LerrynImportExportSourceOrdersToRetry_DEV000221", sTemp) = "" Then
                ' no, add new row in db
                m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.AddLerrynImportExportSourceOrdersToRetry_DEV000221Row(rowOrderToRetry)
                m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                     "CreateLerrynImportExportSourceOrdersToRetry_DEV000221", "UpdateLerrynImportExportSourceOrdersToRetry_DEV000221", _
                     "DeleteLerrynImportExportSourceOrdersToRetry_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                     "Add " & SourceName & " Order for Retry", False)
            Else
                ' yes, get details
                m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                    "ReadLerrynImportExportSourceOrdersToRetry_DEV000221", AT_SOURCE_CODE, rowOrderToRetry.SourceCode_DEV000221, AT_STORE_MERCHANT_ID, _
                    rowOrderToRetry.StoreMerchantID_DEV000221, AT_MERCHANT_ORDER_ID, rowOrderToRetry.MerchantOrderID_DEV000221, AT_IGNORE_RETRY_TIME_AND_COUNT, "1"}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                ' was order due for retry
                If m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221(0).NextRetryDateTime_DEV000221 < Date.Now Then
                    ' yes, update next retry time and count
                    If m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221(0).RetryCount_DEV000221 > 4 Then
                        m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221(0).NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                    Else
                        m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221(0).NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                    End If
                    m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221(0).RetryCount_DEV000221 = m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221(0).RetryCount_DEV000221 - 1
                    m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.TableName, _
                         "CreateLerrynImportExportSourceOrdersToRetry_DEV000221", "UpdateLerrynImportExportSourceOrdersToRetry_DEV000221", _
                         "DeleteLerrynImportExportSourceOrdersToRetry_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                         "Update " & SourceName & " Order for Retry", False)
                End If
            End If
        End If

    End Sub

    Public Function CreateXMLDate(ByVal DateToUse As Date) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Formats the Date into XML 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Function made public for use in Daily Tasks functions
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strXMLDate As String

        strXMLDate = DateToUse.Year & "-" & Right("00" & DateToUse.Month, 2) & "-" & Right("00" & DateToUse.Day, 2)
        strXMLDate = strXMLDate & "T" & Right("00" & DateToUse.Hour, 2) & ":" & Right("00" & DateToUse.Minute, 2)
        strXMLDate = strXMLDate & ":" & Right("00" & DateToUse.Second, 2)
        Return strXMLDate

    End Function
End Module
