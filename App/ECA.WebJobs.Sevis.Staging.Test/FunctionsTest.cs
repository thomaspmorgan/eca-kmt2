using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Settings;
using Moq;
using ECA.Business.Service.Sevis;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using ECA.Business.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using System.Reflection;

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
        public async Task TestProcessTimer()
        {
            var userId = 1;
            appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
            appSettings.Add(AppSettings.SEVIS_MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            var timerInfo = new TimerInfo(new TestTimerSchedule());
            await instance.ProcessTimer(timerInfo);
            service.Verify(x => x.StageBatchesAsync(), Times.Once());
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
