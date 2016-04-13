using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Service;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using ECA.WebJobs.Sevis.Comm;
using ECA.WebJobs.Sevis.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.IO.Compression;
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
        private Mock<ISevisApiResponseHandler> responseHandler;
        private Functions instance;
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private SevisBatchZipArchiveHandler fileProvider;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            service = new Mock<ISevisBatchProcessingService>();
            responseHandler = new Mock<ISevisApiResponseHandler>();
            instance = new Functions(service.Object, responseHandler.Object, settings);
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
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_THUMBPRINT, "f14e780d72921fda4b8079d887114dfd1922d624");
            var timerInfo = new TimerInfo(new TestTimerSchedule());
            service.Setup(x => x.GetBatchesToUploadAsync()).ReturnsAsync(new List<SevisBatchProcessingDTO>());

            await instance.ProcessTimer(timerInfo);   
        }

        [TestMethod]
        public void TestDispose_Service()
        {
            var disposableService = new Mock<ISevisBatchProcessingService>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(disposableService.Object, responseHandler.Object, settings);
            
            var serviceField = typeof(Functions).GetField("service", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_Service_NotDisposable()
        {
            var disposableService = new Mock<ISevisBatchProcessingService>();
            instance = new Functions(disposableService.Object, responseHandler.Object, settings);

            var serviceField = typeof(Functions).GetField("service", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceValue);
        }

        [TestMethod]
        public void TestDispose_ResponseHandler()
        {
            var disposableService = new Mock<ISevisApiResponseHandler>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(service.Object, disposableService.Object, settings);

            var responseHandlerField = typeof(Functions).GetField("responseHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNotNull(responseHandlerField);
            Assert.IsNotNull(handlerValue);

            instance.Dispose();
            handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNull(handlerValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ResponseHandler_NotDisposable()
        {
            var disposableService = new Mock<ISevisApiResponseHandler>();
            instance = new Functions(service.Object, disposableService.Object, settings);

            var responseHandlerField = typeof(Functions).GetField("responseHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNotNull(responseHandlerField);
            Assert.IsNotNull(handlerValue);

            instance.Dispose();
            handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNotNull(handlerValue);
        }
    }
}
