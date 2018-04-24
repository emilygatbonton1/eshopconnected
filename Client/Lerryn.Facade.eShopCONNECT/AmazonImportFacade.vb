' eShopCONNECT for Connected Business
' Module: AmazonImportFacade.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Connected Business SDK and may incorporate certain intellectual 
' property of Interprise Solutions Inc. who's
' rights are hereby recognised.

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
Imports System.IO
Imports System.Xml.Linq
Imports System.Xml.XPath

#Region " AmazonImportFacade "
Public Class AmazonImportFacade
    Inherits Interprise.Facade.Base.BaseFacade
    Implements Interprise.Extendable.Base.Facade.IBaseInterface

#Region " Variables "
    Private m_AmazonImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_AmazonImportRule As Lerryn.Facade.ImportExport.ImportExportFacade
    Private AmazonConnection As Lerryn.Facade.ImportExport.AmazonMWSConnector
    Private ActiveSource As Lerryn.Framework.ImportExport.SourceConfig.SourceSettings
    Private ActiveAmazonSource As Lerryn.Framework.ImportExport.SourceConfig.AmazonSettings

    Private NewItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
    Private WithEvents NewItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_AmazonThrottling As Lerryn.Facade.ImportExport.AmazonMWSThrottling ' TJS 03/07/13

    Private m_ProductsAlreadyImported As Integer = 0
    Private m_ImportLog As String
    Private m_LastError As String
    Private m_ImportLimitReached As Boolean
    Private m_QuantityPublishingType As String
    Private m_QuantityPublishingValue As Decimal
    Private m_BaseProductCode As String
    Private m_BaseProductName As String
    Private m_FieldNames As String()
    Private m_FieldValues As String()
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return m_AmazonImportDataset
        End Get
    End Property
#End Region

#Region " CurrentBusinessRule "
    Public Overrides ReadOnly Property CurrentBusinessRule() As Interprise.Extendable.Base.Business.IBaseInterface
        Get
            Return m_AmazonImportRule
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

#Region " LastErrorMessage "
    ' Last error message (e.g. from web server)
    Private m_LastErrorMessage As String
    Public ReadOnly Property LastErrorMessage() As String
        Get
            Return m_LastErrorMessage
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
    Public Sub New(ByVal p_AmazonImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef p_ErrorNotification As Lerryn.Facade.ImportExport.ErrorNotification, ByVal p_BaseProductCode As String, ByVal p_BaseProductName As String) ' TJS 10/06/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()
        m_AmazonImportDataset = p_AmazonImportDataset
        m_BaseProductCode = p_BaseProductCode
        m_BaseProductName = p_BaseProductName

        m_AmazonImportRule = New Lerryn.Facade.ImportExport.ImportExportFacade(p_AmazonImportDataset, p_ErrorNotification, p_BaseProductCode, p_BaseProductName)
        MyBase.InitializeDataset()

        ' read all licences as we need base licence and all add-ons
        Me.LoadDataSet(New String()() {New String() {m_AmazonImportDataset.LerrynLicences_DEV000221.TableName, _
            "ReadLerrynLicences_DEV000221"}, New String() {m_AmazonImportDataset.System_DEV000221.TableName, _
            "ReadSystem_DEV000221"}, New String() {m_AmazonImportDataset.SystemCompanyInformation.TableName, _
            "ReadSystemCompanyInformation"}, New String() {m_AmazonImportDataset.LerrynImportExportConfig_DEV000221.TableName, _
            "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        m_AmazonImportRule.CheckActivation()
        If m_AmazonImportRule.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then ' TJS 03/07/13
            m_AmazonThrottling = New Lerryn.Facade.ImportExport.AmazonMWSThrottling
        End If

    End Sub
#End Region

#Region " CheckForExistingProductListingReport "
    Public Function CheckForExistingProductListingReport(ByVal AmazonMerchantToken As String, ByRef ExistingReportRequestID As String, _
        ByRef ExistingRequestDate As Date) As Boolean ' TJS 22/03/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Original
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple Merchant accounts
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strErrorDetails As String, iMerchantLoop As Integer, bMerchantIDMatched As Boolean
        Dim bFileImportRequired As Boolean, bWebIORequired As Boolean, bReturnValue As Boolean

        Try
            bReturnValue = False
            m_LastErrorMessage = ""
            m_LastError = ""
            strErrorDetails = ""
            bMerchantIDMatched = False ' TJS 01/04/14
            If m_AmazonImportRule.IsActivated Then
                ActiveSource = New Lerryn.Framework.ImportExport.SourceConfig.SourceSettings
                If m_AmazonImportRule.LoadConfigSettings(m_AmazonImportDataset.LerrynImportExportConfig_DEV000221(0), ActiveSource, bFileImportRequired, bWebIORequired, strErrorDetails) Then
                    For iMerchantLoop = 0 To ActiveSource.AmazonSettingCount - 1
                        If ActiveSource.AmazonSettingCount = 1 Then
                            bMerchantIDMatched = True
                        ElseIf ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken = AmazonMerchantToken Then ' TJS 22/03/13
                            bMerchantIDMatched = True
                        End If
                        If bMerchantIDMatched Then
                            ActiveAmazonSource = ActiveSource.AmazonSettings(iMerchantLoop)
                            If ActiveAmazonSource.AmazonSite <> "" And ActiveAmazonSource.MWSMerchantID <> "" And ActiveAmazonSource.MWSMarketplaceID <> "" Then
                                LoadDataSet(New String()() {New String() {m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                                    "ReadLerrynImportExportAmazonFiles_DEV000221", AT_MERCHANT_ID, ActiveAmazonSource.MerchantToken, _
                                    AT_AMAZON_MESSAGE_TYPE, "_GET_MERCHANT_LISTINGS_DATA_", AT_SITE_ID, ActiveAmazonSource.AmazonSite, _
                                    AT_FILE_SENT_TO_AMAZON, "1", AT_FILE_RESPONSE_RECEIVED, "0"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 22/03/13
                                If m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221.Count > 0 Then
                                    ExistingReportRequestID = m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221(m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1).AmazonDocumentID_DEV000221
                                    ExistingRequestDate = m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221(m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1).DateCreated
                                    bReturnValue = True
                                End If
                            End If
                            Exit For ' TJS 01/04/14
                        End If
                    Next
                End If
            End If
            Return bReturnValue

        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = ex.Message
            Return False

        End Try

    End Function
#End Region

#Region " RequestAmazonProductListingReport "
    Public Function RequestAmazonProductListingReport(ByRef ReportRequestID As String, ByRef Cancel As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Original
        ' 02/08/12 | TJS             | 2012.1.11 | Modified to set submission timestamp
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row
        Dim strErrorDetails As String, dteSubmissionTimestamp As Date, bReturnValue As Boolean ' TJS 02/08/12

        Try
            bReturnValue = False
            m_LastErrorMessage = ""
            m_LastError = ""
            strErrorDetails = ""
            AmazonConnection = New Lerryn.Facade.ImportExport.AmazonMWSConnector(m_AmazonThrottling) ' TJS 03/07/13
            ReportRequestID = AmazonConnection.RequestAmazonReport("_GET_MERCHANT_LISTINGS_DATA_", m_BaseProductName, ActiveSource, ActiveAmazonSource, dteSubmissionTimestamp, strErrorDetails) ' TJS 02/08/12
            If Not String.IsNullOrEmpty(ReportRequestID) Then
                rowAmazonFile = m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221.NewLerrynImportExportAmazonFiles_DEV000221Row
                rowAmazonFile.AmazonDocumentID_DEV000221 = ReportRequestID
                rowAmazonFile.SiteCode_DEV000221 = ActiveAmazonSource.AmazonSite
                rowAmazonFile.MerchantID_DEV000221 = ActiveAmazonSource.MerchantToken ' TJS 22/03/13
                rowAmazonFile.FileName_DEV000221 = ReportRequestID
                rowAmazonFile.AmazonMessageType_DEV000221 = "_GET_MERCHANT_LISTINGS_DATA_"
                rowAmazonFile.FileIsInputFromAmazon_DEV000221 = False
                rowAmazonFile.FileSentToAmazon_DEV000221 = True
                rowAmazonFile.ResponseReceived_DEV000221 = False
                rowAmazonFile.FileContent_DEV000221 = "Report Requested"
                rowAmazonFile.Processed_DEV000221 = False ' TJS 02/08/12
                rowAmazonFile.SubmissionTimestamp_DEV000221 = dteSubmissionTimestamp ' TJS 02/08/12
                m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221.AddLerrynImportExportAmazonFiles_DEV000221Row(rowAmazonFile)
                UpdateDataSet(New String()() {New String() {m_AmazonImportDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                    "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                    "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                    "Save Amazon Report Request", False)
                bReturnValue = True

            Else
                m_LastErrorMessage = strErrorDetails
                m_LastError = strErrorDetails

            End If
            Return bReturnValue

        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = ex.Message
            Return False

        End Try

    End Function
#End Region

#Region " PollAmazonProductListingReport "
    Public Function PollAmazonProductListingReport(ByVal AmazonReportRequestID As String, ByRef Cancel As Boolean, ByVal InhibitWebPosts As Boolean, ByRef ItemsForImport As DataTable) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Original
        ' 02/08/12 | TJS             | 2012.1.11 | Modified to set Processed flag on report request to prevent server trying to process the data
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for non-stock items
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 03/07/13 | TJS             | 2013.1.24 | Modified to cater for Amazon throttling
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to cater for SKUChanged in other import facades
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim reportStream As MemoryStream, reader As StreamReader
        Dim colNewColumn As DataColumn, rowImportItems As System.Data.DataRow
        Dim strImportedProducts As String()()
        Dim strErrorDetails As String = "", strAmazonReportID As String = "", strSQL As String
        Dim strReportLine As String, strFieldValue As String, iLoop As Integer, bReturnValue As Boolean

        Try
            If AmazonConnection Is Nothing Then
                AmazonConnection = New Lerryn.Facade.ImportExport.AmazonMWSConnector(m_AmazonThrottling) ' TJS 03/07/13
            End If
            reportStream = New MemoryStream
            If AmazonConnection.PollAmazonForReport(m_BaseProductName, ActiveSource, ActiveAmazonSource, AmazonReportRequestID, InhibitWebPosts, strAmazonReportID, reportStream, strErrorDetails) Then
                strSQL = "UPDATE LerrynImportExportAmazonFiles_DEV000221 SET ResponseReceived_DEV000221 = 1, ReceivedDocumentID_DEV000221 = '" & strAmazonReportID & _
                    "', Processed_DEV000221 = 1 WHERE AmazonDocumentID_DEV000221 = '" & AmazonReportRequestID & "' AND MerchantID_DEV000221 = '" & ActiveAmazonSource.MerchantToken & _
                    "' AND SiteCode_DEV000221 = '" & ActiveAmazonSource.AmazonSite & "' AND AmazonMessageType_DEV000221 = '_GET_MERCHANT_LISTINGS_DATA_'" ' TJS 02/08/12 TJS 22/03/13
                ExecuteNonQuery(CommandType.Text, strSQL, Nothing)

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
                colNewColumn.Caption = "Amazon Type"
                colNewColumn.ColumnName = "SourceType"
                colNewColumn.DataType = System.Type.GetType("System.String")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "Amazon Listing ID"
                colNewColumn.ColumnName = "SourceItemID"
                colNewColumn.DataType = System.Type.GetType("System.String")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                ' this column is needed on Channel Advisor
                colNewColumn = New DataColumn
                colNewColumn.Caption = "Not Used"
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
                colNewColumn.Caption = "SKU Changed"
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

                ' this column is needed on Channel Advisor and Amazon
                colNewColumn = New DataColumn
                colNewColumn.Caption = "ASIN"
                colNewColumn.ColumnName = "ASIN"
                colNewColumn.DataType = System.Type.GetType("System.String")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "Item Description"
                colNewColumn.ColumnName = "ItemDescription"
                colNewColumn.DataType = System.Type.GetType("System.String")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "Selling Price"
                colNewColumn.ColumnName = "SellingPrice"
                colNewColumn.DataType = System.Type.GetType("System.Decimal")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "Quantity"
                colNewColumn.ColumnName = "Quantity"
                colNewColumn.DataType = System.Type.GetType("System.Int32")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "Condition"
                colNewColumn.ColumnName = "Condition"
                colNewColumn.DataType = System.Type.GetType("System.Int32")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "ASIN2"
                colNewColumn.ColumnName = "ASIN2"
                colNewColumn.DataType = System.Type.GetType("System.String")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "ASIN3"
                colNewColumn.ColumnName = "ASIN3"
                colNewColumn.DataType = System.Type.GetType("System.String")
                ItemsForImport.Columns.Add(colNewColumn)
                colNewColumn.Dispose()

                colNewColumn = New DataColumn
                colNewColumn.Caption = "Fulfillment"
                colNewColumn.ColumnName = "Fulfillment"
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

                strImportedProducts = Me.GetRows(New String() {"ProductID_DEV000221", "ItemName", "ItemCode"}, "InventoryItemAmazonSummaryView_DEV000221")
                m_ProductsAlreadyImported = 0

                reader = New StreamReader(reportStream)
                strReportLine = reader.ReadLine
                m_FieldNames = strReportLine.Split(CChar(vbTab))

                Do While Not reader.EndOfStream
                    strReportLine = reader.ReadLine
                    m_FieldValues = strReportLine.Split(CChar(vbTab))

                    rowImportItems = ItemsForImport.NewRow
                    rowImportItems.Item("Import") = True
                    rowImportItems.Item("AlreadyImported") = False
                    rowImportItems.Item("SKUExists") = False
                    rowImportItems.Item("SKUError") = False
                    rowImportItems.Item("SKUChanged") = False
                    rowImportItems.Item("SourceIDChanged") = False ' TJS 09/08/13
                    rowImportItems.Item("ImportAsKit") = False

                    strFieldValue = GetRowFieldValue("listing-id")
                    rowImportItems.Item("SourceItemID") = strFieldValue
                    For Each ImportedProduct As String() In strImportedProducts
                        If ImportedProduct(0) = strFieldValue Then
                            rowImportItems.Item("Import") = False
                            rowImportItems.Item("AlreadyImported") = True
                            rowImportItems.Item("ItemCode") = ImportedProduct(2)
                            Exit For
                        End If
                    Next

                    strFieldValue = GetRowFieldValue("seller-sku")
                    rowImportItems.Item("ItemSKU") = strFieldValue
                    For Each ImportedProduct As String() In strImportedProducts
                        If ImportedProduct(1) = strFieldValue Then
                            If rowImportItems.IsNull("SourceItemID") Or String.IsNullOrWhiteSpace(ImportedProduct(0)) Then
                                rowImportItems.Item("SKUExists") = True
                                rowImportItems.Item("ItemCode") = ImportedProduct(2)

                            ElseIf rowImportItems.Item("SourceItemID").ToString <> ImportedProduct(0) Then
                                rowImportItems.Item("Import") = False
                                rowImportItems.Item("SKUChanged") = True
                                rowImportItems.Item("ItemCode") = ImportedProduct(2)
                            End If
                            Exit For
                        End If
                    Next

                    rowImportItems.Item("ItemName") = GetRowFieldValue("item-name")
                    rowImportItems.Item("ItemDescription") = GetRowFieldValue("item-description")
                    rowImportItems.Item("SourceType") = "standard"
                    rowImportItems.Item("ASIN") = GetRowFieldValue("asin1")
                    rowImportItems.Item("ASIN2") = GetRowFieldValue("asin2")
                    rowImportItems.Item("ASIN3") = GetRowFieldValue("asin3")
                    strFieldValue = GetRowFieldValue("price")
                    If Not String.IsNullOrEmpty(strFieldValue) Then
                        rowImportItems.Item("SellingPrice") = CDec(strFieldValue)
                    Else
                        rowImportItems.Item("SellingPrice") = 0
                    End If
                    strFieldValue = GetRowFieldValue("price")
                    If Not String.IsNullOrEmpty(strFieldValue) Then
                        rowImportItems.Item("Quantity") = CInt(strFieldValue)
                    Else
                        rowImportItems.Item("Quantity") = 0
                    End If
                    strFieldValue = GetRowFieldValue("item-condition")
                    If Not String.IsNullOrEmpty(strFieldValue) Then
                        rowImportItems.Item("Condition") = CInt(strFieldValue)
                    Else
                        rowImportItems.Item("Condition") = 0
                    End If
                    rowImportItems.Item("Fulfillment") = GetRowFieldValue("fulfillment-channel")

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
                                If ItemsForImport.Rows(iLoop).Item("SourceItemID").ToString.Trim.ToLower = rowImportItems.Item("SourceItemID").ToString.Trim.ToLower Then
                                    ItemsForImport.Rows.Remove(rowImportItems)
                                Else
                                    rowImportItems.SetColumnError("ItemSKU", "Duplicate SKU")
                                    rowImportItems.Item("Import") = False
                                    rowImportItems.Item("SKUError") = True
                                    ItemsForImport.Rows(iLoop).SetColumnError("ItemSKU", "Duplicate SKU")
                                    ItemsForImport.Rows(iLoop).Item("Import") = False
                                    ItemsForImport.Rows(iLoop).Item("SKUError") = True
                                End If
                            End If
                        Next
                    End If

                Loop
                m_ProductsAlreadyImported = GetInventoryItemsImportedCount() ' TJS 29/03/11
                If ItemsForImport.Rows.Count > 0 Then
                    bReturnValue = True
                End If

            Else
                m_LastError = strErrorDetails
                m_LastErrorMessage = strErrorDetails

            End If
            Return bReturnValue

        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = ex.Message
            Return False

        End Try

    End Function
#End Region

#Region " ImportAmazonProducts "
    Public Sub ImportAmazonProducts(ByRef ItemsForImport As DataTable, ByRef Cancel As Boolean, ByRef NoOfProductsImported As Integer, ByRef NoOfProductsSkipped As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Original
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryAmazonDetails_DEV000221Row
        Dim rowAmazonASIN As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryAmazonASIN_DEV000221Row
        Dim rowBasePricingDetail As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway.InventoryItemPricingDetailRow
        Dim strErrorDetails As String, strHomeCurrency As String, strHomeLanguage As String
        Dim strTemp As String, strProductType As String, strItemCode As String
        Dim iLoop As Integer, iItemLoop As Integer, iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer
        Dim bISItemExists As Boolean, bISAmazonItemExists As Boolean

        Try
            m_LastErrorMessage = ""
            m_LastError = ""
            m_ImportLimitReached = False
            m_ImportLog = ""
            If m_AmazonImportRule.IsActivated Then
                strErrorDetails = ""
                m_ProductsAlreadyImported = GetInventoryItemsImportedCount()
                If Not m_AmazonImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                    strHomeCurrency = Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                    strHomeLanguage = Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)

                    NewItemDataset = New Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
                    NewItemFacade = New Interprise.Facade.Inventory.ItemDetailFacade(NewItemDataset)

                    For iItemLoop = 0 To ItemsForImport.Rows.Count - 1
                        ' is Import box checked and not already imported and SKU not changed ?
                        If CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And Not CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) Then
                            ' yes
                            strProductType = ItemsForImport.Rows(iItemLoop).Item("SourceType").ToString

                            ' check Item Name doesn't already exist
                            bISItemExists = False
                            strItemCode = ""
                            strItemCode = Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & "'")
                            If strItemCode <> "" Then
                                bISItemExists = True
                            End If
                            If bISItemExists Then
                                ' load existing item
                                NewItemFacade.LoadInventoryItem(strItemCode)
                                NewItemFacade.LoadItemDescription(strItemCode, strHomeLanguage)

                            Else
                                NewItemDataset.EnforceConstraints = False
                                NewItemDataset.InventoryMatrixGroup.Clear()
                                NewItemDataset.InventoryAttribute.Clear()
                                NewItemDataset.InventoryAttributeValue.Clear()
                                NewItemDataset.InventoryAssembly.Clear()
                                NewItemDataset.InventoryKit.Clear()
                                NewItemDataset.InventoryAssemblyDetailView.Clear()
                                NewItemDataset.InventoryItemPricingDetail.Clear()
                                NewItemDataset.EnforceConstraints = True
                                ' create default non-stock item records
                                NewItemFacade.AddItem(TEMPORARY_DOCUMENTCODE, ITEM_DEFAULT_CLASS_STOCK, ITEM_TYPE_STOCK)
                                NewItemFacade.LanguageCode = strHomeLanguage
                                NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_STOCK)
                                NewItemFacade.AddWebOption()
                            End If

                            bISAmazonItemExists = False
                            strItemCode = Me.GetField("ItemCode_DEV000221", "InventoryAmazonDetails_DEV000221", "ProductID_DEV000221 = '" & ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString & "' AND MerchantID_DEV000221 = '" & ActiveAmazonSource.MerchantToken & "'") ' TJS 22/03/13
                            If Not String.IsNullOrEmpty(strItemCode) Then
                                bISAmazonItemExists = True
                                Me.LoadDataSet(New String()() {New String() {m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.TableName, _
                                        "ReadInventoryAmazonDetails_DEV000221", AT_ITEMCODE, strItemCode, AT_MERCHANT_ID, ActiveAmazonSource.MerchantToken}, _
                                    New String() {m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.TableName, "ReadInventoryAmazonTagDetails_DEV000221", _
                                        AT_ITEMCODE, strItemCode, AT_MERCHANT_ID, ActiveAmazonSource.MerchantToken}, _
                                    New String() {m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.TableName, "ReadInventoryAmazonASIN_DEV000221", _
                                        AT_ITEMCODE, strItemCode, AT_MERCHANT_ID, ActiveAmazonSource.MerchantToken}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 22/03/13
                            End If
                            If bISAmazonItemExists Then
                                rowAmazonDetails = m_AmazonImportDataset.InventoryAmazonDetails_DEV000221(0)
                                If rowAmazonDetails.SellingPrice_DEV000221 <> CDec(ItemsForImport.Rows(iItemLoop).Item("SellingPrice")) Then
                                    rowAmazonDetails.SellingPrice_DEV000221 = CDec(ItemsForImport.Rows(iItemLoop).Item("SellingPrice"))
                                End If
                                If ItemsForImport.Rows(iItemLoop).Item("SellingPrice").ToString = "AMAZON_NA" Then
                                    If rowAmazonDetails.IsFulfillment_DEV000221Null OrElse rowAmazonDetails.Fulfillment_DEV000221 <> "Amazon_FBA" Then
                                        rowAmazonDetails.Fulfillment_DEV000221 = "Amazon_FBA"
                                    End If
                                Else
                                    If rowAmazonDetails.IsFulfillment_DEV000221Null OrElse rowAmazonDetails.Fulfillment_DEV000221 <> "Merchant" Then
                                        rowAmazonDetails.Fulfillment_DEV000221 = "Merchant"
                                    End If
                                End If
                            Else
                                rowAmazonDetails = m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.NewInventoryAmazonDetails_DEV000221Row
                                rowAmazonDetails.MerchantID_DEV000221 = ActiveAmazonSource.MerchantToken ' TJS 22/03/13
                                rowAmazonDetails.SiteCode_DEV000221 = ActiveAmazonSource.AmazonSite
                                rowAmazonDetails.ProductID_DEV000221 = ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString
                                rowAmazonDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
                                rowAmazonDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION ' TJS 13/11/13
                                rowAmazonDetails.BrowseNodeID_DEV000221 = "-1"
                                rowAmazonDetails.SellingPrice_DEV000221 = CDec(ItemsForImport.Rows(iItemLoop).Item("SellingPrice"))
                                If ItemsForImport.Rows(iItemLoop).Item("SellingPrice").ToString = "AMAZON_NA" Then
                                    rowAmazonDetails.Fulfillment_DEV000221 = "Amazon_FBA"
                                Else
                                    rowAmazonDetails.Fulfillment_DEV000221 = "Merchant"
                                End If
                                rowAmazonDetails.FromImportWizard_DEV000221 = True
                                rowAmazonDetails.QtyPublishingType_DEV000221 = m_QuantityPublishingType
                                rowAmazonDetails.QtyPublishingValue_DEV000221 = CInt(m_QuantityPublishingValue)
                                rowAmazonDetails.TotalQtyWhenLastPublished_DEV000221 = 0
                                rowAmazonDetails.QtyLastPublished_DEV000221 = 0
                            End If
                            If NewItemDataset.InventoryItem(0).IsItemNameNull OrElse NewItemDataset.InventoryItem(0).ItemName <> ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString Then
                                NewItemDataset.InventoryItem(0).ItemName = ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString
                            End If
                            If NewItemDataset.InventoryItem(0).IsItemDescriptionNull OrElse NewItemDataset.InventoryItem(0).ItemDescription <> ItemsForImport.Rows(iItemLoop).Item("ItemName").ToString Then
                                NewItemDataset.InventoryItem(0).ItemDescription = ItemsForImport.Rows(iItemLoop).Item("ItemName").ToString
                            End If
                            For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
                                If NewItemDataset.InventoryItemDescription(iLoop).IsItemDescriptionNull OrElse NewItemDataset.InventoryItemDescription(iLoop).ItemDescription <> NewItemDataset.InventoryItem(0).ItemDescription Then
                                    NewItemDataset.InventoryItemDescription(iLoop).ItemDescription = NewItemDataset.InventoryItem(0).ItemDescription
                                End If
                                If Not String.IsNullOrEmpty(ItemsForImport.Rows(iItemLoop).Item("ItemDescription").ToString) Then
                                    If NewItemDataset.InventoryItemDescription(iLoop).IsExtendedDescriptionNull OrElse NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription <> ItemsForImport.Rows(iItemLoop).Item("ItemDescription").ToString Then
                                        NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription = ItemsForImport.Rows(iItemLoop).Item("ItemDescription").ToString
                                    End If
                                End If
                            Next
                            rowAmazonDetails.Publish_DEV000221 = True
                            rowAmazonDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                            If Not bISAmazonItemExists Then
                                m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.AddInventoryAmazonDetails_DEV000221Row(rowAmazonDetails)
                            End If
                            NewItemFacade.IsAutoRecalculate = True
                            rowBasePricingDetail = NewItemDataset.InventoryItemPricingDetail.FindByCurrencyCodeItemCode(strHomeCurrency, NewItemDataset.InventoryItem(0).ItemCode)
                            If rowBasePricingDetail IsNot Nothing Then
                                rowBasePricingDetail.PricingCostRate = rowAmazonDetails.SellingPrice_DEV000221
                                ' PricingCost wasn't set properly for the base currency in 5.6 so set it specifically
                                rowBasePricingDetail.PricingCost = rowBasePricingDetail.PricingCostRate
                                rowBasePricingDetail.RetailPriceRate = rowAmazonDetails.SellingPrice_DEV000221
                                rowBasePricingDetail.WholesalePriceRate = rowAmazonDetails.SellingPrice_DEV000221
                            Else
                                m_LastError = "No price records created"
                                m_LastErrorMessage = m_LastError
                                m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as no price records created" & vbCrLf

                            End If
                            NewItemFacade.ReComputePricing(NewItemDataset.InventoryItem(0).ItemCode, strHomeCurrency, NewItemFacade.IsAutoRecalculate)
                            Try
                                NewItemFacade.RecomputePriceListOnUpdatePricingCost(NewItemDataset.InventoryItem(0).ItemCode, rowAmazonDetails.SellingPrice_DEV000221)
                            Catch ex As Exception
                            End Try
                            NewItemFacade.IsAutoRecalculate = False

                            If Not String.IsNullOrEmpty(ItemsForImport.Rows(iItemLoop).Item("ASIN").ToString) Then
                                rowAmazonASIN = m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.FindByItemCode_DEV000221ASIN_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, ItemsForImport.Rows(iItemLoop).Item("ASIN").ToString)
                                If rowAmazonASIN Is Nothing Then
                                    rowAmazonASIN = m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.NewInventoryAmazonASIN_DEV000221Row
                                    rowAmazonASIN.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                    rowAmazonASIN.ASIN_DEV000221 = ItemsForImport.Rows(iItemLoop).Item("ASIN").ToString
                                    m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.AddInventoryAmazonASIN_DEV000221Row(rowAmazonASIN)
                                End If
                            End If
                            If Not String.IsNullOrEmpty(ItemsForImport.Rows(iItemLoop).Item("ASIN2").ToString) Then
                                rowAmazonASIN = m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.FindByItemCode_DEV000221ASIN_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, ItemsForImport.Rows(iItemLoop).Item("ASIN2").ToString)
                                If rowAmazonASIN Is Nothing Then
                                    rowAmazonASIN = m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.NewInventoryAmazonASIN_DEV000221Row
                                    rowAmazonASIN.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                    rowAmazonASIN.ASIN_DEV000221 = ItemsForImport.Rows(iItemLoop).Item("ASIN2").ToString
                                    m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.AddInventoryAmazonASIN_DEV000221Row(rowAmazonASIN)
                                End If
                            End If
                            If Not String.IsNullOrEmpty(ItemsForImport.Rows(iItemLoop).Item("ASIN3").ToString) Then
                                rowAmazonASIN = m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.FindByItemCode_DEV000221ASIN_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, ItemsForImport.Rows(iItemLoop).Item("ASIN3").ToString)
                                If rowAmazonASIN Is Nothing Then
                                    rowAmazonASIN = m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.NewInventoryAmazonASIN_DEV000221Row
                                    rowAmazonASIN.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                    rowAmazonASIN.ASIN_DEV000221 = ItemsForImport.Rows(iItemLoop).Item("ASIN3").ToString
                                    m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.AddInventoryAmazonASIN_DEV000221Row(rowAmazonASIN)
                                End If
                            End If

                            NewItemFacade.IsFinished = True
                            If NewItemFacade.UpdateDataSet(NewItemFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                                NewItemFacade.GenerateInventoryStockTotal("admin")
                                NewItemFacade.UpdateStockTotal()
                                rowAmazonDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                For iLoop = 0 To m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.Count - 1
                                    If m_AmazonImportDataset.InventoryAmazonASIN_DEV000221(iLoop).ItemCode_DEV000221 <> NewItemDataset.InventoryItem(0).ItemCode Then
                                        m_AmazonImportDataset.InventoryAmazonASIN_DEV000221(iLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                    End If
                                Next
                                For iLoop = 0 To m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.Count - 1
                                    If m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221(iLoop).ItemCode_DEV000221 <> NewItemDataset.InventoryItem(0).ItemCode Then
                                        m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221(iLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                    End If
                                Next
                                If Me.UpdateDataSet(New String()() {New String() {m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.TableName, _
                                        "CreateInventoryAmazonDetails_DEV000221", "UpdateInventoryAmazonDetails_DEV000221", "DeleteInventoryAmazonDetails_DEV000221"}, _
                                    New String() {m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.TableName, _
                                        "CreateInventoryAmazonTagDetails_DEV000221", "UpdateInventoryAmazonTagDetails_DEV000221", "DeleteInventoryAmazonTagDetails_DEV000221"}, _
                                    New String() {m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.TableName, _
                                        "CreateInventoryAmazonASIN_DEV000221", "UpdateInventoryAmazonASIN_DEV000221", "DeleteInventoryAmazonASIN_DEV000221"}}, _
                                    Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                                    NoOfProductsImported = NoOfProductsImported + 1
                                    m_ImportLog = m_ImportLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                    m_ProductsAlreadyImported = m_ProductsAlreadyImported + 1

                                Else
                                    ' get error details
                                    strErrorDetails = ""
                                    For iRowLoop = 0 To m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.Columns.Count - 1
                                            If m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.TableName & _
                                                    "." & m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_AmazonImportDataset.InventoryAmazonDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    For iRowLoop = 0 To m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.Columns.Count - 1
                                            If m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.TableName & _
                                                    "." & m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_AmazonImportDataset.InventoryAmazonTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    For iRowLoop = 0 To m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.Columns.Count - 1
                                            If m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.TableName & _
                                                    "." & m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_AmazonImportDataset.InventoryAmazonASIN_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    m_LastError = strErrorDetails
                                    m_LastErrorMessage = m_LastError
                                    m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted - " & strErrorDetails & vbCrLf
                                    NoOfProductsSkipped = NoOfProductsSkipped + 1

                                End If
                                strTemp = "UPDATE LerrynImportExportInventoryActionStatus_DEV000221 SET ActionComplete_DEV000221 = 1 "
                                strTemp = strTemp & "WHERE ItemCode_DEV000221 = '" & NewItemDataset.InventoryItem(0).ItemCode
                                strTemp = strTemp & "' AND SourceCode_DEV000221 = '" & AMAZON_SOURCE_CODE & "' AND StoreMerchantID_DEV000221 = '"
                                strTemp = strTemp & ActiveAmazonSource.MerchantToken & "'" ' TJS 22/03/13
                                Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing)

                            Else
                                ' get error details
                                strErrorDetails = ""
                                For iTableLoop = 0 To NewItemDataset.Tables.Count - 1
                                    For iRowLoop = 0 To NewItemDataset.Tables(iTableLoop).Rows.Count - 1
                                        For iColumnLoop = 0 To NewItemDataset.Tables(iTableLoop).Columns.Count - 1
                                            If NewItemDataset.Tables(iTableLoop).Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & NewItemDataset.Tables(iTableLoop).TableName & _
                                                    "." & NewItemDataset.Tables(iTableLoop).Columns(iColumnLoop).ColumnName & ", " & _
                                                    NewItemDataset.Tables(iTableLoop).Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                Next
                                m_LastError = strErrorDetails
                                m_LastErrorMessage = m_LastError
                                m_ImportLog = m_ImportLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted - " & strErrorDetails & vbCrLf
                                NoOfProductsSkipped = NoOfProductsSkipped + 1
                            End If

                        ElseIf CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And Not CBool(ItemsForImport.Rows(iItemLoop).Item("AlreadyImported")) And _
                            CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) Then
                            ' product SKU has change, user has selected import so update SKU in IS
                            strTemp = "UPDATE InventoryItem SET ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString
                            strTemp = strTemp & "' WHERE ItemCode = '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'"
                            Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing)

                        End If

                        If m_AmazonImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                            m_LastError = strErrorDetails
                            m_LastErrorMessage = m_LastError
                            m_ImportLimitReached = True
                            Exit For
                        End If
                    Next
                Else
                    m_LastError = strErrorDetails
                    m_LastErrorMessage = m_LastError
                    m_ImportLimitReached = True
                End If
            End If

        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = ex.Message

        End Try

    End Sub
#End Region

#Region " GetRowFieldValue "
    Private Function GetRowFieldValue(ByVal FieldName As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iFieldPosn As Integer

        For iFieldPosn = 0 To m_FieldNames.Length - 1
            If m_FieldNames(iFieldPosn) = FieldName Then
                Return m_FieldValues(iFieldPosn)
            End If
        Next
        Return ""

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
        ' 05/07/12 | TJS             | 2012.1.08 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return CInt(Me.GetField("SELECT (SELECT COUNT(*) FROM InventoryAmazonDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryASPStorefrontDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryChannelAdvDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                                "(SELECT COUNT(*) FROM InventoryMagentoDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1)", CommandType.Text, Nothing))

    End Function
#End Region
#End Region
End Class
#End Region
