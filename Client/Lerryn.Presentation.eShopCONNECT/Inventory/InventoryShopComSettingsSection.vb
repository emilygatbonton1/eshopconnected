' eShopCONNECT for Connected Business
' Module: InventoryShopComSettingsSection.vb
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
' Updated 13 November 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " InventoryShopComSettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.InventoryShopComSettingsSection")> _
Public Class InventoryShopComSettingsSection

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade ' TJS 27/06/09
    Private WithEvents frmPropertyHelp As PropertyHelpForm
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.InventoryShopComSettingsSectionGateway
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
                Me.PanelPublishOnShopCom.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnShopCom.BringToFront()
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
                Me.PanelPublishOnShopCom.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnShopCom.BringToFront()
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

        Me.m_InventorySettingsFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_InventorySettingsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
    End Sub

    Public Sub New(ByVal p_InventoryShopComSettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
          ByVal p_InventoryShopComSettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)

        MyBase.New()

        Me.m_InventorySettingsDataset = p_InventoryShopComSettingsDataset
        Me.m_InventorySettingsFacade = p_InventoryShopComSettingsFacade

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
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to only set Published checkbox if Shop.com detail record exists
        ' 28/07/09 | TJS             | 2009.3.03 | Modified to populate Department list boxes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, removed 
        '                                        | check for non-existent PublishOnShopCom_DEV000221 flag on InventoryItem record 
        '                                        | and modified to load all source config records to remove need to 
        '                                        | reload when switching between source tabs on Inventory form
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLShopComNode As XElement
        Dim XMLShopComNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection ' TJS 28/07/09
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem ' TJS 28/07/09
        Dim strDepartments()() As String, strDepartment() As String, strNodeID As String ' TJS 28/07/09
        Dim iLoop As Integer, bShopComPublishDisabled As Boolean ' TJS 27/06/09 TJS 28/07/09

        ' is eShopCONNECT activated ?
        If Me.m_InventorySettingsFacade.IsActivated Then
            ' yes, is Shop.com connector activated ?
            If Me.m_InventorySettingsFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                ' yes, need to check if there is an Item record in case we are being configured in the User Role !
                If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Rows.Count > 0 Then ' TJS 02/12/11
                    ' has Item been saved yet (i.e. ItemCode is not [To be generated])
                    If Me.m_InventoryItemDataset.InventoryItem(0).ItemCode <> Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
                        ' yes, get Shop.com publishing details 
                        Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryItem.TableName, _
                                "ReadInventoryItem", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode}, _
                             New String() {Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.TableName, _
                                "ReadInventoryShopComDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode}, _
                             New String() {Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221.TableName, _
                                "ReadInventoryShopComTagDetailTemplateView_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode}, _
                             New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 05/06/09 TJS 02/12/11

                        AddHandler Me.CheckPublishOnShopCom.EditValueChanged, AddressOf PublishOnShopComChange
                        AddHandler Me.CheckEditAltImageAvailable.EditValueChanged, AddressOf AltImageAvailableClicked

                        ' get config settings
                        bShopComPublishDisabled = False
                        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(SHOP_COM_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
                        XMLShopComNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST)
                        ' check connector count is valid i.e. number of ShopCom settings is not more then the licence limit
                        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLShopComNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE) Then
                            ' check each ShopCom record 
                            For Each XMLShopComNode In XMLShopComNodeList
                                XMLTemp = XDocument.Parse(XMLShopComNode.ToString)
                                ' is publishing disabled ?
                                If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_DISABLE_PUBLISHING).ToUpper = "YES" Then
                                    ' yes
                                    bShopComPublishDisabled = True
                                    Exit For
                                End If
                            Next
                        End If

                        ' enable Shop.com details tab if publishing not disabled
                        If Not bShopComPublishDisabled Then
                            Me.PanelPublishOnShopCom.Enabled = True

                            ' display Publish On Shop.Com flag
                            If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.Count > 0 Then ' TJS 02/12/11
                                ' only set checkbox if ShomCom record exists and Publish flag set
                                If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).Publish_DEV000221 Then ' TJS 27/06/09 TJS 02/12/11
                                    Me.CheckPublishOnShopCom.Checked = True
                                Else
                                    Me.CheckPublishOnShopCom.Checked = False
                                End If
                            Else
                                Me.CheckPublishOnShopCom.Checked = False
                            End If

                            ' has Shop.com detail record been created yet ?
                            If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.Count > 0 Then ' TJS 27/06/09
                                ' yes, get updates for this category's Attribute Categories 
                                strNodeID = ExtractShopComBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221) ' TJS 27/06/09
                                If strNodeID <> "" Then ' TJS 27/06/09
                                    Me.m_InventorySettingsFacade.GetShopComAttributeCategories(strNodeID) ' TJS 27/06/09
                                Else
                                    Me.m_InventorySettingsFacade.GetShopComAttributeCategories(SHOPCOM_ROOT_ATTRIBUTE_CATEGORY) ' TJS 27/06/09
                                End If

                                ' now populate Browse Tree list with next level
                                If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).IsAttributeCategory_DEV000221Null Or strNodeID = "" Then ' TJS 27/06/09 TJS 28/07/09
                                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                                        "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, SHOPCOM_ROOT_ATTRIBUTE_CATEGORY}}, _
                                        Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 27/06/09
                                Else
                                    ' get intermediate category status
                                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                                        "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_ATTRIBUTE_CATEGORY_STRING, _
                                        ExtractShopComBrowseTreeNode(Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 27/06/09
                                    If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.Count > 0 Then ' TJS 27/06/09
                                        Me.LabelIntermediateCategory.Visible = Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221(0).IntermediateCategory_DEV000221 ' TJS 27/06/09
                                    Else
                                        Me.LabelIntermediateCategory.Visible = False ' TJS 27/06/09
                                    End If
                                    ' now get browse list
                                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                                      "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221}}, _
                                      Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 27/06/09
                                End If
                                PopulateBrowseNodeList() ' TJS 27/06/09

                                ' now enable Browse List controls etc
                                If Me.CheckPublishOnShopCom.Checked Then ' TJS 27/06/09
                                    EnableDisableShopComControls(True)
                                Else
                                    EnableDisableShopComControls(False) ' TJS 27/06/09
                                End If
                            End If
                        End If

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

        ' start of code added TJS 28/07/09
        strDepartments = Me.m_InventorySettingsFacade.GetRows(New String() {"FirstLevelDepartment_DEV000221"}, "InventoryShopComDetails_DEV000221", "FirstLevelDepartment_DEV000221 IS NOT NULL GROUP BY FirstLevelDepartment_DEV000221")
        If strDepartments IsNot Nothing Then
            Coll = Me.ComboBoxFirstLevelDepartment.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            For Each strDepartment In strDepartments
                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(strDepartment(0))
                Coll.Add(CollItem)
            Next
            Coll.EndUpdate()
        End If

        strDepartments = Me.m_InventorySettingsFacade.GetRows(New String() {"SecondLevelDepartment_DEV000221"}, "InventoryShopComDetails_DEV000221", "SecondLevelDepartment_DEV000221 IS NOT NULL GROUP BY SecondLevelDepartment_DEV000221")
        If strDepartments IsNot Nothing Then
            Coll = Me.ComboBoxSecondLevelDepartment.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            For Each strDepartment In strDepartments
                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(strDepartment(0))
                Coll.Add(CollItem)
            Next
            Coll.EndUpdate()
        End If

        strDepartments = Me.m_InventorySettingsFacade.GetRows(New String() {"ThirdLevelDepartment_DEV000221"}, "InventoryShopComDetails_DEV000221", "ThirdLevelDepartment_DEV000221 IS NOT NULL GROUP BY ThirdLevelDepartment_DEV000221")
        If strDepartments IsNot Nothing Then
            Coll = Me.ComboBoxThirdLevelDepartment.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            For Each strDepartment In strDepartments
                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(strDepartment(0))
                Coll.Add(CollItem)
            Next
            Coll.EndUpdate()
        End If
        ' end of code added TJS 28/07/09

    End Sub
#End Region

#Region " EnableDisableShopComControls "
    Private Sub EnableDisableShopComControls(ByVal Enable As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to enable Image URL field and cater for Matrix Items
        ' 28/07/09 | TJS             | 2009.3.03 | Modified to display Matrix Group Item values on Matrix Items
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bAtRootCategory As Boolean, bShopComCategorySet As Boolean, bMatrixGroupItem As Boolean, bMatrixItem As Boolean
        Dim strTemp As String, strProperties() As String, strNodeID As String ' TJS 28/07/09

        bAtRootCategory = True
        ' has a Shop.com Details record been created ?
        If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.Rows.Count > 0 Then
            ' yes, has Shop.com Department (Category) been set ?
            If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).IsAttributeCategory_DEV000221Null Then
                bShopComCategorySet = False
            ElseIf Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221 <> SHOPCOM_ROOT_ATTRIBUTE_CATEGORY And _
                Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221.Length > 2 Then
                bAtRootCategory = False
                ' is value a real Shop.com Department (Category) or just our top level indicatior ?
                If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221.Substring(0, 2).ToLower = "[v" Then
                    ' yes, enable all controls
                    bShopComCategorySet = True
                Else
                    bShopComCategorySet = False
                End If
            Else
                bShopComCategorySet = False
            End If
        Else
            bShopComCategorySet = False
        End If

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

        ' start of code added TJS 28/07/09
        ' are we displaying a Matrix Item ?
        If bMatrixItem Then
            ' yes, get ItemCode for Matrix Group Item
            strTemp = Me.m_InventorySettingsFacade.GetField("SELECT ItemCode FROM dbo.InventoryMatrixItem WHERE MatrixItemCode = '" & _
                Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'", CommandType.Text, Nothing)
            ' now get Matrix Group Item properties
            strProperties = Me.m_InventorySettingsFacade.GetRow(New String() {"GroupName_DEV000221", "GroupDescription_DEV000221", _
                "FirstLevelDepartment_DEV000221", "SecondLevelDepartment_DEV000221", "ThirdLevelDepartment_DEV000221", "CategoryTree_DEV000221", _
                "AttributeCategory_DEV000221", "ImageURL_DEV000221", "AltImageAvailable_DEV000221", "AltImageURL_DEV000221", _
                "AltImagePrompt_DEV000221"}, "InventoryShopComDetails_DEV000221", "ItemCode_DEV000221 = '" & strTemp & "'", False)
            ' remove data bindings for relevant fields
            Me.TextEditProductName.DataBindings.Clear()
            Me.MemoEditDescription.DataBindings.Clear()
            Me.TextEditShopComDepartment.DataBindings.Clear()
            Me.ComboBoxFirstLevelDepartment.DataBindings.Clear()
            Me.ComboBoxSecondLevelDepartment.DataBindings.Clear()
            Me.ComboBoxThirdLevelDepartment.DataBindings.Clear()
            Me.TextEditImageURL.DataBindings.Clear()
            Me.CheckEditAltImageAvailable.DataBindings.Clear()
            Me.MemoEditAltImagePrompt.DataBindings.Clear()
            Me.TextEditAltImageURL.DataBindings.Clear()
            ' now display Matrix Group Items properties
            If strProperties IsNot Nothing Then
                Me.TextEditProductName.EditValue = strProperties(0)
                Me.MemoEditDescription.EditValue = strProperties(1)
                Me.TextEditShopComDepartment.EditValue = strProperties(5)
                Me.ComboBoxFirstLevelDepartment.EditValue = strProperties(2)
                Me.ComboBoxSecondLevelDepartment.EditValue = strProperties(3)
                Me.ComboBoxThirdLevelDepartment.EditValue = strProperties(4)
                Me.TextEditImageURL.EditValue = strProperties(7)
                Me.CheckEditAltImageAvailable.EditValue = CBool(strProperties(8))
                Me.TextEditAltImageURL.EditValue = strProperties(9)
                Me.MemoEditAltImagePrompt.EditValue = strProperties(10)
                ' now update browse node list
                strNodeID = ExtractShopComBrowseTreeRoot(strProperties(6))
            End If

            If strNodeID <> "" Then
                Me.m_InventorySettingsFacade.GetShopComAttributeCategories(strNodeID)
            Else
                Me.m_InventorySettingsFacade.GetShopComAttributeCategories(SHOPCOM_ROOT_ATTRIBUTE_CATEGORY)
            End If

            ' now populate Browse Tree list with next level
            If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).IsAttributeCategory_DEV000221Null Then
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                    "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, SHOPCOM_ROOT_ATTRIBUTE_CATEGORY}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
            Else
                ' get intermediate category status
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                    "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_ATTRIBUTE_CATEGORY_STRING, strNodeID}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If Me.m_InventorySettingsDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.Count > 0 Then
                    Me.LabelIntermediateCategory.Visible = Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221(0).IntermediateCategory_DEV000221
                Else
                    Me.LabelIntermediateCategory.Visible = False
                End If
                ' now get browse list
                Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                  "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221}}, _
                  Interprise.Framework.Base.Shared.ClearType.Specific)
            End If
            PopulateBrowseNodeList()

        End If
        ' end of code added TJS 28/07/09

        ' these controls are enabled to allow selection of Shop.com Catalog ID and Department
        Me.LabelShopComCatalogID.Enabled = Enable
        Me.ComboBoxEditShopComCatalogID.Enabled = Enable
        Me.LabelShopComDepartment.Enabled = Enable And Not bMatrixItem
        ' does list contain any items ?
        If Me.ListBoxBrowseList.ItemCount > 0 Then
            ' yes, use 
            Me.ListBoxBrowseList.Enabled = Enable And Not bMatrixItem
        Else
            Me.ListBoxBrowseList.Enabled = False
        End If
        Me.TextEditShopComDepartment.Enabled = Enable And Not bMatrixItem

        Me.btnBrowseNodeUpLevel.Enabled = Not bAtRootCategory And Enable And Not bMatrixItem

        ' display labels for Matrix Items and Matrix Group Items
        Me.LabelMatrixGroupItem.Visible = bMatrixGroupItem And Enable ' TJS 27/06/09
        Me.LabelMatrixItem.Visible = bMatrixItem And Enable ' TJS 27/06/09

        ' these controls are enabled if Shop.com Department (Category) is set and Item is not a Matrix Item 
        Me.LabelFirstLevelDepartment.Enabled = bShopComCategorySet And Enable And Not bMatrixItem
        Me.LabelSecondLevelDepartment.Enabled = bShopComCategorySet And Enable And Not bMatrixItem
        Me.LabelThirdLevelDepartment.Enabled = bShopComCategorySet And Enable And Not bMatrixItem
        Me.LabelProductName.Enabled = bShopComCategorySet And Enable And Not bMatrixItem ' TJS 27/06/09
        Me.LabelDescription.Enabled = bShopComCategorySet And Enable And Not bMatrixItem ' TJS 27/06/09
        Me.LabelImageURL.Enabled = bShopComCategorySet And Enable And Not bMatrixItem ' TJS 27/06/09
        Me.ComboBoxFirstLevelDepartment.Enabled = bShopComCategorySet And Enable And Not bMatrixItem
        Me.ComboBoxSecondLevelDepartment.Enabled = bShopComCategorySet And Enable And Not bMatrixItem
        Me.ComboBoxThirdLevelDepartment.Enabled = bShopComCategorySet And Enable And Not bMatrixItem
        Me.MemoEditDescription.Enabled = bShopComCategorySet And Enable And Not bMatrixItem ' TJS 27/06/09
        Me.TextEditProductName.Enabled = bShopComCategorySet And Enable And Not bMatrixItem ' TJS 27/06/09
        Me.TextEditImageURL.Enabled = bShopComCategorySet And Enable And Not bMatrixItem ' TJS 27/06/09
        Me.CheckEditAltImageAvailable.Enabled = bShopComCategorySet And Enable And Not bMatrixItem ' TJS 27/06/09

        ' these controls are enabled if Shop.com Department (Category) is set
        Me.LabelKeywords.Enabled = (bShopComCategorySet Or bMatrixItem) And Enable ' TJS 27/06/09
        Me.LabelProperties.Enabled = (bShopComCategorySet Or bMatrixItem) And Enable ' TJS 27/06/09
        Me.LabelShopComSellingPrice.Enabled = (bShopComCategorySet Or bMatrixItem) And Enable ' TJS 27/06/09
        Me.TextEditKeywords.Enabled = (bShopComCategorySet Or bMatrixItem) And Enable ' TJS 27/06/09
        Me.TextEditShopComSellingPrice.Enabled = (bShopComCategorySet Or bMatrixItem) And Enable ' TJS 27/06/09
        Me.GridControlProperties.Enabled = (bShopComCategorySet Or bMatrixItem) And Enable ' TJS 27/06/09

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

        If Enable And bShopComCategorySet And Not bMatrixItem Then ' TJS 27/06/09
            EnableDisableAltImageControls(CBool(Me.CheckEditAltImageAvailable.EditValue))
        Else
            EnableDisableAltImageControls(False)
        End If
    End Sub
#End Region

#Region " EnableDisableAltImageControls "
    Private Sub EnableDisableAltImageControls(ByVal Enable As Boolean)

        Me.LabelAltImagePrompt.Enabled = Enable
        Me.LabelAltImageURL.Enabled = Enable
        Me.MemoEditAltImagePrompt.Enabled = Enable
        Me.TextEditAltImageURL.Enabled = Enable

    End Sub
#End Region

#Region " SetProposedShopComPrice "
    Private Sub SetProposedShopComPrice(ByVal ItemCode As String)
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

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLShopComNode As XElement
        Dim XMLShopComNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strSQL As String, strTemp As String, bUpdatePrice As Boolean

        ' get config settings
        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(SHOP_COM_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
        XMLShopComNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST)
        ' check connector count is valid i.e. number of ShopCom settings is not more then the licence limit
        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLShopComNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE) Then
            ' check each ShopCom record for current Catalog ID
            For Each XMLShopComNode In XMLShopComNodeList
                XMLTemp = XDocument.Parse(XMLShopComNode.ToString)
                ' have we found current Catalog ID ?
                If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_ID) = Me.ComboBoxEditShopComCatalogID.Text Then
                    ' yes, has ShopCom price been set ?
                    bUpdatePrice = False
                    If Me.TextEditShopComSellingPrice.Text <> "" Then
                        If CDec(Me.TextEditShopComSellingPrice.EditValue) = 0 Then
                            bUpdatePrice = True
                        End If
                    Else
                        bUpdatePrice = True
                    End If
                    If bUpdatePrice Then
                        ' get retail selling price
                        strSQL = "SELECT RetailPrice FROM dbo.InventoryItemPricingDetailView WHERE CurrencyCode = '" & Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CURRENCY) & "' AND ItemCode = '" & ItemCode & "'"
                        strTemp = Me.m_InventorySettingsFacade.GetField(strSQL, CommandType.Text, Nothing)
                        ' did we find a price ?
                        If strTemp <> "" Then
                            ' yes, use it
                            Me.TextEditShopComSellingPrice.EditValue = CDec(strTemp)
                            ' now get default uplift percent
                            strTemp = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_DEFAULT_PRICE_UPLIFT)
                            ' is it set ?
                            If strTemp <> "" Then
                                ' uplift price
                                Me.TextEditShopComSellingPrice.EditValue = CDec(Me.TextEditShopComSellingPrice.EditValue) * ((100 + CDec(strTemp)) / 100)
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

        RemoveHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected
        Me.ListBoxBrowseList.Items.BeginUpdate()
        Me.ListBoxBrowseList.Items.Clear()
        For iLoop = 0 To Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.Count - 1
            Me.ListBoxBrowseList.Items.Add(Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221(iLoop).CategoryName_DEV000221)
        Next
        Me.ListBoxBrowseList.Items.EndUpdate()
        ' make sure no list item selected
        Me.ListBoxBrowseList.SelectedIndex = -1
        AddHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected

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
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to ensure Publish on Shop.com flag not set if Shop.com details not created
        '                                        | and added checks for matrix items
        ' 28/07/09 | TJS             | 2009.3.03 | Corrected check for published Matrix Group Item having a Shop.com details record
        ' 19/08/10 | TJS             | 2010.1.00 | Modifies to cater for Publish flag now being on InventoryShopComDetails table
        ' 02/12/11 | TJs             | 2011.2.00 | Modified to prevent error if no rows in dataset
        ' 29/01/13 | TJs             | 2013.0.00 | Modified to prevent error if m_InventoryItemDataset not set
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iCheckLoop As Integer, iMinLineNum As Integer, iMaxLineNum As Integer ' TJS 26/05/09
        Dim bUserEntered As Boolean, bDupLineNumFound As Boolean, bMatrixGroupPublished As Boolean ' TJS 26/05/09 TJS 27/06/09
        Dim strTemp As String, strGroupItemCode As String ' TJS 27/06/09 TJS 28/07/09

        ' start of code added TJS 26/05/09
        ' first check for any rows where the LineNum needs updating
        For iLoop = 0 To Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221.Count - 1
            ' has row been marked for deletion ?
            If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).RowState <> DataRowState.Deleted Then
                ' no, has row been edited or added ?
                If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).RowState = DataRowState.Modified Or _
                    Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).RowState = DataRowState.Added Then
                    ' yes, is row a user entered one with no matching record in LerrynImportExportInventoryTagTemplate_DEV000221
                    ' i.e. no Source Code
                    bUserEntered = False ' TJS 26/05/09
                    If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).IsSourceCode_DEV000221Null Then ' TJS 26/05/09
                        bUserEntered = True ' TJS 26/05/09
                    ElseIf Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).SourceCode_DEV000221 = "" Then ' TJS 26/05/09
                        bUserEntered = True ' TJS 26/05/09
                    End If
                    If bUserEntered Then ' TJS 26/05/09
                        ' yes, check if value need updating
                        bDupLineNumFound = False
                        iMaxLineNum = 0
                        iMinLineNum = 999
                        For iCheckLoop = 0 To Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221.Count - 1
                            ' ignore rows marked for deletion 
                            If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).RowState <> DataRowState.Deleted Then
                                ' ignore row being updated
                                If iCheckLoop <> iLoop Then
                                    ' does tag have same name and location as row being updated ?
                                    If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).TagName_DEV000221 = _
                                        Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagName_DEV000221 And _
                                        Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).TagLocation_DEV000221 = _
                                        Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagLocation_DEV000221 Then
                                        ' yes, is LineNum the highest so far ?
                                        If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221 > iMaxLineNum Then
                                            ' yes, update max value
                                            iMaxLineNum = Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221
                                        End If
                                        ' is LineNum the lowest so far ?
                                        If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221 < iMinLineNum Then
                                            ' yes, update min value
                                            iMinLineNum = Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221
                                        End If
                                        ' is LineNum same as row being checked ?
                                        If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iCheckLoop).LineNum_DEV000221 = _
                                            Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 Then
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
                            If Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 <> 1 Then
                                ' no, update it
                                Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = 1
                            End If
                        ElseIf bDupLineNumFound Or Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = 0 Then
                            ' no, but row being updated has same value as an existing row or is still set to the new row value (0)
                            ' is there a line 1
                            If iMinLineNum > 1 Then
                                ' no, use it
                                Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = 1
                            Else
                                ' line 1 used, set LineNum as next available value
                                Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).LineNum_DEV000221 = iMaxLineNum + 1
                            End If
                        End If
                    End If
                End If
            End If
        Next
        ' end of code added TJS 26/05/09

        ' save any changes to Shop.com settings
        If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Count > 0 AndAlso Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.Count > 0 Then ' TJS 02/12/11 TJS 29/01/13
            If Me.m_InventorySettingsFacade.UpdateDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.TableName, _
                "CreateInventoryShopComDetails_DEV000221", "UpdateInventoryShopComDetails_DEV000221", "DeleteInventoryShopComDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryShopComTagDetailTemplateView_DEV000221.TableName, _
                "CreateInventoryShopComTagDetails_DEV000221", "UpdateInventoryShopComTagDetails_DEV000221", "DeleteInventoryShopComTagDetails_DEV000221"}}, _
                Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Inventory ShopCom Settings", False) Then

                ' is Item a Matrix Item ?
                If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Item" And Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).Publish_DEV000221 Then ' TJS 19/08/10
                    ' yes, has the matrix group item been set for publishing ?
                    ' get Matrix Group Item code
                    strGroupItemCode = Me.m_InventorySettingsFacade.GetField("SELECT ItemCode FROM dbo.InventoryMatrixItem WHERE MatrixItemCode = '" & Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).ItemCode_DEV000221 & "'", CommandType.Text, Nothing) ' TJS 28/07/09
                    ' now get PublishOnShopCom flag for Matrix Group Item
                    strTemp = Me.m_InventorySettingsFacade.GetField("SELECT Publish_DEV000221 FROM dbo.InventoryShopComDetails_DEV000221 WHERE ItemCode = '" & strGroupItemCode & "' AND CatalogID_DEV000221 = '" & Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).CatalogID_DEV000221 & "'", CommandType.Text, Nothing) ' TJS 28/07/09 TJS 19/08/10
                    bMatrixGroupPublished = False
                    ' is Publish On Shop.Com flag set for Matrix Group Item ?
                    If strTemp <> "" Then
                        If CBool(strTemp) Then
                            ' yes
                            bMatrixGroupPublished = True
                        End If
                    End If
                    If Not bMatrixGroupPublished Then
                        Interprise.Presentation.Base.Message.MessageWindow.Show("This Item is part of a Matrix Group and, as such, settings from the Matrix Group Item" & _
                            vbCrLf & "are required in order to publish this Item on Shop.com." & vbCrLf & vbCrLf & "You must set the Publish on Shop.com checkbox, and complete the mandatory fields," & _
                            vbCrLf & "on the eShopCONNECT/Shop.com tab of the Matrix Group Item before any items" & vbCrLf & "can be published to Shop.com", "eShopCONNECT publishing to Shop.com", Interprise.Framework.Base.Shared.MessageWindowButtons.Close)
                    End If
                End If
                ' end of code added TJS 27/06/09
                Me.GridViewProperties.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
                Return MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache)

            Else
                Return DialogResult.Cancel
            End If
        Else
            Return MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache) ' TJS 02/12/11
        End If

    End Function
#End Region

#Region " ExtractShopComBrowseTreeRoot "
    Public Function ExtractShopComBrowseTreeRoot(ByVal BrowseTreeChain As String) As String
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

        Dim iCharPosn As Integer, strTemp As String

        strTemp = ""
        ' get position of last chain separator
        iCharPosn = Microsoft.VisualBasic.InStrRev(BrowseTreeChain, "/(")
        ' did we find one ?
        If iCharPosn > 0 Then
            ' yes, remove last entry from chain
            strTemp = Microsoft.VisualBasic.Left(BrowseTreeChain, iCharPosn - 1)
            ' now extract root category - first find closing bracket
            iCharPosn = Microsoft.VisualBasic.InStrRev(strTemp, ")")
            ' did we find it ?
            If iCharPosn > 0 Then
                ' yes, remove last entry from chain
                strTemp = Microsoft.VisualBasic.Left(strTemp, iCharPosn - 1)
                ' now find opening bracket
                iCharPosn = Microsoft.VisualBasic.InStrRev(strTemp, "(")
                ' did we find it ?
                If iCharPosn > 0 Then
                    ' yes, extract category
                    strTemp = Microsoft.VisualBasic.Mid(strTemp, iCharPosn + 1)
                End If
            End If
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

#Region " ExtractShopComBrowseTreeNode "
    Public Function ExtractShopComBrowseTreeNode(ByVal BrowseTreeChain As String) As String
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
        iCharPosn = Microsoft.VisualBasic.InStrRev(BrowseTreeChain, "/(")
        ' did we find one ?
        If iCharPosn > 0 Then
            ' yes, remove last entry from chain
            strTemp = Microsoft.VisualBasic.Right(BrowseTreeChain, iCharPosn + 1)
            ' now extract root category - first find closing bracket
            iCharPosn = Microsoft.VisualBasic.InStrRev(strTemp, ")")
            ' did we find it ?
            If iCharPosn > 0 Then
                ' yes, remove last entry from chain
                strTemp = Microsoft.VisualBasic.Left(strTemp, iCharPosn - 1)
                ' now find opening bracket
                iCharPosn = Microsoft.VisualBasic.InStrRev(strTemp, "(")
                ' did we find it ?
                If iCharPosn > 0 Then
                    ' yes, extract category
                    strTemp = Microsoft.VisualBasic.Mid(strTemp, iCharPosn + 1)
                End If
            End If
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
#Region " PublishOnShopComChange "
    Private Sub PublishOnShopComChange(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/06/09 | TJS             | 2009.2.10 | Code completed
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to only set Publish on Shop.com flag if 
        '                                        | InventoryItem table already has other changes
        ' 28/07/09 | TJS             | 2009.3.03 | Modified to prevent all browse tree categories being 
        '                                        | displayed if Attribute Category not set properly
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for Publish flag now being on InventoryShopComDetails table
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to cater for all source config records being loaded
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowShopComDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryShopComDetails_DEV000221Row
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLShopComNode As XElement
        Dim XMLShopComNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strTemp As String, strAttributeCategory As String, iLoop As Integer ' TJS 18/07/09

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.CheckPublishOnShopCom.EditValueChanged, AddressOf PublishOnShopComChange
            If CBool(Me.CheckPublishOnShopCom.EditValue) Then
                If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.Rows.Count = 0 Then
                    rowShopComDetails = Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.NewInventoryShopComDetails_DEV000221Row
                    rowShopComDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                    rowShopComDetails.Publish_DEV000221 = True ' TJS 19/08/10
                    rowShopComDetails.AttributeCategory_DEV000221 = SHOPCOM_ROOT_ATTRIBUTE_CATEGORY
                    rowShopComDetails.SellingPrice_DEV000221 = 0
                    rowShopComDetails.GroupName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
                    rowShopComDetails.GroupDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
                    Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.AddInventoryShopComDetails_DEV000221Row(rowShopComDetails)
                End If

                ' now get Shop.com Catalog IDs from Config
                XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(SHOP_COM_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
                XMLShopComNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST)
                ' check connector count is valid i.e. number of Shop.com settings is not more then the licence limit
                If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLShopComNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE) Then

                    Coll = Me.ComboBoxEditShopComCatalogID.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLShopComNode In XMLShopComNodeList
                        XMLTemp = XDocument.Parse(XMLShopComNode.ToString)
                        strTemp = Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_ID)
                        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(strTemp)
                        Coll.Add(CollItem)
                    Next
                    Coll.EndUpdate()

                    ' if only one Catalog ID, select it
                    If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLShopComNodeList) = 1 Then
                        Me.ComboBoxEditShopComCatalogID.SelectedIndex = 0
                        ' write the value to the dataset
                        For iLoop = 0 To Me.ComboBoxEditShopComCatalogID.DataBindings.Count - 1
                            Me.ComboBoxEditShopComCatalogID.DataBindings.Item(iLoop).WriteValue()
                        Next
                    Else
                        Me.ComboBoxEditShopComCatalogID.SelectedIndex = -1
                    End If
                End If

                ' has a Shop.com details record been created >
                If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.Rows.Count = 0 Then
                    ' no, get any updates to root browse items
                    Me.m_InventorySettingsFacade.GetShopComAttributeCategories(SHOPCOM_ROOT_ATTRIBUTE_CATEGORY)
                Else
                    ' yes, get updates for this category's Attribute Categories 
                    strTemp = ExtractShopComBrowseTreeRoot(Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221) ' TJS 27/06/09
                    If strTemp <> "" Then ' TJS 27/06/09
                        Me.m_InventorySettingsFacade.GetShopComAttributeCategories(strTemp) ' TJS 27/06/09
                    Else
                        Me.m_InventorySettingsFacade.GetShopComAttributeCategories(SHOPCOM_ROOT_ATTRIBUTE_CATEGORY) ' TJS 27/06/09
                    End If

                    ' now populate Browse Tree list with next level
                    If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).IsAttributeCategory_DEV000221Null Or strTemp = "" Then ' TJS 27/06/09 TJS 28/07/09
                        Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                            "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, SHOPCOM_ROOT_ATTRIBUTE_CATEGORY}}, _
                            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 27/06/09
                    Else
                        Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                          "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, strTemp}}, _
                          Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 27/06/09
                    End If
                End If

                ' now populate Department (category) list with root categories
                If Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.Count > 0 Then
                    PopulateBrowseNodeList()
                ElseIf Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221.Rows.Count = 0 Then
                    Interprise.Presentation.Base.Message.MessageWindow.Show("No root Department (Categories) found - please check that your internet connection is working correctly.")

                End If

                ' now get any updates to Tag list
                Me.m_InventorySettingsFacade.GetShopComTagTemplates(SHOPCOM_ROOT_CATEGORY)

                ' is item part of a matrix group ?
                If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Item" Then ' TJS 18/07/09
                    ' yes, get ItemCode for Matrix Group Item
                    strTemp = Me.m_InventorySettingsFacade.GetField("SELECT ItemCode FROM dbo.InventoryMatrixItem WHERE MatrixItemCode = '" & _
                        Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'", CommandType.Text, Nothing) ' TJS 18/07/09

                    strAttributeCategory = Me.m_InventorySettingsFacade.GetField("SELECT AttributeCategory_DEV000221 FROM " & _
                        "dbo.InventoryShopComDetails_DEV000221 WHERE ItemCode_DEV000221 = '" & strTemp & "'", CommandType.Text, Nothing) ' TJS 18/07/09
                    If strAttributeCategory.Substring(0, 2).ToLower = "[v" Then ' TJS 18/07/09
                        strAttributeCategory = Me.ExtractShopComBrowseTreeNode(strAttributeCategory) ' TJS 18/07/09
                    End If
                    ' initialise/load Shop.com Tag records
                    Me.m_InventorySettingsFacade.InitialiseInventoryShopComTags(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, strAttributeCategory) ' TJS 18/07/09
                End If

                EnableDisableShopComControls(True)
            Else
                EnableDisableShopComControls(False)
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.CheckPublishOnShopCom.EditValueChanged, AddressOf PublishOnShopComChange

        End Try

    End Sub
#End Region

#Region " ShopComCatalogIDChanged "
    Private Sub ShopComCatalogIDChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxEditShopComCatalogID.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/06/09 | TJS             | 2009.2.10 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        SetProposedShopComPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode)

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
        ' 04/06/09 | TJS             | 2009.2.10 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strAttributeCategory As String, iLoop As Integer, bAtRootBrowseNode As Boolean, bBrowseNodeFound As Boolean

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected
            ' check we have a non blank entry
            If Me.ListBoxBrowseList.SelectedIndex >= 0 Then
                If Me.ListBoxBrowseList.SelectedValue.ToString <> "" Then
                    ' find matching Browse Node record
                    bBrowseNodeFound = False
                    For iLoop = 0 To Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.Count - 1
                        If Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221(iLoop).CategoryName_DEV000221 = Me.ListBoxBrowseList.SelectedValue.ToString Then
                            bBrowseNodeFound = True
                            strAttributeCategory = Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221(iLoop).AttributeCategoryString_DEV000221
                            Exit For
                        End If
                    Next
                    If bBrowseNodeFound Then
                        ' are we at the root level ?
                        If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).IsAttributeCategory_DEV000221Null Then
                            ' yes
                            bAtRootBrowseNode = True
                        ElseIf Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221 = SHOPCOM_ROOT_ATTRIBUTE_CATEGORY Then
                            ' no
                            bAtRootBrowseNode = True
                        Else
                            ' yes
                            bAtRootBrowseNode = False
                        End If
                        Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).BeginEdit()
                        If bAtRootBrowseNode Then
                            ' at the root level, set Browse Node Tree
                            Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).CategoryTree_DEV000221 = Me.ListBoxBrowseList.SelectedValue.ToString

                            ' now get updates for this category's Attribute Categories 
                            Me.m_InventorySettingsFacade.GetShopComAttributeCategories(Me.ListBoxBrowseList.SelectedValue.ToString)

                            ' now get updates for this category's Tag list
                            Me.m_InventorySettingsFacade.GetShopComTagTemplates(strAttributeCategory)

                            ' initialise/load Shop.com Tag records
                            Me.m_InventorySettingsFacade.InitialiseInventoryShopComTags(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, strAttributeCategory)

                        Else
                            ' not at the root level, add new level to Browse Node Tree
                            Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).CategoryTree_DEV000221 += " > " & Me.ListBoxBrowseList.SelectedValue.ToString

                        End If
                        Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221 = strAttributeCategory
                        Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).EndEdit()

                        ' now populate Browse Tree list with next level
                        Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                            "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, Me.ListBoxBrowseList.SelectedValue.ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                        PopulateBrowseNodeList()

                        ' now enable Browse List controls
                        EnableDisableShopComControls(True)
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
        ' 04/06/09 | TJS             | 2009.2.10 | Function added
        ' 27/06/09 | TJS             | 2009.3.00 | Moved code to extract attribute node to ExtractShopComAttributeRoot
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strBrowseTreeChain As String, strBrowseTree As String, strNodeID As String
        Dim iCharPosn As Integer

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected
            If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).IsAttributeCategory_DEV000221Null Then
                strBrowseTreeChain = ""
            Else
                strBrowseTreeChain = Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221
            End If
            If Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).IsCategoryTree_DEV000221Null Then
                strBrowseTree = ""
            Else
                strBrowseTree = Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).CategoryTree_DEV000221
            End If

            Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).BeginEdit()
            strNodeID = ExtractShopComBrowseTreeRoot(strBrowseTreeChain) ' TJS 27/06/09

            ' did we find one ?
            If strNodeID <> "" Then
                ' yes, store it
                Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221 = strBrowseTreeChain

                ' get position of last chain separator
                iCharPosn = Microsoft.VisualBasic.InStrRev(strBrowseTree, " > ")
                ' did we find one ?
                If iCharPosn > 0 Then
                    ' yes, remove last entry from chain
                    strBrowseTree = Microsoft.VisualBasic.Left(strBrowseTree, iCharPosn - 1)
                    ' and store it
                    Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).CategoryTree_DEV000221 = strBrowseTree
                    ' get browse tree items for this level
                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                        "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, strBrowseTree}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                Else
                    ' no, reset browse tree
                    Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).SetCategoryTree_DEV000221Null()
                    ' get browse tree items for this level
                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                        "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, SHOPCOM_ROOT_ATTRIBUTE_CATEGORY}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                End If

            Else
                ' no, must be at root - get any updates to root browse items
                Me.m_InventorySettingsFacade.GetShopComAttributeCategories(SHOPCOM_ROOT_ATTRIBUTE_CATEGORY)
                Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221 = SHOPCOM_ROOT_ATTRIBUTE_CATEGORY
                Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).SetCategoryTree_DEV000221Null()
            End If

            ' now populate Browse Tree list 
            PopulateBrowseNodeList()

            ' now enable Browse List controls
            EnableDisableShopComControls(True)

            Me.m_InventorySettingsDataset.InventoryShopComDetails_DEV000221(0).EndEdit()

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.ListBoxBrowseList.SelectedIndexChanged, AddressOf BrowseListItemSelected

        End Try
    End Sub
#End Region

#Region " AltImageAvailableClicked "
    Private Sub AltImageAvailableClicked(ByVal sender As System.Object, ByVal e As System.EventArgs)

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
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
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
        ' 25/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bAllowEdit As Boolean

        ' are we on a valid row ?
        If Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            Me.GridViewProperties.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
            ' yes, select editor
            Select Case Me.GridViewProperties.GetFocusedRowCellValue("TagDataType_DEV000221").ToString
                Case "Text"
                    Me.colTagTextValue_DEV0002211.ColumnEdit = Me.rbeTextEdit

                Case "Integer", "Numeric"
                    Me.colTagTextValue_DEV0002211.ColumnEdit = Nothing

                Case "Date", "DateTime"
                    Me.colTagTextValue_DEV0002211.ColumnEdit = Me.rbeDateEdit

                Case "Y/N"
                    Me.colTagTextValue_DEV0002211.ColumnEdit = Me.rbeYesNoEdit

                Case Else
                    Me.colTagTextValue_DEV0002211.ColumnEdit = Nothing

            End Select
        Else
            Me.colTagTextValue_DEV0002211.ColumnEdit = Nothing
        End If

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
        ElseIf Me.GridViewProperties.GetFocusedRowCellValue("SourceTagStatus_DEV000221").ToString = "" Then
            ' no, row is a user entered one, allow edit
            bAllowEdit = True
        Else
            bAllowEdit = False
        End If

        Me.colTagLocation_DEV0002211.OptionsColumn.AllowEdit = bAllowEdit
        Me.colTagLocation_DEV0002211.OptionsColumn.ReadOnly = Not bAllowEdit
        Me.colTagName_DEV0002211.OptionsColumn.AllowEdit = bAllowEdit
        Me.colTagName_DEV0002211.OptionsColumn.ReadOnly = Not bAllowEdit
        Me.colTagDataType_DEV0002211.OptionsColumn.AllowEdit = bAllowEdit
        Me.colTagDataType_DEV0002211.OptionsColumn.ReadOnly = Not bAllowEdit

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
        ' 27/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' does tag have any help details ?
        If Me.GridViewProperties.GetFocusedRowCellValue("SourceTagDescription_DEV000221").ToString <> "" Or _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagAcceptedValues_DEV000221").ToString <> "" Or _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagExample_DEV000221").ToString <> "" Then
            ' yes, is help form alrready open >
            If Me.frmPropertyHelp Is Nothing Then
                Me.frmPropertyHelp = New PropertyHelpForm(Me.m_InventorySettingsDataset, Me.m_InventorySettingsFacade)
            End If
            Me.frmPropertyHelp.DisplayTagHelp("Shop.com", Me.GridViewProperties.GetFocusedRowCellValue("TagName_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagDescription_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagAcceptedValues_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagExample_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagStatus_DEV000221").ToString, _
                Me.GridViewProperties.GetFocusedRowCellValue("SourceTagConditionality_DEV000221").ToString)
            Me.frmPropertyHelp.Show()
            ' force form to front
            Me.frmPropertyHelp.TopMost = True
            ' position form in top right corner of screen but with a small offset
            Me.frmPropertyHelp.Left = Screen.PrimaryScreen.Bounds.Width - Me.frmPropertyHelp.Width - 20
            Me.frmPropertyHelp.Top = 20
            ' now let other forms come in front of help form
            Me.frmPropertyHelp.TopMost = False

        ElseIf Me.GridViewProperties.GetFocusedRowCellValue("SourceTagStatus_DEV000221").ToString = "" Then
            Interprise.Presentation.Base.Message.MessageWindow.Show("User entered Property - no help details available")
            ' is Help form already open ?
            If Me.frmPropertyHelp IsNot Nothing Then
                ' yes, clear help form
                Me.frmPropertyHelp.DisplayTagHelp("Shop.com", "", "", "", "", "", "")
            End If

        Else
            Interprise.Presentation.Base.Message.MessageWindow.Show("Property does not have any help details available")
            ' is Help form already open ?
            If Me.frmPropertyHelp IsNot Nothing Then
                ' yes, clear help form
                Me.frmPropertyHelp.DisplayTagHelp("Shop.com", "", "", "", "", "", "")
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
        ' 13/06/09 | TJS             | 2009.2.13 | Function added
        ' 15/06/09 | TJS             | 2009.3.00 | Modified to cater for default text appearing as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION And Not MyBase.IsReadOnly Then ' TJS 13/11/13
            TextEditProductName.Text = String.Empty
            TextEditProductName.ForeColor = Color.Black ' TJS 15/06/09
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
        ' 13/06/09 | TJS             | 2009.2.13 | Function added
        ' 15/06/09 | TJS             | 2009.3.00 | Modified to make default text appear as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
            TextEditProductName.ForeColor = Color.Gray ' TJS 15/06/09
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
        ' 13/06/09 | TJS             | 2009.2.13 | Function added
        ' 15/06/09 | TJS             | 2009.3.00 | Modified to cater for default text appearing as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION And Not MyBase.IsReadOnly Then ' TJS 13/11/13
            MemoEditDescription.Text = String.Empty
            MemoEditDescription.ForeColor = Color.Black ' TJS 15/06/09
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
        ' 13/06/09 | TJS             | 2009.2.13 | Function added
        ' 15/06/09 | TJS             | 2009.3.00 | Modified to make default text appear as grey
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
            MemoEditDescription.ForeColor = Color.Gray ' TJS 15/06/09
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
        ' 24/06/09 | TJS             | 2009.3.00 | Function added
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
        ' 24/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.GridControlProperties.Enabled Then
            e.Appearance.ForeColor = Color.Black
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub
#End Region
#End Region
End Class
#End Region
