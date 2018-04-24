' eShopCONNECT for Connected Business
' Module: InventoryManufacturerIDsSection.vb
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
' Last Updated - 22 March 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Microsoft.VisualBasic ' TJS 02/12/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " InventoryManufacturerIDsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.InventoryManufacturerIDsSection")> _
Public Class InventoryManufacturerIDsSection

#Region " Variables "
    Private m_InventoryManufacturerIDsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_InventoryManufacturerIDsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_ManufacturerControl As Interprise.Extendable.Inventory.Presentation.SystemManager.Manufacturer.IManufacturerInterface
    Private m_ManufacturerDataset As Interprise.Framework.Inventory.DatasetGateway.SystemManager.ManufacturerDatasetGateway
    Private m_ManufacturerFacade As Interprise.Facade.Inventory.SystemManager.ManufacturerFacade
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.InventoryManufacturerIDsSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_InventoryManufacturerIDsSectionFacade

        End Get
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
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Me.m_InventoryManufacturerIDsDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Me.m_InventoryManufacturerIDsSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_InventoryManufacturerIDsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()


        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

    End Sub

    Public Sub New(ByVal InventoryManufacturerIDsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
       ByVal InventoryManufacturerIDsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)


        Me.m_InventoryManufacturerIDsDataset = InventoryManufacturerIDsDataset
        Me.m_InventoryManufacturerIDsSectionFacade = InventoryManufacturerIDsSectionFacade

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
        ' get reference to InventoryItemControl
        Me.m_ManufacturerControl = DirectCast(Me.FindForm.Controls("PanelBody").Controls("ManufacturerControl"), Interprise.Extendable.Inventory.Presentation.SystemManager.Manufacturer.IManufacturerInterface)
        ' from this, get reference to ItemDetailDataset
        Me.m_ManufacturerDataset = DirectCast(m_ManufacturerControl.CurrentDataset, Interprise.Framework.Inventory.DatasetGateway.SystemManager.ManufacturerDatasetGateway)
        ' and ItemDetailFacade
        Me.m_ManufacturerFacade = DirectCast(m_ManufacturerControl.CurrentFacade, Interprise.Facade.Inventory.SystemManager.ManufacturerFacade)

        If Me.m_ManufacturerDataset.SystemManufacturer.Count > 0 Then
            Me.LoadDataSet(New String()() {New String() {Me.m_InventoryManufacturerIDsDataset.SystemManufacturerSourceID_DEV000221.TableName, _
                "ReadSystemManufacturerSourceID_DEV000221", AT_MANUFACTURER_CODE, Me.m_ManufacturerDataset.SystemManufacturer(0).ManufacturerCode}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
        End If
    End Sub
#End Region

#Region " BeforeUpdatePluginDataSet "
    Public Overrides Function BeforeUpdatePluginDataSet(Optional ByVal confirm As Boolean = False, Optional ByVal clear As Boolean = False, Optional ByVal isUseCache As Boolean = False) As System.Windows.Forms.DialogResult
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/01/13 | TJS             | 2013.0.00 | Modified to prevent error if m_ManufacturerDataset not set
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.m_ManufacturerDataset IsNot Nothing AndAlso Me.m_ManufacturerDataset.SystemManufacturer.Count > 0 Then ' TJS 29/01/13
            Me.UpdateDataSet(New String()() {New String() {Me.m_InventoryManufacturerIDsDataset.SystemManufacturerSourceID_DEV000221.TableName, _
                "CreateSystemManufacturerSourceID_DEV000221", "UpdateSystemManufacturerSourceID_DEV000221", "DeleteSystemManufacturerSourceID_DEV000221"}}, _
                False, False, False)

        End If

        Return MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache)

    End Function
#End Region

#Region " FillAccountOrInstanceList "
    Private Sub FillAccountOrInstanceList()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLGroupNode As XElement
        Dim XMLGroupNodeList As System.Collections.Generic.IEnumerable(Of XNode)

        If Me.GridViewSourceManufacturerIDs.FocusedRowHandle >= 0 Then
            If Me.GridViewSourceManufacturerIDs.GetRowCellValue(Me.GridViewSourceManufacturerIDs.FocusedRowHandle, "SourceCode_DEV000221").ToString <> "" Then
                Me.LoadDataSet(New String()() {New String() {Me.m_InventoryManufacturerIDsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, Me.GridViewSourceManufacturerIDs.GetRowCellValue(Me.GridViewSourceManufacturerIDs.FocusedRowHandle, "SourceCode_DEV000221").ToString}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If Me.m_InventoryManufacturerIDsSectionFacade.IsActivated AndAlso Me.m_InventoryManufacturerIDsSectionFacade.IsConnectorActivated(Me.m_InventoryManufacturerIDsSectionFacade.ConnectorProductCode(Me.m_InventoryManufacturerIDsDataset.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221)) Then
                    XMLConfig = XDocument.Parse(Trim(Me.m_InventoryManufacturerIDsDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    XMLGroupNodeList = XMLConfig.XPathSelectElement("eShopCONNECTConfig").Nodes
                    Me.repAccountOrInstance.Items.Clear()
                    Me.repAccountOrInstance.Items.BeginUpdate()
                    For Each XMLGroupNode In XMLGroupNodeList
                        If XMLGroupNode.Name.ToString <> "General" Then
                            XMLTemp = XDocument.Parse(XMLGroupNode.ToString)
                            Select Case Me.GridViewSourceManufacturerIDs.GetRowCellValue(Me.GridViewSourceManufacturerIDs.FocusedRowHandle, "SourceCode_DEV000221").ToString
                                Case "AmazonOrder"
                                    Me.repAccountOrInstance.Items.Add(New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventoryManufacturerIDsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN))) ' TJS 22/03/13
                                Case "ASPStoreFrontOrder"
                                    Me.repAccountOrInstance.Items.Add(New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventoryManufacturerIDsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID)))
                                Case "ChanAdvOrder"
                                    Me.repAccountOrInstance.Items.Add(New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventoryManufacturerIDsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID)))
                                Case "MagentoOrder"
                                    Me.repAccountOrInstance.Items.Add(New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventoryManufacturerIDsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID)))
                                Case "ShopComOrder"
                                    Me.repAccountOrInstance.Items.Add(New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventoryManufacturerIDsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_ID)))
                                Case "VolusionOrder"
                                    Me.repAccountOrInstance.Items.Add(New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventoryManufacturerIDsSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_SITE_ID)))
                            End Select
                        End If
                    Next
                    Me.repAccountOrInstance.Items.EndUpdate()
                Else
                    Me.repAccountOrInstance.Items.Clear()
                End If
            Else
                Me.repAccountOrInstance.Items.Clear()
            End If
        End If

    End Sub
#End Region
#End Region

#Region " Events "
#Region " FocusedColumnChanged "
    Private Sub FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridViewSourceManufacturerIDs.FocusedColumnChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridViewSourceManufacturerIDs.FocusedColumn.FieldName = "AccountOrInstanceID_DEV000221" Then
            FillAccountOrInstanceList()
        End If

    End Sub
#End Region

#Region " FocusedRowChanged "
    Private Sub FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewSourceManufacturerIDs.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridViewSourceManufacturerIDs.FocusedRowHandle >= 0 Then
            FillAccountOrInstanceList()
        End If

    End Sub
#End Region
#End Region

End Class
#End Region
