' eShopCONNECT for Connected Business
' Module: AmazonSettlementSection.vb
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

#Region " AmazonSettlementSection "
Namespace AmazonSettlement
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.AmazonSettlement.AmazonSettlementSection")> _
Public Class AmazonSettlementSection
	
#Region " Variables "
        Private m_AmazonSettlementDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Private m_AmazonSettlementFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    
#End Region

#Region " Properties "
#Region " CurrentDataset "
	Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
		Get
			Return Me.AmazonSettlementSectionGateway
		End Get
	End Property
#End Region

#Region " AmazonSettlementDataset "
	Public ReadOnly Property AmazonSettlementDataset() As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
		Get
			Return Me.m_AmazonSettlementDataset
		End Get
	End Property
#End Region

#Region " CurrentFacade "
	Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
		Get
                Return Me.m_AmazonSettlementFacade
			
		End Get
	End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
	Public Sub New()
		MyBase.New()

		Me.m_AmazonSettlementDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

		'This call is required by the Windows Form Designer.
		Me.InitializeComponent()

		'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return
				
            Me.m_AmazonSettlementFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.m_AmazonSettlementDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)
		
	End Sub

	Public Sub New(ByVal AmazonSettlementDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
				   ByVal AmazonSettlementSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
		MyBase.New()		   
		
		Me.m_AmazonSettlementDataset = AmazonSettlementDataset
            Me.m_AmazonSettlementFacade = AmazonSettlementSectionFacade
		

		'This call is required by the Windows Form Designer.
		Me.InitializeComponent()

            'Add any initialization after the In itializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return
	End Sub
#End Region

#Region " InitializeControl "
	Protected Overrides Sub InitializeControl()
		'This call is required by the Presentation Layer.
		MyBase.InitializeControl()

		'Add any initialization after the InitializeControl() call

	End Sub
#End Region
#End Region

#Region " Events "
#Region " GridViewSettlementDrawGroupRow "
        Private Sub GridViewSettlementDrawGroupRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs) Handles GridViewSettlementDetail.CustomDrawGroupRow

            Dim view As DevExpress.XtraGrid.Views.Grid.GridView
            Dim info As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo

            view = DirectCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
            info = DirectCast(e.Info, DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo)
            If info.Column.FieldName = "TransactionGroup_DEV000221" Then
                info.GroupText = view.GetGroupRowValue(e.RowHandle, colTransactionType_DEV000221).ToString
            ElseIf info.Column.FieldName = "AmazonOrderID_DEV000221" Then
                If CInt(view.GetGroupRowValue(e.RowHandle, colTransactionGroup_DEV000221)) = 3 Then
                    info.GroupText = view.GetGroupRowValue(e.RowHandle, colAmazonOrderID_DEV000221).ToString
                End If
            End If

        End Sub
#End Region
#End Region
    End Class
End Namespace
#End Region

