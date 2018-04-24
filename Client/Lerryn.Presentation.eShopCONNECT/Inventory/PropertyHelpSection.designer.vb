'===============================================================================
' Connected Business SDK
' Copyright © 2004-2008 Interprise Solutions LLC
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

#Region " AmazonPropertyHelpSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropertyHelpSection
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PropertyHelpSection))
        Me.AmazonPropertyHelpSectionGateway = New Interprise.Framework.Base.DatasetGateway.BaseDatasetGateway
        Me.AmazonPropertyHelpSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.AmazonPropertyHelpSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup
        Me.XtraTabHelp = New DevExpress.XtraTab.XtraTabControl
        Me.LayoutItemHelp = New Interprise.Presentation.Base.ExtendedLayoutControlItem
        Me.XtraTabPageDescription = New DevExpress.XtraTab.XtraTabPage
        Me.XtraTabPageAcceptedValues = New DevExpress.XtraTab.XtraTabPage
        Me.XtraTabPageExample = New DevExpress.XtraTab.XtraTabPage
        Me.XtraTabPageStatus = New DevExpress.XtraTab.XtraTabPage
        Me.MemoEditDescription = New DevExpress.XtraEditors.MemoEdit
        Me.MemoEditAcceptedValues = New DevExpress.XtraEditors.MemoEdit
        Me.MemoEditExample = New DevExpress.XtraEditors.MemoEdit
        Me.TextEditStatus = New DevExpress.XtraEditors.TextEdit
        Me.LabelStatus = New DevExpress.XtraEditors.LabelControl
        Me.MemoEditConditionality = New DevExpress.XtraEditors.MemoEdit
        Me.LabelConditionality = New DevExpress.XtraEditors.LabelControl
        CType(Me.AmazonPropertyHelpSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AmazonPropertyHelpSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AmazonPropertyHelpSectionExtendedLayout.SuspendLayout()
        CType(Me.AmazonPropertyHelpSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XtraTabHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabHelp.SuspendLayout()
        CType(Me.LayoutItemHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPageDescription.SuspendLayout()
        Me.XtraTabPageAcceptedValues.SuspendLayout()
        Me.XtraTabPageExample.SuspendLayout()
        Me.XtraTabPageStatus.SuspendLayout()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditAcceptedValues.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditExample.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditStatus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MemoEditConditionality.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AmazonPropertyHelpSectionGateway
        '
        Me.AmazonPropertyHelpSectionGateway.DataSetName = "AmazonPropertyHelpSectionDataset"
        '
        '****************************************************************************************
        '
        'BaseDatasetGateway COMPONENT DESIGNER GENERATED CODE
        'STRICTLY FOLLOW THE STEPS BELOW IN ORDER TO IMPLEMENT DATASET SHARING
        'ACCROSS MULTIPLE USER CONTROLS AND/OR WINFORM CONTROLS
        '
        'NOTE: MAKE SURE YOU HAVE A REFERENCE TO THE FRAMEWORK DATASET COMPONENT IN YOUR TOOLBOX
        '
        '1.  SWITCH TO DESIGN VIEW OF YOUR PROJECT
        '2.  ADD AN "
        'BaseDatasetGateway" COMPONENT3.  IF THIS IS A PUGIN CONTROL, SET THE "Instantiate" PROPERTY TO "False"
        '4.  IF THIS IS THE MAIN PLUGIN CONTAINER, SET THE "Instantiate" PROPERTY TO "True"
        '5.  SWITCH TO CODE VIEW OF YOUR PROJECT
        '6.  ADD THE FF. CODES BELOW AND PLACE IT OUTSIDE THIS FUNCTION
        '
        '        #Region " Private Variables "
        '            Private m_baseDataset as Interprise.Framework.Base.DatasetGateway.BaseDatasetGateway 
        '        #End Region
        '
        '        #Region " Properties "
        '
        '        #Region "BaseDataset"
        '            Public ReadOnly Property BaseDataset AS Interprise.Framework.Base.DatasetGateway.BaseDatasetGateway 
        '                Get
        '                    Return Me.m_baseDataset
        '                End Get
        '            End Property
        '        #End Region
        '
        '        #End Region
        '
        '****************************************************************************************
        '
        '
        'AmazonPropertyHelpSectionExtendedLayout
        '
        Me.AmazonPropertyHelpSectionExtendedLayout.Controls.Add(Me.XtraTabHelp)
        Me.AmazonPropertyHelpSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AmazonPropertyHelpSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.AmazonPropertyHelpSectionExtendedLayout.Name = "AmazonPropertyHelpSectionExtendedLayout"
        Me.AmazonPropertyHelpSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.AmazonPropertyHelpSectionExtendedLayout.Root = Me.AmazonPropertyHelpSectionLayoutGroup
        Me.AmazonPropertyHelpSectionExtendedLayout.Size = New System.Drawing.Size(492, 320)
        Me.AmazonPropertyHelpSectionExtendedLayout.TabIndex = 0
        Me.AmazonPropertyHelpSectionExtendedLayout.Text = "AmazonPropertyHelpSectionExtendedLayout"
        '
        'AmazonPropertyHelpSectionLayoutGroup
        '
        Me.AmazonPropertyHelpSectionLayoutGroup.CustomizationFormText = "AmazonPropertyHelpSectionLayoutGroup"
        Me.AmazonPropertyHelpSectionLayoutGroup.GroupBordersVisible = False
        Me.AmazonPropertyHelpSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutItemHelp})
        Me.AmazonPropertyHelpSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.AmazonPropertyHelpSectionLayoutGroup.Name = "AmazonPropertyHelpSectionLayoutGroup"
        Me.AmazonPropertyHelpSectionLayoutGroup.Size = New System.Drawing.Size(492, 320)
        Me.AmazonPropertyHelpSectionLayoutGroup.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.AmazonPropertyHelpSectionLayoutGroup.Text = "AmazonPropertyHelpSectionLayoutGroup"
        Me.AmazonPropertyHelpSectionLayoutGroup.TextVisible = False
        '
        'XtraTabHelp
        '
        Me.XtraTabHelp.Location = New System.Drawing.Point(4, 9)
        Me.XtraTabHelp.Name = "XtraTabHelp"
        Me.XtraTabHelp.SelectedTabPage = Me.XtraTabPageDescription
        Me.XtraTabHelp.Size = New System.Drawing.Size(485, 308)
        Me.XtraTabHelp.TabIndex = 4
        Me.XtraTabHelp.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPageDescription, Me.XtraTabPageAcceptedValues, Me.XtraTabPageExample, Me.XtraTabPageStatus})
        Me.XtraTabHelp.Text = "Property Help"
        '
        'LayoutItemHelp
        '
        Me.LayoutItemHelp.Control = Me.XtraTabHelp
        Me.LayoutItemHelp.CustomizationFormText = "LayoutItemHelp"
        Me.LayoutItemHelp.Location = New System.Drawing.Point(0, 0)
        Me.LayoutItemHelp.Name = "LayoutItemHelp"
        Me.LayoutItemHelp.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutItemHelp.Size = New System.Drawing.Size(492, 320)
        Me.LayoutItemHelp.Text = "LayoutItemHelp"
        Me.LayoutItemHelp.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemHelp, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemHelp.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutItemHelp.TextSize = New System.Drawing.Size(0, 0)
        '
        'XtraTabPageDescription
        '
        Me.XtraTabPageDescription.Controls.Add(Me.MemoEditDescription)
        Me.XtraTabPageDescription.Name = "XtraTabPageDescription"
        Me.XtraTabPageDescription.Size = New System.Drawing.Size(476, 277)
        Me.XtraTabPageDescription.Text = "Property Description"
        '
        'XtraTabPageAcceptedValues
        '
        Me.XtraTabPageAcceptedValues.Controls.Add(Me.MemoEditAcceptedValues)
        Me.XtraTabPageAcceptedValues.Name = "XtraTabPageAcceptedValues"
        Me.XtraTabPageAcceptedValues.Size = New System.Drawing.Size(476, 277)
        Me.XtraTabPageAcceptedValues.Text = "Accepted Values"
        '
        'XtraTabPageExample
        '
        Me.XtraTabPageExample.Controls.Add(Me.MemoEditExample)
        Me.XtraTabPageExample.Name = "XtraTabPageExample"
        Me.XtraTabPageExample.Size = New System.Drawing.Size(476, 277)
        Me.XtraTabPageExample.Text = "Example"
        '
        'XtraTabPageStatus
        '
        Me.XtraTabPageStatus.Controls.Add(Me.LabelConditionality)
        Me.XtraTabPageStatus.Controls.Add(Me.MemoEditConditionality)
        Me.XtraTabPageStatus.Controls.Add(Me.LabelStatus)
        Me.XtraTabPageStatus.Controls.Add(Me.TextEditStatus)
        Me.XtraTabPageStatus.Name = "XtraTabPageStatus"
        Me.XtraTabPageStatus.Size = New System.Drawing.Size(476, 277)
        Me.XtraTabPageStatus.Text = "Status"
        '
        'MemoEditDescription
        '
        Me.MemoEditDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditDescription.Location = New System.Drawing.Point(0, 0)
        Me.MemoEditDescription.Name = "MemoEditDescription"
        Me.MemoEditDescription.Size = New System.Drawing.Size(476, 277)
        Me.MemoEditDescription.TabIndex = 0
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'MemoEditAcceptedValues
        '
        Me.MemoEditAcceptedValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditAcceptedValues, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditAcceptedValues.Location = New System.Drawing.Point(0, 0)
        Me.MemoEditAcceptedValues.Name = "MemoEditAcceptedValues"
        Me.MemoEditAcceptedValues.Size = New System.Drawing.Size(476, 277)
        Me.MemoEditAcceptedValues.TabIndex = 0
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditAcceptedValues, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'MemoEditExample
        '
        Me.MemoEditExample.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditExample, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditExample.Location = New System.Drawing.Point(0, 0)
        Me.MemoEditExample.Name = "MemoEditExample"
        Me.MemoEditExample.Size = New System.Drawing.Size(476, 277)
        Me.MemoEditExample.TabIndex = 0
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditExample, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TextEditStatus
        '
        Me.ExtendControlProperty.SetHelpText(Me.TextEditStatus, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditStatus.Location = New System.Drawing.Point(51, 11)
        Me.TextEditStatus.Name = "TextEditStatus"
        Me.TextEditStatus.Properties.AutoHeight = False
        Me.TextEditStatus.Size = New System.Drawing.Size(124, 22)
        Me.TextEditStatus.TabIndex = 0
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditStatus, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelStatus
        '
        Me.LabelStatus.Location = New System.Drawing.Point(3, 15)
        Me.LabelStatus.Name = "LabelStatus"
        Me.LabelStatus.Size = New System.Drawing.Size(31, 13)
        Me.LabelStatus.TabIndex = 1
        Me.LabelStatus.Text = "Status"
        '
        'MemoEditConditionality
        '
        Me.ExtendControlProperty.SetHelpText(Me.MemoEditConditionality, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.MemoEditConditionality.Location = New System.Drawing.Point(51, 50)
        Me.MemoEditConditionality.Name = "MemoEditConditionality"
        Me.MemoEditConditionality.Size = New System.Drawing.Size(422, 224)
        Me.MemoEditConditionality.TabIndex = 2
        Me.ExtendControlProperty.SetTextDisplay(Me.MemoEditConditionality, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'LabelConditionality
        '
        Me.LabelConditionality.Location = New System.Drawing.Point(3, 53)
        Me.LabelConditionality.Name = "LabelConditionality"
        Me.LabelConditionality.Size = New System.Drawing.Size(32, 13)
        Me.LabelConditionality.TabIndex = 3
        Me.LabelConditionality.Text = "Details"
        '
        'AmazonPropertyHelpSection
        '
        Me.Controls.Add(Me.AmazonPropertyHelpSectionExtendedLayout)
        Me.Name = "AmazonPropertyHelpSection"
        Me.Size = New System.Drawing.Size(492, 320)
        CType(Me.AmazonPropertyHelpSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AmazonPropertyHelpSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AmazonPropertyHelpSectionExtendedLayout.ResumeLayout(False)
        CType(Me.AmazonPropertyHelpSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XtraTabHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabHelp.ResumeLayout(False)
        CType(Me.LayoutItemHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPageDescription.ResumeLayout(False)
        Me.XtraTabPageAcceptedValues.ResumeLayout(False)
        Me.XtraTabPageExample.ResumeLayout(False)
        Me.XtraTabPageStatus.ResumeLayout(False)
        Me.XtraTabPageStatus.PerformLayout()
        CType(Me.MemoEditDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditAcceptedValues.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditExample.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditStatus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MemoEditConditionality.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents AmazonPropertyHelpSectionGateway As Interprise.Framework.Base.DatasetGateway.BaseDatasetGateway
    Friend WithEvents AmazonPropertyHelpSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents AmazonPropertyHelpSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents XtraTabHelp As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents XtraTabPageDescription As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents XtraTabPageAcceptedValues As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents LayoutItemHelp As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents MemoEditDescription As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents MemoEditAcceptedValues As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents XtraTabPageExample As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents XtraTabPageStatus As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents MemoEditExample As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelConditionality As DevExpress.XtraEditors.LabelControl
    Friend WithEvents MemoEditConditionality As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TextEditStatus As DevExpress.XtraEditors.TextEdit

End Class
#End Region
