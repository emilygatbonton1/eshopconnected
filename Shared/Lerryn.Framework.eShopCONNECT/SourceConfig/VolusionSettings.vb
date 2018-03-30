' eShopCONNECT for Connected Business 
' Module: VolusionSettings.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'
'       © 2012 - 2014  Lerryn Business Solutions Ltd
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
' Last Updated - 11 February 2014

Imports Microsoft.VisualBasic ' TJS 11/01/14

Namespace [SourceConfig]
    Public Class VolusionSettings

        ' Volusion Site ID
        Private pSiteID As String ' TJS 19/06/09
        Public Property SiteID() As String ' TJS 19/06/09
            Get
                Return pSiteID ' TJS 19/06/09
            End Get
            Set(ByVal value As String) ' TJS 19/06/09
                pSiteID = value ' TJS 19/06/09
            End Set
        End Property

        ' Volusion Order Poll URL
        Private pOrderPollURL As String ' TJS 19/06/09
        Public Property OrderPollURL() As String ' TJS 19/06/09
            Get
                Return pOrderPollURL ' TJS 19/06/09
            End Get
            Set(ByVal value As String) ' TJS 19/06/09
                pOrderPollURL = value ' TJS 19/06/09
            End Set
        End Property

        ' Volusion Order Poll Interval Minutes
        Private pOrderPollIntervalMinutes As Integer ' TJS 19/06/09
        Public Property OrderPollIntervalMinutes() As Integer ' TJS 19/06/09
            Get
                Return pOrderPollIntervalMinutes ' TJS 19/06/09
            End Get
            Set(ByVal value As Integer) ' TJS 19/06/09
                pOrderPollIntervalMinutes = value ' TJS 19/06/09
            End Set
        End Property

        ' Volusion IS Item ID Field
        Private pISItemIDField As String ' TJS 19/06/09
        Public Property ISItemIDField() As String ' TJS 19/06/09
            Get
                Return pISItemIDField ' TJS 19/06/09
            End Get
            Set(ByVal value As String) ' TJS 19/06/09
                pISItemIDField = value ' TJS 19/06/09
            End Set
        End Property

        ' Prices Are Tax Inclusive
        Private pPricesAreTaxInclusive As Boolean ' TJS 26/10/11
        Public Property PricesAreTaxInclusive() As Boolean ' TJS 26/10/11
            Get
                Return pPricesAreTaxInclusive ' TJS 26/10/11
            End Get
            Set(ByVal value As Boolean) ' TJS 26/10/11
                pPricesAreTaxInclusive = value ' TJS 26/10/11
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

        ' Volusion Currency
        Private pCurrency As String ' TJS 19/06/09
        Public Property Currency() As String ' TJS 19/06/09
            Get
                Return pCurrency ' TJS 19/06/09
            End Get
            Set(ByVal value As String) ' TJS 19/06/09
                pCurrency = value ' TJS 19/06/09
            End Set
        End Property

        ' Enable Payment Type Translation
        Private pEnablePaymentTypeTranslation As Boolean ' TJS 11/02/14
        Public Property EnablePaymentTypeTranslation() As Boolean ' TJS 11/02/14
            Get
                Return pEnablePaymentTypeTranslation ' TJS 11/02/14
            End Get
            Set(ByVal value As Boolean) ' TJS 11/02/14
                pEnablePaymentTypeTranslation = value ' TJS 11/02/14
            End Set
        End Property

        ' Volusion Next Order Poll time
        Private pNextOrderPollTime As Date ' TJS 19/06/09
        Public Property NextOrderPollTime() As Date ' TJS 19/06/09
            Get
                Return pNextOrderPollTime ' TJS 19/06/09
            End Get
            Set(ByVal value As Date) ' TJS 19/06/09
                pNextOrderPollTime = value ' TJS 19/06/09
            End Set
        End Property

        ' Allow Shipping Last Name Blank
        Private pAllowShippingLastNameBlank As Boolean ' TJS 15/08/09
        Public Property AllowShippingLastNameBlank() As Boolean ' TJS 15/08/09
            Get
                Return pAllowShippingLastNameBlank ' TJS 15/08/09
            End Get
            Set(ByVal value As Boolean) ' TJS 15/08/09
                pAllowShippingLastNameBlank = value ' TJS 15/08/09
            End Set
        End Property

        ' Volusion Default Shipping Method ID
        Private pDefaultShippingMethodID As Integer ' TJS 11/01/14
        Public Property DefaultShippingMethodID() As Integer ' TJS 11/01/14
            Get
                Return pDefaultShippingMethodID ' TJS 11/01/14
            End Get
            Set(value As Integer)
                pDefaultShippingMethodID = value ' TJS 11/01/14
            End Set
        End Property

        ' Enable SKU Alias Lookup
        Private pEnableSKUAliasLookup As Boolean ' TJS 15/01/14
        Public Property EnableSKUAliasLookup() As Boolean ' TJS 15/01/14
            Get
                Return pEnableSKUAliasLookup ' TJS 15/01/14
            End Get
            Set(ByVal value As Boolean) ' TJS 15/01/14
                pEnableSKUAliasLookup = value ' TJS 15/01/14
            End Set
        End Property

        ' Account Disabled
        Private pAccountDisabled As Boolean ' TJS 19/08/10
        Public Property AccountDisabled() As Boolean ' TJS 19/08/10
            Get
                Return pAccountDisabled ' TJS 19/08/10
            End Get
            Set(ByVal value As Boolean) ' TJS 19/08/10
                pAccountDisabled = value ' TJS 19/08/10
            End Set
        End Property

        ' Custom SKU Processing
        Private pCustomSKUProcessing As String ' TJS 19/08/10
        Public Property CustomSKUProcessing() As String ' TJS 19/08/10
            Get
                Return pCustomSKUProcessing ' TJS 19/08/10
            End Get
            Set(ByVal value As String) ' TJS 19/08/10
                pCustomSKUProcessing = value ' TJS 19/08/10
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
            ' 11/01/14 | TJS             | 2013.4.04 | Function added
            ' 15/01/14 | TJS             | 2013.4.05 | added EnableSKUAliasLookup
            ' 11/02/14 | TJS             | 2013.4.09 | added EnablePaymentTypeTranslation
            '------------------------------------------------------------------------------------------

            pSiteID = ""
            pOrderPollURL = ""
            pOrderPollIntervalMinutes = 0
            pCurrency = ""
            pISItemIDField = ""
            pNextOrderPollTime = DateSerial(2099, 1, 1)
            pAllowShippingLastNameBlank = False
            pPricesAreTaxInclusive = False
            pTaxCodeForSourceTax = ""
            pAllowShippingLastNameBlank = False
            pDefaultShippingMethodID = 0
            pEnableSKUAliasLookup = False ' TJS 15/01/14
            pEnablePaymentTypeTranslation = False ' TJS 11/02/14
            pAccountDisabled = False
            pCustomSKUProcessing = ""

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
            ' 11/04/14 | TJS             | 2013.4.04 | Function added
            '------------------------------------------------------------------------------------------

            ClearSettings()

        End Sub

    End Class
End Namespace
