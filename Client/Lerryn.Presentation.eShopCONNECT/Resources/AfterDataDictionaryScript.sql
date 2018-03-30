/********************************************************************************/
/* CUSTOM SCRIPT ADDED HERE - START */
/********************************************************************************/

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportInsertOnCustomerOrderWorkflow_DEV000221]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportInsertOnCustomerOrderWorkflow_DEV000221]
GO

CREATE TRIGGER [dbo].[LerrynImportExportInsertOnCustomerOrderWorkflow_DEV000221] 
   ON  [dbo].[CustomerSalesOrderWorkflow] 
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @SalesCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @Stage nvarchar(50)
	DECLARE @Type nvarchar(25)
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN CustomerSalesOrder B
			ON	 A.SalesOrderCode = B.SalesOrderCode
			WHERE	(B.[Type] = 'Sales Order' OR B.[Type] = 'Back Order')
			AND		A.[Stage] <> 'Void')	
		BEGIN
			SELECT @SalesCode = A.SalesOrderCode, @SourceCode = B.SourceCode, @StoreMerchantID = B.StoreMerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @Stage = A.[Stage], @Type = B.Type 
			FROM inserted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode

			IF @StoreMerchantID IS NOT NULL
				BEGIN
					INSERT INTO LerrynImportExportActionStatus_DEV000221 (SalesOrderCode_DEV000221, SourceCode_DEV000221, 
					  StoreMerchantID_DEV000221, OrderStatus_DEV000221, SalesOrderDetailLineNum_DEV000221, ActionTimestamp_DEV000221, 
					  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
					  MessageAcknowledged_DEV000221, ErrorReported_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
					VALUES (@SalesCode, @SourceCode, @StoreMerchantID, CASE WHEN @Type = 'Back Order' THEN '200' ELSE '100' END, 
					  -1, getdate(), 0 , Null, 0, 0, 0, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET StatusActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportInsertOnCustomerOrderDetail_DEV000221]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportInsertOnCustomerOrderDetail_DEV000221]
GO

CREATE TRIGGER [dbo].[LerrynImportExportInsertOnCustomerOrderDetail_DEV000221] 
   ON  [dbo].[CustomerSalesOrderDetail] 
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @SalesCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @Stage nvarchar(50)
	DECLARE @Type nvarchar(25)
	DECLARE @LineNum int
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode 
			INNER JOIN CustomerSalesOrderWorkflow C ON B.SalesOrderCode = C.SalesOrderCode
			WHERE	(B.[Type] = 'Sales Order' OR B.[Type] = 'Back Order') AND C.[Stage] <> 'Void')	
		BEGIN
			SELECT @SalesCode = A.SalesOrderCode, @SourceCode = B.SourceCode, @StoreMerchantID = B.StoreMerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @Stage = C.[Stage], @Type = B.Type, @LineNum = A.LineNum 
			FROM inserted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode
			  INNER JOIN CustomerSalesOrderWorkflow C ON B.SalesOrderCode = C.SalesOrderCode

			IF @StoreMerchantID IS NOT NULL
				BEGIN
					INSERT INTO LerrynImportExportActionStatus_DEV000221 (SalesOrderCode_DEV000221, SourceCode_DEV000221, 
					  StoreMerchantID_DEV000221, OrderStatus_DEV000221, SalesOrderDetailLineNum_DEV000221, ActionTimestamp_DEV000221, 
					  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
					  MessageAcknowledged_DEV000221, ErrorReported_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
					VALUES (@SalesCode, @SourceCode, @StoreMerchantID, CASE WHEN @Type = 'Back Order' THEN '205' ELSE '105' END, 
					  @LineNum, getdate(), 0 , Null, 0, 0, 0, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET StatusActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportUpdateOnCustomerOrderWorkflow_DEV000221]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportUpdateOnCustomerOrderWorkflow_DEV000221]
GO

CREATE TRIGGER [dbo].[LerrynImportExportUpdateOnCustomerOrderWorkflow_DEV000221] 
   ON  [dbo].[CustomerSalesOrderWorkflow] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @SalesCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @OldStage nvarchar(50)
	DECLARE @NewStage nvarchar(50)
	DECLARE @Type nvarchar(25)
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @ActionXML nvarchar(1000)
	DECLARE @ActionCreated bit

	SET @ActionCreated = 0

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN CustomerSalesOrder B
			ON	 A.SalesOrderCode = B.SalesOrderCode
			WHERE	(B.[Type] = 'Sales Order' OR B.[Type] = 'Back Order'))
		BEGIN
			SELECT @OldStage = [Stage] FROM deleted 

			SELECT @SalesCode = A.SalesOrderCode, @SourceCode = B.SourceCode, @StoreMerchantID = B.StoreMerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @NewStage = A.[Stage], @Type = B.[Type] 
			  FROM inserted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode

			IF @NewStage = 'Completed' AND @StoreMerchantID IS NOT NULL
				BEGIN
					INSERT INTO LerrynImportExportActionStatus_DEV000221 (SalesOrderCode_DEV000221, SourceCode_DEV000221, 
					  StoreMerchantID_DEV000221, OrderStatus_DEV000221, SalesOrderDetailLineNum_DEV000221, ActionTimestamp_DEV000221, 
					  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
					  MessageAcknowledged_DEV000221, ErrorReported_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
					VALUES (@SalesCode, @SourceCode, @StoreMerchantID, '600', -1, getdate(), 0 , Null, 0, 0, 0, 0, 
					  @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END

			IF @OldStage <> 'Void' AND @NewStage = 'Void' AND @StoreMerchantID IS NOT NULL
				BEGIN
					IF EXISTS (SELECT 1 FROM CustomerCardPayment INNER JOIN CustomerCCAuthResponse 
					  ON CustomerCardPayment.CardPaymentCode = CustomerCCAuthResponse.DocumentCode
					  WHERE CustomerCardPayment.ParentTransactionCode = @SalesCode AND GatewayResponseCode = 'OK')
				        	SET @ActionXML = '<CardAuthorised>Yes</CardAuthorised>'
					ELSE
						SET @ActionXML = '<CardAuthorised>No</CardAuthorised>'

					INSERT INTO LerrynImportExportActionStatus_DEV000221 (SalesOrderCode_DEV000221, SourceCode_DEV000221, 
					  StoreMerchantID_DEV000221, OrderStatus_DEV000221, SalesOrderDetailLineNum_DEV000221, ActionTimestamp_DEV000221, 
					  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
					  MessageAcknowledged_DEV000221, ErrorReported_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
					VALUES (@SalesCode, @SourceCode, @StoreMerchantID, '500', -1, getdate(), 0 , @ActionXML, 0, 0, 0, 0, 
					  @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportActionStatus_DEV000221 SET XMLToSend_DEV000221 = 0, ActionComplete_DEV000221 = -1, 
					  XMLMessageType_DEV000221 = 0 
					WHERE SourceCode_DEV000221 = @SourceCode AND SalesOrderCode_DEV000221 = @SalesCode AND 
					  XMLToSend_DEV000221 = -1 AND ActionComplete_DEV000221 = 0
					  
					SET @ActionCreated = 1
				END

				UPDATE LerrynImportExportServiceAction_DEV000221 SET StatusActionRequired_DEV000221 = 1, 
				  UserModified = @UserModified, DateModified = getdate()

		END
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportUpdateOnCustomerOrderDetail_DEV000221]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportUpdateOnCustomerOrderDetail_DEV000221]
GO

CREATE TRIGGER [dbo].[LerrynImportExportUpdateOnCustomerOrderDetail_DEV000221] 
   ON  [dbo].[CustomerSalesOrderDetail] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @SalesCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @Stage nvarchar(50)
	DECLARE @Type nvarchar(25)
	DECLARE @LineNum int
	DECLARE @OrigQty numeric(18,6)
	DECLARE @NewQty numeric(18,6)
	DECLARE @OrigPrice numeric(18,6)
	DECLARE @NewPrice numeric(18,6)
	DECLARE @OrigItemTotal numeric(18,6)
	DECLARE @NewItemTotal numeric(18,6)
	DECLARE @OrigSalesTax numeric(18,6)
	DECLARE @NewSalesTax numeric(18,6)
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @ActionXML nvarchar(1000)
	DECLARE @ActionCreated bit

	SET @ActionCreated = 0

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode 
			INNER JOIN CustomerSalesOrderWorkflow C ON B.SalesOrderCode = C.SalesOrderCode
			WHERE	(B.[Type] = 'Sales Order' OR B.[Type] = 'Back Order') AND C.[Stage] <> 'Void')	
		BEGIN
			SELECT @OrigQty = A.QuantityOrdered, @OrigPrice = A.SalesPrice, @OrigItemTotal = A.ExtPrice, 
			  @OrigSalesTax = A.SalesTaxAmount FROM deleted A 

			SELECT @SalesCode = A.SalesOrderCode, @SourceCode = B.SourceCode, @StoreMerchantID = B.StoreMerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @Stage = C.[Stage], @Type = B.[Type], 
			  @LineNum = A.LineNum, @NewQty = A.QuantityOrdered, @NewPrice = A.SalesPrice, @NewItemTotal = A.ExtPrice, 
			  @NewSalesTax = A.SalesTaxAmount 
			FROM inserted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode
			  INNER JOIN CustomerSalesOrderWorkflow C ON B.SalesOrderCode = C.SalesOrderCode

			IF @NewQty <> @OrigQty AND @Stage <> 'Void' AND @StoreMerchantID IS NOT NULL
				BEGIN
					SET @ActionXML = '<ItemQtyChanged><OriginalQty>' + CAST(@OrigQty AS nvarchar) + '</OriginalQty>' + 
					  '<NewQty>' + CAST(@NewQty AS nvarchar) + '</NewQty><OldItemTotal>' + CAST(@OrigItemTotal AS nvarchar) + 
					  '</OldItemTotal><NewItemTotal>' + CAST(@NewItemTotal AS nvarchar) + '</NewItemTotal><OldSalesTax>' + 
					  CAST(@OrigSalesTax AS nvarchar) + '</OldSalesTax><NewSalesTax>' + CAST(@NewSalesTax AS nvarchar) + 
					  '</NewSalesTax></ItemQtyChanged>'

					INSERT INTO LerrynImportExportActionStatus_DEV000221 (SalesOrderCode_DEV000221, SourceCode_DEV000221, 
					  StoreMerchantID_DEV000221, OrderStatus_DEV000221, SalesOrderDetailLineNum_DEV000221, ActionTimestamp_DEV000221, 
					  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
					  MessageAcknowledged_DEV000221, ErrorReported_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
					VALUES (@SalesCode, @SourceCode, @StoreMerchantID, '305', @LineNum, getdate(), 0 , @ActionXML, 0, 0, 
					  0, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END

			IF @NewQty = @OrigQty AND @OrigPrice <> @NewPrice AND @Stage <> 'Void' AND @StoreMerchantID IS NOT NULL
				BEGIN
					SET @ActionXML = '<ItemPriceChanged><OriginalPrice>' + CAST(@OrigPrice AS nvarchar) + '</OriginalPrice>' + 
					  '<NewPrice>' + CAST(@NewPrice AS nvarchar) + '</NewPrice><OldSalesTax>' + CAST(@OrigSalesTax AS nvarchar) + 
					  '</OldSalesTax><NewSalesTax>' + CAST(@NewSalesTax AS nvarchar) + '</NewSalesTax></ItemPriceChanged>'

					INSERT INTO LerrynImportExportActionStatus_DEV000221 (SalesOrderCode_DEV000221, SourceCode_DEV000221, 
					  StoreMerchantID_DEV000221, OrderStatus_DEV000221, SalesOrderDetailLineNum_DEV000221, ActionTimestamp_DEV000221, 
					  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
					  MessageAcknowledged_DEV000221, ErrorReported_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
					VALUES (@SalesCode, @SourceCode, @StoreMerchantID, '310', @LineNum, getdate(), 0 , @ActionXML, 0, 0, 
					  0, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END

			IF @ActionCreated = 1
				UPDATE LerrynImportExportServiceAction_DEV000221 SET StatusActionRequired_DEV000221 = 1, 
				  UserModified = @UserModified, DateModified = getdate()

		END
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportDeleteOnCustomerOrderDetail_DEV000221]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportDeleteOnCustomerOrderDetail_DEV000221]
GO

CREATE TRIGGER [dbo].[LerrynImportExportDeleteOnCustomerOrderDetail_DEV000221] 
   ON  [dbo].[CustomerSalesOrderDetail] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @SalesCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @Stage nvarchar(50)
	DECLARE @Type nvarchar(25)
	DECLARE @LineNum int
	DECLARE @OrigQty numeric(18,6)
	DECLARE @OrigPrice numeric(18,6)
	DECLARE @OrigItemTotal numeric(18,6)
	DECLARE @OrigSalesTax numeric(18,6)
	DECLARE @StorePurchaseID nvarchar(30)
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @ActionXML nvarchar(1000)

	IF EXISTS(SELECT 1 FROM deleted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode 
			INNER JOIN CustomerSalesOrderWorkflow C ON B.SalesOrderCode = C.SalesOrderCode
			WHERE	(B.[Type] = 'Sales Order' OR B.[Type] = 'Back Order') AND C.[Stage] <> 'Void')	
		BEGIN
			SELECT @SalesCode = A.SalesOrderCode, @SourceCode = B.SourceCode, @StoreMerchantID = B.StoreMerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @Stage = C.[Stage], @Type = B.[Type], 
			  @OrigQty = A.QuantityOrdered, @OrigPrice = A.SalesPrice, @OrigItemTotal = A.ExtPrice, @OrigSalesTax = A.SalesTaxAmount, 
			  @StorePurchaseID = A.SourcePurchaseID_DEV000221, @LineNum = A.LineNum
			FROM deleted A INNER JOIN CustomerSalesOrder B ON A.SalesOrderCode = B.SalesOrderCode
			  INNER JOIN CustomerSalesOrderWorkflow C ON B.SalesOrderCode = C.SalesOrderCode

			IF @Stage <> 'Void' AND @StoreMerchantID IS NOT NULL
				BEGIN
					SET @ActionXML = '<ItemDeleted><StorePurchaseID>' + @StorePurchaseID + '</StorePurchaseID><OriginalQty>' + 
					  CAST(@OrigQty AS nvarchar) + '</OriginalQty><OriginalTotal>' + CAST(@OrigItemTotal AS nvarchar) + 
					  '</OriginalTotal><OldSalesTax>' + CAST(@OrigSalesTax AS nvarchar) + '</OldSalesTax></ItemDeleted>'

					INSERT INTO LerrynImportExportActionStatus_DEV000221 (SalesOrderCode_DEV000221, SourceCode_DEV000221, 
					  StoreMerchantID_DEV000221, OrderStatus_DEV000221, SalesOrderDetailLineNum_DEV000221, ActionTimestamp_DEV000221, 
					  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
					  MessageAcknowledged_DEV000221, ErrorReported_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
					VALUES (@SalesCode, @SourceCode, @StoreMerchantID, '350', @LineNum, getdate(), 0 , @ActionXML, 0, 0, 
					  0, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET StatusActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END
END
GO



IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportUpdateOnInventoryItem]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportUpdateOnInventoryItem]
GO

CREATE TRIGGER dbo.LerrynImportExportUpdateOnInventoryItem
   ON  dbo.InventoryItem 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @ItemCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @StoreCatalogID nvarchar(30)
	/* start of code added TJS 20/11/13 */
	DECLARE @IsPublishOn3DCart bit
	DECLARE @WasPublishOn3DCart bit
	/* end of code added TJS 20/11/13 */
	DECLARE @IsPublishOnAmazon bit
	DECLARE @WasPublishOnAmazon bit
	DECLARE @IsPublishOnASPStorefront bit
	DECLARE @WasPublishOnASPStorefront bit
	DECLARE @IsPublishOnChannelAdv bit
	DECLARE @WasPublishOnChannelAdv bit
	DECLARE @IsPublishOnMagento bit
	DECLARE @WasPublishOnMagento bit
	DECLARE @IsPublishOnShopCom bit
	DECLARE @WasPublishOnShopCom bit
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @Status nvarchar(5)
	DECLARE @ActionCreated bit
	DECLARE @ActionTimestamp datetime

	SET @ActionCreated = 0

	SELECT @WasPublishOnAmazon = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryAmazonDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221

	SELECT @WasPublishOnASPStorefront = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryASPStorefrontDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221

	SELECT @WasPublishOnChannelAdv = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryChannelAdvDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221

	SELECT @WasPublishOnMagento = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryMagentoDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221

	SELECT @WasPublishOnShopCom = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryShopComDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221

	/* start of code added TJS 20/11/13 */
	DECLARE Item3DCart_Cursor CURSOR FOR SELECT A.ItemCode, B.StoreID_DEV000221, '3DCartOrder' AS SourceCode, 
	    A.UserCreated, A.UserModified, B.Publish_DEV000221, getdate() AS ActionTimestamp
	  FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

	OPEN Item3DCart_Cursor
	FETCH NEXT FROM Item3DCart_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
	  @IsPublishOn3DCart, @ActionTimestamp
	WHILE @@FETCH_STATUS = 0
		BEGIN

			SELECT @WasPublishOn3DCart = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN Inventory3DCartDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.StoreID_DEV000221 = @StoreMerchantID

			IF EXISTS(SELECT 1 FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.StoreID_DEV000221 = @StoreMerchantID)
				BEGIN
					SELECT @IsPublishOn3DCart = B.Publish_DEV000221 FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 B 
							ON A.ItemCode = B.ItemCode_DEV000221 AND B.StoreID_DEV000221 = @StoreMerchantID

					IF @IsPublishOn3DCart = 1
						BEGIN
							IF @WasPublishOn3DCart = 0
								SET @Status = '100'
							ELSE
								SET @Status = '200'

							IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
							  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
							  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
							  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
							  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
								INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
								  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
								  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
								  UserCreated, DateCreated, UserModified, DateModified) 
								VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
								  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

							SET @ActionCreated = 1
						END
					ELSE
						IF @WasPublishOn3DCart = 1
							BEGIN
								IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
								  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
								  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
								  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
								  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
									INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
									  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
									  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
									  UserCreated, DateCreated, UserModified, DateModified) 
									VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
									  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

								SET @ActionCreated = 1
							END
				END
		END
	CLOSE Item3DCart_Cursor
	DEALLOCATE Item3DCart_Cursor
	/* end of code added TJS 20/11/13 */

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'AmazonOrder', @StoreMerchantID = B.MerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @IsPublishOnAmazon = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @IsPublishOnAmazon = 1
				BEGIN
					IF @WasPublishOnAmazon = 0
						SET @Status = '100'
					ELSE
						SET @Status = '200'

					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
			ELSE
				IF @WasPublishOnAmazon = 1
					BEGIN
						IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
						  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
						  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
						  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
						  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
							INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
							  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
							  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
							  UserCreated, DateCreated, UserModified, DateModified) 
							VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
							  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

						SET @ActionCreated = 1
					END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ASPStoreFrontOrder', @StoreMerchantID = B.SiteID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @IsPublishOnASPStorefront = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @IsPublishOnASPStorefront = 1
				BEGIN
					IF @WasPublishOnASPStorefront = 0
						SET @Status = '100'
					ELSE
						SET @Status = '200'

					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
			ELSE
				IF @WasPublishOnASPStorefront = 1
					BEGIN
						IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
						  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
						  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
						  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
						  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
							INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
							  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
							  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
							  UserCreated, DateCreated, UserModified, DateModified) 
							VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
							  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

						SET @ActionCreated = 1
					END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ChanAdvOrder', @StoreMerchantID = B.AccountID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @IsPublishOnChannelAdv = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @IsPublishOnChannelAdv = 1
				BEGIN
					IF @WasPublishOnChannelAdv = 0
						SET @Status = '100'
					ELSE
						SET @Status = '200'

					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
			ELSE
				IF @WasPublishOnChannelAdv = 1
					BEGIN
						IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
						  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
						  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
						  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
						  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
							INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
							  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
							  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
							  UserCreated, DateCreated, UserModified, DateModified) 
							VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
							  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

						SET @ActionCreated = 1
					END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'MagentoOrder', @StoreMerchantID = B.InstanceID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @IsPublishOnMagento = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @IsPublishOnMagento = 1
				BEGIN
					IF @WasPublishOnMagento = 0
						SET @Status = '100'
					ELSE
						SET @Status = '200'

					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
			ELSE
				IF @WasPublishOnMagento = 1
					BEGIN
						IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
						  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
						  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
						  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
						  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
							INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
							  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
							  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
							  UserCreated, DateCreated, UserModified, DateModified) 
							VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
							  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

						SET @ActionCreated = 1
					END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryShopComDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ShopComOrder', @StoreCatalogID = B.CatalogID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @IsPublishOnShopCom = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryShopComDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @IsPublishOnShopCom = 1
				BEGIN
					IF @WasPublishOnShopCom = 0
						SET @Status = '100'
					ELSE
						SET @Status = '200'

					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvItem', @Status, @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
			ELSE
				IF @WasPublishOnShopCom = 1
					BEGIN
						IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
						  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
						  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvItem' AND 
						  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
						  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
							INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
							  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
							  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
							  UserCreated, DateCreated, UserModified, DateModified) 
							VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvItem', '300', @ActionTimestamp, 
							  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

						SET @ActionCreated = 1
					END
		END

	IF @ActionCreated = 1
		UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
		  UserModified = @UserModified, DateModified = getdate()

END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportDeleteOnInventoryItem]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportDeleteOnInventoryItem]
GO

CREATE TRIGGER dbo.LerrynImportExportDeleteOnInventoryItem
   ON  dbo.InventoryItem 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @ItemCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @StoreCatalogID nvarchar(30)
	DECLARE @WasPublishOn3DCart bit /* TJS 20/11/13 */
	DECLARE @WasPublishOnAmazon bit
	DECLARE @WasPublishOnASPStorefront bit
	DECLARE @WasPublishOnChannelAdv bit
	DECLARE @WasPublishOnMagento bit
	DECLARE @WasPublishOnShopCom bit
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @ActionTimestamp datetime

	/* start of code added TJS 20/11/13 */
	IF EXISTS(SELECT 1 FROM deleted A INNER JOIN Inventory3DCartDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = '3DCartOrder', @StoreMerchantID = B.StoreID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @WasPublishOn3DCart = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM deleted A INNER JOIN Inventory3DCartDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @WasPublishOn3DCart = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END
	/* end of code added TJS 20/11/13 */

	IF EXISTS(SELECT 1 FROM deleted A INNER JOIN InventoryAmazonDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'AmazonOrder', @StoreMerchantID = B.MerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @WasPublishOnAmazon = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM deleted A INNER JOIN InventoryAmazonDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @WasPublishOnAmazon = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END

	IF EXISTS(SELECT 1 FROM deleted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ASPStoreFrontOrder', @StoreMerchantID = B.SiteID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @WasPublishOnASPStorefront = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM deleted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @WasPublishOnASPStorefront = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END

	IF EXISTS(SELECT 1 FROM deleted A INNER JOIN InventoryChannelAdvDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ChanAdvOrder', @StoreMerchantID = B.AccountID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @WasPublishOnChannelAdv = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM deleted A INNER JOIN InventoryChannelAdvDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @WasPublishOnChannelAdv = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END

	IF EXISTS(SELECT 1 FROM deleted A INNER JOIN InventoryMagentoDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'MagentoOrder', @StoreMerchantID = B.InstanceID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @WasPublishOnMagento = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM deleted A INNER JOIN InventoryMagentoDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @WasPublishOnMagento = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END

	IF EXISTS(SELECT 1 FROM deleted A INNER JOIN InventoryShopComDetails_DEV000221 B
			ON	 A.ItemCode = B.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ShopComOrder', @StoreCatalogID = B.CAtalogID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @WasPublishOnShopCom = B.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM deleted A INNER JOIN InventoryShopComDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

			IF @WasPublishOnShopCom = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvItem' AND 
					  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvItem', '300', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
					  UserModified = @UserModified, DateModified = getdate()
				END
		END

END
GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportUpdateOnInventoryItemDescription]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportUpdateOnInventoryItemDescription]
GO

CREATE TRIGGER dbo.LerrynImportExportUpdateOnInventoryItemDescription
   ON  dbo.InventoryItemDescription 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @ItemCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @StoreCatalogID nvarchar(30)
	DECLARE @PublishOn3DCart bit /* TJS 20/11/13 */
	DECLARE @PublishOnAmazon bit
	DECLARE @PublishOnASPStorefront bit
	DECLARE @PublishOnChannelAdv bit
	DECLARE @PublishOnMagento bit
	DECLARE @PublishOnShopCom bit
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @ActionCreated bit
	DECLARE @ActionTimestamp datetime

	SET @ActionCreated = 0

	/* start of code added TJS 20/11/13 */
	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = '3DCartOrder', @StoreMerchantID = C.StoreID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOn3DCart = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOn3DCart = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END
	/* end of code added TJS 20/11/13 */

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'AmazonOrder', @StoreMerchantID = C.MerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnAmazon = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnAmazon = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ASPStoreFrontOrder', @StoreMerchantID = C.SiteID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnASPStorefront = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnASPStorefront = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ChanAdvOrder', @StoreMerchantID = C.AccountID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnChannelAdv = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnChannelAdv = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'MagentoOrder', @StoreMerchantID = C.InstanceID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnMagento = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnMagento = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryItem B
			ON A.ItemCode = B.ItemCode INNER JOIN InventoryShopComDetails_DEV000221 C
			ON	 A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ShopComOrder', @StoreCatalogID = C.CatalogID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnShopCom = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryItem B ON A.ItemCode = B.ItemCode
			  INNER JOIN InventoryShopComDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnShopCom = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF @ActionCreated = 1
		UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
		  UserModified = @UserModified, DateModified = getdate()

END
GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportUpdateOnInventoryItemWebOptionDescription]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportUpdateOnInventoryItemWebOptionDescription]
GO

CREATE TRIGGER [dbo].[LerrynImportExportUpdateOnInventoryItemWebOptionDescription]
   ON  [dbo].[InventoryItemWebOptionDescription] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @ItemCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @StoreCatalogID nvarchar(30)
	DECLARE @PublishOn3DCart bit
	DECLARE @PublishOnAmazon bit
	DECLARE @PublishOnASPStorefront bit
	DECLARE @PublishOnChannelAdv bit
	DECLARE @PublishOnMagento bit
	DECLARE @PublishOnShopCom bit
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @ActionCreated bit
	DECLARE @ActionTimestamp datetime

	SET @ActionCreated = 0

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = '3DCartOrder', @StoreMerchantID = C.StoreID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOn3DCart = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOn3DCart = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'AmazonOrder', @StoreMerchantID = C.MerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnAmazon = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnAmazon = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ASPStoreFrontOrder', @StoreMerchantID = C.SiteID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnASPStorefront = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnASPStorefront = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ChanAdvOrder', @StoreMerchantID = C.AccountID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnChannelAdv = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnChannelAdv = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'MagentoOrder', @StoreMerchantID = C.InstanceID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnMagento = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnMagento = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryItem B
			ON A.ItemCode = B.ItemCode INNER JOIN InventoryShopComDetails_DEV000221 C
			ON	 A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ShopComOrder', @StoreCatalogID = C.CatalogID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnShopCom = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryItem B ON A.ItemCode = B.ItemCode
			  INNER JOIN InventoryShopComDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnShopCom = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvDesc' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvDesc', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF @ActionCreated = 1
		UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
		  UserModified = @UserModified, DateModified = getdate()

END

GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportUpdateOnInventoryItemPricingDetail]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportUpdateOnInventoryItemPricingDetail]
GO

CREATE TRIGGER [dbo].[LerrynImportExportUpdateOnInventoryItemPricingDetail]
   ON  [dbo].[InventoryItemPricingDetail] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @ItemCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @StoreCatalogID nvarchar(30)
	DECLARE @PublishOn3DCart bit /* TJS 20/11/13 */
	DECLARE @PublishOnAmazon bit
	DECLARE @PublishOnASPStorefront bit
	DECLARE @PublishOnChannelAdv bit
	DECLARE @PublishOnMagento bit
	DECLARE @PublishOnShopCom bit
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @ActionCreated bit
	DECLARE @ActionTimestamp datetime

	SET @ActionCreated = 0

	/* start of code added TJS 20/11/13 */
	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'AmazonOrder', @StoreMerchantID = C.StoreID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOn3DCart = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOn3DCart = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvPrice' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvPrice', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END
	/* end of code added TJS 20/11/13 */

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'AmazonOrder', @StoreMerchantID = C.MerchantID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnAmazon = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnAmazon = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvPrice' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvPrice', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ASPStoreFrontOrder', @StoreMerchantID = C.SiteID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnASPStorefront = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnASPStorefront = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvPrice' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvPrice', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ChanAdvOrder', @StoreMerchantID = C.AccountID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnChannelAdv = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnChannelAdv = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvPrice' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvPrice', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 C 
			ON A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'MagentoOrder', @StoreMerchantID = C.InstanceID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnMagento = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnMagento = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvPrice' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvPrice', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryItem B
			ON A.ItemCode = B.ItemCode INNER JOIN InventoryShopComDetails_DEV000221 C
			ON	 A.ItemCode = C.ItemCode_DEV000221)
		BEGIN
			SELECT @ItemCode = A.ItemCode, @SourceCode = 'ShopComOrder', @StoreCatalogID = C.CatalogID_DEV000221, 
			  @UserCreated = A.UserCreated, @UserModified = A.UserModified, @PublishOnShopCom = C.Publish_DEV000221, 
			  @ActionTimestamp = getdate()
			FROM inserted A INNER JOIN InventoryItem B ON A.ItemCode = B.ItemCode
			  INNER JOIN InventoryShopComDetails_DEV000221 C ON A.ItemCode = C.ItemCode_DEV000221

			IF @PublishOnShopCom = 1
				BEGIN
					IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
					  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
					  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvPrice' AND 
					  InventoryStatus_DEV000221 = '200' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
					  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
						INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
						  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
						  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
						  UserCreated, DateCreated, UserModified, DateModified) 
						VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvPrice', '200', @ActionTimestamp, 
						  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

					SET @ActionCreated = 1
				END
		END

	IF @ActionCreated = 1
		UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
		  UserModified = @UserModified, DateModified = getdate()

END

GO

/* start of code added TJS 05/11/11 */
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportUpdateOnInventoryItem]') AND OBJECTPROPERTY(id,N'IsTrigger') = 1)
DROP TRIGGER [dbo].[LerrynImportExportUpdateOnInventoryItem]
GO

CREATE TRIGGER dbo.LerrynImportExportUpdateOnInventoryItem
   ON  dbo.InventoryItem 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @ItemCode nvarchar(30)
	DECLARE @SourceCode nvarchar(30)
	DECLARE @StoreMerchantID nvarchar(40)
	DECLARE @StoreCatalogID nvarchar(30)
	/* start of code added TJS 20/11/13 */
	DECLARE @IsPublishOn3DCart bit
	DECLARE @WasPublishOn3DCart bit
	/* end of code added TJS 20/11/13 */
	DECLARE @IsPublishOnAmazon bit
	DECLARE @WasPublishOnAmazon bit
	DECLARE @IsPublishOnASPStorefront bit
	DECLARE @WasPublishOnASPStorefront bit
	DECLARE @IsPublishOnChannelAdv bit
	DECLARE @WasPublishOnChannelAdv bit
	DECLARE @IsPublishOnMagento bit
	DECLARE @WasPublishOnMagento bit
	DECLARE @IsPublishOnShopCom bit
	DECLARE @WasPublishOnShopCom bit
	DECLARE @UserCreated nvarchar(30)
	DECLARE @UserModified nvarchar(30)
	DECLARE @Status nvarchar(5)
	DECLARE @ActionCreated bit
	DECLARE @ActionTimestamp datetime

	SET @ActionCreated = 0

	/* start of code added TJS 20/11/13 */
	DECLARE Item3DCart_Cursor CURSOR FOR SELECT A.ItemCode, B.StoreID_DEV000221, '3DCartOrder' AS SourceCode, 
	    A.UserCreated, A.UserModified, B.Publish_DEV000221, getdate() AS ActionTimestamp
	  FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

	OPEN Item3DCart_Cursor
	FETCH NEXT FROM Item3DCart_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
	  @IsPublishOn3DCart, @ActionTimestamp
	WHILE @@FETCH_STATUS = 0
		BEGIN

			SELECT @WasPublishOn3DCart = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN Inventory3DCartDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.StoreID_DEV000221 = @StoreMerchantID

			IF EXISTS(SELECT 1 FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.StoreID_DEV000221 = @StoreMerchantID)
				BEGIN
					SELECT @IsPublishOn3DCart = B.Publish_DEV000221 FROM inserted A INNER JOIN Inventory3DCartDetails_DEV000221 B 
							ON A.ItemCode = B.ItemCode_DEV000221 AND B.StoreID_DEV000221 = @StoreMerchantID

					IF @IsPublishOn3DCart = 1
						BEGIN
							IF @WasPublishOn3DCart = 0
								SET @Status = '100'
							ELSE
								SET @Status = '200'

							IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
							  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
							  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
							  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
							  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
								INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
								  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
								  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
								  UserCreated, DateCreated, UserModified, DateModified) 
								VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
								  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

							SET @ActionCreated = 1
						END
					ELSE
						IF @WasPublishOn3DCart = 1
							BEGIN
								IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
								  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
								  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
								  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
								  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
									INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
									  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
									  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
									  UserCreated, DateCreated, UserModified, DateModified) 
									VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
									  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

								SET @ActionCreated = 1
							END
				END
			FETCH NEXT FROM Item3DCart_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
			  @IsPublishOn3DCart, @ActionTimestamp
		END
	CLOSE Item3DCart_Cursor
	DEALLOCATE Item3DCart_Cursor


	DECLARE ItemAmazon_Cursor CURSOR FOR SELECT A.ItemCode, B.MerchantID_DEV000221, 'AmazonOrder' AS SourceCode, 
	    A.UserCreated, A.UserModified, B.Publish_DEV000221, getdate() AS ActionTimestamp
	  FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

	OPEN ItemAmazon_Cursor
	FETCH NEXT FROM ItemAmazon_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
	  @IsPublishOnAmazon, @ActionTimestamp
	WHILE @@FETCH_STATUS = 0
		BEGIN
		/* end of code added TJS 20/11/13 */
			SELECT @WasPublishOnAmazon = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryAmazonDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.MerchantID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */
					
			IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.MerchantID_DEV000221 = @StoreMerchantID) /* TJS 20/11/13 */
				BEGIN
					SELECT @IsPublishOnAmazon = B.Publish_DEV000221 FROM inserted A INNER JOIN InventoryAmazonDetails_DEV000221 B 
					  ON A.ItemCode = B.ItemCode_DEV000221 AND B.MerchantID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

					IF @IsPublishOnAmazon = 1
						BEGIN
							IF @WasPublishOnAmazon = 0
								SET @Status = '100'
							ELSE
								SET @Status = '200'

						IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
							  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
							  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
							  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
							  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
								INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
								  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
								  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
								  UserCreated, DateCreated, UserModified, DateModified) 
								VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
								  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

							SET @ActionCreated = 1
						END
					ELSE
						IF @WasPublishOnAmazon = 1
							BEGIN
								IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
								  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
								  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
								  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
								  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
									INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
									  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
									  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
									  UserCreated, DateCreated, UserModified, DateModified) 
									VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
									  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

								SET @ActionCreated = 1
							END
				END
			/* start of code added TJS 20/11/13 */
			FETCH NEXT FROM ItemAmazon_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
			  @IsPublishOnAmazon, @ActionTimestamp
		END
	CLOSE ItemAmazon_Cursor
	DEALLOCATE ItemAmazon_Cursor

	DECLARE ItemASPStorefront_Cursor CURSOR FOR SELECT A.ItemCode, B.SiteID_DEV000221, 'ASPStoreFrontOrder' AS SourceCode, 
	    A.UserCreated, A.UserModified, B.Publish_DEV000221, getdate() AS ActionTimestamp
	  FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

	OPEN ItemASPStorefront_Cursor
	FETCH NEXT FROM ItemASPStorefront_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
	  @IsPublishOnASPStorefront, @ActionTimestamp
	WHILE @@FETCH_STATUS = 0
		BEGIN
			/* end of code added TJS 20/11/13 */
			SELECT @WasPublishOnASPStorefront = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryASPStorefrontDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.SiteID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

			IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.SiteID_DEV000221 = @StoreMerchantID) /* TJS 20/11/13 */
				BEGIN
					SELECT @IsPublishOnASPStorefront = B.Publish_DEV000221 FROM inserted A INNER JOIN InventoryASPStorefrontDetails_DEV000221 B 
					  ON A.ItemCode = B.ItemCode_DEV000221 AND B.SiteID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

					IF @IsPublishOnASPStorefront = 1
						BEGIN
							IF @WasPublishOnASPStorefront = 0
								SET @Status = '100'
							ELSE
								SET @Status = '200'

							IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
							  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
							  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
							  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
							  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
								INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
								  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
								  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
								  UserCreated, DateCreated, UserModified, DateModified) 
								VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
								  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

							SET @ActionCreated = 1
						END
					ELSE
						IF @WasPublishOnASPStorefront = 1
							BEGIN
								IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
								  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
								  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
								  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
								  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
									INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
									  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
									  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
									  UserCreated, DateCreated, UserModified, DateModified) 
									VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
									  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

								SET @ActionCreated = 1
							END
				END
			/* start of code added TJS 20/11/13 */
			FETCH NEXT FROM ItemASPStorefront_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
			  @IsPublishOnASPStorefront, @ActionTimestamp
		END
	CLOSE ItemASPStorefront_Cursor
	DEALLOCATE ItemASPStorefront_Cursor

	DECLARE ItemChanAdv_Cursor CURSOR FOR SELECT A.ItemCode, B.AccountID_DEV000221, 'ChanAdvOrder' AS SourceCode, 
	    A.UserCreated, A.UserModified, B.Publish_DEV000221, getdate() AS ActionTimestamp
	  FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

	OPEN ItemChanAdv_Cursor
	FETCH NEXT FROM ItemChanAdv_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
	  @IsPublishOnChannelAdv, @ActionTimestamp
	WHILE @@FETCH_STATUS = 0
		BEGIN
			/* end of code added TJS 20/11/13 */
			SELECT @WasPublishOnChannelAdv = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryChannelAdvDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.AccountID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

			IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.AccountID_DEV000221 = @StoreMerchantID) /* TJS 20/11/13 */
				BEGIN
					SELECT @IsPublishOnChannelAdv = B.Publish_DEV000221 FROM inserted A INNER JOIN InventoryChannelAdvDetails_DEV000221 B 
							ON A.ItemCode = B.ItemCode_DEV000221 AND B.AccountID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

					IF @IsPublishOnChannelAdv = 1
						BEGIN
							IF @WasPublishOnChannelAdv = 0
								SET @Status = '100'
							ELSE
								SET @Status = '200'

							IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
							  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
							  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
							  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
							  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
								INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
								  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
								  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
								  UserCreated, DateCreated, UserModified, DateModified) 
								VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
								  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

							SET @ActionCreated = 1
						END
					ELSE
						IF @WasPublishOnChannelAdv = 1
							BEGIN
								IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
								  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
								  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
								  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
								  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
									INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
									  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
									  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
									  UserCreated, DateCreated, UserModified, DateModified) 
									VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
									  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

								SET @ActionCreated = 1
							END
				END
			/* start of code added TJS 20/11/13 */
			FETCH NEXT FROM ItemChanAdv_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
			  @IsPublishOnChannelAdv, @ActionTimestamp
		END
	CLOSE ItemChanAdv_Cursor
	DEALLOCATE ItemChanAdv_Cursor

	DECLARE ItemMagento_Cursor CURSOR FOR SELECT A.ItemCode, B.InstanceID_DEV000221, 'MagentoOrder' AS SourceCode, 
	    A.UserCreated, A.UserModified, B.Publish_DEV000221, getdate() AS ActionTimestamp
	FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

	OPEN ItemMagento_Cursor
	FETCH NEXT FROM ItemMagento_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
	  @IsPublishOnMagento, @ActionTimestamp
	WHILE @@FETCH_STATUS = 0
		BEGIN
			/* end of code added TJS 20/11/13 */
			SELECT @WasPublishOnMagento = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryMagentoDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.InstanceID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

			IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.InstanceID_DEV000221 = @StoreMerchantID) /* TJS 20/11/13 */
				BEGIN
					SELECT @IsPublishOnMagento = B.Publish_DEV000221 FROM inserted A INNER JOIN InventoryMagentoDetails_DEV000221 B 
							ON A.ItemCode = B.ItemCode_DEV000221 AND B.InstanceID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

					IF @IsPublishOnMagento = 1
						BEGIN
							IF @WasPublishOnMagento = 0
								SET @Status = '100'
							ELSE
								SET @Status = '200'

							IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
							  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
							  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
							  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
							  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
								INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
								  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
								  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
								  UserCreated, DateCreated, UserModified, DateModified) 
								VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', @Status, @ActionTimestamp, 
								  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

							SET @ActionCreated = 1
						END
					ELSE
						IF @WasPublishOnMagento = 1
							BEGIN
								IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
								  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
								  StoreMerchantID_DEV000221 = @StoreMerchantID AND TableTagID_DEV000221 = 'InvItem' AND 
								  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
								  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
									INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
									  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
									  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
									  UserCreated, DateCreated, UserModified, DateModified) 
									VALUES (@ItemCode, @SourceCode, @StoreMerchantID, 'InvItem', '300', @ActionTimestamp, 
									  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

								SET @ActionCreated = 1
							END
				END
			/* start of code added TJS 20/11/13 */
			FETCH NEXT FROM ItemMagento_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
			  @IsPublishOnMagento, @ActionTimestamp
		END
	CLOSE ItemMagento_Cursor
	DEALLOCATE ItemMagento_Cursor

	DECLARE ItemShopCom_Cursor CURSOR FOR SELECT A.ItemCode, B.CatalogID_DEV000221, 'ShopComOrder' AS SourceCode, 
	    A.UserCreated, A.UserModified, B.Publish_DEV000221, getdate() AS ActionTimestamp
	FROM inserted A INNER JOIN InventoryShopComDetails_DEV000221 B ON A.ItemCode = B.ItemCode_DEV000221

	OPEN ItemShopCom_Cursor
	FETCH NEXT FROM ItemShopCom_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
	  @IsPublishOnShopCom, @ActionTimestamp
	WHILE @@FETCH_STATUS = 0
		BEGIN
			/* end of code added TJS 20/11/13 */
			SELECT @WasPublishOnShopCom = ISNULL(B.Publish_DEV000221, 0) FROM deleted A LEFT JOIN InventoryShopComDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.CatalogID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

			IF EXISTS(SELECT 1 FROM inserted A INNER JOIN InventoryShopComDetails_DEV000221 B
					ON	 A.ItemCode = B.ItemCode_DEV000221 AND B.CatalogID_DEV000221 = @StoreMerchantID) /* TJS 20/11/13 */
				BEGIN
					SELECT @IsPublishOnShopCom = B.Publish_DEV000221 FROM inserted A INNER JOIN InventoryShopComDetails_DEV000221 B 
							ON A.ItemCode = B.ItemCode_DEV000221 AND B.CatalogID_DEV000221 = @StoreMerchantID /* TJS 20/11/13 */

					IF @IsPublishOnShopCom = 1
						BEGIN
							IF @WasPublishOnShopCom = 0
								SET @Status = '100'
							ELSE
								SET @Status = '200'

							IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
							  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
							  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvItem' AND 
							  InventoryStatus_DEV000221 = @Status AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
							  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
								INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
								  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
								  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
								  UserCreated, DateCreated, UserModified, DateModified) 
								VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvItem', @Status, @ActionTimestamp, 
								  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

							SET @ActionCreated = 1
						END
					ELSE
						IF @WasPublishOnShopCom = 1
							BEGIN
								IF NOT EXISTS (SELECT Counter FROM LerrynImportExportInventoryActionStatus_DEV000221 WHERE 
								  ItemCode_DEV000221 = @ItemCode AND SourceCode_DEV000221 = @SourceCode AND 
								  StoreMerchantID_DEV000221 = @StoreCatalogID AND TableTagID_DEV000221 = 'InvItem' AND 
								  InventoryStatus_DEV000221 = '300' AND ActionTimestamp_DEV000221 >= DATEADD(ms, -20, @ActionTimestamp) AND
								  ActionTimestamp_DEV000221 <= DATEADD(ms, 1, @ActionTimestamp))
									INSERT INTO LerrynImportExportInventoryActionStatus_DEV000221 (ItemCode_DEV000221, SourceCode_DEV000221, 
									  StoreMerchantID_DEV000221, TableTagID_DEV000221, InventoryStatus_DEV000221, ActionTimestamp_DEV000221, 
									  ActionComplete_DEV000221, ActionXMLFile_DEV000221, XMLMessageType_DEV000221, XMLToSend_DEV000221, 
									  UserCreated, DateCreated, UserModified, DateModified) 
									VALUES (@ItemCode, @SourceCode, @StoreCatalogID, 'InvItem', '300', @ActionTimestamp, 
									  0 , Null, 1, 0, @UserCreated, getdate(), @UserModified, getdate()) 

								SET @ActionCreated = 1
							END
				END
			/* start of code added TJS 20/11/13 */
			FETCH NEXT FROM ItemShopCom_Cursor INTO @ItemCode, @StoreMerchantID, @SourceCode, @UserCreated, @UserModified, 
			  @IsPublishOnShopCom, @ActionTimestamp
		END
	CLOSE ItemShopCom_Cursor
	DEALLOCATE ItemShopCom_Cursor
	/* end of code added TJS 20/11/13 */

	IF @ActionCreated = 1
		UPDATE LerrynImportExportServiceAction_DEV000221 SET InventoryActionRequired_DEV000221 = 1, 
		  UserModified = @UserModified, DateModified = getdate()

END
GO
/* end of code added TJS 05/11/11 */


IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[Customer]') AND name = N'LerrynImportExportCustomerSourceIndex')
  CREATE NONCLUSTERED INDEX [LerrynImportExportCustomerSourceIndex] ON [dbo].[Customer] 
  (
	[ImportCustomerID_DEV000221] ASC,
	[ImportSourceID_DEV000221] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO

IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[CustomerInvoice]') AND name = N'LerrynImportExportInvoiceSourceIndex')
  CREATE NONCLUSTERED INDEX [LerrynImportExportInvoiceSourceIndex] ON [dbo].[CustomerInvoice] 
  (
	[POCode] ASC,
	[SourceCode] ASC,
	[StoreMerchantID_DEV000221] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO

/* start of code added TJS 14/02/12 */
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[CustomerInvoiceDetail]') AND name = N'LerrynImportExportCustomerInvoiceDetailSourcePurchaseIDIndex')
  CREATE NONCLUSTERED INDEX [LerrynImportExportCustomerInvoiceDetailSourcePurchaseIDIndex] ON [dbo].[CustomerInvoiceDetail] 
  (
	[SourcePurchaseID_DEV000221] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO
/* end of code added TJS 14/02/12 */

/* start of code added TJS 11/01/14 */
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[CustomerInvoiceDetail]') AND name = N'LerrynImportExportCustomerInvoiceDetailSourceInvoiceCodeIndex')
  CREATE NONCLUSTERED INDEX [LerrynImportExportCustomerInvoiceDetailSourceInvoiceCodeIndex] ON [dbo].[CustomerInvoiceDetail] 
  (
	[SourceInvoiceCode] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO
/* end of code added TJS 11/01/14 */


IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[CustomerSalesOrder]') AND name = N'LerrynImportExportSalesOrderSourceIndex')
  CREATE NONCLUSTERED INDEX [LerrynImportExportSalesOrderSourceIndex] ON [dbo].[CustomerSalesOrder] 
  (
	[POCode] ASC,
	[SourceCode] ASC,
	[MerchantOrderID_DEV000221] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO

/* start of code added TJS 14/02/12 */
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[ShipmentDetail]') AND name = N'LerrynImportExportShipmentDetailItemIndex')
  CREATE NONCLUSTERED INDEX [LerrynImportExportShipmentDetailItemIndex] ON [dbo].[ShipmentDetail] 
  (
	[ItemCode] ASC,
	[SourceDocument] ASC,
	[LineNum] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO
/* end of code added TJS 14/02/12 */

/* start of code added TJS 30/10/13 */
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportMagentoAttributes_DEV000221]') AND name = N'LerrynImportExportMagentoAttributeInstanceIndex_DEV000221')
  CREATE NONCLUSTERED INDEX [LerrynImportExportMagentoAttributeInstanceIndex_DEV000221] ON [dbo].[LerrynImportExportMagentoAttributes_DEV000221] 
  (
	[InstanceID_DEV000221] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO

IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportMagentoAttributeValues_DEV000221]') AND name = N'LerrynImportExportMagentoAttributeValuesInstanceIndex_DEV000221')
  CREATE NONCLUSTERED INDEX [LerrynImportExportMagentoAttributeValuesInstanceIndex_DEV000221] ON [dbo].[LerrynImportExportMagentoAttributeValues_DEV000221] 
  (
	[InstanceID_DEV000221] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO
/* end of code added TJS 30/10/13 */

/* start of code added TJS 15/11/13 */
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[SystemPostalCode]') AND name = N'SystemPostalCodeStateCountryIndex_DEV000221')
  CREATE NONCLUSTERED INDEX [SystemPostalCodeStateCountryIndex_DEV000221] ON [dbo].[SystemPostalCode] 
  (
  	[State] ASC,
	[CountryCode] ASC
  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  GO
/* end of code added TJS 15/11/13 */

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportServiceAction_DEV000221 WHERE ActionCode_DEV000221 = 'LRYNIMPEXP') 
  INSERT INTO [LerrynImportExportServiceAction_DEV000221]([ActionCode_DEV000221], [StatusActionRequired_DEV000221], 
    [InventoryActionRequired_DEV000221], [ConfigUpdateRequired_DEV000221], [UserCreated], [DateCreated], [UserModified], 
    [DateModified]) VALUES ('LRYNIMPEXP', 0, 0, 0, 'admin', getdate(), 'admin', getdate());

/* This view may exist on some systems and is not needed */
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InventoryMatrixItemsForPublishingOnShopCom_DEV000221]') AND OBJECTPROPERTY(id, N'IsView') = 1)
DROP VIEW [dbo].[InventoryMatrixItemsForPublishingOnShopCom_DEV000221]
GO
DELETE FROM dbo.DataDictionaryColumnLanguage WHERE TableName = 'InventoryMatrixItemsForPublishingOnShopCom_DEV000221'
DELETE FROM dbo.DataDictionaryColumn WHERE TableName = 'InventoryMatrixItemsForPublishingOnShopCom_DEV000221'
DELETE FROM dbo.DataDictionaryTable WHERE TableName = 'InventoryMatrixItemsForPublishingOnShopCom_DEV000221'


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportDeliveryMethods_DEV000221_Old]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	INSERT INTO [dbo].[LerrynImportExportDeliveryMethods_DEV000221]
	  ([SourceCode_DEV000221], [SourceDeliveryMethod_DEV000221] , [SourceDeliveryClass_DEV000221],
	  [SourceDeliveryMethodCode_DEV000221], [SourceDeliveryClassCode_DEV000221], [ShippingMethodGroup_DEV000221],
	  [ShippingMethodCode_DEV000221], [UserCreated], [DateCreated], [UserModified], [DateModified])
	SELECT [DMOld].[SourceCode_DEV000221], [DMOld].[SourceDeliveryMethod_DEV000221], ISNULL([DMOld].[SourceDeliveryClass_DEV000221], ''), 
	  [DMOld].[SourceDeliveryMethodCode_DEV000221], [DMOld].[SourceDeliveryClassCode_DEV000221], [DMOld].[ShippingMethodGroup_DEV000221], 
	  [DMOld].[ShippingMethodCode_DEV000221], [DMOld].[UserCreated], [DMOld].[DateCreated], [DMOld].[UserModified], [DMOld].[DateModified]
	FROM [dbo].[LerrynImportExportDeliveryMethods_DEV000221_Old] [DMOld] LEFT JOIN [dbo].[LerrynImportExportDeliveryMethods_DEV000221] [DMNew]
	ON [DMOld].[SourceCode_DEV000221] = [DMNew].[SourceCode_DEV000221] AND 
	   [DMOld].[SourceDeliveryMethod_DEV000221] = [DMNew].[SourceDeliveryMethod_DEV000221] AND
	   [DMOld].[SourceDeliveryClass_DEV000221] = [DMNew].[SourceDeliveryClass_DEV000221]
	WHERE [DMNew].[Counter] IS NULL;

	DROP TABLE [dbo].[LerrynImportExportDeliveryMethods_DEV000221_Old];
END
GO

UPDATE dbo.LerrynImportExportDeliveryMethods_DEV000221 SET SourceDeliveryMethodCode_DEV000221 = 'USPS', 
  SourceDeliveryClass_DEV000221 = 'Priority Mail'
WHERE SourceCode_DEV000221 = 'ChanAdvOrder' AND SourceDeliveryMethod_DEV000221 = 'US Postal Service' AND
  (SourceDeliveryMethodCode_DEV000221 = 'US Postal Service' OR SourceDeliveryMethodCode_DEV000221 = 'USPS') AND 
  SourceDeliveryClass_DEV000221 = 'PRIORITY' AND SourceDeliveryClassCode_DEV000221 = 'PRIORITY';
  
UPDATE dbo.LerrynImportExportDeliveryMethods_DEV000221 SET SourceDeliveryMethodCode_DEV000221 = 'USPS', 
  SourceDeliveryClass_DEV000221 = 'First-Class Mail'
WHERE SourceCode_DEV000221 = 'ChanAdvOrder' AND SourceDeliveryMethod_DEV000221 = 'US Postal Service' AND
  (SourceDeliveryMethodCode_DEV000221 = 'US Postal Service' OR SourceDeliveryMethodCode_DEV000221 = 'USPS') AND 
  SourceDeliveryClass_DEV000221 = 'FIRSTCLASS' AND SourceDeliveryClassCode_DEV000221 = 'FIRSTCLASS';
  
UPDATE dbo.LerrynImportExportDeliveryMethods_DEV000221 SET SourceDeliveryClass_DEV000221 = 'Ground'
WHERE SourceCode_DEV000221 = 'ChanAdvOrder' AND SourceDeliveryMethod_DEV000221 = 'UPS' AND
  SourceDeliveryMethodCode_DEV000221 = 'UPS' AND SourceDeliveryClass_DEV000221 = 'Ground' AND 
  SourceDeliveryClassCode_DEV000221 = 'Ground';
  

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportInventoryActionStatus_DEV000221_Old]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	INSERT INTO [dbo].[LerrynImportExportInventoryActionStatus_DEV000221]
	  ([ItemCode_DEV000221], [SourceCode_DEV000221], [StoreMerchantID_DEV000221], [TableTagID_DEV000221], [InventoryStatus_DEV000221],
	[ActionTimestamp_DEV000221], [ActionComplete_DEV000221], [ActionXMLFile_DEV000221], [XMLToSend_DEV000221], [XMLMessageType_DEV000221],
	[SendInMessageID_DEV000221], [SentInFileID_DEV000221], [MessageAcknowledged_DEV000221], [ErrorReported_DEV000221],
	[UserCreated], [DateCreated], [UserModified], [DateModified])
	SELECT [ItemCode_DEV000221], [SourceCode_DEV000221], [StoreMerchantID_DEV000221], [TableTagID_DEV000221], [InventoryStatus_DEV000221],
	[ActionTimestamp_DEV000221], [ActionComplete_DEV000221], [ActionXMLFile_DEV000221], [XMLToSend_DEV000221], [XMLMessageType_DEV000221],
	[SendInMessageID_DEV000221], [SentInFileID_DEV000221], [MessageAcknowledged_DEV000221], [ErrorReported_DEV000221],
	[UserCreated], [DateCreated], [UserModified], [DateModified] FROM [dbo].[LerrynImportExportInventoryActionStatus_DEV000221_Old];

	DROP TABLE [dbo].[LerrynImportExportInventoryActionStatus_DEV000221_Old];
END
GO

/* Start of code added TJS 19/03/12 */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportConfig_DEV000221_Old]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
    INSERT INTO [dbo].[LerrynImportExportConfig_DEV000221] ([SourceCode_DEV000221], [SourceName_DEV000221], [SourcePassword_DEV000221], 
        [SourcePasswordIV_DEV000221], [SourcePasswordSalt_DEV000221], [EnableSourcePassword_DEV000221], [InputHandler_DEV000221], 
        [InputMode_DEV000221], [ConfigSettings_DEV000221], [HasSourceIDs_DEV000221], [NotIneCPlus_DEV000221], [ShowIfActivated_DEV000221], 
        [LastOrderStatusUpdate_DEV000221], [UserCreated], [DateCreated], [UserModified], [DateModified])
    SELECT [CFOld].[SourceCode_DEV000221], [CFOld].[SourceName_DEV000221], [CFOld].[SourcePassword_DEV000221], [CFOld].[SourcePasswordIV_DEV000221], 
        [CFOld].[SourcePasswordSalt_DEV000221], [CFOld].[EnableSourcePassword_DEV000221], [CFOld].[InputHandler_DEV000221], 
        [CFOld].[InputMode_DEV000221], [CFOld].[ConfigSettings_DEV000221], Null AS [HasSourceIDs_DEV000221], Null AS [NotIneCPlus_DEV000221], 
        Null AS [ShowIfActivated_DEV000221], Null AS[LastOrderStatusUpdate_DEV000221], [CFOld].[UserCreated], [CFOld].[DateCreated], 
        [CFOld].[UserModified], [CFOld].[DateModified]
    FROM [dbo].[LerrynImportExportConfig_DEV000221_Old] [CFOld] LEFT JOIN [dbo].[LerrynImportExportConfig_DEV000221] [CFNew]
	ON [CFOld].[SourceCode_DEV000221] = [CFNew].[SourceCode_DEV000221] WHERE [CFNew].[Counter] IS NULL;

    DROP TABLE [dbo].[LerrynImportExportConfig_DEV000221_Old];
END
GO
/* End of code added TJS 19/03/12 */

/* Start of code added TJS 30/10/13 */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportCategoryMapping_DEV000221_Old]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
    INSERT INTO [dbo].[LerrynImportExportCategoryMapping_DEV000221] ([SourceCode_DEV000221], [InstanceAccountID_DEV000221], 
        [SourceCategoryID_DEV000221], [SourceParentID_DEV000221], [ISCategoryCode_DEV000221], [UserCreated], [DateCreated], 
        [UserModified], [DateModified])
    SELECT [SourceCode_DEV000221], 'Main' AS [InstanceAccountID_DEV000221], [SourceCategoryID_DEV000221], [SourceParentID_DEV000221], 
        [ISCategoryCode_DEV000221], [UserCreated], [DateCreated], [UserModified], [DateModified]
    FROM [dbo].[LerrynImportExportCategoryMapping_DEV000221_Old]
    
    DROP TABLE [dbo].[LerrynImportExportCategoryMapping_DEV000221_Old];
END
GO
/* End of code added TJS 30/10/13 */


/* Start of code added TJS 13/12/13 */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportSKUAliases_DEV000221_Old]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
    INSERT INTO [dbo].[LerrynImportExportSKUAliases_DEV000221] ([SourceCode_DEV000221], [SourceSKU_DEV000221], [ItemCode_DEV000221],
      [UserCreated], [DateCreated], [UserModified], [DateModified])
    SELECT [SourceCode_DEV000221], [SourceSKU_DEV000221], [ItemCode_DEV000221], [UserCreated], [DateCreated], [UserModified], [DateModified]
    FROM [dbo].[LerrynImportExportSKUAliases_DEV000221_Old]
    
    DROP TABLE [dbo].[LerrynImportExportSKUAliases_DEV000221_Old];
END
GO
/* End of code added TJS 13/12/13 */


IF NOT EXISTS (SELECT * FROM dbo.SystemPaymentType WHERE PaymentTypeCode = 'Ext. System C/Card')
  INSERT INTO SystemPaymentType (PaymentTypeCode, PaymentTypeDescription, PaymentMethodCode, DefaultBankAccountCode, 
    CreditCardGateway, CreditCardGatewayAssemblyName, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  SELECT 'Ext. System C/Card' AS PaymentTypeCode, 'External System Card Payment' AS PaymentTypeDescription, 
    'Cash/Other' AS PaymentMethodCode, DefaultBankAccountCode, Null AS CreditCardGateway, Null AS CreditCardGatewayAssemblyName, 
    1 AS IsActive, 'admin' AS UserCreated, GETDATE() AS DateCreated, 'admin' AS UserModified, GETDATE() AS DateModified 
    FROM dbo.SystemPaymentType WHERE PaymentTypeCode = 'Cash/Other';


DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)

SELECT @DefaultBusType = SystemMessageManager.MessageText FROM SystemCompanyInformation INNER JOIN SystemMessageManager 
ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'GenericXMLImport') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('GenericXMLImport', 'Generic XML Web Import', Null, Null, Null, 1, 'HTTPPost', 'GenericXMLImport.ashx', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>No</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>DEFAULT</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath><EnableLogFile>Yes</EnableLogFile><LogFilePath>C:\eShopCONNECT\LogFiles</LogFilePath></General></eShopCONNECTConfig>', 
    0, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 13/09/13 */
GO

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 0 WHERE SourceCode_DEV000221 = 'GenericXMLImport' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO

DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'ShopComOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('ShopComOrder', 'Shop.com Connector', Null, Null, Null, 0, 'HTTPPost', 'ShopComOrder.ashx', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>Yes</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><ShopDotCom><CatalogID></CatalogID><StatusPostURL>https://admin-amos.shop.com/get_order_status%21251.shtml</StatusPostURL><FTPUploadServerURL></FTPUploadServerURL><FTPUploadUsername></FTPUploadUsername><FTPUploadPassword></FTPUploadPassword><FTPUploadPath>C:\eShopCONNECT\FTPUpload</FTPUploadPath><FTPUploadArchivePath>C:\eShopCONNECT\FTPUpload\FilesUploaded</FTPUploadArchivePath><SourceItemIDField>IT_SOURCECODE</SourceItemIDField><ISItemIDField>ItemName</ISItemIDField><Currency>' + @Currency + '</Currency><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><DefaultUpliftPercent>0</DefaultUpliftPercent><XMLDateFormat>MM/DD/YYYY</XMLDateFormat><AccountDisabled>No</AccountDisabled></ShopDotCom></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 13/09/13 */
GO

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 1 WHERE SourceCode_DEV000221 = 'ShopComOrder' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO


DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)

SELECT @DefaultBusType = SystemMessageManager.MessageText FROM SystemCompanyInformation INNER JOIN SystemMessageManager 
ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'AmazonOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('AmazonOrder', 'Amazon Connector', Null, Null, Null, 0, 'SOAP Poll', 'Amazon eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><AllowBlankPostalcode>Yes</AllowBlankPostalcode><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><Amazon><AmazonSite></AmazonSite><OwnAccessKeyID></OwnAccessKeyID><OwnSecretAccessKey></OwnSecretAccessKey><MerchantToken></MerchantToken><MerchantName></MerchantName><MWSMerchantID></MWSMerchantID><MWSMarketplaceID></MWSMarketplaceID><AmazonManualProcessingPath>C:\eShopCONNECT\FilesForManualAction</AmazonManualProcessingPath><AmazonImportProcessedPath>C:\eShopCONNECT\FilesProcessedSuccessfully</AmazonImportProcessedPath><AmazonImportErrorPath>C:\eShopCONNECT\FilesWithErrors</AmazonImportErrorPath><ISItemIDField>ItemName</ISItemIDField><PaymentType></PaymentType><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><DefaultUpliftPercent>0</DefaultUpliftPercent><EnableSKUAliasLookup>No</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></Amazon></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /** TJS 16/06/12 TJS 22/04/13 TJS 13/09/13 **/
GO

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 1 WHERE SourceCode_DEV000221 = 'AmazonOrder' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO


DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)

SELECT @DefaultBusType = SystemMessageManager.MessageText FROM SystemCompanyInformation INNER JOIN SystemMessageManager 
ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'FileImport') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('FileImport', 'XML File Import', Null, Null, Null, 1, 'Directory Poll', 'File Import eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>No</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><PollGenericImportPath>Yes</PollGenericImportPath><GenericImportPath>C:\eShopCONNECT\FilesForProcessing</GenericImportPath><GenericImportErrorPath>C:\eShopCONNECT\FilesWithErrors</GenericImportErrorPath><GenericImportProcessedPath>C:\eShopCONNECT\FilesProcessedSuccessfully</GenericImportProcessedPath></General></eShopCONNECTConfig>', 
    0, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 13/09/13 */
GO

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 0 WHERE SourceCode_DEV000221 = 'FileImport' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO


DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'VolusionOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('VolusionOrder', 'Volusion Connector', Null, Null, Null, 0, 'Web Poll', 'Volusion eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><Volusion><SiteID>Main</SiteID><OrderPollURL></OrderPollURL><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><Currency>' + @Currency + '</Currency><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><DefaultShippingMethodID></DefaultShippingMethodID><EnableSKUAliasLookup>No</EnableSKUAliasLookup><EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><AccountDisabled>No</AccountDisabled></Volusion></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 13/09/13 TJS 11/01/14 TJS 15/01/14 TJS 11/02/14 */
GO


UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 1 WHERE SourceCode_DEV000221 = 'VolusionOrder' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO


DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'ChanAdvOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('ChanAdvOrder', 'Channel Advisor Connector', Null, Null, Null, 0, 'Web Poll', 'Channel Advisor eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><AllowBlankPostalcode>Yes</AllowBlankPostalcode><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><ChannelAdvisor><OwnDeveloperKey></OwnDeveloperKey><OwnDeveloperPassword></OwnDeveloperPassword><AccountName></AccountName><AccountID></AccountID><ISItemIDField>ItemName</ISItemIDField><Currency>' + @Currency + '</Currency><PaymentType></PaymentType><EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><ActionIfNoPayment>Ignore</ActionIfNoPayment><EnableSKUAliasLookup>No</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></ChannelAdvisor></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 13/09/13 */
GO


UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 1 WHERE SourceCode_DEV000221 = 'ChanAdvOrder' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO


/****** start of code added TJS 19/08/10    ******/
DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'MagentoOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('MagentoOrder', 'Magento Connector', Null, Null, Null, 0, 'Web Poll', 'Magento eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><Magento><InstanceID>Main</InstanceID><APIURL></APIURL><V2SoapAPIWSICompliant>No</V2SoapAPIWSICompliant><APIUser></APIUser><APIPwd></APIPwd><MagentoVersion></MagentoVersion><LerrynAPIVersion>0</LerrynAPIVersion><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><APISupportsPartialShipments>No</APISupportsPartialShipments><CardAuthAndCaptureWithOrder>No</CardAuthAndCaptureWithOrder><ISItemIDField>ItemName</ISItemIDField><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><EnableSKUAliasLookup>No</EnableSKUAliasLookup><ProductListBlockSize>10000</ProductListBlockSize><InhibitInventoryUpdates>No</InhibitInventoryUpdates><CreateCustomerForGuestCheckout>No</CreateCustomerForGuestCheckout><IncludeChildItemsOnOrder>No</IncludeChildItemsOnOrder><UpdateMagentoSpecialPricing>No</UpdateMagentoSpecialPricing><AccountDisabled>No</AccountDisabled></Magento></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /** TJS 10/05/13 TJS 13/09/13 TJS 02/10/13 TJS 05/10/13 TJS 30/10/13 TJS 01/05/14 **/


UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 1 WHERE SourceCode_DEV000221 = 'MagentoOrder' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO


DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'ASPStoreFrontOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('ASPStoreFrontOrder', 'ASPDotNetStoreFront Connector', Null, Null, Null, 0, 'Web Poll', 'ASPDotNetStoreFront eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><AllowBlankPostalcode>Yes</AllowBlankPostalcode><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><ASPStoreFront><SiteID>Main</SiteID><UseWSE3Authentication>No</UseWSE3Authentication><APIURL></APIURL><APIUser></APIUser><APIPwd></APIPwd><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><Currency>' + @Currency + '</Currency><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><EnableSKUAliasLookup>No</EnableSKUAliasLookup><ExtensionDataField1Mapping></ExtensionDataField1Mapping><ExtensionDataField2Mapping></ExtensionDataField2Mapping><ExtensionDataField3Mapping></ExtensionDataField3Mapping><ExtensionDataField4Mapping></ExtensionDataField4Mapping><ExtensionDataField5Mapping></ExtensionDataField5Mapping><AccountDisabled>No</AccountDisabled></ASPStoreFront></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 13/09/13 */
GO

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 1 WHERE SourceCode_DEV000221 = 'ASPStoreFrontOrder' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO


/****** start of code added TJS 29/09/11    ******/
DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'eBayOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('eBayOrder', 'eBay Connector', Null, Null, Null, 0, 'Web Poll', 'eBay eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><AllowBlankPostalcode>Yes</AllowBlankPostalcode><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><eBay><SiteID>Main</SiteID><Country></Country><AuthToken></AuthToken><TokenExpires></TokenExpires><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><PaymentType></PaymentType><EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><ActionIfNoPayment>Ignore</ActionIfNoPayment><EnableSKUAliasLookup>No</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></eBay></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /****** TJS 29/07/13 TJS 13/09/13  ******/
GO
/****** end of code added TJS 29/09/11    ******/


/****** start of code added TJS 12/01/12    ******/
DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'SearsComOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('SearsComOrder', 'Sears.com Connector', Null, Null, Null, 0, 'Web Poll', 'Sears.com eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><AllowBlankPostalcode>Yes</AllowBlankPostalcode><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>Yes</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><SearsDotCom><SiteID>Main</SiteID><APIUser></APIUser><APIPwd></APIPwd><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><PricesAreTaxInclusive>No</PricesAreTaxInclusive><TaxCodeForSourceTax></TaxCodeForSourceTax><Currency></Currency><PaymentType></PaymentType><SearsGeneratesInvoice>Yes</SearsGeneratesInvoice><AccountDisabled>No</AccountDisabled></SearsDotCom></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 13/09/13 */
GO
/****** end of code added TJS 12/01/12    ******/




DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)

SELECT @DefaultBusType = SystemMessageManager.MessageText FROM SystemCompanyInformation INNER JOIN SystemMessageManager 
ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'ProspectLeadImport') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
	VALUES ('ProspectLeadImport', 'Prospect/Lead Import', Null, Null, Null, 1, 'HTTPPost', 'Prospect/Lead eShopCONNECTOR', 
	'<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><RequireSourceCustomerID>Yes</RequireSourceCustomerID></General></eShopCONNECTConfig>', 
    0, 'admin', GETDATE(), 'admin', GETDATE());
ELSE
	UPDATE dbo.LerrynImportExportConfig_DEV000221 SET EnableSourcePassword_DEV000221 = 1, UserModified = 'admin', DateModified = getdate() WHERE SourceCode_DEV000221 = 'ProspectLeadImport'

GO

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET HasSourceIDs_DEV000221 = 0 WHERE SourceCode_DEV000221 = 'ProspectLeadImport' AND HasSourceIDs_DEV000221 IS Null; /** TJS 18/03/11 **/
GO

/****** start of code added TJS 01/07/12    ******/
DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = 'AmazonOrder_FBA') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('AmazonOrder_FBA', 'Amazon FBA Connector (Beta)', Null, Null, Null, 0, 'SOAP Poll', 'Amazon FBA eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><UseShipToClassTemplate>Yes</UseShipToClassTemplate><DefaultShippingMethodGroup></DefaultShippingMethodGroup><DefaultShippingMethod></DefaultShippingMethod><DefaultWarehouse></DefaultWarehouse><DueDateDaysInFuture>1</DueDateDaysInFuture></General><AmazonFBA><AmazonSite></AmazonSite><MerchantToken></MerchantToken><AccountDisabled>No</AccountDisabled></AmazonFBA></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE()); /* TJS 20/11/13 */
GO
/****** end of code added TJS 01/07/12    ******/


/****** start of code added TJS 20/11/13    ******/
DECLARE @DefaultBusClass nvarchar(200)
DECLARE @DefaultBusType nvarchar(20)
DECLARE @Currency nvarchar(30)

SELECT @DefaultBusType = SystemMessageManager.MessageText, @Currency = CurrencyCode FROM SystemCompanyInformation 
INNER JOIN SystemMessageManager ON SystemCompanyInformation.CompanyLanguage = SystemMessageManager.LanguageCode
WHERE SystemMessageManager.MessageType = 'Label' AND SystemMessageManager.MessageCode = 'LBL0023';

IF @DefaultBusType IS NULL
	SET @DefaultBusType = 'Consumer';

SELECT @DefaultBusClass = CustomerClassTemplateDetail.ClassDescription FROM SystemCountry INNER JOIN SystemCompanyInformation 
ON SystemCountry.CountryCode = SystemCompanyInformation.Country INNER JOIN CustomerClassTemplateDetail 
ON SystemCountry.DefaultRetailCustomerBillToClassTemplate = CustomerClassTemplateDetail.ClassCode

IF @DefaultBusClass IS NULL
	SET @DefaultBusClass = 'Default Consumer Customer Class Template';

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportConfig_DEV000221 WHERE SourceCode_DEV000221 = '3DCartOrder') 
  INSERT INTO dbo.LerrynImportExportConfig_DEV000221 (SourceCode_DEV000221, SourceName_DEV000221, SourcePassword_DEV000221, 
    SourcePasswordIV_DEV000221, SourcePasswordSalt_DEV000221, EnableSourcePassword_DEV000221, InputMode_DEV000221, 
    InputHandler_DEV000221, ConfigSettings_DEV000221, HasSourceIDs_DEV000221, UserCreated, DateCreated, UserModified, DateModified) 
    VALUES ('3DCartOrder', '3DCart Connector (Beta)', Null, Null, Null, 0, 'Web Poll', '3DCart eShopCONNECTOR', 
    '<eShopCONNECTConfig><General><ErrorNotificationEmailAddress></ErrorNotificationEmailAddress><SendCodeErrorEmailsToLerryn>Yes</SendCodeErrorEmailsToLerryn><SendSourceErrorEmailsToLerryn>No</SendSourceErrorEmailsToLerryn><CustomerBusinessType>' + @DefaultBusType + '</CustomerBusinessType><CustomerBusinessClass>' + @DefaultBusClass + '</CustomerBusinessClass><CreateCustomerAsCompany>Yes</CreateCustomerAsCompany><EnableDeliveryMethodTranslation>Yes</EnableDeliveryMethodTranslation><DefaultShippingMethodGroup>WEBSHIP</DefaultShippingMethodGroup><DefaultShippingMethod>Standard Delivery Charge</DefaultShippingMethod><DefaultWarehouse>MAIN</DefaultWarehouse><DefaultPaymentTermGroup>Default</DefaultPaymentTermGroup><CreditCardPaymentTermCode>Credit Card</CreditCardPaymentTermCode><DueDateDaysInFuture>1</DueDateDaysInFuture><AuthoriseCreditCardOnImport>No</AuthoriseCreditCardOnImport><ExternalSystemCardPaymentCode>Ext. System C/Card</ExternalSystemCardPaymentCode><EnableCoupons>No</EnableCoupons><RequireSourceCustomerID>Yes</RequireSourceCustomerID><SetDisableFreightCalculation>Yes</SetDisableFreightCalculation><IgnoreVoidedOrdersAndInvoices>Yes</IgnoreVoidedOrdersAndInvoices><AcceptSourceSalesTaxCalculation>No</AcceptSourceSalesTaxCalculation><ImportMissingItemsAsNonStock>No</ImportMissingItemsAsNonStock><AllocateAndReserveStock>Yes</AllocateAndReserveStock><XMLImportFileSavePath></XMLImportFileSavePath></General><ThreeDCart><StoreID>Main</StoreID><StoreURL></StoreURL><UserKey></UserKey><OrderPollIntervalMinutes>15</OrderPollIntervalMinutes><ISItemIDField>ItemName</ISItemIDField><Currency></Currency><EnablePaymentTypeTranslation>No</EnablePaymentTypeTranslation><AllowShippingLastNameBlank>Yes</AllowShippingLastNameBlank><EnableSKUAliasLookup>No</EnableSKUAliasLookup><AccountDisabled>No</AccountDisabled></ThreeDCart></eShopCONNECTConfig>', 
    1, 'admin', GETDATE(), 'admin', GETDATE());
GO
/****** end of code added TJS 20/11/13    ******/


IF NOT EXISTS (SELECT * FROM dbo.SystemStartingNumber WHERE TableTransaction = 'LerrynImpExp' and TableName = 'LerrynImportExportConfig_DEV000221' and ColumnName = 'SourceCode_DEV000221') 
INSERT INTO dbo.SystemStartingNumber (TableTransaction, TableName, ColumnName, Prefix, Number, NumberWidth, IsEnabled, 
  IsMasterFile, TransactionDescription, ParentEntity, IsPostingEntity, UserCreated, DateCreated, UserModified, 
  DateModified) VALUES ('LerrynImpExp', 'LerrynImportExportConfig_DEV000221', 'SourceCode_DEV000221', 'LRYN', 0, 6, 1, 1, 
  'eShopConnect Config', 'eCommerce', 0, 'admin', GETDATE(), 'admin', GETDATE());


IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'GenericXMLImport')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('GenericXMLImport', 'Generic XML Web Import', 1, 'admin', GETDATE(), 'admin', GETDATE());


IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'ShopComOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('ShopComOrder', 'Shop.com website', 1, 'admin', GETDATE(), 'admin', GETDATE());

IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'AmazonOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('AmazonOrder', 'Amazon website', 1, 'admin', GETDATE(), 'admin', GETDATE());

IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'VolusionOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('VolusionOrder', 'Volusion website', 1, 'admin', GETDATE(), 'admin', GETDATE());

IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'ChanAdvOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('ChanAdvOrder', 'Channel Advisor website', 1, 'admin', GETDATE(), 'admin', GETDATE());

IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'MagentoOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('MagentoOrder', 'Magento website', 1, 'admin', GETDATE(), 'admin', GETDATE());

IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'ASPStoreFrontOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('ASPStoreFrontOrder', 'ASPDotNetStoreFront website', 1, 'admin', GETDATE(), 'admin', GETDATE());

IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'eBayOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('eBayOrder', 'eBay website', 1, 'admin', GETDATE(), 'admin', GETDATE());

/****** start of code added TJS 12/01/12    ******/
IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'SearsComOrder')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('SearsComOrder', 'Sears.com website', 1, 'admin', GETDATE(), 'admin', GETDATE());
/****** end of code added TJS 12/01/12    ******/

IF NOT EXISTS (SELECT * FROM dbo.SystemSource WHERE SourceCode = 'ProspectLeadImport')
INSERT INTO dbo.SystemSource (SourceCode, SourceDescription, IsActive, UserCreated, DateCreated, UserModified, DateModified) 
  VALUES ('ProspectLeadImport', 'Prospect/Lead Import', 1, 'admin', GETDATE(), 'admin', GETDATE());

IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportDeliveryMethods_DEV000221 WHERE SourceCode_DEV000221 = 'ShopComOrder' AND SourceDeliveryMethod_DEV000221 = 'Standard' AND SourceDeliveryClass_DEV000221 = '') 
INSERT INTO dbo.LerrynImportExportDeliveryMethods_DEV000221 (SourceCode_DEV000221, SourceDeliveryMethod_DEV000221, 
  SourceDeliveryClass_DEV000221, ShippingMethodGroup_DEV000221, ShippingMethodCode_DEV000221, UserCreated, DateCreated, 
  UserModified, DateModified) VALUES ('ShopComOrder', 'Standard', '', 'DEFAULT', 'Standard Delivery Charge', 'admin', 
  GETDATE(), 'admin', GETDATE());


IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportDeliveryMethods_DEV000221 WHERE SourceCode_DEV000221 = 'ShopComOrder' AND SourceDeliveryMethod_DEV000221 = 'Next Day' AND SourceDeliveryClass_DEV000221 = '') 
INSERT INTO dbo.LerrynImportExportDeliveryMethods_DEV000221 (SourceCode_DEV000221, SourceDeliveryMethod_DEV000221, 
  SourceDeliveryClass_DEV000221, ShippingMethodGroup_DEV000221, ShippingMethodCode_DEV000221, UserCreated, DateCreated, 
  UserModified, DateModified) VALUES ('ShopComOrder', 'Next Day', '', 'DEFAULT', 'Next Day', 'admin', 
  GETDATE(), 'admin', GETDATE());


IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportDeliveryMethods_DEV000221 WHERE SourceCode_DEV000221 = 'ShopComOrder' AND SourceDeliveryMethod_DEV000221 = '2 Day' AND SourceDeliveryClass_DEV000221 = '') 
INSERT INTO dbo.LerrynImportExportDeliveryMethods_DEV000221 (SourceCode_DEV000221, SourceDeliveryMethod_DEV000221, 
  SourceDeliveryClass_DEV000221, ShippingMethodGroup_DEV000221, ShippingMethodCode_DEV000221, UserCreated, DateCreated, 
  UserModified, DateModified) VALUES ('ShopComOrder', '2 Day', '', 'DEFAULT', '2nd Day', 'admin', 
  GETDATE(), 'admin', GETDATE());


IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportDeliveryMethods_DEV000221 WHERE SourceCode_DEV000221 = 'ShopComOrder' AND SourceDeliveryMethod_DEV000221 = '2-3 Day' AND SourceDeliveryClass_DEV000221 = '') 
INSERT INTO dbo.LerrynImportExportDeliveryMethods_DEV000221 (SourceCode_DEV000221, SourceDeliveryMethod_DEV000221, 
  SourceDeliveryClass_DEV000221, ShippingMethodGroup_DEV000221, ShippingMethodCode_DEV000221, UserCreated, DateCreated, 
  UserModified, DateModified) VALUES ('ShopComOrder', '2-3 Day', '', 'DEFAULT', '2nd Day', 'admin', 
  GETDATE(), 'admin', GETDATE());


IF NOT EXISTS (SELECT * FROM dbo.LerrynImportExportDeliveryMethods_DEV000221 WHERE SourceCode_DEV000221 = 'AmazonOrder' AND SourceDeliveryMethod_DEV000221 = 'Standard' AND SourceDeliveryClass_DEV000221 = '') 
INSERT INTO dbo.LerrynImportExportDeliveryMethods_DEV000221 (SourceCode_DEV000221, SourceDeliveryMethod_DEV000221, 
  SourceDeliveryClass_DEV000221, ShippingMethodGroup_DEV000221, ShippingMethodCode_DEV000221, UserCreated, DateCreated, 
  UserModified, DateModified) VALUES ('AmazonOrder', 'Standard', '', 'DEFAULT', 'Standard Delivery Charge', 'admin', 
  GETDATE(), 'admin', GETDATE());


IF NOT EXISTS (SELECT * FROM dbo.SystemPaymentTerm WHERE PaymentTermCode = 'Payment Due on Order')
INSERT INTO dbo.SystemPaymentTerm (PaymentTermCode, DiscountableDays, DiscountPercent, DaysBeforeInterest, InterestPercent, 
  ShowOnB2CWeb, ShowOnB2BWeb, PaymentTermDescription, DueType, DiscountType, DefaultPaymentMethod, DefaultBankAccountCode, 
  StartDate, IsActive, UserCreated, DateCreated, UserModified, DateModified, PaymentType) VALUES ('Payment Due on Order', 
  1, 0, 0, 0, 0, 0, 'Payment Due on Order', 'Net Days - From Invoice Date', 'Percent', 'Check/Cheque', Null, Null, 1, 
  'admin', GETDATE(), 'admin', GETDATE(), 'Cash/Other');


/** start of code added TJS 17/06/12 **/
IF NOT EXISTS (SELECT * FROM dbo.EntityTable WHERE EntityCode = 'eCommerce' AND TableCode = 'LerrynImportExportAmazonSettlement_DEV000221' AND TableDescription = 'Amazon Settlements to reconcile')
INSERT INTO dbo.EntityTable (EntityCode, TableCode, TableDescription, WhereClause, DisplayField, TableTransaction, IsMultiSelect, TabDescription, 
  DefaultSort, UserCreated, DateCreated, UserModified, DateModified) VALUES ('eCommerce', 'LerrynImportExportAmazonSettlement_DEV000221', 
  'Amazon Settlements to reconcile', 'and Reconciled_DEV000221 = 0', 'SettlementCode_DEV000221', Null, 0, Null, 'AmazonSettlementID_DEV000221', 
  'admin', GETDATE(), 'admin', GETDATE());
GO

IF NOT EXISTS (SELECT * FROM dbo.EntityTable WHERE EntityCode = 'eCommerce' AND TableCode = 'LerrynImportExportAmazonSettlement_DEV000221' AND TableDescription = 'Amazon Settlement History')
INSERT INTO dbo.EntityTable (EntityCode, TableCode, TableDescription, WhereClause, DisplayField, TableTransaction, IsMultiSelect, TabDescription, 
  DefaultSort, UserCreated, DateCreated, UserModified, DateModified) VALUES ('eCommerce', 'LerrynImportExportAmazonSettlement_DEV000221', 
  'Amazon Settlement History', 'and Reconciled_DEV000221 = 1', 'SettlementCode_DEV000221', Null, 0, Null, 'AmazonSettlementID_DEV000221', 
  'admin', GETDATE(), 'admin', GETDATE());
GO

IF NOT EXISTS (SELECT * FROM dbo.SystemStartingNumber WHERE TableTransaction = 'AmazonSettlement' and TableName = 'LerrynImportExportAmazonSettlement_DEV000221' and ColumnName = 'SettlementCode_DEV000221') 
INSERT INTO dbo.SystemStartingNumber (TableTransaction, TableName, ColumnName, Prefix, Number, NumberWidth, IsEnabled, 
  IsMasterFile, TransactionDescription, ParentEntity, IsPostingEntity, UserCreated, DateCreated, UserModified, 
  DateModified) VALUES ('AmazonSettlement', 'LerrynImportExportAmazonSettlement_DEV000221', 'SettlementCode_DEV000221', 'SETL', 0, 6, 1, 1, 
  'Amazon Settlement', 'eCommerce', 0, 'admin', GETDATE(), 'admin', GETDATE());
GO
/** end of code added TJS 17/06/12 **/

UPDATE dbo.DataDictionaryColumn SET IsCustomField = 1 WHERE (DeveloperID = 'DEV000221' OR DeveloperID IS NULL) and ColumnName like '%_DEV000221' and IsCustomField = 0;

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'ASIN' WHERE DisplayName = 'ASIN_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'ASPStorefront Product GUID' WHERE DisplayName = 'ASPStorefrontProductGUID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Code Expires' WHERE DisplayName = 'CodeExpires_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Config Settings' WHERE DisplayName = 'ConfigSettings_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Data' WHERE DisplayName = 'Data_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Data IV' WHERE DisplayName = 'DataIV_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Data Salt' WHERE DisplayName = 'DataSalt_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Import Customer ID' WHERE DisplayName = 'ImportCustomerID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Import Source Buyer ID' WHERE DisplayName = 'ImportSourceBuyerID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Import Source ID' WHERE DisplayName = 'ImportSourceID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Import Lead ID' WHERE DisplayName = 'ImportLeadID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Licence Code' WHERE DisplayName = 'LicenceCode_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Magento Product ID' WHERE DisplayName = 'MagentoProductID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Merchant Order ID' WHERE DisplayName = 'MerchantOrderID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Payment Failed Email Sent' WHERE DisplayName = 'PaymentFailedEmailSent_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Product Code' WHERE DisplayName = 'ProductCode_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Publish' WHERE DisplayName = 'Publish_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Source Purchase ID' WHERE DisplayName = 'SourcePurchaseID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Source Feedback Message' WHERE DisplayName = 'SourceFeedbackMessage_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Source Feedback Type' WHERE DisplayName = 'SourceFeedbackType_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Store Merchant ID' WHERE DisplayName = 'StoreMerchantID_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Tax Value Is From Source' WHERE DisplayName = 'TaxValueIsFromSource_DEV000221';

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Source Fulfillment Cost' WHERE DisplayName = 'SourceFulfillmentCost_DEV000221'; /** TJS 17/06/12 **/

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Source Fulfillment Cost Rate' WHERE DisplayName = 'SourceFulfillmentCostRate_DEV000221'; /** TJS 17/06/12 **/

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Source Commission Charge' WHERE DisplayName = 'SourceCommissionCharge_DEV000221'; /** TJS 17/06/12 **/

UPDATE dbo.DataDictionaryColumnLanguage SET DisplayName = 'Source Commission Charge Rate' WHERE DisplayName = 'SourceCommissionChargeRate_DEV000221'; /** TJS 17/06/12 **/

UPDATE dbo.SystemPlugin SET Description = 'Lerryn eShopCONNECT' WHERE AssemblyName = 'Lerryn.Presentation.eShopCONNECT';

UPDATE dbo.SystemCompanyInformation SET CDBID_DEV000221 = CacheDatabaseName WHERE CDBID_DEV000221 IS NULL AND CacheDatabaseName IS NOT NULL;

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET NotIneCPlus_DEV000221 = 0 WHERE NotIneCPlus_DEV000221 IS NULL; /** TJS 18/03/11 **/

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET NotIneCPlus_DEV000221 = 1 WHERE (SourceCode_DEV000221 = 'AmazonOrder' 
  OR SourceCode_DEV000221 = 'ChanAdvOrder' OR SourceCode_DEV000221 = 'eBayOrder' OR SourceCode_DEV000221 = 'ShopComOrder'); /** TJS 18/03/11 TJS 29/09/11 **/

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET ShowIfActivated_DEV000221 = 0 WHERE ShowIfActivated_DEV000221 IS NULL; /** TJS 18/03/11 **/

UPDATE dbo.LerrynImportExportConfig_DEV000221 SET ShowIfActivated_DEV000221 = 1 WHERE (SourceCode_DEV000221 = 'FileImport' 
  OR SourceCode_DEV000221 = 'ProspectLeadImport' OR SourceCode_DEV000221 = 'AmazonOrder' OR 
  SourceCode_DEV000221 = 'ChanAdvOrder' OR SourceCode_DEV000221 = 'eBayOrder' OR SourceCode_DEV000221 = 'ShopComOrder'); /** TJS 18/03/11 TJS 29/09/11 **/

GO

UPDATE dbo.InventoryMagentoDetails_DEV000221 SET QtyPublishingType_DEV000221 = 'None' WHERE QtyPublishingType_DEV000221 Is Null; /** TJS 05/11/11 **/

UPDATE dbo.InventoryMagentoDetails_DEV000221 SET TotalQtyWhenLastPublished_DEV000221 = 0 WHERE TotalQtyWhenLastPublished_DEV000221 Is Null; /** TJS 05/11/11 **/

UPDATE dbo.InventoryMagentoDetails_DEV000221 SET QtyLastPublished_DEV000221 = 0 WHERE QtyLastPublished_DEV000221 Is Null; /** TJS 05/11/11 **/

/** start of code added TJS 30/10/13 **/
UPDATE dbo.InventoryAmazonDetails_DEV000221 SET ProductName_DEV000221 = '<Use standard Item Description>' WHERE ProductName_DEV000221 = '<Use standard Item Name>';

UPDATE dbo.InventoryAmazonDetails_DEV000221 SET ProductDescription_DEV000221 = '<Use standard Item Ext Description>' WHERE ProductDescription_DEV000221 = '<Use standard Item Description>';

UPDATE dbo.InventoryASPStorefrontDetails_DEV000221 SET ProductName_DEV000221 = '<Use standard Item Description>' WHERE ProductName_DEV000221 = '<Use standard Item Name>';

UPDATE dbo.InventoryASPStorefrontDetails_DEV000221 SET ProductDescription_DEV000221 = '<Use standard Item Ext Description>' WHERE ProductDescription_DEV000221 = '<Use standard Item Description>';

UPDATE dbo.InventoryChannelAdvDetails_DEV000221 SET ProductName_DEV000221 = '<Use standard Item Description>' WHERE ProductName_DEV000221 = '<Use standard Item Name>';

UPDATE dbo.InventoryChannelAdvDetails_DEV000221 SET ProductShortDescription_DEV000221 = '<Use standard Item Ext Description>' WHERE ProductShortDescription_DEV000221 = '<Use standard Item Description>';

UPDATE dbo.InventoryMagentoDetails_DEV000221 SET ProductName_DEV000221 = '<Use standard Item Description>' WHERE ProductName_DEV000221 = '<Use standard Item Name>';

UPDATE dbo.InventoryMagentoDetails_DEV000221 SET ProductShortDescription_DEV000221 = '<Use standard Item Ext Description>' WHERE ProductShortDescription_DEV000221 = '<Use standard Item Description>';

UPDATE dbo.InventoryShopComDetails_DEV000221 SET GroupName_DEV000221 = '<Use standard Item Description>' WHERE GroupName_DEV000221 = '<Use standard Item Name>';

UPDATE dbo.InventoryShopComDetails_DEV000221 SET GroupDescription_DEV000221 = '<Use standard Item Ext Description>' WHERE GroupDescription_DEV000221 = '<Use standard Item Description>';

/** end of code added TJS 30/10/13 **/

/** start of code added Mark kee 04/22/15 **/

UPDATE SystemUserRoleMenuFormDescription SET Description = REPLACE(Description, 'eShopCONNECT', 'eShopCONNECTED') where Description = 'eShopCONNECT'

UPDATE SystemPluginAssembly SET FileDescription = REPLACE(FileDescription, 'eShopCONNECT', 'eShopCONNECTED')  where FileDescription = 'Presentation Layer - eShopCONNECT'

Update SystemUserRoleMenuFormSection Set SectionLayout = REPLACE(SectionLayout,'>eShopCONNECT<','>eShopCONNECTED<') where SectionLayout like '%>eShopCONNECT<%'

UPDATE SystemUserRoleMenuFormSectionDescription SET Description = REPLACE(Description, 'eShopCONNECT', 'eShopCONNECTED') where Description like 'eShopCONNECT %'

/** end of code added Mark kee 04/22/15 **/

/** start of code added Mark kee 06/11/15 **/

--Hide Activation Wizard
Update SystemUserRoleMenuForm Set IsVisible = 0 where FormMenuCode  = (select  distinct FormMenuCode from SystemUserRoleMenuFormDescription where Description = 'Activation')
--Hide Solid Commerce
Update SystemUserRoleMenuForm Set IsVisible = 0 where FormMenuCode  = (select  distinct FormMenuCode from SystemUserRoleMenuFormDescription where Description =  'Solid Commerce (Beta)')
/** end of code added Mark kee 06/11/15 **/

/** start of code added Mark kee 06/11/15 **/

--IF (select count(*) from LerrynLicences_DEV000221) > 0
--Update SystemUserRoleMenuForm Set IsVisible = 1 where FormMenuCode in (select Distinct FormMenuCode from SystemUserRoleMenuFormDescription  where Description  like 'eShopCONNECT%'  OR Description = 'Marketplace')
--else 
--Update SystemUserRoleMenuForm Set IsVisible = 0 where FormMenuCode in (select Distinct FormMenuCode from SystemUserRoleMenuFormDescription  where Description  like 'eShopCONNECT%'  OR Description = 'Marketplace')
--GO
/** end of code added Mark kee 06/22/15 **/

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'IsActive' AND [object_id] = OBJECT_ID(N'LerrynImportExportConfig_DEV000221'))
BEGIN
    Alter TAble LerrynImportExportConfig_DEV000221 ADD IsActive bit NULL DEFAULT 0 WITH VALUES
END
GO

/******************************************************************************************/
/* CUSTOM SCRIPT ADDED HERE - END */
/******************************************************************************************/


sp_refreshview CustomerInvoiceDetailView
go

refreshdatadictionarycolumn 'CustomerInvoiceDetailView','admin'
go

sp_refreshview Customersalesorderdetailview
go

refreshdatadictionarycolumn Customersalesorderdetailview, 'admin'
go

sp_refreshview TransactionitemtaxdetailView
go

refreshdatadictionarycolumn TransactionitemtaxdetailView, 'admin'
go

sp_refreshview TransactiontaxdetailView
go

refreshdatadictionarycolumn TransactiontaxdetailView, 'admin'
go

refreshdatadictionarycolumn TransactiontaxdetailView, 'admin'
go

refreshdatadictionarycolumn LerrynImportExportConfig_DEV000221, 'admin'
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReadCustomerInvoiceDetailImportExport_DEV000221]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ReadCustomerInvoiceDetailImportExport_DEV000221]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReadCustomerInvoiceDetailImportExport_DEV000221]

	@InvoiceCode [nvarchar](30),

	@SourceInvoiceCode [nvarchar](30),

	@LineNum [int]

AS

SET NOCOUNT ON

SELECT * INTO #CID FROM CustomerInvoiceDetail

WHERE (InvoiceCode = ISNULL(@InvoiceCode, InvoiceCode)) AND 

            (SourceInvoiceCode = ISNULL(@SourceInvoiceCode, SourceInvoiceCode)) AND 

            (LineNum = ISNULL(@lineNum, LineNum))

UPDATE #CID SET SourcePurchaseID_DEV000221 = CustomerSalesOrderDetail.SourcePurchaseID_DEV000221
FROM CustomerSalesOrderDetail
WHERE #CID.SourceInvoiceCode = CustomerSalesOrderDetail.SalesOrderCode
AND #CID.ItemCode = CustomerSalesOrderDetail.ItemCode
AND #CID.SourceLineNum = CustomerSalesOrderDetail.LineNum

SELECT * FROM #CID

DROP TABLE #CID

GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'SiteCode_DEV000221' AND [object_id] = OBJECT_ID(N'InventoryEBayDetails_DEV000221'))
BEGIN
    Alter TAble InventoryEBayDetails_DEV000221 ADD SiteCode_DEV000221 [nvarchar](10) NOT NULL
END
GO
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ProductName_DEV000221' AND [object_id] = OBJECT_ID(N'InventoryEBayDetails_DEV000221'))
BEGIN
    Alter TAble InventoryEBayDetails_DEV000221 ADD ProductName_DEV000221 [nvarchar](10) NOT NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ProductDescription_DEV000221' AND [object_id] = OBJECT_ID(N'InventoryEBayDetails_DEV000221'))
BEGIN
    Alter TAble InventoryEBayDetails_DEV000221 ADD ProductDescription_DEV000221 [nvarchar](10) NOT NULL
END
GO