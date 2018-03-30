' eShopCONNECT for Connected Business - Windows Service
' Module: MagentoInventoryUpdate.vb
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
' Last Updated - 01 May 2014

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11
Imports System.IO
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Module MagentoInventoryUpdate

    Private MagentoConnection As Lerryn.Facade.ImportExport.MagentoSOAPConnector = Nothing

    Public Sub DoMagentoInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveMagentoSettings As MagentoSettings, ByRef RowID As Integer, ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Added code to detect other changes at same time as an updated item
        ' 04/05/11 | TJS             | 2011.0.13 | Corrected XML Message type for new product creation and Attribute Set parameters
        ' 19/07/11 | TJS/FA          | 2011.1.03 | Modified check for magento tags to test for a value of nothing
        '                                          as this will cause an error
        ' 19/07/11 | FA              | 2011.1.04 | Modified to check for empty string as well, otherwise conversion error can occur
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, to check for 
        '                                        | Magento posting errors and to start processing Inventory stock quantities
        ' 24/02/12 | TJS             | 2011.2.08 | Modifeid to detect Inventory Type and skip items not yet catered for
        ' 10/03/12 | TJS             | 2011.2.09 | Modified to detect and ignore new records imported via Import Wizard
        ' 10/08/12 | TJS             | 2012.1.12 | Added missing Namespace manager references for MAgento returned XML processins
        ' 22/08/12 | TJS             | 2012.1.14 | Modified to update Magento product Categories for Inventory Items
        ' 15/03/13 | TJS             | 2013.1.03 | Corrected calculation of quantity to publish when sending Percent (divide by 100 missing)
        ' 30/04/13 | TJS/FA          | 2013.1.11 | Modified to cater for InhibitInventoryUpdates
        ' 21/06/13 | FA              | 2013.1.20 | Corrected redimensioning of array for item category 100 and 200
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 05/10/13 | TJS             | 2013.3.05 | Modified to pass website ids to CreateCatalogProduct
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to ensure Magento Tag records have same XML Message type as main item, 
        '                                        | to populate website id list and to cater for Matrix Groups
        ' 15/11/13 | TJS             | 2013.3.09 | Modified to trigger Inventory Quantity update after publishing a new product if 
        ' 09/12/13 | TJS             | 2013.4.02 | Corrected use of iOrigRowID and iCheckLoop in several places
        ' 29/01/13 | TJS             | 2013.4.07 | Modified to cater for empty Magento Product ID and to add missing core Magento attribute values
        ' 11/02/14 | TJS             | 2013.4.09 | Added further detail to log messages about inhibited updates
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim rowInventoryActionStatus As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportInventoryActionStatus_DEV000221Row ' TJS 13/11/13
        Dim ProductAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType() ' TJS 25/04/11
        Dim strTemp As String, strSKU As String, strProductType As String, strAtributeSet As String ' TJS 25/04/11 TJS 04/05/11 TJS 02/12/11
        Dim strMatrixItems As String()(), strCategoriesAndWebsites As String()(), strCategorOrWebsite As String() ' TJS 13/11/13
        Dim strMatrixItemIDs As String() ' TJS 13/11/13
        Dim iAttributePtr As Integer, iOrigRowID As Integer, iCheckLoop As Integer ' TJS 25/04/11 TJS 04/05/11 TJS 13/11/13
        Dim decItemQtyAvailable As Decimal, decItemTotalQtyWhenLastPublished As Decimal, decItemQtyLastPublished As Decimal ' TJS 02/12/11
        Dim iQtyToPublish As Integer, iProductCategoryCount As Integer, iProductCategoryList() As Integer, iWebsiteCount As Integer, iWebsiteList() As Integer ' TJS 25/04/11 TJS 02/12/11 TJS 22/08/12 TJS 05/10/13 TJS 13/11/13
        Dim bQtyUpdateRequired As Boolean, bIsInStock As Boolean, bBeenDeleted As Boolean, bInhibitWebPosts As Boolean, bCategoryExists As Boolean ' TJS 02/12/11 TJS 22/08/12
        Dim bWebsiteExists As Boolean ' TJS 13/11/13

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") Or _
                ActiveMagentoSettings.InhibitInventoryUpdates ' TJS 25/04/11 TJS 30/04/13
            ' yes, check for inventory updates
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, "ReadInventoryItem", AT_ITEM_CODE, ItemCode}, _
                New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, "ReadInventoryItemDescription", AT_ITEM_CODE, ItemCode, _
                    AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
                New String() {ImportExportStatusDataset.InventorySpecialPricing.TableName, "ReadInventorySpecialPricing_DEV000221", AT_ITEM_CODE, ItemCode}, _
                New String() {ImportExportStatusDataset.InventoryMagentoDetails_DEV000221.TableName, "ReadInventoryMagentoDetails_DEV000221", AT_ITEM_CODE, ItemCode, _
                    AT_INSTANCE_ID, ActiveMagentoSettings.InstanceID},
                New String() {ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221.TableName, "ReadInventoryMagentoTagDetails_DEV000221", _
                    AT_ITEM_CODE, ItemCode, AT_INSTANCE_ID, ActiveMagentoSettings.InstanceID}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 29/01/13

            If ImportExportStatusDataset.InventoryItem.Count > 0 And ImportExportStatusDataset.InventoryMagentoDetails_DEV000221.Count > 0 Then
                iProductCategoryCount = 0 ' TJS 22/08/12
                ReDim iProductCategoryList(iProductCategoryCount - 1) ' TJS 22/08/12
                iWebsiteCount = 0 ' TJS 13/11/13
                ReDim iWebsiteList(iWebsiteCount - 1) ' TJS 13/11/13
                ' ignore any records already marked as complete since Matrix Items will be processed as part of new Matrix Groups and may not be in sequence
                If Not ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 Then ' TJS 24/02/12
                    Select Case ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221
                        Case "100" ' New Item
                            ' check Magento Product ID is blank and not imported from wizard
                            If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).FromImportWizard_DEV000221 And _
                                (ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsMagentoProductID_DEV000221Null OrElse _
                                ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).MagentoProductID_DEV000221 = "") Then ' TJS 10/03/12
                                ' yes, must be a newly published item
                                If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_STOCK Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_NON_STOCK Or _
                                    ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_SERVICE Or _
                                    ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then ' TJS 24/02/12 TJS 05/10/13 TJS 13/11/13

                                    ' start of code added TJS 25/04/11
                                    BuildProductCoreAttributeList(ImportExportStatusFacade, ImportExportStatusDataset, ProductAttributes, iAttributePtr, ItemCode) ' TJS 13/11/13

                                    ' check for any new tag records or updated item/tag records and mark them as complete
                                    ' also check if a deleted item row exists
                                    bBeenDeleted = False
                                    iOrigRowID = RowID
                                    For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                        If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveMagentoSettings.InstanceID Then ' TJS 02/12/11
                                            If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Or _
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "205" Then
                                                ' new or updated Tag/Category value
                                                strTemp = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).TableTagID_DEV000221
                                                If Left(strTemp, 10) = "InvMagTag-" Then
                                                    AddProductAttribute(ImportExportStatusFacade, ImportExportStatusDataset, ProductAttributes, iAttributePtr, CInt(strTemp.Substring(10))) ' TJS 13/11/13 TJS 29/01/14

                                                ElseIf Left(strTemp, 10) = "InvMagCat-" Then ' TJS 04/05/11
                                                    AddProductCategory(ImportExportStatusFacade, ImportExportStatusDataset, iProductCategoryList, iProductCategoryCount, iWebsiteList, iWebsiteCount, ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221, ItemCode, strTemp.Substring(10)) ' TJS 13/11/13 TJS 29/01/14

                                                End If
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts ' TJS 04/05/11
                                                If Not bInhibitWebPosts Then ' TJS 13/11/13
                                                    ' set XML Message Type to match main record
                                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).XMLMessageType_DEV000221 = 10 ' 10 is CreateCatalogProduct TJS 13/11/13 TJS 09/12/13
                                                End If

                                            ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "200" Then
                                                ' duplicated update item row - mark as complete
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts ' TJS 04/05/11
                                                If Not bInhibitWebPosts Then ' TJS 13/11/13
                                                    ' set XML Message Type to match main record
                                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLMessageType_DEV000221 = 10 ' 10 is CreateCatalogProduct TJS 13/11/13
                                                End If

                                            ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "300" Then
                                                ' deleted item row, inhibit update to Magento
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts ' TJS 04/05/11
                                                bBeenDeleted = True
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

                                    ' was a deleted item row found ?
                                    If Not bBeenDeleted Then
                                        ' no, set message type and mark record as having XML to send
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLMessageType_DEV000221 = 10 ' 10 is CreateCatalogProduct TJS 01/04/11 
                                        If Not bInhibitWebPosts Then
                                            OpenMagentoInventoryConnection(ActiveSource, ActiveMagentoSettings)
                                            If MagentoConnection.LoggedIn Then
                                                If ActiveMagentoSettings.ISItemIDField = "ItemCode" Then
                                                    strSKU = ItemCode
                                                Else
                                                    strSKU = ImportExportStatusDataset.InventoryItem(0).ItemName
                                                End If
                                                If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_STOCK Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_NON_STOCK Or _
                                                    ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_SERVICE Then ' TJS 05/10/13
                                                    strProductType = "simple"
                                                ElseIf ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then ' TJS 13/11/13
                                                    strProductType = "configurable" ' TJS 13/11/13
                                                Else
                                                    strProductType = ""
                                                End If
                                                If ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsAttributeSetID_DEV000221Null Then ' TJS 04/05/11
                                                    strAtributeSet = "" ' TJS 04/05/11
                                                Else
                                                    strAtributeSet = CStr(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).AttributeSetID_DEV000221)
                                                End If
                                                If MagentoConnection.CreateCatalogProduct(strSKU, strProductType, strAtributeSet, ProductAttributes, iProductCategoryList, iWebsiteList) Then ' TJS 04/05/11 TJS 22/08/12 TJS 05/10/13
                                                    ' yes, get Magento Product ID
                                                    XMLNameTabMagento = New System.Xml.NameTable ' TJS 10/08/12
                                                    XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 10/08/12
                                                    If ActiveMagentoSettings.V2SoapAPIWSICompliant And MagentoConnection.V2SoapWSIAPIResponse Then ' TJS 13/11/13
                                                        strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductCreateResponseParam/result", XMLNSManMagento) ' TJS 13/11/13
                                                    ElseIf MagentoConnection.V2SoapAPIResponse Then ' TJS 13/11/13
                                                        strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductCreateResponse/result", XMLNSManMagento) ' TJS 13/11/13
                                                    Else
                                                        strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento) ' TJS 04/05/11 TJS 10/08/12
                                                    End If
                                                    If strTemp <> "" Then
                                                        m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.InventoryMagentoDetails_DEV000221 SET MagentoProductID_DEV000221 = '" & _
                                                            strTemp & "' WHERE ItemCode_DEV000221 = '" & ItemCode & "' AND InstanceID_DEV000221 = '" & _
                                                            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).InstanceID_DEV000221 & "'", Nothing) ' TJS 04/05/11
                                                    End If
                                                    ' and mark row as complete
                                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 04/05/11
                                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Published Inventory Item " & ItemCode & " (" & strSKU & ") as Magento product ID " & strTemp) ' TJS 15/11/13
                                                    ' start of code added TJS 13/11/13
                                                    ' is item either a Matrix Group or a Matrix Item ?
                                                    If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Or ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_ITEM Then
                                                        ' need to process Magento Related Products
                                                        rowInventoryActionStatus = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.NewLerrynImportExportInventoryActionStatus_DEV000221Row()
                                                        If ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then
                                                            rowInventoryActionStatus.ItemCode_DEV000221 = ItemCode
                                                        Else
                                                            strTemp = m_ImportExportConfigFacade.GetField("ItemCode", "InventoryMatrixItem", "MatrixItemCode = '" & ItemCode & "'")
                                                            rowInventoryActionStatus.ItemCode_DEV000221 = strTemp
                                                        End If
                                                        rowInventoryActionStatus.SourceCode_DEV000221 = MAGENTO_SOURCE_CODE
                                                        rowInventoryActionStatus.StoreMerchantID_DEV000221 = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).StoreMerchantID_DEV000221
                                                        rowInventoryActionStatus.TableTagID_DEV000221 = "InvMagentoDet"
                                                        rowInventoryActionStatus.InventoryStatus_DEV000221 = "150"
                                                        rowInventoryActionStatus.ActionTimestamp_DEV000221 = Date.Now
                                                        rowInventoryActionStatus.ActionComplete_DEV000221 = False
                                                        rowInventoryActionStatus.XMLMessageType_DEV000221 = 12 ' Magento Related Products
                                                        rowInventoryActionStatus.MessageAcknowledged_DEV000221 = False
                                                        rowInventoryActionStatus.ErrorReported_DEV000221 = False
                                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.AddLerrynImportExportInventoryActionStatus_DEV000221Row(rowInventoryActionStatus)
                                                        ImportExportStatusFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1", Nothing) ' TJS 09/12/13
                                                    End If
                                                    ' end of code added TJS 13/11/13

                                                    ' start of code added TJS 15/11/13
                                                    If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null AndAlso _
                                                        (ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingType_DEV000221 = "Fixed" Or _
                                                        ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingType_DEV000221 = "Percent") Then
                                                        ' need to process inventory quantity
                                                        rowInventoryActionStatus = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.NewLerrynImportExportInventoryActionStatus_DEV000221Row()
                                                        rowInventoryActionStatus.ItemCode_DEV000221 = ItemCode
                                                        rowInventoryActionStatus.SourceCode_DEV000221 = MAGENTO_SOURCE_CODE
                                                        rowInventoryActionStatus.StoreMerchantID_DEV000221 = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).StoreMerchantID_DEV000221
                                                        rowInventoryActionStatus.TableTagID_DEV000221 = "InvMagentoDet"
                                                        rowInventoryActionStatus.InventoryStatus_DEV000221 = "1000"
                                                        rowInventoryActionStatus.ActionTimestamp_DEV000221 = Date.Now
                                                        rowInventoryActionStatus.ActionComplete_DEV000221 = False
                                                        rowInventoryActionStatus.XMLMessageType_DEV000221 = 20 ' UpdateProductStockQty
                                                        rowInventoryActionStatus.MessageAcknowledged_DEV000221 = False
                                                        rowInventoryActionStatus.ErrorReported_DEV000221 = False
                                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.AddLerrynImportExportInventoryActionStatus_DEV000221Row(rowInventoryActionStatus)
                                                        ImportExportStatusFacade.ExecuteNonQuery(CommandType.Text, "UPDATE dbo.LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1", Nothing) ' TJS 09/12/13
                                                    End If
                                                    ' end of code added TJS 15/11/13
                                                Else
                                                    XMLNameTabMagento = New System.Xml.NameTable ' TJS 10/08/12
                                                    XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 10/08/12
                                                    If MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) IsNot Nothing Then ' TJS 10/08/12
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento CreateCatalogProduct failed for ItemCode " & ItemCode & " with error - " & MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value.Replace(vbCrLf, "")) ' TJS 10/08/12 TJS 13/11/13

                                                    Else
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento CreateCatalogProduct failed for ItemCode " & ItemCode & " - " & MagentoConnection.ReturnedData) ' TJS 10/08/12 TJS 13/11/13
                                                    End If

                                                End If
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionXMLFile_DEV000221 = MagentoConnection.PostedXML ' TJS 04/05/11
                                            End If
                                        Else
                                            m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending new Inventory Item updates to Magento") ' TJS 11/02/14
                                        End If
                                    Else
                                        ' yes, mark row as complete
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True
                                    End If
                                    ' end of code added TJS 25/04/11

                                ElseIf ImportExportStatusDataset.InventoryItem(0).ItemType = ITEM_TYPE_MATRIX_GROUP Then ' TJS 05/10/13

                                Else
                                    ' need add processing for Kits 

                                End If
                                ' start of code added TJS 10/03/12
                            Else
                                ' Magento Product ID is set, must have been imported via Import Wizard - mark row as complete
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True ' TJS 10/03/12 TJS 13/11/13

                                ' check for any new tag records and mark them as complete
                                iOrigRowID = RowID
                                For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveMagentoSettings.InstanceID Then
                                        If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Then
                                            ' new Tag value
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

                            ' start of code added TJS 13/11/13
                        Case "150" ' Publish Matrix Group Related Products
                            strTemp = "ItemType = 'Matrix Item' AND Publish_DEV000221 = 1 AND SourceIsGroupItem_DEV000221 = 0 AND InstanceID_DEV000221 = '" & ActiveMagentoSettings.InstanceID
                            strTemp = strTemp & "' AND MagentoProductID_DEV000221 IS NOT NULL AND ItemCode IN (SELECT MatrixItemCode FROM InventoryMatrixItem WHERE ItemCode = '" & ItemCode & "')"
                            strMatrixItems = m_ImportExportConfigFacade.GetRows(New String() {"ItemCode", "ItemName", "MagentoProductID_DEV000221"}, "InventoryItemMagentoSummaryView_DEV000221", strTemp, Interprise.Framework.Base.Shared.ConnectionStringType.Online)
                            If strMatrixItems IsNot Nothing AndAlso strMatrixItems.Length > 0 AndAlso Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsMagentoProductID_DEV000221Null AndAlso _
                                ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).MagentoProductID_DEV000221 <> "" Then
                                If Not bInhibitWebPosts Then ' TJS 11/02/14
                                    OpenMagentoInventoryConnection(ActiveSource, ActiveMagentoSettings)
                                    If MagentoConnection.LoggedIn Then
                                        ReDim strMatrixItemIDs(strMatrixItems.Length - 1)
                                        iCheckLoop = 0
                                        For Each strMatrixItem As String() In strMatrixItems
                                            strMatrixItemIDs(iCheckLoop) = strMatrixItem(2)
                                            iCheckLoop += 1
                                        Next
                                        If MagentoConnection.LinkConfigWithSimpleProducts(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).MagentoProductID_DEV000221, strMatrixItemIDs) Then
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

                                            ' check for any duplicate records and mark them as complete
                                            iOrigRowID = RowID
                                            For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveMagentoSettings.InstanceID Then
                                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "150" Then
                                                        ' duplicate record
                                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True

                                                    End If
                                                Else
                                                    ' set row pointer return value
                                                    RowID = iCheckLoop - 1
                                                    Exit For
                                                End If
                                                If bShutDownInProgress Then
                                                    Exit For
                                                End If
                                            Next
                                            ' did check loop exit without finding a new row for a different item ?
                                            If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then
                                                ' yes, set row pointer return value
                                                RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                            End If

                                        Else
                                            ' failed to link products
                                            XMLNameTabMagento = New System.Xml.NameTable
                                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                                            If MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) IsNot Nothing Then
                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento LinkConfigWithSimpleProducts failed for ItemCode " & ItemCode & " with error - " & MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value.Replace(vbCrLf, ""))

                                            Else
                                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento LinkConfigWithSimpleProducts failed for ItemCode " & ItemCode & " - " & MagentoConnection.ReturnedData)
                                            End If

                                        End If
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionXMLFile_DEV000221 = MagentoConnection.PostedXML
                                    End If
                                Else
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending Matrix Group Link Product updates to Magento") ' TJS 11/02/14
                                End If
                            Else
                                ' no matrix items found
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 09/12/13
                            End If
                                ' end of code added TJS 13/11/13

                        Case "200", "105", "205" ' Updated Item or New/Updated Magento Tag/Category value TJS 09/12/13
                                If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsMagentoProductID_DEV000221Null AndAlso _
                                    ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).MagentoProductID_DEV000221 <> "" Then ' TJS 29/01/14
                                    BuildProductCoreAttributeList(ImportExportStatusFacade, ImportExportStatusDataset, ProductAttributes, iAttributePtr, ItemCode) ' TJS 13/11/13

                                    ' start of code added TJS 25/04/11
                                    ' check for any new tag records or updated item/tag records and mark them as complete
                                    ' also check if a deleted item row exists
                                    bBeenDeleted = False
                                    iOrigRowID = RowID
                                    For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                        If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveMagentoSettings.InstanceID Then ' TJS 02/12/11
                                            If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Or _
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "205" Then
                                                ' new or updated Tag value
                                                strTemp = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).TableTagID_DEV000221
                                                If Left(strTemp, 10) = "InvMagTag-" Then
                                                    AddProductAttribute(ImportExportStatusFacade, ImportExportStatusDataset, ProductAttributes, iAttributePtr, CInt(strTemp.Substring(10))) ' TJS 13/11/13 TJS 29/01/14

                                                ElseIf Left(strTemp, 10) = "InvMagCat-" Then ' TJS 13/11/13
                                                    AddProductCategory(ImportExportStatusFacade, ImportExportStatusDataset, iProductCategoryList, iProductCategoryCount, iWebsiteList, iWebsiteCount, ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221, ItemCode, strTemp.Substring(10)) ' TJS 13/11/13 TJS 29/01/14

                                                End If
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts ' TJS 04/05/11

                                            ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "200" Then
                                                ' duplicated update item row - mark as complete
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts ' TJS 04/05/11

                                            ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "300" Then
                                                ' deleted item row, inhibit update to Magento
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts ' TJS 04/05/11
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
                                    If iCheckLoop = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count Then ' TJS 27/11/09
                                        ' yes, set row pointer return value
                                        RowID = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1 ' TJS 27/11/09
                                    End If
                                    ' end of code added TJS 25/04/11

                                    ' was a deleted item row found ?
                                    If Not bBeenDeleted Then ' TJS 25/04/11
                                        ' no, set message type and mark record as having XML to send
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLMessageType_DEV000221 = 11 ' 11 is UpdateCatalogProductDetail TJS 25/04/11 TJS 04/05/11
                                        If Not bInhibitWebPosts Then
                                            OpenMagentoInventoryConnection(ActiveSource, ActiveMagentoSettings)
                                            If MagentoConnection.LoggedIn Then
                                                ' start of code added TJS 13/11/13
                                                strCategoriesAndWebsites = ImportExportStatusFacade.GetRows(New String() {"MagentoCategoryID_DEV000221", "MagentoWebSiteID_DEV000221"}, "InventoryMagentoCategories_DEV000221", "ItemCode_DEV000221 = '" & _
                                                     ItemCode & "' AND InstanceID_DEV000221 = '" & ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).StoreMerchantID_DEV000221 & "'", _
                                                    Interprise.Framework.Base.Shared.ConnectionStringType.Online) ' TJS 13/11/13 TJS 09/12/13
                                                For Each strCategorOrWebsite In strCategoriesAndWebsites
                                                    ' is it a Category or a Website record ?
                                                    If strCategorOrWebsite(1) = "-1" Then
                                                        ' a Category record
                                                        bCategoryExists = False
                                                        For iloop = 0 To iProductCategoryCount - 1
                                                            If iProductCategoryList(iloop) = CInt(strCategorOrWebsite(0)) Then
                                                                bCategoryExists = True
                                                                Exit For
                                                            End If
                                                        Next
                                                        If Not bCategoryExists Then
                                                            iProductCategoryCount = iProductCategoryCount + 1
                                                            ReDim iProductCategoryList(iProductCategoryCount - 1)
                                                            iProductCategoryList(iProductCategoryCount - 1) = CInt(strCategorOrWebsite(0))
                                                        End If
                                                    Else
                                                        ' a Website record
                                                        bWebsiteExists = False
                                                        For iloop = 0 To iWebsiteCount - 1
                                                            If iWebsiteList(iloop) = CInt(strCategorOrWebsite(1)) Then
                                                                bWebsiteExists = True
                                                                Exit For
                                                            End If
                                                        Next
                                                        If Not bWebsiteExists Then
                                                            iWebsiteCount = iWebsiteCount + 1
                                                            ReDim iWebsiteList(iWebsiteCount - 1)
                                                            iWebsiteList(iWebsiteCount - 1) = CInt(strCategorOrWebsite(1))
                                                        End If
                                                    End If
                                                Next
                                                ' end of code added TJS 13/11/13
                                                If MagentoConnection.UpdateCatalogProductDetail(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).MagentoProductID_DEV000221, ProductAttributes, iProductCategoryList, iWebsiteList) Then ' TJS 22/08/12 TJS 13/11/13
                                                    ' post to Magento returned data, but was it a success ?
                                                    XMLNameTabMagento = New System.Xml.NameTable ' TJS 09/12/13
                                                    XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 09/12/13
                                                    XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 09/12/13
                                                    XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 09/12/13
                                                    If ActiveMagentoSettings.V2SoapAPIWSICompliant And MagentoConnection.V2SoapWSIAPIResponse Then ' TJS 09/12/13
                                                        strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductUpdateResponseParam/result", XMLNSManMagento) ' TJS 09/12/13
                                                    ElseIf MagentoConnection.V2SoapAPIResponse Then ' TJS 09/12/13
                                                        strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:catalogProductUpdateResponse/result", XMLNSManMagento) ' TJS 09/12/13
                                                    Else
                                                        strTemp = m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento) ' TJS 09/12/13
                                                    End If
                                                    If strTemp = "true" Then ' TJS 09/12/13
                                                        ' yes, mark row as complete
                                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 27/11/09 TJS 04/05/11
                                                    Else
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento UpdateCatalogProductDetail returned false, but no error details - response was " & MagentoConnection.ReturnedXML.ToString) ' TJS 09/12/13
                                                    End If

                                                Else
                                                    XMLNameTabMagento = New System.Xml.NameTable ' TJS 10/08/12
                                                    XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 10/08/12
                                                    If MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) IsNot Nothing Then ' TJS 10/08/12
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento UpdateCatalogProductDetail failed for ItemCode " & ItemCode & " with error - " & MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value.Replace(vbCrLf, "")) ' TJS 10/08/12 TJS 13/11/13

                                                    Else
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento UpdateCatalogProductDetail failed for ItemCode " & ItemCode & " - " & MagentoConnection.ReturnedData) ' TJS 10/08/12 TJS 13/11/13
                                                    End If

                                                End If
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionXMLFile_DEV000221 = MagentoConnection.PostedXML ' TJS 25/04/11 TJS 04/05/11
                                            End If
                                        Else
                                        m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending Inventory change updates to Magento") ' TJS 25/04/11 TJS 11/02/14
                                        End If
                                    Else
                                        ' yes, mark row as complete
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 25/04/11
                                    End If

                                Else
                                    ' Magento ID is empty so update row to be a new Item row
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221 = "100" ' TJS 29/01/14
                                End If

                        Case "205" ' Updated Magento Tag value

                        Case "300" ' Deleted Item

                                ' start of code added TJS 02/12/11
                        Case "1000" ' Stock Quantity changed
                                If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsMagentoProductID_DEV000221Null AndAlso _
                                    ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).MagentoProductID_DEV000221 <> "" Then ' TJS 29/01/14
                                    bQtyUpdateRequired = False
                                    ' get current total quantity available
                                    decItemQtyAvailable = CDec(ImportExportStatusFacade.GetField("SELECT ISNULL(SUM(UnitsAvailable), 0) AS TotalAvailable FROM InventoryStockTotal WHERE ItemCode = '" & ItemCode & "'", System.Data.CommandType.Text, Nothing))
                                    ' and values when last published
                                    If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsTotalQtyWhenLastPublished_DEV000221Null Then
                                        decItemTotalQtyWhenLastPublished = ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).TotalQtyWhenLastPublished_DEV000221
                                    Else
                                        decItemTotalQtyWhenLastPublished = 0
                                    End If
                                    If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null Then
                                        decItemQtyLastPublished = ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyLastPublished_DEV000221
                                    Else
                                        decItemQtyLastPublished = 0
                                    End If

                                    ' what is publishing basis ?
                                    If ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                                        iQtyToPublish = 0
                                    Else
                                        Select Case ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingType_DEV000221
                                            Case "Fixed"
                                                ' always send update as orders may have changed value on source site
                                                iQtyToPublish = CInt(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                                                If iQtyToPublish < 0 Then
                                                    iQtyToPublish = 0
                                                End If
                                                bQtyUpdateRequired = True

                                            Case "Percent"
                                                ' calculate value to be published based on percentage of available qty
                                                iQtyToPublish = CInt((decItemQtyAvailable * ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyPublishingValue_DEV000221) / 100) ' TJS 15/03/13
                                                If iQtyToPublish < 0 Then
                                                    iQtyToPublish = 0
                                                End If
                                                ' is value to publish positive and last value was null or 0 ?
                                                If (ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null OrElse _
                                                    ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyLastPublished_DEV000221 = 0) And iQtyToPublish > 0 Then
                                                    ' yes, send value
                                                    bQtyUpdateRequired = True

                                                ElseIf (Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                                    ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyLastPublished_DEV000221 > 0) And iQtyToPublish = 0 Then
                                                    ' no, quantity now 0 and was previously positive, send value
                                                    bQtyUpdateRequired = True

                                                ElseIf (Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                                    ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyLastPublished_DEV000221 < 10) Or iQtyToPublish < 10 Then
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
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 20 ' 20 is UpdateProductStockQty
                                        If Not bInhibitWebPosts Then
                                            OpenMagentoInventoryConnection(ActiveSource, ActiveMagentoSettings)
                                            If MagentoConnection.LoggedIn Then
                                                If iQtyToPublish > 0 Then
                                                    bIsInStock = True
                                                Else
                                                    bIsInStock = False
                                                End If
                                                If MagentoConnection.UpdateCatalogProductStockQty(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).MagentoProductID_DEV000221, iQtyToPublish, bIsInStock) Then
                                                    ' post to Magento returned data, but was it a success ?
                                                    XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                                                    XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                                                    XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                                                    XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                                                    If m_ImportExportConfigFacade.GetXMLElementText(MagentoConnection.ReturnedXML, "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento) = "true" Then ' TJS 09/12/13
                                                        ' yes, mark row as complete
                                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
                                                        ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).QtyLastPublished_DEV000221 = iQtyToPublish
                                                        ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).TotalQtyWhenLastPublished_DEV000221 = decItemQtyAvailable
                                                        ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.InventoryMagentoDetails_DEV000221.TableName, _
                                                            "CreateInventoryMagentoDetails_DEV000221", "UpdateInventoryMagentoDetails_DEV000221", "DeleteInventoryMagentoDetails_DEV000221"}}, _
                                                            Interprise.Framework.Base.Shared.TransactionType.None, "Update Lerryn Import/Export Inventory Magento Details", False)
                                                    Else
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento UpdateCatalogProductStockQty returned false, but no error details - response was " & MagentoConnection.ReturnedXML.ToString) ' TJS 09/12/13
                                                    End If

                                                Else
                                                    XMLNameTabMagento = New System.Xml.NameTable ' TJS 10/08/12
                                                    XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 10/08/12
                                                    XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 10/08/12
                                                    If MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) IsNot Nothing Then ' TJS 10/08/12
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento UpdateCatalogProductStockQty failed with error - " & MagentoConnection.ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value.Replace(vbCrLf, "")) ' TJS 10/08/12 TJS 13/11/13

                                                    Else
                                                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "Magento UpdateCatalogProductStockQty failed - " & MagentoConnection.ReturnedData) ' TJS 10/08/12 TJS 13/11/13
                                                    End If

                                                End If
                                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = MagentoConnection.PostedXML
                                            End If
                                        Else
                                        m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending Inventory Stock Qty updates to Magento") ' TJS 11/02/14
                                        End If
                                    Else
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
                                    End If
                                    ' check for any additional Stock Quantity changed records and mark them as complete
                                    bBeenDeleted = False
                                    iOrigRowID = RowID
                                    For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                        If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveMagentoSettings.InstanceID Then
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
                                    ' end of code added TJS 02/12/11
                                Else
                                    ' Magento ID is empty so mark row as complete
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 29/01/13
                                End If

                    End Select
                End If

            ElseIf ImportExportStatusDataset.InventoryItem.Count > 0 Then
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "No Inventory Item found for " & ItemCode)
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoMagentoInventoryUpdate", "No Inventory Magento Details found for " & ItemCode)
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoMagentoInventoryUpdate", ex)

        End Try

    End Sub

    Private Sub BuildProductCoreAttributeList(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef ProductAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType(), ByRef iAttributePtr As Integer, _
        ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added using code from DoMagentoInventoryUpdate
        '                                        | and corrected setting of name and short_description attributes
        '                                        | Corrected string constants used to indicate product name and 
        '                                        | description are taken from InventoryItem table
        ' 20/11/13 | TJS             | 2013.4.00 | Modified to cater for Web Option link
        ' 09/12/13 | TJS             | 2013.4.02 | Removed ConvertEntitiesForXML as Magento connector uses special version
        ' 15/01/14 | TJS             | 2013.4.05 | Modified to ensure AttributeValue not left as Nothing 
        ' 29/01/14 | TJS             | 2013.4.07 | Corrected detection of Special Prices coming from CB Promotional records
        '                                        | and modified to add missing core Magento attribute values
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSQL As String, strTemp As String, strPrices() As String, iPriceLoop As Integer ' TJS 20/11/13
        Dim iAttributeLoop As Integer ' TJS 29/01/13

        ReDim ProductAttributes(7) ' TJS 13/11/13
        iAttributePtr = 0
        ProductAttributes(iAttributePtr).AttributeName = "sku"
        ProductAttributes(iAttributePtr).AttributeType = "Text" ' TJS 04/05/11
        ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryItem(0).ItemName ' TJS 09/12/13
        iAttributePtr += 1

        ProductAttributes(iAttributePtr).AttributeName = "name"
        ProductAttributes(iAttributePtr).AttributeType = "Text" ' TJS 04/05/11
        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductName_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductName_DEV000221 <> INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
            ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductName_DEV000221 ' TJS 09/12/13
        ElseIf Not ImportExportStatusDataset.InventoryItemDescription(0).IsItemDescriptionNull Then ' TJS 13/11/13
            ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryItemDescription(0).ItemDescription ' TJS 09/12/13
        Else
            ProductAttributes(iAttributePtr).AttributeValue = "" ' TJS 13/11/13
        End If
        iAttributePtr += 1

        ' start of code added TJS 13/11/13
        ProductAttributes(iAttributePtr).AttributeName = "short_description"
        ProductAttributes(iAttributePtr).AttributeType = "Memo"
        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductShortDescription_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductShortDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then ' TJS 13/11/13 TJS 20/11/13
            If Not ImportExportStatusDataset.InventoryItemDescription(0).IsExtendedDescriptionNull Then ' TJS 13/11/13 TJS 20/11/13
                ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryItemDescription(0).ExtendedDescription ' TJS 09/12/13
            Else
                ProductAttributes(iAttributePtr).AttributeValue = "" ' TJS 20/11/13
            End If
            ' start of code added TJS 20/11/13
        ElseIf Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductShortDescription_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductShortDescription_DEV000221 = INVENTORY_USE_WEB_OPTION_TAB_SUMMARY Then
            strSQL = "ItemCode = '" & ItemCode & "' AND LanguageCode = '" & ImportExportStatusFacade.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
            strSQL = strSQL & "' AND WebSiteCode = '" & ImportExportStatusFacade.GetField("WebSiteCode", "EcommerceSite", "IsDefault = 1") & "'"
            strTemp = ImportExportStatusFacade.GetField("Summary", "InventoryItemWebOptionDescription", strSQL)
            If Not String.IsNullOrEmpty(strTemp) Then
                ProductAttributes(iAttributePtr).AttributeValue = strTemp ' TJS 09/12/13
            Else
                ProductAttributes(iAttributePtr).AttributeValue = "" ' TJS 15/01/14
            End If

        ElseIf Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductShortDescription_DEV000221Null Then
            ' end of code added TJS 20/11/13
            ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductShortDescription_DEV000221 ' TJS 09/12/13
        Else
            ProductAttributes(iAttributePtr).AttributeValue = ""
        End If
        iAttributePtr += 1
        ' end of code added TJS 13/11/13

        ProductAttributes(iAttributePtr).AttributeName = "description"
        ProductAttributes(iAttributePtr).AttributeType = "Memo" ' TJS 04/05/11
        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductDescription_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductDescription_DEV000221 = INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then ' TJS 20/11/13
            ' start of code added TJS 20/11/13
            If Not ImportExportStatusDataset.InventoryItemDescription(0).IsExtendedDescriptionNull Then
                ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryItemDescription(0).ExtendedDescription ' TJS 09/12/13
            Else
                ProductAttributes(iAttributePtr).AttributeValue = ""
            End If

        ElseIf Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductDescription_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductDescription_DEV000221 = INVENTORY_USE_WEB_OPTION_TAB_DESCRIPTION Then
            strSQL = "ItemCode = '" & ItemCode & "' AND LanguageCode = '" & ImportExportStatusFacade.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)
            strSQL = strSQL & "' AND WebSiteCode = '" & ImportExportStatusFacade.GetField("WebSiteCode", "EcommerceSite", "IsDefault = 1") & "'"
            strTemp = ImportExportStatusFacade.GetField("WebDescription", "InventoryItemWebOptionDescription", strSQL)
            If Not String.IsNullOrEmpty(strTemp) Then
                ProductAttributes(iAttributePtr).AttributeValue = strTemp ' TJS 09/12/13
            Else
                ProductAttributes(iAttributePtr).AttributeValue = "" ' TJS 15/01/14
            End If

        ElseIf Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductDescription_DEV000221Null Then
            ' end of code added TJS 20/11/13 
            ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductDescription_DEV000221 ' TJS 09/12/13
        Else
            ProductAttributes(iAttributePtr).AttributeValue = ""
        End If
        iAttributePtr += 1

        ProductAttributes(iAttributePtr).AttributeName = "in_depth"
        ProductAttributes(iAttributePtr).AttributeType = "Memo" ' TJS 04/05/11
        If ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsProductInDepthDescription_DEV000221Null Then
            ProductAttributes(iAttributePtr).AttributeValue = ""
        Else
            ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ProductInDepthDescription_DEV000221 ' TJS 09/12/13
        End If
        iAttributePtr += 1

        ProductAttributes(iAttributePtr).AttributeName = "price"
        ProductAttributes(iAttributePtr).AttributeType = "Numeric" ' TJS 04/05/11
        ' start of code added TJS 13/11/13
        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsUseISPricingDetail_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).UseISPricingDetail_DEV000221 Then
            strSQL = "ItemCode = '" & ItemCode & "' and CurrencyCode = '" & ImportExportStatusFacade.GetField("CurrencyCode", "SystemCompanyInformation", Nothing) & "'"
            strPrices = ImportExportStatusFacade.GetRow(New String() {"WholesalePriceRate", "RetailPriceRate", "SuggestedRetailPriceRate"}, "InventoryItemPricingDetail", strSQL)
            Select Case ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SellingPriceSource_DEV000221
                Case "W"
                    ProductAttributes(iAttributePtr).AttributeValue = Format(CDec(strPrices(0)), "0.00")
                Case "R"
                    ProductAttributes(iAttributePtr).AttributeValue = Format(CDec(strPrices(1)), "0.00")
                Case "S"
                    ProductAttributes(iAttributePtr).AttributeValue = Format(CDec(strPrices(2)), "0.00")
                Case Else
                    ProductAttributes(iAttributePtr).AttributeValue = "0.00"
            End Select
            ' end of code added TJS 13/11/13
        Else
            If ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsSellingPrice_DEV000221Null Then
                ProductAttributes(iAttributePtr).AttributeValue = "0.00"
            Else
                ProductAttributes(iAttributePtr).AttributeValue = Format(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SellingPrice_DEV000221, "0.00")
            End If
        End If
        iAttributePtr += 1

        ProductAttributes(iAttributePtr).AttributeName = "visibility"
        ProductAttributes(iAttributePtr).AttributeType = "Text" ' TJS 04/05/11
        If ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsVisibility_DEV000221Null Then
            ProductAttributes(iAttributePtr).AttributeValue = ""
        Else
            ProductAttributes(iAttributePtr).AttributeValue = CStr(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).Visibility_DEV000221) ' TJS 09/12/13
        End If
        iAttributePtr += 1

        ProductAttributes(iAttributePtr).AttributeName = "status"
        ProductAttributes(iAttributePtr).AttributeType = "Text" ' TJS 04/05/11
        If ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsStatus_DEV000221Null Then
            ProductAttributes(iAttributePtr).AttributeValue = ""
        Else
            ProductAttributes(iAttributePtr).AttributeValue = CStr(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).Status_DEV000221) ' TJS 09/12/13
        End If
        iAttributePtr += 1

        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsNewFrom_DEV000221Null Then
            ReDim Preserve ProductAttributes(ProductAttributes.Length) ' FA 21/06/13
            ProductAttributes(iAttributePtr).AttributeName = "news_from_date"
            ProductAttributes(iAttributePtr).AttributeType = "Date" ' TJS 04/05/11
            ProductAttributes(iAttributePtr).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewFrom_DEV000221)
            iAttributePtr += 1
        End If

        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsNewTo_DEV000221Null Then
            ReDim Preserve ProductAttributes(ProductAttributes.Length)
            ProductAttributes(iAttributePtr).AttributeName = "news_to_date"
            ProductAttributes(iAttributePtr).AttributeType = "Date" ' TJS 04/05/11
            ProductAttributes(iAttributePtr).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ShowAsNewTo_DEV000221)
            iAttributePtr += 1
        End If

        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null AndAlso _
            (ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsSpecialPriceSource_DEV000221Null OrElse _
             ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SpecialPriceSource_DEV000221 <> "P") Then ' TJS 13/11/13 TJS 29/01/14
            ReDim Preserve ProductAttributes(ProductAttributes.Length)
            ProductAttributes(iAttributePtr).AttributeName = "special_from_date"
            ProductAttributes(iAttributePtr).AttributeType = "Date" ' TJS 04/05/11
            ProductAttributes(iAttributePtr).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221)
            iAttributePtr += 1
        End If

        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null AndAlso _
            (ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsSpecialPriceSource_DEV000221Null OrElse _
             ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SpecialPriceSource_DEV000221 <> "P") Then ' TJS 13/11/13 TJS 29/01/14
            ReDim Preserve ProductAttributes(ProductAttributes.Length)
            ProductAttributes(iAttributePtr).AttributeName = "special_to_date"
            ProductAttributes(iAttributePtr).AttributeType = "Date" ' TJS 04/05/11
            ProductAttributes(iAttributePtr).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221)
            iAttributePtr += 1
        End If

        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null Or _
            Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null Or _
            (Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsSpecialPriceSource_DEV000221Null AndAlso _
            ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SpecialPriceSource_DEV000221 = "P") Then ' TJS 13/11/13
            ' start of code added TJS 13/11/13
            If ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).UseISPricingDetail_DEV000221 Then
                strSQL = "ItemCode = '" & ItemCode & "' and CurrencyCode = '" & ImportExportStatusFacade.GetField("CurrencyCode", "SystemCompanyInformation", Nothing) & "'"
                strPrices = ImportExportStatusFacade.GetRow(New String() {"WholesalePriceRate", "RetailPriceRate"}, "InventoryItemPricingDetail", strSQL)
                Select Case ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SpecialPriceSource_DEV000221
                    Case "W"
                        ReDim Preserve ProductAttributes(ProductAttributes.Length)
                        ProductAttributes(iAttributePtr).AttributeName = "special_price"
                        ProductAttributes(iAttributePtr).AttributeType = "Numeric" ' TJS 04/05/11
                        ProductAttributes(iAttributePtr).AttributeValue = Format(CDec(strPrices(0)), "0.00")
                        iAttributePtr += 1

                    Case "R"
                        ReDim Preserve ProductAttributes(ProductAttributes.Length)
                        ProductAttributes(iAttributePtr).AttributeName = "special_price"
                        ProductAttributes(iAttributePtr).AttributeType = "Numeric" ' TJS 04/05/11
                        ProductAttributes(iAttributePtr).AttributeValue = Format(CDec(strPrices(1)), "0.00")
                        iAttributePtr += 1

                    Case "P"
                        For iPriceLoop = 0 To ImportExportStatusDataset.InventorySpecialPricing.Count - 1
                            If (ImportExportStatusDataset.InventorySpecialPricing(iPriceLoop).DateFrom <= Date.Today And _
                                ImportExportStatusDataset.InventorySpecialPricing(iPriceLoop).DateTo >= Date.Today) Or _
                                ImportExportStatusDataset.InventorySpecialPricing(iPriceLoop).DateFrom > Date.Today And _
                                ImportExportStatusDataset.InventorySpecialPricing(iPriceLoop).UnitMeasureCode = "EACH" Then
                                ProductAttributes(iAttributePtr).AttributeValue = Format(CDec(ImportExportStatusDataset.InventorySpecialPricing(iPriceLoop).SpecialPrice), "0.00")
                                iAttributePtr += 1
                                ReDim Preserve ProductAttributes(ProductAttributes.Length + 1)
                                ProductAttributes(iAttributePtr).AttributeName = "special_from_date"
                                ProductAttributes(iAttributePtr).AttributeType = "Date"
                                ProductAttributes(iAttributePtr).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventorySpecialPricing(iPriceLoop).DateFrom)
                                iAttributePtr += 1
                                ProductAttributes(iAttributePtr).AttributeName = "special_to_date"
                                ProductAttributes(iAttributePtr).AttributeType = "Date"
                                ProductAttributes(iAttributePtr).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventorySpecialPricing(iPriceLoop).DateTo)
                                iAttributePtr += 1
                                Exit For
                            End If
                        Next

                    Case Else
                        If Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null Or _
                            Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null Then
                            ReDim Preserve ProductAttributes(ProductAttributes.Length)
                            ProductAttributes(iAttributePtr).AttributeName = "special_price"
                            ProductAttributes(iAttributePtr).AttributeType = "Numeric" ' TJS 04/05/11
                            ProductAttributes(iAttributePtr).AttributeValue = Format(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221, "0.00")
                            iAttributePtr += 1
                        End If
                End Select

            ElseIf Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null Or _
                Not ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null Then
                ReDim Preserve ProductAttributes(ProductAttributes.Length)
                ProductAttributes(iAttributePtr).AttributeName = "special_price"
                ProductAttributes(iAttributePtr).AttributeType = "Numeric"
                ' end of code added TJS 13/11/13
                ProductAttributes(iAttributePtr).AttributeValue = Format(ImportExportStatusDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221, "0.00")
                iAttributePtr += 1
            End If
        End If

        ' start of code added TJS 29/01/14
        For iAttributeLoop = 0 To ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221.Count - 1
            strTemp = ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).TagName_DEV000221
            If strTemp = "weight" Or strTemp = "url_key" Or strTemp = "url_path" Or strTemp = "gift_message_available" Or strTemp = "tax_class_id" Or _
                strTemp = "meta_title" Or strTemp = "meta_keyword" Or strTemp = "meta_description" Or strTemp = "custom_design" Or _
                strTemp = "custom_layout_update" Or strTemp = "options_container" Then
                AddProductAttribute(ImportExportStatusFacade, ImportExportStatusDataset, ProductAttributes, iAttributePtr, ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).Counter)
            End If
        Next
        ' end of code added TJS 29/01/14

    End Sub

    Private Sub AddProductAttribute(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef ProductAttributes As Lerryn.Facade.ImportExport.MagentoSOAPConnector.ProductAttributeType(), _
        ByRef iAttributePtr As Integer, ByVal CounterValue As Integer) ' TJS 29/01/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added using code from DoMagentoInventoryUpdate
        ' 29/01/14 | TJS             | 2013.4.07 | Modified to use data already in InventoryMagentoTagDetails_DEV000221 dataset
        ' 01/05/14 | TJS             | 2014.0.02 | Corrected error when updating existing attribute entry
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iAttributeLoop As Integer, iLoop As Integer ' TJS 29/01/14
        Dim bAddTagValue As Boolean

        For iLoop = 0 To ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221.Count - 1 ' TJS 29/01/14
            If ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).Counter = CounterValue Then ' TJS 29/01/14
                bAddTagValue = False
                Select Case ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDataType_DEV000221 ' TJS 29/01/14
                    Case "Memo"
                        If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagMemoValue_DEV000221Null AndAlso _
                            ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221 <> "" Then ' TJS 29/01/14
                            bAddTagValue = True
                        End If
                    Case "Integer", "Numeric" ' TJS 13/11/13
                        If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagNumericValue_DEV000221Null Then ' TJS 29/01/14
                            bAddTagValue = True
                        End If
                    Case "Date", "DateTime"
                        If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagDateValue_DEV000221Null Then ' TJS 29/01/14
                            bAddTagValue = True
                        End If
                    Case Else
                        If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagTextValue_DEV000221Null AndAlso _
                            ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagTextValue_DEV000221 <> "" Then ' TJS 29/01/14
                            bAddTagValue = True
                        End If
                End Select
                ' start of code added TJS 04/05/11
                If bAddTagValue Then
                    For iAttributeLoop = 0 To ProductAttributes.Length - 1
                        If ProductAttributes(iAttributeLoop).AttributeName = ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagName_DEV000221 Then ' TJS 29/01/14
                            Select Case ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDataType_DEV000221 ' TJS 29/01/14
                                Case "Memo"
                                    If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagMemoValue_DEV000221Null AndAlso _
                                        ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221 <> "" Then ' TJS 02/12/11 TJS 29/01/14
                                        ProductAttributes(iAttributeLoop).AttributeValue = ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221) ' TJS 29/01/14 TJS 01/05/14
                                    End If
                                Case "Integer", "Numeric" ' TJS 13/11/13
                                    If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagNumericValue_DEV000221Null Then ' TJS 02/12/11 TJS 29/01/14
                                        ProductAttributes(iAttributeLoop).AttributeValue = ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagNumericValue_DEV000221.ToString ' TJS 29/01/14 TJS 01/05/14
                                    End If
                                Case "Date", "DateTime"
                                    If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagDateValue_DEV000221Null Then ' TJS 02/12/11 TJS 29/01/14
                                        ProductAttributes(iAttributeLoop).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDateValue_DEV000221) ' TJS 29/01/14 TJS 01/05/14
                                    End If
                                Case Else
                                    If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagTextValue_DEV000221Null AndAlso _
                                        ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagTextValue_DEV000221 <> "" Then ' TJS 02/12/11 TJS 29/01/14
                                        ProductAttributes(iAttributeLoop).AttributeValue = ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagTextValue_DEV000221) ' TJS 29/01/14 TJS 01/05/14
                                    End If
                            End Select
                            bAddTagValue = False
                        End If
                    Next
                End If
                ' end of code added TJS 04/05/11
                If bAddTagValue Then
                    ReDim Preserve ProductAttributes(ProductAttributes.Length)
                    ProductAttributes(iAttributePtr).AttributeName = ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagName_DEV000221 ' TJS 29/01/14
                    ProductAttributes(iAttributePtr).AttributeType = ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDataType_DEV000221 ' TJS 04/05/11 TJS 29/01/14
                    Select Case ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDataType_DEV000221 ' TJS 29/01/14
                        Case "Memo"
                            If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagMemoValue_DEV000221Null AndAlso _
                                ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221 <> "" Then ' TJS 02/12/11 TJS 29/01/14
                                ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagMemoValue_DEV000221) ' TJS 29/01/14
                            End If
                        Case "Integer", "Numeric" ' TJS 13/11/13
                            If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagNumericValue_DEV000221Null Then ' TJS 02/12/11 TJS 29/01/14
                                ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagNumericValue_DEV000221.ToString ' TJS 29/01/14
                            End If
                        Case "Date", "DateTime"
                            If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagDateValue_DEV000221Null Then ' TJS 02/12/11 TJS 29/01/14
                                ProductAttributes(iAttributePtr).AttributeValue = MagentoXMLDate(ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagDateValue_DEV000221) ' TJS 29/01/14
                            End If
                        Case Else
                            If Not ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagTextValue_DEV000221Null AndAlso _
                                ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagTextValue_DEV000221 <> "" Then ' TJS 02/12/11 TJS 29/01/14
                                ProductAttributes(iAttributePtr).AttributeValue = ImportExportStatusFacade.ConvertEntitiesForXML(ImportExportStatusDataset.InventoryMagentoTagDetails_DEV000221(iLoop).TagTextValue_DEV000221) ' TJS 29/01/14
                            End If
                    End Select
                    iAttributePtr += 1
                End If
                Exit For ' TJS 29/01/14
            End If
        Next

    End Sub

    Private Sub AddProductCategory(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef iProductCategoryList() As Integer, ByRef iProductCategoryCount As Integer, ByRef iWebsiteList() As Integer, _
        ByRef iWebsiteCount As Integer, ByVal StoreMerchantID As String, ByVal ItemCode As String, ByVal CounterValue As String) ' TJS 29/01/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added using code from DoMagentoInventoryUpdate
        ' 29/01/14 | TJS             | 2013.4.07 | Modified to enable calling from BuildProductCoreAttributeList
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strTagDetails As String()
        Dim iloop As Integer, bCategoryExists As Boolean, bWebsiteExists As Boolean

        ' start of code added TJS 22/08/12
        strTagDetails = ImportExportStatusFacade.GetRow(New String() {"MagentoCategoryID_DEV000221", "MagentoWebSiteID_DEV000221"}, "InventoryMagentoCategories_DEV000221", _
            "ItemCode_DEV000221 = '" & ItemCode & "' AND InstanceID_DEV000221 = '" & StoreMerchantID & "' AND Counter = " & CounterValue) ' TJS 13/11/13 TJS 29/01/14
        ' is it a Category or a Website record ?
        If strTagDetails(1) = "-1" Then ' TJS 13/11/13
            ' a Category record
            bCategoryExists = False
            For iloop = 0 To iProductCategoryCount - 1
                If iProductCategoryList(iloop) = CInt(strTagDetails(0)) Then
                    bCategoryExists = True
                    Exit For
                End If
            Next
            If Not bCategoryExists Then
                iProductCategoryCount = iProductCategoryCount + 1
                ReDim iProductCategoryList(iProductCategoryCount - 1)
                iProductCategoryList(iProductCategoryCount - 1) = CInt(strTagDetails(0))
            End If
            ' start of code added TJS 13/11/13
        Else
            ' a Website record
            bWebsiteExists = False
            For iloop = 0 To iWebsiteCount - 1
                If iWebsiteList(iloop) = CInt(strTagDetails(1)) Then
                    bWebsiteExists = True
                    Exit For
                End If
            Next
            If Not bWebsiteExists Then
                iWebsiteCount = iWebsiteCount + 1
                ReDim iWebsiteList(iWebsiteCount - 1)
                iWebsiteList(iWebsiteCount - 1) = CInt(strTagDetails(1))
            End If
            ' end of code added TJS 13/11/13
        End If
        ' end of code added TJS 22/08/12

    End Sub

    Public Sub UpdateMagentoSpecialPrices(ByRef ActiveSource As SourceSettings, ByRef ChangesMade As Boolean, ByRef ErrorDetails As String) ' TJS 23/01/14
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ' 23/01/14 | TJS             | 2013.4.06 | Modified to return parameter indicating if any changes were made
        '                                        | and error details if a record won't save
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strInventoryItemsToUpdate As String()(), strInventoryItem As String()
        Dim strSQL As String, strDailyTasksData As String, strLastInventoryItem As String, strTemp As String
        Dim bPromoPriceActive As Boolean

        ChangesMade = False ' TJS 23/01/14
        ErrorDetails = "" ' TJS 23/01/14
        ' loop through the Magento web site list
        For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
            ' has an API URL been set and account not disabled ?
            If ActiveSource.MagentoSettings(iMerchantLoop).APIURL <> "" And Not ActiveSource.MagentoSettings(iMerchantLoop).AccountDisabled Then
                ' find all special pricing records which started or ceased between when we last ran the daily tasks and tomorrow or were changed since we last the daily tasks
                strSQL = "Publish_DEV000221 = 1 AND InstanceID_DEV000221 = '" & ActiveSource.MagentoSettings(iMerchantLoop).InstanceID.Replace("'", "''") & "' AND UseISPricingDetail_DEV000221 = 1"
                strSQL = strSQL & " AND ItemCode_DEV000221 IN (SELECT ItemCode FROM InventorySpecialPricing WHERE (DateFrom > '" & ActiveSource.MagentoSettings(iMerchantLoop).LastDailyTasksRun.ToString("MM/dd/yyyy")
                strSQL = strSQL & "' AND DateFrom < '" & Date.Today.AddDays(1).ToString("MM/dd/yyyy") & "') OR (DateTo > '" & ActiveSource.MagentoSettings(iMerchantLoop).LastDailyTasksRun.ToString("MM/dd/yyyy")
                strSQL = strSQL & "' AND DateTo < '" & Date.Today.AddDays(1).ToString("MM/dd/yyyy") & "') OR (DateModified > '" & ActiveSource.MagentoSettings(iMerchantLoop).LastDailyTasksRun.ToString("MM/dd/yyyy")
                strSQL = strSQL & "' AND (DateFrom < '" & Date.Today.AddDays(1).ToString("MM/dd/yyyy") & "' OR DateTo > '" & Date.Today.AddDays(1).ToString("MM/dd/yyyy") & "')))"
                strInventoryItemsToUpdate = m_ImportExportConfigFacade.GetRows(New String() {"ItemCode_DEV000221"}, "InventoryMagentoDetails_DEV000221", strSQL, Interprise.Framework.Base.Shared.ConnectionStringType.Online)
                strLastInventoryItem = ""
                bPromoPriceActive = False
                ' process each inventory item in turn
                For Each strInventoryItem In strInventoryItemsToUpdate
                    ' get all special price records and Magento details for Inventory Item
                    m_ImportExportConfigFacade.LoadDataSet(New String()() {New String() {m_ImportExportDataset.InventorySpecialPricing.TableName, "ReadInventorySpecialPricing_DEV000221", AT_ITEM_CODE, strInventoryItem(0)}, _
                        New String() {m_ImportExportDataset.InventoryMagentoDetails_DEV000221.TableName, "ReadInventoryMagentoDetails_DEV000221", AT_ITEMCODE, strInventoryItem(0), _
                            AT_INSTANCE_ID, ActiveSource.MagentoSettings(iMerchantLoop).InstanceID}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                    ' now check each item until we find first inactive record
                    For iLoop = 0 To m_ImportExportDataset.InventorySpecialPricing.Count - 1
                        ' is special price record active ?
                        If m_ImportExportDataset.InventorySpecialPricing(iLoop).DateFrom <= Date.Today And m_ImportExportDataset.InventorySpecialPricing(iLoop).DateTo >= Date.Today Then
                            ' yes, have we already found an active record ?
                            If bPromoPriceActive Then
                                ' yes, send error message
                                strTemp = m_ImportExportConfigFacade.GetField("ItemName", "InventoryItem", "ItemCode = '" & strInventoryItem(0) & "'")
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "UpdateMagentoSpecialPrices", "Overlapping Special Price records found for Inventory Item " & strTemp)
                                Exit For
                            End If
                            ' are special price record details different from Magento settings ?
                            If (m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null OrElse _
                                m_ImportExportDataset.InventorySpecialPricing(iLoop).DateFrom <> m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221) OrElse _
                                (m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null OrElse _
                                m_ImportExportDataset.InventorySpecialPricing(iLoop).DateTo <> m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221) OrElse _
                                (m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).IsSpecialPrice_DEV000221Null <> m_ImportExportDataset.InventorySpecialPricing(iLoop).IsSpecialPriceNull OrElse _
                                m_ImportExportDataset.InventorySpecialPricing(iLoop).SpecialPrice <> m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221) Then
                                ' yes, update them
                                If m_ImportExportDataset.InventorySpecialPricing(iLoop).IsSpecialPriceNull Then
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SetShowAsSpecialFrom_DEV000221Null()
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SetShowAsSpecialTo_DEV000221Null()
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SetSpecialPrice_DEV000221Null()
                                    ChangesMade = True ' TJS 23/01/14
                                Else
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221 = m_ImportExportDataset.InventorySpecialPricing(iLoop).DateFrom
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221 = m_ImportExportDataset.InventorySpecialPricing(iLoop).DateTo
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221 = m_ImportExportDataset.InventorySpecialPricing(iLoop).SpecialPrice
                                    ChangesMade = True ' TJS 23/01/14
                                End If
                            End If

                        Else
                            ' special price not active, is Magento special price active ?
                            If Not m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialFrom_DEV000221Null AndAlso _
                                m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221 <= Date.Today AndAlso _
                                Not m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).IsSpecialPrice_DEV000221Null AndAlso _
                                (m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).IsShowAsSpecialTo_DEV000221Null OrElse _
                                 m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221 >= Date.Today) Then
                                ' yes, update it
                                If m_ImportExportDataset.InventorySpecialPricing(iLoop).IsSpecialPriceNull Then
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SetShowAsSpecialFrom_DEV000221Null()
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SetShowAsSpecialTo_DEV000221Null()
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SetSpecialPrice_DEV000221Null()
                                    ChangesMade = True ' TJS 23/01/14
                                Else
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialFrom_DEV000221 = m_ImportExportDataset.InventorySpecialPricing(iLoop).DateFrom
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).ShowAsSpecialTo_DEV000221 = m_ImportExportDataset.InventorySpecialPricing(iLoop).DateTo
                                    m_ImportExportDataset.InventoryMagentoDetails_DEV000221(0).SpecialPrice_DEV000221 = m_ImportExportDataset.InventorySpecialPricing(iLoop).SpecialPrice
                                    ChangesMade = True ' TJS 23/01/14
                                End If
                            End If
                            ' don't need to check any older records
                            Exit For
                        End If
                    Next
                    If Not m_ImportExportConfigFacade.UpdateDataSet(New String()() {New String() {m_ImportExportDataset.InventoryMagentoDetails_DEV000221.TableName, _
                        "CreateInventoryMagentoDetails_DEV000221", "UpdateInventoryMagentoDetails_DEV000221", "DeleteInventoryMagentoDetails_DEV000221"}}, _
                            Interprise.Framework.Base.Shared.TransactionType.None, "Update Magento Special Pricing", False) Then ' TJS 23/01/14
                        ' start of code added TJS 23/01/14
                        strTemp = ""
                        For iRowLoop = 0 To m_ImportExportDataset.InventoryMagentoDetails_DEV000221.Rows.Count - 1
                            For iColumnLoop = 0 To m_ImportExportDataset.InventoryMagentoDetails_DEV000221.Columns.Count - 1
                                If m_ImportExportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                    If strTemp <> "" Then
                                        strTemp = strTemp & ", "
                                    End If
                                    strTemp = strTemp & m_ImportExportDataset.InventoryMagentoDetails_DEV000221.TableName & _
                                        "." & m_ImportExportDataset.InventoryMagentoDetails_DEV000221.Columns(iColumnLoop).ColumnName & " - " & _
                                        m_ImportExportDataset.InventoryMagentoDetails_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop)
                                End If
                            Next
                        Next
                        ErrorDetails = ErrorDetails & "Unable to save Magento Special Price update for " & strInventoryItem(0) & " due to errors in field(s) " & strTemp & vbCrLf
                        ' end of code added TJS 23/01/14
                    End If
                Next
                ActiveSource.MagentoSettings(iMerchantLoop).LastDailyTasksRun = Date.Today
            End If
        Next

        strDailyTasksData = "<eShopCONNECTConfig>"
        For iMerchantLoop = 0 To ActiveSource.MagentoSettingCount - 1
            strDailyTasksData = strDailyTasksData & "<Magento><InstanceID>" & ActiveSource.MagentoSettings(iMerchantLoop).InstanceID & "</InstanceID>"
            strDailyTasksData = strDailyTasksData & "<LastDailyTasksRun>" & CreateXMLDate(ActiveSource.MagentoSettings(iMerchantLoop).LastDailyTasksRun)
            strDailyTasksData = strDailyTasksData & "</LastDailyTasksRun></Magento>"
        Next
        strDailyTasksData = strDailyTasksData & "</eShopCONNECTConfig>"
        m_ImportExportConfigFacade.ExecuteNonQuery(CommandType.Text, "UPDATE LerrynImportExportConfig_DEV000221 SET LastDailyTasksRun_DEV000221 = '" & _
            strDailyTasksData & "' WHERE SourceCode_DEV000221 = '" & ActiveSource.SourceCode & "'", Nothing)


    End Sub

    Private Sub OpenMagentoInventoryConnection(ByVal ActiveSource As SourceSettings, ByVal ActiveMagentoSettings As MagentoSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Corrected function name in error messages and set bMagentoInventoryUpdateConnected
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to cater for Magento login potentially containing XML entities
        ' 15/04/13 | TJS             | 2013.1.10 | Removed setting of bMagentoInventoryUpdateConnected as replaced by a function
        ' 30/09/13 | TJS             | 2013.3.02 | Modified to decode Config XML Value such as password etc 
        ' 02/10/13 | TJS             | 2013.3.03 | Moved decoding of Congif XML values to ImportExportRule LoadXMLConfig
        ' 05/10/13 | TJS             | 2013.3.03 | Modified to cater for Magento V2SoapAPIWSICompliant and to cater for multiple API connections
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            If MagentoConnection Is Nothing Then
                MagentoConnection = New Lerryn.Facade.ImportExport.MagentoSOAPConnector()
            End If
            If Not MagentoConnection.LoggedIn OrElse MagentoConnection.APIURL <> ActiveMagentoSettings.APIURL Then ' TJS 05/10/13
                If MagentoConnection.LoggedIn Then ' TJS 05/10/13
                    MagentoConnection.Logout() ' TJS 05/10/13
                End If
                MagentoConnection.V2SoapAPIWSICompliant = ActiveMagentoSettings.V2SoapAPIWSICompliant ' TJS 05/10/13
                MagentoConnection.MagentoVersion = ActiveMagentoSettings.MagentoVersion ' TJS 13/11/13
                MagentoConnection.LerrynAPIVersion = ActiveMagentoSettings.LerrynAPIVersion ' TJS 13/11/13
                If MagentoConnection.Login(ActiveMagentoSettings.APIURL, m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveMagentoSettings.APIUser), _
                    m_ImportExportConfigFacade.ConvertEntitiesForXML(ActiveMagentoSettings.APIPwd)) Then ' TJS 03/04/13 TJS 30/09/13 TJS 02/10/13
                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "OpenMagentoInventoryConnection", MagentoConnection.LastError, "") ' TJS 02/12/11
                    MagentoConnection = Nothing ' TJS 02/12/11
                End If
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "OpenMagentoInventoryConnection", ex) ' TJS 02/12/11

        End Try

    End Sub

    Public Sub CloseMagentoInventoryConnection(ByVal ActiveSource As SourceSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Corrected function name in error messages
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to clear connected flag
        ' 15/04/13 | TJS             | 2013.1.10 | Removed setting of bMagentoInventoryUpdateConnected as replaced by a function
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            MagentoConnection.Logout()
            MagentoConnection = Nothing

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "CloseMagentoInventoryConnection", ex) ' TJS 02/12/11

        End Try

    End Sub

    Friend Function MagentoInventoryUpdateConnected() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 15/04/13 | TJS             | 2013.1.10 | Function added to replace bMagentoInventoryUpdateConnected 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If MagentoConnection IsNot Nothing Then
            Return MagentoConnection.LoggedIn
        Else
            Return False
        End If

    End Function

End Module
