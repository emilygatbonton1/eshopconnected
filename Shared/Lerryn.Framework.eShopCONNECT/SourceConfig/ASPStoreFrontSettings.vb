' eShopCONNECT for Connected Business 
' Module: ASPStoreFrontSettings.vb
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
' Updated 24 February 2012

Imports Microsoft.VisualBasic ' TJS 09/07/11

Namespace [SourceConfig]
    Public Class ASPStoreFrontSettings
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ' 07/01/11 | TJS             | 2010.1.15 | Removed EnablePollForOrders
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' ASPDotNetStoreFront Site ID
        Private pSiteID As String
        Public Property SiteID() As String
            Get
                Return pSiteID
            End Get
            Set(ByVal value As String)
                pSiteID = value
            End Set
        End Property

        ' Use WSE3 Authentication
        Private pUseWSE3Authentication As Boolean
        Public Property UseWSE3Authentication() As Boolean
            Get
                Return pUseWSE3Authentication
            End Get
            Set(ByVal value As Boolean)
                pUseWSE3Authentication = value
            End Set
        End Property

        ' ASPDotNetStoreFront API URL
        Private pAPIURL As String
        Public Property APIURL() As String
            Get
                Return pAPIURL
            End Get
            Set(ByVal value As String)
                pAPIURL = value
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

        ' ASPDotNetStoreFront Order Poll Interval Minutes
        Private pOrderPollIntervalMinutes As Integer
        Public Property OrderPollIntervalMinutes() As Integer
            Get
                Return pOrderPollIntervalMinutes
            End Get
            Set(ByVal value As Integer)
                pOrderPollIntervalMinutes = value
            End Set
        End Property

        ' ASPDotNetStoreFront Currency
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

        ' start of code added TJS 24/02/12
        ' ASPStorefront Extension Data Field 1 Mapping
        Private pExtensionDataField1Mapping As String
        Public Property ExtensionDataField1Mapping() As String
            Get
                Return pExtensionDataField1Mapping
            End Get
            Set(ByVal value As String)
                pExtensionDataField1Mapping = value
            End Set
        End Property

        ' ASPStorefront Extension Data Field 2 Mapping
        Private pExtensionDataField2Mapping As String
        Public Property ExtensionDataField2Mapping() As String
            Get
                Return pExtensionDataField2Mapping
            End Get
            Set(ByVal value As String)
                pExtensionDataField2Mapping = value
            End Set
        End Property

        ' ASPStorefront Extension Data Field 3 Mapping
        Private pExtensionDataField3Mapping As String
        Public Property ExtensionDataField3Mapping() As String
            Get
                Return pExtensionDataField3Mapping
            End Get
            Set(ByVal value As String)
                pExtensionDataField3Mapping = value
            End Set
        End Property

        ' ASPStorefront Extension Data Field 4 Mapping
        Private pExtensionDataField4Mapping As String
        Public Property ExtensionDataField4Mapping() As String
            Get
                Return pExtensionDataField4Mapping
            End Get
            Set(ByVal value As String)
                pExtensionDataField4Mapping = value
            End Set
        End Property

        ' ASPStorefront Extension Data Field 5 Mapping
        Private pExtensionDataField5Mapping As String
        Public Property ExtensionDataField5Mapping() As String
            Get
                Return pExtensionDataField5Mapping
            End Get
            Set(ByVal value As String)
                pExtensionDataField5Mapping = value
            End Set
        End Property
        ' end of code added TJS 24/02/12

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
            ' 07/01/11 | TJS             | 2010.1.15 | Removed EnablePollForOrders
            ' 24/02/12 | TJS             | 2011.2.08 | Modified to hold ASPStorefront Extension Data Field mapping
            '------------------------------------------------------------------------------------------

            pSiteID = ""
            pUseWSE3Authentication = False
            pAPIURL = ""
            pAPIUser = ""
            pAPIPwd = ""
            pOrderPollIntervalMinutes = 0
            pCurrency = ""
            pISItemIDField = ""
            pNextOrderPollTime = DateSerial(2099, 1, 1)
            pAllowShippingLastNameBlank = False
            pAccountDisabled = False
            pCustomSKUProcessing = ""
            pExtensionDataField1Mapping = "" ' TJS 24/02/12
            pExtensionDataField2Mapping = "" ' TJS 24/02/12
            pExtensionDataField3Mapping = "" ' TJS 24/02/12
            pExtensionDataField4Mapping = "" ' TJS 24/02/12
            pExtensionDataField5Mapping = "" ' TJS 24/02/12

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
