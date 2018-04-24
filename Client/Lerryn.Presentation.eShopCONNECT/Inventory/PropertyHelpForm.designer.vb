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

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropertyHelpForm
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
        '
        'btnHelp
        '
        Me.btnHelp.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.btnHelp.Appearance.Options.UseFont = True
        Me.btnHelp.Location = New System.Drawing.Point(6, 5)
        '
        'Banner
        '
        Me.Banner.Appearance.BackColor = System.Drawing.Color.White
        Me.Banner.Appearance.Options.UseBackColor = True
        Me.Banner.Location = New System.Drawing.Point(0, 51)
        '
        'LabelSubHeading
        '
        Me.LabelSubHeading.Size = New System.Drawing.Size(650, 40)
        '
        'LabelHeading
        '
        Me.LabelHeading.Size = New System.Drawing.Size(664, 20)
        '
        'RepositoryNumKey
        '
        Me.RepositoryNumKey.Mask.EditMask = "###"
        Me.RepositoryNumKey.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        '
        'PanelBody
        '
        Me.PanelBody.Location = New System.Drawing.Point(0, 117)
        Me.PanelBody.Size = New System.Drawing.Size(854, 418)
        '
        'PictureLogo
        '
        Me.PictureLogo.Location = New System.Drawing.Point(790, 0)
        '
        'PropertyHelpForm
        '
        Me.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(854, 592)
        Me.MaximumSize = New System.Drawing.Size(870, 630)
        Me.Name = "PropertyHelpForm"
        Me.Text = "Amazon Property Help"
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
End Class

