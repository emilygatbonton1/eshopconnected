' eShopCONNECT for Connected Business
' Module: PropertyHelpSection.vb
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
' Updated 10 June 2012

Option Explicit On
Option Strict On

#Region " PropertyHelpSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.PropertyHelpSection")> _
Public Class PropertyHelpSection

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.AmazonPropertyHelpSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_InventorySettingsFacade
        End Get
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
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Me.m_InventorySettingsDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        Me.m_InventorySettingsFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_InventorySettingsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
    End Sub

    Public Sub New(ByVal InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
             ByVal InventorySettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)

        MyBase.New()

        Me.m_InventorySettingsDataset = InventorySettingsDataset
        Me.m_InventorySettingsFacade = InventorySettingsSectionFacade

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return
    End Sub
#End Region

#Region " InitializeControl "
    Protected Overrides Sub InitializeControl()
        'This call is required by the Presentation Layer.
        MyBase.InitializeControl()

        'Add any initialization after the InitializeControl() call

    End Sub
#End Region

#Region " DisplayTagHelp "
    Public Sub DisplayTagHelp(ByVal TagDescription As String, ByVal TagAcceptedValues As String, ByRef TagExample As String, ByVal TagStatus As String, ByVal TagConditionality As String)

        Me.MemoEditDescription.Text = TagDescription
        Me.MemoEditAcceptedValues.Text = TagAcceptedValues
        Me.MemoEditExample.Text = TagExample
        Me.TextEditStatus.Text = TagStatus
        Me.MemoEditConditionality.Text = TagConditionality

    End Sub
#End Region

#Region " ShowHelpTabs "
    Public Sub ShowHelpTabs(ByVal ShowDescriptionTag As Boolean, ByVal ShowAcceptedValuesTab As Boolean, _
        ByRef ShowTagExampleTab As Boolean, ByVal ShowTagStatusTab As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.XtraTabPageDescription.PageVisible = ShowDescriptionTag
        Me.XtraTabPageAcceptedValues.PageVisible = ShowAcceptedValuesTab
        Me.XtraTabPageExample.PageVisible = ShowTagExampleTab
        Me.XtraTabPageStatus.PageVisible = ShowTagStatusTab

    End Sub
#End Region
#End Region

End Class
#End Region
