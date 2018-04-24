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

#Region " InventoryASPStorefrontSettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryASPStorefrontSettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryASPStorefrontSettingsSection))
        Me.InventoryASPStorefrontSettingsSectionGateway = Me.ImportExportDataset
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.PanelPublishOnASPStorefront = New DevExpress.XtraEditors.PanelControl()
        Me.lblASPStorefrontAttributes = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditQtyPublishingValue = New DevExpress.XtraEditors.TextEdit()
        Me.lblAvailableTo = New DevExpress.XtraEditors.LabelControl()
        Me.lblQtyPublishingValue = New DevExpress.XtraEditors.LabelControl()
        Me.lblAvailableFrom = New DevExpress.XtraEditors.LabelControl()
        Me.lblQtyPublishing = New DevExpress.XtraEditors.LabelControl()
        Me.RadioGroupQtyPublishing = New DevExpress.XtraEditors.RadioGroup()
        Me.DateEditAvailableTo = New DevExpress.XtraEditors.DateEdit()
        Me.DateEditAvailableFrom = New DevExpress.XtraEditors.DateEdit()
        Me.pnlGetCategoryProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetCategoryProgress = New DevExpress.XtraEditors.LabelControl()
        Me.TreeListCategories = New DevExpress.XtraTreeList.TreeList()
        Me.colTreeListCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryActive = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryID = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryGUID = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.lblDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.lblASPSellingPrice = New DevExpress.XtraEditors.LabelControl()
        Me.chkPriceAsIS = New DevExpress.XtraEditors.CheckEdit()
        Me.GridControlProperties = New DevExpress.XtraGrid.GridControl()
        Me.GridViewProperties = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colPropertiesItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesName_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesParentNode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesLineNumber = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesDataType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDataType = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.colPropertiesDateValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesNumericValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesTextValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesMemoValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesDisplayValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDateEdit = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.rbeTextEdit = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.rbeTrueFalseEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeMemoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit()
        Me.TextEditASPSellingPrice = New DevExpress.XtraEditors.TextEdit()
        Me.LabelMatrixItem = New System.Windows.Forms.Label()
        Me.LabelMatrixGroupItem = New System.Windows.Forms.Label()
        Me.lblSummary = New DevExpress.XtraEditors.LabelControl()
        Me.lblProductName = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditSummary = New DevExpress.XtraEditors.MemoEdit()
        Me.TextEditProductName = New DevExpress.XtraEditors.TextEdit()
        Me.lblSelectASPSite = New DevExpress.XtraEditors.LabelControl()
        Me.cbeSelectASPSite = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.CheckEditPublishOnASPStorefront = New DevExpress.XtraEditors.CheckEdit()
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.PanelControlDummy = New DevExpress.XtraEditors.PanelControl()
        Me.lblDevelopment = New DevExpress.XtraEditors.LabelControl()
        Me.lblActivate = New DevExpress.XtraEditors.LabelControl()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryASPStorefrontSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryASPStorefrontSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelPublishOnASPStorefront, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPublishOnASPStorefront.SuspendLayout()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditAvailableTo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditAvailableTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditAvailableFrom.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditAvailableFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetCategoryProgress.SuspendLayout()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTrueFalseEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditASPSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditSummary.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeSelectASPSite.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditPublishOnASPStorefront.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryASPStorefrontSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDummy.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventoryASPStorefrontSettingsSectionGateway
        '
        Me.InventoryASPStorefrontSettingsSectionGateway.DataSetName = "InventoryASPStorefrontSettingsSectionDataset"
        Me.InventoryASPStorefrontSettingsSectionGateway.Instantiate = False
        Me.InventoryASPStorefrontSettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventoryASPStorefrontSettingsSectionExtendedLayout
        '
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.Controls.Add(Me.PanelPublishOnASPStorefront)
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.Name = "InventoryASPStorefrontSettingsSectionExtendedLayout"
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.Root = Me.InventoryASPStorefrontSettingsSectionLayoutGroup
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.Size = New System.Drawing.Size(974, 497)
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.TabIndex = 0
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.Text = "InventoryASPStorefrontSettingsSectionExtendedLayout"
        '
        'PanelPublishOnASPStorefront
        '
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblASPStorefrontAttributes)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.TextEditQtyPublishingValue)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblAvailableTo)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblQtyPublishingValue)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblAvailableFrom)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblQtyPublishing)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.RadioGroupQtyPublishing)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.DateEditAvailableTo)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.DateEditAvailableFrom)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.pnlGetCategoryProgress)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.TreeListCategories)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblDescription)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.MemoEditDescription)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblASPSellingPrice)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.chkPriceAsIS)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.GridControlProperties)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.TextEditASPSellingPrice)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.LabelMatrixItem)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.LabelMatrixGroupItem)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblSummary)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblProductName)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.MemoEditSummary)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.TextEditProductName)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.lblSelectASPSite)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.cbeSelectASPSite)
        Me.PanelPublishOnASPStorefront.Controls.Add(Me.CheckEditPublishOnASPStorefront)
        Me.PanelPublishOnASPStorefront.Location = New System.Drawing.Point(3, 3)
        Me.PanelPublishOnASPStorefront.Name = "PanelPublishOnASPStorefront"
        Me.PanelPublishOnASPStorefront.Size = New System.Drawing.Size(968, 491)
        Me.PanelPublishOnASPStorefront.TabIndex = 4
        '
        'lblASPStorefrontAttributes
        '
        Me.lblASPStorefrontAttributes.Location = New System.Drawing.Point(7, 246)
        Me.lblASPStorefrontAttributes.Name = "lblASPStorefrontAttributes"
        Me.lblASPStorefrontAttributes.Size = New System.Drawing.Size(194, 13)
        Me.lblASPStorefrontAttributes.TabIndex = 109
        Me.lblASPStorefrontAttributes.Text = "ASPDotNetStorefront Product Attributes"
        '
        'TextEditQtyPublishingValue
        '
        Me.TextEditQtyPublishingValue.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.QtyPublishingValue_DEV000221", True))
        Me.TextEditQtyPublishingValue.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditQtyPublishingValue.Location = New System.Drawing.Point(572, 451)
        Me.TextEditQtyPublishingValue.Name = "TextEditQtyPublishingValue"
        Me.TextEditQtyPublishingValue.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditQtyPublishingValue, False)
        Me.TextEditQtyPublishingValue.Size = New System.Drawing.Size(75, 22)
        Me.TextEditQtyPublishingValue.TabIndex = 100
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblAvailableTo
        '
        Me.lblAvailableTo.Enabled = False
        Me.lblAvailableTo.Location = New System.Drawing.Point(313, 94)
        Me.lblAvailableTo.Name = "lblAvailableTo"
        Me.lblAvailableTo.Size = New System.Drawing.Size(56, 13)
        Me.lblAvailableTo.TabIndex = 108
        Me.lblAvailableTo.Text = "Available to"
        '
        'lblQtyPublishingValue
        '
        Me.lblQtyPublishingValue.Enabled = False
        Me.lblQtyPublishingValue.Location = New System.Drawing.Point(572, 432)
        Me.lblQtyPublishingValue.Name = "lblQtyPublishingValue"
        Me.lblQtyPublishingValue.Size = New System.Drawing.Size(75, 13)
        Me.lblQtyPublishingValue.TabIndex = 99
        Me.lblQtyPublishingValue.Text = "Value to Publish"
        '
        'lblAvailableFrom
        '
        Me.lblAvailableFrom.Enabled = False
        Me.lblAvailableFrom.Location = New System.Drawing.Point(301, 66)
        Me.lblAvailableFrom.Name = "lblAvailableFrom"
        Me.lblAvailableFrom.Size = New System.Drawing.Size(68, 13)
        Me.lblAvailableFrom.TabIndex = 107
        Me.lblAvailableFrom.Text = "Available from"
        '
        'lblQtyPublishing
        '
        Me.lblQtyPublishing.Enabled = False
        Me.lblQtyPublishing.Location = New System.Drawing.Point(552, 325)
        Me.lblQtyPublishing.Name = "lblQtyPublishing"
        Me.lblQtyPublishing.Size = New System.Drawing.Size(121, 13)
        Me.lblQtyPublishing.TabIndex = 98
        Me.lblQtyPublishing.Text = "Stock Quantity Publishing"
        '
        'RadioGroupQtyPublishing
        '
        Me.RadioGroupQtyPublishing.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.QtyPublishingType_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.RadioGroupQtyPublishing.Location = New System.Drawing.Point(563, 344)
        Me.RadioGroupQtyPublishing.Name = "RadioGroupQtyPublishing"
        Me.RadioGroupQtyPublishing.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("None", "None"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Fixed", "Fixed Value"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Percent", "% of Total")})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.RadioGroupQtyPublishing, False)
        Me.RadioGroupQtyPublishing.Size = New System.Drawing.Size(100, 83)
        Me.RadioGroupQtyPublishing.TabIndex = 97
        Me.ExtendControlProperty.SetTextDisplay(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'DateEditAvailableTo
        '
        Me.DateEditAvailableTo.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.AvailableTo_DEV000221", True))
        Me.DateEditAvailableTo.EditValue = Nothing
        Me.DateEditAvailableTo.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditAvailableTo, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditAvailableTo.Location = New System.Drawing.Point(375, 90)
        Me.DateEditAvailableTo.Name = "DateEditAvailableTo"
        Me.DateEditAvailableTo.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditAvailableTo.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditAvailableTo.Properties.AutoHeight = False
        Me.DateEditAvailableTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditAvailableTo.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.DateEditAvailableTo, False)
        Me.DateEditAvailableTo.Size = New System.Drawing.Size(85, 22)
        Me.DateEditAvailableTo.TabIndex = 106
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditAvailableTo, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'DateEditAvailableFrom
        '
        Me.DateEditAvailableFrom.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.AvailableFrom_DEV000221", True))
        Me.DateEditAvailableFrom.EditValue = Nothing
        Me.DateEditAvailableFrom.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditAvailableFrom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditAvailableFrom.Location = New System.Drawing.Point(375, 62)
        Me.DateEditAvailableFrom.Name = "DateEditAvailableFrom"
        Me.DateEditAvailableFrom.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditAvailableFrom.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditAvailableFrom.Properties.AutoHeight = False
        Me.DateEditAvailableFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditAvailableFrom.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.DateEditAvailableFrom, False)
        Me.DateEditAvailableFrom.Size = New System.Drawing.Size(85, 22)
        Me.DateEditAvailableFrom.TabIndex = 105
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditAvailableFrom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'pnlGetCategoryProgress
        '
        Me.pnlGetCategoryProgress.Controls.Add(Me.lblGetCategoryProgress)
        Me.pnlGetCategoryProgress.Location = New System.Drawing.Point(24, 79)
        Me.pnlGetCategoryProgress.Name = "pnlGetCategoryProgress"
        Me.pnlGetCategoryProgress.Size = New System.Drawing.Size(210, 100)
        Me.pnlGetCategoryProgress.TabIndex = 104
        Me.pnlGetCategoryProgress.Visible = False
        '
        'lblGetCategoryProgress
        '
        Me.lblGetCategoryProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblGetCategoryProgress.Location = New System.Drawing.Point(3, 28)
        Me.lblGetCategoryProgress.MinimumSize = New System.Drawing.Size(204, 0)
        Me.lblGetCategoryProgress.Name = "lblGetCategoryProgress"
        Me.lblGetCategoryProgress.Size = New System.Drawing.Size(204, 39)
        Me.lblGetCategoryProgress.TabIndex = 0
        Me.lblGetCategoryProgress.Text = "Getting ASPDotNetStorefront Categories" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please Wait "
        '
        'TreeListCategories
        '
        Me.TreeListCategories.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colTreeListCategoryName, Me.colTreeListCategoryActive, Me.colTreeListCategoryID, Me.colTreeListCategoryGUID})
        Me.TreeListCategories.Enabled = False
        Me.TreeListCategories.KeyFieldName = "SourceCategoryID"
        Me.TreeListCategories.Location = New System.Drawing.Point(7, 33)
        Me.TreeListCategories.Name = "TreeListCategories"
        Me.TreeListCategories.ParentFieldName = "SourceParentID"
        Me.TreeListCategories.Size = New System.Drawing.Size(244, 200)
        Me.TreeListCategories.TabIndex = 103
        '
        'colTreeListCategoryName
        '
        Me.colTreeListCategoryName.Caption = "Category"
        Me.colTreeListCategoryName.FieldName = "SourceCategoryName"
        Me.colTreeListCategoryName.Name = "colTreeListCategoryName"
        Me.colTreeListCategoryName.OptionsColumn.AllowEdit = False
        Me.colTreeListCategoryName.OptionsColumn.ReadOnly = True
        Me.colTreeListCategoryName.Visible = True
        Me.colTreeListCategoryName.VisibleIndex = 0
        Me.colTreeListCategoryName.Width = 150
        '
        'colTreeListCategoryActive
        '
        Me.colTreeListCategoryActive.Caption = "Active"
        Me.colTreeListCategoryActive.FieldName = "Active"
        Me.colTreeListCategoryActive.Name = "colTreeListCategoryActive"
        Me.colTreeListCategoryActive.Visible = True
        Me.colTreeListCategoryActive.VisibleIndex = 1
        Me.colTreeListCategoryActive.Width = 44
        '
        'colTreeListCategoryID
        '
        Me.colTreeListCategoryID.Caption = "ID"
        Me.colTreeListCategoryID.FieldName = "SourceCategoryID"
        Me.colTreeListCategoryID.Name = "colTreeListCategoryID"
        Me.colTreeListCategoryID.Width = 20
        '
        'colTreeListCategoryGUID
        '
        Me.colTreeListCategoryGUID.Caption = "GUID"
        Me.colTreeListCategoryGUID.FieldName = "CategoryGUID"
        Me.colTreeListCategoryGUID.Name = "colTreeListCategoryGUID"
        Me.colTreeListCategoryGUID.Width = 20
        '
        'lblDescription
        '
        Me.lblDescription.Enabled = False
        Me.lblDescription.Location = New System.Drawing.Point(628, 131)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(53, 13)
        Me.lblDescription.TabIndex = 102
        Me.lblDescription.Text = "Description"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.ProductDescription_DEV000221", True))
        Me.MemoEditDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditDescription.Location = New System.Drawing.Point(687, 131)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditDescription, False)
        Me.MemoEditDescription.Size = New System.Drawing.Size(272, 144)
        Me.MemoEditDescription.TabIndex = 101
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblASPSellingPrice
        '
        Me.lblASPSellingPrice.Enabled = False
        Me.lblASPSellingPrice.Location = New System.Drawing.Point(266, 33)
        Me.lblASPSellingPrice.Name = "lblASPSellingPrice"
        Me.lblASPSellingPrice.Size = New System.Drawing.Size(103, 26)
        Me.lblASPSellingPrice.TabIndex = 100
        Me.lblASPSellingPrice.Text = "ASPDotNetStorefront" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Selling Price"
        '
        'chkPriceAsIS
        '
        Me.chkPriceAsIS.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.UseISPricingDetail_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.chkPriceAsIS, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkPriceAsIS.Location = New System.Drawing.Point(463, 35)
        Me.chkPriceAsIS.Name = "chkPriceAsIS"
        Me.chkPriceAsIS.Properties.AutoHeight = False
        Me.chkPriceAsIS.Properties.Caption = "Use Std. price"
        Me.chkPriceAsIS.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.chkPriceAsIS, False)
        Me.chkPriceAsIS.Size = New System.Drawing.Size(109, 22)
        Me.chkPriceAsIS.TabIndex = 99
        Me.ExtendControlProperty.SetTextDisplay(Me.chkPriceAsIS, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GridControlProperties
        '
        Me.GridControlProperties.DataMember = "InventoryASPStorefrontTagDetails_DEV000221"
        Me.GridControlProperties.DataSource = Me.InventoryASPStorefrontSettingsSectionGateway
        Me.GridControlProperties.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlProperties, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.Location = New System.Drawing.Point(7, 265)
        Me.GridControlProperties.MainView = Me.GridViewProperties
        Me.GridControlProperties.Name = "GridControlProperties"
        Me.GridControlProperties.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeDataType, Me.rbeDateEdit, Me.rbeTextEdit, Me.rbeTrueFalseEdit, Me.rbeMemoEdit})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlProperties, False)
        Me.GridControlProperties.Size = New System.Drawing.Size(531, 221)
        Me.GridControlProperties.TabIndex = 98
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewProperties})
        '
        'GridViewProperties
        '
        Me.GridViewProperties.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPropertiesItemCode_DEV000221, Me.colPropertiesName_DEV000221, Me.colPropertiesParentNode, Me.colPropertiesLineNumber, Me.colPropertiesDataType_DEV000221, Me.colPropertiesDateValue_DEV000221, Me.colPropertiesNumericValue_DEV000221, Me.colPropertiesTextValue, Me.colPropertiesMemoValue, Me.colPropertiesDisplayValue})
        Me.GridViewProperties.GridControl = Me.GridControlProperties
        Me.GridViewProperties.Name = "GridViewProperties"
        Me.GridViewProperties.OptionsView.ShowGroupPanel = False
        Me.GridViewProperties.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPropertiesName_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colPropertiesItemCode_DEV000221
        '
        Me.colPropertiesItemCode_DEV000221.Caption = "Item Code"
        Me.colPropertiesItemCode_DEV000221.FieldName = "ItemCode_DEV000221"
        Me.colPropertiesItemCode_DEV000221.Name = "colPropertiesItemCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesItemCode_DEV000221.Width = 20
        '
        'colPropertiesName_DEV000221
        '
        Me.colPropertiesName_DEV000221.Caption = "Name"
        Me.colPropertiesName_DEV000221.FieldName = "TagName_DEV000221"
        Me.colPropertiesName_DEV000221.Name = "colPropertiesName_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesName_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesName_DEV000221.Visible = True
        Me.colPropertiesName_DEV000221.VisibleIndex = 0
        Me.colPropertiesName_DEV000221.Width = 135
        '
        'colPropertiesParentNode
        '
        Me.colPropertiesParentNode.Caption = "GridColumn1"
        Me.colPropertiesParentNode.FieldName = "ParentNode_DEV000221"
        Me.colPropertiesParentNode.Name = "colPropertiesParentNode"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesParentNode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesParentNode.Width = 20
        '
        'colPropertiesLineNumber
        '
        Me.colPropertiesLineNumber.Caption = "Line No."
        Me.colPropertiesLineNumber.FieldName = "LineNumber_DEV000221"
        Me.colPropertiesLineNumber.Name = "colPropertiesLineNumber"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesLineNumber, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesLineNumber.Visible = True
        Me.colPropertiesLineNumber.VisibleIndex = 1
        Me.colPropertiesLineNumber.Width = 60
        '
        'colPropertiesDataType_DEV000221
        '
        Me.colPropertiesDataType_DEV000221.Caption = "Format"
        Me.colPropertiesDataType_DEV000221.ColumnEdit = Me.rbeDataType
        Me.colPropertiesDataType_DEV000221.FieldName = "TagDataType_DEV000221"
        Me.colPropertiesDataType_DEV000221.Name = "colPropertiesDataType_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDataType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDataType_DEV000221.Visible = True
        Me.colPropertiesDataType_DEV000221.VisibleIndex = 2
        '
        'rbeDataType
        '
        Me.rbeDataType.AutoHeight = False
        Me.rbeDataType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeDataType.Items.AddRange(New Object() {"Text", "Memo", "Date", "DateTime", "Integer", "Numeric", "Boolean"})
        Me.rbeDataType.Name = "rbeDataType"
        Me.rbeDataType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'colPropertiesDateValue_DEV000221
        '
        Me.colPropertiesDateValue_DEV000221.Caption = "Date Value"
        Me.colPropertiesDateValue_DEV000221.FieldName = "TagDateValue_DEV000221"
        Me.colPropertiesDateValue_DEV000221.Name = "colPropertiesDateValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDateValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDateValue_DEV000221.Width = 20
        '
        'colPropertiesNumericValue_DEV000221
        '
        Me.colPropertiesNumericValue_DEV000221.Caption = "Numeric Value"
        Me.colPropertiesNumericValue_DEV000221.FieldName = "TagNumericValue_DEV000221"
        Me.colPropertiesNumericValue_DEV000221.Name = "colPropertiesNumericValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesNumericValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesNumericValue_DEV000221.Width = 20
        '
        'colPropertiesTextValue
        '
        Me.colPropertiesTextValue.Caption = "Text Value"
        Me.colPropertiesTextValue.FieldName = "TagTextValue_DEV000221"
        Me.colPropertiesTextValue.Name = "colPropertiesTextValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesTextValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesTextValue.Width = 20
        '
        'colPropertiesMemoValue
        '
        Me.colPropertiesMemoValue.Caption = "Memo Value"
        Me.colPropertiesMemoValue.FieldName = "TagMemoValue_DEV000221"
        Me.colPropertiesMemoValue.Name = "colPropertiesMemoValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesMemoValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesMemoValue.Width = 20
        '
        'colPropertiesDisplayValue
        '
        Me.colPropertiesDisplayValue.Caption = "Value"
        Me.colPropertiesDisplayValue.FieldName = "DisplayedValue"
        Me.colPropertiesDisplayValue.Name = "colPropertiesDisplayValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDisplayValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDisplayValue.UnboundType = DevExpress.Data.UnboundColumnType.[String]
        Me.colPropertiesDisplayValue.Visible = True
        Me.colPropertiesDisplayValue.VisibleIndex = 3
        Me.colPropertiesDisplayValue.Width = 240
        '
        'rbeDateEdit
        '
        Me.rbeDateEdit.AutoHeight = False
        Me.rbeDateEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeDateEdit.Name = "rbeDateEdit"
        Me.rbeDateEdit.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        '
        'rbeTextEdit
        '
        Me.rbeTextEdit.AutoHeight = False
        Me.rbeTextEdit.Name = "rbeTextEdit"
        '
        'rbeTrueFalseEdit
        '
        Me.rbeTrueFalseEdit.AutoHeight = False
        Me.rbeTrueFalseEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeTrueFalseEdit.Items.AddRange(New Object() {"True", "False"})
        Me.rbeTrueFalseEdit.Name = "rbeTrueFalseEdit"
        Me.rbeTrueFalseEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeMemoEdit
        '
        Me.rbeMemoEdit.Name = "rbeMemoEdit"
        '
        'TextEditASPSellingPrice
        '
        Me.TextEditASPSellingPrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.SellingPrice_DEV000221", True))
        Me.TextEditASPSellingPrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditASPSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditASPSellingPrice.Location = New System.Drawing.Point(375, 34)
        Me.TextEditASPSellingPrice.Name = "TextEditASPSellingPrice"
        Me.TextEditASPSellingPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditASPSellingPrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditASPSellingPrice.Properties.AutoHeight = False
        Me.TextEditASPSellingPrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditASPSellingPrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditASPSellingPrice, False)
        Me.TextEditASPSellingPrice.Size = New System.Drawing.Size(82, 22)
        Me.TextEditASPSellingPrice.TabIndex = 97
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditASPSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixItem
        '
        Me.LabelMatrixItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Location = New System.Drawing.Point(496, 64)
        Me.LabelMatrixItem.Name = "LabelMatrixItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixItem, False)
        Me.LabelMatrixItem.Size = New System.Drawing.Size(141, 65)
        Me.LabelMatrixItem.TabIndex = 96
        Me.LabelMatrixItem.Text = "This is a Matrix Item.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Categories" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "are set from the Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Group Item."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Visible = False
        '
        'LabelMatrixGroupItem
        '
        Me.LabelMatrixGroupItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Location = New System.Drawing.Point(496, 64)
        Me.LabelMatrixGroupItem.Name = "LabelMatrixGroupItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixGroupItem, False)
        Me.LabelMatrixGroupItem.Size = New System.Drawing.Size(141, 65)
        Me.LabelMatrixGroupItem.TabIndex = 95
        Me.LabelMatrixGroupItem.Text = "This is a Matrix Group Item." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Categor" & _
            "ies " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "are applied to every Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Item in the Matrix Group."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Visible = False
        '
        'lblSummary
        '
        Me.lblSummary.Enabled = False
        Me.lblSummary.Location = New System.Drawing.Point(629, 35)
        Me.lblSummary.Name = "lblSummary"
        Me.lblSummary.Size = New System.Drawing.Size(44, 13)
        Me.lblSummary.TabIndex = 94
        Me.lblSummary.Text = "Summary"
        '
        'lblProductName
        '
        Me.lblProductName.Enabled = False
        Me.lblProductName.Location = New System.Drawing.Point(614, 9)
        Me.lblProductName.Name = "lblProductName"
        Me.lblProductName.Size = New System.Drawing.Size(59, 13)
        Me.lblProductName.TabIndex = 93
        Me.lblProductName.Text = "Product SKU"
        '
        'MemoEditSummary
        '
        Me.MemoEditSummary.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.ProductSummary_DEV000221", True))
        Me.MemoEditSummary.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditSummary, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditSummary.Location = New System.Drawing.Point(687, 33)
        Me.MemoEditSummary.Name = "MemoEditSummary"
        Me.MemoEditSummary.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditSummary.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditSummary, False)
        Me.MemoEditSummary.Size = New System.Drawing.Size(272, 89)
        Me.MemoEditSummary.TabIndex = 92
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditSummary, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditProductName
        '
        Me.TextEditProductName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.ProductName_DEV000221", True))
        Me.TextEditProductName.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditProductName.Location = New System.Drawing.Point(687, 5)
        Me.TextEditProductName.Name = "TextEditProductName"
        Me.TextEditProductName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditProductName.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditProductName.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditProductName, False)
        Me.TextEditProductName.Size = New System.Drawing.Size(272, 22)
        Me.TextEditProductName.TabIndex = 91
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSelectASPSite
        '
        Me.lblSelectASPSite.Location = New System.Drawing.Point(7, 9)
        Me.lblSelectASPSite.Name = "lblSelectASPSite"
        Me.lblSelectASPSite.Size = New System.Drawing.Size(156, 13)
        Me.lblSelectASPSite.TabIndex = 90
        Me.lblSelectASPSite.Text = "Select ASPDotNetStorefront Site"
        '
        'cbeSelectASPSite
        '
        Me.cbeSelectASPSite.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.SiteID_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.cbeSelectASPSite, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSelectASPSite.Location = New System.Drawing.Point(169, 5)
        Me.cbeSelectASPSite.Name = "cbeSelectASPSite"
        Me.cbeSelectASPSite.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeSelectASPSite.Properties.Appearance.Options.UseBackColor = True
        Me.cbeSelectASPSite.Properties.AutoHeight = False
        Me.cbeSelectASPSite.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.cbeSelectASPSite, False)
        Me.cbeSelectASPSite.Size = New System.Drawing.Size(184, 22)
        Me.cbeSelectASPSite.TabIndex = 89
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSelectASPSite, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'CheckEditPublishOnASPStorefront
        '
        Me.CheckEditPublishOnASPStorefront.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryASPStorefrontSettingsSectionGateway, "InventoryASPStorefrontDetails_DEV000221.Publish_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditPublishOnASPStorefront, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckEditPublishOnASPStorefront.Location = New System.Drawing.Point(359, 6)
        Me.CheckEditPublishOnASPStorefront.Name = "CheckEditPublishOnASPStorefront"
        Me.CheckEditPublishOnASPStorefront.Properties.AutoHeight = False
        Me.CheckEditPublishOnASPStorefront.Properties.Caption = "Publish on this ASPDotNetStorefront Site"
        Me.CheckEditPublishOnASPStorefront.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.CheckEditPublishOnASPStorefront, False)
        Me.CheckEditPublishOnASPStorefront.Size = New System.Drawing.Size(228, 22)
        Me.CheckEditPublishOnASPStorefront.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditPublishOnASPStorefront, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'InventoryASPStorefrontSettingsSectionLayoutGroup
        '
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.CustomizationFormText = "InventoryASPStorefrontSettingsSectionLayoutGroup"
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.Name = "InventoryASPStorefrontSettingsSectionLayoutGroup"
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.Size = New System.Drawing.Size(974, 497)
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.Text = "InventoryASPStorefrontSettingsSectionLayoutGroup"
        Me.InventoryASPStorefrontSettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelPublishOnASPStorefront
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(974, 497)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'PanelControlDummy
        '
        Me.PanelControlDummy.Controls.Add(Me.lblDevelopment)
        Me.PanelControlDummy.Controls.Add(Me.lblActivate)
        Me.PanelControlDummy.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlDummy.Location = New System.Drawing.Point(0, 0)
        Me.PanelControlDummy.Name = "PanelControlDummy"
        Me.PanelControlDummy.Size = New System.Drawing.Size(974, 497)
        Me.PanelControlDummy.TabIndex = 6
        Me.PanelControlDummy.Visible = False
        '
        'lblDevelopment
        '
        Me.lblDevelopment.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblDevelopment.Location = New System.Drawing.Point(375, 237)
        Me.lblDevelopment.Name = "lblDevelopment"
        Me.lblDevelopment.Size = New System.Drawing.Size(215, 13)
        Me.lblDevelopment.TabIndex = 1
        Me.lblDevelopment.Text = "This control is still under development"
        Me.lblDevelopment.Visible = False
        '
        'lblActivate
        '
        Me.lblActivate.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblActivate.Location = New System.Drawing.Point(288, 237)
        Me.lblActivate.Name = "lblActivate"
        Me.lblActivate.Size = New System.Drawing.Size(388, 13)
        Me.lblActivate.TabIndex = 0
        Me.lblActivate.Text = "You must activate this eShopCONNECTOR in order to publish any data"
        Me.lblActivate.Visible = False
        '
        'InventoryASPStorefrontSettingsSection
        '
        Me.Controls.Add(Me.InventoryASPStorefrontSettingsSectionExtendedLayout)
        Me.Controls.Add(Me.PanelControlDummy)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventoryASPStorefrontSettingsSection"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me, False)
        Me.Size = New System.Drawing.Size(974, 497)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryASPStorefrontSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryASPStorefrontSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventoryASPStorefrontSettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelPublishOnASPStorefront, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPublishOnASPStorefront.ResumeLayout(False)
        Me.PanelPublishOnASPStorefront.PerformLayout()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditAvailableTo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditAvailableTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditAvailableFrom.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditAvailableFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetCategoryProgress.ResumeLayout(False)
        Me.pnlGetCategoryProgress.PerformLayout()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTrueFalseEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditASPSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditSummary.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeSelectASPSite.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditPublishOnASPStorefront.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryASPStorefrontSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDummy.ResumeLayout(False)
        Me.PanelControlDummy.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventoryASPStorefrontSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventoryASPStorefrontSettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventoryASPStorefrontSettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelPublishOnASPStorefront As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents CheckEditPublishOnASPStorefront As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkPriceAsIS As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents GridControlProperties As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewProperties As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPropertiesItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesName_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesDataType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeDataType As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents colPropertiesDateValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesNumericValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesTextValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesMemoValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesDisplayValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeDateEdit As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents rbeTextEdit As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents rbeTrueFalseEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeMemoEdit As DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
    Friend WithEvents TextEditASPSellingPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelMatrixItem As System.Windows.Forms.Label
    Friend WithEvents LabelMatrixGroupItem As System.Windows.Forms.Label
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblProductName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditSummary As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents TextEditProductName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSelectASPSite As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSelectASPSite As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblASPSellingPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents colPropertiesLineNumber As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesParentNode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TreeListCategories As DevExpress.XtraTreeList.TreeList
    Friend WithEvents colTreeListCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryActive As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryID As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents pnlGetCategoryProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetCategoryProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents colTreeListCategoryGUID As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents lblAvailableTo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAvailableFrom As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEditAvailableTo As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateEditAvailableFrom As DevExpress.XtraEditors.DateEdit
    Friend WithEvents PanelControlDummy As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblDevelopment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblActivate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditQtyPublishingValue As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblQtyPublishingValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblQtyPublishing As DevExpress.XtraEditors.LabelControl
    Friend WithEvents RadioGroupQtyPublishing As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents lblASPStorefrontAttributes As DevExpress.XtraEditors.LabelControl

#Region " Properties "
#Region " ImportExportDataset "
    Public ReadOnly Property ImportExportDataset() As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_InventorySettingsDataset
        End Get
    End Property
#End Region
#End Region

End Class
#End Region
