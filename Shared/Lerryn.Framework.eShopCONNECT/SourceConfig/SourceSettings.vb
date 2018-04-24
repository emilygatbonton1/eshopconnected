' eShopCONNECT for Connected Business 

' Module: SourceSettings.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 20 November 2013

Imports System.io
Imports System.xml
Imports System.Text
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath
Imports Microsoft.VisualBasic ' TJS 09/07/11

Namespace [SourceConfig]
    Public Class SourceSettings

        ' Source Name
        Private pSourceCode As String ' TJS 02/02/09
        Public Property SourceCode() As String ' TJS 02/02/09
            Get
                Return pSourceCode ' TJS 02/02/09
            End Get
            Set(ByVal value As String) ' TJS 02/02/09
                pSourceCode = value ' TJS 02/02/09
            End Set
        End Property

        ' Source Name
        Private pSourceName As String ' TJS 06/02/09
        Public Property SourceName() As String ' TJS 06/02/09
            Get
                Return pSourceName ' TJS 06/02/09
            End Get
            Set(ByVal value As String) ' TJS 06/02/09
                pSourceName = value ' TJS 06/02/09
            End Set
        End Property

        ' Source Input Handler
        Private pSourceInputHandler As String ' TJS 17/03/09
        Public Property SourceInputHandler() As String ' TJS 17/03/09
            Get
                Return pSourceInputHandler ' TJS 17/03/09
            End Get
            Set(ByVal value As String) ' TJS 17/03/09
                pSourceInputHandler = value ' TJS 17/03/09
            End Set
        End Property

        Private pXMLConfig As XDocument ' TJS 28/01/09
        Public Property XMLConfig() As XDocument ' TJS 28/01/09
            Get
                Return pXMLConfig ' TJS 28/01/09
            End Get
            Set(ByVal value As XDocument) ' TJS 28/01/09
                pXMLConfig = value ' TJS 28/01/09
            End Set
        End Property

        ' FTP Enabled
        Private pFTPEnabled As Boolean ' TJS 20/01/09
        Public Property FTPEnabled() As Boolean ' TJS 20/01/09
            Get
                Return pFTPEnabled ' TJS 20/01/09
            End Get
            Set(ByVal value As Boolean) ' TJS 20/01/09
                pFTPEnabled = value ' TJS 20/01/09
            End Set
        End Property

        ' Error Notification Email Address
        Private pErrorNotificationEmailAddress As String
        Public Property ErrorNotificationEmailAddress() As String
            Get
                Return pErrorNotificationEmailAddress
            End Get
            Set(ByVal value As String)
                pErrorNotificationEmailAddress = value
            End Set
        End Property

        ' start of code removed TJS 07/01/11
        ' Notification Email IIS Config Source
        'Private pNotificationEmailIISConfigSource As String
        'Public Property NotificationEmailIISConfigSource() As String
        '    Get
        '        Return pNotificationEmailIISConfigSource
        '    End Get
        '    Set(ByVal value As String)
        '        pNotificationEmailIISConfigSource = value
        '    End Set
        'End Property
        ' end of code removed TJS 07/01/11

        ' Send Code Error Emails To Lerryn
        Private pSendCodeErrorEmailsToLerryn As Boolean
        Public Property SendCodeErrorEmailsToLerryn() As Boolean
            Get
                Return pSendCodeErrorEmailsToLerryn
            End Get
            Set(ByVal value As Boolean)
                pSendCodeErrorEmailsToLerryn = value
            End Set
        End Property

        ' Send Source Error Emails To Lerryn
        Private pSendSourceErrorEmailsToLerryn As Boolean
        Public Property SendSourceErrorEmailsToLerryn() As Boolean
            Get
                Return pSendSourceErrorEmailsToLerryn
            End Get
            Set(ByVal value As Boolean)
                pSendSourceErrorEmailsToLerryn = value
            End Set
        End Property

        ' Customer Business Type
        Private pCustomerBusinessType As String
        Public Property CustomerBusinessType() As String
            Get
                Return pCustomerBusinessType
            End Get
            Set(ByVal value As String)
                pCustomerBusinessType = value
            End Set
        End Property

        ' Customer Business Class
        Private pCustomerBusinessClass As String
        Public Property CustomerBusinessClass() As String
            Get
                Return pCustomerBusinessClass
            End Get
            Set(ByVal value As String)
                pCustomerBusinessClass = value
            End Set
        End Property

        ' Create Customer As Company
        Private pCreateCustomerAsCompany As Boolean ' TJS 01/06/09
        Public Property CreateCustomerAsCompany() As Boolean ' TJS 01/06/09
            Get
                Return pCreateCustomerAsCompany ' TJS 01/06/09
            End Get
            Set(ByVal value As Boolean) ' TJS 01/06/09
                pCreateCustomerAsCompany = value ' TJS 01/06/09
            End Set
        End Property

        ' ShippingModuleToUse
        Private pShippingModuleToUse As String ' TJS 17/03/09
        Public Property ShippingModuleToUse() As String ' TJS 17/03/09
            Get
                Return pShippingModuleToUse ' TJS 17/03/09
            End Get
            Set(ByVal value As String) ' TJS 17/03/09
                pShippingModuleToUse = value ' TJS 17/03/09
            End Set
        End Property

        ' Enable Delivery Method Translation
        Private pEnableDeliveryMethodTranslation As Boolean
        Public Property EnableDeliveryMethodTranslation() As Boolean
            Get
                Return pEnableDeliveryMethodTranslation
            End Get
            Set(ByVal value As Boolean)
                pEnableDeliveryMethodTranslation = value
            End Set
        End Property

        ' Use ShipTo Class Template
        Private pUseShipToClassTemplate As Boolean ' TJS 05/07/12
        Public Property UseShipToClassTemplate() As Boolean ' TJS 05/07/12
            Get
                Return pUseShipToClassTemplate ' TJS 05/07/12
            End Get
            Set(ByVal value As Boolean) ' TJS 05/07/12
                pUseShipToClassTemplate = value ' TJS 05/07/12
            End Set
        End Property

        ' Default Shipping Method
        Private pDefaultShippingMethod As String
        Public Property DefaultShippingMethod() As String
            Get
                Return pDefaultShippingMethod
            End Get
            Set(ByVal value As String)
                pDefaultShippingMethod = value
            End Set
        End Property

        ' Default Shipping Method Group
        Private pDefaultShippingMethodGroup As String
        Public Property DefaultShippingMethodGroup() As String
            Get
                Return pDefaultShippingMethodGroup
            End Get
            Set(ByVal value As String)
                pDefaultShippingMethodGroup = value
            End Set
        End Property

        ' Credit Card Payment Term Code
        Private pCreditCardPaymentTermCode As String ' TJS 17/02/09
        Public Property CreditCardPaymentTermCode() As String ' TJS 17/02/09
            Get
                Return pCreditCardPaymentTermCode ' TJS 17/02/09
            End Get
            Set(ByVal value As String)
                pCreditCardPaymentTermCode = value ' TJS 17/02/09
            End Set
        End Property

        ' Due Date Days In Future
        Private pDueDateDaysInFuture As Integer
        Public Property DueDateDaysInFuture() As Integer
            Get
                Return pDueDateDaysInFuture
            End Get
            Set(ByVal value As Integer)
                pDueDateDaysInFuture = value
            End Set
        End Property

        ' Authorise Credit Card On Import
        Private pAuthoriseCreditCardOnImport As Boolean
        Public Property AuthoriseCreditCardOnImport() As Boolean
            Get
                Return pAuthoriseCreditCardOnImport
            End Get
            Set(ByVal value As Boolean)
                pAuthoriseCreditCardOnImport = value
            End Set
        End Property

        ' Enable Coupons
        Private pEnableCoupons As Boolean ' TJS 01/06/09
        Public Property EnableCoupons() As Boolean ' TJS 01/06/09
            Get
                Return pEnableCoupons ' TJS 01/06/09
            End Get
            Set(ByVal value As Boolean) ' TJS 01/06/09
                pEnableCoupons = value ' TJS 01/06/09
            End Set
        End Property

        ' Require Source Customer ID
        Private pRequireSourceCustomerID As Boolean
        Public Property RequireSourceCustomerID() As Boolean
            Get
                Return pRequireSourceCustomerID
            End Get
            Set(ByVal value As Boolean)
                pRequireSourceCustomerID = value
            End Set
        End Property

        ' Disable Freight Calculation
        Private pDisableFreightCalculation As Boolean ' TJS 17/03/09
        Public Property DisableFreightCalculation() As Boolean ' TJS 17/03/09
            Get
                Return pDisableFreightCalculation ' TJS 17/03/09
            End Get
            Set(ByVal value As Boolean) ' TJS 17/03/09
                pDisableFreightCalculation = value ' TJS 17/03/09
            End Set
        End Property

        ' Ignore Voided Orders And Invoices
        Private pIgnoreVoidedOrdersAndInvoices As Boolean ' TJS 01/06/09
        Public Property IgnoreVoidedOrdersAndInvoices() As Boolean ' TJS 01/06/09
            Get
                Return pIgnoreVoidedOrdersAndInvoices ' TJS 01/06/09
            End Get
            Set(ByVal value As Boolean) ' TJS 01/06/09
                pIgnoreVoidedOrdersAndInvoices = value ' TJS 01/06/09
            End Set
        End Property

        ' Accept Source Sales Tax Calculation
        Private pAcceptSourceSalesTaxCalculation As Boolean ' TJS 01/06/09
        Public Property AcceptSourceSalesTaxCalculation() As Boolean ' TJS 01/06/09
            Get
                Return pAcceptSourceSalesTaxCalculation ' TJS 01/06/09
            End Get
            Set(ByVal value As Boolean) ' TJS 01/06/09
                pAcceptSourceSalesTaxCalculation = value ' TJS 01/06/09
            End Set
        End Property

        ' Import Missing Items As NonStock
        Private pImportMissingItemsAsNonStock As Boolean ' TJS 19/09/13
        Public Property ImportMissingItemsAsNonStock() As Boolean ' TJS 19/09/13
            Get
                Return pImportMissingItemsAsNonStock ' TJS 19/09/13
            End Get
            Set(ByVal value As Boolean) ' TJS 19/09/13
                pImportMissingItemsAsNonStock = value ' TJS 19/09/13
            End Set
        End Property

        ' Allocate And Reserve Stock
        Private pAllocateAndReserveStock As Boolean ' TJS 18/01/13
        Public Property AllocateAndReserveStock() As Boolean ' TJS 18/01/13
            Get
                Return pAllocateAndReserveStock ' TJS 18/01/13
            End Get
            Set(ByVal value As Boolean) ' TJS 18/01/13
                pAllocateAndReserveStock = value ' TJS 18/01/13
            End Set
        End Property

        ' Enable Log File
        Private pEnableLogFile As Boolean ' TJS 20/01/09
        Public Property EnableLogFile() As Boolean ' TJS 20/01/09
            Get
                Return pEnableLogFile ' TJS 20/01/09
            End Get
            Set(ByVal value As Boolean) ' TJS 20/01/09
                pEnableLogFile = value ' TJS 20/01/09
            End Set
        End Property

        ' Log File Path
        Private pLogFilePath As String
        Public Property LogFilePath() As String
            Get
                Return pLogFilePath
            End Get
            Set(ByVal value As String)
                If value <> "" Then
                    If Right(value, 1) = "\" Then
                        pLogFilePath = value
                    Else
                        pLogFilePath = value & "\"
                    End If
                Else
                    pLogFilePath = value
                End If
            End Set
        End Property

        ' Enable Poll Import Path
        Private pEnablePollGenericImportPath As Boolean ' TJS 20/01/09
        Public Property EnablePollGenericImportPath() As Boolean ' TJS 20/01/09
            Get
                Return pEnablePollGenericImportPath ' TJS 20/01/09
            End Get
            Set(ByVal value As Boolean) ' TJS 20/01/09
                pEnablePollGenericImportPath = value ' TJS 20/01/09
            End Set
        End Property

        ' File input Path
        Private pGenericImportPath As String
        Public Property GenericImportPath() As String
            Get
                Return pGenericImportPath
            End Get
            Set(ByVal value As String)
                If value <> "" Then
                    If Right(value, 1) = "\" Then
                        pGenericImportPath = value
                    Else
                        pGenericImportPath = value & "\"
                    End If
                Else
                    pGenericImportPath = value
                End If

            End Set
        End Property

        Private pGenericImportProcessedPath As String
        Public Property GenericImportProcessedPath() As String
            Get
                Return pGenericImportProcessedPath
            End Get
            Set(ByVal value As String)
                If value <> "" Then
                    If Right(value, 1) = "\" Then
                        pGenericImportProcessedPath = value
                    Else
                        pGenericImportProcessedPath = value & "\"
                    End If
                Else
                    pGenericImportProcessedPath = value
                End If

            End Set
        End Property

        Private pGenericImportErrorPath As String
        Public Property GenericImportErrorPath() As String
            Get
                Return pGenericImportErrorPath
            End Get
            Set(ByVal value As String)
                If value <> "" Then
                    If Right(value, 1) = "\" Then
                        pGenericImportErrorPath = value
                    Else
                        pGenericImportErrorPath = value & "\"
                    End If
                Else
                    pGenericImportErrorPath = value
                End If

            End Set
        End Property

        Private pXMLImportFileSavePath As String ' TJS 22/11/09
        Public Property XMLImportFileSavePath() As String ' TJS 22/11/09
            Get
                Return pXMLImportFileSavePath ' TJS 22/11/09
            End Get
            Set(ByVal value As String) ' TJS 22/11/09
                If value <> "" Then ' TJS 22/11/09
                    If Right(value, 1) = "\" Then ' TJS 22/11/09
                        pXMLImportFileSavePath = value ' TJS 22/11/09
                    Else
                        pXMLImportFileSavePath = value & "\" ' TJS 22/11/09
                    End If
                Else
                    pXMLImportFileSavePath = value ' TJS 22/11/09
                End If

            End Set
        End Property

        ' ShopCom Settings
        Private pShopComSettings As ShopComSettings() ' TJS 20/01/09
        Public ReadOnly Property ShopComSettings(ByVal Index As Integer) As ShopComSettings ' TJS 20/01/09
            Get
                Return pShopComSettings(Index) ' TJS 20/01/09
            End Get
        End Property

        Private pShopComSettingCount As Integer ' TJS 20/01/09
        Public ReadOnly Property ShopComSettingCount() As Integer ' TJS 20/01/09 TJS 02/02/09
            Get
                Return pShopComSettingCount ' TJS 20/01/09 TJS 02/02/09
            End Get
        End Property

        ' Amazon Settings
        Private pAmazonSettings As AmazonSettings() ' TJS 20/01/09
        Public ReadOnly Property AmazonSettings(ByVal Index As Integer) As AmazonSettings ' TJS 20/01/09
            Get
                Return pAmazonSettings(Index) ' TJS 20/01/09
            End Get
        End Property

        ' Volusion Settings
        Private pVolusionSettings As VolusionSettings() ' TJS 19/06/09
        Public ReadOnly Property VolusionSettings(ByVal Index As Integer) As VolusionSettings ' TJS 19/06/09
            Get
                Return pVolusionSettings(Index) ' TJS 19/06/09
            End Get
        End Property

        ' Channel Advisor Settings
        Private pChannelAdvSettings As ChannelAdvisorSettings() ' TJS 10/12/09
        Public ReadOnly Property ChannelAdvSettings(ByVal Index As Integer) As ChannelAdvisorSettings ' TJS 10/12/09
            Get
                Return pChannelAdvSettings(Index) ' TJS 10/12/09
            End Get
        End Property

        ' Magento Settings
        Private pMagentoSettings As MagentoSettings() ' TJS 19/08/10
        Public ReadOnly Property MagentoSettings(ByVal Index As Integer) As MagentoSettings ' TJS 19/08/10
            Get
                Return pMagentoSettings(Index) ' TJS 19/08/10
            End Get
        End Property

        ' ASPStoreFront Settings
        Private pASPStoreFrontSettings As ASPStoreFrontSettings() ' TJS 19/08/10
        Public ReadOnly Property ASPStoreFrontSettings(ByVal Index As Integer) As ASPStoreFrontSettings ' TJS 19/08/10
            Get
                Return pASPStoreFrontSettings(Index) ' TJS 19/08/10
            End Get
        End Property

        ' eBay Settings
        Private peBaySettings As eBaySettings() ' TJS 02/12/11
        Public ReadOnly Property eBaySettings(ByVal Index As Integer) As eBaySettings ' TJS 02/12/11
            Get
                Return peBaySettings(Index) ' TJS 02/12/11
            End Get
        End Property

        ' Sears.com Settings
        Private pSearsComSettings As SearsComSettings() ' TJS 16/01/12
        Public ReadOnly Property SearsComSettings(ByVal Index As Integer) As SearsComSettings ' TJS 16/01/12
            Get
                Return pSearsComSettings(Index) ' TJS 16/01/12
            End Get
        End Property

        ' 3DCart Settings
        Private p3DCartSettings As ThreeDCartSettings() ' TJS 20/11/13
        Public ReadOnly Property ThreeDCartSettings(ByVal Index As Integer) As ThreeDCartSettings ' TJS 20/11/13
            Get
                Return p3DCartSettings(Index) ' TJS 20/11/13
            End Get
        End Property

        Private pAmazonSettingCount As Integer ' TJS 16/02/09
        Public ReadOnly Property AmazonSettingCount() As Integer ' TJS 16/02/09
            Get
                Return pAmazonSettingCount ' TJS 16/02/09
            End Get
        End Property

        Private pVolusionSettingCount As Integer ' TJS 19/06/09
        Public ReadOnly Property VolusionSettingCount() As Integer ' TJS 19/06/09
            Get
                Return pVolusionSettingCount ' TJS 19/06/09
            End Get
        End Property

        Private pChannelAdvSettingCount As Integer ' TJS 10/12/09
        Public ReadOnly Property ChannelAdvSettingCount() As Integer ' TJS 10/12/09
            Get
                Return pChannelAdvSettingCount ' TJS 10/12/09
            End Get
        End Property

        Private pMagentoSettingCount As Integer ' TJS 19/08/10
        Public ReadOnly Property MagentoSettingCount() As Integer ' TJS 19/08/10
            Get
                Return pMagentoSettingCount ' TJS 19/08/10
            End Get
        End Property

        Private pASPStoreFrontSettingCount As Integer ' TJS 19/08/10
        Public ReadOnly Property ASPStoreFrontSettingCount() As Integer ' TJS 19/08/10
            Get
                Return pASPStoreFrontSettingCount ' TJS 19/08/10
            End Get
        End Property

        Private peBaySettingCount As Integer ' TJS 02/12/11
        Public ReadOnly Property eBaySettingCount() As Integer ' TJS 02/12/11
            Get
                Return peBaySettingCount ' TJS 02/12/11
            End Get
        End Property

        Private pSearsComSettingCount As Integer ' TJS 16/01/12
        Public ReadOnly Property SearsComSettingCount() As Integer ' TJS 16/01/12
            Get
                Return pSearsComSettingCount ' TJS 16/01/12
            End Get
        End Property

        Private p3DCartSettingCount As Integer ' TJS 20/11/13
        Public ReadOnly Property ThreeDCartSettingCount() As Integer ' TJS 20/11/13
            Get
                Return p3DCartSettingCount ' TJS 20/11/13
            End Get
        End Property

        Public Function AddShopComSettings(ByRef ShopSet As ShopComSettings) As Integer ' TJS 20/01/09

            pShopComSettingCount = pShopComSettingCount + 1 ' TJS 20/01/09
            ReDim Preserve pShopComSettings(pShopComSettingCount) ' TJS 20/01/09
            pShopComSettings(pShopComSettingCount - 1) = ShopSet ' TJS 20/01/09
            Return pShopComSettingCount - 1 ' TJS 20/01/09

        End Function

        Public Function AddAmazonSettings(ByRef AmazonSet As AmazonSettings) As Integer ' TJS 16/02/09

            pAmazonSettingCount = pAmazonSettingCount + 1 ' TJS 16/02/09
            ReDim Preserve pAmazonSettings(pAmazonSettingCount) ' TJS 16/02/09
            pAmazonSettings(pAmazonSettingCount - 1) = AmazonSet ' TJS 16/02/09
            Return pAmazonSettingCount - 1 ' TJS 16/02/09

        End Function

        Public Function AddVolusionSettings(ByRef VolusionSet As VolusionSettings) As Integer ' TJS 19/06/09

            pVolusionSettingCount = pVolusionSettingCount + 1 ' TJS 19/06/09
            ReDim Preserve pVolusionSettings(pVolusionSettingCount) ' TJS 19/06/09
            pVolusionSettings(pVolusionSettingCount - 1) = VolusionSet ' TJS 19/06/09
            Return pVolusionSettingCount - 1 ' TJS 19/06/09

        End Function

        Public Function AddChannelAdvSettings(ByRef ChannelAdvSet As ChannelAdvisorSettings) As Integer ' TJS 10/12/09

            pChannelAdvSettingCount = pChannelAdvSettingCount + 1 ' TJS 10/12/09
            ReDim Preserve pChannelAdvSettings(pChannelAdvSettingCount) ' TJS 10/12/09
            pChannelAdvSettings(pChannelAdvSettingCount - 1) = ChannelAdvSet ' TJS 10/12/09
            Return pChannelAdvSettingCount - 1 ' TJS 10/12/09

        End Function

        Public Function AddMagentoSettings(ByRef MagentoSet As MagentoSettings) As Integer ' TJS 19/08/10

            pMagentoSettingCount = pMagentoSettingCount + 1 ' TJS 19/08/10
            ReDim Preserve pMagentoSettings(pMagentoSettingCount) ' TJS 19/08/10
            pMagentoSettings(pMagentoSettingCount - 1) = MagentoSet ' TJS 19/08/10
            Return pMagentoSettingCount - 1 ' TJS 19/08/10

        End Function

        Public Function AddASPStoreFrontSettings(ByRef ASPStoreFrontSet As ASPStoreFrontSettings) As Integer ' TJS 19/08/10

            pASPStoreFrontSettingCount = pASPStoreFrontSettingCount + 1 ' TJS 19/08/10
            ReDim Preserve pASPStoreFrontSettings(pASPStoreFrontSettingCount) ' TJS 19/08/10
            pASPStoreFrontSettings(pASPStoreFrontSettingCount - 1) = ASPStoreFrontSet ' TJS 19/08/10
            Return pASPStoreFrontSettingCount - 1 ' TJS 19/08/10

        End Function

        Public Function AddeBaySettings(ByRef eBaySet As eBaySettings) As Integer ' TJS 02/12/11

            peBaySettingCount = peBaySettingCount + 1 ' TJS 02/12/11
            ReDim Preserve peBaySettings(peBaySettingCount) ' TJS 02/12/11
            peBaySettings(peBaySettingCount - 1) = eBaySet ' TJS 02/12/11
            Return peBaySettingCount - 1 ' TJS 02/12/11

        End Function

        Public Function AddSearsComSettings(ByRef SearsComSet As SearsComSettings) As Integer ' TJS 16/01/12

            pSearsComSettingCount = pSearsComSettingCount + 1 ' TJS 16/01/12
            ReDim Preserve pSearsComSettings(pSearsComSettingCount) ' TJS 16/01/12
            pSearsComSettings(pSearsComSettingCount - 1) = SearsComSet ' TJS 16/01/12
            Return pSearsComSettingCount - 1 ' TJS 16/01/12

        End Function

        Public Function Add3DCartSettings(ByRef ThreeDCartSet As ThreeDCartSettings) As Integer ' TJS 20/11/13

            p3DCartSettingCount = p3DCartSettingCount + 1 ' TJS 20/11/13
            ReDim Preserve p3DCartSettings(p3DCartSettingCount) ' TJS 20/11/13
            p3DCartSettings(p3DCartSettingCount - 1) = ThreeDCartSet ' TJS 20/11/13
            Return p3DCartSettingCount - 1 ' TJS 20/11/13

        End Function

        Public Sub ClearSettings() ' TJS 27/04/09
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 28/04/09 | TJS             | 2009.2.05 | Moved code from New() to cater for reloading settings
            ' 19/08/10 | TJS             | 2010.1.00 | Added Magento and ASLDotNetStoreFront connector settings
            ' 02/12/11 | TJS             | 2010.2.00 | Added eBay connector settings
            ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
            ' 19/09/13 | TJS             | 2013.3.00 | Added generic ImportMissingItemsAsNonStock
            ' 20/11/13 | TJS             | 2013.4.00 | Added 3DCart connector settings
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            pSourceCode = "" ' TJS 02/02/09
            pSourceName = "" ' TJS 06/02/09
            pSourceInputHandler = "" ' TJS 17/03/09
            pFTPEnabled = False ' TJS 17/03/09
            'pNotificationEmailIISConfigSource = "" ' TJS 07/01/11
            pErrorNotificationEmailAddress = ""
            pSendCodeErrorEmailsToLerryn = False
            pSendSourceErrorEmailsToLerryn = False
            pCustomerBusinessType = ""
            pCustomerBusinessClass = ""
            pShippingModuleToUse = "" ' TJS 17/03/09
            pEnableDeliveryMethodTranslation = False
            pDefaultShippingMethod = ""
            pDefaultShippingMethodGroup = ""
            pCreditCardPaymentTermCode = "" ' TJS 17/02/09
            pDueDateDaysInFuture = 0
            pAuthoriseCreditCardOnImport = False
            pRequireSourceCustomerID = False
            pDisableFreightCalculation = False ' TJS 17/03/09
            pAllocateAndReserveStock = False ' TJS 18/01/13
            pImportMissingItemsAsNonStock = False ' TJS 19/09/13
            pEnableLogFile = False ' TJS 20/01/09
            pLogFilePath = ""
            pEnablePollGenericImportPath = False ' TJS 20/01/09
            pGenericImportPath = ""
            pGenericImportProcessedPath = ""
            pGenericImportErrorPath = ""
            pXMLImportFileSavePath = "" ' TJS 22/11/09
            pShopComSettingCount = 0 ' TJS 20/01/09
            ReDim pShopComSettings(0) ' TJS 20/01/09
            pAmazonSettingCount = 0 ' TJS 17/03/09
            ReDim pAmazonSettings(0) ' TJS 17/03/09
            pVolusionSettingCount = 0 ' TJS 19/06/09
            ReDim pVolusionSettings(0) ' TJS 19/06/09
            pChannelAdvSettingCount = 0 ' TJS 10/12/09
            ReDim pChannelAdvSettings(0) ' TJS 10/12/09
            pMagentoSettingCount = 0 ' TJS 19/08/10
            ReDim pMagentoSettings(0) ' TJS 19/08/10
            pASPStoreFrontSettingCount = 0 ' TJS 19/08/10
            ReDim pASPStoreFrontSettings(0) ' TJS 19/08/10
            peBaySettingCount = 0 ' TJS 02/12/11
            ReDim peBaySettings(0) ' TJS 02/12/11
            pSearsComSettingCount = 0 ' TJS 16/01/12
            ReDim pSearsComSettings(0) ' TJS 16/01/12
            p3DCartSettingCount = 0 ' TJS 20/11/13
            ReDim p3DCartSettings(0) ' TJS 20/11/13

        End Sub

        Sub New()
            ClearSettings() ' TJS 27/04/09
        End Sub

    End Class
End Namespace
