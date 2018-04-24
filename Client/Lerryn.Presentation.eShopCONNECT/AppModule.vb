' eShopCONNECT for Connected Business
' Module: AppModule.vb
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
' Updated 19 September 2013

Public Class AppModule
    Inherits Interprise.Presentation.Component.BaseAppModule

#Region " Variables "
    Private m_action As String
    Private m_searchParameters As Interprise.Framework.Base.Shared.Structure.SearchParameters
    Private m_pluginForm As Interprise.Extendable.Base.Presentation.Generic.IBaseFormInterface
    Private Shared WithEvents searchDashboard As Interprise.Presentation.Base.Search.BaseSearchDashboard ' TJS 05/07/12
    Private m_dataRowSelected As DataRow ' TJS 05/07/12
    Private m_datarowsSelected() As DataRow ' TJS 05/07/12
#End Region

#Region " Methods "
    Public Overrides Function Execute(ByVal action As String, ByVal searchParameters As Interprise.Framework.Base.Shared.Structure.SearchParameters, ByVal ParamArray param() As Object) As Object
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Added OpenAmazonSettlement
        ' 19/09/13 | TJS             | 2013.3.00 | Added OpenBulkPublishingForm
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Execute = True
        Me.m_action = action
        Me.m_searchParameters = searchParameters
        Me.m_pluginForm = MyBase.RetrieveFormPlugin(action)

        Select Case action.ToUpper

            Case Lerryn.Framework.ImportExport.Shared.Enum.MenuAction.MenuType.OpenImpExpConfigSettingsForm.ToString.ToUpper
                Me.OpenConfigSettingsForm()
            Case Lerryn.Framework.ImportExport.Shared.Enum.MenuAction.MenuType.OpenImpExpActivationForm.ToString.ToUpper
                Me.OpenActivationForm()
            Case Lerryn.Framework.ImportExport.Shared.Enum.MenuAction.MenuType.OpenImpExpInventoryImportForm.ToString.ToUpper ' TJS 18/03/11
                Me.OpenInventoryImportForm() ' TJS 18/03/11
            Case Lerryn.Framework.ImportExport.Shared.Enum.MenuAction.MenuType.OpenAmazonSettlementForm.ToString.ToUpper ' TJS 05/07/12
                Me.OpenAmazonSettlementForm() ' TJS 05/07/12
            Case Lerryn.Framework.ImportExport.Shared.Enum.MenuAction.MenuType.OpenBulkPublishingForm.ToString.ToUpper ' TJS 19/09/13
                Me.OpenBulkPublishingForm() ' TJS 19/09/13
            Case Else
                Execute = Nothing
        End Select
    End Function

#Region " OpenConfigSettingsForm "
    Private Sub OpenConfigSettingsForm()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/03/11 | TJS             | 2011.0.01 | Modified to display activation status
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim formConfigSettings As New SystemManager.Config.ConfigSettingsForm
        formConfigSettings.Show()
        formConfigSettings.DisplayActivationStatus() ' Added by mark jeson kee 8-7-2015

        'Comment Activation Form by mark kee 8-7-2015
        'If Not formConfigSettings.IsActivated Then
        '    formConfigSettings.Close()
        '    OpenActivationForm()
        'Else
        '    formConfigSettings.DisplayActivationStatus() ' TJS 21/03/11
        'End If
       
    End Sub
#End Region

#Region " OpenActivationForm "
    Private Sub OpenActivationForm()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/02/10 | TJS             | 2010.0.05 | Modified to set focus
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim formActivation As New SystemManager.Activation.ActivationForm
        formActivation.Show()

    End Sub
#End Region

#Region " OpenInventoryImportForm "
    Private Sub OpenInventoryImportForm()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim formImportWizard As New ImportWizardForm

        formImportWizard.Show()

        'Comment Activation Form by mark kee 8-7-2015
        'If Not formImportWizard.IsActivated Then
        '    formImportWizard.Close()
        '    OpenActivationForm()
        'End If
    End Sub
#End Region

#Region " OpenAmazonSettlementForm "
    Private Sub OpenAmazonSettlementForm()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.ShowSearchForm(New String() {"Amazon Settlements to reconcile", "Amazon Settlement History"}, Nothing, Nothing, False, False, Nothing, Nothing, False)
        ' and if a row is selected, 
        If Me.m_dataRowSelected IsNot Nothing Then
            Dim frmAmazonSettlement As New AmazonSettlement.AmazonSettlementForm
            If Not String.IsNullOrEmpty(Me.m_dataRowSelected("SettlementCode_DEV000221").ToString) Then
                frmAmazonSettlement.DocumentCode = Me.m_dataRowSelected("SettlementCode_DEV000221").ToString
            End If
            frmAmazonSettlement.Show()
        End If

    End Sub
#End Region

#Region " OpenBulkPublishingForm "
    Private Sub OpenBulkPublishingForm()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim formBulkPublishWizard As New BulkPublishingWizardForm

        formBulkPublishWizard.Show()

        'Comment Activation Form by mark kee 8-7-2015
        'If Not formBulkPublishWizard.IsActivated Then
        '    formBulkPublishWizard.Close()    
        '    OpenActivationForm()
        'End If
    End Sub
#End Region

#Region " ShowSearchForm "
    Protected Function ShowSearchForm(ByVal tableNames() As String, Optional ByVal groupColumns As String() = Nothing, _
        Optional ByVal additionalFilter As String = "", Optional ByVal showFirstNewButton As Boolean = False, _
        Optional ByVal showSecondNewButton As Boolean = False, Optional ByVal firstButtonText As String = Nothing, _
        Optional ByVal secondButtonText As String = Nothing, Optional ByVal ignoreGrouping As Boolean = False) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    This module 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If m_searchParameters.IsEmpty Then
            m_searchParameters = New Interprise.Framework.Base.Shared.Structure.SearchParameters _
                (Interprise.Framework.Base.Shared.Entity.ECommerce, tableNames, m_action, additionalFilter <> String.Empty, _
                additionalFilter, groupColumns, showFirstNewButton, showSecondNewButton, firstButtonText, secondButtonText, ignoreGrouping)
            searchDashboard = DirectCast(Interprise.Presentation.Base.BaseControl.Host.SwitchDashboard(Interprise.Framework.Base.Shared.Const.DASHBOARD_ECOMMERCE_FIND, _
                Interprise.Framework.Base.Shared.DashboardActionType.Refresh, m_searchParameters)(0), Interprise.Presentation.Base.Search.BaseSearchDashboard)
            DirectCast(searchDashboard.ParentForm, Interprise.Presentation.Base.BaseRibbonForm).MenuItemPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            searchDashboard.mnukeyFilePrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

            m_dataRowSelected = Nothing
            m_datarowsSelected = Nothing
            Return Nothing
        Else
            m_dataRowSelected = m_searchParameters.datarowSelected
            m_datarowsSelected = m_searchParameters.datarowsSelected
            ShowSearchForm = m_searchParameters.currentTableDescription
        End If
    End Function
#End Region
#End Region

End Class