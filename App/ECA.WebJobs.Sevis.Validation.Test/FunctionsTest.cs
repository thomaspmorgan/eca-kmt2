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
using ECA.WebJobs.Sevis.Validation;
using ECA.Business.Service.Persons;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using System.Collections.Generic;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;

namespace ECA.WebJobs.Sevis.Validation.Test
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
        private Mock<IParticipantPersonsSevisService> participantPersonsSevisService;
        private Mock<IExchangeVisitorValidationService> exchangeVisitorValidationService;
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
            participantPersonsSevisService = new Mock<IParticipantPersonsSevisService>();
            exchangeVisitorValidationService = new Mock<IExchangeVisitorValidationService>();
            instance = new Functions(participantPersonsSevisService.Object, exchangeVisitorValidationService.Object, settings);
        }

        [TestMethod]
        public async Task TestProcessTimer_HasStartedParticipants()
        {
            var userId = 1;
            appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
            appSettings.Add(AppSettings.SEVIS_MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");

            var participant = new ReadyToValidateParticipantDTO
            {
                ParticipantId = 1,
                ProjectId = 2,
                SevisId = "sevisId"
            };
            var list = new List<ReadyToValidateParticipantDTO>();
            list.Add(participant);
            var results = new PagedQueryResults<ReadyToValidateParticipantDTO>(list.Count, list);

            participantPersonsSevisService.SetupSequence(x => x.GetReadyToValidateParticipantsAsync(It.IsAny<QueryableOperator<ReadyToValidateParticipantDTO>>()))
                .Returns(Task.FromResult<PagedQueryResults<ReadyToValidateParticipantDTO>>(results))
                .Returns(Task.FromResult<PagedQueryResults<ReadyToValidateParticipantDTO>>(new PagedQueryResults<ReadyToValidateParticipantDTO>(0, new List<ReadyToValidateParticipantDTO>())));

            var timerInfo = new TimerInfo(new TestTimerSchedule());
            await instance.ProcessTimer(timerInfo);
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            exchangeVisitorValidationService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestProcessTimer_DoesNotHaveParticipants()
        {
            var userId = 1;
            appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
            appSettings.Add(AppSettings.SEVIS_MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            
            participantPersonsSevisService.Setup(x => x.GetReadyToValidateParticipantsAsync(It.IsAny<QueryableOperator<ReadyToValidateParticipantDTO>>()))
                .Returns(Task.FromResult<PagedQueryResults<ReadyToValidateParticipantDTO>>(new PagedQueryResults<ReadyToValidateParticipantDTO>(0, new List<ReadyToValidateParticipantDTO>())));

            var timerInfo = new TimerInfo(new TestTimerSchedule());
            await instance.ProcessTimer(timerInfo);
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            exchangeVisitorValidationService.Verify(x => x.SaveChangesAsync(), Times.Never());
        }

        #region Dispose
        [TestMethod]
        public void TestDispose_ParticipantPersonsSevisService()
        {
            var disposableService = new Mock<IParticipantPersonsSevisService>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(disposableService.Object, exchangeVisitorValidationService.Object, settings);

            var serviceField = typeof(Functions).GetField("participantPersonsSevisService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ExchangeVisitorValidationService()
        {
            var disposableService = new Mock<IExchangeVisitorValidationService>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(participantPersonsSevisService.Object, disposableService.Object, settings);

            var serviceField = typeof(Functions).GetField("exchangeVisitorValidationService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ParticipantPersonsSevisService_IsNotIDisposable()
        {
            instance = new Functions(participantPersonsSevisService.Object, exchangeVisitorValidationService.Object, settings);

            var serviceField = typeof(Functions).GetField("participantPersonsSevisService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceValue);
        }

        [TestMethod]
        public void TestDispose_ExchangeVisitorValidationService_IsNotIDisposable()
        {
            instance = new Functions(participantPersonsSevisService.Object, exchangeVisitorValidationService.Object, settings);

            var serviceField = typeof(Functions).GetField("exchangeVisitorValidationService", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceValue);
        }
        #endregion
    }
}
