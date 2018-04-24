' eShopCONNECT for Connected Business
' Module: AmazonSettlementSectionContainer.vb
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
' Updated 05 July 2012

Option Explicit On
Option Strict On

#Region " AmazonSettlementForm "
Namespace AmazonSettlement
<MenuActionAttribute.MenuAction("OpenAmazonSettlementForm")> _
Public Class AmazonSettlementForm

#Region " Variables "
    Private m_AmazonSettlementSectionContainer As Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface
    
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New()
        MyBase.New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.MenuItemNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Me.BarLinkExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Me.MenuItemFind.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Me.MenuItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Me.MenuItemEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
#End Region

#End Region

     End Class
End Namespace
#End Region

