' eShopCONNECT for Connected Business
' Module: ChannelAdvImportFacade.vb
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

Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Base.Shared.StoredProcedures
Imports Interprise.Framework.Inventory.Shared.Const

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const

Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Xml.Serialization
Imports System.Xml
Imports System.Xml.Linq
Imports System.Xml.XPath

#Region " ChannelAdvImportFacade "
Public Class ChannelAdvImportFacade
    Inherits Interprise.Facade.Base.BaseFacade
    Implements Interprise.Extendable.Base.Facade.IBaseInterface

#Region " Variables "
    Private m_ChanAdvImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_ChanAdvImportRule As Lerryn.Facade.ImportExport.ImportExportFacade

    Private m_ProductsAlreadyImported As Integer = 0
    Private m_CreateISCategories As Boolean
    Private m_ImportLog As String
    Private m_LastError As String
    Private m_ImportLimitReached As Boolean
    Private m_QuantityPublishingType As String
    Private m_QuantityPublishingValue As Decimal

    Private NewItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
    Private NewManufacturerFacade As Interprise.Facade.Inventory.SystemManager.ManufacturerFacade
    Private NewKitFacade As Interprise.Facade.Inventory.ItemKitFacade

    Private WithEvents NewItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private WithEvents NewManufacturerDataset As Interprise.Framework.Inventory.DatasetGateway.SystemManager.ManufacturerDatasetGateway
    Private WithEvents NewKitDataset As Interprise.Framework.Inventory.DatasetGateway.ItemKitDatasetGateway

    Private Const ChannelAdvProductXMLPath As String = "soap:Envelope/soap:Body/GetInventoryItemListResponse/GetInventoryItemListResult/ResultData/InventoryItemResponse"
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return m_ChanAdvImportDataset
        End Get
    End Property
#End Region

#Region " CurrentBusinessRule "
    Public Overrides ReadOnly Property CurrentBusinessRule() As Interprise.Extendable.Base.Business.IBaseInterface
        Get
            Return m_ChanAdvImportRule
        End Get
    End Property
#End Region

#Region " CurrentTransactionType "
    Public Overrides ReadOnly Property CurrentTransactionType() As Interprise.Framework.Base.Shared.Enum.TransactionType
        Get
            Return Nothing
        End Get
    End Property
#End Region

#Region " CurrentReportType "
    Public Overrides ReadOnly Property CurrentReportType() As Interprise.Framework.Base.Shared.Enum.ReportAction
        Get
            Return Nothing
        End Get
    End Property
#End Region

#Region " ProductCountAlreadyImported "
    Public ReadOnly Property ProductCountAlreadyImported() As Integer
        Get
            Return m_ProductsAlreadyImported
        End Get
    End Property
#End Region

#Region " CreateISCategories "
    Public Property CreateISCategories() As Boolean
        Get
            Return m_CreateISCategories
        End Get
        Set(ByVal value As Boolean)
            m_CreateISCategories = value
        End Set
    End Property
#End Region

#Region " QuantityPublishingType "
    Public Property QuantityPublishingType() As String
        Get
            Return m_QuantityPublishingType
        End Get
        Set(ByVal value As String)
            m_QuantityPublishingType = value
        End Set
    End Property
#End Region

#Region " QuantityPublishingValue "
    Public Property QuantityPublishingValue() As Decimal
        Get
            Return m_QuantityPublishingValue
        End Get
        Set(ByVal value As Decimal)
            m_QuantityPublishingValue = value
        End Set
    End Property
#End Region

#Region " ImportLog "
    Public ReadOnly Property ImportLog() As String
        Get
            Return m_ImportLog
        End Get
    End Property
#End Region

#Region " LastError "
    Public ReadOnly Property LastError() As String
        Get
            Return m_LastError
        End Get
    End Property
#End Region

#Region " ImportLimitReached "
    Public ReadOnly Property ImportLimitReached() As Boolean
        Get
            Return m_ImportLimitReached
        End Get
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New(ByRef p_ChanAdvImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal p_BaseProductCode As String, ByVal p_BaseProductName As String)
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
        m_ChanAdvImportDataset = p_ChanAdvImportDataset
        m_ChanAdvImportRule = New Lerryn.Facade.ImportExport.ImportExportFacade(p_ChanAdvImportDataset, New Lerryn.Facade.ImportExport.ErrorNotification, p_BaseProductCode, p_BaseProductName) ' TJS 10/06/12
        MyBase.InitializeDataset()

        ' read all licences as we need base licence and all add-ons
        Me.LoadDataSet(New String()() {New String() {m_ChanAdvImportDataset.LerrynLicences_DEV000221.TableName, _
            "ReadLerrynLicences_DEV000221"}, New String() {m_ChanAdvImportDataset.System_DEV000221.TableName, _
            "ReadSystem_DEV000221"}, New String() {m_ChanAdvImportDataset.SystemCompanyInformation.TableName, _
            "ReadSystemCompanyInformation"}, New String() {m_ChanAdvImportDataset.LerrynImportExportConfig_DEV000221.TableName, _
            "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        m_ChanAdvImportRule.CheckActivation()

    End Sub
#End Region

#Region " GetChannelAdvProductList "
    Public Function GetChannelAdvProductList(ByVal ChannelAdvisorAccountID As String, ByRef Cancel As Boolean, ByRef ItemsForImport As DataTable) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for non-stock items
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument
        Dim XMLNSManCAItems As System.Xml.XmlNamespaceManager, XMLNameTabCAItems As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement
        Dim colNewColumn As DataColumn, rowImportItems As System.Data.DataRow
        Dim strImportedProducts As String()()
        Dim strSubmit As String, strReturn As String, strSKU As String, strASIN As String
        Dim iPageFilter As Integer, iLoop As Integer, iChannelAdvAccountCount As Integer
        Dim bInstanceMatched As Boolean, bReturnValue As Boolean

        bReturnValue = False
        m_LastError = ""
        If m_ChanAdvImportRule.IsActivated Then
            XMLConfig = XDocument.Parse(Trim(m_ChanAdvImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
            XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)
            iChannelAdvAccountCount = m_ChanAdvImportRule.GetXMLElementListCount(XMLNodeList)
            For Each XMLNode In XMLNodeList
                XMLTemp = XDocument.Parse(XMLNode.ToString)
                If iChannelAdvAccountCount = 1 Then
                    bInstanceMatched = True
                ElseIf m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) = ChannelAdvisorAccountID Then
                    bInstanceMatched = True
                End If
                If bInstanceMatched Then

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Import"
                    colNewColumn.ColumnName = "Import"
                    colNewColumn.DataType = System.Type.GetType("System.Boolean")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Item Name"
                    colNewColumn.ColumnName = "ItemName"
                    colNewColumn.DataType = System.Type.GetType("System.String")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Item SKU"
                    colNewColumn.ColumnName = "ItemSKU"
                    colNewColumn.DataType = System.Type.GetType("System.String")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Channel Advisor Type"
                    colNewColumn.ColumnName = "SourceType"
                    colNewColumn.DataType = System.Type.GetType("System.String")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    ' Channel Advisor doesn't have any ID of it's own for inventory items
                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Not used"
                    colNewColumn.ColumnName = "SourceItemID"
                    colNewColumn.DataType = System.Type.GetType("System.String")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Import as Kit"
                    colNewColumn.ColumnName = "ImportAsKit"
                    colNewColumn.DataType = System.Type.GetType("System.Boolean")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Already Imported"
                    colNewColumn.ColumnName = "AlreadyImported"
                    colNewColumn.DataType = System.Type.GetType("System.Boolean")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "SKU Exists"
                    colNewColumn.ColumnName = "SKUExists"
                    colNewColumn.DataType = System.Type.GetType("System.Boolean")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "SKU Error"
                    colNewColumn.ColumnName = "SKUError"
                    colNewColumn.DataType = System.Type.GetType("System.Boolean")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "ASIN Different"
                    colNewColumn.ColumnName = "SKUChanged"
                    colNewColumn.DataType = System.Type.GetType("System.Boolean")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "Item Code"
                    colNewColumn.ColumnName = "ItemCode"
                    colNewColumn.DataType = System.Type.GetType("System.String")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn
                    colNewColumn.Caption = "ASIN"
                    colNewColumn.ColumnName = "ASIN"
                    colNewColumn.DataType = System.Type.GetType("System.String")
                    ItemsForImport.Columns.Add(colNewColumn)
                    colNewColumn.Dispose()

                    colNewColumn = New DataColumn ' TJS 09/03/13
                    colNewColumn.Caption = "IS Item Type" ' TJS 09/03/13
                    colNewColumn.ColumnName = "ISItemType" ' TJS 09/03/13
                    colNewColumn.DataType = System.Type.GetType("System.String") ' TJS 09/03/13
                    ItemsForImport.Columns.Add(colNewColumn) ' TJS 09/03/13
                    colNewColumn.Dispose() ' TJS 09/03/13

                    colNewColumn = New DataColumn ' TJS 09/08/13
                    colNewColumn.Caption = "Source ID Changed" ' TJS 09/08/13
                    colNewColumn.ColumnName = "SourceIDChanged" ' TJS 09/08/13
                    colNewColumn.DataType = System.Type.GetType("System.Boolean") ' TJS 09/08/13
                    ItemsForImport.Columns.Add(colNewColumn) ' TJS 09/08/13
                    colNewColumn.Dispose() ' TJS 09/08/13


                    strImportedProducts = Me.GetRows(New String() {"ChannelAdvRecordID", "ItemName", "ItemCode", "ASIN_DEV000221"}, "InventoryItemChannelAdvisorSummaryView_DEV000221")
                    m_ProductsAlreadyImported = 0
                    iPageFilter = 1
                    ' loop until no more intentory items
                    Do
                        strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                        strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/""><soapenv:Header>"
                        If m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY) <> "" Then
                            strSubmit = strSubmit & "<web:APICredentials><web:DeveloperKey>" & m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY)
                            strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_PWD)
                            strSubmit = strSubmit & "</web:Password></web:APICredentials>"
                        Else
                            strSubmit = strSubmit & "<web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY & "</web:DeveloperKey>"
                            strSubmit = strSubmit & "<web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD & "</web:Password></web:APICredentials>"
                        End If
                        strSubmit = strSubmit & "</soapenv:Header><soapenv:Body><web:GetFilteredInventoryItemList><web:accountID>" & ChannelAdvisorAccountID & "</web:accountID>"
                        strSubmit = strSubmit & "<web:itemCriteria><web:PageNumber>" & iPageFilter & "</web:PageNumber><web:PageSize>50</web:PageSize>"
                        strSubmit = strSubmit & "</web:itemCriteria><web:detailLevel></web:detailLevel></web:GetFilteredInventoryItemList></soapenv:Body></soapenv:Envelope>"

                        Try
                            WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_INVENTORY_SERVICE_URL)
                            WebSubmit.Method = "POST"
                            WebSubmit.ContentType = "text/xml; charset=utf-8"
                            WebSubmit.ContentLength = strSubmit.Length
                            WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/GetFilteredInventoryItemList")
                            WebSubmit.Timeout = 30000

                            byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                            ' send to Channel Advisor
                            postStream = WebSubmit.GetRequestStream()
                            postStream.Write(byteData, 0, byteData.Length)

                            WebResponse = WebSubmit.GetResponse
                            reader = New StreamReader(WebResponse.GetResponseStream())
                            strReturn = reader.ReadToEnd()

                            If strReturn <> "" Then
                                Try
                                    ' had difficulty getting XPath to read XML with this name space present so remove it
                                    XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))

                                Catch ex As Exception
                                    m_LastError = "Inventory List XML from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")
                                    Exit Do

                                End Try

                                ' was response a success ?
                                XMLNameTabCAItems = New System.Xml.NameTable
                                XMLNSManCAItems = New System.Xml.XmlNamespaceManager(XMLNameTabCAItems)
                                XMLNSManCAItems.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/")

                                If m_ChanAdvImportRule.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/GetFilteredInventoryItemListResponse/GetFilteredInventoryItemListResult/Status", XMLNSManCAItems) = "Success" Then
                                    ' yes, process inventory items
                                    XMLItemList = XMLResponse.XPathSelectElements("soap:Envelope/soap:Body/GetFilteredInventoryItemListResponse/GetFilteredInventoryItemListResult/ResultData/InventoryItemResponse", XMLNSManCAItems)
                                    If m_ChanAdvImportRule.GetXMLElementListCount(XMLItemList) > 0 Then
                                        For Each XMLItemNode In XMLItemList
                                            Dim XMLItemTemp As XDocument = XDocument.Parse(XMLItemNode.ToString)
                                            rowImportItems = ItemsForImport.NewRow
                                            rowImportItems.Item("Import") = True
                                            rowImportItems.Item("AlreadyImported") = False
                                            rowImportItems.Item("SKUExists") = False
                                            rowImportItems.Item("SKUError") = False
                                            rowImportItems.Item("SKUChanged") = False
                                            rowImportItems.Item("SourceIDChanged") = False ' TJS 09/08/13

                                            strASIN = m_ChanAdvImportRule.GetXMLElementText(XMLItemTemp, "InventoryItemResponse/ASIN")
                                            strSKU = m_ChanAdvImportRule.GetXMLElementText(XMLItemTemp, "InventoryItemResponse/Sku")
                                            rowImportItems.Item("ItemSKU") = strSKU
                                            If strASIN <> "" AndAlso IsNumeric(strASIN) Then
                                                For Each ImportedProduct As String() In strImportedProducts
                                                    If strASIN = ImportedProduct(3) And ImportedProduct(1) = strSKU And "" & ImportedProduct(0) <> "" Then
                                                        ' Channel Advisor inventory record exists with same SKU and ASIN matches Inventory ItemCode
                                                        rowImportItems.Item("Import") = False
                                                        rowImportItems.Item("AlreadyImported") = True
                                                        rowImportItems.Item("ItemCode") = ImportedProduct(2)

                                                    ElseIf ImportedProduct(3) = "" And ImportedProduct(1) = strSKU And "" & ImportedProduct(0) <> "" Then
                                                        ' Channel Advisor inventory record exists with same SKU but ASIN is blank
                                                        rowImportItems.Item("Import") = False
                                                        rowImportItems.Item("AlreadyImported") = True
                                                        rowImportItems.Item("ItemCode") = ImportedProduct(2)

                                                    ElseIf strASIN = ImportedProduct(3) And ImportedProduct(1) = strSKU And "" & ImportedProduct(0) = "" Then
                                                        ' no Channel Advisor inventory record, so SKU exists in InventoryItem table only and ASIN matches Inventory ItemCode
                                                        rowImportItems.Item("SKUExists") = True
                                                        rowImportItems.Item("ItemCode") = ImportedProduct(2)

                                                    ElseIf ImportedProduct(3) = "" And ImportedProduct(1) = strSKU And "" & ImportedProduct(0) <> "" Then
                                                        ' no Channel Advisor inventory record, so SKU exists in InventoryItem table only and ASIN is blank
                                                        rowImportItems.Item("SKUExists") = True
                                                        rowImportItems.Item("ItemCode") = ImportedProduct(2)

                                                    ElseIf strASIN = ImportedProduct(3) And ImportedProduct(1) <> strSKU Then
                                                        rowImportItems.Item("Import") = False
                                                        rowImportItems.Item("SKUChanged") = True
                                                        rowImportItems.Item("ItemCode") = ImportedProduct(2)

                                                    End If
                                                Next
                                            Else
                                                For Each ImportedProduct As String() In strImportedProducts
                                                    If ImportedProduct(1) = strSKU Then
                                                        ' Channel Advisor doesn't have it's own product ID so we can only check for the presence of a Channel Advisor inventory record ID
                                                        If "" & ImportedProduct(0) <> "" Then
                                                            ' Channel Advisor inventory record does exist
                                                            rowImportItems.Item("Import") = False
                                                            rowImportItems.Item("AlreadyImported") = True
                                                            rowImportItems.Item("ItemCode") = ImportedProduct(2)

                                                        Else
                                                            ' no Channel Advisor inventory record, so SKU exists in InventoryItem table only
                                                            rowImportItems.Item("SKUExists") = True
                                                            rowImportItems.Item("ItemCode") = ImportedProduct(2)
                                                        End If
                                                        Exit For

                                                    End If
                                                Next
                                            End If

                                            rowImportItems.Item("ItemName") = m_ChanAdvImportRule.GetXMLElementText(XMLItemTemp, "InventoryItemResponse/Title")
                                            If XMLItemTemp.XPathSelectElement("InventoryItemResponse/VariationInfo") IsNot Nothing Then
                                                rowImportItems.Item("SourceType") = "Matrix"
                                            Else
                                                rowImportItems.Item("SourceType") = "Normal"
                                            End If
                                            rowImportItems.Item("ImportAsKit") = False
                                            ItemsForImport.Rows.Add(rowImportItems)

                                            ' check for duplicate/blank SKU
                                            If rowImportItems.Item("ItemSKU").ToString.Trim = "" Then
                                                rowImportItems.Item("Import") = False
                                                rowImportItems.Item("SKUError") = True
                                                rowImportItems.SetColumnError("ItemSKU", "Cannot import blank SKU")
                                            Else
                                                ' now check for duplicates - ignore last row
                                                For iLoop = 0 To ItemsForImport.Rows.Count - 2
                                                    If ItemsForImport.Rows(iLoop).Item("ItemSKU").ToString.Trim.ToLower = rowImportItems.Item("ItemSKU").ToString.Trim.ToLower Then
                                                        rowImportItems.SetColumnError("ItemSKU", "Duplicate SKU")
                                                        rowImportItems.Item("Import") = False
                                                        rowImportItems.Item("SKUError") = True
                                                        ItemsForImport.Rows(iLoop).SetColumnError("ItemSKU", "Duplicate SKU")
                                                        ItemsForImport.Rows(iLoop).Item("Import") = False
                                                        ItemsForImport.Rows(iLoop).Item("SKUError") = True
                                                    End If
                                                Next
                                            End If

                                        Next
                                        m_ProductsAlreadyImported = GetInventoryItemsImportedCount()
                                        If ItemsForImport.Rows.Count > 0 Then
                                            bReturnValue = True
                                        End If

                                    Else
                                        Exit Do
                                    End If

                                ElseIf m_ChanAdvImportRule.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/GetFilteredInventoryItemListResponse/GetFilteredInventoryItemListResult/Status", XMLNSManCAItems) = "Failure" Then
                                    m_LastError = "Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & " was " & m_ChanAdvImportRule.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/GetFilteredInventoryItemListResponse/GetFilteredInventoryItemListResult/Message", XMLNSManCAItems)

                                Else
                                    m_LastError = "Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & " not a success message - " & strReturn
                                    Exit Do
                                End If

                            Else
                                m_LastError = "No Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL
                                Exit Do
                            End If

                        Catch ex As Exception
                            m_LastError = "Unable to poll Channel Advisor for product list at " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & " - " & ex.Message & ", " & ex.StackTrace

                        Finally
                            If Not postStream Is Nothing Then postStream.Close()
                            If Not WebResponse Is Nothing Then WebResponse.Close()

                        End Try
                        If Cancel Then
                            Return False
                        End If
                        iPageFilter = iPageFilter + 1
                        If iPageFilter > 1000 Then
                            m_LastError = "Inventory Service has returned more then 1000 pages of Inventory Items from " & CHANNEL_ADVISOR_ORDER_SERVICE_URL
                            Exit Do

                        End If
                    Loop
                    If bInstanceMatched Then Exit For
                Else
                    m_LastError = "Channel Advisor Account not found."
                End If
        If Cancel Then
            Return False
        End If
            Next
        End If
        Return bReturnValue

    End Function
#End Region

#Region " ImportChannelAdvProducts "
    Public Sub ImportChannelAdvProducts(ByVal ChannelAdvisorAccountID As String, ByRef ItemsForImport As DataTable, ByRef Cancel As Boolean, _
        ByRef NoOfProductsImported As Integer, ByRef NoOfProductsSkipped As Integer)
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

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLInventoryItem As XDocument, XMLAttributes As XDocument
        Dim XMLNSManCAItems As System.Xml.XmlNamespaceManager, XMLNameTabCAItems As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim strErrorDetails As String, strHomeCurrency As String, strHomeLanguage As String
        Dim strSubmit As String, strReturn As String, strProductType As String
        Dim iChannelAdvAccountCount As Integer, iItemLoop As Integer, bInstanceMatched As Boolean

        Try
            m_LastError = ""
            m_ImportLimitReached = False
            m_ImportLog = ""
            If m_ChanAdvImportRule.IsActivated Then
                strErrorDetails = ""
                m_ProductsAlreadyImported = GetInventoryItemsImportedCount()
                If Not m_ChanAdvImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                    strHomeCurrency = Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                    strHomeLanguage = Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
                    XMLConfig = XDocument.Parse(Trim(m_ChanAdvImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)
                    iChannelAdvAccountCount = m_ChanAdvImportRule.GetXMLElementListCount(XMLNodeList)
                    For Each XmlNode In XmlNodeList
                        XMLTemp = XDocument.Parse(XmlNode.ToString)
                        If iChannelAdvAccountCount = 1 Then
                            bInstanceMatched = True
                        ElseIf m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) = ChannelAdvisorAccountID Then
                            bInstanceMatched = True
                        End If
                        If bInstanceMatched Then

                            NewItemDataset = New Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
                            NewItemFacade = New Interprise.Facade.Inventory.ItemDetailFacade(NewItemDataset, Interprise.Framework.Base.Shared.Enum.TransactionType.InventoryItem)

                            For iItemLoop = 0 To ItemsForImport.Rows.Count - 1
                                ' is Import box checked and not already imported ?
                                If CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And Not CBool(ItemsForImport.Rows(iItemLoop).Item("AlreadyImported")) And _
                                    Not CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) Then
                                    ' yes,  get product details from Channel Advisor
                                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                    strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/""><soapenv:Header>"
                                    If m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY) <> "" Then
                                        strSubmit = strSubmit & "<web:APICredentials><web:DeveloperKey>" & m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY)
                                        strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_PWD)
                                        strSubmit = strSubmit & "</web:Password></web:APICredentials>"
                                    Else
                                        strSubmit = strSubmit & "<web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY & "</web:DeveloperKey>"
                                        strSubmit = strSubmit & "<web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD & "</web:Password></web:APICredentials>"
                                    End If
                                    strSubmit = strSubmit & "</soapenv:Header><soapenv:Body><web:GetInventoryItemList><web:accountID>" & ChannelAdvisorAccountID & "</web:accountID>"
                                    strSubmit = strSubmit & "<web:skuList><web:string>" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & "</web:string>"
                                    strSubmit = strSubmit & "</web:skuList></web:GetInventoryItemList></soapenv:Body></soapenv:Envelope>"

                                    Try
                                        WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_INVENTORY_SERVICE_URL)
                                        WebSubmit.Method = "POST"
                                        WebSubmit.ContentType = "text/xml; charset=utf-8"
                                        WebSubmit.ContentLength = strSubmit.Length
                                        WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/GetInventoryItemList")
                                        WebSubmit.Timeout = 30000

                                        byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                                        ' send to Channel Advisor
                                        postStream = WebSubmit.GetRequestStream()
                                        postStream.Write(byteData, 0, byteData.Length)

                                        WebResponse = WebSubmit.GetResponse
                                        reader = New StreamReader(WebResponse.GetResponseStream())
                                        strReturn = reader.ReadToEnd()

                                        If strReturn <> "" Then
                                            Try
                                                ' had difficulty getting XPath to read XML with this name space present so remove it
                                                XMLInventoryItem = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))

                                            Catch ex As Exception
                                                m_LastError = "Unable to read Channel Advisor Product Details for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " due to XML error - " & ex.Message.Replace(vbCrLf, "")
                                                m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as unable to read Channel Advisor Product Details due to XML error - " & ex.Message & vbCrLf
                                                NoOfProductsSkipped = NoOfProductsSkipped + 1

                                            End Try

                                            ' was response a success ?
                                            XMLNameTabCAItems = New System.Xml.NameTable
                                            XMLNSManCAItems = New System.Xml.XmlNamespaceManager(XMLNameTabCAItems)
                                            XMLNSManCAItems.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/")

                                            If m_ChanAdvImportRule.GetXMLElementText(XMLInventoryItem, "soap:Envelope/soap:Body/GetInventoryItemListResponse/GetInventoryItemListResult/Status", XMLNSManCAItems) = "Success" Then
                                                ' yes, get Inventory Attribute list

                                                strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                                                strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/""><soapenv:Header>"
                                                If m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY) <> "" Then
                                                    strSubmit = strSubmit & "<web:APICredentials><web:DeveloperKey>" & m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY)
                                                    strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & m_ChanAdvImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_PWD)
                                                    strSubmit = strSubmit & "</web:Password></web:APICredentials>"
                                                Else
                                                    strSubmit = strSubmit & "<web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY & "</web:DeveloperKey>"
                                                    strSubmit = strSubmit & "<web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD & "</web:Password></web:APICredentials>"
                                                End If
                                                strSubmit = strSubmit & "</soapenv:Header><soapenv:Body><web:GetInventoryItemAttributeList><web:accountID>" & ChannelAdvisorAccountID
                                                strSubmit = strSubmit & "</web:accountID><web:sku>" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString
                                                strSubmit = strSubmit & "</web:sku></web:GetInventoryItemAttributeList></soapenv:Body></soapenv:Envelope>"

                                                Try
                                                    WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_INVENTORY_SERVICE_URL)
                                                    WebSubmit.Method = "POST"
                                                    WebSubmit.ContentType = "text/xml; charset=utf-8"
                                                    WebSubmit.ContentLength = strSubmit.Length
                                                    WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/GetInventoryItemAttributeList")
                                                    WebSubmit.Timeout = 30000

                                                    byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                                                    ' send to Channel Advisor
                                                    postStream = WebSubmit.GetRequestStream()
                                                    postStream.Write(byteData, 0, byteData.Length)

                                                    WebResponse = WebSubmit.GetResponse
                                                    reader = New StreamReader(WebResponse.GetResponseStream())
                                                    strReturn = reader.ReadToEnd()

                                                    If strReturn <> "" Then
                                                        Try
                                                            ' had difficulty getting XPath to read XML with this name space present so remove it
                                                            XMLAttributes = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))

                                                        Catch ex As Exception
                                                            m_LastError = "Unable to read Channel Advisor Product Details for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " due to XML error - " & ex.Message.Replace(vbCrLf, "")
                                                            m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as unable to read Channel Advisor Product Details due to XML error - " & ex.Message & vbCrLf
                                                            NoOfProductsSkipped = NoOfProductsSkipped + 1

                                                        End Try

                                                        If m_ChanAdvImportRule.GetXMLElementText(XMLAttributes, "soap:Envelope/soap:Body/GetInventoryItemAttributeListResponse/GetInventoryItemAttributeListResult/Status", XMLNSManCAItems) = "Success" Then
                                                            ' yes, process data
                                                            If ItemsForImport.Rows(iItemLoop).Item("SourceType").ToString = "Normal" And Not CBool(ItemsForImport.Rows(iItemLoop).Item("ImportAsKit")) Then
                                                                strProductType = ITEM_DEFAULT_CLASS_STOCK

                                                            ElseIf ItemsForImport.Rows(iItemLoop).Item("SourceType").ToString = "Normal" And CBool(ItemsForImport.Rows(iItemLoop).Item("ImportAsKit")) Then
                                                                strProductType = ITEM_DEFAULT_CLASS_KIT

                                                            Else
                                                                strProductType = ITEM_DEFAULT_CLASS_MATRIX_GROUP
                                                            End If
                                                            If CreateItem(XMLInventoryItem, XMLAttributes, XMLNSManCAItems, ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString, strHomeLanguage, ChannelAdvisorAccountID, strProductType) Then
                                                                If SaveItem(ChannelAdvisorAccountID, NoOfProductsImported, NoOfProductsSkipped) Then
                                                                    m_ImportLog = m_ImportLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                                                    m_ProductsAlreadyImported += 1
                                                                End If
                                                            End If

                                                        Else
                                                            m_LastError = "Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & " not a success message - " & strReturn
                                                            m_ImportLog = m_ImportLog & "Failed to import SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " as unable to read product attributes from Channel Advisor" & vbCrLf
                                                        End If

                                                    Else
                                                        m_LastError = "No Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & " for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString
                                                        m_ImportLog = m_ImportLog & "Failed to import SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " as unable to read product attributes from Channel Advisor" & vbCrLf
                                                    End If

                                                Catch ex As Exception
                                                    m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
                                                    m_ImportLog = m_ImportLog & "Failed to import SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " as unable to read product attributes from Channel Advisor" & vbCrLf

                                                End Try

                                            Else
                                                m_LastError = "Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & " not a success message - " & strReturn
                                                m_ImportLog = m_ImportLog & "Failed to import SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " as unable to read product details from Channel Advisor" & vbCrLf

                                            End If
                                        Else
                                            m_LastError = "No Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & "for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString
                                            m_ImportLog = m_ImportLog & "Failed to import SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " as unable to read product details from Channel Advisor" & vbCrLf
                                        End If

                                    Catch ex As Exception
                                        m_LastError = "Unable to read product details from Channel Advisor at " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL & " - " & ex.Message & ", " & ex.StackTrace
                                        m_ImportLog = m_ImportLog & "Failed to import SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " as unable to read product details from Channel Advisor" & vbCrLf

                                    Finally
                                        If Not postStream Is Nothing Then postStream.Close()
                                        If Not WebResponse Is Nothing Then WebResponse.Close()

                                    End Try
                                End If
                                If m_ChanAdvImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                                    m_LastError = strErrorDetails
                                    m_ImportLimitReached = True
                                    Exit For
                                End If

                                NewItemDataset.Clear()
                                m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.Clear()
                                m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.Clear()

                            Next
                            If m_ChanAdvImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                                m_LastError = strErrorDetails
                                m_ImportLimitReached = True
                                m_ImportLog = m_ImportLog & strErrorDetails & vbCrLf
                                Exit For
                            End If
                            NewItemFacade.Dispose()
                            NewItemDataset.Dispose()
                            Exit For

                        Else
                            m_LastError = "Channel Advisor Account not found."
                            m_ImportLog = m_ImportLog & "Cannot connect to Channel Advisor" & vbCrLf
                        End If
                    Next
                Else
                    m_LastError = strErrorDetails
                    m_ImportLimitReached = True
                End If
            End If

        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace

        End Try

    End Sub
#End Region

#Region " CreateItem "
    Private Function CreateItem(ByRef XMLProduct As XDocument, ByRef XMLAttributes As XDocument, ByVal XMLNSManCAItems As System.Xml.XmlNamespaceManager, _
        ByVal SKUToUse As String, ByVal HomeLanguage As String, ByVal AccountID As String, ByVal ItemClassCode As String) As Boolean
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

        Dim rowItemKit As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway.InventoryKitRow
        Dim rowChannelAdvDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryChannelAdvDetails_DEV000221Row
        Dim rowChannelAdvTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryChannelAdvTagDetails_DEV000221Row
        Dim rowManufacturerSource As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemManufacturerSourceID_DEV000221Row
        Dim rowAmazonASIN As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryAmazonASIN_DEV000221Row
        Dim rowManufacturerItems(0) As System.Data.DataRow, XMLTemp As XDocument
        Dim XMLAttributeList As System.Collections.Generic.IEnumerable(Of XElement), XMLAttribute As XElement
        Dim strItemCode As String, strManufacturerCode As String, strSourceManufacturerCode As String
        Dim strTemp As String, strMessage As String, strTagName As String, strXMLPath As String
        Dim iLoop As Integer, bISItemExists As Boolean, bCreateTagRecord As Boolean

        ' check Item Name doesn't already exist
        strItemCode = Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & SKUToUse & "'")
        If strItemCode <> "" Then
            bISItemExists = True
            strTemp = Me.GetField("ItemCode_DEV000221", "InventoryChannelAdvDetails_DEV000221", "ItemCode_DEV000221 = '" & strItemCode & "'")
            If strTemp <> "" Then
                strMessage = "Inventory Item with SKU " & SKUToUse & " already imported - check for duplicated SKUs"
                m_LastError = strMessage
                m_ImportLog = m_ImportLog & strMessage & vbCrLf
                Return False
            End If
        End If
        If bISItemExists Then
            ' load existing item
            NewItemFacade.LoadDataSet(New String()() {New String() {NewItemDataset.InventoryItem.TableName, GETINVENTORYITEM, AT_ITEMCODE, strItemCode}, _
                New String() {NewItemDataset.InventoryItemDescription.TableName, READINVENTORYITEMDESCRIPTION, AT_ITEMCODE, strItemCode, _
                Interprise.Framework.Base.Shared.Const.AT_LANGUAGECODE, HomeLanguage}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        Else
            ' create default item records
            NewItemFacade.AddItem(TEMPORARY_DOCUMENTCODE, ITEM_DEFAULT_CLASS_STOCK, ITEM_TYPE_STOCK)
            NewItemFacade.LanguageCode = HomeLanguage
            NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_STOCK)
            NewItemFacade.AddWebOption()
        End If
        ' make sure there are no records left from previous item import
        m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.Clear()
        m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.Clear()

        Select Case ItemClassCode
            Case ITEM_DEFAULT_CLASS_STOCK
                NewItemFacade.ApplyItemClassTemplate(ItemClassCode)

            Case ITEM_DEFAULT_CLASS_KIT
                NewItemFacade.ApplyItemClassTemplate(ItemClassCode)
                NewItemDataset.EnforceConstraints = False
                NewItemDataset.InventoryMatrixGroup.Clear()
                NewItemDataset.InventoryAttribute.Clear()
                NewItemDataset.InventoryAttributeValue.Clear()
                NewItemDataset.InventoryAssembly.Clear()
                NewItemDataset.InventoryKit.Clear()
                NewItemDataset.InventoryAssemblyDetailView.Clear()
                NewItemDataset.InventoryItemPricingDetail.Clear()
                NewItemDataset.EnforceConstraints = True
                rowItemKit = NewItemDataset.InventoryKit.NewInventoryKitRow
                rowItemKit.ItemKitCode = NewItemDataset.InventoryItem(0).ItemCode
                rowItemKit.PricingType = KIT_DISPLAY_ITEM_PRICE
                NewItemDataset.InventoryKit.AddInventoryKitRow(rowItemKit)
                NewItemDataset.InventoryItem(0).XMLPackage = XML_PACKAGE_KITPRODUCT
                NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_KIT

                NewKitDataset = New Interprise.Framework.Inventory.DatasetGateway.ItemKitDatasetGateway
                NewKitFacade = New Interprise.Facade.Inventory.ItemKitFacade(NewKitDataset)

            Case ITEM_DEFAULT_CLASS_MATRIX_GROUP
                NewItemFacade.ApplyItemClassTemplate(ItemClassCode)
                NewItemDataset.EnforceConstraints = False
                NewItemDataset.InventoryMatrixGroup.Clear()
                NewItemDataset.InventoryAttribute.Clear()
                NewItemDataset.InventoryAttributeValue.Clear()
                NewItemDataset.InventoryAssembly.Clear()
                NewItemDataset.InventoryKit.Clear()
                NewItemDataset.InventoryAssemblyDetailView.Clear()
                NewItemDataset.InventoryItemPricingDetail.Clear()
                NewItemDataset.EnforceConstraints = True
                NewItemDataset.InventoryItem(0).XMLPackage = XML_PACKAGE_MATRIXPRODUCT
                NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP

                If SKUToUse.Length > CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryMatrixGroup' AND ColumnName = 'Prefix'")) Then
                    strMessage = "Import of SKU " & SKUToUse & " aborted as base SKU exceed maximum size for a Matrix Group"
                    m_LastError = strMessage
                    m_ImportLog = m_ImportLog & strMessage & vbCrLf
                    Return False
                End If
                NewItemDataset.InventoryMatrixGroup(0).Prefix = SKUToUse

        End Select

        rowChannelAdvDetails = m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.NewInventoryChannelAdvDetails_DEV000221Row
        rowChannelAdvDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
        rowChannelAdvDetails.Publish_DEV000221 = True
        rowChannelAdvDetails.SellingPrice_DEV000221 = 0
        rowChannelAdvDetails.AccountID_DEV000221 = AccountID
        rowChannelAdvDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
        rowChannelAdvDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
        rowChannelAdvDetails.FromImportWizard_DEV000221 = True
        rowChannelAdvDetails.QtyPublishingType_DEV000221 = m_QuantityPublishingType
        rowChannelAdvDetails.QtyPublishingValue_DEV000221 = CInt(m_QuantityPublishingValue)
        rowChannelAdvDetails.TotalQtyWhenLastPublished_DEV000221 = 0
        rowChannelAdvDetails.QtyLastPublished_DEV000221 = 0
        m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.AddInventoryChannelAdvDetails_DEV000221Row(rowChannelAdvDetails)

        NewItemDataset.InventoryItem(0).ItemName = SKUToUse
        NewItemDataset.InventoryItem(0).ItemDescription = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Title", XMLNSManCAItems)
        For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
            NewItemDataset.InventoryItemDescription(iLoop).ItemDescription = NewItemDataset.InventoryItem(0).ItemDescription
        Next
        rowChannelAdvDetails.ProductSubTitle_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Subtitle", XMLNSManCAItems)
        rowChannelAdvDetails.ProductShortDescription_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/ShortDescription", XMLNSManCAItems)
        If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Description", XMLNSManCAItems).Length > 1000 Then
            NewItemDataset.InventoryItem(0).ExtendedDescription = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Description", XMLNSManCAItems).Substring(0, 1000)
        Else
            NewItemDataset.InventoryItem(0).ExtendedDescription = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Description", XMLNSManCAItems)
        End If
        For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
            NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription = NewItemDataset.InventoryItem(0).ExtendedDescription
        Next
        For iLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1
            NewItemDataset.InventoryItemWebOptionDescription(iLoop).WebDescription = NewItemDataset.InventoryItem(0).ExtendedDescription
        Next
        rowChannelAdvDetails.Condition_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Condition", XMLNSManCAItems)
        rowChannelAdvDetails.Warranty_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Warranty", XMLNSManCAItems)
        rowChannelAdvDetails.FlagStyle_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/FlagStyle", XMLNSManCAItems)
        rowChannelAdvDetails.FlagDescription_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/FlagDescription", XMLNSManCAItems)
        rowChannelAdvDetails.IsBlocked_DEV000221 = CBool(IIf(m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/IsBlocked", XMLNSManCAItems).ToLower = "true", True, False))
        rowChannelAdvDetails.IsBlockedComment_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/BlockComment", XMLNSManCAItems)
        If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/PriceInfo/TakeItPrice", XMLNSManCAItems) <> "" Then
            rowChannelAdvDetails.SellingPrice_DEV000221 = CDec(m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/PriceInfo/TakeItPrice", XMLNSManCAItems))
        End If

        For iLoop = 0 To 1
            bCreateTagRecord = False
            Select Case iLoop
                Case 0
                    strTagName = "ISBN"
                    strXMLPath = ChannelAdvProductXMLPath & "/ISBN"
                    If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems) <> "" Then
                        bCreateTagRecord = True
                    End If
                Case 1
                    strTagName = "Brand"
                    strXMLPath = ChannelAdvProductXMLPath & "/Brand"
                    If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems) <> "" Then
                        bCreateTagRecord = True
                    End If
            End Select
            If bCreateTagRecord Then
                rowChannelAdvTagDetails = m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.NewInventoryChannelAdvTagDetails_DEV000221Row
                rowChannelAdvTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                rowChannelAdvTagDetails.AccountID_DEV000221 = AccountID
                rowChannelAdvTagDetails.TagName_DEV000221 = strTagName
                rowChannelAdvTagDetails.TagLocation_DEV000221 = 1
                rowChannelAdvTagDetails.TagMemoField_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems)
                m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.AddInventoryChannelAdvTagDetails_DEV000221Row(rowChannelAdvTagDetails)
            End If
        Next

        For iLoop = 0 To 4
            bCreateTagRecord = False
            Select Case iLoop
                Case 0
                    strTagName = "RetailPrice"
                    strXMLPath = ChannelAdvProductXMLPath & "/PriceInfo/RetailPrice"
                    If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems) <> "" Then
                        bCreateTagRecord = True
                    End If
                Case 1
                    strTagName = "StartingPrice"
                    strXMLPath = ChannelAdvProductXMLPath & "/PriceInfo/StartingPrice"
                    If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems) <> "" Then
                        bCreateTagRecord = True
                    End If
                Case 2
                    strTagName = "ReservePrice"
                    strXMLPath = ChannelAdvProductXMLPath & "/PriceInfo/ReservePrice"
                    If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems) <> "" Then
                        bCreateTagRecord = True
                    End If
                Case 3
                    strTagName = "SecondChanceOfferPrice"
                    strXMLPath = ChannelAdvProductXMLPath & "/PriceInfo/SecondChanceOfferPrice"
                    If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems) <> "" Then
                        bCreateTagRecord = True
                    End If
                Case 4
                    strTagName = "StorePrice"
                    strXMLPath = ChannelAdvProductXMLPath & "/PriceInfo/StorePrice"
                    If m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems) <> "" Then
                        bCreateTagRecord = True
                    End If
            End Select
            If bCreateTagRecord Then
                rowChannelAdvTagDetails = m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.NewInventoryChannelAdvTagDetails_DEV000221Row
                rowChannelAdvTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                rowChannelAdvTagDetails.AccountID_DEV000221 = AccountID
                rowChannelAdvTagDetails.TagName_DEV000221 = strTagName
                rowChannelAdvTagDetails.TagLocation_DEV000221 = 2
                rowChannelAdvTagDetails.TagMemoField_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, strXMLPath, XMLNSManCAItems)
                m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.AddInventoryChannelAdvTagDetails_DEV000221Row(rowChannelAdvTagDetails)
            End If
        Next

        strSourceManufacturerCode = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/Manufacturer", XMLNSManCAItems)
        If strSourceManufacturerCode <> "" Then
            If Len(strSourceManufacturerCode) > 50 Then
                strSourceManufacturerCode = strSourceManufacturerCode.Substring(0, 50)
            End If
            strManufacturerCode = Me.GetField("ManufacturerCode_DEV000221", "SystemManufacturerSourceID_DEV000221", _
                "SourceCode_DEV000221 = '" & CHANNEL_ADVISOR_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & AccountID & _
                "' AND SourceManufacturerCode_DEV000221 = '" & strSourceManufacturerCode & "'")
            If strManufacturerCode = "" Then
                strTemp = Me.GetField("SELECT ISNULL(MAX(ManufacturerCode_DEV000221), '0') FROM SystemManufacturerSourceID_DEV000221 WHERE SourceCode_DEV000221 = '" & _
                    CHANNEL_ADVISOR_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & AccountID & "'", System.Data.CommandType.Text, Nothing)
                strManufacturerCode = "CHADV" & Microsoft.VisualBasic.Right("000000" & CInt(Microsoft.VisualBasic.Right(strTemp, 6)) + 1, 6)
                rowManufacturerSource = m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.FindByManufacturerCode_DEV000221SourceCode_DEV000221AccountOrInstanceID_DEV000221(strTemp, CHANNEL_ADVISOR_SOURCE_CODE, AccountID)
                If rowManufacturerSource Is Nothing Then
                    NewManufacturerDataset = New Interprise.Framework.Inventory.DatasetGateway.SystemManager.ManufacturerDatasetGateway
                    NewManufacturerFacade = New Interprise.Facade.Inventory.SystemManager.ManufacturerFacade(NewManufacturerDataset)

                    NewManufacturerFacade.AddManufacturer(strManufacturerCode)
                    For iLoop = 0 To NewManufacturerDataset.SystemManufacturerDescription.Count - 1
                        NewManufacturerDataset.SystemManufacturerDescription(iLoop).ManufacturerCode = NewManufacturerDataset.SystemManufacturer(0).ManufacturerCode
                        NewManufacturerDataset.SystemManufacturerDescription(iLoop).Description = "Channel Advisor " & strSourceManufacturerCode
                    Next
                    NewManufacturerFacade.UpdateDataSet(NewManufacturerFacade.CommandSet(), Interprise.Framework.Base.Shared.TransactionType.InventoryManufacturer, "Add Channel Advisor Manufacturer", False)
                    rowManufacturerSource = m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.NewSystemManufacturerSourceID_DEV000221Row
                    rowManufacturerSource.ManufacturerCode_DEV000221 = strManufacturerCode
                    rowManufacturerSource.SourceCode_DEV000221 = CHANNEL_ADVISOR_SOURCE_CODE
                    rowManufacturerSource.AccountOrInstanceID_DEV000221 = AccountID
                    rowManufacturerSource.SourceManufacturerCode_DEV000221 = strSourceManufacturerCode
                    m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.AddSystemManufacturerSourceID_DEV000221Row(rowManufacturerSource)
                End If
            End If
            NewItemDataset.InventoryItem(0).ManufacturerCode = strManufacturerCode
        End If

        strTemp = m_ChanAdvImportRule.GetXMLElementText(XMLProduct, ChannelAdvProductXMLPath & "/ASIN", XMLNSManCAItems)
        If strTemp <> "" AndAlso IsNumeric(strTemp) Then
            rowAmazonASIN = m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.NewInventoryAmazonASIN_DEV000221Row
            rowAmazonASIN.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            rowAmazonASIN.ASIN_DEV000221 = strTemp
            m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.AddInventoryAmazonASIN_DEV000221Row(rowAmazonASIN)

        ElseIf strTemp <> "" Then
            ' ASIN field is not numeric, can't be a real ASIN
            rowChannelAdvTagDetails = m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.NewInventoryChannelAdvTagDetails_DEV000221Row
            rowChannelAdvTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            rowChannelAdvTagDetails.AccountID_DEV000221 = AccountID
            rowChannelAdvTagDetails.TagName_DEV000221 = "ASIN"
            rowChannelAdvTagDetails.TagLocation_DEV000221 = 1
            rowChannelAdvTagDetails.TagMemoField_DEV000221 = strTemp
            m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.AddInventoryChannelAdvTagDetails_DEV000221Row(rowChannelAdvTagDetails)

        End If

        XMLAttributeList = XMLAttributes.XPathSelectElements("soap:Envelope/soap:Body/GetInventoryItemAttributeListResponse/GetInventoryItemAttributeListResult/ResultData/AttributeInfo", XMLNSManCAItems)
        If m_ChanAdvImportRule.GetXMLElementListCount(XMLAttributeList) > 0 Then
            For Each XMLAttribute In XMLAttributeList
                XMLTemp = XDocument.Parse(XMLAttribute.ToString)

                rowChannelAdvTagDetails = m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.NewInventoryChannelAdvTagDetails_DEV000221Row
                rowChannelAdvTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                rowChannelAdvTagDetails.AccountID_DEV000221 = AccountID
                rowChannelAdvTagDetails.TagName_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLTemp, "AttributeInfo/Name")
                rowChannelAdvTagDetails.TagLocation_DEV000221 = 2
                rowChannelAdvTagDetails.TagMemoField_DEV000221 = m_ChanAdvImportRule.GetXMLElementText(XMLTemp, "AttributeInfo/Value")
                m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.AddInventoryChannelAdvTagDetails_DEV000221Row(rowChannelAdvTagDetails)

            Next
        End If

        Return True

    End Function
#End Region

#Region " SaveItem "
    Private Function SaveItem(ByVal AccountID As String, ByRef NoOfProductsImported As Integer, ByRef NoOfProductsSkipped As Integer) As Boolean
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

        Dim strErrorDetails As String, strTemp As String
        Dim iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer

        NewItemFacade.IsFinished = True
        If NewItemFacade.UpdateDataSet(NewItemFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
            NewItemFacade.GenerateInventoryStockTotal("admin")
            NewItemFacade.UpdateStockTotal()
            If NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                'NewItemFacade.GenerateWebOptionDetail() **** TJS IS6 to do
            End If
            m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221(0).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            For iRowLoop = 0 To m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.Count - 1
                m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221(iRowLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            Next
            For iRowLoop = 0 To m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.Count - 1
                m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221(iRowLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            Next

            If Me.UpdateDataSet(New String()() {New String() {m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.TableName, _
                    "CreateInventoryChannelAdvDetails_DEV000221", "UpdateInventoryChannelAdvDetails_DEV000221", "DeleteInventoryChannelAdvDetails_DEV000221"}, _
                New String() {m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.TableName, "CreateInventoryChannelAdvTagDetails_DEV000221", _
                    "UpdateInventoryChannelAdvTagDetails_DEV000221", "DeleteInventoryChannelAdvTagDetails_DEV000221"}, _
                New String() {m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.TableName, "CreateInventoryAmazonASIN_DEV000221", _
                    "UpdateInventoryAmazonASIN_DEV000221", "DeleteInventoryAmazonASIN_DEV000221"}, _
                New String() {m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.TableName, "CreateSystemManufacturerSourceID_DEV000221", _
                    "UpdateSystemManufacturerSourceID_DEV000221", "DeleteSystemManufacturerSourceID_DEV000221"}}, _
                Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                NoOfProductsImported = NoOfProductsImported + 1
                Return True

            Else
                ' get error details
                strErrorDetails = ""
                For iRowLoop = 0 To m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.Columns.Count - 1
                        If m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.TableName & _
                                "." & m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ChanAdvImportDataset.InventoryChannelAdvDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                For iRowLoop = 0 To m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.Columns.Count - 1
                        If m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.TableName & _
                                "." & m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ChanAdvImportDataset.InventoryChannelAdvTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                For iRowLoop = 0 To m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.Columns.Count - 1
                        If m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.TableName & _
                                "." & m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ChanAdvImportDataset.SystemManufacturerSourceID_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                For iRowLoop = 0 To m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.Columns.Count - 1
                        If m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.TableName & _
                                "." & m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ChanAdvImportDataset.InventoryAmazonASIN_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                m_LastError = strErrorDetails
                m_ImportLog = m_ImportLog & "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted - " & strErrorDetails & vbCrLf
                NoOfProductsSkipped = NoOfProductsSkipped + 1

            End If
            strTemp = "UPDATE LerrynImportExportInventoryActionStatus_DEV000221 SET ActionComplete_DEV000221 = 1 "
            strTemp = strTemp & "WHERE ItemCode_DEV000221 = '" & NewItemDataset.InventoryItem(0).ItemCode
            strTemp = strTemp & "' AND SourceCode_DEV000221 = '" & CHANNEL_ADVISOR_SOURCE_CODE
            strTemp = strTemp & "' AND StoreMerchantID_DEV000221 = '" & AccountID & "'"
            Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing)
        Else
            ' get error details
            strErrorDetails = ""
            For iTableLoop = 0 To NewItemFacade.CommandSet.Length - 1
                For iRowLoop = 0 To NewItemDataset.Tables(NewItemFacade.CommandSet(iTableLoop)(0)).Rows.Count - 1
                    For iColumnLoop = 0 To NewItemDataset.Tables(NewItemFacade.CommandSet(iTableLoop)(0)).Columns.Count - 1
                        If NewItemDataset.Tables(NewItemFacade.CommandSet(iTableLoop)(0)).Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & NewItemDataset.Tables(NewItemFacade.CommandSet(iTableLoop)(0)).TableName & _
                                "." & NewItemDataset.Tables(NewItemFacade.CommandSet(iTableLoop)(0)).Columns(iColumnLoop).ColumnName & ", " & _
                                NewItemDataset.Tables(NewItemFacade.CommandSet(iTableLoop)(0)).Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
            Next
            m_LastError = strErrorDetails
            m_ImportLog = m_ImportLog & "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted - " & strErrorDetails & vbCrLf
            NoOfProductsSkipped = NoOfProductsSkipped + 1

        End If

        Return False

    End Function
#End Region

#Region " GetInventoryItemsImportedCount "
    Private Function GetInventoryItemsImportedCount() As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function created 
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon to count
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return CInt(Me.GetField("SELECT (SELECT COUNT(*) FROM InventoryAmazonDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryASPStorefrontDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryChannelAdvDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryMagentoDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1)", CommandType.Text, Nothing)) ' TJS 05/07/12

    End Function
#End Region
#End Region
End Class
#End Region