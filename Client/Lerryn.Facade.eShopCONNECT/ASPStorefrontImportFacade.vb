' eShopCONNECT for Connected Business
' Module: ASPStorefrontImportFacade.vb
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
' Last Updated - 01 April 2014

Option Explicit On
Option Strict On

Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Base.Shared.StoredProcedures
Imports Interprise.Framework.Inventory.Shared.Const

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const

Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " ASPStorefrontImportFacade "
Public Class ASPStorefrontImportFacade
    Inherits Interprise.Facade.Base.BaseFacade
    Implements Interprise.Extendable.Base.Facade.IBaseInterface

#Region " Variables "
    Public Structure ProductType
        Public TypeID As String
        Public Description As String
    End Structure

    Private m_ProductTypeList As ProductType()
    Private m_ASPStorefrontImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_ASPStorefrontImportRule As Lerryn.Facade.ImportExport.ImportExportFacade
    Private m_ASPStorefrontCategories As Lerryn.Facade.ImportExport.ASPStorefrontConnector.CategoryList() ' TJS 29/03/11

    Private m_ProductsAlreadyImported As Integer = 0
    Private m_CreateISCategories As Boolean ' TJS 29/03/11
    Private m_ImportExtData1CustomField As String = "" ' TJS 29/03/11
    Private m_ImportExtData2CustomField As String = "" ' TJS 29/03/11
    Private m_ImportExtData3CustomField As String = "" ' TJS 29/03/11
    Private m_ImportExtData4CustomField As String = "" ' TJS 29/03/11
    Private m_ImportExtData5CustomField As String = "" ' TJS 29/03/11
    Private m_ImportLog As String ' TJS 29/03/11
    Private m_LastError As String ' TJS 02/12/11
    Private m_ImportLimitReached As Boolean ' TJS 02/12/11
    Private m_QuantityPublishingType As String ' TJS 02/12/11
    Private m_QuantityPublishingValue As Decimal ' TJS 02/12/11

    Private NewItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
    Private NewManufacturerFacade As Interprise.Facade.Inventory.SystemManager.ManufacturerFacade
    Private NewKitFacade As Interprise.Facade.Inventory.ItemKitFacade
    Private NewCategoryFacade As Interprise.Facade.Inventory.SystemManager.CategoryFacade ' TJS 29/03/11

    Private WithEvents NewItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private WithEvents NewManufacturerDataset As Interprise.Framework.Inventory.DatasetGateway.SystemManager.ManufacturerDatasetGateway
    Private WithEvents NewKitDataset As Interprise.Framework.Inventory.DatasetGateway.ItemKitDatasetGateway
    Private WithEvents NewCategoryDataset As Interprise.Framework.Inventory.DatasetGateway.SystemManager.CategoryDatasetGateway ' TJS 29/03/11
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return m_ASPStorefrontImportDataset
        End Get
    End Property
#End Region

#Region " CurrentBusinessRule "
    Public Overrides ReadOnly Property CurrentBusinessRule() As Interprise.Extendable.Base.Business.IBaseInterface
        Get
            Return m_ASPStorefrontImportRule
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

#Region " ProductTypeList "
    Public ReadOnly Property ProductTypeList() As ProductType()
        Get
            Return m_ProductTypeList
        End Get
    End Property
#End Region

#Region " ASPStorefrontCategories "
    Public Property ASPStorefrontCategories() As Lerryn.Facade.ImportExport.ASPStorefrontConnector.CategoryList() ' TJS 09/04/11
        Get
            Return m_ASPStorefrontCategories ' TJS 09/04/11
        End Get
        Set(ByVal value As Lerryn.Facade.ImportExport.ASPStorefrontConnector.CategoryList()) ' TJS 09/04/11
            m_ASPStorefrontCategories = value ' TJS 09/04/11
        End Set
    End Property
#End Region

#Region " CreateISCategories "
    Public Property CreateISCategories() As Boolean ' TJS 29/03/11
        Get
            Return m_CreateISCategories ' TJS 29/03/11
        End Get
        Set(ByVal value As Boolean) ' TJS 29/03/11
            m_CreateISCategories = value ' TJS 29/03/11
        End Set
    End Property
#End Region

#Region " QuantityPublishingType "
    Public Property QuantityPublishingType() As String ' TJS 02/12/11
        Get
            Return m_QuantityPublishingType ' TJS 02/12/11
        End Get
        Set(ByVal value As String) ' TJS 02/12/11
            m_QuantityPublishingType = value ' TJS 02/12/11
        End Set
    End Property
#End Region

#Region " QuantityPublishingValue "
    Public Property QuantityPublishingValue() As Decimal ' TJS 02/12/11
        Get
            Return m_QuantityPublishingValue ' TJS 02/12/11
        End Get
        Set(ByVal value As Decimal) ' TJS 02/12/11
            m_QuantityPublishingValue = value ' TJS 02/12/11
        End Set
    End Property
#End Region

#Region " ImportExtData1CustomField "
    Public Property ImportExtData1CustomField() As String ' TJS 29/03/11
        Get
            Return m_ImportExtData1CustomField ' TJS 29/03/11
        End Get
        Set(ByVal value As String) ' TJS 29/03/11
            m_ImportExtData1CustomField = value ' TJS 29/03/11
        End Set
    End Property
#End Region

#Region " ImportExtData2CustomField "
    Public Property ImportExtData2CustomField() As String ' TJS 29/03/11
        Get
            Return m_ImportExtData2CustomField ' TJS 29/03/11
        End Get
        Set(ByVal value As String) ' TJS 29/03/11
            m_ImportExtData2CustomField = value ' TJS 29/03/11
        End Set
    End Property
#End Region

#Region " ImportExtData3CustomField "
    Public Property ImportExtData3CustomField() As String ' TJS 29/03/11
        Get
            Return m_ImportExtData3CustomField ' TJS 29/03/11
        End Get
        Set(ByVal value As String) ' TJS 29/03/11
            m_ImportExtData3CustomField = value ' TJS 29/03/11
        End Set
    End Property
#End Region

#Region " ImportExtData4CustomField "
    Public Property ImportExtData4CustomField() As String ' TJS 29/03/11
        Get
            Return m_ImportExtData4CustomField ' TJS 29/03/11
        End Get
        Set(ByVal value As String) ' TJS 29/03/11
            m_ImportExtData4CustomField = value ' TJS 29/03/11
        End Set
    End Property
#End Region

#Region " ImportExtData5CustomField "
    Public Property ImportExtData5CustomField() As String ' TJS 29/03/11
        Get
            Return m_ImportExtData5CustomField ' TJS 29/03/11
        End Get
        Set(ByVal value As String) ' TJS 29/03/11
            m_ImportExtData5CustomField = value ' TJS 29/03/11
        End Set
    End Property
#End Region

#Region " ImportLog "
    Public ReadOnly Property ImportLog() As String ' TJS 29/03/11
        Get
            Return m_ImportLog ' TJS 29/03/11
        End Get
    End Property
#End Region

#Region " LastError "
    Public ReadOnly Property LastError() As String ' TJS 02/12/11
        Get
            Return m_LastError ' TJS 02/12/11
        End Get
    End Property
#End Region

#Region " ImportLimitReached "
    Public ReadOnly Property ImportLimitReached() As Boolean ' TJS 02/12/11
        Get
            Return m_ImportLimitReached ' TJS 02/12/11
        End Get
    End Property
#End Region
#End Region

#Region " Methods "

#Region " Constructor "
    Public Sub New(ByRef p_ASPStorefrontImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef p_ErrorNotification As Lerryn.Facade.ImportExport.ErrorNotification, ByVal p_BaseProductCode As String, ByVal p_BaseProductName As String) ' TJS 10/06/12
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
        m_ASPStorefrontImportDataset = p_ASPStorefrontImportDataset
        m_ASPStorefrontImportRule = New Lerryn.Facade.ImportExport.ImportExportFacade(m_ASPStorefrontImportDataset, p_ErrorNotification, p_BaseProductCode, p_BaseProductName) ' TJS 10/06/12
        MyBase.InitializeDataset()

        ' read all licences as we need base licence and all add-ons
        Me.LoadDataSet(New String()() {New String() {m_ASPStorefrontImportDataset.LerrynLicences_DEV000221.TableName, _
            "ReadLerrynLicences_DEV000221"}, New String() {m_ASPStorefrontImportDataset.System_DEV000221.TableName, _
            "ReadSystem_DEV000221"}, New String() {m_ASPStorefrontImportDataset.SystemCompanyInformation.TableName, _
            "ReadSystemCompanyInformation"}, New String() {m_ASPStorefrontImportDataset.LerrynImportExportConfig_DEV000221.TableName, _
            "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        m_ASPStorefrontImportRule.CheckActivation()

    End Sub
#End Region

#Region " GetASPStorefrontProductList "
    Public Function GetASPStorefrontProductList(ByVal ASPStorefrontSiteID As String, ByRef Cancel As Boolean, ByRef ItemsForImport As DataTable) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/03/11 | TJs             | 2011.0.04 | Modified to allow operation without connector activated
        ' 31/03/11 | TJS             | 2011.0.05 | Added Error column to cater for duplicate/blank SKU errors
        ' 04/11/11 | TJS             | 2011.1.10 | Modofied to cater for products where SKU is changed in ASPStorefront
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | Also replaced calls to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to cater for ASPStorefront returning GUID as uppercase when publishing inventory
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for non-stock items
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to cater for SKUChanged in other import facades
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple ASPStorefront sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ASPStorefrontConnection As Lerryn.Facade.ImportExport.ASPStorefrontConnector
        Dim XMLResponse As XDocument, XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement
        Dim colNewColumn As DataColumn, rowImportItems As System.Data.DataRow
        Dim strImportedProducts As String()()
        Dim strSubmit As String, strTemp As String, strASPStorefrontURL As String, strItemValue As String ' TJS 04/11/11
        Dim strASPStorefrontUser As String, strASPStorefrontPwd As String, iLoop As Integer
        Dim bInstanceMatched As Boolean, bASPStorefrontUseWSE3 As Boolean, bReturnValue As Boolean
        Dim iASPStorefrontInstanceCount As Integer

        bReturnValue = False
        m_LastError = "" ' TJS 02/12/11
        If m_ASPStorefrontImportRule.IsActivated Then ' TJS 29/03/11
            XMLConfig = XDocument.Parse(Trim(m_ASPStorefrontImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
            XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
            iASPStorefrontInstanceCount = m_ASPStorefrontImportRule.GetXMLElementListCount(XMLNodeList)
            For Each XMLNode In XMLNodeList
                XMLTemp = XDocument.Parse(XMLNode.ToString)
                If iASPStorefrontInstanceCount = 1 Then
                    bInstanceMatched = True
                ElseIf m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID) = ASPStorefrontSiteID Then
                    bInstanceMatched = True
                End If
                If bInstanceMatched Then
                    strASPStorefrontURL = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_URL)
                    strASPStorefrontUser = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_USER)
                    strASPStorefrontPwd = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_PASSWORD)
                    bASPStorefrontUseWSE3 = CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_USE_WSE3_AUTHENTICATION).ToUpper = "YES", True, False))
                    If strASPStorefrontURL <> "" And strASPStorefrontUser <> "" Then

                        strSubmit = "<AspDotNetStorefrontImport Verbose=""false"">" & vbCrLf & "<Query Name=""ProductList"" RowName=""Product"">"
                        strSubmit = strSubmit & "<SQL>" & vbCrLf & "<![CDATA[" & vbCrLf
                        strSubmit = strSubmit & "SELECT ProductID, ProductGUID, Name, SKU, ProductTypeID FROM Product WHERE Deleted = 0" & vbCrLf
                        strSubmit = strSubmit & "]]>" & vbCrLf & "</SQL>" & vbCrLf & "</Query>" & vbCrLf & "</AspDotNetStorefrontImport>"

                        ASPStorefrontConnection = New Lerryn.Facade.ImportExport.ASPStorefrontConnector
                        strTemp = ""

                        XMLResponse = ASPStorefrontConnection.SendXMLToASPStorefront(bASPStorefrontUseWSE3, strASPStorefrontURL, _
                            strASPStorefrontUser, strASPStorefrontPwd, strSubmit, strTemp)
                        ' any errors ?
                        If strTemp = "" Then
                            ' no, process product list
                            XMLItemList = XMLResponse.XPathSelectElements("AspDotNetStorefrontImportResult/Query/Product")

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
                            colNewColumn.Caption = "ASPDotNetStorefront Type"
                            colNewColumn.ColumnName = "SourceType"
                            colNewColumn.DataType = System.Type.GetType("System.String")
                            ItemsForImport.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            colNewColumn = New DataColumn
                            colNewColumn.Caption = "ASPDotNetStorefront Item GUID"
                            colNewColumn.ColumnName = "SourceItemID"
                            colNewColumn.DataType = System.Type.GetType("System.String")
                            ItemsForImport.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            ' this column is needed on Channel Advisor
                            colNewColumn = New DataColumn ' TJS 02/12/11
                            colNewColumn.Caption = "Not Used" ' TJS 02/12/11
                            colNewColumn.ColumnName = "ImportAsKit" ' TJS 02/12/11
                            colNewColumn.DataType = System.Type.GetType("System.Boolean") ' TJS 02/12/11
                            ItemsForImport.Columns.Add(colNewColumn) ' TJS 02/12/11
                            colNewColumn.Dispose() ' TJS 02/12/11

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

                            colNewColumn = New DataColumn ' TJS 31/03/11
                            colNewColumn.Caption = "SKU Error" ' TJS 31/03/11
                            colNewColumn.ColumnName = "SKUError" ' TJS 31/03/11
                            colNewColumn.DataType = System.Type.GetType("System.Boolean") ' TJS 31/03/11
                            ItemsForImport.Columns.Add(colNewColumn) ' TJS 31/03/11
                            colNewColumn.Dispose() ' TJS 31/03/11

                            colNewColumn = New DataColumn ' TJS 04/11/11
                            colNewColumn.Caption = "SKU Changed" ' TJS 04/11/11
                            colNewColumn.ColumnName = "SKUChanged" ' TJS 04/11/11
                            colNewColumn.DataType = System.Type.GetType("System.Boolean") ' TJS 04/11/11
                            ItemsForImport.Columns.Add(colNewColumn) ' TJS 04/11/11
                            colNewColumn.Dispose() ' TJS 04/11/11

                            colNewColumn = New DataColumn ' TJS 04/11/11
                            colNewColumn.Caption = "Item Code" ' TJS 04/11/11
                            colNewColumn.ColumnName = "ItemCode" ' TJS 04/11/11
                            colNewColumn.DataType = System.Type.GetType("System.String") ' TJS 04/11/11
                            ItemsForImport.Columns.Add(colNewColumn) ' TJS 04/11/11
                            colNewColumn.Dispose() ' TJS 04/11/11

                            ' this column is needed on Channel Advisor
                            colNewColumn = New DataColumn ' TJS 02/12/11
                            colNewColumn.Caption = "Not Used" ' TJS 02/12/11
                            colNewColumn.ColumnName = "ASIN" ' TJS 02/12/11
                            colNewColumn.DataType = System.Type.GetType("System.String") ' TJS 02/12/11
                            ItemsForImport.Columns.Add(colNewColumn) ' TJS 02/12/11
                            colNewColumn.Dispose() ' TJS 02/12/11

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

                            strImportedProducts = Me.GetRows(New String() {"ASPStorefrontProductGUID_DEV000221", "ItemName", "ItemCode"}, "InventoryItemASPStorefrontSummaryView_DEV000221") ' TJS 04/11/11
                            m_ProductsAlreadyImported = 0
                            For Each XMLItemNode In XMLItemList
                                XMLTemp = XDocument.Parse(XMLItemNode.ToString)
                                rowImportItems = ItemsForImport.NewRow
                                rowImportItems.Item("Import") = True
                                rowImportItems.Item("AlreadyImported") = False
                                rowImportItems.Item("SKUExists") = False
                                rowImportItems.Item("SKUError") = False ' TJS 31/03/11
                                rowImportItems.Item("SKUChanged") = False ' TJS 04/11/11
                                rowImportItems.Item("SourceIDChanged") = False ' TJS 09/08/13

                                strItemValue = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Product/productguid") ' TJS 04/11/11
                                rowImportItems.Item("SourceItemID") = strItemValue ' TJS 04/11/11
                                For Each ImportedProduct As String() In strImportedProducts
                                    If ImportedProduct(0) = strItemValue Then ' TJS 04/11/11
                                        rowImportItems.Item("Import") = False
                                        rowImportItems.Item("AlreadyImported") = True
                                        rowImportItems.Item("ItemCode") = ImportedProduct(2) ' TJS 04/11/11
                                        Exit For
                                    End If
                                Next

                                strItemValue = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Product/sku") ' TJS 04/11/11
                                rowImportItems.Item("ItemSKU") = strItemValue ' TJS 04/11/11
                                For Each ImportedProduct As String() In strImportedProducts
                                    If ImportedProduct(1) = strItemValue Then ' TJS 04/11/11
                                        If rowImportItems.IsNull("SourceItemID") OrElse "" & ImportedProduct(0) = "" OrElse _
                                            rowImportItems.Item("SourceItemID").ToString.ToUpper = ("" & ImportedProduct(0)).ToUpper Then ' TJS 04/11/11 TJS 24/02/12
                                            rowImportItems.Item("SKUExists") = True
                                            rowImportItems.Item("ItemCode") = ImportedProduct(2) ' TJS 04/11/11

                                        ElseIf rowImportItems.Item("SourceItemID").ToString.ToUpper <> ("" & ImportedProduct(0)).ToUpper Then ' TJS 04/11/11 TJS 24/02/12
                                            rowImportItems.Item("Import") = False ' TJS 04/11/11
                                            rowImportItems.Item("SKUChanged") = True ' TJS 04/11/11
                                            rowImportItems.Item("ItemCode") = ImportedProduct(2) ' TJS 04/11/11
                                        End If
                                        Exit For
                                    End If
                                Next

                                rowImportItems.Item("ItemName") = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Product/name")
                                rowImportItems.Item("SourceType") = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Product/producttypeid")

                                ItemsForImport.Rows.Add(rowImportItems)

                                ' start of code added TJS 31/03/11
                                ' check for duplicate/blank SKU
                                If rowImportItems.Item("ItemSKU").ToString.Trim = "" Then ' TJS 04/11/11
                                    rowImportItems.Item("Import") = False
                                    rowImportItems.Item("SKUError") = True
                                    rowImportItems.SetColumnError("ItemSKU", "Cannot import blank SKU")
                                Else
                                    ' now check for duplicates - ignore last row
                                    For iLoop = 0 To ItemsForImport.Rows.Count - 2
                                        If ItemsForImport.Rows(iLoop).Item("ItemSKU").ToString.Trim.ToLower = rowImportItems.Item("ItemSKU").ToString.Trim.ToLower Then ' TJS 04/11/11
                                            rowImportItems.SetColumnError("ItemSKU", "Duplicate SKU")
                                            rowImportItems.Item("Import") = False
                                            rowImportItems.Item("SKUError") = True
                                            ItemsForImport.Rows(iLoop).SetColumnError("ItemSKU", "Duplicate SKU")
                                            ItemsForImport.Rows(iLoop).Item("Import") = False
                                            ItemsForImport.Rows(iLoop).Item("SKUError") = True
                                        End If
                                    Next
                                End If
                                ' end of code added TJS 31/03/11

                            Next
                            strSubmit = "<AspDotNetStorefrontImport Verbose=""false"">" & vbCrLf & "<Query Name=""ProductTypeList"" RowName=""ProductType"">"
                            strSubmit = strSubmit & "<SQL>" & vbCrLf & "<![CDATA[" & vbCrLf
                            strSubmit = strSubmit & "SELECT ProductTypeID, Name FROM ProductType" & vbCrLf
                            strSubmit = strSubmit & "]]>" & vbCrLf & "</SQL>" & vbCrLf & "</Query>" & vbCrLf & "</AspDotNetStorefrontImport>"

                            strTemp = ""
                            XMLResponse = ASPStorefrontConnection.SendXMLToASPStorefront(bASPStorefrontUseWSE3, strASPStorefrontURL, _
                                strASPStorefrontUser, strASPStorefrontPwd, strSubmit, strTemp)
                            ' any errors ?
                            If strTemp = "" Then
                                ' no, process product list
                                XMLItemList = XMLResponse.XPathSelectElements("AspDotNetStorefrontImportResult/Query/ProductType")
                                ReDim m_ProductTypeList(m_ASPStorefrontImportRule.GetXMLElementListCount(XMLItemList) - 1)
                                iLoop = 0
                                For Each XMLItemNode In XMLItemList
                                    XMLTemp = XDocument.Parse(XMLItemNode.ToString)
                                    m_ProductTypeList(iLoop).TypeID = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "ProductType/producttypeid")
                                    m_ProductTypeList(iLoop).Description = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "ProductType/name")
                                    iLoop += 1
                                Next
                            Else
                                m_LastError = strTemp ' TJS 02/12/11
                            End If
                            m_ProductsAlreadyImported = GetInventoryItemsImportedCount() ' TJS 29/03/11
                            If ItemsForImport.Rows.Count > 0 Then
                                bReturnValue = True
                            End If

                        Else
                            m_LastError = strTemp ' TJS 02/12/11
                        End If
                        ASPStorefrontConnection = Nothing

                        Exit For
                    Else
                        m_LastError = "Please enter your ASPDotNetStorefront API connection settings in the eShopCONNECTED config." ' TJS 02/12/11
                    End If
                    Exit For ' TJS 01/04/14
                End If
                If Cancel Then
                    Return False
                End If
            Next
            If Not bInstanceMatched Then ' TJS 01/04/14
                m_LastError = "ASPDotNetStorefront Instance not found." ' TJS 02/12/11
                m_ImportLog = m_ImportLog & "Cannot connect to ASPDotNetStorefront" & vbCrLf ' TJS 01/04/14
            End If
        End If
        Return bReturnValue


    End Function
#End Region

#Region " GetASPStorefrontCategories "
    Public Function GetASPStorefrontCategories(ByVal ASPStorefrontSiteID As String, ByRef Cancel As Boolean, ByRef CategoryTable As System.Data.DataTable) As Boolean ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/04/11 | TJS             | 2011.0.10 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | Also replaced call to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages and modified to return true/false
        ' 14/02/12 | TJS             | 2011.2.05 | Corrected extraction of category list
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to cater for IsActive being false on a Category record
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowASPStorefrontConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row ' TJS 02/12/11
        Dim ASPStorefrontConnection As Lerryn.Facade.ImportExport.ASPStorefrontConnector
        Dim XMLResponse As XDocument, XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLCategoryList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement, XMLCategory As XElement
        Dim rowCategory As System.Data.DataRow
        Dim strSubmit As String, strTemp As String, iLoop As Integer
        Dim bReturnValue As Boolean ' TJS 02/12/11

        bReturnValue = True ' TJS 02/12/11
        rowASPStorefrontConfig = Me.m_ASPStorefrontImportDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(ASP_STORE_FRONT_SOURCE_CODE) ' TJS 02/12/11
        XMLConfig = XDocument.Parse(Trim(rowASPStorefrontConfig.ConfigSettings_DEV000221)) ' TJS 02/12/11
        XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
        For Each XMLNode In XMLNodeList
            XMLTemp = XDocument.Parse(XMLNode.ToString)
            If Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID) = ASPStorefrontSiteID Then
                If Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_URL) <> "" And _
                    Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_USER) <> "" Then
                    ASPStorefrontConnection = New Lerryn.Facade.ImportExport.ASPStorefrontConnector
                    ' get category details from ASPStorefront
                    strSubmit = "<AspDotNetStorefrontImport Verbose=""false"">" & vbCrLf & "<Query Name=""CategoryList"" RowName=""Category"">"
                    strSubmit = strSubmit & "<SQL>" & vbCrLf & "<![CDATA[" & vbCrLf
                    strSubmit = strSubmit & "SELECT CategoryID, CategoryGUID, Name, ParentCategoryID FROM Category WHERE Deleted = 0" & vbCrLf
                    strSubmit = strSubmit & "]]>" & vbCrLf & "</SQL>" & vbCrLf & "</Query>" & vbCrLf & "</AspDotNetStorefrontImport>"
                    strTemp = ""

                    XMLResponse = ASPStorefrontConnection.SendXMLToASPStorefront(CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_USE_WSE3_AUTHENTICATION).ToUpper = "YES", True, False)), _
                        Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_URL), _
                        Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_USER), _
                        Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_PASSWORD), strSubmit, strTemp)
                    ' any errors ?
                    If strTemp = "" Then
                        XMLCategoryList = XMLResponse.XPathSelectElements("AspDotNetStorefrontImportResult/Query/Category") ' TJS 14/02/12
                        For Each XMLCategory In XMLCategoryList
                            XMLTemp = XDocument.Parse(XMLCategory.ToString)
                            rowCategory = CategoryTable.NewRow
                            rowCategory.Item("Active") = False
                            For iLoop = 0 To Me.m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Count - 1
                                If Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/categoryguid") = Me.m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221(iLoop).CategoryGUID_DEV000221 Then
                                    rowCategory.Item("Active") = Me.m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221(iLoop).IsActive_DEV000221 ' TJS 24/02/12
                                    Exit For
                                End If
                            Next
                            rowCategory.Item("SourceCategoryName") = Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/name")
                            rowCategory.Item("SourceCategoryID") = Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/categoryid")
                            rowCategory.Item("SourceParentID") = Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/parentcategoryid")
                            rowCategory.Item("CategoryGUID") = Me.m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/categoryguid")
                            CategoryTable.Rows.Add(rowCategory)
                            If Cancel Then
                                Exit For
                            End If
                        Next
                    Else
                        m_LastError = "Unable to read ASPDotNetStorefront Category List - " & strTemp & vbCrLf & vbCrLf & "Please check your configuration settings." ' TJS 02/12/11
                        bReturnValue = False ' TJS 02/12/11

                    End If
                End If
                Exit For
            End If
        Next
        Return bReturnValue ' TJS 02/12/11

    End Function
#End Region

#Region " ImportASPStorefrontProducts "
    Public Sub ImportASPStorefrontProducts(ByVal ASPStorefrontSiteID As String, ByRef ItemsForImport As DataTable, ByRef Cancel As Boolean, _
        ByRef NoOfProductsImported As Integer, ByRef NoOfProductsSkipped As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/03/11 | TJS             | 2011.0.04 | Modified to cater for product variants and to allow operation without connector activated
        ' 30/03/11 | TJS             | 2011.0.05 | Corrected generation of Named variants
        ' 04/04/11 | TJS             | 2011.0.07 | Modified to correct processing of Color variants
        ' 08/04/11 | TJS             | 2011.0.09 | Added further import log messages
        ' 05/05/11 | TJS             | 2011.0.13 | Modified to cater for IS 4.8.3 build
        ' 04/11/11 | TJS             | 2011.1.10 | Mod1fied to cater for products where SKU is changed in ASPDotNetStorefront
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | Also replaced calls to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to save Product ID as well as GUID as some update functions need this
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple ASPStorefront sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ASPStorefrontConnection As Lerryn.Facade.ImportExport.ASPStorefrontConnector
        Dim rowItemKitOptGroup As Interprise.Framework.Inventory.DatasetGateway.ItemKitDatasetGateway.InventoryKitOptionGroupRow ' TJS 29/03/11
        Dim XMLProduct As XDocument, XMLVariants As XDocument, XMLConfig As XDocument, XMLTemp As XDocument ' TJS 29/03/11
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLVariantList As System.Collections.Generic.IEnumerable(Of XElement), XMLVariant As XElement ' TJS 29/03/11
        Dim strProductType As String, strErrorDetails As String, strTemp As String
        Dim strHomeCurrency As String, strItemCode As String
        Dim strHomeLanguage As String, strSubmit As String, strASPStorefrontURL As String
        Dim strASPStorefrontUser As String, strASPStorefrontPwd As String
        Dim strMatrixSizeSKUs As String(), strMatrixSizes As String(), strMatrixColours As String() ' TJS 29/03/11
        Dim strMatrixColorSKUs As String(), strKitItemCodes() As String, strItemDescriptions As String() ' TJS 29/03/11
        Dim iRelatedProductCount As Integer, iItemLoop As Integer, iLoop As Integer ' TJS 29/03/11
        Dim iDistributorCount As Integer, iSectionCount As Integer, iGenreCount As Integer
        Dim iMatrixSizeCount As Integer, iMatrixColorCount As Integer, iMatrixSizeLoop As Integer ' TJS 29/03/11
        Dim iMatrixColorLoop As Integer, iMatrixItemLoop As Integer, iKitItemPtr As Integer ' TJS 29/03/11
        Dim bASPStorefrontUseWSE3 As Boolean, bInstanceMatched As Boolean, bISItemExists As Boolean
        Dim bManufacturererFound As Boolean, bKitVariants As Boolean ' TJS 29/03/11
        Dim iASPStorefrontInstanceCount As Integer

        Const ASPStorefrontXMLVariantPath As String = "AspDotNetStorefrontImportResult/Query/ProductVariant/" ' TJS 29/03/11

        Try
            m_LastError = "" ' TJS 02/12/11
            m_ImportLimitReached = False ' TJS 02/12/11
            m_ImportLog = "" ' TJS 29/03/11
            If m_ASPStorefrontImportRule.IsActivated Then
                strErrorDetails = ""
                m_ProductsAlreadyImported = GetInventoryItemsImportedCount() ' TJS 29/03/11
                If Not m_ASPStorefrontImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then ' TJS 29/03/11
                    strHomeCurrency = Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                    strHomeLanguage = Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
                    XMLConfig = XDocument.Parse(Trim(m_ASPStorefrontImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
                    iASPStorefrontInstanceCount = m_ASPStorefrontImportRule.GetXMLElementListCount(XMLNodeList)
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If iASPStorefrontInstanceCount = 1 Then
                            bInstanceMatched = True
                        ElseIf m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID) = ASPStorefrontSiteID Then
                            bInstanceMatched = True
                        End If
                        If bInstanceMatched Then
                            strASPStorefrontURL = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_URL)
                            strASPStorefrontUser = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_USER)
                            strASPStorefrontPwd = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_PASSWORD)
                            bASPStorefrontUseWSE3 = CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_USE_WSE3_AUTHENTICATION).ToUpper = "YES", True, False))
                            If strASPStorefrontURL <> "" And strASPStorefrontUser <> "" Then

                                ASPStorefrontConnection = New Lerryn.Facade.ImportExport.ASPStorefrontConnector

                                NewItemDataset = New Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
                                NewItemFacade = New Interprise.Facade.Inventory.ItemDetailFacade(NewItemDataset, Interprise.Framework.Base.Shared.Enum.TransactionType.InventoryItem) ' TJS 29/03/11

                                iRelatedProductCount = 0
                                iDistributorCount = 0
                                iSectionCount = 0
                                iGenreCount = 0
                                If m_CreateISCategories Then ' TJS 29/03/11
                                    ImportCategories(bASPStorefrontUseWSE3, strASPStorefrontURL, strASPStorefrontUser, strASPStorefrontPwd, strHomeLanguage) ' TJS 29/03/11
                                End If
                                For iItemLoop = 0 To ItemsForImport.Rows.Count - 1
                                    ' is Import box checked and not already imported ?
                                    If CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And Not CBool(ItemsForImport.Rows(iItemLoop).Item("AlreadyImported")) And _
                                        Not CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) Then ' TJS 04/11/11
                                        ' yes,  get product details from ASPStorefront
                                        strSubmit = "<AspDotNetStorefrontImport Verbose=""false""><GetProduct GUID="""
                                        strSubmit = strSubmit & ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString & """/></AspDotNetStorefrontImport>"
                                        strTemp = ""

                                        XMLProduct = ASPStorefrontConnection.SendXMLToASPStorefront(bASPStorefrontUseWSE3, strASPStorefrontURL, _
                                            strASPStorefrontUser, strASPStorefrontPwd, strSubmit, strTemp)
                                        ' any errors ?
                                        If strTemp = "" Then
                                            ' no, get Product Variant details
                                            strSubmit = "<AspDotNetStorefrontImport Verbose=""false"">" & vbCrLf & "<Query Name=""VariantList"" RowName=""ProductVariant"">" ' TJS 29/03/11
                                            strSubmit = strSubmit & "<SQL>" & vbCrLf & "<![CDATA[" & vbCrLf & "SELECT ProductVariant.* FROM ProductVariant INNER JOIN " ' TJS 29/03/11
                                            strSubmit = strSubmit & "Product ON ProductVariant.ProductID = Product.ProductID  WHERE Product.ProductGUID = '" ' TJS 29/03/11
                                            strSubmit = strSubmit & ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString & "' AND Product.Deleted = 0 AND " ' TJS 29/03/11
                                            strSubmit = strSubmit & "ProductVariant.Deleted = 0" & vbCrLf & "]]>" & vbCrLf & "</SQL>" & vbCrLf & "</Query>" ' TJS 29/03/11
                                            strSubmit = strSubmit & vbCrLf & "</AspDotNetStorefrontImport>" ' TJS 29/03/11

                                            strTemp = "" ' TJS 29/03/11

                                            XMLVariants = ASPStorefrontConnection.SendXMLToASPStorefront(bASPStorefrontUseWSE3, strASPStorefrontURL, _
                                                strASPStorefrontUser, strASPStorefrontPwd, strSubmit, strTemp) ' TJS 29/03/11
                                            ' any errors ?
                                            If strTemp = "" Then ' TJS 29/03/11
                                                ' no, process product details
                                                bManufacturererFound = False
                                                NewManufacturerFacade = Nothing
                                                NewManufacturerDataset = Nothing
                                                NewKitFacade = Nothing
                                                NewKitDataset = Nothing
                                                XMLVariantList = XMLVariants.XPathSelectElements("AspDotNetStorefrontImportResult/Query/ProductVariant") ' TJS 29/03/11
                                                bKitVariants = False ' TJS 29/03/11
                                                If m_ASPStorefrontImportRule.GetXMLElementListCount(XMLVariantList) > 1 Then ' TJS 29/03/11
                                                    bKitVariants = True ' TJS 29/03/11

                                                ElseIf m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "SKUSuffix") <> "" Then ' TJS 29/03/11
                                                    ' SKU Suffix variant
                                                    bKitVariants = True ' TJS 29/03/11

                                                End If
                                                bISItemExists = False
                                                strItemCode = ""
                                                ' start of code added TJS 29/03/11
                                                If Not bKitVariants Then
                                                    ' only 1 variant record, are size or colour variants present ?
                                                    If m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "SizeSKUModifiers") <> "" And _
                                                        m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "ColorSKUModifiers") <> "" Then
                                                        ' size and colour variants
                                                        strMatrixSizeSKUs = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "SizeSKUModifiers").Split(CChar(","))
                                                        strMatrixSizes = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "Sizes").Split(CChar(","))
                                                        iMatrixSizeCount = strMatrixSizeSKUs.Length
                                                        strMatrixColorSKUs = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "ColorSKUModifiers").Split(CChar(","))
                                                        strMatrixColours = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "Colors").Split(CChar(","))
                                                        iMatrixColorCount = strMatrixColorSKUs.Length
                                                        strProductType = ITEM_DEFAULT_CLASS_MATRIX_GROUP
                                                        If iMatrixSizeCount = strMatrixSizes.Length And iMatrixColorCount = strMatrixColours.Length Then ' TJS 04/04/11
                                                            If CreateItem(XMLProduct, ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString, ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString, strHomeLanguage, ASPStorefrontSiteID, strProductType) Then
                                                                If ApplySizeAttributes(strMatrixSizes, strMatrixSizeSKUs) Then
                                                                    If ApplyColorAttributes(strMatrixColours, strMatrixColorSKUs) Then
                                                                        ' create Matrix Item records
                                                                        strErrorDetails = ""
                                                                        NewItemFacade.GenerateInventoryMatrixItem(strErrorDetails)
                                                                        If strErrorDetails = "" Then
                                                                            For iMatrixSizeLoop = 0 To iMatrixSizeCount - 1
                                                                                For iMatrixColorLoop = 0 To iMatrixColorCount - 1
                                                                                    For iMatrixItemLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
                                                                                        If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute1 = strMatrixSizes(iMatrixSizeLoop) And _
                                                                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute2 = strMatrixColours(iMatrixColorLoop) Then
                                                                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName = ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & "-" & strMatrixSizeSKUs(iMatrixSizeLoop) & "-" & strMatrixColorSKUs(iMatrixColorLoop)
                                                                                            ' has Matrix item been created before ?
                                                                                            strItemCode = Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName & "'") ' TJS 29/03/11
                                                                                            If strItemCode <> "" Then ' TJS 29/03/11
                                                                                                ' yes, delete matrix Item record
                                                                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Delete() ' TJS 29/03/11
                                                                                                bISItemExists = True ' TJS 29/03/11
                                                                                            End If
                                                                                            Exit For
                                                                                        End If
                                                                                    Next
                                                                                Next
                                                                            Next
                                                                            If Not bISItemExists Then ' TJS 29/03/11
                                                                                NewItemFacade.CreateMatrixItems()
                                                                            Else
                                                                                NewItemDataset.InventoryMatrixGroup(0).Delete() ' TJS 29/03/11
                                                                            End If
                                                                            If SaveItem(ASPStorefrontSiteID, NoOfProductsImported, NoOfProductsSkipped) Then
                                                                                CopyMatrixItemASPSFSettings()
                                                                                m_ImportLog = m_ImportLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                                                                m_ProductsAlreadyImported = m_ProductsAlreadyImported + (iMatrixSizeCount * iMatrixColorCount) + 1
                                                                            End If
                                                                        Else
                                                                            m_LastError = strErrorDetails ' TJS 02/12/11
                                                                            m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted - " & strErrorDetails & vbCrLf
                                                                            NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                        End If
                                                                    Else
                                                                        m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted" & vbCrLf
                                                                        NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                    End If
                                                                Else
                                                                    m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted" & vbCrLf
                                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                End If
                                                            End If
                                                        Else
                                                            If iMatrixSizeCount = strMatrixSizes.Length And iMatrixColorCount = strMatrixColours.Length Then
                                                                m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as number of Size and Color parameters and SKUs do not match" & vbCrLf

                                                            ElseIf iMatrixSizeCount = strMatrixSizes.Length Then
                                                                m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as number of Size parameters and SKUs do not match" & vbCrLf

                                                            Else
                                                                m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as number of Color parameters and SKUs do not match" & vbCrLf
                                                            End If
                                                            NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                        End If

                                                    ElseIf m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "SizeSKUModifiers") <> "" Then
                                                        ' size variants
                                                        strMatrixSizeSKUs = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "SizeSKUModifiers").Split(CChar(","))
                                                        strMatrixSizes = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "Sizes").Split(CChar(","))
                                                        iMatrixSizeCount = strMatrixSizeSKUs.Length
                                                        strProductType = ITEM_DEFAULT_CLASS_MATRIX_GROUP
                                                        If iMatrixSizeCount = strMatrixSizes.Length Then ' TJS 04/04/11
                                                            If CreateItem(XMLProduct, ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString, ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString, strHomeLanguage, ASPStorefrontSiteID, strProductType) Then
                                                                If ApplySizeAttributes(strMatrixSizes, strMatrixSizeSKUs) Then
                                                                    ' create Matrix Item records
                                                                    strErrorDetails = ""
                                                                    NewItemFacade.GenerateInventoryMatrixItem(strErrorDetails)
                                                                    If strErrorDetails = "" Then
                                                                        For iMatrixSizeLoop = 0 To iMatrixSizeCount - 1
                                                                            For iMatrixItemLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
                                                                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute1 = strMatrixSizes(iMatrixSizeLoop) Then
                                                                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName = ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & "-" & strMatrixSizeSKUs(iMatrixSizeLoop)
                                                                                    ' has Matrix item been created before ?
                                                                                    strItemCode = Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName & "'") ' TJS 29/03/11
                                                                                    If strItemCode <> "" Then ' TJS 29/03/11
                                                                                        ' yes, delete matrix Item record
                                                                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Delete() ' TJS 29/03/11
                                                                                        bISItemExists = True ' TJS 29/03/11
                                                                                    End If
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                        Next
                                                                        If Not bISItemExists Then ' TJS 29/03/11
                                                                            NewItemFacade.CreateMatrixItems()
                                                                        Else
                                                                            NewItemDataset.InventoryMatrixGroup(0).Delete() ' TJS 29/03/11
                                                                        End If
                                                                        If SaveItem(ASPStorefrontSiteID, NoOfProductsImported, NoOfProductsSkipped) Then
                                                                            CopyMatrixItemASPSFSettings()
                                                                            m_ImportLog = m_ImportLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                                                            m_ProductsAlreadyImported = m_ProductsAlreadyImported + iMatrixSizeCount + 1
                                                                        End If
                                                                    Else
                                                                        m_LastError = strErrorDetails ' TJS 02/12/11
                                                                        m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted" & vbCrLf
                                                                        NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                    End If
                                                                Else
                                                                    m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted" & vbCrLf
                                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                End If
                                                            End If
                                                        Else
                                                            m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as number of Size parameters and SKUs do not match" & vbCrLf
                                                            NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                        End If

                                                    ElseIf m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "ColorSKUModifiers") <> "" Then
                                                        ' colour variants
                                                        strMatrixColorSKUs = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "ColorSKUModifiers").Split(CChar(","))
                                                        strMatrixColours = m_ASPStorefrontImportRule.GetXMLElementText(XMLVariants, ASPStorefrontXMLVariantPath & "Colors").Split(CChar(","))
                                                        iMatrixColorCount = strMatrixColorSKUs.Length
                                                        strProductType = ITEM_DEFAULT_CLASS_MATRIX_GROUP
                                                        If iMatrixColorCount = strMatrixColours.Length Then ' TJS 04/04/11
                                                            If CreateItem(XMLProduct, ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString, ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString, strHomeLanguage, ASPStorefrontSiteID, strProductType) Then
                                                                If ApplyColorAttributes(strMatrixColours, strMatrixColorSKUs) Then
                                                                    ' create Matrix Item records
                                                                    strErrorDetails = ""
                                                                    NewItemFacade.GenerateInventoryMatrixItem(strErrorDetails)
                                                                    If strErrorDetails = "" Then
                                                                        For iMatrixColorLoop = 0 To iMatrixColorCount - 1
                                                                            For iMatrixItemLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1 ' TJS 04/04/11
                                                                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute1 = strMatrixColours(iMatrixColorLoop) Then
                                                                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName = ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & "-" & strMatrixColorSKUs(iMatrixColorLoop) ' TJS 29/03/11
                                                                                    ' has Matrix item been created before ?
                                                                                    strItemCode = Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName & "'") ' TJS 29/03/11
                                                                                    If strItemCode <> "" Then ' TJS 29/03/11
                                                                                        ' yes, delete matrix Item record
                                                                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Delete() ' TJS 29/03/11
                                                                                        bISItemExists = True ' TJS 29/03/11
                                                                                    End If
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                        Next
                                                                        If Not bISItemExists Then ' TJS 29/03/11
                                                                            NewItemFacade.CreateMatrixItems()
                                                                        Else
                                                                            NewItemDataset.InventoryMatrixGroup(0).Delete() ' TJS 29/03/11
                                                                        End If
                                                                        If SaveItem(ASPStorefrontSiteID, NoOfProductsImported, NoOfProductsSkipped) Then
                                                                            CopyMatrixItemASPSFSettings()
                                                                            m_ImportLog = m_ImportLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                                                            m_ProductsAlreadyImported = m_ProductsAlreadyImported + iMatrixColorCount + 1
                                                                        End If
                                                                    Else
                                                                        m_LastError = strErrorDetails ' TJS 02/12/11
                                                                        m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted" & vbCrLf
                                                                        NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                    End If
                                                                Else
                                                                    m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted" & vbCrLf
                                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                End If
                                                            End If
                                                        Else
                                                            m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as number of Color parameters and SKUs do not match" & vbCrLf
                                                            NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                        End If

                                                    Else
                                                        strProductType = ITEM_DEFAULT_CLASS_STOCK
                                                        If CreateItem(XMLProduct, ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString, ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString, strHomeLanguage, ASPStorefrontSiteID, strProductType) Then
                                                            If SaveItem(ASPStorefrontSiteID, NoOfProductsImported, NoOfProductsSkipped) Then
                                                                m_ImportLog = m_ImportLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                                                m_ProductsAlreadyImported += 1
                                                            End If
                                                        End If

                                                    End If

                                                Else
                                                    ' must be individually named variants - import each first
                                                    iKitItemPtr = 0
                                                    ReDim strKitItemCodes(m_ASPStorefrontImportRule.GetXMLElementListCount(XMLVariantList) - 1)
                                                    For Each XMLVariant In XMLVariantList
                                                        XMLTemp = XDocument.Parse(XMLVariant.ToString)
                                                        strTemp = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "ProductVariant/SKUSuffix")
                                                        If CreateItem(XMLProduct, ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString, ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & strTemp, strHomeLanguage, ASPStorefrontSiteID, ITEM_DEFAULT_CLASS_STOCK) Then
                                                            If SaveItem(ASPStorefrontSiteID, NoOfProductsImported, NoOfProductsSkipped) Then
                                                                strKitItemCodes(iKitItemPtr) = NewItemDataset.InventoryItem(0).ItemCode
                                                                iKitItemPtr += 1
                                                            End If
                                                            NewItemDataset.Clear()
                                                            If NewKitFacade IsNot Nothing Then
                                                                NewKitFacade.Dispose()
                                                                NewKitDataset.Dispose()
                                                            End If
                                                            m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Clear()
                                                            m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Clear()
                                                            m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Clear()

                                                        End If
                                                    Next
                                                    ' now create overall kit item
                                                    iKitItemPtr = 0
                                                    If CreateItem(XMLProduct, ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString, ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString, strHomeLanguage, ASPStorefrontSiteID, ITEM_DEFAULT_CLASS_KIT) Then
                                                        If SaveItem(ASPStorefrontSiteID, NoOfProductsImported, NoOfProductsSkipped) Then
                                                            strItemDescriptions = Me.GetRow(New String() {"ItemDescription", "ExtendedDescription"}, "InventoryItemDescription", "ItemCode = '" & strKitItemCodes(iKitItemPtr) & "' AND LanguageCode= '" & strHomeLanguage & "'")

                                                            NewKitFacade.LoadDataSet(New String()() {New String() {NewKitDataset.InventoryKit.TableName, _
                                                                READINVENTORYKIT, AT_ITEM_KIT_CODE, NewItemDataset.InventoryItem(0).ItemCode}}, _
                                                                Interprise.Framework.Base.Shared.ClearType.Specific)

                                                            rowItemKitOptGroup = NewKitDataset.InventoryKitOptionGroup.NewInventoryKitOptionGroupRow
                                                            rowItemKitOptGroup.ItemKitCode = NewItemDataset.InventoryItem(0).ItemCode
                                                            rowItemKitOptGroup.GroupCode = strItemDescriptions(0)
                                                            rowItemKitOptGroup.GroupType = KIT_GROUP_TYPE_OPTIONAL
                                                            rowItemKitOptGroup.SortOrder = iKitItemPtr + 1
                                                            rowItemKitOptGroup.SelectionControl = KIT_SELECTION_RADIO_LIST
                                                            NewKitDataset.InventoryKitOptionGroup.AddInventoryKitOptionGroupRow(rowItemKitOptGroup)
                                                            NewKitFacade.CreateKitOptionGroupDescription(strItemDescriptions(0), strItemDescriptions(0))
                                                            NewKitFacade.GroupCode = strItemDescriptions(0)
                                                            NewKitFacade.CurrencyCode = strHomeCurrency
                                                            For Each XMLVariant In XMLVariantList
                                                                LoadDataSet(New String()() {New String() {m_ASPStorefrontImportDataset.InventoryItemKitPricingDetailView.TableName, _
                                                                    "ReadInventoryItemKitPricingDetailView_DEV000221", AT_ITEMCODE, strKitItemCodes(iKitItemPtr), _
                                                                    Interprise.Framework.Base.Shared.Const.AT_LANGUAGECODE, NewItemFacade.LanguageCode}}, _
                                                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 30/03/11

                                                                NewKitFacade.AssignKitDetails(New DataRow() {m_ASPStorefrontImportDataset.InventoryItemKitPricingDetailView(0)}, strItemDescriptions(0), strHomeCurrency, Nothing)
                                                                NewKitDataset.InventoryKitDetail(iKitItemPtr).IsDefault = False
                                                                NewKitDataset.InventoryKitDetail(iKitItemPtr).SalesPriceRate = CDec(Me.GetField("RetailPriceRate", "InventoryItemPricingDetail", "ItemCode = '" & strKitItemCodes(iKitItemPtr) & "' AND CurrencyCode = '" & strHomeCurrency & "'"))
                                                                For iLoop = 0 To NewKitDataset.InventoryKitDetailDescription.Count - 1
                                                                    NewKitDataset.InventoryKitDetailDescription(0).ExtendedDescription = strItemDescriptions(1)
                                                                Next
                                                                iKitItemPtr += 1 ' TJS 30/03/11
                                                            Next
                                                            If SaveItemKit() Then
                                                                m_ImportLog = m_ImportLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                                                m_ProductsAlreadyImported = m_ProductsAlreadyImported + m_ASPStorefrontImportRule.GetXMLElementListCount(XMLVariantList)
                                                            End If
                                                        End If
                                                    End If

                                                End If
                                                ' end of code added TJS 29/03/11
                                            Else
                                                m_LastError = "Unable to read ASPDotNetStorefront Product Variants for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " - " & strTemp ' TJS 02/12/11
                                                m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as unable to read ASPDotNetStorefront Product Variants" & vbCrLf ' TJS 08/04/11
                                                NoOfProductsSkipped = NoOfProductsSkipped + 1
                                            End If
                                        Else
                                            m_LastError = "Unable to read ASPDotNetStorefront Product Details for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " - " & strTemp ' TJS 02/12/11
                                            m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as unable to read ASPDotNetStorefront Product Details - " & strTemp & vbCrLf ' TJS 08/04/11
                                            NoOfProductsSkipped = NoOfProductsSkipped + 1
                                        End If

                                    ElseIf CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And Not CBool(ItemsForImport.Rows(iItemLoop).Item("AlreadyImported")) And _
                                        CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) Then ' TJS 04/11/11
                                        ' product SKU has change, user has selected import so update SKU in IS
                                        strTemp = "UPDATE InventoryItem SET ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString ' TJS 04/11/11
                                        strTemp = strTemp & "' WHERE ItemCode = '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'" ' TJS 04/11/11
                                        Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing) ' TJS 04/11/11

                                    End If
                                    If m_ASPStorefrontImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then ' TJS 29/03/11
                                        m_LastError = strErrorDetails ' TJS 02/12/11
                                        m_ImportLimitReached = True ' TJS 02/12/11
                                        Exit For
                                    End If

                                    NewItemDataset.Clear()
                                    If NewKitFacade IsNot Nothing Then
                                        NewKitFacade.Dispose()
                                        NewKitDataset.Dispose()
                                    End If
                                    m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Clear()
                                    m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Clear()
                                    m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Clear()
                                    m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.Clear() ' TJS 04/04/11

                                Next
                                If m_ASPStorefrontImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then ' TJS 29/03/11 
                                    m_LastError = strErrorDetails ' TJS 29/03/11 TJS 02/12/11
                                    m_ImportLimitReached = True ' TJS 02/12/11
                                    m_ImportLog = m_ImportLog & strErrorDetails & vbCrLf ' TJS 04/04/11
                                    Exit For
                                End If
                                NewItemFacade.Dispose()
                                NewItemDataset.Dispose()
                                ASPStorefrontConnection = Nothing
                                Exit For

                            Else
                                m_LastError = "Please enter your ASPDotNetStorefront API connection settings in the eShopCONNECTED config." ' TJS 02/12/11
                                m_ImportLog = m_ImportLog & "Cannot connect to ASPDotNetStorefront - Please enter your ASPDotNetStorefront API connection settings in the eShopCONNECT ASPStorefront Connector configuration" & vbCrLf ' TJS 29/03/11 TJS 08/04/11
                            End If
                            Exit For ' TJS 01/04/14
                        End If
                    Next
                    If Not bInstanceMatched Then ' TJS 01/04/14
                        m_LastError = "ASPDotNetStorefront Instance not found." ' TJS 02/12/11
                        m_ImportLog = m_ImportLog & "Cannot connect to ASPDotNetStorefront" & vbCrLf ' TJS 29/03/11
                    End If
                Else
                    m_LastError = strErrorDetails ' TJS 29/03/11 TJS 02/12/11
                    m_ImportLimitReached = True ' TJS 02/12/11
                End If
            End If

        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace ' TJS 02/12/11

        End Try

    End Sub
#End Region

#Region " ImportCategories "
    Private Sub ImportCategories(ByVal UseWSE3Authentication As Boolean, ByVal APIURL As String, ByVal APIUser As String, _
            ByVal APIPassword As String, ByVal HomeLanguage As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/03/11 | TJS             | 2011.0.04 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ASPStorefrontConnection As Lerryn.Facade.ImportExport.ASPStorefrontConnector
        Dim XMLCategories As XDocument, XMLTemp As XDocument
        Dim XMLCategoryList As System.Collections.Generic.IEnumerable(Of XElement), XMLCategoryNode As XElement
        Dim strSubmit As String, strTemp As String, strErrorDetails As String, iCategoryPtr As Integer
        Dim iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer, iLoop As Integer
        Dim bSomeErrors As Boolean, bDuplicateCategory As Boolean

        bSomeErrors = False
        ASPStorefrontConnection = New Lerryn.Facade.ImportExport.ASPStorefrontConnector

        ' get Product Categories
        strSubmit = "<AspDotNetStorefrontImport Verbose=""false"">" & vbCrLf & "<Query Name=""CategoryList"" RowName=""Category"">"
        strSubmit = strSubmit & "<SQL>" & vbCrLf & "<![CDATA[" & vbCrLf & "SELECT * FROM Category WHERE Deleted = 0 ORDER BY ParentCategoryID"
        strSubmit = strSubmit & vbCrLf & "]]>" & vbCrLf & "</SQL>" & vbCrLf & "</Query>" & vbCrLf & "</AspDotNetStorefrontImport>"

        strTemp = ""

        XMLCategories = ASPStorefrontConnection.SendXMLToASPStorefront(UseWSE3Authentication, APIURL, APIUser, APIPassword, strSubmit, strTemp)
        ' any errors ?
        If strTemp = "" Then
            ' no, process categories
            XMLCategoryList = XMLCategories.XPathSelectElements("AspDotNetStorefrontImportResult/Query/Category")

            ReDim m_ASPStorefrontCategories(m_ASPStorefrontImportRule.GetXMLElementListCount(XMLCategoryList) - 1)
            iCategoryPtr = 0

            NewCategoryDataset = New Interprise.Framework.Inventory.DatasetGateway.SystemManager.CategoryDatasetGateway
            NewCategoryFacade = New Interprise.Facade.Inventory.SystemManager.CategoryFacade(NewCategoryDataset)
            NewCategoryFacade.LanguageCode = HomeLanguage

            For Each XMLCategoryNode In XMLCategoryList
                XMLTemp = XDocument.Parse(XMLCategoryNode.ToString)

                m_ASPStorefrontCategories(iCategoryPtr).CategoryName = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/Name").Replace(" ", "")
                m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).CategoryName.Replace(" ", "")
                ' is category code too long ?
                If m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Length > 30 Then
                    ' yes, truncate it
                    m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Substring(0, 30)
                End If
                ' now check for duplicate category codes
                For iLoop = 0 To m_ASPStorefrontCategories.Length - 1
                    If m_ASPStorefrontCategories(iLoop).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode And _
                        iLoop <> iCategoryPtr Then
                        ' duplicate found - need to get alternative
                        bDuplicateCategory = True
                        Exit For
                    End If
                Next
                strTemp = ""
                Do While bDuplicateCategory
                    If strTemp = "" Then
                        strTemp = "1"
                        ' is category name maximum length ?
                        If m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Length = 30 Then
                            m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Substring(0, 29) & strTemp
                        Else
                            m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode & strTemp
                        End If

                    Else
                        m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Substring(0, m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Length - strTemp.Length)
                        strTemp = (CInt(strTemp) + 1).ToString
                        If m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Length + strTemp.Length > 30 Then
                            m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode.Substring(0, 30 - strTemp.Length)
                        End If
                        m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode & strTemp

                    End If
                    bDuplicateCategory = False
                    For iLoop = 0 To m_ASPStorefrontCategories.Length - 1
                        If m_ASPStorefrontCategories(iLoop).ISCategoryCode = m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode And _
                            iLoop <> iCategoryPtr Then
                            ' duplicate found - need to get another alternative
                            bDuplicateCategory = True
                            Exit For
                        End If
                    Next
                Loop

                m_ASPStorefrontCategories(iCategoryPtr).CategoryDescription = m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/Description")
                m_ASPStorefrontCategories(iCategoryPtr).CategoryID = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/CategoryID"))
                m_ASPStorefrontCategories(iCategoryPtr).ParentID = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/ParentCategoryID"))

                NewCategoryFacade.AddCategory(m_ASPStorefrontCategories(iCategoryPtr).ISCategoryCode, m_ASPStorefrontCategories(iCategoryPtr).CategoryName)
                ' NOTE because we use an SQL query to get the category xml, boolean fields return 0 and 1 not true and false
                NewCategoryDataset.SystemCategory(0).ShowInProductBrowser = CByte(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/ShowInProductBrowser") = "1", 1, 0))
                NewCategoryDataset.SystemCategory(0).IsActive = CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/Published") = "1", True, False))
                If CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/ParentCategoryID")) = 0 Then
                    NewCategoryDataset.SystemCategory(0).ParentCategory = "Default"
                Else
                    For iLoop = 0 To iCategoryPtr
                        If m_ASPStorefrontCategories(iLoop).CategoryID = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLTemp, "Category/ParentCategoryID")) Then
                            NewCategoryDataset.SystemCategory(0).ParentCategory = m_ASPStorefrontCategories(iLoop).ISCategoryCode
                            Exit For
                        End If
                    Next
                End If
                If NewCategoryFacade.UpdateDataSet(NewCategoryFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.None, "New Category import", False) Then
                    iCategoryPtr += 1
                Else
                    strErrorDetails = ""
                    bSomeErrors = True
                    For iTableLoop = 0 To NewCategoryFacade.CommandSet.Length - 1
                        For iRowLoop = 0 To NewCategoryDataset.Tables(NewCategoryFacade.CommandSet(iTableLoop)(0)).Rows.Count - 1
                            For iColumnLoop = 0 To NewCategoryDataset.Tables(NewCategoryFacade.CommandSet(iTableLoop)(0)).Columns.Count - 1
                                If NewCategoryDataset.Tables(NewCategoryFacade.CommandSet(iTableLoop)(0)).Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                    strErrorDetails = strErrorDetails & NewCategoryDataset.Tables(NewCategoryFacade.CommandSet(iTableLoop)(0)).TableName & _
                                        "." & NewCategoryDataset.Tables(NewCategoryFacade.CommandSet(iTableLoop)(0)).Columns(iColumnLoop).ColumnName & ", " & _
                                        NewCategoryDataset.Tables(NewCategoryFacade.CommandSet(iTableLoop)(0)).Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                End If
                            Next
                        Next
                    Next
                    m_LastError = strErrorDetails ' TJS 02/12/11
                    m_ImportLog = m_ImportLog & "Unable to import Category " & m_ASPStorefrontCategories(iCategoryPtr).CategoryName & " - " & strErrorDetails
                End If
            Next
            If Not bSomeErrors Then
                m_ImportLog = m_ImportLog & "Successfully imported Categories from ASPDotNetStorefront" & vbCrLf
            Else
                m_ImportLog = m_ImportLog & "Finished importing Categories from ASPDotNetStorefront with some errors" & vbCrLf
            End If
        Else
            m_LastError = "Unable to read ASPDotNetStorefront Category list" ' TJS 02/12/11
            m_ImportLog = m_ImportLog & "Unable to read ASPDotNetStorefront Category list" & vbCrLf
        End If

    End Sub
#End Region

#Region " CreateItem "
    Private Function CreateItem(ByRef XMLProduct As XDocument, ByVal ProductGUID As String, _
        ByVal SKUToUse As String, ByVal HomeLanguage As String, ByVal SiteID As String, ByVal ItemClassCode As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created using code from ImportASPStorefrontProducts
        '                                        | and added code to set extra IS fields
        ' 30/03/11 | TJS             | 2011.0.05 | Modified to set IS category if categories were imported, 
        '                                        | to cater for named variants and check for duplicate SKUs
        ' 04/04/11 | TJS             | 2011.0.07 | Modified to correct ExtendedData field import and to set 
        '                                        | WebOption summary and description fields
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | Also replaced calls to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages
        '                                        | Also corrected manufacturer code setting
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to save Product ID as well as GUID as some update functions need this
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowItemKit As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway.InventoryKitRow
        Dim rowASPStorefrontDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontDetails_DEV000221Row
        Dim rowASPStorefrontCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontCategories_DEV000221Row
        Dim rowASPStorefrontTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontTagDetails_DEV000221Row
        Dim rowManufacturerSource As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemManufacturerSourceID_DEV000221Row
        Dim rowManufacturerItems(0) As System.Data.DataRow
        Dim XMLMappingList As System.Collections.Generic.IEnumerable(Of XElement), XMLMappingNode As XElement, XMLTemp As XDocument
        Dim strItemCode As String, strTagName As String, strParentNode As String, strXMLPath As String
        Dim strTagDisplayName As String, strTemp As String, strMessage As String
        Dim iDistributorCount As Integer, iSectionCount As Integer, iGenreCount As Integer
        Dim iLoop As Integer, iTextMaxLength As Integer, iLineNumber As Integer, iRelatedProductCount As Integer
        Dim iWebLoop As Integer, bISItemExists As Boolean

        Const ASPStorefrontXMLProductPath As String = "AspDotNetStorefrontImportResult/GetProduct/Product/"

        ' check Item Name doesn't already exist
        strItemCode = Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & SKUToUse & "'")
        If strItemCode <> "" Then
            bISItemExists = True
            strTemp = Me.GetField("ItemCode_DEV000221", "InventoryASPStorefrontDetails_DEV000221", "ItemCode_DEV000221 = '" & strItemCode & "'") ' TJS 30/03/11
            If strTemp <> "" Then ' TJS 30/03/11
                strMessage = "Inventory Item with SKU " & SKUToUse & " already imported - check for duplicated SKUs" ' TJS 30/03/11
                m_LastError = strMessage ' TJS 30/03/11 TJS 02/12/11
                m_ImportLog = m_ImportLog & strMessage & vbCrLf ' TJS 30/03/11
                Return False ' TJS 30/03/11
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
        m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Clear()
        m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Clear()
        m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Clear()

        rowASPStorefrontDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.NewInventoryASPStorefrontDetails_DEV000221Row
        rowASPStorefrontDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
        rowASPStorefrontDetails.Publish_DEV000221 = True
        rowASPStorefrontDetails.SellingPrice_DEV000221 = 0
        rowASPStorefrontDetails.SiteID_DEV000221 = SiteID
        rowASPStorefrontDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
        rowASPStorefrontDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION ' TJS 13/11/13
        rowASPStorefrontDetails.FromImportWizard_DEV000221 = True
        m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.AddInventoryASPStorefrontDetails_DEV000221Row(rowASPStorefrontDetails)

        rowASPStorefrontDetails.ASPStorefrontProductGUID_DEV000221 = ProductGUID
        rowASPStorefrontDetails.ASPStorefrontProductID_DEV000221 = CInt(m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLProduct, Left(ASPStorefrontXMLProductPath, Len(ASPStorefrontXMLProductPath) - 1), "ID")) ' TJS 10/03/12
        NewItemDataset.InventoryItem(0).ItemName = SKUToUse ' TJS 30/03/11
        NewItemDataset.InventoryItem(0).ItemDescription = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Name")
        For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
            NewItemDataset.InventoryItemDescription(iLoop).ItemDescription = NewItemDataset.InventoryItem(0).ItemDescription
        Next

        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Description").Length > 1000 Then
            NewItemDataset.InventoryItem(0).ExtendedDescription = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Description").Substring(0, 1000)
        Else
            NewItemDataset.InventoryItem(0).ExtendedDescription = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Description")
        End If
        For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
            NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription = NewItemDataset.InventoryItem(0).ExtendedDescription
        Next
        For iLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1 ' TJS 04/04/11
            NewItemDataset.InventoryItemWebOptionDescription(iLoop).WebDescription = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Description") ' TJS 04/04/11
        Next
        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Summary") <> "" Then
            rowASPStorefrontDetails.ProductSummary_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Summary")
            For iLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1 ' TJS 04/04/11
                NewItemDataset.InventoryItemWebOptionDescription(iLoop).Summary = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "Summary") ' TJS 04/04/11
            Next
        End If
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
                NewItemDataset.InventoryItem(0).XMLPackage = XML_PACKAGE_MATRIXPRODUCT ' TJS 29/03/11
                NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP ' TJS 29/03/11

                If SKUToUse.Length > CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryMatrixGroup' AND ColumnName = 'Prefix'")) Then ' TJS 04/04/11
                    strMessage = "Import of SKU " & SKUToUse & " aborted as base SKU exceed maximum size for a Matrix Group" ' TJS 04/04/11
                    m_LastError = strMessage ' TJS 04/04/11 TJS 02/12/11
                    m_ImportLog = m_ImportLog & strMessage & vbCrLf ' TJS 04/04/11
                    Return False ' TJS 04/04/11
                End If
                NewItemDataset.InventoryMatrixGroup(0).Prefix = SKUToUse

        End Select
        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "AvailableStartDate") <> "" Then
            rowASPStorefrontDetails.AvailableFrom_DEV000221 = m_ASPStorefrontImportRule.ConvertXMLDate(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "AvailableStartDate"))
            NewItemDataset.InventoryItemWebOptionView(0).StartDate = rowASPStorefrontDetails.AvailableFrom_DEV000221 ' TJS 29/03/11
        End If

        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "AvailableStopDate") <> "" Then
            rowASPStorefrontDetails.AvailableTo_DEV000221 = m_ASPStorefrontImportRule.ConvertXMLDate(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & "AvailableStopDate"))
            NewItemDataset.InventoryItemWebOptionView(0).EndDate = rowASPStorefrontDetails.AvailableTo_DEV000221 ' TJS 29/03/11
        End If

        strTagName = ""
        strXMLPath = ""
        strTagDisplayName = ""
        iTextMaxLength = 0
        strParentNode = ""
        iLineNumber = 1
        ' text tag items
        For iLoop = 0 To 3
            Select Case iLoop
                Case 0
                    strTagName = "ManufacturerPartNumber"
                    strTagDisplayName = "Manufacturer Part Number"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                    iTextMaxLength = 50
                Case 1
                    strTagName = "SEName"
                    strTagDisplayName = "SE Name"
                    strParentNode = "SE"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    iTextMaxLength = 150
                    For iWebLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1
                        NewItemDataset.InventoryItemWebOptionDescription(iWebLoop).SEName = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) ' TJS 29/03/11
                    Next
                Case 2
                    strTagName = "XmlPackage"
                    strTagDisplayName = "Xml Package"
                    strParentNode = "Display"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    iTextMaxLength = 100
                Case 3
                    strTagName = "TemplateName"
                    strTagDisplayName = "Template Name"
                    strParentNode = "Display"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    iTextMaxLength = 50
            End Select
            rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
            rowASPStorefrontTagDetails.TagName_DEV000221 = strTagName
            rowASPStorefrontTagDetails.ParentNode_DEV000221 = strParentNode
            rowASPStorefrontTagDetails.LineNumber_DEV000221 = iLineNumber
            rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = strTagDisplayName
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Text"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = iTextMaxLength
            rowASPStorefrontTagDetails.TagTextValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
            m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

        ' memo tag items
        For iLoop = 0 To 15
            Select Case iLoop
                Case 0
                    strTagName = "SpecTitle"
                    strTagDisplayName = "Spec Title"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 1
                    strTagName = "MiscText"
                    strTagDisplayName = "Misc Text"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 2
                    strTagName = "Notes"
                    strTagDisplayName = strTagName
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 3
                    strTagName = "IsFeaturedTeaser"
                    strTagDisplayName = "Is Featured Teaser"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 4
                    strTagName = "FroogleDescription"
                    strTagDisplayName = "Froogle Description"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 5
                    strTagName = "SwatchImageMap"
                    strTagDisplayName = "Swatch Image Map"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 6
                    strTagName = "SETitle"
                    strTagDisplayName = "SE Title"
                    strParentNode = "SE"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    For iWebLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1
                        NewItemDataset.InventoryItemWebOptionDescription(iWebLoop).SETitle = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) ' TJS 29/03/11
                    Next
                Case 7
                    strTagName = "SEKeywords"
                    strTagDisplayName = "SE Keywords"
                    strParentNode = "SE"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    For iWebLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1
                        NewItemDataset.InventoryItemWebOptionDescription(iWebLoop).SEKeywords = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) ' TJS 29/03/11
                    Next
                Case 8
                    strTagName = "SEDescription"
                    strTagDisplayName = "SE Description"
                    strParentNode = "SE"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    For iWebLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1
                        NewItemDataset.InventoryItemWebOptionDescription(iWebLoop).SEDescription = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) ' TJS 29/03/11
                    Next
                Case 9
                    strTagName = "SENoScript"
                    strTagDisplayName = "SE No Script"
                    strParentNode = "SE"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    For iWebLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1
                        NewItemDataset.InventoryItemWebOptionDescription(iWebLoop).SENoScript = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) ' TJS 29/03/11
                    Next
                Case 10
                    strTagName = "SEAltText"
                    strTagDisplayName = "SE Alt Text"
                    strParentNode = "SE"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                    For iWebLoop = 0 To NewItemDataset.InventoryItemWebOptionDescription.Count - 1
                        NewItemDataset.InventoryItemWebOptionDescription(iWebLoop).SEAltText = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) ' TJS 29/03/11
                    Next
                Case 11
                    strTagName = "SizeOptionPrompt"
                    strTagDisplayName = "Size Option Prompt"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 12
                    strTagName = "ColorOptionPrompt"
                    strTagDisplayName = "Color Option Prompt"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 13
                    strTagName = "SpecCall"
                    strTagDisplayName = "Spec Call"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 14
                    strTagName = "ImageFilenameOverride"
                    strTagDisplayName = "Image Filename Override"
                    strParentNode = "Images"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 15
                    strTagName = "TextOptionPrompt"
                    strTagDisplayName = "Text Option Prompt"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
            End Select
            rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
            rowASPStorefrontTagDetails.TagName_DEV000221 = strTagName
            rowASPStorefrontTagDetails.ParentNode_DEV000221 = strParentNode
            rowASPStorefrontTagDetails.LineNumber_DEV000221 = iLineNumber
            rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = strTagDisplayName
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Memo"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
            rowASPStorefrontTagDetails.TagMemoValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
            m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

        ' Integer tag items
        For iLoop = 0 To 4
            Select Case iLoop
                Case 0
                    strTagName = "ColWidth"
                    strTagDisplayName = "Col Width"
                    strParentNode = "Display"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                Case 1
                    strTagName = "PageSize"
                    strTagDisplayName = "Page Size"
                    strParentNode = "Display"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                Case 2
                    strTagName = "SkinID"
                    strTagDisplayName = "Skin ID"
                    strParentNode = "Display"
                    strXMLPath = ASPStorefrontXMLProductPath & strParentNode & "/" & strTagName
                    iLineNumber = 1
                Case 3
                    strTagName = "PackSize"
                    strTagDisplayName = "Pack Size"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 4
                    strTagName = "TextOptionMaxLength"
                    strTagDisplayName = "Text Option Max Length"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
            End Select
            rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
            rowASPStorefrontTagDetails.TagName_DEV000221 = strTagName
            rowASPStorefrontTagDetails.ParentNode_DEV000221 = strParentNode
            rowASPStorefrontTagDetails.LineNumber_DEV000221 = iLineNumber
            rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = strTagDisplayName
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Integer"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
            rowASPStorefrontTagDetails.TagTextValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & strTagName)
            If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, ASPStorefrontXMLProductPath & strTagName) <> "" Then
                rowASPStorefrontTagDetails.TagNumericValue_DEV000221 = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
            End If
            m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

        ' Boolean tag items
        For iLoop = 0 To 13
            Select Case iLoop
                Case 0
                    strTagName = "SpecsInline"
                    strTagDisplayName = "Specs In line"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 1
                    strTagName = "IsFeatured"
                    strTagDisplayName = "Is Featured"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                    NewItemDataset.InventoryItemWebOptionView(0).IsFeaturedTemp = CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true", True, False)) ' TJS 29/03/11
                Case 2
                    strTagName = "IsAKit"
                    strTagDisplayName = "Is a Kit"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 3
                    strTagName = "IsSystem"
                    strTagDisplayName = "Is System"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 4
                    strTagName = "IsAPack"
                    strTagDisplayName = "Is a Pack"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 5
                    strTagName = "ShowInProductBrowser"
                    strTagDisplayName = "Show In Product Browser"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 6
                    strTagName = "ShowBuyButton"
                    strTagDisplayName = "Show Buy Button"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 7
                    strTagName = "Wholesale"
                    strTagDisplayName = strTagName
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 8
                    strTagName = "RequiresRegistration"
                    strTagDisplayName = "Requires Registration"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                    NewItemDataset.InventoryItemWebOptionView(0).RequiresRegistrationTemp = CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true", True, False)) ' TJS 29/03/11
                Case 9
                    strTagName = "HidePriceUntilCart"
                    strTagDisplayName = "Hide Price Until Cart"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                    NewItemDataset.InventoryItemWebOptionView(0).HidePriceUntilCartTemp = CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true", True, False)) ' TJS 29/03/11
                Case 10
                    strTagName = "IsCallToOrder"
                    strTagDisplayName = "Is Call To Order"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                    NewItemDataset.InventoryItemWebOptionView(0).IsCallToOrderTemp = CBool(IIf(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true", True, False)) ' TJS 29/03/11
                Case 11
                    strTagName = "ExcludeFromPriceFeeds"
                    strTagDisplayName = "Exclude From Price Feeds"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 12
                    strTagName = "GoogleCheckoutAllowed"
                    strTagDisplayName = "Google Checkout Allowed"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                Case 13
                    strTagName = "RequiresTextOption"
                    strTagDisplayName = "Requires Text Option"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                    ' start of code added TJS 24/02/12
                Case 14
                    strTagName = "Published"
                    strTagDisplayName = "Published"
                    strParentNode = "root"
                    strXMLPath = ASPStorefrontXMLProductPath & strTagName
                    iLineNumber = 1
                    ' end of code added TJS 24/02/12
            End Select
            rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
            rowASPStorefrontTagDetails.TagName_DEV000221 = strTagName
            rowASPStorefrontTagDetails.ParentNode_DEV000221 = strParentNode
            rowASPStorefrontTagDetails.LineNumber_DEV000221 = iLineNumber
            rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = strTagDisplayName
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "Boolean"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
            rowASPStorefrontTagDetails.TagTextValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
            m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
        Next

        XMLMappingList = XMLProduct.XPathSelectElements(ASPStorefrontXMLProductPath & "Mappings/Entity")
        For Each XMLMappingNode In XMLMappingList
            XMLTemp = XDocument.Parse(XMLMappingNode.ToString)
            Select Case m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "EntityType")
                Case "Category"
                    rowASPStorefrontCategory = m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.NewInventoryASPStorefrontCategories_DEV000221Row
                    rowASPStorefrontCategory.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                    rowASPStorefrontCategory.SiteID_DEV000221 = SiteID
                    rowASPStorefrontCategory.CategoryID_DEV000221 = CInt(m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID"))
                    rowASPStorefrontCategory.CategoryGUID_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "GUID")
                    m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.AddInventoryASPStorefrontCategories_DEV000221Row(rowASPStorefrontCategory)
                    ' start of code added TJS 30/03/11
                    For iLoop = 0 To m_ASPStorefrontCategories.Length - 1
                        If m_ASPStorefrontCategories(iLoop).CategoryID = rowASPStorefrontCategory.CategoryID_DEV000221 And _
                            m_ASPStorefrontCategories(iLoop).ISCategoryCode <> "" Then
                            LoadDataSet(New String()() {New String() {m_ASPStorefrontImportDataset.SystemCategoryView.TableName, _
                                "ReadSystemCategoryView_DEV000221", AT_CATEGORY_CODE, m_ASPStorefrontCategories(iLoop).ISCategoryCode}}, _
                                Interprise.Framework.Base.Shared.ClearType.Specific)
                            NewItemFacade.AssignInventoryCategory(Nothing, m_ASPStorefrontImportDataset.SystemCategoryView(0))
                        End If
                    Next
                    ' end of code added TJS 30/03/11

                Case "Manufacturer"
                    strTemp = Me.GetField("ManufacturerCode_DEV000221", "SystemManufacturerSourceID_DEV000221", _
                        "SourceCode_DEV000221 = '" & ASP_STORE_FRONT_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & SiteID & _
                        "' AND SourceManufacturerCode_DEV000221 = '" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID") & _
                        ":" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "GUID") & "'")
                    If strTemp = "" Then
                        strTemp = "ASPSF" & Microsoft.VisualBasic.Right("000000" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID"), 6)
                        rowManufacturerSource = m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.FindByManufacturerCode_DEV000221SourceCode_DEV000221AccountOrInstanceID_DEV000221(strTemp, ASP_STORE_FRONT_SOURCE_CODE, SiteID) ' TJS 04/04/11
                        If rowManufacturerSource Is Nothing Then ' TJS 04/04/11
                            NewManufacturerDataset = New Interprise.Framework.Inventory.DatasetGateway.SystemManager.ManufacturerDatasetGateway
                            NewManufacturerFacade = New Interprise.Facade.Inventory.SystemManager.ManufacturerFacade(NewManufacturerDataset)

                            NewManufacturerFacade.AddManufacturer(strTemp)
                            For iLoop = 0 To NewManufacturerDataset.SystemManufacturerDescription.Count - 1
                                NewManufacturerDataset.SystemManufacturerDescription(iLoop).ManufacturerCode = NewManufacturerDataset.SystemManufacturer(0).ManufacturerCode
                                NewManufacturerDataset.SystemManufacturerDescription(iLoop).Description = "ASPDotNetStorefront " & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID")
                            Next
                            NewManufacturerFacade.UpdateDataSet(NewManufacturerFacade.CommandSet(), Interprise.Framework.Base.Shared.TransactionType.InventoryManufacturer, "Add ASPDotNetStorefront Manufacturer", False)
                            rowManufacturerSource = m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.NewSystemManufacturerSourceID_DEV000221Row
                            rowManufacturerSource.ManufacturerCode_DEV000221 = strTemp
                            rowManufacturerSource.SourceCode_DEV000221 = ASP_STORE_FRONT_SOURCE_CODE
                            rowManufacturerSource.AccountOrInstanceID_DEV000221 = SiteID
                            rowManufacturerSource.SourceManufacturerCode_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID") & _
                                ":" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "GUID")
                            m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.AddSystemManufacturerSourceID_DEV000221Row(rowManufacturerSource)
                        End If
                    End If
                    NewItemDataset.InventoryItem(0).ManufacturerCode = strTemp ' TJS 02/12/11

                Case "Section"
                    rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
                    rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                    rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "Entity:Section"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Mappings"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = iSectionCount + 1
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Section"
                    rowASPStorefrontTagDetails.TagDataType_DEV000221 = "GUID"
                    rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID") & _
                        ":" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "GUID")
                    m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
                    iSectionCount += 1

                Case "Distributor"
                    rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
                    rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                    rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "Entity:Distributor"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Mappings"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = iDistributorCount + 1
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Distributor"
                    rowASPStorefrontTagDetails.TagDataType_DEV000221 = "GUID"
                    rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID") & _
                        ":" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "GUID")
                    m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
                    iDistributorCount += 1

                Case "Genre"
                    rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
                    rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                    rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
                    rowASPStorefrontTagDetails.TagName_DEV000221 = "Entity:Genre"
                    rowASPStorefrontTagDetails.ParentNode_DEV000221 = "Mappings"
                    rowASPStorefrontTagDetails.LineNumber_DEV000221 = iGenreCount + 1
                    rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Genre"
                    rowASPStorefrontTagDetails.TagDataType_DEV000221 = "GUID"
                    rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
                    rowASPStorefrontTagDetails.TagTextValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "ID") & _
                        ":" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "Entity", "GUID")
                    m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
                    iGenreCount += 1
            End Select
        Next

        XMLMappingList = XMLProduct.XPathSelectElements(ASPStorefrontXMLProductPath & "RelatedProducts/CX")
        For Each XMLMappingNode In XMLMappingList
            XMLTemp = XDocument.Parse(XMLMappingNode.ToString)
            rowASPStorefrontTagDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.NewInventoryASPStorefrontTagDetails_DEV000221Row
            rowASPStorefrontTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            rowASPStorefrontTagDetails.SiteID_DEV000221 = SiteID
            rowASPStorefrontTagDetails.TagName_DEV000221 = "CX"
            rowASPStorefrontTagDetails.ParentNode_DEV000221 = "RelatedProducts"
            rowASPStorefrontTagDetails.LineNumber_DEV000221 = iRelatedProductCount + 1
            rowASPStorefrontTagDetails.TagDisplayName_DEV000221 = "Related Product"
            rowASPStorefrontTagDetails.TagDataType_DEV000221 = "GUID"
            rowASPStorefrontTagDetails.TextMaxLength_DEV000221 = 0
            rowASPStorefrontTagDetails.TagTextValue_DEV000221 = m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "CX", "ID") & _
                ":" & m_ASPStorefrontImportRule.GetXMLElementAttribute(XMLTemp, "CX", "GUID")
            m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.AddInventoryASPStorefrontTagDetails_DEV000221Row(rowASPStorefrontTagDetails)
            iRelatedProductCount += 1
        Next

        ' start of code aadded TJS 29/03/11
        If Me.ImportExtData1CustomField <> "" Then
            Try
                strXMLPath = ASPStorefrontXMLProductPath & "ExtensionData" ' TJS 04/04/11
                Select Case NewItemDataset.InventoryItem.Columns(Me.ImportExtData1CustomField).DataType.FullName
                    Case "System.Int32", "System.Int16"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData1CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Decimal"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData1CustomField) = CDec(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Boolean"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData1CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Byte"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "false" Or m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData1CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        Else
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData1CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))

                        End If

                    Case "System.DateTime"

                    Case "System.Byte[]"

                    Case Else
                        NewItemDataset.InventoryItem(0)(Me.ImportExtData1CustomField) = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
                End Select

            Catch ex As Exception
                m_LastError = "Cannot convert import ASPStorefront ExtensionData1 field for Product SKU " & SKUToUse & " to Inventory Item " & Me.ImportExtData1CustomField & " Custom Field." & vbCrLf & vbCrLf & ex.Message ' TJS 02/12/11 TJS 24/08/12

            End Try
        End If

        If Me.ImportExtData2CustomField <> "" Then
            Try
                strXMLPath = ASPStorefrontXMLProductPath & "ExtensionData2" ' TJS 04/04/11
                Select Case NewItemDataset.InventoryItem.Columns(Me.ImportExtData2CustomField).DataType.FullName
                    Case "System.Int32", "System.Int16"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData2CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Decimal"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData2CustomField) = CDec(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Boolean"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData2CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Byte"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "false" Or m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData2CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        Else
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData2CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))

                        End If

                    Case "System.DateTime"

                    Case "System.Byte[]"

                    Case Else
                        NewItemDataset.InventoryItem(0)(Me.ImportExtData2CustomField) = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
                End Select

            Catch ex As Exception
                m_LastError = "Cannot convert import ASPStorefront ExtensionData2 field for Product SKU " & SKUToUse & " to Inventory Item " & Me.ImportExtData2CustomField & " Custom Field." & vbCrLf & vbCrLf & ex.Message ' TJS 02/12/11 TJS 24/08/12

            End Try
        End If

        If Me.ImportExtData3CustomField <> "" Then
            Try
                strXMLPath = ASPStorefrontXMLProductPath & "ExtensionData3" ' TJS 04/04/11
                Select Case NewItemDataset.InventoryItem.Columns(Me.ImportExtData3CustomField).DataType.FullName
                    Case "System.Int32", "System.Int16"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData3CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Decimal"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData3CustomField) = CDec(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Boolean"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData3CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Byte"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "false" Or m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData3CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        Else
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData3CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))

                        End If

                    Case "System.DateTime"

                    Case "System.Byte[]"

                    Case Else
                        NewItemDataset.InventoryItem(0)(Me.ImportExtData3CustomField) = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
                End Select

            Catch ex As Exception
                m_LastError = "Cannot convert import ASPStorefront ExtensionData3 field for Product SKU " & SKUToUse & " to Inventory Item " & Me.ImportExtData3CustomField & " Custom Field." & vbCrLf & vbCrLf & ex.Message ' TJS 02/12/11 TJS 24/08/12

            End Try
        End If

        If Me.ImportExtData4CustomField <> "" Then
            Try
                strXMLPath = ASPStorefrontXMLProductPath & "ExtensionData4" ' TJS 04/04/11
                Select Case NewItemDataset.InventoryItem.Columns(Me.ImportExtData4CustomField).DataType.FullName
                    Case "System.Int32", "System.Int16"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData4CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Decimal"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData4CustomField) = CDec(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Boolean"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData4CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Byte"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "false" Or m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData4CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        Else
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData4CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))

                        End If

                    Case "System.DateTime"

                    Case "System.Byte[]"

                    Case Else
                        NewItemDataset.InventoryItem(0)(Me.ImportExtData4CustomField) = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
                End Select

            Catch ex As Exception
                m_LastError = "Cannot convert import ASPStorefront ExtensionData4 field for Product SKU " & SKUToUse & " to Inventory Item " & Me.ImportExtData4CustomField & " Custom Field." & vbCrLf & vbCrLf & ex.Message ' TJS 02/12/11 TJS 24/08/12

            End Try
        End If

        If Me.ImportExtData5CustomField <> "" Then
            Try
                strXMLPath = ASPStorefrontXMLProductPath & "ExtensionData5" ' TJS 04/04/11
                Select Case NewItemDataset.InventoryItem.Columns(Me.ImportExtData5CustomField).DataType.FullName
                    Case "System.Int32", "System.Int16"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData5CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Decimal"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData5CustomField) = CDec(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Boolean"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath) <> "" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData5CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        End If

                    Case "System.Byte"
                        If m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "false" Or m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath).ToLower = "true" Then
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData5CustomField) = CBool(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))
                        Else
                            NewItemDataset.InventoryItem(0)(Me.ImportExtData5CustomField) = CInt(m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath))

                        End If

                    Case "System.DateTime"

                    Case "System.Byte[]"

                    Case Else
                        NewItemDataset.InventoryItem(0)(Me.ImportExtData5CustomField) = m_ASPStorefrontImportRule.GetXMLElementText(XMLProduct, strXMLPath)
                End Select

            Catch ex As Exception
                m_LastError = "Cannot convert import ASPStorefront ExtensionData5 field for Product SKU " & SKUToUse & " to Inventory Item " & Me.ImportExtData5CustomField & " Custom Field." & vbCrLf & vbCrLf & ex.Message ' TJS 02/12/11 TJS 24/08/12

            End Try
        End If
        ' end of code aadded TJS 29/03/11

        Return True

    End Function
#End Region

#Region " CopyMatrixItemASPSFSettings "
    Private Sub CopyMatrixItemASPSFSettings()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowASPStorefrontDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontDetails_DEV000221Row
        Dim strErrorDetails As String, iLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer

        For iLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
            rowASPStorefrontDetails = m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.NewInventoryASPStorefrontDetails_DEV000221Row
            For iColumnLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Columns.Count - 1
                If m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Columns(iColumnLoop).ColumnName = "ItemCode_DEV000221" Then
                    rowASPStorefrontDetails.ItemCode_DEV000221 = NewItemDataset.InventoryMatrixItem(iLoop).MatrixItemCode
                ElseIf m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Columns(iColumnLoop).ColumnName <> "Counter" Then
                    rowASPStorefrontDetails(iColumnLoop) = m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221(0)(iColumnLoop)
                End If
            Next
            m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.AddInventoryASPStorefrontDetails_DEV000221Row(rowASPStorefrontDetails)
        Next
        If Not Me.UpdateDataSet(New String()() {New String() {m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.TableName, _
            "CreateInventoryASPStorefrontDetails_DEV000221", "UpdateInventoryASPStorefrontDetails_DEV000221", "DeleteInventoryASPStorefrontDetails_DEV000221"}}, _
            Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
            strErrorDetails = ""
            For iRowLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Rows.Count - 1
                For iColumnLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Columns.Count - 1
                    If m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                        strErrorDetails = strErrorDetails & m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.TableName & _
                            "." & m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                            m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                    End If
                Next
            Next
            m_LastError = strErrorDetails ' TJS 02/12/11
            m_ImportLog = m_ImportLog & "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted - " & strErrorDetails & vbCrLf
        End If

    End Sub
#End Region

#Region " SaveItem "
    Private Function SaveItem(ByVal SiteID As String, ByRef NoOfProductsImported As Integer, ByRef NoOfProductsSkipped As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created using code from ImportASPStorefrontProducts
        ' 04/04/11 | TJS             | 2011.0.07 | Modified to get error details from SystemManufacturerSourceID_DEV000221 
        ' 02/12/11 | TJS             | 2011.2.00 | Replaced calls to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strErrorDetails As String, strTemp As String, iLoop As Integer
        Dim iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer

        NewItemFacade.IsFinished = True
        If NewItemFacade.UpdateDataSet(NewItemFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
            NewItemFacade.GenerateInventoryStockTotal("admin")
            NewItemFacade.UpdateStockTotal()
            If NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                'NewItemFacade.GenerateWebOptionDetail() **** TJS IS6 to do
            End If
            m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221(0).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            For iLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Count - 1
                m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221(iLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            Next
            For iLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Count - 1
                m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221(iLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
            Next


            If Me.UpdateDataSet(New String()() {New String() {m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.TableName, _
                    "CreateInventoryASPStorefrontDetails_DEV000221", "UpdateInventoryASPStorefrontDetails_DEV000221", "DeleteInventoryASPStorefrontDetails_DEV000221"}, _
                New String() {m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.TableName, "CreateInventoryASPStorefrontCategories_DEV000221", _
                    "UpdateInventoryASPStorefrontCategories_DEV000221", "DeleteInventoryASPStorefrontCategories_DEV000221"}, _
                New String() {m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.TableName, "CreateInventoryASPStorefrontTagDetails_DEV000221", _
                    "UpdateInventoryASPStorefrontTagDetails_DEV000221", "DeleteInventoryASPStorefrontTagDetails_DEV000221"}, _
                New String() {m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.TableName, "CreateSystemManufacturerSourceID_DEV000221", _
                    "UpdateSystemManufacturerSourceID_DEV000221", "DeleteSystemManufacturerSourceID_DEV000221"}}, _
                Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                NoOfProductsImported = NoOfProductsImported + 1
                Return True

            Else
                ' get error details
                strErrorDetails = ""
                For iRowLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Columns.Count - 1
                        If m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.TableName & _
                                "." & m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ASPStorefrontImportDataset.InventoryASPStorefrontDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                For iRowLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Columns.Count - 1
                        If m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.TableName & _
                                "." & m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ASPStorefrontImportDataset.InventoryASPStorefrontCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                For iRowLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Columns.Count - 1
                        If m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.TableName & _
                                "." & m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ASPStorefrontImportDataset.InventoryASPStorefrontTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                For iRowLoop = 0 To m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.Rows.Count - 1
                    For iColumnLoop = 0 To m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.Columns.Count - 1
                        If m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.TableName & _
                                "." & m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                m_ASPStorefrontImportDataset.SystemManufacturerSourceID_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
                m_LastError = strErrorDetails ' TJS 02/12/11
                m_ImportLog = m_ImportLog & "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted - " & strErrorDetails & vbCrLf
                NoOfProductsSkipped = NoOfProductsSkipped + 1

            End If
            strTemp = "UPDATE LerrynImportExportInventoryActionStatus_DEV000221 SET ActionComplete_DEV000221 = 1 "
            strTemp = strTemp & "WHERE ItemCode_DEV000221 = '" & NewItemDataset.InventoryItem(0).ItemCode
            strTemp = strTemp & "' AND SourceCode_DEV000221 = '" & ASP_STORE_FRONT_SOURCE_CODE
            strTemp = strTemp & "' AND StoreMerchantID_DEV000221 = '" & SiteID & "'"
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
            m_LastError = strErrorDetails ' TJS 02/12/11
            m_ImportLog = m_ImportLog & "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted - " & strErrorDetails & vbCrLf
            NoOfProductsSkipped = NoOfProductsSkipped + 1

        End If

        Return False

    End Function

    Private Function SaveItemKit() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strErrorDetails As String, iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer

        If NewKitFacade.UpdateDataSet(NewKitFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.InventoryItem, "Create Kit Item", False) Then
            Return True

        Else
            ' get error details
            strErrorDetails = ""
            For iTableLoop = 0 To NewKitFacade.CommandSet.Length - 1
                For iRowLoop = 0 To NewKitDataset.Tables(NewKitFacade.CommandSet(iTableLoop)(0)).Rows.Count - 1
                    For iColumnLoop = 0 To NewKitDataset.Tables(NewKitFacade.CommandSet(iTableLoop)(0)).Columns.Count - 1
                        If NewKitDataset.Tables(NewKitFacade.CommandSet(iTableLoop)(0)).Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                            strErrorDetails = strErrorDetails & NewKitDataset.Tables(NewKitFacade.CommandSet(iTableLoop)(0)).TableName & _
                                "." & NewKitDataset.Tables(NewKitFacade.CommandSet(iTableLoop)(0)).Columns(iColumnLoop).ColumnName & ", " & _
                                NewKitDataset.Tables(NewKitFacade.CommandSet(iTableLoop)(0)).Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                        End If
                    Next
                Next
            Next
            m_LastError = strErrorDetails ' TJS 02/12/11

        End If

        Return False

    End Function
#End Region

#Region " ApplyAttributes "
    Private Function ApplySizeAttributes(ByVal MatrixSizes As String(), ByVal MatrixSizeCodes As String()) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created 
        ' 31/03/11 | TJS             | 2011.0.05 | Modified to cater for over length values
        ' 04/04/11 | TJS             | 2011.0.07 | Modified to use ApplyAttributes function
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return ApplyAttributes("Size", "Size", MatrixSizes, MatrixSizeCodes)

    End Function

    Private Function ApplyColorAttributes(ByVal MatrixColors As String(), ByVal MatrixColorCodes As String()) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function created 
        ' 31/03/11 | TJS             | 2011.0.05 | Modified to cater for over length values
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return ApplyAttributes("Color", "Color", MatrixColors, MatrixColorCodes)

    End Function

    Private Function ApplyAttributes(ByVal AttributeName As String, ByVal AttributeCode As String, _
        ByVal MatrixValues As String(), ByVal MatrixValueCodes As String()) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/04/11 | TJS             | 2011.0.07 | Function created from code on ApplySizeAttributes and ApplyColorAttributes
        ' 05/04/11 | TJS             | 2011.0.08 | Modified to cater for IS 4.8 build using conditional compile
        ' 13/04/11 | TJs             | 2011.0.10 | Corrected sql for adding new attributes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strIsActive As String(), iLoop As Integer

        ' check if Size attribute exists in SystemAttribute and create if not
        strIsActive = GetRow(New String() {"IsActive"}, "SystemAttribute", "AttributeCode = '" & AttributeCode & "'", False)
        If strIsActive Is Nothing Then
            Me.ExecuteNonQuery(CommandType.Text, "INSERT INTO SystemAttribute (AttributeCode, AttributeDescription, UserCreated, DateCreated, UserModified, DateModified, IsPrinted, PrintCount, IsActive, MLID) VALUES ('" & AttributeCode & "', '" & AttributeName & "', 'admin', getdate(), 'admin', getdate(), 0, 0, 1, null)", Nothing) ' TJS 13/04/11
        ElseIf Not CBool(strIsActive(0)) Then
            Me.ExecuteNonQuery(CommandType.Text, "UPDATE SystemAttribute SET IsActive = 1 WHERE AttributeCode = '" & AttributeCode & "'", Nothing)
        End If
        ' now apply attribute to Matrix Item
        LoadDataSet(New String()() {New String() {m_ASPStorefrontImportDataset.SystemAttribute.TableName, "ReadSystemAttribute", _
          AT_ATTRIBUTE_CODE, AttributeCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 05/04/11
        NewItemFacade.AssignAttribute(Nothing, New System.Data.DataRow() {m_ASPStorefrontImportDataset.SystemAttribute(0)}) ' TJS 05/04/11
        ' now process each Size value in turn
        For iLoop = 0 To MatrixValues.Length - 1
            ' is value too big for DB field ?
            If MatrixValueCodes(iLoop).Length > 10 Then ' TJS 31/03/11
                ' yes, truncate it
                m_ImportLog = m_ImportLog & AttributeName & " attribute value too long - " & MatrixValueCodes(iLoop) & vbCrLf ' TJS 31/03/11
                Return False ' TJS 31/03/11
            End If
            ' check if attribute value exists in SystemAttributeValue and create if not
            strIsActive = GetRow(New String() {"IsActive"}, "SystemAttributeValue", "AttributeCode = '" & AttributeCode & "' AND AttributeValueCode = '" & MatrixValueCodes(iLoop) & "'", False)
            If strIsActive Is Nothing OrElse strIsActive(0) = "" Then
                Me.ExecuteNonQuery(CommandType.Text, "INSERT INTO SystemAttributeValue (AttributeCode, AttributeValueCode, AttributeValueDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified, MLID) VALUES ('" & AttributeCode & "', '" & MatrixValueCodes(iLoop) & "','" & MatrixValues(iLoop) & "', 1, 'admin', getdate(), 'admin', getdate(), null)", Nothing)
            ElseIf Not CBool(strIsActive(0)) Then
                Me.ExecuteNonQuery(CommandType.Text, "UPDATE SystemAttributeValue SET IsActive = 1 WHERE AttributeCode = '" & AttributeCode & "' AND AttributeValueCode = '" & MatrixValueCodes(iLoop) & "'", Nothing)
            End If
            ' now apply attribute value to Matrix Item
            LoadDataSet(New String()() {New String() {m_ASPStorefrontImportDataset.SystemAttributeValue.TableName, "ReadSystemAttributeValue", _
                AT_ATTRIBUTE_CODE, AttributeCode, AT_ATTRIBUTE_VALUE_CODE, MatrixValueCodes(iLoop)}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 05/04/11
            NewItemFacade.AssignAttributeValue(Nothing, New System.Data.DataRow() {m_ASPStorefrontImportDataset.SystemAttributeValue(0)}) ' TJS 05/04/11
        Next
        Return True

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
        ' 29/03/11 | TJS             | 2011.0.04 | Function created 
        ' 02/12/11 | TJS             | 2011.2.00 | Added Channel Advisor to count
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon to count
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return CInt(Me.GetField("SELECT (SELECT COUNT(*) FROM InventoryAmazonDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryASPStorefrontDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryChannelAdvDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryMagentoDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1)", CommandType.Text, Nothing)) ' TJS 02/12/11 TJS 05/07/12

    End Function
#End Region
#End Region

End Class
#End Region
