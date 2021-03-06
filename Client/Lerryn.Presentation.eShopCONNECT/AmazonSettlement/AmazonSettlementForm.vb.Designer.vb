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

Namespace AmazonSettlement
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AmazonSettlementForm
	Inherits Interprise.Presentation.Base.BaseRibbonForm
	Implements Interprise.Extendable.Base.Presentation.Generic.IBaseFormInterface

    'Form overrides dispose to clean up the component list.
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
            CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.RepositoryNumKey, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.MenuApplication, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PanelBody, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DefaultBarAndDockingController.Controller, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BarAndDockingController, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PopupMenuSave, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PopupMenuExport, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.skins, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'RibbonControl
            '
            Me.PropertyExtender.SetAssignActionfrmRbCntrl(Me.RibbonControl, "")
            Me.RibbonControl.ExpandCollapseItem.Id = 0
            Me.RibbonControl.ExpandCollapseItem.Name = ""
            Me.PropertyExtender.SetHelpText(Me.RibbonControl, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.PropertyExtender.SetReadOnlyBackColor(Me.RibbonControl, System.Drawing.Color.Empty)
            Me.PropertyExtender.SetSelectNextControlOnEnterKey(Me.RibbonControl, False)
            Me.RibbonControl.Size = New System.Drawing.Size(954, 144)
            Me.PropertyExtender.SetTextDisplay(Me.RibbonControl, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'StatusBar
            '
            Me.StatusBar.Location = New System.Drawing.Point(0, 734)
            Me.StatusBar.Size = New System.Drawing.Size(954, 31)
            '
            'PanelBody
            '
            Me.PanelBody.Appearance.BackColor = System.Drawing.Color.Transparent
            Me.PanelBody.Appearance.Options.UseBackColor = True
            Me.PanelBody.Plugin = "Lerryn.Presentation.eShopCONNECT.AmazonSettlementSectionContainer"
            Me.PanelBody.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
            Me.PanelBody.Size = New System.Drawing.Size(954, 590)
            Me.PanelBody.Text = " "
            '
            'DefaultBarAndDockingController
            '
            '
            'BarAndDockingController
            '
            Me.BarAndDockingController.PropertiesBar.AllowLinkLighting = False
            '
            'AmazonSettlementForm
            '
            Me.Appearance.ForeColor = System.Drawing.Color.LightSkyBlue
            Me.Appearance.Options.UseForeColor = True
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(954, 765)
            Me.Name = "AmazonSettlementForm"
            Me.Text = "AmazonSettlementForm"
            CType(Me.RibbonControl, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.RepositoryNumKey, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.MenuApplication, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PanelBody, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DefaultBarAndDockingController.Controller, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BarAndDockingController, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PopupMenuSave, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PopupMenuExport, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.skins, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub



#Region "Interprise Plugin Initialization"
#Region "PanelBody_InitializePlugin"

        ''' <summary>
        ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
        ''' </summary>
        ''' <param name="sender">The object instance that invoked the event</param>
        ''' <param name="e">The event argument passed by the sender</param>
        ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

        Protected Overrides Sub PanelBody_InitializePlugin(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PanelBody.DiscoverPlugin(GetType(AmazonSettlementSectionContainer)), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        End Sub

#End Region


#Region "PanelBodyPluginInstance"
        ''' <summary>
        ''' Returns the instance of plugin assigned to the plugin container.
        ''' </summary>
        ''' <value>The instance of plugin assigned to the plugin container</value>
        ''' <returns>Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface</returns>
        ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

        Protected Overridable ReadOnly Property PanelBodyPluginInstance() As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
            Get
                Return CType(PanelBody.PluginInstance, Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
            End Get
        End Property
#End Region
#End Region




End Class
End Namespace

