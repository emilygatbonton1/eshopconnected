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

#Region " ConfigSettingsListSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigSettingsListSection
    Inherits Interprise.Presentation.Component.SharedControl.SystemManager.BaseListDetailControl
    Implements Interprise.Extendable.Base.Presentation.SystemManager.IBaseListDetailInterface ' Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigSettingsListSection))
        Me.ConfigSettingsSectionGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
        Me.PluginContainerDetail = New Interprise.Presentation.Base.PluginContainerControl()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.TabPageDeliveryTranslation = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerDeliveryTranslation = New Interprise.Presentation.Base.PluginContainerControl()
        Me.TabPageSKUAliasLookup = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerSKUAliasLookup = New Interprise.Presentation.Base.PluginContainerControl()
        Me.TabPagePaymentTranslation = New DevExpress.XtraTab.XtraTabPage()
        Me.PluginContainerPaymentTranslation = New Interprise.Presentation.Base.PluginContainerControl()
        CType(Me.PluginContainerList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PluginContainerList.SuspendLayout()
        CType(Me.TabListDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabListDetail.SuspendLayout()
        Me.TabPageList.SuspendLayout()
        Me.TabPageDetail.SuspendLayout()
        CType(Me.PanelSimple, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlBaseDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControlBaseDetail.SuspendLayout()
        CType(Me.PluginContainerBaseDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutGroupBaseDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemBaseDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PluginContainerDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PluginContainerDetail.SuspendLayout()
        Me.TabPageDeliveryTranslation.SuspendLayout()
        CType(Me.PluginContainerDeliveryTranslation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageSKUAliasLookup.SuspendLayout()
        CType(Me.PluginContainerSKUAliasLookup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPagePaymentTranslation.SuspendLayout()
        CType(Me.PluginContainerPaymentTranslation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabListDetail
        '
        Me.TabListDetail.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.TabListDetail.Appearance.Options.UseBackColor = True
        Me.TabListDetail.AppearancePage.Header.Options.UseTextOptions = True
        Me.TabListDetail.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TabListDetail.AppearancePage.PageClient.BackColor = System.Drawing.SystemColors.Control
        Me.TabListDetail.AppearancePage.PageClient.Options.UseBackColor = True
        Me.TabListDetail.PaintStyleName = "Skin"
        Me.TabListDetail.SelectedTabPage = Me.TabPageList
        Me.TabListDetail.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.TabPageDeliveryTranslation, Me.TabPagePaymentTranslation, Me.TabPageSKUAliasLookup})
        Me.TabListDetail.Controls.SetChildIndex(Me.TabPagePaymentTranslation, 0)
        Me.TabListDetail.Controls.SetChildIndex(Me.TabPageSKUAliasLookup, 0)
        Me.TabListDetail.Controls.SetChildIndex(Me.TabPageDeliveryTranslation, 0)
        Me.TabListDetail.Controls.SetChildIndex(Me.TabPageDetail, 0)
        Me.TabListDetail.Controls.SetChildIndex(Me.TabPageList, 0)
        '
        'TabPageList
        '
        Me.TabPageList.Appearance.PageClient.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageList.Appearance.PageClient.Options.UseBackColor = True
        '
        'TabPageDetail
        '
        Me.TabPageDetail.Appearance.PageClient.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageDetail.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageDetail.Controls.Add(Me.PluginContainerDetail)
        Me.TabPageDetail.Controls.SetChildIndex(Me.PluginContainerDetail, 0)
        Me.TabPageDetail.Controls.SetChildIndex(Me.LayoutControlBaseDetail, 0)
        '
        'btnRefresh
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnRefresh, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnRefresh, System.Drawing.Color.Empty)
        Me.ExtendControlProperty.SetTextDisplay(Me.btnRefresh, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnNew
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnNew, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnNew, System.Drawing.Color.Empty)
        Me.ExtendControlProperty.SetTextDisplay(Me.btnNew, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnDelete
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnDelete, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnDelete, System.Drawing.Color.Empty)
        Me.ExtendControlProperty.SetTextDisplay(Me.btnDelete, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnEdit
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnEdit, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnEdit, System.Drawing.Color.Empty)
        Me.ExtendControlProperty.SetTextDisplay(Me.btnEdit, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'listControl
        '
        Me.listControl.AdditionalFilter = "and ShowIfActivated_DEV000221 = 0"
        Me.listControl.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.listControl.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.listControl.Appearance.Options.UseBackColor = True
        Me.listControl.Appearance.Options.UseFont = True
        Me.listControl.DataBindings.Add(New System.Windows.Forms.Binding("DocumentCode", Me.ConfigSettingsSectionGateway, "LerrynImportExportConfig_DEV000221.SourceCode_DEV000221", True))
        Me.ExtendControlProperty.SetHelpText(Me.listControl, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.listControl, System.Drawing.Color.Empty)
        Me.ExtendControlProperty.SetTextDisplay(Me.listControl, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LayoutControlBaseDetail
        '
        Me.LayoutControlBaseDetail.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.LayoutControlBaseDetail.Controls.SetChildIndex(Me.PluginContainerBaseDetail, 0)
        '
        'PluginContainerBaseDetail
        '
        Me.PluginContainerBaseDetail.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerBaseDetail.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerBaseDetail.Plugin = "Lerryn.Presentation.eShopCONNECT.ConfigSettingsDetailSection"
        Me.PluginContainerBaseDetail.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerBaseDetail.ShowCaption = False
        '
        'LayoutItemBaseDetail
        '
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemBaseDetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'ConfigSettingsSectionGateway
        '
        Me.ConfigSettingsSectionGateway.DataSetName = "ConfigSettingsSectionDataset"
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
        'PluginContainerDetail
        '
        Me.PluginContainerDetail.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerDetail.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerDetail.BaseLayoutControl = Nothing
        Me.PluginContainerDetail.ContextMenuButtonCaption = Nothing
        Me.PluginContainerDetail.Controls.Add(Me.SimpleButton1)
        Me.PluginContainerDetail.CurrentControl = Nothing
        Me.PluginContainerDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerDetail.EditorsHeight = 0
        Me.PluginContainerDetail.GroupContextMenu = Nothing
        Me.PluginContainerDetail.HelpCode = Nothing
        Me.PluginContainerDetail.IsCustomTab = False
        Me.PluginContainerDetail.LayoutMode = False
        Me.PluginContainerDetail.Location = New System.Drawing.Point(4, 4)
        Me.PluginContainerDetail.Name = "PluginContainerDetail"
        Me.PluginContainerDetail.PluginManagerButton = Nothing
        Me.PluginContainerDetail.SearchPluginButton = Nothing
        Me.PluginContainerDetail.Size = New System.Drawing.Size(801, 483)
        Me.PluginContainerDetail.TabIndex = 0
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AllowFocus = False
        Me.ExtendControlProperty.SetHelpText(Me.SimpleButton1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.SimpleButton1.Image = CType(resources.GetObject("SimpleButton1.Image"), System.Drawing.Image)
        Me.SimpleButton1.Location = New System.Drawing.Point(657, 2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.SimpleButton1, System.Drawing.Color.Empty)
        Me.SimpleButton1.Size = New System.Drawing.Size(20, 20)
        Me.SimpleButton1.TabIndex = 0
        Me.SimpleButton1.TabStop = False
        Me.ExtendControlProperty.SetTextDisplay(Me.SimpleButton1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TabPageDeliveryTranslation
        '
        Me.TabPageDeliveryTranslation.Controls.Add(Me.PluginContainerDeliveryTranslation)
        Me.TabPageDeliveryTranslation.Name = "TabPageDeliveryTranslation"
        Me.TabPageDeliveryTranslation.PageVisible = False
        Me.TabPageDeliveryTranslation.Size = New System.Drawing.Size(809, 491)
        Me.TabPageDeliveryTranslation.Text = "Delivery Method Translation"
        '
        'PluginContainerDeliveryTranslation
        '
        Me.PluginContainerDeliveryTranslation.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerDeliveryTranslation.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerDeliveryTranslation.BaseLayoutControl = Nothing
        Me.PluginContainerDeliveryTranslation.ContextMenuButtonCaption = Nothing
        Me.PluginContainerDeliveryTranslation.CurrentControl = Nothing
        Me.PluginContainerDeliveryTranslation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerDeliveryTranslation.EditorsHeight = 0
        Me.PluginContainerDeliveryTranslation.GroupContextMenu = Nothing
        Me.PluginContainerDeliveryTranslation.HelpCode = Nothing
        Me.PluginContainerDeliveryTranslation.IsCustomTab = False
        Me.PluginContainerDeliveryTranslation.LayoutMode = False
        Me.PluginContainerDeliveryTranslation.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerDeliveryTranslation.Name = "PluginContainerDeliveryTranslation"
        Me.PluginContainerDeliveryTranslation.Plugin = "Lerryn.Presentation.eShopCONNECT.ConfigSettingsDeliveryTranslationSection"
        Me.PluginContainerDeliveryTranslation.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerDeliveryTranslation.PluginManagerButton = Nothing
        Me.PluginContainerDeliveryTranslation.SearchPluginButton = Nothing
        Me.PluginContainerDeliveryTranslation.ShowCaption = False
        Me.PluginContainerDeliveryTranslation.Size = New System.Drawing.Size(809, 491)
        Me.PluginContainerDeliveryTranslation.TabIndex = 0
        '
        'TabPageSKUAliasLookup
        '
        Me.TabPageSKUAliasLookup.Controls.Add(Me.PluginContainerSKUAliasLookup)
        Me.TabPageSKUAliasLookup.Name = "TabPageSKUAliasLookup"
        Me.TabPageSKUAliasLookup.PageVisible = False
        Me.TabPageSKUAliasLookup.Size = New System.Drawing.Size(809, 491)
        Me.TabPageSKUAliasLookup.Text = "SKU Alias Lookup"
        '
        'PluginContainerSKUAliasLookup
        '
        Me.PluginContainerSKUAliasLookup.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerSKUAliasLookup.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerSKUAliasLookup.BaseLayoutControl = Nothing
        Me.PluginContainerSKUAliasLookup.ContextMenuButtonCaption = Nothing
        Me.PluginContainerSKUAliasLookup.CurrentControl = Nothing
        Me.PluginContainerSKUAliasLookup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerSKUAliasLookup.EditorsHeight = 0
        Me.PluginContainerSKUAliasLookup.GroupContextMenu = Nothing
        Me.PluginContainerSKUAliasLookup.HelpCode = Nothing
        Me.PluginContainerSKUAliasLookup.IsCustomTab = False
        Me.PluginContainerSKUAliasLookup.LayoutMode = False
        Me.PluginContainerSKUAliasLookup.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerSKUAliasLookup.Name = "PluginContainerSKUAliasLookup"
        Me.PluginContainerSKUAliasLookup.Plugin = "Lerryn.Presentation.eShopCONNECT.ConfigSettingsSKUAliasLookupSection"
        Me.PluginContainerSKUAliasLookup.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerSKUAliasLookup.PluginManagerButton = Nothing
        Me.PluginContainerSKUAliasLookup.SearchPluginButton = Nothing
        Me.PluginContainerSKUAliasLookup.ShowCaption = False
        Me.PluginContainerSKUAliasLookup.Size = New System.Drawing.Size(809, 491)
        Me.PluginContainerSKUAliasLookup.TabIndex = 1
        '
        'TabPagePaymentTranslation
        '
        Me.TabPagePaymentTranslation.Controls.Add(Me.PluginContainerPaymentTranslation)
        Me.TabPagePaymentTranslation.Name = "TabPagePaymentTranslation"
        Me.TabPagePaymentTranslation.PageVisible = False
        Me.TabPagePaymentTranslation.Size = New System.Drawing.Size(809, 491)
        Me.TabPagePaymentTranslation.Text = "Payment Type Translation"
        '
        'PluginContainerPaymentTranslation
        '
        Me.PluginContainerPaymentTranslation.AppearanceCaption.Options.UseTextOptions = True
        Me.PluginContainerPaymentTranslation.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PluginContainerPaymentTranslation.BaseLayoutControl = Nothing
        Me.PluginContainerPaymentTranslation.ContextMenuButtonCaption = Nothing
        Me.PluginContainerPaymentTranslation.CurrentControl = Nothing
        Me.PluginContainerPaymentTranslation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PluginContainerPaymentTranslation.EditorsHeight = 0
        Me.PluginContainerPaymentTranslation.GroupContextMenu = Nothing
        Me.PluginContainerPaymentTranslation.HelpCode = Nothing
        Me.PluginContainerPaymentTranslation.IsCustomTab = False
        Me.PluginContainerPaymentTranslation.LayoutMode = False
        Me.PluginContainerPaymentTranslation.Location = New System.Drawing.Point(0, 0)
        Me.PluginContainerPaymentTranslation.Name = "PluginContainerPaymentTranslation"
        Me.PluginContainerPaymentTranslation.Plugin = "Lerryn.Presentation.eShopCONNECT.ConfigSettingsPaymentTranslationSection"
        Me.PluginContainerPaymentTranslation.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PluginContainerPaymentTranslation.PluginManagerButton = Nothing
        Me.PluginContainerPaymentTranslation.SearchPluginButton = Nothing
        Me.PluginContainerPaymentTranslation.ShowCaption = False
        Me.PluginContainerPaymentTranslation.Size = New System.Drawing.Size(809, 491)
        Me.PluginContainerPaymentTranslation.TabIndex = 1
        '
        'ConfigSettingsListSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.DataBindings.Add(New System.Windows.Forms.Binding("DocumentCode", Me.ConfigSettingsSectionGateway, "DisplayedSourcesView_DEV000221.SourceCode_DEV000221", True))
        Me.DisplayField = "SourceCode_DEV000221"
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.KeyField = "SourceCode_DEV000221"
        Me.Name = "ConfigSettingsListSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.ShowAddButton = True
        Me.ShowDeleteButton = True
        Me.ShowEditButton = True
        Me.ShowRefreshButton = True
        Me.Tablename = "SourceList"
        Me.Tablenames = New String() {"DisplayedSourcesView_DEV000221"}
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.PluginContainerList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PluginContainerList.ResumeLayout(False)
        CType(Me.TabListDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabListDetail.ResumeLayout(False)
        Me.TabPageList.ResumeLayout(False)
        Me.TabPageDetail.ResumeLayout(False)
        CType(Me.PanelSimple, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlBaseDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControlBaseDetail.ResumeLayout(False)
        CType(Me.PluginContainerBaseDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutGroupBaseDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemBaseDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PluginContainerDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PluginContainerDetail.ResumeLayout(False)
        Me.TabPageDeliveryTranslation.ResumeLayout(False)
        CType(Me.PluginContainerDeliveryTranslation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageSKUAliasLookup.ResumeLayout(False)
        CType(Me.PluginContainerSKUAliasLookup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPagePaymentTranslation.ResumeLayout(False)
        CType(Me.PluginContainerPaymentTranslation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents ConfigSettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents PluginContainerDetail As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TabPageDeliveryTranslation As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PluginContainerDeliveryTranslation As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents TabPageSKUAliasLookup As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PluginContainerSKUAliasLookup As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents TabPagePaymentTranslation As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PluginContainerPaymentTranslation As Interprise.Presentation.Base.PluginContainerControl

#Region "Interprise Plugin Initialization"

    Protected Overrides Sub PluginContainerBaseDetail_AfterInitializePlugin(ByVal sender As Object, ByVal e As System.EventArgs)
        MyBase.PluginContainerBaseDetail_AfterInitializePlugin(sender, e)
    End Sub

#Region "PluginContainerBaseDetail_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overrides Sub PluginContainerBaseDetail_InitializePlugin(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerBaseDetail.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.ConfigSettingsDetailSection), ConfigSettingsSectionGateway, m_ConfigSettingsSectionFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
    End Sub

#End Region

#Region "PluginContainerBaseDetailPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerBaseDetailPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerBaseDetail.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerDeliveryTranslation_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerDeliveryTranslation_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerDeliveryTranslation.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerDeliveryTranslation.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.ConfigSettingsDeliveryTranslationSection), ConfigSettingsSectionGateway, m_ConfigSettingsSectionFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region

#Region "PluginContainerDeliveryTranslationPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerDeliveryTranslationPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerDeliveryTranslation.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerPaymentTranslation_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerPaymentTranslation_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerPaymentTranslation.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerPaymentTranslation.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.ConfigSettingsPaymentTranslationSection), ConfigSettingsSectionGateway, m_ConfigSettingsSectionFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region

#Region "PluginContainerPaymentTranslationPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerPaymentTranslationPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerPaymentTranslation.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region

#Region "PluginContainerSKUAliasLookup_InitializePlugin"

    ''' <summary>
    ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
    ''' </summary>
    ''' <param name="sender">The object instance that invoked the event</param>
    ''' <param name="e">The event argument passed by the sender</param>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Private Sub PluginContainerSKUAliasLookup_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PluginContainerSKUAliasLookup.InitializePlugin
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PluginContainerSKUAliasLookup.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.ConfigSettingsSKUAliasLookupSection), ConfigSettingsSectionGateway, m_ConfigSettingsSectionFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

    End Sub

#End Region

#Region "PluginContainerSKUAliasLookupPluginInstance"
    ''' <summary>
    ''' Returns the instance of plugin assigned to the plugin container.
    ''' </summary>
    ''' <value>The instance of plugin assigned to the plugin container</value>
    ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
    ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

    Protected Overridable ReadOnly Property PluginContainerSKUAliasLookupPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
        Get
            Return CType(PluginContainerSKUAliasLookup.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Get
    End Property
#End Region
#End Region

End Class
#End Region
