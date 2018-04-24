'===============================================================================
' Connected Business SDK
' Copyright © 2004-2008 Interprise Solutions LLC
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

#Region " ConfigSettingsSKUAliasLookupSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigSettingsSKUAliasLookupSection
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigSettingsSKUAliasLookupSection))
        Me.ConfigSettingsSKUAliasLookupSectionGateway = Me.ImportExportDataset
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.btnImportExport = New DevExpress.XtraEditors.SimpleButton()
        Me.GridControlSKUAliasLookup = New DevExpress.XtraGrid.GridControl()
        Me.GridViewSKUAliasLookup = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colSourceCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceSKU_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ColItemName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repItemCode = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.colUnitMeasureCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repUnitMeasureCode = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem2 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem3 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsSKUAliasLookupSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlSKUAliasLookup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewSKUAliasLookup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repItemCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repUnitMeasureCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'ConfigSettingsSKUAliasLookupSectionGateway
        '
        Me.ConfigSettingsSKUAliasLookupSectionGateway.DataSetName = "ConfigSettingsSKUAliasLookupSectionDataset"
        Me.ConfigSettingsSKUAliasLookupSectionGateway.Instantiate = False
        Me.ConfigSettingsSKUAliasLookupSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        '****************************************************************************************
        '
        'ImportExportDatasetGateway COMPONENT DESIGNER GENERATED CODE
        'STRICTLY FOLLOW THE STEPS BELOW IN ORDER TO IMPLEMENT DATASET SHARING
        'ACCROSS MULTIPLE USER CONTROLS AND/OR WINFORM CONTROLS
        '
        'NOTE: MAKE SURE YOU HAVE A REFERENCE TO THE FRAMEWORK DATASET COMPONENT IN YOUR TOOLBOX
        '
        '1.  SWITCH TO DESIGN VIEW OF YOUR PROJECT
        '2.  ADD AN "
        'ImportExportDatasetGateway" COMPONENT3.  IF THIS IS A PUGIN CONTROL, SET THE "Instantiate" PROPERTY TO "False"
        '4.  IF THIS IS THE MAIN PLUGIN CONTAINER, SET THE "Instantiate" PROPERTY TO "True"
        '5.  SWITCH TO CODE VIEW OF YOUR PROJECT
        '6.  ADD THE FF. CODES BELOW AND PLACE IT OUTSIDE THIS FUNCTION
        '
        '        #Region " Private Variables "
        '            Private m_importExportDataset as Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway 
        '        #End Region
        '
        '        #Region " Properties "
        '
        '        #Region "ImportExportDataset"
        '            Public ReadOnly Property ImportExportDataset AS Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway 
        '                Get
        '                    Return Me.m_importExportDataset
        '                End Get
        '            End Property
        '        #End Region
        '
        '        #End Region
        '
        '****************************************************************************************
        '
        '
        'ConfigSettingsSKUAliasLookupSectionExtendedLayout
        '
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.AllowCustomizationMenu = False
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Controls.Add(Me.PanelControl1)
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Controls.Add(Me.btnImportExport)
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Controls.Add(Me.GridControlSKUAliasLookup)
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.IsResetingLayout = False
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.IsResetSection = False
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Name = "ConfigSettingsSKUAliasLookupSectionExtendedLayout"
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.PluginContainerDataset = Nothing
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Root = Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.TabIndex = 0
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.Text = "ConfigSettingsSKUAliasLookupSectionExtendedLayout"
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'PanelControl1
        '
        Me.PanelControl1.Location = New System.Drawing.Point(106, 395)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(583, 22)
        Me.PanelControl1.TabIndex = 6
        '
        'btnImportExport
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnImportExport, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnImportExport.Location = New System.Drawing.Point(2, 395)
        Me.btnImportExport.Name = "btnImportExport"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnImportExport, System.Drawing.Color.Empty)
        Me.btnImportExport.Size = New System.Drawing.Size(100, 22)
        Me.btnImportExport.StyleController = Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout
        Me.btnImportExport.TabIndex = 5
        Me.btnImportExport.Text = "Import/Export"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnImportExport, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GridControlSKUAliasLookup
        '
        Me.GridControlSKUAliasLookup.DataMember = "LerrynImportExportSKUAliasView_DEV000221"
        Me.GridControlSKUAliasLookup.DataSource = Me.ConfigSettingsSKUAliasLookupSectionGateway
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlSKUAliasLookup, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlSKUAliasLookup, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSKUAliasLookup.Location = New System.Drawing.Point(3, 8)
        Me.GridControlSKUAliasLookup.MainView = Me.GridViewSKUAliasLookup
        Me.GridControlSKUAliasLookup.Name = "GridControlSKUAliasLookup"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlSKUAliasLookup, System.Drawing.Color.Empty)
        Me.GridControlSKUAliasLookup.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repItemCode, Me.repUnitMeasureCode})
        Me.GridControlSKUAliasLookup.Size = New System.Drawing.Size(685, 382)
        Me.GridControlSKUAliasLookup.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlSKUAliasLookup, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSKUAliasLookup.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewSKUAliasLookup})
        '
        'GridViewSKUAliasLookup
        '
        Me.GridViewSKUAliasLookup.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colSourceCode_DEV000221, Me.colSourceSKU_DEV000221, Me.colItemCode_DEV000221, Me.ColItemName, Me.colUnitMeasureCode})
        Me.GridViewSKUAliasLookup.GridControl = Me.GridControlSKUAliasLookup
        Me.GridViewSKUAliasLookup.Name = "GridViewSKUAliasLookup"
        Me.GridViewSKUAliasLookup.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.GridViewSKUAliasLookup.OptionsView.ShowGroupPanel = False
        '
        'colSourceCode_DEV000221
        '
        Me.colSourceCode_DEV000221.Caption = "Source Code"
        Me.colSourceCode_DEV000221.FieldName = "SourceCode_DEV000221"
        Me.colSourceCode_DEV000221.Name = "colSourceCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourceSKU_DEV000221
        '
        Me.colSourceSKU_DEV000221.Caption = "Source SKU"
        Me.colSourceSKU_DEV000221.FieldName = "SourceSKU_DEV000221"
        Me.colSourceSKU_DEV000221.Name = "colSourceSKU_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceSKU_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceSKU_DEV000221.Visible = True
        Me.colSourceSKU_DEV000221.VisibleIndex = 0
        Me.colSourceSKU_DEV000221.Width = 280
        '
        'colItemCode_DEV000221
        '
        Me.colItemCode_DEV000221.Caption = "IS Item"
        Me.colItemCode_DEV000221.FieldName = "ItemCode_DEV000221"
        Me.colItemCode_DEV000221.Name = "colItemCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colItemCode_DEV000221.Width = 20
        '
        'ColItemName
        '
        Me.ColItemName.Caption = "CB Item"
        Me.ColItemName.ColumnEdit = Me.repItemCode
        Me.ColItemName.FieldName = "ItemName"
        Me.ColItemName.Name = "ColItemName"
        Me.ExtendControlProperty.SetTextDisplay(Me.ColItemName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ColItemName.Visible = True
        Me.ColItemName.VisibleIndex = 1
        Me.ColItemName.Width = 230
        '
        'repItemCode
        '
        Me.repItemCode.AdditionalFilter = ""
        Me.repItemCode.AllowEdit = True
        Me.repItemCode.AutoHeight = False
        Me.repItemCode.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repItemCode.ColumnDescriptions = Nothing
        Me.repItemCode.ColumnNames = Nothing
        Me.repItemCode.DataSource = Nothing
        Me.repItemCode.DataSourceColumns = Nothing
        Me.repItemCode.DefaultSort = Nothing
        Me.repItemCode.DisplayField = ""
        Me.repItemCode.IsMultiSelect = False
        Me.repItemCode.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.repItemCode.Name = "repItemCode"
        Me.repItemCode.TableName = "SaleItemView"
        Me.repItemCode.TargetDisplayField = Nothing
        Me.repItemCode.TargetValueMember = "ItemCode_DEV000221"
        Me.repItemCode.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.repItemCode.UseCache = False
        Me.repItemCode.UseSpecifiedColumns = False
        Me.repItemCode.ValueMember = ""
        '
        'colUnitMeasureCode
        '
        Me.colUnitMeasureCode.Caption = "Unit of Measure "
        Me.colUnitMeasureCode.ColumnEdit = Me.repUnitMeasureCode
        Me.colUnitMeasureCode.FieldName = "UnitMeasureCode_DEV000221"
        Me.colUnitMeasureCode.Name = "colUnitMeasureCode"
        Me.ExtendControlProperty.SetTextDisplay(Me.colUnitMeasureCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colUnitMeasureCode.Visible = True
        Me.colUnitMeasureCode.VisibleIndex = 2
        Me.colUnitMeasureCode.Width = 157
        '
        'repUnitMeasureCode
        '
        Me.repUnitMeasureCode.AdditionalFilter = " and ItemCode = ''"
        Me.repUnitMeasureCode.AllowEdit = True
        Me.repUnitMeasureCode.AutoHeight = False
        Me.repUnitMeasureCode.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repUnitMeasureCode.ColumnDescriptions = Nothing
        Me.repUnitMeasureCode.ColumnNames = Nothing
        Me.repUnitMeasureCode.DataSource = Nothing
        Me.repUnitMeasureCode.DataSourceColumns = Nothing
        Me.repUnitMeasureCode.DefaultSort = Nothing
        Me.repUnitMeasureCode.DisplayField = ""
        Me.repUnitMeasureCode.IsMultiSelect = False
        Me.repUnitMeasureCode.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.repUnitMeasureCode.Name = "repUnitMeasureCode"
        Me.repUnitMeasureCode.TableName = "InventoryUnitMeasureView"
        Me.repUnitMeasureCode.TargetDisplayField = Nothing
        Me.repUnitMeasureCode.TargetValueMember = "UnitMeasureCode_DEV000221"
        Me.repUnitMeasureCode.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.repUnitMeasureCode.UseCache = False
        Me.repUnitMeasureCode.UseSpecifiedColumns = False
        Me.repUnitMeasureCode.ValueMember = ""
        '
        'ConfigSettingsSKUAliasLookupSectionLayoutGroup
        '
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.CustomizationFormText = "ConfigSettingsSKUAliasLookupSectionLayoutGroup"
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.GroupBordersVisible = False
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.Name = "ConfigSettingsSKUAliasLookupSectionLayoutGroup"
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.Text = "ConfigSettingsSKUAliasLookupSectionLayoutGroup"
        Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControlSKUAliasLookup
        Me.LayoutControlItem1.CustomizationFormText = "LayoutItemSKUAliasLookup"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem1.ReadOnly = False
        Me.LayoutControlItem1.Size = New System.Drawing.Size(691, 393)
        Me.LayoutControlItem1.Text = " "
        Me.LayoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 5
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnImportExport
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 393)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.ReadOnly = False
        Me.LayoutControlItem2.Size = New System.Drawing.Size(104, 26)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.PanelControl1
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(104, 393)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.ReadOnly = False
        Me.LayoutControlItem3.Size = New System.Drawing.Size(587, 26)
        Me.LayoutControlItem3.Text = "LayoutControlItem3"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem3, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'ConfigSettingsSKUAliasLookupSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout)
        Me.FindSearch = Interprise.Framework.Base.[Shared].[Enum].FindSearch.None
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "ConfigSettingsSKUAliasLookupSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(691, 419)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TransactionType = Interprise.Framework.Base.[Shared].[Enum].TransactionType.None
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsSKUAliasLookupSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ConfigSettingsSKUAliasLookupSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlSKUAliasLookup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewSKUAliasLookup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repItemCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repUnitMeasureCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsSKUAliasLookupSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents ConfigSettingsSKUAliasLookupSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents ConfigSettingsSKUAliasLookupSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents ConfigSettingsSKUAliasLookupSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControlSKUAliasLookup As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewSKUAliasLookup As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents colSourceCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceSKU_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repItemCode As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
    Friend WithEvents colItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btnImportExport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItem3 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents ColItemName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colUnitMeasureCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repUnitMeasureCode As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl

    Public ReadOnly Property ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_ConfigSettingsSKUAliasLookupDataset
        End Get
    End Property

End Class
#End Region
