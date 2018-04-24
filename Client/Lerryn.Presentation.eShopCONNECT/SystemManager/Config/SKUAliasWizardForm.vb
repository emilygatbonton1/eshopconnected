' eShopCONNECT for Connected Business
' Module: SKUAliasWizardForm.vb
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
' Updated 05 July 2013

Option Explicit On
Option Strict On

#Region " SKUAliasWizardForm "
<MenuActionAttribute.MenuAction("OpenSKUAliasWizardForm")> _
Public Class SKUAliasWizardForm

#Region " Variables "
    Private m_SourceCode As String ' TJS 05/07/13
#End Region

#Region " Properties "
#Region " SourceCode "
    Public Property SourceCode As String
        Get
            Return m_SourceCode ' TJS 05/07/13
        End Get
        Set(value As String)
            m_SourceCode = value ' TJS 05/07/13
        End Set
    End Property
#End Region
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
        ' 12/06/13 | TJS             | 2013.1.19 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = PRODUCT_NAME & " SKU Alias Import Wizard"
        Me.ToolBar.Visible = False
        Me.MainMenuBar.Visible = False

    End Sub
#End Region

#Region " InitializeControl "
    Public Overrides Sub InitializeControl()

        Try ' TJS 03/07/13
            MyBase.InitializeControl()
            DirectCast(Me.PanelBodyPluginInstance, SKUAliasWizardSection).InitialiseControls() ' TJS 03/07/13
            DirectCast(PanelBodyPluginInstance, SKUAliasWizardSection).SourceCode = m_SourceCode ' TJS 05/07/13

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex) ' TJS 03/07/13

        End Try

    End Sub
#End Region
#End Region
End Class
#End Region
