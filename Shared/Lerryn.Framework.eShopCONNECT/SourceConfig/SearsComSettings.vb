' eShopCONNECT for Connected Business 
' Module: SearsComSettings.vb
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
' Last Updated - 16 January 2012

Imports Microsoft.VisualBasic

Namespace [SourceConfig]
    Public Class SearsComSettings

        ' Sears.com Site ID
        Private pSiteID As String
        Public Property SiteID() As String
            Get
                Return pSiteID
            End Get
            Set(ByVal value As String)
                pSiteID = value
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

        ' Sears.com Order Poll Interval Minutes
        Private pOrderPollIntervalMinutes As Integer
        Public Property OrderPollIntervalMinutes() As Integer
            Get
                Return pOrderPollIntervalMinutes
            End Get
            Set(ByVal value As Integer)
                pOrderPollIntervalMinutes = value
            End Set
        End Property

        ' Sears.com IS Item ID Field
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
        Private pTaxCodeForSourceTax As String
        Public Property TaxCodeForSourceTax() As String
            Get
                Return pTaxCodeForSourceTax
            End Get
            Set(ByVal value As String)
                pTaxCodeForSourceTax = value
            End Set
        End Property

        ' Sears.com Currency
        Private pCurrency As String
        Public Property Currency() As String
            Get
                Return pCurrency
            End Get
            Set(ByVal value As String)
                pCurrency = value
            End Set
        End Property

        ' Sears.com Payment Type 
        Private pPaymentType As String
        Public Property PaymentType() As String
            Get
                Return pPaymentType
            End Get
            Set(ByVal value As String)
                pPaymentType = value
            End Set
        End Property

        ' Sears Generates Invoice
        Private pSearsGeneratesInvoice As Boolean
        Public Property SearsGeneratesInvoice() As Boolean
            Get
                Return pSearsGeneratesInvoice
            End Get
            Set(ByVal value As Boolean)
                pSearsGeneratesInvoice = value
            End Set
        End Property

        ' Sears.com Next Order Poll time
        Private pNextOrderPollTime As Date
        Public Property NextOrderPollTime() As Date
            Get
                Return pNextOrderPollTime
            End Get
            Set(ByVal value As Date)
                pNextOrderPollTime = value
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

        ' Sears.com Last Order Status Update time
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
            ' 16/01/12 | TJS             | 2010.2.02 | Function added
            '------------------------------------------------------------------------------------------

            pSiteID = ""
            pAPIUser = ""
            pAPIPwd = ""
            pOrderPollIntervalMinutes = 0
            pISItemIDField = ""
            pPricesAreTaxInclusive = False
            pTaxCodeForSourceTax = ""
            pCurrency = ""
            pPaymentType = ""
            pSearsGeneratesInvoice = False
            pNextOrderPollTime = DateSerial(2099, 1, 1)
            pAccountDisabled = False
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
            ' 16/01/12 | TJS             | 2010.2.02 | Function added
            '------------------------------------------------------------------------------------------

            ClearSettings()

        End Sub

    End Class
End Namespace
