' eShopCONNECT for Connected Business
' Module: ConfigConst.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Connected Business SDK and may incorporate certain intellectual 
' property of Interprise Solutions Inc. who's
' rights are hereby recognised.
'

'-------------------------------------------------------------------
'
' Last Updated - 01 May 2014

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'   Description -    This module 
'
' Amendment Log
'------------------------------------------------------------------------------------------
' Date     | Name            | Vers.     | Description
'------------------------------------------------------------------------------------------
' 28/01/09 | TJS             | 2009.1.01 | Restructured to make separate OrderImporter product
' 17/02/09 | TJS             | 2009.1.08 | Added SOURCE_CONFIG_XML_SHOPDOTCOM_IS_ITEM_ID_FIELD, 
'                                        | SOURCE_CONFIG_XML_SHOPDOTCOM_SOURCE_ITEM_ID_FIELD,
'                                        | SOURCE_CONFIG_XML_AMAZON_IS_ITEM_ID_FIELD,
'                                        | SOURCE_CONFIG_XML_AMAZON_AMTU_BASE_PATH plus modified 
'                                        | Amazon and Shop.Com core config settings and separated 
'                                        | Order Importer and eShopCONNECT core config settings
' 08/03/09 | TJS             | 2009.1.09 | Added SOURCE_CONFIG_DEFAULT_PAYMENT_TERM_GROUP and 
'                                        | SOURCE_CONFIG_ENABLE_COUPONS
' 17/03/09 | TJS             | 2009.1.10 | Modified to add SOURCE_CONFIG_SET_DISABLE_FREIGHT_CALCULATION 
'                                        | and SOURCE_CONFIG_SHIPPING_MODULE_TO_USE settings
'                                        | added ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE
' 11/05/09 | TJS             | 2009.2.06 | renamed SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST and
'                                        | added SOURCE_CONFIG_IGNORE_VOIDED_ORDERS_AND_INVOICES
' 17/05/09 | TJS             | 2009.2.07 | Corrected URL for eShopCONNECT Merchant Updates and added 
'                                        | SOURCE_CONFIG_ACCEPT_SOURCE_SALES_TAX_CALCULATION
' 21/05/09 | TJS             | 2009.2.08 | Added Inventory Publishing constants
' 09/07/09 | TJS             | 2009.2.10 | Added SOURCE_CONFIG_CREATE_CUSTOMER_AS_COMPANY and Shop.com 
'                                        | FTP Upload URL and account details plus PricesAreTaxInclusive,
'                                        | DefaultUpliftPercent, DisableShopComPublishing and DefaultWarehouse
' 18/06/09 | TJS             | 2009.2.14 | Added FTPUploadPath and FTPUploadArchivePath
' 08/07/09 | TJS             | 2009.3.00 | Added Volusion settings and Shop.Com XMLDateFormat plus 
'                                        | PROSPECTIMPORTER_BASE_PRODUCT_CODE
' 15/08/09 | TJS             | 2009.3.03 | Added CopyBillingNameIfShippingNameBlank in Volusion settings
' 15/10/09 | TJS             | 2009.3.08 | Modified to support direct connection to Amazon
' 10/12/09 | TJS             | 2009.3.09 | Added Amazon PricesAreTaxInclusive and Channel Advisor constants
' 30/12/09 | TJS             | 2010.0.00 | Added AMazon and Channel ADvisor payment types
' 06/01/10 | TJs             | 2010.0.01 | Renamed PROSPECT_IMPORT_CONNECTOR_CODE to reflect its use in Order importer
' 13/01/10 | TJS             | 2010.0.04 | Added SOURCE_CONFIG_ALLOW_BLANK_POSTALCODE
' 19/08/10 | TJS             | 2010.1.00 | Added Magento and ASLDotNetStoreFront connector constants plus Source Code constants
' 22/09/10 | TJS             | 2010.1.01 | Corrected Core Settings to add missing elements and added 
'                                        | Channel Advisor EnablePaymentTypeTranslation
' 07/01/11 | TJs             | 2010.1.15 | REmoved all references to NotificationEmailIISConfigSource and ShippingModuleToUse
'                                        | plus EnablePollForOrders from Magento and ASPStorefront
' 18/03/11 | TJS/FA          | 2011.0.01 | Modified to correct Magento connector
' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Amazon connection using MWS instead of SOAP
' 26/10/11 | TJS             | 2011.1.xx | Added TaxCodeForSourceTax
' 02/12/11 | TJS             | 2011.2.00 | Added eBay settings and Channel Advisor Inventory Service URL
' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
' 14/02/12 | TJS             | 2011.2.05 | Added ProductListBlockSize to Magento settings
' 24/02/12 | TJS             | 2011.2.08 | Modified for ASPStorefront Extension Data Field mapping
' 20/04/12 | TJS             | 2012.1.02 | Added MAgento SplitSKUSeparatorCharacters setting
' 10/06/12 | TJS             | 2012.1.05 | Added Magento EnablePaymentTypeTranslation
' 16/06/12 | TJS             | 2012.1.07 | Modified to cater for different amazon developerids in different countries
' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings 
' 08/07/12 | TJS             | 2012.1.09 | Added UseShipToClassTemplate 
' 18/01/13 | TJS             | 2012.1.17 | Added AllocateAndReserveStock
' 13/03/13 | TJS             | 2013.1.02 | Added CB_DEMO_CUSTOMER_CODE
' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
' 30/04/13 | TJS             | 2013.1.11 | Added InhibitInventoryUpdates and CreateCustomerForGuestCheckout to Magento settings
'                                        | and corrected missing / in amazon MWSMarketplaceID closing tag
' 08/05/13 | TJS             | 2013.1.13 | Added Magento IncludeChildItemsOnOrder
' 03/07/13 | TJS/FA          | 2013.1.24 | Replaced User and Password in EBAY_CORE_CONFIG_SETTINGS with AuthToken and TokenExpires
' 30/07/13 | TJS             | 2013.1.32 | Added eBay PricesAreTaxInclusive and TaxCodeForSourceTax
' 19/09/13 | TJS             | 2013.3.00 | Added generic ImportMissingItemsAsNonStock
' 02/10/13 | TJS             | 2013.3.03 | Added MagentoVersion
' 13/11/13 | TJS             | 2013.3.08 | Added Magento V2SoapAPIWSICompliant, LerrynAPIVersion and UpdateMagentoSpecialPricing, corrected
'                                        | string constants used to indicate product name and description are taken from InventoryItem table
' 20/11/13 | TJS             | 2013.4.00 | Added 3DCart settings and Magento Web Option description links
' 04/01/14 | TJS             | 2013.4.03 | Updated Channel Advisor service URLS from v5 to v7
' 11/04/14 | TJS             | 2013.4.04 | Added Volusion DefaultShippingMethodID
' 15/04/14 | TJS             | 2013.4.05 | Added Volusion EnableSKUAliasLookup
' 11/02/14 | TJS             | 2013.4.09 | added Volusion EnablePaymentTypeTranslation
' 18/02/14 | TJS             | 2014.0.00 | Added CB14_DEMO_CUSTOMER_CODE
' 01/05/14 | TJS             | 2014.0.02 | Added CardAuthAndCaptureWithOrder, ImportAllOrdersAsSingleCustomer and OverrideMagentoPricesWith
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Namespace [Shared]
    Public Module [ConfigConst] ' TJS 28/01/09
        ' Registry Key root
        Public Const REGISTRY_KEY_ROOT As String = "Software\Lerryn\ISPlugins"

        ' ISPlugin registration URL
        Public Const IS_PLUGIN_URL As String = "https://www.LerrynSecure.com/Activation/"

        ' eShopCONNECT Merchant Browse List  URL
        Public Const BROWSE_LIST_URL As String = "https://www.LerrynSecure.com/eShopCONNECTMerchantUpdates/" ' TJS 17/05/09

        ' Connected Business Demo Company IS Customer Code
        Public Const CB_DEMO_CUSTOMER_CODE As String = "CUST-022290" ' TJS 13/03/13
        Public Const CB14_DEMO_CUSTOMER_CODE As String = "CUST-022465" ' TJS 18/02/14

        ' Product Codes
        Public Const ESHOPCONNECT_BASE_PRODUCT_CODE As String = "AX5" ' TJS 28/01/09
        Public Const ORDERIMPORTER_BASE_PRODUCT_CODE As String = "SKL" ' TJS 28/01/09
        Public Const PROSPECTIMPORTER_BASE_PRODUCT_CODE As String = "9GA" ' TJS 08/07/09
        Public Const SHOP_DOT_COM_CONNECTOR_CODE As String = "M3K"
        Public Const AMAZON_SELLER_CENTRAL_CONNECTOR_CODE As String = "1P5"
        Public Const ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE As String = "WR2" ' TJS 17/03/09
        Public Const PROSPECT_IMPORT_CONNECTOR_CODE As String = "8LM" ' TJS 08/07/09 TJS 06/01/10
        Public Const VOLUSION_CONNECTOR_CODE As String = "LG7" ' TJS 08/07/09
        Public Const CHANNEL_ADVISOR_CONNECTOR_CODE As String = "3L4" ' TJS 10/12/09
        Public Const MAGENTO_CONNECTOR_CODE As String = "6H2" ' TJS 19/08/10
        Public Const ASP_STORE_FRONT_CONNECTOR_CODE As String = "6ST" ' TJS 19/08/10
        Public Const EBAY_CONNECTOR_CODE As String = "LT8" ' TJS 02/12/11
        Public Const SEARS_DOT_COM_CONNECTOR_CODE As String = "CDQ" ' TJS 16/01/12
        Public Const AMAZON_FBA_CONNECTOR_CODE As String = "J4Z" ' TJS 05/07/12
        Public Const THREE_D_CART_CONNECTOR_CODE As String = "L34" ' TJS 20/11/13

        Public Const GENERIC_IMPORT_SOURCE_CODE As String = "GenericXMLImport" ' TJS 19/08/10
        Public Const FILE_IMPORT_SOURCE_CODE As String = "FileImport" ' TJS 19/08/10
        Public Const PROSPECT_LEAD_IMPORT_SOURCE_CODE As String = "ProspectLeadImport" ' TJS 19/08/10
        Public Const SHOP_COM_SOURCE_CODE As String = "ShopComOrder" ' TJS 19/08/10
        Public Const AMAZON_SOURCE_CODE As String = "AmazonOrder" ' TJS 19/08/10
        Public Const VOLUSION_SOURCE_CODE As String = "VolusionOrder" ' TJS 19/08/10
        Public Const CHANNEL_ADVISOR_SOURCE_CODE As String = "ChanAdvOrder" ' TJS 19/08/10
        Public Const MAGENTO_SOURCE_CODE As String = "MagentoOrder" ' TJS 19/08/10
        Public Const ASP_STORE_FRONT_SOURCE_CODE As String = "ASPStoreFrontOrder" ' TJS 19/08/10
        Public Const EBAY_SOURCE_CODE As String = "eBayOrder" ' TJS 02/12/11
        Public Const SEARS_COM_SOURCE_CODE As String = "SearsComOrder" ' TJS 16/01/12
        Public Const AMAZON_FBA_SOURCE_CODE As String = "AmazonOrder_FBA" ' TJS 05/07/12
        Public Const THREE_D_CART_SOURCE_CODE As String = "3DCartOrder" ' TJS 20/11/13

        ' core config settings (NOTE LogFile settings are not included as they only apppear in the default record)
        Public Const CORE_ORDER_IMPORTER_CONFIG_SETTINGS As String = "<eShopCONNECTConfig><General><ErrorNotificationEmailAddress>" & _
            "</ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>" & _
            "No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>Consumer</CustomerBusinessType><CustomerBusinessClass>" & _
            "Default Consumer Customer Class Template</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany>" & _
            "<EnableDeliveryMethodTranslation>No</EnableDeliveryMethodTranslation><UseShipToClassTemplate>Yes</UseShipToClassTemplate>" & _
            "<DefaultShippingMethodGroup></DefaultShippingMethodGroup><DefaultShippingMethod></DefaultShippingMethod><DefaultWarehouse>" & _
            "</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode>" & _
            "<DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>" & _
            "Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID>" & _
            "<SetDisableFreightCalculation>No</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices>" & _
            "<AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock>" & _
            "<AllocateAndReserveStock>Yes</AllocateAndReserveStock><PollGenericImportPath>No</PollGenericImportPath><GenericImportPath></GenericImportPath>" & _
            "<GenericImportProcessedPath></GenericImportProcessedPath><GenericImportErrorPath></GenericImportErrorPath></General></eShopCONNECTConfig>" ' TJS 28/01/09 TJS 17/02/09 TJS 08/03/09 TJS 17/03/09 TJS 11/05/09 TJS 17/05/09 TJS 09/07/09 TJS 09/07/09 TJS 07/01/11 TJS 08/07/12 TJS 18/01/13 TJS 19/09/13

        Public Const CORE_PROSPECT_IMPORTER_CONFIG_SETTINGS As String = "<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>" & _
            "Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>" & _
            "Consumer</CustomerBusinessType><CustomerBusinessClass>Default Consumer Customer Class Template</CustomerBusinessClass>" & _
            "<CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><UseShipToClassTemplate>Yes</UseShipToClassTemplate><DefaultShippingMethodGroup>" & _
            "</DefaultShippingMethodGroup><DefaultShippingMethod></DefaultShippingMethod><RequireSourceCustomerID>Yes</RequireSourceCustomerID>" & _
            "</General></eShopCONNECTConfig>" ' TJS 09/07/09 TJS 08/07/12

        Public Const CORE_ESHOPCONNECT_CONFIG_SETTINGS As String = "<eShopCONNECTConfig><General><ErrorNotificationEmailAddress>" & _
            "</ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>" & _
            "No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>Consumer</CustomerBusinessType><CustomerBusinessClass>" & _
            "Default Consumer Customer Class Template</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany>" & _
            "<EnableDeliveryMethodTranslation>No</EnableDeliveryMethodTranslation><UseShipToClassTemplate>Yes</UseShipToClassTemplate>" & _
            "<DefaultShippingMethodGroup></DefaultShippingMethodGroup><DefaultShippingMethod></DefaultShippingMethod><DefaultWarehouse>" & _
            "</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode>" & _
            "<DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>" & _
            "Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID>" & _
            "<SetDisableFreightCalculation>No</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices>" & _
            "<AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock>" & _
            "<AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General></eShopCONNECTConfig>" ' TJS 17/02/09 TJS 08/03/09 TJS 17/03/09 TJS 11/05/09 TJS 17/05/09 TJS 09/07/09 TJS 07/01/11 TJS 08/07/12 TJS 18/01/13 TJS 19/09/13

        Public Const AMAZON_CORE_CONFIG_SETTINGS As String = "<Amazon><AmazonSite></AmazonSite><OwnAccessKeyID></OwnAccessKeyID><OwnSecretAccessKey></OwnSecretAccessKey>" & _
            "<MerchantToken></MerchantToken><MerchantName></MerchantName><MWSMerchantID></MWSMerchantID><MWSMarketplaceID></MWSMarketplaceID><AmazonManualProcessingPath>" & _
            "</AmazonManualProcessingPath><AmazonImportProcessedPath></AmazonImportProcessedPath><AmazonImportErrorPath></AmazonImportErrorPath><ISItemIDField>ItemCode" & _
            "</ISItemIDField><DefaultUpliftPercent>0</DefaultUpliftPercent><PaymentType></PaymentType><PricesAreTaxInclusive>No</PricesAreTaxInclusive>" & _
            "<TaxCodeForSourceTax></TaxCodeForSourceTax><EnableSKUAliasLookup>No</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></Amazon>" ' TJS 17/02/09 TJS 09/07/09 TJS 15/10/09 TJS 10/12/09 TJS 19/08/10 TJS 22/09/10 TJS 09/07/11 TJS 26/10/11 TJS 16/06/12 TJS 22/03/13 TJS/FA 30/04/13

        Public Const SHOPDOTCOM_CORE_CONFIG_SETTINGS As String = "<ShopDotCom><CatalogID></CatalogID><StatusPostURL>" & _
            "https://admin-amos.shop.com/get_order_status%21251.shtml</StatusPostURL><FTPUploadServerURL></FTPUploadServerURL>" & _
            "<FTPUploadUsername></FTPUploadUsername><FTPUploadPassword></FTPUploadPassword><FTPUploadPath></FTPUploadPath>" & _
            "<FTPUploadArchivePath></FTPUploadArchivePath><SourceItemIDField>IT_SOURCECODE</SourceItemIDField><ISItemIDField>" & _
            "ItemCode</ISItemIDField><Currency></Currency><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax>" & _
            "</TaxCodeForSourceTax><DefaultUpliftPercent>0</DefaultUpliftPercent><XMLDateFormat>MM/DD/YYYY</XMLDateFormat>" & _
            "<AccountDisabled>No</AccountDisabled></ShopDotCom>" ' TJS 17/02/09 TJS 09/07/09 TJS 18/06/09 TJS 08/07/09 TJS 19/08/10 TJS 26/10/11

        Public Const VOLUSION_CORE_CONFIG_SETTINGS As String = "<Volusion><SiteID></SiteID><OrderPollURL></OrderPollURL><OrderPollIntervalMinutes>" & _
            "15</OrderPollIntervalMinutes><ISItemIDField>ItemCode</ISItemIDField><Currency></Currency><AllowShippingLastNameBlank>Yes" & _
            "</AllowShippingLastNameBlank><DefaultShippingMethodID></DefaultShippingMethodID><EnableSKUAliasLookup>No</EnableSKUAliasLookup>" & _
            "<EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><AccountDisabled>No</AccountDisabled></Volusion>" ' TJS 08/07/09 TJS 15/08/09 TJS 19/08/10 TJs 11/01/14 TJS 15/01/14 TJS 11/02/14

        Public Const CHANNEL_ADVISOR_CORE_CONFIG_SETTINGS As String = "<ChannelAdvisor><OwnDeveloperKey></OwnDeveloperKey><OwnDeveloperPassword>" & _
            "</OwnDeveloperPassword><AccountName></AccountName><AccountID></AccountID><ISItemIDField>ItemName</ISItemIDField><Currency>" & _
            "</Currency><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><PaymentType></PaymentType>" & _
            "<EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><ActionIfNoPayment>Ignore</ActionIfNoPayment><EnableSKUAliasLookup>No" & _
            "</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></ChannelAdvisor>" ' TJS 10/12/09 TJS 19/08/10 TJS 22/09/10 TJS 26/10/11

        Public Const MAGENTO_CORE_CONFIG_SETTINGS As String = "<Magento><InstanceID></InstanceID><APIURL></APIURL><V2SoapAPIWSICompliant>No</V2SoapAPIWSICompliant>" & _
            "<APIUser></APIUser><APIPwd></APIPwd><MagentoVersion></MagentoVersion><LerrynAPIVersion>0</LerrynAPIVersion><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes>" & _
            "<APISupportsPartialShipments>Yes</APISupportsPartialShipments><CardAuthAndCaptureWithOrder>No</CardAuthAndCaptureWithOrder><ISItemIDField>ItemName</ISItemIDField>" & _
            "<PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation>" & _
            "<AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><EnableSKUAliasLookup>No</EnableSKUAliasLookup><SplitSKUSeparatorCharacters></SplitSKUSeparatorCharacters>" & _
            "<ProductListBlockSize>10000</ProductListBlockSize><InhibitInventoryUpdates>No</InhibitInventoryUpdates><CreateCustomerForGuestCheckout>No</CreateCustomerForGuestCheckout>" & _
            "<IncludeChildItemsOnOrder>No</IncludeChildItemsOnOrder><UpdateMagentoSpecialPricing>No</UpdateMagentoSpecialPricing><AccountDisabled>No</AccountDisabled></Magento>" ' TJS 19/08/10 TJS 22/09/10 TJS 18/03/11 TJS 26/10/11 TJS 14/02/12 TJS 20/04/12 TJS 10/06/12 TJS 30/04/13 TJS 08/05/12 TJS 02/10/13 TJS 05/10/13 TJS 13/11/13 TJS 01/05/14

        Public Const ASP_STORE_FRONT_CORE_CONFIG_SETTINGS As String = "<ASPStoreFront><SiteID></SiteID><UseWSE3Authentication>No" & _
            "</UseWSE3Authentication><APIURL></APIURL><APIUser></APIUser><APIPwd></APIPwd><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes>" & _
            "<ISItemIDField>ItemName</ISItemIDField><Currency></Currency><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank>" & _
            "<EnableSKUAliasLookup>No</EnableSKUAliasLookup><ExtensionDataField1Mapping></ExtensionDataField1Mapping><ExtensionDataField2Mapping>" & _
            "</ExtensionDataField2Mapping><ExtensionDataField3Mapping></ExtensionDataField3Mapping><ExtensionDataField4Mapping>" & _
            "</ExtensionDataField4Mapping><ExtensionDataField5Mapping></ExtensionDataField5Mapping><AccountDisabled>No</AccountDisabled></ASPStoreFront>" ' TJS 19/08/10 TJS 24/02/12

        Public Const EBAY_CORE_CONFIG_SETTINGS As String = "<eBay><SiteID></SiteID><Country></Country><AuthToken></AuthToken><TokenExpires></TokenExpires>" & _
            "<OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><PricesAreTaxInclusive>No</PricesAreTaxInclusive>" & _
            "<TaxCodeForSourceTax></TaxCodeForSourceTax><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><PaymentType></PaymentType>" & _
            "<EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><ActionIfNoPayment>Ignore</ActionIfNoPayment><EnableSKUAliasLookup>No" & _
            "</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></eBay>" ' TJS 02/12/11 TJS /FA 03/07/13 TJS 30/07/13

        Public Const SEARSDOTCOM_CORE_CONFIG_SETTINGS As String = "<SearsDotCom><SiteID></SiteID><APIUser></APIUser><APIPwd></APIPwd>" & _
            "<OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemCode</ISItemIDField><PricesAreTaxInclusive>No" & _
            "</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><Currency></Currency><PaymentType></PaymentType>" & _
            "<SearsGeneratesInvoice>Yes</SearsGeneratesInvoice><AccountDisabled>No</AccountDisabled></SearsDotCom>" ' TJS 16/01/12

        Public Const AMAZON_FBA_CORE_CONFIG_SETTINGS As String = "<AmazonFBA><AmazonSite></AmazonSite><MerchantToken></MerchantToken><AccountDisabled>No</AccountDisabled></AmazonFBA>" ' TJS 05/07/12 TJS 22/03/13

        Public Const THREE_D_CART_CORE_CONFIG_SETTINGS As String = "<ThreeDCart><StoreID></StoreID><StoreURL></StoreURL><UserKey></UserKey>" & _
            "<OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><Currency></Currency>" & _
            "<EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank>" & _
            "<EnableSKUAliasLookup>No</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></ThreeDCart>" ' TJS 20/11/13

        ' General Config setting XML paths
        Public Const SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS As String = "eShopCONNECTConfig/General/ErrorNotificationEmailAddress"
        Public Const SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN As String = "eShopCONNECTConfig/General/SendCodeErrorEmailsToLerryn"
        Public Const SOURCE_CONFIG_SEND_SOURCE_ERROR_EMAILS_TO_LERRYN As String = "eShopCONNECTConfig/General/SendSourceErrorEmailsToLerryn"
        Public Const SOURCE_CONFIG_CUSTOMER_BUSINESS_TYPE As String = "eShopCONNECTConfig/General/CustomerBusinessType"
        Public Const SOURCE_CONFIG_CUSTOMER_BUSINESS_CLASS As String = "eShopCONNECTConfig/General/CustomerBusinessClass"
        Public Const SOURCE_CONFIG_CREATE_CUSTOMER_AS_COMPANY As String = "eShopCONNECTConfig/General/CreateCustomerAsCompany" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_SHIPPING_MODULE_TO_USE As String = "eShopCONNECTConfig/General/ShippingModuleToUse" ' TJS 17/03/09
        Public Const SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION As String = "eShopCONNECTConfig/General/EnableDeliveryMethodTranslation"
        Public Const SOURCE_CONFIG_USE_SHIPTO_CLASS_TEMPLATE As String = "eShopCONNECTConfig/General/UseShipToClassTemplate" ' TJS 08/07/12
        Public Const SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD As String = "eShopCONNECTConfig/General/DefaultShippingMethod"
        Public Const SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP As String = "eShopCONNECTConfig/General/DefaultShippingMethodGroup"
        Public Const SOURCE_CONFIG_DEFAULT_WAREHOUSE As String = "eShopCONNECTConfig/General/DefaultWarehouse" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_DEFAULT_PAYMENT_TERM_GROUP As String = "eShopCONNECTConfig/General/DefaultPaymentTermGroup" ' TJS 08/03/09
        Public Const SOURCE_CONFIG_DEFAULT_CREDIT_CARD_PAYMENT_TERM As String = "eShopCONNECTConfig/General/CreditCardPaymentTermCode" ' TJS 17/02/09
        Public Const SOURCE_CONFIG_DUE_DATE_OFFSET As String = "eShopCONNECTConfig/General/DueDateDaysInFuture"
        Public Const SOURCE_CONFIG_AUTHORISE_CARD_ON_IMPORT As String = "eShopCONNECTConfig/General/AuthoriseCreditCardOnImport"
        Public Const SOURCE_CONFIG_EXT_SYSTEM_CARD_PAYMENT_CODE As String = "eShopCONNECTConfig/General/ExternalSystemCardPaymentCode" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_ENABLE_COUPONS As String = "eShopCONNECTConfig/General/EnableCoupons" ' TJS 08/03/09
        Public Const SOURCE_CONFIG_XML_FILE_SAVE_PATH As String = "eShopCONNECTConfig/General/XMLImportFileSavePath"
        Public Const SOURCE_CONFIG_REQUIRE_SOURCE_CUSTOMER_ID As String = "eShopCONNECTConfig/General/RequireSourceCustomerID"
        Public Const SOURCE_CONFIG_ALLOW_BLANK_POSTALCODE As String = "eShopCONNECTConfig/General/AllowBlankPostalcode" ' TJS 13/01/10
        Public Const SOURCE_CONFIG_SET_DISABLE_FREIGHT_CALCULATION As String = "eShopCONNECTConfig/General/SetDisableFreightCalculation" ' TJS 17/03/09
        Public Const SOURCE_CONFIG_IGNORE_VOIDED_ORDERS_AND_INVOICES As String = "eShopCONNECTConfig/General/IgnoreVoidedOrdersAndInvoices" ' TJS 11/05/09
        Public Const SOURCE_CONFIG_ACCEPT_SOURCE_SALES_TAX_CALCULATION As String = "eShopCONNECTConfig/General/AcceptSourceSalesTaxCalculation" ' TJS 17/05/09
        Public Const SOURCE_CONFIG_POLL_GENERIC_IMPORT_PATH As String = "eShopCONNECTConfig/General/PollGenericImportPath"
        Public Const SOURCE_CONFIG_GENERIC_IMPORT_PATH As String = "eShopCONNECTConfig/General/GenericImportPath"
        Public Const SOURCE_CONFIG_GENERIC_IMPORT_PROCESSED_PATH As String = "eShopCONNECTConfig/General/GenericImportProcessedPath"
        Public Const SOURCE_CONFIG_GENERIC_IMPORT_ERROR_PATH As String = "eShopCONNECTConfig/General/GenericImportErrorPath"
        Public Const SOURCE_CONFIG_ENABLE_LOG_FILE As String = "eShopCONNECTConfig/General/EnableLogFile"
        Public Const SOURCE_CONFIG_LOG_FILE_PATH As String = "eShopCONNECTConfig/General/LogFilePath"
        Public Const SOURCE_CONFIG_ALLOCATE_AND_RESERVE_STOCK As String = "eShopCONNECTConfig/General/AllocateAndReserveStock" ' TJS 18/01/13
        Public Const SOURCE_CONFIG_IMPORT_MISSING_ITEMS_AS_NONSTOCK As String = "eShopCONNECTConfig/General/ImportMissingItemsAsNonStock" ' TJS 19/09/13

        ' Source specific Config seting XML paths
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST As String = "eShopCONNECTConfig/ShopDotCom" ' TJS 06/02/09 TJS 11/05/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_ID As String = "ShopDotCom/CatalogID"
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_STATUS_POST_URL As String = "ShopDotCom/StatusPostURL"
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_URL As String = "ShopDotCom/FTPUploadServerURL" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_USERNAME As String = "ShopDotCom/FTPUploadUsername" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_PASSWORD As String = "ShopDotCom/FTPUploadPassword" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_PATH As String = "ShopDotCom/FTPUploadPath" ' TJS 18/06/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_ARCHIVE_PATH As String = "ShopDotCom/FTPUploadArchivePath" ' TJS 18/06/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_SOURCE_ITEM_ID_FIELD As String = "ShopDotCom/SourceItemIDField" ' TJS 17/02/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_IS_ITEM_ID_FIELD As String = "ShopDotCom/ISItemIDField" ' TJS 17/02/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_CURRENCY As String = "ShopDotCom/Currency" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_PRICES_ARE_TAX_INCLUSIVE As String = "ShopDotCom/PricesAreTaxInclusive" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_TAX_CODE_FOR_SOURCE_TAX As String = "ShopDotCom/TaxCodeForSourceTax" ' TJS 26/10/11
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_DEFAULT_PRICE_UPLIFT As String = "ShopDotCom/DefaultUpliftPercent" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_DISABLE_PUBLISHING As String = "ShopDotCom/DisableShopComPublishing" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_XML_DATE_FORMAT As String = "ShopDotCom/XMLDateFormat" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_ACCOUNT_DISABLED As String = "ShopDotCom/AccountDisabled" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_SHOPDOTCOM_CUSTOM_SKU_PROCESSING As String = "ShopDotCom/CustomSKUProcessing" ' TJS 19/08/10

        Public Const SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST As String = "eShopCONNECTConfig/Amazon" ' TJS 17/02/09
        Public Const SOURCE_CONFIG_XML_AMAZON_SITE As String = "Amazon/AmazonSite" ' TJS 15/10/09
        Public Const SOURCE_CONFIG_XML_AMAZON_OWN_ACCESS_KEY_ID As String = "Amazon/OwnAccessKeyID" ' TJS 16/06/12
        Public Const SOURCE_CONFIG_XML_AMAZON_ADV_OWN_SECRET_ACCESS_KEY As String = "Amazon/OwnSecretAccessKey" ' TJS 16/06/12
        Public Const SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN As String = "Amazon/MerchantToken" ' TJS 20/03/12
        Public Const SOURCE_CONFIG_XML_AMAZON_MERCHANT_NAME As String = "Amazon/MerchantName" ' TJS 15/10/09
        Public Const SOURCE_CONFIG_XML_AMAZON_MWS_MERCHANT_ID As String = "Amazon/MWSMerchantID" ' TJS 15/10/09 TJS 09/07/11
        Public Const SOURCE_CONFIG_XML_AMAZON_MWS_MARKETPLACE_ID As String = "Amazon/MWSMarketplaceID" ' TJS 15/10/09 TJS 09/07/11
        Public Const SOURCE_CONFIG_XML_AMAZON_MANUAL_PROCESS_PATH As String = "Amazon/AmazonManualProcessingPath" ' TJS 15/10/09
        Public Const SOURCE_CONFIG_XML_AMAZON_IMPORT_PROCESSED_PATH As String = "Amazon/AmazonImportProcessedPath" ' TJS 17/02/09
        Public Const SOURCE_CONFIG_XML_AMAZON_IMPORT_ERROR_PATH As String = "Amazon/AmazonImportErrorPath" ' TJS 17/02/09
        Public Const SOURCE_CONFIG_XML_AMAZON_IS_ITEM_ID_FIELD As String = "Amazon/ISItemIDField" ' TJS 17/02/09
        Public Const SOURCE_CONFIG_XML_AMAZON_DEFAULT_PRICE_UPLIFT As String = "Amazon/DefaultUpliftPercent" ' TJS 09/07/09
        Public Const SOURCE_CONFIG_XML_AMAZON_PAYMENT_TYPE As String = "Amazon/PaymentType" ' TJS 30/12/09
        Public Const SOURCE_CONFIG_XML_AMAZON_PRICES_ARE_TAX_INCLUSIVE As String = "Amazon/PricesAreTaxInclusive" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_AMAZON_TAX_CODE_FOR_SOURCE_TAX As String = "Amazon/TaxCodeForSourceTax" ' TJS 26/10/11
        Public Const SOURCE_CONFIG_XML_AMAZON_ENABLE_SKU_ALIAS_LOOKUP As String = "Amazon/EnableSKUAliasLookup" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_AMAZON_ACCOUNT_DISABLED As String = "Amazon/AccountDisabled" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_AMAZON_CUSTOM_SKU_PROCESSING As String = "Amazon/CustomSKUProcessing" ' TJS 19/08/10

        Public Const SOURCE_CONFIG_XML_VOLUSION_LIST As String = "eShopCONNECTConfig/Volusion" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_XML_VOLUSION_SITE_ID As String = "Volusion/SiteID" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_XML_VOLUSION_ORDER_POLL_URL As String = "Volusion/OrderPollURL" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_XML_VOLUSION_ORDER_POLL_INTERVAL_MINUTES As String = "Volusion/OrderPollIntervalMinutes" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_XML_VOLUSION_IS_ITEM_ID_FIELD As String = "Volusion/ISItemIDField" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_XML_VOLUSION_PRICES_ARE_TAX_INCLUSIVE As String = "Volusion/PricesAreTaxInclusive" ' TJS 26/10/11
        Public Const SOURCE_CONFIG_XML_VOLUSION_TAX_CODE_FOR_SOURCE_TAX As String = "Volusion/TaxCodeForSourceTax" ' TJS 26/10/11
        Public Const SOURCE_CONFIG_XML_VOLUSION_CURRENCY As String = "Volusion/Currency" ' TJS 08/07/09
        Public Const SOURCE_CONFIG_XML_VOLUSION_ALLOW_SHIPPING_LAST_NAME_BLANK As String = "Volusion/AllowShippingLastNameBlank" ' TJS 15/08/09
        Public Const SOURCE_CONFIG_XML_VOLUSION_DEFAULT_SHIPPING_METHOD_ID As String = "Volusion/DefaultShippingMethodID" ' TJS 11/01/14
        Public Const SOURCE_CONFIG_XML_VOLUSION_ENABLE_SKU_ALIAS_LOOKUP As String = "Volusion/EnableSKUAliasLookup" ' TJS 15/01/14
        Public Const SOURCE_CONFIG_XML_VOLUSION_ENABLE_PAYMENT_TYPE_TRANSLATION As String = "Volusion/EnablePaymentTypeTranslation" ' TJS 11/02/14
        Public Const SOURCE_CONFIG_XML_VOLUSION_ACCOUNT_DISABLED As String = "Volusion/AccountDisabled" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_VOLUSION_CUSTOM_SKU_PROCESSING As String = "Volusion/CustomSKUProcessing" ' TJS 19/08/10

        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_LIST As String = "eShopCONNECTConfig/ChannelAdvisor" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY As String = "ChannelAdvisor/OwnDeveloperKey" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_PWD As String = "ChannelAdvisor/OwnDeveloperPassword" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME As String = "ChannelAdvisor/AccountName" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID As String = "ChannelAdvisor/AccountID" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_IS_ITEM_ID_FIELD As String = "ChannelAdvisor/ISItemIDField" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_CURRENCY As String = "ChannelAdvisor/Currency" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_PRICES_ARE_TAX_INCLUSIVE As String = "ChannelAdvisor/PricesAreTaxInclusive" ' TJS 10/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_TAX_CODE_FOR_SOURCE_TAX As String = "ChannelAdvisor/TaxCodeForSourceTax" ' TJS 26/10/11
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_PAYMENT_TYPE As String = "ChannelAdvisor/PaymentType" ' TJS 30/12/09
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_ENABLE_PAYMENT_TYPE_TRANSLATION As String = "ChannelAdvisor/EnablePaymentTypeTranslation" ' TJS 22/09/10
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_ACTION_IF_NO_PMT As String = "ChannelAdvisor/ActionIfNoPayment" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_ENABLE_SKU_ALIAS_LOOKUP As String = "ChannelAdvisor/EnableSKUAliasLookup" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_DISABLED As String = "ChannelAdvisor/AccountDisabled" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_CHANNEL_ADV_CUSTOM_SKU_PROCESSING As String = "ChannelAdvisor/CustomSKUProcessing" ' TJS 19/08/10

        Public Const SOURCE_CONFIG_XML_MAGENTO_LIST As String = "eShopCONNECTConfig/Magento" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID As String = "Magento/InstanceID" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_API_URL As String = "Magento/APIURL" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT As String = "Magento/V2SoapAPIWSICompliant" ' TJS 05/10/13
        Public Const SOURCE_CONFIG_XML_MAGENTO_API_USER As String = "Magento/APIUser" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD As String = "Magento/APIPwd" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_VERSION As String = "Magento/MagentoVersion" ' TJS 02/10/13 TJS 13/11/13
        Public Const SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION As String = "Magento/LerrynAPIVersion" ' TJS 13/11/13
        Public Const SOURCE_CONFIG_XML_MAGENTO_ORDER_POLL_INTERVAL_MINUTES As String = "Magento/OrderPollIntervalMinutes" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_API_SUPPORTS_PARTIAL_SHIP As String = "Magento/APISupportsPartialShipments" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_CARD_AUTH_CAPTURE_ON_ORDER As String = "Magento/CardAuthAndCaptureWithOrder" ' TJS 01/05/14
        Public Const SOURCE_CONFIG_XML_MAGENTO_IS_ITEM_ID_FIELD As String = "Magento/ISItemIDField" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_PRICES_ARE_TAX_INCLUSIVE As String = "Magento/PricesAreTaxInclusive" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_TAX_CODE_FOR_SOURCE_TAX As String = "Magento/TaxCodeForSourceTax" ' TJS 26/10/11
        Public Const SOURCE_CONFIG_XML_MAGENTO_ENABLE_PAYMENT_TYPE_TRANSLATION As String = "Magento/EnablePaymentTypeTranslation" ' TJS 10/06/12
        Public Const SOURCE_CONFIG_XML_MAGENTO_ALLOW_SHIPPING_LAST_NAME_BLANK As String = "Magento/AllowShippingLastNameBlank" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_ENABLE_SKU_ALIAS_LOOKUP As String = "Magento/EnableSKUAliasLookup" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_SPLIT_SKU_SEPARATOR_CHARACTERS As String = "Magento/SplitSKUSeparatorCharacters" ' TJS 20/04/12
        Public Const SOURCE_CONFIG_XML_MAGENTO_PRODUCT_LIST_BLOCK_SIZE As String = "Magento/ProductListBlockSize" ' TJS 14/02/12
        Public Const SOURCE_CONFIG_XML_MAGENTO_INHIBIT_INVENTORY_UPDATES As String = "Magento/InhibitInventoryUpdates" ' TJS 30/04/13
        Public Const SOURCE_CONFIG_XML_MAGENTO_CREATE_GUEST_CUSTOMERS As String = "Magento/CreateCustomerForGuestCheckout" ' TJS 30/04/13
        Public Const SOURCE_CONFIG_XML_MAGENTO_INCLUDE_CHILD_ITEMS_ON_ORDER As String = "Magento/IncludeChildItemsOnOrder" ' TJS 08/05/13
        Public Const SOURCE_CONFIG_XML_MAGENTO_UPDATE_SPECIAL_PRICING As String = "Magento/UpdateMagentoSpecialPricing" 'TJS 30/10/13
        Public Const SOURCE_CONFIG_XML_MAGENTO_IMPORT_ALL_ORDERS_AS_SINGLE_CUSTOMER As String = "Magento/ImportAllOrdersAsSingleCustomer" ' TJS 01/05/14 - setting only visible for relevant customers
        Public Const SOURCE_CONFIG_XML_MAGENTO_OVERRIDE_MAGENTO_PRICES As String = "Magento/OverrideMagentoPricesWith" ' TJS 01/05/14 - setting only visible for relevant customers
        Public Const SOURCE_CONFIG_XML_MAGENTO_ACCOUNT_DISABLED As String = "Magento/AccountDisabled" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_MAGENTO_CUSTOM_SKU_PROCESSING As String = "Magento/CustomSKUProcessing" ' TJS 19/08/10

        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST As String = "eShopCONNECTConfig/ASPStoreFront" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID As String = "ASPStoreFront/SiteID" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_USE_WSE3_AUTHENTICATION As String = "ASPStoreFront/UseWSE3Authentication" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_URL As String = "ASPStoreFront/APIURL" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_USER As String = "ASPStoreFront/APIUser" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_PASSWORD As String = "ASPStoreFront/APIPwd" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_ORDER_POLL_INTERVAL_MINUTES As String = "ASPStoreFront/OrderPollIntervalMinutes" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_IS_ITEM_ID_FIELD As String = "ASPStoreFront/ISItemIDField" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_CURRENCY As String = "ASPStoreFront/Currency" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_ALLOW_SHIPPING_LAST_NAME_BLANK As String = "ASPStoreFront/AllowShippingLastNameBlank" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_ENABLE_SKU_ALIAS_LOOKUP As String = "ASPStoreFront/EnableSKUAliasLookup" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_1_MAPPING As String = "ASPStoreFront/ExtensionDataField1Mapping" ' TJS 24/02/12
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_2_MAPPING As String = "ASPStoreFront/ExtensionDataField2Mapping" ' TJS 24/02/12
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_3_MAPPING As String = "ASPStoreFront/ExtensionDataField3Mapping" ' TJS 24/02/12
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_4_MAPPING As String = "ASPStoreFront/ExtensionDataField4Mapping" ' TJS 24/02/12
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_5_MAPPING As String = "ASPStoreFront/ExtensionDataField5Mapping" ' TJS 24/02/12
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_ACCOUNT_DISABLED As String = "ASPStoreFront/AccountDisabled" ' TJS 19/08/10
        Public Const SOURCE_CONFIG_XML_ASP_STORE_FRONT_CUSTOM_SKU_PROCESSING As String = "ASPStoreFront/CustomSKUProcessing" ' TJS 19/08/10

        ' start of code added TJS 02/12/11
        Public Const SOURCE_CONFIG_XML_EBAY_LIST As String = "eShopCONNECTConfig/eBay"
        Public Const SOURCE_CONFIG_XML_EBAY_SITE_ID As String = "eBay/SiteID"
        Public Const SOURCE_CONFIG_XML_EBAY_COUNTRY As String = "eBay/Country"
        Public Const SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN As String = "eBay/AuthToken"
        Public Const SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN_EXPIRES As String = "eBay/TokenExpires"
        Public Const SOURCE_CONFIG_XML_EBAY_ORDER_POLL_INTERVAL_MINUTES As String = "eBay/OrderPollIntervalMinutes"
        Public Const SOURCE_CONFIG_XML_EBAY_IS_ITEM_ID_FIELD As String = "eBay/ISItemIDField"
        Public Const SOURCE_CONFIG_XML_EBAY_PRICES_ARE_TAX_INCLUSIVE As String = "eBay/PricesAreTaxInclusive" ' TJS 30/07/13
        Public Const SOURCE_CONFIG_XML_EBAY_TAX_CODE_FOR_SOURCE_TAX As String = "eBay/TaxCodeForSourceTax" ' TJS 30/07/13
        Public Const SOURCE_CONFIG_XML_EBAY_ALLOW_SHIPPING_LAST_NAME_BLANK As String = "eBay/AllowShippingLastNameBlank"
        Public Const SOURCE_CONFIG_XML_EBAY_PAYMENT_TYPE As String = "eBay/PaymentType"
        Public Const SOURCE_CONFIG_XML_EBAY_ENABLE_PAYMENT_TYPE_TRANSLATION As String = "eBay/EnablePaymentTypeTranslation"
        Public Const SOURCE_CONFIG_XML_EBAY_ACTION_IF_NO_PMT As String = "eBay/ActionIfNoPayment"
        Public Const SOURCE_CONFIG_XML_EBAY_ENABLE_SKU_ALIAS_LOOKUP As String = "eBay/EnableSKUAliasLookup"
        Public Const SOURCE_CONFIG_XML_EBAY_ACCOUNT_DISABLED As String = "eBay/AccountDisabled"
        Public Const SOURCE_CONFIG_XML_EBAY_CUSTOM_SKU_PROCESSING As String = "eBay/CustomSKUProcessing"
        ' end of code added TJS 02/12/11

        ' start of code added TJS 16/01/12
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_LIST As String = "eShopCONNECTConfig/SearsDotCom"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_SITE_ID As String = "SearsDotCom/SiteID"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_API_USER As String = "SearsDotCom/APIUser"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_API_PASSWORD As String = "SearsDotCom/APIPwd"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_ORDER_POLL_INTERVAL_MINUTES As String = "SearsDotCom/OrderPollIntervalMinutes"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_IS_ITEM_ID_FIELD As String = "SearsDotCom/ISItemIDField"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_PRICES_ARE_TAX_INCLUSIVE As String = "SearsDotCom/PricesAreTaxInclusive"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_TAX_CODE_FOR_SOURCE_TAX As String = "SearsDotCom/TaxCodeForSourceTax"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_CURRENCY As String = "SearsDotCom/Currency"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_PAYMENT_TYPE As String = "SearsDotCom/PaymentType"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_SEARS_INVOICING As String = "SearsDotCom/SearsGeneratesInvoice"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_ACCOUNT_DISABLED As String = "SearsDotCom/AccountDisabled"
        Public Const SOURCE_CONFIG_XML_SEARSDOTCOM_CUSTOM_SKU_PROCESSING As String = "SearsDotCom/CustomSKUProcessing"
        ' end of code added TJS 16/01/12

        ' start of code added TJS 05/07/12
        Public Const SOURCE_CONFIG_XML_AMAZON_FBA_MERCHANT_LIST As String = "eShopCONNECTConfig/AmazonFBA"
        Public Const SOURCE_CONFIG_XML_AMAZON_FBA_SITE As String = "AmazonFBA/AmazonSite"
        Public Const SOURCE_CONFIG_XML_AMAZON_FBA_MERCHANT_TOKEN As String = "AmazonFBA/MerchantToken" ' TJS 22/03/13
        Public Const SOURCE_CONFIG_XML_AMAZON_FBA_ACCOUNT_DISABLED As String = "AmazonFBA/AccountDisabled"
        ' end of code added TJS 05/07/12

        ' start of code added TJS 20/11/13
        Public Const SOURCE_CONFIG_XML_3DCART_LIST As String = "eShopCONNECTConfig/ThreeDCart"
        Public Const SOURCE_CONFIG_XML_3DCART_STORE_ID As String = "ThreeDCart/StoreID"
        Public Const SOURCE_CONFIG_XML_3DCART_STORE_URL As String = "ThreeDCart/StoreURL"
        Public Const SOURCE_CONFIG_XML_3DCART_USER_KEY As String = "ThreeDCart/UserKey"
        Public Const SOURCE_CONFIG_XML_3DCART_ORDER_POLL_INTERVAL_MINUTES As String = "ThreeDCart/OrderPollIntervalMinutes"
        Public Const SOURCE_CONFIG_XML_3DCART_IS_ITEM_ID_FIELD As String = "ThreeDCart/ISItemIDField"
        Public Const SOURCE_CONFIG_XML_3DCART_CURRENCY As String = "ThreeDCart/Currency"
        Public Const SOURCE_CONFIG_XML_3DCART_ENABLE_PAYMENT_TYPE_TRANSLATION As String = "ThreeDCart/EnablePaymentTypeTranslation"
        Public Const SOURCE_CONFIG_XML_3DCART_ALLOW_SHIPPING_LAST_NAME_BLANK As String = "ThreeDCart/AllowShippingLastNameBlank"
        Public Const SOURCE_CONFIG_XML_3DCART_ENABLE_SKU_ALIAS_LOOKUP As String = "ThreeDCart/EnableSKUAliasLookup"
        Public Const SOURCE_CONFIG_XML_3DCART_ACCOUNT_DISABLED As String = "ThreeDCart/AccountDisabled"
        Public Const SOURCE_CONFIG_XML_3DCART_CUSTOM_SKU_PROCESSING As String = "ThreeDCart/CustomSKUProcessing"
        ' end of code added TJS 20/11/13

        ' Inventory publishing
        Public Const INVENTORY_USE_GENERAL_TAB_DESCRIPTION As String = "<Use standard Item Description>" ' TJS 21/05/09 TJS 13/11/13
        Public Const INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION As String = "<Use standard Item Ext Description>" ' TJS 21/05/09 TJS 13/11/13
        Public Const INVENTORY_USE_WEB_OPTION_TAB_SUMMARY As String = "<Use standard Item Web Option Summary>" ' TJS 20/11/13
        Public Const INVENTORY_USE_WEB_OPTION_TAB_DESCRIPTION As String = "<Use standard Item Web Option Description>" ' TJS 20/11/13

        ' Channel Advisor Web Services
        Public Const CHANNEL_ADVISOR_ORDER_SERVICE_URL As String = "https://api.channeladvisor.com/ChannelAdvisorAPI/v7/OrderService.asmx" ' TJS 30/12/09 FA 18/03/11 TJS 02/12/11 TJS 04/01/14
        Public Const CHANNEL_ADVISOR_SHIPPING_SERVICE_URL As String = "https://api.channeladvisor.com/ChannelAdvisorAPI/v7/ShippingService.asmx" ' TJS 30/12/09 FA 18/03/11 TJS 02/12/11 TJS 04/01/14
        Public Const CHANNEL_ADVISOR_INVENTORY_SERVICE_URL As String = "https://api.channeladvisor.com/ChannelAdvisorAPI/v7/InventoryService.asmx" ' TJS 02/12/11 TJS 04/01/14
        Public Const CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY As String = "501aef01-e043-4481-8b0f-ac8fe6f8af0c" ' TJS 30/12/09
        Public Const CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD As String = "Interprise1!" ' TJS 30/12/09 TJS 06/01/10

        ' eBay Sandbox Development Web Services
        Public Const EBAY_SANDBOX_SOAP_SERVER_URL As String = "https://api.sandbox.ebay.com/ws/api.dll" ' TJS 02/12/11
        Public Const EBAY_SANDBOX_LERRYN_DEVELOPER_ID As String = "e47fab0a-818c-4ac1-bd87-1c21edbb4cd4" ' TJS 02/12/11
        Public Const EBAY_SANDBOX_LERRYN_APPLICATION_ID As String = "Interpri-2383-4abc-82be-dea5ed028fed" ' TJS 02/12/11
        Public Const EBAY_SANDBOX_LERRYN_CERTIFICATE_ID As String = "a1b28c19-9d25-45ba-b007-e50e7de19814" ' TJS 02/12/11
        Public Const EBAY_SANDBOX_ESHOPCONNECT_RUNAME As String = "Interprise_Solu-Interpri-2383-4-tdvlp" ' TJS 02/12/11

        ' eBay Live Development Web Services 
        Public Const EBAY_SOAP_SERVER_URL As String = "https://api.ebay.com/ws/api.dll" ' TJS 02/12/11
        Public Const EBAY_LERRYN_DEVELOPER_ID As String = "e47fab0a-818c-4ac1-bd87-1c21edbb4cd4" ' TJS 02/12/11
        Public Const EBAY_LERRYN_APPLICATION_ID As String = "Interpri-2573-43f4-9f6e-c1c6bd684e42" ' TJS 02/12/11
        Public Const EBAY_LERRYN_CERTIFICATE_ID As String = "6342c3f8-d2cd-4d89-baf0-bd68c5e0afa8" ' TJS 02/12/11
        Public Const EBAY_ESHOPCONNECT_RUNAME As String = "Interprise_Solu-Interpri-2573-4-rdame" ' TJS 02/12/11

        ' Sears.com Web Services
        Public Const SEARSDOTCOM_ORDER_POLL_URL As String = "https://seller.marketplace.sears.com/SellerPortal/api/oms/purchaseorder/v3" ' TJS 16/01/12
        Public Const SEARSDOTCOM_SHIPPING_NOTIFICATION_URL As String = "https://seller.marketplace.sears.com/SellerPortal/api/oms/asn/v3" ' TJS 16/01/12
        Public Const SEARSDOTCOM_INVOICE_NOTIFICATION_URL As String = "https://seller.marketplace.sears.com/SellerPortal/api/oms/invoice/v1" ' TJS 16/01/12
        Public Const SEARSDOTCOM_ORDER_UPDATE_URL As String = "https://seller.marketplace.sears.com/SellerPortal/api/oms/orderupdate/v2" ' TJS 16/01/12

        ' 3DCart Web SErvices
        Public Const THREE_D_CART_WEB_SERVICES_URL As String = "http://api.3dcart.com/cart.asmx" ' TJS 20/11/13

    End Module
End Namespace