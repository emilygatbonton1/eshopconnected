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

#Region " InventoryEBaySettingsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryEBaySettingsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryEBaySettingsSection))
        Me.InventoryEBaySettingsSectionGateway = Me.ImportExportDataset
        Me.InventoryEBaySettingsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.PanelControlDummy = New DevExpress.XtraEditors.PanelControl()
        Me.lblDevelopment = New DevExpress.XtraEditors.LabelControl()
        Me.lblActivate = New DevExpress.XtraEditors.LabelControl()
        Me.InventoryEBaySettingsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.PanelPublishOnEBay = New DevExpress.XtraEditors.PanelControl()
        Me.lblSelectEBayCountry = New DevExpress.XtraEditors.LabelControl()
        Me.cbeSelectEBayCountry = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.btnWizard = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelDescription = New DevExpress.XtraEditors.LabelControl()
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit()
        Me.LabelProductName = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditProductName = New DevExpress.XtraEditors.TextEdit()
        Me.CheckEditPublishOnEBay = New DevExpress.XtraEditors.CheckEdit()
        Me.LabelMatrixItem = New System.Windows.Forms.Label()
        Me.LabelMatrixGroupItem = New System.Windows.Forms.Label()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryEBaySettingsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryEBaySettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventoryEBaySettingsSectionExtendedLayout.SuspendLayout()
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControlDummy.SuspendLayout()
        CType(Me.InventoryEBaySettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelPublishOnEBay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPublishOnEBay.SuspendLayout()
        CType(Me.cbeSelectEBayCountry.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditPublishOnEBay.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventoryEBaySettingsSectionGateway
        '
        Me.InventoryEBaySettingsSectionGateway.DataSetName = "InventoryEBaySettingsSectionDataset"
        Me.InventoryEBaySettingsSectionGateway.Instantiate = False
        Me.InventoryEBaySettingsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventoryEBaySettingsSectionExtendedLayout
        '
        Me.InventoryEBaySettingsSectionExtendedLayout.Controls.Add(Me.PanelControlDummy)
        Me.InventoryEBaySettingsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventoryEBaySettingsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventoryEBaySettingsSectionExtendedLayout.Name = "InventoryEBaySettingsSectionExtendedLayout"
        Me.InventoryEBaySettingsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventoryEBaySettingsSectionExtendedLayout.Root = Me.InventoryEBaySettingsSectionLayoutGroup
        Me.InventoryEBaySettingsSectionExtendedLayout.Size = New System.Drawing.Size(974, 497)
        Me.InventoryEBaySettingsSectionExtendedLayout.TabIndex = 0
        Me.InventoryEBaySettingsSectionExtendedLayout.Text = "InventoryEBaySettingsSectionExtendedLayout"
        '
        'PanelControlDummy
        '
        Me.PanelControlDummy.Controls.Add(Me.lblDevelopment)
        Me.PanelControlDummy.Controls.Add(Me.lblActivate)
        Me.PanelControlDummy.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControlDummy.Location = New System.Drawing.Point(2, 2)
        Me.PanelControlDummy.Name = "PanelControlDummy"
        Me.PanelControlDummy.Size = New System.Drawing.Size(964, 487)
        Me.PanelControlDummy.TabIndex = 5
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
        'InventoryEBaySettingsSectionLayoutGroup
        '
        Me.InventoryEBaySettingsSectionLayoutGroup.CustomizationFormText = "InventoryEBaySettingsSectionLayoutGroup"
        Me.InventoryEBaySettingsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventoryEBaySettingsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventoryEBaySettingsSectionLayoutGroup.Name = "InventoryEBaySettingsSectionLayoutGroup"
        Me.InventoryEBaySettingsSectionLayoutGroup.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.InventoryEBaySettingsSectionLayoutGroup.Size = New System.Drawing.Size(974, 497)
        Me.InventoryEBaySettingsSectionLayoutGroup.Text = "InventoryEBaySettingsSectionLayoutGroup"
        Me.InventoryEBaySettingsSectionLayoutGroup.TextVisible = False
        '
        'PanelPublishOnEBay
        '
        Me.PanelPublishOnEBay.Controls.Add(Me.lblSelectEBayCountry)
        Me.PanelPublishOnEBay.Controls.Add(Me.cbeSelectEBayCountry)
        Me.PanelPublishOnEBay.Controls.Add(Me.btnWizard)
        Me.PanelPublishOnEBay.Controls.Add(Me.LabelDescription)
        Me.PanelPublishOnEBay.Controls.Add(Me.MemoEditDescription)
        Me.PanelPublishOnEBay.Controls.Add(Me.LabelProductName)
        Me.PanelPublishOnEBay.Controls.Add(Me.TextEditProductName)
        Me.PanelPublishOnEBay.Controls.Add(Me.CheckEditPublishOnEBay)
        Me.PanelPublishOnEBay.Controls.Add(Me.LabelMatrixItem)
        Me.PanelPublishOnEBay.Controls.Add(Me.LabelMatrixGroupItem)
        Me.PanelPublishOnEBay.Location = New System.Drawing.Point(3, 3)
        Me.PanelPublishOnEBay.Name = "PanelPublishOnEBay"
        Me.PanelPublishOnEBay.Size = New System.Drawing.Size(968, 491)
        Me.PanelPublishOnEBay.TabIndex = 5
        '
        'lblSelectEBayCountry
        '
        Me.lblSelectEBayCountry.Location = New System.Drawing.Point(5, 9)
        Me.lblSelectEBayCountry.Name = "lblSelectEBayCountry"
        Me.lblSelectEBayCountry.Size = New System.Drawing.Size(98, 13)
        Me.lblSelectEBayCountry.TabIndex = 67
        Me.lblSelectEBayCountry.Text = "Select eBay Country"
        '
        'cbeSelectEBayCountry
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeSelectEBayCountry, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSelectEBayCountry.Location = New System.Drawing.Point(109, 5)
        Me.cbeSelectEBayCountry.Name = "cbeSelectEBayCountry"
        Me.cbeSelectEBayCountry.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeSelectEBayCountry.Properties.Appearance.Options.UseBackColor = True
        Me.cbeSelectEBayCountry.Properties.AutoHeight = False
        Me.cbeSelectEBayCountry.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.cbeSelectEBayCountry, False)
        Me.cbeSelectEBayCountry.Size = New System.Drawing.Size(184, 22)
        Me.cbeSelectEBayCountry.TabIndex = 66
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSelectEBayCountry, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnWizard
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnWizard, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnWizard.Location = New System.Drawing.Point(228, 91)
        Me.btnWizard.Name = "btnWizard"
        Me.btnWizard.Size = New System.Drawing.Size(75, 23)
        Me.btnWizard.TabIndex = 65
        Me.btnWizard.Text = "Wizard"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnWizard, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelDescription
        '
        Me.LabelDescription.Enabled = False
        Me.LabelDescription.Location = New System.Drawing.Point(624, 37)
        Me.LabelDescription.Name = "LabelDescription"
        Me.LabelDescription.Size = New System.Drawing.Size(53, 13)
        Me.LabelDescription.TabIndex = 29
        Me.LabelDescription.Text = "Description"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditDescription.Location = New System.Drawing.Point(685, 32)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.MemoEditDescription.Properties.Appearance.Options.UseBackColor = True
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.MemoEditDescription, False)
        Me.MemoEditDescription.Size = New System.Drawing.Size(272, 96)
        Me.MemoEditDescription.TabIndex = 28
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelProductName
        '
        Me.LabelProductName.Enabled = False
        Me.LabelProductName.Location = New System.Drawing.Point(610, 9)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New System.Drawing.Size(67, 13)
        Me.LabelProductName.TabIndex = 27
        Me.LabelProductName.Text = "Product Name"
        '
        'TextEditProductName
        '
        Me.TextEditProductName.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditProductName.Location = New System.Drawing.Point(685, 4)
        Me.TextEditProductName.Name = "TextEditProductName"
        Me.TextEditProductName.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.TextEditProductName.Properties.Appearance.Options.UseBackColor = True
        Me.TextEditProductName.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.TextEditProductName, False)
        Me.TextEditProductName.Size = New System.Drawing.Size(272, 22)
        Me.TextEditProductName.TabIndex = 26
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditProductName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'CheckEditPublishOnEBay
        '
        Me.ExtendControlProperty.SetHelpText(Me.CheckEditPublishOnEBay, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CheckEditPublishOnEBay.Location = New System.Drawing.Point(299, 5)
        Me.CheckEditPublishOnEBay.Name = "CheckEditPublishOnEBay"
        Me.CheckEditPublishOnEBay.Properties.AutoHeight = False
        Me.CheckEditPublishOnEBay.Properties.Caption = "Publish on this eBay Country"
        Me.CheckEditPublishOnEBay.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.CheckEditPublishOnEBay, False)
        Me.CheckEditPublishOnEBay.Size = New System.Drawing.Size(165, 22)
        Me.CheckEditPublishOnEBay.TabIndex = 3
        Me.ExtendControlProperty.SetTextDisplay(Me.CheckEditPublishOnEBay, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelMatrixItem
        '
        Me.LabelMatrixItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Location = New System.Drawing.Point(530, 170)
        Me.LabelMatrixItem.Name = "LabelMatrixItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixItem, False)
        Me.LabelMatrixItem.Size = New System.Drawing.Size(147, 65)
        Me.LabelMatrixItem.TabIndex = 64
        Me.LabelMatrixItem.Text = "This is a Matrix Item.  " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Browse Node" & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "are set from the Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Group Item."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixItem.Visible = False
        '
        'LabelMatrixGroupItem
        '
        Me.LabelMatrixGroupItem.AutoSize = True
        Me.ExtendControlProperty.SetHelpText(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Location = New System.Drawing.Point(530, 170)
        Me.LabelMatrixGroupItem.Name = "LabelMatrixGroupItem"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.LabelMatrixGroupItem, False)
        Me.LabelMatrixGroupItem.Size = New System.Drawing.Size(141, 78)
        Me.LabelMatrixGroupItem.TabIndex = 63
        Me.LabelMatrixGroupItem.Text = "This is a Matrix Group Item." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "As such, the Product Name," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description and Browse " & _
            "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Node are applied to every " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Matrix Item in the Matrix " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Group."
        Me.ExtendControlProperty.SetTextDisplay(Me.LabelMatrixGroupItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LabelMatrixGroupItem.Visible = False
        '
        'InventoryEBaySettingsSection
        '
        Me.Controls.Add(Me.PanelPublishOnEBay)
        Me.Controls.Add(Me.InventoryEBaySettingsSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventoryEBaySettingsSection"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me, False)
        Me.Size = New System.Drawing.Size(974, 497)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryEBaySettingsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryEBaySettingsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventoryEBaySettingsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.PanelControlDummy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControlDummy.ResumeLayout(False)
        Me.PanelControlDummy.PerformLayout()
        CType(Me.InventoryEBaySettingsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelPublishOnEBay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPublishOnEBay.ResumeLayout(False)
        Me.PanelPublishOnEBay.PerformLayout()
        CType(Me.cbeSelectEBayCountry.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditProductName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditPublishOnEBay.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventoryEBaySettingsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventoryEBaySettingsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventoryEBaySettingsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PanelPublishOnEBay As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelProductName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditProductName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CheckEditPublishOnEBay As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LabelMatrixItem As System.Windows.Forms.Label
    Friend WithEvents LabelMatrixGroupItem As System.Windows.Forms.Label
    Friend WithEvents PanelControlDummy As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblDevelopment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblActivate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnWizard As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblSelectEBayCountry As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSelectEBayCountry As DevExpress.XtraEditors.ImageComboBoxEdit

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

