using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Settings;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using ECA.Business.Search;
using ECA.WebJobs.Search.Core;

namespace ECA.WebJobs.Search.Index.All.Test
{
    [TestClass]
    public class SearchUnityContainerTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private SearchUnityContainer container;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            appSettings.Add(AppSettings.SEARCH_API_KEY, "api key");
            appSettings.Add(AppSettings.SEARCH_INDEX_NAME_KEY, "index name");
            appSettings.Add(AppSettings.SEARCH_SERVICE_NAME_KEY, "service name");

            connectionStrings.Add(new ConnectionStringSettings(AppSettings.ECA_CONTEXT_KEY, "eca"));
            connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_DASHBOARD_KEY, "dashboard"));
            connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_STORAGE_KEY, "storage"));

            container = new SearchUnityContainer(settings);
        }

        [TestMethod]
        public void TestGetAzureSearchApiKey()
        {
            Assert.AreEqual(appSettings[AppSettings.SEARCH_API_KEY], container.GetAzureSearchApiKey(settings));
        }

        [TestMethod]
        public void TestGetSeachServiceName()
        {
            Assert.AreEqual(appSettings[AppSettings.SEARCH_SERVICE_NAME_KEY], container.GetSeachServiceName(settings));
        }

        [TestMethod]
        public void TestGetIndexName()
        {
            Assert.AreEqual(appSettings[AppSettings.SEARCH_INDEX_NAME_KEY], container.GetIndexName(settings));
        }

        [TestMethod]
        public void TestGetConnectionString()
        {
            Assert.AreEqual(connectionStrings[AppSettings.ECA_CONTEXT_KEY].ConnectionString, container.GetConnectionString(settings));
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsTrue(container.IsRegistered<IList<IDocumentConfiguration>>());
            Assert.IsTrue(container.IsRegistered<IIndexService>());
            Assert.IsTrue(container.IsRegistered<IIndexNotificationService>());
        }
    }
}
