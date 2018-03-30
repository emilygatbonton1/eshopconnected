using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lerryn.Framework.ImportExport.Shared;

namespace Lerryn.WindowsService.eShopCONNECT.UnitTest
{
    [TestClass]
    public class SendNotification
    {
        [TestMethod]
        public void SendExceptionEmail_ErrorDetailsAsString_SendWithoutXMLSourceNoException()
        {
            var errorNotification = new Lerryn.Facade.ImportExport.ErrorNotification();
            errorNotification.BaseProductName = "eShopCONNECT";
            var importExportDataset = new Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway();
            var importExportConfigFacade = new Lerryn.Facade.ImportExport.ImportExportConfigFacade(ref importExportDataset, ref errorNotification, "AX5", "eShopCONNECT");
            
            importExportConfigFacade.LoadDataSet(new string[][] {new string[] {importExportDataset.LerrynImportExportConfig_DEV000221.TableName, 
                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.Enum.ClearType.Specific);
            General.m_ImportExportConfigFacade = importExportConfigFacade;

            var setts = new Settings();
            var configRows = importExportDataset.LerrynImportExportConfig_DEV000221.Select("SourceCode_Dev000221 = 'AmazonOrder'");
            if (configRows.Length == 0) Assert.Fail("Amazon Connector is not activated.");
            var amazonConfigRow = (Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)configRows[0];
            if (importExportConfigFacade.get_IsConnectorActivated(ConfigConst.AMAZON_SELLER_CENTRAL_CONNECTOR_CODE))
            {
                setts.LoadXMLConfig(ref amazonConfigRow);
            }
            var amazon = (Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)setts.ActiveSources[1];
            var xmlConfig = ((Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)amazon).XMLConfig;
            errorNotification.SendExceptionEmail(xmlConfig, "SendExceptionEmail_ErrorDetailsAsString_SendWithoutXMLSourceNoException", "Testing SendExceptionEmail_ErrorDetailsAsString_SendWithoutXMLSourceNoException", string.Empty);
        }

        [TestMethod]
        public void SendExceptionEmail_ErrorDetailsAsString_SendWithXMLSourceNoException()
        {
            var errorNotification = new Lerryn.Facade.ImportExport.ErrorNotification();
            errorNotification.BaseProductName = "eShopCONNECT";
            var importExportDataset = new Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway();
            var importExportConfigFacade = new Lerryn.Facade.ImportExport.ImportExportConfigFacade(ref importExportDataset, ref errorNotification, "AX5", "eShopCONNECT");
            importExportConfigFacade.LoadDataSet(new string[][] {new string[] {importExportDataset.LerrynImportExportConfig_DEV000221.TableName, 
                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.Enum.ClearType.Specific);
            General.m_ImportExportConfigFacade = importExportConfigFacade;
            var setts = new Settings();
            var configRows = importExportDataset.LerrynImportExportConfig_DEV000221.Select("SourceCode_Dev000221 = 'AmazonOrder'");
            if (configRows.Length == 0) Assert.Fail("Amazon Connector is not activated.");
            var amazonConfigRow = (Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)configRows[0];
            if (importExportConfigFacade.get_IsConnectorActivated(ConfigConst.AMAZON_SELLER_CENTRAL_CONNECTOR_CODE))
            {
                setts.LoadXMLConfig(ref amazonConfigRow);
            }
            var amazon = (Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)setts.ActiveSources[1];
            var xmlConfig = ((Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)amazon).XMLConfig;
            var xmlResponse = "<ErrorResponse xmlns=\"http://mws.amazonservices.com/doc/2009-01-01/\"><Error><Type>Sender</Type><Code>InvalidClientTokenId</Code><Message>The AWS Access Key Id you provided does not exist in our records.</Message><Detail>com.amazonservices.mws.model.Error$Detail@17b6643</Detail></Error><RequestID>b7afc6c3-6f75-4707-bcf4-0475ad23162c</RequestID></ErrorResponse>";
            errorNotification.SendExceptionEmail(xmlConfig, "SendExceptionEmail_ErrorDetailsAsString_SendWithoutXMLSourceNoException", "Testing SendExceptionEmail_ErrorDetailsAsString_SendWithoutXMLSourceNoException", xmlResponse);
        }

        [TestMethod]
        public void SendSrcErrorEmail_SendWithoutXMLSourceNoException()
        {
            var errorNotification = new Lerryn.Facade.ImportExport.ErrorNotification();
            errorNotification.BaseProductName = "eShopCONNECT";
            var importExportDataset = new Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway();
            var importExportConfigFacade = new Lerryn.Facade.ImportExport.ImportExportConfigFacade(ref importExportDataset, ref errorNotification, "AX5", "eShopCONNECT");
            importExportConfigFacade.LoadDataSet(new string[][] {new string[] {importExportDataset.LerrynImportExportConfig_DEV000221.TableName, 
                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.Enum.ClearType.Specific);
            General.m_ImportExportConfigFacade = importExportConfigFacade;
            var setts = new Settings();
            var configRows = importExportDataset.LerrynImportExportConfig_DEV000221.Select("SourceCode_Dev000221 = 'AmazonOrder'");
            if (configRows.Length == 0) Assert.Fail("Amazon Connector is not activated.");
            var amazonConfigRow = (Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)configRows[0];
            if (importExportConfigFacade.get_IsConnectorActivated(ConfigConst.AMAZON_SELLER_CENTRAL_CONNECTOR_CODE))
            {
                setts.LoadXMLConfig(ref amazonConfigRow);
            }
            var amazon = (Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)setts.ActiveSources[1];
            var xmlConfig = ((Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)amazon).XMLConfig;
            errorNotification.SendSrcErrorEmail(xmlConfig, "SendSrcErrorEmail_SendWithoutXMLSourceNoException", "Testing SendSrcErrorEmail_SendWithoutXMLSourceNoException", string.Empty);
        }

        [TestMethod]
        public void SendSrcErrorEmail_SendWithXMLSourceNoException()
        {
            var errorNotification = new Lerryn.Facade.ImportExport.ErrorNotification();
            errorNotification.BaseProductName = "eShopCONNECT";
            var importExportDataset = new Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway();
            var importExportConfigFacade = new Lerryn.Facade.ImportExport.ImportExportConfigFacade(ref importExportDataset, ref errorNotification, "AX5", "eShopCONNECT");
            importExportConfigFacade.LoadDataSet(new string[][] {new string[] {importExportDataset.LerrynImportExportConfig_DEV000221.TableName, 
                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.Enum.ClearType.Specific);
            General.m_ImportExportConfigFacade = importExportConfigFacade;
            var setts = new Settings();
            var configRows = importExportDataset.LerrynImportExportConfig_DEV000221.Select("SourceCode_Dev000221 = 'AmazonOrder'");
            if (configRows.Length == 0) Assert.Fail("Amazon Connector is not activated.");
            var amazonConfigRow = (Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)configRows[0];
            if (importExportConfigFacade.get_IsConnectorActivated(ConfigConst.AMAZON_SELLER_CENTRAL_CONNECTOR_CODE))
            {
                setts.LoadXMLConfig(ref amazonConfigRow);
            }
            var amazon = (Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)setts.ActiveSources[1];
            var xmlConfig = ((Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)amazon).XMLConfig;
            var xmlResponse = "<ErrorResponse xmlns=\"http://mws.amazonservices.com/doc/2009-01-01/\"><Error><Type>Sender</Type><Code>InvalidClientTokenId</Code><Message>The AWS Access Key Id you provided does not exist in our records.</Message><Detail>com.amazonservices.mws.model.Error$Detail@17b6643</Detail></Error><RequestID>b7afc6c3-6f75-4707-bcf4-0475ad23162c</RequestID></ErrorResponse>";
            errorNotification.SendSrcErrorEmail(xmlConfig, "SendSrcErrorEmail_SendWithXMLSourceNoException", "Testing SendSrcErrorEmail_SendWithXMLSourceNoException", xmlResponse);
        }

        [TestMethod]
        public void SendPmntErrorEmail_SendNoException()
        {
            var errorNotification = new Lerryn.Facade.ImportExport.ErrorNotification();
            errorNotification.BaseProductName = "eShopCONNECT";
            var importExportDataset = new Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway();
            var importExportConfigFacade = new Lerryn.Facade.ImportExport.ImportExportConfigFacade(ref importExportDataset, ref errorNotification, "AX5", "eShopCONNECT");
            importExportConfigFacade.LoadDataSet(new string[][] {new string[] {importExportDataset.LerrynImportExportConfig_DEV000221.TableName, 
                "ReadLerrynImportExportConfig_DEV000221"}}, Interprise.Framework.Base.Shared.Enum.ClearType.Specific);
            General.m_ImportExportConfigFacade = importExportConfigFacade;
            var setts = new Settings();
            var configRows = importExportDataset.LerrynImportExportConfig_DEV000221.Select("SourceCode_Dev000221 = 'AmazonOrder'");
            if (configRows.Length == 0) Assert.Fail("Amazon Connector is not activated.");
            var amazonConfigRow = (Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row)configRows[0];
            if (importExportConfigFacade.get_IsConnectorActivated(ConfigConst.AMAZON_SELLER_CENTRAL_CONNECTOR_CODE))
            {
                setts.LoadXMLConfig(ref amazonConfigRow);
            }
            var amazon = (Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)setts.ActiveSources[1];
            var xmlConfig = ((Lerryn.Framework.ImportExport.SourceConfig.SourceSettings)amazon).XMLConfig;
            errorNotification.SendPmntErrorEmail(xmlConfig, "Testing SendPmntErrorEmail_SendNoException");
        }
    }
}
