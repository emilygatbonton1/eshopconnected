' eShopCONNECT for Connected Business 
' Module: eBayXMLConnector.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
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
' Last Updated - 19 September 2013

Imports Microsoft.VisualBasic
Imports System.Collections ' TJS 26/06/13
Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Linq ' TJS 26/06/13
Imports System.Net
Imports System.Web
Imports System.Xml ' TJS 26/06/13
Imports System.Xml.Linq
Imports System.Xml.XPath
Imports System.Xml.Serialization ' TJS 26/06/13

Public Class eBayXMLConnector

#Region " Variables "
    Private Const eBayAPIVersion As String = "841" ' FA 06/09/13

    Public Class TimeFilter
        Private pTimeFrom As DateTime
        Public Property TimeFrom As DateTime
            Get
                Return pTimeFrom
            End Get
            Set(ByVal value As DateTime)
                pTimeFrom = value
            End Set
        End Property

        Private pTimeTo As DateTime
        Public Property TimeTo As DateTime
            Get
                Return pTimeTo
            End Get
            Set(ByVal value As DateTime)
                pTimeTo = value
            End Set
        End Property
    End Class

    Public Structure eBaySites
        Public SiteName As String
        Public SiteID As Integer
    End Structure

    Public Structure eBayShippingCarriers
        Public CarrierName As String
        Public CarrierID As Integer
    End Structure

    Public Structure eBayShipmentDetails
        Public TrackingNumber As String
        Public CarrierUsed As String
    End Structure

    ' start of code added from Lloyd's prototype TJS 26/06/13
    Public Structure eBayFee
        Public Name As String
        Public Fee As Decimal
        Public Currency As String
        Public ItemID As String
    End Structure

    Public Structure CategoryCatalogEnabled
        Public CategoryID As Integer
        Public CatalogEnabled As Boolean
    End Structure

    Public Structure ConditionValues
        Public ID As Integer
        Public DisplayName As String
        Public CategoryID As Integer
    End Structure

    Public Structure CategoryListingDurations
        Public CategoryID As Integer
        Public SiteID As Integer
        Public Duration As String
        Public ID As Integer
        Public Type As String
        Public Duration_Screen As String ' Duration version for display on screen
        Public LerrynID As Integer ' Our URN
    End Structure

    Public Structure CategoryDefaults
        Public CategoryID As Integer
        Public Name As String
        Public Value As String
        Public Type As String
        Public LerrynID As Integer
    End Structure

    Public Structure SiteDefaults
        Public TagName As String
        Public Name As String
        Public Type As String
        Public Value As String
        Public SiteID As Integer
    End Structure

    Public Structure eBayCategory
        Public BestOfferEnabled As Boolean
        Public AutoPayEnabled As Boolean
        Public CategoryID As Integer
        Public CategoryLevel As Integer
        Public CategoryName As String
        Public CategoryParentID As Integer
        Public SelectedOrder As Integer
        Public PercentItemFound As Integer ' Used by suggested categories
        Public CategoryParent As ArrayList
        Public SuggestedCategory As Boolean ' .T. = category is from "suggested categories", .F. = regular "drill down" category
        Public SuggestedCategoryText As String ' String used to search for SuggestedCategory
        Public GetCategoryFeaturesXML As String
        Public FindProductsXML As String
        Public SelectedConditionID As Integer
        Public SelectedCategoryText As String
        Public NumberOfConditionsForCategory As Integer
        Public AllowCategoryMapping As Boolean
        Public ReturnPolicyEnabled As Boolean
        Public CatalogEnabled As Boolean ' If true then this category is "CatalogEnabled" - it uses the FindProducts for AddItem. See here http://pages.ebay.co.uk/help/sell/product-details.html
        Public CatalogPropertiesSelected As Boolean ' If true then the user has selected and edited (if required) the relevent item for publishing 
    End Structure

    ' This is used to populate the form controls with information about the current stock item
    Public Structure InterpriseStockItem
        Public ItemCode As String
        Public Description As String
        Public RetailPrice As Decimal
        Public StockQuantity As Integer
        Public ItemName As String
    End Structure

    Public Structure SiteDetails
        Public Site As String
        Public SiteID As Integer
        Public DetailVersion As Integer
        Public UpdateTime As String
    End Structure

    Public Structure CurrencyDetails
        Public Currency As String
        Public Description As String
        Public DetailVersion As Integer
        Public UpdateTime As String
    End Structure

    Public Structure CountryDetails
        Public Country As String
        Public Description As String
        Public DetailVersion As Integer
        Public UpdateTime As String
    End Structure

    Public Structure CategoryParentID
        Public CategoryID As Integer
        Public CategoryParentID As Integer
        Public CategoryParentDescription As String
    End Structure

    Public Structure AddeBayItemError
        Public Status As String
        Public Timecode As String
        Public ShortMessage As String
        Public LongMessage As String
        Public ErrorCode As String
        Public SeverityCode As String
        Public ErrorClassification As String
    End Structure

    Private m_eBayPublishingWizardFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade ' TJS 26/08/13
#End Region

#Region " Properties "
    Private paCurrencyDetails As New ArrayList
    Public Property aCurrencyDetails() As ArrayList
        Get
            Return paCurrencyDetails
        End Get
        Set(ByVal value As ArrayList)
            paCurrencyDetails = value
        End Set
    End Property

    Private pCurrencyInUse As New CurrencyDetails
    Public Property CurrencyInUse As CurrencyDetails
        Get
            Return pCurrencyInUse
        End Get
        Set(ByVal value As CurrencyDetails)
            pCurrencyInUse = value
        End Set
    End Property

    Private paCountryDetails As New ArrayList
    Public Property aCountryDetails() As ArrayList
        Get
            Return paCountryDetails
        End Get
        Set(ByVal value As ArrayList)
            paCountryDetails = value
        End Set
    End Property

    Private pCountryInUse As New CountryDetails
    Public Property CountryInUse As CountryDetails
        Get
            Return pCountryInUse
        End Get
        Set(ByVal value As CountryDetails)
            pCountryInUse = value
        End Set
    End Property

    ' If an item is posted then any error status is placed here for other parts of the system to use
    Private pm_AddeBayItemError As New ArrayList
    Public Property m_AddeBayItemError() As ArrayList
        Get
            Return pm_AddeBayItemError
        End Get
        Set(ByVal value As ArrayList)
            pm_AddeBayItemError = value
        End Set
    End Property
 
#Region "ShippingLocationDetails"
    Public Class clsShippingLocationDetails
        Private _Timestamp As String
        Private _Ack As String
        Private _Version As Integer
        Private _Build As String
        Private _ShippingLocationDetails As New ArrayList

        Public Property Timestamp() As String
            Get
                Return Me._Timestamp
            End Get
            Set(value As String)
                Me._Timestamp = value
            End Set
        End Property

        Public Property Ack() As String
            Get
                Return Me._Ack
            End Get
            Set(value As String)
                Me._Ack = value
            End Set
        End Property

        Public Property Version() As Integer
            Get
                Return Me._Version
            End Get
            Set(value As Integer)
                Me._Version = value
            End Set
        End Property

        Public Property Build() As String
            Get
                Return Me._Build
            End Get
            Set(value As String)
                Me._Build = value
            End Set
        End Property

        Public Property ShippingLocationDetails() As ArrayList
            Get
                Return Me._ShippingLocationDetails
            End Get
            Set(value As ArrayList)
                Me._ShippingLocationDetails = value
            End Set
        End Property
    End Class

    Public Structure ShippingLocationDetails
        Public ShippingLocation As String
        Public Description As String
        Public DetailVersion As String
        Public UpdateTime As String
        Public LerrynID As Integer
    End Structure

#End Region

#Region "FindProductsResponse"
    Public Class clsFindProductsResponse

        Private _CategoryID As Integer
        Private _Timestamp As String
        Private _Ack As String
        Private _Version As Integer
        Private _Build As String
        Private _ApproximatePages As Integer
        Private _MoreResults As Boolean
        Private _PageNumber As Integer

        Private _Product As New ArrayList

        Private _TotalProducts As Integer

        Public Property CategoryID() As Integer
            Get
                Return Me._CategoryID
            End Get
            Set(value As Integer)
                Me._CategoryID = value
            End Set
        End Property

        Public Property Timestamp() As String
            Get
                Return Me._Timestamp
            End Get
            Set(value As String)
                Me._Timestamp = value
            End Set
        End Property

        Public Property Ack() As String
            Get
                Return Me._Ack
            End Get
            Set(value As String)
                Me._Ack = value
            End Set
        End Property

        Public Property Version() As Integer
            Get
                Return Me._Version
            End Get
            Set(value As Integer)
                Me._Version = value
            End Set
        End Property

        Public Property Build() As String
            Get
                Return Me._Build
            End Get
            Set(value As String)
                Me._Build = value
            End Set
        End Property

        Public Property ApproximatePages() As Integer
            Get
                Return Me._ApproximatePages
            End Get
            Set(value As Integer)
                Me._ApproximatePages = value
            End Set
        End Property

        Public Property MoreResults() As Boolean
            Get
                Return Me._MoreResults
            End Get
            Set(value As Boolean)
                Me._MoreResults = value
            End Set
        End Property

        Public Property PageNumber() As Integer
            Get
                Return Me._PageNumber
            End Get
            Set(value As Integer)
                Me._PageNumber = value
            End Set
        End Property

        Public Property Product() As ArrayList
            Get
                Return Me._Product
            End Get
            Set(value As ArrayList)
                Me._Product = value
            End Set
        End Property

        Public Property TotalProducts() As Integer
            Get
                Return Me._TotalProducts
            End Get
            Set(value As Integer)
                Me._TotalProducts = value
            End Set
        End Property
    End Class

    Public Class clsFindProductsResponseProduct
        Private _LerrynID As Integer
        Private _DomainName As String
        Private _DetailsURL As String
        Private _DisplayStockPhotos As Boolean
        Private _ProductID As New ArrayList
        Private _ItemSpecifics As New ArrayList
        Private _ReviewCount As Integer
        Private _StockPhotoURL As String
        Private _Title As String

        Public Property LerrynID() As Integer
            Get
                Return Me._LerrynID
            End Get
            Set(value As Integer)
                Me._LerrynID = value
            End Set
        End Property

        Public Property DomainName() As String
            Get
                Return Me._DomainName
            End Get
            Set(value As String)
                Me._DomainName = value
            End Set
        End Property

        Public Property DetailsURL() As String
            Get
                Return Me._DetailsURL
            End Get
            Set(value As String)
                Me._DetailsURL = value
            End Set
        End Property

        Public Property DisplayStockPhotos() As Boolean
            Get
                Return Me._DisplayStockPhotos
            End Get
            Set(value As Boolean)
                Me._DisplayStockPhotos = value
            End Set
        End Property

        Public Property ProductID() As ArrayList
            Get
                Return Me._ProductID
            End Get
            Set(value As ArrayList)
                Me._ProductID = value
            End Set
        End Property

        Public Property ItemSpecifics() As ArrayList
            Get
                Return Me._ItemSpecifics
            End Get
            Set(value As ArrayList)
                Me._ItemSpecifics = value
            End Set
        End Property

        Public Property ReviewCount() As Integer
            Get
                Return Me._ReviewCount
            End Get
            Set(value As Integer)
                Me._ReviewCount = value
            End Set
        End Property

        Public Property StockPhotoURL() As String
            Get
                Return Me._StockPhotoURL
            End Get
            Set(value As String)
                Me._StockPhotoURL = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return Me._Title
            End Get
            Set(value As String)
                Me._Title = value
            End Set
        End Property
    End Class

    Public Structure FindProductsResponse_Product_ProductID
        Public Type As String
        Public ProductID As Integer
    End Structure

    Public Class clsFindProductsResponse_Product_ItemSpecifics
        Private _NameValueList As New ArrayList
        Public Property NameValueList() As ArrayList
            Get
                Return Me._NameValueList
            End Get
            Set(value As ArrayList)
                Me._NameValueList = value
            End Set
        End Property
    End Class

    Public Class clsFindProductsResponse_Product_ItemSpecifics_NameValueList
        Private _LerrynID As New ArrayList
        Private _Name As String
        Private _value As New ArrayList

        Public Property LerrynID() As ArrayList
            Get
                Return Me._LerrynID
            End Get
            Set(value As ArrayList)
                Me._LerrynID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
            End Set
        End Property

        Public Property value() As ArrayList
            Get
                Return Me._value
            End Get
            Set(value As ArrayList)
                Me._value = value
            End Set
        End Property
    End Class
#End Region

#Region "ReturnPolicyDetails"
    Public Class clsReturnPolicyDetails

        Private _Timestamp As String
        Private _Ack As String
        Private _Version As Integer
        Private _Build As String

        Private _Refund As New ArrayList
        Private _ReturnsWithin As New ArrayList
        Private _ReturnsAccepted As New ArrayList
        Private _ShippingCostPaidBy As New ArrayList
        Private _RestockingFeeValue As New ArrayList

        Private _DetailVersion As Integer
        Private _UpdateTime As String

        Private _Description As Boolean

        Public Property Timestamp() As String
            Get
                Return Me._Timestamp
            End Get
            Set(value As String)
                Me._Timestamp = value
            End Set
        End Property

        Public Property Ack() As String
            Get
                Return Me._Ack
            End Get
            Set(value As String)
                Me._Ack = value
            End Set
        End Property
        Public Property Version() As Integer
            Get
                Return Me._Version
            End Get
            Set(value As Integer)
                Me._Version = value
            End Set
        End Property

        Public Property Build() As String
            Get
                Return Me._Build
            End Get
            Set(value As String)
                Me._Build = value
            End Set
        End Property

        Public Property Refund() As ArrayList
            Get
                Return Me._Refund
            End Get
            Set(value As ArrayList)
                Me._Refund = value
            End Set
        End Property

        Public Property ReturnsWithin() As ArrayList
            Get
                Return Me._ReturnsWithin
            End Get
            Set(value As ArrayList)
                Me._ReturnsWithin = value
            End Set
        End Property

        Public Property ReturnsAccepted() As ArrayList
            Get
                Return Me._ReturnsAccepted
            End Get
            Set(value As ArrayList)
                Me._ReturnsAccepted = value
            End Set
        End Property

        Public Property ShippingCostPaidBy() As ArrayList
            Get
                Return Me._ShippingCostPaidBy
            End Get
            Set(value As ArrayList)
                Me._ShippingCostPaidBy = value
            End Set
        End Property

        Public Property RestockingFeeValue() As ArrayList
            Get
                Return Me._RestockingFeeValue
            End Get
            Set(value As ArrayList)
                Me._RestockingFeeValue = value
            End Set
        End Property

        Public Property DetailVersion() As Integer
            Get
                Return Me._DetailVersion
            End Get
            Set(value As Integer)
                Me._DetailVersion = value
            End Set
        End Property

        Public Property UpdateTime() As String
            Get
                Return Me._UpdateTime
            End Get
            Set(value As String)
                Me._UpdateTime = value
            End Set
        End Property

        Public Property Description() As Boolean
            Get
                Return Me._Description
            End Get
            Set(value As Boolean)
                Me._Description = value
            End Set
        End Property
    End Class

    Public Structure ReturnPolicyDetailsRefund
        Public LerrynID As Integer
        Public RefundOption As String
        Public Description As String
    End Structure

    Public Structure ReturnPolicyDetailsReturnsWithin
        Public LerrynID As Integer
        Public ReturnsWithinOption As String
        Public Description As String
    End Structure

    Public Structure ReturnPolicyDetailsReturnsAccepted
        Public LerrynID As Integer
        Public ReturnsAcceptedOption As String
        Public Description As String
        Public DetailVersion As Integer
    End Structure

    Public Structure ReturnPolicyDetailsShippingCostPaidBy
        Public LerrynID As Integer
        Public ShippingCostPaidByOption As String
        Public Description As String
    End Structure

    Public Structure ReturnPolicyDetailsShippingRestockingFeeValue
        Public LerrynID As Integer
        Public RestockingFeeValueOption As String
        Public Description As String
    End Structure

#End Region

#Region "ShippingServiceDetails"
    Public Class clsShippingServiceDetails
        Private _Description As String
        Private _InternationalService As Boolean
        Private _ShippingService As String
        Private _ShippingServiceID As Integer
        Private _ShippingTimeMax As Integer
        Private _ShippingTimeMin As Integer
        Private _ServiceType As New ArrayList
        Private _ValidForSellingFlow As String
        Private _DetailVersion As Integer
        Private _UpdateTime As String
        Private _ShippingCategory As String
        Private _ShippingServicePackageDetails As New ArrayList
        Private _ShippingPackage As New ArrayList
        Private _LerrynID As Integer
        Private _WeightRequired As Boolean
        Private _ShippingCarrier As String
        Private _DimensionsRequired As Boolean
        Private _SurchargeApplicable As Boolean
        Private _ExpeditedService As Boolean

        Public Property Description() As String
            Get
                Return Me._Description
            End Get
            Set(value As String)
                Me._Description = value
            End Set
        End Property

        Public Property InternationalService() As Boolean
            Get
                Return Me._InternationalService
            End Get
            Set(value As Boolean)
                Me._InternationalService = value
            End Set
        End Property

        Public Property ShippingService() As String
            Get
                Return Me._ShippingService
            End Get
            Set(value As String)
                Me._ShippingService = value
            End Set
        End Property

        Public Property ShippingServiceID() As Integer
            Get
                Return Me._ShippingServiceID
            End Get
            Set(value As Integer)
                Me._ShippingServiceID = value
            End Set
        End Property

        Public Property ShippingTimeMax() As Integer
            Get
                Return Me._ShippingTimeMax
            End Get
            Set(value As Integer)
                Me._ShippingTimeMax = value
            End Set
        End Property

        Public Property ShippingTimeMin() As Integer
            Get
                Return Me._ShippingTimeMin
            End Get
            Set(value As Integer)
                Me._ShippingTimeMin = value
            End Set
        End Property

        Public Property ServiceType() As ArrayList
            Get
                Return Me._ServiceType
            End Get
            Set(value As ArrayList)
                Me._ServiceType = value
            End Set
        End Property

        Public Property ValidForSellingFlow() As String
            Get
                Return Me._ValidForSellingFlow
            End Get
            Set(value As String)
                Me._ValidForSellingFlow = value
            End Set
        End Property

        Public Property DetailVersion() As Integer
            Get
                Return Me._DetailVersion
            End Get
            Set(value As Integer)
                Me._DetailVersion = value
            End Set
        End Property

        Public Property UpdateTime() As String
            Get
                Return Me._UpdateTime
            End Get
            Set(value As String)
                Me._UpdateTime = value
            End Set
        End Property

        Public Property ShippingCategory() As String
            Get
                Return Me._ShippingCategory
            End Get
            Set(value As String)
                Me._ShippingCategory = value
            End Set
        End Property

        Public Property ShippingServicePackageDetails() As ArrayList
            Get
                Return Me._ShippingServicePackageDetails
            End Get
            Set(value As ArrayList)
                Me._ShippingServicePackageDetails = value
            End Set
        End Property

        Public Property ShippingPackage() As ArrayList
            Get
                Return Me._ShippingPackage
            End Get
            Set(value As ArrayList)
                Me._ShippingPackage = value
            End Set
        End Property

        Public Property LerrynID As Integer
            Get
                Return Me._LerrynID
            End Get
            Set(value As Integer)
                Me._LerrynID = value
            End Set
        End Property

        Public Property WeightRequired As Boolean
            Get
                Return Me._WeightRequired
            End Get
            Set(value As Boolean)
                Me._WeightRequired = value
            End Set
        End Property

        Public Property ShippingCarrier As String
            Get
                Return Me._ShippingCarrier
            End Get
            Set(value As String)
                Me._ShippingCarrier = value
            End Set
        End Property

        Public Property DimensionsRequired As Boolean
            Get
                Return Me._DimensionsRequired
            End Get
            Set(value As Boolean)
                Me._DimensionsRequired = value
            End Set
        End Property

        Public Property SurchargeApplicable As Boolean
            Get
                Return Me._SurchargeApplicable
            End Get
            Set(value As Boolean)
                Me._SurchargeApplicable = value
            End Set
        End Property

        Public Property ExpeditedService As Boolean
            Get
                Return Me._ExpeditedService
            End Get
            Set(value As Boolean)
                Me._ExpeditedService = value
            End Set
        End Property
    End Class

    Public Property aclsShippingServiceDetails() As ArrayList
        Get
            Return pclsShippingServiceDetails
        End Get
        Set(ByVal value As ArrayList)
            pclsShippingServiceDetails = value
        End Set
    End Property

    Private pclsShippingServiceDetails As New ArrayList

    Public Structure ShippingServicePackageDetails
        Public ShippingServicePackageDetails As String
        Public Name As String
        Public LerrynID As Integer
    End Structure

    Public Structure ShippingPackage
        Public LerrynID As Integer
        Public ShippingPackage As String
    End Structure

    Public Property aShippingServiceDetails() As ArrayList
        Get
            Return pShippingServiceDetails
        End Get
        Set(ByVal value As ArrayList)
            pShippingServiceDetails = value
        End Set
    End Property
    Private pShippingServiceDetails As New ArrayList

    Public Property aShippingLocationDetails() As ArrayList
        Get
            Return pShippingLocationDetails
        End Get
        Set(ByVal value As ArrayList)
            pShippingLocationDetails = value
        End Set
    End Property
    Private pShippingLocationDetails As New ArrayList
#End Region
    ' end of code added from Lloyd's prototype TJS 26/06/13

    ' XML Posted to eBay
    Private pPostedXML As String
    Public ReadOnly Property PostedXML() As String
        Get
            Return pPostedXML
        End Get
    End Property

    ' eBay returned data (or error response)
    Private pReturnedData As String
    Public ReadOnly Property ReturnedData() As String
        Get
            Return pReturnedData
        End Get
    End Property

    ' eBay returned XML (if valid XML)
    Private pReturnedXML As XDocument
    Public ReadOnly Property ReturnedXML() As XDocument
        Get
            Return pReturnedXML
        End Get
    End Property

    Private psLastError As String
    Public Property LastError() As String
        Get
            Return psLastError
        End Get
        Set(ByVal value As String)
            psLastError = value
        End Set
    End Property

    Private pbErrorOccured As Boolean
    Public Property ErrorOccured As Boolean
        Get
            Return pbErrorOccured
        End Get
        Set(ByVal value As Boolean)
            pbErrorOccured = value
        End Set
    End Property

    Private peBayDevID As String
    Public Property eBayDevID() As String
        Get
            Return peBayDevID
        End Get
        Set(ByVal value As String)
            peBayDevID = value
        End Set
    End Property

    Private peBayAppID As String
    Public Property eBayAppID() As String
        Get
            Return peBayAppID
        End Get
        Set(ByVal value As String)
            peBayAppID = value
        End Set
    End Property

    Private peBayCertID As String
    Public Property eBayCertID() As String
        Get
            Return peBayCertID
        End Get
        Set(ByVal value As String)
            peBayCertID = value
        End Set
    End Property

    Private peBayAuthToken As String
    Public Property eBayAuthToken() As String
        Get
            Return peBayAuthToken
        End Get
        Set(ByVal value As String)
            peBayAuthToken = value
        End Set
    End Property

    Private peBayXMLServerURL As String
    Public Property eBayXMLServerURL() As String
        Get
            Return peBayXMLServerURL
        End Get
        Set(ByVal value As String)
            peBayXMLServerURL = value
        End Set
    End Property

    Private peBayCountry As Integer
    Public Property eBayCountry As Integer
        Get
            Return peBayCountry
        End Get
        Set(ByVal value As Integer)
            peBayCountry = value
        End Set
    End Property

    ' start of code added from Lloyd's prototype TJS 26/06/13
    Private paCatalogEnabled As New ArrayList
    Public Property CatalogEnabled() As ArrayList
        Get
            Return paCatalogEnabled
        End Get
        Set(ByVal value As ArrayList)
            paCatalogEnabled = value
        End Set
    End Property

    Private paReturnsPolicy As New ArrayList
    Public Property aReturnsPolicy() As ArrayList
        Get
            Return paReturnsPolicy
        End Get
        Set(ByVal value As ArrayList)
            paReturnsPolicy = value
        End Set
    End Property

    Private paFindProductsResponseProducts As New ArrayList
    Public Property aFindProductsResponseProducts() As ArrayList
        Get
            Return paFindProductsResponseProducts
        End Get
        Set(ByVal value As ArrayList)
            paFindProductsResponseProducts = value
        End Set
    End Property

    Private pConditionValues As New ArrayList
    Public Property aConditionValues() As ArrayList
        Get
            Return pConditionValues
        End Get
        Set(ByVal value As ArrayList)
            pConditionValues = value
        End Set
    End Property

    Private pCategoryListingDurations As New ArrayList
    Public Property aCategoryListingDurations() As ArrayList
        Get
            Return pCategoryListingDurations
        End Get
        Set(ByVal value As ArrayList)
            pCategoryListingDurations = value
        End Set
    End Property

    Private pSiteDefaults As New ArrayList
    Public Property aSiteDefaults() As ArrayList
        Get
            Return pSiteDefaults
        End Get
        Set(ByVal value As ArrayList)
            pSiteDefaults = value
        End Set
    End Property

    Private pCategoryDefaults As New ArrayList
    Public Property aCategoryDefaults() As ArrayList
        Get
            Return pCategoryDefaults
        End Get
        Set(ByVal value As ArrayList)
            pCategoryDefaults = value
        End Set
    End Property

    Private pSiteDetails As New ArrayList
    Public Property aSiteDetails() As ArrayList
        Get
            Return pSiteDetails
        End Get
        Set(ByVal value As ArrayList)
            pSiteDetails = value
        End Set
    End Property

    Private paCategories As New ArrayList
    Public Property aCategories() As ArrayList
        Get
            Return paCategories
        End Get
        Set(ByVal value As ArrayList)
            paCategories = value
        End Set
    End Property

    Private paCategoriesSelected As New ArrayList
    Public Property aCategoriesSelected() As ArrayList
        Get
            Return paCategoriesSelected
        End Get
        Set(ByVal value As ArrayList)
            paCategoriesSelected = value
        End Set
    End Property

    ' ArrayList of suggested categories
    Private paSuggestedCategories As New ArrayList
    Public Property aSuggestedCategories() As ArrayList
        Get
            Return paSuggestedCategories
        End Get
        Set(ByVal value As ArrayList)
            paSuggestedCategories = value
        End Set
    End Property

    Private paeBayPostedItemFees As New ArrayList
    Public Property aeBayPostedItemFees() As ArrayList ' Array of eBay fees attached to a listing
        Get
            Return paeBayPostedItemFees
        End Get
        Set(ByVal value As ArrayList)
            paeBayPostedItemFees = value
        End Set
    End Property
    ' end of code added from Lloyd's prototype TJS 26/06/13
#End Region

    Public Function GetXMLHeader(ByVal eBayFunction As String, ByVal IncludeAuth As Boolean) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Builds the eBay Header for XML posts
        '                   Found here: http://developer.ebay.com/DevZone/XML/docs/WebHelp/wwhelp/wwhimpl/js/html/wwhelp.htm?context=eBay_XML_API&topic=StandardData
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sXML As String

        sXML = "<?xml version=""1.0"" encoding=""utf-8""?><" & eBayFunction & "Request xmlns=""urn:ebay:apis:eBLBaseComponents"">"
        If IncludeAuth Then
            sXML = sXML & "<RequesterCredentials><eBayAuthToken>" & eBayAuthToken & "</eBayAuthToken></RequesterCredentials>"
        End If

        Return sXML

    End Function

    Public Function PostXML(ByVal XMLToPost As String, ByVal eBayFunction As String, Optional ByVal ReceiveTimeout As Integer = 60000, _
        Optional ByVal bIncludeAPIVersion As Boolean = True, Optional ByRef sDestination As String = "REGULAR") As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Posts XML data to eBay
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 26/06/13 | TJS             | 2013.2.00 | Modified to call PostXMLRegular and PostXMLShopping
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Select Case sDestination.ToUpper
            Case "REGULAR"
                bSuccess = PostXMLRegular(XMLToPost, eBayFunction, ReceiveTimeout, bIncludeAPIVersion)
            Case "SHOPPING"
                bSuccess = PostXMLShopping(XMLToPost, eBayFunction, ReceiveTimeout, bIncludeAPIVersion)
        End Select

        Return bSuccess

    End Function

    Public Function PostXMLRegular(ByVal XMLToPost As String, ByVal eBayFunction As String, Optional ByVal ReceiveTimeout As Integer = 60000, _
        Optional ByVal bIncludeAPIVersion As Boolean = True) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Posts XML data to eBay
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 26/06/13 | TJS             | 2013.2.00 | Function renamed from PostXML
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader
        Dim bSuccess As Boolean = False

        LastError = "" ' Clear any previous errors
        ' Store the posted XML
        pPostedXML = XMLToPost
        pReturnedData = ""

        byteData = UTF8Encoding.UTF8.GetBytes(XMLToPost)

        ' Send the XML message
        Try
            WebSubmit = System.Net.WebRequest.Create(peBayXMLServerURL)
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "text/xml; charset=utf-8"
            WebSubmit.ContentLength = byteData.Length
            WebSubmit.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", eBayAPIVersion)
            WebSubmit.Headers.Add("X-EBAY-API-DEV-NAME", eBayDevID)
            WebSubmit.Headers.Add("X-EBAY-API-APP-NAME", eBayAppID)
            WebSubmit.Headers.Add("X-EBAY-API-CERT-NAME", eBayCertID)
            WebSubmit.Headers.Add("X-EBAY-API-SITEID", eBayCountry.ToString)
            WebSubmit.Headers.Add("X-EBAY-API-CALL-NAME", eBayFunction)
            WebSubmit.Timeout = ReceiveTimeout

            ' send to LerrynSecure.com (or the URL defined in the Registry)
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            pReturnedData = reader.ReadToEnd()

            If pReturnedData <> "" Then
                bSuccess = True                 ' Success (this just means no error occured and we got something back)
            Else
                LastError = "No response received from eBay API"
                bSuccess = False
            End If
            Return bSuccess

        Catch ex As Exception
            LastError = ex.Message & " - " & ex.StackTrace         ' Expose the error
            Return False

        Finally
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()

        End Try

    End Function

    Public Function PostXMLShopping(ByVal XMLToPost As String, ByVal eBayFunction As String, Optional ByVal ReceiveTimeout As Integer = 60000, _
        Optional ByVal bIncludeAPIVersion As Boolean = True) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Posts XML data to eBay shopping URL
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader
        Dim bSuccess As Boolean = False

        ' Vars for this only
        'Dim eBayAPIVersion As String = "791"
        Dim peBayXMLServerURL As String = "http://open.api.sandbox.ebay.com/shopping?"
        LastError = "" ' Clear any previous errors
        ' Store the posted XML
        pPostedXML = XMLToPost
        pReturnedData = ""

        byteData = UTF8Encoding.UTF8.GetBytes(XMLToPost)

        ' Send the XML message
        Try
            WebSubmit = System.Net.WebRequest.Create(peBayXMLServerURL)
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "text/xml; charset=utf-8"
            WebSubmit.ContentLength = byteData.Length

            WebSubmit.Headers.Add("X-EBAY-API-APP-ID", eBayAppID)
            WebSubmit.Headers.Add("X-EBAY-API-VERSION", eBayAPIVersion)
            WebSubmit.Headers.Add("X-EBAY-API-SITE-ID", eBayCountry.ToString)
            WebSubmit.Headers.Add("X-EBAY-API-CALL-NAME", eBayFunction)
            WebSubmit.Headers.Add("X-EBAY-API-REQUEST-ENCODING", "XML")
            WebSubmit.Timeout = ReceiveTimeout

            'WebSubmit.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", eBayAPIVersion)
            'WebSubmit.Headers.Add("X-EBAY-API-DEV-NAME", eBayDevID)
            'WebSubmit.Headers.Add("X-EBAY-API-CERT-NAME", eBayCertID)

            ' send to LerrynSecure.com (or the URL defined in the Registry)
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            pReturnedData = reader.ReadToEnd()

            If pReturnedData <> "" Then
                bSuccess = True                 ' Success (this just means no error occured and we got something back)
            Else
                LastError = "No response received from eBay API"
                bSuccess = False
            End If
            Return bSuccess

        Catch ex As Exception
            LastError = ex.Message & " - " & ex.StackTrace         ' Expose the error
            Return False

        Finally
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()
        End Try

    End Function

    Public Function GetSessionID(ByVal eBayRuName As String, ByRef eBaySessionID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Gets an eBay Session ID for use when fetching AuthTokens
        '                   http://http://developer.ebay.com/DevZone/XML/docs/Reference/eBay/GetSessionID.html
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/08/12 | TJS             | 2012.1.12 | Corrected error message to include correct function name
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLToPost As String, eBayFunction As String = "GetSessionID"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, False)
        XMLToPost = XMLToPost & "<RuName>" & eBayRuName & "</RuName>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        Try

            If PostXML(XMLToPost, eBayFunction) Then
                ' for some reason XPathSelectElement doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement("GetSessionIDResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GetSessionIDResponse/Ack").Value = "Success" Then
                    eBaySessionID = pReturnedXML.XPathSelectElement("GetSessionIDResponse/SessionID").Value
                    Return True

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement("GetSessionIDResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GetSessionIDResponse/Ack").Value = "Failure" Then
                    LastError = "GetSessionID returned an error - " & pReturnedXML.XPathSelectElement("GetSessionIDResponse/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement("GetSessionIDResponse/Errors/LongMessage").Value ' TJS 10/08/12
                    If Not IsNothing(pReturnedXML.XPathSelectElement("GetSessionIDResponse/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement("GetSessionIDResponse/Message").Value
                    End If
                    ErrorOccured = True
                    Return False

                Else
                    LastError = "GetSessionID did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If
            Else
                Return False
            End If

        Catch ex As Exception
            LastError = ex.Message
            ErrorOccured = True
            Return False

        End Try

    End Function

    Public Function FetchToken(ByVal eBaySessionID As String, ByRef AuthToken As String, ByRef TokenExpires As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Fetches an AuthToken created after GetSessionID has been used and the user sent to the eBay authorisation site
        '                   http://developer.ebay.com/DevZone/XML/docs/Reference/ebay/FetchToken.html
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/08/12 | TJS             | 2012.1.12 | Corrected error message to include correct function name
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLToPost As String, strTokenExpires As String, eBayFunction As String = "FetchToken"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, False)
        XMLToPost = XMLToPost & "<SessionID>" & eBaySessionID & "</SessionID>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        Try

            If PostXML(XMLToPost, eBayFunction) Then
                ' for some reason XPathSelectElement doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement("FetchTokenResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("FetchTokenResponse/Ack").Value = "Success" Then
                    AuthToken = pReturnedXML.XPathSelectElement("FetchTokenResponse/eBayAuthToken").Value
                    TokenExpires = pReturnedXML.XPathSelectElement("FetchTokenResponse/HardExpirationTime").Value
                    Return True

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement("FetchTokenResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("FetchTokenResponse/Ack").Value = "Failure" Then
                    LastError = "FetchToken returned an error - " & pReturnedXML.XPathSelectElement("FetchTokenResponse/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement("FetchTokenResponse/Errors/LongMessage").Value ' TJS 10/08/12
                    If Not IsNothing(pReturnedXML.XPathSelectElement("FetchTokenResponse/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement("FetchTokenResponse/Message").Value
                    End If
                    ErrorOccured = True
                    Return False

                Else
                    LastError = "GetSessionID did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If
            Else
                Return False
            End If

        Catch ex As Exception
            LastError = ex.Message
            ErrorOccured = True
            Return False

        End Try

    End Function

    Public Function GetOfficialEBayTime(ByRef EBayTime As Date) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Gets the official current eBay time
        '                   http://developer.ebay.com/DevZone/XML/docs/HowTo/FirstCall/MakingCallCSharp.html
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/08/12 | TJS             | 2012.1.12 | Corrected extraction of eBay error details using correct response name
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLToPost As String, strEBayTime As String, eBayFunction As String = "GeteBayOfficialTime"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, True)
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        Try

            If PostXML(XMLToPost, eBayFunction) Then
                ' for some reason XPathSelectElement doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Ack").Value = "Success" Then
                    strEBayTime = pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Timestamp").Value
                    EBayTime = DateSerial(CInt(Microsoft.VisualBasic.Left(strEBayTime, 4)), CInt(Microsoft.VisualBasic.Mid(strEBayTime, 6, 2)), CInt(Microsoft.VisualBasic.Mid(strEBayTime, 9, 2))).AddHours(CDbl(Microsoft.VisualBasic.Mid(strEBayTime, 12, 2))).AddMinutes(CDbl(Microsoft.VisualBasic.Mid(strEBayTime, 15, 2))).AddSeconds(CDbl(Microsoft.VisualBasic.Mid(strEBayTime, 18, 2))).AddMilliseconds(CDbl(Microsoft.VisualBasic.Mid(strEBayTime, 21, 3)))
                    Return True

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Ack").Value = "Failure" Then
                    LastError = "GeteBayOfficialTime returned an error - " & pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Errors/LongMessage").Value ' TJS 10/08/12
                    If Not IsNothing(pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement("GeteBayOfficialTimeResponse/Message").Value
                    End If
                    ErrorOccured = True
                    Return False

                Else
                    LastError = "GeteBayOfficialTime did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If
            Else
                Return False
            End If

        Catch ex As Exception
            LastError = ex.Message
            ErrorOccured = True
            Return False

        End Try

    End Function

    Public Function GetEBayDetails(ByRef DetailsToGet As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Gets the specified eBay details
        '                   This list was found here: http://developer.ebay.com/DevZone/xml/docs/Reference/ebay/types/DetailNameCodeType.html
        '                   Simply pass the first word as per below.
        '
        '                   BuyerRequirementDetails	        -  Details various eBay-buyer requirements.
        '                   CountryDetails	                -  Lists the country code and associated name of the countries supported by the eBay system.
        '                   CurrencyDetails	                -  Lists the currencies supported by the eBay system.
        '                   CustomCode	                    -  Reserved for future use.
        '                   DispatchTimeMaxDetails	        -  Details about maximum dispatch times.
        '                   ExcludeShippingLocationDetails	-  Lists the locations supported by the ExcludeShipToLocation feature. 
        '                                                      The codes reflect the ISO 3166 location codes.
        '                   ItemSpecificDetails	            -  Details about Item Specifics rules for the specified site.
        '                   ListingFeatureDetails	        -  Details the listing features available for the specified site.
        '                   ListingStartPriceDetails	    -  Lists the minimum starting prices for the supported types of eBay listings.
        '                   PaymentOptionDetails	        -  Not functional. Do not use this value. 
        '                                                      Formerly, this value was used to get details about specific payment options.
        '                   RecoupmentPolicyDetails	        -  Details the recoupment policies of the site.
        '                   RegionDetails	                -  Not functional. Do not use this value.
        '                   RegionOfOriginDetails	        -  Not functional. Do not use this value.
        '                                                      Details about the region of origin of a listing.
        '                   ReturnPolicyDetails	            -  Lists the return policies supported by the specified eBay site.
        '                   ShippingCarrierDetails	        -  Lists the shipping carriers supported by the specified site.
        '                   ShippingCategoryDetails	        -  Enumeration of the categories in which the shipping services available for the site belongs to.
        '                   ShippingLocationDetails	        -  Lists the regions and locations supported by eBays shipping services.
        '                   ShippingPackageDetails	        -  Lists the various shipping packages supported by the specified site.
        '                   ShippingServiceDetails	        -  Lists the shipping services supported by the specified eBay site.
        '                   SiteDetails	                    -  Lists the available eBay sites and their associated SiteID numbers.
        '                   TaxJurisdiction	                -  Details the different tax jurisdictions supported by the specified eBay site.
        '                   TimeZoneDetails	                -  Lists the details of the time zones supported by the eBay system.
        '                   UnitOfMeasurementDetails	    -  Lists the suggested unit-of-measurement strings to use with Item Specifics descriptions.
        '                   URLDetails	                    -  Lists the different eBay URLs associated with the specified eBay site.
        '                   VariationDetails	            -  Details the multi-variation listing rules for the site.
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLToPost As String, eBayFunction As String = "GeteBayDetails"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, True)
        XMLToPost = XMLToPost & "<DetailName>" & DetailsToGet & "</DetailName>"
        XMLToPost = XMLToPost & "<Version>" & eBayAPIVersion & "</Version>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        If PostXML(XMLToPost, eBayFunction) Then
            ' Feed the XML response into the XMLResponse object for processing
            Try
                ' for some reason XPathSelectElements doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Ack").Value = "Success" Then
                    Return True

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Ack").Value = "Failure" Then
                    LastError = "GeteBayDetails returned an error - " & pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Errors/LongMessage").Value
                    If Not IsNothing(pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement("GeteBayDetailsResponse/Message").Value
                    End If
                    ErrorOccured = True
                    Return False

                Else
                    LastError = "GeteBayDetails did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If

            Catch ex As Exception
                pReturnedXML = Nothing
                LastError = "GetEBayDetails response from " & peBayXMLServerURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")
                Return False

            End Try

        Else
            Return False
        End If

    End Function

    ' Pull in site details
    Public Function GetSiteDetails() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer

        Dim Name As String = ""
        Dim Value As String = ""

        aSiteDetails.Clear()

        Dim OneSiteDetails As SiteDetails

        GetEBayDetails("SiteDetails")

        m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/SiteDetails")

        Dim sNodeXML As String = ""
        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("SiteDetails").DescendantNodes

            OneSiteDetails = Nothing

            'With OneSiteDetails
            '    .Site = ""
            '    .SiteID = 0
            '    .DetailVersion = 0
            '    .UpdateTime = ""
            'End With

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount = 0 Or iElementCount Mod 2 = 0 And iElementCount < XMLChildList.Count Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")

                    Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                    ' Extract the property value
                    Value = StringArrayElement.Substring(iEndBracket)
                    iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                    Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))

                    Select Case Name.ToUpper
                        Case "SITE"
                            OneSiteDetails.Site = Value

                        Case "SITEID"
                            OneSiteDetails.SiteID = CInt(Value)

                        Case "DETAILVERSION"
                            OneSiteDetails.DetailVersion = CInt(Value)

                        Case "UPDATETIME"
                            OneSiteDetails.UpdateTime = Value

                    End Select

                End If
            Next

            aSiteDetails.Add(OneSiteDetails)

        Next

        Return bSuccess
    End Function

    Public Function GetCategories(ByVal VersionOnly As Boolean) As Boolean

        Dim XMLToPost As String, eBayFunction As String = "GetCategories"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, True)
        XMLToPost = XMLToPost & "<WarningLevel>High</WarningLevel>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        If PostXML(XMLToPost, eBayFunction) Then
            ' Feed the XML response into the XMLResponse object for processing
            Try
                ' for some reason XPathSelectElements doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement("GetCategoriesResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GetCategoriesResponse/Ack").Value = "Success" Then
                    Return True

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement("GetCategoriesResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GetCategoriesResponse/Ack").Value = "Failure" Then
                    LastError = "GetCategories returned an error - " & pReturnedXML.XPathSelectElement("GetCategoriesResponse/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement("GetCategoriesResponse/Errors/LongMessage").Value
                    If Not IsNothing(pReturnedXML.XPathSelectElement("GetCategoriesResponse/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement("GetCategoriesResponse/Message").Value
                    End If
                    ErrorOccured = True
                    Return Nothing

                Else
                    LastError = "GetCategories did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If

            Catch ex As Exception
                pReturnedXML = Nothing
                LastError = "GetCategories response from " & peBayXMLServerURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")
                Return False

            End Try

        Else
            Return False
        End If

    End Function

    Public Function GetSuggestedCategories(ByVal sDescription As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Gets suggested eBay categories from a description
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/11/11 | LJG             | 2011.1.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim eBayFunction As String = "GetSuggestedCategories"
        ErrorOccured = False
        Dim XMLToPost As String = ""
        Dim bSuccesss As Boolean = True
        XMLToPost = GetXMLHeader(eBayFunction, True)
        XMLToPost = XMLToPost & "<ErrorLanguage>en_GB</ErrorLanguage>"
        XMLToPost = XMLToPost & "<Version>" & eBayAPIVersion & "</Version>"
        XMLToPost = XMLToPost & "<Query>" & sDescription & "</Query>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        Try
            ErrorOccured = Not PostXML(XMLToPost, eBayFunction)
        Catch ex As Exception
            LastError = ex.Message
            ErrorOccured = True
        End Try

        Return Not ErrorOccured
    End Function

    Public Function GetOrders(ByVal CreateFilter As TimeFilter, ByVal ModifieldFilter As TimeFilter, ByVal OrderID As String, _
        ByVal OrderStatus As String, ByVal PageFilter As Integer) As Boolean ' TJS 10/06/12

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Gets orders frtom eBay
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/08/13 | FA              | 2013.1.37 | Modified to check for 'Warning' messages.  Order is not rejected if this occurs.  
        '                                        | Msg logged and email sent in WebPollIO.vb, PollEbay
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLToPost As String, eBayFunction As String = "GetOrders"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, True)
        If CreateFilter IsNot Nothing Then
            XMLToPost = XMLToPost & "<CreateTimeFrom>" & CreateXMLDate(CreateFilter.TimeFrom) & "</CreateTimeFrom>"
            XMLToPost = XMLToPost & "<CreateTimeTo>" & CreateXMLDate(CreateFilter.TimeTo) & "</CreateTimeTo>"
        End If
        XMLToPost = XMLToPost & "<OrderRole>Seller</OrderRole>"
        XMLToPost = XMLToPost & "<OrderStatus>" & OrderStatus & "</OrderStatus>" ' Active|Completed|Shipped
        XMLToPost = XMLToPost & "<Pagination>"
        XMLToPost = XMLToPost & "  <EntriesPerPage>20</EntriesPerPage>"
        XMLToPost = XMLToPost & "  <PageNumber>" & PageFilter & "</PageNumber>"
        XMLToPost = XMLToPost & "</Pagination>"
        If ModifieldFilter IsNot Nothing Then
            XMLToPost = XMLToPost & "<ModTimeFrom>" & CreateXMLDate(ModifieldFilter.TimeFrom) & "</ModTimeFrom>"
            XMLToPost = XMLToPost & "<ModTimeTo>" & CreateXMLDate(ModifieldFilter.TimeTo) & "</ModTimeTo>"
        End If
        If Not String.IsNullOrEmpty(OrderID) Then
            XMLToPost = XMLToPost & "<OrderIDArray><OrderID>" & OrderID & "</OrderID></OrderIDArray>" ' TJS 10/06/12
        End If
        XMLToPost = XMLToPost & "<IncludeFinalValueFee>true</IncludeFinalValueFee>"
        XMLToPost = XMLToPost & "<DetailLevel>ReturnAll</DetailLevel>"
        XMLToPost = XMLToPost & "<Version>" & eBayAPIVersion & "</Version>"
        XMLToPost = XMLToPost & "<WarningLevel>High</WarningLevel>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        If PostXML(XMLToPost, eBayFunction) Then
            ' Feed the XML response into the XMLResponse object for processing
            Try
                ' for some reason XPathSelectElements doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement("GetOrdersResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GetOrdersResponse/Ack").Value = "Success" Then
                    Return True

                    ' start of code added FA 18/08/13
                    ' warnings such as 'Client schema out of date' warning will cause orders not to be processed.
                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement("GetOrdersResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GetOrdersResponse/Ack").Value = "Warning" Then
                    Return True
                    ' end of code added FA 18/08/13

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement("GetOrdersResponse/Ack")) AndAlso pReturnedXML.XPathSelectElement("GetOrdersResponse/Ack").Value = "Failure" Then
                    LastError = "GetOrders returned an error - " & pReturnedXML.XPathSelectElement("GetOrdersResponse/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement("GetOrdersResponse/Errors/LongMessage").Value
                    If Not IsNothing(pReturnedXML.XPathSelectElement("GetOrdersResponse/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement("GetOrdersResponse/Message").Value
                    End If
                    ErrorOccured = True
                    Return Nothing

                Else
                    LastError = "GetOrders did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If

            Catch ex As Exception
                pReturnedXML = Nothing
                LastError = "GetOrders response from " & peBayXMLServerURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")
                Return False

            End Try

        Else
            Return False
        End If

    End Function

    Public Function CompleteSale(ByVal OrderID As String, ByVal CommentType As String, ByVal CommentText As String, ByVal TargetUser As String, _
        ByVal ShipmentNotes As String, ByRef eBayShippingDetails() As eBayShipmentDetails, ByVal Shipped As Boolean) As Boolean

        Dim XMLToPost As String, eBayFunction As String = "CompleteSale"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, True)
        If CommentType <> "" Then
            XMLToPost = XMLToPost & "<FeedbackInfo>"
            If CommentType <> "" Then
                XMLToPost = XMLToPost & "<CommentType>" & CommentType & "</CommentType>" ' e.g. Positive
            End If
            If CommentText <> "" Then
                XMLToPost = XMLToPost & "<CommentText>" & CommentText & "</CommentText>"
            End If
            If TargetUser <> "" Then
                XMLToPost = XMLToPost & "<TargetUser>" & TargetUser & "</TargetUser>"
            End If
            XMLToPost = XMLToPost & "</FeedbackInfo>"
        End If
        XMLToPost = XMLToPost & "<OrderID>" & OrderID & "</OrderID>"
        If eBayShippingDetails(0).TrackingNumber <> "" Then
            XMLToPost = XMLToPost & "<Shipment>"
            If ShipmentNotes <> "" Then
                XMLToPost = XMLToPost & "<Notes>" & ShipmentNotes & "</Notes>"
            End If
            For Each shipment As eBayShipmentDetails In eBayShippingDetails
                If shipment.TrackingNumber <> "" And shipment.CarrierUsed <> "" Then
                    XMLToPost = XMLToPost & "<ShipmentTrackingDetails>"
                    XMLToPost = XMLToPost & "<ShipmentTrackingNumber>" & shipment.TrackingNumber & "</ShipmentTrackingNumber>"
                    XMLToPost = XMLToPost & "<ShippingCarrierUsed>" & shipment.CarrierUsed & "</ShippingCarrierUsed>"
                    XMLToPost = XMLToPost & "</ShipmentTrackingDetails>"
                End If
            Next
            XMLToPost = XMLToPost & "</Shipment>"
        End If
        If Shipped Then
            XMLToPost = XMLToPost & "<Shipped>True</Shipped>"
        Else
            XMLToPost = XMLToPost & "<Shipped>False</Shipped>"
        End If
        XMLToPost = XMLToPost & "<WarningLevel>High</WarningLevel>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        If PostXML(XMLToPost, eBayFunction) Then
            ' Feed the XML response into the XMLResponse object for processing
            Try
                ' for some reason XPathSelectElements doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack")) AndAlso pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack").Value = "Success" Then
                    Return True

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack")) AndAlso pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack").Value = "Failure" Then
                    LastError = eBayFunction & " returned an error - " & pReturnedXML.XPathSelectElement(eBayFunction & "Response/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement(eBayFunction & "Response/Errors/LongMessage").Value
                    If Not IsNothing(pReturnedXML.XPathSelectElement(eBayFunction & "Response/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement(eBayFunction & "Response/Message").Value
                    End If
                    ErrorOccured = True
                    Return Nothing

                Else
                    LastError = eBayFunction & " did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If

            Catch ex As Exception
                pReturnedXML = Nothing
                LastError = eBayFunction & " response from " & peBayXMLServerURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")
                Return False

            End Try

        Else
            Return False
        End If

    End Function

    Public Function SendItemQuantityUpdates(ByRef InventoryStatusElements() As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Send Quantity Updates for Fixed Price auction items
        '                   http://developer.ebay.com/Devzone/XML/docs/Reference/eBay/ReviseInventoryStatus.html
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLToPost As String, eBayFunction As String = "ReviseInventoryStatus"

        ErrorOccured = False

        XMLToPost = GetXMLHeader(eBayFunction, True)
        For Each InventoryStatusXML As String In InventoryStatusElements
            XMLToPost = XMLToPost & InventoryStatusXML
        Next
        XMLToPost = XMLToPost & "<WarningLevel>High</WarningLevel>"
        XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

        If PostXML(XMLToPost, eBayFunction) Then
            ' Feed the XML response into the XMLResponse object for processing
            Try
                ' for some reason XPathSelectElements doesn't like having this namespace included
                pReturnedXML = XDocument.Parse(pReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                If Not IsNothing(pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack")) AndAlso pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack").Value = "Success" Then
                    Return True

                ElseIf Not IsNothing(pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack")) AndAlso pReturnedXML.XPathSelectElement(eBayFunction & "Response/Ack").Value = "Failure" Then
                    LastError = eBayFunction & " returned an error - " & pReturnedXML.XPathSelectElement(eBayFunction & "Response/Errors/ShortMessage").Value & ", " & pReturnedXML.XPathSelectElement(eBayFunction & "Response/Errors/LongMessage").Value
                    If Not IsNothing(pReturnedXML.XPathSelectElement(eBayFunction & "Response/Message")) Then
                        LastError = LastError & vbCrLf & pReturnedXML.XPathSelectElement(eBayFunction & "Response/Message").Value
                    End If
                    ErrorOccured = True
                    Return Nothing

                Else
                    LastError = eBayFunction & " did not return a Success status"
                    ErrorOccured = True
                    Return False
                End If

            Catch ex As Exception
                pReturnedXML = Nothing
                LastError = eBayFunction & " response from " & peBayXMLServerURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")
                Return False

            End Try

        Else
            Return False
        End If

    End Function

    ' Pull in eBay countries
    Public Function GeteBayCountries() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean
        Dim OneCountry As CountryDetails

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer

        Dim Name As String = ""
        Dim Value As String = ""

        aCountryDetails.Clear()

        GetEBayDetails("CountryDetails")

        m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/CountryDetails")

        Dim sNodeXML As String = ""
        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("CountryDetails").DescendantNodes

            With OneCountry
                .Country = ""
                .Description = ""
                .DetailVersion = 0
                .UpdateTime = ""
            End With

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount = 0 Or iElementCount Mod 2 = 0 And iElementCount < XMLChildList.Count Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")

                    Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                    ' Extract the property value
                    Value = StringArrayElement.Substring(iEndBracket)
                    iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                    Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))

                    Select Case Name.ToUpper
                        Case "COUNTRY"
                            OneCountry.Country = Value

                        Case "DESCRIPTION"
                            OneCountry.Description = Value

                        Case "DETAILVERSION"
                            OneCountry.DetailVersion = CInt(Value)

                        Case "UPDATETIME"
                            OneCountry.UpdateTime = Value
                    End Select

                End If
            Next

            aCountryDetails.Add(OneCountry)

        Next

        Return bSuccess

    End Function

    ' Pull in eBay currencies
    Public Function GetEbayCurrencies() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean
        Dim OneCurrency As CurrencyDetails

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer

        Dim Name As String = ""
        Dim Value As String = ""

        aCurrencyDetails.Clear()

        GetEBayDetails("CurrencyDetails")

        m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/CurrencyDetails")

        Dim sNodeXML As String = ""
        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("CurrencyDetails").DescendantNodes

            With OneCurrency
                .Currency = ""
                .Description = ""
                .DetailVersion = 0
                .UpdateTime = ""
            End With

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount = 0 Or iElementCount Mod 2 = 0 And iElementCount < XMLChildList.Count Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")

                    Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                    ' Extract the property value
                    Value = StringArrayElement.Substring(iEndBracket)
                    iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                    Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))

                    Select Case Name.ToUpper
                        Case "CURRENCY"
                            OneCurrency.Currency = Value

                        Case "DESCRIPTION"
                            OneCurrency.Description = Value

                        Case "DETAILVERSION"
                            OneCurrency.DetailVersion = CInt(Value)

                        Case "UPDATETIME"
                            OneCurrency.UpdateTime = Value
                    End Select

                End If
            Next

            aCurrencyDetails.Add(OneCurrency)

        Next

        Return bSuccess
    End Function

    ' Pull in shipping service details
    Public Function GetShippingServiceDetails() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer

        Dim Name As String = ""
        Dim Value As String = ""
        Dim iLerrynID As Integer = 1

        aShippingLocationDetails.Clear()

        Dim OneShippingLocationDetails As ShippingLocationDetails
        Dim clsShippingLocationDetails As New clsShippingLocationDetails
        GetEBayDetails("ShippingLocationDetails")

        Dim sTimeStamp As String, sACK As String, sVersion As String, sBuild As String, sDetailVersion As String

        ' Get <Timestamp> <Ack> <Version> <Build> etc  values
        m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Timestamp")
            sTimeStamp = XMLNodeList.Value
        Catch ex As Exception
            sTimeStamp = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Ack")
            sACK = XMLNodeList.Value
        Catch ex As Exception
            sACK = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Version")
            sVersion = XMLNodeList.Value
        Catch ex As Exception
            sVersion = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Build")
            sBuild = XMLNodeList.Value
        Catch ex As Exception
            sBuild = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/ReturnPolicyDetails/DetailVersion")
            sDetailVersion = XMLNodeList.Value
        Catch ex As Exception
            sDetailVersion = ""
        End Try

        m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/ShippingLocationDetails")

        clsShippingLocationDetails = New clsShippingLocationDetails

        Dim sNodeXML As String = ""
        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("ShippingLocationDetails").DescendantNodes

            OneShippingLocationDetails = Nothing

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount < XMLChildList.Count Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")

                    If iStartBracket > 0 And iEndBracket > iStartBracket Then
                        Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                        ' Extract the property value
                        Value = StringArrayElement.Substring(iEndBracket)
                        iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                        Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))
                    Else
                        Continue For
                    End If

                    Select Case Name.ToUpper
                        Case "SHIPPINGLOCATION"
                            OneShippingLocationDetails.ShippingLocation = Value
                        Case "DESCRIPTION"
                            OneShippingLocationDetails.Description = Value
                        Case "DETAILVERSION"
                            OneShippingLocationDetails.DetailVersion = Value
                        Case "UPDATETIME"
                            OneShippingLocationDetails.UpdateTime = Value
                        Case Else

                    End Select
                End If
            Next

            OneShippingLocationDetails.LerrynID = iLerrynID
            iLerrynID = iLerrynID + 1
            With clsShippingLocationDetails
                .Timestamp = sTimeStamp
                .Ack = sACK
                .Version = CInt(sVersion)
                .Build = sBuild
                .ShippingLocationDetails.Add(OneShippingLocationDetails)
            End With

            aShippingLocationDetails.Add(clsShippingLocationDetails)
            clsShippingLocationDetails = New clsShippingLocationDetails
        Next

        Return bSuccess
    End Function

    Public Function GetReturnsPolicy() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer

        Dim Name As String = ""
        Dim Value As String = ""

        GetEBayDetails("ReturnPolicyDetails")

        Dim sTimeStamp As String, sACK As String, sVersion As String, sBuild As String, sDetailVersion As String
        ' Get <Timestamp> <Ack> <Version> <Build> etc  values

        m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Timestamp")
            sTimeStamp = XMLNodeList.Value
        Catch ex As Exception
            sTimeStamp = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Ack")
            sACK = XMLNodeList.Value
        Catch ex As Exception
            sACK = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Version")
            sVersion = XMLNodeList.Value
        Catch ex As Exception
            sVersion = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/Build")
            sBuild = XMLNodeList.Value
        Catch ex As Exception
            sBuild = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/ReturnPolicyDetails/DetailVersion")
            sDetailVersion = XMLNodeList.Value
        Catch ex As Exception
            sDetailVersion = ""
        End Try

        ' Now get the rest of the return policy details
        m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/ReturnPolicyDetails")

        Dim sNodeXML As String = ""

        Dim bChildNode As Boolean = False, sParent As String = ""

        Dim OneReturnPolicy As New clsReturnPolicyDetails
        Dim OneReturnPolicyDetailsRefund As ReturnPolicyDetailsRefund
        Dim OneReturnPolicyDetailsReturnsWithin As ReturnPolicyDetailsReturnsWithin
        Dim OneReturnPolicyDetailsReturnsAccepted As ReturnPolicyDetailsReturnsAccepted
        Dim OneReturnPolicyDetailsShippingCostPaidBy As ReturnPolicyDetailsShippingCostPaidBy
        Dim OneReturnPolicyDetailsShippingRestockingFeeValue As ReturnPolicyDetailsShippingRestockingFeeValue

        aReturnsPolicy.Clear()
        Dim iLerrynID As Integer = 1

        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("ReturnPolicyDetails").DescendantNodes

            OneReturnPolicyDetailsRefund = Nothing
            OneReturnPolicyDetailsReturnsWithin = Nothing
            OneReturnPolicyDetailsReturnsAccepted = Nothing
            OneReturnPolicyDetailsShippingCostPaidBy = Nothing
            OneReturnPolicyDetailsShippingRestockingFeeValue = Nothing

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount < XMLChildList.Count Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()

                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")
                    If iStartBracket > 0 And iEndBracket > 0 Then
                        Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                        Value = StringArrayElement.Substring(iEndBracket)
                        iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                        Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))
                    Else
                        Continue For
                    End If

                    If bChildNode Then
                        Select Case sParent.ToUpper
                            Case "REFUND"
                                OneReturnPolicyDetailsRefund.LerrynID = iLerrynID
                                Select Case Name.ToUpper
                                    Case "REFUNDOPTION"
                                        OneReturnPolicyDetailsRefund.RefundOption = Value
                                    Case "DESCRIPTION"
                                        OneReturnPolicyDetailsRefund.Description = Value
                                        OneReturnPolicy.Refund.Add(OneReturnPolicyDetailsRefund)
                                        iLerrynID = iLerrynID + 1
                                        OneReturnPolicyDetailsRefund = Nothing
                                        bChildNode = False
                                End Select

                            Case "RETURNSWITHIN"
                                OneReturnPolicyDetailsReturnsWithin.LerrynID = iLerrynID
                                Select Case Name.ToUpper
                                    Case "RETURNSWITHINOPTION"
                                        OneReturnPolicyDetailsReturnsWithin.ReturnsWithinOption = Value
                                    Case "DESCRIPTION"
                                        OneReturnPolicyDetailsReturnsWithin.Description = Value
                                        OneReturnPolicy.ReturnsWithin.Add(OneReturnPolicyDetailsReturnsWithin)
                                        iLerrynID = iLerrynID + 1
                                        bChildNode = False
                                End Select

                            Case "RETURNSACCEPTED"
                                OneReturnPolicyDetailsReturnsAccepted.LerrynID = iLerrynID
                                Select Case Name.ToUpper
                                    Case "RETURNSACCEPTEDOPTION"
                                        OneReturnPolicyDetailsReturnsAccepted.ReturnsAcceptedOption = Value
                                    Case "DESCRIPTION"
                                        OneReturnPolicyDetailsReturnsAccepted.Description = Value
                                        OneReturnPolicyDetailsReturnsAccepted.DetailVersion = CInt(sDetailVersion)
                                        OneReturnPolicy.ReturnsAccepted.Add(OneReturnPolicyDetailsReturnsAccepted)
                                        iLerrynID = iLerrynID + 1
                                        bChildNode = False
                                End Select

                            Case "SHIPPINGCOSTPAIDBY"
                                OneReturnPolicyDetailsShippingCostPaidBy.LerrynID = iLerrynID
                                Select Case Name.ToUpper
                                    Case "SHIPPINGCOSTPAIDBYOPTION"
                                        OneReturnPolicyDetailsShippingCostPaidBy.ShippingCostPaidByOption = Value
                                    Case "DESCRIPTION"
                                        OneReturnPolicyDetailsShippingCostPaidBy.Description = Value
                                        OneReturnPolicy.ShippingCostPaidBy.Add(OneReturnPolicyDetailsShippingCostPaidBy)
                                        iLerrynID = iLerrynID + 1
                                        bChildNode = False
                                End Select

                            Case "RESTOCKINGFEEVALUE"
                                OneReturnPolicyDetailsShippingRestockingFeeValue.LerrynID = iLerrynID
                                Select Case Name.ToUpper
                                    Case "RESTOCKINGFEEVALUEOPTION"
                                        OneReturnPolicyDetailsShippingRestockingFeeValue.RestockingFeeValueOption = Value
                                    Case "DESCRIPTION"
                                        OneReturnPolicyDetailsShippingRestockingFeeValue.Description = Value
                                        OneReturnPolicy.RestockingFeeValue.Add(OneReturnPolicyDetailsShippingRestockingFeeValue)
                                        iLerrynID = iLerrynID + 1
                                        bChildNode = False
                                End Select
                        End Select
                    Else
                        ' Extract the property value
                        Select Case Name.ToUpper
                            Case "TIMESTAMP"
                                OneReturnPolicy.Timestamp = Value

                            Case "ACK"
                                OneReturnPolicy.Ack = Value

                            Case "VERSION"
                                OneReturnPolicy.Version = CInt(Value)

                            Case "BUILD"
                                OneReturnPolicy.Build = Value

                            Case "DETAILVERSION"
                                OneReturnPolicy.DetailVersion = CInt(Value)

                            Case "UPDATETIME"
                                OneReturnPolicy.UpdateTime = Value

                            Case "REFUND"
                                bChildNode = True
                                sParent = Name

                            Case "RETURNSWITHIN"
                                bChildNode = True
                                sParent = Name

                            Case "RETURNSACCEPTED"
                                bChildNode = True
                                sParent = Name

                            Case "SHIPPINGCOSTPAIDBY"
                                bChildNode = True
                                sParent = Name

                            Case "RESTOCKINGFEEVALUE"
                                bChildNode = True
                                sParent = Name

                            Case "DESCRIPTION"
                                OneReturnPolicy.Description = EBayStringCompare(Value.ToUpper, "TRUE")

                            Case Else

                        End Select
                    End If

                End If
            Next

            With OneReturnPolicy
                .Timestamp = sTimeStamp
                .Ack = sACK
                .Version = CInt(sVersion)
                .Build = sBuild
            End With

            aReturnsPolicy.Add(OneReturnPolicy)
            iLerrynID = iLerrynID + 1
        Next

        GetEBayDetails("AddReturnsPolicyIfRequired")
        ' GetReturnsPolicy2()
        Return bSuccess
    End Function

    Public Function LookupCategoryFeature(ByRef iCategoryID As Integer, ByRef sCategoryFeature As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sFeature As String = ""
        Dim OneCategoryDefault As CategoryDefaults = Nothing
        For Each OneCategoryDefault In aCategoryDefaults
            If OneCategoryDefault.CategoryID = iCategoryID And _
                OneCategoryDefault.Name.ToUpper = sCategoryFeature.ToUpper Then
                sFeature = OneCategoryDefault.Value
                Exit For
            End If
        Next

        Return sFeature
    End Function

    Public Function LookupCountry(sCountryCode As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False
        Dim OneCountry As CountryDetails

        For Each OneCountry In aCountryDetails
            If OneCountry.Country.ToUpper = sCountryCode.ToUpper Then
                CountryInUse = OneCountry
                bSuccess = True
                Exit For
            End If
        Next

        Return bSuccess

    End Function

    Public Function LookupCurrency(sCurrencyCode As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False
        Dim OneCurrency As CurrencyDetails

        For Each OneCurrency In aCurrencyDetails
            If OneCurrency.Currency.ToUpper = sCurrencyCode.ToUpper Then
                CurrencyInUse = OneCurrency
                bSuccess = True
                Exit For
            End If
        Next

        Return bSuccess

    End Function

    Public Function LookupShippingService(iShippingServiceID As Integer) As clsShippingServiceDetails
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False
        Dim SelectedShippingService As clsShippingServiceDetails = Nothing
        Dim OneShippingServiceDetails As clsShippingServiceDetails

        For Each OneShippingServiceDetails In aclsShippingServiceDetails
            If OneShippingServiceDetails.ShippingServiceID = iShippingServiceID Then
                SelectedShippingService = OneShippingServiceDetails
                Exit For
            End If
        Next

        Return SelectedShippingService

    End Function

    Public Function LookupSiteDetails(iSiteDetailsID As Integer) As SiteDetails
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False
        Dim OneSiteDetails As SiteDetails
        Dim SelectedSiteDetails As SiteDetails = Nothing

        For Each OneSiteDetails In aSiteDetails
            If OneSiteDetails.SiteID = iSiteDetailsID Then
                SelectedSiteDetails = OneSiteDetails
                bSuccess = True
                Exit For
            End If
        Next

        Return SelectedSiteDetails

    End Function

    Public Function AddCategoryDefaultsIfRequired(ByRef CategoryDefaultsToAdd As CategoryDefaults) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Dim OneCategoryDefault As CategoryDefaults = Nothing
        Dim bAdd As Boolean = True
        Dim iMax As Integer = 0

        For Each OneCategoryDefault In aCategoryDefaults

            If OneCategoryDefault.LerrynID > iMax Then
                iMax = OneCategoryDefault.LerrynID
            End If

            If OneCategoryDefault.CategoryID = CategoryDefaultsToAdd.CategoryID And _
                OneCategoryDefault.Name = CategoryDefaultsToAdd.Name And _
                OneCategoryDefault.LerrynID = CategoryDefaultsToAdd.LerrynID And _
                OneCategoryDefault.Type = CategoryDefaultsToAdd.Type And _
                OneCategoryDefault.Value = CategoryDefaultsToAdd.Value Then
                bAdd = False
            End If
        Next

        If bAdd Then
            CategoryDefaultsToAdd.LerrynID = iMax + 1
            aCategoryDefaults.Add(CategoryDefaultsToAdd)
        End If

        Return bSuccess
    End Function

    Public Function AddCategoryDetailsIfRequired(ByRef CategoryDetailsToAdd As CategoryListingDurations) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Dim OneCategory As CategoryListingDurations
        Dim bAdd As Boolean = True
        Dim iMax As Integer = 0

        For Each OneCategory In aCategoryListingDurations

            If OneCategory.LerrynID > iMax Then
                iMax = OneCategory.LerrynID
            End If

            If OneCategory.CategoryID = CategoryDetailsToAdd.CategoryID And _
                OneCategory.ID = CategoryDetailsToAdd.ID And _
                OneCategory.Duration = CategoryDetailsToAdd.Duration And _
               OneCategory.SiteID = CategoryDetailsToAdd.SiteID Then
                bAdd = False      ' We allready have this category ID, duration ID and SiteID , so do not add
                'Exit For
            End If
        Next

        If bAdd Then
            CategoryDetailsToAdd.LerrynID = iMax + 1
            aCategoryListingDurations.Add(CategoryDetailsToAdd)
        End If

        Return bSuccess
    End Function

    Public Function AddConditionIfRequired(ByRef ConditionToAdd As ConditionValues) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Dim OneCondition As ConditionValues
        Dim bAdd As Boolean = True

        For Each OneCondition In aConditionValues
            If OneCondition.CategoryID = ConditionToAdd.CategoryID And _
                OneCondition.ID = ConditionToAdd.ID Then
                bAdd = False    ' We allready have this category and condition, so do not add
                Exit For
            End If
        Next

        If bAdd Then
            aConditionValues.Add(ConditionToAdd)
        End If

        Return bSuccess
    End Function

    Public Function AddOneSiteDefaultIfRequired(ByRef OneSiteDefaultsToAdd As SiteDefaults) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Dim OneSiteDefaults As SiteDefaults
        Dim bAdd As Boolean = True

        For Each OneSiteDefaults In aSiteDefaults
            If OneSiteDefaults.Name = OneSiteDefaultsToAdd.Name And
                OneSiteDefaults.SiteID = OneSiteDefaultsToAdd.SiteID And
                OneSiteDefaults.TagName = OneSiteDefaultsToAdd.TagName And
                OneSiteDefaults.Type = OneSiteDefaultsToAdd.Type And
                OneSiteDefaults.Value = OneSiteDefaultsToAdd.Value Then
                bAdd = False       ' We allready have this SiteDefault
                Exit For
            End If
        Next

        If bAdd Then
            aSiteDefaults.Add(OneSiteDefaultsToAdd)
        End If

        Return bSuccess
    End Function

    Public Function AddOneCatalogEnabledIfRequired(ByRef OneCategoryCatalogEnabledToAdd As CategoryCatalogEnabled) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Dim OneCatalogEnabled As CategoryCatalogEnabled
        Dim bAdd As Boolean = True

        For Each OneCatalogEnabled In CatalogEnabled
            If OneCatalogEnabled.CategoryID = OneCategoryCatalogEnabledToAdd.CategoryID And
                OneCatalogEnabled.CatalogEnabled = OneCategoryCatalogEnabledToAdd.CatalogEnabled Then
                bAdd = False       ' We allready have this categories CatalogEnabled status
                Exit For
            End If
        Next

        If bAdd Then
            CatalogEnabled.Add(OneCategoryCatalogEnabledToAdd)
        End If

        Return bSuccess
    End Function

    Public Function VerifyAddItem(sXMLToPost As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True

        bSuccess = PostXML(sXMLToPost, "VerifyAddItem")
        Return bSuccess

    End Function

    Public Function AddItem(sXMLToPost As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        bSuccess = PostXML(sXMLToPost, "AddItem")
        Return bSuccess

    End Function

    Public Function FindProducts(Optional iCategoryID As Integer = 0, Optional ByRef sQueryKeywords As String = "", Optional ByRef iMaxEntries As Integer = 10) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Dim sXMLToPost As String, eBayFunction As String = "FindProducts"
        Dim sXMLResponse As String = ""

        sXMLToPost = GetXMLHeader(eBayFunction, True)

        '<?xml version="1.0" encoding="utf-8"?>
        '<FindProductsRequest xmlns="urn:ebay:apis:eBLBaseComponents">
        '<AvailableItemsOnly>true</AvailableItemsOnly>
        '<MaxEntries>10</MaxEntries>
        '<CategoryID>111422</CategoryID>
        '<HideDuplicateItems>true</HideDuplicateItems>
        '</FindProductsRequest>

        sXMLToPost = sXMLToPost & "<ErrorLanguage>en_GB</ErrorLanguage>"
        sXMLToPost = sXMLToPost & "<Version>" & eBayAPIVersion & "</Version>"

        sXMLToPost = sXMLToPost & "<AvailableItemsOnly>true</AvailableItemsOnly>"
        If iCategoryID > 0 Then
            sXMLToPost = sXMLToPost & "<CategoryID>" & iCategoryID.ToString & "</CategoryID>"
        End If
        'sXMLToPost = sXMLToPost & "<DomainName> string </DomainName>"

        sXMLToPost = sXMLToPost & "<HideDuplicateItems>true</HideDuplicateItems>"
        'sXMLToPost = sXMLToPost & "<IncludeSelector> string </IncludeSelector>"
        sXMLToPost = sXMLToPost & "<MaxEntries>" & iMaxEntries.ToString & "</MaxEntries>"
        'sXMLToPost = sXMLToPost & "<PageNumber> int </PageNumber>"
        'sXMLToPost = sXMLToPost & "<ProductID type="ProductIDCodeType"> ProductIDType (string) </ProductID>
        'sXMLToPost = sXMLToPost & "<ProductSort> ProductSortCodeType </ProductSort>"
        If sQueryKeywords <> "" Then
            sXMLToPost = sXMLToPost & "<QueryKeywords>" & ConvertForXML(sQueryKeywords) & "</QueryKeywords>"
        End If

        'sXMLToPost = sXMLToPost & "<SortOrder> SortOrderCodeType </SortOrder>"

        sXMLToPost = sXMLToPost & "</" & eBayFunction & "Request>"

        Try
            bSuccess = PostXML(sXMLToPost, eBayFunction, , , "SHOPPING")
        Catch ex As Exception
            LastError = ex.Message
            bSuccess = False
        End Try

        If bSuccess Then
            ' Parse the returned XML
            sXMLResponse = ReturnedData
        End If

        Return bSuccess

    End Function

    Public Function GetCategoryFeaturesFromXML(sXML As String, iCategoryID As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer

        Dim NoOfConditionsForCategory As Integer = 0

        Dim Name As String = ""
        Dim Value As String = ""

        Dim OneCondition As ConditionValues

        Dim sName As String = ""
        Dim sTagName As String = ""
        Dim sType As String = ""
        Dim sValue As String = ""

        Dim bStoreSiteDefault As Boolean, bChildNodes As Boolean = False, sChildNodeIs As String = ""

        ' The intention with this XML parse is to pull more info about the category as it is required, not to parse the whole thing incase it is needed
        ' At the moment we need:

        ' ConditionValues
        ' ListingDuration

        m_ReturnedXML = XDocument.Parse(sXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GetCategoryFeaturesResponse/Category/ConditionValues/Condition")

        aConditionValues.Clear()

        Dim sNodeXML As String = ""
        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("Condition").DescendantNodes

            OneCondition = Nothing

            With OneCondition
                .CategoryID = iCategoryID
            End With

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount < XMLChildList.Count And iElementCount Mod 2 = 0 Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")

                    Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                    ' Extract the property value
                    Value = StringArrayElement.Substring(iEndBracket)
                    iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                    Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))

                    Select Case Name.ToUpper
                        Case "ID"
                            OneCondition.ID = CInt(Value)
                            NoOfConditionsForCategory = NoOfConditionsForCategory + 1
                        Case "DISPLAYNAME"
                            OneCondition.DisplayName = Value

                    End Select

                End If
            Next
            ' Store the category (if it does not exist)
            AddConditionIfRequired(OneCondition)
        Next


        '''''''''''''''
        '''''''''''''''
        '''''''''''''''

        ' Pull out the Site defaults - Listing Duration 
        'Public Structure SiteDefaults
        '   Public TagName
        '   Public Name As String
        '   Public Type As String
        '   Public Value As String
        '   Public SiteID as Integer
        'End Structure
        'aSiteDefaults
        Dim OneSiteDefaults As SiteDefaults

        m_ReturnedXML = XDocument.Parse(sXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GetCategoryFeaturesResponse/SiteDefaults")

        aSiteDefaults.Clear()

        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("SiteDefaults").DescendantNodes

            OneSiteDefaults = Nothing

            With OneSiteDefaults
                .SiteID = eBayCountry
            End With

            Dim sTemp As String
            For iElementCount = 0 To XMLChildList.Count
                If iElementCount < XMLChildList.Count Or iElementCount = 0 Then

                    If Not bChildNodes Then
                        sName = ""
                        sTagName = ""
                        sType = ""
                        sValue = ""
                        bStoreSiteDefault = False
                    End If

                    '<GalleryFeaturedDurations>
                    '		<Duration>Days_7</Duration>
                    '		<Duration>Lifetime</Duration>
                    '</GalleryFeaturedDurations>

                    ' TagName GalleryFeaturedDurations
                    ' Name = "Duration"
                    ' Type = ""
                    ' Value = "Days_7"

                    StringArrayElement = XMLChildList(iElementCount).ToString()

                    'If InStr(StringArrayElement.ToUpper, "CombinedFixedPriceTreatmentEnabled".ToUpper) > 0 Then
                    '    sName = ""
                    'End If
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")
                    If iStartBracket > 0 Then
                        Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                    Else
                        Name = StringArrayElement
                    End If
                    ' Extract the property value
                    Value = StringArrayElement.Substring(iEndBracket)
                    iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                    If iEndBracket > 0 Then
                        Value = Value.Substring(0, iEndBracket - 1)
                    End If

                    If Name.ToUpper = Value.ToUpper Then
                        ' This is a data element only, next!
                        Continue For
                    End If

                    '<GalleryFeaturedDurations>
                    '		<Duration>Days_7</Duration>
                    '		<Duration>Lifetime</Duration>
                    '</GalleryFeaturedDurations>

                    ' TagName GalleryFeaturedDurations
                    ' Name = "Duration"
                    ' Type = ""
                    ' Value = "Days_7"

                    If bChildNodes And Name.ToUpper = sChildNodeIs.ToUpper Then
                        bStoreSiteDefault = True
                    Else
                        bChildNodes = False
                        sName = ""
                        sTagName = ""
                        sType = ""
                        sValue = ""
                        bStoreSiteDefault = False
                    End If

                    If Left(Name.ToUpper, 16) = "LISTINGDURATION " Then
                        bStoreSiteDefault = True
                        '	"<ListingDuration type="Chinese">1</ListingDuration>"
                        ' We need:

                        ' 	<ListingDuration type="Chinese">1</ListingDuration>
                        ' TagName ListingDuration
                        ' Name = type
                        ' Type = Chinese
                        ' Value = "1"

                        ' Pull TagName "ListingDuration" out
                        If InStr(StringArrayElement, " ") > 0 Then
                            sTemp = Left(StringArrayElement, InStr(StringArrayElement, " ") - 1)
                            If Left(sTemp, 1) = "<" Then
                                sTemp = sTemp.Substring(1)
                            End If
                            sTagName = sTemp
                        End If

                        ' Pull Name "type" out
                        If InStr(StringArrayElement, "=") > 0 Then
                            sTemp = Left(StringArrayElement, InStr(StringArrayElement, "=") - 1)
                            sName = sTemp.Substring(InStr(sTemp, " "))
                        End If

                        'Pull Type "Chinese" out
                        If InStr(StringArrayElement, "=") > 0 Then
                            sTemp = StringArrayElement.Substring(InStr(StringArrayElement, "=") + 1)
                            If InStr(sTemp, """") > 0 Then
                                sTemp = Left(sTemp, InStr(sTemp, """") - 1)
                            End If
                            sType = sTemp
                        End If

                        ' Pull Value "1" out
                        '<ListingDuration type="Chinese">1</ListingDuration>"
                        If InStr(StringArrayElement, """>") > 0 Then
                            sTemp = StringArrayElement.Substring(InStr(StringArrayElement, """>") + 1)
                            If InStr(sTemp, "<") > 0 Then
                                sTemp = Left(sTemp, InStr(sTemp, "<") - 1)
                            End If
                            sValue = sTemp
                        End If
                    Else
                        If Name <> Value Then
                            sName = Name
                            sValue = Value
                            bStoreSiteDefault = True
                        End If
                    End If

                    ' These elements have child nodes
                    '<GalleryFeaturedDurations>
                    '   <Duration>Days_7</Duration>
                    '   <Duration>Lifetime</Duration>
                    '</GalleryFeaturedDurations>

                    '<StoreOwnerExtendedListingDurations>
                    '   <Duration>GTC</Duration>
                    '</StoreOwnerExtendedListingDurations>

                    If Left(Name.ToUpper, Len("RevisePriceAllowed")) = "RevisePriceAllowed".ToUpper Then
                        bStoreSiteDefault = False
                    End If

                    If Left(Name.ToUpper, 24) = "GalleryFeaturedDurations".ToUpper Then
                        bChildNodes = True
                        sChildNodeIs = "Duration"
                        'ProcessSiteDefaultChildXML(StringArrayElement, sName, iCategoryID, eBayCountry)
                        bStoreSiteDefault = False
                        sTagName = Name
                    End If

                    If Left(Name.ToUpper, 34) = "StoreOwnerExtendedListingDurations".ToUpper And Len(Name) = 34 Then
                        bChildNodes = True
                        sChildNodeIs = "Duration"
                        'ProcessSiteDefaultChildXML(StringArrayElement, sName, iCategoryID, eBayCountry)
                        bStoreSiteDefault = False
                        sTagName = Name
                    End If

                    'Populate the SiteDefault
                    If bStoreSiteDefault Then
                        With OneSiteDefaults
                            .Name = sName
                            .TagName = sTagName
                            .Type = sType
                            .Value = m_eBayPublishingWizardFacade.ConvertFromXML(sValue)
                        End With

                        ' Store the SiteDefault (if it does not exist)
                        AddOneSiteDefaultIfRequired(OneSiteDefaults)

                        bStoreSiteDefault = False
                    End If

                End If
            Next
        Next

        '''''''''''''''
        '''''''''''''''
        '''''''''''''''

        ' Now pull out the category details and add to aCategoryDetails

        Dim sReturnPolicyEnabled As String = "", bReturnPolicyEnabled As Boolean = False

        Dim OneeBayCategoryDetails As CategoryListingDurations
        Dim iUnderscorePos As Integer, iTemp As Integer, sDuration As String = ""

        m_ReturnedXML = XDocument.Parse(sXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GetCategoryFeaturesResponse/FeatureDefinitions/ListingDurations")

        aCategoryListingDurations.Clear()

        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("ListingDurations").DescendantNodes

            OneeBayCategoryDetails = Nothing

            With OneeBayCategoryDetails
                .CategoryID = iCategoryID
                .SiteID = eBayCountry
            End With

            Dim sTemp As String, iID As Integer
            For iElementCount = 0 To XMLChildList.Count
                If iElementCount < XMLChildList.Count Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")
                    If iStartBracket > 0 Then
                        Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                    Else
                        Name = StringArrayElement
                    End If
                    ' Extract the property value
                    Value = StringArrayElement.Substring(iEndBracket)
                    iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                    If iEndBracket > 0 Then
                        Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))
                    End If

                    If Left(Name.ToUpper, 16) = "LISTINGDURATION " Then
                        'ListingDuration durationSetID="8"
                        'Pull the SETID out
                        sTemp = Name.Substring(InStr(Name, "="))
                        sTemp = sTemp.Replace("""", "")
                        Try
                            iID = CInt(sTemp)
                        Catch ex As Exception
                            iID = 0
                        End Try
                        OneeBayCategoryDetails.ID = iID
                    End If

                    If Name.ToUpper = "DURATION" Then
                        OneeBayCategoryDetails.Duration = Value

                        If InStr(OneeBayCategoryDetails.Duration.ToUpper, "DAY") > 0 Then
                            ' Hack Days_n to n Days
                            iUnderscorePos = InStr(OneeBayCategoryDetails.Duration, "_")
                            sTemp = OneeBayCategoryDetails.Duration.Substring(iUnderscorePos)
                            Try
                                iTemp = CInt(sTemp.Trim)
                            Catch ex As Exception
                                iTemp = 0
                                sDuration = OneeBayCategoryDetails.Duration
                            End Try

                            If iTemp > 0 Then
                                sDuration = iTemp.ToString & " Day"
                                If iTemp <> 1 Then
                                    sDuration = sDuration & "s"
                                End If
                            End If
                        Else
                            sDuration = OneeBayCategoryDetails.Duration
                        End If

                        OneeBayCategoryDetails.Duration_Screen = sDuration

                        ' Store the category (if it does not exist)
                        AddCategoryDetailsIfRequired(OneeBayCategoryDetails)
                    End If
                End If
            Next
        Next

        Return bSuccess
    End Function

    Public Function GetDurationFromID(ByRef iListingDuration As Integer) As CategoryListingDurations
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim OneListingDuration As CategoryListingDurations = Nothing

        Dim bSuccess As Boolean = False
        For Each OneListingDuration In aCategoryListingDurations
            If OneListingDuration.LerrynID = iListingDuration Then
                bSuccess = True
                Exit For
            End If
        Next

        If Not bSuccess Then
            OneListingDuration = Nothing
        End If

        Return OneListingDuration

    End Function

    Public Function GetReturnsPolicyFromID(ByRef iReturnsPolicyID As Integer) As ReturnPolicyDetailsReturnsAccepted
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False

        Dim OneReturnsPolicy As clsReturnPolicyDetails
        Dim OneReturn As ReturnPolicyDetailsReturnsAccepted = Nothing

        For Each OneReturnsPolicy In aReturnsPolicy
            For Each OneReturn In OneReturnsPolicy.ReturnsAccepted
                If OneReturn.LerrynID = iReturnsPolicyID Then
                    bSuccess = True
                    Exit For
                End If
            Next
        Next

        If Not bSuccess Then
            OneReturn = Nothing
        End If

        Return OneReturn

    End Function

    Public Function GetRefundOptionsFromID(ByRef iReturnsPolicyID As Integer) As ReturnPolicyDetailsRefund
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False

        Dim OneReturnsPolicy As clsReturnPolicyDetails
        Dim OneRefund As ReturnPolicyDetailsRefund = Nothing

        For Each OneReturnsPolicy In aReturnsPolicy
            For Each OneRefund In OneReturnsPolicy.Refund
                If OneRefund.LerrynID = iReturnsPolicyID Then
                    bSuccess = True
                    Exit For
                End If
            Next
        Next

        If Not bSuccess Then
            OneRefund = Nothing
        End If

        Return OneRefund

    End Function

    Public Function GetReturnPolicyDetailsReturnsWithinFromID(ByRef iReturnsPolicyID As Integer) As ReturnPolicyDetailsReturnsWithin
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False

        Dim OneReturnsPolicy As clsReturnPolicyDetails
        Dim OneReturnsWithin As ReturnPolicyDetailsReturnsWithin = Nothing

        For Each OneReturnsPolicy In aReturnsPolicy
            For Each OneReturnsWithin In OneReturnsPolicy.ReturnsWithin
                If OneReturnsWithin.LerrynID = iReturnsPolicyID Then
                    bSuccess = True
                    Exit For
                End If
            Next
        Next

        If Not bSuccess Then
            OneReturnsWithin = Nothing
        End If

        Return OneReturnsWithin
    End Function

    Public Function GetShippingCostsPaidByFromID(ByRef iReturnsPolicyID As Integer) As ReturnPolicyDetailsShippingCostPaidBy
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False

        Dim OneReturnsPolicy As clsReturnPolicyDetails
        Dim OneShippingPolicy As ReturnPolicyDetailsShippingCostPaidBy = Nothing

        For Each OneReturnsPolicy In aReturnsPolicy
            For Each OneShippingPolicy In OneReturnsPolicy.ShippingCostPaidBy
                If OneShippingPolicy.LerrynID = iReturnsPolicyID Then
                    bSuccess = True
                    Exit For
                End If
            Next
        Next

        If Not bSuccess Then
            OneShippingPolicy = Nothing
        End If

        Return OneShippingPolicy
    End Function

    Public Function GetShippingLocationDetailsFromID(ByRef iShippingLocationDetailsID As Integer) As ShippingLocationDetails
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = False

        Dim clsShippingLocationDetails As clsShippingLocationDetails
        Dim OneShippingLocationDetails As ShippingLocationDetails = Nothing

        For Each clsShippingLocationDetails In aShippingLocationDetails
            For Each OneShippingLocationDetails In clsShippingLocationDetails.ShippingLocationDetails
                If OneShippingLocationDetails.LerrynID = iShippingLocationDetailsID Then
                    bSuccess = True
                    Exit For
                End If
            Next
        Next

        If Not bSuccess Then
            OneShippingLocationDetails = Nothing
        End If

        Return OneShippingLocationDetails
    End Function

    Public Function ParseAddItemResponseXML(sNodeXML As String, seBayFunction As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean
        Dim sStatus As String
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument, XMLNode As XElement
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement)

        Dim sShortMessage As String = ""
        Dim sLongMessage As String
        Dim sErrorCode As String = ""
        Dim sSeverityCode As String
        Dim sErrorClassification As String

        Dim OneError As AddeBayItemError = Nothing
        m_AddeBayItemError.Clear()

        bSuccess = True
        sStatus = ""

        Dim seBayCallStatus As String
        Dim sTimeStamp As String
        Dim sItemID As String

        ' Response from publish:
        '<AddItemResponse>    <Timestamp>2012-09-10T14:22:29.655Z</Timestamp>    <Ack>Success</Ack>    <Version>787</Version>    <Build>E787_INTL_BUNDLED_15262951_R1</Build>    <ItemID>110102487796</ItemID>    <StartTime>2012-09-10T14:22:29.061Z</StartTime>    <EndTime>2012-09-20T14:22:29.061Z</EndTime>    <Fees>      <Fee>        <Name>AuctionLengthFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>BoldFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>BuyItNowFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>CategoryFeaturedFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>FeaturedFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>GalleryPlusFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>FeaturedGalleryFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>FixedPriceDurationFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>GalleryFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>GiftIconFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>HighLightFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>InsertionFee</Name>        <Fee currencyID="GBP">0.4</Fee>      </Fee>      <Fee>        <Name>InternationalInsertionFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>ListingDesignerFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>ListingFee</Name>        <Fee currencyID="GBP">0.4</Fee>      </Fee>      <Fee>        <Name>PhotoDisplayFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>PhotoFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>ReserveFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>SchedulingFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>SubtitleFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>BorderFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>ProPackBundleFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>BasicUpgradePackBundleFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>ValuePackBundleFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>PrivateListingFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>ProPackPlusBundleFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>      <Fee>        <Name>MotorsGermanySearchFee</Name>        <Fee currencyID="GBP">0.0</Fee>      </Fee>    </Fees>  </AddItemResponse>

        m_ReturnedXML = XDocument.Parse(sNodeXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNodeList = m_ReturnedXML.XPathSelectElements(seBayFunction & "Response/Ack")

        ' Get the call status "Success" or "Failure"
        seBayCallStatus = ""
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements(seBayFunction & "Response/Ack")
            seBayCallStatus = XMLNodeList.Value
        Catch ex As Exception
            sStatus = "Failure - Unable to parse returned XML - " & seBayFunction & "Response/Ack"
        End Try

        sTimeStamp = ""
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements(seBayFunction & "Response/Timestamp")
            sTimeStamp = XMLNodeList.Value
        Catch ex As Exception
            sStatus = "Failure - Unable to parse returned XML - " & seBayFunction & "Response/Timestamp"
        End Try

        sItemID = ""
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements(seBayFunction & "Response/ItemID")
            sItemID = XMLNodeList.Value
        Catch ex As Exception
            sStatus = "Failure - Unable to parse returned XML - " & seBayFunction & "Response/ItemID"
        End Try

        ' See if we have the elements Ack, Timestamp and ItemID
        If sStatus <> "" Then
            bSuccess = False
        End If

        Select Case seBayCallStatus.ToUpper

            Case "FAILURE", "WARNING"
                bSuccess = False
                XMLNodeList = m_ReturnedXML.XPathSelectElements(seBayFunction & "Response/Errors")
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    sShortMessage = XMLTemp.XPathSelectElement("Errors/ShortMessage").Value()
                    sLongMessage = XMLTemp.XPathSelectElement("Errors/LongMessage").Value()
                    sErrorCode = XMLTemp.XPathSelectElement("Errors/ErrorCode").Value()
                    sSeverityCode = XMLTemp.XPathSelectElement("Errors/SeverityCode").Value()
                    sErrorClassification = XMLTemp.XPathSelectElement("Errors/ErrorClassification").Value()

                    ' Do we need the ErrorParameters or is Failure and ErrorCode enough?

                    '<Errors>
                    '        <ShortMessage>Invalid Domestic Dispatch Time.</ShortMessage>
                    '        <LongMessage>301 business day(s) is not a valid Domestic Dispatch Time on site 3.</LongMessage>
                    '        <ErrorCode>21806</ErrorCode>
                    '        <SeverityCode>Error</SeverityCode>
                    '        <ErrorParameters ParamID="0">
                    '            <Value>301</Value>
                    '        </ErrorParameters>
                    '        <ErrorParameters ParamID="1">
                    '            <Value>3</Value>
                    '        </ErrorParameters>
                    '        <ErrorClassification>RequestError</ErrorClassification>
                    '    </Errors>

                    ' Store the error info

                    With OneError
                        .Status = seBayCallStatus
                        .Timecode = sTimeStamp
                        .ShortMessage = sShortMessage
                        .LongMessage = sLongMessage
                        .ErrorCode = sErrorCode
                        .SeverityCode = sSeverityCode
                    End With

                    m_AddeBayItemError.Add(OneError)

                    ' Build up the status
                    sStatus = sStatus & sShortMessage & " - " & sErrorCode
                Next

            Case "SUCCESS"
                ' This will populate aeBayPostedItemFees
                bSuccess = ProcesseBayFees(m_ReturnedXML, sItemID, seBayFunction)

        End Select
        Return bSuccess
    End Function

    Public Function ParseFindProductsXML(ByRef sXML As String, ByRef iCategoryID As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer
        Dim iStartQuote As Integer
        'Dim iEndQuote As Integer

        Dim Name As String = ""
        Dim Value As String = ""
        Dim TypeVal As String = ""

        Dim OneCategoryCatalogEnabledToAdd As CategoryCatalogEnabled
        OneCategoryCatalogEnabledToAdd = Nothing
        OneCategoryCatalogEnabledToAdd.CategoryID = 0

        'GetEBayDetails("ReturnPolicyDetails")

        'Dim sTimeStamp As String, sACK As String, sVersion As String, sBuild As String, sDetailVersion As String
        ' Get <Timestamp> <Ack> <Version> <Build> etc  values
        Dim OneFindProductsResponse As New clsFindProductsResponse
        OneFindProductsResponse.CategoryID = iCategoryID

        m_ReturnedXML = XDocument.Parse(sXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))

        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/Timestamp")
            OneFindProductsResponse.Timestamp = XMLNodeList.Value
        Catch ex As Exception
            OneFindProductsResponse.Timestamp = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/Ack")
            OneFindProductsResponse.Ack = XMLNodeList.Value
        Catch ex As Exception
            OneFindProductsResponse.Ack = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/Build")
            OneFindProductsResponse.Build = XMLNodeList.Value
        Catch ex As Exception
            OneFindProductsResponse.Build = ""
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/Version")
            OneFindProductsResponse.Version = CInt(XMLNodeList.Value)
        Catch ex As Exception
            OneFindProductsResponse.Version = 0
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/ApproximatePages")
            OneFindProductsResponse.ApproximatePages = CInt(XMLNodeList.Value)
        Catch ex As Exception
            OneFindProductsResponse.ApproximatePages = 0
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/MoreResults")
            OneFindProductsResponse.MoreResults = EBayStringCompare(XMLNodeList.Value.ToUpper, "TRUE")
        Catch ex As Exception
            OneFindProductsResponse.MoreResults = False
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/PageNumber")
            OneFindProductsResponse.PageNumber = CInt(XMLNodeList.Value)
        Catch ex As Exception
            OneFindProductsResponse.PageNumber = 0
        End Try
        Try
            XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/TotalProducts")
            OneFindProductsResponse.TotalProducts = CInt(XMLNodeList.Value)
        Catch ex As Exception
            OneFindProductsResponse.TotalProducts = 0
        End Try

        Select Case OneFindProductsResponse.Ack.ToUpper
            Case "SUCCESS"

                ' Now get the rest of the return policy details
                m_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                XMLNameTabeBay = New System.Xml.NameTable
                XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
                XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/Product")

                Dim sNodeXML As String = ""

                Dim bChildNode As Boolean = False, sParent As String = ""

                Dim OneclsFindProductsResponseProduct As New clsFindProductsResponseProduct
                Dim OneFindProductsResponse_Product_ProductID As FindProductsResponse_Product_ProductID
                Dim OneclsFindProductsResponse_Product_ItemSpecifics As New clsFindProductsResponse_Product_ItemSpecifics
                Dim OneclsFindProductsResponse_Product_ItemSpecifics_NameValueList As New clsFindProductsResponse_Product_ItemSpecifics_NameValueList

                'aReturnsPolicy.Clear()
                Dim iLerrynID As Integer = 1

                For Each XMLNode In XMLNodeList

                    XMLTemp = XDocument.Parse(XMLNode.ToString)
                    XMLChildList = XMLTemp.XPathSelectElement("Product").DescendantNodes

                    OneclsFindProductsResponseProduct = New clsFindProductsResponseProduct
                    OneFindProductsResponse_Product_ProductID = Nothing
                    OneclsFindProductsResponse_Product_ItemSpecifics = New clsFindProductsResponse_Product_ItemSpecifics
                    OneclsFindProductsResponseProduct.ProductID.Clear()

                    For iElementCount = 0 To XMLChildList.Count
                        If iElementCount < XMLChildList.Count Then
                            StringArrayElement = XMLChildList(iElementCount).ToString()

                            ' Extract the property name
                            iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                            iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")
                            If iStartBracket > 0 And iEndBracket > 0 Then
                                Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)
                                Value = StringArrayElement.Substring(iEndBracket)
                                iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                                Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))
                            Else
                                Continue For
                            End If

                            If Left(Name.ToUpper, 15) = "PRODUCTID TYPE=" Then
                                TypeVal = ""
                                ' Pull "type" out
                                iStartQuote = Microsoft.VisualBasic.InStr(Name, """")
                                If iStartQuote > 0 Then
                                    TypeVal = Name.Substring(iStartQuote).Trim
                                    If Right(TypeVal, 1) = """" Then
                                        TypeVal = Left(TypeVal, Len(TypeVal) - 1)
                                    End If
                                    Name = "PRODUCTID"
                                End If
                            End If

                            If Name.ToUpper = "REVIEWCOUNT" And sParent.ToUpper = "ITEMSPECIFICS/NAMEVALUELIST" Then
                                bChildNode = False
                            End If

                            If bChildNode Then
                                Select Case sParent.ToUpper

                                    Case "ITEMSPECIFICS"
                                        sParent = sParent & "/" & Name

                                    Case "NAMEVALUELIST"
                                        sParent = Name

                                    Case "ITEMSPECIFICS/NAMEVALUELIST"
                                        Select Case Name.ToUpper
                                            Case "NAME"
                                                OneclsFindProductsResponse_Product_ItemSpecifics_NameValueList.Name = Value

                                            Case "VALUE"
                                                OneclsFindProductsResponse_Product_ItemSpecifics_NameValueList.LerrynID.Add(iLerrynID)
                                                iLerrynID = iLerrynID + 1
                                                OneclsFindProductsResponse_Product_ItemSpecifics_NameValueList.value.Add(Value)
                                                bChildNode = False
                                        End Select
                                End Select
                            Else
                                Select Case Name.ToUpper
                                    Case "DOMAINNAME"
                                        OneclsFindProductsResponseProduct.DomainName = Value
                                        OneclsFindProductsResponseProduct.LerrynID = iLerrynID
                                        iLerrynID = iLerrynID + 1
                                    Case "DETAILSURL"
                                        OneclsFindProductsResponseProduct.DetailsURL = Value

                                    Case "DISPLAYSTOCKPHOTOS"
                                        OneclsFindProductsResponseProduct.DisplayStockPhotos = EBayStringCompare(Value.ToUpper, "TRUE")

                                    Case "PRODUCTID"
                                        OneFindProductsResponse_Product_ProductID.ProductID = CInt(Value)
                                        OneFindProductsResponse_Product_ProductID.Type = TypeVal
                                        OneclsFindProductsResponseProduct.ProductID.Add(OneFindProductsResponse_Product_ProductID)
                                        OneFindProductsResponse_Product_ProductID = Nothing
                                    Case "REVIEWCOUNT"
                                        OneclsFindProductsResponseProduct.ReviewCount = CInt(Value)
                                    Case "STOCKPHOTOURL"
                                        OneclsFindProductsResponseProduct.StockPhotoURL = Value
                                    Case "TITLE"
                                        OneclsFindProductsResponseProduct.Title = Value

                                    Case "ITEMSPECIFICS"
                                        bChildNode = True
                                        sParent = Name

                                    Case Else

                                End Select
                            End If

                        End If
                    Next

                    ' Build up the product object

                    ' Add the Name and Value list to the ItemSpecifics
                    OneclsFindProductsResponse_Product_ItemSpecifics.NameValueList.Add(OneclsFindProductsResponse_Product_ItemSpecifics_NameValueList)

                    ' Add the ItemSpecifics to the Product
                    OneclsFindProductsResponseProduct.ItemSpecifics.Add(OneclsFindProductsResponse_Product_ItemSpecifics)

                    OneFindProductsResponse.Product.Add(OneclsFindProductsResponseProduct)
                    ' Add the product to the collection
                    paFindProductsResponseProducts.Add(OneFindProductsResponse)
                    iLerrynID = iLerrynID + 1

                    With OneCategoryCatalogEnabledToAdd
                        .CategoryID = iCategoryID
                        .CatalogEnabled = True
                    End With

                Next

            Case "FAILURE"
                Try
                    XMLNodeList = m_ReturnedXML.XPathSelectElements("FindProductsResponse/Errors/ErrorCode")
                    If XMLNodeList.Value.Trim = "10.84" Then
                        ' Category is not catalogEnabled not use FindProducts e.g.:
                        '<?xml version="1.0" encoding="UTF-8"?>
                        '<FindProductsResponse xmlns="urn:ebay:apis:eBLBaseComponents">
                        '	<Timestamp>2012-09-28T10:43:34.749Z</Timestamp>
                        '	<Ack>Failure</Ack>
                        '	<Errors>
                        '		<ShortMessage>Not catalog enabled.</ShortMessage>
                        '		<LongMessage>The specified category or domain is not catalog enabled. Please check your Input.</LongMessage>
                        '		<ErrorCode>10.84</ErrorCode>
                        '		<SeverityCode>Error</SeverityCode>
                        '		<ErrorClassification>RequestError</ErrorClassification>
                        '	</Errors>
                        '	<Build>E791_CORE_BUNDLED_15340089_R1</Build>
                        '	<Version>791</Version>
                        '</FindProductsResponse>

                        With OneCategoryCatalogEnabledToAdd
                            .CategoryID = iCategoryID
                            .CatalogEnabled = False
                        End With

                    End If
                Catch ex As Exception

                End Try
        End Select

        If OneCategoryCatalogEnabledToAdd.CategoryID > 0 Then
            AddOneCatalogEnabledIfRequired(OneCategoryCatalogEnabledToAdd)
        End If

        Return bSuccess

    End Function

    Private Function ProcesseBayFees(m_ReturnedXML As XDocument, sItemID As String, seBayFunction As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim XMLTemp As XDocument

        Dim iElementCount As Integer, Name As String, Value As String, StringArrayElement As String
        Dim iStartBracket As Integer, iEndBracket As Integer

        'm_ReturnedXML = XDocument.Parse(Me.ReturnedData.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements(seBayFunction & "Response/Fees")

        Dim OneFee As eBayFee
        Dim sNodeXML As String = ""
        Dim sTemp As String, sCurrency As String

        paeBayPostedItemFees.Clear()

        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("Fees").DescendantNodes

            OneFee = Nothing

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount < XMLChildList.Count Then 'Or iElementCount Mod 2 = 0 Then
                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")

                    If iStartBracket > 0 And iEndBracket > 0 Then
                        Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)

                        ' Extract the property value
                        Value = StringArrayElement.Substring(iEndBracket)
                        iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                        Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))

                        Select Case Name.ToUpper.Trim
                            Case "NAME"
                                OneFee.Name = Value

                            Case "FEE"
                                If Value.Trim <> "" Then
                                    OneFee.Fee = CDec(Value)
                                End If

                        End Select

                        If Left(Name.ToUpper, 14) = "FEE CURRENCYID" Then
                            ' Fee currencyID="GBP"
                            iStartBracket = Microsoft.VisualBasic.InStr(Name.ToUpper, "CURRENCYID=") + 11
                            sTemp = Right(Name.ToUpper, Len(Name) - iStartBracket)
                            sCurrency = Replace(sTemp, """", "")
                            With OneFee
                                .Currency = sCurrency
                                .ItemID = sItemID
                            End With
                            paeBayPostedItemFees.Add(OneFee)
                            OneFee = Nothing
                        End If
                    End If
                End If
            Next

            ' Example of "success" XML
            '                            <VerifyAddItemResponse>
            '	<Timestamp>2012-09-10T10:23:07.265Z</Timestamp>
            '	<Ack>Success</Ack>
            '	<Version>787</Version>
            '	<Build>E787_INTL_BUNDLED_15262951_R1</Build>
            '	<ItemID>0</ItemID>
            '	<Fees>
            '		<Fee>
            '			<Name>AuctionLengthFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>BoldFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>BuyItNowFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>CategoryFeaturedFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>FeaturedFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>GalleryPlusFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>FeaturedGalleryFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>FixedPriceDurationFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>GalleryFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>GiftIconFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>HighLightFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>InsertionFee</Name>
            '			<Fee currencyID="GBP">0.8</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>InternationalInsertionFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>ListingDesignerFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>ListingFee</Name>
            '			<Fee currencyID="GBP">0.8</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>PhotoDisplayFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>PhotoFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>ReserveFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>SchedulingFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>SubtitleFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>BorderFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>ProPackBundleFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>BasicUpgradePackBundleFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>ValuePackBundleFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>PrivateListingFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>ProPackPlusBundleFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '		<Fee>
            '			<Name>MotorsGermanySearchFee</Name>
            '			<Fee currencyID="GBP">0.0</Fee>
            '		</Fee>
            '	</Fees>
            '</VerifyAddItemResponse>

            Return True

        Next
    End Function

    Public Function ProcessSiteDefaultsForCategory(iCategoryID As Integer, sXML As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean
        Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
        Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

        Dim iElementCount As Integer
        Dim StringArrayElement As String
        Dim iStartBracket As Integer
        Dim iEndBracket As Integer

        Dim Name As String = ""
        Dim Value As String = ""

        Dim sReturnPolicyEnabled As String = "", bReturnPolicyEnabled As Boolean = False

        Dim OneeBayCategoryDefaults As CategoryDefaults = Nothing
        Dim bInChildNode As Boolean = False

        m_ReturnedXML = XDocument.Parse(sXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
        XMLNameTabeBay = New System.Xml.NameTable
        XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
        XMLNodeList = m_ReturnedXML.XPathSelectElements("GetCategoryFeaturesResponse/SiteDefaults")

        For Each XMLNode In XMLNodeList

            XMLTemp = XDocument.Parse(XMLNode.ToString)
            XMLChildList = XMLTemp.XPathSelectElement("SiteDefaults").DescendantNodes

            For iElementCount = 0 To XMLChildList.Count
                If iElementCount < XMLChildList.Count Then

                    StringArrayElement = XMLChildList(iElementCount).ToString()
                    ' Extract the property name
                    iStartBracket = Microsoft.VisualBasic.InStr(StringArrayElement, "<")
                    iEndBracket = Microsoft.VisualBasic.InStr(StringArrayElement, ">")

                    If iStartBracket > 0 And iEndBracket > iStartBracket Then
                        Name = StringArrayElement.Substring(iStartBracket, iEndBracket - 2)

                        If Not bInChildNode Then

                            If Left(Name, 15).ToUpper = "LISTINGDURATION" Then
                                ' We skip this element - stored elsewhere
                                Continue For
                            End If

                            If Left(Name, 24).ToUpper = "GALLERYFEATUREDDURATIONS" Then
                                ' We skip this element - it has child elements. Will pull in if needed
                                OneeBayCategoryDefaults.Type = Name
                                bInChildNode = True
                                Continue For
                            End If

                            If Left(Name, 34).ToUpper = "STOREOWNEREXTENDEDLISTINGDURATIONS" Then
                                ' We skip this element - it has child elements. Will pull in if needed
                                OneeBayCategoryDefaults.Type = Name
                                bInChildNode = True
                                Continue For
                            End If
                        End If

                        If bInChildNode And Left(Name, 8).ToUpper = "DURATION" Then
                            ' We store this in the usual manner (and .Type will be populated)
                        Else
                            bInChildNode = False
                        End If

                        ' Extract the property value
                        Value = StringArrayElement.Substring(iEndBracket)
                        iEndBracket = Microsoft.VisualBasic.InStr(Value, "<")
                        Value = m_eBayPublishingWizardFacade.ConvertFromXML(Value.Substring(0, iEndBracket - 1))

                        OneeBayCategoryDefaults.Name = Name
                        OneeBayCategoryDefaults.Value = Value

                        OneeBayCategoryDefaults.CategoryID = iCategoryID
                        AddCategoryDefaultsIfRequired(OneeBayCategoryDefaults)
                        OneeBayCategoryDefaults = Nothing
                    End If
                End If

            Next

        Next

        Return bSuccess
    End Function

    Public Function RemoveFindProductResponse(ByRef iCategoryID As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bSuccess As Boolean = True
        Dim NewList As New ArrayList
        Dim ProductResponse As New clsFindProductsResponse

        For Each ProductResponse In aFindProductsResponseProducts
            If ProductResponse.CategoryID <> iCategoryID Then
                NewList.Add(ProductResponse)
            End If
        Next
        aFindProductsResponseProducts.Clear()
        aFindProductsResponseProducts = NewList

        Return bSuccess

    End Function

    Public Function ConvertForXML(ByVal StringToConvert As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    There are 5 characters that are not allowed in an XML value and must be 
        '                    substituted by the relevant XML Entity.
        '
        '                    This function preforms these substitutions
        '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strTemp As String

        strTemp = Replace(StringToConvert, "&", "&amp;")
        strTemp = Replace(strTemp, "<", "&lt;")
        strTemp = Replace(strTemp, ">", "&gt;")
        strTemp = Replace(strTemp, """", "&quot;")
        strTemp = Replace(strTemp, "'", "&apos;")
        Return strTemp

    End Function

    Private Function CreateXMLDate(ByVal DateToUse As Date) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Formats the Date into XML 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strXMLDate As String

        strXMLDate = DateToUse.Year & "-" & Right("00" & DateToUse.Month, 2) & "-" & Right("00" & DateToUse.Day, 2)
        strXMLDate = strXMLDate & "T" & Right("00" & DateToUse.Hour, 2) & ":" & Right("00" & DateToUse.Minute, 2)
        strXMLDate = strXMLDate & ":" & Right("00" & DateToUse.Second, 2)
        Return strXMLDate

    End Function

    Private Function EBayStringCompare(ByRef InputValue As String, ByRef Compare As String, Optional ByRef ReturnIfSame As Boolean = True, _
        Optional ByRef ReturnIfDifferent As Boolean = False) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/13 | TJS/LG          | 2013.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bReturn As Boolean = ReturnIfDifferent

        If InputValue = Compare Then
            bReturn = ReturnIfSame
        End If

        Return bReturn

    End Function

    Public Sub New(ByRef p_eBayPublishingWizardFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade, _
        ByRef ActiveEBaySettings As Lerryn.Framework.ImportExport.SourceConfig.eBaySettings, ByVal UseEBaySandbox As Boolean) ' TJS 26/06/13

        If UseEBaySandbox Then
            Me.eBayDevID = Lerryn.Framework.ImportExport.Shared.EBAY_SANDBOX_LERRYN_DEVELOPER_ID
            Me.eBayAppID = Lerryn.Framework.ImportExport.Shared.EBAY_SANDBOX_LERRYN_APPLICATION_ID
            Me.eBayCertID = Lerryn.Framework.ImportExport.Shared.EBAY_SANDBOX_LERRYN_CERTIFICATE_ID

            Me.eBayXMLServerURL = Lerryn.Framework.ImportExport.Shared.EBAY_SANDBOX_SOAP_SERVER_URL
        Else
            Me.eBayDevID = Lerryn.Framework.ImportExport.Shared.EBAY_LERRYN_DEVELOPER_ID
            Me.eBayAppID = Lerryn.Framework.ImportExport.Shared.EBAY_LERRYN_APPLICATION_ID
            Me.eBayCertID = Lerryn.Framework.ImportExport.Shared.EBAY_LERRYN_CERTIFICATE_ID

            Me.eBayXMLServerURL = Lerryn.Framework.ImportExport.Shared.EBAY_SOAP_SERVER_URL
        End If
        Me.eBayAuthToken = ActiveEBaySettings.AuthToken
        Me.eBayCountry = ActiveEBaySettings.eBayCountry
        m_eBayPublishingWizardFacade = p_eBayPublishingWizardFacade ' TJS 26/06/13

    End Sub
End Class
