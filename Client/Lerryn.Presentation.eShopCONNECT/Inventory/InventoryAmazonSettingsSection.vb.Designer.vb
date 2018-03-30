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

#Region " InventoryAmazonSettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryAmazonSettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryAmazonSettingsSection))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.InventoryAmazonSettingsSectionGateway = Me.ImportExportDataset
        Me.InventoryAmazonSettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.PanelPublishOnAmazon = New DevExpress.XtraEditors.PanelControl()
        Me.TextEditImageURL = New DevExpress.XtraEditors.TextEdit()
        Me.LabelImageURL = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditAmazonSiteCode = New DevExpress.XtraEditors.TextEdit()
        Me.CheckEditSaleActive = New DevExpress.XtraEditors.CheckEdit()
        Me.LabelSalePrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditSalePrice = New DevExpress.XtraEditors.TextEdit()
        Me.LabelSaleEndDate = New DevExpress.XtraEditors.LabelControl()
        Me.DateEditSaleEndDate = New DevExpress.XtraEditors.DateEdit()
        Me.LabelSaleStartDate = New DevExpress.XtraEditors.LabelControl()
        Me.DateEditSaleStartDate = New DevExpress.XtraEditors.DateEdit()
        Me.ComboBoxEditXMLSubType = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelXMLSubType = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxEditXMLType = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelXMLType = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxEditXMLClass = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelXMLClass = New DevExpress.XtraEditors.LabelControl()
        Me.LabelDescriptiveCategory = New DevExpress.XtraEditors.LabelControl()
        Me.LabelBrowseNodeID = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditBrowseNodeID = New DevExpress.XtraEditors.TextEdit()
        Me.btnBrowseNodeUpLevel = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelProperties = New DevExpress.XtraEditors.LabelControl()
        Me.ListBoxBrowseList = New DevExpress.XtraEditors.ListBoxControl()
        Me.LabelBrowseTree = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditBrowseTree = New DevExpress.XtraEditors.TextEdit()
        Me.LabelAmazonSiteCode = New DevExpress.XtraEditors.LabelControl()
        Me.GridControlProperties = New DevExpress.XtraGrid.GridControl()
        Me.GridViewProperties = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colPropertiesItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesLocation_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeLocationEdit = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.colPropertiesName_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesLineNum_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesDataType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDataType = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.colPropertiesDateValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesNumericValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesTextValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeTextEdit = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.colSourceCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSiteCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colParentCategory_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceTagStatus_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceTagConditionality_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceTagDescription_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceTagAcceptedValues_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceTagExample_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colHelp = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repHelpButton = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.rbeDateEdit = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.rbeYesNoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeTaxCodeEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.GridControlSearchTerms = New DevExpress.XtraGrid.GridControl()
        Me.GridViewSearchTerms = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colSearchItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSearchLocation_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSearchName_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSearchLineNum_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSearchDataType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSearchTextValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LabelDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.LabelProductName = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditProductName = New DevExpress.XtraEditors.TextEdit()
        Me.LabelConditionNote = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditConditionNote = New DevExpress.XtraEditors.MemoEdit()
        Me.LabelCondition = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxEditCondition = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelAmazonSellingPrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditAmazonSellingPrice = New DevExpress.XtraEditors.TextEdit()
        Me.LabelReleaseDate = New DevExpress.XtraEditors.LabelControl()
        Me.DateEditReleaseDate = New DevExpress.XtraEditors.DateEdit()
        Me.LabelLaunchDate = New DevExpress.XtraEditors.LabelControl()
        Me.DateEditLaunchDate = New DevExpress.XtraEditors.DateEdit()
        Me.GridControlASIN = New DevExpress.XtraGrid.GridControl()
        Me.GridViewASIN = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colASIN_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LabelAmazonMerchantID = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxEditAmazonMerchantID = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.CheckEditPublishOnAmazon = New DevExpress.XtraEditors.CheckEdit()
        Me.LabelMatrixItem = New System.Windows.Forms.Label()
        Me.LabelMatrixGroupItem = New System.Windows.Forms.Label()
        Me.PanelControlDummy = New DevExpress.XtraEditors.PanelControl()
        Me.lblDevelopment = New DevExpress.XtraEditors.LabelControl()
        Me.lblActivate = New DevExpress.XtraEditors.LabelControl()
        Me.InventoryAmazonSettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryAmazonSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryAmazonSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventoryAmazonSettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelPublishOnAmazon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPublishOnAmazon.SuspendLayout()
        CType(Me.TextEditImageURL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditAmazonSiteCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditSaleActive.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditSalePrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSaleEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSaleEndDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSaleStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSaleStartDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEditXMLSubType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEditXMLType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEditXMLClass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditBrowseNodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ListBoxBrowseList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditBrowseTree.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeLocationEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repHelpButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTaxCodeEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlSearchTerms, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewSearchTerms, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditConditionNote.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEditCondition.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditAmazonSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditReleaseDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditReleaseDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditLaunchDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditLaunchDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlASIN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewASIN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEditAmazonMerchantID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditPublishOnAmazon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDummy.SuspendLayout()
        CType(Me.InventoryAmazonSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventoryAmazonSettingsSectionGateway
        '
        Me.InventoryAmazonSettingsSectionGateway.DataSetName = "InventoryAmazonSettingsSectionDataset"
        Me.InventoryAmazonSettingsSectionGateway.Instantiate = False
        Me.InventoryAmazonSettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventoryAmazonSettingsSectionExtendedLayout
        '
        Me.InventoryAmazonSettingsSectionExtendedLayout.Controls.Add(Me.PanelPublishOnAmazon)
        Me.InventoryAmazonSettingsSectionExtendedLayout.Controls.Add(Me.PanelControlDummy)
        Me.InventoryAmazonSettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventoryAmazonSettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventoryAmazonSettingsSectionExtendedLayout.Name = "InventoryAmazonSettingsSectionExtendedLayout"
        Me.InventoryAmazonSettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventoryAmazonSettingsSectionExtendedLayout.Root = Me.InventoryAmazonSettingsSectionLayoutGroup
        Me.InventoryAmazonSettingsSectionExtendedLayout.Size = New System.Drawing.Size(974, 497)
        Me.InventoryAmazonSettingsSectionExtendedLayout.TabIndex = 0
        Me.InventoryAmazonSettingsSectionExtendedLayout.Text = "InventoryAmazonSettingsSectionExtendedLayout"
        '
        'PanelPublishOnAmazon
        '
        Me.PanelPublishOnAmazon.Controls.Add(Me.TextEditImageURL)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelImageURL)
        Me.PanelPublishOnAmazon.Controls.Add(Me.TextEditAmazonSiteCode)
        Me.PanelPublishOnAmazon.Controls.Add(Me.CheckEditSaleActive)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelSalePrice)
        Me.PanelPublishOnAmazon.Controls.Add(Me.TextEditSalePrice)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelSaleEndDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.DateEditSaleEndDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelSaleStartDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.DateEditSaleStartDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.ComboBoxEditXMLSubType)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelXMLSubType)
        Me.PanelPublishOnAmazon.Controls.Add(Me.ComboBoxEditXMLType)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelXMLType)
        Me.PanelPublishOnAmazon.Controls.Add(Me.ComboBoxEditXMLClass)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelXMLClass)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelDescriptiveCategory)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelBrowseNodeID)
        Me.PanelPublishOnAmazon.Controls.Add(Me.TextEditBrowseNodeID)
        Me.PanelPublishOnAmazon.Controls.Add(Me.btnBrowseNodeUpLevel)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelProperties)
        Me.PanelPublishOnAmazon.Controls.Add(Me.ListBoxBrowseList)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelBrowseTree)
        Me.PanelPublishOnAmazon.Controls.Add(Me.TextEditBrowseTree)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelAmazonSiteCode)
        Me.PanelPublishOnAmazon.Controls.Add(Me.GridControlProperties)
        Me.PanelPublishOnAmazon.Controls.Add(Me.GridControlSearchTerms)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelDescription)
        Me.PanelPublishOnAmazon.Controls.Add(Me.MemoEditDescription)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelProductName)
        Me.PanelPublishOnAmazon.Controls.Add(Me.TextEditProductName)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelConditionNote)
        Me.PanelPublishOnAmazon.Controls.Add(Me.MemoEditConditionNote)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelCondition)
        Me.PanelPublishOnAmazon.Controls.Add(Me.ComboBoxEditCondition)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelAmazonSellingPrice)
        Me.PanelPublishOnAmazon.Controls.Add(Me.TextEditAmazonSellingPrice)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelReleaseDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.DateEditReleaseDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelLaunchDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.DateEditLaunchDate)
        Me.PanelPublishOnAmazon.Controls.Add(Me.GridControlASIN)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelAmazonMerchantID)
        Me.PanelPublishOnAmazon.Controls.Add(Me.ComboBoxEditAmazonMerchantID)
        Me.PanelPublishOnAmazon.Controls.Add(Me.CheckEditPublishOnAmazon)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelMatrixItem)
        Me.PanelPublishOnAmazon.Controls.Add(Me.LabelMatrixGroupItem)
        Me.PanelPublishOnAmazon.Location = New System.Drawing.Point(3, 3)
        Me.PanelPublishOnAmazon.Name = "PanelPublishOnAmazon"
        Me.PanelPublishOnAmazon.Size = New System.Drawing.Size(968, 491)
        Me.PanelPublishOnAmazon.TabIndex = 4
        '
        'TextEditImageURL
        '
        Me.TextEditImageURL.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ImageURL_DEV000221", True))
        Me.TextEditImageURL.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditImageURL, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditImageURL.Location = New System.Drawing.Point(69, 317)
        Me.TextEditImageURL.Name = "TextEditImageURL"
        Me.TextEditImageURL.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditImageURL.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditImageURL.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditImageURL, False)
        Me.TextEditImageURL.Size = New System.Drawing.Size(336, 22)
        Me.TextEditImageURL.TabIndex = 67
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditImageURL, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelImageURL
        '
        Me.LabelImageURL.Enabled = False
        Me.LabelImageURL.Location = New System.Drawing.Point(6, 321)
        Me.LabelImageURL.Name = "LabelImageURL"
        Me.LabelImageURL.Size = New System.Drawing.Size(52, 13)
        Me.LabelImageURL.TabIndex = 66
        Me.LabelImageURL.Text = "Image URL"
        '
        'TextEditAmazonSiteCode
        '
        Me.TextEditAmazonSiteCode.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.SiteCode_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.TextEditAmazonSiteCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditAmazonSiteCode.Location = New System.Drawing.Point(488, 4)
        Me.TextEditAmazonSiteCode.Name = "TextEditAmazonSiteCode"
        Me.TextEditAmazonSiteCode.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextEditAmazonSiteCode.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditAmazonSiteCode.Properties.AutoHeight = False
        Me.TextEditAmazonSiteCode.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditAmazonSiteCode, False)
        Me.TextEditAmazonSiteCode.Size = New System.Drawing.Size(85, 22)
        Me.TextEditAmazonSiteCode.TabIndex = 65
        Me.TextEditAmazonSiteCode.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditAmazonSiteCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'CheckEditSaleActive
        '
        Me.CheckEditSaleActive.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.SalePriceActive_DEV000221", True))
        Me.CheckEditSaleActive.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditSaleActive, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckEditSaleActive.Location = New System.Drawing.Point(5, 60)
        Me.CheckEditSaleActive.Name = "CheckEditSaleActive"
        Me.CheckEditSaleActive.Properties.AutoHeight = False
        Me.CheckEditSaleActive.Properties.Caption = "Sale Price Active"
        Me.CheckEditSaleActive.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.CheckEditSaleActive, False)
        Me.CheckEditSaleActive.Size = New System.Drawing.Size(107, 22)
        Me.CheckEditSaleActive.TabIndex = 54
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditSaleActive, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelSalePrice
        '
        Me.LabelSalePrice.Enabled = False
        Me.LabelSalePrice.Location = New System.Drawing.Point(118, 64)
        Me.LabelSalePrice.Name = "LabelSalePrice"
        Me.LabelSalePrice.Size = New System.Drawing.Size(46, 13)
        Me.LabelSalePrice.TabIndex = 53
        Me.LabelSalePrice.Text = "Sale Price"
        '
        'TextEditSalePrice
        '
        Me.TextEditSalePrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.SalePrice_DEV000221", True))
        Me.TextEditSalePrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditSalePrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditSalePrice.Location = New System.Drawing.Point(170, 60)
        Me.TextEditSalePrice.Name = "TextEditSalePrice"
        Me.TextEditSalePrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditSalePrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditSalePrice.Properties.AutoHeight = False
        Me.TextEditSalePrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditSalePrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditSalePrice, False)
        Me.TextEditSalePrice.Size = New System.Drawing.Size(64, 22)
        Me.TextEditSalePrice.TabIndex = 52
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditSalePrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelSaleEndDate
        '
        Me.LabelSaleEndDate.Enabled = False
        Me.LabelSaleEndDate.Location = New System.Drawing.Point(418, 64)
        Me.LabelSaleEndDate.Name = "LabelSaleEndDate"
        Me.LabelSaleEndDate.Size = New System.Drawing.Size(41, 13)
        Me.LabelSaleEndDate.TabIndex = 51
        Me.LabelSaleEndDate.Text = "Sale End"
        '
        'DateEditSaleEndDate
        '
        Me.DateEditSaleEndDate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.SaleEndDate_DEV000221", True))
        Me.DateEditSaleEndDate.EditValue = Nothing
        Me.DateEditSaleEndDate.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditSaleEndDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditSaleEndDate.Location = New System.Drawing.Point(488, 60)
        Me.DateEditSaleEndDate.Name = "DateEditSaleEndDate"
        Me.DateEditSaleEndDate.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditSaleEndDate.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditSaleEndDate.Properties.AutoHeight = False
        Me.DateEditSaleEndDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditSaleEndDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.DateEditSaleEndDate, False)
        Me.DateEditSaleEndDate.Size = New System.Drawing.Size(85, 22)
        Me.DateEditSaleEndDate.TabIndex = 50
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditSaleEndDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelSaleStartDate
        '
        Me.LabelSaleStartDate.Enabled = False
        Me.LabelSaleStartDate.Location = New System.Drawing.Point(250, 64)
        Me.LabelSaleStartDate.Name = "LabelSaleStartDate"
        Me.LabelSaleStartDate.Size = New System.Drawing.Size(47, 13)
        Me.LabelSaleStartDate.TabIndex = 49
        Me.LabelSaleStartDate.Text = "Sale Start"
        '
        'DateEditSaleStartDate
        '
        Me.DateEditSaleStartDate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.SaleStartDate_DEV000221", True))
        Me.DateEditSaleStartDate.EditValue = Nothing
        Me.DateEditSaleStartDate.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditSaleStartDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditSaleStartDate.Location = New System.Drawing.Point(317, 60)
        Me.DateEditSaleStartDate.Name = "DateEditSaleStartDate"
        Me.DateEditSaleStartDate.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditSaleStartDate.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditSaleStartDate.Properties.AutoHeight = False
        Me.DateEditSaleStartDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditSaleStartDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.DateEditSaleStartDate, False)
        Me.DateEditSaleStartDate.Size = New System.Drawing.Size(85, 22)
        Me.DateEditSaleStartDate.TabIndex = 48
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditSaleStartDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'ComboBoxEditXMLSubType
        '
        Me.ComboBoxEditXMLSubType.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ProductXMLSubType_DEV000221", True))
        Me.ComboBoxEditXMLSubType.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxEditXMLSubType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxEditXMLSubType.Location = New System.Drawing.Point(380, 207)
        Me.ComboBoxEditXMLSubType.Name = "ComboBoxEditXMLSubType"
        Me.ComboBoxEditXMLSubType.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxEditXMLSubType.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxEditXMLSubType.Properties.AutoHeight = False
        Me.ComboBoxEditXMLSubType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBoxEditXMLSubType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxEditXMLSubType, False)
        Me.ComboBoxEditXMLSubType.Size = New System.Drawing.Size(140, 22)
        Me.ComboBoxEditXMLSubType.TabIndex = 47
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxEditXMLSubType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelXMLSubType
        '
        Me.LabelXMLSubType.Enabled = False
        Me.LabelXMLSubType.Location = New System.Drawing.Point(288, 211)
        Me.LabelXMLSubType.Name = "LabelXMLSubType"
        Me.LabelXMLSubType.Size = New System.Drawing.Size(86, 13)
        Me.LabelXMLSubType.TabIndex = 46
        Me.LabelXMLSubType.Text = "Product Sub-Type"
        '
        'ComboBoxEditXMLType
        '
        Me.ComboBoxEditXMLType.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ProductXMLType_DEV000221", True))
        Me.ComboBoxEditXMLType.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxEditXMLType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxEditXMLType.Location = New System.Drawing.Point(358, 179)
        Me.ComboBoxEditXMLType.Name = "ComboBoxEditXMLType"
        Me.ComboBoxEditXMLType.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxEditXMLType.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxEditXMLType.Properties.AutoHeight = False
        Me.ComboBoxEditXMLType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBoxEditXMLType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxEditXMLType, False)
        Me.ComboBoxEditXMLType.Size = New System.Drawing.Size(162, 22)
        Me.ComboBoxEditXMLType.TabIndex = 45
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxEditXMLType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelXMLType
        '
        Me.LabelXMLType.Enabled = False
        Me.LabelXMLType.Location = New System.Drawing.Point(288, 183)
        Me.LabelXMLType.Name = "LabelXMLType"
        Me.LabelXMLType.Size = New System.Drawing.Size(64, 13)
        Me.LabelXMLType.TabIndex = 44
        Me.LabelXMLType.Text = "Product Type"
        '
        'ComboBoxEditXMLClass
        '
        Me.ComboBoxEditXMLClass.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ProductXMLClass_DEV000221", True))
        Me.ComboBoxEditXMLClass.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxEditXMLClass, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxEditXMLClass.Location = New System.Drawing.Point(357, 151)
        Me.ComboBoxEditXMLClass.Name = "ComboBoxEditXMLClass"
        Me.ComboBoxEditXMLClass.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxEditXMLClass.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxEditXMLClass.Properties.AutoHeight = False
        Me.ComboBoxEditXMLClass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBoxEditXMLClass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxEditXMLClass, False)
        Me.ComboBoxEditXMLClass.Size = New System.Drawing.Size(163, 22)
        Me.ComboBoxEditXMLClass.TabIndex = 43
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxEditXMLClass, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelXMLClass
        '
        Me.LabelXMLClass.Enabled = False
        Me.LabelXMLClass.Location = New System.Drawing.Point(288, 155)
        Me.LabelXMLClass.Name = "LabelXMLClass"
        Me.LabelXMLClass.Size = New System.Drawing.Size(47, 13)
        Me.LabelXMLClass.TabIndex = 42
        Me.LabelXMLClass.Text = "XML Class"
        '
        'LabelDescriptiveCategory
        '
        Me.LabelDescriptiveCategory.Appearance.ForeColor = System.Drawing.Color.Red
        Me.LabelDescriptiveCategory.Enabled = False
        Me.LabelDescriptiveCategory.Location = New System.Drawing.Point(467, 126)
        Me.LabelDescriptiveCategory.Name = "LabelDescriptiveCategory"
        Me.LabelDescriptiveCategory.Size = New System.Drawing.Size(101, 13)
        Me.LabelDescriptiveCategory.TabIndex = 41
        Me.LabelDescriptiveCategory.Text = "Descriptive Category"
        Me.LabelDescriptiveCategory.Visible = False
        '
        'LabelBrowseNodeID
        '
        Me.LabelBrowseNodeID.Enabled = False
        Me.LabelBrowseNodeID.Location = New System.Drawing.Point(288, 127)
        Me.LabelBrowseNodeID.Name = "LabelBrowseNodeID"
        Me.LabelBrowseNodeID.Size = New System.Drawing.Size(63, 13)
        Me.LabelBrowseNodeID.TabIndex = 40
        Me.LabelBrowseNodeID.Text = "Browse Node"
        '
        'TextEditBrowseNodeID
        '
        Me.TextEditBrowseNodeID.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.BrowseNodeID_DEV000221", True))
        Me.TextEditBrowseNodeID.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditBrowseNodeID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditBrowseNodeID.Location = New System.Drawing.Point(357, 123)
        Me.TextEditBrowseNodeID.Name = "TextEditBrowseNodeID"
        Me.TextEditBrowseNodeID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextEditBrowseNodeID.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditBrowseNodeID.Properties.AutoHeight = False
        Me.TextEditBrowseNodeID.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditBrowseNodeID.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.TextEditBrowseNodeID.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditBrowseNodeID, False)
        Me.TextEditBrowseNodeID.Size = New System.Drawing.Size(100, 22)
        Me.TextEditBrowseNodeID.TabIndex = 39
        Me.TextEditBrowseNodeID.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditBrowseNodeID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnBrowseNodeUpLevel
        '
        Me.btnBrowseNodeUpLevel.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.btnBrowseNodeUpLevel, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnBrowseNodeUpLevel.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.UpLevel
        Me.btnBrowseNodeUpLevel.Location = New System.Drawing.Point(552, 94)
        Me.btnBrowseNodeUpLevel.Name = "btnBrowseNodeUpLevel"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.btnBrowseNodeUpLevel, False)
        Me.btnBrowseNodeUpLevel.Size = New System.Drawing.Size(21, 22)
        Me.btnBrowseNodeUpLevel.TabIndex = 38
        Me.ExtendControlProperty.SetTextDisplay(Me.btnBrowseNodeUpLevel, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnBrowseNodeUpLevel.ToolTip = "Up Browse Tree Node"
        '
        'LabelProperties
        '
        Me.LabelProperties.Enabled = False
        Me.LabelProperties.Location = New System.Drawing.Point(426, 247)
        Me.LabelProperties.Name = "LabelProperties"
        Me.LabelProperties.Size = New System.Drawing.Size(80, 13)
        Me.LabelProperties.TabIndex = 37
        Me.LabelProperties.Text = "Other Properties"
        '
        'ListBoxBrowseList
        '
        Me.ListBoxBrowseList.Enabled = False
        Me.ListBoxBrowseList.Location = New System.Drawing.Point(7, 123)
        Me.ListBoxBrowseList.Name = "ListBoxBrowseList"
        Me.ListBoxBrowseList.Size = New System.Drawing.Size(272, 118)
        Me.ListBoxBrowseList.SortOrder = System.Windows.Forms.SortOrder.Ascending
        Me.ListBoxBrowseList.TabIndex = 36
        '
        'LabelBrowseTree
        '
        Me.LabelBrowseTree.Enabled = False
        Me.LabelBrowseTree.Location = New System.Drawing.Point(7, 98)
        Me.LabelBrowseTree.Name = "LabelBrowseTree"
        Me.LabelBrowseTree.Size = New System.Drawing.Size(60, 13)
        Me.LabelBrowseTree.TabIndex = 35
        Me.LabelBrowseTree.Text = "Browse Tree"
        '
        'TextEditBrowseTree
        '
        Me.TextEditBrowseTree.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.BrowseTree_DEV000221", True))
        Me.TextEditBrowseTree.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditBrowseTree, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditBrowseTree.Location = New System.Drawing.Point(73, 94)
        Me.TextEditBrowseTree.Name = "TextEditBrowseTree"
        Me.TextEditBrowseTree.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextEditBrowseTree.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditBrowseTree.Properties.AutoHeight = False
        Me.TextEditBrowseTree.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditBrowseTree, False)
        Me.TextEditBrowseTree.Size = New System.Drawing.Size(473, 22)
        Me.TextEditBrowseTree.TabIndex = 34
        Me.TextEditBrowseTree.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditBrowseTree, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelAmazonSiteCode
        '
        Me.LabelAmazonSiteCode.Enabled = False
        Me.LabelAmazonSiteCode.Location = New System.Drawing.Point(418, 9)
        Me.LabelAmazonSiteCode.Name = "LabelAmazonSiteCode"
        Me.LabelAmazonSiteCode.Size = New System.Drawing.Size(59, 13)
        Me.LabelAmazonSiteCode.TabIndex = 32
        Me.LabelAmazonSiteCode.Text = "Amazon Site"
        '
        'GridControlProperties
        '
        Me.GridControlProperties.DataMember = "InventoryAmazonTagDetailTemplateView_DEV000221"
        Me.GridControlProperties.DataSource = Me.InventoryAmazonSettingsSectionGateway
        Me.GridControlProperties.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlProperties, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.Location = New System.Drawing.Point(426, 266)
        Me.GridControlProperties.MainView = Me.GridViewProperties
        Me.GridControlProperties.Name = "GridControlProperties"
        Me.GridControlProperties.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeDataType, Me.repHelpButton, Me.rbeLocationEdit, Me.rbeDateEdit, Me.rbeTextEdit, Me.rbeYesNoEdit, Me.rbeTaxCodeEdit})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlProperties, False)
        Me.GridControlProperties.Size = New System.Drawing.Size(531, 221)
        Me.GridControlProperties.TabIndex = 31
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewProperties})
        '
        'GridViewProperties
        '
        Me.GridViewProperties.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPropertiesItemCode_DEV000221, Me.colPropertiesLocation_DEV000221, Me.colPropertiesName_DEV000221, Me.colPropertiesLineNum_DEV000221, Me.colPropertiesDataType_DEV000221, Me.colPropertiesDateValue_DEV000221, Me.colPropertiesNumericValue_DEV000221, Me.colPropertiesTextValue_DEV000221, Me.colSourceCode_DEV000221, Me.colSiteCode_DEV000221, Me.colParentCategory_DEV000221, Me.colSourceTagStatus_DEV000221, Me.colSourceTagConditionality_DEV000221, Me.colSourceTagDescription_DEV000221, Me.colSourceTagAcceptedValues_DEV000221, Me.colSourceTagExample_DEV000221, Me.colHelp})
        Me.GridViewProperties.GridControl = Me.GridControlProperties
        Me.GridViewProperties.Name = "GridViewProperties"
        Me.GridViewProperties.OptionsView.ShowGroupPanel = False
        Me.GridViewProperties.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPropertiesLocation_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPropertiesName_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPropertiesLineNum_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colPropertiesItemCode_DEV000221
        '
        Me.colPropertiesItemCode_DEV000221.Caption = "ItemCode_DEV000221"
        Me.colPropertiesItemCode_DEV000221.FieldName = "ItemCode_DEV000221"
        Me.colPropertiesItemCode_DEV000221.Name = "colPropertiesItemCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesItemCode_DEV000221.Width = 20
        '
        'colPropertiesLocation_DEV000221
        '
        Me.colPropertiesLocation_DEV000221.Caption = "Location"
        Me.colPropertiesLocation_DEV000221.ColumnEdit = Me.rbeLocationEdit
        Me.colPropertiesLocation_DEV000221.FieldName = "TagLocation_DEV000221"
        Me.colPropertiesLocation_DEV000221.Name = "colPropertiesLocation_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesLocation_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesLocation_DEV000221.Visible = True
        Me.colPropertiesLocation_DEV000221.VisibleIndex = 0
        Me.colPropertiesLocation_DEV000221.Width = 66
        '
        'rbeLocationEdit
        '
        Me.rbeLocationEdit.AutoHeight = False
        Me.rbeLocationEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeLocationEdit.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Core", 1, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Variations", 2, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Classifications", 3, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Prod. Data", 4, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Item Dim.", 5, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Package Dim.", 6, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Inventory", 20, -1)})
        Me.rbeLocationEdit.Name = "rbeLocationEdit"
        '
        'colPropertiesName_DEV000221
        '
        Me.colPropertiesName_DEV000221.Caption = "Name"
        Me.colPropertiesName_DEV000221.FieldName = "TagName_DEV000221"
        Me.colPropertiesName_DEV000221.Name = "colPropertiesName_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesName_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesName_DEV000221.Visible = True
        Me.colPropertiesName_DEV000221.VisibleIndex = 1
        Me.colPropertiesName_DEV000221.Width = 123
        '
        'colPropertiesLineNum_DEV000221
        '
        Me.colPropertiesLineNum_DEV000221.AppearanceCell.Options.UseTextOptions = True
        Me.colPropertiesLineNum_DEV000221.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colPropertiesLineNum_DEV000221.Caption = "Pos'n"
        Me.colPropertiesLineNum_DEV000221.FieldName = "LineNum_DEV000221"
        Me.colPropertiesLineNum_DEV000221.Name = "colPropertiesLineNum_DEV000221"
        Me.colPropertiesLineNum_DEV000221.OptionsColumn.AllowEdit = False
        Me.colPropertiesLineNum_DEV000221.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesLineNum_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesLineNum_DEV000221.Visible = True
        Me.colPropertiesLineNum_DEV000221.VisibleIndex = 2
        Me.colPropertiesLineNum_DEV000221.Width = 45
        '
        'colPropertiesDataType_DEV000221
        '
        Me.colPropertiesDataType_DEV000221.Caption = "Format"
        Me.colPropertiesDataType_DEV000221.ColumnEdit = Me.rbeDataType
        Me.colPropertiesDataType_DEV000221.FieldName = "TagDataType_DEV000221"
        Me.colPropertiesDataType_DEV000221.Name = "colPropertiesDataType_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDataType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDataType_DEV000221.Visible = True
        Me.colPropertiesDataType_DEV000221.VisibleIndex = 3
        Me.colPropertiesDataType_DEV000221.Width = 57
        '
        'rbeDataType
        '
        Me.rbeDataType.AutoHeight = False
        Me.rbeDataType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeDataType.Items.AddRange(New Object() {"Text", "Date", "DateTime", "Integer", "Numeric", "Y/N"})
        Me.rbeDataType.Name = "rbeDataType"
        Me.rbeDataType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'colPropertiesDateValue_DEV000221
        '
        Me.colPropertiesDateValue_DEV000221.Caption = "TagDateValue_DEV000221"
        Me.colPropertiesDateValue_DEV000221.FieldName = "TagDateValue_DEV000221"
        Me.colPropertiesDateValue_DEV000221.Name = "colPropertiesDateValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDateValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colPropertiesNumericValue_DEV000221
        '
        Me.colPropertiesNumericValue_DEV000221.Caption = "TagNumericValue_DEV000221"
        Me.colPropertiesNumericValue_DEV000221.FieldName = "TagNumericValue_DEV000221"
        Me.colPropertiesNumericValue_DEV000221.Name = "colPropertiesNumericValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesNumericValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colPropertiesTextValue_DEV000221
        '
        Me.colPropertiesTextValue_DEV000221.Caption = "Value"
        Me.colPropertiesTextValue_DEV000221.ColumnEdit = Me.rbeTextEdit
        Me.colPropertiesTextValue_DEV000221.FieldName = "TagTextValue_DEV000221"
        Me.colPropertiesTextValue_DEV000221.Name = "colPropertiesTextValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesTextValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesTextValue_DEV000221.Visible = True
        Me.colPropertiesTextValue_DEV000221.VisibleIndex = 4
        Me.colPropertiesTextValue_DEV000221.Width = 190
        '
        'rbeTextEdit
        '
        Me.rbeTextEdit.AutoHeight = False
        Me.rbeTextEdit.Name = "rbeTextEdit"
        '
        'colSourceCode_DEV000221
        '
        Me.colSourceCode_DEV000221.Caption = "SourceCode_DEV000221"
        Me.colSourceCode_DEV000221.FieldName = "SourceCode_DEV000221"
        Me.colSourceCode_DEV000221.Name = "colSourceCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSiteCode_DEV000221
        '
        Me.colSiteCode_DEV000221.Caption = "SiteCode_DEV000221"
        Me.colSiteCode_DEV000221.FieldName = "SiteCode_DEV000221"
        Me.colSiteCode_DEV000221.Name = "colSiteCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSiteCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colParentCategory_DEV000221
        '
        Me.colParentCategory_DEV000221.Caption = "ParentCategory_DEV000221"
        Me.colParentCategory_DEV000221.FieldName = "ParentCategory_DEV000221"
        Me.colParentCategory_DEV000221.Name = "colParentCategory_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colParentCategory_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourceTagStatus_DEV000221
        '
        Me.colSourceTagStatus_DEV000221.Caption = "SourceTagStatus_DEV000221"
        Me.colSourceTagStatus_DEV000221.FieldName = "SourceTagStatus_DEV000221"
        Me.colSourceTagStatus_DEV000221.Name = "colSourceTagStatus_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceTagStatus_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourceTagConditionality_DEV000221
        '
        Me.colSourceTagConditionality_DEV000221.Caption = "GridColumn1"
        Me.colSourceTagConditionality_DEV000221.FieldName = "SourceTagConditionality_DEV000221"
        Me.colSourceTagConditionality_DEV000221.Name = "colSourceTagConditionality_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceTagConditionality_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourceTagDescription_DEV000221
        '
        Me.colSourceTagDescription_DEV000221.Caption = "SourceTagDescription_DEV000221"
        Me.colSourceTagDescription_DEV000221.FieldName = "SourceTagDescription_DEV000221"
        Me.colSourceTagDescription_DEV000221.Name = "colSourceTagDescription_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceTagDescription_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourceTagAcceptedValues_DEV000221
        '
        Me.colSourceTagAcceptedValues_DEV000221.Caption = "SourceTagAcceptedValues_DEV000221"
        Me.colSourceTagAcceptedValues_DEV000221.FieldName = "SourceTagAcceptedValues_DEV000221"
        Me.colSourceTagAcceptedValues_DEV000221.Name = "colSourceTagAcceptedValues_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceTagAcceptedValues_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourceTagExample_DEV000221
        '
        Me.colSourceTagExample_DEV000221.Caption = "SourceTagExample_DEV000221"
        Me.colSourceTagExample_DEV000221.FieldName = "SourceTagExample_DEV000221"
        Me.colSourceTagExample_DEV000221.Name = "colSourceTagExample_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceTagExample_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colHelp
        '
        Me.colHelp.ColumnEdit = Me.repHelpButton
        Me.colHelp.Name = "colHelp"
        Me.colHelp.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        Me.ExtendControlProperty.SetTextDisplay(Me.colHelp, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colHelp.Visible = True
        Me.colHelp.VisibleIndex = 5
        Me.colHelp.Width = 29
        '
        'repHelpButton
        '
        Me.repHelpButton.AutoHeight = False
        SerializableAppearanceObject1.Options.UseImage = True
        Me.repHelpButton.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.Help, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, False)})
        Me.repHelpButton.Name = "repHelpButton"
        Me.repHelpButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'rbeDateEdit
        '
        Me.rbeDateEdit.AutoHeight = False
        Me.rbeDateEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeDateEdit.Name = "rbeDateEdit"
        Me.rbeDateEdit.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        '
        'rbeYesNoEdit
        '
        Me.rbeYesNoEdit.AutoHeight = False
        Me.rbeYesNoEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeYesNoEdit.Items.AddRange(New Object() {"Y", "N"})
        Me.rbeYesNoEdit.Name = "rbeYesNoEdit"
        Me.rbeYesNoEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeTaxCodeEdit
        '
        Me.rbeTaxCodeEdit.AutoHeight = False
        Me.rbeTaxCodeEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeTaxCodeEdit.Items.AddRange(New Object() {"A_GEN-TAX", "A_GEN_NOTAX"})
        Me.rbeTaxCodeEdit.Name = "rbeTaxCodeEdit"
        Me.rbeTaxCodeEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'GridControlSearchTerms
        '
        Me.GridControlSearchTerms.DataMember = "InventoryAmazonSearchTerms"
        Me.GridControlSearchTerms.DataSource = Me.InventoryAmazonSettingsSectionGateway
        Me.GridControlSearchTerms.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlSearchTerms, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlSearchTerms, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSearchTerms.Location = New System.Drawing.Point(5, 349)
        Me.GridControlSearchTerms.MainView = Me.GridViewSearchTerms
        Me.GridControlSearchTerms.Name = "GridControlSearchTerms"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlSearchTerms, False)
        Me.GridControlSearchTerms.Size = New System.Drawing.Size(400, 138)
        Me.GridControlSearchTerms.TabIndex = 30
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlSearchTerms, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSearchTerms.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewSearchTerms})
        '
        'GridViewSearchTerms
        '
        Me.GridViewSearchTerms.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colSearchItemCode_DEV000221, Me.colSearchLocation_DEV000221, Me.colSearchName_DEV000221, Me.colSearchLineNum_DEV000221, Me.colSearchDataType_DEV000221, Me.colSearchTextValue_DEV000221})
        Me.GridViewSearchTerms.GridControl = Me.GridControlSearchTerms
        Me.GridViewSearchTerms.Name = "GridViewSearchTerms"
        Me.GridViewSearchTerms.OptionsView.ShowGroupPanel = False
        '
        'colSearchItemCode_DEV000221
        '
        Me.colSearchItemCode_DEV000221.Caption = "Item Code"
        Me.colSearchItemCode_DEV000221.FieldName = "ItemCode_DEV000221"
        Me.colSearchItemCode_DEV000221.Name = "colSearchItemCode_DEV000221"
        Me.colSearchItemCode_DEV000221.OptionsColumn.AllowEdit = False
        Me.ExtendControlProperty.SetTextDisplay(Me.colSearchItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSearchItemCode_DEV000221.Width = 20
        '
        'colSearchLocation_DEV000221
        '
        Me.colSearchLocation_DEV000221.Caption = "Location"
        Me.colSearchLocation_DEV000221.FieldName = "TagLocation_DEV000221"
        Me.colSearchLocation_DEV000221.Name = "colSearchLocation_DEV000221"
        Me.colSearchLocation_DEV000221.OptionsColumn.AllowEdit = False
        Me.ExtendControlProperty.SetTextDisplay(Me.colSearchLocation_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSearchLocation_DEV000221.Width = 20
        '
        'colSearchName_DEV000221
        '
        Me.colSearchName_DEV000221.Caption = "Search Tag"
        Me.colSearchName_DEV000221.FieldName = "TagName_DEV000221"
        Me.colSearchName_DEV000221.Name = "colSearchName_DEV000221"
        Me.colSearchName_DEV000221.OptionsColumn.AllowEdit = False
        Me.ExtendControlProperty.SetTextDisplay(Me.colSearchName_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSearchName_DEV000221.Visible = True
        Me.colSearchName_DEV000221.VisibleIndex = 0
        Me.colSearchName_DEV000221.Width = 129
        '
        'colSearchLineNum_DEV000221
        '
        Me.colSearchLineNum_DEV000221.AppearanceCell.Options.UseTextOptions = True
        Me.colSearchLineNum_DEV000221.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colSearchLineNum_DEV000221.Caption = "Pos'n"
        Me.colSearchLineNum_DEV000221.FieldName = "LineNum_DEV000221"
        Me.colSearchLineNum_DEV000221.Name = "colSearchLineNum_DEV000221"
        Me.colSearchLineNum_DEV000221.OptionsColumn.AllowEdit = False
        Me.colSearchLineNum_DEV000221.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSearchLineNum_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSearchLineNum_DEV000221.Visible = True
        Me.colSearchLineNum_DEV000221.VisibleIndex = 1
        Me.colSearchLineNum_DEV000221.Width = 45
        '
        'colSearchDataType_DEV000221
        '
        Me.colSearchDataType_DEV000221.Caption = "Data Type"
        Me.colSearchDataType_DEV000221.FieldName = "TagDataType_DEV000221"
        Me.colSearchDataType_DEV000221.Name = "colSearchDataType_DEV000221"
        Me.colSearchDataType_DEV000221.OptionsColumn.AllowEdit = False
        Me.ExtendControlProperty.SetTextDisplay(Me.colSearchDataType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSearchTextValue_DEV000221
        '
        Me.colSearchTextValue_DEV000221.Caption = "Value"
        Me.colSearchTextValue_DEV000221.FieldName = "TagTextValue_DEV000221"
        Me.colSearchTextValue_DEV000221.Name = "colSearchTextValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSearchTextValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSearchTextValue_DEV000221.Visible = True
        Me.colSearchTextValue_DEV000221.VisibleIndex = 2
        Me.colSearchTextValue_DEV000221.Width = 205
        '
        'LabelDescription
        '
        Me.LabelDescription.Enabled = False
        Me.LabelDescription.Location = New System.Drawing.Point(624, 37)
        Me.LabelDescription.Name = "LabelDescription"
        Me.LabelDescription.Size = New System.Drawing.Size(53, 13)
        Me.LabelDescription.TabIndex = 29
        Me.LabelDescription.Text = "Description"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ProductDescription_DEV000221", True))
        Me.MemoEditDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditDescription.Location = New System.Drawing.Point(685, 32)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditDescription, False)
        Me.MemoEditDescription.Size = New System.Drawing.Size(272, 96)
        Me.MemoEditDescription.TabIndex = 28
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelProductName
        '
        Me.LabelProductName.Enabled = False
        Me.LabelProductName.Location = New System.Drawing.Point(610, 9)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New System.Drawing.Size(67, 13)
        Me.LabelProductName.TabIndex = 27
        Me.LabelProductName.Text = "Product Name"
        '
        'TextEditProductName
        '
        Me.TextEditProductName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ProductName_DEV000221", True))
        Me.TextEditProductName.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditProductName.Location = New System.Drawing.Point(685, 4)
        Me.TextEditProductName.Name = "TextEditProductName"
        Me.TextEditProductName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditProductName.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditProductName.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditProductName, False)
        Me.TextEditProductName.Size = New System.Drawing.Size(272, 22)
        Me.TextEditProductName.TabIndex = 26
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelConditionNote
        '
        Me.LabelConditionNote.Enabled = False
        Me.LabelConditionNote.Location = New System.Drawing.Point(845, 142)
        Me.LabelConditionNote.Name = "LabelConditionNote"
        Me.LabelConditionNote.Size = New System.Drawing.Size(71, 13)
        Me.LabelConditionNote.TabIndex = 25
        Me.LabelConditionNote.Text = "Condition Note"
        '
        'MemoEditConditionNote
        '
        Me.MemoEditConditionNote.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ConditionNote_DEV000221", True))
        Me.MemoEditConditionNote.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditConditionNote, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditConditionNote.Location = New System.Drawing.Point(685, 168)
        Me.MemoEditConditionNote.Name = "MemoEditConditionNote"
        Me.MemoEditConditionNote.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditConditionNote.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditConditionNote, False)
        Me.MemoEditConditionNote.Size = New System.Drawing.Size(272, 84)
        Me.MemoEditConditionNote.TabIndex = 24
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditConditionNote, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelCondition
        '
        Me.LabelCondition.Enabled = False
        Me.LabelCondition.Location = New System.Drawing.Point(632, 142)
        Me.LabelCondition.Name = "LabelCondition"
        Me.LabelCondition.Size = New System.Drawing.Size(45, 13)
        Me.LabelCondition.TabIndex = 23
        Me.LabelCondition.Text = "Condition"
        '
        'ComboBoxEditCondition
        '
        Me.ComboBoxEditCondition.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.Condition_DEV000221", True))
        Me.ComboBoxEditCondition.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxEditCondition, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxEditCondition.Location = New System.Drawing.Point(685, 138)
        Me.ComboBoxEditCondition.Name = "ComboBoxEditCondition"
        Me.ComboBoxEditCondition.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxEditCondition.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxEditCondition.Properties.AutoHeight = False
        Me.ComboBoxEditCondition.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBoxEditCondition.Properties.Items.AddRange(New Object() {"New", "UsedLikeNew", "UsedVeryGood", "UsedGood", "UsedAcceptable", "CollectibleLikeNew", "CollectibleVeryGood", "CollectibleGood", "CollectibleAcceptable", "Refurbished", "Club"})
        Me.ComboBoxEditCondition.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxEditCondition, False)
        Me.ComboBoxEditCondition.Size = New System.Drawing.Size(136, 22)
        Me.ComboBoxEditCondition.TabIndex = 22
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxEditCondition, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelAmazonSellingPrice
        '
        Me.LabelAmazonSellingPrice.Enabled = False
        Me.LabelAmazonSellingPrice.Location = New System.Drawing.Point(7, 37)
        Me.LabelAmazonSellingPrice.Name = "LabelAmazonSellingPrice"
        Me.LabelAmazonSellingPrice.Size = New System.Drawing.Size(97, 13)
        Me.LabelAmazonSellingPrice.TabIndex = 14
        Me.LabelAmazonSellingPrice.Text = "Amazon Selling Price"
        '
        'TextEditAmazonSellingPrice
        '
        Me.TextEditAmazonSellingPrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.SellingPrice_DEV000221", True))
        Me.TextEditAmazonSellingPrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditAmazonSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditAmazonSellingPrice.Location = New System.Drawing.Point(110, 32)
        Me.TextEditAmazonSellingPrice.Name = "TextEditAmazonSellingPrice"
        Me.TextEditAmazonSellingPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditAmazonSellingPrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditAmazonSellingPrice.Properties.AutoHeight = False
        Me.TextEditAmazonSellingPrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditAmazonSellingPrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditAmazonSellingPrice, False)
        Me.TextEditAmazonSellingPrice.Size = New System.Drawing.Size(82, 22)
        Me.TextEditAmazonSellingPrice.TabIndex = 13
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditAmazonSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelReleaseDate
        '
        Me.LabelReleaseDate.Enabled = False
        Me.LabelReleaseDate.Location = New System.Drawing.Point(418, 37)
        Me.LabelReleaseDate.Name = "LabelReleaseDate"
        Me.LabelReleaseDate.Size = New System.Drawing.Size(64, 13)
        Me.LabelReleaseDate.TabIndex = 12
        Me.LabelReleaseDate.Text = "Release Date"
        '
        'DateEditReleaseDate
        '
        Me.DateEditReleaseDate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.ReleaseDate_DEV000221", True))
        Me.DateEditReleaseDate.EditValue = Nothing
        Me.DateEditReleaseDate.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditReleaseDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditReleaseDate.Location = New System.Drawing.Point(488, 32)
        Me.DateEditReleaseDate.Name = "DateEditReleaseDate"
        Me.DateEditReleaseDate.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditReleaseDate.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditReleaseDate.Properties.AutoHeight = False
        Me.DateEditReleaseDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditReleaseDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.DateEditReleaseDate, False)
        Me.DateEditReleaseDate.Size = New System.Drawing.Size(85, 22)
        Me.DateEditReleaseDate.TabIndex = 11
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditReleaseDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelLaunchDate
        '
        Me.LabelLaunchDate.Enabled = False
        Me.LabelLaunchDate.Location = New System.Drawing.Point(250, 37)
        Me.LabelLaunchDate.Name = "LabelLaunchDate"
        Me.LabelLaunchDate.Size = New System.Drawing.Size(60, 13)
        Me.LabelLaunchDate.TabIndex = 10
        Me.LabelLaunchDate.Text = "Launch Date"
        '
        'DateEditLaunchDate
        '
        Me.DateEditLaunchDate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.LaunchDate_DEV000221", True))
        Me.DateEditLaunchDate.EditValue = Nothing
        Me.DateEditLaunchDate.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditLaunchDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditLaunchDate.Location = New System.Drawing.Point(317, 32)
        Me.DateEditLaunchDate.Name = "DateEditLaunchDate"
        Me.DateEditLaunchDate.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditLaunchDate.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditLaunchDate.Properties.AutoHeight = False
        Me.DateEditLaunchDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditLaunchDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.DateEditLaunchDate, False)
        Me.DateEditLaunchDate.Size = New System.Drawing.Size(85, 22)
        Me.DateEditLaunchDate.TabIndex = 9
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditLaunchDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GridControlASIN
        '
        Me.GridControlASIN.DataMember = "InventoryAmazonASIN_DEV000221"
        Me.GridControlASIN.DataSource = Me.InventoryAmazonSettingsSectionGateway
        Me.GridControlASIN.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlASIN, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlASIN, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlASIN.Location = New System.Drawing.Point(5, 247)
        Me.GridControlASIN.MainView = Me.GridViewASIN
        Me.GridControlASIN.Name = "GridControlASIN"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlASIN, False)
        Me.GridControlASIN.Size = New System.Drawing.Size(274, 62)
        Me.GridControlASIN.TabIndex = 8
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
        'LabelAmazonMerchantID
        '
        Me.LabelAmazonMerchantID.Enabled = False
        Me.LabelAmazonMerchantID.Location = New System.Drawing.Point(134, 9)
        Me.LabelAmazonMerchantID.Name = "LabelAmazonMerchantID"
        Me.LabelAmazonMerchantID.Size = New System.Drawing.Size(100, 13)
        Me.LabelAmazonMerchantID.TabIndex = 5
        Me.LabelAmazonMerchantID.Text = "Amazon Merchant ID"
        '
        'ComboBoxEditAmazonMerchantID
        '
        Me.ComboBoxEditAmazonMerchantID.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.MerchantID_DEV000221", True))
        Me.ComboBoxEditAmazonMerchantID.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxEditAmazonMerchantID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxEditAmazonMerchantID.Location = New System.Drawing.Point(240, 4)
        Me.ComboBoxEditAmazonMerchantID.Name = "ComboBoxEditAmazonMerchantID"
        Me.ComboBoxEditAmazonMerchantID.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxEditAmazonMerchantID.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxEditAmazonMerchantID.Properties.AutoHeight = False
        Me.ComboBoxEditAmazonMerchantID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBoxEditAmazonMerchantID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxEditAmazonMerchantID, False)
        Me.ComboBoxEditAmazonMerchantID.Size = New System.Drawing.Size(162, 22)
        Me.ComboBoxEditAmazonMerchantID.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxEditAmazonMerchantID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'CheckEditPublishOnAmazon
        '
        Me.CheckEditPublishOnAmazon.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryAmazonSettingsSectionGateway, "InventoryAmazonDetails_DEV000221.Publish_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditPublishOnAmazon, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckEditPublishOnAmazon.Location = New System.Drawing.Point(5, 5)
        Me.CheckEditPublishOnAmazon.Name = "CheckEditPublishOnAmazon"
        Me.CheckEditPublishOnAmazon.Properties.AutoHeight = False
        Me.CheckEditPublishOnAmazon.Properties.Caption = "Publish on Amazon"
        Me.CheckEditPublishOnAmazon.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.CheckEditPublishOnAmazon, False)
        Me.CheckEditPublishOnAmazon.Size = New System.Drawing.Size(118, 22)
        Me.CheckEditPublishOnAmazon.TabIndex = 3
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditPublishOnAmazon, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixItem
        '
        Me.LabelMatrixItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Location = New System.Drawing.Point(530, 170)
        Me.LabelMatrixItem.Name = "LabelMatrixItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixItem, False)
        Me.LabelMatrixItem.Size = New System.Drawing.Size(147, 65)
        Me.LabelMatrixItem.TabIndex = 64
        Me.LabelMatrixItem.Text = "This is a Matrix Item.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Browse Node" & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "are set from the Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Group Item."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Visible = False
        '
        'LabelMatrixGroupItem
        '
        Me.LabelMatrixGroupItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Location = New System.Drawing.Point(530, 170)
        Me.LabelMatrixGroupItem.Name = "LabelMatrixGroupItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixGroupItem, False)
        Me.LabelMatrixGroupItem.Size = New System.Drawing.Size(141, 78)
        Me.LabelMatrixGroupItem.TabIndex = 63
        Me.LabelMatrixGroupItem.Text = "This is a Matrix Group Item." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Browse " & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Node are applied to every " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Matrix Item in the Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Group."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Visible = False
        '
        'PanelControlDummy
        '
        Me.PanelControlDummy.Controls.Add(Me.lblDevelopment)
        Me.PanelControlDummy.Controls.Add(Me.lblActivate)
        Me.PanelControlDummy.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlDummy.Location = New System.Drawing.Point(2, 2)
        Me.PanelControlDummy.Name = "PanelControlDummy"
        Me.PanelControlDummy.Size = New System.Drawing.Size(964, 487)
        Me.PanelControlDummy.TabIndex = 5
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
        'InventoryAmazonSettingsSectionLayoutGroup
        '
        Me.InventoryAmazonSettingsSectionLayoutGroup.CustomizationFormText = "InventoryAmazonSettingsSectionLayoutGroup"
        Me.InventoryAmazonSettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventoryAmazonSettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.InventoryAmazonSettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventoryAmazonSettingsSectionLayoutGroup.Name = "InventoryAmazonSettingsSectionLayoutGroup"
        Me.InventoryAmazonSettingsSectionLayoutGroup.Size = New System.Drawing.Size(974, 497)
        Me.InventoryAmazonSettingsSectionLayoutGroup.Text = "InventoryAmazonSettingsSectionLayoutGroup"
        Me.InventoryAmazonSettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelPublishOnAmazon
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
        'InventoryAmazonSettingsSection
        '
        Me.Controls.Add(Me.InventoryAmazonSettingsSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventoryAmazonSettingsSection"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me, False)
        Me.Size = New System.Drawing.Size(974, 497)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryAmazonSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryAmazonSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventoryAmazonSettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelPublishOnAmazon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPublishOnAmazon.ResumeLayout(False)
        Me.PanelPublishOnAmazon.PerformLayout()
        CType(Me.TextEditImageURL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditAmazonSiteCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditSaleActive.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditSalePrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSaleEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSaleEndDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSaleStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSaleStartDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEditXMLSubType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEditXMLType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEditXMLClass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditBrowseNodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ListBoxBrowseList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditBrowseTree.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeLocationEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repHelpButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTaxCodeEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlSearchTerms, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewSearchTerms, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditConditionNote.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEditCondition.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditAmazonSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditReleaseDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditReleaseDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditLaunchDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditLaunchDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlASIN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewASIN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEditAmazonMerchantID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditPublishOnAmazon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDummy.ResumeLayout(False)
        Me.PanelControlDummy.PerformLayout()
        CType(Me.InventoryAmazonSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventoryAmazonSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventoryAmazonSettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventoryAmazonSettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelPublishOnAmazon As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelBrowseNodeID As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditBrowseNodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnBrowseNodeUpLevel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelProperties As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ListBoxBrowseList As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents LabelBrowseTree As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditBrowseTree As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelAmazonSiteCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GridControlProperties As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewProperties As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPropertiesItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesLocation_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesName_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesDataType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeDataType As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents colPropertiesDateValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesNumericValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesTextValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSiteCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colParentCategory_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceTagStatus_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceTagConditionality_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceTagDescription_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceTagAcceptedValues_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceTagExample_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colHelp As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repHelpButton As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents GridControlSearchTerms As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewSearchTerms As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colSearchItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSearchLocation_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSearchName_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSearchDataType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSearchTextValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LabelDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelProductName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditProductName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelConditionNote As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditConditionNote As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelCondition As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxEditCondition As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelAmazonSellingPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditAmazonSellingPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelReleaseDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEditReleaseDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelLaunchDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEditLaunchDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents GridControlASIN As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewASIN As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colASIN_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LabelAmazonMerchantID As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxEditAmazonMerchantID As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents CheckEditPublishOnAmazon As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents colPropertiesLineNum_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSearchLineNum_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeLocationEdit As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents rbeDateEdit As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents rbeTextEdit As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents rbeYesNoEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents LabelDescriptiveCategory As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxEditXMLSubType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelXMLSubType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxEditXMLType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelXMLType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxEditXMLClass As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelXMLClass As DevExpress.XtraEditors.LabelControl
    Friend WithEvents CheckEditSaleActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LabelSalePrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditSalePrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelSaleEndDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEditSaleEndDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelSaleStartDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEditSaleStartDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelMatrixGroupItem As System.Windows.Forms.Label
    Friend WithEvents LabelMatrixItem As System.Windows.Forms.Label
    Friend WithEvents TextEditAmazonSiteCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents rbeTaxCodeEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents TextEditImageURL As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelImageURL As DevExpress.XtraEditors.LabelControl
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
