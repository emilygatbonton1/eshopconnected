'===============================================================================
' Connected Business SDK
' Copyright © 2009-2010 Interprise Solutions LLC
' All rights reserved.
' 
' Interprise Plugin Factory - Generated Code
'
' This code and information is provided "as is" without warranty
' of any kind, either expressed or implied, including but not
' limited to the implied warranties of merchantability and
' fitness for a particular purpose.
'===============================================================================

Option Explicit On
Option Strict On

#Region " ConfigSettingsPaymentTranslationSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigSettingsPaymentTranslationSection
	Inherits Interprise.Presentation.Base.BaseControl
	Implements Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Protected Overridable Sub InitializeComponent()
        components = New System.ComponentModel.Container()

        Me.ConfigSettingsPaymentTranslationSectionGateway =  m_ConfigSettingsPaymentTranslationDataset
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.GridControlPaymentTranslation = New DevExpress.XtraGrid.GridControl
        Me.GridViewPaymentTranslation = New DevExpress.XtraGrid.Views.Grid.GridView
        Me.colSourceCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colSourcePaymentType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colPaymentTypeCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.colDoNotImport_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn
        Me.repPaymentType = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsPaymentTranslationSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsPaymentTranslationSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.SuspendLayout()
        CType(Me.GridControlPaymentTranslation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewPaymentTranslation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repPaymentType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsPaymentTranslationSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ConfigSettingsPaymentTranslationSectionGateway
        '
        Me.ConfigSettingsPaymentTranslationSectionGateway.DataSetName = "ConfigSettingsPaymentTranslationSectionDataset"
        Me.ConfigSettingsPaymentTranslationSectionGateway.Instantiate = False
        Me.ConfigSettingsPaymentTranslationSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ConfigSettingsPaymentTranslationSectionExtendedLayout
        '
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.Controls.Add(Me.GridControlPaymentTranslation)
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.Name = "ConfigSettingsPaymentTranslationSectionExtendedLayout"
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.Root = Me.ConfigSettingsPaymentTranslationSectionLayoutGroup
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.TabIndex = 0
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.Text = "ConfigSettingsPaymentTranslationSectionExtendedLayout"
        '
        'GridControlPaymentTranslation
        '
        Me.GridControlPaymentTranslation.DataMember = "LerrynImportExportPaymentTypes_DEV000221"
        Me.GridControlPaymentTranslation.DataSource = Me.ConfigSettingsPaymentTranslationSectionGateway
        Me.GridControlPaymentTranslation.EmbeddedNavigator.Name = ""
        Me.ExtendControlProperty.SetHelpText(Me.GridControlPaymentTranslation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlPaymentTranslation.Location = New System.Drawing.Point(4, 9)
        Me.GridControlPaymentTranslation.MainView = Me.GridViewPaymentTranslation
        Me.GridControlPaymentTranslation.Name = "GridControlPaymentTranslation"
        Me.GridControlPaymentTranslation.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repPaymentType})
        Me.GridControlPaymentTranslation.Size = New System.Drawing.Size(684, 407)
        Me.GridControlPaymentTranslation.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlPaymentTranslation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlPaymentTranslation.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewPaymentTranslation})
        '
        'GridViewPaymentTranslation
        '
        Me.GridViewPaymentTranslation.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colSourceCode_DEV000221, Me.colSourcePaymentType_DEV000221, Me.colPaymentTypeCode_DEV000221, Me.colDoNotImport_DEV000221})
        Me.GridViewPaymentTranslation.GridControl = Me.GridControlPaymentTranslation
        Me.GridViewPaymentTranslation.Name = "GridViewPaymentTranslation"
        Me.GridViewPaymentTranslation.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.GridViewPaymentTranslation.OptionsView.ShowGroupPanel = False
        '
        'colSourceCode_DEV000221
        '
        Me.colSourceCode_DEV000221.Caption = "Source Code"
        Me.colSourceCode_DEV000221.FieldName = "SourceCode_DEV000221"
        Me.colSourceCode_DEV000221.Name = "colSourceCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourcePaymentType_DEV000221
        '
        Me.colSourcePaymentType_DEV000221.Caption = "Source Payment Type"
        Me.colSourcePaymentType_DEV000221.FieldName = "SourcePaymentType_DEV000221"
        Me.colSourcePaymentType_DEV000221.Name = "colSourcePaymentType_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourcePaymentType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourcePaymentType_DEV000221.Visible = True
        Me.colSourcePaymentType_DEV000221.VisibleIndex = 0
        Me.colSourcePaymentType_DEV000221.Width = 160
        '
        'colPaymentTypeCode_DEV000221
        '
        Me.colPaymentTypeCode_DEV000221.Caption = "Payment Type Code"
        Me.colPaymentTypeCode_DEV000221.ColumnEdit = Me.repPaymentType
        Me.colPaymentTypeCode_DEV000221.FieldName = "PaymentTypeCode_DEV000221"
        Me.colPaymentTypeCode_DEV000221.Name = "colPaymentTypeCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPaymentTypeCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPaymentTypeCode_DEV000221.Visible = True
        Me.colPaymentTypeCode_DEV000221.VisibleIndex = 1
        Me.colPaymentTypeCode_DEV000221.Width = 233
        '
        'colDoNotImport_DEV000221
        '
        Me.colDoNotImport_DEV000221.Caption = "Do not Import"
        Me.colDoNotImport_DEV000221.FieldName = "DoNotImport_DEV000221"
        Me.colDoNotImport_DEV000221.Name = "colDoNotImport_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colDoNotImport_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colDoNotImport_DEV000221.Visible = True
        Me.colDoNotImport_DEV000221.VisibleIndex = 2
        Me.colDoNotImport_DEV000221.Width = 60
        '
        'repPaymentType
        '
        Me.repPaymentType.AdditionalFilter = " and IsActive = 1 and PaymentMethodCode <> 'Web Checkout' and PaymentMethodCode <> 'Credit Card'"
        Me.repPaymentType.AllowEdit = True
        Me.repPaymentType.AutoHeight = False
        Me.repPaymentType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repPaymentType.ColumnDescriptions = Nothing
        Me.repPaymentType.ColumnNames = Nothing
        Me.repPaymentType.DataSource = Nothing
        Me.repPaymentType.DataSourceColumns = Nothing
        Me.repPaymentType.DefaultSort = Nothing
        Me.repPaymentType.DisplayField = "PaymentTypeCode"
        Me.repPaymentType.IsMultiSelect = False
        Me.repPaymentType.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.repPaymentType.Name = "repPaymentType"
        Me.repPaymentType.TableName = "SystemPaymentType"
        Me.repPaymentType.TargetValueMember = "PaymentTypeCode_DEV000221"
        Me.repPaymentType.UseCache = False
        Me.repPaymentType.UseSpecifiedColumns = False
        Me.repPaymentType.ValueMember = "PaymentTypeCode"
        '
        'ConfigSettingsPaymentTranslationSectionLayoutGroup
        '
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.CustomizationFormText = "ConfigSettingsPaymentTranslationSectionLayoutGroup"
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.GroupBordersVisible = False
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.Name = "ConfigSettingsPaymentTranslationSectionLayoutGroup"
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.Text = "ConfigSettingsPaymentTranslationSectionLayoutGroup"
        Me.ConfigSettingsPaymentTranslationSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControlPaymentTranslation
        Me.LayoutControlItem1.CustomizationFormText = "LayoutItemDeliveryTranslation"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(691, 419)
        Me.LayoutControlItem1.Text = " "
        Me.LayoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'ConfigSettingsPaymentTranslationSection
        '
        Me.Controls.Add(Me.ConfigSettingsPaymentTranslationSectionExtendedLayout)
        Me.Name = "ConfigSettingsPaymentTranslationSection"
        Me.Size = New System.Drawing.Size(691, 419)
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsPaymentTranslationSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsPaymentTranslationSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ConfigSettingsPaymentTranslationSectionExtendedLayout.ResumeLayout(False)
        CType(Me.GridControlPaymentTranslation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewPaymentTranslation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repPaymentType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsPaymentTranslationSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Protected WithEvents ConfigSettingsPaymentTranslationSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents ConfigSettingsPaymentTranslationSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents ConfigSettingsPaymentTranslationSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControlPaymentTranslation As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewPaymentTranslation As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents colSourceCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourcePaymentType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPaymentTypeCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDoNotImport_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repPaymentType As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl

End Class
#End Region
