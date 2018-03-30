' eShopCONNECT for Connected Business
' Module: ConfigSettingsPaymentTranslationSection.vb
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
' Updated 10 June 2012

Option Explicit On
Option Strict On

#Region " ConfigSettingsPaymentTranslationSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, _
    "Lerryn.Presentation.eShopCONNECT.ConfigSettingsPaymentTranslationSection")> _
Public Class ConfigSettingsPaymentTranslationSection

#Region " Variables "
    Private m_ConfigSettingsPaymentTranslationDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_ConfigSettingsPaymentTranslationSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_SourceCode As String
    Private m_SourceConfig As String
    Private bNewRowStarted As Boolean = False
    Private strOrigPmtTypeCode As String
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.ConfigSettingsPaymentTranslationSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_ConfigSettingsPaymentTranslationSectionFacade

        End Get
    End Property
#End Region

    Public WriteOnly Property SourceCode() As String
        Set(ByVal value As String)
            m_SourceCode = value
        End Set
    End Property

    Public WriteOnly Property SourceConfig() As String
        Set(ByVal value As String)
            m_SourceConfig = value
        End Set
    End Property
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

        Me.m_ConfigSettingsPaymentTranslationDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        Me.m_ConfigSettingsPaymentTranslationSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_ConfigSettingsPaymentTranslationDataset, _
            New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

    End Sub

    Public Sub New(ByVal ConfigSettingsPaymentTranslationDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
          ByVal ConfigSettingsPaymentTranslationSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)


        Me.m_ConfigSettingsPaymentTranslationDataset = ConfigSettingsPaymentTranslationDataset
        Me.m_ConfigSettingsPaymentTranslationSectionFacade = ConfigSettingsPaymentTranslationSectionFacade


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

#Region " ResetNewRow "
    Public Sub ResetNewRow()

        bNewRowStarted = False
        Me.GridViewPaymentTranslation.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom

    End Sub
#End Region

#End Region

#Region " Events "
    Private Sub GridViewPaymentTranslation_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewPaymentTranslation.FocusedRowChanged

        Try
            bNewRowStarted = False
            With Me.ConfigSettingsPaymentTranslationSectionGateway
                If GridViewPaymentTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
                    (GridViewPaymentTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Or bNewRowStarted) Then
                    strOrigPmtTypeCode = GridViewPaymentTranslation.GetRowCellValue(GridViewPaymentTranslation.FocusedRowHandle, .LerrynImportExportPaymentTypes_DEV000221.PaymentTypeCode_DEV000221Column.ColumnName).ToString
                End If
            End With

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)
        End Try

    End Sub

    Private Sub GridViewPaymentTranslation_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridViewPaymentTranslation.InitNewRow

        Me.GridViewPaymentTranslation.SetRowCellValue(Me.GridViewPaymentTranslation.FocusedRowHandle, Me.ConfigSettingsPaymentTranslationSectionGateway.LerrynImportExportPaymentTypes_DEV000221.SourceCode_DEV000221Column.ColumnName, m_SourceCode)
        Me.GridViewPaymentTranslation.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
        strOrigPmtTypeCode = ""
        bNewRowStarted = True

    End Sub

    Private Sub repPaymentType_OpenLink(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles repPaymentType.OpenLink

        If e.EditValue Is Nothing Then Return
        Dim PaymentTypeCode As String = CStr(Interprise.Framework.Base.Shared.Common.IsNull(e.EditValue))
        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyPaymentType.ToString, Nothing, PaymentTypeCode)
    End Sub

    Private Sub repPaymentType_PopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles repPaymentType.PopupClose

        Try
            With Me.ConfigSettingsPaymentTranslationSectionGateway
                If GridViewPaymentTranslation.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle And Not bNewRowStarted Then
                    GridViewPaymentTranslation.AddNewRow()
                End If
                GridViewPaymentTranslation.SetRowCellValue(GridViewPaymentTranslation.FocusedRowHandle, .LerrynImportExportPaymentTypes_DEV000221.PaymentTypeCode_DEV000221Column.ColumnName, eRow.DataRowSelected(.SystemPaymentTypeView.PaymentTypeCodeColumn.ColumnName))
            End With

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)
        End Try
    End Sub
#End Region
End Class
#End Region
