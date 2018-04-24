'===============================================================================
' Connected Business SDK
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

#Region " ActivationWizardSectionContainer "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ActivationWizardSectionContainer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ActivationWizardSectionContainer))
        Me.ActivationWizardSectionContainerGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
        Me.ActivationWizardPluginContainerControl = New Interprise.Presentation.Base.PluginContainerControl()
        Me.WizardControlActivation = New Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl()
        Me.TabPageComplete = New DevExpress.XtraTab.XtraTabPage()
        Me.LabelConfirmActivationDetails = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageWelcome = New DevExpress.XtraTab.XtraTabPage()
        Me.lblPluginsURL = New DevExpress.XtraEditors.LabelControl()
        Me.lblWelcomeFurtherDetails = New DevExpress.XtraEditors.LabelControl()
        Me.lblManualActivation = New DevExpress.XtraEditors.LabelControl()
        Me.btnUpdateActivationCode = New DevExpress.XtraEditors.SimpleButton()
        Me.txtManualActivation = New DevExpress.XtraEditors.TextEdit()
        Me.chkEnterManualCode = New DevExpress.XtraEditors.CheckEdit()
        Me.PictureBoxLerrynLogo = New System.Windows.Forms.PictureBox()
        Me.LabelActivationDetailsOrError = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageShared1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared()
        Me.TabPagePriceSummary = New DevExpress.XtraTab.XtraTabPage()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.lblPricingBasis = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageActivationCode = New DevExpress.XtraTab.XtraTabPage()
        Me.PanelControlActivations = New DevExpress.XtraEditors.PanelControl()
        Me.lbl3DCartCost = New DevExpress.XtraEditors.LabelControl()
        Me.cbe3DCartCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chk3DCart = New DevExpress.XtraEditors.CheckEdit()
        Me.lblProspectLeadCost = New DevExpress.XtraEditors.LabelControl()
        Me.chkProspectLead = New DevExpress.XtraEditors.CheckEdit()
        Me.lblFileImportCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblSelectConnectors3 = New DevExpress.XtraEditors.LabelControl()
        Me.chkFileImport = New DevExpress.XtraEditors.CheckEdit()
        Me.lblTurnoverBased = New DevExpress.XtraEditors.LabelControl()
        Me.lblAmazonFBACost = New DevExpress.XtraEditors.LabelControl()
        Me.cbeAmazonFBACount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkAmazonFBA = New DevExpress.XtraEditors.CheckEdit()
        Me.lblEBayCost = New DevExpress.XtraEditors.LabelControl()
        Me.cbeEBayCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkEBay = New DevExpress.XtraEditors.CheckEdit()
        Me.lblImportWizardQty1 = New DevExpress.XtraEditors.LabelControl()
        Me.cbeImportWizardQty1 = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lblImportWizardOnlyCost = New DevExpress.XtraEditors.LabelControl()
        Me.chkImportWizardOnly = New DevExpress.XtraEditors.CheckEdit()
        Me.lblCurrency = New DevExpress.XtraEditors.LabelControl()
        Me.lblTotalCost = New DevExpress.XtraEditors.LabelControl()
        Me.btnUpdateActivationCost = New DevExpress.XtraEditors.SimpleButton()
        Me.chkPayForXYears = New DevExpress.XtraEditors.CheckEdit()
        Me.chkPayMonthly = New DevExpress.XtraEditors.CheckEdit()
        Me.ImageComboBoxEditYears = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblShopComCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblChanlAdvCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblAmazonCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblVolusionCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblASPStorefrontCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblMagentoCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblSearsComCost = New DevExpress.XtraEditors.LabelControl()
        Me.lblCancelText = New DevExpress.XtraEditors.LabelControl()
        Me.chkCancel = New DevExpress.XtraEditors.CheckEdit()
        Me.cbeASPStorefrontCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbeVolusionCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbeAmazonCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbeChanAdvCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbeShopComCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbeMagentoCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbeSearsComCount = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkASPStorefront = New DevExpress.XtraEditors.CheckEdit()
        Me.chkShopCom = New DevExpress.XtraEditors.CheckEdit()
        Me.chkSearsCom = New DevExpress.XtraEditors.CheckEdit()
        Me.chkChanAdvisor = New DevExpress.XtraEditors.CheckEdit()
        Me.lblSelectConnectors2 = New DevExpress.XtraEditors.LabelControl()
        Me.chkAmazon = New DevExpress.XtraEditors.CheckEdit()
        Me.chkVolusion = New DevExpress.XtraEditors.CheckEdit()
        Me.chkMagento = New DevExpress.XtraEditors.CheckEdit()
        Me.lblSelectConnectors1 = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageEvaluationRestrictions = New DevExpress.XtraTab.XtraTabPage()
        Me.PanelControlInventoryImport = New DevExpress.XtraEditors.PanelControl()
        Me.lblImportWizardQty4 = New DevExpress.XtraEditors.LabelControl()
        Me.lblImportWizardQty3 = New DevExpress.XtraEditors.LabelControl()
        Me.lblImportWizardQty2 = New DevExpress.XtraEditors.LabelControl()
        Me.cbeImportWizardQty2 = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkPurchaseNow = New DevExpress.XtraEditors.CheckEdit()
        Me.lblInventoryImport = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageBilling = New DevExpress.XtraTab.XtraTabPage()
        Me.PanelControlBilling = New DevExpress.XtraEditors.PanelControl()
        Me.lblActivationEmail = New DevExpress.XtraEditors.LabelControl()
        Me.lblLoginDetails = New DevExpress.XtraEditors.LabelControl()
        Me.lblBillingCompany = New DevExpress.XtraEditors.LabelControl()
        Me.txtBillingCompany = New DevExpress.XtraEditors.TextEdit()
        Me.lblConfirmPassword = New DevExpress.XtraEditors.LabelControl()
        Me.txtConfirmPassword = New DevExpress.XtraEditors.TextEdit()
        Me.lblPassword = New DevExpress.XtraEditors.LabelControl()
        Me.txtPassword = New DevExpress.XtraEditors.TextEdit()
        Me.lblEmail = New DevExpress.XtraEditors.LabelControl()
        Me.txtEmail = New DevExpress.XtraEditors.TextEdit()
        Me.PhoneControlBilling = New Interprise.Presentation.Component.SharedControl.BasePhoneControl()
        Me.AddressControlBilling = New Interprise.Presentation.Base.Address.AddressControl()
        Me.lblBillingLastName = New DevExpress.XtraEditors.LabelControl()
        Me.txtBillingLastName = New DevExpress.XtraEditors.TextEdit()
        Me.lblBillingFirstName = New DevExpress.XtraEditors.LabelControl()
        Me.lblBillingSalutation = New DevExpress.XtraEditors.LabelControl()
        Me.txtBillingFirstName = New DevExpress.XtraEditors.TextEdit()
        Me.cbeBillingSalutation = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.TabPageDelivery = New DevExpress.XtraTab.XtraTabPage()
        Me.PanelControlDelivery = New DevExpress.XtraEditors.PanelControl()
        Me.lblShippingSalutation = New DevExpress.XtraEditors.LabelControl()
        Me.cbeShippingSalutation = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lblShippingCompany = New DevExpress.XtraEditors.LabelControl()
        Me.txtShippingCompany = New DevExpress.XtraEditors.TextEdit()
        Me.chkBillingIsDelivery = New DevExpress.XtraEditors.CheckEdit()
        Me.PhoneControlShipping = New Interprise.Presentation.Component.SharedControl.BasePhoneControl()
        Me.AddressControlShipping = New Interprise.Presentation.Base.Address.AddressControl()
        Me.lblShippingLastName = New DevExpress.XtraEditors.LabelControl()
        Me.txtShippingLastName = New DevExpress.XtraEditors.TextEdit()
        Me.lblShippingFirstName = New DevExpress.XtraEditors.LabelControl()
        Me.txtShippingFirstName = New DevExpress.XtraEditors.TextEdit()
        Me.TabPagePayment = New DevExpress.XtraTab.XtraTabPage()
        Me.PanelControlPayment = New DevExpress.XtraEditors.PanelControl()
        Me.lblErrorDetails = New DevExpress.XtraEditors.LabelControl()
        Me.lblRequestActivation = New DevExpress.XtraEditors.LabelControl()
        Me.PageDescriptionCollection1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.PageDescriptionCollection()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ActivationWizardSectionContainerGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ActivationWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ActivationWizardPluginContainerControl.SuspendLayout()
        CType(Me.WizardControlActivation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardControlActivation.SuspendLayout()
        Me.TabPageComplete.SuspendLayout()
        Me.TabPageWelcome.SuspendLayout()
        CType(Me.txtManualActivation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkEnterManualCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxLerrynLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPagePriceSummary.SuspendLayout()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        Me.TabPageActivationCode.SuspendLayout()
        CType(Me.PanelControlActivations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlActivations.SuspendLayout()
        CType(Me.cbe3DCartCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chk3DCart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkProspectLead.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkFileImport.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeAmazonFBACount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkAmazonFBA.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeEBayCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkEBay.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeImportWizardQty1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkImportWizardOnly.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkPayForXYears.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkPayMonthly.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageComboBoxEditYears.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkCancel.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeASPStorefrontCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeVolusionCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeAmazonCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeChanAdvCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeShopComCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeMagentoCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeSearsComCount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkASPStorefront.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkShopCom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkSearsCom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkChanAdvisor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkAmazon.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkVolusion.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkMagento.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageEvaluationRestrictions.SuspendLayout()
        CType(Me.PanelControlInventoryImport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlInventoryImport.SuspendLayout()
        CType(Me.cbeImportWizardQty2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkPurchaseNow.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageBilling.SuspendLayout()
        CType(Me.PanelControlBilling, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlBilling.SuspendLayout()
        CType(Me.txtBillingCompany.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtConfirmPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtEmail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBillingLastName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBillingFirstName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeBillingSalutation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageDelivery.SuspendLayout()
        CType(Me.PanelControlDelivery, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDelivery.SuspendLayout()
        CType(Me.cbeShippingSalutation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtShippingCompany.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkBillingIsDelivery.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtShippingLastName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtShippingFirstName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPagePayment.SuspendLayout()
        CType(Me.PanelControlPayment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlPayment.SuspendLayout()
        Me.SuspendLayout()
        '
        'ExtendControlProperty
        '
        Me.ExtendControlProperty.Enabled = False
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'ActivationWizardSectionContainerGateway
        '
        Me.ActivationWizardSectionContainerGateway.DataSetName = "ActivationWizardSectionContainerDataset"
        Me.ActivationWizardSectionContainerGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'ActivationWizardPluginContainerControl
        '
        Me.ActivationWizardPluginContainerControl.AppearanceCaption.Options.UseTextOptions = True
        Me.ActivationWizardPluginContainerControl.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ActivationWizardPluginContainerControl.BaseLayoutControl = Nothing
        Me.ActivationWizardPluginContainerControl.ContextMenuButtonCaption = Nothing
        Me.ActivationWizardPluginContainerControl.Controls.Add(Me.WizardControlActivation)
        Me.ActivationWizardPluginContainerControl.CurrentControl = Nothing
        Me.ActivationWizardPluginContainerControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ActivationWizardPluginContainerControl.EditorsHeight = 0
        Me.ActivationWizardPluginContainerControl.GroupContextMenu = Nothing
        Me.ActivationWizardPluginContainerControl.HelpCode = Nothing
        Me.ActivationWizardPluginContainerControl.IsCustomTab = False
        Me.ActivationWizardPluginContainerControl.LayoutMode = False
        Me.ActivationWizardPluginContainerControl.Location = New System.Drawing.Point(0, 0)
        Me.ActivationWizardPluginContainerControl.Name = "ActivationWizardPluginContainerControl"
        Me.ActivationWizardPluginContainerControl.OverrideUserRoleMode = False
        Me.ActivationWizardPluginContainerControl.PluginManagerButton = Nothing
        Me.ActivationWizardPluginContainerControl.PluginRows = Nothing
        Me.ActivationWizardPluginContainerControl.SearchPluginButton = Nothing
        Me.ActivationWizardPluginContainerControl.ShowCaption = False
        Me.ActivationWizardPluginContainerControl.Size = New System.Drawing.Size(861, 554)
        Me.ActivationWizardPluginContainerControl.TabIndex = 0
        '
        'WizardControlActivation
        '
        Me.WizardControlActivation.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.WizardControlActivation.Appearance.Options.UseBackColor = True
        Me.WizardControlActivation.AppearancePage.Header.Options.UseTextOptions = True
        Me.WizardControlActivation.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WizardControlActivation.AppearancePage.PageClient.BackColor = System.Drawing.SystemColors.Control
        Me.WizardControlActivation.AppearancePage.PageClient.Options.UseBackColor = True
        Me.WizardControlActivation.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.WizardControlActivation.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.WizardControlActivation.DisplayFinishButton = False
        Me.WizardControlActivation.DisplayNextButton = True
        Me.WizardControlActivation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WizardControlActivation.FinishMessage = "Finish message set dynamically"
        Me.WizardControlActivation.FinishPage = Me.TabPageComplete
        Me.WizardControlActivation.IsCustom = False
        Me.WizardControlActivation.IsPlugIn = True
        Me.WizardControlActivation.Location = New System.Drawing.Point(2, 2)
        Me.WizardControlActivation.Name = "WizardControlActivation"
        Me.WizardControlActivation.NextButtonText = "&Next"
        Me.WizardControlActivation.PageDescription = Nothing
        Me.WizardControlActivation.SelectedTabPage = Me.TabPageWelcome
        Me.WizardControlActivation.SharedPage = Me.TabPageShared1
        Me.WizardControlActivation.Size = New System.Drawing.Size(857, 550)
        Me.WizardControlActivation.TabIndex = 0
        Me.WizardControlActivation.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.TabPageWelcome, Me.TabPagePriceSummary, Me.TabPageActivationCode, Me.TabPageEvaluationRestrictions, Me.TabPageBilling, Me.TabPageDelivery, Me.TabPagePayment, Me.TabPageComplete})
        Me.WizardControlActivation.Title = "the eShopCONNECTED Activation"
        Me.WizardControlActivation.WelcomeMessage = "Error - Activation Wizard failed to initialise"
        Me.WizardControlActivation.WelcomePage = Me.TabPageWelcome
        '
        'TabPageComplete
        '
        Me.TabPageComplete.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageComplete.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageComplete.Controls.Add(Me.LabelConfirmActivationDetails)
        Me.TabPageComplete.Name = "TabPageComplete"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageComplete, "")
        Me.TabPageComplete.Size = New System.Drawing.Size(851, 492)
        Me.TabPageComplete.Text = "XtraTabPage2"
        '
        'LabelConfirmActivationDetails
        '
        Me.LabelConfirmActivationDetails.Location = New System.Drawing.Point(177, 113)
        Me.LabelConfirmActivationDetails.MaximumSize = New System.Drawing.Size(570, 50)
        Me.LabelConfirmActivationDetails.MinimumSize = New System.Drawing.Size(450, 22)
        Me.LabelConfirmActivationDetails.Name = "LabelConfirmActivationDetails"
        Me.LabelConfirmActivationDetails.Size = New System.Drawing.Size(450, 22)
        Me.LabelConfirmActivationDetails.TabIndex = 1
        '
        'TabPageWelcome
        '
        Me.TabPageWelcome.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageWelcome.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageWelcome.Controls.Add(Me.lblPluginsURL)
        Me.TabPageWelcome.Controls.Add(Me.lblWelcomeFurtherDetails)
        Me.TabPageWelcome.Controls.Add(Me.lblManualActivation)
        Me.TabPageWelcome.Controls.Add(Me.btnUpdateActivationCode)
        Me.TabPageWelcome.Controls.Add(Me.txtManualActivation)
        Me.TabPageWelcome.Controls.Add(Me.chkEnterManualCode)
        Me.TabPageWelcome.Controls.Add(Me.PictureBoxLerrynLogo)
        Me.TabPageWelcome.Controls.Add(Me.LabelActivationDetailsOrError)
        Me.TabPageWelcome.Name = "TabPageWelcome"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageWelcome, "")
        Me.TabPageWelcome.Size = New System.Drawing.Size(851, 492)
        Me.TabPageWelcome.Text = "XtraTabPage1"
        '
        'lblPluginsURL
        '
        Me.lblPluginsURL.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.lblPluginsURL.Location = New System.Drawing.Point(269, 383)
        Me.lblPluginsURL.Name = "lblPluginsURL"
        Me.lblPluginsURL.Size = New System.Drawing.Size(143, 13)
        Me.lblPluginsURL.TabIndex = 38
        Me.lblPluginsURL.Text = "www.pluginsforinterprise.com"
        '
        'lblWelcomeFurtherDetails
        '
        Me.lblWelcomeFurtherDetails.Location = New System.Drawing.Point(178, 344)
        Me.lblWelcomeFurtherDetails.Name = "lblWelcomeFurtherDetails"
        Me.lblWelcomeFurtherDetails.Size = New System.Drawing.Size(436, 52)
        Me.lblWelcomeFurtherDetails.TabIndex = 37
        Me.lblWelcomeFurtherDetails.Text = "For further details about eShopCONNECTED, please switch to the Help module, click" & _
    " on the" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "IS_PRODUCT_NAME Help icon and see the Working with eShopCONNECTED topic" & _
    ".  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Alternatively see "
        '
        'lblManualActivation
        '
        Me.lblManualActivation.Location = New System.Drawing.Point(179, 470)
        Me.lblManualActivation.Name = "lblManualActivation"
        Me.lblManualActivation.Size = New System.Drawing.Size(186, 13)
        Me.lblManualActivation.TabIndex = 36
        Me.lblManualActivation.Text = "Enter Activation Code and click Update"
        Me.lblManualActivation.Visible = False
        '
        'btnUpdateActivationCode
        '
        Me.btnUpdateActivationCode.Location = New System.Drawing.Point(482, 441)
        Me.btnUpdateActivationCode.Name = "btnUpdateActivationCode"
        Me.btnUpdateActivationCode.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdateActivationCode.TabIndex = 35
        Me.btnUpdateActivationCode.Text = "Update"
        Me.btnUpdateActivationCode.Visible = False
        '
        'txtManualActivation
        '
        Me.txtManualActivation.Location = New System.Drawing.Point(178, 443)
        Me.txtManualActivation.Name = "txtManualActivation"
        Me.txtManualActivation.Properties.Mask.EditMask = "AAAAA-AAAAA-AAAAA-AAAAA-AAAAA-AAAAA-AAAAA"
        Me.txtManualActivation.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple
        Me.txtManualActivation.Properties.MaxLength = 41
        Me.txtManualActivation.Size = New System.Drawing.Size(296, 20)
        Me.txtManualActivation.TabIndex = 34
        Me.txtManualActivation.Visible = False
        '
        'chkEnterManualCode
        '
        Me.chkEnterManualCode.Location = New System.Drawing.Point(175, 418)
        Me.chkEnterManualCode.Name = "chkEnterManualCode"
        Me.chkEnterManualCode.Properties.Caption = "I wish to enter an Activation Code emailed to me by Lerryn"
        Me.chkEnterManualCode.Size = New System.Drawing.Size(306, 19)
        Me.chkEnterManualCode.TabIndex = 33
        '
        'PictureBoxLerrynLogo
        '
        Me.PictureBoxLerrynLogo.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.lerrynlogo
        Me.PictureBoxLerrynLogo.Location = New System.Drawing.Point(673, 395)
        Me.PictureBoxLerrynLogo.Name = "PictureBoxLerrynLogo"
        Me.PictureBoxLerrynLogo.Size = New System.Drawing.Size(164, 54)
        Me.PictureBoxLerrynLogo.TabIndex = 32
        Me.PictureBoxLerrynLogo.TabStop = False
        '
        'LabelActivationDetailsOrError
        '
        Me.LabelActivationDetailsOrError.Appearance.BackColor = System.Drawing.Color.White
        Me.LabelActivationDetailsOrError.Appearance.ForeColor = System.Drawing.Color.Red
        Me.LabelActivationDetailsOrError.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.LabelActivationDetailsOrError.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
        Me.LabelActivationDetailsOrError.Location = New System.Drawing.Point(177, 162)
        Me.LabelActivationDetailsOrError.MinimumSize = New System.Drawing.Size(600, 110)
        Me.LabelActivationDetailsOrError.Name = "LabelActivationDetailsOrError"
        Me.LabelActivationDetailsOrError.Size = New System.Drawing.Size(600, 110)
        Me.LabelActivationDetailsOrError.TabIndex = 31
        Me.LabelActivationDetailsOrError.Visible = False
        '
        'TabPageShared1
        '
        Me.TabPageShared1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabPageShared1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TabPageShared1.Location = New System.Drawing.Point(0, 0)
        Me.TabPageShared1.Name = "TabPageShared1"
        Me.TabPageShared1.Size = New System.Drawing.Size(792, 440)
        Me.TabPageShared1.TabIndex = 5
        '
        'TabPagePriceSummary
        '
        Me.TabPagePriceSummary.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPagePriceSummary.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPagePriceSummary.Controls.Add(Me.PanelControl1)
        Me.TabPagePriceSummary.Name = "TabPagePriceSummary"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPagePriceSummary, "")
        Me.TabPagePriceSummary.Size = New System.Drawing.Size(851, 492)
        Me.TabPagePriceSummary.Text = "Activation Cost"
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.lblPricingBasis)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl1.Location = New System.Drawing.Point(0, 65)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(851, 427)
        Me.PanelControl1.TabIndex = 1
        '
        'lblPricingBasis
        '
        Me.lblPricingBasis.Location = New System.Drawing.Point(20, 19)
        Me.lblPricingBasis.Name = "lblPricingBasis"
        Me.lblPricingBasis.Size = New System.Drawing.Size(165, 13)
        Me.lblPricingBasis.TabIndex = 0
        Me.lblPricingBasis.Text = "Error - failed to obtain pricing data"
        '
        'TabPageActivationCode
        '
        Me.TabPageActivationCode.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageActivationCode.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageActivationCode.Controls.Add(Me.PanelControlActivations)
        Me.TabPageActivationCode.Name = "TabPageActivationCode"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageActivationCode, "")
        Me.TabPageActivationCode.Size = New System.Drawing.Size(851, 492)
        Me.TabPageActivationCode.Text = "Activation Options"
        '
        'PanelControlActivations
        '
        Me.PanelControlActivations.Controls.Add(Me.lbl3DCartCost)
        Me.PanelControlActivations.Controls.Add(Me.cbe3DCartCount)
        Me.PanelControlActivations.Controls.Add(Me.chk3DCart)
        Me.PanelControlActivations.Controls.Add(Me.lblProspectLeadCost)
        Me.PanelControlActivations.Controls.Add(Me.chkProspectLead)
        Me.PanelControlActivations.Controls.Add(Me.lblFileImportCost)
        Me.PanelControlActivations.Controls.Add(Me.lblSelectConnectors3)
        Me.PanelControlActivations.Controls.Add(Me.chkFileImport)
        Me.PanelControlActivations.Controls.Add(Me.lblTurnoverBased)
        Me.PanelControlActivations.Controls.Add(Me.lblAmazonFBACost)
        Me.PanelControlActivations.Controls.Add(Me.cbeAmazonFBACount)
        Me.PanelControlActivations.Controls.Add(Me.chkAmazonFBA)
        Me.PanelControlActivations.Controls.Add(Me.lblEBayCost)
        Me.PanelControlActivations.Controls.Add(Me.cbeEBayCount)
        Me.PanelControlActivations.Controls.Add(Me.chkEBay)
        Me.PanelControlActivations.Controls.Add(Me.lblImportWizardQty1)
        Me.PanelControlActivations.Controls.Add(Me.cbeImportWizardQty1)
        Me.PanelControlActivations.Controls.Add(Me.lblImportWizardOnlyCost)
        Me.PanelControlActivations.Controls.Add(Me.chkImportWizardOnly)
        Me.PanelControlActivations.Controls.Add(Me.lblCurrency)
        Me.PanelControlActivations.Controls.Add(Me.lblTotalCost)
        Me.PanelControlActivations.Controls.Add(Me.btnUpdateActivationCost)
        Me.PanelControlActivations.Controls.Add(Me.chkPayForXYears)
        Me.PanelControlActivations.Controls.Add(Me.chkPayMonthly)
        Me.PanelControlActivations.Controls.Add(Me.ImageComboBoxEditYears)
        Me.PanelControlActivations.Controls.Add(Me.lblShopComCost)
        Me.PanelControlActivations.Controls.Add(Me.lblChanlAdvCost)
        Me.PanelControlActivations.Controls.Add(Me.lblAmazonCost)
        Me.PanelControlActivations.Controls.Add(Me.lblVolusionCost)
        Me.PanelControlActivations.Controls.Add(Me.lblASPStorefrontCost)
        Me.PanelControlActivations.Controls.Add(Me.lblMagentoCost)
        Me.PanelControlActivations.Controls.Add(Me.lblSearsComCost)
        Me.PanelControlActivations.Controls.Add(Me.lblCancelText)
        Me.PanelControlActivations.Controls.Add(Me.chkCancel)
        Me.PanelControlActivations.Controls.Add(Me.cbeASPStorefrontCount)
        Me.PanelControlActivations.Controls.Add(Me.cbeVolusionCount)
        Me.PanelControlActivations.Controls.Add(Me.cbeAmazonCount)
        Me.PanelControlActivations.Controls.Add(Me.cbeChanAdvCount)
        Me.PanelControlActivations.Controls.Add(Me.cbeShopComCount)
        Me.PanelControlActivations.Controls.Add(Me.cbeMagentoCount)
        Me.PanelControlActivations.Controls.Add(Me.cbeSearsComCount)
        Me.PanelControlActivations.Controls.Add(Me.chkASPStorefront)
        Me.PanelControlActivations.Controls.Add(Me.chkShopCom)
        Me.PanelControlActivations.Controls.Add(Me.chkSearsCom)
        Me.PanelControlActivations.Controls.Add(Me.chkChanAdvisor)
        Me.PanelControlActivations.Controls.Add(Me.lblSelectConnectors2)
        Me.PanelControlActivations.Controls.Add(Me.chkAmazon)
        Me.PanelControlActivations.Controls.Add(Me.chkVolusion)
        Me.PanelControlActivations.Controls.Add(Me.chkMagento)
        Me.PanelControlActivations.Controls.Add(Me.lblSelectConnectors1)
        Me.PanelControlActivations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlActivations.Location = New System.Drawing.Point(0, 0)
        Me.PanelControlActivations.Name = "PanelControlActivations"
        Me.PanelControlActivations.Size = New System.Drawing.Size(851, 492)
        Me.PanelControlActivations.TabIndex = 1
        '
        'lbl3DCartCost
        '
        Me.lbl3DCartCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lbl3DCartCost.Location = New System.Drawing.Point(703, 25)
        Me.lbl3DCartCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lbl3DCartCost.Name = "lbl3DCartCost"
        Me.lbl3DCartCost.Size = New System.Drawing.Size(75, 13)
        Me.lbl3DCartCost.TabIndex = 58
        '
        'cbe3DCartCount
        '
        Me.cbe3DCartCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ThreeDCartCount", True))
        Me.cbe3DCartCount.Location = New System.Drawing.Point(431, 22)
        Me.cbe3DCartCount.Name = "cbe3DCartCount"
        Me.cbe3DCartCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbe3DCartCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbe3DCartCount.Size = New System.Drawing.Size(44, 20)
        Me.cbe3DCartCount.TabIndex = 57
        '
        'chk3DCart
        '
        Me.chk3DCart.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.Activate3DCart", True))
        Me.chk3DCart.Location = New System.Drawing.Point(35, 22)
        Me.chk3DCart.Name = "chk3DCart"
        Me.chk3DCart.Properties.AutoHeight = False
        Me.chk3DCart.Properties.Caption = "3DCart - please select the number of separate stores you wish to connect to"
        Me.chk3DCart.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chk3DCart.Size = New System.Drawing.Size(395, 22)
        Me.chk3DCart.TabIndex = 56
        '
        'lblProspectLeadCost
        '
        Me.lblProspectLeadCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblProspectLeadCost.Location = New System.Drawing.Point(703, 298)
        Me.lblProspectLeadCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblProspectLeadCost.Name = "lblProspectLeadCost"
        Me.lblProspectLeadCost.Size = New System.Drawing.Size(75, 13)
        Me.lblProspectLeadCost.TabIndex = 55
        '
        'chkProspectLead
        '
        Me.chkProspectLead.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateProspectLead", True))
        Me.chkProspectLead.Location = New System.Drawing.Point(35, 295)
        Me.chkProspectLead.Name = "chkProspectLead"
        Me.chkProspectLead.Properties.AutoHeight = False
        Me.chkProspectLead.Properties.Caption = "Prospect and Lead import - enables the importing of Prospects and Leads  in addit" & _
    "ion to Customers and Orders"
        Me.chkProspectLead.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkProspectLead.Size = New System.Drawing.Size(556, 22)
        Me.chkProspectLead.TabIndex = 53
        '
        'lblFileImportCost
        '
        Me.lblFileImportCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblFileImportCost.Location = New System.Drawing.Point(703, 278)
        Me.lblFileImportCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblFileImportCost.Name = "lblFileImportCost"
        Me.lblFileImportCost.Size = New System.Drawing.Size(75, 13)
        Me.lblFileImportCost.TabIndex = 52
        '
        'lblSelectConnectors3
        '
        Me.lblSelectConnectors3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.lblSelectConnectors3.LineLocation = DevExpress.XtraEditors.LineLocation.Top
        Me.lblSelectConnectors3.Location = New System.Drawing.Point(31, 259)
        Me.lblSelectConnectors3.MaximumSize = New System.Drawing.Size(600, 25)
        Me.lblSelectConnectors3.MinimumSize = New System.Drawing.Size(600, 13)
        Me.lblSelectConnectors3.Name = "lblSelectConnectors3"
        Me.lblSelectConnectors3.Size = New System.Drawing.Size(600, 13)
        Me.lblSelectConnectors3.TabIndex = 51
        Me.lblSelectConnectors3.Text = "eShopCONNECTED also some other useful connectors.  You can trial these eShopCONNE" & _
    "CTORS FREE for 1 month."
        '
        'chkFileImport
        '
        Me.chkFileImport.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateFileImport", True))
        Me.chkFileImport.Location = New System.Drawing.Point(35, 275)
        Me.chkFileImport.Name = "chkFileImport"
        Me.chkFileImport.Properties.AutoHeight = False
        Me.chkFileImport.Properties.Caption = "File Import connector - enables to importing of records from a nominated director" & _
    "y path"
        Me.chkFileImport.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkFileImport.Size = New System.Drawing.Size(452, 22)
        Me.chkFileImport.TabIndex = 49
        '
        'lblTurnoverBased
        '
        Me.lblTurnoverBased.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblTurnoverBased.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblTurnoverBased.Location = New System.Drawing.Point(51, 318)
        Me.lblTurnoverBased.MinimumSize = New System.Drawing.Size(727, 0)
        Me.lblTurnoverBased.Name = "lblTurnoverBased"
        Me.lblTurnoverBased.Size = New System.Drawing.Size(727, 13)
        Me.lblTurnoverBased.TabIndex = 48
        '
        'lblAmazonFBACost
        '
        Me.lblAmazonFBACost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblAmazonFBACost.Location = New System.Drawing.Point(703, 149)
        Me.lblAmazonFBACost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblAmazonFBACost.Name = "lblAmazonFBACost"
        Me.lblAmazonFBACost.Size = New System.Drawing.Size(75, 13)
        Me.lblAmazonFBACost.TabIndex = 47
        '
        'cbeAmazonFBACount
        '
        Me.cbeAmazonFBACount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.AmazonFBACount", True))
        Me.cbeAmazonFBACount.Enabled = False
        Me.cbeAmazonFBACount.Location = New System.Drawing.Point(456, 146)
        Me.cbeAmazonFBACount.Name = "cbeAmazonFBACount"
        Me.cbeAmazonFBACount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeAmazonFBACount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeAmazonFBACount.Size = New System.Drawing.Size(44, 20)
        Me.cbeAmazonFBACount.TabIndex = 46
        '
        'chkAmazonFBA
        '
        Me.chkAmazonFBA.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateAmazonFBA", True))
        Me.chkAmazonFBA.Enabled = False
        Me.chkAmazonFBA.Location = New System.Drawing.Point(35, 146)
        Me.chkAmazonFBA.Name = "chkAmazonFBA"
        Me.chkAmazonFBA.Properties.AutoHeight = False
        Me.chkAmazonFBA.Properties.Caption = "Amazon FBA - please select the number of Amazon Merchant IDs you wish to use"
        Me.chkAmazonFBA.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkAmazonFBA.Size = New System.Drawing.Size(418, 22)
        Me.chkAmazonFBA.TabIndex = 45
        '
        'lblEBayCost
        '
        Me.lblEBayCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblEBayCost.Location = New System.Drawing.Point(703, 189)
        Me.lblEBayCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblEBayCost.Name = "lblEBayCost"
        Me.lblEBayCost.Size = New System.Drawing.Size(75, 13)
        Me.lblEBayCost.TabIndex = 44
        '
        'cbeEBayCount
        '
        Me.cbeEBayCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.EBayCount", True))
        Me.cbeEBayCount.Location = New System.Drawing.Point(386, 186)
        Me.cbeEBayCount.Name = "cbeEBayCount"
        Me.cbeEBayCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeEBayCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeEBayCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeEBayCount.TabIndex = 43
        '
        'chkEBay
        '
        Me.chkEBay.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateEBay", True))
        Me.chkEBay.Location = New System.Drawing.Point(35, 186)
        Me.chkEBay.Name = "chkEBay"
        Me.chkEBay.Properties.AutoHeight = False
        Me.chkEBay.Properties.Caption = "eBay - please select the number of eBay Accounts you wish to use"
        Me.chkEBay.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkEBay.Size = New System.Drawing.Size(345, 22)
        Me.chkEBay.TabIndex = 42
        '
        'lblImportWizardQty1
        '
        Me.lblImportWizardQty1.Location = New System.Drawing.Point(396, 344)
        Me.lblImportWizardQty1.Name = "lblImportWizardQty1"
        Me.lblImportWizardQty1.Size = New System.Drawing.Size(228, 13)
        Me.lblImportWizardQty1.TabIndex = 41
        Me.lblImportWizardQty1.Text = "of my Inventory Items into Connected Business"
        '
        'cbeImportWizardQty1
        '
        Me.cbeImportWizardQty1.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ImportWizardQty", True))
        Me.cbeImportWizardQty1.Enabled = False
        Me.cbeImportWizardQty1.Location = New System.Drawing.Point(329, 341)
        Me.cbeImportWizardQty1.Name = "cbeImportWizardQty1"
        Me.cbeImportWizardQty1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeImportWizardQty1.Properties.Items.AddRange(New Object() {"250", "2500", "10000", "25000", "50000"})
        Me.cbeImportWizardQty1.Size = New System.Drawing.Size(59, 20)
        Me.cbeImportWizardQty1.TabIndex = 40
        '
        'lblImportWizardOnlyCost
        '
        Me.lblImportWizardOnlyCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblImportWizardOnlyCost.Location = New System.Drawing.Point(703, 344)
        Me.lblImportWizardOnlyCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblImportWizardOnlyCost.Name = "lblImportWizardOnlyCost"
        Me.lblImportWizardOnlyCost.Size = New System.Drawing.Size(75, 13)
        Me.lblImportWizardOnlyCost.TabIndex = 39
        '
        'chkImportWizardOnly
        '
        Me.chkImportWizardOnly.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ImportWizardOnly", True))
        Me.chkImportWizardOnly.Location = New System.Drawing.Point(29, 341)
        Me.chkImportWizardOnly.Name = "chkImportWizardOnly"
        Me.chkImportWizardOnly.Properties.Caption = "I only want to activate the Import Wizard to import up to "
        Me.chkImportWizardOnly.Size = New System.Drawing.Size(300, 19)
        Me.chkImportWizardOnly.TabIndex = 38
        '
        'lblCurrency
        '
        Me.lblCurrency.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblCurrency.Location = New System.Drawing.Point(653, 396)
        Me.lblCurrency.MinimumSize = New System.Drawing.Size(125, 0)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.Size = New System.Drawing.Size(125, 13)
        Me.lblCurrency.TabIndex = 37
        '
        'lblTotalCost
        '
        Me.lblTotalCost.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblTotalCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblTotalCost.Location = New System.Drawing.Point(578, 370)
        Me.lblTotalCost.MinimumSize = New System.Drawing.Size(200, 0)
        Me.lblTotalCost.Name = "lblTotalCost"
        Me.lblTotalCost.Size = New System.Drawing.Size(200, 13)
        Me.lblTotalCost.TabIndex = 36
        '
        'btnUpdateActivationCost
        '
        Me.btnUpdateActivationCost.Enabled = False
        Me.btnUpdateActivationCost.Location = New System.Drawing.Point(484, 365)
        Me.btnUpdateActivationCost.Name = "btnUpdateActivationCost"
        Me.btnUpdateActivationCost.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdateActivationCost.TabIndex = 35
        Me.btnUpdateActivationCost.Text = "Update"
        '
        'chkPayForXYears
        '
        Me.chkPayForXYears.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.PayForXYears", True))
        Me.chkPayForXYears.Location = New System.Drawing.Point(170, 368)
        Me.chkPayForXYears.Name = "chkPayForXYears"
        Me.chkPayForXYears.Properties.Caption = "I want to buy an Activation for "
        Me.chkPayForXYears.Size = New System.Drawing.Size(176, 19)
        Me.chkPayForXYears.TabIndex = 34
        '
        'chkPayMonthly
        '
        Me.chkPayMonthly.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.PayMonthly", True))
        Me.chkPayMonthly.Location = New System.Drawing.Point(29, 368)
        Me.chkPayMonthly.Name = "chkPayMonthly"
        Me.chkPayMonthly.Properties.Caption = "I want to pay Monthly"
        Me.chkPayMonthly.Size = New System.Drawing.Size(135, 19)
        Me.chkPayMonthly.TabIndex = 33
        '
        'ImageComboBoxEditYears
        '
        Me.ImageComboBoxEditYears.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.NoOfYears", True))
        Me.ImageComboBoxEditYears.Enabled = False
        Me.ImageComboBoxEditYears.Location = New System.Drawing.Point(341, 368)
        Me.ImageComboBoxEditYears.Name = "ImageComboBoxEditYears"
        Me.ImageComboBoxEditYears.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ImageComboBoxEditYears.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("1 Year", 1, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("2 Years", 2, -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("3 Years", 3, -1)})
        Me.ImageComboBoxEditYears.Size = New System.Drawing.Size(100, 20)
        Me.ImageComboBoxEditYears.TabIndex = 31
        '
        'lblShopComCost
        '
        Me.lblShopComCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblShopComCost.Location = New System.Drawing.Point(703, 229)
        Me.lblShopComCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblShopComCost.Name = "lblShopComCost"
        Me.lblShopComCost.Size = New System.Drawing.Size(75, 13)
        Me.lblShopComCost.TabIndex = 30
        '
        'lblChanlAdvCost
        '
        Me.lblChanlAdvCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblChanlAdvCost.Location = New System.Drawing.Point(703, 169)
        Me.lblChanlAdvCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblChanlAdvCost.Name = "lblChanlAdvCost"
        Me.lblChanlAdvCost.Size = New System.Drawing.Size(75, 13)
        Me.lblChanlAdvCost.TabIndex = 29
        '
        'lblAmazonCost
        '
        Me.lblAmazonCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblAmazonCost.Location = New System.Drawing.Point(703, 129)
        Me.lblAmazonCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblAmazonCost.Name = "lblAmazonCost"
        Me.lblAmazonCost.Size = New System.Drawing.Size(75, 13)
        Me.lblAmazonCost.TabIndex = 28
        '
        'lblVolusionCost
        '
        Me.lblVolusionCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblVolusionCost.Location = New System.Drawing.Point(703, 85)
        Me.lblVolusionCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblVolusionCost.Name = "lblVolusionCost"
        Me.lblVolusionCost.Size = New System.Drawing.Size(75, 13)
        Me.lblVolusionCost.TabIndex = 27
        '
        'lblASPStorefrontCost
        '
        Me.lblASPStorefrontCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblASPStorefrontCost.Location = New System.Drawing.Point(703, 45)
        Me.lblASPStorefrontCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblASPStorefrontCost.Name = "lblASPStorefrontCost"
        Me.lblASPStorefrontCost.Size = New System.Drawing.Size(75, 13)
        Me.lblASPStorefrontCost.TabIndex = 26
        '
        'lblMagentoCost
        '
        Me.lblMagentoCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblMagentoCost.Location = New System.Drawing.Point(703, 65)
        Me.lblMagentoCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblMagentoCost.Name = "lblMagentoCost"
        Me.lblMagentoCost.Size = New System.Drawing.Size(75, 13)
        Me.lblMagentoCost.TabIndex = 25
        '
        'lblSearsComCost
        '
        Me.lblSearsComCost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblSearsComCost.Location = New System.Drawing.Point(703, 209)
        Me.lblSearsComCost.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblSearsComCost.Name = "lblSearsComCost"
        Me.lblSearsComCost.Size = New System.Drawing.Size(75, 13)
        Me.lblSearsComCost.TabIndex = 30
        '
        'lblCancelText
        '
        Me.lblCancelText.Location = New System.Drawing.Point(49, 395)
        Me.lblCancelText.Name = "lblCancelText"
        Me.lblCancelText.Size = New System.Drawing.Size(457, 26)
        Me.lblCancelText.TabIndex = 24
        Me.lblCancelText.Text = "I wish to cancel my renewal and allow my activation for eShopCONNECTED to expire." & _
    "  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "I understand that I will no longer be able to use eShopCONNECTED once my act" & _
    "ivation expires."
        Me.lblCancelText.Visible = False
        '
        'chkCancel
        '
        Me.chkCancel.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.Cancel", True))
        Me.chkCancel.Location = New System.Drawing.Point(29, 399)
        Me.chkCancel.Name = "chkCancel"
        Me.chkCancel.Properties.Caption = ""
        Me.chkCancel.Size = New System.Drawing.Size(23, 19)
        Me.chkCancel.TabIndex = 13
        Me.chkCancel.Visible = False
        '
        'cbeASPStorefrontCount
        '
        Me.cbeASPStorefrontCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ASPStorefrontCount", True))
        Me.cbeASPStorefrontCount.Location = New System.Drawing.Point(514, 42)
        Me.cbeASPStorefrontCount.Name = "cbeASPStorefrontCount"
        Me.cbeASPStorefrontCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeASPStorefrontCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeASPStorefrontCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeASPStorefrontCount.TabIndex = 4
        '
        'cbeVolusionCount
        '
        Me.cbeVolusionCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.VolusionCount", True))
        Me.cbeVolusionCount.Location = New System.Drawing.Point(547, 82)
        Me.cbeVolusionCount.Name = "cbeVolusionCount"
        Me.cbeVolusionCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeVolusionCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeVolusionCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeVolusionCount.TabIndex = 6
        '
        'cbeAmazonCount
        '
        Me.cbeAmazonCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.AmazonCount", True))
        Me.cbeAmazonCount.Location = New System.Drawing.Point(434, 126)
        Me.cbeAmazonCount.Name = "cbeAmazonCount"
        Me.cbeAmazonCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeAmazonCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeAmazonCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeAmazonCount.TabIndex = 8
        '
        'cbeChanAdvCount
        '
        Me.cbeChanAdvCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ChanAdvCount", True))
        Me.cbeChanAdvCount.Location = New System.Drawing.Point(507, 166)
        Me.cbeChanAdvCount.Name = "cbeChanAdvCount"
        Me.cbeChanAdvCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeChanAdvCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeChanAdvCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeChanAdvCount.TabIndex = 10
        '
        'cbeShopComCount
        '
        Me.cbeShopComCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShopComCount", True))
        Me.cbeShopComCount.Location = New System.Drawing.Point(445, 226)
        Me.cbeShopComCount.Name = "cbeShopComCount"
        Me.cbeShopComCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeShopComCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeShopComCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeShopComCount.TabIndex = 12
        '
        'cbeMagentoCount
        '
        Me.cbeMagentoCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.MagentoCount", True))
        Me.cbeMagentoCount.Location = New System.Drawing.Point(521, 62)
        Me.cbeMagentoCount.Name = "cbeMagentoCount"
        Me.cbeMagentoCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeMagentoCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeMagentoCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeMagentoCount.TabIndex = 2
        '
        'cbeSearsComCount
        '
        Me.cbeSearsComCount.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.SearsComCount", True))
        Me.cbeSearsComCount.Location = New System.Drawing.Point(443, 206)
        Me.cbeSearsComCount.Name = "cbeSearsComCount"
        Me.cbeSearsComCount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeSearsComCount.Properties.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cbeSearsComCount.Size = New System.Drawing.Size(44, 20)
        Me.cbeSearsComCount.TabIndex = 12
        '
        'chkASPStorefront
        '
        Me.chkASPStorefront.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateASPStorefront", True))
        Me.chkASPStorefront.Location = New System.Drawing.Point(35, 42)
        Me.chkASPStorefront.Name = "chkASPStorefront"
        Me.chkASPStorefront.Properties.AutoHeight = False
        Me.chkASPStorefront.Properties.Caption = "ASPDotNetStorefront - please select the number of separate websites you wish to c" & _
    "onnect to"
        Me.chkASPStorefront.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkASPStorefront.Size = New System.Drawing.Size(489, 22)
        Me.chkASPStorefront.TabIndex = 3
        '
        'chkShopCom
        '
        Me.chkShopCom.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateShopCom", True))
        Me.chkShopCom.Location = New System.Drawing.Point(35, 226)
        Me.chkShopCom.Name = "chkShopCom"
        Me.chkShopCom.Properties.AutoHeight = False
        Me.chkShopCom.Properties.Caption = "Shop.com - please select the number of Shop.com Catalog IDs you wish to use"
        Me.chkShopCom.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkShopCom.Size = New System.Drawing.Size(412, 22)
        Me.chkShopCom.TabIndex = 11
        '
        'chkSearsCom
        '
        Me.chkSearsCom.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateSearsCom", True))
        Me.chkSearsCom.Location = New System.Drawing.Point(35, 206)
        Me.chkSearsCom.Name = "chkSearsCom"
        Me.chkSearsCom.Properties.AutoHeight = False
        Me.chkSearsCom.Properties.Caption = "Sears.com - please select the number of Sears.com Accounts you wish to use"
        Me.chkSearsCom.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkSearsCom.Size = New System.Drawing.Size(412, 22)
        Me.chkSearsCom.TabIndex = 11
        '
        'chkChanAdvisor
        '
        Me.chkChanAdvisor.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateChanAdv", True))
        Me.chkChanAdvisor.Location = New System.Drawing.Point(35, 166)
        Me.chkChanAdvisor.Name = "chkChanAdvisor"
        Me.chkChanAdvisor.Properties.AutoHeight = False
        Me.chkChanAdvisor.Properties.Caption = "Channel Advisor - please select the number of Channel Advisor Account IDs you wis" & _
    "h to use"
        Me.chkChanAdvisor.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkChanAdvisor.Size = New System.Drawing.Size(476, 22)
        Me.chkChanAdvisor.TabIndex = 9
        '
        'lblSelectConnectors2
        '
        Me.lblSelectConnectors2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.lblSelectConnectors2.LineLocation = DevExpress.XtraEditors.LineLocation.Top
        Me.lblSelectConnectors2.Location = New System.Drawing.Point(31, 110)
        Me.lblSelectConnectors2.MaximumSize = New System.Drawing.Size(600, 25)
        Me.lblSelectConnectors2.MinimumSize = New System.Drawing.Size(600, 13)
        Me.lblSelectConnectors2.Name = "lblSelectConnectors2"
        Me.lblSelectConnectors2.Size = New System.Drawing.Size(600, 13)
        Me.lblSelectConnectors2.TabIndex = 13
        Me.lblSelectConnectors2.Text = "eShopCONNECTED can also operate with other eMarketplaces.  You can trial these eS" & _
    "hopCONNECTORS FREE for 1 month."
        '
        'chkAmazon
        '
        Me.chkAmazon.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateAmazon", True))
        Me.chkAmazon.Location = New System.Drawing.Point(35, 126)
        Me.chkAmazon.Name = "chkAmazon"
        Me.chkAmazon.Properties.AutoHeight = False
        Me.chkAmazon.Properties.Caption = "Amazon - please select the number of Amazon Merchant IDs you wish to use"
        Me.chkAmazon.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkAmazon.Size = New System.Drawing.Size(404, 22)
        Me.chkAmazon.TabIndex = 7
        '
        'chkVolusion
        '
        Me.chkVolusion.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateVolusion", True))
        Me.chkVolusion.Location = New System.Drawing.Point(35, 82)
        Me.chkVolusion.Name = "chkVolusion"
        Me.chkVolusion.Properties.AutoHeight = False
        Me.chkVolusion.Properties.Caption = "Volusion (Order Import only) - please select the number of separate websites you " & _
    "wish to connect to"
        Me.chkVolusion.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkVolusion.Size = New System.Drawing.Size(513, 22)
        Me.chkVolusion.TabIndex = 5
        '
        'chkMagento
        '
        Me.chkMagento.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ActivateMagento", True))
        Me.chkMagento.Location = New System.Drawing.Point(35, 62)
        Me.chkMagento.Name = "chkMagento"
        Me.chkMagento.Properties.AutoHeight = False
        Me.chkMagento.Properties.Caption = "Magento - please select the number of separate Magento API Instances you wish to " & _
    "connect to"
        Me.chkMagento.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkMagento.Size = New System.Drawing.Size(489, 22)
        Me.chkMagento.TabIndex = 1
        '
        'lblSelectConnectors1
        '
        Me.lblSelectConnectors1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.lblSelectConnectors1.LineLocation = DevExpress.XtraEditors.LineLocation.Top
        Me.lblSelectConnectors1.Location = New System.Drawing.Point(31, 8)
        Me.lblSelectConnectors1.MaximumSize = New System.Drawing.Size(700, 25)
        Me.lblSelectConnectors1.MinimumSize = New System.Drawing.Size(600, 13)
        Me.lblSelectConnectors1.Name = "lblSelectConnectors1"
        Me.lblSelectConnectors1.Size = New System.Drawing.Size(600, 13)
        Me.lblSelectConnectors1.TabIndex = 8
        Me.lblSelectConnectors1.Text = "Please select the External Shopping Carts that you want to use with Interprise Su" & _
    "ite.  You can use these FREE for 1 month.  "
        '
        'TabPageEvaluationRestrictions
        '
        Me.TabPageEvaluationRestrictions.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageEvaluationRestrictions.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageEvaluationRestrictions.Controls.Add(Me.PanelControlInventoryImport)
        Me.TabPageEvaluationRestrictions.Name = "TabPageEvaluationRestrictions"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageEvaluationRestrictions, "")
        Me.TabPageEvaluationRestrictions.Size = New System.Drawing.Size(851, 492)
        Me.TabPageEvaluationRestrictions.Text = "Restrictions during Evaluation "
        '
        'PanelControlInventoryImport
        '
        Me.PanelControlInventoryImport.Controls.Add(Me.lblImportWizardQty4)
        Me.PanelControlInventoryImport.Controls.Add(Me.lblImportWizardQty3)
        Me.PanelControlInventoryImport.Controls.Add(Me.lblImportWizardQty2)
        Me.PanelControlInventoryImport.Controls.Add(Me.cbeImportWizardQty2)
        Me.PanelControlInventoryImport.Controls.Add(Me.chkPurchaseNow)
        Me.PanelControlInventoryImport.Controls.Add(Me.lblInventoryImport)
        Me.PanelControlInventoryImport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlInventoryImport.Location = New System.Drawing.Point(0, 0)
        Me.PanelControlInventoryImport.Name = "PanelControlInventoryImport"
        Me.PanelControlInventoryImport.Size = New System.Drawing.Size(851, 492)
        Me.PanelControlInventoryImport.TabIndex = 1
        '
        'lblImportWizardQty4
        '
        Me.lblImportWizardQty4.Location = New System.Drawing.Point(39, 229)
        Me.lblImportWizardQty4.Name = "lblImportWizardQty4"
        Me.lblImportWizardQty4.Size = New System.Drawing.Size(470, 13)
        Me.lblImportWizardQty4.TabIndex = 10
        Me.lblImportWizardQty4.Text = "Please contact Lerryn (Sales@lerryn.com) if you want to import more than 50000 In" & _
    "ventory Items"
        '
        'lblImportWizardQty3
        '
        Me.lblImportWizardQty3.Location = New System.Drawing.Point(268, 202)
        Me.lblImportWizardQty3.Name = "lblImportWizardQty3"
        Me.lblImportWizardQty3.Size = New System.Drawing.Size(198, 13)
        Me.lblImportWizardQty3.TabIndex = 9
        Me.lblImportWizardQty3.Text = "Inventory Items into Connected Business"
        '
        'lblImportWizardQty2
        '
        Me.lblImportWizardQty2.Location = New System.Drawing.Point(39, 202)
        Me.lblImportWizardQty2.Name = "lblImportWizardQty2"
        Me.lblImportWizardQty2.Size = New System.Drawing.Size(159, 13)
        Me.lblImportWizardQty2.TabIndex = 8
        Me.lblImportWizardQty2.Text = "I want to be able to import up to "
        '
        'cbeImportWizardQty2
        '
        Me.cbeImportWizardQty2.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ImportWizardQty", True))
        Me.cbeImportWizardQty2.Location = New System.Drawing.Point(202, 199)
        Me.cbeImportWizardQty2.Name = "cbeImportWizardQty2"
        Me.cbeImportWizardQty2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeImportWizardQty2.Properties.Items.AddRange(New Object() {"250", "2500", "10000", "25000", "50000"})
        Me.cbeImportWizardQty2.Size = New System.Drawing.Size(59, 20)
        Me.cbeImportWizardQty2.TabIndex = 7
        '
        'chkPurchaseNow
        '
        Me.chkPurchaseNow.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.PurchaseNow", True))
        Me.chkPurchaseNow.Location = New System.Drawing.Point(37, 174)
        Me.chkPurchaseNow.Name = "chkPurchaseNow"
        Me.chkPurchaseNow.Properties.Caption = "I have completed my trial of eShopCONNECTED and wish to purchase a full activatio" & _
    "n now."
        Me.chkPurchaseNow.Size = New System.Drawing.Size(447, 19)
        Me.chkPurchaseNow.TabIndex = 1
        '
        'lblInventoryImport
        '
        Me.lblInventoryImport.Location = New System.Drawing.Point(39, 32)
        Me.lblInventoryImport.Name = "lblInventoryImport"
        Me.lblInventoryImport.Size = New System.Drawing.Size(0, 13)
        Me.lblInventoryImport.TabIndex = 0
        '
        'TabPageBilling
        '
        Me.TabPageBilling.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageBilling.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageBilling.Controls.Add(Me.PanelControlBilling)
        Me.TabPageBilling.Name = "TabPageBilling"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageBilling, "")
        Me.TabPageBilling.Size = New System.Drawing.Size(851, 492)
        Me.TabPageBilling.Text = "Please enter your account and contact details"
        '
        'PanelControlBilling
        '
        Me.PanelControlBilling.Controls.Add(Me.lblActivationEmail)
        Me.PanelControlBilling.Controls.Add(Me.lblLoginDetails)
        Me.PanelControlBilling.Controls.Add(Me.lblBillingCompany)
        Me.PanelControlBilling.Controls.Add(Me.txtBillingCompany)
        Me.PanelControlBilling.Controls.Add(Me.lblConfirmPassword)
        Me.PanelControlBilling.Controls.Add(Me.txtConfirmPassword)
        Me.PanelControlBilling.Controls.Add(Me.lblPassword)
        Me.PanelControlBilling.Controls.Add(Me.txtPassword)
        Me.PanelControlBilling.Controls.Add(Me.lblEmail)
        Me.PanelControlBilling.Controls.Add(Me.txtEmail)
        Me.PanelControlBilling.Controls.Add(Me.PhoneControlBilling)
        Me.PanelControlBilling.Controls.Add(Me.AddressControlBilling)
        Me.PanelControlBilling.Controls.Add(Me.lblBillingLastName)
        Me.PanelControlBilling.Controls.Add(Me.txtBillingLastName)
        Me.PanelControlBilling.Controls.Add(Me.lblBillingFirstName)
        Me.PanelControlBilling.Controls.Add(Me.lblBillingSalutation)
        Me.PanelControlBilling.Controls.Add(Me.txtBillingFirstName)
        Me.PanelControlBilling.Controls.Add(Me.cbeBillingSalutation)
        Me.PanelControlBilling.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlBilling.Location = New System.Drawing.Point(0, 0)
        Me.PanelControlBilling.Name = "PanelControlBilling"
        Me.PanelControlBilling.Size = New System.Drawing.Size(851, 492)
        Me.PanelControlBilling.TabIndex = 1
        '
        'lblActivationEmail
        '
        Me.lblActivationEmail.Location = New System.Drawing.Point(453, 317)
        Me.lblActivationEmail.Name = "lblActivationEmail"
        Me.lblActivationEmail.Size = New System.Drawing.Size(268, 13)
        Me.lblActivationEmail.TabIndex = 23
        Me.lblActivationEmail.Text = "Your Activation Invoice will be sent to this email address"
        '
        'lblLoginDetails
        '
        Me.lblLoginDetails.Location = New System.Drawing.Point(560, 301)
        Me.lblLoginDetails.MaximumSize = New System.Drawing.Size(0, 65)
        Me.lblLoginDetails.Name = "lblLoginDetails"
        Me.lblLoginDetails.Size = New System.Drawing.Size(0, 13)
        Me.lblLoginDetails.TabIndex = 22
        '
        'lblBillingCompany
        '
        Me.lblBillingCompany.Location = New System.Drawing.Point(97, 94)
        Me.lblBillingCompany.Name = "lblBillingCompany"
        Me.lblBillingCompany.Size = New System.Drawing.Size(45, 13)
        Me.lblBillingCompany.TabIndex = 21
        Me.lblBillingCompany.Text = "Company"
        '
        'txtBillingCompany
        '
        Me.txtBillingCompany.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingCompanyName", True))
        Me.txtBillingCompany.Location = New System.Drawing.Point(197, 90)
        Me.txtBillingCompany.Name = "txtBillingCompany"
        Me.txtBillingCompany.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtBillingCompany.Properties.Appearance.Options.UseBackColor = True
        Me.txtBillingCompany.Properties.AutoHeight = False
        Me.txtBillingCompany.Size = New System.Drawing.Size(287, 22)
        Me.txtBillingCompany.TabIndex = 4
        '
        'lblConfirmPassword
        '
        Me.lblConfirmPassword.Location = New System.Drawing.Point(333, 345)
        Me.lblConfirmPassword.Name = "lblConfirmPassword"
        Me.lblConfirmPassword.Size = New System.Drawing.Size(86, 13)
        Me.lblConfirmPassword.TabIndex = 19
        Me.lblConfirmPassword.Text = "Confirm Password"
        Me.lblConfirmPassword.Visible = False
        '
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ConfirmPassword", True))
        Me.txtConfirmPassword.Location = New System.Drawing.Point(430, 341)
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtConfirmPassword.Properties.Appearance.Options.UseBackColor = True
        Me.txtConfirmPassword.Properties.AutoHeight = False
        Me.txtConfirmPassword.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmPassword.Size = New System.Drawing.Size(119, 22)
        Me.txtConfirmPassword.TabIndex = 9
        Me.txtConfirmPassword.Visible = False
        '
        'lblPassword
        '
        Me.lblPassword.Location = New System.Drawing.Point(97, 345)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(74, 13)
        Me.lblPassword.TabIndex = 17
        Me.lblPassword.Text = "Login Password"
        Me.lblPassword.Visible = False
        '
        'txtPassword
        '
        Me.txtPassword.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.Password", True))
        Me.txtPassword.Location = New System.Drawing.Point(197, 341)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtPassword.Properties.Appearance.Options.UseBackColor = True
        Me.txtPassword.Properties.AutoHeight = False
        Me.txtPassword.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(119, 22)
        Me.txtPassword.TabIndex = 8
        Me.txtPassword.Visible = False
        '
        'lblEmail
        '
        Me.lblEmail.Location = New System.Drawing.Point(97, 317)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(24, 13)
        Me.lblEmail.TabIndex = 15
        Me.lblEmail.Text = "Email"
        '
        'txtEmail
        '
        Me.txtEmail.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.Email", True))
        Me.txtEmail.Location = New System.Drawing.Point(197, 313)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtEmail.Properties.Appearance.Options.UseBackColor = True
        Me.txtEmail.Properties.AutoHeight = False
        Me.txtEmail.Size = New System.Drawing.Size(250, 22)
        Me.txtEmail.TabIndex = 7
        '
        'PhoneControlBilling
        '
        Me.PhoneControlBilling.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PhoneControlBilling.Appearance.Options.UseBackColor = True
        Me.PhoneControlBilling.AutoSize = True
        Me.PhoneControlBilling.ChildControls = New Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface(-1) {}
        Me.PhoneControlBilling.CurrentMenuAction = Nothing
        Me.PhoneControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("Country", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingCountry", True))
        Me.PhoneControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("Extension", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingPhoneExtension", True))
        Me.PhoneControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("Telephone", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingPhone", True))
        Me.PhoneControlBilling.DocumentCode = Nothing
        Me.PhoneControlBilling.EntityName = Nothing
        Me.PhoneControlBilling.IsDisposeCurrentFacadeAndDataset = True
        Me.PhoneControlBilling.IsFixedSize = False
        Me.PhoneControlBilling.IsReadOnly = False
        Me.PhoneControlBilling.IsWithSaveCounterIDField = False
        Me.PhoneControlBilling.LabelText = "Phone\Ext"
        Me.PhoneControlBilling.LabelWidth = 205.0!
        Me.PhoneControlBilling.Location = New System.Drawing.Point(94, 284)
        Me.PhoneControlBilling.m_grid = Nothing
        Me.PhoneControlBilling.m_VGrid = Nothing
        Me.PhoneControlBilling.Margin = New System.Windows.Forms.Padding(0)
        Me.PhoneControlBilling.MenuBase = Nothing
        Me.PhoneControlBilling.mnukeyDeleteAll = Nothing
        Me.PhoneControlBilling.mnukeyDeleteSelected = Nothing
        Me.PhoneControlBilling.mnukeyVDeleteAll = Nothing
        Me.PhoneControlBilling.mnukeyVDeleteSelected = Nothing
        Me.PhoneControlBilling.Name = "PhoneControlBilling"
        Me.PhoneControlBilling.RefreshFindDashboardDelegate = Nothing
        Me.PhoneControlBilling.Ribbon = Nothing
        Me.PhoneControlBilling.Size = New System.Drawing.Size(353, 22)
        Me.PhoneControlBilling.StickRibbonItems = False
        Me.PhoneControlBilling.TabIndex = 6
        '
        'AddressControlBilling
        '
        Me.AddressControlBilling.AddressFormat = Interprise.Framework.Base.[Shared].[Enum].AddressFormat.NewFormat
        Me.AddressControlBilling.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.AddressControlBilling.Appearance.Options.UseBackColor = True
        Me.AddressControlBilling.AutoSize = True
        Me.AddressControlBilling.ChildControls = New Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface(-1) {}
        Me.AddressControlBilling.CountryErrorText = ""
        Me.AddressControlBilling.CurrentMenuAction = Nothing
        Me.AddressControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("Address", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingAddress", True))
        Me.AddressControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("City", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingCity", True))
        Me.AddressControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("Country", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingCountry", True))
        Me.AddressControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("County", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingCounty", True))
        Me.AddressControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("PostalCode", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingPostalCode", True))
        Me.AddressControlBilling.DataBindings.Add(New System.Windows.Forms.Binding("State", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingState", True))
        Me.AddressControlBilling.DocumentCode = Nothing
        Me.AddressControlBilling.EntityName = Nothing
        Me.AddressControlBilling.IsDisposeCurrentFacadeAndDataset = True
        Me.AddressControlBilling.IsFixedSize = False
        Me.AddressControlBilling.IsReadOnly = False
        Me.AddressControlBilling.IsWithSaveCounterIDField = False
        Me.AddressControlBilling.LabelWidth = 130.0!
        Me.AddressControlBilling.Location = New System.Drawing.Point(93, 119)
        Me.AddressControlBilling.m_grid = Nothing
        Me.AddressControlBilling.m_VGrid = Nothing
        Me.AddressControlBilling.Margin = New System.Windows.Forms.Padding(0)
        Me.AddressControlBilling.MenuBase = Nothing
        Me.AddressControlBilling.mnukeyDeleteAll = Nothing
        Me.AddressControlBilling.mnukeyDeleteSelected = Nothing
        Me.AddressControlBilling.mnukeyVDeleteAll = Nothing
        Me.AddressControlBilling.mnukeyVDeleteSelected = Nothing
        Me.AddressControlBilling.Name = "AddressControlBilling"
        Me.AddressControlBilling.PostalCodeErrorText = ""
        Me.AddressControlBilling.PostalCodeWidth = 71
        Me.AddressControlBilling.RefreshFindDashboardDelegate = Nothing
        Me.AddressControlBilling.Ribbon = Nothing
        Me.AddressControlBilling.Size = New System.Drawing.Size(391, 158)
        Me.AddressControlBilling.StateWidth = 103
        Me.AddressControlBilling.StickRibbonItems = False
        Me.AddressControlBilling.TabIndex = 5
        '
        'lblBillingLastName
        '
        Me.lblBillingLastName.Location = New System.Drawing.Point(97, 66)
        Me.lblBillingLastName.Name = "lblBillingLastName"
        Me.lblBillingLastName.Size = New System.Drawing.Size(50, 13)
        Me.lblBillingLastName.TabIndex = 6
        Me.lblBillingLastName.Text = "Last Name"
        '
        'txtBillingLastName
        '
        Me.txtBillingLastName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingLastName", True))
        Me.txtBillingLastName.Location = New System.Drawing.Point(197, 62)
        Me.txtBillingLastName.Name = "txtBillingLastName"
        Me.txtBillingLastName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtBillingLastName.Properties.Appearance.Options.UseBackColor = True
        Me.txtBillingLastName.Properties.AutoHeight = False
        Me.txtBillingLastName.Size = New System.Drawing.Size(119, 22)
        Me.txtBillingLastName.TabIndex = 3
        '
        'lblBillingFirstName
        '
        Me.lblBillingFirstName.Location = New System.Drawing.Point(97, 38)
        Me.lblBillingFirstName.Name = "lblBillingFirstName"
        Me.lblBillingFirstName.Size = New System.Drawing.Size(51, 13)
        Me.lblBillingFirstName.TabIndex = 4
        Me.lblBillingFirstName.Text = "First Name"
        '
        'lblBillingSalutation
        '
        Me.lblBillingSalutation.Location = New System.Drawing.Point(97, 10)
        Me.lblBillingSalutation.Name = "lblBillingSalutation"
        Me.lblBillingSalutation.Size = New System.Drawing.Size(48, 13)
        Me.lblBillingSalutation.TabIndex = 2
        Me.lblBillingSalutation.Text = "Salutation"
        '
        'txtBillingFirstName
        '
        Me.txtBillingFirstName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingFirstName", True))
        Me.txtBillingFirstName.Location = New System.Drawing.Point(197, 34)
        Me.txtBillingFirstName.Name = "txtBillingFirstName"
        Me.txtBillingFirstName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtBillingFirstName.Properties.Appearance.Options.UseBackColor = True
        Me.txtBillingFirstName.Properties.AutoHeight = False
        Me.txtBillingFirstName.Size = New System.Drawing.Size(119, 22)
        Me.txtBillingFirstName.TabIndex = 2
        '
        'cbeBillingSalutation
        '
        Me.cbeBillingSalutation.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.BillingSalutation", True))
        Me.cbeBillingSalutation.Location = New System.Drawing.Point(197, 6)
        Me.cbeBillingSalutation.Name = "cbeBillingSalutation"
        Me.cbeBillingSalutation.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeBillingSalutation.Properties.Appearance.Options.UseBackColor = True
        Me.cbeBillingSalutation.Properties.AutoHeight = False
        Me.cbeBillingSalutation.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeBillingSalutation.Properties.Items.AddRange(New Object() {"Mr", "Mrs", "Ms", "Miss", "Sir"})
        Me.cbeBillingSalutation.Size = New System.Drawing.Size(100, 22)
        Me.cbeBillingSalutation.TabIndex = 1
        Me.cbeBillingSalutation.TabStop = False
        '
        'TabPageDelivery
        '
        Me.TabPageDelivery.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageDelivery.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageDelivery.Controls.Add(Me.PanelControlDelivery)
        Me.TabPageDelivery.Name = "TabPageDelivery"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageDelivery, "")
        Me.TabPageDelivery.Size = New System.Drawing.Size(851, 492)
        Me.TabPageDelivery.Text = "System Location Address"
        '
        'PanelControlDelivery
        '
        Me.PanelControlDelivery.Controls.Add(Me.lblShippingSalutation)
        Me.PanelControlDelivery.Controls.Add(Me.cbeShippingSalutation)
        Me.PanelControlDelivery.Controls.Add(Me.lblShippingCompany)
        Me.PanelControlDelivery.Controls.Add(Me.txtShippingCompany)
        Me.PanelControlDelivery.Controls.Add(Me.chkBillingIsDelivery)
        Me.PanelControlDelivery.Controls.Add(Me.PhoneControlShipping)
        Me.PanelControlDelivery.Controls.Add(Me.AddressControlShipping)
        Me.PanelControlDelivery.Controls.Add(Me.lblShippingLastName)
        Me.PanelControlDelivery.Controls.Add(Me.txtShippingLastName)
        Me.PanelControlDelivery.Controls.Add(Me.lblShippingFirstName)
        Me.PanelControlDelivery.Controls.Add(Me.txtShippingFirstName)
        Me.PanelControlDelivery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlDelivery.Location = New System.Drawing.Point(0, 0)
        Me.PanelControlDelivery.Name = "PanelControlDelivery"
        Me.PanelControlDelivery.Size = New System.Drawing.Size(851, 492)
        Me.PanelControlDelivery.TabIndex = 1
        '
        'lblShippingSalutation
        '
        Me.lblShippingSalutation.Location = New System.Drawing.Point(100, 51)
        Me.lblShippingSalutation.Name = "lblShippingSalutation"
        Me.lblShippingSalutation.Size = New System.Drawing.Size(48, 13)
        Me.lblShippingSalutation.TabIndex = 25
        Me.lblShippingSalutation.Text = "Salutation"
        '
        'cbeShippingSalutation
        '
        Me.cbeShippingSalutation.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingSalutation", True))
        Me.cbeShippingSalutation.Location = New System.Drawing.Point(200, 47)
        Me.cbeShippingSalutation.Name = "cbeShippingSalutation"
        Me.cbeShippingSalutation.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeShippingSalutation.Properties.Appearance.Options.UseBackColor = True
        Me.cbeShippingSalutation.Properties.AutoHeight = False
        Me.cbeShippingSalutation.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeShippingSalutation.Properties.Items.AddRange(New Object() {"Mr", "Mrs", "Ms", "Miss", "Sir"})
        Me.cbeShippingSalutation.Size = New System.Drawing.Size(100, 22)
        Me.cbeShippingSalutation.TabIndex = 2
        '
        'lblShippingCompany
        '
        Me.lblShippingCompany.Location = New System.Drawing.Point(100, 135)
        Me.lblShippingCompany.Name = "lblShippingCompany"
        Me.lblShippingCompany.Size = New System.Drawing.Size(45, 13)
        Me.lblShippingCompany.TabIndex = 23
        Me.lblShippingCompany.Text = "Company"
        '
        'txtShippingCompany
        '
        Me.txtShippingCompany.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingCompanyName", True))
        Me.txtShippingCompany.Location = New System.Drawing.Point(200, 131)
        Me.txtShippingCompany.Name = "txtShippingCompany"
        Me.txtShippingCompany.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtShippingCompany.Properties.Appearance.Options.UseBackColor = True
        Me.txtShippingCompany.Properties.AutoHeight = False
        Me.txtShippingCompany.Size = New System.Drawing.Size(287, 22)
        Me.txtShippingCompany.TabIndex = 5
        '
        'chkBillingIsDelivery
        '
        Me.chkBillingIsDelivery.Location = New System.Drawing.Point(97, 15)
        Me.chkBillingIsDelivery.Name = "chkBillingIsDelivery"
        Me.chkBillingIsDelivery.Properties.AutoHeight = False
        Me.chkBillingIsDelivery.Properties.Caption = "System Location Address is same as Billing Address"
        Me.chkBillingIsDelivery.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.chkBillingIsDelivery.Size = New System.Drawing.Size(278, 22)
        Me.chkBillingIsDelivery.TabIndex = 1
        '
        'PhoneControlShipping
        '
        Me.PhoneControlShipping.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PhoneControlShipping.Appearance.Options.UseBackColor = True
        Me.PhoneControlShipping.AutoSize = True
        Me.PhoneControlShipping.ChildControls = New Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface(-1) {}
        Me.PhoneControlShipping.CurrentMenuAction = Nothing
        Me.PhoneControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("Country", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingCountry", True))
        Me.PhoneControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("Extension", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingPhoneExtension", True))
        Me.PhoneControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("Telephone", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingPhone", True))
        Me.PhoneControlShipping.DocumentCode = Nothing
        Me.PhoneControlShipping.EntityName = Nothing
        Me.PhoneControlShipping.IsDisposeCurrentFacadeAndDataset = True
        Me.PhoneControlShipping.IsFixedSize = False
        Me.PhoneControlShipping.IsReadOnly = False
        Me.PhoneControlShipping.IsWithSaveCounterIDField = False
        Me.PhoneControlShipping.LabelText = "Phone\Ext"
        Me.PhoneControlShipping.LabelWidth = 100.0!
        Me.PhoneControlShipping.Location = New System.Drawing.Point(96, 324)
        Me.PhoneControlShipping.m_grid = Nothing
        Me.PhoneControlShipping.m_VGrid = Nothing
        Me.PhoneControlShipping.Margin = New System.Windows.Forms.Padding(0)
        Me.PhoneControlShipping.MenuBase = Nothing
        Me.PhoneControlShipping.mnukeyDeleteAll = Nothing
        Me.PhoneControlShipping.mnukeyDeleteSelected = Nothing
        Me.PhoneControlShipping.mnukeyVDeleteAll = Nothing
        Me.PhoneControlShipping.mnukeyVDeleteSelected = Nothing
        Me.PhoneControlShipping.Name = "PhoneControlShipping"
        Me.PhoneControlShipping.RefreshFindDashboardDelegate = Nothing
        Me.PhoneControlShipping.Ribbon = Nothing
        Me.PhoneControlShipping.Size = New System.Drawing.Size(259, 22)
        Me.PhoneControlShipping.StickRibbonItems = False
        Me.PhoneControlShipping.TabIndex = 7
        '
        'AddressControlShipping
        '
        Me.AddressControlShipping.AddressFormat = Interprise.Framework.Base.[Shared].[Enum].AddressFormat.NewFormat
        Me.AddressControlShipping.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.AddressControlShipping.Appearance.Options.UseBackColor = True
        Me.AddressControlShipping.AutoSize = True
        Me.AddressControlShipping.ChildControls = New Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface(-1) {}
        Me.AddressControlShipping.CountryErrorText = ""
        Me.AddressControlShipping.CurrentMenuAction = Nothing
        Me.AddressControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("Address", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingAddress", True))
        Me.AddressControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("City", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingCity", True))
        Me.AddressControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("Country", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingCountry", True))
        Me.AddressControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("County", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingCounty", True))
        Me.AddressControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("PostalCode", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingPostalCode", True))
        Me.AddressControlShipping.DataBindings.Add(New System.Windows.Forms.Binding("State", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingState", True))
        Me.AddressControlShipping.DocumentCode = Nothing
        Me.AddressControlShipping.EntityName = Nothing
        Me.AddressControlShipping.IsDisposeCurrentFacadeAndDataset = True
        Me.AddressControlShipping.IsFixedSize = False
        Me.AddressControlShipping.IsReadOnly = False
        Me.AddressControlShipping.IsWithSaveCounterIDField = False
        Me.AddressControlShipping.LabelWidth = 188.0!
        Me.AddressControlShipping.Location = New System.Drawing.Point(96, 160)
        Me.AddressControlShipping.m_grid = Nothing
        Me.AddressControlShipping.m_VGrid = Nothing
        Me.AddressControlShipping.Margin = New System.Windows.Forms.Padding(0)
        Me.AddressControlShipping.MenuBase = Nothing
        Me.AddressControlShipping.mnukeyDeleteAll = Nothing
        Me.AddressControlShipping.mnukeyDeleteSelected = Nothing
        Me.AddressControlShipping.mnukeyVDeleteAll = Nothing
        Me.AddressControlShipping.mnukeyVDeleteSelected = Nothing
        Me.AddressControlShipping.Name = "AddressControlShipping"
        Me.AddressControlShipping.PostalCodeErrorText = ""
        Me.AddressControlShipping.PostalCodeWidth = 71
        Me.AddressControlShipping.RefreshFindDashboardDelegate = Nothing
        Me.AddressControlShipping.Ribbon = Nothing
        Me.AddressControlShipping.Size = New System.Drawing.Size(382, 158)
        Me.AddressControlShipping.StateWidth = 99
        Me.AddressControlShipping.StickRibbonItems = False
        Me.AddressControlShipping.TabIndex = 6
        '
        'lblShippingLastName
        '
        Me.lblShippingLastName.Location = New System.Drawing.Point(99, 107)
        Me.lblShippingLastName.Name = "lblShippingLastName"
        Me.lblShippingLastName.Size = New System.Drawing.Size(50, 13)
        Me.lblShippingLastName.TabIndex = 17
        Me.lblShippingLastName.Text = "Last Name"
        '
        'txtShippingLastName
        '
        Me.txtShippingLastName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingLastName", True))
        Me.txtShippingLastName.Location = New System.Drawing.Point(200, 103)
        Me.txtShippingLastName.Name = "txtShippingLastName"
        Me.txtShippingLastName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtShippingLastName.Properties.Appearance.Options.UseBackColor = True
        Me.txtShippingLastName.Properties.AutoHeight = False
        Me.txtShippingLastName.Size = New System.Drawing.Size(119, 22)
        Me.txtShippingLastName.TabIndex = 4
        '
        'lblShippingFirstName
        '
        Me.lblShippingFirstName.Location = New System.Drawing.Point(99, 79)
        Me.lblShippingFirstName.Name = "lblShippingFirstName"
        Me.lblShippingFirstName.Size = New System.Drawing.Size(51, 13)
        Me.lblShippingFirstName.TabIndex = 15
        Me.lblShippingFirstName.Text = "First Name"
        '
        'txtShippingFirstName
        '
        Me.txtShippingFirstName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.ActivationWizardSectionContainerGateway, "ActivationAccountDetails.ShippingFirstName", True))
        Me.txtShippingFirstName.Location = New System.Drawing.Point(200, 75)
        Me.txtShippingFirstName.Name = "txtShippingFirstName"
        Me.txtShippingFirstName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.txtShippingFirstName.Properties.Appearance.Options.UseBackColor = True
        Me.txtShippingFirstName.Properties.AutoHeight = False
        Me.txtShippingFirstName.Size = New System.Drawing.Size(119, 22)
        Me.txtShippingFirstName.TabIndex = 3
        '
        'TabPagePayment
        '
        Me.TabPagePayment.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPagePayment.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPagePayment.Controls.Add(Me.PanelControlPayment)
        Me.TabPagePayment.Name = "TabPagePayment"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPagePayment, "")
        Me.TabPagePayment.Size = New System.Drawing.Size(851, 492)
        Me.TabPagePayment.Text = "Payment Details"
        '
        'PanelControlPayment
        '
        Me.PanelControlPayment.Controls.Add(Me.lblErrorDetails)
        Me.PanelControlPayment.Controls.Add(Me.lblRequestActivation)
        Me.PanelControlPayment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlPayment.Location = New System.Drawing.Point(0, 0)
        Me.PanelControlPayment.Name = "PanelControlPayment"
        Me.PanelControlPayment.Size = New System.Drawing.Size(851, 492)
        Me.PanelControlPayment.TabIndex = 32
        '
        'lblErrorDetails
        '
        Me.lblErrorDetails.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.lblErrorDetails.Location = New System.Drawing.Point(24, 70)
        Me.lblErrorDetails.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblErrorDetails.Name = "lblErrorDetails"
        Me.lblErrorDetails.Size = New System.Drawing.Size(75, 13)
        Me.lblErrorDetails.TabIndex = 28
        '
        'lblRequestActivation
        '
        Me.lblRequestActivation.Location = New System.Drawing.Point(24, 20)
        Me.lblRequestActivation.MinimumSize = New System.Drawing.Size(75, 0)
        Me.lblRequestActivation.Name = "lblRequestActivation"
        Me.lblRequestActivation.Size = New System.Drawing.Size(525, 26)
        Me.lblRequestActivation.TabIndex = 27
        Me.lblRequestActivation.Text = "Click Next to initiate the Activation process which will cause a PayPal invoice t" & _
    "o be sent to your email address.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Once this invoice is paid, Activation Code(s" & _
    ") will be issued automatically"
        '
        'ActivationWizardSectionContainer
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.ActivationWizardPluginContainerControl)
        Me.FindSearch = Interprise.Framework.Base.[Shared].[Enum].FindSearch.None
        Me.Name = "ActivationWizardSectionContainer"
        Me.Size = New System.Drawing.Size(861, 554)
        Me.TransactionType = Interprise.Framework.Base.[Shared].[Enum].TransactionType.None
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ActivationWizardSectionContainerGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ActivationWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ActivationWizardPluginContainerControl.ResumeLayout(False)
        CType(Me.WizardControlActivation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardControlActivation.ResumeLayout(False)
        Me.TabPageComplete.ResumeLayout(False)
        Me.TabPageComplete.PerformLayout()
        Me.TabPageWelcome.ResumeLayout(False)
        Me.TabPageWelcome.PerformLayout()
        CType(Me.txtManualActivation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkEnterManualCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxLerrynLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPagePriceSummary.ResumeLayout(False)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        Me.TabPageActivationCode.ResumeLayout(False)
        CType(Me.PanelControlActivations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlActivations.ResumeLayout(False)
        Me.PanelControlActivations.PerformLayout()
        CType(Me.cbe3DCartCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chk3DCart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkProspectLead.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkFileImport.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeAmazonFBACount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkAmazonFBA.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeEBayCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkEBay.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeImportWizardQty1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkImportWizardOnly.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkPayForXYears.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkPayMonthly.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageComboBoxEditYears.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkCancel.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeASPStorefrontCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeVolusionCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeAmazonCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeChanAdvCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeShopComCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeMagentoCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeSearsComCount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkASPStorefront.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkShopCom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkSearsCom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkChanAdvisor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkAmazon.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkVolusion.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkMagento.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageEvaluationRestrictions.ResumeLayout(False)
        CType(Me.PanelControlInventoryImport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlInventoryImport.ResumeLayout(False)
        Me.PanelControlInventoryImport.PerformLayout()
        CType(Me.cbeImportWizardQty2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkPurchaseNow.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageBilling.ResumeLayout(False)
        CType(Me.PanelControlBilling, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlBilling.ResumeLayout(False)
        Me.PanelControlBilling.PerformLayout()
        CType(Me.txtBillingCompany.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtConfirmPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtEmail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBillingLastName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBillingFirstName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeBillingSalutation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageDelivery.ResumeLayout(False)
        CType(Me.PanelControlDelivery, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDelivery.ResumeLayout(False)
        Me.PanelControlDelivery.PerformLayout()
        CType(Me.cbeShippingSalutation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtShippingCompany.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkBillingIsDelivery.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtShippingLastName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtShippingFirstName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPagePayment.ResumeLayout(False)
        CType(Me.PanelControlPayment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlPayment.ResumeLayout(False)
        Me.PanelControlPayment.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents ActivationWizardSectionContainerGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents ActivationWizardPluginContainerControl As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents WizardControlActivation As Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl
    Friend WithEvents TabPageComplete As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PageDescriptionCollection1 As Interprise.Presentation.Base.ExtendedXtraTabContol.PageDescriptionCollection
    Friend WithEvents TabPageWelcome As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageShared1 As Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared
    Friend WithEvents TabPageActivationCode As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents LabelConfirmActivationDetails As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelActivationDetailsOrError As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureBoxLerrynLogo As System.Windows.Forms.PictureBox
    Friend WithEvents TabPageBilling As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageDelivery As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PanelControlBilling As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControlDelivery As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblBillingSalutation As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtBillingFirstName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents cbeBillingSalutation As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblBillingFirstName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBillingLastName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtBillingLastName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblEmail As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtEmail As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PhoneControlBilling As Interprise.Presentation.Component.SharedControl.BasePhoneControl
    Friend WithEvents AddressControlBilling As Interprise.Presentation.Base.Address.AddressControl
    Friend WithEvents lblConfirmPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtConfirmPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents TabPagePayment As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PanelControlActivations As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblSelectConnectors2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkAmazon As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkVolusion As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkMagento As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblSelectConnectors1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkShopCom As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkChanAdvisor As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents PhoneControlShipping As Interprise.Presentation.Component.SharedControl.BasePhoneControl
    Friend WithEvents AddressControlShipping As Interprise.Presentation.Base.Address.AddressControl
    Friend WithEvents lblShippingLastName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtShippingLastName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblShippingFirstName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtShippingFirstName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents chkBillingIsDelivery As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkASPStorefront As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents PanelControlPayment As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblBillingCompany As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtBillingCompany As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblShippingCompany As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtShippingCompany As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblShippingSalutation As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeShippingSalutation As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbeMagentoCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbeASPStorefrontCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbeVolusionCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbeAmazonCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbeChanAdvCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbeShopComCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblLoginDetails As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkCancel As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblCancelText As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMagentoCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblShopComCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblChanlAdvCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAmazonCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblVolusionCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblASPStorefrontCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ImageComboBoxEditYears As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents chkPayForXYears As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkPayMonthly As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents btnUpdateActivationCost As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblTotalCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TabPageEvaluationRestrictions As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents lblCurrency As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PanelControlInventoryImport As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblInventoryImport As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkPurchaseNow As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txtManualActivation As DevExpress.XtraEditors.TextEdit
    Friend WithEvents chkEnterManualCode As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents btnUpdateActivationCode As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblManualActivation As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkImportWizardOnly As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblImportWizardQty2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeImportWizardQty2 As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblImportWizardQty4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblImportWizardQty3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblImportWizardOnlyCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblImportWizardQty1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeImportWizardQty1 As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents TabPagePriceSummary As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblPricingBasis As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblWelcomeFurtherDetails As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblPluginsURL As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblEBayCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeEBayCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkEBay As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblSearsComCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSearsComCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkSearsCom As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblAmazonFBACost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeAmazonFBACount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkAmazonFBA As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblErrorDetails As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRequestActivation As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblActivationEmail As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblTurnoverBased As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblProspectLeadCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkProspectLead As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblFileImportCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSelectConnectors3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkFileImport As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lbl3DCartCost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbe3DCartCount As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chk3DCart As DevExpress.XtraEditors.CheckEdit
End Class
#End Region
