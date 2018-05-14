' eShopCONNECT for Connected Business - Windows Service
' Module: ServiceMain.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 20 November 2013

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const ' TJS 10/06/12
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports System.Threading
Imports System.Data
Imports System.Data.OleDb

Public Class ServiceMain

    Public bShutdownTime As Boolean = False
    Private dteLastTimerTick As Date ' TJS 13/04/10
    Public tmr As System.Threading.Timer = Nothing ' TJS 13/06/10
    Private ts As TimerState ' TJS 13/06/10

    ' Enable these functions ONLY when building with TestForm included as a Windows application
    'Public Sub TestStart(ByVal args() As String)
    '    OnStart(args)
    'End Sub

    'Public Sub TestStop()
    '    OnStop()
    'End Sub

    ' www.dynenttech.com davidonelson 5/8/2018
    ' Setup debugging of the Windows Service
    ' (1) For Lerryn.WindowsService.eShopCONNECT project
    '       (a) Set as startup project
    '       (b) Set project application type from "Windows Service" to "Windows Forms Application"
    '       (c) Set project application startup object from "ServiceMain" to "Sub Main"
    '       (d) Create/Set registry entry: [HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Lerryn\ISPlugins] "InhibitWebPosts"="Yes"
    '           See Lerryn.ISPlugins.InhibitWebPosts.reg in root folder
    '           Code below will let you know if the program picked up the registry entry as expected
    '       (e) Set breakpoints in these functions to start with
    '               ServiceMain.TestStartupAndStop
    '               ServiceMain.OnStart
    '               ServiceMain.timerCallback
    '               ServiceMain.RunRoutines
    '       (f) Use the CB App Configuration Tool to make sure your windows service project is pointing to the correct database
    '               Set connection in this file Server\Lerryn.WindowsService.eShopCONNECT\App.Config

    ' Controlling function if running in VS debugger
    Protected Sub TestStartupAndStop()
        Dim args() As String

        ' Check to see if bInhibitWebPosts has been set, and inform the developer
        Dim ErrorNotification As New Facade.ImportExport.ErrorNotification ' TJS 10/06/12
        Dim ImportExportDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Dim ImportExportConfigFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(ImportExportDataset, ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
        Dim bInhibitWebPosts As Boolean = ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES"
        If bInhibitWebPosts Then
            MsgBox("WARNING: bInhibitWebPosts is TRUE (web site updates WILL NOT BE happening)")
        Else
            MsgBox("WARNING: bInhibitWebPosts is FALSE (web site updates WILL BE happening)")
        End If

        Me.OnStart(args)
        ' Sleep a good long time, and let the timer trigger until you are done debugging
        Threading.Thread.Sleep(10000000)
        Me.OnStop()
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/09/08 | Craig Griffin   | 1.0.0.0   | Original
        ' 27/01/09 | TJS             | 2009.1.01 | Modified to initialise bIsActivated
        ' 02/02/09 | TJS             | 2009.1.04 | modified to initialise dtStatusUpdateDueAt and dtInventoryUpdateDueAt
        ' 22/02/09 | TJS             | 2009.1.08 | Mofified to cater for renaming of WriteLogProgressRecord
        ' 07/07/09 | TJS             | 2009.3.00 | Modified to include Web Input Polling and stop the service if config not ok
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to check re-activation every midnight 
        ' 13/06/10 | TJS             | 2010.0.07 | Modified callback timer settings to prevent intermittent operation
        '                                        | and to cater for failed DB connection on startup
        ' 22/03/11 | TJS             | 2011.0.03 | Modified to use WriteErrorToEventLog if ServiceState 1 error occurs
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to sign out whilst idle
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 23/07/13 | TJS             | 2013.1.31 | Modified to allow setting of shutdown and startup times in registry
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to initialise bShutDownInProgress
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strShutDownTime As String, strStartUpTime As String, dtToday As Date ' TJS 23/07/13

        Thread.Sleep(100)
        dteLastTimerTick = Date.Today ' TJS 13/04/10
        dtStatusUpdateDueAt = Date.Now ' TJS 02/02/09
        dtInventoryUpdateDueAt = Date.Now ' TJS 02/02/09
        dtFileCreationDueAt = Date.Now
        dtFileReadDueAt = Date.Now
        dtFTPDueAt = Date.Now
        dtShutDownTime = DateAdd(DateInterval.Minute, -2, Now)
        dtStartUpTime = DateAdd(DateInterval.Minute, -1, Now)
        bIsActivated = False ' TJS 27/01/09
        bFTPRequired = False ' TJS 22/02/09
        bFileImportRequired = False ' TJS 22/02/09
        bWebIORequired = False ' TJS 07/07/09
        bFileCreationRequired = False ' TJS 22/02/09
        bShutDownInProgress = False ' TJS 02/08/13

        System.Threading.Thread.Sleep(10000)
        ' create timer, but don't set callback yet
        ts = New TimerState ' TJS 13/06/10
        tmr = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf timerCallback), ts, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite) ' TJS 13/06/10
        m_ErrorNotification = New Lerryn.Facade.ImportExport.ErrorNotification ' TJS 10/06/12
        m_ErrorNotification.BaseProductName = PRODUCT_NAME

        ' do startup code
        If DoStartup(ts) Then ' TJS 13/06/10
            ' startup ok
            bConfigOK = True ' TJS 20/05/09
            bFirstPoll = True ' TJS 14/11/09
            ' start of code added TJS 23/07/13
            strShutDownTime = m_ImportExportConfigFacade.GetField("ShutDownTime_DEV000221", "LerrynImportExportServiceAction_DEV000221")
            strStartUpTime = m_ImportExportConfigFacade.GetField("StartUpTime_DEV000221", "LerrynImportExportServiceAction_DEV000221")
            If Not String.IsNullOrEmpty(strShutDownTime) AndAlso Not String.IsNullOrEmpty(strStartUpTime) Then
                If strShutDownTime.Length = 5 AndAlso strStartUpTime.Length = 5 AndAlso _
                Mid(strShutDownTime, 3, 1) = ":" AndAlso Mid(strStartUpTime, 3, 1) = ":" AndAlso _
                IsNumeric(Mid(strShutDownTime, 1, 2)) AndAlso IsNumeric(Mid(strShutDownTime, 4, 2)) AndAlso _
                IsNumeric(Mid(strStartUpTime, 1, 2)) AndAlso IsNumeric(Mid(strStartUpTime, 4, 2)) Then
                    dtToday = Date.Today
                    dtShutDownTime = dtToday.AddHours(CDbl(Mid(strShutDownTime, 1, 2))).AddMinutes(CDbl(Mid(strShutDownTime, 4, 2)))
                    dtStartUpTime = dtToday.AddHours(CDbl(Mid(strStartUpTime, 1, 2))).AddMinutes(CDbl(Mid(strStartUpTime, 4, 2)))
                    ' is startup time after shutdown time ?
                    If dtStartUpTime < dtShutDownTime Then
                        ' yes, startup time must be next day
                        dtStartUpTime = dtStartUpTime.AddDays(1)
                    End If
                    ' is startup time in the past ?
                    If dtStartUpTime < Date.Now Then
                        ' yes, must be tomorrow
                        dtShutDownTime = dtShutDownTime.AddDays(1)
                        dtStartUpTime = dtStartUpTime.AddDays(1)
                    End If
                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "OnStart", "ShutdownTime and/or StartUpTime invalid") ' TJS 13/11/13
                End If
            ElseIf Not String.IsNullOrEmpty(strShutDownTime) Or Not String.IsNullOrEmpty(strStartUpTime) Then
                m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "OnStart", "ShutdownTime and/or StartUpTime invalid") ' TJS 13/11/13
            End If
            ' end of code added TJS 23/07/13
            ' set callback for 15 seconds
            tmr.Change(15000, 15000) ' TJS 13/06/10
            m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Main Service Timer Started") ' TJS 28/01/09 TJS 22/02/09 TJS 10/06/12

            Try ' TJS 10/03/12
                ' now logout
                Interprise.Facade.Base.SimpleFacade.Instance.SignOut() ' TJS 10/03/12
                m_ImportExportConfigFacade.Dispose() ' TJS 10/06/12
                m_ImportExportDataset.Dispose() ' TJS 10/06/12

            Catch ex As Exception
                ' ignore error on logout as function does not exist pre SP5
            End Try

        ElseIf ts.ServiceState = 1 Then ' TJS 13/06/10
            ' set callback for 10 minute
            tmr.Change(600000, 600000) ' TJS 13/06/10
            ' can't use m_ImportExportConfigFacade.SendErrorEmail here as it could be the cause of the error
            m_ErrorNotification.WriteErrorToEventLog("Main Service Timer Started, but unable to connect to database") ' TJS 13/06/10 TJS 22/02/11 TJS 10/06/12

        Else
            ' can't use m_ImportExportConfigFacade.SendErrorEmail here as it could be the cause of the error
            m_ErrorNotification.WriteErrorToEventLog("Service failed to start correctly, there may be an error in your " & PRODUCT_NAME & " configuration settings.") ' TJS 10/06/12
            ' This line will error when running as a Windows Application during testing
            Me.Stop() ' TJS 07/07/09
        End If
    End Sub

    Protected Overrides Sub OnShutdown()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/08/13 | TJS             | 2013.2.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        bShutDownInProgress = True
        If AmazonConnection IsNot Nothing Then
            AmazonConnection.ShutDownInProgress = True
        End If
        m_ImportExportConfigFacade.WriteLogProgressRecord("System Shutdown detected")
        MyBase.OnShutdown()

    End Sub

    Protected Overrides Sub OnStop()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/03/09 | TJS             | 2009.1.10 | Mofified to write log message and dispose of objects
        ' 13/06/10 | TJS             | 2010.0.07 | Modified callback timer settings to prevent intermittent operation
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to sign out on exit
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            ' stop the timer
            tmr.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite) ' TJS 13/06/10
            m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, "Main Service Timer Stopped") ' TJS 15/03/09 TJS 10/06/12
            If m_ImportExportConfigFacade IsNot Nothing Then ' TJS 15/03/09
                Try ' TJS 10/03/12
                    ' now logout
                    Interprise.Facade.Base.SimpleFacade.Instance.SignOut() ' TJS 10/03/12
                    m_ImportExportConfigFacade.Dispose() ' TJS 10/06/12
                    m_ImportExportDataset.Dispose() ' TJS 10/06/12

                Catch ex As Exception
                    ' ignore error on logout as function does not exist pre SP5
                End Try
                m_ImportExportConfigFacade.Dispose() ' TJS 15/03/09
                m_ImportExportDataset.Dispose() ' TJS 15/03/09
            End If

        Catch ex As Exception
            ' can't use m_ImportExportConfigFacade.SendErrorEmail here as it could be the cause of the error
            m_ErrorNotification.WriteErrorToEventLog("Service failed to stop correctly -n " & ex.Message & ", " & ex.StackTrace) ' TJS 15/03/09 TJS 10/06/12

        End Try

    End Sub

    Private Sub timerCallback(ByVal state As Object)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 22/02/09 | TJS             | 2009.1.08 | Mofified to cater for renaming of WriteLogProgressRecord
        ' 13/06/10 | TJS             | 2010.0.07 | Modified callback timer settings to prevent intermittent operation
        '                                        | and to cater for failed DB connection on startup
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to sign out whilst idle
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to ignore all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim s As TimerState = DirectCast(state, TimerState)

        ' inhibit timer whilst processing
        tmr.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite) ' TJS 13/06/10
        If s.ServiceState = 2 Then ' TJS 13/06/10
            s.TimerCounter = s.TimerCounter + 1
            If Not bShutDownInProgress Then ' TJS 02/08/13
                If Not RunRoutines() Then ' TJS 13/06/10
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Run Routines failed to complete successfully.") ' TJS 28/01/09 TJS 22/02/09
                End If
                Try ' TJS 10/03/12
                    ' now logout
                    Interprise.Facade.Base.SimpleFacade.Instance.SignOut() ' TJS 10/03/12
                    m_ImportExportConfigFacade.Dispose() ' TJS 10/06/12
                    m_ImportExportDataset.Dispose() ' TJS 10/06/12

                Catch ex As Exception
                    ' ignore error on logout as function does not exist pre SP5
                End Try
                ' set callback for 15 seconds
                tmr.Change(15000, 15000) ' TJS 13/06/10
            End If

        ElseIf s.ServiceState = 1 Then ' TJS 13/06/10
            If Not bShutDownInProgress Then ' TJS 02/08/13
                ' do startup code again
                If DoStartup(ts) Then ' TJS 13/06/10
                    ' startup ok
                    bConfigOK = True ' TJS 13/06/10
                    bFirstPoll = True ' TJS 13/06/10
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Database connection now established, Main Service Timer now running") ' TJS 13/06/10
                    Try ' TJS 10/03/12
                        ' now logout
                        Interprise.Facade.Base.SimpleFacade.Instance.SignOut() ' TJS 10/03/12
                        m_ImportExportConfigFacade.Dispose() ' TJS 10/06/12
                        m_ImportExportDataset.Dispose() ' TJS 10/06/12

                    Catch ex As Exception
                        ' ignore error on logout as function does not exist pre SP5
                    End Try
                    ' set callback for 15 seconds
                    tmr.Change(15000, 15000) ' TJS 13/06/10

                Else
                    ' set callback for 10 minute
                    tmr.Change(600000, 600000) ' TJS 13/06/10
                End If
            End If
        End If

    End Sub

    Private Function RunRoutines() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/09/08 | Craig Griffin   | 1.0.0.0   | Original
        ' 02/02/09 | TJS             | 2009.1.04 | modified to update dtStatusUpdateDueAt and dtInventoryUpdateDueAt
        '                                        | and added code for Status and Inventory updates
        ' 22/02/09 | TJS             | 2009.1.08 | Mofified to cater for renaming of WriteLogProgressRecord
        ' 29/04/09 | TJS             | 2009.2.05 | modified to check Config Update Required flag and reload settings if set
        ' 02/06/09 | TJS             | 2009.2.10 | Modified to inhibit Status and Inventory updates on Order Importer build
        ' 07/07/09 | TJS             | 2009.3.00 | Modified to include Web Input Polling
        ' 27/11/09 | TJS             | 2009.3.09 | Modified to only reset bFirstPoll if at least one timer action occured
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to check re-activation every midnight and added registry key to override timer interval
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for Magento and ASLDotNetStoreFront connector expiry messages
        '                                        | and to reset ConfigUpdateRequired flag after reloading config settings
        '                                        | plus use of Source Code constants
        ' 18/10/10 | TJS             | 2010.1.06 | Corrected File_Import and Prospect_Import constant name
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to include relevant Activation code in code expiry messages
        '                                        | and to cater for IS eCommerce Plus version
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to set LastTimerTick so config form can check if service is installed and running
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to sign in and detect licence errors
        ' 10/06/12 | TJS             | 2012.1.05 | Corrected detection of midnight for licence checks
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector
        ' 23/07/13 | TJS             | 2013.1.31 | Added log message on the hour as check on system operation
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to always send usage data, not just on eCommercePlus and 
        '                                        | abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Added Daily Tasks
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim dteNow As Date, ActiveSource As SourceSettings, sTemp As String ' TJS 29/04/09
        Dim bActionOccured As Boolean, iTimerInterval As Integer ' TJs 13/04/10
        Dim iMaxValue As Integer, iMinValue As Integer, bAutoRenewActive As Boolean ' TJS 18/03/11

        Try ' TJS 02/12/11
            Interprise.Facade.Base.SimpleFacade.Instance.SignIn() ' TJS 10/03/12
            m_ImportExportConfigFacade.ExecuteNonQuery(System.Data.CommandType.Text, "UPDATE LerrynImportExportServiceAction_DEV000221 SET LastTimerTick_DEV000221 = getdate()", Nothing) ' TJS 02/12/11
        Catch ex As Exception
            If InStr(ex.Message, "The maximum connection to your company database has been reached") > 0 Then ' TJS 10/03/12
                m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ex.Message.Replace(vbCr, " ").Replace(vbLf, " ")) ' TJS 10/03/12
                Return False ' TJS 10/03/12
            Else
                m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ex) ' TJS 02/12/11
            End If
        End Try

        dteNow = Date.Now
        ' was last timer tick in in last minute of the hour and currene one in first minute ?
        If dteLastTimerTick.Minute = 59 And dteNow.Minute = 0 Then ' TJS 23/07/13
            ' yes - log to show still alive
            m_ImportExportConfigFacade.WriteLogProgressRecord("Start of the hour - dtShutDownTime = " & dtShutDownTime.ToShortDateString & " " & dtShutDownTime.ToShortTimeString & ", dtStartUpTime = " & dtStartUpTime.ToShortDateString & " " & dtStartUpTime.ToShortTimeString) ' TJS 23/07/13
        End If
        If dteNow > dtShutDownTime And dteNow < dtStartUpTime Then
            'Do Nothing, Im sleeping
            If bShutdownTime = False Then
                m_ImportExportConfigFacade.WriteLogProgressRecord("Shutdown time reached, Service will cease operation and restart at " & dtStartUpTime) ' TJS 28/01/09 TJS 22/02/09
                bShutdownTime = True
            End If
        Else
            If bShutdownTime = True Then
                'Start the service back up
                m_ImportExportConfigFacade.WriteLogProgressRecord("Shutdown period finished, Service starting.") ' TJS 28/01/09 TJS 22/02/09
                ' set initial action times as 1 minute from now instead of usual 1/5 minute interval
                dtStatusUpdateDueAt = Date.Now.AddMinutes(1)
                If PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 02/06/09
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Next Status Update run due at " & dtStatusUpdateDueAt) ' TJS 02/02/09 TJS 22/02/09
                End If
                dtInventoryUpdateDueAt = Date.Now.AddMinutes(1)
                If PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 02/06/09
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Next Inventory Update run due at " & dtInventoryUpdateDueAt) ' TJS 02/02/09 TJS 22/02/09
                End If
                dtFileCreationDueAt = Date.Now.AddMinutes(1)
                If bFileCreationRequired And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 22/02/09 TJS 02/06/09
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Next File Creation run due at " & dtFileCreationDueAt) ' TJS 28/01/09 TJS 22/02/09
                End If
                dtFileReadDueAt = Date.Now.AddMinutes(1)
                If bFileImportRequired Then ' TJS 22/02/09
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Next File Import run due at " & dtFileReadDueAt) ' TJS 28/01/09 TJS 22/02/09
                End If
                dtFTPDueAt = Date.Now.AddMinutes(1)
                If bFTPRequired And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 22/02/09 TJS 02/06/09
                    m_ImportExportConfigFacade.WriteLogProgressRecord("Next FTP run due at " & dtFTPDueAt) ' TJS 28/01/09 TJS 22/02/09
                End If
                dtShutDownTime = dtShutDownTime.AddDays(1) ' TJS 23/07/13
                dtStartUpTime = dtStartUpTime.AddDays(1) ' TJS 23/07/13
                bShutdownTime = False
            Else

                Try
                    sTemp = m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "TimerInterval", "5") ' TJS 13/04/10
                    If IsNumeric(sTemp) Then ' TJS 13/04/10
                        iTimerInterval = CInt(sTemp) ' TJS 13/04/10
                    Else
                        iTimerInterval = 5 ' TJS 13/04/10
                    End If

                    ' is Update Config flag set in DB ?
                    If CBool(m_ImportExportConfigFacade.GetField("ConfigUpdateRequired_DEV000221", "LerrynImportExportServiceAction_DEV000221")) Then ' TJS 29/04/09
                        ' yes, 
                        m_ImportExportConfigFacade.WriteLogProgressRecord("Config change detected - reloading service parameters.") ' TJS 29/04/09
                        ' reset existing settings values
                        For Each ActiveSource In Setts.ActiveSources ' TJS 29/04/09
                            ' save Source Key 
                            sTemp = ActiveSource.SourceCode ' TJS 29/04/09
                            ' clear values
                            ActiveSource.ClearSettings() ' TJS 29/04/09
                            ' and remove from collection
                            Setts.ActiveSources.Remove(sTemp) ' TJS 29/04/09
                        Next
                        ' and get updated values
                        If GetSettings() Then ' TJS 29/04/09
                            m_ImportExportConfigFacade.WriteLogProgressRecord("Finished fetching updated parameters from System DB.") ' TJS 29/04/09
                            bConfigOK = True ' TJS 20/05/09
                            m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET ConfigUpdateRequired_DEV000221 = 0", Nothing) ' TJS 19/08/10
                        Else
                            m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", "There was a problem retreiving all updated parameter information, there may be an error in your config settings.") ' TJS 29/04/09
                            bConfigOK = False ' TJS 20/05/09
                            Return False
                        End If

                    ElseIf bConfigOK And m_ImportExportConfigFacade.IsActivated Then ' TJS 20/05/09 TJS 13/04/10
                        bActionOccured = False ' TJS 27/11/09
                        ' Check for any Status updates needing to be processed
                        If Date.Now > dtStatusUpdateDueAt And bIsActivated And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE And Not bShutDownInProgress Then ' TJS 02/02/09 TJS 02/06/09 TJS 02/08/13
                            ' Start Status Update Routine
                            If UpdateStatus() Then ' TJS 02/02/09
                                'OK - Status updates are checked every minute
                                dtStatusUpdateDueAt = Date.Now.AddMinutes(1) ' TJS 02/02/09
                                bActionOccured = True ' TJS 27/11/09
                            End If
                        End If

                        ' Check for any Inventory updates needing to be processed
                        If Date.Now > dtInventoryUpdateDueAt And bIsActivated And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE And Not bShutDownInProgress Then ' TJS 02/02/09 TJS 02/06/09 TJS 02/08/13
                            'Start Inventory Update Routine
                            If UpdateInventory() Then ' TJS 02/02/09
                                'OK - Inventory updates are checked every minute
                                dtInventoryUpdateDueAt = Date.Now.AddMinutes(1) ' TJS 02/02/09
                                bActionOccured = True ' TJS 27/11/09
                            End If
                        End If

                        ' Check if file creation needs to run
                        If Date.Now > dtFileCreationDueAt And bIsActivated And bFileCreationRequired And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE And Not bShutDownInProgress Then ' TJS 02/08/13
                            'Start File Creation Routine
                            If CreateFiles() Then
                                'OK
                                dtFileCreationDueAt = Date.Now.AddMinutes(iTimerInterval) ' TJS 13/04/10
                                m_ImportExportConfigFacade.WriteLogProgressRecord("Next File Creation run due at " & dtFileCreationDueAt) ' TJS 28/01/09 TJS 22/02/09
                                bActionOccured = True ' TJS 27/11/09
                            End If
                        End If

                        ' Check if FTP Process needs to run
                        If Date.Now > dtFTPDueAt And bIsActivated And bFTPRequired And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE And Not bShutDownInProgress Then ' TJS 02/02/09 TJS 02/06/09 TJS 02/08/13
                            'Start the FTP Routine
                            If DoFTP() Then
                                'OK
                                dtFTPDueAt = Date.Now.AddMinutes(iTimerInterval) ' TJS 13/04/10
                                m_ImportExportConfigFacade.WriteLogProgressRecord("Next FTP run due at " & dtFTPDueAt) ' TJS 28/01/09 TJS 22/02/09
                                bActionOccured = True ' TJS 27/11/09
                            End If
                        End If

                        'Check if incoming file processing needs to run
                        If Date.Now > dtFileReadDueAt And bIsActivated And bFileImportRequired And Not bShutDownInProgress Then ' TJS 22/02/09 TJS 10/03/09 TJS 02/08/13
                            'Start the File Input Routine
                            If PollForFileInput() Then
                                'OK
                                dtFileReadDueAt = Date.Now.AddMinutes(iTimerInterval) ' TJS 13/04/10
                                m_ImportExportConfigFacade.WriteLogProgressRecord("Next File Import run due at " & dtFileReadDueAt) ' TJS 28/01/09 TJS 22/02/09
                                bActionOccured = True ' TJS 27/11/09
                            End If

                        End If

                        ' check for web poll on each timer tick as each site will have different poll interval
                        If bWebIORequired And PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE And Not bShutDownInProgress Then ' TJS 07/07/09 TJS 02/08/13
                            If PollForWebIO() Then ' TJS 07/07/09 TJS 27/11/09
                                bActionOccured = True ' TJS 27/11/09
                            End If
                        End If

                        If bActionOccured Then ' TJS 27/11/09
                            bFirstPoll = False ' TJS 14/11/09
                        End If

                        '' start of code added TJS 18/03/11
                        'If Date.Now > dtNotifyUsageTime And Not bShutDownInProgress Then ' TJS 02/08/13 TJS 02/08/13
                        '    ' send usage infomation for transaction charges
                        '    If NotifyUsage() Then
                        '        ' usage info sent successfully, generate a random time between 6 am and 10 pm TOMORROW
                        '        Randomize(CDbl(Right(CStr(Date.Now().ToOADate), 4)))
                        '        iMaxValue = 22 * 60
                        '        iMinValue = 6 * 60
                        '        dtNotifyUsageTime = Date.Today.AddDays(1).AddMinutes(Int((iMaxValue - iMinValue + 1) * Rnd() + iMinValue))
                        '    Else
                        '        ' usage info couldn't be sent, set time to retry in 1 hour
                        '        dtNotifyUsageTime = dtNotifyUsageTime.AddHours(1)
                        '    End If
                        'End If
                        '' end of code added TJS 18/03/11

                    End If

                    ' start of code added TJS 13/04/10
                    ' has midnight just passed (last timer tick was yesterday) ?
                    If dteLastTimerTick.Date <= dteNow.Date.AddDays(-1) And Not bShutDownInProgress Then ' TJS 10/06/12 TJS 02/08/13
                        ' yes, re-check activation
                        bAutoRenewActive = False 'CheckForAutoRenew() ' TJS 18/03/11  
                        m_ImportExportConfigFacade.ReCheckActivation()
                        If Not m_ImportExportConfigFacade.IsActivated Then
                            m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", PRODUCT_NAME & " Base Module Activation Code (" & m_ImportExportConfigFacade.LatestActivationCode & ") has expired") ' TJS 18/03/11

                        ElseIf m_ImportExportConfigFacade.ActivationExpires = dteNow.Date.AddDays(28) Or _
                            m_ImportExportConfigFacade.ActivationExpires = dteNow.Date.AddDays(14) Or _
                            m_ImportExportConfigFacade.ActivationExpires <= dteNow.Date.AddDays(7) And Not bAutoRenewActive Then ' TJS 18/03/11
                            m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", PRODUCT_NAME & " Base Module Activation Code (" & m_ImportExportConfigFacade.LatestActivationCode & ") expires on " & m_ImportExportConfigFacade.ActivationExpires) ' TJS 18/03/11
                        End If
                        For Each ActiveSource In Setts.ActiveSources
                            Select Case ActiveSource.SourceCode
                                Case GENERIC_IMPORT_SOURCE_CODE ' TJS 19/08/10
                                    ' already checked

                                Case FILE_IMPORT_SOURCE_CODE ' TJS 19/08/10 TJS/FA 18/10/10
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If

                                Case PROSPECT_LEAD_IMPORT_SOURCE_CODE ' TJS 19/08/10 TJS/FA 18/10/10
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(PROSPECT_IMPORT_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(PROSPECT_IMPORT_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(PROSPECT_IMPORT_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(PROSPECT_IMPORT_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(PROSPECT_IMPORT_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(PROSPECT_IMPORT_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If

                                Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(SHOP_DOT_COM_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(SHOP_DOT_COM_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(SHOP_DOT_COM_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(SHOP_DOT_COM_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(SHOP_DOT_COM_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(SHOP_DOT_COM_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If

                                Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If

                                Case VOLUSION_SOURCE_CODE ' TJS 19/08/10
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(VOLUSION_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(VOLUSION_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(VOLUSION_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(VOLUSION_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(VOLUSION_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(VOLUSION_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If

                                Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 19/08/10
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(CHANNEL_ADVISOR_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(CHANNEL_ADVISOR_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(CHANNEL_ADVISOR_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(CHANNEL_ADVISOR_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(CHANNEL_ADVISOR_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If

                                    ' start of code added TJS 19/08/10
                                Case MAGENTO_SOURCE_CODE
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(MAGENTO_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(MAGENTO_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(MAGENTO_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(MAGENTO_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(MAGENTO_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(MAGENTO_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If

                                Case ASP_STORE_FRONT_SOURCE_CODE
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(ASP_STORE_FRONT_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(ASP_STORE_FRONT_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then ' TJS 10/06/12
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE) & ") has expired") ' TJS 18/03/11

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(ASP_STORE_FRONT_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(ASP_STORE_FRONT_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(ASP_STORE_FRONT_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then ' TJS 18/03/11
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(ASP_STORE_FRONT_CONNECTOR_CODE)) ' TJS 18/03/11
                                    End If
                                    ' end of code added TJS 19/08/10

                                    ' start of code added TJS 05/07/12
                                Case AMAZON_FBA_SOURCE_CODE
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(AMAZON_FBA_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_FBA_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE) & ") has expired")

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_FBA_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_FBA_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_FBA_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(AMAZON_FBA_CONNECTOR_CODE))
                                    End If
                                    ' end of code added TJS 05/07/12

                                    ' start of code added TJS 20/11/13
                                Case THREE_D_CART_SOURCE_CODE
                                    If Not m_ImportExportConfigFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) And m_ImportExportConfigFacade.HasConnectorBeenActivated(THREE_D_CART_CONNECTOR_CODE) AndAlso _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(THREE_D_CART_CONNECTOR_CODE) > dteNow.Date.AddDays(-2) Then
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & " Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE) & ") has expired")

                                    ElseIf m_ImportExportConfigFacade.ConnectorActivationExpires(THREE_D_CART_CONNECTOR_CODE) = dteNow.Date.AddDays(28) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(THREE_D_CART_CONNECTOR_CODE) = dteNow.Date.AddDays(14) Or _
                                        m_ImportExportConfigFacade.ConnectorActivationExpires(THREE_D_CART_CONNECTOR_CODE) <= dteNow.Date.AddDays(7) And _
                                        Not bAutoRenewActive Then
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", ActiveSource.SourceName & "  Activation Code (" & m_ImportExportConfigFacade.ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE) & ") expires on " & m_ImportExportConfigFacade.ConnectorActivationExpires(THREE_D_CART_CONNECTOR_CODE))
                                    End If
                                    ' end of code added TJS 20/11/13
                            End Select
                            If bShutDownInProgress Then ' TJS 02/08/13
                                Exit For ' TJS 02/08/13
                            End If
                        Next
                        DoSourceSpecificDailyFunctions() ' TJS 13/11/13
                    End If

                    dteLastTimerTick = dteNow
                    ' end of code added TJS 13/04/10
                    Return True

                Catch Ex As Exception
                    m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "RunRoutines", Ex) ' TJS 28/01/09
                    Return False
                End Try
            End If
        End If

        Return True
    End Function


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        Thread.Sleep(20)
        InitializeComponent()

        ' www.dynenttech.com davidonelson 5/4/2018
        ' Default was SSL3 or TLS, we need to allow TLS11 and TLS12 also, some sites are going to TLS12 only these days
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Ssl3 Or Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12


    End Sub

End Class
