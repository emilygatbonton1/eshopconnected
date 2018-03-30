' eShopCONNECT for Connected Business - Windows Service
' Module: ASPStorefrontInventoryUpdate.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
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
' Updated 13 November 2013

Imports System.IO
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Module ASPStorefrontInventoryUpdate

    Private XMLInventoryUpdateFile As XDocument ' TJS 24/02/12
    Private XMLInventoryUpdateNode As XElement
    Public bASPStoreFrontInventoryUpdatesToSend As Boolean ' TJS 24/02/12
    Private iASPStoreFrontXMLMessageType As Integer

    Public Sub DoASPStorefrontInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ByRef RowID As Integer, ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 24/02/12 | TJS             | 2011.2.08 | Added code for inventory publishing
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to cater for Inventory Quantity update and to 
        '                                        | detect and ignore new records imported via Import Wizard
        ' 15/03/13 | TJS             | 2013.1.03 | Corrected calculation of quantity to publish when sending Percent (divide by 100 missing)
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name and orrected string constants used 
        '                                        | to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strAttributeCodes As String()(), strMatrixItems As String()(), strCategories As String()() ' TJS 24/02/12
        Dim strXMLUpdate As String, strXMLDisplay As String, strXMLSE As String, strXMLEntities As String ' TJS 24/02/12
        Dim strXMLImages As String, strXMLInventory As String, strSKU As String, strTemp As String ' TJS 24/02/12
        Dim strXMLRelatedProducts As String, strXMLQtyUpdate As String ' TJS 24/02/12 TJS 10/03/12
        Dim iOrigRowID As Integer, iCheckLoop As Integer, bBeenDeleted As Boolean ' TJS 24/02/12
        Dim decItemQtyAvailable As Decimal, decItemTotalQtyWhenLastPublished As Decimal, decItemQtyLastPublished As Decimal ' TJS 10/03/12
        Dim iQtyToPublish As Integer, bQtyUpdateRequired As Boolean, bInhibitWebPosts As Boolean ' TJS 10/03/12

        Try
            iOrigRowID = RowID ' TJS 24/02/12
            ' yes, check for inventory updates
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
               "ReadInventoryItem", AT_ITEM_CODE, ItemCode}, New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, _
               "ReadInventoryItemDescription", AT_ITEM_CODE, ItemCode, AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
               New String() {ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221.TableName, _
               "ReadInventoryASPStorefrontDetails_DEV000221", AT_ITEM_CODE, ItemCode}, New String() {ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221.TableName, _
               "ReadInventoryASPStorefrontTagDetails_DEV000221", AT_ITEM_CODE, ItemCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.InventoryItem.Count > 0 And ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221.Count > 0 Then
                ' ignore any records already marked as complete since Matrix Items will be processed as part of new Matrix Groups and may not be in sequence
                If Not ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 Then ' TJS 24/02/12
                    Select Case ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221
                        Case "100" ' New Item
                            ' check ASPStorefront GUID is blank and not imported from wizard
                            If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).FromImportWizard_DEV000221 And _
                                (ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsASPStorefrontProductGUID_DEV000221Null OrElse _
                                ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ASPStorefrontProductGUID_DEV000221 = "") Then ' TJS 10/03/12
                                ' yes, must be a newly published item
                                ' start of code added TJS 24/02/12
                                strXMLUpdate = ""
                                strXMLDisplay = ""
                                strXMLSE = ""
                                strXMLImages = ""
                                strXMLEntities = ""
                                strXMLRelatedProducts = ""
                                If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_STOCK Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_NON_STOCK Or _
                                    ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then

                                    ' build XML Product header with all details 
                                    strXMLUpdate = BuildASPStorefrontCoreElements(ImportExportStatusDataset, ActiveASPStoreFrontSettings, "Add", True)

                                    For iloop = 0 To ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221.Count - 1
                                        AddASPStorefrontTagDetail(ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221(iloop), strXMLUpdate, _
                                            strXMLDisplay, strXMLSE, strXMLImages, strXMLEntities, strXMLRelatedProducts)
                                    Next

                                    strXMLUpdate = strXMLUpdate & BuildASPStorefrontExtensions(ImportExportStatusDataset, ActiveASPStoreFrontSettings)

                                    If strXMLSE <> "" Then
                                        strXMLUpdate = strXMLUpdate & "<SE>" & vbCrLf & strXMLSE & "</SE>" & vbCrLf
                                    End If
                                    If strXMLDisplay <> "" Then
                                        strXMLUpdate = strXMLUpdate & "<Display>" & vbCrLf & strXMLDisplay & "</Display>" & vbCrLf
                                    End If
                                    If strXMLImages <> "" Then
                                        strXMLUpdate = strXMLUpdate & "<Images>" & vbCrLf & strXMLImages & "</Images>" & vbCrLf
                                    End If
                                    If strXMLRelatedProducts <> "" Then
                                        strXMLUpdate = strXMLUpdate & "<RelatedProducts>" & vbCrLf & strXMLRelatedProducts & "</RelatedProducts>" & vbCrLf
                                    End If

                                    strCategories = ImportExportStatusFacade.GetRows(New String() {"CategoryGUID_DEV000221", "CategoryID_DEV000221"}, "InventoryASPStorefrontCategories_DEV000221", "ItemCode_DEV000221 = '" & _
                                        ItemCode & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "' AND IsActive_DEV000221 = 1")
                                    For Each Category As String() In strCategories
                                        strXMLEntities = strXMLEntities & "<Entity EntityType=""Category"" ID=""" & Category(1) & """ GUID=""" & Category(0) & """/>" & vbCrLf
                                    Next
                                    If Not ImportExportStatusDataset.InventoryItem(0).IsManufacturerCodeNull AndAlso ImportExportStatusDataset.InventoryItem(0).ManufacturerCode <> "" Then
                                        strTemp = ImportExportStatusFacade.GetField("SourceManufacturerCode_DEV000221", "SystemManufacturerSourceID_DEV000221", "ManufacturerCode_DEV000221 = '" & _
                                            ImportExportStatusDataset.InventoryItem(0).ManufacturerCode & "' AND SourceCode_DEV000221 = '" & ASP_STORE_FRONT_SOURCE_CODE & _
                                            "' AND AccountOrInstanceID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                        If "" & strTemp <> "" Then
                                            strXMLEntities = strXMLEntities & "<Entity EntityType=""Manufacturer"" ID=""" & Left(strTemp, InStr(strTemp, ":") - 1) & """ GUID=""" & Mid(strTemp, InStr(strTemp, ":") + 1) & """/>" & vbCrLf
                                        End If
                                    End If
                                    If strXMLEntities <> "" Then
                                        strXMLUpdate = strXMLUpdate & " <Mappings>" & vbCrLf & strXMLEntities & "</Mappings>" & vbCrLf
                                    End If

                                    If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                                        ' need to create size/color variants from Matrix Items
                                        strAttributeCodes = ImportExportStatusFacade.GetRows(New String() {"AttributeCode", "PositionID"}, "InventoryAttribute", "ItemCode = '" & ItemCode & "' ORDER BY PositionID")
                                        strXMLUpdate = strXMLUpdate & "<Variants><Variant Action=""Add"""
                                        If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsProductName_DEV000221Null AndAlso _
                                           ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221 <> INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
                                            strXMLUpdate = strXMLUpdate & " Name=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221) & """>" & vbCrLf
                                            strXMLUpdate = strXMLUpdate & "<Name>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221) & "</Name>" & vbCrLf
                                        Else
                                            strXMLUpdate = strXMLUpdate & " Name=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName) & """>" & vbCrLf
                                            strXMLUpdate = strXMLUpdate & "<Name>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName) & "</Name>" & vbCrLf
                                        End If
                                        strXMLUpdate = strXMLUpdate & "<IsDefault>True</IsDefault>"
                                        If strAttributeCodes.Length = 2 Then
                                            strMatrixItems = ImportExportStatusFacade.GetRows(New String() {"MatrixItemCode", "Attribute1", "Attribute2", "AttributeCode1", "AttributeCode2"}, "InventoryMatrixItem", "ItemCode = '" & ItemCode & "' AND Selected = 1 ORDER BY Attribute1, Attribute2")
                                            If strMatrixItems(0)(3) = "Size" Then
                                                strXMLUpdate = strXMLUpdate & BuildASPStorefrontSizeOptions(ItemCode, strMatrixItems(0)(3))
                                                strXMLUpdate = strXMLUpdate & BuildASPStorefrontColorOptions(ItemCode, strMatrixItems(0)(4))
                                            Else
                                                strXMLUpdate = strXMLUpdate & BuildASPStorefrontSizeOptions(ItemCode, strMatrixItems(0)(4))
                                                strXMLUpdate = strXMLUpdate & BuildASPStorefrontColorOptions(ItemCode, strMatrixItems(0)(3))
                                            End If
                                            strXMLInventory = "" & vbCrLf
                                            For Each MatrixItem As String() In strMatrixItems
                                                strTemp = ImportExportStatusFacade.GetField("Publish_DEV000221", "InventoryASPStorefrontDetails_DEV000221", "ItemCode_DEV000221 = '" & _
                                                    MatrixItem(0) & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                                If strTemp = "1" Or strTemp.ToLower = "true" Then
                                                    If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
                                                        strSKU = MatrixItem(0)
                                                    Else
                                                        strSKU = ImportExportStatusFacade.GetField("ItemName", "inventoryItem", "ItemCode = '" & MatrixItem(0) & "'")
                                                    End If
                                                    If MatrixItem(3) = "Size" Then
                                                        strXMLInventory = strXMLInventory & "<Inv Color=""" & MatrixItem(2) & """ Size=""" & MatrixItem(1) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                                    Else
                                                        strXMLInventory = strXMLInventory & "<Inv Color=""" & MatrixItem(1) & """ Size=""" & MatrixItem(2) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                                    End If
                                                End If
                                                MarkMatrixItemAsComplete(ImportExportStatusDataset, ActiveSource, ActiveASPStoreFrontSettings, MatrixItem(0))
                                            Next
                                            If strXMLInventory <> "" Then
                                                strXMLUpdate = strXMLUpdate & "<InventoryBySizeAndColor>" & strXMLInventory & "</InventoryBySizeAndColor>" & vbCrLf
                                            End If

                                        ElseIf strAttributeCodes.Length = 1 And strAttributeCodes(0)(3) = "Size" Then
                                            strMatrixItems = ImportExportStatusFacade.GetRows(New String() {"MatrixItemCode", "Attribute1", "AttributeCode1"}, "InventoryMatrixItem", "ItemCode = '" & ItemCode & "' AND Selected = 1 ORDER BY Attribute1")
                                            strXMLUpdate = strXMLUpdate & BuildASPStorefrontSizeOptions(ItemCode, strMatrixItems(0)(2))
                                            strXMLInventory = ""
                                            For Each MatrixItem As String() In strMatrixItems
                                                strTemp = ImportExportStatusFacade.GetField("Publish_DEV000221", "InventoryASPStorefrontDetails_DEV000221", "ItemCode_DEV000221 = '" & _
                                                    MatrixItem(0) & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                                If strTemp = "1" Or strTemp.ToLower = "true" Then
                                                    If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
                                                        strSKU = MatrixItem(0)
                                                    Else
                                                        strSKU = ImportExportStatusFacade.GetField("ItemName", "inventoryItem", "ItemCode = '" & MatrixItem(0) & "'")
                                                    End If
                                                    strXMLInventory = strXMLInventory & "<Inv Size=""" & MatrixItem(1) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                                End If
                                                MarkMatrixItemAsComplete(ImportExportStatusDataset, ActiveSource, ActiveASPStoreFrontSettings, MatrixItem(0))
                                            Next
                                            If strXMLInventory <> "" Then
                                                strXMLUpdate = strXMLUpdate & "<InventoryBySizeAndColor>" & strXMLInventory & "</InventoryBySizeAndColor>" & vbCrLf
                                            End If

                                        ElseIf strAttributeCodes.Length = 1 Then
                                            strMatrixItems = ImportExportStatusFacade.GetRows(New String() {"MatrixItemCode", "Attribute1", "AttributeCode1"}, "InventoryMatrixItem", "ItemCode = '" & ItemCode & "' AND Selected = 1 ORDER BY Attribute1")
                                            strXMLUpdate = strXMLUpdate & BuildASPStorefrontColorOptions(ItemCode, strMatrixItems(0)(2))
                                            strXMLInventory = ""
                                            For Each MatrixItem As String() In strMatrixItems
                                                strTemp = ImportExportStatusFacade.GetField("Publish_DEV000221", "InventoryASPStorefrontDetails_DEV000221", "ItemCode_DEV000221 = '" & _
                                                    MatrixItem(0) & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                                If strTemp = "1" Or strTemp.ToLower = "true" Then
                                                    If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
                                                        strSKU = MatrixItem(0)
                                                    Else
                                                        strSKU = ImportExportStatusFacade.GetField("ItemName", "inventoryItem", "ItemCode = '" & MatrixItem(0) & "'")
                                                    End If
                                                    strXMLInventory = strXMLInventory & "<Inv Color=""" & MatrixItem(1) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                                End If
                                                MarkMatrixItemAsComplete(ImportExportStatusDataset, ActiveSource, ActiveASPStoreFrontSettings, MatrixItem(0))
                                            Next
                                            If strXMLInventory <> "" Then
                                                strXMLUpdate = strXMLUpdate & "<InventoryBySizeAndColor>" & strXMLInventory & "</InventoryBySizeAndColor>" & vbCrLf
                                            End If

                                        End If
                                        strXMLUpdate = strXMLUpdate & "</Variant>" & vbCrLf & "</Variants>" & vbCrLf
                                    End If
                                    strXMLUpdate = strXMLUpdate & "</Product>" & vbCrLf

                                Else
                                    ' need add processing for Kits 

                                End If

                                ' check for any new tag records or updated item/tag records and mark them as complete
                                ' also check if a deleted item row exists
                                bBeenDeleted = False
                                iOrigRowID = RowID
                                For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveASPStoreFrontSettings.SiteID Then
                                        If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Or _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "205" Then
                                            ' new or updated Tag value
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True

                                        ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "200" Then
                                            ' duplicated update item row - mark as complete
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True

                                        ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "300" Then
                                            ' deleted item row, inhibit update to Magento
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True
                                            bBeenDeleted = True
                                        End If
                                    Else
                                        ' set row pointer return value
                                        RowID = iCheckLoop - 1
                                        Exit For
                                    End If
                                    If bShutDownInProgress Then ' TJS 02/08/13
                                        Exit For ' TJS 02/08/13
                                    End If
                                Next
                                ' did check loop exit without finding a new row for a different item ?
                                If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then
                                    ' yes, set row pointer return value
                                    RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                End If

                                ' was a deleted item row found ?
                                If Not bBeenDeleted And strXMLUpdate <> "" Then
                                    ' no, set message type and mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLMessageType_DEV000221 = 10 ' 10 is CreateProduct
                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionXMLFile_DEV000221 = strXMLUpdate
                                    ' set message type and mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLToSend_DEV000221 = True

                                Else
                                    ' yes, mark row as complete
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True
                                End If
                                ' end of code added TJS 24/02/12
                                ' start of code added TJS 10/03/12
                            Else
                                ' ASPStorefront GUID is set, must have been imported via Import Wizard - mark row as complete
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 10/03/12

                                ' check for any new tag records and mark them as complete
                                iOrigRowID = RowID
                                For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveASPStoreFrontSettings.SiteID Then
                                        If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Then
                                            ' new  Tag value
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True

                                        End If
                                    Else
                                        ' set row pointer return value
                                        RowID = iCheckLoop - 1
                                        Exit For
                                    End If
                                    If bShutDownInProgress Then ' TJS 02/08/13
                                        Exit For ' TJS 02/08/13
                                    End If
                                Next
                                ' did check loop exit without finding a new row for a different item ?
                                If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then
                                    ' yes, set row pointer return value
                                    RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                End If
                            End If
                            ' end of code added TJS 10/03/12

                        Case "105", "205" ' New/Updated ASPStorefront Tag value
                            ' start of code added TJS 24/02/12
                            strXMLUpdate = ""
                            strXMLDisplay = ""
                            strXMLSE = ""
                            strXMLImages = ""
                            strXMLEntities = ""
                            strXMLRelatedProducts = ""
                            If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_STOCK Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_NON_STOCK Or _
                                ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then

                                ' build XML Product header only 
                                strXMLUpdate = BuildASPStorefrontCoreElements(ImportExportStatusDataset, ActiveASPStoreFrontSettings, "Update", False)

                                ' add all Tag Details for which there is an Inventory Action Status row
                                For iloop = 0 To ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221.Count - 1
                                    If ASPStorefrontTagAddedOrModified(ImportExportStatusDataset, ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221(iloop), ActiveSource, ActiveASPStoreFrontSettings, ItemCode) Then
                                        AddASPStorefrontTagDetail(ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221(iloop), strXMLUpdate, _
                                            strXMLDisplay, strXMLSE, strXMLImages, strXMLEntities, strXMLRelatedProducts)
                                    End If
                                Next

                                If strXMLSE <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<SE>" & vbCrLf & strXMLSE & "</SE>" & vbCrLf
                                End If
                                If strXMLDisplay <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<Display>" & vbCrLf & strXMLDisplay & "</Display>" & vbCrLf
                                End If
                                If strXMLImages <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<Images>" & vbCrLf & strXMLImages & "</Images>" & vbCrLf
                                End If
                                If strXMLRelatedProducts <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<RelatedProducts>" & vbCrLf & strXMLRelatedProducts & "</RelatedProducts>" & vbCrLf
                                End If

                                strCategories = ImportExportStatusFacade.GetRows(New String() {"CategoryGUID_DEV000221", "CategoryID_DEV000221"}, "InventoryASPStorefrontCategories_DEV000221", "ItemCode_DEV000221 = '" & _
                                    ItemCode & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "' AND IsActive_DEV000221 = 1")
                                For Each Category As String() In strCategories
                                    If True Then
                                        strXMLEntities = strXMLEntities & "<Entity EntityType=""Category"" ID=""" & Category(1) & """ GUID=""" & Category(0) & """/>" & vbCrLf
                                    End If
                                Next
                                If strXMLEntities <> "" Then
                                    strXMLUpdate = strXMLUpdate & " <Mappings>" & vbCrLf & strXMLEntities & "</Mappings>" & vbCrLf
                                End If

                            Else
                                ' need add processing for Kits 

                            End If

                            If strXMLUpdate <> "" Then
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLMessageType_DEV000221 = 11 ' 11 is UpdateProduct
                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionXMLFile_DEV000221 = strXMLUpdate
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLToSend_DEV000221 = True

                            Else
                                ' yes, mark row as complete
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True
                            End If


                        Case "200" ' Updated Item
                            ' start of code added TJS 24/02/12
                            strXMLUpdate = ""
                            strXMLDisplay = ""
                            strXMLSE = ""
                            strXMLImages = ""
                            strXMLEntities = ""
                            strXMLRelatedProducts = ""
                            If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_STOCK Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_NON_STOCK Or _
                                ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then

                                ' build XML Product header with all details
                                strXMLUpdate = BuildASPStorefrontCoreElements(ImportExportStatusDataset, ActiveASPStoreFrontSettings, "Update", True)

                                ' add all Tag Details for which there is an Inventory Action Status row
                                For iloop = 0 To ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221.Count - 1
                                    If ASPStorefrontTagAddedOrModified(ImportExportStatusDataset, ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221(iloop), ActiveSource, ActiveASPStoreFrontSettings, ItemCode) Then
                                        AddASPStorefrontTagDetail(ImportExportStatusDataset.InventoryASPStorefrontTagDetails_DEV000221(iloop), strXMLUpdate, _
                                            strXMLDisplay, strXMLSE, strXMLImages, strXMLEntities, strXMLRelatedProducts)
                                    End If
                                Next

                                strXMLUpdate = strXMLUpdate & BuildASPStorefrontExtensions(ImportExportStatusDataset, ActiveASPStoreFrontSettings)

                                If strXMLSE <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<SE>" & vbCrLf & strXMLSE & "</SE>" & vbCrLf
                                End If
                                If strXMLDisplay <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<Display>" & vbCrLf & strXMLDisplay & "</Display>" & vbCrLf
                                End If
                                If strXMLImages <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<Images>" & vbCrLf & strXMLImages & "</Images>" & vbCrLf
                                End If
                                If strXMLRelatedProducts <> "" Then
                                    strXMLUpdate = strXMLUpdate & "<RelatedProducts>" & vbCrLf & strXMLRelatedProducts & "</RelatedProducts>" & vbCrLf
                                End If

                                strCategories = ImportExportStatusFacade.GetRows(New String() {"CategoryGUID_DEV000221", "CategoryID_DEV000221"}, "InventoryASPStorefrontCategories_DEV000221", "ItemCode_DEV000221 = '" & _
                                    ItemCode & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "' AND IsActive_DEV000221 = 1")
                                For Each Category As String() In strCategories
                                    If True Then
                                        strXMLEntities = strXMLEntities & "<Entity EntityType=""Category"" ID=""" & Category(1) & """ GUID=""" & Category(0) & """/>" & vbCrLf
                                    End If
                                Next
                                If Not ImportExportStatusDataset.InventoryItem(0).IsManufacturerCodeNull AndAlso ImportExportStatusDataset.InventoryItem(0).ManufacturerCode <> "" Then
                                    strTemp = ImportExportStatusFacade.GetField("SourceManufacturerCode_DEV000221", "SystemManufacturerSourceID_DEV000221", "ManufacturerCode_DEV000221 = '" & _
                                        ImportExportStatusDataset.InventoryItem(0).ManufacturerCode & "' AND SourceCode_DEV000221 = '" & ASP_STORE_FRONT_SOURCE_CODE & _
                                        "' AND AccountOrInstanceID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                    If "" & strTemp <> "" Then
                                        strXMLEntities = strXMLEntities & "<Entity EntityType=""Manufacturer"" ID=""" & Left(strTemp, InStr(strTemp, ":") - 1) & """ GUID=""" & Mid(strTemp, InStr(strTemp, ":") + 1) & """/>" & vbCrLf
                                    End If
                                End If
                                If strXMLEntities <> "" Then
                                    strXMLUpdate = strXMLUpdate & " <Mappings>" & vbCrLf & strXMLEntities & "</Mappings>" & vbCrLf
                                End If

                                If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                                    ' need to create size/color variants from Matrix Items
                                    strAttributeCodes = ImportExportStatusFacade.GetRows(New String() {"AttributeCode", "PositionID"}, "InventoryAttribute", "ItemCode = '" & ItemCode & "' ORDER BY PositionID")
                                    strXMLUpdate = strXMLUpdate & "<Variants><Variant Action=""Update"" GUID=""" & ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ASPStorefrontVariantGUID_DEV000221 & """"
                                    If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsProductName_DEV000221Null AndAlso _
                                       ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221 <> INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
                                        strXMLUpdate = strXMLUpdate & " Name=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221) & """>" & vbCrLf
                                        strXMLUpdate = strXMLUpdate & "<Name>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221) & "</Name>" & vbCrLf
                                    Else
                                        strXMLUpdate = strXMLUpdate & " Name=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName) & """>" & vbCrLf
                                        strXMLUpdate = strXMLUpdate & "<Name>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName) & "</Name>" & vbCrLf
                                    End If
                                    strXMLUpdate = strXMLUpdate & "<IsDefault>True</IsDefault>"
                                    If strAttributeCodes.Length = 2 Then
                                        strMatrixItems = ImportExportStatusFacade.GetRows(New String() {"MatrixItemCode", "Attribute1", "Attribute2", "AttributeCode1", "AttributeCode2"}, "InventoryMatrixItem", "ItemCode = '" & ItemCode & "' AND Selected = 1 ORDER BY Attribute1, Attribute2")
                                        If strMatrixItems(0)(3) = "Size" Then
                                            strXMLUpdate = strXMLUpdate & BuildASPStorefrontSizeOptions(ItemCode, strMatrixItems(0)(3))
                                            strXMLUpdate = strXMLUpdate & BuildASPStorefrontColorOptions(ItemCode, strMatrixItems(0)(4))
                                        Else
                                            strXMLUpdate = strXMLUpdate & BuildASPStorefrontSizeOptions(ItemCode, strMatrixItems(0)(4))
                                            strXMLUpdate = strXMLUpdate & BuildASPStorefrontColorOptions(ItemCode, strMatrixItems(0)(3))
                                        End If
                                        strXMLInventory = "" & vbCrLf
                                        For Each MatrixItem As String() In strMatrixItems
                                            strTemp = ImportExportStatusFacade.GetField("Publish_DEV000221", "InventoryASPStorefrontDetails_DEV000221", "ItemCode_DEV000221 = '" & _
                                                MatrixItem(0) & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                            If strTemp = "1" Or strTemp.ToLower = "true" Then
                                                If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
                                                    strSKU = MatrixItem(0)
                                                Else
                                                    strSKU = ImportExportStatusFacade.GetField("ItemName", "inventoryItem", "ItemCode = '" & MatrixItem(0) & "'")
                                                End If
                                                If MatrixItem(3) = "Size" Then
                                                    strXMLInventory = strXMLInventory & "<Inv Color=""" & MatrixItem(2) & """ Size=""" & MatrixItem(1) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                                Else
                                                    strXMLInventory = strXMLInventory & "<Inv Color=""" & MatrixItem(1) & """ Size=""" & MatrixItem(2) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                                End If
                                            End If
                                            MarkMatrixItemAsComplete(ImportExportStatusDataset, ActiveSource, ActiveASPStoreFrontSettings, MatrixItem(0))
                                        Next
                                        If strXMLInventory <> "" Then
                                            strXMLUpdate = strXMLUpdate & "<InventoryBySizeAndColor>" & strXMLInventory & "</InventoryBySizeAndColor>" & vbCrLf
                                        End If

                                    ElseIf strAttributeCodes.Length = 1 And strAttributeCodes(0)(3) = "Size" Then
                                        strMatrixItems = ImportExportStatusFacade.GetRows(New String() {"MatrixItemCode", "Attribute1", "AttributeCode1"}, "InventoryMatrixItem", "ItemCode = '" & ItemCode & "' AND Selected = 1 ORDER BY Attribute1")
                                        strXMLUpdate = strXMLUpdate & BuildASPStorefrontSizeOptions(ItemCode, strMatrixItems(0)(2))
                                        strXMLInventory = ""
                                        For Each MatrixItem As String() In strMatrixItems
                                            strTemp = ImportExportStatusFacade.GetField("Publish_DEV000221", "InventoryASPStorefrontDetails_DEV000221", "ItemCode_DEV000221 = '" & _
                                                MatrixItem(0) & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                            If strTemp = "1" Or strTemp.ToLower = "true" Then
                                                If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
                                                    strSKU = MatrixItem(0)
                                                Else
                                                    strSKU = ImportExportStatusFacade.GetField("ItemName", "inventoryItem", "ItemCode = '" & MatrixItem(0) & "'")
                                                End If
                                                strXMLInventory = strXMLInventory & "<Inv Size=""" & MatrixItem(1) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                            End If
                                            MarkMatrixItemAsComplete(ImportExportStatusDataset, ActiveSource, ActiveASPStoreFrontSettings, MatrixItem(0))
                                        Next
                                        If strXMLInventory <> "" Then
                                            strXMLUpdate = strXMLUpdate & "<InventoryBySizeAndColor>" & strXMLInventory & "</InventoryBySizeAndColor>" & vbCrLf
                                        End If

                                    ElseIf strAttributeCodes.Length = 1 Then
                                        strMatrixItems = ImportExportStatusFacade.GetRows(New String() {"MatrixItemCode", "Attribute1", "AttributeCode1"}, "InventoryMatrixItem", "ItemCode = '" & ItemCode & "' AND Selected = 1 ORDER BY Attribute1")
                                        strXMLUpdate = strXMLUpdate & BuildASPStorefrontColorOptions(ItemCode, strMatrixItems(0)(2))
                                        strXMLInventory = ""
                                        For Each MatrixItem As String() In strMatrixItems
                                            strTemp = ImportExportStatusFacade.GetField("Publish_DEV000221", "InventoryASPStorefrontDetails_DEV000221", "ItemCode_DEV000221 = '" & _
                                                MatrixItem(0) & "' AND SiteID_DEV000221 = '" & ActiveASPStoreFrontSettings.SiteID & "'")
                                            If strTemp = "1" Or strTemp.ToLower = "true" Then
                                                If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
                                                    strSKU = MatrixItem(0)
                                                Else
                                                    strSKU = ImportExportStatusFacade.GetField("ItemName", "inventoryItem", "ItemCode = '" & MatrixItem(0) & "'")
                                                End If
                                                strXMLInventory = strXMLInventory & "<Inv Color=""" & MatrixItem(1) & """ VendorFullSKU=""" & strSKU & """/>" & vbCrLf
                                            End If
                                            MarkMatrixItemAsComplete(ImportExportStatusDataset, ActiveSource, ActiveASPStoreFrontSettings, MatrixItem(0))
                                        Next
                                        If strXMLInventory <> "" Then
                                            strXMLUpdate = strXMLUpdate & "<InventoryBySizeAndColor>" & strXMLInventory & "</InventoryBySizeAndColor>" & vbCrLf
                                        End If

                                    End If
                                    strXMLUpdate = strXMLUpdate & "</Variant>" & vbCrLf & "</Variants>" & vbCrLf
                                End If
                                strXMLUpdate = strXMLUpdate & "</Product>" & vbCrLf

                            Else
                                ' need add processing for Kits 

                            End If

                            ' check for any new tag records or updated item/tag records and mark them as complete
                            ' also check if a deleted item row exists
                            bBeenDeleted = False
                            iOrigRowID = RowID
                            For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveASPStoreFrontSettings.SiteID Then
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Or _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "205" Then
                                        ' new or updated Tag value
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True

                                    ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "200" Then
                                        ' duplicated update item row - mark as complete
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True

                                    ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "300" Then
                                        ' deleted item row, inhibit update to Magento
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True
                                        bBeenDeleted = True
                                    End If
                                Else
                                    ' set row pointer return value
                                    RowID = iCheckLoop - 1
                                    Exit For
                                End If
                                If bShutDownInProgress Then ' TJS 02/08/13
                                    Exit For ' TJS 02/08/13
                                End If
                            Next
                            ' did check loop exit without finding a new row for a different item ?
                            If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then
                                ' yes, set row pointer return value
                                RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                            End If

                            ' was a deleted item row found ?
                            If Not bBeenDeleted And strXMLUpdate <> "" Then
                                ' no, set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLMessageType_DEV000221 = 11 ' 11 is UpdateProduct
                                ' save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionXMLFile_DEV000221 = strXMLUpdate
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLToSend_DEV000221 = True

                            Else
                                ' yes, mark row as complete
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True
                            End If
                            ' end of code added TJS 24/02/12


                        Case "300" ' Deleted Item
                            ' mark row as complete
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True


                        Case "1000" ' Stock Quantity changed
                            bQtyUpdateRequired = False
                            ' get current total quantity available
                            decItemQtyAvailable = CDec(ImportExportStatusFacade.GetField("SELECT ISNULL(SUM(UnitsAvailable), 0) AS TotalAvailable FROM InventoryStockTotal WHERE ItemCode = '" & ItemCode & "'", System.Data.CommandType.Text, Nothing))
                            ' and values when last published
                            If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsTotalQtyWhenLastPublished_DEV000221Null Then
                                decItemTotalQtyWhenLastPublished = ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).TotalQtyWhenLastPublished_DEV000221
                            Else
                                decItemTotalQtyWhenLastPublished = 0
                            End If
                            If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null Then
                                decItemQtyLastPublished = ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).QtyLastPublished_DEV000221
                            Else
                                decItemQtyLastPublished = 0
                            End If

                            ' what is publishing basis ?
                            If ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                                iQtyToPublish = 0
                            Else
                                Select Case ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).QtyPublishingType_DEV000221
                                    Case "Fixed"
                                        ' always send update as orders may have changed value on source site
                                        iQtyToPublish = CInt(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                                        If iQtyToPublish < 0 Then
                                            iQtyToPublish = 0
                                        End If
                                        bQtyUpdateRequired = True

                                    Case "Percent"
                                        ' calculate value to be published based on percentage of available qty
                                        iQtyToPublish = CInt((decItemQtyAvailable * ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).QtyPublishingValue_DEV000221) / 100) ' TJS 15/03/13
                                        If iQtyToPublish < 0 Then
                                            iQtyToPublish = 0
                                        End If
                                        ' is value to publish positive and last value was null or 0 ?
                                        If (ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null OrElse _
                                            ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).QtyLastPublished_DEV000221 = 0) And iQtyToPublish > 0 Then
                                            ' yes, send value
                                            bQtyUpdateRequired = True

                                        ElseIf (Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                            ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).QtyLastPublished_DEV000221 > 0) And iQtyToPublish = 0 Then
                                            ' no, quantity now 0 and was previously positive, send value
                                            bQtyUpdateRequired = True

                                        ElseIf (Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                            ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).QtyLastPublished_DEV000221 < 10) Or iQtyToPublish < 10 Then
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
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 20 ' 20 is Update Product Stock Qty using InventoryUpdate
                                If Not bInhibitWebPosts Then
                                    strXMLQtyUpdate = "<Inv Quantity=""" & iQtyToPublish & """ VariantSKU=""" & ImportExportStatusDataset.InventoryItem(0).ItemName & """ />"
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
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveASPStoreFrontSettings.SiteID Then
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "1000" Then
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts
                                    End If
                                Else
                                    ' set row pointer return value
                                    RowID = iCheckLoop - 1
                                    Exit For
                                End If
                                If bShutDownInProgress Then ' TJS 02/08/13
                                    Exit For ' TJS 02/08/13
                                End If
                            Next

                    End Select
                End If
            ElseIf ImportExportStatusDataset.InventoryItem.Count > 0 Then
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoASPStorefrontInventoryUpdate", "No Inventory Item found for " & ItemCode) ' TJS 13/11/13
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoASPStorefrontInventoryUpdate", "No Inventory ASPStorefront Details found for " & ItemCode) ' TJS 13/11/13
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoASPStorefrontInventoryUpdate", ex)

        End Try
    End Sub

    Public Sub StartASPStoreFrontInventoryFile(ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, _
        ByVal ASPStoreFrontMessageType As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to cater for Inventory Quantity update
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlUpdateNode As XElement

        iASPStoreFrontXMLMessageType = ASPStoreFrontMessageType
        Try
            XMLInventoryUpdateFile = New XDocument

            XMLInventoryUpdateNode = New XElement("AspDotNetStorefrontImport")
            Select Case iASPStoreFrontXMLMessageType ' TJS 10/03/12
                Case 10, 11 ' 10 is CreateProduct, 11 is UpdateProduct
                    XMLInventoryUpdateNode.SetAttributeValue("Verbose", "")

                    xmlUpdateNode = New XElement("Transaction")

                Case 20 ' 20 is Update Product Stock Qty using InventoryUpdate  TJS 10/03/12

                    xmlUpdateNode = New XElement("InventoryUpdate") ' TJS 10/03/12

            End Select
            XMLInventoryUpdateNode.Add(xmlUpdateNode)

            XMLInventoryUpdateFile.Add(XMLInventoryUpdateNode)

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartASPStoreFrontInventoryFile", ex)

        End Try

    End Sub

    Public Sub AddToASPStoreFrontInventoryFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ByRef RowID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to cater for Inventory Quantity update
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStartingUpdateXML As String, iInsertPosn As Integer

        Try
            If Not ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null Then
                ' get current update XML
                strStartingUpdateXML = XMLInventoryUpdateFile.ToString
                ' get position for insert
                Select Case iASPStoreFrontXMLMessageType ' TJS 10/03/12
                    Case 10, 11 ' 10 is CreateProduct, 11 is UpdateProduct
                        iInsertPosn = InStr(strStartingUpdateXML, "</Transaction>")

                    Case 20 ' 20 is Update Product Stock Qty using InventoryUpdate  TJS 10/03/12
                        iInsertPosn = InStr(strStartingUpdateXML, "</InventoryUpdate>") ' TJS 10/03/12

                End Select
                If iInsertPosn > 0 Then
                    ' insert XML from action record
                    strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & Mid(strStartingUpdateXML, iInsertPosn)
                Else
                    ' must be empty Transaction, InventoryUpdate etc element
                    Select Case iASPStoreFrontXMLMessageType ' TJS 10/03/12
                        Case 10, 11 ' 10 is CreateProduct, 11 is UpdateProduct
                            iInsertPosn = InStr(strStartingUpdateXML, "<Transaction")
                            ' insert XML from action record
                            strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn + 11) & ">" & ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & "</Transaction>" & Mid(strStartingUpdateXML, InStr(strStartingUpdateXML, "/>") + 2)

                        Case 20 ' 20 is Update Product Stock Qty using InventoryUpdate  TJS 10/03/12
                            iInsertPosn = InStr(strStartingUpdateXML, "<InventoryUpdate") ' TJS 10/03/12
                            ' insert XML from action record
                            strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn + 15) & ">" & ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 & "</Transaction>" & Mid(strStartingUpdateXML, InStr(strStartingUpdateXML, "/>") + 2)

                    End Select
                End If
                ' reload update XML
                XMLInventoryUpdateFile = XDocument.Parse(strStartingUpdateXML)
            End If

            ' mark action record as XML Sent (change is committed to DB when status file successfully posted to ASPDotNetStoreFront in SendASPStoreFrontInventoryFile)
            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            bASPStoreFrontInventoryUpdatesToSend = True

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddToASPStoreFrontInventoryFile", ex)

        End Try

    End Sub

    Public Sub SendASPStoreFrontInventoryFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to cater for Inventory Quantity update and 
        '                                        | Product/Variant ID being saved as well as GUID because some updated need the ID
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ASPStorefrontConnection As Lerryn.Facade.ImportExport.ASPStorefrontConnector
        Dim XMLResponse As XDocument, XMLTemp As XDocument
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLITemNode As XElement
        Dim strReturn As String, strItemCode As String, sTemp As String, strLogMessage As String
        Dim bInhibitWebPosts As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")

            If Not bInhibitWebPosts Then
                ASPStorefrontConnection = New Lerryn.Facade.ImportExport.ASPStorefrontConnector
                sTemp = ""
                XMLResponse = ASPStorefrontConnection.SendXMLToASPStorefront(ActiveASPStoreFrontSettings.UseWSE3Authentication, _
                    ActiveASPStoreFrontSettings.APIURL, ActiveASPStoreFrontSettings.APIUser, ActiveASPStoreFrontSettings.APIPwd, XMLInventoryUpdateFile.ToString, sTemp)
                ' any errors ?
                If sTemp = "" Then
                    ' no, is response a success message ?
                    Select Case iASPStoreFrontXMLMessageType ' TJS 10/03/12

                        Case 10, 11 ' 10 is CreateProduct, 11 is UpdateProduct
                            If XMLResponse.XPathSelectElement("AspDotNetStorefrontImportResult/Transaction/Item") IsNot Nothing Then
                                ' yes
                                strLogMessage = ""
                                XMLItemList = XMLResponse.XPathSelectElements("AspDotNetStorefrontImportResult/Transaction/Item")
                                For Each XMLITemNode In XMLItemList
                                    ' need to replace nested XML entities
                                    XMLTemp = XDocument.Parse(XMLITemNode.ToString.Replace("&amp;lt;", "&lt;").Replace("&amp;gt;", "&gt;").Replace("&amp;quot;", "&quot;").Replace("&amp;apos;", "&apos;"))
                                    Select Case m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "NodeType")
                                        Case "Product"
                                            If m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "ActionTaken") = "Add" And _
                                                m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Status") = "OK" Then

                                                strItemCode = m_ImportExportConfigFacade.GetField("ItemCode", "InventoryItem", "ItemName = '" & m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Name") & "'")

                                                m_ImportExportConfigFacade.ExecuteNonQuery(System.Data.CommandType.Text, "UPDATE dbo.InventoryASPStorefrontDetails_DEV000221 SET ASPStorefrontProductGUID_DEV000221 = '" & _
                                                    m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "GUID") & "', ASPStorefrontProductID_DEV000221 = " & _
                                                    m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "ID") & " WHERE ItemCode_DEV000221 = '" & strItemCode & "'", Nothing) ' TJS 10/03/12

                                                strLogMessage = strLogMessage & "Successfully added new product " & m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Name") & " to ASPStorefront site " & ActiveASPStoreFrontSettings.SiteID

                                            ElseIf m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "ActionTaken") = "Update" And _
                                                m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Status") = "OK" Then

                                                strLogMessage = strLogMessage & ". Successfully added new product variant " & m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Name") & " to ASPStorefront site " & ActiveASPStoreFrontSettings.SiteID

                                            Else
                                                m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendASPStoreFrontInventoryFile", "Unknown AspDotNetStorefrontImportResult Action/Status", XMLResponse.ToString)
                                                Exit For
                                            End If

                                        Case "Variant"
                                            If m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "ActionTaken") = "Add" And _
                                                m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Status") = "OK" Then

                                                strItemCode = m_ImportExportConfigFacade.GetField("ItemCode", "InventoryItem", "ItemName = '" & m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Name") & "'")

                                                m_ImportExportConfigFacade.ExecuteNonQuery(System.Data.CommandType.Text, "UPDATE dbo.InventoryASPStorefrontDetails_DEV000221 SET ASPStorefrontVariantGUID_DEV000221 = '" & _
                                                    m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "GUID") & "', ASPStorefrontVariantID_DEV000221 = " & _
                                                    m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "ID") & " WHERE ItemCode_DEV000221 = '" & strItemCode & "'", Nothing) ' TJS 10/03/12

                                                strLogMessage = strLogMessage & "Successfully updated product " & m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Name") & " in ASPStorefront site " & ActiveASPStoreFrontSettings.SiteID

                                            ElseIf m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "ActionTaken") = "Update" And _
                                                m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Status") = "OK" Then

                                                strLogMessage = strLogMessage & ". Successfully updated product variant " & m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "Name") & " in ASPStorefront site " & ActiveASPStoreFrontSettings.SiteID

                                            Else
                                                m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendASPStoreFrontInventoryFile", "Unknown AspDotNetStorefrontImportResult Action/Status", XMLResponse.ToString)
                                                Exit For
                                            End If

                                        Case Else
                                            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendASPStoreFrontInventoryFile", "Unknown AspDotNetStorefrontImportResult Item Node Type " & m_ImportExportConfigFacade.GetXMLElementAttribute(XMLTemp, "Item", "NodeType"), XMLResponse.ToString)
                                    End Select
                                Next
                                bASPStoreFrontInventoryUpdatesToSend = False
                                If strLogMessage <> "" Then
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("ASPDotNetStorefront Send Inventory File - " & strLogMessage)
                                End If

                                ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                                    "CreateLerrynImportExportInventoryActionStatus_DEV000221", "UpdateLerrynImportExportInventoryActionStatus_DEV000221", "DeleteLerrynImportExportInventoryActionStatus_DEV000221"}}, _
                                    Interprise.Framework.Base.Shared.TransactionType.None, "ASPDotNetStorefront Send Inventory File", False)

                            Else
                                sTemp = m_ImportExportConfigFacade.GetXMLElementAttribute(XMLResponse, "AspDotNetStorefrontImportResult/Error", "Message")
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendASPStoreFrontInventoryFile", sTemp) ' TJS 13/11/13
                            End If

                        Case 20 ' 20 is Update Product Stock Qty using InventoryUpdate  TJS 10/03/12


                    End Select
                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendASPStoreFrontInventoryFile", sTemp) ' TJS 13/11/13
                End If
                ASPStorefrontConnection = Nothing

            Else
                m_ImportExportConfigFacade.WriteLogProgressRecord("ASPDotNetStorefront Send Inventory File - Inhibited from sending Invntory update file - content " & XMLInventoryUpdateFile.ToString)
                strReturn = "<?xml version=""1.0"" encoding=""UTF-8""?>"

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendASPStoreFrontInventoryFile", ex)

        End Try

    End Sub

    Private Function BuildASPStorefrontCoreElements(ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ByVal Action As String, ByVal IncludeDetails As Boolean) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strReturn As String

        If Action = "Add" Then
            strReturn = "<Product Action=""" & Action & """ EnsureDefaultVariant=""True"" "
        Else
            strReturn = "<Product Action=""" & Action & """ EnsureDefaultVariant=""True"" GUID=""" & ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ASPStorefrontProductGUID_DEV000221 & """ "
        End If
        If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
            strReturn = strReturn & "SKU=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemCode)
        Else
            strReturn = strReturn & "SKU=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName)
        End If
        If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsProductName_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221 <> INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
            strReturn = strReturn & """ Name=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221) & """>" & vbCrLf
            strReturn = strReturn & "<Name>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductName_DEV000221) & "</Name>" & vbCrLf
        Else
            strReturn = strReturn & """ Name=""" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName) & """>" & vbCrLf
            strReturn = strReturn & "<Name>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName) & "</Name>" & vbCrLf
        End If

        If IncludeDetails Then
            If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsProductDescription_DEV000221Null AndAlso _
                ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductDescription_DEV000221 <> INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then ' TJS 13/11/13
                strReturn = strReturn & "<Description>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductDescription_DEV000221) & "</Description>" & vbCrLf
            Else
                strReturn = strReturn & "<Description>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItemDescription(0).ItemDescription) & "</Description>" & vbCrLf
            End If
            If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsProductSummary_DEV000221Null Then
                strReturn = strReturn & "<Summary>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).ProductSummary_DEV000221) & "</Summary>" & vbCrLf
            End If
            If ActiveASPStoreFrontSettings.ISItemIDField = "ItemCode" Then
                strReturn = strReturn & "<SKU>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemCode) & "</SKU>" & vbCrLf
            Else
                strReturn = strReturn & "<SKU>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0).ItemName) & "</SKU>" & vbCrLf
            End If
            If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsAvailableFrom_DEV000221Null Then
                strReturn = strReturn & "<AvailableStartDate>" & ASPStorefrontXMLDate(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).AvailableFrom_DEV000221) & "</AvailableStartDate>" & vbCrLf
            End If
            If Not ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).IsAvailableTo_DEV000221Null Then
                strReturn = strReturn & "<AvailableStopDate>" & ASPStorefrontXMLDate(ImportExportStatusDataset.InventoryASPStorefrontDetails_DEV000221(0).AvailableTo_DEV000221) & "</AvailableStopDate>" & vbCrLf
            End If
        End If

        Return strReturn

    End Function

    Private Function BuildASPStorefrontExtensions(ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings) As String
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

        Dim strReturn As String = ""

        If ActiveASPStoreFrontSettings.ExtensionDataField1Mapping <> "" Then
            strReturn = strReturn & "<ExtensionData>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0)(ActiveASPStoreFrontSettings.ExtensionDataField1Mapping).ToString) & "</ExtensionData>" & vbCrLf
        End If
        If ActiveASPStoreFrontSettings.ExtensionDataField2Mapping <> "" Then
            strReturn = strReturn & "<ExtensionData2>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0)(ActiveASPStoreFrontSettings.ExtensionDataField2Mapping).ToString) & "</ExtensionData2>" & vbCrLf
        End If
        If ActiveASPStoreFrontSettings.ExtensionDataField3Mapping <> "" Then
            strReturn = strReturn & "<ExtensionData3>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0)(ActiveASPStoreFrontSettings.ExtensionDataField3Mapping).ToString) & "</ExtensionData3>" & vbCrLf
        End If
        If ActiveASPStoreFrontSettings.ExtensionDataField4Mapping <> "" Then
            strReturn = strReturn & "<ExtensionData4>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0)(ActiveASPStoreFrontSettings.ExtensionDataField4Mapping).ToString) & "</ExtensionData4>" & vbCrLf
        End If
        If ActiveASPStoreFrontSettings.ExtensionDataField5Mapping <> "" Then
            strReturn = strReturn & "<ExtensionData5>" & m_ImportExportConfigFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryItem(0)(ActiveASPStoreFrontSettings.ExtensionDataField5Mapping).ToString) & "</ExtensionData5>" & vbCrLf
        End If

        Return strReturn

    End Function

    Private Sub AddASPStorefrontTagDetail(ByRef rowASPStorefrontTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontTagDetails_DEV000221Row, _
        ByRef strXMLUpdate As String, ByRef strXMLDisplay As String, ByRef strXMLSE As String, ByRef strXMLImages As String, _
        ByRef strXMLEntities As String, ByRef strXMLRelatedProducts As String)
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

        Dim strTemp As String

        Select Case rowASPStorefrontTagDetails.ParentNode_DEV000221
            Case "root"
                strXMLUpdate = strXMLUpdate & "<" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">"
                strXMLUpdate = strXMLUpdate & GetASPStorefrontTagDataValue(rowASPStorefrontTagDetails)
                strXMLUpdate = strXMLUpdate & "</" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">" & vbCrLf

            Case "Display"
                strXMLDisplay = strXMLDisplay & "<" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">"
                strXMLDisplay = strXMLDisplay & GetASPStorefrontTagDataValue(rowASPStorefrontTagDetails)
                strXMLDisplay = strXMLDisplay & "</" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">" & vbCrLf

            Case "SE"
                strXMLSE = strXMLSE & "<" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">"
                strXMLSE = strXMLSE & GetASPStorefrontTagDataValue(rowASPStorefrontTagDetails)
                strXMLSE = strXMLSE & "</" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">" & vbCrLf

            Case "Images"
                strXMLImages = strXMLImages & "<" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">"
                strXMLImages = strXMLImages & GetASPStorefrontTagDataValue(rowASPStorefrontTagDetails)
                strXMLImages = strXMLImages & "</" & rowASPStorefrontTagDetails.TagName_DEV000221 & ">" & vbCrLf

            Case "Mappings"
                strTemp = rowASPStorefrontTagDetails.TagTextValue_DEV000221
                Select Case rowASPStorefrontTagDetails.TagName_DEV000221
                    Case "Entity:Section"
                        strXMLEntities = strXMLEntities & "<Entity EntityType=""Section"" ID=""" & Left(strTemp, InStr(strTemp, ":") - 1) & """ GUID=""" & Mid(strTemp, InStr(strTemp, ":") + 1) & """/>" & vbCrLf

                    Case "Entity:Distributor"
                        strXMLEntities = strXMLEntities & "<Entity EntityType=""Distributor"" ID=""" & Left(strTemp, InStr(strTemp, ":") - 1) & """ GUID=""" & Mid(strTemp, InStr(strTemp, ":") + 1) & """/>" & vbCrLf

                    Case "Entity:Genre"
                        strXMLEntities = strXMLEntities & "<Entity EntityType=""Manufacturer"" ID=""" & Left(strTemp, InStr(strTemp, ":") - 1) & """ GUID=""" & Mid(strTemp, InStr(strTemp, ":") + 1) & """/>" & vbCrLf

                End Select

            Case "RelatedProducts"
                strTemp = rowASPStorefrontTagDetails.TagTextValue_DEV000221
                strXMLRelatedProducts = strXMLRelatedProducts & "<CX GUID=""" & Mid(strTemp, InStr(strTemp, ":") + 1) & """/>" & vbCrLf

        End Select

    End Sub

    Private Function ASPStorefrontTagAddedOrModified(ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef rowASPStorefrontTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontTagDetails_DEV000221Row, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ByVal ItemCode As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Checks the Inventory Action Status rows for a new or updated TagDetails row 
        '                   matching the ItemCode, SourceCode, SiteID of the item being processed 
        '                   Returns true if a match is found, otherwise false.  
        '                   Also marks the row as action complete 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
            If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).ItemCode_DEV000221 = ItemCode And _
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).StoreMerchantID_DEV000221 = ActiveASPStoreFrontSettings.SiteID And _
                Left(ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).TableTagID_DEV000221, 10) = "InvASPTag-" AndAlso _
                CInt(Mid(ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).TableTagID_DEV000221, 11)) = rowASPStorefrontTagDetails.Counter AndAlso _
                (ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).InventoryStatus_DEV000221 = "105" Or _
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).InventoryStatus_DEV000221 = "205") And _
                Not ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).ActionComplete_DEV000221 Then
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).ActionComplete_DEV000221 = True
                Return True
            End If
        Next

        Return False

    End Function

    Private Function GetASPStorefrontTagDataValue(ByRef rowASPStorefrontTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryASPStorefrontTagDetails_DEV000221Row) As String
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

        Select Case rowASPStorefrontTagDetails.TagDataType_DEV000221
            Case "Text", "Boolean"
                If Not rowASPStorefrontTagDetails.IsTagTextValue_DEV000221Null Then
                    Return m_ImportExportConfigFacade.ConvertEntitiesForXML(rowASPStorefrontTagDetails.TagTextValue_DEV000221)
                Else
                    Return ""
                End If

            Case "Memo"
                If Not rowASPStorefrontTagDetails.IsTagMemoValue_DEV000221Null Then
                    Return m_ImportExportConfigFacade.ConvertEntitiesForXML(rowASPStorefrontTagDetails.TagMemoValue_DEV000221)
                Else
                    Return ""
                End If

            Case "Integer", "Decimal"
                If Not rowASPStorefrontTagDetails.IsTagNumericValue_DEV000221Null Then
                    Return rowASPStorefrontTagDetails.TagNumericValue_DEV000221.ToString
                Else
                    Return ""
                End If

            Case "DateTime"
                If Not rowASPStorefrontTagDetails.IsTagDateValue_DEV000221Null Then
                    Return ASPStorefrontXMLDate(rowASPStorefrontTagDetails.TagDateValue_DEV000221)
                Else
                    Return ""
                End If

            Case Else
                Return ""
        End Select

    End Function

    Private Function BuildASPStorefrontSizeOptions(ByVal ItemCode As String, ByVal AttributeCode As String) As String
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

        Dim strAttributeValues As String()(), strReturn As String, strSeparator As String

        strReturn = ""
        strSeparator = m_ImportExportConfigFacade.GetField("Separator", "InventoryMatrixGroup", "ItemCode = '" & ItemCode & "'")
        strAttributeValues = m_ImportExportConfigFacade.GetRows(New String() {"AttributeValueCode", "AttributeValueDescription"}, "InventoryAttributeValueView_DEV000221", "ItemCode = '" & _
            ItemCode & "' AND AttributeCode = '" & AttributeCode & "' AND LanguageCode = '" & m_ImportExportConfigFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE) & _
            "' ORDER BY eShopCONNECTDisplayOrder_DEV000221, AttributeValueDescription")
        For Each AttributeValue As String() In strAttributeValues
            strReturn = strReturn & "<Size SKUModifier=""" & strSeparator & AttributeValue(0) & """ PriceDelta=""0"">" & AttributeValue(1) & "</Size>"
        Next
        If strReturn <> "" Then
            Return "<Sizes>" & strReturn & "</Sizes>"
        Else
            Return ""
        End If

    End Function

    Private Function BuildASPStorefrontColorOptions(ByVal ItemCode As String, ByVal AttributeCode As String) As String
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

        Dim strAttributeValues As String()(), strReturn As String, strSeparator As String

        strReturn = ""
        strSeparator = m_ImportExportConfigFacade.GetField("Separator", "InventoryMatrixGroup", "ItemCode = '" & ItemCode & "'")
        strAttributeValues = m_ImportExportConfigFacade.GetRows(New String() {"AttributeValueCode", "AttributeValueDescription"}, "InventoryAttributeValueView_DEV000221", "ItemCode = '" & _
            ItemCode & "' AND AttributeCode = '" & AttributeCode & "' AND LanguageCode = '" & m_ImportExportConfigFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE) & _
            "' ORDER BY eShopCONNECTDisplayOrder_DEV000221, AttributeValueDescription")
        For Each AttributeValue As String() In strAttributeValues
            strReturn = strReturn & "<Color SKUModifier=""" & strSeparator & AttributeValue(0) & """ PriceDelta=""0"">" & AttributeValue(1) & "</Color>"
        Next
        If strReturn <> "" Then
            Return "<Colors>" & strReturn & "</Colors>"
        Else
            Return ""
        End If

    End Function

    Private Sub MarkMatrixItemAsComplete(ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveASPStoreFrontSettings As ASPStoreFrontSettings, ByVal MatrixItemCode As String)
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

        Dim iLoop As Integer

        For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
            ' action record for the Matrix Item just processed ?
            If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).ItemCode_DEV000221 = MatrixItemCode And _
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).StoreMerchantID_DEV000221 = ActiveASPStoreFrontSettings.SiteID Then
                ' yes, mark as complete
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iLoop).ActionComplete_DEV000221 = True
            End If
        Next

    End Sub

    Public Function ASPStorefrontXMLDate(ByVal DateToUse As Date) As String

        Dim strASPStorefrontXMLDate As String

        strASPStorefrontXMLDate = DateToUse.Year & "-" & Right("00" & DateToUse.Month, 2) & "-" & Right("00" & DateToUse.Day, 2)
        strASPStorefrontXMLDate = strASPStorefrontXMLDate & "T" & Right("00" & DateToUse.Hour, 2) & ":" & Right("00" & DateToUse.Minute, 2)
        strASPStorefrontXMLDate = strASPStorefrontXMLDate & ":" & Right("00" & DateToUse.Second, 2)
        Return strASPStorefrontXMLDate

    End Function

End Module
