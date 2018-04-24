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

#Region " InventoryShopComSettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryShopComSettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryShopComSettingsSection))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.InventoryShopComSettingsSectionGateway = Me.ImportExportDataset
        Me.InventoryShopComSettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.PanelPublishOnShopCom = New DevExpress.XtraEditors.PanelControl()
        Me.LabelIntermediateCategory = New DevExpress.XtraEditors.LabelControl()
        Me.LabelKeywords = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditKeywords = New DevExpress.XtraEditors.TextEdit()
        Me.LabelMatrixGroupItem = New System.Windows.Forms.Label()
        Me.LabelShopComSellingPrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditShopComSellingPrice = New DevExpress.XtraEditors.TextEdit()
        Me.MemoEditAltImagePrompt = New DevExpress.XtraEditors.MemoEdit()
        Me.LabelAltImagePrompt = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditAltImageURL = New DevExpress.XtraEditors.TextEdit()
        Me.LabelAltImageURL = New DevExpress.XtraEditors.LabelControl()
        Me.CheckEditAltImageAvailable = New DevExpress.XtraEditors.CheckEdit()
        Me.TextEditImageURL = New DevExpress.XtraEditors.TextEdit()
        Me.LabelImageURL = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxThirdLevelDepartment = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelThirdLevelDepartment = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxSecondLevelDepartment = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelSecondLevelDepartment = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxFirstLevelDepartment = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelFirstLevelDepartment = New DevExpress.XtraEditors.LabelControl()
        Me.btnBrowseNodeUpLevel = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelProperties = New DevExpress.XtraEditors.LabelControl()
        Me.ListBoxBrowseList = New DevExpress.XtraEditors.ListBoxControl()
        Me.LabelShopComDepartment = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditShopComDepartment = New DevExpress.XtraEditors.TextEdit()
        Me.GridControlProperties = New DevExpress.XtraGrid.GridControl()
        Me.GridViewProperties = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colItemCode_DEV0002212 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTagLocation_DEV0002211 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeLocationEdit = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.colTagName_DEV0002211 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colLineNum_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTagDataType_DEV0002211 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDataType = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.colTagDateValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTagNumericValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTagTextValue_DEV0002211 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeTextEdit = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.colSourceCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colParentCategory_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colHelp = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repHelpButton = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.rbeDateEdit = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.rbeYesNoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.LabelDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.LabelProductName = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditProductName = New DevExpress.XtraEditors.TextEdit()
        Me.LabelShopComCatalogID = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBoxEditShopComCatalogID = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.CheckPublishOnShopCom = New DevExpress.XtraEditors.CheckEdit()
        Me.LabelMatrixItem = New System.Windows.Forms.Label()
        Me.InventoryShopComSettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.PanelControlDummy = New DevExpress.XtraEditors.PanelControl()
        Me.lblDevelopment = New DevExpress.XtraEditors.LabelControl()
        Me.lblActivate = New DevExpress.XtraEditors.LabelControl()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryShopComSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryShopComSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventoryShopComSettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelPublishOnShopCom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPublishOnShopCom.SuspendLayout()
        CType(Me.TextEditKeywords.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditShopComSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditAltImagePrompt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditAltImageURL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditAltImageAvailable.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditImageURL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxThirdLevelDepartment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxSecondLevelDepartment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxFirstLevelDepartment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ListBoxBrowseList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditShopComDepartment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeLocationEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repHelpButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEditShopComCatalogID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckPublishOnShopCom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryShopComSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDummy.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventoryShopComSettingsSectionGateway
        '
        Me.InventoryShopComSettingsSectionGateway.DataSetName = "InventoryShopComSettingsSectionDataset"
        Me.InventoryShopComSettingsSectionGateway.Instantiate = False
        Me.InventoryShopComSettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventoryShopComSettingsSectionExtendedLayout
        '
        Me.InventoryShopComSettingsSectionExtendedLayout.Controls.Add(Me.PanelPublishOnShopCom)
        Me.InventoryShopComSettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventoryShopComSettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventoryShopComSettingsSectionExtendedLayout.Name = "InventoryShopComSettingsSectionExtendedLayout"
        Me.InventoryShopComSettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventoryShopComSettingsSectionExtendedLayout.Root = Me.InventoryShopComSettingsSectionLayoutGroup
        Me.InventoryShopComSettingsSectionExtendedLayout.Size = New System.Drawing.Size(974, 497)
        Me.InventoryShopComSettingsSectionExtendedLayout.TabIndex = 0
        Me.InventoryShopComSettingsSectionExtendedLayout.Text = "InventoryShopComSettingsSectionExtendedLayout"
        '
        'PanelPublishOnShopCom
        '
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelIntermediateCategory)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelKeywords)
        Me.PanelPublishOnShopCom.Controls.Add(Me.TextEditKeywords)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelMatrixGroupItem)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelShopComSellingPrice)
        Me.PanelPublishOnShopCom.Controls.Add(Me.TextEditShopComSellingPrice)
        Me.PanelPublishOnShopCom.Controls.Add(Me.MemoEditAltImagePrompt)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelAltImagePrompt)
        Me.PanelPublishOnShopCom.Controls.Add(Me.TextEditAltImageURL)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelAltImageURL)
        Me.PanelPublishOnShopCom.Controls.Add(Me.CheckEditAltImageAvailable)
        Me.PanelPublishOnShopCom.Controls.Add(Me.TextEditImageURL)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelImageURL)
        Me.PanelPublishOnShopCom.Controls.Add(Me.ComboBoxThirdLevelDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelThirdLevelDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.ComboBoxSecondLevelDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelSecondLevelDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.ComboBoxFirstLevelDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelFirstLevelDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.btnBrowseNodeUpLevel)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelProperties)
        Me.PanelPublishOnShopCom.Controls.Add(Me.ListBoxBrowseList)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelShopComDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.TextEditShopComDepartment)
        Me.PanelPublishOnShopCom.Controls.Add(Me.GridControlProperties)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelDescription)
        Me.PanelPublishOnShopCom.Controls.Add(Me.MemoEditDescription)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelProductName)
        Me.PanelPublishOnShopCom.Controls.Add(Me.TextEditProductName)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelShopComCatalogID)
        Me.PanelPublishOnShopCom.Controls.Add(Me.ComboBoxEditShopComCatalogID)
        Me.PanelPublishOnShopCom.Controls.Add(Me.CheckPublishOnShopCom)
        Me.PanelPublishOnShopCom.Controls.Add(Me.LabelMatrixItem)
        Me.PanelPublishOnShopCom.Location = New System.Drawing.Point(3, 3)
        Me.PanelPublishOnShopCom.Name = "PanelPublishOnShopCom"
        Me.PanelPublishOnShopCom.Size = New System.Drawing.Size(968, 491)
        Me.PanelPublishOnShopCom.TabIndex = 4
        '
        'LabelIntermediateCategory
        '
        Me.LabelIntermediateCategory.Appearance.ForeColor = System.Drawing.Color.Red
        Me.LabelIntermediateCategory.Enabled = False
        Me.LabelIntermediateCategory.Location = New System.Drawing.Point(312, 180)
        Me.LabelIntermediateCategory.Name = "LabelIntermediateCategory"
        Me.LabelIntermediateCategory.Size = New System.Drawing.Size(110, 13)
        Me.LabelIntermediateCategory.TabIndex = 65
        Me.LabelIntermediateCategory.Text = "Intermediate Category"
        Me.LabelIntermediateCategory.Visible = False
        '
        'LabelKeywords
        '
        Me.LabelKeywords.Enabled = False
        Me.LabelKeywords.Location = New System.Drawing.Point(355, 213)
        Me.LabelKeywords.Name = "LabelKeywords"
        Me.LabelKeywords.Size = New System.Drawing.Size(47, 13)
        Me.LabelKeywords.TabIndex = 64
        Me.LabelKeywords.Text = "Keywords"
        '
        'TextEditKeywords
        '
        Me.TextEditKeywords.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.Keywords_DEV000221", True))
        Me.TextEditKeywords.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditKeywords, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditKeywords.Location = New System.Drawing.Point(413, 209)
        Me.TextEditKeywords.Name = "TextEditKeywords"
        Me.TextEditKeywords.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditKeywords, False)
        Me.TextEditKeywords.Size = New System.Drawing.Size(544, 22)
        Me.TextEditKeywords.TabIndex = 63
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditKeywords, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixGroupItem
        '
        Me.LabelMatrixGroupItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Location = New System.Drawing.Point(423, 58)
        Me.LabelMatrixGroupItem.Name = "LabelMatrixGroupItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixGroupItem, False)
        Me.LabelMatrixGroupItem.Size = New System.Drawing.Size(186, 65)
        Me.LabelMatrixGroupItem.TabIndex = 62
        Me.LabelMatrixGroupItem.Text = "This is a Matrix Group Item.  As such," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the Department, Product Name, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Descripti" & _
            "on and Image URL are all " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "applied to all the Matrix Items in " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the Matrix Group" & _
            "."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Visible = False
        '
        'LabelShopComSellingPrice
        '
        Me.LabelShopComSellingPrice.Enabled = False
        Me.LabelShopComSellingPrice.Location = New System.Drawing.Point(575, 156)
        Me.LabelShopComSellingPrice.Name = "LabelShopComSellingPrice"
        Me.LabelShopComSellingPrice.Size = New System.Drawing.Size(106, 13)
        Me.LabelShopComSellingPrice.TabIndex = 60
        Me.LabelShopComSellingPrice.Text = "Shop.com Selling Price"
        '
        'TextEditShopComSellingPrice
        '
        Me.TextEditShopComSellingPrice.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.SellingPrice_DEV000221", True))
        Me.TextEditShopComSellingPrice.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditShopComSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditShopComSellingPrice.Location = New System.Drawing.Point(689, 152)
        Me.TextEditShopComSellingPrice.Name = "TextEditShopComSellingPrice"
        Me.TextEditShopComSellingPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditShopComSellingPrice.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditShopComSellingPrice.Properties.AutoHeight = False
        Me.TextEditShopComSellingPrice.Properties.EditFormat.FormatString = "0.00"
        Me.TextEditShopComSellingPrice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditShopComSellingPrice, False)
        Me.TextEditShopComSellingPrice.Size = New System.Drawing.Size(100, 22)
        Me.TextEditShopComSellingPrice.TabIndex = 59
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditShopComSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'MemoEditAltImagePrompt
        '
        Me.MemoEditAltImagePrompt.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.AltImagePrompt_DEV000221", True))
        Me.MemoEditAltImagePrompt.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditAltImagePrompt, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditAltImagePrompt.Location = New System.Drawing.Point(83, 415)
        Me.MemoEditAltImagePrompt.Name = "MemoEditAltImagePrompt"
        Me.MemoEditAltImagePrompt.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditAltImagePrompt.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditAltImagePrompt, False)
        Me.MemoEditAltImagePrompt.Size = New System.Drawing.Size(330, 72)
        Me.MemoEditAltImagePrompt.TabIndex = 58
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditAltImagePrompt, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelAltImagePrompt
        '
        Me.LabelAltImagePrompt.Enabled = False
        Me.LabelAltImagePrompt.Location = New System.Drawing.Point(7, 435)
        Me.LabelAltImagePrompt.Name = "LabelAltImagePrompt"
        Me.LabelAltImagePrompt.Size = New System.Drawing.Size(67, 26)
        Me.LabelAltImagePrompt.TabIndex = 57
        Me.LabelAltImagePrompt.Text = "Alternate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Image Prompt"
        '
        'TextEditAltImageURL
        '
        Me.TextEditAltImageURL.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.AltImageURL_DEV000221", True))
        Me.TextEditAltImageURL.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditAltImageURL, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditAltImageURL.Location = New System.Drawing.Point(83, 383)
        Me.TextEditAltImageURL.Name = "TextEditAltImageURL"
        Me.TextEditAltImageURL.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditAltImageURL.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditAltImageURL.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditAltImageURL, False)
        Me.TextEditAltImageURL.Size = New System.Drawing.Size(330, 22)
        Me.TextEditAltImageURL.TabIndex = 56
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditAltImageURL, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelAltImageURL
        '
        Me.LabelAltImageURL.Enabled = False
        Me.LabelAltImageURL.Location = New System.Drawing.Point(8, 379)
        Me.LabelAltImageURL.Name = "LabelAltImageURL"
        Me.LabelAltImageURL.Size = New System.Drawing.Size(52, 26)
        Me.LabelAltImageURL.TabIndex = 55
        Me.LabelAltImageURL.Text = "Alternate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Image URL"
        '
        'CheckEditAltImageAvailable
        '
        Me.CheckEditAltImageAvailable.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.AltImageAvailable_DEV000221", True))
        Me.CheckEditAltImageAvailable.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditAltImageAvailable, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckEditAltImageAvailable.Location = New System.Drawing.Point(6, 351)
        Me.CheckEditAltImageAvailable.Name = "CheckEditAltImageAvailable"
        Me.CheckEditAltImageAvailable.Properties.AutoHeight = False
        Me.CheckEditAltImageAvailable.Properties.Caption = "Alternate Image Available"
        Me.CheckEditAltImageAvailable.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.CheckEditAltImageAvailable, False)
        Me.CheckEditAltImageAvailable.Size = New System.Drawing.Size(165, 22)
        Me.CheckEditAltImageAvailable.TabIndex = 54
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditAltImageAvailable, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditImageURL
        '
        Me.TextEditImageURL.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.ImageURL_DEV000221", True))
        Me.TextEditImageURL.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditImageURL, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditImageURL.Location = New System.Drawing.Point(71, 323)
        Me.TextEditImageURL.Name = "TextEditImageURL"
        Me.TextEditImageURL.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditImageURL.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditImageURL.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditImageURL, False)
        Me.TextEditImageURL.Size = New System.Drawing.Size(342, 22)
        Me.TextEditImageURL.TabIndex = 53
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditImageURL, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelImageURL
        '
        Me.LabelImageURL.Enabled = False
        Me.LabelImageURL.Location = New System.Drawing.Point(8, 327)
        Me.LabelImageURL.Name = "LabelImageURL"
        Me.LabelImageURL.Size = New System.Drawing.Size(52, 13)
        Me.LabelImageURL.TabIndex = 52
        Me.LabelImageURL.Text = "Image URL"
        '
        'ComboBoxThirdLevelDepartment
        '
        Me.ComboBoxThirdLevelDepartment.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.ThirdLevelDepartment_DEV000221", True))
        Me.ComboBoxThirdLevelDepartment.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxThirdLevelDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxThirdLevelDepartment.Location = New System.Drawing.Point(134, 117)
        Me.ComboBoxThirdLevelDepartment.Name = "ComboBoxThirdLevelDepartment"
        Me.ComboBoxThirdLevelDepartment.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxThirdLevelDepartment.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxThirdLevelDepartment.Properties.AutoHeight = False
        Me.ComboBoxThirdLevelDepartment.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxThirdLevelDepartment, False)
        Me.ComboBoxThirdLevelDepartment.Size = New System.Drawing.Size(279, 22)
        Me.ComboBoxThirdLevelDepartment.TabIndex = 51
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxThirdLevelDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelThirdLevelDepartment
        '
        Me.LabelThirdLevelDepartment.Enabled = False
        Me.LabelThirdLevelDepartment.Location = New System.Drawing.Point(8, 122)
        Me.LabelThirdLevelDepartment.Name = "LabelThirdLevelDepartment"
        Me.LabelThirdLevelDepartment.Size = New System.Drawing.Size(112, 13)
        Me.LabelThirdLevelDepartment.TabIndex = 50
        Me.LabelThirdLevelDepartment.Text = "Third Level Department"
        '
        'ComboBoxSecondLevelDepartment
        '
        Me.ComboBoxSecondLevelDepartment.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.SecondLevelDepartment_DEV000221", True))
        Me.ComboBoxSecondLevelDepartment.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxSecondLevelDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxSecondLevelDepartment.Location = New System.Drawing.Point(134, 80)
        Me.ComboBoxSecondLevelDepartment.Name = "ComboBoxSecondLevelDepartment"
        Me.ComboBoxSecondLevelDepartment.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxSecondLevelDepartment.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxSecondLevelDepartment.Properties.AutoHeight = False
        Me.ComboBoxSecondLevelDepartment.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxSecondLevelDepartment, False)
        Me.ComboBoxSecondLevelDepartment.Size = New System.Drawing.Size(279, 22)
        Me.ComboBoxSecondLevelDepartment.TabIndex = 49
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxSecondLevelDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelSecondLevelDepartment
        '
        Me.LabelSecondLevelDepartment.Enabled = False
        Me.LabelSecondLevelDepartment.Location = New System.Drawing.Point(7, 84)
        Me.LabelSecondLevelDepartment.Name = "LabelSecondLevelDepartment"
        Me.LabelSecondLevelDepartment.Size = New System.Drawing.Size(123, 13)
        Me.LabelSecondLevelDepartment.TabIndex = 48
        Me.LabelSecondLevelDepartment.Text = "Second Level Department"
        '
        'ComboBoxFirstLevelDepartment
        '
        Me.ComboBoxFirstLevelDepartment.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.FirstLevelDepartment_DEV000221", True))
        Me.ComboBoxFirstLevelDepartment.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxFirstLevelDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxFirstLevelDepartment.Location = New System.Drawing.Point(134, 43)
        Me.ComboBoxFirstLevelDepartment.Name = "ComboBoxFirstLevelDepartment"
        Me.ComboBoxFirstLevelDepartment.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxFirstLevelDepartment.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxFirstLevelDepartment.Properties.AutoHeight = False
        Me.ComboBoxFirstLevelDepartment.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxFirstLevelDepartment, False)
        Me.ComboBoxFirstLevelDepartment.Size = New System.Drawing.Size(279, 22)
        Me.ComboBoxFirstLevelDepartment.TabIndex = 47
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxFirstLevelDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelFirstLevelDepartment
        '
        Me.LabelFirstLevelDepartment.Enabled = False
        Me.LabelFirstLevelDepartment.Location = New System.Drawing.Point(8, 47)
        Me.LabelFirstLevelDepartment.Name = "LabelFirstLevelDepartment"
        Me.LabelFirstLevelDepartment.Size = New System.Drawing.Size(109, 13)
        Me.LabelFirstLevelDepartment.TabIndex = 46
        Me.LabelFirstLevelDepartment.Text = "First Level Department"
        '
        'btnBrowseNodeUpLevel
        '
        Me.btnBrowseNodeUpLevel.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.btnBrowseNodeUpLevel, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnBrowseNodeUpLevel.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.UpLevel
        Me.btnBrowseNodeUpLevel.Location = New System.Drawing.Point(510, 151)
        Me.btnBrowseNodeUpLevel.Name = "btnBrowseNodeUpLevel"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.btnBrowseNodeUpLevel, False)
        Me.btnBrowseNodeUpLevel.Size = New System.Drawing.Size(21, 23)
        Me.btnBrowseNodeUpLevel.TabIndex = 45
        Me.ExtendControlProperty.SetTextDisplay(Me.btnBrowseNodeUpLevel, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnBrowseNodeUpLevel.ToolTip = "Up Browse Tree Node"
        '
        'LabelProperties
        '
        Me.LabelProperties.Enabled = False
        Me.LabelProperties.Location = New System.Drawing.Point(426, 242)
        Me.LabelProperties.Name = "LabelProperties"
        Me.LabelProperties.Size = New System.Drawing.Size(80, 13)
        Me.LabelProperties.TabIndex = 44
        Me.LabelProperties.Text = "Other Properties"
        '
        'ListBoxBrowseList
        '
        Me.ListBoxBrowseList.Enabled = False
        Me.ListBoxBrowseList.Location = New System.Drawing.Point(8, 186)
        Me.ListBoxBrowseList.Name = "ListBoxBrowseList"
        Me.ListBoxBrowseList.Size = New System.Drawing.Size(272, 126)
        Me.ListBoxBrowseList.SortOrder = System.Windows.Forms.SortOrder.Ascending
        Me.ListBoxBrowseList.TabIndex = 43
        '
        'LabelShopComDepartment
        '
        Me.LabelShopComDepartment.Enabled = False
        Me.LabelShopComDepartment.Location = New System.Drawing.Point(8, 150)
        Me.LabelShopComDepartment.Name = "LabelShopComDepartment"
        Me.LabelShopComDepartment.Size = New System.Drawing.Size(107, 26)
        Me.LabelShopComDepartment.TabIndex = 42
        Me.LabelShopComDepartment.Text = "Shop.com Department" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Category)"
        '
        'TextEditShopComDepartment
        '
        Me.TextEditShopComDepartment.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.CategoryTree_DEV000221", True))
        Me.TextEditShopComDepartment.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditShopComDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditShopComDepartment.Location = New System.Drawing.Point(134, 152)
        Me.TextEditShopComDepartment.Name = "TextEditShopComDepartment"
        Me.TextEditShopComDepartment.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextEditShopComDepartment.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditShopComDepartment.Properties.AutoHeight = False
        Me.TextEditShopComDepartment.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditShopComDepartment, False)
        Me.TextEditShopComDepartment.Size = New System.Drawing.Size(370, 22)
        Me.TextEditShopComDepartment.TabIndex = 41
        Me.TextEditShopComDepartment.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditShopComDepartment, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GridControlProperties
        '
        Me.GridControlProperties.DataMember = "InventoryShopComTagDetailTemplateView_DEV000221"
        Me.GridControlProperties.DataSource = Me.InventoryShopComSettingsSectionGateway
        Me.GridControlProperties.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlProperties, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.Location = New System.Drawing.Point(426, 266)
        Me.GridControlProperties.MainView = Me.GridViewProperties
        Me.GridControlProperties.Name = "GridControlProperties"
        Me.GridControlProperties.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeDataType, Me.repHelpButton, Me.rbeDateEdit, Me.rbeTextEdit, Me.rbeYesNoEdit, Me.rbeLocationEdit})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlProperties, False)
        Me.GridControlProperties.Size = New System.Drawing.Size(531, 221)
        Me.GridControlProperties.TabIndex = 40
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewProperties})
        '
        'GridViewProperties
        '
        Me.GridViewProperties.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colItemCode_DEV0002212, Me.colTagLocation_DEV0002211, Me.colTagName_DEV0002211, Me.colLineNum_DEV000221, Me.colTagDataType_DEV0002211, Me.colTagDateValue_DEV000221, Me.colTagNumericValue_DEV000221, Me.colTagTextValue_DEV0002211, Me.colSourceCode_DEV000221, Me.colParentCategory_DEV000221, Me.colHelp})
        Me.GridViewProperties.GridControl = Me.GridControlProperties
        Me.GridViewProperties.Name = "GridViewProperties"
        Me.GridViewProperties.OptionsView.ShowGroupPanel = False
        '
        'colItemCode_DEV0002212
        '
        Me.colItemCode_DEV0002212.Caption = "ItemCode_DEV000221"
        Me.colItemCode_DEV0002212.FieldName = "ItemCode_DEV000221"
        Me.colItemCode_DEV0002212.Name = "colItemCode_DEV0002212"
        Me.ExtendControlProperty.SetTextDisplay(Me.colItemCode_DEV0002212, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colTagLocation_DEV0002211
        '
        Me.colTagLocation_DEV0002211.Caption = "Location"
        Me.colTagLocation_DEV0002211.ColumnEdit = Me.rbeLocationEdit
        Me.colTagLocation_DEV0002211.FieldName = "TagLocation_DEV000221"
        Me.colTagLocation_DEV0002211.Name = "colTagLocation_DEV0002211"
        Me.ExtendControlProperty.SetTextDisplay(Me.colTagLocation_DEV0002211, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colTagLocation_DEV0002211.Visible = True
        Me.colTagLocation_DEV0002211.VisibleIndex = 0
        Me.colTagLocation_DEV0002211.Width = 66
        '
        'rbeLocationEdit
        '
        Me.rbeLocationEdit.AutoHeight = False
        Me.rbeLocationEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeLocationEdit.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Container", 1, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Product", 2, -1)})
        Me.rbeLocationEdit.Name = "rbeLocationEdit"
        '
        'colTagName_DEV0002211
        '
        Me.colTagName_DEV0002211.Caption = "Name"
        Me.colTagName_DEV0002211.FieldName = "TagName_DEV000221"
        Me.colTagName_DEV0002211.Name = "colTagName_DEV0002211"
        Me.ExtendControlProperty.SetTextDisplay(Me.colTagName_DEV0002211, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colTagName_DEV0002211.Visible = True
        Me.colTagName_DEV0002211.VisibleIndex = 1
        Me.colTagName_DEV0002211.Width = 123
        '
        'colLineNum_DEV000221
        '
        Me.colLineNum_DEV000221.Caption = "Pos'n"
        Me.colLineNum_DEV000221.FieldName = "LineNum_DEV000221"
        Me.colLineNum_DEV000221.Name = "colLineNum_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colLineNum_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colLineNum_DEV000221.Visible = True
        Me.colLineNum_DEV000221.VisibleIndex = 2
        Me.colLineNum_DEV000221.Width = 45
        '
        'colTagDataType_DEV0002211
        '
        Me.colTagDataType_DEV0002211.Caption = "Format"
        Me.colTagDataType_DEV0002211.ColumnEdit = Me.rbeDataType
        Me.colTagDataType_DEV0002211.FieldName = "TagDataType_DEV000221"
        Me.colTagDataType_DEV0002211.Name = "colTagDataType_DEV0002211"
        Me.ExtendControlProperty.SetTextDisplay(Me.colTagDataType_DEV0002211, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colTagDataType_DEV0002211.Visible = True
        Me.colTagDataType_DEV0002211.VisibleIndex = 3
        Me.colTagDataType_DEV0002211.Width = 57
        '
        'rbeDataType
        '
        Me.rbeDataType.AutoHeight = False
        Me.rbeDataType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeDataType.Items.AddRange(New Object() {"Text", "Date", "DateTime", "Integer", "Numeric"})
        Me.rbeDataType.Name = "rbeDataType"
        '
        'colTagDateValue_DEV000221
        '
        Me.colTagDateValue_DEV000221.Caption = "TagDateValue_DEV000221"
        Me.colTagDateValue_DEV000221.FieldName = "TagDateValue_DEV000221"
        Me.colTagDateValue_DEV000221.Name = "colTagDateValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colTagDateValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colTagNumericValue_DEV000221
        '
        Me.colTagNumericValue_DEV000221.Caption = "TagNumericValue_DEV000221"
        Me.colTagNumericValue_DEV000221.FieldName = "TagNumericValue_DEV000221"
        Me.colTagNumericValue_DEV000221.Name = "colTagNumericValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colTagNumericValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colTagTextValue_DEV0002211
        '
        Me.colTagTextValue_DEV0002211.Caption = "Value"
        Me.colTagTextValue_DEV0002211.ColumnEdit = Me.rbeTextEdit
        Me.colTagTextValue_DEV0002211.FieldName = "TagTextValue_DEV000221"
        Me.colTagTextValue_DEV0002211.Name = "colTagTextValue_DEV0002211"
        Me.ExtendControlProperty.SetTextDisplay(Me.colTagTextValue_DEV0002211, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colTagTextValue_DEV0002211.Visible = True
        Me.colTagTextValue_DEV0002211.VisibleIndex = 4
        Me.colTagTextValue_DEV0002211.Width = 190
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
        'colParentCategory_DEV000221
        '
        Me.colParentCategory_DEV000221.Caption = "ParentCategory_DEV000221"
        Me.colParentCategory_DEV000221.FieldName = "ParentCategory_DEV000221"
        Me.colParentCategory_DEV000221.Name = "colParentCategory_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colParentCategory_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
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
        'LabelDescription
        '
        Me.LabelDescription.Enabled = False
        Me.LabelDescription.Location = New System.Drawing.Point(628, 47)
        Me.LabelDescription.Name = "LabelDescription"
        Me.LabelDescription.Size = New System.Drawing.Size(53, 13)
        Me.LabelDescription.TabIndex = 33
        Me.LabelDescription.Text = "Description"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.GroupDescription_DEV000221", True))
        Me.MemoEditDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditDescription.Location = New System.Drawing.Point(689, 44)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditDescription, False)
        Me.MemoEditDescription.Size = New System.Drawing.Size(272, 96)
        Me.MemoEditDescription.TabIndex = 32
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelProductName
        '
        Me.LabelProductName.Enabled = False
        Me.LabelProductName.Location = New System.Drawing.Point(614, 10)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New System.Drawing.Size(67, 13)
        Me.LabelProductName.TabIndex = 31
        Me.LabelProductName.Text = "Product Name"
        '
        'TextEditProductName
        '
        Me.TextEditProductName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.GroupName_DEV000221", True))
        Me.TextEditProductName.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditProductName.Location = New System.Drawing.Point(689, 5)
        Me.TextEditProductName.Name = "TextEditProductName"
        Me.TextEditProductName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditProductName.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditProductName.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditProductName, False)
        Me.TextEditProductName.Size = New System.Drawing.Size(272, 22)
        Me.TextEditProductName.TabIndex = 30
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelShopComCatalogID
        '
        Me.LabelShopComCatalogID.Enabled = False
        Me.LabelShopComCatalogID.Location = New System.Drawing.Point(134, 9)
        Me.LabelShopComCatalogID.Name = "LabelShopComCatalogID"
        Me.LabelShopComCatalogID.Size = New System.Drawing.Size(101, 13)
        Me.LabelShopComCatalogID.TabIndex = 8
        Me.LabelShopComCatalogID.Text = "Shop.com Catalog ID"
        '
        'ComboBoxEditShopComCatalogID
        '
        Me.ComboBoxEditShopComCatalogID.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.InventoryShopComSettingsSectionGateway, "InventoryShopComDetails_DEV000221.CatalogID_DEV000221", True))
        Me.ComboBoxEditShopComCatalogID.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.ComboBoxEditShopComCatalogID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ComboBoxEditShopComCatalogID.Location = New System.Drawing.Point(240, 4)
        Me.ComboBoxEditShopComCatalogID.Name = "ComboBoxEditShopComCatalogID"
        Me.ComboBoxEditShopComCatalogID.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBoxEditShopComCatalogID.Properties.Appearance.Options.UseBackColor = True
        Me.ComboBoxEditShopComCatalogID.Properties.AutoHeight = False
        Me.ComboBoxEditShopComCatalogID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBoxEditShopComCatalogID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.ComboBoxEditShopComCatalogID, False)
        Me.ComboBoxEditShopComCatalogID.Size = New System.Drawing.Size(162, 22)
        Me.ComboBoxEditShopComCatalogID.TabIndex = 7
        Me.ExtendControlProperty.SetTextDisplay(Me.ComboBoxEditShopComCatalogID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'CheckPublishOnShopCom
        '
        Me.ExtendControlProperty.SetHelpText(Me.CheckPublishOnShopCom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckPublishOnShopCom.Location = New System.Drawing.Point(5, 5)
        Me.CheckPublishOnShopCom.Name = "CheckPublishOnShopCom"
        Me.CheckPublishOnShopCom.Properties.AutoHeight = False
        Me.CheckPublishOnShopCom.Properties.Caption = "Publish on Shop.com"
        Me.CheckPublishOnShopCom.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.CheckPublishOnShopCom, False)
        Me.CheckPublishOnShopCom.Size = New System.Drawing.Size(123, 22)
        Me.CheckPublishOnShopCom.TabIndex = 6
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckPublishOnShopCom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixItem
        '
        Me.LabelMatrixItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Location = New System.Drawing.Point(423, 65)
        Me.LabelMatrixItem.Name = "LabelMatrixItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixItem, False)
        Me.LabelMatrixItem.Size = New System.Drawing.Size(164, 52)
        Me.LabelMatrixItem.TabIndex = 61
        Me.LabelMatrixItem.Text = "This is a Matrix Item.  As such, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the Department, Product Name, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description, a" & _
            "nd Image URL are " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "set from the Matrix Group Item."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Visible = False
        '
        'InventoryShopComSettingsSectionLayoutGroup
        '
        Me.InventoryShopComSettingsSectionLayoutGroup.CustomizationFormText = "InventoryShopComSettingsSectionLayoutGroup"
        Me.InventoryShopComSettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventoryShopComSettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.InventoryShopComSettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventoryShopComSettingsSectionLayoutGroup.Name = "InventoryShopComSettingsSectionLayoutGroup"
        Me.InventoryShopComSettingsSectionLayoutGroup.Size = New System.Drawing.Size(974, 497)
        Me.InventoryShopComSettingsSectionLayoutGroup.Text = "InventoryShopComSettingsSectionLayoutGroup"
        Me.InventoryShopComSettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PanelPublishOnShopCom
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
        'InventoryShopComSettingsSection
        '
        Me.Controls.Add(Me.InventoryShopComSettingsSectionExtendedLayout)
        Me.Controls.Add(Me.PanelControlDummy)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventoryShopComSettingsSection"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me, False)
        Me.Size = New System.Drawing.Size(974, 497)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryShopComSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryShopComSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventoryShopComSettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelPublishOnShopCom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPublishOnShopCom.ResumeLayout(False)
        Me.PanelPublishOnShopCom.PerformLayout()
        CType(Me.TextEditKeywords.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditShopComSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditAltImagePrompt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditAltImageURL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditAltImageAvailable.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditImageURL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxThirdLevelDepartment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxSecondLevelDepartment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxFirstLevelDepartment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ListBoxBrowseList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditShopComDepartment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeLocationEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repHelpButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEditShopComCatalogID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckPublishOnShopCom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryShopComSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDummy.ResumeLayout(False)
        Me.PanelControlDummy.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventoryShopComSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventoryShopComSettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventoryShopComSettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelPublishOnShopCom As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelShopComCatalogID As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxEditShopComCatalogID As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents CheckPublishOnShopCom As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LabelDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelProductName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditProductName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnBrowseNodeUpLevel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelProperties As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ListBoxBrowseList As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents LabelShopComDepartment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditShopComDepartment As DevExpress.XtraEditors.TextEdit
    Friend WithEvents GridControlProperties As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewProperties As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colItemCode_DEV0002212 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTagLocation_DEV0002211 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTagName_DEV0002211 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTagDataType_DEV0002211 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeDataType As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents colTagDateValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTagNumericValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTagTextValue_DEV0002211 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colParentCategory_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colHelp As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ComboBoxThirdLevelDepartment As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelThirdLevelDepartment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxSecondLevelDepartment As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelSecondLevelDepartment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBoxFirstLevelDepartment As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelFirstLevelDepartment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents CheckEditAltImageAvailable As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents TextEditImageURL As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelImageURL As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditAltImageURL As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelAltImageURL As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelAltImagePrompt As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditAltImagePrompt As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelShopComSellingPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditShopComSellingPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents colLineNum_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LabelMatrixGroupItem As System.Windows.Forms.Label
    Friend WithEvents LabelMatrixItem As System.Windows.Forms.Label
    Friend WithEvents LabelKeywords As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditKeywords As DevExpress.XtraEditors.TextEdit
    Friend WithEvents rbeLocationEdit As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents rbeDateEdit As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents rbeTextEdit As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents rbeYesNoEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents repHelpButton As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents LabelIntermediateCategory As DevExpress.XtraEditors.LabelControl
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
