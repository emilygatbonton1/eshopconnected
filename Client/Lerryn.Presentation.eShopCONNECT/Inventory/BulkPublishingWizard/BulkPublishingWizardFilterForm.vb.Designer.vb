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

#Region " BulkPublishingWizardFilterForm "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BulkPublishingWizardFilterForm
    Inherits Interprise.Presentation.Base.BaseForm
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
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(665, 7)
        Me.btnCancel.Visible = False
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(595, 7)
        '
        'Footer
        '
        Me.Footer.Location = New System.Drawing.Point(0, 535)
        Me.Footer.Visible = True
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
        Me.PanelBody.Plugin = "Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection"
        Me.PanelBody.PluginInterface = "Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface"
        Me.PanelBody.Size = New System.Drawing.Size(854, 418)
        '
        'PictureLogo
        '
        Me.PictureLogo.Location = New System.Drawing.Point(790, 0)
        '
        'BulkPublishingWizardFilterForm
        '
        Me.Appearance.Font = New System.Drawing.Font("Lato", 8.0!)
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(854, 592)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "BulkPublishingWizardFilterForm"
        Me.Text = "Bulk Publishing Wizard Additional Item Filter"
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
        Dim plugin As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface = CType(PanelBody.DiscoverPlugin(GetType(Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection)), Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface)
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyCategoryFilter = Not String.IsNullOrEmpty(m_CategoryFilter)
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).CategoriesToFilter = m_CategoryFilter
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyDepartmentFilter = Not String.IsNullOrEmpty(m_DepartmentFilter)
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).DepartmentsToFilter = m_DepartmentFilter
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyManufacturerFilter = Not String.IsNullOrEmpty(m_ManufacturerFilter)
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ManufacturerToFilter = m_ManufacturerFilter
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyStatusFilter = Not String.IsNullOrEmpty(m_StatusFilter)
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).StatusToFilter = m_StatusFilter
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplySupplierFilter = Not String.IsNullOrEmpty(m_SupplierFilter)
        DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).SupplierToFilter = m_SupplierFilter
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
#End Region

