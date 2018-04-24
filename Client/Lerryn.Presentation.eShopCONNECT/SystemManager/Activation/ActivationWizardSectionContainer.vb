' eShopCONNECT for Connected Business
' Module: ActivationWizardSectionContainer.vb
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

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Enum
Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " ActivationWizardSectionContainer "
Public Class ActivationWizardSectionContainer

#Region " Variables "
    Private Structure ActivationCost
        Public PricePerMonth As Decimal
        Public PriceFor1Year As Decimal
        Public PriceFor2Years As Decimal
        Public PriceFor3Years As Decimal
        Public TurnoverBased As Boolean ' TJS 09/08/13
        Public PercentageOfSales As Decimal  ' TJS 09/08/13
        Public MonthlyMinimum As Decimal ' TJS 09/08/13
    End Structure
    Private m_ActivationWizardSectionContainerFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private bIgnoreWizardPageChangeEvent As Boolean
    Private dteNextInvoiceDue As Date
    Private dteEndFreeTrial As Date
    Private XMLActivationCost As XDocument ' TJS 02/12/11
    Private decExistingActivationCost As ActivationCost
    Private decNewActivationCost As ActivationCost
    Private strCurrencySymbol As String
    Private bAutoRenewal As Boolean
    Private strExistingPaymentPeriod As String
    Private bNearExpiry As Boolean
    Private bConfigUpdateRequired As Boolean
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.ActivationWizardSectionContainerGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_ActivationWizardSectionContainerFacade
        End Get
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.m_ActivationWizardSectionContainerFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.ActivationWizardSectionContainerGateway, _
            New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
        bIgnoreWizardPageChangeEvent = False

    End Sub
#End Region

#Region " InitialiseControls "
    Public Sub InitialiseControls()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Form re-written for Connected Business out-of-box version
        ' 21/03/11 | TJS             | 2011.0.02 | Modified for payment event handlers
        ' 05/04/11 | TJS             | 2011.0.08 | Modified to cater for IS 4.8 build using conditional compile
        ' 13/04/11 | TJS             | 2011.0.10 | Modified to cater for separate pricing page
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 
        '                                        | and to initialise PostalCode from Company details. ALso modified 
        '                                        | to show existing activation details if no connection to licence server 
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 26/01/12 | TJS             | 2010.2.05 | Modified to show next Invoice Date 
        ' 17/02/12 | TJS             | 2010.2.07 | Modified to show previously activated connectors if base activation has expired
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 18/01/13 | TJS             | 2012.1.17 | Modified to cater for PayPal invoicing
        ' 29/01/13 | TJS             | 2013.0.00 | Modified for CB 13 having different label names
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to detect the demo company and amend activation messages accordingly
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to cater for Turnover based pricing 
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected initialistion of Turnover based pricing when not yet activated
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart
        ' 18/02/14 | TJS             | 2014.0.00 | Modified for CB14_DEMO_CUSTOMER_CODE
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim rowAccountDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.ActivationAccountDetailsRow
        Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim encrypteddata As Byte(), salt As Byte(), vector As Byte()
        Dim strWelcomeMessage As String, strDecrypted() As String, strDateParts() As String, strTemp As String
        Dim iErrorCode As Integer = 0, bCustAcctCreated As Boolean, bConnectorsActivated As Boolean
        Dim bNextInvoiceSet As Boolean ' TJS 26/01/12
        Dim strActivationCostMessage As String ' Mark kee 22/04/15

        Try ' TJS 16/01/12
            ' check for any updated activations from the auto renew service function
            If Me.m_ActivationWizardSectionContainerFacade.GetActivationCode(iErrorCode) <> "" Then
                Me.m_ActivationWizardSectionContainerFacade.ReCheckActivation()
            End If

            Me.m_ActivationWizardSectionContainerFacade.GetSystemLicenceID(True)

            bNextInvoiceSet = False ' TJS 26/01/12
            lblPluginsURL.Text = PLUGINS_WEBSITE_URL.Replace("http://", "") ' TJS 29/01/13
            lblWelcomeFurtherDetails.Text = lblWelcomeFurtherDetails.Text.Replace("IS_PRODUCT_NAME", IS_PRODUCT_NAME) ' TJS 29/01/13
            strExistingPaymentPeriod = "1Y"
            bNearExpiry = False
            bConfigUpdateRequired = False
            If Me.m_ActivationWizardSectionContainerFacade.IsActivated Or Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                ' set default end of free trial as yesterday i.e. expired
                dteEndFreeTrial = Date.Today.AddDays(-1)
                dteNextInvoiceDue = dteEndFreeTrial.AddMonths(1).AddDays(-1) ' TJS 18/01/13
                crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
                rowLicence = Me.ActivationWizardSectionContainerGateway.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(Me.m_ActivationWizardSectionContainerFacade.LatestActivationCode)
                If rowLicence IsNot Nothing Then
                    If Not rowLicence.IsData_DEV000221Null And Not rowLicence.IsDataSalt_DEV000221Null And Not rowLicence.IsDataIV_DEV000221Null Then
                        encrypteddata = System.Convert.FromBase64String(rowLicence.Data_DEV000221)
                        salt = System.Convert.FromBase64String(rowLicence.DataSalt_DEV000221)
                        vector = System.Convert.FromBase64String(rowLicence.DataIV_DEV000221)

                        If Not encrypteddata Is Nothing AndAlso Not salt Is Nothing AndAlso Not vector Is Nothing Then
                            If encrypteddata.Length > 0 AndAlso salt.Length > 0 AndAlso vector.Length > 0 Then
                                strDecrypted = Split(crypto.Decrypt(encrypteddata, salt, vector), ":")
                                If strDecrypted.Length > 0 Then
                                    If strDecrypted(0) <> "" Then
                                        strDateParts = Split(strDecrypted(0), "-")
                                        dteEndFreeTrial = DateSerial(CInt(strDateParts(0)), CInt(strDateParts(1)), CInt(strDateParts(2)))
                                    End If
                                    If strDecrypted(1) <> "" Then
                                        strDateParts = Split(strDecrypted(1), "-")
                                        dteNextInvoiceDue = DateSerial(CInt(strDateParts(0)), CInt(strDateParts(1)), CInt(strDateParts(2)))
                                        bNextInvoiceSet = True ' TJS 26/01/12
                                    End If
                                End If
                                ' element 2 was payment failed - no longer used
                                ' element 3 is Last Notification date
                                If strDecrypted.Length >= 5 Then
                                    strExistingPaymentPeriod = strDecrypted(4)
                                End If
                                If strDecrypted.Length >= 6 Then
                                    If strDecrypted(5) = "T" Then
                                        bAutoRenewal = True
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                dteEndFreeTrial = Date.Today.AddMonths(1).AddDays(-1)
                dteNextInvoiceDue = dteEndFreeTrial.AddDays(-1) ' TJS 18/01/13
                bNextInvoiceSet = True ' TJS 26/01/12
            End If

            ' do we have a system record ?
            If Me.ActivationWizardSectionContainerGateway.System_DEV000221.Count > 0 Then
                ' yes, do we have a Customer Code ?
                bCustAcctCreated = False
                rowAccountDetails = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails.NewActivationAccountDetailsRow
                If Not Me.ActivationWizardSectionContainerGateway.System_DEV000221(0).IsCustCode_DEV000221Null Then
                    If Me.ActivationWizardSectionContainerGateway.System_DEV000221(0).CustCode_DEV000221 <> "" Then
                        bCustAcctCreated = True
                    End If
                End If
                If Not bCustAcctCreated Then
                    ' no, copy company details for user confirmation of billing details
                    rowAccountDetails.BillingCompanyName = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).CompanyName
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsAddressNull Then
                        rowAccountDetails.BillingAddress = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).Address
                    End If
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsCityNull Then
                        rowAccountDetails.BillingCity = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).City
                    End If
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsCountyNull Then
                        rowAccountDetails.BillingCounty = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).County
                    End If
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsStateNull Then
                        rowAccountDetails.BillingState = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).State
                    End If
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsPostalCodeNull Then ' TJS 02/12/11
                        rowAccountDetails.BillingPostalCode = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).PostalCode ' TJS 02/12/11
                    End If
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsCountryNull Then
                        rowAccountDetails.BillingCountry = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).Country
                    End If
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsPhoneNull Then
                        rowAccountDetails.BillingPhone = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).Phone
                    End If
                    If Not Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).IsPhoneExtensionNull Then
                        rowAccountDetails.BillingPhoneExtension = Me.ActivationWizardSectionContainerGateway.SystemCompanyInformation(0).PhoneExtension
                    End If
                End If
                ' start of code added TJS 02/12/11
                Select Case strExistingPaymentPeriod
                    Case "M"
                        rowAccountDetails.PayMonthly = True
                        rowAccountDetails.PayForXYears = False
                        rowAccountDetails.NoOfYears = 1
                        Me.ImageComboBoxEditYears.Enabled = False

                    Case "3Y"
                        rowAccountDetails.PayMonthly = False
                        rowAccountDetails.PayForXYears = True
                        rowAccountDetails.NoOfYears = 3
                        Me.ImageComboBoxEditYears.Enabled = True

                    Case "2Y"
                        rowAccountDetails.PayMonthly = False
                        rowAccountDetails.PayForXYears = True
                        rowAccountDetails.NoOfYears = 2
                        Me.ImageComboBoxEditYears.Enabled = True

                    Case Else
                        ' must be 1 year
                        rowAccountDetails.PayMonthly = False
                        rowAccountDetails.PayForXYears = True
                        rowAccountDetails.NoOfYears = 1
                        Me.ImageComboBoxEditYears.Enabled = True

                End Select
                ' end of code added TJS 02/12/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails.AddActivationAccountDetailsRow(rowAccountDetails)

                bConnectorsActivated = False
                ' start of code added TJS 20/11/13
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                    rowAccountDetails.Activate3DCart = True
                    rowAccountDetails.ThreeDCartCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(THREE_D_CART_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbe3DCartCount.Enabled = True
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(THREE_D_CART_CONNECTOR_CODE) Then
                        rowAccountDetails.Activate3DCart = True
                        rowAccountDetails.ThreeDCartCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(THREE_D_CART_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbe3DCartCount.Enabled = True
                    Else
                        Me.cbe3DCartCount.Enabled = False
                    End If
                Else
                    Me.cbe3DCartCount.Enabled = False
                End If
                ' end of code added TJS 20/11/13
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateASPStorefront = True
                    rowAccountDetails.ASPStorefrontCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbeASPStorefrontCount.Enabled = True ' TJS 21/03/11
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateASPStorefront = True
                        rowAccountDetails.ASPStorefrontCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeASPStorefrontCount.Enabled = True
                    Else
                        Me.cbeASPStorefrontCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeASPStorefrontCount.Enabled = False ' TJS 21/03/11
                End If
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateMagento = True
                    rowAccountDetails.MagentoCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(MAGENTO_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbeMagentoCount.Enabled = True ' TJS 21/03/11
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(MAGENTO_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateMagento = True
                        rowAccountDetails.MagentoCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(MAGENTO_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeMagentoCount.Enabled = True
                    Else
                        Me.cbeMagentoCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeMagentoCount.Enabled = False ' TJS 21/03/11
                End If
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateVolusion = True
                    rowAccountDetails.VolusionCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(VOLUSION_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbeVolusionCount.Enabled = True ' TJS 21/03/11
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(VOLUSION_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateVolusion = True
                        rowAccountDetails.VolusionCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(VOLUSION_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeVolusionCount.Enabled = True
                    Else
                        Me.cbeVolusionCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeVolusionCount.Enabled = False ' TJS 21/03/11
                End If
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateAmazon = True
                    rowAccountDetails.AmazonCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbeAmazonCount.Enabled = True ' TJS 21/03/11
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateAmazon = True
                        rowAccountDetails.AmazonCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeAmazonCount.Enabled = True
                    Else
                        Me.cbeAmazonCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeAmazonCount.Enabled = False ' TJS 21/03/11
                End If
                ' start of code added TJS 05/07/12
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateAmazonFBA = True
                    rowAccountDetails.AmazonFBACount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_FBA_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbeAmazonFBACount.Enabled = True
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateAmazonFBA = True
                        rowAccountDetails.AmazonFBACount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(AMAZON_FBA_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeAmazonFBACount.Enabled = True
                    Else
                        Me.cbeAmazonFBACount.Enabled = False
                    End If
                Else
                    Me.cbeAmazonFBACount.Enabled = False
                End If
                ' end of code added TJS 05/07/12
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateChanAdv = True
                    rowAccountDetails.ChanAdvCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbeChanAdvCount.Enabled = True ' TJS 21/03/11
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateChanAdv = True
                        rowAccountDetails.ChanAdvCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeChanAdvCount.Enabled = True
                    Else
                        Me.cbeChanAdvCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeChanAdvCount.Enabled = False ' TJS 21/03/11
                End If
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                    rowAccountDetails.ActivateEBay = True ' TJS 02/12/11
                    rowAccountDetails.EBayCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(EBAY_CONNECTOR_CODE) ' TJS 02/12/11
                    bConnectorsActivated = True ' TJS 02/12/11
                    Me.cbeEBayCount.Enabled = True ' TJS 02/12/11
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(EBAY_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateEBay = True
                        rowAccountDetails.EBayCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(EBAY_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeEBayCount.Enabled = True
                    Else
                        Me.cbeEBayCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeEBayCount.Enabled = False ' TJS 02/12/11
                End If
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                    rowAccountDetails.ActivateSearsCom = True ' TJS 16/01/12
                    rowAccountDetails.SearsComCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SEARS_DOT_COM_CONNECTOR_CODE) ' TJS 16/01/12
                    bConnectorsActivated = True ' TJS 16/01/12
                    Me.cbeSearsComCount.Enabled = True ' TJS 16/01/12
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateSearsCom = True
                        rowAccountDetails.SearsComCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(SEARS_DOT_COM_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeSearsComCount.Enabled = True
                    Else
                        Me.cbeSearsComCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeSearsComCount.Enabled = False ' TJS 16/01/12
                End If
                ' start of code added TJS 20/08/13
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateFileImport = True
                    bConnectorsActivated = True
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateFileImport = True
                        bConnectorsActivated = True
                    End If
                End If
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateProspectLead = True
                    bConnectorsActivated = True
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateProspectLead = True
                        bConnectorsActivated = True
                    End If
                End If
                ' end of code added TJS 20/08/13
                If Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                    rowAccountDetails.ActivateShopCom = True
                    rowAccountDetails.ShopComCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE)
                    bConnectorsActivated = True
                    Me.cbeShopComCount.Enabled = True ' TJS 21/03/11
                    ' start of code added TJS 17/02/12 
                ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    If Me.m_ActivationWizardSectionContainerFacade.HasConnectorBeenActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        rowAccountDetails.ActivateShopCom = True
                        rowAccountDetails.ShopComCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorLastActivatedAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE)
                        bConnectorsActivated = True
                        Me.cbeShopComCount.Enabled = True
                    Else
                        Me.cbeShopComCount.Enabled = False
                    End If
                    ' end of code added TJS 17/02/12 
                Else
                    Me.cbeShopComCount.Enabled = False ' TJS 21/03/11
                End If
                If Not bConnectorsActivated And Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                    rowAccountDetails.ImportWizardOnly = True
                End If
                rowAccountDetails.ImportWizardQty = Me.m_ActivationWizardSectionContainerFacade.InventoryImportLimit
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
            End If
            'AddRemoveConnectorEventHandlers(True) ' TJS 10/06/12
            'AddRemovePayPeriodEventHandlers(True) ' TJS 21/03/11 TJS 10/06/12

            Me.AddressControlBilling.LayoutItemAddress.Text = Me.AddressControlBilling.LayoutItemAddress.Text.Replace("Billing", "")
            Me.AddressControlBilling.LayoutItemCity.Text = Me.AddressControlBilling.LayoutItemCity.Text.Replace("Billing", "") ' TJS 29/01/13
            Me.AddressControlBilling.LayoutItemState.Text = Me.AddressControlBilling.LayoutItemState.Text.Replace("Billing", "") ' TJS 29/01/13
            Me.AddressControlBilling.LayoutItemPostal.Text = Me.AddressControlBilling.LayoutItemPostal.Text.Replace("Billing", "") ' TJS 29/01/13
            Me.AddressControlBilling.LayoutItemCounty.Text = Me.AddressControlBilling.LayoutItemCounty.Text.Replace("Billing", "")
            Me.AddressControlBilling.LayoutItemCountry.Text = Me.AddressControlBilling.LayoutItemCountry.Text.Replace("Billing", "")
            If Me.AddressControlBilling.comboCountry.Text = "United States of America" Then
                Me.AddressControlBilling.LayoutItemCountry.Text = Me.AddressControlBilling.LayoutItemCountry.Text.Replace("PostalCode", "Zipcode")
            Else
                Me.AddressControlBilling.LayoutItemCountry.Text = Me.AddressControlBilling.LayoutItemCountry.Text.Replace("PostalCode", "Postcode")
            End If
            Me.PhoneControlBilling.LayoutItemTelephone.Text = Me.PhoneControlBilling.LayoutItemTelephone.Text.Replace("Billing", "") ' TJS 29/01/13
            Me.PhoneControlBilling.LayoutItemTelephone.Text = Me.PhoneControlBilling.LayoutItemTelephone.Text.Replace("PhoneExtension", "Extn") ' TJS 29/01/13

            Me.AddressControlShipping.LayoutItemAddress.Text = Me.AddressControlShipping.LayoutItemAddress.Text.Replace("Shipping", "")
            Me.AddressControlShipping.LayoutItemCity.Text = Me.AddressControlShipping.LayoutItemCity.Text.Replace("Shipping", "") ' TJS 29/01/13
            Me.AddressControlShipping.LayoutItemState.Text = Me.AddressControlShipping.LayoutItemState.Text.Replace("Shipping", "") ' TJS 29/01/13
            Me.AddressControlShipping.LayoutItemPostal.Text = Me.AddressControlShipping.LayoutItemPostal.Text.Replace("Shipping", "") ' TJS 29/01/13
            Me.AddressControlShipping.LayoutItemCounty.Text = Me.AddressControlShipping.LayoutItemCounty.Text.Replace("Shipping", "")
            Me.AddressControlShipping.LayoutItemCountry.Text = Me.AddressControlShipping.LayoutItemCountry.Text.Replace("Shipping", "")
            If Me.AddressControlShipping.comboCountry.Text = "United States of America" Then
                Me.AddressControlShipping.LayoutItemCountry.Text = Me.AddressControlShipping.LayoutItemCountry.Text.Replace("PostalCode", "Zipcode")
            Else
                Me.AddressControlShipping.LayoutItemCountry.Text = Me.AddressControlShipping.LayoutItemCountry.Text.Replace("PostalCode", "Postcode")
            End If
            Me.PhoneControlShipping.LayoutItemTelephone.Text = Me.PhoneControlShipping.LayoutItemTelephone.Text.Replace("Shipping", "") ' TJS 29/01/13
            Me.PhoneControlShipping.LayoutItemTelephone.Text = Me.PhoneControlShipping.LayoutItemTelephone.Text.Replace("PhoneExtension", "Extn") ' TJS 29/01/13

            strTemp = Me.m_ActivationWizardSectionContainerFacade.GetActivationCost(True) ' TJS 27/07/11
            If strTemp <> "" Then
                XMLActivationCost = XDocument.Parse(strTemp) ' TJS 27/07/11
            Else
                XMLActivationCost = New XDocument ' TJS 27/07/11
            End If
            If XMLActivationCost.ToString <> "" Then ' TJS 02/12/11
                If Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                    If GetElementText(XMLActivationCost, "LerrynQuote/TurnoverBased").ToUpper = "YES" Then ' TJS 09/08/13
                        decExistingActivationCost.TurnoverBased = True ' TJS 09/08/13
                        decExistingActivationCost.PercentageOfSales = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PercentageOfSales")) ' TJS 09/08/13
                        decExistingActivationCost.MonthlyMinimum = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/MonthlyMinimum")) ' TJS 09/08/13
                        decExistingActivationCost.PricePerMonth = 0 ' TJS 09/08/13
                        decExistingActivationCost.PriceFor1Year = 0 ' TJS 09/08/13
                        decExistingActivationCost.PriceFor2Years = 0 ' TJS 09/08/13
                        decExistingActivationCost.PriceFor3Years = 0 ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).TurnoverBasedPricing = True ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PercentageOfSales = decExistingActivationCost.PercentageOfSales ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MonthlyMinimum = decExistingActivationCost.MonthlyMinimum ' TJS 09/08/13
                    Else
                        decExistingActivationCost.PricePerMonth = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PricePerMonth"))
                        decExistingActivationCost.PriceFor1Year = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor1Year"))
                        decExistingActivationCost.PriceFor2Years = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor2Years"))
                        decExistingActivationCost.PriceFor3Years = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor3Years"))
                        decExistingActivationCost.TurnoverBased = False ' TJS 09/08/13
                        decExistingActivationCost.PercentageOfSales = 0 ' TJS 09/08/13
                        decExistingActivationCost.MonthlyMinimum = 0 ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).TurnoverBasedPricing = False ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PercentageOfSales = 0 ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MonthlyMinimum = 0 ' TJS 09/08/13
                    End If
                Else
                    decExistingActivationCost.PricePerMonth = 0
                    decExistingActivationCost.PriceFor1Year = 0
                    decExistingActivationCost.PriceFor2Years = 0
                    decExistingActivationCost.PriceFor3Years = 0
                    If GetElementText(XMLActivationCost, "LerrynQuote/TurnoverBased").ToUpper = "YES" Then ' TJS 13/11/13
                        decExistingActivationCost.TurnoverBased = True ' TJS 13/11/13
                        decExistingActivationCost.PercentageOfSales = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PercentageOfSales")) ' TJS 13/11/13
                        decExistingActivationCost.MonthlyMinimum = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/MonthlyMinimum")) ' TJS 13/11/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).TurnoverBasedPricing = True ' TJS 13/11/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PercentageOfSales = decExistingActivationCost.PercentageOfSales ' TJS 13/11/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MonthlyMinimum = decExistingActivationCost.MonthlyMinimum ' TJS 13/11/13
                    Else
                        decExistingActivationCost.TurnoverBased = False ' TJS 09/08/13
                        decExistingActivationCost.PercentageOfSales = 0 ' TJS 09/08/13
                        decExistingActivationCost.MonthlyMinimum = 0 ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).TurnoverBasedPricing = False ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PercentageOfSales = 0 ' TJS 09/08/13
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MonthlyMinimum = 0 ' TJS 09/08/13
                    End If
                End If
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 09/08/13
                If GetElementText(XMLActivationCost, "LerrynQuote/NearExpiry").ToUpper = "YES" Then
                    bNearExpiry = True
                End If
                UpdateActivationCostDisplay()
                EnableDisablePaymentSettings(True) ' TJS 20/08/13
                Me.btnUpdateActivationCost.Enabled = False ' TJS 17/02/12
                Me.WizardControlActivation.EnableNextButton = True ' TJS 17/02/12
                strActivationCostMessage = GetElementText(XMLActivationCost, "LerrynQuote/Message") 'Mark Kee' 22/4/15                
                Me.lblPricingBasis.Text = "The current pricing for eShopCONNECTED is as follows :-" & vbCrLf & vbCrLf & strActivationCostMessage.Replace("eShopCONNECT", "eShopCONNECTED") ' Mark Kee 22/4/15
                'Me.lblPricingBasis.Text = "The current pricing for eShopCONNECTED is as follows :-" & vbCrLf & vbCrLf & GetElementText(XMLActivationCost, "LerrynQuote/Message") ' TJS 13/04/11 TJS 25/8/11  Comment Code Mark Kee 22/04/15

            Else
                Me.lblPricingBasis.Text = "Unable to display current pricing or update your activation of eShopCONNECTED" & vbCrLf & "at this time as the Lerryn licence server cannot be contacted." ' TJS 02/12/11
            End If

            If Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                If Me.m_ActivationWizardSectionContainerFacade.ActivationExpires <= Date.Today.AddDays(5) Then
                    If Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                        strWelcomeMessage = "Your activation for " & PRODUCT_NAME & " for " & IS_PRODUCT_NAME & " will expire on " ' TJS 24/08/12
                    Else
                        strWelcomeMessage = "Your evaluation activation for " & PRODUCT_NAME & " for " & IS_PRODUCT_NAME & " will expire on " ' TJS 24/08/12
                    End If
                    strWelcomeMessage = strWelcomeMessage & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString
                    strWelcomeMessage = strWelcomeMessage & " and your next Invoice is due on " & dteNextInvoiceDue.ToShortDateString & "." ' TJS 26/01/12
                Else
                    If Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                        strWelcomeMessage = "Your activation for " & PRODUCT_NAME & " for " & IS_PRODUCT_NAME & " is valid until " ' TJS 24/08/12
                    Else
                        strWelcomeMessage = "Your evaluation activation for " & PRODUCT_NAME & " for " & IS_PRODUCT_NAME & " is valid until " ' TJS 24/08/12
                    End If
                    strWelcomeMessage = strWelcomeMessage & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString
                    If bAutoRenewal And bNextInvoiceSet Then ' TJS 26/01/12
                        strWelcomeMessage = strWelcomeMessage & " and your next Invoice is due on " & dteNextInvoiceDue.ToShortDateString & "." ' TJS 26/01/12
                    End If
                End If
                strWelcomeMessage = strWelcomeMessage & vbCrLf & vbCrLf
                If strExistingPaymentPeriod = "M" Then
                    strWelcomeMessage = strWelcomeMessage & "Your activation will automatically be renewed each month when your PayPay Invoice"
                    strWelcomeMessage = strWelcomeMessage & vbCrLf & "has been paid." & vbCrLf & vbCrLf ' TJS 18/01/13
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayMonthly = True ' TJS 21/03/11
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11

                ElseIf bAutoRenewal Then
                    strWelcomeMessage = strWelcomeMessage & "Your activation will automatically be renewed shortly before the above expiry date"
                    strWelcomeMessage = strWelcomeMessage & vbCrLf & "when your PayPay Invoice has been paid." & vbCrLf & vbCrLf ' TJS 18/01/13
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayForXYears = True ' TJS 21/03/11
                    Select Case strExistingPaymentPeriod
                        Case "3Y"
                            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).NoOfYears = 3 ' TJS 21/03/11
                        Case "2Y"
                            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).NoOfYears = 2 ' TJS 21/03/11
                        Case Else
                            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).NoOfYears = 1 ' TJS 21/03/11
                    End Select
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
                    ' is activation near expiry ?
                    If GetElementText(XMLActivationCost, "LerrynQuote/NearExpiry").ToUpper <> "YES" Then
                        ' no, can't change payment method
                        Me.chkPayMonthly.Enabled = False
                        Me.chkPayForXYears.Enabled = False
                        Me.ImageComboBoxEditYears.Enabled = False ' TJS 20/08/13
                    End If
                End If
                If XMLActivationCost.ToString <> "" Then ' TJS 02/12/11
                    strWelcomeMessage = strWelcomeMessage & "Click Next if you want to activate additional eShopCONNECTORS or to cancel "
                    strWelcomeMessage = strWelcomeMessage & vbCrLf & "your activation renewal and cease using eShopCONNECTED." ' TJS 18/01/13
                Else
                    strWelcomeMessage = strWelcomeMessage & "Click Next to view your current activation details" ' TJS 02/12/11
                End If
                Me.lblSelectConnectors1.Text = "You currently have the following eShopCONNECTORS activated.  "
                If dteEndFreeTrial > Date.Today Then
                    Me.lblSelectConnectors1.Text = Me.lblSelectConnectors1.Text & "Your free trial expires on " & dteEndFreeTrial.ToShortDateString & "."

                ElseIf dteEndFreeTrial = Date.Today Then
                    Me.lblSelectConnectors1.Text = Me.lblSelectConnectors1.Text & "Your free trial expires today."

                Else
                    Me.lblSelectConnectors1.Text = Me.lblSelectConnectors1.Text & "Your current activation(s) expire on " & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString
                    If bAutoRenewal And bNextInvoiceSet Then ' TJS 17/02/12
                        Me.lblSelectConnectors1.Text = Me.lblSelectConnectors1.Text & " and your next Invoice is due on " & dteNextInvoiceDue.ToShortDateString & "." ' TJS 17/02/12
                    End If
                End If
                Me.lblSelectConnectors2.Text = ""
                Me.lblSelectConnectors3.Text = "" ' TJS 20/08/13
                Me.chkCancel.Visible = True
                Me.lblCancelText.Visible = True
                Me.WizardControlActivation.buttonCancel.Text = "Finish" ' TJS 29/03/11

            ElseIf Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                strWelcomeMessage = "Your activation for " & PRODUCT_NAME & " for " & IS_PRODUCT_NAME & " expired on " ' TJS 24/08/12
                strWelcomeMessage = strWelcomeMessage & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString
                strWelcomeMessage = strWelcomeMessage & vbCrLf & vbCrLf
                strWelcomeMessage = strWelcomeMessage & "Click Next if you want to re-activate eShopCONNECTED."
                Me.btnUpdateActivationCost.Enabled = True ' TJS 17/02/12

            ElseIf XMLActivationCost.ToString <> "" Then ' TJS 02/12/11
                strWelcomeMessage = "This Wizard will guide you through the process of activating " & PRODUCT_NAME & " for " & IS_PRODUCT_NAME & "." & vbCrLf & vbCrLf ' TJS 24/08/12
                If Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode = CB_DEMO_CUSTOMER_CODE Or _
                    Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode = CB14_DEMO_CUSTOMER_CODE Then ' TJS 13/03/13 TJS 18/02/14
                    strWelcomeMessage = strWelcomeMessage & "You will be asked which eShopCONNECTORS you wish to activate for your trial." & vbCrLf & vbCrLf ' TJS 13/03/13
                    strWelcomeMessage = strWelcomeMessage & "As you are activating eShopCONNECTED in the " & IS_PRODUCT_NAME & " Demo company, your activation will" & vbCrLf ' TJS 13/03/13
                    strWelcomeMessage = strWelcomeMessage & "expire at the end of your trial and it will not be automatically renewed." & vbCrLf & vbCrLf ' TJS 13/03/13
                    strWelcomeMessage = strWelcomeMessage & "Please contact Lerryn (Sales@lerryn.com) for further assistance if required." ' TJS 13/03/13
                Else
                    strWelcomeMessage = strWelcomeMessage & "You will be asked which eShopCONNECTORS you wish to activate, you will then be asked to enter your" & vbCrLf
                    strWelcomeMessage = strWelcomeMessage & "Billing and System Location details so that we can charge you for the use of eShopCONNECTED at the end" & vbCrLf ' TJS 18/01/13
                    strWelcomeMessage = strWelcomeMessage & "of your trial period and keep " & PRODUCT_NAME & " functioning." & vbCrLf & vbCrLf & "Please click Next to continue."
                End If
                Me.WizardControlActivation.WelcomeMessage = strWelcomeMessage
                strActivationCostMessage = GetElementText(XMLActivationCost, "LerrynQuote/Message") 'Mark Kee' 22/4/15  
                Me.lblPricingBasis.Text = "The current pricing for eShopCONNECTED is as follows :-" & vbCrLf & vbCrLf & strActivationCostMessage.Replace("eShopCONNECT", "eShopCONNECTED") ' TJS 13/04/11
                'Me.lblPricingBasis.Text = "The current pricing for eShopCONNECTED is as follows :-" & vbCrLf & vbCrLf & GetElementText(XMLActivationCost, "LerrynQuote/Message") ' TJS 13/04/11

                Me.lblInventoryImport.Text = Me.lblInventoryImport.Text.Replace("EndFreeTrialDate", dteEndFreeTrial.ToShortDateString)

            Else
                strWelcomeMessage = "Unable to activate eShopCONNECTED at this time as the Lerryn licence server cannot be contacted." ' TJS 02/12/11
                Me.WizardControlActivation.EnableNextButton = False
            End If
            Me.WizardControlActivation.WelcomeMessage = strWelcomeMessage

            Me.lblInventoryImport.Text = Me.lblInventoryImport.Text.Replace("EndFreeTrialDate", dteEndFreeTrial.ToShortDateString)

        Catch ex As Exception ' TJS 16/01/12
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex) ' TJS 16/01/12

        End Try

    End Sub
#End Region

#Region " SetFocus "
    Public Sub SetFocus()

    End Sub
#End Region

#Region " HaveConnectorsChanged "
    Private Function HaveConnectorsChanged(ByRef DecreaseNotIncrease As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJs             | 2011.2.00 | Modified to cater for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        DecreaseNotIncrease = False
        HaveConnectorsChanged = False
        For iLoop = 1 To 12 ' TJS 02/12/11 TJS 16/01/12 TJS 05/07/12 TJS 20/08/13 TJS 20/11/13
            Select Case iLoop
                Case 1 ' ASPDotNetStorefront
                    ' is connector current inactive and user wants to activate it ?
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ASPStorefrontCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        ' yes
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ASPStorefrontCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ASPStorefrontCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If

                Case 2 ' Magento
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MagentoCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MagentoCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(MAGENTO_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MagentoCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(MAGENTO_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If

                Case 3 ' Volusion
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).VolusionCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).VolusionCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(VOLUSION_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).VolusionCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(VOLUSION_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If

                Case 4 ' Amazon
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more onnections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If

                Case 5 ' Channel Advisor
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ChanAdvCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ChanAdvCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ChanAdvCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If

                Case 6 ' Shop.com
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShopComCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShopComCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShopComCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If

                    ' start of code added TJS 02/12/11
                Case 7 ' eBay
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EBayCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EBayCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(EBAY_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EBayCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(EBAY_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If
                    ' end of code added TJS 02/12/11

                    ' start of code added TJS 16/01/12
                Case 8 ' Sears.com
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SearsComCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SearsComCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SEARS_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SearsComCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SEARS_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If
                    ' end of code added TJS 16/01/12

                    ' start of code added TJS 05/07/12
                Case 9 ' Amazon FBA
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_FBA_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more onnections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_FBA_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If
                    ' end of code added TJS 05/07/12

                    ' start of code added TJS 20/08/13
                Case 10 ' File Import
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateFileImport And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateFileImport And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If

                Case 11 ' Prospect and Lead
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateProspectLead And _
                      Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateProspectLead And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If
                    ' end of code added TJS 20/08/13

                    ' start of code added TJS 20/11/13
                Case 12 ' 3DCart
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ThreeDCartCount > 0 And _
                        Not Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ThreeDCartCount > _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(THREE_D_CART_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more connections
                        HaveConnectorsChanged = True

                    ElseIf Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) And _
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ThreeDCartCount < _
                        Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(THREE_D_CART_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants less connections
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True

                    ElseIf Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart And _
                        Me.m_ActivationWizardSectionContainerFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        HaveConnectorsChanged = True
                        DecreaseNotIncrease = True
                    End If
                    ' end of code added TJS 20/11/13

            End Select
        Next

    End Function
#End Region

#Region " GetUpgradeCost "
    Private Function GetUpgradeCost() As Decimal

        Dim decUpgradeTimeMultiplier As Decimal

        If Me.chkPayMonthly.Checked Then
            ' monthly payments, upgrade is simply difference in monthly cost - ignore remainder of current month
            Return decNewActivationCost.PricePerMonth - decExistingActivationCost.PricePerMonth

        ElseIf Me.chkPayForXYears.Checked Then
            ' annual or multi-annual payment - is product activated with full activation ?
            If Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                ' yes, more than 1 year left before expiry ?
                If Me.m_ActivationWizardSectionContainerFacade.ActivationExpires < Date.Today.AddYears(1) Then
                    ' no, multiplier is number of months left divided by 12
                    decUpgradeTimeMultiplier = CDec(DateDiff(DateInterval.Month, Date.Today.AddYears(1), Me.m_ActivationWizardSectionContainerFacade.ActivationExpires) / 12)
                    Return (decNewActivationCost.PriceFor1Year - decExistingActivationCost.PriceFor1Year) * decUpgradeTimeMultiplier

                ElseIf Me.m_ActivationWizardSectionContainerFacade.ActivationExpires < Date.Today.AddYears(2) Then
                    ' between 1 and 2 years left, multiplier is number of months left divided by 24
                    decUpgradeTimeMultiplier = CDec(DateDiff(DateInterval.Month, Date.Today.AddYears(1), Me.m_ActivationWizardSectionContainerFacade.ActivationExpires) / 24)
                    Return (decNewActivationCost.PriceFor2Years - decExistingActivationCost.PriceFor2Years) * decUpgradeTimeMultiplier

                Else
                    ' more than 2 years, multiplier is number of months left divided by 36
                    decUpgradeTimeMultiplier = CDec(DateDiff(DateInterval.Month, Date.Today.AddYears(1), Me.m_ActivationWizardSectionContainerFacade.ActivationExpires) / 36)
                    Return (decNewActivationCost.PriceFor3Years - decExistingActivationCost.PriceFor3Years) * decUpgradeTimeMultiplier


                End If
            Else
                If Me.ImageComboBoxEditYears.SelectedIndex >= 0 Then
                    If Me.ImageComboBoxEditYears.EditValue.ToString = "1" Then
                        Return decNewActivationCost.PriceFor1Year

                    ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "2" Then
                        Return decNewActivationCost.PriceFor2Years

                    ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "3" Then
                        Return decNewActivationCost.PriceFor3Years

                    Else
                        Return 0
                    End If

                Else
                    Return 0
                End If
            End If

        Else
            Return 0
        End If

    End Function
#End Region

#Region " UpdateActivationCostDisplay "
    Private Sub UpdateActivationCostDisplay()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '                                        | and for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 17/02/12 | TJS             | 2010.2.07 | removed setting of Update Cost and Next button as causes issues when activation has expired
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to cater for Turnover based pricing 
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to lock Monthly/Yearly payment controls for Turnover based pricing  
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTemp As XDocument
        Dim XMLConnectorList As System.Collections.Generic.IEnumerable(Of XElement), XMLConnector As XElement
        Dim strConnectorPrice As String, strCurrency As String

        If XMLActivationCost.ToString <> "" Then ' TJS 02/12/11
            strCurrency = GetElementText(XMLActivationCost, "LerrynQuote/Currency")
            If strCurrency = "EURO" Then
                strCurrencySymbol = "€"
            ElseIf strCurrency = "GBP" Then
                strCurrencySymbol = "£"
            Else
                strCurrencySymbol = "$"
            End If
            If GetElementText(XMLActivationCost, "LerrynQuote/TurnoverBased").ToUpper = "YES" Then ' TJS 09/08/13
                strConnectorPrice = GetElementText(XMLActivationCost, "LerrynQuote/PercentageOfSales") & "% of Sales"
                Me.lblASPStorefrontCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblMagentoCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblVolusionCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblAmazonCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblChanlAdvCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblShopComCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblEBayCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblSearsComCost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblAmazonFBACost.Text = strConnectorPrice ' TJS 09/08/13
                Me.lblFileImportCost.Text = strConnectorPrice ' TJS 20/08/13
                Me.lblProspectLeadCost.Text = strConnectorPrice ' TJS 20/08/13
                Me.lbl3DCartCost.Text = strConnectorPrice ' TJS 20/11/13
                Me.lblTurnoverBased.Text = "The above are subject to a minimum monthly charge of " & strCurrencySymbol & GetElementText(XMLActivationCost, "LerrynQuote/MonthlyMinimum") ' TJS 09/08/13
                Me.chkPayForXYears.Checked = False ' TJS 09/08/13
                Me.chkPayMonthly.Checked = True ' TJS 09/08/13
                Me.ImageComboBoxEditYears.SelectedIndex = -1 ' TJS 13/11/13
                Me.chkPayForXYears.Enabled = False ' TJS 13/11/13
                Me.chkPayMonthly.Enabled = False ' TJS 13/11/13
                Me.ImageComboBoxEditYears.Enabled = False ' TJS 13/11/13
                decNewActivationCost.TurnoverBased = True ' TJS 13/11/13
                decNewActivationCost.PricePerMonth = 0 ' TJS 13/11/13
                decNewActivationCost.PriceFor1Year = 0 ' TJS 13/11/13
                decNewActivationCost.PriceFor2Years = 0 ' TJS 13/11/13
                decNewActivationCost.PriceFor3Years = 0 ' TJS 13/11/13
            Else
                XMLConnectorList = XMLActivationCost.XPathSelectElements("LerrynQuote/Connector") ' TJS 02/12/11
                For Each XMLConnector In XMLConnectorList
                    XMLTemp = XDocument.Parse(XMLConnector.ToString) ' TJS 02/12/11
                    If Me.chkPayMonthly.Checked Then
                        strConnectorPrice = strCurrencySymbol & " " & GetElementText(XMLTemp, "Connector/PricePerMonth")

                    ElseIf Me.chkPayForXYears.Checked Then
                        If Me.ImageComboBoxEditYears.SelectedIndex >= 0 Then
                            If Me.ImageComboBoxEditYears.EditValue.ToString = "1" Then
                                strConnectorPrice = strCurrencySymbol & " " & GetElementText(XMLTemp, "Connector/PriceFor1Year")

                            ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "2" Then
                                strConnectorPrice = strCurrencySymbol & " " & GetElementText(XMLTemp, "Connector/PriceFor2Years")

                            ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "3" Then
                                strConnectorPrice = strCurrencySymbol & " " & GetElementText(XMLTemp, "Connector/PriceFor3Years")

                            Else
                                strConnectorPrice = ""
                            End If
                        Else
                            strConnectorPrice = ""
                        End If
                    Else
                        strConnectorPrice = ""
                    End If
                    Select Case GetElementText(XMLTemp, "Connector/ProductCode")
                        Case ASP_STORE_FRONT_CONNECTOR_CODE
                            Me.lblASPStorefrontCost.Text = strConnectorPrice

                        Case MAGENTO_CONNECTOR_CODE
                            Me.lblMagentoCost.Text = strConnectorPrice

                        Case VOLUSION_CONNECTOR_CODE
                            Me.lblVolusionCost.Text = strConnectorPrice

                        Case AMAZON_SELLER_CENTRAL_CONNECTOR_CODE
                            Me.lblAmazonCost.Text = strConnectorPrice

                        Case CHANNEL_ADVISOR_CONNECTOR_CODE
                            Me.lblChanlAdvCost.Text = strConnectorPrice

                        Case SHOP_DOT_COM_CONNECTOR_CODE
                            Me.lblShopComCost.Text = strConnectorPrice

                        Case EBAY_CONNECTOR_CODE ' TJS 02/12/11
                            Me.lblEBayCost.Text = strConnectorPrice ' TJS 02/12/11

                        Case SEARS_DOT_COM_CONNECTOR_CODE ' TJS 16/01/12
                            Me.lblSearsComCost.Text = strConnectorPrice ' TJS 16/01/12

                        Case AMAZON_FBA_CONNECTOR_CODE ' TJS 05/07/12
                            Me.lblAmazonFBACost.Text = strConnectorPrice ' TJS 05/07/12

                        Case ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE ' TJS 20/08/13
                            Me.lblFileImportCost.Text = strConnectorPrice ' TJS 20/08/13

                        Case PROSPECT_IMPORT_CONNECTOR_CODE ' TJS 20/08/13
                            Me.lblProspectLeadCost.Text = strConnectorPrice ' TJS 20/08/13

                        Case THREE_D_CART_CONNECTOR_CODE ' TJS 20/11/13
                            Me.lbl3DCartCost.Text = strConnectorPrice ' TJS 20/11/13

                    End Select
                Next
                decNewActivationCost.TurnoverBased = False ' TJS 13/11/13
                decNewActivationCost.PricePerMonth = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PricePerMonth"))
                decNewActivationCost.PriceFor1Year = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor1Year"))
                decNewActivationCost.PriceFor2Years = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor2Years"))
                decNewActivationCost.PriceFor3Years = CDec("0" & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor3Years"))
            End If

            If Me.chkPayMonthly.Checked Then
                If Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                    Me.lblTotalCost.Text = "Monthly cost to renew " & strCurrencySymbol & " "
                Else
                    Me.lblTotalCost.Text = "Monthly cost to activate " & strCurrencySymbol & " "
                End If
                Me.lblTotalCost.Text = Me.lblTotalCost.Text & GetElementText(XMLActivationCost, "LerrynQuote/PricePerMonth")

            ElseIf Me.chkPayForXYears.Checked Then
                If Me.ImageComboBoxEditYears.SelectedIndex >= 0 Then
                    If Me.ImageComboBoxEditYears.EditValue.ToString = "1" Then
                        If Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                            Me.lblTotalCost.Text = "Annual cost to renew " & strCurrencySymbol & " "
                        Else
                            Me.lblTotalCost.Text = "Annual cost to activate " & strCurrencySymbol & " "
                        End If
                        Me.lblTotalCost.Text = Me.lblTotalCost.Text & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor1Year")

                    ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "2" Then
                        If Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                            Me.lblTotalCost.Text = "Cost to renew for 2 years " & strCurrencySymbol & " "
                        Else
                            Me.lblTotalCost.Text = "Cost to activate for 2 years " & strCurrencySymbol & " "
                        End If
                        Me.lblTotalCost.Text = Me.lblTotalCost.Text & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor2Years")

                    ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "3" Then
                        If Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                            Me.lblTotalCost.Text = "Cost to renew for 3 years " & strCurrencySymbol & " "
                        Else
                            Me.lblTotalCost.Text = "Cost to activate for 3 years " & strCurrencySymbol & " "
                        End If
                        Me.lblTotalCost.Text = Me.lblTotalCost.Text & GetElementText(XMLActivationCost, "LerrynQuote/PriceFor3Years")

                    Else
                        Me.lblTotalCost.Text = ""
                        decNewActivationCost.TurnoverBased = False ' TJS 13/11/13
                        decNewActivationCost.PricePerMonth = 0
                        decNewActivationCost.PriceFor1Year = 0
                        decNewActivationCost.PriceFor2Years = 0
                        decNewActivationCost.PriceFor3Years = 0

                    End If
                Else
                    Me.lblTotalCost.Text = ""
                    decNewActivationCost.TurnoverBased = False ' TJS 13/11/13
                    decNewActivationCost.PricePerMonth = 0
                    decNewActivationCost.PriceFor1Year = 0
                    decNewActivationCost.PriceFor2Years = 0
                    decNewActivationCost.PriceFor3Years = 0

                End If
            Else
                Me.lblTotalCost.Text = ""
                decNewActivationCost.TurnoverBased = False ' TJS 13/11/13
                decNewActivationCost.PricePerMonth = 0
                decNewActivationCost.PriceFor1Year = 0
                decNewActivationCost.PriceFor2Years = 0
                decNewActivationCost.PriceFor3Years = 0

            End If

            If Me.lblTotalCost.Text <> "" Then
                Select Case strCurrency
                    Case "EURO"
                        Me.lblCurrency.Text = "Prices are in Euros"

                    Case "GBP"
                        Me.lblCurrency.Text = "Prices are in UK £"

                    Case "USD"
                        Me.lblCurrency.Text = "Prices are in US $"

                    Case Else

                End Select
            End If

        End If

    End Sub
#End Region

#Region " ClearActivationCostDisplay "
    Private Sub ClearActivationCostDisplay()

        Me.lblTotalCost.Text = ""
        decNewActivationCost.PricePerMonth = 0
        decNewActivationCost.PriceFor1Year = 0
        decNewActivationCost.PriceFor2Years = 0
        decNewActivationCost.PriceFor3Years = 0

    End Sub
#End Region

#Region " ClearConnectorSettings "
    Private Sub ClearConnectorSettings()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 02/12/11 | TJs             | 2011.2.00 | Modified to cater for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart = False ' TJS 20/11/13
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ThreeDCartCount = 0 ' TJS 20/11/13
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront = False ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ASPStorefrontCount = 0 ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento = False ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MagentoCount = 0 ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion = False ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).VolusionCount = 0 ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon = False ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount = 0 ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv = False ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ChanAdvCount = 0 ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay = False ' TJS 02/12/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EBayCount = 0 ' TJS 02/12/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom = False ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShopComCount = 0 ' TJS 21/03/11
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom = False ' TJS 16/01/12
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SearsComCount = 0 ' TJS 16/01/12
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA = False ' TJS 05/07/12
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount = 0 ' TJS 05/07/12
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateFileImport = False ' TJS 20/08/13
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateProspectLead = False ' TJS 20/08/13
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
        Me.lbl3DCartCost.Text = "" ' TJS 20/11/13
        Me.lblASPStorefrontCost.Text = ""
        Me.lblMagentoCost.Text = ""
        Me.lblVolusionCost.Text = ""
        Me.lblAmazonCost.Text = ""
        Me.lblAmazonFBACost.Text = "" ' TJS 05/07/12
        Me.lblChanlAdvCost.Text = ""
        Me.lblEBayCost.Text = "" ' TJS 02/12/11
        Me.lblSearsComCost.Text = "" ' TJS 16/01/12
        Me.lblShopComCost.Text = ""
        Me.lblFileImportCost.Text = "" ' TJS 20/08/13
        Me.lblProspectLeadCost.Text = "" ' TJS 20/08/13
        Me.lblTotalCost.Text = ""
        Me.lblCurrency.Text = ""
        Me.cbe3DCartCount.Enabled = False ' TJS 20/11/13
        Me.cbeASPStorefrontCount.Enabled = False ' TJS 21/03/11
        Me.cbeMagentoCount.Enabled = False ' TJS 21/03/11
        Me.cbeVolusionCount.Enabled = False ' TJS 21/03/11
        Me.cbeAmazonCount.Enabled = False ' TJS 21/03/11
        Me.cbeChanAdvCount.Enabled = False ' TJS 21/03/11
        Me.cbeEBayCount.Enabled = False ' TJS 02/12/11
        Me.cbeShopComCount.Enabled = False ' TJS 21/03/11
        Me.cbeSearsComCount.Enabled = False ' TJS 16/01/12
        Me.cbeAmazonFBACount.Enabled = False ' TJS 05/07/12

    End Sub
#End Region

#Region " ClearActivationSelectionErrors "
    Private Sub ClearActivationSelectionErrors()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJs             | 2011.2.00 | Modified to cater for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.chk3DCart.ErrorText = "" ' TJS 20/11/13
        Me.chkASPStorefront.ErrorText = ""
        Me.chkMagento.ErrorText = ""
        Me.chkVolusion.ErrorText = ""
        Me.chkAmazon.ErrorText = ""
        Me.chkAmazonFBA.ErrorText = "" ' TJS 05/07/12
        Me.chkChanAdvisor.ErrorText = ""
        Me.chkEBay.ErrorText = "" ' TJS 02/12/11
        Me.chkShopCom.ErrorText = ""
        Me.chkSearsCom.ErrorText = "" ' TJS 16/01/12
        Me.chkFileImport.ErrorText = "" ' TJS 20/08/13
        Me.chkProspectLead.ErrorText = "" ' TJS 20/08/13
        Me.chkImportWizardOnly.ErrorText = ""

    End Sub
#End Region

#Region " EnableDisablePaymentSettings "
    Private Sub EnableDisablePaymentSettings(ByVal Enable As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to check if Pay Monthly selected before enabling the Years to Purchase list
        ' 09/08/13 | TSJ             | 2013.2.02 | Modified to cater for turnover based billing
        ' 20/08/13 | TJS             | 2013.2.04 | Corrected enabling of peyment period controls for turnover based billing
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If decNewActivationCost.TurnoverBased Then ' TSJ 09/08/13 TJS 20/08/13
            Me.chkPayMonthly.Enabled = False ' TSJ 09/08/13
            Me.chkPayForXYears.Enabled = False ' TSJ 09/08/13
            Me.chkPayForXYears.Visible = False ' TJS 20/08/13
            Me.ImageComboBoxEditYears.Enabled = False ' TSJ 09/08/13
            Me.ImageComboBoxEditYears.Visible = False ' TJS 20/08/13
            Me.lblTotalCost.Visible = False ' TJS 20/08/13

        Else
            Me.chkPayForXYears.Visible = True ' TJS 20/08/13
            Me.chkPayMonthly.Enabled = Enable
            Me.chkPayForXYears.Enabled = Enable
            If Not Me.chkPayMonthly.Checked Then ' TSJ 02/12/11
                Me.ImageComboBoxEditYears.Enabled = Enable
            Else
                Me.ImageComboBoxEditYears.Enabled = False ' TSJ 02/12/11
            End If
            Me.ImageComboBoxEditYears.Visible = True ' TJS 20/08/13
            Me.lblTotalCost.Visible = True ' TJS 20/08/13
        End If

    End Sub
#End Region

#Region " AddRemoveConnectorEventHandlers "
    Private Sub AddRemoveConnectorEventHandlers(ByVal Add As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJs             | 2011.2.00 | Modified to cater for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Add Then
            AddHandler chk3DCart.CheckedChanged, AddressOf chk3DCart_CheckedChanged ' TJS 20/11/13
            AddHandler chkAmazon.CheckedChanged, AddressOf chkAmazon_CheckedChanged
            AddHandler chkAmazonFBA.CheckedChanged, AddressOf chkAmazonFBA_CheckedChanged ' TJS 05/07/12
            AddHandler chkASPStorefront.CheckedChanged, AddressOf chkASPStorefront_CheckedChanged
            AddHandler chkChanAdvisor.CheckedChanged, AddressOf chkChanAdvisor_CheckedChanged
            AddHandler chkEBay.CheckedChanged, AddressOf chkEBay_CheckedChanged ' TJS 02/12/11
            AddHandler chkMagento.CheckedChanged, AddressOf chkMagento_CheckedChanged
            AddHandler chkSearsCom.CheckedChanged, AddressOf chkSearsCom_CheckedChanged ' TJS 16/01/12
            AddHandler chkShopCom.CheckedChanged, AddressOf chkShopCom_CheckedChanged
            AddHandler chkVolusion.CheckedChanged, AddressOf chkVolusion_CheckedChanged
            AddHandler chkFileImport.CheckedChanged, AddressOf chkFileImport_CheckedChanged ' TJS 20/08/13
            AddHandler chkProspectLead.CheckedChanged, AddressOf chkProspectLead_CheckedChanged ' TJS 20/08/13
            AddHandler chkImportWizardOnly.CheckedChanged, AddressOf chkImportWizardOnly_CheckedChanged
            AddHandler cbe3DCartCount.SelectedIndexChanged, AddressOf cbe3DCartCount_SelectedIndexChanged ' TJS 20/11/13
            AddHandler cbeAmazonCount.SelectedIndexChanged, AddressOf cbeAmazonCount_SelectedIndexChanged
            AddHandler cbeAmazonFBACount.SelectedIndexChanged, AddressOf cbeAmazonFBACount_SelectedIndexChanged ' TJS 05/07/12
            AddHandler cbeASPStorefrontCount.SelectedIndexChanged, AddressOf cbeASPStorefrontCount_SelectedIndexChanged
            AddHandler cbeChanAdvCount.SelectedIndexChanged, AddressOf cbeChanAdvCount_SelectedIndexChanged
            AddHandler cbeEBayCount.SelectedIndexChanged, AddressOf cbeEBayCount_SelectedIndexChanged ' TJS 02/12/11
            AddHandler cbeMagentoCount.SelectedIndexChanged, AddressOf cbeMagentoCount_SelectedIndexChanged
            AddHandler cbeSearsComCount.SelectedIndexChanged, AddressOf cbeSearsComCount_SelectedIndexChanged ' TJS 16/01/12
            AddHandler cbeShopComCount.SelectedIndexChanged, AddressOf cbeShopComCount_SelectedIndexChanged
            AddHandler cbeVolusionCount.SelectedIndexChanged, AddressOf cbeVolusionCount_SelectedIndexChanged
            AddHandler cbeImportWizardQty1.SelectedIndexChanged, AddressOf cbeImportWizardQty1_SelectedIndexChanged
            AddHandler cbeImportWizardQty2.SelectedIndexChanged, AddressOf cbeImportWizardQty2_SelectedIndexChanged
            AddHandler chkPurchaseNow.CheckedChanged, AddressOf chkPurchaseNow_CheckedChanged

        Else
            RemoveHandler chk3DCart.CheckedChanged, AddressOf chk3DCart_CheckedChanged ' TJS 20/11/13
            RemoveHandler chkAmazon.CheckedChanged, AddressOf chkAmazon_CheckedChanged
            RemoveHandler chkAmazonFBA.CheckedChanged, AddressOf chkAmazonFBA_CheckedChanged ' TJS 05/07/12
            RemoveHandler chkASPStorefront.CheckedChanged, AddressOf chkASPStorefront_CheckedChanged
            RemoveHandler chkChanAdvisor.CheckedChanged, AddressOf chkChanAdvisor_CheckedChanged
            RemoveHandler chkEBay.CheckedChanged, AddressOf chkEBay_CheckedChanged ' TJS 02/12/11
            RemoveHandler chkMagento.CheckedChanged, AddressOf chkMagento_CheckedChanged
            RemoveHandler chkSearsCom.CheckedChanged, AddressOf chkSearsCom_CheckedChanged ' TJS 16/01/12
            RemoveHandler chkShopCom.CheckedChanged, AddressOf chkShopCom_CheckedChanged
            RemoveHandler chkVolusion.CheckedChanged, AddressOf chkVolusion_CheckedChanged
            RemoveHandler chkFileImport.CheckedChanged, AddressOf chkFileImport_CheckedChanged ' TJS 20/08/13
            RemoveHandler chkProspectLead.CheckedChanged, AddressOf chkProspectLead_CheckedChanged ' TJS 20/08/13
            RemoveHandler chkImportWizardOnly.CheckedChanged, AddressOf chkImportWizardOnly_CheckedChanged
            RemoveHandler cbe3DCartCount.SelectedIndexChanged, AddressOf cbe3DCartCount_SelectedIndexChanged ' TJS 20/11/13
            RemoveHandler cbeAmazonCount.SelectedIndexChanged, AddressOf cbeAmazonCount_SelectedIndexChanged
            RemoveHandler cbeAmazonFBACount.SelectedIndexChanged, AddressOf cbeAmazonFBACount_SelectedIndexChanged ' TJS 05/07/12
            RemoveHandler cbeASPStorefrontCount.SelectedIndexChanged, AddressOf cbeASPStorefrontCount_SelectedIndexChanged
            RemoveHandler cbeChanAdvCount.SelectedIndexChanged, AddressOf cbeChanAdvCount_SelectedIndexChanged
            RemoveHandler cbeEBayCount.SelectedIndexChanged, AddressOf cbeEBayCount_SelectedIndexChanged ' TJS 02/12/11
            RemoveHandler cbeMagentoCount.SelectedIndexChanged, AddressOf cbeMagentoCount_SelectedIndexChanged
            RemoveHandler cbeSearsComCount.SelectedIndexChanged, AddressOf cbeSearsComCount_SelectedIndexChanged ' TJS 16/01/12
            RemoveHandler cbeShopComCount.SelectedIndexChanged, AddressOf cbeShopComCount_SelectedIndexChanged
            RemoveHandler cbeVolusionCount.SelectedIndexChanged, AddressOf cbeVolusionCount_SelectedIndexChanged
            RemoveHandler cbeImportWizardQty1.SelectedIndexChanged, AddressOf cbeImportWizardQty1_SelectedIndexChanged
            RemoveHandler cbeImportWizardQty2.SelectedIndexChanged, AddressOf cbeImportWizardQty2_SelectedIndexChanged
            RemoveHandler chkPurchaseNow.CheckedChanged, AddressOf chkPurchaseNow_CheckedChanged
        End If

    End Sub
#End Region

#Region " AddRemovePayPeriodEventHandlers "
    Private Sub AddRemovePayPeriodEventHandlers(ByVal Add As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/03/11 | TJS             | 2011.0.02 | Function added
        '------------------------------------------------------------------------------------------

        If Add Then
            AddHandler chkPayMonthly.CheckedChanged, AddressOf PayPeriodChangedMnth
            AddHandler chkPayForXYears.CheckedChanged, AddressOf PayPeriodChangedYrs
            AddHandler ImageComboBoxEditYears.SelectedIndexChanged, AddressOf NumberOfYearsToBuyChanged
        Else
            RemoveHandler chkPayMonthly.CheckedChanged, AddressOf PayPeriodChangedMnth
            RemoveHandler chkPayForXYears.CheckedChanged, AddressOf PayPeriodChangedYrs
            RemoveHandler ImageComboBoxEditYears.SelectedIndexChanged, AddressOf NumberOfYearsToBuyChanged
        End If

    End Sub
#End Region

#Region " InventoryImportQtySet "
    Private Sub InventoryImportQtySet(ByRef QtyControlSet As DevExpress.XtraEditors.ComboBoxEdit, ByRef SecondaryQtyControl As DevExpress.XtraEditors.ComboBoxEdit)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to update imported quantity when set too low for Purchase Now
        '                                        | and to ensure Finish button says Cancel after any changes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strMessage As String

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        If QtyControlSet.SelectedIndex >= 0 Then
            If CInt(QtyControlSet.EditValue) > 250 Then
                ' import quantity selected which requires full activation - has previous activation exired ?
                If Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                    ' yes, full activation required so no message
                ElseIf Me.m_ActivationWizardSectionContainerFacade.IsActivated And Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                    ' currently activated with full activation, again no message
                ElseIf Me.m_ActivationWizardSectionContainerFacade.IsActivated And Not Me.m_ActivationWizardSectionContainerFacade.IsFullActivation And Not Me.chkPurchaseNow.Checked Then
                    ' currently activated with eval activation, so inform user that they must purchase full activation now unless Purchase Now clicked
                    strMessage = "Your selected Inventory Import quantity exceeds that permitted on an evaluation activation." & vbCrLf & vbCrLf
                    strMessage = strMessage & "To import this number of Inventory Items, you must purchase a full activation." & vbCrLf & vbCrLf
                    ' are we near expiry date ?
                    If bNearExpiry Then
                        ' yes
                        strMessage = strMessage & "Your evaluation activation is due to expire on " & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString
                    Else
                        strMessage = strMessage & "Your existing activation does not expire until " & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString

                    End If
                    strMessage = strMessage & vbCrLf & vbCrLf & "If you purchase a full activation code now, at the cost quoted, the activation will "
                    strMessage = strMessage & "become effecive immediately, but the expiry date will be extended to include the time remaining from "
                    strMessage = strMessage & "your evaluation period."
                    ' save quantity before we update other fields otherwise it gets reset
                    QtyControlSet.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
                    If Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage, "Purchase updated activation", Interprise.Framework.Base.Shared.MessageWindowButtons.OKCancel, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Cancel Then
                        ' user cancelled setting of inventory import quantity - reset quantiry to free value
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PurchaseNow = False ' TJS 21/03/11
                    Else
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PurchaseNow = True ' TJS 21/03/11
                    End If
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11

                ElseIf Not Me.chkPurchaseNow.Checked Then
                    ' must be initial activation so inform user that they must purchase full activation now unless Purchase Now clicked
                    strMessage = "Your selected Inventory Import quantity exceeds that permitted on an evaluation activation." & vbCrLf & vbCrLf
                    strMessage = strMessage & "To import this number of Inventory Items, you must purchase a full activation." & vbCrLf & vbCrLf
                    strMessage = strMessage & "If you purchase a full activation code now, at the cost quoted, the activation will become effecive "
                    strMessage = strMessage & "immediately, but the expiry date will be extended to include the time normally provided for "
                    strMessage = strMessage & "an evaluation period." & vbCrLf & vbCrLf & "By clicking OK, you confirm that you are satisfied "
                    strMessage = strMessage & "that eShopCONNECTED will meet your requirements and you accept that no refund will be possble."
                    strMessage = strMessage & "Otherwise click Cancel to obtain an evaluation activation."
                    ' save quantity before we update other fields otherwise it gets reset
                    QtyControlSet.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
                    If Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage, "Purchase full activation", Interprise.Framework.Base.Shared.MessageWindowButtons.OKCancel, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Cancel Then
                        ' user cancelled setting of inventory import quantity - reset quantiry to free value
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PurchaseNow = False ' TJS 21/03/11
                    Else
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PurchaseNow = True ' TJS 21/03/11
                    End If
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
                End If
                Me.WizardControlActivation.buttonCancel.Text = "Cancel" ' TJS 02/12/11

            ElseIf Me.chkPurchaseNow.Checked And CInt(QtyControlSet.EditValue) <= 250 Then ' TJS 21/03/11
                ' user is purchasing an activation so minimum valid quantity is 2500
                strMessage = "The minimum Inventory Import limit for a full activation is 2500 and your selected value has been increased to this." ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 02/12/11
                Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage, "Inventory Import Limit", Interprise.Framework.Base.Shared.MessageWindowButtons.OK, Interprise.Framework.Base.Shared.MessageWindowIcon.Information, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button1) ' TJS 21/03/11
                Me.WizardControlActivation.buttonCancel.Text = "Cancel" ' TJS 02/12/11
            End If
        End If
        AddRemoveConnectorEventHandlers(True)

    End Sub
#End Region

#Region " GetElementText "
    Public Function GetElementText(ByVal XMLMessage As XDocument, ByVal ElementName As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Gets XML element values from an XML document - 
        '                            Used because Facade functions don't return data until activated
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use facade function
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return m_ActivationWizardSectionContainerFacade.GetXMLElementText(XMLMessage, ElementName)

    End Function
#End Region
#End Region

#Region " Events "
#Region " WizardPageChanged "
    Private Sub WizardControlActivation_Finished(ByVal sender As Object, ByVal e As System.EventArgs) Handles WizardControlActivation.Finished
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 22/03/11 | TJs             | 2011.0.03 | Modified to display activation status
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim formConfigSettings As SystemManager.Config.ConfigSettingsForm

        If Me.m_ActivationWizardSectionContainerFacade.IsActivated And bConfigUpdateRequired Then
            If Interprise.Presentation.Base.Message.MessageWindow.Show("You need to set/update your configuration settings for eShopCONNECTED." & vbCrLf & vbCrLf & "Would you like to do that now ?", "eShopCONNECTED Configuration", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button1) = DialogResult.Yes Then
                formConfigSettings = New SystemManager.Config.ConfigSettingsForm
                formConfigSettings.Show()
                formConfigSettings.DisplayActivationStatus() ' TJS 22/03/11
            End If
        End If

    End Sub

    Private Sub WizardPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles WizardControlActivation.SelectedPageChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Form re-written for Connected Business out-of-box version
        ' 22/03/11 | TJS             | 2011.0.03 | Modified to refresh activation cost display after initial connection
        '                                        | settings are displayed and to set config update required flag
        ' 29/03/11 | TJs             | 2011.0.04 | Modified to change Cancel button text instead of tying 
        '                                        | to show Finish button which appears at top left of form
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for Pricing Summary page and to split card error messages 
        '                                        | across 2 lines if necessary.  Also modified to allow display but not 
        '                                        | editing of activation details when licence server not contactable
        '                                        | and to cater for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 24/02/12 | TJS             | 2011.2.08 | Corrected Valid From error text
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings and 50000 Inventory Import qty option
        ' 18/01/13 | TJS             | 2012.1.17 | Modified to cater for PayPal invoicing
        ' 29/01/13 | TJS             | 2013.0.00 | Modified to cater for Amazon FBA being disabled in current release
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to detect the demo company and amend activation process accordingly
        ' 22/03/13 | TJS             | 2013.1.05 | Tidied finish message
        ' 09/08/13 | TJS             | 2013.2.02 | Corrected message when adding connector
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors and to hide 
        '                                        | Password fields as no longer needed with PayPal invoices
        ' 18/02/14 | TJS             | 2014.0.00 | Modified for CB14_DEMO_CUSTOMER_CODE
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim graphicFunction As System.Drawing.Graphics, sizeText As System.Drawing.SizeF ' TJS 02/12/11
        Dim strErrorDetails As String = "", strMessage As String, strActivationPeriod As String
        Dim strActivationType As String, strLine1 As String, strLine2 As String ' TJS 02/12/11
        Dim iErrorCode As Integer = 0, decActivationCost As Decimal
        Dim iStartPosn As Integer, iSpacePosn As Integer ' TJS 02/12/11
        Dim bValidationError As Boolean, bLessConnections As Boolean

        ' have we triggered this event by changing the focused page ?
        If Not Me.bIgnoreWizardPageChangeEvent Then
            ' no, which page is being displayed ?
            Cursor = Cursors.WaitCursor ' TJS 02/12/11
            If ReferenceEquals(e.Page, Me.TabPageWelcome) Then
                ' Welcome Code page, must have clicked Back
                Me.LabelActivationDetailsOrError.Visible = False
                Me.LabelActivationDetailsOrError.Text = ""
                Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = ""

            ElseIf ReferenceEquals(e.Page, Me.TabPagePriceSummary) Then ' TJS 02/12/11
                ' Current pricing summary page, was last page the Welcome page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageWelcome) Then ' TJS 02/12/11
                    ' yes, must have clicked Next, do we have pricing info i.e. could we contact licence server ?
                    ' start of code added TJS 02/12/11
                    If XMLActivationCost Is Nothing OrElse XMLActivationCost.ToString = "" Then
                        ' no, prevent any changes to activation options
                        Me.chkMagento.Enabled = False
                        Me.chkASPStorefront.Enabled = False
                        Me.chkVolusion.Enabled = False
                        Me.chkAmazon.Enabled = False
                        Me.chkAmazonFBA.Enabled = False ' TJS 05/07/12
                        Me.chkChanAdvisor.Enabled = False
                        Me.chkEBay.Enabled = False ' TJS 02/12/11
                        Me.chkSearsCom.Enabled = False ' TJS 16/01/12
                        Me.chkShopCom.Enabled = False
                        Me.chkFileImport.Enabled = False ' TJS 20/08/13
                        Me.chkProspectLead.Enabled = False ' TJS 20/08/13
                        Me.cbeASPStorefrontCount.Enabled = False
                        Me.cbeMagentoCount.Enabled = False
                        Me.cbeVolusionCount.Enabled = False
                        Me.cbeAmazonCount.Enabled = False
                        Me.cbeAmazonFBACount.Enabled = False ' TJS 05/07/12
                        Me.cbeChanAdvCount.Enabled = False
                        Me.cbeEBayCount.Enabled = False ' TJS 02/12/11
                        Me.cbeSearsComCount.Enabled = False ' TJS 16/01/12
                        Me.cbeShopComCount.Enabled = False
                        Me.cbeImportWizardQty1.Enabled = False
                        Me.chkImportWizardOnly.Enabled = False
                        Me.lblImportWizardQty1.Enabled = False
                        Me.chkPayMonthly.Enabled = False
                        Me.chkPayForXYears.Enabled = False
                        Me.ImageComboBoxEditYears.Enabled = False
                        Me.chkCancel.Enabled = False
                        Me.lblCancelText.Enabled = False
                        Me.btnUpdateActivationCost.Enabled = False
                        AddRemovePayPeriodEventHandlers(False)
                        AddRemoveConnectorEventHandlers(False)
                    End If
                    ' end of code added TJS 02/12/11
                Else
                    ' no, must have clicked Back
                    AddRemoveConnectorEventHandlers(False) ' TJS 10/06/12
                    AddRemovePayPeriodEventHandlers(False) ' TJS 10/06/12

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageActivationCode) Then
                ' Activations page, was last page the Pricing Summary page ?
                If ReferenceEquals(e.PrevPage, Me.TabPagePriceSummary) Then ' TJS 02/12/11
                    ' yes, must have clicked Next, do we have pricing info i.e. could we contact licence server ?
                    AddRemoveConnectorEventHandlers(True) ' TJS 10/06/12
                    AddRemovePayPeriodEventHandlers(True) ' TJS TJS 10/06/12
                    ' start of code added TJS 02/12/11
                    If XMLActivationCost.ToString <> "" Then
                        ' yes
                        UpdateActivationCostDisplay() ' TJS 22/03/11
                        EnableDisablePaymentSettings(True) ' TJS 20/08/13
                        If Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                            Me.WizardControlActivation.EnableBackButton = True
                            Me.WizardControlActivation.EnableNextButton = True
                            Me.btnUpdateActivationCost.Enabled = False
                        ElseIf Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 17/02/12
                            Me.WizardControlActivation.EnableBackButton = True ' TJS 17/02/12
                            Me.WizardControlActivation.EnableNextButton = False ' TJS 17/02/12
                            Me.btnUpdateActivationCost.Enabled = True ' TJS 17/02/12
                        Else
                            Me.WizardControlActivation.EnableBackButton = False
                            Me.WizardControlActivation.EnableNextButton = False
                            Me.btnUpdateActivationCost.Enabled = False
                        End If
                    End If
                Else
                    ' no, must have clicked Back
                End If
                Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Activations"

            ElseIf ReferenceEquals(e.Page, Me.TabPageEvaluationRestrictions) Then
                ' Inventory Import page, was last page the Activations page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageActivationCode) Then
                    ' yes, must have clicked Next, do we have pricing info i.e. could we contact licence server ?
                    If XMLActivationCost.ToString <> "" Then
                        ' yes, do we have any validation errors ?
                        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
                        Me.m_ActivationWizardSectionContainerFacade.Validate("ActivationAccountDetails")
                        bValidationError = False
                        If (Not Me.chkMagento.Checked And Not Me.chkASPStorefront.Checked And Not Me.chkVolusion.Checked And _
                            Not Me.chkAmazon.Checked And Not Me.chkAmazonFBA.Checked And Not Me.chkChanAdvisor.Checked And _
                            Not Me.chkShopCom.Checked And Not Me.chkEBay.Checked And Not Me.chkSearsCom.Checked And _
                            Not Me.chkFileImport.Checked And Not Me.chkProspectLead.Checked And _
                            Not Me.chkImportWizardOnly.Checked And Not Me.chkCancel.Checked) Then ' TJS 02/12/11 TJS 16/01/12 TJS 05/07/12 TJS 20/08/13
                            bValidationError = True
                            Me.chkMagento.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected"
                            Me.chkASPStorefront.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected"
                            Me.chkVolusion.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected" '
                            Me.chkAmazon.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected"
                            If Me.chkAmazonFBA.Enabled Then ' TJS 18/01/13
                                Me.chkAmazonFBA.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected" ' TJS 05/07/12
                            End If
                            Me.chkChanAdvisor.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected"
                            Me.chkEBay.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected" ' TJS 02/12/11
                            Me.chkSearsCom.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected" ' TJS 16/01/12
                            Me.chkShopCom.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected"
                            Me.chkFileImport.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected" ' TJS 20/08/13
                            Me.chkProspectLead.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected" ' TJS 20/08/13
                            Me.chkImportWizardOnly.ErrorText = "Either Import Wizard only or an eShopCONNECTOR must be selected"
                        End If
                        If Not Me.chkPayMonthly.Checked And Not Me.chkPayForXYears.Checked And Not Me.chkCancel.Checked Then
                            bValidationError = True
                            Me.chkPayMonthly.ErrorText = "You must select a payment period"
                            Me.chkPayForXYears.ErrorText = "You must select a payment period"

                        ElseIf Me.chkPayForXYears.Checked And Me.ImageComboBoxEditYears.SelectedIndex < 0 And Not Me.chkCancel.Checked Then
                            bValidationError = True
                            Me.ImageComboBoxEditYears.ErrorText = "You must select the number of years to purchase"

                        End If
                        If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails.HasErrors Or bValidationError Then
                            ' reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                            Me.bIgnoreWizardPageChangeEvent = True
                            Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                            Me.bIgnoreWizardPageChangeEvent = False

                        ElseIf Me.chkCancel.Checked Then
                            Me.lblInventoryImport.Visible = False
                            Me.chkPurchaseNow.Visible = False
                            If Interprise.Presentation.Base.Message.MessageWindow.Show("Are you sure you want to cancel the renewal of your Activation and cease using eShopCONNECTED ?", "Confirm Cancellation", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.No Then ' TJS 18/01/13
                                Me.lblInventoryImport.Visible = True
                                Me.chkPurchaseNow.Visible = True
                                ' reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                                Me.bIgnoreWizardPageChangeEvent = True
                                Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                                Me.bIgnoreWizardPageChangeEvent = False
                            Else
                                ' cancel activation
                                If Me.m_ActivationWizardSectionContainerFacade.CancelActivation() Then
                                    ' reset displayed page to Completed page (set Ignore page change flag as we don't want this routine run again)
                                    Me.bIgnoreWizardPageChangeEvent = True
                                    Me.WizardControlActivation.FocusPage = Me.TabPageComplete
                                    Me.bIgnoreWizardPageChangeEvent = False
                                Else
                                    ' reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                                    Me.bIgnoreWizardPageChangeEvent = True
                                    Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                                    Me.bIgnoreWizardPageChangeEvent = False
                                    Interprise.Presentation.Base.Message.MessageWindow.Show("Your cancellation request could not be actioned at this time.  Please try again later.", "eShopCONNECTED Cancellation")
                                End If
                            End If

                        ElseIf Me.chkImportWizardOnly.Checked And Me.chkPurchaseNow.Checked Then
                            ' user only wants Import wizard and Purchase Now set - has customer record already been created ?
                            If Me.m_ActivationWizardSectionContainerFacade.LerrynCustomerCode <> "" Then ' TJS 21/03/11
                                Me.bIgnoreWizardPageChangeEvent = True
                                Me.WizardControlActivation.FocusPage = Me.TabPagePayment
                                Me.bIgnoreWizardPageChangeEvent = False
                                Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Payment"
                                SetRequestActivationMessage() ' TJS 18/01/13

                            Else
                                ' no, reset displayed page to Billing Details page (set Ignore page change flag as we don't want this routine run again)
                                Me.bIgnoreWizardPageChangeEvent = True
                                Me.WizardControlActivation.FocusPage = Me.TabPageBilling
                                Me.bIgnoreWizardPageChangeEvent = False
                            End If
                            If Not Me.m_ActivationWizardSectionContainerFacade.IsActivated Then ' TJS 22/03/11
                                bConfigUpdateRequired = True ' TJS 22/03/11
                            End If

                        ElseIf Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                            bLessConnections = False
                            If HaveConnectorsChanged(bLessConnections) Then
                                bConfigUpdateRequired = True
                                If bLessConnections Then
                                    If Interprise.Presentation.Base.Message.MessageWindow.Show("You have reduced the number of connections required on one or more eShopCONNECTORS." & vbCrLf & vbCrLf & "If you purchase new activation codes now, the change will become effective immediately, rather than when your existing activation(s) expires.", "Purchase updated activation", Interprise.Framework.Base.Shared.MessageWindowButtons.OKCancel, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Cancel Then
                                        ' user cancelled purchase - reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                                        Me.bIgnoreWizardPageChangeEvent = True
                                        Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                                        Me.bIgnoreWizardPageChangeEvent = False
                                    End If
                                Else
                                    ' was user paying monthly and we are not still in the free trial ?
                                    If strExistingPaymentPeriod <> "M" And Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                                        ' no, are existing activations about to expire (i.e. in next 2 months) ?
                                        If bNearExpiry Then
                                            If Interprise.Presentation.Base.Message.MessageWindow.Show("Your existing activation(s) are due to expire on " & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString & vbCrLf & vbCrLf & "If you purchase new activation codes now, at the annual cost quoted, the additional connections requested will become effecive immediately, but there will be no additional charge for them in the period up your existing expiry date.", "Purchase updated activation", Interprise.Framework.Base.Shared.MessageWindowButtons.OKCancel, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Cancel Then
                                                ' user cancelled purchase - reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                                                Me.bIgnoreWizardPageChangeEvent = True
                                                Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                                                Me.bIgnoreWizardPageChangeEvent = False

                                            ElseIf Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then ' TJS 02/12/11
                                                ' already have full activation, no need for Inventory Restrictions, does customer code exist ?
                                                If Me.m_ActivationWizardSectionContainerFacade.LerrynCustomerCode <> "" Then ' TJS 02/12/11
                                                    Me.bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                                                    Me.WizardControlActivation.FocusPage = Me.TabPagePayment ' TJS 02/12/11
                                                    Me.bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11
                                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Payment" ' TJS 02/12/11
                                                    SetRequestActivationMessage() ' TJS 18/01/13

                                                Else
                                                    Me.bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                                                    Me.WizardControlActivation.FocusPage = Me.TabPageBilling ' TJS 02/12/11
                                                    Me.bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11
                                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing" ' TJS 02/12/11
                                                End If

                                            Else
                                                Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                                            End If
                                        Else
                                            strMessage = "Your existing activation(s) do not expire until " & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString & vbCrLf & vbCrLf
                                            strMessage = strMessage & "The additional connections will become effective immediately with the same expiry date as youir existing activation(s). "
                                            strMessage = strMessage & "You will be charged " & strCurrencySymbol & " " & Format(GetUpgradeCost(), "0.00") & " now for the upgrade and the new "
                                            ' start of code added TJS 09/08/13
                                            If Me.chkPayMonthly.Checked Then
                                                strMessage = strMessage & "monthly cost quoted will apply thereafter, subject to any subsequent prices changes."
                                            Else
                                                strMessage = strMessage & "cost quoted will apply at the next activation renewal, subject to any subsequent prices changes."
                                            End If
                                            ' end of code added TJS 09/08/13
                                            If Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage, "Purchase updated activation", Interprise.Framework.Base.Shared.MessageWindowButtons.OKCancel, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Cancel Then ' TJS 09/08/13
                                                ' user cancelled purchase - reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                                                Me.bIgnoreWizardPageChangeEvent = True
                                                Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                                                Me.bIgnoreWizardPageChangeEvent = False
                                            Else
                                                ' start of code added TJS 09/08/13
                                                If Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                                                    ' already have full activation, no need for Inventory Restrictions, does customer code exist ?
                                                    If Me.m_ActivationWizardSectionContainerFacade.LerrynCustomerCode <> "" Then
                                                        Me.bIgnoreWizardPageChangeEvent = True
                                                        Me.WizardControlActivation.FocusPage = Me.TabPagePayment
                                                        Me.bIgnoreWizardPageChangeEvent = False
                                                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Payment"
                                                        SetRequestActivationMessage()

                                                    Else
                                                        Me.bIgnoreWizardPageChangeEvent = True
                                                        Me.WizardControlActivation.FocusPage = Me.TabPageBilling
                                                        Me.bIgnoreWizardPageChangeEvent = False
                                                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                                                    End If

                                                Else
                                                    ' end of code added TJS 09/08/13
                                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                                                End If
                                            End If
                                        End If
                                    Else
                                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                                    End If
                                End If

                            ElseIf Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then ' TJS 02/12/11 
                                ' already have full activation, no need for Inventory Restrictions, does customer code exist ?
                                If Me.m_ActivationWizardSectionContainerFacade.LerrynCustomerCode <> "" Then ' TJS 02/12/11
                                    Me.bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                                    Me.WizardControlActivation.FocusPage = Me.TabPagePayment ' TJS 02/12/11
                                    Me.bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11
                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Payment" ' TJS 02/12/11
                                    SetRequestActivationMessage() ' TJS 18/01/13

                                Else
                                    Me.bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                                    Me.WizardControlActivation.FocusPage = Me.TabPageBilling ' TJS 02/12/11
                                    Me.bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11
                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing" ' TJS 02/12/11
                                End If
                            End If

                        ElseIf Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                            ' has an Import Wizard Quantity been selected ?
                            If Me.cbeImportWizardQty2.SelectedIndex >= 0 OrElse Me.cbeImportWizardQty2.EditValue IsNot Nothing Then ' TJS 17/02/12
                                ' yes, is it the maximum ?
                                If CInt(Me.cbeImportWizardQty2.EditValue) >= 50000 Then ' TJS 05/07/12
                                    ' yes, advance displayed page to Payment page (set Ignore page change flag as we don't want this routine run again)
                                    Me.bIgnoreWizardPageChangeEvent = True
                                    Me.WizardControlActivation.FocusPage = Me.TabPagePayment
                                    Me.bIgnoreWizardPageChangeEvent = False
                                    SetRequestActivationMessage() ' TJS 18/01/13

                                Else
                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                                End If
                            Else
                                Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                            End If
                            If HaveConnectorsChanged(bLessConnections) Then ' TJS 22/03/11
                                bConfigUpdateRequired = True ' TJS 22/03/11
                            End If

                        Else
                            If Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then ' TJS 02/12/11 
                                ' already have full activation, no need for Inventory Restrictions, does customer code exist ?
                                If Me.m_ActivationWizardSectionContainerFacade.LerrynCustomerCode <> "" Then ' TJS 02/12/11
                                    Me.bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                                    Me.WizardControlActivation.FocusPage = Me.TabPagePayment ' TJS 02/12/11
                                    Me.bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11
                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Payment" ' TJS 02/12/11
                                    SetRequestActivationMessage() ' TJS 18/01/13

                                Else
                                    Me.bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                                    Me.WizardControlActivation.FocusPage = Me.TabPageBilling ' TJS 02/12/11
                                    Me.bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11
                                    Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing" ' TJS 02/12/11
                                End If
                            Else
                                Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing" ' TJS 02/12/11
                            End If
                            bConfigUpdateRequired = True ' TJS 22/03/11
                        End If
                    Else
                        ' reset displayed page to Completed page (set Ignore page change flag as we don't want this routine run again)
                        Me.bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlActivation.FocusPage = Me.TabPageComplete
                        Me.WizardControlActivation.FinishMessage = "Click Finish to close the Activation Wizard"
                        Me.WizardControlActivation.DisplayNextButton = False
                        Me.bIgnoreWizardPageChangeEvent = False
                    End If

                Else
                    ' no, must have clicked Back - was Inventory Import page skipped ?
                    If (Me.chkImportWizardOnly.Checked And Me.chkPurchaseNow.Checked) Or Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then ' TJS 21/03/11 TJS 09/08/13
                        ' reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                        Me.bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                        Me.bIgnoreWizardPageChangeEvent = False
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Activations"
                    Else
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                    End If
                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageBilling) Then
                ' Billing Details page, was last page the Inventory Import page
                If ReferenceEquals(e.PrevPage, Me.TabPageEvaluationRestrictions) Then
                    ' yes, has Customer Code been created ?
                    If Me.m_ActivationWizardSectionContainerFacade.LerrynCustomerCode <> "" Then ' 21/03/11
                        ' advance displayed page to Payment page (set Ignore page change flag as we don't want this routine run again)
                        Me.bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlActivation.FocusPage = Me.TabPagePayment
                        Me.bIgnoreWizardPageChangeEvent = False
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Payment"
                        SetRequestActivationMessage() ' TJS 18/01/13

                    Else
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                    End If

                    ' hide password fields and fill with dummy value as we now send PayPal invoices
                    Me.lblPassword.Visible = False ' TJS 13/03/13
                    Me.txtPassword.Visible = False ' TJS 13/03/13
                    Me.txtPassword.EditValue = "Dummy99" ' TJS 13/03/13
                    ' save password value before we update other fields otherwise it gets reset
                    Me.txtPassword.DataBindings("EditValue").WriteValue() ' TJS 13/03/13
                    Me.lblConfirmPassword.Visible = False ' TJS 13/03/13
                    Me.txtConfirmPassword.Visible = False ' TJS 13/03/13
                    Me.txtConfirmPassword.EditValue = "Dummy99" ' TJS 13/03/13
                    ' save confirm password value before we update other fields otherwise it gets reset
                    Me.txtConfirmPassword.DataBindings("EditValue").WriteValue() ' TJS 13/03/13

                Else
                    ' no, must have clicked Back - were Billing and System Location pages skipped because customer code already present ?
                    If Me.m_ActivationWizardSectionContainerFacade.LerrynCustomerCode <> "" Then
                        ' yes, do we have a full activation ?
                        If Not Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then ' TJS 02/12/11 
                            ' no, reset displayed page to Inventory Import page (set Ignore page change flag as we don't want this routine run again)
                            Me.bIgnoreWizardPageChangeEvent = True
                            Me.WizardControlActivation.FocusPage = Me.TabPageEvaluationRestrictions
                            Me.bIgnoreWizardPageChangeEvent = False
                        Else
                            ' yes, reset displayed page to Activation page (set Ignore page change flag as we don't want this routine run again)
                            Me.bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                            Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode ' TJS 02/12/11
                            Me.bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11
                        End If
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Activations"
                    Else
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Billing"
                    End If
                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageDelivery) Then
                ' System Location Details page, was last page the Billing Details page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageBilling) Then
                    ' yes, must have clicked Next - do we have any validation errors ?
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
                    Me.m_ActivationWizardSectionContainerFacade.Validate("ActivationAccountDetails")
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails.HasErrors Then
                        ' yes, reset displayed page to Billing Details page (set Ignore page change flag as we don't want this routine run again)
                        Me.bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlActivation.FocusPage = Me.TabPageBilling
                        Me.bIgnoreWizardPageChangeEvent = False
                    Else
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Shipping"
                        SetDeliveryAddress(Me, Nothing)
                    End If
                Else
                    ' no, must have clicked Back - were Billing and System Location pages skipped because product has already been activated
                    If Me.m_ActivationWizardSectionContainerFacade.IsActivated Or Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                        ' reset displayed page to Activations page (set Ignore page change flag as we don't want this routine run again)
                        Me.bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlActivation.FocusPage = Me.TabPageActivationCode
                        Me.bIgnoreWizardPageChangeEvent = False
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Activations"
                    Else
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Shipping"
                    End If
                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPagePayment) Then
                ' Payment Details page, was last page the System Location Details page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageDelivery) Then
                    ' yes, must have clicked Next - do we have any validation errors ?
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
                    Me.m_ActivationWizardSectionContainerFacade.Validate("ActivationAccountDetails")
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails.HasErrors Then
                        ' yes, reset displayed page to Billing Details page (set Ignore page change flag as we don't want this routine run again)
                        Me.bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlActivation.FocusPage = Me.TabPageDelivery
                        Me.bIgnoreWizardPageChangeEvent = False
                    Else
                        Me.m_ActivationWizardSectionContainerFacade.AccountDetailsValidationStage = "Payment"
                        SetRequestActivationMessage() ' TJS 18/01/13
                    End If
                Else
                    ' no, must have clicked Back, but this would trigger another purchase so
                    ' reset displayed page to Finish page (set Ignore page change flag as we don't want this routine run again)
                    Me.bIgnoreWizardPageChangeEvent = True
                    Me.WizardControlActivation.FocusPage = Me.TabPageComplete
                    Me.bIgnoreWizardPageChangeEvent = False
                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageComplete) Then
                ' Finish page, was last page the Payment Details page ?
                If ReferenceEquals(e.PrevPage, Me.TabPagePayment) Then
                    ' yes, must have clicked Next - do we have any validation errors ?
                    bValidationError = False
                    Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
                    Me.m_ActivationWizardSectionContainerFacade.Validate("ActivationAccountDetails")
                    If Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails.HasErrors Or bValidationError Then
                        ' yes, reset displayed page to Payment page (set Ignore page change flag as we don't want this routine run again)
                        Me.bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlActivation.FocusPage = Me.TabPagePayment
                        Me.bIgnoreWizardPageChangeEvent = False
                        Me.WizardControlActivation.EnableNextButton = True
                    Else
                        ' no, 
                        strActivationPeriod = ""
                        If Me.chkPayMonthly.Checked Then
                            strActivationPeriod = "M"
                        ElseIf Me.chkPayForXYears.Checked Then
                            If Me.ImageComboBoxEditYears.SelectedIndex >= 0 Then
                                strActivationPeriod = Me.ImageComboBoxEditYears.EditValue.ToString & "Y"
                            End If
                        End If
                        strActivationType = SetActivationType() ' TJS 18/01/13
                        If GetUpgradeCost() > 0.005 Then
                            decActivationCost = GetUpgradeCost()
                        Else
                            If Me.chkPayMonthly.Checked Then
                                decActivationCost = decNewActivationCost.PricePerMonth
                            Else
                                Select Case strActivationPeriod
                                    Case "1Y"
                                        decActivationCost = decNewActivationCost.PriceFor1Year
                                    Case "2Y"
                                        decActivationCost = decNewActivationCost.PriceFor2Years
                                    Case "3Y"
                                        decActivationCost = decNewActivationCost.PriceFor3Years
                                End Select
                            End If
                        End If
                        If Me.m_ActivationWizardSectionContainerFacade.PurchaseActivation(strActivationPeriod, strActivationType, decActivationCost, dteEndFreeTrial, dteNextInvoiceDue, Me.chkPurchaseNow.Checked, iErrorCode, strErrorDetails) Then
                            strMessage = "You have successfully completed the Activation Wizard." & vbCrLf & vbCrLf
                            If Me.chkCancel.Checked Then
                                strMessage = strMessage & "Your cancellation request has been actioned and eShopCONNECTED will cease to operate after "
                                strMessage = strMessage & Me.m_ActivationWizardSectionContainerFacade.ActivationExpires.ToShortDateString

                            ElseIf dteEndFreeTrial > Date.Today And Not Me.chkPurchaseNow.Checked Then
                                strMessage = strMessage & "Your free trial of eShopCONNECTED expires on " & dteEndFreeTrial.ToShortDateString
                                ' is user activating in demo company ?
                                If Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode <> CB_DEMO_CUSTOMER_CODE And _
                                    Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode <> CB14_DEMO_CUSTOMER_CODE Then ' TJS 13/03/13 TJS 18/02/14
                                    ' no
                                    If Me.chkPayMonthly.Checked Then
                                        strMessage = strMessage & " and your first invoice for the purchase of an Activation for eShopCONNECTED "
                                    Else
                                        strMessage = strMessage & " and your invoice for the purchase of an Activation for eShopCONNECTED "
                                    End If
                                    strMessage = strMessage & "will sent to your registered email address shortly before " & dteNextInvoiceDue.ToShortDateString
                                End If
                            Else
                                strMessage = strMessage & "Your activation of eShopCONNECTED has been requested and a PayPal invoice has been sent to your registered email address."
                                strMessage = strMessage & vbCrLf & vbCrLf & "Once it has been paid, Activation code(s) will be issued and your can use this Activation Wizard" & vbCrLf ' TJS 22/03/13
                                strMessage = strMessage & "to import the Activation Code(s) and confirm they have been received." & vbCrLf & vbCrLf ' TJS 22/03/13
                                strMessage = strMessage & "NOTE Whilst Activation Codes are normally issued within minutes of confirmation of invoice payment being received" & vbCrLf
                                strMessage = strMessage & "from PayPal, on occasions there can be delays in the PayPal payment notification process."
                            End If
                            strMessage = strMessage & vbCrLf & vbCrLf & "Click Finish to the wizard."
                            Me.WizardControlActivation.FinishMessage = strMessage
                            Me.WizardControlActivation.EnableBackButton = False
                            Me.WizardControlActivation.EnableCancelButton = False

                        Else
                            strMessage = strErrorDetails ' TJS 02/12/11
                            graphicFunction = Me.lblErrorDetails.CreateGraphics() ' TJS 02/12/11
                            sizeText = graphicFunction.MeasureString(strMessage, Me.lblErrorDetails.Font) ' TJS 02/12/11
                            If sizeText.Width > Me.lblErrorDetails.Width Then ' TJS 02/12/11
                                iStartPosn = CInt((strMessage.Length / 2) - 0.5) ' TJS 02/12/11
                                iSpacePosn = InStr(iStartPosn, strErrorDetails, " ") ' TJS 02/12/11
                                strMessage = strErrorDetails.Substring(0, iSpacePosn - 1).Trim & vbCrLf & strErrorDetails.Substring(iSpacePosn).Trim ' TJS 02/12/11
                            End If
                            Me.lblErrorDetails.Text = strMessage ' TJS 02/12/11
                            Me.lblErrorDetails.Visible = True
                            ' reset displayed page to Payment page (set Ignore page change flag as we don't want this routine run again)
                            Me.bIgnoreWizardPageChangeEvent = True
                            Me.WizardControlActivation.FocusPage = Me.TabPagePayment
                            Me.bIgnoreWizardPageChangeEvent = False
                            Me.WizardControlActivation.EnableNextButton = True
                        End If
                    End If
                End If
            Else
                ' no, must have clicked Back
                Me.LabelActivationDetailsOrError.Text = ""
            End If
            Cursor = Cursors.Default ' TJS 02/12/11
        End If

    End Sub
#End Region

#Region " ActivationWizardSectionContainer_Resize "
    Private Sub ActivationWizardSectionContainer_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    This function makes sure that the Lerryn logo remains 
        '                    positioned correctly when the form is resized 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/10/08 | TJS             | 2008.0.01 | Original 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.PictureBoxLerrynLogo.Top = Me.Height - 155

    End Sub
#End Region

#Region " PayPeriodChanged "
    Private Sub PayPeriodChangedMnth(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to remove event handlers during updates
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to prevent monthly payment on Inventory Import only 
        '                                        | and to ensure Finish button says Cancel after any changes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemovePayPeriodEventHandlers(False) ' TJS 21/03/11
        If Me.chkPayMonthly.Checked Then
            If Me.chkImportWizardOnly.Checked Then ' TJS 02/12/11
                Me.chkPayMonthly.Checked = False ' TJS 02/12/11
                Interprise.Presentation.Base.Message.MessageWindow.Show("You cannot pay monthly if you only want to activate the eShopCONNECTED Inventory Import Wizard.") ' TJS 02/12/11
                AddRemovePayPeriodEventHandlers(True) ' TJS 02/12/11
                Exit Sub ' TJS 02/12/11
            End If
            ' save checkbox state before we update other fields otherwise it gets reset
            Me.chkPayMonthly.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayForXYears = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Cancel = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
            Me.ImageComboBoxEditYears.Enabled = False
            Me.chkPayMonthly.ErrorText = ""
            Me.chkPayForXYears.ErrorText = ""
            Me.chkCancel.ErrorText = ""
            Me.WizardControlActivation.buttonCancel.Text = "Cancel" ' TJS 02/12/11

        ElseIf Not Me.chkCancel.Checked Then
            ' save checkbox state before we update other fields otherwise it gets reset
            Me.chkPayMonthly.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayForXYears = True ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
            Me.ImageComboBoxEditYears.Enabled = True
            Me.chkPayMonthly.ErrorText = ""
            Me.chkPayForXYears.ErrorText = ""
            Me.chkCancel.ErrorText = ""
            Me.WizardControlActivation.buttonCancel.Text = "Cancel" ' TJS 02/12/11

        End If
        If Not Me.btnUpdateActivationCost.Enabled Then
            UpdateActivationCostDisplay()
            Me.btnUpdateActivationCost.Enabled = False ' TJS 17/02/12
            Me.WizardControlActivation.EnableNextButton = True ' TJS 17/02/12
        End If
        AddRemovePayPeriodEventHandlers(True) ' TJS 21/03/11

    End Sub

    Private Sub PayPeriodChangedYrs(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to remove event handlers during updates
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to ensure Finish button says Cancel after any changes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemovePayPeriodEventHandlers(False) ' TJS 21/03/11
        If Me.chkPayForXYears.Checked Then
            ' save checkbox state before we update other fields otherwise it gets reset
            Me.chkPayForXYears.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayMonthly = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Cancel = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
            Me.ImageComboBoxEditYears.Enabled = True
            Me.chkPayMonthly.ErrorText = ""
            Me.chkPayForXYears.ErrorText = ""
            Me.chkCancel.ErrorText = ""
            Me.WizardControlActivation.buttonCancel.Text = "Cancel" ' TJS 02/12/11

        ElseIf Not Me.chkCancel.Checked Then
            ' save checkbox state before we update other fields otherwise it gets reset
            Me.chkPayForXYears.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayMonthly = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
            Me.ImageComboBoxEditYears.Enabled = False
            Me.chkPayMonthly.ErrorText = ""
            Me.chkPayForXYears.ErrorText = ""
            Me.chkCancel.ErrorText = ""
            Me.WizardControlActivation.buttonCancel.Text = "Cancel" ' TJS 02/12/11

        End If
        If Not Me.btnUpdateActivationCost.Enabled Then
            UpdateActivationCostDisplay()
            Me.btnUpdateActivationCost.Enabled = False ' TJS 17/02/12
            Me.WizardControlActivation.EnableNextButton = True ' TJS 17/02/12
        End If
        AddRemovePayPeriodEventHandlers(True) ' TJS 21/03/11

    End Sub

    Private Sub NumberOfYearsToBuyChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to remove event handlers during updates
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to ensure Finish button says Cancel after any changes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemovePayPeriodEventHandlers(False) ' TJS 21/03/11
        If Not Me.btnUpdateActivationCost.Enabled Then
            UpdateActivationCostDisplay()
            Me.btnUpdateActivationCost.Enabled = False ' TJS 17/02/12
            Me.WizardControlActivation.EnableNextButton = True ' TJS 17/02/12
        End If
        Me.chkPayMonthly.ErrorText = ""
        Me.chkPayForXYears.ErrorText = ""
        Me.chkCancel.ErrorText = ""
        Me.WizardControlActivation.buttonCancel.Text = "Cancel" ' TJS 02/12/11
        AddRemovePayPeriodEventHandlers(True) ' TJS 21/03/11

    End Sub
#End Region

#Region " chkBillingIsDelivery_CheckedChanged "
    Private Sub SetDeliveryAddress(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBillingIsDelivery.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkBillingIsDelivery.Checked Then
            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingSalutationNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingSalutation = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingSalutation
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingSalutationNull()
            End If
            Me.cbeShippingSalutation.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingSalutation", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingFirstNameNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingFirstName = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingFirstName
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingFirstNameNull()
            End If
            Me.txtShippingFirstName.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingFirstName", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingLastNameNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingLastName = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingLastName
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingLastNameNull()
            End If
            Me.txtShippingLastName.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingLastName", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingCompanyNameNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingCompanyName = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingCompanyName
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingCompanyNameNull()
            End If
            Me.txtShippingCompany.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingCompanyName", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingAddressNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingAddress = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingAddress
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingAddressNull()
            End If
            Me.AddressControlShipping.textAddress.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingAddress", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingCityNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingCity = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingCity
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingCityNull()
            End If
            Me.AddressControlShipping.textCity.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingCity", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingCountyNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingCounty = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingCounty
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingCountyNull()
            End If
            Me.AddressControlShipping.textCounty.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingCounty", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingStateNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingState = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingState
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingStateNull()
            End If
            Me.AddressControlShipping.textState.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingState", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingPostalCodeNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingPostalCode = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingPostalCode
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingPostalCodeNull()
            End If
            Me.AddressControlShipping.comboPostalCode.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingPostalCode", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingCountryNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingCountry = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingCountry
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingCountryNull()
            End If
            Me.AddressControlShipping.comboCountry.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingCountry", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingPhoneNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingPhone = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingPhone
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingPhoneNull()
            End If
            Me.PhoneControlShipping.TextTelephone.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingPhone", "")

            If Not Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).IsBillingPhoneExtensionNull Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShippingPhoneExtension = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BillingPhoneExtension
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetShippingPhoneExtensionNull()
            End If
            Me.PhoneControlShipping.TextExtension.Properties.ReadOnly = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SetColumnError("ShippingPhoneExtension", "")

        Else
            Me.txtShippingFirstName.Properties.ReadOnly = False
            Me.txtShippingLastName.Properties.ReadOnly = False
            Me.txtShippingCompany.Properties.ReadOnly = False
            Me.AddressControlShipping.textAddress.Properties.ReadOnly = False
            Me.AddressControlShipping.textCity.Properties.ReadOnly = False
            Me.AddressControlShipping.textCounty.Properties.ReadOnly = False
            Me.AddressControlShipping.textState.Properties.ReadOnly = False
            Me.AddressControlShipping.comboPostalCode.Properties.ReadOnly = False
            Me.AddressControlShipping.comboCountry.Properties.ReadOnly = False
            Me.PhoneControlShipping.TextTelephone.Properties.ReadOnly = False
            Me.PhoneControlShipping.TextExtension.Properties.ReadOnly = False

        End If
    End Sub
#End Region

#Region " ConnectorSelected "
    Private Sub chk3DCart_CheckedChanged(sender As Object, e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
        If CInt(Me.cbe3DCartCount.EditValue) = 0 And Me.chk3DCart.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ThreeDCartCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart = True
        ElseIf CInt(Me.cbe3DCartCount.EditValue) > 0 And Not Me.chk3DCart.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ThreeDCartCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart = False
        End If
        If Me.chk3DCart.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250
            End If
            EnableDisablePaymentSettings(True)
            Me.cbe3DCartCount.Enabled = True
        Else
            Me.cbe3DCartCount.Enabled = False
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkAmazon_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings and corrected minimum full activation Inventory Import qty
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
        If CInt(Me.cbeAmazonCount.EditValue) = 0 And Me.chkAmazon.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon = True
        ElseIf CInt(Me.cbeAmazonCount.EditValue) > 0 And Not Me.chkAmazon.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon = False
        End If
        If Me.chkAmazon.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False ' TJS 21/03/11
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 21/03/11 TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeAmazonCount.Enabled = True ' TJS 21/03/11
        Else
            Me.cbeAmazonCount.Enabled = False ' TJS 21/03/11
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)
        ' if basic Amazon is disabled then Amazon FBA must be disabled
        If Not Me.chkAmazon.Checked And Me.chkAmazonFBA.Checked Then ' TJS 05/07/12
            Me.chkAmazonFBA.Checked = True ' TJS 05/07/12
            Interprise.Presentation.Base.Message.MessageWindow.Show("Amazon FBA requires the standard Amazon eShopCONNECTOR to be enabled") ' TJS 05/07/12
        End If

    End Sub

    Private Sub chkAmazonFBA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected machine names in checks on Amazon FBS 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        If My.Computer.Name.Substring(0, 5).ToUpper <> "LERHQ" And My.Computer.Name.Substring(0, 9).ToUpper <> "MYPAYE-LT" Then ' TJS 01/05/14
            Interprise.Presentation.Base.Message.MessageWindow.Show("The Amazon FBA connector is still in the final stage of development." & vbCrLf & vbCrLf & "If you would like to join our Beta program and help influence" & vbCrLf & "the connector design, please contact Sales@lerryn.com.", "Please join our Beta program", Interprise.Framework.Base.Shared.MessageWindowButtons.Close)
            Me.chkAmazonFBA.Checked = False
        End If

        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
        If CInt(Me.cbeAmazonFBACount.EditValue) = 0 And Me.chkAmazonFBA.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA = True
        ElseIf CInt(Me.cbeAmazonFBACount.EditValue) > 0 And Not Me.chkAmazonFBA.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA = False
        End If
        If Me.chkAmazonFBA.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeAmazonFBACount.Enabled = True
        Else
            Me.cbeAmazonFBACount.Enabled = False
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)
        ' if Amazon FBA is enabled then basic Amazon must be enabled
        If Not Me.chkAmazon.Checked And Me.chkAmazonFBA.Checked Then
            Me.chkAmazon.Checked = True
            Interprise.Presentation.Base.Message.MessageWindow.Show("Amazon FBA requires the standard Amazon eShopCONNECTOR to be enabled")
        End If

    End Sub

    Private Sub chkASPStorefront_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected minimum full activation Inventory Import qty 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
        If CInt(Me.cbeASPStorefrontCount.EditValue) = 0 And Me.chkASPStorefront.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ASPStorefrontCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront = True
        ElseIf CInt(Me.cbeASPStorefrontCount.EditValue) > 0 And Not Me.chkASPStorefront.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ASPStorefrontCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront = False
        End If
        If Me.chkASPStorefront.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False ' TJS 21/03/11
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 21/03/11 TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeASPStorefrontCount.Enabled = True ' TJS 21/03/11
        Else
            Me.cbeASPStorefrontCount.Enabled = False ' TJS 21/03/11
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkChanAdvisor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected minimum full activation Inventory Import qty 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
        If CInt(Me.cbeChanAdvCount.EditValue) = 0 And Me.chkChanAdvisor.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ChanAdvCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv = True
        ElseIf CInt(Me.cbeChanAdvCount.EditValue) > 0 And Not Me.chkChanAdvisor.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ChanAdvCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv = False
        End If
        If Me.chkChanAdvisor.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False ' TJS 21/03/11
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 21/03/11 TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeChanAdvCount.Enabled = True ' TJS 21/03/11
        Else
            Me.cbeChanAdvCount.Enabled = False ' TJS 21/03/11
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkEBay_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEBay.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected minimum full activation Inventory Import qty 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
        If CInt(Me.cbeEBayCount.EditValue) = 0 And Me.chkEBay.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EBayCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay = True
        ElseIf CInt(Me.cbeEBayCount.EditValue) > 0 And Not Me.chkEBay.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EBayCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay = False
        End If
        If Me.chkEBay.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeEBayCount.Enabled = True
        Else
            Me.cbeEBayCount.Enabled = False
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkMagento_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected minimum full activation Inventory Import qty 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
        If CInt(Me.cbeMagentoCount.EditValue) = 0 And Me.chkMagento.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MagentoCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento = True
        ElseIf CInt(Me.cbeMagentoCount.EditValue) > 0 And Not Me.chkMagento.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MagentoCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento = False
        End If
        If Me.chkMagento.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False ' TJS 21/03/11
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 21/03/11 TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeMagentoCount.Enabled = True ' TJS 21/03/11
        Else
            Me.cbeMagentoCount.Enabled = False ' TJS 21/03/11
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkSearsCom_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected minimum full activation Inventory Import qty 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
        If CInt(Me.cbeSearsComCount.EditValue) = 0 And Me.chkSearsCom.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SearsComCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom = True
        ElseIf CInt(Me.cbeSearsComCount.EditValue) > 0 And Not Me.chkSearsCom.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SearsComCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom = False
        End If
        If Me.chkSearsCom.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeSearsComCount.Enabled = True
        Else
            Me.cbeSearsComCount.Enabled = False
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkShopCom_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected minimum full activation Inventory Import qty 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
        If CInt(Me.cbeShopComCount.EditValue) = 0 And Me.chkShopCom.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShopComCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom = True
        ElseIf CInt(Me.cbeShopComCount.EditValue) > 0 And Not Me.chkShopCom.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShopComCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom = False
        End If
        If Me.chkShopCom.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False ' TJS 21/03/11
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 21/03/11 TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeShopComCount.Enabled = True ' TJS 21/03/11
        Else
            Me.cbeShopComCount.Enabled = False ' TJS 21/03/11
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkVolusion_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 05/07/12 | TJS             | 2012.1.08 | Corrected minimum full activation Inventory Import qty
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
        If CInt(Me.cbeVolusionCount.EditValue) = 0 And Me.chkVolusion.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).VolusionCount = 1
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion = True
        ElseIf CInt(Me.cbeVolusionCount.EditValue) > 0 And Not Me.chkVolusion.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).VolusionCount = 0
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion = False
        End If
        If Me.chkVolusion.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False ' TJS 21/03/11
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 21/03/11 TJS 05/07/12
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
            End If
            EnableDisablePaymentSettings(True)
            Me.cbeVolusionCount.Enabled = True ' TJS 21/03/11
        Else
            Me.cbeVolusionCount.Enabled = False ' TJS 21/03/11
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkFileImport_CheckedChanged(sender As Object, e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/08/13 | TJS             | 2013.2.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
        If Me.chkFileImport.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateFileImport = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250
            End If
            EnableDisablePaymentSettings(True)
        Else
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateFileImport = False
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkProspectLead_CheckedChanged(sender As Object, e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/08/13 | TJS             | 2013.2.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        ClearActivationSelectionErrors()
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
        If Me.chkProspectLead.Checked Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateProspectLead = True
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardOnly = False
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250
            End If
            EnableDisablePaymentSettings(True)
        Else
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateProspectLead = False
        End If
        Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub chkImportWizardOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Corrected enabling of cbeImportWizardQty1 and cbeImportWizardQty2
        '                                        | and modified to force update of dataset
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        If Me.chkImportWizardOnly.Checked Then
            ' save checkbox state before we update other fields otherwise it gets reset
            Me.chkImportWizardOnly.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
            ClearActivationSelectionErrors()
            ClearConnectorSettings()
            ClearActivationCostDisplay()
            EnableDisablePaymentSettings(False)
            Me.btnUpdateActivationCost.Enabled = True
            Me.WizardControlActivation.EnableNextButton = False
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayMonthly = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayForXYears = True ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).NoOfYears = 1 ' TJS 21/03/11
            If Me.chkPurchaseNow.Checked Or (Me.m_ActivationWizardSectionContainerFacade.IsActivated And _
                 Me.m_ActivationWizardSectionContainerFacade.IsFullActivation) Or _
                 (Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry) Or _
                 Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then ' TJS 21/03/11
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500 ' TJS 21/03/11
            Else
                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 ' TJS 21/03/11
            End If
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
            Me.chkPayMonthly.ErrorText = ""
            Me.chkPayForXYears.ErrorText = ""
            Me.ImageComboBoxEditYears.ErrorText = ""
            Me.ImageComboBoxEditYears.Enabled = False ' TJS 21/03/11
            Me.cbeImportWizardQty1.Enabled = True ' TJS 21/03/11
            Me.cbeImportWizardQty2.Enabled = True ' TJS 21/03/11
        Else
            Me.cbeImportWizardQty1.Enabled = False ' TJS 21/03/11
            Me.cbeImportWizardQty2.Enabled = False ' TJS 21/03/11
            Me.ImageComboBoxEditYears.Enabled = True ' TJS 21/03/11
        End If
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbe3DCartCount_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeASPStorefrontCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeMagentoCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeVolusionCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeAmazonCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeAmazonFBACount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)
        ' each Amazon FBA connector requires a standard Amazon connector
        If CInt(Me.cbeAmazonFBACount.EditValue) > CInt(Me.cbeAmazonCount.EditValue) Then
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount = Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount
            Interprise.Presentation.Base.Message.MessageWindow.Show("Amazon FBA requires a standard Amazon eShopCONNECTOR for each FBA connection")
        End If

    End Sub

    Private Sub cbeChanAdvCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeEBayCount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeSearsComCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/01/12 | TJS             | 2011.2.02 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeShopComCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        Me.btnUpdateActivationCost.Enabled = True
        Me.WizardControlActivation.EnableNextButton = False
        ClearActivationCostDisplay()
        AddRemoveConnectorEventHandlers(True)

    End Sub

    Private Sub cbeImportWizardQty1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        InventoryImportQtySet(Me.cbeImportWizardQty1, Me.cbeImportWizardQty2)

    End Sub

    Private Sub cbeImportWizardQty2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to enable Next button after quantity updated 
        '                                        | as InventoryImportQtySet disables it 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        InventoryImportQtySet(Me.cbeImportWizardQty2, Me.cbeImportWizardQty1)
        Me.WizardControlActivation.EnableNextButton = True

    End Sub

    Private Sub chkPurchaseNow_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJs             | 2011.0.02 | Modified to prevent Inventory import quantity being reduced on dataset EndEdit
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to ensure Finish button says Cancel after any changes
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings and corrected minimum full activation Inventory Import qty
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AddRemoveConnectorEventHandlers(False)
        If Me.chkPurchaseNow.Checked And (Me.chkMagento.Checked Or Me.chkASPStorefront.Checked Or Me.chkVolusion.Checked Or _
            Me.chkAmazon.Checked Or Me.chkAmazonFBA.Checked Or Me.chkChanAdvisor.Checked Or Me.chkSearsCom.Checked Or _
            Me.chkShopCom.Checked Or Me.chkFileImport.Checked Or Me.chkProspectLead.Checked) And Not Me.chkCancel.Checked And _
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 Then ' TJS 16/01/12 TJS 05/07/12 TJS 20/08/13
            ' save checkbox state before we update import quantity otherwise it gets reset
            Me.chkPurchaseNow.DataBindings("EditValue").WriteValue()
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
        ElseIf Me.chkPurchaseNow.Checked And Me.chkImportWizardOnly.Checked And Not Me.chkCancel.Checked And _
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 250 Then ' TJS 21/03/11
            ' save checkbox state before we update import quantity otherwise it gets reset
            Me.chkPurchaseNow.DataBindings("EditValue").WriteValue()
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit()
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ImportWizardQty = 2500
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit()
            Me.cbeImportWizardQty2.Enabled = True
        End If
        AddRemoveConnectorEventHandlers(True)

    End Sub
#End Region

#Region " CancelActivation "
    Private Sub CancelActivation(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCancel.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 21/03/11 | TJS             | 2011.0.02 | Modified to force update of dataset
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to ensure Finish button says Cancel after any changes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkCancel.Checked Then ' TJS 21/03/11
            ' save checkbox state before we update import quantity otherwise it gets reset
            Me.chkCancel.DataBindings("EditValue").WriteValue() ' TJS 21/03/11
            ClearConnectorSettings()
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).BeginEdit() ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayMonthly = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).PayForXYears = False ' TJS 21/03/11
            Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EndEdit() ' TJS 21/03/11
            Me.ImageComboBoxEditYears.Enabled = False
            Me.btnUpdateActivationCost.Enabled = False
            Me.WizardControlActivation.EnableNextButton = True
        End If

    End Sub
#End Region

#Region " BillingDetailsChanged "
    Private Sub AddressControlBilling_CountryChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddressControlBilling.CountryChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/01/13 | TJS             | 2013.0.00 | Modified for CB 13 having different label names
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.AddressControlBilling.LayoutItemAddress.Text = Me.AddressControlBilling.LayoutItemAddress.Text.Replace("Billing", "")
        Me.AddressControlBilling.LayoutItemCity.Text = Me.AddressControlBilling.LayoutItemCity.Text.Replace("Billing", "") ' TJS 29/01/13
        Me.AddressControlBilling.LayoutItemState.Text = Me.AddressControlBilling.LayoutItemState.Text.Replace("Billing", "") ' TJS 29/01/13
        Me.AddressControlBilling.LayoutItemPostal.Text = Me.AddressControlBilling.LayoutItemPostal.Text.Replace("Billing", "") ' TJS 29/01/13
        Me.AddressControlBilling.LayoutItemCounty.Text = Me.AddressControlBilling.LayoutItemCounty.Text.Replace("Billing", "")
        Me.AddressControlBilling.LayoutItemCountry.Text = Me.AddressControlBilling.LayoutItemCountry.Text.Replace("Billing", "")

    End Sub

    Private Sub AddressControlBilling_CountryCodePopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles AddressControlBilling.CountryCodePopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/01/13 | TJS             | 2013.0.00 | Modified for CB 13 having different label names
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.AddressControlBilling.LayoutItemAddress.Text = Me.AddressControlBilling.LayoutItemAddress.Text.Replace("Billing", "")
        Me.AddressControlBilling.LayoutItemCity.Text = Me.AddressControlBilling.LayoutItemCity.Text.Replace("Billing", "") ' TJS 29/01/13
        Me.AddressControlBilling.LayoutItemState.Text = Me.AddressControlBilling.LayoutItemState.Text.Replace("Billing", "") ' TJS 29/01/13
        Me.AddressControlBilling.LayoutItemPostal.Text = Me.AddressControlBilling.LayoutItemPostal.Text.Replace("Billing", "") ' TJS 29/01/13
        Me.AddressControlBilling.LayoutItemCounty.Text = Me.AddressControlBilling.LayoutItemCounty.Text.Replace("Billing", "")
        Me.AddressControlBilling.LayoutItemCountry.Text = Me.AddressControlBilling.LayoutItemCountry.Text.Replace("Billing", "")

    End Sub
#End Region

#Region " ShippingDetailsChanged "
    Private Sub AddressControlShipping_CountryChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddressControlShipping.CountryChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/01/13 | TJS             | 2013.0.00 | Modified for CB 13 having different label names
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.AddressControlShipping.LayoutItemAddress.Text = Me.AddressControlShipping.LayoutItemAddress.Text.Replace("Shipping", "")
        Me.AddressControlShipping.LayoutItemCity.Text = Me.AddressControlShipping.LayoutItemCity.Text.Replace("Shipping", "") ' TJS 29/01/13
        Me.AddressControlShipping.LayoutItemState.Text = Me.AddressControlShipping.LayoutItemState.Text.Replace("Shipping", "") ' TJS 29/01/13
        Me.AddressControlShipping.LayoutItemPostal.Text = Me.AddressControlShipping.LayoutItemPostal.Text.Replace("Shipping", "") ' TJS 29/01/13
        Me.AddressControlShipping.LayoutItemCounty.Text = Me.AddressControlShipping.LayoutItemCounty.Text.Replace("Shipping", "")
        Me.AddressControlShipping.LayoutItemCountry.Text = Me.AddressControlShipping.LayoutItemCountry.Text.Replace("Shipping", "")

    End Sub

    Private Sub AddressControlShipping_CountryCodePopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles AddressControlShipping.CountryCodePopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/01/13 | TJS             | 2013.0.00 | Modified for CB 13 having different label names
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.AddressControlShipping.LayoutItemAddress.Text = Me.AddressControlShipping.LayoutItemAddress.Text.Replace("Shipping", "")
        Me.AddressControlShipping.LayoutItemCity.Text = Me.AddressControlShipping.LayoutItemCity.Text.Replace("Shipping", "") ' TJS 29/01/13
        Me.AddressControlShipping.LayoutItemState.Text = Me.AddressControlShipping.LayoutItemState.Text.Replace("Shipping", "") ' TJS 29/01/13
        Me.AddressControlShipping.LayoutItemPostal.Text = Me.AddressControlShipping.LayoutItemPostal.Text.Replace("Shipping", "") ' TJS 29/01/13
        Me.AddressControlShipping.LayoutItemCounty.Text = Me.AddressControlShipping.LayoutItemCounty.Text.Replace("Shipping", "")
        Me.AddressControlShipping.LayoutItemCountry.Text = Me.AddressControlShipping.LayoutItemCountry.Text.Replace("Shipping", "")

    End Sub
#End Region

#Region " GetActivationCost "
    Private Sub GetActivationCost(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateActivationCost.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to prevent error if no response from our server
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strReturn As String

        If Me.chkPayMonthly.Checked Or (Me.chkPayForXYears.Checked And Me.ImageComboBoxEditYears.SelectedIndex >= 0) Then
            strReturn = Me.m_ActivationWizardSectionContainerFacade.GetActivationCost(False)
            If strReturn <> "" Then ' TJS 22/08/13
                XMLActivationCost = XDocument.Parse(strReturn) ' TJS 02/12/11
            Else
                XMLActivationCost = New XDocument ' TJS 22/08/13
            End If
            If XMLActivationCost.ToString <> "" Then ' TJS 22/08/13
                UpdateActivationCostDisplay()
                Me.btnUpdateActivationCost.Enabled = False ' TJS 17/02/12
                Me.WizardControlActivation.EnableNextButton = True ' TJS 17/02/12
            End If

        ElseIf Me.chkPayForXYears.Checked And Me.ImageComboBoxEditYears.SelectedIndex < 0 Then
            Me.ImageComboBoxEditYears.ErrorText = "You must select the number of years to purchase"
        Else
            Me.chkPayMonthly.ErrorText = "You must select a payment period"
            Me.chkPayForXYears.ErrorText = "You must select a payment period"
        End If

    End Sub
#End Region

#Region " SetActivationType "
    Private Function SetActivationType() As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/01/13 | TJS             | 2012.1.17 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' is purchase now set ?
        If Me.chkPurchaseNow.Checked Then
            ' yes, is product activated (ignore if has been activated as this doesn't affect whether we need a base module activation) ?
            If Not Me.m_ActivationWizardSectionContainerFacade.IsActivated Then
                ' no, set initial purchase
                Return "Initial"

            ElseIf Not Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                ' activation is evaluation only, set initial purchase
                Return "Initial"
            Else
                ' purchase now set and is activated, set updated purchase
                Return "Update"
            End If
        Else
            ' is this the first activation i.e. not activated and has not been activated ?
            If Not Me.m_ActivationWizardSectionContainerFacade.IsActivated And Not Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                ' yes
                Return "Eval"

            ElseIf Me.m_ActivationWizardSectionContainerFacade.IsActivated And Not bNearExpiry And _
                Not Me.m_ActivationWizardSectionContainerFacade.IsFullActivation Then
                ' currently activated on evaluation and not near expiry
                Return "EvalUpdate"

            ElseIf Me.m_ActivationWizardSectionContainerFacade.IsActivated And bNearExpiry Then
                ' currently activated on evaluation and near expiry
                Return "Renew"

            ElseIf Me.m_ActivationWizardSectionContainerFacade.HasBeenActivated Then
                ' previous activation has expired, treat as initial activation
                Return "Initial"

            Else
                Return "Update"

            End If
        End If

    End Function
#End Region

#Region " SetRequestActivationMessage "
    Private Sub SetRequestActivationMessage()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/01/13 | TJS             | 2012.1.17 | Function added
        ' 22/03/13 | TJS             | 2013.1.05 | Clarified message regarding PayPal invoice and loading codes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strActivationType As String

        strActivationType = SetActivationType()
        If strActivationType = "Eval" Then
            Me.lblRequestActivation.Text = "Click Next to initiate the Activation process which will create your Evaluation Activation."
        ElseIf strActivationType = "EvalUpdate" Then
            Me.lblRequestActivation.Text = "Click Next to initiate the Activation process which will create your updated Evaluation Activation."
        Else
            Me.lblRequestActivation.Text = "Click Next to initiate the Activation process which will cause a PayPal invoice to be sent to your Billing email address." & vbCrLf & "Once this invoice is paid, Activation Code(s) will be issued automatically and you can use this Activation Wizard again to import the Activation Code(s)." ' TJS 22/03/13
        End If

    End Sub
#End Region

#Region " EnableEmailedCodeEntry "
    Private Sub EnableEmailedCodeEntry(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnterManualCode.CheckedChanged

        If Me.chkEnterManualCode.Checked Then
            Me.txtManualActivation.Visible = True
            Me.btnUpdateActivationCode.Visible = True
            Me.lblManualActivation.Visible = True
        Else
            Me.txtManualActivation.Visible = False
            Me.btnUpdateActivationCode.Visible = False
            Me.lblManualActivation.Visible = False
        End If
    End Sub

    Private Sub UpdateActivationCode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateActivationCode.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/04/11 | TJS             | 2011.0.11 | Modified to call UpdateConnectorMaxAccounts
        ' 02/12/11 | TJS             | 2011.2.00 | Modified call to UpdateConnectorMaxAccounts to prevent 
        '                                        | attempt to update ConfigDataset which is not loaded
        '                                        | and to cater for eBay
        ' 16/01/12 | TJS             | 2011.2.02 | Added Sears.com connector settings
        ' 10/05/12 | TJS             | 2012.1.05 | Modified to save updated config settings after connector count updated
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strUpdatedCode As String = "", strConnectorCode As String, strPaymentPeriod As String
        Dim iloop As Integer, iErrorCode As Integer, bCodeValid As Boolean

        ' has Activation Code been entered ?
        If Me.btnUpdateActivationCode.Text.Length > 0 Then
            ' yes, validate it
            bCodeValid = False
            If Me.chkPayMonthly.Checked Then
                strPaymentPeriod = "M"
            Else
                If Me.ImageComboBoxEditYears.SelectedIndex >= 0 Then
                    If Me.ImageComboBoxEditYears.EditValue.ToString = "1" Then
                        strPaymentPeriod = "1Y"

                    ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "2" Then
                        strPaymentPeriod = "2Y"

                    ElseIf Me.ImageComboBoxEditYears.EditValue.ToString = "3" Then
                        strPaymentPeriod = "3Y"

                    Else
                        strPaymentPeriod = "1Y"
                    End If
                Else
                    strPaymentPeriod = "1Y"
                End If
            End If
            If Me.m_ActivationWizardSectionContainerFacade.UpdateDisplayedActivation(PRODUCT_CODE, Me.txtManualActivation.Text.Replace("-", ""), _
                dteEndFreeTrial, dteNextInvoiceDue, strPaymentPeriod, iErrorCode, strUpdatedCode) Then
                ' licence code valid, update displays
                Me.chkEnterManualCode.Checked = False
                Me.txtManualActivation.Text = ""
                InitialiseControls()
                Interprise.Presentation.Base.Message.MessageWindow.Show("Your Activation Code has been entered successfully.")
                bCodeValid = True

            ElseIf iErrorCode = Licence.ErrorCodes.WrongProductCode Then
                ' only check for connectors if error was wrong product code
                For iloop = 1 To 12 ' TJs 16/01/12 TJS 05/07/12 TJS 20/11/13
                    Select Case iloop
                        Case 1 ' ASPDotNetStorefront
                            strConnectorCode = ASP_STORE_FRONT_CONNECTOR_CODE
                        Case 2 ' Magento
                            strConnectorCode = MAGENTO_CONNECTOR_CODE
                        Case 3 ' Volusion
                            strConnectorCode = VOLUSION_CONNECTOR_CODE
                        Case 4 ' Amazon
                            strConnectorCode = AMAZON_SELLER_CENTRAL_CONNECTOR_CODE
                        Case 5 ' Channel Advisor
                            strConnectorCode = CHANNEL_ADVISOR_CONNECTOR_CODE
                        Case 6 ' Shop.com
                            strConnectorCode = SHOP_DOT_COM_CONNECTOR_CODE
                        Case 7 ' File Import 
                            strConnectorCode = ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE
                        Case 8 ' Prospect/Lead Import
                            strConnectorCode = PROSPECT_IMPORT_CONNECTOR_CODE
                        Case 9 ' eBay TJS 02/12/11
                            strConnectorCode = EBAY_CONNECTOR_CODE ' TJS 02/12/11
                        Case 10 ' Sears.com TJS 16/01/12
                            strConnectorCode = SEARS_DOT_COM_CONNECTOR_CODE ' TJS 16/01/12
                        Case 11 ' Amazom FBA TJS 05/07/12
                            strConnectorCode = AMAZON_FBA_CONNECTOR_CODE ' TJS 05/07/12
                        Case 12 ' 3DCart TJS 20/11/13
                            strConnectorCode = THREE_D_CART_CONNECTOR_CODE ' TJS 20/11/13
                    End Select
                    If Me.m_ActivationWizardSectionContainerFacade.UpdateDisplayedActivation(strConnectorCode, Me.txtManualActivation.Text.Replace("-", ""), _
                        dteEndFreeTrial, dteNextInvoiceDue, strPaymentPeriod, iErrorCode, strUpdatedCode) Then
                        ' licence code valid, update displays
                        Me.chkEnterManualCode.Checked = False
                        Me.txtManualActivation.Text = ""
                        InitialiseControls()
                        bCodeValid = True
                        Select Case iloop
                            Case 1 ' ASPDotNetStorefront
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateASPStorefront = True
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ASPStorefrontCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE)
                            Case 2 ' Magento
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateMagento = True
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).MagentoCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(MAGENTO_CONNECTOR_CODE)
                            Case 3 ' Volusion
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateVolusion = True
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).VolusionCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(VOLUSION_CONNECTOR_CODE)
                            Case 4 ' Amazon
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazon = True
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE)
                            Case 5 ' Channel Advisor
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateChanAdv = True
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ChanAdvCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE)
                            Case 6 ' Shop.com
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateShopCom = True
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ShopComCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE)
                            Case 7 ' File Import TJS 20/08/13
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateFileImport = True ' TJS 20/08/13
                            Case 8 ' Prospect and Lead TJS 20/08/13
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateProspectLead = True ' TJS 20/08/13
                            Case 9 ' eBay TJS 02/12/11 TJS 16/01/12
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateEBay = True ' TJS 02/12/11
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).EBayCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(EBAY_CONNECTOR_CODE) ' TJS 02/12/11
                            Case 10 ' Sears.com TJS 16/01/12
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateSearsCom = True ' TJS 16/01/12
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).SearsComCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(SEARS_DOT_COM_CONNECTOR_CODE) ' TJS 16/01/12
                            Case 11 ' Amazom FBA TJS 05/07/12
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ActivateAmazonFBA = True ' TJS 05/07/12
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).AmazonFBACount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(AMAZON_FBA_CONNECTOR_CODE) ' TJS 05/07/12
                            Case 12 ' 3DCart TJS 20/11/13
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).Activate3DCart = True ' TJS 20/11/13
                                Me.ActivationWizardSectionContainerGateway.ActivationAccountDetails(0).ThreeDCartCount = Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(THREE_D_CART_CONNECTOR_CODE) ' TJS 20/11/13
                            Case Else
                                ' ignore
                        End Select
                        If Not Me.m_ActivationWizardSectionContainerFacade.UpdateConnectorMaxAccounts(strConnectorCode, False) Then ' TJS 18/04/11 TJS 02/12/11
                            Interprise.Presentation.Base.Message.MessageWindow.Show("Your Activation Code only permits a maximum of " & _
                                Me.m_ActivationWizardSectionContainerFacade.ConnectorAccountLimit(strConnectorCode) & _
                                " accounts and the config settings for one or more additional accounts have been removed.") ' TJS 18/04/11
                        Else
                            Interprise.Presentation.Base.Message.MessageWindow.Show("Your Activation Code has been entered successfully.")
                        End If
                        Me.UpdateDataSet(New String()() {New String() {Me.ActivationWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221.TableName, _
                              "", "UpdateLerrynImportExportConfig_DEV000221", ""}}, False, False, False) 'TJS 10/05/12
                        Exit For

                    ElseIf iErrorCode <> Licence.ErrorCodes.WrongProductCode Then
                        ' exit loop if error wasn't wrong product code
                        Exit For
                    End If
                Next
            End If
            If Not bCodeValid Then
                ' licence code invalid 
                Interprise.Presentation.Base.Message.MessageWindow.Show("This code is invalid. Please check you have entered it correctly, otherwise please contact" & vbCrLf & "Support@lerryn.com quoting your System ID (" & Me.m_ActivationWizardSectionContainerFacade.GetSystemLicenceID(False) & vbCrLf & "), your Activation Code and Error No " & iErrorCode.ToString)
            End If
        Else
            Me.txtManualActivation.ErrorText = "Must not be blank"
        End If
    End Sub

    Private Sub TextEditManualActivation_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtManualActivation.EditValueChanging

        Me.txtManualActivation.ErrorText = ""

    End Sub
#End Region

#Region " GotoPluginsWebSite "
    Private Sub GotoPluginsWebSite(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPluginsURL.Click

        System.Diagnostics.Process.Start(Me.lblPluginsURL.Text) ' TJS 13/04/11

    End Sub
#End Region
#End Region
End Class
#End Region
