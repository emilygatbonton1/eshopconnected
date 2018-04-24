' eShopCONNECT for Connected Business - Windows Service
' Module: AmazonInventoryUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 02 August 2013

Imports System.IO
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const ' TJS 05/04/11
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module AmazonInventoryUpdate

    Friend Structure MessageIDInFile
        Dim MessageID As Integer
        Dim FileName As String
    End Structure

    Private XMLInventoryUpdateFile As XDocument
    Private XMLProductMessage As XElement
    Private iAmazonXMLMessageType As Integer
    Private rowAmazonMessages As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonMessageSettings_DEV000221Row ' TJS 01/07/09
    Private xmlInventoryUpdateNode As XElement
    Friend bAmazonInventoryUpdatesToSend As Boolean

    Public Sub DoAmazonInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer, ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | LJG             | 2009.2.10 | Function added
        ' 29/09/09 | TJS             | 2009.3.07 | Completed Product XML 
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to check Amazon next connection time
        ' 27/11/09 | TJS             | 2009.3.09 | corrected various errors
        ' 19/08/10 | TJs             | 2010.1.00 | Modified to set completed flag on records where no Item found
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 05/07/12 | TJS             | 2012.1.08 | Added code to update inventory stock levels to Amazon
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLUpdate As XElement
        Dim iCheckLoop As Integer, iOrigRowID As Integer, bBeenDeleted As Boolean

        Try
            ' Is next amazon connection due ?
            If ActiveAmazonSettings.NextConnectionTime <= Date.Now() Then ' TJS 15/10/09
                ' yes, check for inventory updates
                ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
                   "ReadInventoryItem", AT_ITEM_CODE, ItemCode}, New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, _
                   "ReadInventoryItemDescription", AT_ITEM_CODE, ItemCode, AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
                   New String() {ImportExportStatusDataset.InventoryAmazonDetails_DEV000221.TableName, _
                   "ReadInventoryAmazonDetails_DEV000221", AT_ITEM_CODE, ItemCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                If ImportExportStatusDataset.InventoryItem.Count > 0 And ImportExportStatusDataset.InventoryAmazonDetails_DEV000221.Count > 0 Then
                    Select Case ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221
                        Case "100" ' New Item
                            XMLUpdate = Nothing ' TJS 29/09/09
                            CreateAmazonProductFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, "Update", XMLUpdate) ' TJS 29/09/09 TJS 27/11/09

                            ' check for any new tag records or updated item/tag records and mark them as complete
                            ' also check if a deleted item row exists
                            bBeenDeleted = False
                            iOrigRowID = RowID ' TJS 27/11/09
                            For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1 ' TJS 29/09/09
                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken Then ' TJS 29/09/09 TJS 02/12/11 TJS 22/03/13
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Or _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "200" Or _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "205" Or _
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "210" Then ' TJS 29/09/09 TJS 27/11/09
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True ' TJS 29/09/09

                                    ElseIf ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "300" Then '  TJS 27/11/09
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True ' TJS 27/11/09
                                        bBeenDeleted = True ' TJS 27/11/09
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

                            ' was a deleted item row found ?
                            If Not bBeenDeleted Then
                                ' no, save XML ready for sending
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString ' TJS 29/09/09
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLMessageType_DEV000221 = 11 ' 11 is Product File  TJS 29/09/09
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).XMLToSend_DEV000221 = True ' TJS 29/09/09
                            Else
                                ' yes, mark row as complete
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True ' TJS 27/11/09
                            End If


                        Case "105" ' New Amazon Tag value

                        Case "150" ' Amazon Product File submitted, now sent Image file 
                            XMLUpdate = Nothing ' TJS 29/09/09
                            If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsImageURL_DEV000221Null Then
                                CreateAmazonImageFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, "Update", XMLUpdate) ' TJS 29/09/09
                            End If

                            ' save XML ready for sending
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString ' TJS 29/09/09
                            ' set message type and mark record as having XML to send
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 12 ' 12 is Image File  TJS 29/09/09
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True ' TJS 29/09/09


                        Case "151" ' Amazon Product File submitted, now sent Inventory file 
                            XMLUpdate = Nothing ' TJS 29/09/09
                            CreateAmazonInventoryFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, "Update", XMLUpdate) ' TJS 29/09/09

                            ' save XML ready for sending
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString ' TJS 29/09/09
                            ' set message type and mark record as having XML to send
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 13 ' 13 is Inventory File  TJS 29/09/09
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True ' TJS 29/09/09


                        Case "152" ' Amazon Product File submitted, now sent Price file 
                            XMLUpdate = Nothing ' TJS 29/09/09
                            CreateAmazonPricingFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, XMLUpdate) ' TJS 29/09/09

                            ' save XML ready for sending
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString ' TJS 29/09/09
                            ' set message type and mark record as having XML to send
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 14 ' 14 is Price File  TJS 29/09/09
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True ' TJS 29/09/09


                        Case "153" ' Amazon Product File submitted, now sent Relationship file 
                            If ImportExportStatusDataset.InventoryItem(0).ItemType = "Matrix Group" Or ImportExportStatusDataset.InventoryItem(0).ItemType = "Matrix Item" Then
                                XMLUpdate = Nothing ' TJS 29/09/09
                                If CreateAmazonRelationshipFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, XMLUpdate) Then ' TJS 29/09/09
                                    ' save XML ready for sending
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString ' TJS 29/09/09
                                    ' set message type and mark record as having XML to send
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 15 ' 15 is Relationship File  TJS 29/09/09
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True ' TJS 29/09/09

                                Else
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True ' TJS 29/09/09
                                End If

                            Else
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True ' TJS 29/09/09
                            End If


                        Case "200" ' Updated Item
                            XMLUpdate = Nothing ' TJS 29/09/09
                            If ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductFileSent_DEV000221 Then ' TJS 29/09/09
                                CreateAmazonProductFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, "Update", XMLUpdate) ' TJS 29/09/09
                            Else
                                CreateAmazonProductFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, "New", XMLUpdate) ' TJS 29/09/09
                            End If

                            ' save XML ready for sending
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString ' TJS 29/09/09
                            ' set message type and mark record as having XML to send
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 11 ' 11 is Product File  TJS 29/09/09
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True ' TJS 29/09/09

                            ' check for any new item records and mark them as complete
                            For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1 ' TJS 29/09/09
                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken Then ' TJS 29/09/09 TJS 02/12/11 TJS 22/03/13
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "105" Then ' TJS 29/09/09 TJS 27/11/09
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = True ' TJS 29/09/09
                                    End If
                                Else
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

                        Case "205" ' Updated Amazon Tag value

                        Case "210" ' Updated Amazon Price value

                        Case "300" ' Deleted Item

                            ' start of code added TJS 05/07/12
                        Case "1000" ' Stock Quantity changed
                            XMLUpdate = Nothing
                            CreateAmazonInventoryFile(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, ActiveAmazonSettings, RowID, ItemCode, "Update", XMLUpdate)

                            ' save XML ready for sending
                            If XMLUpdate IsNot Nothing Then
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = XMLUpdate.ToString
                                ' set message type and mark record as having XML to send
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 13 ' 13 is Inventory File 
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True
                            Else
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
                            End If
                            ' end of code added TJS 05/07/12

                    End Select

                ElseIf ImportExportStatusDataset.InventoryItem.Count > 0 Then
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoAmazonInventoryUpdate", "No Inventory Item found for " & ItemCode)
                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True ' TJS 19/08/10

                Else
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoAmazonInventoryUpdate", "No Inventory Amazon Details found for " & ItemCode)
                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True ' TJS 19/08/10

                End If
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoAmazonInventoryUpdate", ex)

        End Try
    End Sub

    Public Sub StartAmazonInventoryFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByVal AmazonXMLMessageType As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | LJG             | 2009.2.10 | Function added
        ' 01/07/09 | TJS             | 2009.3.00 | Modified to get Message ID from config table
        ' 29/09/09 | TJS             | 2009.3.07 | Modified to cater for Image, Inventory etc file
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to check Amazon next connection time
        ' 27/11/09 | TJS             | 2009.3.09 | Completed code for Amazon SOAP Client
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Amazon document ID exceeding SQL int value range
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        '                                        | and corrected generation of Amazon Envelope namespaces
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
        Dim xmlUpdateNode As XElement, xmlDefaultNamespace As XNamespace, xmlXSINamespace As XNamespace ' TJS 22/03/13
        Dim strXMLMessageType As String, strFileType As String, sTemp As String, iLoop As Integer
        Dim bFileExists As Boolean ' TJS 27/11/09

        Try
            ' Is next amazon connection due ?
            If ActiveAmazonSettings.NextConnectionTime <= Date.Now() Then ' TJS 15/10/09
                ' yes, create amazon XML emvelope
                xmlDefaultNamespace = XNamespace.Get("") ' TJS 22/03/13
                xmlXSINamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") ' TJS 22/03/13
                XMLInventoryUpdateFile = New XDocument

                xmlInventoryUpdateNode = New XElement(xmlDefaultNamespace + "AmazonEnvelope") ' TJS 22/03/13
                xmlInventoryUpdateNode.SetAttributeValue(XNamespace.Xmlns + "xsi", xmlXSINamespace.NamespaceName) ' TJS 22/03/13
                xmlInventoryUpdateNode.SetAttributeValue(xmlXSINamespace + "noNamespaceSchemaLocation", "amzn-envelope.xsd") ' TJS 22/03/13
                xmlUpdateNode = New XElement("Header")
                xmlUpdateNode.Add(New XElement("DocumentVersion", "1.01"))
                xmlUpdateNode.Add(New XElement("MerchantIdentifier", ActiveAmazonSettings.MerchantToken)) ' TJS 22/03/13
                xmlInventoryUpdateNode.Add(xmlUpdateNode)
                Select Case AmazonXMLMessageType
                    Case 11
                        strXMLMessageType = "Product"
                        strFileType = "_POST_PRODUCT_DATA_" ' TJS 27/11/09
                    Case 12 ' TJS 29/09/09
                        strXMLMessageType = "ProductImage" ' TJS 29/09/09
                        strFileType = "_POST_PRODUCT_IMAGE_DATA_" ' TJS 27/11/09
                    Case 13 ' TJS 29/09/09
                        strXMLMessageType = "Inventory" ' TJS 29/09/09
                        strFileType = "_POST_INVENTORY_AVAILABILITY_DATA_" ' TJS 27/11/09
                    Case 14 ' TJS 29/09/09
                        strXMLMessageType = "Price" ' TJS 29/09/09
                        strFileType = "_POST_PRODUCT_PRICING_DATA_" ' TJS 27/11/09
                    Case 15 ' TJS 29/09/09
                        strXMLMessageType = "Relationship" ' TJS 29/09/09
                        strFileType = "_POST_PRODUCT_RELATIONSHIP_DATA_" ' TJS 27/11/09
                    Case Else
                        strXMLMessageType = ""
                        strFileType = "" ' TJS 27/11/09
                End Select
                xmlInventoryUpdateNode.Add(New XElement("MessageType", strXMLMessageType))
                iAmazonXMLMessageType = AmazonXMLMessageType
                ' have the Amazon Message Settings been loaded ?
                If ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.Count = 0 Then ' TJS 01/07/09
                    ' no, load them
                    ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.TableName, _
                        "ReadLerrynImportExportAmazonMessageSettings_DEV000221"}}, Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 01/07/09
                End If
                ' find message settings for active Merchant ID
                rowAmazonMessages = ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.FindByMerchantID_DEV000221(ActiveAmazonSettings.MerchantToken) ' TJS 01/07/09 TJS 22/03/13
                ' did we find them? 
                If rowAmazonMessages Is Nothing Then ' TJS 01/07/09
                    ' no, create settings for active Merchant ID starting at message id 1
                    rowAmazonMessages = ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.NewLerrynImportExportAmazonMessageSettings_DEV000221Row ' TJS 01/07/09
                    rowAmazonMessages.MerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken ' TJS 01/07/09 TJS 22/03/13
                    rowAmazonMessages.NextMessageID_DEV000221 = 1 ' TJS 01/07/09
                    ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.AddLerrynImportExportAmazonMessageSettings_DEV000221Row(rowAmazonMessages) ' TJS 01/07/09
                End If

                bFileExists = False ' TJS 27/11/09
                rowAmazonFile = Nothing ' TJS 09/07/11
                For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1 ' TJS 27/11/09
                    If ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = "Current" Then ' TJS 27/11/09
                        rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop) ' TJS 27/11/09
                        bFileExists = True ' TJS 27/11/09
                        Exit For ' TJS 27/11/09
                    End If
                Next
                If Not bFileExists Then ' TJS 27/11/09
                    rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.NewLerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
                    sTemp = m_ImportExportConfigFacade.GetField("SELECT ISNULL(MIN(CAST(AmazonDocumentID_DEV000221 AS bigint)), 0) FROM dbo.LerrynImportExportAmazonFiles_DEV000221 WHERE CAST(AmazonDocumentID_DEV000221 AS bigint) < 0", CommandType.Text, Nothing) ' TJS 27/11/09 TJS 09/07/11
                    If "" & sTemp <> "" Then ' TJS 27/11/09
                        rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(CInt(sTemp) - 1) ' TJS 27/11/09
                    Else
                        rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(-1) ' TJS 27/11/09
                    End If
                End If
                rowAmazonFile.SiteCode_DEV000221 = ActiveAmazonSettings.AmazonSite ' TJS 27/11/09
                rowAmazonFile.MerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken ' TJS 27/11/09 TJS 22/03/13
                rowAmazonFile.FileName_DEV000221 = "Current" ' TJS 27/11/09
                rowAmazonFile.AmazonMessageType_DEV000221 = strFileType ' TJS 27/11/09
                rowAmazonFile.FileIsInputFromAmazon_DEV000221 = False ' TJS 27/11/09
                rowAmazonFile.FileSentToAmazon_DEV000221 = False ' TJS 27/11/09
                rowAmazonFile.ResponseReceived_DEV000221 = False ' TJS 27/11/09
                rowAmazonFile.FileContent_DEV000221 = XMLInventoryUpdateFile.ToString ' TJS 27/11/09
                If Not bFileExists Then ' TJS 27/11/09
                    ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.AddLerrynImportExportAmazonFiles_DEV000221Row(rowAmazonFile) ' TJS 27/11/09
                End If

                XMLInventoryUpdateFile.Add(xmlInventoryUpdateNode)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "StartAmazonInventoryFile", ex)

        End Try

    End Sub

    Public Sub AddToAmazonInventoryFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | LJG             | 2009.2.10 | Function added
        ' 01/07/09 | TJS             | 2009.3.00 | Modified to use Message ID from config table
        ' 29/09/09 | TJS             | 2009.3.07 | Modified to cater for Message element already being created 
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to check Amazon next connection time
        ' 27/11/09 | TJS             | 2009.3.09 | Completed code for Amazon SOAP Client
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strStartingUpdateXML As String, strMessageXML As String, iInsertPosn As Integer
        Dim iLoop As Integer ' TJS 27/11/09

        Try
            ' Is next amazon connection due ?
            If ActiveAmazonSettings.NextConnectionTime <= Date.Now() Then ' TJS 15/10/09
                ' yes, add message to amazon emvelope
                If Not ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).IsActionXMLFile_DEV000221Null Then
                    ' get message XML
                    strMessageXML = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 ' TJS 29/09/09
                    ' does mesage XML end with cr lf ?
                    If Right(strMessageXML, 2) = vbCrLf Then ' TJS 27/11/09
                        ' yes, remove cr lf
                        strMessageXML = Left(strMessageXML, Len(strMessageXML) - 2) ' TJS 27/11/09
                    End If
                    ' Insert MessageID
                    strMessageXML = Left(strMessageXML, 9) & "<MessageID>" & rowAmazonMessages.NextMessageID_DEV000221 & "</MessageID>" & Mid(strMessageXML, 10) ' TJS 29/09/09
                    ' and save in DB 
                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).SendInMessageID_DEV000221 = CStr(rowAmazonMessages.NextMessageID_DEV000221) ' TJS 29/09/09
                    For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1 ' TJS 27/11/09
                        If ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = "Current" Then ' TJS 27/11/09
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).SentInFileID_DEV000221 = _
                                ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).AmazonDocumentID_DEV000221 ' TJS 27/11/09
                            Exit For ' TJS 27/11/09
                        End If
                    Next

                    ' get current update XML
                    strStartingUpdateXML = XMLInventoryUpdateFile.ToString
                    ' get position for insert
                    iInsertPosn = InStr(strStartingUpdateXML, "</AmazonEnvelope>")
                    ' insert XML from action record with elevant surrounding elements including Message ID
                    strStartingUpdateXML = Left(strStartingUpdateXML, iInsertPosn - 1) & strMessageXML & Mid(strStartingUpdateXML, iInsertPosn) ' TJS 01/07/09 TJS 29/09/09
                    ' update Amazon XML Message ID
                    rowAmazonMessages.NextMessageID_DEV000221 = rowAmazonMessages.NextMessageID_DEV000221 + 1 ' TJS 01/07/09
                    ' reload update XML
                    XMLInventoryUpdateFile = XDocument.Parse(strStartingUpdateXML)

                    bAmazonInventoryUpdatesToSend = True ' TJS 01/07/09
                End If

                ' mark action record as XML Sent (change is committed to DB when status file successfully saved in SaveAmazonInventoryFile)
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "AddToAmazonInventoryFile", ex)

        End Try

    End Sub

    Public Sub SaveAmazonInventoryFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/06/09 | LJG             | 2009.2.10 | Function added
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to support direct connection to Amazon
        ' 27/11/09 | TJS             | 2009.3.09 | Completed code for Amazon SOAP Client
        ' 13/06/10 | TJs             | 2010.0.07 | Modified to check Inhibit Web Posts when loading Amazon files to send 
        ' 19/08/10 | TJS             | 2010.1.00 | Corrected InhibitWebPosts initialisation
        ' 06/04/11 | TJS             | 2011.0.08 | Disabled Amazon client to allow dll signing so Web SErvices will work
        ' 09/07/11 | TJS             | 2011.1.00 | Modified to cater for Amazon document ID exceeding SQL int value range
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
        Dim writer As StreamWriter, sXMLFileName As String, strFileType As String, sTemp As String, strXMLcontent As String ' TJS 27/11/09
        Dim iLoop As Integer, bFileExists As Boolean, bInhibitWebPosts As Boolean ' TJS 27/11/09 TJs 13/06/10

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES") ' TJs 13/06/10 TJS 19/08/10

            ' Is next amazon connection due ?
            If ActiveAmazonSettings.NextConnectionTime <= Date.Now() Then ' TJS 15/10/09
                ' yes, check submission Merchant ID is as expected
                If ImportExportStatusFacade.GetXMLElementText(XMLInventoryUpdateFile, AMAZON_STATUS_UPDATE_HEADER & "/MerchantIdentifier") = ActiveAmazonSettings.MerchantToken Then ' TJS 22/03/13
                    Select Case iAmazonXMLMessageType
                        Case 11
                            sXMLFileName = "ProductFile-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 15/10/09
                            strFileType = "_POST_PRODUCT_DATA_" ' TJS 27/11/09
                        Case 12 ' TJS 29/09/09
                            sXMLFileName = "ProductImageFile-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 29/09/09 TJS 15/10/09
                            strFileType = "_POST_PRODUCT_IMAGE_DATA_" ' TJS 27/11/09
                        Case 13 ' TJS 29/09/09
                            sXMLFileName = "InventoryFile-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 29/09/09 TJS 15/10/09
                            strFileType = "_POST_INVENTORY_AVAILABILITY_DATA_" ' TJS 27/11/09
                        Case 14 ' TJS 29/09/09
                            sXMLFileName = "PriceFile-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 29/09/09 TJS 15/10/09
                            strFileType = "_POST_PRODUCT_PRICING_DATA_" ' TJS 27/11/09
                        Case 15 ' TJS 29/09/09
                            sXMLFileName = "RelationshipFile-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 29/09/09 TJS 15/10/09
                            strFileType = "_POST_PRODUCT_RELATIONSHIP_DATA_" ' TJS 27/11/09
                        Case Else
                            sXMLFileName = "Unknown-" & Format(Date.Now.ToString("yyyyMMdd-HHmmssfff") & ".xml") ' TJS 08/04/09 TJS 15/10/09
                            strFileType = "" ' TJS 27/11/09
                    End Select
                    If strFileType <> "" Then ' TJS 27/11/09
                        strXMLcontent = Replace("<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>" & XMLInventoryUpdateFile.ToString, "><", ">" & vbCrLf & "<") ' TJS 27/11/09
                        ' have we inhibited sending files to amazon ?
                        If Not bInhibitWebPosts Then ' TJS 13/06/10
                            ' no, load ready for sending
                            'AmazonClient.AddFileToSend(ActiveAmazonSettings.AmazonSite, sXMLFileName, strXMLcontent, strFileType) ' TJS 27/11/09
                            m_ImportExportConfigFacade.WriteLogProgressRecord("Amazon connector currently disabled") ' TJS 06/04/11
                        Else
                            m_ImportExportConfigFacade.WriteLogProgressRecord("Amazon File " & sXMLFileName & " inhibited from being sent to amazon" & ActiveAmazonSettings.AmazonSite) ' TJS 13/06/10
                        End If
                        ' update File name in array 
                        bFileExists = False ' TJS 27/11/09
                        rowAmazonFile = Nothing ' TJS 09/07/11
                        For iLoop = 0 To ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.Count - 1 ' TJS 27/11/09
                            If ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = "Current" Or _
                                ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop).FileName_DEV000221 = sXMLFileName Then ' TJS 27/11/09
                                rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221(iLoop) ' TJS 27/11/09
                                bFileExists = True ' TJS 27/11/09
                                Exit For ' TJS 27/11/09
                            End If
                        Next
                        If Not bFileExists Then ' TJS 27/11/09
                            rowAmazonFile = ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.NewLerrynImportExportAmazonFiles_DEV000221Row ' TJS 27/11/09
                            sTemp = m_ImportExportConfigFacade.GetField("SELECT ISNULL(MIN(CAST(AmazonDocumentID_DEV000221 AS bigint)), 0) FROM dbo.LerrynImportExportAmazonFiles_DEV000221 WHERE CAST(AmazonDocumentID_DEV000221 AS bigint) < 0", CommandType.Text, Nothing) ' TJS 27/11/09 TJS 09/07/11
                            If "" & sTemp <> "" Then ' TJS 27/11/09
                                rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(CInt(sTemp) - 1) ' TJS 27/11/09
                            Else
                                rowAmazonFile.AmazonDocumentID_DEV000221 = CStr(-1) ' TJS 27/11/09
                            End If
                        End If
                        rowAmazonFile.SiteCode_DEV000221 = ActiveAmazonSettings.AmazonSite ' TJS 27/11/09
                        rowAmazonFile.MerchantID_DEV000221 = ActiveAmazonSettings.MerchantToken ' TJS 27/11/09 TJS 22/03/13
                        rowAmazonFile.FileName_DEV000221 = sXMLFileName ' TJS 27/11/09
                        rowAmazonFile.AmazonMessageType_DEV000221 = strFileType ' TJS 27/11/09
                        rowAmazonFile.FileIsInputFromAmazon_DEV000221 = False ' TJS 27/11/09
                        rowAmazonFile.FileSentToAmazon_DEV000221 = False ' TJS 27/11/09
                        rowAmazonFile.ResponseReceived_DEV000221 = False ' TJS 27/11/09
                        rowAmazonFile.Processed_DEV000221 = False ' TJS 27/11/09
                        rowAmazonFile.FileContent_DEV000221 = strXMLcontent ' TJS 27/11/09
                        If Not bFileExists Then ' TJS 27/11/09
                            ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.AddLerrynImportExportAmazonFiles_DEV000221Row(rowAmazonFile) ' TJS 27/11/09
                        End If

                        m_ImportExportConfigFacade.WriteLogProgressRecord("Amazon Send Inventory File created, File " & sXMLFileName)
                        ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
                            "CreateLerrynImportExportInventoryActionStatus_DEV000221", "UpdateLerrynImportExportInventoryActionStatus_DEV000221", _
                            "DeleteLerrynImportExportInventoryActionStatus_DEV000221"}, _
                            New String() {ImportExportStatusDataset.LerrynImportExportAmazonMessageSettings_DEV000221.TableName, _
                            "CreateLerrynImportExportAmazonMessageSettings_DEV000221", "UpdateLerrynImportExportAmazonMessageSettings_DEV000221", _
                            "DeleteLerrynImportExportAmazonMessageSettings_DEV000221"}, _
                            New String() {ImportExportStatusDataset.LerrynImportExportAmazonFiles_DEV000221.TableName, _
                            "CreateLerrynImportExportAmazonFiles_DEV000221", "UpdateLerrynImportExportAmazonFiles_DEV000221", _
                            "DeleteLerrynImportExportAmazonFiles_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, _
                            "Update Amazon Inventory Action Status", False) ' TJS 01/07/09 TJS 27/11/09
                        bAmazonInventoryUpdatesToSend = False ' TJS 01/07/09
                    Else
                        m_ImportExportConfigFacade.SendSourceErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SaveAmazonInventoryFile", "Unknown Amazon XML Message type " & iAmazonXMLMessageType, "") ' TJS 27/11/09

                    End If

                    If ActiveSource.XMLImportFileSavePath <> "" Then ' TJS 27/11/09
                        'Start the logging to the file
                        writer = New StreamWriter(ActiveSource.XMLImportFileSavePath & sXMLFileName, True) ' TJS 27/11/09

                        writer.WriteLine(XMLInventoryUpdateFile.ToString)
                        writer.Close()
                    End If

                Else
                    ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "SaveAmazonInventoryFile", "Unexpected Merchant ID " & ImportExportStatusFacade.GetXMLElementText(XMLInventoryUpdateFile, AMAZON_STATUS_UPDATE_HEADER & "/MerchantIdentifier") & " in submission file, expected " & ActiveAmazonSettings.MerchantToken, XMLInventoryUpdateFile.ToString) ' TJS 22/03/13
                End If
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "SaveAmazonInventoryFile", ex)

        End Try

    End Sub

    Private Sub CreateAmazonProductFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer, _
        ByVal ItemCode As String, ByVal OperationType As String, ByRef XMLUpdate As XElement)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/09/09 | TJS             | 2009.3.07 | Function added
        ' 05/11/09 | TJS             | 2009.3.09 | Modified to cate for ProductSubtype
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlProductNode As XElement, xmlProductIDNode As XElement
        Dim xmlDescriptionNode As XElement, xmlProductDataNode As XElement, xmlProductClassNode As XElement
        Dim xmlVariationNode As XElement, xmlClassificationDataNode As XElement
        Dim iLoop As Integer

        XMLUpdate = New XElement("Message")
        XMLUpdate.Add(New XElement("OperationType", OperationType))
        xmlProductNode = New XElement("Product")
        xmlProductNode.Add(New XElement("SKU", ItemCode))
        If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsProductID_DEV000221Null And _
            Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsProductIDType_DEV000221Null Then
            xmlProductIDNode = New XElement("StandardProductID")
            xmlProductIDNode.Add(New XElement("Type", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductIDType_DEV000221))
            xmlProductIDNode.Add(New XElement("Value", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductID_DEV000221))
            xmlProductNode.Add(xmlProductIDNode)
        End If
        If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsLaunchDate_DEV000221Null Then
            xmlProductNode.Add(New XElement("LaunchDate", CreateAmazonXMLDate(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).LaunchDate_DEV000221)))
        End If
        If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsReleaseDate_DEV000221Null Then
            xmlProductNode.Add(New XElement("ReleaseDate", CreateAmazonXMLDate(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ReleaseDate_DEV000221)))
        End If

        ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
            "ReadInventoryAmazonTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, AMAZON_CORE_TAG_LOCATION}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)

        ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAmazonSearchTerms.TableName, _
            "ReadInventoryAmazonTagDetails_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, AMAZON_SEARCH_TERM_TAG_LOCATION}}, _
           Interprise.Framework.Base.Shared.ClearType.Specific)

        xmlDescriptionNode = New XElement("DescriptionData")
        If ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsProductName_DEV000221Null Then
            xmlDescriptionNode.Add(New XElement("Title", ImportExportStatusDataset.InventoryItem(0).ItemName))
        ElseIf ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductName_DEV000221 = "<Use standard Item Name>" Then
            xmlDescriptionNode.Add(New XElement("Title", ImportExportStatusDataset.InventoryItem(0).ItemName))
        Else
            xmlDescriptionNode.Add(New XElement("Title", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductName_DEV000221))
        End If
        If ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsProductDescription_DEV000221Null Then
            xmlDescriptionNode.Add(New XElement("Description", ImportExportStatusDataset.InventoryItemDescription(0).ItemDescription))
        ElseIf ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductDescription_DEV000221 = "<Use standard Item Description>" Then
            xmlDescriptionNode.Add(New XElement("Description", ImportExportStatusDataset.InventoryItemDescription(0).ItemDescription))
        Else
            xmlDescriptionNode.Add(New XElement("Description", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductDescription_DEV000221))
        End If
        AddAmazonTagDetails(ImportExportStatusFacade, ImportExportStatusDataset, xmlDescriptionNode) ' TJS 05/11/09
        For iLoop = 0 To ImportExportStatusDataset.InventoryAmazonSearchTerms.Count - 1
            If Not ImportExportStatusDataset.InventoryAmazonSearchTerms(iLoop).IsTagTextValue_DEV000221Null Then
                xmlDescriptionNode.Add(New XElement("SearchTerms", ImportExportStatusDataset.InventoryAmazonSearchTerms(iLoop).TagTextValue_DEV000221))
            End If
        Next
        xmlDescriptionNode.Add(New XElement("RecommendedBrowseNode", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).BrowseNodeID_DEV000221))
        xmlProductNode.Add(xmlDescriptionNode)
        xmlProductDataNode = New XElement("ProductData") ' TJS 05/11/09
        If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsProductXMLClass_DEV000221Null Then
            xmlProductClassNode = New XElement(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductXMLClass_DEV000221)
            If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsProductXMLType_DEV000221Null Then
                xmlProductClassNode.Add(New XElement("ProductType", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductXMLType_DEV000221))
            End If
            If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsProductXMLSubType_DEV000221Null Then ' TJS 05/11/09
                xmlProductClassNode.Add(New XElement("ProductSubtype", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ProductXMLSubType_DEV000221)) ' TJS 05/11/09
            End If

            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
                "ReadInventoryAmazonTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, AMAZON_PRODUCT_DATA_TAG_LOCATION}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            AddAmazonTagDetails(ImportExportStatusFacade, ImportExportStatusDataset, xmlProductClassNode)  ' TJS 05/11/09

            xmlVariationNode = New XElement("VariationData") ' TJS 05/11/09
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
                "ReadInventoryAmazonTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, AMAZON_VARIATION_TAG_LOCATION}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            If AddAmazonTagDetails(ImportExportStatusFacade, ImportExportStatusDataset, xmlVariationNode) > 0 Then ' TJS 05/11/09
                xmlProductClassNode.Add(xmlVariationNode)
            End If

            xmlClassificationDataNode = New XElement("ClassificationData") ' TJS 05/11/09
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
                "ReadInventoryAmazonTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, AMAZON_CLASSIFICATION_DATA_TAG_LOCATION}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            If AddAmazonTagDetails(ImportExportStatusFacade, ImportExportStatusDataset, xmlClassificationDataNode) > 0 Then ' TJS 05/11/09
                xmlProductClassNode.Add(xmlClassificationDataNode)
            End If

            xmlProductDataNode.Add(xmlProductClassNode)
        End If

        xmlProductNode.Add(xmlProductDataNode)
        XMLUpdate.Add(xmlProductNode)

    End Sub

    Private Sub CreateAmazonImageFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer, _
     ByVal ItemCode As String, ByVal OperationType As String, ByRef XMLUpdate As XElement)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/09/09 | TJS             | 2009.3.07 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to use ISItemIDField setting
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlProductNode As XElement

        XMLUpdate = New XElement("Message")
        XMLUpdate.Add(New XElement("OperationType", OperationType))
        xmlProductNode = New XElement("ProductImage")
        If ActiveAmazonSettings.ISItemIDField = "ItemName" Then ' TJS 05/07/12
            xmlProductNode.Add(New XElement("SKU", ImportExportStatusDataset.InventoryItem(0).ItemName)) ' TJS 05/07/12
        Else
            xmlProductNode.Add(New XElement("SKU", ItemCode))
        End If
        If ImportExportStatusDataset.InventoryItem(0).ItemType = "Matrix Item" Then
            xmlProductNode.Add(New XElement("ImageType", "Child"))
        Else
            xmlProductNode.Add(New XElement("ImageType", "Main"))
        End If
        xmlProductNode.Add(New XElement("ImageLocation", ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).ImageURL_DEV000221))
        XMLUpdate.Add(xmlProductNode)

    End Sub

    Private Sub CreateAmazonInventoryFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer, _
     ByVal ItemCode As String, ByVal OperationType As String, ByRef XMLUpdate As XElement)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/09/09 | TJS             | 2009.3.07 | Function added
        ' 05/11/09 | TJS             | 2009.3.09 | Modified to pass XMLUpdate as parameter to AddAmazonTagItem
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to use ISItemIDField setting and Quantiry publishing options
        ' 15/03/13 | TJS             | 2013.1.03 | Corrected calculation of quantity to publish when sending Percent (divide by 100 missing)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlProductNode As XElement
        Dim decItemQtyAvailable As Decimal, decItemTotalQtyWhenLastPublished As Decimal, decItemQtyLastPublished As Decimal ' TJS 05/07/12
        Dim strTemp As String, strItemDueDates As String()(), iQtyOnOrder As Integer, iLoop As Integer, iQtyToPublish As Integer ' TJS 05/07/12
        Dim bNoRestockDate As Boolean, bQtyUpdateRequired As Boolean, bGetOnOrderQty As Boolean ' TJS 05/07/12

        bNoRestockDate = False
        ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221.TableName, _
            "ReadInventoryAmazonTagDetailTemplateView_DEV000221", AT_ITEM_CODE, ItemCode, AT_TAG_LOCATION, AMAZON_INVENTORY_TAG_LOCATION}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)

        XMLUpdate = New XElement("Message")
        XMLUpdate.Add(New XElement("OperationType", OperationType))
        xmlProductNode = New XElement("Inventory")
        If ActiveAmazonSettings.ISItemIDField = "ItemName" Then ' TJS 05/07/12
            xmlProductNode.Add(New XElement("SKU", ImportExportStatusDataset.InventoryItem(0).ItemName)) ' TJS 05/07/12
        Else
            xmlProductNode.Add(New XElement("SKU", ItemCode))
        End If

        ' start of code replaced TJS 05/07/12
        bQtyUpdateRequired = False
        bGetOnOrderQty = False
        ' get current total quantity available
        decItemQtyAvailable = CDec(ImportExportStatusFacade.GetField("SELECT ISNULL(SUM(UnitsAvailable), 0) AS TotalAvailable FROM InventoryStockTotal WHERE ItemCode = '" & ItemCode & "'", System.Data.CommandType.Text, Nothing))
        ' and values when last published
        If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsTotalQtyWhenLastPublished_DEV000221Null Then
            decItemTotalQtyWhenLastPublished = ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).TotalQtyWhenLastPublished_DEV000221
        Else
            decItemTotalQtyWhenLastPublished = 0
        End If
        If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null Then
            decItemQtyLastPublished = ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyLastPublished_DEV000221
        Else
            decItemQtyLastPublished = 0
        End If

        ' what is publishing basis ?
        If ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
            iQtyToPublish = 0
        Else
            Select Case ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyPublishingType_DEV000221
                Case "Fixed"
                    ' always send update as orders may have changed value on source site
                    iQtyToPublish = CInt(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                    If iQtyToPublish < 0 Then
                        iQtyToPublish = 0
                    End If
                    bQtyUpdateRequired = True

                Case "Percent"
                    ' calculate value to be published based on percentage of available qty
                    iQtyToPublish = CInt((decItemQtyAvailable * ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyPublishingValue_DEV000221) / 100) ' TJS 15/03/13
                    If iQtyToPublish < 0 Then
                        iQtyToPublish = 0
                    End If
                    ' is value to publish positive and last value was null or 0 ?
                    If (ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null OrElse _
                        ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyLastPublished_DEV000221 = 0) And iQtyToPublish > 0 Then
                        ' yes, send value
                        bQtyUpdateRequired = True

                    ElseIf (Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                        ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyLastPublished_DEV000221 > 0) And iQtyToPublish = 0 Then
                        ' no, quantity now 0 and was previously positive, send value
                        bQtyUpdateRequired = True
                        bGetOnOrderQty = True

                    ElseIf (Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                        ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyLastPublished_DEV000221 < 10) Or iQtyToPublish < 10 Then
                        ' either new or old value less than 10, send value to make sure we don't show 0 stock due to orders on source 
                        bQtyUpdateRequired = True

                    ElseIf decItemQtyAvailable > (decItemTotalQtyWhenLastPublished * 1.1) Or decItemQtyAvailable < (decItemTotalQtyWhenLastPublished * 0.9) Then
                        ' total available stock has changed by more then 10% since last update send, send value
                        bQtyUpdateRequired = True

                    End If
                    If bGetOnOrderQty Then
                        strTemp = ImportExportStatusFacade.GetField("SELECT SUM(UnitsOnOrder) FROM dbo.InventoryStockTotal WHERE ItemCode = '" & _
                            ItemCode & "'", CommandType.Text, Nothing)
                        If strTemp <> "" Then
                            iQtyOnOrder = CInt(strTemp)
                        Else
                            iQtyOnOrder = 0
                        End If
                        ' calculate value to be published based on percentage of available qty
                        iQtyOnOrder = CInt(iQtyOnOrder * ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                        If iQtyOnOrder < 0 Then
                            iQtyOnOrder = 0
                        End If

                    End If

                Case Else
                    iQtyToPublish = 0
            End Select

        End If
        If bQtyUpdateRequired Then
            xmlProductNode.Add(New XElement("Quantity", iQtyToPublish))
            If iQtyToPublish = 0 And iQtyOnOrder > 0 Then
                ' on order, check if Restock date available
                strItemDueDates = ImportExportStatusFacade.GetRows(New String() {"CONVERT(VARCHAR(19), DueDate, 120)", "CONVERT(VARCHAR(19), RevisedDueDate, 120)"}, "SupplierPurchaseOrderDetail", "ItemCode = '" & ItemCode & "' AND QuantityOrdered > (QuantityReceived + QuantityReserved)", False)
                bNoRestockDate = True
                For Each ItemDueDate As String() In strItemDueDates
                    If Not String.IsNullOrEmpty(ItemDueDate(1)) Then
                        xmlProductNode.Add(New XElement("RestockDate", ItemDueDate(1).Substring(0, 10) & "T" & ItemDueDate(1).Substring(10, 8) & "-00:00"))
                        Exit For

                    ElseIf Not String.IsNullOrEmpty(ItemDueDate(0)) Then
                        xmlProductNode.Add(New XElement("RestockDate", ItemDueDate(0).Substring(0, 10) & "T" & ItemDueDate(0).Substring(10, 8) & "-00:00"))
                        Exit For
                    End If
                Next
            End If
            XMLUpdate.Add(xmlProductNode)
        End If
        ' end of code replaced TJS 05/07/12

    End Sub

    Private Sub CreateAmazonPricingFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer, _
     ByVal ItemCode As String, ByRef XMLUpdate As XElement)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/09/09 | TJS             | 2009.3.07 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to use ISItemIDField setting
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlProductNode As XElement, xmlSaleNode As XElement, xmlPriceNode As XElement

        XMLUpdate = New XElement("Message")
        xmlProductNode = New XElement("Price")
        If ActiveAmazonSettings.ISItemIDField = "ItemName" Then ' TJS 05/07/12
            xmlProductNode.Add(New XElement("SKU", ImportExportStatusDataset.InventoryItem(0).ItemName)) ' TJS 05/07/12
        Else
            xmlProductNode.Add(New XElement("SKU", ItemCode))
        End If
        xmlPriceNode = New XElement("StandardPrice")
        Select Case ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221
            Case ".co.uk"
                xmlPriceNode.SetAttributeValue("currency", "GBP")
            Case Else
                xmlPriceNode.SetAttributeValue("currency", "USD")
        End Select
        xmlPriceNode.Value = Format(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SellingPrice_DEV000221, "0.00")
        xmlProductNode.Add(xmlPriceNode)
        If Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsSaleStartDate_DEV000221Null And _
            Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsSaleEndDate_DEV000221Null And _
            Not ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).IsSalePrice_DEV000221Null Then
            xmlSaleNode = New XElement("Sale")
            xmlSaleNode.Add(New XElement("StartDate", CreateAmazonXMLDate(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SaleStartDate_DEV000221)))
            xmlSaleNode.Add(New XElement("EndDate", CreateAmazonXMLDate(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SaleEndDate_DEV000221)))
            xmlPriceNode = New XElement("SalePrice")
            Select Case ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221
                Case ".co.uk"
                    xmlPriceNode.SetAttributeValue("currency", "GBP")
                Case Else
                    xmlPriceNode.SetAttributeValue("currency", "USD")
            End Select
            xmlPriceNode.Value = Format(ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SalePrice_DEV000221, "0.00")
            xmlSaleNode.Add(xmlPriceNode)
        End If
        XMLUpdate.Add(xmlProductNode)

    End Sub

    Private Function CreateAmazonRelationshipFile(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
     ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
     ByVal ActiveSource As SourceSettings, ByVal ActiveAmazonSettings As AmazonSettings, ByRef RowID As Integer, _
     ByVal ItemCode As String, ByRef XMLUpdate As XElement) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/09/09 | TJS             | 2009.3.07 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to use ISItemIDField setting
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim xmlProductNode As XElement, xmlRelationNode As XElement
        Dim strTemp As String, iLoop As Integer

        If ImportExportStatusDataset.InventoryItem(0).ItemType = "Matrix Group" Then
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryMatrixItemsToPublishOnAmazon_DEV000221.TableName, _
                "ReadInventoryMatrixItemsToPublishOnAmazon_DEV000221", AT_STORE_MERCHANT_ID, ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).MerchantID_DEV000221, _
                AT_SITE_CODE, ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, AT_MATRIX_GROUP_ITEM_CODE, ItemCode}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)

        ElseIf ImportExportStatusDataset.InventoryItem(0).ItemType = "Matrix Item" Then
            strTemp = ImportExportStatusFacade.GetField("SELECT MatrixGroupItemCode FROM dbo.InventoryMatrixItemsToPublishOnAmazon_DEV000221 WHERE MatrixItemCode = '" & _
                ItemCode & "'", CommandType.Text, Nothing)
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryMatrixItemsToPublishOnAmazon_DEV000221.TableName, _
                "ReadInventoryMatrixItemsToPublishOnAmazon_DEV000221", AT_STORE_MERCHANT_ID, ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).MerchantID_DEV000221, _
                AT_SITE_CODE, ImportExportStatusDataset.InventoryAmazonDetails_DEV000221(0).SiteCode_DEV000221, AT_MATRIX_GROUP_ITEM_CODE, strTemp}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)

        End If

        If ImportExportStatusDataset.InventoryMatrixItemsToPublishOnAmazon_DEV000221.Count > 0 Then
            XMLUpdate = New XElement("Message")
            xmlProductNode = New XElement("Relationship")
            If ActiveAmazonSettings.ISItemIDField = "ItemName" Then ' TJS 05/07/12
                xmlProductNode.Add(New XElement("ParentSKU", ImportExportStatusDataset.InventoryItem(0).ItemName)) ' TJS 05/07/12
            Else
                xmlProductNode.Add(New XElement("ParentSKU", ItemCode))
            End If
            For iLoop = 0 To ImportExportStatusDataset.InventoryMatrixItemsToPublishOnAmazon_DEV000221.Count - 1
                xmlRelationNode = New XElement("Relation")
                If ActiveAmazonSettings.ISItemIDField = "ItemName" Then ' TJS 05/07/12
                    strTemp = ImportExportStatusFacade.GetField("ItemName", "InventoryItem", "ItemCode = '" & ImportExportStatusDataset.InventoryMatrixItemsToPublishOnAmazon_DEV000221(iLoop).MatrixItemCode & "'") ' TJS 05/07/12
                    xmlRelationNode.Add(New XElement("SKU", strTemp)) ' TJS 05/07/12
                Else
                    xmlRelationNode.Add(New XElement("SKU", ImportExportStatusDataset.InventoryMatrixItemsToPublishOnAmazon_DEV000221(iLoop).MatrixItemCode))
                End If
                xmlRelationNode.Add(New XElement("Type", "Variation"))
                xmlProductNode.Add(xmlRelationNode)
            Next

            XMLUpdate.Add(xmlProductNode)

            Return True
        Else
            Return False
        End If


    End Function

    Private Function AddAmazonTagDetails(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal TargetNode As XElement) As Integer ' TJS 05/11/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/09/09 | TJS             | 2009.3.07 | Function added
        ' 05/11/09 | TJS             | 2009.3.09 | Added XMLUpdate as parameter
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iNodeAddedCount As Integer

        iNodeAddedCount = 0
        For iLoop = 0 To ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221.Count - 1
            If AddAmazonTagItem(ImportExportStatusFacade, ImportExportStatusDataset, TargetNode, iLoop) Then ' TJS 05/11/09
                iNodeAddedCount = iNodeAddedCount + 1
            End If
        Next
        Return iNodeAddedCount

    End Function

    Private Function AddAmazonTagItem(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal TargetNode As XElement, ByVal RowPosn As Integer) As Boolean ' TJS 05/11/09
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/09/09 | TJS             | 2009.3.07 | Function added
        ' 05/11/09 | TJS             | 2009.3.09 | Added XMLUpdate as parameter
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strTagName As String ' TJS 05/11/09

        If Not ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).IsTagTextValue_DEV000221Null Then
            If ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).TagTextValue_DEV000221 <> "" Then
                If ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).IsTagXMLName_DEV000221Null Then ' TJS 05/11/09
                    strTagName = ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).TagName_DEV000221 ' TJS 05/11/09
                Else
                    strTagName = ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).TagXMLName_DEV000221 ' TJS 05/11/09
                End If
                Select Case ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).TagDataType_DEV000221
                    Case "Date", "DateTime"
                        TargetNode.Add(New XElement(strTagName, CreateAmazonXMLDate(ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).TagDateValue_DEV000221))) ' TJS 05/11/09
                        Return True

                    Case "Integer"
                    Case "Numeric"
                    Case "Currency"

                    Case "Boolean"
                        TargetNode.Add(New XElement(strTagName, ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).TagTextValue_DEV000221.ToLower)) ' TJS 05/11/09
                        Return True

                    Case Else
                        ' must be text 
                        TargetNode.Add(New XElement(strTagName, ImportExportStatusDataset.InventoryAmazonTagDetailTemplateView_DEV000221(RowPosn).TagTextValue_DEV000221)) ' TJS 05/11/09
                        Return True
                End Select
            End If
        End If
        Return False

    End Function

    Private Function CreateAmazonXMLDate(ByVal DateToUse As Date) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/06/10 | TJS             | 2010.0.07 | Modified to cater for day, month etc values less than 10
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strAmazonXMLDate As String

        strAmazonXMLDate = DateToUse.Year & "-" & Right("00" & DateToUse.Month, 2) & "-" & Right("00" & DateToUse.Day, 2) ' TJS 13/06/10
        strAmazonXMLDate = strAmazonXMLDate & "T" & Right("00" & DateToUse.Hour, 2) & ":" & Right("00" & DateToUse.Minute, 2) ' TJS 13/06/10
        strAmazonXMLDate = strAmazonXMLDate & ":" & Right("00" & DateToUse.Second, 2) & "-00:00" ' TJS 13/06/10
        Return strAmazonXMLDate

    End Function
End Module
