' eShopCONNECT for Connected Business
' Module: AmazonSettlementSectionContainer.vb
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

