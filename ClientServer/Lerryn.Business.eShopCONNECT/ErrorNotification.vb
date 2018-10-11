' eShopCONNECT for Connected Business - Windows Service
' Module: ErrorNotification.vb
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
    Private m_LastCodeErrorMessage As String = ""
    Private m_LastCodeErrorTime As Date = DateSerial(2000, 1, 1)
    Private m_LastSourceErrorMessage As String = ""
    Private m_LastSourceErrorTime As Date = DateSerial(2000, 1, 1)
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
    Public Sub SendExceptionEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, _
        ByVal ProcedureException As Exception, Optional ByVal XMLSource As String = "") ' TJS 07/01/11 TJS 02/12/11 TJS 10/06/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Notifies key parties of code errors
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 28/01/09 | TJS             | 2009.1.01 | Corrected email address logic and added BaseProductName parameter
        ' 11/02/09 | TJS             | 2009.1.07 | Modified to enable forcing of emails on Lerryn computers
        ' 22/02/09 | TJS             | 2009.1.08 | Corrected addition of TimSheppard@lerryn.com address
        ' 09/03/09 | TJS             | 2009.1.09 | Modified to trap error if CDO not installed
        ' 03/04/09 | TJS             | 2009.2.00 | Modified to include CDO.Configuration within CDO error trap
        ' 04/10/10 | TJS             | 2010.1.05 | Modifed to include error detail when unable to send email
        ' 12/12/10 | TJS             | 2010.1.12 | Code re-written to use System.Net.Mail instead of CDO and modified 
        '                                        | to only send email if error different from last one or 1 hour has passed
        ' 14/12/10 | FA              | 2010.1.14 | added ObjMail delivery method other wise 'Mailbox unavailable, could not relay message'
        ' 07/01/11 | TJS             | 2010.1.15 | Code re-written to use XML Web Service at LerrynSecure.com to send 
        '                                        | emails as Windows 7 workstation excludes SMTP Mail from IIS
        ' 18/03/11 | TJS             | 2011.0.01 | Corrected entity conversion for XML and calculation message length security value
        ' 04/05/11 | TJS             | 2011.0.13 | Modified procedure name in error handler
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to use core or error message only when detecting repeat error messages
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from ErrorReporting module and modified to use module variable for BaseProductName, BaseProductCode and ActivationCode
        ' 28/07/13 | TJS             | 2013.1.31 | Modified to include product and host version details
        ' 04/01/14 | TJS             | 2013.4.03 | Corrected getting host version details
        ' 19/03/14 | TJS             | 2014.0.00 | Modified to use SystemError@lerryn.com email address
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected machine names in checks on sending emails
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, strReturn As String ' TJS 02/12/11
        Dim strNotificationXML As String, strSubject As String, strContent As String, strAuth As String ' TJS 07/01/11
        Dim strTemp As String, strErrorMsgCore As String, bSendErrorEmail As Boolean ' TJS 12/12/10 TJS 07/01/11 TJS 24/02/12
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"

        Try
            If XMLSource <> "" Then
                If ProcedureException.Message.IndexOf("InnerException") > 1 Then ' TJS 01/06/10
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & ProcedureException.InnerException.Message & vbCrLf & ProcedureException.InnerException.StackTrace & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, "")) ' TJS 01/06/10
                Else
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
                End If
            Else
                If ProcedureException.Message.IndexOf("InnerException") > 1 Then ' TJS 01/06/10
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & ProcedureException.InnerException.Message & vbCrLf & ProcedureException.InnerException.StackTrace) ' TJS 01/06/10
                Else
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace)
                End If
            End If

            ' start of code replaced TJS 12/12/10
            Try

                If SourceConfig IsNot Nothing Then ' TJS 24/11/10
                    ' start of code replaced TJS 07/01/11
                    strNotificationXML = "<LerrynNotifyError>" & vbCrLf & "<Function>Code Exception</Function>" & vbCrLf
                    strNotificationXML = strNotificationXML & "<Product>" & ConvertEntitiesForXML(m_BaseProductCode) & "</Product>" & vbCrLf ' TJS 18/03/11
                    strNotificationXML = strNotificationXML & "<Activation>" & ConvertEntitiesForXML(m_ActivationCode) & "</Activation>" & vbCrLf ' TJS 18/03/11
                    strNotificationXML = strNotificationXML & "<Version>" & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion & "</Version>" ' TJS 28/07/13
                    strNotificationXML = strNotificationXML & "<HostVersion>" & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location.Replace("Lerryn.Business.ImportExport.dll", "Interprise.Facade.Base.dll")).ProductVersion & "</HostVersion>" ' TJS 28/07/13 TJS 04/01/14
                    strNotificationXML = strNotificationXML & "<Message>" & vbCrLf
                    ' has local notification email address been set or notifications to Lerryn not disabled ?
                    If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Or Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Or _
                        GetElementText(SourceConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then ' TJS 10/06/12
                        If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Then ' TJS 10/06/12
                            toAddress = GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
                            strNotificationXML = strNotificationXML & "<MailTo>" & ConvertEntitiesForXML(toAddress) ' TJS 18/03/11
                            strNotificationXML = strNotificationXML & "</MailTo>" & vbCrLf & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf
                            If GetElementText(SourceConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then
                                strNotificationXML = strNotificationXML & "<MailCC>eshopconnected@connectedbusiness.com</MailCC>" & vbCrLf ' TJS 19/03/14
                            End If

                        ElseIf Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Then ' TJS 10/06/12
                            toAddress = ConvertEntitiesForXML(m_BaseErrorEmailAddress)
                            strNotificationXML = strNotificationXML & "<MailTo>" & toAddress  ' TJS 10/06/12
                            strNotificationXML = strNotificationXML & "</MailTo>" & vbCrLf & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf ' TJS 10/06/12
                            If m_BaseSendCodeErrorEmailsToLerryn Then ' TJS 10/06/12
                                strNotificationXML = strNotificationXML & "<MailCC>eshopconnected@connectedbusiness.com</MailCC>" & vbCrLf ' TJS 10/06/12 TJS 19/03/14
                            End If

                        Else
                            strNotificationXML = strNotificationXML & "<MailTo>eshopconnected@connectedbusiness.com</MailTo>" & vbCrLf ' TJS 19/03/14
                            strNotificationXML = strNotificationXML & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf
                        End If
                        strSubject = m_BaseProductName & " Error - " & ProcedureName
                        strNotificationXML = strNotificationXML & "<Subject>" & ConvertEntitiesForXML(strSubject) & "</Subject>" & vbCrLf ' TJS 18/03/11
                        strErrorMsgCore = "Error in " & ProcedureName & vbCrLf & ProcedureException.Message ' TJS 24/02/12
                        strContent = strErrorMsgCore & vbCrLf & ProcedureException.StackTrace ' TJS 24/02/12
                        If ProcedureException.Message.ToLower.IndexOf("innerexception") > 1 Or _
                            ProcedureException.Message.ToLower.IndexOf("inner exception") > 1 Then ' TJS 01/06/10 TJS 09/06/10
                            strContent = strContent & vbCrLf & "Inner Exception - " & ProcedureException.InnerException.Message ' TJS 01/06/10
                            strContent = strContent & vbCrLf & ProcedureException.InnerException.StackTrace ' TJS 01/06/10
                        End If
                        If XMLSource <> "" Then
                            strContent = strContent & vbCrLf & vbCrLf & XMLSource
                        End If
                        strNotificationXML = strNotificationXML & "<Content><![CDATA[" & strContent & "]]></Content>" & vbCrLf ' TJS 18/03/11
                        strTemp = "" ' TJS 18/03/11
                        LongToAlpha(Len(strSubject), 2, strTemp)
                        strAuth = strTemp & ":"
                        ' urlencode and then urldecode content before getting length to mimic processing happening during posting of data
                        LongToAlpha(Len(HttpUtility.UrlDecode(HttpUtility.UrlEncode(strContent))), 5, strTemp) ' TJS 21/03/11
                        strAuth = strAuth & strTemp
                        strNotificationXML = strNotificationXML & "<Auth>" & ConvertEntitiesForXML(strAuth) & "</Auth>" & vbCrLf ' TJS 18/03/11
                        strNotificationXML = strNotificationXML & "</Message>" & vbCrLf & "</LerrynNotifyError>"

                        If m_LastCodeErrorTime <> DateSerial(2000, 1, 1) Then ' TJS 12/12/10
                            ' only send error if different from last one or at least one hour has passed since last email
                            If m_LastCodeErrorMessage <> strErrorMsgCore Or m_LastCodeErrorTime < DateAdd(DateInterval.Hour, -1, Date.Now) Then ' TJS 12/12/10 TJS 24/02/12
                                bSendErrorEmail = True ' TJS 12/12/10
                                m_LastCodeErrorTime = Date.Now ' TJS 24/02/12
                                m_LastCodeErrorMessage = strErrorMsgCore ' TJS 24/02/12
                            Else
                                bSendErrorEmail = False ' TJS 12/12/10
                            End If
                        Else
                            bSendErrorEmail = True ' TJS 12/12/10
                            m_LastCodeErrorTime = Date.Now ' TJS 24/02/12
                            m_LastCodeErrorMessage = strErrorMsgCore ' TJS 24/02/12
                        End If

                        If (Left(UCase(My.Computer.Name), 5) <> "LERHQ" And Left(UCase(My.Computer.Name), 9) <> "MYPAYE-LT" And bSendErrorEmail) Or ForceSendEmail() Then ' TJS 12/12/10 TJS 01/05/14

                            'Use Interprise business rule to send notification
                            Dim emailMessageRule As Interprise.Extendable.Base.Business.CRM.IEmailMessageInterface = New Interprise.Business.Base.CRM.EMailMessageRule()
                            Dim errorMessage As String = String.Empty

                            'TODO: email sending
                            'emailMessageRule.SendEmail(toAddress, ccAddress, strSubject, strNotificationXML, errorMessage)
                        End If
                    End If
                    ' end of code replaced TJS 07/01/11
                End If

            Catch ex As Exception
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendExceptionEmail - " & ex.Message & ex.ToString)
                If XMLSource <> "" Then
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
                Else
                    WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ProcedureException.Message & vbCrLf & ProcedureException.StackTrace)
                End If

            End Try
            ' end of code replaced TJS 12/12/10

        Catch Ex As Exception
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendExceptionEmail(a) - " & Ex.Message & Ex.ToString) ' TJS 04/05/11

        End Try

    End Sub

    Public Sub SendExceptionEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, _
    ByVal ErrorDetails As String, Optional ByVal XMLSource As String = "") ' TJS 02/12/11 TJS 10/06/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Notifies key parties of code errors
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/05/11 | TJS             | 2011.0.13 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to use core or error message only when detecting repeat error messages
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from ErrorReporting module and modified to use module variable for BaseProductName, BaseProductCode and ActivationCode
        ' 28/07/13 | TJS             | 2013.1.31 | Modified to include product and host version details
        ' 04/01/14 | TJS             | 2013.4.03 | Corrected getting host version details
        ' 19/03/14 | TJS             | 2014.0.00 | Modified to use SystemError@lerryn.com email address
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected machine names in checks on sending emails
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, strReturn As String ' TJS 02/12/11
        Dim strNotificationXML As String, strSubject As String, strContent As String, strAuth As String ' TJS 07/01/11
        Dim strTemp As String, strErrorMsgCore As String, bSendErrorEmail As Boolean ' TJS 12/12/10 TJS 07/01/11 TJS 24/02/12
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"

        Try
            If XMLSource <> "" Then
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ErrorDetails & vbCrLf & "Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
            Else
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in " & ProcedureName & " - " & ErrorDetails)
            End If

            Try

                If SourceConfig IsNot Nothing Then
                    strNotificationXML = "<LerrynNotifyError>" & vbCrLf & "<Function>Code Exception</Function>" & vbCrLf
                    strNotificationXML = strNotificationXML & "<Product>" & ConvertEntitiesForXML(m_BaseProductCode) & "</Product>" & vbCrLf
                    strNotificationXML = strNotificationXML & "<Activation>" & ConvertEntitiesForXML(m_ActivationCode) & "</Activation>" & vbCrLf
                    strNotificationXML = strNotificationXML & "<Version>" & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion & "</Version>" ' TJS 28/07/13
                    strNotificationXML = strNotificationXML & "<HostVersion>" & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location.Replace("Lerryn.Business.ImportExport.dll", "Interprise.Facade.Base.dll")).ProductVersion & "</HostVersion>" ' TJS 28/07/13 TJS 04/01/14
                    strNotificationXML = strNotificationXML & "<Message>" & vbCrLf
                    ' has local notification email address been set or notifications to Lerryn not disabled ?
                    If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Or Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Or _
                        GetElementText(SourceConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then
                        If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Then ' TJS 10/06/12
                            toAddress = GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
                            strNotificationXML = strNotificationXML & "<MailTo>" & ConvertEntitiesForXML(toAddress)
                            strNotificationXML = strNotificationXML & "</MailTo>" & vbCrLf & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf
                            If GetElementText(SourceConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then
                                strNotificationXML = strNotificationXML & "<MailCC>eshopconnected@connectedbusiness.com</MailCC>" & vbCrLf ' TJS 19/03/14
                            End If

                        ElseIf Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Then ' TJS 10/06/12
                            toAddress = m_BaseErrorEmailAddress
                            strNotificationXML = strNotificationXML & "<MailTo>" & ConvertEntitiesForXML(m_BaseErrorEmailAddress) ' TJS 10/06/12
                            strNotificationXML = strNotificationXML & "</MailTo>" & vbCrLf & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf ' TJS 10/06/12
                            If m_BaseSendCodeErrorEmailsToLerryn Then ' TJS 10/06/12
                                strNotificationXML = strNotificationXML & "<MailCC>eshopconnected@connectedbusiness.com</MailCC>" & vbCrLf ' TJS 10/06/12 TJS 19/03/14
                            End If

                        Else
                            strNotificationXML = strNotificationXML & "<MailTo>eshopconnected@connectedbusiness.com</MailTo>" & vbCrLf ' TJS 19/03/14
                            strNotificationXML = strNotificationXML & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf
                        End If
                        strSubject = m_BaseProductName & " Error - " & ProcedureName
                        strNotificationXML = strNotificationXML & "<Subject>" & ConvertEntitiesForXML(strSubject) & "</Subject>" & vbCrLf
                        strErrorMsgCore = "Error in " & ProcedureName & vbCrLf & ErrorDetails ' TJS 24/02/12
                        strContent = strErrorMsgCore ' TJS 24/02/12
                        If XMLSource <> "" Then
                            strContent = strContent & vbCrLf & vbCrLf & XMLSource
                        End If
                        strNotificationXML = strNotificationXML & "<Content><![CDATA[" & strContent & "]]></Content>" & vbCrLf
                        strTemp = ""
                        LongToAlpha(Len(strSubject), 2, strTemp)
                        strAuth = strTemp & ":"
                        ' urlencode and then urldecode content before getting length to mimic processing happening during posting of data
                        LongToAlpha(Len(HttpUtility.UrlDecode(HttpUtility.UrlEncode(strContent))), 5, strTemp)
                        strAuth = strAuth & strTemp
                        strNotificationXML = strNotificationXML & "<Auth>" & ConvertEntitiesForXML(strAuth) & "</Auth>" & vbCrLf
                        strNotificationXML = strNotificationXML & "</Message>" & vbCrLf & "</LerrynNotifyError>"

                        If m_LastCodeErrorTime <> DateSerial(2000, 1, 1) Then
                            ' only send error if different from last one or at least one hour has passed since last email
                            If m_LastCodeErrorMessage <> strErrorMsgCore Or m_LastCodeErrorTime < DateAdd(DateInterval.Hour, -1, Date.Now) Then ' TJS 24/02/12
                                bSendErrorEmail = True
                                m_LastCodeErrorTime = Date.Now ' TJS 24/02/12
                                m_LastCodeErrorMessage = strErrorMsgCore ' TJS 24/02/12
                            Else
                                bSendErrorEmail = False
                            End If
                        Else
                            bSendErrorEmail = True
                            m_LastCodeErrorTime = Date.Now ' TJS 24/02/12
                            m_LastCodeErrorMessage = strErrorMsgCore ' TJS 24/02/12
                        End If
                        m_LastCodeErrorMessage = strErrorMsgCore ' TJS 24/02/12

                        If (Left(UCase(My.Computer.Name), 5) <> "LERHQ" And Left(UCase(My.Computer.Name), 9) <> "MYPAYE-LT" And bSendErrorEmail) Or ForceSendEmail() Then ' TJS 01/05/14
                            ' start of code replaced TJS 02/12/11
                            Try
                                'Use Interprise business rule to send notification
                                Dim emailMessageRule As Interprise.Extendable.Base.Business.CRM.IEmailMessageInterface = New Interprise.Business.Base.CRM.EMailMessageRule()
                                Dim errorMessage As String = String.Empty
                                'TODO: Emily :  email sending
                               ' emailMessageRule.SendEmail(toAddress, ccAddress, strSubject, strNotificationXML, errorMessage)

                            Catch ex As Exception
                                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_WARNING, "Unable to send error notification - " & ex.Message)

                            Finally
                                If Not postStream Is Nothing Then postStream.Close()
                                If Not WebResponse Is Nothing Then WebResponse.Close()

                            End Try
                            ' end of code replaced TJS 02/12/11
                        End If
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

        Catch Ex As Exception
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendExceptionEmail(b) - " & Ex.Message & Ex.ToString)

        End Try

    End Sub

    Public Sub SendSrcErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, _
        ByVal Message As String, Optional ByVal XMLSource As String = "") ' TJS 07/01/11 TJS 02/12/11 TJS 10/06/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Notifies key parties of input errors
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 28/01/09 | TJS             | 2009.1.01 | Corrected email address logic and added BaseProductName parameter
        ' 11/02/09 | TJS             | 2009.1.07 | Modified to enable forcing of emails on Lerryn computers
        ' 22/02/09 | TJS             ! 2009.1.08 | Corrected addition of TimSheppard@lerryn.com address
        ' 09/03/09 | TJS             | 2009.1.09 | Modified to trap error if CDO not installed and remove line feeds from source XML for log file
        ' 03/04/09 | TJS             | 2009.2.00 | Modified to include CDO.Configuration within CDO error trap
        ' 04/10/10 | TJS             | 2010.1.05 | Modifed to include error detail when unable to send email
        ' 12/12/10 | TJS             | 2010.1.12 | Code re-written to use System.Net.Mail instead of CDO
        ' 14/12/10 | FA              | 2010.1.14 | added ObjMail delivery method other wise 'Mailbox unavailable, could not relay message'
        ' 07/01/11 | TJS             | 2010.1.15 | Code re-written to use XML Web Service at LerrynSecure.com to send 
        '                                        | emails as Windows 7 workstation excludes SMTP Mail from IIS
        ' 18/03/11 | TJS             | 2011.0.01 | Corrected entity conversion for XML and calculation message length security value
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to prevent sending same message repeatedly
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from ErrorReporting module and modified to use module variable for BaseProductName, BaseProductCode and ActivationCode
        ' 08/07/13 | TJS             | 2013.1.27 | Modified XML Function element to identify as Source error
        ' 19/03/14 | TJS             | 2014.0.00 | Modified to use SystemError@lerryn.com email address
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected machine names in checks on sending emails
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, strReturn As String ' TJS 02/12/11
        Dim strNotificationXML As String, strSubject As String, strContent As String, strAuth As String ' TJS 07/01/11
        Dim strTemp As String, strErrorMsgCore As String, bSendErrorEmail As Boolean ' TJS 07/01/11 TJS 24/02/12
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"

        Try
            If XMLSource <> "" Then
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message & ", Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, ""))
            Else
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message)
            End If

            ' start of code replaced TJS 12/12/10
            Try

                If SourceConfig IsNot Nothing Then ' TJS 24/11/10
                    ' start of code replaced TJS 07/01/11
                    strNotificationXML = "<LerrynNotifyError>" & vbCrLf & "<Function>Source Error</Function>" & vbCrLf ' TJS 08/07/13
                    strNotificationXML = strNotificationXML & "<Product>" & ConvertEntitiesForXML(m_BaseProductCode) & "</Product>" & vbCrLf ' TJS 18/03/11
                    strNotificationXML = strNotificationXML & "<Activation>" & ConvertEntitiesForXML(m_ActivationCode) & "</Activation>" & vbCrLf ' TJS 18/03/11
                    strNotificationXML = strNotificationXML & "<Message>" & vbCrLf
                    ' has local notification email address been set or notifications to Lerryn not disabled ?
                    If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Or Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Or _
                        GetElementText(SourceConfig, SOURCE_CONFIG_SEND_SOURCE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then ' TJS 10/06/12
                        If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Then ' TJS 10/06/12
                            toAddress = GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
                            strNotificationXML = strNotificationXML & "<MailTo>" & ConvertEntitiesForXML(toAddress) ' TJS 18/03/11
                            strNotificationXML = strNotificationXML & "</MailTo>" & vbCrLf & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf
                            If GetElementText(SourceConfig, SOURCE_CONFIG_SEND_SOURCE_ERROR_EMAILS_TO_LERRYN).ToUpper = "YES" Then
                                strNotificationXML = strNotificationXML & "<MailCC>eshopconnected@connectedbusiness.com</MailCC>" & vbCrLf ' TJS 19/03/14
                            End If

                        ElseIf Not String.IsNullOrEmpty(m_BaseErrorEmailAddress) Then ' TJS 10/06/12
                            toAddress = m_BaseErrorEmailAddress
                            strNotificationXML = strNotificationXML & "<MailTo>" & ConvertEntitiesForXML(m_BaseErrorEmailAddress) ' TJS 10/06/12
                            strNotificationXML = strNotificationXML & "</MailTo>" & vbCrLf & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf ' TJS 10/06/12
                            If m_BaseSendSourceErrorEmailsToLerryn Then ' TJS 10/06/12
                                strNotificationXML = strNotificationXML & "<MailCC>eshopconnected@connectedbusiness.com</MailCC>" & vbCrLf ' TJS 10/06/12 TJS 19/03/14
                            End If

                        Else
                            strNotificationXML = strNotificationXML & "<MailTo>eshopconnected@connectedbusiness.com</MailTo>" & vbCrLf ' TJS 19/03/14
                            strNotificationXML = strNotificationXML & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf
                        End If
                        strSubject = m_BaseProductName & " Source Error - " & ProcedureName
                        strNotificationXML = strNotificationXML & "<Subject>" & ConvertEntitiesForXML(strSubject) & "</Subject>" & vbCrLf ' TJS 18/03/11
                        strErrorMsgCore = "Source Error in " & ProcedureName & vbCrLf & Message ' TJS 24/02/12
                        strContent = strErrorMsgCore ' TJS 24/02/12
                        If XMLSource <> "" Then
                            strContent = strContent & vbCrLf & vbCrLf & XMLSource
                        End If
                        strNotificationXML = strNotificationXML & "<Content><![CDATA[" & strContent & "]]></Content>" & vbCrLf ' TJS 18/03/11
                        strTemp = "" ' TJS 18/03/11
                        LongToAlpha(Len(strSubject), 2, strTemp)
                        strAuth = strTemp & ":"
                        ' urlencode and then urldecode content before getting length to mimic processing happening during posting of data
                        LongToAlpha(Len(HttpUtility.UrlDecode(HttpUtility.UrlEncode(strContent))), 5, strTemp) ' TJS 21/03/11
                        strAuth = strAuth & strTemp
                        strNotificationXML = strNotificationXML & "<Auth>" & ConvertEntitiesForXML(strAuth) & "</Auth>" & vbCrLf ' TJS 18/03/11
                        strNotificationXML = strNotificationXML & "</Message>" & vbCrLf & "</LerrynNotifyError>"

                        If m_LastSourceErrorTime <> DateSerial(2000, 1, 1) Then ' TJS 24/02/12
                            ' only send error if different from last one or at least one hour has passed since last email
                            If m_LastSourceErrorMessage <> strErrorMsgCore Or m_LastSourceErrorTime < DateAdd(DateInterval.Hour, -1, Date.Now) Then ' TJS 24/02/12
                                bSendErrorEmail = True ' TJS 24/02/12
                                m_LastSourceErrorTime = Date.Now ' TJS 24/02/12
                                m_LastSourceErrorMessage = strErrorMsgCore ' TJS 24/02/12
                            Else
                                bSendErrorEmail = False ' TJS 24/02/12
                            End If
                        Else
                            bSendErrorEmail = True ' TJS 24/02/12
                            m_LastSourceErrorTime = Date.Now ' TJS 24/02/12
                            m_LastSourceErrorMessage = strErrorMsgCore ' TJS 24/02/12
                        End If

                        If (Left(UCase(My.Computer.Name), 5) <> "LERHQ" And Left(UCase(My.Computer.Name), 9) <> "MYPAYE-LT" And bSendErrorEmail) Or ForceSendEmail() Then ' TJS 24/02/12 TJS 01/05/14
                            ' start of code replaced TJS 02/12/11
                            Try

                                'Use Interprise business rule to send notification
                                Dim emailMessageRule As Interprise.Extendable.Base.Business.CRM.IEmailMessageInterface = New Interprise.Business.Base.CRM.EMailMessageRule()
                                Dim errorMessage As String = String.Empty
                                'TODO: Emily :  email sending
                                'emailMessageRule.SendEmail(toAddress, ccAddress, strSubject, strNotificationXML, errorMessage)

                            Catch ex As Exception
                                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_WARNING, "Unable to send error notification - " & ex.Message) ' TJS 06/10/10

                            Finally
                                If Not postStream Is Nothing Then postStream.Close()
                                If Not WebResponse Is Nothing Then WebResponse.Close()

                            End Try
                            ' end of code replaced TJS 02/12/11
                        End If
                    End If
                    ' end of code replaced TJS 07/01/11
                End If

            Catch ex As Exception
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendSrcErrorEmail - " & ex.Message & ex.ToString)

            End Try
            ' end of code replaced TJS 12/12/10

        Catch ex As Exception ' TJS 03/04/09
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendSrcErrorEmail - " & ex.Message & ex.ToString) ' TJS 03/04/09
            If XMLSource <> "" Then ' TJS 03/04/09
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message & ", Source XML :-" & vbCrLf & XMLSource.Replace(vbCr, "").Replace(vbLf, "")) ' TJS 03/04/09
            Else
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_SOURCE_ERROR, "Source Error in " & ProcedureName & " - " & Message) ' TJS 03/04/09
            End If

        End Try

    End Sub

    Public Sub SendPmntErrorEmail(ByVal SourceConfig As XDocument, ByVal Message As String) ' TJS 07/01/11 TJS 02/12/11 TJS 10/06/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Notifies key parties of payment failures
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 04/10/10 | TJS             | 2010.1.05 | Modifed to include error detail when unable to send email
        ' 12/12/10 | TJS             | 2010.1.12 | Code re-written to use System.Net.Mail instead of CDO
        ' 14/12/10 | FA              | 2010.1.14 | added ObjMail delivery method other wise 'Mailbox unavailable, could not relay message'
        ' 07/01/11 | TJS             | 2010.1.15 | Code re-written to use XML Web Service at LerrynSecure.com to send 
        '                                        | emails as Windows 7 workstation excludes SMTP Mail from IIS
        ' 18/03/11 | TJS             | 2011.0.01 | Corrected entity conversion for XML and calculation message length security value
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from ErrorReporting module and modified to use module variable for BaseProductName, BaseProductCode and ActivationCode
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected machine names in checks on sending emails
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, strReturn As String ' TJS 02/12/11
        Dim strNotificationXML As String, strSubject As String, strAuth As String ' TJS 07/01/11
        Dim strTemp As String ' TJS 07/01/11
        Dim toAddress As String = "eshopconnected@connectedbusiness.com"
        Dim ccAddress As String = "eshopconnected@connectedbusiness.com"

        Try
            WriteToLogFileOrEvent("!PmtErr!", Message)

            Try

                If SourceConfig IsNot Nothing Then ' TJS 24/11/10
                    ' start of code replaced TJS 07/01/11
                    strNotificationXML = "<LerrynNotifyError>" & vbCrLf & "<Function>Payment Error</Function>" & vbCrLf
                    strNotificationXML = strNotificationXML & "<Product>" & ConvertEntitiesForXML(m_BaseProductCode) & "</Product>" & vbCrLf ' TJS 18/03/11
                    strNotificationXML = strNotificationXML & "<Activation>" & ConvertEntitiesForXML(m_ActivationCode) & "</Activation>" & vbCrLf ' TJS 18/03/11
                    strNotificationXML = strNotificationXML & "<Message>" & vbCrLf
                    ' has local notification email address been set ?
                    If Not String.IsNullOrEmpty(GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)) Then ' TJS 10/06/12
                        toAddress = GetElementText(SourceConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
                        strNotificationXML = strNotificationXML & "<MailTo>" & ConvertEntitiesForXML(toAddress) ' TJS 18/03/11
                        strNotificationXML = strNotificationXML & "</MailTo>" & vbCrLf & "<MailFrom>Support@ConnectedBusiness.com</MailFrom>" & vbCrLf
                        strSubject = m_BaseProductName & " Payment Failure"
                        strNotificationXML = strNotificationXML & "<Subject>" & ConvertEntitiesForXML(strSubject) & "</Subject>" & vbCrLf ' TJS 18/03/11
                        strNotificationXML = strNotificationXML & "<Content><![CDATA[" & Message & "]]></Content>" & vbCrLf ' TJS 18/03/11
                        strTemp = "" ' TJS 18/03/11
                        LongToAlpha(Len(strSubject), 2, strTemp)
                        strAuth = strTemp & ":"
                        ' urlencode and then urldecode content before getting length to mimic processing happening during posting of data
                        LongToAlpha(Len(HttpUtility.UrlDecode(HttpUtility.UrlEncode(Message))), 5, strTemp) ' TJS 21/03/11
                        strAuth = strAuth & strTemp
                        strNotificationXML = strNotificationXML & "<Auth>" & ConvertEntitiesForXML(strAuth) & "</Auth>" & vbCrLf ' TJS 18/03/11
                        strNotificationXML = strNotificationXML & "</Message>" & vbCrLf & "</LerrynNotifyError>"

                        If (Left(UCase(My.Computer.Name), 5) <> "LERHQ" And Left(UCase(My.Computer.Name), 9) <> "MYPAYE-LT") Or ForceSendEmail() Then ' TJS 01/05/14
                            ' start of code replaced TJS 02/12/11
                            Try

                                'Use Interprise business rule to send notification
                                Dim emailMessageRule As Interprise.Extendable.Base.Business.CRM.IEmailMessageInterface = New Interprise.Business.Base.CRM.EMailMessageRule()
                                Dim errorMessage As String = String.Empty

                                'TODO: Emily :  email sending
                                'emailMessageRule.SendEmail(toAddress, ccAddress, strSubject, strNotificationXML, errorMessage)

                            Catch ex As Exception
                                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_WARNING, "Unable to send error notification - " & ex.Message) ' TJS 06/10/10

                            Finally
                                If Not postStream Is Nothing Then postStream.Close()
                                If Not WebResponse Is Nothing Then WebResponse.Close()

                            End Try
                            ' end of code replaced TJS 02/12/11
                        End If
                    End If
                    ' end of code replaced TJS 07/01/11
                End If

            Catch ex As Exception
                WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendPmntErrorEmail - " & ex.Message & ex.ToString)

            End Try

        Catch ex As Exception
            WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_ERROR, "Error in SendPmntErrorEmail - " & ex.Message & ex.ToString)
            WriteToLogFileOrEvent("!PmtErr!", Message)

        End Try

    End Sub

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
#End Region
End Class
