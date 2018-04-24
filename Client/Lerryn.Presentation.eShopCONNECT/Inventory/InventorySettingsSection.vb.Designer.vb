'===============================================================================
' Connected Business SDK
' Copyright © 2004-2008 Interprise Software Systems International Inc.
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

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst

#Region " InventorySettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventorySettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventorySettingsSection))
        Me.InventorySettingsSectionGateway = Me.ImportExportDataset
        Me.InventorySettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.XtraTabPublishOn = New DevExpress.XtraTab.XtraTabControl()
        Me.PageTabAmazon = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerAmazonSettings = New Interprise.Presentation.Base.PluginContainerControl()
        Me.PageTab3DCart = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainer3DCartSettings = New Interprise.Presentation.Base.PluginContainerControl()
        Me.PageTabASPStorefront = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerASPStorefrontSettings = New Interprise.Presentation.Base.PluginContainerControl()
        Me.PageTabChannelAdv = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerChannelAdvSettings = New Interprise.Presentation.Base.PluginContainerControl()
        Me.PageTabEBay = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerEBaySettings = New Interprise.Presentation.Base.PluginContainerControl()
        Me.PageTabMagento = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerMagentoSettings = New Interprise.Presentation.Base.PluginContainerControl()
        Me.PageTabShopCom = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerShopComSettings = New Interprise.Presentation.Base.PluginContainerControl()
        Me.InventorySettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutItemPublishOn = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventorySettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventorySettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventorySettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.XtraTabPublishOn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPublishOn.SuspendLayout()
        Me.PageTabAmazon.SuspendLayout()
        CType(Me.PluginContainerAmazonSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PageTab3DCart.SuspendLayout()
        CType(Me.PluginContainer3DCartSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PageTabASPStorefront.SuspendLayout()
        CType(Me.PluginContainerASPStorefrontSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PageTabChannelAdv.SuspendLayout()
        CType(Me.PluginContainerChannelAdvSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PageTabEBay.SuspendLayout()
        CType(Me.PluginContainerEBaySettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PageTabMagento.SuspendLayout()
        CType(Me.PluginContainerMagentoSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PageTabShopCom.SuspendLayout()
        CType(Me.PluginContainerShopComSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventorySettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemPublishOn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventorySettingsSectionGateway
        '
        Me.InventorySettingsSectionGateway.DataSetName = "InventorySettingsSectionDataset"
        Me.InventorySettingsSectionGateway.Instantiate = False
        Me.InventorySettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventorySettingsSectionExtendedLayout
        '
        Me.InventorySettingsSectionExtendedLayout.AllowCustomizationMenu = False
        Me.InventorySettingsSectionExtendedLayout.Controls.Add(Me.XtraTabPublishOn)
        Me.InventorySettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventorySettingsSectionExtendedLayout.IsResetSection = False
        Me.InventorySettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventorySettingsSectionExtendedLayout.Name = "InventorySettingsSectionExtendedLayout"
        Me.InventorySettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventorySettingsSectionExtendedLayout.OptionsView.UseDefaultDragAndDropRendering = False
        Me.InventorySettingsSectionExtendedLayout.PluginContainerDataset = Nothing
        Me.InventorySettingsSectionExtendedLayout.Root = Me.InventorySettingsSectionLayoutGroup
        Me.InventorySettingsSectionExtendedLayout.Size = New System.Drawing.Size(978, 535)
        Me.InventorySettingsSectionExtendedLayout.TabIndex = 0
        Me.InventorySettingsSectionExtendedLayout.Text = "InventorySettingsSectionExtendedLayout"
        Me.InventorySettingsSectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'XtraTabPublishOn
        '
        Me.XtraTabPublishOn.Location = New System.Drawing.Point(3, 8)
        Me.XtraTabPublishOn.Name = "XtraTabPublishOn"
        Me.XtraTabPublishOn.SelectedTabPage = Me.PageTab3DCart
        Me.XtraTabPublishOn.Size = New System.Drawing.Size(972, 524)
        Me.XtraTabPublishOn.TabIndex = 4
        Me.XtraTabPublishOn.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.PageTab3DCart, Me.PageTabAmazon, Me.PageTabASPStorefront, Me.PageTabChannelAdv, Me.PageTabEBay, Me.PageTabMagento, Me.PageTabShopCom})
        '
        'PageTabAmazon
        '
        Me.PageTabAmazon.Controls.Add(Me.PluginContainerAmazonSettings)
        Me.PageTabAmazon.Name = "PageTabAmazon"
        Me.PageTabAmazon.Size = New System.Drawing.Size(966, 496)
        Me.PageTabAmazon.Text = "Amazon"
        '
        'PluginContainerAmazonSettings
        '
        Me.PluginContainerAmazonSettings.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerAmazonSettings.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerAmazonSettings.BaseLayoutControl = Nothing
        Me.PluginContainerAmazonSettings.ContextMenuButtonCaption = Nothing
        Me.PluginContainerAmazonSettings.CurrentControl = Nothing
        Me.PluginContainerAmazonSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerAmazonSettings.EditorsHeight = 0
        Me.PluginContainerAmazonSettings.GroupContextMenu = Nothing
        Me.PluginContainerAmazonSettings.HelpCode = Nothing
        Me.PluginContainerAmazonSettings.IsCustomTab = False
        Me.PluginContainerAmazonSettings.LayoutMode = False
        Me.PluginContainerAmazonSettings.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerAmazonSettings.Name = "PluginContainerAmazonSettings"
        Me.PluginContainerAmazonSettings.Plugin = "Lerryn.Presentation.eShopCONNECT.InventoryAmazonSettingsSection"
        Me.PluginContainerAmazonSettings.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerAmazonSettings.PluginManagerButton = Nothing
        Me.PluginContainerAmazonSettings.PluginRows = Nothing
        Me.PluginContainerAmazonSettings.SearchPluginButton = Nothing
        Me.PluginContainerAmazonSettings.ShowCaption = False
        Me.PluginContainerAmazonSettings.Size = New System.Drawing.Size(966, 496)
        Me.PluginContainerAmazonSettings.TabIndex = 1
        '
        'PageTab3DCart
        '
        Me.PageTab3DCart.Controls.Add(Me.PluginContainer3DCartSettings)
        Me.PageTab3DCart.Name = "PageTab3DCart"
        Me.PageTab3DCart.Size = New System.Drawing.Size(966, 496)
        Me.PageTab3DCart.Text = "3DCart"
        '
        'PluginContainer3DCartSettings
        '
        Me.PluginContainer3DCartSettings.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainer3DCartSettings.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainer3DCartSettings.BaseLayoutControl = Nothing
        Me.PluginContainer3DCartSettings.ContextMenuButtonCaption = Nothing
        Me.PluginContainer3DCartSettings.CurrentControl = Nothing
        Me.PluginContainer3DCartSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainer3DCartSettings.EditorsHeight = 0
        Me.PluginContainer3DCartSettings.GroupContextMenu = Nothing
        Me.PluginContainer3DCartSettings.HelpCode = Nothing
        Me.PluginContainer3DCartSettings.IsCustomTab = False
        Me.PluginContainer3DCartSettings.LayoutMode = False
        Me.PluginContainer3DCartSettings.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainer3DCartSettings.Name = "PluginContainer3DCartSettings"
        Me.PluginContainer3DCartSettings.Plugin = "Lerryn.Presentation.eShopCONNECT.Inventory3DCartSettingsSection"
        Me.PluginContainer3DCartSettings.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainer3DCartSettings.PluginManagerButton = Nothing
        Me.PluginContainer3DCartSettings.PluginRows = Nothing
        Me.PluginContainer3DCartSettings.SearchPluginButton = Nothing
        Me.PluginContainer3DCartSettings.ShowCaption = False
        Me.PluginContainer3DCartSettings.Size = New System.Drawing.Size(966, 496)
        Me.PluginContainer3DCartSettings.TabIndex = 2
        '
        'PageTabASPStorefront
        '
        Me.PageTabASPStorefront.Controls.Add(Me.PluginContainerASPStorefrontSettings)
        Me.PageTabASPStorefront.Name = "PageTabASPStorefront"
        Me.PageTabASPStorefront.Size = New System.Drawing.Size(966, 496)
        Me.PageTabASPStorefront.Text = "ASPDotNetStorefront"
        '
        'PluginContainerASPStorefrontSettings
        '
        Me.PluginContainerASPStorefrontSettings.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerASPStorefrontSettings.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerASPStorefrontSettings.BaseLayoutControl = Nothing
        Me.PluginContainerASPStorefrontSettings.ContextMenuButtonCaption = Nothing
        Me.PluginContainerASPStorefrontSettings.CurrentControl = Nothing
        Me.PluginContainerASPStorefrontSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerASPStorefrontSettings.EditorsHeight = 0
        Me.PluginContainerASPStorefrontSettings.GroupContextMenu = Nothing
        Me.PluginContainerASPStorefrontSettings.HelpCode = Nothing
        Me.PluginContainerASPStorefrontSettings.IsCustomTab = False
        Me.PluginContainerASPStorefrontSettings.LayoutMode = False
        Me.PluginContainerASPStorefrontSettings.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerASPStorefrontSettings.Name = "PluginContainerASPStorefrontSettings"
        Me.PluginContainerASPStorefrontSettings.Plugin = "Lerryn.Presentation.eShopCONNECT.InventoryASPStorefrontSettingsSection"
        Me.PluginContainerASPStorefrontSettings.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerASPStorefrontSettings.PluginManagerButton = Nothing
        Me.PluginContainerASPStorefrontSettings.PluginRows = Nothing
        Me.PluginContainerASPStorefrontSettings.SearchPluginButton = Nothing
        Me.PluginContainerASPStorefrontSettings.ShowCaption = False
        Me.PluginContainerASPStorefrontSettings.Size = New System.Drawing.Size(966, 496)
        Me.PluginContainerASPStorefrontSettings.TabIndex = 2
        '
        'PageTabChannelAdv
        '
        Me.PageTabChannelAdv.Controls.Add(Me.PluginContainerChannelAdvSettings)
        Me.PageTabChannelAdv.Name = "PageTabChannelAdv"
        Me.PageTabChannelAdv.Size = New System.Drawing.Size(966, 496)
        Me.PageTabChannelAdv.Text = "Channel Advisor"
        '
        'PluginContainerChannelAdvSettings
        '
        Me.PluginContainerChannelAdvSettings.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerChannelAdvSettings.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerChannelAdvSettings.BaseLayoutControl = Nothing
        Me.PluginContainerChannelAdvSettings.ContextMenuButtonCaption = Nothing
        Me.PluginContainerChannelAdvSettings.CurrentControl = Nothing
        Me.PluginContainerChannelAdvSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerChannelAdvSettings.EditorsHeight = 0
        Me.PluginContainerChannelAdvSettings.GroupContextMenu = Nothing
        Me.PluginContainerChannelAdvSettings.HelpCode = Nothing
        Me.PluginContainerChannelAdvSettings.IsCustomTab = False
        Me.PluginContainerChannelAdvSettings.LayoutMode = False
        Me.PluginContainerChannelAdvSettings.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerChannelAdvSettings.Name = "PluginContainerChannelAdvSettings"
        Me.PluginContainerChannelAdvSettings.Plugin = "Lerryn.Presentation.eShopCONNECT.InventoryChannelAdvSettingsSection"
        Me.PluginContainerChannelAdvSettings.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerChannelAdvSettings.PluginManagerButton = Nothing
        Me.PluginContainerChannelAdvSettings.PluginRows = Nothing
        Me.PluginContainerChannelAdvSettings.SearchPluginButton = Nothing
        Me.PluginContainerChannelAdvSettings.ShowCaption = False
        Me.PluginContainerChannelAdvSettings.Size = New System.Drawing.Size(966, 496)
        Me.PluginContainerChannelAdvSettings.TabIndex = 2
        '
        'PageTabEBay
        '
        Me.PageTabEBay.Controls.Add(Me.PluginContainerEBaySettings)
        Me.PageTabEBay.Name = "PageTabEBay"
        Me.PageTabEBay.Size = New System.Drawing.Size(966, 496)
        Me.PageTabEBay.Text = "eBay"
        '
        'PluginContainerEBaySettings
        '
        Me.PluginContainerEBaySettings.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerEBaySettings.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerEBaySettings.BaseLayoutControl = Nothing
        Me.PluginContainerEBaySettings.ContextMenuButtonCaption = Nothing
        Me.PluginContainerEBaySettings.CurrentControl = Nothing
        Me.PluginContainerEBaySettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerEBaySettings.EditorsHeight = 0
        Me.PluginContainerEBaySettings.GroupContextMenu = Nothing
        Me.PluginContainerEBaySettings.HelpCode = Nothing
        Me.PluginContainerEBaySettings.IsCustomTab = False
        Me.PluginContainerEBaySettings.LayoutMode = False
        Me.PluginContainerEBaySettings.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerEBaySettings.Name = "PluginContainerEBaySettings"
        Me.PluginContainerEBaySettings.Plugin = "Lerryn.Presentation.eShopCONNECT.InventoryEBaySettingsSection"
        Me.PluginContainerEBaySettings.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerEBaySettings.PluginManagerButton = Nothing
        Me.PluginContainerEBaySettings.PluginRows = Nothing
        Me.PluginContainerEBaySettings.SearchPluginButton = Nothing
        Me.PluginContainerEBaySettings.ShowCaption = False
        Me.PluginContainerEBaySettings.Size = New System.Drawing.Size(966, 496)
        Me.PluginContainerEBaySettings.TabIndex = 3
        '
        'PageTabMagento
        '
        Me.PageTabMagento.Controls.Add(Me.PluginContainerMagentoSettings)
        Me.PageTabMagento.Name = "PageTabMagento"
        Me.PageTabMagento.Size = New System.Drawing.Size(966, 496)
        Me.PageTabMagento.Text = "Magento"
        '
        'PluginContainerMagentoSettings
        '
        Me.PluginContainerMagentoSettings.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerMagentoSettings.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerMagentoSettings.BaseLayoutControl = Nothing
        Me.PluginContainerMagentoSettings.ContextMenuButtonCaption = Nothing
        Me.PluginContainerMagentoSettings.CurrentControl = Nothing
        Me.PluginContainerMagentoSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerMagentoSettings.EditorsHeight = 0
        Me.PluginContainerMagentoSettings.GroupContextMenu = Nothing
        Me.PluginContainerMagentoSettings.HelpCode = Nothing
        Me.PluginContainerMagentoSettings.IsCustomTab = False
        Me.PluginContainerMagentoSettings.LayoutMode = False
        Me.PluginContainerMagentoSettings.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerMagentoSettings.Name = "PluginContainerMagentoSettings"
        Me.PluginContainerMagentoSettings.Plugin = "Lerryn.Presentation.eShopCONNECT.InventoryMagentoSettingsSection"
        Me.PluginContainerMagentoSettings.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerMagentoSettings.PluginManagerButton = Nothing
        Me.PluginContainerMagentoSettings.PluginRows = Nothing
        Me.PluginContainerMagentoSettings.SearchPluginButton = Nothing
        Me.PluginContainerMagentoSettings.ShowCaption = False
        Me.PluginContainerMagentoSettings.Size = New System.Drawing.Size(966, 496)
        Me.PluginContainerMagentoSettings.TabIndex = 3
        '
        'PageTabShopCom
        '
        Me.PageTabShopCom.Controls.Add(Me.PluginContainerShopComSettings)
        Me.PageTabShopCom.Name = "PageTabShopCom"
        Me.PageTabShopCom.Size = New System.Drawing.Size(966, 496)
        Me.PageTabShopCom.Text = "Shop.com"
        '
        'PluginContainerShopComSettings
        '
        Me.PluginContainerShopComSettings.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerShopComSettings.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerShopComSettings.BaseLayoutControl = Nothing
        Me.PluginContainerShopComSettings.ContextMenuButtonCaption = Nothing
        Me.PluginContainerShopComSettings.CurrentControl = Nothing
        Me.PluginContainerShopComSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerShopComSettings.EditorsHeight = 0
        Me.PluginContainerShopComSettings.GroupContextMenu = Nothing
        Me.PluginContainerShopComSettings.HelpCode = Nothing
        Me.PluginContainerShopComSettings.IsCustomTab = False
        Me.PluginContainerShopComSettings.LayoutMode = False
        Me.PluginContainerShopComSettings.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerShopComSettings.Name = "PluginContainerShopComSettings"
        Me.PluginContainerShopComSettings.Plugin = "Lerryn.Presentation.eShopCONNECT.InventoryShopComSettingsSection"
        Me.PluginContainerShopComSettings.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerShopComSettings.PluginManagerButton = Nothing
        Me.PluginContainerShopComSettings.PluginRows = Nothing
        Me.PluginContainerShopComSettings.SearchPluginButton = Nothing
        Me.PluginContainerShopComSettings.ShowCaption = False
        Me.PluginContainerShopComSettings.Size = New System.Drawing.Size(966, 496)
        Me.PluginContainerShopComSettings.TabIndex = 2
        '
        'InventorySettingsSectionLayoutGroup
        '
        Me.InventorySettingsSectionLayoutGroup.CustomizationFormText = "InventorySettingsSectionLayoutGroup"
        Me.InventorySettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventorySettingsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutItemPublishOn})
        Me.InventorySettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventorySettingsSectionLayoutGroup.Name = "InventorySettingsSectionLayoutGroup"
        Me.InventorySettingsSectionLayoutGroup.Size = New System.Drawing.Size(978, 535)
        Me.InventorySettingsSectionLayoutGroup.Text = "InventorySettingsSectionLayoutGroup"
        Me.InventorySettingsSectionLayoutGroup.TextVisible = False
        '
        'LayoutItemPublishOn
        '
        Me.LayoutItemPublishOn.Control = Me.XtraTabPublishOn
        Me.LayoutItemPublishOn.CustomizationFormText = "LayoutItemPublishOn"
        Me.LayoutItemPublishOn.Location = New System.Drawing.Point(0, 0)
        Me.LayoutItemPublishOn.Name = "LayoutItemPublishOn"
        Me.LayoutItemPublishOn.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemPublishOn.Size = New System.Drawing.Size(978, 535)
        Me.LayoutItemPublishOn.Text = "LayoutItemPublishOn"
        Me.LayoutItemPublishOn.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemPublishOn, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemPublishOn.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutItemPublishOn.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutItemPublishOn.TextToControlDistance = 5
        '
        'InventorySettingsSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.InventorySettingsSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventorySettingsSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(978, 535)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventorySettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventorySettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventorySettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.XtraTabPublishOn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPublishOn.ResumeLayout(False)
        Me.PageTabAmazon.ResumeLayout(False)
        CType(Me.PluginContainerAmazonSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PageTab3DCart.ResumeLayout(False)
        CType(Me.PluginContainer3DCartSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PageTabASPStorefront.ResumeLayout(False)
        CType(Me.PluginContainerASPStorefrontSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PageTabChannelAdv.ResumeLayout(False)
        CType(Me.PluginContainerChannelAdvSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PageTabEBay.ResumeLayout(False)
        CType(Me.PluginContainerEBaySettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PageTabMagento.ResumeLayout(False)
        CType(Me.PluginContainerMagentoSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PageTabShopCom.ResumeLayout(False)
        CType(Me.PluginContainerShopComSettings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventorySettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemPublishOn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventorySettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventorySettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventorySettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents XtraTabPublishOn As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents PageTabAmazon As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PageTabShopCom As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents LayoutItemPublishOn As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents PluginContainerAmazonSettings As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents PluginContainerShopComSettings As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents PageTabChannelAdv As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PluginContainerChannelAdvSettings As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents PageTabMagento As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PluginContainerMagentoSettings As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents PageTabASPStorefront As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PluginContainerASPStorefrontSettings As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents PluginContainerEBaySettings As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents PageTabEBay As DevExpress.XtraTab.XtraTabPage

#Region "ImportExportDataset"
    Public ReadOnly Property ImportExportDataset() As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_InventorySettingsDataset
        End Get
    End Property
#End Region

#Region "Interprise Plugin Initialization"
#Region "PluginContainerAmazonSettings_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerAmazonSettings_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerAmazonSettings.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerAmazonSettings.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.InventoryAmazonSettingsSection), m_InventorySettingsDataset, m_InventorySettingsFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region

#Region "PluginContainerAmazonSettingsPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerAmazonSettingsPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerAmazonSettings.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerASPStorefrontSettings_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerASPStorefrontSettings_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerASPStorefrontSettings.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerASPStorefrontSettings.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.InventoryASPStorefrontSettingsSection), m_InventorySettingsDataset, m_InventorySettingsFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region

#Region "PluginContainerASPStorefrontSettingsPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerASPStorefrontSettingsPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerASPStorefrontSettings.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerChannelAdvSettings_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerChannelAdvSettings_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerChannelAdvSettings.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerChannelAdvSettings.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.InventoryChannelAdvSettingsSection), m_InventorySettingsDataset, m_InventorySettingsFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region

#Region "PluginContainerChannelAdvSettingsPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerChannelAdvSettingsPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerChannelAdvSettings.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerEBaySettings_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerEBaySettings_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerEBaySettings.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerEBaySettings.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.InventoryEBaySettingsSection), Me.ImportExportDataset, m_InventorySettingsFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
    End Sub

#End Region

#Region "PluginContainerEBaySettingsPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerEBaySettingsPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerEBaySettings.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerMagentoSettings_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerMagentoSettings_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerMagentoSettings.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerMagentoSettings.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.InventoryMagentoSettingsSection), m_InventorySettingsDataset, m_InventorySettingsFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region

#Region "PluginContainerMagentoSettingsPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerMagentoSettingsPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerMagentoSettings.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerShopComSettings_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerShopComSettings_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerShopComSettings.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerShopComSettings.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.InventoryShopComSettingsSection), m_InventorySettingsDataset, m_InventorySettingsFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub
    Friend WithEvents PageTab3DCart As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PluginContainer3DCartSettings As Interprise.Presentation.Base.PluginContainerControl

#End Region

#Region "PluginContainerShopComSettingsPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerShopComSettingsPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerShopComSettings.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainer3DCartSettings_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainer3DCartSettings_InitializePlugin(sender As System.Object, e As System.EventArgs) Handles PluginContainer3DCartSettings.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainer3DCartSettings.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.Inventory3DCartSettingsSection), Me.ImportExportDataset, m_InventorySettingsFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region


#Region "PluginContainer3DCartSettingsPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainer3DCartSettingsPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainer3DCartSettings.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region
#End Region


End Class
#End Region
