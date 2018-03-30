'===============================================================================
' Interprise Suite SDK
' Copyright © 2009-2010 Interprise Software Solutions Incorporated
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

#Region " ImportWizardSectionContainer "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportWizardSectionContainer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImportWizardSectionContainer))
        Me.ImportWizardSectionContainerGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
        Me.ImportWizardPluginContainerControl = New Interprise.Presentation.Base.PluginContainerControl()
        Me.WizardControlImport = New Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl()
        Me.TabPageComplete = New DevExpress.XtraTab.XtraTabPage()
        Me.CheckEditViewImportLog = New DevExpress.XtraEditors.CheckEdit()
        Me.MemoEditImportLog = New DevExpress.XtraEditors.MemoEdit()
        Me.lblImportLog = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageWelcome = New DevExpress.XtraTab.XtraTabPage()
        Me.lblMultipleSiteOrAccount = New DevExpress.XtraEditors.LabelControl()
        Me.lblSiteOrAccount = New DevExpress.XtraEditors.LabelControl()
        Me.lblImportSource = New DevExpress.XtraEditors.LabelControl()
        Me.cbeSiteOrAccount = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.cbeImportSource = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.PictureBoxLerrynLogo = New System.Windows.Forms.PictureBox()
        Me.TabPageShared1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared()
        Me.TabPageSelectItems = New DevExpress.XtraTab.XtraTabPage()
        Me.pnlGetListError = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetListError = New DevExpress.XtraEditors.LabelControl()
        Me.lblGridColorKey3 = New DevExpress.XtraEditors.LabelControl()
        Me.PictureBoxGridColorKey3 = New System.Windows.Forms.PictureBox()
        Me.lblGridColorKey = New DevExpress.XtraEditors.LabelControl()
        Me.lblGridColorKey1 = New DevExpress.XtraEditors.LabelControl()
        Me.PictureBoxGridColorKey1 = New System.Windows.Forms.PictureBox()
        Me.lblGridColorKey2 = New DevExpress.XtraEditors.LabelControl()
        Me.PictureBoxGridColorKey2 = New System.Windows.Forms.PictureBox()
        Me.pnlGetListProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetListPleaseWait = New DevExpress.XtraEditors.LabelControl()
        Me.lblGetListProgress = New DevExpress.XtraEditors.LabelControl()
        Me.btnSelectAll = New DevExpress.XtraEditors.SimpleButton()
        Me.btnSelectNone = New DevExpress.XtraEditors.SimpleButton()
        Me.GridControlSelectItems = New DevExpress.XtraGrid.GridControl()
        Me.GridViewSelectItems = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colSelect = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colItemName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colItemSKU = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colISItemType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repISItemType = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.colSourceItemID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colImportAsKit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAlreadyImported = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSKUExists = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSKUError = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSKUChanged = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colItemCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repSourceItemType = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.TabPageOptions = New DevExpress.XtraTab.XtraTabPage()
        Me.chkImportMagentoSellingPriceAsSuggestedRetail = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoCostAsPricingCost = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoSellingPriceAsRetail = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoSellingPriceAsWholesale = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoLastCost = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoStandardCost = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoAverageCost = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoSpecialPriceAsRetail = New DevExpress.XtraEditors.CheckEdit()
        Me.chkImportMagentoSpecialPriceAsWholesale = New DevExpress.XtraEditors.CheckEdit()
        Me.TextEditQtyPublishingValue = New DevExpress.XtraEditors.TextEdit()
        Me.lblQtyPublishingValue = New DevExpress.XtraEditors.LabelControl()
        Me.lblQtyPublishing = New DevExpress.XtraEditors.LabelControl()
        Me.RadioGroupQtyPublishing = New DevExpress.XtraEditors.RadioGroup()
        Me.lblCategoriesExist = New DevExpress.XtraEditors.LabelControl()
        Me.lblCustomFields = New DevExpress.XtraEditors.LabelControl()
        Me.lblSourceOptions = New DevExpress.XtraEditors.LabelControl()
        Me.lblGeneralOptions = New DevExpress.XtraEditors.LabelControl()
        Me.cbeASPExtData2CustomField = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkImportASPStorefrontExtData2 = New DevExpress.XtraEditors.CheckEdit()
        Me.cbeASPExtData3CustomField = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkImportASPStorefrontExtData3 = New DevExpress.XtraEditors.CheckEdit()
        Me.cbeASPExtData5CustomField = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkImportASPStorefrontExtData5 = New DevExpress.XtraEditors.CheckEdit()
        Me.cbeASPExtData4CustomField = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkImportASPStorefrontExtData4 = New DevExpress.XtraEditors.CheckEdit()
        Me.cbeASPExtData1CustomField = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkImportASPStorefrontExtData1 = New DevExpress.XtraEditors.CheckEdit()
        Me.chkCreateCategories = New DevExpress.XtraEditors.CheckEdit()
        Me.TabPageCategoryMapping = New DevExpress.XtraTab.XtraTabPage()
        Me.lblMappingNotes = New DevExpress.XtraEditors.LabelControl()
        Me.lblSelectCatgoryMapping = New DevExpress.XtraEditors.LabelControl()
        Me.pnlGetCategoryProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetCategoryProgress = New DevExpress.XtraEditors.LabelControl()
        Me.TreeListCategories = New DevExpress.XtraTreeList.TreeList()
        Me.colTreeListCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListSourceCategoryID = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListISCategoryCode = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListISCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.rbeISCategoryCode = New Lerryn.Presentation.eShopCONNECT.Search.RepSearchTreeControl(Me.components)
        Me.imagelistCategory = New System.Windows.Forms.ImageList(Me.components)
        Me.TabPageImporting = New DevExpress.XtraTab.XtraTabPage()
        Me.pnlImportProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblImportPleaseWait = New DevExpress.XtraEditors.LabelControl()
        Me.lblImportProgress = New DevExpress.XtraEditors.LabelControl()
        Me.chkInhibitWarnings = New DevExpress.XtraEditors.CheckEdit()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImportWizardSectionContainerGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImportWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ImportWizardPluginContainerControl.SuspendLayout()
        CType(Me.WizardControlImport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardControlImport.SuspendLayout()
        Me.TabPageComplete.SuspendLayout()
        CType(Me.CheckEditViewImportLog.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditImportLog.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageWelcome.SuspendLayout()
        CType(Me.cbeSiteOrAccount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeImportSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxLerrynLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageSelectItems.SuspendLayout()
        CType(Me.pnlGetListError, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetListError.SuspendLayout()
        CType(Me.PictureBoxGridColorKey3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxGridColorKey1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxGridColorKey2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlGetListProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetListProgress.SuspendLayout()
        CType(Me.GridControlSelectItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewSelectItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repISItemType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repSourceItemType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageOptions.SuspendLayout()
        CType(Me.chkImportMagentoSellingPriceAsSuggestedRetail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoCostAsPricingCost.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoSellingPriceAsRetail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoSellingPriceAsWholesale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoLastCost.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoStandardCost.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoAverageCost.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoSpecialPriceAsRetail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportMagentoSpecialPriceAsWholesale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeASPExtData2CustomField.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportASPStorefrontExtData2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeASPExtData3CustomField.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportASPStorefrontExtData3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeASPExtData5CustomField.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportASPStorefrontExtData5.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeASPExtData4CustomField.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportASPStorefrontExtData4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeASPExtData1CustomField.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportASPStorefrontExtData1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkCreateCategories.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageCategoryMapping.SuspendLayout()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetCategoryProgress.SuspendLayout()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeISCategoryCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageImporting.SuspendLayout()
        CType(Me.pnlImportProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlImportProgress.SuspendLayout()
        CType(Me.chkInhibitWarnings.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'ImportWizardSectionContainerGateway
        '
        Me.ImportWizardSectionContainerGateway.DataSetName = "ImportWizardSectionContainerDataset"
        Me.ImportWizardSectionContainerGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'ImportWizardPluginContainerControl
        '
        Me.ImportWizardPluginContainerControl.AppearanceCaption.Options.UseTextOptions = True
        Me.ImportWizardPluginContainerControl.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ImportWizardPluginContainerControl.BaseLayoutControl = Nothing
        Me.ImportWizardPluginContainerControl.ContextMenuButtonCaption = Nothing
        Me.ImportWizardPluginContainerControl.Controls.Add(Me.WizardControlImport)
        Me.ImportWizardPluginContainerControl.CurrentControl = Nothing
        Me.ImportWizardPluginContainerControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImportWizardPluginContainerControl.EditorsHeight = 0
        Me.ImportWizardPluginContainerControl.GroupContextMenu = Nothing
        Me.ImportWizardPluginContainerControl.HelpCode = Nothing
        Me.ImportWizardPluginContainerControl.IsCustomTab = False
        Me.ImportWizardPluginContainerControl.LayoutMode = False
        Me.ImportWizardPluginContainerControl.Location = New System.Drawing.Point(0, 0)
        Me.ImportWizardPluginContainerControl.Name = "ImportWizardPluginContainerControl"
        Me.ImportWizardPluginContainerControl.OverrideUserRoleMode = False
        Me.ImportWizardPluginContainerControl.PluginManagerButton = Nothing
        Me.ImportWizardPluginContainerControl.PluginRows = Nothing
        Me.ImportWizardPluginContainerControl.SearchPluginButton = Nothing
        Me.ImportWizardPluginContainerControl.ShowCaption = False
        Me.ImportWizardPluginContainerControl.Size = New System.Drawing.Size(861, 537)
        Me.ImportWizardPluginContainerControl.TabIndex = 0
        '
        'WizardControlImport
        '
        Me.WizardControlImport.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.WizardControlImport.Appearance.Options.UseBackColor = True
        Me.WizardControlImport.AppearancePage.Header.Options.UseTextOptions = True
        Me.WizardControlImport.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WizardControlImport.AppearancePage.PageClient.BackColor = System.Drawing.SystemColors.Control
        Me.WizardControlImport.AppearancePage.PageClient.Options.UseBackColor = True
        Me.WizardControlImport.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.WizardControlImport.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.WizardControlImport.DisplayFinishButton = False
        Me.WizardControlImport.DisplayNextButton = True
        Me.WizardControlImport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WizardControlImport.FinishMessage = "Finish message set dynamically"
        Me.WizardControlImport.FinishPage = Me.TabPageComplete
        Me.WizardControlImport.IsCustom = False
        Me.WizardControlImport.IsPlugIn = True
        Me.WizardControlImport.Location = New System.Drawing.Point(2, 2)
        Me.WizardControlImport.Name = "WizardControlImport"
        Me.WizardControlImport.NextButtonText = "&Next"
        Me.WizardControlImport.PageDescription = Nothing
        Me.WizardControlImport.SelectedTabPage = Me.TabPageWelcome
        Me.WizardControlImport.SharedPage = Me.TabPageShared1
        Me.WizardControlImport.Size = New System.Drawing.Size(857, 533)
        Me.WizardControlImport.TabIndex = 0
        Me.WizardControlImport.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.TabPageWelcome, Me.TabPageSelectItems, Me.TabPageOptions, Me.TabPageCategoryMapping, Me.TabPageImporting, Me.TabPageComplete})
        Me.WizardControlImport.Title = "the Inventory Import"
        Me.WizardControlImport.WelcomeMessage = "Welcome message set dynamically"
        Me.WizardControlImport.WelcomePage = Me.TabPageWelcome
        '
        'TabPageComplete
        '
        Me.TabPageComplete.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageComplete.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageComplete.Controls.Add(Me.CheckEditViewImportLog)
        Me.TabPageComplete.Controls.Add(Me.MemoEditImportLog)
        Me.TabPageComplete.Controls.Add(Me.lblImportLog)
        Me.TabPageComplete.Name = "TabPageComplete"
        Me.TabPageComplete.Size = New System.Drawing.Size(851, 475)
        Me.TabPageComplete.Text = "TabPageComplete"
        '
        'CheckEditViewImportLog
        '
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditViewImportLog, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckEditViewImportLog.Location = New System.Drawing.Point(179, 243)
        Me.CheckEditViewImportLog.Name = "CheckEditViewImportLog"
        Me.CheckEditViewImportLog.Properties.AutoHeight = False
        Me.CheckEditViewImportLog.Properties.Caption = "View Import Log"
        Me.CheckEditViewImportLog.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.CheckEditViewImportLog, System.Drawing.Color.Empty)
        Me.CheckEditViewImportLog.Size = New System.Drawing.Size(112, 22)
        Me.CheckEditViewImportLog.TabIndex = 3
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditViewImportLog, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'MemoEditImportLog
        '
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditImportLog, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditImportLog.Location = New System.Drawing.Point(179, 290)
        Me.MemoEditImportLog.Name = "MemoEditImportLog"
        Me.MemoEditImportLog.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditImportLog.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditImportLog, System.Drawing.Color.Empty)
        Me.MemoEditImportLog.Size = New System.Drawing.Size(604, 176)
        Me.MemoEditImportLog.TabIndex = 1
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditImportLog, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditImportLog.UseOptimizedRendering = True
        Me.MemoEditImportLog.Visible = False
        '
        'lblImportLog
        '
        Me.lblImportLog.Location = New System.Drawing.Point(179, 271)
        Me.lblImportLog.Name = "lblImportLog"
        Me.lblImportLog.Size = New System.Drawing.Size(52, 13)
        Me.lblImportLog.TabIndex = 2
        Me.lblImportLog.Text = "Import Log"
        Me.lblImportLog.Visible = False
        '
        'TabPageWelcome
        '
        Me.TabPageWelcome.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageWelcome.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageWelcome.Controls.Add(Me.lblMultipleSiteOrAccount)
        Me.TabPageWelcome.Controls.Add(Me.lblSiteOrAccount)
        Me.TabPageWelcome.Controls.Add(Me.lblImportSource)
        Me.TabPageWelcome.Controls.Add(Me.cbeSiteOrAccount)
        Me.TabPageWelcome.Controls.Add(Me.cbeImportSource)
        Me.TabPageWelcome.Controls.Add(Me.PictureBoxLerrynLogo)
        Me.TabPageWelcome.Name = "TabPageWelcome"
        Me.TabPageWelcome.Size = New System.Drawing.Size(851, 475)
        Me.TabPageWelcome.Text = "TabPageWelcome"
        '
        'lblMultipleSiteOrAccount
        '
        Me.lblMultipleSiteOrAccount.Location = New System.Drawing.Point(252, 241)
        Me.lblMultipleSiteOrAccount.Name = "lblMultipleSiteOrAccount"
        Me.lblMultipleSiteOrAccount.Size = New System.Drawing.Size(357, 13)
        Me.lblMultipleSiteOrAccount.TabIndex = 38
        Me.lblMultipleSiteOrAccount.Text = "This Source has multiple Instances/Accounts - Please select the one to use"
        Me.lblMultipleSiteOrAccount.Visible = False
        '
        'lblSiteOrAccount
        '
        Me.lblSiteOrAccount.Location = New System.Drawing.Point(252, 265)
        Me.lblSiteOrAccount.Name = "lblSiteOrAccount"
        Me.lblSiteOrAccount.Size = New System.Drawing.Size(113, 13)
        Me.lblSiteOrAccount.TabIndex = 37
        Me.lblSiteOrAccount.Text = "Instance to import from"
        Me.lblSiteOrAccount.Visible = False
        '
        'lblImportSource
        '
        Me.lblImportSource.Location = New System.Drawing.Point(180, 207)
        Me.lblImportSource.Name = "lblImportSource"
        Me.lblImportSource.Size = New System.Drawing.Size(185, 13)
        Me.lblImportSource.TabIndex = 36
        Me.lblImportSource.Text = "Source to import Inventory Items from"
        '
        'cbeSiteOrAccount
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeSiteOrAccount, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSiteOrAccount.Location = New System.Drawing.Point(371, 261)
        Me.cbeSiteOrAccount.Name = "cbeSiteOrAccount"
        Me.cbeSiteOrAccount.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeSiteOrAccount.Properties.Appearance.Options.UseBackColor = True
        Me.cbeSiteOrAccount.Properties.AutoHeight = False
        Me.cbeSiteOrAccount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeSiteOrAccount, System.Drawing.Color.Empty)
        Me.cbeSiteOrAccount.Size = New System.Drawing.Size(234, 22)
        Me.cbeSiteOrAccount.TabIndex = 35
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSiteOrAccount, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSiteOrAccount.Visible = False
        '
        'cbeImportSource
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeImportSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeImportSource.Location = New System.Drawing.Point(371, 203)
        Me.cbeImportSource.Name = "cbeImportSource"
        Me.cbeImportSource.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeImportSource.Properties.Appearance.Options.UseBackColor = True
        Me.cbeImportSource.Properties.AutoHeight = False
        Me.cbeImportSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeImportSource, System.Drawing.Color.Empty)
        Me.cbeImportSource.Size = New System.Drawing.Size(234, 22)
        Me.cbeImportSource.TabIndex = 34
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeImportSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'PictureBoxLerrynLogo
        '
        Me.PictureBoxLerrynLogo.Image = CType(resources.GetObject("PictureBoxLerrynLogo.Image"), System.Drawing.Image)
        Me.PictureBoxLerrynLogo.Location = New System.Drawing.Point(633, 386)
        Me.PictureBoxLerrynLogo.Name = "PictureBoxLerrynLogo"
        Me.PictureBoxLerrynLogo.Size = New System.Drawing.Size(164, 54)
        Me.PictureBoxLerrynLogo.TabIndex = 33
        Me.PictureBoxLerrynLogo.TabStop = False
        Me.PictureBoxLerrynLogo.Visible = False
        '
        'TabPageShared1
        '
        Me.TabPageShared1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabPageShared1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TabPageShared1.Location = New System.Drawing.Point(1, 1)
        Me.TabPageShared1.Name = "TabPageShared1"
        Me.TabPageShared1.Size = New System.Drawing.Size(851, 475)
        Me.TabPageShared1.TabIndex = 5
        '
        'TabPageSelectItems
        '
        Me.TabPageSelectItems.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageSelectItems.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageSelectItems.Controls.Add(Me.pnlGetListError)
        Me.TabPageSelectItems.Controls.Add(Me.lblGridColorKey3)
        Me.TabPageSelectItems.Controls.Add(Me.PictureBoxGridColorKey3)
        Me.TabPageSelectItems.Controls.Add(Me.lblGridColorKey)
        Me.TabPageSelectItems.Controls.Add(Me.lblGridColorKey1)
        Me.TabPageSelectItems.Controls.Add(Me.PictureBoxGridColorKey1)
        Me.TabPageSelectItems.Controls.Add(Me.lblGridColorKey2)
        Me.TabPageSelectItems.Controls.Add(Me.PictureBoxGridColorKey2)
        Me.TabPageSelectItems.Controls.Add(Me.pnlGetListProgress)
        Me.TabPageSelectItems.Controls.Add(Me.btnSelectAll)
        Me.TabPageSelectItems.Controls.Add(Me.btnSelectNone)
        Me.TabPageSelectItems.Controls.Add(Me.GridControlSelectItems)
        Me.TabPageSelectItems.Name = "TabPageSelectItems"
        Me.TabPageSelectItems.Size = New System.Drawing.Size(851, 475)
        Me.TabPageSelectItems.Text = "Select Products to Import"
        '
        'pnlGetListError
        '
        Me.pnlGetListError.Controls.Add(Me.lblGetListError)
        Me.pnlGetListError.Location = New System.Drawing.Point(44, 152)
        Me.pnlGetListError.Name = "pnlGetListError"
        Me.pnlGetListError.Size = New System.Drawing.Size(711, 198)
        Me.pnlGetListError.TabIndex = 17
        Me.pnlGetListError.Visible = False
        '
        'lblGetListError
        '
        Me.lblGetListError.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblGetListError.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.lblGetListError.Location = New System.Drawing.Point(3, 3)
        Me.lblGetListError.MinimumSize = New System.Drawing.Size(705, 192)
        Me.lblGetListError.Name = "lblGetListError"
        Me.lblGetListError.Size = New System.Drawing.Size(705, 192)
        Me.lblGetListError.TabIndex = 1
        Me.lblGetListError.Text = "Error Details" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Here" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblGridColorKey3
        '
        Me.lblGridColorKey3.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.lblGridColorKey3.Location = New System.Drawing.Point(444, 15)
        Me.lblGridColorKey3.Name = "lblGridColorKey3"
        Me.lblGridColorKey3.Size = New System.Drawing.Size(318, 13)
        Me.lblGridColorKey3.TabIndex = 16
        Me.lblGridColorKey3.Text = "= SKU/Source ID has changed - importing will update existing Item"
        '
        'PictureBoxGridColorKey3
        '
        Me.PictureBoxGridColorKey3.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.InvImpSKUChanged
        Me.PictureBoxGridColorKey3.InitialImage = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.InvImpSKUChanged
        Me.PictureBoxGridColorKey3.Location = New System.Drawing.Point(421, 12)
        Me.PictureBoxGridColorKey3.Name = "PictureBoxGridColorKey3"
        Me.PictureBoxGridColorKey3.Size = New System.Drawing.Size(17, 18)
        Me.PictureBoxGridColorKey3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxGridColorKey3.TabIndex = 15
        Me.PictureBoxGridColorKey3.TabStop = False
        '
        'lblGridColorKey
        '
        Me.lblGridColorKey.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.lblGridColorKey.Location = New System.Drawing.Point(231, 14)
        Me.lblGridColorKey.Name = "lblGridColorKey"
        Me.lblGridColorKey.Size = New System.Drawing.Size(86, 13)
        Me.lblGridColorKey.TabIndex = 10
        Me.lblGridColorKey.Text = "Grid row color key"
        '
        'lblGridColorKey1
        '
        Me.lblGridColorKey1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.lblGridColorKey1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGridColorKey1.Location = New System.Drawing.Point(161, 39)
        Me.lblGridColorKey1.Name = "lblGridColorKey1"
        Me.lblGridColorKey1.Size = New System.Drawing.Size(250, 13)
        Me.lblGridColorKey1.TabIndex = 12
        Me.lblGridColorKey1.Text = "= Item already imported - importing will update Item"
        '
        'PictureBoxGridColorKey1
        '
        Me.PictureBoxGridColorKey1.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.InvImpDone
        Me.PictureBoxGridColorKey1.InitialImage = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.InvImpDone
        Me.PictureBoxGridColorKey1.Location = New System.Drawing.Point(138, 36)
        Me.PictureBoxGridColorKey1.Name = "PictureBoxGridColorKey1"
        Me.PictureBoxGridColorKey1.Size = New System.Drawing.Size(17, 18)
        Me.PictureBoxGridColorKey1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxGridColorKey1.TabIndex = 11
        Me.PictureBoxGridColorKey1.TabStop = False
        '
        'lblGridColorKey2
        '
        Me.lblGridColorKey2.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.lblGridColorKey2.Location = New System.Drawing.Point(444, 39)
        Me.lblGridColorKey2.Name = "lblGridColorKey2"
        Me.lblGridColorKey2.Size = New System.Drawing.Size(309, 13)
        Me.lblGridColorKey2.TabIndex = 14
        Me.lblGridColorKey2.Text = "= Item with this SKU exists - importing will add Source properties"
        '
        'PictureBoxGridColorKey2
        '
        Me.PictureBoxGridColorKey2.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.InvImpSKUExists
        Me.PictureBoxGridColorKey2.InitialImage = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.InvImpSKUExists
        Me.PictureBoxGridColorKey2.Location = New System.Drawing.Point(421, 36)
        Me.PictureBoxGridColorKey2.Name = "PictureBoxGridColorKey2"
        Me.PictureBoxGridColorKey2.Size = New System.Drawing.Size(17, 18)
        Me.PictureBoxGridColorKey2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxGridColorKey2.TabIndex = 13
        Me.PictureBoxGridColorKey2.TabStop = False
        '
        'pnlGetListProgress
        '
        Me.pnlGetListProgress.Controls.Add(Me.lblGetListPleaseWait)
        Me.pnlGetListProgress.Controls.Add(Me.lblGetListProgress)
        Me.pnlGetListProgress.Location = New System.Drawing.Point(284, 188)
        Me.pnlGetListProgress.Name = "pnlGetListProgress"
        Me.pnlGetListProgress.Size = New System.Drawing.Size(240, 100)
        Me.pnlGetListProgress.TabIndex = 10
        Me.pnlGetListProgress.Visible = False
        '
        'lblGetListPleaseWait
        '
        Me.lblGetListPleaseWait.Location = New System.Drawing.Point(93, 53)
        Me.lblGetListPleaseWait.Name = "lblGetListPleaseWait"
        Me.lblGetListPleaseWait.Size = New System.Drawing.Size(54, 13)
        Me.lblGetListPleaseWait.TabIndex = 1
        Me.lblGetListPleaseWait.Text = "Please wait"
        '
        'lblGetListProgress
        '
        Me.lblGetListProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblGetListProgress.Location = New System.Drawing.Point(3, 33)
        Me.lblGetListProgress.MinimumSize = New System.Drawing.Size(234, 0)
        Me.lblGetListProgress.Name = "lblGetListProgress"
        Me.lblGetListProgress.Size = New System.Drawing.Size(234, 13)
        Me.lblGetListProgress.TabIndex = 0
        Me.lblGetListProgress.Text = "Getting Product list from"
        '
        'btnSelectAll
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnSelectAll, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnSelectAll.Image = CType(resources.GetObject("btnSelectAll.Image"), System.Drawing.Image)
        Me.btnSelectAll.Location = New System.Drawing.Point(21, 36)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnSelectAll, System.Drawing.Color.Empty)
        Me.btnSelectAll.Size = New System.Drawing.Size(20, 20)
        Me.btnSelectAll.TabIndex = 8
        Me.ExtendControlProperty.SetTextDisplay(Me.btnSelectAll, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnSelectAll.ToolTip = "Select All"
        '
        'btnSelectNone
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnSelectNone, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnSelectNone.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.SelectNone
        Me.btnSelectNone.Location = New System.Drawing.Point(47, 36)
        Me.btnSelectNone.Name = "btnSelectNone"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnSelectNone, System.Drawing.Color.Empty)
        Me.btnSelectNone.Size = New System.Drawing.Size(20, 20)
        Me.btnSelectNone.TabIndex = 9
        Me.ExtendControlProperty.SetTextDisplay(Me.btnSelectNone, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnSelectNone.ToolTip = "Select None"
        '
        'GridControlSelectItems
        '
        Me.GridControlSelectItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlSelectItems, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlSelectItems, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSelectItems.Location = New System.Drawing.Point(0, 0)
        Me.GridControlSelectItems.MainView = Me.GridViewSelectItems
        Me.GridControlSelectItems.Name = "GridControlSelectItems"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlSelectItems, System.Drawing.Color.Empty)
        Me.GridControlSelectItems.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repSourceItemType, Me.repISItemType})
        Me.GridControlSelectItems.Size = New System.Drawing.Size(851, 475)
        Me.GridControlSelectItems.TabIndex = 1
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlSelectItems, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSelectItems.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewSelectItems})
        '
        'GridViewSelectItems
        '
        Me.GridViewSelectItems.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colSelect, Me.colItemName, Me.colItemSKU, Me.colSourceType, Me.colISItemType, Me.colSourceItemID, Me.colImportAsKit, Me.colAlreadyImported, Me.colSKUExists, Me.colSKUError, Me.colSKUChanged, Me.colItemCode})
        Me.GridViewSelectItems.GridControl = Me.GridControlSelectItems
        Me.GridViewSelectItems.Name = "GridViewSelectItems"
        Me.GridViewSelectItems.OptionsNavigation.EnterMoveNextColumn = True
        Me.GridViewSelectItems.OptionsNavigation.UseTabKey = False
        Me.GridViewSelectItems.OptionsView.ShowGroupPanel = False
        '
        'colSelect
        '
        Me.colSelect.Caption = "Import"
        Me.colSelect.FieldName = "Import"
        Me.colSelect.Name = "colSelect"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSelect, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSelect.Visible = True
        Me.colSelect.VisibleIndex = 0
        Me.colSelect.Width = 50
        '
        'colItemName
        '
        Me.colItemName.Caption = "Source Item Name -> IS Item Description"
        Me.colItemName.FieldName = "ItemName"
        Me.colItemName.Name = "colItemName"
        Me.colItemName.OptionsColumn.AllowEdit = False
        Me.colItemName.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colItemName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colItemName.Visible = True
        Me.colItemName.VisibleIndex = 1
        Me.colItemName.Width = 270
        '
        'colItemSKU
        '
        Me.colItemSKU.Caption = "Source Item SKU -> IS Item Code"
        Me.colItemSKU.FieldName = "ItemSKU"
        Me.colItemSKU.Name = "colItemSKU"
        Me.colItemSKU.OptionsColumn.AllowEdit = False
        Me.colItemSKU.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colItemSKU, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colItemSKU.Visible = True
        Me.colItemSKU.VisibleIndex = 2
        Me.colItemSKU.Width = 220
        '
        'colSourceType
        '
        Me.colSourceType.Caption = "Source Item Type"
        Me.colSourceType.FieldName = "SourceType"
        Me.colSourceType.Name = "colSourceType"
        Me.colSourceType.OptionsColumn.AllowEdit = False
        Me.colSourceType.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceType.Visible = True
        Me.colSourceType.VisibleIndex = 3
        Me.colSourceType.Width = 130
        '
        'colISItemType
        '
        Me.colISItemType.Caption = "IS Item Type"
        Me.colISItemType.ColumnEdit = Me.repISItemType
        Me.colISItemType.FieldName = "ISItemType"
        Me.colISItemType.Name = "colISItemType"
        Me.ExtendControlProperty.SetTextDisplay(Me.colISItemType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colISItemType.Visible = True
        Me.colISItemType.VisibleIndex = 4
        '
        'repISItemType
        '
        Me.repISItemType.AutoHeight = False
        Me.repISItemType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repISItemType.Name = "repISItemType"
        '
        'colSourceItemID
        '
        Me.colSourceItemID.Caption = "Source Item ID"
        Me.colSourceItemID.FieldName = "SourceItemID"
        Me.colSourceItemID.Name = "colSourceItemID"
        Me.colSourceItemID.OptionsColumn.AllowEdit = False
        Me.colSourceItemID.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceItemID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceItemID.Width = 20
        '
        'colImportAsKit
        '
        Me.colImportAsKit.Caption = "Import as Kit"
        Me.colImportAsKit.FieldName = "ImportAsKit"
        Me.colImportAsKit.Name = "colImportAsKit"
        Me.ExtendControlProperty.SetTextDisplay(Me.colImportAsKit, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colImportAsKit.Width = 20
        '
        'colAlreadyImported
        '
        Me.colAlreadyImported.Caption = "Already Imported"
        Me.colAlreadyImported.FieldName = "AlreadyImported"
        Me.colAlreadyImported.Name = "colAlreadyImported"
        Me.colAlreadyImported.OptionsColumn.AllowEdit = False
        Me.colAlreadyImported.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAlreadyImported, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAlreadyImported.Width = 20
        '
        'colSKUExists
        '
        Me.colSKUExists.Caption = "SKU Exists"
        Me.colSKUExists.FieldName = "SKUExists"
        Me.colSKUExists.Name = "colSKUExists"
        Me.colSKUExists.OptionsColumn.AllowEdit = False
        Me.colSKUExists.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSKUExists, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSKUExists.Width = 20
        '
        'colSKUError
        '
        Me.colSKUError.Caption = "SKU Error"
        Me.colSKUError.FieldName = "SKUError"
        Me.colSKUError.Name = "colSKUError"
        Me.colSKUError.OptionsColumn.AllowEdit = False
        Me.colSKUError.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSKUError, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSKUError.Width = 20
        '
        'colSKUChanged
        '
        Me.colSKUChanged.Caption = "SKU Changed"
        Me.colSKUChanged.FieldName = "SKUChanged"
        Me.colSKUChanged.Name = "colSKUChanged"
        Me.colSKUChanged.OptionsColumn.AllowEdit = False
        Me.colSKUChanged.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSKUChanged, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSKUChanged.Width = 20
        '
        'colItemCode
        '
        Me.colItemCode.Caption = "Item Code"
        Me.colItemCode.FieldName = "ItemCode"
        Me.colItemCode.Name = "colItemCode"
        Me.colItemCode.OptionsColumn.AllowEdit = False
        Me.colItemCode.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colItemCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colItemCode.Width = 20
        '
        'repSourceItemType
        '
        Me.repSourceItemType.AutoHeight = False
        Me.repSourceItemType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repSourceItemType.Name = "repSourceItemType"
        Me.repSourceItemType.ReadOnly = True
        '
        'TabPageOptions
        '
        Me.TabPageOptions.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageOptions.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoSellingPriceAsSuggestedRetail)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoCostAsPricingCost)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoSellingPriceAsRetail)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoSellingPriceAsWholesale)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoLastCost)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoStandardCost)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoAverageCost)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoSpecialPriceAsRetail)
        Me.TabPageOptions.Controls.Add(Me.chkImportMagentoSpecialPriceAsWholesale)
        Me.TabPageOptions.Controls.Add(Me.TextEditQtyPublishingValue)
        Me.TabPageOptions.Controls.Add(Me.lblQtyPublishingValue)
        Me.TabPageOptions.Controls.Add(Me.lblQtyPublishing)
        Me.TabPageOptions.Controls.Add(Me.RadioGroupQtyPublishing)
        Me.TabPageOptions.Controls.Add(Me.lblCategoriesExist)
        Me.TabPageOptions.Controls.Add(Me.lblCustomFields)
        Me.TabPageOptions.Controls.Add(Me.lblSourceOptions)
        Me.TabPageOptions.Controls.Add(Me.lblGeneralOptions)
        Me.TabPageOptions.Controls.Add(Me.cbeASPExtData2CustomField)
        Me.TabPageOptions.Controls.Add(Me.chkImportASPStorefrontExtData2)
        Me.TabPageOptions.Controls.Add(Me.cbeASPExtData3CustomField)
        Me.TabPageOptions.Controls.Add(Me.chkImportASPStorefrontExtData3)
        Me.TabPageOptions.Controls.Add(Me.cbeASPExtData5CustomField)
        Me.TabPageOptions.Controls.Add(Me.chkImportASPStorefrontExtData5)
        Me.TabPageOptions.Controls.Add(Me.cbeASPExtData4CustomField)
        Me.TabPageOptions.Controls.Add(Me.chkImportASPStorefrontExtData4)
        Me.TabPageOptions.Controls.Add(Me.cbeASPExtData1CustomField)
        Me.TabPageOptions.Controls.Add(Me.chkImportASPStorefrontExtData1)
        Me.TabPageOptions.Controls.Add(Me.chkCreateCategories)
        Me.TabPageOptions.Name = "TabPageOptions"
        Me.TabPageOptions.Size = New System.Drawing.Size(851, 475)
        Me.TabPageOptions.Text = "Import Options"
        '
        'chkImportMagentoSellingPriceAsSuggestedRetail
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoSellingPriceAsSuggestedRetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoSellingPriceAsSuggestedRetail.Location = New System.Drawing.Point(27, 244)
        Me.chkImportMagentoSellingPriceAsSuggestedRetail.Name = "chkImportMagentoSellingPriceAsSuggestedRetail"
        Me.chkImportMagentoSellingPriceAsSuggestedRetail.Properties.AutoHeight = False
        Me.chkImportMagentoSellingPriceAsSuggestedRetail.Properties.Caption = "Import Magento Selling Price as Inventory Item Suggested Retail price"
        Me.chkImportMagentoSellingPriceAsSuggestedRetail.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoSellingPriceAsSuggestedRetail, System.Drawing.Color.Empty)
        Me.chkImportMagentoSellingPriceAsSuggestedRetail.Size = New System.Drawing.Size(334, 22)
        Me.chkImportMagentoSellingPriceAsSuggestedRetail.TabIndex = 109
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoSellingPriceAsSuggestedRetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoCostAsPricingCost
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoCostAsPricingCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoCostAsPricingCost.Location = New System.Drawing.Point(27, 340)
        Me.chkImportMagentoCostAsPricingCost.Name = "chkImportMagentoCostAsPricingCost"
        Me.chkImportMagentoCostAsPricingCost.Properties.AutoHeight = False
        Me.chkImportMagentoCostAsPricingCost.Properties.Caption = "Import Magento Cost as Inventory Item PricingCost"
        Me.chkImportMagentoCostAsPricingCost.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoCostAsPricingCost, System.Drawing.Color.Empty)
        Me.chkImportMagentoCostAsPricingCost.Size = New System.Drawing.Size(293, 22)
        Me.chkImportMagentoCostAsPricingCost.TabIndex = 108
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoCostAsPricingCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoSellingPriceAsRetail
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoSellingPriceAsRetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoSellingPriceAsRetail.Location = New System.Drawing.Point(27, 276)
        Me.chkImportMagentoSellingPriceAsRetail.Name = "chkImportMagentoSellingPriceAsRetail"
        Me.chkImportMagentoSellingPriceAsRetail.Properties.AutoHeight = False
        Me.chkImportMagentoSellingPriceAsRetail.Properties.Caption = "Import Magento Selling Price as Inventory Item Retail price"
        Me.chkImportMagentoSellingPriceAsRetail.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoSellingPriceAsRetail, System.Drawing.Color.Empty)
        Me.chkImportMagentoSellingPriceAsRetail.Size = New System.Drawing.Size(314, 22)
        Me.chkImportMagentoSellingPriceAsRetail.TabIndex = 107
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoSellingPriceAsRetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoSellingPriceAsWholesale
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoSellingPriceAsWholesale, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoSellingPriceAsWholesale.Location = New System.Drawing.Point(27, 308)
        Me.chkImportMagentoSellingPriceAsWholesale.Name = "chkImportMagentoSellingPriceAsWholesale"
        Me.chkImportMagentoSellingPriceAsWholesale.Properties.AutoHeight = False
        Me.chkImportMagentoSellingPriceAsWholesale.Properties.Caption = "Import Magento Selling Price as Inventory Item Wholesale price"
        Me.chkImportMagentoSellingPriceAsWholesale.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoSellingPriceAsWholesale, System.Drawing.Color.Empty)
        Me.chkImportMagentoSellingPriceAsWholesale.Size = New System.Drawing.Size(334, 22)
        Me.chkImportMagentoSellingPriceAsWholesale.TabIndex = 106
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoSellingPriceAsWholesale, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoLastCost
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoLastCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoLastCost.Location = New System.Drawing.Point(27, 436)
        Me.chkImportMagentoLastCost.Name = "chkImportMagentoLastCost"
        Me.chkImportMagentoLastCost.Properties.AutoHeight = False
        Me.chkImportMagentoLastCost.Properties.Caption = "Import Magento Cost as Inventory Item Last Cost"
        Me.chkImportMagentoLastCost.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoLastCost, System.Drawing.Color.Empty)
        Me.chkImportMagentoLastCost.Size = New System.Drawing.Size(272, 22)
        Me.chkImportMagentoLastCost.TabIndex = 105
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoLastCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoStandardCost
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoStandardCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoStandardCost.Location = New System.Drawing.Point(27, 404)
        Me.chkImportMagentoStandardCost.Name = "chkImportMagentoStandardCost"
        Me.chkImportMagentoStandardCost.Properties.AutoHeight = False
        Me.chkImportMagentoStandardCost.Properties.Caption = "Import Magento Cost as Inventory Item Standard Cost"
        Me.chkImportMagentoStandardCost.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoStandardCost, System.Drawing.Color.Empty)
        Me.chkImportMagentoStandardCost.Size = New System.Drawing.Size(293, 22)
        Me.chkImportMagentoStandardCost.TabIndex = 104
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoStandardCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoAverageCost
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoAverageCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoAverageCost.Location = New System.Drawing.Point(27, 372)
        Me.chkImportMagentoAverageCost.Name = "chkImportMagentoAverageCost"
        Me.chkImportMagentoAverageCost.Properties.AutoHeight = False
        Me.chkImportMagentoAverageCost.Properties.Caption = "Import Magento Cost as Inventory Item Average Cost"
        Me.chkImportMagentoAverageCost.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoAverageCost, System.Drawing.Color.Empty)
        Me.chkImportMagentoAverageCost.Size = New System.Drawing.Size(293, 22)
        Me.chkImportMagentoAverageCost.TabIndex = 103
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoAverageCost, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoSpecialPriceAsRetail
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoSpecialPriceAsRetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoSpecialPriceAsRetail.Location = New System.Drawing.Point(384, 276)
        Me.chkImportMagentoSpecialPriceAsRetail.Name = "chkImportMagentoSpecialPriceAsRetail"
        Me.chkImportMagentoSpecialPriceAsRetail.Properties.AutoHeight = False
        Me.chkImportMagentoSpecialPriceAsRetail.Properties.Caption = "Import Magento Special Price as Inventory Item Retail price"
        Me.chkImportMagentoSpecialPriceAsRetail.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoSpecialPriceAsRetail, System.Drawing.Color.Empty)
        Me.chkImportMagentoSpecialPriceAsRetail.Size = New System.Drawing.Size(314, 22)
        Me.chkImportMagentoSpecialPriceAsRetail.TabIndex = 102
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoSpecialPriceAsRetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportMagentoSpecialPriceAsWholesale
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportMagentoSpecialPriceAsWholesale, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportMagentoSpecialPriceAsWholesale.Location = New System.Drawing.Point(384, 308)
        Me.chkImportMagentoSpecialPriceAsWholesale.Name = "chkImportMagentoSpecialPriceAsWholesale"
        Me.chkImportMagentoSpecialPriceAsWholesale.Properties.AutoHeight = False
        Me.chkImportMagentoSpecialPriceAsWholesale.Properties.Caption = "Import Magento Special Price as Inventory Item Wholesale price"
        Me.chkImportMagentoSpecialPriceAsWholesale.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportMagentoSpecialPriceAsWholesale, System.Drawing.Color.Empty)
        Me.chkImportMagentoSpecialPriceAsWholesale.Size = New System.Drawing.Size(334, 22)
        Me.chkImportMagentoSpecialPriceAsWholesale.TabIndex = 101
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportMagentoSpecialPriceAsWholesale, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditQtyPublishingValue
        '
        Me.TextEditQtyPublishingValue.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditQtyPublishingValue.Location = New System.Drawing.Point(305, 170)
        Me.TextEditQtyPublishingValue.Name = "TextEditQtyPublishingValue"
        Me.TextEditQtyPublishingValue.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditQtyPublishingValue, System.Drawing.Color.Empty)
        Me.TextEditQtyPublishingValue.Size = New System.Drawing.Size(75, 22)
        Me.TextEditQtyPublishingValue.TabIndex = 100
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblQtyPublishingValue
        '
        Me.lblQtyPublishingValue.Enabled = False
        Me.lblQtyPublishingValue.Location = New System.Drawing.Point(305, 151)
        Me.lblQtyPublishingValue.Name = "lblQtyPublishingValue"
        Me.lblQtyPublishingValue.Size = New System.Drawing.Size(75, 13)
        Me.lblQtyPublishingValue.TabIndex = 99
        Me.lblQtyPublishingValue.Text = "Value to Publish"
        '
        'lblQtyPublishing
        '
        Me.lblQtyPublishing.Location = New System.Drawing.Point(29, 146)
        Me.lblQtyPublishing.Name = "lblQtyPublishing"
        Me.lblQtyPublishing.Size = New System.Drawing.Size(154, 26)
        Me.lblQtyPublishing.TabIndex = 98
        Me.lblQtyPublishing.Text = "Stock Quantity Publishing option" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "for imported Inventory Items" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'RadioGroupQtyPublishing
        '
        Me.ExtendControlProperty.SetHelpText(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.RadioGroupQtyPublishing.Location = New System.Drawing.Point(199, 118)
        Me.RadioGroupQtyPublishing.Name = "RadioGroupQtyPublishing"
        Me.RadioGroupQtyPublishing.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control
        Me.RadioGroupQtyPublishing.Properties.Appearance.Options.UseBackColor = True
        Me.RadioGroupQtyPublishing.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("None", "None"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Fixed", "Fixed Value"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Percent", "% of Total")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.RadioGroupQtyPublishing, System.Drawing.Color.Empty)
        Me.RadioGroupQtyPublishing.Size = New System.Drawing.Size(100, 83)
        Me.RadioGroupQtyPublishing.TabIndex = 97
        Me.ExtendControlProperty.SetTextDisplay(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblCategoriesExist
        '
        Me.lblCategoriesExist.Location = New System.Drawing.Point(401, 94)
        Me.lblCategoriesExist.Name = "lblCategoriesExist"
        Me.lblCategoriesExist.Size = New System.Drawing.Size(293, 13)
        Me.lblCategoriesExist.TabIndex = 15
        Me.lblCategoriesExist.Text = "(Cannot import Categories as some Categories already exist)"
        Me.lblCategoriesExist.Visible = False
        '
        'lblCustomFields
        '
        Me.lblCustomFields.Location = New System.Drawing.Point(427, 443)
        Me.lblCustomFields.Name = "lblCustomFields"
        Me.lblCustomFields.Size = New System.Drawing.Size(306, 26)
        Me.lblCustomFields.TabIndex = 14
        Me.lblCustomFields.Text = "You can create Custom Fields using the Manage Data Dictionary " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "function in the S" & _
    "ystem Manager module"
        Me.lblCustomFields.Visible = False
        '
        'lblSourceOptions
        '
        Me.lblSourceOptions.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblSourceOptions.Location = New System.Drawing.Point(29, 220)
        Me.lblSourceOptions.Name = "lblSourceOptions"
        Me.lblSourceOptions.Size = New System.Drawing.Size(131, 13)
        Me.lblSourceOptions.TabIndex = 13
        Me.lblSourceOptions.Text = "Source Specific Options"
        '
        'lblGeneralOptions
        '
        Me.lblGeneralOptions.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblGeneralOptions.Location = New System.Drawing.Point(29, 72)
        Me.lblGeneralOptions.Name = "lblGeneralOptions"
        Me.lblGeneralOptions.Size = New System.Drawing.Size(90, 13)
        Me.lblGeneralOptions.TabIndex = 12
        Me.lblGeneralOptions.Text = "General Options"
        '
        'cbeASPExtData2CustomField
        '
        Me.cbeASPExtData2CustomField.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeASPExtData2CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeASPExtData2CustomField.Location = New System.Drawing.Point(427, 297)
        Me.cbeASPExtData2CustomField.Name = "cbeASPExtData2CustomField"
        Me.cbeASPExtData2CustomField.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeASPExtData2CustomField.Properties.Appearance.Options.UseBackColor = True
        Me.cbeASPExtData2CustomField.Properties.AutoHeight = False
        Me.cbeASPExtData2CustomField.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeASPExtData2CustomField, System.Drawing.Color.Empty)
        Me.cbeASPExtData2CustomField.Size = New System.Drawing.Size(250, 22)
        Me.cbeASPExtData2CustomField.TabIndex = 11
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeASPExtData2CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportASPStorefrontExtData2
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportASPStorefrontExtData2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportASPStorefrontExtData2.Location = New System.Drawing.Point(27, 298)
        Me.chkImportASPStorefrontExtData2.Name = "chkImportASPStorefrontExtData2"
        Me.chkImportASPStorefrontExtData2.Properties.AutoHeight = False
        Me.chkImportASPStorefrontExtData2.Properties.Caption = "Import ASPStorefront Extension Data2 field to Inventory Item Custom Field"
        Me.chkImportASPStorefrontExtData2.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportASPStorefrontExtData2, System.Drawing.Color.Empty)
        Me.chkImportASPStorefrontExtData2.Size = New System.Drawing.Size(394, 22)
        Me.chkImportASPStorefrontExtData2.TabIndex = 10
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportASPStorefrontExtData2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeASPExtData3CustomField
        '
        Me.cbeASPExtData3CustomField.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeASPExtData3CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeASPExtData3CustomField.Location = New System.Drawing.Point(427, 333)
        Me.cbeASPExtData3CustomField.Name = "cbeASPExtData3CustomField"
        Me.cbeASPExtData3CustomField.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeASPExtData3CustomField.Properties.Appearance.Options.UseBackColor = True
        Me.cbeASPExtData3CustomField.Properties.AutoHeight = False
        Me.cbeASPExtData3CustomField.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeASPExtData3CustomField, System.Drawing.Color.Empty)
        Me.cbeASPExtData3CustomField.Size = New System.Drawing.Size(250, 22)
        Me.cbeASPExtData3CustomField.TabIndex = 9
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeASPExtData3CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportASPStorefrontExtData3
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportASPStorefrontExtData3, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportASPStorefrontExtData3.Location = New System.Drawing.Point(27, 334)
        Me.chkImportASPStorefrontExtData3.Name = "chkImportASPStorefrontExtData3"
        Me.chkImportASPStorefrontExtData3.Properties.AutoHeight = False
        Me.chkImportASPStorefrontExtData3.Properties.Caption = "Import ASPStorefront Extension Data3 field to Inventory Item Custom Field"
        Me.chkImportASPStorefrontExtData3.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportASPStorefrontExtData3, System.Drawing.Color.Empty)
        Me.chkImportASPStorefrontExtData3.Size = New System.Drawing.Size(394, 22)
        Me.chkImportASPStorefrontExtData3.TabIndex = 8
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportASPStorefrontExtData3, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeASPExtData5CustomField
        '
        Me.cbeASPExtData5CustomField.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeASPExtData5CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeASPExtData5CustomField.Location = New System.Drawing.Point(427, 405)
        Me.cbeASPExtData5CustomField.Name = "cbeASPExtData5CustomField"
        Me.cbeASPExtData5CustomField.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeASPExtData5CustomField.Properties.Appearance.Options.UseBackColor = True
        Me.cbeASPExtData5CustomField.Properties.AutoHeight = False
        Me.cbeASPExtData5CustomField.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeASPExtData5CustomField, System.Drawing.Color.Empty)
        Me.cbeASPExtData5CustomField.Size = New System.Drawing.Size(250, 22)
        Me.cbeASPExtData5CustomField.TabIndex = 7
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeASPExtData5CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportASPStorefrontExtData5
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportASPStorefrontExtData5, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportASPStorefrontExtData5.Location = New System.Drawing.Point(27, 406)
        Me.chkImportASPStorefrontExtData5.Name = "chkImportASPStorefrontExtData5"
        Me.chkImportASPStorefrontExtData5.Properties.AutoHeight = False
        Me.chkImportASPStorefrontExtData5.Properties.Caption = "Import ASPStorefront Extension Data5 field to Inventory Item Custom Field"
        Me.chkImportASPStorefrontExtData5.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportASPStorefrontExtData5, System.Drawing.Color.Empty)
        Me.chkImportASPStorefrontExtData5.Size = New System.Drawing.Size(394, 22)
        Me.chkImportASPStorefrontExtData5.TabIndex = 6
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportASPStorefrontExtData5, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeASPExtData4CustomField
        '
        Me.cbeASPExtData4CustomField.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeASPExtData4CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeASPExtData4CustomField.Location = New System.Drawing.Point(427, 369)
        Me.cbeASPExtData4CustomField.Name = "cbeASPExtData4CustomField"
        Me.cbeASPExtData4CustomField.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeASPExtData4CustomField.Properties.Appearance.Options.UseBackColor = True
        Me.cbeASPExtData4CustomField.Properties.AutoHeight = False
        Me.cbeASPExtData4CustomField.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeASPExtData4CustomField, System.Drawing.Color.Empty)
        Me.cbeASPExtData4CustomField.Size = New System.Drawing.Size(250, 22)
        Me.cbeASPExtData4CustomField.TabIndex = 5
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeASPExtData4CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportASPStorefrontExtData4
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportASPStorefrontExtData4, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportASPStorefrontExtData4.Location = New System.Drawing.Point(27, 370)
        Me.chkImportASPStorefrontExtData4.Name = "chkImportASPStorefrontExtData4"
        Me.chkImportASPStorefrontExtData4.Properties.AutoHeight = False
        Me.chkImportASPStorefrontExtData4.Properties.Caption = "Import ASPStorefront Extension Data4 field to Inventory Item Custom Field"
        Me.chkImportASPStorefrontExtData4.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportASPStorefrontExtData4, System.Drawing.Color.Empty)
        Me.chkImportASPStorefrontExtData4.Size = New System.Drawing.Size(394, 22)
        Me.chkImportASPStorefrontExtData4.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportASPStorefrontExtData4, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeASPExtData1CustomField
        '
        Me.cbeASPExtData1CustomField.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeASPExtData1CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeASPExtData1CustomField.Location = New System.Drawing.Point(427, 261)
        Me.cbeASPExtData1CustomField.Name = "cbeASPExtData1CustomField"
        Me.cbeASPExtData1CustomField.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeASPExtData1CustomField.Properties.Appearance.Options.UseBackColor = True
        Me.cbeASPExtData1CustomField.Properties.AutoHeight = False
        Me.cbeASPExtData1CustomField.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeASPExtData1CustomField, System.Drawing.Color.Empty)
        Me.cbeASPExtData1CustomField.Size = New System.Drawing.Size(250, 22)
        Me.cbeASPExtData1CustomField.TabIndex = 3
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeASPExtData1CustomField, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkImportASPStorefrontExtData1
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkImportASPStorefrontExtData1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkImportASPStorefrontExtData1.Location = New System.Drawing.Point(27, 262)
        Me.chkImportASPStorefrontExtData1.Name = "chkImportASPStorefrontExtData1"
        Me.chkImportASPStorefrontExtData1.Properties.AutoHeight = False
        Me.chkImportASPStorefrontExtData1.Properties.Caption = "Import ASPStorefront Extension Data field to Inventory Item Custom Field"
        Me.chkImportASPStorefrontExtData1.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkImportASPStorefrontExtData1, System.Drawing.Color.Empty)
        Me.chkImportASPStorefrontExtData1.Size = New System.Drawing.Size(394, 22)
        Me.chkImportASPStorefrontExtData1.TabIndex = 2
        Me.ExtendControlProperty.SetTextDisplay(Me.chkImportASPStorefrontExtData1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkCreateCategories
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkCreateCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkCreateCategories.Location = New System.Drawing.Point(27, 91)
        Me.chkCreateCategories.Name = "chkCreateCategories"
        Me.chkCreateCategories.Properties.AutoHeight = False
        Me.chkCreateCategories.Properties.Caption = "Create Connected Business Product Categories from source Categories"
        Me.chkCreateCategories.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkCreateCategories, System.Drawing.Color.Empty)
        Me.chkCreateCategories.Size = New System.Drawing.Size(368, 22)
        Me.chkCreateCategories.TabIndex = 1
        Me.ExtendControlProperty.SetTextDisplay(Me.chkCreateCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TabPageCategoryMapping
        '
        Me.TabPageCategoryMapping.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageCategoryMapping.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageCategoryMapping.Controls.Add(Me.lblMappingNotes)
        Me.TabPageCategoryMapping.Controls.Add(Me.lblSelectCatgoryMapping)
        Me.TabPageCategoryMapping.Controls.Add(Me.pnlGetCategoryProgress)
        Me.TabPageCategoryMapping.Controls.Add(Me.TreeListCategories)
        Me.TabPageCategoryMapping.Name = "TabPageCategoryMapping"
        Me.TabPageCategoryMapping.Size = New System.Drawing.Size(851, 475)
        Me.TabPageCategoryMapping.Text = "Source Category Mapping"
        '
        'lblMappingNotes
        '
        Me.lblMappingNotes.Location = New System.Drawing.Point(535, 143)
        Me.lblMappingNotes.Name = "lblMappingNotes"
        Me.lblMappingNotes.Size = New System.Drawing.Size(75, 13)
        Me.lblMappingNotes.TabIndex = 93
        Me.lblMappingNotes.Text = "text set in code"
        '
        'lblSelectCatgoryMapping
        '
        Me.lblSelectCatgoryMapping.Location = New System.Drawing.Point(19, 72)
        Me.lblSelectCatgoryMapping.MinimumSize = New System.Drawing.Size(500, 45)
        Me.lblSelectCatgoryMapping.Name = "lblSelectCatgoryMapping"
        Me.lblSelectCatgoryMapping.Size = New System.Drawing.Size(500, 45)
        Me.lblSelectCatgoryMapping.TabIndex = 92
        Me.lblSelectCatgoryMapping.Text = "Text set in code"
        '
        'pnlGetCategoryProgress
        '
        Me.pnlGetCategoryProgress.Controls.Add(Me.lblGetCategoryProgress)
        Me.pnlGetCategoryProgress.Location = New System.Drawing.Point(152, 223)
        Me.pnlGetCategoryProgress.Name = "pnlGetCategoryProgress"
        Me.pnlGetCategoryProgress.Size = New System.Drawing.Size(200, 100)
        Me.pnlGetCategoryProgress.TabIndex = 91
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
        Me.lblGetCategoryProgress.Text = "Getting SOURCE Categories" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please Wait "
        '
        'TreeListCategories
        '
        Me.TreeListCategories.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colTreeListCategoryName, Me.colTreeListSourceCategoryID, Me.colTreeListISCategoryCode, Me.colTreeListISCategoryName})
        Me.TreeListCategories.Enabled = False
        Me.TreeListCategories.KeyFieldName = "SourceCategoryID"
        Me.TreeListCategories.Location = New System.Drawing.Point(0, 120)
        Me.TreeListCategories.Name = "TreeListCategories"
        Me.TreeListCategories.ParentFieldName = "SourceParentID"
        Me.TreeListCategories.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeISCategoryCode})
        Me.TreeListCategories.Size = New System.Drawing.Size(520, 355)
        Me.TreeListCategories.TabIndex = 90
        '
        'colTreeListCategoryName
        '
        Me.colTreeListCategoryName.Caption = "Source Category"
        Me.colTreeListCategoryName.FieldName = "SourceCategoryName"
        Me.colTreeListCategoryName.Name = "colTreeListCategoryName"
        Me.colTreeListCategoryName.OptionsColumn.AllowEdit = False
        Me.colTreeListCategoryName.OptionsColumn.ReadOnly = True
        Me.colTreeListCategoryName.Visible = True
        Me.colTreeListCategoryName.VisibleIndex = 0
        Me.colTreeListCategoryName.Width = 150
        '
        'colTreeListSourceCategoryID
        '
        Me.colTreeListSourceCategoryID.Caption = "Source Category ID"
        Me.colTreeListSourceCategoryID.FieldName = "SourceCategoryID"
        Me.colTreeListSourceCategoryID.Name = "colTreeListSourceCategoryID"
        Me.colTreeListSourceCategoryID.Width = 20
        '
        'colTreeListISCategoryCode
        '
        Me.colTreeListISCategoryCode.Caption = "CategoryCode"
        Me.colTreeListISCategoryCode.FieldName = "ISCategoryCode"
        Me.colTreeListISCategoryCode.Name = "colTreeListISCategoryCode"
        Me.colTreeListISCategoryCode.Width = 20
        '
        'colTreeListISCategoryName
        '
        Me.colTreeListISCategoryName.Caption = "Map to CB Product Category"
        Me.colTreeListISCategoryName.ColumnEdit = Me.rbeISCategoryCode
        Me.colTreeListISCategoryName.FieldName = "ISCategoryName"
        Me.colTreeListISCategoryName.Name = "colTreeListISCategoryName"
        Me.colTreeListISCategoryName.Visible = True
        Me.colTreeListISCategoryName.VisibleIndex = 1
        '
        'rbeISCategoryCode
        '
        Me.rbeISCategoryCode.AdditionalFilter = ""
        Me.rbeISCategoryCode.AutoHeight = False
        Me.rbeISCategoryCode.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeISCategoryCode.Columns = New String() {"Description", "ParentCategory"}
        Me.rbeISCategoryCode.DataSource = Nothing
        Me.rbeISCategoryCode.DisplayField = "Description"
        Me.rbeISCategoryCode.KeyField = "CategoryCode"
        Me.rbeISCategoryCode.Movement = Lerryn.Presentation.eShopCONNECT.Search.RepSearchTreeControl.enmMovement.Vertical
        Me.rbeISCategoryCode.Name = "rbeISCategoryCode"
        Me.rbeISCategoryCode.ParentField = "ParentCategory"
        Me.rbeISCategoryCode.RetainValue = False
        Me.rbeISCategoryCode.TableName = "SystemCategoryView"
        Me.rbeISCategoryCode.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.rbeISCategoryCode.TreeImageList = Me.imagelistCategory
        Me.rbeISCategoryCode.ValidateOnEnterKey = True
        '
        'imagelistCategory
        '
        Me.imagelistCategory.ImageStream = CType(resources.GetObject("imagelistCategory.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imagelistCategory.TransparentColor = System.Drawing.Color.Transparent
        Me.imagelistCategory.Images.SetKeyName(0, "ParentCategory.png")
        Me.imagelistCategory.Images.SetKeyName(1, "ChildCategory.png")
        '
        'TabPageImporting
        '
        Me.TabPageImporting.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageImporting.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageImporting.Controls.Add(Me.pnlImportProgress)
        Me.TabPageImporting.Name = "TabPageImporting"
        Me.TabPageImporting.Size = New System.Drawing.Size(851, 475)
        Me.TabPageImporting.Text = "Importing Products"
        '
        'pnlImportProgress
        '
        Me.pnlImportProgress.Controls.Add(Me.lblImportPleaseWait)
        Me.pnlImportProgress.Controls.Add(Me.lblImportProgress)
        Me.pnlImportProgress.Location = New System.Drawing.Point(284, 191)
        Me.pnlImportProgress.Name = "pnlImportProgress"
        Me.pnlImportProgress.Size = New System.Drawing.Size(240, 100)
        Me.pnlImportProgress.TabIndex = 11
        Me.pnlImportProgress.Visible = False
        '
        'lblImportPleaseWait
        '
        Me.lblImportPleaseWait.Location = New System.Drawing.Point(93, 62)
        Me.lblImportPleaseWait.Name = "lblImportPleaseWait"
        Me.lblImportPleaseWait.Size = New System.Drawing.Size(54, 13)
        Me.lblImportPleaseWait.TabIndex = 1
        Me.lblImportPleaseWait.Text = "Please wait"
        '
        'lblImportProgress
        '
        Me.lblImportProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblImportProgress.Location = New System.Drawing.Point(3, 28)
        Me.lblImportProgress.MinimumSize = New System.Drawing.Size(234, 25)
        Me.lblImportProgress.Name = "lblImportProgress"
        Me.lblImportProgress.Size = New System.Drawing.Size(234, 25)
        Me.lblImportProgress.TabIndex = 0
        Me.lblImportProgress.Text = "Importing Products from"
        '
        'chkInhibitWarnings
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkInhibitWarnings, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkInhibitWarnings.Location = New System.Drawing.Point(37, 499)
        Me.chkInhibitWarnings.Name = "chkInhibitWarnings"
        Me.chkInhibitWarnings.Properties.AutoHeight = False
        Me.chkInhibitWarnings.Properties.Caption = "Only ask once to confirm each import update type"
        Me.chkInhibitWarnings.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkInhibitWarnings, System.Drawing.Color.Empty)
        Me.chkInhibitWarnings.Size = New System.Drawing.Size(268, 22)
        Me.chkInhibitWarnings.TabIndex = 11
        Me.ExtendControlProperty.SetTextDisplay(Me.chkInhibitWarnings, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkInhibitWarnings.Visible = False
        '
        'ImportWizardSectionContainer
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.ImportWizardPluginContainerControl)
        Me.FindSearch = Interprise.Framework.Base.[Shared].[Enum].FindSearch.None
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "ImportWizardSectionContainer"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(861, 537)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TransactionType = Interprise.Framework.Base.[Shared].[Enum].TransactionType.None
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImportWizardSectionContainerGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImportWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ImportWizardPluginContainerControl.ResumeLayout(False)
        CType(Me.WizardControlImport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardControlImport.ResumeLayout(False)
        Me.TabPageComplete.ResumeLayout(False)
        Me.TabPageComplete.PerformLayout()
        CType(Me.CheckEditViewImportLog.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditImportLog.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageWelcome.ResumeLayout(False)
        Me.TabPageWelcome.PerformLayout()
        CType(Me.cbeSiteOrAccount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeImportSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxLerrynLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageSelectItems.ResumeLayout(False)
        Me.TabPageSelectItems.PerformLayout()
        CType(Me.pnlGetListError, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetListError.ResumeLayout(False)
        Me.pnlGetListError.PerformLayout()
        CType(Me.PictureBoxGridColorKey3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxGridColorKey1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxGridColorKey2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlGetListProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetListProgress.ResumeLayout(False)
        Me.pnlGetListProgress.PerformLayout()
        CType(Me.GridControlSelectItems, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewSelectItems, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repISItemType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repSourceItemType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageOptions.ResumeLayout(False)
        Me.TabPageOptions.PerformLayout()
        CType(Me.chkImportMagentoSellingPriceAsSuggestedRetail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoCostAsPricingCost.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoSellingPriceAsRetail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoSellingPriceAsWholesale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoLastCost.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoStandardCost.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoAverageCost.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoSpecialPriceAsRetail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportMagentoSpecialPriceAsWholesale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeASPExtData2CustomField.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportASPStorefrontExtData2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeASPExtData3CustomField.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportASPStorefrontExtData3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeASPExtData5CustomField.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportASPStorefrontExtData5.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeASPExtData4CustomField.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportASPStorefrontExtData4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeASPExtData1CustomField.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportASPStorefrontExtData1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkCreateCategories.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageCategoryMapping.ResumeLayout(False)
        Me.TabPageCategoryMapping.PerformLayout()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetCategoryProgress.ResumeLayout(False)
        Me.pnlGetCategoryProgress.PerformLayout()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeISCategoryCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageImporting.ResumeLayout(False)
        CType(Me.pnlImportProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlImportProgress.ResumeLayout(False)
        Me.pnlImportProgress.PerformLayout()
        CType(Me.chkInhibitWarnings.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend ImportWizardSectionContainerGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents ImportWizardPluginContainerControl As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents WizardControlImport As Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl
    Friend WithEvents TabPageComplete As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageWelcome As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageShared1 As Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared
    Friend WithEvents PictureBoxLerrynLogo As System.Windows.Forms.PictureBox
    Friend WithEvents cbeImportSource As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblMultipleSiteOrAccount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSiteOrAccount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblImportSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSiteOrAccount As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents TabPageSelectItems As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents GridControlSelectItems As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewSelectItems As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colSelect As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colItemName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colItemSKU As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceItemID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnSelectAll As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSelectNone As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents colAlreadyImported As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repSourceItemType As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents pnlGetListProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetListProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblGetListPleaseWait As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlImportProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblImportPleaseWait As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblImportProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TabPageImporting As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageOptions As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents cbeASPExtData2CustomField As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkImportASPStorefrontExtData2 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cbeASPExtData3CustomField As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkImportASPStorefrontExtData3 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cbeASPExtData5CustomField As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkImportASPStorefrontExtData5 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cbeASPExtData4CustomField As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkImportASPStorefrontExtData4 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cbeASPExtData1CustomField As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkImportASPStorefrontExtData1 As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkCreateCategories As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblCustomFields As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSourceOptions As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblGeneralOptions As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCategoriesExist As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditImportLog As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblImportLog As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblGridColorKey As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureBoxGridColorKey1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblGridColorKey1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblGridColorKey2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureBoxGridColorKey2 As System.Windows.Forms.PictureBox
    Friend WithEvents TabPageCategoryMapping As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents pnlGetCategoryProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetCategoryProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TreeListCategories As DevExpress.XtraTreeList.TreeList
    Friend WithEvents colTreeListCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListSourceCategoryID As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListISCategoryCode As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents rbeISCategoryCode As Search.RepSearchTreeControl
    Friend WithEvents lblSelectCatgoryMapping As DevExpress.XtraEditors.LabelControl
    Friend WithEvents imagelistCategory As System.Windows.Forms.ImageList
    Friend WithEvents colTreeListISCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents lblMappingNotes As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditQtyPublishingValue As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblQtyPublishingValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblQtyPublishing As DevExpress.XtraEditors.LabelControl
    Friend WithEvents RadioGroupQtyPublishing As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents colSKUExists As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSKUError As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSKUChanged As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colItemCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents lblGridColorKey3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureBoxGridColorKey3 As System.Windows.Forms.PictureBox
    Friend WithEvents colImportAsKit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CheckEditViewImportLog As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents pnlGetListError As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetListError As DevExpress.XtraEditors.LabelControl
    Friend WithEvents colISItemType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repISItemType As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents chkImportMagentoSpecialPriceAsRetail As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoSpecialPriceAsWholesale As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoStandardCost As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoAverageCost As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoLastCost As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkInhibitWarnings As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoSellingPriceAsWholesale As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoSellingPriceAsRetail As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoCostAsPricingCost As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkImportMagentoSellingPriceAsSuggestedRetail As DevExpress.XtraEditors.CheckEdit

End Class
#End Region
