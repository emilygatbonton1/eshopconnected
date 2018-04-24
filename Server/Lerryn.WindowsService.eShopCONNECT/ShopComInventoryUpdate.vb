' eShopCONNECT for Connected Business - Windows Service
' Module: ShopComInventoryUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 13 November 2013

Imports System.IO
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module ShopComInventoryUpdate

    Private XMLInventoryUpdateFile As XDocument
    Private xmlInventoryUpdateNode As XElement
    Private xmlProductNode As XElement
    Private xmlProductContainerNode As XElement
    Public bShopComInventoryUpdatesToSend As Boolean

    Public Sub DoShopComInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveShopComSettings As ShopComSettings, ByRef RowID As Integer, ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | TJS             | 2009.2.10 | Function started
        ' 18/06/09 | TJS             | 2009.3.00 | function completed
        ' 28/07/09 | TJS             | 2009.3.03 | Corrected error when multiple inventory action records exist
        ' 20/10/09 | TJS             | 2009.3.09 | Modified to remove check for Null StoreMerchantID as field does not allows nulls
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected error message call to remove product name
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strLastMatrixGroupItem As String, iLoop As Integer, iCheckLoop As Integer
        Dim iStartCheckRowID As Integer, bMatrixItemError As Boolean ' TJS 28/07/09

        Try
            ' check item record exists ?
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
               "ReadInventoryItem", AT_ITEM_CODE, ItemCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.InventoryItem.Count > 0 Then
                ' yes, no need to determine what type of action as Shop.com always want complete Product list for an change
                ' start Product file
                StartShopComInventoryFile()

                ' get list of Matrix items to publish on Shop.com first
                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221.TableName, _
                   "ReadInventoryMatrixItemsToPublishOnShopCOM_DEV000221", AT_STORE_MERCHANT_ID, ActiveShopComSettings.CatalogID}}, _
                   Interprise.Framework.Base.Shared.ClearType.Specific)
                ' initialise last group code so we read the first group item details
                strLastMatrixGroupItem = ""
                ' process each matrix item
                For iLoop = 0 To ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221.Count - 1
                    ' is matrix group code same as last record ?
                    If ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixGroupItemCode <> strLastMatrixGroupItem Then
                        ' no, wss there a previous Matrix Group Item  ?
                        If strLastMatrixGroupItem <> "" Then
                            ' yes, close product container
                            CloseShopComProductContainer(ImportExportStatusFacade, ImportExportStatusDataset, strLastMatrixGroupItem)
                        End If
                        ' get new Matrix Group Item details
                        ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
                           "ReadInventoryItem", AT_ITEM_CODE, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixGroupItemCode}, _
                            New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, "ReadInventoryItemDescription", _
                            AT_ITEM_CODE, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixGroupItemCode, _
                            AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
                            New String() {ImportExportStatusDataset.InventoryShopComDetails_DEV000221.TableName, "ReadInventoryShopComDetails_DEV000221", _
                            AT_ITEM_CODE, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixGroupItemCode}}, _
                           Interprise.Framework.Base.Shared.ClearType.Specific)
                        ' is there a Shop.com detail record ?
                        If ImportExportStatusDataset.InventoryShopComDetails_DEV000221.Count > 0 Then
                            ' yes, start shop.com product container for matrix group
                            StartShopComProductContainer(ImportExportStatusFacade, ImportExportStatusDataset, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixGroupItemCode, ActiveSource, ActiveShopComSettings)

                            strLastMatrixGroupItem = ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixGroupItemCode
                            bMatrixItemError = False
                        Else
                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoShopComInventoryUpdate", "No Shop.com details found for Inventory Item " & ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixGroupItemCode) ' TJS 13/11/13
                            bMatrixItemError = True
                        End If
                    End If

                    If Not bMatrixItemError Then
                        ' get Matrix Item details
                        ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
                           "ReadInventoryItem", AT_ITEM_CODE, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixItemCode}, _
                            New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, "ReadInventoryItemDescription", _
                            AT_ITEM_CODE, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixItemCode, _
                            AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
                            New String() {ImportExportStatusDataset.InventoryShopComDetails_DEV000221.TableName, "ReadInventoryShopComDetails_DEV000221", _
                            AT_ITEM_CODE, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixItemCode}}, _
                           Interprise.Framework.Base.Shared.ClearType.Specific)
                        ' is there a Shop.com detail record ?
                        If ImportExportStatusDataset.InventoryShopComDetails_DEV000221.Count > 0 Then
                            ' yes, add product details for matrix item
                            AddShopComProductPermutation(ImportExportStatusFacade, ImportExportStatusDataset, ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixItemCode) ' TJS 18/07/09
                        Else
                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoShopComInventoryUpdate", "No Shop.com details found for Inventory Item " & ImportExportStatusDataset.InventoryMatrixItemsToPublishOnShopCOM_DEV000221(iLoop).MatrixItemCode) ' TJS 13/11/13
                        End If
                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
                ' were any Matrix Items published ?
                If strLastMatrixGroupItem <> "" Then
                    ' yes, add tag details
                    AddShopComTagDetails(ImportExportStatusFacade, ImportExportStatusDataset, xmlProductNode, strLastMatrixGroupItem, "2") ' TJS 28/07/09
                    ' and close product container
                    CloseShopComProductContainer(ImportExportStatusFacade, ImportExportStatusDataset, strLastMatrixGroupItem)

                    bShopComInventoryUpdatesToSend = True
                End If

                ' now get list of non matrix items to publish on Shop.com
                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
                   "ReadInventoryItemPublishOnShopCom_DEV000221", AT_STORE_MERCHANT_ID, ActiveShopComSettings.CatalogID}}, _
                   Interprise.Framework.Base.Shared.ClearType.Specific)
                ' are there any items to publish ?
                If ImportExportStatusDataset.InventoryItem.Count > 0 Then
                    ' yes, create xml for each
                    For iLoop = 0 To ImportExportStatusDataset.InventoryItem.Count - 1
                        ' get Matrix Item details
                        ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, "ReadInventoryItemDescription", _
                            AT_ITEM_CODE, ImportExportStatusDataset.InventoryItem(iLoop).ItemCode, _
                            AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
                            New String() {ImportExportStatusDataset.InventoryShopComDetails_DEV000221.TableName, "ReadInventoryShopComDetails_DEV000221", _
                            AT_ITEM_CODE, ImportExportStatusDataset.InventoryItem(iLoop).ItemCode}}, _
                           Interprise.Framework.Base.Shared.ClearType.Specific)
                        ' is there a Shop.com detail record ?
                        If ImportExportStatusDataset.InventoryShopComDetails_DEV000221.Count > 0 Then
                            ' yes, start shop.com product container for item
                            StartShopComProductContainer(ImportExportStatusFacade, ImportExportStatusDataset, ImportExportStatusDataset.InventoryItem(iLoop).ItemCode, ActiveSource, ActiveShopComSettings)
                            ' add product details for item
                            AddShopComProductDetails(ImportExportStatusFacade, ImportExportStatusDataset, ImportExportStatusDataset.InventoryItem(iLoop).ItemCode)
                            ' close product container for item
                            CloseShopComProductContainer(ImportExportStatusFacade, ImportExportStatusDataset, ImportExportStatusDataset.InventoryItem(iLoop).ItemCode)

                            bShopComInventoryUpdatesToSend = True
                        Else
                            ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoShopComInventoryUpdate", "No Shop.com details found for Inventory Item " & ImportExportStatusDataset.InventoryItem(iLoop).ItemCode) ' TJS 13/11/13
                        End If
                        If bShutDownInProgress Then ' TJS 02/08/13
                            Exit For ' TJS 02/08/13
                        End If
                    Next
                End If
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True ' TJS 28/07/09

                ' check for any other item records for the same Shop.com CatalogID and mark them as complete
                iStartCheckRowID = RowID + 1 ' TJS 28/07/09
                For iCheckLoop = iStartCheckRowID To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveShopComSettings.CatalogID Then ' TJS 02/12/11
                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True ' TJS 28/07/09
                    Else
                        Exit For
                    End If
                    If bShutDownInProgress Then ' TJS 02/08/13
                        Exit For ' TJS 02/08/13
                    End If
                Next
                RowID = iCheckLoop - 1 ' TJS 28/07/09
                ' now send file to Shop.com
                SendShopComInventoryFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveShopComSettings)

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoShopComInventoryUpdate", "No Inventory found for " & ItemCode)

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoShopComInventoryUpdate", ex)

        End Try
    End Sub

    Public Sub StartShopComInventoryFile()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | TJS             | 2009.2.10 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            XMLInventoryUpdateFile = New XDocument

            xmlInventoryUpdateNode = New XElement("transmission")
            xmlInventoryUpdateNode.SetAttributeValue("xmlns", "")
            xmlInventoryUpdateNode.SetAttributeValue("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
            xmlInventoryUpdateNode.SetAttributeValue("xsi:noNamespaceSchemaLocation", "C:\Master_Files\PDI_Docs\APDI_Merchant.xsd")

            XMLInventoryUpdateFile.Add(xmlInventoryUpdateNode)

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartShopComInventoryFile", ex)

        End Try

    End Sub

    Public Sub StartShopComProductContainer(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal MatrixGroupItemCode As String, ByVal ActiveSource As SourceSettings, ByVal ActiveShopComSettings As ShopComSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/06/09 | TJS             | 2009.3.00 | Function added
        ' 28/07/09 | TJS             | 2009.3.03 | Corrected xml build
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlImageNode As XElement, xmlDepartmentNode As XElement, xmlOptionNode As XElement
        Dim iAttributeLoop As Integer, iValueLoop As Integer

        Try
            ' start product container element
            xmlProductContainerNode = New XElement("product_container")
            If ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).GroupName_DEV000221 <> INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
                xmlProductContainerNode.Add(New XElement("group_name", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).GroupName_DEV000221))
            Else
                xmlProductContainerNode.Add(New XElement("group_name", ImportExportStatusDataset.InventoryItem(0).ItemName))
            End If
            If ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).GroupDescription_DEV000221 <> INVENTORY_USE_GENERAL_TAB_EXT_DESCRIPTION Then ' TJS 13/11/13
                xmlProductContainerNode.Add(New XElement("group_description", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).GroupDescription_DEV000221))
            Else
                xmlProductContainerNode.Add(New XElement("group_description", ImportExportStatusDataset.InventoryItemDescription(0).ItemDescription))
            End If
            xmlImageNode = New XElement("image")
            xmlImageNode.Add(New XElement("image_url", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).ImageURL_DEV000221))
            xmlProductContainerNode.Add(xmlImageNode)
            xmlProductContainerNode.Add(New XElement("keywords", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).Keywords_DEV000221))
            xmlDepartmentNode = New XElement("department")
            xmlDepartmentNode.Add(New XElement("first_level_department", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).FirstLevelDepartment_DEV000221))
            If Not ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).IsSecondLevelDepartment_DEV000221Null Then
                xmlDepartmentNode.Add(New XElement("second_level_department", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).SecondLevelDepartment_DEV000221))
            End If
            If Not ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).IsThirdLevelDepartment_DEV000221Null Then
                xmlDepartmentNode.Add(New XElement("third_level_department", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).ThirdLevelDepartment_DEV000221))
            End If
            xmlProductContainerNode.Add(xmlDepartmentNode)

            ' is item a Matrix Group record?
            If ImportExportStatusDataset.InventoryItem(0).ItemType = "Matrix Group" Then
                ' yes, start Product element
                xmlProductNode = New XElement("product")
                ' add Item Code or Item Name as per config settings
                If ActiveShopComSettings.ISItemIDField = "ItemName" Then
                    If ActiveShopComSettings.SourceItemIDField = "IT_SKU" Then
                        xmlProductNode.Add(New XElement("sku", ImportExportStatusDataset.InventoryItem(0).ItemName))
                    Else
                        xmlProductNode.Add(New XElement("line_item_code", ImportExportStatusDataset.InventoryItem(0).ItemName))
                    End If
                Else
                    If ActiveShopComSettings.SourceItemIDField = "IT_SKU" Then
                        xmlProductNode.Add(New XElement("sku", ImportExportStatusDataset.InventoryItem(0).ItemCode))
                    Else
                        xmlProductNode.Add(New XElement("line_item_code", ImportExportStatusDataset.InventoryItem(0).ItemCode))
                    End If
                End If

                ' get Matrix Group Attribute list
                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAttributeView.TableName, _
                    "ReadInventoryAttributeView_DEV000221", AT_ITEM_CODE, MatrixGroupItemCode}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 28/07/09
                For iAttributeLoop = 0 To ImportExportStatusDataset.InventoryAttributeView.Count - 1
                    ' get Values for current Attribute
                    ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAttributeValueView_DEV000221.TableName, _
                        "ReadInventoryAttributeValueView_DEV000221", AT_ITEM_CODE, MatrixGroupItemCode, _
                        AT_ATTRIBUTE_CODE, ImportExportStatusDataset.InventoryAttributeView(iAttributeLoop).AttributeCode}}, _
                        Interprise.Framework.Base.Shared.ClearType.Specific)
                    For iValueLoop = 0 To ImportExportStatusDataset.InventoryAttributeValueView_DEV000221.Count - 1
                        xmlOptionNode = New XElement("option")
                        xmlOptionNode.Add(New XElement("option_type", ImportExportStatusDataset.InventoryAttributeView(iAttributeLoop).AttributeDescription))
                        xmlOptionNode.Add(New XElement("option_values", ImportExportStatusDataset.InventoryAttributeValueView_DEV000221(iValueLoop).AttributeValueDescription)) ' TJS 28/07/09
                        xmlProductNode.Add(xmlOptionNode) ' TJS 28/07/09
                    Next
                Next
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartShopComProductContainer", ex)

        End Try

    End Sub

    Public Sub CloseShopComProductContainer(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal MatrixGroupItemCode As String) ' TJS 26/07/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/06/09 | TJS             | 2009.3.00 | Function added
        ' 28/07/09 | TJS             | 2009.3.03 | Corrected xml build and added tag properties
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            xmlProductContainerNode.Add(xmlProductNode) ' TJS 28/07/09

            AddShopComTagDetails(ImportExportStatusFacade, ImportExportStatusDataset, xmlProductContainerNode, MatrixGroupItemCode, "1") ' TJS 28/07/09

            xmlInventoryUpdateNode.Add(xmlProductContainerNode)

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "CloseShopComProductContainer", ex)

        End Try

    End Sub

    Public Sub AddShopComProductDetails(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ItemCode As String) ' TJS 26/07/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | TJS             | 2009.2.10 | Function added
        ' 18/06/09 | TJS             | 2009.3.00 | function completed
        ' 28/07/09 | TJS             | 2009.3.03 | Added tag properties
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected string constants used to indicate product name and description are taken from InventoryItem table
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        Try
            xmlProductNode = New XElement("product")

            xmlProductNode.Add(New XElement("line_item_code", ItemCode))
            If ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).GroupName_DEV000221 <> INVENTORY_USE_GENERAL_TAB_DESCRIPTION Then ' TJS 13/11/13
                xmlProductNode.Add(New XElement("line_item_name", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).GroupName_DEV000221))
            Else
                xmlProductNode.Add(New XElement("line_item_name", ImportExportStatusDataset.InventoryItem(0).ItemName))
            End If
            xmlProductNode.Add(New XElement("line_item_list_price", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).SellingPrice_DEV000221.ToString("0.00")))

            xmlProductNode.Add(New XElement("attributes", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221))

            AddShopComTagDetails(ImportExportStatusFacade, ImportExportStatusDataset, xmlProductNode, ItemCode, "2") ' TJS 28/07/09

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddShopComProductDetails", ex)

        End Try

    End Sub

    Public Sub AddShopComProductPermutation(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ItemCode As String)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/06/09 | TJS             | 2009.3.00 | Function added
        ' 18/07/09 | TJS             | 2009.3.03 | Added code to check stock status
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlPermutationNode As XElement
        Dim decQtyAvailable As Decimal, decQtyBackOrder As Decimal, strTemp As String ' TJS 18/07/09

        Try
            xmlPermutationNode = New XElement("permutation")

            xmlPermutationNode.Add(New XElement("permutation_value", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).AttributeCategory_DEV000221))
            xmlPermutationNode.Add(New XElement("permutation_sku", ImportExportStatusDataset.InventoryItem(0).ItemCode))
            xmlPermutationNode.Add(New XElement("permutation_price", ImportExportStatusDataset.InventoryShopComDetails_DEV000221(0).SellingPrice_DEV000221.ToString("0.00")))
            strTemp = ImportExportStatusFacade.GetField("SELECT SUM(UnitsAvailable) FROM dbo.InventoryStockTotal WHERE ItemCode = '" & _
                ItemCode & "'", CommandType.Text, Nothing) ' TJS 18/07/09
            If strTemp <> "" Then ' TJS 18/07/09
                decQtyAvailable = CDec(strTemp) ' TJS 18/07/09
            Else
                decQtyAvailable = 0 ' TJS 18/07/09
            End If

            If decQtyAvailable > 0 Then ' TJS 18/07/09
                ' in stock
                xmlPermutationNode.Add(New XElement("permutation_inventory_status", "0"))
            Else
                strTemp = ImportExportStatusFacade.GetField("SELECT SUM(UnitsOnBackOrder) FROM dbo.InventoryStockTotal WHERE ItemCode = '" & _
                    ItemCode & "'", CommandType.Text, Nothing) ' TJS 18/07/09
                If strTemp <> "" Then ' TJS 18/07/09
                    decQtyBackOrder = CDec(strTemp) ' TJS 18/07/09
                Else
                    decQtyBackOrder = 0 ' TJS 18/07/09
                End If

                If decQtyBackOrder <= 0 Then ' TJS 18/07/09
                    ' sold out
                    xmlPermutationNode.Add(New XElement("permutation_inventory_status", "3"))
                Else
                    ' on back order
                    xmlPermutationNode.Add(New XElement("permutation_inventory_status", "4"))
                End If
            End If

            xmlProductNode.Add(xmlPermutationNode)

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddShopComProductPermutation", ex)

        End Try

    End Sub

    Public Sub SendShopComInventoryFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveShopComSettings As ShopComSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | TJS             | 2009.2.10 | Function added
        ' 18/06/09 | TJS             | 2009.3.00 | Code completed
        ' 28/07/09 | TJS             | 2009.3.03 | Corrected file name and modified to save inventory action status to DB
        ' 23/08/09 | TJS             | 2009.3.04 | Modified to use CheckRegistryValue
        ' 19/08/10 | TJS             | 2010.1.00 | Corrected InhibitWebPosts initialisation
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim writer As StreamWriter, ff As LerrynFTP.FTP
        Dim strProductFile As String, bInhibitWebPosts As Boolean

        strProductFile = ""
        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") ' TJS 23/08/09 TJS 19/08/10

            If ActiveShopComSettings.FTPUploadPath <> "" And ActiveShopComSettings.FTPUploadURL <> "" And ActiveShopComSettings.FTPUploadUserName <> "" And ActiveShopComSettings.FTPUploadPassword <> "" Then
                ' create the status update file
                strProductFile = "ProductFile_" & Format(Date.Now.ToString("yyyyMMdd_HHmmss")) & ".xml" ' TJS 24/05/09
                writer = New StreamWriter(ActiveShopComSettings.FTPUploadPath & strProductFile, True)
                ' write the XML to it
                writer.WriteLine(XMLInventoryUpdateFile.ToString)
                ' and close it
                writer.Close()

                ff = New LerrynFTP.FTP
                ' Setup the FTP connection details
                ff.RemoteHostFTPServer = ActiveShopComSettings.FTPUploadURL
                ff.RemoteUser = ActiveShopComSettings.FTPUploadUserName
                ff.RemotePassword = ActiveShopComSettings.FTPUploadPassword
                m_ImportExportConfigFacade.WriteLogProgressRecord("Connecting to the FTP Server - " & ActiveShopComSettings.FTPUploadURL)
                If ff.Login() Then
                    ' make sure we are at the root directory
                    ff.ChangeDirectory("/")
                    If Not bInhibitWebPosts Then ' TJS 24/05/09
                        ff.UploadFile(ActiveShopComSettings.FTPUploadPath & strProductFile)
                    End If
                    ' file with this name exist in archive directory ?
                    If My.Computer.FileSystem.FileExists(ActiveShopComSettings.FTPUploadArchivePath & strProductFile) Then
                        ' yes, delete it
                        My.Computer.FileSystem.DeleteFile(ActiveShopComSettings.FTPUploadArchivePath & strProductFile)
                    End If
                    ' move product file to archive
                    My.Computer.FileSystem.MoveFile(ActiveShopComSettings.FTPUploadPath & strProductFile, ActiveShopComSettings.FTPUploadArchivePath & strProductFile)

                    m_ImportExportConfigFacade.WriteLogProgressRecord("FTP Inventory file Upload complete, Disconnecting from the FTP Server - " & ActiveShopComSettings.FTPUploadURL)
                    ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                        "CreateLerrynImportExportInventoryActionStatus_DEV000221", "UpdateLerrynImportExportInventoryActionStatus_DEV000221", "DeleteLerrynImportExportInventoryActionStatus_DEV000221"}}, _
                        Interprise.Framework.Base.Shared.TransactionType.None, "Update Shop.com Inventory Status", False) ' TJS 24/05/09
                    bShopComInventoryUpdatesToSend = False ' TJS 24/05/09
                    ff.CloseConnection()

                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComInventoryFile", "Login failed for FTL Upload URL with settings in Shop.com settings", ActiveShopComSettings.XMLConfig.ToString)
                    My.Computer.FileSystem.DeleteFile(ActiveShopComSettings.FTPUploadPath & strProductFile)

                End If

            ElseIf ActiveShopComSettings.FTPUploadPath <> "" Then
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComInventoryFile", "No FTP Upload Path found in Shop.com settings", ActiveShopComSettings.XMLConfig.ToString)

            ElseIf ActiveShopComSettings.FTPUploadURL <> "" Then
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComInventoryFile", "No FTP Upload URL found in Shop.com settings", ActiveShopComSettings.XMLConfig.ToString)

            ElseIf ActiveShopComSettings.FTPUploadUserName <> "" Then
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComInventoryFile", "No FTP Upload Username found in Shop.com settings", ActiveShopComSettings.XMLConfig.ToString)

            Else
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SendShopComInventoryFile", "No FTP Upload Password found in Shop.com settings", ActiveShopComSettings.XMLConfig.ToString)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SendShopComInventoryFile", ex)
            ' has product file been created ?
            If strProductFile <> "" Then
                If My.Computer.FileSystem.FileExists(ActiveShopComSettings.FTPUploadPath & strProductFile) Then
                    ' yes, delete it
                    My.Computer.FileSystem.DeleteFile(ActiveShopComSettings.FTPUploadPath & strProductFile)
                End If
            End If

        End Try

    End Sub

    Private Sub AddShopComTagDetails(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByRef XmlNodeForTags As XElement, ByVal ItemCode As String, ByVal TagLocation As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 28/07/09 | TJS             | 2009.3.03 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '------------------------------------------------------------------------------------------

        Dim strTagName As String, iLoop As Integer

        ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221.TableName, _
            "ReadInventoryShopComTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, TagLocation}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)
        For iLoop = 0 To ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221.Count - 1
            strTagName = ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagName_DEV000221
            If Not ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).IsTagXMLName_DEV000221Null Then
                If ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagXMLName_DEV000221 <> "" Then
                    strTagName = ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagXMLName_DEV000221
                End If
            End If
            If Not ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).IsTagTextValue_DEV000221Null Then
                If ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagTextValue_DEV000221 <> "" Then
                    Select Case ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagDataType_DEV000221
                        Case "Text", "Y/N"
                            XmlNodeForTags.Add(New XElement(strTagName, ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagTextValue_DEV000221))

                        Case "Date"
                        Case "DateTime"

                        Case "Integer", "Numeric"
                            XmlNodeForTags.Add(New XElement(strTagName, ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagNumericValue_DEV000221.ToString))

                        Case "Currency"
                            XmlNodeForTags.Add(New XElement(strTagName, ImportExportStatusDataset.InventoryShopComTagDetailTemplateView_DEV000221(iLoop).TagNumericValue_DEV000221.ToString("0.00")))

                    End Select
                End If
            End If
        Next

    End Sub
End Module
