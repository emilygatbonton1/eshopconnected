' eShopCONNECT for Connected Business
' Module: DynamicInstaller.vb
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
' Last Updated - 24 August 2012

Imports System.Collections
Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.ServiceProcess
Imports Microsoft.Win32

Namespace ServiceInstaller
    ' <summary>
    ' This is a custom project installer.
    ' Applies a unique name to the service using the /name switch
    ' Sets user name and password using the /user and /password switches
    ' Allows the use of a local account using the /account switch
    ' obtained from http://www.codeproject.com/KB/cs/DynWinServiceInstallUtil.aspx
    ' </summary>

	<RunInstaller(True)> _
	Public Class DynamicInstaller
        Inherits Installer

		Public Property ServiceName() As String
			Get
				Return serviceInstaller.ServiceName
			End Get
			Set
				serviceInstaller.ServiceName = value
			End Set
        End Property

		Public Property DisplayName() As String
			Get
				Return serviceInstaller.DisplayName
			End Get
			Set
				serviceInstaller.DisplayName = value
			End Set
        End Property

		Public Property Description() As String
			Get
				Return serviceInstaller.Description
			End Get
			Set
				serviceInstaller.Description = value
			End Set
        End Property

		Public Property StartType() As ServiceStartMode
			Get
				Return serviceInstaller.StartType
			End Get
			Set
				serviceInstaller.StartType = value
			End Set
        End Property

		Public Property Account() As ServiceAccount
			Get
				Return processInstaller.Account
			End Get
			Set
				processInstaller.Account = value
			End Set
        End Property

		Public Property ServiceUsername() As String
			Get
				Return processInstaller.Username
			End Get
			Set
				processInstaller.Username = value
			End Set
        End Property

		Public Property ServicePassword() As String
			Get
				Return processInstaller.Password
			End Get
			Set
				processInstaller.Password = value
			End Set
        End Property

		Private processInstaller As ServiceProcessInstaller
        Private serviceInstaller As System.ServiceProcess.ServiceInstaller

        Public Sub New()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -   
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            processInstaller = New ServiceProcessInstaller()
            processInstaller.Account = ServiceAccount.LocalService
            processInstaller.Username = Nothing
            processInstaller.Password = Nothing

            serviceInstaller = New System.ServiceProcess.ServiceInstaller()
            serviceInstaller.StartType = ServiceStartMode.Automatic
            serviceInstaller.ServiceName = SERVICE_NAME
            serviceInstaller.DisplayName = "Lerryn eShopCONNECT for " & IS_PRODUCT_NAME & " Service" ' TJS 24/08/12
            serviceInstaller.Description = "The Lerryn eShopCONNECT for " & IS_PRODUCT_NAME & " Service carries out the automated functions of eShopCONNECT sources including the polling for orders and posting of status and inventory updates to/from sources" ' TJS 24/08/12
            serviceInstaller.ServicesDependedOn = New String() {"LanmanServer"}

            Installers.AddRange(New Installer() {processInstaller, serviceInstaller})
        End Sub

		#Region "Access parameters"
		''' <summary>
        ''' Return the value of the parameter in dictated by key
        ''' </summary>
        ''' <PARAM name="key">Context parameter key</PARAM>
        ''' <returns>Context parameter specified by key</returns>

		Public Function GetContextParameter(key As String) As String
			Dim sValue As String = ""
			Try
				sValue = Me.Context.Parameters(key).ToString()
			Catch
				sValue = ""
			End Try
			Return sValue
		End Function
		#End Region
		''' <summary>
        ''' This method is run before the install process.
        ''' This method is overridden to set the following parameters:
        ''' service name (/name switch)
        ''' account type (/account switch)
        ''' for a user account user name (/user switch)
        ''' for a user account password (/password switch)
        ''' Note that when using a user account,
        ''' if the user name or password is not set,
        ''' the installing user is prompted for the credentials to use.
        ''' </summary>
        ''' <PARAM name="savedState"></PARAM>

		Protected Overrides Sub OnBeforeInstall(savedState As IDictionary)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -   Extracts the source settings
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 17/02/12 | TJS             | 2011.2.06 | Modified to allow setting of Service Display Name
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            MyBase.OnBeforeInstall(savedState)
			Dim isUserAccount As Boolean = False

			' Decode the command line switches

			Dim name As String = GetContextParameter("name").Trim()
			If name <> "" Then
				serviceInstaller.ServiceName = name
            End If
            Dim displayname As String = GetContextParameter("displayname").Trim() ' TJS 17/02/12
            If displayname <> "" Then ' TJS 17/02/12
                serviceInstaller.DisplayName = displayname ' TJS 17/02/12
            End If
            Dim desc As String = GetContextParameter("desc").Trim()
			If desc <> "" Then
				serviceInstaller.Description = desc
			End If
			' What type of credentials to use to run the service

			Dim acct As String = GetContextParameter("account")
			Select Case acct.ToLower()
				Case "user"
					processInstaller.Account = ServiceAccount.User
					isUserAccount = True
					Exit Select
				Case "localservice"
					processInstaller.Account = ServiceAccount.LocalService
					Exit Select
				Case "localsystem"
					processInstaller.Account = ServiceAccount.LocalSystem
					Exit Select
				Case "networkservice"
					processInstaller.Account = ServiceAccount.NetworkService
					Exit Select
			End Select
			' User name and password

			Dim username As String = GetContextParameter("user").Trim()
			Dim password As String = GetContextParameter("password").Trim()
			' Should I use a user account?

			If isUserAccount Then
				' If we need to use a user account,

				' set the user name and password

				If username <> "" Then
					processInstaller.Username = username
				End If
				If password <> "" Then
					processInstaller.Password = password
				End If
			End If
		End Sub
		''' <summary>
        ''' Uninstall based on the service name
        ''' </summary>
        ''' <PARAM name="savedState"></PARAM>

		Protected Overrides Sub OnBeforeUninstall(savedState As IDictionary)
			MyBase.OnBeforeUninstall(savedState)
			' Set the service name based on the command line

			Dim name As String = GetContextParameter("name").Trim()
			If name <> "" Then
				serviceInstaller.ServiceName = name
			End If
		End Sub
	End Class
	'end class
End Namespace

