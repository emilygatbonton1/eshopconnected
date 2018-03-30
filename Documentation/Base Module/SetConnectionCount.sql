BEGIN
    DECLARE @ConfigXML nvarchar(max)
	DECLARE @ConnectionCount int
	DECLARE @SourceCode nvarchar(30)
	DECLARE @SearchText nvarchar(30)
	DECLARE @GroupPosn int
	DECLARE Config_Cursor CURSOR FOR SELECT SourceCode_DEV000221, ConfigSettings_DEV000221 FROM dbo.LerrynImportExportConfig_DEV000221

	OPEN Config_Cursor
	FETCH NEXT FROM ItemAmazon_Cursor INTO @SourceCode, @ConfigXML, @ConnectionCount
	WHILE @@FETCH_STATUS = 0
		BEGIN
			SELECT @SearchText = '<General>'
		    IF @SourceCode = 'AmazonOrder'
			    SELECT @SearchText = '<Amazon>'

		    IF @SourceCode = 'ASPStoreFrontOrder'
			    SELECT @SearchText = '<ASPStoreFront>'

		    IF @SourceCode = 'ChanAdvOrder'
			    SELECT @SearchText = '<ChannelAdvisor>'

		    IF @SourceCode = 'eBayOrder'
			    SELECT @SearchText = '<eBay>'

		    IF @SourceCode = 'MagentoOrder'
			    SELECT @SearchText = '<Magento>'

		    IF @SourceCode = 'ShopComOrder'
			    SELECT @SearchText = '<ShopDotCom>'

		    IF @SourceCode = 'VolusionOrder'
			    SELECT @SearchText = '<Volusion>'

			SELECT @GroupPosn CHARINDEX(@SearchText, @ConfigXML)

			WHILE @GroupPosn > 0