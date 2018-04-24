' eShopCONNECT for Connected Business 
' Module: eBaySettings.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 30 July 2013

Imports Microsoft.VisualBasic

Namespace [SourceConfig]
    Public Class eBaySettings

        ' eBay Site ID
        Private pSiteID As String
        Public Property SiteID() As String
            Get
                Return pSiteID
            End Get
            Set(ByVal value As String)
                pSiteID = value
            End Set
        End Property

        ' eBay Country ID
        Private peBayCountry As Integer
        Public Property eBayCountry() As Integer
            Get
                Return peBayCountry
            End Get
            Set(ByVal value As Integer)
                peBayCountry = value
            End Set
        End Property

        ' eBay Auth Token
        Private pAuthToken As String
        Public Property AuthToken() As String
            Get
                Return pAuthToken
            End Get
            Set(ByVal value As String)
                pAuthToken = value
            End Set
        End Property

        ' eBay Auth Token Expires
        Private pTokenExpires As Date
        Public Property TokenExpires() As Date
            Get
                Return pTokenExpires
            End Get
            Set(ByVal value As Date)
                pTokenExpires = value
            End Set
        End Property

        ' eBay Order Poll Interval Minutes
        Private pOrderPollIntervalMinutes As Integer
        Public Property OrderPollIntervalMinutes() As Integer
            Get
                Return pOrderPollIntervalMinutes
            End Get
            Set(ByVal value As Integer)
                pOrderPollIntervalMinutes = value
            End Set
        End Property

        ' ebay Payment Type Field
        Private pPaymentType As String
        Public Property PaymentType() As String
            Get
                Return pPaymentType
            End Get
            Set(ByVal value As String)
                pPaymentType = value
            End Set
        End Property

        ' Enable Payment Type Translation
        Private pEnablePaymentTypeTranslation As Boolean
        Public Property EnablePaymentTypeTranslation() As Boolean
            Get
                Return pEnablePaymentTypeTranslation
            End Get
            Set(ByVal value As Boolean)
                pEnablePaymentTypeTranslation = value
            End Set
        End Property

        ' Prices Are Tax Inclusive
        Private pPricesAreTaxInclusive As Boolean ' TJS 30/07/13
        Public Property PricesAreTaxInclusive() As Boolean ' TJS 30/07/13
            Get
                Return pPricesAreTaxInclusive ' TJS 30/07/13
            End Get
            Set(ByVal value As Boolean) ' TJS 30/07/13
                pPricesAreTaxInclusive = value ' TJS 30/07/13
            End Set
        End Property

        ' Tax Code For Source Tax
        Private pTaxCodeForSourceTax As String ' TJS 30/07/13
        Public Property TaxCodeForSourceTax() As String ' TJS 30/07/13
            Get
                Return pTaxCodeForSourceTax ' TJS 30/07/13
            End Get
            Set(ByVal value As String) ' TJS 30/07/13
                pTaxCodeForSourceTax = value ' TJS 30/07/13
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

        ' Next Order Poll time
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

        ' Action If No Payment
        Private pActionIfNoPayment As String
        Public Property ActionIfNoPayment() As String
            Get
                Return pActionIfNoPayment
            End Get
            Set(ByVal value As String)
                pActionIfNoPayment = value
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

        ' eBay Last Order Status Update time
        Private pLastOrderStatusUpdate As Date
        Public Property LastOrderStatusUpdate() As Date
            Get
                Return pLastOrderStatusUpdate
            End Get
            Set(ByVal value As Date)
                pLastOrderStatusUpdate = value
            End Set
        End Property

        Public Sub ClearSettings()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -   
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 02/12/11 | TJS             | 2010.2.00 | Function added
            ' 30/07/13 | TJS             | 2013.1.32 | Added PricesAreTaxInclusive and TaxCodeForSourceTax
            '------------------------------------------------------------------------------------------

            pSiteID = ""
            peBayCountry = -1
            pAuthToken = ""
            pTokenExpires = Date.Now.AddSeconds(-1)
            pOrderPollIntervalMinutes = 0
            pPaymentType = ""
            pPricesAreTaxInclusive = False ' TJS 30/07/13
            pTaxCodeForSourceTax = "" ' TJS 30/07/13
            pEnablePaymentTypeTranslation = False
            pISItemIDField = ""
            pNextOrderPollTime = DateSerial(2099, 1, 1)
            pAllowShippingLastNameBlank = False
            pAccountDisabled = False
            pActionIfNoPayment = ""
            pEnableSKUAliasLookup = False
            pCustomSKUProcessing = ""
            pLastOrderStatusUpdate = Date.Now

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
            ' 02/12/11 | TJS             | 2010.2.00 | Function added
            '------------------------------------------------------------------------------------------

            ClearSettings()

        End Sub

    End Class
End Namespace
