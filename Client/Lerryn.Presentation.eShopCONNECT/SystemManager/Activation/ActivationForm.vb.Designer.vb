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

Namespace SystemManager.Activation
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ActivationForm
        Inherits Interprise.Presentation.SystemManager.NewCompanyWizard.Base.BaseForm
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
            CType(Me.Footer, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.Footer.SuspendLayout()
            CType(Me.Banner, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.Banner.SuspendLayout()
            CType(Me.RepositoryNumKey, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PanelBody, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureLogo, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Footer
            '
            Me.Footer.Location = New System.Drawing.Point(0, 535)
            '
            'btnHelp
            '
            Me.btnHelp.Appearance.Font = New System.Drawing.Font("Lato", 8.0!)
            Me.btnHelp.Appearance.Options.UseFont = True
            '
            'Banner
            '
            Me.Banner.Location = New System.Drawing.Point(0, 51)
            '
            'LabelSubHeading
            '
            Me.LabelSubHeading.Size = New System.Drawing.Size(758, 40)
            '
            'LabelHeading
            '
            Me.LabelHeading.Size = New System.Drawing.Size(772, 20)
            '
            'RepositoryNumKey
            '
            Me.RepositoryNumKey.Mask.EditMask = "###"
            Me.RepositoryNumKey.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
            '
            'PanelBody
            '
            Me.PanelBody.Location = New System.Drawing.Point(0, 117)
            Me.PanelBody.Plugin = "Lerryn.Presentation.eShopCONNECT.ActivationWizardSectionContainer"
            Me.PanelBody.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
            Me.PanelBody.Size = New System.Drawing.Size(854, 418)
            '
            'PictureLogo
            '
            Me.PictureLogo.Location = New System.Drawing.Point(790, 0)
            '
            'ActivationForm
            '
            Me.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!)
            Me.Appearance.Options.UseFont = True
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.ClientSize = New System.Drawing.Size(854, 592)
            Me.Name = "ActivationForm"
            Me.Text = "Activation Form"
            CType(Me.Footer, System.ComponentModel.ISupportInitialize).EndInit()
            Me.Footer.ResumeLayout(False)
            CType(Me.Banner, System.ComponentModel.ISupportInitialize).EndInit()
            Me.Banner.ResumeLayout(False)
            Me.Banner.PerformLayout()
            CType(Me.RepositoryNumKey, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PanelBody, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureLogo, System.ComponentModel.ISupportInitialize).EndInit()
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
            Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PanelBody.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.ActivationWizardSectionContainer)), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
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
