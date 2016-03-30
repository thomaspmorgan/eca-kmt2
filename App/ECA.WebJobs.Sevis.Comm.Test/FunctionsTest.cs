﻿using ECA.Business.Service;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using ECA.WebJobs.Sevis.Comm;
using ECA.WebJobs.Sevis.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Staging.Test
{
    public class TestTimerSchedule : TimerSchedule
    {
        public TestTimerSchedule()
        {

        }

        public override DateTime GetNextOccurrence(DateTime now)
        {
            return DateTime.UtcNow.AddDays(1.0);
        }
    }

    [TestClass]
    public class FunctionsTest
    {
        private Mock<ISevisBatchProcessingService> service;
        private Functions instance;
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private ZipArchiveDS2019FileProvider fileProvider;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            service = new Mock<ISevisBatchProcessingService>();
            instance = new Functions(service.Object, settings);
        }

        [TestMethod]
        public void TestGetSystemUser()
        {
            var userId = 1;
            appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
            var user = instance.GetSystemUser();
            Assert.AreEqual(userId, user.Id);
        }

        [TestMethod]
        public async Task TestProcessTimer()
        {
            var userId = 1;
            appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
            appSettings.Add(AppSettings.SEVIS_MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_UPLOAD_URI_KEY, "https://egov.ice.gov/alphasevisbatch/action/batchUpload");
            appSettings.Add(AppSettings.SEVIS_DOWNLOAD_URI_KEY, "https://egov.ice.gov/alphasevisbatch/action/batchDownload");
            appSettings.Add(AppSettings.SEVIS_ORGID_KEY, "org id");
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_USERID_KEY, "10");
            appSettings.Add(AppSettings.SEVIS_THUMBPRINT, "f14e780d72921fda4b8079d887114dfd1922d624");
            appSettings.Add(AppSettings.SEVIS_PASSPHRASE, "none");
            var timerInfo = new TimerInfo(new TestTimerSchedule());
            await instance.ProcessTimer(timerInfo);
            
        }

        [TestMethod]
        public void TestGetFileProvider()
        {
            Assert.IsInstanceOfType(instance.GetFileProvider(), typeof(ZipArchiveDS2019FileProvider));
        }

        [TestMethod]
        public void TestDispose_Service()
        {
            var disposableService = new Mock<ISevisBatchProcessingService>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(disposableService.Object, settings);
            
            var serviceField = typeof(Functions).GetField("service", BindingFlags.NonPublic | BindingFlags.Instance);
            var contextValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(contextValue);

            instance.Dispose();
            contextValue = serviceField.GetValue(instance);
            Assert.IsNull(contextValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }
    }
}