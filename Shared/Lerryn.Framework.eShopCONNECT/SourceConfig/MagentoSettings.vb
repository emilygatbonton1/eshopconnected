' eShopCONNECT for Connected Business 
' Module: MagentoSettings.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 01 May 2014

Imports Microsoft.VisualBasic ' TJS 09/07/11

Namespace [SourceConfig]
    Public Class MagentoSettings
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Copied from eShopCONNECT for Tradepoint
        ' 07/01/11 | TJS             | 2010.1.15 | Removed EnablePollForOrders
        ' 18/03/11 | TJS             | 2011.0.01 | Renamed InstanceID as IntanceID to avoid confusion when Magento Instance hosts multiple sites/damains
        ' 14/02/12 | TJS             | 2011.2.05 | Added ProductListBlockSize
        ' 20/04/12 | TJS             | 2012.1.02 | Added Magento SplitSKUSeparatorCharacters setting
        ' 10/06/12 | TJS             | 2012.1.00 | Added MAgento EnablePaymentTypeTranslation and removed redundant Currency setting
        ' 30/04/13 | TJS             | 2013.1.11 | Added InhibitInventoryUpdates and CreateCustomerForGuestCheckout
        ' 08/05/13 | TJS             | 2013.1.13 | Added IncludeChildItemsOnOrder
        ' 02/10/13 | TJS             | 2013.3.03 | Added MagentoVersion
        ' 05/10/13 | TJS             | 2013.3.03 | Added V2SoapAPIWSICompliant
        ' 13/11/13 | TJS             | 2013.3.08 | Added LerrynAPIVersion, UpdateMagentoSpecialPricing and LastDailyTasksRun
        ' 01/05/14 | TJS             | 2014.0.02 | Added CardAuthAndCaptureWithOrder, ImportAllOrdersAsSingleCustomer and OverrideMagentoPricesWith
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' Magento Instance ID
        Private pInstanceID As String ' TJS 18/03/11
        Public Property InstanceID() As String ' TJS 18/03/11
            Get
                Return pInstanceID ' TJS 18/03/11
            End Get
            Set(ByVal value As String)
                pInstanceID = value ' TJS 18/03/11
            End Set
        End Property

        ' Magento API URL
        Private pAPIURL As String
        Public Property APIURL() As String
            Get
                Return pAPIURL
            End Get
            Set(ByVal value As String)
                pAPIURL = value
            End Set
        End Property

        ' V2 Soap API WSI Compliant
        Private pV2SoapAPIWSICompliant As Boolean ' TJS 05/10/13
        Public Property V2SoapAPIWSICompliant() As Boolean ' TJS 05/10/13
            Get
                Return pV2SoapAPIWSICompliant  ' TJS 05/10/13
            End Get
            Set(value As Boolean) ' TJS 05/10/13
                pV2SoapAPIWSICompliant = value ' TJS 05/10/13
            End Set
        End Property

        ' API User
        Private pAPIUser As String
        Public Property APIUser() As String
            Get
                Return pAPIUser
            End Get
            Set(ByVal value As String)
                pAPIUser = value
            End Set
        End Property

        ' API Password
        Private pAPIPwd As String
        Public Property APIPwd() As String
            Get
                Return pAPIPwd
            End Get
            Set(ByVal value As String)
                pAPIPwd = value
            End Set
        End Property

        ' Magento Version
        Private pMagentoVersion As String ' TJS 02/10/13 TJS 13/11/13
        Public Property MagentoVersion() As String ' TJS 02/10/13 TJS 13/11/13
            Get
                Return pMagentoVersion ' TJS 02/10/13
            End Get
            Set(value As String) ' TJS 02/10/13 TJS 13/11/13
                pMagentoVersion = value ' TJS 02/10/13
            End Set
        End Property

        ' Lerryn API Version
        Private pLerrynAPIVersion As Decimal ' TJS 13/11/13
        Public Property LerrynAPIVersion() As Decimal ' TJS 13/11/13
            Get
                Return pLerrynAPIVersion ' TJS 13/11/13
            End Get
            Set(value As Decimal) ' TJS 13/11/13
                pLerrynAPIVersion = value ' TJS 13/11/13
            End Set
        End Property

        ' Magento Order Poll Interval Minutes
        Private pOrderPollIntervalMinutes As Integer
        Public Property OrderPollIntervalMinutes() As Integer
            Get
                Return pOrderPollIntervalMinutes
            End Get
            Set(ByVal value As Integer)
                pOrderPollIntervalMinutes = value
            End Set
        End Property

        ' API Supports Partial Shipments - Magento API does work properly for Partial Shipments prior to 1.4
        Private pAPISupportsPartialShipments As Boolean
        Public Property APISupportsPartialShipments() As Boolean
            Get
                Return pAPISupportsPartialShipments
            End Get
            Set(ByVal value As Boolean)
                pAPISupportsPartialShipments = value
            End Set
        End Property

        ' Card Auth and Capture with Order - means Magento invoices order when placed
        Private pCardAuthAndCaptureWithOrder As Boolean ' TJS 01/05/14
        Public Property CardAuthAndCaptureWithOrder() As Boolean ' TJS 01/05/14
            Get
                Return pCardAuthAndCaptureWithOrder ' TJS 01/05/14
            End Get
            Set(ByVal value As Boolean) ' TJS 01/05/14
                pCardAuthAndCaptureWithOrder = value ' TJS 01/05/14
            End Set
        End Property

        ' IS Item ID Field
        Private pISItemIDField As String
        Public Property ISItemIDField() As String
            Get
                Return pISItemIDField
            End Get
            Set(ByVal value As String)
                pISItemIDField = value
            End Set
        End Property

        ' Prices Are Tax Inclusive
        Private pPricesAreTaxInclusive As Boolean
        Public Property PricesAreTaxInclusive() As Boolean
            Get
                Return pPricesAreTaxInclusive
            End Get
            Set(ByVal value As Boolean)
                pPricesAreTaxInclusive = value
            End Set
        End Property

        ' Tax Code For Source Tax
        Private pTaxCodeForSourceTax As String ' TJS 26/10/11
        Public Property TaxCodeForSourceTax() As String ' TJS 26/10/11
            Get
                Return pTaxCodeForSourceTax ' TJS 26/10/11
            End Get
            Set(ByVal value As String) ' TJS 26/10/11
                pTaxCodeForSourceTax = value ' TJS 26/10/11
            End Set
        End Property

        ' Enable Payment Type Translation
        Private pEnablePaymentTypeTranslation As Boolean ' TJS 10/06/12
        Public Property EnablePaymentTypeTranslation() As Boolean ' TJS 10/06/12
            Get
                Return pEnablePaymentTypeTranslation ' TJS 10/06/12
            End Get
            Set(ByVal value As Boolean) ' TJS 10/06/12
                pEnablePaymentTypeTranslation = value ' TJS 10/06/12
            End Set
        End Property

        ' Magento Next Order Poll time
        Private pNextOrderPollTime As Date
        Public Property NextOrderPollTime() As Date
            Get
                Return pNextOrderPollTime
            End Get
            Set(ByVal value As Date)
                pNextOrderPollTime = value
            End Set
        End Property

        ' Allow Shipping Last Name Blank
        Private pAllowShippingLastNameBlank As Boolean
        Public Property AllowShippingLastNameBlank() As Boolean
            Get
                Return pAllowShippingLastNameBlank
            End Get
            Set(ByVal value As Boolean)
                pAllowShippingLastNameBlank = value
            End Set
        End Property

        ' Split SKU Separator Characters
        Private pSplitSKUSeparatorCharacters As String ' TJS 20/04/12
        Public Property SplitSKUSeparatorCharacters() As String ' TJS 20/04/12
            Get
                Return pSplitSKUSeparatorCharacters ' TJS 20/04/12
            End Get
            Set(ByVal value As String) ' TJS 20/04/12
                pSplitSKUSeparatorCharacters = value ' TJS 20/04/12
            End Set
        End Property

        ' Product List Block Size
        Private pProductListBlockSize As Integer ' TJS 14/02/12
        Public Property ProductListBlockSize() As Integer ' TJS 14/02/12
            Get
                Return pProductListBlockSize ' TJS 14/02/12
            End Get
            Set(ByVal value As Integer) ' TJS 14/02/12
                pProductListBlockSize = value ' TJS 14/02/12
            End Set
        End Property

        ' Inhibit Inventory Updates
        Private pInhibitInventoryUpdates As Boolean ' TJS 30/04/13
        Public Property InhibitInventoryUpdates() As Boolean ' TJS 30/04/13
            Get
                Return pInhibitInventoryUpdates ' TJS 30/04/13
            End Get
            Set(ByVal value As Boolean) ' TJS 30/04/13
                pInhibitInventoryUpdates = value ' TJS 30/04/13
            End Set
        End Property

        ' CreateCustomerForGuestCheckout
        Private pCreateCustomerForGuestCheckout As Boolean ' TJS 30/04/13
        Public Property CreateCustomerForGuestCheckout() As Boolean ' TJS 30/04/13
            Get
                Return pCreateCustomerForGuestCheckout ' TJS 30/04/13
            End Get
            Set(ByVal value As Boolean) ' TJS 30/04/13
                pCreateCustomerForGuestCheckout = value ' TJS 30/04/13
            End Set
        End Property

        ' IncludeChildItemsOnOrder
        Private pIncludeChildItemsOnOrder As Boolean ' TJS 08/05/13
        Public Property IncludeChildItemsOnOrder() As Boolean ' TJS 08/05/13
            Get
                Return pIncludeChildItemsOnOrder ' TJS 08/05/13
            End Get
            Set(ByVal value As Boolean) ' TJS 30/04/13
                pIncludeChildItemsOnOrder = value ' TJS 08/05/13
            End Set
        End Property

        ' Update Magento Special Pricing
        Private pUpdateMagentoSpecialPricing As Boolean ' TJS 13/11/13
        Public Property UpdateMagentoSpecialPricing() As Boolean ' TJS 13/11/13
            Get
                Return pUpdateMagentoSpecialPricing ' TJS 13/11/13
            End Get
            Set(ByVal value As Boolean) ' TJS 13/11/13
                pUpdateMagentoSpecialPricing = value ' TJS 13/11/13
            End Set
        End Property

        ' Account Disabled
        Private pAccountDisabled As Boolean
        Public Property AccountDisabled() As Boolean
            Get
                Return pAccountDisabled
            End Get
            Set(ByVal value As Boolean)
                pAccountDisabled = value
            End Set
        End Property

        ' Magento Last Order Status Update time
        Private pLastOrderStatusUpdate As Date
        Public Property LastOrderStatusUpdate() As Date
            Get
                Return pLastOrderStatusUpdate
            End Get
            Set(ByVal value As Date)
                pLastOrderStatusUpdate = value
            End Set
        End Property

        ' Magento Last Daily Tasks Run time
        Private pLastDailyTasksRun As Date ' TJS 13/11/13
        Public Property LastDailyTasksRun() As Date ' TJS 13/11/13
            Get
                Return pLastDailyTasksRun ' TJS 13/11/13
            End Get
            Set(ByVal value As Date) ' TJS 13/11/13
                pLastDailyTasksRun = value ' TJS 13/11/13
            End Set
        End Property

        ' Enable SKU Alias Lookup
        Private pEnableSKUAliasLookup As Boolean
        Public Property EnableSKUAliasLookup() As Boolean
            Get
                Return pEnableSKUAliasLookup
            End Get
            Set(ByVal value As Boolean)
                pEnableSKUAliasLookup = value
            End Set
        End Property

        ' Custom SKU Processing
        Private pCustomSKUProcessing As String
        Public Property CustomSKUProcessing() As String
            Get
                Return pCustomSKUProcessing
            End Get
            Set(ByVal value As String)
                pCustomSKUProcessing = value
            End Set
        End Property

        ' Start of code added TJS 01/05/14
        ' Import All Orders As Single Customer (config setting only visible for relevant customers)
        Private pImportAllOrdersAsSingleCustomer As String
        Public Property ImportAllOrdersAsSingleCustomer() As String
            Get
                Return pImportAllOrdersAsSingleCustomer
            End Get
            Set(value As String)
                pImportAllOrdersAsSingleCustomer = value
            End Set
        End Property

        ' Override Magento Prices With (config setting only visible for relevant customers)
        Private pOverrideMagentoPricesWith As String
        Public Property OverrideMagentoPricesWith() As String
            Get
                Return pOverrideMagentoPricesWith
            End Get
            Set(value As String)
                pOverrideMagentoPricesWith = value
            End Set
        End Property
        ' end of code added TJS 01/05/14

        Public Sub ClearSettings()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -   
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 19/08/10 | TJS             | 2010.1.00 | Copied from eShopCONNECT for Tradepoint
            ' 07/01/11 | TJS             | 2010.1.15 | Removed EnablePollForOrders
            ' 18/03/11 | TJS             | 2011.0.01 | Renamed SiteID as IntanceID to avoid confusion when Magento Instance hosts multiple sites/damains
            ' 26/10/11 | TJS             | 2011.2.00 | Modified for pTaxCodeForSourceTax
            ' 10/06/12 | TJS             | 2012.1.00 | Added MAgento EnablePaymentTypeTranslation and removed redundant Currency setting
            ' 30/04/13 | TJS             | 2013.1.11 | Added InhibitInventoryUpdates and CreateCustomerForGuestCheckout
            ' 08/05/13 | TJS             | 2013.1.13 | Added pIncludeChildItemsOnOrder
            ' 02/10/13 | TJS             | 2013.3.03 | Added MagentoVersion
            ' 05/10/13 | TJS             | 2013.3.03 | Added V2SoapAPIWSICompliant
            ' 13/11/13 | TJS             | 2013.3.08 | Added LerrynAPIVersion, UpdateMagentoSpecialPricing and LastDailyTasksRun
            ' 01/05/14 | TJS             | 2014.0.02 | Added CardAuthAndCaptureWithOrder, ImportAllOrdersAsSingleCustomer and OverrideMagentoPricesWith
            '------------------------------------------------------------------------------------------

            pInstanceID = "" ' TJS 02/12/11
            pAPIURL = ""
            pV2SoapAPIWSICompliant = False ' TJS 05/10/13
            pAPIUser = ""
            pAPIPwd = ""
            pMagentoVersion = "" ' TJS 02/10/13 TJS 13/11/13
            pLerrynAPIVersion = 0 ' TJS 13/11/13
            pOrderPollIntervalMinutes = 0
            pAPISupportsPartialShipments = False
            pISItemIDField = ""
            pPricesAreTaxInclusive = False
            pTaxCodeForSourceTax = "" ' TJS 26/10/11
            pEnablePaymentTypeTranslation = False ' TJS 10/06/12
            pNextOrderPollTime = DateSerial(2099, 1, 1)
            pAllowShippingLastNameBlank = False
            pAccountDisabled = False
            pLastOrderStatusUpdate = Date.Now
            pEnableSKUAliasLookup = False
            pCustomSKUProcessing = ""
            pImportAllOrdersAsSingleCustomer = "" ' TJS 01/05/14
            OverrideMagentoPricesWith = "" ' TJS 01/05/14
            pInhibitInventoryUpdates = False ' TJS 30/04/13
            pCreateCustomerForGuestCheckout = False ' TJS 30/04/13
            pUpdateMagentoSpecialPricing = False ' TJS 13/11/13
            pIncludeChildItemsOnOrder = False ' TJS 08/05/13
            pLastDailyTasksRun = Date.Today.AddDays(-1) ' TJS 13/11/13

        End Sub

        Sub New()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -   
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 19/08/10 | TJS             | 2010.1.00 | Copied from eShopCONNECT for Tradepoint
            '------------------------------------------------------------------------------------------

            ClearSettings()
        End Sub

    End Class
End Namespace
