' eShopCONNECT for Connected Business
' Module: ImportExportConfigFacade.vb
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
' Last Updated - 01 May 2014

Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports System.Data.Odbc ' TJS 12/06/13
Imports System.IO ' TJS 12/06/13
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11



#Region " ImportExportConfigFacade "
Public Class ImportExportConfigFacade
    Inherits Interprise.Facade.Base.BaseFacade
    Implements Interprise.Extendable.Base.Facade.IBaseInterface

#Region " Custom Events "
    ''' <summary>Occurs before Initial Config Download is initiated.</summary>
    Public Event BeforeInitialConfigDownload(ByVal sender As Object, ByVal WarningMessage As String)
#End Region

#Region " Variables "
    Private m_ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private WithEvents m_ImportExportRule As Lerryn.Facade.ImportExport.ImportExportFacade
    Private m_BaseProductCode As String
    Private m_ChannelAdvCarriers() As ChannelAdvCarrier
    Private m_ErrorNotification As Lerryn.Facade.ImportExport.ErrorNotification
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return m_ImportExportDataset
        End Get
    End Property
#End Region

#Region " CurrentBusinessRule "
    Public Overrides ReadOnly Property CurrentBusinessRule() As Interprise.Extendable.Base.Business.IBaseInterface
        Get
            Return m_ImportExportRule
        End Get
    End Property
#End Region

#Region " CurrentTransactionType "
    Public Overrides ReadOnly Property CurrentTransactionType() As Interprise.Framework.Base.Shared.Enum.TransactionType
        Get
            Return Nothing
        End Get
    End Property
#End Region

#Region " CurrentReportType "
    Public Overrides ReadOnly Property CurrentReportType() As Interprise.Framework.Base.Shared.Enum.ReportAction
        Get
            Return Nothing
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
        ' 11/03/09 | TJS             | 2009.1.10 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Get
            Return m_ImportExportRule.LatestActivationCode() ' TJS 11/03/09
        End Get
    End Property
#End Region

#Region " ConnectorLatestActivationCode "
    Public ReadOnly Property ConnectorLatestActivationCode(ByVal ConnectorProductCode As String) As String ' TJS 11/03/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/03/09 | TJS             | 2009.1.10 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Get
            Return m_ImportExportRule.ConnectorLatestActivationCode(ConnectorProductCode) ' TJS 11/03/09
        End Get
    End Property
#End Region

#Region " IsActivated "
    Public ReadOnly Property IsActivated() As Boolean
        Get
            Return m_ImportExportRule.IsActivated
        End Get
    End Property
#End Region

#Region " IsConnectorActivated "
    Public ReadOnly Property IsConnectorActivated(ByVal ConnectorProductCode As String) As Boolean
        Get
            Return m_ImportExportRule.IsConnectorActivated(ConnectorProductCode)
        End Get
    End Property
#End Region

#Region " HasBeenActivated "
    Public ReadOnly Property HasBeenActivated() As Boolean
        Get
            Return m_ImportExportRule.HasBeenActivated
        End Get
    End Property
#End Region

#Region " HasConnectorBeenActivated "
    Public ReadOnly Property HasConnectorBeenActivated(ByVal ConnectorProductCode As String) As Boolean
        Get
            Return m_ImportExportRule.HasConnectorBeenActivated(ConnectorProductCode)
        End Get
    End Property
#End Region

#Region " ActivationExpires "
    Public ReadOnly Property ActivationExpires() As Date
        Get
            Return m_ImportExportRule.ActivationExpires
        End Get
    End Property
#End Region

#Region " ConnectorActivationExpires "
    Public ReadOnly Property ConnectorActivationExpires(ByVal ConnectorProductCode As String) As Date
        Get
            Return m_ImportExportRule.ConnectorActivationExpires(ConnectorProductCode)
        End Get
    End Property
#End Region

#Region " ConnectorAccountLimit "
    Public ReadOnly Property ConnectorAccountLimit(ByVal ConnectorProductCode As String) As Integer
        Get
            Return m_ImportExportRule.ConnectorAccountLimit(ConnectorProductCode)
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
            Return m_ImportExportRule.ConnectorLastActivatedAccountLimit(ConnectorProductCode)
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
        '------------------------------------------------------------------------------------------

        Get
            Return m_ImportExportRule.InventoryImportLimit
        End Get
    End Property
#End Region

#Region " ConnectorProductCode "
    Public ReadOnly Property ConnectorProductCode(ByVal InputHandler As String) As String
        Get
            Return m_ImportExportRule.ConnectorProductCode(InputHandler)
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
        '------------------------------------------------------------------------------------------

        Get
            Return m_ImportExportRule.IsSystemConfigRecord(InputHandler) ' TJS 20/01/09
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
        ' 03/04/09 | TJS             | 2009.2.00 | Property added
        '------------------------------------------------------------------------------------------

        Get
            Return m_ImportExportRule.IsFullActivation
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
            Return m_ImportExportRule.IsConnectorFullActivation(ConnectorProductCode)
        End Get
    End Property
#End Region

#Region " SourceConfig "
    Public ReadOnly Property SourceConfig() As XDocument ' TJS 02/12/11
        Get
            Return m_ImportExportRule.SourceConfig
        End Get
    End Property
#End Region

#Region " ChannelAdvCarriers "
    Public WriteOnly Property ChannelAdvCarriers() As ChannelAdvCarrier() ' TJS 27/09/10
        Set(ByVal value As ChannelAdvCarrier()) ' TJS 27/09/10
            m_ChannelAdvCarriers = value ' TJS 27/09/10
        End Set
    End Property
#End Region

#Region " AccountDetailsValidationStage "
    Public Property AccountDetailsValidationStage() As String ' TJS 18/03/11
        Get
            Return m_ImportExportRule.AccountDetailsValidationStage ' TJS 18/03/11
        End Get
        Set(ByVal value As String) ' TJS 18/03/11
            m_ImportExportRule.AccountDetailsValidationStage = value ' TJS 18/03/11
        End Set
    End Property
#End Region

#Region " IsHostISeCommercePlus "
    Public ReadOnly Property IsHostISeCommercePlus() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Property added
        ' 04/04/11 | TJS             | 2011.0.07 | Modifed to cater for conditional compilation for IS 4-x and IS 5-5 compatibility
        '------------------------------------------------------------------------------------------

        Get
            If Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.LicenseInfo.ProductEdition = "ISB" Then
                Return True
            Else
                Return False
            End If
        End Get

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
            Return m_ImportExportRule.LerrynCustomerCode
        End Get
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New(ByRef p_LerrynImportExportConfigDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef p_ErrorNotification As Lerryn.Facade.ImportExport.ErrorNotification, ByVal p_BaseProductCode As String, ByVal p_BaseProductName As String) ' TJS 10/06/12
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
        m_ErrorNotification = p_ErrorNotification ' TJS 10/06/12
        If String.IsNullOrEmpty(m_ErrorNotification.BaseProductCode) Then ' TJS 10/06/12
            m_ErrorNotification.BaseProductCode = p_BaseProductCode ' TJS 10/06/12
        End If
        If String.IsNullOrEmpty(m_ErrorNotification.BaseProductName) Then ' TJS 10/06/12
            m_ErrorNotification.BaseProductName = p_BaseProductName ' TJS 10/06/12
        End If
        m_ImportExportDataset = p_LerrynImportExportConfigDataset
        m_ImportExportRule = New Lerryn.Facade.ImportExport.ImportExportFacade(m_ImportExportDataset, m_ErrorNotification, p_BaseProductCode, p_BaseProductName) ' TJS 10/06/12
        m_BaseProductCode = p_BaseProductCode ' TJS 10/07/09
        MyBase.InitializeDataset()
        CheckActivation()

    End Sub
#End Region

#Region " ReCheckActivation "
    Public Sub ReCheckActivation()

        CheckActivation()

    End Sub
#End Region

#Region " CheckActivation "
    Private Sub CheckActivation()

        ' read all licences as we need base licence and all add-ons
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynLicences_DEV000221.TableName, "ReadLerrynLicences_DEV000221"}, _
            New String() {Me.m_ImportExportDataset.System_DEV000221.TableName, "ReadSystem_DEV000221"}, _
            New String() {Me.m_ImportExportDataset.SystemCompanyInformation.TableName, "ReadSystemCompanyInformation"}, _
            New String() {Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, "ReadLerrynImportExportConfig_DEV000221"}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJs 02/12/11

        Me.m_ImportExportRule.CheckActivation()

    End Sub
#End Region

#Region " GetSystemLicenceID "
    Public Function GetSystemLicenceID(ByVal CreateNew As Boolean) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/07/09 | TJS             | 2009.3.01 | Modified to save copy Cache DB ID to prevent licence errors if cache db changes
        ' 04/06/10 | TJS             | 2010.0.07 | Modified to use saved System Hash COde
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim commandSetUpdate()() As String, strReturn As String

        strReturn = Me.m_ImportExportRule.GetSystemLicenceID(CreateNew, Me.m_ImportExportRule.SystemID, _
            Me.m_ImportExportRule.CacheID, Me.m_ImportExportRule.SystemHashCode) ' TJS 04/06/10
        If CreateNew Then
            commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                "CreateLerrynLicences_DEV000221", "UpdateLerrynLicences_DEV000221", "DeleteLerrynLicences_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.System_DEV000221.TableName, _
                "CreateSystem_DEV000221", "UpdateSystem_DEV000221", "DeleteSystem_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.SystemCompanyInformation.TableName, "", "UpdateSystemCompanyInformation", ""}} ' TJS 14/07/09
            Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn System ID created", False)
        End If
        Return strReturn

    End Function
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
        ' 25/01/12 | TJS             | 2011.2.04 | Modified to ensure updated licences are saved
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim commandSetUpdate()() As String, strReturn As String

        strReturn = Me.m_ImportExportRule.GetActivationCode(ErrorCode)
        commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
           "CreateLerrynLicences_DEV000221", "UpdateLerrynLicences_DEV000221", "DeleteLerrynLicences_DEV000221"}} ' TJS 25/01/12
        Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn Licences updated", False) ' TJS 25/01/12
        Return strReturn

    End Function
#End Region

#Region " GetISPluginBaseURL "
    Public Function GetISPluginBaseURL() As String

        Return Me.m_ImportExportRule.GetISPluginBaseURL

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
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return m_ImportExportRule.ValidateSource(Handler, SourceCode, SourcePassword, Reprocess) ' TJS 06/10/09

    End Function
#End Region

#Region " ValidateDisplayedActivation "
    Public Function ValidateDisplayedActivation(ByVal ProductCode As String, ByVal LicenceCode As String, ByRef ErrorCode As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.       | Description
        '------------------------------------------------------------------------------------------
        ' 04/06/10 | TJS             | 2010.0.02.0 | Modified to use saved System Hash Code
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Me.m_ImportExportRule.ValidateDisplayedActivation(ProductCode, LicenceCode, Me.m_ImportExportRule.SystemHashCode, ErrorCode) ' TJS 04/06/10

    End Function
#End Region

#Region " UpdateDisplayedActivation "
    Public Function UpdateDisplayedActivation(ByVal ProductCode As String, ByVal LicenceCode As String, ByVal EndOfFreeTrial As Date, _
        ByVal NextInvoiceDue As Date, ByVal PaymentPeriod As String, ByRef ErrorCode As Integer, ByRef UpdatedCode As String) As Boolean ' TJS 18/03/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.       | Description
        '------------------------------------------------------------------------------------------
        ' 04/06/10 | TJS             | 2010.0.02.0 | Modified to save System Hash Code
        ' 18/03/11 | TJS             | 2011.0.01   | Added EndOfFreeTrial parameter
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim commandSetUpdate()() As String, bIsValid As Boolean

        bIsValid = Me.m_ImportExportRule.UpdateDisplayedActivation(ProductCode, LicenceCode, Me.m_ImportExportRule.SystemHashCode, _
            EndOfFreeTrial, NextInvoiceDue, PaymentPeriod, ErrorCode, UpdatedCode) ' TJS 04/06/10 TJS 18/03/11
        If bIsValid Then
            commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                  "CreateLerrynLicences_DEV000221", "UpdateLerrynLicences_DEV000221", "DeleteLerrynLicences_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.System_DEV000221.TableName, _
                "CreateSystem_DEV000221", "UpdateSystem_DEV000221", "DeleteSystem_DEV000221"}} ' TJS 04/06/10
            Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn Licences updated", False)
        End If
        Return bIsValid

    End Function
#End Region

#Region " PurchaseActivation "
    Public Function PurchaseActivation(ByVal ActivationPeriod As String, ByVal ActivationType As String, ByVal ActivationCost As Decimal, _
        ByVal EndFreeTrial As Date, ByVal NextInvoiceDue As Date, ByVal PurchaseNow As Boolean, ByRef ErrorCode As Integer, _
        ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    first activation or re-activation only
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Added error trap for IS cuncurrency errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim commandSetUpdate()() As String, bIsValid As Boolean

        LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
            "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        bIsValid = Me.m_ImportExportRule.PurchaseActivation(ActivationPeriod, ActivationType, Me.m_ImportExportDataset.SystemCompanyInformation(0).CurrencyCode, _
            ActivationCost, EndFreeTrial, NextInvoiceDue, PurchaseNow, ErrorCode, ErrorDetails)
        If bIsValid Then
            commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                    "CreateLerrynLicences_DEV000221", "UpdateLerrynLicences_DEV000221", "DeleteLerrynLicences_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.System_DEV000221.TableName, _
                    "CreateSystem_DEV000221", "UpdateSystem_DEV000221", "DeleteSystem_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                    "", "UpdateLerrynImportExportConfig_DEV000221", ""}}
            Try ' TJS 02/12/11
                Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn Licences updated", False)

            Catch ex As Exception ' TJS 02/12/11
                Interprise.Presentation.Base.Message.MessageWindow.Show(ex) ' TJS 02/12/11
            End Try
        End If
        Return bIsValid

    End Function
#End Region

#Region " InitialiseMonthlyPrecentageBilling "
    Public Function InitialiseMonthlyPrecentageBilling(ByVal EndFreeTrial As Date, ByVal FirstInvoiceDue As Date, _
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
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim commandSetUpdate()() As String, bIsValid As Boolean

        bIsValid = Me.m_ImportExportRule.InitialiseMonthlyPrecentageBilling(Me.m_ImportExportDataset.SystemCompanyInformation(0).CurrencyCode, _
            EndFreeTrial, FirstInvoiceDue, ErrorCode, ErrorDetails)
        If bIsValid Then
            commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                    "CreateLerrynLicences_DEV000221", "UpdateLerrynLicences_DEV000221", "DeleteLerrynLicences_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.System_DEV000221.TableName, _
                    "CreateSystem_DEV000221", "UpdateSystem_DEV000221", "DeleteSystem_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                    "", "UpdateLerrynImportExportConfig_DEV000221", ""}}
            Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn Licences updated", False)
        End If
        Return bIsValid

    End Function
#End Region

#Region " UpdateMonthlyPercentageBilling "
    Public Function UpdateMonthlyPercentageBilling(ByRef ErrorCode As Integer, ByRef ErrorDetails As String) As Boolean
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

        Dim commandSetUpdate()() As String, bIsValid As Boolean

        bIsValid = Me.m_ImportExportRule.UpdateMonthlyPercentageBilling(Me.m_ImportExportDataset.SystemCompanyInformation(0).CurrencyCode, _
            ErrorCode, ErrorDetails)
        If bIsValid Then
            commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynLicences_DEV000221.TableName, _
                    "CreateLerrynLicences_DEV000221", "UpdateLerrynLicences_DEV000221", "DeleteLerrynLicences_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.System_DEV000221.TableName, _
                    "CreateSystem_DEV000221", "UpdateSystem_DEV000221", "DeleteSystem_DEV000221"}, _
                New String() {Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                    "", "UpdateLerrynImportExportConfig_DEV000221", ""}}
            Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn Licences updated", False)
        End If
        Return bIsValid

    End Function
#End Region

#Region " CancelActivation "
    Public Function CancelActivation() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to pass count of Imported Inventory Items to CancelActivation function
        ' 05/07/12 | TJS             | 2012.1.08 | Enabled Amazon import
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iImportedInventoryCount As Integer ' TJS 02/12/11

        iImportedInventoryCount = CInt(Me.GetField("SELECT (SELECT COUNT(*) FROM InventoryAmazonDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
            "(SELECT COUNT(*) FROM InventoryASPStorefrontDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
            "(SELECT COUNT(*) FROM InventoryChannelAdvDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
            "(SELECT COUNT(*) FROM InventoryMagentoDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1)", CommandType.Text, Nothing)) ' TJS 02/12/11 TJS 05/07/12
        Return Me.m_ImportExportRule.CancelActivation(iImportedInventoryCount) ' TJS 02/12/11

    End Function
#End Region

#Region " GetActivationCost "
    Public Function GetActivationCost(ByVal ExistingConnectors As Boolean) As String
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

        Return Me.m_ImportExportRule.GetActivationCost(ExistingConnectors)

    End Function
#End Region

#Region " NewSource "
    Public Sub NewSource()
        Me.m_ImportExportRule.NewSource()
    End Sub
#End Region

#Region " UpdateConnectorMaxAccounts "
    Public Function UpdateConnectorMaxAccounts(ByVal ConnectorProductCode As String, ByVal UpdateConfigSettingsDataset As Boolean) As Boolean ' TJS 18/03/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to allow use when XMLConfigSettings not loaded
        ' 02/12/11 | TJS             | 2011.2.00 | Modified save Config if connector count changed
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim commandSetUpdate()() As String, bReturnValue As Boolean, bConfigUpdated As Boolean ' TJS 02/12/11

        bReturnValue = Me.m_ImportExportRule.UpdateConnectorMaxAccounts(ConnectorProductCode, UpdateConfigSettingsDataset, bConfigUpdated) ' TJS 18/03/11 TJS 02/12/11
        If bConfigUpdated Then ' TJS 02/12/11
            commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                "", "UpdateLerrynImportExportConfig_DEV000221", ""}} ' TJS 02/12/11
            Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn eShopCONENCT config updated", False) ' TJS 02/12/11
        End If
        Return bReturnValue ' TJS 02/12/11

    End Function
#End Region

#Region " CheckAndUpdateConfigSettings "
    Public Sub CheckAndUpdateConfigSettings()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/04/12 | TJS             | 2012.1.02 | Function added
        ' 10/06/12 | TJS             | 2012.1.05 | Added Magento EnablePaymentTypeTranslation
        ' 16/06/12 | TJS             | 2012.1.07 | Modified to cater for different developerids in different countries
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon FBA connector settings 
        ' 08/07/12 | TJS             | 2012.1.09 | Added UseShipToClassTemplate and replaced connector source codes with relevant constant values
        ' 02/08/12 | TJS             | 2012.1.11 | Added code to ensure HasSourceIDs_DEV000221 is set properly
        ' 18/01/13 | TJS             | 2012.1.17 | Added AllocateAndReserveStock
        ' 09/03/13 | TJS             | 2013.1.01 | Corrected insertion points for ExtensionDataField2Mapping etc
        ' 22/03/13 | TJS             | 2013.1.05 | Added code to rename Amazon MerchantID as MerchantToken
        ' 03/05/13 | TJS             | 2013.1.12 | Added code to rename Amazon FBA MerchantID as MerchantToken
        ' 08/05/13 | TJS             | 2013.1.13 | Added Magento InhibitInventoryUpdates, CreateCustomerForGuestCheckout 
        '                                        | and IncludeChildItemsOnOrder()
        ' 03/07/13 | TJS/FA          | 2013.1.24 | Added code to rename eBay User and Password as AuthToken and TokenExpires
        ' 16/07/13 | TJS/FA          | 2013.1.29 | Corrected eBay config update
        ' 30/07/13 | TJS             | 2013.1.32 | Added eBay PricesAreTaxInclusive and TaxCodeForSourceTax
        ' 19/09/13 | TJS             | 2013.3.00 | Added generic ImportMissingItemsAsNonStock
        ' 02/10/13 | TJS             | 2013.3.03 | Added MagentoVersion
        ' 13/11/13 | TJS             | 2013.3.08 | Added Magento V2SoapAPIWSICompliant, LerrynAPIVersion and UpdateMagentoSpecialPricing and added error handler
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        ' 11/01/14 | TJS             | 2013.4.04 | Added Volusion DefaultShippingMethodID
        ' 15/01/14 | TJS             | 2013.4.05 | Added Volusion EnableSKUAliasLookup
        ' 11/02/14 | TJS             | 2013.4.09 | added Volusion EnablePaymentTypeTranslation
        ' 01/05/14 | TJS             | 2014.0.02 | Added Magento CardAuthAndCaptureWithOrder
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowCheckConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row
        Dim XMLConfig As XDocument = Nothing ', XMLGeneralTemp As XDocument
        Dim XMLGeneralSettings As XElement, XMLConnectorSettings As XElement, XMLNodeToAddAfter As XElement
        Dim XMLConnectorList As System.Collections.Generic.IEnumerable(Of XElement), XMLFirstChild As XNode
        Dim commandSetUpdate()() As String, strCurrency As String, iLoop As Integer, bConfigUpdated As Boolean

        Try
            If IsActivated AndAlso GetField("ConfigVersion_DEV000221", "LerrynImportExportServiceAction_DEV000221", Nothing) <> _
                    System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion Then
                LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

                ' start of code added TJS 05/07/12
                rowCheckConfig = m_ImportExportDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(AMAZON_FBA_SOURCE_CODE)
                If rowCheckConfig Is Nothing Then
                    rowCheckConfig = m_ImportExportDataset.LerrynImportExportConfig_DEV000221.NewLerrynImportExportConfig_DEV000221Row()
                    rowCheckConfig.SourceCode_DEV000221 = AMAZON_FBA_SOURCE_CODE
                    rowCheckConfig.SourceName_DEV000221 = "Amazon FBA Connector"
                    rowCheckConfig.EnableSourcePassword_DEV000221 = False
                    rowCheckConfig.InputMode_DEV000221 = "SOAP Poll"
                    rowCheckConfig.InputHandler_DEV000221 = "Amazon FBA eShopCONNECTOR"
                    rowCheckConfig.ConfigSettings_DEV000221 = "<eShopCONNECTConfig><General><UseShipToClassTemplate>Yes</UseShipToClassTemplate><DefaultShippingMethodGroup></DefaultShippingMethodGroup><DefaultShippingMethod></DefaultShippingMethod><DefaultWarehouse></DefaultWarehouse><DueDateDaysInFuture>1</DueDateDaysInFuture></General><AmazonFBA><AmazonSite></AmazonSite><MerchantToken></MerchantToken><AccountDisabled>No</AccountDisabled></AmazonFBA></eShopCONNECTConfig>" ' TJS 08/07/12 TJS 22/03/13
                    rowCheckConfig.HasSourceIDs_DEV000221 = True
                    rowCheckConfig.NotIneCPlus_DEV000221 = False ' TJS 02/08/12
                    rowCheckConfig.ShowIfActivated_DEV000221 = False ' TJS 02/08/12
                    m_ImportExportDataset.LerrynImportExportConfig_DEV000221.AddLerrynImportExportConfig_DEV000221Row(rowCheckConfig)
                End If
                ' end of code added TJS 05/07/12

                ' start of code added TJS 20/11/13
                rowCheckConfig = m_ImportExportDataset.LerrynImportExportConfig_DEV000221.FindBySourceCode_DEV000221(THREE_D_CART_SOURCE_CODE)
                If rowCheckConfig Is Nothing Then
                    rowCheckConfig = m_ImportExportDataset.LerrynImportExportConfig_DEV000221.NewLerrynImportExportConfig_DEV000221Row()
                    rowCheckConfig.SourceCode_DEV000221 = THREE_D_CART_SOURCE_CODE
                    rowCheckConfig.SourceName_DEV000221 = "3DCart Connector (Beta)"
                    rowCheckConfig.EnableSourcePassword_DEV000221 = False
                    rowCheckConfig.InputMode_DEV000221 = "Web Poll"
                    rowCheckConfig.InputHandler_DEV000221 = "3DCart eShopCONNECTOR"
                    rowCheckConfig.ConfigSettings_DEV000221 = "<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType></CustomerBusinessType><CustomerBusinessClass></CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>DEFAULT</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><ThreeDCart><StoreID>Main</StoreID><StoreURL></StoreURL><UserKey></UserKey><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><Currency></Currency><EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><EnableSKUAliasLookup>No</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></ThreeDCart></eShopCONNECTConfig>"
                    rowCheckConfig.HasSourceIDs_DEV000221 = True
                    rowCheckConfig.NotIneCPlus_DEV000221 = False
                    rowCheckConfig.ShowIfActivated_DEV000221 = False
                    m_ImportExportDataset.LerrynImportExportConfig_DEV000221.AddLerrynImportExportConfig_DEV000221Row(rowCheckConfig)
                End If
                ' end of code added TJS 20/11/13

                strCurrency = GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                For iLoop = 0 To m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count - 1
                    XMLConfig = XDocument.Parse(m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).ConfigSettings_DEV000221)
                    XMLGeneralSettings = XMLConfig.XPathSelectElement("eShopCONNECTConfig/General")
                    bConfigUpdated = False

                    ' start of code added TJS 02/08/12
                    If (m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = GENERIC_IMPORT_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = FILE_IMPORT_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = PROSPECT_LEAD_IMPORT_SOURCE_CODE) Then
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).IsHasSourceIDs_DEV000221Null OrElse _
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).HasSourceIDs_DEV000221 Then
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).HasSourceIDs_DEV000221 = False
                            bConfigUpdated = True
                        End If
                    Else
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).IsHasSourceIDs_DEV000221Null OrElse _
                            Not m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).HasSourceIDs_DEV000221 Then
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).HasSourceIDs_DEV000221 = True
                            bConfigUpdated = True
                        End If
                    End If

                    If (m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = AMAZON_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = CHANNEL_ADVISOR_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = EBAY_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = SHOP_COM_SOURCE_CODE) Then
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).IsNotIneCPlus_DEV000221Null OrElse _
                            Not m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).NotIneCPlus_DEV000221 Then
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).NotIneCPlus_DEV000221 = True
                            bConfigUpdated = True
                        End If
                    Else
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).IsNotIneCPlus_DEV000221Null OrElse _
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).NotIneCPlus_DEV000221 Then
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).NotIneCPlus_DEV000221 = False
                            bConfigUpdated = True
                        End If
                    End If

                    If (m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = AMAZON_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = CHANNEL_ADVISOR_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = EBAY_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = FILE_IMPORT_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = PROSPECT_LEAD_IMPORT_SOURCE_CODE Or _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = SHOP_COM_SOURCE_CODE) Then
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).IsShowIfActivated_DEV000221Null OrElse _
                            Not m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).ShowIfActivated_DEV000221 Then
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).ShowIfActivated_DEV000221 = True
                            bConfigUpdated = True
                        End If
                    Else
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).IsShowIfActivated_DEV000221Null OrElse _
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).ShowIfActivated_DEV000221 Then
                            m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).ShowIfActivated_DEV000221 = False
                            bConfigUpdated = True
                        End If
                    End If
                    ' end of code added TJS 02/08/12

                    ' add General setting CreateCustomerAsCompany if missing
                    If XMLGeneralSettings.XPathSelectElement("CreateCustomerAsCompany") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("CustomerBusinessClass")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("CreateCustomerAsCompany", "Yes"))
                        bConfigUpdated = True
                    End If

                    ' rename General setting CreditCardPaymentMethodCode as CreditCardPaymentTermCode if not already done
                    If XMLGeneralSettings.XPathSelectElement("CreditCardPaymentMethodCode") IsNot Nothing Then
                        XMLGeneralSettings.XPathSelectElement("CreditCardPaymentMethodCode").Name = "CreditCardPaymentTermCode"
                        bConfigUpdated = True
                    End If

                    ' add General setting DefaultPaymentTermGroup if missing
                    If XMLGeneralSettings.XPathSelectElement("DefaultPaymentTermGroup") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("DefaultShippingMethodGroup")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("DefaultPaymentTermGroup", "DEFAULT"))
                        bConfigUpdated = True
                    End If

                    ' add General setting DefaultWarehouse if missing
                    If XMLGeneralSettings.XPathSelectElement("DefaultWarehouse") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE Then
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("DefaultShippingMethod") ' TJS 05/07/12
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("DefaultWarehouse", ""))
                        bConfigUpdated = True
                    End If

                    ' add General setting ExternalSystemCardPaymentCode if missing
                    If XMLGeneralSettings.XPathSelectElement("ExternalSystemCardPaymentCode") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("AuthoriseCreditCardOnImport")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ExternalSystemCardPaymentCode", "Ext. System C/Card"))
                        bConfigUpdated = True
                    End If

                    ' add General setting EnableCoupons if missing
                    If XMLGeneralSettings.XPathSelectElement("EnableCoupons") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("ExternalSystemCardPaymentCode")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("EnableCoupons", "No"))
                        bConfigUpdated = True
                    End If

                    ' add General setting SetDisableFreightCalculation if missing
                    If XMLGeneralSettings.XPathSelectElement("SetDisableFreightCalculation") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("RequireSourceCustomerID")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("SetDisableFreightCalculation", "Yes"))
                        bConfigUpdated = True
                    End If

                    ' add General setting IgnoreVoidedOrdersAndInvoices if missing
                    If XMLGeneralSettings.XPathSelectElement("IgnoreVoidedOrdersAndInvoices") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("SetDisableFreightCalculation")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("IgnoreVoidedOrdersAndInvoices", "Yes"))
                        bConfigUpdated = True
                    End If

                    ' add General setting AcceptSourceSalesTaxCalculation if missing
                    If XMLGeneralSettings.XPathSelectElement("AcceptSourceSalesTaxCalculation") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("IgnoreVoidedOrdersAndInvoices")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("AcceptSourceSalesTaxCalculation", "No"))
                        bConfigUpdated = True
                    End If

                    ' add General setting XMLImportFileSavePath if missing (except on FileImport connector
                    If XMLGeneralSettings.XPathSelectElement("XMLImportFileSavePath") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> FILE_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("AcceptSourceSalesTaxCalculation")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("XMLImportFileSavePath", ""))
                        bConfigUpdated = True
                    End If

                    ' start of code added TJS 05/07/12
                    ' add General setting UseShipToClassTemplate if missing
                    If XMLGeneralSettings.XPathSelectElement("UseShipToClassTemplate") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then ' TJS 05/07/12
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("EnableDeliveryMethodTranslation")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("UseShipToClassTemplate", "No"))
                        bConfigUpdated = True
                    End If
                    ' end of code added TJS 05/07/12

                    ' start of code added TJS 19/09/13
                    ' add General setting ImportMissingItemsAsNonStock if missing
                    If XMLGeneralSettings.XPathSelectElement("ImportMissingItemsAsNonStock") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("AcceptSourceSalesTaxCalculation")
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ImportMissingItemsAsNonStock", "No"))
                        bConfigUpdated = True
                    End If
                    ' end of code added TJS 19/09/13

                    ' start of code added TJS 18/01/13
                    ' add General setting AllocateAndReserveStock if missing
                    If XMLGeneralSettings.XPathSelectElement("AllocateAndReserveStock") Is Nothing And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> PROSPECT_LEAD_IMPORT_SOURCE_CODE And _
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 <> AMAZON_FBA_SOURCE_CODE Then
                        ' find node which preceeds new node
                        XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("ImportMissingItemsAsNonStock") ' TJS 19/09/13
                        ' add new node
                        XMLNodeToAddAfter.AddAfterSelf(New XElement("AllocateAndReserveStock", "No"))
                        bConfigUpdated = True
                    End If
                    ' end of code added TJS 18/01/13

                    Select Case m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221
                        Case AMAZON_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Amazon Order" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Amazon Connector"
                                bConfigUpdated = True
                            End If

                            ' add General setting SetDisableFreightCalculation if missing
                            If XMLGeneralSettings.XPathSelectElement("AllowBlankPostalcode") Is Nothing Then
                                ' find node which preceeds new node
                                XMLNodeToAddAfter = XMLGeneralSettings.XPathSelectElement("RequireSourceCustomerID")
                                ' add new node
                                XMLNodeToAddAfter.AddAfterSelf(New XElement("AllowBlankPostalcode", "Yes"))
                                bConfigUpdated = True
                            End If

                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' add Amazon setting AmazonSite if missing
                                    If XMLConnectorSettings.XPathSelectElement("AmazonSite") Is Nothing Then
                                        ' new node needs to be the first node so get existing first node
                                        XMLFirstChild = XMLConnectorSettings.FirstNode
                                        ' add new node before it
                                        XMLFirstChild.AddBeforeSelf(New XElement("AmazonSite", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' Start of code added TJS 16/06/12
                                    ' add Amazon setting OwnAccessKeyID if missing
                                    If XMLConnectorSettings.XPathSelectElement("OwnAccessKeyID") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("AmazonSite")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("OwnAccessKeyID", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting OwnSecretAccessKey if missing
                                    If XMLConnectorSettings.XPathSelectElement("OwnSecretAccessKey") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("OwnAccessKeyID")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("OwnSecretAccessKey", ""))
                                        bConfigUpdated = True
                                    End If
                                    ' End of code added TJS 16/06/12

                                    ' rename Amazon setting MerchantID to MerchantToken if not already done
                                    If XMLConnectorSettings.XPathSelectElement("MerchantID") IsNot Nothing Then ' TJS 22/03/13
                                        XMLConnectorSettings.XPathSelectElement("MerchantID").Name = "MerchantToken" ' TJS 22/03/13
                                        bConfigUpdated = True ' TJS 22/03/13
                                    End If

                                    ' add Amazon setting MerchantName if missing
                                    If XMLConnectorSettings.XPathSelectElement("MerchantName") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("MerchantToken") ' TJS 22/03/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("MerchantName", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' rename Amazon setting UserLogin to MWSMerchantID if not already done
                                    If XMLConnectorSettings.XPathSelectElement("UserLogin") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("UserLogin").Name = "MWSMerchantID"
                                        bConfigUpdated = True
                                    End If

                                    ' rename Amazon setting LoginPassword to MWSMarketplaceID if not already done
                                    If XMLConnectorSettings.XPathSelectElement("LoginPassword") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("LoginPassword").Name = "MWSMarketplaceID"
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting MWSMerchantID if missing
                                    If XMLConnectorSettings.XPathSelectElement("MWSMerchantID") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("MerchantName")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("MWSMerchantID", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting MWSMarketplaceID if missing
                                    If XMLConnectorSettings.XPathSelectElement("MWSMarketplaceID") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("MWSMerchantID")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("MWSMarketplaceID", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting AmazonManualProcessingPath if missing
                                    If XMLConnectorSettings.XPathSelectElement("AmazonManualProcessingPath") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("MWSMarketplaceID")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("AmazonManualProcessingPath", "C:\eShopCONNECT\FilesForManualAction"))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting PaymentType if missing
                                    If XMLConnectorSettings.XPathSelectElement("PaymentType") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("Currency")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("PaymentType", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' rename Amazon setting SendPricesAsTaxInclusive to PricesAreTaxInclusive if not already done
                                    If XMLConnectorSettings.XPathSelectElement("SendPricesAsTaxInclusive") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("SendPricesAsTaxInclusive").Name = "PricesAreTaxInclusive"
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting PricesAreTaxInclusive if missing
                                    If XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ISItemIDField")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("PricesAreTaxInclusive", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting TaxCodeForSourceTax if missing
                                    If XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("TaxCodeForSourceTax", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting DefaultUpliftPercent if missing
                                    If XMLConnectorSettings.XPathSelectElement("DefaultUpliftPercent") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ISItemIDField")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("DefaultUpliftPercent", "0"))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting EnableSKUAliasLookup if missing
                                    If XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("DefaultUpliftPercent")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("EnableSKUAliasLookup", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add Amazon setting AccountDisabled if missing
                                    If XMLConnectorSettings.XPathSelectElement("AccountDisabled") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("AccountDisabled", "No"))
                                        bConfigUpdated = True
                                    End If

                                Catch ex As Exception

                                End Try
                            Next

                        Case ASP_STORE_FRONT_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "ASPDotNetStoreFront Order" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "ASPDotNetStoreFront Connector"
                                bConfigUpdated = True
                            End If

                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' add ASPStoreFront setting ExtensionDataField1Mapping if missing
                                    If XMLConnectorSettings.XPathSelectElement("ExtensionDataField1Mapping") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ExtensionDataField1Mapping", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ASPStoreFront setting ExtensionDataField2Mapping if missing
                                    If XMLConnectorSettings.XPathSelectElement("ExtensionDataField2Mapping") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ExtensionDataField1Mapping") ' TJS 09/03/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ExtensionDataField2Mapping", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ASPStoreFront setting ExtensionDataField3Mapping if missing
                                    If XMLConnectorSettings.XPathSelectElement("ExtensionDataField3Mapping") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ExtensionDataField2Mapping") ' TJS 09/03/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ExtensionDataField3Mapping", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ASPStoreFront setting ExtensionDataField4Mapping if missing
                                    If XMLConnectorSettings.XPathSelectElement("ExtensionDataField4Mapping") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ExtensionDataField3Mapping") ' TJS 09/03/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ExtensionDataField4Mapping", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ASPStoreFront setting ExtensionDataField5Mapping if missing
                                    If XMLConnectorSettings.XPathSelectElement("ExtensionDataField5Mapping") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ExtensionDataField4Mapping") ' TJS 09/03/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ExtensionDataField5Mapping", ""))
                                        bConfigUpdated = True
                                    End If

                                Catch ex As Exception

                                End Try
                            Next

                        Case CHANNEL_ADVISOR_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Channel Advisor Order" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Channel Advisor Connector"
                                bConfigUpdated = True
                            End If

                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' add ChannelAdvisor setting PaymentType if missing
                                    If XMLConnectorSettings.XPathSelectElement("PaymentType") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("Currency")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("PaymentType", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ChannelAdvisor setting EnablePaymentTypeTranslation if missing
                                    If XMLConnectorSettings.XPathSelectElement("EnablePaymentTypeTranslation") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("PaymentType") ' TJS 09/03/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("EnablePaymentTypeTranslation", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ChannelAdvisor setting TaxCodeForSourceTax if missing
                                    If XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("TaxCodeForSourceTax", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ChannelAdvisor setting ActionIfNoPayment if missing
                                    If XMLConnectorSettings.XPathSelectElement("ActionIfNoPayment") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ActionIfNoPayment", "Ignore"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ChannelAdvisor setting EnableSKUAliasLookup if missing
                                    If XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ActionIfNoPayment")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("EnableSKUAliasLookup", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ChannelAdvisor setting AccountDisabled if missing
                                    If XMLConnectorSettings.XPathSelectElement("AccountDisabled") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("AccountDisabled", "No"))
                                        bConfigUpdated = True
                                    End If

                                Catch ex As Exception

                                End Try
                            Next

                        Case EBAY_SOURCE_CODE
                            ' start of code added TJS/FA 03/07/13
                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_EBAY_LIST) ' TJS 16/07/13
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' rename eBay setting User to AuthToken if not already done
                                    If XMLConnectorSettings.XPathSelectElement("User") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("User").Name = "AuthToken"
                                        bConfigUpdated = True
                                    End If

                                    ' rename eBay setting Password to Token if not already done
                                    If XMLConnectorSettings.XPathSelectElement("Password") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("Password").Name = "TokenExpires"
                                        bConfigUpdated = True
                                    End If

                                    ' start of code added TJS 30/07/13
                                    ' add eBay setting PricesAreTaxInclusive if missing
                                    If XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ISItemIDField")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("PricesAreTaxInclusive", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add eBay setting TaxCodeForSourceTax if missing
                                    If XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("TaxCodeForSourceTax", ""))
                                        bConfigUpdated = True
                                    End If
                                    ' end of code added TJS 30/07/13

                                Catch ex As Exception

                                End Try
                            Next
                            ' end of code added TJS/FA 03/07/13

                        Case FILE_IMPORT_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 <> "XML File Import" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "XML File Import"
                                bConfigUpdated = True
                            End If

                        Case GENERIC_IMPORT_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 <> "Generic XML Web Import" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Generic XML Web Import"
                                bConfigUpdated = True
                            End If

                        Case MAGENTO_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Magento Order" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Magento Connector"
                                bConfigUpdated = True
                            End If

                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' rename Magento setting SiteID to InstanceID if not already done
                                    If XMLConnectorSettings.XPathSelectElement("SiteID") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("SiteID").Name = "InstanceID"
                                        bConfigUpdated = True
                                    End If

                                    ' add Magento setting V2SoapAPIWSICompliant if missing
                                    If XMLConnectorSettings.XPathSelectElement("V2SoapAPIWSICompliant") Is Nothing Then ' TJS 13/11/13
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("APIURL") ' TJS 13/11/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("V2SoapAPIWSICompliant", "No")) ' TJS 13/11/13
                                        bConfigUpdated = True ' TJS 13/11/13
                                    End If

                                    ' add Magento setting MagentoVersion if missing
                                    If XMLConnectorSettings.XPathSelectElement("MagentoVersion") Is Nothing Then ' TJS 02/10/13
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("APIPwd") ' TJS 02/10/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("MagentoVersion", "")) ' TJS 02/10/13 TJS 13/11/13
                                        bConfigUpdated = True ' TJS 02/10/13
                                    End If

                                    ' add Magento setting LerrynAPIVersion if missing
                                    If XMLConnectorSettings.XPathSelectElement("LerrynAPIVersion") Is Nothing Then ' TJS 13/11/13
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("MagentoVersion") ' TJS 13/11/13
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("LerrynAPIVersion", "0")) ' TJS 13/11/13
                                        bConfigUpdated = True ' TJS 13/11/13
                                    End If

                                    ' add Magento setting TaxCodeForSourceTax if missing
                                    If XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("TaxCodeForSourceTax", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add Magento setting EnablePaymentTypeTranslation if missing
                                    If XMLConnectorSettings.XPathSelectElement("EnablePaymentTypeTranslation") Is Nothing Then ' TJS 10/06/12
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax") ' TJS 10/06/12
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("EnablePaymentTypeTranslation", "No")) ' TJS 10/06/12
                                        bConfigUpdated = True ' TJS 10/06/12
                                    End If

                                    ' add Magento setting SplitSKUSeparatorCharacters if missing
                                    If XMLConnectorSettings.XPathSelectElement("SplitSKUSeparatorCharacters") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("SplitSKUSeparatorCharacters", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add Magento setting ProductListBlockSize if missing
                                    If XMLConnectorSettings.XPathSelectElement("ProductListBlockSize") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("SplitSKUSeparatorCharacters")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ProductListBlockSize", "10000"))
                                        bConfigUpdated = True
                                    End If

                                    ' start of code added TJS 08/05/13
                                    ' add Magento setting InhibitInventoryUpdates if missing
                                    If XMLConnectorSettings.XPathSelectElement("InhibitInventoryUpdates") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ProductListBlockSize")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("InhibitInventoryUpdates", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add Magento setting CreateCustomerForGuestCheckout if missing
                                    If XMLConnectorSettings.XPathSelectElement("CreateCustomerForGuestCheckout") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("InhibitInventoryUpdates")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("CreateCustomerForGuestCheckout", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add Magento setting IncludeChildItemsOnOrder if missing
                                    If XMLConnectorSettings.XPathSelectElement("IncludeChildItemsOnOrder") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("CreateCustomerForGuestCheckout")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("IncludeChildItemsOnOrder", "No"))
                                        bConfigUpdated = True
                                    End If
                                    ' end of code added TJS 08/05/13

                                    ' end of code added TJS 13/11/13
                                    ' add Magento setting UpdateMagentoSpecialPricing if missing
                                    If XMLConnectorSettings.XPathSelectElement("UpdateMagentoSpecialPricing") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("IncludeChildItemsOnOrder")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("UpdateMagentoSpecialPricing", "No"))
                                        bConfigUpdated = True
                                    End If
                                    ' end of code added TJS 13/11/13

                                    ' start of code added TJS 01/05/14
                                    ' add Magento setting CardAuthAndCaptureWithOrder if missing
                                    If XMLConnectorSettings.XPathSelectElement("CardAuthAndCaptureWithOrder") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("APISupportsPartialShipments")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("CardAuthAndCaptureWithOrder", "No"))
                                        bConfigUpdated = True
                                    End If
                                    ' end of code added TJS 01/05/14

                                Catch ex As Exception

                                End Try
                            Next

                        Case PROSPECT_LEAD_IMPORT_SOURCE_CODE, SEARS_COM_SOURCE_CODE

                        Case SHOP_COM_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Shop.com Order" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Shop.com Connector"
                                bConfigUpdated = True
                            End If

                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_SHOPDOTCOM_CATALOG_LIST)
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' rename ShopDotCom setting ItemIDField to ISItemIDField if not already done
                                    If XMLConnectorSettings.XPathSelectElement("ItemIDField") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("ItemIDField").Name = "ISItemIDField"
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting FTPUploadServerURL if missing
                                    If XMLConnectorSettings.XPathSelectElement("FTPUploadServerURL") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("StatusPostURL")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("FTPUploadServerURL", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting FTPUploadUsername if missing
                                    If XMLConnectorSettings.XPathSelectElement("FTPUploadUsername") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("FTPUploadServerURL")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("FTPUploadUsername", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting FTPUploadPassword if missing
                                    If XMLConnectorSettings.XPathSelectElement("FTPUploadPassword") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("FTPUploadUsername")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("FTPUploadPassword", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting FTPUploadPath if missing
                                    If XMLConnectorSettings.XPathSelectElement("FTPUploadPath") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("FTPUploadPassword")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("FTPUploadPath", "C:\eShopCONNECT\FTPUpload"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting FTPUploadArchivePath if missing
                                    If XMLConnectorSettings.XPathSelectElement("FTPUploadArchivePath") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("FTPUploadPath")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("FTPUploadArchivePath", "C:\eShopCONNECT\FTPUpload\FilesUploaded"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting SourceItemIDField if missing
                                    If XMLConnectorSettings.XPathSelectElement("SourceItemIDField") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("FTPUploadArchivePath")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("SourceItemIDField", "IT_SOURCECODE"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting ISItemIDField if missing
                                    If XMLConnectorSettings.XPathSelectElement("ISItemIDField") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("SourceItemIDField")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("ISItemIDField", "ItemName"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting Currency if missing
                                    If XMLConnectorSettings.XPathSelectElement("Currency") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("ISItemIDField")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("Currency", strCurrency))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting PricesAreTaxInclusive if missing
                                    If XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("Currency")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("PricesAreTaxInclusive", "No"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting TaxCodeForSourceTax if missing
                                    If XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("PricesAreTaxInclusive")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("TaxCodeForSourceTax", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting DefaultUpliftPercent if missing
                                    If XMLConnectorSettings.XPathSelectElement("DefaultUpliftPercent") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("TaxCodeForSourceTax")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("DefaultUpliftPercent", "0"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting XMLDateFormat if missing
                                    If XMLConnectorSettings.XPathSelectElement("XMLDateFormat") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("DefaultUpliftPercent")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("XMLDateFormat", "MM/DD/YYYY"))
                                        bConfigUpdated = True
                                    End If

                                    ' add ShopDotCom setting AccountDisabled if missing
                                    If XMLConnectorSettings.XPathSelectElement("AccountDisabled") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("XMLDateFormat")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("AccountDisabled", "No"))
                                        bConfigUpdated = True
                                    End If

                                Catch ex As Exception

                                End Try
                            Next

                        Case VOLUSION_SOURCE_CODE
                            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Volusion Order" Then
                                m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceName_DEV000221 = "Volusion Connector"
                                bConfigUpdated = True
                            End If

                            ' start of code added TJS 11/01/14
                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_VOLUSION_LIST)
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' add Volusion setting DefaultShippingMethodID if missing
                                    If XMLConnectorSettings.XPathSelectElement("DefaultShippingMethodID") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("AllowShippingLastNameBlank")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("DefaultShippingMethodID", ""))
                                        bConfigUpdated = True
                                    End If

                                    ' start of code added TJS 15/01/14
                                    ' add Volusion setting EnableSKUAliasLookup if missing
                                    If XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("DefaultShippingMethodID")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("EnableSKUAliasLookup", "No"))
                                        bConfigUpdated = True
                                    End If
                                    ' end of code added TJS 15/01/14

                                    ' start of code added TJS 11/02/14
                                    If XMLConnectorSettings.XPathSelectElement("EnablePaymentTypeTranslation") Is Nothing Then
                                        ' find node which preceeds new node
                                        XMLNodeToAddAfter = XMLConnectorSettings.XPathSelectElement("EnableSKUAliasLookup")
                                        ' add new node
                                        XMLNodeToAddAfter.AddAfterSelf(New XElement("EnablePaymentTypeTranslation", "No"))
                                        bConfigUpdated = True
                                    End If
                                    ' end of code added TJS 11/02/14

                                Catch ex As Exception

                                End Try
                            Next
                            ' end of code added TJS 11/01/14

                            ' start of code added TJS 03/05/13
                        Case AMAZON_FBA_SOURCE_CODE
                            XMLConnectorList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
                            For Each XMLConnectorSettings In XMLConnectorList
                                Try
                                    ' rename Amazon setting MerchantID to MerchantToken if not already done
                                    If XMLConnectorSettings.XPathSelectElement("MerchantID") IsNot Nothing Then
                                        XMLConnectorSettings.XPathSelectElement("MerchantID").Name = "MerchantToken"
                                        bConfigUpdated = True
                                    End If

                                Catch ex As Exception

                                End Try
                            Next
                            ' end of code added TJS 03/05/13

                    End Select
                    If bConfigUpdated Then
                        m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).ConfigSettings_DEV000221 = XMLConfig.ToString
                    End If
                Next

                commandSetUpdate = New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportConfig_DEV000221.TableName, _
                    "CreateLerrynImportExportConfig_DEV000221", "UpdateLerrynImportExportConfig_DEV000221", "DeleteLerrynImportExportConfig_DEV000221"}}
                Me.UpdateDataSet(commandSetUpdate, Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Lerryn System ID created", False)

                ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET ConfigVersion_DEV000221 = '" & _
                    System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion & "', UserModified = '" & _
                    Interprise.Connectivity.Database.Configuration.Design.AppConfig.InterpriseConfiguration.Instance.UserInfo.UserCode() & "', DateModified = getdate()", Nothing)
            End If

        Catch ex As Exception
            If m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count > 0 And XMLConfig IsNot Nothing Then ' TJS 13/11/13
                m_ErrorNotification.SendExceptionEmail(XMLConfig, "CheckAndUpdateConfigSettings", ex) ' TJS 13/11/13
            Else
                m_ErrorNotification.WriteInfoToEventLog("Error in CheckAndUpdateConfigSettings - " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace) ' TJS 13/11/13
            End If
        End Try

    End Sub
#End Region

#Region " ValidateConfigSettings "
    Public Function ValidateConfigSettings(ByVal XMLConfig As XDocument, ByVal ConnectorProductCode As String) As Boolean ' TJS 15/08/09 TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/10/08 | TJS/CG          | 2008.0.01 | Original 
        ' 20/01/09 | TJS             | 2009.1.00 | Corected error when saving ShopDotCom settings 
        '                                        | and added validation for log file settings
        ' 17/02/09 | TJS             | 2009.1.08 | Added validation for new settings and some not previously checked
        ' 11/03/09 | TJS             | 2009.1.09 | Modified to read Customer Business Class names from System Messages 
        '                                        | table and added validation for DefaultPaymentTermGroup and EnableCoupons
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to validate SetDisableFreightCalculation and 
        '                                        | ShippingModuleToUse settings
        ' 21/04/09 | TJS             | 2009.2.03 | Modified to create missing mandatory settings and added 
        '                                        | SetQuantityShippedOnInvoiceImport and SetQuantityShippedOnCreditNoteImport
        ' 11/05/09 | TJS             | 2009.2.06 | Modified to validate IgnoreVoidedOrdersAndInvoices
        ' 17/05/09 | TJS             | 2009.2.06 | Modified to validate AcceptSourceSalesTaxCalculation
        ' 08/06/09 | TJS             | 2009.2.10 | Modified to validate CreateCustomerAsCompany, Shop.com FTP Upload settings,
        '                                        | Currency, DefaultUpliftPercent and DefaultWarehouse
        ' 09/06/09 | TJS             | 2009.2.11 | modified to correct DefaultWarehouse and Currency validation
        ' 10/07/09 | TJS             | 2009.3.00 | Modified to prevent Shop.com PricesAreTaxInclusive being set with 
        '                                        | AcceptSourceSalesTaxCalculation and added FTPUploadPath plus FTPUploadArchivePath
        '                                        | ALso added validation for Volusion connector
        ' 15/08/09 | TJS             | 2009.3.03 | Modified to add ConnectorProductCode as parameter, to cater for 
        '                                        | ESHOPCONNECT_PROSPECT_IMPORT_CONNECTOR_CODE and added 
        '                                        | CopyBillingNameIfShippingNameBlank to Volusion settings
        ' 10/12/09 | TJS             | 2009.3.09 | Added AmazonSite, MerchantName, UserLogin, LoginPassword, 
        '                                        | AmazonManualProcessingPath and PricesAreTaxInclusive plus
        '                                        | Channel Advisor config
        ' 30/12/09 | TJS             | 2010.0.00 | Added validation of Amazon and Channel Advisor Payment Type
        ' 05/01/10 | TJs             | 2010.0.01 | Renamed PROSPECT_IMPORT_CONNECTOR_CODE to reflect its use in Order importer
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for Magento and ASLDotNetStoreFront connector 
        '                                        | settings and to use common functions
        ' 22/09/10 | TJS             | 2010.1.01 | Modified to cater for Channel Advisor Payment Translation
        ' 04/10/10 | TJS             | 2010.1.05 | Removed Import as Order option from Channel ADvisor ActionIfNoPayment congif setting
        ' 07/01/11 | TJs             | 2010.1.15 | REmoved validation of NotificationEmailIISConfigSource
        '                                        | plus EnablePollForOrders from Magento and ASPStorefront
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to prevent : being entered in Magento Instance ID
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for ValidateConnectorConfigOptionSetting allowing a variable number of option values
        '                                        | and for Amazon MWS connection parameters
        ' 26/10/11 | TJS             | 2011.1.xx | Modified for TaxCodeForSourceTax
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and added eBay settings validation
        ' 16/01/12 | TJS             | 2010.2.02 | Added Sears.com settings validation
        ' 23/01/12 | TJS             | 2010.2.03 | Corrected ebay TokenExpires validation
        ' 26/01/12 | TJS             | 2010.2.05 | Corrected detection of Ebay and SearsDotCom groups
        ' 14/02/12 | TJS             | 2011.2.06 | Added ProductListBlockSize to Magento settings
        ' 16/06/12 | TJS             | 2012.1.07 | Modified to cater for different Amazon developerids in different countries
        ' 05/07/12 | TJS             | 2012.1.08 | Modified for Amazon FBA
        ' 08/07/12 | TJS             | 2012.1.09 | Modified to cater for UseShipToClassTemplate
        ' 18/01/13 | TJS             | 2012.1.17 | Modified to cater for AllocateAndReserveStock
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 30/04/13 | TJS             | 2013.1.11 | Added Magento InhibitInventoryUpdates and CreateCustomerForGuestCheckout
        ' 08/05/13 | TJS             | 2013.1.13 | Added Magento IncludeChildItemsOnOrder
        ' 20/05/13 | TJS             | 2013.1.15 | Modified to only force use of Amazon Own Access Keys outside UK and US
        ' 12/06/13 | TJS             | 2013.1.19 | Modified to only force use of Amazon Own Access Keys when site selected
        '                                        | and correct Amazon AWS to MWS
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to prevent error if ConfigSettingValue is null
        ' 19/09/13 | TJS             | 2013.3.00 | Added generic ImportMissingItemsAsNonStock
        ' 02/10/13 | TJS             | 2013.3.03 | Added MagentoVersion
        ' 13/11/13 | TJS             | 2013.3.08 | Added Magento V2SoapAPIWSICompliant, LerrynAPIVersion and UpdateMagentoSpecialPricing
        ' 20/11/13 | TJS             | 2013.4.00 | Added 3DCart connector settings
        ' 11/01/14 | TJS             | 2013.4.04 | Added Volusion DefaultShippingMethodID
        ' 15/01/14 | TJS             | 2013.4.05 | Added Volusion EnableSKUAliasLookup
        ' 11/02/14 | TJS             | 2013.4.09 | added Volusion EnablePaymentTypeTranslation
        ' 01/05/14 | TJS             | 2014.0.02 | Added Magento CardAuthAndCaptureWithOrder
        '------------------------------------------------------------------------------------------

        Dim rowGroupMethodDetail As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.SystemShippingMethodGroupDetailRow
        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim rowConfigSettings2 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim strTemp As String, strBusinessTypeWholesale As String, strBusinessTypeRetail As String, iTemp As Integer ' TJS 11/03/09
        Dim iLoop As Integer, iCheckLoop As Integer, bConfigGroupActive As Boolean, bConfigValid As Boolean, bValueValid As Boolean
        Dim bShopComPublishActive As Boolean, bSettingReqd As Boolean, bUseShipToClassTemplate As Boolean ' TJS 08/06/09 TJS 19/08/10 TJS 08/07/12

        bConfigValid = True

        ' is UseShipToClassTemplate set to Yes ?
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "UseShipToClassTemplate") ' TJS 08/07/12
        If rowConfigSettings1 IsNot Nothing AndAlso rowConfigSettings1.ConfigSettingValue.ToUpper = "YES" Then ' TJS 08/07/12
            bUseShipToClassTemplate = True ' TJS 08/07/12
        Else
            bUseShipToClassTemplate = False ' TJS 08/07/12
        End If

        ' check send Code error emails flag
        If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
            m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_SEND_CODE_ERROR_EMAILS_TO_LERRYN, "SendCodeErrorEmailsToLerryn", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If

        ' check send Source error emails flag
        If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
            m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_SEND_SOURCE_ERROR_EMAILS_TO_LERRYN, "SendSourceErrorEmailsToLerryn", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If

        ' check notification email address
        If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
            m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/07/12
            bValueValid = True
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "ErrorNotificationEmailAddress")
            strTemp = Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_NOTIFICATION_EMAIL_ADDRESS)
            iTemp = strTemp.IndexOf("@")
            If iTemp < 0 Then
                bValueValid = False
            Else
                iTemp = strTemp.Substring(iTemp).IndexOf(".")
                If iTemp < 0 Then
                    bValueValid = False
                End If
            End If
            If Not bValueValid Then
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "ErrorNotificationEmailAddress" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Not a valid email address")
                bConfigValid = False
            Else
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            End If
        End If

        ' check customer business type
        If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
            m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/07/12
            strBusinessTypeWholesale = Me.GetMessage("LBL0022") ' TJS 11/03/09
            strBusinessTypeRetail = Me.GetMessage("LBL0023") ' TJS 11/03/09
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "CustomerBusinessType")
            If Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_CUSTOMER_BUSINESS_TYPE) <> strBusinessTypeWholesale And _
                Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_CUSTOMER_BUSINESS_TYPE) <> strBusinessTypeRetail Then ' TJS 11/03/09
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "CustomerBusinessType" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be " & strBusinessTypeRetail & " or " & strBusinessTypeWholesale) ' TJS 11/03/09
                bConfigValid = False
            Else
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            End If
        End If

        ' check customer business class
        If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
            m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/07/12
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "CustomerBusinessClass")
            Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.CustomerClassTemplateDetailView.TableName, _
                "ReadCustomerClassTemplateDetailView_DEV000221", "@ClassDescription", Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_CUSTOMER_BUSINESS_CLASS)}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            If Me.m_ImportExportDataset.CustomerClassTemplateDetailView.Count = 0 Then
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "CustomerBusinessClass" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Invalid Customer Business Class")
                bConfigValid = False
            Else
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            End If
        End If

        ' start of code added TJS 08/06/09
        ' check Create Customer As Company flag
        If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or
            m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_CREATE_CUSTOMER_AS_COMPANY, "CreateCustomerAsCompany", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If
        ' end of code added TJS 08/06/09

        ' check Shipping Module To Use
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            ' start of code added TJS 17/03/09
            If Not ValidateCoreConfigOptionSetting(XMLConfig, SOURCE_CONFIG_SHIPPING_MODULE_TO_USE, "ShippingModuleToUse", "Interprise Suite basic", "KSI MultiShip", False) Then ' TJS 19/08/10 TJS 07/01/11
                bConfigValid = False ' TJS 19/08/10
            End If
            ' end of code added TJS 17/03/09
        End If

        ' check enable Delivery Method translation flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
                m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "EnableDeliveryMethodTranslation")
            If Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper <> "YES" And _
                    Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper <> "NO" Then
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "EnableDeliveryMethodTranslation" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be Yes or No")
                bConfigValid = False
            ElseIf Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_SHIPPING_MODULE_TO_USE) = "KSI MultiShip" And _
                Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_ENABLE_DELIVERY_METHOD_TRANSLATION).ToUpper <> "NO" Then ' TJS 17/03/09
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "EnableDeliveryMethodTranslation" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Delivery Method translation not valid with KSI MultiShip") ' TJS 17/03/09
                bConfigValid = False ' TJS 17/03/09
            Else
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            End If
        End If

        ' check default shipping method and shipping method group
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.SystemShippingMethodGroupDetail.TableName, _
              "ReadSystemShippingMethodGroupDetailView_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        rowGroupMethodDetail = Me.m_ImportExportDataset.SystemShippingMethodGroupDetail.FindByShippingMethodGroupShippingMethodCode(Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD_GROUP), Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_SHIPPING_METHOD))
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultShippingMethod")
        rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultShippingMethodGroup")
        If rowGroupMethodDetail Is Nothing And Not bUseShipToClassTemplate Then ' TJS 08/07/12
            If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                rowConfigSettings1.ConfigSettingName = "DefaultShippingMethod" ' TJS 21/04/09
                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
            End If
            rowConfigSettings1.SetColumnError("ConfigSettingName", "Invalid Shipping Method and Shipping Method Group combination")
            If rowConfigSettings2 Is Nothing Then ' TJS 21/04/09
                rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                rowConfigSettings2.ConfigGroup = "General" ' TJS 21/04/09
                rowConfigSettings2.ConfigSettingName = "DefaultShippingMethodGroup" ' TJS 21/04/09
                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings2) ' TJS 21/04/09
            End If
            rowConfigSettings2.SetColumnError("ConfigSettingName", "Invalid Shipping Method and Shipping Method Group combination")
            bConfigValid = False
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
            If rowConfigSettings2 IsNot Nothing Then
                rowConfigSettings2.ClearErrors()
            End If
        End If

        ' start of code added TJS 08/06/09
        ' check Default Warehouse
        If m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10
            Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.InventoryWarehouseView.TableName, _
                "ReadInventoryWarehouse", AT_WAREHOUSE_CODE, Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_WAREHOUSE).ToUpper}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultWarehouse")
            If Me.m_ImportExportDataset.InventoryWarehouseView.Count = 0 And Not bUseShipToClassTemplate Then  ' TJS 09/06/09 TJS 08/07/12
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = "General"
                    rowConfigSettings1.ConfigSettingName = "DefaultWarehouse"
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Invalid Default Warehouse")
                bConfigValid = False
            Else
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            End If
        End If
        ' end of code added TJS 08/06/09

        ' check Default Payment Term Group
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.SystemPaymentTermGroupDetail.TableName, _
                "ReadSystemPaymentTermGroupDetail_DEV000221", "@PaymentTermGroup", Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_PAYMENT_TERM_GROUP).ToUpper}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 11/03/09
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DefaultPaymentTermGroup") ' TJS 11/03/09
            If Me.m_ImportExportDataset.SystemPaymentTermGroupDetail.Count = 0 Then ' TJS 11/03/09
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "DefaultPaymentTermGroup" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Invalid Default Payment Term Group") ' TJS 11/03/09
                bConfigValid = False ' TJS 11/03/09
            Else
                If rowConfigSettings1 IsNot Nothing Then ' TJS 11/03/09
                    rowConfigSettings1.ClearErrors() ' TJS 11/03/09
                End If
            End If
        End If

        ' check Credit Card Payment Term Code
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.SystemPaymentTermGroupDetail.TableName, _
                "ReadSystemPaymentTermGroupDetail_DEV000221", "@PaymentTermGroup", Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_PAYMENT_TERM_GROUP).ToUpper, _
                "@PaymentTermCode", Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_DEFAULT_CREDIT_CARD_PAYMENT_TERM)}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 17/02/09 TJS 11/03/09
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "CreditCardPaymentTermCode") ' TJS 17/02/09
            If Me.m_ImportExportDataset.SystemPaymentTermGroupDetail.Count = 0 Then ' TJS 17/02/09
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "CreditCardPaymentTermCode" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Invalid Credit Card Payment Term Code") ' TJS 17/02/09
                bConfigValid = False ' TJS 17/02/09
            Else
                If rowConfigSettings1 IsNot Nothing Then ' TJS 17/02/09
                    rowConfigSettings1.ClearErrors() ' TJS 17/02/09
                End If
            End If
        End If

        ' check Due Days in Future value
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) Then ' TJS 05/07/12
            bValueValid = True
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "DueDateDaysInFuture")
            strTemp = Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_DUE_DATE_OFFSET)
            ' is value numeric ?
            If Not IsNumeric(strTemp) Then ' TJS 17/02/09
                bValueValid = False
            Else
                ' yes, is it an integer value ?
                If CDbl(CInt(strTemp)) <> CDbl(strTemp) Then
                    ' no
                    bValueValid = False
                End If
            End If
            If Not bValueValid Then
                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                    rowConfigSettings1.ConfigGroup = "General" ' TJS 21/04/09
                    rowConfigSettings1.ConfigSettingName = "DueDateDaysInFuture" ' TJS 21/04/09
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be Integer value")
                bConfigValid = False
            Else
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            End If
        End If

        ' check Authorise Card flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_AUTHORISE_CARD_ON_IMPORT, "AuthoriseCreditCardOnImport", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If

        ' start of code added TJS 10/07/09
        ' check External System Card Payment Code
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.SystemPaymentTypeView.TableName, _
                "ReadSystemPaymentType", AT_PAYMENT_TYPE_CODE, Me.m_ImportExportRule.GetXMLElementText(XMLConfig, SOURCE_CONFIG_EXT_SYSTEM_CARD_PAYMENT_CODE)}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "ExternalSystemCardPaymentCode")
            If Me.m_ImportExportDataset.SystemPaymentTermGroupDetail.Count = 0 Then
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = "General"
                    rowConfigSettings1.ConfigSettingName = "ExternalSystemCardPaymentCode"
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Invalid External System Card Payment Code")
                bConfigValid = False
            Else
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            End If
        End If
        ' end of code added TJS 10/07/09

        ' check Enable Coupons flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_ENABLE_COUPONS, "EnableCoupons", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If

        ' check Require Source Customer ID flag
        If (m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
            m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE Then ' TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_REQUIRE_SOURCE_CUSTOMER_ID, "RequireSourceCustomerID", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If

        ' start of code added TJS 13/01/10
        ' check Allow Blank Postal Code flag
        ' Allow Blank Postal Code can be null if config row does not exist
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "AllowBlankPostalcode")
        If rowConfigSettings1 IsNot Nothing Then ' TJS 19/08/10
            bSettingReqd = True ' TJS 19/08/10
        Else
            bSettingReqd = False ' TJS 19/08/10
        End If
        If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_ALLOW_BLANK_POSTALCODE, "AllowBlankPostalcode", bSettingReqd) Then ' TJS 19/08/10
            bConfigValid = False ' TJS 19/08/10
        End If
        ' end of code added TJS 13/01/10

        ' start of code added TJS 17/03/09
        ' check Set Disable Freight Calculation flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_SET_DISABLE_FREIGHT_CALCULATION, "SetDisableFreightCalculation", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If
        ' end of code added TJS 17/03/09

        ' start of code added TJS 11/05/09
        ' check Ignore Voided Orders AndI nvoices flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_IGNORE_VOIDED_ORDERS_AND_INVOICES, "IgnoreVoidedOrdersAndInvoices", True) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If
        ' end of code added TJS 11/05/09

        ' start of code added TJS 17/05/09
        ' check Accept Source Sales Tax Calculation flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10 TJS 05/07/12
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_ACCEPT_SOURCE_SALES_TAX_CALCULATION, "AcceptSourceSalesTaxCalculation", True) Then ' TJS 19/08/10 
                bConfigValid = False ' TJS 19/08/10
            End If
        End If
        ' end of code added TJS 17/05/09

        ' start of code added TJS 19/09/13
        ' check Import Missing Items As NonStock flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_IMPORT_MISSING_ITEMS_AS_NONSTOCK, "ImportMissingItemsAsNonStock", True) Then
                bConfigValid = False
            End If
        End If
        ' end of code added TJS 19/09/13

        ' start of code added TJS 18/01/13
        ' check Allocate And Reserve Stock flag
        If (m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE) And _
            ((m_BaseProductCode = ESHOPCONNECT_BASE_PRODUCT_CODE And ConnectorProductCode <> AMAZON_FBA_CONNECTOR_CODE) Or _
             m_BaseProductCode <> ESHOPCONNECT_BASE_PRODUCT_CODE) Then
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_ALLOCATE_AND_RESERVE_STOCK, "AllocateAndReserveStock", True) Then
                bConfigValid = False
            End If
        End If
        ' end of code added TJS 18/01/13

        ' check Poll Generic Import Path flag
        If m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10
            ' Allow Poll Generic Path as null if config row does not exist
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "PollGenericImportPath")
            If rowConfigSettings1 IsNot Nothing Then ' TJS 19/08/10
                bSettingReqd = True ' TJS 19/08/10
            Else
                bSettingReqd = False ' TJS 19/08/10
            End If
            If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_POLL_GENERIC_IMPORT_PATH, "PollGenericImportPath", bSettingReqd) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If

        ' check Generic Import Path 
        If m_BaseProductCode <> PROSPECTIMPORTER_BASE_PRODUCT_CODE And ConnectorProductCode <> PROSPECT_IMPORT_CONNECTOR_CODE Then ' TJS 10/07/09 TJS 15/08/09 TJS 05/01/10
            ' Allow Generic Path etc as null if config row does not exist or PollGenericImportPath not set to yes
            rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "PollGenericImportPath") ' TJS 08/07/12
            If rowConfigSettings1 IsNot Nothing Then ' TJS 08/07/12
                If Not rowConfigSettings1.IsConfigSettingValueNull AndAlso rowConfigSettings1.ConfigSettingValue.ToUpper = "YES" Then ' TJS 08/07/12
                    bSettingReqd = True ' TJS 08/07/12
                Else
                    bSettingReqd = False ' TJS 08/07/12
                End If
            Else
                bSettingReqd = False ' TJS 19/08/10
            End If
            If Not ValidateCoreConfigStringSetting(XMLConfig, SOURCE_CONFIG_GENERIC_IMPORT_PATH, "GenericImportPath", 250, bSettingReqd) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If

            ' check Generic Import Processed Path 
            If Not ValidateCoreConfigStringSetting(XMLConfig, SOURCE_CONFIG_GENERIC_IMPORT_PROCESSED_PATH, "GenericImportProcessedPath", 250, bSettingReqd) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If

            ' check Generic Import Error Path 
            If Not ValidateCoreConfigStringSetting(XMLConfig, SOURCE_CONFIG_GENERIC_IMPORT_ERROR_PATH, "GenericImportErrorPath", 250, bSettingReqd) Then ' TJS 19/08/10
                bConfigValid = False ' TJS 19/08/10
            End If
        End If

        ' Start of Code Added TJS 20/01/09
        ' check Enable Log File flag
        ' Allow Enable Log File as null if config row does not exist
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "EnableLogFile") ' TJS 19/08/10
        If rowConfigSettings1 IsNot Nothing Then ' TJS 19/08/10
            bSettingReqd = True ' TJS 19/08/10
        Else
            bSettingReqd = False ' TJS 19/08/10
        End If
        If Not ValidateCoreConfigYesNoSetting(XMLConfig, SOURCE_CONFIG_ENABLE_LOG_FILE, "EnableLogFile", bSettingReqd) Then ' TJS 19/08/10
            bConfigValid = False ' TJS 19/08/10
        End If

        ' check Log File Path 
        ' Allow Log File Path as null if config row does not exist or EnableLogFile not set to yes
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "EnableLogFile") ' TJS 08/07/12
        If rowConfigSettings1 IsNot Nothing Then ' TJS 08/07/12
            If Not rowConfigSettings1.IsConfigSettingValueNull AndAlso rowConfigSettings1.ConfigSettingValue.ToUpper = "YES" Then ' TJS 08/07/12
                bSettingReqd = True ' TJS 08/07/12
            Else
                bSettingReqd = False ' TJS 08/07/12
            End If
        Else
            bSettingReqd = False ' TJS 19/08/10
        End If
        If Not ValidateCoreConfigStringSetting(XMLConfig, SOURCE_CONFIG_LOG_FILE_PATH, "LogFilePath", 250, bSettingReqd) Then ' TJS 19/08/10
            bConfigValid = False ' TJS 19/08/10
        End If
        ' End of Code Added TJS 20/01/09

        ' start of Code Added TJS 19/08/10
        ' is Disable Shop.Com Publishing set - Only included in PetMeds system as standard ?
        bShopComPublishActive = True
        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
            If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup = "ShopDotCom" And _
                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = "DisableShopComPublishing" Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue.ToUpper = "YES" Then
                    bShopComPublishActive = False
                End If
            End If
        Next
        ' End of Code Added TJS 19/08/10

        ' check for Connector settings
        For iLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
            If Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 6) = "Amazon" And _
                Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 9) <> "AmazonFBA" Then ' TJS 17/02/09 TJS 05/07/12
                ' Start of Code Added TJS 11/03/09
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "MerchantToken" Then ' TJS 22/03/13
                    ' Merchant Token (Was Merchant ID)
                    If Not ValidateConnectorConfigStringSetting(iLoop, "MerchantToken", 30, True) Then ' TJS 19/08/10 TJS 22/03/13
                        bConfigValid = False ' TJS 19/08/10
                    Else
                        ' start of code added TJS 02/12/11
                        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                            If iLoop <> iCheckLoop Then
                                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                    Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                    Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    bConfigValid = False
                                End If
                            End If
                        Next
                        ' end of code added TJS 02/12/11
                    End If

                    ' start of code added TJS 10/12/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AmazonSite" Then
                    ' Amazon Site
                    If Not ValidateConnectorConfigOptionSetting(iLoop, "AmazonSite", True, New String() {".com", ".co.uk", ".ca", ".de", ".fr", ".jp", ".com.cn"}) Then ' TJS 19/08/10 TJS 09/07/11
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 10/12/09

                    ' Start of code added TJS 16/06/12
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OwnAccessKeyID" Then
                    ' Allow Own Access Key ID as null if amazon site is .co.uk unless OwnSecretAccessKey is set
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, "AmazonSite")
                    rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, "OwnSecretAccessKey")
                    If (rowConfigSettings1 IsNot Nothing AndAlso Not rowConfigSettings1.IsConfigSettingValueNull AndAlso _
                        rowConfigSettings1.ConfigSettingValue <> ".co.uk" AndAlso rowConfigSettings1.ConfigSettingValue <> ".com" AndAlso _
                        rowConfigSettings1.ConfigSettingValue <> "") Or (rowConfigSettings2 IsNot Nothing AndAlso _
                        Not rowConfigSettings2.IsConfigSettingValueNull AndAlso rowConfigSettings2.ConfigSettingValue <> "") Then ' TJS 20/05/13 TJS 12/06/13 TJS 09/08/13
                        bSettingReqd = True
                    Else
                        bSettingReqd = False
                    End If
                    If Not ValidateConnectorConfigStringSetting(iLoop, "OwnAccessKeyID", 99, bSettingReqd) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OwnSecretAccessKey" Then
                    ' Allow Own Secret Access Key as null if amazon site is .co.uk unless OwnAccessKeyID is set
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, "AmazonSite")
                    rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, "OwnAccessKeyID")
                    If (rowConfigSettings1 IsNot Nothing AndAlso Not rowConfigSettings1.IsConfigSettingValueNull AndAlso _
                        rowConfigSettings1.ConfigSettingValue <> ".co.uk" AndAlso rowConfigSettings1.ConfigSettingValue <> ".com" AndAlso _
                        rowConfigSettings1.ConfigSettingValue <> "") Or (rowConfigSettings2 IsNot Nothing AndAlso _
                        Not rowConfigSettings2.IsConfigSettingValueNull AndAlso rowConfigSettings2.ConfigSettingValue <> "") Then ' TJS 20/05/13 TJS 12/06/13 TJS 09/08/13
                        bSettingReqd = True
                    Else
                        bSettingReqd = False
                    End If
                    If Not ValidateConnectorConfigStringSetting(iLoop, "OwnSecretAccessKey", 99, bSettingReqd) Then
                        bConfigValid = False
                    End If
                    ' End of code added TJS 16/06/12

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "MerchantName" Then ' TJS 10/12/09
                    ' Merchant Legal Name
                    If Not ValidateConnectorConfigStringSetting(iLoop, "MerchantName", 99, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "MWSMerchantID" Then ' TJS 10/12/09 TJS 09/07/11 TJS 12/06/13
                    ' User Login to Amazon
                    If Not ValidateConnectorConfigStringSetting(iLoop, "MWSMerchantID", 99, True) Then ' TJS 19/08/10 TJS 12/06/13
                        bConfigValid = False ' TJS 19/08/10
                    End If

                    ' start of code added TJS 10/12/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "MWSMarketplaceID" Then ' TJS 09/07/11 TJS 12/06/13
                    ' Password for Login to Amazon
                    If Not ValidateConnectorConfigStringSetting(iLoop, "MWSMarketplaceID", 99, True) Then ' TJS 19/08/10 TJS 12/06/13
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AmazonManualProcessingPath>" Then
                    ' Path for saving files requiring manual processing
                    If Not ValidateConnectorConfigStringSetting(iLoop, "AmazonManualProcessingPath", 250, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 10/12/09

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AmazonImportProcessedPath" Then
                    ' Amazon Import Processed Path
                    If Not ValidateConnectorConfigStringSetting(iLoop, "AmazonImportProcessedPath", 250, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AmazonImportErrorPath" Then
                    ' AmazonImport Error Path
                    If Not ValidateConnectorConfigStringSetting(iLoop, "AmazonImportErrorPath", 250, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' ISItemIDField
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                    ' start of code added TJS 30/12/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PaymentType" Then
                    ' Send Prices As Tax Inclusive
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop) ' TJS 19/08/10
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.GetField("SELECT IsActive FROM SystemPaymentType WHERE PaymentTypeCode = '" & Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing) = "True" Then
                            If rowConfigSettings1 IsNot Nothing Then
                                rowConfigSettings1.ClearErrors()
                            End If
                        Else
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = "Amazon"
                                rowConfigSettings1.ConfigSettingName = "PaymentType"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be valid and active Payment Type")
                            bConfigValid = False
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If
                    ' end of code added TJS 30/12/09

                    ' start of code added TJS 10/12/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PricesAreTaxInclusive" Then
                    ' Send Prices As Tax Inclusive
                    If Not ValidateConnectorConfigSendPricesAsTaxIncl(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 10/12/09

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "TaxCodeForSourceTax" Then ' TJS 26/10/11
                    ' Tax Code For Source Tax
                    If Not ValidateConnectorConfigTaxCodeForSourceTax(iLoop) Then ' TJS 26/10/11
                        bConfigValid = False ' TJS 26/10/11
                    End If

                    ' start of code added TJS 08/06/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "DefaultUpliftPercent" Then
                    ' check Default Uplift Percent value
                    If Not ValidateConnectorConfigDefaultUpliftPercent(iLoop, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 08/06/09

                    ' start of code added TJS 19/08/10
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnableSKUAliasLookup" Then
                    ' Enable SKU Alias Lookup
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnableSKUAliasLookup", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If
                    ' end of code added TJS 19/08/10

                End If
                ' End of Code Added TJS 11/03/09

            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 10) = "ShopDotCom" Then ' TJS 17/02/09
                ' Start of Code Added TJS 17/02/09
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "CatalogID" Then
                    ' Catalog ID
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop) ' TJS 19/08/10
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "" Then
                            bValueValid = True
                            strTemp = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue
                            ' is value numeric ?
                            If Not IsNumeric(strTemp) Then
                                bValueValid = False
                            Else
                                ' yes, is it an integer value ?
                                If CDbl(CInt(strTemp)) <> CDbl(strTemp) Then
                                    ' no
                                    bValueValid = False
                                End If
                            End If
                            If Not bValueValid Then
                                If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                                    rowConfigSettings1.ConfigGroup = "ShopDotCom" ' TJS 21/04/09
                                    rowConfigSettings1.ConfigSettingName = "CatalogID" ' TJS 21/04/09
                                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                                End If
                                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be Integer value")
                                bConfigValid = False
                            Else
                                If rowConfigSettings1 IsNot Nothing Then
                                    rowConfigSettings1.ClearErrors()
                                End If
                            End If
                        Else
                            If rowConfigSettings1 Is Nothing Then ' TJS 21/04/09
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow ' TJS 21/04/09
                                rowConfigSettings1.ConfigGroup = "ShopDotCom" ' TJS 21/04/09
                                rowConfigSettings1.ConfigSettingName = "CatalogID" ' TJS 21/04/09
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1) ' TJS 21/04/09
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                            bConfigValid = False
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "StatusPostURL" Then
                    ' Status Post URL
                    If Not ValidateConnectorConfigStringSetting(iLoop, "StatusPostURL", 250, bShopComPublishActive) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                    ' start of code added TJS 08/06/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "FTPUploadServerURL" Then
                    ' FTP Upload Server URL
                    If Not ValidateConnectorConfigStringSetting(iLoop, "StatusPostURL", 250, bShopComPublishActive) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "FTPUploadUsername" Then
                    ' FTP Upload Username
                    If Not ValidateConnectorConfigStringSetting(iLoop, "StatusPostURL", 99, bShopComPublishActive) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "FTPUploadPassword" Then
                    ' FTP Upload Password
                    If Not ValidateConnectorConfigStringSetting(iLoop, "FTPUploadPassword", 99, bShopComPublishActive) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 08/06/09

                    ' start of code added TJS 16/06/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "FTPUploadPath" Then
                    ' FTP Upload Password
                    If Not ValidateConnectorConfigStringSetting(iLoop, "FTPUploadPath", 250, bShopComPublishActive) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "FTPUploadArchivePath" Then
                    ' FTP Upload Password
                    If Not ValidateConnectorConfigStringSetting(iLoop, "FTPUploadArchivePath", 250, bShopComPublishActive) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "SourceItemIDField" Then
                    ' SourceItemIDField
                    If Not ValidateConnectorConfigOptionSetting(iLoop, "AmazonSite", True, New String() {"IT_SKU", "IT_SOURCECODE"}) Then ' TJS 19/08/10 TJS 09/07/11
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' ISItemIDField
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                    ' start of code added TJS 08/06/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "Currency" Then
                    ' Currency
                    If Not ValidateConnectorConfigCurrency(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PricesAreTaxInclusive" Then
                    ' Send Prices As Tax Inclusive
                    If Not ValidateConnectorConfigSendPricesAsTaxIncl(iLoop) Then ' TJS 19/08/10 TJS 22/09/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "TaxCodeForSourceTax" Then ' TJS 26/10/11
                    ' Tax Code For Source Tax
                    If Not ValidateConnectorConfigTaxCodeForSourceTax(iLoop) Then ' TJS 26/10/11
                        bConfigValid = False ' TJS 26/10/11
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "DisableShopComPublishing" Then
                    ' Disable Shop.Com Publishing - Only included in PetMeds system as standard
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "DisableShopComPublishing", False) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "DefaultUpliftPercent" Then
                    ' check Default Uplift Percent value
                    If Not ValidateConnectorConfigDefaultUpliftPercent(iLoop, bShopComPublishActive) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 08/06/09

                    ' start of code added TJS 22/06/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "XMLDateFormat" Then
                    ' XML Date Format
                    If Not ValidateConnectorConfigOptionSetting(iLoop, "AmazonSite", True, New String() {"MM/DD/YYYY", "DD/MM/YYYY"}) Then ' TJS 19/08/10 TJS 09/07/11
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 22/06/09

                    ' start of code added TJS 19/08/10
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If
                    ' end of code added TJS 19/08/10
                End If
                ' End of Code Added TJS 17/02/09

            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 8) = "Volusion" Then ' TJS 22/06/09
                ' Start of Code Added TJS 22/06/09
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "SiteID" Then
                    ' Site ID
                    If Not ValidateConnectorConfigStringSetting(iLoop, "SiteID", 30, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    Else
                        ' start of code added TJS 02/12/11
                        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                            If iLoop <> iCheckLoop Then
                                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                    Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                    Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    bConfigValid = False
                                End If
                            End If
                        Next
                        ' end of code added TJS 02/12/11
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OrderPollURL" Then
                    ' Order Poll URL
                    If Not ValidateConnectorConfigStringSetting(iLoop, "OrderPollURL", 250, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OrderPollIntervalMinutes" Then
                    ' check Order Poll Interval Minutes value
                    If Not ValidateConnectorConfigOrderPollIntMins(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' ISItemIDField
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PricesAreTaxInclusive" Then ' TJS 26/10/11
                    ' Send Prices As Tax Inclusive
                    If Not ValidateConnectorConfigSendPricesAsTaxIncl(iLoop) Then ' TJS 26/10/11
                        bConfigValid = False ' TJS 26/10/11
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "TaxCodeForSourceTax" Then ' TJS 26/10/11
                    ' Tax Code For Source Tax
                    If Not ValidateConnectorConfigTaxCodeForSourceTax(iLoop) Then ' TJS 26/10/11
                        bConfigValid = False ' TJS 26/10/11
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "Currency" Then
                    ' Currency
                    If Not ValidateConnectorConfigCurrency(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                    ' start of code added TJS 15/08/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AllowShippingLastNameBlank" Then
                    ' Allow Shipping Last Name Blank
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AllowShippingLastNameBlank", True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If
                    ' end of code added TJS 15/08/09

                    ' end of code added TJS 11/01/14
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "DefaultShippingMethodID" Then
                    ' check Default Shipping Method ID value
                    If Not ValidateConnectorConfigIntegerValue(iLoop, "DefaultShippingMethodID") Then
                        bConfigValid = False
                    End If
                    ' end of code added TJS 11/01/14

                    ' start of code added TJS 15/01/14
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnableSKUAliasLookup" Then
                    ' Enable SKU Alias Lookup
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnableSKUAliasLookup", True) Then
                        bConfigValid = False
                    End If
                    ' end of code added TJS 15/01/14

                    ' start of code added TJS 11/02/14
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnablePaymentTypeTranslation" Then
                    ' Enable Payment Type Translation
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnablePaymentTypeTranslation", True) Then
                        bConfigValid = False
                    End If
                    ' end of code added TJS 11/02/14

                    ' start of code added TJS 19/08/10
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If
                    ' end of code added TJS 19/08/10
                End If
                ' end of code added TJS 22/06/09

            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 14) = "ChannelAdvisor" Then ' TJS 10/12/09
                ' Start of Code Added TJS 10/12/09
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountName" Then
                    ' Account Name
                    If Not ValidateConnectorConfigStringSetting(iLoop, "AccountName", 99, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountID" Then
                    ' Account Name
                    If Not ValidateConnectorConfigStringSetting(iLoop, "AccountID", 99, True) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    Else
                        ' start of code added TJS 02/12/11
                        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                            If iLoop <> iCheckLoop Then
                                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                    Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                    Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    bConfigValid = False
                                End If
                            End If
                        Next
                        ' end of code added TJS 02/12/11
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' IS Item ID Field
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "Currency" Then
                    ' Currency
                    If Not ValidateConnectorConfigCurrency(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                    ' start of code added TJS 30/12/09
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PaymentType" Then
                    ' Payment Type
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop) ' TJS 19/08/10
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, "EnablePaymentTypeTranslation") ' TJS 22/09/10
                    If rowConfigSettings2 IsNot Nothing Then ' TJS 22/09/10
                        strTemp = rowConfigSettings2.Item("ConfigSettingValue").ToString ' TJS 22/09/10
                    Else
                        strTemp = "NO" ' TJS 22/09/10
                    End If
                    If bConfigGroupActive Then
                        If Me.GetField("SELECT IsActive FROM SystemPaymentType WHERE PaymentTypeCode = '" & Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing) = "True" Then
                            If rowConfigSettings1 IsNot Nothing Then
                                rowConfigSettings1.ClearErrors()
                            End If
                        ElseIf strTemp.ToUpper <> "YES" Then ' TJS 22/09/10
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = "ChannelAdvisor"
                                rowConfigSettings1.ConfigSettingName = "PaymentType"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be valid and active Payment Type")
                            bConfigValid = False
                        Else
                            If rowConfigSettings1 IsNot Nothing Then ' TJS 22/09/10
                                rowConfigSettings1.ClearErrors() ' TJS 22/09/10
                            End If
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If
                    ' end of code added TJS 30/12/09

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnablePaymentTypeTranslation" Then ' TJS 22/09/10
                    ' Enable Payment Type Translation
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnablePaymentTypeTranslation", True) Then ' TJS 22/09/10
                        bConfigValid = False ' TJS 22/09/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PricesAreTaxInclusive" Then
                    ' Send Prices As Tax Inclusive
                    If Not ValidateConnectorConfigSendPricesAsTaxIncl(iLoop) Then ' TJS 19/08/10 TJS 22/09/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "TaxCodeForSourceTax" Then ' TJS 26/10/11
                    ' Tax Code For Source Tax
                    If Not ValidateConnectorConfigTaxCodeForSourceTax(iLoop) Then ' TJS 26/10/11
                        bConfigValid = False ' TJS 26/10/11
                    End If

                    ' start of code added TJS 19/08/10
                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnableSKUAliasLookup" Then
                    ' Enable SKU Alias Lookup
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnableSKUAliasLookup", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ActionIfNoPayment" Then
                    ' Action If No Payment
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop)
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "Ignore" And _
                                Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "Import as Quote" Then ' TJS 04/10/10
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup
                                rowConfigSettings1.ConfigSettingName = "ActionIfNoPayment"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be Ignore or Import as Quote") ' TJS 04/10/10
                            bConfigValid = False
                        Else
                            If rowConfigSettings1 IsNot Nothing Then
                                rowConfigSettings1.ClearErrors()
                            End If
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If
                    ' end of code added TJS 19/08/10
                End If
                ' end of code added TJS 10/12/09

                ' start of code added TJS 19/08/10
            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 7) = "Magento" Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "InstanceID" Then ' TJS 18/03/11
                    ' Site ID
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop)
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "" Then
                            If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Length <= 15 Then
                                If InStr(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue, ":") = 0 Then
                                    ' start of code added TJS 02/12/11
                                    For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                                        If iLoop <> iCheckLoop Then
                                            If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                                Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                                Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                                bConfigValid = False
                                            End If
                                        End If
                                    Next
                                    ' end of code added TJS 02/12/11

                                    If rowConfigSettings1 IsNot Nothing And bConfigValid Then ' TJS 02/12/11
                                        rowConfigSettings1.ClearErrors()
                                    End If
                                Else
                                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not contain : characters") ' TJS 18/03/11
                                    bConfigValid = False
                                End If
                            Else
                                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not exceed 15 characters")
                                bConfigValid = False
                            End If
                        Else
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup
                                rowConfigSettings1.ConfigSettingName = "InstanceID"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                            bConfigValid = False
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIURL" Then
                    ' API URL
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIURL", 250, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "V2SoapAPIWSICompliant" Then ' TJS 13/11/13
                    ' Magento Version
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "V2SoapAPIWSICompliant", False) Then ' TJS 13/11/13
                        bConfigValid = False ' TJS 13/11/13
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIUser" Then
                    ' API User
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIURL", 99, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIPwd" Then
                    ' API Password
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIURL", 99, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "MagentoVersion" Then ' TJS 02/10/13
                    ' Magento Version
                    If Not ValidateConnectorConfigStringSetting(iLoop, "MagentoVersion", 99, False) Then ' TJS 02/10/13
                        bConfigValid = False ' TJS 02/10/13
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "LerrynAPIVersion" Then ' TJS 13/11/13
                    ' Magento Version
                    If Not ValidateConnectorConfigStringSetting(iLoop, "LerrynAPIVersion", 99, False) Then ' TJS 13/11/13
                        bConfigValid = False ' TJS 13/11/13
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OrderPollIntervalMinutes" Then
                    ' Order Poll Interval Minutes value
                    If Not ValidateConnectorConfigOrderPollIntMins(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APISupportsPartialShipments" Then
                    ' API Supports Partial Shipments
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "APISupportsPartialShipments", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "CardAuthAndCaptureWithOrder" Then ' TJS 01/05/14
                    ' Card Auth And Capture With Order
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "CardAuthAndCaptureWithOrder", True) Then ' TJS 01/05/14
                        bConfigValid = False ' TJS 01/05/14
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' IS Item ID Field
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PricesAreTaxInclusive" Then
                    ' Send Prices As Tax Inclusive
                    If Not ValidateConnectorConfigSendPricesAsTaxIncl(iLoop) Then ' TJS 19/08/10
                        bConfigValid = False ' TJS 19/08/10
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "TaxCodeForSourceTax" Then ' TJS 26/10/11
                    ' Tax Code For Source Tax
                    If Not ValidateConnectorConfigTaxCodeForSourceTax(iLoop) Then ' TJS 26/10/11
                        bConfigValid = False ' TJS 26/10/11
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnablePaymentTypeTranslation" Then ' TJS 10/06/12
                    ' Enable Payment Type Translation
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnablePaymentTypeTranslation", True) Then ' TJS 10/06/12
                        bConfigValid = False ' TJS 10/06/12
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AllowShippingLastNameBlank" Then
                    ' Allow Shipping Last Name Blank
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AllowShippingLastNameBlank", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnableSKUAliasLookup" Then
                    ' Enable SKU Alias Lookup
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnableSKUAliasLookup", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ProductListBlockSize" Then ' TJS 14/02/12
                    ' Product List Block Size value
                    If Not ValidateConnectorConfigIntegerValue(iLoop, "ProductListBlockSize") Then ' TJS 14/02/12
                        bConfigValid = False ' TJS 14/02/12
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "InhibitInventoryUpdates" Then ' TJS 30/04/13
                    ' Inhibit Inventory Updates
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "InhibitInventoryUpdates", True) Then ' TJS 30/04/13
                        bConfigValid = False ' TJS 30/04/13
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "CreateCustomerForGuestCheckout" Then ' TJS 30/04/13
                    ' Create Customer For Guest Checkout
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "CreateCustomerForGuestCheckout", True) Then ' TJS 30/04/13
                        bConfigValid = False ' TJS 30/04/13
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "IncludeChildItemsOnOrder" Then ' TJS 08/05/13
                    ' Include Child Items On Order
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "IncludeChildItemsOnOrder", True) Then ' TJS 08/05/13
                        bConfigValid = False ' TJS 08/05/13
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "UpdateMagentoSpecialPricing" Then ' TJS 13/11/13
                    ' Magento Version
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "UpdateMagentoSpecialPricing", False) Then ' TJS 13/11/13
                        bConfigValid = False ' TJS 13/11/13
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If
                End If

            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 13) = "ASPStoreFront" Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "SiteID" Then
                    ' Site ID
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop)
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "" Then
                            If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Length <= 15 Then
                                If InStr(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue, "-") = 0 Then
                                    ' start of code added TJS 02/12/11
                                    For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                                        If iLoop <> iCheckLoop Then
                                            If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                                Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                                Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                                bConfigValid = False
                                            End If
                                        End If
                                    Next
                                    ' end of code added TJS 02/12/11

                                    If rowConfigSettings1 IsNot Nothing And bConfigValid Then ' TJS 02/12/11
                                        rowConfigSettings1.ClearErrors()
                                    End If
                                Else
                                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not contain - characters")
                                    bConfigValid = False
                                End If
                            Else
                                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not exceed 15 characters")
                                bConfigValid = False
                            End If
                        Else
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup
                                rowConfigSettings1.ConfigSettingName = "SiteID"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                            bConfigValid = False
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "UseWSE3Authentication" Then
                    ' Enable Poll For Orders
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "UseWSE3Authentication", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIURL" Then
                    ' API URL
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIURL", 250, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OrderPollIntervalMinutes" Then
                    ' Order Poll Interval Minutes value
                    If Not ValidateConnectorConfigOrderPollIntMins(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIUser" Then
                    ' API User
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIUser", 99, True) Then ' TJS 02/12/11
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIPwd" Then
                    ' API Password
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIPwd", 99, True) Then ' TJS 02/12/11
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' IS Item ID Field
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AllowShippingLastNameBlank" Then
                    ' Allow Shipping Last Name Blank
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AllowShippingLastNameBlank", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnableSKUAliasLookup" Then
                    ' Enable SKU Alias Lookup
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnableSKUAliasLookup", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If
                End If
                ' end of code added TJS 19/08/10

                ' start of code added TJS 02/12/11
            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 4) = "eBay" Then ' TJS 26/01/12
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "SiteID" Then
                    ' Site ID
                    If Not ValidateConnectorConfigStringSetting(iLoop, "SiteID", 30, True) Then
                        bConfigValid = False
                    Else
                        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                            If iLoop <> iCheckLoop Then
                                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                    Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                    Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    bConfigValid = False
                                End If
                            End If
                        Next

                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "Country" Then
                    ' Country
                    If Not ValidateConnectorConfigStringSetting(iLoop, "Country", 99, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OrderPollIntervalMinutes" Then
                    ' Order Poll Interval Minutes value
                    If Not ValidateConnectorConfigOrderPollIntMins(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AuthToken" Then
                    ' Auth Token
                    If Not ValidateConnectorConfigStringSetting(iLoop, "AuthToken", 1999, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "TokenExpires" Then ' TJS 16/01/12
                    ' Auth Token Expires
                    If Not ValidateXMLDate(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue) Then ' TJS 23/01/12
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Not a valid XML Date - must be yyyy-mm-ddThh:nn:ss")
                        bConfigValid = False
                        ' start of code added TJS 23/01/12
                    ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(10, 1) <> "T" Or _
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(13, 1) <> ":" Or _
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(16, 1) <> ":" Then
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Not a valid XML Date - must be yyyy-mm-ddThh:nn:ss")
                        bConfigValid = False
                    ElseIf Not IsNumeric(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(11, 2)) Or _
                        Not IsNumeric(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(14, 2)) Or _
                        Not IsNumeric(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(17, 2)) Then
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Not a valid XML Date - must be yyyy-mm-ddThh:nn:ss")
                        bConfigValid = False
                    ElseIf CInt(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(11, 2)) > 23 Or _
                        CInt(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(14, 2)) > 59 Or _
                        CInt(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(17, 2)) > 59 Then
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Not a valid XML Date - must be yyyy-mm-ddThh:nn:ss")
                        bConfigValid = False
                    ElseIf DateSerial(CInt(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(0, 4)), _
                        CInt(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(5, 2)), _
                        CInt(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Substring(8, 2))) < Date.Today Then
                        rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, "AuthToken")
                        If rowConfigSettings2 IsNot Nothing Then
                            rowConfigSettings2.SetColumnError("ConfigSettingName", "AuthToken has expired - please renew")
                        End If
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "AuthToken has expired - please renew")
                        bConfigValid = False
                        ' end of code added TJS 23/01/12
                    Else
                        Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ClearErrors()
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' IS Item ID Field
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PaymentType" Then
                    ' Payment Type
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop)
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, "EnablePaymentTypeTranslation")
                    If rowConfigSettings2 IsNot Nothing Then
                        strTemp = rowConfigSettings2.Item("ConfigSettingValue").ToString
                    Else
                        strTemp = "NO"
                    End If
                    If bConfigGroupActive Then
                        If Me.GetField("SELECT IsActive FROM SystemPaymentType WHERE PaymentTypeCode = '" & Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing) = "True" Then
                            If rowConfigSettings1 IsNot Nothing Then
                                rowConfigSettings1.ClearErrors()
                            End If
                        ElseIf strTemp.ToUpper <> "YES" Then
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = "eBay"
                                rowConfigSettings1.ConfigSettingName = "PaymentType"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be valid and active Payment Type")
                            bConfigValid = False
                        Else
                            If rowConfigSettings1 IsNot Nothing Then
                                rowConfigSettings1.ClearErrors()
                            End If
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnablePaymentTypeTranslation" Then
                    ' Send Prices As Tax Inclusive
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnablePaymentTypeTranslation", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AllowShippingLastNameBlank" Then
                    ' Allow Shipping Last Name Blank
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AllowShippingLastNameBlank", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnableSKUAliasLookup" Then
                    ' Enable SKU Alias Lookup
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnableSKUAliasLookup", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ActionIfNoPayment" Then
                    ' Action If No Payment
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop)
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "Ignore" And _
                                Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "Import as Quote" Then
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup
                                rowConfigSettings1.ConfigSettingName = "ActionIfNoPayment"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be Ignore or Import as Quote")
                            bConfigValid = False
                        Else
                            If rowConfigSettings1 IsNot Nothing Then
                                rowConfigSettings1.ClearErrors()
                            End If
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If
                End If
                ' end of code added TJS 02/12/11

                ' start of code added TJS 16/01/12
            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 11) = "SearsDotCom" Then ' TJS 26/01/12
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "SiteID" Then
                    ' Site ID
                    If Not ValidateConnectorConfigStringSetting(iLoop, "SiteID", 30, True) Then
                        bConfigValid = False
                    Else
                        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                            If iLoop <> iCheckLoop Then
                                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                    Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                    Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    bConfigValid = False
                                End If
                            End If
                        Next

                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIUser" Then
                    ' API User
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIUser", 99, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "APIPwd" Then
                    ' API Password
                    If Not ValidateConnectorConfigStringSetting(iLoop, "APIPwd", 99, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OrderPollIntervalMinutes" Then
                    ' Order Poll Interval Minutes value
                    If Not ValidateConnectorConfigOrderPollIntMins(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' IS Item ID Field
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PricesAreTaxInclusive" Then
                    ' Send Prices As Tax Inclusive
                    If Not ValidateConnectorConfigSendPricesAsTaxIncl(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "TaxCodeForSourceTax" Then
                    ' Tax Code For Source Tax
                    If Not ValidateConnectorConfigTaxCodeForSourceTax(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "Currency" Then
                    ' Currency
                    If Not ValidateConnectorConfigCurrency(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "PaymentType" Then
                    ' Payment Type
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop)
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.GetField("SELECT IsActive FROM SystemPaymentType WHERE PaymentTypeCode = '" & Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing) = "True" Then
                            If rowConfigSettings1 IsNot Nothing Then
                                rowConfigSettings1.ClearErrors()
                            End If
                        Else
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = "SearsDotCom"
                                rowConfigSettings1.ConfigSettingName = "PaymentType"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be valid and active Payment Type")
                            bConfigValid = False
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "SearsGeneratesInvoice" Then
                    ' Sears Generates Invoice
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "SearsGeneratesInvoice", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If

                End If
                ' end of code added TJS 16/01/12

                ' start of code added TJS 05/07/12
            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 9) = "AmazonFBA" Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "MerchantToken" Then ' TJS 22/03/13
                    ' Merchant ID
                    If Not ValidateConnectorConfigStringSetting(iLoop, "MerchantToken", 30, True) Then ' TJS 22/03/13
                        bConfigValid = False
                    Else
                        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                            If iLoop <> iCheckLoop Then
                                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                    Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then ' TJs 09/08/13
                                    Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                    bConfigValid = False
                                End If
                            End If
                        Next
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AmazonSite" Then
                    ' Amazon Site
                    If Not ValidateConnectorConfigStringSetting(iLoop, "AmazonSite", 10, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If

                End If
                ' end of code added TJS 05/07/12

                ' start of code added TJS 20/11/13
            ElseIf Strings.Left(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup, 10) = "ThreeDCart" Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "StoreID" Then
                    ' Store ID
                    bConfigGroupActive = CheckConnectorConfigGroupActive(iLoop)
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoop)
                    If bConfigGroupActive Then
                        If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue <> "" Then
                            If Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue.Length <= 15 Then
                                If InStr(Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue, "-") = 0 Then
                                    For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
                                        If iLoop <> iCheckLoop Then
                                            If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup AndAlso _
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingName = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName AndAlso _
                                                Not Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).IsConfigSettingValueNull AndAlso _
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingValue Then
                                                Me.m_ImportExportDataset.XMLConfigSettings(iLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).SetColumnError("ConfigSettingName", "Must be a unique value")
                                                bConfigValid = False
                                            End If
                                        End If
                                    Next

                                    If rowConfigSettings1 IsNot Nothing And bConfigValid Then
                                        rowConfigSettings1.ClearErrors()
                                    End If
                                Else
                                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not contain - characters")
                                    bConfigValid = False
                                End If
                            Else
                                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not exceed 15 characters")
                                bConfigValid = False
                            End If
                        Else
                            If rowConfigSettings1 Is Nothing Then
                                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                                rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigGroup
                                rowConfigSettings1.ConfigSettingName = "StoreID"
                                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                            End If
                            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                            bConfigValid = False
                        End If
                    Else
                        If rowConfigSettings1 IsNot Nothing Then
                            rowConfigSettings1.ClearErrors()
                        End If
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "StoreURL" Then
                    ' Store URL
                    If Not ValidateConnectorConfigStringSetting(iLoop, "StoreURL", 250, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "UserKey" Then
                    ' User Key
                    If Not ValidateConnectorConfigStringSetting(iLoop, "UserKey", 99, True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "OrderPollIntervalMinutes" Then
                    ' Order Poll Interval Minutes value
                    If Not ValidateConnectorConfigOrderPollIntMins(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "ISItemIDField" Then
                    ' IS Item ID Field
                    If Not ValidateConnectorConfigISItemIDField(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "Currency" Then
                    ' Currency
                    If Not ValidateConnectorConfigCurrency(iLoop) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AllowShippingLastNameBlank" Then
                    ' Allow Shipping Last Name Blank
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AllowShippingLastNameBlank", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "EnableSKUAliasLookup" Then
                    ' Enable SKU Alias Lookup
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "EnableSKUAliasLookup", True) Then
                        bConfigValid = False
                    End If

                ElseIf Me.m_ImportExportDataset.XMLConfigSettings(iLoop).ConfigSettingName = "AccountDisabled" Then
                    ' Account Disabled
                    If Not ValidateConnectorConfigYesNoSetting(iLoop, "AccountDisabled", True) Then
                        bConfigValid = False
                    End If
                End If
                ' end of code added TJS 20/11/13

            End If
        Next
        Return bConfigValid

    End Function

    Private Function ValidateCoreConfigStringSetting(ByVal XMLConfig As XDocument, ByVal XMLConfigPath As String, _
        ByVal SettingName As String, ByVal MaxLength As Integer, ByVal SettingRequired As Boolean) As Boolean ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added using code from ValidateConfigSettings 
        '                                        | with additional check for max length
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim strConfigValue As String

        ValidateCoreConfigStringSetting = True
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", SettingName)
        strConfigValue = Me.m_ImportExportRule.GetXMLElementText(XMLConfig, XMLConfigPath)
        If strConfigValue = "" And SettingRequired Then
            If rowConfigSettings1 Is Nothing Then
                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                rowConfigSettings1.ConfigGroup = "General"
                rowConfigSettings1.ConfigSettingName = SettingName
                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
            End If
            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
            ValidateCoreConfigStringSetting = False

        ElseIf strConfigValue.Length > MaxLength Then
            If rowConfigSettings1 Is Nothing Then
                rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                rowConfigSettings1.ConfigGroup = "General"
                rowConfigSettings1.ConfigSettingName = SettingName
                Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
            End If
            rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be more than " & MaxLength & " characters")
            ValidateCoreConfigStringSetting = False

        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateCoreConfigYesNoSetting(ByVal XMLConfig As XDocument, ByVal XMLConfigPath As String, _
        ByVal SettingName As String, ByVal SettingRequired As Boolean) As Boolean ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added using code from ValidateConfigSettings 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '------------------------------------------------------------------------------------------

        ValidateCoreConfigYesNoSetting = ValidateCoreConfigOptionSetting(XMLConfig, XMLConfigPath, SettingName, "Yes", "No", SettingRequired)

    End Function

    Private Function ValidateCoreConfigOptionSetting(ByVal XMLConfig As XDocument, ByVal XMLConfigPath As String, ByVal SettingName As String, _
        ByVal Option1Value As String, ByVal Option2Value As String, ByVal SettingRequired As Boolean) As Boolean ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added using code from ValidateConfigSettings 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow

        ValidateCoreConfigOptionSetting = True
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", SettingName)
        If Me.m_ImportExportRule.GetXMLElementText(XMLConfig, XMLConfigPath) <> Option1Value And _
                Me.m_ImportExportRule.GetXMLElementText(XMLConfig, XMLConfigPath) <> Option2Value Then
            If Me.m_ImportExportRule.GetXMLElementText(XMLConfig, XMLConfigPath) <> "" Or SettingRequired Then
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = "General"
                    rowConfigSettings1.ConfigSettingName = SettingName
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be " & Option1Value & " or " & Option2Value)
                ValidateCoreConfigOptionSetting = False
            End If
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigCurrency(ByVal iLoopPtr As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim bConfigGroupActive As Boolean

        ValidateConnectorConfigCurrency = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        If bConfigGroupActive Then
            If Me.GetField("SELECT IsActive FROM SystemCurrency WHERE CurrencyCode = '" & Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing) = "True" Then
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            Else
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = "Currency"
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be valid and active Currency Code")
                ValidateConnectorConfigCurrency = False
            End If
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigTaxCodeForSourceTax(ByVal iLoopPtr As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim rowConfigSettings2 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim strTaxRate As String, bConfigGroupActive As Boolean

        ValidateConnectorConfigTaxCodeForSourceTax = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName(Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup, "PricesAreTaxInclusive")
        If bConfigGroupActive And rowConfigSettings2.ConfigSettingValue.ToUpper = "YES" Then
            strTaxRate = Me.GetField("SELECT TaxRate FROM SystemTaxSchemeDetailView WHERE TaxCode = '" & Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.Replace("'", "''") & "'", System.Data.CommandType.Text, Nothing)
            If strTaxRate <> "" AndAlso CDbl(strTaxRate) > 0 Then
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            Else
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = "TaxCodeForSourceTax"
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be a Tax Code with a non-zero Tax Rate")
                ValidateConnectorConfigTaxCodeForSourceTax = False
            End If
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigISItemIDField(ByVal iLoopPtr As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim bConfigGroupActive As Boolean

        ValidateConnectorConfigISItemIDField = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        If bConfigGroupActive Then
            If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue <> "" Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue <> "ItemCode" And _
                    Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue <> "ItemName" Then
                    If rowConfigSettings1 Is Nothing Then
                        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                        rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                        rowConfigSettings1.ConfigSettingName = "ISItemIDField"
                        Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                    End If
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be ItemCode or ItemName")
                Else
                    If rowConfigSettings1 IsNot Nothing Then
                        rowConfigSettings1.ClearErrors()
                    End If
                End If
            Else
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = "ISItemIDField"
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                ValidateConnectorConfigISItemIDField = False
            End If
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigDefaultUpliftPercent(ByVal iLoopPtr As Integer, ByVal SettingRequired As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim strTemp As String, bConfigGroupActive As Boolean, bValueValid As Boolean

        ValidateConnectorConfigDefaultUpliftPercent = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        If bConfigGroupActive Then
            If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue <> "" Then
                strTemp = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue
                bValueValid = True
                ' is value numeric ?
                If Not IsNumeric(strTemp) Then
                    bValueValid = False
                Else
                    ' yes, does it contain any commas ?
                    If InStr(strTemp, ",") > 0 Then
                        ' yes
                        bValueValid = False
                    End If
                End If
                If Not bValueValid Then
                    If rowConfigSettings1 Is Nothing Then
                        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                        rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                        rowConfigSettings1.ConfigSettingName = "DefaultUpliftPercent"
                        Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                    End If
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be numeric")
                Else
                    If rowConfigSettings1 IsNot Nothing Then
                        rowConfigSettings1.ClearErrors()
                    End If
                End If
            ElseIf SettingRequired Then
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = "DefaultUpliftPercent"
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                ValidateConnectorConfigDefaultUpliftPercent = False
            End If

        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigOrderPollIntMins(ByVal iLoopPtr As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to use ValidateConnectorConfigIntegerValue
        '------------------------------------------------------------------------------------------

        ValidateConnectorConfigOrderPollIntMins = ValidateConnectorConfigIntegerValue(iLoopPtr, "OrderPollIntervalMinutes") ' TJS 14/02/12

    End Function

    Private Function ValidateConnectorConfigIntegerValue(ByVal iLoopPtr As Integer, ByVal SettingName As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/02/12 | TJS             | 2011.2.05 | Function created from ValidateConnectorConfigOrderPollIntMins 
        '                                        | to provide a generic Integer value validation function
        ' 02/10/13 | TJS             | 2013.3.03 | Modified to return false in value not an integer
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim strTemp As String, bConfigGroupActive As Boolean, bValueValid As Boolean

        ValidateConnectorConfigIntegerValue = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        If bConfigGroupActive Then
            If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue <> "" Then
                strTemp = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue
                bValueValid = True
                ' is value numeric ?
                If Not IsNumeric(strTemp) Then
                    bValueValid = False
                Else
                    ' yes, does it contain any commas ?
                    If InStr(strTemp, ",") > 0 Then
                        ' yes
                        bValueValid = False
                    End If
                End If
                If Not bValueValid Then
                    If rowConfigSettings1 Is Nothing Then
                        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                        rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                        rowConfigSettings1.ConfigSettingName = SettingName
                        Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                    End If
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be an integer value")
                    ValidateConnectorConfigIntegerValue = False ' TJS 02/10/13
                Else
                    If rowConfigSettings1 IsNot Nothing Then
                        rowConfigSettings1.ClearErrors()
                    End If
                End If
            Else
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = SettingName
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                ValidateConnectorConfigIntegerValue = False
            End If

        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigSendPricesAsTaxIncl(ByVal iLoopPtr As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim rowConfigSettings2 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim strTemp As String, bConfigGroupActive As Boolean

        ValidateConnectorConfigSendPricesAsTaxIncl = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        rowConfigSettings2 = Me.m_ImportExportDataset.XMLConfigSettings.FindByConfigGroupConfigSettingName("General", "AcceptSourceSalesTaxCalculation")
        If rowConfigSettings2 IsNot Nothing Then
            strTemp = rowConfigSettings2.Item("ConfigSettingValue").ToString
        Else
            strTemp = "NO"
        End If
        If bConfigGroupActive Then
            If (Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.ToUpper = "YES" Or _
                Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.ToUpper = "NO") And _
                (Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.ToUpper <> "YES" Or _
                strTemp.ToUpper <> "YES") Then
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            Else
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = "PricesAreTaxInclusive"
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.ToUpper = "YES" And _
                    strTemp.ToUpper = "YES" Then
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Cannot be Yes when AcceptSourceSalesTaxCalculation is Yes")
                    rowConfigSettings2.SetColumnError("ConfigSettingName", "Cannot be Yes when PricesAreTaxInclusive is Yes")
                Else
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be Yes or No")
                End If
                ValidateConnectorConfigSendPricesAsTaxIncl = False
            End If
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigYesNoSetting(ByVal iLoopPtr As Integer, ByVal SettingName As String, _
        ByVal SettingRequired As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint 
        '                                        | and modified to use ValidateConnectorConfigOptionSetting
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for ValidateConnectorConfigOptionSetting allowing a variable number of option values
        '------------------------------------------------------------------------------------------

        ValidateConnectorConfigYesNoSetting = ValidateConnectorConfigOptionSetting(iLoopPtr, SettingName, SettingRequired, New String() {"Yes", "No"}) ' TJS 09/07/11

    End Function

    Private Function ValidateConnectorConfigOptionSetting(ByVal iLoopPtr As Integer, ByVal SettingName As String, _
        ByVal SettingRequired As Boolean, ByVal OptionValues As String()) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to allow variable number of option values
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim strValuesList As String, iLoop As Integer, bConfigGroupActive As Boolean, bValueIsValid As Boolean

        ValidateConnectorConfigOptionSetting = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        If bConfigGroupActive Then
            bValueIsValid = False ' TJS 09/07/11
            strValuesList = "" ' TJS 09/07/11
            For iLoop = 0 To OptionValues.Length - 1 ' TJS 09/07/11
                If strValuesList <> "" Then ' TJS 09/07/11
                    strValuesList = strValuesList & ", " ' TJS 09/07/11
                End If
                strValuesList = strValuesList & OptionValues(iLoop) ' TJS 09/07/11
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.ToUpper = OptionValues(iLoop).ToUpper Then ' TJS 09/07/11
                    bValueIsValid = True ' TJS 09/07/11
                End If
            Next
            If bValueIsValid Then ' TJS 09/07/11
                If rowConfigSettings1 IsNot Nothing Then
                    rowConfigSettings1.ClearErrors()
                End If
            ElseIf SettingRequired Then
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = SettingName
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                If OptionValues.Length = 2 Then ' TJS 09/07/11
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be " & OptionValues(0) & " or " & OptionValues(1)) ' TJS 09/07/11
                Else
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must be one of - " & strValuesList) ' TJS 09/07/11
                End If
                ValidateConnectorConfigOptionSetting = False
            End If
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function ValidateConnectorConfigStringSetting(ByVal iLoopPtr As Integer, ByVal SettingName As String, _
        ByVal MaxLength As Integer, ByVal SettingRequired As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function copied from eShopCONNECT for Tradepoint
        '                                        | with additional check for max length
        '------------------------------------------------------------------------------------------

        Dim rowConfigSettings1 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.XMLConfigSettingsRow
        Dim bConfigGroupActive As Boolean

        ValidateConnectorConfigStringSetting = True
        bConfigGroupActive = CheckConnectorConfigGroupActive(iLoopPtr)
        rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr)
        If bConfigGroupActive Then
            If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue <> "" Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigSettingValue.Length <= MaxLength Then
                    If rowConfigSettings1 IsNot Nothing Then
                        rowConfigSettings1.ClearErrors()
                    End If
                Else
                    rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not exceed " & MaxLength & " characters")
                    ValidateConnectorConfigStringSetting = False

                End If
            ElseIf SettingRequired Then
                If rowConfigSettings1 Is Nothing Then
                    rowConfigSettings1 = Me.m_ImportExportDataset.XMLConfigSettings.NewXMLConfigSettingsRow
                    rowConfigSettings1.ConfigGroup = Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup
                    rowConfigSettings1.ConfigSettingName = SettingName
                    Me.m_ImportExportDataset.XMLConfigSettings.AddXMLConfigSettingsRow(rowConfigSettings1)
                End If
                rowConfigSettings1.SetColumnError("ConfigSettingName", "Must not be blank")
                ValidateConnectorConfigStringSetting = False
            End If
        Else
            If rowConfigSettings1 IsNot Nothing Then
                rowConfigSettings1.ClearErrors()
            End If
        End If

    End Function

    Private Function CheckConnectorConfigGroupActive(ByVal iLoopPtr As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added using code from ValidateConfigSettings 
        '------------------------------------------------------------------------------------------

        Dim iCheckLoop As Integer

        CheckConnectorConfigGroupActive = False
        ' are any settings in current Config Group entered ?
        For iCheckLoop = 0 To Me.m_ImportExportDataset.XMLConfigSettings.Count - 1
            If Me.m_ImportExportDataset.XMLConfigSettings(iLoopPtr).ConfigGroup = _
                Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigGroup Then
                If Me.m_ImportExportDataset.XMLConfigSettings(iCheckLoop).ConfigSettingValue <> "" Then
                    ' yes, group is active
                    CheckConnectorConfigGroupActive = True
                    Exit For
                End If
            End If
        Next

    End Function
#End Region

#Region " LoadConfigSettings "
    Public Function LoadConfigSettings(ByRef SourceConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row, _
        ByRef SourceSettings As Lerryn.Framework.ImportExport.SourceConfig.SourceSettings, ByRef FileImportRequired As Boolean, ByRef WebIORequired As Boolean, ByRef ErrorDetails As String) As Boolean
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

        Return m_ImportExportRule.LoadConfigSettings(SourceConfig, SourceSettings, FileImportRequired, WebIORequired, ErrorDetails)

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
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If XMLNSMan IsNot Nothing Then
            Return Me.m_ImportExportRule.GetXMLElementText(XMLMessage, ElementName, XMLNSMan)
        Else
            Return Me.m_ImportExportRule.GetXMLElementText(XMLMessage, ElementName)
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
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If XMLNSMan IsNot Nothing Then
            Return Me.m_ImportExportRule.GetXMLElementText(XMLMessage, ElementName, XMLNSMan)
        Else
            Return Me.m_ImportExportRule.GetXMLElementText(XMLMessage, ElementName)
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
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Me.m_ImportExportRule.GetXMLElementListCount(XMLElementList)

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
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Me.m_ImportExportRule.GetXMLElementListCount(XMLElementList)

    End Function
#End Region

#Region " GetXMLElementAttribute "
    Public Function GetXMLElementAttribute(ByVal XMLMessage As XElement, ByVal ElementName As String, ByVal AttributeName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String ' TJS 02/12/11

        If XMLNSMan IsNot Nothing Then ' TJS 02/12/11
            Return Me.m_ImportExportRule.GetXMLElementAttribute(XMLMessage, ElementName, AttributeName, XMLNSMan) ' TJS 02/12/11
        Else
            Return Me.m_ImportExportRule.GetXMLElementAttribute(XMLMessage, ElementName, AttributeName)
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
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If XMLNSMan IsNot Nothing Then
            Return Me.m_ImportExportRule.GetXMLElementAttribute(XMLMessage, ElementName, AttributeName, XMLNSMan)
        Else
            Return Me.m_ImportExportRule.GetXMLElementAttribute(XMLMessage, ElementName, AttributeName)
        End If

    End Function
#End Region

#Region " ConvertXMLFromWeb "
    Public Function ConvertXMLFromWeb(ByVal InputString As String) As String

        Return Me.m_ImportExportRule.ConvertXMLFromWeb(InputString)

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
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Me.m_ImportExportRule.ConvertFromXML(InputString)

    End Function
#End Region

#Region " ConvertEntitiesForXML "
    Public Function ConvertEntitiesForXML(ByVal InputString As String) As String
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

        Return Me.m_ImportExportRule.ConvertForXML(InputString)

    End Function
#End Region

#Region " DecodeConfigXMLValue "
    Public Function DecodeConfigXMLValue(ByVal InputString As String) As String
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

        Return Me.m_ImportExportRule.DecodeConfigXMLValue(InputString)

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
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Me.m_ImportExportRule.ValidateXMLDate(XMLDateValue)

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
        ' 11/03/09 | TJS             | 2009.1.09 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Me.m_ImportExportRule.ConvertXMLDate(XMLDateValue)

    End Function
#End Region

#Region " ConvertCRLF "
    Public Function ConvertCRLF(ByVal InputValue As String) As String

        Return Me.m_ImportExportRule.ConvertCRLF(InputValue)

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

        Return Me.m_ImportExportRule.CheckRegistryValue(RegistryKey, ValueName, DefaultValue)

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
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.05 | modified to use Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return Me.m_ImportExportRule.BuildXMLErrorResponseNodeAndEmail(ResponseStatus, ErrorCode, ErrorMessage, _
            SourceConfig, ProcedureName, XMLInput)

    End Function
#End Region

#Region " SendErrorEmail "
    Public Sub SendErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, ByVal ex As Exception, Optional ByVal XMLSource As String = "") ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | modified to cater for use of Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ImportExportRule.SendErrorEmail(SourceConfig, ProcedureName, ex, XMLSource)

    End Sub

    Public Sub SendErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, ByVal ErrorDetails As String, Optional ByVal XMLSource As String = "") ' TJS 02/12/11
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
        ' 10/06/12 | TJS             | 2012.1.05 | modified to cater for use of Error Notification object to simplify facade login/logout
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ImportExportRule.SendErrorEmail(SourceConfig, ProcedureName, ErrorDetails, XMLSource)

    End Sub
#End Region

#Region " SendSourceErrorEmail "
    Public Sub SendSourceErrorEmail(ByVal SourceConfig As XDocument, ByVal ProcedureName As String, ByVal Message As String, Optional ByVal XMLSource As String = "") ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | modified to cater for use of Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ImportExportRule.SendSourceErrorEmail(SourceConfig, ProcedureName, Message, XMLSource)

    End Sub
#End Region

#Region " SendPaymentErrorEmail "
    Public Sub SendPaymentErrorEmail(ByVal SourceConfig As XDocument, ByVal Message As String) ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Notifies key parties of payment failures
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 21/03/11 | TJS             | 2011.0.02 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 10/06/12 | TJS             | 2012.1.05 | modified to cater for use of Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ImportExportRule.SendPaymentErrorEmail(SourceConfig, Message)

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
        ' 10/06/12 | TJS             | 2012.1.05 | modified to cater for use of Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_ImportExportRule.WriteLogProgressRecord(Message)

    End Sub
#End Region

#Region " GetAmazonBrowseListItems "
    Public Sub GetAmazonBrowseListItems(ByVal ParentCategory As String, ByVal SiteCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.06 | Function added
        ' 25/05/09 | TJS             | 2009.2.08 | Corrected processing of received Browse list items
        ' 28/06/09 | TJS             | 2009.3.00 | Corected initial setting of NodeName_DEV000221
        ' 21/10/09 | TJS             | 2009.3.08 | Modified to cater for ProductXMLClass, ProductXMLType and ProductXMLSubType
        ' 08/11/09 | TJS             | 2009.3.09 | Modified to cater for LastUpdated field being null
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLBrowseItems As XDocument, XMLTemp As XDocument
        Dim XMLAmazonBrowseNodes As System.Collections.Generic.IEnumerable(Of XElement), XMLAmazonBrowseNode As XElement
        Dim rowAmazonBrowseNode As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonBrowseNodes_DEV000221Row
        Dim strParentNodeID As String, strParentCategory As String, strNodeID As String
        Dim strSiteCode As String, strTemp As String, dteLastModified As Date, bRecordChanged As Boolean ' TJS 25/05/09

        ' get last modified date in records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodeLastModifiedView_DEV000221.TableName, _
            "ReadLrynImpExpAmazonBrowseNodeLastMod_DEV000221", AT_SITE_CODE, SiteCode, AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 25/05/09
        If Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodeLastModifiedView_DEV000221.Count > 0 Then
            If Not Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodeLastModifiedView_DEV000221(0).IsLastUpdatedNull Then ' TJS 08/11/09
                dteLastModified = Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodeLastModifiedView_DEV000221(0).LastUpdated
                If ParentCategory <> AMAZON_ROOT_CATEGORY Then ' TJS 08/11/09
                    RaiseEvent BeforeInitialConfigDownload(Me, "eShopCONNECTED is downloading the latest updates to the Amazon Browse List data for this Amazon Product category.  Whilst normally very quick, occasionally large updates may take several minutes.") ' TJS 08/11/09
                End If
            Else
                dteLastModified = DateSerial(2009, 1, 1) ' TJS 08/11/09
                If ParentCategory <> AMAZON_ROOT_CATEGORY Then ' TJS 08/11/09
                    RaiseEvent BeforeInitialConfigDownload(Me, "As this is the first time you have published an item in this Amazon Product category, eShopCONNECTED is downloading the latest Amazon Browse List data which may take several minutes.") ' TJS 08/11/09
                End If
            End If
        Else
            dteLastModified = DateSerial(2009, 1, 1)
            If ParentCategory <> AMAZON_ROOT_CATEGORY Then ' TJS 08/11/09
                RaiseEvent BeforeInitialConfigDownload(Me, "As this is the first time you have published an item in this Amazon Product category, eShopCONNECTED is downloading the latest Amazon Browse List data which may take several minutes.") ' TJS 08/11/09
            End If
        End If

        ' now get any updates from LERWEB01
        XMLBrowseItems = m_ImportExportRule.GetAmazonBrowseListItems(ParentCategory, SiteCode, dteLastModified) ' TJS 25/05/09
        ' load dataset with records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
            "ReadLerrynImportExportAmazonBrowseNodes_DEV000221", AT_SITE_CODE, SiteCode, AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 25/05/09
        ' get browse node list
        XMLAmazonBrowseNodes = XMLBrowseItems.XPathSelectElements("eShopCONNECT/AmazonBrowseNode")
        ' now process each in turn
        For Each XMLAmazonBrowseNode In XMLAmazonBrowseNodes
            XMLTemp = XDocument.Parse(XMLAmazonBrowseNode.ToString)
            ' get Primary Key fields
            strSiteCode = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/SiteCode")) ' TJS 25/05/09
            strParentCategory = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ParentCategory")) ' TJS 25/05/09
            strParentNodeID = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ParentNodeID")) ' TJS 25/05/09
            strNodeID = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/NodeID")) ' TJS 25/05/09
            ' does row exist ?
            rowAmazonBrowseNode = Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.FindBySiteCode_DEV000221ParentCategory_DEV000221ParentNodeID_DEV000221NodeID_DEV000221(strSiteCode, CInt(strParentCategory), strParentNodeID, strNodeID) ' TJS 25/05/09
            If rowAmazonBrowseNode Is Nothing Then
                ' no, is record marked as deleted ?
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/Deleted")).ToUpper <> "TRUE" Then ' TJS 25/05/09
                    ' no, need to insert it
                    rowAmazonBrowseNode = Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.NewLerrynImportExportAmazonBrowseNodes_DEV000221Row
                    rowAmazonBrowseNode.SiteCode_DEV000221 = strSiteCode
                    rowAmazonBrowseNode.ParentCategory_DEV000221 = CInt(strParentCategory) ' TJS 25/05/09
                    rowAmazonBrowseNode.ParentNodeID_DEV000221 = strParentNodeID
                    rowAmazonBrowseNode.NodeID_DEV000221 = strNodeID
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/NodeName")) ' TJS 25/05/09 TJS 28/06/09
                    If strTemp <> "" Then ' TJS 25/05/09
                        rowAmazonBrowseNode.NodeName_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    ' end of code added TJS 21/10/09
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ProductXMLClass"))
                    If strTemp <> "" Then
                        rowAmazonBrowseNode.ProductXMLClass_DEV000221 = strTemp
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ProductXMLType"))
                    If strTemp <> "" Then
                        rowAmazonBrowseNode.ProductXMLType_DEV000221 = strTemp
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ProductXMLSubType"))
                    If strTemp <> "" Then
                        rowAmazonBrowseNode.ProductXMLSubType_DEV000221 = strTemp
                    End If
                    ' end of code added TJS 21/10/09
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/DescriptiveCategory")).ToUpper = "TRUE" Then ' TJS 25/05/09
                        rowAmazonBrowseNode.DescriptiveCategory_DEV000221 = True
                    Else
                        rowAmazonBrowseNode.DescriptiveCategory_DEV000221 = False
                    End If
                    rowAmazonBrowseNode.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/LastModified"))
                    Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.AddLerrynImportExportAmazonBrowseNodes_DEV000221Row(rowAmazonBrowseNode)
                End If

            Else
                ' need to update it if something has changed
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/Deleted")).ToUpper = "TRUE" Then ' TJS 25/05/09
                    rowAmazonBrowseNode.Delete() ' TJS 25/05/09
                Else
                    ' only change record in one or more fields have changed as save can take a long time otherwise
                    bRecordChanged = False ' TJS 25/05/09
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/NodeName")) ' TJS 25/05/09
                    If rowAmazonBrowseNode.IsNodeName_DEV000221Null Then ' TJS 25/05/09
                        If strTemp <> "" Then ' TJS 25/05/09
                            rowAmazonBrowseNode.NodeName_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 25/05/09
                            rowAmazonBrowseNode.SetNodeName_DEV000221Null() ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        ElseIf strTemp <> rowAmazonBrowseNode.NodeName_DEV000221 Then ' TJS 25/05/09
                            rowAmazonBrowseNode.NodeName_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    End If
                    ' start of code added TJS 21/10/09
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ProductXMLClass"))
                    If rowAmazonBrowseNode.IsProductXMLClass_DEV000221Null Then
                        If strTemp <> "" Then
                            rowAmazonBrowseNode.ProductXMLClass_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowAmazonBrowseNode.SetProductXMLClass_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowAmazonBrowseNode.ProductXMLClass_DEV000221 Then
                            rowAmazonBrowseNode.ProductXMLClass_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ProductXMLType"))
                    If rowAmazonBrowseNode.IsProductXMLType_DEV000221Null Then
                        If strTemp <> "" Then
                            rowAmazonBrowseNode.ProductXMLType_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowAmazonBrowseNode.SetProductXMLType_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowAmazonBrowseNode.ProductXMLType_DEV000221 Then
                            rowAmazonBrowseNode.ProductXMLType_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/ProductXMLSubType"))
                    If rowAmazonBrowseNode.IsProductXMLSubType_DEV000221Null Then
                        If strTemp <> "" Then
                            rowAmazonBrowseNode.ProductXMLSubType_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowAmazonBrowseNode.SetProductXMLSubType_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowAmazonBrowseNode.ProductXMLSubType_DEV000221 Then
                            rowAmazonBrowseNode.ProductXMLSubType_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    ' end of code added TJS 21/10/09
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/DescriptiveCategory")).ToUpper = "TRUE" And _
                        Not rowAmazonBrowseNode.DescriptiveCategory_DEV000221 Then ' TJS 25/05/09
                        rowAmazonBrowseNode.DescriptiveCategory_DEV000221 = True
                        bRecordChanged = True ' TJS 25/05/09
                    ElseIf rowAmazonBrowseNode.DescriptiveCategory_DEV000221 Then ' TJS 25/05/09
                        rowAmazonBrowseNode.DescriptiveCategory_DEV000221 = False
                        bRecordChanged = True ' TJS 25/05/09
                    End If
                    rowAmazonBrowseNode.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "AmazonBrowseNode/LastModified")) ' TJS 25/05/09
                End If

            End If

        Next

        ' were any Browse Nodes returned ?
        If GetXMLElementListCount(XMLAmazonBrowseNodes) > 0 Then
            ' yes, save updates
            Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportAmazonBrowseNodes_DEV000221.TableName, _
                "CreateLerrynImportExportAmazonBrowseNodes_DEV000221", "UpdateLerrynImportExportAmazonBrowseNodes_DEV000221", "DeleteLerrynImportExportAmazonBrowseNodes_DEV000221"}}, _
                Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Amazon Browse Nodes", False)
        End If

    End Sub
#End Region

#Region " GetAmazonTagTemplates "
    Public Sub GetAmazonTagTemplates(ByVal ParentCategory As String, ByVal SiteCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.06 | Function added
        ' 25/05/09 | TJS             | 2009.2.08 | Corrected processing of received Tag items
        ' 15/08/09 | TJS             | 2009.3.03 | Modified to cater for Tag XML Name
        ' 24/11/09 | TJS             | 2009.3.09 | Modified to cater for LastUpdated field being null and 
        '                                        | for ProductXMLClass, ProductXMLType, ProductXMLSubType,
        '                                        | ParentNode, TagOutputOrder and PlaceHolderOnly
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTagTemplates As XDocument, XMLTemp As XDocument
        Dim XMLAmazonTagTemplates As System.Collections.Generic.IEnumerable(Of XElement), XMLAmazonTagTemplate As XElement
        Dim rowAmazonTagTemplate As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportInventoryTagTemplate_DEV000221Row
        Dim strTagName As String, strParentCategory As String, strTagLocation As String, strTemp As String ' TJS 25/05/09
        Dim strSiteCode As String, strLineNum As String, dteLastModified As Date, bRecordChanged As Boolean ' TJS 25/05/09
        Dim strParentNode As String ' TJS 24/11/09

        ' get last modified date in records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplateLastModifiedView_DEV000221.TableName, _
            "ReadLrynImpExpInventoryTagTemplateLastMod_DEV000221", AT_SOURCE_CODE, "Amazon", AT_SITE_CODE, SiteCode, AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)
        If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplateLastModifiedView_DEV000221.Count > 0 Then
            If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplateLastModifiedView_DEV000221(0).IsLastUpdatedNull Then ' TJS 24/11/09
                dteLastModified = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplateLastModifiedView_DEV000221(0).LastUpdated
            Else
                dteLastModified = DateSerial(2009, 1, 1) ' TJS 24/11/09
            End If
        Else
            dteLastModified = DateSerial(2009, 1, 1)
        End If

        ' now get any updates from LERWEB01
        XMLTagTemplates = m_ImportExportRule.GetAmazonTagTemplates(ParentCategory, SiteCode, dteLastModified)
        ' load dataset with records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
            "ReadLerrynImportExportInventoryTagTemplate_DEV000221", AT_SOURCE_CODE, "Amazon", AT_SITE_CODE, SiteCode, AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)
        ' get browse node list
        XMLAmazonTagTemplates = XMLTagTemplates.XPathSelectElements("eShopCONNECT/AmazonTagTemplate")
        ' now process each in turn
        For Each XMLAmazonTagTemplate In XMLAmazonTagTemplates
            XMLTemp = XDocument.Parse(XMLAmazonTagTemplate.ToString)
            ' get Primary Key fields
            strSiteCode = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SiteCode")) ' TJS 25/05/09
            strParentCategory = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/ParentCategory")) ' TJS 25/05/09
            strTagLocation = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/TagLocation")) ' TJS 25/05/09
            strTagName = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/TagName")) ' TJS 25/05/09
            strLineNum = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/LineNum")) ' TJS 25/05/09
            strParentNode = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/ParentNode")) ' TJS 24/11/09
            ' does row exist ?
            rowAmazonTagTemplate = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.FindBySourceCode_DEV000221SiteCode_DEV000221ParentCategory_DEV000221TagLocation_DEV000221TagName_DEV000221LineNum_DEV000221ParentNode_DEV000221("Amazon", strSiteCode, CInt(strParentCategory), CInt(strTagLocation), strTagName, CInt(strLineNum), strParentNode) ' TSJ 24/11/09
            If rowAmazonTagTemplate Is Nothing Then
                ' no, is record marked as deleted ?
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/Deleted")).ToUpper <> "TRUE" Then ' TJS 25/05/09
                    ' no, need to insert it
                    rowAmazonTagTemplate = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.NewLerrynImportExportInventoryTagTemplate_DEV000221Row
                    rowAmazonTagTemplate.SourceCode_DEV000221 = "Amazon" ' TJS 25/05/09
                    rowAmazonTagTemplate.SiteCode_DEV000221 = strSiteCode
                    rowAmazonTagTemplate.ParentCategory_DEV000221 = CInt(strParentCategory) ' TJS 25/05/09
                    rowAmazonTagTemplate.TagLocation_DEV000221 = CInt(strTagLocation)
                    rowAmazonTagTemplate.TagName_DEV000221 = strTagName
                    rowAmazonTagTemplate.LineNum_DEV000221 = CInt(strLineNum) ' TJS 25/05/09
                    rowAmazonTagTemplate.ParentNode_DEV000221 = strParentNode ' TJS 24/11/09
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/TagXMLName")) ' TJS 15/08/09
                    If strTemp <> "" Then ' TJS 15/08/09
                        rowAmazonTagTemplate.TagXMLName_DEV000221 = strTemp ' TJS 15/08/09
                    Else
                        rowAmazonTagTemplate.TagXMLName_DEV000221 = rowAmazonTagTemplate.TagName_DEV000221 ' TJS 15/08/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/TagDataType")) ' TJS 25/05/09
                    If strTemp <> "" Then ' TJS 25/05/09
                        rowAmazonTagTemplate.TagDataType_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/TagOutputOrder")) ' TJS 24/11/09
                    If strTemp <> "" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.TagOutputOrder_DEV000221 = CDec(strTemp) ' TJS 24/11/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagStatus")) ' TJS 25/05/09
                    If strTemp <> "" Then ' TJS 25/05/09
                        rowAmazonTagTemplate.SourceTagStatus_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagConditionality")) ' TJS 25/05/09
                    If strTemp <> "" Then
                        rowAmazonTagTemplate.SourceTagConditionality_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagDescription")) ' TJS 25/05/09
                    If strTemp <> "" Then
                        rowAmazonTagTemplate.SourceTagDescription_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagAcceptedValues")) ' TJS 25/05/09
                    If strTemp <> "" Then
                        rowAmazonTagTemplate.SourceTagAcceptedValues_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagExample")) ' TJS 25/05/09
                    If strTemp <> "" Then
                        rowAmazonTagTemplate.SourceTagExample_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/ProductXMLClass")) ' TJS 24/11/09
                    If strTemp <> "" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.ProductXMLClass_DEV000221 = strTemp ' TJS 24/11/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/ProductXMLType")) ' TJS 24/11/09
                    If strTemp <> "" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.ProductXMLType_DEV000221 = strTemp ' TJS 24/11/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/ProductXMLSubType")) ' TJS 24/11/09
                    If strTemp <> "" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.ProductXMLSubType_DEV000221 = strTemp ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/PlatinumMerchantOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.PlatinumMerchantOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowAmazonTagTemplate.PlatinumMerchantOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/MatrixGroupOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.MatrixGroupOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowAmazonTagTemplate.MatrixGroupOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/NotMatrixGroup")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.NotMatrixGroup_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowAmazonTagTemplate.NotMatrixGroup_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/MatrixItemOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.MatrixItemOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowAmazonTagTemplate.MatrixItemOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/PlaceHolderOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowAmazonTagTemplate.PlaceHolderOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowAmazonTagTemplate.PlaceHolderOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    rowAmazonTagTemplate.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/LastModified")) ' TJS 25/05/09
                    Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.AddLerrynImportExportInventoryTagTemplate_DEV000221Row(rowAmazonTagTemplate)
                End If

            Else
                ' need to update it if something has changed
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/Deleted")).ToUpper = "TRUE" Then ' TJS 25/05/09
                    rowAmazonTagTemplate.Delete() ' TJS 25/05/09
                Else
                    ' only change record in one or more fields have changed as save can take a long time otherwise
                    bRecordChanged = False ' TJS 25/05/09
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/TagXMLName")) ' TJS 15/08/09
                    If strTemp = "" Then ' TJS 15/08/09
                        rowAmazonTagTemplate.TagXMLName_DEV000221 = rowAmazonTagTemplate.TagName_DEV000221 ' TJS 15/08/09
                        bRecordChanged = True ' TJS 15/08/09
                    ElseIf strTemp <> rowAmazonTagTemplate.TagXMLName_DEV000221 Then ' TJS 15/08/09
                        rowAmazonTagTemplate.TagXMLName_DEV000221 = strTemp ' TJS 15/08/09
                        bRecordChanged = True ' TJS 15/08/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/TagDataType"))
                    If strTemp <> rowAmazonTagTemplate.TagDataType_DEV000221 Then ' TJS 25/05/09
                        rowAmazonTagTemplate.TagDataType_DEV000221 = strTemp ' TJS 25/05/09
                        bRecordChanged = True ' TJS 25/05/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagStatus")) ' TJS 25/05/09
                    If rowAmazonTagTemplate.IsSourceTagStatus_DEV000221Null Then ' TJS 25/05/09
                        If strTemp <> "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagStatus_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SetSourceTagStatus_DEV000221Null() ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        ElseIf strTemp <> rowAmazonTagTemplate.SourceTagStatus_DEV000221 Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagStatus_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagConditionality")) ' TJS 25/05/09
                    If rowAmazonTagTemplate.IsSourceTagConditionality_DEV000221Null Then ' TJS 25/05/09
                        If strTemp <> "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagConditionality_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SetSourceTagConditionality_DEV000221Null() ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        ElseIf strTemp <> rowAmazonTagTemplate.SourceTagConditionality_DEV000221 Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagConditionality_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagDescription")) ' TJS 25/05/09
                    If rowAmazonTagTemplate.IsSourceTagDescription_DEV000221Null Then ' TJS 25/05/09
                        If strTemp <> "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagDescription_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SetSourceTagDescription_DEV000221Null() ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        ElseIf strTemp <> rowAmazonTagTemplate.SourceTagDescription_DEV000221 Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagDescription_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagAcceptedValues")) ' TJS 25/05/09
                    If rowAmazonTagTemplate.IsSourceTagAcceptedValues_DEV000221Null Then ' TJS 25/05/09
                        If strTemp <> "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagAcceptedValues_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SetSourceTagAcceptedValues_DEV000221Null() ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        ElseIf strTemp <> rowAmazonTagTemplate.SourceTagAcceptedValues_DEV000221 Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagAcceptedValues_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/SourceTagExample")) ' TJS 25/05/09
                    If rowAmazonTagTemplate.IsSourceTagExample_DEV000221Null Then ' TJS 25/05/09
                        If strTemp <> "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagExample_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SetSourceTagExample_DEV000221Null() ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        ElseIf strTemp <> rowAmazonTagTemplate.SourceTagExample_DEV000221 Then ' TJS 25/05/09
                            rowAmazonTagTemplate.SourceTagExample_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/ProductXMLClass")) ' TJS 24/11/09
                    If rowAmazonTagTemplate.IsProductXMLClass_DEV000221Null Then ' TJS 24/11/09
                        If strTemp <> "" Then ' TJS 24/11/09
                            rowAmazonTagTemplate.ProductXMLClass_DEV000221 = strTemp ' TJS 24/11/09
                            bRecordChanged = True ' TJS 24/11/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 24/11/09
                            rowAmazonTagTemplate.SetProductXMLClass_DEV000221Null() ' TJS 24/11/09
                            bRecordChanged = True ' TJS 24/11/09
                        ElseIf strTemp <> rowAmazonTagTemplate.ProductXMLClass_DEV000221 Then ' TJS 24/11/09
                            rowAmazonTagTemplate.ProductXMLClass_DEV000221 = strTemp ' TJS 24/11/09
                            bRecordChanged = True ' TJS 24/11/09
                        End If
                    End If
                    If rowAmazonTagTemplate.PlatinumMerchantOnly_DEV000221 <> (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/PlatinumMerchantOnly")).ToUpper = "TRUE") Then ' TJS 24/11/09
                        rowAmazonTagTemplate.PlatinumMerchantOnly_DEV000221 = (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/PlatinumMerchantOnly")).ToUpper = "TRUE") ' TJS 24/11/09
                    End If
                    If rowAmazonTagTemplate.MatrixGroupOnly_DEV000221 <> (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/MatrixGroupOnly")).ToUpper = "TRUE") Then ' TJS 24/11/09
                        rowAmazonTagTemplate.MatrixGroupOnly_DEV000221 = (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/MatrixGroupOnly")).ToUpper = "TRUE") ' TJS 24/11/09
                    End If
                    If rowAmazonTagTemplate.NotMatrixGroup_DEV000221 <> (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/NotMatrixGroup")).ToUpper = "TRUE") Then ' TJS 24/11/09
                        rowAmazonTagTemplate.NotMatrixGroup_DEV000221 = (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/NotMatrixGroup")).ToUpper = "TRUE") ' TJS 24/11/09
                    End If
                    If rowAmazonTagTemplate.MatrixItemOnly_DEV000221 <> (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/MatrixItemOnly")).ToUpper = "TRUE") Then ' TJS 24/11/09
                        rowAmazonTagTemplate.MatrixItemOnly_DEV000221 = (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/MatrixItemOnly")).ToUpper = "TRUE") ' TJS 24/11/09
                    End If
                    If rowAmazonTagTemplate.PlaceHolderOnly_DEV000221 <> (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/PlaceHolderOnly")).ToUpper = "TRUE") Then ' TJS 24/11/09
                        rowAmazonTagTemplate.PlaceHolderOnly_DEV000221 = (m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/PlaceHolderOnly")).ToUpper = "TRUE") ' TJS 24/11/09
                    End If
                    ' only change last modified date if another field has changed since time rounding can cause records to be sent repeatedly
                    If bRecordChanged Then ' TJS 25/05/09
                        rowAmazonTagTemplate.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/LastModified")) ' TJS 25/05/09
                    End If
                End If
            End If
        Next

        ' were any Browse Nodes returned ?
        If GetXMLElementListCount(XMLAmazonTagTemplates) > 0 Then
            ' yes, save updates
            Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
                "CreateLerrynImportExportInventoryTagTemplate_DEV000221", "UpdateLerrynImportExportInventoryTagTemplate_DEV000221", "DeleteLerrynImportExportInventoryTagTemplate_DEV000221"}}, _
                Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Amazon Tag Templates", False)
        End If

    End Sub
#End Region

#Region " GetAmazonXMLTypeDetails "
    Public Sub GetAmazonXMLTypeDetails()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/11/09 | TJS             | 2009.3.09 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTypeDetails As XDocument, XMLTemp As XDocument
        Dim XMLAmazonXMLTypes As System.Collections.Generic.IEnumerable(Of XElement), XMLAmazonXMLType As XElement
        Dim rowAmazonXMLType As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonXMLTypeDetails_DEV000221Row
        Dim strProductXMLClass As String, strProductXMLType As String, strProductXMLSubType As String
        Dim dteLastModified As Date

        ' get last modified date in records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetailsLastModifiedView_DEV000221.TableName, _
            "ReadLrynImpExpAmazonXMLTypeDetailsLastMod_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        If Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetailsLastModifiedView_DEV000221.Count > 0 Then
            If Not Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetailsLastModifiedView_DEV000221(0).IsLastUpdatedNull Then
                dteLastModified = Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetailsLastModifiedView_DEV000221(0).LastUpdated
            Else
                dteLastModified = DateSerial(2009, 1, 1)
            End If
        Else
            dteLastModified = DateSerial(2009, 1, 1)
        End If

        ' now get any updates from LERWEB01
        XMLTypeDetails = m_ImportExportRule.GetAmazonXMLTypeDetails(dteLastModified)
        ' load dataset with records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.TableName, _
            "ReadLerrynImportExportAmazonXMLTypeDetails_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        ' get XML Product Type list
        XMLAmazonXMLTypes = XMLTypeDetails.XPathSelectElements("eShopCONNECT/AmazonXMLTypeDetails")
        ' now process each in turn
        For Each XMLAmazonXMLType In XMLAmazonXMLTypes
            XMLTemp = XDocument.Parse(XMLAmazonXMLType.ToString)
            ' get Primary Key fields
            strProductXMLClass = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonXMLTypeDetails/ProductXMLClass"))
            strProductXMLType = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonXMLTypeDetails/ProductXMLType"))
            ' XMLType field is part of Primary key and can't be null.  IS tries to save empty strings as null so errors
            If strProductXMLType = "" Then
                ' store . as empty field indicator
                strProductXMLType = "."
            End If
            strProductXMLSubType = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonXMLTypeDetails/ProductXMLSubType"))
            ' XMLSubType field is part of Primary key and can't be null.  IS tries to save empty strings as null so errors
            If strProductXMLSubType = "" Then
                ' store . as empty field indicator
                strProductXMLSubType = "."
            End If
            ' does row exist ?
            rowAmazonXMLType = Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.FindByXMLClass_DEV000221XMLSubType_DEV000221XMLType_DEV000221(strProductXMLClass, strProductXMLSubType, strProductXMLType)
            If rowAmazonXMLType Is Nothing Then
                ' no, is record marked as deleted ?
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonXMLTypeDetails/Deleted")).ToUpper <> "TRUE" Then
                    ' no, need to insert it
                    rowAmazonXMLType = Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.NewLerrynImportExportAmazonXMLTypeDetails_DEV000221Row
                    rowAmazonXMLType.XMLClass_DEV000221 = strProductXMLClass
                    rowAmazonXMLType.XMLType_DEV000221 = strProductXMLType
                    rowAmazonXMLType.XMLSubType_DEV000221 = strProductXMLSubType
                    rowAmazonXMLType.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "AmazonXMLTypeDetails/LastModified"))
                    Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.AddLerrynImportExportAmazonXMLTypeDetails_DEV000221Row(rowAmazonXMLType)
                End If

            Else
                ' yes, is record marked as deleted ?
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonXMLTypeDetails/Deleted")).ToUpper = "TRUE" Then
                    ' yes , remove it
                    rowAmazonXMLType.Delete()
                End If

            End If

        Next

        ' were any XML Type Details returned ?
        If GetXMLElementListCount(XMLAmazonXMLTypes) > 0 Then
            ' yes, save updates
            If Not Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportAmazonXMLTypeDetails_DEV000221.TableName, _
                "CreateLerrynImportExportAmazonXMLTypeDetails_DEV000221", "UpdateLerrynImportExportAmazonXMLTypeDetails_DEV000221", "DeleteLerrynImportExportAmazonXMLTypeDetails_DEV000221"}}, _
                Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Amazon Browse Nodes", False) Then
                Throw New Exception("Failed to save Amazon XML Type details")
            End If
        End If

    End Sub
#End Region

#Region " GetEBaySiteList "
    Public Sub GetEBaySiteList(ByRef ActiveEBaySettings As Lerryn.Framework.ImportExport.SourceConfig.eBaySettings, _
        ByVal UseEBaySandbox As Boolean, ByRef EBaySites() As Lerryn.Facade.ImportExport.eBayXMLConnector.eBaySites, ByRef ErrorDetails As String)
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

        Dim eBayConnection As Lerryn.Facade.ImportExport.eBayXMLConnector
        Dim XMLTemp As XDocument, XMLSiteNode As XElement
        Dim XMLSiteList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim iPtr As Integer

        Try

            ErrorDetails = ""
            eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(Me, ActiveEBaySettings, UseEBaySandbox)
            If eBayConnection.GetEBayDetails("SiteDetails") Then
                XMLSiteList = eBayConnection.ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/SiteDetails")
                ReDim EBaySites(GetXMLElementListCount(XMLSiteList) - 1)
                iPtr = 0
                For Each XMLSiteNode In XMLSiteList
                    XMLTemp = XDocument.Parse(XMLSiteNode.ToString)
                    EBaySites(iPtr).SiteName = GetXMLElementText(XMLTemp, "SiteDetails/Site")
                    EBaySites(iPtr).SiteID = CInt(GetXMLElementText(XMLTemp, "SiteDetails/SiteID"))
                    iPtr += 1
                Next
            Else
                ErrorDetails = "Unable to get eBay Options - " & eBayConnection.LastError
            End If

        Catch ex As Exception
            ErrorDetails = "Unable to get eBay Options - " & ex.Message
        End Try

    End Sub
#End Region

#Region " GetEBayShippingServiceList "
    Public Sub GetEBayShippingServiceList(ByRef ActiveEBaySettings As Lerryn.Framework.ImportExport.SourceConfig.eBaySettings, ByVal UseEBaySandbox As Boolean, _
        ByRef EBayShippingCarriers() As Lerryn.Facade.ImportExport.eBayXMLConnector.eBayShippingCarriers, ByRef ErrorDetails As String)
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

        Dim eBayConnection As Lerryn.Facade.ImportExport.eBayXMLConnector
        Dim XMLTemp As XDocument, XMLShippingCarrierNode As XElement
        Dim XMLShippingCarrierList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim iPtr As Integer

        Try

            ErrorDetails = ""
            eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(Me, ActiveEBaySettings, UseEBaySandbox)
            If eBayConnection.GetEBayDetails("ShippingServiceDetails") Then
                XMLShippingCarrierList = eBayConnection.ReturnedXML.XPathSelectElements("GeteBayDetailsResponse/ShippingServiceDetails")
                ReDim EBayShippingCarriers(GetXMLElementListCount(XMLShippingCarrierList) - 1)
                iPtr = 0
                For Each XMLShippingCarrierNode In XMLShippingCarrierList
                    XMLTemp = XDocument.Parse(XMLShippingCarrierNode.ToString)
                    EBayShippingCarriers(iPtr).CarrierName = GetXMLElementText(XMLTemp, "ShippingServiceDetails/Description")
                    EBayShippingCarriers(iPtr).CarrierID = CInt(GetXMLElementText(XMLTemp, "ShippingServiceDetails/ShippingServiceID"))
                    iPtr += 1
                Next
            Else
                ErrorDetails = "Unable to get eBay Options - " & eBayConnection.LastError
            End If

        Catch ex As Exception
            ErrorDetails = "Unable to get eBay Options - " & ex.Message
        End Try

    End Sub
#End Region

#Region " GetEBayCategories "
    Public Sub GetEBayCategories(ByRef EBayConfigSettings As XDocument, ByVal UseEBaySandbox As Boolean, ByRef ErrorDetails As String)
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

        Dim rowEBaySiteSettings As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportEBaySiteSettings_DEV000221Row
        Dim rowEEBayCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportEBayCategories_DEV000221Row
        Dim eBayConnection As Lerryn.Facade.ImportExport.eBayXMLConnector
        Dim ActiveEBaySettings As Lerryn.Framework.ImportExport.SourceConfig.eBaySettings
        Dim XMLTemp As XDocument
        Dim XMLeBaySettingsList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLeBayCategoryList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim strTemp As String, iCountriesChecked() As Integer, iLoop As Integer
        Dim bCategoryUpdateRequired As Boolean, bCountryChecked As Boolean

        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportEBaySiteSettings_DEV000221.TableName, _
            "ReadLerrynImportExportEBaySiteSettings_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        ' initialise list of eBay Countries checked
        ReDim iCountriesChecked(0)
        iCountriesChecked(0) = -1

        ' check every eBay connection
        ActiveEBaySettings = New Lerryn.Framework.ImportExport.SourceConfig.eBaySettings
        XMLeBaySettingsList = EBayConfigSettings.XPathSelectElements(SOURCE_CONFIG_XML_EBAY_LIST)
        For Each EBaySetting As XElement In XMLeBaySettingsList
            XMLTemp = XDocument.Parse(EBaySetting.ToString)
            ActiveEBaySettings.SiteID = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_SITE_ID)
            If GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID") <> "" Then
                ActiveEBaySettings.eBayCountry = CInt(GetXMLElementAttribute(XMLTemp, SOURCE_CONFIG_XML_EBAY_COUNTRY, "ID"))
            Else
                ActiveEBaySettings.eBayCountry = -1
            End If
            ActiveEBaySettings.AuthToken = GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN)
            ActiveEBaySettings.TokenExpires = ConvertXMLDate(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_AUTH_TOKEN_EXPIRES))
            ActiveEBaySettings.AccountDisabled = CBool(IIf(UCase(GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_EBAY_ACCOUNT_DISABLED)) = "YES", True, False))
            ' are connections settings valid ?
            If Not ActiveEBaySettings.AccountDisabled AndAlso ActiveEBaySettings.eBayCountry >= 0 And ActiveEBaySettings.AuthToken <> "" And ActiveEBaySettings.TokenExpires > Date.Now.AddDays(1) Then
                ' yes, have we checked this ebay Country already ?
                bCountryChecked = False
                For iLoop = 0 To iCountriesChecked.Length - 1
                    If iCountriesChecked(iLoop) >= 0 And iCountriesChecked(iLoop) = ActiveEBaySettings.eBayCountry Then
                        bCountryChecked = True
                        Exit For
                    End If
                Next
                If Not bCountryChecked Then
                    ' no, get category version
                    iCountriesChecked(iLoop) = ActiveEBaySettings.eBayCountry
                    bCategoryUpdateRequired = False
                    eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(Me, ActiveEBaySettings, UseEBaySandbox)
                    If eBayConnection.GetCategories(True) Then
                        ' do we have an eBay site record for this country
                        rowEBaySiteSettings = Me.m_ImportExportDataset.LerrynImportExportEBaySiteSettings_DEV000221.FindByCountryID_DEV000221(ActiveEBaySettings.eBayCountry)
                        If rowEBaySiteSettings Is Nothing Then
                            ' no, need to create eBay site record
                            rowEBaySiteSettings = Me.m_ImportExportDataset.LerrynImportExportEBaySiteSettings_DEV000221.NewLerrynImportExportEBaySiteSettings_DEV000221Row
                            rowEBaySiteSettings.CountryID_DEV000221 = ActiveEBaySettings.eBayCountry
                            rowEBaySiteSettings.CategoryVersion_DEV000221 = GetXMLElementText(eBayConnection.ReturnedXML, "GetCategoriesResponse/CategoryVersion")
                            bCategoryUpdateRequired = True

                        ElseIf rowEBaySiteSettings.CategoryVersion_DEV000221 <> GetXMLElementText(eBayConnection.ReturnedXML, "GetCategoriesResponse/CategoryVersion") Then
                            ' site record exists, category version has changed
                            rowEBaySiteSettings.CategoryVersion_DEV000221 = GetXMLElementText(eBayConnection.ReturnedXML, "GetCategoriesResponse/CategoryVersion")
                            bCategoryUpdateRequired = True
                        End If

                        If bCategoryUpdateRequired Then
                            ' update required (new eBay site or category version changed
                            ' update rest of site settings
                            strTemp = GetXMLElementText(eBayConnection.ReturnedXML, "GetCategoriesResponse/ReservePriceAllowed").ToLower
                            If strTemp = "true" And Not rowEBaySiteSettings.ReservePriceAllowed_DEV000221 Then
                                rowEBaySiteSettings.ReservePriceAllowed_DEV000221 = True
                            ElseIf strTemp <> "true" And rowEBaySiteSettings.ReservePriceAllowed_DEV000221 Then
                                rowEBaySiteSettings.ReservePriceAllowed_DEV000221 = False
                            End If
                            strTemp = GetXMLElementText(eBayConnection.ReturnedXML, "GetCategoriesResponse/MinimumReservePrice").ToLower
                            If strTemp = "" Then
                                If rowEBaySiteSettings.MinimumReservePrice_DEV000221 <> 0 Then
                                    rowEBaySiteSettings.MinimumReservePrice_DEV000221 = 0
                                End If
                            Else
                                If rowEBaySiteSettings.MinimumReservePrice_DEV000221 <> CDec(Format(CDec(strTemp), "0.00")) Then
                                    rowEBaySiteSettings.MinimumReservePrice_DEV000221 = CDec(Format(CDec(strTemp), "0.00"))
                                End If
                            End If
                            strTemp = GetXMLElementText(eBayConnection.ReturnedXML, "GetCategoriesResponse/ReduceReserveAllowed").ToLower
                            If strTemp = "true" And Not rowEBaySiteSettings.ReduceReserveAllowed_DEV000221 Then
                                rowEBaySiteSettings.ReduceReserveAllowed_DEV000221 = True
                            ElseIf strTemp <> "true" And rowEBaySiteSettings.ReduceReserveAllowed_DEV000221 Then
                                rowEBaySiteSettings.ReduceReserveAllowed_DEV000221 = False
                            End If
                            If rowEBaySiteSettings.RowState = DataRowState.Added Then
                                Me.m_ImportExportDataset.LerrynImportExportEBaySiteSettings_DEV000221.AddLerrynImportExportEBaySiteSettings_DEV000221Row(rowEBaySiteSettings)
                            End If
                            If Not Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportEBaySiteSettings_DEV000221.TableName, _
                                "CreateLerrynImportExportEBaySiteSettings_DEV000221", "UpdateLerrynImportExportEBaySiteSettings_DEV000221", "DeleteLerrynImportExportEBaySiteSettings_DEV000221"}}, _
                                Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update eBay Site Settings", False) Then
                                Throw New Exception("Failed to save eBay Site Settings for Country ID " & ActiveEBaySettings.eBayCountry)
                            End If
                            ' now get category details
                            If eBayConnection.GetCategories(False) Then
                                Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.TableName, _
                                    "ReadLerrynImportExportEBayCategories_DEV000221", AT_COUNTRY_ID, ActiveEBaySettings.eBayCountry.ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                                ' mark all categories as expired
                                For iLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.Count - 1
                                    Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221(0).Expired_DEV000221 = True
                                Next
                                XMLeBayCategoryList = eBayConnection.ReturnedXML.XPathSelectElements("GetCategoriesResponse/CategoryArray/Category")
                                For Each eBayCategory As XElement In XMLeBayCategoryList
                                    XMLTemp = XDocument.Parse(eBayCategory.ToString)
                                    strTemp = GetXMLElementText(XMLTemp, "Category/CategoryID")
                                    If strTemp <> "" Then
                                        rowEEBayCategory = Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.FindByCountryID_DEV000221CategoryID_DEV000221(ActiveEBaySettings.eBayCountry, CInt(strTemp))
                                        If rowEEBayCategory Is Nothing Then
                                            rowEEBayCategory = Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.NewLerrynImportExportEBayCategories_DEV000221Row
                                            rowEEBayCategory.CountryID_DEV000221 = ActiveEBaySettings.eBayCountry
                                            rowEEBayCategory.CategoryID_DEV000221 = CInt(strTemp)
                                        End If
                                        rowEEBayCategory("Checked") = True
                                        rowEEBayCategory.CategoryParentID_DEV000221 = CInt(GetXMLElementText(XMLTemp, "Category/CategoryParentID"))
                                        rowEEBayCategory.CategoryName_DEV000221 = GetXMLElementText(XMLTemp, "Category/CategoryName")
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/AutoPayEnabled").ToLower = "true" Then
                                            rowEEBayCategory.AutoPayEnabled_DEV000221 = True
                                        Else
                                            rowEEBayCategory.AutoPayEnabled_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/BestOfferEnabled").ToLower = "true" Then
                                            rowEEBayCategory.BestOfferEnabled_DEV000221 = True
                                        Else
                                            rowEEBayCategory.BestOfferEnabled_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/B2BVATEnabled").ToLower = "true" Then
                                            rowEEBayCategory.B2BVATEnabled_DEV000221 = True
                                        Else
                                            rowEEBayCategory.B2BVATEnabled_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/Expired").ToLower = "true" Then
                                            rowEEBayCategory.Expired_DEV000221 = True
                                        Else
                                            rowEEBayCategory.Expired_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/IntlAutosFixedCat").ToLower = "true" Then
                                            rowEEBayCategory.IntlAutosFixedFee_DEV000221 = True
                                        Else
                                            rowEEBayCategory.IntlAutosFixedFee_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/LeafCategory").ToLower = "true" Then
                                            rowEEBayCategory.LeafCategory_DEV000221 = True
                                        Else
                                            rowEEBayCategory.LeafCategory_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/LSD").ToLower = "true" Then
                                            rowEEBayCategory.LotSizeDisabled_DEV000221 = True
                                        Else
                                            rowEEBayCategory.LotSizeDisabled_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/ORPA").ToLower = "true" Then
                                            rowEEBayCategory.OverrideReservePriceAllowed_DEV000221 = True
                                        Else
                                            rowEEBayCategory.OverrideReservePriceAllowed_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/ORRA").ToLower = "true" Then
                                            rowEEBayCategory.OverrideReduceReserveAllowed_DEV000221 = True
                                        Else
                                            rowEEBayCategory.OverrideReduceReserveAllowed_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/SellerGuaranteeEligible").ToLower = "true" Then
                                            rowEEBayCategory.SellerGuaranteeEligible_DEV000221 = True
                                        Else
                                            rowEEBayCategory.SellerGuaranteeEligible_DEV000221 = False
                                        End If
                                        If GetXMLElementText(eBayConnection.ReturnedXML, "Category/Virtual").ToLower = "true" Then
                                            rowEEBayCategory.Virtual_DEV000221 = True
                                        Else
                                            rowEEBayCategory.Virtual_DEV000221 = False
                                        End If

                                        If rowEEBayCategory.RowState = DataRowState.Added Then
                                            Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.AddLerrynImportExportEBayCategories_DEV000221Row(rowEEBayCategory)
                                        End If
                                    End If
                                    If Not Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.TableName, _
                                        "CreateLerrynImportExportEBayCategories_DEV000221", "UpdateLerrynImportExportEBayCategories_DEV000221", "DeleteLerrynImportExportEBayCategories_DEV000221"}}, _
                                        Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update eBay Site Settings", False) Then
                                        Throw New Exception("Failed to save eBay Category Settings for Country ID " & ActiveEBaySettings.eBayCountry)
                                    End If
                                Next

                            End If
                        End If
                    End If
                    eBayConnection = Nothing
                End If
            End If
        Next
    End Sub

    Public Function GetCategoriesFromDatabase(ByRef eBayWrapper As Lerryn.Facade.ImportExport.eBayXMLConnector, _
        Optional ByRef sCategoryParent As String = "", Optional ByRef sLevelLimit As String = "") As Boolean

        Dim iLoop As Integer
        Dim OneCategory As New Lerryn.Facade.ImportExport.eBayXMLConnector.eBayCategory

        eBayWrapper.aCategories.Clear()
        eBayWrapper.aCategoriesSelected.Clear()

        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.TableName, _
        "ReadLerrynImportExportEBayCategories_DEV000221", AT_COUNTRY_ID, eBayWrapper.eBayCountry.ToString}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        For iLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221.Count - 1
            ' Pull the row data into structure and store
            With Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221(iLoop)

                With Me.m_ImportExportDataset.LerrynImportExportEBayCategories_DEV000221(iLoop)
                    OneCategory.BestOfferEnabled = .BestOfferEnabled_DEV000221
                    OneCategory.AutoPayEnabled = .BestOfferEnabled_DEV000221
                    OneCategory.CategoryID = .CategoryID_DEV000221
                    OneCategory.CategoryLevel = .CategoryLevel_DEV000221
                    OneCategory.CategoryName = .CategoryName_DEV000221
                    OneCategory.CategoryParentID = .CategoryParentID_DEV000221
                End With

                ' Squirrel it away
                eBayWrapper.aCategories.Add(OneCategory)
            End With
        Next

        Return True

    End Function
#End Region

#Region " GetShopComAttributeCategories "
    Public Sub GetShopComAttributeCategories(ByVal ParentCategory As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 17/05/09 | TJS             | 2009.2.07 | Function added
        ' 25/05/09 | TJS             | 2009.2.08 | Corrected processing of received Atribute Category items
        ' 08/11/09 | TJS             | 2009.3.09 | Corrected table name when checking for LastUpdated
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLAttributeCategory As XDocument, XMLTemp As XDocument
        Dim XMLShopComAttributeCategories As System.Collections.Generic.IEnumerable(Of XElement), XMLShopComAttributeCategory As XElement
        Dim rowShopComAttributeCategory As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportShopComAttributeCategories_DEV000221Row
        Dim strParentCategory As String, strCategoryName As String, strTemp As String ' TJS 25/05/09
        Dim dteLastModified As Date, bRecordChanged As Boolean ' TJS 25/05/09

        ' get last modified date in records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCatLastModifiedView_DEV000221.TableName, _
            "ReadLrynImpExpShopComAttributeCatLastMod_DEV000221", AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)
        If Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCatLastModifiedView_DEV000221.Count > 0 Then ' TJS 08/11/09
            dteLastModified = Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCatLastModifiedView_DEV000221(0).LastUpdated ' TJS 08/11/09
        Else
            dteLastModified = DateSerial(2009, 1, 1)
        End If

        ' now get any updates from LERWEB01
        XMLAttributeCategory = m_ImportExportRule.GetShopComAttributeCategories(ParentCategory, dteLastModified)
        ' load dataset with records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
            "ReadLerrynImportExportShopComAttributeCategories_DEV000221", AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)
        ' get browse node list
        XMLShopComAttributeCategories = XMLAttributeCategory.XPathSelectElements("eShopCONNECT/ShopComAttributeCategory")
        ' now process each in turn
        For Each XMLShopComAttributeCategory In XMLShopComAttributeCategories
            XMLTemp = XDocument.Parse(XMLShopComAttributeCategory.ToString)
            ' get Primary Key fields
            strParentCategory = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/ParentCategory")) ' TJS 25/05/09
            strCategoryName = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/CategoryName")) ' TJS 25/05/09
            ' does row exist ?
            rowShopComAttributeCategory = Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCategories_DEV000221.FindByParentCategory_DEV000221CategoryName_DEV000221(strParentCategory, strCategoryName)
            If rowShopComAttributeCategory Is Nothing Then
                ' no, is record marked as deleted ?
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/Deleted")).ToUpper <> "TRUE" Then ' TJS 25/05/09
                    ' no, need to insert it
                    rowShopComAttributeCategory = Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCategories_DEV000221.NewLerrynImportExportShopComAttributeCategories_DEV000221Row
                    rowShopComAttributeCategory.ParentCategory_DEV000221 = strParentCategory
                    rowShopComAttributeCategory.CategoryName_DEV000221 = strCategoryName
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/AttributeCategoryString")) ' TJS 25/05/09
                    If strTemp <> "" Then ' TJS 25/05/09
                        rowShopComAttributeCategory.AttributeCategoryString_DEV000221 = strTemp ' TJS 25/05/09
                    End If
                    If Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/IntermediateCategory").ToUpper = "TRUE" Then
                        rowShopComAttributeCategory.IntermediateCategory_DEV000221 = True
                    Else
                        rowShopComAttributeCategory.IntermediateCategory_DEV000221 = False
                    End If
                    rowShopComAttributeCategory.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/LastModified"))
                    Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCategories_DEV000221.AddLerrynImportExportShopComAttributeCategories_DEV000221Row(rowShopComAttributeCategory)
                End If

            Else
                ' need to update it if something has changed
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/Deleted")).ToUpper = "TRUE" Then ' TJS 25/05/09
                    rowShopComAttributeCategory.Delete() ' TJS 25/05/09
                Else
                    ' only change record in one or more fields have changed as save can take a long time otherwise
                    bRecordChanged = False ' TJS 25/05/09
                    strTemp = Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/AttributeCategoryString") ' TJS 25/05/09
                    If rowShopComAttributeCategory.IsAttributeCategoryString_DEV000221Null Then ' TJS 25/05/09
                        If strTemp <> "" Then ' TJS 25/05/09
                            rowShopComAttributeCategory.AttributeCategoryString_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    Else
                        If strTemp = "" Then ' TJS 25/05/09
                            rowShopComAttributeCategory.SetAttributeCategoryString_DEV000221Null()
                            bRecordChanged = True ' TJS 25/05/09
                        ElseIf strTemp <> rowShopComAttributeCategory.AttributeCategoryString_DEV000221 Then ' TJS 25/05/09
                            rowShopComAttributeCategory.AttributeCategoryString_DEV000221 = strTemp ' TJS 25/05/09
                            bRecordChanged = True ' TJS 25/05/09
                        End If
                    End If
                    If Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/IntermediateCategory").ToUpper = "TRUE" Then
                        rowShopComAttributeCategory.IntermediateCategory_DEV000221 = True
                    Else
                        rowShopComAttributeCategory.IntermediateCategory_DEV000221 = False
                    End If
                    ' only change last modified date if another field has changed since time rounding can cause records to be sent repeatedly
                    If bRecordChanged Then ' TJS 25/05/09
                        rowShopComAttributeCategory.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "ShopComAttributeCategory/LastModified"))
                    End If
                End If
            End If
        Next

        ' were any Browse Nodes returned ?
        If GetXMLElementListCount(XMLShopComAttributeCategories) > 0 Then
            ' yes, save updates
            Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportShopComAttributeCategories_DEV000221.TableName, _
                "CreateLerrynImportExportShopComAttributeCategories_DEV000221", "UpdateLerrynImportExportShopComAttributeCategories_DEV000221", "DeleteLerrynImportExportShopComAttributeCategories_DEV000221"}}, _
                Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Shop.com Attribute Categories", False)
        End If

    End Sub
#End Region

#Region " GetShopComTagTemplates "
    Public Sub GetShopComTagTemplates(ByVal ParentCategory As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/06/09 | TJS             | 2009.2.10 | Function added
        ' 28/06/09 | TJS             | 2009.3.00 | Corrected site code used to determine if tag already exists
        ' 15/08/09 | TJS             | 2009.3.03 | Modified to cater for Tag XML Name
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLTagTemplates As XDocument, XMLTemp As XDocument
        Dim XMLShopComTagTemplates As System.Collections.Generic.IEnumerable(Of XElement), XMLShopComTagTemplate As XElement
        Dim rowShopComTagTemplate As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportInventoryTagTemplate_DEV000221Row
        Dim strTagName As String, strParentCategory As String, strTagLocation As String, strTemp As String
        Dim strLineNum As String, dteLastModified As Date, bRecordChanged As Boolean
        Dim strParentNode As String ' TJS 24/11/09

        ' get last modified date in records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplateLastModifiedView_DEV000221.TableName, _
            "ReadLrynImpExpInventoryTagTemplateLastMod_DEV000221", AT_SOURCE_CODE, "ShopCom", AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)
        If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplateLastModifiedView_DEV000221.Count > 0 Then
            dteLastModified = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplateLastModifiedView_DEV000221(0).LastUpdated
        Else
            dteLastModified = DateSerial(2009, 1, 1)
        End If

        ' now get any updates from LERWEB01
        XMLTagTemplates = m_ImportExportRule.GetShopComTagTemplates(ParentCategory, dteLastModified)
        ' load dataset with records received previously from LERWEB01
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
            "ReadLerrynImportExportInventoryTagTemplate_DEV000221", AT_SOURCE_CODE, "ShopCom", AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)
        ' get browse node list
        XMLShopComTagTemplates = XMLTagTemplates.XPathSelectElements("eShopCONNECT/ShopComTagTemplate")
        ' now process each in turn
        For Each XMLShopComTagTemplate In XMLShopComTagTemplates
            XMLTemp = XDocument.Parse(XMLShopComTagTemplate.ToString)
            ' get Primary Key fields
            strParentCategory = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/ParentCategory"))
            strTagLocation = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagLocation"))
            strTagName = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagName"))
            strLineNum = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/LineNum"))
            strParentNode = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "AmazonTagTemplate/ParentNode")) ' TJS 24/11/09
            ' does row exist ?
            rowShopComTagTemplate = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.FindBySourceCode_DEV000221SiteCode_DEV000221ParentCategory_DEV000221TagLocation_DEV000221TagName_DEV000221LineNum_DEV000221ParentNode_DEV000221("ShopCom", "All", CInt(strParentCategory), CInt(strTagLocation), strTagName, CInt(strLineNum), strParentNode) ' TJS 28/06/09 TSJ 24/11/09
            If rowShopComTagTemplate Is Nothing Then
                ' no, is record marked as deleted ?
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/Deleted")).ToUpper <> "TRUE" Then
                    ' no, need to insert it
                    rowShopComTagTemplate = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.NewLerrynImportExportInventoryTagTemplate_DEV000221Row
                    rowShopComTagTemplate.SourceCode_DEV000221 = "ShopCom"
                    rowShopComTagTemplate.SiteCode_DEV000221 = "All"
                    rowShopComTagTemplate.ParentCategory_DEV000221 = CInt(strParentCategory)
                    rowShopComTagTemplate.TagLocation_DEV000221 = CInt(strTagLocation)
                    rowShopComTagTemplate.TagName_DEV000221 = strTagName
                    rowShopComTagTemplate.LineNum_DEV000221 = CInt(strLineNum)
                    rowShopComTagTemplate.ParentNode_DEV000221 = strParentNode ' TJS 24/11/09
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagXMLName")) ' TJS 15/08/09
                    If strTemp <> "" Then ' TJS 15/08/09
                        rowShopComTagTemplate.TagXMLName_DEV000221 = strTemp ' TJS 15/08/09
                    Else
                        rowShopComTagTemplate.TagXMLName_DEV000221 = rowShopComTagTemplate.TagName_DEV000221 ' TJS 15/08/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagDataType"))
                    If strTemp <> "" Then
                        rowShopComTagTemplate.TagDataType_DEV000221 = strTemp
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagOutputOrder")) ' TJS 24/11/09
                    If strTemp <> "" Then ' TJS 24/11/09
                        rowShopComTagTemplate.TagOutputOrder_DEV000221 = CDec(strTemp) ' TJS 24/11/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagStatus"))
                    If strTemp <> "" Then
                        rowShopComTagTemplate.SourceTagStatus_DEV000221 = strTemp
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagConditionality"))
                    If strTemp <> "" Then
                        rowShopComTagTemplate.SourceTagConditionality_DEV000221 = strTemp
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagDescription"))
                    If strTemp <> "" Then
                        rowShopComTagTemplate.SourceTagDescription_DEV000221 = strTemp
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagAcceptedValues"))
                    If strTemp <> "" Then
                        rowShopComTagTemplate.SourceTagAcceptedValues_DEV000221 = strTemp
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagExample"))
                    If strTemp <> "" Then
                        rowShopComTagTemplate.SourceTagExample_DEV000221 = strTemp
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/PlatinumMerchantOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.PlatinumMerchantOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.PlatinumMerchantOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/MatrixGroupOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.MatrixGroupOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.MatrixGroupOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/NotMatrixGroup")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.NotMatrixGroup_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.NotMatrixGroup_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/MatrixItemOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.MatrixItemOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.MatrixItemOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/PlaceHolderOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.PlaceHolderOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.PlaceHolderOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    rowShopComTagTemplate.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/LastModified"))
                    Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.AddLerrynImportExportInventoryTagTemplate_DEV000221Row(rowShopComTagTemplate)
                End If

            Else
                ' need to update it if something has changed
                If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/Deleted")).ToUpper = "TRUE" Then
                    rowShopComTagTemplate.Delete()
                Else
                    ' only change record in one or more fields have changed as save can take a long time otherwise
                    bRecordChanged = False
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagXMLName")) ' TJS 15/08/09
                    If strTemp = "" Then ' TJS 15/08/09
                        rowShopComTagTemplate.TagXMLName_DEV000221 = rowShopComTagTemplate.TagName_DEV000221 ' TJS 15/08/09
                        bRecordChanged = True ' TJS 15/08/09
                    ElseIf strTemp <> rowShopComTagTemplate.TagXMLName_DEV000221 Then ' TJS 15/08/09
                        rowShopComTagTemplate.TagXMLName_DEV000221 = strTemp ' TJS 15/08/09
                        bRecordChanged = True ' TJS 15/08/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagDataType"))
                    If strTemp <> rowShopComTagTemplate.TagDataType_DEV000221 Then
                        rowShopComTagTemplate.TagDataType_DEV000221 = strTemp
                        bRecordChanged = True
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/TagOutputOrder")) ' TJS 24/11/09
                    If strTemp <> "" Then ' TJS 24/11/09
                        rowShopComTagTemplate.TagOutputOrder_DEV000221 = CDec(strTemp) ' TJS 24/11/09
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagStatus"))
                    If rowShopComTagTemplate.IsSourceTagStatus_DEV000221Null Then
                        If strTemp <> "" Then
                            rowShopComTagTemplate.SourceTagStatus_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowShopComTagTemplate.SetSourceTagStatus_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowShopComTagTemplate.SourceTagStatus_DEV000221 Then
                            rowShopComTagTemplate.SourceTagStatus_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagConditionality"))
                    If rowShopComTagTemplate.IsSourceTagConditionality_DEV000221Null Then
                        If strTemp <> "" Then
                            rowShopComTagTemplate.SourceTagConditionality_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowShopComTagTemplate.SetSourceTagConditionality_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowShopComTagTemplate.SourceTagConditionality_DEV000221 Then
                            rowShopComTagTemplate.SourceTagConditionality_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagDescription"))
                    If rowShopComTagTemplate.IsSourceTagDescription_DEV000221Null Then
                        If strTemp <> "" Then
                            rowShopComTagTemplate.SourceTagDescription_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowShopComTagTemplate.SetSourceTagDescription_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowShopComTagTemplate.SourceTagDescription_DEV000221 Then
                            rowShopComTagTemplate.SourceTagDescription_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagAcceptedValues"))
                    If rowShopComTagTemplate.IsSourceTagAcceptedValues_DEV000221Null Then
                        If strTemp <> "" Then
                            rowShopComTagTemplate.SourceTagAcceptedValues_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowShopComTagTemplate.SetSourceTagAcceptedValues_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowShopComTagTemplate.SourceTagAcceptedValues_DEV000221 Then
                            rowShopComTagTemplate.SourceTagAcceptedValues_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    strTemp = m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/SourceTagExample"))
                    If rowShopComTagTemplate.IsSourceTagExample_DEV000221Null Then
                        If strTemp <> "" Then
                            rowShopComTagTemplate.SourceTagExample_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    Else
                        If strTemp = "" Then
                            rowShopComTagTemplate.SetSourceTagExample_DEV000221Null()
                            bRecordChanged = True
                        ElseIf strTemp <> rowShopComTagTemplate.SourceTagExample_DEV000221 Then
                            rowShopComTagTemplate.SourceTagExample_DEV000221 = strTemp
                            bRecordChanged = True
                        End If
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/PlatinumMerchantOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.PlatinumMerchantOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.PlatinumMerchantOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/MatrixGroupOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.MatrixGroupOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.MatrixGroupOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/NotMatrixGroup")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.NotMatrixGroup_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.NotMatrixGroup_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/MatrixItemOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.MatrixItemOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.MatrixItemOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    If m_ImportExportRule.ConvertFromXML(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/PlaceHolderOnly")).ToUpper = "TRUE" Then ' TJS 24/11/09
                        rowShopComTagTemplate.PlaceHolderOnly_DEV000221 = True ' TJS 24/11/09
                    Else
                        rowShopComTagTemplate.PlaceHolderOnly_DEV000221 = False ' TJS 24/11/09
                    End If
                    ' only change last modified date if another field has changed since time rounding can cause records to be sent repeatedly
                    If bRecordChanged Then
                        rowShopComTagTemplate.LastModified_DEV000221 = Me.ConvertXMLDate(Me.GetXMLElementText(XMLTemp, "ShopComTagTemplate/LastModified"))
                    End If
                End If
            End If
        Next

        ' were any Browse Nodes returned ?
        If GetXMLElementListCount(XMLShopComTagTemplates) > 0 Then
            ' yes, save updates
            Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
                "CreateLerrynImportExportInventoryTagTemplate_DEV000221", "UpdateLerrynImportExportInventoryTagTemplate_DEV000221", "DeleteLerrynImportExportInventoryTagTemplate_DEV000221"}}, _
                Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Update Shop.com Tag Templates", False)
        End If

    End Sub
#End Region

#Region " InitialiseInventoryAmazonTags "
    Public Sub InitialiseInventoryAmazonTags(ByVal ItemCode As String, ByVal SiteCode As String, ByVal ProductXMLClass As String, _
        ByVal ProductXMLType As String, ByVal ProductXMLSubType As String, ByVal ParentCategory As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.06 | Function added
        ' 25/05/09 | TJS             | 2009.2.08 | Corrected creation of Item Tag Detail records
        ' 24/11/09 | tjs             | 2009.3.09 | Modified to move code to UpdateInventoryAmazonTags
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonSearchTerm As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryAmazonSearchTermsRow ' TJS 25/05/09
        Dim iItemLoop As Integer, iTemplateLoop As Integer, bRecordExists As Boolean

        ' get existing tag records for Item
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
            "ReadInventoryAmazonTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode, AT_EXCLUDE_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}, _
            New String() {Me.m_ImportExportDataset.InventoryAmazonSearchTerms.TableName, _
            "ReadInventoryAmazonTagDetails_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 25/05/09

        UpdateInventoryAmazonTags(ItemCode, SiteCode, ProductXMLClass, ProductXMLType, ProductXMLSubType, ParentCategory) ' TJS 24/11/09

        ' start of code added TJS 25/05/09
        ' get Amazon search tags 
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
            "ReadLerrynImportExportInventoryTagTemplate_DEV000221", AT_SOURCE_CODE, "Amazon", AT_SITE_CODE, SiteCode, _
            AT_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        ' now process every template record
        For iTemplateLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.Count - 1
            ' check if tag record already exists for item
            bRecordExists = False
            For iItemLoop = 0 To Me.m_ImportExportDataset.InventoryAmazonSearchTerms.Count - 1
                If Me.m_ImportExportDataset.InventoryAmazonSearchTerms(iItemLoop).TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221 And _
                    Me.m_ImportExportDataset.InventoryAmazonSearchTerms(iItemLoop).TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221 And _
                    Me.m_ImportExportDataset.InventoryAmazonSearchTerms(iItemLoop).LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221 Then
                    bRecordExists = True
                    Exit For
                End If
            Next
            If Not bRecordExists Then
                rowAmazonSearchTerm = Me.m_ImportExportDataset.InventoryAmazonSearchTerms.NewInventoryAmazonSearchTermsRow
                ' the Counter field on InventoryAmazonSearchTerms must have AutoIncrement set to true
                ' in the ImportExportDataset.xsd of the Lerryn.Framework,InportExport project
                rowAmazonSearchTerm.ItemCode_DEV000221 = ItemCode
                rowAmazonSearchTerm.TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221
                rowAmazonSearchTerm.TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221
                rowAmazonSearchTerm.LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221
                rowAmazonSearchTerm.TagDataType_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagDataType_DEV000221
                Me.m_ImportExportDataset.InventoryAmazonSearchTerms.AddInventoryAmazonSearchTermsRow(rowAmazonSearchTerm)
            End If
        Next
        ' end of code added TJS 25/05/09

    End Sub
#End Region

#Region " UpdateInventoryAmazonTags "
    Public Sub UpdateInventoryAmazonTags(ByVal ItemCode As String, ByVal SiteCode As String, ByVal ProductXMLClass As String, _
        ByVal ProductXMLType As String, ByVal ProductXMLSubType As String, ByVal ParentCategory As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/11/09 | TJS             | 2009.3.09 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryAmazonTagDetailTemplateView_DEV000221Row
        Dim iItemLoop As Integer, iTemplateLoop As Integer, bRecordExists As Boolean, bProductClassTypeMatch As Boolean ' TJS 24/11/09
        Dim bProductClassTypeMissMatch As Boolean ' TJS 24/11/09

        ' get common Amazon tags 
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
            "ReadLerrynImportExportInventoryTagTemplate_DEV000221", AT_SOURCE_CODE, "Amazon", AT_SITE_CODE, SiteCode, _
            AT_PARENT_CATEGORY, AMAZON_ROOT_CATEGORY, AT_EXCLUDE_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 25/05/09

        ' now process every template record
        For iTemplateLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.Count - 1
            ' ignore any deleted rows
            If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).RowState <> DataRowState.Deleted Then ' TJS 24/11/09
                ' check if tag record already exists for item
                bRecordExists = False
                For iItemLoop = 0 To Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.Count - 1
                    If Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iItemLoop).TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221 And _
                        Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iItemLoop).TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221 And _
                        Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iItemLoop).LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221 Then
                        bRecordExists = True
                        Exit For
                    End If
                Next
                If Not bRecordExists Then
                    bProductClassTypeMatch = False ' TJS 24/11/09
                    bProductClassTypeMissMatch = False ' TJS 24/11/09
                    If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsProductXMLClass_DEV000221Null Then ' TJS 24/11/09
                        ' can't have ProductXMLType or ProductXMLSubType if ProductXMLClass is empty
                        bProductClassTypeMatch = True ' TJS 24/11/09

                    ElseIf Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLClass_DEV000221 = "" Then ' TJS 24/11/09
                        ' can't have ProductXMLType or ProductXMLSubType if ProductXMLClass is empty
                        bProductClassTypeMatch = True ' TJS 24/11/09

                    ElseIf CompareProductXMLClassOrType(ProductXMLClass, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLClass_DEV000221) Then ' TJS 24/11/09
                        ' ProductXMLClass is set and match, is ProductXMLType set ?
                        If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsProductXMLType_DEV000221Null Then ' TJS 24/11/09
                            ' can't have ProductXMLSubType if ProductXMLType is empty even though ProductXMLClass is set
                            bProductClassTypeMatch = True ' TJS 24/11/09

                        ElseIf Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLType_DEV000221 = "" Then ' TJS 24/11/09
                            ' can't have ProductXMLSubType if ProductXMLType is empty even though ProductXMLClass is set
                            bProductClassTypeMatch = True ' TJS 24/11/09

                        ElseIf CompareProductXMLClassOrType(ProductXMLType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLType_DEV000221) Then ' TJS 24/11/09
                            ' ProductXMLClass and ProductXMLType are set and matches, is ProductXMLSubType set ?
                            If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsProductXMLSubType_DEV000221Null Then ' TJS 24/11/09
                                ' ProductXMLSubType is empty so can't be wrong
                                bProductClassTypeMatch = True ' TJS 24/11/09

                            ElseIf Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLSubType_DEV000221 = "" Then ' TJS 24/11/09
                                ' ProductXMLSubType is empty so can't be wrong
                                bProductClassTypeMatch = True ' TJS 24/11/09

                            ElseIf CompareProductXMLClassOrType(ProductXMLSubType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLSubType_DEV000221) Then ' TJS 24/11/09
                                ' ProductXMLClass, ProductXMLType and ProductXMLSubType are all set and match
                                bProductClassTypeMatch = True ' TJS 24/11/09

                            ElseIf Not CompareProductXMLClassOrType(ProductXMLSubType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLSubType_DEV000221) And ProductXMLSubType <> "" Then ' TJS 24/11/09
                                ' ProductXMLClass and ProductXMLType are set, but ProductXMLType does not match tag value 
                                bProductClassTypeMissMatch = True ' TJS 24/11/09

                            End If

                        ElseIf Not CompareProductXMLClassOrType(ProductXMLType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLType_DEV000221) And ProductXMLType <> "" Then ' TJS 24/11/09
                            ' ProductXMLClass and ProductXMLType are set, but ProductXMLType does not match tag value 
                            bProductClassTypeMissMatch = True ' TJS 24/11/09

                        End If

                    ElseIf Not CompareProductXMLClassOrType(ProductXMLClass, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLClass_DEV000221) And ProductXMLClass <> "" Then ' TJS 24/11/09
                        ' ProductXMLClass is set, but does not match tag value 
                        bProductClassTypeMissMatch = True ' TJS 24/11/09

                    End If
                    If bProductClassTypeMatch Then ' TJS 24/11/09
                        rowAmazonTagDetails = Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.NewInventoryAmazonTagDetailTemplateView_DEV000221Row
                        ' the Counter field on InventoryAmazonTagDetailTemplateView_DEV000221 must have AutoIncrement set to true
                        ' in the ImportExportDataset.xsd of the Lerryn.Framework,InportExport project
                        rowAmazonTagDetails.ItemCode_DEV000221 = ItemCode
                        rowAmazonTagDetails.TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.TagDataType_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagDataType_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.SourceCode_DEV000221 = "Amazon"
                        rowAmazonTagDetails.ParentCategory_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ParentCategory_DEV000221 ' TJS 25/05/09
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagStatus_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagStatus_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagStatus_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagConditionality_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagConditionality_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagConditionality_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagDescription_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagDescription_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagDescription_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagAcceptedValues_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagAcceptedValues_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagAcceptedValues_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagExample_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagExample_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagExample_DEV000221 ' TJS 25/05/09
                        End If
                        Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.AddInventoryAmazonTagDetailTemplateView_DEV000221Row(rowAmazonTagDetails)
                    End If
                ElseIf bProductClassTypeMissMatch Then ' TJS 24/11/09
                    Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).Delete() ' TJS 24/11/09
                End If
            End If
        Next

        ' get category specific Amazon tags 
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
            "ReadLerrynImportExportInventoryTagTemplate_DEV000221", AT_SOURCE_CODE, "Amazon", AT_SITE_CODE, SiteCode, _
            AT_PARENT_CATEGORY, ParentCategory, AT_EXCLUDE_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 25/05/09

        ' now process every template record
        For iTemplateLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.Count - 1
            ' ignore any deleted rows
            If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).RowState <> DataRowState.Deleted Then ' TJS 24/11/09
                ' check if tag record already exists for item
                bRecordExists = False
                For iItemLoop = 0 To Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.Count - 1
                    If Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iItemLoop).TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221 And _
                        Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iItemLoop).TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221 And _
                        Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221(iItemLoop).LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221 Then
                        bRecordExists = True
                        Exit For
                    End If
                Next
                If Not bRecordExists Then
                    bProductClassTypeMatch = False ' TJS 24/11/09
                    bProductClassTypeMissMatch = False ' TJS 24/11/09
                    If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsProductXMLClass_DEV000221Null Then ' TJS 24/11/09
                        ' can't have ProductXMLType or ProductXMLSubType if ProductXMLClass is empty
                        bProductClassTypeMatch = True ' TJS 24/11/09

                    ElseIf Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLClass_DEV000221 = "" Then ' TJS 24/11/09
                        ' can't have ProductXMLType or ProductXMLSubType if ProductXMLClass is empty
                        bProductClassTypeMatch = True ' TJS 24/11/09

                    ElseIf CompareProductXMLClassOrType(ProductXMLClass, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLClass_DEV000221) Then ' TJS 24/11/09
                        ' ProductXMLClass is set and match, is ProductXMLType set ?
                        If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsProductXMLType_DEV000221Null Then ' TJS 24/11/09
                            ' can't have ProductXMLSubType if ProductXMLType is empty even though ProductXMLClass is set
                            bProductClassTypeMatch = True ' TJS 24/11/09

                        ElseIf Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLType_DEV000221 = "" Then ' TJS 24/11/09
                            ' can't have ProductXMLSubType if ProductXMLType is empty even though ProductXMLClass is set
                            bProductClassTypeMatch = True ' TJS 24/11/09

                        ElseIf CompareProductXMLClassOrType(ProductXMLType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLType_DEV000221) Then ' TJS 24/11/09
                            ' ProductXMLClass and ProductXMLType are set and matches, is ProductXMLSubType set ?
                            If Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsProductXMLSubType_DEV000221Null Then ' TJS 24/11/09
                                ' ProductXMLSubType is empty so can't be wrong
                                bProductClassTypeMatch = True ' TJS 24/11/09

                            ElseIf Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLSubType_DEV000221 = "" Then ' TJS 24/11/09
                                ' ProductXMLSubType is empty so can't be wrong
                                bProductClassTypeMatch = True ' TJS 24/11/09

                            ElseIf CompareProductXMLClassOrType(ProductXMLSubType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLSubType_DEV000221) Then ' TJS 24/11/09
                                ' ProductXMLClass, ProductXMLType and ProductXMLSubType are all set and match
                                bProductClassTypeMatch = True ' TJS 24/11/09

                            ElseIf Not CompareProductXMLClassOrType(ProductXMLSubType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLSubType_DEV000221) And ProductXMLSubType <> "" Then ' TJS 24/11/09
                                ' ProductXMLClass and ProductXMLType are set, but ProductXMLType does not match tag value 
                                bProductClassTypeMissMatch = True ' TJS 24/11/09

                            End If

                        ElseIf Not CompareProductXMLClassOrType(ProductXMLType, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLType_DEV000221) And ProductXMLType <> "" Then ' TJS 24/11/09
                            ' ProductXMLClass and ProductXMLType are set, but ProductXMLType does not match tag value 
                            bProductClassTypeMissMatch = True ' TJS 24/11/09

                        End If

                    ElseIf Not CompareProductXMLClassOrType(ProductXMLClass, Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ProductXMLClass_DEV000221) And ProductXMLClass <> "" Then ' TJS 24/11/09
                        ' ProductXMLClass is set, but does not match tag value 
                        bProductClassTypeMissMatch = True ' TJS 24/11/09

                    End If
                    If bProductClassTypeMatch Then ' TJS 24/11/09
                        rowAmazonTagDetails = Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.NewInventoryAmazonTagDetailTemplateView_DEV000221Row
                        ' the Counter field on InventoryAmazonTagDetailTemplateView_DEV000221 must have AutoIncrement set to true
                        ' in the ImportExportDataset.xsd of the Lerryn.Framework,InportExport project
                        rowAmazonTagDetails.ItemCode_DEV000221 = ItemCode
                        rowAmazonTagDetails.TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.TagDataType_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagDataType_DEV000221 ' TJS 25/05/09
                        rowAmazonTagDetails.SourceCode_DEV000221 = "Amazon"
                        rowAmazonTagDetails.ParentCategory_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ParentCategory_DEV000221 ' TJS 25/05/09
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagStatus_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagStatus_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagStatus_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagConditionality_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagConditionality_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagConditionality_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagDescription_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagDescription_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagDescription_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagAcceptedValues_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagAcceptedValues_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagAcceptedValues_DEV000221 ' TJS 25/05/09
                        End If
                        If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagExample_DEV000221Null Then ' TJS 25/05/09
                            rowAmazonTagDetails.SourceTagExample_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagExample_DEV000221 ' TJS 25/05/09
                        End If
                        Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.AddInventoryAmazonTagDetailTemplateView_DEV000221Row(rowAmazonTagDetails)
                    End If
                ElseIf bProductClassTypeMissMatch Then ' TJS 24/11/09
                    Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).Delete() ' TJS 24/11/09
                End If
            End If
        Next

    End Sub
#End Region

#Region " InitialiseInventoryShopComTags "
    Public Sub InitialiseInventoryShopComTags(ByVal ItemCode As String, ByVal ParentCategory As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/06/09 | TJS             | 2009.2.10 | Function added
        ' 18/07/09 | TJS             | 2009.3.03 | Corrected Source Code value when loading category specific tags
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowShopComTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryShopComTagDetailTemplateView_DEV000221Row
        Dim iItemLoop As Integer, iTemplateLoop As Integer, bRecordExists As Boolean

        ' get existing tag records for Item
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221.TableName, _
            "ReadInventoryShopComTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        ' get common Shop.com tags 
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
            "ReadLerrynImportExportInventoryTagTemplate_DEV000221", AT_SOURCE_CODE, "ShopCom", AT_PARENT_CATEGORY, SHOPCOM_ROOT_CATEGORY}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)

        ' now process every template record
        For iTemplateLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.Count - 1
            ' check if tag record already exists for item
            bRecordExists = False
            For iItemLoop = 0 To Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221.Count - 1
                If Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221(iItemLoop).TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221 And _
                    Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221(iItemLoop).TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221 And _
                    Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221(iItemLoop).LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221 Then
                    bRecordExists = True
                    Exit For
                End If
            Next
            If Not bRecordExists Then
                rowShopComTagDetails = Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221.NewInventoryShopComTagDetailTemplateView_DEV000221Row
                rowShopComTagDetails.ItemCode_DEV000221 = ItemCode
                rowShopComTagDetails.TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221
                rowShopComTagDetails.TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221
                rowShopComTagDetails.LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221
                rowShopComTagDetails.TagDataType_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagDataType_DEV000221
                rowShopComTagDetails.SourceCode_DEV000221 = "ShopCom"
                rowShopComTagDetails.ParentCategory_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ParentCategory_DEV000221
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagStatus_DEV000221Null Then
                    rowShopComTagDetails.SourceTagStatus_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagStatus_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagConditionality_DEV000221Null Then
                    rowShopComTagDetails.SourceTagConditionality_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagConditionality_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagDescription_DEV000221Null Then
                    rowShopComTagDetails.SourceTagDescription_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagDescription_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagAcceptedValues_DEV000221Null Then
                    rowShopComTagDetails.SourceTagAcceptedValues_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagAcceptedValues_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagExample_DEV000221Null Then
                    rowShopComTagDetails.SourceTagExample_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagExample_DEV000221
                End If
                Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221.AddInventoryShopComTagDetailTemplateView_DEV000221Row(rowShopComTagDetails)
            End If
        Next

        ' get category specific Shop.com tags 
        Me.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.TableName, _
            "ReadLerrynImportExportInventoryTagTemplate_DEV000221", AT_SOURCE_CODE, "ShopCom", AT_PARENT_CATEGORY, ParentCategory}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)

        ' now process every template record
        For iTemplateLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221.Count - 1
            ' check if tag record already exists for item
            bRecordExists = False
            For iItemLoop = 0 To Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221.Count - 1
                If Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221(iItemLoop).TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221 And _
                    Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221(iItemLoop).TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221 And _
                    Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221(iItemLoop).LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221 Then
                    bRecordExists = True
                    Exit For
                End If
            Next
            If Not bRecordExists Then
                rowShopComTagDetails = Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221.NewInventoryShopComTagDetailTemplateView_DEV000221Row
                rowShopComTagDetails.ItemCode_DEV000221 = ItemCode
                rowShopComTagDetails.TagLocation_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagLocation_DEV000221
                rowShopComTagDetails.TagName_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagName_DEV000221
                rowShopComTagDetails.LineNum_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).LineNum_DEV000221
                rowShopComTagDetails.TagDataType_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).TagDataType_DEV000221
                rowShopComTagDetails.SourceCode_DEV000221 = "ShopCom"
                rowShopComTagDetails.ParentCategory_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).ParentCategory_DEV000221
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagStatus_DEV000221Null Then
                    rowShopComTagDetails.SourceTagStatus_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagStatus_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagConditionality_DEV000221Null Then
                    rowShopComTagDetails.SourceTagConditionality_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagConditionality_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagDescription_DEV000221Null Then
                    rowShopComTagDetails.SourceTagDescription_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagDescription_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagAcceptedValues_DEV000221Null Then
                    rowShopComTagDetails.SourceTagAcceptedValues_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagAcceptedValues_DEV000221
                End If
                If Not Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).IsSourceTagExample_DEV000221Null Then
                    rowShopComTagDetails.SourceTagExample_DEV000221 = Me.m_ImportExportDataset.LerrynImportExportInventoryTagTemplate_DEV000221(iTemplateLoop).SourceTagExample_DEV000221
                End If
                Me.m_ImportExportDataset.InventoryShopComTagDetailTemplateView_DEV000221.AddInventoryShopComTagDetailTemplateView_DEV000221Row(rowShopComTagDetails)
            End If
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

        Return m_ImportExportRule.CheckInventoryImportLimit(ProductsImportedToDate, DuringImport, OverlimitMessage)

    End Function
#End Region

#Region " Validate "
    Public Overrides Function Validate(ByVal row As System.Data.DataRow, ByVal columnName As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.06 | Function added
        ' 25/05/09 | TJS             | 2009.2.08 | Modified to validate InventoryAmazonDetails_DEV000221
        ' 05/06/09 | TJS             | 2009.2.10 | Modified to validate InventoryShopComDetails_DEV000221  
        '                                        | and InventoryShopComTagDetails_DEV000221
        ' 15/12/09 | TJS             | 2009.3.09 | Modified to validate LerrynImportExportDeliveryMethods_DEV000221
        ' 27/09/10 | TJs             | 2010.1.02 | Modified to addLerrynImportExportPaymentTypes_DEV000221 and 
        '                                        | LerrynImportExportSKUAliases_DEV000221
        ' 18/03/11 | TJS             | 2011.0.01 | Modified for eCommerce Plus version
        ' 25/04/11 | TJS             | 2011.0.12 | Added Magento and ASPStorefront validation
        ' 10/06/12 | TJS/FA          | 2102.1.05 | Added Channel Advisor validation
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bReturnValue As Boolean

        Try
            bReturnValue = MyBase.Validate(row, columnName)
            If bReturnValue Then ' TJS 18/03/11
                row.SetColumnError(columnName, "") ' TJS 18/03/11
            End If
            Select Case row.Table.TableName
                Case Me.m_ImportExportDataset.InventoryAmazonDetails_DEV000221.TableName ' TJS 25/05/09
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryAmazonDetails(row, columnName, bReturnValue) ' TJS 25/05/09

                Case Me.m_ImportExportDataset.InventoryAmazonSearchTerms.TableName ' TJS 25/05/09
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryAmazonSearchTerms(row, columnName, bReturnValue) ' TJS 25/05/09

                Case Me.m_ImportExportDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryAmazonTagDetails(row, columnName, bReturnValue)

                Case Me.m_ImportExportDataset.InventoryASPStorefrontDetails_DEV000221.TableName ' TJS  25/04/11
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryASPStorefrontDetails(row, columnName, bReturnValue) ' TJS 25/04/11

                Case Me.m_ImportExportDataset.InventoryASPStorefrontCategories_DEV000221.TableName ' TJS  25/04/11
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryASPStorefrontCategories(row, columnName, bReturnValue) ' TJS 25/04/11

                Case Me.m_ImportExportDataset.InventoryASPStorefrontTagDetails_DEV000221.TableName ' TJS  25/04/11
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryASPStorefrontTagDetails(row, columnName, bReturnValue) ' TJS 25/04/11

                Case Me.m_ImportExportDataset.InventoryChannelAdvDetails_DEV000221.TableName ' TJS/FA 10/06/12
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryChannelAdvDetails(row, columnName, bReturnValue) ' TJS/FA 10/06/12

                Case Me.m_ImportExportDataset.InventoryMagentoDetails_DEV000221.TableName ' TJS  25/04/11
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryMagentoDetails(row, columnName, bReturnValue) ' TJS 25/04/11

                Case Me.m_ImportExportDataset.InventoryMagentoCategories_DEV000221.TableName ' TJS  25/04/11
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryMagentoCategories(row, columnName, bReturnValue) ' TJS 25/04/11

                Case Me.m_ImportExportDataset.InventoryMagentoTagDetails_DEV000221.TableName ' TJS  25/04/11
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryMagentoTagDetails(row, columnName, bReturnValue) ' TJS 25/04/11

                Case Me.m_ImportExportDataset.InventoryShopComDetails_DEV000221.TableName ' TJS 05/06/09
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryShopComDetails(row, columnName, bReturnValue) ' TJS 05/06/09

                Case Me.m_ImportExportDataset.InventoryShopComTagDetails_DEV000221.TableName ' TJS 05/06/09
                    bReturnValue = Me.m_ImportExportRule.ValidateInventoryShopComTagDetails(row, columnName, bReturnValue) ' TJS 05/06/09

                Case Me.m_ImportExportDataset.LerrynImportExportDeliveryMethods_DEV000221.TableName ' TJS 15/12/09
                    bReturnValue = Me.m_ImportExportRule.ValidateLerrynImportExportDeliveryMethods(row, columnName, bReturnValue, m_ChannelAdvCarriers) ' TJS 15/12/09

                Case Me.m_ImportExportDataset.LerrynImportExportPaymentTypes_DEV000221.TableName ' TJS 27/09/10
                    bReturnValue = Me.m_ImportExportRule.ValidateLerrynImportExportPaymentTypes(row, columnName, bReturnValue) ' TJS 27/09/10

                Case Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.TableName ' TJS 27/09/10 TJS 03/07/13
                    bReturnValue = Me.m_ImportExportRule.ValidateLerrynImportExportSKUAliases(row, columnName, bReturnValue) ' TJS 27/09/10

                Case Me.m_ImportExportDataset.ActivationAccountDetails.TableName ' TJS 18/03/11
                    bReturnValue = Me.m_ImportExportRule.ValidateActivationAccountDetails(row, columnName, bReturnValue) ' TJS 18/03/11

            End Select
            Return bReturnValue

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Function
#End Region

#Region " CompareProductXMLClassOrType "
    Private Function CompareProductXMLClassOrType(ByVal ItemXMLClassOrType As String, ByVal TagXMLClassOrType As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/11/09 | TJS             | 2009.3.09 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, strClassTypeArray() As String

        ' does Class or Type contain multiple options i.e. contains : as separator
        If TagXMLClassOrType.IndexOf(":") > 0 Then
            ' Class or Type contains multiple options 
            strClassTypeArray = Split(TagXMLClassOrType, ":")
            For iLoop = 0 To strClassTypeArray.Length - 1
                If ItemXMLClassOrType = strClassTypeArray(iLoop) Then
                    Return True
                End If
            Next
            Return False
        Else
            ' Class or Type contains only a single option, 
            Return (ItemXMLClassOrType = TagXMLClassOrType)

        End If
    End Function
#End Region

#Region " CheckWebsiteCodeExists "
    Private Sub m_ImportExportRule_CheckWebsiteCodeExists(ByVal sender As Object, ByVal SourceCode As String, _
        ByVal AccountOrInstanceID As String, ByVal WebSiteCodeToUse As String, ByVal DescriptionToUse As String, _
        ByVal BusinessTypeToUse As String) Handles m_ImportExportRule.CheckWebsiteCodeExists ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/04/11 | TJS             | 2011.0.11 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for IS 6 and to prevent web site being created for blank Account/Instance IDs
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSiteDataset As Interprise.Framework.ECommerce.DatasetGateway.WebSiteDatasetGateway
        Dim WebSiteFacade As Interprise.Facade.ECommerce.WebSiteFacade
        Dim strWebSiteCode As String, strErrorDetails As String, iColumnLoop As Integer

        strWebSiteCode = Me.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & SourceCode & "' AND AccountOrInstanceID_DEV000221 = '" & AccountOrInstanceID.Replace("'", "''") & "'") ' TJS 02/12/11
        If "" & strWebSiteCode = "" And "" & AccountOrInstanceID <> "" Then ' TJS 02/12/11
            Try
                WebSiteDataset = New Interprise.Framework.ECommerce.DatasetGateway.WebSiteDatasetGateway
                WebSiteFacade = New Interprise.Facade.ECommerce.WebSiteFacade(WebSiteDataset)
                WebSiteFacade.AddWebSite(WebSiteCodeToUse, "", "", DescriptionToUse)
                WebSiteDataset.EcommerceSite(0).WebSiteURL = "eShopCONNECTED"
                WebSiteDataset.EcommerceSite(0).WebServiceURL = "eShopCONNECTED"
                WebSiteDataset.EcommerceSite(0).CustomerCode = "DefaultECommerceShopper"
                WebSiteFacade.UpdateBusinessType(BusinessTypeToUse) ' TJS 02/12/11
                WebSiteDataset.EcommerceSite(0)("SourceCode_DEV000221") = SourceCode ' TJS 02/12/11
                WebSiteDataset.EcommerceSite(0)("AccountOrInstanceID_DEV000221") = AccountOrInstanceID ' TJS 02/12/11
                WebSiteDataset.EcommerceSite(0).CustomerNameTemp = "DefaultECommerceShopper" ' TJS 02/12/11
                Try
                    If Not WebSiteFacade.UpdateDataSet(New String()() {New String() {WebSiteDataset.EcommerceSite.TableName, "CreateEcommerceSite", "UpdateEcommerceSite", "DeleteEcommerceSite"}}, _
                        Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Create EcommerceSite", False) Then
                        strErrorDetails = ""
                        For iColumnLoop = 0 To WebSiteDataset.EcommerceSite.Columns.Count - 1
                            If WebSiteDataset.EcommerceSite(0).GetColumnError(iColumnLoop) <> "" Then
                                strErrorDetails = strErrorDetails & WebSiteDataset.EcommerceSite.TableName & "." & WebSiteDataset.EcommerceSite.Columns(iColumnLoop).ColumnName & _
                                    ", " & WebSiteDataset.EcommerceSite(0).GetColumnError(iColumnLoop) & vbCrLf
                            End If
                        Next
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Unable to create EECommerceSite record - " & strErrorDetails) ' TJS 02/12/11
                    End If

                Catch ex2 As Exception
                    ' in IS 6, seems to error the first time we try to save so have a second attempt
                    Try
                        If Not WebSiteFacade.UpdateDataSet(New String()() {New String() {WebSiteDataset.EcommerceSite.TableName, "CreateEcommerceSite", "UpdateEcommerceSite", "DeleteEcommerceSite"}}, _
                            Interprise.Framework.Base.Shared.Enum.TransactionType.None, "Create EcommerceSite", False) Then
                            strErrorDetails = ""
                            For iColumnLoop = 0 To WebSiteDataset.EcommerceSite.Columns.Count - 1
                                If WebSiteDataset.EcommerceSite(0).GetColumnError(iColumnLoop) <> "" Then
                                    strErrorDetails = strErrorDetails & WebSiteDataset.EcommerceSite.TableName & "." & WebSiteDataset.EcommerceSite.Columns(iColumnLoop).ColumnName & _
                                        ", " & WebSiteDataset.EcommerceSite(0).GetColumnError(iColumnLoop) & vbCrLf
                                End If
                            Next
                            Interprise.Presentation.Base.Message.MessageWindow.Show(strErrorDetails)
                        End If

                    Catch ex3 As Exception
                        Interprise.Presentation.Base.Message.MessageWindow.Show(ex3)

                    End Try

                End Try

            Catch ex As Exception
                Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

            End Try
        End If

    End Sub
#End Region

#Region " CheckWebsiteCodeExists "
    Private Sub GetMagentoStoreList(ByVal sender As Object, ByRef XMLAccountConfig As System.Xml.Linq.XDocument, _
        ByRef XMLStoreList As System.Xml.Linq.XDocument) Handles m_ImportExportRule.GetMagentoStoreList
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to decode Config XML Value such as password etc and cater for XML entities
        ' 02/10/13 | TJS             | 2013.3.03 | Modified to save Magento Version in config
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2SoapAPIWSICompliant and API Version
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim MagentoConnection As Lerryn.Facade.ImportExport.MagentoSOAPConnector, XMLVersion As XElement ' 02/10/13
        Dim bMagentoV2APIWSI As Boolean ' TJS 05/0/13

        bMagentoV2APIWSI = CBool(IIf(UCase(GetXMLElementText(XMLAccountConfig, SOURCE_CONFIG_XML_MAGENTO_V2_SOAP_API_WSI_COMPLIANT)) = "YES", True, False)) ' TJS 13/11/13
        MagentoConnection = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
        MagentoConnection.V2SoapAPIWSICompliant = bMagentoV2APIWSI ' TJS 13/11/13
        If GetXMLElementText(XMLAccountConfig, SOURCE_CONFIG_XML_MAGENTO_API_URL) <> "" And GetXMLElementText(XMLAccountConfig, "Magento/AccountDisabled").ToUpper <> "YES" Then ' TJS 13/11/13
            Try
                If MagentoConnection.Login(m_ImportExportRule.DecodeConfigXMLValue(GetXMLElementText(XMLAccountConfig, SOURCE_CONFIG_XML_MAGENTO_API_URL)), _
                    m_ImportExportRule.ConvertForXML(m_ImportExportRule.DecodeConfigXMLValue(GetXMLElementText(XMLAccountConfig, SOURCE_CONFIG_XML_MAGENTO_API_USER))), _
                    m_ImportExportRule.ConvertForXML(m_ImportExportRule.DecodeConfigXMLValue(GetXMLElementText(XMLAccountConfig, SOURCE_CONFIG_XML_MAGENTO_API_PASSWORD))), True) Then ' TJS 19/09/13 TJS 13/11/13
                    If MagentoConnection.GetStoreList() Then
                        XMLStoreList = MagentoConnection.ReturnedXML
                    End If
                    ' start of code added TJS 02/10/13
                    If MagentoConnection.MagentoVersion <> "" Then
                        XMLVersion = XMLAccountConfig.XPathSelectElement(SOURCE_CONFIG_XML_MAGENTO_VERSION)
                        If XMLVersion IsNot Nothing Then
                            If XMLVersion.Value <> MagentoConnection.MagentoVersion Then ' TJS 13/11/13
                                XMLVersion.Value = MagentoConnection.MagentoVersion
                            End If
                        End If
                    End If
                    ' end of code added TJS 02/10/13
                    ' start of code added TJS 13/11/13
                    If MagentoConnection.LerrynAPIVersion <> 0 Then ' TJS 13/11/13
                        XMLVersion = XMLAccountConfig.XPathSelectElement(SOURCE_CONFIG_XML_MAGENTO_API_EXTN_VERSION)
                        If XMLVersion IsNot Nothing Then
                            If XMLVersion.Value <> MagentoConnection.LerrynAPIVersion.ToString Then
                                XMLVersion.Value = MagentoConnection.LerrynAPIVersion.ToString
                            End If
                        End If
                    End If
                    ' end of code added TJS 13/11/13
                    MagentoConnection.Logout()
                End If

            Catch ex As Exception
                Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

            End Try
        End If

    End Sub
#End Region

#Region " CreateSKUAliasTemplate "
    Public Function CreateSKUAliasTemplate(ByVal FileOutputPath As String, ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Function added
        '------------------------------------------------------------------------------------------

        Dim SKUAliasFile As StreamWriter

        Try
            ' create file and write header
            SKUAliasFile = InitiateSKUAliasFile(FileOutputPath)
            ' close file
            SKUAliasFile.Close()
            Return True

        Catch ex As Exception
            ErrorDetails = ex.Message
            Return False

        End Try

    End Function

    Private Function InitiateSKUAliasFile(ByVal FileOutputPath As String) As StreamWriter
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Function added
        ' 03/07/13 | TJS             | 2013.1.24 | Added CB Item Code field and renamed Item Name 
        ' 12/07/13 | TJS             | 2013.1.28 | Removed quotes round field names and space after comma 
        '                                        | as Excel screws up save when present
        '------------------------------------------------------------------------------------------

        InitiateSKUAliasFile = New StreamWriter(FileOutputPath, False)
        InitiateSKUAliasFile.WriteLine("Source Code,Source SKU,CB Item Name,CB Item Code") ' TJS 12/07/13

    End Function
#End Region

#Region " ExportSKUAliasRecords "
    Public Function ExportSKUAliasRecords(ByVal FileOutputPath As String, ByVal SourceCode As String, ByRef ErrorDetails As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Function added
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221 and added Item Name field
        ' 05/07/13 | TJS             | 2013.1.27 | Modified to cater for ItemName being null
        ' 12/07/13 | TJS             | 2013.1.28 | Removed quotes round field values unless they contain a comma
        '                                        | as Excel screws up save when present
        ' 29/01/14 | TJS             | 2013.4.07 | Modified to check for commas in the Source SKU and remove leading " from line unless sku contains a comma
        '------------------------------------------------------------------------------------------

        Dim SKUAliasFile As StreamWriter = Nothing, strOutputLine As String, iLoop As Integer

        Try
            ' create file and write header
            SKUAliasFile = InitiateSKUAliasFile(FileOutputPath)
            ' is Source Code empty ?
            If SourceCode = "" Then
                ' get all SKU Alias values
                MyBase.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
            Else
                ' no, get SKU Alias Values for specified sourcee
                MyBase.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", SourceCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
            End If
            For iLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.Count - 1 ' TJS 03/07/13 
                If InStr(Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).SourceCode_DEV000221, ",") > 0 Then ' TJS 29/01/14
                    strOutputLine = """" & Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).SourceCode_DEV000221 & """," ' TJS 29/01/14
                Else
                    strOutputLine = Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).SourceCode_DEV000221 & "," ' TJS 03/07/13 TJS 12/07/13 TJS 29/01/14
                End If
                strOutputLine = strOutputLine & Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).SourceSKU_DEV000221 & "," ' TJS 03/07/13 TJS 12/07/13
                If Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).IsItemNameNull Then ' TJS 03/07/13 TJS 05/07/13
                    strOutputLine = strOutputLine & "," ' TJS 05/07/13 TJS 12/07/13
                Else
                    If InStr(Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).ItemName, ",") > 0 Then ' TJS 12/07/13
                        strOutputLine = strOutputLine & """" & Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).ItemName & """," ' TJS 12/07/13
                    Else
                        strOutputLine = strOutputLine & Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).ItemName & "," ' TJS 03/07/13 TJS 12/07/13
                    End If
                End If
                strOutputLine = strOutputLine & Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).ItemCode_DEV000221 ' TJS 03/07/13 TJS 12/07/13
                SKUAliasFile.WriteLine(strOutputLine)
            Next
            ' close file
            SKUAliasFile.Close()
            Return True

        Catch ex As Exception
            ErrorDetails = ex.Message
            If SKUAliasFile IsNot Nothing Then ' TJS 05/07/13
                SKUAliasFile.Close() ' TJS 05/07/13
            End If
            Return False

        End Try

    End Function
#End Region

#Region " ImportSKUAliasRecords "
    Public Function ImportSKUAliasRecords(ByVal FileImportPath As String, ByVal SourceCode As String, ByVal DeleteExisting As Boolean, _
        ByVal DeleteAllSources As Boolean, ByRef ErrorDetails As String, ByRef RejectedRecordFilePath As String, _
        ByRef RejectedRowCount As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Function added
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221
        ' 05/07/13 | TJS             | 2013.1.27 | Completed code testing
        ' 29/01/14 | TJS             | 2013.4.07 | Modified to populate the ItemName column if an existing row is updated,
        '                                        | to detect errors caused by . in the file name and allow up to 100 characters in Source SKU
        '------------------------------------------------------------------------------------------

        Dim tempFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
        Dim tempDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Dim rowSKUAlias As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSKUAliasView_DEV000221Row ' TJS 03/07/13
        Dim connSKUAlias As OdbcConnection, dataSKUAlias As OdbcDataAdapter, tableSKUAlias As DataSet, rowImport As DataRow
        Dim strRejectedRecord As String, strItemDetails As String(), iSlashPosn As Integer, iLoop As Integer
        Dim iColumnLoop As Integer, bSourceCodeInvalid As Boolean
        Dim RejectedRecordsFile As StreamWriter = Nothing

        Try
            ErrorDetails = ""
            RejectedRowCount = 0
            ' use ODBC Text driver to open csv order file
            tableSKUAlias = New DataSet()
            ' separate file name from path
            iSlashPosn = InStrRev(FileImportPath, "\")
            If iSlashPosn > 0 Then
                ' connection object needs file path
                connSKUAlias = New OdbcConnection("Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" & FileImportPath.Substring(0, iSlashPosn) & ";")
            Else
                ErrorDetails = "Import File path invalid"
                Return False
            End If
            ' data adaptor needs file name
            dataSKUAlias = New OdbcDataAdapter("SELECT * FROM [" & FileImportPath.Substring(iSlashPosn) & "] ORDER BY [Source Code], [Source SKU]", connSKUAlias)

            ' create table from csv data
            Try
                dataSKUAlias.Fill(tableSKUAlias, "SKUAliasRecords")

            Catch ex As Exception
                If InStr(ex.Message, "ERROR [42S02]") > 0 Then ' TJS 29/01/14
                    ErrorDetails = "Unable to read data - please check the file name does not contain" & vbCrLf & "any . characters" ' TJS 29/01/14
                Else
                    ErrorDetails = "Unable to read data - please check the first line of the file" & vbCrLf & "contains the same column names as the template file"
                End If
                Return False

            End Try

            ' create file and write header
            RejectedRecordFilePath = FileImportPath.Substring(0, iSlashPosn) & "Rejected_SKU_Alias_Records.csv"
            RejectedRecordsFile = InitiateSKUAliasFile(RejectedRecordFilePath)

            If DeleteExisting Then
                tempDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
                tempFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(tempDataset, m_ErrorNotification, m_BaseProductCode, m_ErrorNotification.BaseProductName)
                If DeleteAllSources Then
                    tempFacade.LoadDataSet(New String()() {New String() {tempDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                        "ReadLerrynImportExportSKUAliases_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                Else
                    tempFacade.LoadDataSet(New String()() {New String() {tempDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                        "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", SourceCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                End If
            End If
            For Each rowImport In tableSKUAlias.Tables("SKUAliasRecords").Rows
                If rowImport("Source Code").ToString <> "" Then
                    bSourceCodeInvalid = True
                    For iLoop = 0 To m_ImportExportDataset.LerrynImportExportConfig_DEV000221.Count - 1
                        ' does source code match ?
                        If m_ImportExportDataset.LerrynImportExportConfig_DEV000221(iLoop).SourceCode_DEV000221 = rowImport("Source Code").ToString Then
                            ' yes, source is valid
                            bSourceCodeInvalid = False
                            Exit For
                        End If
                    Next
                    If bSourceCodeInvalid Then
                        ErrorDetails = ErrorDetails & "Invalid Source Code " & rowImport("Source Code").ToString & " - record ignored" & vbCrLf
                        strRejectedRecord = """" & rowImport("Source Code").ToString & """, """ & rowImport("Source SKU").ToString & """, """
                        strRejectedRecord = strRejectedRecord & rowImport("CB Item Name").ToString & """, ""Invalid Source Code"""
                        RejectedRecordsFile.WriteLine(strRejectedRecord)
                        RejectedRowCount = RejectedRowCount + 1
                        Continue For
                    End If
                End If
                If rowImport("Source SKU").ToString = "" And (rowImport("CB Item Name").ToString <> "" Or rowImport("CB Item Code").ToString <> "") Then
                    ErrorDetails = ErrorDetails & "Source SKU cannot be blank - record ignored" & vbCrLf
                    strRejectedRecord = """" & rowImport("Source Code").ToString & """, """ & rowImport("Source SKU").ToString & """, """
                    strRejectedRecord = strRejectedRecord & rowImport("CB Item Name").ToString & """, """ & rowImport("CB Item Code").ToString
                    strRejectedRecord = strRejectedRecord & """, ""Source SKU cannot be blank"""
                    RejectedRecordsFile.WriteLine(strRejectedRecord)
                    RejectedRowCount = RejectedRowCount + 1
                    Continue For

                ElseIf rowImport("Source SKU").ToString <> "" And (rowImport("CB Item Name").ToString <> "" Or rowImport("CB Item Code").ToString <> "") Then
                    If rowImport("Source SKU").ToString.Length > 100 Then ' TJS 29/01/14
                        ErrorDetails = ErrorDetails & "Source SKU cannot exceed 100 characters - record ignored" & vbCrLf ' TJS 29/01/14
                        strRejectedRecord = """" & rowImport("Source Code").ToString & """, """ & rowImport("Source SKU").ToString & """, """
                        strRejectedRecord = strRejectedRecord & rowImport("CB Item Name").ToString & """, """ & rowImport("CB Item Code").ToString
                        strRejectedRecord = strRejectedRecord & """, ""Source SKU cannot exceed 100 characters""" ' TJS 29/01/14
                        RejectedRecordsFile.WriteLine(strRejectedRecord)
                        RejectedRowCount = RejectedRowCount + 1
                        Continue For

                    Else
                        If rowImport("CB Item Name").ToString <> "" Then
                            strItemDetails = Me.GetRow(New String() {"ItemCode", "ItemName"}, "InventoryItem", "ItemName = '" & rowImport("CB Item Name").ToString.Replace("'", "''") & "'")
                        Else
                            strItemDetails = Me.GetRow(New String() {"ItemCode", "ItemName"}, "InventoryItem", "ItemCode = '" & rowImport("CB Item Code").ToString.Replace("'", "''") & "'")
                        End If
                        If strItemDetails Is Nothing OrElse String.IsNullOrEmpty(strItemDetails(0)) Then
                            If rowImport("CB Item Name").ToString <> "" Then
                                ErrorDetails = ErrorDetails & "CB Item Name " & rowImport("CB Item Name").ToString & " not found - record ignored" & vbCrLf
                            Else
                                ErrorDetails = ErrorDetails & "CB Item Code " & rowImport("CB Item Code").ToString & " not found - record ignored" & vbCrLf
                            End If
                            strRejectedRecord = """" & rowImport("Source Code").ToString & """, """ & rowImport("Source SKU").ToString & """, """
                            strRejectedRecord = strRejectedRecord & rowImport("CB Item Name").ToString & """, """ & rowImport("CB Item Code").ToString
                            If rowImport("CB Item Name").ToString <> "" Then
                                strRejectedRecord = strRejectedRecord & """, ""CB Item Name " & rowImport("CB Item Name").ToString & " not found"""
                            Else
                                strRejectedRecord = strRejectedRecord & """, ""CB Item Code " & rowImport("CB Item Code").ToString & " not found"""
                            End If
                            RejectedRecordsFile.WriteLine(strRejectedRecord)
                            RejectedRowCount = RejectedRowCount + 1
                            Continue For
                        Else
                            ' record is valid, check for existing record
                            If rowImport("Source Code").ToString <> "" Then
                                MyBase.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", rowImport("Source Code").ToString, "@SourceSKU", rowImport("Source SKU").ToString}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                            Else
                                MyBase.LoadDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", SourceCode, "@SourceSKU", rowImport("Source SKU").ToString}}, _
                                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                            End If
                            If Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.Count = 0 Then ' TJS 03/07/13
                                ' row not found, create new
                                rowSKUAlias = Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.NewLerrynImportExportSKUAliasView_DEV000221Row ' TJS 03/07/13
                                If rowImport("Source Code").ToString <> "" Then
                                    rowSKUAlias.SourceCode_DEV000221 = rowImport("Source Code").ToString
                                Else
                                    rowSKUAlias.SourceCode_DEV000221 = SourceCode
                                End If
                                rowSKUAlias.SourceSKU_DEV000221 = rowImport("Source SKU").ToString
                                rowSKUAlias.ItemCode_DEV000221 = strItemDetails(0)
                                rowSKUAlias.ItemName = strItemDetails(1)
                                Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.AddLerrynImportExportSKUAliasView_DEV000221Row(rowSKUAlias) ' TJS 03/07/13
                            Else
                                If Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 <> strItemDetails(0) Then ' TJS 29/01/14
                                    Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 = strItemDetails(0) ' TJS 03/07/13
                                    ' check if view has the ItemName populated otherwise record may be rejected
                                    If Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).IsItemNameNull OrElse _
                                        Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).ItemName <> strItemDetails(1) Then ' TJS 29/01/14
                                        Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).ItemName = strItemDetails(1) ' TJS 29/01/14
                                    End If
                                End If
                                If DeleteExisting Then
                                    ' find existing row in temp dataset and remove
                                    For iLoop = 0 To tempDataset.LerrynImportExportSKUAliasView_DEV000221.Count - 1 ' TJS 03/07/13
                                        If tempDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).SourceCode_DEV000221 = _
                                            Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).SourceCode_DEV000221 And _
                                            tempDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).SourceSKU_DEV000221 = _
                                            Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).SourceSKU_DEV000221 Then ' TJS 03/07/13
                                            ' delete it and accept- changes as we don't need to delete row later on
                                            tempDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).Delete() ' TJS 03/07/13
                                            tempDataset.LerrynImportExportSKUAliasView_DEV000221.AcceptChanges() ' TJS 03/07/13
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                            If Not Me.UpdateDataSet(New String()() {New String() {Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                                "CreateLerrynImportExportSKUAliases_DEV000221", "UpdateLerrynImportExportSKUAliases_DEV000221", "DeleteLerrynImportExportSKUAliases_DEV000221"}}, _
                                Interprise.Framework.Base.Shared.TransactionType.None, "SKU Alias Import", False) Then ' TJS 03/07/13
                                For iColumnLoop = 0 To Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.Columns.Count - 1 ' TJS 03/07/13
                                    If Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221(0).GetColumnError(iColumnLoop) <> "" Then ' TJS 03/07/13
                                        If ErrorDetails <> "" Then
                                            ErrorDetails = ErrorDetails & ", "
                                        End If
                                        ErrorDetails = ErrorDetails & Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.Columns(iColumnLoop).ColumnName & " - " & _
                                            Me.m_ImportExportDataset.LerrynImportExportSKUAliasView_DEV000221.Rows(0).GetColumnError(iColumnLoop) ' TJS 03/07/13
                                    End If
                                Next
                                strRejectedRecord = """" & rowImport("Source Code").ToString & """, """ & rowImport("Source SKU").ToString & """, """
                                strRejectedRecord = strRejectedRecord & rowImport("CB Item Name").ToString & """, """ & rowImport("CB Item Code").ToString
                                strRejectedRecord = strRejectedRecord & """, """ & ErrorDetails & """"
                                RejectedRecordsFile.WriteLine(strRejectedRecord)
                                RejectedRowCount = RejectedRowCount + 1
                                ErrorDetails = ErrorDetails & " - record ignored" & vbCrLf
                            End If
                        End If
                    End If
                End If
            Next

            If DeleteExisting Then
                ' delete all remaining rows in temp dataset
                For iLoop = 0 To tempDataset.LerrynImportExportSKUAliasView_DEV000221.Count - 1 ' TJS 03/07/13
                    tempDataset.LerrynImportExportSKUAliasView_DEV000221(iLoop).Delete() ' TJS 03/07/13
                Next
                If Not tempFacade.UpdateDataSet(New String()() {New String() {tempDataset.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                   "CreateLerrynImportExportSKUAliases_DEV000221", "UpdateLerrynImportExportSKUAliases_DEV000221", "DeleteLerrynImportExportSKUAliases_DEV000221"}}, _
                   Interprise.Framework.Base.Shared.TransactionType.None, "SKU Alias Import", False) Then ' TJS 03/07/13
                    For iColumnLoop = 0 To tempDataset.LerrynImportExportSKUAliasView_DEV000221.Columns.Count - 1 ' TJS 03/07/13
                        If tempDataset.LerrynImportExportSKUAliasView_DEV000221(0).GetColumnError(iColumnLoop) <> "" Then ' TJS 03/07/13
                            If ErrorDetails <> "" Then
                                ErrorDetails = ErrorDetails & ", "
                            End If
                            ErrorDetails = ErrorDetails & tempDataset.LerrynImportExportSKUAliasView_DEV000221.Columns(iColumnLoop).ColumnName & " - " & _
                               tempDataset.LerrynImportExportSKUAliasView_DEV000221.Rows(0).GetColumnError(iColumnLoop) ' TJS 03/07/13
                        End If
                    Next
                    ErrorDetails = "Failed to delete all existing records " & ErrorDetails
                End If
            End If
            dataSKUAlias.Dispose()
            connSKUAlias.Close()
            RejectedRecordsFile.Close()
            Return True

        Catch ex As Exception
            ErrorDetails = ErrorDetails & ex.Message
            If RejectedRecordsFile IsNot Nothing Then ' TJS 05/07/13
                RejectedRecordsFile.Close() ' TJS 05/07/13
            End If
            Return False

        End Try
    End Function
#End Region
#End Region
End Class
#End Region
