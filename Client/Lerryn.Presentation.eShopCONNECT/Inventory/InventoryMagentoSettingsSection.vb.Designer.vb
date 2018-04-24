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

#Region " InventoryMagentoSettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryMagentoSettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryMagentoSettingsSection))
        Me.InventoryMagentoSettingsSectionGateway = Me.ImportExportDataset
        Me.InventoryMagentoSettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.PanelPublishOnMagento = New DevExpress.XtraEditors.PanelControl()
        Me.lblMagentoSpecialPriceSource = New DevExpress.XtraEditors.LabelControl()
        Me.cbeMagentoSpecialPriceSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblMagentoPriceSource = New DevExpress.XtraEditors.LabelControl()
        Me.cbeMagentoPriceSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.TextEditQtyPublishingValue = New DevExpress.XtraEditors.TextEdit()
        Me.lblQtyPublishingValue = New DevExpress.XtraEditors.LabelControl()
        Me.lblQtyPublishing = New DevExpress.XtraEditors.LabelControl()
        Me.RadioGroupQtyPublishing = New DevExpress.XtraEditors.RadioGroup()
        Me.lblAttributeSet = New DevExpress.XtraEditors.LabelControl()
        Me.ImageComboBoxEditAttributeSet = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblMagentoAttributes = New DevExpress.XtraEditors.LabelControl()
        Me.pnlGetCategoryProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetCategoryProgress = New DevExpress.XtraEditors.LabelControl()
        Me.pnlGetAttributeProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetAttributeProgress = New DevExpress.XtraEditors.LabelControl()
        Me.chkPriceAsIS = New DevExpress.XtraEditors.CheckEdit()
        Me.lblStatus = New DevExpress.XtraEditors.LabelControl()
        Me.ImageComboBoxEditStatus = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblVisibility = New DevExpress.XtraEditors.LabelControl()
        Me.ImageComboBoxEditVisibility = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblInDepth = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditInDepth = New DevExpress.XtraEditors.MemoEdit()
        Me.lblDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.lblSpecialPrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditSpecialPrice = New DevExpress.XtraEditors.TextEdit()
        Me.lblSpecialTo = New DevExpress.XtraEditors.LabelControl()
        Me.lblSpecialFrom = New DevExpress.XtraEditors.LabelControl()
        Me.lblShowAsNewTo = New DevExpress.XtraEditors.LabelControl()
        Me.lblShowAsNewFrom = New DevExpress.XtraEditors.LabelControl()
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
        Me.DateEditSpecialTo = New DevExpress.XtraEditors.DateEdit()
        Me.DateEditSpecialFrom = New DevExpress.XtraEditors.DateEdit()
        Me.DateEditShowAsNewTo = New DevExpress.XtraEditors.DateEdit()
        Me.DateEditShowAsNewFrom = New DevExpress.XtraEditors.DateEdit()
        Me.lblMagentoSellingPrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditMagentoSellingPrice = New DevExpress.XtraEditors.TextEdit()
        Me.LabelMatrixItem = New System.Windows.Forms.Label()
        Me.LabelMatrixGroupItem = New System.Windows.Forms.Label()
        Me.lblShortDescription = New DevExpress.XtraEditors.LabelControl()
        Me.lblProductName = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditShortDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.TextEditProductName = New DevExpress.XtraEditors.TextEdit()
        Me.lblSelectMagentoInstance = New DevExpress.XtraEditors.LabelControl()
        Me.cbeSelectMagentoInstance = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.TreeListCategories = New DevExpress.XtraTreeList.TreeList()
        Me.colTreeListCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryActive = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryID = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.CheckEditPublishOnMagento = New DevExpress.XtraEditors.CheckEdit()
        Me.InventoryMagentoSettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.PanelControlDummy = New DevExpress.XtraEditors.PanelControl()
        Me.lblDevelopment = New DevExpress.XtraEditors.LabelControl()
        Me.lblActivate = New DevExpress.XtraEditors.LabelControl()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryMagentoSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryMagentoSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventoryMagentoSettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelPublishOnMagento, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPublishOnMagento.SuspendLayout()
        CType(Me.cbeMagentoSpecialPriceSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeMagentoPriceSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageComboBoxEditAttributeSet.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetCategoryProgress.SuspendLayout()
        CType(Me.pnlGetAttributeProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetAttributeProgress.SuspendLayout()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageComboBoxEditStatus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageComboBoxEditVisibility.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditInDepth.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditSpecialPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeSelectEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialTo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialFrom.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditShowAsNewTo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditShowAsNewTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditShowAsNewFrom.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditShowAsNewFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditMagentoSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditShortDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeSelectMagentoInstance.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditPublishOnMagento.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryMagentoSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDummy.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventoryMagentoSettingsSectionGateway
        '
        Me.InventoryMagentoSettingsSectionGateway.DataSetName = "InventoryMagentoSettingsSectionDataset"
        Me.InventoryMagentoSettingsSectionGateway.Instantiate = False
        Me.InventoryMagentoSettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventoryMagentoSettingsSectionExtendedLayout
        '
        Me.InventoryMagentoSettingsSectionExtendedLayout.AllowCustomizationMenu = False
        Me.InventoryMagentoSettingsSectionExtendedLayout.Controls.Add(Me.PanelPublishOnMagento)
        Me.InventoryMagentoSettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventoryMagentoSettingsSectionExtendedLayout.IsResetSection = False
        Me.InventoryMagentoSettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventoryMagentoSettingsSectionExtendedLayout.Name = "InventoryMagentoSettingsSectionExtendedLayout"
        Me.InventoryMagentoSettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventoryMagentoSettingsSectionExtendedLayout.PluginContainerDataset = Nothing
        Me.InventoryMagentoSettingsSectionExtendedLayout.Root = Me.InventoryMagentoSettingsSectionLayoutGroup
        Me.InventoryMagentoSettingsSectionExtendedLayout.Size = New System.Drawing.Size(993, 497)
        Me.InventoryMagentoSettingsSectionExtendedLayout.TabIndex = 0
        Me.InventoryMagentoSettingsSectionExtendedLayout.Text = "InventoryMagentoSettingsSectionExtendedLayout"
        Me.InventoryMagentoSettingsSectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'PanelPublishOnMagento
        '
        Me.PanelPublishOnMagento.Controls.Add(Me.lblMagentoSpecialPriceSource)
        Me.PanelPublishOnMagento.Controls.Add(Me.cbeMagentoSpecialPriceSource)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblMagentoPriceSource)
        Me.PanelPublishOnMagento.Controls.Add(Me.cbeMagentoPriceSource)
        Me.PanelPublishOnMagento.Controls.Add(Me.TextEditQtyPublishingValue)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblQtyPublishingValue)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblQtyPublishing)
        Me.PanelPublishOnMagento.Controls.Add(Me.RadioGroupQtyPublishing)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblAttributeSet)
        Me.PanelPublishOnMagento.Controls.Add(Me.ImageComboBoxEditAttributeSet)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblMagentoAttributes)
        Me.PanelPublishOnMagento.Controls.Add(Me.pnlGetCategoryProgress)
        Me.PanelPublishOnMagento.Controls.Add(Me.pnlGetAttributeProgress)
        Me.PanelPublishOnMagento.Controls.Add(Me.chkPriceAsIS)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblStatus)
        Me.PanelPublishOnMagento.Controls.Add(Me.ImageComboBoxEditStatus)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblVisibility)
        Me.PanelPublishOnMagento.Controls.Add(Me.ImageComboBoxEditVisibility)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblInDepth)
        Me.PanelPublishOnMagento.Controls.Add(Me.MemoEditInDepth)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblDescription)
        Me.PanelPublishOnMagento.Controls.Add(Me.MemoEditDescription)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblSpecialPrice)
        Me.PanelPublishOnMagento.Controls.Add(Me.TextEditSpecialPrice)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblSpecialTo)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblSpecialFrom)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblShowAsNewTo)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblShowAsNewFrom)
        Me.PanelPublishOnMagento.Controls.Add(Me.GridControlProperties)
        Me.PanelPublishOnMagento.Controls.Add(Me.DateEditSpecialTo)
        Me.PanelPublishOnMagento.Controls.Add(Me.DateEditSpecialFrom)
        Me.PanelPublishOnMagento.Controls.Add(Me.DateEditShowAsNewTo)
        Me.PanelPublishOnMagento.Controls.Add(Me.DateEditShowAsNewFrom)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblMagentoSellingPrice)
        Me.PanelPublishOnMagento.Controls.Add(Me.TextEditMagentoSellingPrice)
        Me.PanelPublishOnMagento.Controls.Add(Me.LabelMatrixItem)
        Me.PanelPublishOnMagento.Controls.Add(Me.LabelMatrixGroupItem)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblShortDescription)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblProductName)
        Me.PanelPublishOnMagento.Controls.Add(Me.MemoEditShortDescription)
        Me.PanelPublishOnMagento.Controls.Add(Me.TextEditProductName)
        Me.PanelPublishOnMagento.Controls.Add(Me.lblSelectMagentoInstance)
        Me.PanelPublishOnMagento.Controls.Add(Me.cbeSelectMagentoInstance)
        Me.PanelPublishOnMagento.Controls.Add(Me.TreeListCategories)
        Me.PanelPublishOnMagento.Controls.Add(Me.CheckEditPublishOnMagento)
        Me.PanelPublishOnMagento.Location = New System.Drawing.Point(3, 3)
        Me.PanelPublishOnMagento.Name = "PanelPublishOnMagento"
        Me.PanelPublishOnMagento.Size = New System.Drawing.Size(987, 491)
        Me.PanelPublishOnMagento.TabIndex = 4
        '
        'lblMagentoSpecialPriceSource
        '
        Me.lblMagentoSpecialPriceSource.Enabled = False
        Me.lblMagentoSpecialPriceSource.Location = New System.Drawing.Point(249, 177)
        Me.lblMagentoSpecialPriceSource.Name = "lblMagentoSpecialPriceSource"
        Me.lblMagentoSpecialPriceSource.Size = New System.Drawing.Size(94, 13)
        Me.lblMagentoSpecialPriceSource.TabIndex = 106
        Me.lblMagentoSpecialPriceSource.Text = "Special Price source"
        '
        'cbeMagentoSpecialPriceSource
        '
        Me.cbeMagentoSpecialPriceSource.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.SpecialPriceSource_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.cbeMagentoSpecialPriceSource, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "SpecialPriceSource_DEV000221"))
        Me.cbeMagentoSpecialPriceSource.Location = New System.Drawing.Point(349, 173)
        Me.cbeMagentoSpecialPriceSource.Name = "cbeMagentoSpecialPriceSource"
        Me.cbeMagentoSpecialPriceSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeMagentoSpecialPriceSource.Properties.Appearance.Options.UseFont = True
        Me.cbeMagentoSpecialPriceSource.Properties.AutoHeight = False
        Me.cbeMagentoSpecialPriceSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeMagentoSpecialPriceSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Wholesale Price", "W", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Retail Price", "R", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Promotional Price", "P", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("This form", "N", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeMagentoSpecialPriceSource, System.Drawing.Color.Empty)
        Me.cbeMagentoSpecialPriceSource.Size = New System.Drawing.Size(142, 22)
        Me.cbeMagentoSpecialPriceSource.TabIndex = 105
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeMagentoSpecialPriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblMagentoPriceSource
        '
        Me.lblMagentoPriceSource.Enabled = False
        Me.lblMagentoPriceSource.Location = New System.Drawing.Point(252, 65)
        Me.lblMagentoPriceSource.Name = "lblMagentoPriceSource"
        Me.lblMagentoPriceSource.Size = New System.Drawing.Size(91, 13)
        Me.lblMagentoPriceSource.TabIndex = 104
        Me.lblMagentoPriceSource.Text = "Selling Price source"
        '
        'cbeMagentoPriceSource
        '
        Me.cbeMagentoPriceSource.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.SellingPriceSource_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.cbeMagentoPriceSource, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "SellingPriceSource_DEV000221"))
        Me.cbeMagentoPriceSource.Location = New System.Drawing.Point(349, 61)
        Me.cbeMagentoPriceSource.Name = "cbeMagentoPriceSource"
        Me.cbeMagentoPriceSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeMagentoPriceSource.Properties.Appearance.Options.UseFont = True
        Me.cbeMagentoPriceSource.Properties.AutoHeight = False
        Me.cbeMagentoPriceSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeMagentoPriceSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Wholesale Price", "W", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Retail Price", "R", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Sug Retail Price", "S", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeMagentoPriceSource, System.Drawing.Color.Empty)
        Me.cbeMagentoPriceSource.Size = New System.Drawing.Size(142, 22)
        Me.cbeMagentoPriceSource.TabIndex = 103
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeMagentoPriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditQtyPublishingValue
        '
        Me.TextEditQtyPublishingValue.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.QtyPublishingValue_DEV000221", True))
        Me.TextEditQtyPublishingValue.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "QtyPublishingValue_DEV000221"))
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
        Me.RadioGroupQtyPublishing.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.QtyPublishingType_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "QtyPublishingType_DEV000221"))
        Me.RadioGroupQtyPublishing.Location = New System.Drawing.Point(555, 348)
        Me.RadioGroupQtyPublishing.Name = "RadioGroupQtyPublishing"
        Me.RadioGroupQtyPublishing.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("None", "None"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Fixed", "Fixed Value"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Percent", "% of Total")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.RadioGroupQtyPublishing, System.Drawing.Color.Empty)
        Me.RadioGroupQtyPublishing.Size = New System.Drawing.Size(100, 83)
        Me.RadioGroupQtyPublishing.TabIndex = 93
        Me.ExtendControlProperty.SetTextDisplay(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblAttributeSet
        '
        Me.lblAttributeSet.Enabled = False
        Me.lblAttributeSet.Location = New System.Drawing.Point(281, 233)
        Me.lblAttributeSet.Name = "lblAttributeSet"
        Me.lblAttributeSet.Size = New System.Drawing.Size(62, 13)
        Me.lblAttributeSet.TabIndex = 92
        Me.lblAttributeSet.Text = "Attribute Set"
        '
        'ImageComboBoxEditAttributeSet
        '
        Me.ImageComboBoxEditAttributeSet.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.AttributeSetID_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.ImageComboBoxEditAttributeSet, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "AttributeSetID_DEV000221"))
        Me.ImageComboBoxEditAttributeSet.Location = New System.Drawing.Point(349, 229)
        Me.ImageComboBoxEditAttributeSet.Name = "ImageComboBoxEditAttributeSet"
        Me.ImageComboBoxEditAttributeSet.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ImageComboBoxEditAttributeSet.Properties.Appearance.Options.UseBackColor = True
        Me.ImageComboBoxEditAttributeSet.Properties.AutoHeight = False
        Me.ImageComboBoxEditAttributeSet.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.ImageComboBoxEditAttributeSet, System.Drawing.Color.Empty)
        Me.ImageComboBoxEditAttributeSet.Size = New System.Drawing.Size(142, 22)
        Me.ImageComboBoxEditAttributeSet.TabIndex = 91
        Me.ExtendControlProperty.SetTextDisplay(Me.ImageComboBoxEditAttributeSet, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblMagentoAttributes
        '
        Me.lblMagentoAttributes.Location = New System.Drawing.Point(5, 245)
        Me.lblMagentoAttributes.Name = "lblMagentoAttributes"
        Me.lblMagentoAttributes.Size = New System.Drawing.Size(133, 13)
        Me.lblMagentoAttributes.TabIndex = 90
        Me.lblMagentoAttributes.Text = "Magento Product Attributes"
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
        Me.lblGetCategoryProgress.Text = "Getting Magento Categories" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please Wait"
        '
        'pnlGetAttributeProgress
        '
        Me.pnlGetAttributeProgress.Controls.Add(Me.lblGetAttributeProgress)
        Me.pnlGetAttributeProgress.Location = New System.Drawing.Point(167, 325)
        Me.pnlGetAttributeProgress.Name = "pnlGetAttributeProgress"
        Me.pnlGetAttributeProgress.Size = New System.Drawing.Size(200, 100)
        Me.pnlGetAttributeProgress.TabIndex = 89
        Me.pnlGetAttributeProgress.Visible = False
        '
        'lblGetAttributeProgress
        '
        Me.lblGetAttributeProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblGetAttributeProgress.Location = New System.Drawing.Point(10, 28)
        Me.lblGetAttributeProgress.MinimumSize = New System.Drawing.Size(180, 0)
        Me.lblGetAttributeProgress.Name = "lblGetAttributeProgress"
        Me.lblGetAttributeProgress.Size = New System.Drawing.Size(180, 39)
        Me.lblGetAttributeProgress.TabIndex = 0
        Me.lblGetAttributeProgress.Text = "Getting Magento Attributes" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please Wait"
        '
        'chkPriceAsIS
        '
        Me.chkPriceAsIS.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.UseISPricingDetail_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.chkPriceAsIS, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "UseISPricingDetail_DEV000221"))
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
        'lblStatus
        '
        Me.lblStatus.Enabled = False
        Me.lblStatus.Location = New System.Drawing.Point(504, 177)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(31, 13)
        Me.lblStatus.TabIndex = 87
        Me.lblStatus.Text = "Status"
        '
        'ImageComboBoxEditStatus
        '
        Me.ImageComboBoxEditStatus.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.Status_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.ImageComboBoxEditStatus, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "Status_DEV000221"))
        Me.ImageComboBoxEditStatus.Location = New System.Drawing.Point(541, 173)
        Me.ImageComboBoxEditStatus.Name = "ImageComboBoxEditStatus"
        Me.ImageComboBoxEditStatus.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ImageComboBoxEditStatus.Properties.Appearance.Options.UseBackColor = True
        Me.ImageComboBoxEditStatus.Properties.AutoHeight = False
        Me.ImageComboBoxEditStatus.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ImageComboBoxEditStatus.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Enabled", 1, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Disabled", 2, -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.ImageComboBoxEditStatus, System.Drawing.Color.Empty)
        Me.ImageComboBoxEditStatus.Size = New System.Drawing.Size(114, 22)
        Me.ImageComboBoxEditStatus.TabIndex = 86
        Me.ExtendControlProperty.SetTextDisplay(Me.ImageComboBoxEditStatus, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblVisibility
        '
        Me.lblVisibility.Enabled = False
        Me.lblVisibility.Location = New System.Drawing.Point(306, 205)
        Me.lblVisibility.Name = "lblVisibility"
        Me.lblVisibility.Size = New System.Drawing.Size(37, 13)
        Me.lblVisibility.TabIndex = 85
        Me.lblVisibility.Text = "Visibility"
        '
        'ImageComboBoxEditVisibility
        '
        Me.ImageComboBoxEditVisibility.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.Visibility_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.ImageComboBoxEditVisibility, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "Visibility_DEV000221"))
        Me.ImageComboBoxEditVisibility.Location = New System.Drawing.Point(349, 201)
        Me.ImageComboBoxEditVisibility.Name = "ImageComboBoxEditVisibility"
        Me.ImageComboBoxEditVisibility.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ImageComboBoxEditVisibility.Properties.Appearance.Options.UseBackColor = True
        Me.ImageComboBoxEditVisibility.Properties.AutoHeight = False
        Me.ImageComboBoxEditVisibility.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ImageComboBoxEditVisibility.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Nowhere", 1, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Catalog", 2, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Search", 3, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Catalog, Search", 4, -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.ImageComboBoxEditVisibility, System.Drawing.Color.Empty)
        Me.ImageComboBoxEditVisibility.Size = New System.Drawing.Size(142, 22)
        Me.ImageComboBoxEditVisibility.TabIndex = 84
        Me.ExtendControlProperty.SetTextDisplay(Me.ImageComboBoxEditVisibility, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblInDepth
        '
        Me.lblInDepth.Enabled = False
        Me.lblInDepth.Location = New System.Drawing.Point(656, 289)
        Me.lblInDepth.Name = "lblInDepth"
        Me.lblInDepth.Size = New System.Drawing.Size(42, 13)
        Me.lblInDepth.TabIndex = 83
        Me.lblInDepth.Text = "In Depth"
        '
        'MemoEditInDepth
        '
        Me.MemoEditInDepth.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ProductInDepthDescription_DEV000221", True))
        Me.MemoEditInDepth.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditInDepth, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ProductInDepthDescription_DEV000221"))
        Me.MemoEditInDepth.Location = New System.Drawing.Point(708, 286)
        Me.MemoEditInDepth.Name = "MemoEditInDepth"
        Me.MemoEditInDepth.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditInDepth.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditInDepth, System.Drawing.Color.Empty)
        Me.MemoEditInDepth.Size = New System.Drawing.Size(272, 199)
        Me.MemoEditInDepth.TabIndex = 82
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditInDepth, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblDescription
        '
        Me.lblDescription.Enabled = False
        Me.lblDescription.Location = New System.Drawing.Point(645, 134)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(53, 13)
        Me.lblDescription.TabIndex = 81
        Me.lblDescription.Text = "Description"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ProductDescription_DEV000221", True))
        Me.MemoEditDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ProductDescription_DEV000221"))
        Me.MemoEditDescription.Location = New System.Drawing.Point(708, 131)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditDescription, System.Drawing.Color.Empty)
        Me.MemoEditDescription.Size = New System.Drawing.Size(272, 144)
        Me.MemoEditDescription.TabIndex = 80
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSpecialPrice
        '
        Me.lblSpecialPrice.Enabled = False
        Me.lblSpecialPrice.Location = New System.Drawing.Point(284, 149)
        Me.lblSpecialPrice.Name = "lblSpecialPrice"
        Me.lblSpecialPrice.Size = New System.Drawing.Size(59, 13)
        Me.lblSpecialPrice.TabIndex = 79
        Me.lblSpecialPrice.Text = "Special Price"
        '
        'TextEditSpecialPrice
        '
        Me.TextEditSpecialPrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.SpecialPrice_DEV000221", True))
        Me.TextEditSpecialPrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditSpecialPrice, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "SpecialPrice_DEV000221"))
        Me.TextEditSpecialPrice.Location = New System.Drawing.Point(349, 145)
        Me.TextEditSpecialPrice.Name = "TextEditSpecialPrice"
        Me.TextEditSpecialPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditSpecialPrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditSpecialPrice.Properties.AutoHeight = False
        Me.TextEditSpecialPrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditSpecialPrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditSpecialPrice, System.Drawing.Color.Empty)
        Me.TextEditSpecialPrice.Size = New System.Drawing.Size(82, 22)
        Me.TextEditSpecialPrice.TabIndex = 78
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditSpecialPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSpecialTo
        '
        Me.lblSpecialTo.Enabled = False
        Me.lblSpecialTo.Location = New System.Drawing.Point(433, 121)
        Me.lblSpecialTo.Name = "lblSpecialTo"
        Me.lblSpecialTo.Size = New System.Drawing.Size(10, 13)
        Me.lblSpecialTo.TabIndex = 77
        Me.lblSpecialTo.Text = "to"
        '
        'lblSpecialFrom
        '
        Me.lblSpecialFrom.Enabled = False
        Me.lblSpecialFrom.Location = New System.Drawing.Point(259, 121)
        Me.lblSpecialFrom.Name = "lblSpecialFrom"
        Me.lblSpecialFrom.Size = New System.Drawing.Size(84, 13)
        Me.lblSpecialFrom.TabIndex = 76
        Me.lblSpecialFrom.Text = "Special Price from"
        '
        'lblShowAsNewTo
        '
        Me.lblShowAsNewTo.Enabled = False
        Me.lblShowAsNewTo.Location = New System.Drawing.Point(433, 93)
        Me.lblShowAsNewTo.Name = "lblShowAsNewTo"
        Me.lblShowAsNewTo.Size = New System.Drawing.Size(10, 13)
        Me.lblShowAsNewTo.TabIndex = 75
        Me.lblShowAsNewTo.Text = "to"
        '
        'lblShowAsNewFrom
        '
        Me.lblShowAsNewFrom.Enabled = False
        Me.lblShowAsNewFrom.Location = New System.Drawing.Point(254, 93)
        Me.lblShowAsNewFrom.Name = "lblShowAsNewFrom"
        Me.lblShowAsNewFrom.Size = New System.Drawing.Size(89, 13)
        Me.lblShowAsNewFrom.TabIndex = 74
        Me.lblShowAsNewFrom.Text = "Show as New from"
        '
        'GridControlProperties
        '
        Me.GridControlProperties.DataMember = "InventoryMagentoTagDetails_DEV000221"
        Me.GridControlProperties.DataSource = Me.InventoryMagentoSettingsSectionGateway
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
        'DateEditSpecialTo
        '
        Me.DateEditSpecialTo.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ShowAsSpecialTo_DEV000221", True))
        Me.DateEditSpecialTo.EditValue = Nothing
        Me.DateEditSpecialTo.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditSpecialTo, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ShowAsSpecialTo_DEV000221"))
        Me.DateEditSpecialTo.Location = New System.Drawing.Point(446, 117)
        Me.DateEditSpecialTo.Name = "DateEditSpecialTo"
        Me.DateEditSpecialTo.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditSpecialTo.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditSpecialTo.Properties.AutoHeight = False
        Me.DateEditSpecialTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditSpecialTo.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditSpecialTo, System.Drawing.Color.Empty)
        Me.DateEditSpecialTo.Size = New System.Drawing.Size(81, 22)
        Me.DateEditSpecialTo.TabIndex = 72
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditSpecialTo, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'DateEditSpecialFrom
        '
        Me.DateEditSpecialFrom.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ShowAsSpecialFrom_DEV000221", True))
        Me.DateEditSpecialFrom.EditValue = Nothing
        Me.DateEditSpecialFrom.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditSpecialFrom, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ShowAsSpecialFrom_DEV000221"))
        Me.DateEditSpecialFrom.Location = New System.Drawing.Point(349, 117)
        Me.DateEditSpecialFrom.Name = "DateEditSpecialFrom"
        Me.DateEditSpecialFrom.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditSpecialFrom.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditSpecialFrom.Properties.AutoHeight = False
        Me.DateEditSpecialFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditSpecialFrom.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditSpecialFrom, System.Drawing.Color.Empty)
        Me.DateEditSpecialFrom.Size = New System.Drawing.Size(81, 22)
        Me.DateEditSpecialFrom.TabIndex = 71
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditSpecialFrom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'DateEditShowAsNewTo
        '
        Me.DateEditShowAsNewTo.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ShowAsNewTo_DEV000221", True))
        Me.DateEditShowAsNewTo.EditValue = Nothing
        Me.DateEditShowAsNewTo.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditShowAsNewTo, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ShowAsNewTo_DEV000221"))
        Me.DateEditShowAsNewTo.Location = New System.Drawing.Point(446, 89)
        Me.DateEditShowAsNewTo.Name = "DateEditShowAsNewTo"
        Me.DateEditShowAsNewTo.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditShowAsNewTo.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditShowAsNewTo.Properties.AutoHeight = False
        Me.DateEditShowAsNewTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditShowAsNewTo.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditShowAsNewTo, System.Drawing.Color.Empty)
        Me.DateEditShowAsNewTo.Size = New System.Drawing.Size(81, 22)
        Me.DateEditShowAsNewTo.TabIndex = 70
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditShowAsNewTo, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'DateEditShowAsNewFrom
        '
        Me.DateEditShowAsNewFrom.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ShowAsNewFrom_DEV000221", True))
        Me.DateEditShowAsNewFrom.EditValue = Nothing
        Me.DateEditShowAsNewFrom.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditShowAsNewFrom, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ShowAsNewFrom_DEV000221"))
        Me.DateEditShowAsNewFrom.Location = New System.Drawing.Point(349, 89)
        Me.DateEditShowAsNewFrom.Name = "DateEditShowAsNewFrom"
        Me.DateEditShowAsNewFrom.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditShowAsNewFrom.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditShowAsNewFrom.Properties.AutoHeight = False
        Me.DateEditShowAsNewFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditShowAsNewFrom.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditShowAsNewFrom, System.Drawing.Color.Empty)
        Me.DateEditShowAsNewFrom.Size = New System.Drawing.Size(81, 22)
        Me.DateEditShowAsNewFrom.TabIndex = 69
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditShowAsNewFrom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblMagentoSellingPrice
        '
        Me.lblMagentoSellingPrice.Enabled = False
        Me.lblMagentoSellingPrice.Location = New System.Drawing.Point(242, 37)
        Me.lblMagentoSellingPrice.Name = "lblMagentoSellingPrice"
        Me.lblMagentoSellingPrice.Size = New System.Drawing.Size(101, 13)
        Me.lblMagentoSellingPrice.TabIndex = 68
        Me.lblMagentoSellingPrice.Text = "Magento Selling Price"
        '
        'TextEditMagentoSellingPrice
        '
        Me.TextEditMagentoSellingPrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.SellingPrice_DEV000221", True))
        Me.TextEditMagentoSellingPrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditMagentoSellingPrice, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "SellingPrice_DEV000221"))
        Me.TextEditMagentoSellingPrice.Location = New System.Drawing.Point(349, 33)
        Me.TextEditMagentoSellingPrice.Name = "TextEditMagentoSellingPrice"
        Me.TextEditMagentoSellingPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditMagentoSellingPrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditMagentoSellingPrice.Properties.AutoHeight = False
        Me.TextEditMagentoSellingPrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditMagentoSellingPrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditMagentoSellingPrice, System.Drawing.Color.Empty)
        Me.TextEditMagentoSellingPrice.Size = New System.Drawing.Size(82, 22)
        Me.TextEditMagentoSellingPrice.TabIndex = 67
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditMagentoSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixItem
        '
        Me.LabelMatrixItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Location = New System.Drawing.Point(539, 61)
        Me.LabelMatrixItem.Name = "LabelMatrixItem"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.LabelMatrixItem, System.Drawing.Color.Empty)
        Me.LabelMatrixItem.Size = New System.Drawing.Size(146, 65)
        Me.LabelMatrixItem.TabIndex = 66
        Me.LabelMatrixItem.Text = "This is a Matrix Item.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Attribute S" & _
    "et" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "are set from the Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Group Item."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Visible = False
        '
        'LabelMatrixGroupItem
        '
        Me.LabelMatrixGroupItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Location = New System.Drawing.Point(539, 61)
        Me.LabelMatrixGroupItem.Name = "LabelMatrixGroupItem"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.LabelMatrixGroupItem, System.Drawing.Color.Empty)
        Me.LabelMatrixGroupItem.Size = New System.Drawing.Size(146, 65)
        Me.LabelMatrixGroupItem.TabIndex = 65
        Me.LabelMatrixGroupItem.Text = "This is a Matrix Group Item." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Attribu" & _
    "te Set" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "are applied to every Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Item in the Matrix Group."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Visible = False
        '
        'lblShortDescription
        '
        Me.lblShortDescription.Enabled = False
        Me.lblShortDescription.Location = New System.Drawing.Point(616, 35)
        Me.lblShortDescription.Name = "lblShortDescription"
        Me.lblShortDescription.Size = New System.Drawing.Size(82, 13)
        Me.lblShortDescription.TabIndex = 32
        Me.lblShortDescription.Text = "Short Description"
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
        'MemoEditShortDescription
        '
        Me.MemoEditShortDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ProductShortDescription_DEV000221", True))
        Me.MemoEditShortDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditShortDescription, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ProductShortDescription_DEV000221"))
        Me.MemoEditShortDescription.Location = New System.Drawing.Point(708, 32)
        Me.MemoEditShortDescription.Name = "MemoEditShortDescription"
        Me.MemoEditShortDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditShortDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditShortDescription, System.Drawing.Color.Empty)
        Me.MemoEditShortDescription.Size = New System.Drawing.Size(272, 89)
        Me.MemoEditShortDescription.TabIndex = 30
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditShortDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditProductName
        '
        Me.TextEditProductName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.ProductName_DEV000221", True))
        Me.TextEditProductName.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "ProductName_DEV000221"))
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
        'lblSelectMagentoInstance
        '
        Me.lblSelectMagentoInstance.Location = New System.Drawing.Point(5, 8)
        Me.lblSelectMagentoInstance.Name = "lblSelectMagentoInstance"
        Me.lblSelectMagentoInstance.Size = New System.Drawing.Size(119, 13)
        Me.lblSelectMagentoInstance.TabIndex = 9
        Me.lblSelectMagentoInstance.Text = "Select Magento Instance"
        '
        'cbeSelectMagentoInstance
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeSelectMagentoInstance, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSelectMagentoInstance.Location = New System.Drawing.Point(130, 4)
        Me.cbeSelectMagentoInstance.Name = "cbeSelectMagentoInstance"
        Me.cbeSelectMagentoInstance.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeSelectMagentoInstance.Properties.Appearance.Options.UseBackColor = True
        Me.cbeSelectMagentoInstance.Properties.AutoHeight = False
        Me.cbeSelectMagentoInstance.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeSelectMagentoInstance, System.Drawing.Color.Empty)
        Me.cbeSelectMagentoInstance.Size = New System.Drawing.Size(184, 22)
        Me.cbeSelectMagentoInstance.TabIndex = 8
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSelectMagentoInstance, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
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
        'CheckEditPublishOnMagento
        '
        Me.CheckEditPublishOnMagento.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221.Publish_DEV000221", True))
        Me.CheckEditPublishOnMagento.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditPublishOnMagento, New Interprise.Presentation.Base.DisplayText(Me.InventoryMagentoSettingsSectionGateway, "InventoryMagentoDetails_DEV000221", "Publish_DEV000221"))
        Me.CheckEditPublishOnMagento.Location = New System.Drawing.Point(320, 5)
        Me.CheckEditPublishOnMagento.Name = "CheckEditPublishOnMagento"
        Me.CheckEditPublishOnMagento.Properties.AutoHeight = False
        Me.CheckEditPublishOnMagento.Properties.Caption = "Publish on this Magento Instance"
        Me.CheckEditPublishOnMagento.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.CheckEditPublishOnMagento, System.Drawing.Color.Empty)
        Me.CheckEditPublishOnMagento.Size = New System.Drawing.Size(188, 22)
        Me.CheckEditPublishOnMagento.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditPublishOnMagento, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'InventoryMagentoSettingsSectionLayoutGroup
        '
        Me.InventoryMagentoSettingsSectionLayoutGroup.CustomizationFormText = "InventoryMagentoSettingsSectionLayoutGroup"
        Me.InventoryMagentoSettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventoryMagentoSettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.InventoryMagentoSettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventoryMagentoSettingsSectionLayoutGroup.Name = "InventoryMagentoSettingsSectionLayoutGroup"
        Me.InventoryMagentoSettingsSectionLayoutGroup.Size = New System.Drawing.Size(993, 497)
        Me.InventoryMagentoSettingsSectionLayoutGroup.Text = "InventoryMagentoSettingsSectionLayoutGroup"
        Me.InventoryMagentoSettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelPublishOnMagento
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
        'InventoryMagentoSettingsSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.InventoryMagentoSettingsSectionExtendedLayout)
        Me.Controls.Add(Me.PanelControlDummy)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventoryMagentoSettingsSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(993, 497)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryMagentoSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryMagentoSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventoryMagentoSettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelPublishOnMagento, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPublishOnMagento.ResumeLayout(False)
        Me.PanelPublishOnMagento.PerformLayout()
        CType(Me.cbeMagentoSpecialPriceSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeMagentoPriceSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageComboBoxEditAttributeSet.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetCategoryProgress.ResumeLayout(False)
        Me.pnlGetCategoryProgress.PerformLayout()
        CType(Me.pnlGetAttributeProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetAttributeProgress.ResumeLayout(False)
        Me.pnlGetAttributeProgress.PerformLayout()
        CType(Me.chkPriceAsIS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageComboBoxEditStatus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageComboBoxEditVisibility.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditInDepth.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditSpecialPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeSelectEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialTo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialFrom.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditShowAsNewTo.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditShowAsNewTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditShowAsNewFrom.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditShowAsNewFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditMagentoSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditShortDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeSelectMagentoInstance.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditPublishOnMagento.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryMagentoSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDummy.ResumeLayout(False)
        Me.PanelControlDummy.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventoryMagentoSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventoryMagentoSettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventoryMagentoSettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelPublishOnMagento As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents CheckEditPublishOnMagento As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents TreeListCategories As DevExpress.XtraTreeList.TreeList
    Friend WithEvents colTreeListCategoryActive As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryID As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents lblSelectMagentoInstance As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSelectMagentoInstance As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents MemoEditShortDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents TextEditProductName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblShortDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblProductName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelMatrixItem As System.Windows.Forms.Label
    Friend WithEvents LabelMatrixGroupItem As System.Windows.Forms.Label
    Friend WithEvents lblMagentoSellingPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditMagentoSellingPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents DateEditShowAsNewFrom As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateEditSpecialTo As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateEditSpecialFrom As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateEditShowAsNewTo As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblShowAsNewTo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblShowAsNewFrom As DevExpress.XtraEditors.LabelControl
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
    Friend WithEvents lblSpecialFrom As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSpecialPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditSpecialPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSpecialTo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblInDepth As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditInDepth As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents rbeMemoEdit As DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
    Friend WithEvents lblVisibility As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ImageComboBoxEditVisibility As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents colPropertiesTextValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesMemoValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents lblStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ImageComboBoxEditStatus As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents chkPriceAsIS As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents pnlGetCategoryProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetCategoryProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMagentoAttributes As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAttributeSet As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ImageComboBoxEditAttributeSet As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents pnlGetAttributeProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetAttributeProgress As DevExpress.XtraEditors.LabelControl
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
    Friend WithEvents lblMagentoPriceSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeMagentoPriceSource As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents lblMagentoSpecialPriceSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeMagentoSpecialPriceSource As DevExpress.XtraEditors.ImageComboBoxEdit

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
