' eShopCONNECT for Connected Business
' Module: ConfigSettingsDetailSection.vb
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

Imports Microsoft.VisualBasic
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports System.Xml.Linq ' TJS 05/07/12
Imports System.Xml.XPath ' TJS 05/07/12

#Region " ConfigSettingsDetailSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.Add, _
    "Lerryn.Presentation.ImportExport.ConfigSettingsSection")> _
Public Class ConfigSettingsDetailSection

#Region " Variables "
    Private m_ConfigSettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_strCurrentConfigGroup As String
    Private m_EBaySettings As Lerryn.Framework.ImportExport.SourceConfig.eBaySettings ' TJS 02/12/11
    Private m_EBayDetailsAvailable As Boolean ' TJS 02/12/11
    Private m_EditingEBayCountry As Boolean ' TJS 02/12/11
    Private m_EBaySites() As Lerryn.Facade.ImportExport.eBayXMLConnector.eBaySites ' TJS 02/12/11
    Private m_MagentoConfigCount As Integer ' TJS 15/11/13
    Private m_MagentoAPIURL() As String ' TJS 15/11/13
    Private m_MagentoAPIUser() As String ' TJS 15/11/13
    Private m_MagentoAPIPwd() As String ' TJS 15/11/13
    Private m_MagentoV2APIWSI() As Boolean ' TJS 15/11/13
    Private m_MagentoVersion() As String ' TJS 15/11/13
    Private m_MagentoAPIVersion() As Decimal ' TJS 15/11/13
    Private m_MagentoVersionChecked() As Boolean ' TJS 15/11/13
    Private m_MagentoVersionPtr() As Integer ' TJS 15/11/13
    Private m_MagentoAPIVersionPtr() As Integer ' TJS 15/11/13
    Private WithEvents m_BackgroundWorker As System.ComponentModel.BackgroundWorker ' TJS 02/12/11
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.ConfigSettingsSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_ConfigSettingsSectionFacade
        End Get
    End Property
#End Region

#Region " CurrentConfigGroup "
    Public Property CurrentConfigGroup() As String
        Get
            Return m_strCurrentConfigGroup
        End Get
        Set(ByVal value As String)
            m_strCurrentConfigGroup = value
        End Set
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/04/12 | TJS             | 2011.2.12 | Function added to make work with IS 6.0.4.x
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Me.m_ImportExportDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Me.m_ConfigSettingsSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.m_ImportExportDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByRef gatewayOrderImporterDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
            ByVal facadeAsset As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for eBay
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Me.m_ImportExportDataset = gatewayOrderImporterDataset
        Me.m_ConfigSettingsSectionFacade = facadeAsset

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.TextEditSourceCode.Focus()
        m_EBayDetailsAvailable = False ' TJS 02/12/11
        m_EditingEBayCountry = False ' TJS 02/12/11
        m_EBaySettings = Nothing ' TJS 02/12/11

    End Sub
#End Region

#Region " InitializeControl "
    Protected Overrides Sub InitializeControl()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for eBay
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'This call is required by the Presentation Layer.
        MyBase.InitializeControl()

        'Add any initialization after the InitializeControl() call
        Me.rbeCustBizTypeEdit.Items.AddRange(New Object() {Me.m_ConfigSettingsSectionFacade.GetMessage("LBL0022"), Me.m_ConfigSettingsSectionFacade.GetMessage("LBL0023")}) ' TJS 07/07/09
        ReDim m_EBaySites(0) ' TJS 02/12/11

    End Sub
#End Region

#Region " SetInitialFocus "
    Public Sub SetInitialFocus()

        Me.TextEditSourceCode.Focus()

    End Sub
#End Region

#Region " SetSourceSpecificValues "
    Public Function SetSourceSpecificValues() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 22/04/13 | TJS             | 2013.1.11 | Modified to call GetEBaySiteList even if eBay login not set
        ' 07/05/13 | TJS             | 2013.1.13 | Modified to only check eBay when ebay source selected
        ' 02/10/13 | TJS             | 2013.3.03 | Modified to get Magento Version
        ' 15/11/13 | TJS             | 2013.3.09 | Moved code from GetMagentoVersion to read Magento login details 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strConfigGroup As String, iLoop As Integer ' TJS 15/11/13

        If Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221 = EBAY_SOURCE_CODE Then ' TJS 07/05/13
            m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
            m_BackgroundWorker.WorkerSupportsCancellation = True
            m_BackgroundWorker.WorkerReportsProgress = False
            AddHandler m_BackgroundWorker.DoWork, AddressOf GetEBaySiteList
            AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetEBaySiteListCompleted
            m_BackgroundWorker.RunWorkerAsync()
            If CheckEBayLoginSettings() Then
                Return True
            Else
                Return False
            End If

            ' start of code added TJS 02/10/13
        ElseIf Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221 = MAGENTO_SOURCE_CODE Then
            ' start of code moved from GetMagentoVersion and amended TJS 15/11/13
            strConfigGroup = ""
            m_MagentoConfigCount = 0
            ' have to cater for multiple Magento instances and worker thread cannot access table directly otherwise we get a cross thread error
            For iLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                If Strings.Left(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup, 7) = "Magento" Then
                    If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup <> strConfigGroup Then
                        strConfigGroup = Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup
                        ReDim m_MagentoAPIURL(m_MagentoConfigCount)
                        ReDim m_MagentoAPIUser(m_MagentoConfigCount)
                        ReDim m_MagentoAPIPwd(m_MagentoConfigCount)
                        ReDim m_MagentoV2APIWSI(m_MagentoConfigCount)
                        ReDim m_MagentoVersionChecked(m_MagentoConfigCount)
                        ReDim m_MagentoVersion(m_MagentoConfigCount)
                        ReDim m_MagentoAPIVersion(m_MagentoConfigCount)
                        ReDim m_MagentoVersionPtr(m_MagentoConfigCount)
                        ReDim m_MagentoAPIVersionPtr(m_MagentoConfigCount)
                        m_MagentoAPIURL(m_MagentoConfigCount) = ""
                        m_MagentoAPIUser(m_MagentoConfigCount) = ""
                        m_MagentoAPIPwd(m_MagentoConfigCount) = ""
                        m_MagentoV2APIWSI(m_MagentoConfigCount) = False
                        m_MagentoVersionChecked(m_MagentoConfigCount) = False
                        m_MagentoVersion(m_MagentoConfigCount) = ""
                        m_MagentoAPIVersion(m_MagentoConfigCount) = 0
                        m_MagentoVersionPtr(m_MagentoConfigCount) = -1
                        m_MagentoAPIVersionPtr(m_MagentoConfigCount) = -1
                        m_MagentoConfigCount += 1

                    ElseIf Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName = "APIURL" Then
                        m_MagentoAPIURL(m_MagentoConfigCount - 1) = Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingValue

                    ElseIf Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName = "APIUser" Then
                        m_MagentoAPIUser(m_MagentoConfigCount - 1) = Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingValue

                    ElseIf Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName = "APIPwd" Then
                        m_MagentoAPIPwd(m_MagentoConfigCount - 1) = Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingValue

                    ElseIf Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName = "V2SoapAPIWSICompliant" Then
                        m_MagentoV2APIWSI(m_MagentoConfigCount - 1) = CBool(IIf(UCase(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingValue) = "YES", True, False))

                    ElseIf Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName = "MagentoVersion" Then
                        m_MagentoVersionPtr(m_MagentoConfigCount - 1) = iLoop

                    ElseIf Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName = "LerrynAPIVersion" Then
                        m_MagentoAPIVersionPtr(m_MagentoConfigCount - 1) = iLoop

                    End If
                End If
            Next
            ' end of code moved from GetMagentoVersion TJS 15/11/13
            m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
            m_BackgroundWorker.WorkerSupportsCancellation = True
            m_BackgroundWorker.WorkerReportsProgress = False
            AddHandler m_BackgroundWorker.DoWork, AddressOf GetMagentoVersion
            AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetMagentoVersionCompleted
            m_BackgroundWorker.RunWorkerAsync()
            Return True
            ' end of code added TJS 02/10/13

        Else
            Return True ' TJS 07/05/13
        End If

    End Function
#End Region

#Region " CheckEBayLoginSettings "
    Private Function CheckEBayLoginSettings() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 22/04/13 | TJS/FA          | 2013.1.11 | Corrected detetion of multiple ebay accounts
        ' 07/05/13 | TJS             | 2013.1.13 | Further correction of multiple ebay account detection
        ' 29/05/13 | FA              | 2013.1.19 | Further correction of multiple ebay account detection
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to check if eBay connector activated
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        If Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221 = EBAY_SOURCE_CODE And _
            Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 09/08/13
            If m_EBaySettings Is Nothing Then
                m_EBaySettings = New Lerryn.Framework.ImportExport.SourceConfig.eBaySettings
            End If

            For iLoop = 0 To Me.GridViewConfigSettings.RowCount - 1
                If Not m_EBaySettings.AccountDisabled And m_EBaySettings.AuthToken <> "" And _
                    m_EBaySettings.eBayCountry >= 0 And m_EBaySettings.TokenExpires > Date.Now.AddDays(-1) Then ' TJS 07/05/13
                    Exit For
                End If
                If Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup") IsNot Nothing AndAlso _
                    (Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString = "eBay" OrElse _
                    (Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString.Length > 6 AndAlso _
                    Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString.Substring(0, 6) = "eBay :")) Then ' TJS 22/04/13 TJS 07/05/13 'FA 29/05/13
                    Select Case Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingName").ToString
                        Case "SiteID"
                            m_EBaySettings.SiteID = Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingValue").ToString
                        Case "Country"
                            If Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingID").ToString <> "" Then
                                m_EBaySettings.eBayCountry = CInt(Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingID"))
                            Else
                                Interprise.Presentation.Base.Message.MessageWindow.Show("Please select which eBay Country (Site) you want to connect to") ' TJS 07/05/13
                            End If
                        Case "AuthToken"
                            m_EBaySettings.AuthToken = Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingValue").ToString
                        Case "TokenExpires"
                            If Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingValue").ToString <> "" AndAlso _
                                m_ConfigSettingsSectionFacade.ValidateXMLDate(Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingValue").ToString) Then
                                m_EBaySettings.TokenExpires = m_ConfigSettingsSectionFacade.ConvertXMLDate(Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingValue").ToString)
                            Else
                                m_EBaySettings.TokenExpires = Date.Now.AddSeconds(-1)
                            End If

                        Case "AccountDisabled"
                            m_EBaySettings.AccountDisabled = CBool(IIf(UCase(Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingValue").ToString) = "YES", True, False))
                    End Select
                End If
            Next
            If Not m_EBaySettings.AccountDisabled And m_EBaySettings.AuthToken <> "" And _
                m_EBaySettings.eBayCountry >= 0 And m_EBaySettings.TokenExpires > Date.Now.AddDays(-1) Then ' TJS 07/05/13
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
#End Region

#Region " GetEBaySiteList "
    Private Sub GetEBaySiteList(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 22/04/13 | TJS/FA          | 2013.1.11 | Modified to populate initial list of eBay sites
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to cater for eBay Setting not created if connector not activated
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strErrorDetails As String, bUseEBaySandbox As Boolean, beBayError As Boolean ' TJS 22/04/13

        bUseEBaySandbox = (Me.m_ConfigSettingsSectionFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "UseEBaySandbox", "NO").ToUpper = "YES")
        strErrorDetails = ""
        beBayError = False ' TJS 22/04/13
        If m_EBaySettings IsNot Nothing AndAlso Not m_EBaySettings.AccountDisabled AndAlso m_EBaySettings.AuthToken <> "" AndAlso _
            m_EBaySettings.TokenExpires > Date.Now.AddDays(-1) Then ' TJS 09/08/13
            Me.m_ConfigSettingsSectionFacade.GetEBaySiteList(m_EBaySettings, bUseEBaySandbox, m_EBaySites, strErrorDetails)
            If strErrorDetails <> "" Then
                Interprise.Presentation.Base.Message.MessageWindow.Show(strErrorDetails)
                beBayError = True ' TJS 22/04/13
            End If
        Else
            beBayError = True ' TJS 22/04/13
        End If
        ' start of code added TJS 22/04/13
        If beBayError Then
            ReDim m_EBaySites(21)
            m_EBaySites(0).SiteID = 15
            m_EBaySites(0).SiteName = "Australia"
            m_EBaySites(1).SiteID = 16
            m_EBaySites(1).SiteName = "Austria"
            m_EBaySites(2).SiteID = 123
            m_EBaySites(2).SiteName = "Belgium (Dutch)"
            m_EBaySites(3).SiteID = 23
            m_EBaySites(3).SiteName = "Belgium (French)"
            m_EBaySites(4).SiteID = 2
            m_EBaySites(4).SiteName = "Canada"
            m_EBaySites(5).SiteID = 100
            m_EBaySites(5).SiteName = "eBayMotors"
            m_EBaySites(6).SiteID = 210
            m_EBaySites(6).SiteName = "Canada (French)"
            m_EBaySites(7).SiteID = 71
            m_EBaySites(7).SiteName = "France"
            m_EBaySites(8).SiteID = 77
            m_EBaySites(8).SiteName = "Germany"
            m_EBaySites(9).SiteID = 201
            m_EBaySites(9).SiteName = "HongKong"
            m_EBaySites(10).SiteID = 203
            m_EBaySites(10).SiteName = "India"
            m_EBaySites(11).SiteID = 205
            m_EBaySites(11).SiteName = "Ireland"
            m_EBaySites(12).SiteID = 101
            m_EBaySites(12).SiteName = "Italy"
            m_EBaySites(13).SiteID = 207
            m_EBaySites(13).SiteName = "Malaysia"
            m_EBaySites(14).SiteID = 146
            m_EBaySites(14).SiteName = "Netherlands"
            m_EBaySites(15).SiteID = 211
            m_EBaySites(15).SiteName = "Philippines"
            m_EBaySites(16).SiteID = 212
            m_EBaySites(16).SiteName = "Poland"
            m_EBaySites(17).SiteID = 216
            m_EBaySites(17).SiteName = "Singapore"
            m_EBaySites(18).SiteID = 186
            m_EBaySites(18).SiteName = "Spain"
            m_EBaySites(19).SiteID = 193
            m_EBaySites(19).SiteName = "Switzerland"
            m_EBaySites(20).SiteID = 3
            m_EBaySites(20).SiteName = "UK"
            m_EBaySites(21).SiteID = 0
            m_EBaySites(21).SiteName = "US"
        End If
        ' end of code added TJS 22/04/13

    End Sub

    Private Sub GetEBaySiteListCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem

        If m_EBaySites.Length > 0 Then
            Coll = rbeEbayCountryEdit.Items
            Coll.BeginUpdate()
            For Each eBaySite As Lerryn.Facade.ImportExport.eBayXMLConnector.eBaySites In m_EBaySites
                If eBaySite.SiteName <> "CustomCode" And eBaySite.SiteName <> "" Then
                    CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(eBaySite.SiteName)
                    Coll.Add(CollItem)
                    m_EBayDetailsAvailable = True
                End If
            Next eBaySite
            Coll.EndUpdate()

        End If
        If m_EditingEBayCountry And m_EBayDetailsAvailable Then
            Me.colConfigSettingValue.ColumnEdit = Me.rbeEbayCountryEdit
            m_EditingEBayCountry = False
        End If
        RemoveHandler m_BackgroundWorker.DoWork, AddressOf GetEBaySiteList
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetEBaySiteListCompleted

    End Sub
#End Region

#Region " GetMagentoVersion "
    Private Sub GetMagentoVersion(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/10/13 | TJS             | 2013.3.03 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Code completed
        ' 15/11/13 | TJS             | 2013.3.09 | Moved code to read Magento login details to 
        '                                        | and moved code to set Magento and API version to GetMagentoVersionCompleted 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim MagentoConnection As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim iLoop As Integer

        For iLoop = 0 To m_MagentoConfigCount - 1 ' TJS 15/11/13
            MagentoConnection = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
            MagentoConnection.V2SoapAPIWSICompliant = m_MagentoV2APIWSI(iLoop)
            If MagentoConnection.Login(m_MagentoAPIURL(iLoop), m_MagentoAPIUser(iLoop), m_MagentoAPIPwd(iLoop), True) Then
                m_MagentoVersionChecked(iLoop) = True
                m_MagentoVersion(iLoop) = MagentoConnection.MagentoVersion
                m_MagentoAPIVersion(iLoop) = MagentoConnection.LerrynAPIVersion
            End If
        Next

    End Sub

    Private Sub GetMagentoVersionCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -         
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/10/13 | TJS             | 2013.3.03 | Function added
        ' 15/11/13 | TJS             | 2013.3.09 | Moved code from GetMagentoVersion to set Magento and API version 
        ' 11/01/14 | TJS             | 2013.4.09 | Corrected setting of set Magento and API version in config grid
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iConfigLoop As Integer ' TJS 15/11/13

        RemoveHandler m_BackgroundWorker.DoWork, AddressOf GetMagentoVersion
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetMagentoVersionCompleted
        ' start of code moved from GetMagentoVersion and amended TJS 15/11/13
        For iLoop = 0 To m_MagentoConfigCount - 1 ' TJS 15/11/13
            If m_MagentoVersionChecked(iLoop) Then
                For iConfigLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                    If m_MagentoVersionPtr(iLoop) = iConfigLoop Then
                        Me.ConfigSettingsSectionGateway.XMLConfigSettings(iConfigLoop).ConfigSettingValue = m_MagentoVersion(iLoop) ' TJS 11/01/14
                    End If
                    If m_MagentoAPIVersionPtr(iLoop) = iConfigLoop Then
                        Me.ConfigSettingsSectionGateway.XMLConfigSettings(iConfigLoop).ConfigSettingValue = m_MagentoAPIVersion(iLoop).ToString ' TJS 11/01/14
                    End If
                Next
            End If
        Next
        ' end of code moved from GetMagentoVersion and amended TJS 15/11/13

    End Sub
#End Region
#End Region

#Region " Events "
#Region " ConfigSettingsEdit "
    Private Sub TextEditConfigSettingsCode_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditSourceCode.Enter
        If TextEditSourceCode.Text = Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
            TextEditSourceCode.Text = String.Empty
        End If
    End Sub

    Private Sub TextEditConfigSettingsCode_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditSourceCode.Leave
        If TextEditSourceCode.Text.Trim.Length = 0 Then
            TextEditSourceCode.Text = Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE
        End If
    End Sub
#End Region

#Region " SourceCodeChanged "
    Private Sub SourceCodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditSourceCode.TextChanged

        If Me.m_ImportExportDataset.SystemSource.Count > 0 Then
            Me.m_ImportExportDataset.SystemSource(0).SourceCode = TextEditSourceCode.Text
        End If

    End Sub
#End Region

#Region " SourceNameChanged "
    Private Sub SourceNameChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditSourceName.TextChanged

        If Me.m_ImportExportDataset.SystemSource.Count > 0 Then
            Me.m_ImportExportDataset.SystemSource(0).SourceDescription = TextEditSourceName.Text
        End If

    End Sub
#End Region

#Region " ConfigSettingsValueChanged "
    Private Sub GridViewConfigSettings_CellValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridViewConfigSettings.CellValueChanging
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/07/09 | TJS             | 2009.3.01 | Function added
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 19/09/13 | TJS             | 2013.3.00 | Modified for generic ImportMissingItemsAsNonStock
        '------------------------------------------------------------------------------------------

        Dim strWarning As String

        ' has Config Setting been uppdated ?
        If e.Column.FieldName = "ConfigSettingValue" Then
            ' is it the AcceptSourceSalesTaxCalculation flag ?
            If Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "AcceptSourceSalesTaxCalculation" And _
                Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString = "General" Then
                ' yes, has flag been set to Yes ?
                If e.Value.ToString.ToUpper = "YES" Then
                    strWarning = "Setting the AcceptSourceSalesTaxCalculation flag to Yes means that" & vbCrLf & _
                        IS_PRODUCT_NAME & " will not perfom any Tax calculations and that the " & vbCrLf & _
                        "source Tax values provided in the import XML will be used instead." & vbCrLf & vbCrLf & _
                        "Neither Lerryn nor Interprise Solutions can accept any responsibility" & vbCrLf & _
                        "for the accuracy of the Tax values which are provided in the import XML." ' TJs 24/08/12
                    Interprise.Presentation.Base.Message.MessageWindow.Show(strWarning, "Tax Calculation Override", Interprise.Framework.Base.Shared.MessageWindowButtons.Close)
                End If

                ' start of code added TJS 13/9/13
            ElseIf Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "ImportMissingItemsAsNonStock" And _
                Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString = "General" Then
                ' yes, has flag been set to Yes ?
                If e.Value.ToString.ToUpper = "YES" Then
                    strWarning = "Setting the ImportMissingItemsAsNonStock flag to Yes means that" & vbCrLf & _
                        "an Order or Invoice will not be rejected if it contains an Inventory Item" & vbCrLf & _
                        "that is not found in the database." & vbCrLf & vbCrLf & _
                        "Instead " & PRODUCT_NAME & " will import it using the Non-Stock Inventory Item" & vbCrLf & _
                        "with the Description and Price details from the Order or Invoice." & vbCrLf & vbCrLf & _
                        "Whilst this will allow you to process the Order or Invoice, there will be" & vbCrLf & _
                        "no inventory stock management for such items and there may also be " & vbCrLf & _
                        "implications for your General Ledger accounts as there will be no" & vbCrLf & _
                        "Cost of Sales for these items." & vbCrLf & vbCrLf & _
                        "In the event that Lerryn or Interprise Solutions are requested to " & vbCrLf & _
                        "assist with resolving such issues, all such work would be chargeable."
                    Interprise.Presentation.Base.Message.MessageWindow.Show(strWarning, "Import Missing Inventory Items", Interprise.Framework.Base.Shared.MessageWindowButtons.Close)
                End If
                ' end of code added TJS 13/9/13

            End If
        End If

    End Sub
#End Region

#Region " ConfigSettingsCustomDrawCell "
    Private Sub ConfigSettingsCustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridViewConfigSettings.CustomDrawCell
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 16/01/12 | TJS             | 2010.2.02 | Modified for Sears.com settings
        ' 16/06/12 | TJS             | 2012.1.07 | Modified to cater for different Amazon developerids in different countries
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected detection of Amazon OwnSecretAccessKey row
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '------------------------------------------------------------------------------------------

        Try
            ' are we drawing the Config Setting Value column ?
            If e.Column.FieldName = "ConfigSettingValue" And e.RowHandle >= 0 Then
                ' yes, is it a Password field ?
                If (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "FTPUploadPassword" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 10) = "ShopDotCom") Or _
                    (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "OwnDeveloperPassword" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 14) = "ChannelAdvisor") Or _
                    (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "APIPwd" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 7) = "Magento") Or _
                    (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "APIPwd" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 13) = "ASPStoreFront") Or _
                    (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "APIPwd" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 11) = "SearsDotCom") Or _
                    (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "OwnAccessKeyID" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 6) = "Amazon") Or _
                    (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "OwnSecretAccessKey" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 6) = "Amazon") Or _
                    (Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigSettingName").ToString = "UserKey" And _
                     Strings.Left(Me.GridViewConfigSettings.GetRowCellValue(e.RowHandle, "ConfigGroup").ToString, 10) = "ThreeDCart") Then ' TJS 16/01/12 TJS 05/07/12 TJS 20/11/13
                    ' yes, mask text
                    e.DisplayText = New String(CChar("*"), Len(e.CellValue))
                End If
            End If

        Catch ex As Exception
            ' ignore error as we can't do anything

        End Try

    End Sub
#End Region

#Region " ConfigSettingsRowSelected "
    Private Sub ConfigSettingsRowSelected(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewConfigSettings.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/07/09 | TJS             | 2009.3.00 | Modified to select relevant editor for current config row
        ' 15/08/09 | TJS             | 2009.3.03 | Modified to allow editing of Setting Name only on new rows 
        '                                        | and added AllowShippingLastNameBlank to Volusion
        ' 10/12/09 | TJS             | 2009.3.09 | Modified to support direct connection to Amazon and tax inclusive prices at Amazon 
        '                                        | plus Channel Advisor connector
        ' 30/12/09 | TJS             | 2010.0.00 | Added Amazon and Channel Advisor Payment Type
        ' 13/01/10 | TJS             | 2010.0.04 | Added AllowBlankPostalcode setting
        ' 19/08/10 | TJS             | 2010.1.00 | Added Magento and ASLDotNetStoreFront connectors, AccountDisaable etc
        ' 22/09/10 | TJS             | 2010.1.01 | Added Channel Advisor EnablePaymentTypeTranslation
        '                                        | plus EnablePollForOrders from Magento and ASPStorefront
        ' 26/10/11 | TJS             | 2011.1.xx | Modified for TaxCodeForSourceTax
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for eBay settings
        ' 16/01/12 | TJS             | 2010.2.02 | Modified for Sears.com settings
        ' 10/06/12 | TJS             | 2012.1.05 | Modified for Magento EnablePaymentTypeTranslation
        ' 05/07/12 | TJS             | 2012.1.08 | Modified for Amazon FBA Merchant ID
        ' 08/07/12 | TJS             | 2012.1.09 | Added UseShipToClassTemplate 
        ' 18/01/13 | TJS             | 2012.1.17 | Modified to cater for AllocateAndReserveStock
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 22/04/13 | TJS             | 2013.1.11 | Added Magento InhibitInventoryUpdates and CreateCustomerForGuestCheckout
        ' 08/05/13 | TJS             | 2013.1.13 | Added Magento IncludeChildItemsOnOrder
        ' 28/07/13 | TJS             | 2013.1.31 | Modified to ensure ExternalSystemCardPaymentCode only displays active 
        '                                        | payment types and excludes Web Checkout payment methods 
        ' 30/07/13 | TJS             | 2013.1.32 | Added eBay PricesAreTaxInclusive and TaxCodeForSourceTax, plus
        '                                        | ASPStoreFront ExtensionDataField1Mapping to ExtensionDataField5Mapping
        ' 19/09/13 | TJS             | 2013.3.00 | Added generic ImportMissingItemsAsNonStock
        ' 02/10/13 | TJS             | 2013.3.03 | Modified for Magento Version
        ' 13/11/13 | TJS             | 2013.3.08 | Added Magento V2SoapAPIWSICompliant, LerrynAPIVersion and UpdateMagentoSpecialPricing
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        ' 15/01/14 | TJS             | 2013.4.05 | Added Volusion EnableSKUAliasLookup
        ' 11/02/14 | TJS             | 2013.4.09 | added Volusion EnablePaymentTypeTranslation
        ' 01/05/14 | TJS             | 2014.0.02 | Added CardAuthAndCaptureWithOrder, ImportAllOrdersAsSingleCustomer and OverrideMagentoPricesWith
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow ' TJS 07/07/09
        Dim XMLMerchantIDs As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 05/07/12
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLMerchantID As XElement ' TJS 05/07/12
        Dim strConfigSettings As String ' TJS 05/07/12

        ' start of code added TJS 07/07/09
        ' are we on a valid row ?
        If Me.GridViewConfigSettings.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            Me.GridViewConfigSettings.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And _
            Me.GridViewConfigSettings.FocusedRowHandle >= 0 Then
            ' yes, select editor
            Me.colConfigSettingValue.OptionsColumn.ReadOnly = False ' TJS 02/12/11
            Me.colConfigSettingValue.OptionsColumn.AllowEdit = True ' TJS 02/12/11
            If Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString = "General" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "SendCodeErrorEmailsToLerryn", "SendSourceErrorEmailsToLerryn", "CreateCustomerAsCompany", _
                        "EnableDeliveryMethodTranslation", "AuthoriseCreditCardOnImport", "EnableCoupons", _
                        "RequireSourceCustomerID", "SetDisableFreightCalculation", "IgnoreVoidedOrdersAndInvoices", _
                        "AcceptSourceSalesTaxCalculation", "PollGenericImportPath", "EnableLogFile", _
                        "AllowBlankPostalcode", "UseShipToClassTemplate", "AllocateAndReserveStock", _
                        "ImportMissingItemsAsNonStock" ' TJS 13/01/10 TJS 08/07/12 TJS 18/01/13 TJS 19/09/13
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "CustomerBusinessType"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeCustBizTypeEdit

                    Case "ShippingModuleToUse"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeShipModuleEdit

                    Case "CustomerBusinessClass"
                        Me.rbeISHypLinkEdit.AdditionalFilter = Nothing
                        Me.rbeISHypLinkEdit.DisplayField = "ClassDescription"
                        Me.rbeISHypLinkEdit.TableName = "CustomerClassTemplateDetailView"
                        Me.rbeISHypLinkEdit.ValueMember = "ClassDescription"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "DefaultShippingMethodGroup"
                        rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "UseShipToClassTemplate") ' TJS 08/07/12
                        If rowConfigSettings Is Nothing OrElse rowConfigSettings.ConfigSettingValue.ToUpper <> "YES" Then ' TJS 08/07/12
                            Me.rbeISHypLinkEdit.AdditionalFilter = Nothing
                            Me.rbeISHypLinkEdit.DisplayField = "ShippingMethodGroup"
                            Me.rbeISHypLinkEdit.TableName = "SystemShippingMethodGroup"
                            Me.rbeISHypLinkEdit.ValueMember = "ShippingMethodGroup"
                            Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit
                        Else
                            Me.colConfigSettingValue.ColumnEdit = Nothing ' TJS 08/07/12
                            Me.colConfigSettingValue.OptionsColumn.ReadOnly = True ' TJS 08/07/12
                            Me.colConfigSettingValue.OptionsColumn.AllowEdit = False ' TJS 08/07/12
                        End If

                    Case "DefaultShippingMethod"
                        rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "UseShipToClassTemplate") ' TJS 08/07/12
                        If rowConfigSettings Is Nothing OrElse rowConfigSettings.ConfigSettingValue.ToUpper <> "YES" Then ' TJS 08/07/12
                            rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultShippingMethodGroup")
                            Me.rbeISHypLinkEdit.AdditionalFilter = " and ShippingMethodGroup = '" & rowConfigSettings.ConfigSettingValue & "'"
                            Me.rbeISHypLinkEdit.DisplayField = "ShippingMethodDescription"
                            Me.rbeISHypLinkEdit.TableName = "SystemShippingMethodGroupDetailView"
                            Me.rbeISHypLinkEdit.ValueMember = "ShippingMethodCode"
                            Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit
                        Else
                            Me.colConfigSettingValue.ColumnEdit = Nothing ' TJS 08/07/12
                            Me.colConfigSettingValue.OptionsColumn.ReadOnly = True ' TJS 08/07/12
                            Me.colConfigSettingValue.OptionsColumn.AllowEdit = False ' TJS 08/07/12
                        End If

                    Case "DefaultWarehouse"
                        rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "UseShipToClassTemplate") ' TJS 08/07/12
                        If rowConfigSettings Is Nothing OrElse rowConfigSettings.ConfigSettingValue.ToUpper <> "YES" Then ' TJS 08/07/12
                            Me.rbeISHypLinkEdit.AdditionalFilter = Nothing
                            Me.rbeISHypLinkEdit.DisplayField = "WarehouseDescription"
                            Me.rbeISHypLinkEdit.TableName = "InventoryWarehouseView"
                            Me.rbeISHypLinkEdit.ValueMember = "WarehouseCode"
                            Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit
                        Else
                            Me.colConfigSettingValue.ColumnEdit = Nothing ' TJS 08/07/12
                            Me.colConfigSettingValue.OptionsColumn.ReadOnly = True ' TJS 08/07/12
                            Me.colConfigSettingValue.OptionsColumn.AllowEdit = False ' TJS 08/07/12
                        End If

                    Case "DefaultPaymentTermGroup"
                        Me.rbeISHypLinkEdit.AdditionalFilter = Nothing
                        Me.rbeISHypLinkEdit.DisplayField = "PaymentTermGroup"
                        Me.rbeISHypLinkEdit.TableName = "SystemPaymentTermGroup"
                        Me.rbeISHypLinkEdit.ValueMember = "PaymentTermGroup"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "CreditCardPaymentTermCode"
                        rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultPaymentTermGroup")
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and PaymentTermGroup = '" & rowConfigSettings.ConfigSettingValue & "'"
                        Me.rbeISHypLinkEdit.DisplayField = "PaymentTermDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemPaymentTermGroupDetailView"
                        Me.rbeISHypLinkEdit.ValueMember = "PaymentTermCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "ExternalSystemCardPaymentCode"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and PaymentMethodCode <> 'Credit Card' and IsActive = 1 and PaymentMethodCode <> 'Web Checkout'" ' TJS 28/07/13
                        Me.rbeISHypLinkEdit.DisplayField = "PaymentTypeDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemPaymentType"
                        Me.rbeISHypLinkEdit.ValueMember = "PaymentTypeCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/06/12

                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 6) = "Amazon" And _
                Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 9) <> "AmazonFBA" Then ' TJS 05/07/12
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "PricesAreTaxInclusive", "AccountDisabled", "EnableSKUAliasLookup" ' TJS 10/12/09 TJS 19/08/10
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit ' TJS 10/12/09

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "AmazonSite" ' TJS 10/12/09
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeAmazonSiteEdit ' TJS 10/12/09

                    Case "PaymentType" ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.AdditionalFilter = Nothing ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.DisplayField = "PaymentTypeCode" ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.TableName = "SystemPaymentType" ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.ValueMember = "PaymentTypeCode" ' TJS 30/12/09
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit ' TJS 30/12/09

                    Case "TaxCodeForSourceTax" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and (TaxType = 'AP' or TaxType Is Null)" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.DisplayField = "TaxCode" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.TableName = "SystemTaxSchemeDetailView" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.ValueMember = "TaxCode" ' TJS 26/10/11
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit ' TJS 26/10/11

                    Case "OwnAccessKeyID", "OwnSecretAccessKey" ' TJS 16/06/12
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit ' TJS 16/06/12
                        Me.rbeTextEdit.PasswordChar = CChar("*") ' TJS 16/06/12

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/06/12

                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 10) = "ShopDotCom" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "PricesAreTaxInclusive", "DisableShopComPublishing", "AccountDisabled" ' TJS 19/08/10
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "SourceItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeShopComSourceIDEdit

                    Case "XMLDateFormat"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeShopComXMLDateEdit

                    Case "Currency"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and IsActive = 1"
                        Me.rbeISHypLinkEdit.DisplayField = "CurrencyDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemCurrency"
                        Me.rbeISHypLinkEdit.ValueMember = "CurrencyCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "TaxCodeForSourceTax" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and (TaxType = 'AP' or TaxType Is Null)" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.DisplayField = "TaxCode" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.TableName = "SystemTaxSchemeDetailView" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.ValueMember = "TaxCode" ' TJS 26/10/11
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit ' TJS 26/10/11

                    Case "FTPUploadPassword" ' TJS 16/01/12
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit ' TJS 16/01/12
                        Me.rbeTextEdit.PasswordChar = CChar("*") ' TJS 16/01/12

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/01/12

                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 8) = "Volusion" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "AllowShippingLastNameBlank", "AccountDisabled", "EnableSKUAliasLookup", _
                        "EnablePaymentTypeTranslation" ' TJS 15/08/09 TJS 19/08/10 TJS 15/01/14 TJS 11/02/14
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "Currency"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and IsActive = 1"
                        Me.rbeISHypLinkEdit.DisplayField = "CurrencyDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemCurrency"
                        Me.rbeISHypLinkEdit.ValueMember = "CurrencyCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/06/12

                End Select

                ' start of code added TJS 10/12/09
            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 14) = "ChannelAdvisor" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "PricesAreTaxInclusive", "AccountDisabled", "EnableSKUAliasLookup", _
                        "EnablePaymentTypeTranslation" ' TJS 19/08/10 TJS 22/09/10
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "Currency"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and IsActive = 1"
                        Me.rbeISHypLinkEdit.DisplayField = "CurrencyDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemCurrency"
                        Me.rbeISHypLinkEdit.ValueMember = "CurrencyCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "PaymentType" ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.AdditionalFilter = Nothing ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.DisplayField = "PaymentTypeCode" ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.TableName = "SystemPaymentType" ' TJS 30/12/09
                        Me.rbeISHypLinkEdit.ValueMember = "PaymentTypeCode" ' TJS 30/12/09
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit ' TJS 30/12/09

                    Case "ActionIfNoPayment" ' TJS 19/08/10
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeChanAdvNoPmtAction ' TJS 19/08/10

                    Case "TaxCodeForSourceTax" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and (TaxType = 'AP' or TaxType Is Null)" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.DisplayField = "TaxCode" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.TableName = "SystemTaxSchemeDetailView" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.ValueMember = "TaxCode" ' TJS 26/10/11
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit ' TJS 26/10/11

                    Case "OwnDeveloperPassword" ' TJS 16/01/12
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit ' TJS 16/01/12
                        Me.rbeTextEdit.PasswordChar = CChar("*") ' TJS 16/01/12

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/01/12

                End Select
                ' end of code added TJS 10/12/09

                ' start of code added TJS 19/08/10
            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 7) = "Magento" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "APISupportsPartialShipments", "AccountDisabled", "AllowShippingLastNameBlank", _
                        "PricesAreTaxInclusive", "EnableSKUAliasLookup", "EnablePaymentTypeTranslation", _
                        "InhibitInventoryUpdates", "CreateCustomerForGuestCheckout", "IncludeChildItemsOnOrder", _
                        "V2SoapAPIWSICompliant", "UpdateMagentoSpecialPricing", "CardAuthAndCaptureWithOrder" ' TJS 07/01/11 TJS 10/06/12 TJS 22/04/13 TJS 08/05/13 TJS 13/11/13 TJS 01/05/14
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "MagentoVersion", "LerrynAPIVersion" ' TJS 02/10/13 TJS 30/101/3
                        Me.colConfigSettingValue.OptionsColumn.ReadOnly = True ' TJS 02/10/13
                        Me.colConfigSettingValue.OptionsColumn.AllowEdit = False ' TJS 02/10/13
                        Me.colConfigSettingValue.ColumnEdit = rbeTextEdit ' TJS 02/10/13

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "TaxCodeForSourceTax" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and (TaxType = 'AP' or TaxType Is Null)" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.DisplayField = "TaxCode" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.TableName = "SystemTaxSchemeDetailView" ' TJS 26/10/11
                        Me.rbeISHypLinkEdit.ValueMember = "TaxCode" ' TJS 26/10/11
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit ' TJS 26/10/11

                    Case "APIPwd" ' TJS 16/01/12
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit ' TJS 16/01/12
                        Me.rbeTextEdit.PasswordChar = CChar("*") ' TJS 16/01/12

                        ' start of code added TJS 01/05/14
                    Case "ImportAllOrdersAsSingleCustomer"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and IsProspect = 0"
                        Me.rbeISHypLinkEdit.DisplayField = "CustomerCode"
                        Me.rbeISHypLinkEdit.TableName = "Customer"
                        Me.rbeISHypLinkEdit.ValueMember = "CustomerCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "OverrideMagentoPricesWith"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeOverrideSourcePrice
                        ' end of code added TJS 01/05/14

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Nothing
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/01/12

                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 13) = "ASPStoreFront" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "UseWSE3Authentication", "AllowShippingLastNameBlank", "AccountDisabled", _
                        "EnableSKUAliasLookup" ' TJS 07/01/11
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "Currency"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and IsActive = 1"
                        Me.rbeISHypLinkEdit.DisplayField = "CurrencyDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemCurrency"
                        Me.rbeISHypLinkEdit.ValueMember = "CurrencyCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "APIPwd" ' TJS 16/01/12
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit ' TJS 16/01/12
                        Me.rbeTextEdit.PasswordChar = CChar("*") ' TJS 16/01/12

                        ' start of code added TJS 01/08/13
                    Case "ExtensionDataField1Mapping", "ExtensionDataField2Mapping", "ExtensionDataField3Mapping", _
                        "ExtensionDataField4Mapping", "ExtensionDataField5Mapping"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and TableName = 'InventoryItem' AND IsCustomField = 1"
                        Me.rbeISHypLinkEdit.DisplayField = "ColumnName"
                        Me.rbeISHypLinkEdit.TableName = "DataDictionaryColumn"
                        Me.rbeISHypLinkEdit.ValueMember = "ColumnName"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit
                        ' end of code added TJS 01/08/13

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/01/12

                End Select
                ' end of code added TJS 19/08/10

                ' start of code added TJS 02/12/11
            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 4) = "eBay" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "AllowShippingLastNameBlank", "AccountDisabled", "EnableSKUAliasLookup", _
                        "EnablePaymentTypeTranslation", "PricesAreTaxInclusive" ' TJS 13/11/13
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "AuthToken"
                        Me.colConfigSettingValue.OptionsColumn.ReadOnly = True
                        'Me.colConfigSettingValue.OptionsColumn.AllowEdit = False ' TJS 24/09/13
                        Me.colConfigSettingValue.ColumnEdit = rbeEBayGenAuthToken

                    Case "TokenExpires"
                        Me.colConfigSettingValue.OptionsColumn.ReadOnly = True
                        Me.colConfigSettingValue.OptionsColumn.AllowEdit = False
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "PaymentType"
                        Me.rbeISHypLinkEdit.AdditionalFilter = Nothing
                        Me.rbeISHypLinkEdit.DisplayField = "PaymentTypeCode"
                        Me.rbeISHypLinkEdit.TableName = "SystemPaymentType"
                        Me.rbeISHypLinkEdit.ValueMember = "PaymentTypeCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "Country"
                        If m_EBayDetailsAvailable Then
                            Me.colConfigSettingValue.ColumnEdit = Me.rbeEbayCountryEdit
                        Else
                            Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                            m_EditingEBayCountry = True
                            If Not SetSourceSpecificValues() Then
                                m_EditingEBayCountry = False
                                Interprise.Presentation.Base.Message.MessageWindow.Show("eBay Sites list not available from eBay")
                            End If
                        End If

                    Case "ActionIfNoPayment"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeChanAdvNoPmtAction

                        ' start of code added TJS 13/11/13
                    Case "TaxCodeForSourceTax"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and (TaxType = 'AP' or TaxType Is Null)"
                        Me.rbeISHypLinkEdit.DisplayField = "TaxCode"
                        Me.rbeISHypLinkEdit.TableName = "SystemTaxSchemeDetailView"
                        Me.rbeISHypLinkEdit.ValueMember = "TaxCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit
                        ' end of code added TJS 13/11/13

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing ' TJS 16/06/12

                End Select
                ' end of code added TJS 02/12/11

                ' start of code added TJS 16/01/12
            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 11) = "SearsDotCom" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "AccountDisabled", "PricesAreTaxInclusive", "SearsGeneratesInvoice"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "TaxCodeForSourceTax"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and (TaxType = 'AP' or TaxType Is Null)"
                        Me.rbeISHypLinkEdit.DisplayField = "TaxCode"
                        Me.rbeISHypLinkEdit.TableName = "SystemTaxSchemeDetailView"
                        Me.rbeISHypLinkEdit.ValueMember = "TaxCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "Currency"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and IsActive = 1"
                        Me.rbeISHypLinkEdit.DisplayField = "CurrencyDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemCurrency"
                        Me.rbeISHypLinkEdit.ValueMember = "CurrencyCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "PaymentType"
                        Me.rbeISHypLinkEdit.AdditionalFilter = Nothing
                        Me.rbeISHypLinkEdit.DisplayField = "PaymentTypeCode"
                        Me.rbeISHypLinkEdit.TableName = "SystemPaymentType"
                        Me.rbeISHypLinkEdit.ValueMember = "PaymentTypeCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "APIPwd"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing

                End Select
                ' end of code added TJS 16/01/12

                ' start of code added TJS 05/07/12
            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 9) = "AmazonFBA" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "AccountDisabled"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "AmazonSite"
                        Me.colConfigSettingValue.OptionsColumn.AllowEdit = False
                        Me.colConfigSettingValue.OptionsColumn.ReadOnly = True

                    Case "MerchantToken" ' TJS 22/03/13
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeAmazonMerchantIDEdit
                        ' get Amazon Merchant IDs
                        strConfigSettings = Me.m_ConfigSettingsSectionFacade.GetField("ConfigSettings_DEV000221", "LerrynImportExportConfig_DEV000221", "SourceCode_DEV000221 = '" & AMAZON_SOURCE_CODE & "'")
                        Try
                            XMLConfig = XDocument.Parse(strConfigSettings)

                        Catch ex As Exception
                            Interprise.Presentation.Base.Message.MessageWindow.Show("Failed to read Amazon XML config settings, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & strConfigSettings)
                            Return
                        End Try
                        rbeAmazonMerchantIDEdit.Items.BeginUpdate()
                        rbeAmazonMerchantIDEdit.Items.Clear()
                        XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
                        For Each XMLMerchantID In XMLMerchantIDs
                            Try
                                XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                            Catch ex As Exception
                                Interprise.Presentation.Base.Message.MessageWindow.Show("Failed to load Amazon settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString)
                                Return

                            End Try
                            If Not String.IsNullOrEmpty(Me.m_ConfigSettingsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN)) And _
                                UCase(Me.m_ConfigSettingsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_ACCOUNT_DISABLED)) <> "YES" Then ' TJS 22/03/13
                                rbeAmazonMerchantIDEdit.Items.Add(Me.m_ConfigSettingsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN)) ' TJS 22/03/13
                            End If
                        Next
                        rbeAmazonMerchantIDEdit.Items.EndUpdate()

                End Select
                ' end of code added TJS 05/07/12

                ' start of code added TJS 20/11/13
            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 10) = "ThreeDCart" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "AllowShippingLastNameBlank", "EnablePaymentTypeTranslation", "EnableSKUAliasLookup", "AccountDisabled"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeYesNoEdit

                    Case "ISItemIDField"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISItemIDEdit

                    Case "Currency"
                        Me.rbeISHypLinkEdit.AdditionalFilter = " and IsActive = 1"
                        Me.rbeISHypLinkEdit.DisplayField = "CurrencyDescription"
                        Me.rbeISHypLinkEdit.TableName = "SystemCurrency"
                        Me.rbeISHypLinkEdit.ValueMember = "CurrencyCode"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeISHypLinkEdit

                    Case "UserKey"
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = CChar("*")

                    Case Else
                        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
                        Me.rbeTextEdit.PasswordChar = Nothing

                End Select
                ' end of code added TJS 20/11/13
            End If
            Me.colConfigSettingName.OptionsColumn.AllowEdit = False ' TJS 15/08/09
            Me.colConfigSettingName.OptionsColumn.ReadOnly = True ' TJS 15/08/09

        ElseIf Me.GridViewConfigSettings.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle Then ' TJS 15/08/09
            Me.colConfigSettingValue.ColumnEdit = Nothing ' TJS 15/08/09
            Me.colConfigSettingName.OptionsColumn.AllowEdit = True ' TJS 15/08/09
            Me.colConfigSettingName.OptionsColumn.ReadOnly = False ' TJS 15/08/09

        Else
            Me.colConfigSettingValue.ColumnEdit = Nothing
            Me.colConfigSettingName.OptionsColumn.AllowEdit = False ' TJS 15/08/09
            Me.colConfigSettingName.OptionsColumn.ReadOnly = True ' TJS 15/08/09
        End If
        ' end of code added TJS 07/07/09

        If e.FocusedRowHandle >= 0 Then
            m_strCurrentConfigGroup = GridViewConfigSettings.GetRowCellValue(e.FocusedRowHandle, Me.m_ImportExportDataset.XMLConfigSettings.ConfigGroupColumn.ToString).ToString
        End If

    End Sub
#End Region

#Region " YesNoSelected "
    Private Sub YesNoSelected(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles rbeYesNoEdit.CloseUp
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/07/12 | TJS             | 2012.1.09 | Function added to cater for UseShipToClassTemplate
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowConfigSettings As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow

        If Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString = "General" And _
            Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "UseShipToClassTemplate" And e.Value.ToString.ToUpper = "YES" Then
            rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultShippingMethod")
            rowConfigSettings.ConfigSettingValue = ""
            rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultShippingMethodGroup")
            rowConfigSettings.ConfigSettingValue = ""
            rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultWarehouse")
            rowConfigSettings.ConfigSettingValue = ""
        End If

    End Sub
#End Region

#Region " EBayGetAuthToken "
    Private Sub EBayGetAuthToken(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles rbeEBayGenAuthToken.ButtonClick
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 03/07/13 | FA              | 2013.1.23 | Modified to cater for multiple ebay instances
        ' 08/07/13 | TJS             | 2013.1.27 | Corrected setting of TokenExpires on multiple instances 
        ' 09/08/13 | FA              | 2013.2.02 | Modified change AuthToken conditional statement to cater 
        '                                          for if AuthToken created for first time
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim eBayConnection As Lerryn.Facade.ImportExport.eBayXMLConnector
        Dim strEBaySessionID As String, strEBayAuthURL As String, strEBayRuName As String
        Dim strAuthToken As String, strTokenExpires As String, iLoop As Integer
        Dim bUseEBaySandbox As Boolean
        Dim strEbayGroup As String = "" 'FA 03/07/13

        Me.colConfigSettingValue.OptionsColumn.AllowEdit = False
        strEbayGroup = Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString ' FA 09/08/13
        If Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString = "eBay" OrElse _
            (Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString.Length > 6 AndAlso _
            Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString.Substring(0, 6) = "eBay :") And _
            Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString <> "" Then ' FA 03/07/13
            If Interprise.Presentation.Base.Message.MessageWindow.Show("Are you sure you want to replace the existing Auth Token", "Replace Auth Token", _
                Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, _
                Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.No Then ' FA 03/07/13 FA 09/08/13 changed back from Yes from No
                Me.colConfigSettingValue.OptionsColumn.AllowEdit = True
                Return
            End If
        End If
        CheckEBayLoginSettings()
        bUseEBaySandbox = (Me.m_ConfigSettingsSectionFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "UseEBaySandbox", "NO").ToUpper = "YES")
        strEBaySessionID = ""
        If Not m_EBaySettings.AccountDisabled Then
            eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(Me.m_ConfigSettingsSectionFacade, m_EBaySettings, bUseEBaySandbox)
            If bUseEBaySandbox Then
                strEBayRuName = EBAY_SANDBOX_ESHOPCONNECT_RUNAME
            Else
                strEBayRuName = EBAY_ESHOPCONNECT_RUNAME
            End If
            If eBayConnection.GetSessionID(strEBayRuName, strEBaySessionID) Then
                If bUseEBaySandbox Then
                    strEBayAuthURL = "https://signin.sandbox.ebay.com/ws/eBayISAPI.dll?SignIn&RuName=" & strEBayRuName & "&SessID=" & strEBaySessionID
                Else
                    strEBayAuthURL = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&RuName=" & strEBayRuName & "&SessID=" & strEBaySessionID
                End If
                System.Diagnostics.Process.Start(strEBayAuthURL)
                If Interprise.Presentation.Base.Message.MessageWindow.Show("Did you successfully sign in to eBay and authorise" & vbCrLf & "eShopCONNECTED for " & IS_PRODUCT_NAME & " to access your eBay account ?" & _
                    vbCrLf & vbCrLf & "If so, click Yes and eShopCONNECTED will retrieve" & vbCrLf & "your new Auth Token fron eBay.", "New Auth Token", _
                    Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, _
                    Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Yes Then ' TJS 24/08/12
                    strAuthToken = ""
                    strTokenExpires = ""
                    If eBayConnection.FetchToken(strEBaySessionID, strAuthToken, strTokenExpires) Then
                        Me.GridViewConfigSettings.SetFocusedRowCellValue("ConfigSettingValue", strAuthToken)
                        For iLoop = 0 To Me.GridViewConfigSettings.RowCount - 1
                            If Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigGroup").ToString = strEbayGroup And _
                                    Me.GridViewConfigSettings.GetRowCellValue(iLoop, "ConfigSettingName").ToString = "TokenExpires" Then ' FA 03/07/13 TJS 08/07/13
                                Me.GridViewConfigSettings.SetRowCellValue(iLoop, "ConfigSettingValue", strTokenExpires)
                                Exit For
                            End If
                        Next
                    End If
                End If
                Me.colConfigSettingValue.OptionsColumn.AllowEdit = True
            End If
        End If

    End Sub
#End Region

#Region " EBayCountrySelected "
    Private Sub EBayCountrySelected(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles rbeEbayCountryEdit.CloseUp
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        For iLoop = 0 To m_EBaySites.Length - 1
            If e.Value.ToString = m_EBaySites(iLoop).SiteName Then
                If m_EBaySites(iLoop).SiteID.ToString <> Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingID").ToString Then
                    Me.GridViewConfigSettings.SetFocusedRowCellValue("ConfigSettingID", m_EBaySites(iLoop).SiteID)
                End If
                Exit For
            End If
        Next

    End Sub
#End Region

#Region " AmazonFBAMerchantIDSelected "
    Private Sub AmazonFBAMerchantIDSelected(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles rbeAmazonMerchantIDEdit.CloseUp
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowConfigSettings As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim XMLMerchantIDs As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim strConfigSettings As String

        ' get Amazon Merchant IDs
        strConfigSettings = Me.m_ConfigSettingsSectionFacade.GetField("ConfigSettings_DEV000221", "LerrynImportExportConfig_DEV000221", "SourceCode_DEV000221 = '" & AMAZON_SOURCE_CODE & "'")
        Try
            XMLConfig = XDocument.Parse(strConfigSettings)

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show("Failed to read Amazon XML config settings, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & strConfigSettings)
            Return
        End Try
        XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
        For Each XMLMerchantID In XMLMerchantIDs
            Try
                XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

            Catch ex As Exception
                Interprise.Presentation.Base.Message.MessageWindow.Show("Failed to load Amazon settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString)
                Return

            End Try

            If Not String.IsNullOrEmpty(Me.m_ConfigSettingsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN)) And _
                UCase(Me.m_ConfigSettingsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_ACCOUNT_DISABLED)) <> "YES" And _
                Me.m_ConfigSettingsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN) = e.Value.ToString Then ' TJS 22/03/13
                rowConfigSettings = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, "AmazonSite")
                rowConfigSettings.ConfigSettingValue = Me.m_ConfigSettingsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_SITE)
                Exit For
            End If
        Next

    End Sub
#End Region

#Region " rbeISHypLinkEdit_OpenLink "
    Private Sub rbeISHypLinkEdit_OpenLink(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles rbeISHypLinkEdit.OpenLink
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/07/09 | TJS             | 2009.1.08 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for ASPDotNetStroefront and eBay
        '------------------------------------------------------------------------------------------

        Dim param(0) As String

        ' are we on a valid row ?
        If Me.GridViewConfigSettings.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            Me.GridViewConfigSettings.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And _
            Me.GridViewConfigSettings.FocusedRowHandle >= 0 Then
            ' yes, select link
            If Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString = "General" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "CustomerBusinessClass"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuCustomer.CustomerUtilitiesSetupCustomerClassTemplate.ToString, Nothing, Nothing)

                    Case "DefaultShippingMethodGroup"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyShippingMethodGroup.ToString, Nothing, Nothing)

                    Case "DefaultShippingMethod"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyShippingMethod.ToString, Nothing, Nothing)

                    Case "DefaultWarehouse"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuInventory.InventoryFindWarehouse.ToString, Nothing, Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString)

                    Case "DefaultPaymentTermGroup"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyPaymentTermGroup.ToString, Nothing, Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString)

                    Case "CreditCardPaymentTermCode"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyPaymentTerm.ToString, Nothing, Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString)

                    Case "ExternalSystemCardPaymentCode"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyPaymentType.ToString, Nothing, Nothing)

                    Case Else
                        ' no link to open

                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 6) = "Amazon" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString

                    Case Else
                        ' no link to open

                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 10) = "ShopDotCom" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "Currency"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyMultiCurrency.ToString, Nothing, Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString)

                    Case Else
                        ' no link to open
                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 8) = "Volusion" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "Currency"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyMultiCurrency.ToString, Nothing, Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString)

                    Case Else
                        ' no link to open
                End Select

                ' start of code added TJS 02/12/11
            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 8) = "ASPStoreFront" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "Currency"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyMultiCurrency.ToString, Nothing, Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString)

                    Case Else
                        ' no link to open
                End Select

            ElseIf Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 8) = "eBay" Then
                Select Case Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString
                    Case "Currency"
                        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyMultiCurrency.ToString, Nothing, Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString)

                    Case Else
                        ' no link to open
                End Select
                ' end of code added TJS 02/12/11

            End If
        Else
            Me.colConfigSettingValue.ColumnEdit = Nothing
        End If
        ' prevent IS trying to open non-existent link
        e.Handled = True

    End Sub
#End Region

#Region " NewConfigSetting "
    Private Sub NewConfigSetting(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridViewConfigSettings.InitNewRow

        With Me.ConfigSettingsSectionGateway
            GridViewConfigSettings.SetRowCellValue(GridViewConfigSettings.FocusedRowHandle, .XMLConfigSettings.ConfigGroupColumn.ColumnName, m_strCurrentConfigGroup)
        End With

    End Sub
#End Region

#Region " ChangeSourcePassword "
    Private Sub ChangeSourcePassword(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePwd.Click

        Dim passwordForm As Interprise.Presentation.Component.CRM.Contact.Password.PasswordForm = Nothing
        Dim password As String, passwordSalt As String, passwordVector As String

        Try
            With Me.ConfigSettingsSectionGateway
                If .LerrynImportExportConfig_DEV000221.Count > 0 Then
                    Dim currentRow As DataRow = .LerrynImportExportConfig_DEV000221(0)
                    Dim emailAddress As String = Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserEmailAddress

                    If .LerrynImportExportConfig_DEV000221(0).IsSourcePassword_DEV000221Null Then
                        password = ""
                    Else
                        password = .LerrynImportExportConfig_DEV000221(0).SourcePassword_DEV000221
                    End If
                    If .LerrynImportExportConfig_DEV000221(0).IsSourcePasswordSalt_DEV000221Null Then
                        passwordSalt = ""
                    Else
                        passwordSalt = .LerrynImportExportConfig_DEV000221(0).SourcePasswordSalt_DEV000221
                    End If
                    If .LerrynImportExportConfig_DEV000221(0).IsSourcePassword_DEV000221Null Then
                        passwordVector = ""
                    Else
                        passwordVector = .LerrynImportExportConfig_DEV000221(0).SourcePasswordIV_DEV000221
                    End If
                    Try
                        passwordForm = New Interprise.Presentation.Component.CRM.Contact.Password.PasswordForm(password, passwordVector, passwordSalt)

                    Catch ex As Exception
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Please add the crm in your user role in order to use this password.", , , Interprise.Framework.Base.Shared.Enum.MessageWindowIcon.Error)
                        Return

                    End Try
                    Dim forgotPasswordInfo As New Interprise.Framework.Base.Shared.Structure.ForgotPasswordInfo
                    forgotPasswordInfo.EmailAddress = emailAddress
                    forgotPasswordInfo.EncryptedPassword = password
                    forgotPasswordInfo.EncryptedPasswordSalt = passwordSalt
                    forgotPasswordInfo.EncryptedPasswordIV = passwordVector
                    passwordForm.ForgotPasswordInfo = forgotPasswordInfo
                    If passwordForm.ShowDialog = DialogResult.OK Then
                        If passwordForm.Password <> "" Then
                            .LerrynImportExportConfig_DEV000221(0).BeginEdit()
                            .LerrynImportExportConfig_DEV000221(0).SourcePassword_DEV000221 = passwordForm.Password
                            .LerrynImportExportConfig_DEV000221(0).SourcePasswordSalt_DEV000221 = passwordForm.PasswordSalt
                            .LerrynImportExportConfig_DEV000221(0).SourcePasswordIV_DEV000221 = passwordForm.PasswordVector
                            .LerrynImportExportConfig_DEV000221(0).EndEdit()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            If passwordForm IsNot Nothing AndAlso Not passwordForm.IsDisposed Then passwordForm.Dispose()
        End Try
    End Sub
#End Region

#Region " ActivationCodeTextChanged "
    Private Sub TextEditInitialActivation1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditInitialActivation1.TextChanged

        Me.TextEditInitialActivation1.Text = Me.TextEditInitialActivation1.Text.ToUpper

    End Sub
#End Region

#Region " TextEditCustomDisplayText "
    Private Sub TextEditCustomDisplayText(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs) Handles rbeTextEdit.CustomDisplayText

        Try
            If ((Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "FTPUploadPassword" And _
                 Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 10) = "ShopDotCom") Or _
                (Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "OwnDeveloperPassword" And _
                 Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 14) = "ChannelAdvisor") Or _
                (Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "APIPwd" And _
                 Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 7) = "Magento") Or _
                (Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "APIPwd" And _
                 Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 13) = "ASPStoreFront") Or _
                (Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "APIPwd" And _
                 Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 11) = "SearsDotCom") Or _
                (Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "OwnAccessKeyID" And _
                 Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 6) = "Amazon") Or _
                (Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingName").ToString = "OwnDeveloperPassword" And _
                 Strings.Left(Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigGroup").ToString, 6) = "OwnSecretAccessKey")) And _
                e.Value.ToString = Me.GridViewConfigSettings.GetFocusedRowCellValue("ConfigSettingValue").ToString Then
                ' yes, mask text
                e.DisplayText = New String(CChar("*"), Len(e.Value))
            Else
                e.DisplayText = e.Value.ToString
            End If

        Catch ex As Exception
            ' ignore error as we can't do anything

        End Try

    End Sub

#End Region
#End Region

End Class
#End Region
