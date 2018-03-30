' eShopCONNECT for Connected Business - Windows Service
' Module: eBayInventoryUpdate.vb
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
' Updated 02 August 2013

Imports System.IO
Imports System.Xml.Linq
Imports System.Xml.XPath
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.SourceConfig
Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const

Module eBayInventoryUpdate

    Private eBayConnection As Lerryn.Facade.ImportExport.eBayXMLConnector
    Private strInventoryElements As String()
    Public bEBayInventoryUpdateConnected As Boolean

    Public Sub DoEBayInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveEBaySettings As eBaySettings, ByRef RowID As Integer, ByVal ItemCode As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strUpdateXML As String
        Dim decItemQtyAvailable As Decimal, decItemTotalQtyWhenLastPublished As Decimal, decItemQtyLastPublished As Decimal
        Dim iQtyToPublish As Integer
        Dim bQtyUpdateRequired As Boolean, bInhibitWebPosts As Boolean

        Try
            ' check registry for inhibit web post setting (prevents sending posts during testing)
            bInhibitWebPosts = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")
            ' yes, check for inventory updates
            ImportExportStatusFacade.LoadDataSet(New String()() {New String() {ImportExportStatusDataset.InventoryItem.TableName, _
                "ReadInventoryItem", AT_ITEM_CODE, ItemCode}, New String() {ImportExportStatusDataset.InventoryItemDescription.TableName, _
                "ReadInventoryItemDescription", AT_ITEM_CODE, ItemCode, AT_LANGUAGE_CODE, ImportExportStatusFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE)}, _
                New String() {ImportExportStatusDataset.InventoryEBayDetails_DEV000221.TableName, "ReadInventoryEBayDetails_DEV000221", AT_ITEM_CODE, ItemCode}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
            If ImportExportStatusDataset.InventoryItem.Count > 0 And ImportExportStatusDataset.InventoryEBayDetails_DEV000221.Count > 0 Then
                Select Case ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).InventoryStatus_DEV000221
                    Case "100" ' New Item




                    Case "1000" ' Stock Quantity changed
                        'bQtyUpdateRequired = False
                        '' get current total quantity available
                        'decItemQtyAvailable = CDec(ImportExportStatusFacade.GetField("SELECT ISNULL(SUM(UnitsAvailable), 0) AS TotalAvailable FROM InventoryStockTotal WHERE ItemCode = '" & ItemCode & "'", System.Data.CommandType.Text, Nothing))
                        '' and values when last published
                        'If Not ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).IsTotalQtyWhenLastPublished_DEV000221Null Then
                        '    decItemTotalQtyWhenLastPublished = ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).TotalQtyWhenLastPublished_DEV000221
                        'Else
                        '    decItemTotalQtyWhenLastPublished = 0
                        'End If
                        'If Not ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null Then
                        '    decItemQtyLastPublished = ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).QtyLastPublished_DEV000221
                        'Else
                        '    decItemQtyLastPublished = 0
                        'End If

                        '' what is publishing basis ?
                        'If ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).IsQtyPublishingType_DEV000221Null Then
                        '    iQtyToPublish = 0
                        'Else
                        '    Select Case ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).QtyPublishingType_DEV000221
                        '        Case "Fixed"
                        '            ' always send update as orders may have changed value on source site
                        '            iQtyToPublish = CInt(ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).QtyPublishingValue_DEV000221)
                        '            If iQtyToPublish < 0 Then
                        '                iQtyToPublish = 0
                        '            End If
                        '            bQtyUpdateRequired = True

                        '        Case "Percent"
                        '            ' calculate value to be published based on percentage of available qty
                        '            iQtyToPublish = CInt((decItemQtyAvailable * ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).QtyPublishingValue_DEV000221) / 100)
                        '            If iQtyToPublish < 0 Then
                        '                iQtyToPublish = 0
                        '            End If
                        '            ' is value to publish positive and last value was null or 0 ?
                        '            If (ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null OrElse _
                        '                ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).QtyLastPublished_DEV000221 = 0) And iQtyToPublish > 0 Then
                        '                ' yes, send value
                        '                bQtyUpdateRequired = True

                        '            ElseIf (Not ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                        '                ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).QtyLastPublished_DEV000221 > 0) And iQtyToPublish = 0 Then
                        '                ' no, quantity now 0 and was previously positive, send value
                        '                bQtyUpdateRequired = True

                        '            ElseIf (Not ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).IsQtyLastPublished_DEV000221Null AndAlso _
                        '                ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).QtyLastPublished_DEV000221 < 10) Or iQtyToPublish < 10 Then
                        '                ' either new or old value less than 10, send value to make sure we don't show 0 stock due to orders on source 
                        '                bQtyUpdateRequired = True

                        '            ElseIf decItemQtyAvailable > (decItemTotalQtyWhenLastPublished * 1.1) Or decItemQtyAvailable < (decItemTotalQtyWhenLastPublished * 0.9) Then
                        '                ' total available stock has changed by more then 10% since last update send, send value
                        '                bQtyUpdateRequired = True

                        '            End If

                        '        Case Else
                        '            iQtyToPublish = 0
                        '    End Select
                        'End If
                        If bQtyUpdateRequired Then
                            strUpdateXML = "<InventoryStatus><ItemID>" & ImportExportStatusDataset.InventoryEBayDetails_DEV000221(0).EBayItemID_DEV000221 & "</ItemID><Quantity>" & iQtyToPublish & "</Quantity></InventoryStatus>"
                            'save XML ready for sending
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221 = strUpdateXML
                            ' set message type and mark record as having XML to send
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLMessageType_DEV000221 = 20 ' 20 is UpdateProductStockQty
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).XMLToSend_DEV000221 = True

                        Else
                            ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True
                        End If
                        ' check for any additional Stock Quantity changed records and mark them as complete
                        For iCheckLoop = RowID + 1 To ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.Count - 1
                            If ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).ItemCode_DEV000221 = ItemCode And _
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).SourceCode_DEV000221 = ActiveSource.SourceCode And _
                                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(iCheckLoop).StoreMerchantID_DEV000221 = ActiveEBaySettings.eBayCountry.ToString Then
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

            ElseIf ImportExportStatusDataset.InventoryItem.Count > 0 Then
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoEBayInventoryUpdate", "No Inventory Item found for " & ItemCode)
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            Else
                ImportExportStatusFacade.SendSourceErrorEmail(ActiveSource.XMLConfig, "DoEBayInventoryUpdate", "No Inventory eBay Details found for " & ItemCode)
                ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "DoEBayInventoryUpdate", ex)

        End Try

    End Sub

    Public Sub StartEBayInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveEBaySettings As eBaySettings, ByVal EBayXMLMessageType As Integer)
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
        ReDim strInventoryElements(0)

    End Sub

    Public Sub AddToEBayInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveEBaySettings As eBaySettings, ByRef RowID As Integer)
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

        Dim strMessageXML As String

        strMessageXML = ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionXMLFile_DEV000221
        ' does mesage XML end with cr lf ?
        If Right(strMessageXML, 2) = vbCrLf Then
            ' yes, remove cr lf
            strMessageXML = Left(strMessageXML, Len(strMessageXML) - 2)
        End If
        If strInventoryElements(strInventoryElements.Length - 1) <> "" Then
            ReDim Preserve strInventoryElements(strInventoryElements.Length)
        End If
        strInventoryElements(strInventoryElements.Length - 1) = strMessageXML
        ' mark action record as XML Sent (change is committed to DB when status file successfully saved in SendEBayInventoryUpdate)
        ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221(RowID).ActionComplete_DEV000221 = True

    End Sub

    Public Sub SendEBayInventoryUpdate(ByRef ImportExportStatusFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade, _
    ByRef ImportExportStatusDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
    ByVal ActiveSource As SourceSettings, ByVal ActiveEBaySettings As eBaySettings)
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

        OpenEBayStatusConnection(ActiveSource, ActiveEBaySettings)
        If eBayConnection.SendItemQuantityUpdates(strInventoryElements) Then
            ImportExportStatusFacade.UpdateDataSet(New String()() {New String() {ImportExportStatusDataset.LerrynImportExportInventoryActionStatus_DEV000221.TableName, _
            "CreateLerrynImportExportInventoryActionStatus_DEV000221", "UpdateLerrynImportExportInventoryActionStatus_DEV000221", _
            "DeleteLerrynImportExportInventoryActionStatus_DEV000221"}}, Interprise.Framework.Base.Shared.TransactionType.None, "Update eBay Inventory Action Status", False)
        End If

    End Sub

    Public Sub OpenEBayInventoryConnection(ByVal ActiveSource As SourceSettings, ByVal ActiveEBaySettings As eBaySettings)
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

        Dim bUseEBaySandbox As Boolean

        Try
            If eBayConnection Is Nothing Then
                bUseEBaySandbox = (m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "UseEBaySandbox", "NO").ToUpper = "YES")
                eBayConnection = New Lerryn.Facade.ImportExport.eBayXMLConnector(m_ImportExportConfigFacade, ActiveEBaySettings, bUseEBaySandbox)
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "OpenEBayInventoryConnection", ex)

        End Try

    End Sub

    Public Sub CloseEBayInventoryConnection(ByVal ActiveSource As SourceSettings)
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

        Try
            eBayConnection = Nothing

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(ActiveSource.XMLConfig, "CloseEBayInventoryConnection", ex)

        End Try

    End Sub

End Module
