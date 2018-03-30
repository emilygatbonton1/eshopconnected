<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnGetOrders = New System.Windows.Forms.Button()
        Me.btnCheckStatus = New System.Windows.Forms.Button()
        Me.btnSendFeed = New System.Windows.Forms.Button()
        Me.txtFeedContent = New System.Windows.Forms.TextBox()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SuspendLayout()
        '
        'btnGetOrders
        '
        Me.btnGetOrders.Location = New System.Drawing.Point(108, 12)
        Me.btnGetOrders.Name = "btnGetOrders"
        Me.btnGetOrders.Size = New System.Drawing.Size(75, 23)
        Me.btnGetOrders.TabIndex = 0
        Me.btnGetOrders.Text = "Get Orders"
        Me.btnGetOrders.UseVisualStyleBackColor = True
        '
        'btnCheckStatus
        '
        Me.btnCheckStatus.Location = New System.Drawing.Point(62, 171)
        Me.btnCheckStatus.Name = "btnCheckStatus"
        Me.btnCheckStatus.Size = New System.Drawing.Size(160, 23)
        Me.btnCheckStatus.TabIndex = 1
        Me.btnCheckStatus.Text = "Check Submission Status"
        Me.btnCheckStatus.UseVisualStyleBackColor = True
        '
        'btnSendFeed
        '
        Me.btnSendFeed.Location = New System.Drawing.Point(62, 56)
        Me.btnSendFeed.Name = "btnSendFeed"
        Me.btnSendFeed.Size = New System.Drawing.Size(160, 23)
        Me.btnSendFeed.TabIndex = 2
        Me.btnSendFeed.Text = "Send Feed"
        Me.btnSendFeed.UseVisualStyleBackColor = True
        '
        'txtFeedContent
        '
        Me.txtFeedContent.Location = New System.Drawing.Point(12, 85)
        Me.txtFeedContent.Multiline = True
        Me.txtFeedContent.Name = "txtFeedContent"
        Me.txtFeedContent.Size = New System.Drawing.Size(260, 80)
        Me.txtFeedContent.TabIndex = 3
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Location = New System.Drawing.Point(108, 220)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton1.TabIndex = 4
        Me.SimpleButton1.Text = "SimpleButton1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 290)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.txtFeedContent)
        Me.Controls.Add(Me.btnSendFeed)
        Me.Controls.Add(Me.btnCheckStatus)
        Me.Controls.Add(Me.btnGetOrders)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGetOrders As System.Windows.Forms.Button
    Friend WithEvents btnCheckStatus As System.Windows.Forms.Button
    Friend WithEvents btnSendFeed As System.Windows.Forms.Button
    Friend WithEvents txtFeedContent As System.Windows.Forms.TextBox
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton

End Class
