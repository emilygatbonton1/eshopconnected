' eShopCONNECT for Connected Business
' Module: MagentoImportFacade.vb
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

Imports Interprise.Framework.Base.Shared.Enum ' TJS 15/03/13
Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Base.Shared.StoredProcedures
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const

Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " MagentoImportFacade "
Public Class MagentoImportFacade
    Inherits Interprise.Facade.Base.BaseFacade
    Implements Interprise.Extendable.Base.Facade.IBaseInterface

#Region " Variables "
    ' start of code added TJS 08/04/11
    Private Structure MagentoAttributeValues
        Public ValueID As Integer
        Public OptionID As Integer
        Public StoreID As Integer
        Public Value As String
    End Structure
    Private m_AtributeValues() As MagentoAttributeValues
    ' end of code added TJS 08/04/11

    ' start of code added TJS 13/04/11
    Private Structure MagentoRelatedProducts
        Public SKU As String
        Public Description As String
        Public Price As Decimal ' TJS 24/02/12
        Public ISItemCode As String ' TJS 24/02/12
        Public Attribute1Value As String
        Public Attribute1ValueID As String ' TJS 10/06/12
        Public Attribute1Code As String
        Public Attribute2Value As String
        Public Attribute2ValueID As String ' TJS 10/06/12
        Public Attribute2Code As String
        Public Attribute3Value As String
        Public Attribute3ValueID As String ' TJS 10/06/12
        Public Attribute3Code As String
        Public Attribute4Value As String
        Public Attribute4ValueID As String ' TJS 10/06/12
        Public Attribute4Code As String
        Public Attribute5Value As String
        Public Attribute5ValueID As String ' TJS 10/06/12
        Public Attribute5Code As String
        Public Attribute6Value As String
        Public Attribute6ValueID As String ' TJS 10/06/12
        Public Attribute6Code As String
    End Structure
    Private m_MagentoRelatedProducts() As MagentoRelatedProducts
    Private m_MagentoOptions() As MagentoRelatedProducts ' TJS 24/02/12
    ' end of code added TJS 13/04/11

    ' start of code added TJS 24/02/12
    Private Structure MagentoOptionValues
        Public OptionAttribute As String
        Public OptionValueSKU As String
        Public OptionValueTitle As String
        Public OptionValuePrice As Decimal
        Public OptionIsRequired As Boolean ' TJS 09/08/13
    End Structure
    ' end of code added TJS 24/02/12

    ' start of code added TJS 15/11/13
    Public Structure MagentoPublishingOptions
        Public UseMappedCBCategories As Boolean
        Public AttributeSetID As Integer
        Public MagentoShortDescSource As String
        Public MagentoDescriptionSource As String
        Public MagentoWeightUnits As String
        Public MagentoPriceSource As String
        Public MagentoSpecialPriceSource As String
        Public MagentoSpecialPriceFrom As Object
        Public MagentoSpecialPriceTo As Object
        Public MagentoQtyPublishingOption As String
        Public MagentoQtyPublishingValue As Decimal
    End Structure
    ' end of code added TJS 15/11/13

    Private m_MagentoImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_MagentoImportRule As Lerryn.Facade.ImportExport.ImportExportFacade

    Private m_ProductsAlreadyImported As Integer = 0
    Private m_CreateISCategories As Boolean ' TJS 29/03/11
    Private m_ImportSellingPriceAsPricingCost As Boolean ' TJS 06/02/14
    Private m_ImportSellingPriceAsWholesale As Boolean ' TJS 06/02/14
    Private m_ImportSellingPriceAsRetail As Boolean ' TJS 06/02/14
    Private m_ImportSellingPriceAsSuggestedRetail As Boolean ' TJS 06/02/14
    Private m_ImportSpecialPriceAsWholesale As Boolean ' TJS 13/03/13
    Private m_ImportSpecialPriceAsRetail As Boolean ' TJS 13/03/13
    Private m_ImportCostAsAverage As Boolean ' TJS 21/06/13
    Private m_ImportCostAsStandard As Boolean ' TJS 21/06/13
    Private m_ImportCostAsLast As Boolean ' TJS 21/06/13
    Private m_ImportCostAsPricingCost As Boolean ' TJS 06/02/14
    Private m_ProcessLog As String ' TJS 29/03/11
    Private m_MagentoCategories As Lerryn.Facade.ImportExport.MagentoSOAPConnector.CategoryType() ' TJS 29/03/11
    Private m_AttributeSets As Lerryn.Facade.ImportExport.MagentoSOAPConnector.AttributeSetType() ' TJS 25/04/11
    Private m_ProductAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType() ' TJS 25/04/11
    Private m_LastError As String ' TJS 02/12/11
    Private m_ImportLimitReached As Boolean ' TJS 02/12/11
    Private m_ManualActionRequired As Boolean ' TJS 24/03/13
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

    Private Const MagentoSimplePlusOptNameSuffix As String = "|MG|" ' TJS 20/05/13
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return m_MagentoImportDataset
        End Get
    End Property
#End Region

#Region " CurrentBusinessRule "
    Public Overrides ReadOnly Property CurrentBusinessRule() As Interprise.Extendable.Base.Business.IBaseInterface
        Get
            Return m_MagentoImportRule
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

#Region " MagentoCategories "
    Public Property MagentoCategories() As Lerryn.Facade.ImportExport.MagentoSOAPConnector.CategoryType() ' TJS 09/04/11
        Get
            Return m_MagentoCategories ' TJS 09/04/11
        End Get
        Set(ByVal value As Lerryn.Facade.ImportExport.MagentoSOAPConnector.CategoryType()) ' TJS 09/04/11
            m_MagentoCategories = value ' TJS 09/04/11
        End Set
    End Property
#End Region

#Region " MagentoAttributeSets "
    Public Property MagentoAttributeSets() As Lerryn.Facade.ImportExport.MagentoSOAPConnector.AttributeSetType() ' TJS 25/04/11
        Get
            Return m_AttributeSets ' TJS 25/04/11
        End Get
        Set(ByVal value As Lerryn.Facade.ImportExport.MagentoSOAPConnector.AttributeSetType()) ' TJS 25/04/11
            m_AttributeSets = value ' TJS 25/04/11
        End Set
    End Property
#End Region

#Region " ProductAttributes "
    Public Property ProductAttributes() As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType() ' TJS 25/04/11
        Get
            Return m_ProductAttributes ' TJS 25/04/11
        End Get
        Set(ByVal value As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType()) ' TJS 25/04/11
            m_ProductAttributes = value ' TJS 25/04/11
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

#Region " ImportSellingPriceAsSuggestedRetail "
    Public Property ImportSellingPriceAsSuggestedRetail() As Boolean ' TJS 06/02/14
        Get
            Return m_ImportSellingPriceAsSuggestedRetail ' TJS 06/02/14
        End Get
        Set(ByVal value As Boolean) ' TJS 06/02/14
            m_ImportSellingPriceAsSuggestedRetail = value ' TJS 06/02/14
        End Set
    End Property
#End Region

#Region " ImportSellingPriceAsRetail "
    Public Property ImportSellingPriceAsRetail() As Boolean ' TJS 06/02/14
        Get
            Return m_ImportSellingPriceAsRetail ' TJS 06/02/14
        End Get
        Set(ByVal value As Boolean) ' TJS 06/02/14
            m_ImportSellingPriceAsRetail = value ' TJS 06/02/14
        End Set
    End Property
#End Region

#Region " ImportSellingPriceAsWholesale "
    Public Property ImportSellingPriceAsWholesale() As Boolean ' TJS 06/02/14
        Get
            Return m_ImportSellingPriceAsWholesale ' TJS 06/02/14
        End Get
        Set(ByVal value As Boolean) ' TJS 06/02/14
            m_ImportSellingPriceAsWholesale = value ' TJS 06/02/14
        End Set
    End Property
#End Region

#Region " ImportSellingPriceAsPricingCost "
    Public Property ImportSellingPriceAsPricingCost() As Boolean ' TJS 06/02/14
        Get
            Return m_ImportSellingPriceAsPricingCost ' TJS 06/02/14
        End Get
        Set(ByVal value As Boolean) ' TJS 06/02/14
            m_ImportSellingPriceAsPricingCost = value ' TJS 06/02/14
        End Set
    End Property
#End Region

#Region " ImportSpecialPriceAsWholesale "
    Public Property ImportSpecialPriceAsWholesale() As Boolean ' TJS 13/03/13
        Get
            Return m_ImportSpecialPriceAsWholesale ' TJS 13/03/13
        End Get
        Set(ByVal value As Boolean) ' TJS 13/03/13
            m_ImportSpecialPriceAsWholesale = value ' TJS 13/03/13
        End Set
    End Property
#End Region

#Region " ImportSpecialPriceAsRetail "
    Public Property ImportSpecialPriceAsRetail() As Boolean ' TJS 13/03/13
        Get
            Return m_ImportSpecialPriceAsRetail ' TJS 13/03/13
        End Get
        Set(ByVal value As Boolean) ' TJS 13/03/13
            m_ImportSpecialPriceAsRetail = value ' TJS 13/03/13
        End Set
    End Property
#End Region

#Region " ImportCostAsStandard "
    Public Property ImportCostAsStandard() As Boolean ' TJS 21/06/13
        Get
            Return m_ImportCostAsStandard ' TJS 21/06/13
        End Get
        Set(ByVal value As Boolean) ' TJS 21/06/13
            m_ImportCostAsStandard = value ' TJS 21/06/13
        End Set
    End Property
#End Region

#Region " ImportCostAsAverage "
    Public Property ImportCostAsAverage() As Boolean ' TJS 21/06/13
        Get
            Return m_ImportCostAsAverage ' TJS 21/06/13
        End Get
        Set(ByVal value As Boolean) ' TJS 21/06/13
            m_ImportCostAsAverage = value ' TJS 21/06/13
        End Set
    End Property
#End Region

#Region " ImportCostAsLast"
    Public Property ImportCostAsLast() As Boolean ' TJS 21/06/13
        Get
            Return m_ImportCostAsLast ' TJS 21/06/13
        End Get
        Set(ByVal value As Boolean) ' TJS 21/06/13
            m_ImportCostAsLast = value ' TJS 21/06/13
        End Set
    End Property
#End Region

#Region " ImportCostAsPricingCost "
    Public Property ImportCostAsPricingCost() As Boolean ' TJS 06/02/14
        Get
            Return m_ImportCostAsPricingCost ' TJS 06/02/14
        End Get
        Set(ByVal value As Boolean) ' TJS 06/02/14
            m_ImportCostAsPricingCost = value ' TJS 06/02/14
        End Set
    End Property
#End Region

#Region " ProcessLog "
    Public ReadOnly Property ProcessLog() As String ' TJS 29/03/11
        Get
            Return m_ProcessLog ' TJS 29/03/11
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

#Region " LastErrorMessage "
    ' Last error message (e.g. from web server)
    Private m_LastErrorMessage As String ' TJS 14/02/12
    Public ReadOnly Property LastErrorMessage() As String ' TJS 14/02/12
        Get
            Return m_LastErrorMessage ' TJS 14/02/12
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

#Region " ManualActionRequired "
    Public ReadOnly Property ManualActionRequired() As Boolean ' TJS 24/03/13
        Get
            Return m_ManualActionRequired ' TJS 24/03/13
        End Get
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New(ByVal p_MagentoImportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
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
        m_MagentoImportDataset = p_MagentoImportDataset
        m_MagentoImportRule = New Lerryn.Facade.ImportExport.ImportExportFacade(p_MagentoImportDataset, p_ErrorNotification, p_BaseProductCode, p_BaseProductName) ' TJS 10/06/12
        MyBase.InitializeDataset()

        ' read all licences as we need base licence and all add-ons
        Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynLicences_DEV000221.TableName, _
            "ReadLerrynLicences_DEV000221"}, New String() {m_MagentoImportDataset.System_DEV000221.TableName, _
            "ReadSystem_DEV000221"}, New String() {m_MagentoImportDataset.SystemCompanyInformation.TableName, _
            "ReadSystemCompanyInformation"}, New String() {m_MagentoImportDataset.LerrynImportExportConfig_DEV000221.TableName, _
            "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        m_MagentoImportRule.CheckActivation()
        ReDim m_MagentoCategories(0) ' TJS 02/12/11
        ' when debugging, need to pause here and execute following in the immediate window to prevent cross-thread errors
        'DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true

    End Sub
#End Region

#Region " GetMagentoProductList "
    Public Function GetMagentoProductList(ByVal MagentoInstanceID As String, ByRef Cancel As Boolean, ByRef ItemsForImport As DataTable) As Boolean
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
        ' 08/04/11 | TJS             | 2011.0.09 | Modified to initialise SKUError column
        ' 28/09/11 | FA              | 2011.1.05 | Performance improvement for checking items already imported
        ' 04/11/11 | TJS             | 2011.1.10 | Mod1fied to cater for products where SKU is changed in Magento
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | Also replaced calls to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages
        ' 16/01/12 | TJS             | 2011.2.01 | Modified to get magento list in batchs of 1000 ids
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to cater for Magento items with options
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for non-stock items
        ' 18/03/13 | TJS             | 2013.1.04 | Corrected detection of SKUExists condition
        ' 29/03/13 | TJS             | 2013.1.07 | Modified to check options on potential simple+opt types for Split SKU settings
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to cater for Magento login potentially containing XML entities
        ' 30/04/13 | TJS  *           | 2013.1.11 | Modified to cater for simple+opt types having several Matrix ITems with same product code as MAtrix Group
        ' 23/05/13 | TJS             | 2013.1.16 | Corrected detection of options starting with split SKU chars
        ' 29/05/13 | FA/TJS          | 2013.1.18 | Modified to cater for imports from multiple instances
        '                                          Modified to check for SKU existing and that the MagentoID is the same
        ' 01/06/13 | TJS             | 2013.1.19 | Modified to perform checks for duplicates and SKU errors after all properties have been read
        '                                        | and added error trap
        ' 16/06/13 | TJS             | 2013.1.20 | Replaced strImportedProducts with view in dataset and rowImportedProducts
        ' 09/08/13 | TJS             | 2013.2.02 | Corrected setting of AlreadyImported when SKUChanged is set
        ' 30/09/13 | TJS             | 2013.3.02 | Modified to decode Config XML Value such as password etc 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2SoapAPIWSICompliant
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple magento sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLTemp2 As XDocument
        Dim XMLOptionTemp As XDocument, XMLOptionItemTemp As XDocument ' TJS 29/03/13
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement
        Dim XMLPropertyList As System.Collections.Generic.IEnumerable(Of XElement), XMLPropertyNode As XElement
        Dim XMLOptionsList As System.Collections.Generic.IEnumerable(Of XElement), XMLOptionNode As XElement ' TJS 29/03/13
        Dim XMLOptionItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLOptionItemNode As XElement ' TJS 29/03/13 TJS 01/06/13
        Dim colNewColumn As DataColumn, rowImportItems As System.Data.DataRow
        Dim rowImportedProduct As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryItemMagentoSummaryView_DEV000221Row ' TJS 16/06/13
        Dim iLoop As Integer, bInstanceMatched As Boolean, bReturnValue As Boolean, bMissingProductID As Boolean ' TJS 31/03/11 TJS 16/01/12
        Dim strItemKey As String, strItemValue As String, strTemp As String, strSplitItemSKU As String 'FA 28/09/11 TJS 29/03/13
        Dim iMaxProductID As Integer, iBatchSize As Integer, iBatchLoop As Integer ' TJS 09/03/13
        Dim bHasCustomOptions As Boolean, bSKUFound As Boolean, bMagentoIDFound As Boolean ' TJS 01/06/13
        Dim bMagentoV2APIWSI As Boolean ' TJS 05/0/13

        Try ' TJS 01/06/13
            bReturnValue = False
            m_LastErrorMessage = "" ' TJS 14/02/12
            m_LastError = "" ' TJS 02/12/11
            If m_MagentoImportRule.IsActivated Then ' TJS 29/03/11
                XMLConfig = XDocument.Parse(Trim(m_MagentoImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                bInstanceMatched = False ' TJS 01/06/13
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If m_MagentoImportRule.GetXMLElementListCount(XMLNodeList) = 1 Then
                        bInstanceMatched = True
                    ElseIf m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoInstanceID Then
                        bInstanceMatched = True
                    End If
                    If bInstanceMatched Then
                        If m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL) <> "" And _
                            m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER) <> "" Then
                            strSplitItemSKU = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_SPLIT_SKU_SEPARATOR_CHARACTERS) ' TJS 29/03/13
                            bMagentoV2APIWSI = CBool(IIf(UCase(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False)) ' TJS 13/11/13
                            objMagento = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
                            objMagento.V2SoapAPIWSICompliant = bMagentoV2APIWSI ' TJS 13/11/13
                            objMagento.MagentoVersion = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION) ' TJS 13/11/13
                            strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION) ' TJS 13/11/13
                            If strTemp <> "" Then
                                objMagento.LerrynAPIVersion = CDec(strTemp) ' TJS 13/11/13
                            Else
                                objMagento.LerrynAPIVersion = 0 ' TJS 13/11/13
                            End If
                            If objMagento.Login(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                                m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                                m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)))) Then ' TJS 03/04/13 TJS 30/09/13
                                iMaxProductID = objMagento.GetCatalogProductListMaxID ' TJS 16/01/12
                                If iMaxProductID >= 0 Then ' TJS 16/01/12

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
                                    colNewColumn.Caption = "Magento Type"
                                    colNewColumn.ColumnName = "SourceType"
                                    colNewColumn.DataType = System.Type.GetType("System.String")
                                    ItemsForImport.Columns.Add(colNewColumn)
                                    colNewColumn.Dispose()

                                    colNewColumn = New DataColumn
                                    colNewColumn.Caption = "Magento Item ID"
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

                                    Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.InventoryItemMagentoSummaryView_DEV000221.TableName, _
                                        "ReadInventoryItemMagentoSummaryView_DEV000221", "@SourceIsGroupItem", "0"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 16/06/13
                                    m_ProductsAlreadyImported = 0

                                    XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                                    XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                                    XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                                    XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11

                                    iBatchSize = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_PRODUCT_LIST_BLOCK_SIZE)) ' TJS 14/02/12
                                    For iBatchLoop = 1 To iMaxProductID Step iBatchSize ' TJS 16/01/12 TJS 14/02/12
                                        If objMagento.GetCatalogProductListBatch(iBatchLoop, iBatchSize, False) Then ' TJS 16/01/12 TJS 14/02/12
                                            XMLItemList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)

                                            For Each XMLItemNode In XMLItemList
                                                XMLTemp = XDocument.Parse(XMLItemNode.ToString)
                                                bMissingProductID = False ' TJS 16/01/12
                                                rowImportItems = ItemsForImport.NewRow
                                                rowImportItems.Item("Import") = True
                                                rowImportItems.Item("AlreadyImported") = False
                                                rowImportItems.Item("SKUExists") = False
                                                rowImportItems.Item("SKUError") = False ' TJS 08/04/11
                                                rowImportItems.Item("SKUChanged") = False ' TJS 04/11/11
                                                rowImportItems.Item("SourceIDChanged") = False ' TJS 09/08/13

                                                XMLPropertyList = XMLTemp.XPathSelectElements("item/item") ' TJS 02/12/11 TJS 14/02/12
                                                strItemKey = "" ' TJS 09/03/13
                                                strItemValue = "" ' TJS 09/03/13
                                                bHasCustomOptions = False ' TJS 01/06/13
                                                For Each XMLPropertyNode In XMLPropertyList
                                                    XMLTemp2 = XDocument.Parse(XMLPropertyNode.ToString)
                                                    strItemKey = m_MagentoImportRule.GetXMLElementText(XMLTemp2, "item/key") ' FA 28/09/11
                                                    strItemValue = m_MagentoImportRule.GetXMLElementText(XMLTemp2, "item/value") ' FA 28/09/11
                                                    Select Case strItemKey ' FA 28/09/11
                                                        Case "product_id"
                                                            If strItemValue = "Does Not Exist" Then ' TJS 16/01/12
                                                                bMissingProductID = True ' TJS 16/01/12
                                                                Exit For ' TJS 16/01/12
                                                            End If
                                                            rowImportItems.Item("SourceItemID") = strItemValue ' FA 28/09/11

                                                        Case "sku"
                                                            If strItemValue <> "" Then ' TJS 01/06/13
                                                                rowImportItems.Item("ItemSKU") = strItemValue ' FA 28/09/11 TJS 04/11/11
                                                            End If

                                                        Case "name"
                                                            rowImportItems.Item("ItemName") = strItemValue ' FA 28/09/11

                                                        Case "type"
                                                            rowImportItems.Item("SourceType") = strItemValue ' FA 28/09/11                                                 

                                                            ' start of code added TJS 24/02/12
                                                        Case "ldt_has_custom_options"
                                                            If strItemValue <> "no" Then
                                                                bHasCustomOptions = True ' TJS 01/06/13
                                                            End If
                                                            ' end of code added TJS 24/02/12

                                                    End Select
                                                Next

                                                If Not bMissingProductID Then ' TJS 16/01/12
                                                    ItemsForImport.Rows.Add(rowImportItems)

                                                    ' start of code moved TJS 01/06/13
                                                    Select Case rowImportItems.Item("SourceType").ToString
                                                        Case "configurable"
                                                            rowImportItems.Item("ISItemType") = "Matrix Group"
                                                        Case "grouped"
                                                            rowImportItems.Item("ISItemType") = "Kit"
                                                        Case "virtual"
                                                            rowImportItems.Item("ISItemType") = "Non-Stock"
                                                        Case Else
                                                            rowImportItems.Item("ISItemType") = "Stock"
                                                    End Select
                                                    If bHasCustomOptions Then
                                                        ' start of code added TJS 29/03/13
                                                        ' get custom options from Magento
                                                        If objMagento.GetCatalogProductCustomOptions(rowImportItems.Item("SourceItemID").ToString) Then
                                                            XMLOptionsList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                                                            For Each XMLOptionNode In XMLOptionsList
                                                                XMLOptionTemp = XDocument.Parse(XMLOptionNode.ToString)
                                                                XMLOptionItemList = XMLOptionTemp.XPathSelectElements("item/item") ' TJS 23/05/13' TJS 01/06/13
                                                                For Each XMLOptionItemNode In XMLOptionItemList ' TJS 01/06/13
                                                                    XMLOptionItemTemp = XDocument.Parse(XMLOptionItemNode.ToString)
                                                                    strTemp = m_MagentoImportRule.GetXMLElementText(XMLOptionItemTemp, "item/value")
                                                                    Select Case m_MagentoImportRule.GetXMLElementText(XMLOptionItemTemp, "item/key")
                                                                        Case "sku"
                                                                            ' does sku addition for option start with Split SKU characters ?
                                                                            If strSplitItemSKU = "" OrElse strTemp.Length <= strSplitItemSKU.Length OrElse _
                                                                                strTemp.Substring(0, strSplitItemSKU.Length) <> strSplitItemSKU Then
                                                                                ' no, need to create matrix item on import
                                                                                If rowImportItems.Item("ISItemType").ToString <> "Matrix Group" Then
                                                                                    rowImportItems.Item("SourceType") = rowImportItems.Item("SourceType").ToString & "+opt"
                                                                                    rowImportItems.Item("ISItemType") = "Matrix Group"
                                                                                End If
                                                                            End If
                                                                    End Select
                                                                Next
                                                            Next
                                                        Else
                                                            ' end of code added TJS 29/03/13           
                                                            rowImportItems.Item("SourceType") = rowImportItems.Item("SourceType").ToString & "+opt"
                                                            rowImportItems.Item("ISItemType") = "Matrix Group"
                                                        End If
                                                    End If

                                                    ' do we have a product SKU ?
                                                    If rowImportItems.Item("ItemSKU").ToString.Trim = "" Then ' TJS 18/03/13 TJS 01/06/13
                                                        ' no, check for inventory item with same Magento Product ID and Instance ID
                                                        For Each rowImportedProduct In m_MagentoImportDataset.InventoryItemMagentoSummaryView_DEV000221.Rows ' TJS 16/06/13
                                                            If Not rowImportedProduct.IsMagentoProductID_DEV000221Null AndAlso _
                                                                Not rowImportedProduct.IsInstanceID_DEV000221Null AndAlso _
                                                                rowImportedProduct.MagentoProductID_DEV000221 = rowImportItems.Item("SourceItemID").ToString AndAlso _
                                                                MagentoInstanceID = rowImportedProduct.InstanceID_DEV000221 Then ' FA 28/09/11 TJS 16/06/13
                                                                ' exisitng Magento record found, set ItemCode and SKU
                                                                rowImportItems.Item("Import") = False
                                                                rowImportItems.Item("AlreadyImported") = True
                                                                rowImportItems.Item("ItemCode") = rowImportedProduct.ItemCode ' TJS 04/11/11 TJS 16/06/13
                                                                rowImportItems.Item("ItemSKU") = rowImportedProduct.ItemName ' TJS 18/03/13 TJS 16/06/13
                                                                Exit For
                                                            End If
                                                        Next
                                                        ' did we find item and set product SKU ?
                                                        If rowImportItems.Item("ItemSKU").ToString.Trim = "" Then ' TJS 01/06/13
                                                            ' no
                                                            rowImportItems.Item("Import") = False
                                                            rowImportItems.Item("SKUError") = True
                                                            rowImportItems.SetColumnError("ItemSKU", "Cannot import blank SKU")
                                                        End If
                                                    Else
                                                        ' yes, check for any other instances of the SKU
                                                        bSKUFound = False ' TJS 01/06/13
                                                        bMagentoIDFound = False ' TJS 01/06/13
                                                        For Each rowImportedProduct In m_MagentoImportDataset.InventoryItemMagentoSummaryView_DEV000221.Rows ' TJS 16/06/13
                                                            ' has sku been imported before (Ignore case differences and also cater for Simple Plus Option Matrix Suffix) ?
                                                            If (Not rowImportedProduct.IsItemNameNull And Not rowImportedProduct.IsItemTypeNull) AndAlso _
                                                                (rowImportedProduct.ItemName.ToUpper = rowImportItems.Item("ItemSKU").ToString.ToUpper OrElse _
                                                                (rowImportedProduct.ItemType = ITEM_TYPE_MATRIX_GROUP And _
                                                                rowImportedProduct.ItemName.ToUpper = rowImportItems.Item("ItemSKU").ToString.ToUpper & MagentoSimplePlusOptNameSuffix)) Then ' FA 28/09/11 TJS 04/11/11 TJS 18/03/13 TJS 01/06/13 TJS 16/06/13
                                                                bSKUFound = True ' TJS 01/06/13
                                                                ' have we found a match for the Magento ID (it will be on another item if it was)
                                                                If bMagentoIDFound Then ' TJS 01/06/13
                                                                    rowImportItems.Item("Import") = False
                                                                    rowImportItems.Item("SKUError") = True
                                                                    rowImportItems.SetColumnError("ItemSKU", "Magento item already imported with different SKU and new SKU also exists")
                                                                    Exit For ' TJS/FA 29/05/13
                                                                End If
                                                                ' yes, SKU exists, does it have a Magento ID for this Magento Instance?
                                                                If rowImportedProduct.IsMagentoProductID_DEV000221Null Or (Not rowImportedProduct.IsInstanceID_DEV000221Null AndAlso _
                                                                    MagentoInstanceID <> rowImportedProduct.InstanceID_DEV000221) Then ' TJS 04/11/11 TJS 18/03/13 TJS/FA 29/05/13 TJS 16/06/13
                                                                    ' no - Instance ID must also be empty or not for this Instance
                                                                    rowImportItems.Item("SKUExists") = True
                                                                    rowImportItems.Item("ItemCode") = rowImportedProduct.ItemCode ' TJS 04/11/11 TJS 16/06/13

                                                                ElseIf Not rowImportedProduct.IsMagentoProductID_DEV000221Null AndAlso _
                                                                    Not rowImportedProduct.IsInstanceID_DEV000221Null AndAlso _
                                                                    rowImportItems.Item("SourceItemID").ToString = rowImportedProduct.MagentoProductID_DEV000221 And _
                                                                    MagentoInstanceID = rowImportedProduct.InstanceID_DEV000221 Then ' TJS/FA 29/05/13 TJS 16/06/13
                                                                    ' yes and it is the same
                                                                    rowImportItems.Item("Import") = False ' TJS/FA 29/05/13
                                                                    rowImportItems.Item("AlreadyImported") = True ' TJS/FA 29/05/13
                                                                    rowImportItems.Item("ItemCode") = rowImportedProduct.ItemCode ' TJS/FA 29/05/13 TJS 16/06/13

                                                                ElseIf Not rowImportedProduct.IsMagentoProductID_DEV000221Null AndAlso _
                                                                    Not rowImportedProduct.IsInstanceID_DEV000221Null AndAlso _
                                                                    rowImportItems.Item("SourceItemID").ToString <> rowImportedProduct.MagentoProductID_DEV000221 And _
                                                                    MagentoInstanceID = rowImportedProduct.InstanceID_DEV000221 Then ' TJS 04/11/11 TJS/FA 29/05/13 TJS 16/06/13
                                                                    ' yes and it is different
                                                                    rowImportItems.Item("Import") = False ' TJS 04/11/11
                                                                    rowImportItems.Item("SourceIDChanged") = True ' TJS 04/11/11 TJS 09/08/13
                                                                    rowImportItems.Item("AlreadyImported") = True ' TJS 09/08/13
                                                                    rowImportItems.Item("ItemCode") = rowImportedProduct.ItemCode ' TJS 04/11/11 TJS 16/06/13
                                                                End If

                                                            ElseIf Not rowImportedProduct.IsMagentoProductID_DEV000221Null AndAlso _
                                                                Not rowImportedProduct.IsInstanceID_DEV000221Null AndAlso _
                                                                rowImportItems.Item("SourceItemID").ToString = rowImportedProduct.MagentoProductID_DEV000221 And _
                                                                MagentoInstanceID = rowImportedProduct.InstanceID_DEV000221 Then ' TJS 01/06/13 TJS 16/06/13
                                                                bMagentoIDFound = True ' TJS 01/06/13
                                                                ' have we found a match for the SKU (it will be on another item if it was)
                                                                If bSKUFound Then ' TJS 01/06/13
                                                                    rowImportItems.Item("Import") = False
                                                                    rowImportItems.Item("SKUError") = True
                                                                    rowImportItems.SetColumnError("ItemSKU", "Magento item already imported with different SKU (" & rowImportedProduct.ItemName & ") and new SKU also exists") ' TJS 01/06/13 TJS 16/06/13
                                                                    Exit For ' TJS/FA 29/05/13
                                                                End If
                                                                ' start of code added TJS 09/08/13
                                                                ' has SKU changed ?
                                                                If rowImportedProduct.ItemName.ToUpper <> rowImportItems.Item("ItemSKU").ToString.ToUpper AndAlso _
                                                                    (rowImportedProduct.ItemType <> ITEM_TYPE_MATRIX_GROUP Or _
                                                                    rowImportedProduct.ItemName.ToUpper <> rowImportItems.Item("ItemSKU").ToString.ToUpper & MagentoSimplePlusOptNameSuffix) Then
                                                                    ' yes
                                                                    rowImportItems.Item("Import") = False
                                                                    rowImportItems.Item("SKUChanged") = True
                                                                    rowImportItems.Item("AlreadyImported") = True
                                                                    rowImportItems.Item("ItemCode") = rowImportedProduct.ItemCode
                                                                End If
                                                                ' end of code added TJS 09/08/13
                                                            End If
                                                        Next
                                                    End If
                                                    ' do we have now a product SKU ?
                                                    If rowImportItems.Item("ItemSKU").ToString.Trim <> "" Then ' TJS 18/03/13 TJS 01/06/13
                                                        ' end of code moved TJS 01/06/13
                                                        ' now check for duplicates - ignore last row
                                                        For iLoop = 0 To ItemsForImport.Rows.Count - 2
                                                            If ItemsForImport.Rows(iLoop).Item("ItemSKU").ToString.Trim.ToLower = rowImportItems.Item("ItemSKU").ToString.Trim.ToLower Then ' TJS 04/11/11
                                                                If ItemsForImport.Rows(iLoop).Item("SourceItemID").ToString.Trim.ToLower = rowImportItems.Item("SourceItemID").ToString.Trim.ToLower Then ' TJS 16/01/12
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
                                                End If
                                            Next
                                        Else
                                            m_LastError = "Unable to read Magento Product List starting at Product ID " & iBatchLoop & " - " & objMagento.LastError ' TJS 16/01/12
                                            m_LastErrorMessage = "Unable to read Magento Product List starting at Product ID " & iBatchLoop & " - " & objMagento.LastErrorMessage ' TJS 14/02/12
                                        End If
                                    Next
                                    m_ProductsAlreadyImported = GetInventoryItemsImportedCount() ' TJS 29/03/11
                                    If ItemsForImport.Rows.Count > 0 Then
                                        bReturnValue = True
                                    End If
                                Else
                                    m_LastError = "Unable to read maximum Magento Product ID - " & objMagento.LastError ' TJS 02/12/11 TJS 16/01/12
                                    m_LastErrorMessage = "Unable to read maximum Magento Product ID - " & objMagento.LastErrorMessage ' TJS 14/02/12
                                End If

                                objMagento.Logout()
                            Else
                                m_LastError = "Unable to connect to Magento - " & objMagento.LastError & vbCrLf & vbCrLf & "Please check your configuration settings." ' TJS 02/12/11
                                m_LastErrorMessage = "Unable to connect to Magento - " & objMagento.LastErrorMessage & vbCrLf & vbCrLf & "Please check your configuration settings." ' TJS 14/02/12
                            End If
                            Exit For
                        Else
                            m_LastError = "Please enter your Magento API connection settings in the eShopCONNECT config." ' TJS 02/12/11
                            m_LastErrorMessage = "Please enter your Magento API connection settings in the eShopCONNECT config." ' TJS 14/02/12
                        End If
                        Exit For ' TJS 01/04/14
                    End If
                    If Cancel Then
                        Return False
                    End If
                Next
                If Not bInstanceMatched Then ' TJS/FA 01/06/13
                    m_LastError = "Magento Instance not found." ' TJS/FA 01/06/13
                    m_LastErrorMessage = m_LastError ' TJS/FA 01/06/13
                    m_ProcessLog = m_ProcessLog & "Cannot connect to Magento" & vbCrLf ' TJS/FA 01/06/13
                End If
            End If
            Return bReturnValue

        Catch ex As Exception ' TJS 03/04/13
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace ' TJS 03/04/13
            m_LastErrorMessage = ex.Message ' TJS 03/04/13
            m_ProcessLog = m_ProcessLog & ex.Message ' TJS 03/04/13
            Return False ' TJS 03/04/13

        End Try

    End Function
#End Region

#Region " GetMagentoCategories "
    Public Function GetMagentoCategories(ByVal InstanceID As String, ByRef Cancel As Boolean, ByRef CategoryTable As System.Data.DataTable) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/04/11 | TJS             | 2011.0.10 | Function added
        ' 13/04/11 | TJS             | 2011.0.10 | Modified to use local MagentoCategoryCount variable
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to cater for LerrynImportExportConfig_DEV000221 containing multiple rows
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | Also replaced call to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages and modified to return true/false
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to cater for IsActive being false on a Category record
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to cater for Magento login potentially containing XML entities
        ' 30/09/13 | TJS             | 2013.3.02 | Modified to decode Config XML Value such as password etc 
        '                                        | and to apply any previously saved category mapping details
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API and added error handler
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row ' TJS 25/04/11
        Dim rowCategoryMapping As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportCategoryMappingView_DEV000221Row ' TJS 30/09/13
        Dim objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLCategoryList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim rowCategory As System.Data.DataRow
        Dim strTemp As String ' TJS 13/11/13
        Dim iLoop As Integer, iNodeLoop As Integer, MagentoCategoryCount As Integer ' TJS 13/04/11
        Dim bReturnValue As Boolean, bMagentoV2APIWSI As Boolean ' TJS 02/12/11 TJS 05/0/13

        Try ' TJS 13/11/13
            bReturnValue = True ' TJS 02/12/11
            MagentoCategoryCount = 0 ' TJS 13/04/11
            m_LastErrorMessage = "" ' TJS 14/02/12
            m_LastError = "" ' TJS 02/12/11
            ReDim m_MagentoCategories(MagentoCategoryCount) ' TJS 13/04/11
            rowMagentoConfig = Me.m_MagentoImportDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(MAGENTO_SOURCE_CODE) ' TJS 25/04/11
            XMLConfig = XDocument.Parse(Trim(rowMagentoConfig.ConfigSettings_DEV000221)) ' TJS 25/04/11
            XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
            For Each XMLNode In XMLNodeList
                XMLTemp = XDocument.Parse(XMLNode.ToString)
                If m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = InstanceID Then
                    bMagentoV2APIWSI = CBool(IIf(UCase(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False)) ' TJS 13/11/13
                    objMagento = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
                    objMagento.V2SoapAPIWSICompliant = bMagentoV2APIWSI ' TJS 13/11/13
                    objMagento.MagentoVersion = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION) ' TJS 13/11/13
                    strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION) ' TJS 13/11/13
                    If strTemp <> "" Then
                        objMagento.LerrynAPIVersion = CDec(strTemp) ' TJS 13/11/13
                    Else
                        objMagento.LerrynAPIVersion = 0 ' TJS 13/11/13
                    End If
                    If objMagento.Login(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                        m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                        m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)))) Then ' TJS 03/04/13 TJS 30/09/13
                        If objMagento.GetCatalogTree() Then
                            XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                            XMLCategoryList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento)
                            objMagento.ExtractMagentoCategories(XMLCategoryList, m_MagentoCategories, MagentoCategoryCount, Cancel) ' TJS 13/04/11
                            ' if we had any non-active categories, then we need to remove empty records
                            If MagentoCategoryCount > 0 Then ' TJS 13/04/11
                                ReDim Preserve m_MagentoCategories(MagentoCategoryCount - 1) ' TJS 13/04/11
                                Me.LoadDataSet(New String()() {New String() {Me.m_MagentoImportDataset.LerrynImportExportCategoryMappingView_DEV000221.TableName, _
                                    "ReadLerrynImportExportCategoryMapping_DEV000221", AT_SOURCE_CODE, "MagentoOrder", AT_INSTANCE_ID, InstanceID}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 30/09/13 TJS 13/11/13

                                For iNodeLoop = 0 To MagentoCategoryCount - 1 ' TJS 13/04/11
                                    rowCategory = CategoryTable.NewRow
                                    rowCategory.Item("Active") = False
                                    For iLoop = 0 To Me.m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Count - 1
                                        If m_MagentoCategories(iNodeLoop).CategoryID = Me.m_MagentoImportDataset.InventoryMagentoCategories_DEV000221(iLoop).MagentoCategoryID_DEV000221 Then
                                            rowCategory.Item("Active") = Me.m_MagentoImportDataset.InventoryMagentoCategories_DEV000221(iLoop).IsActive_DEV000221 ' TJS 24/02/12
                                            Exit For
                                        End If
                                    Next
                                    rowCategory.Item("SourceCategoryName") = m_MagentoCategories(iNodeLoop).CategoryName
                                    rowCategory.Item("SourceCategoryID") = m_MagentoCategories(iNodeLoop).CategoryID
                                    rowCategory.Item("SourceParentID") = m_MagentoCategories(iNodeLoop).ParentID
                                    rowCategoryMapping = Me.m_MagentoImportDataset.LerrynImportExportCategoryMappingView_DEV000221.FindBySourceCode_DEV000221InstanceAccountID_DEV000221SourceCategoryID_DEV000221SourceParentID_DEV000221("MagentoOrder", InstanceID, rowCategory.Item("SourceCategoryID").ToString, rowCategory.Item("SourceParentID").ToString) ' TJS 30/09/13 TJS 13/11/13
                                    If rowCategoryMapping IsNot Nothing Then ' TJS 30/09/13
                                        rowCategory.Item("ISCategoryCode") = rowCategoryMapping.ISCategoryCode_DEV000221 ' TJS 30/09/13
                                        rowCategory.Item("ISCategoryName") = rowCategoryMapping.Description ' TJS 13/11/13
                                    End If
                                    CategoryTable.Rows.Add(rowCategory)
                                    If Cancel Then
                                        bReturnValue = False ' TJS 02/12/11
                                        Exit For
                                    End If
                                Next

                            Else
                                ' didn't find any active categories
                                ReDim m_MagentoCategories(0)
                            End If
                        End If
                    Else
                        m_LastError = "Unable to connect to Magento - " & objMagento.LastError & vbCrLf & vbCrLf & "Please check your configuration settings." ' TJS 02/12/11
                        m_LastErrorMessage = "Unable to connect to Magento - " & objMagento.LastErrorMessage ' TJS 14/02/12
                        bReturnValue = False ' TJS 02/12/11
                    End If
                    Exit For
                End If
            Next
            Return bReturnValue ' TJS 02/12/11

            ' start of code added TJS 13/11/13
        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = ex.Message
            m_ProcessLog = m_ProcessLog & ex.Message
            Return False
            ' end of code added TJS 13/11/13
        End Try

    End Function
#End Region

#Region " GetMagentoAttributeSets "
    Public Function GetMagentoAttributeSets(ByVal InstanceID As String, ByRef Cancel As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to cater for Magento login potentially containing XML entities
        ' 30/09/13 | TJS             | 2013.3.00 | Modified to decode Config XML Value such as password etc 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2SoapAPIWSICompliant and modified to return true/false
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row
        Dim objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement, XMLSetNode As XElement, XMLItemNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLSetList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strTemp As String, iSetPtr As Integer, bMagentoV2APIWSI As Boolean ' TJS 05/0/13

        GetMagentoAttributeSets = True
        Try
            rowMagentoConfig = Me.m_MagentoImportDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(MAGENTO_SOURCE_CODE)
            XMLConfig = XDocument.Parse(Trim(rowMagentoConfig.ConfigSettings_DEV000221))
            XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
            For Each XMLNode In XMLNodeList
                XMLTemp = XDocument.Parse(XMLNode.ToString)
                If m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = InstanceID Then
                    bMagentoV2APIWSI = CBool(IIf(UCase(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False)) ' TJS 13/11/13
                    objMagento = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
                    objMagento.V2SoapAPIWSICompliant = bMagentoV2APIWSI ' TJS 13/11/13
                    objMagento.MagentoVersion = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION) ' TJS 13/11/13
                    strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION) ' TJS 13/11/13
                    If strTemp <> "" Then
                        objMagento.LerrynAPIVersion = CDec(strTemp) ' TJS 13/11/13
                    Else
                        objMagento.LerrynAPIVersion = 0 ' TJS 13/11/13
                    End If
                    If objMagento.Login(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                        m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                        m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)))) Then ' TJS 03/04/13 TJS 30/09/13
                        If objMagento.GetProductAttributeSetList() Then
                            ' Get the list of attributes
                            XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                            XMLSetList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)

                            ReDim m_AttributeSets(m_MagentoImportRule.GetXMLElementListCount(XMLSetList) - 1)
                            iSetPtr = 0
                            For Each XMLSetNode In XMLSetList
                                XMLTemp = XDocument.Parse(XMLSetNode.ToString)
                                XMLItemList = XMLTemp.XPathSelectElements("item/item")
                                For Each XMLItemNode In XMLItemList
                                    Select Case XMLItemNode.XPathSelectElement("key").Value.ToLower
                                        Case "set_id"
                                            m_AttributeSets(iSetPtr).SetID = CInt(XMLItemNode.XPathSelectElement("value").Value)

                                        Case "name"
                                            m_AttributeSets(iSetPtr).SetName = XMLItemNode.XPathSelectElement("value").Value
                                    End Select
                                Next
                                iSetPtr += 1
                            Next
                        End If
                    Else
                        m_LastError = "Unable to connect to Magento - " & objMagento.LastError & vbCrLf & vbCrLf & "Please check your configuration settings." ' TJS 13/11/13
                        m_LastErrorMessage = "Unable to connect to Magento - " & objMagento.LastErrorMessage ' TJS 13/11/13
                        GetMagentoAttributeSets = False
                    End If
                    Exit For
                End If
            Next

        Catch ex As Exception
            m_LastError = ex.Message & vbCrLf & vbCrLf & ex.StackTrace ' TJS 13/11/13
            m_LastErrorMessage = ex.Message ' TJS 13/11/13
            GetMagentoAttributeSets = False

        End Try

    End Function
#End Region

#Region " GetMagentoAttributesForSet "
    Public Sub GetMagentoAttributesForSet(ByVal InstanceID As String, ByRef Cancel As Boolean, ByVal AttributeSetID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.05 | Moved extraction code to ExtractMagentoAttributes so it can also be used by 
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to cater for Magento login potentially containing XML entities
        ' 30/09/13 | TJS             | 2013.3.00 | Modified to decode Config XML Value such as password etc 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2Soap API
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row
        Dim objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strTemp As String, bMagentoV2APIWSI As Boolean ' TJS 05/0/13 TJS 13/11/13

        rowMagentoConfig = Me.m_MagentoImportDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(MAGENTO_SOURCE_CODE)
        XMLConfig = XDocument.Parse(Trim(rowMagentoConfig.ConfigSettings_DEV000221))
        XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
        For Each XMLNode In XMLNodeList
            XMLTemp = XDocument.Parse(XMLNode.ToString)
            If m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = InstanceID Then
                bMagentoV2APIWSI = CBool(IIf(UCase(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False)) ' TJS 13/11/13
                objMagento = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
                objMagento.V2SoapAPIWSICompliant = bMagentoV2APIWSI ' TJS 13/11/13
                objMagento.MagentoVersion = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION) ' TJS 13/11/13
                strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION) ' TJS 13/11/13
                If strTemp <> "" Then
                    objMagento.LerrynAPIVersion = CDec(strTemp) ' TJS 13/11/13
                Else
                    objMagento.LerrynAPIVersion = 0 ' TJS 13/11/13
                End If
                If objMagento.Login(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                    m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                    m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)))) Then ' TJS 03/04/13 TJS 30/09/13
                    If objMagento.GetProductAttributeList(AttributeSetID.ToString) Then
                        ExtractMagentoAttributes(objMagento.ReturnedXML, m_ProductAttributes) ' TJS 10/06/12
                    End If
                Else
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Unable to connect to Magento - " & objMagento.LastError)
                End If
                Exit For
            End If
        Next

    End Sub

    Private Sub ExtractMagentoAttributes(ByRef MagentoXML As XDocument, ByRef ProductAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType())
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | Code moved from GetMagentoAttributesForSet
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLTemp As XDocument, XMLAttributeNode As XElement, XMLItemNode As XElement
        Dim XMLAttributeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strKey As String, iPtr As Integer

        ' Get the list of attributes
        XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
        XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
        XMLAttributeList = MagentoXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)

        ' Set the output array to the correct dimensions
        ReDim ProductAttributes(m_MagentoImportRule.GetXMLElementListCount(XMLAttributeList) - 1)
        iPtr = 0
        For Each XMLAttributeNode In XMLAttributeList
            XMLItemList = XMLAttributeNode.XPathSelectElements("item")
            For Each XMLItemNode In XMLItemList
                XMLTemp = XDocument.Parse(XMLItemNode.ToString)
                strKey = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                Select Case strKey.ToLower
                    Case "attribute_id"
                        ProductAttributes(iPtr).AttributeID = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                    Case "code"
                        ProductAttributes(iPtr).AttributeName = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")

                    Case "type"
                        ProductAttributes(iPtr).AttributeType = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")

                    Case "required"
                        ProductAttributes(iPtr).AttributeReqd = CBool(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                    Case "scope"
                        ProductAttributes(iPtr).AttributeScope = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")

                End Select
            Next
            iPtr += 1
        Next

    End Sub

    Private Sub ExtractMagentoProductSuperAttributes(ByRef MagentoXML As XDocument, ByRef ProductSuperAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductSuperAttributeType())
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim tempProductSuperAttribute As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductSuperAttributeType
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLTemp As XDocument, XMLAttributeNode As XElement, XMLItemNode As XElement
        Dim XMLAttributeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strKey As String, iPtr As Integer, iLoop As Integer, bOutOfOrder As Boolean

        ' Get the list of attributes
        XMLNameTabMagento = New System.Xml.NameTable
        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
        XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
        XMLNSManMagento.AddNamespace("ns2", "http://xml.apache.org/xml-soap")
        ' no need to cater for WSI Compliant V2 SOAP API as custom API uses V1 SOAP for now
        XMLAttributeList = MagentoXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)

        ' Set the output array to the correct dimensions
        ReDim ProductSuperAttributes(m_MagentoImportRule.GetXMLElementListCount(XMLAttributeList) - 1)
        iPtr = 0
        For Each XMLAttributeNode In XMLAttributeList
            XMLItemList = XMLAttributeNode.XPathSelectElements("item")
            For Each XMLItemNode In XMLItemList
                XMLTemp = XDocument.Parse(XMLItemNode.ToString)
                strKey = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                Select Case strKey.ToLower
                    Case "product_super_attribute_id"
                        ProductSuperAttributes(iPtr).SuperAttributeID = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                    Case "product_id"
                        ProductSuperAttributes(iPtr).ProductID = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                    Case "attribute_id"
                        ProductSuperAttributes(iPtr).AttributeID = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                    Case "frontend_label"
                        ProductSuperAttributes(iPtr).AttributeName = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")

                    Case "attribute_code"
                        ProductSuperAttributes(iPtr).AttributeCode = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")

                    Case "position"
                        ProductSuperAttributes(iPtr).AttributePosition = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                End Select
            Next
            iPtr += 1
        Next

        ' now check array is in correct order
        Do
            bOutOfOrder = False
            For iLoop = 0 To ProductSuperAttributes.Length - 1
                If iLoop + 1 < ProductSuperAttributes.Length Then
                    If ProductSuperAttributes(iLoop).AttributePosition > ProductSuperAttributes(iLoop + 1).AttributePosition Then
                        tempProductSuperAttribute.ProductID = ProductSuperAttributes(iLoop).ProductID
                        tempProductSuperAttribute.SuperAttributeID = ProductSuperAttributes(iLoop).SuperAttributeID
                        tempProductSuperAttribute.AttributeID = ProductSuperAttributes(iLoop).AttributeID
                        tempProductSuperAttribute.AttributeName = ProductSuperAttributes(iLoop).AttributeName
                        tempProductSuperAttribute.AttributeCode = ProductSuperAttributes(iLoop).AttributeCode
                        tempProductSuperAttribute.AttributePosition = ProductSuperAttributes(iLoop).AttributePosition

                        ProductSuperAttributes(iLoop).ProductID = ProductSuperAttributes(iLoop + 1).ProductID
                        ProductSuperAttributes(iLoop).SuperAttributeID = ProductSuperAttributes(iLoop + 1).SuperAttributeID
                        ProductSuperAttributes(iLoop).AttributeID = ProductSuperAttributes(iLoop + 1).AttributeID
                        ProductSuperAttributes(iLoop).AttributeName = ProductSuperAttributes(iLoop + 1).AttributeName
                        ProductSuperAttributes(iLoop).AttributeCode = ProductSuperAttributes(iLoop + 1).AttributeCode
                        ProductSuperAttributes(iLoop).AttributePosition = ProductSuperAttributes(iLoop + 1).AttributePosition

                        ProductSuperAttributes(iLoop + 1).ProductID = tempProductSuperAttribute.ProductID
                        ProductSuperAttributes(iLoop + 1).SuperAttributeID = tempProductSuperAttribute.SuperAttributeID
                        ProductSuperAttributes(iLoop + 1).AttributeID = tempProductSuperAttribute.AttributeID
                        ProductSuperAttributes(iLoop + 1).AttributeName = tempProductSuperAttribute.AttributeName
                        ProductSuperAttributes(iLoop + 1).AttributeCode = tempProductSuperAttribute.AttributeCode
                        ProductSuperAttributes(iLoop + 1).AttributePosition = tempProductSuperAttribute.AttributePosition

                        bOutOfOrder = True
                        Exit For
                    End If
                End If
            Next
        Loop While bOutOfOrder

    End Sub
#End Region

#Region " ImportMagentoProducts "
    Public Sub ImportMagentoProducts(ByVal MagentoInstanceID As String, ByRef ItemsForImport As DataTable, ByRef Cancel As Boolean, _
        ByRef NoOfMagentoProductsProcessed As Integer, ByRef NoOfProductsImported As Integer, ByRef NoOfProductsSkipped As Integer) ' TJS 16/06/13
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
        ' 30/03/11 | TJS             | 2011.0.05 | Modified to set IS category if categories were imported
        ' 04/04/11 | TJS             | 2011.0.07 | Added code to process configurable products
        ' 08/04/11 | TJS             | 2011.0.09 | Added further import log messages 
        ' 13/04/11 | TJS             | 2011.0.10 | Completed configurable product import
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to cater for tag required flag and attribute set id
        ' 04/11/11 | TJS             | 2011.1.10 | Mod1fied to cater for products where SKU is changed in Magento
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, 
        '                                        | corrected sql used to make imported items into Matrix Items
        '                                        | and modified to cater for Inventory stock quantity publishing
        '                                        | Also replaced calls to Interprise.Presentation.Base.Message.MessageWindow.Show  
        '                                        | for error reporting with m_LastError variable as routine is called from a 
        '                                        | backgroundworker thread and cannot directly show error messages
        '                                        | Also added processing for virtual product types and corrected manufacturer code setting
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to cater for simple types with options
        ' 20/05/12 | TJS             | 2012.1.04 | Modified to cater for color and Shirt_size being blank and to set Magento Category IsActive flag
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to cater for importing updates to products and also AttributeSets defining Matrix Group settings
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected loops updating description fields
        ' 09/03/13 | TJS             | 2013.1.01 | Corrected error messages and processing of manufacturer records plus modified 
        '                                        | to allow selection of Item type for simple item types (Stock/Non-Stock)
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to cater for MAgento import Special Price options
        ' 15/03/13 | TJS             | 2013.1.03 | Modified to load Matrix Items when re-importing existing Matrix Groups
        '                                        | and create InventoryMagentoDetails rows for simple+opt Matrix Items
        ' 18/03/13 | TJS             | 2013.1.04 | Corrected setting of Magento Product ID on InventoryMagentoDetails rows for simple+opt Matrix Items
        ' 24/03/13 | TJS             | 2013.1.06 | Modified to cater for configurable products with options, for character case discrepancies in attribute name and values
        ' 29/03/13 | TJS             | 2013.1.07 | Modifed to correct import of simple+opt Matrix Items with multpile options
        '                                        | and handle updating of changes to simple options which change matrix items to stock
        ' 03/04/13 | TJS             | 2013.1.08 | Corrected setting of Magento prices and to cater for MAgento login and SKUs potentially containing XML entities
        ' 30/04/13 | TJS             | 2013.1.11 | Corrected detection of matching/empty attributes on simple+opt products
        '                                        | and modified to ensure existing Matrix Group Items get all attributes set when updating product
        ' 03/05/13 | TJS             | 2013.1.12 | Moved block of code into separate subroutines to simplify maintenance and 
        '                                        | modified to cater for ' characters in Item names
        ' 20/05/13 | TJS             | 2013.1.15 | Modified to rename the matrix Group for simple+opt products so that one matrix item can have the base Magento SKU for no options
        ' 23/05/13 | TJS             | 2013.1.16 | Modified to mark Inventory Items as obsolete when item type changes
        ' 28/05/13 | TJS             | 2013.1.17 | Modified to update Item Name in db with MagentoSimplePlusOptNameSuffix prior to loading existing data if suffix was omitted on Original import
        ' 29/05/13 | FA/TJS          | 2013.1.18 | Modified to only highlight instance missing after all instances have been checked 
        '                                        | and to trap attempts to rename products to conflicting names
        ' 01/06/13 | TJS             | 2013.1.19 | Corrected detection of instance matching errors
        ' 15/06/13 | TJS             | 2013.1.20 | Corrected setting of ItemCode on magento tag details and categories after save
        ' 16/06/13 | TJS             | 2013.1.20 | Modified to update SKU changes and then do rest of import.  Also changed reporting of number 
        '                                        | of items processed and imported
        ' 23/06/13 | TJS             | 2013.1.22 | Corrected loop error when removing unused matrix items and modified to only set ItemCode on MagentoDetails
        '                                        | record if code was a new record and added code to detect existing matrix items with name suffix (lrynec)
        ' 26/06/13 | TJS             | 2013.1.23 | Modified to detect SKU clashes which can be cause when a Magento simple with options has the same  option SKU as another product
        '                                        | Corrected error in above. Also modified to clear SystemManufacturerSourceID_DEV000221 when import error occurs.  
        ' 03/07/13 | TJS             | 2013.1.24 | Set/Update flag to inform services that import is running
        ' 12/07/13 | TJS             | 2013.1.28 | Modified to only report unsupport product types once
        ' 09/08/13 | TJS             | 2013.1.32 | Modified to detect when API session has expired and re-login
        '                                        | and to cater for SourceIDChanged flag
        ' 18/08/13 | TJs             | 2013.2.03 | Modified to cater for XMLPackage now being on InventoryItemWebOption table
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for multiple items needing marking as obsolete and report SKU being processed if error occurs
        '                                        | and to cater for the same product being marked as obsolete more than once
        ' 30/09/13 | TJS             | 2013.3.02 | Modified to decode Config XML Value such as password etc 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2SoapAPIWSICompliant and category mapping.  Corrected
        '                                        | string constants used to indicate product name and description are taken from InventoryItem table
        ' 23/01/14 | TJS             | 2013.4.06 | Modified to detect InventoryMagentoDetails_DEV000221 rows where MAgento ID not yet added
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple magento sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoDetails_DEV000221Row
        Dim rowMagentoTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row
        Dim rowManufacturerItems(0) As System.Data.DataRow
        Dim objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim ProductAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType() ' TJS 10/06/12
        Dim ProductAttribute As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType ' TJS 10/06/12
        Dim ProductSuperAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductSuperAttributeType() ' TJS 10/06/12
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strProductType As String, strErrorDetails As String, strTemp As String
        Dim strHomeCurrency As String, strInstanceID As String, strItemCode As String
        Dim strHomeLanguage As String, strMagentoID As String ' TJS 26/06/13
        Dim paramset As String()() ' TJS 15/03/13
        Dim iStoreID As Integer ' TJS 04/04/11
        Dim iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer
        Dim iItemLoop As Integer, iLoop As Integer
        Dim bInstanceMatched As Boolean, bISItemExists As Boolean, bISMagentoItemExists As Boolean, bFirstPassDone As Boolean ' TJS 10/06/12
        Dim bMatrixAttributeError As Boolean, bManufacturererFound As Boolean, bNoOptionsToAdd As Boolean  ' TJS 04/04/11 TJS 13/04/11 TJS 20/05/13
        Dim bDataError As Boolean, bTagExists As Boolean, bExitFor As Boolean, bContinueFor As Boolean ' TJS 04/11/11 TJS 10/06/12 TJS 03/05/13
        Dim strMagentoURL As String, strMagentoAPIUser As String, strMagentoAPIPwd As String, strSKUBeingProcessed As String = "" ' TJS 09/08/13 TJS 19/09/13
        Dim strSplitItemSKU As String, strOmittedOptionSKUs As String, strCheckItem() As String ' TJS 29/03/13
        Dim strCheckMatrixItems()() As String, strObsoleteItems()() As String, strMatrixItemTempSuffix As String = "" ' TJS 23/06/13 TJS 19/09/13
        Dim iObsoleteSuffix As Integer, bMagentoV2APIWSI As Boolean ' TJS 19/09/13 TJS 05/10/13

        Try
            m_LastErrorMessage = "" ' TJS 14/02/12
            m_LastError = "" ' TJS 02/12/11
            m_ImportLimitReached = False ' TJS 02/12/11
            m_ManualActionRequired = False ' TJS 24/03/13
            m_ProcessLog = "" ' TJS 29/03/11
            If m_MagentoImportRule.IsActivated Then ' TJS 29/03/11
                strErrorDetails = ""
                m_ProductsAlreadyImported = GetInventoryItemsImportedCount() ' TJS 29/03/11
                If Not m_MagentoImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then ' TJS 29/03/11
                    strHomeCurrency = Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                    strHomeLanguage = Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
                    XMLConfig = XDocument.Parse(Trim(m_MagentoImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                    bInstanceMatched = False ' TJS 01/06/13
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If m_MagentoImportRule.GetXMLElementListCount(XMLNodeList) = 1 Then
                            bInstanceMatched = True
                        ElseIf m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoInstanceID Then
                            bInstanceMatched = True
                        End If
                        If bInstanceMatched Then
                            If m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL) <> "" And _
                                m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER) <> "" Then
                                strSplitItemSKU = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_SPLIT_SKU_SEPARATOR_CHARACTERS) ' TJS 29/03/13
                                strInstanceID = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID)
                                strMagentoURL = m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)) ' TJS 30/09/13
                                strMagentoAPIUser = m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER)) ' TJS 30/09/13
                                strMagentoAPIPwd = m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)) ' TJS 30/09/13
                                bMagentoV2APIWSI = CBool(IIf(UCase(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False)) ' TJS 13/11/13
                                objMagento = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
                                objMagento.V2SoapAPIWSICompliant = bMagentoV2APIWSI ' TJS 13/11/13
                                objMagento.MagentoVersion = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION) ' TJS 13/11/13
                                strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION) ' TJS 13/11/13
                                If strTemp <> "" Then
                                    objMagento.LerrynAPIVersion = CDec(strTemp) ' TJS 13/11/13
                                Else
                                    objMagento.LerrynAPIVersion = 0 ' TJS 13/11/13
                                End If
                                If objMagento.Login(strMagentoURL, m_MagentoImportRule.ConvertForXML(strMagentoAPIUser), m_MagentoImportRule.ConvertForXML(strMagentoAPIPwd)) Then ' TJS 03/04/13
                                    If m_CreateISCategories Then ' TJS 29/03/11
                                        ImportCategories(MagentoInstanceID, objMagento, strHomeLanguage) ' TJS 29/03/11 TJS 13/11/13
                                    End If
                                    GetMagentoAttributeValues(objMagento) ' TJS 08/04/11
                                    NewItemDataset = New Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
                                    NewItemFacade = New Interprise.Facade.Inventory.ItemDetailFacade(NewItemDataset)
                                    bFirstPassDone = False
                                    Do
                                        For iItemLoop = 0 To ItemsForImport.Rows.Count - 1
                                            strSKUBeingProcessed = ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString ' TJS 19/09/13
                                            ' Set/Update flag to inform services that import is running
                                            strTemp = "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET LastImportWizardUpdate_DEV000221 = getdate()" ' TJS 03/07/13
                                            Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing) ' TJS 03/07/13

                                            ' start of code moved TJS 16/06/13
                                            If CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And _
                                                CBool(ItemsForImport.Rows(iItemLoop).Item("AlreadyImported")) And _
                                                CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) And Not bFirstPassDone Then ' TJS 04/11/11 FA/TJS 29/05/13
                                                ' product SKU has change, user has selected import so update SKU in IS provided name doesn't already exist
                                                If String.IsNullOrEmpty(Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") & "' AND ItemCode <> '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'")) Then 'FA/TJS 29/05/13
                                                    strTemp = "UPDATE InventoryItem SET ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") ' TJS 04/11/11 TJS 03/05/13
                                                    strTemp = strTemp & "' WHERE ItemCode = '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'" ' TJS 04/11/11
                                                    Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing) ' TJS 04/11/11
                                                    ' now remove SKU Changed flag so rest of item can be updated
                                                    ItemsForImport.Rows(iItemLoop).Item("SKUChanged") = False ' TJS 16/06/13
                                                Else
                                                    m_LastError = "Unable to update Item Name for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " (ItemCode " & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & ") as name conflicts with another Item " 'FA/TJS 29/05/13
                                                    m_LastErrorMessage = m_LastError 'FA/TJS 29/05/13
                                                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as " & m_LastError & vbCrLf 'FA/TJS 29/05/13
                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1 'FA/TJS 29/05/13
                                                End If
                                            End If
                                            ' end of code moved TJS 16/06/13

                                            ' start of code added TJS 09/08/13
                                            If CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And _
                                                CBool(ItemsForImport.Rows(iItemLoop).Item("AlreadyImported")) And _
                                                CBool(ItemsForImport.Rows(iItemLoop).Item("SourceIDChanged")) And Not bFirstPassDone Then
                                                ' Magento product ID has changed, useer has selected import so update Magento product ID in IS provided ID doesn't already exist
                                                If String.IsNullOrEmpty(Me.GetField("ItemCode_DEV000221", "InventoryMagentoDetails_DEV000221", "MagentoProductID_DEV000221 = '" & ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString & "' AND InstanceID_DEV000221 = '" & strInstanceID.Replace("'", "''") & "' AND ItemCode_DEV000221 <> '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'")) Then
                                                    strTemp = "UPDATE InventoryMagentoDetails_DEV000221 SET MagentoProductID_DEV000221 = '" & ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString.Replace("'", "''")
                                                    strTemp = strTemp & "' WHERE ItemCode_DEV000221 = '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'"
                                                    Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing)
                                                    ' now remove Source ID Changed flag so rest of item can be updated
                                                    ItemsForImport.Rows(iItemLoop).Item("SourceIDChanged") = False
                                                Else
                                                    m_LastError = "Unable to update Magento product ID for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " (ItemCode " & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & ") as ID conflicts with another Item "
                                                    m_LastErrorMessage = m_LastError
                                                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as " & m_LastError & vbCrLf
                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                End If
                                            End If
                                            ' end of code added TJS 09/08/13

                                            ' is Import box checked and not already imported and SKU and Source ID not changed ?
                                            If CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And _
                                                Not CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) And _
                                                Not CBool(ItemsForImport.Rows(iItemLoop).Item("SourceIDChanged")) Then ' TJS 04/11/11 TJS 10/06/12 TJS 09/08/13
                                                ' yes, process simple products on first pass and groups etc on second pass
                                                bDataError = False 'TJS 04/11/11
                                                strOmittedOptionSKUs = "" ' TJS 29/03/13
                                                strProductType = ItemsForImport.Rows(iItemLoop).Item("SourceType").ToString
                                                If ((strProductType = "simple" Or strProductType = "simple+opt" Or strProductType = "virtual") And Not bFirstPassDone) Or _
                                                    ((strProductType = "grouped" Or strProductType = "configurable" Or strProductType = "configurable+opt") And bFirstPassDone) Then ' TJS 04/04/11 TJS 04/11/11 TJS 09/03/13 TJS 24/03/13
                                                    ' get product details from Magento
                                                    If objMagento.GetCatalogProductDetail(ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString) Then
                                                        ' get product XML item list
                                                        XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                                                        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                                                        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                                                        XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                                                        XMLItemList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                                                        bManufacturererFound = False
                                                        NewManufacturerFacade = Nothing
                                                        NewManufacturerDataset = Nothing
                                                        NewKitFacade = Nothing
                                                        NewKitDataset = Nothing
                                                        iStoreID = -1 ' TJS 04/04/11
                                                        ' check Item Name doesn't already exist
                                                        bISItemExists = False
                                                        strItemCode = ""
                                                        ' are we importing a Magento simple product with options which imports as a Matrix Group and needs a suffix on the Group Name ?
                                                        If strProductType = "simple+opt" Then ' TJS 20/05/13 
                                                            ' yes - try getting ItemCode using suffix first
                                                            strCheckItem = Me.GetRow(New String() {"ItemCode", "ItemType", "ItemName"}, "InventoryItem", "ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") & MagentoSimplePlusOptNameSuffix & "' AND ItemType = 'Matrix Group'") ' TJS 20/05/13 TJS 21/05/13
                                                            ' did we find the item ?
                                                            If strCheckItem Is Nothing OrElse strCheckItem(0) = "" Then ' TJS 20/05/13 
                                                                ' no, try using name without suffix
                                                                strCheckItem = Me.GetRow(New String() {"ItemCode", "ItemType", "ItemName"}, "InventoryItem", "ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") & "' AND ItemType = 'Matrix Group'") ' TJS 20/05/13 TJS 21/05/13
                                                                ' did we find the item ?
                                                                If strCheckItem IsNot Nothing AndAlso strCheckItem(0) <> "" Then ' TJS 28/05/13 
                                                                    ' yes, need to update Item Name to prevent later errors
                                                                    Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItem SET ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") & MagentoSimplePlusOptNameSuffix & "' WHERE ItemCode = '" & strCheckItem(0) & "'", Nothing) ' TJS 28/05/13
                                                                    strCheckItem(2) = ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & MagentoSimplePlusOptNameSuffix ' TJS 28/05/13
                                                                    ' start of code added TJS 16/06/13
                                                                Else
                                                                    ' no, check for the item as something other then a matrix item
                                                                    strCheckItem = Me.GetRow(New String() {"ItemCode", "ItemType", "ItemName"}, "InventoryItem", "ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") & "' AND ItemType <> 'Matrix Item'") ' TJS 16/06/13
                                                                    If strCheckItem IsNot Nothing AndAlso strCheckItem(0) <> "" Then ' TJS 16/06/13
                                                                        ' yes, mark item as Obsolete to prevent errors creating matrix items
                                                                        iObsoleteSuffix = 1 ' TJS 19/09/13
                                                                        Do While iObsoleteSuffix < 100 ' TJS 19/09/13
                                                                            If iObsoleteSuffix > 1 Then ' TJS 19/09/13
                                                                                strTemp = strCheckItem(2) & "-OBSOLETE" & iObsoleteSuffix ' TJS 19/09/13
                                                                            Else
                                                                                strTemp = strCheckItem(2) & "-OBSOLETE"
                                                                            End If
                                                                            If strTemp.Length > CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryItem' AND ColumnName = 'ItemName'")) Then
                                                                                strTemp = strCheckItem(2).Substring(0, CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryItem' AND ColumnName = 'ItemName'")) - 11) & "-OBSOLETE"
                                                                            End If
                                                                            If String.IsNullOrEmpty(Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & strTemp.Replace("'", "''") & "'")) Then ' TJS 19/09/13
                                                                                Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItem SET ItemName = '" & strTemp.Replace("'", "''") & "', Status = 'D' WHERE ItemCode = '" & strCheckItem(0) & "'", Nothing)
                                                                                Exit Do ' TJS 19/09/13
                                                                            End If
                                                                            iObsoleteSuffix += 1 ' TJS 19/09/13
                                                                        Loop
                                                                        strCheckItem = Nothing
                                                                    End If
                                                                    ' end of code added TJS 16/06/13
                                                                End If
                                                            End If
                                                        Else
                                                            strCheckItem = Me.GetRow(New String() {"ItemCode", "ItemType", "ItemName"}, "InventoryItem", "ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") & "'") ' TJS 29/03/13 TJS 03/05/13 TJS 21/05/13
                                                        End If
                                                        If strCheckItem IsNot Nothing AndAlso strCheckItem(0) <> "" Then ' TJS 29/03/13
                                                            strItemCode = strCheckItem(0) ' TJS 29/03/13
                                                            bISItemExists = True
                                                        End If
                                                        ' start of code added TJS 29/03/13
                                                        ' Are we updating an existing item to a Matrix Group or Kit ?
                                                        If bISItemExists AndAlso (strProductType = "simple+opt" Or strProductType = "grouped" Or strProductType = "configurable" Or _
                                                            strProductType = "configurable+opt") AndAlso (strCheckItem(1) = ITEM_TYPE_STOCK Or _
                                                            strCheckItem(1) = ITEM_TYPE_NON_STOCK Or strCheckItem(1) = ITEM_TYPE_SERVICE Or _
                                                            strCheckItem(1) = ITEM_TYPE_MATRIX_ITEM) Then ' TJS 23/05/13
                                                            ' yes, must have had options converted to Split SKU so mark item as Obsolete
                                                            strObsoleteItems = Me.GetRows(New String() {"ItemCode", "ItemName"}, "InventoryItem", "ItemCode = '" & strItemCode & "' OR ItemCode IN (SELECT MatrixItemCode FROM InventoryMatrixItem WHERE ItemCode = '" & strItemCode & "')") ' TJS 19/09/13
                                                            iObsoleteSuffix = 1 ' TJS 19/09/13
                                                            For Each strObsoleteItem As String() In strObsoleteItems ' TJS 19/09/13
                                                                Do While iObsoleteSuffix < 100 ' TJS 19/09/13
                                                                    If iObsoleteSuffix > 1 Then ' TJS 19/09/13
                                                                        strTemp = strObsoleteItem(1) & "-OBSOLETE" & iObsoleteSuffix ' TJS 19/09/13
                                                                    Else
                                                                        strTemp = strObsoleteItem(1) & "-OBSOLETE" ' TJS 23/05/13 TJS 19/09/13
                                                                    End If
                                                                    If strTemp.Length > CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryItem' AND ColumnName = 'ItemName'")) Then ' TJS 23/05/13
                                                                        strTemp = strObsoleteItem(1).Substring(0, CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryItem' AND ColumnName = 'ItemName'")) - 11) & "-OBSOLETE" ' TJS 23/05/13
                                                                    End If
                                                                    If String.IsNullOrEmpty(Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & strTemp.Replace("'", "''") & "'")) Then ' TJS 19/09/13
                                                                        Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItem SET ItemName = '" & strTemp.Replace("'", "''") & "', Status = 'D' WHERE ItemCode = '" & strObsoleteItem(0) & "'", Nothing) ' TJS 23/05/13 TJS 19/09/13
                                                                        Exit Do ' TJS 19/09/13
                                                                    End If
                                                                    iObsoleteSuffix += 1 ' TJS 19/09/13
                                                                Loop
                                                            Next
                                                            strItemCode = ""
                                                            bISItemExists = False
                                                        End If
                                                        ' end of code added TJS 29/03/13
                                                        If bISItemExists Then
                                                            ' load existing item
                                                            NewItemFacade.LoadInventoryItem(strItemCode) ' TJS 10/06/12
                                                            NewItemFacade.LoadItemDescription(strItemCode, strHomeLanguage) ' TJS 10/06/12
                                                            ' start of code added TJS 15/03/13
                                                            If NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                                                                paramset = New String()() {New String() {NewItemDataset.InventoryMatrixGroup.TableName, READINVENTORYMATRIXGROUP, AT_ITEM_CODE, strItemCode}, _
                                                                    New String() {NewItemDataset.InventoryAttribute.TableName, GETINVENTORYATTRIBUTE, AT_ITEM_CODE, strItemCode}, _
                                                                    New String() {NewItemDataset.InventoryAttributeValue.TableName, GETINVENTORYATTRIBUTEVALUE, AT_ITEM_CODE, strItemCode, AT_LANGUAGE_CODE, strHomeLanguage}, _
                                                                    New String() {NewItemDataset.InventoryMatrixItem.TableName, READINVENTORYMATRIXITEM, AT_ITEM_CODE, strItemCode, AT_LANGUAGE_CODE, strHomeLanguage}}
                                                                NewItemFacade.LoadDataSet(paramset, ClearType.Specific)
                                                            End If
                                                            ' end of code added TJS 15/03/13

                                                            ' start of code added TJS 23/05/13
                                                            ' is this an existing item which needs changing from a Matrix Group to a standard Stock Item 
                                                            ' i.e. product options have been removed or changed to Split SKU ones
                                                            If bISItemExists AndAlso strProductType = "simple" AndAlso strCheckItem(1) = ITEM_TYPE_MATRIX_GROUP Then
                                                                ' start of code replaced TJS 06/02/14
                                                                strTemp = "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as existing Inventory Item type ("
                                                                strTemp = strTemp & strCheckItem(1) & ") is not compatible with the current Magento type (" & strProductType
                                                                strTemp = strTemp & ").  In order to import this product from Magento, you need to mark the existing CB Inventory Item "
                                                                strTemp = strTemp & "as Discontinued, amend it's Item Name and then re-run the Import Wizard"
                                                                m_LastError = strTemp
                                                                m_LastErrorMessage = strTemp
                                                                m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                                                                Continue For
                                                                ' end of code replaced TJS 06/02/14
                                                            End If
                                                            ' end of code added TJS 23/05/13
                                                        Else
                                                            NewItemDataset.EnforceConstraints = False
                                                            NewItemDataset.InventoryMatrixGroup.Clear()
                                                            NewItemDataset.InventoryMatrixItem.Clear() ' TJS 15/03/13
                                                            NewItemDataset.InventoryAttribute.Clear()
                                                            NewItemDataset.InventoryAttributeValue.Clear()
                                                            NewItemDataset.InventoryAssembly.Clear()
                                                            NewItemDataset.InventoryKit.Clear()
                                                            NewItemDataset.InventoryAssemblyDetailView.Clear()
                                                            NewItemDataset.InventoryItemPricingDetail.Clear()
                                                            NewItemDataset.EnforceConstraints = True
                                                            ' create default item records
                                                            If strProductType = "virtual" Then ' TJS 02/12/11
                                                                ' create default non-stock item records
                                                                NewItemFacade.AddItem(TEMPORARY_DOCUMENTCODE, ITEM_DEFAULT_CLASS_NON_STOCK, ITEM_TYPE_NON_STOCK) ' TJS 02/12/11
                                                                NewItemFacade.LanguageCode = strHomeLanguage
                                                                NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_NON_STOCK) ' TJS 02/12/11

                                                                ' start of code added TJS 24/02/12
                                                            ElseIf strProductType = "simple+opt" Then
                                                                ' create default matrix group item record
                                                                NewItemFacade.AddItem(TEMPORARY_DOCUMENTCODE, ITEM_DEFAULT_CLASS_MATRIX_GROUP, ITEM_TYPE_MATRIX_GROUP)
                                                                NewItemFacade.LanguageCode = strHomeLanguage
                                                                NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_MATRIX_GROUP)
                                                                ' end of code added TJS 24/02/12

                                                            Else
                                                                ' create default item records
                                                                If ItemsForImport.Rows(iItemLoop).Item("ISItemType").ToString = "Non-Stock" Then ' TJS 09/03/13
                                                                    NewItemFacade.AddItem(TEMPORARY_DOCUMENTCODE, ITEM_DEFAULT_CLASS_NON_STOCK, ITEM_TYPE_NON_STOCK) ' TJS 09/03/13
                                                                Else
                                                                    NewItemFacade.AddItem(TEMPORARY_DOCUMENTCODE, ITEM_DEFAULT_CLASS_STOCK, ITEM_TYPE_STOCK)
                                                                End If
                                                                NewItemFacade.LanguageCode = strHomeLanguage
                                                                If ItemsForImport.Rows(iItemLoop).Item("ISItemType").ToString = "Non-Stock" Then ' TJS 09/03/13
                                                                    NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_NON_STOCK) ' TJS 09/03/13
                                                                Else
                                                                    NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_STOCK)
                                                                End If
                                                            End If
                                                            NewItemFacade.AddWebOption()
                                                        End If

                                                        ' start of code added TJS 10/06/12
                                                        bISMagentoItemExists = False
                                                        ReDim ProductAttributes(0)
                                                        ReDim ProductSuperAttributes(0)
                                                        strTemp = "((MagentoProductID_DEV000221 = '" & ItemsForImport.Rows(iItemLoop).Item("SourceItemID").ToString & _
                                                            "' AND InstanceID_DEV000221 = '" & strInstanceID.Replace("'", "''") & "') OR (ItemCode_DEV000221 = '" & _
                                                            strItemCode & "' AND InstanceID_DEV000221 = '" & strInstanceID.Replace("'", "''") & "' AND MagentoProductID_DEV000221 IS Null))" & _
                                                            "AND SourceIsGroupItem_DEV000221 = 0"  'TJS/FA 23/01/14
                                                        strItemCode = Me.GetField("ItemCode_DEV000221", "InventoryMagentoDetails_DEV000221", strTemp) ' TJS 18/03/13 TJS 23/05/13
                                                        If Not String.IsNullOrEmpty(strItemCode) Then
                                                            bISMagentoItemExists = True
                                                            Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                                                "ReadInventoryMagentoDetails_DEV000221", AT_ITEMCODE, strItemCode, AT_INSTANCE_ID, strInstanceID}, _
                                                                New String() {m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.TableName, "ReadInventoryMagentoTagDetails_DEV000221", _
                                                                AT_ITEMCODE, strItemCode, AT_INSTANCE_ID, strInstanceID}, _
                                                                New String() {m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.TableName, "ReadInventoryMagentoCategories_DEV000221", _
                                                                AT_ITEMCODE, strItemCode, AT_INSTANCE_ID, strInstanceID}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                                            ' start of code added TJS/FA 26/06/13
                                                        ElseIf bISItemExists Then
                                                            ' didn't find a Magento record using the MAgento Product ID, do we have a Magento record for this ItemCode on this instance
                                                            ' this can happen if user has a Magento simple product with options and then has the same item SKU on its own
                                                            strMagentoID = Me.GetField("MagentoProductID_DEV000221", "InventoryMagentoDetails_DEV000221", "ItemCode_DEV000221 = '" & strCheckItem(0) & "' AND InstanceID_DEV000221 = '" & strInstanceID.Replace("'", "''") & "'") 'FA 26/06/13 replaced strItemCode with strCheckItem(0)
                                                            If Not String.IsNullOrEmpty(strMagentoID) Then
                                                                NewItemDataset.Clear()
                                                                If NewManufacturerFacade IsNot Nothing Then
                                                                    NewManufacturerFacade.Dispose()
                                                                    NewManufacturerDataset.Dispose()
                                                                End If
                                                                If NewKitFacade IsNot Nothing Then
                                                                    NewKitFacade.Dispose()
                                                                    NewKitDataset.Dispose()
                                                                End If
                                                                m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Clear()
                                                                m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Clear()
                                                                m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Clear()
                                                                m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Clear() ' TJS/FA 26/06/13
                                                                strTemp = Me.GetField("ItemCode_DEV000221", "InventoryMagentoDetails_DEV000221", "MagentoProductID_DEV000221 = '" & strMagentoID & "' AND InstanceID_DEV000221 = '" & strInstanceID.Replace("'", "''") & "' AND SourceIsGroupItem_DEV000221 = 0")
                                                                strTemp = Me.GetField("ItemName", "InventoryItem", "ItemCode = '" & strTemp & "'")
                                                                m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as SKU already exists from another Inventory Item (" & strCheckItem(0) & ", SKU " & strTemp & ")" & vbCrLf 'FA 26/06/13 replaced strItemCode with strCheckItem(0)
                                                                NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                Continue For
                                                            End If
                                                            ' end of code added TJS/FA 26/06/13
                                                        End If
                                                        If bISMagentoItemExists Then
                                                            rowMagentoDetails = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0)
                                                        Else
                                                            ' end of code added TJS 10/06/12
                                                            rowMagentoDetails = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.NewInventoryMagentoDetails_DEV000221Row
                                                            rowMagentoDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode ' TJS 06/02/14
                                                            rowMagentoDetails.SellingPrice_DEV000221 = 0
                                                            rowMagentoDetails.InstanceID_DEV000221 = strInstanceID
                                                            rowMagentoDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
                                                            rowMagentoDetails.ProductShortDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION ' TJS 13/11/13
                                                            rowMagentoDetails.FromImportWizard_DEV000221 = True
                                                            rowMagentoDetails.QtyPublishingType_DEV000221 = m_QuantityPublishingType ' TJS 02/12/11
                                                            rowMagentoDetails.QtyPublishingValue_DEV000221 = CInt(m_QuantityPublishingValue) ' TJS 02/12/11
                                                            rowMagentoDetails.TotalQtyWhenLastPublished_DEV000221 = 0 ' TJS 02/12/11
                                                            rowMagentoDetails.QtyLastPublished_DEV000221 = 0 ' TJS 02/12/11
                                                        End If
                                                        If Not rowMagentoDetails.Publish_DEV000221 Then ' TJS 06/02/14
                                                            rowMagentoDetails.Publish_DEV000221 = True
                                                        End If
                                                        If Not bISMagentoItemExists Then ' TSJ 10/06/12
                                                            m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.AddInventoryMagentoDetails_DEV000221Row(rowMagentoDetails)
                                                        End If

                                                        ' now process each Magento Key/Value pair
                                                        ProcessMagentoXMLKeyValues(objMagento, strInstanceID, strHomeCurrency, strProductType, ItemsForImport.Rows(iItemLoop), _
                                                            XMLItemList, ProductSuperAttributes, rowMagentoDetails, bISItemExists, bFirstPassDone, iStoreID, _
                                                            bManufacturererFound, bDataError) ' TJS 03/05/13

                                                        If Not bDataError Then ' TJS 04/11/11
                                                            ' start of code added TJS 10/06/12
                                                            If ProductAttributes.Length > 1 OrElse Not String.IsNullOrEmpty(ProductAttributes(0).AttributeName) Then
                                                                For Each ProductAttribute In ProductAttributes
                                                                    bTagExists = False
                                                                    For iLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Count - 1
                                                                        If m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagName_DEV000221 = ProductAttribute.AttributeName Then
                                                                            bTagExists = True
                                                                            If m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagRequired_DEV000221Null OrElse _
                                                                                m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagRequired_DEV000221 <> ProductAttribute.AttributeReqd Then
                                                                                m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagRequired_DEV000221 = ProductAttribute.AttributeReqd
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
                                                                                rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                                                                                rowMagentoTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                                                                rowMagentoTagDetails.InstanceID_DEV000221 = strInstanceID
                                                                                rowMagentoTagDetails.TagName_DEV000221 = ProductAttribute.AttributeName
                                                                                rowMagentoTagDetails.TagDisplayName_DEV000221 = ProductAttribute.AttributeName.Replace("_", " ")
                                                                                rowMagentoTagDetails.LineNumber_DEV000221 = 1
                                                                                rowMagentoTagDetails.TagRequired_DEV000221 = ProductAttribute.AttributeReqd
                                                                                Select Case ProductAttribute.AttributeType.ToLower
                                                                                    Case "text"
                                                                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                                                                                    Case "textarea"
                                                                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Memo"
                                                                                    Case "date"
                                                                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Date"
                                                                                    Case "price"
                                                                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Numeric"
                                                                                    Case Else
                                                                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                                                                                End Select
                                                                                SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                                                                                m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)

                                                                        End Select
                                                                    End If
                                                                Next
                                                            End If
                                                            ' end of code added TJS 10/06/12

                                                            ' start of code added TJS 24/02/12
                                                            If strProductType = "simple+opt" And Not bFirstPassDone Then
                                                                ' get custom options from Magento and process them
                                                                bNoOptionsToAdd = False ' TJS 20/05/13
                                                                ProcessMagentoSimpleOptionsPt1(objMagento, XMLNSManMagento, strSplitItemSKU, ItemsForImport.Rows(iItemLoop), _
                                                                    strOmittedOptionSKUs, bISItemExists, bMatrixAttributeError, bNoOptionsToAdd) ' TJS 03/05/13 TJS 20/05/13
                                                                If bMatrixAttributeError Then ' start TJS/FA 29/05/13
                                                                    NewItemDataset.Clear()
                                                                    If NewManufacturerFacade IsNot Nothing Then
                                                                        NewManufacturerFacade.Dispose()
                                                                        NewManufacturerDataset.Dispose()
                                                                    End If
                                                                    If NewKitFacade IsNot Nothing Then
                                                                        NewKitFacade.Dispose()
                                                                        NewKitDataset.Dispose()
                                                                    End If
                                                                    m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Clear()
                                                                    m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Clear()
                                                                    m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Clear()
                                                                    m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Clear() ' TJS /FA 26/06/13
                                                                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as option SKU is blank" & vbCrLf
                                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                                    Continue For ' end TJS/FA 29/05/13
                                                                End If

                                                                ' start of code added TJS 04/04/11 and updated TJS 13/04/11
                                                            ElseIf (strProductType = "configurable" Or strProductType = "configurable+opt") And bFirstPassDone Then ' TJS 24/03/13
                                                                ' get relate product details from Magento (it includes the attribute details) and process them
                                                                bExitFor = False ' TJS 03/05/13
                                                                bContinueFor = False ' TJS 03/05/13
                                                                ProcessMagentoRelatedProducts(objMagento, XMLNSManMagento, strProductType, ItemsForImport.Rows(iItemLoop), _
                                                                    ProductSuperAttributes, bMatrixAttributeError, bExitFor, bContinueFor) ' TJS 03/05/13
                                                                If bExitFor Then ' TJS 03/05/13
                                                                    Exit For ' TJS 03/05/13
                                                                ElseIf bContinueFor Then ' TJS 03/05/13
                                                                    Continue For ' TJS 03/05/13
                                                                End If
                                                            End If
                                                            ' end of code added TJS 04/04/11 and updated TJS 13/04/11

                                                            NewItemFacade.IsFinished = True
                                                            ' start of code added TJS 02/08/12
                                                            If (strProductType = "configurable" Or strProductType = "configurable+opt") And bFirstPassDone Then ' TJS 24/03/13
                                                                ' add suffix to ItemName for Matrix Items to prevent IS from saving new InventoryMatrixItem records
                                                                strTemp = ""
                                                                For iLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
                                                                    If strTemp <> "" Then
                                                                        strTemp = strTemp & ", "
                                                                    End If
                                                                    strTemp = strTemp & "'" & NewItemDataset.InventoryMatrixItem(iLoop).MatrixItemCode & "'"
                                                                Next
                                                                If strTemp <> "" Then
                                                                    ' start of code added TJS 23/06/13
                                                                    strMatrixItemTempSuffix = "lrynec"
                                                                    iLoop = 0
                                                                    Do
                                                                        If iLoop = 0 Then
                                                                            strCheckMatrixItems = Me.GetRows(New String() {"ItemCode"}, "InventoryItem", "ItemName in (SELECT ItemName + '" & strMatrixItemTempSuffix & "' from InventoryItem WHERE ItemCode IN (" & strTemp & "))")
                                                                        Else
                                                                            strCheckMatrixItems = Me.GetRows(New String() {"ItemCode"}, "InventoryItem", "ItemName in (SELECT ItemName + '" & strMatrixItemTempSuffix & iLoop & "' from InventoryItem WHERE ItemCode IN (" & strTemp & "))")
                                                                        End If
                                                                        If strCheckMatrixItems Is Nothing OrElse strCheckMatrixItems.Length = 0 OrElse strCheckMatrixItems(0)(0) = "" Then
                                                                            If iLoop > 0 Then
                                                                                strMatrixItemTempSuffix = strMatrixItemTempSuffix & iLoop
                                                                            End If
                                                                            Exit Do
                                                                        End If
                                                                        iLoop = iLoop + 1
                                                                    Loop
                                                                    ' end of code added TJS 23/06/13
                                                                    Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItem SET ItemName = ItemName + '" & strMatrixItemTempSuffix & "', UserModified = '" & _
                                                                        Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & _
                                                                        "', DateModified = getdate()  WHERE ItemCode IN (" & strTemp & ")", Nothing) ' TJS 23/06/13
                                                                End If
                                                            End If
                                                            ' end of code added TJS 02/08/12
                                                            If NewItemFacade.UpdateDataSet(NewItemFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                                                                ' start of code added TJS 16/06/13
                                                                If (strProductType = "configurable" Or strProductType = "configurable+opt") And _
                                                                    bISItemExists And bFirstPassDone Then
                                                                    ' check for and delete any dummy matrix item records
                                                                    For iLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1 ' TJS 23/06/13
                                                                        ' if a row has been deleted, matric item count will have reduced whilst loop will try to process original row count
                                                                        ' so check we are not trying to access a deleted (non-existent) row
                                                                        If iLoop <= NewItemDataset.InventoryMatrixItem.Count - 1 Then ' TJS 23/06/13
                                                                            If Len(NewItemDataset.InventoryMatrixItem(iLoop).MatrixItemCode) > Len(NewItemDataset.InventoryMatrixItem(iLoop).ItemCode & "-Dmy") Then
                                                                                If NewItemDataset.InventoryMatrixItem(iLoop).MatrixItemCode.Substring(0, Len(NewItemDataset.InventoryMatrixItem(iLoop).ItemCode & "-Dmy")) = NewItemDataset.InventoryMatrixItem(iLoop).ItemCode & "-Dmy" Then
                                                                                    Me.ExecuteNonQuery(CommandType.Text, "DELETE FROM InventoryMatrixItem WHERE ItemCode = '" & NewItemDataset.InventoryMatrixItem(iLoop).ItemCode & "' AND MatrixItemCode = '" & NewItemDataset.InventoryMatrixItem(iLoop).MatrixItemCode & "'", Nothing)
                                                                                    NewItemDataset.InventoryMatrixItem(iLoop).Delete()
                                                                                    NewItemDataset.InventoryMatrixItem.AcceptChanges()
                                                                                End If
                                                                            End If
                                                                        Else
                                                                            Exit For ' TJS 23/06/13
                                                                        End If
                                                                    Next
                                                                End If
                                                                ' end of code added TJS 16/06/13
                                                                NewItemFacade.GenerateInventoryStockTotal("admin")
                                                                NewItemFacade.UpdateStockTotal()
                                                                If rowMagentoDetails.ItemCode_DEV000221 = TEMPORARY_DOCUMENTCODE Then ' TJS 23/06/13
                                                                    rowMagentoDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                                                End If
                                                                For iLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Count - 1
                                                                    If m_MagentoImportDataset.InventoryMagentoCategories_DEV000221(iLoop).ItemCode_DEV000221 = TEMPORARY_DOCUMENTCODE Then ' TJS 10/06/12 TJS 15/06/13
                                                                        m_MagentoImportDataset.InventoryMagentoCategories_DEV000221(iLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                                                    End If
                                                                Next
                                                                For iLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Count - 1
                                                                    If m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iLoop).ItemCode_DEV000221 = TEMPORARY_DOCUMENTCODE Then ' TJS 10/06/12 TJS 15/06/13
                                                                        m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iLoop).ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                                                                    End If
                                                                Next

                                                                If strProductType = "grouped" And bFirstPassDone Then
                                                                    ' yes, get product details from Magento
                                                                    ProcessMagentoGroupedProducts(objMagento, XMLNSManMagento, strInstanceID, strHomeCurrency, _
                                                                        strHomeLanguage, ItemsForImport.Rows(iItemLoop), bISItemExists)

                                                                    ' start of code added TJS 13/04/11
                                                                ElseIf (strProductType = "configurable" Or strProductType = "configurable+opt") And bFirstPassDone Then ' TJS 24/03/13
                                                                    strTemp = ""
                                                                    For iLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
                                                                        If strTemp <> "" Then
                                                                            strTemp = strTemp & ", "
                                                                        End If
                                                                        strTemp = strTemp & "'" & NewItemDataset.InventoryMatrixItem(iLoop).MatrixItemCode & "'" ' TJS 02/12/11
                                                                    Next
                                                                    If strTemp <> "" Then ' TJS 20/05/12
                                                                        ' remove item name suffix (lrynec) added to prevent errors when saving matrix items
                                                                        Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItem SET ItemName = SUBSTRING(ItemName, 0, len(ItemName) - " & Len(strMatrixItemTempSuffix) - 1 & "), ItemType = '" & _
                                                                            ITEM_TYPE_MATRIX_ITEM & "', ClassCode = '" & ITEM_DEFAULT_CLASS_MATRIX_GROUP & "', GLClassCode = '" & ITEM_DEFAULT_CLASS_MATRIX_GROUP & _
                                                                            "', UserModified = '" & Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & _
                                                                            "', DateModified = getdate()  WHERE ItemCode IN (" & strTemp & ")", Nothing) ' TJS 09/03/13 TJS 23/06/13 TJS 18/08/13
                                                                        ' CB13 has XMLPackage on InventoryItemWebOption table 
                                                                        Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItemWebOption SET XMLPackage = '" & XML_PACKAGE_MATRIXPRODUCT & "', UserModified = '" & _
                                                                            Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & _
                                                                            "', DateModified = getdate()  WHERE ItemCode IN (" & strTemp & ")", Nothing) ' TJS 18/08/13
                                                                    End If
                                                                    ' end of code added TJS 13/04/11
                                                                    ' start of code added TJS 24/02/12
                                                                ElseIf strProductType = "simple+opt" And Not bFirstPassDone And Not bNoOptionsToAdd Then ' TJS 20/05/13
                                                                    ProcessMagentoSimpleOptionsPt2(strInstanceID, strHomeCurrency, ItemsForImport.Rows(iItemLoop)) ' TJS 03/05/13
                                                                    ' end of code added TJS 24/02/12
                                                                End If

                                                                If Me.UpdateDataSet(New String()() {New String() {m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                                                        "CreateInventoryMagentoDetails_DEV000221", "UpdateInventoryMagentoDetails_DEV000221", "DeleteInventoryMagentoDetails_DEV000221"}, _
                                                                    New String() {m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                                                        "CreateInventoryMagentoCategories_DEV000221", "UpdateInventoryMagentoCategories_DEV000221", "DeleteInventoryMagentoCategories_DEV000221"}, _
                                                                    New String() {m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                                                        "CreateInventoryMagentoTagDetails_DEV000221", "UpdateInventoryMagentoTagDetails_DEV000221", "DeleteInventoryMagentoTagDetails_DEV000221"}, _
                                                                    New String() {m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.TableName, _
                                                                        "CreateSystemManufacturerSourceID_DEV000221", "UpdateSystemManufacturerSourceID_DEV000221", "DeleteSystemManufacturerSourceID_DEV000221"}}, _
                                                                    Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                                                                    If bManufacturererFound Then
                                                                        rowManufacturerItems(0) = NewItemDataset.InventoryItem.Rows(0)
                                                                        NewManufacturerFacade.AssignItem(Nothing, rowManufacturerItems)
                                                                        NewManufacturerFacade.UpdateDataSet(NewManufacturerFacade.CommandSet(), Interprise.Framework.Base.Shared.TransactionType.InventoryManufacturer, "Add Magento Manufacturer Item", False)
                                                                    End If
                                                                    NoOfMagentoProductsProcessed = NoOfMagentoProductsProcessed + 1 ' TJS 16/06/13
                                                                    If strProductType = "simple+opt" Then ' TJS 16/06/13
                                                                        NoOfProductsImported = NoOfProductsImported + 1 + NewItemDataset.InventoryMatrixItem.Count ' TJS 16/06/13
                                                                    Else
                                                                        NoOfProductsImported = NoOfProductsImported + 1
                                                                    End If
                                                                    If strOmittedOptionSKUs <> "" Then ' TJS 03/04/13
                                                                        m_ProcessLog = m_ProcessLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & ", but the following SKU options were omitted as they utilise the eShopCONNECT Split SKU function which requires separate manually created Inventory Items - " & strOmittedOptionSKUs & vbCrLf ' TJS 03/04/13
                                                                    Else
                                                                        m_ProcessLog = m_ProcessLog & "Successfully imported SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & vbCrLf
                                                                    End If
                                                                    If Not bISItemExists Then ' TJS 16/06/13
                                                                        If strProductType = "simple+opt" Then ' TJS 16/06/13
                                                                            m_ProductsAlreadyImported = m_ProductsAlreadyImported + 1 + NewItemDataset.InventoryMatrixItem.Count ' TJS 16/06/13
                                                                        Else
                                                                            m_ProductsAlreadyImported = m_ProductsAlreadyImported + 1
                                                                        End If
                                                                    End If

                                                                Else
                                                                    ' get error details
                                                                    strErrorDetails = ""
                                                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows.Count - 1
                                                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Columns.Count - 1
                                                                            If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.TableName & _
                                                                                    "." & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                                                    m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                                            End If
                                                                        Next
                                                                    Next
                                                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows.Count - 1
                                                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Columns.Count - 1
                                                                            If m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.TableName & _
                                                                                    "." & m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                                                    m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                                            End If
                                                                        Next
                                                                    Next
                                                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows.Count - 1
                                                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Columns.Count - 1
                                                                            If m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.TableName & _
                                                                                    "." & m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                                                    m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                                            End If
                                                                        Next
                                                                    Next
                                                                    For iRowLoop = 0 To m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Rows.Count - 1
                                                                        For iColumnLoop = 0 To m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Columns.Count - 1
                                                                            If m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.TableName & _
                                                                                    "." & m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                                                    m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                                            End If
                                                                        Next
                                                                    Next
                                                                    m_LastError = strErrorDetails ' TJS 02/12/11
                                                                    m_LastErrorMessage = m_LastError ' TJS 14/02/12
                                                                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted - " & strErrorDetails & vbCrLf ' TJS 08/04/11
                                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1

                                                                End If
                                                                strTemp = "UPDATE LerrynImportExportInventoryActionStatus_DEV000221 SET ActionComplete_DEV000221 = 1 "
                                                                strTemp = strTemp & "WHERE ItemCode_DEV000221 = '" & NewItemDataset.InventoryItem(0).ItemCode
                                                                strTemp = strTemp & "' AND SourceCode_DEV000221 = '" & MAGENTO_SOURCE_CODE & "' AND StoreMerchantID_DEV000221 = '" ' TJS 05/07/12
                                                                strTemp = strTemp & MagentoInstanceID & "'"
                                                                Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing)
                                                            Else
                                                                ' get error details
                                                                strErrorDetails = ""
                                                                For iTableLoop = 0 To NewItemDataset.Tables.Count - 1
                                                                    For iRowLoop = 0 To NewItemDataset.Tables(iTableLoop).Rows.Count - 1
                                                                        For iColumnLoop = 0 To NewItemDataset.Tables(iTableLoop).Columns.Count - 1
                                                                            If NewItemDataset.Tables(iTableLoop).Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                                If ItemsForImport.Rows(iItemLoop).RowState = DataRowState.Deleted Then ' FA Added 23/07/13
                                                                                    strErrorDetails = strErrorDetails & NewItemDataset.Tables(iTableLoop).TableName & _
                                                                                        "." & NewItemDataset.Tables(iTableLoop).Columns(iColumnLoop).ColumnName & ", " & _
                                                                                        NewItemDataset.Tables(iTableLoop).Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                                                Else
                                                                                    strErrorDetails = strErrorDetails & NewItemDataset.Tables(iTableLoop).TableName & _
                                                                                        "." & NewItemDataset.Tables(iTableLoop).Columns(iColumnLoop).ColumnName & ", " & _
                                                                                        NewItemDataset.Tables(iTableLoop).Rows(iRowLoop).GetColumnError(iColumnLoop) & _
                                                                                        " - " & NewItemDataset.InventoryMatrixItem(iRowLoop).MatrixItemName & vbCrLf ' FA Added 23/07/13
                                                                                End If
                                                                            End If
                                                                        Next
                                                                    Next
                                                                Next
                                                                m_LastError = strErrorDetails ' TJS 02/12/11
                                                                m_LastErrorMessage = m_LastError ' TJS 14/02/12
                                                                m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted - " & strErrorDetails & vbCrLf ' TJS 08/04/11
                                                                NoOfProductsSkipped = NoOfProductsSkipped + 1

                                                            End If
                                                        Else
                                                            NoOfProductsSkipped = NoOfProductsSkipped + 1 ' TJS 04/11/11
                                                        End If

                                                        NewItemDataset.Clear()
                                                        If NewManufacturerFacade IsNot Nothing Then
                                                            NewManufacturerFacade.Dispose()
                                                            NewManufacturerDataset.Dispose()
                                                        End If
                                                        If NewKitFacade IsNot Nothing Then
                                                            NewKitFacade.Dispose()
                                                            NewKitDataset.Dispose()
                                                        End If
                                                        m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Clear()
                                                        m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Clear()
                                                        m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Clear()
                                                        m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.Clear() ' TJS /FA 26/06/13

                                                    Else
                                                        ' start of code added TJS 09/08/13
                                                        ' check we haven't been logged out by a previous error
                                                        If Not objMagento.LoggedIn Then
                                                            Try
                                                                If objMagento.Login(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                                                                    m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                                                                    m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)))) Then ' TJS 30/09/13
                                                                    Exit For
                                                                Else
                                                                    ' retry last item
                                                                    iLoop = iLoop - 1
                                                                End If

                                                            Catch ex As Exception
                                                                ' end of code added TJS 09/08/13
                                                                m_LastError = "Unable to read Magento Product Details for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " - " & objMagento.LastError ' TJS 02/12/11
                                                                m_LastErrorMessage = "Unable to read Magento Product Details for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " - " & objMagento.LastErrorMessage ' TJS 14/02/12
                                                                m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as unable to read Magento Product Details - " & objMagento.LastError & vbCrLf ' TJS 08/04/11
                                                                NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                            End Try
                                                        End If
                                                    End If

                                                ElseIf strProductType <> "simple" And strProductType <> "simple+opt" And strProductType <> "virtual" And _
                                                    strProductType <> "grouped" And strProductType <> "configurable" And strProductType <> "configurable+opt" And Not bFirstPassDone Then ' TJS 13/04/11 TJS 02/12/11 TJS 24/02/12 TJS 24/03/13 TJS 12/07/13
                                                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " skipped as product type (" & strProductType & ") not yet supported" & vbCrLf ' TJS 08/04/11
                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1
                                                End If

                                            ElseIf CBool(ItemsForImport.Rows(iItemLoop).Item("Import")) And _
                                                CBool(ItemsForImport.Rows(iItemLoop).Item("AlreadyImported")) And _
                                                CBool(ItemsForImport.Rows(iItemLoop).Item("SKUChanged")) And Not bFirstPassDone Then ' TJS 04/11/11 FA/TJS 29/05/13
                                                ' product SKU has change, user has selected import so update SKU in IS provided name doesn't already exist
                                                If String.IsNullOrEmpty(Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") & "' AND ItemCode <> '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'")) Then 'FA/TJS 29/05/13 FA 03/06/13 changed to isnullorempty
                                                    strTemp = "UPDATE InventoryItem SET ItemName = '" & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString.Replace("'", "''") ' TJS 04/11/11 TJS 03/05/13
                                                    strTemp = strTemp & "' WHERE ItemCode = '" & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & "'" ' TJS 04/11/11
                                                    Me.ExecuteNonQuery(CommandType.Text, strTemp, Nothing) ' TJS 04/11/11
                                                Else
                                                    m_LastError = "Unable to update Item Name for SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " (ItemCode " & ItemsForImport.Rows(iItemLoop).Item("ItemCode").ToString & ") as name conflicts with another Item " 'FA/TJS 29/05/13
                                                    m_LastErrorMessage = m_LastError 'FA/TJS 29/05/13
                                                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImport.Rows(iItemLoop).Item("ItemSKU").ToString & " aborted as " & m_LastError & vbCrLf 'FA/TJS 29/05/13
                                                    NoOfProductsSkipped = NoOfProductsSkipped + 1 'FA/TJS 29/05/13
                                                End If
                                            End If

                                            If m_MagentoImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then ' TJS 29/03/11 
                                                m_LastError = strErrorDetails ' TJS 02/12/11
                                                m_LastErrorMessage = m_LastError ' TJS 14/02/12
                                                m_ImportLimitReached = True ' TJS 02/12/11
                                                Exit For
                                            End If
                                        Next
                                        If bFirstPassDone Then
                                            Exit Do
                                        End If
                                        bFirstPassDone = True
                                        If m_MagentoImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then ' TJS 29/03/11 
                                            m_LastError = strErrorDetails ' TJS 29/03/11 TJS 02/12/11
                                            m_LastErrorMessage = m_LastError ' TJS 14/02/12
                                            m_ImportLimitReached = True ' TJS 02/12/11
                                            m_ProcessLog = m_ProcessLog & strErrorDetails & vbCrLf ' TJS 08/04/11
                                            Exit For
                                        End If

                                    Loop
                                    NewItemFacade.Dispose()
                                    NewItemDataset.Dispose()
                                    objMagento.Logout()
                                Else
                                    m_LastError = "Unable to connect to Magento - " & objMagento.LastError & vbCrLf & vbCrLf & "Please check your configuration settings." ' TJS 02/12/11
                                    m_LastErrorMessage = "Unable to connect to Magento - " & objMagento.LastErrorMessage ' TJS 14/02/12
                                    m_ProcessLog = m_ProcessLog & "Unable to connect to Magento - " & objMagento.LastError & " - Please check your configuration settings." & vbCrLf ' TJS 08/04/11
                                End If
                                Exit For
                            Else
                                m_LastError = "Please enter your Magento API connection settings in the eShopCONNECT config." ' TJS 02/12/11
                                m_LastErrorMessage = m_LastError ' TJS 14/02/12
                                m_ProcessLog = m_ProcessLog & "Cannot connect to Magento - Please enter your Magento API connection settings in the eShopCONNECT Magento Connector configuration" & vbCrLf ' TJS 08/04/11 TJS 09/03/13
                            End If
                            Exit For ' TJS 01/04/14
                        End If
                    Next
                    If Not bInstanceMatched Then ' TJS/FA 29/05/13
                        m_LastError = "Magento Instance not found." ' TJS 02/12/11
                        m_LastErrorMessage = m_LastError ' TJS 14/02/12
                        m_ProcessLog = m_ProcessLog & "Cannot connect to Magento" & vbCrLf ' TJS 29/03/11
                    End If
                Else
                    m_LastError = strErrorDetails ' TJS 02/12/11
                    m_LastErrorMessage = m_LastError ' TJS 14/02/12
                    m_ImportLimitReached = True ' TJS 02/12/11
                End If
            End If

        Catch ex As Exception
            m_LastError = "Error whilst processing SKU " & strSKUBeingProcessed & " - " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace ' TJS 02/12/11
            m_LastErrorMessage = "Error whilst processing SKU " & strSKUBeingProcessed & " - " & ex.Message ' TJS 14/02/12
            m_ProcessLog = m_ProcessLog & "Error whilst processing SKU " & strSKUBeingProcessed & " - " & ex.Message ' TJS 13/03/13 TJS 19/09/13

        End Try

    End Sub
#End Region

#Region " ImportCategories "
    Private Sub ImportCategories(ByVal MagentoInstanceID As String, ByRef objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector, ByVal HomeLanguage As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/03/11 | TJS             | 2011.0.04 | Function added
        ' 13/04/11 | TJS             | 2011.0.10 | Modified to use local MagentoCategoryCount variable
        ' 04/05/11 | TJS             | 2011.0.13 | Modified to initialise category variables 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 16/06/12 | TJS             | 2012.1.07 | Modified to detect empty CategoryNames
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to ensure m_MagentoCategories is updated as each record is processed
        ' 30/09/13 | TJS             | 2013.3.00 | Modified to save category mapping details for re-use
        ' 13/11/13 | TJS             | 2013.3.08 | Added missing UpdateDataset for category mapping and corrected setting of category mapping parent ID
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowCategoryMapping As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportCategoryMappingView_DEV000221Row ' TJS 30/09/13
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLCategoryList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strTemp As String, strErrorDetails As String, iCategoryLoop As Integer
        Dim iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer, iLoop As Integer
        Dim MagentoCategoryCount As Integer, bSomeErrors As Boolean, bDuplicateCategory As Boolean ' TJS 13/04/11

        bSomeErrors = False
        m_LastErrorMessage = "" ' TJS 14/02/12

        If objMagento.GetCatalogTree() Then
            MagentoCategoryCount = 0 ' TJS 04/05/11
            ReDim m_MagentoCategories(MagentoCategoryCount) ' TJS 04/05/11
            XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
            XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
            XMLCategoryList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento)
            objMagento.ExtractMagentoCategories(XMLCategoryList, m_MagentoCategories, MagentoCategoryCount, False) ' TJS 13/04/11
            ' if we had any non-active categories, then we need to remove empty records
            If MagentoCategoryCount > 0 Then ' TJS 13/04/11
                ReDim Preserve m_MagentoCategories(MagentoCategoryCount - 1) ' TJS 13/04/11

            Else
                ' didn't find any active categories
                ReDim m_MagentoCategories(0)
            End If

            NewCategoryDataset = New Interprise.Framework.Inventory.DatasetGateway.SystemManager.CategoryDatasetGateway
            NewCategoryFacade = New Interprise.Facade.Inventory.SystemManager.CategoryFacade(NewCategoryDataset)
            NewCategoryFacade.LanguageCode = HomeLanguage

            For iCategoryLoop = 0 To m_MagentoCategories.Length - 1 ' TJS 09/03/13
                If Not String.IsNullOrEmpty(m_MagentoCategories(iCategoryLoop).CategoryName) Then ' TJS 16/06/12
                    ' save Category Name with any spaces as IS Category Code in UPPER CASE to avoid case releted conflicts
                    m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).CategoryName.ToUpper ' TJS 09/03/13
                    ' is category code too long (Remove any spaces) ?
                    If m_MagentoCategories(iCategoryLoop).ISCategoryCode.Replace(" ", "").Length <= 30 Then
                        ' no
                        m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode.Replace(" ", "")
                    Else
                        ' yes, truncate it
                        m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode.Replace(" ", "").Substring(0, 30)
                    End If
                    ' now check for duplicate category codes
                    For iLoop = 0 To m_MagentoCategories.Length - 1
                        If m_MagentoCategories(iLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode And _
                            iLoop <> iCategoryLoop Then ' TJS 09/03/13
                            ' duplicate found - need to get alternative
                            bDuplicateCategory = True
                            Exit For
                        End If
                    Next
                    strTemp = ""
                    Do While bDuplicateCategory
                        If strTemp = "" Then
                            strTemp = "1"
                            ' is category code maximum length ?
                            If m_MagentoCategories(iCategoryLoop).ISCategoryCode.Length = 30 Then
                                m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode.Substring(0, 29) & strTemp
                            Else
                                m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode & strTemp
                            End If

                        Else
                            m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode.Substring(0, m_MagentoCategories(iCategoryLoop).ISCategoryCode.Length - strTemp.Length)
                            strTemp = (CInt(strTemp) + 1).ToString
                            If m_MagentoCategories(iCategoryLoop).ISCategoryCode.Length + strTemp.Length > 30 Then
                                m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode.Substring(0, 30 - strTemp.Length)
                            End If
                            m_MagentoCategories(iCategoryLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode & strTemp

                        End If
                        bDuplicateCategory = False
                        For iLoop = 0 To m_MagentoCategories.Length - 1
                            If m_MagentoCategories(iLoop).ISCategoryCode = m_MagentoCategories(iCategoryLoop).ISCategoryCode And _
                                iLoop <> iCategoryLoop Then ' TJS 09/03/13
                                ' duplicate found - need to get another alternative
                                bDuplicateCategory = True
                                Exit For
                            End If
                        Next
                    Loop

                    NewCategoryFacade.AddCategory(m_MagentoCategories(iCategoryLoop).ISCategoryCode, m_MagentoCategories(iCategoryLoop).CategoryName)
                    ' NOTE because we use an SQL query to get the category xml, boolean fields return 0 and 1 not true and false
                    NewCategoryDataset.SystemCategory(0).ShowInProductBrowser = 1
                    NewCategoryDataset.SystemCategory(0).IsActive = True
                    If m_MagentoCategories(iCategoryLoop).ParentID = 0 Then
                        NewCategoryDataset.SystemCategory(0).ParentCategory = "Default"
                    Else
                        For iLoop = 0 To iCategoryLoop ' TJS 09/03/13
                            If m_MagentoCategories(iLoop).CategoryID = m_MagentoCategories(iCategoryLoop).ParentID Then
                                NewCategoryDataset.SystemCategory(0).ParentCategory = m_MagentoCategories(iLoop).ISCategoryCode
                                Exit For
                            End If
                        Next
                    End If
                    If Not NewCategoryFacade.UpdateDataSet(NewCategoryFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.None, "New Category import", False) Then
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
                        m_LastErrorMessage = m_LastError ' TJS 14/02/12
                        m_ProcessLog = m_ProcessLog & "Unable to import Category " & m_MagentoCategories(iCategoryLoop).CategoryName & " - " & strErrorDetails
                        ' start of code added TJS 30/09/13
                    Else
                        rowCategoryMapping = Me.m_MagentoImportDataset.LerrynImportExportCategoryMappingView_DEV000221.NewLerrynImportExportCategoryMappingView_DEV000221Row
                        rowCategoryMapping.SourceCode_DEV000221 = "MagentoOrder"
                        rowCategoryMapping.InstanceAccountID_DEV000221 = MagentoInstanceID
                        rowCategoryMapping.SourceCategoryID_DEV000221 = m_MagentoCategories(iCategoryLoop).CategoryID.ToString
                        rowCategoryMapping.SourceParentID_DEV000221 = m_MagentoCategories(iCategoryLoop).ParentID.ToString ' TJS 13/11/13
                        rowCategoryMapping.ISCategoryCode_DEV000221 = NewCategoryDataset.SystemCategory(0).CategoryCode
                        Me.m_MagentoImportDataset.LerrynImportExportCategoryMappingView_DEV000221.AddLerrynImportExportCategoryMappingView_DEV000221Row(rowCategoryMapping)
                        ' end of code added TJS 30/09/13
                    End If
                End If
            Next
            Me.UpdateDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynImportExportCategoryMappingView_DEV000221.TableName, _
                "CreateLerrynImportExportCategoryMapping_DEV000221", "UpdateLerrynImportExportCategoryMapping_DEV000221", "DeleteLerrynImportExportCategoryMapping_DEV000221"}}, _
                Interprise.Framework.Base.Shared.TransactionType.None, "Update Category Mapping", False) ' TJS 30/09/13
            If Not bSomeErrors Then
                m_ProcessLog = m_ProcessLog & "Successfully imported Categories from Magento" & vbCrLf
            Else
                m_ProcessLog = m_ProcessLog & "Finished importing Categories from Magento with some errors" & vbCrLf
            End If
        Else
            m_LastError = "Unable to read Magento Category list" ' TJS 02/12/11
            m_LastErrorMessage = m_LastError ' TJS 14/02/12
            m_ProcessLog = m_ProcessLog & "Unable to read Magento Category list" & vbCrLf
        End If

    End Sub
#End Region

#Region " ApplyAttributes "
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
        ' 04/04/11 | TJS             | 2011.0.07 | Function created 
        ' 05/04/11 | TJS             | 2011.0.08 | Modified to cater for IS 4.8 build using conditional compile
        ' 13/04/11 | TJs             | 2011.0.10 | Corrected sql for adding new attributes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for LanguageCode on SystemAttributeValue table in IS 6
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to detect empty Matrix Values
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to check attribute code and description length
        ' 29/03/13 | TJS             | 2013.1.07 | Modified return false id matrix values are empty
        ' 30/04/13 | TJS             | 2013.1.11 | Modified to detect attribute code and name length before importing to SystemAttribute table
        '                                        | and to remove redundant values from existing itemss
        ' 16/06/13 | TJS/FA          | 2013.1.20 | Modified remove any existing unused values to prevent errors when deleting rows
        '                                        | and to keep attribute error and SKU error on the same line in import log, to make more readable
        ' 28/06/13 | FA              | 2013.1.23 | Added .Replace("'", "''") to GetRow statement and ExecuteNonQuery to prevent 'irish' problem
        '                                        | Added size to error message.
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to ignore case when checking for unused attribute values
        ' 02/12/13 | FA              | 2013.4.01 | Added .Replace("'", "''") to GetRow statement and ExecuteNonQuery to prevent rejection from occurring 
        '                                          if AttributeValueDescription contain '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSystemLanguage As String, strIsActive As String(), iLoop As Integer, iValueLoop As Integer, bValueUsed As Boolean ' TJS 30/04/13

        If AttributeCode.Length > 30 Then ' TJS 22/03/13 TJS 30/04/13
            ' yes, skip it
            m_ProcessLog = m_ProcessLog & AttributeName & " attribute code (" & AttributeCode & ") too long (" & AttributeCode.Length & "). Must not be greater than 30." ' TJS 22/03/13 FA 21/06/13 FA 28/06/13
            Return False ' TJS 22/03/13

        ElseIf AttributeCode.Contains("'") Then ' FA 29/06/13
            'yes, skip it as will cause an issue with GenerateInventoryMatrixItem in ProcMagentoSimpleOpt
            m_ProcessLog = m_ProcessLog & AttributeCode & " contains an apostrophe. Please remove as this is an unhandled symbol.  "  ' TJS 22/03/13 FA 21/06/13 29/06/13 Replaced AttributeName with Code, modified err msg
            Return False ' FA 29/06/13

        ElseIf AttributeName.Length > 50 Then ' TJS 30/04/13
            ' yes, skip it
            m_ProcessLog = m_ProcessLog & AttributeName & " attribute name too long (" & AttributeName.Length & "). Must not be greater than 50" ' TJS 30/04/13 FA 21/06/13 FA 28/06/13
            Return False ' TJS 30/04/13

        End If
        ' check if attribute exists in SystemAttribute and create if not
        strIsActive = GetRow(New String() {"IsActive"}, "SystemAttribute", "AttributeCode = '" & AttributeCode.Replace("'", "''") & "'", False) ' FA 28/06/13
        If strIsActive Is Nothing Then
            Me.ExecuteNonQuery(CommandType.Text, "INSERT INTO SystemAttribute (AttributeCode, AttributeDescription, UserCreated, DateCreated, UserModified, DateModified, IsPrinted, PrintCount, IsActive, MLID) VALUES ('" & AttributeCode.Replace("'", "''") & "', '" & AttributeName.Replace("'", "''") & "', 'admin', getdate(), 'admin', getdate(), 0, 0, 1, null)", Nothing) ' TJS 13/04/11 FA 28/06/13
        ElseIf Not CBool(strIsActive(0)) Then
            Me.ExecuteNonQuery(CommandType.Text, "UPDATE SystemAttribute SET IsActive = 1 WHERE AttributeCode = '" & AttributeCode.Replace("'", "''") & "'", Nothing) ' FA 28/06/13
        End If
        ' now apply attribute to Matrix Item
        LoadDataSet(New String()() {New String() {m_MagentoImportDataset.SystemAttribute.TableName, "ReadSystemAttribute", _
          AT_ATTRIBUTE_CODE, AttributeCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 05/04/11
        NewItemFacade.AssignAttribute(Nothing, New System.Data.DataRow() {m_MagentoImportDataset.SystemAttribute(0)}) ' TJS 05/04/11
        strSystemLanguage = GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
        ' now process each value in turn
        'TJS/FA 11/11/11 If matrix group has no items, then don't do the following
        If MatrixValues.Length > 1 OrElse MatrixValues(0) IsNot Nothing Then
            For iLoop = 0 To MatrixValues.Length - 1
                If MatrixValues(iLoop) IsNot Nothing And MatrixValueCodes(iLoop) IsNot Nothing Then ' TJS 10/06/12
                    ' are values too big for DB field ?
                    If MatrixValueCodes(iLoop).Length > 10 Then
                        ' yes, skip it
                        m_ProcessLog = m_ProcessLog & AttributeName & " attribute value (" & MatrixValueCodes(iLoop) & ") too long (" & MatrixValueCodes(iLoop).Length & "). Must not be greater than 10." 'FA 21/06/13 FA 28/06/13
                        Return False

                    ElseIf MatrixValues(iLoop).Length > 50 Then ' TJS 22/03/13
                        ' yes, skip it
                        m_ProcessLog = m_ProcessLog & AttributeName & " attribute value description (" & MatrixValues(iLoop) & ") too long - (" & MatrixValues(iLoop).Length & "). Must not be greater than 50." ' TJS 22/03/13 FA 21/06/13 FA 28/06/13
                        Return False ' TJS 22/03/13
                    End If
                    ' check if attribute value exists in SystemAttributeValue and create if not
                    strIsActive = GetRow(New String() {"IsActive"}, "SystemAttributeValue", "AttributeCode = '" & AttributeCode.Replace("'", "''") & "' AND AttributeValueCode = '" & MatrixValueCodes(iLoop) & "' AND LanguageCode = '" & strSystemLanguage & "'", False) ' TJS 02/12/11 FA 28/06/13
                    If strIsActive Is Nothing OrElse strIsActive(0) = "" Then
                        Me.ExecuteNonQuery(CommandType.Text, "INSERT INTO SystemAttributeValue (AttributeCode, AttributeValueCode, LanguageCode, AttributeValueDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified, MLID) VALUES ('" & AttributeCode.Replace("'", "''") & "', '" & MatrixValueCodes(iLoop) & "', '" & strSystemLanguage & "', '" & MatrixValues(iLoop).Replace("'", "''") & "', 1, 'admin', getdate(), 'admin', getdate(), null)", Nothing) ' TJS 02/12/11 FA 28/06/13 FA 02/12/13
                    ElseIf Not CBool(strIsActive(0)) Then
                        Me.ExecuteNonQuery(CommandType.Text, "UPDATE SystemAttributeValue SET IsActive = 1 WHERE AttributeCode = '" & AttributeCode.Replace("'", "''") & "' AND AttributeValueCode = '" & MatrixValueCodes(iLoop) & "' AND LanguageCode = '" & strSystemLanguage & "'", Nothing) ' TJS 02/12/11 FA 28/06/13
                    End If
                    ' now apply attribute value to Matrix Item
                    LoadDataSet(New String()() {New String() {m_MagentoImportDataset.SystemAttributeValue.TableName, "ReadSystemAttributeValue", _
                        AT_ATTRIBUTE_CODE, AttributeCode, AT_ATTRIBUTE_VALUE_CODE, MatrixValueCodes(iLoop), AT_LANGUAGE_CODE, strSystemLanguage}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 05/04/11 TJS 02/12/11
                    NewItemFacade.AssignAttributeValue(Nothing, New System.Data.DataRow() {m_MagentoImportDataset.SystemAttributeValue(0)}) ' TJS 05/04/11
                End If
            Next
            ' start of code added TJS 30/04/13
            ' now remove any existing unused values - do loop in reverse as removing rows will change count
            For iLoop = NewItemDataset.InventoryAttributeValue.Count - 1 To 0 Step -1 ' TJS 16/06/13
                If NewItemDataset.InventoryAttributeValue(iLoop).RowState <> DataRowState.Deleted Then
                    bValueUsed = False
                    For iValueLoop = 0 To MatrixValues.Length - 1
                        If MatrixValueCodes(iValueLoop).ToUpper = NewItemDataset.InventoryAttributeValue(iLoop).AttributeValueCode.ToUpper And _
                            AttributeCode.ToUpper = NewItemDataset.InventoryAttributeValue(iLoop).AttributeCode.ToUpper Then ' TJS 19/09/13
                            bValueUsed = True
                        End If
                    Next
                    If Not bValueUsed And AttributeCode.ToUpper = NewItemDataset.InventoryAttributeValue(iLoop).AttributeCode.ToUpper Then ' TJS 19/09/13
                        NewItemDataset.InventoryAttributeValue(iLoop).Delete()
                    End If
                End If
            Next
            ' end of code added TJS 30/04/13
            Return True
        Else
            m_ProcessLog = m_ProcessLog & AttributeName & " has no item values. " ' TJS 29/03/13 FA 21/06/13
            Return False ' TJS 29/03/13
        End If

    End Function
#End Region

#Region " GetMagentoAttributeValues "
    Private Sub GetMagentoAttributeValues(ByRef objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/04/11 | TJS             | 2011.0.09 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLAttribute As XDocument, XMLTemp As XDocument, XMLAttributeNode As XElement, XMLValueNode As XElement
        Dim XMLAttributeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLValueList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim iPtr As Integer

        If objMagento.GetAttributeTable() Then
            XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
            XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
            XMLAttributeList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
            ReDim m_AtributeValues(m_MagentoImportRule.GetXMLElementListCount(XMLAttributeList) - 1)
            iPtr = 0
            For Each XMLAttributeNode In XMLAttributeList
                XMLAttribute = XDocument.Parse(XMLAttributeNode.ToString)
                XMLValueList = XMLAttribute.XPathSelectElements("item/item")
                For Each XMLValueNode In XMLValueList
                    XMLTemp = XDocument.Parse(XMLValueNode.ToString)
                    Select Case m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                        Case "value_id"
                            m_AtributeValues(iPtr).ValueID = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                        Case "option_id"
                            m_AtributeValues(iPtr).OptionID = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                        Case "store_id"
                            m_AtributeValues(iPtr).StoreID = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))

                        Case "value"
                            m_AtributeValues(iPtr).Value = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")

                    End Select
                Next
                iPtr += 1
            Next

        Else
            m_LastError = "Unable to read Magento Attribute values" & vbCrLf & vbCrLf & objMagento.LastError ' TJS 02/12/11
            m_LastErrorMessage = "Unable to read Magento Attribute values" & vbCrLf & vbCrLf & objMagento.LastErrorMessage ' TJS 14/02/12
            m_ProcessLog = m_ProcessLog & "Unable to read Magento Attribute values" & vbCrLf & vbCrLf & objMagento.LastError & vbCrLf
        End If

    End Sub

    Private Function GetAttributeValue(ByVal OptionID As Integer) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/04/11 | TJS             | 2011.0.09 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        For iLoop = 0 To m_AtributeValues.Length - 1
            If m_AtributeValues(iLoop).OptionID = OptionID Then
                Return m_AtributeValues(iLoop).Value
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

#Region " ProcessMagentoXMLKeyValues "
    Private Sub ProcessMagentoXMLKeyValues(ByRef objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector, _
        ByVal InstanceID As String, ByVal HomeCurrency As String, ByRef ProductType As String, ByRef ItemsForImportRow As DataRow, _
        ByRef XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), _
        ByRef ProductSuperAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductSuperAttributeType(), _
        ByRef MagentoDetailsRow As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoDetails_DEV000221Row, _
        ByRef ISItemExists As Boolean, ByRef bFirstPassDone As Boolean, ByRef StoreID As Integer, ByRef ManufacturererFound As Boolean, _
        ByRef DataError As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------ 
        ' 03/05/13 | TJS             | 2013.1.12 | Function created from code removed from ImportMagentoProducts
        ' 20/05/13 | TJS             | 2013.1.15 | Modified to rename the matrix Group for simple+opt products so that one matrix item can have the base Magento SKU for no options
        ' 29/05/13 | FA              | 2013.1.19 | Modified matrix exceeds max size error msg to contian field dimensions
        ' 21/06/13 | TJS             | 2013.1.21 | Modified to cater for Import cost options
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API and Magento Attribute Value selection
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to add tax_class_id as a parameter instead of excluding it
        ' 06/02/14 | TJS             | 2013.4.08 | Modified to only call ApplyItemClassTemplate on new items as it resets costs and also Units of Measure
        '                                        | Also added options to control further pricing and cost updates
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowItemKit As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway.InventoryKitRow
        Dim rowMagentoTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row
        Dim rowMagentoCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoCategories_DEV000221Row
        Dim rowPricingDetail As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway.InventoryItemPricingDetailRow ' TJS 03/04/13
        Dim rowBasePricingDetail As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway.InventoryItemPricingDetailRow
        Dim rowManufacturerSource As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemManufacturerSourceID_DEV000221Row
        Dim XMLTemp As XDocument, XMLItemNode As XElement
        Dim XMLValueList As System.Collections.Generic.IEnumerable(Of XElement), XMLValueNode As XElement
        Dim strTemp As String, iLoop As Integer, bPricesChanged As Boolean ' TJS 06/02/14

        For Each XMLItemNode In XMLItemList
            XMLTemp = XDocument.Parse(XMLItemNode.ToString)
            Select Case m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key").ToLower
                Case "product_id"
                    If MagentoDetailsRow.IsMagentoProductID_DEV000221Null OrElse MagentoDetailsRow.MagentoProductID_DEV000221 <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                        MagentoDetailsRow.MagentoProductID_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                    End If

                Case "sku"
                    If ProductType = "simple+opt" Then ' TJS 20/05/13
                        If NewItemDataset.InventoryItem(0).IsItemNameNull OrElse NewItemDataset.InventoryItem(0).ItemName <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") & MagentoSimplePlusOptNameSuffix Then ' TJS 20/05/13
                            NewItemDataset.InventoryItem(0).ItemName = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") & MagentoSimplePlusOptNameSuffix ' TJS 20/05/13
                        End If
                    Else
                        If NewItemDataset.InventoryItem(0).IsItemNameNull OrElse NewItemDataset.InventoryItem(0).ItemName <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                            NewItemDataset.InventoryItem(0).ItemName = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                        End If
                    End If

                Case "name"
                    If NewItemDataset.InventoryItem(0).IsItemDescriptionNull OrElse NewItemDataset.InventoryItem(0).ItemDescription <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                        NewItemDataset.InventoryItem(0).ItemDescription = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                    End If
                    For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
                        If NewItemDataset.InventoryItemDescription(iLoop).IsItemDescriptionNull OrElse NewItemDataset.InventoryItemDescription(iLoop).ItemDescription <> NewItemDataset.InventoryItem(0).ItemDescription Then ' TJS 10/06/12 TJS 05/07/12
                            NewItemDataset.InventoryItemDescription(iLoop).ItemDescription = NewItemDataset.InventoryItem(0).ItemDescription
                        End If
                    Next

                Case "short_description"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").Length > 1000 Then
                        If NewItemDataset.InventoryItemDescription(0).IsExtendedDescriptionNull OrElse NewItemDataset.InventoryItemDescription(0).ExtendedDescription <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").Substring(0, 1000) Then ' TJS 10/06/12
                            NewItemDataset.InventoryItem(0).ExtendedDescription = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").Substring(0, 1000)
                        End If
                        For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
                            If NewItemDataset.InventoryItemDescription(iLoop).IsExtendedDescriptionNull OrElse NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").Substring(0, 1000) Then ' TJS 10/06/12 TJS 05/07/12
                                NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").Substring(0, 1000)
                            End If
                        Next
                    Else
                        If NewItemDataset.InventoryItemDescription(0).IsExtendedDescriptionNull OrElse NewItemDataset.InventoryItemDescription(0).ExtendedDescription <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                            NewItemDataset.InventoryItem(0).ExtendedDescription = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                        End If
                        For iLoop = 0 To NewItemDataset.InventoryItemDescription.Count - 1
                            If NewItemDataset.InventoryItemDescription(iLoop).IsExtendedDescriptionNull OrElse NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12 TJS 05/07/12
                                NewItemDataset.InventoryItemDescription(iLoop).ExtendedDescription = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                            End If
                        Next
                    End If

                Case "description"
                    If MagentoDetailsRow.IsProductDescription_DEV000221Null OrElse MagentoDetailsRow.ProductDescription_DEV000221 <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                        MagentoDetailsRow.ProductDescription_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                    End If

                Case "in_depth"
                    If MagentoDetailsRow.IsProductInDepthDescription_DEV000221Null OrElse MagentoDetailsRow.ProductInDepthDescription_DEV000221 <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                        MagentoDetailsRow.ProductInDepthDescription_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                    End If

                Case "set" ' TJs 25/04/11
                    If MagentoDetailsRow.IsAttributeSetID_DEV000221Null OrElse MagentoDetailsRow.AttributeSetID_DEV000221 <> CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                        MagentoDetailsRow.AttributeSetID_DEV000221 = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) ' TJs 25/04/11
                    End If
                    ' start of code added TJS 10/06/12
                    If objMagento.GetProductAttributeList(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then
                        ExtractMagentoAttributes(objMagento.ReturnedXML, ProductAttributes)
                    End If
                    If objMagento.GetProductAttributes(ItemsForImportRow.Item("SourceItemID").ToString, m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then
                        ExtractMagentoProductSuperAttributes(objMagento.ReturnedXML, ProductSuperAttributes)

                    End If
                    ' end of code added TJS 10/06/12

                Case "type"
                    If ProductType = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").ToLower Or _
                        (ProductType = "simple+opt" And m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").ToLower = "simple") Or _
                        (ProductType = "configurable+opt" And m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").ToLower = "configurable") Then ' TJS 24/02/12 TJS 24/03/13
                        Select Case ProductType
                            Case "simple"
                                ' start of code added TJS 06/02/14
                                If NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP And ISItemExists Then
                                    strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted as existing Inventory Item type ("
                                    strTemp = strTemp & NewItemDataset.InventoryItem(0).ItemType & ") is not compatible with the current Magento type (" & ProductType
                                    strTemp = strTemp & ").  In order to import this product from Magento, you need to mark the existing CB Inventory Item "
                                    strTemp = strTemp & "as Discontinued, amend it's Item Name and then re-run the Import Wizard"
                                    m_LastError = strTemp
                                    m_LastErrorMessage = strTemp
                                    m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                                    DataError = True
                                    Exit For

                                ElseIf Not ISItemExists Then
                                    ' end of code added TJS 06/02/14
                                    If ItemsForImportRow.Item("ISItemType").ToString = "Non-Stock" Then ' TJS 09/03/13
                                        NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_NON_STOCK) ' TJS 09/03/13
                                    Else
                                        NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_STOCK)
                                    End If

                                End If
                                ' import simple products on first pass and skip on second
                                If bFirstPassDone Then
                                    Exit For
                                End If

                                ' start of code added TJS 24/02/12
                            Case "simple+opt"
                                If Not ISItemExists Then ' TJS 06/02/14
                                    If NewItemDataset.InventoryItem(0).IsXMLPackageNull OrElse NewItemDataset.InventoryItem(0).XMLPackage <> XML_PACKAGE_MATRIXPRODUCT Then ' TJS 10/06/12
                                        NewItemDataset.InventoryItem(0).XMLPackage = XML_PACKAGE_MATRIXPRODUCT
                                    End If
                                    If NewItemDataset.InventoryItem(0).IsItemTypeNull OrElse NewItemDataset.InventoryItem(0).ItemType <> ITEM_TYPE_MATRIX_GROUP Then ' TJS 10/06/12
                                        NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP
                                    End If
                                    If NewItemDataset.InventoryItem(0).IsClassCodeNull OrElse NewItemDataset.InventoryItem(0).ClassCode <> ITEM_DEFAULT_CLASS_MATRIX_GROUP Then ' TJS 10/06/12
                                        NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_MATRIX_GROUP)
                                    End If
                                End If
                                If NewItemDataset.InventoryItem(0).ItemName.Length > CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryMatrixGroup' AND ColumnName = 'Prefix'")) Then
                                    strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " (length: " & NewItemDataset.InventoryItem(0).ItemName.Length.ToString & _
                                        ") aborted as base SKU exceed maximum size for a Matrix Group (max length: " & Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryMatrixGroup' AND ColumnName = 'Prefix'").ToString & ")" ' TJS 04/04/11 FA 29/05/13
                                    m_LastError = strTemp
                                    m_LastErrorMessage = strTemp
                                    m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                                    DataError = True
                                    Exit For
                                End If
                                strTemp = NewItemDataset.InventoryItem(0).ItemName.Substring(0, NewItemDataset.InventoryItem(0).ItemName.Length - MagentoSimplePlusOptNameSuffix.Length) ' TJS 20/05/13
                                If NewItemDataset.InventoryMatrixGroup(0).Prefix <> strTemp Then ' TJS 10/06/12 TJS 20/05/13
                                    NewItemDataset.InventoryMatrixGroup(0).Prefix = strTemp ' TJS 20/05/13
                                End If
                                ' import simple products with options on first pass and skip on second
                                If bFirstPassDone Then
                                    Exit For
                                End If
                                ' end of code added TJS 24/02/12

                            Case "virtual" ' TJS 02/12/11
                                ' start of code added TJS 06/02/14
                                If NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP And ISItemExists Then
                                    strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted as existing Inventory Item type ("
                                    strTemp = strTemp & NewItemDataset.InventoryItem(0).ItemType & ") is not compatible with the current Magento type (" & ProductType
                                    strTemp = strTemp & ").  In order to import this product from Magento, you need to mark the existing CB Inventory Item "
                                    strTemp = strTemp & "as Discontinued, amend it's Item Name and then re-run the Import Wizard"
                                    m_LastError = strTemp
                                    m_LastErrorMessage = strTemp
                                    m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                                    DataError = True
                                    Exit For

                                ElseIf Not ISItemExists Then
                                    ' end of code added TJS 06/02/14
                                    NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_NON_STOCK) ' TJS 02/12/11
                                    ' import simple products on first pass and skip on second
                                    If bFirstPassDone Then ' TJS 02/12/11
                                        Exit For ' TJS 02/12/11
                                    End If
                                End If

                            Case "grouped"
                                ' skip group products on first pass and import on second 
                                ' (once the products which comprise the kit have been imported)
                                If Not bFirstPassDone Then
                                    Exit For
                                End If
                                If Not ISItemExists Then ' TJS 10/06/12
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
                                End If
                                ' start of code added TJS 06/02/14
                                If NewItemDataset.InventoryItem(0).ItemType <> ITEM_TYPE_KIT And ISItemExists Then
                                    strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted as existing Inventory Item type ("
                                    strTemp = strTemp & NewItemDataset.InventoryItem(0).ItemType & ") is not compatible with the current Magento type (" & ProductType
                                    strTemp = strTemp & ").  In order to import this product from Magento, you need to mark the existing CB Inventory Item "
                                    strTemp = strTemp & "as Discontinued, amend it's Item Name and then re-run the Import Wizard"
                                    m_LastError = strTemp
                                    m_LastErrorMessage = strTemp
                                    m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                                    DataError = True
                                    Exit For

                                ElseIf Not ISItemExists Then
                                    ' end of code added TJS 06/02/14

                                    If NewItemDataset.InventoryItem(0).IsXMLPackageNull OrElse NewItemDataset.InventoryItem(0).XMLPackage <> XML_PACKAGE_KITPRODUCT Then ' TJS 10/06/12
                                        NewItemDataset.InventoryItem(0).XMLPackage = XML_PACKAGE_KITPRODUCT
                                    End If
                                    If NewItemDataset.InventoryItem(0).IsItemTypeNull OrElse NewItemDataset.InventoryItem(0).ItemType <> ITEM_TYPE_KIT Then ' TJS 10/06/12
                                        NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_KIT
                                    End If
                                    If NewItemDataset.InventoryItem(0).IsClassCodeNull OrElse NewItemDataset.InventoryItem(0).ClassCode <> ITEM_DEFAULT_CLASS_KIT Then ' TJS 10/06/12
                                        NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_KIT) ' TJS 24/02/12
                                    End If
                                End If

                                NewKitDataset = New Interprise.Framework.Inventory.DatasetGateway.ItemKitDatasetGateway
                                NewKitFacade = New Interprise.Facade.Inventory.ItemKitFacade(NewKitDataset)
                                If ISItemExists Then ' TJS 10/06/12
                                    NewKitFacade.LoadKitItem(NewItemDataset.InventoryItem(0).ItemName) ' TJS 10/06/12
                                End If

                                ' start of code added TJS 29/03/11
                            Case "configurable", "configurable+opt" ' TJS 24/03/13
                                If Not ISItemExists Then ' TJS 10/06/12
                                    NewItemDataset.EnforceConstraints = False
                                    NewItemDataset.InventoryMatrixGroup.Clear()
                                    NewItemDataset.InventoryAttribute.Clear()
                                    NewItemDataset.InventoryAttributeValue.Clear()
                                    NewItemDataset.InventoryAssembly.Clear()
                                    NewItemDataset.InventoryKit.Clear()
                                    NewItemDataset.InventoryAssemblyDetailView.Clear()
                                    NewItemDataset.InventoryItemPricingDetail.Clear()
                                    NewItemDataset.EnforceConstraints = True
                                End If
                                ' start of code added TJS 06/02/14
                                If NewItemDataset.InventoryItem(0).ItemType <> ITEM_TYPE_MATRIX_GROUP And ISItemExists Then
                                    strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted as existing Inventory Item type ("
                                    strTemp = strTemp & NewItemDataset.InventoryItem(0).ItemType & ") is not compatible with the current Magento type (" & ProductType
                                    strTemp = strTemp & ").  In order to import this product from Magento, you need to mark the existing CB Inventory Item "
                                    strTemp = strTemp & "as Discontinued, amend it's Item Name and then re-run the Import Wizard"
                                    m_LastError = strTemp
                                    m_LastErrorMessage = strTemp
                                    m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                                    DataError = True
                                    Exit For

                                ElseIf Not ISItemExists Then
                                    ' end of code added TJS 06/02/14
                                    If NewItemDataset.InventoryItem(0).IsXMLPackageNull OrElse NewItemDataset.InventoryItem(0).XMLPackage <> XML_PACKAGE_MATRIXPRODUCT Then ' TJS 10/06/12
                                        NewItemDataset.InventoryItem(0).XMLPackage = XML_PACKAGE_MATRIXPRODUCT
                                    End If
                                    If NewItemDataset.InventoryItem(0).IsItemTypeNull OrElse NewItemDataset.InventoryItem(0).ItemType <> ITEM_TYPE_MATRIX_GROUP Then ' TJS 10/06/12
                                        NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP
                                    End If
                                    If NewItemDataset.InventoryItem(0).IsClassCodeNull OrElse NewItemDataset.InventoryItem(0).ClassCode <> ITEM_DEFAULT_CLASS_MATRIX_GROUP Then ' TJS 10/06/12
                                        NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_MATRIX_GROUP) ' TJS 24/02/12
                                    End If
                                End If

                                If NewItemDataset.InventoryItem(0).ItemName.Length > CInt(Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryMatrixGroup' AND ColumnName = 'Prefix'")) Then ' TJS 04/04/11
                                    strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " (length: " & NewItemDataset.InventoryItem(0).ItemName.Length.ToString & _
                                        ") aborted as base SKU exceed maximum size for a Matrix Group (max length: " & Me.GetField("ColumnLength", "DataDictionaryColumn", "TableName = 'InventoryMatrixGroup' AND ColumnName = 'Prefix'").ToString & ")" ' TJS 04/04/11 FA 29/05/13
                                    m_LastError = strTemp ' TJS 04/04/11 TJS 02/12/11
                                    m_LastErrorMessage = strTemp ' TJS 14/02/12
                                    m_ProcessLog = m_ProcessLog & strTemp & vbCrLf ' TJS 04/04/11
                                    DataError = True ' TJS 04/04/11
                                    Exit For ' TJS 04/04/11
                                End If
                                If NewItemDataset.InventoryMatrixGroup(0).Prefix <> NewItemDataset.InventoryItem(0).ItemName Then ' TJS 10/06/12
                                    NewItemDataset.InventoryMatrixGroup(0).Prefix = NewItemDataset.InventoryItem(0).ItemName
                                End If
                                ' end of code added TJS 29/03/11

                        End Select

                        ' start of code added TJS 24/02/12
                    Else
                        strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted as Source Type has changed since product list downloaded"
                        m_LastError = strTemp
                        m_LastErrorMessage = strTemp
                        m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                        DataError = True
                        Exit For
                    End If
                    ' end of code added TJS 24/02/12

                Case "categories"
                    XMLValueList = XMLTemp.XPathSelectElements("item/value/item")
                    For Each XMLValueNode In XMLValueList
                        rowMagentoCategory = m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221MagentoCategoryID_DEV000221MagentoWebSiteID_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, CInt(XMLValueNode.Value), -1) ' TJS 10/06/12
                        If rowMagentoCategory Is Nothing Then ' TJS 10/06/12
                            rowMagentoCategory = m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                            rowMagentoCategory.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                            rowMagentoCategory.InstanceID_DEV000221 = InstanceID
                            rowMagentoCategory.MagentoCategoryID_DEV000221 = CInt(XMLValueNode.Value)
                            rowMagentoCategory.MagentoWebSiteID_DEV000221 = -1
                            rowMagentoCategory.IsActive_DEV000221 = True ' TJS 20/05/12
                            m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory)
                        Else
                            If Not rowMagentoCategory.IsActive_DEV000221 Then ' TJS 10/06/12
                                rowMagentoCategory.IsActive_DEV000221 = True ' TJS 10/06/12
                            End If
                        End If
                        ' start of code added TJS 30/03/11
                        For iLoop = 0 To m_MagentoCategories.Length - 1
                            If m_MagentoCategories(iLoop).CategoryID = rowMagentoCategory.MagentoCategoryID_DEV000221 And _
                                m_MagentoCategories(iLoop).ISCategoryCode <> "" Then
                                LoadDataSet(New String()() {New String() {m_MagentoImportDataset.SystemCategoryView.TableName, _
                                    "ReadSystemCategoryView_DEV000221", AT_CATEGORY_CODE, m_MagentoCategories(iLoop).ISCategoryCode}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific)
                                NewItemFacade.AssignInventoryCategory(Nothing, m_MagentoImportDataset.SystemCategoryView(0))
                            End If
                        Next
                        ' end of code added TJS 30/03/11
                    Next

                Case "websites"
                    XMLValueList = XMLTemp.XPathSelectElements("item/value/item")
                    For Each XMLValueNode In XMLValueList
                        rowMagentoCategory = m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221MagentoCategoryID_DEV000221MagentoWebSiteID_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, -1, CInt(XMLValueNode.Value)) ' TJS 10/06/12
                        If rowMagentoCategory Is Nothing Then ' TJS 10/06/12
                            rowMagentoCategory = m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                            rowMagentoCategory.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                            rowMagentoCategory.InstanceID_DEV000221 = InstanceID
                            rowMagentoCategory.MagentoCategoryID_DEV000221 = -1
                            rowMagentoCategory.MagentoWebSiteID_DEV000221 = CInt(XMLValueNode.Value)
                            rowMagentoCategory.IsActive_DEV000221 = True ' TJS 20/05/12
                            m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory)
                        Else
                            If Not rowMagentoCategory.IsActive_DEV000221 Then ' TJS 10/06/12
                                rowMagentoCategory.IsActive_DEV000221 = True ' TJS 10/06/12
                            End If
                        End If
                        If StoreID < 0 Then ' TJS 04/04/11
                            StoreID = CInt(XMLValueNode.Value) ' TJS 04/04/11
                        End If
                    Next

                Case "price"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsSellingPrice_DEV000221Null OrElse _
                            MagentoDetailsRow.SellingPrice_DEV000221 <> CDec(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12 TJS 06/02/14
                            MagentoDetailsRow.SellingPrice_DEV000221 = CDec(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                        bPricesChanged = False
                        rowBasePricingDetail = NewItemDataset.InventoryItemPricingDetail.FindByCurrencyCodeItemCode(HomeCurrency, NewItemDataset.InventoryItem(0).ItemCode)
                        If rowBasePricingDetail IsNot Nothing Then ' TJS 13/04/11
                            If m_ImportSellingPriceAsPricingCost Then ' TJS 06/02/14
                                If rowBasePricingDetail.IsPricingCostRateNull OrElse _
                                    rowBasePricingDetail.PricingCostRate <> MagentoDetailsRow.SellingPrice_DEV000221 Then ' TJS 03/04/13 TJS 06/02/14
                                    rowBasePricingDetail.PricingCostRate = MagentoDetailsRow.SellingPrice_DEV000221
                                    bPricesChanged = True ' TJS 06/02/14
                                End If
                                ' PricingCost wasn't set properly for the base currency in 5.6 so set it specifically
                                If rowBasePricingDetail.IsPricingCostNull OrElse _
                                    rowBasePricingDetail.PricingCost <> rowBasePricingDetail.PricingCostRate Then ' TJS 03/04/13 TJS 06/02/14
                                    rowBasePricingDetail.PricingCost = rowBasePricingDetail.PricingCostRate
                                    bPricesChanged = True ' TJS 06/02/14
                                End If
                            End If
                            If Not m_ImportSpecialPriceAsRetail And m_ImportSellingPriceAsRetail Then ' TJS 03/04/13 TJS 06/02/14
                                If rowBasePricingDetail.IsRetailPriceRateNull OrElse _
                                    rowBasePricingDetail.RetailPriceRate <> MagentoDetailsRow.SellingPrice_DEV000221 Then ' TJS 03/04/13 TJS 06/02/14
                                    rowBasePricingDetail.RetailPriceRate = MagentoDetailsRow.SellingPrice_DEV000221
                                    bPricesChanged = True ' TJS 06/02/14
                                End If
                            End If
                            If Not m_ImportSpecialPriceAsWholesale And m_ImportSellingPriceAsWholesale Then ' TJS 03/04/13 TJS 06/02/14
                                If rowBasePricingDetail.IsWholesalePriceRateNull OrElse _
                                    rowBasePricingDetail.WholesalePriceRate <> MagentoDetailsRow.SellingPrice_DEV000221 Then ' TJS 03/04/13 TJS 06/02/14
                                    rowBasePricingDetail.WholesalePriceRate = MagentoDetailsRow.SellingPrice_DEV000221
                                    bPricesChanged = True ' TJS 06/02/14
                                End If
                            End If
                            ' start of code added TJS 06/02/14
                            If m_ImportSellingPriceAsSuggestedRetail Then
                                If rowBasePricingDetail.IsSuggestedRetailPriceRateNull OrElse _
                                    rowBasePricingDetail.SuggestedRetailPriceRate <> MagentoDetailsRow.SellingPrice_DEV000221 Then
                                    rowBasePricingDetail.SuggestedRetailPriceRate = MagentoDetailsRow.SellingPrice_DEV000221
                                End If
                            End If
                            ' end of code added TJS 06/02/14

                        ElseIf ProductType <> "configurable" And ProductType <> "configurable+opt" Then ' TJS 13/04/11 TJS 24/03/13
                            m_LastError = "No price records created" ' TJS 02/12/11
                            m_LastErrorMessage = m_LastError ' TJS 14/02/12
                            m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted as no price records created" & vbCrLf ' TJS 13/04/11

                        End If
                        If bPricesChanged Or Not ISItemExists Then ' TJS 06/02/14
                            NewItemFacade.IsAutoRecalculate = True
                            NewItemFacade.ReComputePricing(NewItemDataset.InventoryItem(0).ItemCode, HomeCurrency, NewItemFacade.IsAutoRecalculate)
                            If m_ImportSellingPriceAsPricingCost Then ' TJS 06/02/14
                                Try
                                    NewItemFacade.RecomputePriceListOnUpdatePricingCost(NewItemDataset.InventoryItem(0).ItemCode, MagentoDetailsRow.SellingPrice_DEV000221)
                                Catch ex As Exception
                                End Try
                            End If
                            NewItemFacade.IsAutoRecalculate = False
                        End If
                    End If

                Case "cost"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" And m_ImportCostAsAverage Then ' TJS 21/06/13
                        NewItemFacade.UpdateAverageCost(CDec(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")))
                    End If
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" And m_ImportCostAsStandard Then ' TJS 21/06/13
                        NewItemFacade.UpdateStandardCost(CDec(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))) ' TJS 21/06/13
                    End If
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" And m_ImportCostAsLast Then ' TJS 21/06/13
                        NewItemFacade.UpdateLastCost(CDec(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))) ' TJS 21/06/13
                    End If
                    ' start of code added TJS 06/02/14
                    If m_ImportCostAsPricingCost Then
                        bPricesChanged = False
                        rowBasePricingDetail = NewItemDataset.InventoryItemPricingDetail.FindByCurrencyCodeItemCode(HomeCurrency, NewItemDataset.InventoryItem(0).ItemCode)
                        If rowBasePricingDetail IsNot Nothing Then
                            If rowBasePricingDetail.IsPricingCostRateNull OrElse _
                                rowBasePricingDetail.PricingCostRate <> MagentoDetailsRow.SellingPrice_DEV000221 Then
                                rowBasePricingDetail.PricingCostRate = MagentoDetailsRow.SellingPrice_DEV000221
                                bPricesChanged = True
                            End If
                            ' PricingCost wasn't set properly for the base currency in 5.6 so set it specifically
                            If rowBasePricingDetail.IsPricingCostNull OrElse _
                                rowBasePricingDetail.PricingCost <> rowBasePricingDetail.PricingCostRate Then
                                rowBasePricingDetail.PricingCost = rowBasePricingDetail.PricingCostRate
                                bPricesChanged = True
                            End If
                            If bPricesChanged Or Not ISItemExists Then
                                NewItemFacade.IsAutoRecalculate = True
                                NewItemFacade.ReComputePricing(NewItemDataset.InventoryItem(0).ItemCode, HomeCurrency, NewItemFacade.IsAutoRecalculate)
                                If m_ImportSellingPriceAsPricingCost Then
                                    Try
                                        NewItemFacade.RecomputePriceListOnUpdatePricingCost(NewItemDataset.InventoryItem(0).ItemCode, MagentoDetailsRow.SellingPrice_DEV000221)
                                    Catch ex As Exception
                                    End Try
                                End If
                                NewItemFacade.IsAutoRecalculate = False
                            End If
                        End If
                    End If
                    ' end of code added TJS 06/02/14

                Case "news_from_date"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsShowAsNewFrom_DEV000221Null OrElse MagentoDetailsRow.ShowAsNewFrom_DEV000221 <> m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                            MagentoDetailsRow.ShowAsNewFrom_DEV000221 = m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                    End If

                Case "news_to_date"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsShowAsNewTo_DEV000221Null OrElse MagentoDetailsRow.ShowAsNewTo_DEV000221 <> m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                            MagentoDetailsRow.ShowAsNewTo_DEV000221 = m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                    End If

                Case "special_from_date"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsShowAsSpecialFrom_DEV000221Null OrElse MagentoDetailsRow.ShowAsSpecialFrom_DEV000221 <> m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                            MagentoDetailsRow.ShowAsSpecialFrom_DEV000221 = m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                    End If

                Case "special_to_date"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsShowAsSpecialTo_DEV000221Null OrElse MagentoDetailsRow.ShowAsSpecialTo_DEV000221 <> m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                            MagentoDetailsRow.ShowAsSpecialTo_DEV000221 = m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                    End If

                Case "special_price"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsSpecialPrice_DEV000221Null OrElse MagentoDetailsRow.SpecialPrice_DEV000221 <> CDec(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                            MagentoDetailsRow.SpecialPrice_DEV000221 = CDec(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                        ' Start of code added TJS 13/03/13
                        If m_ImportSpecialPriceAsWholesale Or m_ImportSpecialPriceAsRetail Then
                            NewItemFacade.IsAutoRecalculate = True
                            ' Start of code replaced TJS 03/04/13
                            For Each rowPricingDetail In NewItemDataset.InventoryItemPricingDetail.Rows
                                If m_ImportSpecialPriceAsRetail Then
                                    If rowPricingDetail.RetailPriceRate <> RoundCurrency(MagentoDetailsRow.SpecialPrice_DEV000221 * Me.GetExchangerate(rowPricingDetail.CurrencyCode), rowPricingDetail.CurrencyCode) Then
                                        rowPricingDetail.RetailPriceRate = RoundCurrency(MagentoDetailsRow.SpecialPrice_DEV000221 * Me.GetExchangerate(rowPricingDetail.CurrencyCode), rowPricingDetail.CurrencyCode)
                                    End If
                                    If rowPricingDetail.RetailPrice <> MagentoDetailsRow.SpecialPrice_DEV000221 Then
                                        rowPricingDetail.RetailPrice = MagentoDetailsRow.SpecialPrice_DEV000221
                                    End If
                                End If
                                If m_ImportSpecialPriceAsWholesale Then
                                    If rowPricingDetail.WholesalePriceRate <> RoundCurrency(MagentoDetailsRow.SpecialPrice_DEV000221 * Me.GetExchangerate(rowPricingDetail.CurrencyCode), rowPricingDetail.CurrencyCode) Then
                                        rowPricingDetail.WholesalePriceRate = RoundCurrency(MagentoDetailsRow.SpecialPrice_DEV000221 * Me.GetExchangerate(rowPricingDetail.CurrencyCode), rowPricingDetail.CurrencyCode)
                                    End If
                                    If rowPricingDetail.WholesalePrice <> MagentoDetailsRow.SpecialPrice_DEV000221 Then
                                        rowPricingDetail.WholesalePrice = MagentoDetailsRow.SpecialPrice_DEV000221
                                    End If
                                End If
                            Next
                            ' End of code replaced TJS 03/04/13
                            NewItemFacade.IsAutoRecalculate = False
                        End If
                        ' End of code added TJS 13/03/13
                    End If

                Case "visibility"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsVisibility_DEV000221Null OrElse MagentoDetailsRow.Visibility_DEV000221 <> CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                            MagentoDetailsRow.Visibility_DEV000221 = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                    Else
                        If MagentoDetailsRow.IsVisibility_DEV000221Null OrElse MagentoDetailsRow.Visibility_DEV000221 <> 0 Then ' TJS 10/06/12
                            MagentoDetailsRow.Visibility_DEV000221 = 0
                        End If
                    End If

                Case "status"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        If MagentoDetailsRow.IsStatus_DEV000221Null OrElse MagentoDetailsRow.Status_DEV000221 <> CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                            MagentoDetailsRow.Status_DEV000221 = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                    Else
                        If MagentoDetailsRow.IsStatus_DEV000221Null OrElse MagentoDetailsRow.Status_DEV000221 <> 0 Then ' TJS 10/06/12
                            MagentoDetailsRow.Status_DEV000221 = 0
                        End If
                    End If

                Case "manufacturer"
                    If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                        strTemp = Me.GetField("ManufacturerCode_DEV000221", "SystemManufacturerSourceID_DEV000221", _
                            "SourceCode_DEV000221 = '" & MAGENTO_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & InstanceID & _
                            "' AND SourceManufacturerCode_DEV000221 = '" & m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") & "'")
                        NewManufacturerDataset = New Interprise.Framework.Inventory.DatasetGateway.SystemManager.ManufacturerDatasetGateway ' TJS 09/03/13
                        NewManufacturerFacade = New Interprise.Facade.Inventory.SystemManager.ManufacturerFacade(NewManufacturerDataset) ' TJS 09/03/13
                        If strTemp = "" Then
                            strTemp = "Magento" & Microsoft.VisualBasic.Right("000000" & m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"), 6)
                            NewManufacturerFacade.AddManufacturer(strTemp)
                            For iLoop = 0 To NewManufacturerDataset.SystemManufacturerDescription.Count - 1
                                NewManufacturerDataset.SystemManufacturerDescription(iLoop).ManufacturerCode = NewManufacturerDataset.SystemManufacturer(0).ManufacturerCode
                                NewManufacturerDataset.SystemManufacturerDescription(iLoop).Description = "Magento " & m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                            Next
                            NewManufacturerFacade.UpdateDataSet(NewManufacturerFacade.CommandSet(), Interprise.Framework.Base.Shared.TransactionType.InventoryManufacturer, "Add Magento Manufacturer", False)
                            rowManufacturerSource = m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.NewSystemManufacturerSourceID_DEV000221Row
                            rowManufacturerSource.ManufacturerCode_DEV000221 = strTemp
                            rowManufacturerSource.SourceCode_DEV000221 = MAGENTO_SOURCE_CODE
                            rowManufacturerSource.AccountOrInstanceID_DEV000221 = InstanceID
                            rowManufacturerSource.SourceManufacturerCode_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                            m_MagentoImportDataset.SystemManufacturerSourceID_DEV000221.AddSystemManufacturerSourceID_DEV000221Row(rowManufacturerSource)
                        Else
                            NewManufacturerFacade.LoadDataSet(New String()() {New String() {NewManufacturerDataset.SystemManufacturer.TableName, _
                                READSYSTEMMANUFACTURER, AT_MANUFACTURERCODE, strTemp}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 09/03/13
                        End If
                        NewItemDataset.InventoryItem(0).ManufacturerCode = strTemp ' TJS 02/12/11
                        ManufacturererFound = True
                    End If

                Case "custom_design_from", "custom_design_to"
                    rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key"), 1) ' TJS 10/06/12
                    If rowMagentoTagDetails Is Nothing Then ' TJS 10/06/12
                        ' some early builds of eShopCONNECT incorrectly set the LineNumer to 0 so check for this
                        rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key"), 0) ' TJS 10/06/12
                    End If
                    If rowMagentoTagDetails Is Nothing Then ' TJS 10/06/12
                        rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                        rowMagentoTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                        rowMagentoTagDetails.InstanceID_DEV000221 = InstanceID
                        rowMagentoTagDetails.LineNumber_DEV000221 = 1 ' TJS 10/06/12
                        rowMagentoTagDetails.TagName_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                        rowMagentoTagDetails.TagDisplayName_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key").Replace("_", " ") ' TJS 25/04/11
                        rowMagentoTagDetails.TagDataType_DEV000221 = "Date"
                        rowMagentoTagDetails.TagRequired_DEV000221 = False ' TJS 25/04/11
                        rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                        If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                            rowMagentoTagDetails.TagDateValue_DEV000221 = m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                        SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                        m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)
                    Else
                        If rowMagentoTagDetails.LineNumber_DEV000221 = 0 Then ' TJS 10/06/12
                            rowMagentoTagDetails.LineNumber_DEV000221 = 1 ' TJS 10/06/12
                        End If
                        If rowMagentoTagDetails.IsTagTextValue_DEV000221Null OrElse rowMagentoTagDetails.TagTextValue_DEV000221 <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                            rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") ' TJS 10/06/12
                        End If
                        If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then ' TJS 10/06/12
                            If rowMagentoTagDetails.IsTagDateValue_DEV000221Null OrElse rowMagentoTagDetails.TagDateValue_DEV000221 <> m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                                rowMagentoTagDetails.TagDateValue_DEV000221 = m_MagentoImportRule.ConvertXMLDate(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) ' TJS 10/06/12
                            End If
                        End If
                        SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                    End If

                Case "color", "status" ' TJS 13/11/13
                    rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key"), 1) ' TJS 10/06/12
                    If rowMagentoTagDetails Is Nothing Then ' TJS 10/06/12
                        ' some early builds of eShopCONNECT incorrectly set the LineNumer to 0 so check for this
                        rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key"), 0) ' TJS 10/06/12
                    End If
                    If rowMagentoTagDetails Is Nothing Then ' TJS 10/06/12
                        rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                        rowMagentoTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                        rowMagentoTagDetails.InstanceID_DEV000221 = InstanceID
                        rowMagentoTagDetails.LineNumber_DEV000221 = 1 ' TJS 10/06/12
                        rowMagentoTagDetails.TagName_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                        rowMagentoTagDetails.TagDisplayName_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key").Replace("_", " ") ' TJS 25/04/11
                        rowMagentoTagDetails.TagDataType_DEV000221 = "Integer"
                        rowMagentoTagDetails.TagRequired_DEV000221 = False ' TJS 25/04/11
                        rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                        If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then
                            rowMagentoTagDetails.TagNumericValue_DEV000221 = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value"))
                        End If
                        SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                        m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)
                    Else
                        If rowMagentoTagDetails.LineNumber_DEV000221 = 0 Then ' TJS 10/06/12
                            rowMagentoTagDetails.LineNumber_DEV000221 = 1 ' TJS 10/06/12
                        End If
                        If rowMagentoTagDetails.IsTagTextValue_DEV000221Null OrElse rowMagentoTagDetails.TagTextValue_DEV000221 <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                            rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") ' TJS 10/06/12
                        End If
                        If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") <> "" Then ' TJS 10/06/12
                            If rowMagentoTagDetails.IsTagNumericValue_DEV000221Null OrElse rowMagentoTagDetails.TagNumericValue_DEV000221 <> CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) Then ' TJS 10/06/12
                                rowMagentoTagDetails.TagNumericValue_DEV000221 = CInt(m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")) ' TJS 10/06/12
                            End If
                        End If
                        SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                    End If

                Case "type_id", "old_id", "category_ids", "created_at", "updated_at", "tier_price" ' TJS 09/12/13
                    ' these properties are not currently used in IS

                Case Else
                    rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key"), 1) ' TJS 10/06/12
                    If rowMagentoTagDetails Is Nothing Then ' TJS 10/06/12
                        ' some early builds of eShopCONNECT incorrectly set the LineNumer to 0 so check for this
                        rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.FindByItemCode_DEV000221InstanceID_DEV000221TagName_DEV000221LineNumber_DEV000221(NewItemDataset.InventoryItem(0).ItemCode, InstanceID, m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key"), 0) ' TJS 10/06/12
                    End If
                    If rowMagentoTagDetails Is Nothing Then ' TJS 10/06/12
                        rowMagentoTagDetails = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                        rowMagentoTagDetails.ItemCode_DEV000221 = NewItemDataset.InventoryItem(0).ItemCode
                        rowMagentoTagDetails.InstanceID_DEV000221 = InstanceID
                        rowMagentoTagDetails.LineNumber_DEV000221 = 1 ' TJS 10/06/12
                        rowMagentoTagDetails.TagName_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                        rowMagentoTagDetails.TagDisplayName_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key").Replace("_", " ") ' TJS 25/04/11
                        If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").Length > 255 Then
                            rowMagentoTagDetails.TagDataType_DEV000221 = "Memo"
                            rowMagentoTagDetails.TagMemoValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                        Else
                            rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                            rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                        End If
                        rowMagentoTagDetails.TagRequired_DEV000221 = False ' TJS 25/04/11
                        SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                        m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)
                    Else
                        If rowMagentoTagDetails.LineNumber_DEV000221 = 0 Then ' TJS 10/06/12
                            rowMagentoTagDetails.LineNumber_DEV000221 = 1 ' TJS 10/06/12
                        End If
                        If m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value").Length > 255 Then ' TJS 10/06/12
                            If rowMagentoTagDetails.TagDataType_DEV000221 <> "Memo" Then ' TJS 10/06/12
                                rowMagentoTagDetails.TagDataType_DEV000221 = "Memo" ' TJS 10/06/12
                            End If
                            If Not rowMagentoTagDetails.IsTagTextValue_DEV000221Null Then ' TJS 10/06/12
                                rowMagentoTagDetails.SetTagTextValue_DEV000221Null() ' TJS 10/06/12
                            End If
                            If rowMagentoTagDetails.IsTagMemoValue_DEV000221Null OrElse rowMagentoTagDetails.TagMemoValue_DEV000221 <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                                rowMagentoTagDetails.TagMemoValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") ' TJS 10/06/12
                            End If
                        Else
                            If rowMagentoTagDetails.TagDataType_DEV000221 <> "Text" Then ' TJS 10/06/12
                                rowMagentoTagDetails.TagDataType_DEV000221 = "Text" ' TJS 10/06/12
                            End If
                            If Not rowMagentoTagDetails.IsTagMemoValue_DEV000221Null Then ' TJS 10/06/12
                                rowMagentoTagDetails.SetTagMemoValue_DEV000221Null() ' TJS 10/06/12
                            End If
                            If rowMagentoTagDetails.IsTagTextValue_DEV000221Null OrElse rowMagentoTagDetails.TagTextValue_DEV000221 <> m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") Then ' TJS 10/06/12
                                rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value") ' TJS 10/06/12
                            End If
                        End If
                        SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                    End If

            End Select

        Next

    End Sub
#End Region

#Region " ProcessMagentoSimpleOptionsPt1 "
    Private Sub ProcessMagentoSimpleOptionsPt1(ByRef objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector, _
        ByRef XMLNSManMagento As System.Xml.XmlNamespaceManager, ByVal SplitItemSKU As String, ByRef ItemsForImportRow As DataRow, _
        ByRef OmittedOptionSKUs As String, ByRef ISItemExists As Boolean, ByRef MatrixAttributeError As Boolean, _
        ByRef NoOptionsToAdd As Boolean) ' TJS 20/05/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/05/13 | TJS             | 2013.1.12 | Function created from code removed from ImportMagentoProducts
        '                                        | and modified to complete the updating of matrix group items 
        '                                        | when additional attributes are added
        ' 20/05/13 | TJS             | 2013.1.15 | Modified to always add a blank option
        ' 23/05/13 | TJS             | 2013.1.16 | Corrected addition of blank option
        ' 29/05/13 | TJS/FA          | 2013.1.19 | Added check for blank SKU suffix
        ' 16/06/13 | TJS             | 2013.1.20 | Modified to only mention omitted SKU options if ItemName not found in db
        '                                        | Added quotes, full stops to log error msgs to make them clearer to read
        ' 03/07/13 | TJS             | 2013.1.24 | Corrected attribute value
        ' 16/07/13 | TJS/FA          | 2013.1.29 | Added check to prevent trying to set Selected field on deleted rows
        ' 23/07/13 | FA              | 2013.1.31 | Corrected call to ApplyAttributes in CAse 4
        ' 09/08/13 | TJs             | 2013.1.32 | Modified to detect if attributes are required and only add empty value if optional
        ' 08/08/12 | FA              | 2013.2.02 | Modified to cater for Matrix Groups with a projected item combinations 
        '                                          which exceeds 20,0000 items.  Reject group and advise use of SplitSKU functionality
        ' 19/09/13 | FA              | 2013.3.00 | Modified to prevent no row at position 0 error and
        '                                        | to delete existing but now unused attribute values
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTemp As XDocument, XMLItemTemp As XDocument ' TJS 13/04/11
        Dim XMLOptionsList As System.Collections.Generic.IEnumerable(Of XElement), XMLOptionNode As XElement ' TJS 24/02/12
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement
        Dim MagentoOptionValues1 As MagentoOptionValues(), MagentoOptionValues2 As MagentoOptionValues() ' TJS 09/03/13
        Dim MagentoOptionValues3 As MagentoOptionValues(), MagentoOptionValues4 As MagentoOptionValues() ' TJS 09/03/13
        Dim MagentoOptionValues5 As MagentoOptionValues(), MagentoOptionValues6 As MagentoOptionValues() ' TJS 09/03/13
        Dim strMatrixValues1 As String(), strMatrixValues2 As String(), strMatrixValues3 As String() ' TJS 04/04/11
        Dim strMatrixValues4 As String(), strMatrixValues5 As String(), strMatrixValues6 As String() ' TJS 04/04/11
        Dim strMatrixAttributes As String(), strMatrixValueCodes As String(), strOptionValueSKU As String 'TJS 04/04/11 TJS 24/02/12
        Dim strTemp As String, strErrorDetails As String, strOptionValueTitle As String, strSeparator As String ' TJS 03/05/13
        Dim strDescription As String, strAttributeCode As String ' TJS 23/05/13
        Dim iMatrixItemLoop As Integer, iAttributePtr As Integer, iOptPtr As Integer, iLoop As Integer ' TJS 24/02/12 TJS 09/03/13
        Dim iAttributeLoop As Integer, iCheckLoop As Integer, decOptionValuePrice As Decimal ' TJS 03/05/13
        Dim bSkipMatrixOption As Boolean, bAttributeFound As Boolean, bRowDeleted As Boolean ' TJS 13/04/11 TJS 29/03/13 TJS 03/05/13
        Dim bDeletedRowWasNew As Boolean, bOptionIsReqd As Boolean, bExistingAttributeDeleted As Boolean ' TJS  03/05/13 TJS 09/08/13 TJS 19/09/13
        Dim lProjectedMatrixCount As Long = 1 ' FA 09/08/13

        ' get custom options from Magento
        If objMagento.GetCatalogProductCustomOptions(ItemsForImportRow.Item("SourceItemID").ToString) Then
            XMLOptionsList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
            ReDim m_MagentoOptions(m_MagentoImportRule.GetXMLElementListCount(XMLOptionsList) - 1)
            ReDim strMatrixAttributes(0)
            ReDim MagentoOptionValues1(0)
            ReDim MagentoOptionValues2(0)
            ReDim MagentoOptionValues3(0)
            ReDim MagentoOptionValues4(0)
            ReDim MagentoOptionValues5(0)
            ReDim MagentoOptionValues6(0)
            iMatrixItemLoop = 0
            For Each XMLOptionNode In XMLOptionsList
                bSkipMatrixOption = False ' TJS 29/03/13
                iOptPtr = -1
                iAttributePtr = 0
                strOptionValueSKU = ""
                strOptionValueTitle = ""
                decOptionValuePrice = 0
                XMLTemp = XDocument.Parse(XMLOptionNode.ToString)
                XMLItemList = XMLTemp.XPathSelectElements("item/item")
                For Each XMLItemNode In XMLItemList
                    XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                    strTemp = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                    Select Case m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key")
                        Case "sku"
                            ' start of code added TJS 29/03/13
                            ' does sku addition for option start with Split SKU characters ?
                            If SplitItemSKU <> "" AndAlso strTemp.Length > SplitItemSKU.Length AndAlso _
                                strTemp.Substring(0, SplitItemSKU.Length) = SplitItemSKU Then
                                ' yes - skip option and add to message
                                bSkipMatrixOption = True
                                If String.IsNullOrEmpty(Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & strTemp.Substring(SplitItemSKU.Length).Replace("'", "''") & "'")) Then ' TJS 16/06/13
                                    If OmittedOptionSKUs.IndexOf(strTemp.Substring(SplitItemSKU.Length)) < 0 Then
                                        If OmittedOptionSKUs <> "" Then
                                            OmittedOptionSKUs = OmittedOptionSKUs & ", "
                                        End If
                                        OmittedOptionSKUs = OmittedOptionSKUs & strTemp.Substring(SplitItemSKU.Length)
                                    End If
                                End If
                                Exit For
                            End If
                            ' end of code added TJS 29/03/13
                            If iOptPtr < 0 Then
                                strOptionValueSKU = strTemp
                            Else
                                Select Case iAttributePtr
                                    Case 1
                                        MagentoOptionValues1(iOptPtr).OptionValueSKU = strTemp
                                    Case 2
                                        MagentoOptionValues2(iOptPtr).OptionValueSKU = strTemp
                                    Case 3
                                        MagentoOptionValues3(iOptPtr).OptionValueSKU = strTemp
                                    Case 4
                                        MagentoOptionValues4(iOptPtr).OptionValueSKU = strTemp
                                    Case 5
                                        MagentoOptionValues5(iOptPtr).OptionValueSKU = strTemp
                                    Case 6
                                        MagentoOptionValues6(iOptPtr).OptionValueSKU = strTemp
                                End Select
                            End If

                        Case "title"
                            If iOptPtr < 0 Then
                                strOptionValueTitle = strTemp
                            Else
                                Select Case iAttributePtr
                                    Case 1
                                        MagentoOptionValues1(iOptPtr).OptionValueTitle = strTemp
                                    Case 2
                                        MagentoOptionValues2(iOptPtr).OptionValueTitle = strTemp
                                    Case 3
                                        MagentoOptionValues3(iOptPtr).OptionValueTitle = strTemp
                                    Case 4
                                        MagentoOptionValues4(iOptPtr).OptionValueTitle = strTemp
                                    Case 5
                                        MagentoOptionValues5(iOptPtr).OptionValueTitle = strTemp
                                    Case 6
                                        MagentoOptionValues6(iOptPtr).OptionValueTitle = strTemp
                                End Select
                            End If

                        Case "attr_title"
                            If CheckAttributeMatch(1, strTemp) Then ' TJS 30/04/13
                                iAttributePtr = 1
                                If m_MagentoOptions(0).Attribute1Code = "" Then
                                    For iLoop = 0 To MagentoOptionValues1.Length - 1
                                        MagentoOptionValues1(iLoop).OptionAttribute = strTemp
                                    Next
                                End If
                                m_MagentoOptions(iMatrixItemLoop).Attribute1Code = strTemp
                                m_MagentoOptions(iMatrixItemLoop).Attribute1Value = strOptionValueSKU
                                m_MagentoOptions(iMatrixItemLoop).Price = decOptionValuePrice
                                bAttributeFound = False
                                For iLoop = 0 To MagentoOptionValues1.Length - 1
                                    If MagentoOptionValues1(iLoop).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute1Code And _
                                        MagentoOptionValues1(iLoop).OptionValueSKU = strOptionValueSKU And strOptionValueSKU <> "" Then
                                        bAttributeFound = True
                                        iOptPtr = iLoop
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If MagentoOptionValues1.Length = 1 And MagentoOptionValues1(0).OptionValueSKU = "" Then
                                        ReDim Preserve MagentoOptionValues1(MagentoOptionValues1.Length - 1)
                                    Else
                                        ReDim Preserve MagentoOptionValues1(MagentoOptionValues1.Length)
                                    End If
                                    iOptPtr = MagentoOptionValues1.Length - 1
                                    MagentoOptionValues1(iOptPtr).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute1Code
                                    MagentoOptionValues1(iOptPtr).OptionValueSKU = strOptionValueSKU
                                    MagentoOptionValues1(iOptPtr).OptionValueTitle = strOptionValueTitle
                                    MagentoOptionValues1(iOptPtr).OptionValuePrice = decOptionValuePrice
                                    MagentoOptionValues1(iOptPtr).OptionIsRequired = bOptionIsReqd ' TJS 09/08/13
                                End If

                            ElseIf CheckAttributeMatch(2, strTemp) Then ' TJS 30/04/13
                                iAttributePtr = 2
                                If m_MagentoOptions(0).Attribute2Code = "" Then
                                    For iLoop = 0 To MagentoOptionValues2.Length - 1
                                        MagentoOptionValues2(iLoop).OptionAttribute = strTemp
                                    Next
                                End If
                                m_MagentoOptions(iMatrixItemLoop).Attribute2Code = strTemp
                                m_MagentoOptions(iMatrixItemLoop).Attribute2Value = strOptionValueSKU
                                m_MagentoOptions(iMatrixItemLoop).Price = decOptionValuePrice
                                bAttributeFound = False
                                For iLoop = 0 To MagentoOptionValues2.Length - 1
                                    If MagentoOptionValues2(iLoop).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute2Code And _
                                        MagentoOptionValues2(iLoop).OptionValueSKU = strOptionValueSKU And strOptionValueSKU <> "" Then
                                        bAttributeFound = True
                                        iOptPtr = iLoop
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If MagentoOptionValues2.Length = 1 And MagentoOptionValues2(0).OptionValueSKU = "" Then
                                        ReDim Preserve MagentoOptionValues2(MagentoOptionValues2.Length - 1)
                                    Else
                                        ReDim Preserve MagentoOptionValues2(MagentoOptionValues2.Length)
                                    End If
                                    iOptPtr = MagentoOptionValues2.Length - 1
                                    MagentoOptionValues2(iOptPtr).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute2Code
                                    MagentoOptionValues2(iOptPtr).OptionValueSKU = strOptionValueSKU
                                    MagentoOptionValues2(iOptPtr).OptionValueTitle = strOptionValueTitle
                                    MagentoOptionValues2(iOptPtr).OptionValuePrice = decOptionValuePrice
                                    MagentoOptionValues2(iOptPtr).OptionIsRequired = bOptionIsReqd ' TJS 09/08/13
                                End If

                            ElseIf CheckAttributeMatch(3, strTemp) Then ' TJS 30/04/13
                                iAttributePtr = 3
                                If m_MagentoOptions(0).Attribute3Code = "" Then
                                    For iLoop = 0 To MagentoOptionValues3.Length - 1
                                        MagentoOptionValues3(iLoop).OptionAttribute = strTemp
                                    Next
                                End If
                                m_MagentoOptions(iMatrixItemLoop).Attribute3Code = strTemp
                                m_MagentoOptions(iMatrixItemLoop).Attribute3Value = strOptionValueSKU
                                m_MagentoOptions(iMatrixItemLoop).Price = decOptionValuePrice
                                bAttributeFound = False
                                For iLoop = 0 To MagentoOptionValues3.Length - 1
                                    If MagentoOptionValues3(iLoop).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute3Code And _
                                        MagentoOptionValues3(iLoop).OptionValueSKU = strOptionValueSKU And strOptionValueSKU <> "" Then
                                        bAttributeFound = True
                                        iOptPtr = iLoop
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If MagentoOptionValues3.Length = 1 And MagentoOptionValues3(0).OptionValueSKU = "" Then
                                        ReDim Preserve MagentoOptionValues3(MagentoOptionValues3.Length - 1)
                                    Else
                                        ReDim Preserve MagentoOptionValues3(MagentoOptionValues3.Length)
                                    End If
                                    iOptPtr = MagentoOptionValues3.Length - 1
                                    MagentoOptionValues3(iOptPtr).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute3Code
                                    MagentoOptionValues3(iOptPtr).OptionValueSKU = strOptionValueSKU
                                    MagentoOptionValues3(iOptPtr).OptionValueTitle = strOptionValueTitle
                                    MagentoOptionValues3(iOptPtr).OptionValuePrice = decOptionValuePrice
                                    MagentoOptionValues3(iOptPtr).OptionIsRequired = bOptionIsReqd ' TJS 09/08/13
                                End If

                            ElseIf CheckAttributeMatch(4, strTemp) Then ' TJS 30/04/13
                                iAttributePtr = 4
                                If m_MagentoOptions(0).Attribute4Code = "" Then
                                    For iLoop = 0 To MagentoOptionValues4.Length - 1
                                        MagentoOptionValues4(iLoop).OptionAttribute = strTemp
                                    Next
                                End If
                                m_MagentoOptions(iMatrixItemLoop).Attribute4Code = strTemp
                                m_MagentoOptions(iMatrixItemLoop).Attribute4Value = strOptionValueSKU
                                m_MagentoOptions(iMatrixItemLoop).Price = decOptionValuePrice
                                bAttributeFound = False
                                For iLoop = 0 To MagentoOptionValues4.Length - 1
                                    If MagentoOptionValues4(iLoop).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute4Code And _
                                        MagentoOptionValues4(iLoop).OptionValueSKU = strOptionValueSKU And strOptionValueSKU <> "" Then
                                        bAttributeFound = True
                                        iOptPtr = iLoop
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If MagentoOptionValues4.Length = 1 And MagentoOptionValues4(0).OptionValueSKU = "" Then
                                        ReDim Preserve MagentoOptionValues4(MagentoOptionValues4.Length - 1)
                                    Else
                                        ReDim Preserve MagentoOptionValues4(MagentoOptionValues4.Length)
                                    End If
                                    iOptPtr = MagentoOptionValues4.Length - 1
                                    MagentoOptionValues4(iOptPtr).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute4Code
                                    MagentoOptionValues4(iOptPtr).OptionValueSKU = strOptionValueSKU
                                    MagentoOptionValues4(iOptPtr).OptionValueTitle = strOptionValueTitle
                                    MagentoOptionValues4(iOptPtr).OptionValuePrice = decOptionValuePrice
                                    MagentoOptionValues4(iOptPtr).OptionIsRequired = bOptionIsReqd ' TJS 09/08/13
                                End If

                            ElseIf CheckAttributeMatch(5, strTemp) Then ' TJS 30/04/13
                                iAttributePtr = 5
                                If m_MagentoOptions(0).Attribute5Code = "" Then
                                    For iLoop = 0 To MagentoOptionValues5.Length - 1
                                        MagentoOptionValues5(iLoop).OptionAttribute = strTemp
                                    Next
                                End If
                                m_MagentoOptions(iMatrixItemLoop).Attribute5Code = strTemp
                                m_MagentoOptions(iMatrixItemLoop).Attribute5Value = strOptionValueSKU
                                m_MagentoOptions(iMatrixItemLoop).Price = decOptionValuePrice
                                bAttributeFound = False
                                For iLoop = 0 To MagentoOptionValues5.Length - 1
                                    If MagentoOptionValues5(iLoop).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute5Code And _
                                        MagentoOptionValues5(iLoop).OptionValueSKU = strOptionValueSKU And strOptionValueSKU <> "" Then
                                        bAttributeFound = True
                                        iOptPtr = iLoop
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If MagentoOptionValues5.Length = 1 And MagentoOptionValues5(0).OptionValueSKU = "" Then
                                        ReDim Preserve MagentoOptionValues5(MagentoOptionValues5.Length - 1)
                                    Else
                                        ReDim Preserve MagentoOptionValues5(MagentoOptionValues5.Length)
                                    End If
                                    iOptPtr = MagentoOptionValues5.Length - 1
                                    MagentoOptionValues5(iOptPtr).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute5Code
                                    MagentoOptionValues5(iOptPtr).OptionValueSKU = strOptionValueSKU
                                    MagentoOptionValues5(iOptPtr).OptionValueTitle = strOptionValueTitle
                                    MagentoOptionValues5(iOptPtr).OptionValuePrice = decOptionValuePrice
                                    MagentoOptionValues5(iOptPtr).OptionIsRequired = bOptionIsReqd ' TJS 09/08/13
                                End If

                            ElseIf CheckAttributeMatch(6, strTemp) Then ' TJS 30/04/13
                                iAttributePtr = 6
                                If m_MagentoOptions(0).Attribute6Code = "" Then
                                    For iLoop = 0 To MagentoOptionValues6.Length - 1
                                        MagentoOptionValues6(iLoop).OptionAttribute = strTemp
                                    Next
                                End If
                                m_MagentoOptions(iMatrixItemLoop).Attribute6Code = strTemp
                                m_MagentoOptions(iMatrixItemLoop).Attribute6Value = strOptionValueSKU
                                m_MagentoOptions(iMatrixItemLoop).Price = decOptionValuePrice
                                bAttributeFound = False
                                For iLoop = 0 To MagentoOptionValues6.Length - 1
                                    If MagentoOptionValues6(iLoop).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute6Code And _
                                        MagentoOptionValues6(iLoop).OptionValueSKU = strOptionValueSKU And strOptionValueSKU <> "" Then
                                        bAttributeFound = True
                                        iOptPtr = iLoop
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If MagentoOptionValues6.Length = 1 And MagentoOptionValues6(0).OptionValueSKU = "" Then
                                        ReDim Preserve MagentoOptionValues6(MagentoOptionValues6.Length - 1)
                                    Else
                                        ReDim Preserve MagentoOptionValues6(MagentoOptionValues6.Length)
                                    End If
                                    iOptPtr = MagentoOptionValues6.Length - 1
                                    MagentoOptionValues6(iOptPtr).OptionAttribute = m_MagentoOptions(iMatrixItemLoop).Attribute6Code
                                    MagentoOptionValues6(iOptPtr).OptionValueSKU = strOptionValueSKU
                                    MagentoOptionValues6(iOptPtr).OptionValueTitle = strOptionValueTitle
                                    MagentoOptionValues6(iOptPtr).OptionValuePrice = decOptionValuePrice
                                    MagentoOptionValues6(iOptPtr).OptionIsRequired = bOptionIsReqd ' TJS 09/08/13
                                End If

                            End If
                            bAttributeFound = False
                            For iLoop = 0 To strMatrixAttributes.Length - 1
                                If strMatrixAttributes(iLoop) = strTemp Then
                                    bAttributeFound = True
                                End If
                            Next
                            If Not bAttributeFound Then
                                If strMatrixAttributes.Length = 1 And strMatrixAttributes(0) = "" Then
                                    ReDim Preserve strMatrixAttributes(strMatrixAttributes.Length - 1)
                                Else
                                    ReDim Preserve strMatrixAttributes(strMatrixAttributes.Length)
                                End If
                                strMatrixAttributes(strMatrixAttributes.Length - 1) = strTemp
                            End If

                        Case "price"
                            If iOptPtr < 0 Then
                                decOptionValuePrice = CDec(strTemp)
                            Else
                                Select Case iAttributePtr
                                    Case 1
                                        MagentoOptionValues1(iOptPtr).OptionValuePrice = CDec(strTemp)
                                    Case 2
                                        MagentoOptionValues2(iOptPtr).OptionValuePrice = CDec(strTemp)
                                    Case 3
                                        MagentoOptionValues3(iOptPtr).OptionValuePrice = CDec(strTemp)
                                    Case 4
                                        MagentoOptionValues4(iOptPtr).OptionValuePrice = CDec(strTemp)
                                    Case 5
                                        MagentoOptionValues5(iOptPtr).OptionValuePrice = CDec(strTemp)
                                    Case 6
                                        MagentoOptionValues6(iOptPtr).OptionValuePrice = CDec(strTemp)
                                End Select
                                m_MagentoOptions(iMatrixItemLoop).Price = CDec(strTemp)
                            End If

                            ' start of code added TJS 09/08/13
                        Case "attr_is_require"
                            If iOptPtr < 0 Then
                                bOptionIsReqd = CBool(strTemp)
                            Else
                                Select Case iAttributePtr
                                    Case 1
                                        MagentoOptionValues1(iOptPtr).OptionIsRequired = CBool(strTemp)
                                    Case 2
                                        MagentoOptionValues2(iOptPtr).OptionIsRequired = CBool(strTemp)
                                    Case 3
                                        MagentoOptionValues3(iOptPtr).OptionIsRequired = CBool(strTemp)
                                    Case 4
                                        MagentoOptionValues4(iOptPtr).OptionIsRequired = CBool(strTemp)
                                    Case 5
                                        MagentoOptionValues5(iOptPtr).OptionIsRequired = CBool(strTemp)
                                    Case 6
                                        MagentoOptionValues6(iOptPtr).OptionIsRequired = CBool(strTemp)
                                End Select
                                m_MagentoOptions(iMatrixItemLoop).Price = CDec(strTemp)
                            End If
                            ' end of code added TJS 09/08/13
                    End Select
                Next
                ' start of code added TJS 29/03/13
                If bSkipMatrixOption Then
                    ' reduce arrray size to cater for omitted option value
                    ReDim Preserve m_MagentoOptions(m_MagentoOptions.Length - 2)
                Else
                    ' end of code added TJS 29/03/13
                    iMatrixItemLoop += 1
                End If
            Next

            ' start of code added TJS 19/09/13
            If ISItemExists Then
                bExistingAttributeDeleted = False
                ' check that existing attributes are still needed
                For iAttributeLoop = NewItemDataset.InventoryAttribute.Count - 1 To 0 Step -1
                    If NewItemDataset.InventoryAttribute(iAttributeLoop).RowState <> DataRowState.Deleted Then
                        bAttributeFound = False
                        For iLoop = 0 To strMatrixAttributes.Length - 1
                            If strMatrixAttributes(iLoop).ToUpper = NewItemDataset.InventoryAttribute(iAttributeLoop).AttributeCode.ToUpper Then
                                ' attribute still used
                                bAttributeFound = True
                                Exit For
                            End If
                        Next
                        If Not bAttributeFound Then
                            ' attribute no longer used, delete related attribute values
                            For iLoop = 0 To NewItemDataset.InventoryAttributeValue.Count - 1
                                If NewItemDataset.InventoryAttributeValue(iLoop).AttributeCode = NewItemDataset.InventoryAttribute(iAttributeLoop).AttributeCode Then
                                    NewItemDataset.InventoryAttributeValue(iLoop).Delete()
                                End If
                            Next
                            For iLoop = 0 To NewItemDataset.InventoryAttribute.Count - 1
                                If NewItemDataset.InventoryAttribute(iLoop).RowState <> DataRowState.Deleted Then
                                    If NewItemDataset.InventoryAttribute(iLoop).PositionID > NewItemDataset.InventoryAttribute(iAttributeLoop).PositionID Then
                                        NewItemDataset.InventoryAttribute(iLoop).PositionID = NewItemDataset.InventoryAttribute(iLoop).PositionID - 1
                                    End If
                                End If
                            Next
                            NewItemDataset.InventoryAttribute(iAttributeLoop).Delete()
                            bExistingAttributeDeleted = True
                        End If
                    End If
                Next
                If bExistingAttributeDeleted Then
                    NewItemFacade.UpdateDataSet(New String()() {New String() {NewItemDataset.InventoryAttribute.TableName, _
                            CREATEINVENTORYATTRIBUTE, UPDATEINVENTORYATTRIBUTE, DELETEINVENTORYATTRIBUTE}, _
                        New String() {NewItemDataset.InventoryAttributeValue.TableName, _
                            CREATEINVENTORYATTRIBUTEVALUE, UPDATEINVENTORYATTRIBUTEVALUE, DELETEINVENTORYATTRIBUTEVALUE}}, _
                        Interprise.Framework.Base.Shared.TransactionType.None, "Update Matrix Group Attributes", False)
                End If
            End If
            ' end of code added TJS 19/09/13

            MatrixAttributeError = False
            For iAttributeLoop = 0 To strMatrixAttributes.Length - 1
                ' check that the attribute is not empty
                If "" & strMatrixAttributes(iAttributeLoop) <> "" Then ' TJS 13/03/13
                    Select Case iAttributeLoop
                        Case 0
                            ReDim strMatrixValues1(MagentoOptionValues1.Length - 1)
                            ReDim strMatrixValueCodes(MagentoOptionValues1.Length - 1)
                            For iLoop = 0 To MagentoOptionValues1.Length - 1
                                If MagentoOptionValues1(iLoop).OptionValueSKU = "" Then ' TJS/FA 29/06/13
                                    m_ProcessLog = m_ProcessLog & "No SKU suffix exists for option attribute " & strMatrixAttributes(iAttributeLoop) & " value " & MagentoOptionValues1(iLoop).OptionValueTitle & ".  " ' TJS/FA 29/06/13
                                    MatrixAttributeError = True ' TJS/FA 29/06/13
                                    Exit For ' TJS/FA 29/06/13 FA 21/06/13
                                End If
                                strMatrixValueCodes(iLoop) = MagentoOptionValues1(iLoop).OptionValueSKU
                                strMatrixValues1(iLoop) = MagentoOptionValues1(iLoop).OptionValueTitle
                            Next
                            If MatrixAttributeError Then Exit For 'FA 18/06/13 two exit Fors will mean the code will hit the SKU error message at the end of the code
                            ' start of code added TJS 30/04/13 and modified TJS 20/05/13
                            For iLoop = 0 To MagentoOptionValues1.Length - 1 ' TJS 09/08/13
                                If Not MagentoOptionValues1(iLoop).OptionIsRequired Then ' TJS 09/08/13
                                    ReDim Preserve strMatrixValues1(MagentoOptionValues1.Length)
                                    ReDim Preserve strMatrixValueCodes(MagentoOptionValues1.Length)
                                    strMatrixValueCodes(MagentoOptionValues1.Length) = "" ' TJS 23/05/13
                                    strMatrixValues1(MagentoOptionValues1.Length) = "" ' TJS 23/05/13
                                    Exit For ' TJS 09/08/13
                                End If
                            Next
                            ' end of code added TJS 30/04/13 and modified TJS 20/05/13
                            If Not ApplyAttributes(strMatrixAttributes(iAttributeLoop), strMatrixAttributes(iAttributeLoop), strMatrixValues1, strMatrixValueCodes) Then
                                MatrixAttributeError = True
                                Exit For
                            End If
                            lProjectedMatrixCount = lProjectedMatrixCount * strMatrixValueCodes.Length ' FA 09/08/13

                        Case 1
                            ReDim strMatrixValues2(MagentoOptionValues2.Length - 1)
                            ReDim strMatrixValueCodes(MagentoOptionValues2.Length - 1)
                            For iLoop = 0 To MagentoOptionValues2.Length - 1
                                If MagentoOptionValues2(iLoop).OptionValueSKU = "" Then ' TJS/FA 29/06/13
                                    m_ProcessLog = m_ProcessLog & "No SKU suffix exists for option attribute " & strMatrixAttributes(iAttributeLoop) & " value " & MagentoOptionValues2(iLoop).OptionValueTitle & ".  " ' TJS/FA 29/06/13
                                    MatrixAttributeError = True ' TJS/FA 29/06/13
                                    Exit For ' TJS/FA 29/06/13 FA 21/06/13
                                End If
                                strMatrixValueCodes(iLoop) = MagentoOptionValues2(iLoop).OptionValueSKU
                                strMatrixValues2(iLoop) = MagentoOptionValues2(iLoop).OptionValueTitle
                            Next
                            If MatrixAttributeError Then Exit For 'FA 18/06/13 two exit Fors will mean the code will hit the SKU error message at the end of the code
                            ' start of code added TJS 30/04/13 and modified TJS 20/05/13
                            For iLoop = 0 To MagentoOptionValues2.Length - 1 ' TJS 09/08/13
                                If Not MagentoOptionValues2(iLoop).OptionIsRequired Then ' TJS 09/08/13
                                    ReDim Preserve strMatrixValues2(MagentoOptionValues2.Length)
                                    ReDim Preserve strMatrixValueCodes(MagentoOptionValues2.Length)
                                    strMatrixValueCodes(MagentoOptionValues2.Length) = "" ' TJS 23/05/13
                                    strMatrixValues2(MagentoOptionValues2.Length) = "" ' TJS 23/05/13
                                    Exit For ' TJS 09/08/13
                                End If
                            Next
                            ' end of code added TJS 30/04/13 and modified TJS 20/05/13
                            If Not ApplyAttributes(strMatrixAttributes(iAttributeLoop), strMatrixAttributes(iAttributeLoop), strMatrixValues2, strMatrixValueCodes) Then
                                MatrixAttributeError = True
                                Exit For
                            End If
                            lProjectedMatrixCount = lProjectedMatrixCount * strMatrixValueCodes.Length ' FA 09/08/13

                        Case 2
                            ReDim strMatrixValues3(MagentoOptionValues3.Length - 1)
                            ReDim strMatrixValueCodes(MagentoOptionValues3.Length - 1)
                            For iLoop = 0 To MagentoOptionValues3.Length - 1
                                If MagentoOptionValues3(iLoop).OptionValueSKU = "" Then ' TJS/FA 29/06/13
                                    m_ProcessLog = m_ProcessLog & "No SKU suffix exists for option attribute " & strMatrixAttributes(iAttributeLoop) & " value " & MagentoOptionValues3(iLoop).OptionValueTitle & ".  " ' TJS/FA 29/06/13
                                    MatrixAttributeError = True ' TJS/FA 29/06/13
                                    Exit For ' TJS/FA 29/06/13 FA 21/06/13
                                End If
                                strMatrixValueCodes(iLoop) = MagentoOptionValues3(iLoop).OptionValueSKU
                                strMatrixValues3(iLoop) = MagentoOptionValues3(iLoop).OptionValueTitle
                            Next
                            If MatrixAttributeError Then Exit For 'FA 18/06/13 two exit Fors will mean the code will hit the SKU error message at the end of the code
                            ' start of code added TJS 30/04/13 and modified TJS 20/05/13
                            For iLoop = 0 To MagentoOptionValues3.Length - 1 ' TJS 09/08/13
                                If Not MagentoOptionValues3(iLoop).OptionIsRequired Then ' TJS 09/08/13
                                    ReDim Preserve strMatrixValues3(MagentoOptionValues3.Length)
                                    ReDim Preserve strMatrixValueCodes(MagentoOptionValues3.Length)
                                    strMatrixValueCodes(MagentoOptionValues3.Length) = "" ' TJS 23/05/13
                                    strMatrixValues3(MagentoOptionValues3.Length) = "" ' TJS 23/05/13
                                    Exit For ' TJS 09/08/13
                                End If
                            Next
                            ' end of code added TJS 30/04/13 and modified TJS 20/05/13
                            If Not ApplyAttributes(strMatrixAttributes(iAttributeLoop), strMatrixAttributes(iAttributeLoop), strMatrixValues3, strMatrixValueCodes) Then
                                MatrixAttributeError = True
                                Exit For
                            End If
                            lProjectedMatrixCount = lProjectedMatrixCount * strMatrixValueCodes.Length ' FA 09/08/13

                        Case 3
                            ReDim strMatrixValues4(MagentoOptionValues4.Length - 1)
                            ReDim strMatrixValueCodes(MagentoOptionValues4.Length - 1)
                            For iLoop = 0 To MagentoOptionValues4.Length - 1
                                If MagentoOptionValues4(iLoop).OptionValueSKU = "" Then ' TJS/FA 29/06/13
                                    m_ProcessLog = m_ProcessLog & "No SKU suffix exists for option attribute " & strMatrixAttributes(iAttributeLoop) & " value " & MagentoOptionValues4(iLoop).OptionValueTitle & ".  " ' TJS/FA 29/06/13
                                    MatrixAttributeError = True ' TJS/FA 29/06/13
                                    Exit For ' TJS/FA 29/06/13 FA 21/06/13
                                End If
                                strMatrixValueCodes(iLoop) = MagentoOptionValues4(iLoop).OptionValueSKU
                                strMatrixValues4(iLoop) = MagentoOptionValues4(iLoop).OptionValueTitle
                            Next
                            If MatrixAttributeError Then Exit For 'FA 18/06/13 two exit Fors will mean the code will hit the SKU error message at the end of the code
                            ' start of code added TJS 30/04/13 and modified TJS 20/05/13
                            For iLoop = 0 To MagentoOptionValues4.Length - 1 ' TJS 09/08/13
                                If Not MagentoOptionValues4(iLoop).OptionIsRequired Then ' TJS 09/08/13
                                    ReDim Preserve strMatrixValues4(MagentoOptionValues4.Length)
                                    ReDim Preserve strMatrixValueCodes(MagentoOptionValues4.Length)
                                    strMatrixValueCodes(MagentoOptionValues4.Length) = "" ' TJS 23/05/13
                                    strMatrixValues4(MagentoOptionValues4.Length) = "" ' TJS 23/05/13
                                    Exit For ' TJS 09/08/13
                                End If
                            Next
                            ' end of code added TJS 30/04/13 and modified TJS 20/05/13
                            If Not ApplyAttributes(strMatrixAttributes(iAttributeLoop), strMatrixAttributes(iAttributeLoop), strMatrixValues4, strMatrixValueCodes) Then
                                MatrixAttributeError = True
                                Exit For
                            End If
                            lProjectedMatrixCount = lProjectedMatrixCount * strMatrixValueCodes.Length ' FA 09/08/13

                        Case 4
                            ReDim strMatrixValues5(MagentoOptionValues5.Length - 1)
                            ReDim strMatrixValueCodes(MagentoOptionValues5.Length - 1)
                            For iLoop = 0 To MagentoOptionValues5.Length - 1
                                If MagentoOptionValues5(iLoop).OptionValueSKU = "" Then ' TJS/FA 29/06/13
                                    m_ProcessLog = m_ProcessLog & "No SKU suffix exists for option attribute " & strMatrixAttributes(iAttributeLoop) & " value " & MagentoOptionValues5(iLoop).OptionValueTitle & ".  " ' TJS/FA 29/06/13
                                    MatrixAttributeError = True ' TJS/FA 29/06/13
                                    Exit For ' TJS/FA 29/06/13 FA 21/06/13
                                End If
                                strMatrixValueCodes(iLoop) = MagentoOptionValues5(iLoop).OptionValueSKU
                                strMatrixValues5(iLoop) = MagentoOptionValues5(iLoop).OptionValueTitle
                            Next
                            If MatrixAttributeError Then Exit For 'FA 18/06/13 two exit Fors will mean the code will hit the SKU error message at the end of the code
                            ' start of code added TJS 30/04/13 and modified TJS 20/05/13
                            For iLoop = 0 To MagentoOptionValues5.Length - 1 ' TJS 09/08/13
                                If Not MagentoOptionValues5(iLoop).OptionIsRequired Then ' TJS 09/08/13
                                    ReDim Preserve strMatrixValues5(MagentoOptionValues5.Length)
                                    ReDim Preserve strMatrixValueCodes(MagentoOptionValues5.Length)
                                    strMatrixValueCodes(MagentoOptionValues5.Length) = "" ' TJS 23/05/13
                                    strMatrixValues5(MagentoOptionValues5.Length) = "" ' TJS 23/05/13
                                    Exit For ' TJS 09/08/13
                                End If
                            Next
                            ' end of code added TJS 30/04/13 and modified TJS 20/05/13
                            If Not ApplyAttributes(strMatrixAttributes(iAttributeLoop), strMatrixAttributes(iAttributeLoop), strMatrixValues5, strMatrixValueCodes) Then ' TJS 23/07/13
                                MatrixAttributeError = True
                                Exit For
                            End If
                            lProjectedMatrixCount = lProjectedMatrixCount * strMatrixValueCodes.Length ' FA 09/08/13

                        Case 5
                            ReDim strMatrixValues6(MagentoOptionValues6.Length - 1)
                            ReDim strMatrixValueCodes(MagentoOptionValues6.Length - 1)
                            For iLoop = 0 To MagentoOptionValues6.Length - 1
                                If MagentoOptionValues6(iLoop).OptionValueSKU = "" Then ' TJS/FA 29/06/13
                                    m_ProcessLog = m_ProcessLog & "No SKU suffix exists for option attribute " & strMatrixAttributes(iAttributeLoop) & " value " & MagentoOptionValues6(iLoop).OptionValueTitle & ".  " ' TJS/FA 29/06/13
                                    MatrixAttributeError = True ' TJS/FA 29/06/13
                                    Exit For ' TJS/FA 29/06/13 FA 21/06/13
                                End If
                                strMatrixValueCodes(iLoop) = MagentoOptionValues6(iLoop).OptionValueSKU
                                strMatrixValues6(iLoop) = MagentoOptionValues6(iLoop).OptionValueTitle
                            Next
                            If MatrixAttributeError Then Exit For 'FA 18/06/13 two exit Fors will mean the code will hit the SKU error message at the end of the code
                            ' start of code added TJS 30/04/13 and modified TJS 20/05/13
                            For iLoop = 0 To MagentoOptionValues6.Length - 1 ' TJS 09/08/13
                                If Not MagentoOptionValues6(iLoop).OptionIsRequired Then ' TJS 09/08/13
                                    ReDim Preserve strMatrixValues6(MagentoOptionValues6.Length)
                                    ReDim Preserve strMatrixValueCodes(MagentoOptionValues6.Length)
                                    strMatrixValueCodes(MagentoOptionValues6.Length) = "" ' TJS 23/05/13
                                    strMatrixValues6(MagentoOptionValues6.Length) = "" ' TJS 23/05/13
                                    Exit For ' TJS 09/08/13
                                End If
                            Next
                            ' end of code added TJS 30/04/13 and modified TJS 20/05/13
                            If Not ApplyAttributes(strMatrixAttributes(iAttributeLoop), strMatrixAttributes(iAttributeLoop), strMatrixValues6, strMatrixValueCodes) Then
                                MatrixAttributeError = True
                                Exit For
                            End If
                            lProjectedMatrixCount = lProjectedMatrixCount * strMatrixValueCodes.Length ' FA 09/08/13
                    End Select

                    ' start of code added TJS 20/05/13
                ElseIf iAttributeLoop = 0 Then
                    ' start of code added TJS 06/02/14
                    If ISItemExists Then
                        strTemp = "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted as existing Inventory Item type ("
                        strTemp = strTemp & NewItemDataset.InventoryItem(0).ItemType & ") is not compatible with the current Magento type (simple"
                        strTemp = strTemp & ").  In order to import this product from Magento, you need to mark the existing CB Inventory Item "
                        strTemp = strTemp & "as Discontinued, amend it's Item Name and then re-run the Import Wizard"
                        m_LastError = strTemp
                        m_LastErrorMessage = strTemp
                        m_ProcessLog = m_ProcessLog & strTemp & vbCrLf
                        MatrixAttributeError = True
                        Exit For

                    Else
                        ' end of code added TJS 06/02/14
                        NewItemFacade.ApplyItemClassTemplate(ITEM_DEFAULT_CLASS_STOCK)
                        NewItemDataset.InventoryItem(0).XMLPackage = XML_PACKAGE_SIMPLEPRODUCT
                        NewItemDataset.InventoryItem(0).ItemType = ITEM_TYPE_STOCK
                        If NewItemDataset.InventoryMatrixGroup.Count > 0 Then
                            NewItemDataset.InventoryMatrixGroup(0).Delete()
                        End If
                        For iLoop = NewItemDataset.InventoryMatrixItem.Count - 1 To 0 Step -1
                            NewItemDataset.InventoryMatrixItem(iLoop).Delete()
                        Next
                        NewItemDataset.InventoryItem(0).ItemName = NewItemDataset.InventoryItem(0).ItemName.Substring(0, NewItemDataset.InventoryItem(0).ItemName.Length - MagentoSimplePlusOptNameSuffix.Length) ' TJS 20/05/13
                        ItemsForImportRow.Item("SourceType") = "simple" ' TJS 20/05/13
                        NoOptionsToAdd = True
                    End If
                    ' end of code added TJS 20/05/13

                Else
                    MatrixAttributeError = True ' TJS 13/03/13
                    m_ProcessLog = m_ProcessLog & "No attributes found for SKU " & ItemsForImportRow.Item("ItemSKU").ToString & vbCrLf ' TJS 13/03/13
                End If
            Next
            ' start of coded added FA 09/08/13 
            'If the projected number of items exceeds 20000, then reject as MatrixItems will take too long to process.
            If lProjectedMatrixCount > 20000 Then
                MatrixAttributeError = True
                m_ProcessLog = m_ProcessLog & "Projected number of items (" & lProjectedMatrixCount & ") in Matrix Group " & ItemsForImportRow.Item("ItemName").ToString & " (Item SKU: " & ItemsForImportRow.Item("ItemSKU").ToString & ") " & _
                    "exceeds amount processable by Interprise.  Please use 'SplitSKUSeparatorCharacters' option in the eShopCONNECT configuration and modify Magento items accordingly. "
            End If
            ' end of coded added FA 09/08/13 
            If Not MatrixAttributeError And Not NoOptionsToAdd Then ' TJS 20/05/13
                ' create Matrix Item records
                strErrorDetails = ""
                NewItemFacade.GenerateInventoryMatrixItem(strErrorDetails)
                If strErrorDetails = "" Then
                    NewItemFacade.CreateMatrixItems()
                    ' start of code added TJS 30/04/13
                    ' are we updating product ?
                    If ISItemExists Then
                        ' yes, need to ensure existing items get all attributes set as IS may not do it
                        For iMatrixItemLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode1Null OrElse _
                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode1 <> strMatrixAttributes(0) Then
                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode1 = strMatrixAttributes(0)
                            End If
                            If strMatrixAttributes.Length > 1 Then
                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode2Null OrElse _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode2 <> strMatrixAttributes(1) Then
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode2 = strMatrixAttributes(1)
                                End If
                            End If
                            If strMatrixAttributes.Length > 2 Then
                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode3Null OrElse _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode3 <> strMatrixAttributes(2) Then
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode3 = strMatrixAttributes(2)
                                End If
                            End If
                            If strMatrixAttributes.Length > 3 Then
                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode4Null OrElse _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode4 <> strMatrixAttributes(3) Then
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode4 = strMatrixAttributes(3)
                                End If
                            End If
                            If strMatrixAttributes.Length > 4 Then
                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode5Null OrElse _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode5 <> strMatrixAttributes(4) Then
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode5 = strMatrixAttributes(4)
                                End If
                            End If
                            If strMatrixAttributes.Length > 5 Then
                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode6Null OrElse _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode6 <> strMatrixAttributes(5) Then
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode6 = strMatrixAttributes(5)
                                End If
                            End If
                        Next
                    End If

                    ' start of code added TJS 03/05/13
                    '  now need to remove separator(s) from itemname when attribute code is empty
                    strSeparator = NewItemDataset.InventoryMatrixGroup(0).Separator
                    iMatrixItemLoop = 0
                    ' becasue this loop deletes rows, the row count can change within the loop
                    ' so a do while loop is used to accomodate this
                    Do While iMatrixItemLoop <= NewItemDataset.InventoryMatrixItem.Count - 1 ' FA 19/09/13 
                        ' Changed while statement to beginning
                        ' to cater for a count of 0.  Otherwise 'no row at position 0' error.
                        strTemp = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName
                        ' does name end with separator character ?
                        Do While Right(strTemp, 1) = strSeparator
                            ' yes, remove separator from name
                            strTemp = Left(strTemp, Len(strTemp) - 1)
                        Loop
                        ' now replace any double separator characters with a single one
                        strTemp = strTemp.Replace(strSeparator & strSeparator, strSeparator)
                        ' start of code added TJS 23/05/13
                        strDescription = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemDescription
                        ' is Attribute 6 used ?
                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode6Null Then
                            ' yes - does description end with Attribute 6 Code and separator
                            strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode6 & strSeparator
                            If Right(strDescription, Len(strAttributeCode)) = strAttributeCode Then
                                ' yes, remove separator from description
                                strDescription = Left(strDescription, Len(strDescription) - Len(strAttributeCode))
                            End If
                            ' now replace attribute code 5 followed by separator and comma
                            strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode5 & strSeparator & ","
                            strDescription = strDescription.Replace(strAttributeCode, "")
                        End If
                        ' is Attribute 5 used ?
                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode5Null Then
                            ' yes, is Attribute 6 empty ?
                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode6Null Then
                                ' yes - does description end with Attribute 5 Code and separator
                                strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode5 & strSeparator
                                If Right(strDescription, Len(strAttributeCode)) = strAttributeCode Then
                                    ' yes, remove separator from description
                                    strDescription = Left(strDescription, Len(strDescription) - Len(strAttributeCode))
                                End If
                            End If
                            ' now replace attribute code 4 followed by separator and comma
                            strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode4 & strSeparator & ","
                            strDescription = strDescription.Replace(strAttributeCode, "")
                        End If
                        ' is Attribute 4 used ?
                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode4Null Then
                            ' is Attribute 5 empty ?
                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode5Null Then
                                ' yes - does description end with Attribute 4 Code and separator
                                strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode4 & strSeparator
                                If Right(strDescription, Len(strAttributeCode)) = strAttributeCode Then
                                    ' yes, remove separator from description
                                    strDescription = Left(strDescription, Len(strDescription) - Len(strAttributeCode))
                                End If
                            End If
                            ' now replace attribute code 3 followed by separator and comma
                            strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode3 & strSeparator & ","
                            strDescription = strDescription.Replace(strAttributeCode, "")
                        End If
                        ' is Attribute 3 used ?
                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode3Null Then
                            ' is Attribute 4 empty ?
                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode4Null Then
                                ' yes - does description end with Attribute 3 Code and separator
                                strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode3 & strSeparator
                                If Right(strDescription, Len(strAttributeCode)) = strAttributeCode Then
                                    ' yes, remove separator from description
                                    strDescription = Left(strDescription, Len(strDescription) - Len(strAttributeCode))
                                End If
                            End If
                            ' now replace attribute code 2 followed by separator and comma
                            strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode2 & strSeparator & ","
                            strDescription = strDescription.Replace(strAttributeCode, "")
                        End If
                        ' is Attribute 2 used ?
                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode2Null Then
                            ' is Attribute 3 empty ?
                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode3Null Then
                                ' yes - does description end with Attribute 2 Code and separator
                                strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode2 & strSeparator
                                If Right(strDescription, Len(strAttributeCode)) = strAttributeCode Then
                                    ' yes, remove separator from description
                                    strDescription = Left(strDescription, Len(strDescription) - Len(strAttributeCode))
                                End If
                            End If
                            ' now replace attribute code 1 followed by separator and comma
                            strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode1 & strSeparator & ","
                            strDescription = strDescription.Replace(strAttributeCode, "")
                        End If
                        ' is Attribute 1 used ?
                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode1Null Then
                            ' is Attribute 2 empty ?
                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode2Null Then
                                ' yes - does description end with Attribute 1 Code and separator
                                strAttributeCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode1 & strSeparator
                                If Right(strDescription, Len(strAttributeCode)) = strAttributeCode Then
                                    ' yes, remove separator from description
                                    strDescription = Left(strDescription, Len(strDescription) - Len(strAttributeCode))
                                End If
                            End If
                        End If
                        ' end of code added TJS 23/05/13

                        ' now check is we already have a record with this name
                        bRowDeleted = False
                        bDeletedRowWasNew = False
                        For iCheckLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
                            ' ignore record we are modifing and any deleted ones
                            If iCheckLoop <> iMatrixItemLoop And NewItemDataset.InventoryMatrixItem(iCheckLoop).RowState <> DataRowState.Deleted Then
                                If strTemp = NewItemDataset.InventoryMatrixItem(iCheckLoop).MatrixItemName Then
                                    If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemDescription <> strDescription Then ' TJS 23/05/13
                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemDescription = strDescription ' TJS 23/05/13
                                    End If
                                    ' yes, make sure attributes are correct on other row
                                    If strMatrixAttributes.Length > 5 Then
                                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute6Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute6Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute6 <> _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute6) Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute6 = _
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute6
                                        ElseIf NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute6Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute6Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute6 <> "") Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute6 = ""
                                        End If
                                    End If
                                    If strMatrixAttributes.Length > 4 Then
                                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute5Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute5Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute5 <> _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute5) Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute5 = _
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute5
                                        ElseIf NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute5Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute5Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute5 <> "") Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute5 = ""
                                        End If
                                    End If
                                    If strMatrixAttributes.Length > 3 Then
                                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute4Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute4Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute4 <> _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute4) Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute4 = _
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute4
                                        ElseIf NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute4Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute4Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute4 <> "") Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute4 = ""
                                        End If
                                    End If
                                    If strMatrixAttributes.Length > 2 Then
                                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute3Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute3Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute3 <> _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute3) Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute3 = _
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute3
                                        ElseIf NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute3Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute3Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute3 <> "") Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute3 = ""
                                        End If
                                    End If
                                    If strMatrixAttributes.Length > 1 Then
                                        If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute2Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute2Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute2 <> _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute2) Then
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute2 = _
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute2
                                        ElseIf NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute2Null AndAlso _
                                            (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute2Null OrElse _
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute2 <> "") Then ' TJS 03/07/13
                                            NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute2 = "" ' TJS 03/07/13
                                        End If
                                    End If
                                    If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute1Null AndAlso _
                                        (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute1Null OrElse _
                                        NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute1 <> _
                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute1) Then
                                        NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute1 = _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute1
                                    ElseIf NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute1Null AndAlso _
                                        (NewItemDataset.InventoryMatrixItem(iCheckLoop).IsAttribute1Null OrElse _
                                        NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute1 <> "") Then
                                        NewItemDataset.InventoryMatrixItem(iCheckLoop).Attribute1 = ""
                                    End If
                                    ' and delete new row, but before we do, check if row is a new one
                                    If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).RowState = DataRowState.Added Then
                                        ' yes - set flag to indicate row deleted was new
                                        bDeletedRowWasNew = True
                                    End If
                                    ' now delete it
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Delete()
                                    bRowDeleted = True
                                    ' did we delete a row that was a new one
                                    If bDeletedRowWasNew Then
                                        ' yes, remove 1 from loop count as deleting a new row actually removes it immediately
                                        ' effectively changing the row pointer so the next row has the same row number as the deleted row
                                        iMatrixItemLoop = iMatrixItemLoop - 1
                                    End If
                                    Exit For
                                End If
                            End If
                        Next
                        If Not bRowDeleted AndAlso NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName <> strTemp Then ' TJS 23/05/13
                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName = strTemp
                        End If
                        If Not bRowDeleted AndAlso NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemDescription <> strDescription Then ' TJS 23/05/13
                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemDescription = strDescription ' TJS 23/05/13
                        End If
                        If Not bRowDeleted AndAlso Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Selected Then ' TJS/FA 16/07/13
                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Selected = True
                        End If
                        iMatrixItemLoop = iMatrixItemLoop + 1
                        'Loop While iMatrixItemLoop <= NewItemDataset.InventoryMatrixItem.Count - 1
                    Loop ' FA 19/09/13 moved while condition to beginning of Do
                    ' end of code added TJS 03/05/13
                    ' end of code added TJS 30/04/13
                Else
                    m_LastError = strErrorDetails
                    m_LastErrorMessage = m_LastError
                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted as unable to generate Matrix Item records - " & strErrorDetails & vbCrLf
                End If

            ElseIf Not NoOptionsToAdd Then ' TJS 20/05/13
                m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted." & vbCrLf
            End If

        End If
        ' end of code added TJS 24/02/12

    End Sub
#End Region

#Region " ProcessMagentoSimpleOptionsPt2 "
    Private Sub ProcessMagentoSimpleOptionsPt2(ByVal InstanceID As String, ByVal HomeCurrency As String, ByRef ItemsForImportRow As DataRow)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/05/13 | TJS             | 2013.1.12 | Function created from code removed from ImportMagentoProducts
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to update costs on matrix items
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoMatrixItemDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoDetails_DEV000221Row ' TJS 15/03/13
        Dim strMatrixItemPrices As String(), strActiveCurrencies()() As String, strMagentoFields As String() ' TJS 29/03/13
        Dim strMatrixItemCosts As String(), strUpdateSQL As String, decItemCosts() As Decimal  ' TJS 09/08/13
        Dim decItemPrices() As Decimal, iLoop As Integer, iMatrixItemLoop As Integer ' TJS 04/04/11

        For iMatrixItemLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
            ' start of code replaced TJS 29/03/13
            ReDim decItemPrices(2)
            strMatrixItemPrices = Me.GetRow(New String() {"PricingCost", "WholesalePrice", "RetailPrice"}, "InventoryItemPricingDetail", "ItemCode = '" & NewItemDataset.InventoryItem(0).ItemCode & "' AND CurrencyCode = '" & HomeCurrency & "'") ' TJS 03/04/13
            If strMatrixItemPrices IsNot Nothing Then
                decItemPrices(0) = CDec(strMatrixItemPrices(0))
                decItemPrices(1) = CDec(strMatrixItemPrices(1))
                decItemPrices(2) = CDec(strMatrixItemPrices(2))
            Else
                decItemPrices(0) = 0
                decItemPrices(1) = 0
                decItemPrices(2) = 0
            End If
            ' start of code added TJS 09/08/13
            ReDim decItemCosts(5)
            strMatrixItemCosts = Me.GetRow(New String() {"StandardCost", "StandardCostRate", "CurrentCost", "CurrentCostRate", "AverageCost", "AverageCostRate"}, "InventoryItem", "ItemCode = '" & NewItemDataset.InventoryItem(0).ItemCode & "'")
            If strMatrixItemCosts IsNot Nothing Then
                decItemCosts(0) = CDec(strMatrixItemCosts(0))
                decItemCosts(1) = CDec(strMatrixItemCosts(1))
                decItemCosts(2) = CDec(strMatrixItemCosts(2))
                decItemCosts(3) = CDec(strMatrixItemCosts(3))
                decItemCosts(4) = CDec(strMatrixItemCosts(4))
                decItemCosts(5) = CDec(strMatrixItemCosts(5))
            Else
                decItemCosts(0) = 0
                decItemCosts(1) = 0
                decItemCosts(2) = 0
                decItemCosts(3) = 0
                decItemCosts(4) = 0
                decItemCosts(5) = 0
            End If
            ' end of code added TJS 09/08/13
            For iLoop = 0 To m_MagentoOptions.Length - 1
                If Not String.IsNullOrEmpty(m_MagentoOptions(iLoop).Attribute1Code) Then
                    If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode1Null AndAlso _
                        Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute1Null AndAlso _
                        (NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode1.ToLower = m_MagentoOptions(iLoop).Attribute1Code.ToLower And _
                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute1.ToLower = m_MagentoOptions(iLoop).Attribute1Value.ToLower) Then ' TJS 30/04/13
                        decItemPrices(0) = decItemPrices(0) + m_MagentoOptions(iLoop).Price
                        decItemPrices(1) = decItemPrices(1) + m_MagentoOptions(iLoop).Price
                        decItemPrices(2) = decItemPrices(2) + m_MagentoOptions(iLoop).Price
                    End If
                End If
                If Not String.IsNullOrEmpty(m_MagentoOptions(iLoop).Attribute2Code) Then
                    If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode2Null AndAlso _
                        Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute2Null AndAlso _
                        (NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode2.ToLower = m_MagentoOptions(iLoop).Attribute2Code.ToLower And _
                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute2.ToLower = m_MagentoOptions(iLoop).Attribute2Value.ToLower) Then ' TJS 30/04/13
                        decItemPrices(0) = decItemPrices(0) + m_MagentoOptions(iLoop).Price
                        decItemPrices(1) = decItemPrices(1) + m_MagentoOptions(iLoop).Price
                        decItemPrices(2) = decItemPrices(2) + m_MagentoOptions(iLoop).Price
                    End If
                End If
                If Not String.IsNullOrEmpty(m_MagentoOptions(iLoop).Attribute3Code) Then
                    If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode3Null AndAlso _
                        Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute3Null AndAlso _
                        (NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode3.ToLower = m_MagentoOptions(iLoop).Attribute3Code.ToLower And _
                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute3.ToLower = m_MagentoOptions(iLoop).Attribute3Value.ToLower) Then ' TJS 30/04/13
                        decItemPrices(0) = decItemPrices(0) + m_MagentoOptions(iLoop).Price
                        decItemPrices(1) = decItemPrices(1) + m_MagentoOptions(iLoop).Price
                        decItemPrices(2) = decItemPrices(2) + m_MagentoOptions(iLoop).Price
                    End If
                End If
                If Not String.IsNullOrEmpty(m_MagentoOptions(iLoop).Attribute4Code) Then
                    If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode4Null AndAlso _
                        Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute4Null AndAlso _
                        (NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode4.ToLower = m_MagentoOptions(iLoop).Attribute4Code.ToLower And _
                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute4.ToLower = m_MagentoOptions(iLoop).Attribute4Value.ToLower) Then ' TJS 30/04/13
                        decItemPrices(0) = decItemPrices(0) + m_MagentoOptions(iLoop).Price
                        decItemPrices(1) = decItemPrices(1) + m_MagentoOptions(iLoop).Price
                        decItemPrices(2) = decItemPrices(2) + m_MagentoOptions(iLoop).Price
                    End If
                End If
                If Not String.IsNullOrEmpty(m_MagentoOptions(iLoop).Attribute5Code) Then
                    If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode5Null AndAlso _
                        Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute5Null AndAlso _
                        (NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode5.ToLower = m_MagentoOptions(iLoop).Attribute5Code.ToLower And _
                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute5.ToLower = m_MagentoOptions(iLoop).Attribute5Value.ToLower) Then ' TJS 30/04/13
                        decItemPrices(0) = decItemPrices(0) + m_MagentoOptions(iLoop).Price
                        decItemPrices(1) = decItemPrices(1) + m_MagentoOptions(iLoop).Price
                        decItemPrices(2) = decItemPrices(2) + m_MagentoOptions(iLoop).Price
                    End If
                End If
                If Not String.IsNullOrEmpty(m_MagentoOptions(iLoop).Attribute6Code) Then
                    If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttributeCode6Null AndAlso _
                        Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsAttribute6Null AndAlso _
                        (NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode6.ToLower = m_MagentoOptions(iLoop).Attribute6Code.ToLower And _
                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute6.ToLower = m_MagentoOptions(iLoop).Attribute6Value.ToLower) Then ' TJS 30/04/13
                        decItemPrices(0) = decItemPrices(0) + m_MagentoOptions(iLoop).Price
                        decItemPrices(1) = decItemPrices(1) + m_MagentoOptions(iLoop).Price
                        decItemPrices(2) = decItemPrices(2) + m_MagentoOptions(iLoop).Price
                    End If
                End If
            Next
            ' update matrix item price
            strActiveCurrencies = Me.GetRows(New String() {"CurrencyCode"}, "InventoryItemPricingDetail", "ItemCode = '" & NewItemDataset.InventoryItem(0).ItemCode & "'")
            For Each strActiveCurrency As String() In strActiveCurrencies
                Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItemPricingDetail SET PricingCost = " & decItemPrices(0) & _
                    ", PricingCostRate = " & decItemPrices(0) * Me.GetExchangerate(strActiveCurrency(0)) & ", WholesalePrice = " & _
                    decItemPrices(1) & ", WholesalePriceRate = " & decItemPrices(1) * Me.GetExchangerate(strActiveCurrency(0)) & _
                    ", RetailPrice = " & decItemPrices(2) & ", RetailPriceRate = " & decItemPrices(2) * Me.GetExchangerate(strActiveCurrency(0)) & _
                    ", UserModified = '" & Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & _
                    "', DateModified = getdate() WHERE ItemCode = '" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode & _
                    "' AND CurrencyCode = '" & strActiveCurrency(0) & "'", Nothing)
            Next
            ' end of code replaced TJS 29/03/13

            ' start of code added TJS 09/08/13
            strUpdateSQL = ""
            If m_ImportCostAsStandard Then
                If strUpdateSQL <> "" Then
                    strUpdateSQL = strUpdateSQL & ", "
                End If
                strUpdateSQL = strUpdateSQL & "StandardCost = " & decItemCosts(0) & ", StandardCostRate = " & decItemCosts(1)
            End If
            If m_ImportCostAsLast Then
                If strUpdateSQL <> "" Then
                    strUpdateSQL = strUpdateSQL & ", "
                End If
                strUpdateSQL = strUpdateSQL & "CurrentCost = " & decItemCosts(2) & ", CurrentCostRate = " & decItemCosts(3) & ", CurrentCostDate = getdate()"
            End If
            If m_ImportCostAsAverage Then
                If strUpdateSQL <> "" Then
                    strUpdateSQL = strUpdateSQL & ", "
                End If
                strUpdateSQL = strUpdateSQL & "AverageCost = " & decItemCosts(4) & ", AverageCostRate = " & decItemCosts(5) & ", AverageCostDate = getdate()"
            End If
            If strUpdateSQL <> "" Then
                Me.ExecuteNonQuery(CommandType.Text, "UPDATE InventoryItem SET " & strUpdateSQL & ", UserModified = '" & Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & _
                    "', DateModified = getdate() WHERE ItemCode = '" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode & "'", Nothing)
            End If
            ' end of code added TJS 09/08/13

            ' start of code added TJS 15/03/13
            strMagentoFields = Me.GetRow(New String() {"MagentoProductID_DEV000221", "Publish_DEV000221", "ProductName_DEV000221", _
                "ProductShortDescription_DEV000221", "ProductDescription_DEV000221", "ProductInDepthDescription_DEV000221", _
                "SellingPrice_DEV000221", "UseISPricingDetail_DEV000221", "Visibility_DEV000221", "Status_DEV000221", _
                "ShowAsNewFrom_DEV000221", "ShowAsNewTo_DEV000221", "ShowAsSpecialFrom_DEV000221", "ShowAsSpecialTo_DEV000221", _
                "SpecialPrice_DEV000221", "FromImportWizard_DEV000221", "QtyPublishingType_DEV000221", "QtyPublishingValue_DEV000221", _
                "SourceIsGroupItem_DEV000221"}, "InventoryMagentoDetails_DEV000221", "ItemCode_DEV000221 = '" & _
                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode & "' AND InstanceID_DEV000221 = '" & InstanceID & "'")
            If strMagentoFields IsNot Nothing AndAlso Not String.IsNullOrEmpty(strMagentoFields(0)) Then
                ' Magento details exist for Matrix Item, are values correct ?
                strUpdateSQL = ""
                If ((strMagentoFields(1).ToLower = "true" Or strMagentoFields(1).ToLower = "1") And Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Publish_DEV000221) Or _
                    (strMagentoFields(1).ToLower <> "true" And strMagentoFields(1).ToLower <> "1" And m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Publish_DEV000221) Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "Publish_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Publish_DEV000221 ' TJS 03/04/13
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductName_DEV000221Null Then
                    If strMagentoFields(2) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ProductName_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(2) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductName_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & ", ProductName_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductName_DEV000221.Replace("'", "''") & "'" ' TJS 03/04/13
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductShortDescription_DEV000221Null Then
                    If strMagentoFields(3) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ProductShortDescription_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(3) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductShortDescription_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "ProductShortDescription_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductShortDescription_DEV000221.Replace("'", "''") & "'" ' TJS 03/04/13
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductDescription_DEV000221Null Then
                    If strMagentoFields(4) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ProductDescription_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(4) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductDescription_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "ProductDescription_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductDescription_DEV000221.Replace("'", "''") & "'" ' TJS 03/04/13
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductInDepthDescription_DEV000221Null Then
                    If strMagentoFields(5) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ProductInDepthDescription_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(5) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductInDepthDescription_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "ProductInDepthDescription_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductInDepthDescription_DEV000221.Replace("'", "''") & "'" ' TJS 03/04/13
                End If
                If CDbl(strMagentoFields(6)) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).SellingPrice_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "SellingPrice_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).SellingPrice_DEV000221
                End If
                If ((strMagentoFields(7).ToLower = "true" Or strMagentoFields(7).ToLower = "1") And Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).UseISPricingDetail_DEV000221) Or _
                    (strMagentoFields(7).ToLower <> "true" And strMagentoFields(7).ToLower <> "1" And m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).UseISPricingDetail_DEV000221) Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "UseISPricingDetail_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).UseISPricingDetail_DEV000221
                End If
                If CInt(strMagentoFields(8)) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Visibility_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "Visibility_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Visibility_DEV000221
                End If
                If CInt(strMagentoFields(9)) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Status_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "Status_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Status_DEV000221
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsNewFrom_DEV000221Null Then
                    If strMagentoFields(10) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ShowAsNewFrom_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(10) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewFrom_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "ShowAsNewFrom_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewFrom_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") & "'"
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsNewTo_DEV000221Null Then
                    If strMagentoFields(11) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ShowAsNewTo_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(11) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewTo_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "ShowAsNewTo_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewTo_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") & "'"
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null Then
                    If strMagentoFields(12) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ShowAsSpecialFrom_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(12) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "ShowAsSpecialFrom_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") & "'"
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null Then
                    If strMagentoFields(13) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", ShowAsSpecialTo_DEV000221 = Null"
                    End If
                ElseIf strMagentoFields(13) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "ShowAsSpecialTo_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221.ToString("MM/dd/yyyy mm:HH:ss") & "'"
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsSpecialPrice_DEV000221Null Then
                    If strMagentoFields(14) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", SpecialPrice_DEV000221 = Null"
                    End If
                ElseIf CDbl(strMagentoFields(14)) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "SpecialPrice_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221
                End If
                If ((strMagentoFields(15).ToLower = "true" Or strMagentoFields(15).ToLower = "1") And Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).FromImportWizard_DEV000221) Or _
                    (strMagentoFields(15).ToLower <> "true" And strMagentoFields(15).ToLower <> "1" And m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).FromImportWizard_DEV000221) Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "FromImportWizard_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).FromImportWizard_DEV000221
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                    If strMagentoFields(16) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", QtyPublishingType_DEV000221 = 'None'"
                    End If
                ElseIf strMagentoFields(16) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingType_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "QtyPublishingType_DEV000221 = '" & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingType_DEV000221 & "'"
                End If
                If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsQtyPublishingValue_DEV000221Null Then
                    If strMagentoFields(17) <> "" Then
                        If strUpdateSQL <> "" Then
                            strUpdateSQL = strUpdateSQL & ", "
                        End If
                        strUpdateSQL = strUpdateSQL & ", QtyPublishingValue_DEV000221 = Null"
                    End If
                ElseIf CDbl(strMagentoFields(17)) <> m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingValue_DEV000221 Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "QtyPublishingValue_DEV000221 = " & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingValue_DEV000221
                End If
                If strMagentoFields(18).ToLower <> "true" And strMagentoFields(18).ToLower <> "1" Then
                    If strUpdateSQL <> "" Then
                        strUpdateSQL = strUpdateSQL & ", "
                    End If
                    strUpdateSQL = strUpdateSQL & "SourceIsGroupItem_DEV000221 = 1"
                End If
                If strMagentoFields(0) <> ItemsForImportRow.Item("SourceItemID").ToString Then
                    ' no update it
                    strUpdateSQL = "UPDATE InventoryMagentoDetails_DEV000221 SET MagentoProductID_DEV000221 = '" & _
                        ItemsForImportRow.Item("SourceItemID").ToString & "', " & strUpdateSQL & _
                        ", UserModified = '" & Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & _
                        "', DateModified = getdate() WHERE ItemCode_DEV000221 = '" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode & "'"
                ElseIf strUpdateSQL <> "" Then
                    strUpdateSQL = "UPDATE InventoryMagentoDetails_DEV000221 SET " & strUpdateSQL & ", UserModified = '" & _
                        Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & _
                        "', DateModified = getdate() WHERE ItemCode_DEV000221 = '" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode & "'"
                End If
                If strUpdateSQL <> "" Then
                    Me.ExecuteNonQuery(CommandType.Text, strUpdateSQL, Nothing)
                End If

            Else
                rowMagentoMatrixItemDetails = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.NewInventoryMagentoDetails_DEV000221Row
                rowMagentoMatrixItemDetails.ItemCode_DEV000221 = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode
                rowMagentoMatrixItemDetails.MagentoProductID_DEV000221 = ItemsForImportRow.Item("SourceItemID").ToString ' TJS 18/03/13
                rowMagentoMatrixItemDetails.InstanceID_DEV000221 = InstanceID
                rowMagentoMatrixItemDetails.Publish_DEV000221 = True
                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductName_DEV000221Null Then
                    rowMagentoMatrixItemDetails.ProductName_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductName_DEV000221
                End If
                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductShortDescription_DEV000221Null Then
                    rowMagentoMatrixItemDetails.ProductShortDescription_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductShortDescription_DEV000221
                End If
                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductDescription_DEV000221Null Then
                    rowMagentoMatrixItemDetails.ProductDescription_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductDescription_DEV000221
                End If
                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsSellingPrice_DEV000221Null Then
                    rowMagentoMatrixItemDetails.SellingPrice_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).SellingPrice_DEV000221
                Else
                    rowMagentoMatrixItemDetails.SellingPrice_DEV000221 = 0
                End If
                rowMagentoMatrixItemDetails.FromImportWizard_DEV000221 = True
                rowMagentoMatrixItemDetails.SourceIsGroupItem_DEV000221 = True
                rowMagentoMatrixItemDetails.QtyPublishingType_DEV000221 = m_QuantityPublishingType
                rowMagentoMatrixItemDetails.QtyPublishingValue_DEV000221 = CInt(m_QuantityPublishingValue)
                rowMagentoMatrixItemDetails.TotalQtyWhenLastPublished_DEV000221 = 0
                rowMagentoMatrixItemDetails.QtyLastPublished_DEV000221 = 0
                m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.AddInventoryMagentoDetails_DEV000221Row(rowMagentoMatrixItemDetails)
            End If
            ' end of code added TJS 15/03/13
        Next

    End Sub
#End Region

#Region " ProcessMagentoGroupedProducts "
    Private Sub ProcessMagentoGroupedProducts(ByRef objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector, _
        ByRef XMLNSManMagento As System.Xml.XmlNamespaceManager, ByVal InstanceID As String, ByVal HomeCurrency As String, _
        ByRef HomeLanguage As String, ByRef ItemsForImportRow As DataRow, ByRef ISItemExists As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/05/13 | TJS             | 2013.1.12 | Function created from code removed from ImportMagentoProducts
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowItemKitOptGroup As Interprise.Framework.Inventory.DatasetGateway.ItemKitDatasetGateway.InventoryKitOptionGroupRow
        Dim XMLTemp As XDocument
        Dim XMLGroupedList As System.Collections.Generic.IEnumerable(Of XElement), XMLGroupedNode As XElement
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement
        Dim strErrorDetails As String, strItemDescriptions As String(), strMagentoFields As String()
        Dim decItemPrices() As Decimal, iKitDetailCount As Integer, iLoop As Integer
        Dim iTableLoop As Integer, iRowLoop As Integer, iColumnLoop As Integer

        If objMagento.GetCatalogProductLinks(ItemsForImportRow.Item("SourceItemID").ToString, "grouped") Then
            If Not ISItemExists Then ' TJS 10/06/12
                NewKitFacade.LoadDataSet(New String()() {New String() {NewKitDataset.InventoryKit.TableName, _
                    READINVENTORYKIT, AT_ITEM_KIT_CODE, NewItemDataset.InventoryItem(0).ItemCode}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
            End If
            ' get product XML item list
            XMLGroupedList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
            ReDim decItemPrices(m_MagentoImportRule.GetXMLElementListCount(XMLGroupedList) - 1)
            iKitDetailCount = 0
            For Each XMLGroupedNode In XMLGroupedList
                XMLTemp = XDocument.Parse(XMLGroupedNode.ToString)
                XMLItemList = XMLTemp.XPathSelectElements("item/item")
                For Each XMLItemNode In XMLItemList
                    Select Case XMLItemNode.XPathSelectElement("key").Value
                        Case "product_id"
                            strMagentoFields = Me.GetRow(New String() {"ItemCode_DEV000221"}, "InventoryMagentoDetails_DEV000221", "InstanceID_DEV000221 = '" & InstanceID & "' AND MagentoProductID_DEV000221 = '" & XMLItemNode.XPathSelectElement("value").Value & "'")
                            strItemDescriptions = Me.GetRow(New String() {"ItemDescription", "ExtendedDescription"}, "InventoryItemDescription", "ItemCode = '" & strMagentoFields(0) & "' AND LanguageCode= '" & HomeLanguage & "'")
                            decItemPrices(iKitDetailCount) = CDec(Me.GetField("RetailPriceRate", "InventoryItemPricingDetail", "ItemCode = '" & strMagentoFields(0) & "' AND CurrencyCode = '" & HomeCurrency & "'"))

                            rowItemKitOptGroup = NewKitDataset.InventoryKitOptionGroup.FindByItemKitCodeGroupCode(NewItemDataset.InventoryItem(0).ItemCode, strItemDescriptions(0)) ' TJS 10/06/12
                            If rowItemKitOptGroup Is Nothing Then ' TJS 10/06/12
                                rowItemKitOptGroup = NewKitDataset.InventoryKitOptionGroup.NewInventoryKitOptionGroupRow
                                rowItemKitOptGroup.ItemKitCode = NewItemDataset.InventoryItem(0).ItemCode
                                rowItemKitOptGroup.GroupCode = strItemDescriptions(0)
                                rowItemKitOptGroup.GroupType = KIT_GROUP_TYPE_OPTIONAL
                                rowItemKitOptGroup.SortOrder = iKitDetailCount + 1
                                rowItemKitOptGroup.SelectionControl = KIT_SELECTION_RADIO_LIST
                                NewKitDataset.InventoryKitOptionGroup.AddInventoryKitOptionGroupRow(rowItemKitOptGroup)
                                NewKitFacade.CreateKitOptionGroupDescription(strItemDescriptions(0), strItemDescriptions(0))
                                NewKitFacade.GroupCode = strItemDescriptions(0)
                                NewKitFacade.CurrencyCode = HomeCurrency
                                LoadDataSet(New String()() {New String() {m_MagentoImportDataset.InventoryItemKitPricingDetailView.TableName, _
                                    "ReadInventoryItemKitPricingDetailView_DEV000221", AT_ITEMCODE, strMagentoFields(0), _
                                    Interprise.Framework.Base.Shared.Const.AT_LANGUAGECODE, NewItemFacade.LanguageCode}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific)

                                NewKitFacade.AssignKitDetails(New DataRow() {m_MagentoImportDataset.InventoryItemKitPricingDetailView(0)}, strItemDescriptions(0), HomeCurrency, Nothing)
                            Else
                                ' start of code added TJS 10/06/12
                                If rowItemKitOptGroup.IsGroupTypeNull OrElse rowItemKitOptGroup.GroupType <> KIT_GROUP_TYPE_OPTIONAL Then
                                    rowItemKitOptGroup.GroupType = KIT_GROUP_TYPE_OPTIONAL
                                End If
                                If rowItemKitOptGroup.IsSortOrderNull OrElse rowItemKitOptGroup.SortOrder <> iKitDetailCount + 1 Then
                                    rowItemKitOptGroup.SortOrder = iKitDetailCount + 1
                                End If
                                If rowItemKitOptGroup.IsSelectionControlNull OrElse rowItemKitOptGroup.SelectionControl <> KIT_SELECTION_RADIO_LIST Then
                                    rowItemKitOptGroup.SelectionControl = KIT_SELECTION_RADIO_LIST
                                End If
                                If NewKitFacade.GroupCode <> strItemDescriptions(0) Then
                                    NewKitFacade.GroupCode = strItemDescriptions(0)
                                End If
                                If NewKitFacade.CurrencyCode <> HomeCurrency Then
                                    NewKitFacade.CurrencyCode = HomeCurrency
                                End If
                                ' end of code added TJS 10/06/12
                            End If
                            If NewKitDataset.InventoryKitDetail(iKitDetailCount).IsIsDefaultNull OrElse _
                                NewKitDataset.InventoryKitDetail(iKitDetailCount).IsDefault Then ' TJS 10/06/12
                                NewKitDataset.InventoryKitDetail(iKitDetailCount).IsDefault = False
                            End If
                            If NewKitDataset.InventoryKitDetail(iKitDetailCount).IsSalesPriceRateNull OrElse _
                                NewKitDataset.InventoryKitDetail(iKitDetailCount).SalesPriceRate <> decItemPrices(iKitDetailCount) Then ' TJS 10/06/12
                                NewKitDataset.InventoryKitDetail(iKitDetailCount).SalesPriceRate = decItemPrices(iKitDetailCount)
                            End If

                            For iLoop = 0 To NewKitDataset.InventoryKitDetailDescription.Count - 1
                                If NewKitDataset.InventoryKitDetailDescription(iLoop).IsExtendedDescriptionNull OrElse _
                                    NewKitDataset.InventoryKitDetailDescription(iLoop).ExtendedDescription <> strItemDescriptions(1) Then ' TJS 10/06/12
                                    NewKitDataset.InventoryKitDetailDescription(iLoop).ExtendedDescription = strItemDescriptions(1) ' TJS 10/06/12
                                End If
                            Next

                            If NewKitFacade.UpdateDataSet(NewKitFacade.CommandSet, Interprise.Framework.Base.Shared.TransactionType.InventoryItem, "Create Kit Item", False) Then
                                iKitDetailCount = iKitDetailCount + 1
                            Else
                                ' get error details
                                strErrorDetails = ""
                                ' start of code replaced TJS 08/04/11
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
                                ' start of code replaced TJS 08/04/11
                                m_LastError = strErrorDetails ' TJS 02/12/11
                                m_LastErrorMessage = m_LastError ' TJS 14/02/12
                                m_ProcessLog = m_ProcessLog & "Import of SKU " & NewItemDataset.InventoryItem(0).ItemName & " aborted - " & strErrorDetails & vbCrLf ' TJS 08/04/11

                            End If
                    End Select
                Next

            Next
        Else
            m_LastError = "Unable to read Magento Product Group Details for SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " - " & objMagento.LastError ' TJS 02/12/11
            m_LastErrorMessage = "Unable to read Magento Product Group Details for SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " - " & objMagento.LastErrorMessage ' TJS 14/02/12
            m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted as unable to read Magento Product Group Details - " & objMagento.LastError & vbCrLf ' TJS 08/04/11
        End If

    End Sub
#End Region

#Region " ProcessMagentoRelatedProducts "
    Private Sub ProcessMagentoRelatedProducts(ByRef objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector, _
        ByRef XMLNSManMagento As System.Xml.XmlNamespaceManager, ByRef ProductType As String, ByRef ItemsForImportRow As DataRow, _
        ByRef ProductSuperAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductSuperAttributeType(), _
        ByRef MatrixAttributeError As Boolean, ByRef ExitFor As Boolean, ByRef ContinueFor As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/05/13 | TJS             | 2013.1.12 | Function created from code removed from ImportMagentoProducts
        ' 16/06/13 | TJS             | 2013.1.20 | Modified to detect any matrix items no longer required and marked for removal
        ' 23/06/13 | TJS             | 2013.1.22 | Corrected row pointer when marking matrix items for removal
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTemp As XDocument, XMLItemTemp As XDocument
        Dim XMLRelatedItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLRelatedItemNode As XElement ' TJS 13/04/11
        Dim XMLOptionsList As System.Collections.Generic.IEnumerable(Of XElement), XMLOptionNode As XElement ' TJS 24/02/12
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement
        Dim strMatrixValues1 As String(), strMatrixValues2 As String(), strMatrixValues3 As String() ' TJS 04/04/11
        Dim strMatrixValues4 As String(), strMatrixValues5 As String(), strMatrixValues6 As String() ' TJS 04/04/11
        Dim strMatrixValueIDs1 As String(), strMatrixValueIDs2 As String(), strMatrixValueIDs3 As String() ' TJS 10/06/12
        Dim strMatrixValueIDs4 As String(), strMatrixValueIDs5 As String(), strMatrixValueIDs6 As String() ' TJS 10/06/12
        Dim strTemp As String, strConfigurablePlusOptionMsg As String, strConfigurablePlusOptionLine As String ' TJS 15/03/13 TJS 24/03/13
        Dim strErrorDetails As String, strItemCode As String, bAttributeFound As Boolean
        Dim iMatrixItemLoop As Integer, iAttributeLoop As Integer, iLoop As Integer, iListCount As Integer ' TJS 04/04/11 ' TJS 24/03/13
        Dim bItemMatched As Boolean ' TJS 16/06/13

        ' get relate product details from Magento (it includes the attribute details)
        If objMagento.GetCatalogRelatedProducts(m_MagentoImportRule.ConvertForXML(ItemsForImportRow.Item("ItemSKU").ToString)) Then ' TJS 03/04/13
            XMLRelatedItemList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
            ' did we find any related products ?
            iListCount = m_MagentoImportRule.GetXMLElementListCount(XMLRelatedItemList) ' TJS 24/03/13
            If iListCount > 0 Then ' TJS 24/03/13
                ' yes, process them
                ReDim m_MagentoRelatedProducts(iListCount - 1) ' TJS 24/03/13
                'ReDim strMatrixAttributes(0) ' TJS 10/06/12
                ReDim strMatrixValues1(0)
                ReDim strMatrixValues2(0)
                ReDim strMatrixValues3(0)
                ReDim strMatrixValues4(0)
                ReDim strMatrixValues5(0)
                ReDim strMatrixValues6(0)
                ReDim strMatrixValueIDs1(0) ' TJS 10/06/12
                ReDim strMatrixValueIDs2(0) ' TJS 10/06/12
                ReDim strMatrixValueIDs3(0) ' TJS 10/06/12
                ReDim strMatrixValueIDs4(0) ' TJS 10/06/12
                ReDim strMatrixValueIDs5(0) ' TJS 10/06/12
                ReDim strMatrixValueIDs6(0) ' TJS 10/06/12
                iMatrixItemLoop = 0
                For Each XMLRelatedItemNode In XMLRelatedItemList
                    XMLTemp = XDocument.Parse(XMLRelatedItemNode.ToString)
                    XMLItemList = XMLTemp.XPathSelectElements("item/value/item")
                    For Each XMLItemNode In XMLItemList
                        XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                        ' start of code added TJS 10/06/12 
                        ' does element name match any of the ProductSuperAttributes ?
                        If ProductSuperAttributes.Length >= 1 AndAlso Not String.IsNullOrEmpty(ProductSuperAttributes(0).AttributeCode) AndAlso _
                            m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key") = ProductSuperAttributes(0).AttributeCode Then
                            ' yes, matches attribute 1, is value present ?
                            If m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value") <> "" Then
                                ' set Attribute 1 Code in related products array
                                For iLoop = 0 To m_MagentoRelatedProducts.Length - 1
                                    m_MagentoRelatedProducts(iLoop).Attribute1Code = ProductSuperAttributes(0).AttributeCode
                                Next
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute1Value = GetAttributeValue(CInt(m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")))
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute1ValueID = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                                bAttributeFound = False
                                For iLoop = 0 To strMatrixValues1.Length - 1
                                    If strMatrixValues1(iLoop) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute1Value Then
                                        bAttributeFound = True
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If strMatrixValues1.Length = 1 And strMatrixValues1(0) = "" Then
                                        ReDim Preserve strMatrixValues1(strMatrixValues1.Length - 1)
                                        ReDim Preserve strMatrixValueIDs1(strMatrixValueIDs1.Length - 1)
                                    Else
                                        ReDim Preserve strMatrixValues1(strMatrixValues1.Length)
                                        ReDim Preserve strMatrixValueIDs1(strMatrixValueIDs1.Length)
                                    End If
                                    strMatrixValues1(strMatrixValues1.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute1Value
                                    strMatrixValueIDs1(strMatrixValueIDs1.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute1ValueID
                                End If
                            End If

                        ElseIf ProductSuperAttributes.Length >= 2 AndAlso Not String.IsNullOrEmpty(ProductSuperAttributes(1).AttributeCode) AndAlso _
                            m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key") = ProductSuperAttributes(1).AttributeCode Then
                            ' yes, matches attribute 2, is value present ?
                            If m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value") <> "" Then
                                ' set Attribute 2 Code in related products array
                                For iLoop = 0 To m_MagentoRelatedProducts.Length - 1
                                    m_MagentoRelatedProducts(iLoop).Attribute2Code = ProductSuperAttributes(1).AttributeCode
                                Next
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute2Value = GetAttributeValue(CInt(m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")))
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute2ValueID = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                                bAttributeFound = False
                                For iLoop = 0 To strMatrixValues2.Length - 1
                                    If strMatrixValues2(iLoop) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute2Value Then
                                        bAttributeFound = True
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If strMatrixValues2.Length = 1 And strMatrixValues2(0) = "" Then
                                        ReDim Preserve strMatrixValues2(strMatrixValues2.Length - 1)
                                        ReDim Preserve strMatrixValueIDs2(strMatrixValueIDs2.Length - 1)
                                    Else
                                        ReDim Preserve strMatrixValues2(strMatrixValues2.Length)
                                        ReDim Preserve strMatrixValueIDs2(strMatrixValueIDs2.Length)
                                    End If
                                    strMatrixValues2(strMatrixValues2.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute2Value
                                    strMatrixValueIDs2(strMatrixValueIDs2.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute2ValueID
                                End If
                            End If

                        ElseIf ProductSuperAttributes.Length >= 3 AndAlso Not String.IsNullOrEmpty(ProductSuperAttributes(2).AttributeCode) AndAlso _
                            m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key") = ProductSuperAttributes(2).AttributeCode Then
                            ' yes, matches attribute 3, is value present ?
                            If m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value") <> "" Then
                                ' set Attribute 3 Code in related products array
                                For iLoop = 0 To m_MagentoRelatedProducts.Length - 1
                                    m_MagentoRelatedProducts(iLoop).Attribute3Code = ProductSuperAttributes(2).AttributeCode
                                Next
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute3Value = GetAttributeValue(CInt(m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")))
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute3ValueID = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                                bAttributeFound = False
                                For iLoop = 0 To strMatrixValues3.Length - 1
                                    If strMatrixValues3(iLoop) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute3Value Then
                                        bAttributeFound = True
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If strMatrixValues3.Length = 1 And strMatrixValues3(0) = "" Then
                                        ReDim Preserve strMatrixValues3(strMatrixValues3.Length - 1)
                                        ReDim Preserve strMatrixValueIDs3(strMatrixValueIDs3.Length - 1)
                                    Else
                                        ReDim Preserve strMatrixValues3(strMatrixValues3.Length)
                                        ReDim Preserve strMatrixValueIDs3(strMatrixValueIDs3.Length)
                                    End If
                                    strMatrixValues3(strMatrixValues3.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute3Value
                                    strMatrixValueIDs3(strMatrixValueIDs3.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute3ValueID
                                End If
                            End If

                        ElseIf ProductSuperAttributes.Length >= 4 AndAlso Not String.IsNullOrEmpty(ProductSuperAttributes(3).AttributeCode) AndAlso _
                            m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key") = ProductSuperAttributes(3).AttributeCode Then
                            ' yes, matches attribute 4, is value present ?
                            If m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value") <> "" Then
                                ' set Attribute 4 Code in related products array
                                For iLoop = 0 To m_MagentoRelatedProducts.Length - 1
                                    m_MagentoRelatedProducts(iLoop).Attribute4Code = ProductSuperAttributes(3).AttributeCode
                                Next
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute4Value = GetAttributeValue(CInt(m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")))
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute4ValueID = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                                bAttributeFound = False
                                For iLoop = 0 To strMatrixValues4.Length - 1
                                    If strMatrixValues4(iLoop) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute4Value Then
                                        bAttributeFound = True
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If strMatrixValues4.Length = 1 And strMatrixValues4(0) = "" Then
                                        ReDim Preserve strMatrixValues4(strMatrixValues4.Length - 1)
                                        ReDim Preserve strMatrixValueIDs4(strMatrixValueIDs4.Length - 1)
                                    Else
                                        ReDim Preserve strMatrixValues4(strMatrixValues4.Length)
                                        ReDim Preserve strMatrixValueIDs4(strMatrixValueIDs4.Length)
                                    End If
                                    strMatrixValues4(strMatrixValues4.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute4Value
                                    strMatrixValueIDs4(strMatrixValueIDs4.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute4ValueID
                                End If
                            End If

                        ElseIf ProductSuperAttributes.Length >= 5 AndAlso Not String.IsNullOrEmpty(ProductSuperAttributes(4).AttributeCode) AndAlso _
                            m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key") = ProductSuperAttributes(4).AttributeCode Then
                            ' yes, matches attribute 5, is value present ?
                            If m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value") <> "" Then
                                ' set Attribute 5 Code in related products array
                                For iLoop = 0 To m_MagentoRelatedProducts.Length - 1
                                    m_MagentoRelatedProducts(iLoop).Attribute5Code = ProductSuperAttributes(4).AttributeCode
                                Next
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute5Value = GetAttributeValue(CInt(m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")))
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute5ValueID = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                                bAttributeFound = False
                                For iLoop = 0 To strMatrixValues5.Length - 1
                                    If strMatrixValues5(iLoop) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute5Value Then
                                        bAttributeFound = True
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If strMatrixValues5.Length = 1 And strMatrixValues5(0) = "" Then
                                        ReDim Preserve strMatrixValues5(strMatrixValues5.Length - 1)
                                        ReDim Preserve strMatrixValueIDs5(strMatrixValueIDs5.Length - 1)
                                    Else
                                        ReDim Preserve strMatrixValues5(strMatrixValues5.Length)
                                        ReDim Preserve strMatrixValueIDs5(strMatrixValueIDs5.Length)
                                    End If
                                    strMatrixValues5(strMatrixValues5.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute5Value
                                    strMatrixValueIDs5(strMatrixValueIDs5.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute5ValueID
                                End If
                            End If

                        ElseIf ProductSuperAttributes.Length >= 6 AndAlso Not String.IsNullOrEmpty(ProductSuperAttributes(5).AttributeCode) AndAlso _
                            m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key") = ProductSuperAttributes(5).AttributeCode Then
                            ' yes, matches attribute 6, is value present ?
                            If m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value") <> "" Then
                                ' set Attribute 6 Code in related products array
                                For iLoop = 0 To m_MagentoRelatedProducts.Length - 1
                                    m_MagentoRelatedProducts(iLoop).Attribute6Code = ProductSuperAttributes(5).AttributeCode
                                Next
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute6Value = GetAttributeValue(CInt(m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")))
                                m_MagentoRelatedProducts(iMatrixItemLoop).Attribute6ValueID = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                                bAttributeFound = False
                                For iLoop = 0 To strMatrixValues6.Length - 1
                                    If strMatrixValues6(iLoop) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute6Value Then
                                        bAttributeFound = True
                                    End If
                                Next
                                If Not bAttributeFound Then
                                    If strMatrixValues6.Length = 1 And strMatrixValues6(0) = "" Then
                                        ReDim Preserve strMatrixValues6(strMatrixValues6.Length - 1)
                                        ReDim Preserve strMatrixValueIDs6(strMatrixValueIDs6.Length - 1)
                                    Else
                                        ReDim Preserve strMatrixValues6(strMatrixValues6.Length)
                                        ReDim Preserve strMatrixValueIDs6(strMatrixValueIDs6.Length)
                                    End If
                                    strMatrixValues6(strMatrixValues6.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute6Value
                                    strMatrixValueIDs6(strMatrixValueIDs6.Length - 1) = m_MagentoRelatedProducts(iMatrixItemLoop).Attribute6ValueID
                                End If
                            End If

                        Else
                            ' end of code added TJS 10/06/12
                            Select Case m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key")
                                Case "sku"
                                    m_MagentoRelatedProducts(iMatrixItemLoop).SKU = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")

                                Case "name"
                                    m_MagentoRelatedProducts(iMatrixItemLoop).Description = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")

                                Case "shirt_size"
                                    ' code replaced by generic attribute processing above TJS 10/06/12

                                Case "color"
                                    ' code replaced by generic attribute processing above TJS 10/06/12

                            End Select
                        End If

                    Next
                    iMatrixItemLoop += 1
                Next
                MatrixAttributeError = False
                For iAttributeLoop = 0 To ProductSuperAttributes.Length - 1 ' TJS 10/06/12
                    If Not String.IsNullOrEmpty(ProductSuperAttributes(iAttributeLoop).AttributeName) Then ' TJS 20/05/12 TJS 10/06/12
                        Select Case iAttributeLoop
                            Case 0
                                If Not ApplyAttributes(ProductSuperAttributes(iAttributeLoop).AttributeName, ProductSuperAttributes(iAttributeLoop).AttributeCode, strMatrixValues1, strMatrixValueIDs1) Then ' TJS 10/06/12
                                    MatrixAttributeError = True
                                    Exit For
                                End If
                            Case 1
                                If Not ApplyAttributes(ProductSuperAttributes(iAttributeLoop).AttributeName, ProductSuperAttributes(iAttributeLoop).AttributeCode, strMatrixValues2, strMatrixValueIDs2) Then ' TJS 10/06/12
                                    MatrixAttributeError = True
                                    Exit For
                                End If
                            Case 2
                                If Not ApplyAttributes(ProductSuperAttributes(iAttributeLoop).AttributeName, ProductSuperAttributes(iAttributeLoop).AttributeCode, strMatrixValues3, strMatrixValueIDs3) Then ' TJS 10/06/12
                                    MatrixAttributeError = True
                                    Exit For
                                End If
                            Case 3
                                If Not ApplyAttributes(ProductSuperAttributes(iAttributeLoop).AttributeName, ProductSuperAttributes(iAttributeLoop).AttributeCode, strMatrixValues4, strMatrixValueIDs4) Then ' TJS 10/06/12
                                    MatrixAttributeError = True
                                    Exit For
                                End If
                            Case 4
                                If Not ApplyAttributes(ProductSuperAttributes(iAttributeLoop).AttributeName, ProductSuperAttributes(iAttributeLoop).AttributeCode, strMatrixValues5, strMatrixValueIDs5) Then ' TJS 10/06/12
                                    MatrixAttributeError = True
                                    Exit For
                                End If
                            Case 5
                                If Not ApplyAttributes(ProductSuperAttributes(iAttributeLoop).AttributeName, ProductSuperAttributes(iAttributeLoop).AttributeCode, strMatrixValues6, strMatrixValueIDs6) Then ' TJS 10/06/12
                                    MatrixAttributeError = True
                                    Exit For
                                End If
                        End Select

                    Else
                        MatrixAttributeError = True ' TJS 20/05/12
                    End If
                Next
                If Not MatrixAttributeError Then
                    ' start of code added TJS 24/03/13
                    If ProductType = "configurable+opt" Then
                        ' get custom options from Magento
                        If objMagento.GetCatalogProductCustomOptions(ItemsForImportRow.Item("SourceItemID").ToString) Then
                            XMLOptionsList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                            strConfigurablePlusOptionMsg = ""
                            For Each XMLOptionNode In XMLOptionsList
                                strConfigurablePlusOptionLine = ""
                                XMLTemp = XDocument.Parse(XMLOptionNode.ToString)
                                XMLItemList = XMLTemp.XPathSelectElements("item/item")
                                For Each XMLItemNode In XMLItemList
                                    XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                    strTemp = m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/value")
                                    Select Case m_MagentoImportRule.GetXMLElementText(XMLItemTemp, "item/key")
                                        Case "sku"
                                            strConfigurablePlusOptionLine = "   SKU - " & strTemp & strConfigurablePlusOptionLine

                                        Case "title"
                                            strConfigurablePlusOptionLine = strConfigurablePlusOptionLine & ", Option Title - " & strTemp

                                        Case "attr_title"
                                            strConfigurablePlusOptionLine = strConfigurablePlusOptionLine & ", Atttribute Title - " & strTemp

                                        Case "price"
                                            strConfigurablePlusOptionLine = strConfigurablePlusOptionLine & ", Option Price - " & strTemp

                                    End Select
                                Next
                                If strConfigurablePlusOptionLine <> "" Then
                                    strConfigurablePlusOptionMsg = strConfigurablePlusOptionMsg & strConfigurablePlusOptionLine & vbCrLf
                                End If

                            Next
                            If strConfigurablePlusOptionMsg <> "" Then
                                m_ManualActionRequired = True
                                m_ProcessLog = m_ProcessLog & "MANUAL ACTION REQUIRED - Magento configurable product with SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " uses Magento options which" & vbCrLf & "  need to be handled as Interprise Accessory Items using the eShopCONNECT Split SKU function" & vbCrLf & "  The option SKUs that need to be inserted into Interprise are :-" & vbCrLf & strConfigurablePlusOptionMsg & vbCrLf
                            End If
                        End If
                    End If
                    ' end of code added TJS 24/03/13

                    ' create Matrix Item records
                    strErrorDetails = ""
                    NewItemFacade.GenerateInventoryMatrixItem(strErrorDetails)
                    If strErrorDetails = "" Then
                        For iMatrixItemLoop = 0 To NewItemDataset.InventoryMatrixItem.Count - 1
                            bItemMatched = False ' TJS 16/06/13
                            For iLoop = 0 To m_MagentoRelatedProducts.Length - 1
                                If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode1.ToLower = m_MagentoRelatedProducts(iLoop).Attribute1Code.ToLower And _
                                    (ProductSuperAttributes.Length = 1 OrElse (ProductSuperAttributes.Length > 1 And _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode2.ToLower = m_MagentoRelatedProducts(iLoop).Attribute2Code.ToLower)) And _
                                    (ProductSuperAttributes.Length <= 2 OrElse (ProductSuperAttributes.Length > 2 And _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode3.ToLower = m_MagentoRelatedProducts(iLoop).Attribute3Code.ToLower)) And _
                                    (ProductSuperAttributes.Length <= 3 OrElse (ProductSuperAttributes.Length > 3 And _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode4.ToLower = m_MagentoRelatedProducts(iLoop).Attribute4Code.ToLower)) And _
                                    (ProductSuperAttributes.Length <= 4 OrElse (ProductSuperAttributes.Length > 4 And _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode5.ToLower = m_MagentoRelatedProducts(iLoop).Attribute5Code.ToLower)) And _
                                    (ProductSuperAttributes.Length <= 5 OrElse (ProductSuperAttributes.Length = 6 And _
                                    NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).AttributeCode6.ToLower = m_MagentoRelatedProducts(iLoop).Attribute6Code.ToLower)) Then ' TJS 10/06/12 TJS 24/03/13
                                    If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute1.ToLower = m_MagentoRelatedProducts(iLoop).Attribute1ValueID.ToLower And _
                                        (ProductSuperAttributes.Length = 1 OrElse (ProductSuperAttributes.Length > 1 And _
                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute2.ToLower = m_MagentoRelatedProducts(iLoop).Attribute2ValueID.ToLower)) And _
                                        (ProductSuperAttributes.Length <= 2 OrElse (ProductSuperAttributes.Length > 2 And _
                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute3.ToLower = m_MagentoRelatedProducts(iLoop).Attribute3ValueID.ToLower)) And _
                                        (ProductSuperAttributes.Length <= 3 OrElse (ProductSuperAttributes.Length > 3 And _
                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute4.ToLower = m_MagentoRelatedProducts(iLoop).Attribute4ValueID.ToLower)) And _
                                        (ProductSuperAttributes.Length <= 4 OrElse (ProductSuperAttributes.Length > 4 And _
                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute5.ToLower = m_MagentoRelatedProducts(iLoop).Attribute5ValueID.ToLower)) And _
                                        (ProductSuperAttributes.Length <= 5 OrElse (ProductSuperAttributes.Length = 6 And _
                                        NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Attribute6.ToLower = m_MagentoRelatedProducts(iLoop).Attribute6ValueID.ToLower)) Then ' TJS 10/06/12 TJS 24/03/13
                                        ' update Matrix Item record
                                        bItemMatched = True ' TJS 16/06/13
                                        If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsMatrixItemNameNull OrElse _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName <> m_MagentoRelatedProducts(iLoop).SKU Then ' TJS 10/06/12
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemName = m_MagentoRelatedProducts(iLoop).SKU
                                        End If
                                        If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).IsMatrixItemDescriptionNull OrElse _
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemDescription <> m_MagentoRelatedProducts(iLoop).Description Then ' TJS 10/06/12
                                            NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemDescription = m_MagentoRelatedProducts(iLoop).Description
                                        End If
                                        strItemCode = Me.GetField("ItemCode", "InventoryItem", "ItemName = '" & m_MagentoRelatedProducts(iLoop).SKU.Replace("'", "''") & "'") ' TJS 03/05/13
                                        If "" & strItemCode = "" Then ' FA 04/11/11
                                            m_ProcessLog = m_ProcessLog & "Magento SKU name " & m_MagentoRelatedProducts(iLoop).SKU.ToString & " does not match to any matrix items imported into IS for matrix group " & ItemsForImportRow.Item("ItemSKU").ToString & vbCrLf ' FA 04/11/11
                                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Selected Then ' TJS 10/06/12
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Selected = False ' FA 04/11/11 TJS 10/06/12
                                            End If

                                        Else
                                            If NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode <> strItemCode Then ' TJS 10/06/12
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode = strItemCode
                                            End If
                                            If Not NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Selected Then ' TJS 10/06/12
                                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Selected = True ' TJS 10/06/12
                                            End If
                                        End If
                                        Exit For ' TJS 24/02/12 TJS 10/06/12
                                    End If
                                Else
                                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted as Matrix Item attributes do not match source attributes" & vbCrLf ' TJS 13/04/11
                                    ' mark as matched to prevent deletion of records below
                                    bItemMatched = True ' TJS 16/06/13
                                End If
                            Next
                            ' start of code added TJS 16/06/13
                            If Not bItemMatched Then
                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).MatrixItemCode = NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).ItemCode & "-Dmy" & NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Counter ' TJS 23/06/13
                                NewItemDataset.InventoryMatrixItem(iMatrixItemLoop).Selected = False ' TJS 23/06/13
                            End If
                            ' end of code added TJS 16/06/13
                        Next

                    Else
                        m_LastError = strErrorDetails ' TJS 02/12/11
                        m_LastErrorMessage = m_LastError ' TJS 14/02/12
                        m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted as unable to generate Matrix Item records - " & strErrorDetails & vbCrLf ' TJS 08/04/11
                        ' start of code added TJS 24/03/13
                        NewItemDataset.Clear()
                        If NewManufacturerFacade IsNot Nothing Then
                            NewManufacturerFacade.Dispose()
                            NewManufacturerDataset.Dispose()
                        End If
                        If NewKitFacade IsNot Nothing Then
                            NewKitFacade.Dispose()
                            NewKitDataset.Dispose()
                        End If
                        m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Clear()
                        m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Clear()
                        m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Clear()
                        If m_MagentoImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                            m_LastError = strErrorDetails
                            m_LastErrorMessage = m_LastError
                            m_ImportLimitReached = True
                            ExitFor = True
                        Else
                            ContinueFor = True
                        End If
                        ' end of code added TJS 24/03/13
                    End If
                Else
                    m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted" & vbCrLf
                    ' start of code added TJS 24/03/13
                    NewItemDataset.Clear()
                    If NewManufacturerFacade IsNot Nothing Then
                        NewManufacturerFacade.Dispose()
                        NewManufacturerDataset.Dispose()
                    End If
                    If NewKitFacade IsNot Nothing Then
                        NewKitFacade.Dispose()
                        NewKitDataset.Dispose()
                    End If
                    m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Clear()
                    m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Clear()
                    m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Clear()
                    If m_MagentoImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                        m_LastError = strErrorDetails
                        m_LastErrorMessage = m_LastError
                        m_ImportLimitReached = True
                        ExitFor = True
                    Else
                        ContinueFor = True
                    End If
                    ' end of code added TJS 24/03/13
                End If
            Else
                ' start of code added TJS 24/03/13
                m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted as no related MAgento products found" & vbCrLf
                NewItemDataset.Clear()
                If NewManufacturerFacade IsNot Nothing Then
                    NewManufacturerFacade.Dispose()
                    NewManufacturerDataset.Dispose()
                End If
                If NewKitFacade IsNot Nothing Then
                    NewKitFacade.Dispose()
                    NewKitDataset.Dispose()
                End If
                m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Clear()
                m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Clear()
                m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Clear()
                If m_MagentoImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                    m_LastError = strErrorDetails
                    m_LastErrorMessage = m_LastError
                    m_ImportLimitReached = True
                    ExitFor = True
                Else
                    ContinueFor = True
                End If
                ' end of code added TJS 24/03/13
            End If
        Else
            ' start of code added TJS 03/04/13
            m_ProcessLog = m_ProcessLog & "Import of SKU " & ItemsForImportRow.Item("ItemSKU").ToString & " aborted as unable to read Magento related products - " & m_LastError & vbCrLf
            NewItemDataset.Clear()
            If NewManufacturerFacade IsNot Nothing Then
                NewManufacturerFacade.Dispose()
                NewManufacturerDataset.Dispose()
            End If
            If NewKitFacade IsNot Nothing Then
                NewKitFacade.Dispose()
                NewKitDataset.Dispose()
            End If
            m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Clear()
            m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Clear()
            m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Clear()
            If m_MagentoImportRule.CheckInventoryImportLimit(m_ProductsAlreadyImported, False, strErrorDetails) Then
                m_LastError = strErrorDetails
                m_LastErrorMessage = m_LastError
                m_ImportLimitReached = True
                ExitFor = True
            Else
                ContinueFor = True
            End If
            ' end of code added TJS 03/04/13
        End If

    End Sub
#End Region

#Region " CheckAttributeMatch "
    Private Function CheckAttributeMatch(ByVal AttributeCodeToCheck As Integer, ByVal AttributeCodeValue As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/04/13 | TJS             | 2013.1.11 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, bAttributeCodeEmpty As Boolean

        bAttributeCodeEmpty = True
        For iLoop = 0 To m_MagentoOptions.Length - 1
            Select Case AttributeCodeToCheck
                Case 1
                    If m_MagentoOptions(iLoop).Attribute1Code = AttributeCodeValue Then
                        Return True
                    ElseIf m_MagentoOptions(iLoop).Attribute1Code <> "" Then
                        bAttributeCodeEmpty = False
                    End If
                Case 2
                    If m_MagentoOptions(iLoop).Attribute2Code = AttributeCodeValue Then
                        Return True
                    ElseIf m_MagentoOptions(iLoop).Attribute2Code <> "" Then
                        bAttributeCodeEmpty = False
                    End If
                Case 3
                    If m_MagentoOptions(iLoop).Attribute3Code = AttributeCodeValue Then
                        Return True
                    ElseIf m_MagentoOptions(iLoop).Attribute3Code <> "" Then
                        bAttributeCodeEmpty = False
                    End If
                Case 4
                    If m_MagentoOptions(iLoop).Attribute4Code = AttributeCodeValue Then
                        Return True
                    ElseIf m_MagentoOptions(iLoop).Attribute4Code <> "" Then
                        bAttributeCodeEmpty = False
                    End If
                Case 5
                    If m_MagentoOptions(iLoop).Attribute5Code = AttributeCodeValue Then
                        Return True
                    ElseIf m_MagentoOptions(iLoop).Attribute5Code <> "" Then
                        bAttributeCodeEmpty = False
                    End If
                Case 6
                    If m_MagentoOptions(iLoop).Attribute6Code = AttributeCodeValue Then
                        Return True
                    ElseIf m_MagentoOptions(iLoop).Attribute6Code <> "" Then
                        bAttributeCodeEmpty = False
                    End If
            End Select
        Next
        If bAttributeCodeEmpty Then
            Return True
        Else
            Return False
        End If

    End Function
#End Region

#Region " GetMagentoAttributeList "
    Public Function GetMagentoAttributeList(ByVal MagentoInstanceID As String, ByRef Cancel As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function 
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to detect changes which result in duplicate primary key values and delete them
        ' 04/01/14 | TJS             | 2013.4.03 | Modified to prevent errors when checking for timestamp changes on deleted duplicate rows
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple magento sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim rowAttribute As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportMagentoAttributes_DEV000221Row
        Dim rowAttributeValue As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportMagentoAttributeValues_DEV000221Row
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLAttribute As XDocument, XMLAttributeValue As XDocument
        Dim XMLOptionValue As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLAttributeList As System.Collections.Generic.IEnumerable(Of XElement), XMLAttributeNode As XElement
        Dim XMLValueList As System.Collections.Generic.IEnumerable(Of XElement), XMLValueNode As XElement
        Dim XMLOptionList As System.Collections.Generic.IEnumerable(Of XElement), XMLOptionNode As XElement
        Dim XMLOptionValueList As System.Collections.Generic.IEnumerable(Of XElement), XMLOptionValueNode As XElement
        Dim XMLOptionAttributeList As System.Collections.Generic.IEnumerable(Of XAttribute), XMLOptionAttribute As XAttribute
        Dim strTemp As String, strErrorDetails As String, strLabel As String, strName As String, strKey As String
        Dim strScope As String, strType As String, strAttributeCode As String, strAttributeUse As String
        Dim strTimestamp As String(), strTimestampParts As String()
        Dim iAttributeID As Integer, iCheckDuplicates As Integer, dteTimestamp As Date, dteAttributeModTimestamp As Date ' TJS 09/12/13
        Dim bRecordFound As Boolean, bReturnValue As Boolean, bRequired As Boolean, bMagentoV2APIWSI As Boolean
        Dim bOptionValuesAreArray As Boolean, bInstanceMatched As Boolean

        Try
            GetMagentoAttributeList = True
            m_LastErrorMessage = ""
            m_LastError = ""
            If m_MagentoImportRule.IsActivated Then
                XMLConfig = XDocument.Parse(Trim(m_MagentoImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                bInstanceMatched = False
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If m_MagentoImportRule.GetXMLElementListCount(XMLNodeList) = 1 Then
                        bInstanceMatched = True
                    ElseIf m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoInstanceID Then
                        bInstanceMatched = True
                    End If
                    If bInstanceMatched Then
                        bMagentoV2APIWSI = CBool(IIf(UCase(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False))
                        objMagento = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
                        objMagento.V2SoapAPIWSICompliant = bMagentoV2APIWSI
                        objMagento.MagentoVersion = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION)
                        strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION)
                        If strTemp <> "" Then
                            objMagento.LerrynAPIVersion = CDec(strTemp)
                        Else
                            objMagento.LerrynAPIVersion = 0
                        End If
                        If objMagento.Login(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                            m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                            m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)))) Then
                            XMLNameTabMagento = New System.Xml.NameTable
                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                            Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.TableName, _
                                "ReadLerrynImportExportMagentoAttributes_DEV000221", AT_INSTANCE_ID, MagentoInstanceID}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                            ' have we read any attribute previously ?
                            If m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Count = 0 Then
                                ' no, get latest timestamp from Attribute log table to save with new records as not available in list XML
                                If objMagento.GetAttributeTimestamp() Then
                                    strTemp = m_MagentoImportRule.GetXMLElementText(objMagento.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item/item/value", XMLNSManMagento)
                                    If strTemp <> "" Then
                                        strTimestamp = strTemp.Split(CChar(" "))
                                        strTimestampParts = strTimestamp(0).Split(CChar("-"))
                                        dteTimestamp = DateSerial(CInt(strTimestampParts(0)), CInt(strTimestampParts(1)), CInt(strTimestampParts(2)))
                                        strTimestampParts = strTimestamp(1).Split(CChar(":"))
                                        dteTimestamp = dteTimestamp.AddHours(CDbl(strTimestampParts(0))).AddMinutes(CDbl(strTimestampParts(1))).AddSeconds(CDbl(strTimestampParts(2)))
                                    Else
                                        ' nothing in Attribute log table so set a default value for the attribute modification date
                                        dteTimestamp = Date.Now.AddYears(-1)
                                    End If
                                Else
                                    m_LastError = "Unable to read Attribute timestamp from Magento - " & objMagento.LastError
                                    m_LastErrorMessage = "Unable to read Attribute timestamp from Magento - " & objMagento.LastErrorMessage
                                    Return False

                                End If
                                ' now get all attributes
                                bReturnValue = objMagento.GetAttributeList("")
                            Else
                                ' yes, get latest timestamp from db for previously read attributes
                                strTemp = Me.GetField("SELECT CONVERT(varchar, MAX(SourceLastUpdated_DEV000221),120) AS LastUpdate FROM LerrynImportExportMagentoAttributes_DEV000221 WHERE InstanceID_DEV000221 = '" & MagentoInstanceID.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing)
                                ' and get changes since then
                                bReturnValue = objMagento.GetAttributeList(strTemp)
                                ' set a default value for the attribute modification date
                                dteTimestamp = Date.Now.AddYears(-1)
                            End If
                            If bReturnValue Then
                                XMLAttributeList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                                For Each XMLAttributeNode In XMLAttributeList
                                    XMLAttribute = XDocument.Parse(XMLAttributeNode.ToString)
                                    XMLValueList = XMLAttribute.XPathSelectElements("item/item")
                                    rowAttribute = Nothing
                                    ' need something in the AttributeName so use . as empty value
                                    strName = "."
                                    ' need something in the AttributeType so use . as empty value
                                    strType = "."
                                    ' need something in the AttributeScope so use . as empty value
                                    strScope = "."
                                    dteAttributeModTimestamp = dteTimestamp
                                    bRequired = False
                                    For Each XMLValueNode In XMLValueList
                                        XMLTemp = XDocument.Parse(XMLValueNode.ToString)
                                        strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                                        Select Case m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                                            Case "attribute_id"
                                                bRecordFound = False
                                                For iLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Count - 1
                                                    If m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeID_DEV000221 = CInt(strTemp) AndAlso _
                                                        m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).InstanceID_DEV000221 = MagentoInstanceID Then
                                                        rowAttribute = m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop)
                                                        bRecordFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If Not bRecordFound Then
                                                    rowAttribute = m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.NewLerrynImportExportMagentoAttributes_DEV000221Row
                                                    rowAttribute.InstanceID_DEV000221 = MagentoInstanceID
                                                    rowAttribute.SourceAttributeID_DEV000221 = CInt(strTemp)
                                                    ' need something in the AttributeCode so use . as empty value
                                                    rowAttribute.AttributeCode_DEV000221 = "."
                                                    ' similarly with the Matrix or Filter usage 
                                                    rowAttribute.AttributeUseMatrixOrFilter_DEV000221 = "."
                                                    rowAttribute.SourceAttributeName_DEV000221 = strName
                                                    rowAttribute.SourceAttributeType_DEV000221 = strType
                                                    rowAttribute.SourceAttributeScope_DEV000221 = strScope
                                                    rowAttribute.SourceAttributeRequired_DEV000221 = bRequired
                                                    rowAttribute.SourceLastUpdated_DEV000221 = dteAttributeModTimestamp
                                                    m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.AddLerrynImportExportMagentoAttributes_DEV000221Row(rowAttribute)
                                                End If

                                            Case "code"
                                                If rowAttribute Is Nothing Then
                                                    strName = strTemp
                                                Else
                                                    If strTemp = "" Then
                                                        If rowAttribute.SourceAttributeName_DEV000221 <> "." Then
                                                            rowAttribute.SourceAttributeName_DEV000221 = "."
                                                            rowAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                        End If
                                                    ElseIf rowAttribute.SourceAttributeName_DEV000221 <> strTemp Then
                                                        rowAttribute.SourceAttributeName_DEV000221 = strTemp
                                                        rowAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                    End If
                                                End If

                                            Case "type"
                                                If rowAttribute Is Nothing Then
                                                    strName = strTemp
                                                Else
                                                    If strTemp = "" Then
                                                        If rowAttribute.SourceAttributeType_DEV000221 <> "." Then
                                                            rowAttribute.SourceAttributeType_DEV000221 = "."
                                                            rowAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                        End If
                                                    ElseIf rowAttribute.SourceAttributeType_DEV000221 <> strTemp Then
                                                        rowAttribute.SourceAttributeType_DEV000221 = strTemp
                                                        rowAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                    End If
                                                End If

                                            Case "required"
                                                If rowAttribute Is Nothing Then
                                                    bRequired = CBool(strTemp)
                                                Else
                                                    If rowAttribute.SourceAttributeRequired_DEV000221 <> CBool(strTemp) Then
                                                        rowAttribute.SourceAttributeRequired_DEV000221 = CBool(strTemp)
                                                        rowAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                    End If
                                                End If

                                            Case "scope"
                                                If rowAttribute Is Nothing Then
                                                    strName = strTemp
                                                Else
                                                    If strTemp = "" Then
                                                        If rowAttribute.SourceAttributeScope_DEV000221 <> "." Then
                                                            rowAttribute.SourceAttributeScope_DEV000221 = "."
                                                            rowAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                        End If
                                                    ElseIf rowAttribute.SourceAttributeScope_DEV000221 <> strTemp Then
                                                        rowAttribute.SourceAttributeScope_DEV000221 = strTemp
                                                        rowAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                    End If
                                                End If

                                            Case "attribute_modification_date"
                                                strTimestamp = strTemp.Split(CChar(" "))
                                                strTimestampParts = strTimestamp(0).Split(CChar("-"))
                                                dteAttributeModTimestamp = DateSerial(CInt(strTimestampParts(0)), CInt(strTimestampParts(1)), CInt(strTimestampParts(2)))
                                                strTimestampParts = strTimestamp(1).Split(CChar(":"))
                                                dteAttributeModTimestamp = dteAttributeModTimestamp.AddHours(CDbl(strTimestampParts(0))).AddMinutes(CDbl(strTimestampParts(1))).AddSeconds(CDbl(strTimestampParts(2)))
                                                ' is modification timestamp later then previous/default value ?
                                                If dteAttributeModTimestamp > dteTimestamp Then
                                                    ' yes, save for later use
                                                    dteTimestamp = dteAttributeModTimestamp
                                                End If
                                                If rowAttribute IsNot Nothing AndAlso rowAttribute.SourceLastUpdated_DEV000221 <> dteAttributeModTimestamp Then
                                                    rowAttribute.SourceLastUpdated_DEV000221 = dteAttributeModTimestamp
                                                End If
                                        End Select
                                    Next
                                Next
                                If Me.UpdateDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.TableName, _
                                    "CreateLerrynImportExportMagentoAttributes_DEV000221", "UpdateLerrynImportExportMagentoAttributes_DEV000221", "DeleteLerrynImportExportMagentoAttributes_DEV000221"}}, _
                                    Interprise.Framework.Base.Shared.TransactionType.None, "Update Attribute List", False) Then

                                    ' get/update attribute values
                                    Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.TableName, _
                                        "ReadLerrynImportExportMagentoAttributeValues_DEV000221", AT_INSTANCE_ID, MagentoInstanceID}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                    If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Count = 0 Then
                                        bReturnValue = objMagento.GetAllAttributeValues("")
                                    Else
                                        strTemp = Me.GetField("SELECT CONVERT(varchar, MAX(SourceLastUpdated_DEV000221),120) AS LastUpdate FROM LerrynImportExportMagentoAttributeValues_DEV000221 WHERE InstanceID_DEV000221 = '" & MagentoInstanceID.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing)
                                        bReturnValue = objMagento.GetAllAttributeValues(strTemp)
                                    End If
                                    If bReturnValue Then
                                        XMLNameTabMagento = New System.Xml.NameTable
                                        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                                        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                                        XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                                        XMLAttributeList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                                        For Each XMLAttributeNode In XMLAttributeList
                                            XMLAttribute = XDocument.Parse(XMLAttributeNode.ToString)
                                            XMLValueList = XMLAttribute.XPathSelectElements("item/item")
                                            ' need something in the AttributeCode so use . as empty value
                                            strAttributeCode = "."
                                            ' similarly with the Matrix or Filter usage 
                                            strAttributeUse = "."
                                            dteAttributeModTimestamp = dteTimestamp
                                            For Each XMLValueNode In XMLValueList
                                                XMLTemp = XDocument.Parse(XMLValueNode.ToString)
                                                strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                                                strKey = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/key")
                                                Select Case strKey
                                                    Case "attribute_id"
                                                        iAttributeID = CInt(strTemp)
                                                        ' has attribute already been matched ?
                                                        For iLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Count - 1
                                                            If m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeID_DEV000221 = CInt(strTemp) AndAlso _
                                                                m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).InstanceID_DEV000221 = MagentoInstanceID Then
                                                                ' found Attribute - save attribute code and use for attribute values
                                                                strAttributeCode = m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeCode_DEV000221
                                                                strAttributeUse = m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221
                                                                Exit For
                                                            End If
                                                        Next

                                                    Case "options"
                                                        XMLOptionList = XMLTemp.XPathSelectElements("item/value/item")
                                                        For Each XMLOptionNode In XMLOptionList
                                                            XMLAttributeValue = XDocument.Parse(XMLOptionNode.ToString)
                                                            XMLOptionValueList = XMLAttributeValue.XPathSelectElements("item/item")
                                                            rowAttributeValue = Nothing
                                                            strLabel = ""
                                                            For Each XMLOptionValueNode In XMLOptionValueList
                                                                XMLOptionValue = XDocument.Parse(XMLOptionValueNode.ToString)
                                                                strTemp = m_MagentoImportRule.GetXMLElementText(XMLOptionValue, "item/value")
                                                                strKey = m_MagentoImportRule.GetXMLElementText(XMLOptionValue, "item/key")
                                                                XMLOptionAttributeList = XMLOptionValue.XPathSelectElement("item/value").Attributes
                                                                bOptionValuesAreArray = False
                                                                For Each XMLOptionAttribute In XMLOptionAttributeList
                                                                    If XMLOptionAttribute.Name.LocalName = "type" AndAlso XMLOptionAttribute.Value = "SOAP-ENC:Array" Then
                                                                        bOptionValuesAreArray = True
                                                                    End If
                                                                Next
                                                                If Not bOptionValuesAreArray Then
                                                                    Select Case strKey
                                                                        Case "value"
                                                                            bRecordFound = False
                                                                            For iLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Count - 1
                                                                                If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iLoop).RowState <> DataRowState.Deleted Then ' TJS 09/12/13
                                                                                    If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iLoop).SourceAttributeID_DEV000221 = iAttributeID AndAlso _
                                                                                        m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iLoop).SourceValueID_DEV000221 = strTemp AndAlso _
                                                                                        m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iLoop).InstanceID_DEV000221 = MagentoInstanceID Then
                                                                                        rowAttributeValue = m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iLoop)
                                                                                        bRecordFound = True
                                                                                        Exit For
                                                                                    End If
                                                                                End If
                                                                            Next
                                                                            If Not bRecordFound Then
                                                                                rowAttributeValue = m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.NewLerrynImportExportMagentoAttributeValues_DEV000221Row
                                                                                rowAttributeValue.InstanceID_DEV000221 = MagentoInstanceID
                                                                                rowAttributeValue.SourceAttributeID_DEV000221 = iAttributeID
                                                                                rowAttributeValue.SourceValueID_DEV000221 = strTemp
                                                                                rowAttributeValue.AttributeCode_DEV000221 = strAttributeCode
                                                                                rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = strAttributeUse
                                                                                ' need something in the AttributeValueCode so use . as empty value
                                                                                rowAttributeValue.AttributeValueCode_DEV000221 = "."
                                                                                rowAttributeValue.SourceValueSetting_DEV000221 = strLabel
                                                                                rowAttributeValue.SourceLastUpdated_DEV000221 = dteAttributeModTimestamp
                                                                                ' is modification timestamp later then previous/default value ?
                                                                                If dteAttributeModTimestamp > dteTimestamp Then
                                                                                    ' yes, save for later use
                                                                                    dteTimestamp = dteAttributeModTimestamp
                                                                                End If
                                                                                m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.AddLerrynImportExportMagentoAttributeValues_DEV000221Row(rowAttributeValue)

                                                                            Else
                                                                                ' does attribute code need updating ?
                                                                                If rowAttributeValue.AttributeCode_DEV000221.ToLower <> strAttributeCode.ToLower Then
                                                                                    ' yes, set attribute code and use 
                                                                                    rowAttributeValue.AttributeCode_DEV000221 = strAttributeCode
                                                                                    rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = strAttributeUse
                                                                                End If
                                                                                If rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = "." And strAttributeUse <> "." Then
                                                                                    rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = strAttributeUse
                                                                                End If
                                                                            End If

                                                                        Case "label"
                                                                            If rowAttributeValue Is Nothing Then
                                                                                strLabel = strTemp
                                                                            Else
                                                                                If rowAttributeValue.SourceValueSetting_DEV000221 <> strTemp Then
                                                                                    rowAttributeValue.SourceValueSetting_DEV000221 = strTemp
                                                                                    rowAttributeValue.SourceLastUpdated_DEV000221 = dteTimestamp
                                                                                End If
                                                                                ' do we have an Attribute Code but not an Attribute Value Code ?
                                                                                If rowAttributeValue.AttributeValueCode_DEV000221 = "." And rowAttributeValue.AttributeCode_DEV000221 <> "." Then
                                                                                    ' yes, 
                                                                                    If rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = "M" Then
                                                                                        strTemp = "AttributeCode = '" & rowAttributeValue.AttributeCode_DEV000221 & "' AND LanguageCode = '"
                                                                                        strTemp = strTemp & Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing) & "' AND AttributeValueDescription = '"
                                                                                        strTemp = strTemp & rowAttributeValue.SourceValueSetting_DEV000221.Replace("'", "''") & "'"
                                                                                        strTemp = Me.GetField("AttributeValueCode", "SystemAttributeValue", strTemp)

                                                                                    ElseIf rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = "F" Then
                                                                                        strTemp = "AttributeSourceFilterCode = '" & rowAttributeValue.AttributeCode_DEV000221 & "' AND LanguageCode = '"
                                                                                        strTemp = strTemp & Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing) & "' AND SourceFilterDescription = '"
                                                                                        strTemp = strTemp & rowAttributeValue.SourceValueSetting_DEV000221.Replace("'", "''") & "'"
                                                                                        strTemp = Me.GetField("SourceFilterName", "SystemItemAttributeSourceFilterValueDescription", strTemp)

                                                                                    ElseIf rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = "B" Then
                                                                                        strTemp = "AttributeCode = '" & rowAttributeValue.AttributeCode_DEV000221 & "' AND LanguageCode = '"
                                                                                        strTemp = strTemp & Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing) & "' AND AttributeValueDescription = '"
                                                                                        strTemp = strTemp & rowAttributeValue.SourceValueSetting_DEV000221.Replace("'", "''") & "'"
                                                                                        strTemp = Me.GetField("AttributeValueCode", "SystemAttributeValue", strTemp)
                                                                                        If String.IsNullOrEmpty(strTemp) Then
                                                                                            strTemp = "AttributeSourceFilterCode = '" & rowAttributeValue.AttributeCode_DEV000221 & "' AND LanguageCode = '"
                                                                                            strTemp = strTemp & Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing) & "' AND SourceFilterDescription = '"
                                                                                            strTemp = strTemp & rowAttributeValue.SourceValueSetting_DEV000221.Replace("'", "''") & "'"
                                                                                            strTemp = Me.GetField("SourceFilterName", "SystemItemAttributeSourceFilterValueDescription", strTemp)
                                                                                        End If
                                                                                    End If
                                                                                    If Not String.IsNullOrEmpty(strTemp) Then
                                                                                        rowAttributeValue.AttributeValueCode_DEV000221 = strTemp
                                                                                    End If
                                                                                End If
                                                                            End If
                                                                    End Select

                                                                End If
                                                            Next
                                                            ' start of code added TJS 09/12/13
                                                            If rowAttributeValue.RowState = DataRowState.Added Then

                                                            Else
                                                                For iCheckDuplicates = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Count - 1
                                                                    If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).RowState <> DataRowState.Deleted Then
                                                                        If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).InstanceID_DEV000221 = rowAttributeValue.InstanceID_DEV000221 AndAlso _
                                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).AttributeCode_DEV000221 = rowAttributeValue.AttributeCode_DEV000221 AndAlso _
                                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).AttributeValueCode_DEV000221 = rowAttributeValue.AttributeValueCode_DEV000221 AndAlso _
                                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).AttributeUseMatrixOrFilter_DEV000221 = rowAttributeValue.AttributeUseMatrixOrFilter_DEV000221 AndAlso _
                                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).SourceAttributeID_DEV000221 = rowAttributeValue.SourceAttributeID_DEV000221 AndAlso _
                                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).SourceValueID_DEV000221 = rowAttributeValue.SourceValueID_DEV000221 AndAlso _
                                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).Counter <> rowAttributeValue.Counter Then
                                                                            rowAttributeValue.Delete()
                                                                            Exit For
                                                                        End If
                                                                    End If
                                                                Next
                                                            End If
                                                            ' start of code added TJS 09/12/13
                                                        Next

                                                    Case "attribute_modification_date"
                                                        strTimestamp = strTemp.Split(CChar(" "))
                                                        strTimestampParts = strTimestamp(0).Split(CChar("-"))
                                                        dteAttributeModTimestamp = DateSerial(CInt(strTimestampParts(0)), CInt(strTimestampParts(1)), CInt(strTimestampParts(2)))
                                                        strTimestampParts = strTimestamp(1).Split(CChar(":"))
                                                        dteAttributeModTimestamp = dteAttributeModTimestamp.AddHours(CDbl(strTimestampParts(0))).AddMinutes(CDbl(strTimestampParts(1))).AddSeconds(CDbl(strTimestampParts(2)))
                                                        If rowAttributeValue.RowState <> DataRowState.Deleted AndAlso rowAttributeValue IsNot Nothing AndAlso _
                                                            rowAttributeValue.SourceLastUpdated_DEV000221 <> dteAttributeModTimestamp Then ' TJS 04/01/14
                                                            rowAttributeValue.SourceLastUpdated_DEV000221 = dteAttributeModTimestamp
                                                            ' is modification timestamp later then previous/default value ?
                                                            If dteAttributeModTimestamp > dteTimestamp Then
                                                                ' yes, save for later use
                                                                dteTimestamp = dteAttributeModTimestamp
                                                            End If
                                                        End If

                                                End Select
                                            Next
                                        Next
                                        If Not Me.UpdateDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.TableName, _
                                            "CreateLerrynImportExportMagentoAttributeValues_DEV000221", "UpdateLerrynImportExportMagentoAttributeValues_DEV000221", "DeleteLerrynImportExportMagentoAttributeValues_DEV000221"}}, _
                                            Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                                            ' get error details
                                            strErrorDetails = ""
                                            For iRowLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Rows.Count - 1
                                                For iColumnLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Columns.Count - 1
                                                    If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                        strErrorDetails = strErrorDetails & m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.TableName & _
                                                            "." & m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & " - " & _
                                                            m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iRowLoop)(m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Columns(iColumnLoop).ColumnName).ToString & vbCrLf
                                                    End If
                                                Next
                                            Next
                                            m_LastError = strErrorDetails
                                            m_LastErrorMessage = m_LastError
                                            GetMagentoAttributeList = False
                                        End If
                                    End If
                                Else
                                    ' get error details
                                    strErrorDetails = ""
                                    For iRowLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Columns.Count - 1
                                            If m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.TableName & _
                                                    "." & m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & " - " & _
                                                    m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iRowLoop)(m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Columns(iColumnLoop).ColumnName).ToString & vbCrLf
                                            End If
                                        Next
                                    Next
                                    m_LastError = strErrorDetails
                                    m_LastErrorMessage = m_LastError
                                    GetMagentoAttributeList = False
                                End If
                            Else
                                m_LastError = "Unable to read Attribute list from Magento - " & objMagento.LastError
                                m_LastErrorMessage = "Unable to read Attribute list from Magento - " & objMagento.LastErrorMessage
                                GetMagentoAttributeList = False
                            End If

                        Else
                            m_LastError = "Unable to connect to Magento - " & objMagento.LastError & vbCrLf & vbCrLf & "Please check your configuration settings."
                            m_LastErrorMessage = "Unable to connect to Magento - " & objMagento.LastErrorMessage
                            GetMagentoAttributeList = False
                        End If
                        Exit For ' TJS 01/04/14
                    End If
                Next
            End If

        Catch ex As Exception
            m_LastError = "Error getting Magento Attribute List - " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = "Error getting Magento Attribute List - " & ex.Message
            GetMagentoAttributeList = False

        End Try

    End Function
#End Region

#Region " SetMagentoAttributeID "
    Private Sub SetMagentoAttributeID(ByRef rowMagentoTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Checks the LerrynImportExportMagentoAttributes_DEV000221 and 
        '                   LerrynImportExportMagentoAttributeValues_DEV000221 for the attribute name
        '                   and sets the Attribute ID 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iValueLoop As Integer, bHasOptionValues As Boolean

        For iLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.Count - 1
            If m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeName_DEV000221 = rowMagentoTagDetails.TagName_DEV000221 Then
                If rowMagentoTagDetails.IsAttributeID_DEV000221Null OrElse rowMagentoTagDetails.AttributeID_DEV000221 <> _
                    m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeID_DEV000221 Then
                    rowMagentoTagDetails.AttributeID_DEV000221 = m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeID_DEV000221
                    bHasOptionValues = False
                    For iValueLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Count - 1
                        If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).SourceAttributeID_DEV000221 = rowMagentoTagDetails.AttributeID_DEV000221 Then
                            bHasOptionValues = True
                            Exit For
                        End If
                    Next
                    If rowMagentoTagDetails.AttributeHasSelectValues_DEV000221 <> bHasOptionValues Then
                        rowMagentoTagDetails.AttributeHasSelectValues_DEV000221 = bHasOptionValues
                    End If
                End If
                Exit For
            End If
        Next

    End Sub
#End Region

#Region " PublishAttributesAndValues "
    Public Function PublishAttributesAndValues(ByVal MagentoInstanceID As String, ByRef Cancel As Boolean, ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple magento sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoAttribute As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportMagentoAttributes_DEV000221Row
        Dim rowMagentoAttributeValue As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportMagentoAttributeValues_DEV000221Row
        Dim objMagento As Lerryn.Facade.ImportExport.MagentoSOAPConnector
        Dim XMLOptionList As System.Collections.Generic.IEnumerable(Of XElement), XMLOptionNode As XElement
        Dim AttributeToPublish As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType
        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim strAttributeBeingPublished As String = "", strTemp As String, strLabel As String
        Dim strValueID As String, strTimestamp As String(), strTimestampParts As String()
        Dim iAttributeLoop As Integer, iAttributeValueLoop As Integer, iCheckLoop As Integer
        Dim bInstanceMatched As Boolean, bMagentoV2APIWSI As Boolean, bPublishValues As Boolean
        Dim bValuesPublished As Boolean, bReturnValue As Boolean, bPublishAttribute As Boolean
        Dim dteTimestamp As Date

        Try
            m_LastErrorMessage = ""
            m_LastError = ""
            ErrorDetails = ""
            m_ProcessLog = ""
            bReturnValue = True
            If m_MagentoImportRule.IsActivated Then
                XMLConfig = XDocument.Parse(Trim(m_MagentoImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                bInstanceMatched = False
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If m_MagentoImportRule.GetXMLElementListCount(XMLNodeList) = 1 Then
                        bInstanceMatched = True
                    ElseIf m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoInstanceID Then
                        bInstanceMatched = True
                    End If
                    If bInstanceMatched Then
                        bMagentoV2APIWSI = CBool(IIf(UCase(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False))
                        objMagento = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
                        objMagento.V2SoapAPIWSICompliant = bMagentoV2APIWSI
                        objMagento.MagentoVersion = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION)
                        strTemp = m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION)
                        If strTemp <> "" Then
                            objMagento.LerrynAPIVersion = CDec(strTemp)
                        Else
                            objMagento.LerrynAPIVersion = 0
                        End If
                        If objMagento.Login(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                            m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                            m_MagentoImportRule.ConvertForXML(m_MagentoImportRule.DecodeConfigXMLValue(m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)))) Then
                            XMLNameTabMagento = New System.Xml.NameTable
                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento")

                            If objMagento.GetAttributeTimestamp() Then
                                strTemp = m_MagentoImportRule.GetXMLElementText(objMagento.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item/item/value", XMLNSManMagento)
                                If strTemp <> "" Then
                                    strTimestamp = strTemp.Split(CChar(" "))
                                    strTimestampParts = strTimestamp(0).Split(CChar("-"))
                                    dteTimestamp = DateSerial(CInt(strTimestampParts(0)), CInt(strTimestampParts(1)), CInt(strTimestampParts(2)))
                                    strTimestampParts = strTimestamp(1).Split(CChar(":"))
                                    dteTimestamp = dteTimestamp.AddHours(CDbl(strTimestampParts(0))).AddMinutes(CDbl(strTimestampParts(1))).AddSeconds(CDbl(strTimestampParts(2)))
                                Else
                                    ' nothing in Attribute log table so set a default value for the attribute modification date
                                    dteTimestamp = Date.Now.AddYears(-1)
                                End If
                            Else
                                m_LastError = "Unable to read Attribute timestamp from Magento - " & objMagento.LastError
                                m_LastErrorMessage = "Unable to read Attribute timestamp from Magento - " & objMagento.LastErrorMessage
                                m_ProcessLog = m_ProcessLog & m_LastErrorMessage
                                Return False

                            End If

                            For iAttributeLoop = 0 To m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221.Count - 1
                                strAttributeBeingPublished = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeCode
                                If m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).Publish Then
                                    If m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).IsSourceAttributeID_DEV000221Null OrElse _
                                        m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221 < 0 Then
                                        ' check for existing magento attributes with same name (happens if attribute used for both matrix items and search in CB
                                        bPublishAttribute = True
                                        For iCheckLoop = 0 To m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221.Count - 1
                                            If m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iCheckLoop).SourceAttributeName_DEV000221 = _
                                                m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeName_DEV000221 AndAlso _
                                                Not m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iCheckLoop).IsSourceAttributeID_DEV000221Null AndAlso _
                                                m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iCheckLoop).SourceAttributeID_DEV000221 >= 0 Then
                                                bPublishAttribute = False
                                                bPublishValues = True
                                                m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221 = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iCheckLoop).SourceAttributeID_DEV000221
                                                ' doess Attribute Use need updating ?
                                                If m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iCheckLoop).AttributeUseMatrixOrFilter_DEV000221 <> _
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221 Then
                                                    ' yes - can't update view mapped view so get base table record
                                                    With m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iCheckLoop)
                                                        rowMagentoAttribute = m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.FindByInstanceID_DEV000221AttributeCode_DEV000221AttributeUseMatrixOrFilter_DEV000221SourceAttributeID_DEV000221(.InstanceID_DEV000221, .AttributeCode_DEV000221, .AttributeUseMatrixOrFilter_DEV000221, .SourceAttributeID_DEV000221)
                                                        If rowMagentoAttribute IsNot Nothing Then
                                                            rowMagentoAttribute.AttributeUseMatrixOrFilter_DEV000221 = "B"
                                                        End If
                                                    End With
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221 = "B"
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iCheckLoop).AttributeUseMatrixOrFilter_DEV000221 = "B"
                                                End If
                                            End If
                                        Next
                                        If bPublishAttribute Then
                                            bPublishValues = False
                                            AttributeToPublish = New Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType
                                            AttributeToPublish.AttributeName = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeName_DEV000221
                                            AttributeToPublish.AttributeScope = "global"
                                            AttributeToPublish.AttributeType = "select"
                                            AttributeToPublish.AttributeDescription = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeDescription
                                            AttributeToPublish.UsedForConfigurables = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).UsedInMatrixGroups
                                            If objMagento.CreateProductAttribute(AttributeToPublish) Then
                                                ' get Magento Attribute ID
                                                If bMagentoV2APIWSI Then
                                                    strTemp = m_MagentoImportRule.GetXMLElementText(objMagento.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductAttributeCreateResponseParam/result", XMLNSManMagento)
                                                Else
                                                    strTemp = m_MagentoImportRule.GetXMLElementText(objMagento.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductAttributeCreateResponse/result", XMLNSManMagento)
                                                End If
                                                If strTemp <> "" Then
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeType_DEV000221 = AttributeToPublish.AttributeType
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeScope_DEV000221 = AttributeToPublish.AttributeScope
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221 = CInt(strTemp)

                                                    rowMagentoAttribute = m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.NewLerrynImportExportMagentoAttributes_DEV000221Row
                                                    rowMagentoAttribute.InstanceID_DEV000221 = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).InstanceID_DEV000221
                                                    rowMagentoAttribute.AttributeCode_DEV000221 = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeCode_DEV000221
                                                    rowMagentoAttribute.AttributeUseMatrixOrFilter_DEV000221 = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221
                                                    rowMagentoAttribute.SourceAttributeID_DEV000221 = CInt(strTemp)
                                                    rowMagentoAttribute.SourceAttributeName_DEV000221 = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeName_DEV000221
                                                    rowMagentoAttribute.SourceAttributeType_DEV000221 = AttributeToPublish.AttributeType
                                                    rowMagentoAttribute.SourceAttributeRequired_DEV000221 = AttributeToPublish.AttributeReqd
                                                    rowMagentoAttribute.SourceAttributeScope_DEV000221 = AttributeToPublish.AttributeScope
                                                    rowMagentoAttribute.SourceLastUpdated_DEV000221 = dteTimestamp
                                                    m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.AddLerrynImportExportMagentoAttributes_DEV000221Row(rowMagentoAttribute)

                                                    bPublishValues = True
                                                    Me.UpdateDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynImportExportMagentoAttributes_DEV000221.TableName, _
                                                        "CreateLerrynImportExportMagentoAttributes_DEV000221", "UpdateLerrynImportExportMagentoAttributes_DEV000221", "DeleteLerrynImportExportMagentoAttributes_DEV000221"}}, _
                                                        Interprise.Framework.Base.Shared.TransactionType.None, "Publish Magento Attribute", False)
                                                End If
                                            Else
                                                ' get Magento Error details
                                                strTemp = m_MagentoImportRule.GetXMLElementText(objMagento.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento)
                                                If strTemp <> "" Then
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SetColumnError("SourceAttributeName_DEV000221", strTemp)
                                                    m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SetColumnError("SourceAttributeID_DEV000221", strTemp)
                                                    m_ProcessLog = m_ProcessLog & "Attribute " & strAttributeBeingPublished & " - " & strTemp & vbCrLf
                                                    bReturnValue = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        ' attribute already exists, just publish values
                                        bPublishValues = True
                                    End If
                                    ' now publish attribute values (unless attribute failed to create
                                    If bPublishValues Then
                                        If m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221 = "M" Then
                                            Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221.TableName, _
                                                "ReadSystemAttributeValueMagentoMappedView_DEV000221", AT_IS_ACTIVE, "1", AT_INSTANCE_ID, MagentoInstanceID, _
                                                AT_ATTRIBUTE_CODE, m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeCode, _
                                                AT_LANGUAGE_CODE, Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing), AT_ATTRIBUTE_USAGE, "M", AT_ATTRIBUTE_ALT_USAGE, "B"}}, _
                                                Interprise.Framework.Base.Shared.ClearType.Specific)

                                        ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221 = "F" Then
                                            Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221.TableName, _
                                                "ReadSystemAttributeValueMagentoMappedView_DEV000221", AT_IS_ACTIVE, "1", AT_INSTANCE_ID, MagentoInstanceID, _
                                                AT_ATTRIBUTE_CODE, m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeCode, _
                                                AT_LANGUAGE_CODE, Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing), AT_ATTRIBUTE_USAGE, "F", AT_ATTRIBUTE_ALT_USAGE, "B"}}, _
                                                Interprise.Framework.Base.Shared.ClearType.Specific)

                                        ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221 = "B" Then
                                            Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221.TableName, _
                                                "ReadSystemAttributeValueMagentoMappedView_DEV000221", AT_IS_ACTIVE, "1", AT_INSTANCE_ID, MagentoInstanceID, _
                                                AT_ATTRIBUTE_CODE, m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeCode, _
                                                AT_LANGUAGE_CODE, Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)}}, _
                                                Interprise.Framework.Base.Shared.ClearType.Specific)

                                        Else
                                            m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221.Clear()
                                        End If
                                        bValuesPublished = False
                                        For iAttributeValueLoop = 0 To m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221.Count - 1
                                            If m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).IsSourceValueID_DEV000221Null OrElse _
                                                m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).IsSourceValueID_DEV000221Null Then
                                                If objMagento.AddOptionValueToProductAttribute(m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221, _
                                                     m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeValueDescription, iAttributeValueLoop + 1) Then
                                                    bValuesPublished = True
                                                End If

                                            End If
                                        Next
                                        If bValuesPublished Then
                                            If objMagento.GetSingleAttributeValues(m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221) Then
                                                If bMagentoV2APIWSI Then
                                                    XMLOptionList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductAttributeInfoResponseParam/result/options/complexObjectArray", XMLNSManMagento)
                                                Else
                                                    XMLOptionList = objMagento.ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductAttributeInfoResponse/result/options/item", XMLNSManMagento)
                                                End If
                                                For Each XMLOptionNode In XMLOptionList
                                                    XMLTemp = XDocument.Parse(XMLOptionNode.ToString)
                                                    If bMagentoV2APIWSI Then
                                                        strLabel = m_MagentoImportRule.GetXMLElementText(XMLTemp, "complexObjectArray/label")
                                                        strValueID = m_MagentoImportRule.GetXMLElementText(XMLTemp, "complexObjectArray/value")
                                                    Else
                                                        strLabel = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/label")
                                                        strValueID = m_MagentoImportRule.GetXMLElementText(XMLTemp, "item/value")
                                                    End If
                                                    For iAttributeValueLoop = 0 To m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221.Count - 1
                                                        If m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeValueDescription.Trim = strLabel And _
                                                            (m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).IsSourceValueID_DEV000221Null OrElse _
                                                             m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).SourceValueID_DEV000221 = strValueID) And _
                                                            (m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeUseMatrixOrFilter_DEV000221 = _
                                                            m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221 OrElse m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeUseMatrixOrFilter_DEV000221 = "B" OrElse m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).AttributeUseMatrixOrFilter_DEV000221 = "B") Then
                                                            ' matching value record found
                                                            With m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop)
                                                                rowMagentoAttributeValue = m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.FindByInstanceID_DEV000221AttributeCode_DEV000221AttributeValueCode_DEV000221AttributeUseMatrixOrFilter_DEV000221SourceAttributeID_DEV000221SourceValueID_DEV000221(MagentoInstanceID, .AttributeCode, .AttributeValueCode, .AttributeUseMatrixOrFilter_DEV000221, m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221, strValueID)
                                                                ' did we find matching row ?
                                                                If rowMagentoAttributeValue Is Nothing Then
                                                                    ' no, try with 
                                                                    rowMagentoAttributeValue = m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.FindByInstanceID_DEV000221AttributeCode_DEV000221AttributeValueCode_DEV000221AttributeUseMatrixOrFilter_DEV000221SourceAttributeID_DEV000221SourceValueID_DEV000221(MagentoInstanceID, .AttributeCode, ".", .AttributeUseMatrixOrFilter_DEV000221, m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221, strValueID)
                                                                End If
                                                            End With
                                                            If rowMagentoAttributeValue Is Nothing Then
                                                                rowMagentoAttributeValue = m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.NewLerrynImportExportMagentoAttributeValues_DEV000221Row
                                                                rowMagentoAttributeValue.InstanceID_DEV000221 = MagentoInstanceID
                                                                rowMagentoAttributeValue.AttributeCode_DEV000221 = m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeCode
                                                                rowMagentoAttributeValue.AttributeValueCode_DEV000221 = m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeValueCode
                                                                rowMagentoAttributeValue.AttributeUseMatrixOrFilter_DEV000221 = m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeUseMatrixOrFilter_DEV000221
                                                                rowMagentoAttributeValue.SourceAttributeID_DEV000221 = m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iAttributeLoop).SourceAttributeID_DEV000221
                                                                rowMagentoAttributeValue.SourceValueID_DEV000221 = strValueID
                                                                rowMagentoAttributeValue.SourceValueSetting_DEV000221 = m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeValueDescription
                                                                rowMagentoAttributeValue.SourceLastUpdated_DEV000221 = dteTimestamp
                                                                m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.AddLerrynImportExportMagentoAttributeValues_DEV000221Row(rowMagentoAttributeValue)
                                                            End If

                                                            m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeCode_DEV000221 = rowMagentoAttributeValue.AttributeCode_DEV000221
                                                            m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeValueCode_DEV000221 = rowMagentoAttributeValue.AttributeValueCode_DEV000221
                                                            m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).AttributeUseMatrixOrFilter_DEV000221 = rowMagentoAttributeValue.AttributeUseMatrixOrFilter_DEV000221
                                                            m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).SourceAttributeID_DEV000221 = rowMagentoAttributeValue.SourceAttributeID_DEV000221
                                                            m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).SourceValueID_DEV000221 = rowMagentoAttributeValue.SourceValueID_DEV000221
                                                            m_MagentoImportDataset.SystemAttributeValueMagentoMappedView_DEV000221(iAttributeValueLoop).SourceValueSetting_DEV000221 = rowMagentoAttributeValue.SourceValueSetting_DEV000221
                                                            Exit For
                                                        End If
                                                    Next
                                                Next
                                                If Not Me.UpdateDataSet(New String()() {New String() {m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.TableName, _
                                                    "CreateLerrynImportExportMagentoAttributeValues_DEV000221", "UpdateLerrynImportExportMagentoAttributeValues_DEV000221", "DeleteLerrynImportExportMagentoAttributeValues_DEV000221"}}, _
                                                    Interprise.Framework.Base.Shared.TransactionType.None, "Publish Magento Attributes and Values", False) Then
                                                    ' get error details
                                                    strTemp = ""
                                                    For iRowLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Rows.Count - 1
                                                        For iColumnLoop = 0 To m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Columns.Count - 1
                                                            If m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                                strTemp = strTemp & m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.TableName & _
                                                                    "." & m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                                    m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & " - " & _
                                                                    m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iRowLoop)(m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221.Columns(iColumnLoop).ColumnName).ToString & _
                                                                    " for attribute code " & m_MagentoImportDataset.LerrynImportExportMagentoAttributeValues_DEV000221(iRowLoop).AttributeCode_DEV000221 & vbCrLf
                                                            End If
                                                        Next
                                                    Next
                                                    m_LastError = strTemp
                                                    m_LastErrorMessage = m_LastError
                                                    Return False
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            m_LastError = "Unable to connect to Magento - " & objMagento.LastError & vbCrLf & vbCrLf & "Please check your configuration settings."
                            m_LastErrorMessage = "Unable to connect to Magento - " & objMagento.LastErrorMessage
                            m_ProcessLog = m_ProcessLog & m_LastErrorMessage
                            bReturnValue = False
                        End If
                        Exit For ' TJS 01/04/14

                    End If

                Next
            End If

        Catch ex As Exception
            m_LastError = "Error whilst publishing Attribute " & strAttributeBeingPublished & " - " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = "Error whilst publishing Attribute " & strAttributeBeingPublished & " - " & ex.Message
            ErrorDetails = ErrorDetails & "Error whilst publishing Attribute " & strAttributeBeingPublished & " - " & ex.Message
            m_ProcessLog = m_ProcessLog & m_LastErrorMessage
            bReturnValue = False
        End Try
        Return bReturnValue

    End Function
#End Region

#Region " PublishBulkInventoryItems "
    Public Sub PublishBulkInventoryItems(ByVal MagentoInstanceID As String, ByRef ItemsToPublish As DataView, ByRef Cancel As Boolean, _
        ByVal p_BaseProductCode As String, ByVal p_BaseProductName As String, ByRef MagentoCategoryTable As System.Data.DataTable, _
        ByRef PublishingOptions As Lerryn.Facade.ImportExport.MagentoImportFacade.MagentoPublishingOptions, _
        ByRef NoOfItemsProcessed As Integer, ByRef NoOfItemsPublished As Integer, ByRef NoOfItemsSkipped As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -      Publish a list of items using Attribute Values as entered by the user
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Promotional pricing option on special price
        '                                        | and to read attribute values.  Corrected string constants used to 
        '                                        | indicate product name and description are taken from InventoryItem table
        ' 15/11/13 | TJS             | 2013.3.09 | Modified to pass options vis MagentoPublishingOptions including an option to pick up Web descriptions and weight
        ' 20/11/13 | TJS             | 2013.4.00 | Modified to cater for Web Option link
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple magento sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim TempFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
        Dim TempDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Dim rowMagentoDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoDetails_DEV000221Row
        Dim rowMagentoTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row
        Dim rowMagentoCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoCategories_DEV000221Row
        Dim rowItemAttributes As System.Data.DataRow ' TJS 13/11/13
        Dim strHomeCurrency As String, strErrorDetails As String, strSKUBeingProcessed As String = "", strSQL As String ' TJS 13/11/13
        Dim strTemp As String, strPrices() As String, strItemCategories As String()(), strItemCategory As String() ' TJS 13/11/13 TJS 15/11/13
        Dim iAttributeLoop As Integer, iCategoryLoop As Integer
        Dim bInstanceMatched As Boolean

        Try
            m_LastErrorMessage = ""
            m_LastError = ""
            m_ProcessLog = ""
            If m_MagentoImportRule.IsActivated Then
                strErrorDetails = ""
                strHomeCurrency = Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                XMLConfig = XDocument.Parse(Trim(m_MagentoImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                bInstanceMatched = False
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If m_MagentoImportRule.GetXMLElementListCount(XMLNodeList) = 1 Then
                        bInstanceMatched = True
                    ElseIf m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoInstanceID Then
                        bInstanceMatched = True
                    End If
                    If bInstanceMatched Then
                        TempDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
                        TempFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(TempDataset, New Lerryn.Facade.ImportExport.ErrorNotification, p_BaseProductCode, p_BaseProductName)
                        For iItemLoop = 0 To ItemsToPublish.Count - 1
                            strSKUBeingProcessed = ItemsToPublish(iItemLoop).Item("ItemName").ToString
                            If CBool(ItemsToPublish(iItemLoop).Item("Select")) Then
                                TempDataset.Clear()
                                strPrices = Me.GetRow(New String() {"WholesalePriceRate", "RetailPriceRate", "SuggestedRetailPriceRate"}, "InventoryItemPricingDetail", "ItemCode = '" & ItemsToPublish(iItemLoop).Item("ItemCode").ToString & "' and CurrencyCode = '" & strHomeCurrency & "'")
                                rowMagentoDetails = TempDataset.InventoryMagentoDetails_DEV000221.NewInventoryMagentoDetails_DEV000221Row
                                rowMagentoDetails.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                rowMagentoDetails.InstanceID_DEV000221 = MagentoInstanceID
                                rowMagentoDetails.Publish_DEV000221 = True
                                rowMagentoDetails.FromImportWizard_DEV000221 = False
                                rowMagentoDetails.SourceIsGroupItem_DEV000221 = False
                                rowMagentoDetails.AttributeSetID_DEV000221 = PublishingOptions.AttributeSetID ' TJS 15/11/13
                                If PublishingOptions.MagentoPriceSource IsNot Nothing Then ' TJS 15/11/13
                                    Select Case PublishingOptions.MagentoPriceSource.ToString ' TJS 15/11/13
                                        Case "W" ' TJS 13/11/13
                                            rowMagentoDetails.SellingPrice_DEV000221 = CDec(strPrices(0))
                                            rowMagentoDetails.SellingPriceSource_DEV000221 = "W" ' TJS 13/11/13
                                        Case "R" ' TJS 13/11/13
                                            rowMagentoDetails.SellingPrice_DEV000221 = CDec(strPrices(1))
                                            rowMagentoDetails.SellingPriceSource_DEV000221 = "R" ' TJS 13/11/13
                                        Case "S" ' TJS 13/11/13
                                            rowMagentoDetails.SellingPrice_DEV000221 = CDec(strPrices(2))
                                            rowMagentoDetails.SellingPriceSource_DEV000221 = "S" ' TJS 13/11/13
                                        Case Else
                                            rowMagentoDetails.SellingPrice_DEV000221 = 0
                                    End Select
                                End If
                                If PublishingOptions.MagentoSpecialPriceSource IsNot Nothing Then ' TJS 15/11/13
                                    Select Case PublishingOptions.MagentoSpecialPriceSource.ToString ' TJS 15/11/13
                                        Case "W" ' TJS 13/11/13
                                            rowMagentoDetails.SpecialPrice_DEV000221 = CDec(strPrices(0))
                                            rowMagentoDetails.SpecialPriceSource_DEV000221 = "W" ' TJS 13/11/13
                                        Case "R" ' TJS 13/11/13
                                            rowMagentoDetails.SpecialPrice_DEV000221 = CDec(strPrices(1))
                                            rowMagentoDetails.SpecialPriceSource_DEV000221 = "R" ' TJS 13/11/13
                                        Case "P" ' TJS 13/11/13
                                            Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.InventorySpecialPricing.TableName, "ReadInventorySpecialPricing_DEV000221", _
                                                AT_ITEM_CODE, ItemsToPublish(iItemLoop).Item("ItemCode").ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                            For iPriceLoop = 0 To m_MagentoImportDataset.InventorySpecialPricing.Count - 1
                                                If (m_MagentoImportDataset.InventorySpecialPricing(iPriceLoop).DateFrom <= Date.Today And _
                                                    m_MagentoImportDataset.InventorySpecialPricing(iPriceLoop).DateTo >= Date.Today) Or _
                                                    m_MagentoImportDataset.InventorySpecialPricing(iPriceLoop).DateFrom > Date.Today And _
                                                    m_MagentoImportDataset.InventorySpecialPricing(iPriceLoop).UnitMeasureCode = "EACH" Then
                                                    rowMagentoDetails.SpecialPrice_DEV000221 = m_MagentoImportDataset.InventorySpecialPricing(iPriceLoop).SpecialPrice
                                                    rowMagentoDetails.ShowAsSpecialFrom_DEV000221 = m_MagentoImportDataset.InventorySpecialPricing(iPriceLoop).DateFrom
                                                    rowMagentoDetails.ShowAsSpecialTo_DEV000221 = m_MagentoImportDataset.InventorySpecialPricing(iPriceLoop).DateTo
                                                    Exit For
                                                End If
                                            Next
                                            rowMagentoDetails.SpecialPriceSource_DEV000221 = "P" ' TJS 13/11/13 TJS 04/01/14
                                    End Select
                                    If PublishingOptions.MagentoSpecialPriceSource.ToString = "W" Or PublishingOptions.MagentoSpecialPriceSource.ToString = "R" Then ' TJS 13/11/13 TJS 15/11/13
                                        If PublishingOptions.MagentoSpecialPriceFrom IsNot Nothing Then ' TJS 15/11/13
                                            rowMagentoDetails.ShowAsSpecialFrom_DEV000221 = CDate(PublishingOptions.MagentoSpecialPriceFrom) ' TJS 15/11/13
                                        End If
                                        If PublishingOptions.MagentoSpecialPriceTo IsNot Nothing Then ' TJS 15/11/13
                                            rowMagentoDetails.ShowAsSpecialTo_DEV000221 = CDate(PublishingOptions.MagentoSpecialPriceTo) ' TJS 15/11/13
                                        End If
                                    End If
                                End If
                                rowMagentoDetails.ProductName_DEV000221 = INVENTORY_USE_GENERAL_TAB_DESCRIPTION  ' TJS 13/11/13
                                ' start of code added TJS 15/11/13
                                If PublishingOptions.MagentoShortDescSource = "W" Then
                                    rowMagentoDetails.ProductShortDescription_DEV000221 = INVENTORY_USE_WEB_OPTION_TAB_SUMMARY  ' TJS 20/11/13
                                Else
                                    ' end of code added TJS 15/11/13
                                    rowMagentoDetails.ProductShortDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION  ' TJS 13/11/13
                                End If
                                If PublishingOptions.MagentoDescriptionSource = "W" Then ' TJS 15/11/13
                                    rowMagentoDetails.ProductDescription_DEV000221 = INVENTORY_USE_WEB_OPTION_TAB_DESCRIPTION ' TJS 15/11/13 TJS 20/11/13
                                ElseIf PublishingOptions.MagentoDescriptionSource = "D" Then ' TJS 20/11/13
                                    rowMagentoDetails.ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION ' TJS  20/11/13
                                End If
                                rowMagentoDetails.QtyPublishingType_DEV000221 = PublishingOptions.MagentoQtyPublishingOption ' TJS 15/11/13
                                If PublishingOptions.MagentoQtyPublishingOption = "Fixed" Or PublishingOptions.MagentoQtyPublishingOption = "Percent" Then ' TJS 15/11/13
                                    rowMagentoDetails.QtyPublishingValue_DEV000221 = CInt(PublishingOptions.MagentoQtyPublishingValue) ' TJS 15/11/13
                                End If
                                rowMagentoDetails.TotalQtyWhenLastPublished_DEV000221 = 0
                                rowMagentoDetails.QtyLastPublished_DEV000221 = 0
                                TempDataset.InventoryMagentoDetails_DEV000221.AddInventoryMagentoDetails_DEV000221Row(rowMagentoDetails)

                                For iAttributeLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Count - 1
                                    rowMagentoTagDetails = TempDataset.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                                    rowMagentoTagDetails.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                    rowMagentoTagDetails.InstanceID_DEV000221 = MagentoInstanceID
                                    rowMagentoTagDetails.TagName_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagName_DEV000221
                                    rowMagentoTagDetails.TagDisplayName_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagDisplayName_DEV000221
                                    rowMagentoTagDetails.LineNumber_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).LineNumber_DEV000221
                                    rowMagentoTagDetails.TagRequired_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagRequired_DEV000221
                                    rowMagentoTagDetails.TagDataType_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagDataType_DEV000221
                                    SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                                    If Not rowMagentoTagDetails.IsTagTextValue_DEV000221Null AndAlso rowMagentoTagDetails.TagTextValue_DEV000221 <> "" Then
                                        rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagTextValue_DEV000221
                                        Select Case rowMagentoTagDetails.TagDataType_DEV000221
                                            Case "Memo"
                                                If Not rowMagentoTagDetails.IsTagMemoValue_DEV000221Null Then
                                                    rowMagentoTagDetails.TagMemoValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagMemoValue_DEV000221
                                                End If
                                            Case "Date", "DateTime"
                                                If Not rowMagentoTagDetails.IsTagDateValue_DEV000221Null Then
                                                    rowMagentoTagDetails.TagDateValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagDateValue_DEV000221
                                                End If
                                            Case "Integer", "Numeric"
                                                If Not rowMagentoTagDetails.IsTagNumericValue_DEV000221Null Then
                                                    rowMagentoTagDetails.TagNumericValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagNumericValue_DEV000221
                                                End If
                                        End Select
                                    Else
                                        For iLoop = 0 To m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221.Count - 1
                                            If Not m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).IsSourceAttributeID_DEV000221Null AndAlso _
                                                m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeID_DEV000221 = rowMagentoTagDetails.AttributeID_DEV000221 Then
                                                If m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "M" Then
                                                    rowItemAttributes = Me.GetRow(New String() {"AttributeCode1", "AttributeCode2", "AttributeCode3", "AttributeCode4", "AttributeCode5", "AttributeCode6", "Attribute1", "Attribute2", "Attribute3", "Attribute4", "Attribute5", "Attribute6"}, "InventoryMatrixItem", "MatrixItemCode = '" & ItemsToPublish(iItemLoop).Item("ItemCode").ToString & "'", ConnectionStringType.Online)
                                                    If rowItemAttributes IsNot Nothing Then
                                                        If m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221 = rowItemAttributes.Item("AttributeCode1").ToString Then
                                                            rowMagentoTagDetails.TagTextValue_DEV000221 = rowItemAttributes.Item("Attribute1").ToString

                                                        ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221 = rowItemAttributes.Item("AttributeCode2").ToString Then
                                                            rowMagentoTagDetails.TagTextValue_DEV000221 = rowItemAttributes.Item("Attribute2").ToString

                                                        ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221 = rowItemAttributes.Item("AttributeCode3").ToString Then
                                                            rowMagentoTagDetails.TagTextValue_DEV000221 = rowItemAttributes.Item("Attribute3").ToString

                                                        ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221 = rowItemAttributes.Item("AttributeCode4").ToString Then
                                                            rowMagentoTagDetails.TagTextValue_DEV000221 = rowItemAttributes.Item("Attribute4").ToString

                                                        ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221 = rowItemAttributes.Item("AttributeCode5").ToString Then
                                                            rowMagentoTagDetails.TagTextValue_DEV000221 = rowItemAttributes.Item("Attribute5").ToString

                                                        ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221 = rowItemAttributes.Item("AttributeCode6").ToString Then
                                                            rowMagentoTagDetails.TagTextValue_DEV000221 = rowItemAttributes.Item("Attribute6").ToString

                                                        End If
                                                    End If

                                                ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "F" Then
                                                    strSQL = "ItemCode = '" & ItemsToPublish(iItemLoop).Item("ItemCode").ToString & "' AND LanguageCode = '" & Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
                                                    strSQL = strSQL & "' AND AttributeSourceFilterCode = '" & m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221.Replace("'", "''") & "'"
                                                    rowItemAttributes = Me.GetRow(New String() {"AttributeSourceFilterValue"}, "InventoryItemAttributeCategoryDetail", strSQL, ConnectionStringType.Online)
                                                    If rowItemAttributes IsNot Nothing Then
                                                        rowMagentoTagDetails.TagTextValue_DEV000221 = rowItemAttributes.Item("AttributeSourceFilterValue").ToString
                                                    End If

                                                ElseIf m_MagentoImportDataset.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "B" Then ' TJS 04/01/14
                                                    ' code added to enable detection of B values
                                                    strSQL = "" ' TJS 04/01/14
                                                End If
                                            End If
                                        Next
                                        ' start of code added TJS 15/11/13
                                        If rowMagentoTagDetails.TagName_DEV000221 = "weight" AndAlso (rowMagentoTagDetails.IsTagTextValue_DEV000221Null OrElse rowMagentoTagDetails.TagTextValue_DEV000221 = "") Then
                                            If PublishingOptions.MagentoWeightUnits = "L" Then
                                                rowMagentoTagDetails.TagTextValue_DEV000221 = Me.GetField("WeightInPounds", "InventoryUnitMeasure", "ItemCode = '" & ItemsToPublish(iItemLoop).Item("ItemCode").ToString & "' AND UnitMeasureCode = 'EACH'")
                                            ElseIf PublishingOptions.MagentoWeightUnits = "K" Then ' TJS 09/12/13
                                                rowMagentoTagDetails.TagTextValue_DEV000221 = Me.GetField("WeightInKilograms", "InventoryUnitMeasure", "ItemCode = '" & ItemsToPublish(iItemLoop).Item("ItemCode").ToString & "' AND UnitMeasureCode = 'EACH'")
                                            End If

                                        End If
                                        ' end of code added TJS 15/11/13
                                    End If
                                    TempDataset.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)
                                Next

                                ' start of code added TJS 13/11/13
                                If PublishingOptions.UseMappedCBCategories Then ' TJS 15/11/13
                                    strItemCategories = Me.GetRows(New String() {"CategoryCode"}, "InventoryCategory", "ItemCode = '" & ItemsToPublish(iItemLoop).Item("ItemCode").ToString & "' ORDER BY SortOrder", ConnectionStringType.Online)
                                    For Each strItemCategory In strItemCategories
                                        For iCategoryLoop = 0 To MagentoCategoryTable.Rows.Count - 1
                                            If MagentoCategoryTable.Rows(iCategoryLoop).Item("ISCategoryCode").ToString = strItemCategory(0) Then
                                                rowMagentoCategory = TempDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                                                rowMagentoCategory.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                                rowMagentoCategory.InstanceID_DEV000221 = MagentoInstanceID
                                                rowMagentoCategory.MagentoCategoryID_DEV000221 = CInt(MagentoCategoryTable.Rows(iCategoryLoop).Item("SourceCategoryID"))
                                                rowMagentoCategory.MagentoWebSiteID_DEV000221 = -1
                                                rowMagentoCategory.IsActive_DEV000221 = True
                                                TempDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory)
                                            End If
                                        Next
                                    Next
                                Else
                                    For iCategoryLoop = 0 To MagentoCategoryTable.Rows.Count - 1
                                        If CBool(MagentoCategoryTable.Rows(iCategoryLoop).Item("Active")) Then
                                            rowMagentoCategory = TempDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                                            rowMagentoCategory.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                            rowMagentoCategory.InstanceID_DEV000221 = MagentoInstanceID
                                            rowMagentoCategory.MagentoCategoryID_DEV000221 = CInt(MagentoCategoryTable.Rows(iCategoryLoop).Item("SourceCategoryID"))
                                            rowMagentoCategory.MagentoWebSiteID_DEV000221 = -1
                                            rowMagentoCategory.IsActive_DEV000221 = True
                                            TempDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory)
                                        End If
                                    Next
                                End If

                                rowMagentoCategory = TempDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                                rowMagentoCategory.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                rowMagentoCategory.InstanceID_DEV000221 = MagentoInstanceID
                                rowMagentoCategory.MagentoCategoryID_DEV000221 = -1
                                rowMagentoCategory.MagentoWebSiteID_DEV000221 = 1
                                rowMagentoCategory.IsActive_DEV000221 = True
                                TempDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory)
                                ' end of code added TJS 13/11/13

                                If TempFacade.UpdateDataSet(New String()() {New String() {TempDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                        "CreateInventoryMagentoDetails_DEV000221", "UpdateInventoryMagentoDetails_DEV000221", "DeleteInventoryMagentoDetails_DEV000221"}, _
                                    New String() {TempDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                        "CreateInventoryMagentoTagDetails_DEV000221", "UpdateInventoryMagentoTagDetails_DEV000221", "DeleteInventoryMagentoTagDetails_DEV000221"}, _
                                    New String() {TempDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                        "CreateInventoryMagentoCategories_DEV000221", "UpdateInventoryMagentoCategories_DEV000221", "DeleteInventoryMagentoCategories_DEV000221"}}, _
                                    Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Bulk Publish Inventory Magento Settings", False) Then
                                    NoOfItemsPublished += 1

                                Else
                                    ' get error details
                                    strErrorDetails = ""
                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Columns.Count - 1
                                            If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.TableName & _
                                                    "." & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Columns.Count - 1
                                            If m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.TableName & _
                                                    "." & m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Columns.Count - 1
                                            If m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.TableName & _
                                                    "." & m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    m_LastError = "Publishing of Item " & strSKUBeingProcessed & " aborted - " & strErrorDetails
                                    m_LastErrorMessage = m_LastError
                                    m_ProcessLog = m_ProcessLog & "Publishing of Item " & strSKUBeingProcessed & " aborted - " & strErrorDetails & vbCrLf
                                    NoOfItemsSkipped = NoOfItemsSkipped + 1


                                End If
                            End If
                            NoOfItemsProcessed += 1
                        Next
                        Exit For ' TJS 01/04/14
                    End If
                Next
                If Not bInstanceMatched Then
                    m_LastError = "Magento Instance not found."
                    m_LastErrorMessage = m_LastError
                    m_ProcessLog = m_ProcessLog & "Cannot connect to Magento" & vbCrLf
                End If
            End If

        Catch ex As Exception
            m_LastError = "Error whilst processing Item " & strSKUBeingProcessed & " - " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = "Error whilst processing Item " & strSKUBeingProcessed & " - " & ex.Message

        End Try

    End Sub

    Public Sub PublishBulkInventoryItems(ByVal MagentoInstanceID As String, ByRef ItemsToPublish As DataView, ByRef Cancel As Boolean, _
       ByVal p_BaseProductCode As String, ByVal p_BaseProductName As String, ByVal MagentoInstanceToCopy As String, _
       ByRef NoOfItemsProcessed As Integer, ByRef NoOfItemsPublished As Integer, ByRef NoOfItemsSkipped As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -       Publish a list of items using Attribute Values copied from another Magento Instance
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 01/04/14 | TJS             | 2014.0.01 | Modified to properly handle multiple magento sites
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim TempFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
        Dim TempDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Dim rowMagentoDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoDetails_DEV000221Row
        Dim rowMagentoTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row
        Dim rowMagentoCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoCategories_DEV000221Row
        Dim strHomeCurrency As String, strErrorDetails As String, strSKUBeingProcessed As String = ""
        Dim iAttributeLoop As Integer, iCategoryLoop As Integer
        Dim bInstanceMatched As Boolean

        Try
            m_LastErrorMessage = ""
            m_LastError = ""
            m_ProcessLog = ""
            If m_MagentoImportRule.IsActivated Then
                strErrorDetails = ""
                strHomeCurrency = Me.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                XMLConfig = XDocument.Parse(Trim(m_MagentoImportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                bInstanceMatched = False
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    If m_MagentoImportRule.GetXMLElementListCount(XMLNodeList) = 1 Then
                        bInstanceMatched = True
                    ElseIf m_MagentoImportRule.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoInstanceID Then
                        bInstanceMatched = True
                    End If
                    If bInstanceMatched Then
                        TempDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
                        TempFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(TempDataset, New Lerryn.Facade.ImportExport.ErrorNotification, p_BaseProductCode, p_BaseProductName)
                        For iItemLoop = 0 To ItemsToPublish.Count - 1
                            strSKUBeingProcessed = ItemsToPublish(iItemLoop).Item("ItemName").ToString
                            If CBool(ItemsToPublish(iItemLoop).Item("Select")) Then
                                Me.LoadDataSet(New String()() {New String() {m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                        "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, ItemsToPublish(iItemLoop).Item("ItemCode").ToString, AT_INSTANCE_ID, MagentoInstanceToCopy}, _
                                    New String() {m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                        "ReadInventoryMagentoCategories_DEV000221", AT_ITEM_CODE, ItemsToPublish(iItemLoop).Item("ItemCode").ToString, AT_INSTANCE_ID, MagentoInstanceToCopy}, _
                                    New String() {m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                        "ReadInventoryMagentoTagDetails_DEV000221", AT_ITEM_CODE, ItemsToPublish(iItemLoop).Item("ItemCode").ToString, AT_INSTANCE_ID, MagentoInstanceToCopy}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific)
                                TempDataset.Clear()
                                rowMagentoDetails = TempDataset.InventoryMagentoDetails_DEV000221.NewInventoryMagentoDetails_DEV000221Row
                                rowMagentoDetails.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                rowMagentoDetails.InstanceID_DEV000221 = MagentoInstanceID
                                rowMagentoDetails.Publish_DEV000221 = True
                                rowMagentoDetails.FromImportWizard_DEV000221 = False
                                rowMagentoDetails.SourceIsGroupItem_DEV000221 = False
                                rowMagentoDetails.AttributeSetID_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).AttributeSetID_DEV000221
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductName_DEV000221Null Then
                                    rowMagentoDetails.ProductName_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductName_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductShortDescription_DEV000221Null Then
                                    rowMagentoDetails.ProductShortDescription_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductShortDescription_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductDescription_DEV000221Null Then
                                    rowMagentoDetails.ProductDescription_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductDescription_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsProductInDepthDescription_DEV000221Null Then
                                    rowMagentoDetails.ProductInDepthDescription_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ProductInDepthDescription_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsSellingPrice_DEV000221Null Then
                                    rowMagentoDetails.SellingPrice_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).SellingPrice_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsUseISPricingDetail_DEV000221Null Then
                                    rowMagentoDetails.UseISPricingDetail_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).UseISPricingDetail_DEV000221
                                Else
                                    rowMagentoDetails.UseISPricingDetail_DEV000221 = False
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsVisibility_DEV000221Null Then
                                    rowMagentoDetails.Visibility_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Visibility_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsStatus_DEV000221Null Then
                                    rowMagentoDetails.Status_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).Status_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsNewFrom_DEV000221Null Then
                                    rowMagentoDetails.ShowAsNewFrom_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewFrom_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsNewTo_DEV000221Null Then
                                    rowMagentoDetails.ShowAsNewTo_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewTo_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null Then
                                    rowMagentoDetails.ShowAsSpecialFrom_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null Then
                                    rowMagentoDetails.ShowAsSpecialTo_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsSpecialPrice_DEV000221Null Then
                                    rowMagentoDetails.SpecialPrice_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                                    rowMagentoDetails.QtyPublishingType_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingType_DEV000221
                                End If
                                If Not m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).IsQtyPublishingValue_DEV000221Null Then
                                    rowMagentoDetails.QtyPublishingValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingValue_DEV000221
                                End If
                                rowMagentoDetails.TotalQtyWhenLastPublished_DEV000221 = 0
                                rowMagentoDetails.QtyLastPublished_DEV000221 = 0
                                TempDataset.InventoryMagentoDetails_DEV000221.AddInventoryMagentoDetails_DEV000221Row(rowMagentoDetails)

                                For iAttributeLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Count - 1
                                    rowMagentoTagDetails = TempDataset.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                                    rowMagentoTagDetails.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                    rowMagentoTagDetails.InstanceID_DEV000221 = MagentoInstanceID
                                    rowMagentoTagDetails.TagName_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagName_DEV000221
                                    rowMagentoTagDetails.TagDisplayName_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagDisplayName_DEV000221
                                    rowMagentoTagDetails.LineNumber_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).LineNumber_DEV000221
                                    rowMagentoTagDetails.TagRequired_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagRequired_DEV000221
                                    rowMagentoTagDetails.TagDataType_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagDataType_DEV000221
                                    If Not rowMagentoTagDetails.IsTagTextValue_DEV000221Null Then
                                        rowMagentoTagDetails.TagTextValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagTextValue_DEV000221
                                        Select Case rowMagentoTagDetails.TagDataType_DEV000221
                                            Case "Memo"
                                                If Not rowMagentoTagDetails.IsTagMemoValue_DEV000221Null Then
                                                    rowMagentoTagDetails.TagMemoValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagMemoValue_DEV000221
                                                End If
                                            Case "Date", "DateTime"
                                                If Not rowMagentoTagDetails.IsTagDateValue_DEV000221Null Then
                                                    rowMagentoTagDetails.TagDateValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagDateValue_DEV000221
                                                End If
                                            Case "Integer", "Numeric"
                                                If Not rowMagentoTagDetails.IsTagNumericValue_DEV000221Null Then
                                                    rowMagentoTagDetails.TagNumericValue_DEV000221 = m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagNumericValue_DEV000221
                                                End If
                                        End Select
                                    End If
                                    SetMagentoAttributeID(rowMagentoTagDetails) ' TJS 13/11/13
                                    TempDataset.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)
                                Next

                                For iCategoryLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Count - 1
                                    If m_MagentoImportDataset.InventoryMagentoCategories_DEV000221(iCategoryLoop).IsActive_DEV000221 Then
                                        rowMagentoCategory = TempDataset.InventoryMagentoCategories_DEV000221.NewInventoryMagentoCategories_DEV000221Row
                                        rowMagentoCategory.ItemCode_DEV000221 = ItemsToPublish(iItemLoop).Item("ItemCode").ToString
                                        rowMagentoCategory.InstanceID_DEV000221 = MagentoInstanceID
                                        rowMagentoCategory.MagentoCategoryID_DEV000221 = m_MagentoImportDataset.InventoryMagentoCategories_DEV000221(iCategoryLoop).MagentoCategoryID_DEV000221
                                        rowMagentoCategory.MagentoWebSiteID_DEV000221 = m_MagentoImportDataset.InventoryMagentoCategories_DEV000221(iCategoryLoop).MagentoWebSiteID_DEV000221
                                        rowMagentoCategory.IsActive_DEV000221 = True
                                        TempDataset.InventoryMagentoCategories_DEV000221.AddInventoryMagentoCategories_DEV000221Row(rowMagentoCategory)
                                    End If
                                Next

                                If TempFacade.UpdateDataSet(New String()() {New String() {TempDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                        "CreateInventoryMagentoDetails_DEV000221", "UpdateInventoryMagentoDetails_DEV000221", "DeleteInventoryMagentoDetails_DEV000221"}, _
                                    New String() {TempDataset.InventoryMagentoTagDetails_DEV000221.TableName, _
                                        "CreateInventoryMagentoTagDetails_DEV000221", "UpdateInventoryMagentoTagDetails_DEV000221", "DeleteInventoryMagentoTagDetails_DEV000221"}, _
                                    New String() {TempDataset.InventoryMagentoCategories_DEV000221.TableName, _
                                        "CreateInventoryMagentoCategories_DEV000221", "UpdateInventoryMagentoCategories_DEV000221", "DeleteInventoryMagentoCategories_DEV000221"}}, _
                                    Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Bulk Publish Inventory Magento Settings", False) Then
                                    NoOfItemsPublished += 1

                                Else
                                    ' get error details
                                    strErrorDetails = ""
                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Columns.Count - 1
                                            If m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.TableName & _
                                                    "." & m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_MagentoImportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Columns.Count - 1
                                            If m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.TableName & _
                                                    "." & m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_MagentoImportDataset.InventoryMagentoCategories_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    For iRowLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows.Count - 1
                                        For iColumnLoop = 0 To m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Columns.Count - 1
                                            If m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                strErrorDetails = strErrorDetails & m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.TableName & _
                                                    "." & m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                    m_MagentoImportDataset.InventoryMagentoTagDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                            End If
                                        Next
                                    Next
                                    m_LastError = "Publishing of Item " & strSKUBeingProcessed & " aborted - " & strErrorDetails
                                    m_LastErrorMessage = m_LastError
                                    m_ProcessLog = m_ProcessLog & "Publishing of Item " & strSKUBeingProcessed & " aborted - " & strErrorDetails & vbCrLf
                                    NoOfItemsSkipped = NoOfItemsSkipped + 1


                                End If
                            End If
                            NoOfItemsProcessed += 1
                        Next
                        Exit For ' TJS 01/04/14
                    End If
                Next
                If Not bInstanceMatched Then
                    m_LastError = "Magento Instance not found."
                    m_LastErrorMessage = m_LastError
                    m_ProcessLog = m_ProcessLog & "Cannot connect to Magento" & vbCrLf
                End If
            End If

        Catch ex As Exception
            m_LastError = "Error whilst processing Item " & strSKUBeingProcessed & " - " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            m_LastErrorMessage = "Error whilst processing Item " & strSKUBeingProcessed & " - " & ex.Message
            m_ProcessLog = m_ProcessLog & "Error whilst processing Item " & strSKUBeingProcessed & " - " & ex.Message

        End Try

    End Sub
#End Region
#End Region

End Class
#End Region
