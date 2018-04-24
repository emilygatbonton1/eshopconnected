' eShopCONNECT for Connected Business
' Module: ImportExportRule.vb
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

Imports Lerryn.Framework.ImportExport.Shared.Licence
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const ' TJS 27/09/10
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 05/07/12
Imports Microsoft.VisualBasic
Imports System.IO ' TJS 25/04/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 18/03/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " ImportExportRule "
Public Class ImportExportRule
    Inherits Interprise.Business.Base.BaseRule
    Implements Interprise.Extendable.Base.Business.IBaseInterface

#Region " Custom Events "
    Public Event CheckWebsiteCodeExists(ByVal sender As Object, ByVal SourceCode As String, ByVal AccountOrInstanceID As String, ByVal WebsiteCodeToUse As String, ByVal WebDescriptionToUse As String, ByVal BusinessTypeToUse As String) ' TJS 02/12/11
    Public Event GetMagentoStoreList(ByVal sender As Object, ByRef XMLAccountConfig As XDocument, ByRef XMLStoreList As XDocument) ' TJS 02/12/11
#End Region

#Region " Variables "
    Private Structure Connectors
        Public SourceCode As String ' TJS 18/03/11
        Public ProductCode As String
        Public AccountConfigXMLPath As String
        Public AccountCoreConfigSettings As String
        Public InputHandler As String
        Public ExpiryDate As Date
        Public IsActivated As Boolean
        Public HasBeenActivated As Boolean
        Public MaxAccounts As Integer
        Public LastActivatedMaxAccounts As Integer ' TJS 17/02/12
        Public LatestActivationCode As String ' TJS 17/03/09
        Public IsFullActivation As Boolean ' TJS 03/04/09
    End Structure

    Private m_ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_ErrorNotification As ErrorNotification ' TJS 10/06/12
    Private m_ExpiryDate As Date
    Private m_IsActivated As Boolean = False ' TJS 17/03/09 
    Private m_HasBeenActivated As Boolean = False
    Private m_MaxAccounts As Integer = 0 ' TJS 11/03/09
    Private m_LastActivatedMaxAccounts As Integer = 0 ' TJS 17/02/12
    Private m_LatestActivationCode As String = "" ' TJS 11/03/09
    Private m_SystemID As String = ""
    Private m_CacheID As String = ""
    Private m_SystemHashCode As String = "" ' TJS 04/06/10
    Private ConnectorStates As Connectors()
    Private m_BaseProductCode As String
    Private m_BaseProductName As String
    Private m_SourceConfig As XDocument ' TJS 02/12/11
    Private m_ValidatingActivation As Boolean = False ' TJS 17/03/09 
    Private m_IsFullActivation As Boolean = False ' TJS 03/04/09
    Private m_AccountDetailsValidationStage As String = "" ' TJS 18/03/11
    Private m_LerrynCustomerCode As String = "" ' TJS 18/03/11
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return m_ImportExportDataset
        End Get
    End Property
#End Region

#Region " LatestActivationCode "
    Public ReadOnly Property LatestActivationCode() As String ' TJS 11/03/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Get
            Return m_LatestActivationCode ' TJS 11/03/09
        End Get
    End Property
#End Region

#Region " ConnectorLatestActivationCode "
    Public ReadOnly Property ConnectorLatestActivationCode(ByVal ConnectorProductCode As String) As String ' TJS 17/03/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Get
            Dim iLoop As Integer, strReturnValue As String ' TJS 17/03/09

            If ConnectorProductCode = m_BaseProductCode Then ' TJS 17/03/09
                strReturnValue = m_LatestActivationCode ' TJS 17/03/09
            Else
                strReturnValue = "" ' TJS 17/03/09
                For iLoop = 0 To ConnectorStates.Length - 1 ' TJS 17/03/09
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then ' TJS 17/03/09
                        strReturnValue = ConnectorStates(iLoop).LatestActivationCode ' TJS 17/03/09
                        Exit For ' TJS 17/03/09
                    End If
                Next
            End If
            Return strReturnValue ' TJS 17/03/09
        End Get
    End Property
#End Region

#Region " IsActivated "
    Public ReadOnly Property IsActivated() As Boolean
        Get
            Return m_IsActivated
        End Get
    End Property
#End Region

#Region " IsConnectorActivated "
    Public ReadOnly Property IsConnectorActivated(ByVal ConnectorProductCode As String) As Boolean
        Get
            Dim iLoop As Integer, bReturnValue As Boolean

            If ConnectorProductCode = m_BaseProductCode Then
                bReturnValue = m_IsActivated
            Else
                bReturnValue = False
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then
                        bReturnValue = ConnectorStates(iLoop).IsActivated
                    End If
                Next
            End If
            Return bReturnValue
        End Get
    End Property
#End Region

#Region " HasBeenActivated "
    Public ReadOnly Property HasBeenActivated() As Boolean
        Get
            Return m_HasBeenActivated
        End Get
    End Property
#End Region

#Region " HasConnectorBeenActivated "
    Public ReadOnly Property HasConnectorBeenActivated(ByVal ConnectorProductCode As String) As Boolean
        Get
            Dim iLoop As Integer, bReturnValue As Boolean

            If ConnectorProductCode = m_BaseProductCode Then
                bReturnValue = m_HasBeenActivated
            Else
                bReturnValue = False
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then
                        bReturnValue = ConnectorStates(iLoop).HasBeenActivated
                    End If
                Next
            End If
            Return bReturnValue
        End Get
    End Property
#End Region

#Region " ActivationExpires "
    Public ReadOnly Property ActivationExpires() As Date
        Get
            Return m_ExpiryDate
        End Get
    End Property
#End Region

#Region " ConnectorActivationExpires "
    Public ReadOnly Property ConnectorActivationExpires(ByVal ConnectorProductCode As String) As Date
        Get
            Dim iLoop As Integer, dteReturnValue As Date

            If ConnectorProductCode = m_BaseProductCode Then
                dteReturnValue = m_ExpiryDate
            Else
                dteReturnValue = Date.Today.AddDays(-1)
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then
                        dteReturnValue = ConnectorStates(iLoop).ExpiryDate
                    End If
                Next
            End If
            Return dteReturnValue
        End Get
    End Property
#End Region

#Region " ConnectorAccountLimit "
    Public ReadOnly Property ConnectorAccountLimit(ByVal ConnectorProductCode As String) As Integer
        Get
            Dim iLoop As Integer, iReturnValue As Integer

            If ConnectorProductCode = m_BaseProductCode Then
                iReturnValue = m_MaxAccounts
            Else
                iReturnValue = 1
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then
                        iReturnValue = ConnectorStates(iLoop).MaxAccounts
                    End If
                Next
            End If
            Return iReturnValue
        End Get
    End Property
#End Region

#Region " ConnectorLastActivatedAccountLimit "
    Public ReadOnly Property ConnectorLastActivatedAccountLimit(ByVal ConnectorProductCode As String) As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/02/12 | TJS             | 2010.2.07 | property added
        '------------------------------------------------------------------------------------------
        Get
            Dim iLoop As Integer, iReturnValue As Integer

            If ConnectorProductCode = m_BaseProductCode Then
                iReturnValue = m_LastActivatedMaxAccounts
            Else
                iReturnValue = 0
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then
                        iReturnValue = ConnectorStates(iLoop).LastActivatedMaxAccounts
                    End If
                Next
            End If
            Return iReturnValue
        End Get
    End Property
#End Region

#Region " InventoryImportLimit "
    Public ReadOnly Property InventoryImportLimit() As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/03/11 | TJS             | 2011.0.04 | Modified to check for full activation 
        ' 12/09/11 | TJS             | 2011.2.00 | Modified to enable unlimited item import
        '------------------------------------------------------------------------------------------

        Get
            If m_IsFullActivation Then ' TJS 29/03/11
                Select Case m_MaxAccounts
                    Case Is < 0
                        Return 250
                    Case 0, 1
                        Return 250
                    Case 2
                        Return 2500
                    Case 3
                        Return 10000
                    Case Is > 1000 ' TJS 12/09/11
                        Return 999999 ' TJS 12/09/11
                    Case Is >= 4
                        Return 25000
                End Select
            Else
                Return 250 ' TJS 29/03/11
            End If
        End Get
    End Property
#End Region

#Region " ConnectorProductCode "
    Public ReadOnly Property ConnectorProductCode(ByVal InputHandler As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 28/01/09 | TJS             | 2009.1.01 | Modified to cater for dual eShopCONNECT/OrderImport products
        ' 08/07/09 | TJS             | 2009.3.00 | Modified for Prospect Importer
        '------------------------------------------------------------------------------------------
        Get
            Dim iLoop As Integer, stemp As String = ""
            If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And InputHandler = "GenericXMLImport.ashx") Or _
                (m_BaseProductCode = PROSPECTIMPORTER_BASE_PRODUCT_CODE And InputHandler = "GenericXMLImport.ashx") Or _
                (m_BaseProductCode = ORDERIMPORTER_BASE_PRODUCT_CODE And InputHandler = "Windows Service") Then ' TJS 26/01/09 TJS 08/07/09
                stemp = m_BaseProductCode
            Else
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).InputHandler = InputHandler Then
                        stemp = ConnectorStates(iLoop).ProductCode
                    End If
                Next
            End If
            Return stemp
        End Get
    End Property
#End Region

#Region " IsSystemConfigRecord "
    Public ReadOnly Property IsSystemConfigRecord(ByVal InputHandler As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/01/09 | TJS             | 2009.1.00 | Modified to allow entry of new sources
        ' 28/01/09 | TJS             | 2009.1.01 | Modified to cater for dual eShopCONNECT/OrderImport products
        ' 08/07/09 | TJS             | 2009.3.00 | Modified for Prospect Importer
        '------------------------------------------------------------------------------------------

        Get
            If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And InputHandler = "GenericXMLImport.ashx") Or _
                (m_BaseProductCode = PROSPECTIMPORTER_BASE_PRODUCT_CODE And InputHandler = "GenericXMLImport.ashx") Or _
                (m_BaseProductCode = ORDERIMPORTER_BASE_PRODUCT_CODE And InputHandler = "Windows Service") Then ' TJE 26/01/09 TJS 08/07/09
                Return True
            Else
                Return False
            End If
        End Get
    End Property
#End Region

#Region " IsFullActivation "
    Public ReadOnly Property IsFullActivation() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 27/07/11 | TJS             | 2011.1.00 | Function added
        '------------------------------------------------------------------------------------------

        Get
            Return m_IsFullActivation
        End Get
    End Property
#End Region

#Region " IsConnectorFullActivation "
    Public ReadOnly Property IsConnectorFullActivation(ByVal ConnectorProductCode As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/04/09 | TJS             | 2009.2.00 | Property added
        '------------------------------------------------------------------------------------------

        Get
            Dim iLoop As Integer, bReturnValue As Boolean

            If ConnectorProductCode = m_BaseProductCode Then
                bReturnValue = m_IsFullActivation
            Else
                bReturnValue = False
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then
                        bReturnValue = ConnectorStates(iLoop).IsFullActivation
                    End If
                Next
            End If
            Return bReturnValue
        End Get
    End Property
#End Region

#Region " SystemID "
    Public ReadOnly Property SystemID() As String
        Get
            Return m_SystemID
        End Get
    End Property
#End Region

#Region " CacheID "
    Public ReadOnly Property CacheID() As String
        Get
            Return m_CacheID
        End Get
    End Property
#End Region

#Region " SystemHashCode "
    Public Property SystemHashCode() As String ' TJS 04/06/10
        Get
            Return m_SystemHashCode ' TJS 04/06/10
        End Get
        Set(ByVal value As String) ' TJS 04/06/10
            m_SystemHashCode = value ' TJS 04/06/10
        End Set
    End Property
#End Region

#Region " LerrynCustomerCode "
    Public ReadOnly Property LerrynCustomerCode() As String
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

        Get
            Return m_LerrynCustomerCode
        End Get
    End Property
#End Region

#Region " SourceConfig "
    Public ReadOnly Property SourceConfig() As XDocument ' TJS 02/12/11
        Get
            Return m_SourceConfig
        End Get
    End Property
#End Region

#Region " BaseProductName "
    Public ReadOnly Property BaseProductName() As String
        Get
            Return m_BaseProductName
        End Get
    End Property
#End Region

#Region " AccountDetailsValidationStage "
    Public Property AccountDetailsValidationStage() As String ' TJS 18/03/11
        Get
            Return m_AccountDetailsValidationStage ' TJS 18/03/11
        End Get
        Set(ByVal value As String) ' TJS 18/03/11
            m_AccountDetailsValidationStage = value ' TJS 18/03/11
        End Set
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
    Sub New(ByRef p_ImportExportConfigDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef p_ErrorNotification As Lerryn.Business.ImportExport.ErrorNotification, ByVal p_BaseProductCode As String, ByVal p_BaseProductName As String) ' TJS 10/06/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 28/01/09 | TJS             | 2009.1.01 | Modified to cater for dual eShopCONNECT/OrderImport products
        ' 16/02/09 | TJS             | 2009.1.08 | Corrected Amazon handler name
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to initialise m_LatestActivationCode
        ' 08/07/09 | TJS             | 2009.3.00 | Added Volusion Connector and modified for Prospect Importer
        ' 14/08/09 | TJS             | 2009.3.03 | Corrected Prospect Import eShopCONNECTOR handler details
        ' 10/12/09 | TJS             | 2009.3.09 | Added Channel Advisor Connector 
        ' 05/01/10 | TJs             | 2010.0.01 | Modified to allow prospect, lead and customer import in Order importer
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for Magento and ASLDotNetStoreFront connectors
        '                                        | and to prevent empty ConnectorStates entries
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to add Connector Source Code property
        ' 02/12/11 | TJS             | 2011.2.00 | REmoved initialisation of m_SOurceConfig as now uses XML.Linq and XML.XPath instead of MSXML2
        '                                        | and added eBay connector
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 17/02/12 | TJS             | 2010.2.07 | Modified to save previously activated connector counts
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings 
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart
        '------------------------------------------------------------------------------------------

        MyBase.New()
        m_ErrorNotification = p_ErrorNotification ' TJS 10/06/12
        m_ImportExportDataset = p_ImportExportConfigDataset
        If p_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE Or p_BaseProductCode = ORDERIMPORTER_BASE_PRODUCT_CODE Or _
             p_BaseProductCode = PROSPECTIMPORTER_BASE_PRODUCT_CODE Then ' TJS 08/07/09
            m_BaseProductCode = p_BaseProductCode
            m_BaseProductName = p_BaseProductName
        Else
            m_BaseProductCode = "000"
            m_BaseProductName = "Lerryn Import/Export"
        End If
        m_ExpiryDate = Date.Today.AddYears(-100)
        ReDim ConnectorStates(2)
        ConnectorStates(0).SourceCode = SHOP_COM_SOURCE_CODE ' TJS 18/03/11
        ConnectorStates(0).ProductCode = SHOP_DOT_COM_CONNECTOR_CODE
        ConnectorStates(0).AccountConfigXMLPath = "eShopCONNECTConfig/ShopDotCom"
        ConnectorStates(0).AccountCoreConfigSettings = SHOPDOTCOM_CORE_CONFIG_SETTINGS
        ConnectorStates(0).InputHandler = "ShopComOrder.ashx"
        ConnectorStates(0).ExpiryDate = Date.Today.AddYears(-100)
        ConnectorStates(0).HasBeenActivated = False
        ConnectorStates(0).IsActivated = False
        ConnectorStates(0).MaxAccounts = 0
        ConnectorStates(0).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
        ConnectorStates(0).LatestActivationCode = "" ' TJS 17/03/09
        ConnectorStates(1).SourceCode = AMAZON_SOURCE_CODE ' TJS 18/03/11
        ConnectorStates(1).ProductCode = AMAZON_SELLER_CENTRAL_CONNECTOR_CODE
        ConnectorStates(1).AccountConfigXMLPath = "eShopCONNECTConfig/Amazon"
        ConnectorStates(1).AccountCoreConfigSettings = AMAZON_CORE_CONFIG_SETTINGS
        ConnectorStates(1).InputHandler = "Amazon eShopCONNECTOR" ' TJS 16/02/09 TJS 17/03/09
        ConnectorStates(1).ExpiryDate = Date.Today.AddYears(-100)
        ConnectorStates(1).HasBeenActivated = False
        ConnectorStates(1).IsActivated = False
        ConnectorStates(1).MaxAccounts = 0
        ConnectorStates(1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
        ConnectorStates(1).LatestActivationCode = "" ' TJS 17/03/09
        ' start of code added TJS 08/07/09
        ConnectorStates(2).SourceCode = VOLUSION_SOURCE_CODE ' TJS 18/03/11
        ConnectorStates(2).ProductCode = VOLUSION_CONNECTOR_CODE
        ConnectorStates(2).AccountConfigXMLPath = "eShopCONNECTConfig/Volusion"
        ConnectorStates(2).AccountCoreConfigSettings = VOLUSION_CORE_CONFIG_SETTINGS
        ConnectorStates(2).InputHandler = "Volusion eShopCONNECTOR"
        ConnectorStates(2).ExpiryDate = Date.Today.AddYears(-100)
        ConnectorStates(2).HasBeenActivated = False
        ConnectorStates(2).IsActivated = False
        ConnectorStates(2).MaxAccounts = 0
        ConnectorStates(2).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
        ConnectorStates(2).LatestActivationCode = ""
        ' end of code added TJS 08/07/09

        ' ADD INITIALISATION CODE HERE FOR ANY NEW CONNECTORS

        ' start of code added TJS 17/03/09
        If m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE Then
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = FILE_IMPORT_SOURCE_CODE ' TJS 18/03/11
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig"
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = ""
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "File Import eShopCONNECTOR"
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100)
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = ""
        End If
        If m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE Or m_BaseProductCode = ORDERIMPORTER_BASE_PRODUCT_CODE Then ' TJS 05/01/10 
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 08/07/09 19/08/10 
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = PROSPECT_LEAD_IMPORT_SOURCE_CODE ' TJS 18/03/11
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = PROSPECT_IMPORT_CONNECTOR_CODE ' TJS 08/07/09 TJS 05/01/10
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig" ' TJS 08/07/09
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = "" ' TJS 08/07/09
            If m_BaseProductCode = ORDERIMPORTER_BASE_PRODUCT_CODE Then ' TJS 05/01/10
                ConnectorStates(ConnectorStates.Length - 1).InputHandler = "Prospect/Lead Import" ' TJS 05/01/10
            Else
                ConnectorStates(ConnectorStates.Length - 1).InputHandler = "Prospect/Lead eShopCONNECTOR" ' TJS 08/07/09 TJS 14/08/09
            End If
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100) ' TJS 08/07/09
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False ' TJS 08/07/09
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False ' TJS 08/07/09
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0 ' TJS 08/07/09
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = "" ' TJS 08/07/09
        End If
        If m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/01/10
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 10/12/09 19/08/10 
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = CHANNEL_ADVISOR_SOURCE_CODE ' TJS 18/03/11
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = CHANNEL_ADVISOR_CONNECTOR_CODE ' TJS 10/12/09
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig/ChannelAdvisor" ' TJS 10/12/09
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = CHANNEL_ADVISOR_CORE_CONFIG_SETTINGS ' TJS 10/12/09
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "Channel Advisor eShopCONNECTOR" ' TJS 10/12/09 
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100) ' TJS 10/12/09
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False ' TJS 10/12/09
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False ' TJS 10/12/09
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0 ' TJS 10/12/09
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = "" ' TJS 10/12/09
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 19/08/10 
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = MAGENTO_SOURCE_CODE ' TJS 18/03/11
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = MAGENTO_CONNECTOR_CODE ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig/Magento" ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = MAGENTO_CORE_CONFIG_SETTINGS ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "Magento eShopCONNECTOR" ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100) ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0 ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = "" ' TJS 19/08/10
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 19/08/10 
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = ASP_STORE_FRONT_SOURCE_CODE ' TJS 18/03/11
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = ASP_STORE_FRONT_CONNECTOR_CODE ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig/ASPStoreFront" ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = ASP_STORE_FRONT_CORE_CONFIG_SETTINGS ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "ASPDotNetStoreFront eShopCONNECTOR" ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100) ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0 ' TJS 19/08/10
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = "" ' TJS 19/08/10
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = EBAY_SOURCE_CODE ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = EBAY_CONNECTOR_CODE ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig/eBay" ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = EBAY_CORE_CONFIG_SETTINGS ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "eBay eShopCONNECTOR" ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100) ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0 ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = "" ' TJS 02/12/11 
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 02/12/11 
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = SEARS_COM_SOURCE_CODE ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = SEARS_DOT_COM_CONNECTOR_CODE ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig/SearsDotCom" ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = SEARSDOTCOM_CORE_CONFIG_SETTINGS ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "Sears.com eShopCONNECTOR" ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100) ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0 ' TJS 16/01/12 
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 17/02/12
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = "" ' TJS 16/01/12 
            ReDim Preserve ConnectorStates(ConnectorStates.Length) ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = AMAZON_FBA_SOURCE_CODE ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = AMAZON_FBA_CONNECTOR_CODE ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig/AmazonFBA" ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = AMAZON_FBA_CORE_CONFIG_SETTINGS ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "Amazon FBA eShopCONNECTOR" ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100) ' TJS 22/06/12 
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0 ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0 ' TJS 05/07/12 
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = "" ' TJS 05/07/12 
            ' start of code added TJS 20/11/13
            ReDim Preserve ConnectorStates(ConnectorStates.Length)
            ConnectorStates(ConnectorStates.Length - 1).SourceCode = THREE_D_CART_SOURCE_CODE
            ConnectorStates(ConnectorStates.Length - 1).ProductCode = THREE_D_CART_CONNECTOR_CODE
            ConnectorStates(ConnectorStates.Length - 1).AccountConfigXMLPath = "eShopCONNECTConfig/ThreeDCart"
            ConnectorStates(ConnectorStates.Length - 1).AccountCoreConfigSettings = THREE_D_CART_CORE_CONFIG_SETTINGS
            ConnectorStates(ConnectorStates.Length - 1).InputHandler = "3DCart eShopCONNECTOR"
            ConnectorStates(ConnectorStates.Length - 1).ExpiryDate = Date.Today.AddYears(-100)
            ConnectorStates(ConnectorStates.Length - 1).HasBeenActivated = False
            ConnectorStates(ConnectorStates.Length - 1).IsActivated = False
            ConnectorStates(ConnectorStates.Length - 1).MaxAccounts = 0
            ConnectorStates(ConnectorStates.Length - 1).LastActivatedMaxAccounts = 0
            ConnectorStates(ConnectorStates.Length - 1).LatestActivationCode = ""
            ' end of code added TJS 20/11/13
        End If
        ' end of code added TJS 17/03/09

    End Sub
#End Region

#Region " GetISPluginBaseURL "
    Public Function GetISPluginBaseURL() As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 03/04/09 | TJS             | 2009.2.00 | Modified to prevent Registry read permissions problems causing process failure
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use CheckRegistryValue
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return CheckRegistryValue(REGISTRY_KEY_ROOT, "TestURL", IS_PLUGIN_URL) ' TJS 23/08/09

    End Function
#End Region

#Region " ValidateSource "
    Public Function ValidateSource(ByVal Handler As String, ByVal SourceCode As String, ByVal SourcePassword As String, ByVal Reprocess As Boolean) As Boolean ' TJS 06/10/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/10/09 | TJS             | 2009.3.07 | Modified to cater for reprocessing records
        ' 07/01/11 | TJS             | 2010.1.15 | Modified to cater for changes to SendSrcErrorEmail
        ' 10/06/12 | TJS             | 2012.1.14 | modified to use Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bPasswordValid As Boolean = True, crypto As New Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim salt As Byte(), vector As Byte(), iLoop As Integer, strSourceProductCode As String
        Try


            strSourceProductCode = Me.ConnectorProductCode(Handler)
            ' is base module and relevant source activated ?
            If Me.IsActivated And Me.ActivationExpires >= Date.Today And Me.IsConnectorActivated(strSourceProductCode) And _
                Me.ConnectorActivationExpires(strSourceProductCode) >= Date.Today Then
                ' yes, find 
                For iLoop = 0 To m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count - 1
                    ' does source code match ?
                    If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourceCode_DEV000221 = SourceCode Then
                        ' yes, is handler correct i.e. source has called correct function
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221 = Handler Or Reprocess Then ' TJS 06/10/09
                            ' yes, has a password been stored in config data ?
                            If Not m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).IsSourcePassword_DEV000221Null Then
                                ' yes, has a password been supplied by source and stored password is not an empty string ?
                                If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourcePassword_DEV000221 <> "" And SourcePassword = "" Then
                                    ' no, indicate password failure
                                    bPasswordValid = False

                                ElseIf m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourcePassword_DEV000221 <> "" And SourcePassword <> "" Then
                                    ' password exists and source has supplied one, check if correct
                                    Try
                                        ' get password salt and vector
                                        salt = System.Convert.FromBase64String(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourcePasswordSalt_DEV000221)
                                        vector = System.Convert.FromBase64String(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourcePasswordIV_DEV000221)
                                        ' is password correct ?
                                        If Not crypto.Decrypt(System.Convert.FromBase64String(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).SourcePassword_DEV000221), salt, vector).Equals(SourcePassword) Then
                                            ' no
                                            bPasswordValid = False
                                        End If

                                    Catch ex As Exception
                                        bPasswordValid = False

                                    End Try
                                End If
                            End If

                            If bPasswordValid Then
                                ' get source config settings
                                m_SourceConfig = XDocument.Parse(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221) ' TJS 02/12/11
                                Return True
                            Else
                                m_ErrorNotification.SendSrcErrorEmail(m_SourceConfig, "ValidateSource", "Incorrect Password for Source Code " & SourceCode) ' TJS 07/01/11 TJS 10/06/12
                                Return False
                            End If
                        Else
                            m_ErrorNotification.SendSrcErrorEmail(m_SourceConfig, "ValidateSource", "Incorrect Source Handler for Source Code " & SourceCode & ", expected " & Handler & _
                                ", actual is " & m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).InputHandler_DEV000221) ' TJS 07/01/11 TJS 10/06/12
                            Return False
                        End If
                    End If
                Next
                m_ErrorNotification.SendSrcErrorEmail(m_SourceConfig, "ValidateSource", "Unknown Source Code " & SourceCode) ' TJS 07/01/11 TJS 10/06/12
                Return False
            Else
                If Not Me.IsActivated And Not Me.HasBeenActivated Then
                    m_ErrorNotification.SendSrcErrorEmail(m_SourceConfig, "ValidateSource", m_BaseProductName & " not activated") ' TJS 07/01/11 TJS 10/06/12
                ElseIf Not Me.IsActivated And Me.HasBeenActivated Then
                    m_ErrorNotification.SendSrcErrorEmail(m_SourceConfig, "ValidateSource", m_BaseProductName & " activation (" & m_LatestActivationCode & ") has expired") ' TJS 07/01/11 TJS 10/06/12
                ElseIf Not Me.IsConnectorActivated(strSourceProductCode) And _
                    Not Me.HasConnectorBeenActivated(strSourceProductCode) Then
                    m_ErrorNotification.SendSrcErrorEmail(m_SourceConfig, "ValidateSource", m_BaseProductName & " Source Code " & SourceCode & " not activated") ' TJS 07/01/11 TJS 10/06/12
                Else
                    m_ErrorNotification.SendSrcErrorEmail(m_SourceConfig, "ValidateSource", m_BaseProductName & " Source Code " & SourceCode & " activation (" & Me.ConnectorLatestActivationCode(strSourceProductCode) & ") has expired") ' TJS 07/01/11 TJS 10/06/12
                End If
                Return False
            End If

        Catch ex As Exception
            m_ErrorNotification.SendExceptionEmail(m_SourceConfig, "ValidateSource", ex) ' TJS 07/01/11 TJS 10/06/12
        End Try

    End Function
#End Region

#Region " GetSystemLicenceID "
    Public Function GetSystemLicenceID(ByVal RegisterSysID As Boolean, ByRef SystemID As String, ByRef CacheID As String, _
        ByRef SystemHashCode As String) As String ' TJS 04/06/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS             | 2008.0.01 | Original 
        ' 14/07/09 | TJS             | 2009.3.01 | Modified to use copy Cache DB ID to prevent licence errors if cache db changes
        ' 04/06/10 | TJS             | 2010.0.07 | Modified to request and save System Hash COde
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to extract IS Customer Code and set system hash
        '                                        | plus changes ConvertForURL to ConvertEntitiesForXML and urlencoded POST data
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowsystemID As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.System_DEV000221Row
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, strReturn As String ' TJS 02/12/11
        Dim crypto As New Interprise.Licensing.Base.Services.CryptoServiceProvider ' TJS 18/03/11
        Dim encrypteddata As Byte(), salt As Byte(), vector As Byte() ' TJS 18/03/11
        Dim bIsLinked As Boolean, strSend As String, strSystemLicenceID As String

        bIsLinked = False
        ' has a SystemID been generated already ?
        If Me.m_ImportExportDataset.System_DEV000221.Count = 0 Then
            ' no, create one 
            SystemID = Guid.NewGuid.ToString
            rowsystemID = Me.m_ImportExportDataset.System_DEV000221.NewSystem_DEV000221Row
            rowsystemID.SysID_DEV000221 = SystemID
            rowsystemID.IsReg_DEV000221 = False
            Me.m_ImportExportDataset.System_DEV000221.Rows.Add(rowsystemID)
        End If
        If Not Me.m_ImportExportDataset.SystemCompanyInformation(0).IsCDBID_DEV000221Null Then ' TJS 14/07/09
            CacheID = Me.m_ImportExportDataset.SystemCompanyInformation(0).CDBID_DEV000221 ' TJS 14/07/09
        Else
            CacheID = Guid.NewGuid.ToString ' TJS 14/07/09
            Me.m_ImportExportDataset.SystemCompanyInformation(0).CDBID_DEV000221 = CacheID ' TJS 14/07/09
        End If

        SystemID = Me.m_ImportExportDataset.System_DEV000221(0).SysID_DEV000221
        ' start of code added TJS 18/03/11
        If Not Me.m_ImportExportDataset.System_DEV000221(0).IsRegVal_DEV000221Null Then
            m_SystemHashCode = Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221
            SystemHashCode = m_SystemHashCode
        End If
        If Not Me.m_ImportExportDataset.System_DEV000221(0).IsCustCode_DEV000221Null Then
            encrypteddata = System.Convert.FromBase64String(Me.m_ImportExportDataset.System_DEV000221(0).CustCode_DEV000221)
            salt = System.Convert.FromBase64String(Me.m_ImportExportDataset.System_DEV000221(0).CCSalt_DEV000221)
            vector = System.Convert.FromBase64String(Me.m_ImportExportDataset.System_DEV000221(0).CCIV_DEV000221)

            If Not encrypteddata Is Nothing AndAlso Not salt Is Nothing AndAlso Not vector Is Nothing Then
                If encrypteddata.Length > 0 AndAlso salt.Length > 0 AndAlso vector.Length > 0 Then
                    m_LerrynCustomerCode = crypto.Decrypt(encrypteddata, salt, vector)
                End If
            End If
        End If
        ' end of code added TJS 18/03/11
        strSystemLicenceID = BuildSystemRegID(SystemID, CacheID)
        If Not Me.m_ImportExportDataset.System_DEV000221(0).IsReg_DEV000221 Then
            If Not RegisterSysID Then
                ' no, return empty string
                Return strSystemLicenceID
            Else
                ' yes, register SystemID

                ' build registration data starting with SystemID and CacheID
                strSend = "<LerrynISPlugin><SystemID>" & ConvertEntitiesForXML(strSystemLicenceID) & "</SystemID><Function>SysReg</Function><SendHash>Yes</SendHash>" ' TJS 04/06/10 TJS 18/03/11
                ' add Company Name
                strSend = strSend & "<Company><Name>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("CompanyName").ToString) ' TJS 18/03/11
                strSend = strSend & "</Name>"
                ' and address
                strSend = strSend & "<Address>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Address").ToString) & "</Address>" ' TJS 18/03/11
                strSend = strSend & "<City>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("City").ToString) & "</City>" ' TJS 18/03/11
                strSend = strSend & "<County>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("County").ToString) & "</County>" ' TJS 18/03/11
                strSend = strSend & "<State>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("State").ToString) & "</State>" ' TJS 18/03/11
                strSend = strSend & "<PostalCode>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("PostalCode").ToString) & "</PostalCode>' TJS 18/03/11"
                strSend = strSend & "<Country>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Country").ToString) & "</Country>" ' TJS 18/03/11
                ' phone
                strSend = strSend & "<Phone>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Phone").ToString) & "</Phone>" ' TJS 18/03/11
                strSend = strSend & "<PhoneExtn>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("PhoneExtension").ToString) & "</PhoneExtn>" ' TJS 18/03/11
                ' and fax
                strSend = strSend & "<Fax>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Fax").ToString) & "</Fax>" ' TJS 18/03/11
                strSend = strSend & "<FaxExtn>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("FaxExtension").ToString) & "</FaxExtn>" ' TJS 18/03/11
                If Me.m_ImportExportDataset.LerrynLicences_DEV000221.Count > 0 Then
                    strSend = strSend & "</Company><Activation>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.LerrynLicences_DEV000221(0).LicenceCode_DEV000221) ' TJS 18/03/11
                    strSend = strSend & "</Activation></LerrynISPlugin>"
                Else
                    strSend = strSend & "</Company></LerrynISPlugin>"
                End If

                ' start of code replaced TJS 18/07/11
                Try
                    WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "RegisterSystem.ashx")
                    WebSubmit.Method = "POST"
                    WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                    WebSubmit.ContentLength = strSend.Length
                    WebSubmit.Timeout = 30000

                    byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                    ' send to LerrynSecure.com (or the URL defined in the Registry)
                    postStream = WebSubmit.GetRequestStream()
                    postStream.Write(byteData, 0, byteData.Length)

                    WebResponse = WebSubmit.GetResponse
                    reader = New StreamReader(WebResponse.GetResponseStream())
                    strReturn = reader.ReadToEnd()

                Catch ex As Exception
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Unable to register your system for Activation at this time - " & ex.Message)
                    Return strSystemLicenceID

                Finally
                    If Not postStream Is Nothing Then postStream.Close()
                    If Not WebResponse Is Nothing Then WebResponse.Close()

                End Try
                ' end of code replaced TJS 02/12/11

                ' was registration successful ?
                If "" & strReturn.Substring(0, 70) = "<LerrynISPlugin><Function>RegisterSystem</Function><Status>OK</Status>" Then ' TJS 04/06/10
                    ' yes, set flag 
                    Me.m_ImportExportDataset.System_DEV000221(0).IsReg_DEV000221 = True
                    Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 = strReturn.Substring(76, 13) ' TJS 04/06/10
                    m_SystemHashCode = Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 ' TJS 18/03/11
                    SystemHashCode = m_SystemHashCode ' TJS 18/03/11
                    ' return SystemLicenceID
                    Return strSystemLicenceID
                Else
                    ' 
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Unable to register your system for Activation at this time - No response from server.") ' TJS 27/06/08
                    Return strSystemLicenceID
                End If
            End If
        Else
            Return strSystemLicenceID
        End If
    End Function
#End Region

#Region " CheckActivation "
    Public Sub CheckActivation()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 28/01/09 | TJS             | 2009.1.01 | Modified to extract default config settings
        ' 17/03/09 | TJS             | 2009.1.10 | function added
        ' 04/06/10 | TJS             | 2010.0.07 | Modified to use saved System Hash COde
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to initialise IsActivated flags etc to ensure process properly detects licence expiry
        '------------------------------------------------------------------------------------------

        Dim iLoop As Integer, iModuleLoop As Integer, iErrorCode As Integer, bLicenceMatched As Boolean

        m_IsActivated = False ' TJS 10/06/12 
        m_HasBeenActivated = False ' TJS 10/06/12
        m_IsFullActivation = False ' TJS 10/06/12
        For iModuleLoop = 0 To ConnectorStates.Length - 1 ' TJS 10/06/12
            ConnectorStates(iModuleLoop).IsActivated = False ' TJS 10/06/12
            ConnectorStates(iModuleLoop).HasBeenActivated = False ' TJS 10/06/12
            ConnectorStates(iModuleLoop).IsFullActivation = False ' TJS 10/06/12
        Next
        GetSystemLicenceID(False, m_SystemID, m_CacheID, m_SystemHashCode) ' TJS 04/06/10

        For iLoop = 0 To Me.m_ImportExportDataset.LerrynLicences_DEV000221.Count - 1
            bLicenceMatched = False
            If Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = m_BaseProductCode Then
                iErrorCode = ValidateActivationCode(m_BaseProductCode, Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221, _
                    m_SystemHashCode) ' TJS 04/06/10
                bLicenceMatched = True
            Else
                For iModuleLoop = 0 To ConnectorStates.Length - 1
                    If Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = ConnectorStates(iModuleLoop).ProductCode Then
                        iErrorCode = ValidateActivationCode(ConnectorStates(iModuleLoop).ProductCode, Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221, _
                            m_SystemHashCode) ' TJS 04/06/10
                        bLicenceMatched = True
                        Exit For
                    End If
                Next
            End If
            If Not bLicenceMatched Then
                iErrorCode = ErrorCodes.WrongProductCode
            End If
            ' is licence valid and for base module ?
            If iErrorCode = ErrorCodes.NoError And Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = m_BaseProductCode Then
                ' yes, record details 
                m_IsActivated = True
                m_HasBeenActivated = False ' TJS 17/03/09
                ExtractActivationDetails(Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221, False) ' TJS 02/12/11
                ' get default config settings
                GetDefaultConfigSettings() ' TJS 28/01/09

            ElseIf iErrorCode = ErrorCodes.NoError Then
                ' licence is valid, but is for a connector module - find relevant Connector module state record and update it
                For iModuleLoop = 0 To ConnectorStates.Length - 1
                    If Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = ConnectorStates(iModuleLoop).ProductCode Then
                        ConnectorStates(iModuleLoop).IsActivated = True
                        ConnectorStates(iModuleLoop).HasBeenActivated = False
                        ExtractConnectorActivationDetails(ConnectorStates(iModuleLoop), Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221, False) ' TJS 17/03/09 TJS 02/12/11
                        Exit For
                    End If
                Next

            ElseIf iErrorCode = ErrorCodes.LicenceExpired Then
                ' no, it has expired - is it for base module ?
                If Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = m_BaseProductCode Then
                    ' yes, is module active ?
                    If Not m_IsActivated Then ' TJS 17/03/09
                        ' no, mark as having been activated 
                        m_HasBeenActivated = True
                    End If
                    ExtractActivationDetails(Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221, False) ' TJS 02/12/11
                    ' get default config settings
                    GetDefaultConfigSettings() ' TJS 28/01/09

                Else
                    ' no, must be for a connector module - find relevant Connector module state record and update it
                    For iModuleLoop = 0 To ConnectorStates.Length - 1
                        If Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = ConnectorStates(iModuleLoop).ProductCode Then
                            ' is module active ?
                            If Not ConnectorStates(iModuleLoop).IsActivated Then
                                ' no, mark as having been activated
                                ConnectorStates(iModuleLoop).HasBeenActivated = True
                            End If
                            ExtractConnectorActivationDetails(ConnectorStates(iModuleLoop), Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221, False) ' TJS 17/03/09 TJS 02/12/11
                            Exit For
                        End If
                    Next

                End If
            End If
        Next

    End Sub
#End Region

#Region " GetActivationCode "
    Public Function GetActivationCode(ByRef ErrorCode As Integer) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS             | 2008.0.01 | Original 
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to only return activation codes for current 
        '                                        | product whilst adding activations for all products
        ' 14/07/09 | TJS             | 2009.3.01 | Modified to use copy Cache DB ID to prevent licence errors if cache db changes
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to change ConvertForURL to ConvertEntitiesForXML and urlencoded POST data
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 25/01/12 | TJS             | 2010.2.04 | Modified to copy licence data settings on base module activations
        ' 26/01/12 | TJS             | 2010.2.05 | Modified to update Next Invoice Date in licence data settings on base module activations
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected processing of new activation codes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim rowOrigLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim XMLTemp As XDocument, XMLActivations As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLActivation As XElement
        Dim strSend As String, strSystemLicenceID As String, strReturn As String, sActivation As String
        Dim iOffset As Integer, strLicenceCode As String, strMyActivation As String, strTemp As String ' TJS 17/03/09 TJS 25/01/12
        Dim encrypteddata As Byte(), salt As Byte(), vector As Byte(), strDecrypted() As String ' TJS 25/01/12
        Dim strNextInvoiceDue As String, strPaymentPeriod As String ' TJS 25/01/12 TJS 01/05/14

        ErrorCode = ErrorCodes.NoError
        sActivation = ""
        strMyActivation = "" ' TJS 17/03/09
        ' has a SystemID been generated already ?
        If m_ImportExportDataset.LerrynLicences_DEV000221.Count > 0 Then
            ' yes, build Activation Code request starting with SystemID 
            If Not Me.m_ImportExportDataset.SystemCompanyInformation(0).IsCDBID_DEV000221Null Then ' TJS 14/07/09
                strSystemLicenceID = BuildSystemRegID(m_ImportExportDataset.System_DEV000221(0).SysID_DEV000221, _
                    m_ImportExportDataset.SystemCompanyInformation(0).CDBID_DEV000221) ' TJS 14/07/09
            Else
                strSystemLicenceID = BuildSystemRegID(m_ImportExportDataset.System_DEV000221(0).SysID_DEV000221, _
                    m_ImportExportDataset.System_DEV000221(0).SysID_DEV000221)
            End If
            strSend = "<LerrynISPlugin><SystemID>" & ConvertEntitiesForXML(strSystemLicenceID) & "</SystemID>" ' TJS 18/03/11
            strSend = strSend & "<Function>GetCode</Function><SendNextInvoiceDate>Yes</SendNextInvoiceDate></LerrynISPlugin>" ' TJS 25/01/12

            ' start of code replaced TJS 18/07/11
            Try
                WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "GetActivationCode.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = strSend.Length
                WebSubmit.Timeout = 30000

                byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()

            Catch ex As Exception
                ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                Return ""

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try
            ' end of code replaced TJS 02/12/11

            ' was Activation Code found ?
            If strReturn.Length > 44 Then
                If "" & strReturn.Substring(0, 44) = "<LerrynISPlugin><Function>GetCode</Function>" Then
                    ' yes, get activation codes
                    XMLResponse = XDocument.Parse(Trim(strReturn)) ' TJS 02/12/11
                    XMLActivations = XMLResponse.XPathSelectElements("LerrynISPlugin/Activation")
                    For Each XMLActivation In XMLActivations
                        XMLTemp = XDocument.Parse(XMLActivation.ToString) ' TJS 02/12/11
                        m_ValidatingActivation = True ' TJS 17/03/09
                        sActivation = GetXMLElementText(XMLTemp, "Activation/Code")
                        m_ValidatingActivation = False ' TJS 17/03/09
                        If sActivation <> "" Then
                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(sActivation)
                            If rowLicence Is Nothing Then
                                ErrorCode = AlphaToLong(sActivation.Substring(0, 1), iOffset)
                                If ErrorCode = ErrorCodes.NoError Then
                                    strLicenceCode = sActivation.Substring(0, 1) & GetLicenceChars(sActivation.Substring(1), iOffset)
                                    rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row
                                    rowLicence.CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                                    rowLicence.LicenceCode_DEV000221 = sActivation
                                    rowLicence.ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                                    If rowLicence.ProductCode_DEV000221 = m_BaseProductCode Then ' TJS 17/03/09
                                        strMyActivation = sActivation ' TJS 17/03/09
                                        ' start of code added TJS 25/01/12
                                        rowOrigLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(m_LatestActivationCode)
                                        If rowOrigLicence IsNot Nothing Then
                                            If Not rowLicence.IsData_DEV000221Null And Not rowLicence.IsDataSalt_DEV000221Null And Not rowLicence.IsDataIV_DEV000221Null Then
                                                ' start of code added TJS 26/01/12
                                                crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider

                                                encrypteddata = System.Convert.FromBase64String(rowOrigLicence.Data_DEV000221)
                                                salt = System.Convert.FromBase64String(rowOrigLicence.DataSalt_DEV000221)
                                                vector = System.Convert.FromBase64String(rowOrigLicence.DataIV_DEV000221)

                                                If Not encrypteddata Is Nothing AndAlso Not salt Is Nothing AndAlso Not vector Is Nothing Then
                                                    If encrypteddata.Length > 0 AndAlso salt.Length > 0 AndAlso vector.Length > 0 Then
                                                        strTemp = crypto.Decrypt(encrypteddata, salt, vector)
                                                        strDecrypted = Split(strTemp, ":")
                                                        ' element 0 is End Free Trial date
                                                        ' element 1 is Next Invoice Due date
                                                        ' element 2 is Payment Failed Flag
                                                        ' element 3 is Last Notification date
                                                        ' element 4 is Payment Period
                                                        ' element 5 is Auto Renewal flag
                                                        If strDecrypted.Length >= 6 Then
                                                            m_ValidatingActivation = True
                                                            strNextInvoiceDue = GetXMLElementText(XMLTemp, "Activation/NextInvoiceDate")
                                                            strDecrypted(4) = GetXMLElementText(XMLTemp, "Activation/PaymentPeriod") ' TJs 01/05/14
                                                            If GetXMLElementText(XMLTemp, "Activation/AutoRenew").ToUpper = "YES" Then ' TJs 01/05/14
                                                                strDecrypted(5) = "T" ' TJs 01/05/14
                                                            Else
                                                                strDecrypted(5) = "F" ' TJs 01/05/14
                                                            End If
                                                            m_ValidatingActivation = False
                                                            strTemp = strDecrypted(0) & ":" & strNextInvoiceDue & ":" & strDecrypted(2) & ":" & strDecrypted(3) & ":" & strDecrypted(4) & ":" & strDecrypted(5) & ":"
                                                            ' end of code added TJS 26/01/12
                                                            rowLicence.Data_DEV000221 = crypto.Encrypt(strTemp, salt, vector) ' TJS 26/01/12
                                                            rowLicence.DataIV_DEV000221 = rowOrigLicence.DataIV_DEV000221
                                                            rowLicence.DataSalt_DEV000221 = rowOrigLicence.DataSalt_DEV000221

                                                        End If
                                                    End If
                                                End If

                                            End If
                                        End If
                                        ' end of code added TJS 25/01/12
                                    End If
                                    m_ImportExportDataset.LerrynLicences_DEV000221.AddLerrynLicences_DEV000221Row(rowLicence)
                                Else
                                    Return ""
                                End If
                            End If
                        End If
                    Next
                    Return strMyActivation ' TJS 17/03/09
                Else
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                    Return ""
                End If
            Else
                ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                Return ""
            End If
        Else
            ErrorCode = ErrorCodes.NoSystemIDFoundinDB
            Return ""
        End If

    End Function
#End Region

#Region " ValidateDisplayedActivation "
    Public Function ValidateDisplayedActivation(ByVal ProductCode As String, ByVal ActivationCode As String, _
        ByVal SystemHashCode As String, ByRef ErrorCode As Integer) As Boolean ' TJS 04/06/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 
        ' 04/06/10 | TJS             | 2010.0.07 | Modified to use saved System Hash COde
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ErrorCode = ValidateActivationCode(ProductCode, ActivationCode, SystemHashCode) ' TJS 04/06/10
        If ErrorCode = ErrorCodes.NoError Then
            Return True
        Else
            Return False
        End If

    End Function
#End Region

#Region " UpdateDisplayedActivation "
    Public Function UpdateDisplayedActivation(ByVal ProductCode As String, ByVal ActivationCode As String, ByVal SystemHashCode As String, _
        ByVal EndFreeTrial As Date, ByVal NextInvoiceDue As Date, ByVal PaymentPeriod As String, ByRef ErrorCode As Integer, _
        ByRef UpdatedCode As String) As Boolean ' TJS 04/06/10 TJS 18/03/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS             | 2008.0.01 | Original 
        ' 26/04/10 | TJs             | 2010.0.06 | Modified to use ExtractConnectorActivationDetails
        ' 04/06/10 | TJS             | 2010.0.07 | Modified to use saved System Hash COde
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to prevent error is System Hash Code is null and
        '                                        | to set Data column and detect if updated licence already in db
        '                                        | plus changes ConvertForURL to ConvertEntitiesForXML and urlencoded POST data
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and to make 
        '                                        | ExtractActivationDetails and ExtractConnectorActivationDetails always extract activation details 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strLicenceCode As String, strSystemLicenceID As String, strSend As String, strReturn As String
        Dim strTemp As String, iLoop As Integer, iOffset As Integer, iErrorCode As Integer
        Dim salt As Byte(), vector As Byte(), bLicenceExists As Boolean

        bLicenceExists = False
        ErrorCode = ValidateActivationCode(ProductCode, ActivationCode, SystemHashCode) ' TJS 04/06/10
        If ErrorCode = ErrorCodes.NoError Then
            ErrorCode = AlphaToLong(ActivationCode.Substring(0, 1), iOffset)
            If ErrorCode = ErrorCodes.NoError Then
                strLicenceCode = ActivationCode.Substring(0, 1) & GetLicenceChars(ActivationCode.Substring(1), iOffset)
                For iLoop = 0 To m_ImportExportDataset.LerrynLicences_DEV000221.Count - 1
                    If m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221 = ActivationCode Then
                        If m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).IsCodeExpires_DEV000221Null Then
                            m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                        ElseIf m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 <> GetLicenceProductCode(strLicenceCode) Then
                            m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                        End If
                        If m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).CodeExpires_DEV000221 <> GetLicenceExpiryDate(strLicenceCode) Then
                            m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                        End If
                        bLicenceExists = True
                        Exit For
                    End If
                Next
                If Not bLicenceExists Then
                    ' Does System ID match (disallow Dummy ID so that we can detect this)
                    If Not CheckLicenceSystemID(strLicenceCode, SystemHashCode, False) Then ' TJS 04/06/10
                        ' no, since licence is valid, must be dummy ID so need to get updated Code

                        ' build update data starting with SystemID and CacheID
                        strSystemLicenceID = BuildSystemRegID(SystemID, CacheID)
                        strSend = "<LerrynISPlugin><SystemID>" & ConvertEntitiesForXML(strSystemLicenceID) & "</SystemID><Function>UpdSysID</Function><SendHash>Yes</SendHash>" ' TJS 04/06/10 TJS 18/03/11
                        ' add Company Name
                        strSend = strSend & "<Company><Name>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("CompanyName").ToString) ' TJS 18/03/11
                        strSend = strSend & "</Name>"
                        ' and address
                        strSend = strSend & "<Address>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Address").ToString) & "</Address>" ' TJS 18/03/11
                        strSend = strSend & "<City>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("City").ToString) & "</City>" ' TJS 18/03/11
                        strSend = strSend & "<County>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("County").ToString) & "</County>" ' TJS 18/03/11
                        strSend = strSend & "<State>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("State").ToString) & "</State>" ' TJS 18/03/11
                        strSend = strSend & "<PostalCode>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("PostalCode").ToString) & "</PostalCode>" ' TJS 18/03/11
                        strSend = strSend & "<Country>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Country").ToString) & "</Country>" ' TJS 18/03/11
                        ' phone
                        strSend = strSend & "<Phone>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Phone").ToString) & "</Phone>" ' TJS 18/03/11
                        strSend = strSend & "<PhoneExtn>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("PhoneExtension").ToString) & "</PhoneExtn>" ' TJS 18/03/11
                        ' and fax
                        strSend = strSend & "<Fax>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("Fax").ToString) & "</Fax>" ' TJS 18/03/11
                        strSend = strSend & "<FaxExtn>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.SystemCompanyInformation.Rows(0)("FaxExtension").ToString) & "</FaxExtn>" ' TJS 18/03/11
                        strSend = strSend & "</Company><Activation>" & ActivationCode & "</Activation></LerrynISPlugin>"

                        ' start of code replaced TJS 02/12/11
                        Try
                            WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "UpdateSysID.ashx")
                            WebSubmit.Method = "POST"
                            WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                            WebSubmit.ContentLength = strSend.Length
                            WebSubmit.Timeout = 30000

                            byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                            ' send to LerrynSecure.com (or the URL defined in the Registry)
                            postStream = WebSubmit.GetRequestStream()
                            postStream.Write(byteData, 0, byteData.Length)

                            WebResponse = WebSubmit.GetResponse
                            reader = New StreamReader(WebResponse.GetResponseStream())
                            strReturn = reader.ReadToEnd()

                        Catch ex As Exception
                            ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                            strReturn = ""

                        Finally
                            If Not postStream Is Nothing Then postStream.Close()
                            If Not WebResponse Is Nothing Then WebResponse.Close()

                        End Try
                        ' end of code replaced TJS 02/12/11

                        ' was Activation Code found ?
                        If strReturn.Length > 64 Then
                            If "" & strReturn.Substring(0, 64) = "<LerrynISPlugin><Function>UpdSysID</Function><Status>OK</Status>" Then
                                ' yes, check for revised Activation Code
                                XMLResponse = XDocument.Parse(Trim(strReturn)) ' TJS 02/12/11
                                If Me.m_ImportExportDataset.System_DEV000221.Count > 0 Then ' TJS 04/06/10
                                    If Me.m_ImportExportDataset.System_DEV000221(0).IsRegVal_DEV000221Null Then ' TJS 18/03/11
                                        Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 = GetElementText(XMLResponse, "LerrynISPlugin/Hash") ' TJS 18/03/11
                                    Else
                                        If Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 <> GetElementText(XMLResponse, "LerrynISPlugin/Hash") Then ' TJS 04/06/10
                                            Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 = GetElementText(XMLResponse, "LerrynISPlugin/Hash") ' TJS 04/06/10
                                        End If
                                    End If
                                End If
                                m_ValidatingActivation = True ' TJS 17/03/09
                                If ActivationCode <> GetXMLElementText(XMLResponse, "LerrynISPlugin/Activation") Then
                                    UpdatedCode = GetXMLElementText(XMLResponse, "LerrynISPlugin/Activation")
                                    ActivationCode = GetXMLElementText(XMLResponse, "LerrynISPlugin/Activation")
                                    m_ValidatingActivation = False ' TJS 17/03/09
                                    ErrorCode = ValidateActivationCode(ProductCode, ActivationCode, SystemHashCode) ' TJS 04/06/10
                                    If ErrorCode = ErrorCodes.NoError Then
                                        ErrorCode = AlphaToLong(ActivationCode.Substring(0, 1), iOffset)
                                        If ErrorCode = ErrorCodes.NoError Then
                                            strLicenceCode = ActivationCode.Substring(0, 1) & GetLicenceChars(ActivationCode.Substring(1), iOffset)
                                        Else
                                            Return False
                                        End If
                                    Else
                                        Return False
                                    End If
                                End If
                                m_ValidatingActivation = False ' TJS 17/03/09

                            ElseIf "" & strReturn = "<LerrynISPlugin><Function>UpdSysID</Function><Status>InUse</Status></LerrynISPlugin>" Then
                                ' yes, but already in use
                                ErrorCode = ErrorCodes.LicenceAlreadyInUse
                                Return False

                            ElseIf "" & strReturn = "<LerrynISPlugin><Function>UpdSysID</Function><Status>EvalExp</Status></LerrynISPlugin>" Then
                                ' yes, but eval already expired 
                                ErrorCode = ErrorCodes.EvalAlreadyExpired
                                Return False

                            Else
                                ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                            End If
                        Else
                            ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                        End If
                    End If
                    If ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse Then
                        ' couldn't talk to web site, need to check for already expired eval code
                        For iLoop = 0 To Me.m_ImportExportDataset.LerrynLicences_DEV000221.Count - 1
                            iErrorCode = ValidateActivationCode(ProductCode, Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221, _
                                SystemHashCode) ' TJS 04/06/10
                            ' has licence expired
                            If iErrorCode = ErrorCodes.LicenceExpired Then
                                ' yes, was it an eval code
                                iErrorCode = AlphaToLong(Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221.Substring(0, 1), iOffset)
                                If iErrorCode = ErrorCodes.NoError Then
                                    strLicenceCode = Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221.Substring(0, 1) & GetLicenceChars(Me.m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).LicenceCode_DEV000221.Substring(1), iOffset)
                                    If Not GetLicenceIsFull(strLicenceCode) Then
                                        ErrorCode = ErrorCodes.EvalAlreadyExpired
                                        Return False
                                    End If
                                End If
                            End If
                        Next
                    End If
                    rowLicence = Nothing ' TJS 02/12/11
                    ' are we updating the core activation code ?
                    If GetLicenceProductCode(strLicenceCode) = m_BaseProductCode Then ' TJS 02/12/11
                        ' yes, is product currently activated ?
                        If IsActivated Then ' TJS 02/12/11
                            ' yes, find existing activation code record for updating
                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(LatestActivationCode) ' TJS 02/12/11
                        End If
                    Else
                        ' no, is relevant connector currently activated ?
                        If IsConnectorActivated(GetLicenceProductCode(strLicenceCode)) Then ' TJS 02/12/11
                            ' yes, find existing activation code record for updating
                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(GetLicenceProductCode(strLicenceCode))) ' TJS 02/12/11
                        End If
                    End If
                    If rowLicence IsNot Nothing Then ' TJS 18/03/11
                        bLicenceExists = True ' TJS 18/03/11
                    Else
                        rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row ' TJS 27/07/11
                    End If
                    rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row
                    rowLicence.CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                    rowLicence.LicenceCode_DEV000221 = ActivationCode
                    rowLicence.ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                    ' start of code added TJS 18/03/11
                    If m_BaseProductCode = GetLicenceProductCode(strLicenceCode) Then
                        crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
                        salt = crypto.GenerateSalt
                        vector = crypto.GenerateVector

                        strTemp = EndFreeTrial.Year & "-" & Right("00" & EndFreeTrial.Month, 2) & "-" & Right("00" & EndFreeTrial.Day, 2) & ":"
                        strTemp = strTemp & NextInvoiceDue.Year & "-" & Right("00" & NextInvoiceDue.Month, 2) & "-" & Right("00" & NextInvoiceDue.Day, 2)
                        ' element 2 is Payment Failed Flag
                        ' element 3 is Last Notification date
                        ' element 4 is Payment Period
                        ' element 5 is Auto Renewal flag
                        strTemp = strTemp & ":F::" & PaymentPeriod & ":F"
                        rowLicence.Data_DEV000221 = crypto.Encrypt(strTemp, salt, vector)
                        rowLicence.DataSalt_DEV000221 = System.Convert.ToBase64String(salt)
                        rowLicence.DataIV_DEV000221 = System.Convert.ToBase64String(vector)
                    End If
                    If Not bLicenceExists Then ' TJS 18/03/11
                        m_ImportExportDataset.LerrynLicences_DEV000221.AddLerrynLicences_DEV000221Row(rowLicence)
                    End If
                End If
                If ProductCode = m_BaseProductCode Then
                    ExtractActivationDetails(ActivationCode, True) ' TJS 02/12/11
                    m_IsActivated = True
                Else
                    For iLoop = 0 To ConnectorStates.Length - 1
                        If ConnectorStates(iLoop).ProductCode = ProductCode Then
                            ConnectorStates(iLoop).IsActivated = True
                            ExtractConnectorActivationDetails(ConnectorStates(iLoop), ActivationCode, True) ' TJS 26/04/10 TJS 02/12/11
                            Exit For
                        End If
                    Next
                End If
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
#End Region

#Region " PurchaseActivation "
    Public Function PurchaseActivation(ByVal ActivationPeriod As String, ByVal ActivationType As String, ByVal CurrencyCode As String, _
        ByVal ActivationCost As Decimal, ByVal EndFreeTrial As Date, ByVal NextInvoiceDue As Date, ByVal PurchaseNow As Boolean, _
        ByRef ErrorCode As Integer, ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    first activation or re-activation only
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to log activation requests if relevant registry key set
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, to add eBay conenctor 
        '                                        | and corrected sending of connector previous activation code
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 19/04/12 | TJS             | 2012.1.01 | Modified to set CustomerTypeCode and ensure spaces in XML are handled properly
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to use Error Notification object to simplify facade login/logout 
        '                                        | and corrected check for empty Shipping Country field
        ' 14/06/12 | TJS             | 2012.1.06 | Modified to filter set allowed activation currency
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings and 50000 Inventory Import qty option
        ' 18/01/13 | TJS             | 2012.1.17 | Modified to remove credit/debit card registration
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to detect the demo company and amend activation process accordingly
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater for no activations being returned when PayPal invoice is created
        '                                        | and extended timeout delay
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to set connected Business pricing flag
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        ' 18/02/14 | TJS             | 2014.0.00 | Modified for CB14_DEMO_CUSTOMER_CODE
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTemp As XDocument
        Dim XMLConnectorList As System.Collections.Generic.IEnumerable(Of XElement), XMLConnectorNode As XElement
        Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim encrypteddata As Byte(), salt As Byte(), vector As Byte(), strDecrypted() As String
        Dim strSend As String, strReturn As String, strActivationCode As String
        Dim strLicenceCode As String, strISCustID As String, strTemp As String
        Dim iOffset As Integer, iLoop As Integer, iConnectorLoop As Integer
        Dim dteStartDate As Date, dteExpiryDate As Date
        Dim bSaveLerrynCustCode As Boolean, bExistingAutoRenewState As Boolean, bAutoRenew As Boolean
        Dim bUpdateExistingCode As Boolean, bLogActivation As Boolean, bConfigUpdated As Boolean ' TJS 25/04/11 TJS 02/12/11

        dteStartDate = Date.Today
        bLogActivation = (CheckRegistryValue(REGISTRY_KEY_ROOT, "LogActivation", "NO").ToUpper = "YES") ' TJS 25/04/11

        ' has customer record been created and recorded ?
        If m_LerrynCustomerCode = "" Then
            ' no, need to create in Lerryn Systems (LERWEB)
            strSend = "<?xml version=""1.0"" encoding=""UTF-8""?><eShopCONNECT><ImportType>Prospect</ImportType>"
            strSend = strSend & "<Source><SourceName>eShopCONNECT</SourceName><SourceCode>GenericXMLImport</SourceCode></Source>"
            ' get IS Customer ID from Licence
            strISCustID = Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode
            If strISCustID <> "" Then
                strSend = strSend & "<Prospect><SourceProspectID>IS-" & strISCustID & "</SourceProspectID>"
            Else
                strSend = strSend & "<Prospect><SourceProspectID>" & m_SystemHashCode & "</SourceProspectID>"
            End If
            strSend = strSend & "<TradingCurrency>" & GetActivationCurrency(CurrencyCode) & "</TradingCurrency><BillingDetails><Contact>" ' TJS 14/06/12
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingSalutationNull Then
                strSend = strSend & "<NamePrefix>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingSalutation) & "</NamePrefix>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingFirstNameNull Then
                strSend = strSend & "<FirstName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingFirstName) & "</FirstName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingLastNameNull Then
                strSend = strSend & "<LastName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingLastName) & "</LastName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCompanyNameNull Then
                strSend = strSend & "<Company>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCompanyName) & "</Company>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingPhoneNull Then
                strSend = strSend & "<WorkPhone>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingPhone) & "</WorkPhone>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingPhoneExtensionNull Then
                strSend = strSend & "<PhoneExtn>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingPhoneExtension) & "</PhoneExtn>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsEmailNull Then
                strSend = strSend & "<Email>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).Email) & "</Email>"
            End If
            If Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode <> CB_DEMO_CUSTOMER_CODE And _
            Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode <> CB14_DEMO_CUSTOMER_CODE Then ' TJS 13/03/13 TJS 18/02/14
                If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsPasswordNull Then
                    strSend = strSend & "<LoginPwd>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).Password) & "</LoginPwd>"
                End If
            End If
            strSend = strSend & "</Contact><BillingAddress>"
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingAddressNull Then
                strSend = strSend & "<Address>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingAddress) & "</Address>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCityNull Then
                strSend = strSend & "<Town_City>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCity) & "</Town_City>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingStateNull Then
                strSend = strSend & "<State>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingState) & "</State>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCountyNull Then
                strSend = strSend & "<County>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCounty) & "</County>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingPostalCodeNull Then
                strSend = strSend & "<PostalCode>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingPostalCode) & "</PostalCode>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCountryNull Then
                strSend = strSend & "<Country>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCountry) & "</Country>"
            End If
            strSend = strSend & "</BillingAddress></BillingDetails><ShippingDetails><Contact>"
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingSalutationNull Then
                strSend = strSend & "<NamePrefix>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingSalutation) & "</NamePrefix>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingFirstNameNull Then
                strSend = strSend & "<FirstName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingFirstName) & "</FirstName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingLastNameNull Then
                strSend = strSend & "<LastName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingLastName) & "</LastName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCompanyNameNull Then
                strSend = strSend & "<Company>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCompanyName) & "</Company>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingPhoneNull Then
                strSend = strSend & "<Phone>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingPhone) & "</Phone>"
            End If
            strSend = strSend & "</Contact><ShippingAddress>"
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingAddressNull Then
                strSend = strSend & "<Address>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingAddress) & "</Address>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCityNull Then
                strSend = strSend & "<Town_City>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCity) & "</Town_City>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingStateNull Then
                strSend = strSend & "<State>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingState) & "</State>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCountyNull Then
                strSend = strSend & "<County>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCounty) & "</County>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingPostalCodeNull Then
                strSend = strSend & "<PostalCode>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingPostalCode) & "</PostalCode>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCountryNull Then ' TJS 10/06/12
                strSend = strSend & "<Country>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCountry) & "</Country>"
            End If
            strSend = strSend & "</ShippingAddress><ShippingMethod>No Shipping Required</ShippingMethod>"
            strSend = strSend & "<ShippingMethodGroup>WEBSHIP</ShippingMethodGroup></ShippingDetails>"
            strSend = strSend & "<CustomField FieldName=""CustomerTypeCode"">PluginEndUser</CustomField>" ' TJS 19/04/12
            strSend = strSend & "</Prospect><ProspectCount>1</ProspectCount></eShopCONNECT>"
            strSend = strSend.Replace(" ", "%20") ' TJS 19/04/12

            ' start of code replaced TJS 02/12/11
            Try
                If GetActivationCurrency(CurrencyCode) = "USD" Then ' TJS 14/06/12
                    WebSubmit = System.Net.WebRequest.Create("https://www.lerrynsecure.com/eShopCONNECTWebImportUS/GenericXMLImport.ashx")
                Else
                    WebSubmit = System.Net.WebRequest.Create("https://www.lerrynsecure.com/eShopCONNECTWebImport/GenericXMLImport.ashx")
                End If
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = HttpUtility.UrlEncode(strSend).Length
                WebSubmit.Timeout = 30000

                ' start of code added TJS 25/04/11
                If bLogActivation Then
                    Try
                        Dim writer As StreamWriter
                        If m_ErrorNotification.LogFilePath <> "" Then ' TJS 10/06/12
                            If m_ErrorNotification.LogFilePath.Substring(m_ErrorNotification.LogFilePath.Length - 1) <> "\" Then ' TJS 10/06/12
                                m_ErrorNotification.LogFilePath = m_ErrorNotification.LogFilePath & "\" ' TJS 10/06/12
                            End If
                        Else
                            m_ErrorNotification.LogFilePath = System.AppDomain.CurrentDomain.BaseDirectory() ' TJS 10/03/09 TJS 10/06/12
                        End If

                        'Start the logging to the file
                        writer = New StreamWriter(m_ErrorNotification.LogFilePath & "RegisterCustomer_" & Format(Now.Date.ToString("yyyyMMdd") & ".txt"), True) ' TJS 10/06/12

                        writer.Write(strSend)
                        writer.Close()

                    Catch Ex As Exception
                        ' ignore errors
                    End Try
                End If
                ' end of code added TJS 25/04/11

                byteData = UTF8Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(strSend))

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()

                XMLResponse = XDocument.Parse(strReturn) ' TJS 02/12/11
                If GetElementText(XMLResponse, "eShopCONNECT/ImportResponse/Status") = "Success" Then
                    m_LerrynCustomerCode = GetElementText(XMLResponse, "eShopCONNECT/ImportResponse/ProspectCode")
                    If Me.m_ImportExportDataset.System_DEV000221.Count > 0 Then
                        bSaveLerrynCustCode = False
                        If Me.m_ImportExportDataset.System_DEV000221(0).IsCustCode_DEV000221Null Then
                            bSaveLerrynCustCode = True
                        Else
                            If Me.m_ImportExportDataset.System_DEV000221(0).CustCode_DEV000221 <> m_LerrynCustomerCode Then
                                bSaveLerrynCustCode = True
                            End If
                        End If
                        If bSaveLerrynCustCode Then
                            crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
                            salt = crypto.GenerateSalt
                            vector = crypto.GenerateVector

                            Me.m_ImportExportDataset.System_DEV000221(0).CustCode_DEV000221 = crypto.Encrypt(m_LerrynCustomerCode, salt, vector)
                            Me.m_ImportExportDataset.System_DEV000221(0).CCSalt_DEV000221 = System.Convert.ToBase64String(salt)
                            Me.m_ImportExportDataset.System_DEV000221(0).CCIV_DEV000221 = System.Convert.ToBase64String(vector)
                            crypto = Nothing
                        End If
                    End If
                Else
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                    Return False
                End If

            Catch ex As Exception
                ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()
                Return False


            End Try
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()
            ' end of code replaced TJS 02/12/11
        End If

        ' card registration code removed TJS 18/01/13

        If m_LatestActivationCode <> "" Then ' TJS 21/03/11
            crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
            rowLicence = Me.m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(m_LatestActivationCode)
            If Not rowLicence.IsData_DEV000221Null And Not rowLicence.IsDataSalt_DEV000221Null And Not rowLicence.IsDataIV_DEV000221Null Then
                encrypteddata = System.Convert.FromBase64String(rowLicence.Data_DEV000221)
                salt = System.Convert.FromBase64String(rowLicence.DataSalt_DEV000221)
                vector = System.Convert.FromBase64String(rowLicence.DataIV_DEV000221)
            Else
                encrypteddata = Nothing
                salt = Nothing
                vector = Nothing
            End If

            If Not encrypteddata Is Nothing AndAlso Not salt Is Nothing AndAlso Not vector Is Nothing Then
                If encrypteddata.Length > 0 AndAlso salt.Length > 0 AndAlso vector.Length > 0 Then
                    strDecrypted = Split(crypto.Decrypt(encrypteddata, salt, vector), ":")
                    ' element 3 is Last Notification date
                    ' element 4 is Payment Period
                    If strDecrypted.Length >= 6 Then
                        If strDecrypted(5) = "T" Then
                            bExistingAutoRenewState = True
                        End If
                    End If
                End If
            End If

        End If

        ' now generate activations
        If ActivationType = "Eval" Or Not m_IsFullActivation Then
            bAutoRenew = True
        Else
            ' set default condition for auto renew - may be changed below
            bAutoRenew = False
        End If

        If (m_IsActivated And m_IsFullActivation) Or m_HasBeenActivated Or Me.m_ImportExportDataset.ActivationAccountDetails(0).PurchaseNow Then
            Select Case ActivationPeriod
                Case "M"
                    ' has activation already expired ?
                    If m_ExpiryDate > Date.Today Then
                        ' no, new activation must follow on from existing expiry date
                        dteExpiryDate = m_ExpiryDate.AddMonths(1)

                    Else
                        ' yes, add 7 days allowance for payment process
                        dteExpiryDate = Date.Today.AddMonths(1).AddDays(7)
                    End If
                    bAutoRenew = True

                Case "1Y"
                    ' has activation already expired ?
                    If m_ExpiryDate > Date.Today Then
                        ' no, new activation must follow on from existing expiry date
                        dteExpiryDate = m_ExpiryDate.AddYears(1)
                        bAutoRenew = True

                    Else
                        ' yes, need to look at existing auto renew state
                        If bExistingAutoRenewState Then
                            ' auto renew was set, so add 7 days allowance for payment process 
                            dteExpiryDate = Date.Today.AddYears(1).AddDays(7)
                            bAutoRenew = True
                        Else
                            ' auto renew not set, set expiry date 
                            dteExpiryDate = Date.Today.AddYears(1)
                            bAutoRenew = False
                        End If
                    End If

                Case "2Y"
                    ' has activation already expired ?
                    If m_ExpiryDate > Date.Today Then
                        ' no, new activation must follow on from existing expiry date
                        dteExpiryDate = m_ExpiryDate.AddYears(2).AddDays(7)

                    Else
                        ' yes, need to look at existing auto renew state
                        If bExistingAutoRenewState Then
                            ' auto renew was set, so add 7 days allowance for payment process 
                            dteExpiryDate = Date.Today.AddYears(2).AddDays(7)
                            bAutoRenew = True
                        Else
                            ' auto renew not set, set expiry date 
                            dteExpiryDate = Date.Today.AddYears(2)
                            bAutoRenew = False
                        End If
                    End If

                Case "3Y"
                    ' yes, need to look at existing auto renew state
                    If bExistingAutoRenewState Then
                        ' auto renew was set, so add 7 days allowance for payment process 
                        dteExpiryDate = m_ExpiryDate.AddYears(3).AddDays(7)

                    Else
                        ' yes, need to look at existing auto renew state
                        If bExistingAutoRenewState Then
                            ' auto renew was set, so add 7 days allowance for payment process 
                            dteExpiryDate = Date.Today.AddYears(3).AddDays(7)
                            bAutoRenew = True
                        Else
                            ' auto renew not set, set expiry date 
                            dteExpiryDate = Date.Today.AddYears(3)
                            bAutoRenew = False
                        End If
                    End If

                Case Else
                    ErrorDetails = "Invalid Activation period " & ActivationPeriod
                    ErrorCode = ErrorCodes.CalulationError
                    Return False

            End Select
        Else
            ' add 3 days for our renewal process 
            dteExpiryDate = EndFreeTrial.AddDays(3)
        End If

        strSend = "<LerrynPurchaseActivation><Login><Source>ISIntegrated</Source><Password>K$0eW*zU2B</Password>"
        strSend = strSend & "</Login><SystemID>" & BuildSystemRegID(SystemID, CacheID) & "</SystemID>"
        strSend = strSend & "<SysIDType>ISSysID</SysIDType><LerrynCustCode>" & m_LerrynCustomerCode
        strSend = strSend & "</LerrynCustCode><ProductCode>" & m_BaseProductCode & "</ProductCode>"
        strSend = strSend & "<PreviousActivation>" & m_LatestActivationCode & "</PreviousActivation>"
        If Me.m_ImportExportDataset.ActivationAccountDetails(0).TurnoverBasedPricing Then
            strSend = strSend & "<TurnoverBasedPricing>Yes</TurnoverBasedPricing>" ' TJS 09/08/13
            strSend = strSend & "<PercentageOfSales>" & Format(Me.m_ImportExportDataset.ActivationAccountDetails(0).PercentageOfSales, "0.00") & "</PercentageOfSales>" ' TJS 09/08/13
            strSend = strSend & "<MonthlyMinimum>" & Format(Me.m_ImportExportDataset.ActivationAccountDetails(0).MonthlyMinimum, "0.00") & "</MonthlyMinimum>" ' TJS 09/08/13
        End If
        strSend = strSend & "<ExpiryDate>" & dteExpiryDate.Year & "-" & Right("00" & dteExpiryDate.Month, 2)
        strSend = strSend & "-" & Right("00" & dteExpiryDate.Day, 2) & "</ExpiryDate><Currency>"
        strSend = strSend & GetActivationCurrency(CurrencyCode) & "</Currency>" ' TJS 14/06/12
        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ImportWizardOnly Then ' TJS 21/03/11
            Select Case Me.m_ImportExportDataset.ActivationAccountDetails(0).ImportWizardQty
                Case Is <= 250
                    strSend = strSend & "<MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>1</MaxSecondaryCount><ActivationType>"
                Case 2500
                    strSend = strSend & "<MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>2</MaxSecondaryCount><ActivationType>"
                Case 10000
                    strSend = strSend & "<MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>3</MaxSecondaryCount><ActivationType>"
                Case 25000
                    strSend = strSend & "<MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>4</MaxSecondaryCount><ActivationType>"
                Case 50000 ' TJS 05/07/12
                    strSend = strSend & "<MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>5</MaxSecondaryCount><ActivationType>" ' TJS 05/07/12

            End Select
        Else
            strSend = strSend & "<MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>4</MaxSecondaryCount><ActivationType>" ' 02/12/11
        End If
        strSend = strSend & ActivationType & "</ActivationType><TransactionInvoiceDue>" & NextInvoiceDue.Year
        strSend = strSend & "-" & Right("00" & NextInvoiceDue.Month, 2) & "-" & Right("00" & NextInvoiceDue.Day, 2)
        strSend = strSend & "</TransactionInvoiceDue><ActivationTotalCost>" & Format(ActivationCost, "0.00") & "</ActivationTotalCost>"
        ' don't set autorenew on activations in the demo company
        If bAutoRenew And Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode <> CB_DEMO_CUSTOMER_CODE And _
            Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode <> CB14_DEMO_CUSTOMER_CODE Then ' TJS 13/03/13 TJS 18/02/14
            strSend = strSend & "<AutoRenew>Yes</AutoRenew><ActivationPeriod>" & ActivationPeriod & "</ActivationPeriod>" ' TJS 02/12/11
        Else
            strSend = strSend & "<AutoRenew>No</AutoRenew>"
        End If
        If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ImportWizardOnly Then
            For iLoop = 1 To 12 ' TJS 02/12/11 TJS 16/01/12 TJS 05/07/12 TJS 20/08/13 TJS 20/11/13
                Select Case iLoop
                    Case 1 ' ASPDotNetStorefront
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount > 0 Then
                            strSend = strSend & "<Connector><ProductCode>" & ASP_STORE_FRONT_CONNECTOR_CODE & "</ProductCode>"
                            If IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                                strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                                strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(5, 5)
                                strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(10, 5)
                                strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(15, 5)
                                strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(20, 5)
                                strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(25, 5)
                                strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(30, 5)
                                strSend = strSend & "</PreviousActivation>"
                            End If
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount
                            strSend = strSend & "</ConnectionCount></Connector>"
                        End If

                    Case 2 ' Magento
                        strSend = strSend & "<Connector><ProductCode>" & MAGENTO_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).MagentoCount
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 3 ' Volusion
                        strSend = strSend & "<Connector><ProductCode>" & VOLUSION_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).VolusionCount
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 4 ' Amazon
                        strSend = strSend & "<Connector><ProductCode>" & AMAZON_SELLER_CENTRAL_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonCount
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 5 ' Channel Advisor
                        strSend = strSend & "<Connector><ProductCode>" & CHANNEL_ADVISOR_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ChanAdvCount
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 6 ' Shop.com
                        strSend = strSend & "<Connector><ProductCode>" & SHOP_DOT_COM_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ShopComCount
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 7 ' eBay TJS 02/12/11
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount > 0 Then ' TJS 02/12/11
                            strSend = strSend & "<Connector><ProductCode>" & EBAY_CONNECTOR_CODE & "</ProductCode>" ' TJS 02/12/11
                            If IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                                strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                                strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(5, 5) ' TJS 02/12/11
                                strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                                strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                                strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                                strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                                strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                                strSend = strSend & "</PreviousActivation>" ' TJS 02/12/11
                            End If
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount ' TJS 02/12/11
                            strSend = strSend & "</ConnectionCount></Connector>" ' TJS 02/12/11
                        End If

                    Case 8 ' Sears.com TJS 16/01/12
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount > 0 Then ' TJS 16/01/12
                            strSend = strSend & "<Connector><ProductCode>" & SEARS_DOT_COM_CONNECTOR_CODE & "</ProductCode>" ' TJS 16/01/12
                            If IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                                strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 16/01/12
                                strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(5, 5) ' TJS 16/01/12
                                strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 16/01/12
                                strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 16/01/12
                                strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 16/01/12
                                strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 16/01/12
                                strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 16/01/12
                                strSend = strSend & "</PreviousActivation>" ' TJS 16/01/12
                            End If
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount ' TJS 16/01/12
                            strSend = strSend & "</ConnectionCount></Connector>" ' TJS 16/01/12
                        End If

                        ' start of code added TJS 05/07/12
                    Case 9 ' Amazon FBA
                        strSend = strSend & "<Connector><ProductCode>" & AMAZON_FBA_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonFBACount
                        strSend = strSend & "</ConnectionCount></Connector>"
                        ' end of code added TJS 05/07/12

                        ' start of code added TJS 20/08/13
                    Case 10 ' File Import
                        strSend = strSend & "<Connector><ProductCode>" & ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateFileImport Then
                            strSend = strSend & "<ConnectionCount>1"
                        Else
                            strSend = strSend & "<ConnectionCount>0"
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 11 ' Prospect and Lead
                        strSend = strSend & "<Connector><ProductCode>" & PROSPECT_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateProspectLead Then
                            strSend = strSend & "<ConnectionCount>1"
                        Else
                            strSend = strSend & "<ConnectionCount>0"
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"
                        ' end of code added TJS 20/08/13

                        ' start of code added TJS 20/11/13
                    Case 12 ' 3DCart
                        strSend = strSend & "<Connector><ProductCode>" & THREE_D_CART_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ThreeDCartCount
                        strSend = strSend & "</ConnectionCount></Connector>"
                        ' end of code added TJS 20/11/13
                End Select
            Next
        End If
        strSend = strSend & "</LerrynPurchaseActivation>"

        ' start of code replaced TJS 02/12/11
        Try
            WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "PurchaseActivation.ashx")
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
            WebSubmit.ContentLength = strSend.Length
            WebSubmit.Timeout = 90000 ' TJS 22/03/13

            ' start of code added TJS 25/04/11
            If bLogActivation Then
                Try
                    Dim writer As StreamWriter
                    If m_ErrorNotification.LogFilePath <> "" Then ' TJS 10/06/12
                        If m_ErrorNotification.LogFilePath.Substring(m_ErrorNotification.LogFilePath.Length - 1) <> "\" Then ' TJS 10/06/12
                            m_ErrorNotification.LogFilePath = m_ErrorNotification.LogFilePath & "\" ' TJS 10/06/12
                        End If
                    Else
                        m_ErrorNotification.LogFilePath = System.AppDomain.CurrentDomain.BaseDirectory() ' TJS 10/06/12
                    End If

                    'Start the logging to the file
                    writer = New StreamWriter(m_ErrorNotification.LogFilePath & "PurchaseActivation_" & Format(Now.Date.ToString("yyyyMMdd") & ".txt"), True) ' TJS 10/06/12

                    writer.Write(strSend)
                    writer.Close()

                Catch Ex As Exception
                    ' ignore errors
                End Try
            End If
            ' end of code added TJS 25/04/11

            byteData = UTF8Encoding.UTF8.GetBytes(strSend)

            ' send to LerrynSecure.com (or the URL defined in the Registry)
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            strReturn = reader.ReadToEnd()

        Catch ex As Exception
            ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()
            Return False


        End Try
        If Not postStream Is Nothing Then postStream.Close()
        If Not WebResponse Is Nothing Then WebResponse.Close()
        ' end of code replaced TJS 02/12/11

        ' was Activation Code found ?
        If strReturn.Length > 16 Then
            If "" & strReturn.Substring(0, 26) = "<LerrynPurchaseActivation>" Then
                Try ' TJS 22/03/13
                    XMLResponse = XDocument.Parse(strReturn) ' TJS 02/12/11

                Catch ex As Exception
                    ErrorDetails = ex.Message ' TJS 22/03/13
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse ' TJS 22/03/13
                    Return False ' TJS 22/03/13

                End Try
                ' did we get an error ?
                If "" & strReturn.Substring(0, 40) <> "<LerrynPurchaseActivation><ErrorDetails>" Then ' TJS 02/12/11
                    ' no, check for revised Activation Code

                    m_ValidatingActivation = True
                    strActivationCode = GetElementText(XMLResponse, "LerrynPurchaseActivation/ActivationCode").Replace("-", "")
                    If strActivationCode <> "" Then
                        ErrorCode = ValidateActivationCode(m_BaseProductCode, strActivationCode, SystemHashCode)
                        If ErrorCode = ErrorCodes.NoError Then
                            ErrorCode = AlphaToLong(strActivationCode.Substring(0, 1), iOffset)
                            If ErrorCode = ErrorCodes.NoError Then
                                strLicenceCode = strActivationCode.Substring(0, 1) & GetLicenceChars(strActivationCode.Substring(1), iOffset)
                                bUpdateExistingCode = False
                                For iLoop = 0 To m_ImportExportDataset.LerrynLicences_DEV000221.Count - 1
                                    If m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode) And _
                                        m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode) Then
                                        bUpdateExistingCode = True
                                        rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221(iLoop)
                                    End If
                                Next
                                If Not bUpdateExistingCode Then
                                    rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row
                                End If
                                rowLicence.CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                                rowLicence.LicenceCode_DEV000221 = strActivationCode
                                rowLicence.ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                                crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
                                salt = crypto.GenerateSalt
                                vector = crypto.GenerateVector

                                strTemp = EndFreeTrial.Year & "-" & Right("00" & EndFreeTrial.Month, 2) & "-" & Right("00" & EndFreeTrial.Day, 2) & ":"
                                strTemp = strTemp & NextInvoiceDue.Year & "-" & Right("00" & NextInvoiceDue.Month, 2) & "-" & Right("00" & NextInvoiceDue.Day, 2)
                                ' element 2 is Payment Failed Flag
                                ' element 3 is Last Notification date
                                ' element 4 is Payment Period
                                ' element 5 is Auto Renewal flag
                                If bAutoRenew Then
                                    strTemp = strTemp & ":F::" & ActivationPeriod & ":T"
                                Else
                                    strTemp = strTemp & ":F::" & ActivationPeriod & ":F"
                                End If
                                rowLicence.Data_DEV000221 = crypto.Encrypt(strTemp, salt, vector)
                                rowLicence.DataSalt_DEV000221 = System.Convert.ToBase64String(salt)
                                rowLicence.DataIV_DEV000221 = System.Convert.ToBase64String(vector)
                                If Not bUpdateExistingCode Then
                                    m_ImportExportDataset.LerrynLicences_DEV000221.AddLerrynLicences_DEV000221Row(rowLicence)
                                End If
                                ExtractActivationDetails(strActivationCode, True)
                                m_IsActivated = True
                                crypto = Nothing
                            Else
                                m_ValidatingActivation = False
                                Return False
                            End If
                        Else
                            m_ValidatingActivation = False
                            Return False
                        End If
                    End If

                    XMLConnectorList = XMLResponse.XPathSelectElements("LerrynPurchaseActivation/Connector")
                    For Each XMLConnectorNode In XMLConnectorList
                        XMLTemp = XDocument.Parse(XMLConnectorNode.ToString) ' TJS 02/12/11
                        strActivationCode = GetElementText(XMLTemp, "Connector/ActivationCode").Replace("-", "")
                        For iLoop = 1 To 12 ' TJS 02/12/11 TJS 16/01/12 TJS 05/07/12 TJS 20/08/13 TJS 20/11/13
                            Select Case iLoop
                                Case 1 ' ASPDotNetStorefront
                                    ErrorCode = ValidateActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                                Case 2 ' Magento
                                    ErrorCode = ValidateActivationCode(MAGENTO_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                                Case 3 ' Volusion
                                    ErrorCode = ValidateActivationCode(VOLUSION_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                                Case 4 ' Amazon
                                    ErrorCode = ValidateActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                                Case 5 ' Channel Advisor
                                    ErrorCode = ValidateActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                                Case 6 ' Shop.com
                                    ErrorCode = ValidateActivationCode(SHOP_DOT_COM_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                                Case 7 ' eBay TJS 02/12/11
                                    ErrorCode = ValidateActivationCode(EBAY_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 02/12/11

                                Case 8 ' Sears.com TJS 16/01/12
                                    ErrorCode = ValidateActivationCode(SEARS_DOT_COM_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 16/01/12

                                Case 9 ' Amazon FBA TJS 05/07/12
                                    ErrorCode = ValidateActivationCode(AMAZON_FBA_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 05/07/12

                                    ' start of code added TJS 20/08/13
                                Case 10 ' File Import
                                    ErrorCode = ValidateActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                                Case 11 ' Prospect and Lead
                                    ErrorCode = ValidateActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE, strActivationCode, SystemHashCode)
                                    ' end of code added TJS 20/08/13

                                    ' start of code added TJS 20/11/13
                                Case 12 ' 3DCart
                                    ErrorCode = ValidateActivationCode(THREE_D_CART_CONNECTOR_CODE, strActivationCode, SystemHashCode)
                                    ' end of code added TJS 20/11/13
                            End Select
                            If ErrorCode = ErrorCodes.NoError Then
                                Exit For
                            End If
                        Next
                        If ErrorCode = ErrorCodes.NoError Then
                            ErrorCode = AlphaToLong(strActivationCode.Substring(0, 1), iOffset)
                            If ErrorCode = ErrorCodes.NoError Then
                                strLicenceCode = strActivationCode.Substring(0, 1) & GetLicenceChars(strActivationCode.Substring(1), iOffset)
                                bUpdateExistingCode = False
                                For iLoop = 0 To m_ImportExportDataset.LerrynLicences_DEV000221.Count - 1
                                    If m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode) And _
                                        m_ImportExportDataset.LerrynLicences_DEV000221(iLoop).CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode) Then
                                        bUpdateExistingCode = True
                                        rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221(iLoop)
                                    End If
                                Next
                                If Not bUpdateExistingCode Then
                                    rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row
                                End If
                                rowLicence.CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                                rowLicence.LicenceCode_DEV000221 = strActivationCode
                                rowLicence.ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                                If Not bUpdateExistingCode Then
                                    m_ImportExportDataset.LerrynLicences_DEV000221.AddLerrynLicences_DEV000221Row(rowLicence)
                                End If
                                For iConnectorLoop = 0 To ConnectorStates.Length - 1
                                    If ConnectorStates(iConnectorLoop).ProductCode = rowLicence.ProductCode_DEV000221 Then
                                        ConnectorStates(iConnectorLoop).IsActivated = True
                                        ExtractConnectorActivationDetails(ConnectorStates(iConnectorLoop), strActivationCode, True)
                                        UpdateConnectorMaxAccounts(rowLicence.ProductCode_DEV000221, False, bConfigUpdated) ' TJS 02/12/11
                                        Exit For
                                    End If
                                Next
                            Else
                                m_ValidatingActivation = False
                                Return False
                            End If
                        Else
                            m_ValidatingActivation = False
                            Return False
                        End If
                    Next
                Else
                    ErrorDetails = GetElementText(XMLResponse, "LerrynPurchaseActivation/ErrorDetails") ' TJS  02/12/11
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse ' TJS  02/12/11
                    Return False ' TJS  02/12/11
                End If

            Else
                ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                Return False
            End If
        Else
            ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
            Return False
        End If
        Return True

    End Function
#End Region

#Region " InitialiseMonthlyPrecentageBilling "
    Public Function InitialiseMonthlyPrecentageBilling(ByVal CurrencyCode As String, ByVal EndFreeTrial As Date, ByVal FirstInvoiceDue As Date, _
        ByRef ErrorCode As Integer, ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    first activation or re-activation only
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to log activation requests if relevant registry key set
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, to add eBay conenctor 
        '                                        | and corrected sending of connector previous activation code
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 19/04/12 | TJS             | 2012.1.01 | Modified to ensure spaces in XML are handled properly
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to use Error Notification object to simplify facade login/logout
        ' 14/06/12 | TJS             | 2012.1.06 | Modified to filter set allowed activation currency
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings 
        ' 18/01/13 | TJS             | 2012.1.17 | Modified to remove credit/debit card registration
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strSend As String, strSystemLicenceID As String, strReturn As String, strActivationCode As String
        Dim strLicenceCode As String, strISCustID As String, strTemp As String, iOffset As Integer
        Dim iLoop As Integer, iConnectorLoop As Integer, dteStartDate As Date, dteExpiryDate As Date
        Dim bSaveLerrynCustCode As Boolean, bActivateConnector As Boolean, bLogActivation As Boolean ' TJS 25/04/11
        Dim bConfigUpdated As Boolean, salt As Byte(), vector As Byte() ' TJS 02/12/11

        dteStartDate = Date.Today
        bLogActivation = (CheckRegistryValue(REGISTRY_KEY_ROOT, "LogActivation", "NO").ToUpper = "YES") ' TJS 25/04/11

        ' has customer record been created and recorded ?
        If m_LerrynCustomerCode = "" Then
            ' no, need to create in Lerryn Systems (LERWEB)
            strSend = "<?xml version=""1.0"" encoding=""UTF-8""?><eShopCONNECT><ImportType>Prospect</ImportType>"
            strSend = strSend & "<Source><SourceName>eShopCONNECT</SourceName><SourceCode>GenericXMLImport</SourceCode></Source>"
            ' get IS Customer ID from Licence
            strISCustID = Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.CustomerCode
            If strISCustID <> "" Then
                strSend = strSend & "<Prospect><SourceProspectID>IS-" & strISCustID & "</SourceProspectID>"
            Else
                strSend = strSend & "<Prospect><SourceProspectID>" & m_SystemHashCode & "</SourceProspectID>"
            End If
            strSend = strSend & "<TradingCurrency>" & GetActivationCurrency(CurrencyCode) & "</TradingCurrency><BillingDetails><Contact>" ' TJS 14/06/12
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingSalutationNull Then
                strSend = strSend & "<NamePrefix>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingSalutation) & "</NamePrefix>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingFirstNameNull Then
                strSend = strSend & "<FirstName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingFirstName) & "</FirstName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingLastNameNull Then
                strSend = strSend & "<LastName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingLastName) & "</LastName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCompanyNameNull Then
                strSend = strSend & "<Company>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCompanyName) & "</Company>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingPhoneNull Then
                strSend = strSend & "<WorkPhone>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingPhone) & "</WorkPhone>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingPhoneExtensionNull Then
                strSend = strSend & "<PhoneExtn>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingPhoneExtension) & "</PhoneExtn>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsEmailNull Then
                strSend = strSend & "<Email>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).Email) & "</Email>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsPasswordNull Then
                strSend = strSend & "<LoginPwd>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).Password) & "</LoginPwd>"
            End If
            strSend = strSend & "</Contact><BillingAddress>"
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingAddressNull Then
                strSend = strSend & "<Address>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingAddress) & "</Address>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCityNull Then
                strSend = strSend & "<Town_City>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCity) & "</Town_City>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingStateNull Then
                strSend = strSend & "<State>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingState) & "</State>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCountyNull Then
                strSend = strSend & "<County>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCounty) & "</County>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingPostalCodeNull Then
                strSend = strSend & "<PostalCode>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingPostalCode) & "</PostalCode>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsBillingCountryNull Then
                strSend = strSend & "<Country>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).BillingCountry) & "</Country>"
            End If
            strSend = strSend & "</BillingAddress></BillingDetails><ShippingDetails><Contact>"
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingSalutationNull Then
                strSend = strSend & "<NamePrefix>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingSalutation) & "</NamePrefix>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingFirstNameNull Then
                strSend = strSend & "<FirstName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingFirstName) & "</FirstName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingLastNameNull Then
                strSend = strSend & "<LastName>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingLastName) & "</LastName>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCompanyNameNull Then
                strSend = strSend & "<Company>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCompanyName) & "</Company>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingPhoneNull Then
                strSend = strSend & "<Phone>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingPhone) & "</Phone>"
            End If
            strSend = strSend & "</Contact><ShippingAddress>"
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingAddressNull Then
                strSend = strSend & "<Address>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingAddress) & "</Address>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCityNull Then
                strSend = strSend & "<Town_City>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCity) & "</Town_City>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingStateNull Then
                strSend = strSend & "<State>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingState) & "</State>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCountyNull Then
                strSend = strSend & "<County>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCounty) & "</County>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingPostalCodeNull Then
                strSend = strSend & "<PostalCode>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingPostalCode) & "</PostalCode>"
            End If
            If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).IsShippingCountyNull Then
                strSend = strSend & "<Country>" & ConvertEntitiesForXML(Me.m_ImportExportDataset.ActivationAccountDetails(0).ShippingCountry) & "</Country>"
            End If
            strSend = strSend & "</ShippingAddress><ShippingMethod>No Shipping Required</ShippingMethod>"
            strSend = strSend & "<ShippingMethodGroup>WEBSHIP</ShippingMethodGroup></ShippingDetails>"
            strSend = strSend & "</Prospect><ProspectCount>1</ProspectCount></eShopCONNECT>"
            strSend = strSend.Replace(" ", "%20") ' TJS 19/04/12

            ' start of code replaced TJS 02/12/11
            Try
                If GetActivationCurrency(CurrencyCode) = "USD" Then ' TJS 14/06/12
                    WebSubmit = System.Net.WebRequest.Create("https://www.lerrynsecure.com/eShopCONNECTWebImportUS/GenericXMLImport.ashx")
                Else
                    WebSubmit = System.Net.WebRequest.Create("https://www.lerrynsecure.com/eShopCONNECTWebImport/GenericXMLImport.ashx")
                End If
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = HttpUtility.UrlEncode(strSend).Length
                WebSubmit.Timeout = 30000

                ' start of code added TJS 25/04/11
                If bLogActivation Then
                    Try
                        Dim writer As StreamWriter
                        If m_ErrorNotification.LogFilePath <> "" Then ' TJS 10/06/12
                            If m_ErrorNotification.LogFilePath.Substring(m_ErrorNotification.LogFilePath.Length - 1) <> "\" Then ' TJS 10/06/12
                                m_ErrorNotification.LogFilePath = m_ErrorNotification.LogFilePath & "\" ' TJS 10/06/12
                            End If
                        Else
                            m_ErrorNotification.LogFilePath = System.AppDomain.CurrentDomain.BaseDirectory() ' TJS 10/03/09 TJS 10/06/12
                        End If

                        'Start the logging to the file
                        writer = New StreamWriter(m_ErrorNotification.LogFilePath & "RegisterCustomer_" & Format(Now.Date.ToString("yyyyMMdd") & ".txt"), True) ' TJS 10/06/12

                        writer.Write(strSend)
                        writer.Close()

                    Catch Ex As Exception
                        ' ignore errors
                    End Try
                End If
                ' end of code added TJS 25/04/11

                byteData = UTF8Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(strSend))

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()

                XMLResponse = XDocument.Parse(strReturn)
                If GetElementText(XMLResponse, "eShopCONNECT/ImportResponse/Status") = "Success" Then
                    m_LerrynCustomerCode = GetElementText(XMLResponse, "eShopCONNECT/ImportResponse/ProspectCode")
                    If Me.m_ImportExportDataset.System_DEV000221.Count > 0 Then
                        bSaveLerrynCustCode = False
                        If Me.m_ImportExportDataset.System_DEV000221(0).IsCustCode_DEV000221Null Then
                            bSaveLerrynCustCode = True
                        Else
                            If Me.m_ImportExportDataset.System_DEV000221(0).CustCode_DEV000221 <> m_LerrynCustomerCode Then
                                bSaveLerrynCustCode = True
                            End If
                        End If
                        If bSaveLerrynCustCode Then
                            crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
                            salt = crypto.GenerateSalt
                            vector = crypto.GenerateVector

                            Me.m_ImportExportDataset.System_DEV000221(0).CustCode_DEV000221 = crypto.Encrypt(m_LerrynCustomerCode, salt, vector)
                            Me.m_ImportExportDataset.System_DEV000221(0).CCSalt_DEV000221 = System.Convert.ToBase64String(salt)
                            Me.m_ImportExportDataset.System_DEV000221(0).CCIV_DEV000221 = System.Convert.ToBase64String(vector)
                            crypto = Nothing
                        End If
                    End If
                Else
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                    Return False
                End If

            Catch ex As Exception
                ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()
                Return False

            End Try
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()
            ' end of code replaced TJS 02/12/11
        End If

        ' card registration code removed TJS 18/01/13

        ' now generate activations
        ' start with base module
        strSystemLicenceID = BuildSystemRegID(SystemID, CacheID)
        ' is base module active ?
        If Not m_IsActivated Then
            ' no - set expiry date as 14 days after first invoice due date (this gives time for card retries etc)
            dteExpiryDate = FirstInvoiceDue.AddDays(14)
            strSend = "<LerrynISPlugin><Login><Source>ISIntegrated</Source><Password>K$0eW*zU2B</Password></Login>"
            strSend = strSend & "<Activation><SystemID>" & strSystemLicenceID & "</SystemID><SysIDType>ISSysID</SysIDType>"
            strSend = strSend & "<ProductCode>" & m_BaseProductCode & "</ProductCode><OperatingSystem>Z</OperatingSystem>"
            If m_LatestActivationCode <> "" Then
                strSend = strSend & "<Language>V</Language><PreviousActivation>" & m_LatestActivationCode.Substring(0, 5) ' TJS 02/12/11
                strSend = strSend & m_LatestActivationCode.Substring(5, 5) & "-" & m_LatestActivationCode.Substring(10, 5) & "-"
                strSend = strSend & m_LatestActivationCode.Substring(15, 5) & "-" & m_LatestActivationCode.Substring(20, 5) & "-"
                strSend = strSend & m_LatestActivationCode.Substring(25, 5) & "-" & m_LatestActivationCode.Substring(30, 5) & "</PreviousActivation><StartDate>"
            Else
                strSend = strSend & "<Language>V</Language><StartDate>"
            End If
            strSend = strSend & dteStartDate.Year & "-" & Right("00" & dteStartDate.Month, 2) & "-" & Right("00" & dteStartDate.Day, 2)
            strSend = strSend & "</StartDate><ExpiryDateType>Date</ExpiryDateType><ExpiryDate>" & dteExpiryDate.Year & "-"
            strSend = strSend & Right("00" & dteExpiryDate.Month, 2) & "-" & Right("00" & dteExpiryDate.Day, 2) & "</ExpiryDate>"
            strSend = strSend & "<MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>NoLimit</MaxSecondaryCount><EvalOrFull>Full</EvalOrFull>"
            strSend = strSend & "<InvoiceNo>IS eCPlus Trial</InvoiceNo><LerrynCustCode>" & m_LerrynCustomerCode
            strSend = strSend & "</LerrynCustCode><SendHash>Yes</SendHash><TransactionCharging>Yes</TransactionCharging>"
            strSend = strSend & "<TransactionInvoiceDue>" & FirstInvoiceDue.Year & "-" & Right("00" & FirstInvoiceDue.Month, 2)
            strSend = strSend & "-" & Right("00" & FirstInvoiceDue.Day, 2) & "</TransactionInvoiceDue><FreeUsageUntilDate>"
            strSend = strSend & EndFreeTrial.Year & "-" & Right("00" & EndFreeTrial.Month, 2) & "-" & Right("00" & EndFreeTrial.Day, 2)
            strSend = strSend & "</FreeUsageUntilDate></Activation></LerrynISPlugin>"
            strSend = strSend.Replace(" ", "%20") ' TJS 19/04/12

            ' start of code replaced TJS 02/12/11
            Try
                WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "CreateActivationCode.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = HttpUtility.UrlEncode(strSend).Length
                WebSubmit.Timeout = 30000

                ' start of code added TJS 25/04/11
                If bLogActivation Then
                    Try
                        Dim writer As StreamWriter
                        If m_ErrorNotification.LogFilePath <> "" Then ' TJS 10/06/12
                            If m_ErrorNotification.LogFilePath.Substring(m_ErrorNotification.LogFilePath.Length - 1) <> "\" Then ' TJS 10/06/12
                                m_ErrorNotification.LogFilePath = m_ErrorNotification.LogFilePath & "\" ' TJS 10/06/12
                            End If
                        Else
                            m_ErrorNotification.LogFilePath = System.AppDomain.CurrentDomain.BaseDirectory() ' TJS 10/06/12
                        End If

                        'Start the logging to the file
                        writer = New StreamWriter(m_ErrorNotification.LogFilePath & "GetActivation_" & Format(Now.Date.ToString("yyyyMMdd") & ".txt"), True) ' TJS 10/06/12

                        writer.Write(strSend)
                        writer.Close()

                    Catch Ex As Exception
                        ' ignore errors
                    End Try
                End If
                ' end of code added TJS 25/04/11

                byteData = UTF8Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(strSend))

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()

            Catch ex As Exception
                ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()
                Return False

            End Try
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()
            ' end of code replaced TJS 02/12/11

            ' was Activation Code found ?
            If strReturn.Length > 16 Then
                If "" & strReturn.Substring(0, 16) = "<ActivationCode>" Then
                    ' yes, check for revised Activation Code
                    XMLResponse = XDocument.Parse("<Activation>" & Trim(strReturn) & "</Activation>") ' TJS 02/12/11

                    If Me.m_ImportExportDataset.System_DEV000221.Count > 0 Then
                        If Me.m_ImportExportDataset.System_DEV000221(0).IsRegVal_DEV000221Null Then
                            Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 = GetElementText(XMLResponse, "Activation/Hash")
                        Else
                            If Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 <> GetElementText(XMLResponse, "Activation/Hash") Then
                                Me.m_ImportExportDataset.System_DEV000221(0).RegVal_DEV000221 = GetElementText(XMLResponse, "Activation/Hash")
                            End If
                        End If
                    End If
                    m_ValidatingActivation = True
                    strActivationCode = GetElementText(XMLResponse, "Activation/ActivationCode").Replace("-", "")
                    ErrorCode = ValidateActivationCode(m_BaseProductCode, strActivationCode, SystemHashCode)
                    If ErrorCode = ErrorCodes.NoError Then
                        ErrorCode = AlphaToLong(strActivationCode.Substring(0, 1), iOffset)
                        If ErrorCode = ErrorCodes.NoError Then
                            strLicenceCode = strActivationCode.Substring(0, 1) & GetLicenceChars(strActivationCode.Substring(1), iOffset)
                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row
                            rowLicence.CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                            rowLicence.LicenceCode_DEV000221 = strActivationCode
                            rowLicence.ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                            crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
                            salt = crypto.GenerateSalt
                            vector = crypto.GenerateVector

                            strTemp = EndFreeTrial.Year & "-" & Right("00" & EndFreeTrial.Month, 2) & "-" & Right("00" & EndFreeTrial.Day, 2) & ":"
                            strTemp = strTemp & FirstInvoiceDue.Year & "-" & Right("00" & FirstInvoiceDue.Month, 2) & "-" & Right("00" & FirstInvoiceDue.Day, 2)
                            ' element 2 is Payment Failed Flag
                            ' element 3 is Last Notification date
                            ' element 4 is Payment Period
                            ' element 5 is Auto Renewal flag
                            strTemp = strTemp & ":F::M:F"
                            rowLicence.Data_DEV000221 = crypto.Encrypt(strTemp, salt, vector)
                            rowLicence.DataSalt_DEV000221 = System.Convert.ToBase64String(salt)
                            rowLicence.DataIV_DEV000221 = System.Convert.ToBase64String(vector)
                            m_ImportExportDataset.LerrynLicences_DEV000221.AddLerrynLicences_DEV000221Row(rowLicence)
                            ExtractActivationDetails(strActivationCode, True)
                            m_IsActivated = True
                            crypto = Nothing

                        Else
                            m_ValidatingActivation = False
                            Return False
                        End If
                    Else
                        m_ValidatingActivation = False
                        Return False
                    End If
                    m_ValidatingActivation = False

                Else
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                    Return False
                End If
            Else
                ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                Return False
            End If
        End If

        ' now do connector activations
        For iLoop = 1 To 12 ' TJS 02/12/11 TJS 16/01/12 TJS 05/07/12 TJS 20/08/13 TJS 20/11/13
            bActivateConnector = False
            Select Case iLoop
                Case 1 ' ASPDotNetStorefront
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateASPStorefront And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount > 0 And _
                        Not IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If

                Case 2 ' Magento
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateMagento And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).MagentoCount > 0 And _
                        Not IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If

                Case 3 ' Volusion
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateVolusion And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).VolusionCount > 0 And _
                        Not IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If

                Case 4 ' Amazon
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazon And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonCount > 0 And _
                        Not IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If

                Case 5 ' Channel Advisor
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateChanAdv And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ChanAdvCount > 0 And _
                        Not IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If

                Case 6 ' Shop.com
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateShopCom And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ShopComCount > 0 And _
                        Not IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If

                Case 7 ' eBay TJS 02/12/11
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateEBay And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount > 0 And _
                        Not IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                        bActivateConnector = True ' TJS 02/12/11
                    End If

                Case 8 ' Sears.com TJS 16/01/12
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateSearsCom And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount > 0 And _
                        Not IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                        bActivateConnector = True ' TJS 16/01/12
                    End If

                    ' start of code added TJS 05/07/12
                Case 9 ' Amazon FBA
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonFBACount > 0 And _
                        Not IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If
                    ' end of code added TJS 05/07/12

                    ' start of code added TJS 20/08/13
                Case 10 ' File Import
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateFileImport And _
                        Not IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If

                Case 11 ' Prospect and Lead
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateProspectLead And _
                        Not IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If
                    ' end of code added TJS 20/08/13

                    ' start of code added TJS 20/11/13
                Case 12 ' 3DCart
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).Activate3DCart And _
                        Not IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                        bActivateConnector = True
                    End If
                    ' end of code added TJS 20/11/13
            End Select
            If bActivateConnector Then
                strSend = "<LerrynISPlugin><Login><Source>ISIntegrated</Source><Password>K$0eW*zU2B</Password></Login>"
                strSend = strSend & "<Activation><SystemID>" & strSystemLicenceID & "</SystemID><SysIDType>ISSysID</SysIDType>"
                Select Case iLoop
                    Case 1 ' ASPDotNetStorefront
                        strSend = strSend & "<ProductCode>" & ASP_STORE_FRONT_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 2 ' Magento
                        strSend = strSend & "<ProductCode>" & MAGENTO_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 3 ' Volusion
                        strSend = strSend & "<ProductCode>" & VOLUSION_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 4 ' Amazon
                        strSend = strSend & "<ProductCode>" & AMAZON_SELLER_CENTRAL_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 5 ' Channel Advisor
                        strSend = strSend & "<ProductCode>" & CHANNEL_ADVISOR_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 6 ' Shop.com
                        strSend = strSend & "<ProductCode>" & SHOP_DOT_COM_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 7 ' eBay TJS 02/12/11
                        strSend = strSend & "<ProductCode>" & EBAY_CONNECTOR_CODE & "</ProductCode>" ' TJS 02/12/11
                        If ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE) <> "" Then ' TJS 02/12/11
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(5, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                            strSend = strSend & "</PreviousActivation>" ' TJS 02/12/11
                        End If

                    Case 8 ' Sears.com TJS 16/01/12
                        strSend = strSend & "<ProductCode>" & SEARS_DOT_COM_CONNECTOR_CODE & "</ProductCode>" ' TJS 16/01/12
                        If ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE) <> "" Then ' TJS 16/01/12
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(5, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 16/01/12
                            strSend = strSend & "</PreviousActivation>" ' TJS 16/01/12
                        End If

                        ' start of code added TJS 05/07/12
                    Case 9 ' Amazon FBA
                        strSend = strSend & "<ProductCode>" & AMAZON_FBA_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        ' end of code added TJS 05/07/12

                        ' start of code added TJS 20/08/13
                    Case 10 ' File Import
                        strSend = strSend & "<ProductCode>" & ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 11 ' Prospect and Lead
                        strSend = strSend & "<ProductCode>" & PROSPECT_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        ' end of code added TJS 20/08/13

                        ' start of code added TJS 20/11/13
                    Case 12 ' 3DCart
                        strSend = strSend & "<ProductCode>" & THREE_D_CART_CONNECTOR_CODE & "</ProductCode>"
                        If ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE) <> "" Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        ' end of code added TJS 20/11/13

                End Select
                strSend = strSend & "<OperatingSystem>Z</OperatingSystem><Language>V</Language><StartDate>" & "" & dteStartDate.Year
                strSend = strSend & "-" & Right("00" & dteStartDate.Month, 2) & "-" & Right("00" & dteStartDate.Day, 2)
                strSend = strSend & "</StartDate><ExpiryDateType>Date</ExpiryDateType><ExpiryDate>" & dteExpiryDate.Year
                strSend = strSend & "-" & Right("00" & dteExpiryDate.Month, 2) & "-" & Right("00" & dteExpiryDate.Day, 2)
                strSend = strSend & "</ExpiryDate><MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>"
                Select Case iLoop
                    Case 1 ' ASPDotNetStorefront
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount

                    Case 2 ' Magento
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).MagentoCount

                    Case 3 ' Volusion
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).VolusionCount

                    Case 4 ' Amazon
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonCount

                    Case 5 ' Channel Advisor
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ChanAdvCount

                    Case 6 ' Shop.com
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ShopComCount

                    Case 7 ' eBay TJS 02/12/11
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount 'TJS 02/12/11

                    Case 8 ' Sears.com TJS 16/01/12
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount 'TJS 16/01/12

                    Case 9 ' Amazon FBA TJS 05/07/12
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonFBACount ' TJS 05/07/12

                        ' start of code added TJS 20/08/13
                    Case 10 ' File Import
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateFileImport Then
                            strSend = strSend & "1"
                        Else
                            strSend = strSend & "0"
                        End If

                    Case 11 ' Prospect and Lead
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateProspectLead Then
                            strSend = strSend & "1"
                        Else
                            strSend = strSend & "0"
                        End If
                        ' end of code added TJS 20/08/13

                        ' start of code added TJS 20/11/13
                    Case 12 ' 3DCart
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ThreeDCartCount
                        ' end of code added TJS 20/11/13

                End Select
                strSend = strSend & "</MaxSecondaryCount><EvalOrFull>Full</EvalOrFull><InvoiceNo>IS eCPlus Trial</InvoiceNo>"
                strSend = strSend & "<LerrynCustCode>" & m_LerrynCustomerCode & "</LerrynCustCode><TransactionCharging>Yes</TransactionCharging>"
                strSend = strSend & "<TransactionInvoiceDue>" & FirstInvoiceDue.Year & "-" & Right("00" & FirstInvoiceDue.Month, 2)
                strSend = strSend & "-" & Right("00" & FirstInvoiceDue.Day, 2) & "</TransactionInvoiceDue><FreeUsageUntilDate>"
                strSend = strSend & EndFreeTrial.Year & "-" & Right("00" & EndFreeTrial.Month, 2) & "-" & Right("00" & EndFreeTrial.Day, 2)
                strSend = strSend & "</FreeUsageUntilDate></Activation></LerrynISPlugin>"

                ' start of code replaced TJS 02/12/11
                Try
                    WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "CreateActivationCode.ashx")
                    WebSubmit.Method = "POST"
                    WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                    WebSubmit.ContentLength = strSend.Length
                    WebSubmit.Timeout = 30000

                    byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                    ' send to LerrynSecure.com (or the URL defined in the Registry)
                    postStream = WebSubmit.GetRequestStream()
                    postStream.Write(byteData, 0, byteData.Length)

                    WebResponse = WebSubmit.GetResponse
                    reader = New StreamReader(WebResponse.GetResponseStream())
                    strReturn = reader.ReadToEnd()

                Catch ex As Exception
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                    If Not postStream Is Nothing Then postStream.Close()
                    If Not WebResponse Is Nothing Then WebResponse.Close()
                    Return False

                End Try
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()
                ' end of code replaced TJS 02/12/11

                ' was Activation Code found ?
                If strReturn.Length > 16 Then
                    If "" & strReturn.Substring(0, 16) = "<ActivationCode>" Then
                        ' yes, check for revised Activation Code
                        XMLResponse = XDocument.Parse("<Activation>" & Trim(strReturn) & "</Activation>") ' TJS 02/12/11

                        m_ValidatingActivation = True
                        strActivationCode = GetElementText(XMLResponse, "Activation/ActivationCode").Replace("-", "")
                        Select Case iLoop
                            Case 1 ' ASPDotNetStorefront
                                ErrorCode = ValidateActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 2 ' Magento
                                ErrorCode = ValidateActivationCode(MAGENTO_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 3 ' Volusion
                                ErrorCode = ValidateActivationCode(VOLUSION_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 4 ' Amazon
                                ErrorCode = ValidateActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 5 ' Channel Advisor
                                ErrorCode = ValidateActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 6 ' Shop.com
                                ErrorCode = ValidateActivationCode(SHOP_DOT_COM_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 7 ' eBay TJS 02/12/11
                                ErrorCode = ValidateActivationCode(EBAY_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 02/12/11

                            Case 8 ' Sears.com TJS 16/01/12
                                ErrorCode = ValidateActivationCode(SEARS_DOT_COM_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 16/01/12

                            Case 9 ' Amazon FBA TJS 05/07/12
                                ErrorCode = ValidateActivationCode(AMAZON_FBA_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 05/07/12

                                ' start of code added TJS 20/08/13
                            Case 10 ' File Import
                                ErrorCode = ValidateActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 11 ' Prospect and Lead
                                ErrorCode = ValidateActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE, strActivationCode, SystemHashCode)
                                ' end of code added TJS 20/08/13

                                ' start of code added TJS 20/11/13
                            Case 12 ' 3DCart
                                ErrorCode = ValidateActivationCode(THREE_D_CART_CONNECTOR_CODE, strActivationCode, SystemHashCode)
                                ' end of code added TJS 20/11/13

                        End Select
                        If ErrorCode = ErrorCodes.NoError Then
                            ErrorCode = AlphaToLong(strActivationCode.Substring(0, 1), iOffset)
                            If ErrorCode = ErrorCodes.NoError Then
                                strLicenceCode = strActivationCode.Substring(0, 1) & GetLicenceChars(strActivationCode.Substring(1), iOffset)
                                rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row
                                rowLicence.CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                                rowLicence.LicenceCode_DEV000221 = strActivationCode
                                rowLicence.ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                                m_ImportExportDataset.LerrynLicences_DEV000221.AddLerrynLicences_DEV000221Row(rowLicence)
                                For iConnectorLoop = 0 To ConnectorStates.Length - 1
                                    If ConnectorStates(iConnectorLoop).ProductCode = rowLicence.ProductCode_DEV000221 Then
                                        ConnectorStates(iConnectorLoop).IsActivated = True
                                        ExtractConnectorActivationDetails(ConnectorStates(iConnectorLoop), strActivationCode, True)
                                        UpdateConnectorMaxAccounts(rowLicence.ProductCode_DEV000221, False, bConfigUpdated) ' TJS 02/12/11
                                        Exit For
                                    End If
                                Next

                            Else
                                m_ValidatingActivation = False
                                Return False
                            End If
                        Else
                            m_ValidatingActivation = False
                            Return False
                        End If
                        m_ValidatingActivation = False

                    Else
                        ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                        Return False
                    End If
                Else
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                    Return False
                End If
            End If
        Next
        Return True

    End Function
#End Region

#Region " UpdateMonthlyPercentageBilling "
    Public Function UpdateMonthlyPercentageBilling(ByVal CurrencyCode As String, ByRef ErrorCode As Integer, ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Used while product active only
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to log activation requests if relevant registry key set
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, to add eBay conenctor 
        '                                        | and corrected sending of connector previous activation code
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to use Error Notification object to simplify facade login/logout
        ' 14/06/12 | TJS             | 2012.1.06 | Modified to filter set allowed activation currency
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings 
        ' 18/01/13 | TJS             | 2012.1.17 | Modified to remove credit/debit card registration
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim crypto As Interprise.Licensing.Base.Services.CryptoServiceProvider
        Dim rowLicence As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynLicences_DEV000221Row
        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim encrypteddata As Byte(), salt As Byte(), vector As Byte()
        Dim strSend As String, strSystemLicenceID As String, strActivationDates As String, strReturn As String
        Dim strLicenceCode As String, strActivationCode As String, strDecrypted() As String
        Dim strDateParts() As String, iLoop As Integer, iOffset As Integer, iConnectorLoop As Integer
        Dim bActivateConnector As Boolean, bUpdateConnector As Boolean, bCancelConnector As Boolean
        Dim bPaymentFailed As Boolean, bLogActivation As Boolean, bConfigUpdated As Boolean ' TJS 25/04/11 TJS 02/12/11
        Dim dteEndFreeTrial As Date, dteStartDate As Date, dteNextInvoiceDue As Date

        ' check and update connector activations
        strSystemLicenceID = BuildSystemRegID(SystemID, CacheID)
        bLogActivation = (CheckRegistryValue(REGISTRY_KEY_ROOT, "LogActivation", "NO").ToUpper = "YES") ' TJS 25/04/11

        crypto = New Interprise.Licensing.Base.Services.CryptoServiceProvider
        rowLicence = Me.m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(m_LatestActivationCode)
        If Not rowLicence.IsData_DEV000221Null And Not rowLicence.IsDataSalt_DEV000221Null And Not rowLicence.IsDataIV_DEV000221Null Then
            encrypteddata = System.Convert.FromBase64String(rowLicence.Data_DEV000221)
            salt = System.Convert.FromBase64String(rowLicence.DataSalt_DEV000221)
            vector = System.Convert.FromBase64String(rowLicence.DataIV_DEV000221)
        Else
            encrypteddata = Nothing
            salt = Nothing
            vector = Nothing
        End If

        ' set default end of free trial as yesterday i.e. expired
        dteEndFreeTrial = Date.Today.AddDays(-1)
        dteNextInvoiceDue = dteEndFreeTrial.AddMonths(1).AddDays(1)
        bPaymentFailed = False
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
                    End If
                End If
                If strDecrypted.Length >= 3 Then
                    If strDecrypted(2) = "T" Then
                        bPaymentFailed = True
                    End If
                End If
                ' element 3 is Last Notification date
                ' element 4 is Payment Period
                ' element 5 is Auto Renewal flag
            End If
        End If

        For iLoop = 1 To 12 ' TJS 02/12/11 TJS 16/01/12 TJS 05/07/12 TJS 20/08/13 TJS 20/11/13
            bActivateConnector = False
            bUpdateConnector = False
            bCancelConnector = False
            Select Case iLoop
                Case 1 ' ASPDotNetStorefront
                    ' is connector current inactive and user wants to activate it ?
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateASPStorefront And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount > 0 And _
                        Not IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        ' yes
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateASPStorefront And _
                        IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount <> ConnectorAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateASPStorefront And _
                        IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If

                Case 2 ' Magento
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateMagento And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).MagentoCount > 0 And _
                        Not IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateMagento And _
                        IsConnectorActivated(MAGENTO_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).MagentoCount <> ConnectorAccountLimit(MAGENTO_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateMagento And _
                        IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If

                Case 3 ' Volusion
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateVolusion And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).VolusionCount > 0 And _
                        Not IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateVolusion And _
                        IsConnectorActivated(VOLUSION_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).VolusionCount <> ConnectorAccountLimit(VOLUSION_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateVolusion And _
                        IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If

                Case 4 ' Amazon
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazon And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonCount > 0 And _
                        Not IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazon And _
                        IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonCount <> ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazon And _
                        IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If

                Case 5 ' Channel Advisor
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateChanAdv And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ChanAdvCount > 0 And _
                        Not IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateChanAdv And _
                        IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ChanAdvCount <> ConnectorAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateChanAdv And _
                        IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If

                Case 6 ' Shop.com
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateShopCom And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ShopComCount > 0 And _
                        Not IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateShopCom And _
                        IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ShopComCount <> ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateShopCom And _
                        IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If

                Case 7 ' eBay TJS 02/12/11
                    ' is connector current inactive and user wants to activate it ?
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateEBay And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount > 0 And _
                        Not IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                        ' yes
                        bActivateConnector = True ' TJS 02/12/11

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateEBay And _
                        IsConnectorActivated(EBAY_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount <> ConnectorAccountLimit(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True ' TJS 02/12/11

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateEBay And _
                        IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True ' TJS 02/12/11
                    End If

                Case 8 ' Sears.com TJS 16/01/12
                    ' is connector current inactive and user wants to activate it ?
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateSearsCom And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount > 0 And _
                        Not IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                        ' yes
                        bActivateConnector = True ' TJS 16/01/12

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateSearsCom And _
                        IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount <> ConnectorAccountLimit(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True ' TJS 16/01/12

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateSearsCom And _
                        IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True ' TJS 16/01/12
                    End If

                    ' start of code added TJS 05/07/12
                Case 9 ' Amazon FBA
                    ' is connector current inactive and user wants to activate it ?
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonFBACount > 0 And _
                        Not IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                        ' yes
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonFBACount <> ConnectorAccountLimit(AMAZON_FBA_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateAmazonFBA And _
                        IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If
                    ' end of code added TJS 05/07/12

                    ' start of code added TJS 20/08/13
                Case 10 ' File Import
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateFileImport And _
                        Not IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateFileImport And _
                       IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If

                Case 11 ' Prospect and Lead
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateProspectLead And _
                        Not IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateProspectLead And _
                       IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If
                    ' end of code added TJS 20/08/13

                    ' start of code added TJS 20/11/13
                Case 12 ' 3DCart
                    If Me.m_ImportExportDataset.ActivationAccountDetails(0).Activate3DCart And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ThreeDCartCount > 0 And _
                        Not IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                        bActivateConnector = True

                    ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).Activate3DCart And _
                        IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) And _
                        Me.m_ImportExportDataset.ActivationAccountDetails(0).ThreeDCartCount <> ConnectorAccountLimit(THREE_D_CART_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants more or less connections
                        bUpdateConnector = True

                    ElseIf Not Me.m_ImportExportDataset.ActivationAccountDetails(0).Activate3DCart And _
                        IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                        ' connector currently active, but user wants to cancel it
                        bCancelConnector = True
                    End If
                    ' end of code added TJS 20/11/13

            End Select
            If bActivateConnector Or bUpdateConnector Then
                strSend = "<LerrynISPlugin><Login><Source>ISIntegrated</Source><Password>K$0eW*zU2B</Password></Login>"
                strSend = strSend & "<Activation><SystemID>" & strSystemLicenceID & "</SystemID><SysIDType>ISSysID</SysIDType>"
                Select Case iLoop
                    Case 1 ' ASPDotNetStorefront
                        strSend = strSend & "<ProductCode>" & ASP_STORE_FRONT_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 2 ' Magento
                        strSend = strSend & "<ProductCode>" & MAGENTO_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 3 ' Volusion
                        strSend = strSend & "<ProductCode>" & VOLUSION_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 4 ' Amazon
                        strSend = strSend & "<ProductCode>" & AMAZON_SELLER_CENTRAL_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 5 ' Channel Advisor
                        strSend = strSend & "<ProductCode>" & CHANNEL_ADVISOR_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 6 ' Shop.com
                        strSend = strSend & "<ProductCode>" & SHOP_DOT_COM_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 7 ' eBay TJS 02/12/11
                        strSend = strSend & "<ProductCode>" & EBAY_CONNECTOR_CODE & "</ProductCode>" ' TJS 02/12/11
                        If bUpdateConnector Then ' TJS 02/12/11
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(5, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                            strSend = strSend & "</PreviousActivation>" ' TJS 02/12/11
                        End If

                    Case 8 ' Sears.com TJS 16/01/12
                        strSend = strSend & "<ProductCode>" & SEARS_DOT_COM_CONNECTOR_CODE & "</ProductCode>" ' TJS 16/01/12
                        If bUpdateConnector Then ' TJS 16/01/12
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(5, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 16/01/12
                            strSend = strSend & "</PreviousActivation>" ' TJS 16/01/12
                        End If

                        ' start of code added TJS 05/07/12
                    Case 9 ' Amazon FBA
                        strSend = strSend & "<ProductCode>" & AMAZON_FBA_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        ' end of code added TJS 05/07/12

                        ' start of code added TJS 20/08/13
                    Case 10 ' File Import
                        strSend = strSend & "<ProductCode>" & ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If

                    Case 11 ' Prospect and Lead
                        strSend = strSend & "<ProductCode>" & PROSPECT_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        ' end of code added TJS 20/08/13

                        ' start of code added TJS 20/11/13
                    Case 12 ' 3DCart
                        strSend = strSend & "<ProductCode>" & THREE_D_CART_CONNECTOR_CODE & "</ProductCode>"
                        If bUpdateConnector Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        ' end of code added TJS 20/11/13
                End Select
                dteStartDate = Date.Today
                strSend = strSend & "<OperatingSystem>Z</OperatingSystem><Language>V</Language>"
                strSend = strSend & "<StartDate>" & dteStartDate.Year & "-" & Right("00" & dteStartDate.Month, 2) & "-" & Right("00" & dteStartDate.Day, 2) & "</StartDate>"
                strSend = strSend & "<ExpiryDateType>Date</ExpiryDateType><ExpiryDate>" & m_ExpiryDate.Year & "-"
                strSend = strSend & Right("00" & m_ExpiryDate.Month, 2) & "-" & Right("00" & m_ExpiryDate.Day, 2)
                strSend = strSend & "</ExpiryDate><MaxUsers>NoLimit</MaxUsers><MaxSecondaryCount>"
                Select Case iLoop
                    Case 1 ' ASPDotNetStorefront
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount

                    Case 2 ' Magento
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).MagentoCount

                    Case 3 ' Volusion
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).VolusionCount

                    Case 4 ' Amazon
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonCount

                    Case 5 ' Channel Advisor
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ChanAdvCount

                    Case 6 ' Shop.com
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ShopComCount

                    Case 7 ' eBay TJS 02/12/11
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount ' TJS 02/12/11

                    Case 8 ' Sears.com TJS 16/01/12
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount ' TJS 02/12/11

                    Case 9 ' Amazon FBA TJS 05/07/12
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonFBACount ' TJS 05/07/12

                        ' start of code added TJS 20/08/13
                    Case 10 ' File Import
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateFileImport Then
                            strSend = strSend & "1"
                        Else
                            strSend = strSend & "0"
                        End If

                    Case 11 ' Prospect and Lead
                        If Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateProspectLead Then
                            strSend = strSend & "1"
                        Else
                            strSend = strSend & "0"
                        End If
                        ' end of code added TJS 20/08/13

                        ' start of code added TJS 20/11/13
                    Case 12 ' 3DCart
                        strSend = strSend & Me.m_ImportExportDataset.ActivationAccountDetails(0).ThreeDCartCount
                        ' end of code added TJS 20/11/13
                End Select
                strSend = strSend & "</MaxSecondaryCount><EvalOrFull>Full</EvalOrFull><InvoiceNo>IS eCPlus Update</InvoiceNo>"
                strSend = strSend & "<LerrynCustCode>" & m_LerrynCustomerCode & "</LerrynCustCode><TransactionCharging>Yes</TransactionCharging>"
                strSend = strSend & "<TransactionInvoiceDue>" & dteNextInvoiceDue.Year & "-" & Right("00" & dteNextInvoiceDue.Month, 2)
                strSend = strSend & "-" & Right("00" & dteNextInvoiceDue.Day, 2) & "</TransactionInvoiceDue><FreeUsageUntilDate>"
                strSend = strSend & dteEndFreeTrial.Year & "-" & Right("00" & dteEndFreeTrial.Month, 2) & "-" & Right("00" & dteEndFreeTrial.Day, 2)
                strSend = strSend & "</FreeUsageUntilDate></Activation></LerrynISPlugin>"

                ' start of code replaced TJS 02/12/11
                Try
                    WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "CreateActivationCode.ashx")
                    WebSubmit.Method = "POST"
                    WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                    WebSubmit.ContentLength = strSend.Length
                    WebSubmit.Timeout = 30000

                    ' start of code added TJS 25/04/11
                    If bLogActivation Then
                        Try
                            Dim writer As StreamWriter
                            If m_ErrorNotification.LogFilePath <> "" Then ' TJS 10/06/12
                                If m_ErrorNotification.LogFilePath.Substring(m_ErrorNotification.LogFilePath.Length - 1) <> "\" Then ' TJS 10/06/12
                                    m_ErrorNotification.LogFilePath = m_ErrorNotification.LogFilePath & "\" ' TJS 10/06/12
                                End If
                            Else
                                m_ErrorNotification.LogFilePath = System.AppDomain.CurrentDomain.BaseDirectory() ' TJS 10/06/12
                            End If

                            'Start the logging to the file
                            writer = New StreamWriter(m_ErrorNotification.LogFilePath & "GetActivation_" & Format(Now.Date.ToString("yyyyMMdd") & ".txt"), True) ' TJS 10/06/12

                            writer.Write(strSend)
                            writer.Close()

                        Catch Ex As Exception
                            ' ignore errors
                        End Try
                    End If
                    ' end of code added TJS 25/04/11

                    byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                    ' send to LerrynSecure.com (or the URL defined in the Registry)
                    postStream = WebSubmit.GetRequestStream()
                    postStream.Write(byteData, 0, byteData.Length)

                    WebResponse = WebSubmit.GetResponse
                    reader = New StreamReader(WebResponse.GetResponseStream())
                    strReturn = reader.ReadToEnd()

                Catch ex As Exception
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                    If Not postStream Is Nothing Then postStream.Close()
                    If Not WebResponse Is Nothing Then WebResponse.Close()
                    Return False

                End Try
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()
                ' end of code replaced TJS 02/12/11

                ' was Activation Code found ?
                If strReturn.Length > 16 Then
                    If "" & strReturn.Substring(0, 16) = "<ActivationCode>" Then
                        ' yes, check for revised Activation Code
                        XMLResponse = XDocument.Parse("<Activation>" & Trim(strReturn) & "</Activation>") ' TJS 02/12/11

                        m_ValidatingActivation = True
                        strActivationCode = GetElementText(XMLResponse, "Activation/ActivationCode").Replace("-", "")
                        Select Case iLoop
                            Case 1 ' ASPDotNetStorefront
                                ErrorCode = ValidateActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 2 ' Magento
                                ErrorCode = ValidateActivationCode(MAGENTO_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 3 ' Volusion
                                ErrorCode = ValidateActivationCode(VOLUSION_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 4 ' Amazon
                                ErrorCode = ValidateActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 5 ' Channel Advisor
                                ErrorCode = ValidateActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 6 ' Shop.com
                                ErrorCode = ValidateActivationCode(SHOP_DOT_COM_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 7 ' eBay TJS 02/12/11
                                ErrorCode = ValidateActivationCode(EBAY_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 02/12/11

                            Case 8 ' Sears.com TJS 16/01/12
                                ErrorCode = ValidateActivationCode(SEARS_DOT_COM_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS16/01/12

                            Case 9 ' Amazon FBA TJS 05/07/12
                                ErrorCode = ValidateActivationCode(AMAZON_FBA_CONNECTOR_CODE, strActivationCode, SystemHashCode) ' TJS 05/07/12

                                ' start of code added TJS 20/08/13
                            Case 10 ' File Import
                                ErrorCode = ValidateActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE, strActivationCode, SystemHashCode)

                            Case 11 ' Prospect and Lead
                                ErrorCode = ValidateActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE, strActivationCode, SystemHashCode)
                                ' end of code added TJS 20/08/13

                                ' start of code added TJS 20/11/13
                            Case 12 ' 3DCart
                                ErrorCode = ValidateActivationCode(THREE_D_CART_CONNECTOR_CODE, strActivationCode, SystemHashCode)
                                ' end of code added TJS 20/11/13

                        End Select
                        If ErrorCode = ErrorCodes.NoError Then
                            ErrorCode = AlphaToLong(strActivationCode.Substring(0, 1), iOffset)
                            If ErrorCode = ErrorCodes.NoError Then
                                strLicenceCode = strActivationCode.Substring(0, 1) & GetLicenceChars(strActivationCode.Substring(1), iOffset)
                                If bUpdateConnector Then
                                    Select Case iLoop
                                        Case 1 ' ASPDotNetStorefront
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE))

                                        Case 2 ' Magento
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE))

                                        Case 3 ' Volusion
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE))

                                        Case 4 ' Amazon
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE))

                                        Case 5 ' Channel Advisor
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE))

                                        Case 6 ' Shop.com
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE))

                                        Case 7 ' eBay TJS 02/12/11
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE)) ' TJS 02/12/11

                                        Case 8 ' Sears.com TJS 16/01/12
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE)) ' TJS 16/01/12

                                        Case 9 ' Amazon FBA TJS 05/07/12
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE)) ' TJS 05/07/12

                                            ' start of code added TJS 20/08/13
                                        Case 10 ' File Import
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE))

                                        Case 11 ' Prospect and Lead
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE))
                                            ' end of code added TJS 20/08/13

                                            ' start of code added TJS 20/11/13
                                        Case 12 ' 3DCart
                                            rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.FindByLicenceCode_DEV000221(ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE))
                                            ' end of code added TJS 20/11/13
                                    End Select
                                Else
                                    rowLicence = m_ImportExportDataset.LerrynLicences_DEV000221.NewLerrynLicences_DEV000221Row
                                End If
                                rowLicence.CodeExpires_DEV000221 = GetLicenceExpiryDate(strLicenceCode)
                                rowLicence.LicenceCode_DEV000221 = strActivationCode
                                rowLicence.ProductCode_DEV000221 = GetLicenceProductCode(strLicenceCode)
                                If bActivateConnector Then
                                    m_ImportExportDataset.LerrynLicences_DEV000221.AddLerrynLicences_DEV000221Row(rowLicence)
                                End If
                                For iConnectorLoop = 0 To ConnectorStates.Length - 1
                                    If ConnectorStates(iConnectorLoop).ProductCode = rowLicence.ProductCode_DEV000221 Then
                                        ConnectorStates(iConnectorLoop).IsActivated = True
                                        ExtractConnectorActivationDetails(ConnectorStates(iConnectorLoop), strActivationCode, True)
                                        UpdateConnectorMaxAccounts(rowLicence.ProductCode_DEV000221, False, bConfigUpdated) ' TJS 02/12/11
                                        Exit For
                                    End If
                                Next
                            Else
                                Return False
                            End If
                        Else
                            Return False
                        End If
                        m_ValidatingActivation = False

                    Else
                        ErrorCode = ErrorCodes.CannotRetrieveLicenceInvalidResponse
                        Return False
                    End If
                Else
                    ErrorCode = ErrorCodes.CannotRetrieveLicenceNoResponse
                    Return False
                End If
            End If
        Next

        ' card registration code removed TJS 18/01/13
        Return True

    End Function
#End Region

#Region " CancelActivation "
    Public Function CancelActivation(ByVal ImportedInventoryCount As Integer) As Boolean ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2,
        '                                        | to add count of Imported Inventory Items and to add eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strSend As String, strSystemLicenceID As String, strErrorDetails As String
        Dim strReturn As String ' TJS 02/12/11

        strSystemLicenceID = BuildSystemRegID(SystemID, CacheID)

        strSend = "<LerrynCancelActivation><Login><Source>ISIntegrated</Source><Password>K$0eW*zU2B</Password></Login>"
        strSend = strSend & "<LerrynCustCode>" & m_LerrynCustomerCode & "</LerrynCustCode><SystemID>" & strSystemLicenceID
        strSend = strSend & "</SystemID><SysIDType>ISSysID</SysIDType><ImportedInventoryCount>" & ImportedInventoryCount ' TJS 02/12/11
        strSend = strSend & "</ImportedInventoryCount><Activation><ProductCode>" & m_BaseProductCode ' TJS 02/12/11
        strSend = strSend & "</ProductCode><ActivationCode>" & m_LatestActivationCode & "</ActivationCode></Activation>"
        If IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
            strSend = strSend & "<Activation><ProductCode>" & ASP_STORE_FRONT_CONNECTOR_CODE & "</ProductCode><ActivationCode>"
            strSend = strSend & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE) & "</ActivationCode></Activation>"
        End If
        If IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
            strSend = strSend & "<Activation><ProductCode>" & MAGENTO_CONNECTOR_CODE & "</ProductCode><ActivationCode>"
            strSend = strSend & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE) & "</ActivationCode></Activation>"
        End If
        If IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
            strSend = strSend & "<Activation><ProductCode>" & VOLUSION_CONNECTOR_CODE & "</ProductCode><ActivationCode>"
            strSend = strSend & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE) & "</ActivationCode></Activation>"
        End If
        If IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
            strSend = strSend & "<Activation><ProductCode>" & AMAZON_SELLER_CENTRAL_CONNECTOR_CODE & "</ProductCode><ActivationCode>"
            strSend = strSend & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) & "</ActivationCode></Activation>"
        End If
        If IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
            strSend = strSend & "<Activation><ProductCode>" & CHANNEL_ADVISOR_CONNECTOR_CODE & "</ProductCode><ActivationCode>"
            strSend = strSend & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE) & "</ActivationCode></Activation>"
        End If
        If IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
            strSend = strSend & "<Activation><ProductCode>" & EBAY_CONNECTOR_CODE & "</ProductCode><ActivationCode>" ' TJS 02/12/11
            strSend = strSend & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE) & "</ActivationCode></Activation>" ' TJS 02/12/11
        End If
        If IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
            strSend = strSend & "<Activation><ProductCode>" & SEARS_DOT_COM_CONNECTOR_CODE & "</ProductCode><ActivationCode>" ' TJS 16/01/12
            strSend = strSend & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE) & "</ActivationCode></Activation>" ' TJS 02/12/11
        End If
        If IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
            strSend = strSend & "<Activation><ProductCode>" & SHOP_DOT_COM_CONNECTOR_CODE & "</ProductCode><ActivationCode>"
            strSend = strSend & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE) & "</ActivationCode></Activation>"
        End If
        ' start of code added TJS 05/07/12
        If IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
            strSend = strSend & "<Activation><ProductCode>" & AMAZON_FBA_CONNECTOR_CODE & "</ProductCode><ActivationCode>"
            strSend = strSend & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE) & "</ActivationCode></Activation>"
        End If
        ' end of code added TJS 05/07/12
        strSend = strSend & "</LerrynCancelActivation>"

        ' start of code replaced TJS 02/12/11
        Try
            WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "CancelRequested.ashx")
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
            WebSubmit.ContentLength = strSend.Length
            WebSubmit.Timeout = 30000

            byteData = UTF8Encoding.UTF8.GetBytes(strSend)

            ' send to LerrynSecure.com (or the URL defined in the Registry)
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            strReturn = reader.ReadToEnd()

            If strReturn <> "<LerrynCancelActivation><Status>OK</Status></LerrynCancelActivation>" Then
                XMLResponse = XDocument.Parse(strReturn)
                strErrorDetails = GetElementText(XMLResponse, "LerrynCancelActivation/ErrorDetails")
                Return False
            End If

        Catch ex As Exception
            strErrorDetails = ex.Message
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()
            Return False

        End Try
        If Not postStream Is Nothing Then postStream.Close()
        If Not WebResponse Is Nothing Then WebResponse.Close()
        ' end of code replaced TJS 02/12/11

        Return True

    End Function
#End Region

#Region " GetActivationCost "
    Public Function GetActivationCost(ByVal ExistingConnectors As Boolean) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Gets the current prices for activating the product (not for eCommerce Plus)
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, to add eBay conenctor 
        '                                        | and corrected sending of connector previous activation code
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 14/06/12 | TJS             | 2012.1.06 | Modified to filter set allowed activation currency
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings and 50000 Inventory Import qty option
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to set connected Business pricing flag
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to cater for File Import and Prospect/Lead connectors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader ' TJS 02/12/11
        Dim strSend As String, iLoop As Integer

        strSend = "<LerrynGetQuote><Login><Source>ISIntegrated</Source><Password>K$0eW*zU2B</Password>"
        strSend = strSend & "</Login><SystemID>" & BuildSystemRegID(SystemID, CacheID) & "</SystemID>"
        strSend = strSend & "<SysIDType>ISSysID</SysIDType><LerrynCustCode>" & m_LerrynCustomerCode
        strSend = strSend & "</LerrynCustCode><ProductCode>" & m_BaseProductCode & "</ProductCode>"
        strSend = strSend & "<PreviousActivation>" & m_LatestActivationCode & "</PreviousActivation>"
        strSend = strSend & "<CompanyName>" & m_ImportExportDataset.SystemCompanyInformation(0).CompanyName & "</CompanyName>" ' TJS 09/08/13
        strSend = strSend & "<TurnoverBasedPricing>Yes</TurnoverBasedPricing>" ' TJS 09/08/13
        strSend = strSend & "<Currency>" & GetActivationCurrency(m_ImportExportDataset.SystemCompanyInformation(0).CurrencyCode) ' TJS 14/06/12
        If Not Me.m_ImportExportDataset.ActivationAccountDetails(0).ImportWizardOnly Then
            strSend = strSend & "</Currency><UserCount>-1</UserCount><SecondaryCount>625</SecondaryCount>"
            For iLoop = 1 To 12 ' TJS 02/12/11 TJS 16/01/12 TJS 05/07/12 TJS 20/08/13 TJS 20/13/13
                Select Case iLoop
                    Case 1 ' ASPDotNetStorefront
                        strSend = strSend & "<Connector><ProductCode>" & ASP_STORE_FRONT_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ASP_STORE_FRONT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(ASP_STORE_FRONT_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ASPStorefrontCount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 2 ' Magento
                        strSend = strSend & "<Connector><ProductCode>" & MAGENTO_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(MAGENTO_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(MAGENTO_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).MagentoCount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 3 ' Volusion
                        strSend = strSend & "<Connector><ProductCode>" & VOLUSION_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(VOLUSION_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(VOLUSION_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(VOLUSION_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).VolusionCount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 4 ' Amazon
                        strSend = strSend & "<Connector><ProductCode>" & AMAZON_SELLER_CENTRAL_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonCount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 5 ' Channel Advisor
                        strSend = strSend & "<Connector><ProductCode>" & CHANNEL_ADVISOR_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(CHANNEL_ADVISOR_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(CHANNEL_ADVISOR_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ChanAdvCount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 6 ' Shop.com
                        strSend = strSend & "<Connector><ProductCode>" & SHOP_DOT_COM_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SHOP_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(SHOP_DOT_COM_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ShopComCount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 7 ' eBay TJS 02/12/11
                        strSend = strSend & "<Connector><ProductCode>" & EBAY_CONNECTOR_CODE & "</ProductCode>" ' TJS 02/12/11
                        If IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 02/12/11
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(0, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(5, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(10, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(15, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(20, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(25, 5) ' TJS 02/12/11
                            strSend = strSend & "-" & ConnectorLatestActivationCode(EBAY_CONNECTOR_CODE).Substring(30, 5) ' TJS 02/12/11
                            strSend = strSend & "</PreviousActivation>" ' TJS 02/12/11
                        End If
                        If ExistingConnectors Then ' TJS 02/12/11
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(EBAY_CONNECTOR_CODE) ' TJS 02/12/11
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).EBayCount ' TJS 02/12/11
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>" ' TJS 02/12/11

                    Case 8 ' Sears.com TJS 16/01/12
                        strSend = strSend & "<Connector><ProductCode>" & SEARS_DOT_COM_CONNECTOR_CODE & "</ProductCode>" ' TJS 16/01/12
                        If IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE) Then ' TJS 16/01/12
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(0, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(5, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(10, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(15, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(20, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(25, 5) ' TJS 16/01/12
                            strSend = strSend & "-" & ConnectorLatestActivationCode(SEARS_DOT_COM_CONNECTOR_CODE).Substring(30, 5) ' TJS 16/01/12
                            strSend = strSend & "</PreviousActivation>" ' TJS 16/01/12
                        End If
                        If ExistingConnectors Then ' TJS 16/01/12
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(SEARS_DOT_COM_CONNECTOR_CODE) ' TJS 16/01/12
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).SearsComCount ' TJS 16/01/12
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>" ' TJS 16/01/12

                        ' start of code added TJS 05/07/12
                    Case 9 ' Amazon FBA
                        strSend = strSend & "<Connector><ProductCode>" & AMAZON_FBA_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(AMAZON_FBA_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(AMAZON_FBA_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(AMAZON_FBA_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).AmazonFBACount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"
                        ' end of code added TJS 05/07/12

                        ' start of code added TJS 20/08/13
                    Case 10 ' File Import
                        strSend = strSend & "<Connector><ProductCode>" & ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(ESHOPCONNECT_FILE_IMPORT_CONNECTOR_CODE)
                        ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateFileImport Then
                            strSend = strSend & "<ConnectionCount>1"
                        Else
                            strSend = strSend & "<ConnectionCount>0"
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"

                    Case 11 ' Prospect and Lead
                        strSend = strSend & "<Connector><ProductCode>" & PROSPECT_IMPORT_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(PROSPECT_IMPORT_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(PROSPECT_IMPORT_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(PROSPECT_IMPORT_CONNECTOR_CODE)
                        ElseIf Me.m_ImportExportDataset.ActivationAccountDetails(0).ActivateProspectLead Then
                            strSend = strSend & "<ConnectionCount>1"
                        Else
                            strSend = strSend & "<ConnectionCount>0"
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"
                        ' end of code added TJS 20/08/13

                        ' start of code added TJS 20/11/13
                    Case 12 ' 3DCart
                        strSend = strSend & "<Connector><ProductCode>" & THREE_D_CART_CONNECTOR_CODE & "</ProductCode>"
                        If IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                            strSend = strSend & "<PreviousActivation>" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(0, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(5, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(10, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(15, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(20, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(25, 5)
                            strSend = strSend & "-" & ConnectorLatestActivationCode(THREE_D_CART_CONNECTOR_CODE).Substring(30, 5)
                            strSend = strSend & "</PreviousActivation>"
                        End If
                        If ExistingConnectors Then
                            strSend = strSend & "<ConnectionCount>" & ConnectorAccountLimit(THREE_D_CART_CONNECTOR_CODE)
                        Else
                            strSend = strSend & "<ConnectionCount>" & Me.m_ImportExportDataset.ActivationAccountDetails(0).ThreeDCartCount
                        End If
                        strSend = strSend & "</ConnectionCount></Connector>"
                        ' end of code added TJS 20/11/13
                End Select
            Next

        Else
            Select Case Me.m_ImportExportDataset.ActivationAccountDetails(0).ImportWizardQty
                Case Is <= 2500
                    strSend = strSend & "</Currency><UserCount>-1</UserCount><SecondaryCount>2</SecondaryCount>"
                Case 10000
                    strSend = strSend & "</Currency><UserCount>-1</UserCount><SecondaryCount>3</SecondaryCount>"
                Case 25000
                    strSend = strSend & "</Currency><UserCount>-1</UserCount><SecondaryCount>4</SecondaryCount>"
                Case 50000 ' TJS 05/07/12
                    strSend = strSend & "</Currency><UserCount>-1</UserCount><SecondaryCount>5</SecondaryCount>" ' TJS 05/07/12

            End Select
            strSend = strSend & "<PriceWithoutConnectors>Yes</PriceWithoutConnectors>"
        End If
        strSend = strSend & "</LerrynGetQuote>"

        ' start of code replaced TJS 02/12/11
        Try
            WebSubmit = System.Net.WebRequest.Create(GetISPluginBaseURL() & "GetQuote.ashx")
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
            WebSubmit.ContentLength = strSend.Length
            WebSubmit.Timeout = 30000

            byteData = UTF8Encoding.UTF8.GetBytes(strSend)

            ' send to LerrynSecure.com (or the URL defined in the Registry)
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            GetActivationCost = reader.ReadToEnd()

        Catch ex As Exception
            GetActivationCost = ""

        Finally
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()

        End Try
        ' end of code replaced TJS 02/12/11

        ' need to ensure XML functions return data for Activation wizard
        m_ValidatingActivation = True ' TJS 02/12/11

    End Function
#End Region

#Region " GetActivationCurrency "
    Private Function GetActivationCurrency(ByVal CurrencyCode As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Returns the currency code to be used for system activation
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/06/12 | TJS             | 2012.1.06 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If CurrencyCode = "EURO" Or CurrencyCode = "GBP" Then
            Return CurrencyCode

        Else
            Return "USD"
        End If

    End Function
#End Region

#Region " ExtractActivationDetails "
    Private Sub ExtractActivationDetails(ByVal ActivationCode As String, ByVal AlwaysExtract As Boolean) ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to compare expiry dates when extracting details
        ' 03/04/09 | TJS             | 2009.2.00 | Added code to extract whether Activation is Eval or Full
        ' 18/02/10 | TJS             | 2010.0.05 | Corrected setting of m_LatestActivationCode to save 
        '                                        | the Activation Code not the Licence Code
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to extract activation if secondary count is greater 
        '                                        | as well as expiry date being later and remove connector limit 
        ' 17/02/12 | TJS             | 2010.2.07 | Modified to save previously activated max items if activation has expired
        ' 02/04/12 | TJS             | 2011.2.12 | Modified to only extract when secondary count is greater if activation not expired
        ' 14/08/12 | TJS             | 2012.1.13 | Modified to initialise Error Notification with Activation code
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iErrorCode As Integer, iOffset As Integer, strLicenceCode As String
        Dim dtrTemp As Date, bExtractDetails As Boolean ' TJS 17/03/09

        iErrorCode = AlphaToLong(ActivationCode.Substring(0, 1), iOffset)
        If iErrorCode = ErrorCodes.NoError Then
            bExtractDetails = False ' TJS 17/03/09
            strLicenceCode = ActivationCode.Substring(0, 1) & GetLicenceChars(ActivationCode.Substring(1), iOffset)
            dtrTemp = GetLicenceExpiryDate(strLicenceCode) ' TJS 17/03/09
            ' has anything been extracted yet ?
            If m_LatestActivationCode <> "" Then ' TJS 17/03/09
                ' yes, is expiry date later than existing code
                If dtrTemp > m_ExpiryDate Or (GetLicenceSecondaryCount(strLicenceCode) > m_MaxAccounts And dtrTemp >= Date.Today) Or AlwaysExtract Then ' TJS 17/03/09 TJS 02/12/11
                    ' yes, extract details
                    bExtractDetails = True ' TJS 17/03/09
                End If
            Else
                bExtractDetails = True ' TJS 17/03/09
            End If
            If bExtractDetails Then ' TJS 17/03/09
                If dtrTemp >= Date.Today Then
                    If GetLicenceSecondaryCount(strLicenceCode) = 0 Then
                        m_MaxAccounts = 1
                    Else
                        m_MaxAccounts = GetLicenceSecondaryCount(strLicenceCode)
                    End If
                Else
                    m_MaxAccounts = 0
                    m_LastActivatedMaxAccounts = GetLicenceSecondaryCount(strLicenceCode) ' TJS 17/02/12
                End If
                m_ExpiryDate = dtrTemp
                m_LatestActivationCode = ActivationCode ' TJS 17/03/09 TJS 18/02/10
                m_ErrorNotification.ActivationCode = m_LatestActivationCode ' TJS 14/08/12
                m_IsFullActivation = GetLicenceIsFull(strLicenceCode) ' TJS 03/04/09
            End If
        End If

    End Sub
#End Region

#Region " ExtractConnectorActivationDetails "
    Private Sub ExtractConnectorActivationDetails(ByRef ConnectorToUpdate As Connectors, ByVal ActivationCode As String, _
        ByVal AlwaysExtract As Boolean) ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/03/09 | TJS             | 2009.1.10 | function added
        ' 03/04/09 | TJS             | 2009.2.00 | Added code to extract whether Activation is Eval or Full
        ' 18/02/10 | TJS             | 2010.0.05 | Corrected setting of ConnectorToUpdate.LatestActivationCode
        '                                        | to save the Activation Code not the Licence Code
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to extract activation if secondary count is greater 
        '                                        | as well as expiry date being later and remove connector limit 
        ' 17/02/12 | TJS             | 2010.2.07 | Modified to save previously activated connectors if activation has expired
        ' 29/01/14 | TJS             | 2013.4.07 | Modified to prevent extraction of activations that have 
        '                                        | already expired even if the connector count is greater
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iErrorCode As Integer, iOffset As Integer, strLicenceCode As String
        Dim dtrTemp As Date, bExtractDetails As Boolean

        iErrorCode = AlphaToLong(ActivationCode.Substring(0, 1), iOffset)
        If iErrorCode = ErrorCodes.NoError Then
            bExtractDetails = False ' TJS 17/03/09
            strLicenceCode = ActivationCode.Substring(0, 1) & GetLicenceChars(ActivationCode.Substring(1), iOffset)
            dtrTemp = GetLicenceExpiryDate(strLicenceCode) ' TJS 17/03/09
            ' has anything been extracted yet ?
            If ConnectorToUpdate.LatestActivationCode <> "" Then ' TJS 17/03/09
                ' yes, is expiry date later than existing code or is secondary count greater ?
                If dtrTemp > ConnectorToUpdate.ExpiryDate Or (GetLicenceSecondaryCount(strLicenceCode) > ConnectorToUpdate.MaxAccounts And dtrTemp >= Date.Today) Or AlwaysExtract Then ' TJS 17/03/09 TJS 02/12/11 TJS 29/01/14
                    ' yes, extract details
                    bExtractDetails = True ' TJS 17/03/09
                End If
            Else
                bExtractDetails = True ' TJS 17/03/09
            End If
            If bExtractDetails Then ' TJS 17/03/09
                If dtrTemp >= Date.Today Then
                    If GetLicenceSecondaryCount(strLicenceCode) = 0 Then
                        ConnectorToUpdate.MaxAccounts = 1
                    Else
                        ConnectorToUpdate.MaxAccounts = GetLicenceSecondaryCount(strLicenceCode)
                    End If
                Else
                    ConnectorToUpdate.MaxAccounts = 0
                    ConnectorToUpdate.LastActivatedMaxAccounts = GetLicenceSecondaryCount(strLicenceCode) ' TJS 17/02/12
                End If
                ConnectorToUpdate.ExpiryDate = dtrTemp
                ConnectorToUpdate.LatestActivationCode = ActivationCode ' TJS 17/03/09 TJS 18/02/10
                ConnectorToUpdate.IsFullActivation = GetLicenceIsFull(strLicenceCode) ' TJS 03/04/09
            End If
        End If

    End Sub
#End Region

#Region " GetDefaultConfigSettings "
    Private Sub GetDefaultConfigSettings()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/03/09 | TJS             | 2009.1.09 | Modified to only do anything when correctly activated
        ' 08/07/09 | TJS             | 2009.3.00 | Modified for Prospect Importer
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day of expiry
        ' 10/06/12 | TJS             | 2012.1.05 | modified to use Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument
        Dim iConfigLoop As Integer, sBaseHandler As String, sTemp As String

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 11/03/09 TJS 13/04/10 TJS 17/02/12
            If m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE Or m_BaseProductCode = PROSPECTIMPORTER_BASE_PRODUCT_CODE Then ' TJS 08/07/09 
                sBaseHandler = "GenericXMLImport.ashx"
            Else
                sBaseHandler = "Windows Service"
            End If
            For iConfigLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count - 1
                If Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iConfigLoop).InputHandler_DEV000221 = sBaseHandler Then
                    XMLConfig = XDocument.Parse(Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iConfigLoop).ConfigSettings_DEV000221) ' TJS 02/12/11
                    sTemp = GetElementText(XMLConfig, SOURCE_CONFIG_ENABLE_LOG_FILE)
                    If sTemp.ToUpper = "YES" Then
                        sTemp = GetElementText(XMLConfig, SOURCE_CONFIG_LOG_FILE_PATH)
                        If sTemp <> "" Then
                            m_ErrorNotification.LogFilePath = sTemp ' TJS 10/06/12
                            m_ErrorNotification.EnableLogFile = True ' TJS 10/06/12
                        End If
                    End If
                    sTemp = GetElementText(XMLConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS) ' TJS 10/06/12
                    m_ErrorNotification.BaseErrorEmailAddress = sTemp ' TJS 10/06/12
                    m_ErrorNotification.BaseSendCodeErrorEmailsToLerryn = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN)) = "YES", True, False)) ' TJS 10/06/12
                    m_ErrorNotification.BaseSendSourceErrorEmailsToLerryn = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_SEND_SOURCE_ERROR_EMAILS_TO_LERRYN)) = "YES", True, False)) ' TJS 10/06/12
                End If
            Next
        End If
    End Sub
#End Region

#Region " GetAmazonBrowseListItems "
    Public Function GetAmazonBrowseListItems(ByVal ParentCategory As String, ByVal SiteCode As String, ByVal LastModified As Date) As XDocument
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.06 | Function added
        ' 14/05/09 | TJS             | 2009.2.07 | Corrected error response
        ' 25/05/09 | TJS             | 2009.2.08 | Modified to prevent ParentNodeID characters from invalidating XML
        ' 08/07/09 | TJS             | 2009.6.09 | Amended timeouts
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use CheckRegistryValue
        ' 22/10/09 | TJS             | 2009.3.09 | Extended receive timeout
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to change ConvertForURL to ConvertEntitiesForXML and urlencoded POST data
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ' 19/04/12 | TJS             | 2012.1.01 | Modified to set CustomerTypeCode and ensure spaces in XML are handled properly
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strSend As String, strReturn As String, strBrowseListURL As String

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12

            strBrowseListURL = CheckRegistryValue(REGISTRY_KEY_ROOT, "BrowseURL", BROWSE_LIST_URL) ' TJS 23/08/09

            strSend = "<eShopCONNECT><SiteCode>" & SiteCode & "</SiteCode><ParentCategory>" & ConvertEntitiesForXML(ParentCategory) & "</ParentCategory>" ' TJS 25/05/09 TJS 18/03/11
            strSend = strSend & "<LastModified>" & LastModified.Year & "-" & Right("00" & LastModified.Month, 2) & "-"
            strSend = strSend & Right("00" & LastModified.Day, 2) & " " & Right("00" & LastModified.Hour, 2) & ":"
            strSend = strSend & Right("00" & LastModified.Minute, 2) & ":" & Right("00" & LastModified.Second, 2)
            strSend = strSend & "</LastModified></eShopCONNECT>"
            strSend = strSend.Replace(" ", "%20") ' TJS 19/04/12

            ' start of code replaced TJS 02/12/11
            Try
                WebSubmit = System.Net.WebRequest.Create(strBrowseListURL & "GetAmazonBrowseItems.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = HttpUtility.UrlEncode(strSend).Length
                WebSubmit.Timeout = 30000

                byteData = UTF8Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(strSend))

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()

                If strReturn.Substring(0, 14) <> "<eShopCONNECT>" Then
                    strReturn = "<eShopCONNECT><Error>Cannot retrieve Amazon Browse Node Items</Error></eShopCONNECT>" ' TJS 14/05/09
                End If

            Catch ex As Exception
                strReturn = "<eShopCONNECT><Error>Cannot retrieve Amazon Browse Node Items</Error></eShopCONNECT>" ' TJS 14/05/09

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try
            ' end of code replaced TJS 02/12/11
        Else
            strReturn = "<eShopCONNECT><Error>Not activated</Error></eShopCONNECT>"
        End If

        XMLResponse = XDocument.Parse(Trim(strReturn)) ' TJS 02/12/11

        Return XMLResponse

    End Function
#End Region

#Region " GetAmazonTagTemplates "
    Public Function GetAmazonTagTemplates(ByVal ParentCategory As String, ByVal SiteCode As String, ByVal LastModified As Date) As XDocument
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.06 | Function added
        ' 14/05/09 | TJS             | 2009.2.07 | Corrected error response
        ' 25/05/09 | TJS             | 2009.2.08 | Modified to prevent ParentCategory characters from invalidating XML
        ' 08/07/09 | TJS             | 2009.6.09 | Amended timeouts
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use CheckRegistryValue
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to change ConvertForURL to ConvertEntitiesForXML and urlencoded POST data
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strSend As String, strReturn As String, strBrowseListURL As String

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12

            strBrowseListURL = CheckRegistryValue(REGISTRY_KEY_ROOT, "BrowseURL", BROWSE_LIST_URL) ' TJS 23/08/09

            strSend = "<eShopCONNECT><SiteCode>" & SiteCode & "</SiteCode><ParentCategory>" & ConvertEntitiesForXML(ParentCategory) & "</ParentCategory>" ' TJS 25/05/09 TJS 18/03/11
            strSend = strSend & "<LastModified>" & LastModified.Year & "-" & Right("00" & LastModified.Month, 2) & "-"
            strSend = strSend & Right("00" & LastModified.Day, 2) & " " & Right("00" & LastModified.Hour, 2) & ":"
            strSend = strSend & Right("00" & LastModified.Minute, 2) & ":" & Right("00" & LastModified.Second, 2)
            strSend = strSend & "</LastModified></eShopCONNECT>"

            ' start of code replaced TJS 02/12/11
            Try
                WebSubmit = System.Net.WebRequest.Create(strBrowseListURL & "GetAmazonTagTemplate.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = strSend.Length
                WebSubmit.Timeout = 30000

                byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()
                If strReturn.Substring(0, 14) <> "<eShopCONNECT>" Then
                    strReturn = "<eShopCONNECT><Error>Cannot retrieve Amazon Tag Template Items</Error></eShopCONNECT>" ' TJS 14/05/09
                End If

            Catch ex As Exception
                strReturn = "<eShopCONNECT><Error>Cannot retrieve Amazon Tag Template Items</Error></eShopCONNECT>" ' TJS 14/05/09

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try
            ' end of code replaced TJS 02/12/11
        Else
            strReturn = "<eShopCONNECT><Error>Not activated</Error></eShopCONNECT>"
        End If

        XMLResponse = XDocument.Parse(Trim(strReturn)) ' TJS 02/12/11

        Return XMLResponse

    End Function
#End Region

#Region " GetAmazonXMLTypeDetails "
    Public Function GetAmazonXMLTypeDetails(ByVal LastModified As Date) As XDocument
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 22/10/09 | TJS             | 2009.3.09 | Function added
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strSend As String, strReturn As String, strSourceURL As String

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12

            strSourceURL = CheckRegistryValue(REGISTRY_KEY_ROOT, "BrowseURL", BROWSE_LIST_URL)

            strSend = "<eShopCONNECT><LastModified>" & LastModified.Year & "-" & Right("00" & LastModified.Month, 2) & "-"
            strSend = strSend & Right("00" & LastModified.Day, 2) & " " & Right("00" & LastModified.Hour, 2) & ":"
            strSend = strSend & Right("00" & LastModified.Minute, 2) & ":" & Right("00" & LastModified.Second, 2)
            strSend = strSend & "</LastModified></eShopCONNECT>"

            ' start of code replaced TJS 02/12/11
            Try
                WebSubmit = System.Net.WebRequest.Create(strSourceURL & "GetAmazonXMLTypeDetails.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = strSend.Length
                WebSubmit.Timeout = 30000

                byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()
                If strReturn.Substring(0, 14) <> "<eShopCONNECT>" Then
                    strReturn = "<eShopCONNECT><Error>Cannot retrieve Amazon XML Type Details</Error></eShopCONNECT>"
                End If

            Catch ex As Exception
                strReturn = "<eShopCONNECT><Error>Cannot retrieve Amazon XML Type Details</Error></eShopCONNECT>"

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try
            ' end of code replaced TJS 02/12/11
        Else
            strReturn = "<eShopCONNECT><Error>Not activated</Error></eShopCONNECT>"
        End If

        XMLResponse = XDocument.Parse(Trim(strReturn)) ' TJS 02/12/11

        Return XMLResponse

    End Function
#End Region

#Region " GetShopComAttributeCategories "
    Public Function GetShopComAttributeCategories(ByVal ParentCategory As String, ByVal LastModified As Date) As XDocument
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/05/09 | TJS             | 2009.2.07 | Function added
        ' 25/05/09 | TJS             | 2009.2.08 | Modified to prevent ParentCategory characters from invalidating XML
        ' 08/07/09 | TJS             | 2009.6.09 | Amended timeouts
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use CheckRegistryValue
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to change ConvertForURL to ConvertEntitiesForXML and urlencoded POST data
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strSend As String, strReturn As String, strBrowseListURL As String

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12

            strBrowseListURL = CheckRegistryValue(REGISTRY_KEY_ROOT, "BrowseURL", BROWSE_LIST_URL) ' TJS 23/08/09

            strSend = "<eShopCONNECT><ParentCategory>" & ConvertEntitiesForXML(ParentCategory) & "</ParentCategory><LastModified>" ' TJS 25/05/09 TJS 18/03/11
            strSend = strSend & LastModified.Year & "-" & Right("00" & LastModified.Month, 2) & "-"
            strSend = strSend & Right("00" & LastModified.Day, 2) & " " & Right("00" & LastModified.Hour, 2) & ":"
            strSend = strSend & Right("00" & LastModified.Minute, 2) & ":" & Right("00" & LastModified.Second, 2)
            strSend = strSend & "</LastModified></eShopCONNECT>"

            ' start of code replaced TJS 02/12/11
            Try
                WebSubmit = System.Net.WebRequest.Create(strBrowseListURL & "GetShopComAttributeCategories.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = strSend.Length
                WebSubmit.Timeout = 30000

                byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()
                If strReturn.Substring(0, 14) <> "<eShopCONNECT>" Then
                    strReturn = "<eShopCONNECT><Error>Cannot retrieve Shop.com Attribute Categories</Error></eShopCONNECT>"
                End If

            Catch ex As Exception
                strReturn = "<eShopCONNECT><Error>Cannot retrieve Shop.com Attribute Categories</Error></eShopCONNECT>"

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try
            ' end of code replaced TJS 02/12/11
        Else
            strReturn = "<eShopCONNECT><Error>Not activated</Error></eShopCONNECT>"
        End If

        XMLResponse = XDocument.Parse(Trim(strReturn)) ' TJS 02/12/11

        Return XMLResponse

    End Function
#End Region

#Region " GetShopComTagTemplates "
    Public Function GetShopComTagTemplates(ByVal ParentCategory As String, ByVal LastModified As Date) As XDocument
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/06/09 | TJS             | 2009.2.10 | Function added
        ' 08/07/09 | TJS             | 2009.6.09 | Amended timeouts
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use CheckRegistryValue
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to change ConvertForURL to ConvertEntitiesForXML and urlencoded POST data
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument ' TJS 02/12/11
        Dim strSend As String, strReturn As String, strBrowseListURL As String

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12

            strBrowseListURL = CheckRegistryValue(REGISTRY_KEY_ROOT, "BrowseURL", BROWSE_LIST_URL) ' TJS 23/08/09

            strSend = "<eShopCONNECT><ParentCategory>" & ConvertEntitiesForXML(ParentCategory) & "</ParentCategory>" ' TJS 18/03/11
            strSend = strSend & "<LastModified>" & LastModified.Year & "-" & Right("00" & LastModified.Month, 2) & "-"
            strSend = strSend & Right("00" & LastModified.Day, 2) & " " & Right("00" & LastModified.Hour, 2) & ":"
            strSend = strSend & Right("00" & LastModified.Minute, 2) & ":" & Right("00" & LastModified.Second, 2)
            strSend = strSend & "</LastModified></eShopCONNECT>"

            ' start of code replaced TJS 02/12/11
            Try
                WebSubmit = System.Net.WebRequest.Create(strBrowseListURL & "GetShopComTagTemplate.ashx")
                WebSubmit.Method = "POST"
                WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                WebSubmit.ContentLength = strSend.Length
                WebSubmit.Timeout = 30000

                byteData = UTF8Encoding.UTF8.GetBytes(strSend)

                ' send to LerrynSecure.com (or the URL defined in the Registry)
                postStream = WebSubmit.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

                WebResponse = WebSubmit.GetResponse
                reader = New StreamReader(WebResponse.GetResponseStream())
                strReturn = reader.ReadToEnd()
                If strReturn.Substring(0, 14) <> "<eShopCONNECT>" Then
                    strReturn = "<eShopCONNECT><Error>Cannot retrieve Shop.com Tag Template Items</Error></eShopCONNECT>"
                End If

            Catch ex As Exception
                strReturn = "<eShopCONNECT><Error>Cannot retrieve Shop.com Tag Template Items</Error></eShopCONNECT>"

            Finally
                If Not postStream Is Nothing Then postStream.Close()
                If Not WebResponse Is Nothing Then WebResponse.Close()

            End Try
            ' end of code replaced TJS 02/12/11
        Else
            strReturn = "<eShopCONNECT><Error>Not activated</Error></eShopCONNECT>"
        End If

        XMLResponse = XDocument.Parse(Trim(strReturn)) ' TJS 02/12/11

        Return XMLResponse

    End Function
#End Region

#Region " NewSource "
    Public Sub NewSource()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS             | 2008.0.01 | Original 
        ' 28/01/09 | TJs             | 2009.1.01 | Modified to cater for eShopCONNECT and OrderImporter
        ' 16/02/09 | TJS             | 2009.1.08 | Modified to cater for different core config settings 
        '                                        | between eShopCONNECT and Order importer
        ' 26/08/09 | TJS             | 2009.3.05 | modified to cater for EnableSourcePassword_DEV000221 on config settings
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row
        Dim rowSource As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemSourceRow

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12
            With m_ImportExportDataset
                '
                'Clear the current content.
                .LerrynImportExportConfig_DEV000221.Clear()
                .SystemSource.Clear()

                'Get the reference of new  record.
                rowConfig = .LerrynImportExportConfig_DEV000221.NewLerrynImportExportConfig_DEV000221Row()
                rowSource = .SystemSource.NewSystemSourceRow()

                'Initialize default values 
                rowConfig.SourceCode_DEV000221 = Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE
                rowConfig.SourceName_DEV000221 = ""
                rowConfig.InputMode_DEV000221 = "HTTPPost"
                If m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE Then
                    rowConfig.InputHandler_DEV000221 = "GenericXMLImport.ashx"
                    rowConfig.ConfigSettings_DEV000221 = CORE_ESHOPCONNECT_CONFIG_SETTINGS ' TJS 16/02/09

                ElseIf m_BaseProductCode = PROSPECTIMPORTER_BASE_PRODUCT_CODE Then ' TJS 08/07/09
                    rowConfig.InputHandler_DEV000221 = "GenericXMLImport.ashx" ' TJS 08/07/09
                    rowConfig.ConfigSettings_DEV000221 = CORE_PROSPECT_IMPORTER_CONFIG_SETTINGS ' TJS 08/07/09

                Else
                    rowConfig.InputHandler_DEV000221 = "Windows Service" ' TJE 26/01/09
                    rowConfig.ConfigSettings_DEV000221 = CORE_ORDER_IMPORTER_CONFIG_SETTINGS ' TJS 16/02/09
                End If
                rowConfig.EnableSourcePassword_DEV000221 = True ' TJS 26/08/09
                rowSource.SourceCode = rowConfig.SourceCode_DEV000221
                rowSource.SourceDescription = rowConfig.SourceName_DEV000221
                rowSource.IsActive = True

                ' now add row to dataset
                .LerrynImportExportConfig_DEV000221.AddLerrynImportExportConfig_DEV000221Row(rowConfig)
                .SystemSource.AddSystemSourceRow(rowSource)

            End With
        Else
            ActivationFailure(1, "", False)
        End If
    End Sub
#End Region

#Region " UpdateConnectorMaxAccounts "
    Public Function UpdateConnectorMaxAccounts(ByVal ConnectorProductCode As String, ByVal UpdateConfigSettingsDataset As Boolean, ByRef ConfigUpdated As Boolean) As Boolean ' TJS 18/03/11 TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/03/09 | TJS             | 2009.1.09 | Modified to only do anything when correctly activated
        ' 14/08/09 | TJS             | 2009.3.03 | Modified to prevent error if connector AccountCoreConfigSettings is blank
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 25/04/10 | TJS             | 2010.0.07 | Corrected extract of Account Group name
        ' 19/08/10 | TJS             | 2010.0.10 | Modified to update config XML in dataset after adding or removing 
        '                                        | account config entries to prevent repeat adding of config groups
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to allow use when XMLConfigSettings not loaded
        ' 18/04/11 | TJS             | 2011.0.11 | Modified to check for active Website codes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and 
        '                                        | modified to pass Source BusinessType to UpdateWebsiteCodes
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ' 02/10/13 | TJS             | 2013.3.03 | Modified to save Magento Version in config
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLAccountConfig As XDocument
        Dim XMLConfigNodeList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLAccountNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLConfigNode As XElement
        Dim rowConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row ' TJS 18/03/11
        Dim rowXMLConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim iLoop As Integer, iAccountLoop As Integer, iConfigRowLoop As Integer, iAccountCount As Integer
        Dim strGroupName As String, strConfig As String, strTemp As String, strSourceBusinessType As String ' TJS 25/05/10 TJS 02/12/11
        Dim strGroupNameToDelete As String, iStartPosn As Integer, iEndPosn As Integer, iSlashPosn As Integer ' TJS 25/05/10 TJS 02/12/11
        Dim bAccountNodesUpdated As Boolean ' TJS 02/10/13

        ConfigUpdated = False ' TJS 02/12/11
        bAccountNodesUpdated = False ' TJS 02/10/13
        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 11/03/09 TJS 13/04/10 TJS 17/02/12
            If ConnectorProductCode = m_BaseProductCode Then
                Return True
            Else
                For iLoop = 0 To ConnectorStates.Length - 1
                    If ConnectorStates(iLoop).ProductCode = ConnectorProductCode Then
                        rowConfig = Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(ConnectorStates(iLoop).SourceCode) ' TJS 18/03/11
                        ' does source exist (need to do this in case initianl registration screwed up) ?
                        If rowConfig IsNot Nothing Then ' TJS 02/12/11
                            ' yes
                            XMLConfig = XDocument.Parse(Trim(rowConfig.ConfigSettings_DEV000221)) ' TJS 18/03/11 TJS 02/12/11
                            strSourceBusinessType = GetXMLElementText(XMLConfig, SOURCE_CONFIG_CUSTOMER_BUSINESS_TYPE) ' TJS 02/12/11
                            If strSourceBusinessType = "Consumer" Then ' TJS 02/12/11
                                strSourceBusinessType = "Retail" ' TJS 02/12/11
                            ElseIf strSourceBusinessType = "Business" Then ' TJS 02/12/11
                                strSourceBusinessType = "Wholesale" ' TJS 02/12/11
                            End If
                            XMLAccountNodeList = XMLConfig.XPathSelectElements(ConnectorStates(iLoop).AccountConfigXMLPath)
                            ' get Account Config group name for Connector
                            ' is there a group name i.e. a / in the Account config XML path?
                            iSlashPosn = InStrRev(ConnectorStates(iLoop).AccountConfigXMLPath, "/") ' TJS 25/05/10
                            If iSlashPosn <= 0 Then ' TJS 25/05/10
                                ' no
                                Return True ' TJS 25/05/10
                            Else
                                ' yes, get it
                                strGroupName = Mid(ConnectorStates(iLoop).AccountConfigXMLPath, iSlashPosn + 1) ' TJS 25/05/10
                                ' has maximum connector count been exceeded ?
                                iAccountCount = GetElementListCount(XMLAccountNodeList) ' TJS 02/12/11
                                If iAccountCount > ConnectorStates(iLoop).MaxAccounts Then
                                    ' yes, need to remove some account config entries
                                    strGroupNameToDelete = Me.m_ImportExportDataset.XMLConfigSettings(Me.m_ImportExportDataset.XMLConfigSettings.Count - 1).ConfigGroup
                                    For iAccountLoop = iAccountCount - 1 To ConnectorStates(iLoop).MaxAccounts Step -1 ' TJS 02/12/11
                                        If UpdateConfigSettingsDataset Then ' TJS 18/03/11
                                            For iConfigRowLoop = Me.m_ImportExportDataset.XMLConfigSettings.Count - 1 To 0 Step -1
                                                If Me.m_ImportExportDataset.XMLConfigSettings(iConfigRowLoop).RowState <> DataRowState.Deleted Then
                                                    ' is row config group name the same as the last row was ?
                                                    If Me.m_ImportExportDataset.XMLConfigSettings(iConfigRowLoop).ConfigGroup = strGroupNameToDelete Then ' TJS 02/12/11
                                                        ' yes, remove it
                                                        Me.m_ImportExportDataset.XMLConfigSettings(iConfigRowLoop).Delete()
                                                    Else
                                                        ' no, update group name and stop
                                                        strGroupNameToDelete = Me.m_ImportExportDataset.XMLConfigSettings(iConfigRowLoop).ConfigGroup
                                                        Exit For
                                                    End If
                                                End If
                                            Next
                                        End If
                                        ' remove from XML
                                        strConfig = XMLConfig.ToString ' TJS 02/12/11
                                        iEndPosn = InStrRev(strConfig, "</" & strGroupName & ">") ' TJS 02/12/11
                                        iStartPosn = InStrRev(strConfig, "<" & strGroupName & ">", iEndPosn) ' TJS 02/12/11
                                        strTemp = strConfig.Substring(0, iStartPosn - 1) & strConfig.Substring(iEndPosn + Len("</" & strGroupName & ">")) ' TJS 02/12/11
                                        XMLConfig = XDocument.Parse(strTemp) ' TJS 02/12/11
                                    Next
                                    If UpdateConfigSettingsDataset Then ' TJS 18/03/11
                                        Me.m_ImportExportDataset.XMLConfigSettings.AcceptChanges()
                                    End If
                                    Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221 = XMLConfig.ToString ' TJS 19/08/10 TJS 02/12/11
                                    ConfigUpdated = True ' TJS 02/12/11
                                    Return False

                                ElseIf iAccountCount < ConnectorStates(iLoop).MaxAccounts And ConnectorStates(iLoop).AccountCoreConfigSettings <> "" Then ' TJS 14/08/09
                                    ' no, need to add some account config entries
                                    XMLAccountConfig = XDocument.Parse(ConnectorStates(iLoop).AccountCoreConfigSettings) ' TJS 02/12/11
                                    For iAccountLoop = iAccountCount To ConnectorStates(iLoop).MaxAccounts - 1
                                        XMLConfig.XPathSelectElement("eShopCONNECTConfig").Add(XMLAccountConfig.Nodes) ' TJS 02/12/11
                                        If UpdateConfigSettingsDataset Then ' TJS 18/03/11
                                            strGroupName = Me.m_ImportExportDataset.XMLConfigSettings(Me.m_ImportExportDataset.XMLConfigSettings.Count - 1).ConfigGroup
                                            ' does account group alreay have a suffix ?
                                            If strGroupName.IndexOf(" : ") >= 0 Then
                                                ' yes, get existing suffix letter and increment
                                                strGroupName = strGroupName.Substring(0, strGroupName.Length - 1) & Chr(Asc(strGroupName.Substring(strGroupName.Length - 1, 1)) + 1)
                                            Else
                                                ' no, need to add suffix to existing group 
                                                For iConfigRowLoop = Me.m_ImportExportDataset.XMLConfigSettings.Count - 1 To 0 Step -1
                                                    If Me.m_ImportExportDataset.XMLConfigSettings(iConfigRowLoop).ConfigGroup = strGroupName Then
                                                        Me.m_ImportExportDataset.XMLConfigSettings(iConfigRowLoop).ConfigGroup = strGroupName & " : A"
                                                    Else
                                                        Exit For
                                                    End If
                                                Next
                                                strGroupName = strGroupName & " : B"
                                            End If
                                            XMLConfigNodeList = XMLAccountConfig.Elements.Elements
                                            For Each XMLConfigNode In XMLConfigNodeList
                                                rowXMLConfig = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                                rowXMLConfig.ConfigGroup = strGroupName
                                                rowXMLConfig.ConfigSettingName = XMLConfigNode.Name.ToString
                                                rowXMLConfig.ConfigSettingValue = XMLConfigNode.Value
                                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowXMLConfig)
                                            Next
                                        End If
                                    Next
                                    rowConfig.ConfigSettings_DEV000221 = XMLConfig.ToString ' TJS 19/08/10 TJS 18/03/11 TJS 02/12/11
                                    UpdateWebsiteCodes(Me, ConnectorStates(iLoop).SourceCode, strSourceBusinessType, XMLAccountNodeList, bAccountNodesUpdated) ' TJS 18/04/11 TJS 02/12/11 TJS 02/10/13
                                    If bAccountNodesUpdated Then ' TJS 02/10/13
                                        Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221 = XMLConfig.ToString ' TJS 02/10/13
                                    End If
                                    ConfigUpdated = True ' TJS 02/12/11
                                    Return True

                                Else
                                    ' correct number of account config entries
                                    UpdateWebsiteCodes(Me, ConnectorStates(iLoop).SourceCode, strSourceBusinessType, XMLAccountNodeList, bAccountNodesUpdated) ' TJS 18/04/11 TJS 02/12/11 TJS 02/10/13
                                    If bAccountNodesUpdated Then ' TJS 02/10/13
                                        Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221 = XMLConfig.ToString ' TJS 02/10/13
                                        ConfigUpdated = True ' TJS 02/10/13
                                    End If
                                    Return True
                                End If

                                Exit For
                            End If
                        End If
                    End If
                Next
            End If
        Else
            Return Nothing ' TJS 11/03/09
        End If

    End Function
#End Region

#Region " UpdateWebsiteCodes "
    Private Sub UpdateWebsiteCodes(ByVal sender As Object, ByVal SourceCode As String, ByVal BusinessTypeToUse As String, _
        ByRef XMLAccountNodeList As System.Collections.Generic.IEnumerable(Of XElement), ByRef AccountNodesUpdates As Boolean) ' TJS 02/12/11 TJS 02/10/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/04/11 | TJS             | 2011.0.11 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, 
        '                                        | added BusinessTypeToUse parameter and added eBay plus added Magento Store list processing
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 02/10/13 | TJS             | 2013.3.03 | Modified to save Magento Version in config
        ' 13/11/13 | TJS             | 2012.3.08 C Corrected saving of Magento Version and added API Version
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable ' TJS 02/12/11
        Dim XMLAccountConfig As XDocument, XMLMagentoStores As XDocument, XMLTemp As XDocument, XMLMember As XDocument, XMLAccountNode As XElement ' TJS 02/12/11
        Dim XMLStoreList As System.Collections.Generic.IEnumerable(Of XElement), XMLStoreNode As XElement ' TJS 02/12/11
        Dim XMLMagentoMembersList As System.Collections.Generic.IEnumerable(Of XElement), XMLMagentoNode As XElement ' TJS 02/12/11
        Dim strAccountOrInstanceID As String, strWebSiteCodeToUse As String, strDescriptionToUse As String
        Dim iSuffix As Integer, bIsActive As Boolean, bUpdateWebsiteDetails As Boolean ' TJS 02/12/11

        iSuffix = 1
        For Each XMLAccountNode In XMLAccountNodeList
            bUpdateWebsiteDetails = False ' TJS 02/12/11
            XMLAccountConfig = XDocument.Parse(XMLAccountNode.ToString) ' TJS 02/12/11
            Select Case SourceCode
                Case AMAZON_SOURCE_CODE ' TJS 02/12/11
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "Amazon/MerchantToken") ' TJS 22/03/13
                    strWebSiteCodeToUse = "Amazon-" & Right("00" & iSuffix, 2)
                    strDescriptionToUse = "Amazon" & GetXMLElementText(XMLAccountConfig, "Amazon/AmazonSite") & " connected via eShopCONNECT - Merchant Token " & strAccountOrInstanceID
                    bUpdateWebsiteDetails = True ' TJS 02/12/11

                Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 02/12/11
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "ASPStoreFront/SiteID")
                    strWebSiteCodeToUse = "ASPStorefront-" & Right("00" & iSuffix, 2)
                    strDescriptionToUse = "ASPStorefront connected via eShopCONNECT - Site ID " & strAccountOrInstanceID
                    bUpdateWebsiteDetails = True ' TJS 02/12/11

                Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 02/12/11
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "ChannelAdvisor/AccountID")
                    strWebSiteCodeToUse = "ChannelAdvisor-" & Right("00" & iSuffix, 2)
                    strDescriptionToUse = "ChannelAdvisor connected via eShopCONNECT - Account ID " & strAccountOrInstanceID
                    bUpdateWebsiteDetails = True ' TJS 02/12/11

                Case MAGENTO_SOURCE_CODE ' TJS 02/12/11
                    XMLMagentoStores = Nothing ' TJS 02/12/11
                    RaiseEvent GetMagentoStoreList(Me, XMLAccountConfig, XMLMagentoStores) ' TJS 02/12/11
                    ' start of code added TJS 02/10/13
                    XMLMagentoNode = XMLAccountNode.XPathSelectElement("MagentoVersion") ' TJS 13/11/13
                    If XMLMagentoNode IsNot Nothing Then
                        If XMLMagentoNode.Value <> XMLAccountConfig.XPathSelectElement(SOURCE_CONFIG_XML_MAGENTO_VERSION).Value Then ' TJS 13/11/13
                            XMLMagentoNode.Value = XMLAccountConfig.XPathSelectElement(SOURCE_CONFIG_XML_MAGENTO_VERSION).Value ' TJS 13/11/13
                            AccountNodesUpdates = True
                        End If
                    End If
                    ' end of code added TJS 02/10/13
                    ' start of code added TJS 13/11/13
                    XMLMagentoNode = XMLAccountNode.XPathSelectElement("LerrynAPIVersion")
                    If XMLMagentoNode IsNot Nothing Then
                        If XMLMagentoNode.Value <> XMLAccountConfig.XPathSelectElement(SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION).Value Then
                            XMLMagentoNode.Value = XMLAccountConfig.XPathSelectElement(SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION).Value
                            AccountNodesUpdates = True
                        End If
                    End If
                    ' end of code added TJS 13/11/13

                    If XMLMagentoStores IsNot Nothing Then ' TJS 02/12/11
                        XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                        XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                        XMLStoreList = XMLMagentoStores.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento) ' TJS 02/12/11
                        For Each XMLStoreNode In XMLStoreList ' TJS 02/12/11
                            XMLTemp = XDocument.Parse(XMLStoreNode.ToString) ' TJS 02/12/11
                            XMLMagentoMembersList = XMLTemp.XPathSelectElements("item/value/item") ' TJS 02/12/11
                            bIsActive = False
                            strAccountOrInstanceID = ""
                            strDescriptionToUse = ""
                            For Each XMLMagentoNode In XMLMagentoMembersList ' TJS 02/12/11
                                XMLMember = XDocument.Parse(XMLMagentoNode.ToString) ' TJS 02/12/11
                                If GetXMLElementText(XMLMember, "item/key") = "storeid" Then ' TJS 02/12/11
                                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "Magento/InstanceID") & ":" & GetXMLElementText(XMLMember, "item/value") ' TJS 02/12/11

                                ElseIf GetXMLElementText(XMLMember, "item/key") = "storename" Then ' TJS 02/12/11
                                    strDescriptionToUse = "Magento connected via eShopCONNECT - Instance ID " & GetXMLElementText(XMLAccountConfig, "Magento/InstanceID") & ", Store - " & GetXMLElementText(XMLMember, "item/value") ' TJS 02/12/11

                                ElseIf GetXMLElementText(XMLMember, "item/key") = "isactive" Then ' TJS 02/12/11
                                    bIsActive = CBool(GetXMLElementText(XMLMember, "item/value")) ' TJS 02/12/11
                                End If
                            Next
                            If bIsActive And strAccountOrInstanceID <> "" And strDescriptionToUse <> "" Then ' TJS 02/12/11
                                strWebSiteCodeToUse = "Magento-" & Right("00" & iSuffix, 2) ' TJS 02/12/11
                                RaiseEvent CheckWebsiteCodeExists(Me, SourceCode, strAccountOrInstanceID, strWebSiteCodeToUse, strDescriptionToUse, BusinessTypeToUse) ' TJS 02/12/11
                                iSuffix += 1
                            End If
                        Next

                    End If

                Case SHOP_COM_SOURCE_CODE ' TJS 02/12/11
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "ShopDotCom/CatalogID")
                    strWebSiteCodeToUse = "ShopDotCom-" & Right("00" & iSuffix, 2)
                    strDescriptionToUse = "ShopDotCom connected via eShopCONNECT - Catalog ID " & strAccountOrInstanceID
                    bUpdateWebsiteDetails = True ' TJS 02/12/11

                Case VOLUSION_SOURCE_CODE ' TJS 02/12/11
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "Volusion/SiteID")
                    strWebSiteCodeToUse = "Volusion-" & Right("00" & iSuffix, 2)
                    strDescriptionToUse = "Volusion connected via eShopCONNECT - Site ID " & strAccountOrInstanceID
                    bUpdateWebsiteDetails = True ' TJS 02/12/11

                Case EBAY_SOURCE_CODE ' TJS 02/12/11
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "eBay/SiteID") ' TJS 02/12/11
                    strWebSiteCodeToUse = "eBay-" & Right("00" & iSuffix, 2) ' TJS 02/12/11
                    strDescriptionToUse = "eBay connected via eShopCONNECT - Site ID " & strAccountOrInstanceID ' TJS 02/12/11
                    bUpdateWebsiteDetails = True ' TJS 02/12/11

                Case SEARS_COM_SOURCE_CODE ' TJS 16/01/12
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "SearsDotCom/SiteID") ' TJS 16/01/12
                    strWebSiteCodeToUse = "SearsDotCom-" & Right("00" & iSuffix, 2) ' TJS 16/01/12
                    strDescriptionToUse = "Sears.com connected via eShopCONNECT - Site ID " & strAccountOrInstanceID ' TJS 16/01/12
                    bUpdateWebsiteDetails = True ' TJS 16/01/12

                    ' start of code added TJS 05/07/12
                Case AMAZON_SOURCE_CODE
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "AmazonFBA/MerchantToken") ' TJS 22/03/13
                    strWebSiteCodeToUse = "Amazon-" & Right("00" & iSuffix, 2)
                    strDescriptionToUse = "Amazon" & GetXMLElementText(XMLAccountConfig, "AmazonFBA/AmazonSite") & " connected via eShopCONNECT - Merchant ID " & strAccountOrInstanceID
                    bUpdateWebsiteDetails = True
                    ' end of code added TJS 05/07/12

                    ' start of code added TJS 20/11/13
                Case THREE_D_CART_SOURCE_CODE
                    strAccountOrInstanceID = GetXMLElementText(XMLAccountConfig, "ThreeDCart/StoreID")
                    strWebSiteCodeToUse = "3DCart-" & Right("00" & iSuffix, 2)
                    strDescriptionToUse = "3DCart connected via eShopCONNECT - Store ID " & strAccountOrInstanceID
                    bUpdateWebsiteDetails = True
                    ' end of code added TJS 20/11/13

                Case Else
                    Return
            End Select
            If bUpdateWebsiteDetails Then ' TJS 02/12/11
                RaiseEvent CheckWebsiteCodeExists(Me, SourceCode, strAccountOrInstanceID, strWebSiteCodeToUse, strDescriptionToUse, BusinessTypeToUse) ' TJS 02/12/11
            End If
            iSuffix += 1
        Next

    End Sub
#End Region

#Region " CheckInventoryImportLimit "
    Public Function CheckInventoryImportLimit(ByVal ProductsImportedToDate As Integer, ByVal DuringImport As Boolean, _
        ByRef OverlimitMessage As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/03/11 | TJS             | 2011.0.04 | Rewritten to cater for operation without connector activation
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If ProductsImportedToDate >= InventoryImportLimit Then
            If DuringImport Then
                OverlimitMessage = "You have reached "
            Else
                OverlimitMessage = "You have already imported "
            End If
            OverlimitMessage = OverlimitMessage & "the maximum number of Inventory Items permitted on your "
            If IsFullActivation Then
                OverlimitMessage = OverlimitMessage & "activation"
            Else
                OverlimitMessage = OverlimitMessage & "evaluation activation"
            End If
            OverlimitMessage = OverlimitMessage & "." & vbCrLf & vbCrLf & "You must run the eShopCONNECT Activation Wizard and purchase "
            If IsFullActivation Then
                OverlimitMessage = OverlimitMessage & "an upgraded activation"
            Else
                OverlimitMessage = OverlimitMessage & "a full activation"
            End If
            OverlimitMessage = OverlimitMessage & " before you can import any more Inventory Items."
            Return True
        Else
            Return False
        End If

    End Function
#End Region

#Region " LoadConfigSettings "
    Public Function LoadConfigSettings(ByRef SourceConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row, _
        ByRef SourceSet As SourceSettings, ByRef FileImportRequired As Boolean, ByRef WebIORequired As Boolean, ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Extracts the source settings
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/10/08 | Jonathan Foster | 1.0.0.0   | Original
        ' 20/01/09 | TJS             | 2009.1.01 | Modified to use SourceType instead of integer for source selection
        '                                        | and rewritten to use GetXMLElementText instead of xml navigation
        ' 28/01/09 | TJS             | 2009.1.02 | Modified to include config XML within SourceSettings
        ' 02/02/09 | TJS             | 2009.1.04 | Modified to include SourceCode and SourceType within SourceSettings
        ' 06/02/09 | TJS             | 2009.1.05 | Modified to include Shop.Com StatusPostURL
        ' 22/02/09 | TJS             | 2009.1.08 | Modified to cater for Amazon config and Shop.com ItemIDField and renamed
        '                                        | SOURCE_CONFIG_DEFAULT_CREDIT_CARD_PAYMENT_TERM to reflect underlying DB function
        ' 10/03/09 | TJS             | 2009.1.09 | Modified to ensure aall paths end in \ and to enable amazon file i/o
        ' 17/03/09 | TJS             | 2009.1.10 | Added File Input eShopCONNECTOR plus DisableFreightCalculation and ShippingModuleToUse
        ' 11/05/09 | TJS             | 2009.2.06 | renamed SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST
        ' 29/05/09 | TJS             | 2009.2.09 | Added checks for XML load errors
        ' 04/06/09 | TJS             | 2009.2.10 | Added CreateCustomerAsCompany, EnableCoupons, IgnoreVoidedOrdersAndInvoices, 
        '                                        | AcceptSourceSalesTaxCalculation, FTPUploadURL, FTPUploadUserName
        '                                        | FTPUploadPassword, PricesAreTaxInclusive and DefaultUpliftPercent
        ' 18/06/09 | TJS             | 2009.2.14 | Added FTPUploadPath
        ' 21/06/09 | TJS             | 2009.3.00 | Added Volusion settings and Shop.com date format
        ' 15/08/09 | TJS             | 2009.3.03 | Added SOURCE_CONFIG_XML_VOLUSION_ALLOW_SHIPPING_LAST_NAME_BLANK
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to support direct connection to Amazon
        ' 10/12/09 | TJS             | 2009.3.09 | Added SOURCE_CONFIG_XML_FILE_SAVE_PATH and SOURCE_CONFIG_XML_AMAZON_PRICES_ARE_TAX_INCLUSIVE
        '                                        | plus Channel Advisor connector
        ' 17/12/09 | TJS             | 2009.3.11 | Corrected check for trailing \ on XMLImportFileSavePath
        ' 30/12/09 | TJS             | 2010.0.00 | Added AMazon and Channel ADvisor payment types
        ' 05/01/10 | TJS             | 2010.0.01 | Modified to cater for lead, prospect and customer import
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for Magento and ASLDotNetStoreFront connector settings 
        '                                        | and Channel ADvisor customisations for J L Concepts plus Source Code constants
        ' 22/09/10 | TJS             | 2010.1.01 | Added Channel Advisor EnablePaymentTypeTranslation
        ' 18/10/10 | TJS             | 2010.1.06 | Corrected File_Import constant name
        ' 07/01/11 | TJs             | 2010.1.15 | Removed NotificationEmailIISConfigSource and EnablePollForOrders from Magento and ASPStorefront
        '                                        | and added AccountDisabled in Amazon
        ' 18/03/11 | TJS             | 2011.0.01 | Changed Magento SiteID to InstanceID to prevent confusion with WebsiteID within an Instance
        ' 29/03/11 | TJS             | 2011.0.04 | Modified to initialise Channel ADvisor 
        ' 04/04/11 | TJS             | 2011.0.07 | Removed timestamp for checking Payment failure updates
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Amazon connection using MWS instead of SOAP
        ' 26/10/11 | TJS             | 2011.1.xx | Corrected setting of source tax values and codes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and added eBay code
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 14/02/12 | TJS             | 2011.2.05 | Added ProductListBlockSize to Magento settings and modified to detect blank XML Dates
        ' 17/02/12 | TJS             | 2011.2.07 | Corrected Sears.com settings
        ' 24/02/12 | TJS             | 2011.2.08 | Modified for ASPStorefront Extension Data Field mapping
        ' 19/03/12 | TJS             | 2011.2.10 | Modified to prevent errors if Integer settings are empty
        ' 20/04/12 | TJS             | 2012.1.02 | Added MAgento SplitSKUSeparatorCharacters setting
        ' 10/06/12 | TJS             | 2012.1.00 | Added MAgento EnablePaymentTypeTranslation and removed redundant MAgento Currency setting
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to cater for different Amazon developerids in different countries
        '                                        | and code moved from Windows Service Settings.vb
        ' 08/07/12 | TJS             | 2012.1.09 | Modified to cater for UseShipToClassTemplate
        ' 18/01/13 | TJS             | 2012.1.17 | Added AllocateAndReserveStock
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 30/04/13 | TJS             | 2013.1.11 | Added Magento InhibitInventoryUpdates and CreateCustomerForGuestCheckout
        ' 08/05/13 | TJS             | 2013.1.13 | Added Magento IncludeChildItemsOnOrder
        ' 30/07/13 | TJS             | 2013.1.32 | Added eBay PricesAreTaxInclusive and TaxCodeForSourceTax
        ' 19/09/13 | TJS             | 2013.3.00 | Added generic ImportMissingItemsAsNonStock
        ' 02/10/13 | TJS             | 2013.3.03 | Modified to decode Config XML Values such as password etc and to load Magento Version
        ' 13/11/13 | TJS             | 2013.3.08 | Added Magento V2SoapAPIWSICompliant, Magento API Extension Version, UpdateMagentoSpecialPricing and LastDailyTasksRun
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector and corrected loading of Magento Daily Tasks run list
        ' 11/04/14 | TJS             | 2013.4.04 | Added Volusion DefaultShippingMethodID
        ' 15/04/14 | TJS             | 2013.4.05 | Added Volusion EnableSKUAliasLookup
        ' 11/02/14 | TJS             | 2013.4.09 | added Volusion EnablePaymentTypeTranslation
        ' 01/05/14 | TJS             | 2014.0.02 | Added CardAuthAndCaptureWithOrder, ImportAllOrdersAsSingleCustomer and OverrideMagentoPricesWith
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument, XMLLastUpdateTimes As XDocument, XMLLastDailyTaskTimes As XDocument ' TJS 13/11/13
        Dim XMLLastUpdateTime As XElement, XMLMerchantID As XElement, XMLLastDailyTaskTime As XElement ' TJS 19/08/10 TJS 13/11/13
        Dim XMLLastUpdateTimeList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 19/08/10
        Dim XMLLastDailyTaskRunList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 13/11/13
        Dim XMLMerchantIDs As System.Collections.Generic.IEnumerable(Of XElement)
        Dim ShopSet As ShopComSettings, AmazonSet As AmazonSettings ' TJS 22/02/09
        Dim VolusionSet As VolusionSettings, ChannelAdvSet As ChannelAdvisorSettings ' TJS 21/06/09 TJS 10/12/09
        Dim MagentoSet As MagentoSettings, ASPStoreFrontSet As ASPStoreFrontSettings ' TJS 19/08/10
        Dim eBaySet As eBaySettings, SearsSet As SearsComSettings, ThreeDCartSet As ThreeDCartSettings ' TJS 02/12/11 TJS 16/01/12 TJS 20/11/13
        Dim strTemp As String, bXMLError As Boolean, bReturnValue As Boolean ' TJS 14/02/12 TJS 05/07/12

        bReturnValue = True ' TJS 05/07/12
        Try
            XMLConfig = XDocument.Parse(SourceConfig.ConfigSettings_DEV000221) ' TJS 19/08/10

        Catch ex As Exception
            ' can't use m_ImportExportConfigFacade.SendErrorEmail here as config not loaded so can't get settings
            ErrorDetails = "Failed to read XML config settings for " & SourceConfig.SourceName_DEV000221 & ", reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & SourceConfig.ConfigSettings_DEV000221 ' TJS 10/06/12 TJS 05/07/12
            Return False ' TJS 29/05/09

        End Try
        ' XML loaded correctly, get General Settings for source

        SourceSet.SourceCode = SourceConfig.SourceCode_DEV000221 ' TJS 02/02/09 TJS 19/08/10
        SourceSet.SourceName = SourceConfig.SourceName_DEV000221 ' TJS 02/02/09 TJS 06/02/09 TJS 19/08/10
        SourceSet.SourceInputHandler = SourceConfig.InputHandler_DEV000221 ' TJS 17/03/09 TJS 19/08/10
        SourceSet.XMLConfig = XMLConfig ' TJS 28/01/09
        'SourceSet.NotificationEmailIISConfigSource = GetXMLElementText(XMLConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_IIS_CONFIG_SOURCE) ' TJS 07/01/11
        SourceSet.ErrorNotificationEmailAddress = GetXMLElementText(XMLConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
        SourceSet.SendCodeErrorEmailsToLerryn = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN)) = "YES", True, False))
        SourceSet.SendSourceErrorEmailsToLerryn = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_SEND_SOURCE_ERROR_EMAILS_TO_LERRYN)) = "YES", True, False))
        SourceSet.RequireSourceCustomerID = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_REQUIRE_SOURCE_CUSTOMER_ID)) = "YES", True, False))
        SourceSet.CustomerBusinessType = GetXMLElementText(XMLConfig, SOURCE_CONFIG_CUSTOMER_BUSINESS_TYPE)
        SourceSet.CustomerBusinessClass = GetXMLElementText(XMLConfig, SOURCE_CONFIG_CUSTOMER_BUSINESS_CLASS)
        SourceSet.CreateCustomerAsCompany = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_CREATE_CUSTOMER_AS_COMPANY)) = "YES", True, False)) ' TJS 04/06/09
        SourceSet.ShippingModuleToUse = GetXMLElementText(XMLConfig, SOURCE_CONFIG_SHIPPING_MODULE_TO_USE) ' TJS 17/03/09
        SourceSet.EnableDeliveryMethodTranslation = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION)) = "YES", True, False))
        SourceSet.UseShipToClassTemplate = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_USE_SHIPTO_CLASS_TEMPLATE)) = "YES", True, False)) ' TJS 08/07/12
        SourceSet.DefaultShippingMethod = GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD)
        SourceSet.DefaultShippingMethodGroup = GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP)
        SourceSet.CreditCardPaymentTermCode = GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_CREDIT_CARD_PAYMENT_TERM) ' TJS 22/02/09
        If GetXMLElementText(XMLConfig, SOURCE_CONFIG_DUE_DATE_OFFSET) <> "" Then ' TJS 05/01/10
            SourceSet.DueDateDaysInFuture = CInt(GetXMLElementText(XMLConfig, SOURCE_CONFIG_DUE_DATE_OFFSET)) ' TJS 05/01/10
        Else
            SourceSet.DueDateDaysInFuture = 0 ' TJS 05/01/10
        End If
        SourceSet.AuthoriseCreditCardOnImport = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_AUTHORISE_CARD_ON_IMPORT)) = "YES", True, False))
        SourceSet.EnableCoupons = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_ENABLE_COUPONS)) = "YES", True, False)) ' TJS 04/06/09
        SourceSet.RequireSourceCustomerID = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_REQUIRE_SOURCE_CUSTOMER_ID)) = "YES", True, False))
        SourceSet.DisableFreightCalculation = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_SET_DISABLE_FREIGHT_CALCULATION)) = "YES", True, False)) ' TJS 17/03/09
        SourceSet.IgnoreVoidedOrdersAndInvoices = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_IGNORE_VOIDED_ORDERS_AND_INVOICES)) = "YES", True, False)) ' TJS 04/06/09
        SourceSet.AcceptSourceSalesTaxCalculation = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_ACCEPT_SOURCE_SALES_TAX_CALCULATION)) = "YES", True, False)) ' TJS 04/06/09
        SourceSet.ImportMissingItemsAsNonStock = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_IMPORT_MISSING_ITEMS_AS_NONSTOCK)) = "YES", True, False)) ' TJS 19/09/13
        SourceSet.AllocateAndReserveStock = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_ALLOCATE_AND_RESERVE_STOCK)) = "YES", True, False)) ' TJS 18/01/13
        SourceSet.EnableLogFile = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_ENABLE_LOG_FILE)) = "YES", True, False))
        SourceSet.LogFilePath = GetXMLElementText(XMLConfig, SOURCE_CONFIG_LOG_FILE_PATH)
        If SourceSet.LogFilePath <> "" Then ' TJS 10/03/09
            If SourceSet.LogFilePath.Substring(SourceSet.LogFilePath.Length - 1) <> "\" Then ' TJS 10/03/09
                SourceSet.LogFilePath = SourceSet.LogFilePath & "\" ' TJS 10/03/09
            End If
        End If
        SourceSet.EnablePollGenericImportPath = CBool(IIf(UCase(GetXMLElementText(XMLConfig, SOURCE_CONFIG_POLL_GENERIC_IMPORT_PATH)) = "YES", True, False))
        SourceSet.GenericImportPath = GetXMLElementText(XMLConfig, SOURCE_CONFIG_GENERIC_IMPORT_PATH)
        If SourceSet.GenericImportPath <> "" Then ' TJS 10/03/09
            If SourceSet.GenericImportPath.Substring(SourceSet.GenericImportPath.Length - 1) <> "\" Then ' TJS 10/03/09
                SourceSet.GenericImportPath = SourceSet.GenericImportPath & "\" ' TJS 10/03/09
            End If
        End If
        If SourceSet.EnablePollGenericImportPath And SourceSet.GenericImportPath <> "" Then ' TJS 22/02/09 TJS 10/03/09
            FileImportRequired = True ' TJS 22/02/09
        End If
        SourceSet.GenericImportProcessedPath = GetXMLElementText(XMLConfig, SOURCE_CONFIG_GENERIC_IMPORT_PROCESSED_PATH)
        If SourceSet.GenericImportProcessedPath <> "" Then ' TJS 10/03/09
            If SourceSet.GenericImportProcessedPath.Substring(SourceSet.GenericImportProcessedPath.Length - 1) <> "\" Then ' TJS 10/03/09
                SourceSet.GenericImportProcessedPath = SourceSet.GenericImportProcessedPath & "\" ' TJS 10/03/09
            End If
        End If
        SourceSet.GenericImportErrorPath = GetXMLElementText(XMLConfig, SOURCE_CONFIG_GENERIC_IMPORT_ERROR_PATH)
        If SourceSet.GenericImportErrorPath <> "" Then ' TJS 10/03/09
            If SourceSet.GenericImportErrorPath.Substring(SourceSet.GenericImportErrorPath.Length - 1) <> "\" Then ' TJS 10/03/09
                SourceSet.GenericImportErrorPath = SourceSet.GenericImportErrorPath & "\" ' TJS 10/03/09
            End If
        End If
        SourceSet.XMLImportFileSavePath = GetXMLElementText(XMLConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH) ' TJS 10/12/09
        If SourceSet.XMLImportFileSavePath <> "" Then ' TJS 10/12/09
            If SourceSet.XMLImportFileSavePath.Substring(SourceSet.XMLImportFileSavePath.Length - 1) <> "\" Then ' TJS 10/12/09 TJS 17/12/09
                SourceSet.XMLImportFileSavePath = SourceSet.XMLImportFileSavePath & "\" ' TJS 10/12/09
            End If
        End If
        SourceSet.FTPEnabled = False

        ' now do Source specific settings
        Select Case SourceConfig.SourceCode_DEV000221 ' TJS 19/08/10
            Case GENERIC_IMPORT_SOURCE_CODE, FILE_IMPORT_SOURCE_CODE, PROSPECT_LEAD_IMPORT_SOURCE_CODE ' TJS 17/03/09 TJS 19/08/10 TJS/FA 18/10/10

            Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST) ' TJS 06/02/09 TJS 11/05/09
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load Shop.com settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 29/05/09 TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then ' TJS 29/05/09
                        ' yes, get Shop.com Settings for source
                        ShopSet = New ShopComSettings

                        ShopSet.CatalogID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_ID)
                        ShopSet.StatusPostURL = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_STATUS_POST_URL) ' TJS 06/02/09
                        ShopSet.FTPUploadURL = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_URL)) ' TJS 04/06/09 TJS 02/10/13
                        ShopSet.FTPUploadUserName = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_USERNAME)) ' TJS 04/06/09 TJS 02/10/13
                        ShopSet.FTPUploadPassword = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_PASSWORD)) ' TJS 04/06/09 TJS 02/10/13
                        ShopSet.FTPUploadPath = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_PATH) ' TJS 18/06/09
                        If ShopSet.FTPUploadPath <> "" Then ' TJS 18/06/09
                            If ShopSet.FTPUploadPath.Substring(ShopSet.FTPUploadPath.Length - 1) <> "\" Then ' TJS 18/06/09
                                ShopSet.FTPUploadPath = ShopSet.FTPUploadPath & "\" ' TJS 18/06/09
                            End If
                        End If
                        ShopSet.FTPUploadArchivePath = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_FTP_UPLOAD_ARCHIVE_PATH) ' TJS 18/06/09
                        If ShopSet.FTPUploadArchivePath <> "" Then ' TJS 18/06/09
                            If ShopSet.FTPUploadArchivePath.Substring(ShopSet.FTPUploadArchivePath.Length - 1) <> "\" Then ' TJS 18/06/09
                                ShopSet.FTPUploadArchivePath = ShopSet.FTPUploadArchivePath & "\" ' TJS 18/06/09
                            End If
                        End If
                        ShopSet.SourceItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_SOURCE_ITEM_ID_FIELD) ' TJS 22/02/09
                        ShopSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_IS_ITEM_ID_FIELD) ' TJS 22/02/09
                        ShopSet.Currency = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CURRENCY) ' TJS 04/06/09
                        ShopSet.PricesAreTaxInclusive = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_PRICES_ARE_TAX_INCLUSIVE)) = "YES", True, False)) ' TJS 04/06/09
                        ShopSet.TaxCodeForSourceTax = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_TAX_CODE_FOR_SOURCE_TAX) ' TJS 26/10/11
                        ShopSet.DefaultUpliftPercent = CDec(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_DEFAULT_PRICE_UPLIFT)) ' TJS 04/06/09
                        ShopSet.DisableShopComPublishing = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_DISABLE_PUBLISHING)) = "YES", True, False)) ' TJS 04/06/09
                        ShopSet.UKNotUSDateFormat = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_XML_DATE_FORMAT)) = "DD/MM/YYYY", True, False)) ' TJS 21/06/09
                        ShopSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_ACCOUNT_DISABLED)) = "YES", True, False)) ' TJS 19/08/10
                        ShopSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SHOPDOTCOM_CUSTOM_SKU_PROCESSING)  ' TJS 19/08/10 - setting only visible for relevant customers
                        ShopSet.XMLConfig = XMLTemp ' TJS 18/06/09

                        SourceSet.AddShopComSettings(ShopSet)
                    End If
                Next

            Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST) ' TJS 22/02/09
                For Each XMLMerchantID In XMLMerchantIDs ' TJS 22/02/09
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString) ' TJS 22/02/09

                    Catch ex As Exception
                        ErrorDetails = "Failed to load Amazon settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 29/05/09 TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then ' TJS 29/05/09
                        ' yes, get Shop.com Settings for source
                        AmazonSet = New AmazonSettings ' TJS 22/02/09

                        AmazonSet.AmazonSite = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_SITE) ' TJS 15/10/09
                        AmazonSet.OwnAccessKeyID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_OWN_ACCESS_KEY_ID) ' TJS 05/07/12
                        AmazonSet.OwnSecretAccessKey = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_ADV_OWN_SECRET_ACCESS_KEY) ' TJS 05/07/12
                        AmazonSet.MerchantToken = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN) ' TJS 22/02/09 TJS 22/03/13
                        AmazonSet.MerchantName = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_NAME) ' TJS 15/10/09
                        AmazonSet.MWSMerchantID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MWS_MERCHANT_ID) ' TJS 15/10/09 TJS 09/07/11
                        AmazonSet.MWSMarketplaceID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MWS_MARKETPLACE_ID) ' TJS 15/10/09 TJS 09/07/11
                        AmazonSet.ManualProcessingPath = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MANUAL_PROCESS_PATH) ' TJS 22/02/09
                        If AmazonSet.ManualProcessingPath <> "" Then ' TJS 15/10/09
                            If AmazonSet.ManualProcessingPath.Substring(AmazonSet.ManualProcessingPath.Length - 1) <> "\" Then ' TJS 15/10/09
                                AmazonSet.ManualProcessingPath = AmazonSet.ManualProcessingPath & "\" ' TJS 15/10/09
                            End If
                        End If
                        AmazonSet.ImportProcessedPath = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_IMPORT_PROCESSED_PATH) ' TJS 22/02/09
                        If AmazonSet.ImportProcessedPath <> "" Then ' TJS 10/03/09
                            If AmazonSet.ImportProcessedPath.Substring(AmazonSet.ImportProcessedPath.Length - 1) <> "\" Then ' TJS 10/03/09
                                AmazonSet.ImportProcessedPath = AmazonSet.ImportProcessedPath & "\" ' TJS 10/03/09
                            End If
                        End If
                        AmazonSet.ImportErrorPath = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_IMPORT_ERROR_PATH) ' TJS 22/02/09
                        If AmazonSet.ImportErrorPath <> "" Then ' TJS 10/03/09
                            If AmazonSet.ImportErrorPath.Substring(AmazonSet.ImportErrorPath.Length - 1) <> "\" Then ' TJS 10/03/09
                                AmazonSet.ImportErrorPath = AmazonSet.ImportErrorPath & "\" ' TJS 10/03/09
                            End If
                        End If
                        AmazonSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_IS_ITEM_ID_FIELD) ' TJS 22/02/09
                        AmazonSet.PaymentType = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_PAYMENT_TYPE) ' TJS 30/12/09
                        AmazonSet.PricesAreTaxInclusive = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_PRICES_ARE_TAX_INCLUSIVE)) = "YES", True, False)) ' TJS 10/12/09
                        AmazonSet.TaxCodeForSourceTax = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_TAX_CODE_FOR_SOURCE_TAX) ' TJS 26/10/11
                        AmazonSet.DefaultUpliftPercent = CDec(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_DEFAULT_PRICE_UPLIFT)) ' TJS 04/06/09
                        AmazonSet.EnableSKUAliasLookup = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_ENABLE_SKU_ALIAS_LOOKUP)) = "YES", True, False)) ' TJS 19/08/10
                        AmazonSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_ACCOUNT_DISABLED)) = "YES", True, False)) ' TJS 07/01/11
                        AmazonSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_CUSTOM_SKU_PROCESSING) ' TJS 19/08/10 - setting only visible for relevant customers
                        ' set starting poll time
                        AmazonSet.NextConnectionTime = Date.Now.AddMinutes(1) ' TJS 15/10/09 TJS 10/12/09

                        SourceSet.AddAmazonSettings(AmazonSet) ' TJS 22/02/09
                        ' need web input 
                        WebIORequired = True ' TJS 22/11/09
                    End If
                Next

                ' start of code added TJS 21/06/09
            Case VOLUSION_SOURCE_CODE ' TJS 19/08/10
                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_VOLUSION_LIST)
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load Volusion settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then
                        ' yes, get Volusion Settings for source
                        VolusionSet = New VolusionSettings

                        VolusionSet.SiteID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_SITE_ID)
                        VolusionSet.OrderPollURL = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_ORDER_POLL_URL)) ' TJS 02/10/13
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_ORDER_POLL_INTERVAL_MINUTES) <> "" Then ' TJS 19/03/12
                            VolusionSet.OrderPollIntervalMinutes = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_ORDER_POLL_INTERVAL_MINUTES))
                        Else
                            VolusionSet.OrderPollIntervalMinutes = 15 ' TJS 19/03/12
                        End If
                        VolusionSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_IS_ITEM_ID_FIELD)
                        VolusionSet.PricesAreTaxInclusive = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_PRICES_ARE_TAX_INCLUSIVE)) = "YES", True, False)) ' TJS 02/12/11
                        VolusionSet.TaxCodeForSourceTax = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_TAX_CODE_FOR_SOURCE_TAX) ' TJS 26/10/11
                        VolusionSet.Currency = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_CURRENCY)
                        VolusionSet.AllowShippingLastNameBlank = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_ALLOW_SHIPPING_LAST_NAME_BLANK)) = "YES", True, False)) ' TJS 15/08/09
                        VolusionSet.EnablePaymentTypeTranslation = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_ENABLE_PAYMENT_TYPE_TRANSLATION)) = "YES", True, False)) ' TJS 11/02/14
                        VolusionSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_ACCOUNT_DISABLED)) = "YES", True, False)) ' TJS 19/08/10
                        VolusionSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_CUSTOM_SKU_PROCESSING) ' TJS 19/08/10 TJS 16/01/12 - setting only visible for relevant customers
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_DEFAULT_SHIPPING_METHOD_ID) <> "" Then ' TJS 11/01/14
                            VolusionSet.DefaultShippingMethodID = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_DEFAULT_SHIPPING_METHOD_ID)) ' TJS 11/01/14
                        Else
                            VolusionSet.DefaultShippingMethodID = 0 ' TJS 11/01/14
                        End If
                        VolusionSet.EnableSKUAliasLookup = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_VOLUSION_ENABLE_SKU_ALIAS_LOOKUP)) = "YES", True, False)) ' TJS 15/01/14
                        ' set starting poll time
                        VolusionSet.NextOrderPollTime = Date.Now.AddMinutes(1)

                        SourceSet.AddVolusionSettings(VolusionSet)
                        ' need web input 
                        WebIORequired = True
                    End If
                Next
                ' end of code added TJS 21/06/09

                ' start of code added TJS 10/12/09
            Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 19/08/10
                XMLLastUpdateTimes = Nothing ' TJS 14/02/12
                XMLLastUpdateTimeList = Nothing ' TJS 14/02/12
                ' have Last Order Status Update Date/Times been set ?
                If Not SourceConfig.IsLastOrderStatusUpdate_DEV000221Null Then ' TJS 19/08/10
                    If "" & SourceConfig.LastOrderStatusUpdate_DEV000221 <> "" Then ' TJS 19/08/10
                        ' yes, load values
                        XMLLastUpdateTimes = XDocument.Parse(SourceConfig.LastOrderStatusUpdate_DEV000221) ' TJS 19/08/10
                        XMLLastUpdateTimeList = XMLLastUpdateTimes.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST) ' TJS 19/08/10
                    End If
                End If

                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load Channel Advisor settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then
                        ' yes, get Shop.com Settings for source
                        ChannelAdvSet = New ChannelAdvisorSettings

                        ChannelAdvSet.OwnDeveloperKey = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_KEY)
                        ChannelAdvSet.OwnDeveloperPwd = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_OWN_DEV_PWD)
                        ChannelAdvSet.AccountName = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME)
                        ChannelAdvSet.AccountID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID)
                        ChannelAdvSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_IS_ITEM_ID_FIELD)
                        ChannelAdvSet.Currency = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_CURRENCY)
                        ChannelAdvSet.PaymentType = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_PAYMENT_TYPE) ' TJS 30/12/09
                        ChannelAdvSet.EnablePaymentTypeTranslation = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ENABLE_PAYMENT_TYPE_TRANSLATION)) = "YES", True, False)) ' TJS 22/09/10
                        ChannelAdvSet.PricesAreTaxInclusive = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_PRICES_ARE_TAX_INCLUSIVE)) = "YES", True, False))
                        ChannelAdvSet.TaxCodeForSourceTax = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_TAX_CODE_FOR_SOURCE_TAX) ' TJS 26/10/11
                        ChannelAdvSet.ActionIfNoPayment = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACTION_IF_NO_PMT) ' TJS 19/08/10
                        ChannelAdvSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_DISABLED)) = "YES", True, False)) ' TJS 19/08/10
                        ChannelAdvSet.EnableSKUAliasLookup = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ENABLE_SKU_ALIAS_LOOKUP)) = "YES", True, False)) ' TJS 19/08/10
                        ChannelAdvSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_CUSTOM_SKU_PROCESSING) ' TJS 19/08/10 - setting only visible for relevant customers
                        ' set starting poll time
                        ChannelAdvSet.NextConnectionTime = Date.Now.AddMinutes(1)
                        '  set default Last Order Status Update Date/Time value of 1 month ago
                        ChannelAdvSet.LastOrderStatusUpdate = Date.Today.AddMonths(-1) ' TJS 19/08/10
                        ' have Last Order/Payment Status Update Date/Times been set ?
                        If XMLLastUpdateTimes IsNot Nothing Then ' TJS 19/08/10
                            ' yes, find relevant entry
                            For Each XMLLastUpdateTime In XMLLastUpdateTimeList ' TJS 19/08/10
                                XMLTemp = XDocument.Parse(XMLLastUpdateTime.ToString) ' TJS 19/08/10
                                If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) = ChannelAdvSet.AccountID Then ' TJS 19/08/10
                                    strTemp = GetXMLElementText(XMLTemp, "ChannelAdvisor/LastOrderStatusUpdate") ' TJS 14/02/12
                                    If strTemp <> "" Then ' TJS 14/02/12
                                        ChannelAdvSet.LastOrderStatusUpdate = ConvertXMLDate(strTemp) ' TJS 19/08/10
                                    End If
                                    Exit For ' TJS 19/08/10
                                End If
                            Next
                        End If

                        SourceSet.AddChannelAdvSettings(ChannelAdvSet)
                        ' need web input 
                        WebIORequired = True
                    End If
                Next
                ' end of code added TJS 10/12/09

                ' start of code added TJS 19/08/10
            Case MAGENTO_SOURCE_CODE
                XMLLastUpdateTimes = Nothing ' TJS 14/02/12
                XMLLastUpdateTimeList = Nothing ' TJS 14/02/12
                ' have Last Order Status Update Date/Times been set ?
                If Not SourceConfig.IsLastOrderStatusUpdate_DEV000221Null Then
                    If "" & SourceConfig.LastOrderStatusUpdate_DEV000221 <> "" Then
                        ' yes, load values
                        XMLLastUpdateTimes = XDocument.Parse(SourceConfig.LastOrderStatusUpdate_DEV000221)
                        XMLLastUpdateTimeList = XMLLastUpdateTimes.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                    End If
                End If
                ' start of code added TJS 13/11/13
                XMLLastDailyTaskTimes = Nothing
                XMLLastDailyTaskRunList = Nothing
                ' have Last Daily Tasks Run Dates been set ?
                If Not SourceConfig.IsLastDailyTasksRun_DEV000221Null Then
                    If "" & SourceConfig.LastDailyTasksRun_DEV000221 <> "" Then
                        ' yes, load values
                        XMLLastDailyTaskTimes = XDocument.Parse(SourceConfig.LastDailyTasksRun_DEV000221)
                        XMLLastDailyTaskRunList = XMLLastDailyTaskTimes.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                    End If
                End If
                ' end of code added TJS 13/11/13

                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load Magento settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then
                        ' yes, get Magento Settings for source
                        MagentoSet = New MagentoSettings

                        MagentoSet.InstanceID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) ' TJS 18/03/11
                        MagentoSet.APIURL = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_URL)) ' TJS 02/10/13
                        MagentoSet.V2SoapAPIWSICompliant = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False)) ' TJS 13/11/13
                        MagentoSet.APIUser = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_USER)) ' TJS 02/10/13
                        MagentoSet.APIPwd = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD)) ' TJS 02/10/13
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ORDER_POLL_INTERVAL_MINUTES) <> "" Then ' TJS 19/03/12
                            MagentoSet.OrderPollIntervalMinutes = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ORDER_POLL_INTERVAL_MINUTES))
                        Else
                            MagentoSet.OrderPollIntervalMinutes = 15 ' TJS 19/03/12
                        End If
                        MagentoSet.MagentoVersion = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_VERSION) ' TJS 02/10/13 TJS 13/11/13
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION) <> "" Then ' TJS 13/11/13
                            MagentoSet.LerrynAPIVersion = CDec(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION)) ' TJS 13/11/13
                        Else
                            MagentoSet.LerrynAPIVersion = 0 ' TJS 02/10/13
                        End If
                        MagentoSet.APISupportsPartialShipments = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_API_SUPPORTS_PARTIAL_SHIP)) = "YES", True, False))
                        MagentoSet.CardAuthAndCaptureWithOrder = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_CARD_AUTH_CAPTURE_ON_ORDER)) = "YES", True, False)) ' TJS 01/05/14
                        MagentoSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_IS_ITEM_ID_FIELD)
                        MagentoSet.PricesAreTaxInclusive = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_PRICES_ARE_TAX_INCLUSIVE)) = "YES", True, False))
                        MagentoSet.TaxCodeForSourceTax = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_TAX_CODE_FOR_SOURCE_TAX) ' TJS 26/10/11
                        MagentoSet.EnablePaymentTypeTranslation = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ENABLE_PAYMENT_TYPE_TRANSLATION)) = "YES", True, False)) ' TJS 10/06/12
                        MagentoSet.AllowShippingLastNameBlank = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ALLOW_SHIPPING_LAST_NAME_BLANK)) = "YES", True, False))
                        MagentoSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ACCOUNT_DISABLED)) = "YES", True, False))
                        MagentoSet.EnableSKUAliasLookup = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ENABLE_SKU_ALIAS_LOOKUP)) = "YES", True, False))
                        MagentoSet.SplitSKUSeparatorCharacters = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_SPLIT_SKU_SEPARATOR_CHARACTERS) ' TJS 20/04/12
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_PRODUCT_LIST_BLOCK_SIZE) <> "" Then ' TJS 19/03/12
                            MagentoSet.ProductListBlockSize = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_PRODUCT_LIST_BLOCK_SIZE)) ' TJS 14/02/12
                        Else
                            MagentoSet.ProductListBlockSize = 10000 ' TJS 19/03/12
                        End If
                        MagentoSet.InhibitInventoryUpdates = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INHIBIT_INVENTORY_UPDATES)) = "YES", True, False)) ' TJS 30/04/13
                        MagentoSet.CreateCustomerForGuestCheckout = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_CREATE_GUEST_CUSTOMERS)) = "YES", True, False)) ' TJS 30/04/13
                        MagentoSet.IncludeChildItemsOnOrder = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INCLUDE_CHILD_ITEMS_ON_ORDER)) = "YES", True, False)) ' TJS 08/05/13
                        MagentoSet.UpdateMagentoSpecialPricing = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_UPDATE_SPECIAL_PRICING)) = "YES", True, False)) ' TJS 13/11/13
                        MagentoSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_CUSTOM_SKU_PROCESSING) ' setting only visible for relevant customers
                        MagentoSet.ImportAllOrdersAsSingleCustomer = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_IMPORT_ALL_ORDERS_AS_SINGLE_CUSTOMER) ' TJS 01/05/14 - setting only visible for relevant customers
                        MagentoSet.OverrideMagentoPricesWith = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_OVERRIDE_MAGENTO_PRICES) ' TJS 01/05/14 - setting only visible for relevant customers
                        ' set starting poll time
                        MagentoSet.NextOrderPollTime = Date.Now.AddMinutes(1)
                        '  set default Last Order Status Update Date/Time value of 1 month ago
                        MagentoSet.LastOrderStatusUpdate = Date.Today.AddMonths(-1)
                        ' have Last Order Status Update Date/Times been set ?
                        If XMLLastUpdateTimes IsNot Nothing Then
                            ' yes, find relevant entry
                            For Each XMLLastUpdateTime In XMLLastUpdateTimeList
                                XMLTemp = XDocument.Parse(XMLLastUpdateTime.ToString)
                                If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoSet.InstanceID Then ' TJS 18/03/11
                                    strTemp = GetXMLElementText(XMLTemp, "Magento/LastOrderStatusUpdate") ' TJS 14/02/12
                                    If strTemp <> "" Then ' TJS 14/02/12
                                        MagentoSet.LastOrderStatusUpdate = ConvertXMLDate(strTemp) ' TJS 19/07/10
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                        ' start of code added TJS 13/11/13
                        ' have Last Daily Tasks Run Dates been set ?
                        If XMLLastDailyTaskTimes IsNot Nothing Then
                            ' yes, find relevant entry
                            For Each XMLLastDailyTaskTime In XMLLastDailyTaskRunList ' TJS 20/11/13
                                XMLTemp = XDocument.Parse(XMLLastDailyTaskTime.ToString)
                                If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) = MagentoSet.InstanceID Then
                                    strTemp = GetXMLElementText(XMLTemp, "Magento/LastDailyTasksRun")
                                    If strTemp <> "" Then
                                        MagentoSet.LastDailyTasksRun = ConvertXMLDate(strTemp)
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                        ' end of code added TJS 13/11/13
                        SourceSet.AddMagentoSettings(MagentoSet)
                        ' need web input 
                        WebIORequired = True
                    End If
                Next

            Case ASP_STORE_FRONT_SOURCE_CODE
                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load ASPStoreFront settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then
                        ' yes, get AspDotNetStorefront Settings for source
                        ASPStoreFrontSet = New ASPStoreFrontSettings

                        ASPStoreFrontSet.SiteID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID)
                        ASPStoreFrontSet.UseWSE3Authentication = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_USE_WSE3_AUTHENTICATION)) = "YES", True, False))
                        ASPStoreFrontSet.APIURL = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_URL)) ' TJS 02/10/13
                        ASPStoreFrontSet.APIUser = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_USER)) ' TJS 02/10/13
                        ASPStoreFrontSet.APIPwd = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_API_PASSWORD)) ' TJS 02/10/13
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_ORDER_POLL_INTERVAL_MINUTES) <> "" Then ' TJS 19/03/12
                            ASPStoreFrontSet.OrderPollIntervalMinutes = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_ORDER_POLL_INTERVAL_MINUTES))
                        Else
                            ASPStoreFrontSet.OrderPollIntervalMinutes = 15 ' TJS 19/03/12
                        End If
                        ASPStoreFrontSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_IS_ITEM_ID_FIELD)
                        ASPStoreFrontSet.Currency = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_CURRENCY)
                        ASPStoreFrontSet.AllowShippingLastNameBlank = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_ALLOW_SHIPPING_LAST_NAME_BLANK)) = "YES", True, False))
                        ASPStoreFrontSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_ACCOUNT_DISABLED)) = "YES", True, False))
                        ASPStoreFrontSet.EnableSKUAliasLookup = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_ENABLE_SKU_ALIAS_LOOKUP)) = "YES", True, False))
                        ASPStoreFrontSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_CUSTOM_SKU_PROCESSING) ' setting only visible for relevant customers
                        ASPStoreFrontSet.ExtensionDataField1Mapping = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_1_MAPPING) ' TJS 24/02/12
                        ASPStoreFrontSet.ExtensionDataField2Mapping = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_2_MAPPING) ' TJS 24/02/12
                        ASPStoreFrontSet.ExtensionDataField3Mapping = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_3_MAPPING) ' TJS 24/02/12
                        ASPStoreFrontSet.ExtensionDataField4Mapping = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_4_MAPPING) ' TJS 24/02/12
                        ASPStoreFrontSet.ExtensionDataField5Mapping = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_EXTENSION_DATA_FIELD_5_MAPPING) ' TJS 24/02/12
                        ' set starting poll time
                        ASPStoreFrontSet.NextOrderPollTime = Date.Now.AddMinutes(1)

                        SourceSet.AddASPStoreFrontSettings(ASPStoreFrontSet)
                        ' need web input 
                        WebIORequired = True
                    End If
                Next
                ' end of code added TJS 19/08/10


                ' start of code added TJS 02/12/11
            Case EBAY_SOURCE_CODE
                XMLLastUpdateTimes = Nothing ' TJS 14/02/12
                XMLLastUpdateTimeList = Nothing ' TJS 14/02/12
                ' have Last Order Status Update Date/Times been set ?
                If Not SourceConfig.IsLastOrderStatusUpdate_DEV000221Null Then
                    If "" & SourceConfig.LastOrderStatusUpdate_DEV000221 <> "" Then
                        ' yes, load values
                        XMLLastUpdateTimes = XDocument.Parse(SourceConfig.LastOrderStatusUpdate_DEV000221)
                        XMLLastUpdateTimeList = XMLLastUpdateTimes.XPathSelectElements(SOURCE_CONFIG_XML_EBAY_LIST)
                    End If
                End If

                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_EBAY_LIST)
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load eBay settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then
                        ' yes, get eBay Settings for source
                        eBaySet = New eBaySettings

                        eBaySet.SiteID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_SITE_ID)
                        If GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID") <> "" Then
                            eBaySet.eBayCountry = CInt(GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID"))
                        Else
                            eBaySet.eBayCountry = -1
                        End If
                        eBaySet.AuthToken = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN)
                        strTemp = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN_EXPIRES) ' TJS 14/02/12
                        If strTemp <> "" Then ' TJS 14/02/12
                            eBaySet.TokenExpires = ConvertXMLDate(strTemp)
                        Else
                            eBaySet.TokenExpires = Date.Now.AddDays(-1) ' TJS 14/02/12
                        End If
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ORDER_POLL_INTERVAL_MINUTES) <> "" Then ' TJS 19/03/12
                            eBaySet.OrderPollIntervalMinutes = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ORDER_POLL_INTERVAL_MINUTES))
                        Else
                            eBaySet.OrderPollIntervalMinutes = 15 ' TJS 19/03/12
                        End If
                        eBaySet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_IS_ITEM_ID_FIELD)
                        eBaySet.PricesAreTaxInclusive = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_PRICES_ARE_TAX_INCLUSIVE)) = "YES", True, False)) ' TJS 30/07/13
                        eBaySet.TaxCodeForSourceTax = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_TAX_CODE_FOR_SOURCE_TAX) ' TJS 30/07/13
                        eBaySet.PaymentType = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_PAYMENT_TYPE)
                        eBaySet.EnablePaymentTypeTranslation = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ENABLE_PAYMENT_TYPE_TRANSLATION)) = "YES", True, False))
                        eBaySet.AllowShippingLastNameBlank = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ALLOW_SHIPPING_LAST_NAME_BLANK)) = "YES", True, False))
                        eBaySet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ACCOUNT_DISABLED)) = "YES", True, False))
                        eBaySet.ActionIfNoPayment = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ACTION_IF_NO_PMT)
                        eBaySet.EnableSKUAliasLookup = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ENABLE_SKU_ALIAS_LOOKUP)) = "YES", True, False))
                        eBaySet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_CUSTOM_SKU_PROCESSING) ' setting only visible for relevant customers
                        ' set starting poll time
                        eBaySet.NextOrderPollTime = Date.Now.AddMinutes(1)
                        '  set default Last Order Status Update Date/Time value of 30 days ago as eBay won't allow a date range of more then 30 days
                        eBaySet.LastOrderStatusUpdate = Date.Today.AddDays(-30)
                        ' have Last Order/Payment Status Update Date/Times been set ?
                        If XMLLastUpdateTimes IsNot Nothing Then
                            ' yes, find relevant entry
                            For Each XMLLastUpdateTime In XMLLastUpdateTimeList
                                XMLTemp = XDocument.Parse(XMLLastUpdateTime.ToString)
                                If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_SITE_ID) = eBaySet.SiteID Then
                                    strTemp = GetXMLElementText(XMLTemp, "eBay/LastOrderStatusUpdate") ' TJS 14/02/12
                                    If strTemp <> "" Then ' TJS 14/02/12
                                        eBaySet.LastOrderStatusUpdate = ConvertXMLDate(strTemp) ' TJS 19/08/10
                                    End If
                                    Exit For
                                End If
                            Next
                        End If

                        SourceSet.AddeBaySettings(eBaySet)
                        ' need web input 
                        WebIORequired = True
                    End If
                Next
                ' end of code added TJS 02/12/11

                ' start of code added TJS 16/01/12
            Case SEARS_COM_SOURCE_CODE
                XMLLastUpdateTimes = Nothing ' TJS 14/02/12
                XMLLastUpdateTimeList = Nothing ' TJS 14/02/12
                ' have Last Order Status Update Date/Times been set ?
                If Not SourceConfig.IsLastOrderStatusUpdate_DEV000221Null Then
                    If "" & SourceConfig.LastOrderStatusUpdate_DEV000221 <> "" Then
                        ' yes, load values
                        XMLLastUpdateTimes = XDocument.Parse(SourceConfig.LastOrderStatusUpdate_DEV000221)
                        XMLLastUpdateTimeList = XMLLastUpdateTimes.XPathSelectElements(SOURCE_CONFIG_XML_SEARSDOTCOM_LIST) ' TJS 17/02/12
                    End If
                End If

                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_SEARSDOTCOM_LIST)
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load Sears.com settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString ' TJS 05/07/12
                        bXMLError = True
                        bReturnValue = False ' TJS 05/07/12

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then
                        ' yes, get Sears.com Settings for source
                        SearsSet = New SearsComSettings

                        SearsSet.SiteID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_SITE_ID)
                        SearsSet.APIUser = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_API_USER)) ' TJS 02/10/13
                        SearsSet.APIPwd = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_API_PASSWORD)) ' TJS 02/10/13
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_ORDER_POLL_INTERVAL_MINUTES) <> "" Then ' TJS 19/03/12
                            SearsSet.OrderPollIntervalMinutes = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_ORDER_POLL_INTERVAL_MINUTES))
                        Else
                            SearsSet.OrderPollIntervalMinutes = 15 ' TJS 19/03/12
                        End If
                        SearsSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_IS_ITEM_ID_FIELD)
                        SearsSet.PricesAreTaxInclusive = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_PRICES_ARE_TAX_INCLUSIVE)) = "YES", True, False))
                        SearsSet.TaxCodeForSourceTax = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_TAX_CODE_FOR_SOURCE_TAX)
                        SearsSet.Currency = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_CURRENCY)
                        SearsSet.PaymentType = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_PAYMENT_TYPE)
                        SearsSet.SearsGeneratesInvoice = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_SEARS_INVOICING)) = "YES", True, False))
                        SearsSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_ACCOUNT_DISABLED)) = "YES", True, False))
                        SearsSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_CUSTOM_SKU_PROCESSING) ' setting only visible for relevant customers
                        ' set starting poll time
                        SearsSet.NextOrderPollTime = Date.Now.AddMinutes(1)
                        '  set default Last Order Status Update Date/Time value of 1/1/1900 so we can detect this and look for new orders first time round
                        SearsSet.LastOrderStatusUpdate = DateSerial(1900, 1, 1)
                        ' have Last Order/Payment Status Update Date/Times been set ?
                        If XMLLastUpdateTimes IsNot Nothing Then
                            ' yes, find relevant entry
                            For Each XMLLastUpdateTime In XMLLastUpdateTimeList
                                XMLTemp = XDocument.Parse(XMLLastUpdateTime.ToString)
                                If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_SEARSDOTCOM_SITE_ID) = SearsSet.SiteID Then ' TJS 17/02/12
                                    strTemp = GetXMLElementText(XMLTemp, "SearsDotCom/LastOrderStatusUpdate") ' TJS 14/02/12
                                    If strTemp <> "" Then ' TJS 14/02/12
                                        SearsSet.LastOrderStatusUpdate = ConvertXMLDate(strTemp)
                                    End If
                                    Exit For
                                End If
                            Next
                        End If

                        SourceSet.AddSearsComSettings(SearsSet)
                        ' need web input 
                        WebIORequired = True
                    End If
                Next
                ' end of code added TJS 16/01/12

                ' start of code added TJS 20/11/13
            Case THREE_D_CART_SOURCE_CODE
                XMLLastUpdateTimes = Nothing
                XMLLastUpdateTimeList = Nothing
                ' have Last Order Status Update Date/Times been set ?
                If Not SourceConfig.IsLastOrderStatusUpdate_DEV000221Null Then
                    If "" & SourceConfig.LastOrderStatusUpdate_DEV000221 <> "" Then
                        ' yes, load values
                        XMLLastUpdateTimes = XDocument.Parse(SourceConfig.LastOrderStatusUpdate_DEV000221)
                        XMLLastUpdateTimeList = XMLLastUpdateTimes.XPathSelectElements(SOURCE_CONFIG_XML_3DCART_LIST)
                    End If
                End If

                XMLMerchantIDs = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_3DCART_LIST)
                For Each XMLMerchantID In XMLMerchantIDs
                    bXMLError = False
                    Try
                        XMLTemp = XDocument.Parse(XMLMerchantID.ToString)

                    Catch ex As Exception
                        ErrorDetails = "Failed to load 3DCart settings due to config XML error, reason - " & ex.Message.Replace(vbCrLf, "") & ", XML section is " & XMLMerchantID.ToString
                        bXMLError = True
                        bReturnValue = False

                    End Try
                    ' did config XML load correctly
                    If Not bXMLError Then
                        ' yes, get 3DCart Settings for source
                        ThreeDCartSet = New ThreeDCartSettings

                        ThreeDCartSet.StoreID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_STORE_ID)
                        ThreeDCartSet.StoreURL = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_STORE_URL))
                        ThreeDCartSet.UserKey = DecodeConfigXMLValue(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_USER_KEY))
                        If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_ORDER_POLL_INTERVAL_MINUTES) <> "" Then
                            ThreeDCartSet.OrderPollIntervalMinutes = CInt(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_ORDER_POLL_INTERVAL_MINUTES))
                        Else
                            ThreeDCartSet.OrderPollIntervalMinutes = 15
                        End If
                        ThreeDCartSet.Currency = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_CURRENCY)
                        ThreeDCartSet.ISItemIDField = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_IS_ITEM_ID_FIELD)
                        ThreeDCartSet.EnablePaymentTypeTranslation = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_ENABLE_PAYMENT_TYPE_TRANSLATION)) = "YES", True, False))
                        ThreeDCartSet.AllowShippingLastNameBlank = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_ALLOW_SHIPPING_LAST_NAME_BLANK)) = "YES", True, False))
                        ThreeDCartSet.EnableSKUAliasLookup = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_ENABLE_SKU_ALIAS_LOOKUP)) = "YES", True, False))
                        ThreeDCartSet.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_ACCOUNT_DISABLED)) = "YES", True, False))
                        ThreeDCartSet.CustomSKUProcessing = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_CUSTOM_SKU_PROCESSING) ' setting only visible for relevant customers
                        ' set starting poll time
                        ThreeDCartSet.NextOrderPollTime = Date.Now.AddMinutes(1)
                        '  set default Last Order Status Update Date/Time value of 1 month ago
                        ThreeDCartSet.LastOrderStatusUpdate = Date.Today.AddMonths(-1)
                        ' have Last Order Status Update Date/Times been set ?
                        If XMLLastUpdateTimes IsNot Nothing Then
                            ' yes, find relevant entry
                            For Each XMLLastUpdateTime In XMLLastUpdateTimeList
                                XMLTemp = XDocument.Parse(XMLLastUpdateTime.ToString)
                                If GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_3DCART_STORE_ID) = ThreeDCartSet.StoreID Then
                                    strTemp = GetXMLElementText(XMLTemp, "ThreeDCart/LastOrderStatusUpdate")
                                    If strTemp <> "" Then
                                        ThreeDCartSet.LastOrderStatusUpdate = ConvertXMLDate(strTemp)
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                        SourceSet.Add3DCartSettings(ThreeDCartSet)
                        ' need web input 
                        WebIORequired = True
                    End If
                Next
                ' end of code added TJS 20/11/13
        End Select
        Return bReturnValue ' TJS 05/07/12

    End Function
#End Region

#Region " GetXMLElementText "
    Public Function GetXMLElementText(ByVal XMLMessage As XElement, ByVal ElementName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/03/09 | TJS             | 2009.1.09 | Modified to only return anything when correctly activated
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If (m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated) Or m_ValidatingActivation Then ' TJS 11/03/09 TJS 13/04/10 TJS 17/02/12
            If XMLNSMan IsNot Nothing Then ' TJS 02/12/11
                Return GetElementText(XMLMessage, ElementName, XMLNSMan) ' TJS 02/12/11
            Else
                Return GetElementText(XMLMessage, ElementName)
            End If
        Else
            Return Nothing ' TJS 11/03/09
        End If

    End Function

    Public Function GetXMLElementText(ByVal XMLMessage As XDocument, ByVal ElementName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If (m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated) Or m_ValidatingActivation Then ' TJS 17/02/12
            If XMLNSMan IsNot Nothing Then
                Return GetElementText(XMLMessage, ElementName, XMLNSMan)
            Else
                Return GetElementText(XMLMessage, ElementName)
            End If
        Else
            Return Nothing
        End If

    End Function
#End Region

#Region " GetXMLElementListCount "
    Public Function GetXMLElementListCount(ByRef XMLElementList As System.Collections.Generic.IEnumerable(Of XElement)) As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If (m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated) Or m_ValidatingActivation Then ' TJS 17/02/12
            Return GetElementListCount(XMLElementList)
        Else
            Return 0
        End If

    End Function

    Public Function GetXMLElementListCount(ByRef XMLElementList As System.Collections.Generic.IEnumerable(Of XNode)) As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If (m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated) Or m_ValidatingActivation Then ' TJS 17/02/12
            Return GetElementListCount(XMLElementList)
        Else
            Return 0
        End If

    End Function
#End Region

#Region " GetXMLElementAttribute "
    Public Function GetXMLElementAttribute(ByVal XMLMessage As XElement, ByVal ElementName As String, ByVal AttributeName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/03/09 | TJS             | 2009.1.09 | Modified to only return anything when correctly activated
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 11/03/09 TJS 13/04/10 TJS 17/02/12
            If XMLNSMan IsNot Nothing Then ' TJS 02/12/11
                Return GetElementAttribute(XMLMessage, ElementName, AttributeName, XMLNSMan) ' TJS 02/12/11
            Else
                Return GetElementAttribute(XMLMessage, ElementName, AttributeName)
            End If
        Else
            Return Nothing ' TJS 11/03/09
        End If

    End Function

    Public Function GetXMLElementAttribute(ByVal XMLMessage As XDocument, ByVal ElementName As String, ByVal AttributeName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 17/02/12
            If XMLNSMan IsNot Nothing Then
                Return GetElementAttribute(XMLMessage, ElementName, AttributeName, XMLNSMan)
            Else
                Return GetElementAttribute(XMLMessage, ElementName, AttributeName)
            End If
        Else
            Return Nothing
        End If

    End Function
#End Region

#Region " ConvertXMLFromWeb "
    Public Function ConvertXMLFromWeb(ByVal InputString As String) As String

        Return ConvertCharsFromWeb(InputString)

    End Function
#End Region

#Region " ConvertFromXML "
    Public Function ConvertFromXML(ByVal InputString As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/05/09 | TJS             | 2009.2.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return ConvertEntitiesFromXML(InputString)

    End Function
#End Region

#Region " ConvertForXML "
    Public Function ConvertForXML(ByVal InputString As String) As String
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

        Return ConvertEntitiesForXML(InputString)

    End Function
#End Region

#Region " DecodeConfigXMLValue "
    Public Function DecodeConfigXMLValue(ByVal XMLValue As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/09/13 | TJS             | 2013.3.02 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim builder As New StringBuilder(), iLoop As Integer
        For iLoop = 1 To Len(XMLValue)
            If Mid(XMLValue, iLoop, 1) = "%" Then
                builder.Append(Chr(CInt("&H" & Mid(XMLValue, iLoop + 1, 2))))
                iLoop = iLoop + 2
            Else
                builder.Append(Mid(XMLValue, iLoop, 1))
            End If
        Next
        Return builder.ToString

    End Function
#End Region

#Region " ValidateXMLDate "
    Public Function ValidateXMLDate(ByVal XMLDateValue As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/01/09 | TJS             | 2009.1.09 | Function added
        ' 01/06/09 | TJS             | 2009.2.10 | Modified to check date within valid range
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String, dteTestDate As Date

        Try

            If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12
                sTemp = XMLDateValue.Trim
                ' try conveting XML date
                dteTestDate = DateSerial(CInt(sTemp.Substring(0, 4)), CInt(sTemp.Substring(5, 2)), CInt(sTemp.Substring(8, 2)))
                ' it worked, must be valid, now check date is within allowed range
                If dteTestDate >= DateSerial(1900, 1, 1) And dteTestDate <= DateSerial(Date.Today.Year + 50, 31, 12) Then ' TJS 01/06/09
                    Return True ' TJS 01/06/09
                Else
                    Return False ' TJS 01/06/09
                End If
            Else
                Return Nothing
            End If

        Catch ex As Exception
            ' didn't convert, not valid
            Return False

        End Try

    End Function
#End Region

#Region " ValidateInventoryAmazonDetails "
    Public Function ValidateInventoryAmazonDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/05/09 | TJS             | 2009.2.08 | Function added
        ' 14/06/09 | TJS             | 2009.3.00 | Modified to set error on BrowseTree_DEV000221 if BrowseNodeID_DEV000221 blank
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "SiteCode_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "BrowseNodeID_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                        row.SetColumnError("BrowseTree_DEV000221", "Must select a valid Amazon Browse Node") ' TJS 14/06/09
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                        row.SetColumnError("BrowseTree_DEV000221", "Must select a valid Amazon Browse Node") ' TJS 14/06/09
                    End If
                    Return False
                Else
                    If Not IsNumeric(row(columnName)) Then
                        row.SetColumnError(columnName, "Must be numeric")
                    ElseIf CInt(row(columnName)) < 0 Then
                        row.SetColumnError(columnName, "Must select a valid Amazon Browse Node")
                        row.SetColumnError("BrowseTree_DEV000221", "Must select a valid Amazon Browse Node")
                    End If
                End If

            Case "SellingPrice_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "SalePrice_DEV000221"
                ' has Item been marked as Sale Active ?
                If CBool(row("SalePriceActive_DEV000221")) Then
                    ' yes, must enter sale price
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    End If
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryAmazonTagDetails "
    Public Function ValidateInventoryAmazonTagDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.06 | Function added
        ' 29/09/09 | TJS             | 2009.3.07 | Added check for Boolean data type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "TagLocation_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf Not IsNumeric(row(columnName)) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Invalid value")
                    End If
                ElseIf CInt(row(columnName)) <= 0 Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Invalid value")
                    End If
                End If

            Case "TagName_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "TagDataType_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "TagDateValue_DEV000221"
                If row("TagDataType_DEV000221").ToString = "Date" Or row("TagDataType_DEV000221").ToString = "DateTime" Then
                    ' has a text value been entered
                    If Not row.IsNull("TagTextValue_DEV000221") Then
                        If row("TagTextValue_DEV000221").ToString <> "" Then
                            ' yes, is value a valid date ?
                        End If
                    End If
                End If

            Case "TagNumericValue_DEV000221"
                If row("TagDataType_DEV000221").ToString = "Integer" Or row("TagDataType_DEV000221").ToString = "Numeric" Then
                    ' has a text value been entered
                    If Not row.IsNull("TagTextValue_DEV000221") Then
                        If row("TagTextValue_DEV000221").ToString <> "" Then
                            ' yes, is value numeric ?
                            If Not IsNumeric(row("TagTextValue_DEV000221")) Then
                                If row.GetColumnError(columnName) = "" Then
                                    row.SetColumnError(columnName, "Must be numeric")
                                End If
                                Return False
                            ElseIf row("TagDataType_DEV000221").ToString = "Integer" Then
                                If CInt(row("TagTextValue_DEV000221")) <> CDec(row("TagTextValue_DEV000221")) Then
                                    If row.GetColumnError(columnName) = "" Then
                                        row.SetColumnError(columnName, "Must be an integer")
                                    End If
                                    Return False
                                End If
                            End If
                        End If
                    End If

                ElseIf row("TagDataType_DEV000221").ToString = "Boolean" Then ' TJS 29/09/09
                    ' has a text value been entered
                    If Not row.IsNull("TagTextValue_DEV000221") Then ' TJS 29/09/09
                        If row("TagTextValue_DEV000221").ToString <> "" Then ' TJS 29/09/09
                            ' yes, is value valid ?
                            If LCase(row("TagTextValue_DEV000221").ToString) <> "true" And LCase(row("TagTextValue_DEV000221").ToString) <> "false" Then ' TJS 29/09/09
                                If row.GetColumnError(columnName) = "" Then ' TJS 29/09/09
                                    row.SetColumnError(columnName, "Must be True or False") ' TJS 29/09/09
                                End If
                                Return False ' TJS 29/09/09
                            End If
                        End If
                    End If
                End If

            Case "TagTextValue_DEV000221"
                If row("SourceTagStatus_DEV000221").ToString.ToUpper = "REQUIRED" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    End If
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryAmazonSearchTerms "
    Public Function ValidateInventoryAmazonSearchTerms(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/05/09 | TJS             | 2009.2.08 | Function added
        ' 24/06/09 | TJS             | 2009.3.00 | Added validation for TagTextValue_DEV000221
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "TagLocation_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "TagName_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "TagTextValue_DEV000221"
                If row("SourceTagStatus_DEV000221").ToString.ToUpper = "REQUIRED" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    End If
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryASPStorefrontDetails "
    Public Function ValidateInventoryASPStorefrontDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for Storefront Inventory Quantity publishing
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "SellingPrice_DEV000221"
                If row.IsNull(columnName) And (row.IsNull("UseISPricingDetail_DEV000221") OrElse Not CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" And (row.IsNull("UseISPricingDetail_DEV000221") OrElse Not CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

                ' start of code added TJS 02/12/11
            Case "QtyPublishingType_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "QtyPublishingValue_DEV000221"
                If Not row.IsNull(columnName) AndAlso (row("QtyPublishingType_DEV000221").ToString = "Fixed" Or row("QtyPublishingType_DEV000221").ToString = "Percent") Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString.Trim = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False

                    ElseIf CInt(row(columnName)) < 0 Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be negative")
                        End If
                        Return False
                    ElseIf CInt(row(columnName)) > 100 And row("QtyPublishingType_DEV000221").ToString = "Percent" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be greater then 100%")
                        End If
                        Return False
                    End If
                End If
                ' end of code added TJS 02/12/11

        End Select

        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryASPStorefrontCategories "
    Public Function ValidateInventoryASPStorefrontCategories(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryASPStorefrontTagDetails "
    Public Function ValidateInventoryASPStorefrontTagDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryChannelAdvDetails "
    Public Function ValidateInventoryChannelAdvDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/05/12 | FA/TJS          | 2012.1.05 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "SellingPrice_DEV000221"
                If row.IsNull(columnName) And (row.IsNull("UseISPricingDetail_DEV000221") OrElse Not CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" And (row.IsNull("UseISPricingDetail_DEV000221") OrElse Not CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "QtyPublishingType_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "QtyPublishingValue_DEV000221"
                If row.IsNull(columnName) And (Not row.IsNull("QtyPublishingType_DEV000221") AndAlso _
                    (row("QtyPublishingType_DEV000221").ToString = "Fixed" Or _
                        row("QtyPublishingType_DEV000221").ToString = "Percent")) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" And (Not row.IsNull("QtyPublishingType_DEV000221") AndAlso _
                    (row("QtyPublishingType_DEV000221").ToString = "Fixed" Or _
                        row("QtyPublishingType_DEV000221").ToString = "Percent")) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryMagentoDetails "
    Public Function ValidateInventoryMagentoDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for Magento Inventory Quantity publishing
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for price source selectors
        ' 20/11/13 | TJS             | 2013.4.00 | Added AttributeSetID_DEV000221 checks
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "AttributeSetID_DEV000221"
                If row(columnName).ToString = "" Then
                    row.SetColumnError(columnName, "Must not be blank")
                ElseIf CInt(row(columnName)) < 0 Then
                    row.SetColumnError(columnName, "Must select an Attribute Set")
                End If

            Case "SellingPrice_DEV000221"
                If row.IsNull(columnName) And (row.IsNull("UseISPricingDetail_DEV000221") OrElse Not CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" And (row.IsNull("UseISPricingDetail_DEV000221") OrElse Not CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

                ' start of code added TJS 13/11/13
            Case "SellingPriceSource_DEV000221"
                If row.IsNull(columnName) And (Not row.IsNull("UseISPricingDetail_DEV000221") AndAlso CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" And (Not row.IsNull("UseISPricingDetail_DEV000221") AndAlso CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "SpecialPriceSource_DEV000221"
                If row.IsNull(columnName) And (Not row.IsNull("UseISPricingDetail_DEV000221") AndAlso CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" And (Not row.IsNull("UseISPricingDetail_DEV000221") AndAlso CBool(row("UseISPricingDetail_DEV000221"))) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

                ' end of code added TJS 13/11/13

                ' start of code added TJS 02/12/11
            Case "QtyPublishingType_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "QtyPublishingValue_DEV000221"
                If Not row.IsNull(columnName) AndAlso (row("QtyPublishingType_DEV000221").ToString = "Fixed" Or row("QtyPublishingType_DEV000221").ToString = "Percent") Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString.Trim = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf CInt(row(columnName)) < 0 Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be negative")
                        End If
                        Return False
                    ElseIf CInt(row(columnName)) > 100 And row("QtyPublishingType_DEV000221").ToString = "Percent" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be greater then 100%")
                        End If
                        Return False
                    End If
                End If
                ' end of code added TJS 02/12/11

        End Select

        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryMagentoCategories "
    Public Function ValidateInventoryMagentoCategories(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryMagentoTagDetails "
    Public Function ValidateInventoryMagentoTagDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to cater for TagRequired
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' start of code added TJS 09/12/13
        'Select Case columnName
        '    Case "TagTextValue_DEV000221"
        '        If Not row.IsNull("TagRequired_DEV000221") AndAlso CBool(row("TagRequired_DEV000221")) Then
        '            If row.IsNull(columnName) Then
        '                If row.GetColumnError(columnName) = "" Then
        '                    row.SetColumnError(columnName, "Must not be blank")
        '                    row.SetColumnError("TagName_DEV000221", "Must not be blank")
        '                End If
        '                Return False
        '            ElseIf row(columnName).ToString.Trim = "" Then
        '                If row.GetColumnError(columnName) = "" Then
        '                    row.SetColumnError(columnName, "Must not be blank")
        '                    row.SetColumnError("TagName_DEV000221", "Must not be blank")
        '                End If
        '                Return False
        '            End If
        '        End If
        'End Select
        ' end of code added TJS 09/12/13

        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryShopComDetails "
    Public Function ValidateInventoryShopComDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/06/09 | TJS             | 2009.2.10 | Function added
        ' 08/07/09 | TJS             | 2009.3.00 | function completed with correct field names
        ' 14/08/09 | TJS             | 2009.3.03 | Removed redundant SalePrice field and modified Keywords to remove check for Matrix Item
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName

            Case "AttributeCategory_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                        row.SetColumnError("BrowseTree_DEV000221", "Must select a valid Shop.com Department (Category)")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                        row.SetColumnError("BrowseTree_DEV000221", "Must select a valid Shop.com Department (Category)")
                    End If
                    Return False
                End If

            Case "FirstLevelDepartment_DEV000221"
                If row.IsNull(columnName) And Me.m_ImportExportDataset.InventoryItem(0).ItemType <> "Matrix Item" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" And Me.m_ImportExportDataset.InventoryItem(0).ItemType <> "Matrix Item" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "ImageURL_DEV000221"
                If row.IsNull(columnName) And Me.m_ImportExportDataset.InventoryItem(0).ItemType <> "Matrix Item" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" And Me.m_ImportExportDataset.InventoryItem(0).ItemType <> "Matrix Item" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "Keywords_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "SellingPrice_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateInventoryShopComTagDetails "
    Public Function ValidateInventoryShopComTagDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/06/09 | TJS             | 2009.2.10 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "TagLocation_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf Not IsNumeric(row(columnName)) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Invalid value")
                    End If
                ElseIf CInt(row(columnName)) <= 0 Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Invalid value")
                    End If
                End If

            Case "TagName_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "TagDataType_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "TagDateValue_DEV000221"
                If row("TagDataType_DEV000221").ToString = "Date" Or row("TagDataType_DEV000221").ToString = "DateTime" Then
                    ' has a text value been entered
                    If Not row.IsNull("TagTextValue_DEV000221") Then
                        If row("TagTextValue_DEV000221").ToString <> "" Then
                            ' yes, is value a valid date ?
                        End If
                    End If
                End If

            Case "TagNumericValue_DEV000221"
                If row("TagDataType_DEV000221").ToString = "Integer" Or row("TagDataType_DEV000221").ToString = "Numeric" Then
                    ' has a text value been entered
                    If Not row.IsNull("TagTextValue_DEV000221") Then
                        If row("TagTextValue_DEV000221").ToString <> "" Then
                            ' yes, is value numeric ?
                            If Not IsNumeric(row("TagTextValue_DEV000221")) Then
                                If row.GetColumnError(columnName) = "" Then
                                    row.SetColumnError(columnName, "Must be numeric")
                                End If
                                Return False
                            ElseIf row("TagDataType_DEV000221").ToString = "Integer" Then
                                If CInt(row("TagTextValue_DEV000221")) <> CDec(row("TagTextValue_DEV000221")) Then
                                    If row.GetColumnError(columnName) = "" Then
                                        row.SetColumnError(columnName, "Must be an integer")
                                    End If
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If

            Case "TagTextValue_DEV000221"
                If row("SourceTagStatus_DEV000221").ToString.ToUpper = "REQUIRED" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    End If
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateLerrynImportExportDeliveryMethods "
    Public Function ValidateLerrynImportExportDeliveryMethods(ByVal row As System.Data.DataRow, ByVal columnName As String, _
        ByVal BaseValidationState As Boolean, ByVal ChannelAdvCarriers() As ChannelAdvCarrier) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/12/09 | TJS             | 2009.3.09 | Function added
        ' 27/09/10 | TJS             | 2010.1.02 | Added ChannelAdvCarriers and checks for valid 
        '                                        | Channel Advisor Shipping Methods and Classes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to allow SourceDeliveryClass_DEV000221 to be 
        '                                        | an empty string for sources other than Channel Advisor 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowGroupMethodDetail As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemShippingMethodGroupDetailRow
        Dim ChanlAdvCarrier As ChannelAdvCarrier, bValueIsValid As Boolean ' TJS 27/09/10

        Select Case columnName
            Case "SourceCode_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "ShippingMethodGroup_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                Else
                    rowGroupMethodDetail = Me.m_ImportExportDataset.SystemShippingMethodGroupDetail.FindByShippingMethodGroupShippingMethodCode(row(columnName).ToString.Trim, row("ShippingMethodCode_DEV000221").ToString.Trim)
                    If rowGroupMethodDetail Is Nothing Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Shipping Method Group and Shipping Method Code combination")
                            row.SetColumnError("ShippingMethodCode_DEV000221", "Not a valid Shipping Method Group and Shipping Method Code combination")
                        End If
                        Return False
                    End If
                End If

            Case "ShippingMethodCode_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                Else
                    rowGroupMethodDetail = Me.m_ImportExportDataset.SystemShippingMethodGroupDetail.FindByShippingMethodGroupShippingMethodCode(row("ShippingMethodGroup_DEV000221").ToString.Trim, row(columnName).ToString.Trim)
                    If rowGroupMethodDetail Is Nothing Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Shipping Method Group and Shipping Method Code combination")
                            row.SetColumnError("ShippingMethodGroup_DEV000221", "Not a valid Shipping Method Group and Shipping Method Code combination")
                        End If
                        Return False
                    End If
                End If

                ' start of code added TJS 27/09/10
            Case "SourceDeliveryMethod_DEV000221"
                ' is source Channel Advisor ?
                If row("SourceCode_DEV000221").ToString = "ChanAdvOrder" Then
                    ' yes, check Source Delivery Method is valid Carrier for Channel Advisor
                    bValueIsValid = False
                    For Each ChanlAdvCarrier In ChannelAdvCarriers
                        If ChanlAdvCarrier.CarrierName = row(columnName).ToString Then
                            bValueIsValid = True
                            Exit For
                        End If
                    Next
                    If Not bValueIsValid Then
                        row.SetColumnError(columnName, "Not a valid Source Delivery Method")
                        If row.GetColumnError("SourceDeliveryMethodCode_DEV000221") = "" Then
                            row.SetColumnError("SourceDeliveryMethodCode_DEV000221", "Not a valid Source Delivery Method")
                        End If
                    End If
                    ' if Source Delivery Method is valid, then check Carrier/Class combination is valid
                    If bValueIsValid Then
                        bValueIsValid = False
                        For Each ChanlAdvCarrier In ChannelAdvCarriers
                            If ChanlAdvCarrier.CarrierName = row(columnName).ToString And _
                                ChanlAdvCarrier.CarrierCode = row("SourceDeliveryMethodCode_DEV000221").ToString And _
                                ChanlAdvCarrier.ClassName = row("SourceDeliveryClass_DEV000221").ToString And _
                                ChanlAdvCarrier.ClassCode = row("SourceDeliveryClassCode_DEV000221").ToString Then
                                bValueIsValid = True
                                Exit For
                            End If
                        Next
                        If Not bValueIsValid And row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Source Delivery Method/Class combination")
                            If row.GetColumnError("SourceDeliveryMethodCode_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryMethodCode_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryClass_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryClass_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryClassCode_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryClassCode_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                        End If
                    End If
                End If

            Case "SourceDeliveryMethodCode_DEV000221"
                ' is source Channel Advisor ?
                If row("SourceCode_DEV000221").ToString = "ChanAdvOrder" Then
                    ' yes, check Source Delivery Method is valid Carrier for Channel Advisor
                    bValueIsValid = False
                    For Each ChanlAdvCarrier In ChannelAdvCarriers
                        If ChanlAdvCarrier.CarrierCode = row(columnName).ToString Then
                            bValueIsValid = True
                            Exit For
                        End If
                    Next
                    If Not bValueIsValid Then
                        row.SetColumnError(columnName, "Not a valid Source Delivery Method")
                        If row.GetColumnError("SourceDeliveryMethodCode_DEV000221") = "" Then
                            row.SetColumnError("SourceDeliveryMethodCode_DEV000221", "Not a valid Source Delivery Method")
                        End If
                    End If
                    ' if Source Delivery Method is valid, then check Carrier/Class combination is valid
                    If bValueIsValid Then
                        bValueIsValid = False
                        For Each ChanlAdvCarrier In ChannelAdvCarriers
                            If ChanlAdvCarrier.CarrierName = row("SourceDeliveryMethod_DEV000221").ToString And _
                                ChanlAdvCarrier.CarrierCode = row(columnName).ToString And _
                                ChanlAdvCarrier.ClassName = row("SourceDeliveryClass_DEV000221").ToString And _
                                ChanlAdvCarrier.ClassCode = row("SourceDeliveryClassCode_DEV000221").ToString Then
                                bValueIsValid = True
                                Exit For
                            End If
                        Next
                        If Not bValueIsValid And row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Source Delivery Method/Class combination")
                            If row.GetColumnError("SourceDeliveryMethod_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryMethod_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryClass_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryClass_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryClassCode_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryClassCode_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                        End If
                    End If
                End If

            Case "SourceDeliveryClass_DEV000221"
                ' is source Channel Advisor ?
                If row("SourceCode_DEV000221").ToString = "ChanAdvOrder" Then
                    ' yes, check Source Delivery Class is valid Class for Channel Advisor
                    bValueIsValid = False
                    For Each ChanlAdvCarrier In ChannelAdvCarriers
                        If ChanlAdvCarrier.ClassName = row(columnName).ToString Then
                            bValueIsValid = True
                            Exit For
                        End If
                    Next
                    If Not bValueIsValid Then
                        row.SetColumnError(columnName, "Not a valid Source Delivery Class")
                        If row.GetColumnError("SourceDeliveryClassCode_DEV000221") = "" Then
                            row.SetColumnError("SourceDeliveryClassCode_DEV000221", "Not a valid Source Delivery Class")
                        End If
                    End If
                    ' if Source Delivery Class is valid, then check Carrier/Class combination is valid
                    If bValueIsValid Then
                        bValueIsValid = False
                        For Each ChanlAdvCarrier In ChannelAdvCarriers
                            If ChanlAdvCarrier.CarrierName = row("SourceDeliveryMethod_DEV000221").ToString And _
                                ChanlAdvCarrier.CarrierCode = row("SourceDeliveryMethodCode_DEV000221").ToString And _
                                ChanlAdvCarrier.ClassName = row(columnName).ToString And _
                                ChanlAdvCarrier.ClassCode = row("SourceDeliveryClassCode_DEV000221").ToString Then
                                bValueIsValid = True
                                Exit For
                            End If
                        Next
                        If Not bValueIsValid And row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Source Delivery Method/Class combination")
                            If row.GetColumnError("SourceDeliveryMethod_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryMethod_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryMethodCode_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryMethodCode_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryClassCode_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryClassCode_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                        End If
                    End If

                Else
                    ' for sources other than Channel Advisor, field can be an empty string but 
                    ' IS default validation rejects this so need to correct it
                    If Not row.IsNull(columnName) AndAlso row(columnName).ToString = "" Then ' TJS 02/12/11
                        BaseValidationState = True ' TJS 02/12/11
                        row.SetColumnError(columnName, "") ' TJS 02/12/11
                    End If
                End If

            Case "SourceDeliveryClassCode_DEV000221"
                ' is source Channel Advisor ?
                If row("SourceCode_DEV000221").ToString = "ChanAdvOrder" Then
                    ' yes, check Source Delivery Class is valid Class for Channel Advisor
                    bValueIsValid = False
                    For Each ChanlAdvCarrier In ChannelAdvCarriers
                        If ChanlAdvCarrier.ClassCode = row(columnName).ToString Then
                            bValueIsValid = True
                            Exit For
                        End If
                    Next
                    If Not bValueIsValid Then
                        row.SetColumnError(columnName, "Not a valid Source Delivery Class")
                        If row.GetColumnError("SourceDeliveryClass_DEV000221") = "" Then
                            row.SetColumnError("SourceDeliveryClass_DEV000221", "Not a valid Source Delivery Class")
                        End If
                    End If
                    ' if Source Delivery Class is valid, then check Carrier/Class combination is valid
                    If bValueIsValid Then
                        bValueIsValid = False
                        For Each ChanlAdvCarrier In ChannelAdvCarriers
                            If ChanlAdvCarrier.CarrierName = row("SourceDeliveryMethod_DEV000221").ToString And _
                                ChanlAdvCarrier.CarrierCode = row("SourceDeliveryMethodCode_DEV000221").ToString And _
                                ChanlAdvCarrier.ClassName = row("SourceDeliveryClass_DEV000221").ToString And _
                                ChanlAdvCarrier.ClassCode = row(columnName).ToString Then
                                bValueIsValid = True
                                Exit For
                            End If
                        Next
                        If Not bValueIsValid And row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Source Delivery Method/Class combination")
                            If row.GetColumnError("SourceDeliveryMethod_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryMethod_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryMethodCode_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryMethodCode_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                            If row.GetColumnError("SourceDeliveryClass_DEV000221") = "" Then
                                row.SetColumnError("SourceDeliveryClass_DEV000221", "Not a valid Source Delivery Method/Class combination")
                            End If
                        End If
                    End If
                End If
                ' end of code added TJS 27/09/10

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateLerrynImportExportPaymentTypes "
    Public Function ValidateLerrynImportExportPaymentTypes(ByVal row As System.Data.DataRow, ByVal columnName As String, _
        ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 27/09/10 | TJS             | 2010.1.02 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowGroupMethodDetail As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemShippingMethodGroupDetailRow

        Select Case columnName
            Case "SourceCode_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "ShippingMethodGroup_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                Else
                    rowGroupMethodDetail = Me.m_ImportExportDataset.SystemShippingMethodGroupDetail.FindByShippingMethodGroupShippingMethodCode(row(columnName).ToString.Trim, row("ShippingMethodCode_DEV000221").ToString.Trim)
                    If rowGroupMethodDetail Is Nothing Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Shipping Method Group and Shipping Method Code combination")
                            row.SetColumnError("ShippingMethodCode_DEV000221", "Not a valid Shipping Method Group and Shipping Method Code combination")
                        End If
                        Return False
                    End If
                End If

            Case "ShippingMethodCode_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                Else
                    rowGroupMethodDetail = Me.m_ImportExportDataset.SystemShippingMethodGroupDetail.FindByShippingMethodGroupShippingMethodCode(row("ShippingMethodGroup_DEV000221").ToString.Trim, row(columnName).ToString.Trim)
                    If rowGroupMethodDetail Is Nothing Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Not a valid Shipping Method Group and Shipping Method Code combination")
                            row.SetColumnError("ShippingMethodGroup_DEV000221", "Not a valid Shipping Method Group and Shipping Method Code combination")
                        End If
                        Return False
                    End If
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateLerrynImportExportSKUAliases "
    Public Function ValidateLerrynImportExportSKUAliases(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 27/09/10 | TJS             | 2010.1.02 | Function added
        ' 04/07/13 | TJS             | 2013.1.25 | Corrected column names and tests
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "SourceCode_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "SourceSKU_DEV000221" ' TJS 04/07/13
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString.Trim = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

            Case "ItemCode_DEV000221" ' TJS 03/07/13
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

                ' start of code added TJS 04/07/13
            Case "ItemName"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If
                ' end of code added TJS 04/07/13

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateActivationAccountDetails "
    Public Function ValidateActivationAccountDetails(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com connector settings
        ' 14/02/12 | TJS             | 2011.2.05 | Corrected validation of Sears.com activation count
        ' 20/08/13 | TJS             | 2013.2.04 | Modified to allow blank password as no longer used
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iTemp As Integer

        Select Case columnName
            Case "MagentoCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateMagento")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of API connections to activate")
                        End If
                        Return False
                    End If
                End If

            Case "ASPStorefrontCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateASPStorefront")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of website connections to activate")
                        End If
                        Return False
                    End If
                End If

            Case "VolusionCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateVolusion")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of website connections to activate")
                        End If
                        Return False
                    End If
                End If

            Case "AmazonCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateAmazon")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of Merchant IDs to activate")
                        End If
                        Return False
                    End If
                End If

            Case "ChanAdvCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateChanAdv")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of Account IDs to activate")
                        End If
                        Return False
                    End If
                End If

            Case "ShopComCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateShopCom")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of Catalog IDs to activate")
                        End If
                        Return False
                    End If
                End If

                ' start of code added TJS 02/12/11
            Case "EBayCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateEBay")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of API connections to activate")
                        End If
                        Return False
                    End If
                End If

                ' start of code added TJS 16/01/12
            Case "SearsComCount"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "0" And CBool(row("ActivateSearsCom")) Then ' TJS 14/02/12
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Please select the number of API connections to activate")
                        End If
                        Return False
                    End If
                End If
                ' end of code added TJS 16/01/12

            Case "PayMonthly"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) OrElse Not CBool(row(columnName)) Then
                        If row.IsNull("PayForXYears") OrElse Not CBool(row("PayForXYears")) Then
                            If row.GetColumnError(columnName) = "" Then
                                row.SetColumnError(columnName, "You must select a payment period")
                            End If
                            If row.GetColumnError("PayForXYears") = "" Then
                                row.SetColumnError("PayForXYears", "You must select a payment period")
                            End If
                        End If
                    End If
                End If

            Case "PayForXYears"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If row.IsNull(columnName) OrElse Not CBool(row(columnName)) Then
                        If row.IsNull("PayMonthly") OrElse Not CBool(row("PayMonthly")) Then
                            If row.GetColumnError(columnName) = "" Then
                                row.SetColumnError(columnName, "You must select a payment period")
                            End If
                            If row.GetColumnError("PayMonthly") = "" Then
                                row.SetColumnError("PayMonthly", "You must select a payment period")
                            End If
                        End If
                    End If
                End If

            Case "NoOfYears"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Activations" Then
                    If Not row.IsNull("PayForXYears") AndAlso CBool(row("PayForXYears")) Then
                        If row.IsNull(columnName) OrElse row(columnName).ToString = "" Then
                            If row.GetColumnError(columnName) = "" Then
                                row.SetColumnError(columnName, "You must select the number of years to purchase")
                            End If
                        End If
                    End If
                End If
                ' end of code added TJS 02/12/11

            Case "BillingFirstName", "BillingLastName", "Email", "BillingAddress", "BillingCity", "BillingPostalCode", _
                  "BillingCountry" ' TJS 20/08/13
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Billing" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    End If
                    If columnName = "Email" Then
                        iTemp = row(columnName).ToString.IndexOf("@")
                        If iTemp < 0 Then
                            row.SetColumnError(columnName, "Not a valid email address")
                            Return False
                        Else
                            iTemp = row(columnName).ToString.Substring(iTemp).IndexOf(".")
                            If iTemp < 0 Then
                                row.SetColumnError(columnName, "Not a valid email address")
                                Return False
                            End If
                        End If
                    End If
                End If

            Case "Password", "ConfirmPassword" ' TJS 20/08/13
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Billing" Then
                    If columnName = "Password" Then
                        If row(columnName).ToString.Length > 0 And row(columnName).ToString.Length < 6 Then ' TJS 20/08/13
                            row.SetColumnError(columnName, "Must be at least 6 characters")
                            Return False
                        End If
                    End If
                    If columnName = "ConfirmPassword" Then
                        If row(columnName).ToString <> row("Password").ToString Then
                            row.SetColumnError(columnName, "Your Confirm Password must be the same as your Password")
                            row.SetColumnError("Password", "Your Confirm Password must be the same as your Password")
                            Return False
                        End If
                    End If
                End If

            Case "ShippingFirstName", "ShippingLastName", "ShippingAddress", "ShippingCity", "ShippingPostalCode", "ShippingCountry"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Shipping" Then
                    If row.IsNull(columnName) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "" Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    End If
                End If

            Case "CardType", "CardNumber", "NameOnCard", "ExpiryMonth", "ExpiryYear", "CardSecurityCode"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Payment" Then
                    If row.IsNull(columnName) And Not CBool(row("CardUnchanged")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    ElseIf row(columnName).ToString = "" And Not CBool(row("CardUnchanged")) Then
                        If row.GetColumnError(columnName) = "" Then
                            row.SetColumnError(columnName, "Must not be blank")
                        End If
                        Return False
                    End If
                End If

            Case "StartMonth"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Payment" Then
                    If Not CBool(row("CardUnchanged")) Then
                        If row.IsNull(columnName) And row("StartYear").ToString <> "" Then
                            If row.GetColumnError(columnName) = "" Then
                                row.SetColumnError(columnName, "Start Date must be complete or all blank")
                                row.SetColumnError("StartYear", "Start Date must be complete or all blank")
                            End If
                            Return False
                        ElseIf row(columnName).ToString = "" And row("StartYear").ToString <> "" Then
                            If row.GetColumnError(columnName) = "" Then
                                row.SetColumnError(columnName, "Start Date must be complete or all blank")
                                row.SetColumnError("StartYear", "Start Date must be complete or all blank")
                            End If
                            Return False
                        End If
                    End If
                End If

            Case "StartYear"
                If m_AccountDetailsValidationStage = "All" Or m_AccountDetailsValidationStage = "Payment" Then
                    If Not CBool(row("CardUnchanged")) Then
                        If row.IsNull(columnName) And row("StartMonth").ToString <> "" Then
                            If row.GetColumnError(columnName) = "" Then
                                row.SetColumnError(columnName, "Start Date must be complete or all blank")
                                row.SetColumnError("StartMonth", "Start Date must be complete or all blank")
                            End If
                            Return False
                        ElseIf row(columnName).ToString = "" And row("StartMonth").ToString <> "" Then
                            If row.GetColumnError(columnName) = "" Then
                                row.SetColumnError(columnName, "Start Date must be complete or all blank")
                                row.SetColumnError("StartMonth", "Start Date must be complete or all blank")
                            End If
                            Return False
                        End If
                    End If
                End If

        End Select
        Return BaseValidationState

    End Function
#End Region

#Region " ValidateLerrynImportExportAmazonSettlement "
    Public Function ValidateLerrynImportExportAmazonSettlement(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case columnName
            Case "SourceCode_DEV000221"
                If row.IsNull(columnName) Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                ElseIf row(columnName).ToString = "" Then
                    If row.GetColumnError(columnName) = "" Then
                        row.SetColumnError(columnName, "Must not be blank")
                    End If
                    Return False
                End If

        End Select

    End Function
#End Region

#Region " ValidateLerrynImportExportAmazonSettlementDetail "
    Public Function ValidateLerrynImportExportAmazonSettlementDetail(ByVal row As System.Data.DataRow, ByVal columnName As String, ByVal BaseValidationState As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Function
#End Region

#Region " ConvertXMLDate "
    Public Function ConvertXMLDate(ByVal XMLDateValue As String) As Date
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS             | 2008.0.01 | Original 
        ' 24/01/09 | TJS             | 2009.1.00 | Modified to trim any leading or training spaces
        ' 11/03/09 | TJS             | 2009.1.09 | Moved from process rule and modified to check for activation
        ' 06/05/09 | TJS             | 2009.2.05 | Modified to cater for time as well as date
        ' 25/05/09 | TJS             | 2009.2.08 | Corrected processing of time elements
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for XML Dates with decimal fractions of seconds
        ' 15/11/10 | FA              | 2010.1.08 | Modified to cater for 1, 2 or 3 digit decimal fractions
        ' 25/11/10 | TJS             | 2010.1.09 | Corrected 1 and 2 digit millisecond values
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String, dteReturnValue As Date ' TJS 06/05/09

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 11/03/09 TJS 13/04/10 TJS 17/02/12
            sTemp = XMLDateValue.Trim ' TJS 24/01/09
            dteReturnValue = DateSerial(CInt(sTemp.Substring(0, 4)), CInt(sTemp.Substring(5, 2)), CInt(sTemp.Substring(8, 2))) ' TJS 24/01/09 TJS 06/05/09
            If sTemp.Length = 19 Or sTemp.Length = 21 Or sTemp.Length = 22 Or sTemp.Length = 23 Then ' TJS 06/05/09 TJS 19/08/10 'FA 15/11/10
                dteReturnValue = dteReturnValue.AddHours(CInt(sTemp.Substring(11, 2))) ' TJS 06/05/09 TJS 25/05/09
                dteReturnValue = dteReturnValue.AddMinutes(CInt(sTemp.Substring(14, 2))) ' TJS 06/05/09 TJS 25/05/09
                dteReturnValue = dteReturnValue.AddSeconds(CInt(sTemp.Substring(17, 2))) ' TJS 06/05/09 TJS 25/05/09
                Select Case sTemp.Length
                    Case 21
                        dteReturnValue = dteReturnValue.AddMilliseconds(CInt(sTemp.Substring(20, 1) & "00")) ' FA 15/11/10 TJS 25/11/10
                    Case 22
                        dteReturnValue = dteReturnValue.AddMilliseconds(CInt(sTemp.Substring(20, 2) & "0")) ' FA 15/11/10 TJS 25/11/10
                    Case 23
                        dteReturnValue = dteReturnValue.AddMilliseconds(CInt(sTemp.Substring(20, 3))) ' FA 15/11/10
                    Case Else
                        dteReturnValue = dteReturnValue.AddMilliseconds(0) ' FA 15/11/10
                End Select
                'If sTemp.Length = 23 Then ' TJS 19/08/10
                '    dteReturnValue = dteReturnValue.AddMilliseconds(CInt(sTemp.Substring(20, 3))) ' TJS 19/08/10
                'End If
            End If
            Return dteReturnValue ' TJS 06/05/09
        Else
            Return Nothing ' TJS 11/03/09
        End If

    End Function
#End Region

#Region " ConvertCRLF "
    Public Function ConvertCRLF(ByVal InputValue As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/03/09 | TJS             | 2009.1.09 | function added
        ' 08/04/09 | TJS             | 2009.2.02 | rewritten to correctly change CR and LF for Interprise
        ' 17/05/09 | TJS             | 2009.2.07 | corrected detection of CR before LF
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String, iCharPosn As Integer

        sTemp = InputValue
        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 13/04/10 TJS 17/02/12
            iCharPosn = sTemp.IndexOf(Chr(10))
            Do While iCharPosn >= 0
                If iCharPosn > 0 Then
                    If Asc(sTemp.Substring(iCharPosn - 1, 1)) <> 13 Then ' TJS 17/05/09
                        sTemp = Left(sTemp, iCharPosn) & ChrW(13) & ChrW(10) & Right(sTemp, Len(sTemp) - (iCharPosn + 1))
                    Else
                        sTemp = Left(sTemp, iCharPosn - 1) & ChrW(13) & ChrW(10) & Right(sTemp, Len(sTemp) - (iCharPosn + 1))
                    End If
                    iCharPosn = sTemp.IndexOf(Chr(10), iCharPosn + 2)
                Else
                    sTemp = ChrW(13) & ChrW(10) & Right(sTemp, Len(sTemp) - (iCharPosn + 1))
                    iCharPosn = sTemp.IndexOf(Chr(10), iCharPosn + 1)
                End If
            Loop
            Return sTemp
        Else
            Return Nothing
        End If

    End Function
#End Region

#Region " CheckRegistryValue "
    Public Function CheckRegistryValue(ByVal RegistryKey As String, ByVal ValueName As String, ByVal DefaultValue As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Checks the specified LocalMaching Registry Key for the specified ValueName
        '                    and if a value exists, return that value, otherwise is returns the DefaultVlaue
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 23/08/09 | TJS             | 2009.3.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rgISPluginKey As RegistryKey, strKeyNames As String(), strKeyName As String, strReturnValue As String

        strReturnValue = DefaultValue
        Try
            rgISPluginKey = Registry.LocalMachine.OpenSubKey(RegistryKey, False)
            If rgISPluginKey IsNot Nothing Then
                strKeyNames = rgISPluginKey.GetValueNames
                For Each strKeyName In strKeyNames
                    If strKeyName = ValueName Then
                        If rgISPluginKey.GetValue(ValueName).ToString <> "" Then
                            strReturnValue = rgISPluginKey.GetValue(ValueName).ToString
                        End If
                    End If
                Next
                rgISPluginKey.Close()
            End If

        Catch ex As Exception
            ' ignore errors as they could be permissions problems reading the Registry
        End Try

        Return strReturnValue

    End Function
#End Region

#Region " BuildXMLErrorResponseNodeAndEmail "
    Public Function BuildXMLErrorResponseNodeAndEmail(ByVal ResponseStatus As String, ByVal ErrorCode As String, ByVal ErrorMessage As String, _
        ByVal SourceConfig As XDocument, ByVal ProcedureName As String, ByVal XMLInput As String) As XElement ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/03/09 | TJS             | 2009.1.09 | function added
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to prevent operation over midnight causing errors on Expiry Date 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 17/02/12 | TJS             | 2011.2.07 | Modified to ensure data is read on day or expiry
        ' 10/06/12 | TJS             | 2012.1.14 | modified to cater for use of Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlResponseDetailNode As XElement

        If m_ExpiryDate >= Date.Today.AddHours(-2) And m_IsActivated Then ' TJS 11/03/09 TJS 13/04/10 TJS 17/02/12
            xmlResponseDetailNode = New XElement("ImportResponse") ' TJS 02/12/11
            xmlResponseDetailNode.Add(New XElement("Status", ResponseStatus)) ' TJS 02/12/11
            xmlResponseDetailNode.Add(New XElement("ErrorCode", ErrorCode)) ' TJS 02/12/11
            xmlResponseDetailNode.Add(New XElement("ErrorMessage", ErrorMessage)) ' TJS 02/12/11
            SendSourceErrorEmail(SourceConfig, ProcedureName, xmlResponseDetailNode.ToString, XMLInput) ' TJS 02/12/11 TJS 10/06/12

            Return xmlResponseDetailNode
        Else
            Return Nothing ' TJS 11/03/09
        End If

    End Function
#End Region

#Region " SendErrorEmail "
    Public Sub SendErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, ByVal ex As Exception, _
        Optional ByVal XMLSource As String = "")
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.14 | modified to use Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ErrorNotification.SendExceptionEmail(SourceConfig, ProcedureName, ex, XMLSource) ' TJS 18/03/11 TJS 10/06/12

    End Sub

    Public Sub SendErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, ByVal ErrorDetails As String, _
        Optional ByVal XMLSource As String = "") ' TJS 10/06/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/05/11 | TJS             | 2011.0.13 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.14 | modified to use Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ErrorNotification.SendExceptionEmail(SourceConfig, ProcedureName, ErrorDetails, XMLSource) ' TJS 18/03/11 TJS 10/06/12

    End Sub
#End Region

#Region " SendSourceErrorEmail "
    Public Sub SendSourceErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, ByVal Message As String, _
        Optional ByVal XMLSource As String = "")
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.14 | modified to use Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ErrorNotification.SendSrcErrorEmail(SourceConfig, ProcedureName, Message, XMLSource) ' TJS 18/03/11 TJS 10/06/12

    End Sub
#End Region

#Region " SendPaymentErrorEmail "
    Public Sub SendPaymentErrorEmail(ByVal SourceConfig As XDocument, ByVal Message As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Notifies key parties of payment failures
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/06/10 | TJS             | 2010.0.03 | Function added
        ' 07/01/11 | TJS             | 2010.1.15 | Modified to cater for changes to SendPmntErrorEmail
        ' 10/06/12 | TJS             | 2012.1.14 | modified to use Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ErrorNotification.SendPmntErrorEmail(SourceConfig, Message) ' TJS 07/01/11 TJS 10/06/12

    End Sub
#End Region

#Region " WriteLogProgressRecord "
    Public Sub WriteLogProgressRecord(ByVal Message As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 22/02/09 | TJS             | 2009.1.08 | Function renamed and message type removed to ensure 
        '                                        | all error messages are sent via email functions above
        ' 10/06/12 | TJS             | 2012.1.14 | Modified to use Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ErrorNotification.WriteToLogFileOrEvent(LOG_MESSAGE_TYPE_PROGRESS, Message) ' TJS 10/06/12

    End Sub
#End Region

#Region " ActivationFailure "
    Private Sub ActivationFailure(ByVal ConnectorCount As Integer, ByVal ConnectorName As String, ByVal AllowEqualMaxCount As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/09/08 | TJS/CG          | 2008.0.01 | Original 

        If Not m_IsActivated And Not m_HasBeenActivated Then
            Interprise.Presentation.Base.Message.MessageWindow.Show("You must activate " & m_BaseProductName & " first using the Activation function on the Utilities/Setup menu.")
        ElseIf m_ExpiryDate < Date.Today Then
            Interprise.Presentation.Base.Message.MessageWindow.Show("Your Activation for " & m_BaseProductName & " has expired." & vbCrLf & vbCrLf & "Please renew it using the Activation function on the Utilities/Setup menu before attempting to amend any data")
        ElseIf (ConnectorCount >= m_MaxAccounts And Not AllowEqualMaxCount) Or (ConnectorCount > m_MaxAccounts And AllowEqualMaxCount) Then
            Interprise.Presentation.Base.Message.MessageWindow.Show("Your Activation for the " & m_BaseProductName & " " & ConnectorName & " Connector only permits a maximum of " & m_MaxAccounts & " Accounts." & vbCrLf & vbCrLf & "Please upgrade it using the Activation function on the Utilities/Setup menu before attempting to amend any data")
        End If
    End Sub
#End Region
#End Region

End Class
#End Region
