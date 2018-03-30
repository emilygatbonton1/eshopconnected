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

Imports Lerryn.Framework.ImportExport.Shared.Const

#Region " AmazonSettlementSectionContainer "
Namespace AmazonSettlement
Public Class AmazonSettlementSectionContainer
	
#Region " Variables "
    Private m_ImportExportConfigFacadesectionContainerFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade    
#End Region

#Region " Properties "
#Region " CurrentDataset "
	Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
		Get
			Return Me.AmazonSettlementSectionContainerGateway
		End Get
	End Property
#End Region

#Region " CurrentFacade "
	Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
		Get
		 Return Me.m_ImportExportConfigFacadesectionContainerFacade 		 
		 
		End Get
	End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    Me.InitializeComponent()

    'Add any initialization after the InitializeComponent() call
            Me.m_ImportExportConfigFacadesectionContainerFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.AmazonSettlementSectionContainerGateway, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

End Sub
#End Region

#Region " InitializeControl "
Protected Overrides Sub InitializeControl()
    'This call is required by the Presentation Layer.
    MyBase.InitializeControl()

    'Add any initialization after the InitializeControl() call
End Sub
#End Region

#Region " LoadDataSet "
        Public Overrides Function LoadDataSet(ByVal documentCode As String, ByVal row As System.Data.DataRow, _
            Optional ByVal clearTableType As Interprise.Framework.Base.Shared.Enum.ClearType = Interprise.Framework.Base.Shared.Enum.ClearType.None) As Boolean
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -    
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 05/07/12 | TJS             | 2011.2.00 | Original
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim commandSetRead As String()()

            commandSetRead = New String()() {New String() {AmazonSettlementSectionContainerGateway.LerrynImportExportAmazonSettlement_DEV000221.TableName, _
                "ReadLerrynImportExportAmazonSettlement_DEV000221", AT_SETTLEMENT_CODE, documentCode}, _
                New String() {AmazonSettlementSectionContainerGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.TableName, _
                "ReadLerrynImportExportAmazonSettlementDetail_DEV000221", AT_SETTLEMENT_CODE, documentCode}}

            LoadDataSet = MyBase.LoadDataSet(commandSetRead, Interprise.Framework.Base.Shared.ClearType.Specific)

            Me.IsReadOnly = (Me.AmazonSettlementSectionContainerGateway.LerrynImportExportAmazonSettlement_DEV000221.Count = 0)
            Return LoadDataSet

        End Function
#End Region
#End Region
    End Class
End Namespace
#End Region

