'===============================================================================
' Connected Business SDK
' Copyright © 2012 Interprise Solutions
' All rights reserved.
' 
' Connected Business SDK - Generated Code
'
' This code and information is provided "as is" without warranty
' of any kind, either expressed or implied, including but not
' limited to the implied warranties of merchantability and
' fitness for a particular purpose.
'===============================================================================

Option Explicit On
Option Strict On

#Region " SKUAliasWizardSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SKUAliasWizardSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SKUAliasWizardSection))
        Me.SKUAliasWizardSectionGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
        Me.ImportWizardPluginContainerControl = New Interprise.Presentation.Base.PluginContainerControl()
        Me.WizardControlImport = New Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl()
        Me.TabPageComplete = New DevExpress.XtraTab.XtraTabPage()
        Me.TabPageWelcome = New DevExpress.XtraTab.XtraTabPage()
        Me.RadioGroupSelectFunction = New DevExpress.XtraEditors.RadioGroup()
        Me.lblPluginsURL = New DevExpress.XtraEditors.LabelControl()
        Me.lblWelcomeFurtherDetails = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageShared1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared()
        Me.TabPageSelectImportFile = New DevExpress.XtraTab.XtraTabPage()
        Me.TextEditFilePath = New DevExpress.XtraEditors.TextEdit()
        Me.btnGetFile = New DevExpress.XtraEditors.SimpleButton()
        Me.lblFilePathError = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageProcessing = New DevExpress.XtraTab.XtraTabPage()
        Me.lblProgress = New DevExpress.XtraEditors.LabelControl()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SKUAliasWizardSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImportWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ImportWizardPluginContainerControl.SuspendLayout()
        CType(Me.WizardControlImport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardControlImport.SuspendLayout()
        Me.TabPageWelcome.SuspendLayout()
        CType(Me.RadioGroupSelectFunction.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageSelectImportFile.SuspendLayout()
        CType(Me.TextEditFilePath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageProcessing.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'SKUAliasWizardSectionGateway
        '
        Me.SKUAliasWizardSectionGateway.DataSetName = "SKUAliasWizardSectionDataset"
        Me.SKUAliasWizardSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        Me.ImportWizardPluginContainerControl.PluginManagerButton = Nothing
        Me.ImportWizardPluginContainerControl.PluginRows = Nothing
        Me.ImportWizardPluginContainerControl.SearchPluginButton = Nothing
        Me.ImportWizardPluginContainerControl.ShowCaption = False
        Me.ImportWizardPluginContainerControl.Size = New System.Drawing.Size(731, 452)
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
        Me.WizardControlImport.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.[False]
        Me.WizardControlImport.ShowTabHeader = DevExpress.Utils.DefaultBoolean.[False]
        Me.WizardControlImport.Size = New System.Drawing.Size(727, 448)
        Me.WizardControlImport.TabIndex = 0
        Me.WizardControlImport.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.TabPageWelcome, Me.TabPageSelectImportFile, Me.TabPageProcessing, Me.TabPageComplete})
        Me.WizardControlImport.Title = "the SKU Alias Import"
        Me.WizardControlImport.WelcomeMessage = "Error - SKU Alias Import Wizard failed to initialise"
        Me.WizardControlImport.WelcomePage = Me.TabPageWelcome
        '
        'TabPageComplete
        '
        Me.TabPageComplete.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageComplete.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageComplete.Name = "TabPageComplete"
        Me.TabPageComplete.Size = New System.Drawing.Size(721, 390)
        Me.TabPageComplete.Text = "TabPageComplete"
        '
        'TabPageWelcome
        '
        Me.TabPageWelcome.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageWelcome.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageWelcome.Controls.Add(Me.RadioGroupSelectFunction)
        Me.TabPageWelcome.Controls.Add(Me.lblPluginsURL)
        Me.TabPageWelcome.Controls.Add(Me.lblWelcomeFurtherDetails)
        Me.TabPageWelcome.Name = "TabPageWelcome"
        Me.TabPageWelcome.Size = New System.Drawing.Size(721, 390)
        Me.TabPageWelcome.Text = "TabPageWelcome"
        '
        'RadioGroupSelectFunction
        '
        Me.ExtendControlProperty.SetHelpText(Me.RadioGroupSelectFunction, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.RadioGroupSelectFunction.Location = New System.Drawing.Point(177, 140)
        Me.RadioGroupSelectFunction.Name = "RadioGroupSelectFunction"
        Me.RadioGroupSelectFunction.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("N", "Create empty import template file"), New DevExpress.XtraEditors.Controls.RadioGroupItem("E", "Export existing SKU Alias records for editing"), New DevExpress.XtraEditors.Controls.RadioGroupItem("A", "Import new SKU Alias records and add to existing records"), New DevExpress.XtraEditors.Controls.RadioGroupItem("R", "Import new and updated SKU Alias records and delete all existing records")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.RadioGroupSelectFunction, System.Drawing.Color.Empty)
        Me.RadioGroupSelectFunction.Size = New System.Drawing.Size(389, 96)
        Me.RadioGroupSelectFunction.TabIndex = 41
        Me.ExtendControlProperty.SetTextDisplay(Me.RadioGroupSelectFunction, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblPluginsURL
        '
        Me.lblPluginsURL.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.lblPluginsURL.Location = New System.Drawing.Point(268, 326)
        Me.lblPluginsURL.Name = "lblPluginsURL"
        Me.lblPluginsURL.Size = New System.Drawing.Size(143, 13)
        Me.lblPluginsURL.TabIndex = 40
        Me.lblPluginsURL.Text = "www.pluginsforinterprise.com"
        '
        'lblWelcomeFurtherDetails
        '
        Me.lblWelcomeFurtherDetails.Location = New System.Drawing.Point(177, 287)
        Me.lblWelcomeFurtherDetails.Name = "lblWelcomeFurtherDetails"
        Me.lblWelcomeFurtherDetails.Size = New System.Drawing.Size(423, 52)
        Me.lblWelcomeFurtherDetails.TabIndex = 39
        Me.lblWelcomeFurtherDetails.Text = "For further details about eShopCONNECTED, please switch to the Help module, click o" & _
    "n the" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "IS_PRODUCT_NAME Help icon and see the Working with eShopCONNECTED topic.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Alternatively see "
        '
        'TabPageShared1
        '
        Me.TabPageShared1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabPageShared1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TabPageShared1.Location = New System.Drawing.Point(0, 0)
        Me.TabPageShared1.Name = "TabPageShared1"
        Me.TabPageShared1.Size = New System.Drawing.Size(809, 483)
        Me.TabPageShared1.TabIndex = 5
        '
        'TabPageSelectImportFile
        '
        Me.TabPageSelectImportFile.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageSelectImportFile.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageSelectImportFile.Controls.Add(Me.TextEditFilePath)
        Me.TabPageSelectImportFile.Controls.Add(Me.btnGetFile)
        Me.TabPageSelectImportFile.Controls.Add(Me.lblFilePathError)
        Me.TabPageSelectImportFile.Name = "TabPageSelectImportFile"
        Me.TabPageSelectImportFile.Size = New System.Drawing.Size(721, 390)
        Me.TabPageSelectImportFile.Text = "Select file path"
        '
        'TextEditFilePath
        '
        Me.ExtendControlProperty.SetHelpText(Me.TextEditFilePath, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditFilePath.Location = New System.Drawing.Point(34, 138)
        Me.TextEditFilePath.Name = "TextEditFilePath"
        Me.TextEditFilePath.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditFilePath, System.Drawing.Color.Empty)
        Me.TextEditFilePath.Size = New System.Drawing.Size(379, 22)
        Me.TextEditFilePath.TabIndex = 5
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditFilePath, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnGetFile
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnGetFile, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnGetFile.Location = New System.Drawing.Point(437, 137)
        Me.btnGetFile.Name = "btnGetFile"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnGetFile, System.Drawing.Color.Empty)
        Me.btnGetFile.Size = New System.Drawing.Size(59, 23)
        Me.btnGetFile.TabIndex = 4
        Me.btnGetFile.Text = "Browse"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnGetFile, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblFilePathError
        '
        Me.lblFilePathError.Location = New System.Drawing.Point(34, 166)
        Me.lblFilePathError.Name = "lblFilePathError"
        Me.lblFilePathError.Size = New System.Drawing.Size(0, 13)
        Me.lblFilePathError.TabIndex = 6
        '
        'TabPageProcessing
        '
        Me.TabPageProcessing.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageProcessing.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageProcessing.Controls.Add(Me.lblProgress)
        Me.TabPageProcessing.Name = "TabPageProcessing"
        Me.TabPageProcessing.Size = New System.Drawing.Size(721, 390)
        Me.TabPageProcessing.Text = "Processing"
        '
        'lblProgress
        '
        Me.lblProgress.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
        Me.lblProgress.Location = New System.Drawing.Point(22, 84)
        Me.lblProgress.MinimumSize = New System.Drawing.Size(650, 280)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(650, 280)
        Me.lblProgress.TabIndex = 1
        Me.lblProgress.Text = "Progress"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'SKUAliasWizardSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.ImportWizardPluginContainerControl)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "SKUAliasWizardSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(731, 452)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SKUAliasWizardSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImportWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ImportWizardPluginContainerControl.ResumeLayout(False)
        CType(Me.WizardControlImport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardControlImport.ResumeLayout(False)
        Me.TabPageWelcome.ResumeLayout(False)
        Me.TabPageWelcome.PerformLayout()
        CType(Me.RadioGroupSelectFunction.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageSelectImportFile.ResumeLayout(False)
        Me.TabPageSelectImportFile.PerformLayout()
        CType(Me.TextEditFilePath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageProcessing.ResumeLayout(False)
        Me.TabPageProcessing.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents SKUAliasWizardSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents ImportWizardPluginContainerControl As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents WizardControlImport As Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl
    Friend WithEvents TabPageComplete As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageWelcome As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageShared1 As Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblPluginsURL As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblWelcomeFurtherDetails As DevExpress.XtraEditors.LabelControl
    Friend WithEvents RadioGroupSelectFunction As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents TabPageSelectImportFile As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TextEditFilePath As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnGetFile As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblFilePathError As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TabPageProcessing As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents lblProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog

End Class
#End Region

