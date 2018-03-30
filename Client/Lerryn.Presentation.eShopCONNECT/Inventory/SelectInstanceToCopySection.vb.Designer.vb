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

#Region " SelectInstanceToCopySection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectInstanceToCopySection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectInstanceToCopySection))
        Me.SelectInstanceToCopySectionGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
        Me.SelectInstanceToCopySectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.lblUseSettings = New DevExpress.XtraEditors.LabelControl()
        Me.cbeInstanceToCopy = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.rgSelectOption = New DevExpress.XtraEditors.RadioGroup()
        Me.SelectInstanceToCopySectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlSelectInstance = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem2 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlSelectOption = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SelectInstanceToCopySectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SelectInstanceToCopySectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SelectInstanceToCopySectionExtendedLayout.SuspendLayout()
        CType(Me.cbeInstanceToCopy.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgSelectOption.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SelectInstanceToCopySectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlSelectInstance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlSelectOption, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'SelectInstanceToCopySectionGateway
        '
        Me.SelectInstanceToCopySectionGateway.DataSetName = "SelectInstanceToCopySectionDataset"
        Me.SelectInstanceToCopySectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'SelectInstanceToCopySectionExtendedLayout
        '
        Me.SelectInstanceToCopySectionExtendedLayout.AllowCustomizationMenu = False
        Me.SelectInstanceToCopySectionExtendedLayout.Controls.Add(Me.lblUseSettings)
        Me.SelectInstanceToCopySectionExtendedLayout.Controls.Add(Me.cbeInstanceToCopy)
        Me.SelectInstanceToCopySectionExtendedLayout.Controls.Add(Me.rgSelectOption)
        Me.SelectInstanceToCopySectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SelectInstanceToCopySectionExtendedLayout.IsResetSection = False
        Me.SelectInstanceToCopySectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.SelectInstanceToCopySectionExtendedLayout.Name = "SelectInstanceToCopySectionExtendedLayout"
        Me.SelectInstanceToCopySectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.SelectInstanceToCopySectionExtendedLayout.PluginContainerDataset = Nothing
        Me.SelectInstanceToCopySectionExtendedLayout.Root = Me.SelectInstanceToCopySectionLayoutGroup
        Me.SelectInstanceToCopySectionExtendedLayout.Size = New System.Drawing.Size(390, 239)
        Me.SelectInstanceToCopySectionExtendedLayout.TabIndex = 0
        Me.SelectInstanceToCopySectionExtendedLayout.Text = "SelectInstanceToCopySectionExtendedLayout"
        Me.SelectInstanceToCopySectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'lblUseSettings
        '
        Me.lblUseSettings.Location = New System.Drawing.Point(2, 2)
        Me.lblUseSettings.MinimumSize = New System.Drawing.Size(0, 50)
        Me.lblUseSettings.Name = "lblUseSettings"
        Me.lblUseSettings.Size = New System.Drawing.Size(361, 50)
        Me.lblUseSettings.StyleController = Me.SelectInstanceToCopySectionExtendedLayout
        Me.lblUseSettings.TabIndex = 5
        Me.lblUseSettings.Text = "You have already published this Inventory Item to one Account/Instance" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Do you " & _
    "want to use those  settings as the basic for this Account/Instance ?"
        '
        'cbeInstanceToCopy
        '
        Me.cbeInstanceToCopy.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeInstanceToCopy, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeInstanceToCopy.Location = New System.Drawing.Point(167, 120)
        Me.cbeInstanceToCopy.MaximumSize = New System.Drawing.Size(200, 22)
        Me.cbeInstanceToCopy.Name = "cbeInstanceToCopy"
        Me.cbeInstanceToCopy.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeInstanceToCopy.Properties.Appearance.Options.UseFont = True
        Me.cbeInstanceToCopy.Properties.AutoHeight = False
        Me.cbeInstanceToCopy.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeInstanceToCopy, System.Drawing.Color.Empty)
        Me.cbeInstanceToCopy.Size = New System.Drawing.Size(200, 22)
        Me.cbeInstanceToCopy.StyleController = Me.SelectInstanceToCopySectionExtendedLayout
        Me.cbeInstanceToCopy.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeInstanceToCopy, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'rgSelectOption
        '
        Me.ExtendControlProperty.SetHelpText(Me.rgSelectOption, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.rgSelectOption.Location = New System.Drawing.Point(2, 56)
        Me.rgSelectOption.MaximumSize = New System.Drawing.Size(0, 60)
        Me.rgSelectOption.MinimumSize = New System.Drawing.Size(0, 60)
        Me.rgSelectOption.Name = "rgSelectOption"
        Me.rgSelectOption.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("N", "No - create an empty set of settings"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Y", "Yes - Copy settings from :-")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rgSelectOption, System.Drawing.Color.Empty)
        Me.rgSelectOption.Size = New System.Drawing.Size(386, 60)
        Me.rgSelectOption.StyleController = Me.SelectInstanceToCopySectionExtendedLayout
        Me.rgSelectOption.TabIndex = 6
        Me.ExtendControlProperty.SetTextDisplay(Me.rgSelectOption, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'SelectInstanceToCopySectionLayoutGroup
        '
        Me.SelectInstanceToCopySectionLayoutGroup.CustomizationFormText = "SelectInstanceToCopySectionLayoutGroup"
        Me.SelectInstanceToCopySectionLayoutGroup.GroupBordersVisible = False
        Me.SelectInstanceToCopySectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlSelectInstance, Me.LayoutControlItem2, Me.LayoutControlSelectOption})
        Me.SelectInstanceToCopySectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.SelectInstanceToCopySectionLayoutGroup.Name = "SelectInstanceToCopySectionLayoutGroup"
        Me.SelectInstanceToCopySectionLayoutGroup.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.SelectInstanceToCopySectionLayoutGroup.Size = New System.Drawing.Size(390, 239)
        Me.SelectInstanceToCopySectionLayoutGroup.Text = "SelectInstanceToCopySectionLayoutGroup"
        Me.SelectInstanceToCopySectionLayoutGroup.TextVisible = False
        '
        'LayoutControlSelectInstance
        '
        Me.LayoutControlSelectInstance.Control = Me.cbeInstanceToCopy
        Me.LayoutControlSelectInstance.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlSelectInstance.Location = New System.Drawing.Point(0, 118)
        Me.LayoutControlSelectInstance.Name = "LayoutControlSelectInstance"
        Me.LayoutControlSelectInstance.Size = New System.Drawing.Size(390, 121)
        Me.LayoutControlSelectInstance.Text = " "
        Me.LayoutControlSelectInstance.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlSelectInstance, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlSelectInstance.TextSize = New System.Drawing.Size(160, 13)
        Me.LayoutControlSelectInstance.TextToControlDistance = 5
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.lblUseSettings
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(390, 54)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlSelectOption
        '
        Me.LayoutControlSelectOption.Control = Me.rgSelectOption
        Me.LayoutControlSelectOption.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlSelectOption.Location = New System.Drawing.Point(0, 54)
        Me.LayoutControlSelectOption.Name = "LayoutControlSelectOption"
        Me.LayoutControlSelectOption.Size = New System.Drawing.Size(390, 64)
        Me.LayoutControlSelectOption.Text = "LayoutControlSelectOption"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlSelectOption, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlSelectOption.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlSelectOption.TextToControlDistance = 0
        Me.LayoutControlSelectOption.TextVisible = False
        '
        'SelectInstanceToCopySection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.SelectInstanceToCopySectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "SelectInstanceToCopySection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SelectInstanceToCopySectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SelectInstanceToCopySectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SelectInstanceToCopySectionExtendedLayout.ResumeLayout(False)
        CType(Me.cbeInstanceToCopy.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgSelectOption.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SelectInstanceToCopySectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlSelectInstance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlSelectOption, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents SelectInstanceToCopySectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents SelectInstanceToCopySectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents SelectInstanceToCopySectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents lblUseSettings As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeInstanceToCopy As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents rgSelectOption As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents LayoutControlSelectInstance As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItem2 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlSelectOption As Interprise.Presentation.Base.ExtendedLayoutControlItem

End Class
#End Region

