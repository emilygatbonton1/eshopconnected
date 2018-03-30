' eShopCONNECT for Connected Business 
' Module: ThreeDCartSettings.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'
'       © 2013         Lerryn Business Solutions Ltd
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
' Updated 20 November 2013

Imports Microsoft.VisualBasic

Namespace [SourceConfig]
    Public Class ThreeDCartSettings

        ' 3DCart Store ID
        Private pStoreID As String
        Public Property StoreID() As String
            Get
                Return pStoreID
            End Get
            Set(ByVal value As String)
                pStoreID = value
            End Set
        End Property

        ' 3DCart Store ID
        Private pStoreURL As String
        Public Property StoreURL() As String
            Get
                Return pStoreURL
            End Get
            Set(ByVal value As String)
                pStoreURL = value
            End Set
        End Property

        ' 3DCart User Key
        Private pUserKey As String
        Public Property UserKey() As String
            Get
                Return pUserKey
            End Get
            Set(ByVal value As String)
                pUserKey = value
            End Set
        End Property

        ' 3DCart Order Poll Interval Minutes
        Private pOrderPollIntervalMinutes As Integer
        Public Property OrderPollIntervalMinutes() As Integer
            Get
                Return pOrderPollIntervalMinutes
            End Get
            Set(ByVal value As Integer)
                pOrderPollIntervalMinutes = value
            End Set
        End Property

        ' 3DCart Currency
        Private pCurrency As String
        Public Property Currency() As String
            Get
                Return pCurrency
            End Get
            Set(ByVal value As String)
                pCurrency = value
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

        ' 3DCart Last Order Status Update time
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
            ' 20/11/13 | TJS             | 2013.4.00 | Function added
            '------------------------------------------------------------------------------------------

            pStoreID = ""
            pStoreURL = ""
            pUserKey = ""
            pEnablePaymentTypeTranslation = False
            pOrderPollIntervalMinutes = 0
            pCurrency = ""
            pISItemIDField = ""
            pNextOrderPollTime = DateSerial(2099, 1, 1)
            pAllowShippingLastNameBlank = False
            pEnableSKUAliasLookup = False
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
            ' 20/11/13 | TJS             | 2013.4.00 | Function added
            '------------------------------------------------------------------------------------------

            ClearSettings()
        End Sub

    End Class
End Namespace
