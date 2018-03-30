'===============================================================================
' Interprise Suite SDK
' Copyright © 2004-2008 Interprise Software Solutions Incorporated
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

#Region " InventoryChannelAdvSettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryChannelAdvSettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryChannelAdvSettingsSection))
        Me.InventoryChannelAdvSettingsSectionGateway = Me.ImportExportDataset
        Me.InventoryChannelAdvSettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl()
        Me.PanelPublishOnChannelAdv = New DevExpress.XtraEditors.PanelControl()
        Me.GridControlASIN = New DevExpress.XtraGrid.GridControl()
        Me.GridViewASIN = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colASIN_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.lblChannelAdvAttributes = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditQtyPublishingValue = New DevExpress.XtraEditors.TextEdit()
        Me.lblQtyPublishingValue = New DevExpress.XtraEditors.LabelControl()
        Me.lblQtyPublishing = New DevExpress.XtraEditors.LabelControl()
        Me.RadioGroupQtyPublishing = New DevExpress.XtraEditors.RadioGroup()
        Me.chkPriceAsIS = New DevExpress.XtraEditors.CheckEdit()
        Me.lblDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.lblShortDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditShortDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.GridControlProperties = New DevExpress.XtraGrid.GridControl()
        Me.GridViewProperties = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colPropertiesItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesName_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesMemoValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeMemoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit()
        Me.lblChannelAdvSellingPrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditChannelAdvSellingPrice = New DevExpress.XtraEditors.TextEdit()
        Me.LabelMatrixItem = New System.Windows.Forms.Label()
        Me.LabelMatrixGroupItem = New System.Windows.Forms.Label()
        Me.lblSubTitle = New DevExpress.XtraEditors.LabelControl()
        Me.lblProductTitle = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditSubTitle = New DevExpress.XtraEditors.TextEdit()
        Me.TextEditProductTitle = New DevExpress.XtraEditors.TextEdit()
        Me.lblSelectChannelAdvAccountID = New DevExpress.XtraEditors.LabelControl()
        Me.cbeSelectChannelAdvAccountID = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.CheckEditPublishOnChannelAdv = New DevExpress.XtraEditors.CheckEdit()
        Me.InventoryChannelAdvSettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.PanelControlDummy = New DevExpress.XtraEditors.PanelControl()
        Me.lblDevelopment = New DevExpress.XtraEditors.LabelControl()
        Me.lblActivate = New DevExpress.XtraEditors.LabelControl()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryChannelAdvSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryChannelAdvSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelPublishOnChannelAdv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPublishOnChannelAdv.SuspendLayout()
        CType(Me.GridControlASIN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewASIN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditShortDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditChannelAdvSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditSubTitle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditProductTitle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeSelectChannelAdvAccountID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditPublishOnChannelAdv.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryChannelAdvSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDummy.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventoryChannelAdvSettingsSectionGateway
        '
        Me.InventoryChannelAdvSettingsSectionGateway.DataSetName = "InventoryChannelAdvSettingsSectionDataset"
        Me.InventoryChannelAdvSettingsSectionGateway.Instantiate = False
        Me.InventoryChannelAdvSettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventoryChannelAdvSettingsSectionExtendedLayout
        '
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.Controls.Add(Me.PanelPublishOnChannelAdv)
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.Name = "InventoryChannelAdvSettingsSectionExtendedLayout"
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.Root = Me.InventoryChannelAdvSettingsSectionLayoutGroup
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.Size = New System.Drawing.Size(974, 497)
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.TabIndex = 0
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.Text = "InventoryChannelAdvSettingsSectionExtendedLayout"
        '
        'PanelPublishOnChannelAdv
        '
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.GridControlASIN)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblChannelAdvAttributes)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.TextEditQtyPublishingValue)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblQtyPublishingValue)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblQtyPublishing)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.RadioGroupQtyPublishing)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.chkPriceAsIS)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblDescription)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.MemoEditDescription)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblShortDescription)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.MemoEditShortDescription)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.GridControlProperties)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblChannelAdvSellingPrice)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.TextEditChannelAdvSellingPrice)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.LabelMatrixItem)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.LabelMatrixGroupItem)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblSubTitle)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblProductTitle)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.TextEditSubTitle)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.TextEditProductTitle)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.lblSelectChannelAdvAccountID)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.cbeSelectChannelAdvAccountID)
        Me.PanelPublishOnChannelAdv.Controls.Add(Me.CheckEditPublishOnChannelAdv)
        Me.PanelPublishOnChannelAdv.Location = New System.Drawing.Point(3, 3)
        Me.PanelPublishOnChannelAdv.Name = "PanelPublishOnChannelAdv"
        Me.PanelPublishOnChannelAdv.Size = New System.Drawing.Size(968, 491)
        Me.PanelPublishOnChannelAdv.TabIndex = 4
        '
        'GridControlASIN
        '
        Me.GridControlASIN.DataMember = "InventoryAmazonASIN_DEV000221"
        Me.GridControlASIN.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlASIN, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlASIN, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlASIN.Location = New System.Drawing.Point(685, 348)
        Me.GridControlASIN.MainView = Me.GridViewASIN
        Me.GridControlASIN.Name = "GridControlASIN"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlASIN, False)
        Me.GridControlASIN.Size = New System.Drawing.Size(272, 62)
        Me.GridControlASIN.TabIndex = 118
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlASIN, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlASIN.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewASIN})
        '
        'GridViewASIN
        '
        Me.GridViewASIN.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colItemCode_DEV000221, Me.colASIN_DEV000221})
        Me.GridViewASIN.GridControl = Me.GridControlASIN
        Me.GridViewASIN.Name = "GridViewASIN"
        Me.GridViewASIN.OptionsView.ShowGroupPanel = False
        '
        'colItemCode_DEV000221
        '
        Me.colItemCode_DEV000221.Caption = "Item Code"
        Me.colItemCode_DEV000221.FieldName = "ItemCode_DEV000221"
        Me.colItemCode_DEV000221.Name = "colItemCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colItemCode_DEV000221.Width = 20
        '
        'colASIN_DEV000221
        '
        Me.colASIN_DEV000221.Caption = "Amazon ASIN"
        Me.colASIN_DEV000221.FieldName = "ASIN_DEV000221"
        Me.colASIN_DEV000221.Name = "colASIN_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colASIN_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colASIN_DEV000221.Visible = True
        Me.colASIN_DEV000221.VisibleIndex = 0
        '
        'lblChannelAdvAttributes
        '
        Me.lblChannelAdvAttributes.Location = New System.Drawing.Point(5, 245)
        Me.lblChannelAdvAttributes.Name = "lblChannelAdvAttributes"
        Me.lblChannelAdvAttributes.Size = New System.Drawing.Size(169, 13)
        Me.lblChannelAdvAttributes.TabIndex = 117
        Me.lblChannelAdvAttributes.Text = "Channel Advisor Product Attributes"
        '
        'TextEditQtyPublishingValue
        '
        Me.TextEditQtyPublishingValue.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.QtyPublishingValue_DEV000221", True))
        Me.TextEditQtyPublishingValue.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditQtyPublishingValue.Location = New System.Drawing.Point(564, 455)
        Me.TextEditQtyPublishingValue.Name = "TextEditQtyPublishingValue"
        Me.TextEditQtyPublishingValue.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditQtyPublishingValue, False)
        Me.TextEditQtyPublishingValue.Size = New System.Drawing.Size(75, 22)
        Me.TextEditQtyPublishingValue.TabIndex = 116
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblQtyPublishingValue
        '
        Me.lblQtyPublishingValue.Enabled = False
        Me.lblQtyPublishingValue.Location = New System.Drawing.Point(564, 436)
        Me.lblQtyPublishingValue.Name = "lblQtyPublishingValue"
        Me.lblQtyPublishingValue.Size = New System.Drawing.Size(75, 13)
        Me.lblQtyPublishingValue.TabIndex = 115
        Me.lblQtyPublishingValue.Text = "Value to Publish"
        '
        'lblQtyPublishing
        '
        Me.lblQtyPublishing.Enabled = False
        Me.lblQtyPublishing.Location = New System.Drawing.Point(544, 329)
        Me.lblQtyPublishing.Name = "lblQtyPublishing"
        Me.lblQtyPublishing.Size = New System.Drawing.Size(121, 13)
        Me.lblQtyPublishing.TabIndex = 114
        Me.lblQtyPublishing.Text = "Stock Quantity Publishing"
        '
        'RadioGroupQtyPublishing
        '
        Me.RadioGroupQtyPublishing.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.QtyPublishingType_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.RadioGroupQtyPublishing.Location = New System.Drawing.Point(555, 348)
        Me.RadioGroupQtyPublishing.Name = "RadioGroupQtyPublishing"
        Me.RadioGroupQtyPublishing.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("None", "None"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Fixed", "Fixed Value"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Percent", "% of Total")})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.RadioGroupQtyPublishing, False)
        Me.RadioGroupQtyPublishing.Size = New System.Drawing.Size(100, 83)
        Me.RadioGroupQtyPublishing.TabIndex = 113
        Me.ExtendControlProperty.SetTextDisplay(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkPriceAsIS
        '
        Me.chkPriceAsIS.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.UseISPricingDetail_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.chkPriceAsIS, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkPriceAsIS.Location = New System.Drawing.Point(461, 34)
        Me.chkPriceAsIS.Name = "chkPriceAsIS"
        Me.chkPriceAsIS.Properties.AutoHeight = False
        Me.chkPriceAsIS.Properties.Caption = "Use Std. price"
        Me.chkPriceAsIS.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.chkPriceAsIS, False)
        Me.chkPriceAsIS.Size = New System.Drawing.Size(109, 22)
        Me.chkPriceAsIS.TabIndex = 112
        Me.ExtendControlProperty.SetTextDisplay(Me.chkPriceAsIS, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblDescription
        '
        Me.lblDescription.Enabled = False
        Me.lblDescription.Location = New System.Drawing.Point(626, 118)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(53, 13)
        Me.lblDescription.TabIndex = 111
        Me.lblDescription.Text = "Description"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.ProductDescription_DEV000221", True))
        Me.MemoEditDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditDescription.Location = New System.Drawing.Point(685, 115)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditDescription, False)
        Me.MemoEditDescription.Size = New System.Drawing.Size(272, 199)
        Me.MemoEditDescription.TabIndex = 110
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblShortDescription
        '
        Me.lblShortDescription.Enabled = False
        Me.lblShortDescription.Location = New System.Drawing.Point(597, 63)
        Me.lblShortDescription.Name = "lblShortDescription"
        Me.lblShortDescription.Size = New System.Drawing.Size(82, 13)
        Me.lblShortDescription.TabIndex = 109
        Me.lblShortDescription.Text = "Short Description"
        '
        'MemoEditShortDescription
        '
        Me.MemoEditShortDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.ProductShortDescription_DEV000221", True))
        Me.MemoEditShortDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditShortDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditShortDescription.Location = New System.Drawing.Point(685, 60)
        Me.MemoEditShortDescription.Name = "MemoEditShortDescription"
        Me.MemoEditShortDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditShortDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditShortDescription, False)
        Me.MemoEditShortDescription.Size = New System.Drawing.Size(272, 49)
        Me.MemoEditShortDescription.TabIndex = 108
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditShortDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GridControlProperties
        '
        Me.GridControlProperties.DataMember = "InventoryChannelAdvTagDetails_DEV000221"
        Me.GridControlProperties.DataSource = Me.InventoryChannelAdvSettingsSectionGateway
        Me.GridControlProperties.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlProperties, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.Location = New System.Drawing.Point(5, 264)
        Me.GridControlProperties.MainView = Me.GridViewProperties
        Me.GridControlProperties.Name = "GridControlProperties"
        Me.GridControlProperties.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeMemoEdit})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlProperties, False)
        Me.GridControlProperties.Size = New System.Drawing.Size(522, 221)
        Me.GridControlProperties.TabIndex = 107
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewProperties})
        '
        'GridViewProperties
        '
        Me.GridViewProperties.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPropertiesItemCode_DEV000221, Me.colPropertiesName_DEV000221, Me.colPropertiesMemoValue})
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
        Me.colPropertiesName_DEV000221.Width = 200
        '
        'colPropertiesMemoValue
        '
        Me.colPropertiesMemoValue.Caption = "Value"
        Me.colPropertiesMemoValue.FieldName = "TagMemoField_DEV000221"
        Me.colPropertiesMemoValue.Name = "colPropertiesMemoValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesMemoValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesMemoValue.Visible = True
        Me.colPropertiesMemoValue.VisibleIndex = 1
        Me.colPropertiesMemoValue.Width = 304
        '
        'rbeMemoEdit
        '
        Me.rbeMemoEdit.Name = "rbeMemoEdit"
        '
        'lblChannelAdvSellingPrice
        '
        Me.lblChannelAdvSellingPrice.Enabled = False
        Me.lblChannelAdvSellingPrice.Location = New System.Drawing.Point(230, 37)
        Me.lblChannelAdvSellingPrice.Name = "lblChannelAdvSellingPrice"
        Me.lblChannelAdvSellingPrice.Size = New System.Drawing.Size(137, 13)
        Me.lblChannelAdvSellingPrice.TabIndex = 106
        Me.lblChannelAdvSellingPrice.Text = "Channel Advisor Selling Price"
        '
        'TextEditChannelAdvSellingPrice
        '
        Me.TextEditChannelAdvSellingPrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.SellingPrice_DEV000221", True))
        Me.TextEditChannelAdvSellingPrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditChannelAdvSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditChannelAdvSellingPrice.Location = New System.Drawing.Point(373, 33)
        Me.TextEditChannelAdvSellingPrice.Name = "TextEditChannelAdvSellingPrice"
        Me.TextEditChannelAdvSellingPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditChannelAdvSellingPrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditChannelAdvSellingPrice.Properties.AutoHeight = False
        Me.TextEditChannelAdvSellingPrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditChannelAdvSellingPrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditChannelAdvSellingPrice, False)
        Me.TextEditChannelAdvSellingPrice.Size = New System.Drawing.Size(82, 22)
        Me.TextEditChannelAdvSellingPrice.TabIndex = 105
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditChannelAdvSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixItem
        '
        Me.LabelMatrixItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Location = New System.Drawing.Point(423, 98)
        Me.LabelMatrixItem.Name = "LabelMatrixItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixItem, False)
        Me.LabelMatrixItem.Size = New System.Drawing.Size(142, 52)
        Me.LabelMatrixItem.TabIndex = 104
        Me.LabelMatrixItem.Text = "This is a Matrix Item.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and Description are set " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "fr" & _
            "om the Matrix Group Item."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Visible = False
        '
        'LabelMatrixGroupItem
        '
        Me.LabelMatrixGroupItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Location = New System.Drawing.Point(423, 98)
        Me.LabelMatrixGroupItem.Name = "LabelMatrixGroupItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixGroupItem, False)
        Me.LabelMatrixGroupItem.Size = New System.Drawing.Size(140, 65)
        Me.LabelMatrixGroupItem.TabIndex = 103
        Me.LabelMatrixGroupItem.Text = "This is a Matrix Group Item." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and Description are appl" & _
            "ied " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "to every Matrix Item in the " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Matrix Group."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Visible = False
        '
        'lblSubTitle
        '
        Me.lblSubTitle.Enabled = False
        Me.lblSubTitle.Location = New System.Drawing.Point(637, 37)
        Me.lblSubTitle.Name = "lblSubTitle"
        Me.lblSubTitle.Size = New System.Drawing.Size(42, 13)
        Me.lblSubTitle.TabIndex = 102
        Me.lblSubTitle.Text = "Sub-Title"
        '
        'lblProductTitle
        '
        Me.lblProductTitle.Enabled = False
        Me.lblProductTitle.Location = New System.Drawing.Point(619, 7)
        Me.lblProductTitle.Name = "lblProductTitle"
        Me.lblProductTitle.Size = New System.Drawing.Size(60, 13)
        Me.lblProductTitle.TabIndex = 101
        Me.lblProductTitle.Text = "Product Title"
        '
        'TextEditSubTitle
        '
        Me.TextEditSubTitle.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.ProductSubTitle_DEV000221", True))
        Me.TextEditSubTitle.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditSubTitle, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditSubTitle.Location = New System.Drawing.Point(685, 32)
        Me.TextEditSubTitle.Name = "TextEditSubTitle"
        Me.TextEditSubTitle.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditSubTitle.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditSubTitle, False)
        Me.TextEditSubTitle.Size = New System.Drawing.Size(272, 22)
        Me.TextEditSubTitle.TabIndex = 100
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditSubTitle, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditProductTitle
        '
        Me.TextEditProductTitle.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.ProductName_DEV000221", True))
        Me.TextEditProductTitle.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditProductTitle, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditProductTitle.Location = New System.Drawing.Point(685, 4)
        Me.TextEditProductTitle.Name = "TextEditProductTitle"
        Me.TextEditProductTitle.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditProductTitle.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditProductTitle.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditProductTitle, False)
        Me.TextEditProductTitle.Size = New System.Drawing.Size(272, 22)
        Me.TextEditProductTitle.TabIndex = 99
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditProductTitle, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSelectChannelAdvAccountID
        '
        Me.lblSelectChannelAdvAccountID.Location = New System.Drawing.Point(5, 8)
        Me.lblSelectChannelAdvAccountID.Name = "lblSelectChannelAdvAccountID"
        Me.lblSelectChannelAdvAccountID.Size = New System.Drawing.Size(152, 13)
        Me.lblSelectChannelAdvAccountID.TabIndex = 98
        Me.lblSelectChannelAdvAccountID.Text = "Select Channel Advisor Account"
        '
        'cbeSelectChannelAdvAccountID
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeSelectChannelAdvAccountID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSelectChannelAdvAccountID.Location = New System.Drawing.Point(166, 3)
        Me.cbeSelectChannelAdvAccountID.Name = "cbeSelectChannelAdvAccountID"
        Me.cbeSelectChannelAdvAccountID.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeSelectChannelAdvAccountID.Properties.Appearance.Options.UseBackColor = True
        Me.cbeSelectChannelAdvAccountID.Properties.AutoHeight = False
        Me.cbeSelectChannelAdvAccountID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.cbeSelectChannelAdvAccountID, False)
        Me.cbeSelectChannelAdvAccountID.Size = New System.Drawing.Size(184, 22)
        Me.cbeSelectChannelAdvAccountID.TabIndex = 97
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSelectChannelAdvAccountID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'CheckEditPublishOnChannelAdv
        '
        Me.CheckEditPublishOnChannelAdv.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryChannelAdvSettingsSectionGateway, "InventoryChannelAdvDetails_DEV000221.Publish_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditPublishOnChannelAdv, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckEditPublishOnChannelAdv.Location = New System.Drawing.Point(356, 5)
        Me.CheckEditPublishOnChannelAdv.Name = "CheckEditPublishOnChannelAdv"
        Me.CheckEditPublishOnChannelAdv.Properties.AutoHeight = False
        Me.CheckEditPublishOnChannelAdv.Properties.Caption = "Publish on Channel Advisor"
        Me.CheckEditPublishOnChannelAdv.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.CheckEditPublishOnChannelAdv.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.CheckEditPublishOnChannelAdv, False)
        Me.CheckEditPublishOnChannelAdv.Size = New System.Drawing.Size(162, 22)
        Me.CheckEditPublishOnChannelAdv.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditPublishOnChannelAdv, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'InventoryChannelAdvSettingsSectionLayoutGroup
        '
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.CustomizationFormText = "InventoryChannelAdvSettingsSectionLayoutGroup"
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.Name = "InventoryChannelAdvSettingsSectionLayoutGroup"
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.Size = New System.Drawing.Size(974, 497)
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.Text = "InventoryChannelAdvSettingsSectionLayoutGroup"
        Me.InventoryChannelAdvSettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelPublishOnChannelAdv
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
        'InventoryChannelAdvSettingsSection
        '
        Me.Controls.Add(Me.InventoryChannelAdvSettingsSectionExtendedLayout)
        Me.Controls.Add(Me.PanelControlDummy)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventoryChannelAdvSettingsSection"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me, False)
        Me.Size = New System.Drawing.Size(974, 497)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryChannelAdvSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryChannelAdvSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventoryChannelAdvSettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelPublishOnChannelAdv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPublishOnChannelAdv.ResumeLayout(False)
        Me.PanelPublishOnChannelAdv.PerformLayout()
        CType(Me.GridControlASIN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewASIN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditShortDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditChannelAdvSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditSubTitle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditProductTitle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeSelectChannelAdvAccountID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditPublishOnChannelAdv.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryChannelAdvSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDummy.ResumeLayout(False)
        Me.PanelControlDummy.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventoryChannelAdvSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventoryChannelAdvSettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventoryChannelAdvSettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelPublishOnChannelAdv As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents CheckEditPublishOnChannelAdv As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents TextEditQtyPublishingValue As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblQtyPublishingValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblQtyPublishing As DevExpress.XtraEditors.LabelControl
    Friend WithEvents RadioGroupQtyPublishing As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents chkPriceAsIS As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblShortDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditShortDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents GridControlProperties As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewProperties As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPropertiesItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesName_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesMemoValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeMemoEdit As DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
    Friend WithEvents lblChannelAdvSellingPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditChannelAdvSellingPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelMatrixItem As System.Windows.Forms.Label
    Friend WithEvents LabelMatrixGroupItem As System.Windows.Forms.Label
    Friend WithEvents lblSubTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblProductTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditSubTitle As DevExpress.XtraEditors.TextEdit
    Friend WithEvents TextEditProductTitle As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSelectChannelAdvAccountID As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSelectChannelAdvAccountID As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents lblChannelAdvAttributes As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GridControlASIN As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewASIN As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colASIN_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents PanelControlDummy As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblDevelopment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblActivate As DevExpress.XtraEditors.LabelControl

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
