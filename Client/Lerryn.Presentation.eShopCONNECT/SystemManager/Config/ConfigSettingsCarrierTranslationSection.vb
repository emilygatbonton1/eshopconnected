
'Added JET 12/15/15
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const '
Imports System.IO ' TJS 25/04/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 18/03/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath '
Imports System.Collections.Generic
Imports Interprise.Framework.Base.Shared.Const

#Region " ConfigSettingsCarrierTranslationSection"
Public Class ConfigSettingsCarrierTranslationSection
    Implements Lerryn.Extendable.ImportExport.Presentation.SystemManager.Config.ICarrierTranslationSectionInterface

#Region " Variables "

    Private m_ConfigSettingsCarrierTranslationDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_ConfigSettingsCarrierTranslationSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_CarrierList As List(Of String)
    Private m_CarrierServiceType As List(Of String)
    Private m_SourceCode As String
    Private m_SourceConfig As String
    Private m_NewCarrierTranslationRow As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.eShopCarrierTranslationView_DEV000221Row
    'Dim Coll As DevExpress.XtraEditors.Controls.
    'Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

#End Region

#Region " Properties "

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return m_ConfigSettingsCarrierTranslationSectionFacade
        End Get
    End Property
#End Region

#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.m_ConfigSettingsCarrierTranslationDataset
        End Get
    End Property
    Public Overridable ReadOnly Property ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return m_ConfigSettingsCarrierTranslationDataset
        End Get
    End Property

#End Region

#Region " SourceCode "
    Public WriteOnly Property SourceCode() As String Implements Extendable.ImportExport.Presentation.SystemManager.Config.ICarrierTranslationSectionInterface.SourceCode
        Set(value As String)
            m_SourceCode = value
        End Set
    End Property
#End Region

#Region " SourceConfig "
    Public WriteOnly Property SourceConfig() As String Implements Extendable.ImportExport.Presentation.SystemManager.Config.ICarrierTranslationSectionInterface.SourceConfig
        Set(ByVal value As String)
            m_SourceConfig = value
        End Set
    End Property
#End Region

#End Region

#Region " Methods "

#Region " Constructor "
    Public Sub New()

        Me.m_ConfigSettingsCarrierTranslationDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.m_ConfigSettingsCarrierTranslationSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_ConfigSettingsCarrierTranslationDataset, _
           New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)
    End Sub

    Public Sub New(ByVal ConfigSettingsCarrierTranslationDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
         ByVal ConfigSettingsCarrierTranslationSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)

        MyBase.New()

        Me.m_ConfigSettingsCarrierTranslationDataset = ConfigSettingsCarrierTranslationDataset
        Me.m_ConfigSettingsCarrierTranslationSectionFacade = ConfigSettingsCarrierTranslationSectionFacade

        'This call is required by the Windows Form Designer.S
        Me.InitializeComponent()
    End Sub
#End Region

#Region " LoadDataSet "
    Public Overrides Function LoadDataSet(documentCode As String, row As System.Data.DataRow, Optional clearTableType As Interprise.Framework.Base.Shared.Enum.ClearType = Interprise.Framework.Base.Shared.Enum.ClearType.None) As Boolean

        LoadDataSet = MyBase.LoadDataSet(New String()() {New String() {Me.m_ConfigSettingsCarrierTranslationDataset.eShopCarrierTranslationView_DEV000221.TableName, _
                                "ReadeShopCarrierTranslation_DEV000221", "@SourceCode", m_SourceCode}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)

        Return LoadDataSet
    End Function
#End Region

#Region " UpdateDataSet"
    Public Overrides Function UpdateDataSet(Optional ByVal confirm As Boolean = False, Optional ByVal clear As Boolean = False, Optional ByVal isUseCache As Boolean = False) As System.Windows.Forms.DialogResult
        m_ConfigSettingsCarrierTranslationSectionFacade.UpdateCarrierTranslationDataset()
        Return DialogResult.Yes
    End Function
#End Region

#Region " GetChannelAdvShippingCarrierList "
    Public Sub GetChannelAdvShippingCarrierList()

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader ' TJS 02/12/11
        Dim XMLConfig As XDocument, XMLResponse As XDocument, XMLTemp As XDocument
        Dim XMLAccountList As System.Collections.Generic.IEnumerable(Of XElement), XMLCarrierList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLAccountID As XElement, XMLCarrier As XElement
        Dim XMLNSManCAShipping As System.Xml.XmlNamespaceManager, XMLNameTabCAShipping As System.Xml.NameTable ' TJS 02/04/14
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim strSubmit As String, strReturn As String, iLoop As Integer, iCheckLoop As Integer
        Dim bCarrierServiceAlreadyLoaded As Boolean
        Dim bCarrierAlreadyLoaded As Boolean

        If m_SourceCode = CHANNEL_ADVISOR_SOURCE_CODE Then
            m_CarrierList = New List(Of String)
            m_CarrierServiceType = New List(Of String)

            XMLConfig = XDocument.Parse(m_SourceConfig)
            XMLAccountList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)

            For Each XMLAccountID In XMLAccountList
                XMLTemp = XDocument.Parse(XMLAccountID.ToString)
                If XMLTemp.ToString <> "" Then

                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                    strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/"">"
                    If "" & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY) <> "" Then
                        strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY)
                        strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_PWD)
                    Else
                        strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY
                        strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                    End If
                    strSubmit = strSubmit & "</web:Password></web:APICredentials></soapenv:Header><soapenv:Body><web:GetShippingCarrierList><web:accountID>"
                    strSubmit = strSubmit & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID)
                    strSubmit = strSubmit & "</web:accountID></web:GetShippingCarrierList></soapenv:Body></soapenv:Envelope>"

                    strReturn = ""
                    Try
                        WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_SHIPPING_SERVICE_URL)
                        WebSubmit.Method = "POST"
                        WebSubmit.ContentType = "text/xml; charset=utf-8" ' TJS 02/04/14
                        WebSubmit.ContentLength = strSubmit.Length
                        WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/GetShippingCarrierList")
                        WebSubmit.Timeout = 30000

                        byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                        ' send to LerrynSecure.com (or the URL defined in the Registry)
                        postStream = WebSubmit.GetRequestStream()
                        postStream.Write(byteData, 0, byteData.Length)

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        strReturn = reader.ReadToEnd()

                    Catch ex As Exception
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor - error details: " & ex.Message)

                    Finally
                        If Not postStream Is Nothing Then postStream.Close()
                        If Not WebResponse Is Nothing Then WebResponse.Close()

                    End Try

                    If strReturn <> "" Then
                        If strReturn.Length > 38 Then
                            If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                ' had difficulty getting XPath to read XML with this name space present so remove it
                                XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", "")) ' TJS 02/04/14
                                'FA 03/12/2010 moved before the For loop as this was over-writing values
                                'if more than one Source
                                'iChannelAdvCarrierCount = 0
                                'ReDim ChannelAdvCarriers(iChannelAdvCarrierCount)
                                XMLNameTabCAShipping = New System.Xml.NameTable ' TJS 02/04/14
                                XMLNSManCAShipping = New System.Xml.XmlNamespaceManager(XMLNameTabCAShipping) ' TJS 02/04/14
                                XMLNSManCAShipping.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/04/14
                                XMLCarrierList = XMLResponse.XPathSelectElements("soap:Envelope/soap:Body/GetShippingCarrierListResponse/GetShippingCarrierListResult/ResultData/ShippingCarrier", XMLNSManCAShipping) ' TJS 02/04/14

                                For Each XMLCarrier In XMLCarrierList
                                    Try
                                        XMLTemp = XDocument.Parse(XMLCarrier.ToString)
                                        bCarrierAlreadyLoaded = False
                                        If m_CarrierList.Count > 0 Then
                                            For iLoop = 0 To m_CarrierList.Count - 1
                                                If m_CarrierList(iLoop) = m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/CarrierCode") Then
                                                    bCarrierAlreadyLoaded = True
                                                End If
                                            Next
                                        End If
                                        If Not bCarrierAlreadyLoaded Then
                                            m_CarrierList.Add(m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/CarrierCode").ToString)
                                        End If

                                        bCarrierServiceAlreadyLoaded = False
                                        If m_CarrierServiceType.Count > 0 Then
                                            For iLoop = 0 To m_CarrierServiceType.Count - 1
                                                If m_CarrierServiceType(iLoop) = m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/ClassCode") Then
                                                    bCarrierServiceAlreadyLoaded = True
                                                End If
                                            Next
                                        End If
                                        If Not bCarrierServiceAlreadyLoaded Then
                                            m_CarrierServiceType.Add(m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/ClassCode").ToString)
                                        End If

                                    Catch ex As Exception
                                        Interprise.Presentation.Base.Message.MessageWindow.Show("XML Error in Channel Advisor Shipping Carrier List - " & ex.Message)

                                    End Try

                                Next
                            Else
                                Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor for account " & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME) & " (" & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) & ") - response data is not valid XML") ' TJS 12/10/10
                            End If
                        Else
                            Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor for account " & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME) & " (" & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) & ") - response data is not valid XML") ' TJS 12/10/10
                        End If
                    Else
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor for account " & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME) & " (" & m_ConfigSettingsCarrierTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) & ") - response data is blank") ' TJS 12/10/10
                    End If
                Else
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor as your config settings are invalid")
                End If
            Next
        End If
    End Sub
#End Region

#Region " SetMarketplaceCarrierServiceVisibility "
    Public Sub SetMarketplaceCarrierServiceVisibility()
        If m_SourceCode = AMAZON_SOURCE_CODE Then
            Me.colMarketplaceServiceType.Visible = False
        Else
            Me.colMarketplaceServiceType.Visible = True
        End If
    End Sub
#End Region

#End Region

#Region " Events "

#Region " CBServiceTypeSearchComboControl_BeforePopup "
    Private Sub CBServiceTypeSearchComboControl_BeforePopup(sender As Object, e As CancelEventArgs) Handles CBServiceTypeSearchComboControl.BeforePopup

        With m_ConfigSettingsCarrierTranslationDataset
            Dim carrierCode = CarrierTranslationGridView.GetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.CBCarrierCode_DEV000221Column.ColumnName)
            Dim warehouseCode = CarrierTranslationGridView.GetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.WarehouseCodeColumn.ColumnName)
            If carrierCode IsNot Nothing Then
                Me.CBServiceTypeSearchComboControl.AdditionalFilter = String.Format(FORMAT_FILTER_0, .ShipmentCarrierServices.CarrierCodeColumn.ColumnName, carrierCode)
            Else
                e.Cancel = True
            End If
        End With
    End Sub
#End Region
   
#Region " MarketplaceCarrierCodeSearchComboControl_BeforePopup "
    Private Sub MarketplaceCarrierCodeSearchComboControl_BeforePopup(sender As Object, e As CancelEventArgs) Handles MarketplaceCarrierCodeSearchComboControl.BeforePopup

        If Not CarrierTranslationGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
            If m_SourceCode = AMAZON_SOURCE_CODE Then
                With MarketplaceCarrierCodeSearchComboControl
                    .TableName = m_ConfigSettingsCarrierTranslationDataset.eShopAmazonCarrierCodes_DEV000221.TableName
                    .DataSource = Nothing
                    .ColumnDescriptions = Nothing
                    .DisplayField = Nothing
                    .ValueMember = Nothing
                End With
            Else
                Dim carrierCodeDataTable As DataTable
                carrierCodeDataTable = New DataTable
                Dim carrierCodeDesc() As String = New String(0) {"CarrierCode"}
                m_ConfigSettingsCarrierTranslationSectionFacade.GetChannelAdvCarrierCodesList(carrierCodeDataTable, m_CarrierList)
                With MarketplaceCarrierCodeSearchComboControl
                    .DataSource = carrierCodeDataTable
                    .TableName = "carrierCodeDataTable"
                    .ColumnDescriptions = carrierCodeDesc
                End With
            End If
        Else
            e.Cancel = True
        End If
    End Sub
#End Region

#Region " MarketplaceServiceTypeComboControl_BeforePopup "
    Private Sub MarketplaceServiceTypeComboControl_BeforePopup(sender As Object, e As CancelEventArgs) Handles MarketplaceServiceTypeComboControl.BeforePopup

        If Not CarrierTranslationGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle Then

            Dim serviceTypeDataTable As DataTable
            serviceTypeDataTable = New DataTable
            Dim serviceTypeDesc() As String = New String(0) {"ServiceType"}
            m_ConfigSettingsCarrierTranslationSectionFacade.GetChannelAdvServiceTypeList(serviceTypeDataTable, m_CarrierServiceType)
            With MarketplaceServiceTypeComboControl
                .DataSource = serviceTypeDataTable
                .TableName = "serviceTypeDataTable"
                .ColumnDescriptions = serviceTypeDesc
                .DataSourceColumns = serviceTypeDesc
            End With
        Else
            e.Cancel = True
        End If

    End Sub
#End Region

#Region " CBCarrierCodeSearchComboControl_PopupClose "
    Private Sub CBCarrierCodeSearchComboControl_PopupClose(sender As Object, eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles CBCarrierCodeSearchComboControl.PopupClose

        With m_ConfigSettingsCarrierTranslationDataset
            Dim serviceType = CarrierTranslationGridView.GetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.CBServiceType_DEV000221Column.ColumnName)
            If Not m_ConfigSettingsCarrierTranslationSectionFacade.CheckforDuplicateCarrierTranslation(m_SourceCode, eRow.DataRowSelected(.ShipmentCarrier.CarrierCodeColumn.ColumnName).ToString, serviceType) Then

                If CarrierTranslationGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
                    CarrierTranslationGridView.AddNewRow()
                    m_ConfigSettingsCarrierTranslationSectionFacade.SetNewCarrierTranslationRow(m_NewCarrierTranslationRow, m_SourceCode, eRow.DataRowSelected(.ShipmentCarrier.CarrierCodeColumn.ColumnName).ToString, eRow.DataRowSelected(.ShipmentCarrier.WarehouseCodeColumn.ColumnName).ToString, eRow.DataRowSelected(.ShipmentCarrier.CarrierDescriptionColumn.ColumnName).ToString)
                Else
                    CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.CBCarrierCode_DEV000221Column.ColumnName, eRow.DataRowSelected(.ShipmentCarrier.CarrierCodeColumn.ColumnName).ToString)
                    CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.CarrierDescriptionColumn.ColumnName, eRow.DataRowSelected(.ShipmentCarrier.CarrierDescriptionColumn.ColumnName).ToString)
                    CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.WarehouseCodeColumn.ColumnName, eRow.DataRowSelected(.ShipmentCarrier.WarehouseCodeColumn.ColumnName).ToString)
                    CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.CBServiceType_DEV000221Column.ColumnName, "")
                End If
            Else
                Interprise.Presentation.Base.Message.MessageWindow.Show("Carrier Code and Service Type already Exists")
            End If

        End With
    End Sub
#End Region

#Region " CBServiceTypeSearchComboControl_PopupClose "
    Private Sub CBServiceTypeSearchComboControl_PopupClose(sender As Object, eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles CBServiceTypeSearchComboControl.PopupClose

        If Not CarrierTranslationGridView.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle Then
            With m_ConfigSettingsCarrierTranslationDataset
                Dim carrierCode = CarrierTranslationGridView.GetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.CBCarrierCode_DEV000221Column.ColumnName)
                If Not m_ConfigSettingsCarrierTranslationSectionFacade.CheckforDuplicateCarrierTranslation(m_SourceCode, carrierCode, eRow.DataRowSelected(.ShipmentCarrierServices.ServiceTypeColumn.ColumnName).ToString) Then
                    CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.CBServiceType_DEV000221Column.ColumnName, eRow.DataRowSelected(.ShipmentCarrierServices.ServiceTypeColumn.ColumnName).ToString)
                Else
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Carrier Code and Service Type already Exists")
                End If
            End With
        End If
    End Sub
#End Region

#Region " MarketplaceCarrierCodeSearchComboControl_PopupClose "
    Private Sub MarketplaceCarrierCodeSearchComboControl_PopupClose(sender As Object, eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles MarketplaceCarrierCodeSearchComboControl.PopupClose
        With m_ConfigSettingsCarrierTranslationDataset
            If m_SourceCode = AMAZON_SOURCE_CODE Then
                CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.MarketplaceCarrierCode_DEV000221Column.ColumnName, eRow.DataRowSelected(.eShopAmazonCarrierCodes_DEV000221.AmazonCarrierCodes_DEV000221Column.ColumnName).ToString)
            Else
                CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.MarketplaceCarrierCode_DEV000221Column.ColumnName, eRow.DataRowSelected("CarrierCode").ToString)
            End If
        End With

    End Sub
#End Region

#Region " MarketplaceServiceTypeComboControl_PopupClose "
    Private Sub MarketplaceServiceTypeComboControl_PopupClose(sender As Object, eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles MarketplaceServiceTypeComboControl.PopupClose
        With m_ConfigSettingsCarrierTranslationDataset
            CarrierTranslationGridView.SetRowCellValue(CarrierTranslationGridView.FocusedRowHandle, .eShopCarrierTranslationView_DEV000221.MarketplaceServiceType_DEV000221Column.ColumnName, eRow.DataRowSelected(0).ToString)
        End With
    End Sub
#End Region

#Region " CarrierTranslationGridView_InitNewRow "

    Private Sub CarrierTranslationGridView_InitNewRow(sender As Object, e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles CarrierTranslationGridView.InitNewRow
        Me.m_NewCarrierTranslationRow = DirectCast(CarrierTranslationGridView.GetDataRow(e.RowHandle), Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.eShopCarrierTranslationView_DEV000221Row)

    End Sub
#End Region

#End Region

End Class

#End Region

