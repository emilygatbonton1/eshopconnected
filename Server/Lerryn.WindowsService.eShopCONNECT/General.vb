' eShopCONNECT for Connected Business - Windows Service
' Module: General.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 23 January 2014

Imports System
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.IO
Imports System.Threading
Imports System.Data
Imports System.Data.OleDb
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath
Imports System.Text
Imports System.Net ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const ' TJS 11/12/09
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Public Module General

    Friend Const logProgress As String = "Progress"
    Friend Const logWarning As String = "Warning!"
    Friend Const logError As String = "!Error!!"

    Friend ftpHost As String = ""
    Friend ftpUser As String = ""
    Friend ftpPwd As String = ""

    Friend dtStatusUpdateDueAt As Date
    Friend dtInventoryUpdateDueAt As Date
    Friend dtFileCreationDueAt As Date
    Friend dtFileReadDueAt As Date
    Friend dtFTPDueAt As Date
    Friend dtShutDownTime As Date
    Friend dtStartUpTime As Date
    'Friend dtNotifyUsageTime As Date ' TJS 18/03/11

    Friend m_ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Public m_ImportExportConfigFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Friend m_ErrorNotification As Facade.ImportExport.ErrorNotification ' TJS 10/06/12
    Friend Setts As New Settings
    Friend bIsActivated As Boolean ' TJS 28/01/09
    Friend bFTPRequired As Boolean ' TJS 22/02/09
    Friend bFileImportRequired As Boolean ' TJS 22/02/09
    Friend bWebIORequired As Boolean ' TJS 19/06/09
    Friend bFileCreationRequired As Boolean ' TJS 22/02/09
    Friend bConfigOK As Boolean = False ' TJS 20/05/09
    Friend bFirstPoll As Boolean ' TJS 11/12/09
    Friend bShutDownInProgress As Boolean ' TJS 02/08/13
    'Friend WithEvents AmazonClient As AmazonSOAPClient.AmazonSellerCentral ' TJS 15/10/09
    Friend m_AmazonThrottling As Lerryn.Facade.ImportExport.AmazonMWSThrottling ' TJS 03/07/13

    Public Class TimerState ' TJS 13/06/10

        Public TimerCounter As Integer = 0 ' TJS 13/06/10
        Public ServiceState As Integer = 0 ' TJS 13/06/10

    End Class

    Public Function App_Path() As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.   | Description
        '------------------------------------------------------------------------------------------
        ' 18/09/08 | Craig Griffin   | 1.0.0.0   | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return System.AppDomain.CurrentDomain.BaseDirectory()

    End Function

    Public Function DoStartup(ByVal ts As TimerState) As Boolean ' TJS 13/06/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/09/08 | Craig Griffin   | 1.0.0.0   | Original
        ' 20/10/09 | TJS             | 2009.1.00 | Tidied message formats and content
        ' 22/02/09 | TJS             | 2009.1.08 | Mofified to cater for renaming of WriteLogProgressRecord
        ' 10/03/09 | TJS             | 2009.1.09 | Modified to ensure timer settings are run if GetSettings is successful
        ' 28/04/09 | TJS             | 2009.2.05 | Moved facade creation from GetSettings() to facilitate
        '                                        | auto update of settings after config change
        ' 02/06/09 | TJS             | 2009.2.10 | Modified to inhibit Status and Inventory updates on Order Importer build
        ' 13/06/10 | TJS             | 2010.0.07 | Modified callback timer settings to prevent intermittent operation
        '                                        | and to cater for failed DB connection on startup
        ' 18/03/11 | TJs             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 02/12/11 | TJS             | 2011.2.00 | modified to check user code in config file exists as an active user
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to detect licence/User count error
        ' 20/04/12 | TJS             | 2012.1.02 | Modified to call CheckAndUpdateConfigSettings
        ' 20/05/12 | TJS             | 2012.1.04 | Modified to log connection details to log file and event log
        ' 10/06/12 | TJS             | 2012.1.05 | modified to use Error Notification object to simplify facade login/logout
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ActiveSource As SourceSettings ' TJS 03/07/13
        Dim strUserCodeActive As String, strConfigText As String, iMaxValue As Integer, iMinValue As Integer ' TJS 18/03/11 TJS 20/05/12
        Dim lPasswordPosn As Integer ' TJS 20/05/12

        Try
            ' set timer state to failed db connection
            ts.ServiceState = 1 ' TJS 13/06/10
            Try ' TJS 10/03/12
                ' create config dataset and facade
                m_ImportExportDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
                m_ImportExportConfigFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_ImportExportDataset, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            Catch ex As Exception ' TJS 10/03/12
                ' can't use m_ImportExportConfigFacade.SendErrorEmail here as it could be the cause of the error
                ' start of code added TJS 20/05/12
                If Interprise.Facade.Base.SimpleFacade.Instance.OnlineConnectionString IsNot Nothing Then
                    strConfigText = PRODUCT_NAME & " for CB Version " & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion
                    lPasswordPosn = InStr(Interprise.Facade.Base.SimpleFacade.Instance.OnlineConnectionString, ";Password=")
                    If lPasswordPosn > 0 Then
                        strConfigText = strConfigText & " connected to " & Left(Interprise.Facade.Base.SimpleFacade.Instance.OnlineConnectionString, lPasswordPosn + 9) & "******"
                    Else
                        strConfigText = strConfigText & " connected to " & Interprise.Facade.Base.SimpleFacade.Instance.OnlineConnectionString
                    End If
                    m_ErrorNotification.WriteErrorToEventLog(strConfigText & " - Unable to create ImportExportConfigFacade - " & ex.Message) ' TJS 10/06/12
                    ' end of code added TJS 20/05/12
                Else
                    m_ErrorNotification.WriteErrorToEventLog("Unable to create ImportExportConfigFacade - " & ex.Message) ' TJS 10/03/12 TJS 10/06/12
                End If
                Return False ' TJS 10/03/12
            End Try

            If m_ImportExportConfigFacade.IsHostISeCommercePlus Then ' TJS 18/03/11
                strConfigText = "eShopCONNECT for IS eCommerce Plus Version " & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion ' TJS 18/03/11 TJS 20/05/12
            Else
                strConfigText = PRODUCT_NAME & " for CB Version " & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion ' TJS 03/04/09 TJS 20/05/12
            End If
            ' start of code added TJS 20/05/12
            lPasswordPosn = InStr(m_ImportExportConfigFacade.OnlineConnectionString, ";Password=")
            If lPasswordPosn > 0 Then
                strConfigText = strConfigText & " connected to " & Left(m_ImportExportConfigFacade.OnlineConnectionString, lPasswordPosn + 9) & "******"
            Else
                strConfigText = strConfigText & " connected to " & m_ImportExportConfigFacade.OnlineConnectionString
            End If
            m_ErrorNotification.WriteInfoToEventLog(strConfigText)
            ' end of code added TJS 20/05/12

            If PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 13/06/10 
                ' clear config Update flag and set Status and Inventory Action flags to make sure we pick up any changes not processed when service shut down
                m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET ConfigUpdateRequired_DEV000221 = 0, StatusActionRequired_DEV000221 = 1, InventoryActionRequired_DEV000221 = 1", Nothing) ' TJS 28/04/09 TJS 13/06/10
            Else
                ' clear config Update flag  
                m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET ConfigUpdateRequired_DEV000221 = 0", Nothing) ' TJS 28/04/09 TJS 13/06/10
            End If

            ' set timer state to db connected
            ts.ServiceState = 2 ' TJS 13/06/10

            ' start of code added TJS/FA 02/12/11
            If IsNothing(Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode()) Then
                m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "DoStartup", "There is no User Code specified in the config file for the Lerryn eShopCONNECT service.") ' TJS 10/06/12
                Return False
            Else
                strUserCodeActive = m_ImportExportConfigFacade.GetField("IsActive", "SystemUserAccount", "UserCode = '" & Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & "'")
                If IsNothing(strUserCodeActive) Then
                    m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "DoStartup", "The User Code specified in the config file for the Lerryn eShopCONNECT service does not exist in the IS database.") ' TJS 10/06/12
                    Return False
                ElseIf Not CBool(strUserCodeActive) Then
                    m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "DoStartup", "The User Code specified in the config file for the Lerryn eShopCONNECT service exists in the IS database, but is not marked as Active.") ' TJS 10/06/12
                    Return False
                End If
            End If
            ' end of code added TJS/FA 02/12/11

            m_ImportExportConfigFacade.CheckAndUpdateConfigSettings() ' TJS 20/04/12

            'Set the application parameters (return from Interprise database)
            If GetSettings() Then
                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, strConfigText) ' TJS 20/05/12 TJS 10/06/12
                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Finished fetching parameters from Interprise DB.") ' TJS 28/01/09 TJS 22/02/09 TJS 10/06/12

                ' start of code added TJS 03/07/13
                For Each ActiveSource In Setts.ActiveSources
                    If ActiveSource.SourceCode = AMAZON_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        m_AmazonThrottling = New Lerryn.Facade.ImportExport.AmazonMWSThrottling
                    End If
                Next
                ' end of code added TJS 03/07/13

                ' set initial action times as 1 minute from now instead of usual 1/5 minute interval
                dtStatusUpdateDueAt = Date.Now.AddMinutes(1)
                If PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 02/06/09
                    m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Next Status Update run due at " & dtStatusUpdateDueAt) ' TJS 02/02/09 TJS 22/02/09 TJS 10/06/12
                End If
                dtInventoryUpdateDueAt = Date.Now.AddMinutes(1)
                If PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 02/06/09
                    m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Next Inventory Update run due at " & dtInventoryUpdateDueAt) ' TJS 02/02/09 TJS 22/02/09 TJS 10/06/12
                End If
                dtFileCreationDueAt = Date.Now.AddMinutes(1)
                If bFileCreationRequired And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 22/02/09 TJS 02/06/09 
                    m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Next File Creation run due at " & dtFileCreationDueAt) ' TJS 28/01/09 TJS 22/02/09 TJS 10/06/12
                End If
                dtFileReadDueAt = Date.Now.AddMinutes(1)
                If bFileImportRequired Then ' TJS 22/02/09
                    m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Next File Import run due at " & dtFileReadDueAt) ' TJS 28/01/09 TJS 22/02/09 TJS 10/06/12
                End If
                dtFTPDueAt = Date.Now.AddMinutes(1)
                If bFTPRequired And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 22/02/09 TJS 02/06/09
                    m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Next FTP run due at " & dtFTPDueAt) ' TJS 28/01/09 TJS 22/02/09 TJS 10/06/12
                End If

                '' start of code added TJS 18/03/11
                '' generate a random time between 6 am and 10 pm
                'Randomize(CDbl(Right(CStr(Date.Now().ToOADate), 4)))
                'iMaxValue = 22 * 60
                'iMinValue = 6 * 60
                'dtNotifyUsageTime = Date.Today.AddMinutes(Int((iMaxValue - iMinValue + 1) * Rnd() + iMinValue))
                '' is notify time in the past ?
                'Do While dtNotifyUsageTime < Date.Now
                '    ' yes, add 1 hour
                '    dtNotifyUsageTime = dtNotifyUsageTime.AddHours(1)
                'Loop
                '' end of code added TJS 18/03/11

                Return True ' 10/03/09

            Else
                m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "DoStartup", "There was a problem retrieving all parmeter information, there may be a problem with your App.config file.") ' TJS 28/01/09 TJS 10/06/12
                Return False
            End If

        Catch Ex As Exception
            ' can't use m_ImportExportConfigFacade.SendErrorEmail here as it could be the cause of the error
            m_ErrorNotification.WriteErrorToEventLog("Error encountered in 'DoStartup'. " & Ex.Message & ", " & Ex.StackTrace) ' TJS 10/03/09 TJS 10/06/12
            Return False
        End Try
    End Function

    Public Function DoFTP() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/08 | Craig Griffin   | 1.0.0.0   | Original
        ' 22/02/09 | TJS             | 2009.1.08 | Mofified to cater for renaming of WriteLogProgressRecord
        ' 26/03/12 | TJS             | 2011.2.11 | Modified to ensure error handler works if ActiveSource not setr
        ' 10/06/12 | TJS             | 2012.1.05 | modified to use Error Notification object to simplify facade login/logout
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ff As LerrynFTP.FTP = Nothing
        Dim ActiveSource As SourceSettings, bFTPActive As Boolean

        Dim localFileList As String()

        ActiveSource = Nothing  ' TJS 26/03/12
        dtFTPDueAt = dtFTPDueAt.AddMinutes(5)
        Try
            bFTPActive = False
            ' are there any sources with active FTP ?
            For Each ActiveSource In Setts.ActiveSources ' TJS 28/01/09
                If ActiveSource.FTPEnabled Then ' TJS 28/01/09
                    bFTPActive = True
                    m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "FTP Input routine starting.") ' TJS 28/01/09 TJs 22/02/09 TJS 10/06/12
                    Exit For
                End If
            Next
            If bFTPActive Then
                For Each ActiveSource In Setts.ActiveSources ' TJS 28/01/09
                    If ActiveSource.FTPEnabled Then ' TJS 28/01/09
                        localFileList = Directory.GetFiles("File Output Directory to go here")
                        ftpHost = ""
                        ' Create an instance of the FTP Class.
                        ff = New LerrynFTP.FTP
                        ' Setup the FTP connection details
                        ff.RemoteHostFTPServer = ftpHost
                        ff.RemoteUser = ftpUser
                        ff.RemotePassword = ftpPwd
                        m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Connecting to the FTP Server - " & ftpHost) ' TJS 28/01/09 TJS 22/02/09 TJS 10/06/12
                        'WriteToLog(logProgress, "Checking for files to download" & sFile)
                        If (ff.Login()) Then
                            'See Coelima code for FTP code requirements



                        End If
                        m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "FTP Process complete, Disconnecting from the FTP Server - " & ftpHost) ' TJS 28/01/09 TJS 22/02/09 TJS 10/06/12
                        ff.CloseConnection()
                        Return True
                        Exit Function
                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
            End If
            Return True

        Catch Ex As Exception
            If ActiveSource IsNot Nothing Then ' TJS 26/03/12
                m_ErrorNotification.SendExceptionEmail(ActiveSource.XMLConfig, "DoFTP", Ex) ' TJS 28/01/09 TJS 26/03/12 TJS 10/06/12
            Else
                m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "DoFTP", Ex) ' TJS 26/03/12 TJS 10/06/12
            End If
            If ff IsNot Nothing Then
                ff.CloseConnection()
            End If
            Return False
        End Try

    End Function

    Public Function StripSpecialCharacters(ByVal sIn As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.   | Description
        '------------------------------------------------------------------------------------------
        ' 18/09/08 | Craig Griffin   | 1.0.0.0   | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If sIn = "," Or sIn = " ," Or sIn = ", " Or sIn = "" Or sIn = " " Then
            'Leave it
        Else
            sIn = sIn.Replace(",", "")
        End If
        sIn = sIn.Replace(Chr(13), " ")
        sIn = sIn.Replace(Chr(10), " ")

        StripSpecialCharacters = sIn
    End Function

    Public Function GetSettings() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Gets the settings from the eShopConnectConfig Table and checks for a valid 
        '                   licence in the LerrynLicences Table 
        '                   
        '                   Return Values - TRUE (Function worked correctly)
        '                                   FALSE (Function generated an error)
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/10/08 | Jonathan Foster | 1.0.0.0   | Original
        ' 28/01/09 | TJS             | 2009.1.01 | Modified to pass SourceCode_Dev000221 field directly to LoadXMLConfig
        ' 17/03/09 | TJS             | 2009.1.10 | Added File Input eShopCONNECTOR and modified to handle user sources
        ' 03/04/09 | TJS             | 2009.2.00 | Modified to write service version to log file
        ' 28/04/09 | TJS             | 2009.2.05 | Moved facade creation to DoStartp() to facilitate
        '                                        | auto update of settings after config change
        ' 07/07/09 | TJS             | 2009.3.00 | Added Volusion connector and added log message if product not activated
        ' 15/10/09 | TJS             | 2009.3.08 | Modified for Amazon direct connection
        ' 11/12/09 | TJS             | 2009.3.09 | modified to reload any amazon files created but not sent and added Channel Advisor connector
        ' 05/01/10 | TJS             | 2010.0.01 | Modified to cater for lead, prospect and customer import
        ' 13/06/10 | TJs             | 2010.0.07 | Modified to check Inhibit Web Posts when loading Amazon files to send 
        ' 19/08/10 | TJS             | 2010.1.00 | Corrected InhibitWebPosts initialisation and modified to use Source Code constants
        ' 18/10/10 | TJS             | 2010.1.06 | Corrected File_Import and Prospect_Import constant name
        ' 06/04/11 | TJS             | 2011.0.08 | Disabled Amazon client to allow dll signing so Web SErvices will work
        ' 09/07/11 | TJS             | 2011.1.00 | Moved Amazon file submission to new Amazon MWS connector and PollAmazon function in WebPollIO
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 10/06/12 | TJS             | 2012.1.05 | modified to use Error Notification object to simplify facade login/logout and to get updated activation if not currently activated
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim pRow As DataRow
        Dim sXML As String, iErrorCode As Integer, bInhibitWebPosts As Boolean ' TJs 13/06/10 TJS 10/06/12

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") ' TJs 13/06/10 TJS 19/08/10

            ' load config settings
            m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count > 0 Then
                sXML = m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221

                For Each pRow In m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Rows
                    sXML = pRow("ConfigSettings_Dev000221").ToString
                    Select Case pRow("SourceCode_Dev000221").ToString
                        Case GENERIC_IMPORT_SOURCE_CODE ' TJS 19/08/10
                            If Not m_ImportExportConfigFacade.IsActivated() Then ' TJS 10/06/12
                                ' now check for updated activations
                                m_ImportExportConfigFacade.GetActivationCode(iErrorCode) ' TJS 10/06/12
                                If iErrorCode > 0 Then ' TJS 10/06/12
                                    m_ErrorNotification.WriteErrorToEventLog("Unable to check for updated activations - error code is " & iErrorCode) ' TJS 10/06/12
                                Else
                                    m_ImportExportConfigFacade.ReCheckActivation() ' TJS 10/06/12
                                End If
                            End If
                            If m_ImportExportConfigFacade.IsActivated() Then
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 28/01/09
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Generic XML Import activated") ' TJS 10/03/09 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10
                            Else
                                ' can't use m_ImportExportConfigFacade.SendErrorEmail here as product not activated and config not set
                                ' has activation expired ?
                                If m_ImportExportConfigFacade.HasBeenActivated() Then ' TJS 07/07/09
                                    ' yes
                                    m_ErrorNotification.WriteErrorToEventLog("Your Activation code for " & PRODUCT_NAME & " has expired.") ' TJS 07/07/09 TJS 10/06/12
                                Else
                                    ' no
                                    m_ErrorNotification.WriteErrorToEventLog("You must activate " & PRODUCT_NAME & " first.") ' TJS 07/07/09 TJS 10/06/12
                                End If

                            End If

                        Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                            If m_ImportExportConfigFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 28/01/09
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Shop.com eShopCONNECTOR activated") ' TJS 10/03/09 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10
                            End If

                        Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                            If m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then ' TJS 28/01/09
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 28/01/09
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Amazon eShopCONNECTOR activated") ' TJS 10/03/09 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10
                            End If

                        Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 19/08/10
                            If m_ImportExportConfigFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then ' TJS 11/12/09
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 11/12/09
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Channel Advisor eShopCONNECTOR activated") ' TJS 11/12/09 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10
                            End If

                        Case FILE_IMPORT_SOURCE_CODE ' TJS 19/08/10 TJS/FA 18/10/10
                            ' File Import connector only valid on eShopCONNECT
                            If m_ImportExportConfigFacade.IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) And _
                                PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 17/03/09
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 17/03/09
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "File Input eShopCONNECTOR activated") ' TJS 17/03/09 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10
                            End If

                        Case VOLUSION_SOURCE_CODE ' TJS 19/08/10
                            If m_ImportExportConfigFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then ' TJS 07/07/09
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 07/07/09
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Volusion eShopCONNECTOR activated") ' TJS 07/07/09 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10
                            End If

                            ' start of code added TJS 19/08/10
                        Case MAGENTO_SOURCE_CODE
                            If m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Magento eShopCONNECTOR activated") ' TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row))
                            End If

                        Case ASP_STORE_FRONT_SOURCE_CODE
                            If m_ImportExportConfigFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "ASPDotNetStoreFront eShopCONNECTOR activated") ' TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row))
                            End If
                            ' end of code added TJS 19/08/10

                            ' start of code added TJS 02/12/11
                        Case EBAY_SOURCE_CODE
                            If m_ImportExportConfigFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "eBay eShopCONNECTOR activated") ' TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row))
                            End If
                            ' end of code added TJS 02/12/11

                            ' start of code added TJS 16/01/12
                        Case SEARS_COM_SOURCE_CODE
                            If m_ImportExportConfigFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Sears.com eShopCONNECTOR activated") ' TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row))
                            End If
                            ' end of code added TJS 16/01/12

                        Case PROSPECT_LEAD_IMPORT_SOURCE_CODE ' TJS 05/01/10 TJS 19/08/10 TJS/FA 18/10/10
                            If m_ImportExportConfigFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then ' TJS 05/01/10
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 05/01/10
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, pRow("SourceName_DEV000221").ToString & " activated") ' TJS 05/01/10 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10
                            End If

                            ' start of code added TJS 20/11/13
                        Case THREE_D_CART_SOURCE_CODE
                            If m_ImportExportConfigFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "3DCart eShopCONNECTOR activated")
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row))
                            End If
                            ' end of code added TJS 20/11/13

                        Case Else
                            ' check source is valid
                            If m_ImportExportConfigFacade.IsActivated() And ((pRow("InputHandler_DEV000221").ToString = "GenericXMLImport.ashx" And _
                                PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE) Or _
                                (pRow("InputHandler_DEV000221").ToString = "Windows Service" And PRODUCT_CODE = ORDERIMPORTER_BASE_PRODUCT_CODE)) Then ' TJS 17/03/09 
                                ' Valid Licence Found, populate Settings from the eShopCONNECT/OrderImport Config XML
                                bIsActivated = True ' TJS 17/03/09
                                m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, pRow("SourceName_DEV000221").ToString & " source activated") ' TJS 17/03/09 TJS 10/06/12
                                Setts.LoadXMLConfig(DirectCast(pRow, Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)) ' TJS 28/01/09 TJS 17/03/09 TJS 19/08/10

                            End If

                    End Select
                Next
            End If
            Return bIsActivated ' TJS 07/07/09

        Catch Ex As Exception
            ' can't use m_ImportExportConfigFacade.SendErrorEmail here as it could be the cause of the error
            m_ErrorNotification.WriteErrorToEventLog("Error encountered in 'GetSettings'. " & Ex.Message & ", " & Ex.StackTrace) ' TJS 10/06/12
            Return False
        End Try
    End Function

    Public Function NotifyUsage() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Send transaction usage info to our web service for transaction based charging 
        '                   
        '                   Return Values - TRUE (Function worked correctly)
        '                                   FALSE (Function generated an error)
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJs             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.05 | modified to use Error Notification object to simplify facade login/logout
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to send previous period value plus transaction counts
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected processing of new activation codes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try

            Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
            Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
            Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
            Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
            Dim XMLTemp As XDocument
            Dim XMLActivationList As System.Collections.Generic.IEnumerable(Of XElement), XMLActivationNode As XElement
            Dim encrypteddata As Byte(), salt As Byte(), vector As Byte()
            Dim strActivationDates As String, strSend As String, strLerrynCustomerCode As String, strPaymentPeriod As String ' TJS 01/05/14
            Dim strActivation As String, strProductCode As String, strAutoRenew As String, strDecrypted() As String ' TJS 01/05/14
            Dim strResponse As String, strDateParts() As String, iLoop As Integer ' TJS 02/08/13
            Dim dteEndFreeTrial As Date = Nothing, dteNextInvoiceDue As Date = Nothing, dteToday As Date ' TJS 02/08/13
            Dim dtePeriodStartDate As Date, dtePeriodEndDate As Date, dtePrevPeriodStartDate As Date, dtePrevPeriodEndDate As Date ' TJS 02/08/13
            Dim dteLastNotification As Date, dteExpiryDate As Date, dteYearStartDate As Date, dteYearEndDate As Date ' TJS 02/08/13

            dteToday = Date.Today ' TJS 02/08/13
            dtePeriodStartDate = DateSerial(dteToday.Year, dteToday.Month, 1) ' TJS 02/08/13
            dtePeriodEndDate = dtePeriodStartDate.AddMonths(1).AddDays(-1) ' TJS 02/08/13
            ' start of code added TJS 02/08/13
            ' are we in January ?
            If dteToday.Month = 1 Then
                ' yes, send annual values for last year
                dteYearStartDate = DateSerial(dteToday.Year - 1, 1, 1)
                ' previous period will be December last year
                dtePrevPeriodStartDate = DateSerial(dteToday.Year - 1, 12, 1)
            Else
                ' no, send annual values for year to date
                dteYearStartDate = DateSerial(dteToday.Year, 1, 1)
                ' previous period will be last month
                dtePrevPeriodStartDate = DateSerial(dteToday.Year, dteToday.Month - 1, 1)
            End If
            dteYearEndDate = dteYearStartDate.AddYears(1).AddDays(-1)
            dtePrevPeriodEndDate = dtePrevPeriodStartDate.AddMonths(1).AddDays(-1)
            ' end of code added TJS 02/08/13

            crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider

            ' make sure we have the latest licence data loaded
            m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                "ReadLerrynLicences_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(m_ImportExportConfigFacade.LatestActivationCode)
            If rowLicence IsNot Nothing Then
                If Not rowLicence.IsData_DEV000221Null And Not rowLicence.IsDataSalt_DEV000221Null And Not rowLicence.IsDataIV_DEV000221Null Then
                    encrypteddata = System.Convert.FromBase64String(rowLicence.Data_DEV000221)
                    salt = System.Convert.FromBase64String(rowLicence.DataSalt_DEV000221)
                    vector = System.Convert.FromBase64String(rowLicence.DataIV_DEV000221)

                    If Not encrypteddata Is Nothing AndAlso Not salt Is Nothing AndAlso Not vector Is Nothing Then
                        If encrypteddata.Length > 0 AndAlso salt.Length > 0 AndAlso vector.Length > 0 Then
                            strDecrypted = Split(crypto.Decrypt(encrypteddata, salt, vector), ":")
                            If strDecrypted.Length > 0 Then
                                If strDecrypted(0) <> "" Then
                                    strDateParts = Split(strDecrypted(0), "-")
                                    dteEndFreeTrial = DateSerial(CInt(strDateParts(0)), CInt(strDateParts(1)), CInt(strDateParts(2)))
                                End If
                                If strDecrypted(1) <> "" Then
                                    strDateParts = Split(strDecrypted(1), "-")
                                    dteNextInvoiceDue = DateSerial(CInt(strDateParts(0)), CInt(strDateParts(1)), CInt(strDateParts(2)))
                                End If
                            End If
                            ' element 2 is Payment Failed Flag
                            ' element 3 is Last Notification date
                            ' element 4 is Payment Period
                            ' element 5 is Auto Renewal flag
                        End If
                    End If
                End If
            End If

            strLerrynCustomerCode = ""
            dteLastNotification = Date.Today
            m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.System_DEV000221.TableName, _
                "ReadSystem_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If Not m_ImportExportDataset.System_DEV000221(0).IsCustCode_DEV000221Null Then
                encrypteddata = System.Convert.FromBase64String(m_ImportExportDataset.System_DEV000221(0).CustCode_DEV000221)
                salt = System.Convert.FromBase64String(m_ImportExportDataset.System_DEV000221(0).CCSalt_DEV000221)
                vector = System.Convert.FromBase64String(m_ImportExportDataset.System_DEV000221(0).CCIV_DEV000221)

                If Not encrypteddata Is Nothing AndAlso Not salt Is Nothing AndAlso Not vector Is Nothing Then
                    If encrypteddata.Length > 0 AndAlso salt.Length > 0 AndAlso vector.Length > 0 Then
                        strLerrynCustomerCode = crypto.Decrypt(encrypteddata, salt, vector)
                    End If
                End If
            End If

            m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.SystemCompanyInformation.TableName, _
                "ReadSystemCompanyInformation"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

            strSend = "<LerrynNotifyUsage><Login><Source>ISIntegrated</Source><Password>K$0eW*zU2B</Password></Login>"
            strSend = strSend & "<LerrynCustCode>" & strLerrynCustomerCode & "</LerrynCustCode><ProductCode>" & PRODUCT_CODE
            strSend = strSend & "</ProductCode><Activation>" & m_ImportExportConfigFacade.LatestActivationCode.Substring(0, 5)
            strSend = strSend & "-" & m_ImportExportConfigFacade.LatestActivationCode.Substring(5, 5) & "-"
            strSend = strSend & m_ImportExportConfigFacade.LatestActivationCode.Substring(10, 5) & "-"
            strSend = strSend & m_ImportExportConfigFacade.LatestActivationCode.Substring(15, 5) & "-"
            strSend = strSend & m_ImportExportConfigFacade.LatestActivationCode.Substring(20, 5) & "-"
            strSend = strSend & m_ImportExportConfigFacade.LatestActivationCode.Substring(25, 5) & "-"
            strSend = strSend & m_ImportExportConfigFacade.LatestActivationCode.Substring(30, 5) & "</Activation><SystemID>"
            strSend = strSend & m_ImportExportConfigFacade.GetSystemLicenceID(False) & "</SystemID><SysIDType>ISSysID</SysIDType>"
            strSend = strSend & "<CompanyName>" & m_ImportExportDataset.SystemCompanyInformation(0).CompanyName & "</CompanyName><Usage>" ' TJS 02/08/13
            strSend = strSend & "<PrevPeriodStart>" & dtePrevPeriodStartDate.Year & "-" & Right("00" & dtePrevPeriodStartDate.Month, 2) & "-"
            strSend = strSend & Right("00" & dtePrevPeriodStartDate.Day, 2) & "</PrevPeriodStart><PrevPeriodEnd>" & dtePrevPeriodEndDate.Year
            strSend = strSend & "-" & Right("00" & dtePrevPeriodEndDate.Month, 2) & "-" & Right("00" & dtePrevPeriodEndDate.Day, 2)
            strSend = strSend & "</PrevPeriodEnd><PeriodStart>" & dtePeriodStartDate.Year & "-" & Right("00" & dtePeriodStartDate.Month, 2)
            strSend = strSend & "-" & Right("00" & dtePeriodStartDate.Day, 2) & "</PeriodStart><PeriodEnd>" & dtePeriodEndDate.Year & "-"
            strSend = strSend & Right("00" & dtePeriodEndDate.Month, 2) & "-" & Right("00" & dtePeriodEndDate.Day, 2) & "</PeriodEnd>"
            strSend = strSend & "<YearStart>" & dteYearStartDate.Year & "-" & Right("00" & dteYearStartDate.Month, 2) & "-"
            strSend = strSend & Right("00" & dteYearStartDate.Day, 2) & "</YearStart><YearEnd>" & dteYearEndDate.Year
            strSend = strSend & "-" & Right("00" & dteYearEndDate.Month, 2) & "-" & Right("00" & dteYearEndDate.Day, 2)
            strSend = strSend & "</YearEnd><Currency>" & m_ImportExportDataset.SystemCompanyInformation(0).CurrencyCode & "</Currency>"

            ' load config settings
            m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count > 0 Then
                For iLoop = 0 To m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count - 1
                    If m_ImportExportConfigFacade.IsConnectorActivated(m_ImportExportConfigFacade.ConnectorProductCode(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).InputHandler_DEV000221)) Or _
                       (m_ImportExportConfigFacade.HasConnectorBeenActivated(m_ImportExportConfigFacade.ConnectorProductCode(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).InputHandler_DEV000221)) And _
                        m_ImportExportConfigFacade.ConnectorActivationExpires(m_ImportExportConfigFacade.ConnectorProductCode(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).InputHandler_DEV000221)) > Date.Today.AddMonths(-2)) Then

                        m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.UsageInPeriod.TableName, _
                            "GetUsageInPeriod_DEV000221", "@SourceCode", m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221, _
                            "@PrevPeriodStartDate", Right("00" & dtePrevPeriodStartDate.Month, 2) & "/" & Right("00" & dtePrevPeriodStartDate.Day, 2) & "/" & dtePrevPeriodStartDate.Year, _
                            "@PrevPeriodEndDate", Right("00" & dtePrevPeriodEndDate.Month, 2) & "/" & Right("00" & dtePrevPeriodEndDate.Day, 2) & "/" & dtePrevPeriodEndDate.Year, _
                            "@PeriodStartDate", Right("00" & dtePeriodStartDate.Month, 2) & "/" & Right("00" & dtePeriodStartDate.Day, 2) & "/" & dtePeriodStartDate.Year, _
                            "@PeriodEndDate", Right("00" & dtePeriodEndDate.Month, 2) & "/" & Right("00" & dtePeriodEndDate.Day, 2) & "/" & dtePeriodEndDate.Year, _
                            "@YearStartDate", Right("00" & dteYearStartDate.Month, 2) & "/" & Right("00" & dteYearStartDate.Day, 2) & "/" & dteYearStartDate.Year, _
                            "@YearEndDate", Right("00" & dteYearEndDate.Month, 2) & "/" & Right("00" & dteYearEndDate.Day, 2) & "/" & dteYearEndDate.Year}}, _
                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 02/08/13

                        strProductCode = m_ImportExportConfigFacade.ConnectorProductCode(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).InputHandler_DEV000221)
                        strSend = strSend & "<Connector><ProductCode>" & strProductCode & "</ProductCode><Activation>"
                        strSend = strSend & m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode).Substring(0, 5) & "-"
                        strSend = strSend & m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode).Substring(5, 5) & "-"
                        strSend = strSend & m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode).Substring(10, 5) & "-"
                        strSend = strSend & m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode).Substring(15, 5) & "-"
                        strSend = strSend & m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode).Substring(20, 5) & "-"
                        strSend = strSend & m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode).Substring(25, 5) & "-"
                        strSend = strSend & m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode).Substring(30, 5)
                        strSend = strSend & "</Activation><PrevPeriodImportValue>" & Format(m_ImportExportDataset.UsageInPeriod(0).PrevPeriodValue, "0.00") ' TJS 02/08/13
                        strSend = strSend & "</PrevPeriodImportValue><PrevPeriodImportCount>" & Format(m_ImportExportDataset.UsageInPeriod(0).PrevPeriodCount, "0.00") ' TJS 02/08/13
                        strSend = strSend & "</PrevPeriodImportCount><PeriodImportValue>" & Format(m_ImportExportDataset.UsageInPeriod(0).PeriodValue, "0.00") ' TJS 02/08/13
                        strSend = strSend & "</PeriodImportValue><PeriodImportCount>" & Format(m_ImportExportDataset.UsageInPeriod(0).PeriodCount, "0.00") ' TJS 02/08/13
                        strSend = strSend & "</PeriodImportCount><TotalImportValue>" & Format(m_ImportExportDataset.UsageInPeriod(0).TotalValue, "0.00") ' TJS 02/08/13
                        strSend = strSend & "</TotalImportValue><TotalImportCount>" & Format(m_ImportExportDataset.UsageInPeriod(0).TotalCount, "0.00") ' TJS 02/08/13
                        strSend = strSend & "</TotalImportCount></Connector>"
                    End If
                Next
            End If

            m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.UsageInPeriod.TableName, _
                "GetUsageInPeriod_DEV000221", "@PrevPeriodStartDate", Right("00" & dtePrevPeriodStartDate.Month, 2) & "/" & Right("00" & dtePrevPeriodStartDate.Day, 2) & "/" & dtePrevPeriodStartDate.Year, _
                "@PrevPeriodEndDate", Right("00" & dtePrevPeriodEndDate.Month, 2) & "/" & Right("00" & dtePrevPeriodEndDate.Day, 2) & "/" & dtePrevPeriodEndDate.Year, _
                "@PeriodStartDate", Right("00" & dtePeriodStartDate.Month, 2) & "/" & Right("00" & dtePeriodStartDate.Day, 2) & "/" & dtePeriodStartDate.Year, _
                "@PeriodEndDate", Right("00" & dtePeriodEndDate.Month, 2) & "/" & Right("00" & dtePeriodEndDate.Day, 2) & "/" & dtePeriodEndDate.Year, _
                "@YearStartDate", Right("00" & dteYearStartDate.Month, 2) & "/" & Right("00" & dteYearStartDate.Day, 2) & "/" & dteYearStartDate.Year, _
                "@YearEndDate", Right("00" & dteYearEndDate.Month, 2) & "/" & Right("00" & dteYearEndDate.Day, 2) & "/" & dteYearEndDate.Year}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 02/08/13

            strSend = strSend & "<PrevPeriodImportValueAllSources>" & Format(m_ImportExportDataset.UsageInPeriod(0).PrevPeriodValue, "0.00") & "</PrevPeriodImportValueAllSources>" ' TJS 02/08/13
            strSend = strSend & "<PrevPeriodImportCountAllSources>" & Format(m_ImportExportDataset.UsageInPeriod(0).PrevPeriodCount, "0.00") & "</PrevPeriodImportCountAllSources>" ' TJS 02/08/13
            strSend = strSend & "<PeriodImportValueAllSources>" & Format(m_ImportExportDataset.UsageInPeriod(0).PeriodValue, "0.00") & "</PeriodImportValueAllSources>"
            strSend = strSend & "<PeriodImportCountAllSources>" & Format(m_ImportExportDataset.UsageInPeriod(0).PeriodCount, "0.00") & "</PeriodImportCountAllSources>" ' TJS 02/08/13
            strSend = strSend & "<TotalImportValueAllSources>" & Format(m_ImportExportDataset.UsageInPeriod(0).TotalValue, "0.00") & "</TotalImportValueAllSources>"
            strSend = strSend & "<TotalImportCountAllSources>" & Format(m_ImportExportDataset.UsageInPeriod(0).TotalCount, "0.00") & "</TotalImportCountAllSources>" ' TJS 02/08/13
            strSend = strSend & "</Usage></LerrynNotifyUsage>"

            ' send to LerrynSecure.com (or the URL defined in the Registry)
            ' start of code replaced TJS 18/07/11
            Try
                WebSubmit = System.Net.WebRequest.Create(m_ImportExportConfigFacade.GetISPluginBaseURL() & "NotifyUsage.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = strSend.Length
                WebSubmit.Timeout = 30000

                byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())

                strResponse = reader.ReadToEnd()
                If Not String.IsNullOrEmpty(strResponse) Then ' TJS 02/08/13
                    XMLResponse = XDocument.Parse(strResponse)
                    If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "LerrynNotifyUsage/Status") = "OK" Then
                        m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                            "ReadLerrynLicences_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

                        rowLicence = Nothing ' TJS 01/05/14
                        strPaymentPeriod = "M" ' TJS 01/05/14
                        strAutoRenew = "F" ' TJS 01/05/14

                        ' start of code moved TJS 02/08/13
                        XMLActivationList = XMLResponse.XPathSelectElements("LerrynNotifyUsage/NewActivation")
                        If XMLActivationList IsNot Nothing Then
                            If m_ImportExportConfigFacade.GetXMLElementListCount(XMLActivationList) > 0 Then
                                For Each XMLActivationNode In XMLActivationList
                                    XMLTemp = XDocument.Parse(XMLActivationNode.ToString)
                                    strActivation = m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "NewActivation/ActivationCode").Replace("-", "")
                                    strProductCode = m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "NewActivation/ProductCode")
                                    dteExpiryDate = m_ImportExportConfigFacade.ConvertXMLDate(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "NewActivation/ExpiryDate"))
                                    dteNextInvoiceDue = m_ImportExportConfigFacade.ConvertXMLDate(m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "NewActivation/NextInvoiceDate"))
                                    strPaymentPeriod = m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "NewActivation/PaymentPeriod") ' TJS 01/05/14
                                    If m_ImportExportConfigFacade.GetXMLElementText(XMLTemp, "NewActivation/AutoRenew").ToUpper = "YES" Then ' TJS 01/05/14
                                        strAutoRenew = "T" ' TJS 01/05/14
                                    End If
                                    ' get 
                                    rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(m_ImportExportConfigFacade.ConnectorLatestActivationCode(strProductCode))
                                    If rowLicence IsNot Nothing Then
                                        rowLicence.LicenceCode_DEV000221 = strActivation
                                        rowLicence.ProductCode_DEV000221 = strProductCode
                                        rowLicence.CodeExpires_DEV000221 = dteExpiryDate
                                    Else
                                        m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "NotifyUsage", "Unable to update activation, existing Activation Code " & m_ImportExportConfigFacade.LatestActivationCode & " not found ", XMLResponse.ToString) ' TJS 10/06/12
                                    End If
                                Next
                            End If
                        End If
                        ' end of code moved TJS 02/08/13

                        If rowLicence Is Nothing Then ' TJS 01/05/14
                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(m_ImportExportConfigFacade.LatestActivationCode)
                        End If
                        If rowLicence IsNot Nothing Then
                            strActivationDates = dteEndFreeTrial.Year & "-" & Right("00" & dteEndFreeTrial.Month, 2) & "-" & Right("00" & dteEndFreeTrial.Day, 2) & ":"
                            strActivationDates = strActivationDates & dteNextInvoiceDue.Year & "-" & Right("00" & dteNextInvoiceDue.Month, 2) & "-" & Right("00" & dteNextInvoiceDue.Day, 2)
                            If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "LerrynNotifyUsage/PaymentFailed").ToUpper = "YES" Then
                                strActivationDates = strActivationDates & ":T:"
                            Else
                                strActivationDates = strActivationDates & ":F:"
                            End If
                            strActivationDates = strActivationDates & dteLastNotification.Year & "-" & Right("00" & dteLastNotification.Month, 2) & "-" & Right("00" & dteLastNotification.Day, 2)
                            ' element 4 is Payment Period
                            ' element 5 is Auto Renewal flag
                            strActivationDates = strActivationDates & ":" & strPaymentPeriod & ":" & strAutoRenew ' TJS 01/05/14

                            If Not rowLicence.IsDataSalt_DEV000221Null AndAlso Not rowLicence.IsDataIV_DEV000221Null Then ' TJS 02/08/13
                                salt = System.Convert.FromBase64String(rowLicence.DataSalt_DEV000221)
                                vector = System.Convert.FromBase64String(rowLicence.DataIV_DEV000221)
                            Else
                                salt = Nothing ' TJS 02/08/13
                                vector = Nothing ' TJS 02/08/13
                            End If
                            If salt Is Nothing Or vector Is Nothing Then
                                salt = crypto.GenerateSalt
                                vector = crypto.GenerateVector

                            ElseIf salt.Length = 0 Or vector.Length = 0 Then
                                salt = crypto.GenerateSalt
                                vector = crypto.GenerateVector

                            End If
                            rowLicence.Data_DEV000221 = crypto.Encrypt(strActivationDates, salt, vector)
                            rowLicence.DataSalt_DEV000221 = System.Convert.ToBase64String(salt)
                            rowLicence.DataIV_DEV000221 = System.Convert.ToBase64String(vector)

                            ' start of code added TJS 02/08/13
                            If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "LerrynNotifyUsage/CustomerCode") <> strLerrynCustomerCode Then
                                strLerrynCustomerCode = m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "LerrynNotifyUsage/CustomerCode")
                                salt = crypto.GenerateSalt
                                vector = crypto.GenerateVector

                                m_ImportExportDataset.System_DEV000221(0).CustCode_DEV000221 = crypto.Encrypt(strLerrynCustomerCode, salt, vector)
                                m_ImportExportDataset.System_DEV000221(0).CCSalt_DEV000221 = System.Convert.ToBase64String(salt)
                                m_ImportExportDataset.System_DEV000221(0).CCIV_DEV000221 = System.Convert.ToBase64String(vector)
                            End If
                            ' end of code added TJS 02/08/13
                        Else
                            m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "NotifyUsage", "Unable to update activation, existing Activation Code " & m_ImportExportConfigFacade.LatestActivationCode & " not found ", XMLResponse.ToString) ' TJS 10/06/12
                        End If

                        m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                            "CreateLerrynLicences_DEV000221", "UpdateLerrynLicences_DEV000221", "DeleteLerrynLicences_DEV000221"}, _
                            New String() {m_ImportExportDataset.System_DEV000221.TableName, _
                            "CreateSystem_DEV000221", "UpdateSystem_DEV000221", "DeleteSystem_DEV000221"}}, _
                            Interprise.Framework.Base.Shared.TransactionType.None, "Update Lerryn Activations", False) ' TJS 02/08/13
                        m_ImportExportConfigFacade.ReCheckActivation()
                    End If
                End If

            Catch ex As Exception
                m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "NotifyUsage", ex)
                Return False

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try
            ' end of code replaced TJS 02/12/11

            crypto = Nothing ' TJS 02/08/13

        Catch ex As Exception
            m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "NotifyUsage", ex)
            Return False

        End Try

        Return True

    End Function

    Public Function CheckForAutoRenew() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Checks the Auto Renew flag in the activation data and also polls our 
        '                   server for updated activations
        '                   
        '                   Return Values - TRUE (Auto Renew of activations is active)
        '                                   FALSE (Auto Renew of activations NOT active)
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJs             | 2011.0.01 | Function added
        ' 10/06/12 | TJS             | 2012.1.05 | modified to use Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim encrypteddata As Byte(), salt As Byte(), vector As Byte()
        Dim strDecrypted() As String, iErrorCode As Integer = 0
        Dim bAutoRenewal As Boolean = False

        crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider

        ' make sure we have the latest licence data loaded
        m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
            "ReadLerrynLicences_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(m_ImportExportConfigFacade.LatestActivationCode)
        If rowLicence IsNot Nothing Then
            If Not rowLicence.IsData_DEV000221Null And Not rowLicence.IsDataSalt_DEV000221Null And Not rowLicence.IsDataIV_DEV000221Null Then
                encrypteddata = System.Convert.FromBase64String(rowLicence.Data_DEV000221)
                salt = System.Convert.FromBase64String(rowLicence.DataSalt_DEV000221)
                vector = System.Convert.FromBase64String(rowLicence.DataIV_DEV000221)

                If Not encrypteddata Is Nothing AndAlso Not salt Is Nothing AndAlso Not vector Is Nothing Then
                    If encrypteddata.Length > 0 AndAlso salt.Length > 0 AndAlso vector.Length > 0 Then
                        strDecrypted = Split(crypto.Decrypt(encrypteddata, salt, vector), ":")
                        ' element 0 is End Free Trial date
                        ' element 1 is Next Invoice Due date
                        ' element 2 is Payment Failed Flag
                        ' element 3 is Last Notification date
                        ' element 4 is Payment Period
                        If strDecrypted.Length >= 6 Then
                            If strDecrypted(5) = "T" Then
                                bAutoRenewal = True
                            End If
                        End If
                    End If
                End If
            End If
        End If

        ' now check for updated activations
        m_ImportExportConfigFacade.GetActivationCode(iErrorCode)
        If iErrorCode > 0 Then
            m_ErrorNotification.SendExceptionEmail(m_ImportExportConfigFacade.SourceConfig, "CheckForAutoRenew", "Unable to check for updated activations - error code is " & iErrorCode, "") ' TJS 10/06/12
        End If

        Return bAutoRenewal

    End Function

    Public Sub DoSourceSpecificDailyFunctions()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ' 23/01/14 | TJS             | 2013.4.06 | Added log message
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ActiveSource As SourceSettings
        Dim strLogMessage As String, strErrorDetails As String ' TJS 23/01/14
        Dim bInhibitWebPosts As Boolean, bChangesMade As Boolean ' TJS 23/01/14

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")

            ' are there any sources with daily tasks ?
            For Each ActiveSource In Setts.ActiveSources
                If ActiveSource.SourceCode = MAGENTO_SOURCE_CODE And m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                    bChangesMade = False ' TJS 23/01/14
                    strErrorDetails = "" ' TJS 23/01/14
                    UpdateMagentoSpecialPrices(ActiveSource, bChangesMade, strErrorDetails) ' TJS 23/01/14
                    ' start of code added TJS 23/01/14
                    strLogMessage = "Magento Special Prices Update run"
                    If bChangesMade Then
                        strLogMessage = strLogMessage & " - one or more products had their Magento Special Price details updated"
                    Else
                        strLogMessage = strLogMessage & " - no Megento Special Price changes were found to apply"
                    End If
                    m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, strLogMessage)
                    If strErrorDetails <> "" Then
                        m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "DoSourceSpecificDailyFunctions", strErrorDetails)
                    End If
                    ' end of code added TJS 23/01/14
                End If
                If bShutDownInProgress Then
                    Exit For
                End If
            Next

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "DoSourceSpecificDailyFunctions", ex)

        End Try

    End Sub

End Module
