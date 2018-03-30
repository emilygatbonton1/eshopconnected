'===============================================================================
' Interprise Suite SDK
' Copyright © 2004-2007 Interprise Software Systems International Inc.
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

#Region " ConfigSettingsDetailSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigSettingsDetailSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigSettingsDetailSection))
        Me.ConfigSettingsSectionGateway = Me.ImportExportDataset
        Me.ConfigSettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.LabelFiller = New DevExpress.XtraEditors.LabelControl()
        Me.btnChangePwd = New DevExpress.XtraEditors.SimpleButton()
        Me.GroupControlActivationCode = New DevExpress.XtraEditors.GroupControl()
        Me.LabelActivationStatus = New DevExpress.XtraEditors.LabelControl()
        Me.LabelInitialHyphen6 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelInitialHyphen5 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelInitialHyphen4 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelInitialHyphen3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelInitialHyphen2 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelInitialHyphen1 = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditInitialActivation1 = New DevExpress.XtraEditors.TextEdit()
        Me.TextEditInputMode = New DevExpress.XtraEditors.TextEdit()
        Me.ImageComboBoxEditInputHandler = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.GridControlConfigSettings = New DevExpress.XtraGrid.GridControl()
        Me.GridViewConfigSettings = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colConfigGroup = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colConfigSettingName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colConfigSettingValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeTextEdit = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.colConfigSettingID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeYesNoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeISItemIDEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeCustBizTypeEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeISHypLinkEdit = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.rbeShipModuleEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeShopComSourceIDEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeShopComXMLDateEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeAmazonSiteEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeChanAdvNoPmtAction = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeEbayCountryEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeEBayGenAuthToken = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.rbeOverrideSourcePrice = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.TextEditSourcePwd = New DevExpress.XtraEditors.TextEdit()
        Me.TextEditSourceName = New DevExpress.XtraEditors.TextEdit()
        Me.TextEditSourceCode = New DevExpress.XtraEditors.TextEdit()
        Me.ConfigSettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutItemSourceCode = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutItemSourceName = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutItemSourcePwd = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutItemConfigSettings = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutItemInputHandler = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayouItemInputMode = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutItemActivationCode = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem2 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.rbeAmazonMerchantIDEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ConfigSettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.GroupControlActivationCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControlActivationCode.SuspendLayout()
        CType(Me.TextEditInitialActivation1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditInputMode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageComboBoxEditInputHandler.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlConfigSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewConfigSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeISItemIDEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeCustBizTypeEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeISHypLinkEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeShipModuleEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeShopComSourceIDEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeShopComXMLDateEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeAmazonSiteEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeChanAdvNoPmtAction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeEbayCountryEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeEBayGenAuthToken, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeOverrideSourcePrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditSourcePwd.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditSourceName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditSourceCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemSourceCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemSourceName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemSourcePwd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemConfigSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemInputHandler, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayouItemInputMode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemActivationCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeAmazonMerchantIDEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'ConfigSettingsSectionGateway
        '
        Me.ConfigSettingsSectionGateway.DataSetName = "ConfigSettingsSectionDataset"
        Me.ConfigSettingsSectionGateway.Instantiate = False
        Me.ConfigSettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'ConfigSettingsSectionExtendedLayout
        '
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.LabelFiller)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.btnChangePwd)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.GroupControlActivationCode)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.TextEditInputMode)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.ImageComboBoxEditInputHandler)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.GridControlConfigSettings)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.TextEditSourcePwd)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.TextEditSourceName)
        Me.ConfigSettingsSectionExtendedLayout.Controls.Add(Me.TextEditSourceCode)
        Me.ConfigSettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ConfigSettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsSectionExtendedLayout.Name = "ConfigSettingsSectionExtendedLayout"
        Me.ConfigSettingsSectionExtendedLayout.OptionsFocus.EnableAutoTabOrder = False
        Me.ConfigSettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.ConfigSettingsSectionExtendedLayout.Root = Me.ConfigSettingsSectionLayoutGroup
        Me.ConfigSettingsSectionExtendedLayout.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsSectionExtendedLayout.TabIndex = 0
        Me.ConfigSettingsSectionExtendedLayout.Text = "ConfigSettingsSectionExtendedLayout"
        '
        'LabelFiller
        '
        Me.LabelFiller.Location = New System.Drawing.Point(366, 59)
        Me.LabelFiller.Name = "LabelFiller"
        Me.LabelFiller.Size = New System.Drawing.Size(322, 22)
        Me.LabelFiller.StyleController = Me.ConfigSettingsSectionExtendedLayout
        Me.LabelFiller.TabIndex = 16
        '
        'btnChangePwd
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnChangePwd, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnChangePwd.Location = New System.Drawing.Point(300, 59)
        Me.btnChangePwd.MaximumSize = New System.Drawing.Size(60, 22)
        Me.btnChangePwd.MinimumSize = New System.Drawing.Size(60, 22)
        Me.btnChangePwd.Name = "btnChangePwd"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnChangePwd, System.Drawing.Color.Empty)
        Me.btnChangePwd.Size = New System.Drawing.Size(60, 22)
        Me.btnChangePwd.StyleController = Me.ConfigSettingsSectionExtendedLayout
        Me.btnChangePwd.TabIndex = 13
        Me.btnChangePwd.Text = "Change"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnChangePwd, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GroupControlActivationCode
        '
        Me.GroupControlActivationCode.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.GroupControlActivationCode.Controls.Add(Me.LabelActivationStatus)
        Me.GroupControlActivationCode.Controls.Add(Me.LabelInitialHyphen6)
        Me.GroupControlActivationCode.Controls.Add(Me.LabelInitialHyphen5)
        Me.GroupControlActivationCode.Controls.Add(Me.LabelInitialHyphen4)
        Me.GroupControlActivationCode.Controls.Add(Me.LabelInitialHyphen3)
        Me.GroupControlActivationCode.Controls.Add(Me.LabelInitialHyphen2)
        Me.GroupControlActivationCode.Controls.Add(Me.LabelInitialHyphen1)
        Me.GroupControlActivationCode.Controls.Add(Me.TextEditInitialActivation1)
        Me.GroupControlActivationCode.Location = New System.Drawing.Point(89, 115)
        Me.GroupControlActivationCode.MaximumSize = New System.Drawing.Size(0, 22)
        Me.GroupControlActivationCode.MinimumSize = New System.Drawing.Size(0, 22)
        Me.GroupControlActivationCode.Name = "GroupControlActivationCode"
        Me.GroupControlActivationCode.Size = New System.Drawing.Size(599, 22)
        Me.GroupControlActivationCode.TabIndex = 15
        '
        'LabelActivationStatus
        '
        Me.LabelActivationStatus.Appearance.ForeColor = System.Drawing.Color.Red
        Me.LabelActivationStatus.Location = New System.Drawing.Point(355, 1)
        Me.LabelActivationStatus.MaximumSize = New System.Drawing.Size(0, 20)
        Me.LabelActivationStatus.MinimumSize = New System.Drawing.Size(0, 20)
        Me.LabelActivationStatus.Name = "LabelActivationStatus"
        Me.LabelActivationStatus.Size = New System.Drawing.Size(135, 20)
        Me.LabelActivationStatus.TabIndex = 44
        Me.LabelActivationStatus.Text = "Activation Code has expired"
        '
        'LabelInitialHyphen6
        '
        Me.LabelInitialHyphen6.Appearance.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.LabelInitialHyphen6.Location = New System.Drawing.Point(322, -15)
        Me.LabelInitialHyphen6.Name = "LabelInitialHyphen6"
        Me.LabelInitialHyphen6.Size = New System.Drawing.Size(4, 11)
        Me.LabelInitialHyphen6.TabIndex = 43
        Me.LabelInitialHyphen6.Text = "-"
        '
        'LabelInitialHyphen5
        '
        Me.LabelInitialHyphen5.Appearance.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.LabelInitialHyphen5.Location = New System.Drawing.Point(267, -15)
        Me.LabelInitialHyphen5.Name = "LabelInitialHyphen5"
        Me.LabelInitialHyphen5.Size = New System.Drawing.Size(4, 11)
        Me.LabelInitialHyphen5.TabIndex = 42
        Me.LabelInitialHyphen5.Text = "-"
        '
        'LabelInitialHyphen4
        '
        Me.LabelInitialHyphen4.Appearance.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.LabelInitialHyphen4.Location = New System.Drawing.Point(212, -15)
        Me.LabelInitialHyphen4.Name = "LabelInitialHyphen4"
        Me.LabelInitialHyphen4.Size = New System.Drawing.Size(4, 11)
        Me.LabelInitialHyphen4.TabIndex = 41
        Me.LabelInitialHyphen4.Text = "-"
        '
        'LabelInitialHyphen3
        '
        Me.LabelInitialHyphen3.Appearance.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.LabelInitialHyphen3.Location = New System.Drawing.Point(157, -15)
        Me.LabelInitialHyphen3.Name = "LabelInitialHyphen3"
        Me.LabelInitialHyphen3.Size = New System.Drawing.Size(4, 11)
        Me.LabelInitialHyphen3.TabIndex = 40
        Me.LabelInitialHyphen3.Text = "-"
        '
        'LabelInitialHyphen2
        '
        Me.LabelInitialHyphen2.Appearance.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.LabelInitialHyphen2.Location = New System.Drawing.Point(102, -15)
        Me.LabelInitialHyphen2.Name = "LabelInitialHyphen2"
        Me.LabelInitialHyphen2.Size = New System.Drawing.Size(4, 11)
        Me.LabelInitialHyphen2.TabIndex = 39
        Me.LabelInitialHyphen2.Text = "-"
        '
        'LabelInitialHyphen1
        '
        Me.LabelInitialHyphen1.Appearance.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.LabelInitialHyphen1.Location = New System.Drawing.Point(47, -15)
        Me.LabelInitialHyphen1.Name = "LabelInitialHyphen1"
        Me.LabelInitialHyphen1.Size = New System.Drawing.Size(4, 11)
        Me.LabelInitialHyphen1.TabIndex = 33
        Me.LabelInitialHyphen1.Text = "-"
        '
        'TextEditInitialActivation1
        '
        Me.TextEditInitialActivation1.EditValue = ""
        Me.ExtendControlProperty.SetHelpText(Me.TextEditInitialActivation1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditInitialActivation1.Location = New System.Drawing.Point(1, 0)
        Me.TextEditInitialActivation1.Name = "TextEditInitialActivation1"
        Me.TextEditInitialActivation1.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditInitialActivation1.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEditInitialActivation1.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditInitialActivation1.Properties.Appearance.Options.UseFont = True
        Me.TextEditInitialActivation1.Properties.AutoHeight = False
        Me.TextEditInitialActivation1.Properties.Mask.EditMask = "AAAAA-AAAAA-AAAAA-AAAAA-AAAAA-AAAAA-AAAAA"
        Me.TextEditInitialActivation1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple
        Me.TextEditInitialActivation1.Properties.MaxLength = 41
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditInitialActivation1, System.Drawing.Color.Empty)
        Me.TextEditInitialActivation1.Size = New System.Drawing.Size(342, 22)
        Me.TextEditInitialActivation1.TabIndex = 6
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditInitialActivation1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditInputMode
        '
        Me.TextEditInputMode.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ConfigSettingsSectionGateway, "LerrynImportExportConfig_DEV000221.InputMode_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.TextEditInputMode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditInputMode.Location = New System.Drawing.Point(432, 87)
        Me.TextEditInputMode.MaximumSize = New System.Drawing.Size(150, 22)
        Me.TextEditInputMode.MinimumSize = New System.Drawing.Size(150, 22)
        Me.TextEditInputMode.Name = "TextEditInputMode"
        Me.TextEditInputMode.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextEditInputMode.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditInputMode.Properties.AutoHeight = False
        Me.TextEditInputMode.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditInputMode, System.Drawing.Color.Empty)
        Me.TextEditInputMode.Size = New System.Drawing.Size(150, 22)
        Me.TextEditInputMode.StyleController = Me.ConfigSettingsSectionExtendedLayout
        Me.TextEditInputMode.TabIndex = 5
        Me.TextEditInputMode.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditInputMode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'ImageComboBoxEditInputHandler
        '
        Me.ImageComboBoxEditInputHandler.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ConfigSettingsSectionGateway, "LerrynImportExportConfig_DEV000221.InputHandler_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.ImageComboBoxEditInputHandler, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ImageComboBoxEditInputHandler.Location = New System.Drawing.Point(89, 87)
        Me.ImageComboBoxEditInputHandler.MaximumSize = New System.Drawing.Size(200, 22)
        Me.ImageComboBoxEditInputHandler.MinimumSize = New System.Drawing.Size(200, 22)
        Me.ImageComboBoxEditInputHandler.Name = "ImageComboBoxEditInputHandler"
        Me.ImageComboBoxEditInputHandler.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ImageComboBoxEditInputHandler.Properties.Appearance.Options.UseBackColor = True
        Me.ImageComboBoxEditInputHandler.Properties.AutoHeight = False
        Me.ImageComboBoxEditInputHandler.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ImageComboBoxEditInputHandler.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.ImageComboBoxEditInputHandler, System.Drawing.Color.Empty)
        Me.ImageComboBoxEditInputHandler.Size = New System.Drawing.Size(200, 22)
        Me.ImageComboBoxEditInputHandler.StyleController = Me.ConfigSettingsSectionExtendedLayout
        Me.ImageComboBoxEditInputHandler.TabIndex = 4
        Me.ImageComboBoxEditInputHandler.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.ImageComboBoxEditInputHandler, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'GridControlConfigSettings
        '
        Me.GridControlConfigSettings.DataMember = "XMLConfigSettings"
        Me.GridControlConfigSettings.DataSource = Me.ConfigSettingsSectionGateway
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlConfigSettings, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlConfigSettings, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlConfigSettings.Location = New System.Drawing.Point(89, 143)
        Me.GridControlConfigSettings.MainView = Me.GridViewConfigSettings
        Me.GridControlConfigSettings.Name = "GridControlConfigSettings"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlConfigSettings, System.Drawing.Color.Empty)
        Me.GridControlConfigSettings.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeYesNoEdit, Me.rbeISItemIDEdit, Me.rbeCustBizTypeEdit, Me.rbeISHypLinkEdit, Me.rbeShipModuleEdit, Me.rbeShopComSourceIDEdit, Me.rbeShopComXMLDateEdit, Me.rbeAmazonSiteEdit, Me.rbeTextEdit, Me.rbeChanAdvNoPmtAction, Me.rbeEbayCountryEdit, Me.rbeEBayGenAuthToken, Me.rbeAmazonMerchantIDEdit})
        Me.GridControlConfigSettings.Size = New System.Drawing.Size(599, 273)
        Me.GridControlConfigSettings.TabIndex = 14
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlConfigSettings, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlConfigSettings.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewConfigSettings})
        '
        'GridViewConfigSettings
        '
        Me.GridViewConfigSettings.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colConfigGroup, Me.colConfigSettingName, Me.colConfigSettingValue, Me.colConfigSettingID})
        Me.GridViewConfigSettings.GridControl = Me.GridControlConfigSettings
        Me.GridViewConfigSettings.GroupCount = 1
        Me.GridViewConfigSettings.Name = "GridViewConfigSettings"
        Me.GridViewConfigSettings.OptionsMenu.EnableGroupPanelMenu = False
        Me.GridViewConfigSettings.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.GridViewConfigSettings.OptionsView.ShowGroupPanel = False
        Me.GridViewConfigSettings.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colConfigGroup, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colConfigGroup
        '
        Me.colConfigGroup.Caption = "Config Group"
        Me.colConfigGroup.FieldName = "ConfigGroup"
        Me.colConfigGroup.Name = "colConfigGroup"
        Me.ExtendControlProperty.SetTextDisplay(Me.colConfigGroup, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colConfigSettingName
        '
        Me.colConfigSettingName.Caption = "Config Setting Name"
        Me.colConfigSettingName.FieldName = "ConfigSettingName"
        Me.colConfigSettingName.Name = "colConfigSettingName"
        Me.colConfigSettingName.OptionsColumn.AllowEdit = False
        Me.colConfigSettingName.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colConfigSettingName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colConfigSettingName.Visible = True
        Me.colConfigSettingName.VisibleIndex = 0
        Me.colConfigSettingName.Width = 200
        '
        'colConfigSettingValue
        '
        Me.colConfigSettingValue.Caption = "Config Setting Value"
        Me.colConfigSettingValue.ColumnEdit = Me.rbeTextEdit
        Me.colConfigSettingValue.FieldName = "ConfigSettingValue"
        Me.colConfigSettingValue.Name = "colConfigSettingValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colConfigSettingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colConfigSettingValue.Visible = True
        Me.colConfigSettingValue.VisibleIndex = 1
        Me.colConfigSettingValue.Width = 381
        '
        'rbeTextEdit
        '
        Me.rbeTextEdit.AutoHeight = False
        Me.rbeTextEdit.Name = "rbeTextEdit"
        '
        'colConfigSettingID
        '
        Me.colConfigSettingID.Caption = "ID"
        Me.colConfigSettingID.FieldName = "ConfigSettingID"
        Me.colConfigSettingID.Name = "colConfigSettingID"
        Me.ExtendControlProperty.SetTextDisplay(Me.colConfigSettingID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colConfigSettingID.Width = 20
        '
        'rbeYesNoEdit
        '
        Me.rbeYesNoEdit.AutoHeight = False
        Me.rbeYesNoEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeYesNoEdit.Items.AddRange(New Object() {"Yes", "No"})
        Me.rbeYesNoEdit.Name = "rbeYesNoEdit"
        Me.rbeYesNoEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeISItemIDEdit
        '
        Me.rbeISItemIDEdit.AutoHeight = False
        Me.rbeISItemIDEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeISItemIDEdit.Items.AddRange(New Object() {"ItemCode", "ItemName"})
        Me.rbeISItemIDEdit.Name = "rbeISItemIDEdit"
        Me.rbeISItemIDEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeCustBizTypeEdit
        '
        Me.rbeCustBizTypeEdit.AutoHeight = False
        Me.rbeCustBizTypeEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeCustBizTypeEdit.Name = "rbeCustBizTypeEdit"
        Me.rbeCustBizTypeEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeISHypLinkEdit
        '
        Me.rbeISHypLinkEdit.AdditionalFilter = Nothing
        Me.rbeISHypLinkEdit.AllowEdit = True
        Me.rbeISHypLinkEdit.AutoHeight = False
        Me.rbeISHypLinkEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeISHypLinkEdit.ColumnDescriptions = Nothing
        Me.rbeISHypLinkEdit.ColumnNames = Nothing
        Me.rbeISHypLinkEdit.DataSource = Nothing
        Me.rbeISHypLinkEdit.DataSourceColumns = Nothing
        Me.rbeISHypLinkEdit.DefaultSort = Nothing
        Me.rbeISHypLinkEdit.DisplayField = Nothing
        Me.rbeISHypLinkEdit.IsMultiSelect = False
        Me.rbeISHypLinkEdit.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.rbeISHypLinkEdit.Name = "rbeISHypLinkEdit"
        Me.rbeISHypLinkEdit.TableName = Nothing
        Me.rbeISHypLinkEdit.TargetDisplayField = Nothing
        Me.rbeISHypLinkEdit.TargetValueMember = "ConfigSettingValue"
        Me.rbeISHypLinkEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.rbeISHypLinkEdit.UseCache = False
        Me.rbeISHypLinkEdit.UseSpecifiedColumns = False
        Me.rbeISHypLinkEdit.ValueMember = ""
        '
        'rbeShipModuleEdit
        '
        Me.rbeShipModuleEdit.AutoHeight = False
        Me.rbeShipModuleEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeShipModuleEdit.Items.AddRange(New Object() {"Interprise Suite basic", "KSI MultiShip"})
        Me.rbeShipModuleEdit.Name = "rbeShipModuleEdit"
        Me.rbeShipModuleEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeShopComSourceIDEdit
        '
        Me.rbeShopComSourceIDEdit.AutoHeight = False
        Me.rbeShopComSourceIDEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeShopComSourceIDEdit.Items.AddRange(New Object() {"IT_SKU", "IT_SOURCECODE"})
        Me.rbeShopComSourceIDEdit.Name = "rbeShopComSourceIDEdit"
        Me.rbeShopComSourceIDEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeShopComXMLDateEdit
        '
        Me.rbeShopComXMLDateEdit.AutoHeight = False
        Me.rbeShopComXMLDateEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeShopComXMLDateEdit.Items.AddRange(New Object() {"MM/DD/YYYY", "DD/MM/YYYY"})
        Me.rbeShopComXMLDateEdit.Name = "rbeShopComXMLDateEdit"
        Me.rbeShopComXMLDateEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeAmazonSiteEdit
        '
        Me.rbeAmazonSiteEdit.AutoHeight = False
        Me.rbeAmazonSiteEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeAmazonSiteEdit.Items.AddRange(New Object() {".com", ".co.uk", ".ca", ".de", ".fr", ".jp", ".com.cn"})
        Me.rbeAmazonSiteEdit.Name = "rbeAmazonSiteEdit"
        Me.rbeAmazonSiteEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeChanAdvNoPmtAction
        '
        Me.rbeChanAdvNoPmtAction.AutoHeight = False
        Me.rbeChanAdvNoPmtAction.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeChanAdvNoPmtAction.Items.AddRange(New Object() {"Ignore", "Import as Quote"})
        Me.rbeChanAdvNoPmtAction.Name = "rbeChanAdvNoPmtAction"
        Me.rbeChanAdvNoPmtAction.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeEbayCountryEdit
        '
        Me.rbeEbayCountryEdit.AutoHeight = False
        Me.rbeEbayCountryEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeEbayCountryEdit.Name = "rbeEbayCountryEdit"
        Me.rbeEbayCountryEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeEBayGenAuthToken
        '
        Me.rbeEBayGenAuthToken.AutoHeight = False
        Me.rbeEBayGenAuthToken.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.rbeEBayGenAuthToken.Name = "rbeEBayGenAuthToken"
        '
        'rbeOverrideSourcePrice
        '
        Me.rbeOverrideSourcePrice.AutoHeight = False
        Me.rbeOverrideSourcePrice.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeOverrideSourcePrice.Items.AddRange(New Object() {"", "Retail", "Wholesale"})
        Me.rbeOverrideSourcePrice.Name = "rbeOverrideSourcePrice"
        Me.rbeOverrideSourcePrice.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'TextEditSourcePwd
        '
        Me.TextEditSourcePwd.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ConfigSettingsSectionGateway, "LerrynImportExportConfig_DEV000221.SourcePassword_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.TextEditSourcePwd, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditSourcePwd.Location = New System.Drawing.Point(89, 59)
        Me.TextEditSourcePwd.MaximumSize = New System.Drawing.Size(200, 22)
        Me.TextEditSourcePwd.MinimumSize = New System.Drawing.Size(200, 22)
        Me.TextEditSourcePwd.Name = "TextEditSourcePwd"
        Me.TextEditSourcePwd.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextEditSourcePwd.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditSourcePwd.Properties.AutoHeight = False
        Me.TextEditSourcePwd.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextEditSourcePwd.Properties.ReadOnly = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditSourcePwd, System.Drawing.Color.Empty)
        Me.TextEditSourcePwd.Size = New System.Drawing.Size(200, 22)
        Me.TextEditSourcePwd.StyleController = Me.ConfigSettingsSectionExtendedLayout
        Me.TextEditSourcePwd.TabIndex = 3
        Me.TextEditSourcePwd.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditSourcePwd, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditSourceName
        '
        Me.TextEditSourceName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ConfigSettingsSectionGateway, "LerrynImportExportConfig_DEV000221.SourceName_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.TextEditSourceName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditSourceName.Location = New System.Drawing.Point(89, 31)
        Me.TextEditSourceName.MaximumSize = New System.Drawing.Size(200, 22)
        Me.TextEditSourceName.MinimumSize = New System.Drawing.Size(200, 22)
        Me.TextEditSourceName.Name = "TextEditSourceName"
        Me.TextEditSourceName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditSourceName.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditSourceName.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditSourceName, System.Drawing.Color.Empty)
        Me.TextEditSourceName.Size = New System.Drawing.Size(200, 22)
        Me.TextEditSourceName.StyleController = Me.ConfigSettingsSectionExtendedLayout
        Me.TextEditSourceName.TabIndex = 2
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditSourceName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditSourceCode
        '
        Me.TextEditSourceCode.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ConfigSettingsSectionGateway, "LerrynImportExportConfig_DEV000221.SourceCode_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.TextEditSourceCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditSourceCode.Location = New System.Drawing.Point(89, 3)
        Me.TextEditSourceCode.MaximumSize = New System.Drawing.Size(200, 22)
        Me.TextEditSourceCode.MinimumSize = New System.Drawing.Size(0, 22)
        Me.TextEditSourceCode.Name = "TextEditSourceCode"
        Me.TextEditSourceCode.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditSourceCode.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditSourceCode.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditSourceCode, System.Drawing.Color.Empty)
        Me.TextEditSourceCode.Size = New System.Drawing.Size(200, 22)
        Me.TextEditSourceCode.StyleController = Me.ConfigSettingsSectionExtendedLayout
        Me.TextEditSourceCode.TabIndex = 1
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditSourceCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'ConfigSettingsSectionLayoutGroup
        '
        Me.ConfigSettingsSectionLayoutGroup.CustomizationFormText = "ConfigSettingsSectionLayoutGroup"
        Me.ConfigSettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.ConfigSettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutItemSourceCode, Me.LayoutItemSourceName, Me.LayoutItemSourcePwd, Me.LayoutItemConfigSettings, Me.LayoutItemInputHandler, Me.LayouItemInputMode, Me.LayoutItemActivationCode, Me.LayoutControlItem1, Me.LayoutControlItem2})
        Me.ConfigSettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsSectionLayoutGroup.Name = "ConfigSettingsSectionLayoutGroup"
        Me.ConfigSettingsSectionLayoutGroup.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsSectionLayoutGroup.Text = "ConfigSettingsSectionLayoutGroup"
        Me.ConfigSettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutItemSourceCode
        '
        Me.LayoutItemSourceCode.Control = Me.TextEditSourceCode
        Me.LayoutItemSourceCode.CustomizationFormText = "Asset Group Name"
        Me.LayoutItemSourceCode.Location = New System.Drawing.Point(0, 0)
        Me.LayoutItemSourceCode.Name = "LayoutItemSourceCode"
        Me.LayoutItemSourceCode.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemSourceCode.Size = New System.Drawing.Size(691, 28)
        Me.LayoutItemSourceCode.Text = "Source Code"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemSourceCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemSourceCode.TextSize = New System.Drawing.Size(82, 13)
        '
        'LayoutItemSourceName
        '
        Me.LayoutItemSourceName.Control = Me.TextEditSourceName
        Me.LayoutItemSourceName.CustomizationFormText = "Asset Group Description"
        Me.LayoutItemSourceName.Location = New System.Drawing.Point(0, 28)
        Me.LayoutItemSourceName.Name = "LayoutItemSourceName"
        Me.LayoutItemSourceName.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemSourceName.Size = New System.Drawing.Size(691, 28)
        Me.LayoutItemSourceName.Text = "Source Name"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemSourceName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemSourceName.TextSize = New System.Drawing.Size(82, 13)
        '
        'LayoutItemSourcePwd
        '
        Me.LayoutItemSourcePwd.Control = Me.TextEditSourcePwd
        Me.LayoutItemSourcePwd.CustomizationFormText = "Source Password"
        Me.LayoutItemSourcePwd.Location = New System.Drawing.Point(0, 56)
        Me.LayoutItemSourcePwd.MaxSize = New System.Drawing.Size(0, 28)
        Me.LayoutItemSourcePwd.MinSize = New System.Drawing.Size(1, 28)
        Me.LayoutItemSourcePwd.Name = "LayoutItemSourcePwd"
        Me.LayoutItemSourcePwd.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemSourcePwd.Size = New System.Drawing.Size(297, 28)
        Me.LayoutItemSourcePwd.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutItemSourcePwd.Text = "Source Password"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemSourcePwd, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemSourcePwd.TextSize = New System.Drawing.Size(82, 13)
        '
        'LayoutItemConfigSettings
        '
        Me.LayoutItemConfigSettings.Control = Me.GridControlConfigSettings
        Me.LayoutItemConfigSettings.CustomizationFormText = "Config Settings"
        Me.LayoutItemConfigSettings.Location = New System.Drawing.Point(0, 140)
        Me.LayoutItemConfigSettings.Name = "LayoutItemConfigSettings"
        Me.LayoutItemConfigSettings.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemConfigSettings.Size = New System.Drawing.Size(691, 279)
        Me.LayoutItemConfigSettings.Text = "Config Settings"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemConfigSettings, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemConfigSettings.TextSize = New System.Drawing.Size(82, 13)
        '
        'LayoutItemInputHandler
        '
        Me.LayoutItemInputHandler.Control = Me.ImageComboBoxEditInputHandler
        Me.LayoutItemInputHandler.CustomizationFormText = "Input Handler"
        Me.LayoutItemInputHandler.Location = New System.Drawing.Point(0, 84)
        Me.LayoutItemInputHandler.MaxSize = New System.Drawing.Size(0, 28)
        Me.LayoutItemInputHandler.MinSize = New System.Drawing.Size(1, 28)
        Me.LayoutItemInputHandler.Name = "LayoutItemInputHandler"
        Me.LayoutItemInputHandler.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemInputHandler.Size = New System.Drawing.Size(343, 28)
        Me.LayoutItemInputHandler.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutItemInputHandler.Text = "Input Handler"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemInputHandler, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemInputHandler.TextSize = New System.Drawing.Size(82, 13)
        '
        'LayouItemInputMode
        '
        Me.LayouItemInputMode.Control = Me.TextEditInputMode
        Me.LayouItemInputMode.CustomizationFormText = "Input Mode"
        Me.LayouItemInputMode.Location = New System.Drawing.Point(343, 84)
        Me.LayouItemInputMode.MaxSize = New System.Drawing.Size(0, 28)
        Me.LayouItemInputMode.MinSize = New System.Drawing.Size(1, 28)
        Me.LayouItemInputMode.Name = "LayouItemInputMode"
        Me.LayouItemInputMode.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayouItemInputMode.Size = New System.Drawing.Size(348, 28)
        Me.LayouItemInputMode.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayouItemInputMode.Text = "Input Mode"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayouItemInputMode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayouItemInputMode.TextSize = New System.Drawing.Size(82, 13)
        '
        'LayoutItemActivationCode
        '
        Me.LayoutItemActivationCode.Control = Me.GroupControlActivationCode
        Me.LayoutItemActivationCode.CustomizationFormText = "Activation Code"
        Me.LayoutItemActivationCode.Location = New System.Drawing.Point(0, 112)
        Me.LayoutItemActivationCode.Name = "LayoutItemActivationCode"
        Me.LayoutItemActivationCode.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemActivationCode.Size = New System.Drawing.Size(691, 28)
        Me.LayoutItemActivationCode.Text = "Activation Code"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemActivationCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemActivationCode.TextSize = New System.Drawing.Size(82, 13)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.btnChangePwd
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(297, 56)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(66, 28)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.LabelFiller
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(363, 56)
        Me.LayoutControlItem2.MaxSize = New System.Drawing.Size(0, 28)
        Me.LayoutControlItem2.MinSize = New System.Drawing.Size(1, 28)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem2.Size = New System.Drawing.Size(328, 28)
        Me.LayoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'rbeAmazonMerchantIDEdit
        '
        Me.rbeAmazonMerchantIDEdit.AutoHeight = False
        Me.rbeAmazonMerchantIDEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeAmazonMerchantIDEdit.Name = "rbeAmazonMerchantIDEdit"
        Me.rbeAmazonMerchantIDEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'ConfigSettingsDetailSection
        '
        Me.Controls.Add(Me.ConfigSettingsSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "ConfigSettingsDetailSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(691, 419)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ConfigSettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.GroupControlActivationCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControlActivationCode.ResumeLayout(False)
        Me.GroupControlActivationCode.PerformLayout()
        CType(Me.TextEditInitialActivation1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditInputMode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageComboBoxEditInputHandler.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlConfigSettings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewConfigSettings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeISItemIDEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeCustBizTypeEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeISHypLinkEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeShipModuleEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeShopComSourceIDEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeShopComXMLDateEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeAmazonSiteEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeChanAdvNoPmtAction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeEbayCountryEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeEBayGenAuthToken, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeOverrideSourcePrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditSourcePwd.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditSourceName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditSourceCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemSourceCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemSourceName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemSourcePwd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemConfigSettings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemInputHandler, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayouItemInputMode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemActivationCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeAmazonMerchantIDEdit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents ConfigSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

#Region " Private Variables "
    Private m_ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents ConfigSettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents ConfigSettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents TextEditSourceCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutItemSourceCode As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents TextEditSourceName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutItemSourceName As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents TextEditInputMode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ImageComboBoxEditInputHandler As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents GridControlConfigSettings As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewConfigSettings As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents TextEditSourcePwd As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutItemSourcePwd As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutItemConfigSettings As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutItemInputHandler As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayouItemInputMode As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents colConfigGroup As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colConfigSettingName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colConfigSettingValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GroupControlActivationCode As DevExpress.XtraEditors.GroupControl
    Friend WithEvents LayoutItemActivationCode As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LabelInitialHyphen6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelInitialHyphen5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelInitialHyphen4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelInitialHyphen3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelInitialHyphen2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelInitialHyphen1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditInitialActivation1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelActivationStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnChangePwd As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LabelFiller As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControlItem2 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents rbeYesNoEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeISItemIDEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeCustBizTypeEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeISHypLinkEdit As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
    Friend WithEvents rbeShipModuleEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeShopComSourceIDEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeShopComXMLDateEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeAmazonSiteEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeTextEdit As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents rbeChanAdvNoPmtAction As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeEbayCountryEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeOverrideSourcePrice As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents colConfigSettingID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeEBayGenAuthToken As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents rbeAmazonMerchantIDEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
#End Region

#Region "ImportExportDataset"
    Public ReadOnly Property ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_importExportDataset
        End Get
    End Property
#End Region

End Class
#End Region
