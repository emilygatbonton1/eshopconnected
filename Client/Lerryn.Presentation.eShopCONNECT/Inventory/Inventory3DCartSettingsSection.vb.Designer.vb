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

#Region " Inventory3DCartSettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Inventory3DCartSettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Inventory3DCartSettingsSection))
        Me.Inventory3DCartSettingsSectionGateway = Me.ImportExportDataset
        Me.Inventory3DCartSettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl()
        Me.PanelPublishOn3DCart = New DevExpress.XtraEditors.PanelControl()
        Me.lbl3DCartSalePriceSource = New DevExpress.XtraEditors.LabelControl()
        Me.cbe3DCartSalePriceSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lbl3DCartPriceSource = New DevExpress.XtraEditors.LabelControl()
        Me.cbe3DCartPriceSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.TextEditQtyPublishingValue = New DevExpress.XtraEditors.TextEdit()
        Me.lblQtyPublishingValue = New DevExpress.XtraEditors.LabelControl()
        Me.lblQtyPublishing = New DevExpress.XtraEditors.LabelControl()
        Me.RadioGroupQtyPublishing = New DevExpress.XtraEditors.RadioGroup()
        Me.lbl3DCartAttributes = New DevExpress.XtraEditors.LabelControl()
        Me.pnlGetCategoryProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetCategoryProgress = New DevExpress.XtraEditors.LabelControl()
        Me.chkPriceAsIS = New DevExpress.XtraEditors.CheckEdit()
        Me.lblExtendedDesc = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditExtendedDesc = New DevExpress.XtraEditors.MemoEdit()
        Me.lblDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.lblSalePrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditSalePrice = New DevExpress.XtraEditors.TextEdit()
        Me.GridControlProperties = New DevExpress.XtraGrid.GridControl()
        Me.GridViewProperties = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colPropertiesItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesName_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesDataType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDataType = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.colPropertiesDateValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesNumericValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesTextValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesMemoValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesDisplayValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeHasSelectValues_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDateEdit = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.rbeTextEdit = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.rbeYesNoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeMemoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit()
        Me.rbeSelectEdit = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.lbl3DCartSellingPrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEdit3DCartSellingPrice = New DevExpress.XtraEditors.TextEdit()
        Me.LabelMatrixItem = New System.Windows.Forms.Label()
        Me.LabelMatrixGroupItem = New System.Windows.Forms.Label()
        Me.lblTitle = New DevExpress.XtraEditors.LabelControl()
        Me.lblProductName = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditTitle = New DevExpress.XtraEditors.MemoEdit()
        Me.TextEditProductName = New DevExpress.XtraEditors.TextEdit()
        Me.lblSelect3DCartStore = New DevExpress.XtraEditors.LabelControl()
        Me.cbeSelect3DCartStore = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.TreeListCategories = New DevExpress.XtraTreeList.TreeList()
        Me.colTreeListCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryActive = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryID = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.CheckEditPublishOn3DCart = New DevExpress.XtraEditors.CheckEdit()
        Me.Inventory3DCartSettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.PanelControlDummy = New DevExpress.XtraEditors.PanelControl()
        Me.lblDevelopment = New DevExpress.XtraEditors.LabelControl()
        Me.lblActivate = New DevExpress.XtraEditors.LabelControl()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Inventory3DCartSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Inventory3DCartSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Inventory3DCartSettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelPublishOn3DCart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPublishOn3DCart.SuspendLayout()
        CType(Me.cbe3DCartSalePriceSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbe3DCartPriceSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetCategoryProgress.SuspendLayout()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditExtendedDesc.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditSalePrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeSelectEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit3DCartSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditTitle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeSelect3DCartStore.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditPublishOn3DCart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Inventory3DCartSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDummy.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'Inventory3DCartSettingsSectionGateway
        '
        Me.Inventory3DCartSettingsSectionGateway.DataSetName = "Inventory3DCartSettingsSectionDataset"
        Me.Inventory3DCartSettingsSectionGateway.Instantiate = False
        Me.Inventory3DCartSettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'Inventory3DCartSettingsSectionExtendedLayout
        '
        Me.Inventory3DCartSettingsSectionExtendedLayout.AllowCustomizationMenu = False
        Me.Inventory3DCartSettingsSectionExtendedLayout.Controls.Add(Me.PanelPublishOn3DCart)
        Me.Inventory3DCartSettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Inventory3DCartSettingsSectionExtendedLayout.IsResetSection = False
        Me.Inventory3DCartSettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.Inventory3DCartSettingsSectionExtendedLayout.Name = "Inventory3DCartSettingsSectionExtendedLayout"
        Me.Inventory3DCartSettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.Inventory3DCartSettingsSectionExtendedLayout.PluginContainerDataset = Nothing
        Me.Inventory3DCartSettingsSectionExtendedLayout.Root = Me.Inventory3DCartSettingsSectionLayoutGroup
        Me.Inventory3DCartSettingsSectionExtendedLayout.Size = New System.Drawing.Size(993, 497)
        Me.Inventory3DCartSettingsSectionExtendedLayout.TabIndex = 0
        Me.Inventory3DCartSettingsSectionExtendedLayout.Text = "Inventory3DCartSettingsSectionExtendedLayout"
        Me.Inventory3DCartSettingsSectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'PanelPublishOn3DCart
        '
        Me.PanelPublishOn3DCart.Controls.Add(Me.lbl3DCartSalePriceSource)
        Me.PanelPublishOn3DCart.Controls.Add(Me.cbe3DCartSalePriceSource)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lbl3DCartPriceSource)
        Me.PanelPublishOn3DCart.Controls.Add(Me.cbe3DCartPriceSource)
        Me.PanelPublishOn3DCart.Controls.Add(Me.TextEditQtyPublishingValue)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblQtyPublishingValue)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblQtyPublishing)
        Me.PanelPublishOn3DCart.Controls.Add(Me.RadioGroupQtyPublishing)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lbl3DCartAttributes)
        Me.PanelPublishOn3DCart.Controls.Add(Me.pnlGetCategoryProgress)
        Me.PanelPublishOn3DCart.Controls.Add(Me.chkPriceAsIS)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblExtendedDesc)
        Me.PanelPublishOn3DCart.Controls.Add(Me.MemoEditExtendedDesc)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblDescription)
        Me.PanelPublishOn3DCart.Controls.Add(Me.MemoEditDescription)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblSalePrice)
        Me.PanelPublishOn3DCart.Controls.Add(Me.TextEditSalePrice)
        Me.PanelPublishOn3DCart.Controls.Add(Me.GridControlProperties)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lbl3DCartSellingPrice)
        Me.PanelPublishOn3DCart.Controls.Add(Me.TextEdit3DCartSellingPrice)
        Me.PanelPublishOn3DCart.Controls.Add(Me.LabelMatrixItem)
        Me.PanelPublishOn3DCart.Controls.Add(Me.LabelMatrixGroupItem)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblTitle)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblProductName)
        Me.PanelPublishOn3DCart.Controls.Add(Me.MemoEditTitle)
        Me.PanelPublishOn3DCart.Controls.Add(Me.TextEditProductName)
        Me.PanelPublishOn3DCart.Controls.Add(Me.lblSelect3DCartStore)
        Me.PanelPublishOn3DCart.Controls.Add(Me.cbeSelect3DCartStore)
        Me.PanelPublishOn3DCart.Controls.Add(Me.TreeListCategories)
        Me.PanelPublishOn3DCart.Controls.Add(Me.CheckEditPublishOn3DCart)
        Me.PanelPublishOn3DCart.Location = New System.Drawing.Point(3, 3)
        Me.PanelPublishOn3DCart.Name = "PanelPublishOn3DCart"
        Me.PanelPublishOn3DCart.Size = New System.Drawing.Size(987, 491)
        Me.PanelPublishOn3DCart.TabIndex = 4
        '
        'lbl3DCartSalePriceSource
        '
        Me.lbl3DCartSalePriceSource.Enabled = False
        Me.lbl3DCartSalePriceSource.Location = New System.Drawing.Point(262, 129)
        Me.lbl3DCartSalePriceSource.Name = "lbl3DCartSalePriceSource"
        Me.lbl3DCartSalePriceSource.Size = New System.Drawing.Size(81, 13)
        Me.lbl3DCartSalePriceSource.TabIndex = 106
        Me.lbl3DCartSalePriceSource.Text = "Sale Price source"
        '
        'cbe3DCartSalePriceSource
        '
        Me.cbe3DCartSalePriceSource.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.SalePriceSource_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.cbe3DCartSalePriceSource, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "SalePriceSource_DEV000221"))
        Me.cbe3DCartSalePriceSource.Location = New System.Drawing.Point(349, 125)
        Me.cbe3DCartSalePriceSource.Name = "cbe3DCartSalePriceSource"
        Me.cbe3DCartSalePriceSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbe3DCartSalePriceSource.Properties.Appearance.Options.UseFont = True
        Me.cbe3DCartSalePriceSource.Properties.AutoHeight = False
        Me.cbe3DCartSalePriceSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbe3DCartSalePriceSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Wholesale Price", "W", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Retail Price", "R", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Promotional Price", "P", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("This form", "N", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbe3DCartSalePriceSource, System.Drawing.Color.Empty)
        Me.cbe3DCartSalePriceSource.Size = New System.Drawing.Size(142, 22)
        Me.cbe3DCartSalePriceSource.TabIndex = 105
        Me.ExtendControlProperty.SetTextDisplay(Me.cbe3DCartSalePriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lbl3DCartPriceSource
        '
        Me.lbl3DCartPriceSource.Enabled = False
        Me.lbl3DCartPriceSource.Location = New System.Drawing.Point(252, 66)
        Me.lbl3DCartPriceSource.Name = "lbl3DCartPriceSource"
        Me.lbl3DCartPriceSource.Size = New System.Drawing.Size(91, 13)
        Me.lbl3DCartPriceSource.TabIndex = 104
        Me.lbl3DCartPriceSource.Text = "Selling Price source"
        '
        'cbe3DCartPriceSource
        '
        Me.cbe3DCartPriceSource.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.SellingPriceSource_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.cbe3DCartPriceSource, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "SellingPriceSource_DEV000221"))
        Me.cbe3DCartPriceSource.Location = New System.Drawing.Point(349, 62)
        Me.cbe3DCartPriceSource.Name = "cbe3DCartPriceSource"
        Me.cbe3DCartPriceSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbe3DCartPriceSource.Properties.Appearance.Options.UseFont = True
        Me.cbe3DCartPriceSource.Properties.AutoHeight = False
        Me.cbe3DCartPriceSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbe3DCartPriceSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Wholesale Price", "W", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Retail Price", "R", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Sug Retail Price", "S", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbe3DCartPriceSource, System.Drawing.Color.Empty)
        Me.cbe3DCartPriceSource.Size = New System.Drawing.Size(142, 22)
        Me.cbe3DCartPriceSource.TabIndex = 103
        Me.ExtendControlProperty.SetTextDisplay(Me.cbe3DCartPriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditQtyPublishingValue
        '
        Me.TextEditQtyPublishingValue.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.QtyPublishingValue_DEV000221", True))
        Me.TextEditQtyPublishingValue.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "QtyPublishingValue_DEV000221"))
        Me.TextEditQtyPublishingValue.Location = New System.Drawing.Point(564, 455)
        Me.TextEditQtyPublishingValue.Name = "TextEditQtyPublishingValue"
        Me.TextEditQtyPublishingValue.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditQtyPublishingValue, System.Drawing.Color.Empty)
        Me.TextEditQtyPublishingValue.Size = New System.Drawing.Size(75, 22)
        Me.TextEditQtyPublishingValue.TabIndex = 96
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblQtyPublishingValue
        '
        Me.lblQtyPublishingValue.Enabled = False
        Me.lblQtyPublishingValue.Location = New System.Drawing.Point(564, 436)
        Me.lblQtyPublishingValue.Name = "lblQtyPublishingValue"
        Me.lblQtyPublishingValue.Size = New System.Drawing.Size(75, 13)
        Me.lblQtyPublishingValue.TabIndex = 95
        Me.lblQtyPublishingValue.Text = "Value to Publish"
        '
        'lblQtyPublishing
        '
        Me.lblQtyPublishing.Enabled = False
        Me.lblQtyPublishing.Location = New System.Drawing.Point(544, 329)
        Me.lblQtyPublishing.Name = "lblQtyPublishing"
        Me.lblQtyPublishing.Size = New System.Drawing.Size(121, 13)
        Me.lblQtyPublishing.TabIndex = 94
        Me.lblQtyPublishing.Text = "Stock Quantity Publishing"
        '
        'RadioGroupQtyPublishing
        '
        Me.RadioGroupQtyPublishing.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.QtyPublishingType_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "QtyPublishingType_DEV000221"))
        Me.RadioGroupQtyPublishing.Location = New System.Drawing.Point(555, 348)
        Me.RadioGroupQtyPublishing.Name = "RadioGroupQtyPublishing"
        Me.RadioGroupQtyPublishing.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("None", "None"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Fixed", "Fixed Value"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Percent", "% of Total")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.RadioGroupQtyPublishing, System.Drawing.Color.Empty)
        Me.RadioGroupQtyPublishing.Size = New System.Drawing.Size(100, 83)
        Me.RadioGroupQtyPublishing.TabIndex = 93
        Me.ExtendControlProperty.SetTextDisplay(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lbl3DCartAttributes
        '
        Me.lbl3DCartAttributes.Location = New System.Drawing.Point(5, 245)
        Me.lbl3DCartAttributes.Name = "lbl3DCartAttributes"
        Me.lbl3DCartAttributes.Size = New System.Drawing.Size(125, 13)
        Me.lbl3DCartAttributes.TabIndex = 90
        Me.lbl3DCartAttributes.Text = "3DCart Product Attributes"
        '
        'pnlGetCategoryProgress
        '
        Me.pnlGetCategoryProgress.Controls.Add(Me.lblGetCategoryProgress)
        Me.pnlGetCategoryProgress.Location = New System.Drawing.Point(16, 75)
        Me.pnlGetCategoryProgress.Name = "pnlGetCategoryProgress"
        Me.pnlGetCategoryProgress.Size = New System.Drawing.Size(200, 100)
        Me.pnlGetCategoryProgress.TabIndex = 89
        Me.pnlGetCategoryProgress.Visible = False
        '
        'lblGetCategoryProgress
        '
        Me.lblGetCategoryProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblGetCategoryProgress.Location = New System.Drawing.Point(10, 28)
        Me.lblGetCategoryProgress.MinimumSize = New System.Drawing.Size(180, 0)
        Me.lblGetCategoryProgress.Name = "lblGetCategoryProgress"
        Me.lblGetCategoryProgress.Size = New System.Drawing.Size(180, 39)
        Me.lblGetCategoryProgress.TabIndex = 0
        Me.lblGetCategoryProgress.Text = "Getting 3DCart Categories" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please Wait"
        '
        'chkPriceAsIS
        '
        Me.chkPriceAsIS.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.UseISPricingDetail_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.chkPriceAsIS, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "UseISPricingDetail_DEV000221"))
        Me.chkPriceAsIS.Location = New System.Drawing.Point(437, 34)
        Me.chkPriceAsIS.Name = "chkPriceAsIS"
        Me.chkPriceAsIS.Properties.AutoHeight = False
        Me.chkPriceAsIS.Properties.Caption = "Use Std. price"
        Me.chkPriceAsIS.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkPriceAsIS, System.Drawing.Color.Empty)
        Me.chkPriceAsIS.Size = New System.Drawing.Size(109, 22)
        Me.chkPriceAsIS.TabIndex = 88
        Me.ExtendControlProperty.SetTextDisplay(Me.chkPriceAsIS, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblExtendedDesc
        '
        Me.lblExtendedDesc.Enabled = False
        Me.lblExtendedDesc.Location = New System.Drawing.Point(596, 228)
        Me.lblExtendedDesc.Name = "lblExtendedDesc"
        Me.lblExtendedDesc.Size = New System.Drawing.Size(102, 13)
        Me.lblExtendedDesc.TabIndex = 83
        Me.lblExtendedDesc.Text = "Extended Description"
        '
        'MemoEditExtendedDesc
        '
        Me.MemoEditExtendedDesc.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.ProductExtendedDescription_DEV000221", True))
        Me.MemoEditExtendedDesc.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditExtendedDesc, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "ProductExtendedDescription_DEV000221"))
        Me.MemoEditExtendedDesc.Location = New System.Drawing.Point(708, 225)
        Me.MemoEditExtendedDesc.Name = "MemoEditExtendedDesc"
        Me.MemoEditExtendedDesc.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditExtendedDesc.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditExtendedDesc, System.Drawing.Color.Empty)
        Me.MemoEditExtendedDesc.Size = New System.Drawing.Size(272, 199)
        Me.MemoEditExtendedDesc.TabIndex = 82
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditExtendedDesc, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblDescription
        '
        Me.lblDescription.Enabled = False
        Me.lblDescription.Location = New System.Drawing.Point(645, 78)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(53, 13)
        Me.lblDescription.TabIndex = 81
        Me.lblDescription.Text = "Description"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.ProductDescription_DEV000221", True))
        Me.MemoEditDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "ProductDescription_DEV000221"))
        Me.MemoEditDescription.Location = New System.Drawing.Point(708, 75)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditDescription, System.Drawing.Color.Empty)
        Me.MemoEditDescription.Size = New System.Drawing.Size(272, 144)
        Me.MemoEditDescription.TabIndex = 80
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSalePrice
        '
        Me.lblSalePrice.Enabled = False
        Me.lblSalePrice.Location = New System.Drawing.Point(263, 101)
        Me.lblSalePrice.Name = "lblSalePrice"
        Me.lblSalePrice.Size = New System.Drawing.Size(80, 13)
        Me.lblSalePrice.TabIndex = 79
        Me.lblSalePrice.Text = "3DCartSale Price"
        '
        'TextEditSalePrice
        '
        Me.TextEditSalePrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.SalePrice_DEV000221", True))
        Me.TextEditSalePrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditSalePrice, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "SalePrice_DEV000221"))
        Me.TextEditSalePrice.Location = New System.Drawing.Point(349, 97)
        Me.TextEditSalePrice.Name = "TextEditSalePrice"
        Me.TextEditSalePrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditSalePrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditSalePrice.Properties.AutoHeight = False
        Me.TextEditSalePrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditSalePrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditSalePrice, System.Drawing.Color.Empty)
        Me.TextEditSalePrice.Size = New System.Drawing.Size(82, 22)
        Me.TextEditSalePrice.TabIndex = 78
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditSalePrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GridControlProperties
        '
        Me.GridControlProperties.DataMember = "Inventory3DCartTagDetails_DEV000221"
        Me.GridControlProperties.DataSource = Me.Inventory3DCartSettingsSectionGateway
        Me.GridControlProperties.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlProperties, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.Location = New System.Drawing.Point(3, 264)
        Me.GridControlProperties.MainView = Me.GridViewProperties
        Me.GridControlProperties.Name = "GridControlProperties"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlProperties, System.Drawing.Color.Empty)
        Me.GridControlProperties.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeDataType, Me.rbeDateEdit, Me.rbeTextEdit, Me.rbeYesNoEdit, Me.rbeMemoEdit, Me.rbeSelectEdit})
        Me.GridControlProperties.Size = New System.Drawing.Size(522, 221)
        Me.GridControlProperties.TabIndex = 73
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewProperties})
        '
        'GridViewProperties
        '
        Me.GridViewProperties.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPropertiesItemCode_DEV000221, Me.colPropertiesName_DEV000221, Me.colPropertiesDataType_DEV000221, Me.colPropertiesDateValue_DEV000221, Me.colPropertiesNumericValue_DEV000221, Me.colPropertiesTextValue, Me.colPropertiesMemoValue, Me.colPropertiesDisplayValue, Me.colAttributeID_DEV000221, Me.colAttributeHasSelectValues_DEV000221})
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
        Me.colPropertiesName_DEV000221.Width = 157
        '
        'colPropertiesDataType_DEV000221
        '
        Me.colPropertiesDataType_DEV000221.Caption = "Format"
        Me.colPropertiesDataType_DEV000221.ColumnEdit = Me.rbeDataType
        Me.colPropertiesDataType_DEV000221.FieldName = "TagDataType_DEV000221"
        Me.colPropertiesDataType_DEV000221.Name = "colPropertiesDataType_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDataType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDataType_DEV000221.Visible = True
        Me.colPropertiesDataType_DEV000221.VisibleIndex = 1
        Me.colPropertiesDataType_DEV000221.Width = 78
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
        Me.colPropertiesDisplayValue.VisibleIndex = 2
        Me.colPropertiesDisplayValue.Width = 275
        '
        'colAttributeID_DEV000221
        '
        Me.colAttributeID_DEV000221.Caption = "Attribute ID"
        Me.colAttributeID_DEV000221.FieldName = "AttributeID_DEV000221"
        Me.colAttributeID_DEV000221.Name = "colAttributeID_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeID_DEV000221.Width = 20
        '
        'colAttributeHasSelectValues_DEV000221
        '
        Me.colAttributeHasSelectValues_DEV000221.Caption = "Attribute Has Select Values"
        Me.colAttributeHasSelectValues_DEV000221.FieldName = "AttributeHasSelectValues_DEV000221"
        Me.colAttributeHasSelectValues_DEV000221.Name = "colAttributeHasSelectValues_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeHasSelectValues_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeHasSelectValues_DEV000221.Width = 20
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
        'rbeYesNoEdit
        '
        Me.rbeYesNoEdit.AutoHeight = False
        Me.rbeYesNoEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeYesNoEdit.Items.AddRange(New Object() {"Y", "N"})
        Me.rbeYesNoEdit.Name = "rbeYesNoEdit"
        Me.rbeYesNoEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeMemoEdit
        '
        Me.rbeMemoEdit.Name = "rbeMemoEdit"
        '
        'rbeSelectEdit
        '
        Me.rbeSelectEdit.AdditionalFilter = Nothing
        Me.rbeSelectEdit.AllowEdit = True
        Me.rbeSelectEdit.AutoHeight = False
        Me.rbeSelectEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeSelectEdit.ColumnDescriptions = Nothing
        Me.rbeSelectEdit.ColumnNames = Nothing
        Me.rbeSelectEdit.DataSource = Nothing
        Me.rbeSelectEdit.DataSourceColumns = Nothing
        Me.rbeSelectEdit.DefaultSort = Nothing
        Me.rbeSelectEdit.DisplayField = Nothing
        Me.rbeSelectEdit.IsMultiSelect = False
        Me.rbeSelectEdit.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.rbeSelectEdit.Name = "rbeSelectEdit"
        Me.rbeSelectEdit.TableName = Nothing
        Me.rbeSelectEdit.TargetDisplayField = Nothing
        Me.rbeSelectEdit.TargetValueMember = "DisplayedValue"
        Me.rbeSelectEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.rbeSelectEdit.UseCache = False
        Me.rbeSelectEdit.UseSpecifiedColumns = False
        Me.rbeSelectEdit.ValueMember = ""
        '
        'lbl3DCartSellingPrice
        '
        Me.lbl3DCartSellingPrice.Enabled = False
        Me.lbl3DCartSellingPrice.Location = New System.Drawing.Point(250, 37)
        Me.lbl3DCartSellingPrice.Name = "lbl3DCartSellingPrice"
        Me.lbl3DCartSellingPrice.Size = New System.Drawing.Size(93, 13)
        Me.lbl3DCartSellingPrice.TabIndex = 68
        Me.lbl3DCartSellingPrice.Text = "3DCart Selling Price"
        '
        'TextEdit3DCartSellingPrice
        '
        Me.TextEdit3DCartSellingPrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.SellingPrice_DEV000221", True))
        Me.TextEdit3DCartSellingPrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEdit3DCartSellingPrice, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "SellingPrice_DEV000221"))
        Me.TextEdit3DCartSellingPrice.Location = New System.Drawing.Point(349, 33)
        Me.TextEdit3DCartSellingPrice.Name = "TextEdit3DCartSellingPrice"
        Me.TextEdit3DCartSellingPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEdit3DCartSellingPrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEdit3DCartSellingPrice.Properties.AutoHeight = False
        Me.TextEdit3DCartSellingPrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEdit3DCartSellingPrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEdit3DCartSellingPrice, System.Drawing.Color.Empty)
        Me.TextEdit3DCartSellingPrice.Size = New System.Drawing.Size(82, 22)
        Me.TextEdit3DCartSellingPrice.TabIndex = 67
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEdit3DCartSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixItem
        '
        Me.LabelMatrixItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Location = New System.Drawing.Point(528, 132)
        Me.LabelMatrixItem.Name = "LabelMatrixItem"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.LabelMatrixItem, System.Drawing.Color.Empty)
        Me.LabelMatrixItem.Size = New System.Drawing.Size(137, 52)
        Me.LabelMatrixItem.TabIndex = 66
        Me.LabelMatrixItem.Text = "This is a Matrix Item.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and Title are set from the " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Matrix Group Item."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Visible = False
        '
        'LabelMatrixGroupItem
        '
        Me.LabelMatrixGroupItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Location = New System.Drawing.Point(528, 132)
        Me.LabelMatrixGroupItem.Name = "LabelMatrixGroupItem"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.LabelMatrixGroupItem, System.Drawing.Color.Empty)
        Me.LabelMatrixGroupItem.Size = New System.Drawing.Size(161, 52)
        Me.LabelMatrixGroupItem.TabIndex = 65
        Me.LabelMatrixGroupItem.Text = "This is a Matrix Group Item." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and Title are applied to" & _
    " every " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Matrix Item in the Matrix Group."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.Enabled = False
        Me.lblTitle.Location = New System.Drawing.Point(638, 35)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(60, 13)
        Me.lblTitle.TabIndex = 32
        Me.lblTitle.Text = "Product Title"
        '
        'lblProductName
        '
        Me.lblProductName.Enabled = False
        Me.lblProductName.Location = New System.Drawing.Point(631, 8)
        Me.lblProductName.Name = "lblProductName"
        Me.lblProductName.Size = New System.Drawing.Size(67, 13)
        Me.lblProductName.TabIndex = 31
        Me.lblProductName.Text = "Product Name"
        '
        'MemoEditTitle
        '
        Me.MemoEditTitle.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.ProductTitle_DEV000221", True))
        Me.MemoEditTitle.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditTitle, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "ProductTitle_DEV000221"))
        Me.MemoEditTitle.Location = New System.Drawing.Point(708, 32)
        Me.MemoEditTitle.Name = "MemoEditTitle"
        Me.MemoEditTitle.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditTitle.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditTitle, System.Drawing.Color.Empty)
        Me.MemoEditTitle.Size = New System.Drawing.Size(272, 37)
        Me.MemoEditTitle.TabIndex = 30
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditTitle, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditProductName
        '
        Me.TextEditProductName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.ProductName_DEV000221", True))
        Me.TextEditProductName.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "ProductName_DEV000221"))
        Me.TextEditProductName.Location = New System.Drawing.Point(708, 4)
        Me.TextEditProductName.Name = "TextEditProductName"
        Me.TextEditProductName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditProductName.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditProductName.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditProductName, System.Drawing.Color.Empty)
        Me.TextEditProductName.Size = New System.Drawing.Size(272, 22)
        Me.TextEditProductName.TabIndex = 29
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSelect3DCartStore
        '
        Me.lblSelect3DCartStore.Location = New System.Drawing.Point(5, 8)
        Me.lblSelect3DCartStore.Name = "lblSelect3DCartStore"
        Me.lblSelect3DCartStore.Size = New System.Drawing.Size(95, 13)
        Me.lblSelect3DCartStore.TabIndex = 9
        Me.lblSelect3DCartStore.Text = "Select 3DCart Store"
        '
        'cbeSelect3DCartStore
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeSelect3DCartStore, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSelect3DCartStore.Location = New System.Drawing.Point(130, 4)
        Me.cbeSelect3DCartStore.Name = "cbeSelect3DCartStore"
        Me.cbeSelect3DCartStore.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeSelect3DCartStore.Properties.Appearance.Options.UseBackColor = True
        Me.cbeSelect3DCartStore.Properties.AutoHeight = False
        Me.cbeSelect3DCartStore.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeSelect3DCartStore, System.Drawing.Color.Empty)
        Me.cbeSelect3DCartStore.Size = New System.Drawing.Size(184, 22)
        Me.cbeSelect3DCartStore.TabIndex = 8
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSelect3DCartStore, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TreeListCategories
        '
        Me.TreeListCategories.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colTreeListCategoryName, Me.colTreeListCategoryActive, Me.colTreeListCategoryID})
        Me.TreeListCategories.Enabled = False
        Me.TreeListCategories.KeyFieldName = "SourceCategoryID"
        Me.TreeListCategories.Location = New System.Drawing.Point(2, 33)
        Me.TreeListCategories.Name = "TreeListCategories"
        Me.TreeListCategories.ParentFieldName = "SourceParentID"
        Me.TreeListCategories.Size = New System.Drawing.Size(231, 200)
        Me.TreeListCategories.TabIndex = 7
        '
        'colTreeListCategoryName
        '
        Me.colTreeListCategoryName.Caption = "Category"
        Me.colTreeListCategoryName.FieldName = "SourceCategoryName"
        Me.colTreeListCategoryName.Name = "colTreeListCategoryName"
        Me.colTreeListCategoryName.OptionsColumn.AllowEdit = False
        Me.colTreeListCategoryName.OptionsColumn.FixedWidth = True
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
        Me.colTreeListCategoryActive.OptionsColumn.FixedWidth = True
        Me.colTreeListCategoryActive.Visible = True
        Me.colTreeListCategoryActive.VisibleIndex = 1
        Me.colTreeListCategoryActive.Width = 40
        '
        'colTreeListCategoryID
        '
        Me.colTreeListCategoryID.Caption = "Category ID"
        Me.colTreeListCategoryID.FieldName = "SourceCategoryID"
        Me.colTreeListCategoryID.Name = "colTreeListCategoryID"
        Me.colTreeListCategoryID.Width = 20
        '
        'CheckEditPublishOn3DCart
        '
        Me.CheckEditPublishOn3DCart.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221.Publish_DEV000221", True))
        Me.CheckEditPublishOn3DCart.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditPublishOn3DCart, New Interprise.Presentation.Base.DisplayText(Me.Inventory3DCartSettingsSectionGateway, "Inventory3DCartDetails_DEV000221", "Publish_DEV000221"))
        Me.CheckEditPublishOn3DCart.Location = New System.Drawing.Point(320, 5)
        Me.CheckEditPublishOn3DCart.Name = "CheckEditPublishOn3DCart"
        Me.CheckEditPublishOn3DCart.Properties.AutoHeight = False
        Me.CheckEditPublishOn3DCart.Properties.Caption = "Publish on this 3DCart Store"
        Me.CheckEditPublishOn3DCart.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.CheckEditPublishOn3DCart, System.Drawing.Color.Empty)
        Me.CheckEditPublishOn3DCart.Size = New System.Drawing.Size(188, 22)
        Me.CheckEditPublishOn3DCart.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditPublishOn3DCart, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'Inventory3DCartSettingsSectionLayoutGroup
        '
        Me.Inventory3DCartSettingsSectionLayoutGroup.CustomizationFormText = "Inventory3DCartSettingsSectionLayoutGroup"
        Me.Inventory3DCartSettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.Inventory3DCartSettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.Inventory3DCartSettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.Inventory3DCartSettingsSectionLayoutGroup.Name = "Inventory3DCartSettingsSectionLayoutGroup"
        Me.Inventory3DCartSettingsSectionLayoutGroup.Size = New System.Drawing.Size(993, 497)
        Me.Inventory3DCartSettingsSectionLayoutGroup.Text = "Inventory3DCartSettingsSectionLayoutGroup"
        Me.Inventory3DCartSettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelPublishOn3DCart
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(993, 497)
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
        Me.PanelControlDummy.Size = New System.Drawing.Size(993, 497)
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
        'Inventory3DCartSettingsSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.Inventory3DCartSettingsSectionExtendedLayout)
        Me.Controls.Add(Me.PanelControlDummy)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "Inventory3DCartSettingsSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(993, 497)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Inventory3DCartSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Inventory3DCartSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Inventory3DCartSettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelPublishOn3DCart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPublishOn3DCart.ResumeLayout(False)
        Me.PanelPublishOn3DCart.PerformLayout()
        CType(Me.cbe3DCartSalePriceSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbe3DCartPriceSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetCategoryProgress.ResumeLayout(False)
        Me.pnlGetCategoryProgress.PerformLayout()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditExtendedDesc.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditSalePrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeSelectEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit3DCartSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditTitle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeSelect3DCartStore.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditPublishOn3DCart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Inventory3DCartSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDummy.ResumeLayout(False)
        Me.PanelControlDummy.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents Inventory3DCartSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents Inventory3DCartSettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents Inventory3DCartSettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelPublishOn3DCart As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents CheckEditPublishOn3DCart As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents TreeListCategories As DevExpress.XtraTreeList.TreeList
    Friend WithEvents colTreeListCategoryActive As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryID As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents lblSelect3DCartStore As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSelect3DCartStore As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents MemoEditTitle As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents TextEditProductName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblProductName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelMatrixItem As System.Windows.Forms.Label
    Friend WithEvents LabelMatrixGroupItem As System.Windows.Forms.Label
    Friend WithEvents lbl3DCartSellingPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEdit3DCartSellingPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents GridControlProperties As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewProperties As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPropertiesItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesName_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesDataType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeDataType As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents colPropertiesDateValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesNumericValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesDisplayValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeTextEdit As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents rbeDateEdit As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents rbeYesNoEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents lblSalePrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditSalePrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblExtendedDesc As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditExtendedDesc As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents rbeMemoEdit As DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
    Friend WithEvents colPropertiesTextValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesMemoValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents chkPriceAsIS As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents pnlGetCategoryProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetCategoryProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lbl3DCartAttributes As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PanelControlDummy As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblDevelopment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblActivate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditQtyPublishingValue As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblQtyPublishingValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblQtyPublishing As DevExpress.XtraEditors.LabelControl
    Friend WithEvents RadioGroupQtyPublishing As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents rbeSelectEdit As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
    Friend WithEvents colAttributeID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeHasSelectValues_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents lbl3DCartPriceSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbe3DCartPriceSource As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents lbl3DCartSalePriceSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbe3DCartSalePriceSource As DevExpress.XtraEditors.ImageComboBoxEdit

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
