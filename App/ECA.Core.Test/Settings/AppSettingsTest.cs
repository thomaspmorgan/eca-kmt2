using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;
using ECA.Core.Settings;
using System.Reflection;

namespace ECA.Core.Test.Settings
{
    [TestClass]
    public class AppSettingsTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
        }

        [TestMethod]
        public void TestAzureWebJobsDashboardConnectionString()
        {
            var value = "connection string";
            connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_DASHBOARD_KEY, value));
            Assert.AreEqual(value, settings.AzureWebJobsDashboardConnectionString.ConnectionString);
        }

        [TestMethod]
        public void TestAzureWebJobsStorageConnectionString()
        {
            var value = "connection string";
            connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_STORAGE_KEY, value));
            Assert.AreEqual(value, settings.AzureWebJobsStorageConnectionString.ConnectionString);
        }

        [TestMethod]
        public void TestCamContextConnectionString()
        {
            var value = "connection string";
            connectionStrings.Add(new ConnectionStringSettings(AppSettings.CAM_CONTEXT_KEY, value));
            Assert.AreEqual(value, settings.CamContextConnectionString.ConnectionString);
        }

        [TestMethod]
        public void TestCamContextKeyIsEqualEcaContextKey()
        {
            Assert.AreEqual(AppSettings.ECA_CONTEXT_KEY, AppSettings.CAM_CONTEXT_KEY);
        }

        [TestMethod]
        public void TestEcaContextConnectionString()
        {
            var value = "connection string";
            connectionStrings.Add(new ConnectionStringSettings(AppSettings.ECA_CONTEXT_KEY, value));
            Assert.AreEqual(value, settings.EcaContextConnectionString.ConnectionString);
        }

        [TestMethod]
        public void TestEcaContextConnectionString_NoSettingsExist()
        {
            var message = String.Format("Cannot find the connection string with key {0}.", AppSettings.ECA_CONTEXT_KEY);
            Action a = () =>
            {
                var value = settings.EcaContextConnectionString;
            };
            a.ShouldThrow<ConfigurationErrorsException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAppSettings_NoSettingsExist()
        {
            var message = String.Format("Cannot find the application setting with key {0}.", AppSettings.SEARCH_SERVICE_NAME_KEY);
            Action a = () =>
            {
                var value = settings.SearchServiceName;
            };
            a.ShouldThrow<ConfigurationErrorsException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAppSettings_SearchIndexName()
        {
            var value = "value";
            appSettings.Add(AppSettings.SEARCH_INDEX_NAME_KEY, value);
            Assert.AreEqual(value, settings.SearchIndexName);
        }

        [TestMethod]
        public void TestAppSettings_SearchServiceName()
        {
            var value = "value";
            appSettings.Add(AppSettings.SEARCH_SERVICE_NAME_KEY, value);
            Assert.AreEqual(value, settings.SearchServiceName);
        }

        [TestMethod]
        public void TestAppSettings_SearchApiKey()
        {
            var value = "value";
            appSettings.Add(AppSettings.SEARCH_API_KEY, value);
            Assert.AreEqual(value, settings.SearchApiKey);
        }

        [TestMethod]
        public void TestAppSettings_SearchDocumentQueueName()
        {
            var value = "value";
            appSettings.Add(AppSettings.SEARCH_INDEX_QUEUE_NAME_KEY, value);
            Assert.AreEqual(value, settings.SearchDocumentQueueName);
        }

        [TestMethod]
        public void TestAppSettings_ApplicationInsightsInstrumentationKey()
        {
            var value = "value";
            appSettings.Add(AppSettings.APPLICATION_INSIGHTS_INSTRUMENTATION_SETTINGS_KEY, value);
            Assert.AreEqual(value, settings.AppInsightsInstrumentationKey);
        }

        [TestMethod]
        public void TestAppSettings_AdTenantId()
        {
            var value = "value";
            appSettings.Add(AppSettings.AD_TENANT_ID, value);
            Assert.AreEqual(value, settings.AdTenantId);
        }

        [TestMethod]
        public void TestAppSettings_AdClientId()
        {
            var value = "value";
            appSettings.Add(AppSettings.AD_CLIENT_ID, value);
            Assert.AreEqual(value, settings.AdClientId);
        }

        [TestMethod]
        public void TestConstructor_ZeroArgument_AppSettings()
        {
            var testSettings = new AppSettings();
            var appSettingsField = typeof(AppSettings).GetField("appSettings", BindingFlags.Instance | BindingFlags.NonPublic);
            var appSettingsValue = appSettingsField.GetValue(testSettings);
            Assert.IsNotNull(appSettingsField);
            Assert.IsNotNull(appSettingsValue);
            Assert.IsTrue(Object.ReferenceEquals(appSettingsValue, ConfigurationManager.AppSettings));
        }

        [TestMethod]
        public void TestConstructor_ZeroArgument_ConnectionStrings()
        {
            var testSettings = new AppSettings();
            var connectionStringsField = typeof(AppSettings).GetField("connectionStrings", BindingFlags.Instance | BindingFlags.NonPublic);
            var connectionStringsValue = connectionStringsField.GetValue(testSettings);
            Assert.IsNotNull(connectionStringsField);
            Assert.IsNotNull(connectionStringsValue);
            Assert.IsTrue(Object.ReferenceEquals(connectionStringsValue, ConfigurationManager.ConnectionStrings));
        }
    }
}
