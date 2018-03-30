' eShopCONNECT for Connected Business
' Module: SelectDescriptionSourceSection.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
'
'       © 2012 - 2013  Lerryn Business Solutions Ltd
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
' eShopCONNECT is a Trademark of Lerryn Business Solutions Ltd
'-------------------------------------------------------------------
'
' Updated 09 December 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Microsoft.VisualBasic

#Region " SelectDescriptionSourceSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceSection")> _
Public Class SelectDescriptionSourceSection
	
#Region " Variables "
    Private m_SelectDescriptionSourceDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_SelectDescriptionSourceSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_PublishedInstanceCount As Integer = 0
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.SelectDescriptionSourceSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_SelectDescriptionSourceSectionFacade

        End Get
    End Property
#End Region

#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New()
        MyBase.New()

        Me.m_SelectDescriptionSourceDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        ' To solve this error, you must use any facade other than the two:
        Me.m_SelectDescriptionSourceSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.m_SelectDescriptionSourceDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

    End Sub

    Public Sub New(ByVal SelectDescriptionSourceDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
       ByVal SelectDescriptionSourceSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
        MyBase.New()

        Me.m_SelectDescriptionSourceDataset = SelectDescriptionSourceDataset
        Me.m_SelectDescriptionSourceSectionFacade = SelectDescriptionSourceSectionFacade


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

#Region " SetSource1Options "
    Friend Sub SetSource1Options(ByVal OptionDescriptions As String())
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iOptionPtr As Integer

        iOptionPtr = 0
        For Each OptionDescription In OptionDescriptions
            Me.rgSelectSource1.Properties.Items(iOptionPtr).Description = OptionDescription
            iOptionPtr += 1
        Next

    End Sub
#End Region

#Region " SetSource2Options "
    Friend Sub SetSource2Options(ByVal OptionDescriptions As String())
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iOptionPtr As Integer

        iOptionPtr = 0
        For Each OptionDescription In OptionDescriptions
            Me.rgSelectSource2.Properties.Items(iOptionPtr).Description = OptionDescription
            iOptionPtr += 1
        Next

    End Sub
#End Region
#End Region

#Region " Events "
#Region " Source1Selected "
    Private Sub Source1Selected(sender As Object, e As System.EventArgs) Handles rgSelectSource1.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to cater for Weight Units
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.rgSelectSource2.EditValue Is Nothing Or Me.cbeWeightUnits.EditValue Is Nothing Then ' TJS 09/12/13
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).OKButtonEnabled = False
        Else
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).OKButtonEnabled = True
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).ButtonOk.Focus()
        End If
    End Sub
#End Region

#Region " Source2Selected "
    Private Sub Source2Selected(sender As Object, e As System.EventArgs) Handles rgSelectSource2.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to cater for Weight Units
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.rgSelectSource1.EditValue Is Nothing Or Me.cbeWeightUnits.EditValue Is Nothing Then ' TJS 09/12/13
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).OKButtonEnabled = False
        Else
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).OKButtonEnabled = True
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).ButtonOk.Focus()
        End If
    End Sub
#End Region

#Region " WeightUnitsSelected "
    Private Sub cbeWeightUnits_EditValueChanged(sender As Object, e As System.EventArgs) Handles cbeWeightUnits.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/12/13 | TJS             | 2013.4.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.rgSelectSource1.EditValue Is Nothing Or Me.rgSelectSource2.EditValue Is Nothing Then
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).OKButtonEnabled = False
        Else
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).OKButtonEnabled = True
            DirectCast(Me.ParentForm, SelectDescriptionSourceForm).ButtonOk.Focus()
        End If

    End Sub
#End Region
#End Region

End Class
#End Region

