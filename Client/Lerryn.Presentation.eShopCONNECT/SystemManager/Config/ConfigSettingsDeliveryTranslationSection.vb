' eShopCONNECT for Connected Business
' Module: ConfigSettingsDeliveryTranslationSection.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
'
'       © 2012 - 2014  Lerryn Business Solutions Ltd
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
' Updated 02 April 2014

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const ' TJS 27/09/10
Imports Microsoft.VisualBasic ' TJS 30/12/09
Imports System.IO ' TJS 25/04/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 18/03/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " ConfigSettingsDeliveryTranslationSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, _
    "Lerryn.Presentation.ImportExport.ConfigSettingsDeliveryTranslationSection")> _
Public Class ConfigSettingsDeliveryTranslationSection

#Region " Variables "
    Private m_ConfigSettingsDeliveryTranslationDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_ConfigSettingsDeliveryTranslationSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_SourceCode As String
    Private m_SourceConfig As String ' TJS 30/12/09
    Private strOrigShipMthGrpCode As String = ""
    Private strOrigShipMthCode As String = ""
    Private bNewRowStarted As Boolean = False ' TJS 17/03/09
    Private ChannelAdvCarriers() As ChannelAdvCarrier ' TJS 30/12/09
    Private iChannelAdvCarrierCount As Integer ' TJS 30/12/09
    Private m_EBaySettings As Lerryn.Framework.ImportExport.SourceConfig.eBaySettings ' TJS 02/12/11
    Private m_EBayShippingCarriers() As Lerryn.Facade.ImportExport.eBayXMLConnector.eBayShippingCarriers ' TJS 02/12/11
    Private WithEvents m_BackgroundWorker As System.ComponentModel.BackgroundWorker ' TJS 02/12/11
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.ConfigSettingsDeliveryTranslationSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_ConfigSettingsDeliveryTranslationSectionFacade
        End Get
    End Property
#End Region

    Public WriteOnly Property SourceCode() As String
        Set(ByVal value As String)
            m_SourceCode = value
        End Set
    End Property

    Public WriteOnly Property SourceConfig() As String ' TJS 30/12/09
        Set(ByVal value As String) ' TJS 30/12/09
            m_SourceConfig = value ' TJS 30/12/09
        End Set
    End Property
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
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 30/01/09 | TJS             | 2009.1.03 | Modified to use PRODUCT_CODE, PRODUCT_NAME when creating Facade
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '------------------------------------------------------------------------------------------

        MyBase.New()

        Me.m_ConfigSettingsDeliveryTranslationDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        Me.m_ConfigSettingsDeliveryTranslationSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_ConfigSettingsDeliveryTranslationDataset, _
            New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 30/01/09 TJS 10/06/12
    End Sub

    Public Sub New(ByVal ConfigSettingsDeliveryTranslationDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
          ByVal ConfigSettingsDeliveryTranslationSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)

        MyBase.New()

        Me.m_ConfigSettingsDeliveryTranslationDataset = ConfigSettingsDeliveryTranslationDataset
        Me.m_ConfigSettingsDeliveryTranslationSectionFacade = ConfigSettingsDeliveryTranslationSectionFacade

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return
    End Sub
#End Region

#Region " InitializeControl "
    Protected Overrides Sub InitializeControl()
        'This call is required by the Presentation Layer.
        MyBase.InitializeControl()

        'Add any initialization after the InitializeControl() call

    End Sub
#End Region

#Region " ResetNewRow "
    Public Sub ResetNewRow() ' TJS 17/03/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | function added
        '------------------------------------------------------------------------------------------

        bNewRowStarted = False
        Me.GridViewDeliveryTranslation.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom

    End Sub
#End Region

#Region " GetChannelAdvShippingCarrierList "
    Public Sub GetChannelAdvShippingCarrierList()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/12/09 | TJS             | 2010.0.00 | Function added
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for Source Code constants
        ' 27/09/10 | TJs             | 2010.1.02 | Modified to use PopulateShippingClassList ApplyFilter parameter 
        '                                        | to initialise Channel Advisor Source Delivary Class listbox with 
        '                                        | all values so data is displayed in full
        ' 12/10/10 | TJS             | 2010.1.06 | Modified to include CA Account details in error message
        ' 03/12/10 | FA              | 2010.1.10 | Moved initialization of ChannelAdvisorCarrierCount to before the
        '                                        | For loop as this was over-writing values
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 02/04/14 | TJS             | 2014.0.01 | Corrected ContentType header setting and added missing namespace manager
        '------------------------------------------------------------------------------------------

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader ' TJS 02/12/11
        Dim XMLConfig As XDocument, XMLResponse As XDocument, XMLTemp As XDocument
        Dim XMLAccountList As System.Collections.Generic.IEnumerable(Of XElement), XMLCarrierList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLAccountID As XElement, XMLCarrier As XElement
        Dim XMLNSManCAShipping As System.Xml.XmlNamespaceManager, XMLNameTabCAShipping As System.Xml.NameTable ' TJS 02/04/14
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim strSubmit As String, strReturn As String, iLoop As Integer, iCheckLoop As Integer
        Dim bAlreadyLoaded As Boolean

        If m_SourceCode = CHANNEL_ADVISOR_SOURCE_CODE Then ' TJS 19/08/10
            XMLConfig = XDocument.Parse(m_SourceConfig)

            XMLAccountList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)

            'FA/TJS 03/12/2010
            'FA 03/12/2010 moved before the For loop as this was over-writing values
            'if more than one account in eShopConnect
            iChannelAdvCarrierCount = 0
            ReDim ChannelAdvCarriers(iChannelAdvCarrierCount)
            For Each XMLAccountID In XMLAccountList
                XMLTemp = XDocument.Parse(XMLAccountID.ToString)
                ' did config XML load correctly
                If XMLTemp.ToString <> "" Then
                    strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" "
                    strSubmit = strSubmit & "xmlns:web=""http://api.channeladvisor.com/webservices/"">"
                    If "" & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY) <> "" Then
                        strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY)
                        strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_PWD)
                    Else
                        strSubmit = strSubmit & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY
                        strSubmit = strSubmit & "</web:DeveloperKey><web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                    End If
                    strSubmit = strSubmit & "</web:Password></web:APICredentials></soapenv:Header><soapenv:Body><web:GetShippingCarrierList><web:accountID>"
                    strSubmit = strSubmit & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID)
                    strSubmit = strSubmit & "</web:accountID></web:GetShippingCarrierList></soapenv:Body></soapenv:Envelope>"

                    strReturn = ""
                    ' start of code replaced TJS 18/07/11
                    Try
                        WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_SHIPPING_SERVICE_URL)
                        WebSubmit.Method = "POST"
                        WebSubmit.ContentType = "text/xml; charset=utf-8" ' TJS 02/04/14
                        WebSubmit.ContentLength = strSubmit.Length
                        WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/GetShippingCarrierList")
                        WebSubmit.Timeout = 30000

                        byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

                        ' send to LerrynSecure.com (or the URL defined in the Registry)
                        postStream = WebSubmit.GetRequestStream()
                        postStream.Write(byteData, 0, byteData.Length)

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        strReturn = reader.ReadToEnd()

                    Catch ex As Exception
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor - error details: " & ex.Message)

                    Finally
                        If Not postStream Is Nothing Then postStream.Close()
                        If Not WebResponse Is Nothing Then WebResponse.Close()

                    End Try
                    ' end of code replaced TJS 02/12/11

                    If strReturn <> "" Then
                        If strReturn.Length > 38 Then
                            If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                                ' had difficulty getting XPath to read XML with this name space present so remove it
                                XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", "")) ' TJS 02/04/14
                                'FA 03/12/2010 moved before the For loop as this was over-writing values
                                'if more than one Source
                                'iChannelAdvCarrierCount = 0
                                'ReDim ChannelAdvCarriers(iChannelAdvCarrierCount)
                                XMLNameTabCAShipping = New System.Xml.NameTable ' TJS 02/04/14
                                XMLNSManCAShipping = New System.Xml.XmlNamespaceManager(XMLNameTabCAShipping) ' TJS 02/04/14
                                XMLNSManCAShipping.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/04/14
                                XMLCarrierList = XMLResponse.XPathSelectElements("soap:Envelope/soap:Body/GetShippingCarrierListResponse/GetShippingCarrierListResult/ResultData/ShippingCarrier", XMLNSManCAShipping) ' TJS 02/04/14
                                For Each XMLCarrier In XMLCarrierList
                                    Try
                                        XMLTemp = XDocument.Parse(XMLCarrier.ToString)
                                        bAlreadyLoaded = False
                                        For iLoop = 0 To iChannelAdvCarrierCount - 1
                                            If ChannelAdvCarriers(iLoop).CarrierCode = m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/CarrierCode") And _
                                                ChannelAdvCarriers(iLoop).ClassCode = m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/ClassCode") Then
                                                bAlreadyLoaded = True
                                                Exit For
                                            End If
                                        Next
                                        If Not bAlreadyLoaded Then
                                            iChannelAdvCarrierCount += 1
                                            ReDim Preserve ChannelAdvCarriers(iChannelAdvCarrierCount - 1)
                                            ChannelAdvCarriers(iChannelAdvCarrierCount - 1).CarrierCode = m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/CarrierCode")
                                            ChannelAdvCarriers(iChannelAdvCarrierCount - 1).CarrierName = m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/CarrierName")
                                            ChannelAdvCarriers(iChannelAdvCarrierCount - 1).ClassCode = m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/ClassCode")
                                            ChannelAdvCarriers(iChannelAdvCarrierCount - 1).ClassName = m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, "ShippingCarrier/ClassName")
                                        End If

                                    Catch ex As Exception
                                        Interprise.Presentation.Base.Message.MessageWindow.Show("XML Error in Channel Advisor Shipping Carrier List - " & ex.Message)

                                    End Try
                                Next
                                Coll = Me.repSourceDeliveryMethod.Items
                                Coll.BeginUpdate()
                                Coll.Clear()
                                For iLoop = 0 To iChannelAdvCarrierCount - 1
                                    bAlreadyLoaded = False
                                    For iCheckLoop = 0 To Coll.Count - 1
                                        If Coll.Item(iCheckLoop).Value.ToString = ChannelAdvCarriers(iLoop).CarrierCode Then
                                            bAlreadyLoaded = True
                                            Exit For
                                        End If
                                    Next
                                    If Not bAlreadyLoaded Then
                                        CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(ChannelAdvCarriers(iLoop).CarrierName, ChannelAdvCarriers(iLoop).CarrierCode)
                                        Coll.Add(CollItem)
                                    End If
                                Next
                                Coll.EndUpdate()
                                If GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
                                    (GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Or bNewRowStarted) Then
                                    If GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryMethodCode_DEV000221").ToString <> "" Then
                                        PopulateShippingClassList(False) ' TJS 27/09/10
                                    End If
                                End If
                                Me.m_ConfigSettingsDeliveryTranslationSectionFacade.ChannelAdvCarriers = ChannelAdvCarriers ' TJS 27/09/10

                            Else
                                Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor for account " & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME) & " (" & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) & ") - response data is not valid XML") ' TJS 12/10/10
                            End If
                        Else
                            Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor for account " & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME) & " (" & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) & ") - response data is not valid XML") ' TJS 12/10/10
                        End If
                    Else
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor for account " & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME) & " (" & m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) & ") - response data is blank") ' TJS 12/10/10
                    End If
                Else
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot read Shipping Carrier List from Channel Advisor as your config settings are invalid")
                End If
            Next
        End If

    End Sub
#End Region

#Region " SetSourceSpecificValues "
    Public Function SetSourceSpecificValues() As Boolean
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

        If CheckEBayLoginSettings() Then
            m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
            m_BackgroundWorker.WorkerSupportsCancellation = True
            m_BackgroundWorker.WorkerReportsProgress = False
            AddHandler m_BackgroundWorker.DoWork, AddressOf GetEBayShippingServiceList
            AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetEBayShippingServiceListCompleted
            m_BackgroundWorker.RunWorkerAsync()
            Return True
        Else
            Return False
        End If

    End Function
#End Region

#Region " PopulateShippingClassList "
    Private Sub PopulateShippingClassList(ByVal ApplyFilter As Boolean) ' TJs 27/09/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/12/09 | TJS             | 2010.0.00 | Function added
        ' 27/09/10 | TJs             | 2010.1.02 | Added ApplyFilter parameter
        '------------------------------------------------------------------------------------------

        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim iLoop As Integer, iCheckLoop As Integer, bAlreadyLoaded As Boolean

        If GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            (GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Or bNewRowStarted) Then
            Coll = Me.repSourceDeliveryClass.Items
            Coll.BeginUpdate()
            Coll.Clear()
            For iLoop = 0 To iChannelAdvCarrierCount - 1
                If ChannelAdvCarriers(iLoop).CarrierCode = GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryMethodCode_DEV000221").ToString Or _
                    Not ApplyFilter Then ' TJs 27/09/10
                    bAlreadyLoaded = False
                    For iCheckLoop = 0 To Coll.Count - 1
                        If Coll.Item(iCheckLoop).Value.ToString = ChannelAdvCarriers(iLoop).ClassCode Then
                            bAlreadyLoaded = True
                            Exit For
                        End If
                    Next
                    If Not bAlreadyLoaded Then
                        CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(ChannelAdvCarriers(iLoop).ClassName, ChannelAdvCarriers(iLoop).ClassCode)
                        Coll.Add(CollItem)
                    End If
                End If
            Next
            Coll.EndUpdate()
        End If

    End Sub
#End Region

#Region " CheckEBayLoginSettings "
    Private Function CheckEBayLoginSettings() As Boolean
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

        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLEBayAccountList As System.Collections.Generic.IEnumerable(Of XNode), XMLEBayAccount As XElement

        If Me.m_ConfigSettingsDeliveryTranslationDataset.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221 = EBAY_SOURCE_CODE Then
            If m_EBaySettings Is Nothing Then
                m_EBaySettings = New Lerryn.Framework.ImportExport.SourceConfig.eBaySettings
            End If
            XMLConfig = XDocument.Parse(Trim(Me.m_ConfigSettingsDeliveryTranslationDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
            XMLEBayAccountList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_EBAY_LIST)

            For Each XMLEBayAccount In XMLEBayAccountList
                XMLTemp = XDocument.Parse(XMLEBayAccount.ToString)
                If Not m_EBaySettings.AccountDisabled And m_EBaySettings.AuthToken <> "" And m_EBaySettings.TokenExpires > Date.Now.AddDays(-1) Then
                    Exit For
                End If
                m_EBaySettings.SiteID = Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_SITE_ID)
                If Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY) <> "" Then
                    m_EBaySettings.eBayCountry = CInt(Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID"))
                Else
                    m_EBaySettings.eBayCountry = 0
                End If
                m_EBaySettings.AuthToken = Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN)
                If Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN_EXPIRES) <> "" AndAlso _
                    m_ConfigSettingsDeliveryTranslationSectionFacade.ValidateXMLDate(Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN_EXPIRES)) Then
                    m_EBaySettings.TokenExpires = m_ConfigSettingsDeliveryTranslationSectionFacade.ConvertXMLDate(Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN_EXPIRES))
                Else
                    m_EBaySettings.TokenExpires = Date.Now.AddSeconds(-1)
                End If
                m_EBaySettings.AccountDisabled = CBool(IIf(UCase(Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ACCOUNT_DISABLED)) = "YES", True, False))

            Next
            If Not m_EBaySettings.AccountDisabled And m_EBaySettings.AuthToken <> "" And m_EBaySettings.TokenExpires > Date.Now.AddDays(-1) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
#End Region

#Region " GetEBayShippingServiceList "
    Private Sub GetEBayShippingServiceList(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
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

        Dim strErrorDetails As String, bUseEBaySandbox As Boolean

        bUseEBaySandbox = (Me.m_ConfigSettingsDeliveryTranslationSectionFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "UseEBaySandbox", "NO").ToUpper = "YES")
        strErrorDetails = ""
        If Not m_EBaySettings.AccountDisabled And m_EBaySettings.AuthToken <> "" And m_EBaySettings.TokenExpires > Date.Now.AddDays(-1) Then
            Me.m_ConfigSettingsDeliveryTranslationSectionFacade.GetEBayShippingServiceList(m_EBaySettings, bUseEBaySandbox, m_EBayShippingCarriers, strErrorDetails)
            If strErrorDetails <> "" Then
                Interprise.Presentation.Base.Message.MessageWindow.Show(strErrorDetails)
            End If
        End If

    End Sub

    Private Sub GetEBayShippingServiceListCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
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

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

        If m_EBayShippingCarriers.Length > 0 Then
            Coll = repSourceDeliveryMethod.Items
            Coll.BeginUpdate()
            For Each eBayCarrier As Lerryn.Facade.ImportExport.eBayXMLConnector.eBayShippingCarriers In m_EBayShippingCarriers
                If eBayCarrier.CarrierName <> "CustomCode" And eBayCarrier.CarrierName <> "" Then
                    CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(eBayCarrier.CarrierName, eBayCarrier.CarrierID)
                    Coll.Add(CollItem)
                End If
            Next eBayCarrier
            Coll.EndUpdate()

        End If

        RemoveHandler m_BackgroundWorker.DoWork, AddressOf GetEBayShippingServiceList
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetEBayShippingServiceListCompleted

    End Sub
#End Region
#End Region

#Region " Events "
    Private Sub repDeliveryMethod_BeforePopup(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles repDeliveryMethod.BeforePopup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/12/09 | TJS             | 2010.0.00 | Function added
        '------------------------------------------------------------------------------------------

        Dim strFilter As String

        If GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            (GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Or bNewRowStarted) Then
            strFilter = " and IsActive = 1 and ShippingMethodCode IN (SELECT ShippingMethodCode FROM "
            strFilter = strFilter & "SystemShippingMethodGroupDetail WHERE ShippingMethodGroup = '"
            strFilter = strFilter & GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, Me.ConfigSettingsDeliveryTranslationSectionGateway.LerrynImportExportDeliveryMethods_DEV000221.ShippingMethodGroup_DEV000221Column.ColumnName).ToString
            strFilter = strFilter & "')"
        Else
            strFilter = " and IsActive = 1"
        End If
        repDeliveryMethod.AdditionalFilter = strFilter

    End Sub

    Private Sub repDeliveryMethod_OpenLink(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles repDeliveryMethod.OpenLink

        If e.EditValue Is Nothing Then Return
        Dim ShippingMethodCode As String = CStr(Interprise.Framework.Base.Shared.Common.IsNull(e.EditValue))
        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyShippingMethod.ToString, Nothing, ShippingMethodCode)

    End Sub

    Private Sub repDeliveryMethod_PopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles repDeliveryMethod.PopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to check new row started flag
        ' 15/12/09 | TJS             | 2009.3.09 | Modified to cater for errors on new rows
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for SourceDeliveryClass being a required field even if just an empty string
        '------------------------------------------------------------------------------------------

        Dim rowGroupMethodDetail As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemShippingMethodGroupDetailRow

        Try
            With Me.ConfigSettingsDeliveryTranslationSectionGateway
                If GridViewDeliveryTranslation.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle And Not bNewRowStarted Then ' TJS 17/03/09
                    GridViewDeliveryTranslation.AddNewRow()
                    Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryClass_DEV000221", "") ' TJS 22/09/10
                End If
                GridViewDeliveryTranslation.SetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, .LerrynImportExportDeliveryMethods_DEV000221.ShippingMethodCode_DEV000221Column.ColumnName, eRow.DataRowSelected(.SystemShippingMethod.ShippingMethodCodeColumn.ColumnName))
                ' update original Shipping Method Group Code so that we can detect and check manually entered values
                strOrigShipMthCode = GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, .LerrynImportExportDeliveryMethods_DEV000221.ShippingMethodCode_DEV000221Column.ColumnName).ToString

                If strOrigShipMthGrpCode <> "" And strOrigShipMthCode <> "" Then
                    rowGroupMethodDetail = Me.ConfigSettingsDeliveryTranslationSectionGateway.SystemShippingMethodGroupDetail.FindByShippingMethodGroupShippingMethodCode(strOrigShipMthGrpCode, strOrigShipMthCode)
                    If rowGroupMethodDetail Is Nothing Then
                        GridViewDeliveryTranslation.SetColumnError(GridViewDeliveryTranslation.Columns("ShippingMethodCode_DEV000221"), "Not a valid Shipping Method Code for this Shipping Method Group") ' TJS 15/12/09
                    Else
                        GridViewDeliveryTranslation.SetColumnError(GridViewDeliveryTranslation.Columns("ShippingMethodCode_DEV000221"), "") ' TJS 15/12/09
                    End If
                End If
            End With

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)
        End Try

    End Sub

    Private Sub repDeliveryMethodGroup_OpenLink(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles repDeliveryMethodGroup.OpenLink

        If e.EditValue Is Nothing Then Return
        Dim ShippingMethodGroup As String = CStr(Interprise.Framework.Base.Shared.Common.IsNull(e.EditValue))
        Host.Execute(Interprise.Framework.Base.Shared.Enum.MenuAction.MenuSystemManager.SystemManagerMaintainCompanyShippingMethodGroup.ToString, Nothing, ShippingMethodGroup)

    End Sub

    Private Sub repDeliveryMethodGroup_PopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles repDeliveryMethodGroup.PopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to check new row started flag
        ' 15/12/09 | TJS             | 2009.3.09 | Modified to cater for errors on new rows
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for SourceDeliveryClass being a required field even if just an empty string
        '------------------------------------------------------------------------------------------

        Dim rowGroupMethodDetail As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemShippingMethodGroupDetailRow

        Try
            With Me.ConfigSettingsDeliveryTranslationSectionGateway
                If GridViewDeliveryTranslation.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle And Not bNewRowStarted Then ' TJS 17/03/09
                    GridViewDeliveryTranslation.AddNewRow()
                    Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryClass_DEV000221", "") ' TJS 22/09/10
                End If
                GridViewDeliveryTranslation.SetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, .LerrynImportExportDeliveryMethods_DEV000221.ShippingMethodGroup_DEV000221Column.ColumnName, eRow.DataRowSelected(.SystemShippingMethodGroup.ShippingMethodGroupColumn.ColumnName))
                ' update original Shipping Method Group Code so that we can detect and check manually entered values
                strOrigShipMthGrpCode = GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, .LerrynImportExportDeliveryMethods_DEV000221.ShippingMethodGroup_DEV000221Column.ColumnName).ToString

                If strOrigShipMthGrpCode <> "" And strOrigShipMthCode <> "" Then
                    rowGroupMethodDetail = Me.ConfigSettingsDeliveryTranslationSectionGateway.SystemShippingMethodGroupDetail.FindByShippingMethodGroupShippingMethodCode(strOrigShipMthGrpCode, strOrigShipMthCode)
                    If rowGroupMethodDetail Is Nothing Then
                        GridViewDeliveryTranslation.SetColumnError(GridViewDeliveryTranslation.Columns("ShippingMethodGroup_DEV000221"), "Not a valid Shipping Method Group for this Shipping Method Code") ' TJS 15/12/09
                    Else
                        GridViewDeliveryTranslation.SetColumnError(GridViewDeliveryTranslation.Columns("ShippingMethodGroup_DEV000221"), "") ' TJS 15/12/09
                    End If
                End If
            End With

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)
        End Try

    End Sub

    Private Sub GridViewDeliveryTranslation_CustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridViewDeliveryTranslation.CustomDrawCell
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added to try and ensure data displayed properly
        '------------------------------------------------------------------------------------------

        If GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, e.Column.FieldName) IsNot Nothing Then
            e.CellValue = GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, e.Column.FieldName).ToString
        End If
    End Sub

    Private Sub GridViewDeliveryTranslation_FocusedColumnChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs) Handles GridViewDeliveryTranslation.FocusedColumnChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/12/09 | TJS             | 2010.0.00 | Function added
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for Source Code constants
        ' 27/09/10 | TJs             | 2010.1.02 | Modified to use PopulateShippingClassList ApplyFilter parameter 
        '                                        | to initialise Channel Advisor Source Delivary Class listbox with 
        '                                        | all values so data is displayed in full
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to cater for FocusedColumn not being set
        '------------------------------------------------------------------------------------------

        If e.FocusedColumn IsNot Nothing AndAlso e.FocusedColumn.Name = "colSourceDeliveryClassCode_DEV000221" And m_SourceCode = CHANNEL_ADVISOR_SOURCE_CODE Then ' TJS 27/09/10 TJS 10/06/12
            PopulateShippingClassList(True) ' TJS 19/08/10 TJS 27/09/10
        ElseIf m_SourceCode = CHANNEL_ADVISOR_SOURCE_CODE Then ' TJS 27/09/10
            PopulateShippingClassList(False) ' TJS 27/09/10
        End If

    End Sub

    Private Sub GridViewDeliveryTranslation_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewDeliveryTranslation.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to clear new row started flag
        ' 30/12/09 | TJS             | 2010.0.00 | Modified to refresh ShippingClass list
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for Source Code constants
        ' 27/09/10 | TJs             | 2010.1.02 | Modified call to PopulateShippingClassList so that it only happened 
        '                                        | if the focused column is the Channel Advisor Class Code
        '------------------------------------------------------------------------------------------

        Try
            bNewRowStarted = False ' TJS 17/03/09
            With Me.ConfigSettingsDeliveryTranslationSectionGateway
                If GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
                    (GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle Or bNewRowStarted) Then ' TJS 30/12/09
                    strOrigShipMthCode = GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, .LerrynImportExportDeliveryMethods_DEV000221.ShippingMethodCode_DEV000221Column.ColumnName).ToString
                    strOrigShipMthGrpCode = GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, .LerrynImportExportDeliveryMethods_DEV000221.ShippingMethodGroup_DEV000221Column.ColumnName).ToString
                    If GridViewDeliveryTranslation.FocusedColumn.Name = "colSourceDeliveryClassCode_DEV000221" And _
                        m_SourceCode = CHANNEL_ADVISOR_SOURCE_CODE Then ' TJS 27/09/10
                        PopulateShippingClassList(True) ' TJS 27/09/10
                    ElseIf m_SourceCode = CHANNEL_ADVISOR_SOURCE_CODE Then ' TJS 27/09/10
                        PopulateShippingClassList(False) ' TJS 27/09/10
                    End If
                ElseIf m_SourceCode = CHANNEL_ADVISOR_SOURCE_CODE Then ' TJS 27/09/10
                    PopulateShippingClassList(False) ' TJS 27/09/10
                End If
            End With

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)
        End Try
    End Sub

    Private Sub GridViewDeliveryTranslation_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs) Handles GridViewDeliveryTranslation.InitNewRow
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to set new row started flag
        '------------------------------------------------------------------------------------------

        Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, Me.ConfigSettingsDeliveryTranslationSectionGateway.LerrynImportExportDeliveryMethods_DEV000221.SourceCode_DEV000221Column.ColumnName, m_SourceCode)
        Me.GridViewDeliveryTranslation.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
        strOrigShipMthGrpCode = ""
        strOrigShipMthCode = ""
        bNewRowStarted = True ' TJS 17/03/09

    End Sub

    Private Sub repSourceDeliveryMethod_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles repSourceDeliveryMethod.CloseUp
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/12/09 | TJS             | 2010.0.00 | Function added
        ' 22/09/10 | TJS             | 2010.1.01 | modified to initiate new row and trap for there not being a 
        '                                        | row of data if the editor is closed without selecting a value
        '------------------------------------------------------------------------------------------

        If GridViewDeliveryTranslation.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle And Not bNewRowStarted Then ' TJS 22/09/10
            GridViewDeliveryTranslation.AddNewRow() ' TJS 22/09/10
            Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryClass_DEV000221", "") ' TJS 22/09/10
        End If
        If e.Value IsNot Nothing Then ' TJS 22/09/10
            If e.Value.GetType.Name = "ImageComboBoxItem" Then ' TJS 22/09/10
                If GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle Then ' TJS 22/09/10
                    If DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description <> GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryMethod_DEV000221").ToString Or _
                        DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value.ToString <> GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryMethodCode_DEV000221").ToString Then ' TJS 22/09/10
                        Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryMethod_DEV000221", DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description) ' TJS 22/09/10
                        Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryMethodCode_DEV000221", DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value)
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub repSourceDeliveryClass_CloseUp(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles repSourceDeliveryClass.CloseUp
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/12/09 | TJS             | 2010.0.00 | Function added
        ' 22/09/10 | TJS             | 2010.1.01 | modified to initiate new row and trap for there not being a 
        '                                        | row of data if the editor is closed without selecting a value
        '------------------------------------------------------------------------------------------

        If GridViewDeliveryTranslation.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle And Not bNewRowStarted Then ' TJS 22/09/10
            GridViewDeliveryTranslation.AddNewRow() ' TJS 22/09/10
        End If
        If e.Value IsNot Nothing Then ' TJS 22/09/10
            If e.Value.GetType.Name = "ImageComboBoxItem" Then ' TJS 22/09/10
                If GridViewDeliveryTranslation.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle Then ' TJS 22/09/10
                    If DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description <> GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryClass_DEV000221").ToString Or _
                        DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value.ToString <> GridViewDeliveryTranslation.GetRowCellValue(GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryClassCode_DEV000221").ToString Then ' TJS 22/09/10
                        Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryClass_DEV000221", DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description) ' TJS 22/09/10
                        Me.GridViewDeliveryTranslation.SetRowCellValue(Me.GridViewDeliveryTranslation.FocusedRowHandle, "SourceDeliveryClassCode_DEV000221", DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value)
                    End If
                End If
            End If
        End If

    End Sub
#End Region
End Class
#End Region
