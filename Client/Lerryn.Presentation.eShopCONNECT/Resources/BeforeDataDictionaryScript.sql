/* Start of code added TJS 30/10/13 */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportCategoryMapping_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  NOT EXISTS(select * from sys.columns where Name = N'InstanceAccountID_DEV000221' AND Object_ID = Object_ID(N'[dbo].[LerrynImportExportCategoryMapping_DEV000221]'))
  BEGIN
    EXEC sp_rename 'LerrynImportExportCategoryMapping_DEV000221', 'LerrynImportExportCategoryMapping_DEV000221_Old';

    EXEC sp_rename 'LerrynImportExportCategoryMapping_DEV000221Index', 'LerrynImportExportCategoryMapping_DEV000221Index_Old';

    EXEC sp_rename 'DF_LerrynImportExportCategoryMapping_DEV000221_InstanceAccountID_DEV000221', 'DF_LerrynImportExportCategoryMapping_DEV000221_InstanceAccountID_DEV000221_Old';
    
    EXEC sp_rename 'DF_LerrynImportExportCategoryMapping_DEV000221_ISCategoryCode_DEV000221', 'DF_LerrynImportExportCategoryMapping_DEV000221_ISCategoryCode_DEV000221_Old';

    EXEC sp_rename 'DF_LerrynImportExportCategoryMapping_DEV000221_SourceCategoryID_DEV000221', 'DF_LerrynImportExportCategoryMapping_DEV000221_SourceCategoryID_DEV000221_Old';

    EXEC sp_rename 'DF_LerrynImportExportCategoryMapping_DEV000221_SourceParentID_DEV000221', 'DF_LerrynImportExportCategoryMapping_DEV000221_SourceParentID_DEV000221_Old';
  END
GO
/* End of code added TJS 30/10/13 */


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportDeliveryMethods_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND
  NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].LerrynImportExportDeliveryMethods_DEV000221_Old') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  (NOT EXISTS(select * from sys.columns where Name = N'SourceDeliveryClass_DEV000221' AND Object_ID = Object_ID(N'[dbo].[LerrynImportExportDeliveryMethods_DEV000221]')) 
    OR (SELECT ColumnLength FROM dbo.DataDictionaryColumn WHERE TableName = 'LerrynImportExportDeliveryMethods_DEV000221' AND ColumnName = 'SourceDeliveryMethod_DEV000221') <> 70 
    OR (SELECT ColumnLength FROM dbo.DataDictionaryColumn WHERE TableName = 'LerrynImportExportDeliveryMethods_DEV000221' AND ColumnName = 'SourceDeliveryMethodCode_DEV000221') <> 70)
    BEGIN
	EXEC sp_rename 'LerrynImportExportDeliveryMethods_DEV000221', 'LerrynImportExportDeliveryMethods_DEV000221_Old';

	EXEC sp_rename 'LerrynImportExportDeliveryMethods_DEV000221Index', 'LerrynImportExportDeliveryMethods_DEV000221Index_Old';
	
	UPDATE DataDictionaryColumn SET ColumnLength = 70, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportDeliveryMethods_DEV000221' AND ColumnName = 'SourceDeliveryMethod_DEV000221';

	UPDATE DataDictionaryColumn SET ColumnLength = 70, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportDeliveryMethods_DEV000221' AND ColumnName = 'SourceDeliveryMethodCode_DEV000221';
    END

    /* Start of code added TJS 03/03/14 */
ELSE
    BEGIN
	UPDATE DataDictionaryColumn SET ColumnLength = 70, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportDeliveryMethods_DEV000221' AND ColumnName = 'SourceDeliveryMethod_DEV000221' AND ColumnLength <> 70;

	UPDATE DataDictionaryColumn SET ColumnLength = 70, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportDeliveryMethods_DEV000221' AND ColumnName = 'SourceDeliveryMethodCode_DEV000221' AND ColumnLength <> 70;
    END
    /* End of code added TJS 03/03/14 */
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportInventoryActionStatus_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND
  NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].LerrynImportExportInventoryActionStatus_DEV000221_Old') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  (SELECT ColumnLength FROM dbo.DataDictionaryColumn WHERE TableName = 'LerrynImportExportInventoryActionStatus_DEV000221' AND ColumnName = 'TableTagID_DEV000221') = 20
    BEGIN
	EXEC sp_rename 'LerrynImportExportInventoryActionStatus_DEV000221Index', 'LerrynImportExportInventoryActionStatus_DEV000221Index_Old';

	EXEC sp_rename 'DF_LerrynImportExportInventoryActionStatus_DEV000221_ActionComplete_DEV000221', 'DF_LerrynImportExportInventoryActionStatus_DEV000221_ActionComplete_DEV000221_Old';
	
	EXEC sp_rename 'DF_LerrynImportExportInventoryActionStatus_DEV000221_ErrorReported_DEV000221', 'DF_LerrynImportExportInventoryActionStatus_DEV000221_ErrorReported_DEV000221_Old';
	
	EXEC sp_rename 'DF_LerrynImportExportInventoryActionStatus_DEV000221_MessageAcknowledged_DEV000221', 'DF_LerrynImportExportInventoryActionStatus_DEV000221_MessageAcknowledged_DEV000221_Old';
	
	EXEC sp_rename 'DF_LerrynImportExportInventoryActionStatus_DEV000221_XMLMessageType_DEV000221', 'DF_LerrynImportExportInventoryActionStatus_DEV000221_XMLMessageType_DEV000221_Old';
	
	EXEC sp_rename 'DF_LerrynImportExportInventoryActionStatus_DEV000221_XMLToSend_DEV000221', 'DF_LerrynImportExportInventoryActionStatus_DEV000221_XMLToSend_DEV000221_Old';

	EXEC sp_rename 'LerrynImportExportInventoryActionStatus_DEV000221', 'LerrynImportExportInventoryActionStatus_DEV000221_Old';

	UPDATE dbo.DataDictionaryColumn SET ColumnLength = 25, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportInventoryActionStatus_DEV000221' AND ColumnName = 'TableTagID_DEV000221';
    END

    /* Start of code added TJS 03/03/14 */
ELSE
    UPDATE dbo.DataDictionaryColumn SET ColumnLength = 25, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportInventoryActionStatus_DEV000221' AND ColumnName = 'TableTagID_DEV000221' AND ColumnLength <> 25;
    /* End of code added TJS 03/03/14 */
GO

/* Start of code added TJS 13/12/13 */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportSKUAliases_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND
  NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].LerrynImportExportSKUAliases_DEV000221_Old') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  (SELECT ColumnLength FROM dbo.DataDictionaryColumn WHERE TableName = 'LerrynImportExportSKUAliases_DEV000221' AND ColumnName = 'SourceSKU_DEV000221') <> 100
    BEGIN
	EXEC sp_rename 'LerrynImportExportSKUAliases_DEV000221', 'LerrynImportExportSKUAliases_DEV000221_Old';

	EXEC sp_rename 'LerrynImportExportSKUAliases_DEV000221Index', 'LerrynImportExportSKUAliases_DEV000221Index_Old';
	
	UPDATE DataDictionaryColumn SET ColumnLength = 100, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportSKUAliases_DEV000221' AND ColumnName = 'SourceSKU_DEV000221';
    END

    /* Start of code added TJS 03/03/14 */
ELSE
    UPDATE DataDictionaryColumn SET ColumnLength = 100, UserModified = 'admin', DateModified = getdate() WHERE TableName = 'LerrynImportExportSKUAliases_DEV000221' AND ColumnName = 'SourceSKU_DEV000221' AND ColumnLength <> 100;
    /* End of code added TJS 03/03/14 */
GO
/* End of code added TJS 13/12/13 */


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InventoryASPStorefrontDetails_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  NOT EXISTS(select * from sys.columns where Name = N'ASPStorefrontProductID_DEV000221' AND Object_ID = Object_ID(N'[dbo].[InventoryASPStorefrontDetails_DEV000221]'))
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryASPStorefrontDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryASPStorefrontDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InventoryMagentoDetails_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  NOT EXISTS(select * from sys.columns where Name = N'MagentoProductID_DEV000221' AND Object_ID = Object_ID(N'[dbo].[InventoryMagentoDetails_DEV000221]'))
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryMagentoDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryMagentoDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InventoryMagentoDetails_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  EXISTS(select * from sys.columns where Name = N'SiteID_DEV000221' AND Object_ID = Object_ID(N'[dbo].[InventoryMagentoDetails_DEV000221]'))
  BEGIN
    EXEC sp_rename 'InventoryMagentoDetails_DEV000221.SiteID_DEV000221', 'InstanceID_DEV000221' , 'COLUMN';
    
    UPDATE dbo.DataDictionaryColumn SET ColumnName = 'InstanceID_DEV000221' WHERE TableName = 'InventoryMagentoDetails_DEV000221' 
      AND ColumnName = 'SiteID_DEV000221';
  END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InventoryMagentoDetails_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  EXISTS (select * from sys.columns where Name = N'ManufacturerCode_DEV000221' AND Object_ID = Object_ID(N'[dbo].[InventoryMagentoDetails_DEV000221]'))
  BEGIN
    DECLARE @SQL nvarchar(250)
	
    set @SQL = 'UPDATE InventoryItem SET ManufacturerCode = (SELECT ManufacturerCode_DEV000221 FROM InventoryMagentoDetails_DEV000221 WHERE InventoryItem.ItemCode = InventoryMagentoDetails_DEV000221.ItemCode_DEV000221) WHERE ManufacturerCode IS NULL'
      
    EXECUTE (@SQL)

    ALTER TABLE InventoryMagentoDetails_DEV000221 DROP COLUMN ManufacturerCode_DEV000221;
    
    DELETE FROM dbo.DataDictionaryColumnLanguage WHERE TableName = 'InventoryMagentoDetails_DEV000221' AND ColumnName = 'ManufacturerCode_DEV000221';

    DELETE FROM dbo.DataDictionaryColumn WHERE TableName = 'InventoryMagentoDetails_DEV000221' AND ColumnName = 'ManufacturerCode_DEV000221';
  END
GO
 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InventoryMagentoCategories_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  EXISTS(select * from sys.columns where Name = N'SiteID_DEV000221' AND Object_ID = Object_ID(N'[dbo].[InventoryMagentoCategories_DEV000221]'))
  BEGIN
    EXEC sp_rename 'InventoryMagentoCategories_DEV000221.SiteID_DEV000221', 'InstanceID_DEV000221' , 'COLUMN';
    
    UPDATE dbo.DataDictionaryColumn SET ColumnName = 'InstanceID_DEV000221' WHERE TableName = 'InventoryMagentoCategories_DEV000221' 
      AND ColumnName = 'SiteID_DEV000221';
  END
GO

/* Start of code added TJS 19/03/12 */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[LerrynImportExportConfig_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND
  NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].LerrynImportExportConfig_DEV000221_Old') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  (SELECT t.name FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[LerrynImportExportConfig_DEV000221]') and c.name = 'ConfigSettings_DEV000221') = 'ntext'
BEGIN
	EXEC sp_rename 'LerrynImportExportConfig_DEV000221', 'LerrynImportExportConfig_DEV000221_Old';

	EXEC sp_rename 'LerrynImportExportConfig_DEV000221Index', 'LerrynImportExportConfig_DEV000221Index_Old';

	EXEC sp_rename 'DF_LerrynImportExportConfig_DEV000221_EnableSourcePassword_DEV000221', 'DF_LerrynImportExportConfig_DEV000221_EnableSourcePassword_DEV000221_Old';

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LerrynImportExportConfig_DEV000221_EnableSourcePassword_DEV000221]') AND type = 'D')
	    BEGIN
		ALTER TABLE [dbo].[LerrynImportExportConfig_DEV000221_Old] DROP CONSTRAINT [DF_LerrynImportExportConfig_DEV000221_EnableSourcePassword_DEV000221]
	    END

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LerrynImportExportConfig_DEV000221_HasSourceIDs_DEV000221]') AND type = 'D')
	    BEGIN
		ALTER TABLE [dbo].[LerrynImportExportConfig_DEV000221_Old] DROP CONSTRAINT [DF_LerrynImportExportConfig_DEV000221_HasSourceIDs_DEV000221]
	    END

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LerrynImportExportConfig_DEV000221_NotIneCPlus_DEV000221]') AND type = 'D')
	    BEGIN
		ALTER TABLE [dbo].[LerrynImportExportConfig_DEV000221_Old] DROP CONSTRAINT [DF_LerrynImportExportConfig_DEV000221_NotIneCPlus_DEV000221]
	    END

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LerrynImportExportConfig_DEV000221_ShowIfActivated_DEV000221]') AND type = 'D')
	    BEGIN
		ALTER TABLE [dbo].[LerrynImportExportConfig_DEV000221_Old] DROP CONSTRAINT [DF_LerrynImportExportConfig_DEV000221_ShowIfActivated_DEV000221]
	    END
END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryAmazonDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryAmazonDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryAmazonDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryAmazonTagDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryAmazonTagDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryAmazonTagDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryASPStorefrontDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryASPStorefrontDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryASPStorefrontDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryASPStorefrontTagDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryASPStorefrontTagDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryASPStorefrontTagDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryChannelAdvDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryChannelAdvDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryChannelAdvDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryChannelAdvTagDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryChannelAdvTagDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryChannelAdvTagDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryMagentoDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryMagentoDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryMagentoDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryMagentoTagDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryMagentoTagDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryMagentoTagDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryMagentoDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryMagentoDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryMagentoDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[InventoryMagentoTagDetails_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryMagentoTagDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryMagentoTagDetails_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[LerrynImportExportAmazonFiles_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[LerrynImportExportAmazonFiles_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[LerrynImportExportAmazonFiles_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[LerrynImportExportInventoryActionStatus_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[LerrynImportExportInventoryActionStatus_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[LerrynImportExportInventoryActionStatus_DEV000221]
      END
  END
GO

IF EXISTS (SELECT c.* FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id=t.user_type_id WHERE c.object_id = OBJECT_ID('[dbo].[LerrynImportExportInventoryTagTemplate_DEV000221]') and t.name = 'ntext')
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[LerrynImportExportInventoryTagTemplate_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[LerrynImportExportInventoryTagTemplate_DEV000221]
      END
  END
GO
/* End of code added TJS 19/03/12 */


/* Start of code added TJS 01/07/12 */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InventoryAmazonTagDetails_DEV000221]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) AND 
  NOT EXISTS(select * from sys.columns where Name = N'MerchantID_DEV000221' AND Object_ID = Object_ID(N'[dbo].[InventoryAmazonTagDetails_DEV000221]'))
  BEGIN
    IF (SELECT COUNT(*) FROM [dbo].[InventoryAmazonTagDetails_DEV000221]) = 0
      BEGIN
        DROP TABLE [dbo].[InventoryAmazonTagDetails_DEV000221]
      END
  END
GO
/* End of code added TJS 01/07/12 */


