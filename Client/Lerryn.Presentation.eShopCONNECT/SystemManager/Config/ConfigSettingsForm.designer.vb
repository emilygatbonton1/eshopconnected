Namespace SystemManager.Config
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ConfigSettingsForm

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
            Me.RibbonControl.Size = New System.Drawing.Size(830, 144)
            Me.PropertyExtender.SetTextDisplay(Me.RibbonControl, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'RepositoryNumKey
            '
            Me.RepositoryNumKey.Mask.EditMask = "###"
            Me.RepositoryNumKey.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
            '
            'PanelBody
            '
            Me.PanelBody.Appearance.BackColor = System.Drawing.Color.White
            Me.PanelBody.Appearance.Options.UseBackColor = True
            Me.PanelBody.AppearanceCaption.Options.UseTextOptions = True
            Me.PanelBody.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            Me.PanelBody.Plugin = "Lerryn.Presentation.eShopCONNECT.ConfigSettingsListSection"
            Me.PanelBody.PluginInterface = "Interprise.Extendable.Base.Presentation.SystemManager.IBaseListDetailInterface"
            '
            'DefaultBarAndDockingController
            '
            '
            'BarAndDockingController
            '
            Me.BarAndDockingController.PropertiesBar.AllowLinkLighting = False
            '
            'ConfigSettingsForm
            '
            Me.Appearance.BackColor = System.Drawing.SystemColors.Control
            Me.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.Appearance.ForeColor = System.Drawing.Color.LightSkyBlue
            Me.Appearance.Options.UseBackColor = True
            Me.Appearance.Options.UseFont = True
            Me.Appearance.Options.UseForeColor = True
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.ClientSize = New System.Drawing.Size(830, 700)
            Me.Name = "ConfigSettingsForm"
            Me.Text = "Config Settings"
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
        Friend WithEvents LabelPluginsForInterprise As DevExpress.XtraEditors.LabelControl

#Region "Interprise Plugin Initialization"
#Region "PanelBody_AfterInitializePlugin"
        Protected Overrides Sub PanelBody_AfterInitializePlugin(ByVal sender As Object, ByVal e As System.EventArgs)
            MyBase.PanelBody_AfterInitializePlugin(sender, e)
            DirectCast(PanelBodyPluginInstance, ConfigSettingsListSection).SetGridListViewHandle(DirectCast(PanelBodyPluginInstance, ConfigSettingsListSection).listControl.gvwSearch)
            DirectCast(PanelBodyPluginInstance, ConfigSettingsListSection).InitialiseControls()
        End Sub
#End Region

#Region "PanelBody_InitializePlugin"

        ''' <summary>
        ''' Initializes the plugin assigned to the container using the Interprise plugin framework.
        ''' </summary>
        ''' <param name="sender">The object instance that invoked the event</param>
        ''' <param name="e">The event argument passed by the sender</param>
        ''' <remarks>Code generated by Interprise Solution's Plugin Initialization Code Generator. Modifying the generated code manually may cause errors. You may also lost your changes when the codes are regenerated.</remarks>

        Protected Overrides Sub PanelBody_InitializePlugin(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim plugin As Interprise.Extendable.Base.Presentation.SystemManager.IBaseListDetailInterface = CType(PanelBody.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.ConfigSettingsListSection)), Interprise.Extendable.Base.Presentation.SystemManager.IBaseListDetailInterface)
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
