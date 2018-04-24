' eShopCONNECT for Connected Business - Windows Service
' Module: AmazonImport.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 22 May 2014

Imports System.IO ' TJS 15/12/09
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Module AmazonImport

    Private eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportProcessFacade
    Private decItemTotal As Decimal
    Private decItemTaxTotal As Decimal
    Private decItemShippingTotal As Decimal

    Public Function ProcessAmazonXML(ByVal SourceConfig As SourceSettings, ByVal AmazonConfig As AmazonSettings, ByVal strInputXML As String, _
        ByVal strFileID As String, ByRef rowOrigAmazonFile As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonFiles_DEV000221Row, _
        ByRef RowOrderToRetry As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportSourceOrdersToRetry_DEV000221Row) As Boolean ' TJS 15/12/09 TJS 19/08/10 TJS 26/06/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Processes files received from Amazon
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 14/02/09 | TJS             | 2009.1.08 | Function added
        ' 10/03/09 | TJS             | 2009.1.09 | Added checks for no orders and no items, plus corrected other errors and omissions
        ' 17/03/09 | TJS             | 2009.1.10 | Modified to correctly extract amazon Order ID
        ' 06/10/09 | TJS             | 2009.3.07 | Added code to process Processing Report Files 
        '                                        | and to cater for reprocessing records
        ' 16/10/09 | TJS             | 2009.3.08 | Modified to logout even if error occurs
        ' 15/12/09 | TJS             | 2009.3.09 | Changed strFileName parameter to strFileID and added parameter rowOrigAmazonFile for
        '                                        | comaptibility with direct amazon connection and added handling of Prices including tax
        '                                        | Also added logging of received files
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for QuoteToConvert parameter on XMLOrderImport
        '                                        | and CustomSKUProcessing plus Source COde constants
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 25/04/11 | TJS             | 2011.0.12 | Modified to check for and use relevant website record
        ' 09/07/11 | TJS             | 2011.1.00 | Corrected call to XMLOrderImport
        ' 26/10/11 | TJS             | 2011.1.xx | Corrected setting of source tax values and codes
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 and for IS 6
        ' 26/03/12 | TJS             | 2011.2.11 | Removed facade signout as now handled in ServiceMain
        ' 10/06/12 | TJS             | 2012.1.05 | Corrected error responses and added Error Notification object to simplify facade login/logout
        ' 05/07/12 | TJS             | 2012.1.08 | Added processing for Amazon Settlement reports
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 26/06/13 | TJS             | 2013.1.23 | Added code to retry orders which failed to import and inhibited 
        '                                        | Settlement processing unless registry key set
        ' 05/07/13 | TJS             | 2013.1.26 | Modified to exit loop and skip other orders in file after processing an order for a retry
        ' 16/07/13 | TJS             | 2013.1.29 | Added missing Amazon source payment node to order
        ' 02/08/13 | TJS             | 2013.2.01 | Modified to abort all activity during system shutdown
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowAmazonSettlement As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221Row ' TJS 05/07/12
        Dim rowAmazonSettlementDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221Row ' TJS 05/07/12
        Dim rowAmazonSettlementDetails2 As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221Row ' TJS 05/07/12
        Dim XMLGenericOrder As XDocument, XMLRequest As XDocument, XMLTemp As XDocument, XMLItemFeeTemp As XDocument ' TJS 05/07/12
        Dim XMLItemTemp As XDocument, XMLItemComponentTemp As XDocument ' TJS 10/03/09
        Dim XMLOrderList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemList As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLItemComponentList As System.Collections.Generic.IEnumerable(Of XElement), XMLErrorList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 06/10/09
        Dim XMLAdjustmentList As System.Collections.Generic.IEnumerable(Of XElement), XMLOtherTransactionList As System.Collections.Generic.IEnumerable(Of XElement) ' TJS 05/07/12
        Dim XMLItemFees As System.Collections.Generic.IEnumerable(Of XElement), XMLItemFeeNode As XElement ' TJS 05/07/12
        Dim XMLOrderNode As XElement, XMLItemNode As XElement, XMLOrderSourceNode As XElement, XMLISOrderImportNode As XElement
        Dim XMLItemComponentNode As XElement, XMLOrderTotalsNode As XElement, XMLImportResponseNode As XElement ' TJS 10/03/09
        Dim XMLErrorNode As XElement, XMLOrderResponseNode As XElement, XMLAdjustmentNode As XElement, XMLOtherTransactionNode As XElement ' TJS 06/10/09 TJS 05/07/12
        Dim XMLPaymentDetailsNode As XElement ' TJS 16/07/13
        Dim writer As StreamWriter ' TJS 15/12/09

        Dim strCustomerEmail As String, strISCustomerCode As String, strErrorMessage As String, strTemp As String ' TJS 15/12/09
        Dim sLogFileName As String, strErrorMessageIDs As String, strUpdateSQL As String, strSettlementCode As String ' TJS 15/12/09 TJS 05/07/12
        Dim strAmazonSettlementID As String, strISSalesOrderCode As String, strISItemDetails As String() ' TJS 05/07/12
        Dim bOneOrMoreRecordsfailed As Boolean, bSettlementReconciled As Boolean, bReturnValue As Boolean ' TJS 15/12/09 TJS 16/10/09 TJS 05/07/12
        Dim bMarkOrderForRetry As Boolean, bRetryingOrder As Boolean ' TJS 26/06/13 TJS 05/07/13
        Dim decTotalPrincipal As Decimal, decTotalShipping As Decimal, decTotalSalesTax As Decimal, decTotalFBAPerOrderFees As Decimal ' TJS 05/07/12
        Dim decTotalFBAPerUnitFees As Decimal, decTotalFBAWeightFees As Decimal, decTotalCommission As Decimal ' TJS 05/07/12
        Dim decTotalInboundShipping As Decimal, decTotalSubscriptionFees As Decimal, decTotalOthers As Decimal ' TJS 05/07/12
        Dim decTotalServiceFeesSalesTax As Decimal, iSettlementLineNo As Integer, iRowLoop As Integer, iColumnLoop As Integer ' TJS 05/07/12 TJS 26/06/13

        Try
            bReturnValue = True ' TJS 16/10/09
            bRetryingOrder = False ' TJS 05/07/13
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportProcessFacade(eShopCONNECTDatasetGateway, m_ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 18/03/11 TJS 10/06/12

            ' check XML starts with correct headers
            If Left(Trim(strInputXML.ToLower), 38) = "<?xml version=""1.0"" encoding=""utf-8""?>" Then ' TJS 10/03/09
                ' yes, load into XML cocument
                XMLRequest = XDocument.Parse(Trim(strInputXML))
                ' check module and connector are activated and valid
                If eShopCONNECTFacade.ValidateSource("Amazon eShopCONNECTOR", AMAZON_SOURCE_CODE, "", False) Then ' TJS 17/03/09 TJS 06/10/09 TJS 12/09/10
                    ' yes
                    bOneOrMoreRecordsfailed = False ' TJS 10/03/09
                    Select Case eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_TYPE)
                        Case "OrderReport"
                            ' process orders
                            XMLOrderList = XMLRequest.XPathSelectElements(AMAZON_ORDER_LIST)
                            If XMLOrderList IsNot Nothing Then ' TJS 10/03/09
                                For Each XMLOrderNode In XMLOrderList
                                    XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                                    ' are we re-trying an order ?
                                    If RowOrderToRetry IsNot Nothing Then ' TJS 26/06/13
                                        ' yes, cycle through orders in file to find the one for reprocessing
                                        bRetryingOrder = True ' TJS 05/07/13
                                        If RowOrderToRetry.MerchantOrderID_DEV000221 <> eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID") Then ' TJS 26/06/13
                                            Continue For ' TJS 26/06/13
                                        End If
                                    End If
                                    ' create Generic XML document
                                    XMLGenericOrder = New XDocument
                                    XMLISOrderImportNode = New XElement("eShopCONNECT")
                                    bMarkOrderForRetry = False 'TJS 26/06/13

                                    'SOURCE DETAILS
                                    XMLOrderSourceNode = New XElement("Source")
                                    XMLOrderSourceNode.Add(New XElement("SourceName", "Amazon Order"))
                                    XMLOrderSourceNode.Add(New XElement("SourceCode", AMAZON_SOURCE_CODE))
                                    XMLISOrderImportNode.Add(XMLOrderSourceNode)

                                    ' Amazon doesn't have Customer IDs, they use the email address
                                    strCustomerEmail = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_BILLING_DETAILS & "BuyerEmailAddress")
                                    ' check if customer exists in IS
                                    eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.TableName, _
                                        "ReadCustomerImportExportView_DEV000221", AT_IMPORT_CUSTOMER_ID, strCustomerEmail, AT_IMPORT_SOURCE_ID, AMAZON_SOURCE_CODE}}, _
                                        Interprise.Framework.Base.Shared.ClearType.Specific)
                                    If eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221.Count > 0 Then
                                        strISCustomerCode = eShopCONNECTDatasetGateway.CustomerImportExportView_DEV000221(0).CustomerCode
                                    Else
                                        strISCustomerCode = ""
                                    End If

                                    ' ORDER DETAILS
                                    XMLItemList = XMLTemp.XPathSelectElements(AMAZON_ORDER_ITEM_LIST)
                                    XMLItemNode = XMLItemList(0)
                                    XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                    XMLItemComponentList = XMLItemTemp.XPathSelectElements(AMAZON_ORDER_ITEM_PRICE_COMPONENT_LIST)
                                    XMLItemComponentNode = XMLItemComponentList(0)
                                    XMLItemComponentTemp = XDocument.Parse(XMLItemComponentNode.ToString)

                                    XMLOrderNode = New XElement("Order")
                                    XMLOrderNode.Add(New XElement("CustomerOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonSessionID"))) ' TJS 17/03/09
                                    XMLOrderNode.Add(New XElement("SourceOrderRef", eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID"))) ' TJS 17/03/09
                                    strTemp = eShopCONNECTFacade.GetField("WebSiteCode", "EcommerceSite", "SourceCode_DEV000221 = '" & AMAZON_SOURCE_CODE & "' AND AccountOrInstanceID_DEV000221 = '" & AmazonConfig.MerchantToken.Replace("'", "''") & "'") ' TJS 25/04/11 TJS 02/12/11 TJS 22/03/13
                                    If "" & strTemp <> "" Then ' TJS 25/04/11
                                        XMLOrderNode.Add(New XElement("SourceWebSiteRef", strTemp)) ' TJS 25/04/11
                                    Else
                                        XMLOrderNode.Add(New XElement("SourceWebSiteRef", "Amazon"))
                                    End If
                                    XMLOrderNode.Add(New XElement("SourceMerchantID", eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_MERCHANT_ID)))
                                    If AmazonConfig.PricesAreTaxInclusive Then ' TJS 26/10/11
                                        XMLOrderNode.Add(New XElement("PricesIncludeTax", "Yes")) ' TJS 26/10/11
                                        XMLOrderNode.Add(New XElement("TaxCodeForSourceTax", AmazonConfig.TaxCodeForSourceTax)) ' TJS 26/10/11
                                    End If
                                    ' get Amazon order date (Datae + time) and strip out time
                                    XMLOrderNode.Add(New XElement("OrderDate", eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "OrderDate").Substring(0, 10)))
                                    ' take currency from first item
                                    XMLOrderNode.Add(New XElement("OrderCurrency", eShopCONNECTFacade.GetXMLElementAttribute(XMLItemComponentTemp, AMAZON_ORDER_ITEM_PRICE_COMPONENT_DETAILS & "Amount", "currency"))) ' TJS 10/03/09

                                    ' BILLING DETAILS
                                    XMLOrderNode.Add(BillingDetails(XMLTemp, strISCustomerCode))

                                    ' SHIPPING DETAILS
                                    XMLOrderNode.Add(ShippingDetails(XMLTemp, SourceConfig))

                                    ' ITEM DETAILS
                                    strErrorMessage = ""
                                    decItemTotal = 0 ' TJS 10/03/09
                                    decItemShippingTotal = 0 ' TJS 10/03/09
                                    decItemTaxTotal = 0 ' TJS 10/03/09
                                    If XMLItemList IsNot Nothing Then ' TJS 10/03/09
                                        For Each XMLItemNode In XMLItemList
                                            XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                            XMLOrderNode.Add(ItemDetails(XMLItemTemp, AmazonConfig, strErrorMessage))
                                            If strErrorMessage <> "" Then
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        m_ImportExportConfigFacade.BuildXMLErrorResponseNodeAndEmail("Error", "013", "No Order Items found", _
                                            m_ImportExportConfigFacade.SourceConfig, "ProcessAmazonXML", XMLGenericOrder.ToString) ' TJS 10/03/09 TJS 18/03/11 TJS 10/06/12
                                    End If

                                    ' start of code added TJS 16/07/13
                                    XMLPaymentDetailsNode = New XElement("PaymentDetails")
                                    XMLPaymentDetailsNode.Add(New XElement("PaymentMethod", "Source"))
                                    XMLPaymentDetailsNode.Add(New XElement("PaymentType", AmazonConfig.PaymentType))
                                    XMLPaymentDetailsNode.Add(New XElement("SourcePaymentID", eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID")))
                                    XMLOrderNode.Add(XMLPaymentDetailsNode)
                                    ' end of code added TJS 16/07/13

                                    ' ORDER TOTALS
                                    XMLOrderTotalsNode = New XElement("OrderTotals") ' TJS 10/03/09
                                    XMLOrderTotalsNode.Add(New XElement("SubTotal", Format(decItemTotal, "0.00"))) ' TJS 10/03/09
                                    XMLOrderTotalsNode.Add(New XElement("Shipping", Format(decItemShippingTotal, "0.00"))) ' TJS 10/03/09
                                    XMLOrderTotalsNode.Add(New XElement("Tax", Format(decItemTaxTotal, "0.00"))) ' TJS 10/03/09
                                    XMLOrderTotalsNode.Add(New XElement("Total", Format(decItemTotal + decItemShippingTotal + decItemTaxTotal, "0.00"))) ' TJS 10/03/09
                                    XMLOrderNode.Add(XMLOrderTotalsNode) ' TJS 10/03/09
                                    XMLISOrderImportNode.Add(XMLOrderNode) ' TJS 10/03/09

                                    XMLISOrderImportNode.Add(New XElement("OrderCount", "1")) ' TJS 10/03/09
                                    XMLGenericOrder.Add(XMLISOrderImportNode) ' TJS 10/03/09

                                    strTemp = eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH) ' TJS 15/12/09
                                    If strTemp <> "" Then ' TJS 15/12/09
                                        If strTemp.Substring(strTemp.Length - 1, 1) <> "\" Then ' TJS 15/12/09
                                            strTemp = strTemp & "\" ' TJS 15/12/09
                                        End If
                                        XMLTemp.Save(strTemp & "Amazon_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID") & ".xml") ' TJS 15/12/09
                                        XMLGenericOrder.Save(strTemp & "GenericXML_for_Amazon_Order_" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID") & ".xml") ' TJS 15/12/09
                                    End If

                                    'Generic XML Built, now do call to create order and customer (where customer does not exist)
                                    XMLImportResponseNode = eShopCONNECTFacade.XMLOrderImport(XMLGenericOrder, "") ' TJS 10/03/09 TJS 19/08/10 TJS 09/07/11
                                    XMLOrderResponseNode = XMLImportResponseNode.XPathSelectElement("/ImportResponse/Status") ' TJS 10/03/09
                                    If XMLOrderResponseNode IsNot Nothing Then ' TJS 10/03/09
                                        If XMLOrderResponseNode.Value = "Success" Then ' TJS 10/03/09
                                            ' don't send any response now, new order DB trigger will create response
                                            eShopCONNECTFacade.WriteLogProgressRecord("Amazon Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID") & " successfully imported from Merchant ID " & AmazonConfig.MerchantToken & " as " & XMLImportResponseNode.XPathSelectElement("/ImportResponse/OrderNumber").Value) ' TJS 10/03/09 TJS 17/03/09 TJS 19/08/10 TJS 22/03/13
                                        Else
                                            bOneOrMoreRecordsfailed = True ' TJS 10/03/09
                                            bMarkOrderForRetry = False ' TJS 26/06/13
                                            eShopCONNECTFacade.WriteLogProgressRecord("Amazon Order Ref " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID") & " import from Merchant ID " & AmazonConfig.MerchantToken & " failed") ' TJS 10/03/09 TJS 19/08/10 TJS 22/03/13
                                        End If
                                    Else
                                        bOneOrMoreRecordsfailed = True ' TJS 10/03/09
                                        bMarkOrderForRetry = False ' TJS 26/06/13
                                        eShopCONNECTFacade.SendSourceErrorEmail(eShopCONNECTFacade.SourceConfig, "ProcessAmazonXML", XMLImportResponseNode.ToString, "") ' TJS 10/03/09
                                    End If
                                    ' start of code added TJS 26/06/13
                                    If Not bMarkOrderForRetry Then
                                        If RowOrderToRetry IsNot Nothing Then
                                            RowOrderToRetry.SuccessfullyImported_DEV000221 = True
                                            RowOrderToRetry.RetryCount_DEV000221 = 0
                                        End If
                                    Else
                                        If RowOrderToRetry IsNot Nothing Then
                                            If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                                                RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                            Else
                                                RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                                            End If
                                            RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                                        Else
                                            RowOrderToRetry = m_ImportExportDataset.LerrynImportExportSourceOrdersToRetry_DEV000221.NewLerrynImportExportSourceOrdersToRetry_DEV000221Row()
                                            RowOrderToRetry.SourceCode_DEV000221 = AMAZON_SOURCE_CODE
                                            RowOrderToRetry.StoreMerchantID_DEV000221 = AmazonConfig.MerchantToken
                                            RowOrderToRetry.MerchantOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_ORDER_DETAILS & "AmazonOrderID")
                                            RowOrderToRetry.SourceFileRecordID_DEV000221 = strFileID
                                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                                            RowOrderToRetry.RetryCount_DEV000221 = 6
                                        End If
                                    End If
                                    ' end of code added TJS 26/06/13
                                    If bRetryingOrder Then ' TJS 05/07/13
                                        Exit For ' TJS 05/07/13
                                    Else
                                        RecordOrderIDToRetry(RowOrderToRetry, AmazonConfig.MerchantToken, "Amazon") ' TJS 05/07/13
                                        RowOrderToRetry = Nothing ' TJS 05/07/13
                                    End If
                                    If bShutDownInProgress Then ' TJS 02/08/13
                                        Exit For ' TJS 02/08/13
                                    End If
                                Next
                            Else
                                bOneOrMoreRecordsfailed = True ' TJS 10/03/09
                                m_ImportExportConfigFacade.BuildXMLErrorResponseNodeAndEmail("Error", "013", "No Order records found", _
                                    m_ImportExportConfigFacade.SourceConfig, "ProcessAmazonXML", strInputXML) ' TJS 10/03/09 TJS 10/06/12
                            End If

                            ' start of code added TJS 06/10/09
                        Case "ProcessingReport"
                            ' process report message
                            strErrorMessageIDs = ""

                            XMLErrorList = XMLRequest.XPathSelectElements(AMAZON_REPORT_ERROR_LIST)
                            If XMLErrorList IsNot Nothing Then
                                For Each XMLErrorNode In XMLErrorList
                                    XMLTemp = XDocument.Parse(XMLErrorNode.ToString)
                                    If eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_REPORT_ERROR_DETAILS & "ResultCode") = "Error" Then
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Error message in Amazon Processing Report file - " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_REPORT_ERROR_DETAILS & "ResultDescription"), XMLErrorNode.ToString)
                                        If strErrorMessageIDs <> "" Then
                                            strErrorMessageIDs = strErrorMessageIDs & ", "
                                        End If
                                        strErrorMessageIDs = strErrorMessageIDs & "'" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_REPORT_ERROR_DETAILS & "MessageID") & "'"
                                    Else
                                        m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Warning message in Amazon Processing Report file - " & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_REPORT_ERROR_DETAILS & "ResultDescription"), XMLErrorNode.ToString)
                                    End If
                                Next

                                ' did we find an original message ?
                                If rowOrigAmazonFile IsNot Nothing Then ' TJS 09/07/11
                                    ' yes, what was the original message about ?
                                    Select Case rowOrigAmazonFile.AmazonMessageType_DEV000221 ' TJS 15/12/09
                                        Case "_POST_ORDER_ACKNOWLEDGEMENT_DATA_", "_POST_ORDER_FULFILLMENT_DATA_", "_POST_PAYMENT_ADJUSTMENT_DATA_" ' TJS 15/12/09
                                            ' were any messages sucessful ?
                                            If CInt(eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_REPORT_SUMMARY & "/MessagesSuccessful")) > 0 Then ' TJS 15/12/09
                                                ' yes, mark messages as processed
                                                strUpdateSQL = "UPDATE dbo.LerrynImportExportActionStatus_DEV000221 SET MessageAcknowledged_DEV000221 = 1, " ' TJS 15/12/09
                                                strUpdateSQL = strUpdateSQL & "ErrorReported_DEV000221 = 0 WHERE XMLMessageType_DEV000221 = 1 AND " ' TJS 15/12/09
                                                strUpdateSQL = strUpdateSQL & "SentInFileID_DEV000221 = '" & rowOrigAmazonFile.AmazonDocumentID_DEV000221 ' TJS 15/12/09
                                                strUpdateSQL = strUpdateSQL & "' AND SendInMessageID_DEV000221 IS NOT NULL"
                                                ' did we get any errors ?
                                                If strErrorMessageIDs <> "" Then
                                                    ' exclude those from acknowledged update
                                                    strUpdateSQL = strUpdateSQL & " AND SendInMessageID_DEV000221 NOT IN (" & strErrorMessageIDs & ")"
                                                    eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, strUpdateSQL, Nothing)
                                                    ' and flag errored messages
                                                    strUpdateSQL = "UPDATE dbo.LerrynImportExportActionStatus_DEV000221 SET MessageAcknowledged_DEV000221 = 1, " ' TJS 15/12/09
                                                    strUpdateSQL = strUpdateSQL & "ErrorReported_DEV000221 = 1 WHERE XMLMessageType_DEV000221 = 1 AND " ' TJS 15/12/09
                                                    strUpdateSQL = strUpdateSQL & "SentInFileID_DEV000221 = '" & rowOrigAmazonFile.AmazonDocumentID_DEV000221 ' TJS 15/12/09
                                                    strUpdateSQL = strUpdateSQL & "' AND SendInMessageID_DEV000221 IS NOT NULL AND SendInMessageID_DEV000221 IN ("
                                                    strUpdateSQL = strUpdateSQL & strErrorMessageIDs & ")"
                                                End If

                                            Else
                                                ' no, flag errored messages
                                                strUpdateSQL = "UPDATE dbo.LerrynImportExportActionStatus_DEV000221 SET MessageAcknowledged_DEV000221 = 1, " ' TJS 15/12/09
                                                strUpdateSQL = strUpdateSQL & "ErrorReported_DEV000221 = 1 WHERE XMLMessageType_DEV000221 = 1 AND " ' TJS 15/12/09
                                                strUpdateSQL = strUpdateSQL & "SentInFileID_DEV000221 = '" & rowOrigAmazonFile.AmazonDocumentID_DEV000221 & "'" ' TJS 15/12/09

                                            End If
                                            eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, strUpdateSQL, Nothing)

                                        Case "_POST_PRODUCT_DATA_" ' TJS 15/12/09
                                            ' were any messages sucessful ?
                                            If CInt(eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_REPORT_SUMMARY & "/MessagesSuccessful")) > 0 Then ' TJS 15/12/09
                                                ' yes, mark messages as processed
                                                strUpdateSQL = "UPDATE dbo.LerrynImportExportInventoryActionStatus_DEV000221 SET MessageAcknowledged_DEV000221 = 1, "
                                                strUpdateSQL = strUpdateSQL & "ErrorReported_DEV000221 = 0 WHERE XMLMessageType_DEV000221 = 11 AND "
                                                strUpdateSQL = strUpdateSQL & "SentInFileID_DEV000221 = '" & rowOrigAmazonFile.AmazonDocumentID_DEV000221
                                                strUpdateSQL = strUpdateSQL & "' AND SendInMessageID_DEV000221 IS NOT NULL"
                                                ' did we get any errors ?
                                                If strErrorMessageIDs <> "" Then
                                                    ' exclude those from acknowledged update
                                                    strUpdateSQL = strUpdateSQL & " AND SendInMessageID_DEV000221 NOT IN (" & strErrorMessageIDs & ")"
                                                    eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, strUpdateSQL, Nothing)
                                                    ' and flag errored messages
                                                    strUpdateSQL = "UPDATE dbo.LerrynImportExportInventoryActionStatus_DEV000221 SET MessageAcknowledged_DEV000221 = 1, "
                                                    strUpdateSQL = strUpdateSQL & "ErrorReported_DEV000221 = 0 WHERE XMLMessageType_DEV000221 = 11 AND "
                                                    strUpdateSQL = strUpdateSQL & "SentInFileID_DEV000221 = '" & rowOrigAmazonFile.AmazonDocumentID_DEV000221
                                                    strUpdateSQL = strUpdateSQL & "' AND SendInMessageID_DEV000221 IS NOT NULL AND SendInMessageID_DEV000221 IN ("
                                                    strUpdateSQL = strUpdateSQL & strErrorMessageIDs & ")"
                                                End If

                                            Else
                                                ' no, flag errored messages
                                                strUpdateSQL = "UPDATE dbo.LerrynImportExportInventoryActionStatus_DEV000221 SET MessageAcknowledged_DEV000221 = 1, " ' TJS 15/12/09
                                                strUpdateSQL = strUpdateSQL & "ErrorReported_DEV000221 = 1 WHERE XMLMessageType_DEV000221 = 1 AND " ' TJS 15/12/09
                                                strUpdateSQL = strUpdateSQL & "SentInFileID_DEV000221 = '" & rowOrigAmazonFile.AmazonDocumentID_DEV000221 & "'" ' TJS 15/12/09

                                            End If
                                            eShopCONNECTFacade.ExecuteNonQuery(CommandType.Text, strUpdateSQL, Nothing)

                                        Case "_POST_PRODUCT_IMAGE_DATA_" ' TJS 15/12/09

                                        Case "_POST_INVENTORY_AVAILABILITY_DATA_" ' TJS 15/12/09

                                        Case "_POST_PRODUCT_PRICING_DATA_" ' TJS 15/12/09

                                        Case "_POST_PRODUCT_RELATIONSHIP_DATA_" ' TJS 15/12/09

                                    End Select
                                    strTemp = eShopCONNECTFacade.GetXMLElementText(eShopCONNECTFacade.SourceConfig, SOURCE_CONFIG_XML_FILE_SAVE_PATH) ' TJS 15/12/09
                                    If strTemp <> "" Then ' TJS 15/12/09
                                        If strTemp.Substring(strTemp.Length - 1, 1) <> "\" Then ' TJS 15/12/09
                                            strTemp = strTemp & "\" ' TJS 15/12/09
                                        End If
                                        XMLRequest.Save(strTemp & "Amazon_Report_" & strFileID & ".xml") ' TJS 15/12/09
                                    End If

                                Else
                                    ' write file to manual processing directory
                                    sLogFileName = AmazonConfig.ManualProcessingPath & "Amazon_" & eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_TYPE).ToUpper & "_" & strFileID & ".xml" ' TJS 09/07/11
                                    ' create the status update file
                                    writer = New StreamWriter(sLogFileName, True) ' TJS 09/07/11
                                    ' write the XML to it
                                    writer.WriteLine(strInputXML) ' TJS 09/07/11
                                    ' and close it
                                    writer.Close() ' TJS 09/07/11

                                End If

                            End If
                            ' end of code added TJS 06/10/09

                            ' start of code added TJS 05/07/12
                        Case "SettlementReport"
                            ' has settlement report already been processed ?
                            strAmazonSettlementID = eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_SETTLEMENT_REPORT_DATA & "AmazonSettlementID")
                            strSettlementCode = eShopCONNECTFacade.GetField("SettlementCode_DEV000221", "LerrynImportExportAmazonSettlement_DEV000221", "MerchantID_DEV000221 = '" & _
                                eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_MERCHANT_ID).Replace("'", "''") & "' AND AmazonSettlementID_DEV000221 = '" & strAmazonSettlementID & "'") ' TJS 14/08/12
                            If String.IsNullOrEmpty(strSettlementCode) And m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "ProcessAmazonSettlements", "NO").ToUpper = "YES" Then ' TJS 26/06/13
                                If AmazonConfig.MerchantToken = eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_MERCHANT_ID) Then ' TJS 22/03/13
                                    ' no, create Settlement record
                                    rowAmazonSettlement = eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.NewLerrynImportExportAmazonSettlement_DEV000221Row
                                    rowAmazonSettlement.SettlementCode_DEV000221 = Interprise.Framework.Base.Shared.Const.TEMPORARY_DOCUMENTCODE
                                    rowAmazonSettlement.MerchantID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_MERCHANT_ID)
                                    rowAmazonSettlement.MerchantName_DEV000221 = AmazonConfig.MerchantName
                                    rowAmazonSettlement.AmazonSettlementID_DEV000221 = strAmazonSettlementID
                                    rowAmazonSettlement.SettlementStartDate_DEV000221 = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_SETTLEMENT_REPORT_DATA & "StartDate"))
                                    rowAmazonSettlement.SettlementEndDate_DEV000221 = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_SETTLEMENT_REPORT_DATA & "EndDate"))
                                    rowAmazonSettlement.DepositDate_DEV000221 = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_SETTLEMENT_REPORT_DATA & "DepositDate"))
                                    rowAmazonSettlement.CurrencyCode_DEV000221 = eShopCONNECTFacade.GetXMLElementAttribute(XMLRequest, AMAZON_SETTLEMENT_REPORT_DATA & "TotalAmount", "currency")
                                    rowAmazonSettlement.ExchangeRate_DEV000221 = eShopCONNECTFacade.GetExchangerate(rowAmazonSettlement.CurrencyCode_DEV000221)
                                    rowAmazonSettlement.TotalAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_SETTLEMENT_REPORT_DATA & "TotalAmount"))
                                    rowAmazonSettlement.TotalAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlement.TotalAmountRate_DEV000221 / rowAmazonSettlement.ExchangeRate_DEV000221)
                                    ' initially assume it won't be reconciled
                                    rowAmazonSettlement.Reconciled_DEV000221 = False
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.AddLerrynImportExportAmazonSettlement_DEV000221Row(rowAmazonSettlement)

                                    ' set flag to record non-reconciled detail records 
                                    bSettlementReconciled = True
                                    decTotalPrincipal = 0
                                    decTotalShipping = 0
                                    decTotalSalesTax = 0
                                    decTotalFBAPerOrderFees = 0
                                    decTotalFBAPerUnitFees = 0
                                    decTotalFBAWeightFees = 0
                                    decTotalCommission = 0
                                    decTotalInboundShipping = 0
                                    decTotalSubscriptionFees = 0
                                    decTotalServiceFeesSalesTax = 0
                                    decTotalOthers = 0
                                    iSettlementLineNo = 1

                                    ' now process settlement order records
                                    XMLOrderList = XMLRequest.XPathSelectElements(AMAZON_SETTLEMENT_ORDER_LIST)
                                    If XMLOrderList IsNot Nothing Then
                                        For Each XMLOrderNode In XMLOrderList
                                            XMLTemp = XDocument.Parse(XMLOrderNode.ToString)
                                            XMLItemList = XMLTemp.XPathSelectElements(AMAZON_SETTLEMENT_ORDER_FULFILLMENT_ITEM_LIST)
                                            If XMLItemList IsNot Nothing Then
                                                For Each XMLItemNode In XMLItemList
                                                    XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                                    rowAmazonSettlementDetails = eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.NewLerrynImportExportAmazonSettlementDetail_DEV000221Row
                                                    rowAmazonSettlementDetails.SettlementCode_DEV000221 = rowAmazonSettlement.SettlementCode_DEV000221
                                                    rowAmazonSettlementDetails.TransactionGroup_DEV000221 = 1
                                                    rowAmazonSettlementDetails.TransactionType_DEV000221 = "New Orders"
                                                    rowAmazonSettlementDetails.LineNumber_DEV000221 = iSettlementLineNo
                                                    rowAmazonSettlementDetails.AmazonOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_DETAILS & "AmazonOrderID")
                                                    rowAmazonSettlementDetails.ShipmentOrAdjustmentID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_DETAILS & "ShipmentID")
                                                    rowAmazonSettlementDetails.AmazonMarketplace_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_DETAILS & "MarketplaceName")
                                                    rowAmazonSettlementDetails.FulfillmentType_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_FULFILLMENT_DETAILS & "MerchantFulfillmentID")
                                                    rowAmazonSettlementDetails.PostedDate_DEV000221 = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_FULFILLMENT_DETAILS & "PostedDate"))
                                                    rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ORDER_FULFILLMENT_ITEM_DETAILS & "AmazonOrderItemCode")
                                                    rowAmazonSettlementDetails.ItemSKU_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ORDER_FULFILLMENT_ITEM_DETAILS & "SKU")
                                                    If Not String.IsNullOrEmpty(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ORDER_FULFILLMENT_ITEM_DETAILS & "Quantity")) Then
                                                        rowAmazonSettlementDetails.ItemQuantity_DEV000221 = CInt(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ORDER_FULFILLMENT_ITEM_DETAILS & "Quantity"))
                                                    Else
                                                        rowAmazonSettlementDetails.ItemQuantity_DEV000221 = 0
                                                    End If
                                                    rowAmazonSettlementDetails.CurrencyCode_DEV000221 = rowAmazonSettlement.CurrencyCode_DEV000221
                                                    rowAmazonSettlementDetails.ExchangeRate_DEV000221 = rowAmazonSettlement.ExchangeRate_DEV000221
                                                    rowAmazonSettlementDetails.PrincipalAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.ShippingAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.ShippingAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.TaxAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.TaxAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerOrderFulfillmentFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerUnitFulfillmentFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAWeightBasedFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.CommissionAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.SalesTaxServiceFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.SalesTaxServiceFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.Reconciled_DEV000221 = False

                                                    XMLItemFees = XMLItemTemp.XPathSelectElements(AMAZON_SETTLEMENT_ORDER_FULFILLMENT_ITEM_DETAILS & "ItemPrice/Component")
                                                    For Each XMLItemFeeNode In XMLItemFees
                                                        XMLItemFeeTemp = XDocument.Parse(XMLItemFeeNode.ToString)
                                                        Select Case eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Type")
                                                            Case "Principal"
                                                                rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Amount"))
                                                                rowAmazonSettlementDetails.PrincipalAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalPrincipal = decTotalPrincipal + rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221

                                                            Case "Shipping"
                                                                rowAmazonSettlementDetails.ShippingAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Amount"))
                                                                rowAmazonSettlementDetails.ShippingAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.ShippingAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalShipping = decTotalShipping + rowAmazonSettlementDetails.ShippingAmountRate_DEV000221

                                                            Case "Tax"
                                                                rowAmazonSettlementDetails.TaxAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Amount"))
                                                                rowAmazonSettlementDetails.TaxAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.TaxAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalSalesTax = decTotalSalesTax + rowAmazonSettlementDetails.TaxAmountRate_DEV000221

                                                            Case Else
                                                                m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Unknown ItemPrice Component " & eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Type") & " for Amazon Order " & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & " in Settlement Report " & strAmazonSettlementID, XMLRequest.ToString)
                                                        End Select
                                                        If bShutDownInProgress Then ' TJS 02/08/13
                                                            Exit For ' TJS 02/08/13
                                                        End If
                                                    Next

                                                    XMLItemFees = XMLItemTemp.XPathSelectElements(AMAZON_SETTLEMENT_ORDER_FULFILLMENT_ITEM_DETAILS & "ItemFees/Fee")
                                                    For Each XMLItemFeeNode In XMLItemFees
                                                        XMLItemFeeTemp = XDocument.Parse(XMLItemFeeNode.ToString)
                                                        Select Case eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Type")
                                                            Case "FBAPerOrderFulfillmentFee"
                                                                rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.FBAPerOrderFulfillmentFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalFBAPerOrderFees = decTotalFBAPerOrderFees + rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221

                                                            Case "FBAPerUnitFulfillmentFee"
                                                                rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.FBAPerUnitFulfillmentFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalFBAPerUnitFees = decTotalFBAPerUnitFees + rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221

                                                            Case "FBAWeightBasedFee"
                                                                rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.FBAWeightBasedFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalFBAWeightFees = decTotalFBAWeightFees + rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221

                                                            Case "ShippingHB"

                                                            Case "ShippingChargeback"

                                                            Case "Commission"
                                                                rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.CommissionAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalCommission = decTotalCommission + rowAmazonSettlementDetails.CommissionAmountRate_DEV000221

                                                            Case "SalesTaxServiceFee"
                                                                rowAmazonSettlementDetails.SalesTaxServiceFeeRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.SalesTaxServiceFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.SalesTaxServiceFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalServiceFeesSalesTax = decTotalServiceFeesSalesTax + rowAmazonSettlementDetails.SalesTaxServiceFeeRate_DEV000221

                                                            Case "TransactionFee" ' TJS 14/08/12

                                                            Case Else
                                                                m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Unknown ItemFees Fee " & eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Type") & " for Amazon Order " & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & " in Settlement Report " & strAmazonSettlementID, XMLRequest.ToString) ' TJS 14/08/12
                                                        End Select
                                                        If bShutDownInProgress Then ' TJS 02/08/13
                                                            Exit For ' TJS 02/08/13
                                                        End If
                                                    Next

                                                    ' can we find this order ?
                                                    strISSalesOrderCode = eShopCONNECTFacade.GetField("SalesOrderCode", "CustomerSalesOrder", "StoreMerchantID_DEV000221 = '" & rowAmazonSettlement.MerchantID_DEV000221 & _
                                                        "' AND MerchantOrderID_DEV000221 = '" & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & "' AND [Type] = 'Sales Order' AND IsVoided = 0")
                                                    If String.IsNullOrEmpty(strISSalesOrderCode) Then
                                                        ' no, try using MerchantOrderID
                                                        strISSalesOrderCode = eShopCONNECTFacade.GetField("SalesOrderCode", "CustomerSalesOrder", "StoreMerchantID_DEV000221 = '" & rowAmazonSettlement.MerchantID_DEV000221 & _
                                                            "' AND MerchantOrderID_DEV000221 = '" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_DETAILS & "MerchantOrderID") & _
                                                            "' AND [Type] = 'Sales Order' AND IsVoided = 0")
                                                        If Not String.IsNullOrEmpty(strISSalesOrderCode) Then
                                                            rowAmazonSettlementDetails.AmazonOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_DETAILS & "MerchantOrderID")
                                                        End If
                                                    End If
                                                    If Not String.IsNullOrEmpty(strISSalesOrderCode) Then
                                                        ' yes, record it
                                                        rowAmazonSettlementDetails.ISSalesOrderCode_DEV000221 = strISSalesOrderCode
                                                        ' now check for Item details
                                                        strISItemDetails = eShopCONNECTFacade.GetRow(New String() {"ItemCode", "LineNum", "SourceCommissionChargeRate_DEV000221", "SourceFulfillmentCostRate_DEV000221"}, _
                                                            "CustomerSalesOrderDetail", "SalesOrderCode = '" & strISSalesOrderCode & "' AND SourcePurchaseID_DEV000221 = '" & rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221 & "'")
                                                        If strISItemDetails IsNot Nothing AndAlso strISItemDetails.Length = 4 Then
                                                            rowAmazonSettlementDetails.ISItemCode_DEV000221 = strISItemDetails(0)
                                                            rowAmazonSettlementDetails.ISItemLineNum_DEV000221 = CInt(strISItemDetails(1))
                                                            ' don't mark as reconciled yet as we need to amalgamate and check quantities etc
                                                            ' but don't mark overall settlement as not reconciled yet
                                                        Else
                                                            bSettlementReconciled = False
                                                            rowAmazonSettlementDetails.ReconciliationComments_DEV000221 = "Amazon Order Item Code " & rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221 & " not found"
                                                        End If
                                                    Else
                                                        ' no, need manual reconciliation
                                                        bSettlementReconciled = False
                                                        rowAmazonSettlementDetails.ReconciliationComments_DEV000221 = "Amazon Order " & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & " not found"
                                                    End If
                                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.AddLerrynImportExportAmazonSettlementDetail_DEV000221Row(rowAmazonSettlementDetails)
                                                    iSettlementLineNo += 1
                                                    If bShutDownInProgress Then ' TJS 02/08/13
                                                        Exit For ' TJS 02/08/13
                                                    End If
                                                Next
                                            End If
                                            If bShutDownInProgress Then ' TJS 02/08/13
                                                Exit For ' TJS 02/08/13
                                            End If
                                        Next
                                    End If

                                    ' now process settlement adjustment records
                                    XMLAdjustmentList = XMLRequest.XPathSelectElements(AMAZON_SETTLEMENT_ADJUSTMENT_LIST)
                                    If XMLAdjustmentList IsNot Nothing Then
                                        ' adjustments must be manually reconciled so set non-reconciled detail record flag
                                        bSettlementReconciled = False
                                        For Each XMLAdjustmentNode In XMLAdjustmentList
                                            XMLTemp = XDocument.Parse(XMLAdjustmentNode.ToString)
                                            XMLItemList = XMLTemp.XPathSelectElements(AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_LIST)
                                            If XMLItemList IsNot Nothing Then
                                                For Each XMLItemNode In XMLItemList
                                                    XMLItemTemp = XDocument.Parse(XMLItemNode.ToString)
                                                    rowAmazonSettlementDetails = eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.NewLerrynImportExportAmazonSettlementDetail_DEV000221Row
                                                    rowAmazonSettlementDetails.SettlementCode_DEV000221 = rowAmazonSettlement.SettlementCode_DEV000221
                                                    rowAmazonSettlementDetails.TransactionGroup_DEV000221 = 2
                                                    rowAmazonSettlementDetails.TransactionType_DEV000221 = "Adjustments"
                                                    rowAmazonSettlementDetails.LineNumber_DEV000221 = iSettlementLineNo
                                                    rowAmazonSettlementDetails.AmazonOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ADJUSTMENT_DETAILS & "AmazonOrderID")
                                                    rowAmazonSettlementDetails.ShipmentOrAdjustmentID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ADJUSTMENT_DETAILS & "AdjustmentID")
                                                    rowAmazonSettlementDetails.AmazonMarketplace_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ADJUSTMENT_DETAILS & "MarketplaceName")
                                                    rowAmazonSettlementDetails.FulfillmentType_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_DETAILS & "MerchantFulfillmentID")
                                                    rowAmazonSettlementDetails.PostedDate_DEV000221 = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_DETAILS & "PostedDate"))
                                                    rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_DETAILS & "AmazonOrderItemCode")
                                                    rowAmazonSettlementDetails.MerchantAdjustmentItemID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_DETAILS & "MerchantAdjustmentItemID")
                                                    rowAmazonSettlementDetails.ItemSKU_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_DETAILS & "SKU")
                                                    If Not String.IsNullOrEmpty(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_DETAILS & "Quantity")) Then
                                                        rowAmazonSettlementDetails.ItemQuantity_DEV000221 = CInt(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_DETAILS & "Quantity"))
                                                    Else
                                                        rowAmazonSettlementDetails.ItemQuantity_DEV000221 = 0
                                                    End If
                                                    rowAmazonSettlementDetails.CurrencyCode_DEV000221 = rowAmazonSettlement.CurrencyCode_DEV000221
                                                    rowAmazonSettlementDetails.ExchangeRate_DEV000221 = rowAmazonSettlement.ExchangeRate_DEV000221
                                                    rowAmazonSettlementDetails.PrincipalAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.ShippingAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.ShippingAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.TaxAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.TaxAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerOrderFulfillmentFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerUnitFulfillmentFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAWeightBasedFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.CommissionAmount_DEV000221 = 0
                                                    rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.SalesTaxServiceFee_DEV000221 = 0
                                                    rowAmazonSettlementDetails.SalesTaxServiceFeeRate_DEV000221 = 0
                                                    rowAmazonSettlementDetails.Reconciled_DEV000221 = False

                                                    XMLItemFees = XMLItemTemp.XPathSelectElements(AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_DETAILS & "ItemPriceAdjustments/Component")
                                                    For Each XMLItemFeeNode In XMLItemFees
                                                        XMLItemFeeTemp = XDocument.Parse(XMLItemFeeNode.ToString)
                                                        Select Case eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Type")
                                                            Case "Principal"
                                                                rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Amount"))
                                                                rowAmazonSettlementDetails.PrincipalAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalPrincipal = decTotalPrincipal + rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221

                                                            Case "Shipping"
                                                                rowAmazonSettlementDetails.ShippingAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Amount"))
                                                                rowAmazonSettlementDetails.ShippingAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.ShippingAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalShipping = decTotalShipping + rowAmazonSettlementDetails.ShippingAmountRate_DEV000221

                                                            Case "Tax"
                                                                rowAmazonSettlementDetails.TaxAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Amount"))
                                                                rowAmazonSettlementDetails.TaxAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.TaxAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalSalesTax = decTotalSalesTax + rowAmazonSettlementDetails.TaxAmountRate_DEV000221

                                                            Case Else
                                                                m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Unknown ItemPriceAdjustments Component " & eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Component/Type") & " for Amazon Order " & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & " in Settlement Report " & strAmazonSettlementID, XMLRequest.ToString)
                                                        End Select
                                                        If bShutDownInProgress Then ' TJS 02/08/13
                                                            Exit For ' TJS 02/08/13
                                                        End If
                                                    Next

                                                    XMLItemFees = XMLItemTemp.XPathSelectElements(AMAZON_SETTLEMENT_ADJUSTMENT_FULFILLMENT_ITEM_DETAILS & "ItemFeeAdjustments/Fee")
                                                    For Each XMLItemFeeNode In XMLItemFees
                                                        XMLItemFeeTemp = XDocument.Parse(XMLItemFeeNode.ToString)
                                                        Select Case eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Type")
                                                            Case "FBAPerOrderFulfillmentFee"
                                                                rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.FBAPerOrderFulfillmentFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalFBAPerOrderFees = decTotalFBAPerOrderFees + rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221

                                                            Case "FBAPerUnitFulfillmentFee"
                                                                rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.FBAPerUnitFulfillmentFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalFBAPerUnitFees = decTotalFBAPerUnitFees + rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221

                                                            Case "FBAWeightBasedFee"
                                                                rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.FBAWeightBasedFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalFBAWeightFees = decTotalFBAWeightFees + rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221

                                                            Case "ShippingHB"


                                                            Case "Commission"
                                                                rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.CommissionAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalCommission = decTotalCommission + rowAmazonSettlementDetails.CommissionAmountRate_DEV000221

                                                            Case "RefundCommission"
                                                                rowAmazonSettlementDetails2 = eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.NewLerrynImportExportAmazonSettlementDetail_DEV000221Row
                                                                rowAmazonSettlementDetails2.SettlementCode_DEV000221 = rowAmazonSettlement.SettlementCode_DEV000221
                                                                rowAmazonSettlementDetails2.TransactionGroup_DEV000221 = rowAmazonSettlementDetails.TransactionGroup_DEV000221
                                                                rowAmazonSettlementDetails2.TransactionType_DEV000221 = rowAmazonSettlementDetails.TransactionType_DEV000221
                                                                rowAmazonSettlementDetails2.LineNumber_DEV000221 = rowAmazonSettlementDetails.LineNumber_DEV000221 + 1
                                                                rowAmazonSettlementDetails2.AmazonOrderID_DEV000221 = rowAmazonSettlementDetails.AmazonOrderID_DEV000221
                                                                rowAmazonSettlementDetails2.ShipmentOrAdjustmentID_DEV000221 = rowAmazonSettlementDetails.ShipmentOrAdjustmentID_DEV000221
                                                                rowAmazonSettlementDetails2.AmazonMarketplace_DEV000221 = rowAmazonSettlementDetails.AmazonMarketplace_DEV000221
                                                                rowAmazonSettlementDetails2.FulfillmentType_DEV000221 = rowAmazonSettlementDetails.FulfillmentType_DEV000221
                                                                rowAmazonSettlementDetails2.PostedDate_DEV000221 = rowAmazonSettlementDetails.PostedDate_DEV000221
                                                                rowAmazonSettlementDetails2.AmazonOrderItemCode_DEV000221 = rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221
                                                                rowAmazonSettlementDetails2.MerchantAdjustmentItemID_DEV000221 = rowAmazonSettlementDetails.MerchantAdjustmentItemID_DEV000221
                                                                rowAmazonSettlementDetails2.ItemSKU_DEV000221 = rowAmazonSettlementDetails.ItemSKU_DEV000221
                                                                rowAmazonSettlementDetails2.ItemQuantity_DEV000221 = rowAmazonSettlementDetails.ItemQuantity_DEV000221
                                                                rowAmazonSettlementDetails2.CurrencyCode_DEV000221 = rowAmazonSettlement.CurrencyCode_DEV000221
                                                                rowAmazonSettlementDetails2.ExchangeRate_DEV000221 = rowAmazonSettlement.ExchangeRate_DEV000221
                                                                rowAmazonSettlementDetails2.PrincipalAmount_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.PrincipalAmountRate_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.ShippingAmount_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.ShippingAmountRate_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.TaxAmount_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.TaxAmountRate_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.FBAPerOrderFulfillmentFee_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.FBAPerOrderFulfillmentFeeRate_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.FBAPerUnitFulfillmentFee_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.FBAPerUnitFulfillmentFeeRate_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.FBAWeightBasedFee_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.FBAWeightBasedFeeRate_DEV000221 = 0
                                                                rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 = 0 - CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Amount"))
                                                                rowAmazonSettlementDetails.CommissionAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                                decTotalCommission = decTotalCommission + rowAmazonSettlementDetails.CommissionAmountRate_DEV000221
                                                                rowAmazonSettlementDetails2.SalesTaxServiceFee_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.SalesTaxServiceFeeRate_DEV000221 = 0
                                                                rowAmazonSettlementDetails2.Reconciled_DEV000221 = False
                                                                eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.AddLerrynImportExportAmazonSettlementDetail_DEV000221Row(rowAmazonSettlementDetails2)
                                                                iSettlementLineNo += 1

                                                            Case Else
                                                                m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Unknown ItemFeeAdjustments Fee " & eShopCONNECTFacade.GetXMLElementText(XMLItemFeeTemp, "Fee/Type") & " for Amazon Order " & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & " in Settlement Report " & strAmazonSettlementID, XMLRequest.ToString)
                                                        End Select
                                                        If bShutDownInProgress Then ' TJS 02/08/13
                                                            Exit For ' TJS 02/08/13
                                                        End If
                                                    Next

                                                    ' can we find this order ?
                                                    strISSalesOrderCode = eShopCONNECTFacade.GetField("SalesOrderCode", "CustomerSalesOrder", "StoreMerchantID_DEV000221 = '" & rowAmazonSettlement.MerchantID_DEV000221 & _
                                                        "' AND MerchantOrderID_DEV000221 = '" & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & "' AND [Type] = 'Sales Order' AND IsVoided = 0")
                                                    If String.IsNullOrEmpty(strISSalesOrderCode) Then
                                                        ' no, try using MerchantOrderID
                                                        strISSalesOrderCode = eShopCONNECTFacade.GetField("SalesOrderCode", "CustomerSalesOrder", "StoreMerchantID_DEV000221 = '" & rowAmazonSettlement.MerchantID_DEV000221 & _
                                                            "' AND MerchantOrderID_DEV000221 = '" & eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_DETAILS & "MerchantOrderID") & _
                                                            "' AND [Type] = 'Sales Order' AND IsVoided = 0")
                                                        If Not String.IsNullOrEmpty(strISSalesOrderCode) Then
                                                            rowAmazonSettlementDetails.AmazonOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLTemp, AMAZON_SETTLEMENT_ORDER_DETAILS & "MerchantOrderID")
                                                        End If
                                                    End If
                                                    If Not String.IsNullOrEmpty(strISSalesOrderCode) Then
                                                        ' yes, record it
                                                        rowAmazonSettlementDetails.ISSalesOrderCode_DEV000221 = strISSalesOrderCode
                                                        ' now check for Item details
                                                        strISItemDetails = eShopCONNECTFacade.GetRow(New String() {"ItemCode", "LineNum", "SourceCommissionChargeRate_DEV000221", "SourceFulfillmentCostRate_DEV000221"}, _
                                                            "CustomerSalesOrderDetail", "SalesOrderCode = '" & strISSalesOrderCode & "' AND SourcePurchaseID_DEV000221 = '" & rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221 & "'")
                                                        If strISItemDetails IsNot Nothing AndAlso strISItemDetails.Length = 4 Then
                                                            rowAmazonSettlementDetails.ISItemCode_DEV000221 = strISItemDetails(0)
                                                            rowAmazonSettlementDetails.ISItemLineNum_DEV000221 = CInt(strISItemDetails(1))
                                                            ' don't mark adjustments as reconciled as they need confirming
                                                            bSettlementReconciled = False
                                                        Else
                                                            bSettlementReconciled = False
                                                            rowAmazonSettlementDetails.ReconciliationComments_DEV000221 = "Amazon Order Item Code " & rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221 & " not found"
                                                        End If
                                                    Else
                                                        ' no, need manual reconciliation
                                                        bSettlementReconciled = False
                                                        rowAmazonSettlementDetails.ReconciliationComments_DEV000221 = "Amazon Order " & rowAmazonSettlementDetails.AmazonOrderID_DEV000221 & " not found"
                                                    End If
                                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.AddLerrynImportExportAmazonSettlementDetail_DEV000221Row(rowAmazonSettlementDetails)
                                                    iSettlementLineNo += 1
                                                    If bShutDownInProgress Then ' TJS 02/08/13
                                                        Exit For ' TJS 02/08/13
                                                    End If
                                                Next
                                            End If
                                            If bShutDownInProgress Then ' TJS 02/08/13
                                                Exit For ' TJS 02/08/13
                                            End If
                                        Next
                                    End If

                                    ' now process settlement other transactions records
                                    XMLOtherTransactionList = XMLRequest.XPathSelectElements(AMAZON_SETTLEMENT_OTHER_TRANSACTION_LIST)
                                    If XMLOtherTransactionList IsNot Nothing Then
                                        ' oth mer transaction records ust be manually reconciled so set non-reconciled detail record flag
                                        bSettlementReconciled = False
                                        For Each XMLOtherTransactionNode In XMLOtherTransactionList
                                            XMLItemTemp = XDocument.Parse(XMLOtherTransactionNode.ToString)
                                            rowAmazonSettlementDetails = eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.NewLerrynImportExportAmazonSettlementDetail_DEV000221Row
                                            rowAmazonSettlementDetails.SettlementCode_DEV000221 = rowAmazonSettlement.SettlementCode_DEV000221
                                            rowAmazonSettlementDetails.AmazonOrderID_DEV000221 = eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_OTHER_TRANSACTION_DETAILS & "TransactionType")
                                            rowAmazonSettlementDetails.AmazonMarketplace_DEV000221 = "Amazon" & AmazonConfig.AmazonSite
                                            rowAmazonSettlementDetails.AmazonOrderItemCode_DEV000221 = "-"
                                            rowAmazonSettlementDetails.TransactionGroup_DEV000221 = 3
                                            rowAmazonSettlementDetails.TransactionType_DEV000221 = "Other Transactions"
                                            rowAmazonSettlementDetails.LineNumber_DEV000221 = iSettlementLineNo
                                            rowAmazonSettlementDetails.PostedDate_DEV000221 = eShopCONNECTFacade.ConvertXMLDate(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_OTHER_TRANSACTION_DETAILS & "PostedDate"))
                                            rowAmazonSettlementDetails.CurrencyCode_DEV000221 = rowAmazonSettlement.CurrencyCode_DEV000221
                                            rowAmazonSettlementDetails.ExchangeRate_DEV000221 = rowAmazonSettlement.ExchangeRate_DEV000221
                                            rowAmazonSettlementDetails.PrincipalAmount_DEV000221 = 0
                                            rowAmazonSettlementDetails.PrincipalAmountRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.ShippingAmount_DEV000221 = 0
                                            rowAmazonSettlementDetails.ShippingAmountRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.TaxAmount_DEV000221 = 0
                                            rowAmazonSettlementDetails.TaxAmountRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.FBAPerOrderFulfillmentFee_DEV000221 = 0
                                            rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.FBAPerUnitFulfillmentFee_DEV000221 = 0
                                            rowAmazonSettlementDetails.FBAPerUnitFulfillmentFeeRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.FBAWeightBasedFee_DEV000221 = 0
                                            rowAmazonSettlementDetails.FBAWeightBasedFeeRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.CommissionAmount_DEV000221 = 0
                                            rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.SalesTaxServiceFee_DEV000221 = 0
                                            rowAmazonSettlementDetails.SalesTaxServiceFeeRate_DEV000221 = 0
                                            rowAmazonSettlementDetails.Reconciled_DEV000221 = False

                                            Select Case eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_OTHER_TRANSACTION_DETAILS & "TransactionType")
                                                Case "Inbound Transportation Charge"
                                                    rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_OTHER_TRANSACTION_DETAILS & "Amount"))
                                                    rowAmazonSettlementDetails.FBAPerOrderFulfillmentFee_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                    decTotalInboundShipping = decTotalInboundShipping + rowAmazonSettlementDetails.FBAPerOrderFulfillmentFeeRate_DEV000221

                                                Case "Subscription Fee"
                                                    rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 = CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_OTHER_TRANSACTION_DETAILS & "Amount"))
                                                    rowAmazonSettlementDetails.CommissionAmount_DEV000221 = RoundDecimalValue(rowAmazonSettlementDetails.CommissionAmountRate_DEV000221 / rowAmazonSettlementDetails.ExchangeRate_DEV000221)
                                                    decTotalSubscriptionFees = decTotalSubscriptionFees + rowAmazonSettlementDetails.CommissionAmountRate_DEV000221

                                                Case "Previous Reserve Amount Balance"

                                                Case "Current Reserve Amount"
                                                    decTotalOthers = decTotalOthers + CDec(eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_OTHER_TRANSACTION_DETAILS & "Amount"))

                                                Case Else
                                                    m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Unknown OtherTransaction  " & eShopCONNECTFacade.GetXMLElementText(XMLItemTemp, AMAZON_SETTLEMENT_OTHER_TRANSACTION_DETAILS & "TransactionType") & " in Settlement Report " & strAmazonSettlementID, XMLRequest.ToString)

                                            End Select
                                            eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.AddLerrynImportExportAmazonSettlementDetail_DEV000221Row(rowAmazonSettlementDetails)
                                            iSettlementLineNo += 1
                                            If bShutDownInProgress Then ' TJS 02/08/13
                                                Exit For ' TJS 02/08/13
                                            End If
                                        Next
                                    End If

                                    ' update totals 
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalPrincipalRate_DEV000221 = decTotalPrincipal
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalPrincipal_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalPrincipalRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalShippingRate_DEV000221 = decTotalShipping
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalShipping_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalShippingRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalSalesTaxRate_DEV000221 = decTotalSalesTax
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalSalesTax_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalSalesTaxRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalGrossSalesRate_DEV000221 = decTotalPrincipal + decTotalShipping + decTotalSalesTax
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalGrossSales_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalGrossSalesRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAPerOrderFeesRate_DEV000221 = decTotalFBAPerOrderFees
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAPerOrderFees_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAPerOrderFeesRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAPerUnitFeesRate_DEV000221 = decTotalFBAPerUnitFees
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAPerUnitFees_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAPerUnitFeesRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAWeightFeesRate_DEV000221 = decTotalFBAWeightFees
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAWeightFees_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalFBAWeightFeesRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalCommissionRate_DEV000221 = decTotalCommission
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalCommission_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalCommissionRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalInboundShippingRate_DEV000221 = decTotalInboundShipping
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalInboundShipping_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalInboundShippingRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalSubscriptionFeesRate_DEV000221 = decTotalSubscriptionFees
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalSubscriptionFees_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalSubscriptionFeesRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalServiceFeeSalesTaxRate_DEV000221 = decTotalServiceFeesSalesTax
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalServiceFeeSalesTax_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalServiceFeeSalesTaxRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalOthersRate_DEV000221 = decTotalOthers
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalOthers_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalOthersRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalChargesRate_DEV000221 = decTotalFBAPerOrderFees + decTotalFBAPerUnitFees + decTotalFBAWeightFees + decTotalInboundShipping + decTotalCommission + decTotalSubscriptionFees + decTotalServiceFeesSalesTax
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalCharges_DEV000221 = RoundDecimalValue(eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).TotalChargesRate_DEV000221 / eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).ExchangeRate_DEV000221)

                                    ' update settlement reconciled status
                                    eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221(0).Reconciled_DEV000221 = bSettlementReconciled
                                    ' and save it
                                    If Not bShutDownInProgress Then ' TJS 02/08/13
                                        If eShopCONNECTFacade.UpdateDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.TableName, _
                                            "CreateLerrynImportExportAmazonSettlement_DEV000221", "UpdateLerrynImportExportAmazonSettlement_DEV000221", "DeleteLerrynImportExportAmazonSettlement_DEV000221"}, _
                                            New String() {eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.TableName, _
                                            "CreateLerrynImportExportAmazonSettlementDetail_DEV000221", "UpdateLerrynImportExportAmazonSettlementDetail_DEV000221", "DeleteLerrynImportExportAmazonSettlementDetail_DEV000221"}}, _
                                            Interprise.Framework.Base.Shared.TransactionType.None, "Import Inventory Item", False) Then
                                            bReturnValue = True
                                        Else
                                            strTemp = ""
                                            For iRowLoop = 0 To eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.Rows.Count - 1
                                                For iColumnLoop = 0 To eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.Columns.Count - 1
                                                    If eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                        strTemp = strTemp & eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.TableName & _
                                                            "." & eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                            eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlement_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                    End If
                                                Next
                                            Next
                                            For iRowLoop = 0 To eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.Rows.Count - 1
                                                For iColumnLoop = 0 To eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.Columns.Count - 1
                                                    If eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                                        strTemp = strTemp & eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.TableName & _
                                                            "." & eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.Columns(iColumnLoop).ColumnName & ", " & _
                                                            eShopCONNECTDatasetGateway.LerrynImportExportAmazonSettlementDetail_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) & vbCrLf
                                                    End If
                                                Next
                                            Next
                                            m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Failed to save Settlement Report " & strAmazonSettlementID & " - " & strTemp, XMLRequest.ToString)
                                        End If
                                    End If
                                Else
                                    m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Incorrect Amazon Merchant ID in Settlement Report " & strAmazonSettlementID & ", expected " & AmazonConfig.MerchantToken & ", actual is " & eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_MERCHANT_ID), XMLRequest.ToString) ' TJS 22/03/13
                                End If

                            ElseIf m_ImportExportConfigFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "ProcessAmazonSettlements", "NO").ToUpper = "YES" Then ' TJS 26/06/13
                                ' yes 
                                eShopCONNECTFacade.WriteLogProgressRecord("Amazon Settlement Report " & strAmazonSettlementID & " has already been processed")
                                bReturnValue = True
                            End If
                            ' end of code added TJS 05/07/12

                        Case Else
                            ' write file to manual processing directory
                            sLogFileName = AmazonConfig.ManualProcessingPath & "Amazon_" & eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_TYPE).ToUpper & "_" & strFileID & ".xml" ' TJS 15/12/09
                            ' create the status update file
                            writer = New StreamWriter(sLogFileName, True) ' TJS 15/12/09
                            ' write the XML to it
                            writer.WriteLine(strInputXML) ' TJS 15/12/09
                            ' and close it
                            writer.Close() ' TJS 15/12/09

                    End Select
                    bReturnValue = Not bOneOrMoreRecordsfailed ' TJS 10/03/09 TJS 16/10/09
                Else
                    bReturnValue = False ' TJS 10/03/09 TJS 16/10/09
                    ' start of code added TJS 26/06/13
                    If RowOrderToRetry IsNot Nothing And eShopCONNECTFacade.GetXMLElementText(XMLRequest, AMAZON_MESSAGE_TYPE) = "OrderReport" Then
                        If RowOrderToRetry.RetryCount_DEV000221 > 4 Then
                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(8)
                        Else
                            RowOrderToRetry.NextRetryDateTime_DEV000221 = Date.Now.AddHours(24)
                        End If
                        RowOrderToRetry.RetryCount_DEV000221 = RowOrderToRetry.RetryCount_DEV000221 - 1
                    End If
                    ' end of code added TJS 26/06/13   
                End If
            Else
                m_ImportExportConfigFacade.SendSourceErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", "Amazon File does not start with expected XML header <?xml version=""1.0"" encoding=""UTF-8""?>", strInputXML) ' TJS 15/12/09
                bReturnValue = False ' TJS 10/03/09 TJS 16/10/09
            End If

        Catch ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(SourceConfig.XMLConfig, "ProcessAmazonXML", ex, m_ImportExportConfigFacade.ConvertXMLFromWeb(strInputXML.ToString))
            bReturnValue = False ' TJS 10/03/09 TJS 16/10/09

        Finally
            eShopCONNECTFacade.Dispose() ' TJS 16/10/09

        End Try
        Return bReturnValue ' TJS 16/10/09

    End Function

    Private Function BillingDetails(ByVal XMLAmazonOrder As XDocument, ByVal ISCustomerCode As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/02/09 | TJS             | 2009.1.08 | Function added
        ' 15/10/09 | TJS             | 2009.3.08 | Modified to cater for Amazon allowing State to be the full state
        '                                        | name instead of just the 2 character code for the US and canada
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerBill As XElement, XMLBillingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strTemp As String

        XMLResult = New XElement("BillingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerBill = New XElement("Customer")
        ' Amazon doesn't have Customer IDs, they use the email address
        XMLCustomerBill.Add(New XElement("SourceCustomerID", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_DETAILS & "BuyerEmailAddress")))
        XMLCustomerBill.Add(New XElement("ISCustomerCode", ISCustomerCode))
        XMLCustomerBill.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_DETAILS & "BuyerName")))
        XMLCustomerBill.Add(New XElement("HomePhone", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_DETAILS & "BuyerPhoneNumber")))
        XMLCustomerBill.Add(New XElement("Email", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_DETAILS & "BuyerEmailAddress")))

        ' BILLING ADDRESS
        XMLBillingAddress = New XElement("BillingAddress")
        ' is there a card billing address ?
        If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "AddressFieldOne") <> "" Then
            ' yes, use it
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "AddressFieldOne")
            If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "AddressFieldTwo") <> "" Then
                strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "AddressFieldTwo")
            End If
            If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "AddressFieldThree") <> "" Then
                strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "AddressFieldThree")
            End If
            XMLBillingAddress.Add(New XElement("Address", strTemp))
            XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "City")))
            ' need to ensure Country matches those used in IS
            strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "CountryCode")) ' TJS 16/10/09
            If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "StateOrRegion").Length <= 2 Then ' TJS 16/10/09
                XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "StateOrRegion")))
            Else
                XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "StateOrRegion")))
            End If
            XMLBillingAddress.Add(New XElement("Country", strCountry))
            XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_BILLING_CARD_ADDRESS_DETAILS & "PostalCode")))
        Else
            ' no, use Fulfillment address
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldOne")
            If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldTwo") <> "" Then
                strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldTwo")
            End If
            If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldThree") <> "" Then
                strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldThree")
            End If
            XMLBillingAddress.Add(New XElement("Address", strTemp))
            XMLBillingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "City")))
            ' need to ensure Country matches those used in IS
            strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "CountryCode")) ' TJS 16/10/09
            If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "StateOrRegion").Length <= 2 Then ' TJS 16/10/09
                XMLBillingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "StateOrRegion")))
            Else
                XMLBillingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "StateOrRegion")))
            End If
            XMLBillingAddress.Add(New XElement("Country", strCountry))
            XMLBillingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "PostalCode")))
        End If

        XMLResult.Add(XMLCustomerBill)
        XMLResult.Add(XMLBillingAddress)

        Return XMLResult

    End Function

    Private Function ShippingDetails(ByVal XMLAmazonOrder As XDocument, ByVal SourceConfig As SourceSettings) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/02/09 | TJS             | 2009.1.08 | Function added
        ' 16/10/09 | TJS             | 2009.3.08 | Modified to cater for Amazon allowing State to be the full state
        '                                        | name instead of just the 2 character code for the US and canada
        ' 15/12/09 | TJS             | 2009.3.09 | Modified to cater for SourceDeliveryClass on TranslateDeliveryMethodToIS
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to directly use config setting values and Source Code constants
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLCustomerShip As XElement, XMLShippingAddress As XElement
        Dim XMLResult As XElement, strCountry As String, strTemp As String, strShippingMethodGroup As String


        XMLResult = New XElement("ShippingDetails")
        ' CUSTOMER DETAILS
        XMLCustomerShip = New XElement("Customer")
        XMLCustomerShip.Add(New XElement("LastName", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "Name")))
        XMLCustomerShip.Add(New XElement("Telephone", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "PhoneNumber")))

        ' SHIPPING ADDRESS
        XMLShippingAddress = New XElement("ShippingAddress")
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldOne")
        If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldTwo") <> "" Then
            strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldTwo")
        End If
        If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldThree") <> "" Then
            strTemp = strTemp & vbCrLf & eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "AddressFieldThree")
        End If
        XMLShippingAddress.Add(New XElement("Address", strTemp))
        XMLShippingAddress.Add(New XElement("Town_City", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "City")))
        ' need to ensure Country matches those used in IS
        strCountry = ConvertCountryCode(eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "CountryCode")) ' TJS 16/10/09
        If (strCountry = "Canada" Or strCountry = "United States of America") And eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "StateOrRegion").Length <= 2 Then ' TJS 16/10/09
            XMLShippingAddress.Add(New XElement("State", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "StateOrRegion")))
        Else
            XMLShippingAddress.Add(New XElement("County", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "StateOrRegion")))
        End If
        XMLShippingAddress.Add(New XElement("Country", strCountry))
        XMLShippingAddress.Add(New XElement("PostalCode", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS_ADDRESS & "PostalCode")))

        XMLResult.Add(XMLCustomerShip)
        XMLResult.Add(XMLShippingAddress)
        If SourceConfig.EnableDeliveryMethodTranslation And _
            eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS & "FulfillmentServiceLevel") <> "" Then ' TJS 10/03/09 TJS 19/08/10
            strShippingMethodGroup = ""
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.TranslateDeliveryMethodToIS(AMAZON_SOURCE_CODE, eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS & "FulfillmentServiceLevel"), "", strShippingMethodGroup))) ' TJS 10/03/09 TJS 15/12/09
            XMLResult.Add(New XElement("ShippingMethodGroup", strShippingMethodGroup))

        ElseIf eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS & "FulfillmentServiceLevel") = "" Then ' TJS 10/03/09
            XMLResult.Add(New XElement("ShippingMethod", SourceConfig.DefaultShippingMethod))
            XMLResult.Add(New XElement("ShippingMethodGroup", SourceConfig.DefaultShippingMethodGroup))

        Else
            XMLResult.Add(New XElement("ShippingMethod", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrder, AMAZON_ORDER_FULFILLMENT_DETAILS & "FulfillmentServiceLevel"))) ' TJS 10/03/09
            XMLResult.Add(New XElement("ShippingMethodGroup", SourceConfig.DefaultShippingMethodGroup))
        End If

        Return XMLResult
    End Function

    Private Function ItemDetails(ByVal XMLAmazonOrderItem As XDocument, ByVal AmazonConfig As AmazonSettings, ByRef ErrorMessage As String) As XElement
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/02/09 | TJS             | 2009.1.08 | Function added
        ' 10/03/09 | TJS             | 2009.1.09 | Modified to cater for Commision being negative in Amazon schema
        ' 19/08/10 | TJS             | 2010.1.00 | Modified for CustomSKUProcessing and EnableSKUAliasLookup
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 03/07/13 | TJS             | 2013.1.24 | Modifed to use LerrynImportExportSKUAliasView_DEV000221 and corrected SKU Alias processing
        ' 22/05/14 | TJS             | 2014.0.01 | Modified for Unit of Measure on SKU Alias lookup
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLItemPriceTemp As XDocument, XMLItemPriceComponents As System.Collections.Generic.IEnumerable(Of XElement)
        Dim XMLResult As XElement, XMLItemPriceComponent As XElement
        Dim strTemp As String, decItemQuantity As Decimal, decItemCommission As Decimal

        XMLResult = New XElement("Item")
        'ITEM DETAILS
        XMLResult.Add(New XElement("SourceItemPurchaseID", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_DETAILS & "AmazonOrderItemCode")))
        ' get Item SKU
        strTemp = eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_DETAILS & "SKU")
        ' is there an SKU for the item ?
        If strTemp <> "" Then
            ' yes, has Custom SKU Processing been enabled ?
            If AmazonConfig.CustomSKUProcessing <> "" Then
                ' yes, convert SKU
                strTemp = ConvertSKU(AmazonConfig.CustomSKUProcessing, strTemp)
            End If
            If AmazonConfig.EnableSKUAliasLookup Then
                eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.TableName, _
                    "ReadLerrynImportExportSKUAliases_DEV000221", "@SourceCode", AMAZON_SOURCE_CODE, "@SourceSKU", strTemp}}, _
                    Interprise.Framework.Base.Shared.ClearType.Specific) ' TJS 03/07/13
                If eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221.Count > 0 Then ' TJS 03/07/13
                    strTemp = eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).ItemCode_DEV000221 ' TJS 03/07/13
                    XMLResult.Add(New XElement("ISItemCode", strTemp)) ' TJS 03/07/13
                    If Not eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).IsUnitMeasureCode_DEV000221Null Then ' TJS 22/05/14
                        XMLResult.Add(New XElement("ItemUnitOfMeasure", eShopCONNECTDatasetGateway.LerrynImportExportSKUAliasView_DEV000221(0).UnitMeasureCode_DEV000221)) ' TJS 22/05/14
                    End If
                Else
                    XMLResult.Add(New XElement("IS" & AmazonConfig.ISItemIDField, strTemp))
                End If

            Else
                XMLResult.Add(New XElement("IS" & AmazonConfig.ISItemIDField, strTemp))

            End If
        Else
            ErrorMessage = "No Item SKU found for Amazon Order Item Code " & eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_DETAILS & "AmazonOrderItemCode")
            Return XMLResult
        End If
        If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_DETAILS & "Title") <> "" Then ' TJS 10/03/09
            XMLResult.Add(New XElement("ItemDescription", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_DETAILS & "Title")))
        End If
        XMLResult.Add(New XElement("ItemQuantity", eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_DETAILS & "Quantity")))
        decItemQuantity = CDec(eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_DETAILS & "Quantity"))
        XMLItemPriceComponents = XMLAmazonOrderItem.XPathSelectElements(AMAZON_ORDER_ITEM_PRICE_COMPONENT_LIST)
        For Each XMLItemPriceComponent In XMLItemPriceComponents
            XMLItemPriceTemp = XDocument.Parse(XMLItemPriceComponent.ToString)
            strTemp = eShopCONNECTFacade.GetXMLElementText(XMLItemPriceTemp, AMAZON_ORDER_ITEM_PRICE_COMPONENT_DETAILS & "Amount")
            Select Case eShopCONNECTFacade.GetXMLElementText(XMLItemPriceTemp, AMAZON_ORDER_ITEM_PRICE_COMPONENT_DETAILS & "Type")
                Case "Principal"

                    XMLResult.Add(New XElement("ItemUnitPrice", CDec(strTemp) / decItemQuantity))
                    XMLResult.Add(New XElement("ItemSubTotal", strTemp))
                    decItemTotal = decItemTotal + CDec(strTemp) ' TJS 10/03/09

                Case "Shipping"
                    decItemShippingTotal = decItemShippingTotal + CDec(strTemp)

                Case "Tax"
                    XMLResult.Add(New XElement("ItemTaxValue", strTemp))
                    decItemTaxTotal = decItemTaxTotal + CDec(strTemp) ' TJS 10/03/09

                Case "ShippingTax"
                    decItemTaxTotal = decItemTaxTotal + CDec(strTemp)

            End Select
        Next
        If eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_FEE_DETAILS & "Type") = "Commission" Then
            decItemCommission = CDec(eShopCONNECTFacade.GetXMLElementText(XMLAmazonOrderItem, AMAZON_ORDER_ITEM_FEE_DETAILS & "Amount"))
            XMLResult.Add(New XElement("SourceItemCommission", Format(0 - decItemCommission, "0.00")))
        End If
        ItemDetails = XMLResult

    End Function

    Private Function ConvertCountryCode(ByVal strCountry As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 16/02/09 | TJS             | 2009.1.08 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' get Country record IS
        eShopCONNECTFacade.LoadDataSet(New String()() {New String() {eShopCONNECTDatasetGateway.SystemCountry.TableName, _
            "ReadSystemCountryImportExport_DEV000221", AT_ISO_CODE, strCountry}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        If eShopCONNECTDatasetGateway.SystemCountry.Count > 0 Then
            ConvertCountryCode = eShopCONNECTDatasetGateway.SystemCountry(0).CountryCode
        Else
            ConvertCountryCode = strCountry
        End If

    End Function

    Private Function ConvertSKU(ByVal ProcessingMode As String, ByVal SourceSKU As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case ProcessingMode
            Case "Lerryn Test"
                Return "NONSTOCK"

            Case Else
                Return SourceSKU

        End Select

    End Function

    Private Function RoundDecimalValue(ByVal InputValue As Decimal, Optional ByVal DecimalPlacesRequired As Integer = 2) As Decimal
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strFormat As String

        If DecimalPlacesRequired > 0 Then
            strFormat = "0" & ".0000000000".Substring(0, DecimalPlacesRequired + 1)
        Else
            strFormat = "0"
        End If
        Return CDec(InputValue.ToString(strFormat))

    End Function

End Module
