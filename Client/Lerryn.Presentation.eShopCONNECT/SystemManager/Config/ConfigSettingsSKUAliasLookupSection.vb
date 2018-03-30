' eShopCONNECT for Connected Business
' Module: ConfigSettingsSKUAliasLookupSection.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
'
'       © 2012 - 2014  Lerryn Business Solutions Ltd
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
' Updated 22 May 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 22/05/14
Imports System.Xml.XPath ' TJS 22/05/14

#Region " ConfigSettingsSKUAliasLookupSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, _
    "Lerryn.Presentation.ImportExport.ConfigSettingsSKUAliasLookupSection")> _
Public Class ConfigSettingsSKUAliasLookupSection

#Region " Variables "
    Private m_ConfigSettingsSKUAliasLookupDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_ConfigSettingsSKUAliasLookupSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_SourceCode As String
    Private m_SourceConfig As String
    Private strOrigShipMthGrpCode As String = ""
    Private bNewRowStarted As Boolean = False
    Private strOrigItemName As String = "" ' TJS 05/07/13
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.ConfigSettingsSKUAliasLookupSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_ConfigSettingsSKUAliasLookupSectionFacade
        End Get
    End Property
#End Region

    Public WriteOnly Property SourceCode() As String
        Set(ByVal value As String)
            m_SourceCode = value
        End Set
    End Property

    Public WriteOnly Property SourceConfig() As String
        Set(ByVal value As String)
            m_SourceConfig = value
        End Set
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
        ' 19/08/10 | TJS             | 2010.1.00 | Original
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '------------------------------------------------------------------------------------------

        MyBase.New()

        Me.m_ConfigSettingsSKUAliasLookupDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        Me.m_ConfigSettingsSKUAliasLookupSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_ConfigSettingsSKUAliasLookupDataset, _
            New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
    End Sub

    Public Sub New(ByVal ConfigSettingsSKUAliasLookupDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
          ByVal ConfigSettingsSKUAliasLookupSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)

        MyBase.New()

        Me.m_ConfigSettingsSKUAliasLookupDataset = ConfigSettingsSKUAliasLookupDataset
        Me.m_ConfigSettingsSKUAliasLookupSectionFacade = ConfigSettingsSKUAliasLookupSectionFacade

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
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

#Region " ResetNewRow "
    Public Sub ResetNewRow() ' TJS 17/03/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Original
        '------------------------------------------------------------------------------------------

        bNewRowStarted = False
        Me.GridViewSKUAliasLookup.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom

    End Sub
#End Region
#End Region

#Region " Events "
    Private Sub repItemCode_BeforePopup(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles repItemCode.BeforePopup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Original
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to select default language 
        '------------------------------------------------------------------------------------------

        Dim XMLConfig As XDocument, XMLGroupNode As XElement, XMLConfigNode As XElement ' TJS 22/05/14
        Dim XMLGroupNodeList As System.Collections.Generic.IEnumerable(Of XNode) ' TJS 22/05/14
        Dim XMLConfigNodeList As System.Collections.Generic.IEnumerable(Of XNode) ' TJS 22/05/14
        Dim strDefaultWarehouse As String, bXMLError As Boolean ' TJS 22/05/14

        ' start of code added TJS 22/05/14
        bXMLError = False
        strDefaultWarehouse = ""
        Try
            XMLConfig = XDocument.Parse(Trim(Me.m_ConfigSettingsSKUAliasLookupDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show("XML Config error - " & ex.Message)
            bXMLError = True
        End Try

        If Not bXMLError Then
            XMLGroupNodeList = XMLConfig.XPathSelectElement("eShopCONNECTConfig").Nodes
            For Each XMLGroupNode In XMLGroupNodeList
                XMLConfigNodeList = XMLGroupNode.Nodes
                For Each XMLConfigNode In XMLConfigNodeList
                    If XMLConfigNode.Name = "DefaultWarehouse" Then
                        strDefaultWarehouse = XMLConfigNode.Value
                    End If
                Next
            Next

        End If
        ' end of code added TJS 22/05/14

        repItemCode.AdditionalFilter = "and LanguageCode = '" & Me.GetField("CompanyLanguage", "SystemCompanyInformation") & "' and WarehouseCode = '" & strDefaultWarehouse.Replace("'", "''") & "'" ' TJS 22/05/14

    End Sub

    Private Sub repItemCode_PopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles repItemCode.PopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/07/11 | TJS             | 2011.1.00 | Function added
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to set ItemName for dispay purposes
        '------------------------------------------------------------------------------------------

        If GridViewSKUAliasLookup.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle And Not bNewRowStarted Then
            GridViewSKUAliasLookup.AddNewRow()
        End If
        Me.GridViewSKUAliasLookup.SetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemCode_DEV000221", eRow.DataRowSelected("ItemCode"))
        Me.GridViewSKUAliasLookup.SetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemName", eRow.DataRowSelected("ItemName")) ' TJS 03/07/13

    End Sub

    Private Sub repUnitMeasureCode_BeforePopup(sender As Object, e As CancelEventArgs) Handles repUnitMeasureCode.BeforePopup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 22/05/14 | TJS             | 2014.0.01 | Function added 
        '------------------------------------------------------------------------------------------

        If Me.GridViewSKUAliasLookup.GetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemCode_DEV000221") IsNot Nothing Then
            repUnitMeasureCode.AdditionalFilter = "and ItemCode = '" & Me.GridViewSKUAliasLookup.GetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemCode_DEV000221").ToString & "'"
        Else
            repUnitMeasureCode.AdditionalFilter = "and ItemCode = ''"
        End If

    End Sub

    Private Sub repUnitMeasureCode_PopupClose(sender As Object, eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles repUnitMeasureCode.PopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 22/05/14 | TJS             | 2014.0.01 | Function added 
        '------------------------------------------------------------------------------------------

        If GridViewSKUAliasLookup.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle And Not bNewRowStarted Then
            GridViewSKUAliasLookup.AddNewRow()
        End If
        Me.GridViewSKUAliasLookup.SetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "UnitMeasureCode_DEV000221", eRow.DataRowSelected("UnitMeasureCode"))

    End Sub

    Private Sub GridViewSKUAliasLookup_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridViewSKUAliasLookup.FocusedColumnChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/13 | TJS             | 2013.1.26 | Function added
        '------------------------------------------------------------------------------------------

        Dim strItemDetails() As String

        Try
            If e.PrevFocusedColumn IsNot Nothing AndAlso e.PrevFocusedColumn.FieldName = "ItemName" AndAlso _
                Me.GridViewSKUAliasLookup.GetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemName") IsNot Nothing Then
                If Me.GridViewSKUAliasLookup.GetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemName").ToString <> strOrigItemName Then
                    strItemDetails = Me.m_ConfigSettingsSKUAliasLookupSectionFacade.GetRow(New String() {"ItemCode", "ItemName"}, "InventoryItem", "ItemName = '" & Me.GridViewSKUAliasLookup.GetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemName").ToString.Replace("'", "''") & "'")
                    If strItemDetails IsNot Nothing AndAlso Not String.IsNullOrEmpty(strItemDetails(0)) Then
                        Me.GridViewSKUAliasLookup.SetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemCode_DEV000221", strItemDetails(0))
                        Me.GridViewSKUAliasLookup.SetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemName", strItemDetails(1))
                    Else
                        Me.GridViewSKUAliasLookup.SetColumnError(ColItemName, "Not a valid Inventory Item Name")
                    End If
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try

    End Sub

    Private Sub GridViewSKUAliasLookup_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewSKUAliasLookup.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/13 | TJS             | 2013.1.26 | Function added
        '------------------------------------------------------------------------------------------

        Dim strItemCode As String

        Try
            If e.PrevFocusedRowHandle >= 0 AndAlso Me.GridViewSKUAliasLookup.GetRowCellValue(e.PrevFocusedRowHandle, "ItemName") IsNot Nothing Then
                If Me.GridViewSKUAliasLookup.GetRowCellValue(e.PrevFocusedRowHandle, "ItemName").ToString <> strOrigItemName Then
                    strItemCode = Me.m_ConfigSettingsSKUAliasLookupSectionFacade.GetField("ItemCode", "InventoryItem", "ItemName = '" & Me.GridViewSKUAliasLookup.GetRowCellValue(e.PrevFocusedRowHandle, "ItemName").ToString.Replace("'", "''") & "'")
                    If Not String.IsNullOrEmpty(strItemCode) Then
                        Me.GridViewSKUAliasLookup.SetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, "ItemCode_DEV000221", strItemCode)
                    Else
                        Me.GridViewSKUAliasLookup.FocusedRowHandle = e.PrevFocusedRowHandle
                        Me.GridViewSKUAliasLookup.SetColumnError(ColItemName, "Not a valid Inventory Item Name")
                    End If
                End If
            End If
            If e.FocusedRowHandle >= 0 Then
                strOrigItemName = Me.GridViewSKUAliasLookup.GetRowCellValue(e.FocusedRowHandle, "ItemName").ToString
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Sub

    Private Sub GridViewSKUAliasLookup_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridViewSKUAliasLookup.InitNewRow
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Original
        '------------------------------------------------------------------------------------------

        Me.GridViewSKUAliasLookup.SetRowCellValue(Me.GridViewSKUAliasLookup.FocusedRowHandle, Me.ConfigSettingsSKUAliasLookupSectionGateway.LerrynImportExportDeliveryMethods_DEV000221.SourceCode_DEV000221Column.ColumnName, m_SourceCode)
        Me.GridViewSKUAliasLookup.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
        bNewRowStarted = True

    End Sub

    Private Sub btnImportExport_Click(sender As System.Object, e As System.EventArgs) Handles btnImportExport.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Function added
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to set source code in wizard
        '------------------------------------------------------------------------------------------

        Dim frmSKUImport As SKUAliasWizardForm

        frmSKUImport = New SKUAliasWizardForm
        frmSKUImport.SourceCode = Me.m_SourceCode
        frmSKUImport.Show()

    End Sub
#End Region
End Class
#End Region
