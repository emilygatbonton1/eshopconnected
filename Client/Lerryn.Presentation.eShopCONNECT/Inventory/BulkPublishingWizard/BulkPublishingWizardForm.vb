' eShopCONNECT for Connected Business
' Module: BulkPublishingWizardForm.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Connected Business SDK and may incorporate certain intellectual 
' property of Interprise Solutions Inc. who's
' rights are hereby recognised.
'

'-------------------------------------------------------------------
'
' Updated 13 November 2013

Option Explicit On
Option Strict On

#Region " BulkPublishingWizardForm "
<MenuActionAttribute.MenuAction("OpenBulkPublishingForm")> _
Public Class BulkPublishingWizardForm

#Region " Properties "
    Public ReadOnly Property IsActivated() As Boolean
        Get
            If PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(PanelBodyPluginInstance, BulkPublishingWizardSectionContainer).IsActivated
            Else
                Return False
            End If
        End Get
    End Property
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to ensure form opens at correct size
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = PRODUCT_NAME & " Bulk Inventory Publishing Wizard"
        Me.ToolBar.Visible = False
        Me.MainMenuBar.Visible = False

        Me.Height = 670 ' TJS 13/11/13
        Me.Width = 990 ' TJS 13/11/13
    End Sub
#End Region

#End Region

End Class
#End Region

