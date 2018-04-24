' eShopCONNECT for Connected Business
' Module: InventoryASPStorefrontSettingsSection.vb
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
' Last Updated - 13 November 2013

Option Explicit On
Option Strict On

Imports Interprise.Framework.Base.Shared.Const ' 24/02/12
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " InventoryASPStorefrontSettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.InventoryASPStorefrontSettingsSection")> _
Public Class InventoryASPStorefrontSettingsSection

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private WithEvents m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
    Private m_ASPStorefrontImportFacade As Lerryn.Facade.ImportExport.ASPStorefrontImportFacade ' TJS 09/04/11
    Private m_CategoryTable As System.Data.DataTable = Nothing
    Private m_ASPStorefrontCategoryCount As Integer = 0

    Private WithEvents m_BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.InventoryASPStorefrontSettingsSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_InventorySettingsFacade
        End Get
    End Property
#End Region

#Region " InventoryItemDataset "
    Public Property InventoryItemDataset() As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
        Get
            Return Me.m_InventoryItemDataset
        End Get
        Set(ByVal value As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway)
            Me.m_InventoryItemDataset = value
        End Set
    End Property
#End Region

#Region " InventoryItemFacade "
    Public Property InventoryItemFacade() As Interprise.Facade.Inventory.ItemDetailFacade
        Get
            Return Me.m_InventoryItemFacade
        End Get
        Set(ByVal value As Interprise.Facade.Inventory.ItemDetailFacade)
            Me.m_InventoryItemFacade = value
        End Set
    End Property
#End Region

#Region " ShowActivateMessage "
    ' start of code added TJS 02/12/11
    Public Property ShowActivateMessage() As Boolean
        Get
            Return Me.lblActivate.Visible And Me.PanelControlDummy.Visible
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.PanelControlDummy.BringToFront()
                Me.PanelControlDummy.Visible = True
                Me.PanelPublishOnASPStorefront.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnASPStorefront.BringToFront()
            End If
            Me.lblActivate.Visible = value
            If value Then
                Me.lblDevelopment.Visible = False
            End If
        End Set
    End Property
    ' end of code added TJS 02/12/11
#End Region

#Region " ShowDevelopmentMessage "
    ' start of code added TJS 02/12/11
    Public Property ShowDevelopmentMessage() As Boolean
        Get
            Return Me.lblDevelopment.Visible And Me.PanelControlDummy.Visible
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.PanelControlDummy.BringToFront()
                Me.PanelControlDummy.Visible = True
                Me.PanelPublishOnASPStorefront.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnASPStorefront.BringToFront()
            End If
            Me.lblDevelopment.Visible = value
            If value Then
                Me.lblActivate.Visible = False
            End If
        End Set
    End Property
    ' end of code added TJS 02/12/11
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

        Me.m_InventorySettingsDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        Me.m_InventorySettingsFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.m_InventorySettingsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
    End Sub

    Public Sub New(ByVal p_InventoryAmazonSettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal p_InventoryAmazonSettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)

        MyBase.New()

        Me.m_InventorySettingsDataset = p_InventoryAmazonSettingsDataset
        Me.m_InventorySettingsFacade = p_InventoryAmazonSettingsFacade

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
        InitialiseControls() ' TJS 02/12/11

    End Sub

    Public Sub InitialiseControls()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Code added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to load all source config records to remove need to 
        '                                        | reload when switching between source tabs on Inventory form
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem

        ' is eShopCONNECT activated ?
        If Me.m_InventorySettingsFacade.IsActivated Then
            ' yes, is ASPStorefront connector activated ?
            If Me.m_InventorySettingsFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                ' yes, need to check if there is an Item record in case we are being configured in the User Role !
                If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Rows.Count > 0 Then ' TJS 02/12/11
                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                        "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 02/12/11
                    XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(ASP_STORE_FRONT_SOURCE_CODE).ConfigSettings_DEV000221)) ' TJS 02/12/11
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
                    Coll = Me.cbeSelectASPSite.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_ACCOUNT_DISABLED).ToUpper <> "YES" Then
                            CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID))
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    If Coll.Count > 0 Then
                        ' has Item been saved yet (i.e. ItemCode is not [To be generated])
                        If Me.m_InventoryItemDataset.InventoryItem(0).ItemCode <> Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
                            ' yes, enable ASPStorefront details tab
                            Me.PanelPublishOnASPStorefront.Enabled = True
                            If Coll.Count = 1 Then
                                Me.cbeSelectASPSite.SelectedIndex = 0
                                ASPStorefrontSiteSelected(Nothing, Nothing)
                            End If

                        Else
                            Me.PanelPublishOnASPStorefront.Enabled = False
                        End If
                        AddHandler Me.cbeSelectASPSite.EditValueChanged, AddressOf ASPStorefrontSiteSelected
                        AddHandler Me.CheckEditPublishOnASPStorefront.EditValueChanged, AddressOf PublishOnASPStorefrontChange
                    Else
                        ShowActivateMessage = True ' TJS 02/12/11
                    End If
                Else
                    ShowActivateMessage = True ' TJS 02/12/11
                End If
            Else
                ShowActivateMessage = True ' TJS 02/12/11
            End If
        Else
            ShowActivateMessage = True ' TJS 02/12/11
        End If

    End Sub
#End Region

#Region " EnableDisableASPStorefrontControls "
    Private Sub EnableDisableASPStorefrontControls(ByVal Enable As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for Inventory Qty publishing
        ' 24/02/12 | TJS             | 2011.2.08 | Added missing chkPriceAsIS control and modified to 
        '                                        | lock controls which are set from MAtrix Group
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bMatrixGroupItem As Boolean, bMatrixItem As Boolean
        Dim strTemp As String, strProperties() As String

        ' is item a Matrix Group Item or a Matrix Item ?
        If Me.m_InventoryItemDataset.InventoryItem.Count > 0 Then
            If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                bMatrixGroupItem = True
                bMatrixItem = False
            ElseIf Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Then
                bMatrixGroupItem = False
                bMatrixItem = True
            Else
                bMatrixGroupItem = False
                bMatrixItem = False
            End If

        Else
            bMatrixGroupItem = False
            bMatrixItem = False
        End If

        ' are we displaying a Matrix Item ?
        If bMatrixItem Then
            ' yes, get ItemCode for Matrix Group Item
            strTemp = Me.m_InventorySettingsFacade.GetField("SELECT ItemCode FROM dbo.InventoryMatrixItem WHERE MatrixItemCode = '" & _
                Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'", CommandType.Text, Nothing)
            ' now get Matrix Group Item properties
            strProperties = Me.m_InventorySettingsFacade.GetRow(New String() {"ProductName_DEV000221", "ProductDescription_DEV000221"}, "InventoryASPStorefrontDetails_DEV000221", _
                "ItemCode_DEV000221 = '" & strTemp & "' AND SiteID_DEV000221 = '" & Me.cbeSelectASPSite.EditValue.ToString & "'", False)
            If strProperties IsNot Nothing Then
                ' remove data bindings for relevant fields
                Me.TextEditProductName.DataBindings.Clear()
                Me.MemoEditDescription.DataBindings.Clear()
                ' now display Matrix Group Items properties
                Me.TextEditProductName.EditValue = strProperties(0)
                Me.MemoEditDescription.EditValue = strProperties(1)
            End If
            Me.TreeListCategories.OptionsBehavior.Editable = False ' TJS 24/02/12
            Me.DateEditAvailableFrom.Properties.ReadOnly = True ' TJS 24/02/12
            Me.DateEditAvailableTo.Properties.ReadOnly = True ' TJS 24/02/12
            Me.GridViewProperties.OptionsBehavior.Editable = False ' TJS 24/02/12
            Me.MemoEditDescription.Properties.ReadOnly = True ' TJS 24/02/12
            Me.MemoEditSummary.Properties.ReadOnly = True ' TJS 24/02/12
            Me.TextEditProductName.Properties.ReadOnly = True ' TJS 24/02/12
        End If

        ' these controls are enabled if Instance ID is set
        If Me.cbeSelectASPSite.SelectedIndex >= 0 Then
            Me.CheckEditPublishOnASPStorefront.Enabled = True
        Else
            Me.CheckEditPublishOnASPStorefront.Enabled = False
        End If

        Me.DateEditAvailableFrom.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.DateEditAvailableTo.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        If m_CategoryTable IsNot Nothing Then
            If m_CategoryTable.Rows.Count > 0 Then
                Me.TreeListCategories.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
            Else
                Me.TreeListCategories.Enabled = False ' TJS 24/02/12
            End If
        Else
            Me.TreeListCategories.Enabled = False ' TJS 24/02/12
        End If
        Me.chkPriceAsIS.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked ' TJS 24/02/12
        Me.GridControlProperties.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.lblDescription.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.lblASPSellingPrice.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.lblProductName.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.lblSummary.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.lblAvailableFrom.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.lblAvailableTo.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.lblASPStorefrontAttributes.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked ' TJS 24/02/12
        Me.MemoEditDescription.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.MemoEditSummary.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.TextEditASPSellingPrice.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.TextEditProductName.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked
        Me.colPropertiesDisplayValue.AppearanceHeader.ForeColor = Color.Gray
        ' display labels for Matrix Items and Matrix Group Items
        Me.LabelMatrixGroupItem.Visible = bMatrixGroupItem And Enable
        Me.LabelMatrixItem.Visible = bMatrixItem And Enable

        ' now set Product name and description text colour
        If Me.TextEditProductName.Enabled Then
            If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 30/10/1
                TextEditProductName.ForeColor = Color.Gray
            Else
                TextEditProductName.ForeColor = Color.Black
            End If
        End If
        If Me.MemoEditDescription.Enabled Then
            If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then ' TJS 13/11/13
                MemoEditDescription.ForeColor = Color.Gray
            Else
                MemoEditDescription.ForeColor = Color.Black
            End If
        End If

        Me.lblQtyPublishing.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked ' TJS 02/12/11
        Me.RadioGroupQtyPublishing.Enabled = Enable And Me.CheckEditPublishOnASPStorefront.Checked ' TJS 02/12/11
        If Me.RadioGroupQtyPublishing.SelectedIndex >= 0 And Enable And Me.CheckEditPublishOnASPStorefront.Checked Then ' TJS 02/12/11
            QtyPublishingTypeChanged(Me, Nothing) ' TJS 02/12/11
        Else
            Me.TextEditQtyPublishingValue.Enabled = False ' TJS 02/12/11
            Me.lblQtyPublishingValue.Enabled = False ' TJS 02/12/11
            Me.lblQtyPublishingValue.Text = "" ' TJS 02/12/11
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
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to caater for Inventory Qty publishing
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ReturnValue As DialogResult ' TJS 02/12/11

        Try
            ' save any changes to ASP Storefront settings
            If Me.m_InventorySettingsFacade.UpdateDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.TableName, _
                    "CreateInventoryASPStorefrontDetails_DEV000221", "UpdateInventoryASPStorefrontDetails_DEV000221", "DeleteInventoryASPStorefrontDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.TableName, _
                    "CreateInventoryASPStorefrontTagDetails_DEV000221", "UpdateInventoryASPStorefrontTagDetails_DEV000221", "DeleteInventoryASPStorefrontTagDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontCategories_DEV000221.TableName, _
                    "CreateInventoryASPStorefrontCategories_DEV000221", "UpdateInventoryASPStorefrontCategories_DEV000221", "DeleteInventoryASPStorefrontCategories_DEV000221"}}, _
                    Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Inventory ASP Storefront Settings", False) Then
                ReturnValue = MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache)

            Else
                ReturnValue = DialogResult.Cancel
            End If
            ' setting error text on dataset doesn't cause control to display it so copy it
            If Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.Count > 0 Then ' TJS 02/12/11
                Me.RadioGroupQtyPublishing.ErrorText = Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221(0).GetColumnError("QtyPublishingType_DEV000221") ' TJS 02/12/11
            End If
            Return ReturnValue ' TJS 02/12/11

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Function
#End Region

#Region " SetProposedASPStorefrontPrice "
    Private Sub SetProposedASPStorefrontPrice(ByVal ItemCode As String, ByVal CurrencyCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to cater for all source config records being loaded
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLASPStorefrontNode As XElement
        Dim XMLASPStorefrontNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strSQL As String, strTemp As String, bUpdatePrice As Boolean

        ' get config settings
        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(ASP_STORE_FRONT_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
        XMLASPStorefrontNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
        ' check connector count is valid i.e. number of Amazon settings is not more then the licence limit
        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLASPStorefrontNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE) Then
            ' check each ASPStorefront record for current Instance ID
            For Each XMLASPStorefrontNode In XMLASPStorefrontNodeList
                XMLTemp = XDocument.Parse(XMLASPStorefrontNode.ToString)
                ' have we found current Instance ID ?
                If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID) = Me.cbeSelectASPSite.Text Then
                    ' yes, has ASPStorefront price been set ?
                    bUpdatePrice = False
                    If Me.TextEditASPSellingPrice.Text <> "" Then
                        If CDec(Me.TextEditASPSellingPrice.EditValue) = 0 Then
                            bUpdatePrice = True
                        End If
                    Else
                        bUpdatePrice = True
                    End If
                    If bUpdatePrice Then
                        ' get retail selling price
                        strSQL = "SELECT RetailPrice FROM dbo.InventoryItemPricingDetailView WHERE CurrencyCode = '" & CurrencyCode & "' AND ItemCode = '" & ItemCode & "'"
                        strTemp = Me.m_InventorySettingsFacade.GetField(strSQL, CommandType.Text, Nothing)
                        ' did we find a price ?
                        If strTemp <> "" Then
                            ' yes, use it
                            Me.TextEditASPSellingPrice.EditValue = CDec(strTemp)
                        End If
                    End If
                End If
                Exit For
            Next
        End If

    End Sub
#End Region

#Region " CreateASPStorefrontTags "
    Private Sub CreateASPStorefrontTags()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to initialise all boolean tag items and set Matrix Group attribute prompts
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowASPStorefrontTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontTagDetails_DEV000221Row
        Dim strSEDetails As String(), strOptions As String(), strAttributeCodes As String()() ' TJS 24/02/12
        Dim strTemp As String, strSQLCondition As String, iLoop As Integer, bPromptTextSet As Boolean ' TJS 24/02/12
        Dim bOnlySizeAndOrColor As Boolean ' TJS 24/02/12

        strTemp = Me.m_ASPStorefrontImportFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & ASP_STORE_FRONT_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & Me.cbeSelectASPSite.EditValue.ToString.Replace("'", "''") & "'")
        If "" & strTemp = "" Then
            strTemp = Me.m_ASPStorefrontImportFacade.GetField("WebSiteCode", "EcommerceSite", "ISDefault = 1")
        End If
        strSQLCondition = "ItemCode = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND WebSiteCode = '" & strTemp & "'"
        strOptions = Me.m_ASPStorefrontImportFacade.GetRow(New String() {"IsFeatured", "RequiresRegistration", "HidePriceUntilCart", "IsCallToOrder", "ShowBuyButton"}, "InventoryItemWebOption", strSQLCondition)
        strSQLCondition = "ItemCode = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND LanguageCode = '"
        strSQLCondition = strSQLCondition & Me.m_ASPStorefrontImportFacade.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
        strSQLCondition = strSQLCondition & "' AND WebSiteCode = '" & strTemp & "'"
        strSEDetails = Me.m_ASPStorefrontImportFacade.GetRow(New String() {"SEName", "SETitle", "SEKeywords", "SEDescription", "SENoScript", "SEAltText"}, "InventoryItemWebOptionDescription", strSQLCondition)

        ' text tag items
        For iLoop = 0 To 3
            rowASPStorefrontTagDetails = m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = Me.cbeSelectASPSite.EditValue.ToString
            Select Case iLoop
                Case 0
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "ManufacturerPartNumber"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Manufacturer Part Number"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 50
                Case 1
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SEName"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "SE Name"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "SE"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 150
                    If "" & strSEDetails(0) <> "" Then
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = strSEDetails(0)
                    End If
                Case 2
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "XmlPackage"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Xml Package"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Display"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 100
                Case 3
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "TemplateName"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Template Name"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Display"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 50
            End Select
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Text"
            m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

        ' memo tag items
        For iLoop = 0 To 15
            rowASPStorefrontTagDetails = m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = Me.cbeSelectASPSite.EditValue.ToString
            Select Case iLoop
                Case 0
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SpecTitle"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Spec Title"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 1
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "MiscText"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Misc Text"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 2
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "Notes"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = rowASPStorefrontTagDetails.TagName_DEV000221
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 3
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "IsFeaturedTeaser"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Is Featured Teaser"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 4
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "FroogleDescription"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Froogle Description"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 5
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SwatchImageMap"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Swatch Image Map"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 6
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SETitle"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "SE Title"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "SE"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strSEDetails(1) <> "" Then
                        rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = strSEDetails(1)
                    End If
                Case 7
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SEKeywords"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "SE Keywords"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "SE"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strSEDetails(2) <> "" Then
                        rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = strSEDetails(2)
                    End If
                Case 8
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SEDescription"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "SE Description"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "SE"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strSEDetails(3) <> "" Then
                        rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = strSEDetails(3)
                    End If
                Case 9
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SENoScript"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "SE No Script"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "SE"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strSEDetails(4) <> "" Then
                        rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = strSEDetails(4)
                    End If
                Case 10
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SEAltText"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "SE Alt Text"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "SE"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strSEDetails(5) <> "" Then
                        rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = strSEDetails(5)
                    End If
                Case 11
                    If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                        strAttributeCodes = m_InventorySettingsFacade.GetRows(New String() {"AttributeCode", "PositionID"}, "InventoryAttribute", "ItemCode = '" & _
                            Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' ORDER BY PositionID")
                        bPromptTextSet = False
                        bOnlySizeAndOrColor = True
                        For Each AttributeCode As String() In strAttributeCodes
                            If AttributeCode(0) = "Size" Then
                                rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = "Select Size"
                                bPromptTextSet = True

                            ElseIf AttributeCode(0) <> "Color" Then
                                bOnlySizeAndOrColor = False
                            End If
                        Next
                        If Not bPromptTextSet And strAttributeCodes.Length = 2 And Not bOnlySizeAndOrColor Then
                            rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = "Select " & strAttributeCodes(1)(0)
                        End If
                    End If
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SizeOptionPrompt"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Size Option Prompt"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 12
                    If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                        strAttributeCodes = m_InventorySettingsFacade.GetRows(New String() {"AttributeCode", "PositionID"}, "InventoryAttribute", "ItemCode = '" & _
                            Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' ORDER BY PositionID")
                        bPromptTextSet = False
                        bOnlySizeAndOrColor = True
                        For Each AttributeCode As String() In strAttributeCodes
                            If AttributeCode(0) = "Color" Then
                                rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = "Select Color"
                                bPromptTextSet = True

                            ElseIf AttributeCode(0) <> "Size" Then
                                bOnlySizeAndOrColor = False
                            End If
                        Next
                        If Not bPromptTextSet And strAttributeCodes.Length <= 2 And Not bOnlySizeAndOrColor Then
                            rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = "Select " & strAttributeCodes(0)(0)
                        End If
                    End If
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "ColorOptionPrompt"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Color Option Prompt"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 13
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SpecCall"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Spec Call"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 14
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "ImageFilenameOverride"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Image Filename Override"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Images"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 15
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "TextOptionPrompt"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Text Option Prompt"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
            End Select
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Memo"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
            m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

        ' Integer tag items
        For iLoop = 0 To 4
            rowASPStorefrontTagDetails = m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = Me.cbeSelectASPSite.EditValue.ToString
            Select Case iLoop
                Case 0
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "ColWidth"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Col Width"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Display"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 1
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "PageSize"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Page Size"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Display"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 2
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SkinID"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Skin ID"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Display"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 3
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "PackSize"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Pack Size"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                Case 4
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "TextOptionMaxLength"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Text Option Max Length"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
            End Select
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Integer"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
            m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

        ' Boolean tag items
        For iLoop = 0 To 14
            rowASPStorefrontTagDetails = m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = Me.cbeSelectASPSite.EditValue.ToString
            Select Case iLoop
                Case 0
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "SpecsInline"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Specs In line"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 1
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "IsFeatured"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Is Featured"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strOptions(0) = "1" Or "" & strOptions(0).ToLower = "true" Then ' TJS 24/02/12
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "True"
                    Else
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False"
                    End If
                Case 2
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "IsAKit"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Is a Kit"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 3
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "IsSystem"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Is System"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 4
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "IsAPack"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Is a Pack"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 5
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "ShowInProductBrowser"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Show In Product Browser"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 6
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "ShowBuyButton"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Show Buy Button"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strOptions(4) = "1" Or "" & strOptions(4).ToLower = "true" Then ' TJS 24/02/12
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "True" ' TJS 24/02/12
                    Else
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                    End If
                Case 7
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "Wholesale"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = rowASPStorefrontTagDetails.TagName_DEV000221
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 8
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "RequiresRegistration"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Requires Registration"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strOptions(1) = "1" Or "" & strOptions(1).ToLower = "true" Then ' TJS 24/02/12
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "True"
                    Else
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False"
                    End If
                Case 9
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "HidePriceUntilCart"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Hide Price Until Cart"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strOptions(2) = "1" Or "" & strOptions(2).ToLower = "true" Then ' TJS 24/02/12
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "True"
                    Else
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False"
                    End If
                Case 10
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "IsCallToOrder"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Is Call To Order"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    If "" & strOptions(3) = "1" Or "" & strOptions(3).ToLower = "true" Then ' TJS 24/02/12
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "True"
                    Else
                        rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False"
                    End If
                Case 11
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "ExcludeFromPriceFeeds"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Exclude From Price Feeds"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 12
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "GoogleCheckoutAllowed"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Google Checkout Allowed"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                Case 13
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "RequiresTextOption"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Requires Text Option"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "False" ' TJS 24/02/12
                    ' start of code added TJS 24/02/12
                Case 14
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "Published"
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Published"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "root"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = 1
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = "True"
                    ' end of code added TJS 24/02/12
            End Select
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Boolean"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
            m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

    End Sub
#End Region

#Region " GetASPStorefrontCategories "
    Private Sub GetASPStorefrontCategories(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles m_BackgroundWorker.DoWork
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 09/04/11 | TJS             | 2011.0.10 | Code moved to ASPStorefrontImportFacade
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        m_ASPStorefrontImportFacade.GetASPStorefrontCategories(Me.cbeSelectASPSite.EditValue.ToString, worker.CancellationPending, m_CategoryTable)

    End Sub

    Private Sub GetGetASPStorefrontCategoriesCategoriesCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles m_BackgroundWorker.RunWorkerCompleted
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to only enable category tree if publish is checked 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.TreeListCategories.DataSource = m_CategoryTable
        Me.TreeListCategories.ExpandAll()
        If Me.CheckEditPublishOnASPStorefront.Checked Then ' TJS 24/02/12
            Me.TreeListCategories.Enabled = True
        End If
        Me.pnlGetCategoryProgress.Visible = False
        Cursor = Cursors.Default

    End Sub
#End Region
#End Region

#Region " Events "
#Region " ASPStorefrontSiteSelected "
    Private Sub ASPStorefrontSiteSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 09/04/11 | TJS             | 2011.0.09 | Modified to initialise category list in Import facade
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to cater for all source config records being loaded
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to use Categories and Tag Properties from MAtrix Group on Matrix Items
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim colNewColumn As DataColumn, strTemp As String ' TJS 24/02/12
        Dim iLoop As Integer, iColonPosn As Integer, iSelectedSiteIndex As Integer ' TJS 02/12/11

        Try
            Cursor = Cursors.WaitCursor
            If Me.cbeSelectASPSite.SelectedIndex >= 0 Then
                ' save Site Index ID
                iSelectedSiteIndex = Me.cbeSelectASPSite.SelectedIndex ' TJS 02/12/11
                ' get ASPStorefront publishing details - are we displaying a Matrix Item
                If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Then ' TJS 24/02/12
                    ' yes, get ItemCode for Matrix Group Item
                    strTemp = Me.m_InventorySettingsFacade.GetField("SELECT ItemCode FROM dbo.InventoryMatrixItem WHERE MatrixItemCode = '" & _
                        Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'", CommandType.Text, Nothing) ' TJS 24/02/12

                    Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.TableName, _
                            "ReadInventoryASPStorefrontDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_SITE_ID, Me.cbeSelectASPSite.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontCategories_DEV000221.TableName, _
                            "ReadInventoryASPStorefrontCategories_DEV000221", AT_ITEM_CODE, strTemp, AT_SITE_ID, Me.cbeSelectASPSite.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.TableName, _
                            "ReadInventoryASPStorefrontTagDetails_DEV000221", AT_ITEM_CODE, strTemp, AT_SITE_ID, Me.cbeSelectASPSite.EditValue.ToString}}, _
                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 24/02/12
                Else
                    Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.TableName, _
                            "ReadInventoryASPStorefrontDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_SITE_ID, Me.cbeSelectASPSite.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontCategories_DEV000221.TableName, _
                            "ReadInventoryASPStorefrontCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_SITE_ID, Me.cbeSelectASPSite.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.TableName, _
                            "ReadInventoryASPStorefrontTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_SITE_ID, Me.cbeSelectASPSite.EditValue.ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 02/12/11

                End If

                Me.cbeSelectASPSite.SelectedIndex = iSelectedSiteIndex ' TJS 02/12/11
                For iLoop = 0 To Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.Count - 1
                    Select Case Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).TagDataType_DEV000221
                        Case "Text", "Boolean"
                            If Not Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).IsTagTextValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).TagTextValue_DEV000221
                            End If

                        Case "Memo"
                            If Not Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).IsTagMemoValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221
                            End If

                        Case "Date", "DateTime"
                            If Not Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).IsTagDateValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).TagDateValue_DEV000221.ToString
                            End If

                        Case "Integer", "Numeric"
                            If Not Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).IsTagNumericValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).TagNumericValue_DEV000221.ToString
                            End If

                        Case "GUID"
                            If Not Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).IsTagNumericValue_DEV000221Null Then
                                iColonPosn = Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).TagTextValue_DEV000221.IndexOf(":")
                                If iColonPosn > 0 Then
                                    Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).TagTextValue_DEV000221.Substring(iColonPosn + 1)
                                End If
                            End If

                    End Select
                Next
                XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(ASP_STORE_FRONT_SOURCE_CODE).ConfigSettings_DEV000221)) ' TJS 02/12/11
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID) = Me.cbeSelectASPSite.EditValue.ToString Then
                        If m_BackgroundWorker IsNot Nothing Then
                            m_BackgroundWorker.CancelAsync()
                        End If
                        m_ASPStorefrontImportFacade = New Lerryn.Facade.ImportExport.ASPStorefrontImportFacade(Me.m_InventorySettingsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 09/04/11 TJS 02/12/11 TJS 10/06/12
                        ReDim m_ASPStorefrontImportFacade.ASPStorefrontCategories(0) ' TJS 09/04/11
                        m_ASPStorefrontImportFacade.ASPStorefrontCategories(0).CategoryID = 0 ' TJS 09/04/11
                        m_CategoryTable = New DataTable

                        colNewColumn = New DataColumn
                        colNewColumn.Caption = "Active"
                        colNewColumn.ColumnName = "Active"
                        colNewColumn.DataType = System.Type.GetType("System.Boolean")
                        m_CategoryTable.Columns.Add(colNewColumn)
                        colNewColumn.Dispose()

                        colNewColumn = New DataColumn
                        colNewColumn.Caption = "Category"
                        colNewColumn.ColumnName = "SourceCategoryName"
                        colNewColumn.DataType = System.Type.GetType("System.String")
                        m_CategoryTable.Columns.Add(colNewColumn)
                        colNewColumn.Dispose()

                        colNewColumn = New DataColumn
                        colNewColumn.Caption = "ID"
                        colNewColumn.ColumnName = "SourceCategoryID"
                        colNewColumn.DataType = System.Type.GetType("System.Decimal")
                        m_CategoryTable.Columns.Add(colNewColumn)
                        colNewColumn.Dispose()

                        colNewColumn = New DataColumn
                        colNewColumn.Caption = "Parent"
                        colNewColumn.ColumnName = "SourceParentID"
                        colNewColumn.DataType = System.Type.GetType("System.Decimal")
                        m_CategoryTable.Columns.Add(colNewColumn)
                        colNewColumn.Dispose()

                        colNewColumn = New DataColumn
                        colNewColumn.Caption = "GUID"
                        colNewColumn.ColumnName = "CategoryGUID"
                        colNewColumn.DataType = System.Type.GetType("System.String")
                        m_CategoryTable.Columns.Add(colNewColumn)
                        colNewColumn.Dispose()

                        Me.TreeListCategories.Enabled = False
                        Me.pnlGetCategoryProgress.Visible = True

                        ' get ASP Storefront Categories as background task
                        If m_BackgroundWorker Is Nothing Then
                            m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                            m_BackgroundWorker.WorkerSupportsCancellation = True
                            m_BackgroundWorker.WorkerReportsProgress = False
                        End If
                        m_BackgroundWorker.RunWorkerAsync()
                        Exit For
                    End If
                Next

                ' has ASP Storefront detail record been created yet ?
                If Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.Count > 0 Then
                    ' yes, 

                End If
                ' now enable Browse List controls etc
                If Me.CheckEditPublishOnASPStorefront.Checked Then
                    EnableDisableASPStorefrontControls(True)
                Else
                    EnableDisableASPStorefrontControls(False)
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            If m_BackgroundWorker Is Nothing Then
                Cursor = Cursors.Default
            ElseIf Not m_BackgroundWorker.IsBusy Then
                Cursor = Cursors.Default
            End If

        End Try

    End Sub
#End Region

#Region " PublishOnASPStorefrontChange "
    Private Sub PublishOnASPStorefrontChange(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowASPStorefrontDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontDetails_DEV000221Row

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.CheckEditPublishOnASPStorefront.EditValueChanged, AddressOf PublishOnASPStorefrontChange
            If CheckEditPublishOnASPStorefront.EditValue.ToString <> "" Then
                If CBool(Me.CheckEditPublishOnASPStorefront.EditValue) Then
                    If Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.Rows.Count = 0 Then
                        rowASPStorefrontDetails = Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.NewInventoryASPStorefrontDetails_DEV000221Row
                        rowASPStorefrontDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        rowASPStorefrontDetails.Publish_DEV000221 = True
                        rowASPStorefrontDetails.SellingPrice_DEV000221 = 0
                        rowASPStorefrontDetails.SiteID_DEV000221 = Me.cbeSelectASPSite.EditValue.ToString
                        rowASPStorefrontDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
                        rowASPStorefrontDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
                        Me.m_InventorySettingsDataset.InventoryASPStorefrontDetails_DEV000221.AddInventoryASPStorefrontDetails_DEV000221Row(rowASPStorefrontDetails)
                        SetProposedASPStorefrontPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, "")
                    End If

                    If Me.m_InventorySettingsDataset.InventoryASPStorefrontTagDetails_DEV000221.Rows.Count = 0 Then ' TJS 02/12/11
                        CreateASPStorefrontTags() ' TJS 02/12/11
                    End If


                    EnableDisableASPStorefrontControls(True)
                Else
                    EnableDisableASPStorefrontControls(False)
                End If
            Else
                EnableDisableASPStorefrontControls(False)
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.CheckEditPublishOnASPStorefront.EditValueChanged, AddressOf PublishOnASPStorefrontChange

        End Try

    End Sub
#End Region

#Region " CategoryTreeValueChanged "
    Private Sub CategoryTreeValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.CellValueChangedEventArgs) Handles TreeListCategories.CellValueChanging
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowTreeList As System.Data.DataRowView
        Dim rowASPStorefrontCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontCategories_DEV000221Row

        Try

            If e.Column.Name = "colTreeListCategoryActive" Then
                rowTreeList = DirectCast(Me.TreeListCategories.GetDataRecordByNode(e.Node), System.Data.DataRowView)
                rowASPStorefrontCategory = Me.m_InventorySettingsDataset.InventoryASPStorefrontCategories_DEV000221.FindByItemCode_DEV000221CategoryGUID_DEV000221SiteID_DEV000221(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, rowTreeList("CategoryGUID").ToString, Me.cbeSelectASPSite.EditValue.ToString)
                If CBool(e.Value) Then
                    If rowASPStorefrontCategory Is Nothing Then
                        rowASPStorefrontCategory = Me.m_InventorySettingsDataset.InventoryASPStorefrontCategories_DEV000221.NewInventoryASPStorefrontCategories_DEV000221Row
                        rowASPStorefrontCategory.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        rowASPStorefrontCategory.SiteID_DEV000221 = Me.cbeSelectASPSite.EditValue.ToString
                        rowASPStorefrontCategory.CategoryGUID_DEV000221 = rowTreeList("CategoryGUID").ToString
                        rowASPStorefrontCategory.CategoryID_DEV000221 = CInt(rowTreeList("SourceCategoryID"))
                        rowASPStorefrontCategory.IsActive_DEV000221 = True
                        Me.m_InventorySettingsDataset.InventoryASPStorefrontCategories_DEV000221.AddInventoryASPStorefrontCategories_DEV000221Row(rowASPStorefrontCategory)
                    Else
                        If Not rowASPStorefrontCategory.IsActive_DEV000221 Then
                            rowASPStorefrontCategory.IsActive_DEV000221 = True
                        End If
                    End If
                Else
                    If rowASPStorefrontCategory IsNot Nothing AndAlso rowASPStorefrontCategory.IsActive_DEV000221 Then
                        rowASPStorefrontCategory.IsActive_DEV000221 = False
                    End If
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try

    End Sub
#End Region

#Region " ASPStorefrontPriceAsIS "
    Private Sub ASPStorefrontPriceAsIS(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPriceAsIS.CheckedChanged

        If Me.chkPriceAsIS.Checked Then
            Me.lblASPSellingPrice.Enabled = False
            Me.TextEditASPSellingPrice.Enabled = False
        Else
            Me.lblASPSellingPrice.Enabled = True
            Me.TextEditASPSellingPrice.Enabled = True
        End If
    End Sub
#End Region

#Region " QtyPublishingTypeChanged "
    Private Sub QtyPublishingTypeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroupQtyPublishing.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If RadioGroupQtyPublishing.SelectedIndex >= 0 Then
            Select Case RadioGroupQtyPublishing.EditValue.ToString
                Case "Fixed"
                    Me.TextEditQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Text = "Value to Publish"

                Case "Percent"
                    Me.TextEditQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Text = "% of Total Qty"

                Case Else
                    Me.TextEditQtyPublishingValue.Enabled = False
                    Me.lblQtyPublishingValue.Enabled = False
                    Me.lblQtyPublishingValue.Text = ""

            End Select
        End If
    End Sub
#End Region

#Region " PropertiesGridFunctions "
    Private Sub GridViewProperties_CustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridViewProperties.CustomDrawCell

        If e.Column.Name = "colPropertiesDisplayValue" Then
            Select Case Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagDataType_DEV000221").ToString
                Case "Text"
                    e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagTextValue_DEV000221").ToString
                    e.Handled = True

                Case "Memo"
                    e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagMemoValue_DEV000221").ToString
                    e.Handled = True

                Case "Date", "DateTime"
                    e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagMemoValue_DEV000221").ToString
                    e.Handled = True

                Case "Integer", "Numeric"
                    e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagMemoValue_DEV000221").ToString
                    e.Handled = True

            End Select
        End If

    End Sub

    Private Sub GridViewProperties_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewProperties.FocusedRowChanged

        If e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then
            Select Case Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text", "Boolean"
                    Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagTextValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))

                Case "Memo"
                    Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagMemoValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))

                Case "Date", "DateTime"
                    Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagDateValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))

                Case "Integer", "Numeric"
                    Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagNumericValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))

            End Select
        End If

        If e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then

            Select Case Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit

                Case "Memo"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeMemoEdit

                Case "Date", "DateTime"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeDateEdit

                Case "Integer", "Numeric"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit

                Case "Boolean"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTrueFalseEdit

                Case "GUID"

                Case Else
                    Me.colPropertiesDisplayValue.ColumnEdit = rbeTextEdit
            End Select
        End If

    End Sub
#End Region

#Region " ProductNameEdit "
    Private Sub TextEditProductName_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditProductName.Enter
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION And Not MyBase.IsReadOnly And Not Me.TextEditProductName.Properties.ReadOnly Then ' TJS 13/11/13
            TextEditProductName.Text = String.Empty
            TextEditProductName.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextEditProductName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditProductName.Leave
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text.Trim.Length = 0 And Not MyBase.IsReadOnly And Not Me.TextEditProductName.Properties.ReadOnly Then ' TJS 13/11/13
            TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION
            TextEditProductName.ForeColor = Color.Gray
        End If

    End Sub
#End Region

#Region " ProductDescriptionEdit "
    Private Sub MemoEditDescription_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemoEditDescription.Enter
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION And Not MyBase.IsReadOnly And Not Me.MemoEditDescription.Properties.ReadOnly Then ' TJS 13/11/13
            MemoEditDescription.Text = String.Empty
            MemoEditDescription.ForeColor = Color.Black
        End If
    End Sub

    Private Sub MemoEditDescription_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemoEditDescription.Leave
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text.Trim.Length = 0 And Not MyBase.IsReadOnly And Not Me.MemoEditDescription.Properties.ReadOnly Then
            MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
            MemoEditDescription.ForeColor = Color.Gray
        End If

    End Sub
#End Region
#End Region
End Class
#End Region
