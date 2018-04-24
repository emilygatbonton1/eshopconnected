' eShopCONNECT for Connected Business - Windows Service
' Module: ChannelAdvInventoryUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 13 November 2013

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Xml.Linq
Imports System.Xml.XPath

Module ChannelAdvInventoryUpdate

    Private XMLNSManCAItems As System.Xml.XmlNamespaceManager
    Private XMLNameTabCAItems As System.Xml.NameTable
    Private XMLInventoryUpdateFile As XDocument
    Private XMLInventoryUpdateNode As XElement
    Public bChannelAdvInventoryUpdatesToSend As Boolean
    Private iChannelAdvXMLMessageType As Integer
    Private iChannelAdvXMLMessageCount As Integer

    Public Sub DoChannelAdvInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings, ByRef RowID As Integer, ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to detect and ignore new records imported via Import Wizard
        ' 10/06/12 | FA              | 2012.1.05 | added missing parameter AT_ACCOUNT_ID, changed ref from Magento to CA
        ' 15/03/13 | TJS             | 2013.1.03 | Corrected calculation of quantity to publish when sending Percent (divide by 100 missing)
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowChannelAdvTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryChannelAdvTagDetails_DEV000221Row
        Dim strXMLQtyUpdate As String, strPriceInfo As String, strSQL As String, strTemp As String
        Dim decItemQtyAvailable As Decimal, decItemTotalQtyWhenLastPublished As Decimal, decItemQtyLastPublished As Decimal
        Dim iOrigRowID As Integer, iCheckLoop As Integer, iQtyToPublish As Integer ' TJS 10/03/12
        Dim bInhibitWebPosts As Boolean, bQtyUpdateRequired As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")
            ' yes, check for inventory updates
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
               "ReadInventoryItem", AT_ITEM_CODE, ItemCode}, New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, _
               "ReadInventoryItemDescription", AT_ITEM_CODE, ItemCode, AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
               New String() {ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221.TableName, _
               "ReadInventoryChannelAdvDetails_DEV000221", AT_ITEM_CODE, ItemCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.InventoryItem.Count > 0 And ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221.Count > 0 Then
                Select Case ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221
                    Case "100", "200" ' 100 is New Item, 200 is Updated Item
                        ' check not imported from wizard
                        If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).FromImportWizard_DEV000221 OrElse _
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221 = "200" Then ' TJS 10/03/12
                            ' yes, must be a newly published item
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 11 ' 11 is Add/Update Product Details using SynchInventoryItemList
                            If Not bInhibitWebPosts Then
                                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.TableName, _
                                    "ReadInventoryChannelAdvTagDetails_DEV000221", AT_ITEM_CODE, ItemCode, AT_ACCOUNT_ID, ActiveChannelAdvSettings.AccountID}, _
                                    New String() {ImportExportStatusDataset.InventoryAmazonASIN_DEV000221.TableName, _
                   "                ReadInventoryAmazonASIN_DEV000221", AT_ITEM_CODE, ItemCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 10/06/12

                                strXMLQtyUpdate = "<web:InventoryItemSubmit><web:Sku>" & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName)
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:Sku><web:Title>"
                                strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).ProductName_DEV000221)
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:Title><web:Subtitle>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsProductSubTitle_DEV000221Null Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).ProductSubTitle_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:Subtitle><web:ShortDescription>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsProductShortDescription_DEV000221Null Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).ProductShortDescription_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:ShortDescription><web:Description>"
                                If ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).ProductDescription_DEV000221 <> INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItemDescription(0).ItemDescription)
                                Else
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).ProductDescription_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:Description><web:FlagStyle>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsFlagStyle_DEV000221Null Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).FlagStyle_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:FlagStyle><web:FlagDescription>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsFlagDescription_DEV000221Null Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).FlagDescription_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:FlagDescription><web:IsBlocked>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsBlocked_DEV000221 Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & "true"
                                Else
                                    strXMLQtyUpdate = strXMLQtyUpdate & "false"
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:IsBlocked><web:BlockComment>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsIsBlockedComment_DEV000221Null Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsBlockedComment_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:BlockComment>"
                                If ImportExportStatusDataset.InventoryAmazonASIN_DEV000221.Count > 0 Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & "<web:ASIN>" & ImportExportStatusDataset.InventoryAmazonASIN_DEV000221(0).ASIN_DEV000221 & "</web:ASIN>"
                                Else
                                    rowChannelAdvTagDetails = ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.FindByItemCode_DEV000221AccountID_DEV000221TagName_DEV000221TagLocation_DEV000221(ItemCode, ActiveChannelAdvSettings.AccountID, "ASIN", 1)
                                    If rowChannelAdvTagDetails IsNot Nothing Then
                                        strXMLQtyUpdate = strXMLQtyUpdate & "<web:ASIN>" & rowChannelAdvTagDetails.TagMemoField_DEV000221 & "</web:ASIN>"
                                    End If
                                End If
                                rowChannelAdvTagDetails = ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.FindByItemCode_DEV000221AccountID_DEV000221TagName_DEV000221TagLocation_DEV000221(ItemCode, ActiveChannelAdvSettings.AccountID, "ISBN", 1)
                                If rowChannelAdvTagDetails IsNot Nothing Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & "<web:ISBN>" & rowChannelAdvTagDetails.TagMemoField_DEV000221 & "</web:ISBN>"
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "<web:Condition>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsCondition_DEV000221Null Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).Condition_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:Condition><web:Warranty>"
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsWarranty_DEV000221Null Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).Warranty_DEV000221)
                                End If
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:Warranty>"
                                ' is this a new Item ?
                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221 = "100" Then
                                    ' yes, send quantity into
                                    strXMLQtyUpdate = strXMLQtyUpdate & "<web:QuantityInfo><web:UpdateType>Available</web:UpdateType<web:Total>"
                                    ' what is publishing basis ?
                                    If ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                                        iQtyToPublish = 0
                                    Else
                                        Select Case ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyPublishingType_DEV000221
                                            Case "Fixed"
                                                ' always send update as orders may have changed value on source site
                                                iQtyToPublish = CInt(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                                                If iQtyToPublish < 0 Then
                                                    iQtyToPublish = 0
                                                End If

                                            Case "Percent"
                                                ' calculate value to be published based on percentage of available qty
                                                iQtyToPublish = CInt(decItemQtyAvailable * ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                                                If iQtyToPublish < 0 Then
                                                    iQtyToPublish = 0
                                                End If

                                            Case Else
                                                iQtyToPublish = 0
                                        End Select
                                    End If
                                    strXMLQtyUpdate = strXMLQtyUpdate & iQtyToPublish & "</web:Total></web:QuantityInfo>"
                                End If
                                strPriceInfo = ""
                                rowChannelAdvTagDetails = ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.FindByItemCode_DEV000221AccountID_DEV000221TagName_DEV000221TagLocation_DEV000221(ItemCode, ActiveChannelAdvSettings.AccountID, "RetailPrice", 2)
                                If rowChannelAdvTagDetails IsNot Nothing Then
                                    strPriceInfo = strPriceInfo & "<web:RetailPrice>" & rowChannelAdvTagDetails.TagMemoField_DEV000221 & "</web:RetailPrice>"
                                End If
                                rowChannelAdvTagDetails = ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.FindByItemCode_DEV000221AccountID_DEV000221TagName_DEV000221TagLocation_DEV000221(ItemCode, ActiveChannelAdvSettings.AccountID, "StartingPrice", 2)
                                If rowChannelAdvTagDetails IsNot Nothing Then
                                    strPriceInfo = strPriceInfo & "<web:StartingPrice>" & rowChannelAdvTagDetails.TagMemoField_DEV000221 & "</web:StartingPrice>"
                                End If
                                rowChannelAdvTagDetails = ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.FindByItemCode_DEV000221AccountID_DEV000221TagName_DEV000221TagLocation_DEV000221(ItemCode, ActiveChannelAdvSettings.AccountID, "ReservePrice", 2)
                                If rowChannelAdvTagDetails IsNot Nothing Then
                                    strPriceInfo = strPriceInfo & "<web:ReservePrice>" & rowChannelAdvTagDetails.TagMemoField_DEV000221 & "</web:ReservePrice>"
                                End If
                                If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).UseISPricingDetail_DEV000221 Then
                                    ' get retail selling price
                                    strSQL = "SELECT RetailPrice FROM dbo.InventoryItemPricingDetailView WHERE CurrencyCode = '" & ImportExportStatusFacade.GetField("CurrencyCode", "SystemCompanyInformation", Nothing)
                                    strSQL = strSQL & "' AND ItemCode = '" & ItemCode & "'"
                                    strTemp = ImportExportStatusFacade.GetField(strSQL, CommandType.Text, Nothing)
                                    ' did we find a price ?
                                    If strTemp <> "" Then
                                        ' yes, use it
                                        strPriceInfo = strPriceInfo & "<web:TakeItPrice>" & strTemp & "</web:TakeItPrice>"
                                    Else
                                        strPriceInfo = strPriceInfo & "<web:TakeItPrice>0.00</web:TakeItPrice>"
                                    End If
                                Else
                                    If ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsSellingPrice_DEV000221Null Then ' TJS 10/06/12
                                        strPriceInfo = strPriceInfo & "<web:TakeItPrice>0.00</web:TakeItPrice>"
                                    Else
                                        strPriceInfo = strPriceInfo & "<web:TakeItPrice>" & Format(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).SellingPrice_DEV000221, "0.00") ' TJS 10/06/12
                                        strPriceInfo = strPriceInfo & "</web:TakeItPrice>"
                                    End If
                                End If
                                rowChannelAdvTagDetails = ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.FindByItemCode_DEV000221AccountID_DEV000221TagName_DEV000221TagLocation_DEV000221(ItemCode, ActiveChannelAdvSettings.AccountID, "SecondChanceOfferPrice", 2)
                                If rowChannelAdvTagDetails IsNot Nothing Then
                                    strPriceInfo = strPriceInfo & "<web:SecondChanceOfferPrice>" & rowChannelAdvTagDetails.TagMemoField_DEV000221 & "</web:SecondChanceOfferPrice>"
                                End If
                                rowChannelAdvTagDetails = ImportExportStatusDataset.InventoryChannelAdvTagDetails_DEV000221.FindByItemCode_DEV000221AccountID_DEV000221TagName_DEV000221TagLocation_DEV000221(ItemCode, ActiveChannelAdvSettings.AccountID, "StorePrice", 2)
                                If rowChannelAdvTagDetails IsNot Nothing Then
                                    strPriceInfo = strPriceInfo & "<web:StorePrice>" & rowChannelAdvTagDetails.TagMemoField_DEV000221 & "</web:StorePrice>"
                                End If
                                If strPriceInfo <> "" Then
                                    strXMLQtyUpdate = strXMLQtyUpdate & "<web:PriceInfo>" & strPriceInfo & "</web:PriceInfo>"
                                End If
                                For iLoop = 0 To ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221.Count - 1

                                Next
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:InventoryItemSubmit>"
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = strXMLQtyUpdate
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True

                                ' check for any new tag records or updated item/tag records and mark them as complete
                                For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveChannelAdvSettings.AccountID Then
                                        If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Or _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "205" Then
                                            ' new or updated Tag value
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts
                                        End If
                                    Else
                                        ' set row pointer return value
                                        RowID = iCheckLoop - 1
                                        Exit For
                                    End If
                                Next
                            Else
                                m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending updates to Channel Advisor")
                            End If
                            ' start of code added TJS 10/03/12
                        Else
                            ' ASPStorefront GUID is set, must have been imported via Import Wizard - mark row as complete
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 10/03/12

                            ' check for any new tag records and mark them as complete
                            iOrigRowID = RowID
                            For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveChannelAdvSettings.AccountID Then
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Then
                                        ' new  Tag value
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True

                                    End If
                                Else
                                    ' set row pointer return value
                                    RowID = iCheckLoop - 1
                                    Exit For
                                End If
                            Next
                            ' did check loop exit without finding a new row for a different item ?
                            If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then
                                ' yes, set row pointer return value
                                RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                            End If
                        End If
                        ' end of code added TJS 10/03/12

                    Case "105" ' New Channel Advisor Tag value

                    Case "205" ' Updated Channel Advisor Tag value

                    Case "300" ' Deleted Item

                    Case "1000" ' Stock Quantity changed
                        bQtyUpdateRequired = False
                        ' get current total quantity available
                        decItemQtyAvailable = CDec(ImportExportStatusFacade.GetField("SELECT ISNULL(SUM(UnitsAvailable), 0) AS TotalAvailable FROM InventoryStockTotal WHERE ItemCode = '" & ItemCode & "'", System.Data.CommandType.Text, Nothing))
                        ' and values when last published
                        If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsTotalQtyWhenLastPublished_DEV000221Null Then
                            decItemTotalQtyWhenLastPublished = ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).TotalQtyWhenLastPublished_DEV000221
                        Else
                            decItemTotalQtyWhenLastPublished = 0
                        End If
                        If Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null Then
                            decItemQtyLastPublished = ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyLastPublished_DEV000221
                        Else
                            decItemQtyLastPublished = 0
                        End If

                        ' what is publishing basis ?
                        If ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                            iQtyToPublish = 0
                        Else
                            Select Case ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyPublishingType_DEV000221
                                Case "Fixed"
                                    ' always send update as orders may have changed value on source site
                                    iQtyToPublish = CInt(ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                                    If iQtyToPublish < 0 Then
                                        iQtyToPublish = 0
                                    End If
                                    bQtyUpdateRequired = True

                                Case "Percent"
                                    ' calculate value to be published based on percentage of available qty
                                    iQtyToPublish = CInt((decItemQtyAvailable * ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyPublishingValue_DEV000221) / 100) ' TJS 15/03/13
                                    If iQtyToPublish < 0 Then
                                        iQtyToPublish = 0
                                    End If
                                    ' is value to publish positive and last value was null or 0 ?
                                    If (ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null OrElse _
                                        ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyLastPublished_DEV000221 = 0) And iQtyToPublish > 0 Then
                                        ' yes, send value
                                        bQtyUpdateRequired = True

                                    ElseIf (Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                        ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyLastPublished_DEV000221 > 0) And iQtyToPublish = 0 Then
                                        ' no, quantity now 0 and was previously positive, send value
                                        bQtyUpdateRequired = True

                                    ElseIf (Not ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                        ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221(0).QtyLastPublished_DEV000221 < 10) Or iQtyToPublish < 10 Then
                                        ' either new or old value less than 10, send value to make sure we don't show 0 stock due to orders on source 
                                        bQtyUpdateRequired = True

                                    ElseIf decItemQtyAvailable > (decItemTotalQtyWhenLastPublished * 1.1) Or decItemQtyAvailable < (decItemTotalQtyWhenLastPublished * 0.9) Then
                                        ' total available stock has changed by more then 10% since last update send, send value
                                        bQtyUpdateRequired = True

                                    End If

                                Case Else
                                    iQtyToPublish = 0
                            End Select
                        End If
                        If bQtyUpdateRequired Then
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 20 ' 20 is Update Product Stock Qty using UpdateInventoryItemQuantityAndPriceList
                            If Not bInhibitWebPosts Then
                                strXMLQtyUpdate = "<web:InventoryItemQuantityAndPrice><web:Sku>" & ImportExportStatusDataset.InventoryItem(0).ItemName & "</web:Sku>"
                                strXMLQtyUpdate = strXMLQtyUpdate & "<web:QuantityInfo><web:UpdateType>Available</web:UpdateType<web:Total>" & iQtyToPublish & "</web:Total>"
                                strXMLQtyUpdate = strXMLQtyUpdate & "</web:QuantityInfo</web:InventoryItemQuantityAndPrice>"
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = strXMLQtyUpdate
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True
                            Else
                                m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending updates to Channel Advisor")
                            End If
                        Else
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
                        End If
                        ' check for any additional Stock Quantity changed records and mark them as complete
                        For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                            If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveChannelAdvSettings.AccountID Then
                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "1000" Then
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts
                                End If
                            Else
                                ' set row pointer return value
                                RowID = iCheckLoop - 1
                                Exit For
                            End If
                        Next

                End Select

            ElseIf ImportExportStatusDataset.InventoryItem.Count > 0 Then
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoChannelAdvInventoryUpdate", "No Inventory Item found for " & ItemCode)
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoChannelAdvInventoryUpdate", "No Inventory Channel Advisor Details found for " & ItemCode) ' TJS 10/06/12
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoChannelAdvInventoryUpdate", ex)

        End Try

    End Sub

    Public Sub StartChannelAdvInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings, ByVal XMLMessageType As Integer)
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

        Dim strMessageBody As String

        Select Case XMLMessageType
            Case 11 ' 11 is Add/Update Product Details using SynchInventoryItemList
                strMessageBody = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://api.channeladvisor.com/webservices/"">"
                If "" & ActiveChannelAdvSettings.OwnDeveloperKey <> "" Then
                    strMessageBody = strMessageBody & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & ActiveChannelAdvSettings.OwnDeveloperKey
                    strMessageBody = strMessageBody & "</web:DeveloperKey><web:Password>" & ActiveChannelAdvSettings.OwnDeveloperPwd
                Else
                    strMessageBody = strMessageBody & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY
                    strMessageBody = strMessageBody & "</web:DeveloperKey><web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                End If
                strMessageBody = strMessageBody & "</web:Password></web:APICredentials></soapenv:Header><soapenv:Body><web:SynchInventoryItemList>"
                strMessageBody = strMessageBody & "<web:accountID>" & ActiveChannelAdvSettings.AccountID & "</web:accountID><web:itemList>"
                strMessageBody = strMessageBody & "</web:itemList</web:SynchInventoryItemList></soapenv:Body></soapenv:Envelope>"

            Case 20 ' 20 is Update Product Stock Qty using UpdateInventoryItemQuantityAndPriceList
                strMessageBody = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://api.channeladvisor.com/webservices/"">"
                If "" & ActiveChannelAdvSettings.OwnDeveloperKey <> "" Then
                    strMessageBody = strMessageBody & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & ActiveChannelAdvSettings.OwnDeveloperKey
                    strMessageBody = strMessageBody & "</web:DeveloperKey><web:Password>" & ActiveChannelAdvSettings.OwnDeveloperPwd
                Else
                    strMessageBody = strMessageBody & "<soapenv:Header><web:APICredentials><web:DeveloperKey>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_KEY
                    strMessageBody = strMessageBody & "</web:DeveloperKey><web:Password>" & CHANNEL_ADVISOR_LERRYN_DEVELOPER_PASSWORD
                End If
                strMessageBody = strMessageBody & "</web:Password></web:APICredentials></soapenv:Header><soapenv:Body><web:UpdateInventoryItemQuantityAndPriceList>"
                strMessageBody = strMessageBody & "<web:accountID>" & ActiveChannelAdvSettings.AccountID & "</web:accountID><web:itemQuantityAndPriceList>"
                strMessageBody = strMessageBody & "</web:itemQuantityAndPriceList></web:UpdateInventoryItemQuantityAndPriceList></soapenv:Body></soapenv:Envelope>"

            Case Else
                Throw New Exception("Unknown Channel Advisor XML Message Type " & XMLMessageType)
        End Select

        XMLInventoryUpdateFile = XDocument.Parse(strMessageBody)
        iChannelAdvXMLMessageType = XMLMessageType
        iChannelAdvXMLMessageCount = 0
        XMLNameTabCAItems = New System.Xml.NameTable
        XMLNSManCAItems = New System.Xml.XmlNamespaceManager(XMLNameTabCAItems)
        XMLNSManCAItems.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/")

    End Sub

    Public Sub AddToChannelAdvInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings, ByRef RowID As Integer)
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

        Dim XMLInsertNode As XElement

        If Not ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null AndAlso _
            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 <> "" Then
            If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = iChannelAdvXMLMessageType Then
                Select Case iChannelAdvXMLMessageType
                    Case 11 ' 11 is Add/Update Product Details using SynchInventoryItemList
                        XMLInsertNode = XMLInventoryUpdateFile.XPathSelectElement("soapenv:Envelope/soapenv:Body/web:SynchInventoryItemList/web:itemList", XMLNSManCAItems)

                    Case 20 ' 20 is Update Product Stock Qty using UpdateInventoryItemQuantityAndPriceList
                        XMLInsertNode = XMLInventoryUpdateFile.XPathSelectElement("soapenv:Envelope/soapenv:Body/web:UpdateInventoryItemQuantityAndPriceList/web:itemQuantityAndPriceList", XMLNSManCAItems)
                End Select

            Else
                Throw New Exception("Cannot add Channel Advisor XML Message Type " & ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 & " to file type " & iChannelAdvXMLMessageType)
            End If
            XMLInsertNode.Add(XElement.Parse(ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221))
            bChannelAdvInventoryUpdatesToSend = True
            iChannelAdvXMLMessageCount += 1
        End If

        ' mark action record as XML Sent (change is committed to DB when inventory file successfully sent in SendChannelAdvInventoryUpdate)
        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

    End Sub

    Public Function CheckChannelAdvInventoryFileLimit(ByVal XMLMessageType As Integer) As Boolean
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

        Dim bReturnValue As Boolean

        bReturnValue = False
        Select Case XMLMessageType
            Case 11 ' 11 is Add/Update Product Details using SynchInventoryItemList
                If iChannelAdvXMLMessageCount >= 25 Then
                    bReturnValue = True
                End If

            Case 20 ' 20 is Update Product Stock Qty using UpdateInventoryItemQuantityAndPriceList
                If iChannelAdvXMLMessageCount >= 100 Then
                    bReturnValue = True
                End If
        End Select

        Return bReturnValue

    End Function

    Public Sub SendChannelAdvInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal ActiveChannelAdvSettings As ChannelAdvisorSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 10/06/12 | FA              | 2012.1.05 | 'Not inhibitWebPost' rather than 'InhibitWebPost'
        '                                           CHANNEL_ADV_STATUS_UPDATE_HEADER not Amazon_Status... 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument
        Dim strReturn As String, bInhibitWebPosts As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")

            ' yes, check submission Merchant ID is as expected
            If ImportExportStatusFacade.GetXMLElementText(XMLInventoryUpdateFile, CHANNEL_ADV_STATUS_UPDATE_HEADER & "/MerchantIdentifier", XMLNSManCAItems) = ActiveChannelAdvSettings.AccountID Then ' TJS/FA 10/06/12
                Select Case iChannelAdvXMLMessageType
                    Case 11 ' 11 is Add/Update Product Details using SynchInventoryItemList
                        If Not bInhibitWebPosts Then ' TJS/FA 10/06/12
                            WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_INVENTORY_SERVICE_URL)
                            WebSubmit.Method = "POST"
                            WebSubmit.ContentType = "text/xml; charset=utf-8"
                            WebSubmit.ContentLength = XMLInventoryUpdateFile.ToString.Length
                            WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/SynchInventoryItemList")
                            WebSubmit.Timeout = 60000

                            byteData = UTF8Encoding.UTF8.GetBytes(XMLInventoryUpdateFile.ToString)

                            ' send to LerrynSecure.com (or the URL defined in the Registry)
                            postStream = WebSubmit.GetRequestStream()
                            postStream.Write(byteData, 0, byteData.Length)

                            WebResponse = WebSubmit.GetResponse
                            reader = New StreamReader(WebResponse.GetResponseStream())
                            strReturn = reader.ReadToEnd()

                            If strReturn <> "" Then
                                Try
                                    ' had difficulty getting XPath to read XML with this name space present so remove it
                                    XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))

                                Catch ex As Exception
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvInventoryUpdate", "Unable to process response from Channel Advisor due to XML error - " & ex.Message.Replace(vbCrLf, ""), strReturn)
                                    Return

                                End Try

                                If ImportExportStatusFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/SynchInventoryItemListResponse/SynchInventoryItemListResult/Status", XMLNSManCAItems) = "Success" Then
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor Inventory Update file sent for Account ID " & ActiveChannelAdvSettings.AccountID & " - " & ActiveChannelAdvSettings.AccountName)
                                    ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                                        "CreateLerrynImportExportInventoryActionStatus_DEV000221", "UpdateLerrynImportExportInventoryActionStatus_DEV000221", _
                                        "DeleteLerrynImportExportInventoryActionStatus_DEV000221"}, _
                                        New String() {ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221.TableName, _
                                        "CreateInventoryChannelAdvDetails_DEV000221", "UpdateInventoryChannelAdvDetails_DEV000221", "DeleteInventoryChannelAdvDetails_DEV000221"}}, _
                                        Interprise.Framework.Base.Shared.TransactionType.None, "Update Channel Advisor Inventory Action Status", False)
                                    bChannelAdvInventoryUpdatesToSend = False

                                Else
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvInventoryUpdate", "Response from Channel Advisor was not a Success message", strReturn)
                                End If

                            Else
                                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvInventoryUpdate", "No Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL, XMLInventoryUpdateFile.ToString)
                            End If

                        Else
                            m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending updates to Channel Advisor")
                        End If

                    Case 20 ' 20 is Update Product Stock Qty using UpdateInventoryItemQuantityAndPriceList
                        If Not bInhibitWebPosts Then ' TJS/FA 10/06/12
                            WebSubmit = System.Net.WebRequest.Create(CHANNEL_ADVISOR_INVENTORY_SERVICE_URL)
                            WebSubmit.Method = "POST"
                            WebSubmit.ContentType = "text/xml; charset=utf-8"
                            WebSubmit.ContentLength = XMLInventoryUpdateFile.ToString.Length
                            WebSubmit.Headers.Add("SOAPAction", "http://api.channeladvisor.com/webservices/UpdateInventoryItemQuantityAndPrice")
                            WebSubmit.Timeout = 30000

                            byteData = UTF8Encoding.UTF8.GetBytes(XMLInventoryUpdateFile.ToString)

                            ' send to LerrynSecure.com (or the URL defined in the Registry)
                            postStream = WebSubmit.GetRequestStream()
                            postStream.Write(byteData, 0, byteData.Length)

                            WebResponse = WebSubmit.GetResponse
                            reader = New StreamReader(WebResponse.GetResponseStream())
                            strReturn = reader.ReadToEnd()

                            If strReturn <> "" Then
                                Try
                                    ' had difficulty getting XPath to read XML with this name space present so remove it
                                    XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://api.channeladvisor.com/webservices/""", ""))

                                Catch ex As Exception
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvInventoryUpdate", "Unable to process response from Channel Advisor due to XML error - " & ex.Message.Replace(vbCrLf, ""), strReturn)
                                    Return

                                End Try

                                If ImportExportStatusFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/UpdateInventoryItemQuantityAndPriceResponse/UpdateInventoryItemQuantityAndPriceResult/Status", XMLNSManCAItems) = "Success" Then
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Channel Advisor Inventory Quantity Update file sent for Account ID " & ActiveChannelAdvSettings.AccountID & " - " & ActiveChannelAdvSettings.AccountName)
                                    ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                                        "CreateLerrynImportExportInventoryActionStatus_DEV000221", "UpdateLerrynImportExportInventoryActionStatus_DEV000221", _
                                        "DeleteLerrynImportExportInventoryActionStatus_DEV000221"}, _
                                        New String() {ImportExportStatusDataset.InventoryChannelAdvDetails_DEV000221.TableName, _
                                        "CreateInventoryChannelAdvDetails_DEV000221", "UpdateInventoryChannelAdvDetails_DEV000221", "DeleteInventoryChannelAdvDetails_DEV000221"}}, _
                                        Interprise.Framework.Base.Shared.TransactionType.None, "Update Channel Advisor Inventory Action Status", False)
                                    bChannelAdvInventoryUpdatesToSend = False

                                Else
                                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvInventoryUpdate", "Response from Channel Advisor was not a Success message", strReturn)
                                End If

                            Else
                                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvInventoryUpdate", "No Response from " & CHANNEL_ADVISOR_INVENTORY_SERVICE_URL, XMLInventoryUpdateFile.ToString)
                            End If

                        Else
                            m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending updates to Channel Advisor")
                        End If

                    Case Else
                        Throw New Exception("Unknown Channel Advisor XML Message Type " & iChannelAdvXMLMessageType)

                End Select

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendChannelAdvInventoryUpdate", "Unexpected Account ID " & ImportExportStatusFacade.GetXMLElementText(XMLInventoryUpdateFile, AMAZON_STATUS_UPDATE_HEADER & "/MerchantIdentifier") & " in submission file, expected " & ActiveChannelAdvSettings.AccountID, XMLInventoryUpdateFile.ToString)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendChannelAdvInventoryUpdate", ex)

        End Try

    End Sub

End Module
