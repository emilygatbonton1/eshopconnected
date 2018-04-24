' eShopCONNECT for Connected Business - Windows Service
' Module: ThreeDCartInventoryUpdate.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 20 November 2013

Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports System.IO
Imports System.Net

Module ThreeDCartInventoryUpdate

    Public Sub Do3DCartInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal Active3DCartSettings As ThreeDCartSettings, ByRef RowID As Integer, ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iOrigRowID As Integer, iQtyToPublish As Integer, decItemQtyAvailable As Decimal, decItemTotalQtyWhenLastPublished As Decimal
        Dim decItemQtyLastPublished As Decimal
        Dim bQtyUpdateRequired As Boolean, bInhibitWebPosts As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")

            iOrigRowID = RowID
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
                    "ReadInventoryItem", AT_ITEM_CODE, ItemCode},
                New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, "ReadInventoryItemDescription", AT_ITEM_CODE, ItemCode, _
                    AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
                New String() {ImportExportStatusDataset.Inventory3DCartDetails_DEV000221.TableName, "ReadInventory3DCartDetails_DEV000221", AT_ITEM_CODE, ItemCode}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.InventoryItem.Count > 0 And ImportExportStatusDataset.Inventory3DCartDetails_DEV000221.Count > 0 Then
                ' ignore any records already marked as complete since Matrix Items will be processed as part of new Matrix Groups and may not be in sequence
                If Not ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 Then
                    Select Case ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221
                        Case "100" ' New Item

                        Case "105", "205" ' New/Updated 3DCart Tag value

                        Case "200" ' Updated Item

                        Case "300" ' Deleted Item
                            ' mark row as complete
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iOrigRowID).ActionComplete_DEV000221 = True

                        Case "1000" ' Stock Quantity changed
                            bQtyUpdateRequired = False
                            ' get current total quantity available
                            decItemQtyAvailable = CDec(ImportExportStatusFacade.GetField("SELECT ISNULL(SUM(UnitsAvailable), 0) AS TotalAvailable FROM InventoryStockTotal WHERE ItemCode = '" & ItemCode & "'", System.Data.CommandType.Text, Nothing))
                            ' and values when last published
                            If Not ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).IsTotalQtyWhenLastPublished_DEV000221Null Then
                                decItemTotalQtyWhenLastPublished = ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).TotalQtyWhenLastPublished_DEV000221
                            Else
                                decItemTotalQtyWhenLastPublished = 0
                            End If
                            If Not ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null Then
                                decItemQtyLastPublished = ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).QtyLastPublished_DEV000221
                            Else
                                decItemQtyLastPublished = 0
                            End If

                            ' what is publishing basis ?
                            If ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                                iQtyToPublish = 0
                            Else
                                Select Case ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).QtyPublishingType_DEV000221
                                    Case "Fixed"
                                        ' always send update as orders may have changed value on source site
                                        iQtyToPublish = CInt(ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                                        If iQtyToPublish < 0 Then
                                            iQtyToPublish = 0
                                        End If
                                        bQtyUpdateRequired = True

                                    Case "Percent"
                                        ' calculate value to be published based on percentage of available qty
                                        iQtyToPublish = CInt((decItemQtyAvailable * ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).QtyPublishingValue_DEV000221) / 100)
                                        If iQtyToPublish < 0 Then
                                            iQtyToPublish = 0
                                        End If
                                        ' is value to publish positive and last value was null or 0 ?
                                        If (ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null OrElse _
                                            ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).QtyLastPublished_DEV000221 = 0) And iQtyToPublish > 0 Then
                                            ' yes, send value
                                            bQtyUpdateRequired = True

                                        ElseIf (Not ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                            ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).QtyLastPublished_DEV000221 > 0) And iQtyToPublish = 0 Then
                                            ' no, quantity now 0 and was previously positive, send value
                                            bQtyUpdateRequired = True

                                        ElseIf (Not ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                                            ImportExportStatusDataset.Inventory3DCartDetails_DEV000221(0).QtyLastPublished_DEV000221 < 10) Or iQtyToPublish < 10 Then
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
                                    If Send3DCartInventoryQty(ImportExportStatusFacade, ImportExportStatusDataset, ActiveSource, Active3DCartSettings, RowID, ImportExportStatusDataset.InventoryItem(0).ItemName, iQtyToPublish) Then
                                        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
                                    End If
                                Else
                                    m_ImportExportConfigFacade.WriteLogProgressRecord("Inhibited from sending updates to 3DCart")
                                End If
                            Else
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
                            End If
                            ' check for any additional Stock Quantity changed records and mark them as complete
                            For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                                If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                    ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = Active3DCartSettings.StoreID Then
                                    If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).InventoryStatus_DEV000221 = "1000" Then
                                        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ActionComplete_DEV000221 = Not bInhibitWebPosts
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

                    End Select
                End If

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Do3DCartInventoryUpdate", "No Inventory Item found for " & ItemCode)
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "Do3DCartInventoryUpdate", ex)

        End Try

    End Sub

    Private Function Send3DCartInventoryQty(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
        ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
        ByVal ActiveSource As SourceSettings, ByVal Active3DCartSettings As ThreeDCartSettings, ByRef RowID As Integer, _
        ByVal ProductID As String, ByVal QtyToPublish As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/11/13 | TJS             | 2013.4.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing
        Dim byteData() As Byte, reader As StreamReader, XMLResponse As XDocument
        Dim strSubmit As String, strReturn As String
        Dim bXMLError As Boolean

        strSubmit = "<?xml version=""1.0"" encoding=""utf-8""?>"
        strSubmit = strSubmit & "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSubmit = strSubmit & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">"
        strSubmit = strSubmit & "<soap:Body><updateProductInventory xmlns=""http://3dcart.com/"">"
        strSubmit = strSubmit & "<storeUrl>" & Active3DCartSettings.StoreURL & "</storeUrl>"
        strSubmit = strSubmit & "<userKey>" & Active3DCartSettings.UserKey & "</userKey>"
        strSubmit = strSubmit & "<productId>" & ProductID & "</productId>"
        strSubmit = strSubmit & "<quantity>" & QtyToPublish.ToString & "</quantity>"
        strSubmit = strSubmit & "<replaceStock>true</replaceStock>"
        strSubmit = strSubmit & "<callBackURL></callBackURL>"
        strSubmit = strSubmit & "</updateProductInventory></soap:Body></soap:Envelope>"
        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 20 ' 20 is UpdateProductStockQty
        ' save XML ready for sending
        ImportExportStatusDataset.LerrynImportExportActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = strSubmit

        Try
            WebSubmit = System.Net.WebRequest.Create(THREE_D_CART_WEB_SERVICES_URL)
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "text/xml; charset=utf-8"
            WebSubmit.ContentLength = strSubmit.Length
            WebSubmit.Timeout = 30000

            byteData = UTF8Encoding.UTF8.GetBytes(strSubmit)

            ' send to 3DCart
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            strReturn = reader.ReadToEnd()

            If strReturn <> "" Then
                If strReturn.Length > 38 Then
                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Or strReturn.Trim.Substring(0, 14).ToLower = "<soap:envelope" Then
                        bXMLError = False
                        Try
                            ' had difficulty getting XPath to read XML with this name space present so remove it
                            XMLResponse = XDocument.Parse(strReturn.Replace(" xmlns=""http://3dcart.com/""", ""))

                        Catch ex As Exception
                            m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartInventoryQty", "updateProductInventory XML from " & THREE_D_CART_WEB_SERVICES_URL & " for " & Active3DCartSettings.StoreID.ToString & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, ""))
                            bXMLError = True

                        End Try

                        ' did XML load correctly
                        If Not bXMLError Then
                            ' yes
                            ' did we get an error ?
                            If m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "UpdateInventoryResponse/NewInventory") = QtyToPublish.ToString Then
                                ' no, mark as completed
                                Return True

                            Else
                                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartInventoryQty", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & _
                                    Active3DCartSettings.StoreID & " was " & m_ImportExportConfigFacade.GetXMLElementText(XMLResponse, "soap:Envelope/soap:Body/getOrderResponse/getOrderResult/Error"), _
                                    strSubmit.Replace(Active3DCartSettings.UserKey, "********"))
                            End If
                        End If
                    Else
                        m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartInventoryQty", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " for " & Active3DCartSettings.StoreID.ToString & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                    End If

                Else
                    m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartInventoryQty", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
                End If

            Else
                m_ImportExportConfigFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "Send3DCartInventoryQty", "Response from " & THREE_D_CART_WEB_SERVICES_URL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>", strReturn)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "Send3DCartInventoryQty", ex)

        End Try
        Return False

    End Function

End Module
