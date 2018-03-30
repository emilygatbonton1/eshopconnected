' eShopCONNECT for Connected Business
' Module: eBayPublishingWizardForm.vb
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

Imports System.Xml.Linq
Imports System.Xml.XPath
Imports Lerryn.Facade.ImportExport.eBayXMLConnector

#Region " eBayPublishingWizardForm "
Namespace eBayWizard
    <MenuActionAttribute.MenuAction("OpeneBayPublishingWizardForm")> _
    Public Class eBayPublishingWizardForm

#Region " Variables "
        Private m_eBayPublishingWizardSection As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Private m_eBayPublishingWizardSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade

        ' Wizard 1
        Private m_SKU As String ' SKU for the Interprise stock item
        Private m_Description As String  ' Brief description for the stock item
        Private m_eBayXMLConfig As XDocument
        Private m_eBayAuthToken As String
        Private m_eBayCountryID As Integer

        ' Wizard 4
        Private m_HTML As String    ' Template HTML for stock item

        Private m_InterpriseStockItem As InterpriseStockItem
#End Region

#Region " Properties "
#Region " eBayXMLConfig "
        Public Property eBayXMLConfig() As XDocument ' TJS/LG 99/99/12
            Get
                Return m_eBayXMLConfig ' TJS/LG 99/99/12
            End Get
            Set(value As XDocument)
                m_eBayXMLConfig = value ' TJS/LG 99/99/12
            End Set
        End Property
#End Region
#Region " SKUToPublish "
        Public Property SKUToPublish() As String
            Get
                Return m_SKU
            End Get
            Set(ByVal value As String)
                m_SKU = value
            End Set
        End Property
#End Region

#Region " DescriptionToPublish "
        Public Property DescriptionToPublish() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property
#End Region

        ' LJG 99/99/99 Start
#Region " InterpriseStockItem "
        Public Property InterpriseStockItem As InterpriseStockItem
            Get
                Return m_InterpriseStockItem
            End Get
            Set(ByVal value As InterpriseStockItem)
                m_InterpriseStockItem = value
            End Set
        End Property
#End Region
        ' LJG 99/99/99 End

#Region " eBayAuthToken "
        Public Property eBayAuthToken() As String
            Get
                Return m_eBayAuthToken
            End Get
            Set(ByVal value As String)
                m_eBayAuthToken = value
            End Set
        End Property
#End Region

#Region " eBayCountryID "
        Public Property eBayCountryID() As Integer
            Get
                Return m_eBayCountryID
            End Get
            Set(ByVal value As Integer)
                m_eBayCountryID = value
            End Set
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

            Me.m_eBayPublishingWizardSection = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            Me.m_eBayPublishingWizardSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.m_eBayPublishingWizardSection, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.ToolBar.Visible = False
            Me.MainMenuBar.Visible = False

        End Sub

        Public Sub New(ByVal InventoryEBaySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
            ByVal InventoryEBaySettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
            MyBase.New()

            Me.m_eBayPublishingWizardSection = InventoryEBaySettingsDataset
            Me.m_eBayPublishingWizardSectionFacade = InventoryEBaySettingsSectionFacade

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.ToolBar.Visible = False
            Me.MainMenuBar.Visible = False

        End Sub
#End Region

#End Region

    End Class
End Namespace
#End Region

