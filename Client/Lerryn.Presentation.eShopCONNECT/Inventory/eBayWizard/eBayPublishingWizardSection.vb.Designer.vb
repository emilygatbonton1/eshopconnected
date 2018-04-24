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

#Region " eBayPublishingWizardSection "
Namespace eBayWizard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class eBayPublishingWizardSection
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(eBayPublishingWizardSection))
            Me.eBayPublishingWizardSectionGateway = Me.ImportExportDataset
            Me.eBayPublishingWizardPluginContainerControl = New Interprise.Presentation.Base.PluginContainerControl()
            Me.WizardControlEBayPub = New Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl()
            Me.TabPageComplete = New DevExpress.XtraTab.XtraTabPage()
            Me.PageDescriptionCollection1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.PageDescriptionCollection()
            Me.TabPageWelcome = New DevExpress.XtraTab.XtraTabPage()
            Me.txtDescriptiveTitle = New DevExpress.XtraEditors.TextEdit()
            Me.lblAuctionType = New System.Windows.Forms.Label()
            Me.rdoAuctionType = New DevExpress.XtraEditors.RadioGroup()
            Me.lblAuctionTypeDescription = New DevExpress.XtraEditors.LabelControl()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.TabPageSelectCategory = New DevExpress.XtraTab.XtraTabPage()
            Me.chkAllowCategoryMapping = New DevExpress.XtraEditors.CheckEdit()
            Me.lblCondition = New System.Windows.Forms.Label()
            Me.cmbCondition = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.btnBrowseNodeUpLeveleBay = New DevExpress.XtraEditors.SimpleButton()
            Me.TextEditCategoryTreeeBay = New DevExpress.XtraEditors.TextEdit()
            Me.imgListeBayCategoriesToAdd = New DevExpress.XtraEditors.ImageListBoxControl()
            Me.btnRemoveCategory = New DevExpress.XtraEditors.SimpleButton()
            Me.btnAddCategory = New DevExpress.XtraEditors.SimpleButton()
            Me.txtSuggestedCategories = New System.Windows.Forms.TextBox()
            Me.btnGetSuggestedCategories = New DevExpress.XtraEditors.SimpleButton()
            Me.cmdBrowseCategories = New DevExpress.XtraEditors.SimpleButton()
            Me.imgListeBayCategory = New DevExpress.XtraEditors.ImageListBoxControl()
            Me.lblEbayStatus = New System.Windows.Forms.Label()
            Me.lblSelectCategory = New System.Windows.Forms.Label()
            Me.TabPageAddPictures = New DevExpress.XtraTab.XtraTabPage()
            Me.PictureBox3 = New System.Windows.Forms.PictureBox()
            Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
            Me.PictureBox2 = New System.Windows.Forms.PictureBox()
            Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
            Me.PictureBox1 = New System.Windows.Forms.PictureBox()
            Me.cmdAddPhoto = New DevExpress.XtraEditors.SimpleButton()
            Me.TabPageDescribeItem = New DevExpress.XtraTab.XtraTabPage()
            Me.lblHTMLTemplateFilePath = New DevExpress.XtraEditors.LabelControl()
            Me.cmdGetTemplate = New DevExpress.XtraEditors.SimpleButton()
            Me.MemoHTMLTemplate = New DevExpress.XtraEditors.MemoEdit()
            Me.lblHTMLTemplate = New DevExpress.XtraEditors.LabelControl()
            Me.tabPagePricePostage = New DevExpress.XtraTab.XtraTabPage()
            Me.lblReturnWithin = New DevExpress.XtraEditors.LabelControl()
            Me.cmbReturnWithin = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.lblShippingCostsPaidBy = New DevExpress.XtraEditors.LabelControl()
            Me.cmbShippingCostsPaidBy = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.lblRefundOption = New DevExpress.XtraEditors.LabelControl()
            Me.cmbRefundOptions = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.lblReturnsPolicyText = New DevExpress.XtraEditors.LabelControl()
            Me.txtReturnsPolicyText = New DevExpress.XtraEditors.TextEdit()
            Me.lblReturnsPolicy = New DevExpress.XtraEditors.LabelControl()
            Me.cmbReturnsPolicy = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.spnPercentOfInterpriseSellingPrice = New DevExpress.XtraEditors.SpinEdit()
            Me.chkPercentOfInterpriseSellingPrice = New DevExpress.XtraEditors.CheckEdit()
            Me.lblOrSellingPrice = New DevExpress.XtraEditors.LabelControl()
            Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
            Me.cmbDispatchTime = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.TimeEditScheduledTime = New DevExpress.XtraEditors.TimeEdit()
            Me.DateEditScheduledTime = New DevExpress.XtraEditors.DateEdit()
            Me.rdoStartTime = New DevExpress.XtraEditors.RadioGroup()
            Me.cmbService = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.cmbPostalDestination = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton()
            Me.txtPostageCostToBuyer = New DevExpress.XtraEditors.TextEdit()
            Me.lblPostageCostToBuyer = New DevExpress.XtraEditors.LabelControl()
            Me.lblService = New DevExpress.XtraEditors.LabelControl()
            Me.lblPostalDestination = New DevExpress.XtraEditors.LabelControl()
            Me.txtBuyItNowPrice = New DevExpress.XtraEditors.TextEdit()
            Me.lblToTheListing = New DevExpress.XtraEditors.LabelControl()
            Me.PictureBox4 = New System.Windows.Forms.PictureBox()
            Me.chkAddBuyItNow = New DevExpress.XtraEditors.CheckEdit()
            Me.lblLastingFor = New DevExpress.XtraEditors.LabelControl()
            Me.lblStartAuction = New DevExpress.XtraEditors.LabelControl()
            Me.txtStartAuction = New DevExpress.XtraEditors.TextEdit()
            Me.lblCurrency3 = New DevExpress.XtraEditors.LabelControl()
            Me.lblCurrency2 = New DevExpress.XtraEditors.LabelControl()
            Me.lblCurrency1 = New DevExpress.XtraEditors.LabelControl()
            Me.cmbLastingFor = New DevExpress.XtraEditors.ImageComboBoxEdit()
            Me.tabPageHowPaid = New DevExpress.XtraTab.XtraTabPage()
            Me.cmdPasteXML = New System.Windows.Forms.Button()
            Me.MemoEditPublishStatus = New DevExpress.XtraEditors.MemoEdit()
            Me.cmdPublish = New System.Windows.Forms.Button()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.txtPaypalEmail = New DevExpress.XtraEditors.TextEdit()
            Me.PictureBox5 = New System.Windows.Forms.PictureBox()
            Me.lblAcceptPaymentWith = New System.Windows.Forms.Label()
            Me.lblHowPaid = New System.Windows.Forms.Label()
            Me.picTick = New System.Windows.Forms.PictureBox()
            Me.tabPageStockQuantity = New DevExpress.XtraTab.XtraTabPage()
            Me.lblYourLocation = New System.Windows.Forms.Label()
            Me.txtLocation = New DevExpress.XtraEditors.TextEdit()
            Me.lblStockQuantity = New System.Windows.Forms.Label()
            Me.spnPercentOfTotal = New DevExpress.XtraEditors.SpinEdit()
            Me.rdoAuctionQuantity = New DevExpress.XtraEditors.RadioGroup()
            Me.tabPageStockMultiple = New DevExpress.XtraTab.XtraTabPage()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.spnMultipleAuctions = New DevExpress.XtraEditors.SpinEdit()
            Me.grpFrequencyNormal = New System.Windows.Forms.GroupBox()
            Me.rdoRepatAuctionFor = New DevExpress.XtraEditors.RadioGroup()
            Me.spnRepeatQuantity = New DevExpress.XtraEditors.SpinEdit()
            Me.chkRepeatAuction = New DevExpress.XtraEditors.CheckEdit()
            Me.grpSchedule = New System.Windows.Forms.GroupBox()
            Me.TimeEditSpecifiedTime = New DevExpress.XtraEditors.TimeEdit()
            Me.rdoSchedule = New DevExpress.XtraEditors.RadioGroup()
            Me.tabPageCatalogEnabledCategory1 = New DevExpress.XtraTab.XtraTabPage()
            Me.chkFlagAsComplete1 = New DevExpress.XtraEditors.CheckEdit()
            Me.lblCatalogEnabledXofY1 = New System.Windows.Forms.Label()
            Me.tabPageCatalogEnabledCategory2 = New DevExpress.XtraTab.XtraTabPage()
            Me.TabPageShared1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared()
            Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
            Me.chkFlagAsComplete2 = New DevExpress.XtraEditors.CheckEdit()
            Me.lblCatalogEnabledXofY2 = New System.Windows.Forms.Label()
            CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.eBayPublishingWizardSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.eBayPublishingWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.eBayPublishingWizardPluginContainerControl.SuspendLayout()
            CType(Me.WizardControlEBayPub, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.WizardControlEBayPub.SuspendLayout()
            Me.TabPageWelcome.SuspendLayout()
            CType(Me.txtDescriptiveTitle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.rdoAuctionType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPageSelectCategory.SuspendLayout()
            CType(Me.chkAllowCategoryMapping.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbCondition.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditCategoryTreeeBay.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.imgListeBayCategoriesToAdd, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.imgListeBayCategory, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPageAddPictures.SuspendLayout()
            CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPageDescribeItem.SuspendLayout()
            CType(Me.MemoHTMLTemplate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPagePricePostage.SuspendLayout()
            CType(Me.cmbReturnWithin.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbShippingCostsPaidBy.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbRefundOptions.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtReturnsPolicyText.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbReturnsPolicy.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.spnPercentOfInterpriseSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkPercentOfInterpriseSellingPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbDispatchTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox1.SuspendLayout()
            CType(Me.TimeEditScheduledTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditScheduledTime.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditScheduledTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.rdoStartTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbService.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbPostalDestination.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPostageCostToBuyer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtBuyItNowPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkAddBuyItNow.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtStartAuction.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cmbLastingFor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPageHowPaid.SuspendLayout()
            CType(Me.MemoEditPublishStatus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPaypalEmail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picTick, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPageStockQuantity.SuspendLayout()
            CType(Me.txtLocation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.spnPercentOfTotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.rdoAuctionQuantity.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPageStockMultiple.SuspendLayout()
            CType(Me.spnMultipleAuctions.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpFrequencyNormal.SuspendLayout()
            CType(Me.rdoRepatAuctionFor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.spnRepeatQuantity.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkRepeatAuction.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpSchedule.SuspendLayout()
            CType(Me.TimeEditSpecifiedTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.rdoSchedule.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPageCatalogEnabledCategory1.SuspendLayout()
            CType(Me.chkFlagAsComplete1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPageCatalogEnabledCategory2.SuspendLayout()
            CType(Me.chkFlagAsComplete2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ImageCollectionContextMenu
            '
            Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
            '
            'eBayPublishingWizardSectionGateway
            '
            Me.eBayPublishingWizardSectionGateway.DataSetName = "eBayPublishingWizardSectionDataset"
            Me.eBayPublishingWizardSectionGateway.Instantiate = False
            Me.eBayPublishingWizardSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
            'eBayPublishingWizardPluginContainerControl
            '
            Me.eBayPublishingWizardPluginContainerControl.AppearanceCaption.Options.UseTextOptions = True
            Me.eBayPublishingWizardPluginContainerControl.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            Me.eBayPublishingWizardPluginContainerControl.BaseLayoutControl = Nothing
            Me.eBayPublishingWizardPluginContainerControl.ContextMenuButtonCaption = Nothing
            Me.eBayPublishingWizardPluginContainerControl.Controls.Add(Me.WizardControlEBayPub)
            Me.eBayPublishingWizardPluginContainerControl.CurrentControl = Nothing
            Me.eBayPublishingWizardPluginContainerControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.eBayPublishingWizardPluginContainerControl.EditorsHeight = 0
            Me.eBayPublishingWizardPluginContainerControl.GroupContextMenu = Nothing
            Me.eBayPublishingWizardPluginContainerControl.HelpCode = Nothing
            Me.eBayPublishingWizardPluginContainerControl.LayoutMode = False
            Me.eBayPublishingWizardPluginContainerControl.Location = New System.Drawing.Point(0, 0)
            Me.eBayPublishingWizardPluginContainerControl.Name = "eBayPublishingWizardPluginContainerControl"
            Me.eBayPublishingWizardPluginContainerControl.PluginManagerButton = Nothing
            Me.eBayPublishingWizardPluginContainerControl.SearchPluginButton = Nothing
            Me.eBayPublishingWizardPluginContainerControl.ShowCaption = False
            Me.eBayPublishingWizardPluginContainerControl.Size = New System.Drawing.Size(700, 417)
            Me.eBayPublishingWizardPluginContainerControl.TabIndex = 0
            '
            'WizardControlEBayPub
            '
            Me.WizardControlEBayPub.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.WizardControlEBayPub.Appearance.Options.UseBackColor = True
            Me.WizardControlEBayPub.AppearancePage.Header.Options.UseTextOptions = True
            Me.WizardControlEBayPub.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            Me.WizardControlEBayPub.AppearancePage.PageClient.BackColor = System.Drawing.SystemColors.Control
            Me.WizardControlEBayPub.AppearancePage.PageClient.Options.UseBackColor = True
            Me.WizardControlEBayPub.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
            Me.WizardControlEBayPub.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
            Me.WizardControlEBayPub.DisplayFinishButton = False
            Me.WizardControlEBayPub.DisplayNextButton = True
            Me.WizardControlEBayPub.Dock = System.Windows.Forms.DockStyle.Fill
            Me.WizardControlEBayPub.FinishMessage = "You have successfully completed the eBay publishing Wizard."
            Me.WizardControlEBayPub.FinishPage = Me.TabPageComplete
            Me.WizardControlEBayPub.IsCustom = False
            Me.WizardControlEBayPub.IsPlugIn = True
            Me.WizardControlEBayPub.Location = New System.Drawing.Point(2, 2)
            Me.WizardControlEBayPub.Name = "WizardControlEBayPub"
            Me.WizardControlEBayPub.NextButtonText = "&Next >"
            Me.WizardControlEBayPub.PageDescription = Me.PageDescriptionCollection1
            Me.WizardControlEBayPub.PaintStyleName = "Skin"
            Me.WizardControlEBayPub.SelectedTabPage = Me.TabPageWelcome
            Me.WizardControlEBayPub.SharedPage = Me.TabPageShared1
            Me.WizardControlEBayPub.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.[False]
            Me.WizardControlEBayPub.ShowTabHeader = DevExpress.Utils.DefaultBoolean.[False]
            Me.WizardControlEBayPub.Size = New System.Drawing.Size(696, 413)
            Me.WizardControlEBayPub.TabIndex = 1
            Me.WizardControlEBayPub.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.TabPageWelcome, Me.TabPageSelectCategory, Me.TabPageAddPictures, Me.TabPageDescribeItem, Me.tabPagePricePostage, Me.tabPageStockQuantity, Me.tabPageStockMultiple, Me.tabPageHowPaid, Me.tabPageCatalogEnabledCategory1, Me.tabPageCatalogEnabledCategory2, Me.TabPageComplete})
            Me.WizardControlEBayPub.Title = "the eBay publishing"
            Me.WizardControlEBayPub.WelcomeMessage = "Sell an item on eBay using eShopCONNECTED"
            Me.WizardControlEBayPub.WelcomePage = Me.TabPageWelcome
            '
            'TabPageComplete
            '
            Me.TabPageComplete.Appearance.PageClient.BackColor = System.Drawing.Color.White
            Me.TabPageComplete.Appearance.PageClient.Options.UseBackColor = True
            Me.TabPageComplete.Name = "TabPageComplete"
            Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageComplete, "")
            Me.TabPageComplete.Size = New System.Drawing.Size(690, 355)
            Me.TabPageComplete.Text = "TabPageComplete"
            '
            'TabPageWelcome
            '
            Me.TabPageWelcome.Appearance.PageClient.BackColor = System.Drawing.Color.White
            Me.TabPageWelcome.Appearance.PageClient.Options.UseBackColor = True
            Me.TabPageWelcome.Controls.Add(Me.txtDescriptiveTitle)
            Me.TabPageWelcome.Controls.Add(Me.lblAuctionType)
            Me.TabPageWelcome.Controls.Add(Me.rdoAuctionType)
            Me.TabPageWelcome.Controls.Add(Me.lblAuctionTypeDescription)
            Me.TabPageWelcome.Controls.Add(Me.lblDescription)
            Me.TabPageWelcome.Name = "TabPageWelcome"
            Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageWelcome, "")
            Me.TabPageWelcome.Size = New System.Drawing.Size(690, 407)
            Me.TabPageWelcome.Text = "TabPageWelcome"
            '
            'txtDescriptiveTitle
            '
            Me.ExtendControlProperty.SetHelpText(Me.txtDescriptiveTitle, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtDescriptiveTitle.Location = New System.Drawing.Point(317, 153)
            Me.txtDescriptiveTitle.Name = "txtDescriptiveTitle"
            Me.txtDescriptiveTitle.Properties.AutoHeight = False
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtDescriptiveTitle, System.Drawing.Color.Empty)
            Me.txtDescriptiveTitle.Size = New System.Drawing.Size(354, 22)
            Me.txtDescriptiveTitle.TabIndex = 39
            Me.ExtendControlProperty.SetTextDisplay(Me.txtDescriptiveTitle, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblAuctionType
            '
            Me.lblAuctionType.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblAuctionType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblAuctionType.Location = New System.Drawing.Point(213, 264)
            Me.lblAuctionType.Name = "lblAuctionType"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblAuctionType, System.Drawing.Color.Empty)
            Me.lblAuctionType.Size = New System.Drawing.Size(74, 13)
            Me.lblAuctionType.TabIndex = 38
            Me.lblAuctionType.Text = "Auction Type:"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblAuctionType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblAuctionType.Visible = False
            '
            'rdoAuctionType
            '
            Me.ExtendControlProperty.SetHelpText(Me.rdoAuctionType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.rdoAuctionType.Location = New System.Drawing.Point(317, 212)
            Me.rdoAuctionType.Name = "rdoAuctionType"
            Me.rdoAuctionType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.rdoAuctionType.Properties.Appearance.Options.UseBackColor = True
            Me.rdoAuctionType.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Normal"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Stock Quantity"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Multiple")})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rdoAuctionType, System.Drawing.Color.Empty)
            Me.rdoAuctionType.Size = New System.Drawing.Size(179, 112)
            Me.rdoAuctionType.TabIndex = 37
            Me.ExtendControlProperty.SetTextDisplay(Me.rdoAuctionType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.rdoAuctionType.Visible = False
            '
            'lblAuctionTypeDescription
            '
            Me.lblAuctionTypeDescription.Location = New System.Drawing.Point(336, 335)
            Me.lblAuctionTypeDescription.Name = "lblAuctionTypeDescription"
            Me.lblAuctionTypeDescription.Size = New System.Drawing.Size(123, 13)
            Me.lblAuctionTypeDescription.TabIndex = 20
            Me.lblAuctionTypeDescription.Text = "lblAuctionTypeDescription"
            Me.lblAuctionTypeDescription.Visible = False
            '
            'lblDescription
            '
            Me.lblDescription.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblDescription.Location = New System.Drawing.Point(213, 157)
            Me.lblDescription.Name = "lblDescription"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblDescription, System.Drawing.Color.Empty)
            Me.lblDescription.Size = New System.Drawing.Size(87, 13)
            Me.lblDescription.TabIndex = 3
            Me.lblDescription.Text = "Descriptive Title:"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TabPageSelectCategory
            '
            Me.TabPageSelectCategory.Controls.Add(Me.chkAllowCategoryMapping)
            Me.TabPageSelectCategory.Controls.Add(Me.lblCondition)
            Me.TabPageSelectCategory.Controls.Add(Me.cmbCondition)
            Me.TabPageSelectCategory.Controls.Add(Me.btnBrowseNodeUpLeveleBay)
            Me.TabPageSelectCategory.Controls.Add(Me.TextEditCategoryTreeeBay)
            Me.TabPageSelectCategory.Controls.Add(Me.imgListeBayCategoriesToAdd)
            Me.TabPageSelectCategory.Controls.Add(Me.btnRemoveCategory)
            Me.TabPageSelectCategory.Controls.Add(Me.btnAddCategory)
            Me.TabPageSelectCategory.Controls.Add(Me.txtSuggestedCategories)
            Me.TabPageSelectCategory.Controls.Add(Me.btnGetSuggestedCategories)
            Me.TabPageSelectCategory.Controls.Add(Me.cmdBrowseCategories)
            Me.TabPageSelectCategory.Controls.Add(Me.imgListeBayCategory)
            Me.TabPageSelectCategory.Controls.Add(Me.lblEbayStatus)
            Me.TabPageSelectCategory.Controls.Add(Me.lblSelectCategory)
            Me.TabPageSelectCategory.Name = "TabPageSelectCategory"
            Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageSelectCategory, "")
            Me.TabPageSelectCategory.Size = New System.Drawing.Size(690, 355)
            Me.TabPageSelectCategory.Text = "Select the category that best describes your item"
            '
            'chkAllowCategoryMapping
            '
            Me.chkAllowCategoryMapping.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.chkAllowCategoryMapping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.chkAllowCategoryMapping.Location = New System.Drawing.Point(519, 298)
            Me.chkAllowCategoryMapping.Name = "chkAllowCategoryMapping"
            Me.chkAllowCategoryMapping.Properties.AutoHeight = False
            Me.chkAllowCategoryMapping.Properties.Caption = "Allow Category Mapping?"
            Me.chkAllowCategoryMapping.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkAllowCategoryMapping, System.Drawing.Color.Empty)
            Me.chkAllowCategoryMapping.Size = New System.Drawing.Size(154, 22)
            Me.chkAllowCategoryMapping.TabIndex = 43
            Me.ExtendControlProperty.SetTextDisplay(Me.chkAllowCategoryMapping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblCondition
            '
            Me.lblCondition.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblCondition, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblCondition.Location = New System.Drawing.Point(464, 161)
            Me.lblCondition.Name = "lblCondition"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblCondition, System.Drawing.Color.Empty)
            Me.lblCondition.Size = New System.Drawing.Size(52, 13)
            Me.lblCondition.TabIndex = 42
            Me.lblCondition.Text = "Condition"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblCondition, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'cmbCondition
            '
            Me.cmbCondition.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.cmbCondition, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbCondition.Location = New System.Drawing.Point(521, 157)
            Me.cmbCondition.Name = "cmbCondition"
            Me.cmbCondition.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbCondition.Properties.Appearance.Options.UseBackColor = True
            Me.cmbCondition.Properties.AutoHeight = False
            Me.cmbCondition.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbCondition, System.Drawing.Color.Empty)
            Me.cmbCondition.Size = New System.Drawing.Size(152, 22)
            Me.cmbCondition.TabIndex = 41
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbCondition, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'btnBrowseNodeUpLeveleBay
            '
            Me.btnBrowseNodeUpLeveleBay.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.btnBrowseNodeUpLeveleBay, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.btnBrowseNodeUpLeveleBay.Image = CType(resources.GetObject("btnBrowseNodeUpLeveleBay.Image"), System.Drawing.Image)
            Me.btnBrowseNodeUpLeveleBay.Location = New System.Drawing.Point(442, 104)
            Me.btnBrowseNodeUpLeveleBay.Name = "btnBrowseNodeUpLeveleBay"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnBrowseNodeUpLeveleBay, System.Drawing.Color.Empty)
            Me.btnBrowseNodeUpLeveleBay.Size = New System.Drawing.Size(21, 22)
            Me.btnBrowseNodeUpLeveleBay.TabIndex = 40
            Me.ExtendControlProperty.SetTextDisplay(Me.btnBrowseNodeUpLeveleBay, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.btnBrowseNodeUpLeveleBay.ToolTip = "Up Browse Tree Node"
            '
            'TextEditCategoryTreeeBay
            '
            Me.TextEditCategoryTreeeBay.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.TextEditCategoryTreeeBay, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditCategoryTreeeBay.Location = New System.Drawing.Point(5, 104)
            Me.TextEditCategoryTreeeBay.Name = "TextEditCategoryTreeeBay"
            Me.TextEditCategoryTreeeBay.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
            Me.TextEditCategoryTreeeBay.Properties.Appearance.Options.UseBackColor = True
            Me.TextEditCategoryTreeeBay.Properties.AutoHeight = False
            Me.TextEditCategoryTreeeBay.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditCategoryTreeeBay, System.Drawing.Color.Empty)
            Me.TextEditCategoryTreeeBay.Size = New System.Drawing.Size(436, 22)
            Me.TextEditCategoryTreeeBay.TabIndex = 39
            Me.TextEditCategoryTreeeBay.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditCategoryTreeeBay, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'imgListeBayCategoriesToAdd
            '
            Me.imgListeBayCategoriesToAdd.Appearance.BorderColor = System.Drawing.Color.Red
            Me.imgListeBayCategoriesToAdd.Appearance.Options.UseBorderColor = True
            Me.imgListeBayCategoriesToAdd.Location = New System.Drawing.Point(281, 132)
            Me.imgListeBayCategoriesToAdd.Name = "imgListeBayCategoriesToAdd"
            Me.imgListeBayCategoriesToAdd.Size = New System.Drawing.Size(182, 188)
            Me.imgListeBayCategoriesToAdd.TabIndex = 38
            '
            'btnRemoveCategory
            '
            Me.ExtendControlProperty.SetHelpText(Me.btnRemoveCategory, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.btnRemoveCategory.Location = New System.Drawing.Point(188, 161)
            Me.btnRemoveCategory.Name = "btnRemoveCategory"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnRemoveCategory, System.Drawing.Color.Empty)
            Me.btnRemoveCategory.Size = New System.Drawing.Size(90, 23)
            Me.btnRemoveCategory.TabIndex = 37
            Me.btnRemoveCategory.Text = "Remove"
            Me.ExtendControlProperty.SetTextDisplay(Me.btnRemoveCategory, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'btnAddCategory
            '
            Me.ExtendControlProperty.SetHelpText(Me.btnAddCategory, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.btnAddCategory.Location = New System.Drawing.Point(188, 132)
            Me.btnAddCategory.Name = "btnAddCategory"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnAddCategory, System.Drawing.Color.Empty)
            Me.btnAddCategory.Size = New System.Drawing.Size(90, 23)
            Me.btnAddCategory.TabIndex = 36
            Me.btnAddCategory.Text = "Add"
            Me.ExtendControlProperty.SetTextDisplay(Me.btnAddCategory, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'txtSuggestedCategories
            '
            Me.ExtendControlProperty.SetHelpText(Me.txtSuggestedCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtSuggestedCategories.Location = New System.Drawing.Point(3, 326)
            Me.txtSuggestedCategories.Name = "txtSuggestedCategories"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtSuggestedCategories, System.Drawing.Color.Empty)
            Me.txtSuggestedCategories.Size = New System.Drawing.Size(182, 21)
            Me.txtSuggestedCategories.TabIndex = 35
            Me.ExtendControlProperty.SetTextDisplay(Me.txtSuggestedCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'btnGetSuggestedCategories
            '
            Me.ExtendControlProperty.SetHelpText(Me.btnGetSuggestedCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.btnGetSuggestedCategories.Location = New System.Drawing.Point(191, 325)
            Me.btnGetSuggestedCategories.Name = "btnGetSuggestedCategories"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnGetSuggestedCategories, System.Drawing.Color.Empty)
            Me.btnGetSuggestedCategories.Size = New System.Drawing.Size(90, 23)
            Me.btnGetSuggestedCategories.TabIndex = 34
            Me.btnGetSuggestedCategories.Text = "Get Suggested"
            Me.ExtendControlProperty.SetTextDisplay(Me.btnGetSuggestedCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.btnGetSuggestedCategories.ToolTip = "Click to get a list of suggested categories"
            '
            'cmdBrowseCategories
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmdBrowseCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmdBrowseCategories.Location = New System.Drawing.Point(5, 75)
            Me.cmdBrowseCategories.Name = "cmdBrowseCategories"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmdBrowseCategories, System.Drawing.Color.Empty)
            Me.cmdBrowseCategories.Size = New System.Drawing.Size(106, 23)
            Me.cmdBrowseCategories.TabIndex = 22
            Me.cmdBrowseCategories.Text = "Browse Categories"
            Me.ExtendControlProperty.SetTextDisplay(Me.cmdBrowseCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmdBrowseCategories.ToolTip = "Click to show the top level eBay categories"
            '
            'imgListeBayCategory
            '
            Me.imgListeBayCategory.Location = New System.Drawing.Point(3, 132)
            Me.imgListeBayCategory.Name = "imgListeBayCategory"
            Me.imgListeBayCategory.Size = New System.Drawing.Size(182, 188)
            Me.imgListeBayCategory.TabIndex = 4
            '
            'lblEbayStatus
            '
            Me.lblEbayStatus.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblEbayStatus, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblEbayStatus.Location = New System.Drawing.Point(294, 329)
            Me.lblEbayStatus.Name = "lblEbayStatus"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblEbayStatus, System.Drawing.Color.Empty)
            Me.lblEbayStatus.Size = New System.Drawing.Size(147, 13)
            Me.lblEbayStatus.TabIndex = 3
            Me.lblEbayStatus.Text = "Getting categories from eBay"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblEbayStatus, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblSelectCategory
            '
            Me.lblSelectCategory.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblSelectCategory, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblSelectCategory.Location = New System.Drawing.Point(117, 79)
            Me.lblSelectCategory.Name = "lblSelectCategory"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblSelectCategory, System.Drawing.Color.Empty)
            Me.lblSelectCategory.Size = New System.Drawing.Size(111, 13)
            Me.lblSelectCategory.TabIndex = 1
            Me.lblSelectCategory.Text = "Select eBay Category"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblSelectCategory, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TabPageAddPictures
            '
            Me.TabPageAddPictures.Controls.Add(Me.PictureBox3)
            Me.TabPageAddPictures.Controls.Add(Me.SimpleButton2)
            Me.TabPageAddPictures.Controls.Add(Me.PictureBox2)
            Me.TabPageAddPictures.Controls.Add(Me.SimpleButton1)
            Me.TabPageAddPictures.Controls.Add(Me.PictureBox1)
            Me.TabPageAddPictures.Controls.Add(Me.cmdAddPhoto)
            Me.TabPageAddPictures.Name = "TabPageAddPictures"
            Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageAddPictures, "")
            Me.TabPageAddPictures.Size = New System.Drawing.Size(690, 355)
            Me.TabPageAddPictures.Text = "Bring your item to life with pictures"
            '
            'PictureBox3
            '
            Me.PictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.PictureBox3.Location = New System.Drawing.Point(473, 68)
            Me.PictureBox3.Name = "PictureBox3"
            Me.PictureBox3.Size = New System.Drawing.Size(201, 194)
            Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox3.TabIndex = 5
            Me.PictureBox3.TabStop = False
            '
            'SimpleButton2
            '
            Me.ExtendControlProperty.SetHelpText(Me.SimpleButton2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.SimpleButton2.Location = New System.Drawing.Point(543, 279)
            Me.SimpleButton2.Name = "SimpleButton2"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.SimpleButton2, System.Drawing.Color.Empty)
            Me.SimpleButton2.Size = New System.Drawing.Size(75, 23)
            Me.SimpleButton2.TabIndex = 6
            Me.SimpleButton2.Text = "Add Photo"
            Me.ExtendControlProperty.SetTextDisplay(Me.SimpleButton2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'PictureBox2
            '
            Me.PictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.PictureBox2.Location = New System.Drawing.Point(248, 68)
            Me.PictureBox2.Name = "PictureBox2"
            Me.PictureBox2.Size = New System.Drawing.Size(201, 194)
            Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox2.TabIndex = 3
            Me.PictureBox2.TabStop = False
            '
            'SimpleButton1
            '
            Me.ExtendControlProperty.SetHelpText(Me.SimpleButton1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.SimpleButton1.Location = New System.Drawing.Point(318, 279)
            Me.SimpleButton1.Name = "SimpleButton1"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.SimpleButton1, System.Drawing.Color.Empty)
            Me.SimpleButton1.Size = New System.Drawing.Size(75, 23)
            Me.SimpleButton1.TabIndex = 4
            Me.SimpleButton1.Text = "Add Photo"
            Me.ExtendControlProperty.SetTextDisplay(Me.SimpleButton1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'PictureBox1
            '
            Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.PictureBox1.Location = New System.Drawing.Point(26, 69)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(201, 194)
            Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox1.TabIndex = 1
            Me.PictureBox1.TabStop = False
            '
            'cmdAddPhoto
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmdAddPhoto, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmdAddPhoto.Location = New System.Drawing.Point(96, 280)
            Me.cmdAddPhoto.Name = "cmdAddPhoto"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmdAddPhoto, System.Drawing.Color.Empty)
            Me.cmdAddPhoto.Size = New System.Drawing.Size(75, 23)
            Me.cmdAddPhoto.TabIndex = 2
            Me.cmdAddPhoto.Text = "Add Photo"
            Me.ExtendControlProperty.SetTextDisplay(Me.cmdAddPhoto, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TabPageDescribeItem
            '
            Me.TabPageDescribeItem.Controls.Add(Me.lblHTMLTemplateFilePath)
            Me.TabPageDescribeItem.Controls.Add(Me.cmdGetTemplate)
            Me.TabPageDescribeItem.Controls.Add(Me.MemoHTMLTemplate)
            Me.TabPageDescribeItem.Controls.Add(Me.lblHTMLTemplate)
            Me.TabPageDescribeItem.Name = "TabPageDescribeItem"
            Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageDescribeItem, "")
            Me.TabPageDescribeItem.Size = New System.Drawing.Size(690, 355)
            Me.TabPageDescribeItem.Text = "Describe the item you're selling"
            '
            'lblHTMLTemplateFilePath
            '
            Me.lblHTMLTemplateFilePath.Location = New System.Drawing.Point(49, 331)
            Me.lblHTMLTemplateFilePath.Name = "lblHTMLTemplateFilePath"
            Me.lblHTMLTemplateFilePath.Size = New System.Drawing.Size(117, 13)
            Me.lblHTMLTemplateFilePath.TabIndex = 24
            Me.lblHTMLTemplateFilePath.Text = "HTML Template File Path"
            '
            'cmdGetTemplate
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmdGetTemplate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmdGetTemplate.Location = New System.Drawing.Point(575, 324)
            Me.cmdGetTemplate.Name = "cmdGetTemplate"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmdGetTemplate, System.Drawing.Color.Empty)
            Me.cmdGetTemplate.Size = New System.Drawing.Size(75, 23)
            Me.cmdGetTemplate.TabIndex = 23
            Me.cmdGetTemplate.Text = "Template"
            Me.ExtendControlProperty.SetTextDisplay(Me.cmdGetTemplate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'MemoHTMLTemplate
            '
            Me.ExtendControlProperty.SetHelpText(Me.MemoHTMLTemplate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.MemoHTMLTemplate.Location = New System.Drawing.Point(49, 87)
            Me.MemoHTMLTemplate.Name = "MemoHTMLTemplate"
            Me.MemoHTMLTemplate.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.MemoHTMLTemplate.Properties.Appearance.Options.UseBackColor = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoHTMLTemplate, System.Drawing.Color.Empty)
            Me.MemoHTMLTemplate.Size = New System.Drawing.Size(601, 231)
            Me.MemoHTMLTemplate.TabIndex = 1
            Me.ExtendControlProperty.SetTextDisplay(Me.MemoHTMLTemplate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblHTMLTemplate
            '
            Me.lblHTMLTemplate.Location = New System.Drawing.Point(49, 68)
            Me.lblHTMLTemplate.Name = "lblHTMLTemplate"
            Me.lblHTMLTemplate.Size = New System.Drawing.Size(77, 13)
            Me.lblHTMLTemplate.TabIndex = 2
            Me.lblHTMLTemplate.Text = "HTML Template:"
            '
            'tabPagePricePostage
            '
            Me.tabPagePricePostage.Controls.Add(Me.lblReturnWithin)
            Me.tabPagePricePostage.Controls.Add(Me.cmbReturnWithin)
            Me.tabPagePricePostage.Controls.Add(Me.lblShippingCostsPaidBy)
            Me.tabPagePricePostage.Controls.Add(Me.cmbShippingCostsPaidBy)
            Me.tabPagePricePostage.Controls.Add(Me.lblRefundOption)
            Me.tabPagePricePostage.Controls.Add(Me.cmbRefundOptions)
            Me.tabPagePricePostage.Controls.Add(Me.lblReturnsPolicyText)
            Me.tabPagePricePostage.Controls.Add(Me.txtReturnsPolicyText)
            Me.tabPagePricePostage.Controls.Add(Me.lblReturnsPolicy)
            Me.tabPagePricePostage.Controls.Add(Me.cmbReturnsPolicy)
            Me.tabPagePricePostage.Controls.Add(Me.spnPercentOfInterpriseSellingPrice)
            Me.tabPagePricePostage.Controls.Add(Me.chkPercentOfInterpriseSellingPrice)
            Me.tabPagePricePostage.Controls.Add(Me.lblOrSellingPrice)
            Me.tabPagePricePostage.Controls.Add(Me.LabelControl1)
            Me.tabPagePricePostage.Controls.Add(Me.cmbDispatchTime)
            Me.tabPagePricePostage.Controls.Add(Me.GroupBox1)
            Me.tabPagePricePostage.Controls.Add(Me.cmbService)
            Me.tabPagePricePostage.Controls.Add(Me.cmbPostalDestination)
            Me.tabPagePricePostage.Controls.Add(Me.SimpleButton4)
            Me.tabPagePricePostage.Controls.Add(Me.txtPostageCostToBuyer)
            Me.tabPagePricePostage.Controls.Add(Me.lblPostageCostToBuyer)
            Me.tabPagePricePostage.Controls.Add(Me.lblService)
            Me.tabPagePricePostage.Controls.Add(Me.lblPostalDestination)
            Me.tabPagePricePostage.Controls.Add(Me.txtBuyItNowPrice)
            Me.tabPagePricePostage.Controls.Add(Me.lblToTheListing)
            Me.tabPagePricePostage.Controls.Add(Me.PictureBox4)
            Me.tabPagePricePostage.Controls.Add(Me.chkAddBuyItNow)
            Me.tabPagePricePostage.Controls.Add(Me.lblLastingFor)
            Me.tabPagePricePostage.Controls.Add(Me.lblStartAuction)
            Me.tabPagePricePostage.Controls.Add(Me.txtStartAuction)
            Me.tabPagePricePostage.Controls.Add(Me.lblCurrency3)
            Me.tabPagePricePostage.Controls.Add(Me.lblCurrency2)
            Me.tabPagePricePostage.Controls.Add(Me.lblCurrency1)
            Me.tabPagePricePostage.Controls.Add(Me.cmbLastingFor)
            Me.tabPagePricePostage.Name = "tabPagePricePostage"
            Me.PageDescriptionCollection1.SetPageDescription(Me.tabPagePricePostage, "")
            Me.tabPagePricePostage.Size = New System.Drawing.Size(690, 355)
            Me.tabPagePricePostage.Text = "Set a price and P&P Details"
            '
            'lblReturnWithin
            '
            Me.lblReturnWithin.Location = New System.Drawing.Point(179, 254)
            Me.lblReturnWithin.Name = "lblReturnWithin"
            Me.lblReturnWithin.Size = New System.Drawing.Size(71, 13)
            Me.lblReturnWithin.TabIndex = 46
            Me.lblReturnWithin.Text = "Returns Within"
            '
            'cmbReturnWithin
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbReturnWithin, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbReturnWithin.Location = New System.Drawing.Point(178, 268)
            Me.cmbReturnWithin.Name = "cmbReturnWithin"
            Me.cmbReturnWithin.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbReturnWithin.Properties.Appearance.Options.UseBackColor = True
            Me.cmbReturnWithin.Properties.AutoHeight = False
            Me.cmbReturnWithin.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.cmbReturnWithin.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbReturnWithin, System.Drawing.Color.Empty)
            Me.cmbReturnWithin.Size = New System.Drawing.Size(152, 22)
            Me.cmbReturnWithin.TabIndex = 47
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbReturnWithin, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblShippingCostsPaidBy
            '
            Me.lblShippingCostsPaidBy.Location = New System.Drawing.Point(348, 254)
            Me.lblShippingCostsPaidBy.Name = "lblShippingCostsPaidBy"
            Me.lblShippingCostsPaidBy.Size = New System.Drawing.Size(108, 13)
            Me.lblShippingCostsPaidBy.TabIndex = 45
            Me.lblShippingCostsPaidBy.Text = "Shipping Costs Paid By"
            '
            'cmbShippingCostsPaidBy
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbShippingCostsPaidBy, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbShippingCostsPaidBy.Location = New System.Drawing.Point(348, 268)
            Me.cmbShippingCostsPaidBy.Name = "cmbShippingCostsPaidBy"
            Me.cmbShippingCostsPaidBy.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbShippingCostsPaidBy.Properties.Appearance.Options.UseBackColor = True
            Me.cmbShippingCostsPaidBy.Properties.AutoHeight = False
            Me.cmbShippingCostsPaidBy.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.cmbShippingCostsPaidBy.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbShippingCostsPaidBy, System.Drawing.Color.Empty)
            Me.cmbShippingCostsPaidBy.Size = New System.Drawing.Size(152, 22)
            Me.cmbShippingCostsPaidBy.TabIndex = 44
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbShippingCostsPaidBy, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblRefundOption
            '
            Me.lblRefundOption.Location = New System.Drawing.Point(180, 291)
            Me.lblRefundOption.Name = "lblRefundOption"
            Me.lblRefundOption.Size = New System.Drawing.Size(70, 13)
            Me.lblRefundOption.TabIndex = 43
            Me.lblRefundOption.Text = "Refund Option"
            '
            'cmbRefundOptions
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbRefundOptions, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbRefundOptions.Location = New System.Drawing.Point(180, 306)
            Me.cmbRefundOptions.Name = "cmbRefundOptions"
            Me.cmbRefundOptions.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbRefundOptions.Properties.Appearance.Options.UseBackColor = True
            Me.cmbRefundOptions.Properties.AutoHeight = False
            Me.cmbRefundOptions.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.cmbRefundOptions.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbRefundOptions, System.Drawing.Color.Empty)
            Me.cmbRefundOptions.Size = New System.Drawing.Size(152, 22)
            Me.cmbRefundOptions.TabIndex = 42
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbRefundOptions, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblReturnsPolicyText
            '
            Me.lblReturnsPolicyText.Location = New System.Drawing.Point(3, 334)
            Me.lblReturnsPolicyText.Name = "lblReturnsPolicyText"
            Me.lblReturnsPolicyText.Size = New System.Drawing.Size(93, 13)
            Me.lblReturnsPolicyText.TabIndex = 41
            Me.lblReturnsPolicyText.Text = "Returns Policy Text"
            '
            'txtReturnsPolicyText
            '
            Me.txtReturnsPolicyText.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.txtReturnsPolicyText, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtReturnsPolicyText.Location = New System.Drawing.Point(102, 330)
            Me.txtReturnsPolicyText.Name = "txtReturnsPolicyText"
            Me.txtReturnsPolicyText.Properties.AutoHeight = False
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtReturnsPolicyText, System.Drawing.Color.Empty)
            Me.txtReturnsPolicyText.Size = New System.Drawing.Size(586, 22)
            Me.txtReturnsPolicyText.TabIndex = 40
            Me.ExtendControlProperty.SetTextDisplay(Me.txtReturnsPolicyText, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblReturnsPolicy
            '
            Me.lblReturnsPolicy.Location = New System.Drawing.Point(3, 291)
            Me.lblReturnsPolicy.Name = "lblReturnsPolicy"
            Me.lblReturnsPolicy.Size = New System.Drawing.Size(68, 13)
            Me.lblReturnsPolicy.TabIndex = 38
            Me.lblReturnsPolicy.Text = "Returns Policy"
            '
            'cmbReturnsPolicy
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbReturnsPolicy, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbReturnsPolicy.Location = New System.Drawing.Point(2, 306)
            Me.cmbReturnsPolicy.Name = "cmbReturnsPolicy"
            Me.cmbReturnsPolicy.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbReturnsPolicy.Properties.Appearance.Options.UseBackColor = True
            Me.cmbReturnsPolicy.Properties.AutoHeight = False
            Me.cmbReturnsPolicy.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.cmbReturnsPolicy.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbReturnsPolicy, System.Drawing.Color.Empty)
            Me.cmbReturnsPolicy.Size = New System.Drawing.Size(152, 22)
            Me.cmbReturnsPolicy.TabIndex = 39
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbReturnsPolicy, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'spnPercentOfInterpriseSellingPrice
            '
            Me.spnPercentOfInterpriseSellingPrice.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
            Me.spnPercentOfInterpriseSellingPrice.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.spnPercentOfInterpriseSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.spnPercentOfInterpriseSellingPrice.Location = New System.Drawing.Point(400, 68)
            Me.spnPercentOfInterpriseSellingPrice.Name = "spnPercentOfInterpriseSellingPrice"
            Me.spnPercentOfInterpriseSellingPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.spnPercentOfInterpriseSellingPrice.Properties.Appearance.Options.UseBackColor = True
            Me.spnPercentOfInterpriseSellingPrice.Properties.AutoHeight = False
            Me.spnPercentOfInterpriseSellingPrice.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.spnPercentOfInterpriseSellingPrice.Properties.Mask.EditMask = "P"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.spnPercentOfInterpriseSellingPrice, System.Drawing.Color.Empty)
            Me.spnPercentOfInterpriseSellingPrice.Size = New System.Drawing.Size(56, 22)
            Me.spnPercentOfInterpriseSellingPrice.TabIndex = 37
            Me.ExtendControlProperty.SetTextDisplay(Me.spnPercentOfInterpriseSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'chkPercentOfInterpriseSellingPrice
            '
            Me.ExtendControlProperty.SetHelpText(Me.chkPercentOfInterpriseSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.chkPercentOfInterpriseSellingPrice.Location = New System.Drawing.Point(239, 69)
            Me.chkPercentOfInterpriseSellingPrice.Name = "chkPercentOfInterpriseSellingPrice"
            Me.chkPercentOfInterpriseSellingPrice.Properties.AutoHeight = False
            Me.chkPercentOfInterpriseSellingPrice.Properties.Caption = "% Of Interprise Selling Price"
            Me.chkPercentOfInterpriseSellingPrice.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkPercentOfInterpriseSellingPrice, System.Drawing.Color.Empty)
            Me.chkPercentOfInterpriseSellingPrice.Size = New System.Drawing.Size(164, 22)
            Me.chkPercentOfInterpriseSellingPrice.TabIndex = 36
            Me.ExtendControlProperty.SetTextDisplay(Me.chkPercentOfInterpriseSellingPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblOrSellingPrice
            '
            Me.lblOrSellingPrice.Location = New System.Drawing.Point(226, 73)
            Me.lblOrSellingPrice.Name = "lblOrSellingPrice"
            Me.lblOrSellingPrice.Size = New System.Drawing.Size(12, 13)
            Me.lblOrSellingPrice.TabIndex = 35
            Me.lblOrSellingPrice.Text = "Or"
            '
            'LabelControl1
            '
            Me.LabelControl1.Location = New System.Drawing.Point(3, 254)
            Me.LabelControl1.Name = "LabelControl1"
            Me.LabelControl1.Size = New System.Drawing.Size(64, 13)
            Me.LabelControl1.TabIndex = 27
            Me.LabelControl1.Text = "Dispatch time"
            '
            'cmbDispatchTime
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbDispatchTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbDispatchTime.Location = New System.Drawing.Point(3, 268)
            Me.cmbDispatchTime.Name = "cmbDispatchTime"
            Me.cmbDispatchTime.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbDispatchTime.Properties.Appearance.Options.UseBackColor = True
            Me.cmbDispatchTime.Properties.AutoHeight = False
            Me.cmbDispatchTime.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.cmbDispatchTime.Properties.Sorted = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbDispatchTime, System.Drawing.Color.Empty)
            Me.cmbDispatchTime.Size = New System.Drawing.Size(152, 22)
            Me.cmbDispatchTime.TabIndex = 26
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbDispatchTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.TimeEditScheduledTime)
            Me.GroupBox1.Controls.Add(Me.DateEditScheduledTime)
            Me.GroupBox1.Controls.Add(Me.rdoStartTime)
            Me.GroupBox1.Location = New System.Drawing.Point(-7, 128)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(665, 75)
            Me.GroupBox1.TabIndex = 25
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Start Time"
            '
            'TimeEditScheduledTime
            '
            Me.TimeEditScheduledTime.EditValue = New Date(2011, 11, 8, 0, 0, 0, 0)
            Me.TimeEditScheduledTime.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.TimeEditScheduledTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TimeEditScheduledTime.Location = New System.Drawing.Point(288, 45)
            Me.TimeEditScheduledTime.Name = "TimeEditScheduledTime"
            Me.TimeEditScheduledTime.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.TimeEditScheduledTime.Properties.Appearance.Options.UseBackColor = True
            Me.TimeEditScheduledTime.Properties.AutoHeight = False
            Me.TimeEditScheduledTime.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TimeEditScheduledTime, System.Drawing.Color.Empty)
            Me.TimeEditScheduledTime.Size = New System.Drawing.Size(73, 22)
            Me.TimeEditScheduledTime.TabIndex = 18
            Me.ExtendControlProperty.SetTextDisplay(Me.TimeEditScheduledTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'DateEditScheduledTime
            '
            Me.DateEditScheduledTime.EditValue = Nothing
            Me.DateEditScheduledTime.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.DateEditScheduledTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.DateEditScheduledTime.Location = New System.Drawing.Point(171, 45)
            Me.DateEditScheduledTime.Name = "DateEditScheduledTime"
            Me.DateEditScheduledTime.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.DateEditScheduledTime.Properties.Appearance.Options.UseBackColor = True
            Me.DateEditScheduledTime.Properties.AutoHeight = False
            Me.DateEditScheduledTime.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.DateEditScheduledTime.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditScheduledTime, System.Drawing.Color.Empty)
            Me.DateEditScheduledTime.Size = New System.Drawing.Size(97, 22)
            Me.DateEditScheduledTime.TabIndex = 17
            Me.ExtendControlProperty.SetTextDisplay(Me.DateEditScheduledTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'rdoStartTime
            '
            Me.ExtendControlProperty.SetHelpText(Me.rdoStartTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.rdoStartTime.Location = New System.Drawing.Point(10, 17)
            Me.rdoStartTime.Name = "rdoStartTime"
            Me.rdoStartTime.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.rdoStartTime.Properties.Appearance.Options.UseBackColor = True
            Me.rdoStartTime.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.rdoStartTime.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Start listing immediately"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Schedule start time")})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rdoStartTime, System.Drawing.Color.Empty)
            Me.rdoStartTime.Size = New System.Drawing.Size(179, 56)
            Me.rdoStartTime.TabIndex = 16
            Me.ExtendControlProperty.SetTextDisplay(Me.rdoStartTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'cmbService
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbService, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbService.Location = New System.Drawing.Point(178, 227)
            Me.cmbService.Name = "cmbService"
            Me.cmbService.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbService.Properties.Appearance.Options.UseBackColor = True
            Me.cmbService.Properties.AutoHeight = False
            Me.cmbService.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbService, System.Drawing.Color.Empty)
            Me.cmbService.Size = New System.Drawing.Size(375, 22)
            Me.cmbService.TabIndex = 24
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbService, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'cmbPostalDestination
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbPostalDestination, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbPostalDestination.Location = New System.Drawing.Point(3, 227)
            Me.cmbPostalDestination.Name = "cmbPostalDestination"
            Me.cmbPostalDestination.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbPostalDestination.Properties.Appearance.Options.UseBackColor = True
            Me.cmbPostalDestination.Properties.AutoHeight = False
            Me.cmbPostalDestination.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbPostalDestination, System.Drawing.Color.Empty)
            Me.cmbPostalDestination.Size = New System.Drawing.Size(152, 22)
            Me.cmbPostalDestination.TabIndex = 23
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbPostalDestination, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'SimpleButton4
            '
            Me.ExtendControlProperty.SetHelpText(Me.SimpleButton4, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.SimpleButton4.Location = New System.Drawing.Point(571, 102)
            Me.SimpleButton4.Name = "SimpleButton4"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.SimpleButton4, System.Drawing.Color.Empty)
            Me.SimpleButton4.Size = New System.Drawing.Size(75, 23)
            Me.SimpleButton4.TabIndex = 22
            Me.SimpleButton4.Text = "SimpleButton4"
            Me.ExtendControlProperty.SetTextDisplay(Me.SimpleButton4, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.SimpleButton4.Visible = False
            '
            'txtPostageCostToBuyer
            '
            Me.ExtendControlProperty.SetFormatType(Me.txtPostageCostToBuyer, Interprise.Framework.Base.[Shared].[Enum].EnmFormatType.Price)
            Me.ExtendControlProperty.SetHelpText(Me.txtPostageCostToBuyer, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtPostageCostToBuyer.Location = New System.Drawing.Point(600, 224)
            Me.txtPostageCostToBuyer.Name = "txtPostageCostToBuyer"
            Me.txtPostageCostToBuyer.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.txtPostageCostToBuyer.Properties.Appearance.Options.UseBackColor = True
            Me.txtPostageCostToBuyer.Properties.AutoHeight = False
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtPostageCostToBuyer, System.Drawing.Color.Empty)
            Me.txtPostageCostToBuyer.Size = New System.Drawing.Size(83, 22)
            Me.txtPostageCostToBuyer.TabIndex = 20
            Me.ExtendControlProperty.SetTextDisplay(Me.txtPostageCostToBuyer, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblPostageCostToBuyer
            '
            Me.lblPostageCostToBuyer.Location = New System.Drawing.Point(582, 207)
            Me.lblPostageCostToBuyer.Name = "lblPostageCostToBuyer"
            Me.lblPostageCostToBuyer.Size = New System.Drawing.Size(106, 13)
            Me.lblPostageCostToBuyer.TabIndex = 19
            Me.lblPostageCostToBuyer.Text = "Postage cost to buyer"
            '
            'lblService
            '
            Me.lblService.Location = New System.Drawing.Point(179, 209)
            Me.lblService.Name = "lblService"
            Me.lblService.Size = New System.Drawing.Size(35, 13)
            Me.lblService.TabIndex = 18
            Me.lblService.Text = "Service"
            '
            'lblPostalDestination
            '
            Me.lblPostalDestination.Location = New System.Drawing.Point(3, 209)
            Me.lblPostalDestination.Name = "lblPostalDestination"
            Me.lblPostalDestination.Size = New System.Drawing.Size(85, 13)
            Me.lblPostalDestination.TabIndex = 13
            Me.lblPostalDestination.Text = "Postal destination"
            '
            'txtBuyItNowPrice
            '
            Me.txtBuyItNowPrice.Enabled = False
            Me.ExtendControlProperty.SetFormatType(Me.txtBuyItNowPrice, Interprise.Framework.Base.[Shared].[Enum].EnmFormatType.Price)
            Me.ExtendControlProperty.SetHelpText(Me.txtBuyItNowPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtBuyItNowPrice.Location = New System.Drawing.Point(232, 99)
            Me.txtBuyItNowPrice.Name = "txtBuyItNowPrice"
            Me.txtBuyItNowPrice.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.txtBuyItNowPrice.Properties.Appearance.Options.UseBackColor = True
            Me.txtBuyItNowPrice.Properties.AutoHeight = False
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtBuyItNowPrice, System.Drawing.Color.Empty)
            Me.txtBuyItNowPrice.Size = New System.Drawing.Size(100, 22)
            Me.txtBuyItNowPrice.TabIndex = 8
            Me.ExtendControlProperty.SetTextDisplay(Me.txtBuyItNowPrice, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblToTheListing
            '
            Me.lblToTheListing.Location = New System.Drawing.Point(149, 103)
            Me.lblToTheListing.Name = "lblToTheListing"
            Me.lblToTheListing.Size = New System.Drawing.Size(59, 13)
            Me.lblToTheListing.TabIndex = 7
            Me.lblToTheListing.Text = "to the listing"
            '
            'PictureBox4
            '
            Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
            Me.PictureBox4.Location = New System.Drawing.Point(54, 101)
            Me.PictureBox4.Name = "PictureBox4"
            Me.PictureBox4.Size = New System.Drawing.Size(89, 20)
            Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox4.TabIndex = 6
            Me.PictureBox4.TabStop = False
            '
            'chkAddBuyItNow
            '
            Me.ExtendControlProperty.SetHelpText(Me.chkAddBuyItNow, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.chkAddBuyItNow.Location = New System.Drawing.Point(1, 99)
            Me.chkAddBuyItNow.Name = "chkAddBuyItNow"
            Me.chkAddBuyItNow.Properties.AutoHeight = False
            Me.chkAddBuyItNow.Properties.Caption = "Add"
            Me.chkAddBuyItNow.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkAddBuyItNow, System.Drawing.Color.Empty)
            Me.chkAddBuyItNow.Size = New System.Drawing.Size(47, 22)
            Me.chkAddBuyItNow.TabIndex = 5
            Me.ExtendControlProperty.SetTextDisplay(Me.chkAddBuyItNow, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblLastingFor
            '
            Me.lblLastingFor.Location = New System.Drawing.Point(473, 72)
            Me.lblLastingFor.Name = "lblLastingFor"
            Me.lblLastingFor.Size = New System.Drawing.Size(80, 13)
            Me.lblLastingFor.TabIndex = 3
            Me.lblLastingFor.Text = "Auction Duration"
            '
            'lblStartAuction
            '
            Me.lblStartAuction.Location = New System.Drawing.Point(3, 73)
            Me.lblStartAuction.Name = "lblStartAuction"
            Me.lblStartAuction.Size = New System.Drawing.Size(114, 13)
            Me.lblStartAuction.TabIndex = 1
            Me.lblStartAuction.Text = "Start Auction Bidding At"
            '
            'txtStartAuction
            '
            Me.ExtendControlProperty.SetFormatType(Me.txtStartAuction, Interprise.Framework.Base.[Shared].[Enum].EnmFormatType.Price)
            Me.ExtendControlProperty.SetHelpText(Me.txtStartAuction, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtStartAuction.Location = New System.Drawing.Point(142, 69)
            Me.txtStartAuction.Name = "txtStartAuction"
            Me.txtStartAuction.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.txtStartAuction.Properties.Appearance.Options.UseBackColor = True
            Me.txtStartAuction.Properties.AutoHeight = False
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtStartAuction, System.Drawing.Color.Empty)
            Me.txtStartAuction.Size = New System.Drawing.Size(77, 22)
            Me.txtStartAuction.TabIndex = 2
            Me.ExtendControlProperty.SetTextDisplay(Me.txtStartAuction, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblCurrency3
            '
            Me.lblCurrency3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblCurrency3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblCurrency3.Location = New System.Drawing.Point(572, 229)
            Me.lblCurrency3.Name = "lblCurrency3"
            Me.lblCurrency3.Size = New System.Drawing.Size(24, 13)
            Me.lblCurrency3.TabIndex = 21
            Me.lblCurrency3.Text = "lblCurrency3"
            '
            'lblCurrency2
            '
            Me.lblCurrency2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblCurrency2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblCurrency2.Location = New System.Drawing.Point(206, 103)
            Me.lblCurrency2.Name = "lblCurrency2"
            Me.lblCurrency2.Size = New System.Drawing.Size(24, 13)
            Me.lblCurrency2.TabIndex = 16
            Me.lblCurrency2.Text = "lblCurrency2"
            '
            'lblCurrency1
            '
            Me.lblCurrency1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
            Me.lblCurrency1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            Me.lblCurrency1.Location = New System.Drawing.Point(119, 73)
            Me.lblCurrency1.Name = "lblCurrency1"
            Me.lblCurrency1.Size = New System.Drawing.Size(21, 13)
            Me.lblCurrency1.TabIndex = 15
            Me.lblCurrency1.Text = "lblCurrency1"
            '
            'cmbLastingFor
            '
            Me.ExtendControlProperty.SetHelpText(Me.cmbLastingFor, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.cmbLastingFor.Location = New System.Drawing.Point(558, 68)
            Me.cmbLastingFor.Name = "cmbLastingFor"
            Me.cmbLastingFor.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.cmbLastingFor.Properties.Appearance.Options.UseBackColor = True
            Me.cmbLastingFor.Properties.AutoHeight = False
            Me.cmbLastingFor.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.cmbLastingFor.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", Nothing, -1)})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cmbLastingFor, System.Drawing.Color.Empty)
            Me.cmbLastingFor.Size = New System.Drawing.Size(100, 22)
            Me.cmbLastingFor.TabIndex = 4
            Me.ExtendControlProperty.SetTextDisplay(Me.cmbLastingFor, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'tabPageHowPaid
            '
            Me.tabPageHowPaid.Controls.Add(Me.cmdPasteXML)
            Me.tabPageHowPaid.Controls.Add(Me.MemoEditPublishStatus)
            Me.tabPageHowPaid.Controls.Add(Me.cmdPublish)
            Me.tabPageHowPaid.Controls.Add(Me.Label3)
            Me.tabPageHowPaid.Controls.Add(Me.txtPaypalEmail)
            Me.tabPageHowPaid.Controls.Add(Me.PictureBox5)
            Me.tabPageHowPaid.Controls.Add(Me.lblAcceptPaymentWith)
            Me.tabPageHowPaid.Controls.Add(Me.lblHowPaid)
            Me.tabPageHowPaid.Controls.Add(Me.picTick)
            Me.tabPageHowPaid.Name = "tabPageHowPaid"
            Me.PageDescriptionCollection1.SetPageDescription(Me.tabPageHowPaid, "")
            Me.tabPageHowPaid.Size = New System.Drawing.Size(690, 355)
            Me.tabPageHowPaid.Text = "Decide how you'd like to be paid"
            '
            'cmdPasteXML
            '
            Me.cmdPasteXML.Location = New System.Drawing.Point(586, 265)
            Me.cmdPasteXML.Name = "cmdPasteXML"
            Me.cmdPasteXML.Size = New System.Drawing.Size(75, 23)
            Me.cmdPasteXML.TabIndex = 9
            Me.cmdPasteXML.Text = "Get XML"
            Me.cmdPasteXML.UseVisualStyleBackColor = True
            '
            'MemoEditPublishStatus
            '
            Me.MemoEditPublishStatus.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.MemoEditPublishStatus, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.MemoEditPublishStatus.Location = New System.Drawing.Point(35, 172)
            Me.MemoEditPublishStatus.Name = "MemoEditPublishStatus"
            Me.MemoEditPublishStatus.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.MemoEditPublishStatus.Properties.Appearance.Options.UseBackColor = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.MemoEditPublishStatus, System.Drawing.Color.Empty)
            Me.MemoEditPublishStatus.Size = New System.Drawing.Size(517, 159)
            Me.MemoEditPublishStatus.TabIndex = 8
            Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditPublishStatus, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'cmdPublish
            '
            Me.cmdPublish.Location = New System.Drawing.Point(586, 308)
            Me.cmdPublish.Name = "cmdPublish"
            Me.cmdPublish.Size = New System.Drawing.Size(75, 23)
            Me.cmdPublish.TabIndex = 7
            Me.cmdPublish.Text = "Publish"
            Me.cmdPublish.UseVisualStyleBackColor = True
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.Label3, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.Label3.Location = New System.Drawing.Point(558, 135)
            Me.Label3.Name = "Label3"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.Label3, System.Drawing.Color.Empty)
            Me.Label3.Size = New System.Drawing.Size(72, 13)
            Me.Label3.TabIndex = 6
            Me.Label3.Text = "email address"
            Me.ExtendControlProperty.SetTextDisplay(Me.Label3, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'txtPaypalEmail
            '
            Me.ExtendControlProperty.SetHelpText(Me.txtPaypalEmail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtPaypalEmail.Location = New System.Drawing.Point(227, 130)
            Me.txtPaypalEmail.Name = "txtPaypalEmail"
            Me.txtPaypalEmail.Properties.AutoHeight = False
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtPaypalEmail, System.Drawing.Color.Empty)
            Me.txtPaypalEmail.Size = New System.Drawing.Size(325, 22)
            Me.txtPaypalEmail.TabIndex = 5
            Me.ExtendControlProperty.SetTextDisplay(Me.txtPaypalEmail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'PictureBox5
            '
            Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
            Me.PictureBox5.Location = New System.Drawing.Point(158, 133)
            Me.PictureBox5.Name = "PictureBox5"
            Me.PictureBox5.Size = New System.Drawing.Size(63, 20)
            Me.PictureBox5.TabIndex = 4
            Me.PictureBox5.TabStop = False
            '
            'lblAcceptPaymentWith
            '
            Me.lblAcceptPaymentWith.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblAcceptPaymentWith, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblAcceptPaymentWith.Location = New System.Drawing.Point(49, 135)
            Me.lblAcceptPaymentWith.Name = "lblAcceptPaymentWith"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblAcceptPaymentWith, System.Drawing.Color.Empty)
            Me.lblAcceptPaymentWith.Size = New System.Drawing.Size(108, 13)
            Me.lblAcceptPaymentWith.TabIndex = 3
            Me.lblAcceptPaymentWith.Text = "Accept payment with"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblAcceptPaymentWith, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblHowPaid
            '
            Me.lblHowPaid.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblHowPaid, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblHowPaid.Location = New System.Drawing.Point(32, 95)
            Me.lblHowPaid.Name = "lblHowPaid"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblHowPaid, System.Drawing.Color.Empty)
            Me.lblHowPaid.Size = New System.Drawing.Size(160, 13)
            Me.lblHowPaid.TabIndex = 1
            Me.lblHowPaid.Text = "Decide how you'd like to be paid"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblHowPaid, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'picTick
            '
            Me.picTick.Image = CType(resources.GetObject("picTick.Image"), System.Drawing.Image)
            Me.picTick.Location = New System.Drawing.Point(35, 135)
            Me.picTick.Name = "picTick"
            Me.picTick.Size = New System.Drawing.Size(15, 18)
            Me.picTick.TabIndex = 2
            Me.picTick.TabStop = False
            '
            'tabPageStockQuantity
            '
            Me.tabPageStockQuantity.Controls.Add(Me.lblYourLocation)
            Me.tabPageStockQuantity.Controls.Add(Me.txtLocation)
            Me.tabPageStockQuantity.Controls.Add(Me.lblStockQuantity)
            Me.tabPageStockQuantity.Controls.Add(Me.spnPercentOfTotal)
            Me.tabPageStockQuantity.Controls.Add(Me.rdoAuctionQuantity)
            Me.tabPageStockQuantity.Name = "tabPageStockQuantity"
            Me.PageDescriptionCollection1.SetPageDescription(Me.tabPageStockQuantity, "")
            Me.tabPageStockQuantity.Size = New System.Drawing.Size(690, 355)
            Me.tabPageStockQuantity.Text = "1 eBay auction with stock quantity"
            '
            'lblYourLocation
            '
            Me.lblYourLocation.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblYourLocation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblYourLocation.Location = New System.Drawing.Point(20, 281)
            Me.lblYourLocation.Name = "lblYourLocation"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblYourLocation, System.Drawing.Color.Empty)
            Me.lblYourLocation.Size = New System.Drawing.Size(51, 13)
            Me.lblYourLocation.TabIndex = 39
            Me.lblYourLocation.Text = "Location:"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblYourLocation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'txtLocation
            '
            Me.ExtendControlProperty.SetHelpText(Me.txtLocation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.txtLocation.Location = New System.Drawing.Point(124, 279)
            Me.txtLocation.Name = "txtLocation"
            Me.txtLocation.Properties.AutoHeight = False
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.txtLocation, System.Drawing.Color.Empty)
            Me.txtLocation.Size = New System.Drawing.Size(325, 22)
            Me.txtLocation.TabIndex = 38
            Me.ExtendControlProperty.SetTextDisplay(Me.txtLocation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblStockQuantity
            '
            Me.lblStockQuantity.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblStockQuantity, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblStockQuantity.Location = New System.Drawing.Point(20, 158)
            Me.lblStockQuantity.Name = "lblStockQuantity"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblStockQuantity, System.Drawing.Color.Empty)
            Me.lblStockQuantity.Size = New System.Drawing.Size(82, 13)
            Me.lblStockQuantity.TabIndex = 37
            Me.lblStockQuantity.Text = "Stock Quantity:"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblStockQuantity, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'spnPercentOfTotal
            '
            Me.spnPercentOfTotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
            Me.spnPercentOfTotal.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.spnPercentOfTotal, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.spnPercentOfTotal.Location = New System.Drawing.Point(317, 190)
            Me.spnPercentOfTotal.Name = "spnPercentOfTotal"
            Me.spnPercentOfTotal.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.spnPercentOfTotal.Properties.Appearance.Options.UseBackColor = True
            Me.spnPercentOfTotal.Properties.AutoHeight = False
            Me.spnPercentOfTotal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.spnPercentOfTotal, System.Drawing.Color.Empty)
            Me.spnPercentOfTotal.Size = New System.Drawing.Size(59, 22)
            Me.spnPercentOfTotal.TabIndex = 33
            Me.ExtendControlProperty.SetTextDisplay(Me.spnPercentOfTotal, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'rdoAuctionQuantity
            '
            Me.ExtendControlProperty.SetHelpText(Me.rdoAuctionQuantity, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.rdoAuctionQuantity.Location = New System.Drawing.Point(124, 109)
            Me.rdoAuctionQuantity.Name = "rdoAuctionQuantity"
            Me.rdoAuctionQuantity.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.rdoAuctionQuantity.Properties.Appearance.Options.UseBackColor = True
            Me.rdoAuctionQuantity.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "None"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Fixed Value"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "% of Total")})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rdoAuctionQuantity, System.Drawing.Color.Empty)
            Me.rdoAuctionQuantity.Size = New System.Drawing.Size(179, 112)
            Me.rdoAuctionQuantity.TabIndex = 36
            Me.ExtendControlProperty.SetTextDisplay(Me.rdoAuctionQuantity, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'tabPageStockMultiple
            '
            Me.tabPageStockMultiple.Controls.Add(Me.Label4)
            Me.tabPageStockMultiple.Controls.Add(Me.spnMultipleAuctions)
            Me.tabPageStockMultiple.Controls.Add(Me.grpFrequencyNormal)
            Me.tabPageStockMultiple.Controls.Add(Me.grpSchedule)
            Me.tabPageStockMultiple.Name = "tabPageStockMultiple"
            Me.PageDescriptionCollection1.SetPageDescription(Me.tabPageStockMultiple, "")
            Me.tabPageStockMultiple.Size = New System.Drawing.Size(690, 355)
            Me.tabPageStockMultiple.Text = "Multiple eBay auctions for 1 SKU"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.Label4, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.Label4.Location = New System.Drawing.Point(26, 99)
            Me.Label4.Name = "Label4"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.Label4, System.Drawing.Color.Empty)
            Me.Label4.Size = New System.Drawing.Size(253, 13)
            Me.Label4.TabIndex = 39
            Me.Label4.Text = "Maximum Number of Simultaneous Auctions for SKU"
            Me.ExtendControlProperty.SetTextDisplay(Me.Label4, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'spnMultipleAuctions
            '
            Me.spnMultipleAuctions.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
            Me.ExtendControlProperty.SetHelpText(Me.spnMultipleAuctions, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.spnMultipleAuctions.Location = New System.Drawing.Point(288, 95)
            Me.spnMultipleAuctions.Name = "spnMultipleAuctions"
            Me.spnMultipleAuctions.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.spnMultipleAuctions.Properties.Appearance.Options.UseBackColor = True
            Me.spnMultipleAuctions.Properties.AutoHeight = False
            Me.spnMultipleAuctions.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.spnMultipleAuctions, System.Drawing.Color.Empty)
            Me.spnMultipleAuctions.Size = New System.Drawing.Size(72, 22)
            Me.spnMultipleAuctions.TabIndex = 38
            Me.ExtendControlProperty.SetTextDisplay(Me.spnMultipleAuctions, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'grpFrequencyNormal
            '
            Me.grpFrequencyNormal.Controls.Add(Me.rdoRepatAuctionFor)
            Me.grpFrequencyNormal.Controls.Add(Me.spnRepeatQuantity)
            Me.grpFrequencyNormal.Controls.Add(Me.chkRepeatAuction)
            Me.grpFrequencyNormal.Location = New System.Drawing.Point(16, 166)
            Me.grpFrequencyNormal.Name = "grpFrequencyNormal"
            Me.grpFrequencyNormal.Size = New System.Drawing.Size(665, 66)
            Me.grpFrequencyNormal.TabIndex = 37
            Me.grpFrequencyNormal.TabStop = False
            Me.grpFrequencyNormal.Text = "Frequency"
            '
            'rdoRepatAuctionFor
            '
            Me.rdoRepatAuctionFor.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.rdoRepatAuctionFor, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.rdoRepatAuctionFor.Location = New System.Drawing.Point(211, 19)
            Me.rdoRepatAuctionFor.Name = "rdoRepatAuctionFor"
            Me.rdoRepatAuctionFor.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.rdoRepatAuctionFor.Properties.Appearance.Options.UseBackColor = True
            Me.rdoRepatAuctionFor.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.rdoRepatAuctionFor.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Pieces"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Days")})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rdoRepatAuctionFor, System.Drawing.Color.Empty)
            Me.rdoRepatAuctionFor.Size = New System.Drawing.Size(136, 22)
            Me.rdoRepatAuctionFor.TabIndex = 31
            Me.ExtendControlProperty.SetTextDisplay(Me.rdoRepatAuctionFor, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'spnRepeatQuantity
            '
            Me.spnRepeatQuantity.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
            Me.spnRepeatQuantity.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.spnRepeatQuantity, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.spnRepeatQuantity.Location = New System.Drawing.Point(133, 19)
            Me.spnRepeatQuantity.Name = "spnRepeatQuantity"
            Me.spnRepeatQuantity.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.spnRepeatQuantity.Properties.Appearance.Options.UseBackColor = True
            Me.spnRepeatQuantity.Properties.AutoHeight = False
            Me.spnRepeatQuantity.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.spnRepeatQuantity, System.Drawing.Color.Empty)
            Me.spnRepeatQuantity.Size = New System.Drawing.Size(72, 22)
            Me.spnRepeatQuantity.TabIndex = 33
            Me.ExtendControlProperty.SetTextDisplay(Me.spnRepeatQuantity, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'chkRepeatAuction
            '
            Me.ExtendControlProperty.SetHelpText(Me.chkRepeatAuction, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.chkRepeatAuction.Location = New System.Drawing.Point(11, 20)
            Me.chkRepeatAuction.Name = "chkRepeatAuction"
            Me.chkRepeatAuction.Properties.AutoHeight = False
            Me.chkRepeatAuction.Properties.Caption = "Repeat auction for"
            Me.chkRepeatAuction.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkRepeatAuction, System.Drawing.Color.Empty)
            Me.chkRepeatAuction.Size = New System.Drawing.Size(116, 22)
            Me.chkRepeatAuction.TabIndex = 32
            Me.ExtendControlProperty.SetTextDisplay(Me.chkRepeatAuction, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'grpSchedule
            '
            Me.grpSchedule.Controls.Add(Me.TimeEditSpecifiedTime)
            Me.grpSchedule.Controls.Add(Me.rdoSchedule)
            Me.grpSchedule.Location = New System.Drawing.Point(16, 238)
            Me.grpSchedule.Name = "grpSchedule"
            Me.grpSchedule.Size = New System.Drawing.Size(665, 102)
            Me.grpSchedule.TabIndex = 36
            Me.grpSchedule.TabStop = False
            Me.grpSchedule.Text = "Schedule Next Auction"
            '
            'TimeEditSpecifiedTime
            '
            Me.TimeEditSpecifiedTime.EditValue = New Date(2011, 11, 8, 0, 0, 0, 0)
            Me.TimeEditSpecifiedTime.Enabled = False
            Me.ExtendControlProperty.SetHelpText(Me.TimeEditSpecifiedTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TimeEditSpecifiedTime.Location = New System.Drawing.Point(144, 50)
            Me.TimeEditSpecifiedTime.Name = "TimeEditSpecifiedTime"
            Me.TimeEditSpecifiedTime.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
            Me.TimeEditSpecifiedTime.Properties.Appearance.Options.UseBackColor = True
            Me.TimeEditSpecifiedTime.Properties.AutoHeight = False
            Me.TimeEditSpecifiedTime.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TimeEditSpecifiedTime, System.Drawing.Color.Empty)
            Me.TimeEditSpecifiedTime.Size = New System.Drawing.Size(73, 22)
            Me.TimeEditSpecifiedTime.TabIndex = 35
            Me.ExtendControlProperty.SetTextDisplay(Me.TimeEditSpecifiedTime, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'rdoSchedule
            '
            Me.ExtendControlProperty.SetHelpText(Me.rdoSchedule, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.rdoSchedule.Location = New System.Drawing.Point(6, 20)
            Me.rdoSchedule.Name = "rdoSchedule"
            Me.rdoSchedule.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.rdoSchedule.Properties.Appearance.Options.UseBackColor = True
            Me.rdoSchedule.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            Me.rdoSchedule.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Immediately after previous auction ends"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Specified Time")})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rdoSchedule, System.Drawing.Color.Empty)
            Me.rdoSchedule.Size = New System.Drawing.Size(223, 57)
            Me.rdoSchedule.TabIndex = 34
            Me.ExtendControlProperty.SetTextDisplay(Me.rdoSchedule, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'tabPageCatalogEnabledCategory1
            '
            Me.tabPageCatalogEnabledCategory1.Controls.Add(Me.chkFlagAsComplete1)
            Me.tabPageCatalogEnabledCategory1.Controls.Add(Me.lblCatalogEnabledXofY1)
            Me.tabPageCatalogEnabledCategory1.Name = "tabPageCatalogEnabledCategory1"
            Me.PageDescriptionCollection1.SetPageDescription(Me.tabPageCatalogEnabledCategory1, "")
            Me.tabPageCatalogEnabledCategory1.Size = New System.Drawing.Size(690, 355)
            Me.tabPageCatalogEnabledCategory1.Text = "tabPageCatalogEnabledCategory1"
            '
            'chkFlagAsComplete1
            '
            Me.ExtendControlProperty.SetHelpText(Me.chkFlagAsComplete1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.chkFlagAsComplete1.Location = New System.Drawing.Point(226, 204)
            Me.chkFlagAsComplete1.Name = "chkFlagAsComplete1"
            Me.chkFlagAsComplete1.Properties.AutoHeight = False
            Me.chkFlagAsComplete1.Properties.Caption = "Flag Category"
            Me.chkFlagAsComplete1.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkFlagAsComplete1, System.Drawing.Color.Empty)
            Me.chkFlagAsComplete1.Size = New System.Drawing.Size(97, 22)
            Me.chkFlagAsComplete1.TabIndex = 3
            Me.ExtendControlProperty.SetTextDisplay(Me.chkFlagAsComplete1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblCatalogEnabledXofY1
            '
            Me.lblCatalogEnabledXofY1.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblCatalogEnabledXofY1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblCatalogEnabledXofY1.Location = New System.Drawing.Point(40, 82)
            Me.lblCatalogEnabledXofY1.Name = "lblCatalogEnabledXofY1"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblCatalogEnabledXofY1, System.Drawing.Color.Empty)
            Me.lblCatalogEnabledXofY1.Size = New System.Drawing.Size(114, 13)
            Me.lblCatalogEnabledXofY1.TabIndex = 2
            Me.lblCatalogEnabledXofY1.Text = "lblCatalogEnabledXofY"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblCatalogEnabledXofY1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'tabPageCatalogEnabledCategory2
            '
            Me.tabPageCatalogEnabledCategory2.Controls.Add(Me.chkFlagAsComplete2)
            Me.tabPageCatalogEnabledCategory2.Controls.Add(Me.lblCatalogEnabledXofY2)
            Me.tabPageCatalogEnabledCategory2.Name = "tabPageCatalogEnabledCategory2"
            Me.PageDescriptionCollection1.SetPageDescription(Me.tabPageCatalogEnabledCategory2, "")
            Me.tabPageCatalogEnabledCategory2.Size = New System.Drawing.Size(690, 355)
            Me.tabPageCatalogEnabledCategory2.Text = "tabPageCatalogEnabledCategory2"
            '
            'TabPageShared1
            '
            Me.TabPageShared1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabPageShared1.ForeColor = System.Drawing.SystemColors.ControlText
            Me.TabPageShared1.Location = New System.Drawing.Point(1, 1)
            Me.TabPageShared1.Name = "TabPageShared1"
            Me.TabPageShared1.Size = New System.Drawing.Size(690, 355)
            Me.TabPageShared1.TabIndex = 5
            '
            'OpenFileDialog1
            '
            Me.OpenFileDialog1.FileName = "OpenFileDialog1"
            '
            'chkFlagAsComplete2
            '
            Me.ExtendControlProperty.SetHelpText(Me.chkFlagAsComplete2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.chkFlagAsComplete2.Location = New System.Drawing.Point(259, 208)
            Me.chkFlagAsComplete2.Name = "chkFlagAsComplete2"
            Me.chkFlagAsComplete2.Properties.AutoHeight = False
            Me.chkFlagAsComplete2.Properties.Caption = "Flag Category"
            Me.chkFlagAsComplete2.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkFlagAsComplete2, System.Drawing.Color.Empty)
            Me.chkFlagAsComplete2.Size = New System.Drawing.Size(97, 22)
            Me.chkFlagAsComplete2.TabIndex = 5
            Me.ExtendControlProperty.SetTextDisplay(Me.chkFlagAsComplete2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'lblCatalogEnabledXofY2
            '
            Me.lblCatalogEnabledXofY2.AutoSize = True
            Me.ExtendControlProperty.SetHelpText(Me.lblCatalogEnabledXofY2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.lblCatalogEnabledXofY2.Location = New System.Drawing.Point(73, 86)
            Me.lblCatalogEnabledXofY2.Name = "lblCatalogEnabledXofY2"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.lblCatalogEnabledXofY2, System.Drawing.Color.Empty)
            Me.lblCatalogEnabledXofY2.Size = New System.Drawing.Size(114, 13)
            Me.lblCatalogEnabledXofY2.TabIndex = 4
            Me.lblCatalogEnabledXofY2.Text = "lblCatalogEnabledXofY"
            Me.ExtendControlProperty.SetTextDisplay(Me.lblCatalogEnabledXofY2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'eBayPublishingWizardSection
            '
            Me.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.Appearance.Options.UseBackColor = True
            Me.Controls.Add(Me.eBayPublishingWizardPluginContainerControl)
            Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.Name = "eBayPublishingWizardSection"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
            Me.Size = New System.Drawing.Size(700, 417)
            Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.eBayPublishingWizardSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.eBayPublishingWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
            Me.eBayPublishingWizardPluginContainerControl.ResumeLayout(False)
            CType(Me.WizardControlEBayPub, System.ComponentModel.ISupportInitialize).EndInit()
            Me.WizardControlEBayPub.ResumeLayout(False)
            Me.TabPageWelcome.ResumeLayout(False)
            Me.TabPageWelcome.PerformLayout()
            CType(Me.txtDescriptiveTitle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.rdoAuctionType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPageSelectCategory.ResumeLayout(False)
            Me.TabPageSelectCategory.PerformLayout()
            CType(Me.chkAllowCategoryMapping.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbCondition.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditCategoryTreeeBay.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.imgListeBayCategoriesToAdd, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.imgListeBayCategory, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPageAddPictures.ResumeLayout(False)
            CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPageDescribeItem.ResumeLayout(False)
            Me.TabPageDescribeItem.PerformLayout()
            CType(Me.MemoHTMLTemplate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPagePricePostage.ResumeLayout(False)
            Me.tabPagePricePostage.PerformLayout()
            CType(Me.cmbReturnWithin.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbShippingCostsPaidBy.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbRefundOptions.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtReturnsPolicyText.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbReturnsPolicy.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.spnPercentOfInterpriseSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkPercentOfInterpriseSellingPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbDispatchTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox1.ResumeLayout(False)
            CType(Me.TimeEditScheduledTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditScheduledTime.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditScheduledTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.rdoStartTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbService.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbPostalDestination.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPostageCostToBuyer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtBuyItNowPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkAddBuyItNow.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtStartAuction.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cmbLastingFor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPageHowPaid.ResumeLayout(False)
            Me.tabPageHowPaid.PerformLayout()
            CType(Me.MemoEditPublishStatus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPaypalEmail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picTick, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPageStockQuantity.ResumeLayout(False)
            Me.tabPageStockQuantity.PerformLayout()
            CType(Me.txtLocation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.spnPercentOfTotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.rdoAuctionQuantity.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPageStockMultiple.ResumeLayout(False)
            Me.tabPageStockMultiple.PerformLayout()
            CType(Me.spnMultipleAuctions.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpFrequencyNormal.ResumeLayout(False)
            CType(Me.rdoRepatAuctionFor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.spnRepeatQuantity.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkRepeatAuction.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpSchedule.ResumeLayout(False)
            CType(Me.TimeEditSpecifiedTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.rdoSchedule.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPageCatalogEnabledCategory1.ResumeLayout(False)
            Me.tabPageCatalogEnabledCategory1.PerformLayout()
            CType(Me.chkFlagAsComplete1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPageCatalogEnabledCategory2.ResumeLayout(False)
            Me.tabPageCatalogEnabledCategory2.PerformLayout()
            CType(Me.chkFlagAsComplete2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend eBayPublishingWizardSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Friend WithEvents eBayPublishingWizardPluginContainerControl As Interprise.Presentation.Base.PluginContainerControl
        Friend WithEvents WizardControlEBayPub As Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl
        Friend WithEvents TabPageComplete As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents PageDescriptionCollection1 As Interprise.Presentation.Base.ExtendedXtraTabContol.PageDescriptionCollection
        Friend WithEvents TabPageWelcome As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents TabPageShared1 As Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared
        Friend WithEvents TabPageSelectCategory As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents lblSelectCategory As System.Windows.Forms.Label
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents TabPageAddPictures As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents cmdAddPhoto As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
        Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
        Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
        Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents TabPageDescribeItem As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents MemoHTMLTemplate As DevExpress.XtraEditors.MemoEdit
        Friend WithEvents lblHTMLTemplate As DevExpress.XtraEditors.LabelControl
        Friend WithEvents tabPagePricePostage As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents lblStartAuction As DevExpress.XtraEditors.LabelControl
        Friend WithEvents txtStartAuction As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblLastingFor As DevExpress.XtraEditors.LabelControl
        Friend WithEvents chkAddBuyItNow As DevExpress.XtraEditors.CheckEdit
        Friend WithEvents lblToTheListing As DevExpress.XtraEditors.LabelControl
        Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
        Friend WithEvents txtBuyItNowPrice As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblEbayStatus As System.Windows.Forms.Label
        Friend WithEvents lblPostalDestination As DevExpress.XtraEditors.LabelControl
        Friend WithEvents lblCurrency1 As DevExpress.XtraEditors.LabelControl
        Friend WithEvents lblCurrency2 As DevExpress.XtraEditors.LabelControl
        Friend WithEvents lblCurrency3 As DevExpress.XtraEditors.LabelControl
        Friend WithEvents txtPostageCostToBuyer As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblPostageCostToBuyer As DevExpress.XtraEditors.LabelControl
        Friend WithEvents lblService As DevExpress.XtraEditors.LabelControl
        Friend WithEvents tabPageHowPaid As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents lblHowPaid As System.Windows.Forms.Label
        Friend WithEvents picTick As System.Windows.Forms.PictureBox
        Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
        Friend WithEvents lblAcceptPaymentWith As System.Windows.Forms.Label
        Friend WithEvents txtPaypalEmail As DevExpress.XtraEditors.TextEdit 'System.Windows.Forms.TextBox
        Friend WithEvents imgListeBayCategory As DevExpress.XtraEditors.ImageListBoxControl
        Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents cmbPostalDestination As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents cmbService As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents TimeEditScheduledTime As DevExpress.XtraEditors.TimeEdit
        Friend WithEvents DateEditScheduledTime As DevExpress.XtraEditors.DateEdit
        Friend WithEvents rdoStartTime As DevExpress.XtraEditors.RadioGroup
        Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
        Friend WithEvents cmbDispatchTime As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents tabPageStockQuantity As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents tabPageStockMultiple As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents grpSchedule As System.Windows.Forms.GroupBox
        Friend WithEvents grpFrequencyNormal As System.Windows.Forms.GroupBox
        Friend WithEvents rdoRepatAuctionFor As DevExpress.XtraEditors.RadioGroup
        Friend WithEvents spnRepeatQuantity As DevExpress.XtraEditors.SpinEdit
        Friend WithEvents chkRepeatAuction As DevExpress.XtraEditors.CheckEdit
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents spnMultipleAuctions As DevExpress.XtraEditors.SpinEdit
        Friend WithEvents rdoSchedule As DevExpress.XtraEditors.RadioGroup
        Friend WithEvents TimeEditSpecifiedTime As DevExpress.XtraEditors.TimeEdit
        Friend WithEvents lblAuctionTypeDescription As DevExpress.XtraEditors.LabelControl
        Friend WithEvents cmdGetTemplate As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents lblStockQuantity As System.Windows.Forms.Label
        Friend WithEvents rdoAuctionQuantity As DevExpress.XtraEditors.RadioGroup
        Friend WithEvents lblAuctionType As System.Windows.Forms.Label
        Friend WithEvents rdoAuctionType As DevExpress.XtraEditors.RadioGroup
        Friend WithEvents imgListeBayCategoriesToAdd As DevExpress.XtraEditors.ImageListBoxControl
        Friend WithEvents btnRemoveCategory As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnAddCategory As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents txtSuggestedCategories As System.Windows.Forms.TextBox
        Friend WithEvents btnGetSuggestedCategories As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents cmdBrowseCategories As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents lblHTMLTemplateFilePath As DevExpress.XtraEditors.LabelControl
        Friend WithEvents spnPercentOfInterpriseSellingPrice As DevExpress.XtraEditors.SpinEdit
        Friend WithEvents chkPercentOfInterpriseSellingPrice As DevExpress.XtraEditors.CheckEdit
        Friend WithEvents lblOrSellingPrice As DevExpress.XtraEditors.LabelControl
        Friend WithEvents cmdPublish As System.Windows.Forms.Button
        Friend WithEvents spnPercentOfTotal As DevExpress.XtraEditors.SpinEdit
        Friend WithEvents btnBrowseNodeUpLeveleBay As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents TextEditCategoryTreeeBay As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblCondition As System.Windows.Forms.Label
        Friend WithEvents cmbCondition As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents chkAllowCategoryMapping As DevExpress.XtraEditors.CheckEdit
        Friend WithEvents MemoEditPublishStatus As DevExpress.XtraEditors.MemoEdit
        Friend WithEvents txtDescriptiveTitle As DevExpress.XtraEditors.TextEdit
        Friend WithEvents cmbLastingFor As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents lblReturnsPolicy As DevExpress.XtraEditors.LabelControl
        Friend WithEvents cmbReturnsPolicy As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents lblReturnsPolicyText As DevExpress.XtraEditors.LabelControl
        Friend WithEvents txtReturnsPolicyText As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblYourLocation As System.Windows.Forms.Label
        Friend WithEvents txtLocation As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblRefundOption As DevExpress.XtraEditors.LabelControl
        Friend WithEvents cmbRefundOptions As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents lblShippingCostsPaidBy As DevExpress.XtraEditors.LabelControl
        Friend WithEvents cmbShippingCostsPaidBy As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents cmdPasteXML As System.Windows.Forms.Button
        Friend WithEvents lblReturnWithin As DevExpress.XtraEditors.LabelControl
        Friend WithEvents cmbReturnWithin As DevExpress.XtraEditors.ImageComboBoxEdit
        Friend WithEvents tabPageCatalogEnabledCategory1 As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents lblCatalogEnabledXofY1 As System.Windows.Forms.Label
        Friend WithEvents chkFlagAsComplete1 As DevExpress.XtraEditors.CheckEdit
        Friend WithEvents tabPageCatalogEnabledCategory2 As DevExpress.XtraTab.XtraTabPage
        Friend WithEvents chkFlagAsComplete2 As DevExpress.XtraEditors.CheckEdit
        Friend WithEvents lblCatalogEnabledXofY2 As System.Windows.Forms.Label

#Region " Properties "
#Region " ImportExportDataset "
        Public ReadOnly Property ImportExportDataset() As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            Get
                Return Me.m_eBayPublishingWizardSection
            End Get
        End Property
#End Region
#End Region

    End Class
End Namespace
#End Region

