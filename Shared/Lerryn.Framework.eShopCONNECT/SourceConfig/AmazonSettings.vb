' eShopCONNECT for Connected Business 
' Module: AmazonSettings.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 22 March 2013

Imports Microsoft.VisualBasic

Namespace [SourceConfig]
    Public Class AmazonSettings

        ' Amazon Site
        Private pAmazonSite As String ' TJS 15/10/09
        Public Property AmazonSite() As String ' TJS 15/10/09
            Get
                Return pAmazonSite ' TJS 15/10/09
            End Get
            Set(ByVal value As String) ' TJS 15/10/09
                pAmazonSite = value ' TJS 15/10/09
            End Set
        End Property

        ' Start of code added TJS 05/07/12
        ' Own Access Key ID
        Private pAccessKeyID As String
        Public Property OwnAccessKeyID() As String
            Get
                Return pAccessKeyID
            End Get
            Set(ByVal value As String)
                pAccessKeyID = value
            End Set
        End Property

        ' Own Secret Access Key
        Private pOwnSecretAccessKey As String
        Public Property OwnSecretAccessKey() As String
            Get
                Return pOwnSecretAccessKey
            End Get
            Set(ByVal value As String)
                pOwnSecretAccessKey = value
            End Set
        End Property
        ' End of code added TJS 05/07/12

        ' Amazon Merchant Token (was Merchant ID)
        Private pMerchantToken As String ' TJS 22/03/13
        Public Property MerchantToken() As String ' TJS 22/03/13
            Get
                Return pMerchantToken ' TJS 22/03/13
            End Get
            Set(ByVal value As String)
                pMerchantToken = value ' TJS 22/03/13
            End Set
        End Property

        ' Amazon Merchant Name
        Private pMerchantName As String ' TJS 15/10/09
        Public Property MerchantName() As String ' TJS 15/10/09
            Get
                Return pMerchantName ' TJS 15/10/09
            End Get
            Set(ByVal value As String) ' TJS 15/10/09
                pMerchantName = value ' TJS 15/10/09
            End Set
        End Property

        ' Amazon MWS Merchant ID
        Private pMWSMerchantID As String ' TJS 15/10/09 TJS 09/07/11
        Public Property MWSMerchantID() As String ' TJS 15/10/09 TJS 09/07/11
            Get
                Return pMWSMerchantID ' TJS 15/10/09 TJS 09/07/11
            End Get
            Set(ByVal value As String) ' TJS 15/10/09 TJS 09/07/11
                pMWSMerchantID = value ' TJS 15/10/09 TJS 09/07/11
            End Set
        End Property

        ' Amazon MWS Marketplace ID
        Private pMWSMarketplaceID As String ' TJS 15/10/09 TJS 09/07/11
        Public Property MWSMarketplaceID() As String ' TJS 15/10/09 TJS 09/07/11
            Get
                Return pMWSMarketplaceID ' TJS 15/10/09 TJS 09/07/11
            End Get
            Set(ByVal value As String) ' TJS 15/10/09 TJS 09/07/11
                pMWSMarketplaceID = value ' TJS 15/10/09 TJS 09/07/11
            End Set
        End Property

        ' Amazon Manual Processing Path
        Private pManualProcessingPath As String ' TJS 15/10/09
        Public Property ManualProcessingPath() As String ' TJS 15/10/09
            Get
                Return pManualProcessingPath ' TJS 15/10/09
            End Get
            Set(ByVal value As String) ' TJS 15/10/09
                pManualProcessingPath = value ' TJS 15/10/09
            End Set
        End Property

        ' Amazon Import Processed Path
        Private pImportProcessedPath As String
        Public Property ImportProcessedPath() As String
            Get
                Return pImportProcessedPath
            End Get
            Set(ByVal value As String)
                pImportProcessedPath = value
            End Set
        End Property

        ' Amazon Import Error Path
        Private pImportErrorPath As String
        Public Property ImportErrorPath() As String
            Get
                Return pImportErrorPath
            End Get
            Set(ByVal value As String)
                pImportErrorPath = value
            End Set
        End Property

        ' Amazon Item ID Field
        Private pISItemIDField As String
        Public Property ISItemIDField() As String
            Get
                Return pISItemIDField
            End Get
            Set(ByVal value As String)
                pISItemIDField = value
            End Set
        End Property

        ' Amazon Payment Type Field
        Private pPaymentType As String ' TJS 17/12/09
        Public Property PaymentType() As String ' TJS 17/12/09
            Get
                Return pPaymentType ' TJS 17/12/09
            End Get
            Set(ByVal value As String)
                pPaymentType = value ' TJS 17/12/09
            End Set
        End Property

        ' Prices Are Tax Inclusive
        Private pPricesAreTaxInclusive As Boolean ' TJS 24/11/09
        Public Property PricesAreTaxInclusive() As Boolean ' TJS 24/11/09
            Get
                Return pPricesAreTaxInclusive ' TJS 24/11/09
            End Get
            Set(ByVal value As Boolean) ' TJS 24/11/09
                pPricesAreTaxInclusive = value ' TJS 24/11/09
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

        ' Default Markup Percent
        Private pDefaultUpliftPercent As Decimal ' TJS 03/06/09
        Public Property DefaultUpliftPercent() As Decimal ' TJS 03/06/09
            Get
                Return pDefaultUpliftPercent ' TJS 03/06/09
            End Get
            Set(ByVal value As Decimal) ' TJS 03/06/09
                pDefaultUpliftPercent = value ' TJS 03/06/09
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

        ' Amazon Next Connection time
        Private pNextConnectionTime As Date ' TJS 15/10/09
        Public Property NextConnectionTime() As Date ' TJS 15/10/09
            Get
                Return pNextConnectionTime ' TJS 15/10/09
            End Get
            Set(ByVal value As Date) ' TJS 15/10/09
                pNextConnectionTime = value ' TJS 15/10/09
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
            ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Amazon connection using MWS instead of SOAP
            ' 26/10/11 | TJS             | 2011.2.00 | Modified for pTaxCodeForSourceTax
            ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
            '------------------------------------------------------------------------------------------

            pAmazonSite = ""
            pMerchantToken = "" ' TJS 22/03/13
            pMerchantName = ""
            pMWSMerchantID = "" ' TJS 09/07/11
            pMWSMarketplaceID = "" ' TJS 09/07/11
            pManualProcessingPath = ""
            pImportProcessedPath = ""
            pImportErrorPath = ""
            pISItemIDField = ""
            pPaymentType = ""
            pPricesAreTaxInclusive = False
            pTaxCodeForSourceTax = "" ' TJS 26/10/11
            pDefaultUpliftPercent = 0
            pNextConnectionTime = DateSerial(2099, 1, 1)
            pAccountDisabled = False
            pEnableSKUAliasLookup = False
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
            ' 19/08/10 | TJS             | 2010.1.00 | Function added
            '------------------------------------------------------------------------------------------

            ClearSettings()
        End Sub

    End Class
End Namespace
