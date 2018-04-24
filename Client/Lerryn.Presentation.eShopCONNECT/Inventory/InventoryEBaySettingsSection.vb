' eShopCONNECT for Connected Business
' Module: InventoryEBaySettingsSection.vb
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
Imports Interprise.Framework.Inventory.Shared.Const
Imports Microsoft.VisualBasic ' 24/02/12
Imports System.Xml.Linq
Imports System.Xml.XPath

#Region " InventoryEBaySettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.InventoryEBaySettingsSection")> _
Public Class InventoryEBaySettingsSection
	
#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private WithEvents m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
#End Region

#Region " Properties "
#Region " CurrentDataset "
	Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
		Get
			Return Me.InventoryEBaySettingsSectionGateway
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
                Me.PanelPublishOnEBay.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnEBay.BringToFront()
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
                Me.PanelPublishOnEBay.SendToBack()
            Else
                Me.PanelControlDummy.SendToBack()
                Me.PanelPublishOnEBay.BringToFront()
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

	Public Sub New(ByVal InventoryEBaySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
				   ByVal InventoryEBaySettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
		MyBase.New()		   
		
        Me.m_InventorySettingsDataset = InventoryEBaySettingsDataset
        Me.m_InventorySettingsFacade = InventoryEBaySettingsSectionFacade
		

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
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 24/02/12 | TJS             | 2011.2.08 | Code added for eBay publishing
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement ' TJS 24/02/12
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 24/02/12
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection ' TJS 24/02/12
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem ' TJS 24/02/12

        ' is eShopCONNECT activated ?
        If Me.m_InventorySettingsFacade.IsActivated Then
            ' yes, is eBay connector activated ?
            If Me.m_InventorySettingsFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then
                ' yes, need to check if there is an Item record in case we are being configured in the User Role !
                If Me.m_InventoryItemDataset IsNot Nothing AndAlso Me.m_InventoryItemDataset.InventoryItem.Rows.Count > 0 Then
                    ' start of code added TJS 24/02/12
                    Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
                        "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                    XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(EBAY_SOURCE_CODE).ConfigSettings_DEV000221))
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_EBAY_LIST)
                    Coll = Me.cbeSelectEBayCountry.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XmlNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ACCOUNT_DISABLED).ToUpper <> "YES" Then
                            If m_InventorySettingsFacade.GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID") <> "" Then
                                CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY), CInt(m_InventorySettingsFacade.GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID")))
                            Else
                                CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY), -1)
                            End If
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    If Coll.Count > 0 Then
                        ' has Item been saved yet (i.e. ItemCode is not [To be generated])
                        If Me.m_InventoryItemDataset.InventoryItem(0).ItemCode <> Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE Then
                            ' yes, enable eBay details tab
                            Me.PanelPublishOnEBay.Enabled = True
                            If Coll.Count = 1 Then
                                Me.cbeSelectEBayCountry.SelectedIndex = 0
                                EBayCountrySelected(Nothing, Nothing)
                            Else
                                AddHandler Me.CheckEditPublishOnEBay.EditValueChanged, AddressOf PublishOnEBayChange
                                AddHandler Me.cbeSelectEBayCountry.EditValueChanged, AddressOf EBayCountrySelected
                            End If

                        Else
                            Me.PanelPublishOnEBay.Enabled = False
                            AddHandler Me.CheckEditPublishOnEBay.EditValueChanged, AddressOf PublishOnEBayChange
                            AddHandler Me.cbeSelectEBayCountry.EditValueChanged, AddressOf EBayCountrySelected

                        End If
                        AddHandler Me.CheckEditPublishOnEBay.EditValueChanged, AddressOf PublishOnEBayChange
                        ' for now show development message to prevent user's trying non-functional code
                        ShowDevelopmentMessage = True

                    Else
                        ShowActivateMessage = True
                    End If
                    ' end of code added TJS 24/02/12
                Else
                    ' for now show development message to prevent user's trying non-functional code
                    ShowDevelopmentMessage = True
                End If
                Else
                    ' for now show development message to prevent user's trying non-functional code
                    ShowDevelopmentMessage = True
                End If
        Else
            ' for now show development message to prevent user's trying non-functional code
            ShowDevelopmentMessage = True
        End If

    End Sub
#End Region

#Region " EnableDisableEBayControls "
    Private Sub EnableDisableEBayControls(ByVal Enable As Boolean)
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
        Dim bEBaySiteSet As Boolean, bMatrixGroupItem As Boolean, bMatrixItem As Boolean

        ' has an eBay Details record been created ?
        If Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221.Rows.Count > 0 Then
            ' yes, has eBay Site been set ?
            If Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221(0).CountryID_DEV000221 >= 0 Then
                bEBaySiteSet = True
            Else
                bEBaySiteSet = False
            End If
        Else
            bEBaySiteSet = False
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

        ' are we displaying a Matrix Item ?
        If bMatrixItem Then
            ' yes, get ItemCode for Matrix Group Item
            strTemp = Me.m_InventorySettingsFacade.GetField("SELECT ItemCode FROM dbo.InventoryMatrixItem WHERE MatrixItemCode = '" & _
                Me.m_InventoryItemDataset.InventoryItem(0).ItemCode & "'", CommandType.Text, Nothing)
            ' now get Matrix Group Item properties
            strProperties = Me.m_InventorySettingsFacade.GetRow(New String() {"ProductName_DEV000221", "ProductDescription_DEV000221", _
                "SiteCode_DEV000221"}, "InventoryEBayDetails_DEV000221", "ItemCode_DEV000221 = '" & strTemp & "'", False)
            If strProperties IsNot Nothing Then
                ' remove data bindings for relevant fields
                Me.TextEditProductName.DataBindings.Clear()
                Me.MemoEditDescription.DataBindings.Clear()
                ' now display Matrix Group Items properties
                Me.TextEditProductName.EditValue = strProperties(0)
                Me.MemoEditDescription.EditValue = strProperties(1)
            End If

        End If

        ' these controls are enabled if Instance ID is set
        If Me.cbeSelectEBayCountry.SelectedIndex >= 0 Then
            Me.CheckEditPublishOnEBay.Enabled = True
        Else
            Me.CheckEditPublishOnEBay.Enabled = False
        End If

        If Me.CheckEditPublishOnEBay.Checked Then
            Me.btnWizard.Enabled = True
        Else
            Me.btnWizard.Enabled = False
        End If

        ' display labels for Matrix Items and Matrix Group Items
        Me.LabelMatrixGroupItem.Visible = bMatrixGroupItem And Enable
        Me.LabelMatrixItem.Visible = bMatrixItem And Enable

        ' these controls are enabled if Browse Node is set

        ' now set Product name and description text colour
        If Me.TextEditProductName.Enabled Then
            If TextEditProductName.Text = INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
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

    End Sub
#End Region
#End Region

#Region " Events "
#Region " EBayCountrySelected "
    Private Sub EBayCountrySelected(ByVal sender As Object, ByVal e As System.EventArgs)
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

        Dim iSelectedSiteIndex As Integer

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.cbeSelectEBayCountry.EditValueChanged, AddressOf EBayCountrySelected
            RemoveHandler Me.CheckEditPublishOnEBay.EditValueChanged, AddressOf PublishOnEBayChange
            If Me.cbeSelectEBayCountry.SelectedIndex >= 0 Then
                ' save Site Index ID
                iSelectedSiteIndex = Me.cbeSelectEBayCountry.SelectedIndex
                ' get eBay publishing details 
                Me.m_InventorySettingsFacade.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221.TableName, _
                        "ReadInventoryEBayDetails_DEV000221", AT_ITEM_CODE, Me.m_InventoryItemDataset.InventoryItem(0).ItemCode, _
                        AT_COUNTRY_ID, Me.cbeSelectEBayCountry.Properties.Items(0).ImageIndex.ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific)

                Me.cbeSelectEBayCountry.SelectedIndex = iSelectedSiteIndex

                ' now enable Browse List controls etc
                If Me.CheckEditPublishOnEBay.Checked Then
                    EnableDisableEBayControls(True)
                Else
                    EnableDisableEBayControls(False)
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.CheckEditPublishOnEBay.EditValueChanged, AddressOf PublishOnEBayChange
            AddHandler Me.cbeSelectEBayCountry.EditValueChanged, AddressOf EBayCountrySelected

        End Try

    End Sub
#End Region

#Region " PublishOnEBayChange "
    Private Sub PublishOnEBayChange(ByVal sender As Object, ByVal e As System.EventArgs)
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

        Dim rowEBayDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryEBayDetails_DEV000221Row

        Try
            Cursor = Cursors.WaitCursor
            RemoveHandler Me.CheckEditPublishOnEBay.EditValueChanged, AddressOf PublishOnEBayChange
            If CBool(Me.CheckEditPublishOnEBay.EditValue) Then
                If Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221.Rows.Count = 0 Then
                    RemoveHandler Me.cbeSelectEBayCountry.EditValueChanged, AddressOf EBayCountrySelected
                    rowEBayDetails = Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221.NewInventoryEBayDetails_DEV000221Row
                    rowEBayDetails.ItemCode_DEV000221 = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                    rowEBayDetails.Publish_DEV000221 = True
                    rowEBayDetails.CountryID_DEV000221 = CInt(Me.cbeSelectEBayCountry.Properties.Items(0).ImageIndex)
                    'rowEBayDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
                    'rowEBayDetails.TotalQtyWhenLastPublished_DEV00022 = 0
                    'rowEBayDetails.QtyLastPublished_DEV000221 = 0 
                    Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221.AddInventoryEBayDetails_DEV000221Row(rowEBayDetails)
                    AddHandler Me.cbeSelectEBayCountry.EditValueChanged, AddressOf EBayCountrySelected
                End If

                EnableDisableEBayControls(True)
            Else
                EnableDisableEBayControls(False)
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default
            AddHandler Me.CheckEditPublishOnEBay.EditValueChanged, AddressOf PublishOnEBayChange

        End Try

    End Sub
#End Region
#End Region

    Private Sub btnWizard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWizard.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim frmEbayWiz As eBayWizard.eBayPublishingWizardForm
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)

        frmEbayWiz = New eBayWizard.eBayPublishingWizardForm(m_InventorySettingsDataset, m_InventorySettingsFacade)
        Me.LoadDataSet(New String()() {New String() {Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.TableName, _
            "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        XMLConfig = XDocument.Parse(Trim(Me.m_InventorySettingsDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(EBAY_SOURCE_CODE).ConfigSettings_DEV000221))
        XmlNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_EBAY_LIST)
        For Each XMLNode In XMLNodeList
            XMLTemp = XDocument.Parse(XMLNode.ToString)
            If m_InventorySettingsFacade.GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID") <> "" AndAlso _
                m_InventorySettingsFacade.GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID") = Me.cbeSelectEBayCountry.Properties.Items(0).ImageIndex.ToString Then
                If m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_IS_ITEM_ID_FIELD) = "ItemCode" Then
                    frmEbayWiz.SKUToPublish = Me.m_InventoryItemDataset.InventoryItem(0).ItemCode
                Else
                    frmEbayWiz.SKUToPublish = Me.m_InventoryItemDataset.InventoryItem(0).ItemName
                End If
                frmEbayWiz.eBayAuthToken = m_InventorySettingsFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN)
                If m_InventorySettingsFacade.GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID") <> "" Then
                    frmEbayWiz.eBayCountryID = CInt(m_InventorySettingsFacade.GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID"))
                Else
                    frmEbayWiz.eBayCountryID = -1
                End If
                Exit For
            End If
        Next
        'If Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221(0).ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  Then ' TJS 13/11/13
        ' frmEbayWiz.DescriptionToPublish = Me.m_InventorySettingsDataset.InventoryEBayDetails_DEV000221(0).ProductName_DEV000221
        'Else
        frmEbayWiz.DescriptionToPublish = Me.m_InventoryItemDataset.InventoryItemDescription(0).ItemDescription
        'End If
        frmEbayWiz.Show()


    End Sub
End Class
#End Region

