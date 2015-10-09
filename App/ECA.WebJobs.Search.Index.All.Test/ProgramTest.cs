//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ECA.Core.Settings;
//using System.Collections.Specialized;
//using System.Configuration;
//using Microsoft.Practices.Unity;
//using System.Collections.Generic;
//using ECA.Business.Search;

//namespace ECA.WebJobs.Search.Index.All.Test
//{
//    [TestClass]
//    public class ProgramTest
//    {
//        private NameValueCollection appSettings;
//        private ConnectionStringSettingsCollection connectionStrings;
//        private AppSettings settings;

//        [TestInitialize]
//        public void TestInit()
//        {
//            appSettings = new NameValueCollection();
//            connectionStrings = new ConnectionStringSettingsCollection();
//            settings = new AppSettings(appSettings, connectionStrings);
//        }

//        [TestMethod]
//        public void TestGetAzureSearchApiKey()
//        {
//            var value = "value";
//            appSettings.Add(AppSettings.SEARCH_API_KEY, value);
//            Assert.AreEqual(value, Program.GetAzureSearchApiKey(settings));
//        }

//        [TestMethod]
//        public void TestGetSeachServiceName()
//        {
//            var value = "value";
//            appSettings.Add(AppSettings.SEARCH_SERVICE_NAME_KEY, value);
//            Assert.AreEqual(value, Program.GetSeachServiceName(settings));
//        }

//        [TestMethod]
//        public void TestGetIndexName()
//        {
//            var value = "value";
//            appSettings.Add(AppSettings.SEARCH_INDEX_NAME_KEY, value);
//            Assert.AreEqual(value, Program.GetIndexName(settings));
//        }

//        [TestMethod]
//        public void TestGetConnectionString()
//        {
//            var value = "connection string";
//            connectionStrings.Add(new ConnectionStringSettings(AppSettings.ECA_CONTEXT_KEY, value));
//            Assert.AreEqual(value, Program.GetConnectionString(settings));
//        }

//        [TestMethod]
//        public void TestGetUnityContainer()
//        {
//            var unityContainer = new UnityContainer();
//            var instance = Program.GetUnityContainer(unityContainer);
//            Assert.IsNotNull(Program.GetUnityContainer(unityContainer));
//            Assert.IsTrue(instance.IsRegistered<IList<IDocumentConfiguration>>());
//            Assert.IsTrue(instance.IsRegistered<IIndexService>());
//            Assert.IsTrue(instance.IsRegistered<IIndexNotificationService>());
//        }
//    }
//}
