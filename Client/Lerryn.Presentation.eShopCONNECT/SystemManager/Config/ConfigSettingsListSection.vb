' eShopCONNECT for Connected Business
' Module: ConfigSettingsListSection.vb
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
' Last Updated - 14 JAnuary 2014

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Licence
Imports Microsoft.VisualBasic
Imports System.Text ' TJS 19/08/10
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " ConfigSettingsListSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.Add, _
    "Lerryn.Presentation.ImportExport.ConfigSettingsListSection")> _
Public Class ConfigSettingsListSection

#Region " Variables "
    Private m_ConfigSettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private WithEvents m_GridListView As DevExpress.XtraGrid.Views.Grid.GridView
    Private bIgnorePageChangeEvent As Boolean
    Private bInhibitLoadDetails As Boolean
    Private bWebServiceRequired As Boolean ' TJS 02/12/11
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.ConfigSettingsSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_ConfigSettingsSectionFacade
        End Get
    End Property
#End Region

#Region " IsActivated "
    Public ReadOnly Property IsActivated() As Boolean
        Get
            If m_ConfigSettingsSectionFacade IsNot Nothing Then
                Return m_ConfigSettingsSectionFacade.IsActivated
            Else
                Return False
            End If
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
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 30/01/09 | TJS             | 2009.1.03 | Modified to use PRODUCT_CODE, PRODUCT_NAME when creating Facade
        ' 27/09/10 | TJS             | 2010.1.02 | REmoved duplicate instantiation of ConfigSettingsSectionGateway
        ' 21/03/11 | TJS             | 2011/0/02 | Changed data source so we can display Active flag and Expiry Date on list
        ' 20/04/12 | TJS             | 2012.1.02 | Modified to call CheckAndUpdateConfigSettings
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '------------------------------------------------------------------------------------------

        MyBase.New()

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        If System.ComponentModel.LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Return
        'Add any initialization after the InitializeComponent() call
        Me.m_ConfigSettingsSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.ConfigSettingsSectionGateway, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 30/01/09 TJS 10/06/12
        Me.m_ConfigSettingsSectionFacade.CheckAndUpdateConfigSettings() ' TJS 20/04/12
        Me.SystemFormType = FormType.ListDetail
        Me.Tablename = "DisplayedSourcesView_DEV000221" ' TJS 21/03/11
        Me.DisplayField = "SourceCode_DEV000221"
        Me.AllowDelete = True
        Me.bInhibitLoadDetails = False
        Me.bIgnorePageChangeEvent = False
    End Sub
#End Region

#Region " InitializeControl "
    Protected Overrides Sub InitializeControl()

        'This call is required by the Presentation Layer.
        MyBase.InitializeControl()

        'Add any initialization after the InitializeControl() call
        InitialiseControls()

    End Sub
#End Region

#Region " SetGridListViewHandle "
    Public Sub SetGridListViewHandle(ByRef BaseGridListView As DevExpress.XtraGrid.Views.Grid.GridView)

        m_GridListView = BaseGridListView

    End Sub
#End Region

#Region " InitialiseControls "
    Public Sub InitialiseControls()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to ensure List Control filter properly applied
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strShowIfActive As String()(), strFilter As String, strActive As String

        'Add any initialization after the InitializeControl() call
        strActive = ""
        strShowIfActive = Me.m_ConfigSettingsSectionFacade.GetRows(New String() {"SourceCode_DEV000221"}, "LerrynImportExportConfig_DEV000221", "ShowIfActivated_DEV000221 = 1")
        For Each Source As String() In strShowIfActive
            Select Case Source(0)
                Case "FileImport"
                    If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                        strActive = strActive & " or SourceCode_DEV000221 = '" & Source(0) & "'"
                    End If

                Case "ProspectLeadImport"
                    If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                        strActive = strActive & " or SourceCode_DEV000221 = '" & Source(0) & "'"
                    End If
            End Select
        Next
        If strActive <> "" Then
            strFilter = " and ((ShowIfActivated_DEV000221 = 0 " & strActive & ")"
        Else
            strFilter = " and (ShowIfActivated_DEV000221 = 0"
        End If
        If Me.m_ConfigSettingsSectionFacade.IsHostISeCommercePlus Then
            strActive = ""
            strShowIfActive = Me.m_ConfigSettingsSectionFacade.GetRows(New String() {"SourceCode_DEV000221"}, "LerrynImportExportConfig_DEV000221", "ShowIfActivated_DEV000221 = 1 AND NotIneCPlus_DEV000221 = 0")
            For Each Source As String() In strShowIfActive
                Select Case Source(0)
                    Case "AmazonOrder"
                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                            strActive = strActive & " or SourceCode_DEV000221 = '" & Source(0) & "'"
                        End If

                    Case "ChanAdvOrder"
                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                            strActive = strActive & " or SourceCode_DEV000221 = '" & Source(0) & "'"
                        End If

                    Case "ShopComOrder"
                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                            strActive = strActive & " or SourceCode_DEV000221 = '" & Source(0) & "'"
                        End If
                End Select
            Next
            If strActive <> "" Then
                strFilter = " and (NotIneCPlus_DEV000221 = 0 " & strActive & ")" & strFilter
            Else
                strFilter = " and NotIneCPlus_DEV000221 = 0" & strFilter
            End If
        Else
            strFilter = strFilter & " or NotIneCPlus_DEV000221 = 1"
        End If
        strFilter = strFilter & ")"

        Me.AdditionalFilter = strFilter 'Comment Original Code by: mark kee 6/13/2015
        'Me.AdditionalFilter = strFilter + Lerryn.Framework.ImportExport.Shared.CB_ADDSTRFILTER  ' Additional Code by mark kee 6/13/2015
        Me.listControl.AdditionalFilter = strFilter

    End Sub
#End Region

#Region " AddData "
    Public Overrides Sub AddData(Optional ByVal documentCode As String = "")
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        '------------------------------------------------------------------------------------------

        Dim result As DialogResult

        Try
            result = Me.UpdateDataSet(True, False, False)
            If result = DialogResult.No Then
                Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.RejectChanges()
            End If
            bInhibitLoadDetails = True
            If result <> DialogResult.Cancel Then
                Me.m_ConfigSettingsSectionFacade.NewSource()
            End If
            Me.IsNew = True
            Me.IsReadOnly = (Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count = 0)
            If Me.TabListDetail.SelectedTabPage.Name <> Me.TabPageDetail.Name Then
                Me.TabListDetail.SelectedTabPage = Me.TabPageDetail
            End If
            SetControlOptions()
            bInhibitLoadDetails = False

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Sub
#End Region

#Region " ShowNewForm "
    Public Overrides Sub ShowNewForm()
        Me.AddData()
    End Sub
#End Region

#Region " SetControlOptions "
    Private Sub SetControlOptions()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 20/01/09 | TJS             | 2009.1.00 | Modified to allow entry of new sources
        ' 30/01/09 | TJS             | 2009.1.03 | Modified to use PRODUCT_CODE when determining if default source
        ' 27/05/09 | TJS             | 2009.2.09 | Added check for XML errors
        ' 26/08/09 | TJS             | 2009.3.05 | modified to cater for EnableSourcePassword_DEV000221 on config settings
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for SKU ALias Loopup and to validate config settings when displayed
        '                                        | Also to handle passwords etc containing control characters
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for Payment Translation
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to only validate activated sources
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and modified to cater for ID attribute on some eBay settings
        ' 26/01/12 | TJS             | 2010.2.05 | Corrected enabling of EnablePaymentTypeTranslation and EnableSKUAliasLookup
        ' 10/06/12 | TJS             | 2012.1.05 | Modified for MAgento EnablePaymentTypeTranslation
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to use facade DecodeConfigXMLValue function for consistency
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector and EnableDisableOptionTabs function
        '------------------------------------------------------------------------------------------

        Dim XMLConfig As XDocument
        Dim XMLGroupNodeList As System.Collections.Generic.IEnumerable(Of XNode), XMLGroupNode As XElement
        Dim XMLConfigNodeList As System.Collections.Generic.IEnumerable(Of XNode), XMLConfigNode As XElement
        Dim rowConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim strProductCode As String, strGroupNodes(0, 0) As String, iGroupNodeSuffix As Integer
        Dim iLoop As Integer, iLoop2 As Integer, iGroupCount As Integer

        Me.TabPageDeliveryTranslation.PageVisible = False
        Me.TabPagePaymentTranslation.PageVisible = False ' TJS 22/09/10
        Me.TabPageSKUAliasLookup.PageVisible = False ' TJS 19/08/10
        Me.ConfigSettingsSectionGateway.XMLConfigSettings.Clear()
        Try
            XMLConfig = XDocument.Parse(Trim(Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))

            XMLGroupNodeList = XMLConfig.XPathSelectElement("eShopCONNECTConfig").Nodes
            ' get an array of the group node names
            iGroupCount = 0
            For Each XMLGroupNode In XMLGroupNodeList
                iGroupCount += 1
            Next
            ReDim strGroupNodes(iGroupCount - 1, 1)
            iLoop = 0
            For Each XMLGroupNode In XMLGroupNodeList
                strGroupNodes(iLoop, 0) = XMLGroupNode.Name.ToString
                strGroupNodes(iLoop, 1) = ""
                iLoop = iLoop + 1
            Next
            ' now check for repeats
            iGroupNodeSuffix = 0
            iLoop = 0
            For iLoop = 0 To CInt(strGroupNodes.Length / 2) - 1
                For iLoop2 = 0 To CInt(strGroupNodes.Length / 2) - 1
                    ' is it a duplicate
                    If strGroupNodes(iLoop, 0) = strGroupNodes(iLoop2, 0) And iLoop <> iLoop2 Then
                        ' yes, has suffix been set on first record ?
                        If strGroupNodes(iLoop, 1) = "" Then
                            ' no, set it
                            strGroupNodes(iLoop, 1) = " : " & Chr(Asc("A") + iGroupNodeSuffix)
                            iGroupNodeSuffix = iGroupNodeSuffix + 1
                        End If
                        ' has suffix been set on second record ?
                        If strGroupNodes(iLoop2, 1) = "" Then
                            ' no, set it
                            strGroupNodes(iLoop2, 1) = " : " & Chr(Asc("A") + iGroupNodeSuffix)
                            iGroupNodeSuffix = iGroupNodeSuffix + 1
                        End If
                    End If
                Next
            Next
            ' now populate grid
            iLoop = 0
            For Each XMLGroupNode In XMLGroupNodeList
                XMLConfigNodeList = XMLGroupNode.Nodes
                For Each XMLConfigNode In XMLConfigNodeList
                    rowConfig = Me.ConfigSettingsSectionGateway.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfig.ConfigGroup = strGroupNodes(iLoop, 0) & strGroupNodes(iLoop, 1)
                    rowConfig.ConfigSettingName = XMLConfigNode.Name.ToString
                    rowConfig.ConfigSettingValue = m_ConfigSettingsSectionFacade.DecodeConfigXMLValue(XMLConfigNode.Value) ' TJS 19/08/10 TJS 19/09/13
                    If XMLConfigNode.Attribute("ID") IsNot Nothing Then ' TJS 02/12/11
                        rowConfig.ConfigSettingID = XMLConfigNode.Attribute("ID").Value ' TJS 02/12/11
                    Else
                        rowConfig.ConfigSettingID = "" ' TJS 02/12/11
                    End If
                    Me.ConfigSettingsSectionGateway.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfig)
                    EnableDisableOptionTabs(XMLGroupNode, XMLConfigNode) ' TJS 20/11/13
                Next
                iLoop = iLoop + 1
            Next
            Me.ConfigSettingsSectionGateway.XMLConfigSettings.AcceptChanges()

            If Me.ConfigSettingsSectionGateway.XMLConfigSettings.Count > 0 Then
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).CurrentConfigGroup = Me.ConfigSettingsSectionGateway.XMLConfigSettings(0).ConfigGroup
            Else
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).CurrentConfigGroup = "General"
            End If

            strProductCode = Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221)
            If strProductCode = PRODUCT_CODE Then ' TJS 30/01/09
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LayoutItemActivationCode.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Else
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LayoutItemActivationCode.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            End If
            If Me.m_ConfigSettingsSectionFacade.IsSystemConfigRecord(Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221) Then ' TJS 20/01/09
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditSourceCode.Properties.ReadOnly = False
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditSourceName.Properties.ReadOnly = False
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.Properties.ReadOnly = False
                ' clear Input Handler list and populate with allowed options for user entered sources
                Coll = DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.Properties.Items
                Coll.BeginUpdate()
                Coll.Clear()
                If PRODUCT_CODE = ORDERIMPORTER_BASE_PRODUCT_CODE Then ' TJS 26/01/09
                    CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Generic XML Import", "Windows Service") ' TJS 26/01/09
                Else
                    CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Generic XML Web Import", "GenericXMLImport.ashx")
                End If
                Coll.Add(CollItem)
                Coll.EndUpdate()
            Else
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditSourceCode.Properties.ReadOnly = True
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditSourceName.Properties.ReadOnly = True
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.Properties.ReadOnly = True
                ' clear Input Handler list and populate with current value only
                Coll = DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.Properties.Items
                Coll.BeginUpdate()
                Coll.Clear()
                CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221, Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221)
                Coll.Add(CollItem)
                Coll.EndUpdate()
            End If
            If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).EnableSourcePassword_DEV000221 Then ' TJS 26/08/09
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditSourcePwd.Enabled = True ' TJS 26/08/09
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).btnChangePwd.Enabled = True ' TJS 26/08/09
            Else
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditSourcePwd.Enabled = False ' TJS 26/08/09
                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).btnChangePwd.Enabled = False ' TJS 26/08/09
            End If
            If DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled Then ' TJE 21/03/11
                m_ConfigSettingsSectionFacade.ValidateConfigSettings(XMLConfig, Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.EditValue.ToString)) ' TJS 19/08/10
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show("Error in Config XML - " & ex.Message & " in " & Trim(Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))

        End Try

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
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 18/03/11 | TJs             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to refresh list control data
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            If Me.AdditionalFilter <> "" Then ' TJS 18/03/11
                LoadDataSet = MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName, _
                   "ReadLerrynImportExportConfig_DEV000221", AT_ADDITIONAL_FILTER, Me.AdditionalFilter.Substring(5)}, New String() {Me.ConfigSettingsSectionGateway.LerrynLicences_DEV000221.TableName, _
                   "ReadLerrynLicences_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 18/03/11
            Else
                LoadDataSet = MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221"}, New String() {Me.ConfigSettingsSectionGateway.LerrynLicences_DEV000221.TableName, _
                    "ReadLerrynLicences_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            End If
            Me.IsReadOnly = (Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count = 0)
            Me.ReadList() ' TJS 02/12/11

            Return LoadDataSet

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Function
#End Region

#Region " UpdateDataSet "
    Public Overrides Function UpdateDataSet(Optional ByVal confirm As Boolean = False, Optional ByVal clear As Boolean = False, _
        Optional ByVal isUseCache As Boolean = False) As System.Windows.Forms.DialogResult
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 06/02/09 | TJS             | 2009.1.05 | Modified to ensure error message is reset after 
        '                                        | incorrect entry even if no internet connection
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to call Delivery Translation section reset new row
        ' 17/05/09 | TJS             | 2009.2.07 | Mofified to set Config Update Required flag if config changed
        ' 08/06/09 | TJS             | 2009.2.10 | Modified to prevent errors if config settings have null entries
        ' 24/07/09 | TJS             | 2009.3.03 | Modified to cater for ESHOPCONNECT_PROSPECT_IMPORT_CONNECTOR_CODE
        ' 18/02/10 | TJS             | 2010.0.05 | Modified to use a single TextEdit for the Activation Code 
        '                                        | with a Mask so that users can paste the Activation Code
        ' 26/04/10 | TJS             | 2010.0.06 | Corrected display of updated code
        ' 04/06/10 | TJs             | 2010.0.07 | Modified build of XML from config settings to ensure correct order of elements
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for SKU ALias Lookup and null setting values
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for Payment Translation
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for change to UpdateConnectorMaxAccounts
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and update 
        '                                        | the list control stored config settings after changes
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to show hourglass
        ' 07/05/13 | TJS             | 2013.1.13 | modified to rebuild XML Config settings BEFORE they are used for UpdateConnectorMaxAccounts etc
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221
        ' 02/10/13 | TJS             | 2013.3.03 | Modified to update display after saving
        '------------------------------------------------------------------------------------------

        Dim XMLConfig As XDocument, xmlRoot As XElement, xmlGroup As XElement, xmlElem As XElement, ListDataSource As System.Data.DataView ' TJS 02/12/11
        Dim iLoop As Integer, iErrorCode As Integer, strActivationCode As String, strUpdatedCode As String
        Dim strMessage As String, strTablesToCheck(0) As String, iColonPosn As Integer, bConfigChanged As Boolean ' TJS 17/05/09
        Dim strAccountGroup As String, strLastGroupName As String ' TJS 04/06/10

        Try
            Cursor = Cursors.WaitCursor ' TJS 14/02/12
            bConfigChanged = False ' TJS 17/05/09
            Me.EndCurrentEdit(New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName})
            If Me.TabListDetail.SelectedTabPage.Name = Me.TabPageDetail.Name Then
                strTablesToCheck(0) = "XMLConfigSettings"
                If Me.ConfigSettingsSectionGateway.HasChanges(strTablesToCheck) Then
                    bConfigChanged = True ' TJS 17/05/09
                    ' Start of code replaced from Tradepoint TJS 04/06/10
                    XMLConfig = New XDocument()
                    xmlRoot = New XElement("eShopCONNECTConfig")
                    ' first process the General group (checking for Account Group name as we go)
                    strAccountGroup = ""
                    xmlGroup = New XElement("General")
                    For iLoop = 0 To Me.ConfigSettingsSectionGateway.XMLConfigSettings.Count - 1
                        ' is row part of General group ?
                        If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup = "General" Then
                            ' yes, add if name not blank
                            If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName <> "" Then
                                If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).IsConfigSettingValueNull Then ' TJS 19/08/10
                                    xmlElem = New XElement(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName, "") ' TJS 19/08/10 TJS 02/12/11
                                Else
                                    xmlElem = New XElement(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName, EncodeConfigXMLValue(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingValue)) ' TJS 19/08/10 TJS 02/12/11
                                End If
                                If Not Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).IsConfigSettingIDNull AndAlso Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingID <> "" Then ' TJS 02/12/11
                                    xmlElem.SetAttributeValue("ID", Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingID) ' TJS 02/12/11
                                End If
                                xmlGroup.Add(xmlElem) ' TJS 02/12/11
                            End If

                        ElseIf strAccountGroup = "" Then
                            ' no and Account Group Name not saved so save it now
                            strAccountGroup = Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup
                        End If
                    Next
                    xmlRoot.Add(xmlGroup)

                    ' is there an Account Group ?
                    If strAccountGroup <> "" Then
                        ' yes - does name contain a suffix 
                        iColonPosn = strAccountGroup.IndexOf(" : ")
                        If iColonPosn >= 0 Then
                            ' yes, get Account name without suffix
                            strAccountGroup = strAccountGroup.Substring(0, iColonPosn).Trim()
                        End If
                        strLastGroupName = ""
                        xmlGroup = New XElement(strAccountGroup)
                        For iLoop = 0 To Me.ConfigSettingsSectionGateway.XMLConfigSettings.Count - 1
                            ' is row part of General group ?
                            If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup <> "General" Then
                                ' no, has group name changes from previous row ?
                                If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup <> strLastGroupName Then
                                    If strLastGroupName <> "" Then
                                        ' yes, add group to XML document
                                        xmlRoot.Add(xmlGroup)
                                        ' and start another group
                                        xmlGroup = New XElement(strAccountGroup)
                                    End If
                                    strLastGroupName = Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigGroup
                                End If
                                If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName <> "" Then
                                    If Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).IsConfigSettingValueNull Then ' TJS 19/08/10
                                        xmlElem = New XElement(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName, "") ' TJS 19/08/10 TJS 02/12/11
                                    Else
                                        xmlElem = New XElement(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingName, EncodeConfigXMLValue(Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingValue)) ' TJS 19/08/10 TJS 02/12/11
                                    End If
                                    If Not Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).IsConfigSettingIDNull AndAlso Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingID <> "" Then ' TJS 02/12/11
                                        xmlElem.SetAttributeValue("ID", Me.ConfigSettingsSectionGateway.XMLConfigSettings(iLoop).ConfigSettingID) ' TJS 02/12/11
                                    End If
                                    xmlGroup.Add(xmlElem) ' TJS 02/12/11
                                End If
                            End If
                        Next
                        xmlRoot.Add(xmlGroup)
                    End If
                    XMLConfig.Add(xmlRoot)
                    ' End of code replaced from Tradepoint TJS 04/06/10

                    If m_ConfigSettingsSectionFacade.ValidateConfigSettings(XMLConfig, Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.EditValue.ToString)) Then ' TJS 24/07/09
                        If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221 <> XMLConfig.ToString Then
                            Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221 = XMLConfig.ToString
                            ListDataSource = DirectCast(listControl.SearchGrid.DataSource, System.Data.DataView) ' TJS 02/12/11
                            For iLoop = 0 To ListDataSource.Count ' TJS 02/12/11
                                If ListDataSource(iLoop)("SourceCode_DEV000221").ToString = Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221 Then ' TJS 02/12/11
                                    ListDataSource(iLoop)("ConfigSettings_DEV000221") = XMLConfig.ToString ' TJS 02/12/11
                                    Exit For
                                End If
                            Next
                        End If
                    Else
                        Cursor = Cursors.Default ' TJS 14/02/12
                        Return DialogResult.No
                    End If
                End If

                ' start of code moved TJS 07/05/13
                If DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LayoutItemActivationCode.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always Then
                    strActivationCode = DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditInitialActivation1.Text.Replace("-", "") ' TJS 18/02/10
                    'strActivationCode = strActivationCode & Me.m_ConfigSettingsdetailSection.TextEditInitialActivation3.Text & Me.m_ConfigSettingsdetailSection.TextEditInitialActivation4.Text ' TJS 18/02/10
                    'strActivationCode = strActivationCode & Me.m_ConfigSettingsdetailSection.TextEditInitialActivation5.Text & Me.m_ConfigSettingsdetailSection.TextEditInitialActivation6.Text ' TJS 18/02/10
                    'strActivationCode = strActivationCode & Me.m_ConfigSettingsdetailSection.TextEditInitialActivation7.Text ' TJS 18/02/10
                    ' has Activation Code been entered ?
                    If strActivationCode.Length > 0 Then
                        ' yes, make sure company info is loaded and validate it
                        Me.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.SystemCompanyInformation.TableName, _
                            "ReadSystemCompanyInformation"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                        strUpdatedCode = ""
                        If Me.m_ConfigSettingsSectionFacade.UpdateDisplayedActivation(Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.EditValue.ToString), _
                            strActivationCode, Nothing, Nothing, "", iErrorCode, strUpdatedCode) Then ' TJS 18/03/11
                            ' licence code valid, update Finish message
                            ' has Activation Code changed ?
                            If strUpdatedCode <> "" Then
                                ' yes 
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditInitialActivation1.Text = strUpdatedCode.Substring(0, 5) & "-" & _
                                    strUpdatedCode.Substring(5, 5) & "-" & strUpdatedCode.Substring(10, 5) & "-" & _
                                    strUpdatedCode.Substring(15, 5) & "-" & strUpdatedCode.Substring(20, 5) & "-" & _
                                    strUpdatedCode.Substring(25, 5) & "-" & strUpdatedCode.Substring(30, 5) ' TJS 18/02/10 TJS 26/04/10
                                strMessage = "Your Activation Code has been updated to " & strUpdatedCode.Substring(0, 5)
                                strMessage = strMessage & "-" & strUpdatedCode.Substring(5, 5) & "-" & strUpdatedCode.Substring(10, 5) & "-"
                                strMessage = strMessage & strUpdatedCode.Substring(15, 5) & "-" & strUpdatedCode.Substring(20, 5) & "-"
                                strMessage = strMessage & strUpdatedCode.Substring(25, 5) & "-" & strUpdatedCode.Substring(30, 5) & vbCrLf
                                strMessage = strMessage & "and a copy has been sent to your registered email address."
                                strMessage = strMessage & vbCrLf & vbCrLf & "Your code expires on " & Me.m_ConfigSettingsSectionFacade.ConnectorActivationExpires(Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.EditValue.ToString)).ToShortDateString
                                Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage)
                            End If
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Text = "Activation expires " & Me.m_ConfigSettingsSectionFacade.ConnectorActivationExpires(Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.EditValue.ToString)).ToShortDateString ' TJS 04/02/09
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Appearance.ForeColor = System.Drawing.Color.Black ' TJS 04/02/09
                            If Not Me.m_ConfigSettingsSectionFacade.UpdateConnectorMaxAccounts(Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.EditValue.ToString), True) Then ' TJS 18/03/11
                                Interprise.Presentation.Base.Message.MessageWindow.Show("Your Activation Code only permits a maximum of " & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorAccountLimit(Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).ImageComboBoxEditInputHandler.EditValue.ToString)) & _
                                    " accounts and the config settings for one or more additional accounts have been removed.")
                            End If
                            SetControlOptions() ' TJS 02/10/13
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled = True
                        Else
                            ' licence code invalid, stop save
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Text = "Activation invalid - Error No " & iErrorCode.ToString
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Appearance.ForeColor = System.Drawing.Color.Red
                            Cursor = Cursors.Default ' TJS 14/02/12
                            Return DialogResult.Cancel
                        End If
                    End If
                End If
                ' end of code moved TJS 07/05/13
            End If
            strTablesToCheck(0) = "LerrynImportExportConfig_DEV000221"
            If Me.ConfigSettingsSectionGateway.HasChanges(strTablesToCheck) Then
                bConfigChanged = True ' TJS 17/05/09
                If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221 = SHOP_COM_SOURCE_CODE Then ' TJS 02/12/11
                    bWebServiceRequired = True ' TJS 02/12/11
                End If
            End If
            UpdateDataSet = MyBase.UpdateDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName, _
                "CreateLerrynImportExportConfig_DEV000221", "UpdateLerrynImportExportConfig_DEV000221", "DeleteLerrynImportExportConfig_DEV000221"}, _
                New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportDeliveryMethods_DEV000221.TableName, _
                "CreateLerrynImportExportDeliveryMethods_DEV000221", "UpdateLerrynImportExportDeliveryMethods_DEV000221", "DeleteLerrynImportExportDeliveryMethods_DEV000221"}, _
                New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportPaymentTypes_DEV000221.TableName, _
                "CreateLerrynImportExportPaymentTypes_DEV000221", "UpdateLerrynImportExportPaymentTypes_DEV000221", "DeleteLerrynImportExportPaymentTypes_DEV000221"}, _
                New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                "CreateLerrynImportExportSKUAliases_DEV000221", "UpdateLerrynImportExportSKUAliases_DEV000221", "DeleteLerrynImportExportSKUAliases_DEV000221"}, _
                New String() {Me.ConfigSettingsSectionGateway.SystemSource.TableName, "CreateSystemSource", "UpdateSystemSource", _
                "DeleteSystemSource"}}, confirm, False, False) ' TJS 19/08/10 TJS 22/09/10 TJS 03/07/13
            If bConfigChanged And UpdateDataSet = System.Windows.Forms.DialogResult.Yes Then
                Me.m_ConfigSettingsSectionFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET ConfigUpdateRequired_DEV000221 = 1", Nothing) ' TJS 17/05/09
                listControl_RowChanged(Me, Nothing) ' TJS 02/12/11
            End If
            If UpdateDataSet = System.Windows.Forms.DialogResult.Yes Then ' TJS 02/12/11
                Me.ConfigSettingsSectionGateway.XMLConfigSettings.AcceptChanges()
                If PluginContainerDeliveryTranslationPluginInstance IsNot Nothing Then ' TJS 17/03/09
                    DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).ResetNewRow() ' TJS 17/03/09
                End If
                If Me.PluginContainerPaymentTranslationPluginInstance IsNot Nothing Then ' TJS 22/09/10
                    DirectCast(Me.PluginContainerPaymentTranslationPluginInstance, ConfigSettingsPaymentTranslationSection).ResetNewRow() ' TJS 22/09/10 TJS 10/06/12
                End If
                If Me.PluginContainerSKUAliasLookupPluginInstance IsNot Nothing Then ' TJS 22/09/10
                    DirectCast(Me.PluginContainerSKUAliasLookupPluginInstance, ConfigSettingsSKUAliasLookupSection).ResetNewRow() ' TJS 22/09/10
                End If
                Me.IsReadOnly = (Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count = 0)
            End If
            Cursor = Cursors.Default ' TJS 14/02/12
            Return UpdateDataSet

        Catch ex As Exception
            Cursor = Cursors.Default ' TJS 14/02/12
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Function
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
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to call Delivery Translation section reset new row
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for SKU ALias Loopup
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for Payment Translation
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221
        '------------------------------------------------------------------------------------------

        Dim iLoop As Integer

        Try
            Me.EndCurrentEdit(New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName})
            Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.RejectChanges()
            Me.EndCurrentEdit(New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportDeliveryMethods_DEV000221.TableName})
            Me.ConfigSettingsSectionGateway.LerrynImportExportDeliveryMethods_DEV000221.RejectChanges()
            Me.EndCurrentEdit(New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportPaymentTypes_DEV000221.TableName}) ' TJS 22/09/10
            Me.ConfigSettingsSectionGateway.LerrynImportExportPaymentTypes_DEV000221.RejectChanges() ' TJS 22/09/10
            Me.EndCurrentEdit(New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportSKUAliasView_DEV000221.TableName}) ' TJS 19/08/10 TJS 03/07/13
            Me.ConfigSettingsSectionGateway.LerrynImportExportSKUAliasView_DEV000221.RejectChanges() ' TJS 19/08/10 TJS 03/07/13
            Me.EndCurrentEdit(New String() {Me.ConfigSettingsSectionGateway.XMLConfigSettings.TableName})
            Me.ConfigSettingsSectionGateway.XMLConfigSettings.RejectChanges()
            For iLoop = 0 To Me.ConfigSettingsSectionGateway.XMLConfigSettings.Count - 1
                Me.ConfigSettingsSectionGateway.XMLConfigSettings.Rows(iLoop).ClearErrors()
            Next
            If PluginContainerDeliveryTranslationPluginInstance IsNot Nothing Then ' TJS 17/03/09
                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).ResetNewRow() ' TJS 17/03/09
            End If
            Me.IsReadOnly = (Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count = 0)

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Sub
#End Region

#Region " EncodeConfigXMLValue "
    Private Function EncodeConfigXMLValue(ByVal XMLValue As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        '------------------------------------------------------------------------------------------

        Dim builder As New StringBuilder(), iLoop As Integer
        For iLoop = 1 To Len(XMLValue)
            If Asc(Mid(XMLValue, iLoop, 1)) < 32 Then
                builder.Append("%" & Asc(Mid(XMLValue, iLoop, 1)).ToString("X2"))
            ElseIf Mid(XMLValue, iLoop, 1) = "%" Then
                builder.Append("%25")
            Else
                builder.Append(Mid(XMLValue, iLoop, 1))
            End If
        Next
        Return builder.ToString

    End Function
#End Region

#Region " DisplayActivationStatus "
    Public Sub DisplayActivationStatus()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/03/11 | TJs             | 2011.0.02 | Function added
        ' 22/03/11 | TJS             | 2011.0.03 | Modified to update form name to reflect selected row
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to call listControl_RowChanged to initialise other tabs etc in IS 6
        '                                        | and for Channel Advisor Inventory Import
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strProductCode As String
        Dim iLoop As Integer

        For iLoop = 0 To Me.ListGridView.RowCount - 1
            strProductCode = Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(Me.ListGridView.GetRowCellValue(iLoop, Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.InputHandler_DEV000221Column.ColumnName).ToString)
            If strProductCode = PRODUCT_CODE Then
                If Me.m_ConfigSettingsSectionFacade.IsActivated Then
                    'Me.ListGridView.SetRowCellValue(iLoop, "Active", True) ' comment by mark kee 6/24/2015
                    Me.ListGridView.SetRowCellValue(iLoop, "Active", False) '
                End If
                If Me.m_ConfigSettingsSectionFacade.IsActivated Or Me.m_ConfigSettingsSectionFacade.HasBeenActivated Then
                    'Me.ListGridView.SetRowCellValue(iLoop, "ExpiryDate", Me.m_ConfigSettingsSectionFacade.ActivationExpires) ' comment by mark kee

                    'added code by mark jeson kee 6/25/2015
                    If Me.m_ConfigSettingsSectionFacade.ActivationExpires <> Date.Today.AddYears(-100) Then
                        Me.ListGridView.SetRowCellValue(iLoop, "ExpiryDate", Me.m_ConfigSettingsSectionFacade.ConnectorActivationExpires(strProductCode))
                    End If
                    'end code by mark kee 6/25/2015
                End If

            ElseIf (strProductCode = ASP_STORE_FRONT_CONNECTOR_CODE Or strProductCode = CHANNEL_ADVISOR_CONNECTOR_CODE Or strProductCode = MAGENTO_CONNECTOR_CODE) And _
                Not Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(strProductCode) Then ' TJS 02/12/11
                If Me.m_ConfigSettingsSectionFacade.IsActivated Then

                    'comment by mark kee 6/24/2015

                    'Me.ListGridView.SetRowCellValue(iLoop, "Active", True)
                    'Me.ListGridView.SetRowCellValue(iLoop, "InventoryImportOnly", True)
                    'Me.ListGridView.SetRowCellValue(iLoop, "ExpiryDate", Me.m_ConfigSettingsSectionFacade.ActivationExpires)

                    'end comment

                    'added code by mark kee 6/24/2015 
                    Me.ListGridView.SetRowCellValue(iLoop, "Active", False)
                    Me.ListGridView.SetRowCellValue(iLoop, "InventoryImportOnly", False)
                    If Me.m_ConfigSettingsSectionFacade.ConnectorActivationExpires(strProductCode) <> Date.Today.AddYears(-100) Then
                        Me.ListGridView.SetRowCellValue(iLoop, "ExpiryDate", Me.m_ConfigSettingsSectionFacade.ConnectorActivationExpires(strProductCode))
                    End If
                    'end code by mark kee
                End If

            Else
                If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(strProductCode) Then
                    Me.ListGridView.SetRowCellValue(iLoop, "Active", True)
                End If
                If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(strProductCode) Or Me.m_ConfigSettingsSectionFacade.HasConnectorBeenActivated(strProductCode) Then
                    Me.ListGridView.SetRowCellValue(iLoop, "ExpiryDate", Me.m_ConfigSettingsSectionFacade.ConnectorActivationExpires(strProductCode))
                End If
            End If
        Next
        Me.FindForm.Text = PRODUCT_NAME & " Configuration Settings" ' TJS 02/12/11
        listControl_RowChanged(Me, Nothing) ' TJS 02/12/11        

    End Sub
#End Region

#Region " CheckServicesOperational "
    Public Sub CheckServicesOperational()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    This function checks to see if the eShopCONNECT services appears to be installed and working  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 02/10/13 | TJS             | 2013.3.03 | Corrected layout of service not running message
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strTimerFields As String(), dteLastTick As Date, dteServerTime As Date

        Try
            ' get service last timer tick and server time from db
            strTimerFields = Me.m_ConfigSettingsSectionFacade.GetRow(New String() {"CONVERT(VARCHAR(19), LastTimerTick_DEV000221, 120)", "CONVERT(VARCHAR(19), LastWebPost_DEV000221, 120)", "CONVERT(VARCHAR(19), GETDATE(), 120)"}, "LerrynImportExportServiceAction_DEV000221", Nothing, False)

        Catch ex As Exception
            ' failed to get service timings 
            ReDim strTimerFields(2)

        End Try

        ' is there a service last timer tick value ?
        If "" & strTimerFields(0) = "" Then
            ' no, is Web Service needed and no web posts yet ?
            If bWebServiceRequired And "" & strTimerFields(1) = "" Then
                ' yes, warn about service not installed or not running
                If Interprise.Presentation.Base.Message.MessageWindow.Show("The eShopCONNECTED Windows Service does not appear to be installed, or" & vbCrLf & _
                    "else it has not been configured and started, and one or more of your" & vbCrLf & "active sources require the eShopCONNECTED Web Service to be" & vbCrLf & _
                    "installed and configured." & vbCrLf & vbCrLf & "If you have a hosted installation of " & IS_PRODUCT_NAME & ", please contact" & vbCrLf & _
                    "Interprise Support and ask them to check that the eShopCONNECTED" & vbCrLf & "Windows and Web Services are installed and configured properly." & vbCrLf & vbCrLf & _
                    "If you have an on-premise installation of " & IS_PRODUCT_NAME & ", click Yes for" & vbCrLf & "details of how to install, configure and start the eShopCONNECTED Windows" & _
                    vbCrLf & "and Web Services." & vbCrLf & vbCrLf & "NOTE:  If you have only just done the initial activation and configuration" & vbCrLf & _
                    "of eShopCONNECTED, it can take 15 - 20 minutes for the Windows Service to" & vbCrLf & "recognise the changes and complete it's initialization.", _
                    "eShopCONNECTED Windows Service not working", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, _
                    Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Yes Then
                    System.Diagnostics.Process.Start(PLUGINS_WEBSITE_URL & "/Plugins/eShopCONNECT/Help/ServicesInstallAndConfig.htm") ' TJS 24/08/12
                End If
            Else
                ' no, warn about service not installed or not running
                If Interprise.Presentation.Base.Message.MessageWindow.Show("The eShopCONNECTED Windows Service does not appear to be installed or" & vbCrLf & _
                    "else it has not been configured and started." & vbCrLf & vbCrLf & "If you have a hosted installation of " & IS_PRODUCT_NAME & ", please contact" & vbCrLf & _
                    "Interprise Support and ask them to check that the eShopCONNECTED" & vbCrLf & "Windows Service is installed and configured properly." & vbCrLf & vbCrLf & _
                    "If you have an on-premise installation of " & IS_PRODUCT_NAME & ", click Yes for" & vbCrLf & "details of how to install, configure and start the eShopCONNECTED " & _
                    vbCrLf & "Windows Service." & vbCrLf & vbCrLf & "NOTE:  If you have only just done the initial activation and configuration" & vbCrLf & _
                    "of eShopCONNECTED, it can take 15 - 20 minutes for the service to recognise" & vbCrLf & "the changes and complete it's initialization.", _
                    "eShopCONNECTED Windows Service not working", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, _
                    Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Yes Then
                    System.Diagnostics.Process.Start(PLUGINS_WEBSITE_URL & "/Plugins/eShopCONNECT/Help/ServicesInstallAndConfig.htm") ' TJS 24/08/12
                End If
            End If
            Return
        Else
            ' yes, is it less than 2 minutes earlier than server time
            dteLastTick = DateSerial(CInt(strTimerFields(0).Substring(0, 4)), CInt(strTimerFields(0).Substring(5, 2)), CInt(strTimerFields(0).Substring(8, 2)))
            dteLastTick = dteLastTick.AddHours(CDbl(strTimerFields(0).Substring(11, 2))).AddMinutes(CDbl(strTimerFields(0).Substring(14, 2))).AddSeconds(CDbl(strTimerFields(0).Substring(17, 2)))
            dteServerTime = DateSerial(CInt(strTimerFields(2).Substring(0, 4)), CInt(strTimerFields(2).Substring(5, 2)), CInt(strTimerFields(2).Substring(8, 2)))
            dteServerTime = dteServerTime.AddHours(CDbl(strTimerFields(2).Substring(11, 2))).AddMinutes(CDbl(strTimerFields(2).Substring(14, 2))).AddSeconds(CDbl(strTimerFields(2).Substring(17, 2)))
            If DateDiff(DateInterval.Minute, dteLastTick, dteServerTime) > 10 Then
                ' no, warn about service not running
                If Interprise.Presentation.Base.Message.MessageWindow.Show("Whilst the eShopCONNECTED Windows Service does appear to be installed and" & vbCrLf & _
                    "have been running previously, it does not appear to running at present." & vbCrLf & vbCrLf & "If you have a hosted installation of " & _
                     IS_PRODUCT_NAME & ", please contact" & vbCrLf & "Interprise Support and ask them to check that the eShopCONNECTED" & vbCrLf & _
                    "Windows Service is working properly." & vbCrLf & vbCrLf & "If you have an on-premise installation of " & IS_PRODUCT_NAME & ", click Yes for" & _
                    vbCrLf & "details of how to check the eShopCONNECTED Windows Service is" & vbCrLf & "working properly.", "eShopCONNECTED Windows Service not working", _
                    Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, _
                    Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Yes Then ' TJS 02/10/13
                    System.Diagnostics.Process.Start(PLUGINS_WEBSITE_URL & "/Plugins/eShopCONNECT/Help/WindowsServiceCheck.htm") ' TJS 24/08/12
                End If
                Return

            ElseIf bWebServiceRequired And "" & strTimerFields(1) = "" Then
                If Interprise.Presentation.Base.Message.MessageWindow.Show("One or more of your active sources require the eShopCONNECTED Web Service" & vbCrLf & _
                    "to be installed and configured" & vbCrLf & vbCrLf & "If you have a hosted installation of " & IS_PRODUCT_NAME & ", and you have not" & vbCrLf & _
                    "requested that eShopCONNECTED be installed, please contact Interprise" & vbCrLf & "Support and ask them to check that the eShopCONNECTED Windows" & vbCrLf & _
                    "and Web Services are installed and configured." & vbCrLf & vbCrLf & "If you have an on-premise installation of " & IS_PRODUCT_NAME & ", " & _
                    "click Yes for details of how to check install and configure the eShopCONNECTED Windows and Web Services.", "eShopCONNECTED Windows Service not working", _
                    Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, _
                    Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Yes Then
                    System.Diagnostics.Process.Start(PLUGINS_WEBSITE_URL & "/Plugins/eShopCONNECT/Help/ServicesInstallAndConfig.htm") ' TJS 24/08/12
                End If
                Return

            End If
        End If

    End Sub
#End Region

#Region " EnableDisableOptionTabs "
    Private Sub EnableDisableOptionTabs(ByVal XMLGroupNode As XElement, ByVal XMLConfigNode As XElement)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function created from duplicated code in listControl_RowChanged and SetControlOptions
        '                                        | and modified for 3DCart connector
        ' 15/04/14 | TJS             | 2013.4.05 | Modified for Volusion EnableSKUAliasLookup
        ' 11/02/14 | TJS             | 2013.4.09 | added Volusion EnablePaymentTypeTranslation
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If XMLConfigNode.Name.ToString = "EnableDeliveryMethodTranslation" And XMLGroupNode.Name.ToString = "General" Then
            If XMLConfigNode.Value.ToUpper = "YES" Then
                Me.TabPageDeliveryTranslation.PageVisible = True
                MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.SystemShippingMethodGroupDetail.TableName, _
                    "ReadSystemShippingMethodGroupDetailView_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            End If
        End If
        If XMLConfigNode.Name.ToString = "EnablePaymentTypeTranslation" And (Strings.Left(XMLGroupNode.Name.ToString, 14) = "ChannelAdvisor" Or _
            Strings.Left(XMLGroupNode.Name.ToString, 7) = "Magento" Or Strings.Left(XMLGroupNode.Name.ToString, 4) = "eBay" Or _
            Strings.Left(XMLGroupNode.Name.ToString, 10) = "ThreeDCart" Or Strings.Left(XMLGroupNode.Name.ToString, 8) = "Volusion") Then ' TJS 22/09/10 TJS 10/06/12 TJS 20/11/13 TJS 11/02/14
            If XMLConfigNode.Value.ToUpper = "YES" Then ' TJS 22/09/10
                Me.TabPagePaymentTranslation.PageVisible = True ' TJS 22/09/10
            End If
        End If
        If XMLConfigNode.Name.ToString = "EnableSKUAliasLookup" And (Strings.Left(XMLGroupNode.Name.ToString, 6) = "Amazon" Or _
            Strings.Left(XMLGroupNode.Name.ToString, 13) = "ASPStoreFront" Or Strings.Left(XMLGroupNode.Name.ToString, 14) = "ChannelAdvisor" Or _
            Strings.Left(XMLGroupNode.Name.ToString, 7) = "Magento" Or Strings.Left(XMLGroupNode.Name.ToString, 4) = "eBay" Or _
            Strings.Left(XMLGroupNode.Name.ToString, 10) = "ThreeDCart" Or Strings.Left(XMLGroupNode.Name.ToString, 8) = "Volusion") Then ' TJS 19/08/10 TJS 22/09/10 TJS 20/11/13 TJS 15/01/14
            If XMLConfigNode.Value.ToUpper = "YES" Then ' TJS 19/08/10
                Me.TabPageSKUAliasLookup.PageVisible = True ' TJS 19/08/10
            End If
        End If

    End Sub
#End Region
#End Region

#Region " Events "
#Region " TabListDetail_SelectedPageChanged "
    Public Overrides Sub TabListDetail_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 30/01/09 | TJS             | 2009.1.03 | Modified to use PRODUCT_CODE when determining if default source
        ' 30/12/09 | TJS             | 2010.0.00 | Modified to show/hide Source Delivery Class on Delivery Method Translation tab
        ' 18/02/10 | TJS             | 2010.0.05 | Modified to use a single TextEdit for the Activation Code 
        '                                        | with a Mask so that users can paste the Activation Code
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for SKU ALias Loopup
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for Payment Translation
        ' 27/09/10 | TJS             | 2010.1.02 | Modified to cater for Channel Advisor using a Combo box to display 
        '                                        | the Source Delivery Method and Class
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to cater for Inventory Import only activation
        ' 22/03/11 | TJS             | 2011.0.03 | Modified to update form name to reflect selected row
        ' 09/07/11 | TJS             | 2011.1.00 | Corrected error when selecting SKUAlias tab
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to ensure correct Activation code displayed when more than 1 in dataabse 
        '                                        | and modified for eBay Delivery Method translation
        ' 10/06/12 | TJS             | 2012.1.05 | Corrected casting of PaymentTranslation section instance
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to prevent errors during user role config
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to check if source activated before creating missing System Sources
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '------------------------------------------------------------------------------------------

        Dim rowSource As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemSourceRow
        Dim strProductCode As String, bCreateSource As Boolean ' TJS 09/08/13

        Try
            Cursor = Cursors.WaitCursor ' TJS 27/09/10
            MyBase.TabListDetail_SelectedPageChanged(sender, e)
            If Not bIgnorePageChangeEvent Then
                bIgnorePageChangeEvent = True
                Select Case e.Page.Name
                    Case Me.TabPageList.Name
                        If UpdateDataSet() = DialogResult.Yes Then
                            If Me.AdditionalFilter <> "" Then ' TJS 18/03/11
                                MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName, _
                                   "ReadLerrynImportExportConfig_DEV000221", AT_ADDITIONAL_FILTER, Me.AdditionalFilter.Substring(5)}}, _
                                   Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 18/03/11
                            Else
                                MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName, _
                                    "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                            End If
                        Else
                            Me.TabListDetail.SelectedTabPage = e.PrevPage
                        End If

                    Case Me.TabPageDetail.Name

                        If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count > 0 And Me.ListGridView.FocusedRowHandle >= 0 And Not bInhibitLoadDetails Then
                            MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.TableName, _
                                 "ReadLerrynImportExportConfig_DEV000221", "@SourceCode", Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString}, _
                                 New String() {Me.ConfigSettingsSectionGateway.LerrynLicences_DEV000221.TableName, "ReadLerrynLicences_DEV000221"}, _
                                 New String() {Me.ConfigSettingsSectionGateway.SystemSource.TableName, "ReadSystemSource", _
                                 "@SourceCode", Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 18/03/11
                            ' if no System Source exists e.g. it has been renamed, then create replacement if source activated
                            If Me.ConfigSettingsSectionGateway.SystemSource.Count = 0 Then
                                ' start of code added TJS 09/08/13
                                Select Case Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString
                                    Case ASP_STORE_FRONT_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case MAGENTO_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case VOLUSION_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case AMAZON_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case CHANNEL_ADVISOR_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case SHOP_COM_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case EBAY_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case SEARS_COM_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                    Case AMAZON_FBA_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If

                                        ' start of code added TJS 20/11/13
                                    Case THREE_D_CART_SOURCE_CODE
                                        If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                                            bCreateSource = True
                                        End If
                                        ' end of code added TJS 20/11/13

                                    Case Else
                                        bCreateSource = False
                                End Select
                                If bCreateSource Then
                                    ' end  of code added TJS 09/08/13
                                    rowSource = Me.ConfigSettingsSectionGateway.SystemSource.NewSystemSourceRow()
                                    rowSource.SourceCode = Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221
                                    rowSource.SourceDescription = Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).SourceName_DEV000221
                                    rowSource.IsActive = True
                                    Me.ConfigSettingsSectionGateway.SystemSource.AddSystemSourceRow(rowSource)
                                End If
                            End If

                            MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.SystemShippingMethodGroupDetail.TableName, _
                                "ReadSystemShippingMethodGroupDetailView_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

                            strProductCode = Me.m_ConfigSettingsSectionFacade.ConnectorProductCode(Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221)
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Text = "Source not activated"
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Appearance.ForeColor = System.Drawing.Color.Red
                            DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditInitialActivation1.Text = ""
                            'Me.m_ConfigSettingsdetailSection.TextEditInitialActivation2.Text = "" ' TJS 18/02/10
                            'Me.m_ConfigSettingsdetailSection.TextEditInitialActivation3.Text = "" ' TJS 18/02/10
                            'Me.m_ConfigSettingsdetailSection.TextEditInitialActivation4.Text = "" ' TJS 18/02/10
                            'Me.m_ConfigSettingsdetailSection.TextEditInitialActivation5.Text = "" ' TJS 18/02/10
                            'Me.m_ConfigSettingsdetailSection.TextEditInitialActivation6.Text = "" ' TJS 18/02/10
                            'Me.m_ConfigSettingsdetailSection.TextEditInitialActivation7.Text = "" ' TJS 18/02/10
                            ' is source part of the base module ?
                            If strProductCode <> PRODUCT_CODE Then ' TJS 30/01/09
                                ' no, is connector activated for Inventory Import ?
                                If (strProductCode = ASP_STORE_FRONT_CONNECTOR_CODE Or strProductCode = CHANNEL_ADVISOR_CONNECTOR_CODE Or strProductCode = MAGENTO_CONNECTOR_CODE) And _
                                    Me.m_ConfigSettingsSectionFacade.IsActivated Then ' TJS 21/03/11 TJS 02/12/11

                                    'Additional code to hide LayoutItemActivationCode, LabelActivationStatus, TextEditInitialActivation1 by: mark kee 6/13/2015
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Visible = False
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditInitialActivation1.Visible = False
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LayoutItemActivationCode.TextVisible = False
                                    'End Code
                                    ' yes
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Text = "Activated for Inventory Import only" ' TJS 21/03/11
                                    'DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled = True ' TJS 21/03/11 'comment code by mark kee 6/24/2015
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled = False ' added code by mark 6/24/2015  
                                Else
                                    ' no, disable config settings unless activated
                                    'Additional code to hide LayoutItemActivationCode, LabelActivationStatus, TextEditInitialActivation1 by: mark kee 6/13/2015
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Visible = False
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditInitialActivation1.Visible = False
                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LayoutItemActivationCode.TextVisible = False
                                    'End Code

                                    DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled = False

                                End If
                            Else
                                'DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled = True ' comment code by mark 6/24/2015
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled = False ' added code by mark 6/24/2015 
                            End If
                            SetControlOptions()
                            If Me.m_ConfigSettingsSectionFacade.IsConnectorActivated(strProductCode) Then ' TJS 02/12/11
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Text = "Activation expires " & Me.m_ConfigSettingsSectionFacade.ConnectorActivationExpires(strProductCode).ToShortDateString ' TJS 02/12/11
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Appearance.ForeColor = System.Drawing.Color.Black
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditInitialActivation1.Text = Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(0, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(5, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(10, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(15, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(20, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(25, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(30, 5) ' TJS 18/02/10 TJS 02/12/11
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).GridControlConfigSettings.Enabled = True

                            ElseIf Me.m_ConfigSettingsSectionFacade.HasConnectorBeenActivated(strProductCode) Then ' TJS 02/12/11
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).LabelActivationStatus.Text = "Activation Code has expired"
                                DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).TextEditInitialActivation1.Text = Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(0, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(5, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(10, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(15, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(20, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(25, 5) & "-" & _
                                    Me.m_ConfigSettingsSectionFacade.ConnectorLatestActivationCode(strProductCode).Substring(30, 5) ' TJS 18/02/10 TJS 02/12/11
                            End If
                        End If
                        If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count = 0 And Me.IsNew Then
                            Me.m_ConfigSettingsSectionFacade.NewSource()
                        End If
                        DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).SetSourceSpecificValues() ' TJS 02/12/11
                        DirectCast(Me.PluginContainerBaseDetailPluginInstance, ConfigSettingsDetailSection).SetInitialFocus()

                    Case Me.TabPageDeliveryTranslation.Name
                        DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).SourceCode = Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString
                        If e.PrevPage.Name = Me.TabPageList.Name Then
                            ' menus will be disabled, need to make base control enable them
                            ' make it display the detail page briefly
                            Me.TabListDetail.SelectedTabPage = Me.TabPageDetail
                            Application.DoEvents()
                            ' now display the Delivery Translation page again
                            Me.TabListDetail.SelectedTabPage = Me.TabPageDeliveryTranslation
                        End If
                        If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count > 0 And Me.ListGridView.FocusedRowHandle >= 0 And Not bInhibitLoadDetails Then
                            MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportDeliveryMethods_DEV000221.TableName, _
                                "ReadLerrynImportExportDeliveryMethods_DEV000221", "@SourceCode", Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)

                            MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.SystemShippingMethodGroupDetail.TableName, _
                                "ReadSystemShippingMethodGroupDetailView_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                            If Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString = CHANNEL_ADVISOR_SOURCE_CODE Then ' TJS 30/12/09 TJS 12/081/0
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethod_DEV000221.Visible = False ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethod_DEV000221.VisibleIndex = -1 ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.Visible = True ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.VisibleIndex = 0 ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.ColumnEdit = DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).repSourceDeliveryMethod ' TJS 30/12/09
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClass_DEV000221.Visible = False ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClass_DEV000221.VisibleIndex = -1 ' TJS 30/12/09 TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.Visible = True ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.VisibleIndex = 1 ' TJS 30/12/09 TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.ColumnEdit = DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).repSourceDeliveryClass ' TJS 30/12/09
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).SourceConfig = Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "ConfigSettings_DEV000221").ToString ' TJS 30/12/09
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodGroup_DEV000221.Visible = True ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodGroup_DEV000221.VisibleIndex = 2 ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodCode_DEV000221.Visible = True ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodCode_DEV000221.VisibleIndex = 3 ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).GetChannelAdvShippingCarrierList() ' TJS 30/12/09

                            ElseIf Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString = EBAY_SOURCE_CODE Then ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethod_DEV000221.Visible = False ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethod_DEV000221.VisibleIndex = -1 ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.Visible = True ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.VisibleIndex = 0 ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.ColumnEdit = DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).repSourceDeliveryMethod ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClass_DEV000221.Visible = False ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClass_DEV000221.VisibleIndex = -1 ' TJS 30/12/09 TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.Visible = False ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.VisibleIndex = -1 ' TJS 30/12/09 TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.ColumnEdit = Nothing ' TJS 30/12/09
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodGroup_DEV000221.Visible = True ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodGroup_DEV000221.VisibleIndex = 1 ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodCode_DEV000221.Visible = True ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodCode_DEV000221.VisibleIndex = 2 ' TJS 02/12/11
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).SetSourceSpecificValues() ' TJS 02/12/11

                            Else
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethod_DEV000221.Visible = True ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethod_DEV000221.VisibleIndex = 0 ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.Visible = False ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.VisibleIndex = -1 ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryMethodCode_DEV000221.ColumnEdit = Nothing ' TJS 30/12/09
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClass_DEV000221.Visible = False ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClass_DEV000221.VisibleIndex = -1 ' TJS 30/12/09 TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.Visible = False ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.VisibleIndex = -1 ' TJS 30/12/09 TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colSourceDeliveryClassCode_DEV000221.ColumnEdit = Nothing ' TJS 30/12/09
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodGroup_DEV000221.Visible = True ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodGroup_DEV000221.VisibleIndex = 1 ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodCode_DEV000221.Visible = True ' TJS 27/09/10
                                DirectCast(Me.PluginContainerDeliveryTranslationPluginInstance, ConfigSettingsDeliveryTranslationSection).colShippingMethodCode_DEV000221.VisibleIndex = 2 ' TJS 27/09/10
                            End If
                        End If

                        ' start of code added TJS 22/09/10
                    Case Me.TabPagePaymentTranslation.Name
                        DirectCast(Me.PluginContainerPaymentTranslationPluginInstance, ConfigSettingsPaymentTranslationSection).SourceCode = Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString ' TJS 10/06/12
                        If e.PrevPage.Name = Me.TabPageList.Name Then
                            ' menus will be disabled, need to make base control enable them
                            ' make it display the detail page briefly
                            Me.TabListDetail.SelectedTabPage = Me.TabPageDetail
                            Application.DoEvents()
                            ' now display the Payment Translation page again
                            Me.TabListDetail.SelectedTabPage = Me.TabPagePaymentTranslation
                        End If
                        If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count > 0 And Me.ListGridView.FocusedRowHandle >= 0 And Not bInhibitLoadDetails Then
                            MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportPaymentTypes_DEV000221.TableName, _
                                "ReadLerrynImportExportPaymentTypes_DEV000221", "@SourceCode", Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)
                        End If
                        ' end of code added TJS 22/09/10

                        ' start of code added TJS 19/08/10
                    Case Me.TabPageSKUAliasLookup.Name
                        DirectCast(Me.PluginContainerSKUAliasLookupPluginInstance, ConfigSettingsSKUAliasLookupSection).SourceCode = Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString ' TJS 09/07/11
                        If e.PrevPage.Name = Me.TabPageList.Name Then
                            ' menus will be disabled, need to make base control enable them
                            ' make it display the detail page briefly
                            Me.TabListDetail.SelectedTabPage = Me.TabPageDetail
                            Application.DoEvents()
                            ' now display the SKU Alias Lookup page again
                            Me.TabListDetail.SelectedTabPage = Me.TabPageSKUAliasLookup
                        End If
                        If Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.Count > 0 And Me.ListGridView.FocusedRowHandle >= 0 And Not bInhibitLoadDetails Then
                            MyBase.LoadDataSet(New String()() {New String() {Me.ConfigSettingsSectionGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                                "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", Me.ListGridView.GetRowCellValue(Me.ListGridView.FocusedRowHandle, "SourceCode_DEV000221").ToString}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                        End If
                        ' end of code added TJS 19/08/10

                End Select
                bIgnorePageChangeEvent = False
                If e.Page.Name = Me.TabPageList.Name Then ' TJS 02/12/11
                    Me.FindForm.Text = PRODUCT_NAME & " Configuration Settings" ' TJS 02/12/11
                ElseIf listControl.SearchGrid.GetRowCellValue(listControl.SearchGrid.FocusedRowHandle, Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.SourceName_DEV000221Column.ColumnName) IsNot Nothing Then ' TJS 05/07/12
                    Me.FindForm.Text = PRODUCT_NAME & " Configuration - " & listControl.SearchGrid.GetRowCellValue(listControl.SearchGrid.FocusedRowHandle, Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.SourceName_DEV000221Column.ColumnName).ToString ' TJS 22/03/11
                End If
            End If

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        Finally
            Cursor = Cursors.Default

        End Try
    End Sub
#End Region

#Region " listControl_RowChanged "
    Private Sub listControl_RowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles m_GridListView.FocusedRowChanged ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for SKU ALias Loopup
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for Payment Translation
        ' 22/03/11 | TJS             | 2011.0.03 | Modified to update form name to reflect selected row
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and to cater for IS 6
        ' 10/06/12 | TJS             | 2012.1.05 | Modified for MAgento EnablePaymentTypeTranslation
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector and EnableDisableOptionTabs function
        '------------------------------------------------------------------------------------------

        Dim XMLConfig As XDocument, XMLGroupNode As XElement, XMLConfigNode As XElement
        Dim XMLGroupNodeList As System.Collections.Generic.IEnumerable(Of XNode)
        Dim XMLConfigNodeList As System.Collections.Generic.IEnumerable(Of XNode)
        Dim bXMLError As Boolean, bEnableNew As Boolean ' TJS 02/12/11

        Me.TabPageDeliveryTranslation.PageVisible = False
        Me.TabPagePaymentTranslation.PageVisible = False ' TJS 22/09/10
        Me.TabPageSKUAliasLookup.PageVisible = False ' TJS 19/08/10
        bEnableNew = False ' TJS 02/12/11
        If listControl.SearchGrid.FocusedRowHandle >= 0 Then
            bXMLError = False ' TJS 02/12/11
            XMLConfig = Nothing ' TJS 02/12/11
            Try
                XMLConfig = XDocument.Parse(Trim(listControl.SearchGrid.GetRowCellValue(listControl.SearchGrid.FocusedRowHandle, Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.ConfigSettings_DEV000221Column.ColumnName).ToString))

            Catch ex As Exception
                Interprise.Presentation.Base.Message.MessageWindow.Show("XML Config error - " & ex.Message) ' TJS 02/12/11
                bXMLError = True ' TJS 02/12/11
            End Try

            If Not bXMLError Then
                If CBool(Me.ListGridView.GetRowCellValue(listControl.SearchGrid.FocusedRowHandle, "Active")) Then ' TJS 02/12/11
                    XMLGroupNodeList = XMLConfig.XPathSelectElement("eShopCONNECTConfig").Nodes ' TJS 20/11/13
                    For Each XMLGroupNode In XMLGroupNodeList
                        XMLConfigNodeList = XMLGroupNode.Nodes
                        For Each XMLConfigNode In XMLConfigNodeList
                            EnableDisableOptionTabs(XMLGroupNode, XMLConfigNode) ' TJS 20/11/13
                        Next
                    Next
                    If (PRODUCT_CODE = ESHOPCONNECT_BASE_PRODUCT_CODE Or PRODUCT_CODE = PROSPECTIMPORTER_BASE_PRODUCT_CODE) And _
                        listControl.SearchGrid.GetRowCellValue(listControl.SearchGrid.FocusedRowHandle, Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.InputHandler_DEV000221Column.ColumnName).ToString = "GenericXMLImport.ashx" Then ' TJS 02/12/11
                        bEnableNew = True ' TJS 02/12/11

                    ElseIf PRODUCT_CODE = ORDERIMPORTER_BASE_PRODUCT_CODE And _
                        listControl.SearchGrid.GetRowCellValue(listControl.SearchGrid.FocusedRowHandle, Me.ConfigSettingsSectionGateway.LerrynImportExportConfig_DEV000221.InputHandler_DEV000221Column.ColumnName).ToString = "Windows Service" Then ' TJS 02/12/11
                        bEnableNew = True ' TJS 02/12/11
                    End If
                End If
                If bEnableNew Then ' TJS 02/12/11
                    DirectCast(Me.FindForm, SystemManager.Config.ConfigSettingsForm).MenuItemNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Always ' TJS 02/12/11
                Else
                    DirectCast(Me.FindForm, SystemManager.Config.ConfigSettingsForm).MenuItemNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never ' TJS 02/12/11
                End If
            End If
        Else
            Me.FindForm.Text = PRODUCT_NAME & " Configuration Settings"
        End If

    End Sub
#End Region
#End Region

End Class
#End Region
