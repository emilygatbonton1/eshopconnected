' eShopCONNECT for Connected Business
' Module: SelectDescriptionSourceForm.vb
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
' Updated 09 December 2013

Option Explicit On
Option Strict On

#Region " SelectDescriptionSourceForm "
<MenuActionAttribute.MenuAction("OpenSelectDescriptionSourceForm")> _
Public Class SelectDescriptionSourceForm

#Region " Variables "
    Private m_Source1Descriptions As String()
    Private m_Source2Descriptions As String()
#End Region

#Region " Properties "
#Region " Source1Option "
    Public ReadOnly Property Source1Option() As String
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceSection).rgSelectSource1.EditValue.ToString
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " Source2Option "
    Public ReadOnly Property Source2Option() As String
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceSection).rgSelectSource2.EditValue.ToString
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " WeightUnits "
    Public ReadOnly Property WeightUnits() As String
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceSection).rgSelectSource2.EditValue.ToString
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " Source1Descriptions "
    Public WriteOnly Property Source1Descriptions() As String()
        Set(value As String())
            m_Source1Descriptions = value
        End Set
    End Property
#End Region

#Region " Source2Descriptions "
    Public WriteOnly Property Source2Descriptions() As String()
        Set(value As String())
            m_Source2Descriptions = value
        End Set
    End Property
#End Region

#Region " OKButtonEnabled "
    Friend Property OKButtonEnabled() As Boolean
        Get
            Return Me.btnOk.Enabled
        End Get
        Set(value As Boolean)
            Me.btnOk.Enabled = value
        End Set
    End Property
#End Region

#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New()
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.ToolBar.Visible = False
        Me.MainMenuBar.Visible = False
        Me.Width = 400
        Me.Height = 300
        Me.btnOk.Enabled = False

    End Sub
#End Region

#Region " InitializeControl "
    Public Overrides Sub InitializeControl()

        MyBase.InitializeControl()

        If Me.PanelBodyPluginInstance IsNot Nothing Then
            DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceSection).SetSource1Options(m_Source1Descriptions)
            DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceSection).SetSource2Options(m_Source2Descriptions)
        End If

    End Sub
#End Region
#End Region

End Class
#End Region

