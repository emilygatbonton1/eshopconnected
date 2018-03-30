
' eShopCONNECT for Connected Business
' Module: ChannelAdvisorSettings.vb
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
' Updated 23 October 2011

Imports Microsoft.VisualBasic ' TJS 09/07/11

Namespace [SourceConfig]
    Public Class ChannelAdvisorSettings

        ' Own Developer Key
        Private pOwnDeveloperKey As String
        Public Property OwnDeveloperKey() As String
            Get
                Return pOwnDeveloperKey
            End Get
            Set(ByVal value As String)
                pOwnDeveloperKey = value
            End Set
        End Property

        ' Own Developer Password
        Private pOwnDeveloperPwd As String
        Public Property OwnDeveloperPwd() As String
            Get
                Return pOwnDeveloperPwd
            End Get
            Set(ByVal value As String)
                pOwnDeveloperPwd = value
            End Set
        End Property

        ' Account Name
        Private pAccountName As String
        Public Property AccountName() As String
            Get
                Return pAccountName
            End Get
            Set(ByVal value As String)
                pAccountName = value
            End Set
        End Property

        ' Account ID
        Private pAccountID As String
        Public Property AccountID() As String
            Get
                Return pAccountID
            End Get
            Set(ByVal value As String)
                pAccountID = value
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

        ' Channel Advisor Currency Field
        Private pCurrency As String
        Public Property Currency() As String
            Get
                Return pCurrency
            End Get
            Set(ByVal value As String)
                pCurrency = value
            End Set
        End Property

        ' Channel Advisor Payment Type Field
        Private pPaymentType As String ' TJS 17/12/09
        Public Property PaymentType() As String ' TJS 17/12/09
            Get
                Return pPaymentType ' TJS 17/12/09
            End Get
            Set(ByVal value As String)
                pPaymentType = value ' TJS 17/12/09
            End Set
        End Property

        ' Enable Payment Type Translation
        Private pEnablePaymentTypeTranslation As Boolean ' TJS 22/09/10
        Public Property EnablePaymentTypeTranslation() As Boolean ' TJS 22/09/10
            Get
                Return pEnablePaymentTypeTranslation ' TJS 22/09/10
            End Get
            Set(ByVal value As Boolean) ' TJS 22/09/10
                pEnablePaymentTypeTranslation = value ' TJS 22/09/10
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

        ' Channel Advisor Next Connection time
        Private pNextConnectionTime As Date
        Public Property NextConnectionTime() As Date
            Get
                Return pNextConnectionTime
            End Get
            Set(ByVal value As Date)
                pNextConnectionTime = value
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

        ' Action If No Payment
        Private pActionIfNoPayment As String ' TJS 19/08/10
        Public Property ActionIfNoPayment() As String ' TJS 19/08/10
            Get
                Return pActionIfNoPayment ' TJS 19/08/10
            End Get
            Set(ByVal value As String) ' TJS 19/08/10
                pActionIfNoPayment = value ' TJS 19/08/10
            End Set
        End Property

        ' Enable SKU Alias Lookup
        Private pEnableSKUAliasLookup As Boolean ' TJS 19/08/10
        Public Property EnableSKUAliasLookup() As Boolean ' TJS 19/08/10
            Get
                Return pEnableSKUAliasLookup ' TJS 19/08/10
            End Get
            Set(ByVal value As Boolean) ' TJS 19/08/10
                pEnableSKUAliasLookup = value ' TJS 19/08/10
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

        ' Channel Advisor Last Order Status Update time
        Private pLastOrderStatusUpdate As Date ' TJS 19/08/10
        Public Property LastOrderStatusUpdate() As Date ' TJS 19/08/10
            Get
                Return pLastOrderStatusUpdate ' TJS 19/08/10
            End Get
            Set(ByVal value As Date) ' TJS 19/08/10
                pLastOrderStatusUpdate = value ' TJS 19/08/10
            End Set
        End Property

        ' Channel Advisor First GetOrderList Call time
        Private pFirstGetOrderListCall As DateTime? ' TJS 19/08/10
        Public Property FirstGetOrderListCall() As DateTime? ' TJS 19/08/10
            Get
                Return pFirstGetOrderListCall ' TJS 19/08/10
            End Get
            Set(ByVal value As DateTime?) ' TJS 19/08/10
                pFirstGetOrderListCall = value ' TJS 19/08/10
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
            ' 19/08/10 | TJS             | 2010.1.00 | Function added
            ' 26/10/11 | TJS             | 2011.2.00 | Modified for pTaxCodeForSourceTax
            '------------------------------------------------------------------------------------------

            pOwnDeveloperKey = ""
            pOwnDeveloperPwd = ""
            pAccountName = ""
            pAccountID = ""
            pISItemIDField = ""
            pCurrency = ""
            pPaymentType = ""
            pPricesAreTaxInclusive = False
            pTaxCodeForSourceTax = "" ' TJS 26/10/11
            pNextConnectionTime = DateSerial(2099, 1, 1)
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
            ' 19/08/10 | TJS             | 2010.1.00 | Function added
            '------------------------------------------------------------------------------------------

            ClearSettings()

        End Sub

    End Class
End Namespace
