' eShopCONNECT for Connected Business 
' Module: ShopComSettings.vb
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
' Last Updated - 02 December 2011

Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath

Namespace [SourceConfig]
    Public Class ShopComSettings

        ' Shop.com Merchant ID
        Private pCatalogID As String
        Public Property CatalogID() As String
            Get
                Return pCatalogID
            End Get
            Set(ByVal value As String)
                pCatalogID = value
            End Set
        End Property

        ' Shop.com Status Post URL
        Private pStatusPostURL As String ' TJS 06/02/09
        Public Property StatusPostURL() As String ' TJS 06/02/09
            Get
                Return pStatusPostURL ' TJS 06/02/09
            End Get
            Set(ByVal value As String) ' TJS 06/02/09
                pStatusPostURL = value ' TJS 06/02/09
            End Set
        End Property

        ' Shop.com FTP Upload URL
        Private pFTPUploadURL As String ' TJS 04/06/09
        Public Property FTPUploadURL() As String ' TJS 04/06/09
            Get
                Return pFTPUploadURL ' TJS 04/06/09
            End Get
            Set(ByVal value As String) ' TJS 04/06/09
                pFTPUploadURL = value ' TJS 04/06/09
            End Set
        End Property

        ' Shop.com FTP Upload Username
        Private pFTPUploadUserName As String ' TJS 04/06/09
        Public Property FTPUploadUserName() As String ' TJS 04/06/09
            Get
                Return pFTPUploadUserName ' TJS 04/06/09
            End Get
            Set(ByVal value As String) ' TJS 04/06/09
                pFTPUploadUserName = value ' TJS 04/06/09
            End Set
        End Property

        ' Shop.com FTP Upload Password
        Private pFTPUploadPassword As String ' TJS 04/06/09
        Public Property FTPUploadPassword() As String ' TJS 04/06/09
            Get
                Return pFTPUploadPassword ' TJS 04/06/09
            End Get
            Set(ByVal value As String) ' TJS 04/06/09
                pFTPUploadPassword = value ' TJS 04/06/09
            End Set
        End Property

        ' Shop.com FTP Upload Path
        Private pFTPUploadPath As String ' TJS 18/06/09
        Public Property FTPUploadPath() As String ' TJS 18/06/09
            Get
                Return pFTPUploadPath ' TJS 18/06/09
            End Get
            Set(ByVal value As String) ' TJS 18/06/09
                pFTPUploadPath = value ' TJS 18/06/09
            End Set
        End Property

        ' Shop.com FTP Upload Archive Path
        Private pFTPUploadArchivePath As String ' TJS 18/06/09
        Public Property FTPUploadArchivePath() As String ' TJS 18/06/09
            Get
                Return pFTPUploadArchivePath ' TJS 18/06/09
            End Get
            Set(ByVal value As String) ' TJS 18/06/09
                pFTPUploadArchivePath = value ' TJS 18/06/09
            End Set
        End Property

        ' Shop.com Source Item ID Field
        Private pSourceItemIDField As String ' TJS 22/02/09
        Public Property SourceItemIDField() As String ' TJS 22/02/09
            Get
                Return pSourceItemIDField ' TJS 22/02/09
            End Get
            Set(ByVal value As String) ' TJS 22/02/09
                pSourceItemIDField = value ' TJS 22/02/09
            End Set
        End Property

        ' Shop.com IS Item ID Field
        Private pISItemIDField As String ' TJS 16/02/09
        Public Property ISItemIDField() As String ' TJS 16/02/09
            Get
                Return pISItemIDField ' TJS 16/02/09
            End Get
            Set(ByVal value As String) ' TJS 16/02/09
                pISItemIDField = value ' TJS 16/02/09
            End Set
        End Property

        ' Shop.com Currency Field
        Private pCurrency As String ' TJS 04/06/09
        Public Property Currency() As String ' TJS 04/06/09
            Get
                Return pCurrency ' TJS 04/06/09
            End Get
            Set(ByVal value As String) ' TJS 04/06/09
                pCurrency = value ' TJS 04/06/09
            End Set
        End Property

        ' Prices Are Tax Inclusive
        Private pPricesAreTaxInclusive As Boolean ' TJS 04/06/09
        Public Property PricesAreTaxInclusive() As Boolean ' TJS 04/06/09
            Get
                Return pPricesAreTaxInclusive ' TJS 04/06/09
            End Get
            Set(ByVal value As Boolean) ' TJS 04/06/09
                pPricesAreTaxInclusive = value ' TJS 04/06/09
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
        Private pDefaultUpliftPercent As Decimal ' TJS 04/06/09
        Public Property DefaultUpliftPercent() As Decimal ' TJS 04/06/09
            Get
                Return pDefaultUpliftPercent ' TJS 04/06/09
            End Get
            Set(ByVal value As Decimal) ' TJS 04/06/09
                pDefaultUpliftPercent = value ' TJS 04/06/09
            End Set
        End Property

        ' Disable Shop.Com Publishing
        Private pDisableShopComPublishing As Boolean ' TJS 04/06/09
        Public Property DisableShopComPublishing() As Boolean ' TJS 04/06/09
            Get
                Return pDisableShopComPublishing ' TJS 04/06/09
            End Get
            Set(ByVal value As Boolean) ' TJS 04/06/09
                pDisableShopComPublishing = value ' TJS 04/06/09
            End Set
        End Property

        Private pXMLConfig As XDocument ' TJS 18/06/09
        Public Property XMLConfig() As XDocument ' TJS 18/06/09
            Get
                Return pXMLConfig ' TJS 18/06/09
            End Get
            Set(ByVal value As XDocument) ' TJS 18/06/09
                pXMLConfig = value ' TJS 18/06/09
            End Set
        End Property

        ' Shop.Com dates can be UK or US format
        Private pUKNotUSDateFormat As Boolean ' TJS 20/06/09
        Public Property UKNotUSDateFormat() As Boolean ' TJS 20/06/09
            Get
                Return pUKNotUSDateFormat ' TJS 20/06/09
            End Get
            Set(ByVal value As Boolean) ' TJS 20/06/09
                pUKNotUSDateFormat = value ' TJS 20/06/09
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

        Public Function ValidateShipMethod(ByVal sShipper As String) As Boolean
            Select Case sShipper

                Case "DHL @HOME Residential"
                    Return True
                Case "DHL 2nd Day"
                    Return True
                Case "DHL Domestic Express"
                    Return True
                Case "DHL Ground"
                    Return True
                Case "DHL Same day"
                    Return True
                Case "FedEx 2Day"
                    Return True
                Case "FedEx 3Day"
                    Return True
                Case "FedEx Express Saver"
                    Return True
                Case "FedEx First Overnight"
                    Return True
                Case "FedEx Freight Services"
                    Return True
                Case "FedEx Ground"
                    Return True
                Case "FedEx Home Delivery"
                    Return True
                Case "FedEx International Economy"
                    Return True
                Case "FedEx International Next Flight"
                    Return True
                Case "FedEx International Priority"
                    Return True
                Case "FedEx Priority Overnight"
                    Return True
                Case "UPS 2nd Day Air"
                    Return True
                Case "UPS 3 Day Select"
                    Return True
                Case "UPS Ground"
                    Return True
                Case "UPS Next Day Air"
                    Return True
                Case "UPS Standard To Canada"
                    Return True
                Case "UPS Worldwide Express"
                    Return True
                Case "USPS Express Mail"
                    Return True
                Case "USPS First-Class Mail"
                    Return True
                Case "USPS Global Direct Canada"
                    Return True
                Case "USPS Global Express Mail"
                    Return True
                Case "USPS Global Priority Mail"
                    Return True
                Case "USPS International Priority Mail"
                    Return True
                Case "USPS Media Mail"
                    Return True
                Case "USPS Parcel Post"
                    Return True
                Case "USPS Parcel Select"
                    Return True
                Case "USPS Priority Mail"
                    Return True
                Case "USPS Standard Mail"
                    Return True
                Case ""
                    Return True
                Case Else
                    Return False

            End Select

        End Function

    End Class
End Namespace
