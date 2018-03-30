' eShopCONNECT for Connected Business - Windows Service
' Module: StatusUpdates.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
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
' Last Updated - 04 January 2014

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst ' TJS 13/04/10
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module StatusUpdates

    Private dteLoopStartTime As Date ' TJS 19/09/13

    Public Function UpdateStatus() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/09 | TJS             | 2009.1.05 | Function added
        ' 10/03/09 | TJS             | 2009.1.09 | Modified to add Amazon status updates
        ' 16/03/09 | TJS             | 2009.1.10 | Corrected source matching in send XML loop
        ' 05/07/09 | TJS             | 2009.3.00 | Modified to pass dataset reference to amazon functions
        ' 27/11/09 | TJS             | 2009.3.09 | Modified to always check for update on first poll after service start
        '                                        | and to use global dataset
        ' 30/12/09 | TJS             | 2010.0.00 | Modified to add Channel Advisor status updates
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to check activation in case it expired while service running 
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for Magento and ASPDotNetStoreFront Status updates
        '                                        | plus Source Code constants
        ' 18/03/11 | TJS             | 2011.0.01 | Renamed SiteID as IntanceID to avoid confusion when Magento Instance hosts multiple sites/damains
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for eBay
        ' 16/01/12 | TJS             | 2010.2.02 | Modified to cater for Sears.com 
        ' 14/02/12 | TJS             | 2011.2.05 | Corrected Sears.com connector count
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to initialise bASPStoreFrontStatusUpdatesToSend
        ' 26/03/12 | TJS             | 2011.2.11 | Modified to ensure error handler works if ActiveSource not set
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 05/07/12 | TJS             | 2012.1.08 | Moved Magento and ebay logout to ensure connection is closed properly
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 15/04/13 | TJS             | 2013.1.10 | Removed setting of bMagentoStatusUpdateConnected as replaced by a function
        ' 23/07/13 | TJS             | 2013.1.31 | Modified to log error details if updatedataset fils to save
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to limit loop execution time
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        ' 04/01/14 | TJS             | 2013.4.03 | Modified for Volusion status updates
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
        Dim ActiveSource As SourceSettings, ActiveShopComSettings As ShopComSettings, ActiveAmazonSettings As AmazonSettings ' TJS 10/03/09
        Dim ActiveChannelAdvSettings As ChannelAdvisorSettings, ActiveMagentoSettings As MagentoSettings ' TJS 30/12/09 TJS 19/08/10
        Dim ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ActiveEBaySettings As eBaySettings ' TJS 19/08/10 TJS 02/12/11
        Dim ActiveSearsComSettings As SearsComSettings, Active3DCartSettings As ThreeDCartSettings ' TJS 16/01/12 TJS 20/11/13
        Dim ActiveVolusionSettings As VolusionSettings ' TJS 04/01/13
        Dim strLastSourceCode As String, strLastStoreMerchantID As String, strLastSalesOrderCode As String
        Dim strCurrentSourceCode As String, strCurrentStoreMerchantID As String, strTemp As String
        Dim iLastXMLMessageType As Integer, iCurrentXMLMessageType As Integer, iTemp As Integer ' TJS 30/12/09
        Dim iActionLoop As Integer, iMerchantLoop As Integer, bSourceFound As Boolean, bChangeXMLFile As Boolean ' TJS 30/12/09
        Dim iRowLoop As Integer, iColumnLoop As Integer, bSourceActivated As Boolean ' TJS 13/04/10 TJS 23/07/13

        Try
            ActiveSource = Nothing  ' TJS 26/03/12
            ActiveShopComSettings = Nothing  ' TJS 26/03/12
            ActiveAmazonSettings = Nothing  ' TJS 26/03/12
            ActiveMagentoSettings = Nothing ' TJS 26/03/12
            ActiveEBaySettings = Nothing ' TJS 26/03/12
            ActiveChannelAdvSettings = Nothing ' TJS 26/03/12
            ActiveASPStoreFrontSettings = Nothing ' TJS 26/03/12
            ActiveSearsComSettings = Nothing ' TJS 20/11/13
            Active3DCartSettings = Nothing ' TJS 20/11/13
            ActiveVolusionSettings = Nothing ' TJS 04/01/13
            dteLoopStartTime = Date.Now ' TJS 19/09/13

            ' set last record values to force source initialisation on first record
            strLastSourceCode = ""
            strLastStoreMerchantID = ""
            strLastSalesOrderCode = ""
            iLastXMLMessageType = 0 ' TJS 30/12/09
            bSourceFound = False
            bShopComStatusUpdatesToSend = False ' TJS 11/02/09
            bAmazonStatusUpdatesToSend = False ' TJS 10/03/09
            bChannelAdvStatusUpdatesToSend = False ' TJS 30/12/09
            bEBayStatusUpdateConnected = False ' TJS 02/12/11
            bSearsComStatusUpdatesToSend = False ' TJS 16/01/12
            bASPStoreFrontStatusUpdatesToSend = False ' TJS 24/02/12
            bSourceActivated = False ' TJS 13/04/10

            ImportExportStatusFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(m_ImportExportDataset, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            ' is Status Action Required set in DB ?
            If CBool(ImportExportStatusFacade.GetField("StatusActionRequired_DEV000221", "LerrynImportExportServiceAction_DEV000221")) Or bFirstPoll Then ' TJS 27/11/09
                ' yes, clear flag, then get Action List - this way we ensure that any action records created 
                ' by a trigger between reading flag and clearing it are not missed
                ImportExportStatusFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET StatusActionRequired_DEV000221 = 0", Nothing)
                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                    "ReadLerrynImportExportActionStatus_DEV000221", AT_ACTION_COMPLETE, "0", AT_XML_TO_SEND, "0"}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Count > 0 Then
                    For iActionLoop = 0 To m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Count - 1
                        ' get current Source Code and Store Merchant ID
                        strCurrentSourceCode = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).SourceCode_DEV000221
                        strCurrentStoreMerchantID = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).StoreMerchantID_DEV000221
                        iCurrentXMLMessageType = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221 ' TJS 30/12/09
                        ' is there a Source and Merchant ID ?
                        If strCurrentSourceCode <> "" And strCurrentStoreMerchantID <> "" Then
                            ' yes, has source changed
                            If (strLastSourceCode <> strCurrentSourceCode Or strLastStoreMerchantID <> strCurrentStoreMerchantID) Then
                                ' yes, find source config details
                                bSourceFound = False ' TJS 19/08/10
                                For Each ActiveSource In Setts.ActiveSources
                                    If ActiveSource.SourceCode = strCurrentSourceCode Then
                                        ' now check Store Merchant ID (where relevant)
                                        Select Case ActiveSource.SourceCode
                                            Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) ' TJS 13/04/10
                                                For iMerchantLoop = 0 To ActiveSource.ShopComSettingCount - 1
                                                    If ActiveSource.ShopComSettings(iMerchantLoop).CatalogID = strCurrentStoreMerchantID Then
                                                        ActiveShopComSettings = ActiveSource.ShopComSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If

                                            Case AMAZON_SOURCE_CODE ' TJS 10/03/09 TJS 19/08/10
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) ' TJS 13/04/10
                                                For iMerchantLoop = 0 To ActiveSource.AmazonSettingCount - 1 ' TJS 10/03/09
                                                    If ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken = strCurrentStoreMerchantID Then ' TJS 10/03/09 TJS 22/03/13
                                                        ActiveAmazonSettings = ActiveSource.AmazonSettings(iMerchantLoop) ' TJS 10/03/09
                                                        bSourceFound = True ' TJS 10/03/09
                                                        Exit For ' TJS 10/03/09
                                                    End If
                                                Next
                                                If bSourceFound Then ' TJS 10/03/09
                                                    Exit For ' TJS 10/03/09
                                                End If

                                            Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 30/12/09 TJS 19/08/10
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) ' TJS 13/04/10
                                                For iMerchantLoop = 0 To ActiveSource.ChannelAdvSettingCount - 1 ' TJS 30/12/09
                                                    If ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID = strCurrentStoreMerchantID Then ' TJS 30/12/09
                                                        ActiveChannelAdvSettings = ActiveSource.ChannelAdvSettings(iMerchantLoop) ' TJS 30/12/09
                                                        bSourceFound = True ' TJS 30/12/09
                                                        Exit For ' TJS 30/12/09
                                                    End If
                                                Next
                                                If bSourceFound Then ' TJS 30/12/09
                                                    Exit For ' TJS 30/12/09
                                                End If

                                                ' start of code added TJS 19/08/10
                                            Case MAGENTO_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE)
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
                                                    ' does Instance ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.MagentoSettings(iMerchantLoop).InstanceID = strCurrentStoreMerchantID Then ' TJS 02/12/11 TJS 18/03/11
                                                        ActiveMagentoSettings = ActiveSource.MagentoSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If

                                            Case ASP_STORE_FRONT_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE)
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.ASPStoreFrontSettingCount - 1
                                                    ' does Site ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.ASPStoreFrontSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                        ActiveASPStoreFrontSettings = ActiveSource.ASPStoreFrontSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If
                                                ' end of code added TJS 19/08/10

                                                ' start of code added TJS 02/12/11
                                            Case EBAY_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE)
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.eBaySettingCount - 1
                                                    ' does Site ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.eBaySettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                        ActiveEBaySettings = ActiveSource.eBaySettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If
                                                ' end of code added TJS 02/12/11

                                                ' start of code added TJS 16/01/12
                                            Case SEARS_COM_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE)
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.SearsComSettingCount - 1 ' TJS 14/02/12
                                                    ' does Site ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.SearsComSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                        ActiveSearsComSettings = ActiveSource.SearsComSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If
                                                ' end of code added TJS 16/01/12

                                            Case GENERIC_IMPORT_SOURCE_CODE ' TJS 19/08/10
                                                ' Store Merchant ID not relevant
                                                bSourceActivated = m_ImportExportConfigFacade.IsActivated() ' TJS 13/04/10
                                                bSourceFound = True
                                                Exit For

                                                ' start of code added TJS 20/11/13
                                            Case THREE_D_CART_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE)
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.ThreeDCartSettingCount - 1
                                                    ' does Store ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID = strCurrentStoreMerchantID Then
                                                        Active3DCartSettings = ActiveSource.ThreeDCartSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If
                                                ' end of code added TJS 20/11/13

                                                ' start of code added TJS 04/01/13
                                            Case VOLUSION_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE)
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.VolusionSettingCount - 1
                                                    ' does Site ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.VolusionSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                        ActiveVolusionSettings = ActiveSource.VolusionSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If
                                                ' end of code added TJS 04/01/13
                                        End Select
                                    End If
                                Next
                                ' update last source
                                strLastSourceCode = strCurrentSourceCode
                                strLastStoreMerchantID = strCurrentStoreMerchantID
                                iLastXMLMessageType = iCurrentXMLMessageType ' TJS 30/12/09
                                ' force change of sales order
                                strLastSalesOrderCode = ""
                            End If

                            ' has source been matched ?
                            If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                strTemp = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).SalesOrderCode_DEV000221
                                ' yes, get source type
                                Select Case ActiveSource.SourceCode
                                    Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                        DoShopComOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveShopComSettings, iActionLoop, strTemp)

                                    Case AMAZON_SOURCE_CODE ' TJS 10/03/09 TJS 19/08/10
                                        DoAmazonOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings, iActionLoop, strTemp) ' TJS 10/03/09

                                    Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 30/12/09 TJS 19/08/10
                                        DoChannelAdvOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings, iActionLoop, strTemp) ' TJS 30/12/09

                                    Case MAGENTO_SOURCE_CODE ' TJS 19/08/10
                                        DoMagentoOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveMagentoSettings, iActionLoop, strTemp) ' TJS 19/08/10

                                    Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 19/08/10
                                        DoASPStoreFrontOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStoreFrontSettings, iActionLoop, strTemp) ' TJS 19/08/10

                                    Case EBAY_SOURCE_CODE ' TJS 02/12/11
                                        DoEBayOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveEBaySettings, iActionLoop, strTemp) ' TJS 02/12/11

                                    Case SEARS_COM_SOURCE_CODE ' TJS 16/01/12
                                        DoSearsComOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveSearsComSettings, iActionLoop, strTemp) ' TJS 16/01/12

                                    Case THREE_D_CART_SOURCE_CODE ' TJS 20/11/13
                                        Do3DCartOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, Active3DCartSettings, iActionLoop, strTemp)  ' TJS 20/11/13

                                    Case VOLUSION_SOURCE_CODE ' TJS 04/01/13
                                        DoVolusionOrderStatusUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveVolusionSettings, iActionLoop, strTemp)  ' TJS 04/01/13

                                    Case Else
                                        ' no update required, mark record as complete
                                        m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).ActionComplete_DEV000221 = True

                                End Select

                            End If
                        Else
                            ' either Source and Merchant ID missing, no update required, mark record as complete
                            m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).ActionComplete_DEV000221 = True
                        End If
                        ' exit loop if shutdown in progress or loop has been running for 5 minutes or more
                        If bShutDownInProgress Or DateDiff(DateInterval.Minute, dteLoopStartTime, Date.Now) > 5 Then ' TJS 02/08/13 TJS 19/09/13
                            Exit For ' TJS 02/08/13
                        End If
                    Next
                    If Not bShutDownInProgress Then ' TJS 02/08/13
                        If Not ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                            "CreateLerrynImportExportActionStatus_DEV000221", "UpdateLerrynImportExportActionStatus_DEV000221", "DeleteLerrynImportExportActionStatus_DEV000221"}}, _
                            Interprise.Framework.Base.Shared.TransactionType.None, "Update Lerryn Import/Export Action Status", False) Then ' TJS 23/07/13
                            ' start of code added TJS 23/07/13
                            strTemp = ""
                            For iRowLoop = 0 To m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Rows.Count - 1
                                For iColumnLoop = 0 To m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Columns.Count - 1
                                    If m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                        strTemp = strTemp & m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.TableName & _
                                            "." & m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                            m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                    End If
                                Next
                            Next
                            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "UpdateStatus", "Failed to save Status updates - " & strTemp) ' TJS 13/11/13
                        End If
                        ' end of code added TJS 23/07/13
                    End If
                End If

                ' reset last record values to force source initialisation on first record
                strLastSourceCode = ""
                strLastStoreMerchantID = ""
                strLastSalesOrderCode = ""
                iLastXMLMessageType = 0 ' TJS 30/12/09
                bSourceFound = False

                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.TableName, _
                    "ReadLerrynImportExportActionStatus_DEV000221", AT_ACTION_COMPLETE, "0", AT_XML_TO_SEND, "1"}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Count > 0 Then
                    For iActionLoop = 0 To m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221.Count - 1
                        ' get current Source Code and Store Merchant ID
                        strCurrentSourceCode = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).SourceCode_DEV000221
                        strCurrentStoreMerchantID = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).StoreMerchantID_DEV000221
                        iCurrentXMLMessageType = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221 ' TJS 30/12/09
                        ' has source or Merchant ID changed ?
                        If strLastSourceCode <> strCurrentSourceCode Or strLastStoreMerchantID <> strCurrentStoreMerchantID Or _
                            iLastXMLMessageType <> iCurrentXMLMessageType Then ' TJS 30/12/09
                            ' yes, was last source been matched ?
                            If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                ' yes, get last type
                                Select Case ActiveSource.SourceCode
                                    Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                        ' send completed Status Update file
                                        SendShopComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveShopComSettings)

                                    Case AMAZON_SOURCE_CODE ' TJS 10/03/09 TJS 19/08/10
                                        ' send completed Status Update file
                                        SaveAmazonStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings) ' TJS 10/03/09

                                    Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 30/12/09 TJS 19/08/10
                                        ' send completed Status Update file
                                        SendChannelAdvStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings) ' TJS 30/12/09

                                    Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 19/08/10
                                        ' send completed Status Update file
                                        SendASPStoreFrontStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStoreFrontSettings) ' TJS 19/08/10

                                    Case SEARS_COM_SOURCE_CODE ' TJS 16/01/12
                                        ' send completed Status Update file
                                        SendSearsComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveSearsComSettings) ' TJS 16/01/12

                                    Case Else
                                        ' no action required

                                End Select
                            End If

                            ' find new source config details
                            bSourceFound = False
                            For Each ActiveSource In Setts.ActiveSources
                                If ActiveSource.SourceCode = strCurrentSourceCode Then
                                    ' now check Store Merchant ID (where relevant)
                                    Select Case ActiveSource.SourceCode
                                        Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) ' TJS 13/04/10
                                            For iMerchantLoop = 0 To ActiveSource.ShopComSettingCount - 1
                                                If ActiveSource.ShopComSettings(iMerchantLoop).CatalogID = strCurrentStoreMerchantID Then
                                                    ActiveShopComSettings = ActiveSource.ShopComSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then ' TJS 16/03/09
                                                Exit For ' TJS 16/03/09
                                            End If

                                        Case AMAZON_SOURCE_CODE ' TJS 10/03/09 TJS 19/08/10
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) ' TJS 13/04/10
                                            For iMerchantLoop = 0 To ActiveSource.AmazonSettingCount - 1 ' TJS 10/03/09
                                                If ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken = strCurrentStoreMerchantID Then ' TJS 10/03/09 TJS 22/03/13
                                                    ActiveAmazonSettings = ActiveSource.AmazonSettings(iMerchantLoop) ' TJS 10/03/09
                                                    bSourceFound = True ' TJS 10/03/09
                                                    Exit For ' TJS 10/03/09
                                                End If
                                            Next
                                            If bSourceFound Then ' TJS 16/03/09
                                                Exit For ' TJS 16/03/09
                                            End If

                                        Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 30/12/09 TJS 19/08/10
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) ' TJS 13/04/10
                                            For iMerchantLoop = 0 To ActiveSource.ChannelAdvSettingCount - 1 ' TJS 30/12/09
                                                If ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID = strCurrentStoreMerchantID Then ' TJS 30/12/09
                                                    ActiveChannelAdvSettings = ActiveSource.ChannelAdvSettings(iMerchantLoop) ' TJS 30/12/09
                                                    bSourceFound = True ' TJS 30/12/09
                                                    Exit For ' TJS 30/12/09
                                                End If
                                            Next
                                            If bSourceFound Then ' TJS 30/12/09
                                                Exit For ' TJS 30/12/09
                                            End If

                                            ' start of code added TJS 19/08/10
                                        Case MAGENTO_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE)
                                            For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
                                                If ActiveSource.MagentoSettings(iMerchantLoop).InstanceID = strCurrentStoreMerchantID Then ' TJS 02/12/11 TJS 18/03/11
                                                    ActiveMagentoSettings = ActiveSource.MagentoSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If

                                        Case ASP_STORE_FRONT_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE)
                                            For iMerchantLoop = 0 To ActiveSource.ASPStoreFrontSettingCount - 1
                                                If ActiveSource.ASPStoreFrontSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                    ActiveASPStoreFrontSettings = ActiveSource.ASPStoreFrontSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If
                                            ' end of code added TJS 19/08/10

                                            ' start of code added TJS 16/01/12
                                        Case SEARS_COM_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(SEARS_DOT_COM_CONNECTOR_CODE)
                                            ' find relevant account settings
                                            For iMerchantLoop = 0 To ActiveSource.SearsComSettingCount - 1 ' TJS 14/02/12
                                                ' does Site ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                If ActiveSource.SearsComSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                    ActiveSearsComSettings = ActiveSource.SearsComSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If
                                            ' end of code added TJS 16/01/12

                                        Case GENERIC_IMPORT_SOURCE_CODE ' TJS 19/08/10
                                            ' Store Merchant ID not relevant
                                            bSourceActivated = m_ImportExportConfigFacade.IsActivated() ' TJS 13/04/10
                                            bSourceFound = True
                                            Exit For

                                            ' start of code added TJS 20/11/13
                                        Case THREE_D_CART_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE)
                                            ' find relevant account settings
                                            For iMerchantLoop = 0 To ActiveSource.ThreeDCartSettingCount - 1
                                                ' does Store ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                If ActiveSource.ThreeDCartSettings(iMerchantLoop).StoreID = strCurrentStoreMerchantID Then
                                                    Active3DCartSettings = ActiveSource.ThreeDCartSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If
                                            ' end of code added TJS 20/11/13

                                            ' start of code added TJS 04/01/13
                                        Case VOLUSION_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(VOLUSION_CONNECTOR_CODE)
                                            ' find relevant account settings
                                            For iMerchantLoop = 0 To ActiveSource.VolusionSettingCount - 1
                                                ' does Site ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                If ActiveSource.VolusionSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                    ActiveVolusionSettings = ActiveSource.VolusionSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If
                                            ' end of code added TJS 04/01/13
                                    End Select
                                End If
                            Next
                            ' update last source
                            strLastSourceCode = strCurrentSourceCode
                            strLastStoreMerchantID = strCurrentStoreMerchantID
                            iLastXMLMessageType = iCurrentXMLMessageType ' TJS 30/12/09
                            ' force change of sales order
                            strLastSalesOrderCode = ""

                            ' has new source been matched ?
                            If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                Select Case ActiveSource.SourceCode
                                    Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                        ' start new Status Update file
                                        StartShopComStatusFile(ActiveSource, ActiveShopComSettings)

                                    Case AMAZON_SOURCE_CODE ' TJS 10/03/09 TJS 19/08/10
                                        ' start new Status Update file
                                        StartAmazonStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 10/03/09 TJS 05/07/09

                                    Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 30/12/09 TJS 19/08/10
                                        ' start new Status Update file
                                        StartChannelAdvStatusFile(ActiveSource, ActiveChannelAdvSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 30/12/09

                                    Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 19/08/10
                                        ' start new Status Update file
                                        StartASPStoreFrontStatusFile(ActiveSource, ActiveASPStoreFrontSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 19/08/10

                                    Case SEARS_COM_SOURCE_CODE ' TJS 16/01/12
                                        ' start new Status Update file
                                        StartSearsComStatusFile(ActiveSource, ActiveSearsComSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 16/01/12

                                    Case Else
                                        ' no action required

                                End Select

                            End If
                        Else
                            ' source not changed, has Sales Order or XML Message Type changed (which depends on Source)
                            strTemp = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).SalesOrderCode_DEV000221
                            iTemp = m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221 ' TJS 30/12/09
                            bChangeXMLFile = False
                            If strLastSalesOrderCode <> strTemp And (ActiveSource.SourceCode = SHOP_COM_SOURCE_CODE Or _
                                (ActiveSource.SourceCode = CHANNEL_ADVISOR_SOURCE_CODE And iLastXMLMessageType = 2)) Then ' TJS 30/12/09 TJS 19/08/10
                                bChangeXMLFile = True ' TJS 30/12/09
                            ElseIf iLastXMLMessageType <> iTemp Then ' TJS 30/12/09
                                bChangeXMLFile = True ' TJS 30/12/09
                            End If
                            If bChangeXMLFile Then ' TJS 30/12/09
                                ' yes, has new source been matched ?
                                If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                    Select Case ActiveSource.SourceCode
                                        Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                            ' send completed Status Update file
                                            SendShopComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveShopComSettings)
                                            ' start new Status Update file
                                            StartShopComStatusFile(ActiveSource, ActiveShopComSettings)

                                        Case AMAZON_SOURCE_CODE ' TJS 10/03/09 TJS 19/08/10
                                            ' send completed Status Update file
                                            SaveAmazonStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings) ' TJS 10/03/09
                                            ' start new Status Update file
                                            StartAmazonStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 10/03/09 TJS 05/07/09

                                        Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 30/12/09 TJS 19/08/10
                                            ' send completed Status Update file
                                            SendChannelAdvStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings) ' TJS 30/12/09
                                            ' start new Status Update file
                                            StartChannelAdvStatusFile(ActiveSource, ActiveChannelAdvSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 30/12/09

                                        Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 19/08/10
                                            ' send completed Status Update file
                                            SendASPStoreFrontStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStoreFrontSettings) ' TJS 19/08/10
                                            ' start new Status Update file
                                            StartASPStoreFrontStatusFile(ActiveSource, ActiveASPStoreFrontSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' 19/08/10

                                        Case SEARS_COM_SOURCE_CODE ' TJS 16/01/12
                                            ' send completed Status Update file
                                            SendSearsComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveSearsComSettings) ' TJS 16/01/12
                                            ' start new Status Update file
                                            StartSearsComStatusFile(ActiveSource, ActiveSearsComSettings, m_ImportExportDataset.LerrynImportExportActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 16/01/12

                                        Case Else
                                            ' no action required

                                    End Select
                                End If
                                strLastSalesOrderCode = strTemp
                                iLastXMLMessageType = iTemp ' TJS 30/12/09
                            End If
                        End If
                        ' has source been matched ?
                        If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                            Select Case ActiveSource.SourceCode
                                Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                    ' add new status update record
                                    AddToShopComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveShopComSettings, iActionLoop)

                                Case AMAZON_SOURCE_CODE ' TJS 10/03/09 TJS 19/08/10
                                    ' add new status update record
                                    AddToAmazonStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings, iActionLoop) ' TJS 10/03/09 TJS 05/07/09

                                Case CHANNEL_ADVISOR_SOURCE_CODE ' TJS 30/12/09 TJS 19/08/10
                                    ' add new status update record
                                    AddToChannelAdvStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings, iActionLoop) ' TJS 30/12/09

                                Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 19/08/10
                                    ' add new status update record
                                    AddToASPStoreFrontStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStoreFrontSettings, iActionLoop) ' TJS  19/08/10

                                Case SEARS_COM_SOURCE_CODE ' TJS 16/01/12
                                    ' add new status update record
                                    AddToSearsComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveSearsComSettings, iActionLoop) ' TJS  16/01/12

                                Case Else
                                    ' no action required

                            End Select

                        End If
                        ' exit loop if shutdown in progress or loop has been running for 5 minutes or more
                        If bShutDownInProgress Or DateDiff(DateInterval.Minute, dteLoopStartTime, Date.Now) > 5 Then ' TJS 02/08/13 TJS 19/09/13
                            Exit For ' TJS 02/08/13
                        End If
                    Next
                    If Not bShutDownInProgress Then ' TJS 02/08/13
                        If bShopComStatusUpdatesToSend Then ' TJS 11/02/09
                            ' send completed Status Update file
                            SendShopComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveShopComSettings) ' TJS 11/02/09
                        End If
                        If bAmazonStatusUpdatesToSend Then ' TJS 10/03/09
                            ' send completed Status Update file
                            SaveAmazonStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings) ' TJS 11/02/09' TJS 10/03/09
                        End If
                        If bChannelAdvStatusUpdatesToSend Then ' TJS 30/12/09
                            ' send completed Status Update file
                            SendChannelAdvStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings) ' TJS 30/12/09
                        End If
                        If bASPStoreFrontStatusUpdatesToSend Then ' TJS 19/08/10
                            ' send completed Status Update file
                            SendASPStoreFrontStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStoreFrontSettings)  ' TJS 19/08/10
                        End If
                        If bSearsComStatusUpdatesToSend Then ' TJS 16/01/12
                            ' send completed Status Update file
                            SendSearsComStatusFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveSearsComSettings) ' TJS 16/01/12
                        End If
                    End If
                End If
                ' start of code moved TJS 05/07/12
                If MagentoStatusUpdateConnected() Then ' TJS 19/08/10 TJS 15/04/13
                    ' logout and release Magneto connector
                    CloseMagentoStatusConnection(ActiveSource) ' TJS 19/08/10
                End If
                If bEBayStatusUpdateConnected Then ' TJS 02/12/11
                    CloseEBayStatusConnection(ActiveSource) ' TJS 02/12/11
                End If
                ' end of code moved TJS 05/07/12
            End If
            Return True

        Catch Ex As Exception
            If ActiveSource IsNot Nothing Then ' TJS 26/03/12
                m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "UpdateStatus", Ex)
            Else
                m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "UpdateStatus", Ex) ' TJS 26/03/12
            End If
            Return False
        End Try

    End Function
End Module
