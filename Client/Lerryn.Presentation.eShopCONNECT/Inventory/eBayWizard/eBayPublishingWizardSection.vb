' eShopCONNECT for Connected Business
' Module: eBayPublishingWizardSection.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
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
' Last Updated - 26 June 2013

Option Explicit On
Option Strict On

Imports System.Xml
Imports System.Xml.Serialization

Imports System.Xml.Linq
Imports System.Xml.XPath
Imports System.IO
Imports System.Xml.Schema
Imports System.Collections
Imports DevExpress.XtraEditors.Controls
Imports Lerryn.Facade.ImportExport.eBayXMLConnector
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Facade.ImportExport

Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Globalization
Imports System.Net
Imports System.Web

#Region " eBayPublishingWizardSection "
Namespace eBayWizard
    <Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.eBayWizard.eBayPublishingWizardSection")> _
    Public Class eBayPublishingWizardSection

#Region " Variables "

        Public m_bIgnoreChange As Boolean

        Public bManualCategories As Boolean
        Public bGotInitialCategories As Boolean
        Public sLastSearch As String = ""

        Public iMaxDepth As Integer = 8

        Public iSelectedCategoryDepth As Integer
        Public Const AuctionTypeNormal As Integer = 0
        Public Const AuctionTypeStockQuantity As Integer = 1
        Public Const AuctionTypeMultiple As Integer = 2

        Private WithEvents btnBack As DevExpress.XtraEditors.SimpleButton
        Private WithEvents btnNext As DevExpress.XtraEditors.SimpleButton

        Private m_eBayPublishingWizardSection As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Private m_eBayPublishingWizardSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
        Private m_IgnorePageChangeEvent As Boolean

        Public Property oeBayWrapper As Lerryn.Facade.ImportExport.eBayXMLConnector = Nothing
        Public Property oEbaySettings As Lerryn.Framework.ImportExport.SourceConfig.eBaySettings = Nothing
        Private m_EBayCurrency As String
        Private m_EBayCurrencySymbol As String
        Private m_EBayCountryString As String

        Private m_UseEBaySandbox As Boolean = False

        Public m_WizardDirection As String
        Public m_AuctionType As Integer

        Public m_Direction As String

        Public Property m_InterpriseStockItem As InterpriseStockItem

#End Region

#Region " Properties "
#Region " CurrentDataset "
        Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
            Get
                Return Me.eBayPublishingWizardSectionGateway
            End Get
        End Property
#End Region

#Region " CurrentFacade "
        Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
            Get
                Return Me.m_eBayPublishingWizardSectionFacade

            End Get
        End Property
#End Region

        ' This holds the category id's for the CatalogEnabled categories
        Private poForceToNext As New DevExpress.XtraTab.XtraTabPage
        Public Property oForceToNext As DevExpress.XtraTab.XtraTabPage
            Get
                Return poForceToNext
            End Get
            Set(poForceToNext As DevExpress.XtraTab.XtraTabPage)
            End Set
        End Property

        ' This keeps track of which tab we are using when viewing Catalog enabled categories
        Private pCatalogEnabledCategorysTab As New Integer
        Public Property CatalogEnabledCategorysTab As Integer
            Get
                Return pCatalogEnabledCategorysTab
            End Get
            Set(pCatalogEnabledCategorysTab As Integer)
            End Set
        End Property

        ' This holds the category id's for the CatalogEnabled categories
        Private pCatalogEnabledCategorys As New ArrayList
        Public Property CatalogEnabledCategorys As ArrayList
            Get
                Return pCatalogEnabledCategorys
            End Get
            Set(pCatalogEnabledCategorys As ArrayList)
            End Set
        End Property

        ' This holds the category ID for the category being viewed / edited
        Private pCatalogEnabledCategoryBeingViewed As New ArrayList
        Public Property CatalogEnabledCategoryBeingViewed As ArrayList
            Get
                Return pCatalogEnabledCategoryBeingViewed
            End Get
            Set(pCatalogEnabledCategoryBeingViewed As ArrayList)
            End Set
        End Property

        Private paCategoriesToAddItemTo As New ArrayList
        Public Property aCategoriesToAddItemTo() As ArrayList ' Array of eBay categories the item is being added to
            Get
                Return paCategoriesToAddItemTo
            End Get
            Set(ByVal value As ArrayList)
                paCategoriesToAddItemTo = value
            End Set
        End Property

        Public Property m_SuggestedCategories As SuggestedCategories()
        Public Structure SuggestedCategories
            Public CategoryName As String
            Public CategoryID As Integer
            Public PercentItemFound As Integer
            'Public CategoryParent As CategoryParentID
        End Structure

        Public Property m_CategoryParentID As CategoryParentID()
        Public Structure CategoryParentID
            Public CategoryID As Integer
            Public CategoryParentID As Integer
            Public CategoryParentDescription As String
        End Structure

        Public Property m_ShipmentPostalDesination As ShipmentPostalDesination()
        Public Structure ShipmentPostalDesination
            Public ShippingCarrierID As String
            Public Description As String
            Public DetailVersion As String
            Public UpdateTime As String
        End Structure

        Public Property m_geteBayDetails As GeteBayDetails()
        Public Structure GeteBayDetails
            Public Request As String
        End Structure

        Public Property m_ShippingServiceDetails As ShippingServiceDetails()
        Public Structure ShippingServiceDetails
            Public Description As String
            Public InternationalService As String
            Public ShippingService As String
            Public ShippingServiceID As Integer
            Public ServiceType As ServiceType()
            Public ValidForSellingFlow As String
            Public DetailVersion As Integer
            Public UpdateTime As String
            Public ShippingCategory As String
        End Structure

        Public Structure ServiceType
            Public Type As String
        End Structure

        Public Property m_DispatchTimeMaxDetails As DispatchTimeMaxDetails()
        Public Structure DispatchTimeMaxDetails
            Public DispatchTimeMax As String
            Public Description As String
            Public DetailVersion As Integer
            Public UpdateTime As String
        End Structure

        Structure ComboSortedItems
            Public Description As String
            Public iURN As Integer
            Public sURN As String
        End Structure

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
            Me.m_eBayPublishingWizardSection = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            Me.m_eBayPublishingWizardSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_eBayPublishingWizardSection, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            Me.InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            m_IgnorePageChangeEvent = False
        End Sub

        Public Sub New(ByVal InventoryEBaySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
            ByVal InventoryEBaySettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
            MyBase.New()

            'This call is required by the Windows Form Designer.

            m_eBayPublishingWizardSection = InventoryEBaySettingsDataset
            Me.m_eBayPublishingWizardSectionFacade = InventoryEBaySettingsSectionFacade
            Me.InitializeComponent()

        End Sub
#End Region

#Region " InitialiseControls "
        Public Sub InitialiseControls()

            Dim strErrorDetails As String

            m_UseEBaySandbox = (m_eBayPublishingWizardSectionFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "UseEBaySandbox", "NO").ToUpper = "YES")
            Me.m_eBayPublishingWizardSectionFacade.GetEBayCategories(DirectCast(Me.ParentForm, eBayPublishingWizardForm).eBayXMLConfig, m_UseEBaySandbox, strErrorDetails)

            txtDescriptiveTitle.Text = DirectCast(Me.ParentForm, eBayPublishingWizardForm).DescriptionToPublish   ' Pull in the passsed in description

            ' Pull in multiple parameters from the calling form about the selected stock item
            m_InterpriseStockItem = DirectCast(Me.ParentForm, eBayPublishingWizardForm).InterpriseStockItem

            ' We need to pull in more info about the selected stock item

            ' Instantiate the Lerryn eBay wrapper
            oEbaySettings = New Lerryn.Framework.ImportExport.SourceConfig.eBaySettings

            With oEbaySettings
                .eBayCountry = DirectCast(Me.ParentForm, eBayPublishingWizardForm).eBayCountryID
                .SiteID = CStr(.eBayCountry)
                .AuthToken = DirectCast(Me.ParentForm, eBayPublishingWizardForm).eBayAuthToken
            End With
            m_EBayCurrency = ""
            oeBayWrapper = New Lerryn.Facade.ImportExport.eBayXMLConnector(m_eBayPublishingWizardSectionFacade, oEbaySettings, True)

            ' Pull in eBay site details
            oeBayWrapper.GetSiteDetails()

            ' Pull in eBay countries
            oeBayWrapper.GeteBayCountries()

            ' Pull in eBay currencies
            oeBayWrapper.GetEbayCurrencies()

            ' Pull in shipping services
            oeBayWrapper.GetShippingServiceDetails()

            ' Pull in shipping locations
            oeBayWrapper.GetShippingServiceDetails()

            ' Get the incoming ebay site id -site to publish to id- "3" and get the site string "UK" and populate the ebay settings properties
            Dim OneSite As SiteDetails
            OneSite = oeBayWrapper.LookupSiteDetails(CInt(oEbaySettings.SiteID))

            btnBack = WizardControlEBayPub.buttonBack
            btnNext = WizardControlEBayPub.buttonNext

            lblHTMLTemplateFilePath.Text = ""
            ' Derive currency from ebay site code 
            Select Case oEbaySettings.eBayCountry
                Case 0  ' US
                    m_EBayCurrency = "USD"
                    m_EBayCountryString = "US"
                Case 2 ' Canada
                    m_EBayCurrency = "CAD"
                    m_EBayCountryString = "CA"
                Case 3 ' UK
                    m_EBayCurrency = "GBP"
                    m_EBayCountryString = "GB"
                Case 77 ' Germany
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "DE"
                Case 15 ' Australia
                    m_EBayCurrency = "AUD"
                    m_EBayCountryString = "AU"
                Case 71 ' France
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "FR"
                Case 100 ' eBayMotors
                    m_EBayCurrency = "USD"
                    m_EBayCountryString = ""
                Case 101 ' Italy
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "IT"
                Case 146 ' Netherlands
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "NL"
                Case 186 ' Spain
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "SE"
                Case 203 ' India
                    m_EBayCurrency = "INR"
                    m_EBayCountryString = "IN"
                Case 201 ' HongKong
                    m_EBayCurrency = "HKD"
                    m_EBayCountryString = "HK"
                Case 216 ' Singapore
                    m_EBayCurrency = "SGD"
                    m_EBayCountryString = "SG"
                Case 207 ' Malaysia
                    m_EBayCurrency = "MYR"
                    m_EBayCountryString = "MY"
                Case 211 ' Philippines
                    m_EBayCurrency = "PHP"
                    m_EBayCountryString = "PH"
                Case 210 ' CanadaFrench
                    m_EBayCurrency = "CAD"
                    m_EBayCountryString = "CA"
                Case 212 ' Poland
                    m_EBayCurrency = "PLN"
                    m_EBayCountryString = "PL"
                Case 123 'Belgium_Dutch
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "BE"
                Case 23 ' Belgium_French
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "BE"
                Case 16 ' Austria
                    m_EBayCurrency = "EUR"
                    m_EBayCountryString = "AT"
                Case 193 ' Switzerland
                    m_EBayCurrency = "CHF"
                    m_EBayCountryString = "CH"
                Case 205 ' Ireland
                    m_EBayCurrency = "EUR" '
                    m_EBayCountryString = "IE"
                Case Else
                    m_EBayCurrency = ""
                    m_EBayCountryString = ""
            End Select
            m_EBayCurrencySymbol = Me.m_eBayPublishingWizardSectionFacade.GetField("Symbol", "SystemCurrency", "CurrencyCode = '" & m_EBayCurrency & "'")

            oEbaySettings.SiteID = CStr(3)

            oeBayWrapper.LookupCountry(m_EBayCountryString)
            oeBayWrapper.LookupCurrency(m_EBayCurrency)

            oeBayWrapper.GetReturnsPolicy()

            ' Setup the returns accepted policy combo based upon the web service details
            Dim OneReturnsPolicy As clsReturnPolicyDetails
            Dim OneReturn As ReturnPolicyDetailsReturnsAccepted
            cmbReturnsPolicy.Properties.Items.Clear()
            For Each OneReturnsPolicy In oeBayWrapper.aReturnsPolicy
                For Each OneReturn In OneReturnsPolicy.ReturnsAccepted
                    With cmbReturnsPolicy
                        .Properties.Items.Add(New ImageComboBoxItem(OneReturn.Description, OneReturn.LerrynID))
                    End With
                Next
            Next

            ' Populate the refund options combbo based upon the web service details
            Dim OneRefundOption As ReturnPolicyDetailsRefund
            cmbRefundOptions.Properties.Items.Clear()
            For Each OneReturnsPolicy In oeBayWrapper.aReturnsPolicy
                For Each OneRefundOption In OneReturnsPolicy.Refund
                    With cmbRefundOptions
                        .Properties.Items.Add(New ImageComboBoxItem(OneRefundOption.Description, OneRefundOption.LerrynID))
                    End With
                Next
            Next

            ' Populate cmbShippingCostsPaidBy
            Dim OneShippingCosts As ReturnPolicyDetailsShippingCostPaidBy
            cmbShippingCostsPaidBy.Properties.Items.Clear()
            For Each OneReturnsPolicy In oeBayWrapper.aReturnsPolicy
                For Each OneShippingCosts In OneReturnsPolicy.ShippingCostPaidBy
                    With cmbShippingCostsPaidBy
                        .Properties.Items.Add(New ImageComboBoxItem(OneShippingCosts.Description, OneShippingCosts.LerrynID))
                    End With
                Next
            Next

            ' Populate cmbPostalDestination
            Dim clsShippingLocationDetails As clsShippingLocationDetails
            Dim OneShippingLocationDetails As ShippingLocationDetails
            cmbPostalDestination.Properties.Items.Clear()
            For Each clsShippingLocationDetails In oeBayWrapper.aShippingLocationDetails
                For Each OneShippingLocationDetails In clsShippingLocationDetails.ShippingLocationDetails
                    With cmbPostalDestination
                        .Properties.Items.Add(New ImageComboBoxItem(OneShippingLocationDetails.Description, OneShippingLocationDetails.LerrynID))
                    End With
                Next
            Next

            ' Populate ReturnsWithin
            Dim OneReturnsWithin As ReturnPolicyDetailsReturnsWithin
            cmbReturnWithin.Properties.Items.Clear()
            For Each OneReturnsPolicy In oeBayWrapper.aReturnsPolicy
                For Each OneReturnsWithin In OneReturnsPolicy.ReturnsWithin
                    With cmbReturnWithin
                        .Properties.Items.Add(New ImageComboBoxItem(OneReturnsWithin.Description, OneReturnsWithin.LerrynID))
                    End With
                Next
            Next

            ' Populate the form with the currency symbol
            lblCurrency1.Text = m_EBayCurrencySymbol
            lblCurrency2.Text = m_EBayCurrencySymbol
            lblCurrency3.Text = m_EBayCurrencySymbol

            ' Setup the postal destinations
            'With cmbPostalDestination_old.Properties.Items
            '    .Clear()
            '    .Add("United Kingdom")
            '    .Add("European Union")
            '    .Add("Worldwide")
            'End With


            ' get retail selling price
            'Dim strSQL As String
            'Dim strTemp As String

            'strSQL = "SELECT RetailPrice FROM dbo.InventoryItemPricingDetailView WHERE CurrencyCode = '" & GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
            'strSQL = strSQL & "' AND ItemCode = '" & InterpriseStockItem.ItemCode & "'"
            'strTemp = GetField(strSQL, CStr(CommandType.Text), Nothing)
            '' did we find a price ?
            'If strTemp <> "" Then
            '    InterpriseStockItem.RetailPrice = CDec(strTemp)
            'End If

            ' Pull in info from the interprise stock structure

            With m_InterpriseStockItem
                txtStartAuction.Text = CStr(.RetailPrice)
            End With
            rdoAuctionType.SelectedIndex = 1
            'rdoAuctionQuantity_SelectedIndexChanged(Me, Nothing)
            m_WizardDirection = "NONE"

            ' Now get the categories from the DB into our memory structures
            Me.GetCategoriesFromDatabase()

            CatalogEnabledCategorysTab = 0
        End Sub
#End Region

#Region " InitializeControl "
        Protected Overrides Sub InitializeControl()
            'This call is required by the Presentation Layer.
            MyBase.InitializeControl()

            'Add any initialization after the InitializeControl() call

        End Sub
#End Region
#End Region

#Region " Events "
        '        Private Sub wizardControl1_SelectedPageChanging(ByVal sender As Object, ByVal e As
        'DevExpress.XtraWizard.WizardPageChangingEventArgs) Handles wizardControl1.SelectedPageChanging
        '            If e.Page Is wizardControl1.Pages(1) AndAlso checkEdit1.Checked Then
        '                Dim nextPageIndex As Integer
        '                If (e.Direction = DevExpress.XtraWizard.Direction.Forward) Then
        '                    nextPageIndex = (1) + wizardControl1.Pages.IndexOf(e.Page)
        '                Else
        '                    nextPageIndex = (-1) + wizardControl1.Pages.IndexOf(e.Page)
        '                End If
        '                e.Page = wizardControl1.Pages(nextPageIndex)
        '            End If
        '        End Sub
        Private Sub WizardControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles WizardControlEBayPub.SelectedPageChanged
            Dim Direction As String = m_Direction
            Dim sCurrentName As String = e.Page.Name
            Dim sPreviousName As String = e.PrevPage.Name
            Dim bForcePageChange As Boolean = False
            Dim oForceTo As DevExpress.XtraTab.XtraTabPage = Nothing
            Dim OneCategory As eBayCategory
            Dim dDec As Decimal

            Dim iNoCatalogEnabled As Integer
            Dim iNoCatalogPropertiesSelected As Integer
            Dim bCatalogEnabled As Boolean = False

            Dim iCategoryBeingViewed As Integer, iCategoryToView As Integer, iCategoryToViewNextPosition As Integer

            If Not m_IgnorePageChangeEvent Then

                Select Case e.Page.Name

                    Case TabPageSelectCategory.Name
                        If Direction = "NEXT" Then
                            ' Going forward on wizard
                            With rdoAuctionType
                                If .SelectedIndex < 0 Then
                                    .ErrorText = "Must select an Auction Type"
                                    bForcePageChange = True
                                    oForceTo = Me.TabPageWelcome
                                End If
                            End With

                            With txtDescriptiveTitle
                                If .Text.Trim = "" Then
                                    .ErrorText = "Must input a description"
                                    bForcePageChange = True
                                    oForceTo = Me.TabPageWelcome
                                End If
                            End With

                            ' We only do this if the text has changed
                            ' Move the text over from the previous box if the destination is empty
                            If Not bForcePageChange Then
                                If txtSuggestedCategories.Text = "" Then
                                    txtSuggestedCategories.Text = txtDescriptiveTitle.Text
                                End If

                                If txtDescriptiveTitle.Text.ToUpper.Trim <> sLastSearch.ToUpper.Trim Then
                                    Application.DoEvents()
                                    oeBayWrapper.aCategoriesSelected.Clear()
                                    UpdateBreadcrumbs()
                                    bGotInitialCategories = True
                                    bManualCategories = False

                                    iSelectedCategoryDepth = 0
                                    Me.SetupSelectedCategory(txtDescriptiveTitle.Text)
                                    Application.DoEvents()
                                End If
                            End If
                        End If

                    Case TabPageAddPictures.Name
                        If Direction = "NEXT" Then
                            ' Going forward
                            ' Validate the previous form controls

                            'imgListeBayCategoriesToAdd doesn't have an error control!
                            imgListeBayCategoriesToAdd_ToDefaults()
                            With imgListeBayCategoriesToAdd
                                If .ItemCount = 0 Then
                                    UpdateStatus("A category must be selected", True)
                                    bForcePageChange = True
                                    oForceTo = Me.TabPageSelectCategory
                                    .BorderStyle = BorderStyles.Simple
                                    .Appearance.BorderColor = Color.Red
                                End If
                            End With

                            ' Pull in and parse the XML for the categories (so we can load the combos etc with category specific info)
                            GetCategoryFeaturesForAll()

                            oeBayWrapper.aConditionValues.Clear()
                            For Each OneCategory In aCategoriesToAddItemTo
                                oeBayWrapper.GetCategoryFeaturesFromXML(OneCategory.GetCategoryFeaturesXML, OneCategory.CategoryID)
                            Next

                            ' Work through the selected categories ensuring they all have a condition if requried
                            For Each OneCategory In aCategoriesToAddItemTo
                                If OneCategory.NumberOfConditionsForCategory > 0 And OneCategory.SelectedConditionID = 0 Then
                                    ' This category has conditions applied to it and none are selected
                                    cmbCondition.ErrorText = "Not all categories have item condition set"
                                    bForcePageChange = True
                                    oForceTo = Me.TabPageSelectCategory
                                End If
                            Next

                            ' We need to validate the CatalogEnabled category tab before allowing movement forward.
                            ' We need to populate the form property "CatalogEnabledCategorys" with a list of CatalogEnabled categories
                            ' This is so we can navigate back and forth through them
                            CatalogEnabledCategorys.Clear()
                            For Each OneCategory In aCategoriesToAddItemTo
                                If OneCategory.CatalogEnabled Then
                                    CatalogEnabledCategorys.Add(OneCategory.CategoryID)
                                End If
                            Next

                            ' If the category is CatalogEnabled we move to tabCatalogEnabledCategory
                            ' If any category has CatalogEnabled = true and CatalogPropertiesSelected = false
                            ' we force change to tabPageCatalogEnabledCategory

                            If oForceTo Is Nothing Then

                                ' We only do this if there is no force tab present 
                                If CatalogEnabledCategorys.Count = 0 Then
                                    ' We move to add pictures - no catalog enabled categories selected
                                    bForcePageChange = True
                                    oForceTo = TabPageAddPictures
                                Else
                                    ' Now get the category (if any) being viewed and the next category to view (if any)
                                    If CatalogEnabledCategoryBeingViewed.Count > 0 Then
                                        ' There is a category being viewed, calculate the next category to view (or this may be the last)
                                        iCategoryBeingViewed = CInt(CatalogEnabledCategoryBeingViewed(0))
                                        iCategoryToView = GetNextCatalogEnabledCategoryToView("NEXT", iCategoryBeingViewed)
                                    Else
                                        ' We view the first category
                                        iCategoryBeingViewed = 0
                                        iCategoryToView = CInt(CatalogEnabledCategorys(0))
                                    End If

                                    If iCategoryToView = 0 Then
                                        ' iCategoryToView = 0 means "last category" has been viewed, move to add pictures
                                        bForcePageChange = True
                                        oForceTo = TabPageAddPictures
                                    Else
                                        ' Populate the tab that we wish to move to
                                        iCategoryToViewNextPosition = GetCategoryPosition(iCategoryToView)

                                        ' Populate the form property showing which category we are working on
                                        With CatalogEnabledCategoryBeingViewed
                                            .Clear()
                                            .Add(iCategoryToView)
                                        End With

                                        bForcePageChange = True
                                        If CatalogEnabledCategorysTab < 2 Then
                                            ' Use tab 2
                                            SetCategoryToView2(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory2
                                            CatalogEnabledCategorysTab = 2
                                        Else
                                            ' Use tab 1
                                            SetCategoryToView2(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory1
                                            CatalogEnabledCategorysTab = 1
                                        End If

                                    End If
                                End If
                            End If
                        End If

                        If Direction = "BACK" Then
                            If CatalogEnabledCategorys.Count = 0 Then
                                ' We move back to select category - no catalog enabled categories selected
                                bForcePageChange = True
                                oForceTo = TabPageSelectCategory
                            Else
                                ' Work out where we are in the categories
                                If CatalogEnabledCategoryBeingViewed.Count > 0 Then
                                    ' Get the category being viewed
                                    iCategoryBeingViewed = CInt(CatalogEnabledCategoryBeingViewed(0))
                                    iCategoryToView = GetNextCatalogEnabledCategoryToView("BACK", iCategoryBeingViewed)
                                    ' Populate the form property showing which category we are working on
                                    With CatalogEnabledCategoryBeingViewed
                                        .Clear()
                                        .Add(iCategoryToView)
                                    End With
                                    bForcePageChange = True

                                    If iCategoryToView = 0 Then
                                        ' Back to Select category
                                        bForcePageChange = True
                                        oForceTo = TabPageSelectCategory
                                    End If

                                    If oForceTo Is Nothing Then
                                        If CatalogEnabledCategorysTab = 1 Then
                                            ' Use tab 2
                                            SetCategoryToView2(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory2
                                            CatalogEnabledCategorysTab = 2
                                        Else
                                            ' Use tab 1
                                            SetCategoryToView1(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory1
                                            CatalogEnabledCategorysTab = 1
                                        End If
                                    End If
                                    ' iCategoryToViewNextPosition = GetCategoryPosition(iCategoryToView)
                                Else
                                    ' Back to Select category
                                    bForcePageChange = True
                                    oForceTo = TabPageSelectCategory
                                End If
                            End If
                        End If

                    Case tabPageCatalogEnabledCategory1.Name
                            ' Here we can move to tabPageCatalogEnabledCategory2 or TabPageAddPictures
                            oForceTo = Nothing
                            ' Validate previous form

                        If Direction = "NEXT" Then
                            If oForceTo Is Nothing Then
                                ' We only do this if there is no force tab present 
                                If CatalogEnabledCategorys.Count = 0 Then
                                    ' We move to add pictures - no catalog enabled categories selected
                                    bForcePageChange = True
                                    oForceTo = TabPageAddPictures
                                Else
                                    ' Now get the category (if any) being viewed and the next category to view (if any)
                                    If CatalogEnabledCategoryBeingViewed.Count > 0 Then
                                        ' There is a category being viewed, calculate the next category to view (or this may be the last)
                                        iCategoryBeingViewed = CInt(CatalogEnabledCategoryBeingViewed(0))
                                        iCategoryToView = GetNextCatalogEnabledCategoryToView("NEXT", iCategoryBeingViewed)
                                    Else
                                        ' We view the first category
                                        iCategoryBeingViewed = 0
                                        iCategoryToView = CInt(CatalogEnabledCategorys(0))
                                    End If

                                    ' Populate the form property showing which category we are working on
                                    With CatalogEnabledCategoryBeingViewed
                                        .Clear()
                                        .Add(iCategoryToView)
                                    End With

                                    bForcePageChange = True
                                    If iCategoryToView = 0 Then
                                        ' iCategoryToView = 0 means "last category" has been viewed, move to add pictures
                                        bForcePageChange = True
                                        oForceTo = TabPageAddPictures
                                    Else
                                        ' Populate the tab that we wish to move to
                                        iCategoryToViewNextPosition = GetCategoryPosition(iCategoryToView)
                                        If CatalogEnabledCategorysTab = 1 Then
                                            ' Use tab 2
                                            SetCategoryToView2(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory2
                                            CatalogEnabledCategorysTab = 2
                                        Else
                                            ' Use tab 1
                                            SetCategoryToView1(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory1
                                            CatalogEnabledCategorysTab = 1
                                        End If
                                    End If

                                End If
                            End If
                        End If

                        If Direction = "BACK" Then
                            If CatalogEnabledCategorys.Count = 0 Then
                                ' We move back to select category - no catalog enabled categories selected
                                bForcePageChange = True
                                oForceTo = TabPageSelectCategory
                            Else
                                ' Work out where we are in the categories
                                If CatalogEnabledCategoryBeingViewed.Count > 0 Then
                                    ' Get the category being viewed
                                    iCategoryBeingViewed = CInt(CatalogEnabledCategoryBeingViewed(0))
                                    iCategoryToView = GetNextCatalogEnabledCategoryToView("BACK", iCategoryBeingViewed)
                                    ' Populate the form property showing which category we are working on
                                    With CatalogEnabledCategoryBeingViewed
                                        .Clear()
                                        .Add(iCategoryToView)
                                    End With
                                    bForcePageChange = True

                                    If iCategoryToView = 0 Then
                                        ' Back to Select category
                                        bForcePageChange = True
                                        oForceTo = TabPageSelectCategory
                                    End If

                                    If oForceTo Is Nothing Then
                                        If CatalogEnabledCategorysTab = 1 Then
                                            ' Use tab 2
                                            SetCategoryToView2(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory2
                                            CatalogEnabledCategorysTab = 2
                                        Else
                                            ' Use tab 1
                                            SetCategoryToView1(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory1
                                            CatalogEnabledCategorysTab = 1
                                        End If
                                    End If
                                    ' iCategoryToViewNextPosition = GetCategoryPosition(iCategoryToView)
                                Else
                                    ' Back to Select category
                                    bForcePageChange = True
                                    oForceTo = TabPageSelectCategory
                                End If
                            End If
                        End If

                    Case tabPageCatalogEnabledCategory2.Name

                        ' Here we can move to tabPageCatalogEnabledCategory1 or TabPageAddPictures or TabPageSelectCategory

                        oForceTo = Nothing
                        ' Validate previous form

                        If oForceTo Is Nothing Then
                            ' We only do this if there is no force tab present 

                            ' Now get the category (if any) being viewed and the next category to view (if any)
                            If CatalogEnabledCategoryBeingViewed.Count > 0 Then
                                ' There is a category being viewed, calculate the next category to view (or this may be the last)
                                iCategoryBeingViewed = CInt(CatalogEnabledCategoryBeingViewed(0))
                                iCategoryToView = GetNextCatalogEnabledCategoryToView("NEXT", iCategoryBeingViewed)
                            Else
                                ' We view the first category
                                iCategoryBeingViewed = 0
                                iCategoryToView = CInt(CatalogEnabledCategorys(0))
                            End If

                            If iCategoryToView = 0 Then
                                ' iCategoryToView = 0 means "last category" has been viewed, move to add pictures
                                bForcePageChange = True
                                oForceTo = TabPageAddPictures
                            Else
                                ' Populate the tab that we wish to move to
                                'iCategoryToViewNextPosition = GetCategoryPosition(iCategoryToView)
                                SetCategoryToView1(iCategoryToView)

                                ' Populate the form property showing which category we are working on
                                With CatalogEnabledCategoryBeingViewed
                                    .Clear()
                                    .Add(iCategoryToView)
                                End With

                                bForcePageChange = True
                                oForceTo = tabPageCatalogEnabledCategory1
                            End If
                        End If

                        If Direction = "BACK" Then
                            If CatalogEnabledCategorys.Count = 0 Then
                                ' We move back to select category - no catalog enabled categories selected
                                bForcePageChange = True
                                oForceTo = TabPageSelectCategory
                            Else
                                ' Work out where we are in the categories
                                If CatalogEnabledCategoryBeingViewed.Count > 0 Then
                                    ' Get the category being viewed
                                    iCategoryBeingViewed = CInt(CatalogEnabledCategoryBeingViewed(0))
                                    iCategoryToView = GetNextCatalogEnabledCategoryToView("BACK", iCategoryBeingViewed)
                                    ' Populate the form property showing which category we are working on
                                    With CatalogEnabledCategoryBeingViewed
                                        .Clear()
                                        .Add(iCategoryToView)
                                    End With
                                    bForcePageChange = True

                                    If iCategoryToView = 0 Then
                                        ' Back to Select category
                                        bForcePageChange = True
                                        oForceTo = TabPageSelectCategory
                                    End If

                                    If oForceTo Is Nothing Then
                                        If CatalogEnabledCategorysTab = 1 Then
                                            ' Use tab 2
                                            SetCategoryToView2(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory2
                                            CatalogEnabledCategorysTab = 2
                                        Else
                                            ' Use tab 1
                                            SetCategoryToView1(iCategoryToView)
                                            oForceTo = tabPageCatalogEnabledCategory1
                                            CatalogEnabledCategorysTab = 1
                                        End If
                                    End If
                                    ' iCategoryToViewNextPosition = GetCategoryPosition(iCategoryToView)
                                Else
                                    ' Back to Select category
                                    bForcePageChange = True
                                    oForceTo = TabPageSelectCategory
                                End If
                            End If
                        End If

                    Case tabPagePricePostage.Name

                        ' Pull in eBay data for controls
                        If Direction = "NEXT" Then
                            SetuptabPagePricePostage()
                        End If

                    Case tabPageStockQuantity.Name

                        If Direction = "NEXT" Then
                            ' Validate the previous form controls

                            If chkAddBuyItNow.Checked Then
                                ' If buy it now is ticked we must have a buy it now price
                                With txtBuyItNowPrice
                                    Try
                                        dDec = CDec(.Text)
                                    Catch ex As Exception
                                        ' Failed to convert input to decimal
                                        .ErrorText = "You must input a buy it now price"
                                        bForcePageChange = True
                                    End Try
                                End With
                            End If

                            If rdoStartTime.SelectedIndex = 1 Then
                                ' We need a start time
                                With DateEditScheduledTime
                                    If .EditValue Is Nothing Then
                                        .ErrorText = "You must pick a scheduled start time"
                                        bForcePageChange = True
                                    End If
                                End With
                            End If

                            With cmbLastingFor
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick an auction duration"
                                    bForcePageChange = True
                                End If
                            End With

                            With cmbDispatchTime
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick a dispatch time"
                                    bForcePageChange = True
                                End If
                            End With

                            With cmbService
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick a postal service"
                                    bForcePageChange = True
                                End If
                            End With

                            With cmbReturnsPolicy
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick a returns policy"
                                    bForcePageChange = True
                                End If
                            End With

                            With cmbRefundOptions
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick a refund option"
                                    bForcePageChange = True
                                End If
                            End With

                            With cmbShippingCostsPaidBy
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick who pays shipping costs"
                                    bForcePageChange = True
                                End If
                            End With

                            With cmbPostalDestination
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick a postal destination"
                                    bForcePageChange = True
                                End If
                            End With

                            With cmbReturnWithin
                                If .EditValue Is Nothing Then
                                    .ErrorText = "You must pick the Returns Witin value"
                                    bForcePageChange = True
                                End If
                            End With

                            With txtPostageCostToBuyer
                                Try
                                    dDec = CDec(.Text)
                                Catch ex As Exception
                                    ' Failed to convert input to decimal
                                    .ErrorText = "You must input a postage cost to buyer"
                                    bForcePageChange = True
                                End Try
                            End With

                            With txtReturnsPolicyText
                                If .Enabled Then
                                    If .Text.Trim = "" Then
                                        .ErrorText = "You must input some return policy text"
                                        bForcePageChange = True
                                    End If
                                End If
                            End With

                            If chkPercentOfInterpriseSellingPrice.Checked Then
                                ' Calculate % of interprise selling price is the starting price
                                With spnPercentOfInterpriseSellingPrice
                                    If .Value < 1 Then
                                        .ErrorText = "You must select a % of interprise selling price"
                                        bForcePageChange = True
                                    End If
                                End With
                            Else
                                ' We are using a user input starting price
                                With txtStartAuction
                                    Try
                                        dDec = CDec(.Text)
                                    Catch ex As Exception
                                        ' Failed to convert input to decimal
                                        .ErrorText = "You must input an auction start price"
                                        bForcePageChange = True
                                    End Try
                                End With
                            End If
                        End If

                        If bForcePageChange And oForceTo Is Nothing Then
                            ' If no page is specified we default to the previous
                            oForceTo = Me.tabPagePricePostage
                        End If

                        If Not bForcePageChange Then
                            ' Previous tab passed error checking, so navigate in the usual way
                            Select Case e.PrevPage.Name

                                Case Direction
                                Case "NEXT"
                                    ' Going forward on the wizard

                                    Select Case m_AuctionType
                                        Case AuctionTypeNormal
                                            ' We skip this page
                                            bForcePageChange = True
                                            oForceTo = Me.tabPageHowPaid

                                        Case AuctionTypeStockQuantity

                                        Case AuctionTypeMultiple
                                            ' We skip this page
                                            bForcePageChange = True
                                            oForceTo = Me.tabPageStockMultiple
                                        Case Else
                                    End Select
                                Case "BACK"
                                    ' Going backwards on the wizard
                                    Select Case m_AuctionType
                                        Case AuctionTypeNormal
                                            ' We skip this page
                                            bForcePageChange = True
                                            oForceTo = Me.tabPagePricePostage

                                        Case AuctionTypeStockQuantity

                                        Case AuctionTypeMultiple
                                            ' We skip this page
                                            bForcePageChange = True
                                            oForceTo = Me.tabPagePricePostage
                                        Case Else
                                    End Select
                            End Select

                        End If

                    Case tabPageStockMultiple.Name
                        'Select Direction
                    Case "BACK"
                        ' Going backward
                        Select Case m_AuctionType
                            Case AuctionTypeNormal
                                ' We skip this page
                                bForcePageChange = True
                                oForceTo = Me.tabPagePricePostage

                            Case AuctionTypeStockQuantity
                                '' We skip this page
                                bForcePageChange = True
                                oForceTo = Me.tabPageStockQuantity

                            Case AuctionTypeMultiple

                        End Select

                    Case "NEXT"
                        ' Going forward
                        With txtLocation
                            If .Text.Trim() = "" Then
                                .ErrorText = "You must input the auction location"
                                bForcePageChange = True
                                oForceTo = Me.tabPageStockQuantity
                            End If
                        End With

                        If Not bForcePageChange Then
                            Select Case m_AuctionType
                                Case AuctionTypeNormal
                                    ' We skip this page
                                    bForcePageChange = True
                                    oForceTo = Me.tabPageHowPaid

                                Case AuctionTypeStockQuantity
                                    ' We skip this page
                                    bForcePageChange = True
                                    oForceTo = Me.tabPageHowPaid

                                Case AuctionTypeMultiple
                            End Select
                        End If

                    Case TabPageComplete.Name
                        If Direction = "NEXT" Then

                            ' Validate the previous form
                            ' No Catalog Enabled action. Validate previous form
                            With txtPaypalEmail
                                If .Text.Trim = "" Then
                                    .ErrorText = "You must input a PayPal address to receive payment to"
                                    bForcePageChange = True
                                End If
                                If InStr(.Text, "@") = 0 Or InStr(.Text, ".") = 0 Or Len(.Text.Trim) < 5 Then
                                    .ErrorText = "You must input a valid email address to receive payment to"
                                    bForcePageChange = True
                                End If
                            End With

                            If oForceTo Is Nothing Then
                                ' No errors on previous tab. Decide where to go.
                                iNoCatalogEnabled = CountCatalogEnabledCategories()
                                iNoCatalogPropertiesSelected = CountCatalogPropertiesSelected()

                                ' If any category is CatalogEnabled and not been dealt with we move to TabPageSelectCategory, else  TabPageAddPictures
                                If iNoCatalogEnabled = 0 Then
                                    ' No CatalogEnabled categories, go straight to AddPictures
                                    bForcePageChange = True
                                    oForceTo = TabPageAddPictures
                                Else
                                    bForcePageChange = True
                                    oForceTo = tabPageCatalogEnabledCategory1
                                    bCatalogEnabled = True
                                End If
                            End If

                            ' If any category has CatalogEnabled = true and CatalogPropertiesSelected = false
                            ' we force change to tabPageCatalogEnabledCategory
                            ' Note if the user has selected 5 categories and 3 are CatalogEnabled this page will appear 3 times.
                            ' The page will set [Category].CatalogPropertiesSelected = true
                        Else
                            ' Going back
                            ' Move back to select category
                            bForcePageChange = True
                            oForceTo = TabPageSelectCategory

                            ' If any category has CatalogEnabled = true and CatalogPropertiesSelected = false
                            ' we force change to tabPageCatalogEnabledCategory
                            ' Note if the user has selected 5 categories and 3 are CatalogEnabled this page will appear 3 times.
                            ' The page will set [Category].CatalogPropertiesSelected = true

                            ' We need to parse this and display the LAST, so we are really "going backwards"

                            For Each OneCategory In aCategoriesToAddItemTo
                                If OneCategory.CatalogEnabled And OneCategory.CatalogPropertiesSelected = False Then
                                    ' This category is Catalog Enabled and the user has not selected the properties for it.
                                    bForcePageChange = True
                                    oForceTo = tabPageCatalogEnabledCategory1
                                    Exit For
                                End If
                            Next
                        End If

                        If bForcePageChange And oForceTo Is Nothing Then
                            oForceTo = tabPageHowPaid
                        End If

                    Case tabPageCatalogEnabledCategory1.Name
                End Select

            End If

            ' If we have been asked to change the pagetab and have been given a tab to move to, do so
            If bForcePageChange And Not oForceTo Is Nothing Then
                m_IgnorePageChangeEvent = True
                WizardControlEBayPub.SelectedTabPage = oForceTo
                m_IgnorePageChangeEvent = False
            End If
        End Sub
#End Region

        Public Sub SetCategoryToView1(iCategoryToView As Integer)
            ' Set the tab up with the detail for the category
            Dim OneCategory As eBayCategory
            ' Get the category
            OneCategory = GetSelectedCategoryFromForm(iCategoryToView)

            ' Populate tabPageCatalogEnabledCategory
            PopulateCategoryEnabledTabFromCategory1(OneCategory)

        End Sub

        Public Sub SetCategoryToView2(iCategoryToView As Integer)
            ' Set the tab up with the detail for the category
            Dim OneCategory As eBayCategory
            ' Get the category
            OneCategory = GetSelectedCategoryFromForm(iCategoryToView)

            ' Populate tabPageCatalogEnabledCategory
            PopulateCategoryEnabledTabFromCategory2(OneCategory)

        End Sub
        Public Function GetCategoryPosition(ByRef iCategoryID As Integer) As Integer
            Dim iCategoryToViewNextPosition As Integer = 0

            For iLoop = 0 To CatalogEnabledCategorys.Count - 1
                If CInt(CatalogEnabledCategorys(iLoop)) = iCategoryID Then
                    iCategoryToViewNextPosition = iLoop
                    Exit For
                End If
            Next

            Return iCategoryToViewNextPosition
        End Function

        Public Function GetNextCatalogEnabledCategoryToView(ByRef sDirection As String, ByRef iCategoryBeingViewed As Integer) As Integer

            Dim iCategoryToShowNext As Integer = 0
            Dim iLoopCount As Integer

            ' Get the category ID to view next
            Select Case sDirection.ToUpper
                Case "NEXT"
                    ' Go forward through the collection of category ID's
                    If CatalogEnabledCategorys.Count = 1 Then

                    Else
                        If iCategoryBeingViewed = 0 Then
                            ' First category
                            If CatalogEnabledCategorys.Count > 0 Then
                                iCategoryToShowNext = CInt(CatalogEnabledCategorys(0))
                            End If
                        Else
                            For iLoopCount = 0 To CatalogEnabledCategorys.Count - 1
                                If CInt(CatalogEnabledCategorys(iLoopCount)) = iCategoryBeingViewed Then
                                    ' We have a match. We need the next array element if it exists
                                    If iLoopCount + 1 <= CatalogEnabledCategorys.Count - 1 Then
                                        iCategoryToShowNext = CInt(CatalogEnabledCategorys(iLoopCount + 1))
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                Case "BACK"
                    ' Go backwards through the collection of category ID's

                    If iCategoryBeingViewed = 0 Then
                        ' Last category
                        If CatalogEnabledCategorys.Count > 0 Then
                            iCategoryToShowNext = CInt(CatalogEnabledCategorys(CatalogEnabledCategorys.Count - 1))
                        End If
                    Else
                        For iLoopCount = CatalogEnabledCategorys.Count - 1 To 0 Step -1
                            If CInt(CatalogEnabledCategorys(iLoopCount)) = iCategoryBeingViewed Then
                                If iLoopCount = 0 Then
                                    ' No more catalog enabled categories
                                Else
                                    iCategoryToShowNext = CInt(CatalogEnabledCategorys(iLoopCount - 1))
                                End If
                                Exit For
                            End If
                        Next
                    End If

            End Select

            Return iCategoryToShowNext
        End Function

        Private Sub PopulateCategoryEnabledTabFromCategory1(OneCategory As eBayCategory)

            Dim iNoSelectedCategories As Integer = CountSelectedCategories()
            Dim iNoCatalogEnabled As Integer = CountCatalogEnabledCategories()
            Dim iNoCatalogPropertiesSelected As Integer = CountCatalogPropertiesSelected()
            Dim iCategoryBeingViewed As Integer = 1
            'Dim OneTempCategory As eBayCategory

            Dim sText As String = DateTime.Now.ToString & " : "

            iCategoryBeingViewed = GetCategoryPosition(OneCategory.CategoryID) + 1
            ' Get the X of Y for the Catalog Enabled categories

            ' Now populate the tab from the category
            sText = sText & "You have picked " & iNoSelectedCategories & " catego"
            If iNoSelectedCategories = 1 Then
                sText = sText & "ry"
            Else
                sText = sText & "ries. Of these categories " & iNoCatalogEnabled.ToString & " "
                If iNoCatalogEnabled = 1 Then
                    sText = sText & "is"
                Else
                    sText = sText & "are"
                End If
                sText = sText & " catalog enabled. "
            End If

            If iNoSelectedCategories > 1 Then
                sText = sText & " You are viewing category " & iCategoryBeingViewed.ToString & " of " & iNoCatalogEnabled.ToString
                sText = sText & " - " & OneCategory.CategoryName & "."
            Else
            End If
            lblCatalogEnabledXofY1.Text = sText
        End Sub

        Private Sub PopulateCategoryEnabledTabFromCategory2(OneCategory As eBayCategory)

            Dim iNoSelectedCategories As Integer = CountSelectedCategories()
            Dim iNoCatalogEnabled As Integer = CountCatalogEnabledCategories()
            Dim iNoCatalogPropertiesSelected As Integer = CountCatalogPropertiesSelected()
            Dim iCategoryBeingViewed As Integer = 1
            'Dim OneTempCategory As eBayCategory

            Dim sText As String = DateTime.Now.ToString & " : "

            iCategoryBeingViewed = GetCategoryPosition(OneCategory.CategoryID) + 1
            ' Get the X of Y for the Catalog Enabled categories

            ' Now populate the tab from the category
            sText = sText & "You have picked " & iNoSelectedCategories & " catego"
            If iNoSelectedCategories = 1 Then
                sText = sText & "ry"
            Else
                sText = sText & "ries. Of these categories " & iNoCatalogEnabled.ToString & " "
                If iNoCatalogEnabled = 1 Then
                    sText = sText & "is"
                Else
                    sText = sText & "are"
                End If
                sText = sText & " catalog enabled. "
            End If

            If iNoCatalogEnabled > 1 Then
                sText = sText & " You are viewing category " & iCategoryBeingViewed.ToString & " of " & iNoCatalogEnabled.ToString
                sText = sText & " - " & OneCategory.CategoryName & "."
            Else
            End If
            lblCatalogEnabledXofY2.Text = sText
        End Sub
        Private Function CountCatalogEnabledCategories() As Integer

            Dim iNoCatalogEnabled As Integer = CatalogEnabledCategorys.Count
            Return iNoCatalogEnabled
        End Function

        Private Function CountCatalogPropertiesSelected() As Integer

            Dim OneCategory As eBayCategory
            Dim iNoCatalogPropertiesSelected As Integer = 0

            For Each OneCategory In aCategoriesToAddItemTo
                If OneCategory.CatalogPropertiesSelected Then
                    iNoCatalogPropertiesSelected = iNoCatalogPropertiesSelected + 1
                End If
            Next

            Return iNoCatalogPropertiesSelected
        End Function

        Private Function CountSelectedCategories() As Integer

            Dim iSelectedCategories As Integer = aCategoriesToAddItemTo.Count

            Return iSelectedCategories
        End Function
        Private Sub SetuptabPagePricePostage()

            Application.DoEvents()
            GetShippingServiceDetails()
            Application.DoEvents()
            GetDispatchTimeMaxDetails()
            Application.DoEvents()

        End Sub
        Private Sub UpdateStatus(ByRef sStatus As String, Optional ByRef bError As Boolean = False)
            Dim iColour As System.Drawing.Color = Color.Black

            Me.lblEbayStatus.Text = sStatus
            If bError Then
                iColour = Color.Red
            End If

            Me.lblEbayStatus.ForeColor = iColour

        End Sub

        Private Function GetCategoriesFromDatabase() As Boolean
            Dim bSuccess As Boolean = True
            Dim sStatus As String = ""
            Dim sDescription As String = ""
            Dim sXML As String = ""

            Try
                bSuccess = Me.m_eBayPublishingWizardSectionFacade.GetCategoriesFromDatabase(oeBayWrapper)
            Catch ex As Exception
                sStatus = ex.Message
                bSuccess = False
            End Try

            Return bSuccess
        End Function

        Private Sub SetupSelectedCategory(sDescription As String)
            Dim bSuccess As Boolean = True

            Dim iNoOfCategories As Long = 0
            Dim sStatus As String = ""
            Dim OneCategory As eBayCategory

            Dim DisplayText As String = ""

            imgListeBayCategory.Items.Clear()
            lblEbayStatus.Text = "Getting categories from eBay"

            Try
                sLastSearch = sDescription
                bSuccess = oeBayWrapper.GetSuggestedCategories(sDescription)

            Catch ex As Exception
                sStatus = ex.Message
                bSuccess = False
            End Try

            If bSuccess Then
                For Each OneCategory In oeBayWrapper.aSuggestedCategories
                    imgListeBayCategory.Items.Add(OneCategory.CategoryName, OneCategory.CategoryID)
                    iNoOfCategories = iNoOfCategories + 1
                Next
            End If

            If iNoOfCategories = 0 Then
                DisplayText = "No categories suggested from eBay"
            Else
                DisplayText = iNoOfCategories.ToString & " "
                If iNoOfCategories = 1 Then
                    DisplayText = DisplayText & "category"
                Else
                    DisplayText = DisplayText & "categories"
                End If
                DisplayText = DisplayText & " suggested from eBay"
            End If

            lblEbayStatus.Text = DisplayText
            If sStatus <> "" Then
                'Me.UpdateStatus(sStatus)
            End If
        End Sub
        Private Sub cmdAddPhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddPhoto.Click
            Dim sPictureFile As String = ""
            With OpenFileDialog1
                .Title = "Select Picture To Upload"
                .InitialDirectory = "c:\"
                .Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*" ' LJG 18/06/2012
                .FileName = ""
                .ShowDialog()

                sPictureFile = .FileName.ToString()
            End With

            If sPictureFile <> "" And File.Exists(sPictureFile) Then ' LJG 18/06/2012 Added file.exists
                PictureBox1.ImageLocation = sPictureFile
            End If
        End Sub

        Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
            Dim sPictureFile As String = ""
            With OpenFileDialog1
                .Title = "Select Picture To Upload"
                .InitialDirectory = "c:\"
                .Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*" ' LJG 18/06/2012
                .FileName = ""
                .ShowDialog()

                sPictureFile = .FileName.ToString()
            End With

            If sPictureFile <> "" And File.Exists(sPictureFile) Then ' LJG 18/06/2012 Added file.exists
                PictureBox2.ImageLocation = sPictureFile
            End If
        End Sub

        Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
            Dim sPictureFile As String = ""
            With OpenFileDialog1
                .Title = "Select Picture To Upload"
                .InitialDirectory = "c:\"
                .Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*" ' LJG 18/06/2012
                .FileName = ""
                .ShowDialog()

                sPictureFile = .FileName.ToString()
            End With

            If sPictureFile <> "" And File.Exists(sPictureFile) Then ' LJG 18/06/2012 Added file.exists
                PictureBox3.ImageLocation = sPictureFile
            End If
        End Sub

        Private Sub chkAdd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAddBuyItNow.CheckedChanged
            txtBuyItNowPrice.Enabled = chkAddBuyItNow.Checked
            If Not chkAddBuyItNow.Checked Then
                txtBuyItNowPrice.ErrorText = ""
            End If
        End Sub

        Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim sVal As String = ""
            Dim iVal As Integer = 0

            ' Pull out the selected text and category ID
            sVal = imgListeBayCategory.SelectedItem.ToString()
            iVal = imgListeBayCategory.Items(imgListeBayCategory.SelectedIndex).ImageIndex
        End Sub

        Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
            Dim bSuccess As Boolean = True
            Dim sXML As String = ""
            bSuccess = GetShippingServiceDetails()

            If bSuccess Then
                sXML = oeBayWrapper.ReturnedData
                ' Now parse the carriers
            End If
        End Sub

        Private Sub GeteBayLists()
            ReDim m_geteBayDetails(0)
            Dim sNodeXML As String
            Dim bSuccess As Boolean

            AddToEbayDetails("BuyerRequirementDetails")
            AddToEbayDetails("CountryDetails")
            AddToEbayDetails("CurrencyDetails")
            'AddToEbayDetails("CustomCode")
            AddToEbayDetails("DispatchTimeMaxDetails")
            AddToEbayDetails("ExcludeShippingLocationDetails")
            AddToEbayDetails("ItemSpecificDetails")
            AddToEbayDetails("ListingFeatureDetails")
            'AddToEbayDetails("PaymentOptionDetails")
            AddToEbayDetails("RecoupmentPolicyDetails")
            'AddToEbayDetails("RegionDetails")
            'AddToEbayDetails("RegionOfOriginDetails")
            AddToEbayDetails("ReturnPolicyDetails")
            AddToEbayDetails("ShippingCarrierDetails")
            AddToEbayDetails("ShippingCategoryDetails")
            AddToEbayDetails("ShippingLocationDetails")
            AddToEbayDetails("ShippingPackageDetails")
            AddToEbayDetails("ShippingServiceDetails")
            AddToEbayDetails("SiteDetails")
            AddToEbayDetails("TaxJurisdiction")
            AddToEbayDetails("TimeZoneDetails")
            AddToEbayDetails("UnitOfMeasurementDetails")
            AddToEbayDetails("URLDetails")
            AddToEbayDetails("VariationDetails")

            For Each Listing In m_geteBayDetails
                sNodeXML = Listing.Request
                bSuccess = oeBayWrapper.GetEBayDetails(Listing.Request)
                If bSuccess Then
                    sNodeXML = oeBayWrapper.ReturnedData
                End If
            Next
        End Sub
        Private Function GetPostalDestination() As Boolean
            Dim bSuccess As Boolean = True

            Dim sNodeXML As String = ""
            Dim XMLNSManeBay As System.Xml.XmlNamespaceManager, XMLNameTabeBay As System.Xml.NameTable
            Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
            'Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
            Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

            'Dim ShippingCarrierID As String
            'Dim Description As String
            'Dim DetailVersion As String
            'Dim UpdateTime As String

            bSuccess = oeBayWrapper.GetEBayDetails("ShippingCarrierDetails")
            ReDim m_ShipmentPostalDesination(0)

            If bSuccess Then
                ' Parse the incoming XML
                'Dim sReplaceFrom As String = ">" & Microsoft.VisualBasic.vbCr & Microsoft.VisualBasic.vbLf & "  <"
                'Dim sReplaceTo As String = ">" & Microsoft.VisualBasic.vbCr & Microsoft.VisualBasic.vbLf & "<"

                'sNodeXML = oeBayWrapper.ReturnedData.ToString().Replace(sReplaceFrom, sReplaceTo)
                sNodeXML = oeBayWrapper.ReturnedData
                ' for some reason XPathSelectElement doesn't like having this namespace included
                m_ReturnedXML = XDocument.Parse(sNodeXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                XMLNameTabeBay = New System.Xml.NameTable
                XMLNSManeBay = New System.Xml.XmlNamespaceManager(XMLNameTabeBay)
                XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/ShippingCarrierDetails")
                For Each XMLNode In XMLNodeList
                    XMLTemp = XDocument.Parse(XMLNode.ToString)

                    If m_ShipmentPostalDesination.Length > 1 OrElse m_ShipmentPostalDesination(0).Description <> "" Then
                        ReDim Preserve m_ShipmentPostalDesination(m_ShipmentPostalDesination.Length)
                    End If

                    With m_ShipmentPostalDesination(m_ShipmentPostalDesination.Length - 1)
                        .ShippingCarrierID = XMLTemp.XPathSelectElement("ShippingCarrierDetails/ShippingCarrierID").Value()
                        .Description = XMLTemp.XPathSelectElement("ShippingCarrierDetails/Description").Value()
                        .DetailVersion = XMLTemp.XPathSelectElement("ShippingCarrierDetails/DetailVersion").Value()
                        .UpdateTime = XMLTemp.XPathSelectElement("ShippingCarrierDetails/UpdateTime").Value()
                    End With
                Next
            End If

            Return bSuccess
        End Function

        Private Sub AddToEbayDetails(ByVal sDetailsToAdd As String)

            If m_geteBayDetails.Length > 1 OrElse m_geteBayDetails(0).Request <> "" Then
                ReDim Preserve m_geteBayDetails(m_geteBayDetails.Length)
            End If

            m_geteBayDetails(m_geteBayDetails.Length - 1).Request = sDetailsToAdd
        End Sub

        Private Function GetShippingServiceDetails() As Boolean
            Dim bSuccess As Boolean = True

            Dim sNodeXML As String = ""
            Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
            Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

            bSuccess = oeBayWrapper.GetEBayDetails("ShippingServiceDetails")
            ReDim m_ShippingServiceDetails(0)
            ReDim Preserve m_ShippingServiceDetails(0).ServiceType(0)

            Dim Description As String

            If bSuccess Then
                sNodeXML = oeBayWrapper.ReturnedData

                ' for some reason XPathSelectElement doesn't like having this namespace included
                m_ReturnedXML = XDocument.Parse(sNodeXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/ShippingServiceDetails")

                For Each XMLNode In XMLNodeList

                    XMLTemp = XDocument.Parse(XMLNode.ToString)

                    If m_ShippingServiceDetails.Length > 1 OrElse m_ShippingServiceDetails(0).Description <> "" Then
                        ReDim Preserve m_ShippingServiceDetails(m_ShippingServiceDetails.Length)
                        ReDim m_ShippingServiceDetails(m_ShippingServiceDetails.Length - 1).ServiceType(0)
                    End If

                    With m_ShippingServiceDetails(m_ShippingServiceDetails.Length - 1)
                        .Description = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/Description")
                        .InternationalService = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/InternationalService")
                        .ShippingService = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/ShippingService")
                        .ShippingServiceID = CInt(m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/ShippingServiceID"))
                        ' Extend the structure (if required) and add the new elements
                        If .ServiceType.Length > 1 OrElse .ServiceType(0).Type <> "" Then
                            ReDim Preserve .ServiceType(.ServiceType.Length)
                        End If
                        .ServiceType(.ServiceType.Length - 1).Type = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/ServiceType")
                        .ValidForSellingFlow = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/ValidForSellingFlow")
                        .DetailVersion = CInt(m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/DetailVersion"))
                        .UpdateTime = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/UpdateTime")
                        .ShippingCategory = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "ShippingServiceDetails/ShippingCategory")
                    End With
                Next

                ' Now parse the structures to populate the UI
                cmbService.Properties.Items.Clear()
                For Each Carrier As ShippingServiceDetails In m_ShippingServiceDetails
                    Description = Carrier.Description
                    With cmbService
                        .Properties.Items.Add(New ImageComboBoxItem(Description, Carrier.ShippingServiceID))
                    End With
                Next
            End If

            Return bSuccess

        End Function

        Private Sub rdoStartTime_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoStartTime.SelectedIndexChanged
            Dim bEnabled As Boolean = False
            If rdoStartTime.SelectedIndex = 1 Then
                bEnabled = True
            End If
            TimeEditScheduledTime.Enabled = bEnabled
            DateEditScheduledTime.Enabled = bEnabled
        End Sub

        Private Function GetDispatchTimeMaxDetails() As Boolean
            Dim bSuccess As Boolean = True

            Dim sNodeXML As String = ""
            Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
            'Dim XMLChildList As System.Collections.Generic.IEnumerable(Of System.Xml.Linq.XNode)
            Dim m_ReturnedXML As XDocument, XMLTemp As XDocument

            bSuccess = oeBayWrapper.GetEBayDetails("DispatchTimeMaxDetails")
            ReDim m_DispatchTimeMaxDetails(0)

            Dim Description As String

            If bSuccess Then
                sNodeXML = oeBayWrapper.ReturnedData

                ' for some reason XPathSelectElement doesn't like having this namespace included
                m_ReturnedXML = XDocument.Parse(sNodeXML.Replace(" xmlns=""urn:ebay:apis:eBLBaseComponents""", ""))
                XMLNodeList = m_ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/DispatchTimeMaxDetails")

                For Each XMLNode In XMLNodeList

                    XMLTemp = XDocument.Parse(XMLNode.ToString)

                    If m_DispatchTimeMaxDetails.Length > 1 OrElse m_DispatchTimeMaxDetails(0).Description <> "" Then
                        ReDim Preserve m_DispatchTimeMaxDetails(m_DispatchTimeMaxDetails.Length)
                    End If

                    With m_DispatchTimeMaxDetails(m_DispatchTimeMaxDetails.Length - 1)
                        .DispatchTimeMax = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "DispatchTimeMaxDetails/DispatchTimeMax")
                        .Description = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "DispatchTimeMaxDetails/Description")
                        .DetailVersion = CInt(m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "DispatchTimeMaxDetails/DetailVersion"))
                        .UpdateTime = m_eBayPublishingWizardSectionFacade.GetXMLElementText(XMLTemp, "DispatchTimeMaxDetails/UpdateTime")
                    End With
                Next

                ' Now parse the structures to populate the UI
                ' As we are getting 0 days, 1 days, 10 days, 2 days, 20 days etc we pre-sort into display order
                ' First, turn Sorted off
                cmbDispatchTime.Properties.Sorted = False

                cmbDispatchTime.Properties.Items.Clear()

                Dim iOrder As Integer
                Dim iSpacePos As Integer
                Dim sString As String

                Dim ComboSortedItemsExpanded As ComboSortedItems()
                ReDim ComboSortedItemsExpanded(0)

                For Each Dispatch In m_DispatchTimeMaxDetails
                    Description = Dispatch.Description
                    iSpacePos = Microsoft.VisualBasic.InStr(Description, " ")
                    sString = Description.Substring(0, iSpacePos - 1)
                    iOrder = CInt(sString)

                    If iOrder > ComboSortedItemsExpanded.Length - 1 Then
                        ReDim Preserve ComboSortedItemsExpanded(iOrder)
                    End If

                    With ComboSortedItemsExpanded(iOrder)
                        .Description = Description
                        .iURN = iOrder
                        .sURN = CStr(iOrder)
                    End With
                Next

                For Each Pair In ComboSortedItemsExpanded
                    If Not Pair.Description Is Nothing Then
                        With cmbDispatchTime
                            .Properties.Items.Add(New ImageComboBoxItem(Pair.Description, Pair.iURN))
                        End With
                    End If
                Next
            End If

            Return bSuccess

        End Function

        Private Sub chkRepeatAuction_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim bSuccess As Boolean = chkRepeatAuction.Checked
            rdoRepatAuctionFor.Enabled = bSuccess
            spnRepeatQuantity.Enabled = bSuccess
        End Sub

        Private Sub rdoSchedule_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoSchedule.SelectedIndexChanged
            Dim bSuccess As Boolean = False
            If rdoSchedule.SelectedIndex = 1 Then
                bSuccess = True
            End If
            TimeEditSpecifiedTime.Enabled = bSuccess
        End Sub

        Private Sub chkRepeatAuction_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRepeatAuction.CheckedChanged
            Dim bSuccess As Boolean = chkRepeatAuction.Checked
            spnRepeatQuantity.Enabled = bSuccess
            rdoRepatAuctionFor.Enabled = bSuccess
        End Sub

        Private Sub eBayPublishingWizardSection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            lblAuctionTypeDescription.Text = "" ' LJG 18/06/2012
        End Sub

        Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.MouseDown
            m_Direction = "NEXT"
        End Sub

        Private Sub btnNext_KeyDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.KeyDown
            m_Direction = "NEXT"
        End Sub

        Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.MouseDown
            m_Direction = "BACK"
        End Sub

        Private Sub btnBack_KeyDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.KeyDown
            m_Direction = "BACK"
        End Sub

        ' LJG 18/06/2012
        ' Fixed 2 typos in text
        ' Copeied routine to first page

        ' LJG 18/06/2012 Start
        Private Sub cmdGetTemplate_Click(sender As System.Object, e As System.EventArgs) Handles cmdGetTemplate.Click
            Dim sTemplateFile As String = ""
            Dim sHTMLTemplate As String = ""

            With OpenFileDialog1
                .Title = "Select Template To Use"
                .InitialDirectory = "c:\"
                .Filter = "HTML Files(*.HTM;*.HTML)|*.HTM;*.HTML|All files (*.*)|*.*"
                .FileName = ""
                .ShowDialog()

                sTemplateFile = .FileName.ToString()
            End With

            If sTemplateFile <> "" And File.Exists(sTemplateFile) Then
                MemoHTMLTemplate.Text = File.ReadAllText(sTemplateFile)
            End If
        End Sub
        ' LJG 18/06/2012 End

        Private Sub rdoAuctionQuantity_SelectedIndexChanged_1(sender As System.Object, e As System.EventArgs) Handles rdoAuctionQuantity.SelectedIndexChanged
            m_AuctionType = rdoAuctionQuantity.SelectedIndex
            Dim bEnabled As Boolean = False

            If m_AuctionType = 1 Or m_AuctionType = 2 Then
                bEnabled = True
            End If

            spnPercentOfTotal.Enabled = bEnabled
        End Sub

        Private Sub rdoAuctionType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles rdoAuctionType.SelectedIndexChanged
            m_AuctionType = rdoAuctionType.SelectedIndex
            Dim sText As String = "Regular auction, optionally with Buy It Now and scheduled start time"

            Select Case m_AuctionType
                Case 0
                    ' Default text
                Case 1
                    sText = "Auction with visible stock quantity"
                Case 2
                    sText = "Multiple auctions for the same item"

            End Select
            lblAuctionTypeDescription.Text = sText

        End Sub

        Private Sub btnAddCategory_Click(sender As System.Object, e As System.EventArgs) Handles btnAddCategory.Click
            AddCategoryToList()
            imgListeBayCategoriesToAdd_ToDefaults()
        End Sub

        Public Function AddCategoryToList() As Boolean
            ' Add the currently highlighted category in the listbox to the suggested category list

            Dim bSuccess As Boolean = True
            Dim CategoryToAdd As eBayCategory
            Dim OneCategory As eBayCategory

            Dim CategoryName As String, iCategory As Integer
            Dim bDuplicate As Boolean

            Me.UpdateStatus("")

            ' Pull out the selected text and category ID
            CategoryName = imgListeBayCategory.SelectedItem.ToString()
            iCategory = imgListeBayCategory.Items(imgListeBayCategory.SelectedIndex).ImageIndex

            CategoryToAdd = GetOneCategory(iCategory, CategoryName)

            ' Check this isn't a duplicate prior to adding
            For Each OneCategory In aCategoriesToAddItemTo
                If OneCategory.CategoryID = iCategory And OneCategory.CategoryName = CategoryName Then
                    bDuplicate = True
                    Exit For
                End If
            Next

            If Not bDuplicate Then
                ' Flag as "suggested category" or "drill down" category
                CategoryToAdd.SuggestedCategory = Not bManualCategories

                aCategoriesToAddItemTo.Add(CategoryToAdd)
                DisplayCategoriesToAddTo()

                ' Now pull out the category info (this only gets XML if it is required)
                GetCategoryFeaturesForAll()

                oeBayWrapper.aConditionValues.Clear()
                For Each OneCategory In aCategoriesToAddItemTo
                    oeBayWrapper.GetCategoryFeaturesFromXML(OneCategory.GetCategoryFeaturesXML, OneCategory.CategoryID)
                Next
            Else
                lblEbayStatus.Text = "Duplicate Category"
            End If

            Return bSuccess
        End Function

        Public Sub DisplayCategoriesToAddTo()
            Dim OneCategory As eBayCategory
            imgListeBayCategoriesToAdd.Items.Clear()
            For Each OneCategory In aCategoriesToAddItemTo
                imgListeBayCategoriesToAdd.Items.Add(OneCategory.CategoryName, OneCategory.CategoryID)
            Next
        End Sub

        Public Function RemoveCategoryFromList() As Boolean
            ' Add the currently highlighted category in the listbox to the suggested category list

            Dim bSuccess As Boolean = True
            Dim CategoryToAdd As eBayCategory
            Dim OneCategory As eBayCategory
            Dim CategoryToRemove As New eBayCategory

            Dim CategoryName As String, iCategory As Integer

            ' Pull out the selected text and category ID
            CategoryName = imgListeBayCategoriesToAdd.SelectedItem.ToString()
            iCategory = imgListeBayCategoriesToAdd.Items(imgListeBayCategoriesToAdd.SelectedIndex).ImageIndex

            CategoryToAdd = GetOneCategory(iCategory, CategoryName)

            ' Find the category to remove
            For Each OneCategory In aCategoriesToAddItemTo
                If OneCategory.CategoryID = iCategory And OneCategory.CategoryName = CategoryName Then
                    CategoryToRemove = OneCategory
                    Exit For
                End If
            Next

            aCategoriesToAddItemTo.Remove(CategoryToRemove)

            DisplayCategoriesToAddTo()

            ' Now remove the FindProductsResponses (if any)
            oeBayWrapper.RemoveFindProductResponse(iCategory)

            Return bSuccess
        End Function

        Public Function GetOneCategory(iCategory As Integer, CategoryName As String) As eBayCategory
            Dim ReturnCategory As eBayCategory = Nothing
            Dim OneCategory As eBayCategory
            Dim bSuccess As Boolean = True

            If oeBayWrapper.aCategories.Count = 0 Then
                ' Get the categories
                bSuccess = Me.GetCategoriesFromeBay()
            End If

            If bSuccess Then
                For Each OneCategory In oeBayWrapper.aCategories
                    If OneCategory.CategoryID = iCategory And OneCategory.CategoryName = CategoryName Then
                        ReturnCategory = OneCategory
                        Exit For
                    End If
                Next
            End If

            Return ReturnCategory
        End Function

        Public Function GetOneSelectedCategory(iCategory As Integer) As eBayCategory
            Dim ReturnCategory As eBayCategory = Nothing
            Dim OneCategory As eBayCategory
            Dim bSuccess As Boolean = True

            For Each OneCategory In aCategoriesToAddItemTo
                If OneCategory.CategoryID = iCategory Then
                    ReturnCategory = OneCategory
                    Exit For
                End If
            Next

            Return ReturnCategory
        End Function

        Public Function GetSelectedCategoryIDFromForm(Optional iCategoryToGet As Integer = 0) As Integer
            Dim iCategoryID As Integer = 0, SelectedCategory As eBayCategory = Nothing

            If Not imgListeBayCategoriesToAdd.SelectedItem Is Nothing Then
                iCategoryID = imgListeBayCategoriesToAdd.Items(imgListeBayCategoriesToAdd.SelectedIndex).ImageIndex
            End If

            Return iCategoryID

        End Function
        Public Function GetSelectedCategoryFromForm(Optional iCategoryToGet As Integer = 0) As eBayCategory
            Dim iCategoryID As Integer, SelectedCategory As eBayCategory = Nothing

            If iCategoryToGet = 0 Then
                iCategoryID = GetSelectedCategoryIDFromForm()   ' Pull selected category from form
            Else
                iCategoryID = iCategoryToGet    ' Use supplied category ID
            End If

            If iCategoryID > 0 Then
                ' A category is selected on the form, get the category structure
                SelectedCategory = GetOneSelectedCategory(iCategoryID)
            End If

            Return SelectedCategory

        End Function
        Public Function SetOneSelectedCategory(OneCategoryToSave As eBayCategory) As Boolean
            Dim bSuccess As Boolean = False
            Dim iLoop As Integer
            Dim OneCategory As eBayCategory

            ' Go round the categories and save the category
            For iLoop = 0 To aCategoriesToAddItemTo.Count - 1
                ' Pull out the category
                OneCategory = CType(aCategoriesToAddItemTo(iLoop), eBayCategory)
                ' Compare the category
                If OneCategory.CategoryID = OneCategoryToSave.CategoryID Then
                    aCategoriesToAddItemTo(iLoop) = OneCategoryToSave ' Put it back into arraylist (thus exposing it)
                    bSuccess = True
                    Exit For
                End If
            Next

            Return bSuccess
        End Function
        Public Function GetCategoriesFromeBay() As Boolean
            Dim bSuccess As Boolean = True

            If oeBayWrapper.aCategories.Count = 0 Then
                lblEbayStatus.Text = "Retrieving categories from eBay"
                Application.DoEvents()

                bSuccess = Me.GetCategoriesFromDatabase()

                lblEbayStatus.Text = ""
                Application.DoEvents()

            End If
            Return bSuccess
        End Function

        Private Sub btnGetSuggestedCategories_Click(sender As System.Object, e As System.EventArgs) Handles btnGetSuggestedCategories.Click
            oeBayWrapper.aCategoriesSelected.Clear()
            'UpdateCategoryButtons()
            'UpdateSelectedCategoryButtons()
            UpdateBreadcrumbs()
            bGotInitialCategories = True
            bManualCategories = False

            iSelectedCategoryDepth = 0
            Me.SetupSelectedCategory(txtSuggestedCategories.Text)
        End Sub
        'Public Sub UpdateSelectedCategoryButtons()

        '    Dim OneCategory As eBayCategory
        '    Dim iEnableCount As Integer = 0
        '    Dim iDisableCount As Integer = 0

        '    Dim OneButton As DevExpress.XtraEditors.SimpleButton

        '    ' Put the text into the controls
        '    For Each OneCategory In aCategoriesToAddItemTo
        '        iEnableCount = iEnableCount + 1

        '        If iEnableCount <= iMaxDepth Then
        '            ' Grab the form control (button)
        '            Try
        '                OneButton = DirectCast(Me.Controls.Find("btnSelectedCategory" & iEnableCount.ToString, True)(0), DevExpress.XtraEditors.SimpleButton)
        '            Catch ex As Exception
        '                OneButton = Nothing
        '            End Try

        '            If OneButton IsNot Nothing Then
        '                ' Fixup the button
        '                With OneButton
        '                    .Visible = True
        '                    .Enabled = False    ' These buttons are for show only
        '                    .Text = Microsoft.VisualBasic.Replace(OneCategory.CategoryName, "&", "&&")
        '                End With
        '            End If
        '        End If
        '    Next
        '    iEnableCount = iEnableCount + 1
        '    ' Disable the remaining controls
        '    For iDisableCount = iEnableCount To iMaxDepth
        '        Try
        '            OneButton = DirectCast(Me.Controls.Find("btnSelectedCategory" & iDisableCount.ToString, True)(0), DevExpress.XtraEditors.SimpleButton)
        '        Catch ex As Exception
        '            OneButton = Nothing
        '        End Try

        '        If OneButton IsNot Nothing Then
        '            With OneButton
        '                .Visible = False
        '                .Enabled = False
        '                .Text = ""
        '            End With
        '        End If
        '    Next
        'End Sub

        Public Sub UpdateBreadcrumbs()
            Dim OneCategory As eBayCategory
            Dim sBreadcrumb As String = ""
            Dim bUp As Boolean = False

            ' Put the text into the controls
            For Each OneCategory In oeBayWrapper.aCategoriesSelected
                If sBreadcrumb <> "" Then
                    sBreadcrumb = sBreadcrumb & " > "
                End If
                sBreadcrumb = sBreadcrumb & OneCategory.CategoryName
                ' Flag enable up node control
                bUp = True
            Next
            TextEditCategoryTreeeBay.Text = sBreadcrumb

            btnBrowseNodeUpLeveleBay.Enabled = bUp

        End Sub

        Public Sub GoUpOneLevel()
            Dim iLevel As Integer = 0
            Dim eBayCategoryToSelect As eBayCategory
            Dim sBreadcrumb As String = TextEditCategoryTreeeBay.Text
            Dim iLoop As Integer = 0

            ' Calculate the level (count the number of ">" chars in the breadcrum string
            For iLoop = 0 To sBreadcrumb.Length - 1
                If sBreadcrumb.Substring(iLoop, 1) = ">" Then
                    iLevel = iLevel + 1
                End If
            Next

            'iLevel = CInt(btnPressed.Name.Substring(11))
            eBayCategoryToSelect = GetSelectedCategory(iLevel)
            UpdateEbayCategories(eBayCategoryToSelect.CategoryID, eBayCategoryToSelect.CategoryName, iLevel)
            lblEbayStatus.Text = ""
            ' Update the form buttons
            'UpdateCategoryButtons()
            'UpdateSelectedCategoryButtons()
            UpdateBreadcrumbs()
        End Sub

        'Public Sub UpdateCategoryButtons()

        '    Dim OneCategory As eBayCategory
        '    Dim iEnableCount As Integer = 0
        '    Dim iDisableCount As Integer = 0

        '    Dim OneButton As DevExpress.XtraEditors.SimpleButton

        '    ' Put the text into the controls
        '    For Each OneCategory In oeBayWrapper.aCategoriesSelected
        '        iEnableCount = iEnableCount + 1

        '        If iEnableCount <= iMaxDepth Then
        '            ' Grab the form control (button)
        '            Try
        '                OneButton = DirectCast(Me.Controls.Find("btnCategory" & iEnableCount.ToString, True)(0), DevExpress.XtraEditors.SimpleButton)
        '            Catch ex As Exception
        '                OneButton = Nothing
        '            End Try

        '            If OneButton IsNot Nothing Then
        '                ' Fixup the button
        '                With OneButton
        '                    .Visible = True
        '                    .Enabled = True
        '                    .Text = Microsoft.VisualBasic.Replace(OneCategory.CategoryName, "&", "&&")
        '                End With
        '            End If
        '        End If
        '    Next
        '    iEnableCount = iEnableCount + 1
        '    ' Disable the remaining controls
        '    For iDisableCount = iEnableCount To iMaxDepth
        '        Try
        '            OneButton = DirectCast(Me.Controls.Find("btnCategory" & iDisableCount.ToString, True)(0), DevExpress.XtraEditors.SimpleButton)
        '        Catch ex As Exception
        '            OneButton = Nothing
        '        End Try

        '        If OneButton IsNot Nothing Then
        '            With OneButton
        '                .Visible = False
        '                .Enabled = False
        '                .Text = ""
        '            End With
        '        End If
        '    Next
        'End Sub

        'Private Sub btnCategory1_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory1)
        'End Sub
        'Private Sub btnCategory2_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory2)
        'End Sub
        'Private Sub btnCategory3_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory3)
        'End Sub
        'Private Sub btnCategory4_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory4)
        'End Sub
        'Private Sub btnCategory5_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory5)
        'End Sub
        'Private Sub btnCategory6_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory6)
        'End Sub
        'Private Sub btnCategory7_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory7)
        'End Sub
        'Private Sub btnCategory8_Click(sender As System.Object, e As System.EventArgs)
        '    SelectCategoryFromButton(btnCategory8)
        'End Sub

        'Public Sub SelectCategoryFromButton(btnPressed As DevExpress.XtraEditors.SimpleButton)
        '    Dim iLevel As Integer
        '    Dim eBayCategoryToSelect As eBayCategory

        '    iLevel = CInt(btnPressed.Name.Substring(11))
        '    eBayCategoryToSelect = GetSelectedCategory(iLevel)
        '    UpdateEbayCategories(eBayCategoryToSelect.CategoryID, eBayCategoryToSelect.CategoryName, iLevel)
        '    lblEbayStatus.Text = ""
        '    ' Update the form buttons
        '    'UpdateCategoryButtons()
        '    'UpdateSelectedCategoryButtons()
        '    UpdateBreadcrumbs()
        'End Sub

        Public Function GetSelectedCategory(iDepth As Integer) As eBayCategory
            Dim ReturnCategory As eBayCategory = Nothing
            Dim OneCategory As eBayCategory

            For Each OneCategory In oeBayWrapper.aCategoriesSelected
                If OneCategory.SelectedOrder = iDepth Then
                    ReturnCategory = OneCategory
                    Exit For
                End If
            Next

            Return ReturnCategory
        End Function
        Public Function UpdateEbayCategories(iCategory As Integer, CategoryName As String, Optional iDepth As Integer = 0) As Integer

            Dim ParentCategory As eBayCategory
            Dim OneCategory As eBayCategory
            'Dim CategoryToRemove As eBayCategory

            Dim aCategoriesToAdd As New ArrayList
            Dim aSelectedCategories As New ArrayList
            Dim iLoopCount As Integer

            aCategoriesToAdd.Clear()

            ParentCategory = GetOneCategory(iCategory, CategoryName)

            For Each OneCategory In oeBayWrapper.aCategories
                If OneCategory.CategoryParentID = ParentCategory.CategoryID And OneCategory.CategoryName <> ParentCategory.CategoryName Then
                    aCategoriesToAdd.Add(OneCategory)
                End If
            Next

            If iDepth > 0 Then
                ' Direct selection of this category, so trim the arrays down and set the depth
                aSelectedCategories.Clear()
                For iLoopCount = 1 To iDepth
                    aSelectedCategories.Add(GetSelectedCategory(iLoopCount))
                Next

                ' Now repopulate the selected array
                oeBayWrapper.aCategoriesSelected.Clear()
                For Each OneCategory In aSelectedCategories
                    oeBayWrapper.aCategoriesSelected.Add(OneCategory)
                Next

                ' Set the new depth
                iSelectedCategoryDepth = iDepth
            End If

            If aCategoriesToAdd.Count > 0 Then
                imgListeBayCategory.Items.Clear()
                For Each OneCategory In aCategoriesToAdd
                    imgListeBayCategory.Items.Add(OneCategory.CategoryName, OneCategory.CategoryID)
                Next
            End If

            Return aCategoriesToAdd.Count
        End Function

        Private Sub imgListeBayCategory_DoubleClick(sender As Object, e As System.EventArgs) Handles imgListeBayCategory.DoubleClick
            Dim sVal As String = ""
            Dim iVal As Integer = 0
            Dim iSubCategoriesAdded As Integer = 0

            Dim SelectedCategory As eBayCategory

            If bManualCategories Then
                ' Pull out the selected text and category ID
                sVal = imgListeBayCategory.SelectedItem.ToString()
                iVal = imgListeBayCategory.Items(imgListeBayCategory.SelectedIndex).ImageIndex

                ' Now go and get the child categories and add to listbox
                iSubCategoriesAdded = UpdateEbayCategories(iVal, sVal)

                If iSubCategoriesAdded > 0 Then
                    iSelectedCategoryDepth = iSelectedCategoryDepth + 1 ' Increment the selection counter

                    ' Get the full category info
                    SelectedCategory = GetOneCategory(iVal, sVal)
                    ' Store the selected depth
                    SelectedCategory.SelectedOrder = iSelectedCategoryDepth

                    ' Store the category
                    oeBayWrapper.aCategoriesSelected.Add(SelectedCategory)
                    lblEbayStatus.Text = ""
                Else
                    lblEbayStatus.Text = "No more sub-categories"
                End If

                ' Update the form buttons
                'UpdateCategoryButtons()
                'UpdateSelectedCategoryButtons()
                UpdateBreadcrumbs()

            End If

        End Sub

        Private Sub cmdBrowseCategories_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowseCategories.Click
            Dim bSuccess As Boolean = True
            bManualCategories = True
            Dim category As eBayCategory

            iSelectedCategoryDepth = 0
            oeBayWrapper.aCategoriesSelected.Clear()

            bSuccess = GetCategoriesFromeBay()
            If oeBayWrapper.aCategories.Count = 0 Then
                lblEbayStatus.Text = "Retrieving categories from eBay"
                Application.DoEvents()

                bSuccess = GetCategoriesFromDatabase()

                lblEbayStatus.Text = ""
                Application.DoEvents()

            End If

            If bSuccess Then
                imgListeBayCategory.Items.Clear()

                Try
                    For Each category In oeBayWrapper.aCategories
                        ' Look for anything with CategoryLevel 1 and add to list
                        If category.CategoryLevel = 1 Then
                            imgListeBayCategory.Items.Add(category.CategoryName, category.CategoryID)
                        End If
                    Next

                Catch ex As Exception
                    bSuccess = False

                End Try
                'UpdateCategoryButtons()
                'UpdateSelectedCategoryButtons()
                UpdateBreadcrumbs()
            End If
        End Sub

        Private Sub btnRemoveCategory_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveCategory.Click
            RemoveCategoryFromList()
            RefreshFormControls()
        End Sub
        Private Function VerifyAndAddItem() As Boolean

            Dim bSuccess As Boolean = True
            Dim sResponse As String = ""
            Dim seBayFunction As String = "VerifyAddItem"
            ' Due to the amount of XML and form properties required for this we generate it here and pass it into the wrapper VerifyAddItem() method.
            Dim sXML As String

            MemoEditPublishStatus.Enabled = False
            MemoEditPublishStatus.Text = ""

            sXML = GenerateAddItemXML(seBayFunction)

            bSuccess = VerifyAddItem(sXML)
            If bSuccess Then
                ' We posted XML and got a response, parse the response
                bSuccess = oeBayWrapper.ParseAddItemResponseXML(oeBayWrapper.ReturnedData, seBayFunction)

                UpdatePostStatus(seBayFunction)
            End If

            If bSuccess Then
                ' The test post worked, now actually post it
                seBayFunction = "AddItem"
                sXML = GenerateAddItemXML(seBayFunction)
                bSuccess = AddItem(sXML)

                UpdatePostStatus(seBayFunction)

            End If

            Return bSuccess

        End Function

        Private Sub UpdatePostStatus(seBayFunction As String)
            Dim bSuccess As Boolean, sStatus As String, dFeeTot As Decimal, OneError As AddeBayItemError, OneFee As eBayFee = Nothing

            ' We posted XML and got a response, parse the response
            bSuccess = oeBayWrapper.ParseAddItemResponseXML(oeBayWrapper.ReturnedData, seBayFunction)

            If bSuccess Then
                ' Item added OK
                ' Add up the listing value
                For Each OneFee In oeBayWrapper.aeBayPostedItemFees
                    dFeeTot = dFeeTot + OneFee.Fee
                Next

                If Microsoft.VisualBasic.Left(seBayFunction.ToUpper, 6) = "VERIFY" Then
                    sStatus = "Test" & vbCrLf
                Else
                    sStatus = "Live"
                End If
                sStatus = sStatus & " insertion to eBay successfull" & vbCrLf & vbCrLf

                sStatus = sStatus & "Item ID: " & OneFee.ItemID.ToString & vbCrLf
                sStatus = sStatus & "Total insertion fees: " & dFeeTot.ToString & vbCrLf
            Else
                ' Item failed. Get failure reason(s)
                sStatus = ""

                If Microsoft.VisualBasic.Left(seBayFunction.ToUpper, 6) = "VERIFY" Then
                    sStatus = "Test" & vbCrLf
                Else
                    sStatus = "Live"
                End If
                sStatus = sStatus & " insertion to eBay failed." & vbCrLf & vbCrLf

                For Each OneError In oeBayWrapper.m_AddeBayItemError
                    sStatus = OneError.ShortMessage & vbCrLf
                    sStatus = sStatus & OneError.LongMessage & vbCrLf
                    sStatus = sStatus & "Error - number " & OneError.ErrorCode & vbCrLf
                    sStatus = sStatus & "Error - severity " & OneError.SeverityCode & vbCrLf
                    sStatus = sStatus & "Error - classification " & OneError.ErrorClassification & vbCrLf
                Next
            End If
            MemoEditPublishStatus.Enabled = False
            MemoEditPublishStatus.Text = sStatus

        End Sub

        Private Function VerifyAddItem(Optional sXMLToPost As String = "") As Boolean
            Dim bSuccess As Boolean = True
            Dim sXML As String
            Dim sResponseXML As String

            If sXMLToPost = "" Then
                ' Generate XML
                sXML = GenerateAddItemXML("VerifyAddItem")
            Else
                ' Post provided XML
                sXML = sXMLToPost
            End If

            bSuccess = oeBayWrapper.VerifyAddItem(sXML)
            ' Pasrse the returned XML

            sResponseXML = oeBayWrapper.ReturnedData

            Return bSuccess

        End Function

        Private Function AddItem(Optional sXMLToPost As String = "") As Boolean

            Dim bSuccess As Boolean = True
            Dim sXML As String

            If sXMLToPost = "" Then
                ' Generate XML
                sXML = GenerateAddItemXML("AddItem")
            Else
                ' Post provided XML
                sXML = sXMLToPost
            End If

            bSuccess = oeBayWrapper.AddItem(sXML)

            Return bSuccess
        End Function

        Private Function GenerateAddItemXML(eBayFunction As String) As String

            ' Due to the amount of XML and form properties required for this we generate it here and pass it into the wrapper VerifyAddItem() method.
            ' Memvars for properties
            Dim sXML As String
            'Dim eBayFunction As String = "VerifyAddItem"

            ' Need to get this from somewhere in Interprise.
            Dim sLocation As String = txtLocation.Text.Trim() ' "Cheadle Hulme"

            Dim oObject As DevExpress.XtraEditors.Controls.ImageComboBoxItem
            oObject = CType(cmbService.SelectedItem, ImageComboBoxItem)
            GetCategoryFeaturesForAll()
            Dim OneShippingService As clsShippingServiceDetails 'eBayXMLConnector.ShippingServiceDetails
            Dim iOneShippingService As Integer
            If Not oObject Is Nothing Then
                iOneShippingService = oObject.ImageIndex
            Else
                iOneShippingService = 0
            End If

            OneShippingService = oeBayWrapper.LookupShippingService(iOneShippingService)

            Dim sTitle As String = Me.txtDescriptiveTitle.Text.Trim
            Dim sDescription As String = ""
            Dim dPrice As Decimal = CDec(12.5)
            Dim iQuantity As Integer = 0

            Dim iCategoryCount As Integer = 0
            Dim OneCategory As eBayCategory
            Dim iCategoryID(aCategoriesToAddItemTo.Count - 1) As Integer
            Dim sPayPalEmail As String = txtPaypalEmail.Text.Trim
            Dim iDespatchTime As Integer

            Dim dPostCostToBuyer As Decimal
            Dim dAdditionalCostToBuyer As Decimal = 0

            Try
                dPostCostToBuyer = CDec(txtPostageCostToBuyer.Text)
            Catch ex As Exception
                dPostCostToBuyer = 0
            End Try

            dPrice = GetStartPrice()
            iQuantity = GetAuctionQuantity()
            ' Get the buy it now price
            Dim bBuyItNow As Boolean = chkAddBuyItNow.Checked
            Dim dBuyItNowPrice As Decimal = 0

            If bBuyItNow Then
                Try
                    dBuyItNowPrice = CDec(txtBuyItNowPrice.Text)
                Catch ex As Exception

                End Try
            End If

            oeBayWrapper.aConditionValues.Clear()
            For Each OneCategory In aCategoriesToAddItemTo
                iCategoryID(iCategoryCount) = OneCategory.CategoryID
                oeBayWrapper.GetCategoryFeaturesFromXML(OneCategory.GetCategoryFeaturesXML, OneCategory.CategoryID)
                iCategoryCount = iCategoryCount + 1
            Next

            ' Get the eBay site details
            Dim OneSiteDetails As SiteDetails
            OneSiteDetails = oeBayWrapper.LookupSiteDetails(CInt(oEbaySettings.eBayCountry))

            'iDespatchTime = CStr(cmbDispatchTime.Value)
            oObject = CType(cmbDispatchTime.SelectedItem, ImageComboBoxItem)

            If Not oObject Is Nothing Then
                iDespatchTime = oObject.ImageIndex
            Else
                iDespatchTime = 0
            End If

            ' Get the return policy
            Dim OneReturnAccepted As ReturnPolicyDetailsReturnsAccepted = Nothing
            oObject = CType(cmbReturnsPolicy.SelectedItem, ImageComboBoxItem)
            OneReturnAccepted = oeBayWrapper.GetReturnsPolicyFromID(oObject.ImageIndex)

            Dim sReturnText As String = ""
            If txtReturnsPolicyText.Enabled Then
                sReturnText = txtReturnsPolicyText.Text.Trim()
            End If

            ' Get the returns within
            Dim OneReturnPolicyDetailsReturnsWithin As ReturnPolicyDetailsReturnsWithin
            oObject = CType(cmbReturnWithin.SelectedItem, ImageComboBoxItem)
            OneReturnPolicyDetailsReturnsWithin = oeBayWrapper.GetReturnPolicyDetailsReturnsWithinFromID(oObject.ImageIndex)

            ' Get the refund options
            Dim OneRefund As ReturnPolicyDetailsRefund = Nothing
            oObject = CType(cmbRefundOptions.SelectedItem, ImageComboBoxItem)
            OneRefund = oeBayWrapper.GetRefundOptionsFromID(oObject.ImageIndex)

            ' Get the shipping costs
            Dim OneShippingCost As ReturnPolicyDetailsShippingCostPaidBy = Nothing
            oObject = CType(cmbShippingCostsPaidBy.SelectedItem, ImageComboBoxItem)
            OneShippingCost = oeBayWrapper.GetShippingCostsPaidByFromID(oObject.ImageIndex)

            ' Get the postal destination
            ' Populate cmbPostalDestination
            Dim OneShippingLocationDetails As ShippingLocationDetails = Nothing
            oObject = CType(cmbPostalDestination.SelectedItem, ImageComboBoxItem)
            OneShippingLocationDetails = oeBayWrapper.GetShippingLocationDetailsFromID(oObject.ImageIndex)

            ' Get the listing duration
            Dim OneListingDuration As CategoryListingDurations, iListingDuration As Integer
            oObject = CType(cmbLastingFor.SelectedItem, ImageComboBoxItem)
            iListingDuration = oObject.ImageIndex
            OneListingDuration = oeBayWrapper.GetDurationFromID(iListingDuration)

            '' Get the shipping costs paid by
            'Dim OneShippingCosts As ReturnPolicyDetailsShippingCostPaidBy
            'oObject = CType(cmbShippingCostsPaidBy.SelectedItem, ImageComboBoxItem)
            'iListingDuration = oObject.ImageIndex
            'OneShippingCosts = oeBayWrapper.GetShippingCostsPaidByFromID(iListingDuration)


            ' Pull the eBay last for details

            ' Get the primary category
            Dim PrimaryCategory As eBayCategory
            PrimaryCategory = GetSelectedCategoryFromForm(iCategoryID(0))

            ' Pull in the template (if any)
            Dim sTemplate As String = ""
            If MemoHTMLTemplate.Text.Trim <> "" Then
                sTemplate = oeBayWrapper.ConvertForXML(MemoHTMLTemplate.Text)
            End If

            ' This XML works for VerifyAddItem
            '<?xml version="1.0" encoding="utf-8"?>
            '<VerifyAddItemRequest xmlns="urn:ebay:apis:eBLBaseComponents">
            '    <RequesterCredentials>
            '        <eBayAuthToken>AgAAAA**AQAAAA**aAAAAA**htIsUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4CoAZmFowidj6x9nY+seQ**ZqIBAA**AAMAAA**kriTsHLflcRuxD6ejQraGlS1IDRyMNdOgW3SNcHiUYsMzIkkMg67mNRbvSfj1JqH23GvED0aPVOa7O2P1tcMQ6EgVKObT913HCW3qyVyi3cMtgA079hlw6DsL0ha6E+b5dXgq0rx7oWq7oKAf90TeeRUU66TL5TKomvgFeNoeVOuPKl/r3vzgBSel5hoya6i7TdDkn08VoW3xdE3NohUuiXXAE7BMPOuMKaahUqAj49moAHdbZVF02tXdnL3nYzB8/sefL0B1XZ06zEO8i/AqC6Z0LehlVStfY1MRxDOU3pK34mabeE01ZZXQ8FhB9F687VbDdtOTXET/YJimpotYi96H8JGdPd5xVa3VKjHfr9YplnxFHIsWTB9JjuPxw6SJIo0LBFFymWD3MCQLSjSztvPX5+tLcY9jAzwECQdiByT7K6LGS/wWmHL1/c2AWCalPOaAavNV2rBd5C/xZ3ElzMw72TzhzYP55C+3xjJVkOXqhqVg/rkMs402OmXZCrtWcH4jHreYVi3B+gqrDos98nTSGrnQcIAtVSDjnnGG4zEsHYG1UvPBmZ5piOGGr00vfP8q1uiF1nvA19YiRpRH6YWHotfNhS7VVt870KvUmHZ0jg1srUPnzPMtFdazMy0iSaF7SXOFIRJDgYvODLsecoLf9ddsrhL/Svgqsxt3RSxy1TzJio/P25IO9jX3qku9kZnMioXDOc7HMtl5AfJBKJ3hXCb0i4qYZATbV9F3hF5JbHjG24c2HqrMJyatdrX</eBayAuthToken>
            '    </RequesterCredentials>
            '    <Item>
            '        <ShippingDetails>
            '            <ShippingServiceOptions>
            '                <ShippingService>UK_RoyalMailSecondClassStandard</ShippingService>
            '                <ShippingServiceCost currencyID="GBP">5.0</ShippingServiceCost>
            '                <ShippingServiceAdditionalCost currencyID="GBP">0.0</ShippingServiceAdditionalCost>
            '                <ShippingServicePriority>1</ShippingServicePriority>
            '                <ExpeditedService>false</ExpeditedService>
            '                <ShippingTimeMin>2</ShippingTimeMin>
            '                <ShippingTimeMax>3</ShippingTimeMax>
            '            </ShippingServiceOptions>
            '        </ShippingDetails>
            '        <CategoryBasedAttributesPrefill>true</CategoryBasedAttributesPrefill>
            '        <CategoryMappingAllowed>true</CategoryMappingAllowed>
            '        <Country>GB</Country>
            '        <Currency>GBP</Currency>
            '        <Description>eBay Sample Listing</Description>
            '        <ListingDuration>Days_10</ListingDuration>
            '        <ListingType>FixedPriceItem</ListingType>
            '        <Location>Cheadle Hulme</Location>
            '        <PaymentMethods>PayPal</PaymentMethods>
            '        <PayPalEmailAddress>ebay-user1@lerryn.com</PayPalEmailAddress>
            '        <PictureDetails>
            '            <PictureURL>http://example.com/f37_2.JPG</PictureURL>
            '        </PictureDetails>
            '        <PrimaryCategory>
            '            <CategoryID>111422</CategoryID>
            '        </PrimaryCategory>
            '        <Quantity>1</Quantity>
            '        <Site>UK</Site>
            '        <StartPrice currencyID="GBP">100.0</StartPrice>
            '        <Title>Sample Listing</Title>
            '        <ReturnPolicy>
            '            <ReturnsAcceptedOption>ReturnsAccepted</ReturnsAcceptedOption>
            '            <RefundOption>MoneyBack</RefundOption>
            '            <ReturnsWithinOption>Days_30</ReturnsWithinOption>
            '            <Description>If you are not happy, return the item for refund.</Description>
            '            <ShippingCostPaidByOption>Buyer</ShippingCostPaidByOption>
            '        </ReturnPolicy>
            '        <DispatchTimeMax>3</DispatchTimeMax>
            '        <ConditionID>1000</ConditionID>
            '       </Item>
            '</VerifyAddItemRequest>

            ' Build XML 
            sXML = oeBayWrapper.GetXMLHeader(eBayFunction, True)

            sXML = sXML & "<Item>"

            ' Shipping details
            sXML = sXML & "<ShippingDetails>"
            sXML = sXML & "<ShippingServiceOptions>"
            sXML = sXML & "<ShippingService>" & OneShippingService.ShippingService & "</ShippingService>"
            If dPostCostToBuyer > 0 Then
                sXML = sXML & "<ShippingServiceCost currencyID=""" & m_EBayCurrency & """>" & dPostCostToBuyer.ToString & "</ShippingServiceCost>"
            End If
            sXML = sXML & "<ShippingServiceAdditionalCost currencyID=""" & m_EBayCurrency & """>" & dAdditionalCostToBuyer.ToString & "</ShippingServiceAdditionalCost>"
            ' Not sure where this is specified
            sXML = sXML & "<ShippingServicePriority>1</ShippingServicePriority>"
            sXML = sXML & "<ExpeditedService>false</ExpeditedService>"
            sXML = sXML & "<ShippingTimeMin>" & OneShippingService.ShippingTimeMin.ToString & "</ShippingTimeMin>" ' 2
            sXML = sXML & "<ShippingTimeMax>" & OneShippingService.ShippingTimeMax.ToString & "</ShippingTimeMax>" ' 3
            sXML = sXML & "</ShippingServiceOptions>"
            sXML = sXML & "</ShippingDetails>"

            sXML = sXML & "<CategoryBasedAttributesPrefill>true</CategoryBasedAttributesPrefill>"
            sXML = sXML & "<CategoryMappingAllowed>" & PrimaryCategory.AllowCategoryMapping.ToString.ToLower & "</CategoryMappingAllowed>"

            sXML = sXML & "<Country>" & m_EBayCountryString & "</Country>"
            sXML = sXML & "<Currency>" & m_EBayCurrency & "</Currency>"

            ' This accepts HTML so I *think* you can send the template from here
            If sTemplate <> "" Then
                sXML = sXML & "<Description>" & sTemplate & "</Description>"
            Else
                sXML = sXML & "<Description>" & m_InterpriseStockItem.Description.Trim() & "</Description>"
            End If

            sXML = sXML & "<ListingDuration>" & OneListingDuration.Duration & "</ListingDuration>" ' Days_10

            ' Need to expose this on UI
            sXML = sXML & "<ListingType>FixedPriceItem</ListingType>"
            sXML = sXML & "<Location>" & sLocation & "</Location>"

            ' Need to expose this on UI
            sXML = sXML & "<PaymentMethods>PayPal</PaymentMethods>"
            sXML = sXML & "<PayPalEmailAddress>" & sPayPalEmail & "</PayPalEmailAddress>"

            ' Need to pull these from Interprise
            sXML = sXML & "<PictureDetails>"
            sXML = sXML & "<PictureURL>http://example.com/f37_2.JPG</PictureURL>"
            sXML = sXML & "</PictureDetails>"

            sXML = sXML & "<PrimaryCategory>"
            sXML = sXML & "<CategoryID>" & PrimaryCategory.CategoryID.ToString & "</CategoryID>"
            sXML = sXML & "</PrimaryCategory>"

            ' Add secondary categories here
            Dim CatToAdd As Integer
            Dim CategoryToAdd As eBayCategory

            For Each CatToAdd In iCategoryID
                If CatToAdd <> PrimaryCategory.CategoryID Then
                    CategoryToAdd = GetSelectedCategoryFromForm(CatToAdd)

                    ' Add it
                    sXML = sXML & "<SecondaryCategory>"
                    sXML = sXML & "<CategoryID>" & CategoryToAdd.CategoryID.ToString & "</CategoryID>"
                    sXML = sXML & "</SecondaryCategory>"
                End If
            Next

            '<SecondaryCategory> CategoryType
            '<CategoryID> string </CategoryID>

            ''AutoPayEnabled Of Boolean
            ''B2BVATEnabled Of Boolean
            ''BestOfferEnabled Of Boolean
            ''CatalogEnabled(Of Boolean
            ''CategoryID  String
            ''CategoryLevel int
            ''CategoryName String
            ''CategoryParentID Of String
            ''CategoryParentName Of String
            ''CharacteristicsSets(CharacteristicsSetType)
            ''Expired Of Boolean
            ''IntlAutosFixedCat Boolean
            ''Keywords
            ''LeafCategory Boolean
            ''LSD Boolean
            ''NumOfItems
            ''ORPA Boolean
            ''ORRA Boolean
            ''ProductFinderIDs(ExtendedProductFinderIDType)
            ''ProductSearchPageAvailable Boolean
            ''SellerGuaranteeEligible Boolean
            ''Virtual Boolean
            '</SecondaryCategory>

            sXML = sXML & "<Quantity>" & iQuantity.ToString & "</Quantity>"
            sXML = sXML & "<Site>" & OneSiteDetails.Site & "</Site>"
            sXML = sXML & "<StartPrice currencyID=""" & m_EBayCurrency & """>" & dPrice.ToString & "</StartPrice>"
            sXML = sXML & "<Title>" & m_InterpriseStockItem.ItemName & "</Title>"

            ' This needs to be set globally

            ' We have a return policy

            sXML = sXML & "<ReturnPolicy>"
            If OneReturnAccepted.LerrynID > 0 Then
                sXML = sXML & "<ReturnsAcceptedOption>" & OneReturnAccepted.ReturnsAcceptedOption & "</ReturnsAcceptedOption>"
                sXML = sXML & "<ReturnsWithinOption>" & OneReturnPolicyDetailsReturnsWithin.ReturnsWithinOption & "</ReturnsWithinOption>" ' Days_30
            End If

            If OneRefund.LerrynID > 0 Then
                sXML = sXML & "<RefundOption>" & OneRefund.RefundOption & "</RefundOption>" ' MoneyBack
            End If

            'sXML = sXML & "<Description>If you are not happy, return the item for refund.</Description>"
            sXML = sXML & "<Description>" & oeBayWrapper.ConvertForXML(sReturnText) & "</Description>"
            sXML = sXML & "<ShippingCostPaidByOption>Buyer</ShippingCostPaidByOption>"
            sXML = sXML & "</ReturnPolicy>"

            'sXML = sXML & "<ReturnPolicy>"
            'sXML = sXML & "<ReturnsAcceptedOption>ReturnsAccepted</ReturnsAcceptedOption>"
            'sXML = sXML & "<RefundOption>MoneyBack</RefundOption>"
            'sXML = sXML & "<ReturnsWithinOption>Days_30</ReturnsWithinOption>"
            'sXML = sXML & "<Description>If you are not happy, return the item for refund.</Description>"
            'sXML = sXML & "<ShippingCostPaidByOption>Buyer</ShippingCostPaidByOption>"
            'sXML = sXML & "</ReturnPolicy>"

            sXML = sXML & "<DispatchTimeMax>" & iDespatchTime.ToString & "</DispatchTimeMax>"
            sXML = sXML & "<ConditionID>" & PrimaryCategory.SelectedConditionID.ToString & "</ConditionID>"

            If bBuyItNow Then
                ' Add buy it now XML
                sXML = sXML & "<BuyItNowPrice currencyID=""" & m_EBayCurrency & """>" & dBuyItNowPrice.ToString & "</BuyItNowPrice>"
            End If

            sXML = sXML & "</Item>"
            sXML = sXML & "</" & eBayFunction & "Request>"

            Return sXML
        End Function

        Private Sub cmdPublish_Click(sender As System.Object, e As System.EventArgs) Handles cmdPublish.Click
            VerifyAndAddItem()
        End Sub

        Private Function GetStartPrice() As Decimal

            Dim dPrice As Decimal = 0
            Dim bPCOFInterprisePrice As Boolean = chkPercentOfInterpriseSellingPrice.Checked    ' Start price is % of interprise selling price?
            Dim iPCOFInterprisePrice As Integer = CInt(spnPercentOfInterpriseSellingPrice.Value) ' % of interprise selling price
            Dim dInterpriseSellingPrice As Decimal = 0
            Dim dStartAuctionPrice As Decimal = CDec(txtStartAuction.Text)

            If bPCOFInterprisePrice Then
                ' We set the start price as a % of interprise selling price
                If dInterpriseSellingPrice > 0 Then
                    dPrice = (dInterpriseSellingPrice / 100) * iPCOFInterprisePrice
                End If
            Else
                ' Take the given form start price
                dPrice = dStartAuctionPrice
            End If

            Return dPrice

        End Function

        Private Function GetAuctionQuantity() As Integer
            Dim iQuantity As Integer
            Dim iInterpriseStockQuantity As Integer = 0

            Select Case rdoAuctionQuantity.SelectedIndex
                Case 0  ' None
                    iQuantity = 0

                Case 1 ' Fixed value
                    iQuantity = CInt(spnPercentOfTotal.Value)

                Case 2 ' % of total
                    iQuantity = CInt((iInterpriseStockQuantity / 100) * CInt(spnPercentOfTotal.Value))
            End Select

            Return iQuantity

        End Function

        Public Function GetCategoryFeaturesForAll() As Boolean

            Dim bSuccess As Boolean = True
            Dim OneCategory As eBayCategory
            Dim OutCategories As New ArrayList
            Dim sDescription As String = Me.txtSuggestedCategories.Text.Trim()
            Dim OneCatalogEnabled As CategoryCatalogEnabled
            Dim bCatalogEnabled As Boolean = False

            OutCategories.Clear()

            For Each OneCategory In aCategoriesToAddItemTo

                If OneCategory.GetCategoryFeaturesXML Is Nothing Then
                    If GetCategoryFeatures(OneCategory.CategoryID) Then
                        OneCategory.GetCategoryFeaturesXML = oeBayWrapper.ReturnedData
                    End If
                End If

                ' Now run FindProducts on our new category
                If OneCategory.FindProductsXML Is Nothing Then
                    If oeBayWrapper.FindProducts(OneCategory.CategoryID) Then
                        ' Add the FindProducts XML to the ebay category
                        OneCategory.FindProductsXML = oeBayWrapper.ReturnedData

                        ' Parse the category into it's objects
                        oeBayWrapper.ParseFindProductsXML(OneCategory.FindProductsXML, OneCategory.CategoryID)

                        ' Now populate the CatalogProudcts flag for the category
                        For Each OneCatalogEnabled In oeBayWrapper.CatalogEnabled
                            If OneCatalogEnabled.CategoryID = OneCategory.CategoryID Then
                                OneCategory.CatalogEnabled = OneCatalogEnabled.CatalogEnabled
                                Exit For
                            End If
                        Next
                    End If
                End If

                OutCategories.Add(OneCategory)
            Next

            aCategoriesToAddItemTo = OutCategories

            Return bSuccess

        End Function

        Public Function GetCategoryFeatures(iCategoryID As Integer) As Boolean

            Dim XMLToPost As String, eBayFunction As String = "GetCategoryFeatures"
            Dim bSuccess As Boolean = True

            XMLToPost = oeBayWrapper.GetXMLHeader(eBayFunction, True)

            XMLToPost = XMLToPost & "<CategoryID>" & iCategoryID.ToString & "</CategoryID>"
            XMLToPost = XMLToPost & "<DetailLevel>ReturnAll</DetailLevel>"
            XMLToPost = XMLToPost & "<ViewAllNodes>true</ViewAllNodes>"
            XMLToPost = XMLToPost & "</" & eBayFunction & "Request>"

            If oeBayWrapper.PostXML(XMLToPost, eBayFunction) Then
                Try

                Catch ex As Exception

                End Try
            Else
                bSuccess = False
            End If

            Return bSuccess

        End Function

        Private Sub btnBrowseNodeUpLeveleBay_Click(sender As System.Object, e As System.EventArgs) Handles btnBrowseNodeUpLeveleBay.Click
            GoUpOneLevel()
        End Sub

        Private Sub imgListeBayCategoriesToAdd_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles imgListeBayCategoriesToAdd.SelectedIndexChanged
            ' Populate the category specific controls
            GetCategoryFeaturesForAll()
            PopulateCategorySpecificControls()
        End Sub

        Public Sub PopulateCategorySpecificControls()
            Dim OneCategory As eBayCategory
            Dim OneCondition As ConditionValues
            Dim iCategoryID As Integer
            Dim sVal As String, iVal As Integer
            Dim iConditionID As Integer = 0
            'Dim iConditionCount As Integer = 0
            Dim NewValue As String = ""
            Dim bReturnPolicyEnabled As Boolean

            Dim sCategoryFeatureValue As String = ""

            ' Determine the selected category from the imgList
            ' Pull out the selected text and category ID from the UI
            If Not imgListeBayCategoriesToAdd.SelectedItem Is Nothing Then
                sVal = imgListeBayCategoriesToAdd.SelectedItem.ToString()
                iVal = imgListeBayCategoriesToAdd.Items(imgListeBayCategoriesToAdd.SelectedIndex).ImageIndex

                ' Get the category, category ID and condition ID
                OneCategory = GetOneSelectedCategory(iVal)
                iCategoryID = OneCategory.CategoryID
                iConditionID = OneCategory.SelectedConditionID
                NewValue = OneCategory.SelectedCategoryText

                If OneCategory.CatalogEnabled Then
                    Me.UpdateStatus("This category is catalog enabled")
                Else
                    Me.UpdateStatus("")
                End If

                oeBayWrapper.GetCategoryFeaturesFromXML(OneCategory.GetCategoryFeaturesXML, OneCategory.CategoryID)

                ' Get the ReturnsPolicy status for the category
                bReturnPolicyEnabled = oeBayWrapper.ProcessSiteDefaultsForCategory(OneCategory.CategoryID, OneCategory.GetCategoryFeaturesXML)
                sCategoryFeatureValue = oeBayWrapper.LookupCategoryFeature(OneCategory.CategoryID, "ReturnPolicyEnabled")
                If sCategoryFeatureValue.ToUpper = "TRUE" Then
                    bReturnPolicyEnabled = True
                Else
                    bReturnPolicyEnabled = False
                End If
                AddReturnPolicyToCategory(OneCategory.CategoryID, bReturnPolicyEnabled)

                ' Now parse the structures for the category to populate the UI

                ' Ensure the category count is up to date
                CountConditionsForCategory()

                cmbCondition.Properties.Items.Clear()
                m_bIgnoreChange = True
                cmbCondition.SelectedIndex = -1
                cmbCondition.EditValue = ""
                For Each OneCondition In oeBayWrapper.aConditionValues
                    If OneCondition.CategoryID = iCategoryID Then
                        ' iConditionCount = iConditionCount + 1
                        With cmbCondition
                            .Properties.Items.Add(New ImageComboBoxItem(OneCondition.DisplayName, OneCondition.ID))
                        End With
                    End If
                Next

                If NewValue <> "" Then
                    m_bIgnoreChange = True
                    ' Change the condition combo selected entry to the condition ID for the category
                    ' Make the change
                    cmbCondition.EditValue = NewValue
                End If
                m_bIgnoreChange = False

                ' Set the allow category mapping checkbox
                chkAllowCategoryMapping.Checked = OneCategory.AllowCategoryMapping

                ' Now populate the duration
                Dim OneCategoryListingDuration As CategoryListingDurations
                Dim iDurationCount As Integer = 0
                'Dim iUnderscorePos As Integer

                ' We need to get a ListingDuration ID from the SiteDefaults
                Dim OneSiteDefault As SiteDefaults, sType As String = "", iType As Integer = 0
                For Each OneSiteDefault In oeBayWrapper.aSiteDefaults
                    If OneSiteDefault.TagName.ToUpper = "LISTINGDURATION" And OneSiteDefault.Type.ToUpper = "LIVE" Then
                        sType = OneSiteDefault.Value
                        Try
                            iType = CInt(sType)
                        Catch ex As Exception

                        End Try
                    End If
                Next

                cmbLastingFor.Properties.Items.Clear()

                For Each OneCategoryListingDuration In oeBayWrapper.aCategoryListingDurations

                    If OneCategoryListingDuration.SiteID = oeBayWrapper.eBayCountry And OneCategoryListingDuration.ID = iType Then

                        If iDurationCount = 0 Then
                            iCategoryID = OneCategoryListingDuration.CategoryID
                        End If
                        If OneCategoryListingDuration.CategoryID = iCategoryID Then
                            ' Add to combo
                            'sDuration = ""
                            'If InStr(OneCategoryListingDuration.Duration.ToUpper, "DAY") > 0 Then
                            '    ' Hack Days_n to n Days
                            '    iUnderscorePos = InStr(OneCategoryListingDuration.Duration, "_")
                            '    sTemp = OneCategoryListingDuration.Duration.Substring(iUnderscorePos)
                            '    Try
                            '        iTemp = CInt(sTemp.Trim)
                            '    Catch ex As Exception
                            '        iTemp = 0
                            '        sDuration = OneCategoryListingDuration.Duration
                            '    End Try

                            '    If iTemp > 0 Then
                            '        sDuration = iTemp.ToString & " Day"
                            '        If iTemp <> 1 Then
                            '            sDuration = sDuration & "s"
                            '        End If
                            '    End If
                            'Else
                            '    sDuration = OneCategoryListingDuration.Duration
                            'End If
                            With cmbLastingFor
                                .Properties.Items.Add(New ImageComboBoxItem(OneCategoryListingDuration.Duration_Screen, OneCategoryListingDuration.LerrynID))
                            End With

                        End If
                    End If

                Next
            End If

            RefreshFormControls()

        End Sub

        Public Function AddReturnPolicyToCategory(ByRef iCategoryID As Integer, ByRef bReturnPolicyEnabled As Boolean) As Boolean
            Dim bSuccess As Boolean = True
            Dim OneCategoryToModify As eBayCategory
            OneCategoryToModify = GetSelectedCategoryFromForm(iCategoryID)

            If OneCategoryToModify.CategoryID = iCategoryID And OneCategoryToModify.ReturnPolicyEnabled <> bReturnPolicyEnabled Then
                OneCategoryToModify.ReturnPolicyEnabled = bReturnPolicyEnabled ' Modify it
                SetOneSelectedCategory(OneCategoryToModify) ' Store it
                bSuccess = True
            End If

            Return bSuccess
        End Function

        Private Sub RefreshFormControls()
            Dim bEnabled As Boolean = False

            If Not imgListeBayCategoriesToAdd.SelectedItem Is Nothing Then
                ' There is a category selected

                If cmbCondition.Properties.Items.Count > 0 Then
                    ' There are conditions for the category
                    bEnabled = True
                Else
                    ' No conditions for the category
                    bEnabled = False
                End If
            End If

            cmbCondition.Enabled = bEnabled
            chkAllowCategoryMapping.Enabled = bEnabled

        End Sub

        Private Sub cmbCondition_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbCondition.SelectedIndexChanged
            Dim iCategoryID As Integer
            Dim sSelectedCondition As String, iSelectedCondition As Integer
            Dim oObject As DevExpress.XtraEditors.Controls.ImageComboBoxItem
            Dim OneCategoryToModify As eBayCategory

            If Not m_bIgnoreChange Then
                ' We need to put the selected condition into the category.SelectedConditionID
                ' Determine the selected category from the imgList
                ' Pull out the selected text and category ID from the UI

                iCategoryID = GetSelectedCategoryIDFromForm()
                OneCategoryToModify = GetSelectedCategoryFromForm(iCategoryID)

                ' Pull out the condition ID from the UI
                sSelectedCondition = cmbCondition.SelectedItem.ToString()
                oObject = CType(cmbCondition.SelectedItem, ImageComboBoxItem)
                iSelectedCondition = oObject.ImageIndex

                If OneCategoryToModify.CategoryID = iCategoryID Then
                    OneCategoryToModify.SelectedConditionID = iSelectedCondition ' Modify it
                    OneCategoryToModify.SelectedCategoryText = sSelectedCondition
                    SetOneSelectedCategory(OneCategoryToModify)
                End If

            End If

        End Sub

        Private Sub chkAllowCategoryMapping_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAllowCategoryMapping.CheckedChanged
            Dim bAllowCategoryMapping As Boolean = chkAllowCategoryMapping.Checked
            Dim OneCategoryToModify As eBayCategory, iCategoryID As Integer

            iCategoryID = GetSelectedCategoryIDFromForm()

            If iCategoryID > 0 Then
                OneCategoryToModify = GetSelectedCategoryFromForm(iCategoryID)

                If OneCategoryToModify.CategoryID = iCategoryID And OneCategoryToModify.AllowCategoryMapping <> bAllowCategoryMapping Then
                    ' Modify this category
                    OneCategoryToModify.AllowCategoryMapping = bAllowCategoryMapping ' Modify it
                    ' Save the category
                    SetOneSelectedCategory(OneCategoryToModify)
                End If
            End If

        End Sub

        Private Sub imgListeBayCategoriesToAdd_Enter(sender As System.Object, e As System.EventArgs) Handles imgListeBayCategoriesToAdd.Enter

            Me.UpdateStatus("")
            imgListeBayCategoriesToAdd_ToDefaults()

        End Sub

        Private Sub imgListeBayCategoriesToAdd_ToDefaults()
            ' Reset to defaults when got control
            imgListeBayCategoriesToAdd.BorderStyle = BorderStyles.Default
            imgListeBayCategoriesToAdd.Appearance.BorderColor = Color.Black
        End Sub

        Public Sub CountConditionsForCategory()
            Dim OneCondition As ConditionValues, OneCategory As eBayCategory
            Dim iConditionsForCategory As Integer = 0
            Dim iCategoryID(aCategoriesToAddItemTo.Count) As Integer
            Dim iLoopCount As Integer = 0
            Dim iOneCategoryID As Integer = 0

            ' Store the number of conditions for the category within the category structure
            For Each OneCategory In aCategoriesToAddItemTo
                iCategoryID(iLoopCount) = OneCategory.CategoryID
                iLoopCount = iLoopCount + 1
            Next

            For iLoopCount = 0 To aCategoriesToAddItemTo.Count - 1
                iOneCategoryID = iCategoryID(iLoopCount)
                ' Count the number of conditions in the category
                For Each OneCondition In oeBayWrapper.aConditionValues
                    If OneCondition.CategoryID = iOneCategoryID Then
                        iConditionsForCategory = iConditionsForCategory + 1
                    End If
                Next
                ' Store it away
                AddNumberOfConditionsToCategory(iOneCategoryID, iConditionsForCategory)
            Next

        End Sub
        Public Function AddNumberOfConditionsToCategory(ByRef iCategoryID As Integer, ByRef NoOfConditionsForCategory As Integer) As Boolean
            Dim bSuccess As Boolean = False

            Dim OneCategoryToModify As eBayCategory
            OneCategoryToModify = GetSelectedCategoryFromForm(iCategoryID)

            If OneCategoryToModify.CategoryID = iCategoryID And OneCategoryToModify.NumberOfConditionsForCategory <> NoOfConditionsForCategory Then
                OneCategoryToModify.NumberOfConditionsForCategory = NoOfConditionsForCategory ' Modify it
                SetOneSelectedCategory(OneCategoryToModify) ' Store it
                bSuccess = True
            End If

            Return bSuccess
        End Function

        Public Function AddCatalogEnabledToCategory(ByRef iCategoryID As Integer, ByRef bCatalogEnabled As Boolean) As Boolean
            Dim bSuccess As Boolean = False

            Dim OneCategoryToModify As eBayCategory
            OneCategoryToModify = GetSelectedCategoryFromForm(iCategoryID)

            If OneCategoryToModify.CategoryID = iCategoryID And OneCategoryToModify.CatalogEnabled <> bCatalogEnabled Then
                OneCategoryToModify.CatalogEnabled = bCatalogEnabled ' Modify it
                SetOneSelectedCategory(OneCategoryToModify) ' Store it
                bSuccess = True
            End If

            Return bSuccess
        End Function

        Private Sub cmdPasteXML_Click(sender As System.Object, e As System.EventArgs) Handles cmdPasteXML.Click
            Clipboard.SetText(Me.oeBayWrapper.PostedXML)
        End Sub

        Private Sub cmbReturnsPolicy_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbReturnsPolicy.SelectedIndexChanged
            Dim bEnabled As Boolean = True

            Select Case cmbReturnsPolicy.Text.ToUpper
                Case "NO RETURNS ACCEPTED"
                    bEnabled = False
                    ' Expecting more here
            End Select

            Me.txtReturnsPolicyText.Enabled = bEnabled

        End Sub

        Private Sub chkFlagAsComplete_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkFlagAsComplete1.CheckedChanged
            ' Flag the currently displayed category as "complete"
            Dim OneCategoryToSave As eBayCategory, OneCategoryBeingEditedID As Integer
            Dim OneCategoryBeingEdited As eBayCategory

            OneCategoryBeingEditedID = GetCategoryIDBeingEdited()

            OneCategoryBeingEdited = GetSelectedCategoryFromForm(OneCategoryBeingEditedID)

            If OneCategoryBeingEdited.CategoryID > 0 Then
                ' Pull the category object (in case edits have been made to it)
                OneCategoryToSave = GetSelectedCategoryFromForm(OneCategoryBeingEdited.CategoryID)
                OneCategoryToSave.CatalogPropertiesSelected = True ' The change we want to make
                SetOneSelectedCategory(OneCategoryToSave) ' This tucks the catalog object away back into aCategoriesToAddItemTo
            End If
        End Sub

        Public Function GetCategoryIDBeingEdited() As Integer
            Dim OneCategory As eBayCategory = Nothing
            Dim iCategoryID As Integer = 0

            If CatalogEnabledCategoryBeingViewed.Count > 0 Then
                iCategoryID = CInt(CatalogEnabledCategoryBeingViewed(0))
            End If

            Return iCategoryID
        End Function
    End Class
End Namespace
#End Region
