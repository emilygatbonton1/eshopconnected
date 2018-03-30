' eShopCONNECT for Connected Business
' Module: InventoryChannelAdvSettingsSection.vb
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
' Updated 13 November 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Inventory.Shared.Const
Imports Microsoft.VisualBasic
Imports System.Xml.Linq
Imports System.Xml.XPath

#Region " InventoryChannelAdvSettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.InventoryChannelAdvSettingsSection")> _
Public Class InventoryChannelAdvSettingsSection

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private WithEvents m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade ' TJS 27/06/09
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.InventoryChannelAdvSettingsSectionGateway
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
    Public Property ShowActivateMessage() As Boolean
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

        Get
            Return Me.lblActivate.Visible And Me.PanelControlDummy.Visible
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.PanelControlDummy.BringToFront()
                Me.PanelControlDummy.Visible = True
                Me.PanelPublishOnChannelAdv.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnChannelAdv.BringToFront()
            End If
            Me.lblActivate.Visible = value
            If value Then
                Me.lblDevelopment.Visible = False
            End If
        End Set
    End Property
#End Region

#Region " ShowDevelopmentMessage "
    Public Property ShowDevelopmentMessage() As Boolean
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

        Get
            Return Me.lblDevelopment.Visible And Me.PanelControlDummy.Visible
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.PanelControlDummy.BringToFront()
                Me.PanelControlDummy.Visible = True
                Me.PanelPublishOnChannelAdv.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnChannelAdv.BringToFront()
            End If
            Me.lblDevelopment.Visible = value
            If value Then
                Me.lblActivate.Visible = False
            End If
        End Set
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
        InitialiseControls()

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
        ' 02/12/11 | TJS             | 2011.2.00 | Code added
        ' 10/06/12 | TJS/FA          | 2012.1.05 | Modified to enable Browse List controls etc
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

        ' is eShopCONNECT activated ?
        If Me.m_InventorySettingsFacade.IsActivated Then
            ' yes, is Magento connector activated ?
            If Me.m_InventorySettingsFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                ' yes, need to check if there is an Item record in case we are being configured in the User Role !
                If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Rows.Count > 0 Then

                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                        "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                    XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(CHANNEL_ADVISOR_SOURCE_CODE).ConfigSettings_DEV000221))
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)
                    Coll = Me.cbeSelectChannelAdvAccountID.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_DISABLED).ToUpper <> "YES" Then
                            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME), Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID))
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    If Coll.Count > 0 Then
                        ' has Item been saved yet (i.e. ItemCode is not [To be generated])
                        If Me.m_InventoryItemDataset.InventoryItem(0).ItemCode <> Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
                            ' yes, enable Magento details tab
                            Me.PanelPublishOnChannelAdv.Enabled = True
                            If Coll.Count = 1 Then
                                Me.cbeSelectChannelAdvAccountID.SelectedIndex = 0
                                ChannelAdvSiteSelected(Nothing, Nothing)
                            Else
                                AddHandler Me.CheckEditPublishOnChannelAdv.EditValueChanged, AddressOf PublishOnChannelAdvChange
                                AddHandler Me.cbeSelectChannelAdvAccountID.EditValueChanged, AddressOf ChannelAdvSiteSelected
                            End If

                            ' FAgar 21/05/12 these controls are enabled if Account ID is set
                            ' now enable Browse List controls etc
                            If Me.cbeSelectChannelAdvAccountID.SelectedIndex >= 0 Then
                                EnableDisableChannelAdvControls(True)
                            Else
                                EnableDisableChannelAdvControls(False)
                            End If
                        Else
                            Me.PanelPublishOnChannelAdv.Enabled = False
                            AddHandler Me.CheckEditPublishOnChannelAdv.EditValueChanged, AddressOf PublishOnChannelAdvChange
                            AddHandler Me.cbeSelectChannelAdvAccountID.EditValueChanged, AddressOf ChannelAdvSiteSelected
                        End If
                    Else
                        ShowActivateMessage = True
                    End If
                Else
                    ShowActivateMessage = True
                End If
            Else
                ShowActivateMessage = True
            End If
        Else
            ShowActivateMessage = True
        End If

    End Sub
#End Region

#Region " EnableDisableChannelAdvControls "
    Private Sub EnableDisableChannelAdvControls(ByVal Enable As Boolean)
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

        Dim strTemp As String, strProperties() As String
        Dim bMatrixGroupItem As Boolean, bMatrixItem As Boolean, bTemp As Boolean

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
                "AccountID_DEV000221"}, "InventoryChannelAdvDetails_DEV000221", "ItemCode_DEV000221 = '" & strTemp & "'", False)
            If strProperties IsNot Nothing Then
                ' remove data bindings for relevant fields
                Me.TextEditProductTitle.DataBindings.Clear()
                Me.MemoEditShortDescription.DataBindings.Clear()
                ' now display Matrix Group Items properties
                Me.TextEditProductTitle.EditValue = strProperties(0)
                Me.MemoEditDescription.EditValue = strProperties(1)
            End If
        End If

        ' these controls are enabled if Account ID is set
        If Me.cbeSelectChannelAdvAccountID.SelectedIndex >= 0 Then
            Me.CheckEditPublishOnChannelAdv.Enabled = True
        Else
            Me.CheckEditPublishOnChannelAdv.Enabled = False
        End If

        bTemp = False
        If Enable AndAlso Me.CheckEditPublishOnChannelAdv.Checked Then
            bTemp = True
        End If
        Me.chkPriceAsIS.Enabled = bTemp
        Me.GridControlProperties.Enabled = bTemp
        Me.GridControlASIN.Enabled = bTemp
        Me.lblDescription.Enabled = bTemp
        Me.lblSubTitle.Enabled = bTemp
        Me.lblShortDescription.Enabled = bTemp
        Me.lblChannelAdvAttributes.Enabled = bTemp
        Me.lblChannelAdvSellingPrice.Enabled = bTemp
        Me.lblProductTitle.Enabled = bTemp
        Me.lblShortDescription.Enabled = bTemp
        Me.MemoEditDescription.Enabled = bTemp
        Me.MemoEditShortDescription.Enabled = bTemp
        Me.TextEditProductTitle.Enabled = bTemp
        Me.TextEditSubTitle.Enabled = bTemp
        Me.TextEditChannelAdvSellingPrice.Enabled = bTemp
        Me.TextEditProductTitle.Enabled = bTemp
        ' display labels for Matrix Items and Matrix Group Items
        Me.LabelMatrixGroupItem.Visible = bMatrixGroupItem And Enable
        Me.LabelMatrixItem.Visible = bMatrixItem And Enable

        ' now set Product name and description text colour
        If Me.TextEditProductTitle.Enabled Then
            If TextEditProductTitle.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
                TextEditProductTitle.ForeColor = Color.Gray
            Else
                TextEditProductTitle.ForeColor = Color.Black
            End If
        End If
        If Me.MemoEditDescription.Enabled Then
            If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then ' TJS 13/11/13
                MemoEditDescription.ForeColor = Color.Gray
            Else
                MemoEditDescription.ForeColor = Color.Black
            End If
        End If

        Me.lblQtyPublishing.Enabled = bTemp
        Me.RadioGroupQtyPublishing.Enabled = bTemp
        If Me.RadioGroupQtyPublishing.SelectedIndex >= 0 And bTemp Then
            QtyPublishingTypeChanged(Me, Nothing)
        Else
            Me.TextEditQtyPublishingValue.Enabled = False
            Me.lblQtyPublishingValue.Enabled = False
            Me.lblQtyPublishingValue.Text = ""
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
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ReturnValue As DialogResult

        Try
            ' save any changes to Channel Advisor settings
            If Me.m_InventorySettingsFacade.UpdateDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryChannelAdvDetails_DEV000221.TableName, _
                    "CreateInventoryChannelAdvDetails_DEV000221", "UpdateInventoryChannelAdvDetails_DEV000221", "DeleteInventoryChannelAdvDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryChannelAdvTagDetails_DEV000221.TableName, _
                    "CreateInventoryChannelAdvTagDetails_DEV000221", "UpdateInventoryChannelAdvTagDetails_DEV000221", "DeleteInventoryChannelAdvTagDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.InventoryAmazonASIN_DEV000221.TableName, _
                    "CreateInventoryAmazonASIN_DEV000221", "UpdateInventoryAmazonASIN_DEV000221", "DeleteInventoryAmazonASIN_DEV000221"}}, _
                    Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Inventory Channel Advisor Settings", False) Then
                ReturnValue = MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache)

            Else
                ReturnValue = DialogResult.Cancel
            End If
            ' setting error text on dataset doesn't cause control to display it so copy it
            If Me.m_InventorySettingsDataset.InventoryChannelAdvDetails_DEV000221.Count > 0 Then
                Me.RadioGroupQtyPublishing.ErrorText = Me.m_InventorySettingsDataset.InventoryChannelAdvDetails_DEV000221(0).GetColumnError("QtyPublishingType_DEV000221")
            End If
            Return ReturnValue

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Function
#End Region

#Region " SetProposedChannelAdvPrice "
    Private Sub SetProposedChannelAdvPrice(ByVal ItemCode As String, ByVal CurrencyCode As String)
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

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLChannelAdvNode As XElement
        Dim XMLChannelAdvNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strSQL As String, strTemp As String, bUpdatePrice As Boolean

        ' get config settings
        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(CHANNEL_ADVISOR_SOURCE_CODE).ConfigSettings_DEV000221.Trim)
        XMLChannelAdvNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)
        ' check connector count is valid i.e. number of Amazon settings is not more then the licence limit
        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XMLChannelAdvNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
            ' check each Magento record for current Instance ID
            For Each XMLChannelAdvNode In XMLChannelAdvNodeList
                XMLTemp = XDocument.Parse(XMLChannelAdvNode.ToString)
                ' have we found current Instance ID ?
                If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) = Me.cbeSelectChannelAdvAccountID.Text Then
                    ' yes, has Magento price been set ?
                    bUpdatePrice = False
                    If Me.TextEditChannelAdvSellingPrice.Text <> "" Then
                        If CDec(Me.TextEditChannelAdvSellingPrice.EditValue) = 0 Then
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
                            Me.TextEditChannelAdvSellingPrice.EditValue = CDec(strTemp)
                        End If
                    End If
                End If
                Exit For
            Next
        End If

    End Sub
#End Region
#End Region

#Region " Events "
#Region " ChannelAdvSiteSelected "
    Private Sub ChannelAdvSiteSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/06/12 | FA/TJS          | 2012.1.05 | Set cursor to default at end of function
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iSelectedSiteIndex As Integer

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.cbeSelectChannelAdvAccountID.EditValueChanged, AddressOf ChannelAdvSiteSelected
            RemoveHandler Me.CheckEditPublishOnChannelAdv.EditValueChanged, AddressOf PublishOnChannelAdvChange
            If Me.cbeSelectChannelAdvAccountID.SelectedIndex >= 0 Then
                ' save Site Index ID
                iSelectedSiteIndex = Me.cbeSelectChannelAdvAccountID.SelectedIndex
                ' get Channel Advisor publishing details 
                Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryChannelAdvDetails_DEV000221.TableName, _
                        "ReadInventoryChannelAdvDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                        AT_ACCOUNT_ID, Me.cbeSelectChannelAdvAccountID.EditValue.ToString}, _
                    New String() {Me.m_InventorySettingsDataset.InventoryChannelAdvTagDetails_DEV000221.TableName, _
                        "ReadInventoryChannelAdvTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                        AT_ACCOUNT_ID, Me.cbeSelectChannelAdvAccountID.EditValue.ToString}, _
                    New String() {Me.m_InventorySettingsDataset.InventoryAmazonASIN_DEV000221.TableName, _
                        "ReadInventoryAmazonASIN_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)

                Me.cbeSelectChannelAdvAccountID.SelectedIndex = iSelectedSiteIndex

                ' now enable Browse List controls etc
                If Me.CheckEditPublishOnChannelAdv.Checked Then
                    EnableDisableChannelAdvControls(True)
                Else
                    EnableDisableChannelAdvControls(False)
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            AddHandler Me.CheckEditPublishOnChannelAdv.EditValueChanged, AddressOf PublishOnChannelAdvChange
            AddHandler Me.cbeSelectChannelAdvAccountID.EditValueChanged, AddressOf ChannelAdvSiteSelected
            Cursor = Cursors.Default ' TJS/FA 10/06/12

        End Try

    End Sub
#End Region

#Region " PublishOnChannelAdvChange "
    Private Sub PublishOnChannelAdvChange(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/06/12 | FA/TJS          | 2012.1.05 | Set cursor to default at end of function.  Corrected 
        '                                          reference to Magento.  Enabled CA controls if Publish set to true
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowChannelAdvDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryChannelAdvDetails_DEV000221Row

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.CheckEditPublishOnChannelAdv.EditValueChanged, AddressOf PublishOnChannelAdvChange
            If CheckEditPublishOnChannelAdv.EditValue.ToString <> "" Then
                If CBool(Me.CheckEditPublishOnChannelAdv.EditValue) Then
                    If Me.m_InventorySettingsDataset.InventoryChannelAdvDetails_DEV000221.Rows.Count = 0 Then ' TJS/FA 10/06/12
                        RemoveHandler Me.cbeSelectChannelAdvAccountID.EditValueChanged, AddressOf ChannelAdvSiteSelected
                        rowChannelAdvDetails = Me.m_InventorySettingsDataset.InventoryChannelAdvDetails_DEV000221.NewInventoryChannelAdvDetails_DEV000221Row
                        rowChannelAdvDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        rowChannelAdvDetails.Publish_DEV000221 = True
                        rowChannelAdvDetails.SellingPrice_DEV000221 = 0
                        rowChannelAdvDetails.AccountID_DEV000221 = Me.cbeSelectChannelAdvAccountID.EditValue.ToString
                        rowChannelAdvDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
                        rowChannelAdvDetails.ProductShortDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
                        rowChannelAdvDetails.TotalQtyWhenLastPublished_DEV000221 = 0
                        rowChannelAdvDetails.QtyLastPublished_DEV000221 = 0
                        Me.m_InventorySettingsDataset.InventoryChannelAdvDetails_DEV000221.AddInventoryChannelAdvDetails_DEV000221Row(rowChannelAdvDetails)
                        SetProposedChannelAdvPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, "")
                        AddHandler Me.cbeSelectChannelAdvAccountID.EditValueChanged, AddressOf ChannelAdvSiteSelected
                    End If
                    EnableDisableChannelAdvControls(True) ' TJS/FA 10/06/12

                Else
                    EnableDisableChannelAdvControls(False)
                End If
            Else
                EnableDisableChannelAdvControls(False)
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            AddHandler Me.CheckEditPublishOnChannelAdv.EditValueChanged, AddressOf PublishOnChannelAdvChange
            Cursor = Cursors.Default ' TJS/FA 10/06/12

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

#Region " ProductNameEdit "
    Private Sub TextEditProductName_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditProductTitle.Enter
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

        If TextEditProductTitle.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION And Not MyBase.IsReadOnly Then ' TJS 13/11/13
            TextEditProductTitle.Text = String.Empty
            TextEditProductTitle.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextEditProductName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditProductTitle.Leave
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

        If TextEditProductTitle.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            TextEditProductTitle.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
            TextEditProductTitle.ForeColor = Color.Gray
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
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION And Not MyBase.IsReadOnly Then ' TJS 13/11/13
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
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
            MemoEditDescription.ForeColor = Color.Gray
        End If

    End Sub
#End Region
#End Region

End Class
#End Region
