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

#Region " AmazonSettlementSectionContainer "
Namespace AmazonSettlement
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AmazonSettlementSectionContainer
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AmazonSettlementSectionContainer))
            Me.AmazonSettlementSectionContainerGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
            Me.AmazonSettlementExtendedLayoutControl = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
            Me.AmazonSettlementPluginContainerControl = New Interprise.Presentation.Base.PluginContainerControl()
            Me.AmazonSettlementLayoutControlGroup = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.AmazonSettlementLayoutControlItem = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmazonSettlementSectionContainerGateway, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmazonSettlementExtendedLayoutControl, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.AmazonSettlementExtendedLayoutControl.SuspendLayout()
            CType(Me.AmazonSettlementPluginContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmazonSettlementLayoutControlGroup, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmazonSettlementLayoutControlItem, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ImageCollectionContextMenu
            '
            Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
            '
            'AmazonSettlementSectionContainerGateway
            '
            Me.AmazonSettlementSectionContainerGateway.DataSetName = "AmazonSettlementSectionContainerDataset"
            Me.AmazonSettlementSectionContainerGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
            'AmazonSettlementExtendedLayoutControl
            '
            Me.AmazonSettlementExtendedLayoutControl.Controls.Add(Me.AmazonSettlementPluginContainerControl)
            Me.AmazonSettlementExtendedLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.AmazonSettlementExtendedLayoutControl.Location = New System.Drawing.Point(0, 0)
            Me.AmazonSettlementExtendedLayoutControl.Name = "AmazonSettlementExtendedLayoutControl"
            Me.AmazonSettlementExtendedLayoutControl.Root = Me.AmazonSettlementLayoutControlGroup
            Me.AmazonSettlementExtendedLayoutControl.Size = New System.Drawing.Size(954, 590)
            Me.AmazonSettlementExtendedLayoutControl.TabIndex = 0
            Me.AmazonSettlementExtendedLayoutControl.Text = "AmazonSettlementExtendedLayoutControl"
            '
            'AmazonSettlementPluginContainerControl
            '
            Me.AmazonSettlementPluginContainerControl.AppearanceCaption.Options.UseTextOptions = True
            Me.AmazonSettlementPluginContainerControl.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            Me.AmazonSettlementPluginContainerControl.BaseLayoutControl = Nothing
            Me.AmazonSettlementPluginContainerControl.ContextMenuButtonCaption = Nothing
            Me.AmazonSettlementPluginContainerControl.CurrentControl = Nothing
            Me.AmazonSettlementPluginContainerControl.EditorsHeight = 0
            Me.AmazonSettlementPluginContainerControl.GroupContextMenu = Nothing
            Me.AmazonSettlementPluginContainerControl.HelpCode = Nothing
            Me.AmazonSettlementPluginContainerControl.LayoutMode = False
            Me.AmazonSettlementPluginContainerControl.Location = New System.Drawing.Point(12, 12)
            Me.AmazonSettlementPluginContainerControl.Name = "AmazonSettlementPluginContainerControl"
            Me.AmazonSettlementPluginContainerControl.Plugin = "Lerryn.Presentation.eShopCONNECT.AmazonSettlement.AmazonSettlementSection"
            Me.AmazonSettlementPluginContainerControl.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
            Me.AmazonSettlementPluginContainerControl.PluginManagerButton = Nothing
            Me.AmazonSettlementPluginContainerControl.SearchPluginButton = Nothing
            Me.AmazonSettlementPluginContainerControl.ShowCaption = False
            Me.AmazonSettlementPluginContainerControl.Size = New System.Drawing.Size(930, 566)
            Me.AmazonSettlementPluginContainerControl.TabIndex = 4
            Me.AmazonSettlementPluginContainerControl.Text = "AmazonSettlementPluginContainerControl"
            '
            'AmazonSettlementLayoutControlGroup
            '
            Me.AmazonSettlementLayoutControlGroup.CustomizationFormText = "AmazonSettlementLayoutControlGroup"
            Me.AmazonSettlementLayoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
            Me.AmazonSettlementLayoutControlGroup.GroupBordersVisible = False
            Me.AmazonSettlementLayoutControlGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.AmazonSettlementLayoutControlItem})
            Me.AmazonSettlementLayoutControlGroup.Location = New System.Drawing.Point(0, 0)
            Me.AmazonSettlementLayoutControlGroup.Name = "AmazonSettlementLayoutControlGroup"
            Me.AmazonSettlementLayoutControlGroup.Size = New System.Drawing.Size(954, 590)
            Me.AmazonSettlementLayoutControlGroup.Text = "AmazonSettlementLayoutControlGroup"
            Me.AmazonSettlementLayoutControlGroup.TextVisible = False
            '
            'AmazonSettlementLayoutControlItem
            '
            Me.AmazonSettlementLayoutControlItem.Control = Me.AmazonSettlementPluginContainerControl
            Me.AmazonSettlementLayoutControlItem.CustomizationFormText = "AmazonSettlementLayoutControlItem"
            Me.AmazonSettlementLayoutControlItem.Location = New System.Drawing.Point(0, 0)
            Me.AmazonSettlementLayoutControlItem.Name = "AmazonSettlementLayoutControlItem"
            Me.AmazonSettlementLayoutControlItem.Size = New System.Drawing.Size(934, 570)
            Me.AmazonSettlementLayoutControlItem.Text = "AmazonSettlementLayoutControlItem"
            Me.ExtendControlProperty.SetTextDisplay(Me.AmazonSettlementLayoutControlItem, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.AmazonSettlementLayoutControlItem.TextSize = New System.Drawing.Size(0, 0)
            Me.AmazonSettlementLayoutControlItem.TextToControlDistance = 0
            Me.AmazonSettlementLayoutControlItem.TextVisible = False
            '
            'AmazonSettlementSectionContainer
            '
            Me.Controls.Add(Me.AmazonSettlementExtendedLayoutControl)
            Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.Name = "AmazonSettlementSectionContainer"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
            Me.Size = New System.Drawing.Size(954, 590)
            Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmazonSettlementSectionContainerGateway, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmazonSettlementExtendedLayoutControl, System.ComponentModel.ISupportInitialize).EndInit()
            Me.AmazonSettlementExtendedLayoutControl.ResumeLayout(False)
            CType(Me.AmazonSettlementPluginContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmazonSettlementLayoutControlGroup, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmazonSettlementLayoutControlItem, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
		
        Friend AmazonSettlementSectionContainerGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Friend WithEvents AmazonSettlementExtendedLayoutControl As Interprise.Presentation.Base.ExtendedLayoutControl
        Friend WithEvents AmazonSettlementPluginContainerControl As Interprise.Presentation.Base.PluginContainerControl
        Friend WithEvents AmazonSettlementLayoutControlGroup As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents AmazonSettlementLayoutControlItem As Interprise.Presentation.Base.ExtendedLayoutControlItem

#Region "Interprise Plugin Initialization"
#Region "AmazonSettlementPluginContainerControl_InitializePlugin"

        ''' <summary>
        ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
        ''' </summary>
        ''' <param name="sender">The object instance that invoked the event</param>
        ''' <param name="e">The event argument passed by the sender</param>
        ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

        Private Sub AmazonSettlementPluginContainerControl_InitializePlugin(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AmazonSettlementPluginContainerControl.InitializePlugin
            Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(AmazonSettlementPluginContainerControl.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.AmazonSettlement.AmazonSettlementSection), AmazonSettlementSectionContainerGateway, m_ImportExportConfigFacadesectionContainerFacade), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)

        End Sub

#End Region

#Region "AmazonSettlementPluginContainerControlPluginInstance"
        ''' <summary>
        ''' Returns the instance of plugin assigned to the plugin container.
        ''' </summary>
        ''' <value>The instance of plugin assigned to the plugin container</value>
        ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
        ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

        Protected Overridable ReadOnly Property AmazonSettlementPluginContainerControlPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
            Get
                Return CType(AmazonSettlementPluginContainerControl.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
            End Get
        End Property
#End Region
#End Region
End Class
End Namespace
#End Region

