' eShopCONNECT for Connected Business
' Module: SelectInstanceToCopyForm.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
'
'       © 2012 - 2013  Lerryn Business Solutions Ltd
'                      2 East View
'                      Bessie Lane
'                      Bradwell
'                      Hope Valley
'                      S33 9HZ
'
'  Tel +44 (0)1433 621584
'  Email Support@lerryn.com
'
' Lerryn is a Trademark of Lerryn Business Solutions Ltd
' eShopCONNECT is a Trademark of Lerryn Business Solutions Ltd
'-------------------------------------------------------------------
'
' Updated 29 May 2013

Option Explicit On
Option Strict On

#Region " SelectInstanceToCopyForm "
<MenuActionAttribute.MenuAction("OpenSelectInstanceToCopyForm")> _
Public Class SelectInstanceToCopyForm

#Region " Variables "
    Private m_SourceCode As String
    Private m_PublishedInstances As String()
#End Region

#Region " Properties "
#Region " InstanceToCopy "
    Public ReadOnly Property InstanceToCopy() As String
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing AndAlso DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopySection).rgSelectOption.EditValue.ToString = "Y" Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopySection).cbeInstanceToCopy.EditValue.ToString
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " SourceCode "
    Public WriteOnly Property SourceCode() As String
        Set(value As String)
            m_SourceCode = value
        End Set
    End Property
#End Region

#Region " PublishedInstances "
    Public WriteOnly Property PublishedInstances() As String()
        Set(value As String())
            m_PublishedInstances = value
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
        Me.Width = 400  'FA 29/05/13
        Me.Height = 300 'FA 29/05/13
        Me.btnOk.Enabled = False

    End Sub
#End Region

#Region " InitializeControl "
    Public Overrides Sub InitializeControl()

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim iActiveInstances As Integer

        MyBase.InitializeControl()

        If Me.PanelBodyPluginInstance IsNot Nothing Then

            DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopySection).SetSourceCode(m_SourceCode)

            Coll = DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopySection).cbeInstanceToCopy.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            iActiveInstances = 0
            For Each PublishedInstance As String In m_PublishedInstances
                If Not String.IsNullOrEmpty(PublishedInstance) Then
                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(PublishedInstance)
                Coll.Add(CollItem)
                    iActiveInstances = iActiveInstances + 1
                End If
            Next
            Coll.EndUpdate()
            DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopySection).PublishedInstanceCount = m_PublishedInstances.Length
        End If

    End Sub
#End Region
#End Region

End Class
#End Region

