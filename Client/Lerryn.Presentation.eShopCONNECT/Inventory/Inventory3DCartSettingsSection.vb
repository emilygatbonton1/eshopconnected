' eShopCONNECT for Connected Business
' Module: Inventory3DCartSettingsSection.vb
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
' Updated 20 November 2013

Option Explicit On
Option Strict On

Imports Interprise.Framework.Base.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Inventory.Shared.Const
Imports Microsoft.VisualBasic
Imports System.Xml.Linq
Imports System.Xml.XPath

#Region " Inventory3DCartSettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.Inventory3DCartSettingsSection")> _
Public Class Inventory3DCartSettingsSection

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private WithEvents m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
    Private m_CategoryTable As System.Data.DataTable = Nothing
    Private m_TempDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_tempFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade

    Private WithEvents m_BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing
    Private bGet3DCartCategories As Boolean = False
    Private bCopyInstanceSettings As Boolean
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.Inventory3DCartSettingsSectionGateway
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
        Get
            Return Me.lblActivate.Visible And Me.PanelControlDummy.Visible
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.PanelControlDummy.BringToFront()
                Me.PanelControlDummy.Visible = True
                Me.PanelPublishOn3DCart.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOn3DCart.BringToFront()
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
        Get
            Return Me.lblDevelopment.Visible And Me.PanelControlDummy.Visible
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.PanelControlDummy.BringToFront()
                Me.PanelControlDummy.Visible = True
                Me.PanelPublishOn3DCart.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOn3DCart.BringToFront()
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Me.m_InventorySettingsDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        Me.m_InventorySettingsFacade = New Facade.ImportExport.ImportExportConfigFacade(Me.m_InventorySettingsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem

        ' is eShopCONNECT activated ?
        If Me.m_InventorySettingsFacade.IsActivated Then
            ' yes, is 3DCart connector activated ?
            If Me.m_InventorySettingsFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                ' yes, need to check if there is an Item record in case we are being configured in the User Role !
                If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Rows.Count > 0 Then

                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                        "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

                    XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(THREE_D_CART_SOURCE_CODE).ConfigSettings_DEV000221))
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_3DCART_LIST)
                    Coll = Me.cbeSelect3DCartStore.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_ACCOUNT_DISABLED).ToUpper <> "YES" Then
                            CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_STORE_ID))
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    If Coll.Count > 0 Then
                        ' has Item been saved yet (i.e. ItemCode is not [To be generated])
                        If Me.m_InventoryItemDataset.InventoryItem(0).ItemCode <> Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
                            ' yes, enable 3DCart details tab
                            Me.PanelPublishOn3DCart.Enabled = True
                            If Coll.Count = 1 Then
                                Me.cbeSelect3DCartStore.SelectedIndex = 0
                                ThreeDCartSiteSelected(Nothing, Nothing)
                            Else
                                AddHandler Me.CheckEditPublishOn3DCart.EditValueChanged, AddressOf PublishOn3DCartChange
                                AddHandler Me.cbeSelect3DCartStore.EditValueChanged, AddressOf ThreeDCartSiteSelected
                            End If

                        Else
                            Me.PanelPublishOn3DCart.Enabled = False
                            AddHandler Me.CheckEditPublishOn3DCart.EditValueChanged, AddressOf PublishOn3DCartChange
                            AddHandler Me.cbeSelect3DCartStore.EditValueChanged, AddressOf ThreeDCartSiteSelected
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

#Region " EnableDisable3DCartControls "
    Private Sub EnableDisable3DCartControls(ByVal Enable As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strMatrixItemCode As String, strTemp As String, strProperties() As String
        Dim bMatrixGroupItem As Boolean, bMatrixItem As Boolean, bTemp As Boolean
        Dim bShowMatrixGroupLabel As Boolean, bShowMatrixItemLabel As Boolean

        ' is item a Matrix Group Item or a Matrix Item ?
        If Me.m_InventoryItemDataset.InventoryItem.Count > 0 Then
            If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Group" Then
                bMatrixGroupItem = True
                bMatrixItem = False
                bShowMatrixGroupLabel = True
                bShowMatrixItemLabel = False
            ElseIf Me.m_InventoryItemDataset.InventoryItem(0).ItemType = "Matrix Item" Then
                bMatrixGroupItem = False
                bMatrixItem = True
                bShowMatrixGroupLabel = False
                bShowMatrixItemLabel = True
            Else
                bMatrixGroupItem = False
                bMatrixItem = False
                bShowMatrixGroupLabel = False
                bShowMatrixItemLabel = False
            End If

        Else
            bMatrixGroupItem = False
            bMatrixItem = False
            bShowMatrixGroupLabel = False
            bShowMatrixItemLabel = False
        End If

        ' are we displaying a Matrix Item ?
        If bMatrixItem Then
            ' yes, get ItemCode for Matrix Group Item
            strMatrixItemCode = Me.m_InventorySettingsFacade.GetField("ItemCode", "InventoryMatrixItem", "MatrixItemCode = '" & _
                Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'")
            strTemp = Me.m_InventorySettingsFacade.GetField("SourceIsGroupItem_DEV000221", "Inventory3DCartDetails_DEV000221", _
                "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND StoreID_DEV000221 = '" & _
                Me.cbeSelect3DCartStore.EditValue.ToString & "'")
            If Not String.IsNullOrEmpty(strTemp) AndAlso (strTemp.ToLower = "true" Or strTemp = "1") Then
                Me.LabelMatrixItem.Text = "3DCart only has a single" & vbCrLf & "product record for this" & vbCrLf & "Matrix Group so all 3DCart" & vbCrLf & "settings must be set on the" & vbCrLf & "Matrix Group Item."
                Enable = False
            Else
                ' now get Matrix Group Item properties
                strProperties = Me.m_InventorySettingsFacade.GetRow(New String() {"ProductName_DEV000221", "ProductDescription_DEV000221", _
                    "StoreID_DEV000221"}, "Inventory3DCartDetails_DEV000221", "ItemCode_DEV000221 = '" & strMatrixItemCode & "'", False)
                If strProperties IsNot Nothing Then
                    ' remove data bindings for relevant fields
                    Me.TextEditProductName.DataBindings.Clear()
                    Me.MemoEditTitle.DataBindings.Clear()
                    ' now display Matrix Group Items properties
                    Me.TextEditProductName.EditValue = strProperties(0)
                    Me.MemoEditTitle.EditValue = strProperties(1)

                    Me.TreeListCategories.OptionsBehavior.Editable = False
                    Me.TextEditProductName.Properties.ReadOnly = True
                End If
            End If
        End If

        ' these controls are enabled if Instance ID is set
        If Me.cbeSelect3DCartStore.SelectedIndex >= 0 Then
            Me.CheckEditPublishOn3DCart.Enabled = True
        Else
            Me.CheckEditPublishOn3DCart.Enabled = False
        End If

        bTemp = False
        If Enable AndAlso Me.CheckEditPublishOn3DCart.Checked Then
            bTemp = True
        End If
        Me.chkPriceAsIS.Enabled = bTemp
        Me.GridControlProperties.Enabled = bTemp
        Me.lblDescription.Enabled = bTemp
        Me.lblExtendedDesc.Enabled = bTemp
        Me.lbl3DCartAttributes.Enabled = bTemp
        Me.lblProductName.Enabled = bTemp
        Me.lblTitle.Enabled = bTemp
        Me.MemoEditDescription.Enabled = bTemp
        Me.MemoEditTitle.Enabled = bTemp
        Me.MemoEditExtendedDesc.Enabled = bTemp
        Me.TreeListCategories.Enabled = bTemp
        If bTemp Then
            If CBool(Me.chkPriceAsIS.EditValue) Then
                Me.lbl3DCartSellingPrice.Enabled = False
                Me.lbl3DCartPriceSource.Enabled = True
                Me.lbl3DCartSalePriceSource.Enabled = True
                Me.cbe3DCartPriceSource.Enabled = True
                Me.cbe3DCartSalePriceSource.Enabled = True
                If Me.cbe3DCartSalePriceSource.EditValue IsNot Nothing AndAlso Me.cbe3DCartSalePriceSource.EditValue.ToString = "N" Then
                    Me.lblSalePrice.Enabled = True
                    Me.TextEditSalePrice.Enabled = True
                Else
                    Me.lblSalePrice.Enabled = False
                    Me.TextEditSalePrice.Enabled = False
                End If
                Me.TextEdit3DCartSellingPrice.Enabled = False
            Else
                Me.lbl3DCartSellingPrice.Enabled = True
                Me.lblSalePrice.Enabled = True
                Me.lbl3DCartPriceSource.Enabled = False
                Me.lbl3DCartSalePriceSource.Enabled = False
                Me.cbe3DCartPriceSource.Enabled = False
                Me.cbe3DCartSalePriceSource.Enabled = False
                Me.TextEdit3DCartSellingPrice.Enabled = True
                Me.TextEditSalePrice.Enabled = True
            End If
        Else
            Me.lbl3DCartSellingPrice.Enabled = bTemp
            Me.lblSalePrice.Enabled = bTemp
            Me.lbl3DCartPriceSource.Enabled = bTemp
            Me.lbl3DCartSalePriceSource.Enabled = bTemp
            Me.TextEdit3DCartSellingPrice.Enabled = bTemp
            Me.TextEditSalePrice.Enabled = bTemp
            Me.cbe3DCartPriceSource.Enabled = bTemp
            Me.cbe3DCartSalePriceSource.Enabled = bTemp
        End If
        Me.TextEditProductName.Enabled = bTemp
        Me.colPropertiesDisplayValue.AppearanceHeader.ForeColor = Color.Gray
        ' display labels for Matrix Items and Matrix Group Items
        Me.LabelMatrixGroupItem.Visible = bShowMatrixGroupLabel
        Me.LabelMatrixItem.Visible = bShowMatrixItemLabel

        ' now set Product name and description text colour
        If Me.TextEditProductName.Enabled Then
            If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then
                TextEditProductName.ForeColor = Color.Gray
            Else
                TextEditProductName.ForeColor = Color.Black
            End If
        End If
        If Me.MemoEditDescription.Enabled Then
            If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ReturnValue As DialogResult ' TJS 02/12/11

        Try
            ' save any changes to 3DCart settings
            If Me.m_InventorySettingsFacade.UpdateDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                    "CreateInventory3DCartDetails_DEV000221", "UpdateInventory3DCartDetails_DEV000221", "DeleteInventory3DCartDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                    "CreateInventory3DCartTagDetails_DEV000221", "UpdateInventory3DCartTagDetails_DEV000221", "DeleteInventory3DCartTagDetails_DEV000221"}, _
                New String() {Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName, _
                    "CreateInventory3DCartCategories_DEV000221", "UpdateInventory3DCartCategories_DEV000221", "DeleteInventory3DCartCategories_DEV000221"}}, _
                    Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Inventory 3DCart Settings", False) Then
                ReturnValue = MyBase.BeforeUpdatePluginDataSet(confirm, clear, isUseCache)

            Else
                ReturnValue = DialogResult.Cancel
            End If
            ' setting error text on dataset doesn't cause control to display it so copy it
            If Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.Count > 0 Then
                Me.RadioGroupQtyPublishing.ErrorText = Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221(0).GetColumnError("QtyPublishingType_DEV000221")
            End If
            Return ReturnValue

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Function
#End Region

#Region " SetProposed3DCartPrice "
    Private Sub SetProposed3DCartPrice(ByVal ItemCode As String, ByVal CurrencyCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XML3DCartNode As XElement
        Dim XML3DCartNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strSQL As String, strTemp As String, bUpdatePrice As Boolean

        ' get config settings
        XMLConfig = XDocument.Parse(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(THREE_D_CART_SOURCE_CODE).ConfigSettings_DEV000221.Trim)
        XML3DCartNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_3DCart_LIST)
        ' check connector count is valid i.e. number of 3DCart settings is not more then the licence limit
        If Me.m_InventorySettingsFacade.GetXMLElementListCount(XML3DCartNodeList) <= Me.m_InventorySettingsFacade.ConnectorAccountLimit(THREE_D_CART_CONNECTOR_CODE) Then
            ' check each 3DCart record for current Store ID
            For Each XML3DCartNode In XML3DCartNodeList
                XMLTemp = XDocument.Parse(XML3DCartNode.ToString)
                ' have we found current Store ID ?
                If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_STORE_ID) = Me.cbeSelect3DCartStore.Text Then
                    ' yes, has 3DCart price been set ?
                    bUpdatePrice = False
                    If Me.TextEdit3DCartSellingPrice.Text <> "" Then
                        If CDec(Me.TextEdit3DCartSellingPrice.EditValue) = 0 Then
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
                            Me.TextEdit3DCartSellingPrice.EditValue = CDec(strTemp)
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)


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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim row3DCartTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.Inventory3DCartTagDetails_DEV000221Row
        Dim row3DCartTagDetailsToCopy As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.Inventory3DCartTagDetails_DEV000221Row
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim iLoop As Integer, bTagExists As Boolean

        If bGet3DCartCategories Then
            Me.TreeListCategories.DataSource = m_CategoryTable
            If bCopyInstanceSettings Then
                For Each CategoryRow As System.Data.DataRow In m_CategoryTable.Rows
                    If CategoryRow.Item("SourceCategoryID").ToString = m_TempDataset.Inventory3DCartCategories_DEV000221(0).ThreeDCartCategoryID_DEV000221.ToString Then
                        m_TempDataset.Inventory3DCartCategories_DEV000221(0).IsActive_DEV000221 = True
                    End If
                Next
            End If
            Me.TreeListCategories.ExpandAll()
            Me.TreeListCategories.Enabled = True
            Me.pnlGetCategoryProgress.Visible = False
            bGet3DCartCategories = False
        End If

        EnableDisable3DCartControls(True)
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.UndoChanges()

        Try
            Me.EndCurrentEdit(New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName})
            Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.RejectChanges()
            Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.RejectChanges()
            Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.RejectChanges()

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try

    End Sub
#End Region
#End Region

#Region " Events "
#Region " ThreeDCartSiteSelected "
    Private Sub ThreeDCartSiteSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim colNewColumn As DataColumn, strMatrixItemCode As String, strTemp As String
        Dim iLoop As Integer, iSelectedSiteIndex As Integer

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.cbeSelect3DCartStore.EditValueChanged, AddressOf ThreeDCartSiteSelected
            RemoveHandler Me.CheckEditPublishOn3DCart.EditValueChanged, AddressOf PublishOn3DCartChange
            If Me.cbeSelect3DCartStore.SelectedIndex >= 0 Then
                ' save Site Index ID
                iSelectedSiteIndex = Me.cbeSelect3DCartStore.SelectedIndex
                ' get 3DCart publishing details - are we displaying a Matrix Item
                If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Then
                    ' yes, get ItemCode for Matrix Group Item
                    strMatrixItemCode = Me.m_InventorySettingsFacade.GetField("ItemCode", "InventoryMatrixItem", "MatrixItemCode = '" & _
                        Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'")
                    strTemp = Me.m_InventorySettingsFacade.GetField("SourceIsGroupItem_DEV000221", "Inventory3DCartDetails_DEV000221", _
                        "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND StoreID_DEV000221 = '" & _
                        Me.cbeSelect3DCartStore.EditValue.ToString & "'")
                    If Not String.IsNullOrEmpty(strTemp) AndAlso strTemp.ToLower = "true" Or strTemp = "1" Then
                        ' Inventory3DCartDetails_DEV000221 etc are only held for Matrix Group
                        Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                                "ReadInventory3DCartDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName, _
                                "ReadInventory3DCartCategories_DEV000221", AT_ITEM_CODE, strMatrixItemCode, AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                                "ReadInventory3DCartTagDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)
                    Else
                        ' Inventory3DCartDetails_DEV000221 etc are held for Matrix Item
                        Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                                "ReadInventory3DCartDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName, _
                                "ReadInventory3DCartCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}, _
                            New String() {Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                                "ReadInventory3DCartTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)
                    End If
                Else
                    Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                            "ReadInventory3DCartDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName, _
                            "ReadInventory3DCartCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}, _
                        New String() {Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                            "ReadInventory3DCartTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                            AT_STORE_ID, Me.cbeSelect3DCartStore.EditValue.ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                End If

                Me.cbeSelect3DCartStore.SelectedIndex = iSelectedSiteIndex ' TJS 02/12/11
                ' copy tag properties to DisplayedValue field for display purposes
                For iLoop = 0 To Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.Count - 1
                    Select Case Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).TagDataType_DEV000221
                        Case "Text", "Boolean"
                            If Not Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).IsTagTextValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).TagTextValue_DEV000221
                            End If

                        Case "Memo"
                            If Not Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).IsTagMemoValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221
                            End If

                        Case "Date", "DateTime"
                            If Not Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).IsTagDateValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).TagDateValue_DEV000221.ToString
                            End If

                        Case "Integer", "Numeric"
                            If Not Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).IsTagNumericValue_DEV000221Null Then
                                Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).DisplayedValue = Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221(iLoop).TagNumericValue_DEV000221.ToString
                            End If

                    End Select
                Next
                ' now we have set DisplayedValue field, accept changes so we can tell if user changes anything
                Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.AcceptChanges()
                XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(THREE_D_CART_SOURCE_CODE).ConfigSettings_DEV000221))
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_3DCART_LIST)
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_STORE_ID) = Me.cbeSelect3DCartStore.EditValue.ToString Then
                        If m_BackgroundWorker IsNot Nothing Then
                            m_BackgroundWorker.CancelAsync()
                        End If
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

                        If Me.CheckEditPublishOn3DCart.Checked Then
                            Me.TreeListCategories.Enabled = False
                            Me.pnlGetCategoryProgress.Visible = True

                            ' get 3DCart Categories as background task
                            If m_BackgroundWorker Is Nothing Then
                                m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                                m_BackgroundWorker.WorkerSupportsCancellation = True
                                m_BackgroundWorker.WorkerReportsProgress = False
                            End If
                            bGet3DCartCategories = True
                            m_BackgroundWorker.RunWorkerAsync()
                        End If
                        Exit For
                    End If
                Next

                ' now enable Browse List controls etc
                If Me.CheckEditPublishOn3DCart.Checked And (m_BackgroundWorker Is Nothing OrElse Not m_BackgroundWorker.IsBusy) Then
                    EnableDisable3DCartControls(True)
                Else
                    EnableDisable3DCartControls(False)
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            If m_BackgroundWorker Is Nothing OrElse Not m_BackgroundWorker.IsBusy Then
                Cursor = Cursors.Default
            End If
            AddHandler Me.CheckEditPublishOn3DCart.EditValueChanged, AddressOf PublishOn3DCartChange
            AddHandler Me.cbeSelect3DCartStore.EditValueChanged, AddressOf ThreeDCartSiteSelected

        End Try

    End Sub
#End Region

#Region " PublishOn3DCartChange "
    Private Sub PublishOn3DCartChange(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim row3DCartDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.Inventory3DCartDetails_DEV000221Row
        Dim row3DCartCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.Inventory3DCartCategories_DEV000221Row
        Dim frmSelectForCopy As Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopyForm
        Dim strPublishedStores As String(), iLoop As Integer, iInstLoop As Integer
        Dim strStores As String()(), strMatrixItemCode As String, strTemp As String
        Dim bInstanceFound As Boolean

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.CheckEditPublishOn3DCart.EditValueChanged, AddressOf PublishOn3DCartChange
            If CheckEditPublishOn3DCart.EditValue.ToString <> "" Then
                If CBool(Me.CheckEditPublishOn3DCart.EditValue) Then
                    If Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.Rows.Count = 0 Then
                        ' do we have multiple Stores ?
                        bCopyInstanceSettings = False
                        If Me.cbeSelect3DCartStore.Properties.Items.Count > 1 Then
                            strStores = Me.m_InventorySettingsFacade.GetRows(New String() {"StoreID_DEV000221"}, "Inventory3DCartDetails_DEV000221", "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND Publish_DEV000221 = 1")

                            ReDim strPublishedStores(Me.cbeSelect3DCartStore.Properties.Items.Count - 1)
                            bInstanceFound = False
                            For iLoop = 0 To Me.cbeSelect3DCartStore.Properties.Items.Count - 1
                                For iInstLoop = 0 To strStores.Length - 1
                                    If strStores(iInstLoop)(0).ToString = Me.cbeSelect3DCartStore.Properties.Items(iLoop).ToString Then
                                        strPublishedStores(iLoop) = Me.cbeSelect3DCartStore.Properties.Items(iLoop).ToString
                                        bInstanceFound = True
                                    End If
                                Next
                            Next
                            If bInstanceFound Then
                                frmSelectForCopy = New Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopyForm
                                frmSelectForCopy.SourceCode = THREE_D_CART_SOURCE_CODE
                                frmSelectForCopy.PublishedInstances = strPublishedStores
                                If frmSelectForCopy.ShowDialog <> DialogResult.OK Then
                                    Me.CheckEditPublishOn3DCart.EditValue = False
                                    Return
                                Else
                                    If frmSelectForCopy.InstanceToCopy <> "" Then
                                        m_TempDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
                                        m_tempFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_TempDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)
                                        ' get 3DCart publishing details - are we displaying a Matrix Item
                                        If Me.m_InventoryItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Then
                                            ' yes, get ItemCode for Matrix Group Item
                                            strMatrixItemCode = Me.m_InventorySettingsFacade.GetField("ItemCode", "InventoryMatrixItem", "MatrixItemCode = '" & _
                                                Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'")
                                            strTemp = Me.m_InventorySettingsFacade.GetField("SourceIsGroupItem_DEV000221", "Inventory3DCartDetails_DEV000221", _
                                                "ItemCode_DEV000221 = '" & Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "' AND StoreID_DEV000221 = '" & _
                                                Me.cbeSelect3DCartStore.EditValue.ToString & "'")
                                            If Not String.IsNullOrEmpty(strTemp) AndAlso strTemp.ToLower = "true" Or strTemp = "1" Then
                                                ' Inventory3DCartDetails_DEV000221 etc are only held for Matrix Group
                                                m_tempFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                                                       "ReadInventory3DCartDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, _
                                                        AT_STORE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName, _
                                                        "ReadInventory3DCartCategories_DEV000221", AT_ITEM_CODE, strMatrixItemCode, _
                                                        AT_STORE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                                                        "ReadInventory3DCartTagDetails_DEV000221", AT_ITEM_CODE, strMatrixItemCode, _
                                                        AT_STORE_ID, frmSelectForCopy.InstanceToCopy}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                            Else
                                                ' Inventory3DCartDetails_DEV000221 are held for Matrix Item, but rest are for Matrix Group
                                                m_tempFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                                                        "ReadInventory3DCartDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                        AT_STORE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName, _
                                                        "ReadInventory3DCartCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                        AT_STORE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                    New String() {Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                                                        "ReadInventory3DCartTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                        AT_STORE_ID, frmSelectForCopy.InstanceToCopy}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                            End If
                                        Else
                                            m_tempFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.TableName, _
                                                    "ReadInventory3DCartDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                    AT_STORE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                New String() {Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.TableName, _
                                                    "ReadInventory3DCartCategories_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                    AT_STORE_ID, frmSelectForCopy.InstanceToCopy}, _
                                                New String() {Me.m_InventorySettingsDataset.Inventory3DCartTagDetails_DEV000221.TableName, _
                                                    "ReadInventory3DCartTagDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                                                    AT_STORE_ID, frmSelectForCopy.InstanceToCopy}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                        End If

                                        bCopyInstanceSettings = True
                                    End If
                                End If
                            End If
                        End If
                        RemoveHandler Me.cbeSelect3DCartStore.EditValueChanged, AddressOf ThreeDCartSiteSelected
                        row3DCartDetails = Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.NewInventory3DCartDetails_DEV000221Row
                        row3DCartDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        row3DCartDetails.Publish_DEV000221 = True
                        row3DCartDetails.SellingPrice_DEV000221 = 0
                        row3DCartDetails.StoreID_DEV000221 = Me.cbeSelect3DCartStore.EditValue.ToString
                        row3DCartDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION
                        row3DCartDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION
                        row3DCartDetails.TotalQtyWhenLastPublished_DEV000221 = 0
                        row3DCartDetails.QtyLastPublished_DEV000221 = 0
                        Me.m_InventorySettingsDataset.Inventory3DCartDetails_DEV000221.AddInventory3DCartDetails_DEV000221Row(row3DCartDetails)
                        SetProposed3DCartPrice(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing))

                        AddHandler Me.cbeSelect3DCartStore.EditValueChanged, AddressOf ThreeDCartSiteSelected
                    End If

                    Me.TreeListCategories.Enabled = False
                    Me.pnlGetCategoryProgress.Visible = True

                    ' get 3DCart Categories as background task
                    If m_BackgroundWorker Is Nothing Then
                        m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                        m_BackgroundWorker.WorkerSupportsCancellation = True
                        m_BackgroundWorker.WorkerReportsProgress = False
                    End If
                    bGet3DCartCategories = True
                    m_BackgroundWorker.RunWorkerAsync()

                Else
                    EnableDisable3DCartControls(False)
                End If
            Else
                EnableDisable3DCartControls(False)
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            If m_BackgroundWorker Is Nothing OrElse Not m_BackgroundWorker.IsBusy Then
                Cursor = Cursors.Default
            End If
            AddHandler Me.CheckEditPublishOn3DCart.EditValueChanged, AddressOf PublishOn3DCartChange

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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowTreeList As System.Data.DataRowView
        Dim row3DCartCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.Inventory3DCartCategories_DEV000221Row

        Try

            If e.Column.Name = "colTreeListCategoryActive" Then
                rowTreeList = DirectCast(Me.TreeListCategories.GetDataRecordByNode(e.Node), System.Data.DataRowView)
                row3DCartCategory = Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.FindByItemCode_DEV000221StoreID_DEV000221ThreeDCartCategoryID_DEV000221(Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, Me.cbeSelect3DCartStore.EditValue.ToString, CInt(rowTreeList("SourceCategoryID")))
                If CBool(e.Value) Then
                    If row3DCartCategory Is Nothing Then
                        row3DCartCategory = Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.NewInventory3DCartCategories_DEV000221Row
                        row3DCartCategory.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                        row3DCartCategory.StoreID_DEV000221 = Me.cbeSelect3DCartStore.EditValue.ToString
                        row3DCartCategory.ThreeDCartCategoryID_DEV000221 = CInt(rowTreeList("SourceCategoryID"))
                        row3DCartCategory.IsActive_DEV000221 = True
                        Me.m_InventorySettingsDataset.Inventory3DCartCategories_DEV000221.AddInventory3DCartCategories_DEV000221Row(row3DCartCategory)
                    Else
                        If Not row3DCartCategory.IsActive_DEV000221 Then
                            row3DCartCategory.IsActive_DEV000221 = True
                        End If
                    End If
                Else
                    If row3DCartCategory IsNot Nothing AndAlso row3DCartCategory.IsActive_DEV000221 Then
                        row3DCartCategory.IsActive_DEV000221 = False
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
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
    Private Sub GridViewProperties_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewProperties.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then
            Select Case Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text", "Boolean"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagTextValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagTextValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Memo"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagMemoValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagMemoValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Date", "DateTime"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagDateValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagDateValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Integer", "Numeric"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagNumericValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagNumericValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

            End Select
        End If

        If e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then

            Select Case Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text"
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and StoreID_DEV000221 = '" & Me.cbeSelect3DCartStore.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExport3DCartAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit
                    End If

                Case "Memo"
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and StoreID_DEV000221 = '" & Me.cbeSelect3DCartStore.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExport3DCartAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeMemoEdit
                    End If

                Case "Date", "DateTime"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeDateEdit

                Case "Integer", "Numeric"
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and StoreID_DEV000221 = '" & Me.cbeSelect3DCartStore.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExport3DCartAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit
                    End If

                Case "Boolean"
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and StoreID_DEV000221 = '" & Me.cbeSelect3DCartStore.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExport3DCartAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeYesNoEdit
                    End If

            End Select
        End If

    End Sub
#End Region

#Region " 3DCartPriceAsIS "
    Private Sub ThreeDCartPriceAsIS(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPriceAsIS.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkPriceAsIS.Checked Then
            Me.lbl3DCartSellingPrice.Enabled = False
            Me.lbl3DCartPriceSource.Enabled = True
            Me.lbl3DCartSalePriceSource.Enabled = True
            Me.cbe3DCartPriceSource.Enabled = True
            Me.cbe3DCartSalePriceSource.Enabled = True
            Me.TextEdit3DCartSellingPrice.Enabled = False
            If Me.cbe3DCartSalePriceSource.EditValue IsNot Nothing AndAlso Me.cbe3DCartSalePriceSource.EditValue.ToString = "N" Then
                Me.lblSalePrice.Enabled = True
                Me.TextEditSalePrice.Enabled = True
            Else
                Me.lblSalePrice.Enabled = False
                Me.TextEditSalePrice.Enabled = False
            End If
        Else
            Me.lbl3DCartSellingPrice.Enabled = True
            Me.lblSalePrice.Enabled = True
            Me.lbl3DCartPriceSource.Enabled = False
            Me.lbl3DCartSalePriceSource.Enabled = False
            Me.cbe3DCartPriceSource.Enabled = False
            Me.cbe3DCartSalePriceSource.Enabled = False
            Me.TextEdit3DCartSellingPrice.Enabled = True
            Me.TextEditSalePrice.Enabled = True
        End If

    End Sub

    Private Sub cbe3DCartSpecialPriceSource_EditValueChanged(sender As Object, e As System.EventArgs) Handles cbe3DCartSalePriceSource.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.cbe3DCartSalePriceSource.EditValue.ToString = "N" Then
            Me.lblSalePrice.Enabled = True
            Me.TextEditSalePrice.Enabled = True
        Else
            Me.lblSalePrice.Enabled = False
            Me.TextEditSalePrice.Enabled = False
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION And Not MyBase.IsReadOnly Then
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If TextEditProductName.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION And Not MyBase.IsReadOnly Then
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
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MemoEditDescription.Text.Trim.Length = 0 And Not MyBase.IsReadOnly Then
            MemoEditDescription.Text = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION
            MemoEditDescription.ForeColor = Color.Gray
        End If

    End Sub
#End Region
#End Region

End Class
#End Region
