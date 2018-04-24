' eShopCONNECT for Connected Business - Windows Service
' Module: StatusUpdates.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 19 September 2013

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst ' TJS 13/04/10
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module InventoryUpdates

    Private dteLoopStartTime As Date ' TJS 19/09/13

    Public Function UpdateInventory() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/02/09 | TJS             | 2009.1.04 | Function added
        ' 02/06/09 | LJG             | 2009.2.10 | Code added
        ' 28/09/09 | TJS             | 2009.3.07 | modified to cater for SourceCode and StoreMerchantID no longer allowing nulls
        ' 27/11/09 | TJS             | 2009.3.09 | Modified to reset variable before output file build and to use global dataset 
        ' 13/04/10 | TJS             | 2010.0.06 | Modified to check activation in case it expired while service running 
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for Source Code constants
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to add ASPStorefront and Magento Inventory publishing 
        ' 04/05/11 | TJS             | 2011.0.13 | Modified to log UpdateDataset errors
        ' 02/12/11 | TJS             | 2011.2.00 | Added initialisation for bMagentoInventoryUpdateConnected and prepared 
        '                                        | for Channel Advisor and eBay inventory publishing
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to send ASPSroefront Inventory updates and corrected checks for connectors activated
        ' 26/03/12 | TJS             | 2011.2.11 | Modified to ensure error handler works if ActiveSource not setr
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 05/07/12 | TJS             | 2012.1.08 | Moved Magento logout to ensure connection is closed properly
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 15/04/13 | TJS             | 2013.1.10 | Removed setting of bMagentoInventoryUpdateConnected as replaced by a function
        ' 03/07/13 | TJS/FA          | 2013.1.24 | Added check to see if inventory import is occurring.  Postpone updates until import wizard has finished
        '                                          otherwise may cause concurrency violation (2 process trying to update same record)
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to limit loop execution time
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
        Dim ActiveSource As SourceSettings, ActiveShopComSettings As ShopComSettings, ActiveAmazonSettings As AmazonSettings
        Dim ActiveMagentoSettings As MagentoSettings, ActiveASPStorefrontSettings As ASPStoreFrontSettings  ' TJS 18/03/11
        Dim ActiveChannelAdvSettings As ChannelAdvisorSettings, ActiveEBaySettings As eBaySettings ' TJS 02/12/11
        Dim strLastSourceCode As String, strLastStoreMerchantID As String, strLastInventoryItemCode As String
        Dim strCurrentSourceCode As String, strCurrentStoreMerchantID As String, strTemp As String
        Dim strDateTime() As String, strTime() As String, strDate() As String, dteLastUpdate As Date 'TJS/FA 03/07/13
        Dim iLastXMLMessageType As Integer, iCurrentXMLMessageType As Integer, iActionLoop As Integer ' TJS 02/12/11
        Dim iMerchantLoop As Integer, iTemp As Integer, bSourceFound As Boolean, bSourceActivated As Boolean ' TJS 13/04/10 TJS 02/12/11
        Dim bChangeXMLFile As Boolean ' TJS 02/12/11

        Try

            ActiveSource = Nothing  ' TJS 18/03/11
            ActiveShopComSettings = Nothing  ' TJS 18/03/11
            ActiveAmazonSettings = Nothing  ' TJS 18/03/11
            ActiveMagentoSettings = Nothing ' TJS 18/03/11
            ActiveEBaySettings = Nothing ' TJS 02/12/11
            ActiveChannelAdvSettings = Nothing ' TJS 02/12/11
            ActiveASPStorefrontSettings = Nothing ' TJS 24/02/12
            dteLoopStartTime = Date.Now ' TJS 19/09/13

            ' set last record values to force source initialisation on first record
            strLastSourceCode = ""
            strLastStoreMerchantID = ""
            strLastInventoryItemCode = ""
            bSourceFound = False
            bAmazonInventoryUpdatesToSend = False
            bASPStoreFrontInventoryUpdatesToSend = False ' TJS 24/02/12
            bChannelAdvInventoryUpdatesToSend = False ' TJS 02/12/11
            bEBayInventoryUpdateConnected = False ' TJS 02/12/11
            bShopComInventoryUpdatesToSend = False
            bSourceActivated = False ' TJS 13/04/10

            ImportExportStatusFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(m_ImportExportDataset, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

            ' start of code added TJS/FA 03/07/13
            ' get import wizard timestamp
            strTemp = ImportExportStatusFacade.GetField("LastImportWizardUpdate_DEV000221", "LerrynImportExportServiceAction_DEV000221")
            ' is there a value ?
            If Not String.IsNullOrEmpty(strTemp) Then
                ' yes, convert date format
                strDateTime = strTemp.Split(CChar(" "))
                strDate = strDateTime(0).Split(CChar("/"))
                strTime = strDateTime(1).Split(CChar(":"))
                dteLastUpdate = DateSerial(CInt(strDate(2)), CInt(strDate(0)), CInt(strDate(1))).AddHours(CDbl(strTime(0))).AddMinutes(CDbl(strTime(1))).AddSeconds(CDbl(strTime(2)))
                ' is timestamp in last five minutes ?
                If dteLastUpdate > Date.Now.AddMinutes(-5) Then
                    ' yes, inventory import running - ignore inventory updates for now
                    Return True
                End If
            End If
            ' end of code added TJS/FA 03/07/13

            ' is Inventory Action Required set in DB ?
            If CBool(ImportExportStatusFacade.GetField("InventoryActionRequired_DEV000221", "LerrynImportExportServiceAction_DEV000221")) Or bFirstPoll Then ' TJS 27/11/09
                ' yes, clear flag, then get Action List - this way we ensure that any action records created 
                ' by a trigger between reading flag and clearing it are not missed
                ImportExportStatusFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 0", Nothing)
                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                    "ReadLerrynImportExportInventoryActionStatus_DEV000221", AT_ACTION_COMPLETE, "0", AT_XML_TO_SEND, "0"}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count > 0 Then
                    For iActionLoop = 0 To m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                        ' get current Source Code and Store Merchant ID
                        strCurrentSourceCode = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).SourceCode_DEV000221
                        strCurrentStoreMerchantID = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).StoreMerchantID_DEV000221
                        If Not m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).IsXMLMessageType_DEV000221Null Then ' TJS 02/12/11
                            iCurrentXMLMessageType = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221 ' TJS 02/12/11
                        Else
                            iCurrentXMLMessageType = 0 ' TJS 02/12/11
                        End If

                        ' is there a Source and Merchant ID ?
                        If strCurrentSourceCode <> "" And strCurrentStoreMerchantID <> "" Then
                            ' yes, has source changed
                            If (strLastSourceCode <> strCurrentSourceCode Or strLastStoreMerchantID <> strCurrentStoreMerchantID) Then
                                ' yes, find source config details
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

                                            Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) ' TJS 13/04/10
                                                For iMerchantLoop = 0 To ActiveSource.AmazonSettingCount - 1
                                                    If ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken = strCurrentStoreMerchantID Then ' TJS 22/03/13
                                                        ActiveAmazonSettings = ActiveSource.AmazonSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If

                                                ' start of code added TJS 18/03/11
                                            Case MAGENTO_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE)
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
                                                    ' does Instance ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.MagentoSettings(iMerchantLoop).InstanceID = strCurrentStoreMerchantID Then
                                                        ActiveMagentoSettings = ActiveSource.MagentoSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If

                                            Case ASP_STORE_FRONT_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) ' TJS 24/02/12
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.ASPStoreFrontSettingCount - 1
                                                    ' does Store ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.ASPStoreFrontSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                        ActiveASPStorefrontSettings = ActiveSource.ASPStoreFrontSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If
                                                ' end of code added TJS 18/03/11

                                                ' start of code added TJS 02/12/11
                                            Case EBAY_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) ' TJS 24/02/12
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

                                            Case CHANNEL_ADVISOR_SOURCE_CODE
                                                bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) ' TJS 24/02/12
                                                ' find relevant account settings
                                                For iMerchantLoop = 0 To ActiveSource.ChannelAdvSettingCount - 1
                                                    ' does Account ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                    If ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID = strCurrentStoreMerchantID Then
                                                        ActiveChannelAdvSettings = ActiveSource.ChannelAdvSettings(iMerchantLoop)
                                                        bSourceFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If bSourceFound Then
                                                    Exit For
                                                End If
                                                ' end of code added TJS 02/12/11

                                        End Select
                                    End If
                                Next
                                ' update last source
                                strLastSourceCode = strCurrentSourceCode
                                strLastStoreMerchantID = strCurrentStoreMerchantID
                                ' force change of item code
                                strLastInventoryItemCode = ""
                            End If

                            ' has source been matched ?
                            If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                strTemp = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).ItemCode_DEV000221
                                ' yes, get source type
                                Select Case ActiveSource.SourceCode
                                    Case SHOP_COM_SOURCE_CODE ' TJS 19/08/10
                                        DoShopComInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveShopComSettings, iActionLoop, strTemp)

                                    Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                                        DoAmazonInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings, iActionLoop, strTemp)

                                        ' start of code added TJS 18/03/11
                                    Case MAGENTO_SOURCE_CODE
                                        DoMagentoInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveMagentoSettings, iActionLoop, strTemp)

                                    Case ASP_STORE_FRONT_SOURCE_CODE
                                        DoASPStorefrontInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStorefrontSettings, iActionLoop, strTemp)
                                        ' end of code added TJS 18/03/11

                                        ' start of code added TJS 02/12/11
                                    Case EBAY_SOURCE_CODE
                                        DoEBayInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveEBaySettings, iActionLoop, strTemp)

                                    Case CHANNEL_ADVISOR_SOURCE_CODE
                                        DoChannelAdvInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings, iActionLoop, strTemp)
                                        ' end of code added TJS 02/12/11

                                    Case Else
                                        ' no update required, mark record as complete
                                        m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).ActionComplete_DEV000221 = True

                                End Select

                            End If
                        Else
                            ' either Source and Merchant ID missing, no update required, mark record as complete
                            m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).ActionComplete_DEV000221 = True
                        End If
                        ' exit loop if shutdown in progress or loop has been running for 5 minutes or more
                        If bShutDownInProgress Or DateDiff(DateInterval.Minute, dteLoopStartTime, Date.Now) > 5 Then ' TJS 02/08/13 TJS 19/09/13
                            Exit For ' TJS 02/08/13
                        End If
                    Next
                    ' Shop.com want product file with all items for any change so sent it now
                    If bShopComInventoryUpdatesToSend Then
                        ' send completed Inventory Update file
                        SendShopComInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveShopComSettings)
                    End If
                    Dim iRowLoop As Integer, iColumnLoop As Integer, strErrorDetails As String ' TJS 04/05/11
                    If Not bShutDownInProgress Then ' TJS 02/08/13
                        If Not ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                             "CreateLerrynImportExportInventoryActionStatus_DEV000221", "UpdateLerrynImportExportInventoryActionStatus_DEV000221", "DeleteLerrynImportExportInventoryActionStatus_DEV000221"}}, _
                             Interprise.Framework.Base.Shared.TransactionType.None, "Update Lerryn Import/Export Action Status", False) Then ' TJS 04/05/11
                            strErrorDetails = "" ' TJS 04/05/11
                            For iRowLoop = 0 To m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Rows.Count - 1 ' TJS 04/05/11
                                If m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iRowLoop).HasErrors Then ' TJS 04/05/11
                                    For iColumnLoop = 0 To m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Columns.Count - 1 ' TJS 04/05/11
                                        If m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then ' TJS 04/05/11
                                            strErrorDetails = strErrorDetails & m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName & _
                                                "." & m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf ' TJS 04/05/11
                                        End If
                                    Next
                                End If
                            Next
                            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "UpdateInventory", strErrorDetails, "") ' TJS 04/05/11
                        End If
                    End If
                End If

                ' reset last record values to force source initialisation on first record
                strLastSourceCode = "" ' TJS 27/11/09
                strLastStoreMerchantID = "" ' TJS 27/11/09
                strLastInventoryItemCode = "" ' TJS 27/11/09
                iLastXMLMessageType = 0 ' TJS 02/12/11
                bSourceFound = False ' TJS 27/11/09

                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                    "ReadLerrynImportExportInventoryActionStatus_DEV000221", AT_ACTION_COMPLETE, "0", AT_XML_TO_SEND, "1"}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific)
                If m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count > 0 Then
                    For iActionLoop = 0 To m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                        ' get current Source Code and Store Merchant ID
                        strCurrentSourceCode = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).SourceCode_DEV000221
                        strCurrentStoreMerchantID = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).StoreMerchantID_DEV000221
                        iCurrentXMLMessageType = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221 ' TJS 02/12/11
                        ' has source or Merchant ID changed ?
                        If strLastSourceCode <> strCurrentSourceCode Or strLastStoreMerchantID <> strCurrentStoreMerchantID Or _
                            iLastXMLMessageType <> iCurrentXMLMessageType Then ' TJS 02/12/11
                            ' yes, was last source been matched ?
                            If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                ' yes, get last type
                                Select Case ActiveSource.SourceCode
                                    Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                                        ' send completed Inventory Update file
                                        SaveAmazonInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings)

                                        ' start of code added TJS 02/12/11
                                    Case EBAY_SOURCE_CODE
                                        SendEBayInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveEBaySettings)

                                    Case CHANNEL_ADVISOR_SOURCE_CODE
                                        SendChannelAdvInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings)
                                        ' end of code added TJS 02/12/11

                                    Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 24/02/12
                                        SendASPStoreFrontInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStorefrontSettings) ' TJS 24/02/12

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
                                            If bSourceFound Then
                                                Exit For
                                            End If

                                        Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) ' TJS 13/04/10
                                            For iMerchantLoop = 0 To ActiveSource.AmazonSettingCount - 1
                                                If ActiveSource.AmazonSettings(iMerchantLoop).MerchantToken = strCurrentStoreMerchantID Then ' TJS 22/03/13
                                                    ActiveAmazonSettings = ActiveSource.AmazonSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If

                                            ' start of code added TJS 18/03/11
                                        Case MAGENTO_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE)
                                            For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
                                                If ActiveSource.MagentoSettings(iMerchantLoop).InstanceID = strCurrentStoreMerchantID Then
                                                    ActiveMagentoSettings = ActiveSource.MagentoSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If

                                        Case ASP_STORE_FRONT_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) ' TJS 24/02/12
                                            ' find relevant account settings
                                            For iMerchantLoop = 0 To ActiveSource.ASPStoreFrontSettingCount - 1
                                                ' does Store ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                If ActiveSource.ASPStoreFrontSettings(iMerchantLoop).SiteID = strCurrentStoreMerchantID Then
                                                    ActiveASPStorefrontSettings = ActiveSource.ASPStoreFrontSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If
                                            ' end of code added TJS 18/03/11

                                            ' start of code added TJS 02/12/11
                                        Case EBAY_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) ' TJS 24/02/12
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

                                        Case CHANNEL_ADVISOR_SOURCE_CODE
                                            bSourceActivated = m_ImportExportConfigFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) ' TJS 24/02/12
                                            ' find relevant account settings
                                            For iMerchantLoop = 0 To ActiveSource.ChannelAdvSettingCount - 1
                                                ' does Site ID match (ignore account disabled flag as order must have been imported before flag was set) ?
                                                If ActiveSource.ChannelAdvSettings(iMerchantLoop).AccountID = strCurrentStoreMerchantID Then
                                                    ActiveChannelAdvSettings = ActiveSource.ChannelAdvSettings(iMerchantLoop)
                                                    bSourceFound = True
                                                    Exit For
                                                End If
                                            Next
                                            If bSourceFound Then
                                                Exit For
                                            End If
                                            ' end of code added TJS 02/12/11

                                    End Select
                                End If
                            Next
                            ' update last source
                            strLastSourceCode = strCurrentSourceCode
                            strLastStoreMerchantID = strCurrentStoreMerchantID
                            iLastXMLMessageType = iCurrentXMLMessageType ' TJS 02/12/11
                            ' force change of item code
                            strLastInventoryItemCode = ""

                            ' has new source been matched ?
                            If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                Select Case ActiveSource.SourceCode
                                    Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                                        ' start new Inventory Update file
                                        StartAmazonInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings, m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221)

                                        ' start of code added TJS 02/12/11
                                    Case EBAY_SOURCE_CODE
                                        StartEBayInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveEBaySettings, m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221)

                                    Case CHANNEL_ADVISOR_SOURCE_CODE
                                        StartChannelAdvInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings, m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221)
                                        ' end of code added TJS 02/12/11

                                    Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 24/02/12
                                        StartASPStoreFrontInventoryFile(ActiveSource, ActiveASPStorefrontSettings, m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 24/02/12

                                    Case Else
                                        ' no action required

                                End Select

                            End If
                        Else
                            ' source not changed, has Item Code or XML Message Type changed (which depends on Source)
                            strTemp = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).ItemCode_DEV000221
                            iTemp = m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221 ' TJS 02/12/11
                            bChangeXMLFile = False ' TJS 02/12/11
                            If strLastInventoryItemCode <> strTemp Then
                                ' start of code added TJS 02/12/11
                                bChangeXMLFile = True
                            ElseIf iLastXMLMessageType <> iTemp Then
                                bChangeXMLFile = True
                            Else
                                Select Case ActiveSource.SourceCode
                                    Case CHANNEL_ADVISOR_SOURCE_CODE
                                        bChangeXMLFile = CheckChannelAdvInventoryFileLimit(iLastXMLMessageType)

                                    Case Else
                                        ' no specific file limit to check
                                End Select
                                ' end of code added TJS 02/12/11
                            End If
                            If bChangeXMLFile Then ' TJS 02/12/11
                                ' yes, has new source been matched ?
                                If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                                    ' yes, do any sources need items in separate files ?
                                    Select Case ActiveSource.SourceCode
                                        ' start of code added TJS 18/03/11
                                        Case MAGENTO_SOURCE_CODE

                                        Case ASP_STORE_FRONT_SOURCE_CODE
                                            ' end of code added TJS 18/03/11

                                            ' start of code added TJS 02/12/11
                                        Case CHANNEL_ADVISOR_SOURCE_CODE
                                            ' send completed Inventory Update file
                                            SendChannelAdvInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings)
                                            ' start new Inventory Update file
                                            StartChannelAdvInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings, m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221)
                                            ' end of code added TJS 02/12/11

                                        Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 24/02/12
                                            ' send completed Inventory Update file
                                            SendASPStoreFrontInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStorefrontSettings) ' TJS 24/02/12
                                            ' start new Inventory Update file
                                            StartASPStoreFrontInventoryFile(ActiveSource, ActiveASPStorefrontSettings, m_ImportExportDataset.LerrynImportExportInventoryActionStatus_DEV000221(iActionLoop).XMLMessageType_DEV000221) ' TJS 24/02/12

                                        Case Else
                                            ' no action required

                                    End Select
                                End If
                                strLastInventoryItemCode = strTemp
                                iLastXMLMessageType = iTemp ' TJS 02/12/11
                            End If
                        End If
                        ' has source been matched ?
                        If bSourceFound And bSourceActivated Then ' TJS 13/04/10
                            Select Case ActiveSource.SourceCode
                                Case AMAZON_SOURCE_CODE ' TJS 19/08/10
                                    ' add new inventory update record
                                    AddToAmazonInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings, iActionLoop)

                                    ' start of code added TJS 02/12/11
                                Case EBAY_SOURCE_CODE
                                    AddToEBayInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveEBaySettings, iActionLoop)

                                Case CHANNEL_ADVISOR_SOURCE_CODE
                                    AddToChannelAdvInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings, iActionLoop)
                                    ' end of code added TJS 02/12/11

                                Case ASP_STORE_FRONT_SOURCE_CODE ' TJS 24/02/12
                                    AddToASPStoreFrontInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStorefrontSettings, iActionLoop) ' TJS 24/02/12

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
                        If bAmazonInventoryUpdatesToSend Then ' TJS 27/11/09
                            ' send completed Inventory Update file
                            SaveAmazonInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveAmazonSettings)
                        End If
                        If bChannelAdvInventoryUpdatesToSend Then ' TJS 02/12/11
                            ' send completed Inventory Update file
                            SendChannelAdvInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveChannelAdvSettings) ' TJS 02/12/11
                        End If
                        If bEBayInventoryUpdateConnected Then ' TJS 02/12/11
                            ' send completed Inventory Update file
                            SendEBayInventoryUpdate(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveEBaySettings) ' TJS 02/12/11
                            CloseEBayInventoryConnection(ActiveSource) ' TJS 02/12/11
                        End If
                        If bASPStoreFrontInventoryUpdatesToSend Then ' TJS 24/02/12
                            ' send completed Inventory Update file
                            SendASPStoreFrontInventoryFile(ImportExportStatusFacade, m_ImportExportDataset, ActiveSource, ActiveASPStorefrontSettings) ' TJS 24/02/12
                        End If
                    End If
                End If
                ' start of code moved TJS 05/07/12
                If MagentoInventoryUpdateConnected() Then ' TJS 18/03/11 TJS 15/04/13
                    ' logout and release Magneto connector
                    CloseMagentoInventoryConnection(ActiveSource) ' TJS 18/03/11
                End If
                ' end of code moved TJS 05/07/12
            End If
            Return True

        Catch ex As Exception
            If ActiveSource IsNot Nothing Then ' TJS 26/03/12
                m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "UpdateInventory", ex)
            Else
                m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "UpdateInventory", ex) ' TJS 26/03/12
            End If
            Return False

        End Try

    End Function
End Module
