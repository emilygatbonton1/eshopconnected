' eShopCONNECT for Connected Business
' Module: InventoryAmazonSettingsSection.vb
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

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " InventoryAmazonSettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.InventoryAmazonSettingsSection")> _
Public Class InventoryAmazonSettingsSection

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private WithEvents m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade ' TJS 27/06/09
    Private WithEvents frmPropertyHelp As PropertyHelpForm
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.InventoryAmazonSettingsSectionGateway
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
    Public Property InventoryItemFacade() As Interprise.Facade.Inventory.ItemDetailFacade ' TJS 27/06/09
        Get
            Return Me.m_InventoryItemFacade ' TJS 27/06/09
        End Get
        Set(ByVal value As Interprise.Facade.Inventory.ItemDetailFacade) ' TJS 27/06/09
            Me.m_InventoryItemFacade = value ' TJS 27/06/09
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
                Me.PanelPublishOnAmazon.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnAmazon.BringToFront()
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
                Me.PanelPublishOnAmazon.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnAmazon.BringToFront()
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
        ' 05/06/09 | TJS             | 2009.2.10 | Modified to load Amazon config settings
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to only set Published checkbox if Amazon detail record exists
        ' 01/12/09 | TJS             | 2009.3.09 | Modified to cater for XML Class, Type and SubType
        ' 19/08/10 | TJS             | 2010.1.00 | Modifies to use Source Code constants and to cater for Publish flag now being on InventoryAmazonDetails table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection ' TJS 01/12/09
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem ' TJS 01/12/09
        Dim strNodeID As String, iLoop As Integer, iCheckLoop As Integer ' TJS 27/06/09 TJS 01/12/09
        Dim bItemExists As Boolean ' TJS 01/12/09

        ' is eShopCONNECT activated ?
        If Me.m_InventorySettingsFacade.IsActivated Then
            ' yes, is Amazon connector activated ?
            If Me.m_InventorySettingsFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                ' yes, need to check if there is an Item record in case we are being configured in the User Role !
                If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Rows.Count > 0 Then ' TJS 02/12/11
                    ' start of code added TJS 01/12/09
                    Me.m_InventorySettingsFacade.GetAmazonXMLTypeDetails()
                    ' populate Amazon XML Class list
                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.TableName, _
                        "ReadLerrynImportExportAmazonXMLTypeDetails_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                    Coll = Me.ComboBoxEditXMLClass.Properties.Items
                    Coll.BeginUpdate()
                    For iLoop = 0 To Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.Count - 1
                        ' XML Class field is part of Primary key and can't be null.  IS tries to save empty strings as null so causes errors
                        ' . used as empty field indicator and should be ignored when polulating list box
                        If Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLClass_DEV000221 <> "." Then
                            ' need to avoid adding records more than once
                            bItemExists = False
                            For iCheckLoop = 0 To Coll.Count - 1
                                If Coll.Item(iCheckLoop).ToString = Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLClass_DEV000221 Then
                                    bItemExists = True
                                End If
                            Next
                            If Not bItemExists Then
                                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLClass_DEV000221)
                                Coll.Add(CollItem)
                            End If
                        End If
                    Next
                    Coll.EndUpdate()
                    ' end of code added TJS 01/12/09

                    ' has Item been saved yet (i.e. ItemCode is not [To be generated])
                    If Me.m_InventoryItemDataset.InventoryItem(0).ItemCode <> Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
                        ' yes, enable Amazon details tab
                        Me.PanelPublishOnAmazon.Enabled = True
                        ' get Amazon publishing details 
                        Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryItem.TableName, _
                                "ReadInventoryItem", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.TableName, _
                                "ReadInventoryAmazonDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
                                "ReadInventoryAmazonTagDetailTemplateView_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_EXCLUDE_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryAmazonASIN_DEV000221.TableName, _
                                "ReadInventoryAmazonASIN_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryAmazonSearchTerms.TableName, _
                                "ReadInventoryAmazonTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}, _
                            New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                                "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, AMAZON_SOURCE_CODE}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 05/06/09 TJS 19/08/10

                        ' has Amazon detail record been created yet ?
                        If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.Count > 0 Then ' TJS 26/05/09
                            ' yes, get updates for this category's Attribute Categories 
                            strNodeID = ExtractAmazonBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221) ' TJS 27/06/09
                            If strNodeID <> "" Then ' TJS 27/06/09
                                Me.m_InventorySettingsFacade.GetAmazonBrowseListItems(strNodeID, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221) ' TJS 27/06/09
                            Else
                                Me.m_InventorySettingsFacade.GetAmazonBrowseListItems(AMAZON_ROOT_CATEGORY, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221) ' TJS 27/06/09
                            End If

                            ' now populate Browse Tree list with next level
                            If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseNodeID_DEV000221Null Then ' TJS 26/05/09
                                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                                    "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, _
                                    AT_PARENT_NODE, AMAZON_ROOT_CATEGORY}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 26/05/09
                                Me.LabelDescriptiveCategory.Visible = False ' TJS 27/06/09
                            Else
                                ' get descriptive category status
                                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                                    "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, _
                                    AT_NODE_ID, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221, _
                                    AT_PARENT_CATEGORY, ExtractAmazonBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221)}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 27/06/09 TJS 01/12/09
                                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.Count > 0 Then ' TJS 27/06/09
                                    Me.LabelDescriptiveCategory.Visible = Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(0).DescriptiveCategory_DEV000221 ' TJS 27/06/09
                                Else
                                    Me.LabelDescriptiveCategory.Visible = False ' TJS 27/06/09
                                End If
                                ' now get browse list
                                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                                    "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, _
                                    AT_PARENT_NODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221, _
                                    AT_PARENT_CATEGORY, ExtractAmazonBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221)}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 26/05/09 TJS 01/12/09
                            End If

                            PopulateBrowseNodeList() ' TJS 26/05/09
                            PopulateXMLTypeList() ' TJS 01/12/09
                            PopulateXMLSubTypeList() ' TJS 01/12/09

                            ' now enable Browse List controls etc
                            If Me.CheckEditPublishOnAmazon.Checked Then ' TJS 26/05/09
                                EnableDisableAmazonControls(True)
                            Else
                                EnableDisableAmazonControls(False) ' TJS 26/05/09
                            End If
                        End If

                    End If
                    AddHandler Me.CheckEditPublishOnAmazon.EditValueChanged, AddressOf PublishOnAmazonChange
                    AddHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected
                    AddHandler Me.ComboBoxEditXMLClass.EditValueChanged, AddressOf XMLClassSelected ' TJS 01/12/09
                    AddHandler Me.ComboBoxEditXMLType.EditValueChanged, AddressOf XMLTypeSelected ' TJS 01/12/09
                    AddHandler Me.ComboBoxEditXMLSubType.EditValueChanged, AddressOf XMLSubTypeSelected ' TJS 01/12/09
                    ' for now show development message to prevent user's trying non-functional code
                    ShowDevelopmentMessage = True ' TJS 02/12/11

                Else
                    ' for now show development message to prevent user's trying non-functional code
                    ShowDevelopmentMessage = True ' TJS 02/12/11
                End If
            Else
                ' for now show development message to prevent user's trying non-functional code
                ShowDevelopmentMessage = True ' TJS 02/12/11
            End If
        Else
            ' for now show development message to prevent user's trying non-functional code
            ShowDevelopmentMessage = True ' TJS 02/12/11
        End If

    End Sub
#End Region

#Region " EnableDisableAmazonControls "
    Private Sub EnableDisableAmazonControls(ByVal Enable As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Modified to correctly detect if Browse Node is set or not
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to cater for Descriptive CAtegory label
        ' 01/12/09 | TJS             | 2009.3.09 | Modified to disable certain settings on matrix group items
        '                                        | and for Amazon site now being set from Merchant ID config
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bBrowseNodeSet As Boolean, bAmazonSiteSet As Boolean, bMatrixGroupItem As Boolean, bMatrixItem As Boolean ' TJS 01/12/09
        Dim strTemp As String, strProperties() As String, strNodeID As String ' TJS 01/12/09

        ' has an Amazon Details record been created ?
        If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.Rows.Count > 0 Then
            ' yes, has Amazon Site been set ?
            If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221 <> "" Then
                bAmazonSiteSet = True
                ' yes, has Browse Tree been set ?
                If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseNodeID_DEV000221Null Then
                    bBrowseNodeSet = False
                ElseIf CInt(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221) <> CInt(AMAZON_ROOT_CATEGORY) Then ' TJS 26/05/09 TJS 01/12/09
                    ' yes, enable all controls
                    bBrowseNodeSet = True
                Else
                    bBrowseNodeSet = False
                End If
            Else
                bAmazonSiteSet = False
                bBrowseNodeSet = False
            End If
        Else
            bAmazonSiteSet = False
            bBrowseNodeSet = False
        End If

        ' start of code added TJS 01/12/09
        ' is item a Matrix Group Item or a Matrix Item ?
        If Me.m_InventoryItemDataset.InventoryItem.Count > 0 Then
            If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Group" Then
                bMatrixGroupItem = True
                bMatrixItem = False
            ElseIf Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Item" Then
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
            strProperties = Me.m_InventorySettingsFacade.GetRow(New String() {"ProductName_DEV000221", "ProductDescription_DEV000221", _
                "SiteCode_DEV000221", "BrowseNodeID_DEV000221", "BrowseTree_DEV000221", "BrowseTreeChain_DEV000221", "ProductIDType_DEV000221", _
                "ProductID_DEV000221", "ImageURL_DEV000221"}, "InventoryAmazonDetails_DEV000221", "ItemCode_DEV000221 = '" & strTemp & "'", False)
            If strProperties IsNot Nothing Then
                ' remove data bindings for relevant fields
                Me.TextEditProductName.DataBindings.Clear()
                Me.MemoEditDescription.DataBindings.Clear()
                ' now display Matrix Group Items properties
                Me.TextEditProductName.EditValue = strProperties(0)
                Me.MemoEditDescription.EditValue = strProperties(1)
                ' now update browse node list
                strNodeID = ExtractAmazonBrowseTreeRoot(strProperties(5))

                If strNodeID <> "" Then
                    Me.m_InventorySettingsFacade.GetAmazonBrowseListItems(strNodeID, strProperties(2))
                Else
                    Me.m_InventorySettingsFacade.GetAmazonBrowseListItems(AMAZON_ROOT_CATEGORY, strProperties(2))
                End If
            End If

            ' now populate Browse Tree list with next level
            If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseNodeID_DEV000221Null Then
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                    "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, _
                    AT_PARENT_NODE, AMAZON_ROOT_CATEGORY}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                Me.LabelDescriptiveCategory.Visible = False
            Else
                ' get descriptive category status
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                    "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, _
                    AT_NODE_ID, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221, _
                    AT_PARENT_CATEGORY, ExtractAmazonBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221)}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 01/12/09
                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.Count > 0 Then
                    Me.LabelDescriptiveCategory.Visible = Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(0).DescriptiveCategory_DEV000221
                Else
                    Me.LabelDescriptiveCategory.Visible = False
                End If
                ' now get browse list
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                    "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, _
                    AT_PARENT_NODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221, _
                    AT_PARENT_CATEGORY, ExtractAmazonBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221)}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 01/12/09
            End If
            PopulateBrowseNodeList()

        End If
        ' end of code added TJS 01/12/09

        ' these controls are enabled to allow selection of Amazon Merchant ID 
        Me.LabelAmazonMerchantID.Enabled = Enable
        Me.ComboBoxEditAmazonMerchantID.Enabled = Enable
        Me.LabelAmazonSiteCode.Enabled = Enable
        Me.TextEditAmazonSiteCode.Enabled = Enable

        ' these controls are enabled if Site Code is set
        Me.LabelBrowseTree.Enabled = bAmazonSiteSet And Enable
        Me.LabelDescriptiveCategory.Enabled = bAmazonSiteSet And Enable ' TJS 27/06/09
        ' does list contain any items ?
        If Me.ListBoxBrowseList.ItemCount > 0 Then
            ' yes, use 
            Me.ListBoxBrowseList.Enabled = bAmazonSiteSet And Enable And Not bMatrixItem ' TJS 01/12/09
        Else
            Me.ListBoxBrowseList.Enabled = False
        End If
        Me.TextEditBrowseTree.Enabled = bAmazonSiteSet And Enable And Not bMatrixItem ' TJS 01/12/09

        ' display labels for Matrix Items and Matrix Group Items
        Me.LabelMatrixGroupItem.Visible = bMatrixGroupItem And Enable ' TJS 01/12/09
        Me.LabelMatrixItem.Visible = bMatrixItem And Enable ' TJS 01/12/09

        ' these controls are enabled if Browse Node is set
        Me.btnBrowseNodeUpLevel.Enabled = bBrowseNodeSet And Enable And Not bMatrixItem ' TJS 30/09/09
        Me.CheckEditSaleActive.Enabled = bBrowseNodeSet And Enable
        Me.ComboBoxEditCondition.Enabled = bBrowseNodeSet And Enable
        Me.DateEditLaunchDate.Enabled = bBrowseNodeSet And Enable
        Me.DateEditReleaseDate.Enabled = bBrowseNodeSet And Enable
        Me.DateEditSaleStartDate.Enabled = bBrowseNodeSet And Enable
        Me.DateEditSaleEndDate.Enabled = bBrowseNodeSet And Enable
        Me.GridControlASIN.Enabled = bBrowseNodeSet And Enable
        Me.GridControlProperties.Enabled = bBrowseNodeSet And Enable
        Me.GridControlSearchTerms.Enabled = bBrowseNodeSet And Enable
        Me.LabelAmazonSellingPrice.Enabled = bBrowseNodeSet And Enable
        Me.LabelBrowseNodeID.Enabled = bBrowseNodeSet And Enable And Not bMatrixItem ' TJS 30/09/09
        Me.LabelCondition.Enabled = bBrowseNodeSet And Enable
        Me.LabelConditionNote.Enabled = bBrowseNodeSet And Enable
        Me.LabelDescription.Enabled = bBrowseNodeSet And Enable And Not bMatrixItem ' TJS 30/09/09
        Me.LabelLaunchDate.Enabled = bBrowseNodeSet And Enable
        Me.LabelProductName.Enabled = bBrowseNodeSet And Enable And Not bMatrixItem ' TJS 30/09/09
        Me.LabelProperties.Enabled = bBrowseNodeSet And Enable
        Me.LabelReleaseDate.Enabled = bBrowseNodeSet And Enable
        Me.LabelSaleEndDate.Enabled = bBrowseNodeSet And Enable
        Me.LabelSalePrice.Enabled = bBrowseNodeSet And Enable
        Me.LabelSaleStartDate.Enabled = bBrowseNodeSet And Enable
        Me.LabelImageURL.Enabled = bBrowseNodeSet And Enable ' TJS 25/11/09
        Me.MemoEditConditionNote.Enabled = bBrowseNodeSet And Enable
        Me.MemoEditDescription.Enabled = bBrowseNodeSet And Enable And Not bMatrixItem ' TJS 30/09/09
        Me.TextEditAmazonSellingPrice.Enabled = bBrowseNodeSet And Enable
        Me.TextEditBrowseNodeID.Enabled = bBrowseNodeSet And Enable And Not bMatrixItem ' TJS 30/09/09
        Me.TextEditProductName.Enabled = bBrowseNodeSet And Enable And Not bMatrixItem ' TJS 30/09/09
        Me.TextEditSalePrice.Enabled = bBrowseNodeSet And Enable
        Me.TextEditImageURL.Enabled = bBrowseNodeSet And Enable ' TJS 25/11/09
        If bBrowseNodeSet And Enable And Not bMatrixItem Then ' TJS 01/12/09
            If Me.ComboBoxEditXMLClass.Properties.Items.Count > 0 Then ' TJS 01/12/09
                Me.LabelXMLClass.Enabled = True ' TJS 01/12/09
                Me.ComboBoxEditXMLClass.Enabled = True ' TJS 01/12/09
            Else
                Me.LabelXMLClass.Enabled = False ' TJS 01/12/09
                Me.ComboBoxEditXMLClass.Enabled = False ' TJS 01/12/09
            End If
            If Me.ComboBoxEditXMLType.Properties.Items.Count > 0 Then ' TJS 01/12/09
                Me.LabelXMLType.Enabled = True ' TJS 01/12/09
                Me.ComboBoxEditXMLType.Enabled = True ' TJS 01/12/09
            Else
                Me.LabelXMLType.Enabled = False ' TJS 01/12/09
                Me.ComboBoxEditXMLType.Enabled = False ' TJS 01/12/09
            End If
            ' ProductSubType includes a blank row so a count of 1 is still empty
            If Me.ComboBoxEditXMLSubType.Properties.Items.Count > 1 Then ' TJS 01/12/09
                Me.LabelXMLSubType.Enabled = True ' TJS 01/12/09
                Me.ComboBoxEditXMLSubType.Enabled = True ' TJS 01/12/09
            Else
                Me.LabelXMLSubType.Enabled = False ' TJS 01/12/09
                Me.ComboBoxEditXMLSubType.Enabled = False ' TJS 01/12/09
            End If
        Else
            Me.LabelXMLClass.Enabled = False ' TJS 01/12/09
            Me.LabelXMLType.Enabled = False ' TJS 01/12/09
            Me.LabelXMLSubType.Enabled = False ' TJS 01/12/09
            Me.ComboBoxEditXMLClass.Enabled = False ' TJS 01/12/09
            Me.ComboBoxEditXMLType.Enabled = False ' TJS 01/12/09
            Me.ComboBoxEditXMLSubType.Enabled = False ' TJS 01/12/09
        End If

        ' now set Product name and description text colour
        If Me.TextEditProductName.Enabled Then ' TJS 27/06/09
            If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 27/06/09 TJS 13/11/13
                TextEditProductName.ForeColor = Color.Gray ' TJS 27/06/09
            Else
                TextEditProductName.ForeColor = Color.Black ' TJS 27/06/09 
            End If
        End If
        If Me.MemoEditDescription.Enabled Then ' TJS 27/06/09
            If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then ' TJS 27/06/09 TJS 13/11/13
                MemoEditDescription.ForeColor = Color.Gray ' TJS 27/06/09
            Else
                MemoEditDescription.ForeColor = Color.Black ' TJS 27/06/09 
            End If
        End If

        If bBrowseNodeSet And Enable Then
            Me.GridViewASIN.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
            Me.GridViewProperties.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Else
            Me.GridViewASIN.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            Me.GridViewProperties.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
        End If

    End Sub
#End Region

#Region " SetProposedAmazonPrice "
    Private Sub SetProposedAmazonPrice(ByVal ItemCode As String, ByVal CurrencyCode As String)
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
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLAmazonNode As XElement
        Dim XMLAmazonNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strSQL As String, strTemp As String, bUpdatePrice As Boolean

        ' get config settings
        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(AMAZON_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
        XMLAmazonNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
        ' check connector count is valid i.e. number of Amazon settings is not more then the licence limit
        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLAmazonNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
            ' check each amazon record for current Merchant ID
            For Each XMLAmazonNode In XMLAmazonNodeList
                XMLTemp = XDocument.Parse(XMLAmazonNode.ToString)
                ' have we found current Merchant ID ?
                If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN) = Me.ComboBoxEditAmazonMerchantID.Text Then ' tjs 22/03/13
                    ' yes, has Amazon price been set ?
                    bUpdatePrice = False
                    If Me.TextEditAmazonSellingPrice.Text <> "" Then
                        If CDec(Me.TextEditAmazonSellingPrice.EditValue) = 0 Then
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
                            Me.TextEditAmazonSellingPrice.EditValue = CDec(strTemp)
                            ' now get default uplift percent
                            strTemp = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_DEFAULT_PRICE_UPLIFT)
                            ' is it set ?
                            If strTemp <> "" Then
                                ' uplift price
                                Me.TextEditAmazonSellingPrice.EditValue = CDec(Me.TextEditAmazonSellingPrice.EditValue) * ((100 + CDec(strTemp)) / 100)
                            End If
                        End If
                    End If
                End If
                Exit For
            Next
        End If

    End Sub
#End Region

#Region " PopulateBrowseNodeList "
    Private Sub PopulateBrowseNodeList()

        Dim iLoop As Integer

        Me.ListBoxBrowseList.Items.BeginUpdate()
        Me.ListBoxBrowseList.Items.Clear()
        For iLoop = 0 To Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.Count - 1
            Me.ListBoxBrowseList.Items.Add(Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).NodeName_DEV000221)
        Next
        Me.ListBoxBrowseList.Items.EndUpdate()
        ' make sure no list item selected
        Me.ListBoxBrowseList.SelectedIndex = -1

    End Sub
#End Region

#Region " PopulateXMLTypeList "
    Private Sub PopulateXMLTypeList()

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim iLoop As Integer, iCheckLoop As Integer, bItemExists As Boolean

        Coll = Me.ComboBoxEditXMLType.Properties.Items
        Coll.BeginUpdate()
        ' clear any existing list values
        Coll.Clear()
        For iLoop = 0 To Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.Count - 1
            ' only populate list with values which apply to the selected XML Class
            If Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLClass_DEV000221 = Me.ComboBoxEditXMLClass.EditValue.ToString Then
                ' XML Type field is part of Primary key and can't be null.  IS tries to save empty strings as null so causes errors
                ' . used as empty field indicator and should be ignored when populating list box
                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLType_DEV000221 <> "." Then
                    ' need to avoid adding records more than once
                    bItemExists = False
                    For iCheckLoop = 0 To Coll.Count - 1
                        If Coll.Item(iCheckLoop).ToString = Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLType_DEV000221 Then
                            bItemExists = True
                        End If
                    Next
                    If Not bItemExists Then
                        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLType_DEV000221)
                        Coll.Add(CollItem)
                    End If
                End If
            End If
        Next
        Coll.EndUpdate()

    End Sub
#End Region

#Region " PopulateXMLSubTypeList "
    Private Sub PopulateXMLSubTypeList()

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim iLoop As Integer, iCheckLoop As Integer, bItemExists As Boolean

        Coll = Me.ComboBoxEditXMLSubType.Properties.Items
        Coll.BeginUpdate()
        ' clear any existing list values
        Coll.Clear()
        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("")
        Coll.Add(CollItem)
        For iLoop = 0 To Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.Count - 1
            ' only populate list with values which apply to the selected XML Class and Type
            If Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLClass_DEV000221 = Me.ComboBoxEditXMLClass.EditValue.ToString And _
                Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLType_DEV000221 = Me.ComboBoxEditXMLType.EditValue.ToString Then
                ' XML Sub Type field is part of Primary key and can't be null.  IS tries to save empty strings as null so causes errors
                ' . used as empty field indicator and should be ignored when populating list box
                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLSubType_DEV000221 <> "." Then
                    ' need to avoid adding records more than once
                    bItemExists = False
                    For iCheckLoop = 0 To Coll.Count - 1
                        If Coll.Item(iCheckLoop).ToString = Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLSubType_DEV000221 Then
                            bItemExists = True
                        End If
                    Next
                    If Not bItemExists Then
                        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(Me.m_InventorySettingsDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221(iLoop).XMLSubType_DEV000221)
                        Coll.Add(CollItem)
                    End If
                End If
            End If
        Next
        Coll.EndUpdate()

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
        ' 26/05/09 | TJS             | 2009.2.08 | Modified to set LineNum on manually entered Tag Properties
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to ensure Publish on Amazon flag not set if Amazon details not created
        ' 22/11/09 | TJS             | 2009.3.09 | Modified to ignore updated items when publishing new item
        ' 19/08/10 | TJS             | 2010.1.00 | Modifies to cater for Publish flag now being on InventoryAmazonDetails table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iCheckLoop As Integer, iMinLineNum As Integer, iMaxLineNum As Integer ' TJS 26/05/09
        Dim bUserEntered As Boolean, bDupLineNumFound As Boolean ' TJS 26/05/09

        Try
            ' start of code added TJS 26/05/09
            ' first check for any rows where the LineNum needs updating
            For iLoop = 0 To Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.Count - 1
                ' has row been marked for deletion ?
                If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).RowState <> DataRowState.Deleted Then
                    ' no, has row been edited or added ?
                    If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).RowState = DataRowState.Modified Or _
                        Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).RowState = DataRowState.Added Then
                        ' yes, is row a user entered one with no matching record in LerrynImportExportInventoryTagTemplate_DEV000221
                        ' i.e. no Source Code
                        bUserEntered = False ' TJS 26/05/09
                        If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).IsSourceCode_DEV000221Null Then ' TJS 26/05/09
                            bUserEntered = True ' TJS 26/05/09
                        ElseIf Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).SourceCode_DEV000221 = "" Then ' TJS 26/05/09
                            bUserEntered = True ' TJS 26/05/09
                        End If
                        If bUserEntered Then ' TJS 26/05/09
                            ' yes, check if value need updating
                            bDupLineNumFound = False
                            iMaxLineNum = 0
                            iMinLineNum = 999
                            For iCheckLoop = 0 To Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.Count - 1
                                ' ignore rows marked for deletion 
                                If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).RowState <> DataRowState.Deleted Then
                                    ' ignore row being updated
                                    If iCheckLoop <> iLoop Then
                                        ' does tag have same name and location as row being updated ?
                                        If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).TagName_DEV000221 = _
                                            Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).TagName_DEV000221 And _
                                            Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).TagLocation_DEV000221 = _
                                            Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).TagLocation_DEV000221 Then
                                            ' yes, is LineNum the highest so far ?
                                            If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221 > iMaxLineNum Then
                                                ' yes, update max value
                                                iMaxLineNum = Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221
                                            End If
                                            ' is LineNum the lowest so far ?
                                            If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221 < iMinLineNum Then
                                                ' yes, update min value
                                                iMinLineNum = Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221
                                            End If
                                            ' is LineNum same as row being checked ?
                                            If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221 = _
                                                Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 Then
                                                ' yes, 
                                                bDupLineNumFound = True
                                            End If

                                        End If
                                    End If
                                End If
                            Next
                            ' were any rows with same name and location found ?
                            If iMaxLineNum = 0 And iMinLineNum = 999 Then
                                ' no, is line number set to 1
                                If Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 <> 1 Then
                                    ' no, update it
                                    Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = 1
                                End If
                            ElseIf bDupLineNumFound Or Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = 0 Then
                                ' no, but row being updated has same value as an existing row or is still set to the new row value (0)
                                ' is there a line 1
                                If iMinLineNum > 1 Then
                                    ' no, use it
                                    Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = 1
                                Else
                                    ' line 1 used, set LineNum as next available value
                                    Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = iMaxLineNum + 1
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            ' end of code added TJS 26/05/09

            ' save any changes to Amazon settings
            If Me.m_InventorySettingsFacade.UpdateDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.TableName, _
                    "CreateInventoryAmazonDetails_DEV000221", "UpdateInventoryAmazonDetails_DEV000221", "DeleteInventoryAmazonDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
                    "CreateInventoryAmazonTagDetails_DEV000221", "UpdateInventoryAmazonTagDetails_DEV000221", "DeleteInventoryAmazonTagDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryAmazonASIN_DEV000221.TableName, _
                    "CreateInventoryAmazonASIN_DEV000221", "UpdateInventoryAmazonASIN_DEV000221", "DeleteInventoryAmazonASIN_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryAmazonSearchTerms.TableName, _
                    "CreateInventoryAmazonTagDetails_DEV000221", "UpdateInventoryAmazonTagDetails_DEV000221", "DeleteInventoryAmazonTagDetails_DEV000221"}}, _
                    Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Inventory Amazon Settings", False) Then
                Me.GridViewASIN.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
                Me.GridViewProperties.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
                Return MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache)

            Else
                Return DialogResult.Cancel
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex) ' TJS 01/12/09

        End Try

    End Function
#End Region

#Region " ExtractAmazonBrowseTreeRoot "
    Public Function ExtractAmazonBrowseTreeRoot(ByVal BrowseTreeChain As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 27/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iCharPosn As Integer, strTemp As String

        strTemp = ""
        ' get position of last chain separator
        iCharPosn = Microsoft.VisualBasic.InStr(BrowseTreeChain, ":")
        ' did we find one ?
        If iCharPosn > 0 Then
            ' yes, remove last entry from chain
            strTemp = Microsoft.VisualBasic.Left(BrowseTreeChain, iCharPosn - 1)
        End If
        ' did we find it ?
        If iCharPosn > 0 Then
            ' yes, 
            Return strTemp

        Else
            Return ""
        End If

    End Function
#End Region
#End Region

#Region " Events "
#Region " PublishOnAmazonChange "
    Private Sub PublishOnAmazonChange(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Modified to set default Product Name and Description text
        ' 03/06/09 | TJS             | 2009.2.10 | Modified to cater for 
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to only set Publish on Amazon flag if 
        '                                        | InventoryItem table already has other changes
        ' 01/12/09 | TJS             | 2009.3.09 | Modifed to set Amazon Site from Merchant config
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for Publish flag now being on InventoryAmazonDetails table
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to cater for all source config records being loaded
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryAmazonDetails_DEV000221Row
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLAmazonNode As XElement
        Dim XMLAmazonNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strTemp As String, iLoop As Integer

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.CheckEditPublishOnAmazon.EditValueChanged, AddressOf PublishOnAmazonChange
            If CBool(Me.CheckEditPublishOnAmazon.EditValue) Then
                If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.Rows.Count = 0 Then
                    rowAmazonDetails = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.NewInventoryAmazonDetails_DEV000221Row
                    rowAmazonDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                    rowAmazonDetails.Publish_DEV000221 = True
                    rowAmazonDetails.BrowseNodeID_DEV000221 = AMAZON_ROOT_CATEGORY '
                    rowAmazonDetails.SalePriceActive_DEV000221 = False
                    rowAmazonDetails.SellingPrice_DEV000221 = 0
                    rowAmazonDetails.SiteCode_DEV000221 = ""
                    rowAmazonDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 26/05/09 TJS 13/11/13
                    rowAmazonDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 26/05/09 TJS 13/11/13
                    Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.AddInventoryAmazonDetails_DEV000221Row(rowAmazonDetails)
                End If

                ' now get Amazon Merchant IDs from Config
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, AMAZON_SOURCE_CODE}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(AMAZON_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
                XMLAmazonNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
                ' check connector count is valid i.e. number of Amazon settings is not more then the licence limit
                If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLAmazonNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then

                    Coll = Me.ComboBoxEditAmazonMerchantID.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLAmazonNode In XMLAmazonNodeList
                        XMLTemp = XDocument.Parse(XMLAmazonNode.ToString)
                        strTemp = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN) ' TJS 22/03/13
                        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(strTemp)
                        Coll.Add(CollItem)
                    Next
                    Coll.EndUpdate()

                    ' if only one Merchant ID, select it
                    If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLAmazonNodeList) = 1 Then
                        Me.ComboBoxEditAmazonMerchantID.SelectedIndex = 0
                        ' write the value to the dataset
                        For iLoop = 0 To Me.ComboBoxEditAmazonMerchantID.DataBindings.Count - 1
                            Me.ComboBoxEditAmazonMerchantID.DataBindings.Item(iLoop).WriteValue()
                        Next
                        ' set site id
                        For Each XMLAmazonNode In XMLAmazonNodeList ' TJS 01/12/09
                            XMLTemp = XDocument.Parse(XMLAmazonNode.ToString) ' TJS 01/12/09
                            If Me.ComboBoxEditAmazonMerchantID.EditValue.ToString = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN) Then ' TJS 01/12/09 TJS 22/03/13
                                Me.TextEditAmazonSiteCode.EditValue = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_SITE) ' TJS 01/12/09
                                AmazonSiteChanged() ' TJS 01/12/09
                                Exit For ' TJS 01/12/09
                            End If
                        Next

                    Else
                        Me.ComboBoxEditAmazonMerchantID.SelectedIndex = -1
                    End If
                End If

                EnableDisableAmazonControls(True)
            Else
                EnableDisableAmazonControls(False)
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.CheckEditPublishOnAmazon.EditValueChanged, AddressOf PublishOnAmazonChange

        End Try

    End Sub
#End Region

#Region " AmazonMerchantIDChanged "
    Private Sub AmazonMerchantIDChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEditAmazonMerchantID.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/06/09 | TJS             | 2009.2.10 | Function added
        ' 09/11/09 | TJS             | 2009.3.09 | Modifed to set Amazon Site from Merchant config
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to use Source Code constants
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to load all source config records to remove need to 
        '                                        | reload when switching between source tabs on Inventory form
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLAmazonNode As XElement
        Dim XMLAmazonNodeList As System.Collections.Generic.IEnumerable(Of XElement)

        If Me.TextEditAmazonSiteCode.EditValue IsNot Nothing Then ' TJS 09/11/09
            If Me.TextEditAmazonSiteCode.EditValue.ToString = ".co.uk" Then
                SetProposedAmazonPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, "GBP")
            Else
                SetProposedAmazonPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, "USD")
            End If
        End If
        ' start of code added TJS 09/11/09
        ' now get Amazon Merchant IDs from Config
        Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
            "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 02/12/11
        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(AMAZON_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
        XMLAmazonNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
        ' check connector count is valid i.e. number of Amazon settings is not more then the licence limit
        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLAmazonNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
            ' set site id
            For Each XMLAmazonNode In XMLAmazonNodeList
                XMLTemp = XDocument.Parse(XMLAmazonNode.ToString)
                If Me.ComboBoxEditAmazonMerchantID.EditValue.ToString = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN) Then ' TJS 22/03/13
                    Me.TextEditAmazonSiteCode.EditValue = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_SITE)
                    AmazonSiteChanged() ' TJS 09/11/09
                    Exit For
                End If
            Next

        End If
    End Sub
#End Region

#Region " AmazonSiteChanged "
    Private Sub AmazonSiteChanged()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Modified to prevent population of ListBoxBrowseList 
        '                                        | from triggering of BrowseListItemSelected and to cater 
        '                                        | for GetAmazonBrowseListItems requiring Node ID not Node name
        ' 01/12/09 | TJS             | 2009.3.09 | Modified to use PopulateBrowseNodeList and to cater for dummy  
        '                                        | descriptive Browse Node categories with negative valaues
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        Try
            Cursor = Cursors.WaitCursor
            If Me.TextEditAmazonSiteCode.EditValue.ToString <> "" Then
                ' has an Amazon Details record been created ?
                If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221.Rows.Count > 0 Then
                    ' yes, has Browse Tree been set ?
                    If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseNodeID_DEV000221Null Then
                        ' not set, no action required
                    ElseIf CInt(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221) <> CInt(AMAZON_ROOT_CATEGORY) Then ' TJS 01/12/09
                        ' yes, reset back to root
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221 = AMAZON_ROOT_CATEGORY ' TJS 26/05/09
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SetBrowseTreeChain_DEV000221Null()
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SetBrowseTree_DEV000221Null()
                    End If

                    ' now get any updates to root browse items
                    Me.m_InventorySettingsFacade.GetAmazonBrowseListItems(AMAZON_ROOT_CATEGORY, Me.TextEditAmazonSiteCode.EditValue.ToString) ' TJS 26/05/09

                    ' now populate Browse Tree list with root categories
                    If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.Count > 0 Then
                        RemoveHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected ' TJS 26/05/09
                        PopulateBrowseNodeList() ' TJS 01/12/09
                        AddHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected ' TJS 26/05/09
                    Else
                        Interprise.Presentation.Base.Message.MessageWindow.Show("No root Browse Note Categories found - please check that your internet connection is working correctly.")

                    End If

                    ' write the value to the dataset
                    For iLoop = 0 To Me.TextEditAmazonSiteCode.DataBindings.Count - 1
                        Me.TextEditAmazonSiteCode.DataBindings.Item(iLoop).WriteValue()
                    Next

                    ' now get any updates to Tag list
                    Me.m_InventorySettingsFacade.GetAmazonTagTemplates(AMAZON_ROOT_CATEGORY, Me.TextEditAmazonSiteCode.EditValue.ToString) ' TJS 26/05/09

                    If Me.TextEditAmazonSiteCode.EditValue.ToString <> "" Then
                        If Me.TextEditAmazonSiteCode.EditValue.ToString = ".co.uk" Then
                            SetProposedAmazonPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, "GBP")
                        Else
                            SetProposedAmazonPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, "USD")
                        End If
                    End If

                    ' now enable Browse List controls
                    EnableDisableAmazonControls(True)
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default

        End Try
    End Sub
#End Region

#Region " BrowseListItemSelected "
    Private Sub BrowseListItemSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Modified to cater for GetAmazonBrowseListItems 
        '                                        | requiring Node ID not Node name
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to cater for Descriptive CAtegory label
        ' 01/12/09 | TJS             | 2009.3.09 | Modified to set XML Class etc and to cater for dummy descriptive 
        '                                        | Browse Node categories with negative valaues
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strProductXMLClass As String, strProductXMLType As String, strProductXMLSubType As String ' TJS 01/12/09
        Dim strNodeID As String, iLoop As Integer, bAtRootBrowseNode As Boolean, bBrowseNodeFound As Boolean
        Dim bDescriptiveCategory As Boolean ' TJS 27/06/09

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected
            ' check we have a non blank entry
            strNodeID = "" ' TJS 26/05/09
            strProductXMLClass = "" ' TJS 01/12/09
            strProductXMLType = "" ' TJS 01/12/09
            strProductXMLSubType = "" ' TJS 01/12/09
            If Me.ListBoxBrowseList.SelectedIndex >= 0 Then
                If Me.ListBoxBrowseList.SelectedValue.ToString <> "" Then
                    ' find matching Browse Node record
                    bBrowseNodeFound = False
                    For iLoop = 0 To Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.Count - 1
                        If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).NodeName_DEV000221 = Me.ListBoxBrowseList.SelectedValue.ToString Then
                            bBrowseNodeFound = True
                            strNodeID = Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).NodeID_DEV000221
                            bDescriptiveCategory = Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).DescriptiveCategory_DEV000221 ' TJS 27/06/09
                            If Not Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).IsProductXMLClass_DEV000221Null Then ' TJS 01/12/09
                                ' XML Class field is part of Primary key and can't be null.  IS tries to save empty strings as null so causes errors
                                ' . used as empty field indicator and should be treated as an empty string
                                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).ProductXMLClass_DEV000221 <> "." Then ' TJS 01/12/09
                                    strProductXMLClass = Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).ProductXMLClass_DEV000221 ' TJS 01/12/09
                                End If
                            End If
                            If Not Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).IsProductXMLType_DEV000221Null Then ' TJS 01/12/09
                                ' XML Type field is part of Primary key and can't be null.  IS tries to save empty strings as null so causes errors
                                ' . used as empty field indicator and should be treated as an empty string
                                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).ProductXMLType_DEV000221 <> "." Then ' TJS 01/12/09
                                    strProductXMLType = Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).ProductXMLType_DEV000221 ' TJS 01/12/09
                                End If
                            End If
                            If Not Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).IsProductXMLSubType_DEV000221Null Then ' TJS 01/12/09
                                ' XML Sub Type field is part of Primary key and can't be null.  IS tries to save empty strings as null so causes errors
                                ' . used as empty field indicator and should be treated as an empty string
                                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).ProductXMLSubType_DEV000221 <> "." Then ' TJS 01/12/09
                                    strProductXMLSubType = Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221(iLoop).ProductXMLSubType_DEV000221 ' TJS 01/12/09
                                End If
                            End If
                            Exit For
                        End If
                    Next
                    If bBrowseNodeFound Then
                        ' are we at the root level ?
                        If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseNodeID_DEV000221Null Then
                            ' yes
                            bAtRootBrowseNode = True
                        ElseIf CInt(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221) <> CInt(AMAZON_ROOT_CATEGORY) Then ' TJS 26/05/09 TJS 01/12/09
                            ' no
                            bAtRootBrowseNode = False
                        Else
                            ' yes
                            bAtRootBrowseNode = True
                        End If
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BeginEdit()
                        If bAtRootBrowseNode Then
                            ' at the root level, set Browse Node Tree
                            Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTree_DEV000221 = Me.ListBoxBrowseList.SelectedValue.ToString
                            Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221 = strNodeID

                            ' now get updates for this category's Browse Nodes 
                            Me.m_InventorySettingsFacade.GetAmazonBrowseListItems(strNodeID, TextEditAmazonSiteCode.EditValue.ToString) ' TJS 26/05/09

                            ' now get updates for this category's Tag list
                            Me.m_InventorySettingsFacade.GetAmazonTagTemplates(strNodeID, Me.TextEditAmazonSiteCode.EditValue.ToString) ' TJS 26/05/09

                            ' initialise/load Amazon Tag records
                            Me.m_InventorySettingsFacade.InitialiseInventoryAmazonTags(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                Me.TextEditAmazonSiteCode.EditValue.ToString, Me.ComboBoxEditXMLClass.EditValue.ToString, _
                                Me.ComboBoxEditXMLType.EditValue.ToString, Me.ComboBoxEditXMLSubType.EditValue.ToString, strNodeID) ' TJS 26/05/09 TJS 01/12/09

                        Else
                            ' not at the root level, add new level to Browse Node Tree
                            Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTree_DEV000221 += " > " & Me.ListBoxBrowseList.SelectedValue.ToString
                            Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221 += ":" & strNodeID

                        End If
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221 = strNodeID
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).ProductXMLClass_DEV000221 = strProductXMLClass ' TJS 01/12/09
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).EndEdit() ' TJS 01/12/09
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).ProductXMLType_DEV000221 = strProductXMLType ' TJS 01/12/09
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).EndEdit() ' TJS 01/12/09
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).ProductXMLSubType_DEV000221 = strProductXMLSubType ' TJS 01/12/09
                        Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).EndEdit()
                        Me.LabelDescriptiveCategory.Visible = bDescriptiveCategory ' TJS 27/06/09

                        ' now populate Browse Tree list with next level
                        Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                            "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, _
                            AT_PARENT_NODE, strNodeID, AT_PARENT_CATEGORY, ExtractAmazonBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221)}}, _
                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 01/12/09
                        PopulateBrowseNodeList()

                        ' now enable Browse List controls
                        EnableDisableAmazonControls(True)

                    Else

                    End If
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected

        End Try
    End Sub
#End Region

#Region " BrowseNodeUpOneLevel "
    Private Sub BrowseNodeUpOneLevel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseNodeUpLevel.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Modified to cater for GetAmazonBrowseListItems 
        '                                        | requiring Node ID not Node name and for null values
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strBrowseTreeChain As String, strBrowseTree As String, strNodeID As String
        Dim iCharPosn As Integer

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected
            If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseTreeChain_DEV000221Null Then ' TJS 26/05/09
                strBrowseTreeChain = "" ' TJS 26/05/09
            Else
                strBrowseTreeChain = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221
            End If
            If Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseTree_DEV000221Null Then ' TJS 26/05/09
                strBrowseTree = "" ' TJS 26/05/09
            Else
                strBrowseTree = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTree_DEV000221
            End If

            Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BeginEdit()
            ' get position of last chain separator
            iCharPosn = Microsoft.VisualBasic.InStrRev(strBrowseTreeChain, ":")
            ' did we find one ?
            If iCharPosn > 0 Then
                ' yes, remove last entry from chain
                strBrowseTreeChain = Microsoft.VisualBasic.Left(strBrowseTreeChain, iCharPosn - 1)
                ' and store it
                Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221 = strBrowseTreeChain
                ' get position of previous chain separator
                iCharPosn = Microsoft.VisualBasic.InStrRev(strBrowseTreeChain, ":")
                ' did we find one ?
                If iCharPosn > 0 Then
                    ' yes, Node ID is the characters after separator
                    strNodeID = Microsoft.VisualBasic.Mid(strBrowseTreeChain, iCharPosn + 1)

                Else
                    ' no, Node ID is the whole string
                    strNodeID = strBrowseTreeChain
                End If
                Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221 = strNodeID

                ' get browse tree items for this level
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                    "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, TextEditAmazonSiteCode.EditValue.ToString, _
                    AT_PARENT_NODE, strNodeID}}, Interprise.Framework.Base.Shared.ClearType.Specific)

                ' get position of last chain separator
                iCharPosn = Microsoft.VisualBasic.InStrRev(strBrowseTree, " > ")
                ' did we find one ?
                If iCharPosn > 0 Then
                    ' yes, remove last entry from chain
                    strBrowseTree = Microsoft.VisualBasic.Left(strBrowseTree, iCharPosn - 1)
                    ' and store it
                    Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTree_DEV000221 = strBrowseTree
                Else
                    ' no, reset browse tree
                    Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SetBrowseTree_DEV000221Null()
                End If

            Else
                ' no, must be at root - get any updates to root browse items
                Me.m_InventorySettingsFacade.GetAmazonBrowseListItems(AMAZON_ROOT_CATEGORY, Me.TextEditAmazonSiteCode.EditValue.ToString) ' TJS 26/05/09
                Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221 = AMAZON_ROOT_CATEGORY ' TJS 26/05/09
                Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SetBrowseTreeChain_DEV000221Null()
                Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).SetBrowseTree_DEV000221Null()
            End If

            ' now populate Browse Tree list 
            PopulateBrowseNodeList()

            ' now enable Browse List controls
            EnableDisableAmazonControls(True)

            Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).EndEdit()

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected

        End Try
    End Sub
#End Region

#Region " TagHelpButtonClicked "
    Private Sub TagHelpButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles repHelpButton.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Main code moved to DisplayPropertyHelp
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' help button clicked so display help even if form not yet open
        ' is focused row valid ?
        If Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
            ' yes, display help including opening form if necessary
            DisplayPropertyHelp()
        End If

    End Sub
#End Region

#Region " TagPropertiesSelectedRowChanged "
    Private Sub TagPropertiesSelectedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewProperties.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to select text value editor depending on tag data type
        ' 01/12/09 | TJS             | 2009.3.09 | Modified to trap invalid row selected
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bAllowEdit As Boolean

        ' are we on a valid row ?
        ' start of code added TJS 27/06/09
        If Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
            ' yes, select editor
            Select Case Me.GridViewProperties.GetFocusedRowCellValue("TagDataType_DEV000221").ToString
                Case "Text"
                    If Me.GridViewProperties.GetFocusedRowCellValue("TagLocation_DEV000221").ToString = "1" And _
                        Me.GridViewProperties.GetFocusedRowCellValue("TagName_DEV000221").ToString = "Product Tax Code" Then ' TJS 01/12/09
                        Me.colPropertiesTextValue_DEV000221.ColumnEdit = Me.rbeTaxCodeEdit ' TJS 01/12/09
                    Else
                        Me.colPropertiesTextValue_DEV000221.ColumnEdit = Me.rbeTextEdit
                    End If

                Case "Integer", "Numeric"
                    Me.colPropertiesTextValue_DEV000221.ColumnEdit = Nothing

                Case "Date", "DateTime"
                    Me.colPropertiesTextValue_DEV000221.ColumnEdit = Me.rbeDateEdit

                Case "Y/N"
                    Me.colPropertiesTextValue_DEV000221.ColumnEdit = Me.rbeYesNoEdit

                Case Else
                    Me.colPropertiesTextValue_DEV000221.ColumnEdit = Nothing

            End Select
        Else
            Me.colPropertiesDataType_DEV000221.ColumnEdit = Nothing
        End If
        ' end of code added TJS 27/06/09

        ' is Help form already open ?
        If Me.frmPropertyHelp IsNot Nothing Then
            ' yes, is focused row valid ?
            If Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
                Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
                ' update help display
                DisplayPropertyHelp()
            End If
        End If

        ' is focused row the new row ?
        If Me.GridViewProperties.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
            ' yes, allow edit
            bAllowEdit = True
        ElseIf Me.GridViewProperties.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle Then ' TJS 01/12/09
            ' no, invalid row handle
            bAllowEdit = False ' TJS 01/12/09
        ElseIf Me.GridViewProperties.GetFocusedRowCellValue("SourceTagStatus_DEV000221").ToString = "" Then
            ' no, row is a user entered one, allow edit
            bAllowEdit = True
        Else
            bAllowEdit = False
        End If

        Me.colPropertiesLocation_DEV000221.OptionsColumn.AllowEdit = bAllowEdit
        Me.colPropertiesLocation_DEV000221.OptionsColumn.ReadOnly = Not bAllowEdit
        Me.colPropertiesName_DEV000221.OptionsColumn.AllowEdit = bAllowEdit
        Me.colPropertiesName_DEV000221.OptionsColumn.ReadOnly = Not bAllowEdit
        Me.colPropertiesDataType_DEV000221.OptionsColumn.AllowEdit = bAllowEdit
        Me.colPropertiesDataType_DEV000221.OptionsColumn.ReadOnly = Not bAllowEdit

    End Sub
#End Region

#Region " DisplayPropertyHelp "
    Private Sub DisplayPropertyHelp()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Modified to bring help form to front and position in top right corner of screen
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' does tag have any help details ?
        If Me.GridViewProperties.GetFocusedRowCellValue("SourceTagDescription_DEV000221").ToString <> "" Or _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagAcceptedValues_DEV000221").ToString <> "" Or _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagExample_DEV000221").ToString <> "" Then
            ' yes, is help form alrready open >
            If Me.frmPropertyHelp Is Nothing Then
                Me.frmPropertyHelp = New PropertyHelpForm(Me.m_InventorySettingsDataset, Me.m_InventorySettingsFacade)
            End If
            Me.frmPropertyHelp.DisplayTagHelp("Amazon", Me.GridViewProperties.GetFocusedRowCellValue("TagName_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagDescription_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagAcceptedValues_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagExample_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagStatus_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagConditionality_DEV000221").ToString) ' TJS 26/05/09 TJS 26/06/09
            Me.frmPropertyHelp.Show() ' TJS 26/05/09
            ' force form to front
            Me.frmPropertyHelp.TopMost = True ' TJS 26/05/09
            ' position form in top right corner of screen but with a small offset
            Me.frmPropertyHelp.Left = Screen.PrimaryScreen.Bounds.Width - Me.frmPropertyHelp.Width - 20
            Me.frmPropertyHelp.Top = 20
            ' now let other forms come in front of help form
            Me.frmPropertyHelp.TopMost = False ' TJS 26/05/09

        ElseIf Me.GridViewProperties.GetFocusedRowCellValue("SourceTagStatus_DEV000221").ToString = "" Then
            Interprise.Presentation.Base.Message.MessageWindow.Show("User entered Property - no help details available")
            ' is Help form already open ?
            If Me.frmPropertyHelp IsNot Nothing Then ' TJS 26/05/09
                ' yes, clear help form
                Me.frmPropertyHelp.DisplayTagHelp("Amazon", "", "", "", "", "", "") ' TJS 26/05/09 TJS 26/06/09
            End If

        Else
            Interprise.Presentation.Base.Message.MessageWindow.Show("Property does not have any help details available")
            ' is Help form already open ?
            If Me.frmPropertyHelp IsNot Nothing Then ' TJS 26/05/09
                ' yes, clear help form
                Me.frmPropertyHelp.DisplayTagHelp("Amazon", "", "", "", "", "", "") ' TJS 26/05/09 TJS 26/06/09
            End If

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
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        ' 26/06/09 | TJS             | 2009.3.00 | Modified to cater for default text appearing as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION And Not MyBase.IsReadOnly Then ' TJS 13/11/13
            TextEditProductName.Text = String.Empty
            TextEditProductName.ForeColor = Color.Black ' TJS 26/06/09
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
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        ' 26/06/09 | TJS             | 2009.3.00 | Modified to make default text appear as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
            TextEditProductName.ForeColor = Color.Gray ' TJS 26/06/09
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
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        ' 26/06/09 | TJS             | 2009.3.00 | Modified to cater for default text appearing as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION And Not MyBase.IsReadOnly Then ' TJS 13/11/13
            MemoEditDescription.Text = String.Empty
            MemoEditDescription.ForeColor = Color.Black ' TJS 26/06/09
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
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        ' 26/06/09 | TJS             | 2009.3.00 | Modified to make default text appear as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
            MemoEditDescription.ForeColor = Color.Gray ' TJS 26/06/09
        End If

    End Sub
#End Region

#Region " PropertyHelpFormClosed "
    Private Sub PropertyHelpFormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles frmPropertyHelp.FormClosed
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.frmPropertyHelp = Nothing

    End Sub

#End Region

#Region " NewASINRow "
    Private Sub NewASINRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridViewASIN.InitNewRow
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        GridViewASIN.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
        Me.GridViewASIN.SetRowCellValue(Me.GridViewASIN.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonASIN_DEV000221.ItemCode_DEV000221Column.ColumnName, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode)

    End Sub
#End Region

#Region " NewTagPropertiesRow "
    Private Sub NewTagPropertiesRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridViewProperties.InitNewRow
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        With Me.GridViewProperties
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.ItemCode_DEV000221Column.ColumnName, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode)
            If .FocusedColumn.FieldName <> Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TagLocation_DEV000221Column.ColumnName Then
                .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TagLocation_DEV000221Column.ColumnName, 0)
            End If
            If .FocusedColumn.FieldName <> Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TagName_DEV000221Column.ColumnName Then
                .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TagName_DEV000221Column.ColumnName, "")
            End If
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.LineNum_DEV000221Column.ColumnName, 0)
            If .FocusedColumn.FieldName <> Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TagDataType_DEV000221Column.ColumnName Then
                .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TagDataType_DEV000221Column.ColumnName, "")
            End If
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.SourceCode_DEV000221Column.ColumnName, "")
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.SiteCode_DEV000221Column.ColumnName, Me.TextEditAmazonSiteCode.EditValue.ToString)
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.ParentCategory_DEV000221Column.ColumnName, AMAZON_ROOT_CATEGORY)
        End With

    End Sub
#End Region

#Region " NewSearchTermsRow "
    Private Sub NewSearchTermsRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridViewSearchTerms.InitNewRow
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/05/09 | TJS             | 2009.2.08 | Function added, but not used as new rows not enabled
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        With Me.GridViewSearchTerms
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonSearchTerms.ItemCode_DEV000221Column.ColumnName, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode)
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetails_DEV000221.TagLocation_DEV000221Column.ColumnName, AMAZON_SEARCH_TERM_TAG_LOCATION)
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetails_DEV000221.LineNum_DEV000221Column.ColumnName, 0)
            .SetRowCellValue(.FocusedRowHandle, Me.m_InventorySettingsDataset.InventoryAmazonTagDetails_DEV000221.TagDataType_DEV000221Column.ColumnName, "Text")
        End With

    End Sub
#End Region

#Region " ValidateTagPropertiesRow "
    Private Sub GridViewProperties_ValidateRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs) Handles GridViewProperties.ValidateRow

        Dim rowAmazonTagProperties As System.Data.DataRowView
        Dim iLoop As Integer

        rowAmazonTagProperties = DirectCast(e.Row, System.Data.DataRowView)
        For iLoop = 0 To Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.Columns.Count - 1
            Me.m_InventorySettingsFacade.Validate(rowAmazonTagProperties.Row, Me.m_InventorySettingsDataset.InventoryAmazonTagDetailTemplateView_DEV000221.Columns(iLoop).ColumnName)
        Next
        e.Valid = True
    End Sub
#End Region

#Region " DrawASINCell "
    Private Sub DrawASINCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridViewASIN.CustomDrawCell
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridControlASIN.Enabled Then
            e.Appearance.ForeColor = Color.Black
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub
#End Region

#Region " DrawASINHeader "
    Private Sub DrawASINHeader(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs) Handles GridViewASIN.CustomDrawColumnHeader
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridControlASIN.Enabled Then
            e.Appearance.ForeColor = Color.Black
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub
#End Region

#Region " DrawPropertiesCell "
    Private Sub DrawPropertiesCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridViewProperties.CustomDrawCell
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridControlProperties.Enabled Then
            e.Appearance.ForeColor = Color.Black
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub
#End Region

#Region " DrawPropertiesHeader "
    Private Sub DrawPropertiesHeader(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs) Handles GridViewProperties.CustomDrawColumnHeader
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridControlProperties.Enabled Then
            e.Appearance.ForeColor = Color.Black
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub
#End Region

#Region " DrawSearchTermsCell "
    Private Sub DrawSearchTermsCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridViewSearchTerms.CustomDrawCell
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridControlSearchTerms.Enabled Then
            e.Appearance.ForeColor = Color.Black
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub
#End Region

#Region " DrawSearchTermsHeader "
    Private Sub DrawSearchTermsHeader(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs) Handles GridViewSearchTerms.CustomDrawColumnHeader
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridControlSearchTerms.Enabled Then
            e.Appearance.ForeColor = Color.Black
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub
#End Region

#Region " XMLClassSelected "
    Private Sub XMLClassSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 01/12/09 | TJS             | 2009.3.09 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iColonPosn As Integer, strNodeID As String

        RemoveHandler Me.ComboBoxEditXMLType.EditValueChanged, AddressOf XMLTypeSelected
        PopulateXMLTypeList()
        AddHandler Me.ComboBoxEditXMLType.EditValueChanged, AddressOf XMLTypeSelected
        Me.LabelXMLType.Enabled = Me.ComboBoxEditXMLClass.Enabled
        Me.ComboBoxEditXMLType.Enabled = Me.ComboBoxEditXMLClass.Enabled

        ' has browse node been set ?
        If Not Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseTreeChain_DEV000221Null Then
            ' yes, find parent category (Not root category)
            iColonPosn = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221.IndexOf(":")
            If iColonPosn > 0 Then
                strNodeID = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221.Substring(0, iColonPosn)
            Else
                strNodeID = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221
            End If
            Me.m_InventorySettingsFacade.UpdateInventoryAmazonTags(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                Me.TextEditAmazonSiteCode.EditValue.ToString, Me.ComboBoxEditXMLClass.EditValue.ToString, _
                Me.ComboBoxEditXMLType.EditValue.ToString, Me.ComboBoxEditXMLSubType.EditValue.ToString, strNodeID)
        End If

    End Sub
#End Region

#Region " XMLTypeSelected "
    Private Sub XMLTypeSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 01/12/09 | TJS             | 2009.3.09 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iColonPosn As Integer, strNodeID As String

        RemoveHandler Me.ComboBoxEditXMLSubType.EditValueChanged, AddressOf XMLSubTypeSelected
        PopulateXMLSubTypeList()
        AddHandler Me.ComboBoxEditXMLSubType.EditValueChanged, AddressOf XMLSubTypeSelected
        Me.LabelXMLSubType.Enabled = Me.ComboBoxEditXMLClass.Enabled
        Me.ComboBoxEditXMLSubType.Enabled = Me.ComboBoxEditXMLClass.Enabled

        ' has browse node been set ?
        If Not Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseTreeChain_DEV000221Null Then
            ' yes, find parent category (Not root category)
            iColonPosn = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221.IndexOf(":")
            If iColonPosn > 0 Then
                strNodeID = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221.Substring(0, iColonPosn)
            Else
                strNodeID = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221
            End If
            Me.m_InventorySettingsFacade.UpdateInventoryAmazonTags(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                Me.TextEditAmazonSiteCode.EditValue.ToString, Me.ComboBoxEditXMLClass.EditValue.ToString, _
                Me.ComboBoxEditXMLType.EditValue.ToString, Me.ComboBoxEditXMLSubType.EditValue.ToString, strNodeID)
        End If

    End Sub
#End Region

#Region " XMLSubTypeSelected "
    Private Sub XMLSubTypeSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 01/12/09 | TJS             | 2009.3.09 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iColonPosn As Integer, strNodeID As String

        ' has browse node been set ?
        If Not Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).IsBrowseTreeChain_DEV000221Null Then
            ' yes, find parent category (Not root category)
            iColonPosn = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221.IndexOf(":")
            If iColonPosn > 0 Then
                strNodeID = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221.Substring(0, iColonPosn)
            Else
                strNodeID = Me.m_InventorySettingsDataset.InventoryAmazonDetails_DEV000221(0).BrowseTreeChain_DEV000221
            End If
            Me.m_InventorySettingsFacade.UpdateInventoryAmazonTags(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                Me.TextEditAmazonSiteCode.EditValue.ToString, Me.ComboBoxEditXMLClass.EditValue.ToString, _
                Me.ComboBoxEditXMLType.EditValue.ToString, Me.ComboBoxEditXMLSubType.EditValue.ToString, strNodeID)
        End If

    End Sub
#End Region

#Region " BeforeConfigDownload "
    Private Sub BeforeConfigDownload(ByVal sender As Object, ByVal WarningMessage As String) Handles m_InventorySettingsFacade.BeforeInitialConfigDownload
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 01/12/09 | TJS             | 2009.3.09 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Interprise.Presentation.Base.Message.MessageWindow.Show(WarningMessage, "eShopCONNECT Config Download", Interprise.Framework.Base.Shared.MessageWindowButtons.OK)

    End Sub
#End Region
#End Region

End Class
#End Region
