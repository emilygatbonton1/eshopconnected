' eShopCONNECT for Connected Business
' Module: BulkPublishingWizardFilterForm.vb
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
' Updated 13 December 2013

Option Explicit On
Option Strict On

#Region " BulkPublishingWizardFilterForm "
<MenuActionAttribute.MenuAction("OpenBulkPublishingWizardFilterForm")> _
Public Class BulkPublishingWizardFilterForm

#Region " Variables "
    Private m_CategoryFilter As String = ""
    Private m_DepartmentFilter As String = ""
    Private m_ManufacturerFilter As String = ""
    Private m_StatusFilter As String = ""
    Private m_SupplierFilter As String = ""
#End Region

#Region " Properties "
#Region " CategoryFilter "
    Public ReadOnly Property ApplyCategoryFilter() As Boolean
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyCategoryFilter
            Else
                Return False
            End If
        End Get
    End Property

    Public Property CategoriesToFilter() As String
        Set(value As String)
            m_CategoryFilter = value
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).CategoriesToFilter = value
            End If
        End Set
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).CategoriesToFilter
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " DepartmentFilter "
    Public ReadOnly Property ApplyDepartmentFilter() As Boolean
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyDepartmentFilter
            Else
                Return False
            End If
        End Get
    End Property

    Public Property DepartmentsToFilter() As String
        Set(value As String)
            m_DepartmentFilter = value
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).DepartmentsToFilter = value
            End If
        End Set
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).DepartmentsToFilter
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " ManufacturerFilter "
    Public ReadOnly Property ApplyManufacturerFilter() As Boolean
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyManufacturerFilter
            Else
                Return False
            End If
        End Get
    End Property

    Public Property ManufacturerToFilter() As String
        Set(value As String)
            m_ManufacturerFilter = value
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ManufacturerToFilter = value
            End If
        End Set
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ManufacturerToFilter
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " StatusFilter "
    Public ReadOnly Property ApplyStatusFilter() As Boolean
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplyStatusFilter
            Else
                Return False
            End If
        End Get
    End Property

    Public Property StatusToFilter() As String
        Set(value As String)
            m_StatusFilter = value
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).StatusToFilter = value
            End If
        End Set
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).StatusToFilter
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " SupplierFilter "
    Public ReadOnly Property ApplySupplierFilter() As Boolean
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).ApplySupplierFilter
            Else
                Return False
            End If
        End Get
    End Property

    Public Property SupplierToFilter() As String
        Set(value As String)
            m_SupplierFilter = value
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).SupplierToFilter = value
            End If
        End Set
        Get
            If Me.PanelBodyPluginInstance IsNot Nothing Then
                Return DirectCast(Me.PanelBodyPluginInstance, Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection).SupplierToFilter
            Else
                Return ""
            End If
        End Get
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
        Me.Height = 360
        Me.btnOk.Enabled = True

    End Sub
#End Region
#End Region

End Class
#End Region

