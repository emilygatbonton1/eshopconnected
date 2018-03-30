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

#Region " SelectDescriptionSourceSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectDescriptionSourceSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectDescriptionSourceSection))
        Me.SelectDescriptionSourceSectionGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
        Me.SelectDescriptionSourceSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.lblSelectSource = New DevExpress.XtraEditors.LabelControl()
        Me.rgSelectSource1 = New DevExpress.XtraEditors.RadioGroup()
        Me.rgSelectSource2 = New DevExpress.XtraEditors.RadioGroup()
        Me.SelectDescriptionSourceSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlSelectSource1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlSelectSource2 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.lblWeightUnits = New DevExpress.XtraEditors.LabelControl()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.cbeWeightUnits = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.LayoutControlItem3 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SelectDescriptionSourceSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SelectDescriptionSourceSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SelectDescriptionSourceSectionExtendedLayout.SuspendLayout()
        CType(Me.rgSelectSource1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rgSelectSource2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SelectDescriptionSourceSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlSelectSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlSelectSource2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeWeightUnits.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'SelectDescriptionSourceSectionGateway
        '
        Me.SelectDescriptionSourceSectionGateway.DataSetName = "SelectDescriptionSourceSectionDataset"
        Me.SelectDescriptionSourceSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'SelectDescriptionSourceSectionExtendedLayout
        '
        Me.SelectDescriptionSourceSectionExtendedLayout.AllowCustomizationMenu = False
        Me.SelectDescriptionSourceSectionExtendedLayout.Controls.Add(Me.cbeWeightUnits)
        Me.SelectDescriptionSourceSectionExtendedLayout.Controls.Add(Me.lblWeightUnits)
        Me.SelectDescriptionSourceSectionExtendedLayout.Controls.Add(Me.lblSelectSource)
        Me.SelectDescriptionSourceSectionExtendedLayout.Controls.Add(Me.rgSelectSource1)
        Me.SelectDescriptionSourceSectionExtendedLayout.Controls.Add(Me.rgSelectSource2)
        Me.SelectDescriptionSourceSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SelectDescriptionSourceSectionExtendedLayout.IsResetSection = False
        Me.SelectDescriptionSourceSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.SelectDescriptionSourceSectionExtendedLayout.Name = "SelectDescriptionSourceSectionExtendedLayout"
        Me.SelectDescriptionSourceSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.SelectDescriptionSourceSectionExtendedLayout.PluginContainerDataset = Nothing
        Me.SelectDescriptionSourceSectionExtendedLayout.Root = Me.SelectDescriptionSourceSectionLayoutGroup
        Me.SelectDescriptionSourceSectionExtendedLayout.Size = New System.Drawing.Size(390, 239)
        Me.SelectDescriptionSourceSectionExtendedLayout.TabIndex = 0
        Me.SelectDescriptionSourceSectionExtendedLayout.Text = "SelectDescriptionSourceSectionExtendedLayout"
        Me.SelectDescriptionSourceSectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'lblSelectSource
        '
        Me.lblSelectSource.Location = New System.Drawing.Point(2, 2)
        Me.lblSelectSource.MinimumSize = New System.Drawing.Size(0, 22)
        Me.lblSelectSource.Name = "lblSelectSource"
        Me.lblSelectSource.Size = New System.Drawing.Size(254, 22)
        Me.lblSelectSource.StyleController = Me.SelectDescriptionSourceSectionExtendedLayout
        Me.lblSelectSource.TabIndex = 5
        Me.lblSelectSource.Text = "Please select the required Description Source options"
        '
        'rgSelectSource1
        '
        Me.ExtendControlProperty.SetHelpText(Me.rgSelectSource1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.rgSelectSource1.Location = New System.Drawing.Point(2, 28)
        Me.rgSelectSource1.MaximumSize = New System.Drawing.Size(0, 60)
        Me.rgSelectSource1.MinimumSize = New System.Drawing.Size(0, 60)
        Me.rgSelectSource1.Name = "rgSelectSource1"
        Me.rgSelectSource1.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("1", "Source1Option1"), New DevExpress.XtraEditors.Controls.RadioGroupItem("2", "Source1Option2")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rgSelectSource1, System.Drawing.Color.Empty)
        Me.rgSelectSource1.Size = New System.Drawing.Size(386, 60)
        Me.rgSelectSource1.StyleController = Me.SelectDescriptionSourceSectionExtendedLayout
        Me.rgSelectSource1.TabIndex = 6
        Me.ExtendControlProperty.SetTextDisplay(Me.rgSelectSource1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'rgSelectSource2
        '
        Me.ExtendControlProperty.SetHelpText(Me.rgSelectSource2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.rgSelectSource2.Location = New System.Drawing.Point(2, 92)
        Me.rgSelectSource2.MaximumSize = New System.Drawing.Size(0, 85)
        Me.rgSelectSource2.MinimumSize = New System.Drawing.Size(0, 85)
        Me.rgSelectSource2.Name = "rgSelectSource2"
        Me.rgSelectSource2.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("1", "Source2Option1"), New DevExpress.XtraEditors.Controls.RadioGroupItem("2", "Source2Option2"), New DevExpress.XtraEditors.Controls.RadioGroupItem("3", "Source2Option3")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.rgSelectSource2, System.Drawing.Color.Empty)
        Me.rgSelectSource2.Size = New System.Drawing.Size(386, 85)
        Me.rgSelectSource2.StyleController = Me.SelectDescriptionSourceSectionExtendedLayout
        Me.rgSelectSource2.TabIndex = 7
        Me.ExtendControlProperty.SetTextDisplay(Me.rgSelectSource2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'SelectDescriptionSourceSectionLayoutGroup
        '
        Me.SelectDescriptionSourceSectionLayoutGroup.CustomizationFormText = "SelectDescriptionSourceSectionLayoutGroup"
        Me.SelectDescriptionSourceSectionLayoutGroup.GroupBordersVisible = False
        Me.SelectDescriptionSourceSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlSelectSource1, Me.LayoutControlSelectSource2, Me.LayoutControlItem1, Me.LayoutControlItem3})
        Me.SelectDescriptionSourceSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.SelectDescriptionSourceSectionLayoutGroup.Name = "SelectDescriptionSourceSectionLayoutGroup"
        Me.SelectDescriptionSourceSectionLayoutGroup.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.SelectDescriptionSourceSectionLayoutGroup.Size = New System.Drawing.Size(390, 239)
        Me.SelectDescriptionSourceSectionLayoutGroup.Text = "SelectDescriptionSourceSectionLayoutGroup"
        Me.SelectDescriptionSourceSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.lblSelectSource
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(390, 26)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlSelectSource1
        '
        Me.LayoutControlSelectSource1.Control = Me.rgSelectSource1
        Me.LayoutControlSelectSource1.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlSelectSource1.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlSelectSource1.Name = "LayoutControlSelectSource1"
        Me.LayoutControlSelectSource1.Size = New System.Drawing.Size(390, 64)
        Me.LayoutControlSelectSource1.Text = "LayoutControlSelectSource1"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlSelectSource1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlSelectSource1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlSelectSource1.TextToControlDistance = 0
        Me.LayoutControlSelectSource1.TextVisible = False
        '
        'LayoutControlSelectSource2
        '
        Me.LayoutControlSelectSource2.Control = Me.rgSelectSource2
        Me.LayoutControlSelectSource2.CustomizationFormText = "LayoutControlSelectSource2"
        Me.LayoutControlSelectSource2.Location = New System.Drawing.Point(0, 90)
        Me.LayoutControlSelectSource2.Name = "LayoutControlSelectSource2"
        Me.LayoutControlSelectSource2.Size = New System.Drawing.Size(390, 89)
        Me.LayoutControlSelectSource2.Text = "LayoutControlSelectSource2"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlSelectSource2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlSelectSource2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlSelectSource2.TextToControlDistance = 0
        Me.LayoutControlSelectSource2.TextVisible = False
        '
        'lblWeightUnits
        '
        Me.lblWeightUnits.Location = New System.Drawing.Point(2, 181)
        Me.lblWeightUnits.MaximumSize = New System.Drawing.Size(0, 22)
        Me.lblWeightUnits.MinimumSize = New System.Drawing.Size(150, 22)
        Me.lblWeightUnits.Name = "lblWeightUnits"
        Me.lblWeightUnits.Size = New System.Drawing.Size(150, 22)
        Me.lblWeightUnits.StyleController = Me.SelectDescriptionSourceSectionExtendedLayout
        Me.lblWeightUnits.TabIndex = 8
        Me.lblWeightUnits.Text = "Please select the Weight Units"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.lblWeightUnits
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 179)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(154, 60)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'cbeWeightUnits
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeWeightUnits, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeWeightUnits.Location = New System.Drawing.Point(156, 181)
        Me.cbeWeightUnits.MaximumSize = New System.Drawing.Size(137, 22)
        Me.cbeWeightUnits.Name = "cbeWeightUnits"
        Me.cbeWeightUnits.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeWeightUnits.Properties.Appearance.Options.UseFont = True
        Me.cbeWeightUnits.Properties.AutoHeight = False
        Me.cbeWeightUnits.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeWeightUnits.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Pounds (lbs)", "L", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Kilograms (Kg)", "K", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeWeightUnits, System.Drawing.Color.Empty)
        Me.cbeWeightUnits.Size = New System.Drawing.Size(137, 22)
        Me.cbeWeightUnits.StyleController = Me.SelectDescriptionSourceSectionExtendedLayout
        Me.cbeWeightUnits.TabIndex = 9
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeWeightUnits, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.cbeWeightUnits
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(154, 179)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(236, 60)
        Me.LayoutControlItem3.Text = "LayoutControlItem3"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem3, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'SelectDescriptionSourceSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.SelectDescriptionSourceSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "SelectDescriptionSourceSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SelectDescriptionSourceSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SelectDescriptionSourceSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SelectDescriptionSourceSectionExtendedLayout.ResumeLayout(False)
        CType(Me.rgSelectSource1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rgSelectSource2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SelectDescriptionSourceSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlSelectSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlSelectSource2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeWeightUnits.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents SelectDescriptionSourceSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents SelectDescriptionSourceSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents SelectDescriptionSourceSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents lblSelectSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents rgSelectSource1 As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents LayoutControlItem2 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlSelectSource1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents rgSelectSource2 As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents LayoutControlSelectSource2 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents lblWeightUnits As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents cbeWeightUnits As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents LayoutControlItem3 As Interprise.Presentation.Base.ExtendedLayoutControlItem

End Class
#End Region

