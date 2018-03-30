' eShopCONNECT for Connected Business
' Module: InventoryMagentoSettingsSection.vb
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
' Updated 09 December 2013

Option Explicit On
Option Strict On

Imports Interprise.Framework.Base.Shared.Const ' 24/02/12
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " InventoryMagentoSettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.InventoryMagentoSettingsSection")> _
Public Class InventoryMagentoSettingsSection

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private WithEvents m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
    Private m_MagentoImportFacade As Lerryn.Facade.ImportExport.MagentoImportFacade ' TJS 09/04/11
    Private m_CategoryTable As System.Data.DataTable = Nothing
    Private m_TempDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway 'TJS/FA 29/05/13
    Private m_tempFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade 'TJS/FA 29/05/13

    Private WithEvents m_BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing
    Private bGetMagentoCategories As Boolean = False ' TJS 25/04/11
    Private bGetMagentoAttributeSets As Boolean = False ' TJS 25/04/11
    Private bGetMagentoAttributesForSet As Boolean = False ' TJS 25/04/11
    Private GetAttributesForSetID As Integer = -1 ' TJS 25/04/11
    Private bCopyInstanceSettings As Boolean 'TJS/FA 29/05/13
    Private m_WeightUnits As String = "" ' TJS 09/12/13
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.InventoryMagentoSettingsSectionGateway
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
                Me.PanelPublishOnMagento.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnMagento.BringToFront()
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
                Me.PanelPublishOnMagento.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnMagento.BringToFront()
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
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to create Magento Import Facade for backgrounf worker use
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to load all source config records to remove need to 
        '                                        | reload when switching between source tabs on Inventory form
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem

        ' is eShopCONNECT activated ?
        If Me.m_InventorySettingsFacade.IsActivated Then
            ' yes, is Magento connector activated ?
            If Me.m_InventorySettingsFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                ' yes, need to check if there is an Item record in case we are being configured in the User Role !
                If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Rows.Count > 0 Then ' TJS 02/12/11
                    m_MagentoImportFacade = New Lerryn.Facade.ImportExport.MagentoImportFacade(Me.m_InventorySettingsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 25/04/11 TJS 10/06/12

                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                        "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 02/12/11
                    XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(MAGENTO_SOURCE_CODE).ConfigSettings_DEV000221)) ' TJS 02/12/11
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                    Coll = Me.cbeSelectMagentoInstance.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ACCOUNT_DISABLED).ToUpper <> "YES" Then
                            CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID))
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    If Coll.Count > 0 Then
                        ' has Item been saved yet (i.e. ItemCode is not [To be generated])
                        If Me.m_InventoryItemDataset.InventoryItem(0).ItemCode <> Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
                            ' yes, enable Magento details tab
                            Me.PanelPublishOnMagento.Enabled = True
                            If Coll.Count = 1 Then
                                Me.cbeSelectMagentoInstance.SelectedIndex = 0
                                MagentoSiteSelected(Nothing, Nothing)
                            Else
                                AddHandler Me.CheckEditPublishOnMagento.EditValueChanged, AddressOf PublishOnMagentoChange
                                AddHandler Me.cbeSelectMagentoInstance.EditValueChanged, AddressOf MagentoSiteSelected
                                AddHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected
                            End If

                        Else
                            Me.PanelPublishOnMagento.Enabled = False
                            AddHandler Me.CheckEditPublishOnMagento.EditValueChanged, AddressOf PublishOnMagentoChange
                            AddHandler Me.cbeSelectMagentoInstance.EditValueChanged, AddressOf MagentoSiteSelected
                            AddHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected
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
        Else
            ShowActivateMessage = True ' TJS 02/12/11
        End If

    End Sub
#End Region

#Region " EnableDisableMagentoControls "
    Private Sub EnableDisableMagentoControls(ByVal Enable As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to cater for Attribute Set 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for Inventory Qty publishing
        ' 24/02/12 | TJS             | 2011.2.08 | Added missing chkPriceAsIS control and modified to 
        '                                        | lock controls which are set from MAtrix Group
        ' 13/03/13 | TJS             | 2013.1.02 | Corrected InstanceID_DEV000221 field name and modified to cater 
        '                                        | for Magento simple+opt items creating Matrix Groups where only 
        '                                        | the Group Item has a Magento record
        ' 24/03/13 | TJS             | 2013.1.06 | Modified to prevent error if no InventoryMagentoDetails_DEV000221 record found
        ' 30/09/13 | TJS             | 2013.3.02 | Modified to clear Attribute Set error text if control not enabled
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected enabling of lblAttributeSet and ImageComboBoxEditAttributeSet when first
        '                                        | publishing, also modified to cater for price source selectors, corrected string constants 
        '                                        | used to indicate product name and description are taken from InventoryItem table
        ' 20/11/13 | TJS             | 2013.4.00 | Modified to cater for Web Option link
        ' 09/12/13 | TJS             | 2013.4.02 | Corrected setting of DisplayValue column header colour
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strMatrixItemCode As String, strTemp As String, strProperties() As String ' TJS 13/03/13
        Dim bMatrixGroupItem As Boolean, bMatrixItem As Boolean, bTemp As Boolean ' TJS 25/04/11
        Dim bShowMatrixGroupLabel As Boolean, bShowMatrixItemLabel As Boolean ' TJS 13/03/13

        ' is item a Matrix Group Item or a Matrix Item ?
        If Me.m_InventoryItemDataset.InventoryItem.Count > 0 Then
            If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Group" Then
                bMatrixGroupItem = True
                bMatrixItem = False
                bShowMatrixGroupLabel = True ' TJS 13/03/13
                bShowMatrixItemLabel = False ' TJS 13/03/13
            ElseIf Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Item" Then
                bMatrixGroupItem = False
                bMatrixItem = True
                bShowMatrixGroupLabel = False ' TJS 13/03/13
                bShowMatrixItemLabel = True ' TJS 13/03/13
            Else
                bMatrixGroupItem = False
                bMatrixItem = False
                bShowMatrixGroupLabel = False ' TJS 13/03/13
                bShowMatrixItemLabel = False ' TJS 13/03/13
            End If

        Else
            bMatrixGroupItem = False
            bMatrixItem = False
            bShowMatrixGroupLabel = False ' TJS 13/03/13
            bShowMatrixItemLabel = False ' TJS 13/03/13
        End If

        ' are we displaying a Matrix Item ?
        If bMatrixItem Then
            ' yes, get ItemCode for Matrix Group Item
            strMatrixItemCode = Me.m_InventorySettingsFacade.GetField("ItemCode", "InventoryMatrixItem", "MatrixItemCode = '" & _
                Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'")
            strTemp = Me.m_InventorySettingsFacade.GetField("SourceIsGroupItem_DEV000221", "InventoryMagentoDetails_DEV000221", _
                "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND InstanceID_DEV000221 = '" & _
                Me.cbeSelectMagentoInstance.EditValue.ToString & "'") ' TJS 13/03/13
            If Not String.IsNullOrEmpty(strTemp) AndAlso (strTemp.ToLower = "true" Or strTemp = "1") Then ' TJS 13/03/13 TJS 24/03/13
                Me.LabelMatrixItem.Text = "Magento only has a single" & vbCrLf & "product record for this" & vbCrLf & "Matrix Group so all Magento" & vbCrLf & "settings must be set on the" & vbCrLf & "Matrix Group Item." ' TJS 13/03/13
                Enable = False ' TJS 13/03/13
            Else
                ' now get Matrix Group Item properties
                strProperties = Me.m_InventorySettingsFacade.GetRow(New String() {"ProductName_DEV000221", "ProductShortDescription_DEV000221", _
                    "InstanceID_DEV000221"}, "InventoryMagentoDetails_DEV000221", "ItemCode_DEV000221 = '" & strMatrixItemCode & "'", False) ' TJS 02/12/11 TJS 13/11/135
                If strProperties IsNot Nothing Then ' TJS 24/03/13
                    ' remove data bindings for relevant fields
                    Me.TextEditProductName.DataBindings.Clear()
                    Me.MemoEditShortDescription.DataBindings.Clear()
                    ' now display Matrix Group Items properties
                    Me.TextEditProductName.EditValue = strProperties(0)
                    Me.MemoEditShortDescription.EditValue = strProperties(1)

                    Me.TreeListCategories.OptionsBehavior.Editable = False ' TJS 24/02/12
                    Me.TextEditProductName.Properties.ReadOnly = True ' TJS 24/02/12
                End If
            End If
        End If

        ' these controls are enabled if Instance ID is set
        If Me.cbeSelectMagentoInstance.SelectedIndex >= 0 Then
            Me.CheckEditPublishOnMagento.Enabled = True
        Else
            Me.CheckEditPublishOnMagento.Enabled = False
        End If

        If Enable AndAlso Me.CheckEditPublishOnMagento.Checked Then ' TJS 25/04/11 TJS 13/11/13
            Me.ImageComboBoxEditAttributeSet.Enabled = True ' TJS 25/04/11
            Me.lblAttributeSet.Enabled = True ' TJS 25/04/11
        Else
            Me.ImageComboBoxEditAttributeSet.Enabled = False ' TJS 25/04/11
            Me.lblAttributeSet.Enabled = False ' TJS 25/04/11
        End If

        bTemp = False ' TJS 25/04/11
        If Enable AndAlso Me.CheckEditPublishOnMagento.Checked AndAlso ImageComboBoxEditAttributeSet.EditValue IsNot Nothing AndAlso _
            CInt(Me.ImageComboBoxEditAttributeSet.EditValue) >= 0 Then ' TJS 25/04/11
            bTemp = True ' TJS 25/04/11
        End If
        Me.chkPriceAsIS.Enabled = bTemp ' TJS 25/04/11
        Me.DateEditShowAsNewFrom.Enabled = bTemp ' TJS 25/04/11
        Me.DateEditShowAsNewTo.Enabled = bTemp ' TJS 25/04/11
        Me.GridControlProperties.Enabled = bTemp ' TJS 25/04/11
        Me.ImageComboBoxEditVisibility.Enabled = bTemp ' TJS 25/04/11
        Me.ImageComboBoxEditStatus.Enabled = bTemp ' TJS 25/04/11
        Me.lblDescription.Enabled = bTemp ' TJS 25/04/11
        Me.lblInDepth.Enabled = bTemp ' TJS 25/04/11
        Me.lblMagentoAttributes.Enabled = bTemp ' TJS 25/04/11
        Me.lblProductName.Enabled = bTemp ' TJS 25/04/11
        Me.lblShortDescription.Enabled = bTemp ' TJS 25/04/11
        Me.lblShowAsNewFrom.Enabled = bTemp ' TJS 25/04/11
        Me.lblShowAsNewTo.Enabled = bTemp ' TJS 25/04/11
        Me.lblStatus.Enabled = bTemp ' TJS 25/04/11
        Me.lblVisibility.Enabled = bTemp ' TJS 25/04/11
        'Me.lblAttributeSet.Enabled = bTemp ' TJS 13/03/13 TJS 13/11/13
        Me.MemoEditDescription.Enabled = bTemp ' TJS 25/04/11
        Me.MemoEditShortDescription.Enabled = bTemp ' TJS 25/04/11
        Me.MemoEditInDepth.Enabled = bTemp ' TJS 25/04/11
        Me.TreeListCategories.Enabled = bTemp ' TJS 25/04/11
        If bTemp Then ' TJS 13/11/13
            If CBool(Me.chkPriceAsIS.EditValue) Then ' TJS 13/11/13
                Me.lblMagentoSellingPrice.Enabled = False ' TJS 13/11/13
                Me.lblMagentoPriceSource.Enabled = True ' TJS 13/11/13
                Me.lblMagentoSpecialPriceSource.Enabled = True ' TJS 13/11/13
                Me.cbeMagentoPriceSource.Enabled = True ' TJS 13/11/13
                Me.cbeMagentoSpecialPriceSource.Enabled = True ' TJS 13/11/13
                If Me.cbeMagentoSpecialPriceSource.EditValue IsNot Nothing AndAlso Me.cbeMagentoSpecialPriceSource.EditValue.ToString = "N" Then ' TJS 13/11/13
                    Me.lblSpecialPrice.Enabled = True ' TJS 13/11/13
                    Me.lblSpecialFrom.Enabled = True ' TJS 13/11/13
                    Me.lblSpecialTo.Enabled = True ' TJS 13/11/13
                    Me.DateEditSpecialFrom.Enabled = True ' TJS 25/04/11
                    Me.DateEditSpecialTo.Enabled = True ' TJS 25/04/11
                    Me.TextEditSpecialPrice.Enabled = True ' TJS 13/11/13
                Else
                    Me.lblSpecialPrice.Enabled = False ' TJS 13/11/13
                    Me.lblSpecialFrom.Enabled = False ' TJS 13/11/13
                    Me.lblSpecialTo.Enabled = False ' TJS 13/11/13
                    Me.DateEditSpecialFrom.Enabled = False ' TJS 25/04/11
                    Me.DateEditSpecialTo.Enabled = False ' TJS 25/04/11
                    Me.TextEditSpecialPrice.Enabled = False ' TJS 13/11/13
                End If
                Me.TextEditMagentoSellingPrice.Enabled = False ' TJS 13/11/13
            Else
                Me.lblMagentoSellingPrice.Enabled = True ' TJS 13/11/13
                Me.lblSpecialPrice.Enabled = True ' TJS 13/11/13
                Me.lblSpecialFrom.Enabled = True ' TJS 13/11/13
                Me.lblSpecialTo.Enabled = True ' TJS 13/11/13
                Me.lblMagentoPriceSource.Enabled = False ' TJS 13/11/13
                Me.lblMagentoSpecialPriceSource.Enabled = False ' TJS 13/11/13
                Me.cbeMagentoPriceSource.Enabled = False ' TJS 13/11/13
                Me.cbeMagentoSpecialPriceSource.Enabled = False ' TJS 13/11/13
                Me.DateEditSpecialFrom.Enabled = True ' TJS 25/04/11
                Me.DateEditSpecialTo.Enabled = True ' TJS 25/04/11
                Me.TextEditMagentoSellingPrice.Enabled = True ' TJS 13/11/13
                Me.TextEditSpecialPrice.Enabled = True ' TJS 13/11/13
            End If
        Else
            Me.lblMagentoSellingPrice.Enabled = bTemp ' TJS 25/04/11
            Me.lblSpecialPrice.Enabled = bTemp ' TJS 25/04/11
            Me.lblSpecialFrom.Enabled = bTemp ' TJS 25/04/11
            Me.lblSpecialTo.Enabled = bTemp ' TJS 25/04/11
            Me.lblMagentoPriceSource.Enabled = bTemp ' TJS 13/11/13
            Me.lblMagentoSpecialPriceSource.Enabled = bTemp ' TJS 13/11/13
            Me.DateEditSpecialFrom.Enabled = bTemp ' TJS 25/04/11
            Me.DateEditSpecialTo.Enabled = bTemp ' TJS 25/04/11
            Me.TextEditMagentoSellingPrice.Enabled = bTemp ' TJS 25/04/11
            Me.TextEditSpecialPrice.Enabled = bTemp ' TJS 25/04/11
            Me.cbeMagentoPriceSource.Enabled = bTemp ' TJS 13/11/13
            Me.cbeMagentoSpecialPriceSource.Enabled = bTemp ' TJS 13/11/13
        End If
        Me.TextEditProductName.Enabled = bTemp ' TJS 25/04/11
        'Me.ImageComboBoxEditAttributeSet.Enabled = bTemp ' TJS 13/03/13 TJS 13/11/13
        If Not Me.ImageComboBoxEditAttributeSet.Enabled And Me.ImageComboBoxEditAttributeSet.ErrorText <> "" Then ' TJS 30/09/13 TJS 20/11/13
            Me.ImageComboBoxEditAttributeSet.ErrorText = "" ' TJS 30/09/13
        End If
        If Me.GridControlProperties.Enabled Then ' TJS 09/12/13
            Me.colPropertiesDisplayValue.AppearanceHeader.ForeColor = Color.Black ' TJS 09/12/13
        Else
            Me.colPropertiesDisplayValue.AppearanceHeader.ForeColor = Color.Gray
        End If
        ' display labels for Matrix Items and Matrix Group Items
        Me.LabelMatrixGroupItem.Visible = bShowMatrixGroupLabel ' TJS 13/03/13
        Me.LabelMatrixItem.Visible = bShowMatrixItemLabel ' TJS 13/03/13

        ' now set Product name and description text colour
        If Me.TextEditProductName.Enabled Then
            If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
                TextEditProductName.ForeColor = Color.Gray
            Else
                TextEditProductName.ForeColor = Color.Black
            End If
        End If
        If Me.MemoEditShortDescription.Enabled Then
            If MemoEditShortDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Or _
                MemoEditShortDescription.Text = INVENTORY_USE_WEB_OPTION_TAB_SUMMARY Then ' TJS 13/11/13 TJS 20/11/13
                MemoEditShortDescription.ForeColor = Color.Gray
            Else
                MemoEditShortDescription.ForeColor = Color.Black
            End If
        End If
        ' start of code added TJS 20/11/13
        If Me.MemoEditDescription.Enabled Then
            If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Or _
                MemoEditDescription.Text = INVENTORY_USE_WEB_OPTION_TAB_DESCRIPTION Then
                MemoEditDescription.ForeColor = Color.Gray
            Else
                MemoEditDescription.ForeColor = Color.Black
            End If
        End If
        ' end of code added TJS 20/11/13

        Me.lblQtyPublishing.Enabled = bTemp ' TJS 02/12/11
        Me.RadioGroupQtyPublishing.Enabled = bTemp ' TJS 02/12/11
        If Me.RadioGroupQtyPublishing.SelectedIndex >= 0 And bTemp Then ' TJS 02/12/11
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
            ' save any changes to Magento settings
            If Me.m_InventorySettingsFacade.UpdateDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                    "CreateInventoryMagentoDetails_DEV000221", "UpdateInventoryMagentoDetails_DEV000221", "DeleteInventoryMagentoDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                    "CreateInventoryMagentoTagDetails_DEV000221", "UpdateInventoryMagentoTagDetails_DEV000221", "DeleteInventoryMagentoTagDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName, _
                    "CreateInventoryMagentoCategories_DEV000221", "UpdateInventoryMagentoCategories_DEV000221", "DeleteInventoryMagentoCategories_DEV000221"}}, _
                    Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Inventory Magento Settings", False) Then
                ReturnValue = MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache)

            Else
                ReturnValue = DialogResult.Cancel
            End If
            ' setting error text on dataset doesn't cause control to display it so copy it
            If Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.Count > 0 Then ' TJS 02/12/11
                Me.RadioGroupQtyPublishing.ErrorText = Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221(0).GetColumnError("QtyPublishingType_DEV000221") ' TJS 02/12/11
            End If
            Return ReturnValue ' TJS 02/12/11

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Function
#End Region

#Region " SetProposedMagentoPrice "
    Private Sub SetProposedMagentoPrice(ByVal ItemCode As String, ByVal CurrencyCode As String)
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

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLMagentoNode As XElement
        Dim XMLMagentoNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strSQL As String, strTemp As String, bUpdatePrice As Boolean

        ' get config settings
        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(MAGENTO_SOURCE_CODE).ConfigSettings_DEV000221.Trim) ' TJS 02/12/11
        XMLMagentoNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
        ' check connector count is valid i.e. number of Magento settings is not more then the licence limit
        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLMagentoNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(MAGENTO_CONNECTOR_CODE) Then
            ' check each Magento record for current Instance ID
            For Each XMLMagentoNode In XMLMagentoNodeList
                XMLTemp = XDocument.Parse(XMLMagentoNode.ToString)
                ' have we found current Instance ID ?
                If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = Me.cbeSelectMagentoInstance.Text Then
                    ' yes, has Magento price been set ?
                    bUpdatePrice = False
                    If Me.TextEditMagentoSellingPrice.Text <> "" Then
                        If CDec(Me.TextEditMagentoSellingPrice.EditValue) = 0 Then
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
                            Me.TextEditMagentoSellingPrice.EditValue = CDec(strTemp)
                        End If
                    End If
                End If
                Exit For
            Next
        End If

    End Sub
#End Region

#Region " DoBackgroundTasks "
    Private Sub DoBackgroundTasks(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles m_BackgroundWorker.DoWork
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 09/04/11 | TJS             | 2011.0.10 | Code moved to MagentoImportFacade
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to cater for Magento Product Attribute Sets
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento Atribute values table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        If bGetMagentoCategories Then ' TJS 25/04/11
            m_MagentoImportFacade.GetMagentoCategories(Me.cbeSelectMagentoInstance.EditValue.ToString, worker.CancellationPending, m_CategoryTable)
        End If

        If bGetMagentoAttributeSets Then ' TJS 25/04/11
            m_MagentoImportFacade.GetMagentoAttributeSets(Me.cbeSelectMagentoInstance.EditValue.ToString, worker.CancellationPending) ' TJS 25/04/11
        End If

        If bGetMagentoAttributesForSet And GetAttributesForSetID >= 0 Then ' TJS 25/04/11
            m_MagentoImportFacade.GetMagentoAttributesForSet(Me.cbeSelectMagentoInstance.EditValue.ToString, worker.CancellationPending, GetAttributesForSetID) ' TJS 25/04/11

            m_MagentoImportFacade.GetMagentoAttributeList(Me.cbeSelectMagentoInstance.EditValue.ToString, worker.CancellationPending) ' TJS 13/11/13
        End If

    End Sub

    Private Sub DoBackgroundTasksCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles m_BackgroundWorker.RunWorkerCompleted
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to cater for background worker thread being 
        '                                        | used to get product attributes for publishing new items
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to set Attribute Set error flag if no attribute selected
        ' 10/05/12 | TJS             | 2012.1.05 | Corrected detection of TagRequired_DEV000221 as null value
        ' 23/11/12 | TJS             | 2012.1.16 | Modified to prevent errors if no attributes returned because Magento API could not be contacted
        ' 29/05/13 | TJS/FA          | 2013.1.19 | Modified to allow publishing item from another instance
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento Attribute Value selection
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to add tax_class_id as a parameter instead of excluding it
        '                                        | and to set weight parameter
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row ' TJS 25/04/11
        Dim rowMagentoTagDetailsToCopy As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row ' TJS/FA 29/05/13
        Dim ProductAttribute As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType ' TJS 25/04/11
        Dim AttributeSet As Lerryn.Facade.ImportExport.MagentoSOAPConnector.AttributeSetType ' TJS 25/04/11
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection ' TJS 25/04/11
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem ' TJS 25/04/11
        Dim iLoop As Integer, bTagExists As Boolean, bTagsUpdated As Boolean ' TJS 25/04/11

        If bGetMagentoCategories Then ' TJS 25/04/11
            Me.TreeListCategories.DataSource = m_CategoryTable
            ' start of code added FA 29/05/13
            If bCopyInstanceSettings Then
                For Each CategoryRow As System.Data.DataRow In m_CategoryTable.Rows
                    If CategoryRow.Item("SourceCategoryID").ToString = m_TempDataset.InventoryMagentoCategories_DEV000221(0).MagentoCategoryID_DEV000221.ToString Then
                        m_TempDataset.InventoryMagentoCategories_DEV000221(0).IsActive_DEV000221 = True
                    End If
                Next
            End If
            ' end of code added FA 29/05/13
            Me.TreeListCategories.ExpandAll()
            Me.TreeListCategories.Enabled = True
            Me.pnlGetCategoryProgress.Visible = False
            bGetMagentoCategories = False ' TJS 25/04/11
        End If

        ' start of coded added TJS 25/04/11
        If bGetMagentoAttributeSets Then
            If m_MagentoImportFacade.MagentoAttributeSets.Length > 0 Then
                Coll = Me.ImageComboBoxEditAttributeSet.Properties.Items
                Coll.BeginUpdate()
                Coll.Clear()
                For Each AttributeSet In m_MagentoImportFacade.MagentoAttributeSets
                    CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
                    CollItem.Description = AttributeSet.SetName
                    CollItem.Value = AttributeSet.SetID
                    Coll.Add(CollItem)
                Next
                Coll.EndUpdate()
                If Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221(0).IsAttributeSetID_DEV000221Null OrElse _
                    Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221(0).AttributeSetID_DEV000221 < 0 Then ' TJS 02/12/11
                    Me.ImageComboBoxEditAttributeSet.ErrorText = "Please select an Attribute Set" ' TJS 02/12/11
                End If
            End If
            bGetMagentoAttributeSets = False
        End If

        If bGetMagentoAttributesForSet And m_MagentoImportFacade.ProductAttributes IsNot Nothing Then ' TJS 23/11/12
            bTagsUpdated = False
            For Each ProductAttribute In m_MagentoImportFacade.ProductAttributes
                bTagExists = False
                For iLoop = 0 To Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.Count - 1
                    If Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagName_DEV000221 = ProductAttribute.AttributeName Then
                        bTagExists = True
                        If Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagRequired_DEV000221Null OrElse _
                            Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagRequired_DEV000221 <> ProductAttribute.AttributeReqd Then ' TJS 10/06/12
                            Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagRequired_DEV000221 = ProductAttribute.AttributeReqd
                            bTagsUpdated = True
                        End If
                    End If
                Next
                If Not bTagExists Then
                    Select Case ProductAttribute.AttributeName
                        Case "product_id", "sku", "name", "short_description", "description", "in_depth", "set", "type", "categories", "websites", "price", _
                            "cost", "news_from_date", "news_to_date", "special_from_date", "special_to_date", "special_price", "visibility", "status", _
                            "manufacturer", "type_id", "old_id", "category_ids", "created_at", "updated_at", "tier_price", "sku type" ' TJS 09/12/13
                            ' ignore as these are either included in the MagentoDetails table or not handled

                        Case Else
                            rowMagentoTagDetails = Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                            rowMagentoTagDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                            rowMagentoTagDetails.InstanceID_DEV000221 = Me.cbeSelectMagentoInstance.EditValue.ToString
                            rowMagentoTagDetails.TagName_DEV000221 = ProductAttribute.AttributeName
                            rowMagentoTagDetails.TagDisplayName_DEV000221 = ProductAttribute.AttributeName.Replace("_", " ")
                            rowMagentoTagDetails.LineNumber_DEV000221 = 1
                            rowMagentoTagDetails.TagRequired_DEV000221 = ProductAttribute.AttributeReqd
                            rowMagentoTagDetails.AttributeID_DEV000221 = ProductAttribute.AttributeID ' TJS 13/11/13
                            rowMagentoTagDetails.AttributeHasSelectValues_DEV000221 = False ' TJS 13/11/13
                            Select Case ProductAttribute.AttributeType.ToLower
                                Case "text"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                                Case "textarea"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Memo"
                                Case "date"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Date"
                                Case "price"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Numeric"
                                    ' start of code added TJS 13/11/13
                                Case "select"
                                    If ProductAttribute.AttributeName.ToLower = "color" Or ProductAttribute.AttributeName.ToLower = "status" Then
                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Integer"
                                    Else
                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                                    End If
                                    rowMagentoTagDetails.AttributeHasSelectValues_DEV000221 = True
                                    ' end of code added TJS 13/11/13
                                Case Else
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                            End Select
                            ' start of code added TJS/FA 29/05/13
                            If bCopyInstanceSettings Then
                                'rowMagentoTagDetailsToCopy = Me.m_TempDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(Me.m_TempDataset.InventoryItem(0).ItemCode, Me.m_TempDataset.InventoryMagentoDetails_DEV000221(0).InstanceID_DEV000221, ProductAttribute.AttributeName, 1)
                                'FA 29/05/13 corrected variable passed
                                rowMagentoTagDetailsToCopy = Me.m_TempDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(Me.m_TempDataset.InventoryMagentoDetails_DEV000221(0).ItemCode_DEV000221, Me.m_TempDataset.InventoryMagentoDetails_DEV000221(0).InstanceID_DEV000221, ProductAttribute.AttributeName, 1)
                                If rowMagentoTagDetailsToCopy IsNot Nothing Then
                                    If Not rowMagentoTagDetailsToCopy.IsTagTextValue_DEV000221Null Then
                                        rowMagentoTagDetails.TagTextValue_DEV000221 = rowMagentoTagDetailsToCopy.TagTextValue_DEV000221
                                    End If
                                    Select Case rowMagentoTagDetailsToCopy.TagDataType_DEV000221
                                        Case "Memo"
                                            If Not rowMagentoTagDetailsToCopy.IsTagMemoValue_DEV000221Null Then
                                                rowMagentoTagDetails.TagMemoValue_DEV000221 = rowMagentoTagDetailsToCopy.TagMemoValue_DEV000221
                                            End If
                                        Case "Date"
                                            If Not rowMagentoTagDetailsToCopy.IsTagDateValue_DEV000221Null Then
                                                rowMagentoTagDetails.TagDateValue_DEV000221 = rowMagentoTagDetailsToCopy.TagDateValue_DEV000221
                                            End If
                                        Case "Integer", "Numeric"
                                            If Not rowMagentoTagDetailsToCopy.IsTagNumericValue_DEV000221Null Then
                                                rowMagentoTagDetails.TagNumericValue_DEV000221 = rowMagentoTagDetailsToCopy.TagNumericValue_DEV000221
                                            End If

                                    End Select
                                End If

                                ' start of code added TJS 09/12/13
                            ElseIf rowMagentoTagDetails.TagName_DEV000221 = "weight" Then
                                If m_WeightUnits = "L" Then
                                    rowMagentoTagDetails.TagTextValue_DEV000221 = Me.GetField("WeightInPounds", "InventoryUnitMeasure", "ItemCode = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND UnitMeasureCode = 'EACH'")
                                    rowMagentoTagDetails.DisplayedValue = rowMagentoTagDetails.TagTextValue_DEV000221
                                ElseIf m_WeightUnits = "K" Then
                                    rowMagentoTagDetails.TagTextValue_DEV000221 = Me.GetField("WeightInKilograms", "InventoryUnitMeasure", "ItemCode = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND UnitMeasureCode = 'EACH'")
                                    rowMagentoTagDetails.DisplayedValue = rowMagentoTagDetails.TagTextValue_DEV000221
                                End If
                                ' end of code added TJS 09/12/13
                            End If
                            ' end of code added TJS/FA 29/05/13
                            Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)
                            bTagsUpdated = True

                    End Select
                End If
            Next
            Me.pnlGetAttributeProgress.Visible = False
            bGetMagentoAttributesForSet = False
            GetAttributesForSetID = -1
            If bTagsUpdated And Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221(0).RowState <> DataRowState.Added Then
                Interprise.Presentation.Base.Message.MessageWindow.Show("One or more Magento Product Attributes have been added or their Required flag updated - their values need checking and saving.")
            End If
        End If
        EnableDisableMagentoControls(True)
        ' end of coded added TJS 25/04/11
        Cursor = Cursors.Default

    End Sub
#End Region

#Region " UndoChanges "
    Public Overrides Sub UndoChanges()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/09/13 | TJS             | 2013.3.02 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.UndoChanges()

        Try
            Me.EndCurrentEdit(New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName})
            Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.RejectChanges()
            Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.RejectChanges()
            Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.RejectChanges()

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try

    End Sub
#End Region
#End Region

#Region " Events "
#Region " MagentoSiteSelected "
    Private Sub MagentoSiteSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 09/04/11 | TJS             | 2011.0.09 | Modified to use category list in Import facade
        ' 25/04/11 | TJS             | 2011.0.12 | Corrected Magento Instance use
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and modified to cater for all source config records being loaded
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to cater for Magento simple+opt items creating MAtrix Groups 
        '                                        | where only the Group Item has an InventoryMagentoDetails_DEV000221 record
        ' 24/03/13 | TJS             | 2013.1.06 | Modified to prevent error if no InventoryMagentoDetails_DEV000221 record found
        ' 30/09/13 | TJS             | 2013.3.02 | Corrected reading of MAtrix Item Tag properties and categories
        ' 13/11/13 | TJS             | 2013.3.08 | Added missing category mapping columns 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim colNewColumn As DataColumn, strMatrixItemCode As String, strTemp As String ' TJS 13/03/13
        Dim iLoop As Integer, iSelectedSiteIndex As Integer ' TJS 02/12/11

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.cbeSelectMagentoInstance.EditValueChanged, AddressOf MagentoSiteSelected ' TJS 25/04/11
            RemoveHandler Me.CheckEditPublishOnMagento.EditValueChanged, AddressOf PublishOnMagentoChange ' TJS 25/04/11
            RemoveHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected ' TJS 25/04/11
            If Me.cbeSelectMagentoInstance.SelectedIndex >= 0 Then
                ' save Site Index ID
                iSelectedSiteIndex = Me.cbeSelectMagentoInstance.SelectedIndex ' TJS 02/12/11
                ' get Magento publishing details - are we displaying a Matrix Item
                If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Then ' TJS 24/02/12
                    ' yes, get ItemCode for Matrix Group Item
                    strMatrixItemCode = Me.m_InventorySettingsFacade.GetField("ItemCode", "InventoryMatrixItem", "MatrixItemCode = '" & _
                        Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'") ' TJS 24/02/12
                    strTemp = Me.m_InventorySettingsFacade.GetField("SourceIsGroupItem_DEV000221", "InventoryMagentoDetails_DEV000221", _
                        "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND InstanceID_DEV000221 = '" & _
                        Me.cbeSelectMagentoInstance.EditValue.ToString & "'") ' TJS 13/03/13
                    If Not String.IsNullOrEmpty(strTemp) AndAlso strTemp.ToLower = "true" Or strTemp = "1" Then ' TJS 13/03/13 TJS 24/03/13
                        ' InventoryMagentoDetails_DEV000221 etc are only held for Matrix Group
                        Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                "ReadInventoryMagentoCategories_DEV000221", AT_ITEM_CODE, strMatrixItemCode, AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                "ReadInventoryMagentoTagDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)  ' TJS 13/03/13
                    Else
                        ' InventoryMagentoDetails_DEV000221 etc are held for Matrix Item
                        Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                "ReadInventoryMagentoCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                "ReadInventoryMagentoTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)  ' TJS 24/02/12 TJS 30/09/13
                    End If
                Else
                    Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                            "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName, _
                            "ReadInventoryMagentoCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                            "ReadInventoryMagentoTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_INSTANCE_ID, Me.cbeSelectMagentoInstance.EditValue.ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 25/04/11
                End If

                Me.cbeSelectMagentoInstance.SelectedIndex = iSelectedSiteIndex ' TJS 02/12/11
                ' copy tag properties to DisplayedValue field for display purposes
                For iLoop = 0 To Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.Count - 1
                    Select Case Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDataType_DEV000221
                        Case "Text", "Boolean"
                            If Not Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagTextValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagTextValue_DEV000221
                            End If

                        Case "Memo"
                            If Not Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagMemoValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221
                            End If

                        Case "Date", "DateTime"
                            If Not Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagDateValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDateValue_DEV000221.ToString
                            End If

                        Case "Integer", "Numeric"
                            If Not Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagNumericValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagNumericValue_DEV000221.ToString
                            End If

                    End Select
                Next
                ' now we have set DisplayedValue field, accept changes so we can tell if user changes anything
                Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.AcceptChanges() ' TJS 20/05/12
                XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(MAGENTO_SOURCE_CODE).ConfigSettings_DEV000221)) ' TJS 02/12/11
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = Me.cbeSelectMagentoInstance.EditValue.ToString Then
                        If m_BackgroundWorker IsNot Nothing Then
                            m_BackgroundWorker.CancelAsync()
                        End If
                        ReDim m_MagentoImportFacade.MagentoCategories(0) ' TJS 09/04/11
                        m_MagentoImportFacade.MagentoCategories(0).CategoryID = 0 ' TJS 09/04/11
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

                        ' start of code added TJS 13/11/13
                        colNewColumn = New DataColumn
                        colNewColumn.Caption = "IS Category Name"
                        colNewColumn.ColumnName = "ISCategoryName"
                        colNewColumn.DataType = System.Type.GetType("System.String")
                        m_CategoryTable.Columns.Add(colNewColumn)
                        colNewColumn.Dispose()

                        colNewColumn = New DataColumn
                        colNewColumn.Caption = "IS Category Code"
                        colNewColumn.ColumnName = "ISCategoryCode"
                        colNewColumn.DataType = System.Type.GetType("System.String")
                        m_CategoryTable.Columns.Add(colNewColumn)
                        colNewColumn.Dispose()
                        ' end of code added TJS 13/11/13

                        If Me.CheckEditPublishOnMagento.Checked Then ' TJS 25/04/11
                            Me.TreeListCategories.Enabled = False
                            Me.pnlGetCategoryProgress.Visible = True
                            ReDim m_MagentoImportFacade.MagentoAttributeSets(0) ' TJS 25/04/11

                            ' get Magento Categories as background task
                            If m_BackgroundWorker Is Nothing Then
                                m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                                m_BackgroundWorker.WorkerSupportsCancellation = True
                                m_BackgroundWorker.WorkerReportsProgress = False
                            End If
                            bGetMagentoCategories = True ' TJS 25/04/11
                            bGetMagentoAttributeSets = True ' TJS 25/04/11
                            If ImageComboBoxEditAttributeSet.EditValue IsNot Nothing AndAlso CInt(ImageComboBoxEditAttributeSet.EditValue) >= 0 Then ' TJS 25/04/11
                                GetAttributesForSetID = CInt(ImageComboBoxEditAttributeSet.EditValue) ' TJS 25/04/11
                                bGetMagentoAttributesForSet = True ' TJS 25/04/11
                                Me.pnlGetAttributeProgress.Visible = True ' TJS 25/04/11
                            Else
                                bGetMagentoAttributesForSet = False ' TJS 25/04/11
                                Me.pnlGetAttributeProgress.Visible = False ' TJS 25/04/11
                            End If
                            m_BackgroundWorker.RunWorkerAsync()
                        End If
                        Exit For
                    End If
                Next

                ' now enable Browse List controls etc
                If Me.CheckEditPublishOnMagento.Checked And (m_BackgroundWorker Is Nothing OrElse Not m_BackgroundWorker.IsBusy) Then ' TJS 25/04/11
                    EnableDisableMagentoControls(True)
                Else
                    EnableDisableMagentoControls(False)
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            If m_BackgroundWorker Is Nothing OrElse Not m_BackgroundWorker.IsBusy Then ' TJS 25/04/11
                Cursor = Cursors.Default
            End If
            AddHandler Me.CheckEditPublishOnMagento.EditValueChanged, AddressOf PublishOnMagentoChange
            AddHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected
            AddHandler Me.cbeSelectMagentoInstance.EditValueChanged, AddressOf MagentoSiteSelected

        End Try

    End Sub
#End Region

#Region " PublishOnMagentoChange "
    Private Sub PublishOnMagentoChange(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to cater for Magento Product Attribute Sets
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for Inventory stock quantity publishing
        ' 28/05/13 | TJS             | 2013.1.18 | Added ability to copy initial settings from another instance
        ' 29/05/13 | FA/TJS          | 2013.1.19 | Modified to return only instances where item has already been published
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to create Magento website record and orrected string constants 
        '                                        | used to indicate product name and description are taken from InventoryItem table
        ' 20/11/13 | TJs             | 2013.4.00 | Modified to pass currency to SetProposedMagentoPrice function and cater for Description sources
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to cater for Weight Units
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoDetails_DEV000221Row
        Dim rowMagentoCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoCategories_DEV000221Row ' TJS 13/11/13
        Dim frmSelectForCopy As Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopyForm ' TJS 23/05/13
        Dim frmSelectDescriptionSources As Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceForm ' TJS 20/11/13
        Dim strPublishedInstances As String(), iLoop As Integer, iInstLoop As Integer ' TJS 23/05/13 'FA 29/05/13
        Dim strInstances As String()(), strMatrixItemCode As String, strTemp As String
        Dim bInstanceFound As Boolean, strMagentoShortDescSource As String, strMagentoDescriptionSource As String 'FA 29/05/13 TJS 20/11/13

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.CheckEditPublishOnMagento.EditValueChanged, AddressOf PublishOnMagentoChange
            If CheckEditPublishOnMagento.EditValue.ToString <> "" Then
                If CBool(Me.CheckEditPublishOnMagento.EditValue) Then
                    If Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.Rows.Count = 0 Then
                        strMagentoShortDescSource = "1" ' TJS 20/11/13
                        strMagentoDescriptionSource = "1" ' TJS 20/11/13
                        ' start of code added TJS/FA 29/05/13
                        ' do we have multiple instances ?
                        bCopyInstanceSettings = False
                        If Me.cbeSelectMagentoInstance.Properties.Items.Count > 1 Then
                            'TJS 29/05/13
                            strInstances = Me.m_InventorySettingsFacade.GetRows(New String() {"InstanceID_DEV000221", "MagentoProductID_DEV000221"}, "InventoryMagentoDetails_DEV000221", "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND Publish_DEV000221 = 1")

                            ReDim strPublishedInstances(Me.cbeSelectMagentoInstance.Properties.Items.Count - 1)
                            bInstanceFound = False
                            For iLoop = 0 To Me.cbeSelectMagentoInstance.Properties.Items.Count - 1
                                'FA 29/05/13
                                For iInstLoop = 0 To strInstances.Length - 1
                                    If strInstances(iInstLoop)(0).ToString = Me.cbeSelectMagentoInstance.Properties.Items(iLoop).ToString Then
                                        strPublishedInstances(iLoop) = Me.cbeSelectMagentoInstance.Properties.Items(iLoop).ToString
                                        bInstanceFound = True
                                    End If
                                Next
                            Next
                            If bInstanceFound Then
                                frmSelectForCopy = New Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopyForm
                                frmSelectForCopy.SourceCode = MAGENTO_SOURCE_CODE
                                frmSelectForCopy.PublishedInstances = strPublishedInstances
                                If frmSelectForCopy.ShowDialog <> DialogResult.OK Then
                                    Me.CheckEditPublishOnMagento.EditValue = False
                                    Return
                                Else
                                    If frmSelectForCopy.InstanceToCopy <> "" Then
                                        m_TempDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway 'TJS/FA 29/05/13
                                        m_tempFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_TempDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) 'TJS/FA 29/05/13
                                        ' get Magento publishing details - are we displaying a Matrix Item
                                        If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Then
                                            ' yes, get ItemCode for Matrix Group Item
                                            strMatrixItemCode = Me.m_InventorySettingsFacade.GetField("ItemCode", "InventoryMatrixItem", "MatrixItemCode = '" & _
                                                Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'") ' TJS 24/02/12
                                            strTemp = Me.m_InventorySettingsFacade.GetField("SourceIsGroupItem_DEV000221", "InventoryMagentoDetails_DEV000221", _
                                                "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND InstanceID_DEV000221 = '" & _
                                                Me.cbeSelectMagentoInstance.EditValue.ToString & "'") ' TJS 13/03/13
                                            If Not String.IsNullOrEmpty(strTemp) AndAlso strTemp.ToLower = "true" Or strTemp = "1" Then
                                                ' InventoryMagentoDetails_DEV000221 etc are only held for Matrix Group
                                                m_tempFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                                       "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, _
                                                        AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                                        "ReadInventoryMagentoCategories_DEV000221", AT_ITEM_CODE, strMatrixItemCode, _
                                                        AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                                        "ReadInventoryMagentoTagDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, _
                                                        AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' FA 29/05/13
                                            Else
                                                ' InventoryMagentoDetails_DEV000221 are held for Matrix Item, but rest are for Matrix Group
                                                m_tempFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                                        "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                        AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                                        "ReadInventoryMagentoCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                        AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                                        "ReadInventoryMagentoTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                        AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' FA 29/05/13
                                            End If
                                        Else
                                            m_tempFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                                    "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                    AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                New String() {Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                                    "ReadInventoryMagentoCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                    AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                New String() {Me.m_InventorySettingsDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                                    "ReadInventoryMagentoTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                    AT_INSTANCE_ID, frmSelectForCopy.InstanceToCopy}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' FA 29/05/13
                                        End If

                                        bCopyInstanceSettings = True
                                    End If
                                End If
                            End If
                        End If
                        ' end of code added TJS/FA 29/05/13
                        ' start of code added TJS 20/11/13
                        If Not bCopyInstanceSettings Then
                            frmSelectDescriptionSources = New Lerryn.Presentation.eShopCONNECT.SelectDescriptionSourceForm
                            frmSelectDescriptionSources.Source1Descriptions = New String() {"Magento Short Description to be set as CB Item Ext Description", "Magento Short Description to be set as CB Item Web Option Summary"}
                            frmSelectDescriptionSources.Source2Descriptions = New String() {"Magento Description to be entered manually", "Magento Description to be set as CB Item Ext Description", "Magento Description to be set as CB Item Web Option Description"}
                            If frmSelectDescriptionSources.ShowDialog <> DialogResult.OK Then
                                Me.CheckEditPublishOnMagento.EditValue = False
                                Return
                            Else
                                strMagentoShortDescSource = frmSelectDescriptionSources.Source1Option
                                strMagentoDescriptionSource = frmSelectDescriptionSources.Source2Option
                                m_WeightUnits = frmSelectDescriptionSources.WeightUnits ' TJS 09/12/13
                            End If
                        End If
                        ' end of code added TJS 20/11/13
                        RemoveHandler Me.cbeSelectMagentoInstance.EditValueChanged, AddressOf MagentoSiteSelected ' TJS 25/04/11
                        RemoveHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected ' TJS 25/04/11
                        rowMagentoDetails = Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.NewInventoryMagentoDetails_DEV000221Row
                        rowMagentoDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        rowMagentoDetails.Publish_DEV000221 = True
                        rowMagentoDetails.AttributeSetID_DEV000221 = -1 ' TJS 25/04/11
                        rowMagentoDetails.SellingPrice_DEV000221 = 0
                        rowMagentoDetails.InstanceID_DEV000221 = Me.cbeSelectMagentoInstance.EditValue.ToString
                        rowMagentoDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION ' TJS 13/11/13
                        If strMagentoShortDescSource = "2" Then ' TJS 20/11/13
                            rowMagentoDetails.ProductShortDescription_DEV000221 = INVENTORY_USE_WEB_OPTION_TAB_SUMMARY ' TJS 20/11/13
                        Else
                            rowMagentoDetails.ProductShortDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION ' TJS 25/04/11 TJS 13/11/13
                        End If
                        ' start of code added TJS 20/11/13
                        If strMagentoDescriptionSource = "3" Then
                            rowMagentoDetails.ProductDescription_DEV000221 = INVENTORY_USE_WEB_OPTION_TAB_DESCRIPTION
                        ElseIf strMagentoDescriptionSource = "2" Then
                            rowMagentoDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION
                        End If
                        ' end of code added TJS 20/11/13
                        rowMagentoDetails.TotalQtyWhenLastPublished_DEV000221 = 0 ' TJS 02/12/11
                        rowMagentoDetails.QtyLastPublished_DEV000221 = 0 ' TJS 02/12/11
                        Me.m_InventorySettingsDataset.InventoryMagentoDetails_DEV000221.AddInventoryMagentoDetails_DEV000221Row(rowMagentoDetails)
                        SetProposedMagentoPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)) ' TJS 20/11/13

                        ' start of code added TJS 13/11/13
                        rowMagentoCategory = Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                        rowMagentoCategory.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        rowMagentoCategory.InstanceID_DEV000221 = Me.cbeSelectMagentoInstance.EditValue.ToString
                        rowMagentoCategory.MagentoCategoryID_DEV000221 = -1
                        rowMagentoCategory.MagentoWebSiteID_DEV000221 = 1
                        rowMagentoCategory.IsActive_DEV000221 = True
                        Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory)
                        ' end of code added TJS 13/11/13

                        AddHandler Me.cbeSelectMagentoInstance.EditValueChanged, AddressOf MagentoSiteSelected ' TJS 25/04/11
                        AddHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected ' TJS 25/04/11
                    End If

                    ' start of coded added TJS 25/04/11
                    Me.TreeListCategories.Enabled = False
                    Me.pnlGetCategoryProgress.Visible = True
                    ReDim m_MagentoImportFacade.MagentoAttributeSets(0)

                    ' get Magento Categories as background task
                    If m_BackgroundWorker Is Nothing Then
                        m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                        m_BackgroundWorker.WorkerSupportsCancellation = True
                        m_BackgroundWorker.WorkerReportsProgress = False
                    End If
                    bGetMagentoCategories = True
                    bGetMagentoAttributeSets = True
                    If ImageComboBoxEditAttributeSet.EditValue IsNot Nothing AndAlso CInt(ImageComboBoxEditAttributeSet.EditValue) >= 0 Then
                        GetAttributesForSetID = CInt(ImageComboBoxEditAttributeSet.EditValue)
                        bGetMagentoAttributesForSet = True
                        Me.pnlGetAttributeProgress.Visible = True
                    Else
                        bGetMagentoAttributesForSet = False
                        Me.pnlGetAttributeProgress.Visible = False
                    End If
                    m_BackgroundWorker.RunWorkerAsync()
                    ' end of coded added TJS 25/04/11

                Else
                    EnableDisableMagentoControls(False)
                End If
            Else
                EnableDisableMagentoControls(False)
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            If m_BackgroundWorker Is Nothing OrElse Not m_BackgroundWorker.IsBusy Then ' TJS 25/04/11
                Cursor = Cursors.Default
            End If
            AddHandler Me.CheckEditPublishOnMagento.EditValueChanged, AddressOf PublishOnMagentoChange

        End Try

    End Sub
#End Region

#Region " MagentoAttributeSetSelected "
    Private Sub MagentoAttributeSetSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to clear error flag when attribute set selected
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected

            If ImageComboBoxEditAttributeSet.EditValue IsNot Nothing AndAlso CInt(ImageComboBoxEditAttributeSet.EditValue) >= 0 Then
                Me.ImageComboBoxEditAttributeSet.ErrorText = "" ' TJS 02/12/11
                GetAttributesForSetID = CInt(ImageComboBoxEditAttributeSet.EditValue)
                If m_BackgroundWorker Is Nothing Then
                    m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                    m_BackgroundWorker.WorkerSupportsCancellation = True
                    m_BackgroundWorker.WorkerReportsProgress = False
                End If

                ' get Magento Categories as background task
                If m_BackgroundWorker Is Nothing Then
                    m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                    m_BackgroundWorker.WorkerSupportsCancellation = True
                    m_BackgroundWorker.WorkerReportsProgress = False
                End If
                bGetMagentoCategories = False
                bGetMagentoAttributeSets = False
                bGetMagentoAttributesForSet = True
                Me.pnlGetAttributeProgress.Visible = True
                m_BackgroundWorker.RunWorkerAsync()
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            If m_BackgroundWorker Is Nothing OrElse Not m_BackgroundWorker.IsBusy Then
                Cursor = Cursors.Default
            End If
            AddHandler Me.ImageComboBoxEditAttributeSet.EditValueChanged, AddressOf MagentoAttributeSetSelected

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
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 24/02/12 | TJS             | 2011.2.08 | Added missing code to add row to dataset
        ' 19/09/13 | TJS             | 2013.3.00 | Corrected initialisation of MagentoWebSiteID value
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowTreeList As System.Data.DataRowView
        Dim rowMagentoCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoCategories_DEV000221Row

        Try

            If e.Column.Name = "colTreeListCategoryActive" Then
                rowTreeList = DirectCast(Me.TreeListCategories.GetDataRecordByNode(e.Node), System.Data.DataRowView)
                rowMagentoCategory = Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221MagentoCategoryID_DEV000221MagentoWebSiteID_DEV000221(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, Me.cbeSelectMagentoInstance.EditValue.ToString, CInt(rowTreeList("SourceCategoryID")), -1) ' TJS 19/09/13
                If CBool(e.Value) Then
                    If rowMagentoCategory Is Nothing Then
                        rowMagentoCategory = Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                        rowMagentoCategory.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        rowMagentoCategory.InstanceID_DEV000221 = Me.cbeSelectMagentoInstance.EditValue.ToString
                        rowMagentoCategory.MagentoCategoryID_DEV000221 = CInt(rowTreeList("SourceCategoryID"))
                        rowMagentoCategory.MagentoWebSiteID_DEV000221 = -1 ' TJS 19/09/13
                        rowMagentoCategory.IsActive_DEV000221 = True
                        Me.m_InventorySettingsDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory) ' TJS 24/02/12
                    Else
                        If Not rowMagentoCategory.IsActive_DEV000221 Then
                            rowMagentoCategory.IsActive_DEV000221 = True
                        End If
                    End If
                Else
                    If rowMagentoCategory IsNot Nothing AndAlso rowMagentoCategory.IsActive_DEV000221 Then
                        rowMagentoCategory.IsActive_DEV000221 = False
                    End If
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try

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
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/05/12 | TJS             | 2012.1.04 | Code removed as conflicts with use of DisplayedValue field
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'If e.Column.Name = "colPropertiesDisplayValue" Then
        '    Select Case Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagDataType_DEV000221").ToString
        '        Case "Text"
        '            e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagTextValue_DEV000221").ToString
        '            e.Handled = True

        '        Case "Memo"
        '            e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagMemoValue_DEV000221").ToString
        '            e.Handled = True

        '        Case "Date", "DateTime"
        '            e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagMemoValue_DEV000221").ToString
        '            e.Handled = True

        '        Case "Integer", "Numeric"
        '            e.DisplayText = Me.GridViewProperties.GetRowCellValue(e.RowHandle, "TagMemoValue_DEV000221").ToString
        '            e.Handled = True

        '    End Select
        'End If

    End Sub

    Private Sub GridViewProperties_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewProperties.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/05/12 | TJS             | 2012.1.04 | Modified to only update values if a change has occured
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento Attribute Value selection
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then
            Select Case Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text", "Boolean"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagTextValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagTextValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Memo"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagMemoValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagMemoValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Date", "DateTime"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagDateValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagDateValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Integer", "Numeric"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagNumericValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagNumericValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

            End Select
        End If

        If e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then

            Select Case Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSelectMagentoInstance.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit
                    End If

                Case "Memo"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSelectMagentoInstance.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeMemoEdit
                    End If

                Case "Date", "DateTime"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeDateEdit

                Case "Integer", "Numeric"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSelectMagentoInstance.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit
                    End If

                Case "Boolean"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSelectMagentoInstance.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeYesNoEdit
                    End If

            End Select
        End If

    End Sub
#End Region

#Region " MagentoPriceAsIS "
    Private Sub MagentoPriceAsIS(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPriceAsIS.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for price source selectors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkPriceAsIS.Checked Then
            Me.lblMagentoSellingPrice.Enabled = False
            Me.lblMagentoPriceSource.Enabled = True ' TJS 13/11/13
            Me.lblMagentoSpecialPriceSource.Enabled = True ' TJS 13/11/13
            Me.cbeMagentoPriceSource.Enabled = True ' TJS 13/11/13
            Me.cbeMagentoSpecialPriceSource.Enabled = True ' TJS 13/11/13
            Me.TextEditMagentoSellingPrice.Enabled = False
            If Me.cbeMagentoSpecialPriceSource.EditValue IsNot Nothing AndAlso Me.cbeMagentoSpecialPriceSource.EditValue.ToString = "N" Then ' TJS 13/11/13
                Me.lblSpecialPrice.Enabled = True ' TJS 13/11/13
                Me.lblSpecialFrom.Enabled = True ' TJS 13/11/13
                Me.lblSpecialTo.Enabled = True ' TJS 13/11/13
                Me.DateEditSpecialFrom.Enabled = True ' TJS 13/11/13
                Me.DateEditSpecialTo.Enabled = True ' TJS 13/11/13
                Me.TextEditSpecialPrice.Enabled = True ' TJS 13/11/13
            Else
                Me.lblSpecialPrice.Enabled = False ' TJS 13/11/13
                Me.lblSpecialFrom.Enabled = False ' TJS 13/11/13
                Me.lblSpecialTo.Enabled = False ' TJS 13/11/13
                Me.DateEditSpecialFrom.Enabled = False ' TJS 13/11/13
                Me.DateEditSpecialTo.Enabled = False ' TJS 13/11/13
                Me.TextEditSpecialPrice.Enabled = False ' TJS 13/11/13
            End If
        Else
            Me.lblMagentoSellingPrice.Enabled = True
            Me.lblSpecialPrice.Enabled = True ' TJS 13/11/13
            Me.lblSpecialFrom.Enabled = True ' TJS 13/11/13
            Me.lblSpecialTo.Enabled = True ' TJS 13/11/13
            Me.lblMagentoPriceSource.Enabled = False ' TJS 13/11/13
            Me.lblMagentoSpecialPriceSource.Enabled = False ' TJS 13/11/13
            Me.cbeMagentoPriceSource.Enabled = False ' TJS 13/11/13
            Me.cbeMagentoSpecialPriceSource.Enabled = False ' TJS 13/11/13
            Me.DateEditSpecialFrom.Enabled = True ' TJS 13/11/13
            Me.DateEditSpecialTo.Enabled = True ' TJS 13/11/13
            Me.TextEditMagentoSellingPrice.Enabled = True
            Me.TextEditSpecialPrice.Enabled = True ' TJS 13/11/13
        End If

    End Sub

    Private Sub cbeMagentoSpecialPriceSource_EditValueChanged(sender As Object, e As System.EventArgs) Handles cbeMagentoSpecialPriceSource.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.cbeMagentoSpecialPriceSource.EditValue.ToString = "N" Then
            Me.lblSpecialPrice.Enabled = True
            Me.lblSpecialFrom.Enabled = True
            Me.lblSpecialTo.Enabled = True
            Me.DateEditSpecialFrom.Enabled = True
            Me.DateEditSpecialTo.Enabled = True
            Me.TextEditSpecialPrice.Enabled = True
        Else
            Me.lblSpecialPrice.Enabled = False
            Me.lblSpecialFrom.Enabled = False
            Me.lblSpecialTo.Enabled = False
            Me.DateEditSpecialFrom.Enabled = False
            Me.DateEditSpecialTo.Enabled = False
            Me.TextEditSpecialPrice.Enabled = False
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
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION And Not MyBase.IsReadOnly Then ' TJS 13/11/13
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
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
            TextEditProductName.ForeColor = Color.Gray
        End If

    End Sub
#End Region

#Region " ProductShortDescriptionEdit "
    Private Sub MemoEditShortDescription_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemoEditShortDescription.Enter
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 13/03/13 | TJS             | 2013.1.02 | Corrected relevant control name
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        ' 20/11/13 | TJS             | 2013.4.00 | Modified to cater for Web Option link
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If (MemoEditShortDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Or MemoEditShortDescription.Text = INVENTORY_USE_WEB_OPTION_TAB_SUMMARY) And _
            Not MyBase.IsReadOnly Then ' TJS 13/11/13 TJS 20/11/13
            MemoEditShortDescription.Text = String.Empty
            MemoEditShortDescription.Tag = MemoEditShortDescription.Text ' TJS 20/11/13
            MemoEditShortDescription.ForeColor = Color.Black
        End If

    End Sub

    Private Sub MemoEditShortDescription_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemoEditShortDescription.Leave
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 13/03/13 | TJS             | 2013.1.02 | Corrected relevant control name
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        ' 20/11/13 | TJS             | 2013.4.00 | Modified to cater for Web Option link
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditShortDescription.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            If MemoEditShortDescription.Tag IsNot Nothing AndAlso MemoEditShortDescription.Tag.ToString <> "" Then
                MemoEditShortDescription.Text = MemoEditShortDescription.Tag.ToString ' TJS 20/11/13
            Else
                MemoEditShortDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
            End If
            MemoEditShortDescription.ForeColor = Color.Gray
        End If

    End Sub
#End Region

#Region " ProductDescriptionEdit "
    Private Sub MemoEditDescription_Enter(sender As Object, e As System.EventArgs) Handles MemoEditDescription.Enter
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If (MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Or MemoEditDescription.Text = INVENTORY_USE_WEB_OPTION_TAB_DESCRIPTION) And _
            Not MyBase.IsReadOnly Then
            MemoEditDescription.Text = String.Empty
            MemoEditDescription.Tag = MemoEditDescription.Text
            MemoEditDescription.ForeColor = Color.Black
        End If

    End Sub

    Private Sub MemoEditDescription_Leave(sender As Object, e As System.EventArgs) Handles MemoEditDescription.Leave
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            If MemoEditDescription.Tag IsNot Nothing AndAlso MemoEditDescription.Tag.ToString <> "" Then
                MemoEditDescription.Text = MemoEditDescription.Tag.ToString
                MemoEditDescription.ForeColor = Color.Gray
            End If
        End If

    End Sub
#End Region
#End Region

End Class
#End Region
