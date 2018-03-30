' eShopCONNECT for Connected Business - Windows Service
' Module: ErrorNotification.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
'
'       © 2012 - 2014  Lerryn Business Solutions Ltd
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
' Last Updated - 01 May 2014

Option Explicit On
Option Strict On

Imports System.Diagnostics
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Web
Imports System.Xml.Linq
Imports Microsoft.VisualBasic
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Interprise.Framework.Base.Shared.Const
Imports Microsoft.Win32
Imports Interprise.Framework.Base.Shared.Enum
Imports Interprise
Imports System.Collections.Generic

Public Class ErrorNotification

#Region " Variables "
    Private m_BaseErrorEmailAddress As String = ""
    Private m_BaseSendCodeErrorEmailsToLerryn As Boolean = True
    Private m_BaseSendSourceErrorEmailsToLerryn As Boolean = False
    Private m_LogFilePath As String = ""
    Private m_BaseProductName As String = ""
    Private m_BaseProductCode As String = ""
    Private m_ActivationCode As String = ""
    Private m_EnableLogFile As Boolean = False
    Private m_LastCodeErrorTime As Date = DateSerial(2000, 1, 1)
    Private m_ErrorConditionList As New List(Of ErrorConditionList)

#End Region

#Region " Properties "
#Region " BaseErrorEmailAddress "
    Public Property BaseErrorEmailAddress() As String
        Set(ByVal value As String)
            m_BaseErrorEmailAddress = value
        End Set
        Get
            Return m_BaseErrorEmailAddress
        End Get
    End Property
#End Region

#Region " BaseSendCodeErrorEmailsToLerryn "
    Public Property BaseSendCodeErrorEmailsToLerryn() As Boolean
        Set(ByVal value As Boolean)
            m_BaseSendCodeErrorEmailsToLerryn = value
        End Set
        Get
            Return m_BaseSendCodeErrorEmailsToLerryn
        End Get
    End Property
#End Region

#Region " BaseSendSourceErrorEmailsToLerryn "
    Public Property BaseSendSourceErrorEmailsToLerryn() As Boolean
        Set(ByVal value As Boolean)
            m_BaseSendSourceErrorEmailsToLerryn = value
        End Set
        Get
            Return m_BaseSendSourceErrorEmailsToLerryn
        End Get
    End Property
#End Region

#Region " LogFilePath "
    Public Property LogFilePath() As String
        Set(ByVal value As String)
            m_LogFilePath = value
        End Set
        Get
            Return m_LogFilePath
        End Get
    End Property
#End Region

#Region " EnableLogFile "
    Public Property EnableLogFile() As Boolean
        Get
            Return m_EnableLogFile
        End Get
        Set(ByVal value As Boolean)
            m_EnableLogFile = value
        End Set
    End Property
#End Region

#Region " BaseProductName "
    Public Property BaseProductName() As String
        Set(ByVal value As String)
            m_BaseProductName = value
        End Set
        Get
            Return m_BaseProductName
        End Get
    End Property
#End Region

#Region " BaseProductCode "
    Public Property BaseProductCode() As String
        Set(ByVal value As String)
            m_BaseProductCode = value
        End Set
        Get
            Return m_BaseProductCode
        End Get
    End Property
#End Region

#Region " ActivationCode "
    Public Property ActivationCode() As String
        Set(ByVal value As String)
            m_ActivationCode = value
        End Set
        Get
            Return m_ActivationCode
        End Get
    End Property
#End Region
#End Region

#Region " Methods "

#Region " SendExceptionEmail "
    Public Sub SendExceptionEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, _
        ByVal ProcedureException As Exception, Optional ByVal XMLSource As String = "")

        '! Variables
        Dim emailBody As String = String.Empty
        Dim emailMHT As String                                              'Email full Notification
        Dim emailSubject As String                                          'Email Subject
        Dim emailErrorDetails As String                                     'Email Error Details
        Dim emailErrorMessage As String                                     'Error Message
        Dim strErrorMsgCore As String                                       'Error Message
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"    'Email toAddress
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"    'Email ccAddress

        '! Start of Code for SendExceptionEmail
        Try
            '! Determine if there's an XML Source
            If XMLSource <> "" Then
                If ProcedureException.Message.IndexOf("InnerException") > 1 Then
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & ProcedureException.InnerException.Message & vbCrLf & ProcedureException.InnerException.StackTrace & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, "")) ' TJS 01/06/10
                Else
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
                End If
            Else
                If ProcedureException.Message.IndexOf("InnerException") > 1 Then
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & ProcedureException.InnerException.Message & vbCrLf & ProcedureException.InnerException.StackTrace) ' TJS 01/06/10
                Else
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace)
                End If
            End If

            '! Determine if the error message is repeated or one hour has passed
            strErrorMsgCore = "Error in " & ProcedureName & vbCrLf & ProcedureException.Message
            If SendingErrorCondition(strErrorMsgCore) Or ForceSendEmail() Then
                Try
                    If SourceConfig IsNot Nothing Then
                        If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Or Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Or _
                            GetElementText(SourceConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then

                            toAddress = Me.GetToAddress(SourceConfig)
                            emailSubject = "Connected Business eShopCONNECTED Error - " & ProcedureName
                            emailErrorMessage = "Error in " & ProcedureName
                            emailErrorDetails = ProcedureException.Message & "<br>" & ProcedureException.StackTrace

                            '! Procedure in getting the ErrorDetails
                            If ProcedureException.Message.ToLower.IndexOf("innerexception") > 1 Or _
                                ProcedureException.Message.ToLower.IndexOf("inner exception") > 1 Then
                                emailErrorDetails = emailErrorDetails & vbCrLf & "Inner Exception - " & ProcedureException.InnerException.Message
                                emailErrorDetails = emailErrorDetails & vbCrLf & ProcedureException.InnerException.StackTrace
                            End If
                            If XMLSource <> "" Then
                                emailErrorDetails = emailErrorDetails & vbCrLf & vbCrLf & XMLSource
                            End If

                            emailBody = Me.CreateEmailBody(toAddress, emailErrorMessage, emailErrorDetails) ' Get Email Body
                            emailMHT = Me.ConvertHTMLToMHT(emailBody)                                       ' Convert HTML Body to MHT Email

                            '! Send Email
                            Try
                                Me.SendEmail(Me.CreateEmail(emailMHT, emailSubject, toAddress, ccAddress))
                            Catch ex As Exception
                                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_WARNING, "Unable to send error notification - " & ex.Message)
                            End Try
                        End If
                    End If

                Catch ex As Exception
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendExceptionEmail - " & ex.Message & ex.ToString)
                    If XMLSource <> "" Then
                        WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
                    Else
                        WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace)
                    End If
                End Try
            End If
        Catch Ex As Exception
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendExceptionEmail(a) - " & Ex.Message & Ex.ToString)
        End Try
        '! End of Code for SendExceptionEmail
    End Sub
#End Region

#Region " SendExceptionEmail "
    Public Sub SendExceptionEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, _
        ByVal ErrorDetails As String, Optional ByVal XMLSource As String = "")

        '! Variables
        Dim WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim emailBody As String = String.Empty
        Dim emailMHT As String                                              'Email full Notification
        Dim emailSubject As String                                          'Email Subject
        Dim emailErrorDetails As String                                     'Email Error Details
        Dim emailErrorMessage As String                                     'Error Message
        Dim strErrorMsgCore As String                                       'Error Message
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"    'Email toAddress
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"    'Email ccAddress

        '! Start of Code for SendExceptionEmail
        Try
            '! Determine if there's an XML Source
            If XMLSource <> "" Then
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ErrorDetails & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
            Else
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ErrorDetails)
            End If

            '! Determine if the error message is repeated or one hour has passed
            strErrorMsgCore = "Error in " & ProcedureName & vbCrLf & ErrorDetails
            If SendingErrorCondition(strErrorMsgCore) Or ForceSendEmail() Then
                Try
                    If SourceConfig IsNot Nothing Then
                        If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Or Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Or _
                            GetElementText(SourceConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then

                            toAddress = Me.GetToAddress(SourceConfig)
                            emailSubject = "Connected Business eShopCONNECTED Error - " & ProcedureName                            ' Get Email Subject
                            emailErrorMessage = "Error in " & ProcedureName
                            emailErrorDetails = "<br>" & ErrorDetails

                            '! Procedure in getting the ErrorDetails
                            If XMLSource <> "" Then
                                emailErrorDetails = emailErrorDetails & vbCrLf & vbCrLf & XMLSource
                            End If

                            emailBody = Me.CreateEmailBody(toAddress, emailErrorMessage, emailErrorDetails) ' Get Email Body
                            emailMHT = Me.ConvertHTMLToMHT(emailBody)                                       ' Convert HTML Body to MHT Email

                            '! Send Email
                            Try
                                Me.SendEmail(Me.CreateEmail(emailMHT, emailSubject, toAddress, ccAddress))
                            Catch ex As Exception
                                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_WARNING, "Unable to send error notification - " & ex.Message)
                            Finally
                                If Not postStream Is Nothing Then postStream.Close()
                                If Not WebResponse Is Nothing Then WebResponse.Close()
                            End Try
                        End If
                    End If

                Catch ex As Exception
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendExceptionEmail - " & ex.Message & ex.ToString)
                    If XMLSource <> "" Then
                        WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ErrorDetails & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
                    Else
                        WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ErrorDetails)
                    End If

                End Try
            End If


        Catch Ex As Exception
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendExceptionEmail(b) - " & Ex.Message & Ex.ToString)

        End Try
        '! End of Code for SendExceptionEmail
    End Sub
#End Region

#Region " SendSrcErrorEmail "
    Public Sub SendSrcErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, _
        ByVal Message As String, Optional ByVal XMLSource As String = "")

        '! Variables
        Dim WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim emailBody As String = String.Empty
        Dim emailMHT As String                                              'Email full Notification
        Dim emailSubject As String                                          'Email Subject
        Dim emailErrorDetails As String                                     'Email Error Details
        Dim emailErrorMessage As String                                     'Error Message
        Dim strErrorMsgCore As String                                       'Error Message
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"    'Email toAddress
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"    'Email ccAddress

        '! Start of Code for SendSrcErrorEmail
        Try
            '! Determine if there's an XML Source
            If XMLSource <> "" Then
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message & ", Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
            Else
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message)
            End If

            '! Determine if the error message is repeated or one hour has passed
            strErrorMsgCore = "Source Error in " & ProcedureName & vbCrLf & Message
            If SendingErrorCondition(strErrorMsgCore) Or ForceSendEmail() Then
                Try
                    If SourceConfig IsNot Nothing Then

                        If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Or Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Or _
                            GetElementText(SourceConfig, SOURCE_CONFIG_SEND_SOURCE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then


                            toAddress = Me.GetToAddress(SourceConfig)
                            emailSubject = "Connected Business eShopCONNECTED Source Error - " & ProcedureName
                            emailErrorMessage = "Source Error in " & ProcedureName
                            emailErrorDetails = Message

                            '! Procedure in getting the ErrorDetails
                            If XMLSource <> "" Then
                                emailErrorDetails = emailErrorDetails & vbCrLf & vbCrLf & XMLSource
                            End If

                            emailBody = Me.CreateEmailBody(toAddress, emailErrorMessage, emailErrorDetails) ' Get Email Body
                            emailMHT = Me.ConvertHTMLToMHT(emailBody)                                       ' Convert HTML Body to MHT Email

                            '! Send Email
                            Try
                                Me.SendEmail(Me.CreateEmail(emailMHT, emailSubject, toAddress, ccAddress))
                            Catch ex As Exception
                                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_WARNING, "Unable to send error notification - " & ex.Message)
                            Finally
                                If Not postStream Is Nothing Then postStream.Close()
                                If Not WebResponse Is Nothing Then WebResponse.Close()
                            End Try
                        End If
                    End If

                Catch ex As Exception
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendSrcErrorEmail - " & ex.Message & ex.ToString)
                End Try
            End If

        Catch ex As Exception
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendSrcErrorEmail - " & ex.Message & ex.ToString)
            If XMLSource <> "" Then
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message & ", Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, "")) ' TJS 03/04/09
            Else
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message)
            End If
        End Try
    End Sub
#End Region

#Region " SendPmntErrorEMail "
    Public Sub SendPmntErrorEmail(ByVal SourceConfig As XDocument, ByVal Message As String)

        '! Variables
        Dim WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim emailBody As String = String.Empty
        Dim emailMHT As String                                              'Email full Notification
        Dim emailSubject As String                                          'Email Subject
        Dim emailErrorDetails As String                                     'Email Error Details
        Dim emailErrorMessage As String                                     'Error Message
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"    'Email toAddress
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"    'Email ccAddress

        '! Start of Code for SendExceptionEmail
        Try
            WriteToLogFileOrEvent("!PmtErr!", Message)

            Try
                If SourceConfig IsNot Nothing Then
                    If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Then ' TJS 10/06/12

                        toAddress = GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
                        emailSubject = "Connected Business eShopCONNECTED Error - Payment Failure"
                        emailErrorMessage = "Error in Payment Failure"
                        emailErrorDetails = Message
                        emailBody = Me.CreateEmailBody(toAddress, emailErrorMessage, emailErrorDetails) ' Get Email Body
                        emailMHT = Me.ConvertHTMLToMHT(emailBody)

                        'Send Email
                        Try
                            Me.SendEmail(Me.CreateEmail(emailMHT, emailSubject, toAddress, ccAddress))
                        Catch ex As Exception
                            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_WARNING, "Unable to send error notification - " & ex.Message)
                        Finally
                            If Not postStream Is Nothing Then postStream.Close()
                            If Not WebResponse Is Nothing Then WebResponse.Close()
                        End Try
                    End If
                End If
            Catch ex As Exception
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendPmntErrorEmail - " & ex.Message & ex.ToString)
            End Try
        Catch ex As Exception
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendPmntErrorEmail - " & ex.Message & ex.ToString)
            WriteToLogFileOrEvent("!PmtErr!", Message)
        End Try
    End Sub
#End Region

#Region " WriteToLogFileOrEvent "
    Public Sub WriteToLogFileOrEvent(ByVal MessageType As String, ByVal MessageText As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Write to the log file or the computer event log if an error and log not enabled
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 28/01/09 | TJS             | 2009.1.01 | Code replaced
        ' 10/03/09 | TJS             | 2009.1.09 | Modified to use app path if no log path found
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from ErrorReporting module
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim writer As StreamWriter, sLogFileName As String

        If EnableLogFile And LogFilePath <> "" Then

            Try
                If LogFilePath <> "" Then ' TJS 10/03/09
                    If LogFilePath.Substring(LogFilePath.Length - 1) <> "\" Then
                        LogFilePath = LogFilePath & "\"
                    End If
                Else
                    LogFilePath = System.AppDomain.CurrentDomain.BaseDirectory() ' TJS 10/03/09
                End If
                sLogFileName = LogFilePath & Format(Now.Date.ToString("yyyyMMdd") & ".log")

                'Start the logging to the file
                writer = New StreamWriter(sLogFileName, True)

                writer.WriteLine(Now.ToString("HHmmss") & "    " & MessageType & "     - " & MessageText)
                writer.Close()
                writer.Dispose()

            Catch Ex As Exception
                WriteErrorToEventLog("Unable to write to Log File for " & m_BaseProductName & vbCrLf & Ex.Message & vbCrLf & Ex.StackTrace)

            End Try

        ElseIf MessageType = LOG_MESSAGE_TYPE_ERROR Then
            WriteErrorToEventLog(MessageText)
        End If

    End Sub
#End Region

#Region " WriteInfoToEventLog "
    Public Sub WriteInfoToEventLog(ByVal MessageText As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Write information to the computer application event log 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim objEventLog As New EventLog()

        Dim EventLog As New System.Diagnostics.EventLog()

        If Not EventLog.SourceExists(m_BaseProductName) Then
            EventLog.CreateEventSource(m_BaseProductName, "Application")
        End If
        EventLog.Source = m_BaseProductName
        EventLog.WriteEntry(MessageText, System.Diagnostics.EventLogEntryType.Information)

    End Sub

#End Region

#Region " WriteErrorToEventLog "
    Public Sub WriteErrorToEventLog(ByVal MessageText As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Write error to the computer application event log 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 28/01/09 | TJS             | 2009.1.01 | Function added
        ' 22/02/09 | TJS             | 2009.1.08 | Modified to set log type to error
        ' 02/04/12 | TJS             | 2011.2.12 | Modified to trap errors writing to event log
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from ErrorREporting module
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim EventLog As New System.Diagnostics.EventLog()

        Try ' TJS 02/04/12
            If Not System.Diagnostics.EventLog.SourceExists(m_BaseProductName) Then
                System.Diagnostics.EventLog.CreateEventSource(m_BaseProductName, "Application")
            End If
            EventLog.Source = m_BaseProductName
            EventLog.WriteEntry(MessageText, System.Diagnostics.EventLogEntryType.Error) 'TJS 22/02/09

        Catch ex As Exception ' TJS 02/04/12
            ' Can't do much with this error if we can't write to the event log
        End Try

    End Sub
#End Region

#Region " ForceSendEmail "
    Private Function ForceSendEmail() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Write to the computer event log 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/02/09 | TJS             | 2009.1.07 | Function added
        ' 03/04/09 | TJS             | 2009.2.00 | Modified to prevent Registry read permissions problems causing process failure
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to open registry key as read only
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from ErrorReporting module
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rgISPluginSettings As RegistryKey, strKeyNames As String(), strKeyName As String

        ' check registry for force send email (overrides normal inhibition of email sending on Lerryn computers during testing)
        ForceSendEmail = False
        Try

            rgISPluginSettings = Registry.LocalMachine.OpenSubKey(REGISTRY_KEY_ROOT, False) ' TJS 23/08/09
            If rgISPluginSettings IsNot Nothing Then
                strKeyNames = rgISPluginSettings.GetValueNames
                For Each strKeyName In strKeyNames
                    If strKeyName = "ForceSendEmail" Then
                        If rgISPluginSettings.GetValue("ForceSendEmail").ToString.ToUpper = "YES" Then
                            ForceSendEmail = True
                        End If
                    End If
                Next
                rgISPluginSettings.Close()
            End If

        Catch ex As Exception
            ' ignore errors as they could be permissions problems reading the Registry
        End Try

    End Function
#End Region

#Region " SendingErrorCondition "
    Private Function SendingErrorCondition(strErrorMsgCore As String) As Boolean
        Dim bSendErrorEmail As Boolean

        Dim indexError As Integer = -1
        If m_LastCodeErrorTime <> DateSerial(2000, 1, 1) Then
            indexError = m_ErrorConditionList.IndexOf(m_ErrorConditionList.Find(Function(matchA) matchA.ErrorMessage = strErrorMsgCore))

            If indexError > -1 Then
                If m_ErrorConditionList(indexError).ErrorDate < DateAdd(DateInterval.Hour, -1, Date.Now) Then
                    bSendErrorEmail = True
                    m_ErrorConditionList(indexError).ErrorDate = Date.Now
                    m_LastCodeErrorTime = Date.Now
                Else
                    bSendErrorEmail = False
                End If
            Else
                bSendErrorEmail = True
                m_ErrorConditionList.Add(New ErrorConditionList(strErrorMsgCore, Date.Now))
                m_LastCodeErrorTime = Date.Now
            End If
        Else
            bSendErrorEmail = True
            m_ErrorConditionList.Add(New ErrorConditionList(strErrorMsgCore, Date.Now))
            m_LastCodeErrorTime = Date.Now
        End If

        Return bSendErrorEmail
    End Function
#End Region

#Region " GetToAddress "
    Private Function GetToAddress(SourceConfig As XDocument) As String
        Dim toAddress As String = String.Empty

        '! Procedure in getting toAddress
        If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Then
            toAddress = GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
        ElseIf Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Then
            toAddress = ConvertEntitiesForXML(m_BaseErrorEmailAddress)
        End If

        Return toAddress
    End Function
#End Region

#Region " CreateEmailBody "
    Protected Function CreateEmailBody(toAddress As String, errorMessage As String, errorDetails As String) As String

        '! Create Email Body Header
        Dim body As String
        body = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">" _
                & "<html xmlns=""http://www.w3.org/1999/xhtml"">" _
                & "<head> <meta name=""generator"" content=""HTML Tidy for Linux/x86 (vers 1st November 2002), see www.w3.org"" />" _
                & "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />" _
                & "<title> eShopCONNECT Error </title> </head>" _
                & "<body>" _
                & "Hi <i>" & toAddress & "</i>, <br><br> An error occurred in Connected Business eShopCONNECTED windows service. Please review the error details below to identify the cause of the problem and kindly take necessary action. <br><br>" _
                & "<b>Error:</b> " & errorMessage & "<br>" _
                & "<b>Details:</b> " & errorDetails & " <br><br>" _
                & "<b> Version:</b> " & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion & "<br><br>" _
                & "<b> Host Version:</b> " & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location.Replace("Lerryn.Facade.ImportExport.dll", "Interprise.Facade.Base.dll")).ProductVersion & "<br><br>" _
                & "Please contact <a href='http://support.connectedbusiness.com/'>Connected Business Support Center</a> for any further assistance <br><br>" _
                & "Thank you, <br><br>" _
                & "Connected Business eShopCONNECTED Team" _
                & "</body></html>"
        Return body ' returns header content
    End Function
#End Region

#Region " ConvertHTMLToMHT "
    Protected Overridable Function ConvertHTMLToMHT(ByVal emailHTML As String) As String
        '! Convert HTML to MHT for email

        Dim mhtBuilder As Chilkat.Mht = DirectCast(Interprise.Licensing.Base.ComponentFactory.Instance.GetLicensedComponent(Interprise.Licensing.Base.Shared.Enum.LicensedComponent.ChilkatMHT), Chilkat.Mht)
        Try
            Return mhtBuilder.HtmlToMHT(emailHTML)
        Finally
            mhtBuilder.Dispose()
        End Try
    End Function
#End Region

#Region " CreateEmail "
    Protected Function CreateEmail(htmlMessage As String, subject As String, toaddress As String, ccAddress As String) As String

        '! Create Email details in MHT
        Dim emailMHT As String
        Using emailMessage As New Interprise.Licensing.Base.Connect.EMailMessage
            emailMessage.SetFromMimeText(htmlMessage)
            emailMessage.Subject = subject
            emailMessage.ToAddress = toaddress
            emailMessage.BccAddress = ccAddress
            emailMHT = emailMessage.GetMime
        End Using
        Return emailMHT
    End Function
#End Region

#Region " SendEmail "
    Protected Sub SendEmail(ByVal email As String)
        '! Send the Email using CRM EmailMessage
        Dim emailsSent As Boolean
        Using facadeEmail As Interprise.Extendable.Base.Facade.CRM.Connect.IEmailMessageInterface = New Interprise.Facade.Base.CRM.Connect.EMailMessageFacade
            Dim emailAccountCode As String = facadeEmail.GetEmailAccountToUse(EmailAccountType.POP3) 'Facade.Base.SimpleFacade.Instance.GetUserDefaultEmailAccount
            Dim messageCode As String() = {}
            facadeEmail.SaveToFolder(emailAccountCode, OUTBOX_FOLDER, EMailStatus.ReadyToSend, messageCode, email)

            Using emailObject As New Interprise.Licensing.Base.Connect.EMailMessage
                emailObject.SetFromMimeText(email)
                If emailObject IsNot Nothing AndAlso messageCode IsNot Nothing AndAlso messageCode.Length > 0 Then
                    emailObject.AddHeaderField(CONNECTMESSAGEHEADER_MESSAGECODE_COLUMN, messageCode(0))
                    emailsSent = facadeEmail.SendEmail(emailAccountCode, emailObject.GetMime) 'Send email through CRM
                End If
            End Using
        End Using
    End Sub
#End Region

#End Region

End Class
